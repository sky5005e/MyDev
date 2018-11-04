//
//  Search.m
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "Search.h"


@implementation Search

@synthesize id;
@synthesize businessname;
@synthesize street;
@synthesize city;
@synthesize state;
@synthesize zipcode;
@synthesize phone;
@synthesize businesscategory;
@synthesize verified;
@synthesize reviewrate;
@synthesize review;

@synthesize serviceoffered;
@synthesize accreditationtext;

@synthesize mon_am;
@synthesize mon_pm;
@synthesize tue_am;
@synthesize tue_pm;
@synthesize wed_am;
@synthesize wed_pm;
@synthesize thru_am;
@synthesize thru_pm;
@synthesize fri_am;
@synthesize fri_pm;
@synthesize sat_am;
@synthesize sat_pm;
@synthesize sun_am;
@synthesize sun_pm;

-(void)dealloc
{
	
	[businessname release];
	[street release];
	[city release];
	[state release];
	[zipcode release];
	[phone release];
	[businesscategory release];
	[verified release];
	[review release];
	
	[serviceoffered release];
	[accreditationtext release];
	
	[mon_am release];
	[mon_pm release];
	[tue_am release];
	[tue_pm release];
	[wed_am release];
	[wed_pm release];
	[thru_am release];
	[thru_pm release];
	[fri_am release];
	[fri_pm release];
	[sat_am release];
	[sat_pm release];
	[sun_am release];
	[sun_pm release];
	
	[super dealloc];
}

@end
