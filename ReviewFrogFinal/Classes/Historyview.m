//
//  Historyview.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Historyview.h"
#import "History.h"
#import "xmlDelete.h"
#import "xmlPost.h"
#import "User.h"
#import "LoginInfo.h"




@implementation Historyview
@synthesize selectedUser;
@synthesize lblEmail;
@synthesize lblName;
@synthesize lblBDate;
@synthesize lblSDate;
@synthesize txtDesc;
@synthesize history;
@synthesize appDelegate;
@synthesize btnDelete;
@synthesize btnPost;
@synthesize lblDelete;
@synthesize lblPost;
@synthesize Frogimg;
@synthesize internalimg;
@synthesize postimg;
@synthesize lblTitle;
@synthesize lblCategory;

- (void)viewDidLoad {
    [super viewDidLoad];
	
	appDelegate=(Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	
	history= [[History alloc]init];
	
	lblEmail.text=selectedUser.tableDataEmail;
	lblName.text=selectedUser.tableDataName;
	lblBDate.text=selectedUser.tableDataCity;
	lblSDate.text=selectedUser.tableDataDate;
	lblTitle.text=selectedUser.tableDataTitle;
	txtDesc.text=selectedUser.tableDataDescription;
	lblCategory.text = selectedUser.tableDataCategory;
	
	[scrView addSubview:viewHistory];
	[scrView setContentSize:CGSizeMake(viewHistory.frame.size.width, viewHistory.frame.size.height)];	
    
	if (![selectedUser.tablequestionone isEqualToString:@"0"]) {
		
		NSArray *Strinlist = [selectedUser.tablequestionone componentsSeparatedByString:@":"];
		lblQuesionOne.text = [Strinlist objectAtIndex:0];
		lblAnswerOne.text  = [Strinlist objectAtIndex:1];
        
	}
	
	if (![selectedUser.tablequestiontwo isEqualToString:@"0"]) {
		
		NSArray *Strinlist = [selectedUser.tablequestiontwo componentsSeparatedByString:@":"];
		lblQuesionTwo.text = [Strinlist objectAtIndex:0];
		lblAnswerTwo.text  = [Strinlist objectAtIndex:1];
        
	}
	if (![selectedUser.tablequestionthree isEqualToString:@"0"]) {
		
		NSArray *Strinlist   = [selectedUser.tablequestionthree componentsSeparatedByString:@":"];
		lblQuesionThree.text = [Strinlist objectAtIndex:0];
		lblAnswerThree.text  = [Strinlist objectAtIndex:1];
		
	}
	if([selectedUser.tablequestionone isEqualToString:@"(null):"])  {
		lblQuesionOne.text = @"-";
		lblAnswerOne.text  = @"-";
	}
	if ([selectedUser.tablequestiontwo isEqualToString:@"(null):"]) {
		
		lblQuesionTwo.text = @"-";
		lblAnswerTwo.text = @"-";
		
	}
	if ([selectedUser.tablequestionthree isEqualToString:@"(null):"]) {
		
		lblQuesionThree.text = @"-";
		lblAnswerThree.text  = @"-";
		
	}
	
	btnDelete.backgroundColor = [UIColor clearColor];
	btnPost.backgroundColor = [UIColor clearColor];
	
}
-(void)viewWillAppear:(BOOL)animated
{
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
        
    }
	
	/*LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
     
     if (alogin.imgBlogo!= nil) {
     //NSLog(@"Not Right");
     imgBLogo.image = alogin.imgBlogo;
     
     } else {
     
     if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
     
     NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
     UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
     imgBLogo.image = image;
     
     
     }
     else {
     UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
     imgBLogo.image = img;
     }
     
     
     }
     if (alogin.imgGiveAway!= nil) {
     NSLog(@"Not Right");
     imgGiveAway.image = alogin.imgGiveAway;
     
     } else {
     
     if (![alogin.userbusinessgiveaway isEqualToString:@"0"]) {
     NSString *str = [alogin.userbusinessgiveaway stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
     NSURL *url = [[NSURL alloc] initWithString:str];
     UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
     alogin.imgGiveAway = image;
     imgGiveAway.image = image;
     
     }else {
     UIImage *img = [UIImage imageNamed:@"giftcard.png"];
     imgGiveAway.image = img;
     }
     }
     */	
	
}
-(IBAction)Backevent{
    appDelegate.HViewClick=TRUE;
	[self dismissModalViewControllerAnimated:YES];
}
-(IBAction)DeleteClickEvent{
	
	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else if ([selectedUser.tableDatauserStatus isEqualToString:@"D"]) {
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"This Review is already internal" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	
	else
	{
		alertConfirm = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are You Sure Want To Keep This Review Internal?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes",nil];
		[alertConfirm show];
		[alertConfirm release];
	}
}
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex 
{
	if (alertView == alertConfirm) {
		if (buttonIndex == 0) {
			NSLog(@"alertConfirmNo");
		} else {
			appDelegate.flgDismissview=TRUE;
			appDelegate.flgchangeD=TRUE;
			[self DataDelete];
			
        }
	}else if (alertView==alertdeleted) {
		[self dismissModalViewControllerAnimated:YES];
		
	}else if (alertView==alertposted) {
		appDelegate.flgDismissview=TRUE;
		appDelegate.flgchangeA=TRUE;
        [self dismissModalViewControllerAnimated:YES];
		
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

-(void) DataDelete 
{
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(Startdeleting) onTarget:self withObject:nil animated:YES];
    
}
-(void)Startdeleting
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php&version=2",appDelegate.ChangeUrl]];
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	[request setHTTPMethod:@"POST"];
	NSData *requestBody = [[NSString stringWithFormat:@"api=internal&data_id=%d",selectedUser.tableDataID] dataUsingEncoding:NSUTF8StringEncoding];
	NSLog(@"api=internal&data_id=%d",selectedUser.tableDataID);
	[request setHTTPBody:requestBody];
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	NSLog(@"responseString=%@",responseString);
    
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];	
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
		alertdeleted = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Review Updated To Kept Internally" delegate:self 
                                        cancelButtonTitle:@"OK" 
                                        otherButtonTitles:nil];
        [alertdeleted show];
        [alertdeleted release];
		NSLog(@"Done");	}
	else
	{
		NSLog(@"Not Done");
	}
	
	[pool release];	
	
}
-(IBAction)PostClickEvent{
	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else
	{	
		[self DataPostStatus];
	}
	
	
}
-(void)DataPostStatus{
	// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(StartPostStatus) onTarget:self withObject:nil animated:YES];
	
}
-(void)StartPostStatus
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
	if ([selectedUser.tableDatauserStatus isEqualToString:@"A"]) {
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"This Review is already published" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else {
        
        NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php&version=2",appDelegate.ChangeUrl]];
        NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
        [request setHTTPMethod:@"POST"];
        NSData *requestBody = [[NSString stringWithFormat:@"api=UpdateUserData&user_id=%d&status=A",selectedUser.tableDataID] dataUsingEncoding:NSUTF8StringEncoding];
        NSLog(@"api=UpdateUserData&user_id=%d&status=A",selectedUser.tableDataID);
        [request setHTTPBody:requestBody];
        NSURLResponse *response = NULL;
        NSError *requestError = NULL;
        NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
        NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
        NSLog(@"responseString=%@",responseString);
        [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
        
        NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:responseData];
        
        //Initialize the delegate.
        xmlPost *parser = [[xmlPost alloc] initxmlPost];
        
        //Set delegate
        [xmlParser setDelegate:parser];
        
        //Start parsing the XML file.
        BOOL success = [xmlParser parse];
        
        if(success){
            NSLog(@"No Errors");
			
			alertposted = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Record Posted Successfully " delegate:self 
                                           cancelButtonTitle:@"OK" 
                                           otherButtonTitles:nil];
			[alertposted show];
			[alertposted release];	
        }
        else
            NSLog(@"Error Error Error!!!");
        [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
        
	}
	[pool release];
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
