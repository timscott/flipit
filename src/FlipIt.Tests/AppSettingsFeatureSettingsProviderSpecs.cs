using FlipIt.Settings;
using Machine.Specifications;
using Should.Fluent;

namespace FlipIt.Tests
{
    public abstract class beahves_like_config_settings_provider_test
    {
        protected static ConfigFeatureSettingsProvider provider;

        Establish context = () => provider = new ConfigFeatureSettingsProvider();
    }

    public class when_getting_list_setting : beahves_like_config_settings_provider_test
    {
        static FeatureListSetting<int?> result;

        Because of = () => result = provider.GetList<int?>("numberList");

        It should_not_be_missing = () => result.Missing.Should().Be.False();

        It should_get_the_list = () => result.Value
            .Should().Not.Be.Null()
            .Should().Count.Exactly(5)
            .Should().Contain.One(1)
            .Should().Contain.One(3)
            .Should().Contain.One(5)
            .Should().Contain.One((int?)null)
            .Should().Contain.One(7); 
    }

    public class when_getting_a_list_setting_that_is_missing : beahves_like_config_settings_provider_test
    {
        static FeatureListSetting<int> result;

        Because of = () => result = provider.GetList<int>("notFound");

        It should_be_missing = () => result.Missing.Should().Be.True();

        It should_have_null_value = () => result.Value.Should().Be.Null();
    }

    public class when_getting_a_list_setting_that_is_blank : beahves_like_config_settings_provider_test
    {
        static FeatureListSetting<int> result;

        Because of = () => result = provider.GetList<int>("blankList");

        It should_not_be_missing = () => result.Missing.Should().Be.False();

        It should_return_an_empty_list = () => result.Value.Should().Be.Empty();
    }

    public class when_getting_avalue_type_setting_that_is_missing : beahves_like_config_settings_provider_test
    {
        static FeatureSetting<int> result;

        Because of = () => result = provider.Get<int>("notFound");

        It should_be_missing = () => result.Missing.Should().Be.True();

        It should_have_null_value = () => result.Value.Should().Equal(0);
    }

    public class when_getting_a_nullable_value_type_setting_that_is_missing : beahves_like_config_settings_provider_test
    {
        static FeatureSetting<bool?> result;

        Because of = () => result = provider.Get<bool?>("notFound");

        It should_be_missing = () => result.Missing.Should().Be.True();

        It should_have_null_value = () => result.Value.Should().Be.Null();
    }

    public class when_getting_a_setting_twice : beahves_like_config_settings_provider_test
    {
        static FeatureSetting<int> result1;
        static FeatureSetting<int> result2;

        Because of = () =>
        {
            result1 = provider.Get<int>("numberSetting");
            result2 = provider.Get<int>("numberSetting");
        };

        It should_get_same_instance = () => result1.Should().Be.SameAs(result2);
    }
}