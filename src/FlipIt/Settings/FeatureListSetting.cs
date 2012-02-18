using System.Collections.Generic;

namespace FlipIt.Settings
{
    /// <summary>
    /// A setting where the value is a list of items.
    /// </summary>
    /// <typeparam name="T">The item type of the list</typeparam>
    public class FeatureListSetting<T> : FeatureSettingBase<IEnumerable<T>> 
    {
        /// <param name="missing">Whether the setting is missing.</param>
        /// <param name="value">The value of the setting.</param>
        public FeatureListSetting(bool missing, IEnumerable<T> value) : base(missing, value) { }
    }
}