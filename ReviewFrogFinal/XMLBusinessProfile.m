//
//  XMLBusinessProfile.m
//  Review Frog
//
//  Created by agilepc-32 on 10/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLBusinessProfile.h"


@implementation XMLBusinessProfile

-(XMLBusinessProfile *) initXMLBusinessProfile{
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
	if ([elementName isEqualToString:@"listextraprofile"]) {
		appDelegate.objBprofile = [[Businessprofile alloc]init];
	}

}

- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
	if ([elementName isEqualToString:@"businessinfo"])
		return;
	
	if ([elementName isEqualToString:@"business"]) {
				
	}
	else 
	{
		@try {
			
			[appDelegate.objBprofile setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
			
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
