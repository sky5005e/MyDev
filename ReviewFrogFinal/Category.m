//
//  Category.m
//  Review Frog
//
//  Created by AgileMac4 on 8/10/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Category.h"


@implementation Category

@synthesize id;
@synthesize categoryname;
@synthesize Category_UserID;

- (void)dealloc {
	
	[categoryname release];
    [Category_UserID release];
    [super dealloc];
}
@end
