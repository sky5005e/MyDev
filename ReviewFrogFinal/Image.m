//
//  Image.m
//  Review Frog
//
//  Created by agilepc-32 on 9/30/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Image.h"


@implementation Image

@synthesize id;
@synthesize imageurl;
@synthesize image;

-(void)dealloc
{
	[imageurl release];
	[image release];
	[super dealloc];
}



@end
