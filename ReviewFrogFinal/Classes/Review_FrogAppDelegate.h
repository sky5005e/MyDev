//
//  Review_FrogAppDelegate.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 31/12/10.
//  Copyright __MyCompanyName__ 2010. All rights reserved.
//
 
#import <UIKit/UIKit.h>
#import <CoreFoundation/CoreFoundation.h>
#import <AVFoundation/AVAudioPlayer.h>
#import <AVFoundation/AVFoundation.h>
#import "Reachability.h"
#import <sqlite3.h>
#import "VideoObject.h"
#import "SplashScreenViewController.h"
#import "Businessprofile.h"
#import "WebAndListingViewController.h"
#import "WriteORRecodeViewController.h"
#import "WriteObject.h"
#import "XMLVideo.h"
#import "MBProgressHUD.h"
#import "LoginInfo.h"
//#import "Xmlparser.h"

@class SplashScreenViewController;
@class WebAndListingViewController;
@class WriteORRecodeViewController;
@class ApplicationValidation; 
@class VideoObject;
@class WriteObject;
@class XMLVideo;
@interface Review_FrogAppDelegate : NSObject <UIApplicationDelegate,MBProgressHUDDelegate,AVAudioPlayerDelegate> {
    UIWindow *window;
	SplashScreenViewController *viewController;
	WebAndListingViewController *objWebandListing;
	WriteORRecodeViewController *awriteRecord;
    
    
    MBProgressHUD *HUD;
	
	Reachability *remoteHost;
	int remoteHostStatus;
	
	int i;
    int f;
	int loop;
	NSString *AdminCity;
	NSString *admincitycounter;
	NSString *category_text;
	NSString *userId;
	NSString *userEmail1;
	NSString *Data_name;
	NSString *Data_email;
	NSString *CityName;
	NSString *Data_desc;
	NSString *Data_useid;
	NSString *Data_date;
	NSString *Data_bdate;
	NSString *Data_Rstatus;
	NSString *Data_Title;
	NSString *imagePath;
	NSString *dataPath;
	
	NSMutableArray *ErrorCode;
	
	NSMutableArray *HistoryList;
	NSMutableArray *CityList;
	NSMutableArray *delArrCitylist;

	NSMutableArray *delarrVideoList;
	
	NSString *AdminEmail1;
	NSString *search;
	NSString *strSearch;
	NSString *Suggestion;
	NSString *btntitle;
	
	NSData *DataVideo;
	NSURL *createdVideoURL;
	BOOL HViewClick;
	NSString *VideoName;
	float Rating;	
	
	NSTimer *idleTimer;
	NSTimer *idleTimer2;
	
	BOOL checkflag;
	BOOL isdeleted;
	BOOL isposted;
	BOOL LoginFlag;
	BOOL LoginConform;
	BOOL AdminFlag;
	BOOL HistoryFlag;
	BOOL SwitchFlag;
	BOOL HistoryLoginErorrFlag;
	BOOL FirstUser;
	BOOL txtcleanflag;
	BOOL sugestionflag;
	BOOL flgTypeRecordClick;
	BOOL flgReRecord;
	
	NSString *UserEmail;
	NSString *UserPassword;
	
	int selectedTabIndex;
	
	AVAudioPlayer *audioPlayer;
	 
	
	NSString *poststatus;
	NSString *admin_email;
	NSString *admin_password;
	NSString *adminID;

	NSString *videoName1;
	NSString *SelectedCityName;

	// Settings
	NSString *set_UserEmail;
	NSString *set_APIurl;
	NSString *websiteAPIurl;
	//Db Objects
	NSString *databaseName;
	NSString *databasePath;
	
	NSURL *VideoPath;
	NSData *NewData;
	UIButton *btnDelete;
	
	NSMutableArray *ArrInactive;
	NSMutableArray *ArrActive;
	NSMutableArray *ArrDelete;
	NSMutableArray *array_get_videoName;
	
	
	NSString *BusinessVideoTitle;
	NSURL *BusinessUrl;
	NSString *strBusinessTitle;
	
	BOOL flgBusinessxml;	
	NSMutableArray *delarrBusinessList;
	
	NSData *BusinessDataVideo;
	NSString *BusinessVideoName;
	NSURL *BusinessVideoPath;
	BOOL flgBusinessReRecord;
	UIButton *btnDeleteBusiness;
	
	NSData *NewBusinessData;
	BOOL flgReRecSub;
	
	NSMutableArray *delarrGalleryList;
	
	//Save pic into Database
	NSURL *picPath;
	NSURL *createdPicURL;
	NSData *DataPic;
	BOOL picUpload;
	
	BOOL flgAfterimage;
	BOOL flgBeforeimage;
	
	NSMutableArray *delArrCategoryWiseList;
	UIButton *btnPosted;
	UIButton *btnNotPosted;
	UIButton *btnInternal;
	
	NSString *SelectedType;
	NSString *SelectedCategory;
	
	NSMutableArray *ArrCategoryList;
	NSString *strCategoryname;
	
	BOOL flgClickOnWriteReview;
	BOOL flgClcikOnRecordReview;
	NSMutableArray *delarrBusinessCategory;
	NSMutableArray *delArrSearch;
	NSMutableArray *delArrSearchResult;
	
	NSMutableArray *delArrBusinessSearch;	
	BOOL flgBack;
	
	BOOL flgDismiss;
	NSString *strCity;
	NSString *strCategory;
	NSString *UserBusinesslogo;
	UIImage * imgBusinesslogo;
	NSMutableArray *delArrUerinfo;
	NSMutableArray *delArrSearchList;
	BOOL flgsearch;
	NSMutableArray *delArrvieoSearchList;
	BOOL flgVideoSearch;
	NSData *dataSplash;
	Businessprofile *objBprofile;
	NSString *Set_BusinessUrl;
	BOOL flgBusinessRecording;
	
    
    NSData *data1;
    NSData *data2;
    
	NSString *FirstAns;
	NSString *SecondAns;
	NSString *ThridAns;
	BOOL flgDismissview;
	int businessid;
	NSString *VideoSelectedType;
	NSString *strDefaultCategory;
	BOOL flgStopReloading;
	BOOL flggiveaway;
	BOOL flgswich;
	BOOL flgchangeA;
	BOOL flgchangeD;
    BOOL flgSelectedType;
    BOOL flgSelectedVideoType;
    NSString *Set_Termurl;
	NSMutableArray *array_detail;
    NSMutableArray *categoryFromDB;
    NSMutableArray *array_write;
    NSMutableArray *array_video_in_offline;
    NSMutableArray *array_desp_video;
    NSString *userBusinessId;
    NSString *pathBussinesslogo;
    NSString *pathGiveAway;
    NSString *dateStoreinPre;
    NSString *ChangeUrl;
	BOOL VBack;
    NSString *pref_Q1;
    NSString *pref_Q2;
    NSString *pref_Q3;
    UINavigationController *navigationController;
    NSString *ChangeBusiness;
    
    BOOL category;
    
    LoginInfo *alogininfo;
    
    NSMutableArray *questions;
    
    NSString *answerOne,*answerTwo,*answerthree;
    
    BOOL ansOne,ansTwo,ansThrre;
    
    NSString *questionOneId,*questionTwoId,*questionThreeId;
    
    NSString *answerOneId,*answerTwoId,*answerThreeId;
    
    NSString *rating1,*rating2,*rating3;
    
    BOOL rating1Bool,rating2Bool,rating3Bool;
    
    BOOL sweepstake;
}

@property (nonatomic) BOOL sweepstake;

@property (nonatomic,retain) NSString *answerOneId,*answerTwoId,*answerThreeId;

@property (nonatomic,retain) NSString *rating1,*rating2,*rating3;

@property (nonatomic) BOOL rating1Bool,rating2Bool,rating3Bool;

@property (nonatomic,retain) NSString *questionOneId,*questionTwoId,*questionThreeId;

@property (nonatomic,retain) NSMutableArray *questions;

@property (nonatomic,retain) NSString *answerOne,*answerTwo,*answerthree;

@property (nonatomic) BOOL ansOne,ansTwo,ansThrre;

@property (nonatomic) BOOL category;
@property (nonatomic, retain) NSString *ChangeBusiness;
@property BOOL HViewClick;
@property (nonatomic, retain) IBOutlet UINavigationController *navigationController;
@property (nonatomic, retain) NSString *pref_Q1;
@property (nonatomic, retain) NSString *pref_Q2;
@property (nonatomic, retain) NSString *pref_Q3;
@property (nonatomic, retain) NSString *userBusinessId;
@property (nonatomic, retain) NSString *ChangeUrl;
@property (nonatomic, retain) NSString *dateStoreinPre;
@property (nonatomic, retain) NSMutableArray *array_video_in_offline;
@property (nonatomic, retain) NSMutableArray *array_detail;
@property (nonatomic, retain) NSMutableArray *array_desp_video;
@property (nonatomic, retain) NSMutableArray *array_write;
@property (nonatomic, retain) NSMutableArray *array_get_videoName;

@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet SplashScreenViewController *viewController;
@property (nonatomic, retain) IBOutlet WebAndListingViewController *objWebandListing;
@property (nonatomic, retain) IBOutlet WriteORRecodeViewController *awriteRecord;

@property (nonatomic, retain) NSMutableArray *ErrorCode;
@property (nonatomic, retain) NSString *userId;
@property (nonatomic, retain) NSString *imagePath;
@property (nonatomic, retain) NSMutableArray *HistoryList;
@property (nonatomic, retain) NSMutableArray *categoryFromDB;
@property (nonatomic, retain) NSString *userEmail1;
@property (nonatomic, retain) NSString *Data_name; 
@property (nonatomic, retain) NSString *Data_email;
@property (nonatomic, retain) NSString *Data_desc;
@property (nonatomic, retain) NSString *Data_useid;
@property (nonatomic, retain) NSString *Data_date;
@property (nonatomic, retain) NSString *Data_bdate;
@property (nonatomic, retain) NSString *Data_Title;
@property (nonatomic, retain) NSString *Data_Rstatus;
@property (nonatomic, retain) NSString *poststatus;
@property (nonatomic, retain) NSString *admin_email;
@property (nonatomic, retain) NSString *admin_password;
@property (nonatomic, retain) NSString *adminID;
@property (nonatomic, retain) NSMutableArray *CityList;
@property (nonatomic, retain) NSMutableArray *delArrCitylist;
@property (nonatomic, retain) NSString *AdminCity;
@property (nonatomic, retain) NSString *admincitycounter;
@property (nonatomic, retain) NSString *UserEmail;
@property (nonatomic, retain) NSString *UserPassword;
@property (nonatomic, retain) NSTimer *idleTimer;
@property (nonatomic, retain) NSTimer *idleTimer2;
@property (nonatomic, retain) AVAudioPlayer *audioPlayer;
@property (nonatomic, retain) NSString *AdminEmail1;
@property (nonatomic, retain) NSString *CityName;
@property (nonatomic, retain) NSString *search;
@property (nonatomic, retain) NSString *strSearch;
@property (nonatomic, retain) NSString *Suggestion;
@property (nonatomic, retain) NSString *SelectedCityName;
@property (nonatomic, retain) NSString *btntitle;
@property float Rating;
@property (nonatomic, retain) NSString *videoName1;
@property (nonatomic, retain) NSString *category_text;

@property (nonatomic, retain) NSData *DataVideo;
@property (nonatomic, retain) NSURL *createdVideoURL;

@property (nonatomic, retain) NSString *databaseName;
@property (nonatomic, retain) NSString *databasePath;
@property (nonatomic, retain) NSString *VideoName;

@property (nonatomic, retain) NSURL *VideoPath;

@property (nonatomic, retain) NSMutableArray *delarrVideoList;
@property (nonatomic, retain) NSString *dataPath;

@property (nonatomic, retain) NSData *NewData;
@property (nonatomic, retain) UIButton *btnDelete;
@property (nonatomic, retain) NSString *BusinessVideoTitle;

@property (nonatomic, retain) NSURL *BusinessUrl;
@property (nonatomic, retain) NSString *strBusinessTitle;
@property (nonatomic, retain) NSData *BusinessDataVideo;
@property BOOL flgBusinessReRecord;
@property BOOL flgReRecSub;

@property (nonatomic, retain) NSMutableArray *delArrCategoryWiseList;
@property (nonatomic, retain) NSMutableArray *delArrSearch;
@property (nonatomic, retain) NSMutableArray *delArrSearchResult;
@property (nonatomic, retain) NSMutableArray *delArrBusinessSearch;
@property (nonatomic, retain) NSString *UserBusinesslogo;
@property BOOL flgBack;
@property (nonatomic, retain) UIImage *imgBusinesslogo;
@property (nonatomic, retain) NSMutableArray *delArrUerinfo;
@property (nonatomic, retain) NSString *VideoSelectedType;
@property (nonatomic, retain) NSString *strDefaultCategory;
@property BOOL flgchangeA;
@property BOOL flgchangeD;

@property BOOL flggiveaway;
@property BOOL flgswich;
@property BOOL flgSelectedType;
@property BOOL flgSelectedVideoType;

// Database Functions
-(void) dbconnect;
-(void) checkAndCreateDatabase;
-(sqlite3_stmt*) PrepareStatement:(const char *)sql;


@property BOOL checkflag;
@property BOOL isdeleted;
@property BOOL isposted;
@property BOOL LoginFlag;
@property BOOL LoginConform;
@property BOOL AdminFlag;
@property BOOL HistoryFlag;
@property BOOL SwitchFlag;
@property BOOL HistoryLoginErorrFlag;
@property BOOL FirstUser;
@property int selectedTabIndex;
@property BOOL txtcleanflag;
@property int  remoteHostStatus;
@property BOOL sugestionflag;
@property BOOL flgTypeRecordClick;
@property BOOL flgReRecord;
@property BOOL flgStopReloading;

@property int businessid;
@property BOOL picUpload;
@property (nonatomic, retain) NSString *pathBussinesslogo;
@property (nonatomic, retain) NSString *pathGiveAway;

@property (nonatomic, retain) NSString *set_UserEmail;
@property (nonatomic, retain) NSString *set_APIurl;

@property (nonatomic, retain) NSString *websiteAPIurl;
@property (nonatomic, retain) NSData *NewBusinessData;

@property (nonatomic, retain) NSData *data1;
@property (nonatomic, retain) NSData *data2;

@property (nonatomic, retain) NSMutableArray *ArrInactive;
@property (nonatomic, retain) NSMutableArray *ArrActive;
@property (nonatomic, retain) NSMutableArray *ArrDelete;

@property (nonatomic, retain) NSString *BusinessVideoName;
@property (nonatomic, retain) NSURL *BusinessVideoPath;

@property BOOL flgBusinessxml;	
@property (nonatomic, retain) NSMutableArray *delarrBusinessList;

@property (nonatomic, retain) UIButton *btnDeleteBusiness;
@property (nonatomic, retain) NSMutableArray *delarrGalleryList;
@property (nonatomic, retain) NSURL *picPath;
@property (nonatomic, retain) NSURL *createdPicURL;
@property (nonatomic, retain) NSData *DataPic;
@property  BOOL flgAfterimage;
@property  BOOL flgBeforeimage;

@property (nonatomic, retain) UIButton *btnPosted;
@property (nonatomic, retain) UIButton *btnNotPosted;
@property (nonatomic, retain) UIButton *btnInternal;

@property (nonatomic, retain) NSString *SelectedType;
@property (nonatomic, retain) NSString *SelectedCategory;
@property (nonatomic, retain) NSMutableArray *ArrCategoryList;
@property (nonatomic, retain) NSString *strCategoryname;
@property BOOL flgClickOnWriteReview;
@property BOOL flgClcikOnRecordReview;
@property (nonatomic, retain) NSMutableArray *delarrBusinessCategory;

@property BOOL flgDismiss;
@property (nonatomic, retain) NSString *strCity;
@property (nonatomic, retain) NSString *strCategory;
@property (nonatomic, retain) NSMutableArray *delArrSearchList;
@property BOOL flgsearch;
@property (nonatomic, retain) NSMutableArray *delArrvieoSearchList;
@property BOOL flgVideoSearch;
@property (nonatomic, retain) NSData *dataSplash;
@property (nonatomic, retain) Businessprofile *objBprofile;
@property (nonatomic, retain) NSString *Set_BusinessUrl;
@property BOOL flgBusinessRecording;

@property (nonatomic, retain) NSString *FirstAns;
@property (nonatomic, retain) NSString *SecondAns;
@property (nonatomic, retain) NSString *ThridAns;
@property BOOL flgDismissview;
@property (nonatomic, retain) NSString *Set_Termurl;
@property BOOL VBack;


- (void)readSettings;

- (void)getUserPreferences;

- (void)saveUserPreferences :(int)Preferences;

- (void)ResetTimer;

- (void)OnTimer;

- (void)resetIdleTimer;

- (void)idleTimerExceeded;

- (IBAction) playMovie;

- (void)VideoInfoSave:(NSString *)strvideoName;

- (void)connect;

-(int)saveVideoToDir;

- (BOOL)apiPostReviewVideo :(int)VideoID VTitle:(NSString *)VideoTitle video:(NSData *)ReveiwVideo ;

- (BOOL)apiPostBusinessVideo :(int)VideoID VTitle:(NSString *)VideoTitle video:(NSData *)ReveiwVideo ;

- (NSString*)apiGallery :(NSString *)userid;

-(BOOL)apiInserGalleryinfo :(NSString *)name pemail:(NSString *)email comment:(NSString *)comment bpic:(NSString *)bpic apic:(NSString*)apic status:(NSString *)status useremail:(NSString *)useremail userid:(NSString*)userid;

-(void)GetAllVideo;

-(int)saveVideoToBusinessDir;

-(int)savePicToDir :(NSString *)picName picimage:(UIImage *)img;

-(BOOL)apiUploadGalleryinfo :(NSString *)name pemail:(NSString *)email comment:(NSString *)comment bpic:(NSString *)bpic status:(NSString *)status useremail:(NSString *)useremail userid:(NSString*)userid beforeimage:(UIImage *)bimage galleryid:(int)gid;

-(BOOL)apiafterimageuploading :(int)gid apic:(NSString*)apic afterimage:(UIImage *)aimage;

-(UIImage *)imageByScalingProportionallyToSize:(CGSize)targetSize srcImg:(UIImage *)image;

-(void)UpDateCategory:(int)categoryid cname:(NSString *)Categoryname;

-(void)LoadReviewCategory;

-(NSString *)apiSearch:(NSString*)category city:(NSString*)cityname;

-(NSString *)apiSearchResult:(int)Bid;

-(BOOL)apiCategory;

-(void)dbconnect_secondDB;

-(void)checkAndCreateDatabase1;

-(void)addToDatabase:(NSString *)Name email:(NSString *)email citycode:(NSString *)citycode cat:(NSString *)category title:(NSString *)title Description:(NSString *)Description QuestionOne:(NSString *)QuestionOne
         QuestionTwo:(NSString *)QuestionTwo QuestionThree:(NSString *)QuestionThree;

-(void)addToLoginDetail:(NSString *)userbusinesslogo BusinessSplash:(NSString *)BusinessSplash questionone:(NSString *)questionone questiontwo:(NSString *)questiontwo questionthree:(NSString *)questionthree userbusinessid:(NSString *)userbusinessid;

-(void)addCategory:(NSString *)userid category:(NSString *)category;

-(void)addWriteReview:(NSString *)name email:(NSString *)email city:(NSString *)city questionone:(NSString *)questionone questiontwo:(NSString *)questiontwo questionthree:(NSString *)questionthree category:(NSString *)category reviewtitle:(NSString *)reviewtitle  desp:(NSString *)desp rating:(NSString *)rating userid:(NSString *)userid status:(NSString *)status date:(NSString *)date;

-(void)addVideoReview:(NSString *)name email:(NSString *)email city:(NSString *)city questionone:(NSString *)questionone questiontwo:(NSString *)questiontwo questionthree:(NSString *)questionthree category:(NSString *)category reviewtitle:(NSString *)reviewtitle rating:(NSString *)rating userid:(NSString *)userid status:(NSString *)status date:(NSString *)date ReviewVideoName:(NSString *)ReviewVideoName;

-(void)getVideo:(NSString *)strvideoName videourl:(NSString *)strvideoURL;

-(BOOL)checkResponse;

-(void)readFreomDB;

-(void)readWriteView;

-(void)categoryFromDBMethod;

-(void)Delete;

-(void)DeleteQueInfo;

-(void)DeleteCategory;

-(void)SubmitWriteReview;

-(void)DeleteWriteRecord :(NSString *)name;

-(void)threadMethod;

-(void)read_video_review;

-(void)threadMethodVideo;

-(void)SubmitVideoReview;

-(void)SubmitVideo;

-(void)readVideoName;

-(void)DeleteVideo_DESP ;

-(void)DeleteVideoName :(NSString *)name;

-(void)timercheckIsDataUploaded;


@end

