//
//  XMLBusinessCategory.h
//  Review Frog
//
//  Created by agilepc-32 on 10/7/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "BusinessCategory.h"


@interface XMLBusinessCategory : NSObject {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	
	BusinessCategory *aBcategory;
}
-(XMLBusinessCategory *) initXMLBusinessCategory;



@end
