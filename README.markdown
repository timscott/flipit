# Introduction

FlipIt is a feature flipper.  It provides a simple and fexible way to flip features in a .NET application. Conditionally turn features or and off using any kind of logic. Change those conditions over time without touching the production code where the features are implemented.

# Usage

## Simple Scenario

Scenario: Flip a feature on and off for everyone all the time.

Create a feature.  Let's make it default to **on** if the setting is missing. You'll see why below.

	public class MyFeature : IFeature
	{
	    public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
	    {
	        return featureSettingsProvider.Get<bool?>("myFeatureIsOn") ?? true; 
	    }
	}

Use it in one or more places in your app to flip the feature.

	public class PlaceWhereMyFeatureIsImplemented
	
		private readonly IFeatureFlipper flipper;

		public PlaceWhereMyFeatureIsImplemented(IFeatureFlipper flipper)
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


Let's flip the feature **off** by adding a setting.

	<appSettings>
		<add key="myFeatureIsOn" value="false"/>
	</appSettings>

To flip the feature on, change the value to `true`.  When you're sure you never want to flip it off again, delete the setting. (You don't need some old setting hanging around cluttering things up.)


## More Complex Scenario

Scenario: Roll out a feature region by region.  (Send notifications to delivery locations when an order changes to In Route status.) 

	public class SendInRouteNotificationsFeature : IFeature
	{
		private readonly Region region;

		public SendInRouteNotificationsFeature(Region region)
		{
			this.region = region;
		}

	    public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
	    {
	        var onRegionIds = featureSettingsProvider.GetList<int>("regionsWithInRouteNotificationsOn");
			return onRegionIds == null || onRegionIds.Contains(region.Id);
	    }
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
		<add key="regionsWithInRouteNotificationsOn" value="12|31"/>
	</appSettings>

To roll out the feature, add more region IDs to the pipe delimited list.  When the roll out is complete, just remove the setting.

# Other Stuff

## IoC

### IoC Lovers

The above examples provide dependencies with constructor injection. For that you probably want to use an IoC container. Configure FlipIt in your container, for example with StructureMap.

	factory.For<IFeatureFlipper>().Use<FeatureFlipper>();
	factory.For<IFeatureSettingsProvider>().Use<AppSettingsFeatureSettingsProvider>();

### IoC Haters

Use the built in static class instead of providing an instance via the constructor.  But don't blame us when your code is hard to unit test.

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
