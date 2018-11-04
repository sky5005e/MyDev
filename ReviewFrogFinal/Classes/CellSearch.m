    //
//  CellSearch.m
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "CellSearch.h"


@implementation CellSearch
@synthesize lblbusinessname;
@synthesize lblbusinessAdd;
@synthesize lblReviewCount;
@synthesize lblServices;
@synthesize lblAccreditations;
@synthesize lblPhone;


@synthesize imgGivenRating;
@synthesize imgNoRating;
@synthesize imghalfRating;
@synthesize imgcheck;
@synthesize lblNo;
@synthesize btnCliamyourbusiness;

@synthesize mon_am; 
@synthesize mon_pm; 
@synthesize tue_am; 
@synthesize tue_pm; 
@synthesize wed_am; 
@synthesize wed_pm; 
@synthesize thru_am;
@synthesize thru_pm;
@synthesize fri_am; 
@synthesize fri_pm; 
@synthesize sat_am; 
@synthesize sat_pm; 
@synthesize sun_am;
@synthesize sun_pm;

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
