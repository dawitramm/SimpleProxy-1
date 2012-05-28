using System.Configuration;

namespace SimpleProxy.Library.Configuration
{
    public sealed class SimpleProxyConfigurationSection : ConfigurationSection
    {
        public static SimpleProxyConfigurationSection GetConfigSection()
        {
            return ConfigurationManager.GetSection("simpleProxySettings") as SimpleProxyConfigurationSection;
        }

        [ConfigurationProperty("listeners")]
        public ListenerElementCollection Listeners
        {
            get { return (ListenerElementCollection)base["listeners"]; }
        }

        [ConfigurationProperty("requestFilters")]
        public RequestFilterElementCollection RequestFilters
        {
            get { return (RequestFilterElementCollection)base["requestFilters"]; }
        }

        [ConfigurationProperty("whitelist")]
        public RegexElementCollection Whitelist
        {
            get { return (RegexElementCollection)base["whitelist"]; }
        }

        [ConfigurationProperty("blacklist")]
        public RegexElementCollection Blacklist
        {
            get { return (RegexElementCollection)base["blacklist"]; }
        }
    }
}
