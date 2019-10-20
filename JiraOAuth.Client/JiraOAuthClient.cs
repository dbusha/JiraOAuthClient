using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;


namespace JiraOAuth.Client
{
    public class JiraOAuthClient
    {
        private readonly IRestClient client_;
        private JiraRequestToken requestToken_;


        public JiraOAuthClient(string baseUrl, string consumerKey, string consumerSecret)
        {
            client_ = new RestClient();
            if (baseUrl != null)
                client_.BaseUrl = new Uri(baseUrl);
            Consumer = new ConsumerCredentials(consumerKey, consumerSecret);
        }
        

        public JiraAccessToken AccessToken { get; private set; }
        public ConsumerCredentials Consumer { get; }
        public string AuthorizationUrl => $"{client_.BaseUrl}plugins/servlet/oauth/authorize";


        public string GetAuthorizationUrlWithCredentials() => $"{AuthorizationUrl}?{requestToken_}";
        
        
        public void GetRequestToken()
        {
            client_.Authenticator = OAuth1Authenticator.ForRequestToken(
                Consumer.Key, 
                Consumer.Secret, 
                OAuthSignatureMethod.RsaSha1);
            
            var request = new RestRequest(new Uri($"{client_.BaseUrl}plugins/servlet/oauth/request-token"), Method.POST);
            var response = client_.Execute(request);
                               
            if (response.StatusCode != HttpStatusCode.OK)
                throw new WebException("Did not receive expected status code: " + response.StatusCode);
            
            var content = response.Content;
            if (content == null)
                throw new NullReferenceException();
        
            requestToken_ = new JiraRequestToken(content);
        }
        
        
        public void GetAccessToken()
        {
            if (requestToken_ == null)
                throw new InvalidOperationException("Request token must be acquired before requesting an access token");
                    
            client_.Authenticator = OAuth1Authenticator.ForAccessToken(
                Consumer.Key, 
                Consumer.Secret, 
                requestToken_.Token, 
                requestToken_.TokenSecret,
                OAuthSignatureMethod.RsaSha1);
            var request = new RestRequest(new Uri($"{client_.BaseUrl}plugins/servlet/oauth/access-token"), Method.POST);
            var response = client_.Execute(request);
                                
            var content = response.Content;
            if (content == null)
                throw new NullReferenceException();

            AccessToken = new JiraAccessToken(content);
        }

        
        public IAuthenticator GetRequestAuthenticator()
        {
            if (requestToken_ == null || AccessToken == null)
                throw new InvalidOperationException("You must acquire an access token before making a request");
            
            return OAuth1Authenticator.ForProtectedResource(
                Consumer.Key,
                Consumer.Secret,
                AccessToken.Token,
                AccessToken.TokenSecret,
                OAuthSignatureMethod.RsaSha1);

        }
    }
}

