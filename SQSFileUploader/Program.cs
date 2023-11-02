using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Configurations;

namespace SQSFileUploader
{
    class Program
    {
        static Task Main(string[] args)
        {



            // Define the folder to monitor
            var folderPath = "C:\\YourFolderToMonitor";

            // Create a file system watcher to monitor the folder
            var fileWatcher = new FileSystemWatcher(folderPath)
            {
                Filter = "*.json",
                EnableRaisingEvents = true
            };

            // Event handler for when a new JSON file is created
            fileWatcher.Created += async (sender, e) =>
            {
                try
                {
                    // Read the content of the JSON file
                    var content = File.ReadAllText(e.FullPath);

                    // Define the parameters for sending a message to the SQS queue
                    var request = new SendMessageRequest
                    {
                        QueueUrl = "YOUR_QUEUE_URL", // Replace with your actual SQS queue URL
                        MessageBody = content
                    };

                    // Send the message to the SQS queue
                    var response = await sqsClient.SendMessageAsync(request);
                    Console.WriteLine($"Message sent to SQS with MessageId: {response.MessageId}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            };

            Console.WriteLine("Monitoring folder for new JSON files. Press Enter to exit.");
            Console.ReadLine(); 
            return Task.CompletedTask;
        }
    }

}
