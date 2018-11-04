//
//  SubcategoryPopViewController.h
//  Review Frog
//
//  Created by AgileMac4 on 4/13/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol AnswerSelectedDelegate

-(void) didSelectAnswer;

@end

#import "Review_FrogAppDelegate.h"

@interface SubcategoryPopViewController : UIViewController<UITableViewDelegate,UITableViewDataSource>{
    
    Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UITableView *tblCategory;
	id<AnswerSelectedDelegate>delegate;
    
    int index;
}
@property (nonatomic) int index;

@property (nonatomic, assign) id<AnswerSelectedDelegate>delegate;

@end
