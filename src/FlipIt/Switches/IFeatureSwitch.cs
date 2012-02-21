using FlipIt.Settings;

namespace FlipIt.Switches
{
    /// <summary>
    /// An interface for feature switches.
    /// </summary>
    public interface IFeatureSwitch
    {
        /// <summary>
        /// Whether the feature is OFF or ON.
        /// </summary>
        /// <param name="featureSettingsProvider">The feature settings provider.</param>
        /// <returns>Whether the feature is OFF or ON.</returns>
        bool IsOn(IFeatureSettingsProvider featureSettingsProvider);
    }
}