using System;
using System.Net;
using JiraOAuth.Client;
using Xunit;


namespace JiraOAuth.Tests
{
    public class OAuthTests
    {
        // ToDo: Load these from config
        private const string BaseUrl = "https://jira.domain.com";
        private const string ConsumerKey= "JiraOAuthKey";
        private const string ConsumerSecret = "-----BEGIN RSA PRIVATE KEY----- your .pem file contents here -----END RSA PRIVATE KEY-----";

        private const string TestUrl = "http://jira.example.com";
        private const string TestKey = "TestKey";

        
        [Fact]
        public void GetRequestTokenTest()
        {
            var jiraOAuthClient = new JiraOAuthClient(BaseUrl, ConsumerKey, ConsumerSecret);
            jiraOAuthClient.GetRequestToken();

            var url = jiraOAuthClient.GetAuthorizationUrlWithCredentials();
            Assert.Contains("oauth_token", url);
            Assert.Contains("oauth_token_secret", url);
        }
        
        
        [Theory]
        [InlineData(TestUrl, TestKey, ConsumerSecret)]
        [InlineData(TestUrl, ConsumerKey, ConsumerSecret)]
        [InlineData(BaseUrl, TestKey, ConsumerSecret)]
        public void GetRequestTokenWithCredentialsTest(string baseUrl, string consumerKey, string consumerSecret)
        {
            var jiraOAuthClient = new JiraOAuthClient(baseUrl, consumerKey, consumerSecret);
            Assert.Throws<WebException>(() => jiraOAuthClient.GetRequestToken());
        }

        
        [Fact]
        public void GetAccessTokenWithoutRequestTokenTest()
        {
            var jiraOAuthClient = new JiraOAuthClient(BaseUrl, ConsumerKey, ConsumerSecret);
            Assert.Throws<InvalidOperationException>(() => jiraOAuthClient.GetAccessToken()); 
        }
    }
}