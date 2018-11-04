    //
//  LocalBusinesSearchViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 7/25/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "LocalBusinesSearchViewController.h"
#import "WebAndListingViewController.h"
#import "SearchCityViewController.h"
#import "XMLBusinessCategory.h"
#import "XMLSearch.h"
#import "BusinessSearchListViewController.h"

@implementation LocalBusinesSearchViewController
@synthesize Activeindicator;

- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate*)[[UIApplication sharedApplication]delegate];
	picCategory.hidden=YES;
	tlbCategory.hidden=YES;	
	[scrView addSubview:ViewSearch];
	[scrView setContentSize:CGSizeMake(ViewSearch.frame.size.width, ViewSearch.frame.size.height)];
	
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillShowNotification:)  
												 name:UIKeyboardWillShowNotification  
											   object:nil];  
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillHideNotification)  
												 name:UIKeyboardWillHideNotification  
											   object:nil]; 	
}

-(void)viewWillAppear:(BOOL)animated
{
	picCategory.hidden=YES;
	tlbCategory.hidden=YES;

}
- (void)viewDidUnload {
    [super viewDidUnload];
}

-(IBAction) BackClick
{
	WebAndListingViewController *awriteorRecode = [[WebAndListingViewController alloc]initWithNibName:@"WebAndListingViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
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
-(IBAction)btnSearchClick
{
	[txtCity resignFirstResponder];
	[txtCategory resignFirstResponder];
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
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];	
	if ([[txtCity.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtCity.text==nil){
		
		alertcity = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please Enter City" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alertcity show];
		[alertcity release];
		
	}
	else if ([[txtCategory.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtCategory.text==nil){
		
		alertcategory = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please Enter Category" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alertcategory show];
		[alertcategory release];
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
		NSLog(@"No Errors coutner %d",[appDelegate.delArrSearch count]);
				[txtCity resignFirstResponder];
			[txtCategory resignFirstResponder];
			BusinessSearchListViewController *objbusinessSearch = [[BusinessSearchListViewController alloc]initWithNibName:@"BusinessSearchListViewController" bundle:nil];
			objbusinessSearch.strCity=txtCity.text;
			objbusinessSearch.strCategory=txtCategory.text;
        [self.navigationController pushViewController:objbusinessSearch animated:YES];
        [objbusinessSearch release];	
	} else{
		NSLog(@"Error Error Error!!!");
	}
		
}

	[pool release];
}
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex 
{
	if (alertView == alertcity) {
		[txtCity becomeFirstResponder];
	}
	else if (alertView == alertcategory) {
		[txtCategory becomeFirstResponder];
	}
}
- (void)textFieldDidBeginEditing:(UITextField *)textField
{
	if (textField==txtCategory) {
		ViewSearch.frame=CGRectMake(0, -120, 1024, 748);
	}
}
-(void)textFieldDidEndEditing:(UITextField *)textField
{
	ViewSearch.frame=CGRectMake(0, 0, 1024, 748);
	
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
-(void)didTap
{
	NSLog(@"City name:-%@",appDelegate.CityName);
	txtCity.text=appDelegate.CityName;
	[popView dismissPopoverAnimated:YES];	
}

-(void)keyboardWillShowNotification:(NSNotification *)note{	
	
	scrView.frame=CGRectMake(0, 0, 1024, 450);
	
	
}
-(void)keyboardWillHideNotification
{
	scrView.frame=CGRectMake(0, 0, 1024, 748);
	
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

- (void)dealloc {
    [super dealloc];
}


@end
