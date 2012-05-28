using System;
using System.Linq;

namespace SimpleProxy.Library.Plugins
{
    public class PluginLoader
    {
        public static IWebRequestFilter LoadRequestFilter(string filterType)
        {
            var type = (
                from a in AppDomain.CurrentDomain.GetAssemblies()
                from t in a.GetTypes()
                where t.FullName == filterType
                select t).SingleOrDefault();

            if (type == null)
                throw new ArgumentException(String.Format("Cannot load {0}.", filterType));

            if (type.GetInterface(typeof(IWebRequestFilter).Name) == null)
                throw new ArgumentException(String.Format("{0} does not implement {1}.", filterType, typeof(IWebRequestFilter).Name));
                
            var constructor = type.GetConstructor(new Type[] {});
            if (constructor == null)
                throw new ArgumentException(String.Format("{0} does not have a public parameterless constructor.", filterType));

            var filter = constructor.Invoke(null);

            return (IWebRequestFilter)filter;
        }
    }
}
