//
//  SubcategoryPopViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 4/13/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "SubcategoryPopViewController.h"
#import "Category.h"

@implementation SubcategoryPopViewController
@synthesize delegate; 
@synthesize index;
- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}

- (void)didReceiveMemoryWarning
{
    // Releases the view if it doesn't have a superview.
    [super didReceiveMemoryWarning];
    
    // Release any cached data, images, etc that aren't in use.
}

#pragma mark - View lifecycle

- (void)viewDidLoad
{
    [super viewDidLoad];
    
    appDelegate = (Review_FrogAppDelegate*)[[UIApplication sharedApplication]delegate];
    
    NSLog(@"index == %d",self.index);
    
    // Do any additional setup after loading the view from its nib.
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    NSLog(@"[appDelegate.categoryFromDB count] %d",[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] count]);
	return [[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] count];
}

- (NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section{
    
   return [[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"];
}


- (CGFloat)tableView:(UITableView *)tableView heightForRowAtIndexPath:(NSIndexPath *)indexPath {
	return 60.0;
}

// Customize the appearance of table view cells.
- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
	static NSString *CellIdentifier = @"Cell";
	
	UITableViewCell *cell = [tblCategory dequeueReusableCellWithIdentifier:CellIdentifier];
	//if (cell == nil) {
    
	cell = [[[UITableViewCell alloc] initWithFrame:CGRectZero reuseIdentifier:CellIdentifier] autorelease];
	
	cell.textLabel.text = [[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] allKeys] objectAtIndex:indexPath.row];
	
	return cell;
}
- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    
    switch (self.index) {
                    
        case 0:
            
            appDelegate.ansOne=YES;
            
            appDelegate.answerOneId=[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] objectForKey:[[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] allKeys] objectAtIndex:indexPath.row]];
            
            appDelegate.answerOne=[[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] allKeys] objectAtIndex:indexPath.row];
            
            appDelegate.questionOneId=[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"id"];
            
            break;
            
        case 1:
            
            appDelegate.ansTwo=YES;
            
            appDelegate.answerTwoId=[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] objectForKey:[[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] allKeys] objectAtIndex:indexPath.row]];
            
            appDelegate.answerTwo=[[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] allKeys] objectAtIndex:indexPath.row];
            
            appDelegate.questionTwoId=[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"id"];
            
            break;
            
        case 2:
            
            appDelegate.ansThrre=YES;
            
            appDelegate.answerThreeId=[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] objectForKey:[[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] allKeys] objectAtIndex:indexPath.row]];
            
            appDelegate.answerthree=[[[[appDelegate.questions objectAtIndex:self.index] objectForKey:[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"question"]] allKeys] objectAtIndex:indexPath.row];
            
            appDelegate.questionThreeId=[[appDelegate.questions objectAtIndex:self.index] objectForKey:@"id"];
            
            break;
            
        default:
            
            break;
    }
    
    [self.delegate didSelectAnswer];
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


@end
