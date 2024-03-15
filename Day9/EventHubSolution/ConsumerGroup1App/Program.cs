using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Azure.Amqp.Framing;
class Program
{


    private const string ehubNamespaceConnectionString = "Endpoint=sb://myneweventhubnsv2.servicebus.windows.net/;SharedAccessKeyName=readAndWrite;SharedAccessKey=9dtNKR6yy+4NyBEXVjozggVY4Ul7gyfai+AEhOJfb9g=;EntityPath=apphub";
    private const string eventHubName = "apphub";


    private const string blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=mysourceeventgrid2024;AccountKey=RQ90PoTJ+t8UY/LpOlQ1dqnZUWHwloy6d8EbUNYciFI0Rsj2nG0p8BR5c/eRUiKT3f5Ea5me7V8++AStBzZmfw==;EndpointSuffix=core.windows.net";
    private const string blobContainerName = "myeventhubcheckpoints";

    static async Task Main()
    {
        // Read from the default consumer group: $Default
        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
        // Create a blob container client that the event processor will use
        BlobContainerClient storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);
        // Create an event processor client to process events in the event hub
        EventProcessorClient processor = new EventProcessorClient(storageClient,
            consumerGroup, ehubNamespaceConnectionString, eventHubName);
        // Register handlers for processing events and handling errors
        processor.ProcessEventAsync += ProcessEventHandler;
        processor.ProcessErrorAsync += ProcessErrorHandler;
        // Start the processing
        await processor.StartProcessingAsync();
        // Wait for 10 seconds for the events to be processed
        //await Task.Delay(TimeSpan.FromSeconds(10));
        Console.WriteLine("Press Enter to stop...");
        Console.ReadLine();
        // Stop the processing
        await processor.StopProcessingAsync();
    }
    static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
    {
        long sequenceNumber = eventArgs.Data.SequenceNumber;
        string partitionId = eventArgs.Partition.PartitionId;
        Console.Write($"partion {partitionId} and seq {sequenceNumber}");
        // Write the body of the event to the console window
        Console.WriteLine("\tRecevied event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
        // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
        await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
    }
    static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
    {
        // Write details about the error to the console window
        Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': " +
            $"an unhandled exception was encountered. This was not expected to happen.");
        Console.WriteLine(eventArgs.Exception.Message);
        return Task.CompletedTask;
    }
}