using System;
using FlipIt.Features;

namespace FlipIt
{
    public interface IFeatureFlipper
    {
        /// <summary>
        /// Whether the feature is OFF or ON.
        /// </summary>
        /// <typeparam name="T">The type of the feature.</typeparam>
        /// <param name="feature">The feature.</param>
        /// <returns>Whether the feature is OFF or ON.</returns>
        bool IsOn<T>(T feature) where T : IFeature;

        /// <summary>
        /// Perform the specified action if the feature is On.
        /// </summary>
        /// <typeparam name="T">The type of the feature.</typeparam>
        /// <param name="feature">The feature.</param>
        /// <param name="action">The action to perform if the feature is ON.</param>
        void DoIfOn<T>(T feature, Action action) where T : IFeature;
    }
}