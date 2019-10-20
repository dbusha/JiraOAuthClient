using JiraOAuth.Client;
using Xunit;

namespace JiraOAuth.Tests
{
    public class JiraRequestTokenTests
    {
        private const string Token = "test_token";
        private const string Secret = "super_top_secret";
        private static readonly string RequestString = $"oauth_token={Token}&oauth_token_secret={Secret}";

        [Fact]
        public void ConstructorTest()
        {
            var requestToken = new JiraRequestToken(RequestString);
            Assert.NotNull(requestToken);
            Assert.Equal(Token, requestToken.Token);
            Assert.Equal(Secret, requestToken.TokenSecret);
        }
        
        
        
        [Fact]
        public void ToStringTest()
        {
            var requestToken = new JiraRequestToken(RequestString);
            Assert.Equal(RequestString, requestToken.ToString());
        }
    }
}