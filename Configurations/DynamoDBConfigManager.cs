using Amazon.DynamoDBv2;

namespace Configurations
{
    public class DynamoDBConfigManager : AWSConfigManagerBase
    {
        public AmazonDynamoDBClient DynamoDBClient { get; private set; }

        public DynamoDBConfigManager()
        {
            DynamoDBClient = new AmazonDynamoDBClient(Credentials, Region);
        }
    }
}
