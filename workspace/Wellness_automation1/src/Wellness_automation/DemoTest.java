package Wellness_automation;

import static org.junit.Assert.*;

import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.junit.Test;

public class DemoTest {

	@Test
	public void test() {
		
		List<String> listOfBoldTextOnPage =  Arrays.asList(
				"Introduction to Your Dietary Assessment",
				"The Meschino Optimal Living Program",
				"Your Personal Feedback Report",
				"Based upon your responses",
				"38.00",
				"Using cream in your coffee or tea can significantly add more saturated fat and cholesterol to your daily diet. It is far better to use 1% or non-fat milk for this purpose.",
				"Your frequent consumption of whole eggs is of some concern",
				"Your frequent intake of fried foods is of some concern",
				"Your frequent intake of high fat pastries and related treats is a cause for some concern",
				"Your frequent consumption of fried snack foods and/or chocolate bars (or other high fat chocolate products) is a cause for some concern",
				"Your frequent intake of sugary candies, and/or soda pops is a cause for some concern.",
				"Your diet appears to lack proper attention to the intake of vegetables.",
				"Your diet appears to lack proper attention to fruit intake.",
				"Your frequent consumption of alcohol may be a cause for some concern",
				"You indicated that on a weekly basis you often have 4 alcoholic drinks or more on at least one day of the week.",
				"You indicated that you eat high fat meat products 3 or more times per week and consume very little, if any, high fat dairy products,",
				"Your practice of using butter on baked potatoes or included in the cooking of vegetables, eggs or sauces etc. is a cause for concern.");
				
		List<String> expectedBoldStrings = EXPECTED_STRINGS.get("Dietary Assessment_Strong");

		for (String expectedBoldString : expectedBoldStrings) {

			if (listOfBoldTextOnPage.contains(expectedBoldString)) {
				System.out.println("Validated Bold Text===>> " + expectedBoldString);
			} else {
				System.out.println("Not Validated Bold Text===>>  " + expectedBoldString);
			}
		}

	}
	public static Map<String, List<String>> EXPECTED_STRINGS = new HashMap<String, List<String>>();
	{
		/**
		 * Map Format: Key = LinkText_<format> - Eg: Dietary Assessment_Strong,
		 * Dietary Assessment_Italics Value = List of expected strings
		 */
		// Bold strings Dietary Assessment
		EXPECTED_STRINGS.put("Dietary Assessment_Strong",
				Arrays.asList(//"Introduction to Your Dietary Assessment", 
						//"Based upon your responses", 
						//"38.00", 
						"Using cream in your coffee or tea can significantly add more saturated fat and cholesterol to your daily diet.",
						"Your frequent consumption of whole eggs is of some concern",
						"Your frequent intake of fried foods is of some concern",
						"Your frequent intake of high fat pastries and related treats is a cause for some concern.",
						"Your frequent consumption of fried snack foods and/or chocolate bars (or other high fat chocolate products) is a cause for some concern.",
						"Your frequent intake of sugary candies, and/or soda pops is a cause for some concern.",
						"Your diet appears to lack proper attention to the intake of vegetables.",
						"Your diet appears to lack proper attention to fruit intake.",
						"Your frequent consumption of alcohol may be a cause for some concern.",
						"You indicated that on a weekly basis you often have 4 alcoholic drinks or more on at least one day of the week.", 
						"You indicated that you eat high fat meat products 3 or more times per week and consume very little, if any, high fat dairy products, on average.",
						"Your practice of using butter on baked potatoes or included in the cooking of vegetables, eggs or sauces etc. is a cause for concern."));


	}

}
