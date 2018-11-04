//
//  xmlPost.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 13/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "xmlPost.h"
#import "Review_FrogAppDelegate.h"


@implementation xmlPost


- (xmlPost *) initxmlPost{

		[super init];
		appDelegate.isposted=NO;
		appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
		appDelegate.ErrorCode =[[NSMutableArray alloc]init];
		return self;
}
	
-(void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName attributes:(NSDictionary *)attributeDict {
		//	NSLog(@"did start element");
		
		if (elementName) {
			currentElement = elementName;
		}
		NSLog(@"Current element %@",currentElement);
	}

-(void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName{
		NSLog(@"did end element");
		
		/*if (elementName) {
		 currentElement = elementName;
		 }*/
}
-(void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string
{
		
		NSLog(@"found character");
		if([currentElement isEqualToString:@"return"]){
			[appDelegate.ErrorCode addObject:string];
			if([string isEqual:@"1"])
			{
				appDelegate.isposted=YES;
				
			}
		}
		
}
@end
