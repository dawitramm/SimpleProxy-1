using System.Configuration;

namespace SimpleProxy.Library.Configuration
{
    [ConfigurationCollection(typeof(ListenerElement), AddItemName="listener")]
    public class ListenerElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ListenerElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ListenerElement)element).Prefix;
        }
    }
}