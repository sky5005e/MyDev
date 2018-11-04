//
//  xmlPost.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 13/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class Review_FrogAppDelegate;

@interface xmlPost : NSObject {
	
	NSString *currentElement;
	Review_FrogAppDelegate *appDelegate;


}
- (xmlPost *) initxmlPost;
@end
