//
//  CellBusinessVideo.h
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"

@interface CellBusinessVideo : UIViewController {

	
	Review_FrogAppDelegate *appDelegate;
	
	IBOutlet UILabel *lblVideoName;
	IBOutlet UILabel *lblVideoStatus;
}
@property (nonatomic, retain) IBOutlet UILabel *lblVideoName;
@property (nonatomic, retain) IBOutlet UILabel *lblVideoStatus;
 
@end
