//
//  WriteORRecodeViewController.h
//  Review Frog
//
//  Created by agilepc-32 on 10/4/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "XMLBusinessProfile.h"
#import "MBProgressHUD.h"
@class Review_FrogAppDelegate;
@interface WriteORRecodeViewController : UIViewController<MBProgressHUDDelegate> {
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UIButton *btnWriteReview;
	IBOutlet UIButton *btnRecordReview;	
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgtxtPowerdby;
	IBOutlet UIImageView *imgGiveAway;
	IBOutlet UILabel *lblwithwin;
	IBOutlet UILabel *lblwithoutwin;
	IBOutlet UIButton *Home;
	IBOutlet UIButton *btnadmin;
    UIPopoverController *popSweepStakes;
    MBProgressHUD *HUD;
    NSString *dateStore;
    
    IBOutlet UIButton *btnsweepstake;
}
@property (nonatomic, retain) IBOutlet UIImageView *imgBLogo;
@property (nonatomic, retain) NSString *dateStore;
-(IBAction) btnWriteReviewClick;
-(IBAction) btnRecordReviewClick;
-(IBAction) Admin;
-(IBAction)btnsweepstakeClcik;
-(void)load;
-(void)checkResponse;
-(void)Continue;
@end
