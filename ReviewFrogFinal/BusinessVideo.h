//
//  BusinessVideo.h
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>



@interface BusinessVideo : NSObject {

	int id;
	int userid;
	float ReviewRate;
	
	NSString *ReviewPersonEmail;
	NSString *ReviewPersonName;
	NSString *ReviewVideoTitle;
	NSString *ReviewVideoName;
	NSString *PostedDate;
	NSString *VideoStatus;
	NSString *ReviewPersonCity;
	NSString *ReviewVideoUrl;
	
}
@property int id;
@property int userid;
@property float ReviewRate;

@property (nonatomic, retain) NSString *ReviewPersonEmail;
@property (nonatomic, retain) NSString *ReviewPersonName;
@property (nonatomic, retain) NSString *ReviewVideoName;
@property (nonatomic, retain) NSString *ReviewVideoTitle;
@property (nonatomic, retain) NSString *PostedDate;
@property (nonatomic, retain) NSString *VideoStatus;
@property (nonatomic, retain) NSString *ReviewPersonCity;
@property (nonatomic, retain) NSString *ReviewVideoUrl;


@end
