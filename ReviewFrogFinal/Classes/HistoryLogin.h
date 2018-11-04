//
//  HistoryLogin.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 26/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"

@interface HistoryLogin : UIViewController<MBProgressHUDDelegate,UIScrollViewDelegate,UITextFieldDelegate> {
	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UIScrollView *scrView;
	IBOutlet UIView *viewLogin;

	
	IBOutlet UITextField *txtEmail;
	IBOutlet UITextField *txtPassword;
	
		
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgtxtPowerdby;
	IBOutlet UIImageView *imgGiveAway;
	
	UIAlertView *alertemail;
	UIAlertView *aleremailtest;
	UIAlertView *alertpassword;
	MBProgressHUD *HUD;

	IBOutlet UIButton *btnadmin;
}

@property(nonatomic, retain) IBOutlet UITextField *txtEmail;
@property(nonatomic, retain) IBOutlet UITextField *txtPassword;


-(IBAction)Home;
-(IBAction) TERMS_CONDITIONS;
//-(IBAction) Admin;
-(IBAction)LoginSuccess;
-(void)startAdminLogin;
-(void)getAdminLogin;
-(IBAction) UPDATEbtn;
@end
