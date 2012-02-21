using FlipIt.Settings;

namespace FlipIt.Switches
{
    /// <summary>
    /// A base class that provides a terse way to create IFeatureSwitches that are based on a simple boolean settings.
    /// </summary>
    public abstract class BooleanFeatureSwitch : FeatureSwitch<FeatureSetting<bool>,  bool>
    {
        /// <param name="settingName">The name of the setting.</param>
        protected BooleanFeatureSwitch(string settingName) : base(settingName, v => v) { }

        protected override FeatureSetting<bool> GetFeatureSetting(IFeatureSettingsProvider featureSettingsProvider)
        {
            return featureSettingsProvider.Get<bool>(settingName);
        }
    }
}