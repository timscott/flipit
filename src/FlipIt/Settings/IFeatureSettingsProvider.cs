namespace FlipIt.Settings
{
    /// <summary>
    /// Interface for a feature settings provider.
    /// </summary>
    public interface IFeatureSettingsProvider
    {
        /// <summary>
        /// Gets a setting with the specified name having a value of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="name">The unique setting name.</param>
        /// <returns>The setting value.</returns>
        FeatureSetting<T> Get<T>(string name);

        /// <summary>
        /// Gets a setting with the specified name having a a list of values of the specified type.
        /// </summary>
        /// <typeparam name="T">The item type of the returned list.</typeparam>
        /// <param name="name">The setting name.</param>
        /// <returns>A list of values.</returns>
        FeatureListSetting<T> GetList<T>(string name);
    }
}