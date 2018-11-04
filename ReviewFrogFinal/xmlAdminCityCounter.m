//
//  xmlAdminCityCounter.m
//  Review Frog
//
//  Created by Agile Infoways Pvt.ltd. on 01/02/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "xmlAdminCityCounter.h"


@implementation xmlAdminCityCounter

- (xmlAdminCityCounter *) initxmlAdminCityCounter
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
		//[appDelegate.ErrorCode addObject:string];
//		if([string isEqual:@"0"])
//		{
//			
//			NSLog(@"userid== %@",string);
//			
//		}
//		else
//		{
			appDelegate.admincitycounter=string;
			NSLog(@"ADmincity-------%@",string);
			
			NSLog(@"usertid==%@",appDelegate.admincitycounter);
			
		//}
	}
	
}


@end
