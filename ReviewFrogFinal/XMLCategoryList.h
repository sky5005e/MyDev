//
//  XMLCategoryList.h
//  Review Frog
//
//  Created by AgileMac4 on 8/29/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "CategoryWiseList.h"

@interface XMLCategoryList : NSObject {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	
	CategoryWiseList *acategorywiselist;
}
-(XMLCategoryList *) initXMLCategoryList;
@end
