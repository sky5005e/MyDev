//
//  XMLBusinessProfile.h
//  Review Frog
//
//  Created by agilepc-32 on 10/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "Businessprofile.h"


@interface XMLBusinessProfile : NSObject <NSXMLParserDelegate>{

	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;

}
-(XMLBusinessProfile *) initXMLBusinessProfile;

@end
