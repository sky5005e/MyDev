    //
//  suggestionViewController.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 11/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "suggestionViewController.h"
#import "Thank_you.h"

#import "ApplicationValidation.h"
#import "Terms_Condition.h"
#import "HistoryLogin.h"
#import "ThreeTabButton.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"


@implementation suggestionViewController
@synthesize txtSuggestion;
@synthesize btncontinue;


 // The designated initializer.  Override if you create the controller programmatically and want to perform customization that is not appropriate for viewDidLoad.
/*
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization.
    }
    return self;
}
*/


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	btncontinue.backgroundColor=[UIColor clearColor];
	[txtSuggestion becomeFirstResponder];
}



-(IBAction) submitsuggestion
{
	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else if([[txtSuggestion.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtSuggestion.text == nil) {
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please enter suggestion." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else
	{
		appDelegate.sugestionflag=FALSE;
		appDelegate.Suggestion=[txtSuggestion text];
		
		NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?version=2",appDelegate.set_APIurl]];
		NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
		[request setHTTPMethod:@"POST"];
		NSData *requestBody = [[NSString stringWithFormat:@"api=postsuggestion&fromUserIpad=%@&toAdminEmail=%@&suggestionText=%@",appDelegate.UserEmail,appDelegate.admin_email,appDelegate.Suggestion] dataUsingEncoding:NSUTF8StringEncoding];
		NSLog(@"api=postsuggestion&fromUserIpad=%@&toAdminEmail=%@&suggestionText=%@",appDelegate.UserEmail,appDelegate.admin_email,appDelegate.Suggestion);
		[request setHTTPBody:requestBody];

		//[request setHTTPBody:requestBody];
		NSURLResponse *response = NULL;
		NSError *requestError = NULL;
		NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
		NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
		NSLog(@"responseString=%@",responseString);
		
		//return
		
		responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
		;
		
		
		NSRange rngAdminIDStart = [responseString rangeOfString:@"<return>"];
		if (rngAdminIDStart.length > 0) {
			responseString = [responseString substringFromIndex:rngAdminIDStart.location+rngAdminIDStart.length];
		}
		
		NSRange rngAdminIDEnd = [responseString rangeOfString:@"</return>"];
		
		if (rngAdminIDEnd.length > 0) {
			responseString = [responseString substringToIndex:rngAdminIDEnd.location];
		}
		
		
		if ([responseString isEqualToString:@"1"])
		{
			Thank_you *Thank = [[Thank_you alloc]init];
            [self.navigationController pushViewController:Thank animated:YES];
            [Thank release];
		}
		else
		{
			NSLog(@"Not Send");
		}
		
	}
		
}
-(IBAction) BackClick
{
    OptionOfHistoryViewController *objoption= [[OptionOfHistoryViewController alloc]init];
    [self.navigationController pushViewController:objoption animated:YES];
    [objoption release];
	//[self dismissModalViewControllerAnimated:YES];
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

-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];

}
-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
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
		OptionOfHistoryViewController *objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
        [self.navigationController pushViewController:objoption animated:YES];
        [objoption release];
	}
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
