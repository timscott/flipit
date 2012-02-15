using System;

namespace FlipIt
{
    public class FeatureFlipper : IFeatureFlipper
    {
        private readonly IFeatureSettingsProvider featureSettingsProvider;

        public FeatureFlipper(IFeatureSettingsProvider featureSettingsProvider)
        {
            this.featureSettingsProvider = featureSettingsProvider;
        }

        public bool IsOn<T>(T feature) where T : IFeature
        {
            return feature.IsOn(featureSettingsProvider);
        }

        public void DoIfOn<T>(T feature, Action action) where T : IFeature
        {
            if (IsOn(feature))
            {
                action();
            }
        }
    }
}