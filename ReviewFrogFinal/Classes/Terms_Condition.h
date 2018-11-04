//
//  Terms_Condition.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 31/12/10.
//  Copyright 2010 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "xmlparser_para.h"

@class Review_FrogAppDelegate;
@interface Terms_Condition : UIViewController<UIWebViewDelegate> {
    IBOutlet UITextView *txtNote;
	BOOL isChecked;
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UIButton *TermsButton;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIButton *btnadmin;
    IBOutlet UIWebView *webTerm;
    IBOutlet UIActivityIndicatorView *ActiviIndicator;
}
@property (nonatomic, retain) IBOutlet UITextView *txtNote;
@property (nonatomic, assign) BOOL isChecked;
@property (nonatomic, retain) IBOutlet UIButton *TermsButton;
@property (nonatomic, retain) UIWebView *webTerm;
@property (nonatomic, retain) UIActivityIndicatorView *ActiviIndicator;

-(IBAction) Home;
-(IBAction) Admin;

@end
