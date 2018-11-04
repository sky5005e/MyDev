//
//  ClientBusinessSplashScreemViewController.h
//  Review Frog
//
//  Created by agilepc-32 on 10/18/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"


@interface ClientBusinessSplashScreemViewController : UIViewController {
	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UIImageView *Bsplash;
	NSTimer *TimeProcess;
	UIImage *imgB;
	

}
@property (nonatomic, retain) IBOutlet UIImageView *Bsplash;
@property (nonatomic, retain) UIImage *imgB;
@end
