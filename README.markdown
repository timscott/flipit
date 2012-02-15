# Introduction

FlipIt is a feature flipper.  It provides a simple and fexible way to flip features in a .NET application. Conditionally turn features or and off using any kind of logic. Change those conditions over time without touching the production code where the features are implemented.

# Usage

## Simple Scenario

Scenario: Flip a feature on and off for everyone all the time.  You'll see why below.

Create a feature.  Let's make it default **on** if the setting is missing. 

	public class MyFeature : IFeature
	{
	    public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
	    {
	        return featureSettingsProvider.Get<bool?>("myFeatureIsOn") ?? true; 
	    }
	}

Use the feature one or more places in your app to flip the feature.

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

To flip the feature on, change the value to `true`.  When you're sure you never want to turn it off again, you don't need that setting hanging around clutting things up.  Simply delete it.

### IoC

`PlaceWhereMyFeatureIsImplemented` has a dependency on `IFeatureSettingsProvider`. We like to use an IoC container for dependency injection. For example with StructureMap.

	x.For<IFeatureSettingsProvider>.Use<AppSettingsFeatureSettingsProvider>();

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
	        var onRegionIds = featureSettingsProvider.GetList<in>("regionsWithInRouteNotificationsOn");
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

# Open/Closed Principle

Code that implements features should be "closed for modification" after we set up flipping.  We want to be able to change application behavior with settings only.  Worst case, we should only have to change the feature class.  We should *never* have to change the code where the feature is flipped.

# Settings

## What Are Settings?

A setting can be anything. It is whatever bits of information you need to flip a feature using any kind of logic.  Some flipper tools decide how to flip features based strictly on users and user groups. You can do that with FlipIt, but it's not baked in.

## Where Are Settings?

The preceding examples use the built-in `AppSettingsFeatureSettingsProvider` which uses .NET configuration as the settings store.  This is simple and natural in many environments.  However, what if you want non-technical staff (or techies without production access) to be able to flip features?

Create your own implementation of `IFeatureSettingsProvider`. For example, you might create `SqlServerFeatureSettingsProvider` or `MongoFeatureSettingsProvider`.  From there it's not hard to imagine simple admin UI for feature flipping.  Oh yeah, if you create these implementations, please share!
