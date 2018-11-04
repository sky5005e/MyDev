//
//  Image.h
//  Review Frog
//
//  Created by agilepc-32 on 9/30/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface Image : NSObject {

	
	int id;
	NSString *imageurl;
	UIImage *image;

}


@property int id;
@property (nonatomic, retain) NSString *imageurl;
@property (nonatomic, retain) UIImage *image;

@end
