//
//  xmlparser_para.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 03/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@class Review_FrogAppDelegate;

@interface xmlparser_para : NSObject {
	NSString *currentElement;
	Review_FrogAppDelegate *appDelegate;

}

- (xmlparser_para *) initxmlparser_para;
@end
