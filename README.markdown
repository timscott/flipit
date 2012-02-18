# Introduction

FlipIt is a feature flipper.  It provides a simple and flexible way to flip features in a .NET application. Conditionally turn features or and off using any kind of logic. Change those conditions over time without touching the production code where the features are implemented.

# Usage

## Simple Scenario

Scenario: Flip a feature on and off for everyone all the time.

Create a feature.

	public class MyFeature : BooleanFeature
	{
	    public MyFeature() : base("my_feature_is_on") { }
	}

Use it in one or more places in your app to flip the feature.

	public class WhereMyFeatureIsImplemented
	{
		private readonly IFeatureFlipper flipper;

		public WhereMyFeatureIsImplemented(IFeatureFlipper flipper)
		{
			this.flipper = flipper;
		}

		public void DoSomething()
		{
			flipper.DoIfOn(new MyFeature(), () => 
			{
				//do the feature
			});
		}
	}


Let's flip the feature `OFF` by adding a setting.

	<appSettings>
		<add key="my_feature_is_on" value="false"/>
	</appSettings>

To flip the feature back `ON`, change the value to `true`.  When you're sure you never want to flip it off again, delete the setting. (You don't need some old setting hanging around cluttering things up.)


## More Complex Scenario

Scenario: Roll out a feature region by region.  (Send notifications to delivery locations when an order changes to In Route status.) 

Create the feature.

	public class SendInRouteNotificationsFeature : ListFeature<int>
	{
		public SendInRouteNotificationsFeature(Region region) : base(
			settingName: "region_ids_with_in_route_notifications_enabled", 
			isOnFunc: ids => ids.Contains(region.Id)) { }
	}

Flip the feature.

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

			var notifyFeature = new SendInRouteNotificationsFeature(evt.Order.Destination.Region);
			if (flipper.IfOn(notifyFeature))
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

In the above examples we satisfy dependencies using constructor injection. For that you probably want to use an IoC container. Configure FlipIt in your container, for example with StructureMap.

	factory.For<IFeatureFlipper>().Use<FeatureFlipper>();
	factory.For<IFeatureSettingsProvider>().Use<AppSettingsFeatureSettingsProvider>();

### IoC Haters

Use the built-in static class instead of providing an instance via the constructor.  But don't blame us when your code is hard to unit test.

	Flipper.DoIfOn(new MyFeature(), () => 
	{
		//do the feature
	});

The default settings provider is `AppSettingsFeatureSettingsProvider`.  You can change it.

	FlipItConfig.FeatureSettingsProvider = new SomeCustomFeatureSettingsProvider();

## Open/Closed Principle

Code that implements features should be "closed for modification" after flipping is set up.  We want to be able to change application behavior with settings only.  Worst case, we should only have to change the feature class.  We should *never* have to change the code where the feature is flipped.

## Settings

### What Are Settings?

A setting can be anything. It's whatever bits of information you need to flip a feature using custom logic.  Some feature flippers only allow `on` and `off` settings.  That's too limiting for our needs.  Some feature flippers are based strictly on users and user groups. You can do that with FlipIt, but it's not baked in.

### Where Are Settings?

The preceding examples use the built-in `AppSettingsFeatureSettingsProvider` which uses .NET configuration as the settings store.  This is simple and natural in many environments.  However, what if you want non-technical staff (or techies without production access) to be able to flip features?

Create your own implementation of `IFeatureSettingsProvider`. For example, you might create `SqlServerFeatureSettingsProvider` or `MongoFeatureSettingsProvider`.  From there it's easy to imagine a simple admin UI for feature flipping.  Oh yeah, if you create any of these implementations, please share!

## Making Features

The preceding examples create features using the base classes `BooleanFeature` and `ListFeature<T>`.  There's `Feature<T>` too.  But you don't have to use these.  It's easy to create features from scratch that do anything you can imagine.

	public class CoinTossFeature : IFeature
	{
	    public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
	    {
	        return new Random().Next(2) == 1; //good enough 
	    }
	}

