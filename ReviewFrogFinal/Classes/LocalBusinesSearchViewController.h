//
//  LocalBusinesSearchViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 7/25/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "SearchCityViewController.h"


@interface LocalBusinesSearchViewController : UIViewController <OptionsViewControllerDelegate,UITextFieldDelegate,UIPickerViewDelegate,UIPickerViewDataSource,UIScrollViewDelegate,MBProgressHUDDelegate,UIAlertViewDelegate>{
	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UITextField *txtCity;
	IBOutlet UITextField *txtCategory;
	IBOutlet UIPickerView *picCategory;
	IBOutlet UIToolbar *tlbCategory;
	UIAlertView *alertcity;
	UIAlertView *alertcategory;
	IBOutlet UIScrollView *scrView;
	IBOutlet UIView *ViewSearch;
	BOOL flgdidSelectRow;
	UIPopoverController *popView;
	MBProgressHUD *HUD;
	
}
@property (nonatomic, retain) IBOutlet UIActivityIndicatorView *Activeindicator;
-(IBAction) BackClick;
-(void)didTap;
-(void)CategoryList;
-(IBAction)btnDoneClick;
-(IBAction)btnSearchClick;

@end
