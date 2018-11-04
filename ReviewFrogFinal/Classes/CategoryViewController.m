    //
//  CategoryViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 8/10/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "CategoryViewController.h"
#import "Category.h"
#import "ThreeTabButton.h"
#import "ApplicationValidation.h"
#import "Terms_Condition.h"
#import "HistoryLogin.h"
#import "OptionOfHistoryViewController.h"
#import "WriteORRecodeViewController.h"
#import "LoginInfo.h"
#import "XMLCategory.h"
#import "DefaultCategoryViewController.h"

@implementation CategoryViewController
@synthesize tblCategory;
@synthesize flag_Category;
// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	txtDefaultCategory.text = appDelegate.strDefaultCategory;
	[self StartCategoryLoging];
}
-(void)viewWillAppear:(BOOL)animated
{
    imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:appDelegate.pathBussinesslogo] autorelease];
    
    

/*	LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
	if (alogin.imgBlogo!= nil) {
	//	NSLog(@"Not Right");
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
	}*/
/*	if (alogin.imgGiveAway!= nil) {
		NSLog(@"Not Right");
		imgGiveAway.image = alogin.imgGiveAway;
		
	} else {
		
		if (![alogin.userbusinessgiveaway isEqualToString:@"0"]) {
			NSString *str = [alogin.userbusinessgiveaway stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
			NSURL *url = [[NSURL alloc] initWithString:str];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			alogin.imgGiveAway = image;
			imgGiveAway.image = image;
			
		}else {
			UIImage *img = [UIImage imageNamed:@"giftcard.png"];
			imgGiveAway.image = img;
		}
	}
*/	
	
}

-(void) StartCategoryLoging
{
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
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
	BOOL iscategory = [appDelegate apiCategory];
	if (iscategory) {
		[self performSelectorOnMainThread:@selector(ReloadTable) withObject:nil waitUntilDone:YES];
	}
	else {
			NSLog(@"Error");
	}
	[pool release];

}

-(void) ReloadTable
{
	[self.tblCategory reloadData];
}
- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	NSLog(@"CategorCounter:-%d",[appDelegate.ArrCategoryList count]);
	return [appDelegate.ArrCategoryList count];
}
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	
	UITableViewCell *cell = [self.tblCategory dequeueReusableCellWithIdentifier:CellIdentifier];
	
	cell = [[[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier] autorelease];
		objcategory = [appDelegate.ArrCategoryList objectAtIndex:indexPath.row];
		
		cell.textLabel.text = objcategory.categoryname;
		
		btnDelete = [[UIButton alloc]init];
		btnDelete = [UIButton buttonWithType:UIButtonTypeCustom];
		[btnDelete setFrame:CGRectMake(974, 15, 29, 31)];
		btnDelete.tag = objcategory.id;
    if(objcategory.Category_UserID)
    {        
        [btnDelete setBackgroundImage:[UIImage imageNamed:@"deletebtn.png"] forState:UIControlStateNormal];
        [btnDelete addTarget:self action:@selector(DeleteButton_Click:)forControlEvents:UIControlEventTouchUpInside];
        [cell.contentView addSubview:btnDelete];
    }

    [cell setSelectionStyle:UITableViewCellSelectionStyleNone];
		return cell;
}
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{	
	[tableView deselectRowAtIndexPath:indexPath animated:NO];    
}

-(void)DeleteButton_Click :(int)index
{	btnDelete = (UIButton *)index;
	cid = btnDelete.tag;
	NSLog(@"Strcategoryid:%d",cid);	
	NSLog(@"Delete");
	alertConfirm = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are you sure you want to delete this category as there are reviews associated with it. Deleting the category will also delete the reviews associated with it." delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes",nil];
	[alertConfirm show];
	[alertConfirm release];
	
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex 
{
	if (alertView == alertConfirm) 
	{
		if (buttonIndex == 0) {
			NSLog(@"No");
		} else {
			[self DeleteDone];
		}
	}else if (alertView == AlertUpdate) 
	{
		if (buttonIndex == 0) {
			NSLog(@"No");
		} else {
			[self UPdateDone];
		}
	}else if (alertView == AddAlert) 
	{
		if (buttonIndex == 0) {
		
			NSLog(@"No");
		
		} else {
			NSString *strCatgory;
			strCatgory=[txtCategory.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
			if ([[strCatgory stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""]||strCatgory==nil) {
				
				UIAlertView *addalert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"please Add category" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
				[addalert show];
				[addalert release];
			}
			else {
				[self StartAddingCategory];
				[txtCategory resignFirstResponder];
				[self.tblCategory reloadData];
			}

			
		}
	}

}
-(void) StartAddingCategory
{
	NSString *strcategory;
	strcategory = [txtCategory.text stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	
	NSString * str = [NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_CategoryInsert&category=%@&userid=%@&version=2",appDelegate.ChangeUrl,strcategory,appDelegate.userId];
	
	NSLog(@"URL String category insert=%@",str);
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	NSString *webpage = [[NSString alloc] initWithData:xmldata encoding:NSUTF8StringEncoding];
	NSLog(@"Web page====%@",webpage);
	
	NSString *responseString = [webpage stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSRange rngAdminIDStart = [responseString rangeOfString:@"<return>"];
	if (rngAdminIDStart.length > 0) {
		responseString = [responseString substringFromIndex:rngAdminIDStart.location+rngAdminIDStart.length];
	}
	
	NSRange rngAdminIDEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngAdminIDEnd.length > 0) {
		responseString = [responseString substringToIndex:rngAdminIDEnd.location];
	}
	
	
	if ([responseString isEqualToString:@"1"]) {
		[self StartCategoryLoging];			
	}
	if ([responseString isEqualToString:@"-1"]) {
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"This category already exists please enter new one" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	if ([responseString isEqualToString:@"0"]) 
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alert show];
		[alert release];
		
	}
	
	
}
-(void)DeleteDone
{	
	NSString * str = [NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_CategoryDelete&category_id=%d&userid=%@&version=2",appDelegate.ChangeUrl,cid,appDelegate.userId];
	
	NSLog(@"URL String=%@",str);
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	NSString *webpage = [[NSString alloc] initWithData:xmldata encoding:NSUTF8StringEncoding];
	NSLog(@"Web page====%@",webpage);
	
	NSString *responseString = [webpage stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSRange rngAdminIDStart = [responseString rangeOfString:@"<return>"];
	if (rngAdminIDStart.length > 0) {
		responseString = [responseString substringFromIndex:rngAdminIDStart.location+rngAdminIDStart.length];
	}
	
	NSRange rngAdminIDEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngAdminIDEnd.length > 0) {
		responseString = [responseString substringToIndex:rngAdminIDEnd.location];
	}
	
	
	if ([responseString isEqualToString:@"1"]) {
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Category deleted successfully.   If deleted category was your default category, then please re-select new default category." delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
			[alert show];
			[alert release];
		if (txtDefaultCategory.text = appDelegate.strDefaultCategory) {
			appDelegate.strDefaultCategory = @"";
			NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
			[prefs setObject:appDelegate.strDefaultCategory forKey:@"DefaultCategory"];	
			[appDelegate getUserPreferences];
			txtDefaultCategory.text = appDelegate.strDefaultCategory;
		}
		[self StartCategoryLoging];		
	} else {
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Error" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alert show];
		[alert release];
		
	}
	
}	

/*-(void)UpdateButton_Click :(int)index
{
	AlertUpdate = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are you sure want to update this category?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes",nil];
	[AlertUpdate show];
	[AlertUpdate release];
	
	btnDelete = (UIButton *)index;
	objcategory = [appDelegate.ArrCategoryList objectAtIndex:btnDelete.tag];
	cid = objcategory.categoryid;
	cname =objcategory.categoryname;
	NSLog(@"Strcategoryid:%d",cid);
	
}
-(void)UPdateDone
{
	
	[appDelegate UpDateCategory:cid cname:@"kesur"];
	[appDelegate LoadReviewCategory];	
	[self.tblCategory reloadData];
	
}	
*/

-(IBAction)BackClick
{
    if (flag_Category==FALSE) {
        
        VideoCategoryWiseListViewController *obj = [[VideoCategoryWiseListViewController alloc]initWithNibName:@"VideoCategoryWiseListViewController" bundle:nil];
        [self.navigationController pushViewController:obj animated:YES];
        [obj release];
    }
    else{
        
        ReviewCategoryWiseListViewController *obj = [[ReviewCategoryWiseListViewController alloc]initWithNibName:@"ReviewCategoryWiseListViewController" bundle:nil];
        [self.navigationController pushViewController:obj animated:YES];
        [obj release];    
        
    }
}

-(IBAction) AddCategory{
		//showAlert=YES; //boolean variable

	
	AddAlert =[UIAlertView new]; 
	AddAlert.title = @"Enter Category";
			   
	[AddAlert addButtonWithTitle:@"Cancel"];
	[AddAlert addButtonWithTitle:@"Add"];
	AddAlert.message = @"\n";
	AddAlert.delegate = self;
	[AddAlert show];
    	
	txtCategory = [[UITextField alloc] initWithFrame:CGRectMake(12.0, 45.0, 260.0, 25.0)];
	txtCategory.text=@"";
	[txtCategory setBackgroundColor:[UIColor whiteColor]];
	//    [txtName setKeyboardAppearance:UIKeyboardAppearanceAlert];
	[txtCategory setAutocorrectionType:UITextAutocorrectionTypeNo];
	[txtCategory setKeyboardType:UIKeyboardTypeAlphabet];
	[txtCategory setTextAlignment:UITextAlignmentLeft];
	[AddAlert addSubview:txtCategory];
	
}

-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}

-(IBAction) Admin
{	
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
-(IBAction) DefaultCategoryClick
{
	DefaultCategoryViewController *objPopview = [[DefaultCategoryViewController alloc]initWithNibName:@"DefaultCategoryViewController" bundle:nil];
	objPopview.delegate = self;
	popView =[[UIPopoverController alloc]initWithContentViewController:objPopview];
	[popView presentPopoverFromRect:CGRectMake(199, 149, 376, 260) inView:self.view permittedArrowDirections:0 animated:NO];
	[popView setPopoverContentSize:CGSizeMake(376, 260)];
	
}
-(void) didSelectDefaultCategory
{
	NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
	[prefs setObject:appDelegate.strDefaultCategory forKey:@"DefaultCategory"];	
	
	txtDefaultCategory.text = appDelegate.strDefaultCategory;
	[popView dismissPopoverAnimated:YES];
	
}

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}

- (BOOL)textFieldShouldBeginEditing:(UITextField *)textField
{	
	
	if (textField==txtDefaultCategory) {
		[textField resignFirstResponder];
		[self DefaultCategoryClick];
		return NO;
	}
	return YES;
}
- (BOOL)textFieldShouldEndEditing:(UITextField *)textField
{
	return YES;
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
   
    [super didReceiveMemoryWarning];
}


- (void)viewDidUnload {
    [super viewDidUnload];
}


- (void)dealloc {
    [super dealloc];
}


@end
