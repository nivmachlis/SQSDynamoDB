using Amazon.SQS;
using Microsoft.Extensions.Configuration;

namespace Configurations
{
    public class SQSConfigManager : AWSConfigManagerBase
    {
        public string Qurl { get; private set; }
        public AmazonSQSClient SqsClient { get; private set; }

        public SQSConfigManager()
        {
            Qurl = configuration.GetSection("AWS:Qurl").Value;
            SqsClient = new AmazonSQSClient(Credentials, Region);
        }
    }
}
