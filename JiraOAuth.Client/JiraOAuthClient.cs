using System;
using System.IO;
using System.Net;
using System.Web;
using OAuth;

namespace JiraOAuthClient
{
    public class JiraClient
    {
        private readonly string realm_;
        private readonly string baseUrl_;
        private readonly string consumerKey_;
        private readonly string consumerSecret_;


        public JiraClient(string baseUrl, string consumerKey, string consumerSecret)
        {
            baseUrl_ = baseUrl;
            realm_ = baseUrl_;
            consumerKey_ = consumerKey;
            consumerSecret_ = consumerSecret;
        }
        

        public string RequestTokenUrl => $"{baseUrl_}/plugins/servlet/oauth/request-token";
        public string AccessTokenUrl => $"{baseUrl_}/plugins/servlet/oauth/access-token";
        public string AuthorizationUrl => $"{baseUrl_}/plugins/servlet/oauth/authorize";
        
        
        public JiraRequestToken RequestToken { get; private set; }
        public JiraAccessToken AccessToken { get; private set; }
        
        
        public Uri GetAuthorizationUrl() => new Uri($"{AuthorizationUrl}?{RequestToken}");
        

        public void GetRequestToken()
        {
            try {
                
                var client = new OAuthRequest
                {
                    Method = "POST",
                    Type = OAuthRequestType.RequestToken,
                    SignatureMethod = OAuthSignatureMethod.RsaSha1,
                    ConsumerKey = consumerKey_,
                    ConsumerSecret = consumerSecret_,
                    RequestUrl = RequestTokenUrl,
                    Realm = realm_, 
                    CallbackUrl = "oob" // 'Out Of Band' which indicates we need a verification code from the service
                }; 
                
                var auth = client.GetAuthorizationQuery();
                var request = (HttpWebRequest) WebRequest.Create($"{client.RequestUrl}?{auth}");
                request.Method = "POST"; // POST is required by JIRA when retrieving tokens
                
                var response = (HttpWebResponse) request.GetResponse();
                
                using (var stream = response.GetResponseStream()) {
                    if (stream != null) {
                        RequestToken = new JiraRequestToken(new StreamReader(stream).ReadToEnd());
                    } else {
                        throw new NullReferenceException("Response stream should not be null");
                    }
                }
            } catch (Exception err) {
                throw new Exception("Failed to get Request token", err);
            }
        }

        
        public void GetAccessToken(string verificationCode)
        {
            try {
                if (RequestToken == null)
                    throw new InvalidOperationException("You must get a Request token before attempting to get an Access token");
                
                var client = new OAuthRequest
                {
                    Method = "POST",
                    Type = OAuthRequestType.AccessToken,
                    SignatureMethod = OAuthSignatureMethod.RsaSha1,
                    ConsumerKey = consumerKey_,
                    ConsumerSecret = consumerSecret_,
                    RequestUrl = AccessTokenUrl,
                    Token = RequestToken.Token,
                    TokenSecret = RequestToken.TokenSecret,
                    Verifier = verificationCode,
                    Realm = realm_
                };

                var auth = client.GetAuthorizationQuery();
                var request = (HttpWebRequest) WebRequest.Create($"{client.RequestUrl}?{auth}");
                request.Method = "POST"; // POST is required by JIRA when retrieving tokens

                var response = (HttpWebResponse) request.GetResponse();

                using (var stream = response.GetResponseStream()) {
                    if (stream != null) {
                        AccessToken = new JiraAccessToken(new StreamReader(stream).ReadToEnd());
                    }
                    else {
                        throw new NullReferenceException("Response stream should not be null");
                    }
                }
            } catch (Exception err) {
                throw new Exception("Failed to get Access token", err);
            }
        }
        
        
        public class JiraAccessToken
        {
            public JiraAccessToken(string accessTokenString)
            {
                var query = HttpUtility.ParseQueryString(accessTokenString);
                Token = query.Get("oauth_token");
                TokenSecret = query.Get("oauth_token_secret");
                TokenExpiresIn = long.Parse(query.Get("oauth_expires_in"));
                SessionHandle = query.Get("oauth_session_handle"); 
                SessionExpiresIn = long.Parse(query.Get("oauth_authorization_expires_in"));
                LeaseDate = DateTime.UtcNow.Date;
            }
            
            public string Token { get; }
            public string TokenSecret { get; }
            public long TokenExpiresIn { get; }
            public string SessionHandle { get; }
            public long SessionExpiresIn { get; }
            public DateTime LeaseDate { get; }


            public override string ToString()
            {
                return $"oauth_token={Token}" +
                       $"&oauth_token_secret={TokenSecret}" +
                       $"&oauth_expires_in={TokenExpiresIn}" +
                       $"&oauth_session_handle={SessionHandle}" +
                       $"&oauth_authorization_expires_in={SessionExpiresIn}";
            }
        }
        
        
        public class JiraRequestToken
        {
            public JiraRequestToken(string requestTokenString)
            {
                var query = HttpUtility.ParseQueryString(requestTokenString);
                Token = query.Get("oauth_token");
                TokenSecret = query.Get("oath_token_secret");
                IsCallbackConfirmed = Boolean.Parse(query.Get("oauth_callback_confirmed"));
            }
            
            public string Token { get; }
            public string TokenSecret { get; }
            public bool IsCallbackConfirmed { get; }


            public override string ToString()
            {
                return $"oauth_token={Token}" +
                       $"&oauth_token_secret={TokenSecret}" +
                       $"&oauth_callback_confirmed={IsCallbackConfirmed}";
            }
        }
    }
}

