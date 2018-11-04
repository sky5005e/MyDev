//
//  Search.h
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface Search : NSObject {
	
	int id;
	float reviewrate;
	NSString *businessname;
	NSString *street;
	NSString *city;
	NSString *state;
	NSString *zipcode;
	NSString *phone;
	NSString *businesscategory;
	NSString *verified;
	
	NSString *serviceoffered;
	NSString *accreditationtext;
	
	NSString *mon_am;
	NSString *mon_pm;
	NSString *tue_am;
	NSString *tue_pm;
	NSString *wed_am;
	NSString *wed_pm;
	NSString *thru_am;
	NSString *thru_pm;
	NSString *fri_am;
	NSString *fri_pm;
	NSString *sat_am;
	NSString *sat_pm;
	NSString *sun_am;
	NSString *sun_pm;
	

	NSString *review;

}
@property int id;
@property float reviewrate;
@property (nonatomic, retain) NSString *businessname;
@property (nonatomic, retain) NSString *street;
@property (nonatomic, retain) NSString *city;
@property (nonatomic, retain) NSString *state;
@property (nonatomic, retain) NSString *zipcode;
@property (nonatomic, retain) NSString *phone;
@property (nonatomic, retain) NSString *businesscategory;
@property (nonatomic, retain) NSString *verified;
@property (nonatomic, retain) NSString *review;

@property (nonatomic, retain) NSString *serviceoffered;
@property (nonatomic, retain) NSString *accreditationtext;


@property (nonatomic, retain) NSString *mon_am;
@property (nonatomic, retain) NSString *mon_pm;
@property (nonatomic, retain) NSString *tue_am;
@property (nonatomic, retain) NSString *tue_pm;
@property (nonatomic, retain) NSString *wed_am;
@property (nonatomic, retain) NSString *wed_pm;
@property (nonatomic, retain) NSString *thru_am;
@property (nonatomic, retain) NSString *thru_pm;
@property (nonatomic, retain) NSString *fri_am;
@property (nonatomic, retain) NSString *fri_pm;
@property (nonatomic, retain) NSString *sat_am;
@property (nonatomic, retain) NSString *sat_pm;
@property (nonatomic, retain) NSString *sun_am;
@property (nonatomic, retain) NSString *sun_pm;


@end
