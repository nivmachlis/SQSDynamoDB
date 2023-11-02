using Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace SQSUploader
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly SQSConfigManager _sqsConfigManager;
        private readonly string _folderPath;

        public Worker(ILogger<Worker> logger, SQSConfigManager sqsConfigManager, IConfiguration configuration)
        {
            _logger = logger;
            _sqsConfigManager = sqsConfigManager;
            _folderPath = configuration.GetSection("AppSettings")["FolderPath"];
            _logger.LogInformation($"SQSUploader: folder path the service listen to is {_folderPath}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Create a file system watcher to monitor the folder
            var fileWatcher = new FileSystemWatcher(_folderPath)
            {
                Filter = "*.json",
                EnableRaisingEvents = true
            };

            // Event handler for when a new JSON file is created
            fileWatcher.Created += async (sender, e) =>
            {
                try
                {
                    var content = File.ReadAllText(e.FullPath);
                    _logger.LogInformation($"SQSUploader: the file {e.Name} content is {content}");

                    var request = new SendMessageRequest
                    {
                        QueueUrl = _sqsConfigManager.Qurl,
                        MessageBody = content
                    };

                    var response = await _sqsConfigManager.SqsClient.SendMessageAsync(request);
                    _logger.LogInformation($"SQSUploader: Message sent to SQS with MessageId{response.MessageId}");
                    
                    File.Delete(e.FullPath);
                    _logger.LogInformation($"SQSUploader: file {e.Name} deleted from folder path {_folderPath}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SQSUploader: Error - {ex.Message}");
                }
            };
            await Task.Delay(1000, stoppingToken);
        }
    }
}
