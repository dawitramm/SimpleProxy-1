using System.Configuration;

namespace SimpleProxy.Library.Configuration
{
    [ConfigurationCollection(typeof(RegexElement))]
    public class RegexElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RegexElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RegexElement)element).Regex;
        }
    }
}