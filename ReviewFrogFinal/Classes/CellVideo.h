//
//  CellVideo.h
//  Review Frog
//
//  Created by AgileMac4 on 6/27/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"


@interface CellVideo : UIViewController {

	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UILabel *lblName;
	IBOutlet UILabel *lblSDate;
	IBOutlet UILabel *lblVideoTitle;
	IBOutlet UILabel *lblCategory;
	IBOutlet UILabel *lblStatus;
	IBOutlet UILabel *lblNO;
	
	IBOutlet UIButton *btnDelete;
	
	
	IBOutlet UIImageView *imgGivenRating;
	IBOutlet UIImageView *imgNoRating;
	IBOutlet UIImageView *imghalfRating;
}

@property (nonatomic, retain) IBOutlet UILabel *lblName;
@property (nonatomic, retain) IBOutlet UILabel *lblSDate;
@property (nonatomic, retain) IBOutlet UILabel *lblVideoTitle;
@property (nonatomic, retain) UILabel *lblNO;
@property (nonatomic, retain) UILabel *lblCategory;
@property (nonatomic, retain) IBOutlet UIButton *btnDelete;

@property (nonatomic, retain) IBOutlet UIImageView *imgGivenRating;
@property (nonatomic, retain) IBOutlet UIImageView *imgNoRating;
@property (nonatomic, retain) IBOutlet UIImageView *imghalfRating;
@property (nonatomic, retain) IBOutlet UILabel *lblStatus;
@end
