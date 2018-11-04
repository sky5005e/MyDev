    //
//  Thank_you.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 07/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Thank_you.h"
#import "History.h"
#import "Review_FrogViewController.h"
#import "Terms_Condition.h"
#import "HistoryLogin.h"
#import "AdminCoupon.h"
#import "SoundSwitch.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "BusinessVideoListViewController.h"
#import "LoginInfo.h"
#import "OptionOfHistoryViewController.h"
#import "BusinessVideoListViewController.h"

@implementation Thank_you

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
    [super viewDidLoad];
	
	appDelegate=(Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
		
	[appDelegate.Data_email isEqualToString:@""];
	[appDelegate.Data_name isEqualToString:@""];
	[appDelegate.CityName isEqualToString:@""];
	[appDelegate.Data_desc isEqualToString:@""];
	
		if (appDelegate.idleTimer2) 
		{
			[appDelegate.idleTimer2 invalidate];
			appDelegate.idleTimer2=nil;
		}
				
			appDelegate.idleTimer2 = [NSTimer scheduledTimerWithTimeInterval:3 target:self selector:@selector(idleTimerExceededForThanks) userInfo:nil repeats:NO];
		//[idleTimer fire];
		
}	
-(void)viewWillAppear:(BOOL)animated
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
        imgPowerdby.hidden=YES;
        imgtxtPowerdby.hidden=YES;
    }
}      
        /*
        alogin = [appDelegate.delArrUerinfo objectAtIndex:0];

    
	if (alogin.imgBlogo!= nil) {
		NSLog(@"GiveAway Image : None");
		imgBLogo.image = alogin.imgBlogo;
		
	} else {
		
		if (![alogin.userbusinesslogo isEqualToString:@"0"]) 
        {
			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			alogin.imgBlogo = image;
			imgBLogo.image = image;
			imgPowerdby.hidden=NO;
			imgtxtPowerdby.hidden=NO;
            
			
			
		}else {
			UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
			imgBLogo.image = img;
			imgPowerdby.hidden=YES;
			imgtxtPowerdby.hidden=YES;
            
		}
		
	}
	if (alogin.imgGiveAway!= nil) {
		NSLog(@"GiveAway Image : None");
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
//    }
//    else{
//
//        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
//        
//        imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
//
//    }
//		
//    
//	if (alogin.imgBlogo!= nil) {
//		//NSLog(@"Not Right");
//		imgBLogo.image = alogin.imgBlogo;
//		
//	} else {
//		
//		if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
//			
//			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
//			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
//			imgBLogo.image = image;
//			imgPowerdby.hidden=NO;
//			imgtxtPowerdby.hidden=NO;
//		}else {
//			UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
//			imgBLogo.image = img;
//			imgPowerdby.hidden=YES;
//			imgtxtPowerdby.hidden=YES;
//			
//		}
//		
//	}
//	if (alogin.imgGiveAway!= nil) {
//		//NSLog(@"Not Right");
//		imgGiveAway.image = alogin.imgGiveAway;
//		
//	} else {
//		
//		if (![alogin.userbusinessgiveaway isEqualToString:@"0"]) {
//			NSString *str = [alogin.userbusinessgiveaway stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
//			NSURL *url = [[NSURL alloc] initWithString:str];
//			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
//			alogin.imgGiveAway = image;
//			imgGiveAway.image = image;
//			
//		}else {
//			if(appDelegate.flggiveaway)
//			{
//				imgGiveAway.image = nil;	
//			}
//			else {
//				UIImage *img = [UIImage imageNamed:@"giftcard.png"];
//				imgGiveAway.image = img;
//				
//			}
//		}
//	}
	
	


- (void)idleTimerExceededForThanks

{	
	if (appDelegate.sugestionflag==FALSE)
	{
		appDelegate.sugestionflag=TRUE;
		OptionOfHistoryViewController *Objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
        [self.navigationController pushViewController:Objoption animated:YES];
        [Objoption release];	
	}
	
	else
	{
			WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
        [self.navigationController pushViewController:awriteorRecode animated:YES];
        [awriteorRecode release];

	}

		
		
				
	if (appDelegate.idleTimer2) 
	{
		[appDelegate.idleTimer2 invalidate];
		appDelegate.idleTimer2=nil;
	}
}


-(IBAction)Home{
	
	appDelegate.txtcleanflag=FALSE;
	ApplicationValidation *home = [[ApplicationValidation alloc]init];
    [self.navigationController pushViewController:home animated:YES];
    [home release];	
}
-(IBAction)History{
	
	appDelegate.HistoryFlag=TRUE;
	if(appDelegate.LoginConform==FALSE)
	{
		
		HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];
        [self.navigationController pushViewController:historylogin animated:YES];
        [historylogin release];

		appDelegate.AdminFlag=TRUE;
		
	}
	else
	{
		History *history= [[History alloc]init];
        [self.navigationController pushViewController:history animated:YES];
        [history release];
	}
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
		ThreeTabButton *TTab = [[ThreeTabButton alloc] initWithNibName:@"ThreeTabButton" bundle:nil];
        [self.navigationController pushViewController:TTab animated:YES];
        [TTab release];
	}
	
}


-(IBAction) TERMS_CONDITIONS
{
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
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
