    //
//  ReviewAutoResponderViewConroller.m
//  reviewfrogbussiness
//
//  Created by AgileMac11 on 10/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "ReviewAutoResponderViewConroller.h"
#import <CoreGraphics/CoreGraphics.h>
#import <QuartzCore/QuartzCore.h>
#import "LoginInfo.h"
#import "WriteORRecodeViewController.h"
#import "HistoryLogin.h"
#import "OptionOfHistoryViewController.h"
#import "Terms_Condition.h"

@implementation ReviewAutoResponderViewConroller
@synthesize autoresponderonetext;
@synthesize autorespondertwotext;
@synthesize surveylink;
 

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
	
	[super viewDidLoad];

	appDelegate=(Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];

	
	[scroll addSubview:viewAuto];
	[scroll setContentSize:CGSizeMake(viewAuto.frame.size.width, viewAuto.frame.size.height)];
	
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillShowNotification:)  
												 name:UIKeyboardWillShowNotification  
											   object:nil];  
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillHideNotification)  
												 name:UIKeyboardWillHideNotification  
											   object:nil]; 
	
	
	
	responder1Txtvw.delegate=self;
	responder2Txtvw.delegate=self;
	subscribeTxt.delegate=self;
	
	
	responder1Txtvw.layer.borderWidth=2;
	responder2Txtvw.layer.borderWidth=2;
	responder1Txtvw.layer.borderColor=[[UIColor blackColor] CGColor];
	responder2Txtvw.layer.borderColor=[[UIColor blackColor] CGColor];
	aloginingo = [appDelegate.delArrUerinfo objectAtIndex:0];
	[self GetAutoResponderinto];
		
	
}
-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}


-(IBAction) Admin
{	
	if(appDelegate.LoginConform==FALSE)
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

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}

-(void) GetAutoResponderinto
{
	HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(startLodingAutoResponder) onTarget:self withObject:nil animated:YES];
	
}
-(void)startLodingAutoResponder
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
	
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?version=2",appDelegate.set_APIurl]];
	
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    
	[request setHTTPMethod:@"POST"];
    
	NSData *requestBody = [[NSString stringWithFormat:@"api=itunesversion1_1_autoresponder&businesscategoryid=%@",aloginingo.userbusinessid] dataUsingEncoding:NSUTF8StringEncoding];
    
	NSLog(@"api=itunesversion1_1_autoresponder&businesscategoryid=%@",aloginingo.userbusinessid);
    
	[request setHTTPBody:requestBody];
    
	NSURLResponse *response = NULL;
    
	NSError *requestError = NULL;
    
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
    
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
    
	NSLog(@"responseString=%@",responseString);	
    
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	
	NSRange rngautoresponderonetextStart = [responseString rangeOfString:@"<autoresponderonetext>"];
    
	if (rngautoresponderonetextStart.length > 0) {
        
		self.autoresponderonetext = [responseString substringFromIndex:rngautoresponderonetextStart.location+rngautoresponderonetextStart.length];
		
		NSRange rngautoresponderonetextEnd = [self.autoresponderonetext rangeOfString:@"</autoresponderonetext>"];
        
		if (rngautoresponderonetextEnd.length > 0) {
            
			self.autoresponderonetext = [self.autoresponderonetext substringToIndex:rngautoresponderonetextEnd.location];
				
		}		
        
	} 
    
    else {
        
		self.autoresponderonetext = @"";
	}
	
	NSRange rngautorespondertwotextStart = [responseString rangeOfString:@"<autorespondertwotext>"];
    
	if (rngautorespondertwotextStart.length > 0) {
        
		self.autorespondertwotext = [responseString substringFromIndex:rngautorespondertwotextStart.location+rngautorespondertwotextStart.length];
		
		NSRange rngautorespondertwotextEnd = [self.autorespondertwotext rangeOfString:@"</autorespondertwotext>"];
        
		if (rngautorespondertwotextEnd.length > 0) {
            
			self.autorespondertwotext = [self.autorespondertwotext substringToIndex:rngautorespondertwotextEnd.location];
			NSLog(@"autorespondertwotext:-%@",self.autorespondertwotext);
			
		}		
	}
    
    else {
        
		self.autorespondertwotext = @"";
	}
	
	NSRange rngsurveylinkStart = [responseString rangeOfString:@"<surveylink>"];
	if (rngsurveylinkStart.length > 0) {
		self.surveylink = [responseString substringFromIndex:rngsurveylinkStart.location+rngsurveylinkStart.length];
		
		NSRange rngsurveylinkEnd = [self.surveylink rangeOfString:@"</surveylink>"];
		if (rngsurveylinkEnd.length > 0) {
			self.surveylink = [self.surveylink substringToIndex:rngsurveylinkEnd.location];
			

		}		
	} else {
		self.surveylink = @"";
	}

	[self performSelectorOnMainThread:@selector(loadcomplite) withObject:nil waitUntilDone:YES];
	
	
	
	[pool release];

}

-(void)loadcomplite
{
	responder1Txtvw.text = self.autoresponderonetext;
	responder2Txtvw.text = self.autorespondertwotext;
	subscribeTxt.text = self.surveylink;
}
	
		

//- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)text
//{
//	if([text isEqualToString:@"\n"])
//	{
//		[textView resignFirstResponder];
//		return NO;
//	}
//	return YES;
//}
//
//- (BOOL)textViewShouldBeginEditing:(UITextView *)textView
//{
//	
//	return YES;
//}
//
//- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string  
//{
//	if([string isEqualToString:@"\n"])
//	{
//		[textField resignFirstResponder];
//		return NO;
//	}
//	return YES;
//}




-(IBAction)submit
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?version=2",appDelegate.set_APIurl]];
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	[request setHTTPMethod:@"POST"];
	NSData *requestBody = [[NSString stringWithFormat: @"api=itunesversion1_1_autoresponder_update&businesscategoryid=%@&autoresponderonetext=%@&autorespondertwotext=%@&surveylink=%@",aloginingo.userbusinessid,responder1Txtvw.text,responder2Txtvw.text,subscribeTxt.text] dataUsingEncoding:NSUTF8StringEncoding];
	[request setHTTPBody:requestBody];
	NSLog(@"%@api=itunesversion1_1_autoresponder_update&businesscategoryid=%d&autoresponderonetext=%@&autorespondertwotext=%@&surveylink=%@",url,1,responder1Txtvw.text,responder2Txtvw.text,subscribeTxt.text);
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\%@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	if ([responseString isEqualToString:@"1"])
	{        
		UIAlertView *altSuccess = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Submitted" delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altSuccess show];
		[altSuccess release];
	}
	else 
	{
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Failed to submit" delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
	}
}
-(IBAction) BackClick
{
    OptionOfHistoryViewController *obj= [[OptionOfHistoryViewController alloc]init];
    [self.navigationController pushViewController:obj animated:YES];
    [obj release];
//	[self dismissModalViewControllerAnimated:YES];
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

-(void)keyboardWillShowNotification:(NSNotification *)note{	
	
	scroll.frame=CGRectMake(0, 0, 1024, 450);
	
	
}
-(void)keyboardWillHideNotification
{
	scroll.frame=CGRectMake(0, 0, 1024, 748);
	
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
