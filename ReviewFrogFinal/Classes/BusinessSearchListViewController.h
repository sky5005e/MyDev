//
//  BusinessSearchListViewController.h
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"

@interface BusinessSearchListViewController : UIViewController<UITableViewDataSource,UITableViewDelegate,MBProgressHUDDelegate,UIPickerViewDelegate,UIPickerViewDataSource,UITextFieldDelegate> {

	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UITableView *tblSearch;
	NSMutableArray *selectedArrSearch;
	MBProgressHUD *HUD;
	int businessid;
	
	IBOutlet UIPickerView *picCategory;
	IBOutlet UIToolbar *tlbCategory;
	BOOL flgdidSelectRow;
	UIButton *btnReview;
	UIButton *ReviewClick;
		
	UILabel *lblCategory;
	UILabel *lblCity;
	IBOutlet UILabel *lblNoRecord;
	IBOutlet UILabel *lblResultNot;
		
	IBOutlet UITextField *txtCategory;
	IBOutlet UITextField *txtCity;
	NSString *strCategory;
	NSString *strCity;
}
@property int businessid;
@property (nonatomic, retain) NSString *strCategory;
@property (nonatomic, retain) NSString *strCity;
@property (nonatomic, retain) IBOutlet UITableView *tblSearch;
@property (nonatomic, retain) NSMutableArray *selectedArrSearch;

-(void)SearchResult :(int)index;
-(IBAction)BackClcik;
-(IBAction)btnSearchClick;
-(IBAction)btnDoneClick;
@end
