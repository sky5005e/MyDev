//
//  CategoryViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 8/12/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "Category.h"
#import "MBProgressHUD.h"
#import "VideoCategoryWiseListViewController.h"
#import "ReviewCategoryWiseListViewController.h"
#import "DefaultCategoryViewController.h"

@interface CategoryViewController : UIViewController <UITableViewDelegate, UITableViewDataSource,UIAlertViewDelegate, MBProgressHUDDelegate, UITextFieldDelegate, DefaultCategorydelegate>{
	
	Review_FrogAppDelegate *appDelegate;
	MBProgressHUD *HUD;
	Category *objcategory;
	BOOL flag_Category;
    
	UIPopoverController *popView;
	
	IBOutlet UITableView *tblCategory;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UITextField *txtCategory;
	IBOutlet UITextField *txtDefaultCategory;
	IBOutlet UIImageView *imgGiveAway;
	
	UIButton *btnDelete;
	UIButton *btnUpdate;
	
	UIAlertView *alertConfirm;
	UIAlertView *AlertUpdate;
	UIAlertView *AddAlert;
	
	int cid;
	NSString *cname;
	IBOutlet UIButton *btnDefaultSelection;
	IBOutlet UIButton *btnadmin;
}
@property BOOL flag_Category;
@property (nonatomic, retain) IBOutlet UITableView *tblCategory;
-(IBAction) BackClick;
-(IBAction) AddCategory;
-(IBAction) Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) DefaultCategoryClick;

-(void) DeleteDone;
-(void) UPdateDone;
-(void) StartCategoryLoging;
-(void) GetCategoryList;
-(void) StartAddingCategory;
@end
