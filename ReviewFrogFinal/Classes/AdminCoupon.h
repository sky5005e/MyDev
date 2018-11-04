//
//  AdminCoupon.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 27/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogViewController.h"
#import "MBProgressHUD.h"


@interface AdminCoupon : UIViewController<UITextViewDelegate,UIPickerViewDataSource,UIPickerViewDelegate,UIScrollViewDelegate, MBProgressHUDDelegate> {
	
	Review_FrogAppDelegate *appDelegate;
	MBProgressHUD *HUD;
	
	IBOutlet UIButton *btnsumhidde;
	IBOutlet UIButton *btnCity;
	IBOutlet UIButton *btncontinue3;
	
	IBOutlet UIScrollView *scrlView;
	IBOutlet UIView *ViewCoupon;
	
	IBOutlet UITextView *txtCoupon;
	IBOutlet UITextField *txtBusinessname;
	IBOutlet UITextField *txtBusinessphone;
	IBOutlet UITextView *txtBusinessAddress;
	IBOutlet UITextField *txtCouponeoffers;
	IBOutlet UITextField *txtCouponeEdate;
	IBOutlet UIPickerView *pickercity;
	IBOutlet UITextView *txtAllcity;
	
	IBOutlet UILabel *lblAdminCityCounter;
	
	IBOutlet UISearchBar *searchA;
	
	
	IBOutlet UIToolbar *tlBar;
	IBOutlet UIBarButtonItem *btnAdd;
	BOOL FlgExit;
	BOOL fistCity;
	
	BOOL fistCity2;
	BOOL pickerflg1;
	
	
	
	NSMutableArray *Allcity;
	NSString *selectedcity;
	
	
	NSString *Businessname;
	NSString *Description;
	NSString *CitysName;
	NSString *peopleCounter;
	NSString *Businessphone;
	NSString *BusinessAddress;
	NSString *AEmail;
	NSString *Couponeoffers;
	NSString *CouponeEdate;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgtxtPowerdby;	
	
}

@property (nonatomic, retain) IBOutlet UIButton *btnsumhidde;
@property (nonatomic, retain) IBOutlet UITextView *txtCoupon;
@property (nonatomic, retain) NSString *Businessname;
@property (nonatomic, retain) NSString *Description;
@property (nonatomic, retain) NSString *CitysName;
@property (nonatomic, retain) NSString *peopleCounter;
@property (nonatomic, retain) NSString *Businessphone;
@property (nonatomic, retain) NSString *BusinessAddress;
@property (nonatomic, retain) NSString *AEmail;
@property (nonatomic, retain) NSString *Couponeoffers;
@property (nonatomic, retain) NSString *CouponeEdate;
@property (nonatomic, retain) IBOutlet UIButton *btnCity;
@property (nonatomic, retain) IBOutlet UIPickerView *pickercity;
@property (nonatomic, retain) IBOutlet UITextView *txtAllcity;
@property (nonatomic, retain) NSMutableArray *Allcity;
@property (nonatomic, retain) IBOutlet UILabel *lblAdminCityCounter;
@property (nonatomic, retain) IBOutlet UITextField *txtBusinessname;
@property (nonatomic, retain) IBOutlet UITextField *txtBusinessphone;
@property (nonatomic, retain) IBOutlet UITextView *txtBusinessAddress;
@property (nonatomic, retain) IBOutlet UIView *ViewCoupon;
@property (nonatomic, retain) IBOutlet UIButton *btncontinue3;
@property (nonatomic, retain) IBOutlet UISearchBar *searchA;
@property BOOL FlgExit;
@property BOOL fistCity;
@property BOOL fistCity2;
@property BOOL pickerflg1;


-(IBAction) Home;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(IBAction) SubmitCoupon;
-(IBAction) btnCity_clicked;
-(IBAction) btnAdd_clicked;
-(void) GetAllCity;
-(void) GetAdminCity;
-(void) refreshcity;
-(void) AdminCityCounter;
-(IBAction) LogOut;


@end
