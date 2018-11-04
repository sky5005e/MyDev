//
//  SoundSwitch.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 03/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Review_FrogAppDelegate.h"


@interface SoundSwitch : UIViewController {
	
	
	Review_FrogAppDelegate *appDelegate;
	IBOutlet UISwitch *Switch1;
	IBOutlet UIButton *btnSwitch;
	IBOutlet UIImageView *imgBLogo;
	IBOutlet UIImageView *imgPowerdby;
	IBOutlet UIImageView *imgtxtPowerdby;
}

@property (nonatomic, retain) IBOutlet UISwitch *Switch1;
@property (nonatomic, retain) IBOutlet UIButton *btnSwitch;

-(IBAction)SwitchOnOff;
-(IBAction)Home;
-(IBAction)Admin;
-(IBAction)TERMS_CONDITIONS;
-(IBAction) LogOut;
@end
