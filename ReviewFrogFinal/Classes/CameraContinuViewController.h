//
//  CameraContinuViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 6/17/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"
#import "VideoListViewController.h"


@class Term_para;
@interface CameraContinuViewController : UIViewController <UIImagePickerControllerDelegate,UIVideoEditorControllerDelegate>{
	
	Review_FrogAppDelegate *appDelegate;
	NSString *dataPath;
	IBOutlet UIButton *btnList;
	NSData *DataVideo;
	NSString *tempExportPath;
	NSString *videoName;
	NSURL *createdVideoURL;
	NSString *strVideo;
	BOOL termpage;
	NSTimer *timeprocess1;
	
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgtxtPowerdby;
	
}

@property (nonatomic, retain) IBOutlet UIButton *btnList;
@property (nonatomic, retain) NSString *dataPath;
@property (nonatomic, retain) NSData *DataVideo;
@property (nonatomic, retain) NSString *tempExportPath;
@property (nonatomic, retain) NSString *videoName;
@property (nonatomic, retain) NSURL *createdVideoURL;
@property (nonatomic, retain) NSString *strVideo;
@property BOOL termpage;

//Db Objects
@property (nonatomic, retain) NSString *databaseName;
@property (nonatomic, retain) NSString *databasePath;

-(IBAction) Continue_Click;
-(IBAction) Backtohome;
-(IBAction)Home;
-(IBAction) Admin;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Switch_Click;

-(void) connect;
-(IBAction) MoveToPlayList;

-(void) SubmitVideo;

@end