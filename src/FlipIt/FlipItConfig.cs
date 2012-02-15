namespace FlipIt
{
    public static class FlipItConfig
    {
        private static IFeatureSettingsProvider featureSettingsProvider;

        public static IFeatureSettingsProvider FeatureSettingsProvider
        {
            get { return featureSettingsProvider = (featureSettingsProvider ?? new AppSettingsFeatureSettingsProvider()); }
            set { featureSettingsProvider = value; }
        }
    }
}