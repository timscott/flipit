using FlipIt.Settings;

namespace FlipIt.Tests
{
    public class NullFeatureSettingsProvider : IFeatureSettingsProvider
    {
        public FeatureSetting<T> Get<T>(string name)
        {
            return new FeatureSetting<T>(false, default(T));
        }

        public FeatureListSetting<T> GetList<T>(string name)
        {
            return new FeatureListSetting<T>(false, default(T[]));
        }
    }
}