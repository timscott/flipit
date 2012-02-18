using System;
using FlipIt.Features;

namespace FlipIt
{
    /// <summary>
    /// A static class for feature flipping as an alerternative of inversion of control pattern.
    /// </summary>
    public static class Flipper
    {
        private static readonly FeatureFlipper flipper;

        static Flipper()
        {
            flipper = new FeatureFlipper(FlipItConfig.FeatureSettingsProvider);
        }

        /// <summary>
        /// Whether the feature is OFF or ON.
        /// </summary>
        /// <typeparam name="T">The type of the feature.</typeparam>
        /// <param name="feature">The feature.</param>
        /// <returns>Whether the feature is OFF or ON.</returns>
        public static bool IsOn<T>(T feature) where T : IFeature
        {
            return flipper.IsOn(feature);
        }

        /// <summary>
        /// Perform the specified action if the feature is ON.
        /// </summary>
        /// <typeparam name="T">The type of the feature.</typeparam>
        /// <param name="feature">The feature.</param>
        /// <param name="action">The action to perform if the feature is ON.</param>
        public static void DoIfOn<T>(T feature, Action action) where T : IFeature
        {
            flipper.DoIfOn(feature, action);
        }
    }
}