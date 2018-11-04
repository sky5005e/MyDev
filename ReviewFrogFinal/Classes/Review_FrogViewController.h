//
//  ApplicationValidation.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 02/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "MBProgressHUD.h"
#import "LoginInfo.h"
//#import "Review_FrogAppDelegate.h"

@class Review_FrogAppDelegate;


@interface Review_FrogViewController : UIViewController<MBProgressHUDDelegate> {
	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UITextField *txtUserEmail;
	IBOutlet UITextField *txtUSerPassword;
	MBProgressHUD *HUD;
    LoginInfo *alogin;
}
@property (nonatomic, retain) IBOutlet UITextField *txtUserEmail;
@property (nonatomic, retain) IBOutlet UITextField *txtUSerPassword;
-(IBAction)LoginClick;
-(void)getApplicationValidation;
-(IBAction) BackClick;

@end
