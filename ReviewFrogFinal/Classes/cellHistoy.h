//
//  cellHistoy.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "RateView.h"

@protocol cellHistoyDelegate
- (void)viewClicked:(int)selectedIndex;
- (void)deleteClicked:(int)selectedIndex;
@end

@class Review_FrogAppDelegate;
@interface cellHistoy : UIViewController {
	
	
	
	Review_FrogAppDelegate *appDelegate;
	//UINavigationController *navigationController;
	IBOutlet UILabel* lblEmail;
	IBOutlet UILabel* lblName;
	IBOutlet UILabel* lblBDate;
	IBOutlet UILabel* lblSDate;
	IBOutlet UILabel* lblTitle;
	IBOutlet UILabel* lblSource;
	IBOutlet UILabel* lbltype;
	IBOutlet UILabel*lblNO; 
	
	IBOutlet UIButton* btnView;
	IBOutlet UIButton* btnDelete;
	
	IBOutlet UIImageView *PostedReview;
	
	
	IBOutlet UIImageView *imgGivenRating;
	IBOutlet UIImageView *imgNoRating;
	IBOutlet UIImageView *imghalfRating;
		
	id<cellHistoyDelegate> _delegate;
	

}


@property (nonatomic, retain) UILabel* lblEmail;
@property (nonatomic, retain) UILabel* lblName;
@property (nonatomic, retain) UILabel* lblBDate;
@property (nonatomic, retain) UILabel* lblSDate;
@property (nonatomic, retain) UILabel* lblTitle;
@property (nonatomic, retain) UILabel*lblNO; 

@property (nonatomic, retain) IBOutlet UIImageView *imgGivenRating;
@property (nonatomic, retain) IBOutlet UIImageView *imgNoRating;
@property (nonatomic, retain) IBOutlet UIImageView *imghalfRating;
@property (nonatomic, retain) UILabel* lblSource;
@property (nonatomic, retain) UILabel* lbltype;

@property (nonatomic, retain) UIButton* btnView;
@property (nonatomic, retain) UIButton* btnDelete;

@property (nonatomic, retain) UIImageView *PostedReview;

@property (nonatomic, assign) id<cellHistoyDelegate> delegate;

-(IBAction) btnView_clicked:(id)sender;
-(IBAction) btnDelete_clicked:(id)sender;

@end
