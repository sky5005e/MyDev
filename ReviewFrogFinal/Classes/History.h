//
//  History.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "xmlparsarhistory.h"
#import "cellHistoy.h"
#import "MBProgressHUD.h"
#import "PostedReview.h"

@class Review_FrogAppDelegate;
@interface History : UIViewController <UITableViewDataSource,UITableViewDelegate, cellHistoyDelegate,MBProgressHUDDelegate,UIPickerViewDelegate, UIPickerViewDataSource>{
	
	IBOutlet UITableView *tblp1;
	Review_FrogAppDelegate *appDelegate;
	PostedReview *post;
	
	NSString *strACount;
	NSString *strDCount;
	NSString *strICount;
	
	IBOutlet UITextField *checkOuttxt;
	IBOutlet UITextField *checkIntxt;
	
	UIPopoverController *popView;
	UIAlertView *alertConfirm;
	User *objSelectedUser1;
	IBOutlet UIButton *Historybutton;
	NSDate *SysDate1;
	NSString *SysDate2;
		
	UIDatePicker *checkInPicker;
	UIDatePicker *checkOutPicker;
	BOOL pickeflag;
	BOOL pickeflag1;
	NSDate *date;
	
	NSString *Fromdate;
	NSString *Todate;
	float strRating;
	IBOutlet UILabel *Acount;
	IBOutlet UILabel *Icount;
	IBOutlet UILabel *Dcount;
	
	IBOutlet UILabel *lblAname;
	IBOutlet UILabel *lblIname;
	IBOutlet UILabel *lblDname;
	
	IBOutlet UIButton *btnPost;
	IBOutlet UIButton *btnNotPost;
	IBOutlet UIButton *btnInternal;
	IBOutlet UIButton *btnFilter;
	
	
	IBOutlet UIButton *btnStatistics;
	
	IBOutlet UIPickerView *picFilter;
	IBOutlet UIToolbar *tlbFilter;
	IBOutlet UILabel *lblFirst;
	IBOutlet UILabel *lblFrom;
	
	int index;
	BOOL flgrate;
	BOOL flgPicfilte;
	
	BOOL flgInactive;
	BOOL flgactive;
	BOOL flgDelete;
	
	
	NSMutableArray *ArrFilterRecord;
	MBProgressHUD *HUD;
	NSString *stronestar;
	NSString *strtwostar;
	NSString *strthreestar;
	NSString *strfourstar;
	NSString *strfivestar;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgGiveAway;
	
/*	int A;
	int I;
	int D ;
	int fivestar;
	int fourstar;
	int threestar;
	int twostar;
	int onestar;*/
	BOOL flgreloadstop;
	int Count;
	NSString *strCategory;
	NSString *strStar; 
	
	int counter;
	int searchcounter;
	IBOutlet UIButton *btnadmin;
	IBOutlet UIButton *btnnext;
	IBOutlet UIButton *btnprevious;
	BOOL flgsearch;
    BOOL flgNewSearch;
    BOOL flgDateSearch;
}
@property(nonatomic, retain) IBOutlet UITableView *tblp1;
@property(nonatomic, retain) IBOutlet UIButton *Historybutton;
@property(nonatomic, retain) NSDate *SysDate1;
@property(nonatomic, retain) NSString *SysDate2;
@property(nonatomic, retain) IBOutlet UITextField *txtSearch;

@property (nonatomic, retain) UIDatePicker *checkInPicker;
@property (nonatomic, retain) UIDatePicker *checkOutPicker; 

@property BOOL pickeflag;
@property BOOL pickeflag1;
@property BOOL flgreloadstop;

@property (nonatomic, retain) NSDate *date;

@property (nonatomic, retain) NSString *Fromdate;
@property (nonatomic, retain) NSString *Todate;

@property (nonatomic, retain) IBOutlet UILabel *Acount;
@property (nonatomic, retain) IBOutlet UILabel *Icount;
@property (nonatomic, retain) IBOutlet UILabel *Dcount;
@property (nonatomic, retain) NSMutableArray *ArrFilterRecord;
@property (nonatomic, retain) NSString *strACount;
@property (nonatomic, retain) NSString *strDCount;
@property (nonatomic, retain) NSString *strICount;
@property (nonatomic, retain) NSString *stronestar;
@property (nonatomic, retain) NSString *strtwostar;
@property (nonatomic, retain) NSString *strthreestar;
@property (nonatomic, retain) NSString *strfourstar;
@property (nonatomic, retain) NSString *strfivestar;
@property (nonatomic, retain) NSString *strCategory;
@property (nonatomic, retain) NSString *strStar;

-(IBAction) Home;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(IBAction) LogOut;
-(IBAction) check_In;
-(IBAction) check_Out;
-(IBAction) SearchFromDate;
-(IBAction) RefreshHistorytable;

-(void) Inactive_click;
-(void) Active_click;
-(void) Internal_Click;
-(IBAction) Backbtn_Click;

-(void) CountofStatus;
-(IBAction) StatisticsClick;
-(IBAction) btnFilterClcik;

-(void) showloding;
-(void) getHistoryList;
-(void) DataDelete :(User *)user;
-(void) ReloadTableData;

-(IBAction)btnDoneClick;
-(void)RatingSearch;
-(void)StarCount;
-(void)RatewiseSort;
-(IBAction) btnNextClick;
-(IBAction) btnPreviousClick;

@end
