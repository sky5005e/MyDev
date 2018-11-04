    //
//  Terms_Condition.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 31/12/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import "Terms_Condition.h"
#import <QuartzCore/QuartzCore.h>
#import "Review_FrogViewController.h"
#import "Review_FrogAppDelegate.h"
#import "Terms_Condition.h"
#import "History.h"
#import "xmlparser_para.h"
#import "HistoryLogin.h"
#import "AdminCoupon.h"
#import "SoundSwitch.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"


@implementation Terms_Condition
@synthesize txtNote;
@synthesize isChecked;
@synthesize TermsButton;
@synthesize webTerm;
@synthesize ActiviIndicator; 

/*
 // The designated initializer.  Override if you create the controller programmatically and want to perform customization that is not appropriate for viewDidLoad.
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    if ((self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil])) {
        // Custom initialization
    }
    return self;
}
*/


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	[super viewDidLoad];
	[self.ActiviIndicator startAnimating];    
    NSURL *url = [[NSURL alloc]initWithString:[NSString stringWithFormat:@"%@",appDelegate.Set_Termurl]];
    NSURLRequest *urlreq = [[NSURLRequest alloc] initWithURL:url];
    [webTerm loadRequest:urlreq];
    
		if (appDelegate.flgswich) {
			btnadmin.hidden=YES;
		}
		else {
			btnadmin.hidden=YES;
		}
}

- (void)viewWillAppear:(BOOL)animated
{
/*	LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
	if (alogin.imgBlogo!= nil) {
		//NSLog(@"Not Right");
		imgBLogo.image = alogin.imgBlogo;
		
	} else {		
		if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
			
			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			imgBLogo.image = image;			
		}else {
			UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
			imgBLogo.image = img;
		}		
	}	*/

    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    
    
    if (pathBussinesslogo !=nil) {   
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }
    
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
        
    }
    
    //imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];

	[TermsButton setImage:[UIImage imageNamed:@"NtermsD_v2.png"] forState:UIControlStateNormal];
}

- (void)viewWillDisappear:(BOOL)animated
{
    
	[TermsButton setImage:[UIImage imageNamed:@"Nterms_v2.png"] forState:UIControlStateNormal];
}

-(void)webViewDidFinishLoad:(UIWebView *)webView
{	
	[self.ActiviIndicator stopAnimating];

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

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
	}
}
-(IBAction)Home{
    [self.ActiviIndicator stopAnimating];
	appDelegate.LoginConform=FALSE;
	appDelegate.txtcleanflag=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];	
}
-(IBAction) Admin
{	[self.ActiviIndicator stopAnimating];	
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
