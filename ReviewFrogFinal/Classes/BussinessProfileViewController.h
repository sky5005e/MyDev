//
//  BussinessProfileViewController.h
//  reviewfrogbussiness
//
//  Created by AgileMac11 on 10/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "LoginInfo.h"
#import "MBProgressHUD.h"
#import "LoginInfo.h"

@interface BussinessProfileViewController : UIViewController <UIScrollViewDelegate,UIImagePickerControllerDelegate,UITextFieldDelegate,MBProgressHUDDelegate,UIActionSheetDelegate>{
	
	Review_FrogAppDelegate *appDelegate;
	LoginInfo *alogin;
	MBProgressHUD *HUD;
	
	UIImagePickerController *imgPck;
	
	IBOutlet UIImageView *logoImgvw,*splashImgvw, *imgGiveaway, *imgBLogo;
	UIPopoverController *popoverController;
	
	IBOutlet UIButton *logosaveBtn,*splashsaveBtn,*logoremoveBtn,*splashremoveBtn,*update,*logopreview,*splashpreview, *btngiveawaysave, *btngiveawayremove;
	
	IBOutlet UIScrollView *scrollVw;
	IBOutlet UIView *viewBusinessProfile;

	
	int logoflag,que1cnt,que2cnt,que3cnt,continueflag,chklogoflag,chklogoflag1,flag1,flag2,flag3;
	
	UIAlertView *msg1,*msg2,*msg3,*msg4;
	
	IBOutlet UITextField *que1Txt,*que2Txt,*que3Txt;
	
	IBOutlet UILabel *remque1Txt,*remque2Txt,*remque3Txt;
	
	UIView *preVw;
	UIButton *closeButton;

	NSData *data1,*data2;
	NSString *temoqu1;
	NSString *temoqu2;
	NSString *temoqu3;
	NSString *imgname1,*imgname2,*imgname3;

	LoginInfo *objlogin;
	BOOL flgimgGive;
	IBOutlet UISwitch *SwhBusiness;
	IBOutlet UIButton *btnadmin;
	
}

-(void)closeButtonClicked;
-(IBAction)logoSave;
-(IBAction)logoRemove;
-(IBAction)splashSave;
-(IBAction)splashRemove;
-(IBAction)GiveawaySave;
-(IBAction)GiveawayRemove;

-(IBAction)textFieldDidUpdate:(id)sender;
-(IBAction)update;
-(IBAction)preview:(id)sender;
-(void)updateComplete;
-(BOOL)apiBusinessprofile :(NSString *)userid question1:(NSString *) questionone question2:(NSString *) questiontwo question3:(NSString *) questionthree img:(UIImage *)image imgnmae:(NSString *)imagename imgtype:(NSString *)imagetype;
-(BOOL)apiBusinessprofile : (NSString *)businessid question1:(NSString *) questionone question2:(NSString *) questiontwo question3:(NSString *) questionthree imgtype:(NSString *)imagetype;

-(IBAction) BackClick;
-(void) getbusinessprofile;
-(void) loadbusinessprofile;
-(IBAction) Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) SwitchOnOffClick;
@end
