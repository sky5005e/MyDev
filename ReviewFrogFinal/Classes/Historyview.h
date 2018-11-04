//
//  Historyview.h
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "User.h"
#import "History.h"
#import "Review_FrogAppDelegate.h"
#import "MBProgressHUD.h"

@interface Historyview : UIViewController<MBProgressHUDDelegate> {

	User *selectedUser;
	MBProgressHUD *HUD;
	
	IBOutlet UIScrollView *scrView;
	IBOutlet UIView *viewHistory;
	
	IBOutlet UILabel* lblEmail;
	IBOutlet UILabel* lblName;
	IBOutlet UILabel* lblBDate;
	IBOutlet UILabel* lblSDate;
	IBOutlet UILabel* lblTitle;
	IBOutlet UILabel* lblCategory;
	
	IBOutlet UITextView *txtDesc;
	IBOutlet UIButton *btnDelete;
	IBOutlet UIButton *btnPost;
	IBOutlet UILabel *lblDelete;
	IBOutlet UILabel *lblPost;
    
	UIAlertView *alertConfirm;
	UIAlertView *alertdeleted;
	UIAlertView *alertposted;
	History *history;
	
	IBOutlet UIImageView *Frogimg;
	IBOutlet UIImageView *internalimg;
	IBOutlet UIImageView *postimg;
	IBOutlet UIImageView *imgBLogo;
	
	IBOutlet UILabel *lblQuesionOne;
	IBOutlet UILabel *lblQuesionTwo;
	IBOutlet UILabel *lblQuesionThree;
	
	IBOutlet UILabel *lblAnswerOne;
	IBOutlet UILabel *lblAnswerTwo;
	IBOutlet UILabel *lblAnswerThree;
	
	IBOutlet UIImageView *imgGiveAway;
	
	Review_FrogAppDelegate *appDelegate;
	
}
@property (retain, nonatomic) User *selectedUser;
@property (retain, nonatomic) IBOutlet UILabel* lblEmail;
@property (retain, nonatomic) IBOutlet UILabel* lblName;
@property (retain, nonatomic) IBOutlet UILabel* lblBDate;
@property (retain, nonatomic) IBOutlet UILabel* lblSDate;
@property (retain, nonatomic) UILabel* lblTitle;

@property (retain, nonatomic) IBOutlet UITextView *txtDesc;
@property (retain, nonatomic) IBOutlet UIButton *btnDelete;
@property (retain, nonatomic) IBOutlet UIButton *btnPost;
@property (retain, nonatomic) IBOutlet UILabel *lblDelete;
@property (retain, nonatomic) IBOutlet UILabel *lblPost;
@property (retain, nonatomic) IBOutlet UIImageView *Frogimg;
@property (retain, nonatomic) IBOutlet UIImageView *internalimg;
@property (retain, nonatomic) IBOutlet UIImageView *postimg;
@property (retain, nonatomic) UILabel* lblCategory;

@property (retain,nonatomic) History *history;


@property (retain,nonatomic) Review_FrogAppDelegate *appDelegate;
-(IBAction)Backevent;
-(IBAction)DeleteClickEvent;
-(void) DataDelete;	
-(IBAction)PostClickEvent;
-(void)DataPostStatus;






@end
