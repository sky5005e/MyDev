//
//  XMLGallery.m
//  Review Frog
//
//  Created by agilepc-32 on 9/19/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLGallery.h"


@implementation XMLGallery

-(XMLGallery *) initXMLGallery
{
	[super init];
	
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];

	return self;
}
- (void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string {
	
	if(!currentElementValue)
		currentElementValue = [[NSMutableString alloc] initWithString:string];
	else
		[currentElementValue appendString:string];
	
	//	NSLog(@"Processing Value: %@", currentElementValue);
	
}


#pragma mark -
#pragma mark Feed list parser

- (void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qualifiedName
	attributes:(NSDictionary *)attributeDict {
	
	if([elementName isEqualToString:@"galleryinfo"]) {
		//Initialize the array.
			
			appDelegate.delarrGalleryList = [[NSMutableArray alloc]init];
				
	}
	else if([elementName isEqualToString:@"gallery"]) {
		
		//Initialize
		agallery = [[Gallery alloc] init];
		
		//Extract the attribute here.
		agallery.id = [[attributeDict objectForKey:@"id"] integerValue];
		
	}	
	
}


- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName {
	
	if([elementName isEqualToString:@"galleryinfo"])
		return;
	
		if([elementName isEqualToString:@"gallery"]) {		
			
		[appDelegate.delarrGalleryList addObject:agallery];
			
		[agallery release];
		agallery = nil;
	
	}
	else{
		
		@try {
			[agallery setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] forKey:elementName];
			[currentElementValue release];
			currentElementValue = nil;
		}
		@catch (NSException * e) {
			
		}
	}

}

@end
