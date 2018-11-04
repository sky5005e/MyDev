//
//  XMLCategory.m
//  Review Frog
//
//  Created by agilepc-32 on 11/8/11.
//  Copyright 2011 __MyCompanyName__. All rights reserved.
//

#import "XMLCategory.h"


@implementation XMLCategory

@synthesize questionID,answerID,permission;

-(XMLCategory *) initXMLCategory
{
	[super init];
	appDelegate = (Review_FrogAppDelegate *)[[UIApplication sharedApplication]delegate];
	return self;
}

- (void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string
{
//	if (!currentElementValue) 
//		currentElementValue = [[NSMutableString alloc] initWithString:string];
//	else 
//		[currentElementValue appendString:string];
    
    currentElementValue=[NSString stringWithString:[string stringByTrimmingCharactersInSet:[NSCharacterSet newlineCharacterSet]]];
}

- (void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qualifiedName
	attributes:(NSDictionary *)attributeDict
{
	if ([elementName isEqualToString:@"categories"]) {
		appDelegate.ArrCategoryList = [[NSMutableArray alloc]init];
        
        appDelegate.questions=[NSMutableArray new];
	}
    
	else if([elementName isEqualToString:@"category"]){
        
		objCategory = [[Category alloc]init];
        
		objCategory.id = [[attributeDict objectForKey:@"id"]integerValue];
	}	
    
    else if([elementName isEqualToString:@"Question"]){
        
        self.questionID=[attributeDict objectForKey:@"id"];
        
        answers=[NSMutableDictionary new];
        
    }
    
    else if([elementName isEqualToString:@"answer"]){
        
        self.answerID=[attributeDict objectForKey:@"id"];
    }
}

- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName
  namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName
{
	if ([elementName isEqualToString:@"categories"])
		return;
	
	else if ([elementName isEqualToString:@"category"]) {
		
//		[appDelegate.ArrCategoryList addObject:objCategory];
//		[objCategory release];
//         objCategory =nil;
	}
    
    else if ([elementName isEqualToString:@"sweepstake"]){
        
        appDelegate.sweepstake=[currentElementValue isEqualToString:@"0"]?NO:YES;
    }
    
    else if([elementName isEqualToString:@"Question"])
    {        
        [appDelegate.questions addObject:[NSDictionary dictionaryWithObjectsAndKeys:answers,q,self.questionID,@"id",self.permission,@"permission",q,@"question", nil]];
    }
    
    else if([elementName isEqualToString:@"text"])
    {
        q=[NSString new];
        
        q=currentElementValue;
    }
    
    else if([elementName isEqualToString:@"answer"])
    {
        [answers setValue:self.answerID forKey:currentElementValue];
    }
    
    else if([elementName isEqualToString:@"permission"]){
        
        self.permission=currentElementValue;
    }
        
	else if([elementName isEqualToString:@"categoryname"])
	{
		@try {
			
//			[objCategory setValue:[currentElementValue stringByTrimmingCharactersInSet:[NSCharacterSet whitespaceCharacterSet]] forKey:elementName];
//			[currentElementValue release];
//			currentElementValue = nil;
            
            [appDelegate.ArrCategoryList addObject:currentElementValue];
		}
		@catch (NSException * e) {
			
		}
	}
}
- (void)parser:(NSXMLParser *)parser foundCDATA:(NSData *)CDATABlock
{	
    currentElementValue = [[NSMutableString alloc] initWithData:CDATABlock encoding:NSUTF8StringEncoding];
}

@end
