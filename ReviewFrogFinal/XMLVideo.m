//
//  XMLVideo.m
//  Review Frog
//
//  Created by AgileMac4 on 6/23/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLVideo.h"
#import "Review_FrogAppDelegate.h"
#import "Video.h"


@implementation XMLVideo

-(XMLVideo *) initXMLVideo
{
	[super init];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
		
	return self;
}
#pragma mark -
#pragma mark Feed list parser

- (void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qualifiedName
	attributes:(NSDictionary *)attributeDict {
	
	

	
	if([elementName isEqualToString:@"Videos"]) {
		//Initialize the array.
		
		if (appDelegate.flgBusinessxml==TRUE) {
			appDelegate.delarrBusinessList = [[NSMutableArray alloc]init];
		}
		else if (appDelegate.flgVideoSearch==TRUE) {
			appDelegate.delArrvieoSearchList = [[NSMutableArray alloc]init];
		}
		
		
		
	}
	else if([elementName isEqualToString:@"Video"]) {
		
		//Initialize
		avideo = [[Video alloc] init];
		
		//Extract the attribute here.
		avideo.id = [[attributeDict objectForKey:@"id"] integerValue];
		NSLog(@"Video --> id %d",avideo.id);
	}
	
	//	NSLog(@"Processing Element: %@", elementName);
}

- (void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string {
	
	if(!currentElementValue)
		currentElementValue = [[NSMutableString alloc] initWithString:string];
	else
		[currentElementValue appendString:string];
	
	//	NSLog(@"Processing Value: %@", currentElementValue);
	
}

- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName {
	
	if([elementName isEqualToString:@"Videos"])
		return;
	
	//There is nothing to do if we encounter the Books element here.
	//If we encounter the Book element howevere, we want to add the book object to the array
	// and release the object.
	if([elementName isEqualToString:@"Video"]) {
			
		if (appDelegate.flgBusinessxml==TRUE) {
			
			[appDelegate.delarrBusinessList addObject:avideo];
			NSLog(@"BusinessVideoList");
		}
		else if(appDelegate.flgVideoSearch==TRUE)
		{
			[appDelegate.delArrvieoSearchList addObject:avideo];
		}
		else {
			
			[appDelegate.delarrVideoList addObject:avideo];
			
			NSLog(@"VideoList");
		}

					
		[avideo release];
		avideo = nil;
	}
	else{
	
		@try {
			[avideo setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] forKey:elementName];
			[currentElementValue release];
			currentElementValue = nil;

		}
		@catch (NSException * e) {
		
		}
		
	}
		
	
	}

@end
