using System.ComponentModel;
using System.Web;

namespace JiraOAuth.Client
{
    public class JiraRequestToken : TokenBase
    {
        public JiraRequestToken(string requestTokenString)
        {
            var query = HttpUtility.ParseQueryString(requestTokenString);
            Token = query.Get(GetDescription(nameof(Token)));
            TokenSecret = query.Get(GetDescription(nameof(TokenSecret)));
        }
            

        public override string ToString()
        { 
            return Token == null ? "" 
                : $"{GetDescription(nameof(Token))}={Token}" +
                  $"&{GetDescription(nameof(TokenSecret))}={TokenSecret}"; 
        }
    }
}