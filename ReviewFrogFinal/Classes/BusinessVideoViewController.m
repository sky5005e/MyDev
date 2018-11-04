    //
//  BusinessVideoViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BusinessVideoViewController.h"
#import "ApplicationValidation.h"
#import "HistoryLogin.h"
#import "ThreeTabButton.h"
#import "Terms_Condition.h"
#import <MobileCoreServices/MobileCoreServices.h>
#import "Thank_you.h"
#import <MediaPlayer/MediaPlayer.h>
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"

@implementation BusinessVideoViewController

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	
	
	//NSURLRequest *urlReq = [[NSURLRequest alloc] initWithURL:appDelegate.BusinessUrl];
//	NSLog(@"URL:>%@",urlReq);
//	[webVideoplay loadRequest:urlReq];
	
}
-(void)viewWillAppear:(BOOL)animated
{
	/*LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	btnSubmit.hidden = NO;
	imgSubmit.hidden = NO;

	if (alogin.imgBlogo!= nil) {
		//NSLog(@"Not Right");
		imgBLogo.image = alogin.imgBlogo;
		
	} else {
		
		if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
			
			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			imgBLogo.image = image;
				
			
		}else {
			UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
			imgBLogo.image = img;
			
		}
	}*/
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    
    
    if (pathBussinesslogo !=nil) {   
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }

    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;

    }
   
    
    //imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];

	
	
	NSURLRequest *urlReq = [[NSURLRequest alloc] initWithURL:appDelegate.BusinessUrl];
	NSLog(@"URL:>%@",urlReq);
	[webVideoplay loadRequest:urlReq];	
		
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

-(IBAction) Back_Click
{
    BusinessVideoListViewController *objBusinessVideo = [[BusinessVideoListViewController alloc]initWithNibName:@"BusinessVideoListViewController" bundle:nil];
    [self.navigationController pushViewController:objBusinessVideo animated:YES];
    [objBusinessVideo release];


}

-(IBAction) ReRecord_Clcik
{
	if ([UIImagePickerController isSourceTypeAvailable:
		 UIImagePickerControllerSourceTypeCamera])
	{
		UIImagePickerController *imagePicker =
		[[UIImagePickerController alloc] init];
		imagePicker.delegate = self;
		imagePicker.sourceType = 
		UIImagePickerControllerSourceTypeCamera;
		imagePicker.cameraDevice = UIImagePickerControllerCameraDeviceFront;
		imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *) kUTTypeMovie,nil];
		imagePicker.allowsEditing = NO;
		
		imagePicker.cameraCaptureMode = UIImagePickerControllerCameraCaptureModeVideo;
		
		imagePicker.videoMaximumDuration = 30;
		
		[self presentModalViewController:imagePicker animated:YES];
		[imagePicker release];
	}

}

-(void)imagePickerController:(UIImagePickerController *)picker
didFinishPickingMediaWithInfo:(NSDictionary *)info
{
	
	NSString *mediaType = [info objectForKey:UIImagePickerControllerMediaType];
		
	if([mediaType isEqualToString:(NSString *)kUTTypeMovie]) 
	{				
		appDelegate.BusinessUrl = [info objectForKey:UIImagePickerControllerMediaURL];
		NSLog(@"Businessurl:%@",appDelegate.BusinessUrl);
		
		appDelegate.BusinessDataVideo = [NSData dataWithContentsOfURL:appDelegate.BusinessUrl];
		
				
		[picker dismissModalViewControllerAnimated:YES];
		
	}
	
}
-(IBAction) SubmitVideo_Click;
{			
	if ([txtTitle.text isEqualToString:@""]||txtTitle.text == nil) {
		UIAlertView *alrmsg = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Enter your business title" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alrmsg show];
		[alrmsg release];
	}
	else {
		appDelegate.BusinessVideoTitle=[txtTitle.text stringByReplacingOccurrencesOfString:@" " withString:@"_"];
		//[txtTitle.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
		//	NSLog(@"Remove space:->%@",txtTitle.text);
		[appDelegate saveVideoToBusinessDir];
		[self SaveVideoinfo];
		
	}

		
}
-(void) SaveVideoinfo
{
	
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php&version=2",appDelegate.ChangeUrl]];
	NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
	[request setHTTPMethod:@"POST"];
	NSData *requestBody = [[NSString stringWithFormat:@"api=businessvideo&uid=%@&title=%@&videofile=%@",appDelegate.userId,appDelegate.BusinessVideoTitle,appDelegate.BusinessVideoName] dataUsingEncoding:NSUTF8StringEncoding];
	[request setHTTPBody:requestBody];
	NSLog(@"api=businessvideo&uid=%@&title=%@&videofile=%@",appDelegate.userId,appDelegate.BusinessVideoTitle,appDelegate.BusinessVideoName);
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	NSLog(@"responseString=%@",responseString);
	
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
		NSLog(@"Done");	
//		appDelegate.flgBusinessRecording=YES;
		btnSubmit.hidden = YES;
		imgSubmit.hidden = YES;
		UIAlertView *alret = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Your Business Video Submitted successfully"delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		NSLog(@"True flag");
		[alret show];
		[alret release];		
	} 
	else 
	{
		NSLog(@"Not Done");				
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
    
    // Release any cached data, images, etc. that aren't in use.
}
- (void)viewWillDisappear:(BOOL)animated
{
		
	[webVideoplay loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:@"about:blank"]]];
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
