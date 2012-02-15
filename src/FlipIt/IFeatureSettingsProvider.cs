namespace FlipIt
{
    public interface IFeatureSettingsProvider
    {
        T Get<T>(string name);
        T[] GetList<T>(string name);
    }
}