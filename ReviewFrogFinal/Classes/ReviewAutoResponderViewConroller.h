//
//  ReviewAutoResponderViewConroller.h
//  reviewfrogbussiness
//
//  Created by AgileMac11 on 10/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"
#import "LoginInfo.h"

@interface ReviewAutoResponderViewConroller : UIViewController <UITextFieldDelegate,UITextViewDelegate,UIScrollViewDelegate,MBProgressHUDDelegate>{
	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UIImageView *customizetxtImgvw;
	IBOutlet UITextView *responder1Txtvw,*responder2Txtvw;
	IBOutlet UIButton *responder1Btn,*responder2Btn;
	IBOutlet UITextField *subscribeTxt;
	IBOutlet UIButton *submit;
	NSString *apiurl;
	IBOutlet UIScrollView *scroll;
	IBOutlet UIView *viewAuto;
	MBProgressHUD *HUD;
	
	NSString *autoresponderonetext;
	NSString *autorespondertwotext;
	NSString *surveylink;
	LoginInfo *aloginingo;
	IBOutlet UIButton *btnadmin;
}
@property (nonatomic, retain) NSString *autoresponderonetext;
@property (nonatomic, retain) NSString *autorespondertwotext;
@property (nonatomic, retain) NSString *surveylink;

-(IBAction)	responder1;
-(IBAction)	responder2;
-(IBAction)	submit;
-(IBAction) BackClick;
-(void) GetAutoResponderinto;
-(void)startLodingAutoResponder;
-(void)loadcomplite;

-(IBAction)Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;

@end
