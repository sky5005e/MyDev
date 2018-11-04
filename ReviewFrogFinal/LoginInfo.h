//
//  LoginInfo.h
//  Review Frog
//
//  Created by agilepc-32 on 10/13/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import <Foundation/Foundation.h>


@interface LoginInfo : NSObject {
	
	int id;
	NSString *userLevelId;
	NSString *userSubnetId;
	NSString *userProfilecatId;
	NSString *userEmail;
	NSString *usernewEmail;
	NSString *userFname;
	NSString *userLname;
	NSString *username;
	NSString *userDisplayname;
	NSString *userPassword;
	NSString *userPasswordmethod;
	NSString *userCode;
	NSString *userEnabled;
	NSString *userVerified;
	NSString *userLanguageId;
	NSString *userSignupdate;
	NSString *userLastlogindate;
	NSString *userLastactive;
	NSString *userIpSignup;
	NSString *userIpLastactive;
	NSString *userStatus;
	NSString *userStatusdate;
	NSString *userLogins;
	NSString *userInvitesleft;
	NSString *userTimezone;
	NSString *userDateupdated;
	NSString *userInvisible;
	NSString *userSaveviews;
	NSString *userPhoto;
	NSString *userSearch;
	NSString *userPrivacy;
	NSString *userComments;
	NSString *userHasnotifys;
	NSString *userUserpointsAllowed;
	NSString *userbusinesslogo;
	NSString *userbusinessgiveaway;
	NSString *userbusinessid;
	
	NSString *BusinessSplash;
	NSString *questionone;
	NSString *questiontwo;
	NSString *questionthree;
	
	
	UIImage *imgBlogo;
	UIImage *imgBsplash;
	UIImage *imgGiveAway;
}

@property int id;
@property (nonatomic,retain) NSString *userLevelId;
@property (nonatomic,retain) NSString *userSubnetId;
@property (nonatomic,retain) NSString *userProfilecatId;
@property (nonatomic,retain) NSString *userEmail;
@property (nonatomic,retain) NSString *usernewEmail;
@property (nonatomic,retain) NSString *userFname;
@property (nonatomic,retain) NSString *userLname;
@property (nonatomic,retain) NSString *username;
@property (nonatomic,retain) NSString *userDisplayname;
@property (nonatomic,retain) NSString *userPassword;
@property (nonatomic,retain) NSString *userPasswordmethod;
@property (nonatomic,retain) NSString *userCode;
@property (nonatomic,retain) NSString *userEnabled;
@property (nonatomic,retain) NSString *userVerified;
@property (nonatomic,retain) NSString *userLanguageId;
@property (nonatomic,retain) NSString *userSignupdate;
@property (nonatomic,retain) NSString *userLastlogindate;
@property (nonatomic,retain) NSString *userLastactive;
@property (nonatomic,retain) NSString *userIpSignup;
@property (nonatomic,retain) NSString *userIpLastactive;
@property (nonatomic,retain) NSString *userStatus;
@property (nonatomic,retain) NSString *userStatusdate;
@property (nonatomic,retain) NSString *userLogins;
@property (nonatomic,retain) NSString *userInvitesleft;
@property (nonatomic,retain) NSString *userTimezone;
@property (nonatomic,retain) NSString *userDateupdated;
@property (nonatomic,retain) NSString *userInvisible;
@property (nonatomic,retain) NSString *userSaveviews;
@property (nonatomic,retain) NSString *userPhoto;
@property (nonatomic,retain) NSString *userSearch;
@property (nonatomic,retain) NSString *userPrivacy;
@property (nonatomic,retain) NSString *userComments;
@property (nonatomic,retain) NSString *userHasnotifys;
@property (nonatomic,retain) NSString *userUserpointsAllowed;
@property (nonatomic,retain) NSString *userbusinesslogo;
@property (nonatomic,retain) NSString *userbusinessid;
@property (nonatomic,retain) NSString *userbusinessgiveaway;
@property (nonatomic,retain) UIImage *imgBlogo;
@property (nonatomic,retain) UIImage *imgGiveAway;

@property (nonatomic,retain) NSString *BusinessSplash;
@property (nonatomic,retain) NSString *questionone;
@property (nonatomic,retain) NSString *questiontwo;
@property (nonatomic,retain) NSString *questionthree;

@property (nonatomic,retain) UIImage *imgBsplash;

  
 @end
 
 
 