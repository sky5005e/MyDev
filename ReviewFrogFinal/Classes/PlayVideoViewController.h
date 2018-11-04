//
//  PlayVideoViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 6/23/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "Video.h"
#import "MBProgressHUD.h"
#import <MediaPlayer/MediaPlayer.h>

@interface PlayVideoViewController : UIViewController<UIWebViewDelegate,UIImagePickerControllerDelegate,UIVideoEditorControllerDelegate,MBProgressHUDDelegate> {

	
	Review_FrogAppDelegate *appDelegate;
	Video *avideo;
	MBProgressHUD *HUD;
	IBOutlet UIScrollView *scrView;
	IBOutlet UIView *viewVideo;

	
	NSURLRequest *urlReq;
	IBOutlet UIImageView *imgRecord;
	IBOutlet UIImageView *imgSubmit;
	IBOutlet UILabel *lblTitle;
	
	IBOutlet UIButton *btnRecord;
	IBOutlet UIButton *btnSubmit;
	
	IBOutlet UIWebView *webVideoplay;
	
	
	MPMoviePlayerController *moviePlayer;
	BOOL isUploaded;
	IBOutlet UIImageView *imgBLogo;
	
	
	IBOutlet UILabel *lblQuesionOne;
	IBOutlet UILabel *lblQuesionTwo;
	IBOutlet UILabel *lblQuesionThree;
	
	IBOutlet UILabel *lblAnswerOne;
	IBOutlet UILabel *lblAnswerTwo;
	IBOutlet UILabel *lblAnswerThree;
	BOOL flgStopReloading;
	IBOutlet UIButton *btnadmin;
	
}
//@property (nonatomic, retain) NSData *DataVideo;
@property BOOL flgStopReloading;
@property (nonatomic, retain) Video *avideo;

-(IBAction) Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Back_Click;
-(IBAction) ReRecord_click;
-(IBAction) SubmitVideo_Click;

@end
