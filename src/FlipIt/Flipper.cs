using System;

namespace FlipIt
{
    public static class Flipper
    {
        private static readonly FeatureFlipper flipper;

        static Flipper()
        {
            flipper = new FeatureFlipper(FlipItConfig.FeatureSettingsProvider);
        }

        public static bool IsOn<T>(T feature) where T : IFeature
        {
            return flipper.IsOn(feature);
        }

        public static void DoIfOn<T>(T feature, Action action) where T : IFeature
        {
            flipper.DoIfOn(feature, action);
        }
    }
}