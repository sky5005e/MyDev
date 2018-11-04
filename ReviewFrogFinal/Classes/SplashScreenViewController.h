//
//  SplashScreenViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 7/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"

@class Review_FrogAppDelegate;
@interface SplashScreenViewController : UIViewController {
	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UIImageView *imgSplash; 
	
	UIImage *Bsplashimage;
	
	NSTimer *TimeProcess;

}
@property(nonatomic, retain) UIImage *Bsplashimage;
-(void)GoToNextScreen;
@end
