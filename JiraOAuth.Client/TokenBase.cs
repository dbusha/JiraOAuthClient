using System;
using System.ComponentModel;
using System.Linq;

namespace JiraOAuth.Client
{
    public class TokenBase
    {
        [Description("oauth_token")] 
        public string Token { get; protected set; }
        
        
        [Description("oauth_token_secret")] 
        public string TokenSecret { get; protected set; }
        
        
        protected string GetDescription(string propertyName)
        {
            var type = this.GetType();
            var property = type.GetProperty(propertyName);
                
            if (property == null)
                throw new ArgumentException($"{propertyName} does not exist on type");

            if (!(property.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute attribute))
                throw new ArgumentException($"{propertyName} has no description attribute");

            return attribute.Description;
        }
        
    }
}