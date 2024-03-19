using System.Globalization;
using System.Resources;

namespace PhanMenQuanLy
{
    public class ResourceHelper
    {
        private static ResourceManager _resourceManager;

        static ResourceHelper()
        {
            _resourceManager = new ResourceManager("PhanMenQuanLy.Resource", typeof(ResourceHelper).Assembly);
        }

        public static string GetString(string key, CultureInfo culture = null)
        {
            if (culture == null)
                culture = CultureInfo.CurrentUICulture;

            return _resourceManager.GetString(key, culture);
        }
    }
}
