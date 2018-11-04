    //
//  UpdateViewController.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 12/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "UpdateViewController.h"
#import "ApplicationValidation.h"
#import "HistoryLogin.h"
#import "ThreeTabButton.h"
#import "Terms_Condition.h"
 




@implementation UpdateViewController





// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
}

-(IBAction) BACKbtn
{

	[self dismissModalViewControllerAnimated:YES];
	
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
