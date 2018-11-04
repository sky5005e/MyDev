//
//  suggestionViewController.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 11/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"


@interface suggestionViewController : UIViewController {
	
	
	Review_FrogAppDelegate *appDelegate;
	
	
	IBOutlet UITextView *txtSuggestion;
	
	IBOutlet UIButton *btncontinue;
	IBOutlet UIButton *btnadmin;
}

@property (nonatomic, retain) IBOutlet UITextView *txtSuggestion;
@property (nonatomic, retain) IBOutlet UIButton *btncontinue;



-(IBAction) submitsuggestion;
-(IBAction)Home;
-(IBAction) TERMS_CONDITIONS;
-(IBAction) Admin;
-(IBAction) BackClick;


@end


