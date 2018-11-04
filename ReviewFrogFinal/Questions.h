//
//  Questions.h
//  Review Frog
//
//  Created by AgileMac4 on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface Questions : NSObject
{
    NSString *question;
    
    NSString *answer;
}
@property (nonatomic,retain) NSString *question,*answer;

@end
