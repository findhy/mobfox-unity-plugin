#import <Foundation/Foundation.h>
#import <MobFox/MobFox.h>

@class MobFoxBinding;

typedef enum
{
	MobFoxAdPositionTopLeft,
	MobFoxAdPositionTopCenter,
	MobFoxAdPositionTopRight,
	MobFoxAdPositionCentered,
	MobFoxAdPositionBottomLeft,
	MobFoxAdPositionBottomCenter,
	MobFoxAdPositionBottomRight
} MobFoxAdPosition;

MobFoxBannerView *bannerView;
MobFoxAdPosition bannerPosition;

MobFoxBinding *myInstance;

@interface MobFoxBinding : NSObject <MobFoxVideoInterstitialViewControllerDelegate, MobFoxBannerViewDelegate>
{
}
@property (nonatomic, strong) NSString *idForBanners;
@property (nonatomic, strong) NSString *idForInterstitials;
@property (nonatomic) MobFoxAdType loadedInterstitialType;
@property (nonatomic, strong) MobFoxVideoInterstitialViewController *videoInterstitialViewController;

extern "C" void MobFoxCreateBanner(const char*  adUnitId, MobFoxAdPosition alignment, int width, int height );
extern "C" void MobFoxSetBannerVisibility( bool visible );
extern "C" void MobFoxDestroyBanner();
extern "C" void MobFoxRequestInterstitalAd( const char*  adUnitId );
extern "C" void MobFoxShowInterstitalAd();

@end
