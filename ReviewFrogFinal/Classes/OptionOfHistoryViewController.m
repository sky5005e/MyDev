    //
//  OptionOfHistoryViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 6/27/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "OptionOfHistoryViewController.h"
#import "History.h"
#import "VideoListViewController.h"
#import "ThreeTabButton.h"
#import "ApplicationValidation.h"
#import "Terms_Condition.h"
#import "HistoryLogin.h"
#import "ThreeTabButton.h"
#import "BusinessVideoListViewController.h"
#import "CategoryViewController.h"
#import "suggestionViewController.h"
#import "WriteORRecodeViewController.h"
#import "SoundSwitch.h"
#import "AdminCoupon.h"

#import "OptionOfHistoryViewController.h"
#import "BussinessProfileViewController.h"
#import "ReviewAutoResponderViewConroller.h"

#import "WriteORRecodeViewController.h"
#import "ReviewCategoryWiseListViewController.h"
#import "VideoCategoryWiseListViewController.h"
@implementation OptionOfHistoryViewController



// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	btnwrite.backgroundColor=[UIColor clearColor];
	btnvideo.backgroundColor=[UIColor clearColor];
	btnbusiness.backgroundColor=[UIColor clearColor];
}
- (void)viewWillAppear:(BOOL)animated
{
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
	[btnadmin setImage:[UIImage imageNamed:@"NadminD_v2.png"] forState:UIControlStateNormal];
}

- (void)viewWillDisappear:(BOOL)animated
{
	[btnadmin setImage:[UIImage imageNamed:@"Nadmin_v2.png"] forState:UIControlStateNormal];
}
-(IBAction) CategoryClick
{
	CategoryViewController *objcategroy = [[CategoryViewController alloc]initWithNibName:@"CategoryViewController" bundle:nil];
    [self.navigationController pushViewController:objcategroy animated:YES];
    [objcategroy release];
}
-(IBAction) SupportClcik
{
	suggestionViewController *suggestion=[[suggestionViewController alloc]initWithNibName:@"suggestionViewController" bundle:nil];
    [self.navigationController pushViewController:suggestion animated:YES];
    [suggestion release];	
}
-(IBAction) AdminCouponClick
{
	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else
	{
		AdminCoupon *admin= [[AdminCoupon alloc]init];
        [self.navigationController pushViewController:admin animated:YES];
        [admin release];
	}
	
}

-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}
-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
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
-(IBAction) ViewTypeRecord_Click
{
	History *history= [[History alloc]init];
    [self.navigationController pushViewController:history animated:YES];
    [history release];	
}
-(IBAction) ViewVideoRecord_Click;
{
	VideoListViewController *objvideolist= [[VideoListViewController alloc]init];
    [self.navigationController pushViewController:objvideolist animated:YES];
    [objvideolist release];
}
-(IBAction) BusinessVideoListing_Click
{
	BusinessVideoListViewController *objbusinessVideolist = [[BusinessVideoListViewController alloc]init];
    [self.navigationController pushViewController:objbusinessVideolist animated:YES];
    [objbusinessVideolist release];
}

-(IBAction) BusinessProfileClick
{
	BussinessProfileViewController *objBProfile = [[BussinessProfileViewController alloc]initWithNibName:@"BussinessProfileViewController" bundle:nil];
    [self.navigationController pushViewController:objBProfile animated:YES];
    [objBProfile release];
}
-(IBAction) AutoResponderClick
{

	ReviewAutoResponderViewConroller *objReviewAuto = [[ReviewAutoResponderViewConroller alloc]initWithNibName:@"ReviewAutoResponderViewConroller" bundle:nil];
    [self.navigationController pushViewController:objReviewAuto animated:YES];
    [objReviewAuto release];
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

-(IBAction) LogOut
{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *objWriteRecord = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];	
    [self.navigationController pushViewController:objWriteRecord animated:YES];
    [objWriteRecord release];
	
}
-(IBAction) WriteStaticstics
{
    appDelegate.flgSelectedType=TRUE;
    
	ReviewCategoryWiseListViewController *objCategoryList = [[ReviewCategoryWiseListViewController alloc]init];
    [self.navigationController pushViewController:objCategoryList animated:YES];
    [objCategoryList release];

}
-(IBAction) VideoStaticstics
{
    appDelegate.flgSelectedVideoType=TRUE;
	VideoCategoryWiseListViewController *objCategoryList = [[VideoCategoryWiseListViewController alloc]init];
    [self.navigationController pushViewController:objCategoryList animated:YES];
    [objCategoryList release];

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
