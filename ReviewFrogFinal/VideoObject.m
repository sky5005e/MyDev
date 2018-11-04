//
//  VideoObject.m
//  Review Frog
//
//  Created by AgileMac4 on 6/22/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "VideoObject.h"


@implementation VideoObject

@synthesize videoname;
@synthesize videoid;
@synthesize videourl;
- (void) dealloc {
	
	[videoname release];
    [videourl release];
	[super dealloc];
}

@end

