namespace FlipIt.Settings
{
    /// <summary>
    /// A setting with a single value.
    /// </summary>
    /// <typeparam name="T">The item type of the list</typeparam>
    public class FeatureSetting<T> : FeatureSettingBase<T>
    {
        /// <param name="missing">Whether the setting is missing.</param>
        /// <param name="value">The value of the setting.</param>
        public FeatureSetting(bool missing, T value) : base(missing, value) { }
    }
}