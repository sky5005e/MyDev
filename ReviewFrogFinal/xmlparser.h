//
//  xmlparser.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 03/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "LoginInfo.h"

@class Review_FrogAppDelegate;
@interface xmlparser : NSObject {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	
	LoginInfo *alogininfo;
}
-(xmlparser *) initxmlparser;

@end
