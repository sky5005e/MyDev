//
//  LoginInfo.m
//  Review Frog
//
//  Created by agilepc-32 on 10/13/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "LoginInfo.h"


@implementation LoginInfo

@synthesize id;
@synthesize userLevelId;
@synthesize userSubnetId;
@synthesize userProfilecatId;
@synthesize userEmail;
@synthesize usernewEmail;
@synthesize userFname;
@synthesize userLname;
@synthesize username;
@synthesize userDisplayname;
@synthesize userPassword;
@synthesize userPasswordmethod;
@synthesize userCode;
@synthesize userEnabled;
@synthesize userVerified;
@synthesize userLanguageId;
@synthesize userSignupdate;
@synthesize userLastlogindate;
@synthesize userLastactive;
@synthesize userIpSignup;
@synthesize userIpLastactive;
@synthesize userStatus;
@synthesize userStatusdate;
@synthesize userLogins;
@synthesize userInvitesleft;
@synthesize userTimezone;
@synthesize userDateupdated;
@synthesize userInvisible;
@synthesize userSaveviews;
@synthesize userPhoto;
@synthesize userSearch;
@synthesize userPrivacy;
@synthesize userComments;
@synthesize userHasnotifys;
@synthesize userUserpointsAllowed;
@synthesize userbusinesslogo;
@synthesize userbusinessid;
@synthesize imgBlogo;
@synthesize BusinessSplash;
@synthesize questionone;
@synthesize questiontwo;
@synthesize questionthree;
@synthesize imgBsplash;
@synthesize userbusinessgiveaway;
@synthesize imgGiveAway;

-(void)dealloc
{
[userLevelId release];
[userSubnetId release];
[userProfilecatId release];
[userEmail release];
[usernewEmail release];
[userFname release];
[userLname release];
[username release];
[userDisplayname release];
[userPassword release];
[userPasswordmethod release];
[userCode release];
[userEnabled release];
[userVerified release];
[userLanguageId release];
[userSignupdate release];
[userLastlogindate release];
[userLastactive release];
[userIpSignup release];
[userIpLastactive release];
[userStatus release];
[userStatusdate release];
[userLogins release];
[userInvitesleft release];
[userTimezone release];
[userDateupdated release];
[userInvisible release];
[userSaveviews release];
[userPhoto release];
[userSearch release];
[userPrivacy release];
[userComments release];
[userHasnotifys release];
[userUserpointsAllowed release];
[userbusinesslogo release];
[userbusinessid release];
[imgBlogo release];
	[imgGiveAway release];
	[questionone release];
	[questiontwo release];
	[questionthree release];
	[userbusinessgiveaway release];	
	
[super dealloc];
}
@end
