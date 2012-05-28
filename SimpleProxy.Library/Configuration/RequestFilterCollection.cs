using System.Configuration;

namespace SimpleProxy.Library.Configuration
{
    [ConfigurationCollection(typeof(RequestFilterElement), AddItemName = "filter")]
    public class RequestFilterElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RequestFilterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RequestFilterElement)element).Name;
        }
    }
}