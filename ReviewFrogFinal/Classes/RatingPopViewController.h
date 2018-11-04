//
//  RatingPopViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 4/27/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

#import "Review_FrogAppDelegate.h"

@protocol RatingSelectedDelegate

-(void) didSelectRating;

@end

@interface RatingPopViewController : UIViewController<UITableViewDataSource,UITableViewDelegate>
{
    IBOutlet UITableView *table;
    
    NSDictionary *ratings;
    
    id<RatingSelectedDelegate>delegate;
    
    int index;
    
    Review_FrogAppDelegate *appDel;
    
    NSDictionary *sectionNames;
    
    IBOutlet UILabel *question;
    
    NSString *QuestionText;
    
    NSString *checkedCell;
    
    UITableViewCell *selectedCell;
    int Sec,row;
    
    
}

@property (nonatomic, assign) id<RatingSelectedDelegate>delegate;

@property (nonatomic,retain) NSString *QuestionText,*checkedCell;

@property (nonatomic) int index;

@end
