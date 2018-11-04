//
//  XMLCityname.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 28/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLCityname.h"
#import "Cityname.h"


@implementation XMLCityname

- (XMLCityname *) initXMLCityname
{
	
	[super init];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	
	return self;
}

- (void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qualifiedName
	attributes:(NSDictionary *)attributeDict {
	if([elementName isEqualToString:@"usertabledata"]) {
		//Initialize the array.
		appDelegate.CityList = [[NSMutableArray alloc] init];
	}
	else if([elementName isEqualToString:@"details"]) {
		
		//Initialize the book.
		acityname = [[Cityname alloc] init];
		
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
		//if(!acityname.cityName)
			[appDelegate.CityList addObject:acityname];
		
		[acityname release];
		acityname = nil;
	}
	else {
		
		@try {
			[acityname setValue:currentElementValue forKey:elementName];
			
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
