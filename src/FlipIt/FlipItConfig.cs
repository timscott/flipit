using FlipIt.Settings;

namespace FlipIt
{
    /// <summary>
    /// Static context for configuration.
    /// </summary>
    public static class FlipItConfig
    {
        private static IFeatureSettingsProvider featureSettingsProvider;

        /// <summary>
        /// Static definition of the feature settings provider to use when using FlipIt statically.
        /// </summary>
        public static IFeatureSettingsProvider FeatureSettingsProvider
        {
            get { return featureSettingsProvider = (featureSettingsProvider ?? new AppSettingsFeatureSettingsProvider()); }
            set { featureSettingsProvider = value; }
        }
    }
}