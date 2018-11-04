//
//  BusinessVideoListViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 7/6/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "BusinessVideoListViewController.h"
#import "XMLVideo.h"
#import "CellVideo.h"
#import "PlayVideoViewController.h"
#import "CellBusinessVideo.h"
#import "HistoryLogin.h"
#import "ThreeTabButton.h"
#import "Terms_Condition.h"
#import "ApplicationValidation.h"
#import "PlayBusinessVideoViewController.h"
#import "Video.h";
#import "OptionOfHistoryViewController.h"
#import "WriteORRecodeViewController.h"
#import <MobileCoreServices/MobileCoreServices.h>
#import "BusinessVideoViewController.h"
#import "LoginInfo.h"

@implementation BusinessVideoListViewController
@synthesize tblBusinessList;
@synthesize objVideo;
@synthesize filteredArray; 
@synthesize SearchCityBar;
@synthesize searchDC;
@synthesize flgBusinessVideo;

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
-(void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	btnDelete.hidden=YES;
	SearchCityBar.delegate = self;
	
	self.searchDC = [[[UISearchDisplayController alloc] initWithSearchBar:self.SearchCityBar contentsController:self] autorelease];
	self.searchDC.searchResultsDataSource = self;
	self.searchDC.searchResultsDelegate = self;
	
	SearchCityBar.showsCancelButton = NO;
	
	

}
-(void)viewWillAppear:(BOOL)animated
{
	[self StartLoding];
    NSUserDefaults *prefs = [NSUserDefaults standardUserDefaults];
    NSString *pathGiveAway = [prefs stringForKey:@"pathGiveAway"];
    if (pathGiveAway !=nil) {   
        
        imgGiveAway.image= [[[UIImage alloc] initWithContentsOfFile:pathGiveAway] autorelease];
        
    }
    NSString *pathBussinesslogo = [prefs stringForKey:@"pathBussinesslogo"];
    if (pathBussinesslogo !=nil) {   
        
        imgBLogo.image= [[[UIImage alloc] initWithContentsOfFile:pathBussinesslogo] autorelease];
        
    }
    if (!imgGiveAway.image) {
        UIImage *img = [UIImage imageNamed:@"giftcard.png"];
        imgGiveAway.image = nil;
    }
    if (!imgBLogo.image) {
        UIImage *img = [UIImage imageNamed:@"Nlogo1_v2.png"];
        imgBLogo.image = img;
    }
  
}
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation {
	// Overriden to allow any orientation.
	if(interfaceOrientation == UIInterfaceOrientationLandscapeRight || interfaceOrientation == UIInterfaceOrientationLandscapeLeft)
		return YES;
	else if(interfaceOrientation == UIInterfaceOrientationPortrait)
		return NO;
	else 
		return NO;
}
-(IBAction)Home{
	appDelegate.txtcleanflag=FALSE;
	appDelegate.LoginConform=FALSE;
	WriteORRecodeViewController *awriteorRecode = [[WriteORRecodeViewController alloc]initWithNibName:@"WriteORRecodeViewController" bundle:nil];
    [self.navigationController pushViewController:awriteorRecode animated:YES];
    [awriteorRecode release];
	
}
-(IBAction) Admin
{	
	if(appDelegate.LoginConform==FALSE)
	
	{	
	HistoryLogin *historylogin = [[HistoryLogin alloc] initWithNibName:@"HistoryLogin" bundle:nil];
	
        [self.navigationController pushViewController:historylogin animated:YES];
        [historylogin release];

	}
	else
	{
	OptionOfHistoryViewController *objoption = [[OptionOfHistoryViewController alloc]initWithNibName:@"OptionOfHistoryViewController" bundle:nil];
		[self.navigationController pushViewController:objoption animated:YES];
        [objoption release];	
	}
	
}

-(IBAction) TERMS_CONDITIONS
{
	appDelegate.LoginConform=FALSE;
	Terms_Condition *Terms= [[Terms_Condition alloc]init];
    [self.navigationController pushViewController:Terms animated:YES];
    [Terms release];
}

-(IBAction) Back_Click
{	appDelegate.flgBusinessRecording = TRUE;	
    OptionOfHistoryViewController *obj= [[OptionOfHistoryViewController alloc]init];
    [self.navigationController pushViewController:obj animated:YES];
    [obj release];
}
-(void) StartLoding
{
	HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(GetBuseinessVideoList) onTarget:self withObject:nil animated:YES];
}


-(void)GetBuseinessVideoList
{
	NSAutoreleasePool* pool = [[NSAutoreleasePool alloc] init];
	appDelegate.flgBusinessxml=TRUE;
	
	NSString * str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=fetchBusinessVideo&UserID=%@&version=2",appDelegate.ChangeUrl,appDelegate.userId];
	//NSLog(@"Use data id==%");
	
	NSLog(@"URL String=%@",str);
	
	
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	//Initialize the delegate.
	XMLVideo *parser = [[XMLVideo alloc] initXMLVideo];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		NSLog(@"No Errors");
		appDelegate.flgBusinessxml=FALSE;
		if([appDelegate.delarrBusinessList count] ==0) {
			UIAlertView *loginalert = [[UIAlertView alloc] initWithTitle:@" Review Frog" message:@"You haven’t uploaded any Business Video yet" delegate:self
													   cancelButtonTitle:@"OK" otherButtonTitles:nil];
			
			[loginalert show];
			[loginalert release];
			
		}
		
		NSLog(@"cnt %d",[appDelegate.delarrBusinessList count]);
		[self performSelectorOnMainThread:@selector(finishloading) withObject:nil waitUntilDone:YES];

	}
	else
	{
		NSLog(@"Error Error Error!!!");
		UIAlertView *altError = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Some error occurred. Please try again some time " delegate:self
												 cancelButtonTitle:@"OK" otherButtonTitles:nil];
		
		[altError show];
		[altError release];
	}
	[[UIApplication sharedApplication]setNetworkActivityIndicatorVisible:NO];
	
		[pool release];
}


-(void) finishloading
{
	[self.tblBusinessList reloadData];
}



- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    // Return the number of rows in the section.
	if (tableView==tblBusinessList)return [appDelegate.delarrBusinessList count]; 
	
	
	NSString *searchText = self.SearchCityBar.text;
	
	[self.filteredArray removeAllObjects];
	self.filteredArray = [[NSMutableArray alloc]init];
	for (int i=0 ;i<[appDelegate.delarrBusinessList count]; i++) {
		
		Video *avideo = [appDelegate.delarrBusinessList objectAtIndex:i];
		NSString *sTemp = avideo.ReviewVideoTitle;
		NSRange titleResultsRange = [sTemp rangeOfString:searchText options:NSCaseInsensitiveSearch];
		
		if (titleResultsRange.length > 0) {
			[self.filteredArray addObject:avideo];
		}
	}
	
	NSLog(@"ArrCounterBusiness:>%d",[self.filteredArray count]);
    return [self.filteredArray count];
}
- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	return 60.0;
}

// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
	
	
	static NSString *CellIdentifier = @"Cell";
    
	UITableViewCell *cell = [self.tblBusinessList dequeueReusableCellWithIdentifier:CellIdentifier];
	
	cell = [[[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier] autorelease];
    //set cell	
	CellBusinessVideo *objCell = [[CellBusinessVideo alloc]init];
	[cell.contentView addSubview:objCell.view];
	
	if (tableView == tblBusinessList) {
		
		objVideo = [appDelegate.delarrBusinessList objectAtIndex:indexPath.row];
		objCell.lblVideoName.text = [objVideo.ReviewVideoTitle stringByReplacingOccurrencesOfString:@"_" withString:@" "];		
		if([objVideo.VideoStatus isEqualToString:@"P"])
		{	
			objCell.lblVideoStatus.text = @"Pending";
			
			btnDelete = [[UIButton alloc] init];
			btnDelete = [UIButton buttonWithType:UIButtonTypeCustom];
			[btnDelete setFrame:CGRectMake(974, 15, 29, 31)];
			btnDelete.tag = indexPath.row;
			UIImage *imgbtn = [UIImage imageNamed:@"deletebtn.png"];
			[btnDelete setBackgroundImage:imgbtn forState:UIControlStateNormal];
			[btnDelete addTarget:self action:@selector(DeleteButton_Click:)forControlEvents:UIControlEventTouchUpInside];
			[cell.contentView addSubview:btnDelete];
		}
		else {
			
			objCell.lblVideoStatus.text = @"Ready";
			
			btnDelete = [[UIButton alloc] init];
			btnDelete = [UIButton buttonWithType:UIButtonTypeCustom];
			[btnDelete setFrame:CGRectMake(974, 15, 29, 31)];
			btnDelete.tag = indexPath.row;
			UIImage *imgbtn = [UIImage imageNamed:@"deletebtn.png"];
			[btnDelete setBackgroundImage:imgbtn forState:UIControlStateNormal];
			[btnDelete addTarget:self action:@selector(DeleteButton_Click:)forControlEvents:UIControlEventTouchUpInside];
			[cell.contentView addSubview:btnDelete];
			
			
			
		}
		
	}
	else {
		objVideo = [self.filteredArray objectAtIndex:indexPath.row];
		objCell.lblVideoName.text = [objVideo.ReviewVideoTitle stringByReplacingOccurrencesOfString:@"_" withString:@" "];
		
		if([objVideo.VideoStatus isEqualToString:@"P"])
		{	
			objCell.lblVideoStatus.text = @"Pending";
			
			btnDelete = [[UIButton alloc] init];
			btnDelete = [UIButton buttonWithType:UIButtonTypeCustom];
			[btnDelete setFrame:CGRectMake(974, 15, 29, 31)];
			btnDelete.tag = indexPath.row;
			UIImage *imgbtn = [UIImage imageNamed:@"deletebtn.png"];
			[btnDelete setBackgroundImage:imgbtn forState:UIControlStateNormal];
			[btnDelete addTarget:self action:@selector(DeleteButton_Click:)forControlEvents:UIControlEventTouchUpInside];
			[cell.contentView addSubview:btnDelete];
		}
		else {
			objCell.lblVideoStatus.text = @"Ready";
			
			objCell.lblVideoStatus.text = @"Ready";
			
			btnDelete = [[UIButton alloc] init];
			btnDelete = [UIButton buttonWithType:UIButtonTypeCustom];
			[btnDelete setFrame:CGRectMake(974, 15, 29, 31)];
			btnDelete.tag = indexPath.row;
			UIImage *imgbtn = [UIImage imageNamed:@"deletebtn.png"];
			[btnDelete setBackgroundImage:imgbtn forState:UIControlStateNormal];
			[btnDelete addTarget:self action:@selector(DeleteButton_Click:)forControlEvents:UIControlEventTouchUpInside];
			[cell.contentView addSubview:btnDelete];
			
		}
		
	}
		
	return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
	if (tableView == tblBusinessList) {
		
		objVideo = [appDelegate.delarrBusinessList objectAtIndex:indexPath.row];
		
		NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
		NSString *documentsDirectory = [paths objectAtIndex:0]; 
		
		NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
		NSString *videoName = objVideo.ReviewVideoName;
		
		appDelegate.BusinessVideoTitle =  objVideo.ReviewVideoName;
		
		NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,videoName];
		NSData *dt = [[NSData alloc] initWithContentsOfFile:exportPath];
		appDelegate.NewBusinessData = dt;
		NSLog(@"exportPath %@",exportPath);
		
		appDelegate.BusinessVideoPath = [NSURL fileURLWithPath:exportPath];
		NSLog(@"VideoURL:>%@",appDelegate.BusinessVideoPath);
		NSLog(@"Done");
		
	}
	else {
		objVideo = [self.filteredArray objectAtIndex:indexPath.row];
		
		NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
		NSString *documentsDirectory = [paths objectAtIndex:0]; 
		
		NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
		NSString *videoName = objVideo.ReviewVideoName;
		
		appDelegate.BusinessVideoTitle =  objVideo.ReviewVideoName;
		
		NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,videoName];
		NSData *dt = [[NSData alloc] initWithContentsOfFile:exportPath];
		appDelegate.NewBusinessData = dt;
		NSLog(@"exportPath %@",exportPath);
		
		appDelegate.BusinessVideoPath = [NSURL fileURLWithPath:exportPath];
		NSLog(@"VideoURL:>%@",appDelegate.BusinessVideoPath);
		NSLog(@"Done");
		
	}

		[self MoveToWebView];
}



-(void)DeleteButton_Click :(int)index
{
	appDelegate.btnDeleteBusiness = (UIButton *)index;
	NSLog(@"Btnselect-----%d",appDelegate.btnDeleteBusiness.tag);
	[self ToDeleteWatchRecode];
}
-(void)ToDeleteWatchRecode
{
	NSLog(@"DeleteShruffle");
	alertConfirm = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Are You Sure Want To Delete This Record?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes",nil];
	[alertConfirm show];
	[alertConfirm release];
	
}

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex 
{
	if (alertView == alertConfirm) {
		if (buttonIndex == 0) {
			NSLog(@"No");
		} else {
			[self DeleteDone];
		}
	}
}
-(void)DeleteDone
{
	NSLog(@"Delete Done");
	
	objVideo = [appDelegate.delarrBusinessList objectAtIndex:appDelegate.btnDeleteBusiness.tag];
	
	//NSFileManager *fileManager = [NSFileManager defaultManager];
	NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES); 
	NSString *documentsDirectory = [paths objectAtIndex:0]; 
	
	NSString *albumPath = [documentsDirectory stringByAppendingPathComponent:@"MyReviewFrog"];
	
	NSString *videoName = objVideo.ReviewVideoName;
	
	NSString *exportPath = [NSString stringWithFormat:@"%@/%@",albumPath,videoName];
	NSLog(@"exportPath %@",exportPath);
	
	NSFileManager *fileManager = [NSFileManager defaultManager];
	if([fileManager fileExistsAtPath:exportPath]){
		NSError *err;
		[fileManager removeItemAtPath:exportPath error:&err];
	}
	else{
		NSLog(@"Video Not Exist");
	}
	
	
	//Delete Frog Database
	
	
	NSString * str = [[[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=RemoveBusinessVideo&videoid=%d&version=2",appDelegate.ChangeUrl,objVideo.id]stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    
	
	NSLog(@"URL String=%@",str);
	
	
	NSMutableURLRequest *request = [[[NSMutableURLRequest alloc] init] autorelease];
	[request setURL:[NSURL URLWithString:str]];
	[request setHTTPMethod:@"GET"];
	
	NSURLResponse *response = NULL;
	NSError *requestError = NULL;
	NSData *responseData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&requestError];
	NSString *responseString = [[[NSString alloc] initWithData:responseData encoding:NSUTF8StringEncoding] autorelease];
	responseString = [responseString stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
	
	NSLog(@"responseString %@",responseString);
	NSRange rngXMLNode = [responseString rangeOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>"];
	NSRange rngReturnStart = [responseString rangeOfString:@"<return>"];
	NSRange rngReturnEnd = [responseString rangeOfString:@"</return>"];
	
	if (rngXMLNode.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" withString:@""];
	
	if (rngReturnStart.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"<return>" withString:@""];
	
	if (rngReturnEnd.length > 0)
		responseString = [responseString stringByReplacingOccurrencesOfString:@"</return>" withString:@""];
	
	if ([responseString isEqualToString:@"1"]) {
		NSLog(@"Done");
		[appDelegate.delarrBusinessList removeObjectAtIndex:appDelegate.btnDeleteBusiness.tag];
		[self.tblBusinessList reloadData];
		
		
		UIAlertView *alrDelete = [[UIAlertView alloc]initWithTitle:@"Review Frog" message:@"Your Record Delete Successfully" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil];		
		[alrDelete show];
		[alrDelete release];
		
		
	}
	else {
		NSLog(@"Erro");
	}
	
	
}

-(void) MoveToWebView
{
	PlayBusinessVideoViewController *objvideoplay= [[PlayBusinessVideoViewController alloc]init];
	objvideoplay.avideo = objVideo;
    [self.navigationController pushViewController:objvideoplay animated:YES];
    [objvideoplay release];
}
-(IBAction) BusinessVideoRecodingClick
{
	if ([UIImagePickerController isSourceTypeAvailable:
		 UIImagePickerControllerSourceTypeCamera])
	{
		UIImagePickerController *imagePicker =
		[[UIImagePickerController alloc] init];
		imagePicker.delegate = self;
		imagePicker.sourceType = 
		UIImagePickerControllerSourceTypeCamera;
		imagePicker.cameraDevice = UIImagePickerControllerCameraDeviceFront;
		imagePicker.mediaTypes = [NSArray arrayWithObjects:(NSString *) kUTTypeMovie,nil];
		imagePicker.allowsEditing = NO;
		
		imagePicker.cameraCaptureMode = UIImagePickerControllerCameraCaptureModeVideo;
		
		imagePicker.videoMaximumDuration = 30;
		
		[self presentModalViewController:imagePicker animated:YES];
		[imagePicker release];
	}
	
}
-(void)imagePickerController:(UIImagePickerController *)picker
didFinishPickingMediaWithInfo:(NSDictionary *)info
{
	
	NSString *mediaType = [info objectForKey:UIImagePickerControllerMediaType];
	flgBusinessVideo=TRUE;		
	if([mediaType isEqualToString:(NSString *)kUTTypeMovie]) 
	{				
		appDelegate.BusinessUrl = [info objectForKey:UIImagePickerControllerMediaURL];
		NSLog(@"Businessurl:%@",appDelegate.BusinessUrl);
		
		appDelegate.BusinessDataVideo = [NSData dataWithContentsOfURL:appDelegate.BusinessUrl];
		
		[picker dismissModalViewControllerAnimated:YES];
		
	}
	
}
-(void)viewDidAppear:(BOOL)animated
{	
	if (flgBusinessVideo==TRUE) {
		flgBusinessVideo=FALSE;
		BusinessVideoViewController *objBusinessVideo = [[BusinessVideoViewController alloc]init];
        [self.navigationController pushViewController:objBusinessVideo animated:YES];
        [objBusinessVideo release];
	}
	
}



- (void)didReceiveMemoryWarning {
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc. that aren't in use.
}


- (void)viewDidUnload {
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}


- (void)dealloc {
    [super dealloc];
}


@end
