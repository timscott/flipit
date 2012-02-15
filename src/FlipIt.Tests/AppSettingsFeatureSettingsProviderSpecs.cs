using Machine.Specifications;
using Should.Fluent;

namespace FlipIt.Tests
{
    public abstract class beahves_like_config_settings_provider_test
    {
        protected static AppSettingsFeatureSettingsProvider provider;

        Establish context = () => provider = new AppSettingsFeatureSettingsProvider();
    }

    public class when_getting_settings_that_is_a_list : beahves_like_config_settings_provider_test
    {
        protected static int[] result;

        Because of = () => result = provider.GetList<int>("numberList");

        It should_get_the_list = () => result
            .Should().Not.Be.Null()
            .Should().Count.Exactly(4)
            .Should().Contain.One(1)
            .Should().Contain.One(3)
            .Should().Contain.One(5)
            .Should().Contain.One(7); 
    }

    public class when_getting_a_list_settings_that_is_missing : beahves_like_config_settings_provider_test
    {
        protected static int[] result;

        Because of = () => result = provider.GetList<int>("notFound");

        It should_get_null = () => result.Should().Be.Null();
    }

    public class when_getting_a_settings_that_is_missing : beahves_like_config_settings_provider_test
    {
        protected static bool? result;

        Because of = () => result = provider.Get<bool?>("notFound");

        It should_get_false = () => result.Should().Be.Null();
    }
}