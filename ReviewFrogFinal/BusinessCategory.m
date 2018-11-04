//
//  BusinessCategory.m
//  Review Frog
//
//  Created by agilepc-32 on 10/7/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BusinessCategory.h"


@implementation BusinessCategory
@synthesize categoryName;

-(void)dealloc
{
	[categoryName release];
	[super dealloc];
}

@end
