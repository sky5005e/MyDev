//
//  Review_FrogViewController.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 31/12/10.
//  Copyright __MyCompanyName__ 2010. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Xmlparser.h"
#import "HistoryLogin.h"
#import "AdminCoupon.h"
#import "SearchCityViewController.h"
#import "CategoryPopViewController.h"
#import "RateView.h"
#import "MBProgressHUD.h"
#import "SweepStakesViewController.h"
#import "SubcategoryPopViewController.h"
#import "RatingPopViewController.h"

@class Review_FrogAppDelegate;

@interface ApplicationValidation : UIViewController <UISearchBarDelegate,UISearchDisplayDelegate,OptionsViewControllerDelegate,UIScrollViewDelegate,UITextFieldDelegate,UIAlertViewDelegate,RateViewDelegate,UITextViewDelegate,CategorydidSelectdelegate,MBProgressHUDDelegate,AVAudioPlayerDelegate,AVCaptureVideoDataOutputSampleBufferDelegate,UIImagePickerControllerDelegate,AnswerSelectedDelegate,RatingSelectedDelegate,UINavigationControllerDelegate>{
	
	Review_FrogAppDelegate *appDelegate;
	RateView *_rateView;
	MBProgressHUD *HUD;	
    BOOL valid;
    NSString *emailString;
    NSPredicate *emailTest;
    
	IBOutlet UIScrollView *scrView;
	IBOutlet UIView *ViewPost;

	IBOutlet UIImageView *imgQueone;
	IBOutlet UIImageView *imgQueTwo;
	IBOutlet UIImageView *imgQueThree;
	IBOutlet UIImageView *imgGiveAway;
	
	UIAlertView *alertname;
	UIAlertView *alertemail;
	UIAlertView *alertemailtest;
	UIAlertView *alertcity;
	UIAlertView *AlertQueone;
	UIAlertView *AlertQueTwo;
	UIAlertView *AlertQueThree;
	UIAlertView *AlertReviewtitle;
	UIAlertView *AlertDescription;
    UIAlertView *AlertCategory;
	
	IBOutlet UIBarButtonItem *btnDone;
	IBOutlet UIButton *btnWrite;
	IBOutlet UIButton *btnVideo;
	IBOutlet UIImageView *imgtxtbordar;
	IBOutlet UIPickerView *picCategory;
	IBOutlet UIToolbar *tlbCategory;
	IBOutlet UITextField *txtcategory;
    IBOutlet UITextField *txtsubcategory;
	
	IBOutlet UITextView *txtDescription;
	IBOutlet UITextField *txtTitle;
	
	IBOutlet UITextField *txtQuestionOne;	
	IBOutlet UITextField *txtQuestionTwo;
	IBOutlet UITextField *txtQuestionThree;	
		
	UIPopoverController *popView;
    
	
	IBOutlet UITextField *Name;
	IBOutlet UITextField *Email;
	IBOutlet UITextField *txtCityCode;
	NSString * currentElement;
	NSMutableArray *ErrorCode;
	IBOutlet UIButton *btnHome;
	IBOutlet UIButton *btncontinue;
	NSString *stringbirthdate;
	BOOL pickeflag;
	BOOL worngflag;
	NSString *selectedcity;
	
	BOOL fistCity2;
	BOOL pickerflg;
	BOOL flgCatPic;
	BOOL flgRating;
	BOOL flgCleantxt;
	BOOL termpage;
	
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgtxtPowerdby;
	IBOutlet UIButton *btnCategory;
	IBOutlet UIButton *btnadmin;
	IBOutlet UILabel *lblwithwin;
	IBOutlet UILabel *lblwithoutwin;
    UIPopoverController *popSweepStakes;
    
    NSTimer *timer;
    NSTimer *timer1;
    NSTimer *timer2;
    BOOL checkFLAG;
    
    NSTimer *stopWatchTimer;

    
    LoginInfo* alogin;
    
    BOOL resAPI;
    
    UIImageView *crosshairView;
    
    UIImageView *publishImage;
    
    UIToolbar *toolbar;
    
    BOOL video;
    
    AVCaptureVideoOrientation *orientation;
    
    UIImagePickerController *imagePicker;
    
    int index;
        
    UIAlertView *alertForAnswer;
    
    IBOutlet UIButton *ratingButton1,*ratingButton2,*ratingButton3;
    
    IBOutlet UILabel *ratingLabel1,*ratingLabel2,*ratingLabel3;
    
    int questionIndex;
    
    IBOutlet UIImageView *categoryListImage;
    
    IBOutlet UIButton *btnsweepstake;    
    
    int touchflag;
}
@property(nonatomic,retain) UITextField *Name;
@property(nonatomic,retain) UITextField *Email;
@property(nonatomic,retain) UIButton *btnHome;
@property(nonatomic,retain) NSString * currentElement;
@property(nonatomic,retain) NSMutableArray *ErrorCode;

@property(nonatomic,retain) UITextField *txtQuestionOne;	
@property(nonatomic,retain) UITextField *txtQuestionTwo;
@property(nonatomic,retain) UITextField *txtQuestionThree;	


@property(nonatomic,retain) IBOutlet RateView *rateView; 
@property(nonatomic,retain) UIBarButtonItem *btnDone;
@property(nonatomic,retain) UIButton *btnWrite;
@property(nonatomic,retain) UIButton *btnVideo;
@property(nonatomic,retain) UIImageView *imgtxtbordar;
@property(nonatomic,retain) IBOutlet UIPickerView *picCategory;
@property(nonatomic,retain) UIToolbar *tlbCategory;
@property(nonatomic,retain) UITextField *txtcategory;

@property(nonatomic,retain) UITextView *txtDescription;
@property(nonatomic,retain) UITextField *txtTitle;

@property (weak, nonatomic) NSTimer *stopWatchTimer;

@property BOOL pickeflag;
@property BOOL worngflag;
@property BOOL checkFLAG;
@property(nonatomic,retain) NSString *stringbirthdate;
@property (nonatomic, retain)IBOutlet UIButton *btncontinue; 
@property (nonatomic, retain)IBOutlet UITextField *txtCityCode;

@property (nonatomic, retain) NSString *selectedcity;
@property BOOL fistCity2;
@property BOOL pickerflg;

-(IBAction) TextFieldDoneEditing;

//-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(IBAction) btnBackClick;
-(IBAction) Home;
-(void)validVideoREview;

-(IBAction) CategoryListClick;
-(IBAction) DoneButtonClick;

-(IBAction) WriteReviewClcik;
-(IBAction) VideoReviewClcik;
-(IBAction)btnsweepstakeClcik;

-(void) getLoginList;
-(void) resetIdleTimer;
-(void) idleTimerExceeded;
-(void) didTap;
// APi.
-(void) loadThreeQuestion;

-(void) startCamera;
-(void) didSelect;
-(void) showloding;
-(void) timer;
-(void)checkValidation;
-(void)displayFromLocal;
- (void) showlodingDB ;
-(int) saveVideoToDir;
- (int)deviceOrientationDidChange;
- (void) showlodingResponse;
- (void) showResponse ;
-(void)checkResponse;
-(void)loadAnswers;

-(IBAction)showRatings:(id)sender;
@end

