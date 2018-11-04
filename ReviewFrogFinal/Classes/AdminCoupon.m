    //
//  AdminCoupon.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 27/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "AdminCoupon.h"
#import "Review_FrogViewController.h"
#import "Terms_Condition.h"
#import "HistoryLogin.h"
#import "History.h"
#import "Cityname.h"
#import "XMLCityname.h"
#import "xmlAdmincity.h"
#import "xmlAdminCityCounter.h"
#import "Thank_you.h"
#import "SoundSwitch.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"
@implementation AdminCoupon

@synthesize btnsumhidde;
@synthesize txtCoupon;
@synthesize Businessname;
@synthesize Description;
@synthesize CitysName;
@synthesize peopleCounter;
@synthesize Businessphone;
@synthesize BusinessAddress;
@synthesize btnCity;
@synthesize pickercity;
@synthesize txtAllcity;
@synthesize Allcity;
@synthesize lblAdminCityCounter;
@synthesize FlgExit;
@synthesize txtBusinessname;
@synthesize txtBusinessphone;
@synthesize txtBusinessAddress;
@synthesize AEmail;
@synthesize ViewCoupon;
@synthesize btncontinue3;
@synthesize Couponeoffers;
@synthesize CouponeEdate;
@synthesize fistCity;
@synthesize fistCity2;
@synthesize pickerflg1;

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
	fistCity=TRUE;
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	
	btncontinue3.backgroundColor=[UIColor clearColor];
	btnCity.hidden=YES;
	searchA.hidden=YES;
	txtAllcity.hidden=YES;
	lblAdminCityCounter.hidden=YES;
	
		AEmail= [[NSString alloc]init];
		Businessname= [[NSString alloc]init];
		Description= [[NSString alloc]init];
		CitysName= [[NSString alloc]init];
		peopleCounter= [[NSString alloc]init];
		Businessphone=[[NSString alloc]init];
		BusinessAddress= [[NSString alloc]init];
	
	[scrlView addSubview:ViewCoupon];
	[scrlView setContentSize:CGSizeMake(ViewCoupon.frame.size.width, ViewCoupon.frame.size.height)];
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillShowNotification:)  
												 name:UIKeyboardWillShowNotification  
											   object:nil];  
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillHideNotification)  
												 name:UIKeyboardWillHideNotification  
											   object:nil]; 
		
	
	btnsumhidde.backgroundColor=[UIColor clearColor];
	self.pickercity.hidden = YES;
	tlBar.hidden = YES;
	
	Allcity=[[NSMutableArray alloc]init];
	
	
		
	appDelegate.delArrCitylist=[[NSMutableArray alloc]init];
		
	//[self GetAllCity];
	[self GetAdminCity];
	
	
	
	if(![appDelegate.AdminCity isEqualToString:@""]) {
		[btnCity setTitle:appDelegate.AdminCity forState:UIControlStateNormal];
		[Allcity addObject:appDelegate.AdminCity];
		
		txtAllcity.text=appDelegate.AdminCity;
		NSLog(@"txtallcity-------%@",txtAllcity.text);
		
	}
	
	[self AdminCityCounter];
	
	searchA.delegate=self;
	
	lblAdminCityCounter.text=appDelegate.admincitycounter;

}

-(void)searchBar:(UISearchBar *)searchBar textDidChange:(NSString *)searchText{
	
	int len=[searchText length];
	NSLog(@"search txt %@",searchText);
	appDelegate.strSearch = searchText;
	NSLog(@"appDelegate.strSearch---%@",appDelegate.strSearch);
	appDelegate.strSearch = [appDelegate.strSearch stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
	
	if(len == 4)
	{
		if(appDelegate.remoteHostStatus==0)
		{
			UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
			[alert show];
			[alert release];
		}
		else
		{
			[self GetAllCity];
		}
		
		
	}
}


- (void) viewWillAppear:(BOOL)animated
{
    imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:appDelegate.pathBussinesslogo] autorelease];    
   // imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];

/*	LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
	if (alogin.imgBlogo!= nil) {
		//NSLog(@"Not Right");
		imgBLogo.image = alogin.imgBlogo;
		
	} else {
		
		if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
			
			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			imgBLogo.image = image;
			imgPowerdby.hidden=NO;
			imgtxtPowerdby.hidden=NO;
			
			
		}else {
			UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
			imgBLogo.image = img;
			imgPowerdby.hidden=YES;
			imgtxtPowerdby.hidden=YES;
			
		}
		
	}*/
	
	
}
- (void)viewWillDisappear:(BOOL)animated
{
		
}

-(void)keyboardWillShowNotification:(NSNotification *)note{	
	
	scrlView.frame=CGRectMake(0, 0, 1024, 450);

		
}
-(void)keyboardWillHideNotification
{
	scrlView.frame=CGRectMake(0, 0, 1024, 748);
	
}
- (BOOL)textViewShouldBeginEditing:(UITextView *)textView
{
	txtCoupon.text=@"";
	
	return YES;
}

-(void) GetAllCity {
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	NSString * str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=searchCity&city=%@&version=2",appDelegate.ChangeUrl,appDelegate.strSearch];
	
	NSLog(@"URL String=%@",str);
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	
		//Initialize the delegate.
	XMLCityname *parser = [[XMLCityname alloc] initXMLCityname];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		NSLog(@"No Errors");
		[pickercity reloadAllComponents];
		
		if([txtCoupon isFirstResponder])
			[txtCoupon resignFirstResponder];
		if([txtBusinessname isFirstResponder])
			[txtBusinessname resignFirstResponder];
		if([txtBusinessname isFirstResponder])
			[txtBusinessname resignFirstResponder];
		if([txtBusinessphone isFirstResponder])
			[txtBusinessphone resignFirstResponder];
		if([searchA isFirstResponder])
			[searchA resignFirstResponder];
		if([txtBusinessAddress isFirstResponder])
			[txtBusinessAddress resignFirstResponder];
		if([txtCouponeoffers isFirstResponder])
			[txtCouponeoffers resignFirstResponder];
		if([txtCouponeEdate isFirstResponder])
			[txtCouponeEdate resignFirstResponder];
		
		
		self.pickercity.hidden = NO;
		tlBar.hidden = NO;
		pickerflg1=TRUE;
		
		
		
	} else
		NSLog(@"Error Error Error!!! coupon");
	
	//[self performSelectorOnMainThread:@selector(loadingComplete) withObject:nil waitUntilDone:YES];
	[pool release];
	
}
-(void)GetAdminCity
{
	
	//NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	NSString * str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=userCity&user_id=%@&version=2",appDelegate.set_APIurl,appDelegate.userId];
	
	NSLog(@"URL String=%@",str);
	
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
			
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	xmlAdmincity *parser = [[xmlAdmincity alloc] initxmlAdmincity];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success){
		NSLog(@"No Errors");
				
	}
			else{
				NSLog(@"Error Error Error!!!");
			}
		
	[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
}


-(void)AdminCityCounter
{
	
	NSLog(@"TextField-------%@",txtAllcity.text);
	
	NSString * str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=count&cityName=%@&version=2",appDelegate.ChangeUrl,txtAllcity.text];
	
	NSLog(@"URL String=%@",str);
	
	
	NSURL *url = [NSURL URLWithString:[str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	xmlAdminCityCounter *parser = [[xmlAdminCityCounter alloc] initxmlAdminCityCounter];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success)
		NSLog(@"No Errors");
	else
		NSLog(@"Error Error Error!!!");
	[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
}


-(IBAction) btnCity_clicked
{
	
	if(pickerflg1==FALSE)
	{
		self.pickercity.hidden = NO;
		tlBar.hidden = NO;

		pickerflg1=TRUE;
	}
	else
	{
		self.pickercity.hidden = YES;
		tlBar.hidden = YES;
		
		pickerflg1=FALSE;	
	}
	
	
	
		
}
-(void)refreshcity
{	
	txtAllcity.text = @"";
	
	for(int i=0;i<[Allcity count];i++)
	{
				txtAllcity.text=[txtAllcity.text stringByAppendingString:[Allcity objectAtIndex:i]];
			if(i!=[Allcity count]-1)
				txtAllcity.text=[txtAllcity.text stringByAppendingString:@","];
	}
}

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
	
	return 1;
	
}

- (NSInteger)pickerView:(UIPickerView *)thePickerView numberOfRowsInComponent:(NSInteger)component {
	NSLog(@"Citylist counter-----%d",[appDelegate.CityList count]);
	return [appDelegate.CityList count];
	
}

- (NSString *)pickerView:(UIPickerView *)thePickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component {
	
	Cityname *objcityname = [appDelegate.CityList objectAtIndex:row];
				
		
		return objcityname.cityName;
	
}

- (void)pickerView:(UIPickerView *)thePickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component {
	
		Cityname *objcityname = [appDelegate.CityList objectAtIndex:row];
		btnCity.enabled = YES;
				
		[btnCity setTitle:objcityname.cityName forState:UIControlStateNormal];
		
		selectedcity=objcityname.cityName;
	fistCity=FALSE;
		
	
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
-(IBAction) Home{
	appDelegate.txtcleanflag=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}
-(IBAction) TERMS_CONDITIONS
{
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
	
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}
-(IBAction) History{
	appDelegate.HistoryFlag=TRUE;
	if(appDelegate.LoginConform==FALSE)
	{
		
		HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];
        [self.navigationController pushViewController:historylogin animated:YES];
        [historylogin release];
		appDelegate.AdminFlag=TRUE;
		
	}
	else
	{
		History *history= [[History alloc]init];
        [self.navigationController pushViewController:history animated:YES];
        [history release];
		
	}
	
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

-(IBAction) LogOut
{
	appDelegate.LoginConform=FALSE;
	ApplicationValidation *home = [[ApplicationValidation alloc]init];
    [self.navigationController pushViewController:home animated:YES];
    [home release];
	
}

-(IBAction) SubmitCoupon
{
	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else if([[txtCouponeoffers.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet] ]isEqualToString:@""] ||txtCouponeoffers.text == nil) {
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please enter offer value." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
		
	}
	else if([[txtCouponeEdate.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""]||txtCouponeEdate.text == nil)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please enter coupon expiration date." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else if([[txtBusinessphone.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtBusinessphone.text == nil) {
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please enter business phone number." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else if([[txtBusinessAddress.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtBusinessAddress.text == nil) {
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please enter business address." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
		
	}
	else if ([[txtBusinessname.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtBusinessname.text == nil) {
		
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please enter business name." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
		
	}
	else if([[txtCoupon.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtCoupon.text == nil) {
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please enter description." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else
	{
		appDelegate.sugestionflag=TRUE;
		Couponeoffers=[txtCouponeoffers text];
		CouponeEdate=[txtCouponeEdate text];
		appDelegate.admin_email;
		appDelegate.AdminEmail1;
		Businessname=[txtBusinessname text];
		Description=[txtCoupon text];
		CitysName=[txtAllcity text];
		peopleCounter=[lblAdminCityCounter text];
		Businessphone=[txtBusinessphone text];
		BusinessAddress=[txtBusinessAddress text];
		
		NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php&version=2",appDelegate.ChangeUrl]];
		
		NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
		[request setHTTPMethod:@"POST"];
		NSData *requestBody = [[NSString stringWithFormat:@"api=mail&admin_Email=%@&cityName=%@&cityCount=%@&businessName=%@&phone=%@&businessAddress=%@&description=%@&offer=%@&expireDate=%@&UserIpad_Email=%@",appDelegate.admin_email,CitysName,peopleCounter,Businessname,Businessphone,BusinessAddress,Description,Couponeoffers,CouponeEdate,appDelegate.UserEmail] dataUsingEncoding:NSUTF8StringEncoding];
		NSLog(@"api=mail&admin_Email=%@&cityName=%@&cityCount=%@&businessName=%@&phone=%@&businessAddress=%@&description=%@&offer=%@&expireDate=%@&UserIpad_Email=%@",appDelegate.admin_email,CitysName,peopleCounter,Businessname,Businessphone,BusinessAddress,Description,Couponeoffers,CouponeEdate,appDelegate.UserEmail);
		[request setHTTPBody:requestBody];
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
		else {
			
			UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please Enter Valid Data" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
			[alert show];
			[alert release];
			
			
		}

		
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
