    //
//  SearchCityViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 5/26/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "SearchCityViewController.h"
#import "ApplicationValidation.h"

@implementation SearchCityViewController
@synthesize tblCityList;
@synthesize btnA;
@synthesize btnB;
@synthesize SearchCityBar;
@synthesize searchDC;
@synthesize filteredArray;

@synthesize delegate;


 // The designated initializer.  Override if you create the controller programmatically and want to perform customization that is not appropriate for viewDidLoad.
/*
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil {
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization.
    }
    return self;
}
*/


// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	SearchCityBar.delegate = self;
	
	self.searchDC = [[[UISearchDisplayController alloc] initWithSearchBar:self.SearchCityBar contentsController:self] autorelease];
	self.searchDC.searchResultsDataSource = self;
	self.searchDC.searchResultsDelegate = self;
	SearchCityBar.showsCancelButton = NO;
	
	
			
	
}
-(void)viewWillAppear:(BOOL)animated
{
	[btnA setTitleColor:[UIColor redColor] forState:UIControlStateNormal];
	appDelegate.btntitle = @"A";
	[self doLoadShruffleList];

}
- (void)searchBarSearchButtonClicked:(UISearchBar *)searchBar
{
	NSLog(@"SearchString------%@",searchBar.text);
	appDelegate.strSearch = searchBar.text;
	
	
}

- (void) doLoadShruffleList {
    // The hud will dispable all input on the view (use the higest view possible in the view hierarchy)
    HUD = [[MBProgressHUD alloc] initWithView:self.view];
	
    // Add HUD to screen
    [self.view addSubview:HUD];
	
    // Regisete for HUD callbacks so we can remove it from the window at the right time
    HUD.delegate = self;
	
    HUD.labelText = @"Loading";
	
    // Show the HUD while the provided method executes in a new thread
    [HUD showWhileExecuting:@selector(loadstart) onTarget:self withObject:nil animated:YES];
}

-(IBAction)Click_button:(id)sender
{	
	[btnA setTitleColor:[UIColor blackColor] forState:UIControlStateNormal];
	
	UIButton *btn = (UIButton *)sender;
	int temp = btn.tag;
	appDelegate.btntitle = btn.titleLabel.text;
	[btn setTitleColor:[UIColor redColor]forState:UIControlStateNormal];
	[btnprvious setTitleColor:[UIColor blackColor]forState:UIControlStateNormal];
	NSLog(@"ButtonTittle-----%@",appDelegate.btntitle);
	NSLog(@"Tagvalue----%d",temp);
	btnprvious = btn;
	[self doLoadShruffleList];
}
-(IBAction)Click_button2:(id)sender
{


}
-(void) loadstart{
	
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	NSString * str = [[NSString alloc]initWithFormat:@"%@frmReviewFrogApi_ver2.php?api=searchCity&city=%@&version=2",appDelegate.ChangeUrl,appDelegate.btntitle];
	
	NSLog(@"URL String=%@",str);
		
	NSURL *url = [NSURL URLWithString:str];
	NSData *xmldata=[[NSData alloc]initWithContentsOfURL:url];
	
	NSXMLParser *xmlParser = [[NSXMLParser alloc] initWithData:xmldata];
	
	
	//Initialize the delegate.
	XMLCityname *parser = [[XMLCityname alloc] initXMLCityname];
	
	//Set delegate
	[xmlParser setDelegate:parser];
	
	//Start parsing the XML file.
	BOOL success = [xmlParser parse];
	
	if(success) {
		[self performSelectorOnMainThread:@selector(ReloadTable) withObject:nil waitUntilDone:YES];	
		//	[self.tblCityList reloadData];
	} else
		NSLog(@"Error Error Error!!!");
	
	[pool release];
	
}
-(void) ReloadTable
{
	[self.tblCityList reloadData];
}
- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView {
	
	return 1;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
	if (tableView == self.tblCityList) return [appDelegate.CityList count];
	
	NSString *searchText = self.SearchCityBar.text;
	
	[self.filteredArray removeAllObjects];
	self.filteredArray = [[NSMutableArray alloc]init];
	for (int i=0; i<[appDelegate.CityList count]; i++) {

		Cityname *objcityname = [appDelegate.CityList objectAtIndex:i];
		NSLog(@"categaoryname-----%@",objcityname);	
		NSString *sTemp =objcityname.cityName;
		NSRange titleResultsRange = [sTemp rangeOfString:searchText options:NSCaseInsensitiveSearch];
		
		if (titleResultsRange.length > 0)
		{
			[self.filteredArray addObject:objcityname];
			
		}
	}
	NSLog(@"FilterArray-----%d",[filteredArray count]);
	return self.filteredArray.count;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
	
	static NSString *CellIdentifier = @"Cell";
	
	UITableViewCell *cell = [self.tblCityList dequeueReusableCellWithIdentifier:CellIdentifier];
	//if (cell == nil) {
	cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];
	//}
	if (tableView == tblCityList) {
		
	Cityname *objcityname = [appDelegate.CityList objectAtIndex:indexPath.row];
	cell.textLabel.text = objcityname.cityName;	
	}
	else {
		Cityname *objcityname = [self.filteredArray objectAtIndex:indexPath.row];
		cell.textLabel.text = objcityname.cityName;
		
	}
	
	cell.selectionStyle = UITableViewCellSelectionStyleBlue;
	return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
	
	if (tableView == tblCityList) 
	{	appDelegate.txtcleanflag=TRUE;
		Cityname *objcityname=[appDelegate.CityList objectAtIndex:indexPath.row];
		appDelegate.CityName=objcityname.cityName;
		NSLog(@"City name---%@",appDelegate.CityName);
		[self.delegate didTap];
	}
	else 
	{
		appDelegate.txtcleanflag=TRUE;
		Cityname *objcityname=[self.filteredArray objectAtIndex:indexPath.row];
		appDelegate.CityName=objcityname.cityName;
		NSLog(@"City name---%@",appDelegate.CityName);
		[self.delegate didTap];
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
