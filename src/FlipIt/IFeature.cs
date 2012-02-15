namespace FlipIt
{
    public interface IFeature
    {
        bool IsOn(IFeatureSettingsProvider featureSettingsProvider);
    }
}