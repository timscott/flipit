# Introduction

FlipIt is a feature flipper for .NET.  It is a very simple tool with a limited feature set.  We simply want to provide a way to flip features based on any kind of conditions and to change those conditions over time without touching the features themselves.  Put another way we want to respect the Open/Closed principle.  We also want to fully support Inversion of Control.

# Usage

## Simple Scenareo

I want to be able to flip a feature on and off without deploying code.

Create a feature.

	public class MyFeature : IFeature
	{
	    public bool IsOn(IFeatureSettingsProvider featureSettingsProvider)
	    {
	        return featureSettingsProvider.Get<bool>("myFeatureIsOn");
	    }
	}

Use it one or more places in your app to flip the feature.

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


Because there is no app setting the feature is **on**.  Let's flip it **off**.

	<appSettings>
		<add key="myFeatureIsOn" value="false"/>
	</appSettings>

To flip the feature back on, simply delete the app setting, or change the value to `true`.

One last thing.  When you create PlaceWhereMyFeatureIsImplemented, you must supply the flipper, which in turn needs a settings provider.  We normally use an IoC container to do this. For example with StructureMap.

	container.For<IFeatureSettingsProvider>.Use<AppSettingsFeatureSettingsProvider>();

## More Complex Scenareo

I want to roll out a feature region by region.

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
			if (flipper.IfOn(notifyFeature)
			{
				SendNotification(evt.Order);
			}
		}
		
		private void SendNotification(Order order)
		{
			//do it
		}

	}

Limit the feature to beta regions 12 and 31.

	<appSettings>
		<add key="regionsWithInRouteNotificationsOn" value="12|31"/>
	</appSettings>

As we roll out, simply add more region IDs to the pipe delimited list.  When you're done rolling out, just remove the setting.

# Open/Closed Principle

Once you set up flipping for a feature you can consider that code "closed for modification."  Change you application's behavior by changing settings only.  Worst case, you can change the feature class only.  You should *never* have to change the code where the feature is flipped.

# Settings

## What Are Settings?

A setting can be anything. It's whatever information you need to flip a feature based on any kind of logic.  Some flipper tools decide how to flip features based only on users and user groups. You can do that with FlitIt, but it's not baked in.

## Where Are Settings?

The preceding examples use the built-in `AppSettingsFeatureSettingsProvider` which uses .NET configuration as the settings store.  This is simple and natural in many environments.  However, what if you want non-technical staff (or techies without production access) to be able to flip features?

Create your own implementation of `IFeatureSettingsProvider` and store feature settings any way that you want. For example you could create `SqlServerFeatureSettingsProvider` or `MongoFeatureSettingsProvider`.  From there it's not hard to imagine simple admin UI for feature flipping.
