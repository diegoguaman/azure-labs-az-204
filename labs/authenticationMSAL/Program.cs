using Microsoft.Identity.Client;
using dotenv.net;

// Load environment variables from .env file
DotEnv.Load();
var envVars = DotEnv.Read();

// Verificar que las variables existen para evitar errores nulos
if (!envVars.ContainsKey("CLIENT_ID") || !envVars.ContainsKey("TENANT_ID"))
{
    Console.WriteLine("Error: No se encontraron CLIENT_ID o TENANT_ID en el archivo .env");
    return;
}

// Retrieve Azure AD Application ID and tenant ID from environment variables
string _clientId = envVars["CLIENT_ID"];
string _tenantId = envVars["TENANT_ID"];

// ADD CODE TO DEFINE SCOPES AND CREATE CLIENT
// Define the scopes required for authentication
string[] _scopes = { "User.Read" };

// Build the MSAL public client application with authority and redirect URI
var app = PublicClientApplicationBuilder.Create(_clientId)
    .WithAuthority(AzureCloudInstance.AzurePublic, _tenantId)
    .WithDefaultRedirectUri()
    .Build();

Console.WriteLine("Iniciando proceso de autenticación...");
// ADD CODE TO ACQUIRE AN ACCESS TOKEN
try 
{
    // 4. Adquirir token mediante Device Code Flow (Ideal para Codespaces/Consola)
    // Se usa _scopes (con guion bajo) para coincidir con la definición de arriba
    var result = await app.AcquireTokenWithDeviceCode(_scopes, deviceCodeResult =>
    {
        // Esto imprimirá la URL y el código que debes pegar en tu navegador
        Console.WriteLine("\n***************************************************");
        Console.WriteLine(deviceCodeResult.Message);
        Console.WriteLine("***************************************************\n");
        return Task.CompletedTask;
    }).ExecuteAsync();

    // 5. Mostrar resultado exitoso
    Console.WriteLine("¡Autenticación exitosa!");
    Console.WriteLine($"Token de acceso (primeros 20 caracteres): {result.AccessToken.Substring(0, 20)}...");
    Console.WriteLine($"Usuario: {result.Account.Username}");
}
catch (MsalException ex)
{
    Console.WriteLine($"Error de MSAL: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error inesperado: {ex.Message}");
}