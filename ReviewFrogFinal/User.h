//
//  User.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface User : NSObject {

	int tableDataID;
	NSString *tableDataEmail;
	NSString *tableDataName;
	NSString *tableDataDescription;
    int tableDatauserid;
	NSString *tableDataDate;
	NSString *tableDatauserStatus;
	NSString *tableDataCity;
	float tableDataRate;
	//NSString *return1;
	NSString *tableDataTitle;
	NSString *tableDataFrom;
	NSString *tableDataCategory;

	NSString *tablequestionone;
	NSString *tablequestiontwo;
	NSString *tablequestionthree;
	
		
}
@property int tableDataID;
@property(nonatomic, retain) NSString *tableDataEmail;
@property(nonatomic, retain) NSString *tableDataName;
@property(nonatomic, retain) NSString *tableDataDescription;
@property int tableDatauserid;
@property(nonatomic, retain) NSString *tableDataDate;
@property(nonatomic, retain) NSString *tableDataCity;
//@property(nonatomic, retain) NSString *return1;
@property(nonatomic, retain) NSString *tableDatauserStatus;
@property float tableDataRate;
@property (nonatomic, retain) NSString *tableDataTitle;
@property (nonatomic, retain) NSString *tableDataFrom;
@property (nonatomic, retain) NSString *tableDataCategory;

@property (nonatomic, retain) NSString *tablequestionone;
@property (nonatomic, retain) NSString *tablequestiontwo;
@property (nonatomic, retain) NSString *tablequestionthree;



@end
