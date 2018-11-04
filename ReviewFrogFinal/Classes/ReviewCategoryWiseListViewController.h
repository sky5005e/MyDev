//
//  ReviewCategoryWiseListViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 8/29/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"
#import "CategoryWiseList.h"

@interface ReviewCategoryWiseListViewController : UIViewController <UITableViewDataSource,UITableViewDelegate,MBProgressHUDDelegate> {

	Review_FrogAppDelegate *appDelegate;
	IBOutlet UITableView *tblCategoryList;
	CategoryWiseList *objcategoryWiseList;
	MBProgressHUD *HUD;

	UIButton *btnfiveStar;
	UIButton *btnFourStar;
	UIButton *btnThreeStar;
	UIButton *btnTwoStar;
	UIButton *btnOneStar;
	IBOutlet UIImageView *imgBLogo;
	int fiveStar;
	IBOutlet UIButton *btnadmin;
	
	
}
@property (nonatomic, retain) UITableView *tblCategoryList;
@property (nonatomic, retain) UIButton *btnfiveStar;
@property (nonatomic, retain) UIButton *btnFourStar;
@property (nonatomic, retain) UIButton *btnThreeStar;
@property (nonatomic, retain) UIButton *btnTwoStar;
@property (nonatomic, retain) UIButton *btnOneStar;



-(void) LoadingStart;
-(void) GetCategoryList;
-(void) LoadTableData;
-(void) Showhistorylist;
-(IBAction) BackClick;
-(IBAction) Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) AddCategoryClick;

@end

