    //
//  PlayBusinessVideoViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "PlayBusinessVideoViewController.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "Terms_Condition.h"
#import "VideoListViewController.h"
#import <MobileCoreServices/UTCoreTypes.h>
#import "CameraContinuViewController.h"
#import "BusinessVideoListViewController.h"
#import <MediaPlayer/MediaPlayer.h>
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"
@implementation PlayBusinessVideoViewController
@synthesize avideo;
@synthesize Activeindicator;
 

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];

}

-(void)viewWillAppear:(BOOL)animated
{
	
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    
    
    if (pathBussinesslogo !=nil) {   
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }
    
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
        
    }
    

	lblTitle.text = [avideo.ReviewVideoTitle stringByReplacingOccurrencesOfString:@"_" withString:@" "];
	
	if ([avideo.VideoStatus isEqualToString:@"A"]) {
		
	NSURL *url = [NSURL URLWithString:avideo.ReviewVideoUrl];
	NSURLRequest *urlReq = [[NSURLRequest alloc] initWithURL:url]; 
	[webVideoplay loadRequest:urlReq];
			
	[self.Activeindicator startAnimating];	
		btnRecord.hidden = YES;
		btnSubmit.hidden = YES;
		
		imgRecord.hidden = YES;
		lblsubmit.hidden = YES;
		
	}
	else {
		
		NSURLRequest *urlReq = [[NSURLRequest alloc] initWithURL:appDelegate.BusinessVideoPath];
		NSLog(@"URL:>%@",urlReq);
		[webVideoplay loadRequest:urlReq];
		
		btnRecord.hidden = NO;
		btnSubmit.hidden = NO;
		imgRecord.hidden = NO;
		lblsubmit.hidden = NO;
	}

}

-(IBAction)Home
{
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

-(IBAction) Back_Click
{
    BusinessVideoListViewController *objvideoplay= [[BusinessVideoListViewController alloc]init];
	
    [self.navigationController pushViewController:objvideoplay animated:YES];
    [objvideoplay release];

}
-(IBAction) ReRecord_click
{
	appDelegate.flgBusinessReRecord=TRUE;

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
		
		appDelegate.BusinessDataVideo = [NSData dataWithContentsOfURL:urlVideo];
		
		//appDelegate.DataVideo = UIImagePNGRepresentation(image);
		
		[appDelegate saveVideoToBusinessDir];
		
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
	NSLog(@"AutoResponder:-",avideo.ReviewPersonEmail);
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	//[appDelegate apiPostReviewVideo:avideo.id VTitle:avideo.ReviewVideoName video:appDelegate.NewData];
	BOOL isPostBussiness = [appDelegate apiPostBusinessVideo:avideo.id VTitle:avideo.ReviewVideoName video:appDelegate.NewBusinessData];
	//[appDelegate apiPostReviewVideo:5 VTitle:@"angrybird_2011-07-01-1747.png" video:appDelegate.DataVideo];
	
	if (isPostBussiness) {
		NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
		NSString *documentsDirectory = [paths objectAtIndex:0]; 
		
		NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
		
		NSString *videoName = avideo.ReviewVideoName;
		
		NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,videoName];
		NSLog(@"exportPath %@",exportPath);
		
		NSFileManager *fileManager = [NSFileManager defaultManager];
		if([fileManager fileExistsAtPath:exportPath]){
			NSError *err;
			[fileManager removeItemAtPath:exportPath error:&err];
			NSLog(@"Video Remove frog local");
		}
		else{
			NSLog(@"Video Not Exist");
		}
		
		btnRecord.hidden = YES;
		btnSubmit.hidden = YES;
		
		imgRecord.hidden = YES;
		lblsubmit.hidden = YES;
		
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Your Business Video Upload successfully" delegate:self 
											  cancelButtonTitle:@"OK" 
											  otherButtonTitles:nil];
		[alert show];
		[alert release];
		
	}
	else {
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Whoops!! Business Video uploading failed." delegate:self 
											  cancelButtonTitle:@"OK" 
											  otherButtonTitles:nil];
		[alert show];
		[alert release];
		
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
