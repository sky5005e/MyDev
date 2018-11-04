//
//  CategoryPopViewController.h
//  Review Frog
//
//  Created by agilepc-32 on 11/7/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol CategorydidSelectdelegate

-(void) didSelect;

@end


#import "Review_FrogAppDelegate.h"

@interface CategoryPopViewController : UIViewController<UITableViewDelegate,UITableViewDataSource> {
	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UITableView *tblCategory;
	id<CategorydidSelectdelegate>delegate;
    
    NSString *question;

}
@property (nonatomic, retain) UITableView *tblCategory;
@property (nonatomic, assign) id<CategorydidSelectdelegate>delegate;

@property (nonatomic,retain) NSString *question;

@end
