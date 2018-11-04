    //
//  BusinessSearchListViewController.m
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BusinessSearchListViewController.h"
#import "Search.h"
#import "CellSearch.h"
#import "XMLSearchResult.h"
#import "BusinessSearchResultViewController.h"
#import "LocalBusinesSearchViewController.h"
#import "XMLSearch.h"
#import "XMLBusinessCategory.h"
#import "BusinessCategory.h"
#import "WebclaimyourbusinessViewController.h"

@implementation BusinessSearchListViewController
@synthesize tblSearch;
@synthesize selectedArrSearch;
@synthesize businessid;
@synthesize strCategory;
@synthesize strCity;

- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	lblNoRecord.hidden=YES;
    picCategory.hidden=YES;
    tlbCategory.hidden=YES;
   txtCategory.text=strCity;
   txtCategory.text=strCategory;
}
-(void)viewWillAppear:(BOOL)animated
{
	if (appDelegate.flgBack==TRUE) {
	NSLog(@"BackClick");
		appDelegate.flgBack=FALSE;
	}
	else {
		
	if (appDelegate.flgDismiss) {
		appDelegate.flgDismiss=FALSE;
		[lblCity removeFromSuperview];
		[lblCategory removeFromSuperview];

		strCategory=appDelegate.strCategory;
		strCity=appDelegate.strCity;
	}	
	
	picCategory.hidden=YES;
	tlbCategory.hidden=YES;

	if ([appDelegate.delArrSearch count]==0)
	{	
		lblNoRecord.hidden=NO;
		lblResultNot.hidden=YES;
		tblSearch.hidden=YES;
	}
	else{	
		
	lblNoRecord.hidden=YES;
	lblResultNot.hidden=NO;
		tblSearch.hidden=NO;
		lblCategory.hidden=NO;
		lblCity.hidden=NO;
		
	
	txtCategory.text = strCategory;
		txtCity.text = strCity;
	
	//Add Category Label
	lblCategory = [[UILabel alloc] init];
	[lblCategory setFont:[UIFont fontWithName:@"Arial" size:22]];

	CGSize maximumSize = CGSizeMake(300, 27);
	UIFont *CFont = [UIFont fontWithName:@"Arial"size:22];
	//UIFont *CFont = [UIFont boldSystemFontOfSize:22];
	CGSize CategoryStringSize = [strCategory sizeWithFont:CFont 
										constrainedToSize:maximumSize 
											lineBreakMode:lblCategory.lineBreakMode];
	CGRect CategoryFrame = CGRectMake(283, 240, CategoryStringSize.width, 27);
	NSLog(@"Categoryframesize %f",CategoryStringSize.width);
	lblCategory.frame = CategoryFrame;
	
	[lblCategory setTextColor:[UIColor lightGrayColor]];
	lblCategory.text = strCategory;
	[self.view addSubview:lblCategory];	
	
		//Add city Label
	lblCity = [[UILabel alloc] init];
	[lblCity setFont:[UIFont fontWithName:@"Arial" size:22]];
		strCity = [NSString stringWithFormat:@"%@",strCity];
	CGSize CityStringSize = [strCity sizeWithFont:CFont 
									constrainedToSize:maximumSize 
										lineBreakMode:lblCity.lineBreakMode];
	CGRect CityFrame = CGRectMake(290+CategoryStringSize.width, 240, CityStringSize.width, 27);
	
	NSLog(@"Cityframesize %f",CityStringSize.width);
	lblCity.frame = CityFrame;
	
	[lblCity setTextColor:[UIColor lightGrayColor]];
	lblCity.text = strCity;
	[self.view addSubview:lblCity];	
	
	[self.tblSearch reloadData];	
	
	}
	}

}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	NSLog(@"appDelegate.delArrSearch:-%d",[appDelegate.delArrSearch count]);
	return [appDelegate.delArrSearch count];
}
- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	return 430.0;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	UITableViewCell *Cell = [self.tblSearch dequeueReusableCellWithIdentifier:CellIdentifier];
	
	Cell =[[[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier]autorelease];
	
	//set Cell
	CellSearch *objcell = [[CellSearch alloc]init];
	[Cell.contentView addSubview:objcell.view];
	
	Search *asearch = [appDelegate.delArrSearch objectAtIndex:indexPath.row];
	
	objcell.lblbusinessname.text = asearch.businessname;
	objcell.lblbusinessAdd.text = asearch.street;
	objcell.lblReviewCount.text = asearch.review;
	objcell.lblServices.text = asearch.serviceoffered;
	objcell.lblAccreditations.text = asearch.accreditationtext;
	objcell.lblPhone.text = asearch.phone;
	objcell.lblNo.text = [NSString stringWithFormat:@"%d.",indexPath.row+1];
	
	objcell.mon_am.text = asearch.mon_am; 
	objcell.mon_pm.text = asearch.mon_pm; 
	objcell.tue_am.text = asearch.tue_am; 
	objcell.tue_pm.text = asearch.tue_pm; 
	objcell.wed_am.text = asearch.wed_am; 
	objcell.wed_pm.text = asearch.wed_pm; 
	objcell.thru_am.text = asearch.thru_am;
	objcell.thru_pm.text = asearch.thru_pm;
	objcell.fri_am.text = asearch.fri_am; 
	objcell.fri_pm.text = asearch.fri_pm; 
	objcell.sat_am.text = asearch.sat_am; 
	objcell.sat_pm.text = asearch.sat_pm;
	objcell.sun_am.text = asearch.sun_am; 
	objcell.sun_pm.text = asearch.sun_pm;
	
	
	btnReview = [[UIButton alloc] init];
	btnReview = [UIButton buttonWithType:UIButtonTypeCustom];
	[btnReview setFrame:CGRectMake(546, 200, 143, 37)];
	btnReview.tag = indexPath.row;
	UIImage *imgbtn = [UIImage imageNamed:@"btnReadreviewYellow.png"];
	[btnReview setBackgroundImage:imgbtn forState:UIControlStateNormal];
	[btnReview addTarget:self action:@selector(SearchResult:)forControlEvents:UIControlEventTouchUpInside];
	[Cell.contentView addSubview:btnReview];
	//543X112
	if ([asearch.verified isEqualToString:@"0"]) {
			
		
		objcell.btnCliamyourbusiness.tag = indexPath.row;
		UIImage *imgbtn = [UIImage imageNamed:@"cliamyourbusiness.png"];
		[objcell.btnCliamyourbusiness setBackgroundImage:imgbtn forState:UIControlStateNormal];
		[objcell.btnCliamyourbusiness addTarget:self action:@selector(CliamyourbusinessClick:)forControlEvents:UIControlEventTouchUpInside];
	}
	if ([asearch.verified isEqualToString:@"1"]) {
		
		objcell.btnCliamyourbusiness = [[UIButton alloc] init];
		objcell.btnCliamyourbusiness.tag = indexPath.row;
		UIImage *imgbtn = [UIImage imageNamed:@"verfiedimg.png"];
		[objcell.btnCliamyourbusiness setBackgroundImage:imgbtn forState:UIControlStateNormal];
		
	}
	
	
	float fltRating = floor(asearch.reviewrate);
	float diffValue = ((asearch.reviewrate)-fltRating);
	int x = 546, y = 52, w = 29, h = 26;
	
	if (asearch.reviewrate < 0.4) {
		NSLog(@"No Rating");
		for (int i=0; i<5; i++) {
		//objCell.lblNoRating.hidden = NO;
		CGRect rect = CGRectMake(x, y, w, h);
		objcell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
		[objcell.imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
		[Cell.contentView addSubview:objcell.imghalfRating];
		[objcell.imghalfRating release];
		x += w;
		}
		
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
			[objcell.imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[Cell.contentView addSubview:objcell.imghalfRating];
			[objcell.imghalfRating release];
			x += w;
			fltRating +=1;
			
		} else if (diffValue >= 0.4 && diffValue <= 0.8) 
		{
			CGRect rect = CGRectMake(x, y, w, h);
			objcell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[objcell.imghalfRating setImage:[UIImage imageNamed:@"half25X25.png"]];
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
	
		[Cell setSelectionStyle:UITableViewCellSelectionStyleNone];
	return Cell;
}
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
	
	[tableView deselectRowAtIndexPath:indexPath animated:YES];
	
}
-(void)CliamyourbusinessClick :(int)index
{
	ReviewClick = (UIButton *)index;
	Search *asearch = [appDelegate.delArrSearch objectAtIndex:ReviewClick.tag];
	self.businessid = asearch.id;
	NSLog(@"businessid:-%d",self.businessid);
	
	WebclaimyourbusinessViewController *objwebview = [[WebclaimyourbusinessViewController alloc] initWithNibName:@"WebclaimyourbusinessViewController" bundle:nil];
	appDelegate.businessid = self.businessid;

    [self presentModalViewController:objwebview animated:YES];
    [objwebview release];
}

-(void)SearchResult :(int)index
{
	ReviewClick = (UIButton *)index;
	Search *asearch = [appDelegate.delArrSearch objectAtIndex:ReviewClick.tag];
	self.businessid = asearch.id;
	NSLog(@"businessid:-%d",self.businessid);
	
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(GetSearchResult) onTarget:self withObject:nil animated:YES];	
}
-(void)GetSearchResult
{
	NSAutoreleasePool *pool	= [[NSAutoreleasePool alloc] init];
	
	NSString *xmlString = [appDelegate apiSearchResult:self.businessid];
	NSData *xmlData = [xmlString dataUsingEncoding:NSUTF8StringEncoding];
	NSXMLParser *xmlParser = [[NSXMLParser alloc]initWithData:xmlData];
	
	XMLSearchResult *parser = [[XMLSearchResult alloc]initXMLSearchResult];
	
	[xmlParser setDelegate:parser];
	
	BOOL success = [xmlParser parse];
	if (success) {
		NSLog(@"delArrSearchResult:-%d",[appDelegate.delArrSearchResult count]);
		[txtCity resignFirstResponder];
		[txtCategory resignFirstResponder];

		BusinessSearchResultViewController * objBusinessResult = [[BusinessSearchResultViewController alloc]initWithNibName:@"BusinessSearchResultViewController" bundle:nil];
		objBusinessResult.strCity=txtCity.text;
		objBusinessResult.strCategory=txtCategory.text;
		[self presentModalViewController:objBusinessResult animated:YES];
        [objBusinessResult release];
	}
	else {
		
	}
	
	[pool release];

}
-(IBAction)BackClcik
{	
	LocalBusinesSearchViewController *awriteorRecode = [[LocalBusinesSearchViewController alloc]initWithNibName:@"LocalBusinesSearchViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
}
-(IBAction)btnSearchClick
{	
	[txtCity resignFirstResponder];
	[txtCategory resignFirstResponder];
	[lblCity removeFromSuperview];
	[lblCategory removeFromSuperview];
	
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
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
	
	if ([[txtCategory.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtCategory.text==nil){
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please Enter City" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else if ([[txtCity.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtCity.text==nil){
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please Enter City" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	 	
	else {
		
	NSString *xmlString = [appDelegate apiSearch:[txtCategory.text stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding] city:[txtCity.text stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
	NSData *xmlData = [xmlString dataUsingEncoding:NSUTF8StringEncoding];
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmlData];
	
	//Initialize the delegate.
	XMLSearch *parser = [[XMLSearch alloc] initXMLSearch];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		if ([appDelegate.delArrSearch count]==0) {
		//	[self.tblSearch reloadData];
			lblNoRecord.hidden=NO;
			lblResultNot.hidden=YES;
			tblSearch.hidden=YES;
			lblCategory.hidden=YES;
			lblCity.hidden=YES;
		}
		else {
			strCity=txtCity.text;
			strCategory=txtCategory.text;

			[self viewWillAppear:YES];
	}		
	} else{
		NSLog(@"Error Error Error!!!");
	}
	}
	
	[pool release];
}


-(void)CategoryList
{
	NSAutoreleasePool *pool= [[NSAutoreleasePool alloc]init];
	lblCity.hidden=YES;
	lblCategory.hidden=YES;
	NSString *str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=searchCategory&category=%@&version=2",appDelegate.ChangeUrl,txtCategory.text];
	NSLog(@"URL String=%@",str);
	
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	
	//Initialize the delegate.
	XMLBusinessCategory *parser = [[XMLBusinessCategory alloc] initXMLBusinessCategory];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		NSLog(@"No Errors");
		[txtCategory resignFirstResponder];
		[txtCity resignFirstResponder];
		tlbCategory.hidden=NO;
		picCategory.hidden=NO;
		NSLog(@"countofbusinesscategory:-%d",[appDelegate.delarrBusinessCategory count]);
		[picCategory reloadAllComponents];
		
	} else
		NSLog(@"Error Error Error!!!");
	
	[pool release];
	
	
}
- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
	return 1;
}

- (NSInteger)pickerView:(UIPickerView *)thePickerView numberOfRowsInComponent:(NSInteger)component {
	NSLog(@"Business counter-----%d",[appDelegate.delarrBusinessCategory count]);
	return [appDelegate.delarrBusinessCategory count];
}

- (NSString *)pickerView:(UIPickerView *)thePickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component {
	BusinessCategory *aBcategory = [[BusinessCategory alloc]init];
	
	aBcategory = [appDelegate.delarrBusinessCategory objectAtIndex:row];
	
	return aBcategory.categoryName;
	
}

- (void)pickerView:(UIPickerView *)thePickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
{	flgdidSelectRow=TRUE;
	BusinessCategory *aBcategory = [appDelegate.delarrBusinessCategory objectAtIndex:row];
	txtCategory.text = aBcategory.categoryName;
	
}

-(IBAction)btnDoneClick
{
	lblCity.hidden=NO;
	lblCategory.hidden=NO;

	picCategory.hidden=YES;
	tlbCategory.hidden=YES;
	
	if(flgdidSelectRow==FALSE)
	{
		BusinessCategory *aBcategory = [appDelegate.delarrBusinessCategory objectAtIndex:0];
		txtCategory.text = aBcategory.categoryName;
	}
}
- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string
{
	if (textField==txtCategory) {
		
		int characterlen= [txtCategory.text length];
		if (characterlen >=3) {
			[self CategoryList];
		}	
	}
	return YES;
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
     [super didReceiveMemoryWarning];
    
 
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
