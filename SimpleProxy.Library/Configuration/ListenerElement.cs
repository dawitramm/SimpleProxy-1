using System.Configuration;

namespace SimpleProxy.Library.Configuration
{
    public class ListenerElement : ConfigurationElement
    {
        [ConfigurationProperty("prefix", IsKey = true, IsRequired = true)]
        public string Prefix
        {
            get { return (string)this["prefix"]; }
            set { this["prefix"] = value; }
        }
    }
}