//
//  PlayBusinessVideoViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"
#import "Video.h"
#import <MediaPlayer/MediaPlayer.h>
@interface PlayBusinessVideoViewController : UIViewController <UIWebViewDelegate,MBProgressHUDDelegate>{
	Review_FrogAppDelegate *appDelegate;
	
	Video *avideo;
	MBProgressHUD *HUD;
	
	IBOutlet UIImageView *imgRecord;
	IBOutlet UILabel *lblsubmit;
	IBOutlet UILabel *lblTitle;
	
	IBOutlet UIButton *btnRecord;
	IBOutlet UIButton *btnSubmit;
	
	IBOutlet UIWebView *webVideoplay;
	MPMoviePlayerController *moviePlayer;
	
	IBOutlet UIActivityIndicatorView *Activeindicator;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIButton *btnadmin;
}
//@property (nonatomic, retain) NSData *DataVideo;
@property (nonatomic, retain) Video *avideo;
@property (nonatomic, retain) IBOutlet UIActivityIndicatorView *Activeindicator;

-(IBAction) Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Back_Click;
-(IBAction) ReRecord_click;
-(IBAction) SubmitVideo_Click;

@end
