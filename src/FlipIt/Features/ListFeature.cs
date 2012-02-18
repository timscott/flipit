using System;
using System.Collections.Generic;
using FlipIt.Settings;

namespace FlipIt.Features
{
    /// <summary>
    /// Base class for a feature that uses a list setting to flip a feature.
    /// </summary>
    /// <typeparam name="T">The setting used to decide whether the feature is off or on is a list. This is the item type of that list.</typeparam>
    public abstract class ListFeature<T> : Feature<FeatureListSetting<T>,  IEnumerable<T>>
    {
        protected ListFeature(string settingName, Func<IEnumerable<T>, bool> isOnFunc) :base(settingName, isOnFunc) { }

        protected override FeatureListSetting<T> GetFeatureSetting(IFeatureSettingsProvider featureSettingsProvider)
        {
            return featureSettingsProvider.GetList<T>(settingName);
        }
    }
}