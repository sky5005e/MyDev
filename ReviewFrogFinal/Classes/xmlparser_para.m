//
//  xmlparser_para.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 03/01/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "xmlparser_para.h"
#import "Review_FrogAppDelegate.h"


@implementation xmlparser_para

- (xmlparser_para *) initxmlparser_para {
	
	[super init];
	
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
			
			//UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Review Frog" message:@"Data Save" delegate:self 
//												  cancelButtonTitle:@"OK" 
//												  otherButtonTitles:nil];
//			[alert show];
//			[alert release];	
			
		}
	}
		
}

@end

