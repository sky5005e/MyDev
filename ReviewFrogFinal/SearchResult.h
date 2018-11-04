//
//  SearchResult.h
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface SearchResult : NSObject {
	
												
	int Rid;
	NSString *reviewerimage;
	NSString *reviewtitle;
	NSString *postedby;
	float reviewrate;
	NSString *reviewdate;
	NSString *reviewdescription;
	NSString *yescount;
	NSString *nocount;
	
	UIImage *imgReviewer;

}

@property int Rid;
@property (nonatomic, retain) NSString *reviewerimage;
@property (nonatomic, retain) NSString *reviewtitle;
@property (nonatomic, retain) NSString *postedby;
@property float reviewrate;
@property (nonatomic, retain) NSString *reviewdate;
@property (nonatomic, retain) NSString *reviewdescription;
@property (nonatomic, retain) NSString *yescount;
@property (nonatomic, retain) NSString *nocount;
@property (nonatomic, retain) UIImage *imgReviewer;

@end
