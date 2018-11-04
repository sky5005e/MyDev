    //
//  BusinessSearchResultViewController.m
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BusinessSearchResultViewController.h"
#import "SearchResult.h"
#import "CellResult.h"
#import "CellSearch.h"
#import "BusinessSearch.h"
#import "XMLBusinessCategory.h"
#import "BusinessCategory.h" 
#import "XMLSearchResult.h"
#import "XMLSearch.h"
#import "BusinessSearchListViewController.h"

@implementation BusinessSearchResultViewController
@synthesize tblResult;
@synthesize strCategory;
@synthesize strCity;

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	picCategory.hidden=YES;
	tlbCategory.hidden=YES;
	txtCategory.text = strCategory;
	txtCity.text = strCity;
}
- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
	return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	NSLog(@"appDelegate.delArrSearch:-%d",[appDelegate.delArrSearchResult count]);
	return [appDelegate.delArrSearchResult count];

}
- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	
	return 220;
	
}

- (CGFloat)tableView:(UITableView *)tableView heightForHeaderInSection:(NSInteger)section{
	return 166;
}
- (UIView *)tableView:(UITableView *)tableView viewForHeaderInSection:(NSInteger)section{
	
	BusinessSearch *aBSearch = [appDelegate.delArrBusinessSearch objectAtIndex:0];
	
	UIView *customView = [[UIView alloc] init];
	[customView setBackgroundColor:[UIColor whiteColor]];
	UILabel *lblBusinessName = [[UILabel alloc] initWithFrame:CGRectMake(28, 26, 321, 26)];
	[lblBusinessName setFont:[UIFont boldSystemFontOfSize:17]];
	[lblBusinessName setNumberOfLines:1];
	[lblBusinessName setTextColor:[UIColor orangeColor]];
	lblBusinessName.text = aBSearch.businessname;
	[customView addSubview:lblBusinessName];
	
	
	UILabel *lblBusinessAdd = [[UILabel alloc] initWithFrame:CGRectMake(32, 52, 299, 38)];
	[lblBusinessAdd setFont:[UIFont systemFontOfSize:17]];
	[lblBusinessAdd setNumberOfLines:2];
	[lblBusinessAdd setTextColor:[UIColor lightGrayColor]];
	lblBusinessAdd.text = aBSearch.street;
	[customView addSubview:lblBusinessAdd];
	
	
	UILabel *lblphone = [[UILabel alloc] initWithFrame:CGRectMake(32, 90, 206, 26)];
	[lblphone setFont:[UIFont boldSystemFontOfSize:17]];
	[lblphone setNumberOfLines:1];
	[lblphone setTextColor:[UIColor lightGrayColor]];
	lblphone.text = aBSearch.phone;
	[customView addSubview:lblphone];
	
	
	UILabel *lblMostReviewed = [[UILabel alloc] initWithFrame:CGRectMake(538, 26, 124, 26)];
	[lblMostReviewed setFont:[UIFont boldSystemFontOfSize:17]];
	[lblMostReviewed setNumberOfLines:1];
	[lblMostReviewed setTextColor:[UIColor orangeColor]];
	lblMostReviewed.text = @"Most Reviewed";
	[customView addSubview:lblMostReviewed];
	
	UILabel *lblReviewed = [[UILabel alloc] initWithFrame:CGRectMake(578, 87, 84, 26)];
	[lblReviewed setFont:[UIFont boldSystemFontOfSize:17]];
	[lblReviewed setNumberOfLines:1];
	[lblReviewed setTextColor:[UIColor orangeColor]];
	lblReviewed.text = @"Reviews";
	[customView addSubview:lblReviewed];
	
	UILabel *lblCount = [[UILabel alloc] initWithFrame:CGRectMake(529, 90, 42, 21)];
	[lblCount setFont:[UIFont boldSystemFontOfSize:17]];
	[lblCount setTextAlignment:UITextAlignmentRight];
	[lblCount setNumberOfLines:1];
	[lblCount setTextColor:[UIColor orangeColor]];
	lblCount.text = [NSString stringWithFormat:@"%0.f",aBSearch.reviewcounter];;
	[customView addSubview:lblCount];
	
	UIImageView *imgview = [[UIImageView alloc]initWithFrame:CGRectMake(0, 150, 700, 2)];
	UIImage *img = [UIImage imageNamed:@"line.png"];
	imgview.image = img;
	[customView addSubview:imgview];
	
	//Business Rate
	float fltRating = floor(aBSearch.avgreviewrate);
	float diffValue = ((aBSearch.avgreviewrate)-fltRating);
	int x = 530, y = 55, w = 29, h = 26;
	
	if (aBSearch.avgreviewrate < 0.4) {
		NSLog(@"No Rating");
		for (int i=0; i<5; i++) {
			//objCell.lblNoRating.hidden = NO;
			CGRect rect = CGRectMake(x, y, w, h);
			UIImageView *imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[customView addSubview:imghalfRating];
			[imghalfRating release];
			x += w;
		}
		
	} else {
		for (int i=0; i<fltRating; i++) {
			CGRect rect = CGRectMake(x, y, w, h);
			UIImageView *imgGivenRating = [[UIImageView alloc] initWithFrame:rect];
			[imgGivenRating setImage:[UIImage imageNamed:@"Fullyellow25X25.png"]];
			[customView addSubview:imgGivenRating];
			[imgGivenRating release];
			x += w;
		}
		if (diffValue > 0 && diffValue < 0.4) 
		{
			CGRect rect = CGRectMake(x, y, w, h);
			UIImageView *imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[customView addSubview:imghalfRating];
			[imghalfRating release];
			x += w;
			fltRating +=1;
			
		} else if (diffValue >= 0.4 && diffValue <= 0.8) 
		{
			CGRect rect = CGRectMake(x, y, w, h);
			UIImageView *imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[imghalfRating setImage:[UIImage imageNamed:@"half25X25.png"]];
			[customView addSubview:imghalfRating];
			[imghalfRating release];
			x += w;
			fltRating +=1;
			
		} 		
		
		for (int j=fltRating; j<5; j++) {
			CGRect rect = CGRectMake(x, y, w, h);
			UIImageView *imgNoRating = [[UIImageView alloc] initWithFrame:rect];
			[imgNoRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[customView addSubview:imgNoRating];
			[imgNoRating release];
			x += w;
		}		
	}
	
	return customView;
}
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	UITableViewCell *Cell = [self.tblResult dequeueReusableCellWithIdentifier:CellIdentifier];
	
	Cell =[[[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier]autorelease];

	CellResult *objBCell = [[CellResult alloc]init];
	[Cell.contentView addSubview:objBCell.view];

	SearchResult *aResult = [appDelegate.delArrSearchResult objectAtIndex:indexPath.row];
			
	objBCell.lblReviewTitle.text = aResult.reviewtitle;
	objBCell.lblPostedbyName.text = aResult.postedby;
	objBCell.lblDescription.text = aResult.reviewdescription;
	
	objBCell.lblDate.text = aResult.reviewdate;
	objBCell.lblNo.text = [NSString stringWithFormat:@"%d.",indexPath.row+1];
	
	
		
	
	//ReviewRate
	float RfltRating = floor(aResult.reviewrate);
	float RdiffValue = ((aResult.reviewrate)-RfltRating);
	int Rx = 529, Ry = 32, Rw = 21, Rh = 18;
	
	if (aResult.reviewrate < 0.4) {
		NSLog(@"No Rating");
		for (int i=0; i<5; i++) {
			//objCell.lblNoRating.hidden = NO;
			CGRect rect = CGRectMake(Rx, Ry, Rw, Rh);
			objBCell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[objBCell.imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[Cell.contentView addSubview:objBCell.imghalfRating];
			[objBCell.imghalfRating release];
			Rx += Rw;
		}
		
	} else {
		for (int i=0; i<RfltRating; i++) {
			CGRect rect = CGRectMake(Rx, Ry, Rw, Rh);
			objBCell.imgGivenRating = [[UIImageView alloc] initWithFrame:rect];
			[objBCell.imgGivenRating setImage:[UIImage imageNamed:@"Fullyellow25X25.png"]];
			[Cell.contentView addSubview:objBCell.imgGivenRating];
			[objBCell.imgGivenRating release];
			Rx += Rw;
		}
		if (RdiffValue > 0 && RdiffValue < 0.4) 
		{
			CGRect rect = CGRectMake(Rx, Ry, Rw, Rh);
			objBCell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[objBCell.imghalfRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[Cell.contentView addSubview:objBCell.imghalfRating];
			[objBCell.imghalfRating release];
			Rx += Rw;
			RfltRating +=1;
			
		} else if (RdiffValue >= 0.4 && RdiffValue <= 0.8) 
		{
			CGRect rect = CGRectMake(Rx, Ry, Rw, Rh);
			objBCell.imghalfRating = [[UIImageView alloc] initWithFrame:rect];
			[objBCell.imghalfRating setImage:[UIImage imageNamed:@"half25X25.png"]];
			[Cell.contentView addSubview:objBCell.imghalfRating];
			[objBCell.imghalfRating release];
			Rx += Rw;
			RfltRating +=1;
			
		} 		
		
		for (int j=RfltRating; j<5; j++) {
			CGRect rect = CGRectMake(Rx, Ry, Rw, Rh);
			objBCell.imgNoRating = [[UIImageView alloc] initWithFrame:rect];
			[objBCell.imgNoRating setImage:[UIImage imageNamed:@"Fullgray25X25.png"]];
			[Cell.contentView addSubview:objBCell.imgNoRating];
			[objBCell.imgNoRating release];
			Rx += Rw;
		}		
	}
	
	if (aResult.imgReviewer != nil) {
	//	NSLog(@"Not Right");
		objBCell.imgReview.image = aResult.imgReviewer;
		
	} else {
		
		NSMutableArray *arrObjects = [[NSMutableArray alloc] init];
		[arrObjects addObject:aResult];
		[arrObjects addObject:objBCell];
		
		[NSThread detachNewThreadSelector:@selector(loadImage:) toTarget:self withObject:arrObjects];
		[arrObjects release];
	}	
	
	
	
	[Cell setSelectionStyle:UITableViewCellSelectionStyleNone];
	return Cell;
}
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{	
	[tableView deselectRowAtIndexPath:indexPath animated:NO];
	

}
#pragma mark -
#pragma mark Loading image

- (void)loadImage :(NSMutableArray *)arrObjects 
{
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	SearchResult *objResult = [arrObjects objectAtIndex:0];
	CellResult *objBCell = [arrObjects objectAtIndex:1];
	//FOR ReviewImage
	if (!objResult.reviewerimage) {
		UIImage *img = [UIImage imageNamed:@"noimage.png"];
		objBCell.imgReview.image =img;
	}
	else {
	//	NSString *str = [objResult.reviewerimage stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
		NSURL *url = [[NSURL alloc] initWithString:objResult.reviewerimage];
		UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
		objResult.imgReviewer = image;
		
		[self performSelectorOnMainThread:@selector(loadImageComplete:) withObject:arrObjects waitUntilDone:YES];
		
	}	
	[pool release];
}
- (void)loadImageComplete:(NSMutableArray *)arrObjects {
	
	SearchResult *objResult = [arrObjects objectAtIndex:0];
	CellResult *objBCell = [arrObjects objectAtIndex:1];
	
   	objBCell.imgReview.image = objResult.imgReviewer;
	
	
}
-(void)CategoryList
{
	NSAutoreleasePool *pool= [[NSAutoreleasePool alloc]init];
	
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
	picCategory.hidden=YES;
	tlbCategory.hidden=YES;
	if(flgdidSelectRow==FALSE)
	{
		BusinessCategory *aBcategory = [appDelegate.delarrBusinessCategory objectAtIndex:0];
		txtCategory.text = aBcategory.categoryName;
	}
}

-(IBAction)BackClcik
{
	appDelegate.flgBack=TRUE;	
    [self dismissModalViewControllerAnimated:YES];
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
-(IBAction)btnSearchResultClcik
{
	[txtCity resignFirstResponder];
	[txtCategory resignFirstResponder];

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
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please Enter Category" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
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
			UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Results Not Found In City and Category you Entered. Please refine your Search" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
			[alert show];
			[alert release];
			
		}
		else {
			appDelegate.flgDismiss=TRUE;
			appDelegate.strCity=txtCity.text;
			appDelegate.strCategory = txtCategory.text;
			[self dismissModalViewControllerAnimated:YES];
			
		}		
	} else{
		NSLog(@"Error Error Error!!!");
	}
	}
	[pool release];
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
-(void)viewDidUnload {
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}
- (void)dealloc {
    [super dealloc];
}
@end
