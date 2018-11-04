    //
//  ReviewCategoryWiseListViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 8/29/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "ReviewCategoryWiseListViewController.h"
#import "XMLCategoryList.h"
#import "CellCategoryList.h"
#import "CategoryWiseList.h"
#import "History.h"
#import "HistoryLogin.h"
#import "ApplicationValidation.h"
#import "Terms_Condition.h"
#import "ThreeTabButton.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"
#import "xmlparsarhistory.h"
#import "CategoryViewController.h"

@implementation ReviewCategoryWiseListViewController
@synthesize tblCategoryList;

@synthesize btnfiveStar;
@synthesize btnFourStar;
@synthesize btnThreeStar;
@synthesize btnTwoStar;
@synthesize btnOneStar;





// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	
	[self LoadingStart];
	//[self Showhistorylist];
	
}
-(void)viewWillAppear:(BOOL)animated
{
    imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:appDelegate.pathBussinesslogo] autorelease];

    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
    }
}
/*- (void) Showhistorylist {
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(getHistoryList) onTarget:self withObject:nil animated:YES];
}
-(void)getHistoryList
{
	
	NSAutoreleasePool* pool = [[NSAutoreleasePool alloc] init];
	
	NSString * str;
	
	str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi.php?api=itunesversion1_1_Hotel_fetchUserData&user_id=%@",appDelegate.set_APIurl,appDelegate.userId];
	
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
	//<?xml version="1.0" encoding="UTF-8" ?>
	//<userdata><return>null</return></userdata>
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<usertabledata><details><return1>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return1></details></usertabledata>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	
	if(success) {
		NSLog(@"No Errors");
		[self CategoryCount];
	}
	else
	{
		NSLog(@"Error Error Error!!!");
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
		
	}
	[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
	NSLog(@"cnt %d",[appDelegate.HistoryList count]);
	

	[pool release];
	
}*/


- (void) LoadingStart
{
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(GetCategoryList) onTarget:self withObject:nil animated:YES];
	

}
-(void) GetCategoryList
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	NSString *str = [[NSString alloc] initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_ReviewCategoryCounter&UserID=%@&version=2",appDelegate.ChangeUrl,appDelegate.userId];
	
	NSLog(@"URL String:-%@",str);
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	XMLCategoryList *parser = [[XMLCategoryList alloc] initXMLCategoryList];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		NSLog(@"No Errors");
		[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
		[self performSelectorOnMainThread:@selector(LoadTableData) withObject:nil waitUntilDone:YES];
			
	}	
	else
	{
		NSLog(@"Error Error Error!!!");
		
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
		
	}
	
	[pool release];
	
}
-(void)LoadTableData
{
	[self.tblCategoryList reloadData];
}
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	NSLog(@"CategoryWiseList:-%d",[appDelegate.delArrCategoryWiseList count]);
	return [appDelegate.delArrCategoryWiseList count];
}
- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	return 60.0;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	UITableViewCell *Cell = [self.tblCategoryList dequeueReusableCellWithIdentifier:CellIdentifier];
	
	Cell =[[[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier]autorelease];
	
	//set Cell
	CellCategoryList *objcell = [[CellCategoryList alloc]init];
	[Cell.contentView addSubview:objcell.view];
	
	objcategoryWiseList = [appDelegate.delArrCategoryWiseList objectAtIndex:indexPath.row];
	
	objcell.lblCategory.text = objcategoryWiseList.name;
	
	
	int one = [objcategoryWiseList.onestar integerValue];
	int two = [objcategoryWiseList.twostar integerValue];
	int three = [objcategoryWiseList.threestar integerValue];
	int four = [objcategoryWiseList.fourstar integerValue];
	int five = [objcategoryWiseList.fivestar integerValue];
	
	float avgsum = (float)(one*1+two*2+three*3+four*4+five*5);
	
	//float avgsum = (float)(one+two+three+four+five);
	float ToT = [objcategoryWiseList.CategoryWiseTotal integerValue];
	float avg = (avgsum/ToT);
	NSLog(@"avg:-%f",avg);
	
	//objcell.lblTotal.text = [NSString stringWithFormat:@"%.0f",avgsum];
	objcell.lblTotal.text = objcategoryWiseList.CategoryWiseTotal;
	float fltRating = floor(avg);
	float diffValue = ((avg)-fltRating);
	int x = 841, y = 20, w = 21, h = 21;
	
	if (avg < 0.4) {
		CGRect rect = CGRectMake(x, y, w, h);
		objcell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
		[objcell.imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
		[Cell.contentView addSubview:objcell.imghalfRating];
		[objcell.imghalfRating release];
		x += w;
		fltRating +=1;
		
	} else {
		for (int i=0; i<fltRating; i++) {
			CGRect rect = CGRectMake(x, y, w, h);
			objcell.imgGivenRating = [[UIImageView alloc] initWithFrame:rect];
			[objcell.imgGivenRating setImage:[UIImage imageNamed:@"Fullyellow25X25.png"]];
			[Cell.contentView addSubview:objcell.imgGivenRating];
			[objcell.imgGivenRating release];
			x += w;
		}
		if (diffValue > 0 && diffValue < 0.4) 
		{
			CGRect rect = CGRectMake(x, y, w, h);
			objcell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[objcell.imghalfRating setImage:[UIImage imageNamed:@"Fullyellow25X25.png"]];
			[Cell.contentView addSubview:objcell.imghalfRating];
			[objcell.imghalfRating release];
			x += w;
			fltRating +=1;
			
		} 		
		for (int j=fltRating; j<5; j++) {
			CGRect rect = CGRectMake(x, y, w, h);
			objcell.imgNoRating = [[UIImageView alloc] initWithFrame:rect];
			[objcell.imgNoRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[Cell.contentView addSubview:objcell.imgNoRating];
			[objcell.imgNoRating release];
			x += w;
		}		
	}
	btnOneStar =[[UIButton alloc]init];
	btnOneStar = [UIButton buttonWithType:UIButtonTypeCustom];
	[btnOneStar setFrame:CGRectMake(400, 13, 75, 37)];
	btnOneStar.tag = indexPath.row;
	[btnOneStar setTitleColor:[UIColor orangeColor] forState:UIControlStateNormal];
	[btnOneStar setFont:[UIFont boldSystemFontOfSize:17]];
	if (objcategoryWiseList.onestar) {
		[btnOneStar setTitle:objcategoryWiseList.onestar forState:UIControlStateNormal];	
	}
	else {
			[btnOneStar setTitle:@"0"forState:UIControlStateNormal];
	}
	[btnOneStar addTarget:self action:@selector(OneStarClick:)forControlEvents:UIControlEventTouchUpInside];
	[Cell.contentView addSubview:btnOneStar];
	
	
	
	btnTwoStar =[[UIButton alloc]init];
	btnTwoStar = [UIButton buttonWithType:UIButtonTypeCustom];
	[btnTwoStar setFrame:CGRectMake(488, 13, 75, 37)];
	btnTwoStar.tag = indexPath.row;
	[btnTwoStar setTitleColor:[UIColor orangeColor] forState:UIControlStateNormal];
	[btnTwoStar setFont:[UIFont boldSystemFontOfSize:17]];
	
	if (objcategoryWiseList.twostar) {
		[btnTwoStar setTitle:objcategoryWiseList.twostar forState:UIControlStateNormal];	
	}
	else {
		[btnTwoStar setTitle:@"0" forState:UIControlStateNormal];
	}
	[btnTwoStar addTarget:self action:@selector(TwoStarClick:)forControlEvents:UIControlEventTouchUpInside];
	[Cell.contentView addSubview:btnTwoStar];
	
	
	
	btnThreeStar =[[UIButton alloc]init];
	btnThreeStar = [UIButton buttonWithType:UIButtonTypeCustom];
	[btnThreeStar setFrame:CGRectMake(576, 13, 75, 37)];
	btnThreeStar.tag = indexPath.row;	
	[btnThreeStar setTitleColor:[UIColor orangeColor] forState:UIControlStateNormal];
	[btnThreeStar setFont:[UIFont boldSystemFontOfSize:17]];
	if (objcategoryWiseList.threestar) {
		[btnThreeStar setTitle:objcategoryWiseList.threestar forState:UIControlStateNormal];	
	}
	else {
		[btnThreeStar setTitle:@"0" forState:UIControlStateNormal];
	}	
	[btnThreeStar addTarget:self action:@selector(ThreeStarClick:)forControlEvents:UIControlEventTouchUpInside];
	[Cell.contentView addSubview:btnThreeStar];
	
	btnFourStar =[[UIButton alloc]init];
	btnFourStar = [UIButton buttonWithType:UIButtonTypeCustom];
	[btnFourStar setFrame:CGRectMake(664, 13, 75, 37)];
	btnFourStar.tag = indexPath.row;	
	[btnFourStar setTitleColor:[UIColor orangeColor] forState:UIControlStateNormal];
	[btnFourStar setFont:[UIFont boldSystemFontOfSize:17]];
	if (objcategoryWiseList.fourstar) {
		[btnFourStar setTitle:objcategoryWiseList.fourstar forState:UIControlStateNormal];	
	}
	else {
		[btnFourStar setTitle:@"0" forState:UIControlStateNormal];
	}	
	[btnFourStar addTarget:self action:@selector(FourStarClick:)forControlEvents:UIControlEventTouchUpInside];
	[Cell.contentView addSubview:btnFourStar];
	
	btnfiveStar =[[UIButton alloc]init];
	btnfiveStar = [UIButton buttonWithType:UIButtonTypeCustom];
	[btnfiveStar setFrame:CGRectMake(752, 13, 75, 37)];
	btnfiveStar.tag = indexPath.row;	
	[btnfiveStar setTitleColor:[UIColor orangeColor] forState:UIControlStateNormal];
	[btnfiveStar setFont:[UIFont boldSystemFontOfSize:17]];
	if (objcategoryWiseList.fivestar) {
		[btnfiveStar setTitle:objcategoryWiseList.fivestar forState:UIControlStateNormal];	
	}
	else {
		[btnfiveStar setTitle:@"0" forState:UIControlStateNormal];
	}	
	[btnfiveStar addTarget:self action:@selector(FiveStarClick:)forControlEvents:UIControlEventTouchUpInside];
	[Cell.contentView addSubview:btnfiveStar];
	
	Cell.selectionStyle = UITableViewCellSelectionStyleNone;
	
	return Cell;
}
-(void)OneStarClick :(int)index
{
	btnOneStar = (UIButton *)index;
	NSLog(@"btnOneStar-----%d",btnOneStar.tag);

	if ([btnOneStar.currentTitle isEqualToString:@"0"]) {
		UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
												   cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[loginalert show];
		[loginalert release];
	}
	else {
		
		objcategoryWiseList = [appDelegate.delArrCategoryWiseList objectAtIndex:btnOneStar.tag];
		appDelegate.SelectedCategory = objcategoryWiseList.name;
		appDelegate.SelectedType = @"1";
		
		History *history= [[History alloc]init];
		history.flgreloadstop = TRUE;
		history.strStar=@"1";
		history.strCategory=objcategoryWiseList.name;
        [self.navigationController pushViewController:history animated:YES];
        [history release];			
	}

}
-(void)TwoStarClick :(int)index
{
	btnTwoStar = (UIButton *)index;
	NSLog(@"btnNotPosted-----%d",btnTwoStar.tag);

	if ([btnTwoStar.currentTitle isEqualToString:@"0"]) {
		UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
												   cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[loginalert show];
		[loginalert release];
	}
	else {
		
	objcategoryWiseList = [appDelegate.delArrCategoryWiseList objectAtIndex:btnTwoStar.tag];
	appDelegate.SelectedCategory = objcategoryWiseList.name;
	appDelegate.SelectedType = @"2";
	
	History *history= [[History alloc]init];
	history.flgreloadstop = TRUE;
	history.strStar=@"2";
	history.strCategory=objcategoryWiseList.name;
    [self.navigationController pushViewController:history animated:YES];
    [history release];	
	}
}
-(void)ThreeStarClick :(int)index
{
	btnThreeStar = (UIButton *)index;
	NSLog(@"btnInternal-----%d",btnThreeStar.tag);
	
	
	if ([btnThreeStar.currentTitle isEqualToString:@"0"]) {
		UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
												   cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[loginalert show];
		[loginalert release];
	}else{	
	objcategoryWiseList = [appDelegate.delArrCategoryWiseList objectAtIndex:btnThreeStar.tag];
	appDelegate.SelectedCategory = objcategoryWiseList.name;
	appDelegate.SelectedType = @"3";
	
	History *history= [[History alloc]init];
	history.flgreloadstop = TRUE;
	history.strStar=@"3";
	history.strCategory=objcategoryWiseList.name;
    [self.navigationController pushViewController:history animated:YES];
    [history release];
	}
}
-(void)FourStarClick :(int)index
{
	btnFourStar = (UIButton *)index;
	NSLog(@"btnInternal-----%d",btnFourStar.tag);
	
	
	if ([btnFourStar.currentTitle isEqualToString:@"0"]) {
		UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
												   cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[loginalert show];
		[loginalert release];
	}else {	
	objcategoryWiseList = [appDelegate.delArrCategoryWiseList objectAtIndex:btnFourStar.tag];
	appDelegate.SelectedCategory = objcategoryWiseList.name;
	appDelegate.SelectedType = @"4";
	
	History *history= [[History alloc]init];
	history.flgreloadstop = TRUE;
	history.strStar=@"4";
	history.strCategory=objcategoryWiseList.name;
    [self.navigationController pushViewController:history animated:YES];
    [history release];
	}
}
-(void)FiveStarClick :(int)index
{
	btnfiveStar = (UIButton *)index;
	NSLog(@"btnInternal-----%d",btnfiveStar.tag);

	if ([btnfiveStar.currentTitle isEqualToString:@"0"]) {
		UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! No Reviews Found." delegate:self
												   cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[loginalert show];
		[loginalert release];
	}else {
	objcategoryWiseList = [appDelegate.delArrCategoryWiseList objectAtIndex:btnfiveStar.tag];
	appDelegate.SelectedCategory = objcategoryWiseList.name;
	appDelegate.SelectedType = @"5";
	
	History *history= [[History alloc]init];
	history.flgreloadstop = TRUE;
	history.strStar=@"5";
	history.strCategory=objcategoryWiseList.name;
    [self.navigationController pushViewController:history animated:YES];
    [history release];
	}
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
	[tableView deselectRowAtIndexPath:indexPath animated:YES];
}
-(IBAction)BackClick
{
	appDelegate.SelectedType=@"";
	appDelegate.SelectedType = nil;
    appDelegate.flgSelectedType=FALSE;
    OptionOfHistoryViewController *obj= [[OptionOfHistoryViewController alloc]init];
    [self.navigationController pushViewController:obj animated:YES];
    [obj release];	
}
-(IBAction)Home
{
	appDelegate.SelectedType=@"";
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
	appDelegate.SelectedType=@"";
	appDelegate.SelectedType = nil;
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}
-(IBAction) AddCategoryClick
{
	CategoryViewController *objaddcategory = [[CategoryViewController alloc]initWithNibName:@"CategoryViewController" bundle:nil];
    objaddcategory.flag_Category=TRUE;
    [self.navigationController pushViewController:objaddcategory animated:YES];
    [objaddcategory release];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
	
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
