//
//  Gallery.h
//  Review Frog
//
//  Created by agilepc-32 on 9/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface Gallery : NSObject {
	
	int id;
	NSString *patientname;
	NSString *patientemail;
	NSString *ipaduserid;
	NSString *beforepic;
	NSString *afterpic;
	NSString *comments;
	NSString *status;
	NSString *date;
	
	UIImage *imgbeforepic;
	UIImage *imgafterpic;
	
	NSMutableArray *delArrimage;

}
@property int id;
@property(nonatomic, retain) NSString *patientname;
@property(nonatomic, retain) NSString *patientemail;
@property(nonatomic, retain) NSString *ipaduserid;
@property(nonatomic, retain) NSString *beforepic;
@property(nonatomic, retain) NSString *afterpic;
@property(nonatomic, retain) NSString *comments;
@property(nonatomic, retain) NSString *status;
@property(nonatomic, retain) NSString *date;

@property (nonatomic, retain) UIImage *imgbeforepic;
@property (nonatomic, retain) UIImage *imgafterpic;
@property (nonatomic, retain) NSMutableArray *delArrimage;

@end
