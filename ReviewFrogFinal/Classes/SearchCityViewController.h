//
//  SearchCityViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 5/26/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol OptionsViewControllerDelegate

-(void)didTap;

@end

#import "Review_FrogAppDelegate.h"
#import "ApplicationValidation.h"
#import "Cityname.h"
#import "XMLCityname.h"
#import "MBProgressHUD.h"


@interface SearchCityViewController : UIViewController<UISearchBarDelegate,UISearchDisplayDelegate,UITableViewDelegate,UITableViewDataSource,MBProgressHUDDelegate> {
	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UISearchBar *SearchCityBar;
	UISearchDisplayController *searchDC;
	NSMutableArray *filteredArray;
	
	MBProgressHUD *HUD;
	
	IBOutlet UITableView *tblCityList;
	IBOutlet UIButton *btnA;
	UIButton *btnprvious;
	IBOutlet UIButton *btnB;
	id<OptionsViewControllerDelegate> delegate;

}
@property (nonatomic, retain) IBOutlet UITableView *tblCityList;
@property (nonatomic, retain) IBOutlet UIButton *btnA;
@property (nonatomic, retain) IBOutlet UIButton *btnB;

@property (nonatomic, retain) IBOutlet UISearchBar *SearchCityBar;
@property (nonatomic, retain) UISearchDisplayController *searchDC;
@property (nonatomic, retain) NSMutableArray *filteredArray;
@property (nonatomic, assign) id<OptionsViewControllerDelegate> delegate;

-(IBAction)Click_button:(id)sender;
- (void) doLoadShruffleList;
@end
