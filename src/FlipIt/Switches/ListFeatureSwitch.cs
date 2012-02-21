using System;
using System.Collections.Generic;
using FlipIt.Settings;

namespace FlipIt.Switches
{
    /// <summary>
    /// Base class for a feature switch that uses a list setting to flip a feature.
    /// </summary>
    /// <typeparam name="T">The setting used to decide whether the feature is off or on is a list. This is the item type of that list.</typeparam>
    public abstract class ListFeatureSwitch<T> : FeatureSwitch<FeatureListSetting<T>,  IEnumerable<T>>
    {
        protected ListFeatureSwitch(string settingName, Func<IEnumerable<T>, bool> isOnFunc) :base(settingName, isOnFunc) { }

        protected override FeatureListSetting<T> GetFeatureSetting(IFeatureSettingsProvider featureSettingsProvider)
        {
            return featureSettingsProvider.GetList<T>(settingName);
        }
    }
}