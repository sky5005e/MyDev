//
//  CellSearch.h
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"


@interface CellSearch : UIViewController {
	
	IBOutlet UILabel *lblbusinessname;
	IBOutlet UILabel *lblbusinessAdd;
	IBOutlet UILabel *lblReviewCount;
	IBOutlet UILabel *lblServices;
	IBOutlet UILabel *lblAccreditations;
	IBOutlet UILabel *lblNo;
	IBOutlet UILabel *lblPhone;
	
	
	IBOutlet UIImageView *imgGivenRating;
	IBOutlet UIImageView *imgNoRating;
	IBOutlet UIImageView *imghalfRating;
	
	IBOutlet UIImageView *imgcheck;
	
	IBOutlet UIButton *btnCliamyourbusiness;

	IBOutlet UILabel *mon_am; 
	IBOutlet UILabel *mon_pm; 
	IBOutlet UILabel *tue_am; 
	IBOutlet UILabel *tue_pm; 
	IBOutlet UILabel *wed_am; 
	IBOutlet UILabel *wed_pm; 
	IBOutlet UILabel *thru_am;
	IBOutlet UILabel *thru_pm;
	IBOutlet UILabel *fri_am; 
	IBOutlet UILabel *fri_pm; 
	IBOutlet UILabel *sat_am; 
	IBOutlet UILabel *sat_pm; 
	IBOutlet UILabel *sun_am;
	IBOutlet UILabel *sun_pm; 
}
@property(nonatomic, retain) UILabel *lblbusinessname;
@property(nonatomic, retain) UILabel *lblbusinessAdd;
@property(nonatomic, retain) UILabel *lblReviewCount;
@property(nonatomic, retain) UILabel *lblNo;
@property(nonatomic, retain) UILabel *lblPhone; 

@property(nonatomic, retain) UILabel *lblServices;
@property(nonatomic, retain) UILabel *lblAccreditations;

@property (nonatomic, retain) UIButton *btnCliamyourbusiness;



@property(nonatomic, retain) UIImageView *imgGivenRating;
@property(nonatomic, retain) UIImageView *imgNoRating;
@property(nonatomic, retain) UIImageView *imghalfRating;
@property(nonatomic, retain) UIImageView *imgcheck;

@property(nonatomic, retain) UILabel *mon_am; 
@property(nonatomic, retain) UILabel *mon_pm; 
@property(nonatomic, retain) UILabel *tue_am; 
@property(nonatomic, retain) UILabel *tue_pm; 
@property(nonatomic, retain) UILabel *wed_am; 
@property(nonatomic, retain) UILabel *wed_pm; 
@property(nonatomic, retain) UILabel *thru_am;
@property(nonatomic, retain) UILabel *thru_pm;
@property(nonatomic, retain) UILabel *fri_am;
@property(nonatomic, retain) UILabel *fri_pm; 
@property(nonatomic, retain) UILabel *sat_am; 
@property(nonatomic, retain) UILabel *sat_pm; 
@property(nonatomic, retain) UILabel *sun_am;
@property(nonatomic, retain) UILabel *sun_pm; 

@end