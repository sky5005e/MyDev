//
//  WriteObject.h
//  Review Frog
//
//  Created by AgileMac6 on 1/25/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface WriteObject : NSObject
{
    NSString *name ;
    NSString *email ;
    NSString * city;
    NSString * desp;
    NSString * title;
    NSString * rating;
    NSString * q1;
    NSString * q2;
    NSString * q3;
    NSString * category;
    NSString * userid;
    NSString * status;
    NSString * date;
}
@property (nonatomic, retain)NSString * name ;
@property (nonatomic, retain)NSString * email ;
@property (nonatomic, retain)NSString * city;
@property (nonatomic, retain)NSString * desp;
@property (nonatomic, retain)NSString * title;
@property (nonatomic, retain)NSString * rating;
@property (nonatomic, retain)NSString * q1;
@property (nonatomic, retain)NSString * q2;
@property (nonatomic, retain)NSString * q3;
@property (nonatomic, retain)NSString * category;
@property (nonatomic, retain)NSString * userid;
@property (nonatomic, retain)NSString * status;
@property (nonatomic, retain)NSString * date;

@end
