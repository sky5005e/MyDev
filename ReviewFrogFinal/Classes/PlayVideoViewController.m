    //
//  PlayVideoViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 6/23/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "PlayVideoViewController.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "Terms_Condition.h"
#import "VideoListViewController.h"
#import <MobileCoreServices/UTCoreTypes.h>
#import "CameraContinuViewController.h"
#import <MediaPlayer/MediaPlayer.h>
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"

@implementation PlayVideoViewController
@synthesize avideo;
@synthesize flgStopReloading;
// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	[scrView addSubview:viewVideo];
	[scrView setContentSize:CGSizeMake(viewVideo.frame.size.width, viewVideo.frame.size.height)];
}
-(void)viewWillAppear:(BOOL)animated
{
	lblTitle.text = [avideo.ReviewVideoTitle stringByReplacingOccurrencesOfString:@"_" withString:@" "];
	
	if (![avideo.tablequestionone isEqualToString:@"0"]) {
		
		NSArray *Strinlist = [avideo.tablequestionone componentsSeparatedByString:@":"];
		lblQuesionOne.text = [Strinlist objectAtIndex:0];
		lblAnswerOne.text  = [Strinlist objectAtIndex:1];
	}
	
	if (![avideo.tablequestiontwo isEqualToString:@"0"]) {
		
		NSArray *Strinlist = [avideo.tablequestiontwo componentsSeparatedByString:@":"];
		lblQuesionTwo.text = [Strinlist objectAtIndex:0];
		lblAnswerTwo.text  = [Strinlist objectAtIndex:1];
		
	}
	if (![avideo.tablequestionthree isEqualToString:@"0"]) {
		
		NSArray *Strinlist   = [avideo.tablequestionthree componentsSeparatedByString:@":"];
		lblQuesionThree.text = [Strinlist objectAtIndex:0];
		lblAnswerThree.text  = [Strinlist objectAtIndex:1];
		
	}
	if([avideo.tablequestionone isEqualToString:@"(null):"])  {
		lblQuesionOne.text = @"-";
		lblAnswerOne.text  = @"-";
	}
	if ([avideo.tablequestiontwo isEqualToString:@"(null):"]) {
		lblQuesionTwo.text = @"-";
		lblAnswerTwo.text = @"-";
		
	}
	if ([avideo.tablequestionthree isEqualToString:@"(null):"]) {
		
		lblQuesionThree.text = @"-";
		lblAnswerThree.text  = @"-";
		
	}
    imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:appDelegate.pathBussinesslogo] autorelease];  

    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
//        imgPowerdby.hidden=YES;
//        imgtxtPowerdby.hidden=YES;
    }
    
 	if ([avideo.VideoStatus isEqualToString:@"A"]) {
		
		NSURL *url = [NSURL URLWithString:avideo.ReviewVideoUrl];
		urlReq = [[NSURLRequest alloc] initWithURL:url]; 
		[webVideoplay loadRequest:urlReq];
				
		btnRecord.hidden = YES;
		btnSubmit.hidden = YES;
		
		imgRecord.hidden = YES;
		imgSubmit.hidden = YES;	
	}
	else {
		urlReq = [[NSURLRequest alloc] initWithURL:appDelegate.VideoPath];
		NSLog(@"URL:>%@",urlReq);
		[webVideoplay loadRequest:urlReq];
		btnRecord.hidden = NO;
		btnSubmit.hidden = NO;
		imgRecord.hidden = NO;
		imgSubmit.hidden = NO;
	}
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
{   appDelegate.VBack=TRUE;
	VideoListViewController *objvideoplay= [[VideoListViewController alloc]init];

    [self.navigationController pushViewController:objvideoplay animated:YES];
    [objvideoplay release];
}
-(IBAction) ReRecord_click
{
	appDelegate.flgReRecord=TRUE;

	if ([UIImagePickerController isSourceTypeAvailable:
		 UIImagePickerControllerSourceTypeCamera])
	{
		UIImagePickerController *imagePicker =
		[[UIImagePickerController alloc] init];
		imagePicker.delegate = self;
		imagePicker.sourceType = 
		UIImagePickerControllerSourceTypeCamera;
		imagePicker.cameraDevice = UIImagePickerControllerCameraDeviceFront;
		imagePicker.mediaTypes = [NSArray arrayWithObjects:
								  (NSString *) kUTTypeMovie,
								  nil];
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
	
	//[self dismissModalViewControllerAnimated:YES];
	
	if([mediaType isEqualToString:(NSString *)kUTTypeMovie]) 
	{
		
		//UIImage *image = [info objectForKey:UIImagePickerControllerOriginalImage];
		
		NSURL *urlVideo = [info 
						   objectForKey:UIImagePickerControllerMediaURL];
		
		appDelegate.DataVideo = [NSData dataWithContentsOfURL:urlVideo];
			
		[appDelegate saveVideoToDir];
		
		[picker dismissModalViewControllerAnimated:YES];
		
	}
}
-(IBAction) SubmitVideo_Click
{		
		// The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
		HUD = [[MBProgressHUD alloc] initWithView:self.view];
		
		// Add HUD to screen
		[self.view addSubview:HUD];
		
		// Regisete for HUD callbacks so we can remove it from the window at the right time
		HUD.delegate = self;
		
		HUD.labelText = @"Loading";
		
		// Show the HUD while the provided method executes in a new thread
		[HUD showWhileExecuting:@selector(loadingStart) onTarget:self withObject:nil animated:YES];
		
}
-(void) loadingStart
{
	NSLog(@"AutoResponder:-%@",avideo.ReviewPersonEmail);
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	if (appDelegate.flgReRecSub==TRUE) 
    {
		appDelegate.flgReRecSub=FALSE;
		isUploaded =[appDelegate apiPostReviewVideo:avideo.id VTitle:avideo.ReviewVideoName video:appDelegate.DataVideo];	
	
	}else 
    {
		
		isUploaded =[appDelegate apiPostReviewVideo:avideo.id VTitle:avideo.ReviewVideoName video:appDelegate.NewData];

	}
    NSLog(@"Avideo.id: %d", avideo.id);
    NSLog(@"and Review videoname %@",avideo.ReviewVideoName);
    NSLog(@"and Video Data %@", appDelegate.NewData);
    
	if (isUploaded)        
	{
		appDelegate.flgStopReloading=TRUE;
		NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
		NSString *documentsDirectory = [paths objectAtIndex:0]; 
		
		NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
		
		NSString *videoName = avideo.ReviewVideoName;
		
		NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,videoName];
		NSLog(@"exportPath %@",exportPath);
		
		NSFileManager *fileManager = [NSFileManager defaultManager];
		if([fileManager fileExistsAtPath:exportPath])
        {
			NSError *err;
			[fileManager removeItemAtPath:exportPath error:&err];
		}
		else
        {
			NSLog(@"Video Not Exist");
		}
		
		//Auto Responder mail
	
		btnRecord.hidden = YES;
		btnSubmit.hidden = YES;
		
		imgRecord.hidden = YES;
		imgSubmit.hidden = YES;
		
		
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Your Video Review Submitted successfully." delegate:self 
											  cancelButtonTitle:@"OK" 
											  otherButtonTitles:nil];
		[alert show];
		[alert release];
		
	}
	
	else 
    {
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!!  Video Review uploading failed." delegate:self 
											  cancelButtonTitle:@"OK" 
											  otherButtonTitles:nil];
		[alert show];
		[alert release];                     
	}
	[pool release];

}
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation 
{
	// Overriden to allow any orientation.
	if(interfaceOrientation == UIInterfaceOrientationLandscapeRight || interfaceOrientation == UIInterfaceOrientationLandscapeLeft)
		return YES;
	else if(interfaceOrientation == UIInterfaceOrientationPortrait)
		return NO;
	else 
		return NO;
}
- (void)viewWillDisappear:(BOOL)animated
{
	[webVideoplay loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:@"about:blank"]]];
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
