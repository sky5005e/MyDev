//
//  Businessprofile.m
//  Review Frog
//
//  Created by agilepc-32 on 10/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Businessprofile.h"


@implementation Businessprofile
@synthesize businesslogo;
@synthesize businesssplash;
@synthesize userbusinessgiveaway;

@synthesize questionone;
@synthesize questiontwo;
@synthesize questionthree;

-(void)dealloc 
{
	[businesslogo release];
	[businesssplash release];
	[userbusinessgiveaway release];
	
	[questionone release];
	[questiontwo release];
	[questionthree release];
	
	[super dealloc];
}


@end
