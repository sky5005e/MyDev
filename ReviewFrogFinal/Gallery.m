//
//  Gallery.m
//  Review Frog
//
//  Created by agilepc-32 on 9/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Gallery.h"


@implementation Gallery

@synthesize id;
@synthesize patientname;
@synthesize patientemail;
@synthesize ipaduserid;
@synthesize beforepic;
@synthesize afterpic;
@synthesize comments;
@synthesize status;
@synthesize date;
@synthesize imgbeforepic;
@synthesize imgafterpic;
@synthesize delArrimage;

- (void) dealloc {
	
	[patientname release];
	[patientemail release];
	[ipaduserid release];
	[beforepic release];
	[afterpic release];
	[comments release];
	[status release];
	[date release];
	[imgbeforepic release];
	[imgafterpic release];
	[delArrimage release];
	[super dealloc];
}

@end
