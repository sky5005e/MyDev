    //
//  WebAndListingViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 7/25/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "WebAndListingViewController.h"
#import "Review_FrogViewController.h"
#import "LocalBusinesSearchViewController.h"


@implementation WebAndListingViewController

 // Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	
}

-(IBAction) LocalBusinesSearchClick
{
	LocalBusinesSearchViewController *alocalbusiness = [[LocalBusinesSearchViewController alloc]initWithNibName:@"LocalBusinesSearchViewController" bundle:nil];
    [self.navigationController pushViewController:alocalbusiness animated:YES];
    [alocalbusiness release];
}
-(IBAction) BusinessListingCustomerClick
{
	Review_FrogViewController *aReviewFrog = [[Review_FrogViewController alloc]initWithNibName:@"Review_FrogViewController" bundle:nil];
    [self.navigationController pushViewController:aReviewFrog animated:YES];
    [aReviewFrog release];

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
- (void)viewDidUnload {
    [super viewDidUnload];
}


- (void)dealloc {
    [super dealloc];
}


@end
