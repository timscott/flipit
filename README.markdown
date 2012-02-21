# Introduction

FlipIt is a feature flipper.  It provides a simple and flexible way to flip features in a .NET application. Use FlipIt to conditionally turn features `OFF` and `ON` using custom logic per feature.  Change behavior over time without touching the production code where features are implemented.

The essence of FlipIt can be seen in this interface.

  	public interface IFeatureSwitch
  	{
  		bool IsOn(IFeatureSettingsProvider featureSettingsProvider);
  	}

# Usage

## Simple Case

Scenario: Flip a feature `ON` and `OFF` for everyone all the time.

Create a feature.

 	public class MyFeatureSwitch : BooleanFeatureSwitch
  	{
		public MyFeatureSwitch() : base("my_feature_enabled") { }
	}

Use it anywhere in your app to conditionally enable the feature.

	public class WhereMyFeatureIsImplemented
	{
		private readonly IFeatureFlipper flipper;

		public WhereMyFeatureIsImplemented(IFeatureFlipper flipper)
		{
			this.flipper = flipper;
		}

		public void DoSomething()
		{
			flipper.DoIfOn(new MyFeatureSwitch(), () => 
			{
				//do the feature
			});
		}
	}


The feature is `ON` by default.  Let's flip it `OFF` by adding a setting.  One way to store settings is in your application's config.

	<appSettings>
		<add key="my_feature_enabled" value="false"/>
	</appSettings>

To flip the feature back `ON`, change the value to `true`.  When you're sure you never want to flip it off again, delete the setting.


## More Complex Case

Scenario: Roll out a feature region by region.  (Send notifications to delivery locations when an order changes to In Route status.) 

Create the feature.

	public class SendInRouteNotificationsFeatureSwitch : ListFeatureSwitch<int>
	{
		public SendInRouteNotificationsFeatureSwitch(Region region) : base(
			settingName: "region_ids_with_in_route_notifications_enabled", 
			isOnFunc: ids => ids.Contains(region.Id)) { }
	}

Use it.

	public class HandleOrderInRouteEvent
	{
		private readonly IFeatureFlipper flipper;

		public InRouteNotifier(IFeatureFlipper flipper)
		{
			this.flipper = flipper;
		}

		public void Handle(OrderInRouteEvent evt)
		{
			//do stuff

			var switch = new SendInRouteNotificationsFeatureSwitch(evt.Order.Destination.Region);
			if (flipper.IfOn(switch))
			{
				//do it;
			}

			//do stuff
		}
	}

Start with regions 12 and 31.

	<appSettings>
		<add key="region_ids_with_in_route_notifications_enabled" value="12|31"/>
	</appSettings>

To roll out the feature to more regions, add their IDs to the pipe delimited list.  When the roll out is complete, just remove the setting.

# Other Stuff

## Inversion Of Control (IoC)

### IoC Lovers

In the preceding examples we satisfy dependencies using constructor injection. For that you probably want to use an IoC container. Configure FlipIt in your container, for example with StructureMap.

	factory.For<IFeatureFlipper>().Use<FeatureFlipper>();
	factory.For<IFeatureSettingsProvider>().Use<ConfigFeatureSettingsProvider>();

### IoC Haters

Use the built in static class instead of providing an instance via the constructor.  But don't blame us when your code is hard to unit test.

	Flipper.DoIfOn(new MyFeatureSwitch(), () => 
	{
		//do the feature
	});

The default settings provider is `ConfigFeatureSettingsProvider`.  You can change it.

	FlipItConfig.FeatureSettingsProvider = new SomeCustomFeatureSettingsProvider();

## Open/Closed Principle

Code that implements features should be "closed for modification" after flipping has been set up.  We want to be able to change how features are applied by changing settings only.  Worst case, we should only have to change the feature class.  We should *never* have to change the code where the feature is actually implemented.

## Making Features

In the usage examples we create features using the base classes `BooleanFeatureSwitch` and `ListFeatureSwitch<T>`.  There's `FeatureSwitch<T>` too.  These are nice, but you don't have to use them.  It's easy to create features from scratch that do anything you can imagine.

	public class CoinToss : IFeatureSwitch
	{
	    public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
	    {
	        return new Random().Next(2) == 1; //good enough 
	    }
	}

## Settings

### What Are Settings?

A setting can be anything. It's whatever bits of information you need to flip a feature using custom logic.  Some feature flippers only allow `ON` and `OFF` settings.  That's too limiting for our needs.  Some feature flippers are based strictly on users and user groups. You can do that with FlipIt, but it's not baked in.

### Where Are Settings?

The preceding examples use the built-in `ConfigFeatureSettingsProvider` which uses .NET configuration as the settings store.  This is simple and natural in many environments.  However, what if you want non-technical staff (or techies without production access) to be able to flip features?

Create your own implementation of `IFeatureSettingsProvider`. For example, you might create `SqlServerFeatureSettingsProvider` or `MongoFeatureSettingsProvider`.  From there it's easy to imagine a simple admin UI for feature flipping.  Oh yeah, if you create any of these implementations, please share!

## Missing Settings

All of the built-in FeatureSwitch classes use settings to flip features.  They treat a feature as `ON` if a setting is missing (by looking at the Missing property).  The reason is simple: features tend to move from `OFF` to permanently `ON`.  We don't want a bunch of old settings hanging around.  So when we're done flipping a feature, we can just remove the setting and leave the code alone.
