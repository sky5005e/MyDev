//
//  BusinessSearch.h
//  Review Frog
//
//  Created by agilepc-32 on 10/10/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface BusinessSearch : NSObject {
	
	
	
	int Bid;
	NSString *businessname;
	NSString *street;
	NSString *phone;
	NSString *businesscategory;
	NSString *verified;
	float reviewcounter;
	float avgreviewrate;


}
@property int Bid;
@property (nonatomic, retain) NSString *businessname;
@property (nonatomic, retain) NSString *street;
@property (nonatomic, retain) NSString *phone;
@property (nonatomic, retain) NSString *businesscategory;
@property (nonatomic, retain) NSString *verified;
@property float reviewcounter;
@property float avgreviewrate;
@end
