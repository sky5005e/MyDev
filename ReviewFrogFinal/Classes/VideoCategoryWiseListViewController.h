//
//  VideoCategoryWiseListViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 9/2/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"
#import "CategoryWiseList.h"

@interface VideoCategoryWiseListViewController : UIViewController<UITableViewDataSource,UITableViewDelegate,MBProgressHUDDelegate> {

	Review_FrogAppDelegate *appDelegate;
	IBOutlet UITableView *tblCategoryList;
	CategoryWiseList *objcategoryWiseList;
	MBProgressHUD *HUD;
	
	UIButton *btnPosted;
	UIButton *btnNotPosted;
	UIButton *btnInternal;
	IBOutlet UIImageView *imgBLogo;
	
	UIButton *btnfiveStar;
	UIButton *btnFourStar;
	UIButton *btnThreeStar;
	UIButton *btnTwoStar;
	UIButton *btnOneStar;
	
	int intone; 
	int inttwo; 
	int intthree;
	int intfour; 
	int intfive; 
	IBOutlet UIButton *btnadmin;
}
@property (nonatomic, retain) UITableView *tblCategoryList;
@property (nonatomic, retain) UIButton *btnPosted;
@property (nonatomic, retain) UIButton *btnNotPosted;
@property (nonatomic, retain) UIButton *btnInternal;

-(void) LoadingStart;
-(void) GetCategoryList;
-(void)LoadTableData;
-(IBAction)BackClick;
-(IBAction)Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) AddCategoryClick;
@end
