//
//  XMLCategoryList.m
//  Review Frog
//
//  Created by AgileMac4 on 8/29/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLCategoryList.h"


@implementation XMLCategoryList

-(XMLCategoryList *) initXMLCategoryList
{	
	[super init];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	
	return self;
}

- (void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string
{
		if (!currentElementValue) 
			currentElementValue = [[NSMutableString alloc] initWithString:string];
		else 
			[currentElementValue appendString:string];
}
- (void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qualifiedName
	attributes:(NSDictionary *)attributeDict
{
	if ([elementName isEqualToString:@"Categories"]) {
		appDelegate.delArrCategoryWiseList = [[NSMutableArray alloc]init];
	}
	else if ([elementName isEqualToString:@"category"]){
		acategorywiselist = [[CategoryWiseList alloc]init];
		acategorywiselist.name = [attributeDict objectForKey:@"name"];
	}

}

- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
	if ([elementName isEqualToString:@"Categories"])
		return;
	
	if ([elementName isEqualToString:@"category"]) {
		[appDelegate.delArrCategoryWiseList addObject:acategorywiselist];
		NSLog(@"CategoryList");
		[acategorywiselist release];
		acategorywiselist = nil;
	}
	else 
	{
		@try {
			
			[acategorywiselist setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
			
			[currentElementValue release];
			currentElementValue = nil;
			
		}
		@catch (NSException * e) {
			
		}
	}

}

@end
