//
//  xmlDelete.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 05/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class Review_FrogAppDelegate;

@interface xmlDelete : NSObject {
	NSString *currentElement;
	Review_FrogAppDelegate *appDelegate;


}

- (xmlDelete *) initxmlDelete;
@end
