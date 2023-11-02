using Amazon.SQS.Model;
using Configurations;
using DataBase;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SQSReaderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly SQSConfigManager _sqsConfigManager;
        private readonly DynamoDBWriterService _dynamoDBWriterService;

        public Worker(ILogger<Worker> logger, SQSConfigManager qsConfigManager, DynamoDBWriterService dynamoDBWriterService) 
        {
            _logger = logger;
            _sqsConfigManager = qsConfigManager;
            _dynamoDBWriterService = dynamoDBWriterService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var receiveMessageRequest = new ReceiveMessageRequest
                    {
                        QueueUrl = _sqsConfigManager.Qurl,
                        WaitTimeSeconds = 20, 
                        MaxNumberOfMessages = 10
                    };

                    var receiveMessageResponse = await _sqsConfigManager.SqsClient.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

                    _logger.LogInformation($"SQSReaderService: recvied {receiveMessageResponse.Messages.Count} from SQS");

                    if (receiveMessageResponse.Messages.Count > 0)
                    {
                        foreach (var message in receiveMessageResponse.Messages)
                        {
                            try
                            {
                                var messageContent = message.Body;

                                // Log or further process the message content
                                _logger.LogInformation($"SQSReaderService: Received message with contet {messageContent}");

                                // Save the message content to DynamoDB
                                await _dynamoDBWriterService.WriteToDynamoDB(messageContent);

                                // Delete the message from the SQS queue
                                var deleteRequest = new DeleteMessageRequest
                                {
                                    QueueUrl = _sqsConfigManager.Qurl,
                                    ReceiptHandle = message.ReceiptHandle
                                };
                                var response = await _sqsConfigManager.SqsClient.DeleteMessageAsync(deleteRequest, stoppingToken);
                                _logger.LogInformation($"SQSReaderService: {messageContent} deleted from SQS");

                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"SQSReaderService: Error processing message - {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error receiving messages: {ex.Message}");
                }
            }
        }
    }
}
