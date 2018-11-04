//
//  Questions.m
//  Review Frog
//
//  Created by AgileMac4 on 4/18/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "Questions.h"

@implementation Questions
@synthesize question,answer;

-(void)dealloc{
    
    [super dealloc];
    
    [question release];
    
    [answer release];
}

@end
