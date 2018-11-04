//
//  xmlHistoryLogin.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 27/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "xmlHistoryLogin.h"
#import "History.h"
#import "HistoryLogin.h"


@implementation xmlHistoryLogin

- (xmlHistoryLogin *) initxmlHistoryLogin
{
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	appDelegate.CityList=[[NSMutableArray alloc]init];
	
	
	return self;
}

-(void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName attributes:(NSDictionary *)attributeDict {
	//	NSLog(@"did start element");
	
	if([elementName isEqualToString:@"usertabledata"]) {
		//Initialize the array.
		appDelegate.CityList = [[NSMutableArray alloc] init];
	}
	else if([elementName isEqualToString:@"details"]) {
		
		//Initialize the book.
		objadmin = [[AdminEmail alloc] init];
		
		//Extract the attribute here.
		
	}
	
	NSLog(@"Processing Element: %@", elementName);
	
}
-(void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName{
	NSLog(@"did end element");
	
	if([elementName isEqualToString:@"usertabledata"])
		return;
	
	//There is nothing to do if we encounter the Books element here.
	//If we encounter the Book element howevere, we want to add the book object to the array
	// and release the object.
	if([elementName isEqualToString:@"details"]) {
		[appDelegate.CityList addObject:objadmin];
		
		[objadmin release];
		objadmin = nil;
	} else {
		
		@try {
			[objadmin setValue:currentElementValue forKey:elementName];
			[currentElementValue release];
			currentElementValue = nil;

		}
		@catch (NSException * e) {
			
		}
		
	}
}

-(void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string
{   
	
	if(!currentElementValue)
		currentElementValue = [[NSMutableString alloc] initWithString:string];
	else
		[currentElementValue appendString:string];
	
	NSLog(@"Processing Value: %@", currentElementValue);
	
}


@end
