    //
//  ThreeTabButton.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 05/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "ThreeTabButton.h"
#import "History.h"
#import "AdminCoupon.h"
#import "SoundSwitch.h"
#import "ApplicationValidation.h"
#import "HistoryLogin.h"
#import "suggestionViewController.h"
#import "OptionOfHistoryViewController.h"
#import <MobileCoreServices/MobileCoreServices.h>
#import "BusinessVideoViewController.h"
#import "BusinessVideoListViewController.h"
#import "Terms_Condition.h"
#import "HistoryLogin.h"
#import "Terms_Condition.h"
#import "CategoryViewController.h"
#import "WriteORRecodeViewController.h"

@implementation ThreeTabButton
@synthesize btnsugestion;
@synthesize flgBusinessVideo;



// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	
	btnsugestion.backgroundColor=[UIColor clearColor];
}
- (void)viewWillAppear:(BOOL)animated
{
	[btnAdmin setImage:[UIImage imageNamed:@"NadminD_v2.png"] forState:UIControlStateNormal];
}

- (void)viewWillDisappear:(BOOL)animated
{
	[btnAdmin setImage:[UIImage imageNamed:@"Nadmin_v2.png"] forState:UIControlStateNormal];
}

-(IBAction)Home{
	appDelegate.LoginConform=FALSE;
	appDelegate.txtcleanflag=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}
-(IBAction)Back_click
{
	HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];
    [self.navigationController pushViewController:historylogin animated:YES];
    [historylogin release];

}

-(IBAction) TERMS_CONDITIONS
{appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}

-(IBAction)History
{
	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else
	{
		OptionOfHistoryViewController *objoption= [[OptionOfHistoryViewController alloc]init];
        [self.navigationController pushViewController:objoption animated:YES];
        [objoption release];	
	}

}

-(IBAction) AdminCoupon
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
