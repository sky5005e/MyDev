//
//  BusinessVideo.m
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BusinessVideo.h"


@implementation BusinessVideo
@synthesize id;
@synthesize userid;
@synthesize ReviewRate;
@synthesize ReviewPersonEmail;
@synthesize ReviewPersonName;
@synthesize ReviewVideoTitle;
@synthesize ReviewVideoName;
@synthesize PostedDate;
@synthesize VideoStatus;
@synthesize ReviewPersonCity;
@synthesize ReviewVideoUrl;

- (void) dealloc {
	
	[ReviewPersonEmail release];
	[ReviewPersonName release];	
	[ReviewVideoTitle release];
	[ReviewVideoName release];
	[PostedDate release];
	[VideoStatus release];
	[ReviewPersonCity release];
	[ReviewVideoUrl release];
	[super dealloc];
	
	
}




@end
