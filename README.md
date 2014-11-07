mobfox-unity-plugin
===================

Unity3D Plugin for displaying MobFox ads on iOS and Android

# MobFox Unity Plugin

The MobFox Unity Plugin allows you to provide MobFox ads in Unity projects deployed as native iOS and Android applications. The following ad formats are supported by the plugin:
-banner ads
-static fullscreen ads
-video fullscreen ads
-MRAID ads

The plugin is distributed as [.unitypackage file](https://github.com/mobfox/mobfox-unity-plugin/blob/master/MobFox_Unity.unitypackage?raw=true) for simple importing. The single package provides support for both Android and iOS.
The source code is also made available for those willing to fine-tune the plugin to their needs.

## Integration instructions
---
1. Open your project in Unity editor.
2. Import the [MobFox .unitypackage file](https://github.com/mobfox/mobfox-unity-plugin/blob/master/MobFox_Unity.unitypackage?raw=true) to your project
3. Select the files you want to import:
	- MobFox folder is required
	- From Plugins folder, choose which platform you want to support (or select both). Note that Android folder contains AndroidManifest.xml file, if you already have such file in your Plugins/Android/ catalog skip it.
	- To get implementation example, import also MobFoxDemoScene from Scenes folder and MobFoxUIManager from Scripts folder
4. In the scene where you want to display ads, create object with MobFox.cs script attached. You can simply drop the MobFox prefab to your scene to do so.

### Android instructions
---
1. Add the google-play-services_lib folder, located at ANDROID_SDK_LOCATION/extras/google/google_play_services/libproject, into the Plugins/Android folder of your project.
2. If you already had the AndroidManifest.xml file in Plugins/Android/ and skipped the one contained in MobFox .unitypackage, add the necessary permissions and activities required by MobFox SDK:
```
	<uses-permission android:name="android.permission.INTERNET" />
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
																			
	<activity
		android:name="com.adsdk.sdk.banner.InAppWebView"
		android:configChanges="keyboard|keyboardHidden|orientation|screenLayout|uiMode|screenSize|smallestScreenSize" />
	<activity
		android:name="com.adsdk.sdk.video.RichMediaActivity"
		android:configChanges="keyboard|keyboardHidden|orientation|screenLayout|uiMode|screenSize|smallestScreenSize"	
		android:hardwareAccelerated="false" />
	<activity		
		android:name="com.adsdk.sdk.mraid.MraidBrowser"
		android:configChanges="keyboard|keyboardHidden|orientation|screenLayout|uiMode|screenSize|smallestScreenSize" />
	<meta-data android:name="com.google.android.gms.version"
	android:value="@integer/google_play_services_version" /> 
```
3. To generate your project for Android, click File -> Build Settings, select the Android platform, then Switch Platform, mark Google Android Project checkbox, then Export. 
4. Import generated projects into your Eclipse workspace and run your application

### iOS instructions
---
1. To generate your project for iOS, click File -> Build Settings, select the iOS platform, then Switch Platform and click Build.
2. In generated XCode project:
	- Add the -ObjC linker flag to your Other Linker Flags under Project -> Build Settings.
	- Add mandatory Frameworks:
	```
		AudioToolbox
		AVFoundation
		iAd
		StoreKit
		SystemConfiguration
		MessageUI
		CoreTelephony
		EventKit
		EventKitUI
		AdSupport
		UIKit
		CoreLocation
		MediaPlayer
		Foundation
		CoreGraphics
	```
	- Add [MobFox.embeddedframework](https://github.com/mobfox/mobfox-unity-plugin/blob/master/MobFox.embeddedframework.zip?raw=true) to the Frameworks folder of your project in Xcode (checking the  "Copy items into destination group's folder" option).
3. Run your application.

## API Documentation
---
The MobFox/MobFox.cs plugin exposes the following methods:
```
// Creates a banner with the given ad unit placed based on the position parameter
public static void createBanner (string adUnitId, BannerAlignment alignment)

// Creates a banner with the given ad unit and size placed based on the position parameter
public static void createBanner (string adUnitId, BannerAlignment alignment, int width, int height)

// Hides/shows the ad banner
public static void setBannerVisibility (bool visible)

// Destroys the banner and removes it from view
public static void destroyBanner ()

// Starts loading an interstitial ad
public static void requestInterstitalAd (string adUnitId)

// If an interstitial ad is loaded this will take over the screen and show the ad
public static void showInterstitalAd ()
```

### Also the following events notifying about the state of the ads are available:
```
// Fired when a new banner ad is loaded
public static event Action onBannerAdLoadedEvent;

// Fired when a banner ad fails to load
public static event Action onBannerAdFailedEvent;

// Fired when a banner ad is clicked
public static event Action onBannerAdClickedEvent;

// Fired when a banner ad extends/opens fullscreen view
public static event Action onBannerAdShownEvent;

// Fired when a banner ad closes back to its initial size
public static event Action onBannerAdClosedEvent;

// Fired when an interstitial ad is loaded
public static event Action onInterstitialLoadedEvent;

// Fired when an interstitial ad fails to load
public static event Action onInterstitialFailedEvent;

// Fired when an interstitial ad is displayed
public static event Action onInterstitialShownEvent;

// Fired when an interstitial ad is clicked
public static event Action onInterstitialClickedEvent;

// Fired when an interstitial ad is dismissed
public static event Action onInterstitialDismissedEvent;
```

See also the Scripts/MobFoxUIManager.cs to see an example how the presented API can be utilized.
