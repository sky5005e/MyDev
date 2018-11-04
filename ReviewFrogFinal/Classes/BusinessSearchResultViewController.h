//
//  BusinessSearchResultViewController.h
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"

@interface BusinessSearchResultViewController : UIViewController<UITableViewDelegate,UITableViewDataSource,MBProgressHUDDelegate>{

	Review_FrogAppDelegate *appDelegate;
	IBOutlet UITableView *tblResult;	
	
	MBProgressHUD *HUD;
	
	IBOutlet UIPickerView *picCategory;
	IBOutlet UIToolbar *tlbCategory;
	BOOL flgdidSelectRow;
	
	IBOutlet UITextField *txtCategory;
	IBOutlet UITextField *txtCity;
	NSString *strCategory;
	NSString *strCity;

	
	
}
@property (nonatomic, retain) NSString *strCategory;
@property (nonatomic, retain) NSString *strCity;
@property (nonatomic,retain) UITableView *tblResult;

-(IBAction)BackClcik;
-(IBAction)btnDoneClick;
-(IBAction)btnSearchResultClcik;
@end
