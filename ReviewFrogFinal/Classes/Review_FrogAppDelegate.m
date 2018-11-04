//
//  Review_FrogAppDelegate.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 31/12/10.
//  Copyright __MyCompanyName__ 2010. All rights reserved.
//

#import "Review_FrogAppDelegate.h"
#import "SplashScreenViewController.h"
#import "Terms_Condition.h"
#import "ApplicationValidation.h"
#import "PlayVideoViewController.h"
#import "Category.h"
#import "LocalBusinesSearchViewController.h"
#import "LoginInfo.h"
#import "XMLCategory.h"


@implementation Review_FrogAppDelegate

@synthesize window;
@synthesize ChangeBusiness;
@synthesize pref_Q1;
@synthesize pref_Q2;
@synthesize pref_Q3;
@synthesize navigationController;
@synthesize ChangeUrl;
@synthesize HViewClick;
@synthesize data1;
@synthesize data2;
@synthesize pathBussinesslogo;
@synthesize pathGiveAway;
@synthesize userBusinessId;
@synthesize websiteAPIurl;
@synthesize array_video_in_offline;
@synthesize array_get_videoName;
@synthesize array_detail;
@synthesize categoryFromDB;
@synthesize viewController;
@synthesize objWebandListing;
@synthesize awriteRecord;
@synthesize ErrorCode;
@synthesize userId;
@synthesize userEmail1;
@synthesize Data_name; 
@synthesize Data_email;
@synthesize Data_desc;
@synthesize Data_useid;
@synthesize Data_date;
@synthesize Data_bdate;
@synthesize Data_Title;
@synthesize HistoryList;
@synthesize Data_Rstatus;
@synthesize checkflag;
@synthesize poststatus;
@synthesize isdeleted;
@synthesize isposted;
@synthesize set_UserEmail;
@synthesize CityList;
@synthesize HistoryFlag;
@synthesize AdminCity;
@synthesize LoginFlag;
@synthesize LoginConform;
@synthesize admin_password;
@synthesize admin_email;
@synthesize adminID;
@synthesize AdminFlag;
@synthesize selectedTabIndex;
@synthesize delArrCitylist;
@synthesize admincitycounter;
@synthesize UserEmail;
@synthesize UserPassword;
@synthesize idleTimer;
@synthesize SwitchFlag;
@synthesize HistoryLoginErorrFlag;
@synthesize AdminEmail1;
@synthesize FirstUser;
@synthesize idleTimer2;
@synthesize CityName;
@synthesize txtcleanflag;
@synthesize search;
@synthesize strSearch;
@synthesize remoteHostStatus;
@synthesize Suggestion;
@synthesize set_APIurl;
@synthesize sugestionflag;
@synthesize Rating;
@synthesize SelectedCityName;
@synthesize btntitle;
@synthesize flgTypeRecordClick;
@synthesize databaseName;
@synthesize databasePath;
@synthesize imagePath;
@synthesize flgReRecord;
@synthesize category_text;
@synthesize VideoName;
@synthesize array_desp_video;

@synthesize VideoPath;
@synthesize createdVideoURL;
@synthesize delarrVideoList;
@synthesize videoName1;
@synthesize DataVideo;
@synthesize dataPath;
@synthesize NewData;
@synthesize btnDelete;

@synthesize ArrInactive;
@synthesize ArrActive;
@synthesize ArrDelete;

@synthesize BusinessVideoTitle;
@synthesize BusinessUrl;
@synthesize strBusinessTitle;

@synthesize flgBusinessxml;
@synthesize delarrBusinessList;
@synthesize BusinessDataVideo;

@synthesize BusinessVideoName;
@synthesize BusinessVideoPath;
@synthesize flgBusinessReRecord;
@synthesize btnDeleteBusiness;

@synthesize NewBusinessData;
@synthesize audioPlayer;
@synthesize flgReRecSub;
@synthesize delarrGalleryList;


@synthesize picPath;
@synthesize createdPicURL;
@synthesize DataPic;
@synthesize picUpload;

@synthesize flgAfterimage;
@synthesize flgBeforeimage;
@synthesize delArrCategoryWiseList;


@synthesize btnPosted;
@synthesize btnNotPosted;
@synthesize btnInternal;
@synthesize SelectedType;
@synthesize SelectedCategory;

@synthesize ArrCategoryList;
@synthesize strCategoryname;
@synthesize flgClickOnWriteReview;
@synthesize flgClcikOnRecordReview;
@synthesize delarrBusinessCategory;
@synthesize delArrSearch;
@synthesize delArrSearchResult;
@synthesize delArrBusinessSearch;
@synthesize flgBack;
@synthesize flgDismiss;
@synthesize strCity;
@synthesize strCategory;
@synthesize UserBusinesslogo;
@synthesize imgBusinesslogo;
@synthesize delArrUerinfo;
@synthesize delArrSearchList;
@synthesize flgsearch;
@synthesize delArrvieoSearchList;
@synthesize flgVideoSearch;
@synthesize dataSplash;
@synthesize objBprofile;
@synthesize Set_BusinessUrl;
@synthesize flgBusinessRecording;
@synthesize FirstAns;
@synthesize SecondAns;
@synthesize ThridAns;
@synthesize flgDismissview;
@synthesize businessid;
@synthesize VideoSelectedType;
@synthesize strDefaultCategory;
@synthesize flgStopReloading;
@synthesize flggiveaway;
@synthesize flgswich;
@synthesize flgchangeA;
@synthesize flgchangeD;
@synthesize flgSelectedType;
@synthesize flgSelectedVideoType;
@synthesize Set_Termurl;
@synthesize array_write;
@synthesize dateStoreinPre;
@synthesize VBack;
@synthesize category;

@synthesize questions;
@synthesize answerOne,answerTwo,answerthree;

@synthesize questionOneId,questionTwoId,questionThreeId;

@synthesize rating1,rating2,rating3;

@synthesize rating1Bool,rating2Bool,rating3Bool;

@synthesize ansOne,ansTwo,ansThrre;

@synthesize answerOneId,answerTwoId,answerThreeId;

@synthesize sweepstake;

- (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {    
    
    ansOne=NO;
    ansTwo=NO;
    ansThrre=NO;
    
    self.rating1=@"0";
    
    self.rating2=@"0";
    
    self.rating3=@"0";

    alogininfo=[[LoginInfo alloc] init];

	[self readSettings];
	[self getUserPreferences];
	//[self apiCategory];
	self.sugestionflag=TRUE;
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(reachabilityChanged:) name:kReachabilityChangedNotification object: nil];
	
	remoteHost = [[Reachability reachabilityWithHostName:@"www.apple.com"] retain];
	[remoteHost startNotifier];
		
	NSString *newAudioFile = [[NSBundle mainBundle] pathForResource:@"Frog"  ofType:@"mp3"];
	
	audioPlayer =  [[AVAudioPlayer alloc] initWithContentsOfURL:[NSURL fileURLWithPath:newAudioFile] error:NULL];
	
	[audioPlayer setDelegate:self];
	
	[audioPlayer prepareToPlay];
	if(SwitchFlag ==TRUE)
	{
		[self resetIdleTimer];
	
	}
	
//	[self dbconnect];
	[self connect];
    [self dbconnect_secondDB];
//    [self timercheckIsDataUploaded];
//	[window addSubview:viewController.view];
    [self.window addSubview:navigationController.view];
	[window makeKeyAndVisible];	

	return YES;
}
-(void) dbconnect_secondDB{
    NSLog(@"IN DATABASE");
    self.databaseName = @"ReviewFrog.sqlite";
    NSArray *documentPaths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDir = [documentPaths objectAtIndex:0];
    self.databasePath = [documentsDir stringByAppendingPathComponent:self.databaseName];
    [self checkAndCreateDatabase1];
}
-(void) checkAndCreateDatabase1{
    
    // Check if the SQL database has already been saved to the users phone, if not then copy it over
    BOOL success;
    
    NSFileManager *fileManager = [NSFileManager defaultManager];
    
    // Check if the database has already been created in the users filesystem
    success = [fileManager fileExistsAtPath:databasePath];
    
    // If the database already exists then return without doing anything
    if(success) {
        return;
    }
    // If not then proceed to copy the database from the application to the users filesystem
    
    // Get the path to the database in the application package
    NSString *databasePathFromApp = [[[NSBundle mainBundle] resourcePath] stringByAppendingPathComponent:databaseName];
    
    // Copy the database from the package to the users filesystem
    [fileManager copyItemAtPath:databasePathFromApp toPath:self.databasePath error:nil];

    
}

-(void)addToLoginDetail:(NSString *)userbusinesslogo BusinessSplash:(NSString *)BusinessSplash questionone:(NSString *)questionone questiontwo:(NSString *)questiontwo questionthree:(NSString *)questionthree userbusinessid:(NSString *)userbusinessid 
{	
	sqlite3 *database;
	
    NSLog(@"%@",BusinessSplash );
    NSLog(@"%@",questionone );
    NSLog(@" userbusinessid :--%@",userbusinessid );
    
    NSData *receivedData = [[NSData dataWithContentsOfURL:[NSURL URLWithString:userbusinesslogo]] retain];
    UIImage *image = [[UIImage alloc] initWithData:receivedData] ;
    
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "INSERT INTO ReviewFrog (userbusinesslogo,BusinessSplash,questionone,questiontwo,questionthree,userbusinessid) VALUES (?,?,?,?,?,?)";
		sqlite3_stmt *statement;
		statement = [self PrepareStatement:sql];
        
		int a1 = sqlite3_bind_text(statement, 1, [userbusinesslogo UTF8String], -1, SQLITE_TRANSIENT);
		int a2 = sqlite3_bind_text(statement, 2, [BusinessSplash UTF8String], -1, SQLITE_TRANSIENT);
        
		int a3 = sqlite3_bind_text(statement, 3, [questionone UTF8String], -1, SQLITE_TRANSIENT);
		
		int a4 = sqlite3_bind_text(statement, 4, [questiontwo UTF8String], -1, SQLITE_TRANSIENT);
        
		int a5 = sqlite3_bind_text(statement, 5, [questionthree UTF8String], -1, SQLITE_TRANSIENT);
        
		int a6 = sqlite3_bind_text(statement, 6, [userbusinessid UTF8String], -1, SQLITE_TRANSIENT);        
		if (statement)
		{
			if ( a1 != SQLITE_OK || a3 != SQLITE_OK || a4 != SQLITE_OK|| a5 != SQLITE_OK|| a6 != SQLITE_OK|| a2 != SQLITE_OK )
			{
				sqlite3_finalize(statement);
				NSLog(@"not inserted");
				return;
			}
			NSLog(@"inserted1 ~~~~~ addToLoginDetail");
			sqlite3_step(statement);
			NSLog(@"inserted2 ~~~ addToLoginDetail");
		}
		sqlite3_finalize(statement);
	}
	NSLog(@"~~~inserted~~~addToLoginDetail");
}

-(void)addWriteReview:(NSString *)name email:(NSString *)email city:(NSString *)city questionone:(NSString *)questionone questiontwo:(NSString *)questiontwo questionthree:(NSString *)questionthree category:(NSString *)category reviewtitle:(NSString *)reviewtitle  desp:(NSString *)desp rating:(NSString *)rating userid:(NSString *)userid status:(NSString *)status date:(NSString *)date
{	
	sqlite3 *database;

    NSLog(@"%@",rating );
    NSLog(@"%@",desp );

	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "INSERT INTO write_review (name,email,city,que1,que2,que3,category,reviewtitle,desp,rating,userid,status,date) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)";
		sqlite3_stmt *statement;
		statement = [self PrepareStatement:sql];
        
		int a1 = sqlite3_bind_text(statement, 1, [name UTF8String], -1, SQLITE_TRANSIENT);
		int a2 = sqlite3_bind_text(statement, 2, [email UTF8String], -1, SQLITE_TRANSIENT);
        
        int a3 = sqlite3_bind_text(statement, 3, [city UTF8String], -1, SQLITE_TRANSIENT);
        
		int a4 = sqlite3_bind_text(statement, 4, [questionone UTF8String], -1, SQLITE_TRANSIENT);
		
		int a5 = sqlite3_bind_text(statement, 5, [questiontwo UTF8String], -1, SQLITE_TRANSIENT);
        
		int a6 = sqlite3_bind_text(statement, 6, [questionthree UTF8String], -1, SQLITE_TRANSIENT);
        
		int a7 = sqlite3_bind_text(statement, 7, [category UTF8String], -1, SQLITE_TRANSIENT);
        
        int a8 = sqlite3_bind_text(statement, 8, [reviewtitle UTF8String], -1, SQLITE_TRANSIENT);
        
        int a9 = sqlite3_bind_text(statement, 9, [desp UTF8String], -1, SQLITE_TRANSIENT);
        
        int a10 = sqlite3_bind_text(statement, 10, [rating UTF8String], -1, SQLITE_TRANSIENT);

        int a11 = sqlite3_bind_text(statement, 11, [userid UTF8String], -1, SQLITE_TRANSIENT);
        
        int a12 = sqlite3_bind_text(statement, 12, [status UTF8String], -1, SQLITE_TRANSIENT);
        
        int a13 = sqlite3_bind_text(statement, 13, [date UTF8String], -1, SQLITE_TRANSIENT);
        
		if (statement)
		{
			if ( a1 != SQLITE_OK || a3 != SQLITE_OK || a4 != SQLITE_OK|| a5 != SQLITE_OK|| a6 != SQLITE_OK|| a2 != SQLITE_OK || a7 != SQLITE_OK || a8 != SQLITE_OK || a9 != SQLITE_OK || a10 != SQLITE_OK || a11 != SQLITE_OK || a12 != SQLITE_OK || a13 != SQLITE_OK)
			{
				sqlite3_finalize(statement);
				NSLog(@"not inserted");
				return;
			}
			NSLog(@"inserted1");
			sqlite3_step(statement);
			NSLog(@"inserted2");
		}
		sqlite3_finalize(statement);
	}
	NSLog(@"inserted");
}

-(void)addVideoReview:(NSString *)name email:(NSString *)email city:(NSString *)city questionone:(NSString *)questionone questiontwo:(NSString *)questiontwo questionthree:(NSString *)questionthree category:(NSString *)category reviewtitle:(NSString *)reviewtitle rating:(NSString *)rating userid:(NSString *)userid status:(NSString *)status date:(NSString *)date ReviewVideoName:(NSString *)ReviewVideoName
{	
	sqlite3 *database;
    
    NSLog(@"questionone%@",questionone);
    
    NSLog(@"questiontwo%@",questiontwo );
    
    NSLog(@"questionthree%@",questionthree );

    
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "INSERT INTO video_review (name,email,city,que1,que2,que3,category,reviewtitle,rating,userid,status,date,ReviewVideoName) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?)";
		sqlite3_stmt *statement;
		statement = [self PrepareStatement:sql];
        
		int a1 = sqlite3_bind_text(statement, 1, [name UTF8String], -1, SQLITE_TRANSIENT);
		int a2 = sqlite3_bind_text(statement, 2, [email UTF8String], -1, SQLITE_TRANSIENT);
        
        int a3 = sqlite3_bind_text(statement, 3, [city UTF8String], -1, SQLITE_TRANSIENT);
        
		int a4 = sqlite3_bind_text(statement, 4, [questionone UTF8String], -1, SQLITE_TRANSIENT);
		
		int a5 = sqlite3_bind_text(statement, 5, [questiontwo UTF8String], -1, SQLITE_TRANSIENT);
        
		int a6 = sqlite3_bind_text(statement, 6, [questionthree UTF8String], -1, SQLITE_TRANSIENT);
        
		int a7 = sqlite3_bind_text(statement, 7, [category UTF8String], -1, SQLITE_TRANSIENT);
        
        int a8 = sqlite3_bind_text(statement, 8, [reviewtitle UTF8String], -1, SQLITE_TRANSIENT);
        
        int a9 = sqlite3_bind_text(statement, 9, [rating UTF8String], -1, SQLITE_TRANSIENT);
        
        int a10 = sqlite3_bind_text(statement, 10, [userid UTF8String], -1, SQLITE_TRANSIENT);
        
        int a11 = sqlite3_bind_text(statement, 11, [status UTF8String], -1, SQLITE_TRANSIENT);
        
        int a12 = sqlite3_bind_text(statement, 12, [date UTF8String], -1, SQLITE_TRANSIENT);
        
        
        int a13 = sqlite3_bind_text(statement, 13, [ReviewVideoName UTF8String], -1, SQLITE_TRANSIENT);
		if (statement)
		{
			if ( a1 != SQLITE_OK || a3 != SQLITE_OK || a4 != SQLITE_OK|| a5 != SQLITE_OK|| a6 != SQLITE_OK|| a2 != SQLITE_OK || a7 != SQLITE_OK || a8 != SQLITE_OK || a9 != SQLITE_OK || a10 != SQLITE_OK || a11 != SQLITE_OK || a12 != SQLITE_OK  || a13 != SQLITE_OK)
			{
				sqlite3_finalize(statement);
				NSLog(@"inserted inserted");
				return;
			}
			NSLog(@"inserted1");
			sqlite3_step(statement);
			NSLog(@"inserted2");
		}
		sqlite3_finalize(statement);
	}
	NSLog(@"inserted");
}

-(void) read_video_review{
    NSLog(@"read_video_review");
	// Setup the database object
	sqlite3 *database;
    // Init the animals Array
	array_desp_video = [[NSMutableArray alloc] init];
    //NSLog(@"index in DB %@",index);
	// Open the database from the users filessytem
	if(sqlite3_open([databasePath UTF8String], &database) == SQLITE_OK) {
		// Setup the SQL Statement and compile it for faster access
		const char *sqlStatement = "select * from video_review";
		sqlite3_stmt *compiledStatement;
		if(sqlite3_prepare_v2(database, sqlStatement, -1, &compiledStatement, NULL) == SQLITE_OK) {
            
          while(sqlite3_step(compiledStatement) == SQLITE_ROW) {
                
                Video *obj=[[Video alloc]init];
				// Read the data from the result row
                obj.ReviewPersonName= [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 0)];
				obj.ReviewPersonEmail = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 1)];
				obj.ReviewPersonCity = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 2)];
                obj.tablequestionone = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 3)];
                obj.tablequestiontwo=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 4)];
                obj.tablequestionthree=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 5)];
                obj.ReviewCategory=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 6)];
                obj.ReviewVideoTitle=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 7)];
                obj.ReviewRate=[[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 8)] floatValue];
              
//                obj.user_id=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 9)];
                obj.VideoStatus=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 10)];
                obj.PostedDate=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 11)];
              
                obj.ReviewVideoName=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 12)];
              
				[self.array_desp_video addObject:obj];
              [obj release];
                NSLog(@"total count array_desp_video%d",[self.array_desp_video count]);
                
            }
        }      
        NSLog(@"total count array_desp_video %d",[array_desp_video count]);
        sqlite3_finalize(compiledStatement);
    }
    sqlite3_close(database);
    
}
-(void) readFreomDB{
	// Setup the database object
	sqlite3 *database;
    // Init the animals Array
	array_detail = [[NSMutableArray alloc] init];
    //NSLog(@"index in DB %@",index);
	// Open the database from the users filessytem
	if(sqlite3_open([databasePath UTF8String], &database) == SQLITE_OK) {
		// Setup the SQL Statement and compile it for faster access
		const char *sqlStatement = "select * from ReviewFrog";
		sqlite3_stmt *compiledStatement;
		if(sqlite3_prepare_v2(database, sqlStatement, -1, &compiledStatement, NULL) == SQLITE_OK) {
            
            //            sqlite3_bind_text(compiledStatement, 1, [index UTF8String], -1, NULL);
			// Loop through the results and add them to the feeds array
			while(sqlite3_step(compiledStatement) == SQLITE_ROW) {
                
                LoginInfo *obj=[[LoginInfo alloc]init];
				// Read the data from the result row
                
                char *isNil;
                isNil = (char *)sqlite3_column_text(compiledStatement, 0);
                
                if (isNil != nil) {
                obj.userbusinesslogo= [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 0)];
                }
                isNil = (char *)sqlite3_column_text(compiledStatement, 1);
                
                if (isNil != nil) {
                obj.BusinessSplash = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 1)];
                }
                isNil = (char *)sqlite3_column_text(compiledStatement, 2);
                
                if (isNil != nil) {
                obj.questionone = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 2)];
                }
                isNil = (char *)sqlite3_column_text(compiledStatement, 3);
                
                if (isNil != nil) {
                obj.questiontwo = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 3)];
                }
                isNil = (char *)sqlite3_column_text(compiledStatement, 4);
                
                if (isNil != nil) {
                obj.questionthree=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 4)];
                }
                isNil = (char *)sqlite3_column_text(compiledStatement, 5);
                
                if (isNil != nil) {
                obj.userbusinessid=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 5)]; 
                }
                
				[self.array_detail addObject:obj];
                
                NSLog(@"total count%d array_detail",[self.array_detail count]);
                
            }
        }      
        NSLog(@"total count array_detail%d",[array_detail count]);
        sqlite3_finalize(compiledStatement);
    }
    sqlite3_close(database);
    
}
-(void)readWriteView
{
	// Setup the database object
	sqlite3 *database;
    // Init the animals Array
	array_write = [[NSMutableArray alloc] init];

	if(sqlite3_open([databasePath UTF8String], &database) == SQLITE_OK) {
		// Setup the SQL Statement and compile it for faster access
		const char *sqlStatement = "select * from write_review";
		sqlite3_stmt *compiledStatement;
		if(sqlite3_prepare_v2(database, sqlStatement, -1, &compiledStatement, NULL) == SQLITE_OK) {
            
            //            sqlite3_bind_text(compiledStatement, 1, [index UTF8String], -1, NULL);
			// Loop through the results and add them to the feeds array
			while(sqlite3_step(compiledStatement) == SQLITE_ROW) {
                
                WriteObject *obj=[[WriteObject alloc]init];
				// Read the data from the result row
                char *isNil;
                isNil = (char *)sqlite3_column_text(compiledStatement, 0);
                
                if (isNil != nil) {
                obj.name= [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 0)];
                }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 1);
                 if (isNil != nil) {
				obj.email = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 1)];
                 }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 2);
				 if (isNil != nil) {
                obj.city = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 2)];
                 }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 3);
                 if (isNil != nil) {
                obj.q1 = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 3)];
                 }
                     
                isNil = (char *)sqlite3_column_text(compiledStatement, 4);
                 if (isNil != nil) {
                obj.q2=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 4)];
                 }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 5);
                if (isNil != nil) {
                obj.q3=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 5)];
                }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 6);
                if (isNil != nil) {
                obj.category=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 6)];
                    
                }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 7);
                if (isNil != nil) {
                obj.title=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 7)];
                }    
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 8);
                if (isNil != nil) {
                obj.desp=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 8)];
                }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 9);
                if (isNil != nil) {
                obj.rating=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 9)];
                }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 10);
                if (isNil != nil) {
                obj.userid=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 10)];
                }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 11);
                if (isNil != nil) {
                obj.status=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 11)];
                }
                
                isNil = (char *)sqlite3_column_text(compiledStatement, 12);
                if (isNil != nil) {
                obj.date=[NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 12)];
                }
				[self.array_write addObject:obj];
                
                NSLog(@"total count array_write%d",[self.array_write count]);
                
            }
        }      
        NSLog(@"total count array_write%d",[array_write count]);
        sqlite3_finalize(compiledStatement);
    }
    sqlite3_close(database);
    
}
-(void)categoryFromDBMethod
{
	sqlite3 *database;
    
	categoryFromDB = [[NSMutableArray alloc] init];
    
	if(sqlite3_open([databasePath UTF8String], &database) == SQLITE_OK) {

		const char *sqlStatement = "select * from Category";
		sqlite3_stmt *compiledStatement;
        
		if(sqlite3_prepare_v2(database, sqlStatement, -1, &compiledStatement, NULL) == SQLITE_OK) {
            
			while(sqlite3_step(compiledStatement) == SQLITE_ROW) {
                
                Category *objCategory =[[Category alloc]init];
                
//                objCategory.Category_UserID= [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 0)];
                
                char *isNil;
                isNil = (char *)sqlite3_column_text(compiledStatement, 1);
                
                if (isNil != nil) {
                
				objCategory.categoryname = [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 1)];
                }
                
                [self.categoryFromDB addObject:objCategory];
                
                [objCategory release];
            }
        }      
        NSLog(@"total count category%d",[self.categoryFromDB count]);
        sqlite3_finalize(compiledStatement);
    }
    sqlite3_close(database);
   
}
-(void)addCategory:(NSString *)userid category:(NSString *)category 
{	
	sqlite3 *database;
	NSLog(@"%@",category );
   
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "INSERT INTO Category (UserId,Category) VALUES (?,?)";
		sqlite3_stmt *statement;
		statement = [self PrepareStatement:sql];
		int a1 = sqlite3_bind_text(statement, 1, [userid UTF8String], -1, SQLITE_TRANSIENT);
		int a2 = sqlite3_bind_text(statement, 2, [category UTF8String], -1, SQLITE_TRANSIENT);

		if (statement)
		{
			if ( a1 != SQLITE_OK || a2 != SQLITE_OK)
			{
				sqlite3_finalize(statement);
				NSLog(@"not inserted");
				return;
			}
			NSLog(@"inserted1");
			sqlite3_step(statement);
			NSLog(@"inserted2");
		}
		sqlite3_finalize(statement);
	}
	NSLog(@"inserted");
}

- (void)updateNetwork:(Reachability*)curReach 
{
	
	NetworkStatus connection = [curReach currentReachabilityStatus];
	
	if(curReach == remoteHost){
		switch (connection)
		{
			case NotReachable:
			{
				self.remoteHostStatus = 0;
				break;
			}
			case ReachableViaWWAN:
			{
				self.remoteHostStatus = 1;
				break;
			}
			case ReachableViaWiFi:
			{
				self.remoteHostStatus = 2;
				break;
			}
		}
		
		if(remoteHostStatus != 0) {
			
            NSLog(@"0 connection"); // available
            
            [NSThread detachNewThreadSelector:@selector(threadMethodWrite) 
                                     toTarget:self 
                                   withObject:nil];
            
            [NSThread detachNewThreadSelector:@selector(threadMethodVideo) 
                                     toTarget:self 
                                   withObject:nil];
            
		} 
		NSLog(@"Connection 1 status : %d", remoteHostStatus);
	}
}

-(void)timercheckIsDataUploaded
{
    [NSTimer scheduledTimerWithTimeInterval:900.0f
                                           target:self
                                         selector:@selector(go)
                                         userInfo:nil
                                          repeats:YES];
}
-(void)go
{
    if (self.remoteHostStatus!=0) {
        [NSThread detachNewThreadSelector:@selector(threadMethodWrite) 
                                 toTarget:self 
                               withObject:nil];
        
        [NSThread detachNewThreadSelector:@selector(threadMethodVideo) 
                                 toTarget:self 
                               withObject:nil];
    }
    
}


-(void)threadMethodWrite
{
    NSLog(@"threadMethod write");
    [self readWriteView];
    if ([self.array_write count]>0) {
        for (i=0; i<[self.array_write count]; i++) {
            
            [self SubmitWriteReview];
        }
    }
}
-(void)threadMethodVideo
{
    NSLog(@" threadMethodVideo---");
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    self.userId=[prefs stringForKey:@"userid"];
    [self read_video_review];
    NSLog(@"Array ---> [self.array_desp_video count] %d ",[self.array_desp_video count]);
    
    if ([self.array_desp_video count]>0) {
        for (f=0; f<[self.array_desp_video count]; f++) {
            
            [self SubmitVideoReview];
        }
    // delete video desp ----
        [self DeleteVideo_DESP];
        
    // call api to get the id ---------- >>>>>
    
    [self SubmitVideo];
    
    // get array of local video.
    [self readVideoName];
    NSLog(@"array of local db video--? %d",[self.array_get_videoName count]);
    
    // Compare name from db and actual name.
    if ([self.array_get_videoName count]>0) {
            
    for (loop=0; loop<[self.array_get_videoName count]; loop++) {
        
        VideoObject *obj=[self.array_get_videoName objectAtIndex:loop];
        
         NSLog(@"obj.videoname %@",obj.videoname);
         NSLog(@"obj.videourl %@",obj.videourl);
        
        for (int k=0; k<[self.delarrVideoList count]; k++) {
            
            Video *objVideo = [self.delarrVideoList objectAtIndex:k];
            
            NSLog(@"obj.videoname----- %@",obj.videoname);
            NSLog(@"objVideo.ReviewVideoName %@",objVideo.ReviewVideoName);
            NSLog(@"objVideo.id-->%d",objVideo.id);
            if ([obj.videoname isEqualToString:objVideo.ReviewVideoName])
            {
                NSLog(@"objVideo.ReviewVideoName %@",objVideo.ReviewVideoName);
               // call video upload api -----
                NSURL *url = [NSURL URLWithString:obj.videourl];
                NSData *urldata=[[NSData alloc]initWithContentsOfURL:url];
                [self apiPostReviewVideo:objVideo.id VTitle:objVideo.ReviewVideoName video:urldata];
               
            }
          }
        }
      }  
        
    }
}

-(void) SubmitVideo
{
    NSLog(@"SubmitVideo ----");
    NSString *str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_Hotel_fetchUserVideo&UserID=%@&pagging=%d&version=2",self.ChangeUrl,self.userId,0];
    self.delarrVideoList=[[NSMutableArray alloc]init];
    
    NSLog(@"str--> to get the id %@",str);
    
    str = [str stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    
    NSLog(@"URLString=%@",str);	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	XMLVideo *parser = [[XMLVideo alloc] initXMLVideo];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		
        NSLog(@"Array count %d",[self.delarrVideoList count]);
        
	}
	else{
        
		NSLog(@"Error Error Error!!!");
        
	}

}


// SUBMIT video review from database ------------------

-(void) SubmitVideoReview

{
    NSLog(@"SubmitVideoReview ~~~~~~~~~~~~~~~~~~~~");
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    self.userId=[prefs stringForKey:@"userid"];
    NSLog(@"userid------- %@",userId);
    NSLog(@"SubmitVideoReview 000000");
    Video *obj=[self.array_desp_video objectAtIndex:f];
    
    // INTERNET availbale -------
    
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php&version=2",self.ChangeUrl]];
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    [request setHTTPMethod:@"POST"];

    
    
     NSData *requestBody = [[NSString stringWithFormat:@"api=itunesversion1_1_Hotel_Inserttblvideo&user_email=%@&dataname=%@&videofile=%@&uid=%@&date=%@&cityname=%@&status=%@&title=%@&rate=%.0f&Data_Category=%@&videoquestion1=%@&videoquestion2=%@&videoquestion3=%@",obj.ReviewPersonEmail,obj.ReviewPersonName,obj.ReviewVideoName,self.userId,obj.PostedDate,obj.ReviewPersonCity,obj.VideoStatus,obj.ReviewVideoTitle,obj.ReviewRate,obj.ReviewCategory,obj.tablequestionone,obj.tablequestiontwo,obj.tablequestionthree] dataUsingEncoding:NSUTF8StringEncoding];
    
    [request setHTTPBody:requestBody];
    
    NSLog(@"api=itunesversion1_1_Hotel_Inserttblvideo&user_email=%@&dataname=%@&videofile=%@&uid=%@&date=%@&cityname=%@&status=%@&title=%@&rate=%.0f&Data_Category=%@&videoquestion1=%@&videoquestion2=%@&videoquestion3=%@",obj.ReviewPersonEmail,obj.ReviewPersonName,obj.ReviewVideoName,self.userId,obj.PostedDate,obj.ReviewPersonCity,obj.VideoStatus,obj.ReviewVideoTitle,obj.ReviewRate,obj.ReviewCategory,obj.tablequestionone,obj.tablequestiontwo,obj.tablequestionthree);
    
    NSURLResponse *response = NULL;
    NSError *requestError = NULL;
    NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
    NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
    NSLog(@"responseString=%@",responseString);
    
    NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:responseData];
    
    //Initialize the delegate.
    xmlparser_para *parser = [[xmlparser_para alloc] initxmlparser_para];
    
    //Set delegate
    [xmlParser setDelegate:parser];
    
    //Start parsing the XML file.
    BOOL success = [xmlParser parse];
    
    if(success)
    {
        NSLog(@"DATA submitted No Errors video review submitted--------->");

    }	
    else
    {		
        NSLog(@"DAta NOT SUBMITTED . . . ");
//        UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"DATA submitted" delegate:self
//                                                 cancelButtonTitle:@"OK" otherButtonTitles:nil];
//        [altError show];
//        [altError release];
        
    }
	
}

// SUBMIT write review from database -----------

-(void) SubmitWriteReview
{	
    NSLog(@" SubmitWriteReview");
    @try{
    WriteObject *obj=[self.array_write objectAtIndex:i];

    obj.q1=[obj.q1 stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    obj.q2=[obj.q2 stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    obj.q3=[obj.q3 stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?version=2",ChangeUrl]];
    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url];
    [request setHTTPMethod:@"POST"];
    
    NSData *requestBody = [[NSString stringWithFormat:@"api=itunesversion1_1_Hotel_InserttblData&user_email=%@&Data_Name=%@&Desc=%@&uId=%@&Date=%@&Zipcode=%@&status=%@&Title=%@&Rate=%.0f&Data_Category=%@&answer1=%@&answer2=%@&answer3=%@",obj.email,obj.name,obj.desp,obj.userid,obj.date,obj.city,obj.status,obj.title,[obj.rating floatValue],obj.category,obj.q1,obj.q2,obj.q3]dataUsingEncoding:NSUTF8StringEncoding];
//    
    [request setHTTPBody:requestBody];
    
    NSLog(@"api=itunesversion1_1_Hotel_InserttblData&user_email=%@&Data_Name=%@&Desc=%@&uId=%@&Date=%@&Zipcode=%@&status=%@&Title=%@&Rate=%.0f&Data_Category=%@&answer1=%@&answer2=%@&answer3=%@",obj.email,obj.name,obj.desp,obj.userid,obj.date,obj.city,obj.status,obj.title,[obj.rating floatValue],obj.category,obj.q1,obj.q2,obj.q3);
    
    NSURLResponse *response = NULL;
    NSError *requestError = NULL;
    NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
    NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
    responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    
    NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
    NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
    NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
    
    if (rngXMLNode.length > 0)
        responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
    
    if (rngReturnStart.length > 0)
        responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
    
    if (rngReturnEnd.length > 0)
        responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
    responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    if ([responseString isEqualToString:@"1"])
    {	
        
        NSLog(@"submit data from db");
        
        
        // Delete data from DB 
        [self DeleteWriteRecord:obj.name];

    }
    else 
    {
        NSLog(@"Not submit  Done");	

    }
    }
    

    @catch (NSException *exception) {
        
        NSLog(@"Excertiion -- >%@",exception);
    }

    
    
	
}

//-------------------------------------

- (void) reachabilityChanged: (NSNotification* )note {
	
	Reachability* curReach = [note object];
	[self updateNetwork:curReach];
	
}
-(void) connect{
	NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
	NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents folder
	self.dataPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
	NSError *error;
	
	if (![[NSFileManager defaultManager] fileExistsAtPath:self.dataPath])
		[[NSFileManager defaultManager] createDirectoryAtPath:self.dataPath withIntermediateDirectories:NO attributes:nil error:&error]; //Create folder
}

-(int) saveVideoToDir{
    
    
    Video *video=[[Video alloc]init];
	NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
	NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents folder
	NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
	
	NSDateFormatter *format = [[NSDateFormatter alloc] init];
	[format setDateFormat:@"yyyy-MM-dd-HHmm"];
	NSDate *now = [[NSDate alloc] init];
	
	NSString *dateString = [format stringFromDate:now];
	NSLog(@"dateString: %@",dateString);
	
	if (self.flgReRecord==FALSE) {
		self.VideoName = [NSString stringWithFormat:@"%@_%@.mov",self.Data_Title,dateString];
	}
	else{
		self.flgReRecord=FALSE;
		self.flgReRecSub=TRUE;
		self.VideoName=self.Data_Title;
	}
		
    video.ReviewVideoName=self.VideoName;
    video.videoData=self.DataVideo;
    [self.array_video_in_offline addObject:video];
	NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,self.VideoName];
	NSLog(@"ExportPath>>>>>>>%@",exportPath);
	
	NSURL *exportUrl = [NSURL fileURLWithPath:exportPath];
	self.VideoPath = exportUrl;
	BOOL Sucess;
	Sucess = [[NSFileManager defaultManager] fileExistsAtPath:exportPath];		
	[self.DataVideo writeToFile:exportPath atomically:YES];
	self.createdVideoURL = exportUrl;
    // store video in db
    
    if (self.remoteHostStatus==0) {
        NSLog(@" getVideo ----");
    [self getVideo:self.VideoName videourl:[NSString stringWithFormat:@"%@",self.createdVideoURL]];
        
    }
    
	return 0;
}
-(int) saveVideoToBusinessDir
{
	
	NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
	NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents folder
	NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
	
	NSDateFormatter *format = [[NSDateFormatter alloc] init];
	[format setDateFormat:@"yyyy-MM-dd-HHmm"];
	NSDate *now = [[NSDate alloc] init];
		
	NSString *dateString = [format stringFromDate:now];
	NSLog(@"dateString: %@",dateString);
	
	if (self.flgBusinessReRecord==FALSE) {
		self.BusinessVideoName = [NSString stringWithFormat:@"%@_%@.mov",self.BusinessVideoTitle,dateString];
	}
	else{
		self.flgBusinessReRecord=FALSE;
		self.BusinessVideoName=self.BusinessVideoTitle;
	}
	
	
	NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,self.BusinessVideoName];
	NSLog(@"ExportPath>------%@",exportPath);
	
	NSURL *exportUrl = [NSURL fileURLWithPath:exportPath];
	self.BusinessVideoPath = exportUrl;
	BOOL Sucess;
	
	Sucess = [[NSFileManager defaultManager] fileExistsAtPath:exportPath];
	
	
	[self.BusinessDataVideo writeToFile:exportPath atomically:YES];
	
	self.createdVideoURL = exportUrl;
	return 0;
}

-(int) savePicToDir :(NSString *)picName picimage:(UIImage *)img{
		
	NSLog(@"PicString%@",picName);
	NSLog(@"image:-%@",img);
	self.DataPic = UIImagePNGRepresentation(img);
	
	NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
	NSString *documentsDirectory = [paths objectAtIndex:0]; // Get documents folder
	NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
	
	

	NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,picName];
	NSLog(@"ExportPath>%@",exportPath);
	
	NSURL *exportUrl = [NSURL fileURLWithPath:exportPath];
	self.picPath = exportUrl;
	BOOL Sucess;
	
	Sucess = [[NSFileManager defaultManager] fileExistsAtPath:exportPath];
	
	
	[self.DataPic writeToFile:exportPath atomically:YES];
	
	self.createdPicURL = exportUrl;
	
	return 0;
}

#pragma mark -
#pragma mark Api Review Video

- (BOOL)apiPostReviewVideo :(int)VideoID VTitle:(NSString *)VideoTitle video:(NSData *)ReveiwVideo 
{
    
    NSLog(@"VideoTitle apiPostReviewVideo -----> %@",VideoTitle);
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?version=2",self.ChangeUrl]];
	NSLog(@"URL========%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	NSLog(@"URL String:-%@",url);
	[request setURL:url];
	[request setHTTPMethod:@"POST"];
	NSString *api = @"UploadVideo";
		
	NSString *boundary = [NSString stringWithString:@"---------------------------14737809831466499882746641449"];
	//	[request setHTTPBody:postData];
	NSString *contentType = [NSString stringWithFormat:@"multipart/form-data; boundary=%@",boundary];
    [request setValue:contentType forHTTPHeaderField: @"Content-Type"];
	
	//	[request setValue:@"application/x-www-form-urlencoded" forHTTPHeaderField:@"Content-Type"];
	
	NSString *filename = [NSString stringWithFormat:@"%@",VideoTitle];
	//NSData *imageData = UIImagePNGRepresentation(Shruffleimage);
	NSMutableData *body = [NSMutableData data];
	for (int t=0; t<2; t++) {
		
		switch (t) {
			case 0:
			{	
				[body appendData:[[NSString stringWithFormat:@"--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"api\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[api dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
			case 1:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"videoid\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[[NSString stringWithFormat:@"%d",VideoID] dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
										
			default:
				break;
		}
		[body appendData:[[NSString stringWithFormat:@"\r\n--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
	}
	//    [body appendData:[[NSString stringWithFormat:@"\r\n--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
    [body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"videofile\"; filename=\"%@\"\r\n",filename]] dataUsingEncoding:NSUTF8StringEncoding]];
    [body appendData:[[NSString stringWithString:@"Content-Type: application/octet-stream\r\n\r\n"] dataUsingEncoding:NSUTF8StringEncoding]];
	//	[body appendData:postData];
    [body appendData:[NSData dataWithData:ReveiwVideo]];
    [body appendData:[[NSString stringWithFormat:@"\r\n--%@--\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
    [request setHTTPBody:body];
	NSString *postLength = [NSString stringWithFormat:@"%d", [body length]];
	[request setValue:postLength forHTTPHeaderField:@"Content-Length"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\%@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	//<?xml version="1.0" encoding="UTF-8"?>
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	if ([responseString isEqualToString:@"1"]) {
        NSLog(@" done done done ");
        [self DeleteVideoName:VideoTitle];
		return YES;
	} else {
		return NO;
	}	
}

#pragma mark -
#pragma mark Business Video

- (BOOL)apiPostBusinessVideo :(int)VideoID VTitle:(NSString *)VideoTitle video:(NSData *)ReveiwVideo 
{
//	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"http://www.turnkeymedicalmarketing.com/ReviewFrog/main/frmReviewFrogApi.php?"]];
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?version=2",self.ChangeUrl]];
	//NSLog(@"URL===%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	NSLog(@"URL String:-%@",url);
	[request setURL:url];
	[request setHTTPMethod:@"POST"];
	NSString *api = @"UploadBusinessVideo";
	
	NSString *boundary = [NSString stringWithString:@"---------------------------14737809831466499882746641449"];
	//	[request setHTTPBody:postData];
	NSString *contentType = [NSString stringWithFormat:@"multipart/form-data; boundary=%@",boundary];
    [request setValue:contentType forHTTPHeaderField: @"Content-Type"];

	
	NSString *filename = [NSString stringWithFormat:@"%@",VideoTitle];
	//NSData *imageData = UIImagePNGRepresentation(Shruffleimage);
	NSMutableData *body = [NSMutableData data];
	for (int i=0; i<2; i++) {
		
		switch (i) {
			case 0:
			{	
				[body appendData:[[NSString stringWithFormat:@"--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"api\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[api dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
			case 1:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"videoid\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[[NSString stringWithFormat:@"%d",VideoID] dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
				
			default:
				break;
		}
		[body appendData:[[NSString stringWithFormat:@"\r\n--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
	}

    [body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"videofile\"; filename=\"%@\"\r\n",filename]] dataUsingEncoding:NSUTF8StringEncoding]];
    [body appendData:[[NSString stringWithString:@"Content-Type: application/octet-stream\r\n\r\n"] dataUsingEncoding:NSUTF8StringEncoding]];
	//	[body appendData:postData];
    [body appendData:[NSData dataWithData:ReveiwVideo]];
    [body appendData:[[NSString stringWithFormat:@"\r\n--%@--\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
    [request setHTTPBody:body];
	NSString *postLength = [NSString stringWithFormat:@"%d", [body length]];
	[request setValue:postLength forHTTPHeaderField:@"Content-Length"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\%@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	if ([responseString isEqualToString:@"1"]) {
		return YES;
	} else {
		return NO;
	}	
}

#pragma mark -
#pragma mark Api Gallery
-(NSString *)apiGallery :(NSString *)userid
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"http://www.turnkeymedicalmarketing.com/ReviewFrog/main/frmReviewFrogApi_ver2.php?api=getgalleryinfo&ipaduserid=%@&version=2",userid]];
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
	return responseString;
}
#pragma mark -
#pragma mark Api Category

-(BOOL)apiCategory
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_CategorySelect_with_subcategory&userid=%@&business_id=%@&version=2",self.ChangeUrl,self.userId,self.userBusinessId]];
    
        
	NSLog(@"CategoryUrl:-%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
    
    NSLog(@"Response String == %@",responseString);
    
    NSLog(@"Stack Swipe ==  %@",self.sweepstake?@"Yes":@"No");
    
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	XMLCategory *parser = [[XMLCategory alloc] initXMLCategory];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	BOOL success = [xmlParser parse];
	
	if (success) {
        
        NSLog(@"%@",self.questions);
        
		return YES;
	}
	else {
		return NO;
	}
}

-(BOOL)checkResponse
{   
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=itunesversion1_1_syncstatus&user_id=%@&version=2",self.ChangeUrl,self.userId]];
    NSLog(@"checkResponse Appdelegate:-%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
    NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
    NSLog(@"responseString:\n%@",responseString);
	if ([responseString isEqualToString:@"1"]) {
		return YES;
	} else {
		return NO;
	}
    
}
-(NSString *) apiSearch:(NSString*)cat city:(NSString*)cityname
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=searchbusiness&category=%@&city=%@&version=2",self.set_APIurl,cat,cityname]];
	NSLog(@"searchbusiness-%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\n%@",responseString);
	return responseString;
	
}
-(NSString *) apiSearchResult:(int)Bid
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=businessdetail&businessid=%d&version=2",self.set_APIurl,Bid]];
	NSLog(@"SearchResult:-%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding]autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	NSLog(@"responesString:-%@", responseString);
	return responseString;
}
-(BOOL)apiInserGalleryinfo :(NSString *)name pemail:(NSString *)email comment:(NSString *)comment bpic:(NSString *)bpic apic:(NSString*)apic status:(NSString *)status useremail:(NSString *)useremail userid:(NSString*)userid
{
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"http://www.turnkeymedicalmarketing.com/ReviewFrog/main/frmReviewFrogApi_ver2.php?version=2"]];
	NSLog(@"galleryinfourl:-%@",url);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"POST"];
	
	NSString *post =[NSString stringWithFormat:@"api=insertgalleryinfo&patientname=%@&patientemail=%@&beforepic=%@&afterpic=%@&comments=%@&status=%@&ipaduseremail=%@&ipaduserid=%@",name,email,bpic,apic,comment,status,useremail,userid];
	NSLog(@"post:\n%@",post);
	NSData *postData = [post dataUsingEncoding:NSASCIIStringEncoding allowLossyConversion:YES];
	NSString *postLength = [NSString stringWithFormat:@"%d", [postData length]];
	
	[request setHTTPBody:postData];
	[request setValue:postLength forHTTPHeaderField:@"Content-Length"];
	[request setValue:@"application/x-www-form-urlencoded" forHTTPHeaderField:@"Content-Type"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSISO2022JPStringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	NSLog(@"responseString:\n%@",responseString);
	
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<galleryinfo><return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return></galleryinfo>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<galleryinfo><return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return></galleryinfo>" withString:@""];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	if ([responseString isEqualToString:@"1"]) {
		return YES;
	} else {
		return NO;
	}	
}

#pragma mark -
#pragma mark Upload
-(BOOL)apiUploadGalleryinfo :(NSString *)name pemail:(NSString *)email comment:(NSString *)comment bpic:(NSString *)bpic  status:(NSString *)status useremail:(NSString *)useremail userid:(NSString*)userid beforeimage:(UIImage *)bimage galleryid:(int)gid
{
   // NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_ver2.php?api=businessdetail&businessid=%d&version=2",self.set_APIurl,Bid]];
    
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_website_ver2.php?version=2",self.set_APIurl]];
	NSLog(@"uploadGalleryinfourl:-%@",url);
	NSLog(@"galleryid:-%d",gid);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"POST"];
	NSString *api = @"uploadgalleryinfo";
	NSString *boundary = [NSString stringWithString:@"---------------------------14737809831466499882746641449"];
	NSString *contentType = [NSString stringWithFormat:@"multipart/form-data; boundary=%@",boundary];
    [request setValue:contentType forHTTPHeaderField: @"Content-Type"];
		
	NSString *Beforefilename = [NSString stringWithFormat:@"%@",bpic];
	NSData *BeforeimageData = UIImagePNGRepresentation(bimage);
	
	
	NSMutableData *body = [NSMutableData data];
	for (int i=0; i<9; i++) {
		switch (i) {
			case 0:
			{	
				[body appendData:[[NSString stringWithFormat:@"--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"api\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[api dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
			case 1:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"patientname\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[name dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
			case 2:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"patientemail\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[email dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
			case 3:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"beforepic\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[bpic dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}		
			case 4:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"comments\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[comment dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
				
			}
			case 5:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"status\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[status dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
				
			}
			case 6:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"ipaduseremail\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[useremail dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
				
			}
			case 7:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"ipaduserid\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[userid dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
			case 8:
			{	
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"galleryid\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[[NSString stringWithFormat:@"%d",gid] dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
				
			default:
			break;
		}
		[body appendData:[[NSString stringWithFormat:@"\r\n--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
	}
    [body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"Beforepicfile\"; filename=\"%@\"\r\n",Beforefilename]] dataUsingEncoding:NSUTF8StringEncoding]];
	
    [body appendData:[[NSString stringWithString:@"Content-Type: application/octet-stream\r\n\r\n"] dataUsingEncoding:NSUTF8StringEncoding]];	
	[body appendData:[NSData dataWithData:BeforeimageData]];
	 [body appendData:[[NSString stringWithFormat:@"\r\n--%@--\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];	
    [request setHTTPBody:body];
	NSString *postLength = [NSString stringWithFormat:@"%d", [body length]];
	[request setValue:postLength forHTTPHeaderField:@"Content-Length"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\n%@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	
	if ([responseString isEqualToString:@"1"]) {
		return YES;
	} else {
		return NO;
	}	

}
-(BOOL)apiafterimageuploading :(int)gid apic:(NSString*)apic afterimage:(UIImage *)aimage
{
    	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"%@frmReviewFrogApi_website_ver2.php?version=2",self.set_APIurl]];

	
	NSLog(@"apiafterimageuploading:-%@",url);
	NSLog(@"galleryid:-%d",gid);
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:url];
	[request setHTTPMethod:@"POST"];
	NSString *api = @"afterimageuploading";
	NSString *boundary = [NSString stringWithString:@"---------------------------14737809831466499882746641449"];
	NSString *contentType = [NSString stringWithFormat:@"multipart/form-data; boundary=%@",boundary];
    [request setValue:contentType forHTTPHeaderField: @"Content-Type"];
	
	NSString *Afterfilename = [NSString stringWithFormat:@"%@",apic];
	NSData *AfterimageData = UIImagePNGRepresentation(aimage);
	
	
	NSMutableData *body = [NSMutableData data];
	for (int i=0; i<3; i++) {
		switch (i) {
			case 0:
			{	
				[body appendData:[[NSString stringWithFormat:@"--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"api\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[api dataUsingEncoding:NSUTF8StringEncoding]]];
				break;
			}
			case 1:
			{	
				 [body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"afterpic\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				 [body appendData:[NSData dataWithData:[apic dataUsingEncoding:NSUTF8StringEncoding]]];
				 break;
			}
			case 2:
			{
				[body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"galleryid\"\r\n\r\n"]] dataUsingEncoding:NSUTF8StringEncoding]];
				[body appendData:[NSData dataWithData:[[NSString stringWithFormat:@"%d",gid] dataUsingEncoding:NSUTF8StringEncoding]]];			
				
				break;
			}	
							
			default:
				break;
		}
		[body appendData:[[NSString stringWithFormat:@"\r\n--%@\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
	}
	
    [body appendData:[[NSString stringWithString:[NSString stringWithFormat:@"Content-Disposition: form-data; name=\"Afterpicfile\"; filename=\"%@\"\r\n",Afterfilename]] dataUsingEncoding:NSUTF8StringEncoding]];
	
    [body appendData:[[NSString stringWithString:@"Content-Type: application/octet-stream\r\n\r\n"] dataUsingEncoding:NSUTF8StringEncoding]];	
	[body appendData:[NSData dataWithData:AfterimageData]];
	[body appendData:[[NSString stringWithFormat:@"\r\n--%@--\r\n",boundary] dataUsingEncoding:NSUTF8StringEncoding]];
	
    [request setHTTPBody:body];
	NSString *postLength = [NSString stringWithFormat:@"%d", [body length]];
	[request setValue:postLength forHTTPHeaderField:@"Content-Length"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString:\n%@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\"?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	
	if ([responseString isEqualToString:@"1"]) {
		return YES;
	} else {
		return NO;
	}	
	
	
}

#pragma mark -
#pragma mark Database methods
-(void) dbconnect{
	// Setup some globals
	self.databaseName = @"ReviewVideo.sqlite";
	
	// Get the path to the documents directory and append the databaseName
	NSArray *documentPaths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
	NSString *documentsDir = [documentPaths objectAtIndex:0];
	self.databasePath = [documentsDir stringByAppendingPathComponent:self.databaseName];
	
	// Execute the "checkAndCreateDatabase" function
	
	[self checkAndCreateDatabase];
}
//Database Function

-(void) checkAndCreateDatabase{
	// Check if the SQL database has already been saved to the users phone, if not then copy it over
	BOOL success;
	NSFileManager *fileManager = [NSFileManager defaultManager];
	
	// Check if the database has already been created in the users filesystem
	success = [fileManager fileExistsAtPath:databasePath];
	
	// If the database already exists then return without doing anything
	if(success) {
		//[fileManager removeFileAtPath:databasePath handler:nil]; //************************************************************
		return;
	}
	// If not then proceed to copy the database from the application to the users filesystem
	
	// Get the path to the database in the application package
	NSString *databasePathFromApp = [[[NSBundle mainBundle] resourcePath] stringByAppendingPathComponent:databaseName];
	
	// Copy the database from the package to the users filesystem
	[fileManager copyItemAtPath:databasePathFromApp toPath:self.databasePath error:nil];
	
	[fileManager release];
}
//Database Function
-(sqlite3_stmt*) PrepareStatement:(const char *)sql
{
	// Setup the database object
	sqlite3 *database;
	// Init the animals Array
	
	// Open the database from the users filessytem
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		// Setup the SQL Statement and compile it for faster access
		const char *sqlStatement = sql;
		sqlite3_stmt *compiledStatement;
		
		if(sqlite3_prepare_v2(database, sqlStatement,-1, &compiledStatement, NULL) == SQLITE_OK) {
						return compiledStatement;
			
		}
	}
	return nil;
}

-(void)VideoInfoSave:(NSString *)strvideoName{
	
	sqlite3 *database;
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "INSERT INTO ReviewVideo (videoname) VALUES (?)";
		sqlite3_stmt *statement;
		
		
		statement = [self PrepareStatement:sql];
		
		int a1 = sqlite3_bind_text(statement, 1, [strvideoName UTF8String], -1, SQLITE_TRANSIENT);
						
		if (statement)
		{
			if (a1 != SQLITE_OK )
			{
				sqlite3_finalize(statement);
				NSLog(@"not inserted");
				return;
			}
			
			sqlite3_step(statement);
		}
		sqlite3_finalize(statement);
	}	
	[self GetAllVideo];
}


-(void)getVideo:(NSString *)strvideoName videourl:(NSString *)strvideoURL{
	
	sqlite3 *database;
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "INSERT INTO get_video (videoname,videourl) VALUES (?,?)";
		sqlite3_stmt *statement;
		
		
		statement = [self PrepareStatement:sql];
		
		int a1 = sqlite3_bind_text(statement, 1, [strvideoName UTF8String], -1, SQLITE_TRANSIENT);
        
        int a2 = sqlite3_bind_text(statement, 2, [strvideoURL UTF8String], -1, SQLITE_TRANSIENT);
        
		if (statement)
		{
			if ( a1 != SQLITE_OK || a2 != SQLITE_OK)
			{
				sqlite3_finalize(statement);
				NSLog(@"not inserted");
				return;
			}
			NSLog(@"inserted1");
			sqlite3_step(statement);
			NSLog(@"inserted2");
		}
		sqlite3_finalize(statement);
	}
	NSLog(@"inserted");
}
-(void)readVideoName
{

        // Setup the database object
        sqlite3 *database;
        // Init the animals Array
        array_get_videoName = [[NSMutableArray alloc] init];
        
        if(sqlite3_open([databasePath UTF8String], &database) == SQLITE_OK) {
            // Setup the SQL Statement and compile it for faster access
            const char *sqlStatement = "select * from get_video";
            sqlite3_stmt *compiledStatement;
            if(sqlite3_prepare_v2(database, sqlStatement, -1, &compiledStatement, NULL) == SQLITE_OK) {
                
                while(sqlite3_step(compiledStatement) == SQLITE_ROW) {
                    
                    VideoObject *obj=[[VideoObject alloc]init];
                    // Read the data from the result row
                    char *isNil;
                    isNil = (char *)sqlite3_column_text(compiledStatement, 0);
                    
                    if (isNil != nil) {
                    obj.videoname= [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 0)];
                    }
                    
                    isNil = (char *)sqlite3_column_text(compiledStatement, 1);
                    
                    if (isNil != nil) {
                    obj.videourl= [NSString stringWithUTF8String:(char *)sqlite3_column_text(compiledStatement, 1)];
                    }
                    
                    [self.array_get_videoName addObject:obj];
                    
                    NSLog(@"total count%d",[self.array_get_videoName count]);
                }
            }      
            NSLog(@"total count%d",[array_get_videoName count]);
            sqlite3_finalize(compiledStatement);
        }
        sqlite3_close(database);
}
/*-(void)ReviewCategoryList:(NSString *)strCategory{
	
	NSLog(@"CategoryName:-%@",strCategory);
	
	sqlite3 *database;
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "INSERT INTO reviewcategory (categoryname) VALUES (?)";
		sqlite3_stmt *statement;
		
		statement = [self PrepareStatement:sql];
		
		int a1 = sqlite3_bind_text(statement, 1, [strCategory UTF8String], -1, SQLITE_TRANSIENT);
		
		if (statement)
		{
			if (a1 != SQLITE_OK )
			{
				sqlite3_finalize(statement);
				NSLog(@"not inserted");
				return;
			}
			
			sqlite3_step(statement);
		}
		sqlite3_finalize(statement);
	}	
	
}*/

/*-(void)LoadReviewCategory
{
	self.ArrCategoryList = [[NSMutableArray alloc]init];
	
	sqlite3 *database;
	
	if(sqlite3_open([databasePath UTF8String], &database) == SQLITE_OK) {
		
		const char *sqlStatement = "select * from reviewcategory";
		sqlite3_stmt *compiledStatement;
		if(sqlite3_prepare_v2(database, sqlStatement, -1, &compiledStatement, NULL) == SQLITE_OK) {
			// Loop through the results and add them to the feeds array
			while(sqlite3_step(compiledStatement) == SQLITE_ROW) {
				
				Category *acategory=[[Category alloc] init];
				int Categoryid = sqlite3_column_int(compiledStatement, 0);
				NSString *Categoryname = 	[[NSString alloc] initWithUTF8String:(const char *) sqlite3_column_text(compiledStatement, 1)];
				//NSLog(@"DailyFood_Name %@ ",Categoryname);
				[acategory setCategoryid:Categoryid];
                [acategory setCategoryname:Categoryname];
				
                [self.ArrCategoryList addObject:acategory];
                [acategory release];
				
			}
		}
		
		sqlite3_finalize(compiledStatement);
	}
	sqlite3_close(database);
	
}*/

//Delete news from database 
-(void)DeleteCategory :(int)categoryid
{
	NSString *strcategoryid = [NSString stringWithFormat:@"%d",categoryid];
	NSLog(@"categorystr:%@",strcategoryid);
	
	sqlite3 *database;
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "Delete from reviewcategory where categoryid = ?";
		sqlite3_stmt *statement;
		statement = [self PrepareStatement:sql];	
		int a1 = sqlite3_bind_text(statement, 1, [strcategoryid UTF8String], -1, SQLITE_TRANSIENT);
		
		if (statement)
		{
			if (a1 != SQLITE_OK )
			{
				sqlite3_finalize(statement);
				return;
			}
			sqlite3_step(statement);
		}
		sqlite3_finalize(statement);
	}	
	
}

-(void)DeleteWriteRecord :(NSString *)name
{
//	NSString *strcategoryid = [NSString stringWithFormat:@"%d",categoryid];
//	NSLog(@"categorystr:%@",strcategoryid);
	
	sqlite3 *database;
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "Delete from write_review where name = ?";
		sqlite3_stmt *statement;
		statement = [self PrepareStatement:sql];	
		int a1 = sqlite3_bind_text(statement, 1, [name UTF8String], -1, SQLITE_TRANSIENT);
		
		if (statement)
		{
			if (a1 != SQLITE_OK )
			{
				sqlite3_finalize(statement);
				return;
			}
			sqlite3_step(statement);
		}
		sqlite3_finalize(statement);
	}	
	
}


-(void)DeleteVideo_DESP
{
    //	NSString *strcategoryid = [NSString stringWithFormat:@"%d",categoryid];
    //	NSLog(@"categorystr:%@",strcategoryid);
	
    sqlite3 *database;
	// NSLog(@" evename delete-- %@",eveName );
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "Delete from video_review";
		sqlite3_stmt *statement;
		//localpath =@"abc";
		statement = [self PrepareStatement:sql];    
		//int a1 = sqlite3_bind_int(statement, 1, eveId);
		//NSLog(SQLITE_OK);
		if (statement)
		{
			sqlite3_step(statement);
			NSLog(@"Deleted video_review -vandanananananan------");
			
		}
		sqlite3_finalize(statement);
	}  
	
}

-(void)DeleteVideoName :(NSString *)name
{
    //	NSString *strcategoryid = [NSString stringWithFormat:@"%d",categoryid];
    //	NSLog(@"categorystr:%@",strcategoryid);
	
    NSLog(@"Delete video Name:-----%@",name);
	sqlite3 *database;
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "Delete from get_video where videoname = ?";
		sqlite3_stmt *statement;
		statement = [self PrepareStatement:sql];	
		int a1 = sqlite3_bind_text(statement, 1, [name UTF8String], -1, SQLITE_TRANSIENT);
		
		if (statement)
		{
			if (a1 != SQLITE_OK )
			{
				sqlite3_finalize(statement);
				return;
			}
			sqlite3_step(statement);
            NSLog(@"DELETED video nameeee");
		}
		sqlite3_finalize(statement);
	}	
}


/*-(void)UpDateCategory:(int)categoryid cname:(NSString *)Categoryname{
	
	sqlite3 *database;
	NSLog(@"categoryname:-%@",Categoryname);
	NSString *strcategoryid = [NSString stringWithFormat:@"%d",categoryid];
	NSLog(@"categorystr:%@",strcategoryid);
	
	
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "update reviewcategory set categoryname = ?Where categoryid = ?";
		sqlite3_stmt *UpdateStatement;
		
		UpdateStatement = [self PrepareStatement:sql];
		int a1 = sqlite3_bind_int(UpdateStatement, 2, categoryid);
		int a2 = sqlite3_bind_text(UpdateStatement, 1, [Categoryname UTF8String], -1, SQLITE_TRANSIENT);		
		
		if (UpdateStatement)
		{
			if (a1 != SQLITE_OK || a2 != SQLITE_OK)
			{
				sqlite3_finalize(UpdateStatement);
				NSLog(@"not inserted");
				return;
			}
			sqlite3_step(UpdateStatement);
		}
		sqlite3_finalize(UpdateStatement);
	}	
	
}*/


- (void)getUserPreferences {
    
	NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
	self.poststatus = [prefs stringForKey:@"post_status"];
	if (self.poststatus == nil) self.poststatus = @"A";
	
	self.UserEmail = [prefs stringForKey:@"UserEmail"];
	NSLog(@"USEREMAIL-------%@",self.UserEmail);
	if (self.UserEmail == nil) self.UserEmail = @"";
	
	self.SwitchFlag = [prefs boolForKey:@"SwitchFlag"];
	
	self.admin_email = [prefs stringForKey:@"AdminEmail"];
	NSLog(@"ADMINEMAIL--------%@",self.admin_email);
	if (self.admin_email == nil) self.admin_email = @"";
	
	//self.dataSplash = [prefs dataForKey:@"splashimage"];

	self.strDefaultCategory = [prefs stringForKey:@"DefaultCategory"];
	NSLog(@"Cagegory:=%@",self.strDefaultCategory);
	
	self.flggiveaway = [prefs boolForKey:@"Givewayimage"];
	
	self.flgswich = [prefs boolForKey:@"flgswich"];
    self.userId=[prefs stringForKey:@"userid"];
    self.userBusinessId = [prefs stringForKey:@"BusinessId"];
    NSLog(@"USer Businessid:------->%@",self.userBusinessId);
    self.pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    self.pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
}
- (void)resetIdleTimer {
    
    if (idleTimer) {
        [idleTimer invalidate];
        idleTimer=nil;
    }
	
    idleTimer = [[NSTimer scheduledTimerWithTimeInterval:60.0 target:self selector:@selector(idleTimerExceeded) userInfo:nil repeats:YES] retain];
}

- (void)idleTimerExceeded {
    NSLog(@"idle time exceeded");
	[self playMovie];
}

-(void)playMovie
{
	[audioPlayer play];
}

- (UIImage *)imageByScalingProportionallyToSize:(CGSize)targetSize srcImg:(UIImage *)image{
	
	UIImage *sourceImage = image;
	UIImage *newImage = nil;
	
	CGSize imageSize = sourceImage.size;
	CGFloat width = imageSize.width;
	CGFloat height = imageSize.height;
	
	CGFloat targetWidth = targetSize.width;
	CGFloat targetHeight = targetSize.height;
	
	CGFloat scaleFactor = 0.0;
	CGFloat scaledWidth = targetWidth;
	CGFloat scaledHeight = targetHeight;
	
	CGPoint thumbnailPoint = CGPointMake(0.0,0.0);
	
	if (CGSizeEqualToSize(imageSize, targetSize) == NO) {
		
        CGFloat widthFactor = targetWidth / width;
        CGFloat heightFactor = targetHeight / height;
		
        if (widthFactor < heightFactor) 
			scaleFactor = widthFactor;
        else
			scaleFactor = heightFactor;
		
        scaledWidth  = width * scaleFactor;
        scaledHeight = height * scaleFactor;
		
        // center the image
		
        if (widthFactor < heightFactor) {
			thumbnailPoint.y = (targetHeight - scaledHeight) * 0.5; 
        } else if (widthFactor > heightFactor) {
			thumbnailPoint.x = (targetWidth - scaledWidth) * 0.5;
        }
	}
	
	// this is actually the interesting part:
	
	UIGraphicsBeginImageContext(targetSize);
	
	CGRect thumbnailRect = CGRectZero;
	thumbnailRect.origin = thumbnailPoint;
	thumbnailRect.size.width  = scaledWidth;
	thumbnailRect.size.height = scaledHeight;
	
	[sourceImage drawInRect:thumbnailRect];
	
	newImage = UIGraphicsGetImageFromCurrentImageContext();
	UIGraphicsEndImageContext();
	
	if(newImage == nil) NSLog(@"could not scale image");
	
	return newImage ;
}

- (void)saveUserPreferences :(int)Preferences
{
	NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
	if(Preferences==0)
	{
		[prefs setObject:self.UserEmail forKey:@"UserEmail"];
	}
	else if(Preferences==1)
	{
		[prefs setObject:self.poststatus forKey:@"post_status"];
	
	}
	else if(Preferences==2)
	{
		[prefs setBool:self.SwitchFlag forKey:@"SwitchFlag"];
		
	}
	else if(Preferences==3)
	{
		[prefs setObject:self.admin_email forKey:@"AdminEmail"];
	}
		
	[prefs synchronize];
}


- (void) readSettings {
	
	NSString *path = [[NSBundle mainBundle] bundlePath];
	NSString *finalPath = [path stringByAppendingPathComponent:@"Settings.plist"];
	NSDictionary *plistData = [[NSDictionary dictionaryWithContentsOfFile:finalPath] retain];
			
	self.set_UserEmail = [plistData valueForKey:@"UserEmail"];
	self.set_APIurl = [plistData valueForKey:@"APIurl"];
    self.websiteAPIurl = [plistData valueForKey:@"WebsiteAPIurl"];
	self.Set_BusinessUrl = [plistData valueForKey:@"Businessurl"];
    self.Set_Termurl = [plistData valueForKey:@"Termurl"];
    self.ChangeUrl =[plistData valueForKey:@"ChangeUrl"];
    self.ChangeBusiness=[plistData valueForKey:@"ChangeBusiness"];
	
}
-(void)DeleteCategory
{
    sqlite3 *database;
	// NSLog(@" evename delete-- %@",eveName );
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "Delete from Category";
		sqlite3_stmt *statement;
		//localpath =@"abc";
		statement = [self PrepareStatement:sql];    
		//int a1 = sqlite3_bind_int(statement, 1, eveId);
		//NSLog(SQLITE_OK);
		if (statement)
		{
			sqlite3_step(statement);
			NSLog(@"Deleted");
			
		}
		sqlite3_finalize(statement);
	}  
}
-(void)DeleteQueInfo
{
    sqlite3 *database;
	// NSLog(@" evename delete-- %@",eveName );
	if(sqlite3_open([self.databasePath UTF8String], &database) == SQLITE_OK) {
		const char* sql = "Delete from ReviewFrog";
		sqlite3_stmt *statement;
		//localpath =@"abc";
		statement = [self PrepareStatement:sql];   
        
		if (statement)
		{
			sqlite3_step(statement);
			NSLog(@"Deleted");
			
		}
		sqlite3_finalize(statement);
	}  
    
}

- (void)dealloc {
	[[NSNotificationCenter defaultCenter] removeObserver:self];

    [viewController release];
    [window release];
    [super dealloc];
}




@end
