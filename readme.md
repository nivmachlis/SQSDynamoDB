# AWS SQS Uploader and Reader Service

This project demonstrates how to use Amazon Simple Queue Service (SQS) for uploading and reading messages to and from SQS queues. It consists of two services: SQSUploader and SQSReaderService, as well as a DynamoDBWriterService.

## Prerequisites

Before you get started, make sure you have the following prerequisites in place:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- An Amazon Web Services (AWS) account
- [Visual Studio Code](https://code.visualstudio.com/) or Visual Studio (optional but recommended)
- AWS Access Key and Secret Key
- The URL of your SQS queue (SQS URL)
- A DynamoDB table configured in your AWS account

## Configuration

To configure and run the services, follow these steps:

1. **AWS Credentials**: Adjust your AWS credentials in the `Configurations\appsettings.json` file.

2. **SQSUploader Configuration**: Configure the folder path from which the SQSUploader service will read JSON files and upload them to SQS. Update the settings in the `SQSUploader\appsettings.json` file.

3. **DynamoDB Table Name**: Set your DynamoDB table name in the `DataBase\appsettings.json` file.

Make these adjustments to ensure that your services work correctly and are properly configured.
