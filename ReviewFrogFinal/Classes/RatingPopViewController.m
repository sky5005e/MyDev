//
//  RatingPopViewController.m
//  Review Frog
//
//  Created by AgileMac4 on 4/27/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "RatingPopViewController.h"

@implementation RatingPopViewController

@synthesize delegate;

@synthesize index;

@synthesize QuestionText,checkedCell;

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

    // Uncomment the following line to preserve selection between presentations.
    // self.clearsSelectionOnViewWillAppear = NO;
 
    // Uncomment the following line to display an Edit button in the navigation bar for this view controller.
    // self.navigationItem.rightBarButtonItem = self.editButtonItem;
    
    
    ratings=[[NSDictionary alloc] initWithObjectsAndKeys:[NSArray arrayWithObjects:@"2",@"1", nil],@"Poor",[NSArray arrayWithObjects:@"4",@"3", nil],@"Fair",[NSArray arrayWithObjects:@"6",@"5", nil],@"Good",[NSArray arrayWithObjects:@"8",@"7", nil],@"Great",[NSArray arrayWithObjects:@"10",@"9", nil],@"Excellent", nil];
    
    sectionNames=[[NSDictionary alloc] initWithObjectsAndKeys:@"Poor",@"0",@"Fair",@"1",@"Good",@"2",@"Great",@"3",@"Excellent",@"4", nil];
    
    appDel=(Review_FrogAppDelegate*)[[UIApplication sharedApplication] delegate];
    
    question.text=[NSString stringWithFormat:@"How Would You Rate %@?",self.QuestionText];
 
}

- (void)viewDidUnload
{
    [super viewDidUnload];
    // Release any retained subviews of the main view.
    // e.g. self.myOutlet = nil;
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
    
    appDel.rating1Bool=NO;
    appDel.rating2Bool=NO;
    appDel.rating3Bool=NO;
}

- (void)viewDidAppear:(BOOL)animated
{
    [super viewDidAppear:animated];
}

- (void)viewWillDisappear:(BOOL)animated
{
    [super viewWillDisappear:animated];
}

- (void)viewDidDisappear:(BOOL)animated
{
    [super viewDidDisappear:animated];
}

- (BOOL)shouldAutorotateToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation
{
    // Return YES for supported orientations
    return (interfaceOrientation == UIInterfaceOrientationPortrait);
}

#pragma mark - Table view data source

- (NSInteger)numberOfSectionsInTableView:(UITableView *)tableView
{
    // Return the number of sections.
    return 5;
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section
{
    // Return the number of rows in the section.
    return 2;
}

- (NSString *)tableView:(UITableView *)tableView titleForHeaderInSection:(NSInteger)section{
    
    return [sectionNames objectForKey:[NSString stringWithFormat:@"%d",4-section]];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath
{
    static NSString *CellIdentifier = @"Cell";
    
    UITableViewCell *cell = [tableView dequeueReusableCellWithIdentifier:CellIdentifier];
    //if (cell == nil) {
        cell = [[[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:CellIdentifier] autorelease];
    //}
    
    // Configure the cell...
    
    if ([self.checkedCell isEqualToString:[[ratings objectForKey:[sectionNames objectForKey:[NSString stringWithFormat:@"%d",4-indexPath.section]]] objectAtIndex:indexPath.row]]) {
        
        cell.accessoryType=UITableViewCellAccessoryCheckmark;
        
        //selectedCell=[[UITableViewCell alloc] init];
       // selectedCell=cell;
        Sec = indexPath.section;
        row = indexPath.row;
    }
    
    cell.textLabel.text=[[ratings objectForKey:[sectionNames objectForKey:[NSString stringWithFormat:@"%d",4-indexPath.section]]] objectAtIndex:indexPath.row];
    
    return cell;
}

/*
// Override to support conditional editing of the table view.
- (BOOL)tableView:(UITableView *)tableView canEditRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Return NO if you do not want the specified item to be editable.
    return YES;
}
*/

/*
// Override to support editing the table view.
- (void)tableView:(UITableView *)tableView commitEditingStyle:(UITableViewCellEditingStyle)editingStyle forRowAtIndexPath:(NSIndexPath *)indexPath
{
    if (editingStyle == UITableViewCellEditingStyleDelete) {
        // Delete the row from the data source
        [tableView deleteRowsAtIndexPaths:[NSArray arrayWithObject:indexPath] withRowAnimation:UITableViewRowAnimationFade];
    }   
    else if (editingStyle == UITableViewCellEditingStyleInsert) {
        // Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view
    }   
}
*/

/*
// Override to support rearranging the table view.
- (void)tableView:(UITableView *)tableView moveRowAtIndexPath:(NSIndexPath *)fromIndexPath toIndexPath:(NSIndexPath *)toIndexPath
{
}
*/

/*
// Override to support conditional rearranging of the table view.
- (BOOL)tableView:(UITableView *)tableView canMoveRowAtIndexPath:(NSIndexPath *)indexPath
{
    // Return NO if you do not want the item to be re-orderable.
    return YES;
}
*/

#pragma mark - Table view delegate

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath
{
    NSIndexPath *indexP = [NSIndexPath indexPathForRow:row inSection:Sec];
    UITableViewCell *cell1 = [tableView cellForRowAtIndexPath:indexP];
    [cell1 setAccessoryType:UITableViewCellAccessoryNone];
    // Navigation logic may go here. Create and push another view controller.
    /*
     <#DetailViewController#> *detailViewController = [[<#DetailViewController#> alloc] initWithNibName:@"<#Nib name#>" bundle:nil];
     // ...
     // Pass the selected object to the new view controller.
     [self.navigationController pushViewController:detailViewController animated:YES];
     [detailViewController release];
     */
    
   // [selectedCell setAccessoryType:UITableViewCellAccessoryNone];
    
    [[tableView cellForRowAtIndexPath:indexPath] setAccessoryType:UITableViewCellAccessoryCheckmark];
    
    switch (self.index) {
            
        case 1:
            
            appDel.rating1=[[ratings objectForKey:[sectionNames objectForKey:[NSString stringWithFormat:@"%d",4-indexPath.section]]] objectAtIndex:indexPath.row];
            
            appDel.rating1Bool=YES;
            
            break;
            
        case 2:
            
            appDel.rating2=[[ratings objectForKey:[sectionNames objectForKey:[NSString stringWithFormat:@"%d",4-indexPath.section]]] objectAtIndex:indexPath.row];
           
            appDel.rating2Bool=YES;
            
            break;
            
        case 3:
            
            appDel.rating3=[[ratings objectForKey:[sectionNames objectForKey:[NSString stringWithFormat:@"%d",4-indexPath.section]]] objectAtIndex:indexPath.row];
            
            appDel.rating3Bool=YES;
            
            break;
            
        default:
            break;
    }
    
    [self.delegate didSelectRating];
}

@end
