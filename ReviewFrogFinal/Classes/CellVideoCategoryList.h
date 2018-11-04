//
//  CellVideoCategoryList.h
//  Review Frog
//
//  Created by AgileMac4 on 9/2/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"

@interface CellVideoCategoryList : UIViewController {

	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UILabel *lblCategory;
	IBOutlet UILabel *lblTotal;
	IBOutlet UILabel *avg;
	IBOutlet UIImageView *imgGivenRating;
	IBOutlet UIImageView *imgNoRating;
	IBOutlet UIImageView *imghalfRating;
	
	
	
}

@property (nonatomic, retain) UILabel *lblCategory;
@property (nonatomic, retain) UILabel *lblTotal;
@property (nonatomic, retain) UILabel *avg;
@property (nonatomic, retain) UIImageView *imgGivenRating;
@property (nonatomic, retain) UIImageView *imgNoRating;
@property (nonatomic, retain) UIImageView *imghalfRating;




@end
