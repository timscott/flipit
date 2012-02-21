using System;
using Machine.Specifications;
using Should.Fluent;

namespace FlipIt.Tests
{
    public class beahves_like_flipper_spec
    {
        protected static TestFeatureSwitch featureSwitch;

        Establish context = () =>
        {
            FlipItConfig.FeatureSettingsProvider = new NullFeatureSettingsProvider();
            featureSwitch = new TestFeatureSwitch(true);
        };
    }

    public class when_checking_if_flipper_feature_is_on : beahves_like_flipper_spec
    {
        static bool result;
        Because of = () => result = Flipper.IsOn(featureSwitch);
        It should_return_true = () => result.Should().Be.True();
        It should_call_feature_is_on = () => featureSwitch.TimesIsOnWasCalled.Should().Equal(1);
    }

    public class when_doing_flipper_feature_is_on : beahves_like_flipper_spec
    {
        static bool didDo;
        Because of = () => Flipper.DoIfOn(featureSwitch, () => { didDo = true; });
        It should_do_action = () => didDo.Should().Be.True();
    }
}