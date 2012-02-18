using FlipIt.Settings;

namespace FlipIt.Features
{
    /// <summary>
    /// An interface for features.
    /// </summary>
    public interface IFeature
    {
        /// <summary>
        /// Whether the feature is OFF or ON.
        /// </summary>
        /// <param name="featureSettingsProvider">The feature settings provider.</param>
        /// <returns>Whether the feature is OFF or ON.</returns>
        bool IsOn(IFeatureSettingsProvider featureSettingsProvider);
    }
}