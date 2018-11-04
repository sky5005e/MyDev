    //
//  BussinessProfileViewController.m
//  reviewfrogbussiness
//
//  Created by AgileMac11 on 10/15/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BussinessProfileViewController.h"
#import "ReviewAutoResponderViewConroller.h"
#import "LoginInfo.h"
#import "Thank_you.h"
#import "XMLBusinessProfile.h"
#import "LoginInfo.h"
#import <MobileCoreServices/MobileCoreServices.h>
#import "WriteORRecodeViewController.h"
#import "HistoryLogin.h"
#import "OptionOfHistoryViewController.h"
#import "Terms_Condition.h"
#import "LoginInfo.h"

/*Black
Default
Your Choice
*/

@implementation BussinessProfileViewController

 
// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
	
	[super viewDidLoad];

	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate]; 
	
	
	
	[SwhBusiness setOn:appDelegate.flgswich];
	
	
	imgPck=[[UIImagePickerController alloc] init];
	imgPck.delegate=self;
	
	que1Txt.delegate=self;
	que2Txt.delegate=self;
	que3Txt.delegate=self;
	
	imgPck.sourceType=UIImagePickerControllerSourceTypePhotoLibrary;
	objlogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
	[scrollVw addSubview:viewBusinessProfile];
	[scrollVw setContentSize:CGSizeMake(viewBusinessProfile.frame.size.width, viewBusinessProfile.frame.size.height)];
	
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillShowNotification:)  
												 name:UIKeyboardWillShowNotification  
											   object:nil];  
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillHideNotification)  
												 name:UIKeyboardWillHideNotification  
											   object:nil]; 
	[self getbusinessprofile];
	
	logosaveBtn.hidden=TRUE;
	logoremoveBtn.hidden=TRUE;
	splashremoveBtn.hidden=TRUE;
	splashsaveBtn.hidden=TRUE;
	
	preVw =[[UIView alloc] initWithFrame:CGRectMake(112,200,800,500)];

	closeButton = [[UIButton alloc]initWithFrame:CGRectMake(700,0,160, 40)];
	[closeButton addTarget:self action:@selector(closeButtonClicked) forControlEvents:UIControlEventTouchUpInside];
	[closeButton setTitle:@"Close Preview" forState:normal];
	[closeButton setTitleColor:[UIColor blueColor] forState:normal];
	[preVw addSubview:closeButton];
	[self.view addSubview:preVw];
	preVw.hidden = TRUE;
	
	alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
/*	if (alogin.imgBlogo!= nil) {
//		NSLog(@"Not Right");
//			logoImgvw.image =  alogin.imgBlogo;
//		logoremoveBtn.hidden = FALSE;
//		logosaveBtn.hidden = TRUE;
//		
//	} 
//	if (alogin.imgBsplash != nil)
//	{
//		splashImgvw.image =  alogin.imgBsplash;
//		splashremoveBtn.hidden = FALSE;
//		splashsaveBtn.hidden = TRUE;
//	
}
*/
	
}
-(IBAction) SwitchOnOffClick
{
	if (SwhBusiness.on) {
		appDelegate.flgswich = TRUE;
		NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
		[prefs setBool:appDelegate.flgswich forKey:@"flgswich"];	

	}else {
		appDelegate.flgswich = FALSE;
		NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
		[prefs setBool:appDelegate.flgswich forKey:@"flgswich"];	
		
	}

	
}
-(void)viewWillAppear:(BOOL)animated
{
	if(logoImgvw.image==nil)
	{
		logosaveBtn.hidden=FALSE;
		logoremoveBtn.hidden=TRUE;
	}
	else
	{
		logosaveBtn.hidden=TRUE;
		logoremoveBtn.hidden=FALSE;
	}
	if(imgGiveaway.image==nil)
	{
		btngiveawaysave.hidden=FALSE;
		btngiveawayremove.hidden=TRUE;
	}
	else
	{
		btngiveawaysave.hidden=TRUE;
		btngiveawayremove.hidden=FALSE;
	}
	
	
	if(splashImgvw.image==nil)
	{
		splashsaveBtn.hidden=FALSE;
		splashremoveBtn.hidden=TRUE;
	}
	else 
	{
		splashsaveBtn.hidden=TRUE;
		splashremoveBtn.hidden=FALSE;
	}
	
//	LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	
/*	if (alogin.imgBlogo!= nil) {
		NSLog(@"Not Right");
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
-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
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

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}


-(void) getbusinessprofile
{  
	
	HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(loadbusinessprofile) onTarget:self withObject:nil animated:YES];

}
-(void) loadbusinessprofile
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];	
	NSURL *url =[NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_website_ver2.php?api=itunesversion1_1_BusinessProfileSplashLogoQuestions&businessid=%@&version=2",appDelegate.ChangeBusiness,appDelegate.userBusinessId]];
	NSLog(@"urltab2-%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\n%@",responseString);		
	
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	
	XMLBusinessProfile *parser = [[XMLBusinessProfile alloc] initXMLBusinessProfile];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
		
	if(success) {
		NSLog(@"No Errors Businn");
		
		if (appDelegate.objBprofile.businesslogo) {
			NSURL *url = [[NSURL alloc] initWithString:appDelegate.objBprofile.businesslogo];
			UIImage *img = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]];	
			
			logoImgvw.image =img;
			logoremoveBtn.hidden = FALSE;
			logosaveBtn.hidden = TRUE;
		}
		else {
			logoImgvw.image = [UIImage imageNamed:@"Nlogo1_v2.png"];
		}

		if (appDelegate.objBprofile.businesssplash) 
		{
			NSURL *url = [[NSURL alloc] initWithString:appDelegate.objBprofile.businesssplash];
			UIImage *img = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]];	
			
			splashImgvw.image =  img;
			splashremoveBtn.hidden = FALSE;
			splashsaveBtn.hidden = TRUE;
		}
		else {
			splashImgvw.image = [UIImage imageNamed:@"splashscreen.png"];
		}
		if (appDelegate.objBprofile.userbusinessgiveaway) 
		{
			NSURL *url = [[NSURL alloc] initWithString:appDelegate.objBprofile.userbusinessgiveaway];
			UIImage *img = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]];	
			
			imgGiveaway.image =  img;
			btngiveawayremove.hidden = FALSE;
			btngiveawaysave.hidden = TRUE;
		}
		else {
			
			if(appDelegate.flggiveaway)
			{
				imgGiveaway.image = nil;	
			}
			else {
				imgGiveaway.image = [UIImage imageNamed:@"giftcard.png"];
			}
		}
		
		que1Txt.text = appDelegate.objBprofile.questionone;
		que2Txt.text = appDelegate.objBprofile.questiontwo;
		que3Txt.text = appDelegate.objBprofile.questionthree;	
	}
	
	else
	{
		NSLog(@"Error Error Error!!!");
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
		
	}
	
	[pool release];
	
}
-(void)closeButtonClicked
{
	preVw.hidden = TRUE;
	[preVw removeFromSuperview];
}

-(IBAction)update
{
	
	if([que1Txt.text isEqualToString:@""] && [que2Txt.text isEqualToString:@""] && [que3Txt.text isEqualToString:@""] && logoImgvw.image==nil && splashImgvw.image==nil)
	{
		UIAlertView *msg=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please select/enter atleast one field" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[msg show];
		[msg release];
	}
	else if([que1Txt.text isEqualToString:@""] && [que2Txt.text isEqualToString:@""] && [que3Txt.text isEqualToString:@""])
	{
		msg3=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are you sure you want to keep rest of fields left as blank" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:@"cancel",nil];
		[msg3 show];
		[msg3 release];
	}
	else
	{
		[self updateComplete];
	}
	
	
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex{
	
	if(alertView==msg3 && buttonIndex==0)
	{
		[self updateComplete];
	}
	if (alertView == msg1  || alertView == msg2 || alertView == msg4)
	{
		if(buttonIndex==0)
		{
			UIAlertView *msg11=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Image has been removed Successfully" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
			[msg11 show];
			if(alertView==msg1)
			{
				logoImgvw.image=nil;

				logosaveBtn.hidden=FALSE;
				logoremoveBtn.hidden=TRUE;
			}
			if(alertView==msg2)
			{
//				NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
//				appDelegate.dataSplash = nil;
//				[prefs setObject:appDelegate.dataSplash forKey:@"splashimage"];

				
				splashImgvw.image=nil;
				splashsaveBtn.hidden=FALSE;
				splashremoveBtn.hidden=TRUE;
			}
			if (alertView==msg4) {
				imgGiveaway.image=nil;
				btngiveawaysave.hidden=FALSE;
				btngiveawayremove.hidden=TRUE;
				
				
			}
			
		}
//		[self updateComplete];
	}
	
	
	
}		

-(void)updateComplete
{
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(StarUpdating) onTarget:self withObject:nil animated:YES];
	

}
-(void)StarUpdating
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
	
	UIImage *tempimage1,*tempimage2,*tempimage3;
	
	if(logoImgvw.image)
	{
		NSDate *temodate=[NSDate date];
		NSDateFormatter *dtfromat=[[NSDateFormatter alloc] init];
		[dtfromat setDateFormat:@"ddMMyyyyHHMMSS"];
		NSString *datestr=[dtfromat stringFromDate:temodate];

		tempimage1 = logoImgvw.image;
		imgname1=[NSString stringWithFormat:@"businesslogo%@",datestr];
		chklogoflag=1;
	}
	if(splashImgvw.image)
	{
		NSDate *temodate=[NSDate date];
		NSDateFormatter *dtfromat=[[NSDateFormatter alloc] init];
		[dtfromat setDateFormat:@"ddMMyyyyHHMMSS"];
		NSString *datestr=[dtfromat stringFromDate:temodate];
		
		tempimage2 = splashImgvw.image;
		appDelegate.dataSplash = UIImagePNGRepresentation(tempimage2);
		
		NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
		[prefs setObject:appDelegate.dataSplash forKey:@"splashimage"];


		imgname2=[NSString stringWithFormat:@"businesssplesh%@",datestr];
		chklogoflag1=2;
	}
	if (imgGiveaway.image) {
		NSDate *temodate=[NSDate date];
		NSDateFormatter *dtfromat=[[NSDateFormatter alloc] init];
		[dtfromat setDateFormat:@"ddMMyyyyHHMMSS"];
		NSString *datestr=[dtfromat stringFromDate:temodate];
		tempimage3 = imgGiveaway.image;
		imgname3=[NSString stringWithFormat:@"businessgiveaway%@",datestr];
	}
	if([que1Txt.text isEqualToString:@""] || que1Txt.text==nil)
		temoqu1=@"";
	else
		temoqu1=que1Txt.text;

	if([que2Txt.text isEqualToString:@""] || que2Txt.text==nil)
		temoqu2=@"";
	else
		temoqu2=que2Txt.text;
	
	if([que3Txt.text isEqualToString:@""] || que3Txt.text==nil)
		temoqu3=@"";
	else 
		temoqu3=que3Txt.text;
	
	for (int i=0;i<3; i++) {
		if (i==0) {
			if (!logoImgvw.image) {
				
				flag1=[self apiBusinessprofile:appDelegate.userBusinessId question1:temoqu1 question2:temoqu2 question3:temoqu3 imgtype:@"L"];
				
			}
			else{
					   flag1=[self apiBusinessprofile:appDelegate.userBusinessId question1:temoqu1 question2:temoqu2 question3:temoqu3 img:tempimage1 imgnmae:imgname1 imgtype:@"L"];
			}
		}
		if (i==1) {
			if (!splashImgvw.image) {
				
				flag2=[self apiBusinessprofile:appDelegate.userBusinessId question1:temoqu1 question2:temoqu2 question3:temoqu3 imgtype:@"S"];
			}
			else{
			flag2=[self apiBusinessprofile:appDelegate.userBusinessId question1:temoqu1 question2:temoqu2 question3:temoqu3 img:tempimage2 imgnmae:imgname2 imgtype:@"S"];
			}

		}
		if (i==2) {
			if ((!imgGiveaway.image) || flgimgGive) {
				
				flag3=[self apiBusinessprofile:appDelegate.userBusinessId question1:temoqu1 question2:temoqu2 question3:temoqu3 imgtype:@"G"];
				flgimgGive=FALSE;
			}
			else{
				flag3=[self apiBusinessprofile:appDelegate.userBusinessId question1:temoqu1 question2:temoqu2 question3:temoqu3 img:tempimage3 imgnmae:imgname3 imgtype:@"G"];
			}
			
		}
	}
	
	if(flag1==YES || flag2==YES || flag3==YES)
	{
		UIAlertView *conmsg1=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Business Profile Updated successfully" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[conmsg1 show];
		[conmsg1 release];	}
	else {
		UIAlertView *conmsg1=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! Business Profile faced some problem to Update.  Please try again later." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[conmsg1 show];
		[conmsg1 release];
	}
	
	[pool release];

}
-(BOOL)apiBusinessprofile :(NSString *)businessid question1:(NSString *) questionone question2:(NSString *) questiontwo question3:(NSString *) questionthree img:(UIImage *)image imgnmae:(NSString *)imagename imgtype:(NSString *)imagetype
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_website_ver2.php&version=2",appDelegate.ChangeBusiness]];
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	NSLog(@"URL String:-%@",url);
	[request setURL:url];
	[request setHTTPMethod:@"POST"];
	NSString *api = @"itunesversion1_1_BusinessProfileLogoQuestionsUpdate";	
	NSString *boundary = [NSString stringWithString:@"---------------------------14737809831466499882746641449"];
	NSString *contentType = [NSString stringWithFormat:@"multipart/form-data; boundary=%@",boundary];
	[request setValue:contentType forHTTPHeaderField: @"Content-Type"];	
	NSString *filename = [NSString stringWithFormat:@"%@.png",imagename];
	NSData *imageData = UIImagePNGRepresentation(image);
	NSMutableData *body = [NSMutableData data];
	for (int i=0; i<6; i++)
	{
	switch (i) {
	case 0:
		{    
			[body appendData:[[NSString stringWithFormat:@"--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
			[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"api\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
			[body appendData:[NSData dataWithData:[api dataUsingEncoding:NSUTF8StringEncoding]]];
			break;
		}	
		case 1:
		{    
			[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"businessid\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
			[body appendData:[NSData dataWithData:[businessid dataUsingEncoding:NSUTF8StringEncoding]]];
			break;
		}
		case 2:
		{    
			[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"businessquestion1\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
			[body appendData:[NSData dataWithData:[questionone dataUsingEncoding:NSUTF8StringEncoding]]];
			break;
		}
		case 3:
		{    
			[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"businessquestion2\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
			[body appendData:[NSData dataWithData:[questiontwo dataUsingEncoding:NSUTF8StringEncoding]]];
			break;
		}
		case 4:
		{    
			[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"businessquestion3\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
			[body appendData:[NSData dataWithData:[questionthree dataUsingEncoding:NSUTF8StringEncoding]]];
			 break;
		}
		case 5:
		{
			[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"imagetype\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
			[body appendData:[NSData dataWithData:[imagetype dataUsingEncoding:NSUTF8StringEncoding]]];
			break;
			
			break;
		}
								  
		default:
		 break;
	  }
	  [body appendData:[[NSString stringWithFormat:@"\r\n--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
	}
	
	 [body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"businesslogo\"; filename=\"%@\"\r\n",filename]] dataUsingEncoding:NSUTF8StringEncoding]];
	
	 [body appendData:[[NSString stringWithString:@"Content-Type: application/octet-stream\r\n\r\n"] dataUsingEncoding:NSUTF8StringEncoding]];
	 
	[body appendData:[NSData dataWithData:imageData]];
	 [body appendData:[[NSString stringWithFormat:@"\r\n--%@--\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
	 [request setHTTPBody:body];
	 NSString *postLength = [NSString stringWithFormat:@"%d", [body length]];
	 [request setValue:postLength forHTTPHeaderField:@"Content-Length"];
								  
	 NSURLResponse *response = NULL;
	 NSError *requestError = NULL;
	 NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	 NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	 responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
								  
	 NSLog(@"responseString:\%@",responseString);
	 NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	 NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	 NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
								  
	if (rngXMLNode.length > 0)
    responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
								  
    if (rngReturnStart.length > 0)
	    responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
								  
	if (rngReturnEnd.length > 0)
	    responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
		responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	if ([responseString isEqualToString:@"1"]) 
	{
	return YES;
	} else 
	{
		return NO;
	}    
}
					   
-(BOOL)apiBusinessprofile : (NSString *)businessid question1:(NSString *) questionone question2:(NSString *) questiontwo question3:(NSString *) questionthree imgtype:(NSString *)imagetype;
{
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_website_ver2.php&version=2",appDelegate.ChangeBusiness]];
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	[request setHTTPMethod:@"POST"];	
	NSData *requestBody = [[NSString stringWithFormat:@"api=itunesversion1_1_BusinessProfileLogoQuestionsUpdate&businessid=%@&businessquestion1=%@&businessquestion2=%@&businessquestion3=%@&imagetype=%@",appDelegate.userBusinessId,questionone,questiontwo,questionthree,imagetype] dataUsingEncoding:NSUTF8StringEncoding];
	[request setHTTPBody:requestBody];
	NSLog(@"api=itunesversion1_1_BusinessProfileLogoQuestionsUpdate&businessid=%@&businessquestion1=%@&businessquestion2=%@&businessquestion3=%@&imagetype=%@",appDelegate.userBusinessId,questionone,questiontwo,questionthree,imagetype);
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\%@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
    if (rngReturnStart.length > 0)
	    responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
	    responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	if ([responseString isEqualToString:@"1"]) 
	{
		return YES;
	} else 
	{
		return NO;
	}    
	
}
- (BOOL)textField:(UITextField *)textField shouldChangeCharactersInRange:(NSRange)range replacementString:(NSString *)string   // return NO to not change text
{
	if([string isEqualToString:@"\n"])
	{
		[textField resignFirstResponder];
		return NO;
	}
	return YES;
}

-(IBAction)textFieldDidUpdate:(id)sender
{
	UITextField * textField = (UITextField *) sender;
	if(textField ==que1Txt)
	{
		int maxChars = 50;
		int charsLeft = maxChars - [textField.text length];
		remque1Txt.text = [NSString stringWithFormat:@"%d characters remaining.",charsLeft];	
		if(charsLeft == 0) {
			UIAlertView * alert = [[UIAlertView alloc] initWithTitle:@"No more characters"
														 message:[NSString stringWithFormat:@"You have reached the character limit of %d.",maxChars]
														delegate:nil
											   cancelButtonTitle:@"Ok"
											   otherButtonTitles:nil];
			[alert show];
			[alert release];
			}
	}
	
	if(textField ==que2Txt)
	{
		int maxChars = 50;
		int charsLeft = maxChars - [textField.text length];
		remque2Txt.text = [NSString stringWithFormat:@"%d characters remaining.",charsLeft];	

		if(charsLeft == 0) {
			UIAlertView * alert = [[UIAlertView alloc] initWithTitle:@"No more characters"
															 message:[NSString stringWithFormat:@"You have reached the character limit of %d.",maxChars]
															delegate:nil
												   cancelButtonTitle:@"Ok"
												   otherButtonTitles:nil];
			[alert show];
			[alert release];
		}
	}
	if(textField ==que3Txt)
	{
		int maxChars = 50;
		int charsLeft = maxChars - [textField.text length];
		remque3Txt.text = [NSString stringWithFormat:@"%d characters remaining.",charsLeft];	
		if(charsLeft == 0) {
			UIAlertView * alert = [[UIAlertView alloc] initWithTitle:@"No more characters"
															 message:[NSString stringWithFormat:@"You have reached the character limit of %d.",maxChars]
															delegate:nil
												   cancelButtonTitle:@"Ok"
												   otherButtonTitles:nil];
			[alert show];
			[alert release];
		}
	}
}

-(IBAction)logoSave
{
	
	logoflag=1;


	
	if ([popoverController isPopoverVisible]) {
        [popoverController dismissPopoverAnimated:YES];
        [popoverController release];
    } else {
        if ([UIImagePickerController isSourceTypeAvailable:
             UIImagePickerControllerSourceTypePhotoLibrary])
        {
            UIImagePickerController *imagePicker =
            [[UIImagePickerController alloc] init];
            imagePicker.delegate = self;
            imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
            imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *) kUTTypeImage,nil];           
			imagePicker.allowsEditing = NO;
			
            popoverController = [[UIPopoverController alloc]
								 initWithContentViewController:imagePicker];
			
			//  popoverController.delegate=self;
			
			
            [popoverController presentPopoverFromRect:CGRectMake(103, 310, 320, 400) inView:self.view permittedArrowDirections:UIPopoverArrowDirectionDown animated:YES];			
            [imagePicker release];
			
        }
    }
}


-(IBAction)logoRemove
{
	if(logoImgvw.image!=nil)
	{
		msg1=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are you sure you want to remove the image?" delegate:self cancelButtonTitle:@"Yes" otherButtonTitles:@"No",nil];
		[msg1 show];
	}	
}

-(IBAction)splashSave
{	
	logoflag=0;
	if ([popoverController isPopoverVisible]) {
        [popoverController dismissPopoverAnimated:YES];
        [popoverController release];
    } else {
        if ([UIImagePickerController isSourceTypeAvailable:
             UIImagePickerControllerSourceTypePhotoLibrary])
        {
            UIImagePickerController *imagePicker =
            [[UIImagePickerController alloc] init];
            imagePicker.delegate = self;
            imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
            imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *) kUTTypeImage,nil];           
			imagePicker.allowsEditing = NO;
			
            popoverController = [[UIPopoverController alloc]
								 initWithContentViewController:imagePicker];
			//popoverController.delegate=self;
            [popoverController presentPopoverFromRect:CGRectMake(449, 310, 320, 400) inView:self.view permittedArrowDirections:UIPopoverArrowDirectionDown animated:YES];
            [imagePicker release];
			
        }
    }
}

-(IBAction)splashRemove
{
	if(splashImgvw.image!=nil)
	{
		msg2=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are you sure you want to remove the image?" delegate:self cancelButtonTitle:@"Yes" otherButtonTitles:@"No",nil];
		[msg2 show];
	}		
}
-(IBAction)GiveawaySave
{	
	UIActionSheet *action = [[[UIActionSheet alloc] initWithTitle:@"Review Frog" delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:@"Blank", @"Default", @"Your Choice", nil] autorelease];
	
	action.actionSheetStyle = UIBarStyleBlackTranslucent;
	[action  showInView:self.view];	
}
-(IBAction)GiveawayRemove
{
	if(imgGiveaway.image!=nil)
	{
		msg4=[[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are you sure you want to remove the image?" delegate:self cancelButtonTitle:@"Yes" otherButtonTitles:@"No",nil];
		[msg4 show];
		
	}
}
- (void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex
{
	if (buttonIndex == 0)
	{
		imgGiveaway.image=nil;
		btngiveawaysave.hidden=FALSE;
		btngiveawayremove.hidden=TRUE;
		appDelegate.flggiveaway = TRUE;
		NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
		[prefs setBool:appDelegate.flggiveaway forKey:@"Givewayimage"];	
		[appDelegate getUserPreferences];

		
	}
	if (buttonIndex == 1) {
		
		
		UIImage *img = [UIImage imageNamed:@"giftcard.png"];
		imgGiveaway.image = img;
		flgimgGive=TRUE;
		appDelegate.flggiveaway = FALSE;
        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
		[prefs setBool:appDelegate.flggiveaway forKey:@"Givewayimage"];	
		[appDelegate getUserPreferences];

//		NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
//		[prefs setBool:appDelegate.flggiveaway forKey:@"Givewayimage"];	
//		[appDelegate getUserPreferences];
		
		
	}
	if (buttonIndex == 2) {
		logoflag=2;
        appDelegate.flggiveaway = FALSE;
        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
		[prefs setBool:appDelegate.flggiveaway forKey:@"Givewayimage"];	
		[appDelegate getUserPreferences];

		if ([popoverController isPopoverVisible]) {
			[popoverController dismissPopoverAnimated:YES];
			[popoverController release];
		} else {
			if ([UIImagePickerController isSourceTypeAvailable:
				 UIImagePickerControllerSourceTypePhotoLibrary])
			{
				UIImagePickerController *imagePicker =
				[[UIImagePickerController alloc] init];
				imagePicker.delegate = self;
				imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
				imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *) kUTTypeImage,nil];           
				imagePicker.allowsEditing = NO;
				
				popoverController = [[UIPopoverController alloc]
									 initWithContentViewController:imagePicker];
				
				[popoverController presentPopoverFromRect:CGRectMake(690, 310, 320, 400) inView:self.view permittedArrowDirections:UIPopoverArrowDirectionDown animated:YES];
				
				[imagePicker release];
				
			}
		}
		
	}

}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info
{
	[picker dismissModalViewControllerAnimated:NO];
	[popoverController dismissPopoverAnimated:YES];
	if(logoflag==1)
	{
		logoImgvw.image = [info objectForKey:@"UIImagePickerControllerOriginalImage"];
		logoremoveBtn.hidden=FALSE;
		logosaveBtn.hidden=TRUE;
	}
	
	if (logoflag==0) 
	{
		splashImgvw.image = [info objectForKey:@"UIImagePickerControllerOriginalImage"];
		splashsaveBtn.hidden=TRUE;
		splashremoveBtn.hidden=FALSE;
	}
	if (logoflag==2) 
	{
		imgGiveaway.image = [info objectForKey:@"UIImagePickerControllerOriginalImage"];
		btngiveawaysave.hidden=TRUE;
		btngiveawayremove.hidden=FALSE;
	}
	
}


-(IBAction)BackClick
{
    OptionOfHistoryViewController *obj= [[OptionOfHistoryViewController alloc]init];
    [self.navigationController pushViewController:obj animated:YES];
    [obj release];
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
-(void)textFieldDidBeginEditing:(UITextField *)textField
{
	if (textField==que1Txt) {
		[que1Txt selectAll:self];
	}
	if (textField==que2Txt) {
		[que2Txt selectAll:self];
	}
	if (textField==que3Txt) {
		[que3Txt selectAll:self];
	}
}
-(void)textFieldDidEndEditing:(UITextField *)textField
{

	
}
-(void)keyboardWillShowNotification:(NSNotification *)note{	
	
	scrollVw.frame=CGRectMake(0, 0, 1024, 450);	
}
-(void)keyboardWillHideNotification
{
	scrollVw.frame=CGRectMake(0, 0, 1024, 748);
	
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
