//
//  PostedReview.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 13/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "PostedReview.h"
#import "Review_FrogAppDelegate.h"

@implementation PostedReview

@synthesize isChecked;

- (id)initWithFrame:(CGRect)frame {
    if (self = [super initWithFrame:frame]) {
        // Initialization code
		appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];

		//self.frame =frame;
		self.contentHorizontalAlignment = UIControlContentHorizontalAlignmentLeft;
		if([appDelegate.poststatus isEqualToString:@"I"])
        {
			//[self setImage:[UIImage imageNamed:@"checkbox_not_ticked.png"] forState:UIControlStateNormal];
        }
		else {
			//[self setImage:[UIImage imageNamed:@"checkbox_ticked.png"] forState:UIControlStateNormal];
			
			self.isChecked =YES;
		}
		//[self addTarget:self action:@selector(postedReviewClicked) forControlEvents:UIControlEventTouchUpInside];
	}
    return self;
}

-(IBAction)postedReviewClicked{
	
	if(self.isChecked ==NO){
		
		self.isChecked =YES;
		[self setImage:[UIImage imageNamed:@"checkbox_ticked.png"] forState:UIControlStateNormal];
		appDelegate.checkflag=TRUE;
		appDelegate.poststatus=@"A";
		[appDelegate saveUserPreferences:1];
		
		
	}else{
		self.isChecked =NO;
		[self setImage:[UIImage imageNamed:@"checkbox_not_ticked.png"] forState:UIControlStateNormal];
		appDelegate.checkflag=FALSE;
		
		appDelegate.poststatus=@"I";
		[appDelegate saveUserPreferences:1];
		
	}
	
}

- (void)dealloc {
    [super dealloc];
}


@end

