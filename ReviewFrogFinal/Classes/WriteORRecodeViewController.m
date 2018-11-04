    //
//  WriteORRecodeViewController.m
//  Review Frog
//
//  Created by agilepc-32 on 10/4/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "WriteORRecodeViewController.h"
#import "CameraContinuViewController.h"
#import "ApplicationValidation.h"
#import "WebAndListingViewController.h"
#import "OptionOfHistoryViewController.h"
#import "HistoryLogin.h"
#import "Terms_Condition.h"
#import "LoginInfo.h"

@implementation WriteORRecodeViewController
@synthesize imgBLogo;
@synthesize dateStore;
// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
    
	appDelegate = (Review_FrogAppDelegate*)[[UIApplication sharedApplication]delegate];
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    appDelegate.userBusinessId = [prefs stringForKey:@"BusinessId"];
    
    NSLog(@"appDelegate.remoteHostStatus %d",appDelegate.remoteHostStatus);
    
    if (appDelegate.remoteHostStatus!=0) {
        
        [self performSelectorOnMainThread:@selector(showResponse) withObject:Nil waitUntilDone:YES];
    }
    else{
        [self Continue];
    }
	
}

-(void)Continue
{
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
    if (pathGiveAway !=nil) {   
        
        imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
        btnsweepstake.hidden=NO;
        
    }
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    //if (appDelegate.objBprofile.businesslogo !=nil)
    if (pathBussinesslogo !=nil) {   
        // NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }
    if (!imgGiveAway.image) {
        if (appDelegate.flggiveaway) {
            NSLog(@"Nothing 3");
            imgGiveAway.image=nil;                
        }else{
            NSLog(@"Nothing 4");
            UIImage *img = [UIImage imageNamed:@"giftcard.png"];
            imgGiveAway.image = img;
            
            btnsweepstake.hidden=NO;
        }
    }
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
        imgPowerdby.hidden=YES;
        imgtxtPowerdby.hidden=YES;
    }
    
}

- (void) showResponse 
{
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
    [self.view addSubview:HUD];
    HUD.delegate = self;
    HUD.labelText = @"Loading";
    
    if (appDelegate.remoteHostStatus!=0) {
        // INTERNET AVAILABLE.
        [HUD showWhileExecuting:@selector(checkResponse) onTarget:self withObject:nil animated:YES];
    }
    else{
        
        // not available : -------
        [self Continue];
    }
}
-(void)checkResponse
{
    
    [self load];
    
//    NSLog(@"checkResponse Write :");
//    //MBProgressHUD
//    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
//    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
//    self.dateStore=[prefs valueForKey:@"dateStore"];
//    NSLog(@"dateStore %@",self.dateStore);
//    appDelegate.dateStoreinPre=dateStore;
//    
//    NSURL *url = [NSURL URLWithString:[[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_syncstatus&user_id=%@&lc_datetitme=%@&version=2",appDelegate.ChangeUrl,appDelegate.userId,[self.dateStore stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]]]stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
//    
//    // 2012-02-07 03:24:00bjm
//    NSLog(@"checkResponse Write:-%@",url);
//	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
//	[request setURL:url];
//	[request setHTTPMethod:@"GET"];
//	
//	NSURLResponse *response = NULL;
//	NSError *requestError = NULL;
//	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
//	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
//    
//    NSLog(@"response == %@",responseString);
//    
////    UIAlertView *alert=[[UIAlertView alloc] initWithTitle:@"ReviewFrog" message:[NSString stringWithFormat:@"Response = %@",responseString] delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
////    
////    [alert show];
//    
//	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
//		
//    NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
//	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
//	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
//	
//	if (rngXMLNode.length > 0)
//		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
//	
//	if (rngReturnStart.length > 0)
//		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
//	
//	if (rngReturnEnd.length > 0)
//		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
//    
//    responseString=[responseString stringByReplacingOccurrencesOfString:@"<sweepstake>0</sweepstake>" withString:@""];
//    
//    responseString=[responseString stringByReplacingOccurrencesOfString:@"<sweepstake>1</sweepstake>" withString:@""];
//    
//	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
//    
////    int r=[responseString rangeOfString:@"<"].location;
////    
////    NSLog(@"%u",[responseString rangeOfString:@"<"].location);
////    
////    responseString=[responseString substringToIndex:r];
//    
//    NSLog(@"responseString: write :: %@",responseString);
//    
//	if ([responseString isEqualToString:@"0"]) {
//        // No updation from admin side
//            NSLog(@"// NO Overrite.write ");
//            
//            NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
//        
//            NSString *pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
//        
//        
//            NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
//        
//            if (pathBussinesslogo !=nil) {   
//               
//                imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
//            }
//        
//        if ([[prefs objectForKey:@"sweepstake"] isEqualToString:@"1"]) {
//            
//            lblwithwin.hidden=NO;
//            
//            if (pathGiveAway !=nil) {   
//                
//                NSLog(@"fkdhfjkhsd");
//                imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
//                btnsweepstake.hidden=NO;
//            }
//            
//            if (!imgGiveAway.image) {
//                
//                if (appDelegate.flggiveaway) {
//                    NSLog(@"Nothing 5");
//                    imgGiveAway.image=nil;                
//                }else{
//                    NSLog(@"Nothing 6");
//                    UIImage *img = [UIImage imageNamed:@"giftcard.png"];
//                    imgGiveAway.image = img;
//                    btnsweepstake.hidden=NO;
//                }
//            }
//        }
//        
//        if ([[prefs objectForKey:@"sweepstake"] isEqualToString:@"0"]) {
//            
//            imgGiveAway.image=NULL;
//        }
//        
//            if (!imgBLogo.image) {
//                UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
//                imgBLogo.image = img;
//                imgPowerdby.hidden=YES;
//                imgtxtPowerdby.hidden=YES;
//            }
//    }
//    
//    else{
//        
//        /// change something from admin ---------
//        
//        NSLog(@"-- Get Response Date --");
//        
//        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
//        
//        [prefs setObject:responseString forKey:@"dateStore"];
//        
//        [prefs synchronize];
//        
//        [self load];
//    }
//    
//    [pool release];
}

-(void)load
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
    
    NSLog(@"-- load---");
    NSURL *url =[NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_website.php?api=itunesversion1_1_BusinessProfileSplashLogoQuestions&businessid=%@&version=2", appDelegate.websiteAPIurl, appDelegate.userBusinessId]];
    
    
	//NSURL *url =[NSURL URLWithString:[NSString stringWithFormat:@"http://184.168.101.3/LiveReviewFrogWebCN/admin/main/frmReviewFrogApi_website.php?api=itunesversion1_1_BusinessProfileSplashLogoQuestions&businessid=%@&version=2",appDelegate.userBusinessId]];
    
	NSLog(@"urltab2-----%@",url);
    
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
	
	XMLBusinessProfile *parser = [[XMLBusinessProfile alloc] initXMLBusinessProfile];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
    
	if(success) {
        
        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
        
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
        
        NSString *documentsDirectory = [paths objectAtIndex:0];
        
        NSString *profolder2=@"Bussiness.png";
        
        NSString *profolder3=@"GiveAway.png";
        
        NSString *pathBussinesslogo = [documentsDirectory stringByAppendingPathComponent:profolder2];
        
        NSString *pathGiveAway = [documentsDirectory stringByAppendingPathComponent:profolder3];
        
    if (appDelegate.objBprofile.businesslogo !=nil) {
            
        NSString *str = [appDelegate.objBprofile.businesslogo stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
            
        NSURL *url = [[NSURL alloc] initWithString:str];
            
        UIImage *image = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:url]]; 
        
        NSData *imageData = UIImagePNGRepresentation(image);
            
        UIImage *imgPNG = [[UIImage alloc] initWithData:imageData];
            
        if (imageData != nil ) {
                
                [UIImagePNGRepresentation(imgPNG) writeToFile:pathBussinesslogo atomically:YES];
                
                [prefs setObject:pathBussinesslogo forKey:@"pathBussinesslogo"];
                
                [prefs synchronize];
            
                imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
            } 
        }
        
    else{
            if (!imgBLogo.image) {
                
                UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
                
                [UIImagePNGRepresentation(img) writeToFile:pathBussinesslogo atomically:YES];
                
                [prefs setObject:pathBussinesslogo forKey:@"pathBussinesslogo"];
                
                [prefs synchronize];
                
                imgBLogo.image = img;
                imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
                
                imgPowerdby.hidden=YES;
                
                imgtxtPowerdby.hidden=YES;
            }
            //set by kirti
            
        }
        // -- second image
        
        NSLog(@"appDelegate.objBprofile.userbusinessgiveaway %@",appDelegate.objBprofile.userbusinessgiveaway);
        
    if (appDelegate.objBprofile.userbusinessgiveaway !=nil) {
                    
        NSString *strGiveAway = [appDelegate.objBprofile.userbusinessgiveaway stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
            
        NSURL *urlGiveAway = [[NSURL alloc] initWithString:strGiveAway];
            
        UIImage *imageGiveAway = [[UIImage alloc] initWithData:[[NSData alloc] initWithContentsOfURL:urlGiveAway]]; 
        
        NSData *imageDataGiveAway = UIImagePNGRepresentation(imageGiveAway);
            
        UIImage *imgPNGGiveAway = [[UIImage alloc] initWithData:imageDataGiveAway];
            
        if (imageDataGiveAway !=nil){
                
            NSLog(@"%@",[UIImagePNGRepresentation(imgPNGGiveAway) writeToFile:pathGiveAway atomically:NO]?@"Yes":@"NO");
            
                [prefs setObject:pathGiveAway forKey:@"pathGiveAway"];
                        
                [prefs synchronize];
                
                if ([[prefs objectForKey:@"sweepstake"] isEqualToString:@"1"]) {
                    
                    imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
                    
                    lblwithwin.hidden=NO;
                    
                    btnsweepstake.hidden=NO;
                    
                    if (!imgGiveAway.image) {
                        
                        if (appDelegate.flggiveaway) {
                            NSLog(@"Nothing 5");
                            imgGiveAway.image=nil;                
                        }else{
                            NSLog(@"Nothing 6");
                            UIImage *img = [UIImage imageNamed:@"giftcard.png"];
                            imgGiveAway.image = img;
                        }
                    }
                }
                
                if ([[prefs objectForKey:@"sweepstake"] isEqualToString:@"0"]) {
                    
                    imgGiveAway.image=NULL;
                    
                    btnsweepstake.hidden=YES;
                    
                    lblwithwin.hidden=YES;
                }
            }
        
        else{
            
            if (!imgGiveAway.image) {
                
                if ([[prefs objectForKey:@"sweepstake"] isEqualToString:@"1"]) {
                    
                    if (appDelegate.flggiveaway) {
                        
                        NSLog(@"Nothing 1");
                        
                        NSFileManager *fileManager = [NSFileManager defaultManager];
                        
                        [fileManager removeItemAtPath:pathGiveAway error:nil];
                        
                        [prefs setObject:pathGiveAway forKey:@"pathGiveAway"];
                        
                        [prefs synchronize];
                        
                        imgGiveAway.image=nil; 
                    }
                    else{ 
                        
                        NSLog(@"Nothing 2");
                        
                        UIImage *img = [UIImage imageNamed:@"giftcard.png"];
                        
                        [UIImagePNGRepresentation(img) writeToFile:pathGiveAway atomically:YES];
                        
                        [prefs setObject:pathGiveAway forKey:@"pathGiveAway"];
                        
                        [prefs synchronize];
                        
                        lblwithwin.hidden=NO;
                        
                        imgGiveAway.image = img;
                        
                        btnsweepstake.hidden=NO;
                    }
                }
                
                else {
                    
                    imgGiveAway.image=NULL;
                    
                    lblwithwin.hidden=YES;
                    
                    lblwithwin.hidden=YES;
                }
            }
            
        }
        
        }
        
        
    }
    
    else 
    {
        NSLog(@" error in parsing "); 
    }
    
    [pool release];
}

- (void)viewWillAppear:(BOOL)animated
{
    NSLog(@"viewWillAppear write");

    NSLog(@"appdelegate remote write : %d",appDelegate.remoteHostStatus);
    
	appDelegate.flgClickOnWriteReview=FALSE;
    
	appDelegate.flgClcikOnRecordReview=FALSE;
    
	appDelegate.flgTypeRecordClick =FALSE;
	
	if (appDelegate.flgswich) {
        
		btnadmin.hidden=NO;
	}
    
	else {
		btnadmin.hidden=NO;
	}
	
	[Home setImage:[UIImage imageNamed:@"NNewhomebtnD_v2.png"] forState:UIControlStateNormal];
    
    NSLog(@"appDelegate.remoteHostStatus  %d",appDelegate.remoteHostStatus);
        
//	if (appDelegate.flggiveaway) {
//        
//		lblwithwin.hidden=YES;
//        
//		lblwithoutwin.hidden=NO;
//	}
//	else {
//        
//		lblwithwin.hidden=NO;
//        
//		lblwithoutwin.hidden=YES;
//	}
	
}
-(IBAction)btnsweepstakeClcik{
    
    SweepStakesViewController *objPopview = [[SweepStakesViewController alloc]initWithNibName:@"SweepStakesViewController" bundle:nil];    
    
    popSweepStakes =[[UIPopoverController alloc]initWithContentViewController:objPopview];
    
    [popSweepStakes presentPopoverFromRect:CGRectMake(840, 30, 200, 100) inView:self.view permittedArrowDirections:UIPopoverArrowDirectionRight animated:YES];
    
    [popSweepStakes setPopoverContentSize:CGSizeMake(200, 100)];
}


- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	int value = [touches count];
	NSLog(@"intvalue%d",value);
	if (value==3) {
	 
      if(appDelegate.remoteHostStatus!=0) 
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
      else{
          
          UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet is not available" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
          [alert show];
          [alert release];
       //   INTERNET not AVAILABLE
      }
		
	}
	
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
	}	
}

-(IBAction) btnWriteReviewClick
{
    NSLog(@"UseEmail:->%@",appDelegate.UserEmail);
    NSLog(@"BusinessId:->%@",appDelegate.userBusinessId);
    NSLog(@"Useid:->%@",appDelegate.userId);
    
	appDelegate.flgClickOnWriteReview=TRUE;
    
    if (appDelegate.remoteHostStatus==0) {
        
        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet is not available" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [alert show];
        [alert release];
    }
    
    else{
        
        ApplicationValidation *objapplication = [[ApplicationValidation alloc]initWithNibName:@"ApplicationValidation" bundle:nil];
        [self.navigationController pushViewController:objapplication animated:YES];
        [objapplication release];
    }
}

-(IBAction) btnRecordReviewClick
{
	appDelegate.flgClcikOnRecordReview=TRUE;
	appDelegate.flgTypeRecordClick =TRUE;
    
	if (appDelegate.remoteHostStatus==0) {
        
        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet is not available" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [alert show];
        [alert release];
    }
    
    else{
        
        ApplicationValidation *objapplication = [[ApplicationValidation alloc]initWithNibName:@"ApplicationValidation" bundle:nil];
        [self.navigationController pushViewController:objapplication animated:YES];
        [objapplication release];
    }
}
- (void)viewWillDisappear:(BOOL)animated
{
	[Home setImage:[UIImage imageNamed:@"NNewhomebtn_v2.png"] forState:UIControlStateNormal];
}
-(IBAction) Admin
{
    if (appDelegate.remoteHostStatus!=0) {
         
	if(appDelegate.LoginConform==FALSE)		
	{	HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];		
        [self.navigationController pushViewController:historylogin animated:YES];
        [historylogin release];
	}else{
		OptionOfHistoryViewController *objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
        [self.navigationController pushViewController:objoption animated:YES];
        [objoption release];		
	}	
        
    }
    else{
        
        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet is not available" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [alert show];
        [alert release];
        
    }
}
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation 
{
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
