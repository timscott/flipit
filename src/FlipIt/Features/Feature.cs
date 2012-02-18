using System;
using FlipIt.Settings;

namespace FlipIt.Features
{
    /// <summary>
    /// Base class for a feature.  Uses a setting to flip a feature.
    /// </summary>
    /// <typeparam name="T">The type of the value of the setting used to decide whether the feature is OFF or ON.</typeparam>
    public abstract class Feature<T> : Feature<FeatureSetting<T>, T>
    {
        protected Feature(string settingName, Func<T, bool> isOnFunc) : base(settingName, isOnFunc) { }

        protected override FeatureSetting<T> GetFeatureSetting(IFeatureSettingsProvider featureSettingsProvider)
        {
            return featureSettingsProvider.Get<T>(settingName);
        }
    }

    /// <summary>
    /// Base class for a feature that uses a list setting to flip a feature.
    /// </summary>
    /// <typeparam name="T">The setting used to decide whether the feature is off or on is a list. This is the item type of that list.</typeparam>
    /// <typeparam name="TFeatureSetting">What kind of setting the feature uses.</typeparam>
    public abstract class Feature<TFeatureSetting, T> : IFeature where TFeatureSetting : FeatureSettingBase<T>
    {
        protected readonly string settingName;
        protected readonly Func<T, bool> isOnFunc;

        /// <param name="settingName">The name of the setting.</param>
        /// <param name="isOnFunc">The function used to decide if the feature is OFF or ON.  The input parameter is the setting value.</param>
        protected Feature(string settingName, Func<T, bool> isOnFunc)
        {
            this.settingName = settingName;
            this.isOnFunc = isOnFunc;
        }

        /// <summary>
        /// Whether the feature is OFF or ON.
        /// </summary>
        /// <param name="featureSettingsProvider">The provider used to fetch the setting.</param>
        /// <returns>Whether the feature is OFF or ON.</returns>
        public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
        {
            var setting = GetFeatureSetting(featureSettingsProvider);
            return setting.IsOn(isOnFunc);
        }

        protected abstract TFeatureSetting GetFeatureSetting(IFeatureSettingsProvider featureSettingsProvider);
    }
}