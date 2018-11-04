//
//  VideoObject.h
//  Review Frog
//
//  Created by AgileMac4 on 6/22/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface VideoObject : NSObject {
	
	int videoid;
	NSString *videoname;
    NSString *videourl;

}

@property int videoid;
@property (nonatomic, retain) NSString *videoname;
@property (nonatomic, retain) NSString *videourl;
@end
