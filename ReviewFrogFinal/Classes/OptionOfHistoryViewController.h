//
//  OptionOfHistoryViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 6/27/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"


@interface OptionOfHistoryViewController : UIViewController {

	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UIButton *btnadmin;
	IBOutlet UIButton *btnwrite;
	IBOutlet UIButton *btnvideo;
	IBOutlet UIButton *btnbusiness;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgGiveAway;

}
-(IBAction) ViewTypeRecord_Click;
-(IBAction) ViewVideoRecord_Click;
-(IBAction) BusinessVideoListing_Click;
-(IBAction) BusinessProfileClick;
-(IBAction) LogOut;
-(IBAction) Home;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(IBAction) CategoryClick;
-(IBAction) SupportClcik;
//-(IBAction) SoundSwitchClick;
-(IBAction) AdminCouponClick;
-(IBAction) AutoResponderClick;
-(IBAction) WriteStaticstics;
-(IBAction) VideoStaticstics;
@end
