//
//  CellResult.h
//  Review Frog
//
//  Created by agilepc-32 on 10/10/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>


@interface CellResult : UIViewController {
	
	
	IBOutlet UILabel *lblBusinessName;
	IBOutlet UILabel *lblBusinessAdd;
	IBOutlet UILabel *lblReviewCounter;
	IBOutlet UILabel *lblNo;
	IBOutlet UILabel *lblDescription;
	IBOutlet UITextView *txtDescription;
	
	IBOutlet UILabel *lblReviewTitle;
	IBOutlet UILabel *lblPostedbyName;
	
	IBOutlet UILabel *lblDate;
	
	IBOutlet UIImageView *imgReview;
	
	IBOutlet UIImageView *imgGivenRating;
	IBOutlet UIImageView *imgNoRating;
	IBOutlet UIImageView *imghalfRating;
	
}

@property (nonatomic, retain) UILabel *lblBusinessName;
@property (nonatomic, retain) UILabel *lblBusinessAdd;
@property (nonatomic, retain) UILabel *lblReviewCounter;
@property (nonatomic, retain) UILabel *lblNo;
@property (nonatomic, retain) UILabel *lblDescription;

@property (nonatomic, retain) UILabel *lblReviewTitle;
@property (nonatomic, retain) UILabel *lblPostedbyName;

@property (nonatomic, retain) UILabel *lblDate;

@property (nonatomic, retain) UIImageView *imgReview;

@property (nonatomic, retain) UIImageView *imgGivenRating;
@property (nonatomic, retain) UIImageView *imgNoRating;
@property (nonatomic, retain) UIImageView *imghalfRating;
@property (nonatomic, retain) UITextView *txtDescription;

@end
