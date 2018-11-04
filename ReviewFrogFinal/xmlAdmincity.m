//
//  xmlAdmincity.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 01/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "xmlAdmincity.h"


@implementation xmlAdmincity


- (xmlAdmincity *) initxmlAdmincity
{	[super init];

	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication] delegate];
	//appDelegate.AdminCity=[[NSMutableArray alloc]init];
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
	if([currentElement isEqualToString:@"cityName"]){
		[appDelegate.ErrorCode addObject:string];
		if([string isEqual:@"null"]||[string isEqual:@""])
		{
			
			NSLog(@"userid== %@",string);
			appDelegate.AdminCity=@"";
			
		}
		else
		{
			appDelegate.AdminCity=string;
			NSLog(@"ADmincity-------%@",string);
			
			NSLog(@"usertid==%@",appDelegate.AdminCity);
			
		}
	}
		
}


@end
