//
//  ApplicationValidation.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 02/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Review_FrogViewController.h"
#import "ApplicationValidation.h"
#import "WebAndListingViewController.h"
#import "WriteORRecodeViewController.h"
#import "LoginInfo.h"
#import "Category.h"


@implementation Review_FrogViewController


@synthesize txtUserEmail;
@synthesize txtUSerPassword;


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	[self shouldAutorotateToInterfaceOrientation:UIInterfaceOrientationIsLandscape(UIInterfaceOrientationLandscapeLeft)];
    NSNotificationCenter *notificationCenter = [NSNotificationCenter defaultCenter];
    [[UIDevice currentDevice] beginGeneratingDeviceOrientationNotifications];
    [notificationCenter addObserver:self selector:@selector(shouldAutorotateToInterfaceOrientation:) name:UIDeviceOrientationDidChangeNotification object:nil];
}

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{	
	if(appDelegate.SwitchFlag ==TRUE)
	{
		NSLog(@"touchesBegan");
		
		[appDelegate resetIdleTimer];
	}
}
- (void)viewWillAppear:(BOOL)animated {
    
	[super viewWillAppear:YES];
}

- (void)viewDidAppear:(BOOL)animated
{	
	if ([appDelegate.UserEmail isEqualToString:@""]|| appDelegate.UserEmail == nil) 
	{
		NSLog(@"UserEmail:-%@",appDelegate.UserEmail);
	}
	else {
		[self getApplicationValidation];
	}	
}
-(IBAction) BackClick
{
	appDelegate.UserEmail=@"";
	[appDelegate.UserEmail isEqual:@""];
	(appDelegate.UserEmail = nil);
	
	NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
	
	[prefs setObject:appDelegate.UserEmail forKey:@"UserEmail"];
    WebAndListingViewController *objwebandlisting = [[WebAndListingViewController alloc] initWithNibName:@"WebAndListingViewController" bundle:nil];
    [self.navigationController pushViewController:objwebandlisting animated:YES];
}

-(IBAction)LoginClick
{	
	if(appDelegate.remoteHostStatus==0)
	{
		UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Internet Connection not available currently. Please check your internet connectivity and try again after sometime." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alert show];
		[alert release];
	}
	else if ([[txtUserEmail.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""]) {
		
			UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please Enter Email" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
			[alert show];
			[alert release];
	}
	else
	{	[txtUserEmail resignFirstResponder];
		[self getApplicationValidation];
	}
}

-(void)getApplicationValidation
{
    HUD = [[MBProgressHUD alloc] initWithView:self.view];

    [self.view addSubview:HUD];

    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    [HUD showWhileExecuting:@selector(startvalidation) onTarget:self withObject:nil animated:YES];
}

-(void)	startvalidation
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
	NSString * str;
	if ([appDelegate.UserEmail isEqualToString:@""]|| appDelegate.UserEmail == nil) 
	{
        str = [NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_LoginCheck_LogoSplash&user_email=%@&version=2",appDelegate.ChangeUrl,txtUserEmail.text];	
    }else
    {
        str = [NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_LoginCheck_LogoSplash&user_email=%@&version=2",appDelegate.ChangeUrl,appDelegate.UserEmail];
	}
	
	NSURL *url =[NSURL URLWithString:str];
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
	xmlparser *parser = [[xmlparser alloc] initxmlparser];
	
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<userdata><return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return></userdata>"];
	//<?xml version="1.0" encoding="UTF-8" ?>
	//<userdata><return>null</return></userdata>
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<userdata><return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return></userdata>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	NSLog(@"responseString :%@",responseString);
	if ([responseString isEqualToString:@"null"]) 
    {
		NSLog(@"Error Error Error!!! View Controller");
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Your are not registered user please check your email address " delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[altError show];
		[altError release];
		
	} else 
    {
		
    [[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
        
        if(appDelegate.FirstUser==TRUE)
        {
			if ([appDelegate.UserEmail isEqualToString:@""]|| appDelegate.UserEmail == nil) 
			{
                NSUserDefaults *prefs=[NSUserDefaults standardUserDefaults];
                
				alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
				int userid = alogin.id;
				appDelegate.UserEmail= alogin.userEmail;
				appDelegate.userId=[NSString stringWithFormat:@"%d",userid];
                
                NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
                NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents folder
               
                
                NSString *profolder2=@"Bussiness.png";
                NSString *profolder3=@"GiveAway.png";
                
                
                [prefs setObject:appDelegate.userId forKey:@"userid"];
                [prefs setObject:appDelegate.UserEmail forKey:@"UserEmail"];
                
                {
					
					NSURL *url = [[NSURL alloc] initWithString:alogin.BusinessSplash];
                    
					appDelegate.dataSplash = [[NSData alloc] initWithContentsOfURL:url];
					UIImage *img = [UIImage imageWithData:appDelegate.dataSplash];
					alogin.imgBsplash = img;
                    
                    
                    //----here store image 3 image-----
                    
                    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
                    NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents folder
                    NSString *profolder1=@"SplashImageDB.png";
                    NSString *pathSplash = [documentsDirectory stringByAppendingPathComponent:profolder1];
                    
                    
                    NSData *imageData = UIImagePNGRepresentation(img);
                    UIImage *imgPNG = [[UIImage alloc] initWithData:imageData];
                    
                    
                    if (imageData != nil) {
                        [UIImagePNGRepresentation(imgPNG) writeToFile:pathSplash atomically:YES];
                        
                        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
                        [prefs setObject:pathSplash forKey:@"pathSplash"];
                       
                        
                        [prefs synchronize];
                    }
				}	
                
				if (![alogin.userbusinesslogo isEqualToString:@"0"])
                {
				    NSString *pathBussinesslogo = [documentsDirectory stringByAppendingPathComponent:profolder2];
                    
                    NSURL *url1 = [[NSURL alloc] initWithString:alogin.userbusinesslogo];
                    NSLog(@"url1:->%@",url1);
					appDelegate.data1 = [[NSData alloc] initWithContentsOfURL:url1];
				    UIImage *imgPNG1 = [[UIImage alloc] initWithData:appDelegate.data1];
                    if (appDelegate.data1 != nil) {
						[UIImagePNGRepresentation(imgPNG1) writeToFile:pathBussinesslogo atomically:YES];
						[prefs setObject:pathBussinesslogo forKey:@"pathBussinesslogo"];
                    }
				}	
                
                
//----here store image 3 image-----
                                
              	if (![alogin.userbusinessgiveaway isEqualToString:@"0"])
	            {   
			        NSString *pathGiveAway = [documentsDirectory stringByAppendingPathComponent:profolder3];
                    
                   	NSURL *url2 = [[NSURL alloc] initWithString:alogin.userbusinessgiveaway];
					appDelegate.data2 = [[NSData alloc] initWithContentsOfURL:url2];
                    
                    UIImage *imgPNG2 = [[UIImage alloc] initWithData:appDelegate.data2];
                    
					if (appDelegate.data2 != nil) {
                        [UIImagePNGRepresentation(imgPNG2) writeToFile:pathGiveAway atomically:YES];
                        [prefs setObject:pathGiveAway forKey:@"pathGiveAway"];
					}
                    
				}
                if (![alogin.userbusinessid isEqualToString:@"0"])
	            {          
                    [prefs setObject:alogin.userbusinessid forKey:@"BusinessId"];
				}
                [prefs synchronize];
                [appDelegate getUserPreferences];
                
                
				[appDelegate saveUserPreferences:0 ];
			}
            NSLog(@"alogin.userbusinessgiveaway %@",alogin.userbusinessgiveaway);
            
            // add to database .
//            [appDelegate addToLoginDetail:alogin.userbusinesslogo BusinessSplash:alogin.BusinessSplash questionone:alogin.questionone questiontwo:alogin.questiontwo questionthree:alogin.questionthree userbusinessid:appDelegate.userId];
            
            NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
            [prefs setObject:alogin.questionone forKey:@"Q1"];
            [prefs setObject:alogin.questiontwo forKey:@"Q2"];
            [prefs setObject:alogin.questionthree forKey:@"Q3"];
            [prefs synchronize];
            
            // category list from server--------
            [appDelegate apiCategory];
            
            NSLog(@"array %d",[appDelegate.ArrCategoryList count]);
            
            for (int i=0; i<[appDelegate.ArrCategoryList count]; i++) 
            {
                Category *objCategory =[appDelegate.ArrCategoryList objectAtIndex:i];
                NSLog(@"objCategory.categoryname%@",objCategory.categoryname);
                
                // store in db category ----------------
                [appDelegate addCategory:objCategory.Category_UserID category:objCategory.categoryname];
            }
            
            WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
            
            [self.navigationController pushViewController:awriteorRecode animated:YES];
            
            [awriteorRecode release];
		}
		else 
        {
			UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
			[altError show];
			[altError release];
		}
    }
	[pool release];
}	

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    NSLog(@"shouldAutorotateToInterfaceOrientation");
	if(interfaceOrientation == UIInterfaceOrientationLandscapeRight || interfaceOrientation == UIInterfaceOrientationLandscapeLeft)
		return YES;
	else if(interfaceOrientation == UIInterfaceOrientationPortrait)
		return NO;
	else 
		return NO;
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
}

- (void)viewDidUnload {
    [super viewDidUnload];
}

- (void)dealloc {
    [super dealloc];
}
@end
