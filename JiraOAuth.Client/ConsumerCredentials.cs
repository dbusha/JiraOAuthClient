using System;
using System.Security.Cryptography;
using OAuth;

namespace JiraOAuth.Client
{
    public class ConsumerCredentials
    {
        public ConsumerCredentials(string key, string pemFileContents)
        {
            Key = key ?? throw new ArgumentException("The key cannot be null");
            
            if (pemFileContents == null || String.IsNullOrWhiteSpace(pemFileContents))
                throw new ArgumentException("The consumer secret cannot be null or empty");
            Secret = RSA.Create(RsaPrivateKeyParser.ParsePem(pemFileContents)).ToXmlString(true);
        }
                
        public string Key { get; }
        public string Secret { get; }
    }
}