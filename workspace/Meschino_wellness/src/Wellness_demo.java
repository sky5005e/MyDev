import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.junit.*;
import org.openqa.selenium.*;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.Select;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.Test;

public class Wellness_demo {


	private static final String CONTAINER_SELECTOR = "white-box-cms-wrap";
String driverPath = "C:\\geaco\\";
public WebDriver driver;


@Before
public void setUp()
{
	System.out.println("launching firefox browser"); 
	System.setProperty("webdriver.gecko.driver", driverPath+"geckodriver.exe");
	driver = new FirefoxDriver();
	driver.manage().window().maximize();
	driver.manage().deleteAllCookies();
	driver.navigate().to("http://demo-meschinowellness.azurewebsites.net/Account/Login");
	driver.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
	  	 
}

@Test(priority = 1)
public void login() throws Exception {
	

			// Enter Username
			driver.findElement(By.id("UserName")).click();
			Thread.sleep(1000);
			driver.findElement(By.id("UserName")).sendKeys("M1HRAtest@gmail.com");
			// Enter password
			driver.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).click();
			driver.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).sendKeys("Test123@");
			// Click on login button
			driver.findElement(By.xpath("/html/body/div[3]/form/div[3]/div/div[2]/button")).click();
			driver.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
			try
			{
				WebElement popupElement = driver.findElement(By.id("chklistModelVideoPopup"));
				if (popupElement!=null) {
					System.out.println("popup displayed");
					// Click on close button
					driver.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div[2]/div/a[2]")).click();
				}			
			} catch (Exception  e) {
				System.out.println("Exception while closing popup " + e.getMessage());
			}
			Thread.sleep(5000);
			// Verfiy Dashboard
			if (driver.findElement(By.id("dashboard")) != null) {
				System.out.println("Dashboard Title  is verified");
			} else {
				System.out.println("Dashboard Title  is not  Present");
			}
			Thread.sleep(5000);
			System.out.println("User login successful");
		//}

		//@Test(priority = 2)
		//public void MyHealthRiskConsiderations() throws Exception {
			// Click on My Wellness Report tab
			driver.findElement(By.id("riskreportNew")).click();
			Thread.sleep(5000);
			// Click on what is this link
			/*
			WebElement Linkcontainer = driver.findElement(By.className("bmi-list"));
			WebElement whatisthislink = Linkcontainer.findElement(By.id("btnBioAgeScorePopup"));
			whatisthislink.click();
			// Click on close button
			driver.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div/div/div/a")).click();
			Thread.sleep(5000);
			// Click on MHR what is this link
			WebElement MhrScorePopup = Linkcontainer.findElement(By.id("btnMHRScorePopup"));
			MhrScorePopup.click();
			Thread.sleep(3000);
			// Click on MHR close button
			driver.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div[2]/div/a")).click();
			Thread.sleep(5000);
			*/
			ArrayList<WebElement> list = getListOfLinks("tracking-tools", 0);
			Assert.assertNotEquals(0, list.size());
			for (int i = 0, l = list.size(); i < l; i++) {
				validateLinkAndRelatedPage("tracking-tools", 0, i, "wbc-heading", "h3", "button");
			}
			System.out.println("MyHealthRiskConsiderations test completed.");
			Thread.sleep(10000);
		//}

		//@Test(priority = 3)
		//public void MyHealthIssues() throws Exception {
			System.out.println("MyHealthIssues test started.");
			// d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
			// Click on My Wellness Report tab
			// d.findElement(By.id("riskreportNew")).click();
			ArrayList<WebElement> list1 = getListOfLinks("tracking-tools", 1);
			Assert.assertNotEquals(0, list1.size());
			for (int i = 0, l = list1.size(); i < l; i++) {
				validateLinkAndRelatedPage("tracking-tools", 1, i, "active-goal-heading", "h2", "a");
			}
		}

		private ArrayList<WebElement> getListOfLinks(String sectionName, int index) {
			ArrayList<WebElement> sections = (ArrayList<WebElement>) driver.findElements(By.className(sectionName));
			if (sections.size() > 0) {
				WebElement section = sections.get(index);
				// Get the track-list
				WebElement listContainer = (WebElement) section.findElements(By.className("track-list")).get(0);
				ArrayList<WebElement> list = (ArrayList<WebElement>) listContainer.findElements(By.tagName("li"));
				return list;
			}
			return null;
		}

		/*
		 * Map Dietary Assessment_Links - List<String> Dietary Assessment_Italics -
		 * List<String> Dietary Assessment_Strong - List<String>
		 */

		public static Map<String, List<String>> EXPECTED_STRINGS = new HashMap<String, List<String>>();
		{
			/**
			 * Map Format: Key = LinkText_<format> - Eg: Dietary Assessment_Strong,
			 * Dietary Assessment_Italics Value = List of expected strings
			 */
			// Bold strings Dietary Assessment
			EXPECTED_STRINGS.put("Dietary Assessment_Strong",
					Arrays.asList("Introduction to Your Dietary Assessment", 
							"Based upon your responses", 
							"38.00", 
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

			// Links Dietary Assessment
			EXPECTED_STRINGS.put("Dietary Assessment_Links",
					Arrays.asList("http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/144"));

			// Bold strings Body Composition
			EXPECTED_STRINGS.put("Body Composition_Strong",
					Arrays.asList("Your Body Mass Index is:��62.69", 
							"This places you in the Grade 3 overweight category",
							"Your present waist circumference is", 
							"38."));
			// Links Body Composition_
			EXPECTED_STRINGS.put("Body Composition_Links",
					Arrays.asList("https://meschinowellness.blob.core.windows.net/downloads/Intensive Weight Loss Plan_2.pdf",
							"http://demo-meschinowellness.azurewebsites.net/ResearchReviews/ContentInfo/Part_1_Clinical_Weight_Loss_Program",
							"http://demo-meschinowellness.azurewebsites.net/ResearchReviews/ContentInfo/Part_2_Clinical_Weight_Loss_Program",
							"http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/ViewWeightTrackerDetail?healthActivityId=8",
							"http://demo-meschinowellness.azurewebsites.net/ResearchReviews/ContentInfo/Part_3_Clinical_Weight_Loss_Program"));

			// Bold strings Physical Activity
			EXPECTED_STRINGS.put("Physical Activity_Strong",
					Arrays.asList("Endurance / Aerobic Activity",
							"You indicated that you perform endurance/aerobic exercise for 50 minutes or less per week, on average.",
							"Based�upon�your�present�age,�your�aerobic�training�zone�is�between�89�and�126�heart�beats per�minute",
							"You indicated that you regularly perform resistance or strength training once a week.",
							"In�your�particular�case,�your�protein�requirement�would�be�approximately�150�grams�per�day.",
							"you indicated that you regularly perform flexibility or stretching exercises once a week.",
							"Flexibility / Stretching Exercise"));

			// Links Physical Activity
			EXPECTED_STRINGS.put("Physical Activity_Links",
					Arrays.asList());

			// Bold strings Basic Supplement Considerations
			EXPECTED_STRINGS.put("Basic Supplement Considerations_Strong",
					Arrays.asList());

			// Links Basic Supplement Considerations
			EXPECTED_STRINGS.put("Basic Supplement Considerations_Links",
					Arrays.asList("http://www.nature.com/nrc/journal/v4/n3/full/nrc1298.html",
							"http://www.pnas.org/content/110/23/9523.short",
							"http://journals.plos.org/plosone/article?id=10.1371/journal.pone.0012244",
							"http://www.sciencedaily.com/releases/2010/09/100912213050.htm"));

			// Bold strings Nutrient Deficiencies and Depletion of Nutrients
			EXPECTED_STRINGS.put("Nutrient Deficiencies and Depletion of Nutrients_Strong", 
					Arrays.asList("Nutrient Deficiencies and Depletion of Nutrients",
							"Consuming more than 3 alcoholic beverages per week, on average, is associated with depletion of the following nutrients:",
							"Your diet appears to lack sufficient intake of fruits and vegetables. Insufficient intake of fruits and vegetables (less than 5 servings per day, combined).",
							"Flaky seborrheic conditions on the face can be caused or aggravated by deficiencies in Vitamins B2, B6.",
							"Soft nails or nails that chip, crack or peel easily, and/or are they brittle or contain ridges (not smooth) can result from deficiencies in calcium, and possibly other minerals.",
							"White spots under your fingernails can be a sign of zinc deficiency.",
							"Red spots under the skin are often an indicator of vitamin C deficiency.",
							"Drinking more than two cups of caffeinated beverages (e.g. coffee, tea, etc) per day tends to increase the frequency and volume of urination with",
							"Smoking",
							"Damage to the Skin from Sunlight and Tanning Beds results in the production of free radicals and depletion of antioxidants, such as vitamin C, vitamin E, selenium, betacarotene, and lycopene, which protect the skin from ultra-violet light damage.",
							"Cracks at the outer margins (corners) of the lips can be caused or aggravated by a Vitamin B2 or vitamin B6 deficiency.",
							"A sore or burning tongue can be caused by deficiency in Vitamins B2, B3, B6, and/or B12.",
							"A reduced ability to taste food is can be caused by a marginal zinc deficiency.",
							"Gums that bleed easily is often a sign of Vitamin C deficiency.",
							"From a nutritional standpoint skin that bruises easily can be caused by a deficiency",
							"Slow healing capability can be an indication of sub-optimal nutrient status of vitamin C and other vitamins and minerals.",
							"Feeling chronically fatigued in the absence of disease is often associated with multiple vitamin and mineral deficiency.",
							"Irregular eating patterns often results in inadequate intake of many essential vitamins and minerals",
							"Your regular use of laxatives is associated with depletion of the following nutrients:",
							"Your regular use of pain-killers and/or anti-inflammatory drugs is associated with depletion",
							"Your regular use of barbiturate drugs is associated with depletion of the following nutrients:",
							"Your regular use of antidepressant drugs is associated with depletion of the following nutrients:",
							"Your regular use of amphetamines is associated with depletion",
							"Your regular use of anticonvulsant phenytoin drugs is associated with depletion of the following nutrients:",
							"Your regular use of antibiotics is associated with depletion of the following nutrients:",
							"Your regular use of statin drugs for high cholesterol is associated with depletion of Coenzyme Q10.",
							"Your regular use of ACE-Inhibitor drugs is associated with depletion of the following nutrients: Zinc.",
							"Your regular use of an anti-gout drug is associated with depletion of the following nutrients: Vitamin A Vitamin D Vitamin B12 Folic Acid Iron",
							"Stress"));

			// Links Nutrient Deficiencies and Depletion of Nutrients
			EXPECTED_STRINGS.put("Nutrient Deficiencies and Depletion of Nutrients_Links",
					Arrays.asList("http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/188",
					"http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/StressTracker?healthActivityId=13",
					"https://meschinowellness.blob.core.windows.net/downloads/02Nutr Defic and Depl_REFERENCES.pdf"));

			// Bold strings Drug-Nutrient Interactions and Precautions
			EXPECTED_STRINGS.put("Drug-Nutrient Interactions and Precautions_Strong", 
					Arrays.asList("Drug-Nutrient Interactions and Precautions",
							"Drug-Nutrient Interactions and Precautions",
							"Various sections of your Meschino Health and Wellness report"));

			// Italics Drug-Nutrient Interactions and Precautions
			EXPECTED_STRINGS.put("Drug-Nutrient Interactions and Precautions_Italics", 
					Arrays.asList(
							"\"Do you suffer from a hemolytic anemia due to glucose�6 phosphate dehydrogenase deficiency?\"",
							"\"Do you suffer from kidney failure or are on dialysis treatment?\"",
							"\"Do you have Wilson?s disease?\"",
							"\"Do you have hemochromatosis?\"",
							"\"Have you received an organ transplant of any kind?\"", 
							"\"Are you taking an immuno�suppressive drug (ie cyclosporin)?\"", 
							"\"Have you ever been diagnosed with Crohn?s Disease or Ulcerative Colitis?\"",
							"\"Are you an insulin-dependent diabetic?\"", 
							"\"Are you an insulin-dependent diabetic?\"",
							"\"Do you have only one functioning kidney (due to one kidney being removed or one kidney known to be non-functional)?\"", 
							"\"Are you a diabetic or have known blood sugar regulation problems?\""));

			// Bold strings Family Health History
			EXPECTED_STRINGS.put("Family Health History_Strong",
					Arrays.asList("You indicated that there is a family history of Alzheimer�s disease or dementia.",
							"You indicated that there is a family history of Parkinson�s disease.",
							"You indicated that a first degree relative sustained a heart attack before age 60.",
							"You indicated that there is a family history of colon cancer."));

			// Links Family Health History
			EXPECTED_STRINGS.put("Family Health History_Links",
					Arrays.asList("https://meschinowellness.blob.core.windows.net/downloads/27F%20FHH%20Breast-Colon.pdf"));

			// Bold strings Healthy Aging Supplement Considerations and Early
			// Detection
			EXPECTED_STRINGS.put("Healthy Aging Supplement Considerations and Early Detection_Strong",
					Arrays.asList("Because you are a man over 40 years of age you may wish to consider taking a daily supplement designed to support prostate health",
							"Because you are over 45 you should consider a supplement providing coenzyme Q10 and hawthorn for heart health.",
							"Because you are a man over the age of 49 you may wish to consider taking a daily supplement to support your immune and detoxification systems.",
							"Because you are a man over the age of 54 you may wish to consider taking a supplement to support brain and memory function.",
							"PSA Blood Test",
							"Colonoscopy"));

			// Bold strings Basic Blood Tests of Significance
			EXPECTED_STRINGS.put("Basic Blood Tests of Significance_Strong",
					Arrays.asList());

			// Links Basic Blood Tests of Significance
			EXPECTED_STRINGS.put("Basic Blood Tests of Significance_Links",
					Arrays.asList("https://meschinowellness.blob.core.windows.net/downloads/basic_blood_tests.pdf",
							"https://meschinowellness.blob.core.windows.net/downloads/blood_interp.pdf"));

		}

		private void validateLinkAndRelatedPage(String sectionName, int sectionIndex, int elementIndex,
				String headingContainer, String headingSelector, String buttonSelector) {
			ArrayList<WebElement> list = getListOfLinks(sectionName, sectionIndex);
			System.out.println("Text = " + list.get(elementIndex).getText());
			String currentLinkText = list.get(elementIndex).getText(); // Eg:DietaryAssessment
			WebElement currentLIItem = list.get(elementIndex);
			// Find the anchor tag
			WebElement anchorTag = currentLIItem.findElement(By.tagName("a"));
			Assert.assertNotNull(anchorTag);
			// continue execution and click on links
			JavascriptExecutor je = (JavascriptExecutor) driver;
			je.executeScript("arguments[0].click();", anchorTag);

			// Goes to the next page, get the header in the next page
			WebElement headingContainerElement = driver.findElement(By.className(headingContainer));
			Assert.assertNotNull(headingContainerElement);

			// Get the heading text (h3)
			WebElement headingText = headingContainerElement.findElement(By.tagName(headingSelector));
			Assert.assertNotNull(headingText);
			System.out.println("Heading Text = " + headingText.getText() + ", Link Text = " + currentLinkText);
			Assert.assertEquals(headingText.getText(), currentLinkText);

			// Query map for strong strings

			List<String> expectedBoldStrings = EXPECTED_STRINGS.get(currentLinkText + "_Strong");

			if (expectedBoldStrings != null) {
				WebElement container = driver.findElement(By.className(CONTAINER_SELECTOR));
				List<WebElement> boldStringsInSection = (List<WebElement>) container.findElements(By.tagName("strong"));

				List<String> listOfBoldTextOnPage = new ArrayList<String>();
				for (WebElement element : boldStringsInSection) {
					listOfBoldTextOnPage.add(element.getText().trim());
				}
				System.out.println("START UI Bold Text List===>> ");
				for (String BoldString : listOfBoldTextOnPage) {
					System.out.println(BoldString);
				}
				System.out.println("END UI Bold Text List===>> ");
				for (String expectedBoldString : expectedBoldStrings) {

					if (listOfBoldTextOnPage.contains(expectedBoldString)) {
						System.out.println("Validated Bold Text===>> " + expectedBoldString);
					} else {
						System.out.println("Not Validated Bold Text===>>  " + expectedBoldString);
					}
				}
			}

			// Query map for Link Texts
			List<String> expectedLinkTexts = EXPECTED_STRINGS.get(currentLinkText + "_Links");

			if (expectedLinkTexts != null) {
				WebElement container1 = driver.findElement(By.className(CONTAINER_SELECTOR));
				List<WebElement> linkTextsInSection = (List<WebElement>) container1.findElements(By.tagName("a"));

				List<String> listOfLinkTextOnPage = new ArrayList<String>();
				for (WebElement element : linkTextsInSection) {
					listOfLinkTextOnPage.add(element.getAttribute("href"));
				}
				System.out.println("START UI LINK Text List===>> ");
				for (String links : listOfLinkTextOnPage) {
					System.out.println(links);
				}
				System.out.println("END UI LINK Text List===>> ");
				for (String expectedLinkText : expectedLinkTexts) {

					if (listOfLinkTextOnPage.contains(expectedLinkText)) {
						System.out.println("Validated Link Text===>> " + expectedLinkText);
					} else {
						System.out.println("Not Validated Link Text===>>  " + expectedLinkText);
					}
				}
			}
			// Query map for ItalicTexts
			List<String> expectedItalicTexts = EXPECTED_STRINGS.get(currentLinkText + "_Italics");
			if (expectedItalicTexts != null) {
				WebElement container2 = driver.findElement(By.className(CONTAINER_SELECTOR));
				List<WebElement> italicTextsInSection = (List<WebElement>) container2.findElements(By.tagName("em"));

				List<String> listOfItalicTextOnPage = new ArrayList<String>();
				for (WebElement element : italicTextsInSection) {
					listOfItalicTextOnPage.add(element.getText().trim());
				}
				System.out.println("START UI Italic Text List===>> ");
				for (String italics : listOfItalicTextOnPage) {
					System.out.println(italics);
				}
				System.out.println("END UI Italic Text List===>> ");
				
				for (String expectedItalicText : expectedItalicTexts) {

					if (listOfItalicTextOnPage.contains(expectedItalicText)) {
						System.out.println("Validated Italic Text===>> " + expectedItalicText);
					} else {
						System.out.println("Not Validated Italic Text===>>  " + expectedItalicText);
					}
				}
			}
			
			System.out.println("Validated " + currentLinkText + ", Going back for the rest!!");

			// Go back
			WebElement backButton = headingContainerElement.findElement(By.tagName(buttonSelector));
			backButton.click();
		}
}
	
