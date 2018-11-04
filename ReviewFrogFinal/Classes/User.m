//
//  User.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "User.h"


@implementation User

@synthesize tableDataEmail;
@synthesize tableDataName;
@synthesize tableDataDescription;
@synthesize tableDataDate;
@synthesize tableDataCity;
@synthesize tableDataID;
@synthesize tableDatauserid;
@synthesize tableDatauserStatus;
@synthesize tableDataRate;
@synthesize tableDataFrom;
@synthesize tableDataTitle;
@synthesize tableDataCategory;
@synthesize tablequestionone;
@synthesize tablequestiontwo;
@synthesize tablequestionthree;
- (void)dealloc {
	
	
	[tableDataEmail release];
	[tableDataName release];
	[tableDataDescription release];
	[tableDataDate release];
	[tableDataCity release];
	[tableDatauserStatus release];
	[tableDataFrom release];
	[tableDataTitle release];
	[tableDataCategory release];
	[tablequestionone release];
	[tablequestiontwo release];
	[tablequestionthree release];

    [super dealloc];
}

@end