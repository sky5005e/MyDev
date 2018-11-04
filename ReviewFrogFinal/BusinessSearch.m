//
//  BusinessSearch.m
//  Review Frog
//
//  Created by agilepc-32 on 10/10/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BusinessSearch.h"


@implementation BusinessSearch
@synthesize Bid;
@synthesize businessname;
@synthesize street;
@synthesize phone;
@synthesize businesscategory;
@synthesize verified;
@synthesize reviewcounter;
@synthesize avgreviewrate;

-(void)dealloc
{
	[businessname release];
	[street release];
	[phone release];
	[businesscategory release];
	[verified release];
	[super dealloc];
}
@end
