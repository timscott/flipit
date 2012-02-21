using System;
using FlipIt.Settings;
using FlipIt.Switches;

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

        public bool IsOn<T>(T feature) where T : IFeatureSwitch
        {
            return feature.IsOn(featureSettingsProvider);
        }

        public void DoIfOn<T>(T feature, Action action) where T : IFeatureSwitch
        {
            if (IsOn(feature))
            {
                action();
            }
        }
    }
}