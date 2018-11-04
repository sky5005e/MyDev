//
//  Term_para.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 07/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Term_para.h"
#import <QuartzCore/QuartzCore.h>
#import "Terms_Condition.h"
#import "Review_FrogAppDelegate.h"
#import "Review_FrogViewController.h"
#import "Review_FrogViewController.h"
#import "History.h"
#import "MICheckBox.h"
#import "Thank_you.h"
#import "Review_FrogAppDelegate.h"
#import "HistoryLogin.h"
#import "AdminCoupon.h"
#import "SoundSwitch.h"
#import "ApplicationValidation.h"
#import "ThreeTabButton.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h"

@implementation Term_para
@synthesize window;
@synthesize submitflag;
@synthesize sumhidde;
@synthesize btncontinue3;
@synthesize ActivIndicator;
@synthesize avideo;

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.

- (void)viewDidLoad {
    appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
    [self.ActivIndicator startAnimating];
    
    if (appDelegate.remoteHostStatus!=0) {
        NSURL *url = [[NSURL alloc]initWithString:[NSString stringWithFormat:@"%@",appDelegate.Set_Termurl]];
        NSLog(@"appDelegate.Set_Termurl %@",appDelegate.Set_Termurl);
        NSURLRequest *url1=[[NSURLRequest alloc]initWithURL:url];
        
        //        NSURLRequest *urlreq =[NSURL fileURLWithPath:[[NSBundle mainBundle] pathForResource:@"termsconditions.php" ofType:@"html"]isDirectory:NO];
        
        [webTerm loadRequest:url1];
    }
    else{
        
        [webTerm loadRequest:[NSURLRequest requestWithURL:[NSURL fileURLWithPath:[[NSBundle mainBundle] pathForResource:@"termsconditions" ofType:@"html"]isDirectory:NO]]];
        
        //  [webTerm loadRequest:urlreq];
        
    }
    
	sumhidde.backgroundColor=[UIColor clearColor];
	
	MICheckBox *checkBox =[[MICheckBox alloc]initWithFrame:CGRectMake(179, 402, 35, 35)];
	[checkBox setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
	[self.view addSubview:checkBox];
	btncontinue3.backgroundColor=[UIColor clearColor];
	
	if (appDelegate.flgswich) 
    {
		btnadmin.hidden=YES;
	}
	else
    {
		btnadmin.hidden=YES;
	}
	
    [super viewDidLoad];
}
-(void)viewWillAppear:(BOOL)animated
{
    
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
    
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    if (pathBussinesslogo !=nil) {   
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
        imgPowerdby.hidden=YES;
        imgtxtPowerdby.hidden=YES;
    }
    
    if (appDelegate.sweepstake) {
        
        if (pathGiveAway !=nil) {   
            
            imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
            
        }
        
        if (!imgGiveAway.image) {
            UIImage *img = [UIImage imageNamed:@"giftcard.png"];
            imgGiveAway.image = nil;
        }
    }
    
    //    }
}

-(IBAction) Backtohome
{
    [self.ActivIndicator stopAnimating];
	appDelegate.txtcleanflag=TRUE;
    //    ApplicationValidation *awriteorRecode = [[ApplicationValidation alloc]initWithNibName:@"ApplicationValidation" bundle:nil];
    //    [self.navigationController pushViewController:awriteorRecode animated:YES];
    //    [awriteorRecode release];
    
    [self.navigationController popViewControllerAnimated:YES];
    
}


-(IBAction)SubmitClcik  
{
    
	[self.ActivIndicator stopAnimating];
    
    appDelegate.sugestionflag=TRUE;
    
    if (appDelegate.flgTypeRecordClick==FALSE) {
        
        if (appDelegate.remoteHostStatus!=0) {
            
            [self SubmitWriteReview];
            //[self SubmitVideoReview];
            
        }
        
        else{
            // offline 
            [self writeReviewLocal];
        }
    }
    
    else {
        
        [self SubmitVideoReview];
    }	
}
-(void)writeReviewLocal
{
    
    if(appDelegate.checkflag==TRUE)
    {
        
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Internet is not available" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
        [alert show];
        [alert release];
        
    }
    
    else{
        
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please agree to terms and conditions" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
        [alert show];
        [alert release];
        
    }
    
}

-(void) SubmitWriteReview
{	
    if(appDelegate.checkflag==TRUE)
	{
		appDelegate.flgClickOnWriteReview=FALSE;		
		
        appDelegate.checkflag=FALSE;
        
        if ([appDelegate.Data_desc rangeOfString:@"&"].location!=NSNotFound) {
            
            appDelegate.Data_desc=[appDelegate.Data_desc stringByReplacingOccurrencesOfString:@"&" withString:@"aa123aa"];
        }
        
        NSLog(@"appDelegate.Data_desc=%@",appDelegate.Data_desc);
        
		NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?version=2",appDelegate.ChangeUrl]];
        
        NSLog(@"url=%@",url);
		
        NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
        
        [request setHTTPMethod:@"POST"];
        
        NSData *requestBody = [[NSString stringWithFormat:@"api=itunesversion1_1_Hotel_InserttblData&user_email=%@&Data_Name=%@&Desc=%@&uId=%@&Date=%@&Zipcode=%@&status=%@&Title=%@&Rate=%.0f&Data_Category=%@&BusinessID=1859&QuestionNo1=%@&SubCategoryID1=%@&Ratting1=%@&QuestionNo2=%@&SubCategoryID2=%@&Ratting2=%@&QuestionNo3=%@&SubCategoryID3=%@&Ratting3=%@",appDelegate.Data_email,appDelegate.Data_name,appDelegate.Data_desc,appDelegate.Data_useid,appDelegate.Data_date,appDelegate.CityName,appDelegate.Data_Rstatus,appDelegate.Data_Title,appDelegate.Rating,appDelegate.strCategoryname,appDelegate.questionOneId,appDelegate.answerOneId,appDelegate.rating1,appDelegate.questionTwoId,appDelegate.answerTwoId,appDelegate.rating2,appDelegate.questionThreeId,appDelegate.answerThreeId,appDelegate.rating3] dataUsingEncoding:NSUTF8StringEncoding];
        
        
        NSLog(@"api=itunesversion1_1_Hotel_InserttblData&user_email=%@&Data_Name=%@&Desc=%@&uId=%@&Date=%@&Zipcode=%@&status=%@&Title=%@&Rate=%.0f&Data_Category=%@&BusinessID=1859&QuestionNo1=%@&SubCategoryID1=%@&Ratting1=%@&QuestionNo2=%@&SubCategoryID2=%@&Ratting2=%@&QuestionNo3=%@&SubCategoryID3=%@&Ratting3=%@",appDelegate.Data_email,appDelegate.Data_name,appDelegate.Data_desc,appDelegate.Data_useid,appDelegate.Data_date,appDelegate.CityName,appDelegate.Data_Rstatus,appDelegate.Data_Title,appDelegate.Rating,appDelegate.strCategoryname,appDelegate.questionOneId,appDelegate.answerOneId,appDelegate.rating1,appDelegate.questionTwoId,appDelegate.answerTwoId,appDelegate.rating2,appDelegate.questionThreeId,appDelegate.answerThreeId,appDelegate.rating3);
        
		[request setHTTPBody:requestBody];
        
        
        
        [request addValue:[NSString stringWithFormat:@"%d",[requestBody length]] forHTTPHeaderField:@"Content-Length"];
        
        
		NSURLResponse *response = NULL;
        
		NSError *requestError = NULL;
        
		NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
        
		NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
        NSString *resString = @"1";
        
        NSLog(@"response %@",responseString);
        
        NSString *combineString=[NSString stringWithFormat:@"-----sent Data------\n%@\n\n\n------Your Response-------\n%@",[NSString stringWithFormat:@"api=itunesversion1_1_Hotel_InserttblData&user_email=%@&Data_Name=%@&Desc=%@&uId=%@&Date=%@&Zipcode=%@&status=%@&Title=%@&Rate=%.0f&Data_Category=%@&BusinessID=1859&QuestionNo1=%@&SubCategoryID1=%@&Ratting1=%@&QuestionNo2=%@&SubCategoryID2=%@&Ratting2=%@&QuestionNo3=%@&SubCategoryID3=%@&Ratting3=%@",appDelegate.Data_email,appDelegate.Data_name,appDelegate.Data_desc,appDelegate.Data_useid,appDelegate.Data_date,appDelegate.CityName,appDelegate.Data_Rstatus,appDelegate.Data_Title,appDelegate.Rating,appDelegate.strCategoryname,appDelegate.questionOneId,appDelegate.answerOneId,appDelegate.rating1,appDelegate.questionTwoId,appDelegate.answerTwoId,appDelegate.rating2,appDelegate.questionThreeId,appDelegate.answerThreeId,appDelegate.rating3],responseString];
        
        
        
		responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
        
        NSLog(@"responseTrim %@",responseString);
        
        
        NSLog(@"responseNew %@",responseString);
        
        NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
		if (rngReturnStart.length > 0) {
			responseString = [responseString substringFromIndex: rngReturnStart.location+ rngReturnStart.length];
		}
		
		NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
		
		if (rngReturnEnd.length > 0) {
			responseString = [responseString substringToIndex: rngReturnEnd.location];
		}
        
        
        if ([responseString isEqualToString:@"1"])
		{		
            NSLog(@"DONE");
			appDelegate.Data_Title = @"";
			appDelegate.Data_name = @"";
            
			appDelegate.CityName = @"";
			appDelegate.VideoName = @"";
			appDelegate.Rating = 0;
			appDelegate.strCategoryname = @"";
			
			appDelegate.FirstAns=@"";
			appDelegate.SecondAns=@"";
			appDelegate.ThridAns=@"";
			
			[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
            appDelegate.Rating = 0;
            appDelegate.txtcleanflag=FALSE;
            Thank_you *Thank = [[Thank_you alloc]init];
            
            [self.navigationController pushViewController:Thank animated:YES];
            
            [Thank release];
		}
        else 
        {
            appDelegate.checkflag=TRUE;
            NSLog(@"Not Done");	
            UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Server not responding.  Please try again after some time " delegate:self
                                                     cancelButtonTitle:@"OK" otherButtonTitles:nil];
            
            [altError show];
            [altError release];
            /*
             [self writeReviewLocal];
             */
        }
        
	}
	else {
		
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please agree to terms and conditions" delegate:self 
											  cancelButtonTitle:@"OK" 
											  otherButtonTitles:nil];
		[alert show];
		[alert release];
		
		
	}
	
}


-(void) SubmitVideoReview
{
    //    http://74.208.68.90/ReviewFrogCN/main/frmReviewFrogApi.php?api=itunesversion1_1_Hotel_Inserttblvideo&videoid=1&videotitle=aaaa
    
    if(appDelegate.checkflag==TRUE)
	{
		appDelegate.flgClcikOnRecordReview=FALSE;
		appDelegate.checkflag=FALSE;
		
        if (appDelegate.remoteHostStatus!=0) {
            
            // INTERNET availbale -------
            
            //NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi.php?",self.ChangeUrl]];
            
            NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
            
            NSString *documentsDirectory = [paths objectAtIndex:0];
            
            NSString *appFile = [documentsDirectory stringByAppendingPathComponent:[NSString stringWithFormat:@"%@.mov",appDelegate.Data_Title]];
            
            if ([appDelegate.DataVideo writeToFile:appFile atomically:YES]) {
                
                NSLog(@"file is written");
            }
            
            else {
                NSLog(@"file not written");
            }
            
            NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php",appDelegate.ChangeUrl]];
            
            
            
            ASIFormDataRequest *request = [ASIFormDataRequest requestWithURL:url];
            
            request.delegate = self;
            
            [request addFile:appFile forKey:@"videofile"];
            
            [request addPostValue:@"itunesversion1_1_Hotel_Inserttblvideo" forKey:@"api"];
            
            [request addPostValue:@"143143" forKey:@"videoid"];
            
            [request addPostValue:@"2" forKey:@"version"];
            
            NSLog(@"Video Title -->%@",appDelegate.Data_Title);
            
            [request addPostValue:appDelegate.Data_Title forKey:@"videotitle"];
            
            [request addPostValue:appDelegate.Data_email forKey:@"user_email"];
            
            [request addPostValue:appDelegate.Data_name forKey:@"dataname"];
            
            [request addPostValue:appDelegate.Data_useid forKey:@"uid"];
            
            [request addPostValue:appDelegate.Data_date forKey:@"date"];
            
            [request addPostValue:appDelegate.CityName forKey:@"cityname"];
            
            [request addPostValue:appDelegate.Data_Rstatus forKey:@"status"];
            
            [request addPostValue:appDelegate.Data_Title forKey:@"title"];
            
            [request addPostValue:[NSString stringWithFormat:@"%f",appDelegate.Rating] forKey:@"rate"];
            
            [request addPostValue:appDelegate.strCategoryname forKey:@"Data_Category"];
            
            [request addPostValue:appDelegate.FirstAns forKey:@"videoquestion1"];
            
            [request addPostValue:appDelegate.SecondAns forKey:@"videoquestion2"];
            
            [request addPostValue:appDelegate.ThridAns forKey:@"videoquestion3"];
            
            
            [request setAllowCompressedResponse:YES];
            
            [request startSynchronous];
            
            NSLog(@" response --> %@",[request responseString]);
            
            NSLog(@" response end ");
           
            
            NSString *responseString = [request responseString];
            
            NSData *responseData = [responseString dataUsingEncoding:NSUTF8StringEncoding];
            
            NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:responseData];
            
            //Initialize the delegate.
            xmlparser_para *parser = [[xmlparser_para alloc] initxmlparser_para];
            
            //Set delegate
            [xmlParser setDelegate:parser];
            
            //Start parsing the XML file.
            BOOL success = [xmlParser parse];
            
            if(success)
            {
                
                // post video alsoooooo-----
                
                //[appDelegate SubmitVideo];
                
                // The hud will displayble all input on the view (use the higest view possible in the view hierarchy)
                
                 HUD = [[MBProgressHUD alloc] initWithView:self.view];
                 
                 // Add HUD to screen
                 [self.view addSubview:HUD];
                 
                 // Register for HUD callbacks so we can remove it from the window at the right time
                 HUD.delegate = self;
                 
                 HUD.labelText = @"Loading";
                 
                 // Show the HUD while the provided method executes in a new thread
                 [HUD showWhileExecuting:@selector(loadingVideoStart) onTarget:self withObject:nil animated:YES];
                 
                 /*mychanges */
                
                for (int k=0; k<[appDelegate.delarrVideoList count]; k++) {
                    
                    Video *objVideo = [appDelegate.delarrVideoList objectAtIndex:k];
                    NSLog(@" // call video upload api 1----- ");
                    NSLog(@"appDelegate.VideoName %@",appDelegate.VideoName);
                    NSLog(@"objVideo.ReviewVideoName %@",objVideo.ReviewVideoName);
                    
                    NSLog(@"objVideo.id-->%d",objVideo.id);
                    if ([appDelegate.VideoName isEqualToString:objVideo.ReviewVideoName])
                    {
                        NSLog(@"objVideo.ReviewVideoName %@",objVideo.ReviewVideoName);
                        // call video upload api -----
                        NSLog(@" // call video upload api 2----- ");
                        //[appDelegate apiPostReviewVideo:objVideo.id VTitle:objVideo.ReviewVideoName video:appDelegate.DataVideo];
                        
                    }
                }
                
                NSLog(@"No Errors");
                appDelegate.Data_Title = @"";
                appDelegate.Data_name = @"";
                appDelegate.Data_email = @"";
                appDelegate.CityName = @"";
                appDelegate.VideoName = @"";
                appDelegate.Rating = 0;
                appDelegate.strCategoryname = @"";
                
                appDelegate.FirstAns=@"";
                appDelegate.SecondAns=@"";
                appDelegate.ThridAns=@"";
                [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
                Thank_you *Thank = [[Thank_you alloc]init];
                [self.navigationController pushViewController:Thank animated:YES];
                [Thank release];
                
                //--- -----------   //start upload video------------------
                
                //            isUploaded =[appDelegate apiPostReviewVideo:avideo.id VTitle:avideo.ReviewVideoName video:appDelegate.NewData];
                
                
            }	
            else
            {		
                appDelegate.checkflag=TRUE;
                
                NSLog(@"Error Error Error!!! video review");
                // -- Add to Data base 
                [self videoStoreInDB];
                
                //			UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self
                //													 cancelButtonTitle:@"OK" otherButtonTitles:nil];
                //			
                //			[altError show];
                //			[altError release];
                
            }
            
        }
        else{
            [self videoStoreInDB];
        }
        
        
	}
	
	else {
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please click check box to agree for terms and condition" delegate:self 
											  cancelButtonTitle:@"OK" 
											  otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
    
}

-(void) loadingVideoStart
{
	NSLog(@"AutoResponder:-%@", avideo.ReviewPersonEmail);
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	if (appDelegate.flgReRecSub==TRUE) 
    {
		appDelegate.flgReRecSub=FALSE;
		isUploaded =[appDelegate apiPostReviewVideo:avideo.id VTitle:avideo.ReviewVideoName video:appDelegate.DataVideo];	
        
	}
    else 
    {
		
		isUploaded =[appDelegate apiPostReviewVideo:avideo.id VTitle:avideo.ReviewVideoName video:appDelegate.NewData];
        
	}		
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


-(void)videoStoreInDB
{
    // 		// submit in local if it is not available.
    
    [appDelegate addVideoReview:appDelegate.Data_name email:appDelegate.Data_email city:appDelegate.CityName questionone:appDelegate.FirstAns questiontwo:appDelegate.SecondAns questionthree:appDelegate.ThridAns category:appDelegate.strCategoryname reviewtitle:appDelegate.Data_Title rating:[NSString stringWithFormat:@"%f",appDelegate.Rating] userid:appDelegate.Data_useid status:appDelegate.Data_Rstatus date:appDelegate.Data_date ReviewVideoName:appDelegate.VideoName];
    
    appDelegate.Data_Title = @"";
    appDelegate.Data_name = @"";
    appDelegate.Data_email = @"";
    appDelegate.CityName = @"";
    appDelegate.VideoName = @"";
    appDelegate.Rating = 0;
    appDelegate.strCategoryname = @"";
    
    appDelegate.FirstAns=@"";
    appDelegate.SecondAns=@"";
    appDelegate.ThridAns=@"";
    
    
    [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
    Thank_you *Thank = [[Thank_you alloc]init];
    [self.navigationController pushViewController:Thank animated:YES];
    [Thank release];
    
}

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	NSLog(@"touchesBegan");
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
	}
	
}

-(IBAction)Home{
    
    [self.navigationController popToViewController:[[self.navigationController viewControllers] objectAtIndex:1] animated:YES];
	
}
-(IBAction) Admin
{		[self.ActivIndicator stopAnimating];
	if(appDelegate.LoginConform==FALSE){	
        HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];	
        [self.navigationController pushViewController:historylogin animated:YES];
        [historylogin release];
        
	}else{
        OptionOfHistoryViewController *objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
        [self.navigationController pushViewController:objoption animated:YES];
        [objoption release];
        
	}	
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


- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
}


- (void)viewDidUnload {
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

-(void)webViewDidFinishLoad:(UIWebView *)webView
{	
	[self.ActivIndicator stopAnimating];
}
- (void)dealloc {
    [window release];
    [super dealloc];
}


@end
