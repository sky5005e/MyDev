    //
//  CellResult.m
//  Review Frog
//
//  Created by agilepc-32 on 10/10/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "CellResult.h"


@implementation CellResult

@synthesize lblBusinessName;
@synthesize lblBusinessAdd;
@synthesize lblReviewCounter;
@synthesize lblReviewTitle;
@synthesize lblPostedbyName;
@synthesize lblDescription;

@synthesize lblDate;
@synthesize lblNo;

@synthesize imgReview;
@synthesize imgGivenRating;
@synthesize imgNoRating;
@synthesize imghalfRating;
@synthesize txtDescription;

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
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
