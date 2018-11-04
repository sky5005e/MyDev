//
//  xmlparser.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 03/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "xmlparser.h"
#import "Review_FrogAppDelegate.h"

@implementation xmlparser

		//xmlparser for hardcode login in

-(xmlparser *) initxmlparser
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
	if ([elementName isEqualToString:@"userdata"]) {
		appDelegate.delArrUerinfo = [[NSMutableArray alloc]init];
	
	}
	else if ([elementName isEqualToString:@"user"]){
		alogininfo = [[LoginInfo alloc]init];
		alogininfo.id = [[attributeDict objectForKey:@"id"] integerValue];
		NSLog(@"alogininfo:-%d",alogininfo.id);
	}
}
- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
	if ([elementName isEqualToString:@"userdata"])
		return;
	
		if ([elementName isEqualToString:@"user"]) {
			[appDelegate.delArrUerinfo addObject:alogininfo];
			[alogininfo release];
			alogininfo=nil;
		}
		else 
		{
			@try {
				appDelegate.FirstUser=TRUE;
				[alogininfo setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
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
