//
//  xmlparsarhistory.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "User.h"

@class Review_FrogAppDelegate;

@interface xmlparsarhistory : NSObject {
	NSString *currentElement;
	Review_FrogAppDelegate *appDelegate;
	
	User *objUser;

	NSMutableString *currentElementValue;
	

}

- (xmlparsarhistory *) initxmlparsarhistory;

@end
