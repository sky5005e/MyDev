    //
//  ClientBusinessSplashScreemViewController.m
//  Review Frog
//
//  Created by agilepc-32 on 10/18/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "ClientBusinessSplashScreemViewController.h"
#import "WriteORRecodeViewController.h"


@implementation ClientBusinessSplashScreemViewController
@synthesize Bsplash;
@synthesize imgB;


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	Bsplash.image =imgB;
	TimeProcess = [NSTimer scheduledTimerWithTimeInterval:5 target:self selector:@selector(GoToNextScreen) userInfo:nil repeats:NO];
}
-(void)GoToNextScreen
{
	[TimeProcess invalidate];
	TimeProcess = nil;
	WriteORRecodeViewController *awriteRecord= [[WriteORRecodeViewController alloc]init];
    [self.navigationController pushViewController:awriteRecord animated:YES];
    [awriteRecord release];
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


- (void)viewDidUnload {
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}


- (void)dealloc {
    [super dealloc];
}


@end
