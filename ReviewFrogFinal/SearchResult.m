//
//  SearchResult.m
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "SearchResult.h"


@implementation SearchResult

@synthesize Rid;
@synthesize reviewerimage;
@synthesize reviewtitle;
@synthesize postedby;
@synthesize reviewrate;
@synthesize reviewdate;
@synthesize reviewdescription;
@synthesize yescount;
@synthesize nocount;
@synthesize imgReviewer;

-(void) dealloc
{	

	
	[reviewerimage release];
	[reviewtitle release];
	[reviewtitle release];
	[postedby release];
	[imgReviewer release];
	[reviewdate release];
	[reviewdescription release];
	[yescount release];
	[nocount release];
	[super dealloc];
	
	
}


@end
