    //
//  HistoryLogin.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 26/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "HistoryLogin.h"
#import <CommonCrypto/CommonDigest.h>
#import "History.h"
#import "xmlHistoryLogin.h"
#import "Review_FrogViewController.h"
#import "Terms_Condition.h"
#import "AdminCoupon.h"
#import "SoundSwitch.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "UpdateViewController.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"

@implementation HistoryLogin
@synthesize txtEmail;
@synthesize txtPassword;

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad 
{
    [super viewDidLoad];
	
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	
	txtEmail.text = appDelegate.admin_email;
	
	[scrView addSubview:viewLogin];
	[scrView setContentSize:CGSizeMake(viewLogin.frame.size.width, viewLogin.frame.size.height)];
	
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillShowNotification:)  
												 name:UIKeyboardWillShowNotification  
											   object:nil];  
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillHideNotification)  
												 name:UIKeyboardWillHideNotification  
											   object:nil]; 
	

}

-(void) viewWillAppear:(BOOL)animated
{
	
	[btnadmin setImage:[UIImage imageNamed:@"NadminD_v2.png"] forState:UIControlStateNormal];
    
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
    if (pathGiveAway !=nil) {   
        
        imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
        
    }
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    if (pathBussinesslogo !=nil) {   
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }
    if (!imgGiveAway.image) {
        UIImage *img = [UIImage imageNamed:@"giftcard.png"];
        imgGiveAway.image = nil;
    }
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
        imgPowerdby.hidden=YES;
        imgtxtPowerdby.hidden=YES;
    }

		
/*	if (alogin.imgBlogo!= nil) {
		//NSLog(@"Not Right");
		imgBLogo.image = alogin.imgBlogo;
		
	} 
	else {
		
		if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
			
			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			imgBLogo.image = image;
			imgPowerdby.hidden=NO;
			imgtxtPowerdby.hidden=NO;
			
			
		}		
	else {
		UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
		imgBLogo.image = img;
		imgPowerdby.hidden=YES;
		imgtxtPowerdby.hidden=YES;
		
	}
	}
	if (alogin.imgGiveAway!= nil) {
			//NSLog(@"Not Right");
			imgGiveAway.image = alogin.imgGiveAway;
			
		} else {
			
			if (![alogin.userbusinessgiveaway isEqualToString:@"0"]) {
				NSString *str = [alogin.userbusinessgiveaway stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
				NSURL *url = [[NSURL alloc] initWithString:str];
				UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
				alogin.imgGiveAway = image;
				imgGiveAway.image = image;
				
			}else {
				if(appDelegate.flggiveaway)
				{
					imgGiveAway.image = nil;	
				}
				else {
					UIImage *img = [UIImage imageNamed:@"giftcard.png"];
					imgGiveAway.image = img;
					
				}
			}
		}
*/

	
	
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

-(IBAction) UPDATEbtn
{
	UpdateViewController *update = [[UpdateViewController alloc] initWithNibName:@"UpdateViewController" bundle:nil];
    [self.navigationController pushViewController:update animated:YES];
    [update release];
}
- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
	}
	
}

-(IBAction)LoginSuccess
{
	[txtEmail resignFirstResponder];
	[txtPassword resignFirstResponder];
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(startAdminLogin) onTarget:self withObject:nil animated:YES];
}	
	
-(void)startAdminLogin
{
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];

	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else
	{
		
		appDelegate.HistoryLoginErorrFlag=TRUE;
		NSString *emailString = txtEmail.text; // storing the entered email in a string.
		
		// Regular expression to checl the email format.
		NSString *emailReg = @"[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}";
		
		NSPredicate *emailTest = [NSPredicate predicateWithFormat:@"SELF MATCHES %@", emailReg];	
		
		NSString *hashkey = [txtPassword text];
		
		//NSString *hash = [hashkey base64Encoding]; 
		NSLog(@"First hashkey:->%@",hashkey);
		// PHP uses ASCII encoding, not UTF
		const char *s = [hashkey cStringUsingEncoding:NSASCIIStringEncoding];
		
		NSLog(@"sstrin==%s",s);
		NSData *keyData = [NSData dataWithBytes:s length:strlen(s)];
		NSLog(@"after Encoding:-%@",keyData);
		
		// This is the destination
		uint8_t digest[CC_SHA1_DIGEST_LENGTH] = {0};
		// This one function does an unkeyed SHA1 hash of your hash data
		CC_SHA1(keyData.bytes, keyData.length, digest);
		
		// Now convert to NSData structure to make it usable again
		NSData *out = [NSData dataWithBytes:digest length:CC_SHA1_DIGEST_LENGTH];
		NSLog(@"Final Answer OUT=%@",out);
		// description converts to hex but puts <> around it and spaces every 4 bytes
		NSString *hash = [out description];
		hash = [hash stringByReplacingOccurrencesOfString:@" " withString:@""];
		hash = [hash stringByReplacingOccurrencesOfString:@"<" withString:@""];
		hash = [hash stringByReplacingOccurrencesOfString:@">" withString:@""];
		//hash is now a string with just the 40char hash value in it
		
		NSLog(@"Final Answer=%@",hash);
		
		
		if([[txtEmail.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtEmail.text==nil)
		{	appDelegate.HistoryLoginErorrFlag=FALSE;
			alertemail = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter Your Email " delegate:self 
												  cancelButtonTitle:@"OK" 
												  otherButtonTitles:nil];
			[alertemail show];
			[alertemail release];
			
		}
		else if (([emailTest evaluateWithObject:emailString] != YES))// || [emailString isEqualToString:@""])
		{
			appDelegate.HistoryLoginErorrFlag=FALSE;
			aleremailtest = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"Please Enter Valid Email (abc@example.com)" delegate:self
													   cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[aleremailtest show];
			[aleremailtest release];
			return;
		}
		else if([[txtPassword.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtPassword.text==nil)
		{	appDelegate.HistoryLoginErorrFlag=FALSE;
			alertpassword = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter Password " delegate:self 
												  cancelButtonTitle:@"OK" 
												  otherButtonTitles:nil];
			[alertpassword show];
			[alertpassword release];
			return;
		}
		else  {
			
			appDelegate.admin_email = [txtEmail text];
			appDelegate.admin_password = hash;
			
			NSLog(@"User Email:->%@",appDelegate.admin_email);
			NSLog(@"User Password:->%@",appDelegate.admin_password);
			
			
			[self getAdminLogin];
		}

	}
	[pool release];
}

-(void)getAdminLogin
{
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	
	NSString * str = [NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=historyLogin&admin_Email=%@&password=%@&version=2",appDelegate.set_APIurl,appDelegate.admin_email,appDelegate.admin_password];
		
	NSLog(@"URL String=%@",str);
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	NSString *webpage = [[NSString alloc] initWithData:xmldata encoding:NSUTF8StringEncoding];
    
	NSLog(@"Web page====%@",webpage);
	
	NSString *responseString = [webpage stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSRange rngAdminIDStart = [responseString rangeOfString:@"<adminEmail>"];
	if (rngAdminIDStart.length > 0) {
		responseString = [responseString substringFromIndex:rngAdminIDStart.location+rngAdminIDStart.length];
	}
	
	NSRange rngAdminIDEnd = [responseString rangeOfString:@"</adminEmail>"];
	
	if (rngAdminIDEnd.length > 0) {
		responseString = [responseString substringToIndex:rngAdminIDEnd.location];
	}


	if ([responseString isEqualToString:@"null"]) {
		appDelegate.LoginConform=FALSE;
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Incorrect Email/Password" delegate:self 
			cancelButtonTitle:@"OK" 
			otherButtonTitles:nil];
		[alert show];
		[alert release];
						  
		
	} else {
		[appDelegate saveUserPreferences:3];
		appDelegate.LoginConform=TRUE;
		OptionOfHistoryViewController *objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
		objoption.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;

        [self.navigationController pushViewController:objoption animated:YES];
        [objoption release];
	}

}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex 
{
	
	if (alertView == alertemail) {
		[txtEmail becomeFirstResponder];
	}
	else if (alertView == aleremailtest) {
		[txtPassword becomeFirstResponder];
		
	}	
	else if (alertView == alertpassword) {
		[txtPassword becomeFirstResponder];
	}
}

- (void)textFieldDidBeginEditing:(UITextField *)textField
{
	viewLogin.frame=CGRectMake(0, -120, 1024, 748);
}
-(void)textFieldDidEndEditing:(UITextField *)textField
{
	viewLogin.frame=CGRectMake(0, 0, 1024, 748);
}

-(void)keyboardWillShowNotification:(NSNotification *)note{	
	
	scrView.frame=CGRectMake(0, 0, 1024, 450);
	
}
-(void)keyboardWillHideNotification
{
	scrView.frame=CGRectMake(0, 0, 1024, 748);
	
}

- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc. that aren't in use.
}


- (void)viewDidUnload {
    [super viewDidUnload];
}

- (void)dealloc {
    [super dealloc];
}


@end
