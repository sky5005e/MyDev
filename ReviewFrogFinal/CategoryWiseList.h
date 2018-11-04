//
//  CategoryWiseList.h
//  Review Frog
//
//  Created by AgileMac4 on 8/29/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface CategoryWiseList : NSObject {
	
	NSString *name;
	NSString *fivestar;
	NSString *fourstar;
	NSString *threestar;
	NSString *twostar;
	NSString *onestar;
	NSString *CategoryWiseTotal;
}
@property (nonatomic, retain) NSString *name;
@property (nonatomic, retain) NSString *fivestar;
@property (nonatomic, retain) NSString *fourstar;
@property (nonatomic, retain) NSString *threestar;
@property (nonatomic, retain) NSString *twostar;
@property (nonatomic, retain) NSString *onestar;
@property (nonatomic, retain) NSString *CategoryWiseTotal;
@end
