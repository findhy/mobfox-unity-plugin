using UnityEngine;
using System.Collections;

public class MobFoxUIManager : MonoBehaviour {

	private string _bannerAdUnitId = "fe96717d9875b9da4339ea5367eff1ec";
	private string _interstitialAdUnitId = "80187188f458cfde788d961b6882fd53";
	private string _labelText = "";
	
	private Vector2 scrollPosition;

	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 800 || Screen.height >= 800 ) ? 320 : 160;
		float height = ( Screen.width >= 800 || Screen.height >= 800 ) ? 80 : 40;
		float heightPlus = height + 10.0f;
		
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Create default Banner ad" ) )
		{
			// will create banner with default 320x50 size.
			MobFox.createBanner( _bannerAdUnitId, MobFox.BannerAlignment.BottomCenter );
		}

		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Create Banner ad with size: 300x250" ) )
		{
			MobFox.createBanner( _bannerAdUnitId, MobFox.BannerAlignment.BottomLeft, 300, 250 );
		}	
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Destroy Banner" ) )
		{
			MobFox.destroyBanner();
		}		
		
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Request Interstitial" ) )
		{
			MobFox.requestInterstitalAd( _interstitialAdUnitId );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Interstitial" ) )
		{
			MobFox.showInterstitalAd();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Banner" ) )
		{
			MobFox.setBannerVisibility( true );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Hide Banner" ) )
		{
			MobFox.setBannerVisibility( false );
		}

		float labelWidth = Screen.width - 2.5F * width;
		float labelHeight = Screen.height / 2;

		if (labelWidth < width) { //not enough space between buttons for log
			labelWidth = width;
			yPos += heightPlus;
			labelHeight = Screen.height - yPos - 60; //leave some place for banner and buttons
		} else {
			xPos = Screen.width / 2 - labelWidth / 2;
			yPos = 5.0f;
		}

		scrollPosition = new Vector2(scrollPosition.x, Mathf.Infinity);
		GUILayout.BeginArea(new Rect(xPos, yPos, labelWidth, labelHeight));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width (labelWidth), GUILayout.Height (labelHeight));
		GUILayout.Label (_labelText);
		GUILayout.EndScrollView ();
		GUILayout.EndArea();
		
	}
	
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		MobFox.onBannerAdLoadedEvent += onBannerAdLoadedEvent;
		MobFox.onBannerAdFailedEvent += onBannerAdFailedEvent;
		MobFox.onBannerAdClickedEvent += onBannerAdClickedEvent;
		MobFox.onBannerAdShownEvent += onBannerAdShownEvent;
		MobFox.onBannerAdClosedEvent += onBannerAdClosedEvent;
		MobFox.onInterstitialLoadedEvent += onInterstitialLoadedEvent;
		MobFox.onInterstitialFailedEvent += onInterstitialFailedEvent;
		MobFox.onInterstitialShownEvent += onInterstitialShownEvent;
		MobFox.onInterstitialClickedEvent += onInterstitialClickedEvent;
		MobFox.onInterstitialDismissedEvent += onInterstitialDismissedEvent;
	}
	
	
	void OnDisable()
	{
		// Remove all event handlers
		MobFox.onBannerAdLoadedEvent -= onBannerAdLoadedEvent;
		MobFox.onBannerAdFailedEvent -= onBannerAdFailedEvent;
		MobFox.onBannerAdClickedEvent -= onBannerAdClickedEvent;
		MobFox.onBannerAdShownEvent -= onBannerAdShownEvent;
		MobFox.onBannerAdClosedEvent -= onBannerAdClosedEvent;
		MobFox.onInterstitialLoadedEvent -= onInterstitialLoadedEvent;
		MobFox.onInterstitialFailedEvent -= onInterstitialFailedEvent;
		MobFox.onInterstitialShownEvent -= onInterstitialShownEvent;
		MobFox.onInterstitialClickedEvent -= onInterstitialClickedEvent;
		MobFox.onInterstitialDismissedEvent -= onInterstitialDismissedEvent;
	}
	
	
	
	void onBannerAdLoadedEvent()
	{
		Debug.Log( "banner ad loaded" );
		_labelText += "banner ad loaded\n";
	}
	
	void onBannerAdFailedEvent()
	{
		Debug.Log( "banner ad failed" );
		_labelText += "banner ad failed\n";
	}
	
	void onBannerAdClickedEvent()
	{
		Debug.Log( "banner ad clicked" );
		_labelText += "banner ad clicked\n";
	}
	
	void onBannerAdShownEvent()
	{
		Debug.Log( "banner ad shown" );
		_labelText += "banner ad shown\n";
	}
	
	void onBannerAdClosedEvent()
	{
		Debug.Log( "banner ad closed" );
		_labelText += "banner ad closed\n";
	}
	
	void onInterstitialLoadedEvent()
	{
		Debug.Log( "interstitial ad loaded" );
		_labelText += "interstitial ad loaded\n";
	}
	
	void onInterstitialFailedEvent()
	{
		Debug.Log( "interstitial ad failed" );
		_labelText += "interstitial ad failed\n";
	}
	
	void onInterstitialShownEvent()
	{
		Debug.Log( "interstitial ad shown" );
		_labelText += "interstitial ad shown\n";
	}
	
	void onInterstitialClickedEvent()
	{
		Debug.Log( "interstitial ad clicked" );
		_labelText += "interstitial ad clicked\n";
	}
	
	void onInterstitialDismissedEvent()
	{
		Debug.Log( "interstitial ad dismissed" );
		_labelText += "interstitial ad dismissed\n";
	}


}
