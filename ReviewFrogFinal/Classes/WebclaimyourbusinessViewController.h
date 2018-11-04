//
//  WebclaimyourbusinessViewController.h
//  Review Frog
//
//  Created by agilepc-32 on 10/24/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BusinessSearchListViewController.h"
#import "Review_FrogAppDelegate.h"

@interface WebclaimyourbusinessViewController : UIViewController<UIWebViewDelegate> {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSURLRequest *urlReq;
	IBOutlet UIWebView *web;
	IBOutlet UIActivityIndicatorView *ActiviIndicator;
	IBOutlet UIButton *btnBack;
	int businessid;
	
}

@property (nonatomic, retain) UIActivityIndicatorView *ActiviIndicator;
@property int businessid;

-(IBAction) BackClcik;

@end
