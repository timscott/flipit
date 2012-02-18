using System;
using FlipIt.Features;
using FlipIt.Settings;

namespace FlipIt
{
    /// <summary>
    /// Feature flipper class.  This class exists to provide a level of indirection primarily to support dependency 
    /// injection patterns.
    /// </summary>
    public class FeatureFlipper : IFeatureFlipper
    {
        private readonly IFeatureSettingsProvider featureSettingsProvider;

        /// <param name="featureSettingsProvider">The feature settings provider.</param>
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