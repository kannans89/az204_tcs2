
using Azure.Identity;
using Azure.Storage.Blobs;

string tenantId = "f0093ae3-bfa4-46e1-9b91-668278209d56";
string clientId = "bf7f6d65-fbb3-4358-b144-a6615eec099d";
string clientSecret = "1Dd8Q~oCVWGuq8JlZ22tm5W_1ZN6fiDpvnE9lcPV";


string blobURI = "https://day5store2024.blob.core.windows.net/images/burger.jpg";
string filePath = "C:\\temp\\new.jpg";
ClientSecretCredential clientCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

BlobClient blobClient = new BlobClient(new Uri(blobURI), clientCredential);

await blobClient.DownloadToAsync(filePath);

Console.WriteLine("The blob is downloaded");

