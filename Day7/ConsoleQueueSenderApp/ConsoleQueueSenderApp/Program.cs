﻿//sender connection string:
using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://day7sbns.servicebus.windows.net/;SharedAccessKeyName=policySender;SharedAccessKey=hQwfR25oufbsujKr+uYNuYxh6lrejenhG+ASbApUIqU=;EntityPath=stocks";
const string queueName = "stocks";
ServiceBusClient? client = default;
ServiceBusSender? sender = default;
const int numOfMessages = 3;
client = new ServiceBusClient(connectionString);
sender = client.CreateSender(queueName);

using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= numOfMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}
try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}