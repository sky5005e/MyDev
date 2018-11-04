    //
//  DefaultCategoryViewController.m
//  Review Frog
//
//  Created by agilepc-32 on 11/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "DefaultCategoryViewController.h"
#import "Category.h"

@implementation DefaultCategoryViewController
@synthesize tblCategory;
@synthesize delegate; 

 

// Implement viewDidLoad to do additional setup after loading the view, typically from a nib.
- (void)viewDidLoad {
    [super viewDidLoad];
	
	appDelegate = (Review_FrogAppDelegate*)[[UIApplication sharedApplication]delegate];	
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
	return [appDelegate.ArrCategoryList count];
}
- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	return 60.0;
}

// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	
	UITableViewCell *cell = [self.tblCategory dequeueReusableCellWithIdentifier:CellIdentifier];
	//if (cell == nil) {
	cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];
	
	Category *acategory = [[Category alloc]init];
	
	acategory = [appDelegate.ArrCategoryList objectAtIndex:indexPath.row];
	
	cell.textLabel.text = acategory.categoryname;
	
	return cell;
}
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
	Category *acategory = [appDelegate.ArrCategoryList objectAtIndex:indexPath.row];
	appDelegate.strDefaultCategory = acategory.categoryname;
	[self.delegate didSelectDefaultCategory];
	
}
- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation 
{
	
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
