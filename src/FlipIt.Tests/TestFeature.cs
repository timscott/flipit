namespace FlipIt.Tests
{
    public class TestFeature : IFeature
    {
        private readonly bool isOn;

        public TestFeature(bool isOn)
        {
            this.isOn = isOn;
        }

        public int TimesIsOnWasCalled { get; private set; }

        public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
        {
            TimesIsOnWasCalled++;
            return isOn;
        }
    }
}