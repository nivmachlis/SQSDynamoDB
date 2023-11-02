using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataBase
{
    public class DynamoDBWriterService
    {
        private readonly AmazonDynamoDBClient _dynamoDBClient;
        private readonly string _tableName;
/*        private readonly ILogger<DynamoDBWriterService> _logger;
*/

        public DynamoDBWriterService(DynamoDBConfigManager dynamoDBConfigManager)
        {
            _dynamoDBClient = dynamoDBConfigManager.DynamoDBClient;
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "DataBase"))
            .AddJsonFile(path: "appsettings.json")
            .Build();
            _tableName = configuration.GetSection("AWS:DynamoDBTableName").Value;

/*            _logger.LogInformation($"DynamoDBWriterService: dynamoDB table name {_tableName}");
*/        }

        public async Task WriteToDynamoDB(string messageContent)
        {
            try
            {
                var table = Table.LoadTable(_dynamoDBClient, _tableName);

                var document = Document.FromJson(messageContent);
/*                _logger.LogInformation($"DynamoDBWriterService: document contet is {_tableName}");
*/
                await table.PutItemAsync(document);
/*                _logger.LogInformation($"DynamoDBWriterService: document {_tableName} saved to {_tableName}");
*/            }
            catch (Exception ex)
            {
/*                _logger.LogError($"DynamoDBWriterService: Error writing to DynamoDB - {ex.Message}");
*/            }
        }
    }
}
