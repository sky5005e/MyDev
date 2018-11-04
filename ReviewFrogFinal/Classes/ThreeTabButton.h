//
//  ThreeTabButton.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 05/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"


@interface ThreeTabButton : UIViewController {
	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UIButton *btnsugestion;
	MBProgressHUD *HUD;
	BOOL flgBusinessVideo;
	IBOutlet UIButton *btnAdmin;
}
@property (nonatomic, retain) IBOutlet UIButton *btnsugestion;
@property BOOL flgBusinessVideo;

-(IBAction) Home;
-(IBAction) History;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(IBAction) Back_click;

//-(IBAction) BusinessRecording_Click;
//-(IBAction) BusinessVideo_Click;
//-(IBAction) AdminCoupon;
//-(IBAction) Sound;
//-(IBAction) LogOut;


@end

