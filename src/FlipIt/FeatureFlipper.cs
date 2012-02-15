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
            FlipperConfiguration.FeatureSettingsProvider = new AppSettingsFeatureSettingsProvider();
            if (Flipper.IsOn(new Foo()))
            {
            }
        }
    }

    public class Foo : IFeature
    {
        public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
        {
            throw new NotImplementedException();
        }
    }

    public static class FlipperConfiguration
    {
        private static IFeatureSettingsProvider featureSettingsProvider;

        public static IFeatureSettingsProvider FeatureSettingsProvider
        {
            get { return featureSettingsProvider = (featureSettingsProvider ?? new AppSettingsFeatureSettingsProvider()); }
            set { featureSettingsProvider = value; }
        }
    }

    public static class Flipper
    {
        public static bool IsOn<T>(T feature) where T : IFeature
        {
            return feature.IsOn(FlipperConfiguration.FeatureSettingsProvider);
        }

        public static void DoIfOn<T>(T feature, Action action) where T : IFeature
        {
            if (IsOn(feature))
            {
                action();
            }
        }
    }
}