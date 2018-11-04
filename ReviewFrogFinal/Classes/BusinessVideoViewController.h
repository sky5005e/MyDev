//
//  BusinessVideoViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import <MediaPlayer/MediaPlayer.h>
#import "BusinessVideoListViewController.h"
@interface BusinessVideoViewController : UIViewController<UIWebViewDelegate> {

	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UIImageView *imgRecord;
	IBOutlet UIImageView *imgSubmit;
	
	IBOutlet UIButton *btnRecord;
	IBOutlet UIButton *btnSubmit;
	
	IBOutlet UILabel *lblTitle;
	
	IBOutlet UITextField *txtTitle;
	MPMoviePlayerController *moviePlayer;
	IBOutlet UIWebView *webVideoplay;	
	IBOutlet UIImageView *imgBLogo;
}
-(IBAction)Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Back_Click;
-(IBAction) ReRecord_Clcik;
-(IBAction) SubmitVideo_Click;
-(void) SaveVideoinfo;
-(void) SubmitToDocDir;

@end
