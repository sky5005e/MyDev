import java.util.*;

import org.junit.Test;

public class Demo_class {

	@Test//(priority = 1)
	public void login() throws Exception {
		
	List<String> listOfBoldTextOnPage =  Arrays.asList(
			"Using cream in your coffee or tea can significantly add more saturated fat and cholesterol to your daily diet. It is far better to use 1% or non-fat milk for this purpose.",
			"Your frequent consumption of whole eggs is of some concern",
			"Your practice of using butter on baked potatoes or included in the cooking of vegetables, eggs or sauces etc. is a cause for concern.");
			
	List<String> expectedBoldStrings = EXPECTED_STRINGS.get("Dietary Assessment_Strong");

	for (String expectedBoldString : expectedBoldStrings) {

		if (listOfBoldTextOnPage.stream().filter(x -> x.contains(expectedBoldString)).count() > 0) {
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
												"Your practice of using butter on baked potatoes or included in the cooking of vegetables, eggs or sauces etc. is a cause for concern."));

	}
}
