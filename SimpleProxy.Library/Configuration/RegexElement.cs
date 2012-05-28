using System.Configuration;

namespace SimpleProxy.Library.Configuration
{
    public class RegexElement : ConfigurationElement
    {
        [ConfigurationProperty("regex", IsKey = true)]
        public string Regex
        {
            get { return (string)this["regex"]; }
            set { this["regex"] = value; }
        }
    }
}