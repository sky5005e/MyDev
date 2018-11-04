    //
//  CameraContinuViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 6/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "CameraContinuViewController.h"
#import <MobileCoreServices/UTCoreTypes.h>
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "Terms_Condition.h"
#import "Term_para.h"
#import "xmlparser.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"
#import "LoginInfo.h"

@implementation CameraContinuViewController
@synthesize btnList;
@synthesize dataPath;
@synthesize DataVideo;
@synthesize tempExportPath;
@synthesize videoName;
@synthesize createdVideoURL;
@synthesize strVideo;
@synthesize termpage;



// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	    
}

- (void)viewWillAppear:(BOOL)animated{
		
	if (termpage==TRUE) {
		
		Term_para *Termsp = [[Term_para alloc]init];
        [self.navigationController pushViewController:Termsp animated:YES];
        [Termsp release];
	}
	LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
	if (alogin.imgBlogo!= nil) {
		//NSLog(@"Not Right");
		imgBLogo.image = alogin.imgBlogo;
		
	} else {
		
		if (![alogin.userbusinesslogo isEqualToString:@"0"]) {
			
			NSURL *url = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
			UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
			alogin.imgBlogo = image;
			imgBLogo.image = image;
			imgPowerdby.hidden=NO;
			imgtxtPowerdby.hidden=NO;			
		}else {
			UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
			imgBLogo.image = img;
			imgPowerdby.hidden=YES;
			imgtxtPowerdby.hidden=YES;
			
		}
		
	}
	
}
-(IBAction) Continue_Click
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
	else {
		UIAlertView *Alret = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please make sure your camera settings are on your ipad." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[Alret show];
		[Alret release];
	}	
}
-(void)imagePickerController:(UIImagePickerController *)picker
didFinishPickingMediaWithInfo:(NSDictionary *)info
{
	
	NSString *mediaType = [info objectForKey:UIImagePickerControllerMediaType];
	termpage=TRUE;
	
		if([mediaType isEqualToString:(NSString *)kUTTypeMovie]) 
		{
			
			NSURL *urlVideo = [info objectForKey:UIImagePickerControllerMediaURL];
			appDelegate.DataVideo = [NSData dataWithContentsOfURL:urlVideo];
							
			[appDelegate saveVideoToDir];
			[picker dismissModalViewControllerAnimated:YES];
			
											
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



-(IBAction) Switch_Click
{
	
}
-(IBAction) Backtohome
{	
	appDelegate.txtcleanflag=TRUE;
[self dismissModalViewControllerAnimated:YES];	
}

-(IBAction)Home{
	appDelegate.LoginConform=FALSE;
	appDelegate.txtcleanflag=FALSE;
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
-(IBAction) MoveToPlayList
{
	VideoListViewController *objplayvideo= [[VideoListViewController alloc]init];
    [self.navigationController pushViewController:objplayvideo animated:YES];
    [objplayvideo release];
}

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}

- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc. that aren't in use.
}

-(void)viewDidAppear:(BOOL)animated
{
	if (termpage==TRUE) {
		termpage=FALSE;
		Term_para *Termsp = [[Term_para alloc]init];
        [self.navigationController pushViewController:Termsp animated:YES];
        [Termsp release];
	}
	
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
