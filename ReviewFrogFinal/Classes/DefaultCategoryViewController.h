//
//  DefaultCategoryViewController.h
//  Review Frog
//
//  Created by agilepc-32 on 11/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
@protocol DefaultCategorydelegate

-(void) didSelectDefaultCategory;

@end

#import "Review_FrogAppDelegate.h"
@interface DefaultCategoryViewController : UIViewController<UITableViewDelegate,UITableViewDataSource> {

	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UITableView *tblCategory;
	id<DefaultCategorydelegate>delegate;
	
}
@property (nonatomic, retain) UITableView *tblCategory;
@property (nonatomic, assign) id<DefaultCategorydelegate>delegate;

@end
