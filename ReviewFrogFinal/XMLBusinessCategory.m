//
//  XMLBusinessCategory.m
//  Review Frog
//
//  Created by agilepc-32 on 10/7/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLBusinessCategory.h"


@implementation XMLBusinessCategory
-(XMLBusinessCategory *) initXMLBusinessCategory
{
	[super init];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
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
	if ([elementName isEqualToString:@"usertabledata"]) {
		appDelegate.delarrBusinessCategory = [[NSMutableArray alloc]init];
	}
	else if ([elementName isEqualToString:@"details"]){
		aBcategory = [[BusinessCategory alloc]init];
			}
	
}

- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
	if ([elementName isEqualToString:@"usertabledata"])
		return;
	
	if ([elementName isEqualToString:@"details"]) {
		[appDelegate.delarrBusinessCategory addObject:aBcategory];
		NSLog(@"CategoryList");
		[aBcategory release];
		aBcategory = nil;
	}
	else 
	{
		@try {
			
			[aBcategory setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
			
			[currentElementValue release];
			currentElementValue = nil;
			
		}
		@catch (NSException * e) {
			
		}
	}
	
}

@end


