    //
//  VideoListViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 6/22/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "VideoListViewController.h"
#import "Video.h"
#import "PlayVideoViewController.h"
#import "XMLVideo.h"
#import "Review_FrogAppDelegate.h"
#import "CellVideo.h"
#import "ApplicationValidation.h"
#import "HistoryLogin.h"
#import "Terms_Condition.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "OptionOfHistoryViewController.h"
#import "VideoCategoryWiseListViewController.h"
#import "WriteORRecodeViewController.h"
#import "LoginInfo.h"

@implementation VideoListViewController
@synthesize tblVideoList;

@synthesize checkOuttxt;
@synthesize checkIntxt;
@synthesize SysDate1;
@synthesize date;
@synthesize checkInPicker;
@synthesize checkOutPicker;
@synthesize pickeflag;
@synthesize pickeflag1;
@synthesize SysDate2;
@synthesize Fromdate;
@synthesize Todate;
@synthesize objVideo;
@synthesize ArrselectedVideoList;
@synthesize flgrate;
@synthesize strACount;
@synthesize strPCount;
@synthesize ArrActive;
@synthesize ArrPending;

@synthesize stronestar;
@synthesize strtwostar;
@synthesize strthreestar;
@synthesize strfourstar;
@synthesize strfivestar;
@synthesize flgStopReloading;
@synthesize strCategory;
@synthesize strStar; 
@synthesize flgreloadstop;
 // The designated initializer.  Override if you create the controller programmatically and want to perform customization that is not appropriate for viewDidLoad.

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	picFilter.hidden=YES;
	tlbFilter.hidden=YES;
	lblFirst.hidden=NO;
	lblFrom.hidden=YES;

	self.strACount = @"0";
	self.strPCount  = @"0";
	self.strfivestar = @"0";
	self.strfourstar = @"0";
	self.strthreestar = @"0";
	self.strtwostar = @"0";
	self.stronestar= @"0";
	
	NSLog(@"strone:-%@",self.stronestar);
	
	appDelegate.delarrVideoList = [[NSMutableArray alloc] init];
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
	UIDatePicker *theDatePicker = [[UIDatePicker alloc] initWithFrame:CGRectMake(74, 220, 320.0, 216.0)];
    theDatePicker.datePickerMode = UIDatePickerModeDate;
    self.checkInPicker = theDatePicker;
    [theDatePicker release];
	 [checkInPicker addTarget:self action:@selector(dateChangedIn) forControlEvents:UIControlEventValueChanged];	
	UIDatePicker *theDatePickerOut = [[UIDatePicker alloc] initWithFrame:CGRectMake(296, 220, 320.0, 216.0)];
    theDatePickerOut.datePickerMode = UIDatePickerModeDate;
    self.checkOutPicker = theDatePickerOut;
    [theDatePickerOut release];
	
	btnDelete.hidden=YES;
	
    [checkOutPicker addTarget:self action:@selector(dateChangedOut) forControlEvents:UIControlEventValueChanged];
	if (flgreloadstop==TRUE) {
		
		//Add Category Label
		UILabel *lblCategory = [[UILabel alloc] init];
		[lblCategory setFont:[UIFont fontWithName:@"Helvetica-Bold" size:22]];
		
		CGSize maximumSize = CGSizeMake(300, 27);
		UIFont *CFont = [UIFont fontWithName:@"Helvetica-Bold"size:22];
		//UIFont *CFont = [UIFont boldSystemFontOfSize:22];
		CGSize CategoryStringSize = [strCategory sizeWithFont:CFont 
											constrainedToSize:maximumSize 
												lineBreakMode:lblCategory.lineBreakMode];
		CGRect CategoryFrame = CGRectMake(417, 221, CategoryStringSize.width, 27);
		NSLog(@"Categoryframesize %f",CategoryStringSize.width);
		lblCategory.frame = CategoryFrame;
		
		[lblCategory setTextColor:[UIColor blackColor]];
		lblCategory.text = strCategory;
		[self.view addSubview:lblCategory];	
		
		//Add Star
		UILabel *lblStar = [[UILabel alloc] init];
		[lblStar setFont:[UIFont fontWithName:@"Helvetica-Bold" size:22]];
		strStar = [NSString stringWithFormat:@"- %@ Star",strStar];
		CGSize StarStringSize = [strStar sizeWithFont:CFont 
									constrainedToSize:maximumSize 
										lineBreakMode:lblStar.lineBreakMode];
		CGRect StarFrame = CGRectMake(420+CategoryStringSize.width, 221,StarStringSize.width, 27);
		
		NSLog(@"Starframesize %f",StarStringSize.width);
		lblStar.frame = StarFrame;
		
		[lblStar setTextColor:[UIColor blackColor]];
		lblStar.text = strStar;
		[self.view addSubview:lblStar];	
	}
	
	[self showloding];
}
-(void)viewWillAppear:(BOOL)animated
{
    if (!appDelegate.VBack) {
        appDelegate.VBack=FALSE;
        self.strACount = @"0";
        self.strPCount  = @"0";
        self.strfivestar = @"0";
        self.strfourstar = @"0";
        self.strthreestar = @"0";
        self.strtwostar = @"0";
        self.stronestar= @"0";
        
    }
     
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
    if (pathGiveAway !=nil) {   
        
        imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
        
    }
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    if (pathBussinesslogo !=nil) {   
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }
    if (!imgGiveAway.image) {
        UIImage *img = [UIImage imageNamed:@"giftcard.png"];
        imgGiveAway.image = nil;
    }
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;

    }	/*LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
	if (alogin.imgBlogo!= nil) {
		//NSLog(@"Not Right");
		imgBLogo.image = alogin.imgBlogo;
		
	} else {
		
		if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
			
			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			imgBLogo.image = image;			
		}
		else {
			UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
			imgBLogo.image = img;
				
		}
	}
		
	if (alogin.imgGiveAway!= nil) {
		//NSLog(@"Not Right");
		imgGiveAway.image = alogin.imgGiveAway;
		
	} else {
		
		if (![alogin.userbusinessgiveaway isEqualToString:@"0"]) {
			NSString *str = [alogin.userbusinessgiveaway stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
			NSURL *url = [[NSURL alloc] initWithString:str];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			alogin.imgGiveAway = image;
			imgGiveAway.image = image;
			
		}else {
			if(appDelegate.flggiveaway)
			{
				imgGiveAway.image = nil;	
			}else {
				UIImage *img = [UIImage imageNamed:@"giftcard.png"];
				imgGiveAway.image = img;
            }
		}
	}*/
	
	if (appDelegate.VideoSelectedType) {

		btnFilter.hidden=YES;
	}else {
        if(flgDateSearch)
        {
            btnFilter.hidden = YES;
        }else{     
		btnFilter.hidden=NO;
        }
	}
/*	if (appDelegate.flgStopReloading) {
		appDelegate.flgStopReloading=FALSE;
		int tempconter = [appDelegate.delarrVideoList count];
		for (int i = counter;i<tempconter; i++) {
		[appDelegate.delarrVideoList removeObjectAtIndex:counter];
		}
		[self showloding];
	}
*/
    if (appDelegate.flgStopReloading) {
		appDelegate.flgStopReloading=FALSE;
        if(flgNewSearch){NSLog(@"Not for search");
            [self SearchFromDate];
        }else{
            int tempconter = [appDelegate.delarrVideoList count];
            for (int i = counter;i<tempconter; i++) {
                [appDelegate.delarrVideoList removeObjectAtIndex:counter];
            }
            [self showloding];
        }
		
	}		
}
/*-(void) StarCount
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi.php",appDelegate.set_APIurl]];
	
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	[request setHTTPMethod:@"POST"];
	NSData *requestBody = [[NSString stringWithFormat:@"api=itunesversion1_1_starwisevideocount&user_id=%@",appDelegate.userId] dataUsingEncoding:NSUTF8StringEncoding];
	[request setHTTPBody:requestBody];
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	NSLog(@"responseString=%@",responseString);	
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	
	NSRange rngonestarStart = [responseString rangeOfString:@"<onestar>"];
	if (rngonestarStart.length > 0) {
		self.stronestar = [responseString substringFromIndex:rngonestarStart.location+rngonestarStart.length];
		
		NSRange rngonestarEnd = [self.stronestar rangeOfString:@"</onestar>"];
		if (rngonestarEnd.length > 0) {
			self.stronestar = [self.stronestar substringToIndex:rngonestarEnd.location];
		}		
	} else {
		self.stronestar = @"0";
	}
	
	NSRange rngtwostarStart = [responseString rangeOfString:@"<twostar>"];
	if (rngtwostarStart.length > 0) {
		self.strtwostar = [responseString substringFromIndex:rngtwostarStart.location+rngtwostarStart.length];
		
		NSRange rngtwostarEnd = [self.strtwostar rangeOfString:@"</twostar>"];
		if (rngtwostarEnd.length > 0) {
			self.strtwostar = [self.strtwostar substringToIndex:rngtwostarEnd.location];
			NSLog(@"StrAccount:-%@",self.strtwostar);
		}		
	} else {
		self.strtwostar = @"0";
	}
	
	NSRange rngthreestarStart = [responseString rangeOfString:@"<threestar>"];
	if (rngthreestarStart.length > 0) {
		self.strthreestar = [responseString substringFromIndex:rngthreestarStart.location+rngthreestarStart.length];
		
		NSRange rngthreestarEnd = [self.strthreestar rangeOfString:@"</threestar>"];
		if (rngthreestarEnd.length > 0) {
			self.strthreestar = [self.strthreestar substringToIndex:rngthreestarEnd.location];
		}		
	} else {
		self.strthreestar = @"0";
	}
	
	NSRange rngfourstarStart = [responseString rangeOfString:@"<fourstar>"];
	if (rngfourstarStart.length > 0) {
		self.strfourstar = [responseString substringFromIndex:rngfourstarStart.location+rngfourstarStart.length];
		
		NSRange rngfourstarEnd = [self.strfourstar rangeOfString:@"</fourstar>"];
		if (rngfourstarEnd.length > 0) {
			self.strfourstar = [self.strfourstar substringToIndex:rngfourstarEnd.location];
		}		
	} else {
		self.strfourstar = @"0";
	}
	NSRange rngfivestarStart = [responseString rangeOfString:@"<fivestar>"];
	if (rngfivestarStart.length > 0) {
		self.strfivestar = [responseString substringFromIndex:rngfivestarStart.location+rngfivestarStart.length];
		
		NSRange rngfivestarEnd = [self.strfivestar rangeOfString:@"</fivestar>"];
		if (rngfivestarEnd.length > 0) {
			self.strfivestar = [self.strfivestar substringToIndex:rngfivestarEnd.location];
		}		
	} else {
		self.strfivestar = @"0";
	}
	
	
}*/

/*-(void)CountofStatus
{
		
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi.php",appDelegate.set_APIurl]];
	
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	[request setHTTPMethod:@"POST"];
	NSData *requestBody = [[NSString stringWithFormat:@"api=countVideoStatus&user_id=%@",appDelegate.userId] dataUsingEncoding:NSUTF8StringEncoding];
	[request setHTTPBody:requestBody];
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	NSLog(@"responseString=%@",responseString);
	
	
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];	
	NSRange rngACountStart = [responseString rangeOfString:@"<active>"];
	if (rngACountStart.length > 0) {
		self.strACount = [responseString substringFromIndex:rngACountStart.location+rngACountStart.length];
		
		NSRange rngACountEnd = [self.strACount rangeOfString:@"</active>"];
		if (rngACountEnd.length > 0) {
			self.strACount = [self.strACount substringToIndex:rngACountEnd.location];
			NSLog(@"StrAccount:-%@",self.strACount);
		}		
	} else {
		self.strACount = @"0";
	}	
	NSRange rngICountStart = [responseString rangeOfString:@"<pending>"];
	if (rngICountStart.length > 0) {
		self.strPCount = [responseString substringFromIndex:rngICountStart.location+rngICountStart.length];
		
		NSRange rngICountEnd = [self.strPCount rangeOfString:@"</pending>"];
		if (rngICountEnd.length > 0) {
			self.strPCount = [self.strPCount substringToIndex:rngICountEnd.location];
			NSLog(@"strPCount:-%@",self.strPCount);
		}		
	} else {
		self.strPCount = @"0";
	}
	
}*/
-(IBAction) btnNextClick
{
	flgactive=FALSE;
	flgPending=FALSE;
	self.flgrate=FALSE;
	appDelegate.flgVideoSearch=FALSE;
	checkIntxt.text =@"" ;
	checkOuttxt.text =@"";
	txtrating.text = @"";
	self.strACount = @"0";
	self.strPCount  = @"0";
	self.strfivestar = @"0";
	self.strfourstar = @"0";
	self.strthreestar = @"0";
	self.strtwostar = @"0";
	self.stronestar= @"0";
	
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
	counter+=25;
	
	if (counter>[appDelegate.delarrVideoList count]) {
		
		UIAlertView *alrmsg = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:
							   @"There is no more records."delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alrmsg show];
		[alrmsg release];
		
		counter-=25;
	}
	else {
		if (counter == [appDelegate.delarrVideoList count]) {
			[self showloding];
		}
		else {
			[self.tblVideoList reloadData];
		}
		
	}
	
}
-(IBAction) btnPreviousClick
{
	flgactive=FALSE;
	flgPending=FALSE;
	self.flgrate=FALSE;
	appDelegate.flgVideoSearch=FALSE;
	checkIntxt.text =@"" ;
	checkOuttxt.text =@"";
	txtrating.text = @"";
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
	/*self.strACount = @"0";
	self.strPCount  = @"0";
	self.strfivestar = @"0";
	self.strfourstar = @"0";
	self.strthreestar = @"0";
	self.strtwostar = @"0";
	self.stronestar= @"0";*/
	
	if (counter==0) {
		UIAlertView *alrmsg = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:
							   @"There is no more records."delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alrmsg show];
		[alrmsg release];
		
	}
	else {
		int tempconter = [appDelegate.delarrVideoList count];
		for (int i = counter;i<tempconter; i++) {
			[appDelegate.delarrVideoList removeObjectAtIndex:counter];
		}
		counter-=25;
		[self finishloading];
		[picFilter reloadAllComponents];		
	}
	
}

- (void) showloding {
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(GetVideoList) onTarget:self withObject:nil animated:YES];
}


-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.VideoSelectedType=@"";
	appDelegate.VideoSelectedType = nil;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}

-(IBAction) Admin
{	
	appDelegate.VideoSelectedType=@"";
	appDelegate.VideoSelectedType = nil;
	
	if(appDelegate.LoginConform==FALSE)
	
	{	
		HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];
	
        [self.navigationController pushViewController:historylogin animated:YES];
        [historylogin release];

	}
	else
	{
		OptionOfHistoryViewController *objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
        [self.navigationController pushViewController:objoption animated:YES];
        [objoption release];	
	}
	
}

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.VideoSelectedType=@"";
	appDelegate.VideoSelectedType = nil;
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}

-(IBAction) Backbtn_Click
{
	appDelegate.flgVideoSearch=FALSE;
	if (appDelegate.VideoSelectedType) {
        VideoCategoryWiseListViewController *obj= [[VideoCategoryWiseListViewController alloc]init];
        [self.navigationController pushViewController:obj animated:YES];
        [obj release];

		//[self dismissModalViewControllerAnimated:YES];
	}
	else {

        OptionOfHistoryViewController *obj= [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
        [self.navigationController pushViewController:obj animated:YES];
        [obj release];
	}
}

-(IBAction)StatisticsClick
{
	VideoCategoryWiseListViewController *objCategoryList = [[VideoCategoryWiseListViewController alloc]init];
    [self.navigationController pushViewController:objCategoryList animated:YES];
    [objCategoryList release];
}

-(IBAction) LogOut
{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *objWriteRecord = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];	
    [self.navigationController pushViewController:objWriteRecord animated:YES];
    [objWriteRecord release];
}

-(IBAction)dateChangedIn
{
    self.date = [checkInPicker date];	
}

-(IBAction)check_In
{   
	
	if(!pickeflag)
		
	{	
		if(pickeflag1)
		{
			[checkOutPicker removeFromSuperview]; 
			pickeflag1=FALSE;
			
		}
		
		[self.view addSubview:checkInPicker];
		
		NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
		[formatter setDateFormat:@"MM-dd-yyyy"];
		
		[formatter setTimeZone:[NSTimeZone timeZoneWithName:@"..."]];
		self.date = [checkInPicker date];
		checkIntxt.text = [formatter stringFromDate:self.date];
		NSLog(@"CheckIn Data----%@",checkIntxt.text);
		
		pickeflag=TRUE;
		
	}
	else
		
	{	NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
		[formatter setDateFormat:@"MM-dd-yyyy"];
		
		[formatter setTimeZone:[NSTimeZone timeZoneWithName:@"..."]];
		self.date = [checkInPicker date];
		checkIntxt.text = [formatter stringFromDate:self.date];
		NSLog(@"CheckIn Data----%@",checkIntxt.text);
		NSLog(@"Check Text-----%@",checkIntxt.text);
		[checkInPicker removeFromSuperview]; 
		pickeflag=FALSE;
		
	}
}


-(IBAction)dateChangedOut
{
    self.date = [checkOutPicker date];
	
}

-(IBAction)check_Out
{
	if(!pickeflag1){
		if(pickeflag)
		{
			[checkInPicker removeFromSuperview]; 
			pickeflag=FALSE;
		}
		[self.view addSubview:checkOutPicker];
		
		NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
		[formatter setDateFormat:@"MM-dd-yyyy"];
		
		//Optionally for time zone converstions
		[formatter setTimeZone:[NSTimeZone timeZoneWithName:@"..."]];
		self.date = [checkOutPicker date];
		checkOuttxt.text = [formatter stringFromDate:self.date];
		NSLog(@"CheckIn Data----%@",checkOuttxt.text);
		
		pickeflag1=TRUE;
		
	}
	else
		
	{		NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
		[formatter setDateFormat:@"MM-dd-yyyy"];
		
		//Optionally for time zone converstions
		[formatter setTimeZone:[NSTimeZone timeZoneWithName:@"..."]];
		self.date = [checkOutPicker date];
		checkOuttxt.text = [formatter stringFromDate:self.date];
		NSLog(@"CheckIn Data----%@",checkOuttxt.text);
		
		NSLog(@"Check Text-----%@",checkOuttxt.text);
		[checkOutPicker removeFromSuperview]; 
		pickeflag1=FALSE;
		
		
		NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
		[dateFormatter setDateFormat:@"MM-dd-yyyy"];
		NSDate *Date1 = [dateFormatter dateFromString:checkIntxt.text];
		
		
		NSDateFormatter *dateFormatter1 = [[NSDateFormatter alloc] init];
		[dateFormatter1 setDateFormat:@"MM-dd-yyyy"];
		NSDate *Date2 = [dateFormatter1 dateFromString:checkOuttxt.text];
		
		
		NSComparisonResult result = [Date2 compare:Date1];
		
		if (result==NSOrderedAscending) {
			
			UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"The from date should be later than the to date" delegate:self 
												  cancelButtonTitle:@"OK" 
												  otherButtonTitles:nil];
			[alert show];
			[alert release];
			
			
			NSLog(@"Date1 is in the future");
			
		}
		if(result==NSOrderedSame)
		{
			UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"The from date and to date are same" delegate:self 
												  cancelButtonTitle:@"OK" 
												  otherButtonTitles:nil];
			[alert show];
			[alert release];
			
			
			NSLog(@"Date1 is in the future");
		}
		
	}
	
}
-(IBAction)SearchFromDate
{
	[checkInPicker removeFromSuperview];
	[checkOutPicker removeFromSuperview];
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal];
	lblFirst.hidden=YES;
	lblFrom.hidden=NO;
	flgactive=FALSE;
	flgPending=FALSE;
	flgrate=FALSE;
	btnFilter.hidden = YES;
	btnnext.hidden = YES;
	btnPrevious.hidden = YES;
	appDelegate.flgVideoSearch=TRUE;
    flgDateSearch=TRUE;
    flgNewSearch = TRUE;
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(StartSearch) onTarget:self withObject:nil animated:YES];
	
}
-(void)StartSearch
{	
	NSAutoreleasePool* pool = [[NSAutoreleasePool alloc] init];	
	Fromdate=[checkIntxt text];
	Todate=[checkOuttxt text];
	NSString * str;
	
	if (appDelegate.VideoSelectedType) {
	
		str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_CategoryWiseSearchVideo&startdate=%@&enddate=%@&user_id=%@&user_category=%@&user_categorytype=%@&version=2",appDelegate.ChangeUrl,Fromdate,Todate,appDelegate.userId,appDelegate.SelectedCategory,appDelegate.VideoSelectedType];
		str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];

	}
	else {
		str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_BetweenDateSearchVideo&startdate=%@&enddate=%@&user_id=%@&version=2",appDelegate.ChangeUrl,Fromdate,Todate,appDelegate.userId];
		str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];

	}
	
	NSLog(@"URLString=---~~~%@",str);	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	XMLVideo *parser = [[XMLVideo alloc] initXMLVideo];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		NSLog(@"No Errors");
		if([appDelegate.delArrvieoSearchList count] ==0) {
			UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
													   cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[loginalert show];
			[loginalert release];
			
		}
		[self.tblVideoList reloadData];
		if([appDelegate.delArrvieoSearchList count] >1)
		{	
		NSIndexPath *iPath = [NSIndexPath indexPathForRow:0 inSection:0];
		[self.tblVideoList scrollToRowAtIndexPath:iPath atScrollPosition:UITableViewScrollPositionTop animated:NO];
		}
	}
	else{
		NSLog(@"Error Error Error!!!");
	
	}[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
	NSLog(@"cnt %d",[appDelegate.delArrvieoSearchList count]);
	[pool release];
	
}

-(void)GetVideoList
{
	NSAutoreleasePool* pool = [[NSAutoreleasePool alloc] init];

	NSString * str;
	if (appDelegate.VideoSelectedType) 
	{					
		str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_VideoReviewCategoryRecord&UserID=%@&starrate=%@&category=%@&pagging=%d&version=2",appDelegate.ChangeUrl,appDelegate.userId,appDelegate.VideoSelectedType,appDelegate.SelectedCategory,counter];
        str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	}
	else {		
		
	str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_fetchUserVideo&UserID=%@&pagging=%d&version=2",appDelegate.ChangeUrl,appDelegate.userId,counter];
        str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	
	}
	NSLog(@"URL String=%@",str);
	
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	XMLVideo *parser = [[XMLVideo alloc] initXMLVideo];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		NSLog(@"No Errors");
		if([appDelegate.delarrVideoList count] ==0 && appDelegate.flgVideoSearch==FALSE) {
			UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
													   cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[loginalert show];
			[loginalert release];
			
		}
		
	}
	else
	{
		NSLog(@"Error Error Error!!!");
	
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
		
	}[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
	NSLog(@"cnt %d",[appDelegate.delarrVideoList count]);
	[self performSelectorOnMainThread:@selector(finishloading) withObject:nil waitUntilDone:YES];
	[pool release];
}

-(void) finishloading
{
	if (!appDelegate.VideoSelectedType)
	{
		
	int I=0,A=0,fivestar=0,fourstar=0,threestar=0,twostar=0,onestar=0;
		for (int i = counter; i<[appDelegate.delarrVideoList count]; i++) {
			objVideo = [appDelegate.delarrVideoList objectAtIndex:i];
			if ([objVideo.VideoStatus isEqualToString:@"A"]) {			
				I++;
				self.strACount = [NSString stringWithFormat:@"%d",I];
				
				NSLog(@"striCount:-%@",self.strACount);
			}
			if ([objVideo.VideoStatus isEqualToString:@"P"])
			{
				A++;
				self.strPCount = [NSString stringWithFormat:@"%d",A];
				NSLog(@"strAcount:-%@",self.strPCount);
			}
						
			if(objVideo.ReviewRate==5) {
				fivestar++;
				self.strfivestar = [NSString stringWithFormat:@"%d",fivestar];
			}
			
			if(objVideo.ReviewRate==4) {
				fourstar++;
				self.strfourstar = [NSString stringWithFormat:@"%d",fourstar];
				
			}
			if(objVideo.ReviewRate==3) {
				threestar++;
				self.strthreestar = [NSString stringWithFormat:@"%d",threestar];
				NSLog(@"self.strthreestar:-%@",self.strthreestar);
			}
			if(objVideo.ReviewRate==2) {
				twostar++;
				self.strtwostar = [NSString stringWithFormat:@"%d",twostar];
				NSLog(@"self.strtwostar:-%@",self.strtwostar);
			}
			if(objVideo.ReviewRate ==1) {
				onestar++;
				self.stronestar = [NSString stringWithFormat:@"%d",onestar];
				NSLog(@"self.stronestar:-%@",self.stronestar);
			}
			
		}
	}
	/*if (appDelegate.VideoSelectedType) {
		[self RatewiseSort];
		
	}*/
	
	[self.tblVideoList reloadData];
	[picFilter reloadAllComponents];
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
 
	if (flgactive==TRUE) {
		return [self.ArrActive count];
	}
	else if(flgPending==TRUE) {
		return [self.ArrPending count];	
	}
	else if (self.flgrate==TRUE) {
//		if (([self.ArrselectedVideoList count]-counter)<25) {
//			return ([self.ArrselectedVideoList count]-counter);
//		}	
		//return 25;
		return ([self.ArrselectedVideoList count]);
	}
	else if(appDelegate.flgVideoSearch==TRUE) {
		return [appDelegate.delArrvieoSearchList count];
	}

	else {
		if (([appDelegate.delarrVideoList count]-counter)<25) {
			return ([appDelegate.delarrVideoList count]-counter);
		}	
		return 25;
	}
}
- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	return 60.0;
}

// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
	
	
	static NSString *CellIdentifier = @"Cell";
    
	UITableViewCell *cell = [self.tblVideoList dequeueReusableCellWithIdentifier:CellIdentifier];
	
	cell = [[[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier] autorelease];
    //set cell	
	CellVideo *objCell = [[CellVideo alloc]init];
	[cell.contentView addSubview:objCell.view];
	if (flgactive==TRUE) {
			objVideo = [self.ArrActive objectAtIndex:indexPath.row];
	}
	else if(flgPending==TRUE) {
		objVideo = [self.ArrPending objectAtIndex:indexPath.row];
	}

	else if (self.flgrate==TRUE) {
		objVideo = [self.ArrselectedVideoList objectAtIndex:indexPath.row];
	}
	else if(appDelegate.flgVideoSearch==TRUE) {
		objVideo = [appDelegate.delArrvieoSearchList objectAtIndex:indexPath.row];
	}
	else{
		objVideo = [appDelegate.delarrVideoList objectAtIndex:indexPath.row+counter];
}
	int SNO=indexPath.row+counter;
	//NSString *strNo = [NSString stringWithFormat:@""%d]
	objCell.lblNO.text = [NSString stringWithFormat:@"%d",SNO+1];
	
	objCell.lblName.text = objVideo.ReviewPersonName;
	objCell.lblVideoTitle.text = [objVideo.ReviewVideoTitle stringByReplacingOccurrencesOfString:@"_" withString:@" "];
	objCell.lblSDate.text = objVideo.PostedDate; 
	objCell.lblCategory.text = objVideo.ReviewCategory;
	
	if([objVideo.VideoStatus isEqualToString:@"P"])
	{
	objCell.lblStatus.text = @"Pending";	
	btnDelete = [[UIButton alloc] init];
	btnDelete = [UIButton buttonWithType:UIButtonTypeCustom];
	[btnDelete setFrame:CGRectMake(984, 15, 29, 31)];
	btnDelete.tag = indexPath.row;
	UIImage *imgbtn = [UIImage imageNamed:@"deletebtn.png"];
	[btnDelete setBackgroundImage:imgbtn forState:UIControlStateNormal];
	[btnDelete addTarget:self action:@selector(DeleteButton_Click:)forControlEvents:UIControlEventTouchUpInside];
	[cell.contentView addSubview:btnDelete];
	}
	else {
		objCell.lblStatus.text = @"Active";
	}
	float fltRating = floor(objVideo.ReviewRate);
	float diffValue = ((objVideo.ReviewRate)-fltRating);
	int x = 75, y = 20, w = 21, h = 21;
	
	if (objVideo.ReviewRate < 0.4) {
		NSLog(@"No Rating");
		
	} else {
		for (int i=0; i<fltRating; i++) {
			CGRect rect = CGRectMake(x, y, w, h);
			objCell.imgGivenRating = [[UIImageView alloc] initWithFrame:rect];
			[objCell.imgGivenRating setImage:[UIImage imageNamed:@"Fullyellow25X25.png"]];
			[cell.contentView addSubview:objCell.imgGivenRating];
			[objCell.imgGivenRating release];
			x += w;
		}
		if (diffValue > 0 && diffValue < 0.4) 
		{
			CGRect rect = CGRectMake(x, y, w, h);
			objCell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[objCell.imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[cell.contentView addSubview:objCell.imghalfRating];
			[objCell.imghalfRating release];
			x += w;
			fltRating +=1;
			
		}		
		for (int j=fltRating; j<5; j++) {
			CGRect rect = CGRectMake(x, y, w, h);
			objCell.imgNoRating = [[UIImageView alloc] initWithFrame:rect];
			[objCell.imgNoRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[cell.contentView addSubview:objCell.imgNoRating];
			[objCell.imgNoRating release];
			x += w;
		}		
	}
	
	    return cell;
}
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
	if (flgactive==TRUE) {
		objVideo = [self.ArrActive objectAtIndex:indexPath.row];
	}
	else if(flgPending==TRUE) {
		objVideo = [self.ArrPending objectAtIndex:indexPath.row];
	}
	else if (self.flgrate==TRUE) {
		objVideo = [self.ArrselectedVideoList objectAtIndex:indexPath.row];
	}
	else if(appDelegate.flgVideoSearch==TRUE) {
		objVideo =  [appDelegate.delArrvieoSearchList objectAtIndex:indexPath.row];
	}
	else{
		objVideo = [appDelegate.delarrVideoList objectAtIndex:indexPath.row+counter];
	}
	NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
	NSString *documentsDirectory = [paths objectAtIndex:0]; 
	
	NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
	
	NSString *videoName = objVideo.ReviewVideoName;
	
	appDelegate.Data_Title =  objVideo.ReviewVideoName;
	
	NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,videoName];
	NSData *dt = [[NSData alloc] initWithContentsOfFile:exportPath];
	appDelegate.NewData = dt;
	
	NSLog(@"exportPath %@",exportPath);
	
	appDelegate.VideoPath = [NSURL fileURLWithPath:exportPath];
	
	
	//NSLog(@"VideoURL:>%@",appDelegate.VideoPath);
	[tableView deselectRowAtIndexPath:indexPath animated:YES];
	
		
		NSLog(@"Done");
		[self MoveToWebView];
}

/*
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
	
	objVideo = [appDelegate.delarrVideoList objectAtIndex:indexPath.row];
	
	NSString *str= [[NSBundle mainBundle] pathForResource:@"idiots"
															ofType:@"mov"];
		
	appDelegate.NewData = [[NSData alloc] initWithContentsOfFile:str];
		
	NSLog(@"Done");
	[self MoveToWebView];
	
}
*/

-(IBAction)RefreshHistorytable
{	
    if (appDelegate.VideoSelectedType) {
		
		btnFilter.hidden=YES;
	}else {		
		btnFilter.hidden=NO;		
	}
    if(flgNewSearch)
    {
        if(appDelegate.flgSelectedVideoType)
        {
            NSLog(@"Viedo category");
        }else{        
        appDelegate.VideoSelectedType=@"";
        appDelegate.VideoSelectedType=nil;
        }
        int tempconter = [appDelegate.delarrVideoList count];
		for (int i = counter;i<tempconter; i++) {
			[appDelegate.delarrVideoList removeObjectAtIndex:counter];
		}
        
        [self showloding];
    }


	checkIntxt.text =@"" ;
	checkOuttxt.text =@"";
	txtrating.text = @"";
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
	flgactive=FALSE;
	flgPending=FALSE;
    flgDateSearch=FALSE;
    flgNewSearch=FALSE;
	self.flgrate=FALSE;	
	appDelegate.flgVideoSearch=FALSE;
	
	btnnext.hidden = NO;
	btnPrevious.hidden = NO;

	
	
	[self.tblVideoList reloadData];
}


-(void) MoveToWebView
{
	
	PlayVideoViewController *objvideoplay= [[PlayVideoViewController alloc]init];
	objvideoplay.avideo = objVideo;
    [self.navigationController pushViewController:objvideoplay animated:YES];
    [objvideoplay release];
}


-(void)DeleteButton_Click :(int)index
{
	
	appDelegate.btnDelete = (UIButton *)index;
	NSLog(@"Btnselect-----%d",appDelegate.btnDelete.tag);
	[self ToDeleteWatchRecode];
	
}
-(void)ToDeleteWatchRecode
{
	NSLog(@"DeleteShruffle");
	alertConfirm = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are you sure want to delete this record?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes",nil];
	[alertConfirm show];
	[alertConfirm release];
	
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex 
{
	if (alertView == alertConfirm) {
		if (buttonIndex == 0) {
			NSLog(@"No");
		} else {
			[self DeleteDone];
		}
		
	}
}
-(void)DeleteDone
{
	NSLog(@"Delete Done");
	
	objVideo = [appDelegate.delarrVideoList objectAtIndex:appDelegate.btnDelete.tag];
	
	NSArray *paths =
    NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
	NSString *documentsDirectory = [paths objectAtIndex:0]; 
	
	NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
	
	NSString *videoName = objVideo.ReviewVideoName;
		
	NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,videoName];
	NSLog(@"exportPath %@",exportPath);
	
	NSFileManager *fileManager = [NSFileManager defaultManager];
	if([fileManager fileExistsAtPath:exportPath]){
		NSError *err;
		[fileManager removeItemAtPath:exportPath error:&err];
	}
	else{
		NSLog(@"Video Not Exist");
	}
	
	//Delete Frog Database
	
	
	NSString * str = [[[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=RemoveVideo&videoid=%d&version=2",appDelegate.set_APIurl,objVideo.id]stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	
	NSLog(@"URL String=%@",str);
	
	
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:[NSURL URLWithString:str]];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString %@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	
	if ([responseString isEqualToString:@"1"]) {
		NSLog(@"Done");
		[appDelegate.delarrVideoList removeObjectAtIndex:appDelegate.btnDelete.tag];
		
		[self performSelectorOnMainThread:@selector(TableReload) withObject:nil waitUntilDone:YES];
		
		UIAlertView *alrDelete = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Your record delete successfully" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];		
		[alrDelete show];
		[alrDelete release];
		
		
	}
	else {
		NSLog(@"Erro");
	}

	
}
-(IBAction)TableReload
{
	[self.tblVideoList reloadData];
}
-(void)Loadtablrecord
{
	[self.tblVideoList reloadData];
	
	//[txtrating resignFirstResponder];
}
- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
	return 1;
}

- (NSInteger)pickerView:(UIPickerView *)thePickerView numberOfRowsInComponent:(NSInteger)component 
{
	
	return 7;
}

- (UIView *)pickerView:(UIPickerView *)pickerView viewForRow:(NSInteger)row
		  forComponent:(NSInteger)component reusingView:(UIView *)view
{
	
	if (row==0) 
    {
        UILabel *NameLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, 100, 46)];
		NameLabel.text = @"ACTIVE";
		NameLabel.textAlignment = UITextAlignmentLeft;
		NameLabel.backgroundColor = [UIColor clearColor];
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.strACount;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:NameLabel atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	
	if (row==1) 
    {
		UILabel *NameLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, 100, 46)];
		NameLabel.text = @"PENDING";
		NameLabel.textAlignment = UITextAlignmentLeft;
		NameLabel.backgroundColor = [UIColor clearColor];
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.strPCount;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:NameLabel atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	if (row==2) 
    {
		UIImage *img = [UIImage imageNamed:@"5star.png"];
        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
        temp.frame = CGRectMake(0, 4, 140, 29);
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.strfivestar;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:temp atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	if (row==3) 
    {
		UIImage *img = [UIImage imageNamed:@"4star.png"];
        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
        temp.frame = CGRectMake(0, 4, 114, 29);
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.strfourstar;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:temp atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	
	if (row==4) 
    {
		UIImage *img = [UIImage imageNamed:@"3star.png"];
        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
        temp.frame = CGRectMake(0, 4, 84, 29);
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.strthreestar;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:temp atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	
	if (row==5) 
    {
		UIImage *img = [UIImage imageNamed:@"2star.png"];
        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
        temp.frame = CGRectMake(0, 4, 54, 29);
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.strtwostar;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:temp atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
	}
	if (row==6) 
    {
		UIImage *img = [UIImage imageNamed:@"1star.png"];
        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
        temp.frame = CGRectMake(0, 4, 29, 29);
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.stronestar;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:temp atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	
}
- (void)pickerView:(UIPickerView *)thePickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{	
	if (row==0) {
		index=row;
	}	
	if (row==1) {
		index=row;
	}
	if (row==2) {
		index=row;	
	}
	if (row==3) {
		index=row;	
	}
	if (row==4) {
		index=row;	
	}
	if (row==5) {
		index=row;	
	}
	if (row==6) {
		index=row;	
	}
		
}

-(IBAction)btnDoneClick
{
	picFilter.hidden=YES;
	tlbFilter.hidden=YES;
	flgPicfilte=FALSE;	
	lblFirst.hidden=YES;
	lblFrom.hidden=NO;

	appDelegate.flgVideoSearch=FALSE;
	
	if (index==0) {
		
		[btnFilter setTitle:[NSString stringWithFormat:@"Active            %@",self.strACount] forState:UIControlStateNormal];
			
		[self Active_click];
		[self.tblVideoList reloadData];
	}
	if (index==1) {		
		[self Pending_click];
		[btnFilter setTitle:[NSString stringWithFormat:@"Pending           %@",self.strPCount] forState:UIControlStateNormal];
		[self.tblVideoList reloadData];
	}
	if (index==2) {		
		strRating=5;
		[btnFilter setTitle:[NSString stringWithFormat:@"5Star                %@",strfivestar] forState:UIControlStateNormal]; 
		[self RatingSearch];
		[self.tblVideoList reloadData];
	}
	if (index==3) {		
		strRating=4;
		[btnFilter setTitle:[NSString stringWithFormat:@"4Star                %@",strfourstar] forState:UIControlStateNormal];
		[self RatingSearch];
		[self.tblVideoList reloadData];
	}
	if (index==4) {		
		strRating=3;
		[btnFilter setTitle:[NSString stringWithFormat:@"3Star                %@",strthreestar] forState:UIControlStateNormal]; 
		[self RatingSearch];
		[self.tblVideoList reloadData];
	}
	if (index==5) {		
		strRating=2;
		[btnFilter setTitle:[NSString stringWithFormat:@"2Star                %@",strtwostar] forState:UIControlStateNormal];
		[self RatingSearch];
		[self.tblVideoList reloadData];
	}
	if (index==6) {		
		strRating=1;
		[btnFilter setTitle:[NSString stringWithFormat:@"1Star                %@",stronestar] forState:UIControlStateNormal];
		[self RatingSearch];
		[self.tblVideoList reloadData];
	}	
}

-(void) Active_click
{	
	

	self.ArrActive = [[NSMutableArray alloc]init];
	
	
	flgactive=TRUE;
	flgPending=FALSE;
	flgrate=FALSE;
	
	for (int i=counter; i<[appDelegate.delarrVideoList count]; i++) {
		objVideo = [appDelegate.delarrVideoList objectAtIndex:i];
		
		if ([objVideo.VideoStatus isEqualToString:@"A" ])
		{
			[self.ArrActive addObject:objVideo];
		}
	}
	NSLog(@"ArrActive:%d",[self.ArrActive count]);
	
	[self.ArrActive count];
	[self.tblVideoList reloadData];
}
-(void) Pending_click
{		
	self.ArrPending = [[NSMutableArray alloc]init];

	
	flgactive=FALSE;
	flgPending=TRUE;
	flgrate=FALSE;
	
	for (int i=counter; i<[appDelegate.delarrVideoList count]; i++) {
		objVideo = [appDelegate.delarrVideoList objectAtIndex:i];
		
		if ([objVideo.VideoStatus isEqualToString:@"P" ])
		{
			[self.ArrPending addObject:objVideo];
		}
	}
	NSLog(@"ArrActive:%d",[self.ArrPending count]);
	
	[self.ArrPending count];
	[self.tblVideoList reloadData];
}
-(void) RatingSearch
{
	flgactive=FALSE;
	flgPending=FALSE;
	flgrate=TRUE;
	self.ArrselectedVideoList = [[NSMutableArray alloc]init];

	
	for (int i=counter; i<[appDelegate.delarrVideoList count]; i++) {
		
		objVideo = [appDelegate.delarrVideoList objectAtIndex:i];
		float flatRating = floor(objVideo.ReviewRate);
		
		if (flatRating==strRating) {
			
			if (appDelegate.VideoSelectedType) 
            {
				if ([objVideo.ReviewCategory isEqualToString:appDelegate.SelectedCategory]) 
                {
					[self.ArrselectedVideoList addObject:objVideo];
		
				}
			}
			else
			{
				[self.ArrselectedVideoList addObject:objVideo];
								
			}			
		}
		
	}
/*	if ([self.ArrselectedVideoList count]==0) {
	UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"Whoops!! No Reviews Found." delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
	
	[loginalert show];
	[loginalert release];
 }*/
		
	
}
-(void) RatewiseSort
{
	if ([appDelegate.VideoSelectedType isEqualToString:@"5"]) {		
		strRating=5;
		[self RatingSearch];
	}
	if ([appDelegate.VideoSelectedType isEqualToString:@"4"]) {		
		strRating=4;
		[self RatingSearch];
	}
	if ([appDelegate.VideoSelectedType isEqualToString:@"3"]) {		
		strRating=3;
		[self RatingSearch];
	}
	if ([appDelegate.VideoSelectedType isEqualToString:@"2"]) {		
		strRating=2;
		[self RatingSearch];
	}
	if ([appDelegate.VideoSelectedType isEqualToString:@"1"]) {		
		strRating=1;
		[self RatingSearch];
	}
}

-(IBAction) btnFilterClcik
{
	if (!flgPicfilte) {
		picFilter.hidden=NO;
		tlbFilter.hidden=NO;
		flgPicfilte=TRUE;
	}
	else {
		flgPicfilte=FALSE;
		picFilter.hidden=YES;
		tlbFilter.hidden=YES;
	}	
}
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
	// Overriden to allow any orientation.
	if(interfaceOrientation == UIInterfaceOrientationLandscapeRight || interfaceOrientation == UIInterfaceOrientationLandscapeLeft)
		return YES;
	else if(interfaceOrientation == UIInterfaceOrientationPortrait)
		return NO;
	else 
		return NO;
}

- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc. that aren't in use.
}


- (void)viewDidUnload {
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}


- (void)dealloc {
    [super dealloc];
}


@end
