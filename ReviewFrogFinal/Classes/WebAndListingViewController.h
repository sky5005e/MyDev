//
//  WebAndListingViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 7/25/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"

@class Review_FrogAppDelegate;
@interface WebAndListingViewController : UIViewController {

		Review_FrogAppDelegate *appDelegate;
}


-(IBAction) LocalBusinesSearchClick;
-(IBAction) BusinessListingCustomerClick;

@end
