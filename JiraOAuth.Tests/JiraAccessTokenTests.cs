using JiraOAuth.Client;
using Xunit;

namespace JiraOAuth.Tests
{
    public class JiraAccessTokenTests
    {
        private const string Token = "test_token";
        private const string Secret = "super_top_secret";
        private const long TokenExpiresIn = 1234;
        private const string Session = "session";
        private const long SessionExpiresIn = 1234;
        private static readonly string RequestString = $"oauth_token={Token}" +
                                                       $"&oauth_token_secret={Secret}" +
                                                       $"&oauth_expires_in={TokenExpiresIn}" +
                                                       $"&oauth_session_handle={Session}" +
                                                       $"&oauth_authorization_expires_in={SessionExpiresIn}";

        [Fact]
        public void ConstructorTest()
        {
            var accessToken = new JiraAccessToken(RequestString);
            Assert.NotNull(accessToken);
            Assert.Equal(Token, accessToken.Token);
            Assert.Equal(Secret, accessToken.TokenSecret);
            Assert.Equal(Session, accessToken.SessionHandle);
            Assert.Equal(TokenExpiresIn, accessToken.TokenExpiresIn);
            Assert.Equal(SessionExpiresIn, accessToken.SessionExpiresIn);
        }
        
        
        
        [Fact]
        public void ToStringTest()
        {
            var accessToken = new JiraAccessToken(RequestString);
            Assert.Equal(RequestString, accessToken.ToString());
        }
    }
}