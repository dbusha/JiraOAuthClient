using System;
using System.ComponentModel;
using System.Web;

namespace JiraOAuth.Client
{
    public class JiraAccessToken : TokenBase
    {
        public JiraAccessToken(string accessTokenString) 
        {
            var query = HttpUtility.ParseQueryString(accessTokenString);
            Token = query.Get(GetDescription(nameof(Token)));
            TokenSecret = query.Get(GetDescription(nameof(TokenSecret)));
            TokenExpiresIn = long.Parse(query.Get(GetDescription(nameof(TokenExpiresIn))));
            SessionHandle = query.Get(GetDescription(nameof(SessionHandle)));
            SessionExpiresIn = long.Parse(query.Get(GetDescription(nameof(SessionExpiresIn))));
            LeaseDate = DateTime.UtcNow.Date;
        }

        
        public JiraAccessToken(string accessToken, string accessTokenSecret, string session)
        {
            Token = accessToken;
            TokenSecret = accessTokenSecret;
            SessionHandle = session;
        }
        

        [Description("oauth_expires_in")]
        public long TokenExpiresIn { get; }
        
        
        [Description("oauth_session_handle")]
        public string SessionHandle { get; }
        
        
        [Description("oauth_authorization_expires_in")]
        public long SessionExpiresIn { get; }
        
        
        public DateTime LeaseDate { get; }


        public override string ToString()
        {
            if (string.IsNullOrEmpty(Token))
                return "";
            
            return $"{GetDescription(nameof(Token))}={Token}" +
                   $"&{GetDescription(nameof(TokenSecret))}={TokenSecret}" +
                   $"&{GetDescription(nameof(TokenExpiresIn))}={TokenExpiresIn}" +
                   $"&{GetDescription(nameof(SessionHandle))}={SessionHandle}" +
                   $"&{GetDescription(nameof(SessionExpiresIn))}={SessionExpiresIn}";
        }
    }
}