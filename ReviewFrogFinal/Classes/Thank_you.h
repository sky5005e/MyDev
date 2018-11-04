//
//  Thank_you.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 07/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogViewController.h"
@interface Thank_you : UIViewController {
	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgtxtPowerdby;
	IBOutlet UIImageView *imgGiveAway;

}

-(IBAction)Home;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(void)idleTimerExceededForThanks;
@end
