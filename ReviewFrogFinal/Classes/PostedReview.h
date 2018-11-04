//
//  PostedReview.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 13/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class Review_FrogAppDelegate;
@interface PostedReview : UIButton {
	
	Review_FrogAppDelegate *appDelegate;
	BOOL isChecked;


}

@property (nonatomic,assign) BOOL isChecked;

-(IBAction)postedReviewClicked;

@end
