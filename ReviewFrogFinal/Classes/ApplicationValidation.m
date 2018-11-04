//
//  Review_FrogViewController.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 31/12/10.
//  Copyright __MyCompanyName__ 2010. All rights reserved.
//


#import "ApplicationValidation.h"
#import "xmlparser.h"
#import "Review_FrogAppDelegate.h"
#import "History.h"
#import "Terms_Condition.h"
#import "HistoryLogin.h"
#import "AdminCoupon.h"
#import "SoundSwitch.h"
#import "ThreeTabButton.h"
#import "Cityname.h"
#import "XMLCityname.h"
#import "WebAndListingViewController.h"
#import "WriteORRecodeViewController.h"
#import "OptionOfHistoryViewController.h" 
#import "CameraContinuViewController.h"
#import "Term_para.h"
#import "Category.h"
#import "LoginInfo.h"
#import "XMLBusinessProfile.h"
#import <MobileCoreServices/UTCoreTypes.h>
#import "CategoryPopViewController.h"
#import "SweepStakesViewController.h"
#import "SubcategoryPopViewController.h"
#import "RatingPopViewController.h"

@implementation ApplicationValidation

@synthesize Name;
@synthesize Email;
@synthesize ErrorCode;
@synthesize currentElement;
@synthesize btnHome;
@synthesize stringbirthdate;
@synthesize pickeflag;
@synthesize worngflag;
@synthesize btncontinue;
@synthesize txtCityCode;
@synthesize selectedcity;
@synthesize fistCity2;
@synthesize pickerflg;

@synthesize rateView = _rateView;
@synthesize btnDone;
@synthesize btnWrite;
@synthesize btnVideo;
@synthesize imgtxtbordar;
@synthesize picCategory;
@synthesize tlbCategory;
@synthesize txtcategory;
@synthesize txtDescription;
@synthesize txtTitle;

@synthesize txtQuestionOne;	
@synthesize txtQuestionTwo;
@synthesize txtQuestionThree;
@synthesize checkFLAG;
@synthesize stopWatchTimer;

int count = 0;

- (void)viewDidLoad 
{
    
    
    touchflag=0;
    
    //ViewPost.multipleTouchEnabled=NO;
    
    //self.view.multipleTouchEnabled=NO;
    
    //scrView.multipleTouchEnabled=NO;
    
	[super viewDidLoad];
    // this is for touch.
    UITapGestureRecognizer *tapGesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(tapGestureHandler:)];
    tapGesture.cancelsTouchesInView = NO;
    [scrView addGestureRecognizer:tapGesture];
    
    // this is for drag and touch both
    //UITapGestureRecognizer *singledrag = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(scrollViewWillBeginDragging:)];
    //[scrView addGestureRecognizer:singledrag];
    
    NSLog(@"viewDidLoad");
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
    
	_rateView.notSelectedImage = [UIImage imageNamed:@"rating40X40.png"];
    _rateView.fullSelectedImage = [UIImage imageNamed:@"rating40X40_hover.png"];
   	_rateView.rating = 5;
    _rateView.editable = YES;
    _rateView.maxRating = 5;
    _rateView.delegate = self;
	picCategory.hidden=YES;
	tlbCategory.hidden=YES;
	
	[scrView addSubview:ViewPost];
	[scrView setContentSize:CGSizeMake(ViewPost.frame.size.width, ViewPost.frame.size.height)];
	NSLog(@"width:-%f",ViewPost.frame.size.width);
	
	NSLog(@"height:-%f",ViewPost.frame.size.height);
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillShowNotification:)  
												 name:UIKeyboardWillShowNotification  
											   object:nil];  
	
	[[NSNotificationCenter defaultCenter] addObserver:self  
											 selector:@selector(keyboardWillHideNotification)  
												 name:UIKeyboardWillHideNotification  
											   object:nil]; 
	btncontinue.backgroundColor=[UIColor clearColor];
	fistCity2=TRUE;
	imgQueone.hidden=YES;
	imgQueTwo.hidden=YES;
	imgQueThree.hidden=YES;
	
	if (appDelegate.flgClickOnWriteReview) {
		
		btnVideo.hidden=YES;
		btnWrite.hidden=NO;
		btnWrite.hidden = NO;
		imgtxtbordar.hidden = NO;
		txtDescription.hidden = NO;
        
		
	}
	else if(appDelegate.flgClcikOnRecordReview)
	{
		btnWrite.hidden=YES;		
		imgtxtbordar.hidden = YES;
		txtDescription.hidden=YES;
		btnVideo.hidden=NO;
        
	}
	if (appDelegate.flgswich) {
		btnadmin.hidden=YES;
	}
	else {
		btnadmin.hidden=YES;
	}
	
	txtcategory.text = appDelegate.strDefaultCategory;
    
	appDelegate.strCategoryname = appDelegate.strDefaultCategory;
    
    Name.text = @"";
    [self RescheduleTimer];
    [self performSelectorOnMainThread:@selector(showResponse) withObject:Nil waitUntilDone:YES];
}
// touch event in scroll view. 
/*
- (void)singleTapGestureCaptured:(UITapGestureRecognizer *)gesture
{ 
    CGPoint touchPoint=[gesture locationInView:scrView];
    NSString *string = [NSString stringWithFormat:@"%d", touchPoint]; //touchp
    NSLog(@"touchPoint %d", string);
    
    
    [self RescheduleTimer];
}
*/

- (void) tapGestureHandler:(UIGestureRecognizer *) gestureRecognizer 
{
    
    // Get the position of the point tapped in the window co-ordinate system
    CGPoint tapPoint = [gestureRecognizer locationInView:nil];
    
    // If there are no buttons beneath this tap then move to the next page if near the page edge
    UIView *viewAtBottomOfHeirachy = [appDelegate.window hitTest:tapPoint withEvent:nil];
    if (![viewAtBottomOfHeirachy isKindOfClass:[UIButton class]]) {
        
        CGPoint touchPoint=[gestureRecognizer locationInView:scrView];
        NSString *string = [NSString stringWithFormat:@"%d", touchPoint]; //touchp
        NSLog(@"touchPoint1 %@", string);
        [self RescheduleTimer];
    }   
}
 
/*
- (void)scrollViewWillBeginDragging:(UIScrollView *)scrollView
{
    
    NSLog(@"scrollViewWillBeginDragging");
    [self RescheduleTimer];
    
}
*/
-(void)viewWillAppear:(BOOL)animated
{
    
    NSLog(@"viewWillAppear");
    
    if (termpage==TRUE) {
		
		Term_para *Termsp = [[Term_para alloc]init];
        [self.navigationController pushViewController:Termsp animated:YES];
        [Termsp release];
        
        termpage=FALSE;
	}       
    
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    
    //NSString *str=[prefs objectForKey:@"pathGiveAway"];
    
    //    if (str !=nil) {
    //        
    //        imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:str] autorelease];
    //    }
    
    NSString *obj=[prefs objectForKey:@"pathBussinesslogo"];
    
    if (obj !=nil) {
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:obj] autorelease];
    }  
    
    //    if (!imgGiveAway.image) {
    //        // UIImage *img = [UIImage imageNamed:@"giftcard.png"];
    //        imgGiveAway.image = nil;
    //    }
    
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
        imgPowerdby.hidden=YES;
        imgtxtPowerdby.hidden=YES;
    }
    
    // image for GiveAWay --
    
    //	if (appDelegate.flggiveaway) {
    //        
    //		lblwithwin.hidden=YES;
    //        
    //		lblwithoutwin.hidden=NO;
    //	}
    //    
    //	else {
    //        
    //		lblwithwin.hidden=NO;
    //        
    //		lblwithoutwin.hidden=YES;
    //	}
    
}

-(void)checkResponse
{
    
    NSLog(@"checkResponse");
    
    [appDelegate apiCategory];
    
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    
    [prefs setValue:@"0" forKey:@"sweepstake"];
    
    if (appDelegate.sweepstake) {
        
        //        lblwithwin.hidden=YES;
        //        
        //        lblwithoutwin.hidden=YES;
        //        
        //        imgGiveAway.hidden=YES;
        //        
        //        btnsweepstake.hidden=YES;
        
        [prefs setValue:@"1" forKey:@"sweepstake"];
        
        UIImage *img = [UIImage imageNamed:@"giftcard.png"];
        
        imgGiveAway.image=img;
        
        NSString *pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
        
        lblwithwin.hidden=NO;
        
        NSLog(@"%@",pathGiveAway);
        
        if (pathGiveAway !=nil) {
            
            imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
            
            imgGiveAway.hidden=NO;
            
            btnsweepstake.hidden=NO;
        }
        
        if (!imgGiveAway.image) {
            // UIImage *img = [UIImage imageNamed:@"giftcard.png"];
            imgGiveAway.image = nil;
        }
        //        if (appDelegate.flggiveaway) {
        //            
        //            lblwithwin.hidden=YES;
        //            
        //            lblwithoutwin.hidden=NO;
        //        }
        //        
        //        else {
        //            
        //            lblwithwin.hidden=NO;
        //            
        //            lblwithoutwin.hidden=YES;
        //        }
    }
    
    else {
        
        lblwithwin.hidden=YES;
        
        imgGiveAway.hidden=YES;
        
        btnsweepstake.hidden=YES;
    }
    
    [self loadThreeQuestion];
    ///writecode
    
    
    //    //MBProgressHUD
    //    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
    //    
    //    NSURL *url = [NSURL URLWithString:[[NSString stringWithFormat:@"%@frmReviewFrogApi.php?api=itunesversion1_1_syncstatus&user_id=%@&lc_datetitme=%@",appDelegate.ChangeUrl,appDelegate.userId,[appDelegate.dateStoreinPre stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]]]stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding]];
    //    
    //    NSLog(@"checkResponse in Application:-%@",url);
    //	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
    //	[request setURL:url];
    //	[request setHTTPMethod:@"GET"];
    //	
    //	NSURLResponse *response = NULL;
    //	NSError *requestError = NULL;
    //	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
    //	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
    //	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    //	
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
    //	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    //    NSLog(@"responseString:\n%@",responseString);
    //    
    //
    //    
    //	if ([responseString isEqualToString:@"0"])  {
    //        
    //    NSLog(@" NO CHANGE DONE in ANY API application.");
    //    [self displayFromLocal];
    //    
    //    } else {
    //        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    //        [prefs setObject:responseString forKey:@"dateStore"];
    //        [prefs synchronize];
    //        
    //            NSLog(@"Change ----");
    //            [self loadThreeQuestion];
    //            
    //            //delete and store in db again.
    //            //[appDelegate DeleteQueInfo];
    //            [appDelegate DeleteCategory];
    //            
    //            if ([appDelegate.delArrUerinfo count]>0) {
    //                
    //                alogin = [appDelegate.delArrUerinfo objectAtIndex:0];
    //                
    //                if (appDelegate.flgClcikOnRecordReview ==TRUE &&
    //                    appDelegate.flgTypeRecordClick ==TRUE) {
    //                    // ADd    
    //                    
    //                }
    //             
    //            }
    //        
    //        // Get new category --
    
    
    //        [self timer];
    //        NSLog(@"array %d",[appDelegate.ArrCategoryList count]);
    //        for (int i=0; i<[appDelegate.ArrCategoryList count]; i++) 
    //        {
    //            Category *objCategory =[appDelegate.ArrCategoryList objectAtIndex:i];
    //            NSLog(@"objCategory.categoryname-%@",objCategory.categoryname);
    //            [appDelegate addCategory:objCategory.Category_UserID category:objCategory.categoryname];
    //        }
    //        [appDelegate categoryFromDBMethod];
    //        //change bye kirti
    //        [self displayFromLocal];
    //	}
    //    
    //    [pool release];
    
    
}

-(void)displayFromLocal
{
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
    // fetch offline category.-------------
    [appDelegate categoryFromDBMethod];
    NSLog(@"displayFromLocal");
    txtQuestionOne.hidden=NO;
	txtQuestionTwo.hidden=NO;
	txtQuestionThree.hidden=NO;
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString * q1=[prefs objectForKey:@"Q1"];
    NSString * q2=[prefs objectForKey:@"Q2"];
    NSString * q3=[prefs objectForKey:@"Q3"];
    
    
    NSLog(@"Value 1 %@",q1 );
    NSLog(@"Value 2 %@",q2);
    NSLog(@"Value 3 %@",q3 );
    NSLog(@"user id 3 %@",appDelegate.userId );
    [txtQuestionOne setPlaceholder:q1];
    [txtQuestionTwo setPlaceholder:q2];
    [txtQuestionThree setPlaceholder:q3];
    
    imgQueone.hidden=NO;
    imgQueTwo.hidden=NO;
    imgQueThree.hidden=NO;
    
    [pool release];
}
/*
 -(void)timer
 {
 timer=[NSTimer scheduledTimerWithTimeInterval:240.0f target:self selector:@selector(check) userInfo:nil repeats:YES];
 }
 
 -(void)check
 {
 [self RescheduleTimer];
 NSLog(@"check ");
 if(self.checkFLAG==FALSE)
 {
 [timer invalidate];
 [self btnBackClick];
 [popView dismissPopoverAnimated:YES];
 }
 else
 {
 self.checkFLAG=FALSE;
 NSLog(@"else ");
 [timer invalidate];
 [self timer];
 }
 }
 */

- (void) touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event 
{ 
    NSLog(@"Touches began"); 
    
}
- (void) touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event 
{ 
    NSLog(@"Touches moved"); 
} 
- (void) touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event 
{ 
    NSLog(@"Touches ended"); 
}
- (void) touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event 
{
    NSLog(@"Touches cancelled"); 
}


- (void)rateView:(RateView *)rateView ratingDidChange:(float)rating
{
 	NSLog(@"Ratingdigites------%.0f",rating);
	appDelegate.Rating = rating;
    self.checkFLAG=TRUE;
	NSLog(@"appDelegateRating----%.0f",appDelegate.Rating);
	flgRating=TRUE;
    ///writecode
    [self RescheduleTimer];
}

-(IBAction)btnsweepstakeClcik
{
    SweepStakesViewController *objPopview = [[SweepStakesViewController alloc]initWithNibName:@"SweepStakesViewController" bundle:nil];    
    popSweepStakes =[[UIPopoverController alloc]initWithContentViewController:objPopview];
    [popSweepStakes presentPopoverFromRect:CGRectMake(840, 30, 200, 100) inView:self.view permittedArrowDirections:UIPopoverArrowDirectionRight animated:YES];
    [popSweepStakes setPopoverContentSize:CGSizeMake(200, 100)];
    [self RescheduleTimer];
    
}

- (void)updateTick:(int) Count
{
    count +=1;
    
	if(count>= 10)
	{
        if([self.stopWatchTimer isValid])
        {
            [self.stopWatchTimer invalidate];
            self.stopWatchTimer = nil;
            
        }
        
        count = 0;
        [self btnBackClick];
	}
}

-(void) RescheduleTimer
{
    
    //cancel the previous  ReidrectToHome
    [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(ReidrectToHome) object:nil];    
    
    //perform the search in a seconds time. If the user enters addition data then this search will be cancelled by the previous line
    [self performSelector:@selector(ReidrectToHome) withObject:nil afterDelay:240];
}

- (void) showResponse 
{
    HUD = [[MBProgressHUD alloc] initWithView:self.navigationController.view];
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	[self.navigationController.view addSubview:HUD];
    if (appDelegate.remoteHostStatus!=0) {
        
        [HUD showWhileExecuting:@selector(checkResponse) onTarget:self withObject:nil animated:YES];
    }
    else
    {
        //[HUD showWhileExecuting:@selector(displayFromLocal) onTarget:self withObject:nil animated:YES];
        
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Internet is not available" delegate:nil 
                                              cancelButtonTitle:@"OK" 
                                              otherButtonTitles:nil];
        [alert show];
        [alert release];
    }
}

-(void)loadThreeQuestion
{
    
    //	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
    // 
    //    NSLog(@"loadThreeQuestion ");
    //	NSURL *url =[NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_website.php?api=itunesversion1_1_BusinessProfileSplashLogoQuestions&businessid=%@",appDelegate.ChangeBusiness,appDelegate.userBusinessId]];
    //	NSLog(@"urltab2Application-%@",url);
    //	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
    //	[request setURL:url];
    //	[request setHTTPMethod:@"GET"];
    //	
    //	NSURLResponse *response = NULL;
    //	NSError *requestError = NULL;
    //	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
    //	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
    //	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    //	
    //	NSLog(@"responseString:\n%@",responseString);		
    //	
    //	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
    //	
    //	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
    //	
    //	XMLBusinessProfile *parser = [[XMLBusinessProfile alloc] initXMLBusinessProfile];
    //	
    //	//Set delegate
    //	[xmlParser setDelegate:parser];
    //	
    //	//Start parsing the XML file.
    //	BOOL success = [xmlParser parse];
    //    
    //	if(success) {
    //        NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    //		NSLog(@"No Errors");
    //		if (!appDelegate.objBprofile.questionone) {
    //			txtQuestionOne.hidden=YES;
    //            imgQueone.hidden=YES;
    //			appDelegate.objBprofile.questionone=nil;
    //			appDelegate.objBprofile.questionone=@"";
    //		}
    //		else {
    //            txtQuestionOne.hidden=NO;
    //            imgQueone.hidden=NO;
    ////			[txtQuestionOne setPlaceholder:appDelegate.objBprofile.questionone];
    //            [prefs setObject:appDelegate.objBprofile.questionone forKey:@"Q1"];
    //		}
    //		if (!appDelegate.objBprofile.questiontwo) {
    //			txtQuestionTwo.hidden=YES;
    //            imgQueTwo.hidden=YES;
    //			appDelegate.objBprofile.questiontwo=nil;
    //			appDelegate.objBprofile.questiontwo=@"";
    //        }
    //		else {
    //            txtQuestionTwo.hidden=NO;
    //            imgQueTwo.hidden=NO;
    ////			[txtQuestionTwo setPlaceholder:appDelegate.objBprofile.questiontwo];
    //            [prefs setObject:appDelegate.objBprofile.questiontwo forKey:@"Q2"];
    //		}
    //		if(!appDelegate.objBprofile.questionthree)
    //		{
    //			txtQuestionThree.hidden=YES;
    //			imgQueThree.hidden=YES;            
    //            appDelegate.objBprofile.questionthree=nil;
    //			appDelegate.objBprofile.questionthree=@"";
    //		}
    //		else {
    //            txtQuestionThree.hidden=NO;
    //			imgQueThree.hidden=NO;            
    //            [prefs setObject:appDelegate.objBprofile.questionthree forKey:@"Q3"];
    ////			[txtQuestionThree setPlaceholder:appDelegate.objBprofile.questionthree];
    //		}
    //        [prefs synchronize];
    //        [appDelegate apiCategory];
    //		
    //	}
    //	
    //	else
    //	{
    //		NSLog(@"Error Error Error!!! Application Validation .");
    //
    //	}
    // 
    //	[pool release];
    
    
    switch ([appDelegate.questions count]) {
            
        case 1:
            txtQuestionOne.hidden=NO;
            txtQuestionTwo.hidden=YES;
            txtQuestionThree.hidden=YES;
            
            imgQueone.hidden=NO;
            imgQueTwo.hidden=YES;
            imgQueThree.hidden=YES;
            
            [txtQuestionOne setPlaceholder:[[appDelegate.questions objectAtIndex:0] objectForKey:@"question"]];
            
            if ([[[appDelegate.questions objectAtIndex:0] objectForKey:@"permission"] isEqualToString:@"1"]) {
                
                ratingButton1.hidden=NO;
                
                ratingLabel1.hidden=NO;
            }
            
            break;
            
        case 2:
            txtQuestionOne.hidden=NO;
            txtQuestionTwo.hidden=NO;
            txtQuestionThree.hidden=YES;
            
            imgQueone.hidden=NO;
            imgQueTwo.hidden=NO;
            imgQueThree.hidden=YES;
            
            [txtQuestionOne setPlaceholder:[[appDelegate.questions objectAtIndex:0] objectForKey:@"question"]];
            
            if ([[[appDelegate.questions objectAtIndex:0] objectForKey:@"permission"] isEqualToString:@"1"]) {
                
                ratingButton1.hidden=NO;
                
                ratingLabel1.hidden=NO;
            }
            
            
            [txtQuestionTwo setPlaceholder:[[appDelegate.questions objectAtIndex:1] objectForKey:@"question"]];
            
            if ([[[appDelegate.questions objectAtIndex:1] objectForKey:@"permission"] isEqualToString:@"1"]) {
                
                ratingButton2.hidden=NO;
                
                ratingLabel2.hidden=NO;
            }
            
            
            break;
            
        case 3:
            txtQuestionOne.hidden=NO;
            txtQuestionTwo.hidden=NO;
            txtQuestionThree.hidden=NO;
            
            imgQueone.hidden=NO;
            imgQueTwo.hidden=NO;
            imgQueThree.hidden=NO;
            
            [txtQuestionOne setPlaceholder:[[appDelegate.questions objectAtIndex:0] objectForKey:@"question"]];
            
            if ([[[appDelegate.questions objectAtIndex:0] objectForKey:@"permission"] isEqualToString:@"1"]) {
                
                ratingButton1.hidden=NO;
                
                ratingLabel1.hidden=NO;
            }
            
            
            [txtQuestionTwo setPlaceholder:[[appDelegate.questions objectAtIndex:1] objectForKey:@"question"]];
            
            if ([[[appDelegate.questions objectAtIndex:1] objectForKey:@"permission"] isEqualToString:@"1"]) {
                
                ratingButton2.hidden=NO;
                
                ratingLabel2.hidden=NO;
            }
            
            
            [txtQuestionThree setPlaceholder:[[appDelegate.questions objectAtIndex:2] objectForKey:@"question"]];
            
            if ([[[appDelegate.questions objectAtIndex:2] objectForKey:@"permission"] isEqualToString:@"1"]) {
                
                ratingButton3.hidden=NO;
                
                ratingLabel3.hidden=NO;
            }
            
            break;
            
        default:
            
            txtQuestionOne.hidden=YES;
            txtQuestionTwo.hidden=YES;
            txtQuestionThree.hidden=YES;
            
            break;
    }
    
}

-(IBAction)showRatings:(id)sender{
    
    touchflag=1;
    [self RescheduleTimer];
    [Name resignFirstResponder];
	[Email resignFirstResponder];
	[txtCityCode resignFirstResponder];
	[txtcategory resignFirstResponder];
	[txtTitle resignFirstResponder];
	[txtQuestionOne resignFirstResponder];
	[txtQuestionTwo resignFirstResponder];
	[txtQuestionThree resignFirstResponder];
    [txtsubcategory resignFirstResponder];
    
    UIButton *b=(UIButton*)sender;
    
    RatingPopViewController *objPopview = [[RatingPopViewController alloc]initWithNibName:@"RatingPopViewController" bundle:nil]; 
    
    objPopview.index=b.tag;
    
    switch (b.tag) {
        case 1:
            objPopview.QuestionText=appDelegate.answerOne;
            objPopview.checkedCell=ratingLabel1.text;
            break;
            
        case 2:
            objPopview.QuestionText=appDelegate.answerTwo;
            objPopview.checkedCell=ratingLabel2.text;
            break;
            
        case 3:
            objPopview.QuestionText=appDelegate.answerthree;
            objPopview.checkedCell=ratingLabel3.text;
            break;
            
        default:
            break;
    }
    
    objPopview.delegate=self;
    
    popSweepStakes =[[UIPopoverController alloc]initWithContentViewController:objPopview];
    
    [popSweepStakes presentPopoverFromRect:b.frame inView:ViewPost permittedArrowDirections:UIPopoverArrowDirectionRight animated:YES];
    
    [popSweepStakes setPopoverContentSize:CGSizeMake(300, 400)];
    
}

-(void) didSelectRating{
    
    touchflag=0;
    
    [popSweepStakes dismissPopoverAnimated:YES];
    
    if (appDelegate.rating1Bool==YES) {
        
        ratingLabel1.text=appDelegate.rating1;
    }
    
    if (appDelegate.rating2Bool==YES) {
        
        ratingLabel2.text=appDelegate.rating2;
    }
    
    if (appDelegate.rating3Bool==YES) {
        
        ratingLabel3.text=appDelegate.rating3;
    }
    [self RescheduleTimer];
    
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

-(IBAction)Home
{    
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *objwrite = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:objwrite animated:YES];
    [objwrite release];
    
}


-(IBAction) CategoryListClick
{
    if (touchflag==1) {
        
        NSLog(@"CategoryListClick");
        self.checkFLAG=TRUE;
        [Name resignFirstResponder];
        [Email resignFirstResponder];
        [txtCityCode resignFirstResponder];
        [txtcategory resignFirstResponder];
        [txtTitle resignFirstResponder];
        [txtQuestionOne resignFirstResponder];
        [txtQuestionTwo resignFirstResponder];
        [txtQuestionThree resignFirstResponder];
        [txtsubcategory resignFirstResponder];
        
        CategoryPopViewController *objPopview = [[CategoryPopViewController alloc]initWithNibName:@"CategoryPopViewController" bundle:nil];
        
        objPopview.delegate = self;
        
        popView =[[UIPopoverController alloc]initWithContentViewController:objPopview];
        
        [popView presentPopoverFromRect:categoryListImage.frame inView:ViewPost permittedArrowDirections:0 animated:NO];
        
        [popView setPopoverContentSize:CGSizeMake(376, 260)];
    }
}

-(void) didSelect
{
    if (appDelegate.category==YES) {
        
        txtcategory.text = appDelegate.strCategoryname;
    }
    
	else{
        
        txtsubcategory.text=appDelegate.strCategoryname;
    }
    
    for (UIImageView *img in ViewPost.subviews)
    {
        if ([img isKindOfClass:[UIImageView class]]) 
        {
            if (img.tag==txtcategory.tag) 
            {
                [img setImage:[UIImage imageNamed:@"textbox.png"]];
            }
        }        
    }
    
	[popView dismissPopoverAnimated:YES];
}

//- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
//{
//	return 1;
//}
//
//- (NSInteger)pickerView:(UIPickerView *)thePickerView numberOfRowsInComponent:(NSInteger)component {	
//	
//    return 5;
//}
//
//- (UIView *)pickerView:(UIPickerView *)pickerView viewForRow:(NSInteger)row forComponent:(NSInteger)component reusingView:(UIView *)view
//{
//    
//    UIView *tmpView = [[UIView alloc] initWithFrame:CGRectMake(5, 0, 278, 46)];
//    
//	if (row==0) {
//        
//		UIImage *img = [UIImage imageNamed:@"5star.png"];
//        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
//        temp.frame = CGRectMake(0, 4, 140, 29);
//		
//		
//		[tmpView insertSubview:temp atIndex:0];		
//	}
//    
//	if (row==1) {
//		UIImage *img = [UIImage imageNamed:@"4star.png"];
//        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
//        temp.frame = CGRectMake(0, 4, 114, 29);
//		
//		[tmpView insertSubview:temp atIndex:0];
//		
//	}
//    
//	if (row==2) {
//        
//		UIImage *img = [UIImage imageNamed:@"3star.png"];
//        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
//        temp.frame = CGRectMake(0, 4, 84, 29);
//				
//		[tmpView insertSubview:temp atIndex:0];
//		
//	}
//    
//	if (row==3) {
//		UIImage *img = [UIImage imageNamed:@"2star.png"];
//        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
//        temp.frame = CGRectMake(0, 4, 54, 29);
//		
//        [tmpView insertSubview:temp atIndex:0];
//	}
//    
//	if (row==4) {
//		UIImage *img = [UIImage imageNamed:@"1star.png"];
//        UIImageView *temp = [[UIImageView alloc] initWithImage:img];        
//        temp.frame = CGRectMake(0, 4, 29, 29);
//		
//		[tmpView insertSubview:temp atIndex:0];
//		
//	}
//    
//    return tmpView;
//}
//
//- (void)pickerView:(UIPickerView *)thePickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component
//{	
//	[picFilter setHidden:YES];
//}

-(IBAction) DoneButtonClick
{
	picCategory.hidden = YES;
	tlbCategory.hidden = YES;
	flgCatPic=NO;
}

-(IBAction)WriteReviewClcik
{
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    
    NSString * q1=[prefs objectForKey:@"Q1"];
    NSString * q2=[prefs objectForKey:@"Q2"];
    NSString * q3=[prefs objectForKey:@"Q3"];
    
	[txtcategory resignFirstResponder];
	[txtTitle resignFirstResponder];
	[txtDescription resignFirstResponder];
	[Email resignFirstResponder];
	[Name resignFirstResponder];
	[txtCityCode resignFirstResponder];
	[txtQuestionOne resignFirstResponder];
	[txtQuestionTwo resignFirstResponder];
	[txtQuestionThree resignFirstResponder];
    
	picCategory.hidden=YES;
	
	if (!flgRating) 
    {
		appDelegate.Rating = 5;
	}
	NSLog(@"appDelegate.remoteHostStatus %d",appDelegate.remoteHostStatus);
	if(appDelegate.remoteHostStatus==0)
	{
        // --------- INTERNET Connection not available -------
        
        [self checkValidation];
        
        if (valid) {
            valid=FALSE;    
            appDelegate.Data_desc  = [[NSString alloc]init];	
            appDelegate.Data_desc = txtDescription.text;
            appDelegate.Data_Title = txtTitle.text;
            NSDateFormatter *format = [[NSDateFormatter alloc] init];
            [format setDateFormat:@"yyyy-MM-dd HH:mm"];
            NSDate *now = [[NSDate alloc] init];
            
            NSString *dateString = [format stringFromDate:now];
            NSLog(@"dateString: %@",dateString);
            
            appDelegate.Data_name=Name.text;			
            appDelegate.Data_email=Email.text;			
            appDelegate.CityName=txtCityCode.text;	
            appDelegate.Data_bdate=self.stringbirthdate;			
            appDelegate.Data_date = dateString;
            
            appDelegate.Data_useid = appDelegate.userId;
            appDelegate.Data_Rstatus = appDelegate.poststatus;
            NSLog(@"appDelegate.Data_Rstatus>%@",appDelegate.Data_Rstatus);
            if (appDelegate.objBprofile.questionone !=nil) {
                
                appDelegate.FirstAns = [NSString stringWithFormat:@"%@:%@",appDelegate.objBprofile.questionone,txtQuestionOne.text];
                NSLog(@"appDelegate.FirstAns:%@",appDelegate.FirstAns);
                appDelegate.SecondAns = [NSString stringWithFormat:@"%@:%@",appDelegate.objBprofile.questiontwo,txtQuestionTwo.text];
                appDelegate.ThridAns = [NSString stringWithFormat:@"%@:%@",appDelegate.objBprofile.questionthree,txtQuestionThree.text];
                
            }
            else{
                NSLog(@" NI L L L L L ");
                
                appDelegate.FirstAns = [NSString stringWithFormat:@"%@:%@",q1,txtQuestionOne.text];
                NSLog(@"appDelegate.FirstAns:%@",appDelegate.FirstAns);
                appDelegate.SecondAns = [NSString stringWithFormat:@"%@:%@",q2,txtQuestionTwo.text];
                appDelegate.ThridAns = [NSString stringWithFormat:@"%@:%@",q3,txtQuestionThree.text];
            }
            
            NSLog(@"appDelegate.date >%@",appDelegate.Data_date);
            NSLog(@"appDelegate.Data_Rstatus>%@",appDelegate.Data_Rstatus);
            appDelegate.category_text=txtcategory.text;
            //canceldelay
            //cancel the previous  ReidrectToHome
            [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(ReidrectToHome) object:nil]; 
            Term_para *Termsp = [[Term_para alloc]init];
            [self.navigationController pushViewController:Termsp animated:YES];
            [Termsp release];
        }
	}
    
	else
	{	        
        [self checkValidation];
        if (valid) {
            valid=FALSE;
            
            appDelegate.Data_desc = txtDescription.text;
			appDelegate.Data_Title = txtTitle.text;
			NSDateFormatter *format = [[NSDateFormatter alloc] init];
			[format setDateFormat:@"yyyy-MM-dd HH:mm"];
			NSDate *now = [[NSDate alloc] init];
			
			NSString *dateString = [format stringFromDate:now];
			NSLog(@"dateString: %@",dateString);
			
			appDelegate.Data_name=Name.text;			
			appDelegate.Data_email=Email.text;			
			appDelegate.CityName=txtCityCode.text;	
			appDelegate.Data_bdate=self.stringbirthdate;			
			appDelegate.Data_date = dateString;
			appDelegate.Data_useid = appDelegate.userId;
            NSLog(@"appDelegate.Data_useid>%@",appDelegate.userId);
			appDelegate.Data_Rstatus = appDelegate.poststatus;
            NSLog(@"appDelegate.Data_Rstatus>%@",appDelegate.Data_Rstatus);
            
			appDelegate.FirstAns = [NSString stringWithFormat:@"%@:%@",q1,txtQuestionOne.text];   
            NSLog(@"FirstAns:->%@",appDelegate.FirstAns);
			appDelegate.SecondAns = [NSString stringWithFormat:@"%@:%@",q2,txtQuestionTwo.text];
			appDelegate.ThridAns = [NSString stringWithFormat:@"%@:%@",q3,txtQuestionThree.text];
			
			//appDelegate.strCategoryname;
            //canceldelay
            //cancel the previous  ReidrectToHome
            [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(ReidrectToHome) object:nil]; 
			Term_para *Termsp = [[Term_para alloc]init];
            [self.navigationController pushViewController:Termsp animated:YES];
            [Termsp release];
        }	
    }
}


-(void)checkValidation
{
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc]init];
    emailString = Email.text; 
	NSString *emailReg = @"[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}";
	emailTest = [NSPredicate predicateWithFormat:@"SELF MATCHES %@", emailReg];
	
    if([[Name.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || Name.text==nil)
    {
        alertname = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter Your Name" delegate:self 
                                     cancelButtonTitle:@"OK" 
                                     otherButtonTitles:nil];
        [alertname show];
        [alertname release];
        return;
    }
    
    else if([[Email.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || Email.text==nil)
    {
        alertemail = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter Your Email " delegate:self 
                                      cancelButtonTitle:@"OK" 
                                      otherButtonTitles:nil];
        [alertemail show];
        [alertemail release];
        return;
        
    }
    else if (([emailTest evaluateWithObject:emailString] != YES))// || [emailString isEqualToString:@""])
    {
        alertemailtest = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"Please Enter Valid Email (abc@example.com)" delegate:self
                                          cancelButtonTitle:@"OK" otherButtonTitles:nil];
        
        [alertemailtest show];
        [alertemailtest release];
        return;
    } 
    else if([[txtCityCode.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtCityCode.text==nil)
    {
        alertcity = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter City" delegate:self 
                                     cancelButtonTitle:@"OK" 
                                     otherButtonTitles:nil];
        [alertcity show];
        [alertcity release];
        return;
    }
    else if ([appDelegate.strCategoryname isEqualToString:@""] || appDelegate.strCategoryname==nil) 
    {
        AlertCategory = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Select Review Category" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertCategory show];
        [AlertCategory release];
        return;
    }
    else if ([[txtTitle.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""]|| txtTitle.text == nil) 
    {    
        AlertReviewtitle = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please enter review title" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertReviewtitle show];
        [AlertReviewtitle release];
        return;
    }
    else if (appDelegate.Rating == 0 && _rateView.rating==0) 
    {
        UIAlertView *alertError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Rate this Review out of 5 Stars." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [alertError show];
        [alertError release];
        return;
    }
    
    else if(![appDelegate.objBprofile.questionone isEqualToString:@""] && [txtQuestionOne.text isEqualToString:appDelegate.objBprofile.questionone] && ![txtQuestionOne isHidden])
    {
        NSString *apnd1a = appDelegate.objBprofile.questionone;
        
        NSString *msg1a = @"Please Select Answer to the Question : ";
        
        NSString *msglq1a = [msg1a stringByAppendingString:apnd1a]; 
        AlertQueone = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:msglq1a delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueone show];
        [AlertQueone release];
        return;
    }
    else if(![appDelegate.objBprofile.questionone isEqualToString:@""] &&[[txtQuestionOne.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] && ![txtQuestionOne isHidden]) 
    {
        NSString *apnd1 = appDelegate.objBprofile.questionone;
        
        NSString *msg1 = @"Please Select Answer to the Question : ";
        
        NSString *msglq1 = [msg1 stringByAppendingString:apnd1];
        
        AlertQueone = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:msglq1 delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueone show];
        [AlertQueone release];	
        return;
    }
    else if([[ratingLabel1.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@"0"]&& ![txtQuestionOne isHidden])
        
    {
        NSString *apnd1 = appDelegate.objBprofile.questionone;
        
        NSString *msg1 = @"Please Select Rating to the Question : ";
        
        NSString * msglbl1 = [msg1 stringByAppendingString:apnd1];
        
        UIAlertView *alert1= [[UIAlertView alloc] initWithTitle:@"Review Frog" message:msglbl1  delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [alert1 show];
        [alert1 release];
        return;
    }
    
    else if(![appDelegate.objBprofile.questiontwo isEqualToString:@""] && [txtQuestionTwo.text isEqualToString:appDelegate.objBprofile.questiontwo] && ![txtQuestionTwo isHidden])
    {
        NSString *apnd2a = appDelegate.objBprofile.questiontwo;
        
        NSString *msg2a = @"Please Select Answer to the Question : ";
        
        NSString *msglq2a = [msg2a stringByAppendingString:apnd2a];
        
        AlertQueTwo= [[UIAlertView alloc] initWithTitle:@"Review Frog" message: msglq2a
                                               delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueTwo show];
        [AlertQueTwo release];	
		return;
    }
    else if(![appDelegate.objBprofile.questiontwo isEqualToString:@""] &&[[txtQuestionTwo.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] && ![txtQuestionTwo isHidden]) 
    {
        NSString *apnd2 = appDelegate.objBprofile.questiontwo;
        
        NSString *msg2 = @"Please Select Answer to the Question : ";
        
        NSString *msglq2 = [msg2 stringByAppendingString:apnd2];
        
        AlertQueTwo = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:
                                                msglq2 delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueTwo show];
        [AlertQueTwo release];	
        return;
    }
    
    else if([[ratingLabel2.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@"0"]&& ![txtQuestionTwo isHidden])
        
    {
        NSString *apnd2 = appDelegate.objBprofile.questiontwo;
        
        NSString *msg2 = @"Please Select Rating to the Question : ";
        
        NSString * msglbl2 = [msg2 stringByAppendingString:apnd2];
        
        UIAlertView *alert2= [[UIAlertView alloc] initWithTitle:@"Review Frog" message:msglbl2  delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [alert2 show];
        [alert2 release];
        return;
    }
    
    else if(![appDelegate.objBprofile.questionthree isEqualToString:@""] && [txtQuestionThree.text isEqualToString:appDelegate.objBprofile.questionthree] && ![txtQuestionThree isHidden])
    {
        NSString *apnd3a = appDelegate.objBprofile.questionthree;
        
        NSString *msg3a = @"Please Select Answer to the Question : ";
        
        NSString * msglq3a = [msg3a stringByAppendingString:apnd3a];
        
        AlertQueThree = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:
                                                  msglq3a delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueThree show];
        [AlertQueThree release];
        return;
    }
    else if(![appDelegate.objBprofile.questionthree isEqualToString:@""] &&[[txtQuestionThree.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] &&![txtQuestionThree isHidden]) {
        
        NSString *apnd3 = appDelegate.objBprofile.questionthree;
        
        NSString *msg3 = @"Please Select Answer to the Question : ";
        
        NSString * msglq3 = [msg3 stringByAppendingString:apnd3];

        AlertQueThree = [[UIAlertView alloc] initWithTitle:@"Review Frog" message: msglq3
                                                  delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueThree show];
        [AlertQueThree release];	
        return;
    }
    else if(flgRating==FALSE) {         
        
        UIAlertView *alrstar = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please confirm how many stars you would like to give by sliding your finger on the stars."delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
        [alrstar show];
        [alrstar release];	
        return;
    }
    
    else if([txtDescription.text isEqualToString:@"Click here to tell us about your experience"]) 
    {
        AlertDescription = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please enter description." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertDescription show];
        [AlertDescription release];
        return;
        
    }
    else if([[txtDescription.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""]|| txtDescription.text == nil) {
        
        AlertDescription = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please enter description." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertDescription show];
        [AlertDescription release];
        return;
        
    }
    
    else if([[ratingLabel2.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@"0"]&& ![txtQuestionThree isHidden])
        
    {
        NSString *apnd3 = appDelegate.objBprofile.questionthree;
        
        NSString *msg3 = @"Please Select Rating to the Question : ";
        
        NSString * msglbl3 = [msg3 stringByAppendingString:apnd3];
        
        UIAlertView *alert3 = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:msglbl3  delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [alert3 show];
        [alert3 release];
        return;
    }

    
    valid=TRUE;
    [pool release];
}
-(IBAction) VideoReviewClcik
{ 
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString * q1=[prefs objectForKey:@"Q1"];
    NSString * q2=[prefs objectForKey:@"Q2"];
    NSString * q3=[prefs objectForKey:@"Q3"];
 	[txtcategory resignFirstResponder];
	[txtTitle resignFirstResponder];
	[txtDescription resignFirstResponder];
	[txtQuestionOne resignFirstResponder];
	[txtQuestionTwo resignFirstResponder];
	[txtQuestionThree resignFirstResponder];
	emailString = Email.text; 
	picCategory.hidden=YES;
	NSString *emailReg = @"[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}";
	
	emailTest = [NSPredicate predicateWithFormat:@"SELF MATCHES %@", emailReg];
	
	if (!flgRating) 
    {
		
		appDelegate.Rating = 5;
	}
    
    if(appDelegate.remoteHostStatus!=0)
	{
        
        //------ internet  availble in video ----------- 
        
        [self validVideoREview];
		if (valid) {
            valid=FALSE;
            
            NSDateFormatter *format = [[NSDateFormatter alloc] init];
            [format setDateFormat:@"yyyy-MM-dd HH:mm"];
            NSDate *now = [[NSDate alloc] init];
            
            NSString *dateString = [format stringFromDate:now];
            NSLog(@"dateString: %@",dateString);
            
            appDelegate.Data_name=Name.text;			
            appDelegate.Data_email=Email.text;			
            appDelegate.CityName=txtCityCode.text;	
            appDelegate.Data_bdate=self.stringbirthdate;			
            appDelegate.Data_date = dateString;
            appDelegate.Data_useid = appDelegate.userId;
            appDelegate.Data_Rstatus = appDelegate.poststatus;
            NSLog(@"appDelegate.Data_useid>%@",appDelegate.userId);
            
            appDelegate.FirstAns = [NSString stringWithFormat:@"%@:%@",q1,txtQuestionOne.text];
            appDelegate.SecondAns = [NSString stringWithFormat:@"%@:%@",q2,txtQuestionTwo.text];
            appDelegate.ThridAns = [NSString stringWithFormat:@"%@:%@",q3,txtQuestionThree.text];
            
            NSString *strVtitle = [txtTitle.text stringByReplacingOccurrencesOfString:@" " withString:@"_"];
            NSLog(@"Remove String:-%@",strVtitle);
            appDelegate.Data_Title = strVtitle;
            
            NSLog(@"appDelegate.Data_name>%@",appDelegate.Data_name);
            NSLog(@"appDelegate.Data_email>%@",appDelegate.Data_email);
            NSLog(@"appDelegate.CityName>%@",appDelegate.CityName);
            
            NSLog(@"appDelegate.Data_date>%@",stringbirthdate);
            NSLog(@"appDelegate.Data_Rstatus>%@",appDelegate.Data_Rstatus);
            NSLog(@"appDelegate.poststatus>%@",appDelegate.poststatus);
            NSLog(@"appDelegate.Data_Rstatus>%@",appDelegate.Data_Rstatus);
            NSLog(@"appDelegate.Data_useid>%@",appDelegate.userId);
            NSLog(@"appDelegate.SecondAns %@", appDelegate.SecondAns);
            NSLog(@"appDelegate.FirstAns %@", appDelegate.FirstAns);
            NSLog(@"appDelegate.SecondAns %@", appDelegate.SecondAns);
            NSLog(@"appDelegate.ThridAns %@",appDelegate.ThridAns);
           
            
            
            //canceldelay
            //cancel the previous  ReidrectToHome
            [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(ReidrectToHome) object:nil]; 
            [self startCamera];
        }
        
    }
    else{
        
        // INTERNET not availble-----------------------------               
        [self validVideoREview];
        if (valid) {
            valid=FALSE;
            NSDateFormatter *format = [[NSDateFormatter alloc] init];
            [format setDateFormat:@"yyyy-MM-dd HH:mm"];
            NSDate *now = [[NSDate alloc] init];
            
            NSString *dateString = [format stringFromDate:now];
            NSLog(@"dateString: %@",dateString);
            
            appDelegate.Data_name=Name.text;			
            appDelegate.Data_email=Email.text;			
            appDelegate.CityName=txtCityCode.text;	
            appDelegate.Data_bdate=self.stringbirthdate;			
            appDelegate.Data_date = dateString;
            appDelegate.Data_useid = appDelegate.userId;
            appDelegate.Data_Rstatus = appDelegate.poststatus;
            NSLog(@"appDelegate.Data_useid>%@",appDelegate.userId);
            
            appDelegate.FirstAns = [NSString stringWithFormat:@"%@:%@",q1,txtQuestionOne.text];
            appDelegate.SecondAns = [NSString stringWithFormat:@"%@:%@",q2,txtQuestionTwo.text];
            appDelegate.ThridAns = [NSString stringWithFormat:@"%@:%@",q3,txtQuestionThree.text];
            
            NSString *strVtitle = [txtTitle.text stringByReplacingOccurrencesOfString:@" " withString:@"_"];
            NSLog(@"Remove String:-%@",strVtitle);
            appDelegate.Data_Title = strVtitle;
            
            NSLog(@"appDelegate.ThridAns %@",appDelegate.ThridAns);
            
            
            //canceldelay
            //cancel the previous  ReidrectToHome
            [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(ReidrectToHome) object:nil]; 
            
            [self startCamera];
        } 
    }
}
-(void)validVideoREview
{
    if([[Name.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || Name.text==nil)
	{
		alertname = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter Your Name" delegate:self 
									 cancelButtonTitle:@"OK" 
									 otherButtonTitles:nil];
		[alertname show];
		[alertname release];
        return;
	}
	else if([[Email.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || Email.text==nil)
	{
		alertemail = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter Your Email " delegate:self 
									  cancelButtonTitle:@"OK" 
									  otherButtonTitles:nil];
		[alertemail show];
		[alertemail release];	
        return;
	}
	else if (([emailTest evaluateWithObject:emailString] != YES))// || [emailString isEqualToString:@""])
	{
		alertemailtest = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"Please Enter Valid Email (abc@example.com)" delegate:self
										  cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[alertemailtest show];
		[alertemailtest release];
        return;
	} 
	else if([[txtCityCode.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] || txtCityCode.text==nil)
	{
		alertcity = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Enter City" delegate:self 
									 cancelButtonTitle:@"OK" 
									 otherButtonTitles:nil];
		[alertcity show];
		[alertcity release];
        return;
	}
	else if ([appDelegate.strCategoryname isEqualToString:@""] || appDelegate.strCategoryname==nil) 
	{
		AlertCategory = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Select Review Category" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[AlertCategory show];
		[AlertCategory release];
        return;
	}
	else if ([txtTitle.text isEqualToString:@""]||txtTitle.text == nil ) 
    {
		AlertReviewtitle = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please enter review title." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[AlertReviewtitle show];
		[AlertReviewtitle release];	
        return;
	}
    else if(![appDelegate.objBprofile.questionone isEqualToString:@""] && [txtQuestionOne.text isEqualToString:appDelegate.objBprofile.questionone] && ![txtQuestionOne isHidden])
    {
        AlertQueone = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Give answer of question." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueone show];
        [AlertQueone release];
        return;
    }
    else if(![appDelegate.objBprofile.questionone isEqualToString:@""] &&[[txtQuestionOne.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] && ![txtQuestionOne isHidden]) 
    {
        
        AlertQueone = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Give answer of question." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueone show];
        [AlertQueone release];	
        return;
    }
    
    else if(![appDelegate.objBprofile.questiontwo isEqualToString:@""] && [txtQuestionTwo.text isEqualToString:appDelegate.objBprofile.questiontwo] && ![txtQuestionTwo isHidden])
    {
        AlertQueTwo= [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Give answer of question." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueTwo show];
        [AlertQueTwo release];	
		return;
    }
    else if(![appDelegate.objBprofile.questiontwo isEqualToString:@""] &&[[txtQuestionTwo.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] && ![txtQuestionTwo isHidden]) 
    {
        
        AlertQueTwo = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Give answer of question." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueTwo show];
        [AlertQueTwo release];	
        return;
    }
    
    else if(![appDelegate.objBprofile.questionthree isEqualToString:@""] && [txtQuestionThree.text isEqualToString:appDelegate.objBprofile.questionthree] && ![txtQuestionThree isHidden])
    {
        AlertQueThree = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Give answer of question." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueThree show];
        [AlertQueThree release];
        return;
    }
    else if(![appDelegate.objBprofile.questionthree isEqualToString:@""] &&[[txtQuestionThree.text stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] isEqualToString:@""] &&![txtQuestionThree isHidden]) {
        
        AlertQueThree = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please Give answer of question." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
        [AlertQueThree show];
        [AlertQueThree release];	
        return;
    }
    
	else if (appDelegate.Rating == 0 && _rateView.rating == 0) 
    {
		UIAlertView *alertError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Please rate this review out of 5 Stars." delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil];
		[alertError show];
		[alertError release];
        return;
	}
    else if(flgRating==FALSE) {
		UIAlertView *alrstar = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Please confirm how many stars you would like to give by sliding your finger on the stars."delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];
		[alrstar show];
		[alrstar release];	
        return;
	}
    valid=TRUE;
}
-(void) startCamera
{
    
	if ([UIImagePickerController isSourceTypeAvailable:
		 UIImagePickerControllerSourceTypeCamera])
	{
		imagePicker =[[UIImagePickerController alloc] init];
		imagePicker.delegate = self;        
        imagePicker.navigationBarHidden=YES;
        
		imagePicker.sourceType = UIImagePickerControllerSourceTypeCamera;
		imagePicker.cameraDevice = UIImagePickerControllerCameraDeviceFront;
		imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *) kUTTypeMovie,nil];
		imagePicker.allowsEditing = NO;
		
		imagePicker.cameraCaptureMode = UIImagePickerControllerCameraCaptureModeVideo;
		
		imagePicker.videoMaximumDuration = 30;
        
        imagePicker.wantsFullScreenLayout = YES;
        UIImage *crosshair = [UIImage imageNamed:@"start_re.png"];
        crosshairView = [[UIImageView alloc] 
                         initWithImage:crosshair];
		crosshairView.frame = CGRectMake(320, 630, 384, 74);
		[imagePicker.view addSubview:crosshairView];
        
        UIImage *publish = [UIImage imageNamed:@"after_re.png"];
        publishImage = [[UIImageView alloc] 
                        initWithImage:publish];
		publishImage.frame = CGRectMake(780, 630, 222, 74);
		[imagePicker.view addSubview:publishImage];
        video = YES;
        
		[self presentModalViewController:imagePicker animated:YES];
        
		[imagePicker release];
	}
}
-(void)imagePickerController:(UIImagePickerController *)picker
didFinishPickingMediaWithInfo:(NSDictionary *)info
{
    NSLog(@"~~~~didFinishPickingMediaWithInfo~~~~~");
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

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex 
{
    
	if (alertView == alertname) {
		[Name becomeFirstResponder];
	}
	else if (alertView == alertemail) {
		[Email becomeFirstResponder];
	}
	else if (alertView == alertemailtest) {
		[Email becomeFirstResponder];
	}
	else if (alertView == alertcity) {
		[txtCityCode becomeFirstResponder];
	}
	else if (alertView == AlertQueone) {
		[txtQuestionOne becomeFirstResponder];
	}
	else if (alertView == AlertQueTwo) {
		[txtQuestionTwo becomeFirstResponder];
	}
	else if (alertView == AlertQueThree) {
		[txtQuestionThree becomeFirstResponder];
	}
	else if (alertView == AlertReviewtitle) {
		[txtTitle becomeFirstResponder];
	}
	else if (alertView == AlertDescription) {
        
		[txtDescription becomeFirstResponder];
	}
    else if (alertView == AlertCategory){
        
        [Name resignFirstResponder];
        [Email resignFirstResponder];
        [txtCityCode resignFirstResponder];
        
        [txtTitle resignFirstResponder];
        [txtQuestionOne resignFirstResponder];
        [txtQuestionTwo resignFirstResponder];
        [txtQuestionThree resignFirstResponder];
        
        appDelegate.category=YES;
        
        [txtcategory becomeFirstResponder];
        
        // [self CategoryListClick];
    }
    ///writecode
    [self RescheduleTimer];
}

-(void) ReidrectToHome
{
    [self.navigationController popViewControllerAnimated:YES];
}

-(IBAction)btnBackClick
{
    //    WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    //    [self.navigationController pushViewController:awriteorRecode animated:YES];
    //    [awriteorRecode release];
    //cancel the previous  ReidrectToHome
    [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(ReidrectToHome) object:nil]; 
    [self.navigationController popViewControllerAnimated:YES];
}

// Override to allow orientations other than the default portrait orientation.
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation 
{
    NSLog(@"shouldAutorotateToInterfaceOrientation");
	// Overriden to allow any orientation.
	if(interfaceOrientation == UIInterfaceOrientationLandscapeRight || interfaceOrientation == UIInterfaceOrientationLandscapeLeft)
		return YES;    
    else if(interfaceOrientation == UIInterfaceOrientationPortrait)
    {
        NSLog(@"1 Retun No");
		return NO;
    }
    else
    {
        return NO;
    }
    
    NSLog(@"shouldAutorotateToInterfaceOrientation");
}

- (int)deviceOrientationDidChange
{    
    UIDeviceOrientation deviceOrientation = [[UIDevice currentDevice] orientation];
    int val;
    if (deviceOrientation == UIDeviceOrientationPortrait){
        //   orientation = AVCaptureVideoOrientationPortrait;
        val=1;
        NSLog(@"AVCaptureVideoOrientationPortrait");
    }
    else if (deviceOrientation == UIDeviceOrientationPortraitUpsideDown){
        // orientation = AVCaptureVideoOrientationPortraitUpsideDown;
        val=2;
        NSLog(@"AVCaptureVideoOrientationPortraitUpsideDown");
    }
    else if (deviceOrientation == UIDeviceOrientationLandscapeLeft){
        //  orientation = AVCaptureVideoOrientationLandscapeRight;
        val=3;
        NSLog(@"AVCaptureVideoOrientationLandscapeRight");
        
    }
    else if (deviceOrientation == UIDeviceOrientationLandscapeRight){
        // orientation = AVCaptureVideoOrientationLandscapeLeft;
        val=4;
        NSLog(@"AVCaptureVideoOrientationLandscapeLeft");
        
    }
    return val;
    
}


- (void)didReceiveMemoryWarning {
	// Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
	
}

- (void)viewDidUnload {
	// Release any retained subviews of the main view.
	// e.g. self.myOutlet = nil;
}
-(IBAction)TextFieldDoneEditing
{
    
	
}
/*
 - (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
 {
 int value = [touches count];
 NSLog(@"intvalue%d",value);
 }
 
 - (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
 {
 NSLog(@"touchesBegan");
 
 [self.view setUserInteractionEnabled:NO];
 
 [ViewPost setUserInteractionEnabled:NO];
 
 if(appDelegate.SwitchFlag ==TRUE)
 {
 NSLog(@"touchesBegan");
 
 [appDelegate resetIdleTimer];
 }
 [self RescheduleTimer];
 }
 
 - (void)touchesMoved:(NSSet *)touches withEvent:(UIEvent *)event
 {
 NSLog(@"touchesMoved ");
 }
 
 - (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event{
 
 [self.view setUserInteractionEnabled:YES];
 
 [ViewPost setUserInteractionEnabled:YES];
 
 [self RescheduleTimer];
 }
 */

- (BOOL)textFieldShouldBeginEditing:(UITextField *)textField
{	
    BOOL editing=YES;
    
    for (UIImageView *img in ViewPost.subviews)
    {   
        if ([img isKindOfClass:[UIImageView class]]) 
        {
            if (img.tag==textField.tag) 
            {
                [img setImage:[UIImage imageNamed:@"Redtextbox.png"]];;
            }
            else{   
                if (img.tag!=0) {
                    if (img.tag != 10) {
                        
                        [img setImage:[UIImage imageNamed:@"textbox.png"]];
                    }
                    else
                    {
                        [img setImage:[UIImage imageNamed:@"inputbox.png"]];
                    }
                    
                }
                
            }
        }        
        
    }
    
	if (textField==txtcategory) {
        
		[textField resignFirstResponder];
        
        appDelegate.category=YES;
        
        editing=NO;
        
        touchflag=1;
        
        [self performSelectorOnMainThread:@selector(CategoryListClick) withObject:nil waitUntilDone:YES];
		//[self CategoryListClick];
        
		return NO;
	}
    
    if (textField==txtsubcategory) {
        
        appDelegate.category=NO;
        
        editing=NO;
        
        [self performSelectorOnMainThread:@selector(CategoryListClick) withObject:nil waitUntilDone:YES];
    }
    
    if (textField==txtQuestionOne) {
        
        editing=NO;
        
        index=0;
        
        touchflag=1;
        
        [self performSelectorOnMainThread:@selector(loadAnswers) withObject:nil waitUntilDone:YES];
    }
    
    if (textField==txtQuestionTwo) {
        
        editing=NO;
        
        index=1;
        
        touchflag=1;
        
        [self performSelectorOnMainThread:@selector(loadAnswers) withObject:nil waitUntilDone:YES];
    }
    
    if (textField==txtQuestionThree) {
        
        editing=NO;
        
        index=2;
        
        touchflag=1;
        
        [self performSelectorOnMainThread:@selector(loadAnswers) withObject:nil waitUntilDone:YES];
    }
    ///writecode
	return editing;
}

-(void)loadAnswers{
    
    if (touchflag==1) {
        
        [Name resignFirstResponder];
        [Email resignFirstResponder];
        [txtCityCode resignFirstResponder];
        [txtcategory resignFirstResponder];
        [txtTitle resignFirstResponder];
        [txtQuestionOne resignFirstResponder];
        [txtQuestionTwo resignFirstResponder];
        [txtQuestionThree resignFirstResponder];
        [txtsubcategory resignFirstResponder];
        
        SubcategoryPopViewController *objPopview = [[SubcategoryPopViewController alloc]initWithNibName:@"SubcategoryPopViewController" bundle:nil];
        
        objPopview.delegate = self;
        
        objPopview.index=index;
        
        popView =[[UIPopoverController alloc]initWithContentViewController:objPopview];
        [self RescheduleTimer];
        switch (index) {
                
            case 0:
                
                [popView presentPopoverFromRect:imgQueone.frame inView:ViewPost permittedArrowDirections:0 animated:NO];
                
                [popView setPopoverContentSize:CGSizeMake(376, 260)];
                
                break;
                
            case 1:
                
                [popView presentPopoverFromRect:imgQueTwo.frame inView:ViewPost permittedArrowDirections:0 animated:NO];
                
                [popView setPopoverContentSize:CGSizeMake(376, 260)];
                
                break;
                
            case 2:
                
                [popView presentPopoverFromRect:imgQueThree.frame inView:ViewPost permittedArrowDirections:0 animated:NO];
                
                [popView setPopoverContentSize:CGSizeMake(376, 260)];
                
                break;
                
            default:
                break;
        }
    }
}

-(void) didSelectAnswer{
    
    touchflag=0;
    
    [popView dismissPopoverAnimated:YES];
    [self RescheduleTimer];
    if (appDelegate.ansOne==YES) {
        
        txtQuestionOne.text=appDelegate.answerOne;
        
        appDelegate.ansOne=NO;
        
        questionIndex=1;
        
        if (![ratingButton1 isHidden]) {
            
            ratingButton1.enabled=YES;
            
            RatingPopViewController *objPopview = [[RatingPopViewController alloc]initWithNibName:@"RatingPopViewController" bundle:nil]; 
            
            objPopview.index=questionIndex;
            
            objPopview.checkedCell=ratingLabel1.text;
            
            objPopview.QuestionText=appDelegate.answerOne;
            
            objPopview.delegate=self;
            
            popSweepStakes =[[UIPopoverController alloc]initWithContentViewController:objPopview];
            
            [popSweepStakes presentPopoverFromRect:ratingButton1.frame inView:ViewPost permittedArrowDirections:UIPopoverArrowDirectionRight animated:YES];
            
            [popSweepStakes setPopoverContentSize:CGSizeMake(300, 400)];
            
        }
        
        return;
    }
    
    if (appDelegate.ansTwo==YES) {
        
        txtQuestionTwo.text=appDelegate.answerTwo;
        
        appDelegate.ansTwo=NO;
        
        questionIndex=2;
        
        if (![ratingButton2 isHidden]) {
            
            ratingButton2.enabled=YES;
            
            RatingPopViewController *objPopview = [[RatingPopViewController alloc]initWithNibName:@"RatingPopViewController" bundle:nil]; 
            
            objPopview.index=questionIndex;
            
            objPopview.checkedCell=ratingLabel2.text;
            
            objPopview.QuestionText=appDelegate.answerTwo;
            
            objPopview.delegate=self;
            
            popSweepStakes =[[UIPopoverController alloc]initWithContentViewController:objPopview];
            
            [popSweepStakes presentPopoverFromRect:ratingButton2.frame inView:ViewPost permittedArrowDirections:UIPopoverArrowDirectionRight animated:YES];
            
            [popSweepStakes setPopoverContentSize:CGSizeMake(300, 400)];
            
        }
        
        return;
    }
    
    if (appDelegate.ansThrre==YES) {
        
        txtQuestionThree.text=appDelegate.answerthree;
        
        appDelegate.ansThrre=NO;
        
        questionIndex=3;
        
        if (![ratingButton3 isHidden]) {
            
            ratingButton3.enabled=YES;
            
            RatingPopViewController *objPopview = [[RatingPopViewController alloc]initWithNibName:@"RatingPopViewController" bundle:nil]; 
            
            objPopview.index=questionIndex;
            
            objPopview.checkedCell=ratingLabel3.text;
            
            objPopview.QuestionText=appDelegate.answerthree;
            
            objPopview.delegate=self;
            
            popSweepStakes =[[UIPopoverController alloc]initWithContentViewController:objPopview];
            
            [popSweepStakes presentPopoverFromRect:ratingButton3.frame inView:ViewPost permittedArrowDirections:UIPopoverArrowDirectionRight animated:YES];
            
            [popSweepStakes setPopoverContentSize:CGSizeMake(300, 400)];
            
        }
        
        return;
    }
}

- (BOOL)textFieldShouldEndEditing:(UITextField *)textField
{
	[Name resignFirstResponder];
	[Email resignFirstResponder];
	[txtCityCode resignFirstResponder];
	[txtcategory resignFirstResponder];
	[txtTitle resignFirstResponder];
	[txtQuestionOne resignFirstResponder];
	[txtQuestionTwo resignFirstResponder];
	[txtQuestionThree resignFirstResponder];
    [self RescheduleTimer];
    ///writecode
	return YES;
}
- (void)textFieldDidBeginEditing:(UITextField *)textField
{
    [self RescheduleTimer];
    NSLog(@"textFieldDidBeginEditing");
    self.checkFLAG=TRUE;
    
    if (appDelegate.flgClickOnWriteReview) {
        
        if (textField == txtTitle) 
        {
            ViewPost.frame = CGRectMake(0, -50, 1024, 970);
        }
    }
    
    else {
        
        if (textField == txtTitle) 
        {
            ViewPost.frame = CGRectMake(0, -74, 1024, 970);
        }
    }
    
}

-(void)textFieldDidEndEditing:(UITextField *)textField
{
    ViewPost.frame = CGRectMake(0, 0, 1024, 1025);	
}


- (void)textViewDidBeginEditing:(UITextView *)textView
{
    [self RescheduleTimer];
    NSLog(@"textViewDidBeginEditing");
    
    self.checkFLAG=TRUE;
    
    for (UIImageView *img in ViewPost.subviews)
    {
        if ([img isKindOfClass:[UIImageView class]]) 
        {
            if (img.tag==textView.tag) 
            {
                [img setImage:[UIImage imageNamed:@"Redinputbox.png"]];
            }else{
                if (img.tag!=0) {     
                    
                    {
                        [img setImage:[UIImage imageNamed:@"textbox.png"]];
                    }
                }
            }
        }        
        
    }
    
	if (textView==txtDescription) 
    {
		ViewPost.frame = CGRectMake(0, -350, 1024, 970);	
        
		if (!flgCleantxt) 
        {
			flgCleantxt=TRUE;
			txtDescription.text = @"";
		}
	}
}

- (void)textViewDidEndEditing:(UITextView *)textView
{
    
    //    ViewPost.frame = CGRectMake(0, -370, 1024, 970);
    
    for (UIImageView *img in ViewPost.subviews)
    {
        if ([img isKindOfClass:[UIImageView class]]) 
        {
            if (img.tag==textView.tag) 
            {
                [img setImage:[UIImage imageNamed:@"inputbox.png"]];
                
            }
        }        
        
    }
}


-(void)keyboardWillShowNotification:(NSNotification *)note
{	
	picCategory.hidden=YES;
	tlbCategory.hidden=YES;
	flgCatPic=NO;
	//scrView.frame=CGRectMake(0, 0, 1024, 450);
    ///writecode
    [self RescheduleTimer];
}
-(void)keyboardWillHideNotification
{
    ViewPost.frame = CGRectMake(0, 0, 1024, 970);
    ///writecode
    [self RescheduleTimer];
    //	scrView.frame = CGRectMake(0,0 , 1024,748);
	
}
-(void)viewDidAppear:(BOOL)animated
{
    //	if (termpage==TRUE)
    //    {
    //		termpage=FALSE;
    //		Term_para *Termsp = [[Term_para alloc]init];
    //        [self.navigationController pushViewController:Termsp animated:YES];
    //        [Termsp release];
    //        
    //        termpage=FALSE;
    //	}
}

- (void)dealloc 
{
    [[self class] cancelPreviousPerformRequestsWithTarget:self selector:@selector(ReidrectToHome) object:nil];  
	[super dealloc];
}

@end
