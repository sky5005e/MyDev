//
//  XMLGallery.h
//  Review Frog
//
//  Created by agilepc-32 on 9/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "Gallery.h"

@interface XMLGallery : NSObject {

	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	
	Gallery *agallery;
}
	
-(XMLGallery *) initXMLGallery;
	
@end
