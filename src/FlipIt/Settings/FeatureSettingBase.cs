using System;

namespace FlipIt.Settings
{
    /// <summary>
    /// A base class for feature settings.
    /// </summary>
    public abstract class FeatureSettingBase
    {
        protected FeatureSettingBase(bool missing)
        {
            Missing = missing;
        }

        /// <summary>
        /// Whether the setting is missing.
        /// </summary>
        public bool Missing { get; private set; }
    }

    /// <summary>
    /// A base class for feature settings.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    public abstract class FeatureSettingBase<T> : FeatureSettingBase
    {
        protected FeatureSettingBase(bool missing, T value) : base(missing)
        {
            Value = value;
        }

        /// <summary>
        /// The value of the setting.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// The provided function determines whether the feature is OFF or ON, unless the setting is missing in which
        /// case the feature is deemed to be ON.
        /// </summary>
        /// <param name="func">The function used to determine if a feature is OFF or ON when the setting is present.</param>
        /// <returns>Whether the feature is OFF or ON.</returns>
        public bool IsOn(Func<T, bool> func)
        {
            return Missing || func(Value);
        }
    }
}