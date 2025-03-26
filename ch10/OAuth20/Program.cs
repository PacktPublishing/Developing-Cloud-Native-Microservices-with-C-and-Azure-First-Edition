using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

class Program
{
    // Application (client) ID of the app registration in Azure AD
    private static readonly string clientId = "[your-clientId]";

    // Client secret of the app registration in Azure AD
    private static readonly string clientSecret = "[your-clientSecret]";

    // Directory (tenant) ID of the Azure AD
    private static readonly string tenantId = "[your-tenantId]";

    // Redirect URI configured in the app registration
    private static readonly string redirectUri = "http://localhost";
    // Authority URL for Azure AD
    private static readonly string authority = $"https://login.microsoftonline.com/{tenantId}";

    // Scopes required for the Microsoft Graph API
    private static readonly string[] scopes = { "https://graph.microsoft.com/.default" };

    static async Task Main(string[] args)
    {
        try
        {
            // Get the access token
            string accessToken = await GetAccessTokenForAnAppAsync();
            Console.WriteLine("Access Token: " + accessToken);

            // Get the user profile
            await GetUserProfile();
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during the process
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    // Method to get the access token using client credentials
    private static async Task<string> GetAccessTokenForAnAppAsync()
    {
        var app = ConfidentialClientApplicationBuilder.Create(clientId)
            .WithClientSecret(clientSecret)
            .WithAuthority(new Uri(authority))
            .Build();

        var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        return authResult.AccessToken;
    }

    // Method to get the user profile using interactive authentication
    private static async Task GetUserProfile()
    {
        IPublicClientApplication clientApp = PublicClientApplicationBuilder.Create(clientId)
                     .WithRedirectUri(redirectUri)
                     .WithAuthority(AzureCloudInstance.AzurePublic, "common")
                     .Build();

        var resultadoAzureAd = await clientApp.AcquireTokenInteractive(scopes)
                    .WithPrompt(Prompt.SelectAccount)
                    .ExecuteAsync();
        if (resultadoAzureAd != null)
        {
            // Print the username of the authenticated user
            Console.WriteLine("User: " + resultadoAzureAd.Account.Username);
        }
    }
}
