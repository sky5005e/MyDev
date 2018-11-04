//
//  XMLCategory.h
//  Review Frog
//
//  Created by agilepc-32 on 11/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "Category.h"

@interface XMLCategory : NSObject {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
    
	Category *objCategory;
    
    NSMutableDictionary *answers;
    
    NSString *q;
    
    NSMutableString *questionID;

    NSMutableString *answerID;
    
    NSMutableString *permission;
}

@property (nonatomic,retain) NSMutableString *questionID,*answerID,*permission;

-(XMLCategory *) initXMLCategory;

@end
