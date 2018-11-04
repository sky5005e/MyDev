//
//  XMLSearchResult.h
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "SearchResult.h"
#import "BusinessSearch.h"

@interface XMLSearchResult : NSObject {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	BOOL flgsearch;
	SearchResult *aResult;
	BusinessSearch *aBSearch;
	
}
-(XMLSearchResult *) initXMLSearchResult;

@end
