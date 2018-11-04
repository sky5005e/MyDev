//
//  History.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "History.h"
#import "xmlparsarhistory.h"
#import "Review_FrogAppDelegate.h"
#import "User.h"
#import "Historyview.h"
#import "MICheckBox.h"
#import "Terms_Condition.h"
#import "Review_FrogViewController.h"
#import "xmlDelete.h"
#import "PostedReview.h"
#import "HistoryLogin.h"
#import "AdminCoupon.h"
#import "SoundSwitch.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "OptionOfHistoryViewController.h"
#import "ReviewCategoryWiseListViewController.h"
#import "WriteORRecodeViewController.h"
#import "LoginInfo.h"
#import "WriteORRecodeViewController.h"

@implementation History
@synthesize tblp1;
@synthesize Historybutton;

@synthesize SysDate1;
@synthesize SysDate2;
@synthesize txtSearch;

@synthesize checkInPicker;
@synthesize checkOutPicker;

@synthesize date;
@synthesize pickeflag;
@synthesize pickeflag1;

@synthesize Fromdate;
@synthesize Todate;
@synthesize Acount;
@synthesize Icount;
@synthesize Dcount;
@synthesize ArrFilterRecord;
@synthesize strACount;
@synthesize strDCount;
@synthesize strICount;

@synthesize stronestar;
@synthesize strtwostar;
@synthesize strthreestar;
@synthesize strfourstar;
@synthesize strfivestar;
@synthesize flgreloadstop;
@synthesize strCategory;
@synthesize strStar; 

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
    [super viewDidLoad];
	
	picFilter.hidden=YES;
	tlbFilter.hidden=YES;
	lblFirst.hidden=NO;
	lblFrom.hidden=YES;
	
	
	appDelegate.HistoryList = [[NSMutableArray alloc] init];
	appDelegate.delArrSearchList= [[NSMutableArray alloc] init];
    self.strICount = @"0";
	self.strACount = @"0";
	self.strDCount = @"0";
	self.strfivestar = @"0";
	self.strfourstar = @"0";
	self.strthreestar = @"0";
	self.strtwostar = @"0";
	self.stronestar = @"0";
    
	post =[[PostedReview alloc]initWithFrame:CGRectMake(957, 221, 25, 25)];
	[post setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
	[self.view addSubview:post];
	
	
	SysDate2 = [[NSString alloc] init];
	appDelegate.search=[[NSString alloc]init];
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
	
	UIDatePicker *theDatePicker = [[UIDatePicker alloc] initWithFrame:CGRectMake(29, 205, 320.0, 216.0)];
    theDatePicker.datePickerMode = UIDatePickerModeDate;
    self.checkInPicker = theDatePicker;
    [theDatePicker release];
    [checkInPicker addTarget:self action:@selector(dateChangedIn) forControlEvents:UIControlEventValueChanged];
    
	UIDatePicker *theDatePickerOut = [[UIDatePicker alloc] initWithFrame:CGRectMake(212, 205, 320.0, 216.0)];
    theDatePickerOut.datePickerMode = UIDatePickerModeDate;
    self.checkOutPicker = theDatePickerOut;
    [theDatePickerOut release];
    
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
- (void)viewWillAppear:(BOOL)animated
{	
    if (!appDelegate.HViewClick) {
        appDelegate.HViewClick=FALSE;
        self.strICount = @"0";
        self.strACount = @"0";
        self.strDCount = @"0";
        self.strfivestar = @"0";
        self.strfourstar = @"0";
        self.strthreestar = @"0";
        self.strtwostar = @"0";
        self.stronestar = @"0";
    }
    
	//LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
	if (appDelegate.SelectedType) {
		Acount.hidden = YES;
		Icount.hidden = YES;
		Dcount.hidden = YES;
		
		lblAname.hidden = YES;
		lblDname.hidden = YES;
		lblIname.hidden = YES;
		
		btnPost.hidden = YES;
		btnNotPost.hidden = YES;
		btnInternal.hidden = YES;
        
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
    }
    
	/*if (alogin.imgBlogo!= nil) {
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
     }
     else {
     UIImage *img = [UIImage imageNamed:@"giftcard.png"];
     imgGiveAway.image = img;
     }
     }
     }*/
	
	if (appDelegate.SelectedType) {
		btnFilter.hidden=YES;
	}else {
        if (flgDateSearch){
            btnFilter.hidden=YES;
        }
        else{
            btnFilter.hidden=NO;
        }
	}
	[Historybutton setImage:[UIImage imageNamed:@"NhistoryD_v2.png"] forState:UIControlStateNormal];
	if (appDelegate.flgDismissview) {
		appDelegate.flgDismissview=FALSE;
        if(flgNewSearch){NSLog(@"Not for search");
            [self SearchFromDate];
        }else{
            int tempconter = [appDelegate.HistoryList count];
            for (int i = counter;i<tempconter; i++) {
                [appDelegate.HistoryList removeObjectAtIndex:counter];
            }
            [self showloding];
        }
		
	}
	
}
/*-(void)CountofStatus
 {	
 NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi.php",appDelegate.set_APIurl]];
 
 NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
 [request setHTTPMethod:@"POST"];
 NSData *requestBody = [[NSString stringWithFormat:@"api=countStatus&user_id=%@",appDelegate.userId] dataUsingEncoding:NSUTF8StringEncoding];
 [request setHTTPBody:requestBody];
 NSURLResponse *response = NULL;
 NSError *requestError = NULL;
 NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
 NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
 NSLog(@"responseString=%@",responseString);
 
 responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];	
 
 NSRange rngACountStart = [responseString rangeOfString:@"<Acount>"];
 if (rngACountStart.length > 0) {
 self.strACount = [responseString substringFromIndex:rngACountStart.location+rngACountStart.length];
 
 NSRange rngACountEnd = [self.strACount rangeOfString:@"</Acount>"];
 if (rngACountEnd.length > 0) {
 self.strACount = [self.strACount substringToIndex:rngACountEnd.location];
 NSLog(@"StrAccount:-%@",self.strACount);
 }		
 } else {
 self.strACount = @"0";
 }
 
 NSRange rngDCountStart = [responseString rangeOfString:@"<Dcount>"];
 if (rngDCountStart.length > 0) {
 self.strDCount = [responseString substringFromIndex:rngDCountStart.location+rngDCountStart.length];
 
 NSRange rngDCountEnd = [self.strDCount rangeOfString:@"</Dcount>"];
 if (rngDCountEnd.length > 0) {
 self.strDCount = [self.strDCount substringToIndex:rngDCountEnd.location];
 }		
 } else {
 self.strDCount = @"0";
 }
 
 NSRange rngICountStart = [responseString rangeOfString:@"<Icount>"];
 if (rngICountStart.length > 0) {
 self.strICount = [responseString substringFromIndex:rngICountStart.location+rngICountStart.length];
 
 NSRange rngICountEnd = [self.strICount rangeOfString:@"</Icount>"];
 if (rngICountEnd.length > 0) {
 self.strICount = [self.strICount substringToIndex:rngICountEnd.location];
 }		
 } else {
 self.strICount = @"0";
 }
 }
 */

/*-(void) StarCount
 {
 NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi.php",appDelegate.set_APIurl]];
 
 NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
 [request setHTTPMethod:@"POST"];
 NSData *requestBody = [[NSString stringWithFormat:@"api=itunesversion1_1_starwisereviewcount&user_id=%@",appDelegate.userId] dataUsingEncoding:NSUTF8StringEncoding];
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
 
 }
 */
- (void) showloding {
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
    // Add HUD to screen
    [self.view addSubview:HUD];
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
    HUD.labelText = @"Loading";
    // Show the HUD while the provided method executes in a new thread
    
    
    // if response 1 
    //    [HUD showWhileExecuting:@selector(getHistoryList) onTarget:self withObject:nil animated:YES];
    
    [HUD showWhileExecuting:@selector(getHistoryList) onTarget:self withObject:nil animated:YES];
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
	{	
		NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
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
		
	{	//DoneButton.hidden=TRUE; 
		NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
		[formatter setDateFormat:@"MM-dd-yyyy"];
		
		//Optionally for time zone converstions
		[formatter setTimeZone:[NSTimeZone timeZoneWithName:@"..."]];
		self.date = [checkOutPicker date];
		checkOuttxt.text = [formatter stringFromDate:self.date];
		NSLog(@"CheckIn Data----%@",checkOuttxt.text);
		
		NSLog(@"Check Text-----%@",checkOuttxt.text);
		[checkOutPicker removeFromSuperview]; 
		pickeflag1=FALSE;
		//DateDiff=FALSE;
		
		NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
		[dateFormatter setDateFormat:@"MM-dd-yyyy"];
		NSDate *Date1 = [dateFormatter dateFromString:checkIntxt.text];
		
		
		NSDateFormatter *dateFormatter1 = [[NSDateFormatter alloc] init];
		[dateFormatter1 setDateFormat:@"MM-dd-yyyy"];
		NSDate *Date2 = [dateFormatter1 dateFromString:checkOuttxt.text];
		
		
		NSComparisonResult result = [Date2 compare:Date1];
		
		if (result==NSOrderedAscending) {
			
			UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"The From Date Should Be Later Than The To Date" delegate:self 
												  cancelButtonTitle:@"OK" 
												  otherButtonTitles:nil];
			[alert show];
			[alert release];			
			NSLog(@"Date1 is in the future");
			
		}
		if(result==NSOrderedSame)
		{
			UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"The From Date And To Date Are Same" delegate:self 
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
    
	flgInactive=FALSE;
	flgactive=FALSE;
	flgDelete=FALSE;
	flgrate=FALSE;
	flgsearch = TRUE;
    flgNewSearch = TRUE;
	appDelegate.flgsearch=TRUE;
	btnFilter.hidden = YES;
	btnnext.hidden = YES;
	btnprevious.hidden = YES;
    flgDateSearch=TRUE;
	
	
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
	if (appDelegate.SelectedType)
	{
		str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_CategoryWiseSearch&startdate=%@&enddate=%@&user_id=%@&user_category=%@&user_categorytype=%@&version=2",appDelegate.ChangeUrl,Fromdate,Todate,appDelegate.userId,appDelegate.SelectedCategory,appDelegate.SelectedType];
	    str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	}else{
		
		str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_BetweenDateSearch&startdate=%@&enddate=%@&user_id=%@&version=2",appDelegate.ChangeUrl,Fromdate,Todate,appDelegate.userId];
		str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
        
	}	
	
	NSLog(@"URL String=%@",str);
	
	NSURL *url = [NSURL URLWithString:str];
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	NSLog(@"responseString:\n%@",responseString);			
	
	
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	xmlparsarhistory *parser = [[xmlparsarhistory alloc] initxmlparsarhistory];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<usertabledata><details><return1>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return1></details></usertabledata>"];
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<usertabledata><details><return1>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return1></details></usertabledata>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	
	if(success) {
		NSLog(@"No Errors");
		if([appDelegate.delArrSearchList count] ==0) {
			UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
													   cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[loginalert show];
			[loginalert release];
            
		}
		if ([responseString isEqualToString:@"null"]) {
			UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
													 cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[altError show];
			[altError release];		
			[appDelegate.delArrSearchList removeAllObjects];
		}
		[self.tblp1 reloadData];
        
	}
	else
	{
		NSLog(@"Error Error Error!!!");
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time test 2" delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
		
	}
    [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
	NSLog(@"cnt %d",[appDelegate.delArrSearchList count]);
	[pool release];
    
}
-(IBAction)RefreshHistorytable
{	
    if (appDelegate.SelectedType) {
        btnFilter.hidden=YES;
    }else {
        btnFilter.hidden=NO;
    }    
    if(flgNewSearch)        
    {   if(appDelegate.flgSelectedType){
        NSLog(@"Category wise serach is active");
    }else{
        appDelegate.SelectedType=@"";
        appDelegate.SelectedType=nil;}
        int tempconter = [appDelegate.HistoryList count];
		for (int i = counter;i<tempconter; i++) {
			[appDelegate.HistoryList removeObjectAtIndex:counter];
		}
        [self showloding];
    }
	checkIntxt.text =@"" ;
	checkOuttxt.text =@"";
    
	flgInactive = FALSE;
	flgactive = FALSE;
	flgDelete = FALSE;
	flgrate=FALSE;
    flgDateSearch=FALSE;
	appDelegate.flgsearch=FALSE;
    flgNewSearch=FALSE;
	
	btnnext.hidden = NO;
	btnprevious.hidden = NO;
	lblFirst.hidden=NO;
	lblFrom.hidden=YES;
	flgsearch=FALSE;
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
	//[self showloding];
	[self.tblp1 reloadData];
	
}


-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.SelectedType = nil;
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];

    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
}

-(IBAction) Admin
{
	appDelegate.SelectedType=@"";
	appDelegate.SelectedType = nil;
	if(appDelegate.LoginConform==FALSE)
        
	{	
        HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];
        
        [self presentModalViewController:historylogin animated:YES];
	}
	else
	{
		OptionOfHistoryViewController *objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
		[self presentModalViewController:objoption animated:YES];
	}
	
}

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.SelectedType=@"";
	appDelegate.SelectedType = nil;
	appDelegate.LoginConform=FALSE;
    Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];

}

-(IBAction) Backbtn_Click
{
	appDelegate.flgsearch=FALSE;
    // flgNewSearch=FALSE;
	if (appDelegate.SelectedType) {
        
		appDelegate.SelectedType=@"";
		appDelegate.SelectedType = nil;
        
        //[self dismissModalViewControllerAnimated:YES];
	    ReviewCategoryWiseListViewController *obj= [[ReviewCategoryWiseListViewController alloc]init];
        [self.navigationController pushViewController:obj animated:YES];
        [obj release];	
	}
	else {
		//[self dismissModalViewControllerAnimated:YES];
        OptionOfHistoryViewController *obj= [[OptionOfHistoryViewController alloc]init];
        [self.navigationController pushViewController:obj animated:YES];
        [obj release];
    }
}

-(IBAction) LogOut
{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *objWriteRecord = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];	
	[self presentModalViewController:objWriteRecord animated:YES];
    
}


- (void)viewWillDisappear:(BOOL)animated
{
	[Historybutton setImage:[UIImage imageNamed:@"Nhistory_v2.png"] forState:UIControlStateNormal];
}

-(void)getHistoryList
{
	
	NSAutoreleasePool* pool = [[NSAutoreleasePool alloc] init];
	
	NSString * str;
	if (appDelegate.SelectedType) 
	{			
		str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_ReviewCategoryRecord&UserID=%@&starrate=%@&category=%@&pagging=%d&version=2",appDelegate.ChangeUrl,appDelegate.userId,appDelegate.SelectedType,appDelegate.SelectedCategory,counter];
        str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	}
	else {	
		str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_fetchUserData&user_id=%@&pagging=%d&version=2",appDelegate.ChangeUrl,appDelegate.userId,counter];
        str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	}
	NSLog(@"URL String=%@",str);	
	NSURL *url = [NSURL URLWithString:str];
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    
    NSLog(@"Response String == %@",responseString);
	
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
    
    
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	xmlparsarhistory *parser = [[xmlparsarhistory alloc] initxmlparsarhistory];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];	
	
	
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<usertabledata><details><return1>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return1></details></usertabledata>"];
    
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<usertabledata><details><return1>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return1></details></usertabledata>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	
	if(success) {
		NSLog(@"No Errors");
		if([appDelegate.HistoryList count] ==0) {
			UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
													   cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[loginalert show];
			[loginalert release];
			
		}
		if ([responseString isEqualToString:@"null"]) {
			UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
													 cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[altError show];
			[altError release];			
		}
		
	}
	else
	{
		NSLog(@"Error Error Error!!!");
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time test 3 " delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
		
	}
	[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
	NSLog(@"cnt %d",[appDelegate.HistoryList count]);	
	[self performSelectorOnMainThread:@selector(ReloadTableData) withObject:nil waitUntilDone:YES];
	
	[pool release];
	
}
-(IBAction) btnNextClick
{
	flgInactive = FALSE;
	flgactive = FALSE;
	flgDelete = FALSE;
	flgrate=FALSE;
	appDelegate.flgsearch=FALSE;
    // flgNewSearch=FALSE;
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
	self.strICount = @"0";
	self.strACount = @"0";
	self.strDCount = @"0";
	self.strfivestar = @"0";
	self.strfourstar = @"0";
	self.strthreestar = @"0";
	self.strtwostar = @"0";
	self.stronestar = @"0";		
	
	
	counter+=25;
    NSLog(@"HistoryListConter:-%d",[appDelegate.HistoryList count]);
	if (counter>[appDelegate.HistoryList count]) {
		
		UIAlertView *alrmsg = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:
							   @"There is no more records."delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alrmsg show];
		[alrmsg release];
		
		counter-=25;
	}
	else {
		if (counter == [appDelegate.HistoryList count]) {
			[self showloding];
		}
		else {
			[self.tblp1 reloadData];
            
		}
        
	}
}
-(IBAction) btnPreviousClick
{
	flgInactive = FALSE;
	flgactive = FALSE;
	flgDelete = FALSE;
	flgrate=FALSE;
	appDelegate.flgsearch=FALSE;
    //  flgNewSearch=FALSE;
	[btnFilter setTitle:@"FILTER" forState:UIControlStateNormal]; 
    //	self.strICount = @"0";
    //	self.strACount = @"0";
    //	self.strDCount = @"0";
    //	self.strfivestar = @"0";
    //	self.strfourstar = @"0";
    //	self.strthreestar = @"0";
    //	self.strtwostar = @"0";
    //	self.stronestar = @"0";		
	
	if (counter==0) {
		UIAlertView *alrmsg = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:
							   @"There is no more records."delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alrmsg show];
		[alrmsg release];
		
	}
	else {
        
        int tempconter = [appDelegate.HistoryList count];
        for (int i = counter;i<tempconter; i++) {
            [appDelegate.HistoryList removeObjectAtIndex:counter];
            
        }
        
        counter-=25;				
        [self ReloadTableData];
        [picFilter reloadAllComponents];
        
	}
	
}

-(void) ReloadTableData
{	
	
	if (!appDelegate.SelectedType) {
        int I=0,A=0,D=0,fivestar=0,fourstar=0,threestar=0,twostar=0,onestar=0;
        for (int i = counter; i<[appDelegate.HistoryList count]; i++) {
            
            objSelectedUser1 = [appDelegate.HistoryList objectAtIndex:i];
            if ([objSelectedUser1.tableDatauserStatus isEqualToString:@"I"]) {			
                I++;
                self.strICount = [NSString stringWithFormat:@"%d",I];
                
            }
            if ([objSelectedUser1.tableDatauserStatus isEqualToString:@"A"])
            {
                A++;
                self.strACount = [NSString stringWithFormat:@"%d",A];
                
            }
            if ([objSelectedUser1.tableDatauserStatus isEqualToString:@"D"])
            {
                D++;
                self.strDCount = [NSString stringWithFormat:@"%d",D];
                
            }
            
            if(objSelectedUser1.tableDataRate==5) {
                fivestar++;
                self.strfivestar = [NSString stringWithFormat:@"%d",fivestar];
            }
            
            if(objSelectedUser1.tableDataRate==4) {
                fourstar++;
                self.strfourstar = [NSString stringWithFormat:@"%d",fourstar];
                
            }
            if(objSelectedUser1.tableDataRate==3) {
                threestar++;
                self.strthreestar = [NSString stringWithFormat:@"%d",threestar];
                
            }
            if(objSelectedUser1.tableDataRate==2) {
                twostar++;
                self.strtwostar = [NSString stringWithFormat:@"%d",twostar];
                
            }
            if(objSelectedUser1.tableDataRate ==1) {
                onestar++;
                self.stronestar = [NSString stringWithFormat:@"%d",onestar];
                
            }
            
        }
    }
	NSLog(@"striCount:-%@",self.strICount);
	NSLog(@"strAcount:-%@",self.strACount);
	NSLog(@"strDCount:-%@",self.strDCount);
	NSLog(@"self.strtFivetar:-%@",self.strfivestar);
	NSLog(@"self.strfourstar:-%@",self.strfourstar);
	NSLog(@"self.strthreestar:-%@",self.strthreestar);
	NSLog(@"self.strtwostar:-%@",self.strtwostar);
	NSLog(@"self.stronestar:-%@",self.stronestar);
	
	/*if (appDelegate.SelectedType) {
     [self RatewiseSort];
     
     }*/
	[picFilter reloadAllComponents];
	[self.tblp1 reloadData];
}
- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
	
	return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	
	if (flgInactive==TRUE) {
		
        
        Count = [appDelegate.ArrInactive count]; 
		return [appDelegate.ArrInactive count];
	}
	if (flgactive==TRUE) {	
        
		
		Count = [appDelegate.ArrActive count];
		return [appDelegate.ArrActive count];
	}
	if (flgDelete==TRUE) {
		
		Count = [appDelegate.ArrDelete count];
		return [appDelegate.ArrDelete count];
	}
	if (flgrate==TRUE) {
		Count = [self.ArrFilterRecord count];
		return [self.ArrFilterRecord count];
	}
	if (appDelegate.flgsearch==TRUE) {
		Count = [appDelegate.delArrSearchList count];
		return [appDelegate.delArrSearchList count];
	}
    else {
        
		if (([appDelegate.HistoryList count] - counter)<25) {
			NSLog(@"Hisotory Counter - %d",[appDelegate.HistoryList count]);
			return ([appDelegate.HistoryList count]-counter);
		}	
		return 25;
	}
    
    
}

- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath
{
	return 60.0;
}


- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
	
	static NSString *CellIdentifier = @"Cell";
	
	UITableViewCell *cell = [self.tblp1 dequeueReusableCellWithIdentifier:CellIdentifier];
	if (cell == nil) {
		cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];
	}
	
	// Set up the cell...
	cellHistoy *objCell = [[cellHistoy alloc] init];
	objCell.delegate = self;
	[cell.contentView addSubview:objCell.view];
	if (flgInactive==TRUE) {
        objSelectedUser1 = [appDelegate.ArrInactive objectAtIndex:indexPath.row];
	}
	else if (flgactive==TRUE) {
        
		objSelectedUser1 = [appDelegate.ArrActive objectAtIndex:indexPath.row];
	}
	else if (flgDelete==TRUE) {
		objSelectedUser1 = [appDelegate.ArrDelete objectAtIndex:indexPath.row];
	}
	else if (flgrate==TRUE) {
		
		objSelectedUser1 = [self.ArrFilterRecord objectAtIndex:indexPath.row];
	}
	else if (appDelegate.flgsearch==TRUE) {
		objSelectedUser1= [appDelegate.delArrSearchList objectAtIndex:indexPath.row];
	}
	else {
		objSelectedUser1 = [appDelegate.HistoryList objectAtIndex:indexPath.row+counter];
	}
	int SNO=indexPath.row+counter;
	objCell.lblNO.text = [NSString stringWithFormat:@"%d",SNO+1];
	
	objCell.lblName.text = objSelectedUser1.tableDataName;
	objCell.lblEmail.text = objSelectedUser1.tableDataEmail;
    
	objCell.lblSDate.text= objSelectedUser1.tableDataDate;
	objCell.lblSource.text= objSelectedUser1.tableDataFrom;
	objCell.btnView.tag = indexPath.row;
	objCell.btnDelete.tag = indexPath.row;
	NSLog(@"Rate of ReviewFrog:-%f",objSelectedUser1.tableDataRate);
    
	
	if ([objSelectedUser1.tableDatauserStatus isEqualToString:@"I"]) {
		NSLog(@"I Counter");
		objCell.lbltype.textColor=[UIColor grayColor];
		objCell.lbltype.text =@"UNREAD"; 
		
	} else if([objSelectedUser1.tableDatauserStatus isEqualToString:@"A"])
	{
		objCell.lbltype.textColor=[UIColor greenColor];
		
		objCell.lbltype.text = @"PUBLISHED";
        
	}
	else if([objSelectedUser1.tableDatauserStatus isEqualToString:@"D"])
	{
		objCell.lbltype.textColor=[UIColor redColor];
		objCell.lbltype.text = @"INTERNAL";
	}
	float fltRating = floor(objSelectedUser1.tableDataRate);
	float diffValue = ((objSelectedUser1.tableDataRate)-fltRating);
	int x = 75, y = 20, w = 21, h = 21;
	
	if (objSelectedUser1.tableDataRate < 0.4) {
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
	
    
    
	cell.selectionStyle = UITableViewCellSelectionStyleNone;
	
	return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
	
}

- (void)viewClicked:(int)selectedIndex {
	NSLog(@"index %d",selectedIndex);
	
	if (flgInactive==TRUE) {
		
		User *objSelectedUser =[appDelegate.ArrInactive objectAtIndex:selectedIndex];
		Historyview *objHView = [[Historyview alloc] initWithNibName:@"Historyview" bundle:nil];
		objHView.selectedUser = objSelectedUser;
		
		[self presentModalViewController:objHView animated:YES];
		
	}
	else if (flgactive==TRUE) {
		User *objSelectedUser =[appDelegate.ArrActive objectAtIndex:selectedIndex];
		Historyview *objHView = [[Historyview alloc] initWithNibName:@"Historyview" bundle:nil];
		objHView.selectedUser = objSelectedUser;
		
		[self presentModalViewController:objHView animated:YES];
	}
	else if (flgDelete==TRUE) {
		User *objSelectedUser =[appDelegate.ArrDelete objectAtIndex:selectedIndex];
		Historyview *objHView = [[Historyview alloc] initWithNibName:@"Historyview" bundle:nil];
		objHView.selectedUser = objSelectedUser;
		
		[self presentModalViewController:objHView animated:YES];
	}
	else if(flgrate==TRUE) {
		User *objSelectedUser =[self.ArrFilterRecord objectAtIndex:selectedIndex];
		Historyview *objHView = [[Historyview alloc] initWithNibName:@"Historyview" bundle:nil];
		objHView.selectedUser = objSelectedUser;
		
		[self presentModalViewController:objHView animated:YES];
		
	}
	else if (appDelegate.flgsearch==TRUE) {
		User *objSelectedUser =[appDelegate.delArrSearchList objectAtIndex:selectedIndex];
		Historyview *objHView = [[Historyview alloc] initWithNibName:@"Historyview" bundle:nil];
		objHView.selectedUser = objSelectedUser;		
		[self presentModalViewController:objHView animated:YES];
        
	}
    
	else {
		User *objSelectedUser =[appDelegate.HistoryList objectAtIndex:selectedIndex+counter];
		Historyview *objHView = [[Historyview alloc] initWithNibName:@"Historyview" bundle:nil];
		objHView.selectedUser = objSelectedUser;		
		[self presentModalViewController:objHView animated:YES];
	}
    
}

- (void)deleteClicked:(int)selectedIndex {
	NSLog(@"index %d",selectedIndex);
	objSelectedUser1 =[appDelegate.HistoryList objectAtIndex:selectedIndex];
	
	alertConfirm = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are You Sure Want To Delete?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes",nil];
	[alertConfirm show];
	[alertConfirm release];
    
}

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
	}
	
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex {
	if (alertView == alertConfirm) {
		if (buttonIndex == 0) {
			NSLog(@"No");
		} else {
			[self DataDelete:objSelectedUser1];
			[self getHistoryList];
			[self.tblp1 reloadData];			
		}
	}
}
-(void) DataDelete :(User *)user
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php&version=2",appDelegate.ChangeUrl]];
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	[request setHTTPMethod:@"POST"];
	NSData *requestBody = [[NSString stringWithFormat:@"api=delUserData&user_id=%d",user.tableDataID] dataUsingEncoding:NSUTF8StringEncoding];
	[request setHTTPBody:requestBody];
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	NSLog(@"responseString=%@",responseString);
    [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:responseData];
	
	//Initialize the delegate.
	xmlDelete *parser = [[xmlDelete alloc] initxmlDelete];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success)
	{
        
		NSLog(@"No Errors");
	}
	else
	{
		NSLog(@"Error Error Error!!!");
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time test1" delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
		
	}
	[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
}
-(IBAction)StatisticsClick
{
	checkIntxt.text =@"" ;
	checkOuttxt.text =@"";
	flgInactive = FALSE;
	flgactive = FALSE;
	flgDelete = FALSE;
	
	ReviewCategoryWiseListViewController *objCategoryList = [[ReviewCategoryWiseListViewController alloc]init];
	[self presentModalViewController:objCategoryList animated:YES];
	
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


- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
	return 1;
}

- (NSInteger)pickerView:(UIPickerView *)thePickerView numberOfRowsInComponent:(NSInteger)component {	
	return 8;
}

- (UIView *)pickerView:(UIPickerView *)pickerView viewForRow:(NSInteger)row
		  forComponent:(NSInteger)component reusingView:(UIView *)view
{
	if (row==0) {
		UILabel *NameLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, 100, 46)];
		NameLabel.text = @"PUBLISHED";
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
	
	if (row==1) {
		
		UILabel *NameLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, 100, 46)];
		NameLabel.text = @"UNREAD";
		NameLabel.textAlignment = UITextAlignmentLeft;
		NameLabel.backgroundColor = [UIColor clearColor];
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		countLabel.text = self.strICount;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:NameLabel atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	if (row==2) {
		UILabel *NameLabel = [[UILabel alloc] initWithFrame:CGRectMake(0, 0, 100, 46)];
		NameLabel.text = @"INTERNAL";
		NameLabel.textAlignment = UITextAlignmentLeft;
		NameLabel.backgroundColor = [UIColor clearColor];
		
		UILabel *countLabel = [[UILabel alloc] initWithFrame:CGRectMake(250, 0, 50, 46)];
		NSLog(@"strDccount:-%@",self.strDCount);
		countLabel.text = self.strDCount;
		countLabel.textAlignment = UITextAlignmentLeft;
		countLabel.backgroundColor = [UIColor clearColor];		
		UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
		[tmpView insertSubview:NameLabel atIndex:0];
		[tmpView insertSubview:countLabel atIndex:0];
		return tmpView;
		
	}
	if (row==3) {
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
	if (row==4) {
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
    
	if (row==5) {
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
    
	if (row==6) {
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
    
	if (row==7) {
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
	if (row==7) {
		index=row;	
	}
	
}

-(IBAction)btnDoneClick
{
	
	picFilter.hidden=YES;
	tlbFilter.hidden=YES;
	lblFirst.hidden=YES;
	lblFrom.hidden=NO;
	flgPicfilte=FALSE;	
	appDelegate.flgsearch=FALSE;
    //flgNewSearch=FALSE;
	
	if (index==0) {
		
		[self Active_click];
		[btnFilter setTitle:[NSString stringWithFormat:@"PUBLISHED            %@",self.strACount] forState:UIControlStateNormal]; 
		[self.tblp1 reloadData];
		
	}
	if (index==1) {		
		[self Inactive_click];
		[btnFilter setTitle:[NSString stringWithFormat:@"UNREAD               %@",self.strICount] forState:UIControlStateNormal]; 
		[self.tblp1 reloadData];
	}
	if (index==2) {		
		[self Internal_Click];
		[btnFilter setTitle:[NSString stringWithFormat:@"INTERNAL             %@",self.strDCount] forState:UIControlStateNormal]; 
		[self.tblp1 reloadData];
	}
	if (index==3) {		
		strRating=5;
		[self RatingSearch];
		[btnFilter setTitle:[NSString stringWithFormat:@"5Star                %@",self.strfivestar] forState:UIControlStateNormal]; 
		[self.tblp1 reloadData];
	}
	if (index==4) {		
		strRating=4;
		[self RatingSearch];
		[btnFilter setTitle:[NSString stringWithFormat:@"4Star                %@",self.strfourstar] forState:UIControlStateNormal];
		[self.tblp1 reloadData];
	}
	if (index==5) {		
		strRating=3;
		[self RatingSearch];
		[btnFilter setTitle:[NSString stringWithFormat:@"3Star                %@",self.strthreestar] forState:UIControlStateNormal]; 
		[self.tblp1 reloadData];
	}
	if (index==6) {		
		strRating=2;
		[self RatingSearch];
		[btnFilter setTitle:[NSString stringWithFormat:@"2Star                %@",self.strtwostar] forState:UIControlStateNormal];
		[self.tblp1 reloadData];
	}
	if (index==7) {		
		strRating=1;
		[self RatingSearch];
		
		[btnFilter setTitle:[NSString stringWithFormat:@"1Star                %@",self.stronestar] forState:UIControlStateNormal];
		[self.tblp1 reloadData];
	}
	
	
}
-(void) Inactive_click
{	
	appDelegate.ArrInactive = [[NSMutableArray alloc]init];
	appDelegate.SelectedType=@"";
	appDelegate.SelectedType = nil;
	flgInactive=TRUE;
	flgactive=FALSE;
	flgDelete=FALSE;
	flgrate=FALSE;
	for (int i=counter; i<[appDelegate.HistoryList count]; i++) {
		User *objHistory = [appDelegate.HistoryList objectAtIndex:i];
		
		if ([objHistory.tableDatauserStatus isEqualToString:@"I" ])
		{
			[appDelegate.ArrInactive addObject:objHistory];
		}
	}
	NSLog(@"appDelegate.ArrInactive:%d",[appDelegate.ArrInactive count]);
	
	[appDelegate.ArrInactive count];
	
}

-(void) Active_click
{	
	appDelegate.ArrActive = [[NSMutableArray alloc]init];
    
	appDelegate.SelectedType=@"";
	appDelegate.SelectedType = nil;
	
	flgactive=TRUE;
	flgInactive=FALSE;
	flgDelete=FALSE;
	flgrate=FALSE;
	for (int i=counter; i<[appDelegate.HistoryList count]; i++) {
		User *objHistory = [appDelegate.HistoryList objectAtIndex:i];
		
		if ([objHistory.tableDatauserStatus isEqualToString:@"A" ])
		{
			[appDelegate.ArrActive addObject:objHistory];
		}
	}
	NSLog(@"appDelegate.ArrActive:%d",[appDelegate.ArrActive count]);
    //	self.strACount = [NSString stringWithFormat:@"%d",[appDelegate.ArrActive count]];
	[appDelegate.ArrActive count];
	//[self performSelectorOnMainThread:@selector(ReloadTableData) withObject:nil waitUntilDone:YES];
}

-(void) Internal_Click
{
	appDelegate.ArrDelete = [[NSMutableArray alloc]init];
    
	appDelegate.SelectedType=@"";
	appDelegate.SelectedType = nil;
	
	flgDelete=TRUE;
	flgactive=FALSE;
	flgInactive=FALSE;
	flgrate=FALSE;
	for (int i=counter; i<[appDelegate.HistoryList count]; i++) {
		User *objHistory = [appDelegate.HistoryList objectAtIndex:i];
		
		if ([objHistory.tableDatauserStatus isEqualToString:@"D" ])
		{
			[appDelegate.ArrDelete addObject:objHistory];
		}
	}
	NSLog(@"appDelegate.ArrDelete:%d",[appDelegate.ArrDelete count]);
    
	[appDelegate.ArrDelete count];
	[self.tblp1 reloadData];
	//[self performSelectorOnMainThread:@selector(ReloadTableData) withObject:nil waitUntilDone:YES];
	
	
}
-(void)RatingSearch
{
	flgDelete=FALSE;
	flgactive=FALSE;
	flgInactive=FALSE;
	flgrate=TRUE;
	self.ArrFilterRecord = [[NSMutableArray alloc]init];
    NSLog(@"CounterHistory:-%d",[appDelegate.HistoryList count]);
	for (int i=counter; i<[appDelegate.HistoryList count]; i++) {
		User *objHistory = [appDelegate.HistoryList objectAtIndex:i];
		
		float flatRating = floor(objHistory.tableDataRate);
		
		if (flatRating == strRating) {
			if (appDelegate.SelectedType) {
				if ([objHistory.tableDataCategory isEqualToString:appDelegate.SelectedCategory]) {
					[self.ArrFilterRecord addObject:objHistory];
				}
			}
			else {
                [self.ArrFilterRecord addObject:objHistory];
				
			}
		}
	}
	NSLog(@"ArrFilterRecord:-%d",[self.ArrFilterRecord count]);
	
    /*	if ([self.ArrFilterRecord count]==0) {
     UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
     [alert show];
     [alert release];
     
     }*/
}

-(void) RatewiseSort
{
	if ([appDelegate.SelectedType isEqualToString:@"5"]) {		
		strRating=5;
		[self RatingSearch];
	}
	if ([appDelegate.SelectedType isEqualToString:@"4"]) {		
		strRating=4;
		[self RatingSearch];
	}
	if ([appDelegate.SelectedType isEqualToString:@"3"]) {		
		strRating=3;
		[self RatingSearch];
	}
	if ([appDelegate.SelectedType isEqualToString:@"2"]) {		
		strRating=2;
		[self RatingSearch];
	}
	if ([appDelegate.SelectedType isEqualToString:@"1"]) {		
		strRating=1;
		[self RatingSearch];
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
    
    // Release any cached data, images, etc that aren't in use.
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
