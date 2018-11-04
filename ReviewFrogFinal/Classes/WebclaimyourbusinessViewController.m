    //
//  WebclaimyourbusinessViewController.m
//  Review Frog
//
//  Created by agilepc-32 on 10/24/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "WebclaimyourbusinessViewController.h"


@implementation WebclaimyourbusinessViewController
@synthesize ActiviIndicator;
@synthesize businessid;


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
    NSLog(@"~~~~~~~WebclaimyourbusinessViewController~~~~~~");
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	
	[self.ActiviIndicator startAnimating];
	NSString *strUrl = [NSString stringWithFormat:@"http://www.reviewfrog.com/claimyourbusiness/%d",appDelegate.businessid];
	NSURL *url = [NSURL URLWithString:strUrl];
	urlReq = [[NSURLRequest alloc] initWithURL:url]; 
	[web loadRequest:urlReq];
	
}

-(IBAction)BackClcik
{	[self.ActiviIndicator stopAnimating];
    [self dismissModalViewControllerAnimated:YES];
//	BusinessSearchListViewController *objwebview = [[BusinessSearchListViewController alloc] initWithNibName:@"BusinessSearchListViewController" bundle:nil];   
//    [self.navigationController pushViewController:objwebview animated:YES];
//    [objwebview release];
}
-(void)webViewDidFinishLoad:(UIWebView *)webView
{
	[self.ActiviIndicator stopAnimating];
}

- (void)viewWillDisappear:(BOOL)animated
{
	[web loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:@"about:blank"]]];
	
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
