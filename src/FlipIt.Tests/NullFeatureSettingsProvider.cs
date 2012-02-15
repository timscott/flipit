namespace FlipIt.Tests
{
    public class NullFeatureSettingsProvider : IFeatureSettingsProvider
    {
        public T Get<T>(string name)
        {
            return default(T);
        }

        public T[] GetList<T>(string name)
        {
            return default(T[]);
        }
    }
}