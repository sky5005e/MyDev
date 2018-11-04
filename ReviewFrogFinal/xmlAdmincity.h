//
//  xmlAdmincity.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 01/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"

@interface xmlAdmincity : NSObject {
	
	
	NSString *currentElement;
	Review_FrogAppDelegate *appDelegate;
	
	
}

- (xmlAdmincity *) initxmlAdmincity;

@end
