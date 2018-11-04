//
//  Category.h
//  Review Frog
//
//  Created by AgileMac4 on 8/12/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface Category : NSObject {

	int id;
	NSString *categoryname;
    NSString *Category_UserID;
}
@property int id;
@property (nonatomic, retain) NSString *categoryname;
@property (nonatomic, retain) NSString *Category_UserID;
@end
