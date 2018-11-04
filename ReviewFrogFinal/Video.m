//
//  Video.m
//  Review Frog
//
//  Created by AgileMac4 on 6/23/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Video.h"


@implementation Video

@synthesize id;
@synthesize userid;
@synthesize user_id;
@synthesize ReviewRate;
@synthesize ReviewPersonEmail;
@synthesize ReviewPersonName;
@synthesize ReviewVideoTitle;
@synthesize ReviewVideoName;
@synthesize PostedDate;
@synthesize VideoStatus;
@synthesize ReviewPersonCity;
@synthesize ReviewVideoUrl;
@synthesize ReviewCategory;
@synthesize tablequestionone;
@synthesize tablequestiontwo;
@synthesize tablequestionthree;
@synthesize videoData;
@synthesize tableDataFrom;
- (void) dealloc {
	
	[ReviewPersonEmail release];
	[ReviewPersonName release];	
	[ReviewVideoTitle release];
    [videoData release];
	[ReviewVideoName release];
	[PostedDate release];
	[VideoStatus release];
	[ReviewPersonCity release];
	[ReviewVideoUrl release];
	[ReviewCategory release];
	[tablequestionone release];
	[tablequestiontwo release];
	[tablequestionthree release];
	[tableDataFrom release];
	[super dealloc];
	
	
}


@end
