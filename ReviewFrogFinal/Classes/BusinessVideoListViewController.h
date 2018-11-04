//
//  BusinessVideoListViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "Video.h"
#import "MBProgressHUD.h"

@interface BusinessVideoListViewController : UIViewController <UITableViewDataSource,UITableViewDelegate,UIAlertViewDelegate,UISearchDisplayDelegate,UISearchBarDelegate, MBProgressHUDDelegate,UIImagePickerControllerDelegate>{
	
	Review_FrogAppDelegate *appDelegate;
	MBProgressHUD *HUD;
	IBOutlet UITableView *tblBusinessList;
	IBOutlet UISearchBar *SearchCityBar;
	UISearchDisplayController *searchDC;
	
	NSMutableArray *filteredArray;

	UIAlertView *alertConfirm;
	
	UIButton *btnDelete;
	Video *objVideo;
	BOOL flgBusinessVideo;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgGiveAway;
	IBOutlet UIButton *btnadmin;
	
}
@property BOOL flgBusinessVideo;
@property (nonatomic, retain) IBOutlet UITableView *tblBusinessList;
@property (nonatomic, retain) Video *objVideo;
@property (nonatomic, retain) IBOutlet UISearchBar *SearchCityBar;
@property (nonatomic, retain) UISearchDisplayController *searchDC;
@property (nonatomic, retain) NSMutableArray *filteredArray;

-(void)GetBuseinessVideoList;
-(void) MoveToWebView;
-(void) StartLoding;

-(IBAction) Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Back_Click;
-(IBAction) BusinessVideoRecodingClick;

@end
