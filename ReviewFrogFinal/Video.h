//
//  Video.h
//  Review Frog
//
//  Created by AgileMac4 on 6/23/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface Video : NSObject {
	
	int id;
	int userid;
	float ReviewRate;
	
	NSString *ReviewPersonEmail;
	NSString *ReviewPersonName;
	NSString *ReviewVideoTitle;
	NSString *ReviewVideoName;
	NSString *PostedDate;
    NSString *user_id;
	NSString *VideoStatus;
	NSString *ReviewPersonCity;
	NSString *ReviewVideoUrl;
	NSString *ReviewCategory;
	NSString *tablequestionone;
	NSString *tablequestiontwo;
	NSString *tablequestionthree;
	NSString *tableDataFrom;
    NSData   *videoData;
}

@property int id;
@property int userid;
@property float ReviewRate;
@property (nonatomic, retain) NSData   *videoData;
@property (nonatomic, retain) NSString *ReviewPersonEmail;
@property (nonatomic, retain) NSString *ReviewPersonName;
@property (nonatomic, retain) NSString *user_id;
@property (nonatomic, retain) NSString *ReviewVideoName;
@property (nonatomic, retain) NSString *ReviewVideoTitle;
@property (nonatomic, retain) NSString *PostedDate;
@property (nonatomic, retain) NSString *VideoStatus;
@property (nonatomic, retain) NSString *ReviewPersonCity;
@property (nonatomic, retain) NSString *ReviewVideoUrl;
@property (nonatomic, retain) NSString *ReviewCategory;
@property (nonatomic, retain) NSString *tablequestionone;
@property (nonatomic, retain) NSString *tablequestiontwo;
@property (nonatomic, retain) NSString *tablequestionthree;
@property (nonatomic, retain) NSString *tableDataFrom;




@end
