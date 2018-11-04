//
//  CategoryWiseList.m
//  Review Frog
//
//  Created by AgileMac4 on 8/29/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "CategoryWiseList.h"


@implementation CategoryWiseList
@synthesize name;
@synthesize fivestar;
@synthesize fourstar;
@synthesize threestar;
@synthesize twostar;
@synthesize onestar;
@synthesize CategoryWiseTotal;


-(void) dealloc
{
	[super dealloc];
	[name release];
	[fivestar release];
	[fourstar release];
	[threestar release];
	[twostar release];
	[onestar release];
	[CategoryWiseTotal release];
}
@end
