    //
//  cellHistoy.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "cellHistoy.h"
#import "Historyview.h"
#import "PostedReview.h"


@implementation cellHistoy

@synthesize lblEmail;
@synthesize lblName;
@synthesize lblBDate;
@synthesize lblSDate;
@synthesize btnView;
@synthesize btnDelete;
@synthesize PostedReview;
@synthesize delegate = _delegate;
@synthesize imgGivenRating;
@synthesize imgNoRating;
@synthesize imghalfRating;
@synthesize lblTitle;
@synthesize lblSource;
@synthesize lbltype;
@synthesize lblNO; 
/*
 // The designated initializer.  Override if you create the controller programmatically and want to perform customization that is not appropriate for viewDidLoad.
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    if ((self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil])) {
        // Custom initialization
    }
    return self;
}
*/


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib

- (void)viewDidLoad {
	[super viewDidLoad];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];

}


-(IBAction) btnView_clicked:(id)sender {
	
	
	if (_delegate != nil) {
        int index = [sender tag];
        [_delegate viewClicked:index];
		
    }	
}

-(IBAction) btnDelete_clicked:(id)sender {
	if (_delegate != nil) {
        int index = [sender tag];
        [_delegate deleteClicked:index];
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
