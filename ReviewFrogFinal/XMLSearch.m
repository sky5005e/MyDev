//
//  XMLSearch.m
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLSearch.h"


@implementation XMLSearch

-(XMLSearch *) initXMLSearch
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
	if ([elementName isEqualToString:@"businessinfo"]) {
		appDelegate.delArrSearch = [[NSMutableArray alloc]init];
	}
	else if ([elementName isEqualToString:@"business"]){
		asearch = [[Search alloc]init];
		asearch.id = [[attributeDict objectForKey:@"id"] integerValue];
	}
	
}

- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
	if ([elementName isEqualToString:@"businessinfo"])
		return;
	
	if ([elementName isEqualToString:@"business"]) {
		[appDelegate.delArrSearch addObject:asearch];
		
		[asearch release];
		asearch = nil;
	}
	else 
	{
		@try {
			
			[asearch setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
			
			[currentElementValue release];
			currentElementValue = nil;
			
		}
		@catch (NSException * e) {
			
		}
	}
}
- (void)parser:(NSXMLParser *)parser foundCDATA:(NSData *)CDATABlock
{	
    currentElementValue = [[NSString alloc] initWithData:CDATABlock encoding:NSUTF8StringEncoding];
}

@end

