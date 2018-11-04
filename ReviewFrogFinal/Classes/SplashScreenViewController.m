    //
//  SplashScreenViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 7/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "SplashScreenViewController.h"
#import "Review_FrogViewController.h"
#import "ApplicationValidation.h"
#import "WebAndListingViewController.h"
#import "WriteORRecodeViewController.h"
#import "LoginInfo.h"
#import "ClientBusinessSplashScreemViewController.h"


@implementation SplashScreenViewController
@synthesize Bsplashimage;
 

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	self.navigationController.navigationBarHidden=TRUE;
	TimeProcess = [NSTimer scheduledTimerWithTimeInterval:0.2 target:self selector:@selector(GoToNextScreen) userInfo:nil repeats:NO];
	
	if (!appDelegate.dataSplash) {
		imgSplash.image = [UIImage imageNamed:@"splashscreen.png"];
		NSLog(@"No splash");
	}
	else {
        
       	UIImage *img = [UIImage imageWithData:appDelegate.dataSplash];
        // SEt Path-------------
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
        NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents folder
        NSString *profolder=@"SplashImageDB.png";
        NSString *path = [documentsDirectory stringByAppendingPathComponent:profolder];
        
        // store image in document directory-----
        
        NSData *imageData = UIImagePNGRepresentation(img);
        UIImage *imgPNG = [[UIImage alloc] initWithData:imageData];
        if (imageData != nil) {
            [UIImagePNGRepresentation(imgPNG) writeToFile:path atomically:YES];
            NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
            [prefs setObject:path forKey:@"path"];
            [prefs synchronize];
        }
        
        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
        NSString *mail = [prefs stringForKey:@"path"];
        NSLog(@"image path %@",mail);
        
		imgSplash.image= [[[UIImage alloc] initWithContentsOfFile:mail] autorelease];	
	}
}

-(void)GoToNextScreen
{
	[TimeProcess invalidate];
	TimeProcess = nil;
	NSString * str;
	
	if ([appDelegate.UserEmail isEqualToString:@""]||appDelegate.UserEmail == nil) 
	{
		WebAndListingViewController *objWebandListing = [[WebAndListingViewController alloc] initWithNibName:@"WebAndListingViewController" bundle:nil];
	
        [self.navigationController pushViewController:objWebandListing animated:YES];
        [objWebandListing release];
	}
	else 
	{
        
        if (appDelegate.remoteHostStatus!=0) {
            
//            str = [NSString stringWithFormat:@"%@frmReviewFrogApi.php?api=itunesversion1_1_LoginCheck_LogoSplash&user_email=%@",appDelegate.set_APIurl,appDelegate.UserEmail];
            
            str = [NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_LoginCheck_LogoSplash&user_email=%@&version=2",appDelegate.ChangeUrl,appDelegate.UserEmail];
            
            NSLog(@"urlstr in splash:-%@",str);
			
            NSURL *url = [NSURL URLWithString:str];
            NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];	
            NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
            
            //Initialize the delegate.
            xmlparser *parser = [[xmlparser alloc] initxmlparser];
            
            //Set delegate
            [xmlParser setDelegate:parser];
            
            //Start parsing the XML file.
            BOOL success = [xmlParser parse];
            if(success)
            {
                NSLog(@"No Errors");
                LoginInfo *alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
                int userid = alogin.id;
                appDelegate.UserEmail= alogin.userEmail;
                appDelegate.userId = [NSString stringWithFormat:@"%d",userid];
                
                if (![alogin.BusinessSplash isEqualToString:@"0"]) {
                    NSURL *url = [[NSURL alloc] initWithString:alogin.BusinessSplash];
                    appDelegate.dataSplash = [NSData dataWithContentsOfURL:url];
                    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
                    [prefs setObject:appDelegate.dataSplash forKey:@"splashimage"];		
                }
                else {
                    appDelegate.dataSplash = nil;
                    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
                    [prefs setObject:appDelegate.dataSplash forKey:@"splashimage"];		
                }
                alogin.imgBsplash = [UIImage imageWithData:appDelegate.dataSplash];
                if (!alogin.imgBsplash) {
                   imgSplash.image =[UIImage imageNamed:@"splashscreen.png"];
                }
                appDelegate.userId=[NSString stringWithFormat:@"%d",userid];		
            }
            else
            {
                WriteORRecodeViewController *awriteRecord= [[WriteORRecodeViewController alloc]init];
                awriteRecord.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;
                
        [self.navigationController pushViewController:awriteRecord animated:YES];
        [awriteRecord release];
                
            }
            
            [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
            if(appDelegate.FirstUser==TRUE)
            {		
                WriteORRecodeViewController *awriteRecord= [[WriteORRecodeViewController alloc]init];
                awriteRecord.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;
                
        [self.navigationController pushViewController:awriteRecord animated:YES];
                [awriteRecord release];
            }
        }
        else{
            
            WriteORRecodeViewController *awriteRecord= [[WriteORRecodeViewController alloc]init];
            awriteRecord.modalTransitionStyle = UIModalTransitionStyleFlipHorizontal;
           [self.navigationController pushViewController:awriteRecord animated:YES];
            [awriteRecord release];
        }
    }	
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
    NSLog(@"shouldAutorotateToInterfaceOrientation");
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


- (void)viewDidUnload {
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}


- (void)dealloc {
    [super dealloc];
}


@end
