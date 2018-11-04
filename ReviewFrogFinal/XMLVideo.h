//
//  XMLVideo.h
//  Review Frog
//
//  Created by AgileMac4 on 6/23/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "Video.h"


@interface XMLVideo : NSObject {

	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	
	Video *avideo;
}

-(XMLVideo *) initXMLVideo;


@end
