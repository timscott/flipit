using Machine.Specifications;
using Should.Fluent;

namespace FlipIt.Tests
{
    public abstract class beahves_like_config_settings_provider_test
    {
        protected static ConfigFeatureSettingsProvider provider;
        protected static int[] result;

        Establish context = () => provider = new ConfigFeatureSettingsProvider();
    }

    public class when_getting_settings_that_is_a_list : beahves_like_config_settings_provider_test
    {
        Because of = () => result = provider.GetList<int>("numberList");

        It should_get_the_list = () => result
            .Should().Not.Be.Null()
            .Should().Count.Exactly(4)
            .Should().Contain.One(1)
            .Should().Contain.One(3)
            .Should().Contain.One(5)
            .Should().Contain.One(7); 
    }

    public class when_getting_settings_that_is_a_list_is_missing : beahves_like_config_settings_provider_test
    {
        Because of = () => result = provider.GetList<int>("notFound");

        It should_get_nullt = () => result.Should().Be.Null();
    }
}