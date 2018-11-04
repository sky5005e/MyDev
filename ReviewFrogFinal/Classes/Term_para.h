//
//  Term_para.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 07/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "xmlparser_para.h"
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"
#import "ASIHTTPRequest.h"

#import "ASIFormDataRequest.h"

@interface Term_para : UIViewController<UIWebViewDelegate,ASIHTTPRequestDelegate,MBProgressHUDDelegate> {
    UIWindow *window;
	BOOL submitflag;
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UIButton *sumhidde; 
	IBOutlet UIButton *btncontinue3;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgtxtPowerdby;
	IBOutlet UIImageView *imgGiveAway;
	IBOutlet UIButton *btnadmin;
    MBProgressHUD *HUD;
    IBOutlet UIWebView *webTerm;
    BOOL isUploaded;
    IBOutlet UIActivityIndicatorView *ActivIndicator;
    
}
@property BOOL submitflag;
@property (nonatomic, retain) IBOutlet UIWindow *window;
@property (nonatomic, retain) IBOutlet UIButton *sumhidde;
@property (nonatomic, retain) IBOutlet UIButton *btncontinue3;
@property (nonatomic, retain) UIActivityIndicatorView *ActivIndicator;
@property (nonatomic, retain) Video *avideo;

-(IBAction)SubmitClcik;
-(IBAction)Home;
//-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(void) SubmitWriteReview;
-(void) SubmitVideoReview;
-(void) loadingVideoStart;
-(IBAction) Backtohome;
-(void)writeReviewLocal;
-(void)videoStoreInDB;
@end
