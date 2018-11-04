//
//  XMLCityname.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 28/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "Cityname.h"

@interface XMLCityname : NSObject {
	
	Review_FrogAppDelegate *appDelegate;
	
	NSMutableString *currentElementValue;
	
	Cityname *acityname;



}

- (XMLCityname *) initXMLCityname;


@end
