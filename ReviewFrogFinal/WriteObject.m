//
//  WriteObject.m
//  Review Frog
//
//  Created by AgileMac6 on 1/25/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "WriteObject.h"

@implementation WriteObject
@synthesize name ;
@synthesize email ;
@synthesize city;
@synthesize desp;
@synthesize title;
@synthesize rating;
@synthesize q1;
@synthesize q2;
@synthesize q3;
@synthesize category;
@synthesize userid;
@synthesize status;
@synthesize  date;
- (void) dealloc {
	
	[name release];
    [email release];
    [city release];
    [desp release];
    [title release];
    [rating release];
    [q1 release];
    [q2 release];
    [q3 release];
    [category release];
    [userid release];
    [status release];
    [date release];
	[super dealloc];
}


@end
