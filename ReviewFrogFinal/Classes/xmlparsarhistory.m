//
//  xmlparsarhistory.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 04/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "xmlparsarhistory.h"
#import "Review_FrogAppDelegate.h"



@implementation xmlparsarhistory
- (xmlparsarhistory *) initxmlparsarhistory
 {
	
	[super init];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	//appDelegate.HistoryList=[[NSMutableArray alloc]init];
	return self;
}

-(void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName attributes:(NSDictionary *)attributeDict {
	//	NSLog(@"did start element");
	
	if([elementName isEqualToString:@"usertabledata"]) {
		//Initialize the array.
		if (appDelegate.flgsearch==TRUE)
		{
			appDelegate.delArrSearchList= [[NSMutableArray alloc] init];
		}

	}
	else if([elementName isEqualToString:@"details"]) {
		
		objUser = [[User alloc] init];
				
	}
}
-(void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName{
	//NSLog(@"did end element");
	
	if([elementName isEqualToString:@"usertabledata"])
	return;
	
	if([elementName isEqualToString:@"details"]) {
		if (appDelegate.flgsearch==TRUE) {
			[appDelegate.delArrSearchList addObject:objUser];
			[objUser release];
			objUser = nil;

		}
		
		//else if(!objUser.return1){
		else {
			[appDelegate.HistoryList addObject:objUser];
		
			[objUser release];
			objUser = nil;
		}
				
	} else 
	
	{
		@try {
			[objUser setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] forKey:elementName];

		}
		@catch (NSException * e) {
		
		}
							
	}
	
	[currentElementValue release];
	currentElementValue = nil;
}

-(void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string
{
	if(!currentElementValue)
		currentElementValue = [[NSMutableString alloc] initWithString:string];
	else
		[currentElementValue appendString:string];
}
- (void)parser:(NSXMLParser *)parser foundCDATA:(NSData *)CDATABlock
{	
    currentElementValue = [[NSString alloc] initWithData:CDATABlock encoding:NSUTF8StringEncoding];
}

@end
