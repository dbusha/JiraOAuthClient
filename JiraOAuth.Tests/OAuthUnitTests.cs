using System;
using Xunit;

namespace JiraOAuthClient.Tests
{
    public class OAuthTests
    {
        // ToDo: Load these from config
        private const string BaseUrl = "https://jira.domain.com";
        private const string ConsumerKey= "JiraOAuthKey";
        private const string ConsumerSecret = "-----BEGIN RSA PRIVATE KEY----- -----END RSA PRIVATE KEY-----";


        [Fact]
        public void GetRequestTokenTest()
        {
            JiraClient client = new JiraClient(BaseUrl, ConsumerKey, ConsumerSecret);
            client.GetRequestToken();
            Assert.NotNull(client.RequestToken);
        }
        
        
        [Theory]
        [InlineData("", "", "")]
        [InlineData(BaseUrl, "", "")]
        [InlineData("", ConsumerKey, "")]
        [InlineData("", "", ConsumerSecret)]
        [InlineData(BaseUrl, ConsumerKey, "")]
        [InlineData("", ConsumerKey, ConsumerSecret)]
        [InlineData(BaseUrl, "", ConsumerKey)]
        public void GetRequestTokenWithCredentialsTest(string baseUrl, string consumerKey, string consumerSecret)
        {
            JiraClient client = new JiraClient(baseUrl, consumerKey, consumerSecret);

            try { client.GetRequestToken(); } catch {
                Assert.Null(client.RequestToken);
                return;
            }
            
            // Invalid credentials should cause GetRequestToken to throw so we shouldn't get here
            throw new InvalidOperationException("Test should have already passed");
        }

        
        [Fact]
        public void GetAccessTokenWithoutRequestTokenTest()
        {
            JiraClient client = new JiraClient(BaseUrl, ConsumerKey, ConsumerSecret);
            Assert.Throws<Exception>(() => client.GetAccessToken("")); 
        }
    }
}