using System;
using JiraOAuth.Client;
using Xunit;

namespace JiraOAuth.Tests
{
    public class ConsumerCredentialsTests
    {
        private const string ConsumerKey = "TestKey";
        private const string ConsumerSecret = "-----BEGIN RSA PRIVATE KEY-----MIICXgIBAAKBgQCvxN/cd1plcAHVH7JVbu62ZqS1Ljacbn/ZTPUlY3Fed0ffBsZqoZUfzHtrCOmCywGbqV0C86ujpreGoRVyWuSWzWtcc2jWfICozc+dcEymUMKGVu515Bx8vJMdNmyr+OsBb2qPjKNnAej/rjTLKV/GDPJ6AZdX0BSfOyX6H6iz4QIDAQABAoGBAIE6hjxZpCSgTTicrUkS3n9MyBxNdelddLIXWsW6b0e8+yKGoKsVUcanWLZBPy3ig7QmuTPKY49Wq+hX1qTVHupo/T/CUHIZf/rvdNQt37KN1jHAo7vuzQsUxqXelexLaC5QFEP77HVwmXOAmGif5NPzcnwmG/qjr2IH2GQ4bCLhAkEA1l+db7GdKurkHQHakdNY/bZ+g9JT2mDmECzuTWGVE7Pf7DbVmdHMLBnGPx/k/RADsQv8UbkNJ+pcTZLPhpMLnwJBANHmQP5GLto5NtDwFbypq0VQdrkcZ/xWwnww07ZGEb7Pikhn21r+OfwceW8Est6V9HgsdEpeZNdKfcy/xhKEEH8CQQC6WOlZM3Mk4ISktnzR8H9WJLI7UrTzivHSM8x+1YmqU90dz9jD2nx6BXmwW8BJPweGep2+SHcoMHBGOo1NNTuPAkEApHLzpMQz3QFKczRGs4NzDmFWQv2AcvE+erJ+jKYlLQvJWaUU9RuNLnrYoSRbR4zQ9n5Nth5yf0J7gTkW5FbX3QJAd1ykTRLJ2P9JMeUNABGkhVytLx6qfLGvOyARHfscg+z5kLUneJ+Kkbo5ho9Qx8rkBMkMJgoJZTIJZzM4N+QZEw==-----END RSA PRIVATE KEY-----";
            
        [Fact]
        public void ConsumerConstructorTest()
        {
            var consumer = new ConsumerCredentials(ConsumerKey, ConsumerSecret);
            Assert.Equal(ConsumerKey, consumer.Key);
            Assert.NotNull(consumer.Secret);
        }
        
        
        [Fact]
        public void InvalidSecretFormatTest()
        {
            var ctor = new Action(() => new ConsumerCredentials(ConsumerKey, "Consumer Secret with Invalid Format"));
            Assert.Throws<InvalidOperationException>(ctor);
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData(ConsumerKey, null)]
        [InlineData(null, ConsumerSecret)]
        [InlineData(ConsumerKey, "")]
        public void NullAndEmptyArgumentTest(string key, string secret)
        {
            var consumer = new Action(() => new ConsumerCredentials(key, secret));
            Assert.Throws<ArgumentException>(consumer);
        }
        
        
        [Fact]
        public void ConsumerKeyWhitespaceTest()
        {
            var consumer = new ConsumerCredentials("", ConsumerSecret);
            Assert.NotNull(consumer);
        }
    }
}