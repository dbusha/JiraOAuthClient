using System;
using System.Diagnostics;
using JiraOAuth.Client;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;

namespace Examples
{
    class Program
    {
        private const string BaseUrl = "https://jira.domain.com";
        private const string ConsumerKey = "JiraOAuthKey";
        private const string ConsumerSecret = "-----BEGIN RSA PRIVATE KEY----- contents of your .pem file -----END RSA PRIVATE KEY-----";

        private static string accessToken;
        private static string accessTokenSecret;
        
        
        static void Main(string[] args)
        {
            var restClient = new RestClient();
            var oauthClient = new JiraOAuthClient(BaseUrl, ConsumerKey, ConsumerSecret);
           
            AuthorizeClient(oauthClient);

            SetAuthenticatorFromOAuthClient(restClient, oauthClient);
            //SetAuthenticatorFromTokens(restClient);
            
            MakeRequest(restClient);
            
            Console.WriteLine("Hit Enter to Exit");
            Console.ReadLine();
        }
        

        private static void AuthorizeClient(JiraOAuthClient oauthClient)
        {
           oauthClient.GetRequestToken();
           LaunchBrowser(oauthClient);
           
           Console.WriteLine("Hit Enter after you've accepted the access request in your browser");
           Console.ReadLine();
           
           oauthClient.GetAccessToken();

           // Save tokens for the example of manually generating Authenticator
           accessToken = oauthClient.AccessToken.Token;
           accessTokenSecret = oauthClient.AccessToken.TokenSecret; 
        }


        private static void LaunchBrowser(JiraOAuthClient oauthClient)
        {
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true, // Required by netcore https://github.com/dotnet/corefx/issues/10361
                FileName = oauthClient.GetAuthorizationUrlWithCredentials()
            };
            Process.Start(startInfo);
        }
        
        
        private static void SetAuthenticatorFromOAuthClient(RestClient restClient, JiraOAuthClient oAuthClient)
        {
            restClient.Authenticator = oAuthClient.GetRequestAuthenticator();
        }
        
        
        private static void SetAuthenticatorFromTokens(RestClient restClient)
        {
            // ConsumerCredentials converts the secret into the necessary RSA format
            var consumer = new ConsumerCredentials(ConsumerKey, ConsumerSecret);
            restClient.Authenticator = OAuth1Authenticator.ForProtectedResource(
                consumer.Key,
                consumer.Secret,
                accessToken,
                accessTokenSecret,
                OAuthSignatureMethod.RsaSha1);
        }
        
        
        private static void MakeRequest(RestClient restClient)
        {
            var request = new RestRequest($"{BaseUrl}/rest/api/2/project");
            var response = restClient.Execute(request);
            Console.WriteLine(response.Content);
        }
    }
}