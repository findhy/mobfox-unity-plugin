using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MobFox : MonoBehaviour
{
		private bool bannerAdShownWasReported = false;
		private bool bannerClickLeftApp = false;

		public enum BannerAlignment
		{
				TopLeft,
				TopCenter,
				TopRight,
				Centered,
				BottomLeft,
				BottomCenter,
				BottomRight
		}

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
	
		void Awake ()
		{
				// Set the GameObject name to the class name for easy access from Obj-C
				gameObject.name = this.GetType ().ToString ();
				DontDestroyOnLoad (this);
		}
	
		public void onBannerAdLoaded (string empty)
		{
				bannerAdShownWasReported = false;
				bannerClickLeftApp = false;
				if (onBannerAdLoadedEvent != null)
						onBannerAdLoadedEvent ();
		}
	
		public void onBannerAdFailed (string empty)
		{
				if (onBannerAdFailedEvent != null)
						onBannerAdFailedEvent ();
		}
	
		public void onBannerAdClicked (string empty)
		{
				if (onBannerAdClickedEvent != null)
						onBannerAdClickedEvent ();
		}
	
		public void onBannerAdShown (string empty)
		{
				bannerAdShownWasReported = true;
				if (onBannerAdShownEvent != null)
						onBannerAdShownEvent ();
		}

		public void onBannerAdClickWillLeaveApp (string empty)
		{
				if (!bannerAdShownWasReported) {
						bannerClickLeftApp = true;
						onBannerAdShown ("");
				}
		}
	
		public void onBannerAdClosed (string empty)
		{
				bannerAdShownWasReported = false;
				if (onBannerAdClosedEvent != null)
						onBannerAdClosedEvent ();
		}
	
		public void onInterstitialLoaded (string empty)
		{
				if (onInterstitialLoadedEvent != null)
						onInterstitialLoadedEvent ();
		}
	
		public void onInterstitialFailed (string empty)
		{
				if (onInterstitialFailedEvent != null)
						onInterstitialFailedEvent ();
		}
	
		public void onInterstitialShown (string empty)
		{
				if (onInterstitialShownEvent != null)
						onInterstitialShownEvent ();
		}
	
		public void onInterstitialClicked (string empty)
		{
				if (onInterstitialClickedEvent != null)
						onInterstitialClickedEvent ();
		}
	
		public void onInterstitialDismissed (string empty)
		{
				if (onInterstitialDismissedEvent != null)
						onInterstitialDismissedEvent ();
		}

		void OnApplicationPause (bool pauseStatus)
		{
				if (!pauseStatus && bannerClickLeftApp) { //used to report banner closed after coming back from app opened by banner click.
						onBannerAdClosed ("");
						bannerClickLeftApp = false;
				}
		}


		#region Externals
		[DllImport("__Internal")]
		private static extern void MobFoxCreateBanner (string adUnitId, BannerAlignment alignment, int width, int height);

		[DllImport("__Internal")]
		private static extern void MobFoxSetBannerVisibility (bool visible);

		[DllImport("__Internal")]
		private static extern void MobFoxDestroyBanner ();

		[DllImport("__Internal")]
		private static extern void MobFoxRequestInterstitalAd (string  adUnitId);

		[DllImport("__Internal")]
		private static extern void MobFoxShowInterstitalAd ();
		#endregion
			
		#if UNITY_ANDROID
		private static AndroidJavaObject _plugin;
		#endif

		// Creates a banner with the given ad unit placed based on the position parameter
		public static void createBanner (string adUnitId, BannerAlignment alignment)
		{
				createBanner (adUnitId, alignment, 320, 50);
		}
	
	
		// Creates a banner with the given ad unit and size placed based on the position parameter
		public static void createBanner (string adUnitId, BannerAlignment alignment, int width, int height)
		{
				#if UNITY_ANDROID
			if (_plugin == null) {
					using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.mobfox.unityplugin.MobFoxUnityPlugin")) {
							_plugin = pluginClass.CallStatic<AndroidJavaObject> ("instance");
					}
			}
			_plugin.Call ("createBanner", adUnitId, (int)alignment, width, height);
				#elif UNITY_IOS
			MobFoxCreateBanner(adUnitId, alignment, width, height);
				#endif


		}

		// Hides/shows the ad banner
		public static void setBannerVisibility (bool visible)
		{
				#if UNITY_ANDROID
			_plugin.Call( "setBannerVisibility", visible );
				#elif UNITY_IOS
		MobFoxSetBannerVisibility(visible);
				#endif
		}	
		
		// Destroys the banner and removes it from view
		public static void destroyBanner ()
		{
				#if UNITY_ANDROID
			_plugin.Call( "destroyBanner" );
				#elif UNITY_IOS
			MobFoxDestroyBanner();
				#endif
		}
		
		
		// Starts loading an interstitial ad
		public static void requestInterstitalAd (string adUnitId)
		{
				#if UNITY_ANDROID
			if (_plugin == null) {
				using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.mobfox.unityplugin.MobFoxUnityPlugin")) {
					_plugin = pluginClass.CallStatic<AndroidJavaObject> ("instance");
				}
			}
			
			_plugin.Call( "requestInterstitalAd", adUnitId );
				#elif UNITY_IOS
			MobFoxRequestInterstitalAd(adUnitId);
				#endif
		}		
		
		// If an interstitial ad is loaded this will take over the screen and show the ad
		public static void showInterstitalAd ()
		{
				#if UNITY_ANDROID
			_plugin.Call( "showInterstitalAd" );
				#elif UNITY_IOS
			MobFoxShowInterstitalAd();
				#endif
			
		}
		
		


}
