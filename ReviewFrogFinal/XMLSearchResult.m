//
//  XMLSearchResult.m
//  Review Frog
//
//  Created by agilepc-32 on 10/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLSearchResult.h"


@implementation XMLSearchResult

-(XMLSearchResult *) initXMLSearchResult
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
		appDelegate.delArrSearchResult = [[NSMutableArray alloc]init];
		appDelegate.delArrBusinessSearch = [[NSMutableArray alloc]init];
		
	}
	else if ([elementName isEqualToString:@"business"]){
		aBSearch = [[BusinessSearch alloc]init];
		aBSearch.Bid = [[attributeDict objectForKey:@"id"] integerValue];
		NSLog(@"BSearch:-%d",aBSearch.Bid);
		
	}
	else if([elementName isEqualToString:@"review"]){
		flgsearch=TRUE;
		aResult = [[SearchResult alloc]init];
		aResult.Rid = [[attributeDict objectForKey:@"id"] integerValue];
	}

}
- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
	if ([elementName isEqualToString:@"businessinfo"])
		return;
	if (flgsearch==FALSE) {
		if ([elementName isEqualToString:@"business"]) {
			[appDelegate.delArrBusinessSearch addObject:aBSearch];
			[aBSearch release];
			aBSearch=nil;
		}
		else 
		{
			@try {
				
				[aBSearch setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
				[currentElementValue release];
				currentElementValue = nil;
				
			}
			@catch (NSException * e) {
				
			}
		}
		
	}
	if (flgsearch==TRUE) {
		if ([elementName isEqualToString:@"review"]){
			[appDelegate.delArrSearchResult addObject:aResult];
			NSLog(@"count xml %d",[appDelegate.delArrSearchResult count]);
			NSLog(@"DoFill");
			[aResult release];
			aResult = nil;
		}
		else 
		{
			@try {
				
				[aResult setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
				[currentElementValue release];
				currentElementValue = nil;
				
			}
			@catch (NSException * e) {
				
			}
		}
		
	}
}
- (void)parser:(NSXMLParser *)parser foundCDATA:(NSData *)CDATABlock
{	
    currentElementValue = [[NSString alloc] initWithData:CDATABlock encoding:NSUTF8StringEncoding];
}


@end
