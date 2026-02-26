using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

class Program
{
    static async Task Main()
    {
        string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Hace falta la variable AZURE_STORAGE_CONNECTION_STRING");
            return;
        }

        string containerName = "archivos";
        string blobName = "mensaje.txt";

        var blobClient = new BlobClient(connectionString, containerName, blobName);
        var download = await blobClient.DownloadAsync();

        using var reader = new StreamReader(download.Value.Content);
        string contenido = await reader.ReadToEndAsync();
        Console.WriteLine("Contenido del blob:");
        Console.WriteLine(contenido);
    }
}
