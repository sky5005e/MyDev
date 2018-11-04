    //
//  SoundSwitch.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 03/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "SoundSwitch.h"
#import "History.h"
#import "Review_FrogAppDelegate.h"
#import "HistoryLogin.h"
#import "AdminCoupon.h"
#import "Terms_Condition.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "WriteORRecodeViewController.h" 
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"
@implementation SoundSwitch
@synthesize Switch1;
@synthesize btnSwitch;

 // The designated initializer.  Override if you create the controller programmatically and want to perform customization that is not appropriate for viewDidLoad.
/*
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization.
    }
    return self;
}
*/


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	
	[Switch1 setOn:appDelegate.SwitchFlag];
}

-(IBAction)SwitchOnOff
{
	
	if (Switch1.on) 
	{
		
		appDelegate.SwitchFlag =TRUE;
		[appDelegate saveUserPreferences:2];
		[appDelegate resetIdleTimer];
		
	}
     else 
	 {  
         appDelegate.SwitchFlag =FALSE;
		 [appDelegate saveUserPreferences:2];
		 if (appDelegate.idleTimer) {
			 [appDelegate.idleTimer invalidate];
			 appDelegate.idleTimer=nil;
		 }
		 
     }
	
}

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
	}
	
}
-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}


-(IBAction) Admin
{	if(appDelegate.LoginConform==FALSE)
	
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

-(IBAction) LogOut
{
	appDelegate.LoginConform=FALSE;
	ApplicationValidation *home = [[ApplicationValidation alloc]init];
    [self.navigationController pushViewController:home animated:YES];
    [home release];		
	
}

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
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

-(void)viewWillAppear:(BOOL)animated
{

    imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:appDelegate.pathBussinesslogo] autorelease];

    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;

    }
    
        //imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];

	[btnSwitch setImage:[UIImage imageNamed:@"NsoundD_v2.png"] forState:UIControlStateNormal];	
}




- (void)viewWillDisappear:(BOOL)animated
{
	[btnSwitch setImage:[UIImage imageNamed:@"NSOUND_v2.png"] forState:UIControlStateNormal];
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
