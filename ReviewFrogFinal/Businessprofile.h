//
//  Businessprofile.h
//  Review Frog
//
//  Created by agilepc-32 on 10/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface Businessprofile : NSObject {
	
	NSString *businesslogo;
	NSString *businesssplash;
	NSString *userbusinessgiveaway;
	
	NSString *questionone;
	NSString *questiontwo;
	NSString *questionthree;

}
@property (nonatomic, retain) NSString *businesslogo;
@property (nonatomic, retain) NSString *businesssplash;
@property (nonatomic, retain) NSString *userbusinessgiveaway;

@property (nonatomic, retain) NSString *questionone;
@property (nonatomic, retain) NSString *questiontwo;
@property (nonatomic, retain) NSString *questionthree;

@end
