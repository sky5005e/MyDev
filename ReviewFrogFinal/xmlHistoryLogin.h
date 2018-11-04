//
//  xmlHistoryLogin.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 27/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Review_FrogAppDelegate.h"
#import "AdminEmail.h"


@interface xmlHistoryLogin : NSObject {
	
	
	Review_FrogAppDelegate *appDelegate;
	
	AdminEmail *objadmin;
	
	NSMutableString *currentElementValue;
	
}

- (xmlHistoryLogin *) initxmlHistoryLogin;

@end
