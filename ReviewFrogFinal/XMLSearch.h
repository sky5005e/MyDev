//
//  XMLSearch.h
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "Search.h"

@interface XMLSearch : NSObject {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	
	Search *asearch;
}
-(XMLSearch *) initXMLSearch;



@end
