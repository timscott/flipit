using FlipIt.Settings;
using FlipIt.Switches;

namespace FlipIt.Tests
{
    public class TestFeatureSwitch : IFeatureSwitch
    {
        private readonly bool isOn;

        public TestFeatureSwitch(bool isOn)
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