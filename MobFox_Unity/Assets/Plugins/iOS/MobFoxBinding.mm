#import "MobFoxBinding.h"

extern UIViewController* UnityGetGLViewController();
extern void UnitySendMessage(const char*, const char*, const char*);

@interface MobFoxBinding() {
    BOOL interstitialClickWasReported;
}

@end

@implementation MobFoxBinding

-(id)init
{
    self = [super init];
    myInstance = self;
    return self;
}

- (void)adjustAdViewFrameToShowAdView
{
	// fetch screen dimensions and useful values
	CGRect origFrame = bannerView.frame;
	
	CGFloat screenHeight = [UIScreen mainScreen].bounds.size.height;
	CGFloat screenWidth = [UIScreen mainScreen].bounds.size.width;
	
	if( (UIInterfaceOrientationIsLandscape( UnityGetGLViewController().interfaceOrientation)
         && [[[UIDevice currentDevice] systemVersion] compare:@"8" options:NSNumericSearch] == NSOrderedAscending) ) //iOS version lower than 8
	{
		screenWidth = screenHeight;
		screenHeight = [UIScreen mainScreen].bounds.size.width;
	}
	
	switch( bannerPosition )
	{
		case MobFoxAdPositionTopLeft:
			origFrame.origin.x = 0;
			origFrame.origin.y = 0;
			bannerView.autoresizingMask = ( UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case MobFoxAdPositionTopCenter:
			origFrame.origin.x = ( screenWidth / 2 ) - ( origFrame.size.width / 2 );
			origFrame.origin.y = 0;
			bannerView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case MobFoxAdPositionTopRight:
			origFrame.origin.x = screenWidth - origFrame.size.width;
			origFrame.origin.y = 0;
			bannerView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case MobFoxAdPositionCentered:
			origFrame.origin.x = ( screenWidth / 2 ) - ( origFrame.size.width / 2 );
			origFrame.origin.y = ( screenHeight / 2 ) - ( origFrame.size.height / 2 );
			bannerView.autoresizingMask = ( UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case MobFoxAdPositionBottomLeft:
			origFrame.origin.x = 0;
			origFrame.origin.y = screenHeight - origFrame.size.height;
			bannerView.autoresizingMask = ( UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin );
			break;
		case MobFoxAdPositionBottomCenter:
			origFrame.origin.x = ( screenWidth / 2 ) - ( origFrame.size.width / 2 );
			origFrame.origin.y = screenHeight - origFrame.size.height;
			bannerView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin );
			break;
		case MobFoxAdPositionBottomRight:
			origFrame.origin.x = screenWidth - bannerView.frame.size.width;
			origFrame.origin.y = screenHeight - origFrame.size.height;
			bannerView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin );
			break;
	}
	
	bannerView.frame = origFrame;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

void MobFoxCreateBanner(const char*  adUnitId, MobFoxAdPosition alignment, int width, int height ) {
    if(!myInstance) {
        [[MobFoxBinding alloc] init];
    }
    
    if( bannerView ) {
		MobFoxDestroyBanner();
    }
    myInstance.idForBanners = [NSString stringWithUTF8String:adUnitId];
    bannerPosition = alignment;
	
	bannerView = [[MobFoxBannerView alloc] initWithFrame:CGRectZero];
    bannerView.allowDelegateAssigmentToRequestAd = NO;
	bannerView.requestURL = @"http://my.mobfox.com/request.php";

	bannerView.adspaceWidth = width;
	bannerView.adspaceHeight = height;
	bannerView.delegate = myInstance;
	[UnityGetGLViewController().view addSubview:bannerView];
    [bannerView requestAd];
}


void MobFoxDestroyBanner()
{
    if(!bannerView) {
        return;
    }
    
	[bannerView removeFromSuperview];
	bannerView.delegate = nil;
	bannerView = nil;
}

void MobFoxSetBannerVisibility( bool visible ){
	if( !bannerView )
		return;

	bannerView.hidden = !visible;
}


void MobFoxRequestInterstitalAd( const char*  adUnitId )
{
    if(!myInstance) {
        [[MobFoxBinding alloc] init];
    }
    
    myInstance.idForInterstitials = [NSString stringWithUTF8String:adUnitId];
	if(!myInstance.videoInterstitialViewController) {
		myInstance.videoInterstitialViewController = [[MobFoxVideoInterstitialViewController alloc] init];
        myInstance.videoInterstitialViewController.enableVideoAds = YES;
        myInstance.videoInterstitialViewController.prioritizeVideoAds = YES;
 		myInstance.videoInterstitialViewController.delegate = myInstance;
	    [UnityGetGLViewController().view  addSubview:myInstance.videoInterstitialViewController.view];
	    myInstance.videoInterstitialViewController.requestURL = @"http://my.mobfox.com/request.php";
	}
	
	[myInstance.videoInterstitialViewController requestAd];
}


void MobFoxShowInterstitalAd()
{
	if( !myInstance.videoInterstitialViewController)
	{
		return;
	}
	
	[myInstance.videoInterstitialViewController presentAd:myInstance.loadedInterstitialType];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Banner delegate

- (NSString *)publisherIdForMobFoxBannerView:(MobFoxBannerView *)banner {
    return myInstance.idForBanners;
}


- (void)mobfoxBannerViewDidLoadMobFoxAd:(MobFoxBannerView *)banner {
    [self adjustAdViewFrameToShowAdView];
    UnitySendMessage( "MobFox", "onBannerAdLoaded", "" );
}

- (void)mobfoxBannerViewDidLoadRefreshedAd:(MobFoxBannerView *)banner {
    [self adjustAdViewFrameToShowAdView];
    UnitySendMessage( "MobFox", "onBannerAdLoaded", "" );
}

- (void)mobfoxBannerView:(MobFoxBannerView *)banner didFailToReceiveAdWithError:(NSError *)error {
    UnitySendMessage( "MobFox", "onBannerAdFailed", "" );
}

- (BOOL)mobfoxBannerViewActionShouldBegin:(MobFoxBannerView *)banner willLeaveApplication:(BOOL)willLeave {
    UnitySendMessage( "MobFox", "onBannerAdClicked", "" );
    return YES;
}

- (void)mobfoxBannerViewActionWillPresent:(MobFoxBannerView *)banner {
    UnitySendMessage( "MobFox", "onBannerAdShown", "" );
}

-(void)mobfoxBannerViewActionWillLeaveApplication:(MobFoxBannerView *)banner {
    UnitySendMessage( "MobFox", "onBannerAdClickWillLeaveApp", "" );
}

- (void)mobfoxBannerViewActionDidFinish:(MobFoxBannerView *)banner {
    UnitySendMessage( "MobFox", "onBannerAdClosed", "" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Interstitial delegate

- (NSString *)publisherIdForMobFoxVideoInterstitialView:(MobFoxVideoInterstitialViewController *)videoInterstitial {
    return myInstance.idForInterstitials;
}

- (void)mobfoxVideoInterstitialViewDidLoadMobFoxAd:(MobFoxVideoInterstitialViewController *)videoInterstitial advertTypeLoaded:(MobFoxAdType)advertType {
    myInstance.loadedInterstitialType = advertType;
    interstitialClickWasReported = NO;
    UnitySendMessage( "MobFox", "onInterstitialLoaded", "" );
}

- (void)mobfoxVideoInterstitialView:(MobFoxVideoInterstitialViewController *)videoInterstitial didFailToReceiveAdWithError:(NSError *)error {
    UnitySendMessage( "MobFox", "onInterstitialFailed", "" );
}

- (void)mobfoxVideoInterstitialViewActionWillPresentScreen:(MobFoxVideoInterstitialViewController *)videoInterstitial {
    UnitySendMessage( "MobFox", "onInterstitialShown", "" );
}

- (void)mobfoxVideoInterstitialViewDidDismissScreen:(MobFoxVideoInterstitialViewController *)videoInterstitial {
    UnitySendMessage( "MobFox", "onInterstitialDismissed", "" );
}

-(void)mobfoxVideoInterstitialViewWasClicked:(MobFoxVideoInterstitialViewController *)videoInterstitial {
    if(!interstitialClickWasReported) {
        UnitySendMessage( "MobFox", "onInterstitialClicked", "" );
    }
    interstitialClickWasReported = YES;
}

-(void)mobfoxVideoInterstitialViewActionWillLeaveApplication:(MobFoxVideoInterstitialViewController *)videoInterstitial {
    if(!interstitialClickWasReported) {
        UnitySendMessage( "MobFox", "onInterstitialClicked", "" );
    }
    interstitialClickWasReported = YES;
}


@end
