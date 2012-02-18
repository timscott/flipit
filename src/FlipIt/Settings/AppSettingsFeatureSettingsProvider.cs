using System;
using System.Configuration;
using System.Linq;

namespace FlipIt.Settings
{
    /// <summary>
    /// An implementation of IFeatureSettingsProvider based on .NET configuration app settings.
    /// </summary>
    public class AppSettingsFeatureSettingsProvider : IFeatureSettingsProvider
    {
        private readonly ICache<string, FeatureSettingBase> cachedSettings = new ThreadSafeCache<string, FeatureSettingBase>();

        /// <summary>
        /// Gets a setting from app settings using the name as key and converts the value to the specific type.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="name">The app settings key.</param>
        /// <returns>The setting value.</returns>
        public FeatureSetting<T> Get<T>(string name)
        {
            FeatureSetting<T> cached;
            if (TryGetFromCache(name, out cached))
            {
                return cached;
            }
            var setting = GetSetting<T>(name);
            cachedSettings.Store(name, setting);
            return setting;
        }

        /// <summary>
        /// Gets a setting from app settings using the name as key and converts the value to a list of items of the
        /// specific type.  The list is pipe delimited.  Blank entries are retained and treated as null.
        /// </summary>
        /// <typeparam name="T">The item type of the returned list.</typeparam>
        /// <param name="name">The app settings key.</param>
        /// <returns>A list of values.</returns>
        public FeatureListSetting<T> GetList<T>(string name)
        {
            FeatureListSetting<T> cached;
            if (TryGetFromCache(name, out cached))
            {
                return cached;
            }
            var setting = GetDelimitedSettingList<T>(name);
            cachedSettings.Store(name, setting);
            return setting;
        }

        protected FeatureSetting<T> GetSetting<T>(string key)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                return new FeatureSetting<T>(true, default(T));
            }
            var value = ChangeType<T>(setting);
            return new FeatureSetting<T>(false, value);
        }

        protected FeatureListSetting<T> GetDelimitedSettingList<T>(string key)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                return new FeatureListSetting<T>(true, default(T[]));
            }
            if (setting == string.Empty)
            {
                return new FeatureListSetting<T>(false, new T[0]);
            }
            var items = setting.Split(new[] { '|' }).Select(x => x == string.Empty ? null : x);
            var value = items.Select(ChangeType<T>).ToArray();
            return new FeatureListSetting<T>(false, value);
        }

        private static T ChangeType<T>(object value)
        {
            var type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null)
                {
                    return default(T);
                }
                type = Nullable.GetUnderlyingType(type);
            }
            return (T)Convert.ChangeType(value, type);
        }


        private bool TryGetFromCache<TSetting>(string name, out TSetting cached) where TSetting : FeatureSettingBase
        {
            FeatureSettingBase found;
            if (!cachedSettings.TryGet(name, out found))
            {
                cached = null;
                return false;
            }
            if (found.GetType() == typeof(TSetting))
            {
                cached = (TSetting)found;
                return true;
            }
            throw new InvalidOperationException(string.Format(
                "Cached feature setting with name '{0}' was the not the expected type: {1}.  It was of type {2}",
                name, typeof(TSetting).Name, found.GetType().Name));
            
        }
    }
}