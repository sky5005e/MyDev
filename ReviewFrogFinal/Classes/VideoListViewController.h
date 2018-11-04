//
//  VideoListViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 6/22/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"
#import "Video.h"
@interface VideoListViewController : UIViewController <UITableViewDelegate,UITableViewDataSource,MBProgressHUDDelegate,UIAlertViewDelegate,UIPickerViewDelegate,UIPickerViewDataSource>{

	
	Review_FrogAppDelegate *appDelegate;
	Video *objVideo;
	MBProgressHUD *HUD;
	
	IBOutlet UITextField *checkOuttxt;
	IBOutlet UITextField *checkIntxt;
	IBOutlet UILabel *lblFirst;
	IBOutlet UILabel *lblFrom;
	

	NSString *strACount;
	NSString *strPCount;
	
	UIAlertView *alertConfirm;
	
	NSDate *SysDate1;
	NSDate *date;
	
	UIDatePicker *checkInPicker;
	UIDatePicker *checkOutPicker;
	
	BOOL pickeflag;
	BOOL pickeflag1;
	
	UIButton *btnDelete;
	
	NSString *SysDate2;
	NSString *Fromdate;
	NSString *Todate;
		
	IBOutlet UITableView *tblVideoList;
	
	NSMutableArray *ArrselectedVideoList;
	BOOL flgrate;
	IBOutlet UITextField *txtrating;
	IBOutlet UIButton *btnsearch;
	IBOutlet UIButton *btnStatistics;
	
	IBOutlet UIPickerView *picFilter;
	IBOutlet UIToolbar *tlbFilter;
	IBOutlet UIButton *btnFilter;
	
	int index;
	float strRating;

	BOOL flgPicfilte;
	BOOL flgactive;
	BOOL flgPending;
	
	NSMutableArray *ArrActive;
	NSMutableArray *ArrPending;
	
	NSString *stronestar;
	NSString *strtwostar;
	NSString *strthreestar;
	NSString *strfourstar;
	NSString *strfivestar;
	IBOutlet UIImageView *imgBLogo;
	
/*	int A;
	int I;
	int D ;
	int fivestar;
	int fourstar;
	int threestar;
	int twostar;
	int onestar;*/
	BOOL flgreloadstop;
	BOOL flgStopReloading;
	NSString *strCategory;
	NSString *strStar; 

	IBOutlet UIImageView *imgGiveAway;
	IBOutlet UIButton *btnadmin;
	IBOutlet UIButton *btnnext;
	IBOutlet UIButton *btnPrevious;
	int counter;
	BOOL flgDateSearch;
    BOOL flgNewSearch;
	
}

@property (nonatomic, retain) UITableView *tblVideoList;
@property (nonatomic, retain) Video *objVideo;

@property (nonatomic, retain) IBOutlet UITextField *checkOuttxt;
@property (nonatomic, retain) IBOutlet UITextField *checkIntxt;
@property (nonatomic, retain) NSDate *SysDate1;
@property (nonatomic, retain) NSDate *date;
@property (nonatomic, retain) UIDatePicker *checkInPicker;
@property (nonatomic, retain) UIDatePicker *checkOutPicker;

@property BOOL flgrate;
@property BOOL pickeflag;
@property BOOL pickeflag1;
@property BOOL flgStopReloading;
@property BOOL flgreloadstop;


@property (nonatomic, retain) NSString *SysDate2;
@property (nonatomic, retain) NSString *Fromdate;
@property (nonatomic, retain) NSString *Todate;
@property (nonatomic, retain) NSMutableArray *ArrselectedVideoList;

@property (nonatomic, retain) IBOutlet UITextField *txtrating;
@property (nonatomic, retain) NSString *strACount;
@property (nonatomic, retain) NSString *strPCount;

@property (nonatomic, retain) NSMutableArray *ArrActive;
@property (nonatomic, retain) NSMutableArray *ArrPending;

@property (nonatomic, retain) NSString *stronestar;
@property (nonatomic, retain) NSString *strtwostar;
@property (nonatomic, retain) NSString *strthreestar;
@property (nonatomic, retain) NSString *strfourstar;
@property (nonatomic, retain) NSString *strfivestar;
@property (nonatomic, retain) NSString *strCategory;
@property (nonatomic, retain) NSString *strStar; 

-(IBAction) Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) LogOut;
-(IBAction) check_In;
-(IBAction) check_Out;
-(IBAction) SearchFromDate;
-(IBAction) RefreshHistorytable;
-(IBAction) Backbtn_Click;

-(IBAction) StatisticsClick;
-(IBAction) btnDoneClick;
-(IBAction) btnFilterClcik;

-(IBAction) btnNextClick;
-(IBAction) btnPreviousClick;

-(void) Active_click;
-(void) Pending_click;

-(void) MoveToWebView;
-(void) GetVideoList;
-(void) ToDeleteWatchRecode;
-(void) DeleteDone;
-(void) LoadingStart;
-(void) CountofStatus;
-(void) showloding;
-(void) RatingSearch;
-(void) StarCount;
-(void) finishloading;

@end
