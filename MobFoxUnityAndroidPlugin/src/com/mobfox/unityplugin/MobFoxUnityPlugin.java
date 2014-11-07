package com.mobfox.unityplugin;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.util.DisplayMetrics;
import android.view.Gravity;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.LinearLayout.LayoutParams;
import android.widget.RelativeLayout;
import com.adsdk.sdk.Ad;
import com.adsdk.sdk.AdListener;
import com.adsdk.sdk.AdManager;
import com.adsdk.sdk.banner.AdView;
import com.unity3d.player.UnityPlayer;

public class MobFoxUnityPlugin {
	private static MobFoxUnityPlugin sInstance;

	private AdView bannerView;
	private int width;
	private int height;

	private AdManager adManager;
	private RelativeLayout mLayout;

	public static MobFoxUnityPlugin instance() {
		if (sInstance == null) {
			sInstance = new MobFoxUnityPlugin();
		}
		return sInstance;
	}

	/*
	 * Banners API
	 */
	public void createBanner(final String adUnitId, final int alignment,
			final int width, final int height) {
		getActivity().runOnUiThread(new Runnable() {
			public void run() {
				if (bannerView != null) {
					destroyBanner();
				}
				MobFoxUnityPlugin.this.width = width;
				MobFoxUnityPlugin.this.height = height;

				bannerView = new AdView(getActivity(),
						"http://my.mobfox.com/request.php", adUnitId);
				bannerView.setAdspaceWidth(width);
				bannerView.setAdspaceHeight(height);
				bannerView.setAdListener(createBannerListener());

				prepLayout(alignment);

				mLayout.addView(bannerView);
				getActivity().addContentView(
						mLayout,
						new LayoutParams(LayoutParams.MATCH_PARENT,
								LayoutParams.MATCH_PARENT));

				mLayout.setVisibility(RelativeLayout.VISIBLE);
			}
		});
	}

	public void setBannerVisibility(final boolean visible) {
		if (bannerView == null) {
			return;
		}

		getActivity().runOnUiThread(new Runnable() {
			public void run() {
				if (visible) {
					bannerView.setVisibility(View.VISIBLE);
				} else {
					bannerView.setVisibility(View.GONE);
				}
			}
		});
	}

	public void destroyBanner() {
		getActivity().runOnUiThread(new Runnable() {
			public void run() {
				if (bannerView == null || mLayout == null)
					return;

				mLayout.removeAllViews();
				mLayout.setVisibility(LinearLayout.GONE);
				bannerView.release();
				bannerView = null;
			}
		});
	}

	/*
	 * Interstitials API
	 */
	public void requestInterstitalAd(final String adUnitId) {
		getActivity().runOnUiThread(new Runnable() {
			public void run() {
				if (adManager == null) {
					adManager = new AdManager(getActivity(),
							"http://my.mobfox.com/request.php", adUnitId, true);
					adManager.setListener(createInterstitialListener());
					adManager.setVideoAdsEnabled(true);
					adManager.setPrioritizeVideoAds(true);
				}

				adManager.requestAd();
			}
		});
	}

	public void showInterstitalAd() {
		getActivity().runOnUiThread(new Runnable() {
			public void run() {
				if (adManager != null && adManager.isAdLoaded()) {
					adManager.showAd();
				}
			}
		});
	}

	/*
	 * BannerAdListener implementation
	 */
	protected AdListener createBannerListener() {
		return new AdListener() {

			@Override
			public void noAdFound() {
				UnityPlayer.UnitySendMessage("MobFox", "onBannerAdFailed", "");
			}

			@Override
			public void adShown(Ad arg0, boolean arg1) {
				UnityPlayer.UnitySendMessage("MobFox", "onBannerAdShown", "");
			}

			@Override
			public void adLoadSucceeded(Ad arg0) {
				UnityPlayer.UnitySendMessage("MobFox", "onBannerAdLoaded", "");

				float density = getScreenDensity();

				RelativeLayout.LayoutParams params = (RelativeLayout.LayoutParams) bannerView
						.getLayoutParams();
				params.width = (int) (width * density);
				params.height = (int) (height * density);

				bannerView.setLayoutParams(params);
			}

			@Override
			public void adClosed(Ad arg0, boolean arg1) {
				UnityPlayer.UnitySendMessage("MobFox", "onBannerAdClosed", "");
			}

			@Override
			public void adClicked() {
				UnityPlayer.UnitySendMessage("MobFox", "onBannerAdClicked", "");
			}
		};
	}

	/*
	 * InterstitialAdListener implementation
	 */
	protected AdListener createInterstitialListener() {
		return new AdListener() {

			@Override
			public void noAdFound() {
				UnityPlayer.UnitySendMessage("MobFox", "onInterstitialFailed",
						"");
			}

			@Override
			public void adShown(Ad arg0, boolean arg1) {
				UnityPlayer.UnitySendMessage("MobFox", "onInterstitialShown",
						"");
			}

			@Override
			public void adLoadSucceeded(Ad arg0) {
				UnityPlayer.UnitySendMessage("MobFox", "onInterstitialLoaded",
						"");
			}

			@Override
			public void adClosed(Ad arg0, boolean arg1) {
				UnityPlayer.UnitySendMessage("MobFox",
						"onInterstitialDismissed", "");
			}

			@Override
			public void adClicked() {
				UnityPlayer.UnitySendMessage("MobFox", "onInterstitialClicked",
						"");

			}
		};
	}

	private Activity getActivity() {
		return UnityPlayer.currentActivity;
	}

	private float getScreenDensity() {
		DisplayMetrics metrics = new DisplayMetrics();
		getActivity().getWindowManager().getDefaultDisplay()
				.getMetrics(metrics);

		return metrics.density;
	}

	@SuppressLint("RtlHardcoded")
	private void prepLayout(int alignment) {
		// create a RelativeLayout and add the ad view to it
		if (mLayout == null) {
			mLayout = new RelativeLayout(getActivity());
		} else {
			// remove the layout if it has a parent
			FrameLayout parentView = (FrameLayout) mLayout.getParent();
			if (parentView != null)
				parentView.removeView(mLayout);
		}

		int gravity = 0;

		switch (alignment) {
		case 0:
			gravity = Gravity.TOP | Gravity.LEFT;
			break;
		case 1:
			gravity = Gravity.TOP | Gravity.CENTER_HORIZONTAL;
			break;
		case 2:
			gravity = Gravity.TOP | Gravity.RIGHT;
			break;
		case 3:
			gravity = Gravity.CENTER_VERTICAL | Gravity.CENTER_HORIZONTAL;
			break;
		case 4:
			gravity = Gravity.BOTTOM | Gravity.LEFT;
			break;
		case 5:
			gravity = Gravity.BOTTOM | Gravity.CENTER_HORIZONTAL;
			break;
		case 6:
			gravity = Gravity.BOTTOM | Gravity.RIGHT;
			break;
		}

		mLayout.setGravity(gravity);
	}

}
