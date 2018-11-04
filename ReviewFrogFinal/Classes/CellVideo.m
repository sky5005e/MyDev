    //
//  CellVideo.m
//  Review Frog
//
//  Created by AgileMac4 on 6/27/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "CellVideo.h"


@implementation CellVideo

@synthesize lblName;
@synthesize lblSDate;
@synthesize imgGivenRating;
@synthesize imgNoRating;
@synthesize imghalfRating;
@synthesize lblVideoTitle;
@synthesize btnDelete;
@synthesize lblCategory;
@synthesize lblStatus;
@synthesize lblNO;

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
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
