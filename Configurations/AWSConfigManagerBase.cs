using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Configurations
{
    public class AWSConfigManagerBase
    {
        public string AccessKey { get; private set; }
        public string SecretKey { get; private set; }
        public RegionEndpoint Region { get; private set; }
        public BasicAWSCredentials Credentials { get; private set; }

        public IConfiguration configuration;

        public AWSConfigManagerBase()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Configurations"))
                .AddJsonFile(path: "appsettings.json")
                .Build();

            AccessKey = configuration.GetSection("AWS:AccessKey").Value;
            SecretKey = configuration.GetSection("AWS:SecretKey").Value;
            Region = RegionEndpoint.GetBySystemName(configuration.GetSection("AWS:Region").Value);

            Credentials = new BasicAWSCredentials(AccessKey, SecretKey);


        }
    }
}
