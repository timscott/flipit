using Machine.Specifications;

namespace FlipIt.Tests
{
    public class when_checking_if_a_feature_is_on
    {
        //Establish context = () => feature = new TestFeature();
    }

    public class TestFeature : IFeature
    {
        public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
        {
            return true;
        }
    }
}