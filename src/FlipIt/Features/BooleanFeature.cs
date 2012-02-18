using FlipIt.Settings;

namespace FlipIt.Features
{
    /// <summary>
    /// A base class that provides a terse way to create IFeatures that are based on a simple boolean settings.
    /// </summary>
    public abstract class BooleanFeature : Feature<FeatureSetting<bool>,  bool>
    {
        /// <param name="settingName">The name of the setting.</param>
        protected BooleanFeature(string settingName) : base(settingName, v => v) { }

        protected override FeatureSetting<bool> GetFeatureSetting(IFeatureSettingsProvider featureSettingsProvider)
        {
            return featureSettingsProvider.Get<bool>(settingName);
        }
    }
}