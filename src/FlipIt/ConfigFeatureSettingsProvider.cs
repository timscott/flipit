using System;
using System.Configuration;
using System.Linq;

namespace FlipIt
{
    public class ConfigFeatureSettingsProvider : IFeatureSettingsProvider
    {
        public T Get<T>(string name)
        {
            return GetSetting<T>(name);
        }

        public T[] GetList<T>(string name)
        {
            return GetDelimitedSettingList<T>(name);
        }

        protected T GetSetting<T>(string key)
        {
            var setting = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(setting)
                ? default(T)
                : ChangeType<T>(setting);
        }

        protected T[] GetDelimitedSettingList<T>(string key)
        {
            var list = GetSetting<string>(key);
            if (list == null)
            {
                return default(T[]);
            }
            var items = list.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            return items.Select(ChangeType<T>).ToArray();
        }

        public static T ChangeType<T>(object value)
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
    }
}