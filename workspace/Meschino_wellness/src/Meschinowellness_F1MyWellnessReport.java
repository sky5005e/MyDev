

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.By.ByClassName;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.interactions.Actions;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeTest;
import org.testng.annotations.Test;


public class Meschinowellness_F1MyWellnessReport {
	WebDriver d;

	@BeforeTest
	public void Setup() throws Exception {
		System.setProperty("webdriver.firefox.marionette","C:\\Selenium\\geckodriver-v0.11.1-win64\\geckodriver.exe");
		// Launch browser
		d = new FirefoxDriver();
		d.manage().window().maximize();
		d.manage().deleteAllCookies();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		d.get("http://demo-meschinowellness.azurewebsites.net/Account/Login");
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
	}
	@Test(priority = 1)
	public void login() throws Exception {
		// Enter Username
		d.findElement(By.id("UserName")).click();
		Thread.sleep(1000);
		d.findElement(By.id("UserName")).sendKeys("F1HRAtest@gmail.com");
		// Enter password
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).click();
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).sendKeys("Test123@");
		// Click on login button
		d.findElement(By.xpath("/html/body/div[3]/form/div[3]/div/div[2]/button")).click();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		// Click on close button
		d.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div[2]/div/a[2]")).click();
		Thread.sleep(5000);
		// Verfiy Dashboard
		if (d.findElement(By.id("dashboard")) != null) {
			System.out.println("Dashboard Title  is verified");
		} else {
			System.out.println("Dashboard Title  is not  Present");
		}
		Thread.sleep(5000);
		System.out.println("User login successful");
	}
	@Test(priority = 2)
	public void MyHealthRiskConsiderations() throws Exception {
		// Click on My Wellness Report tab
		d.findElement(By.id("riskreportNew")).click();
		Thread.sleep(5000);
		// Click on what is this link
		WebElement Linkcontainer = d.findElement(By.className("bmi-list"));
		WebElement whatisthislink =	Linkcontainer.findElement(By.id("btnBioAgeScorePopup"));
		whatisthislink.click();
		// Click on close button
       d.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div/div/div/a")).click();
       Thread.sleep(5000);
      // Click on MHR what is this link
       WebElement MhrScorePopup =Linkcontainer.findElement(By.id("btnMHRScorePopup"));
       MhrScorePopup.click();
       Thread.sleep(3000);
    // Click on MHR close button
       d.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div[2]/div/a")).click();
       Thread.sleep(5000);
		ArrayList<WebElement> list = getListOfLinks("tracking-tools", 0);
		Assert.assertNotEquals(0, list.size());
		for (int i = 0, l = list.size(); i < l; i++) {
			validateLinkAndRelatedPage("tracking-tools", 0, i, "wbc-heading", "h3", "button", true);
		}
		System.out.println("MyHealthRiskConsiderations test completed. ");
		  Thread.sleep(10000);
	}
	
	@Test(priority = 3)
	public void MyHealthIssues() throws Exception {
		System.out.println("MyHealthIssues test started.");
		//d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		// Click on My Wellness Report tab
		//d.findElement(By.id("riskreportNew")).click();
		ArrayList<WebElement> list = getListOfLinks("tracking-tools", 1);
		Assert.assertNotEquals(0, list.size());
		for (int i = 0, l = list.size(); i < l; i++) {
			validateLinkAndRelatedPage("tracking-tools", 1, i, "active-goal-heading", "h2", "a", false);
		}
	}
	private ArrayList<WebElement> getListOfLinks(String sectionName, int index) {
		ArrayList<WebElement> sections = (ArrayList<WebElement>) d.findElements(By.className(sectionName));
		if (sections.size() > 0) {
			WebElement section = sections.get(index);
			// Get the track-list
			WebElement listContainer = (WebElement) section.findElements(By.className("track-list")).get(0);
			ArrayList<WebElement> list = (ArrayList<WebElement>) listContainer.findElements(By.tagName("li"));
			return list;
		}
		return null;
	}
	private void validateLinkAndRelatedPage(String sectionName, int sectionIndex, int elementIndex,
			String headingContainer, String headingSelector, String buttonSelector, boolean checktext) {
		ArrayList<WebElement> list = getListOfLinks(sectionName, sectionIndex);
		System.out.println("Text = " + list.get(elementIndex).getText());
		String currentLinkText = list.get(elementIndex).getText();
		WebElement currentLIItem = list.get(elementIndex);
		// Find the anchor tag
		WebElement anchorTag = currentLIItem.findElement(By.tagName("a"));
		Assert.assertNotNull(anchorTag);
		// continue execution and click on links
		JavascriptExecutor je = (JavascriptExecutor) d;
		je.executeScript("arguments[0].click();", anchorTag);
		// Goes to the next page, get the header in the next page
		WebElement headingContainerElement = d.findElement(By.className(headingContainer));
		Assert.assertNotNull(headingContainerElement);
		// Get the heading text (h3)
		WebElement headingText = headingContainerElement.findElement(By.tagName(headingSelector));
		Assert.assertNotNull(headingText);
		System.out.println("Heading Text = " + headingText.getText() + ", Link Text = " + currentLinkText);
		Assert.assertEquals(headingText.getText(), currentLinkText);
		
		// Bold Text verification
		if (checktext) {
			List<String> Verifytextlist = new ArrayList<>(Arrays.asList("Introduction to Your Dietary Assessment",
					"The Meschino Optimal Living Program",
					"Using cream in your coffee or tea can significantly add more saturated fat and cholesterol to your daily diet. It is far better to use 1% or non-fat milk for this purpose.",
					"Based upon your responses", "38.00", "Your frequent consumption of whole eggs is of some concern",
					"Your frequent intake of fried foods is of some concern",
					"Your frequent intake of high fat pastries and related treats is a cause for some concern",
					"Your frequent consumption of fried snack foods and/or chocolate bars (or other high fat chocolate products) is a cause for some concern",
					"Your frequent intake of sugary candies, and/or soda pops is a cause for some concern.",
					"Your diet appears to lack proper attention to the intake of vegetables.",
					"Your diet appears to lack proper attention to fruit intake.",
					"Your frequent consumption of alcohol may be a cause for some concern",
					"You indicated that on a weekly basis you often have 4 alcoholic drinks or more on at least one day of the week.",
					"You indicated that you eat high fat meat products 3 or more times per week and consume very little, if any, high fat dairy products,",
					"Your practice of using butter on baked potatoes or included in the cooking of vegetables, eggs or sauces etc. is a cause for concern.",

					// Body Composition text verification
					"Body Composition",
					"Your Body Mass Index is: 57.47", "This places you in the Grade 3 overweight category,",
					"Your present waist circumference is", "35",

					// Physical Activity
					"Physical Activity",
					"Endurance / Aerobic Activity",
					"You indicated that you perform endurance/aerobic exercise for 50 minutes or less per week, on average.",
					"Resistance / Strength Training",
					"You indicated that you regularly perform resistance or strength training once a week.",
					"Flexibility / Stretching Exercise",

					// Basic Supplement Considerations
					"Basic Supplement Considerations",
				
					// Nutrient Deficiencies and Depletion of Nutrients
					"Consuming more than 3 alcoholic beverages per week, on average, is associated with depletion of the following nutrients",
					"Your diet appears to lack sufficient intake of fruits and vegetables.",
					"Flaky Seborrheic Condition", "Soft Nails or Nails that Chip, Crack or Peel easily,",
					"White Spots under your Fingernails",
					"Red spots under the Skin are often an Indicator of Vitamin C Deficienc",
					"Drinking More Than Two Cups of Caffeinated Beverages", "Per Day", "Smoking",
					"Your regular use of laxatives is associated with depletion of the following nutrients",
					"Your regular use of pain-killers and/or anti-inflammatory drugs is associated with depletion",
					"Your regular use of barbiturate drugs is associated with depletion of the following nutrients:",
					"Your regular use of antidepressant drugs is associated with depletion of the following nutrients",
					"Your regular use of amphetamines is associated with depletion",
					"Your regular use of anticonvulsant phenytoin drugs is associated with depletion of the following nutrients:",
					"Your regular use of antibiotics is associated with depletion of the following nutrients",
					"Your regular use of oral contraceptive drugs is associated with depletion of the following nutrie",
					"Your regular use of estrogen replacement therapy is associated with depletion of the following nutrients:",
					"Your regular use of statin drugs for high cholesterol is associated with depletion of Coenzyme Q10.",
					"Your regular use of ACE-Inhibitor drugs",
					"Your regular use of an anti-gout drug is associated with depletion", "Stress",

					// Drug-Nutrient Interactions and Precautions
					"Drug-Nutrient Interactions and Precautions",
					"Various sections of your Meschino Health and Wellness report",
					"these warning statements will be listed immediately below this paragraph. If no warning appears then you may consider proceeding with the supplement considerations that appear in various sections of your feedback report. However, you should always consult a health care professional before making any change to your diet, exercise or supplementation program.",

					// Family Health History
					"You indicated that there is a family history of Alzheimer’s disease or dementia.",
					"Family History of Parkinson's Disease",
					"You indicated that a first degree relative sustained a heart attack before age 60.",
					"You indicated that there is a family history of colon cancer.",

					// Healthy Aging Supplement Considerations and Early Detection
					"Healthy Aging Supplement Considerations and Early Detection Screening",
					"Women age 55 and older",
					"Mammogram:",
					"Glucosamine Supplement With Natural Anti-Inflammatory Agents",
					"Coenzyme Q10 and Hawthorn", 
					"Immune and Detoxification Support",
					"Female Support Nutrients",
					"Brain and Memory Support Nutrients",
					"Mammogram","Colonoscopy:",
					
			
			//Basic Blood Tests of Significance
			"Basic Blood Tests of Significance"));

			// Link Text verification
			List<String> Verifylinkslist = new ArrayList<>
			// Dietary Assessment
			(Arrays.asList("http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/144",
					"http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/144",

					// Body Composition
					"https://meschinowellness.blob.core.windows.net/downloads/Intensive%20Weight%20Loss%20Plan.pdf",
					"http://demo-meschinowellness.azurewebsites.net/ResearchReviews/ContentInfo/Part_1_Clinical_Weight_Loss_Program",
					"http://demo-meschinowellness.azurewebsites.net/ResearchReviews/ContentInfo/Part_2_Clinical_Weight_Loss_Program",
					"http://demo-meschinowellness.azurewebsites.net/ResearchReviews/ContentInfo/Part_3_Clinical_Weight_Loss_Program",
					"http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/ViewWeightTrackerDetail?healthActivityId=8",

					// Physical Activity
					
					"http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/StepTracker?healthActivityId=10",
					"https://meschinowellness.blob.core.windows.net/downloads/protein_food_chart.pdf",
					"https://meschinowellness.blob.core.windows.net/downloads/StrengthTraining2.pdf",
					
					//Basic Supplement Considerations
					"http://www.nature.com/nrc/journal/v4/n3/full/nrc1298.html",
					"http://www.pnas.org/content/110/23/9523.short",
					"http://journals.plos.org/plosone/article?id=10.1371/journal.pone.0012244",
					"http://www.sciencedaily.com/releases/2010/09/100912213050.htm",
					

					// Drug-Nutrient Interactions and Precautions 
					"http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/187",
					"http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/Pledge?healthActivityId=46",
					"http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/188",
					"http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/StressTracker?healthActivityId=13",
					"https://meschinowellness.blob.core.windows.net/downloads/02Nutr%20Defic%20and%20Depl_REFERENCES.pdf",

					//Family Health History
					"http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/170",
					"https://meschinowellness.blob.core.windows.net/downloads/13U%20FHH%20Colon.pdf",

					// Basic Blood Tests of Significance
					"https://meschinowellness.blob.core.windows.net/downloads/basic_blood_tests.pdf",
					"https://meschinowellness.blob.core.windows.net/downloads/blood_interp.pdf"));

			// italic Text verification
			List<String> Verifyitaliclist = new ArrayList<>
			// Drug-Nutrient Interactions and Precautions
			(Arrays.asList("\"Have you ever had an allergic reaction to a vitamin supplement in the past?\"",
					"\"Are you pregnant or breast–feeding?\"",
					"\"Do you suffer from a hemolytic anemia due to glucose–6 phosphate dehydrogenase deficiency?\"",
					"\"Do you suffer from kidney failure or are on dialysis treatment?\"",
					"\"Do you have Wilson?s disease?\"", "\"Do you have hemochromatosis?\"",
					"\"Have you received an organ transplant of any kind?\"", 
					"\"Have you ever been diagnosed with breast or ovarian cancer?\"",
					"\"Are you taking an immuno–suppressive drug (ie cyclosporin)?\"",
					"\"Are you taking an immuno–suppressive drug (ie cyclosporin)?\"",
					"\"Have you ever been diagnosed with Crohn?s Disease or Ulcerative Colitis?\"",
					"\"Are you an insulin–dependent diabetic?\"", "\"Are you an insulin–dependent diabetic?\"",
					"\"Do you have only one functioning kidney (due to one kidney being removed or one kidney known to be non–functional)?\"",
					"\"Are you a diabetic or have known blood sugar regulation problems?\""));

			// Bold Text verification
			WebElement Contentofpage = d.findElement(By.className("wbc-content"));
			List<WebElement> Boldtexts = (List<WebElement>) Contentofpage.findElements(By.tagName("strong"));
			for (WebElement Boldtext : Boldtexts) {
				String text = Boldtext.getText();
				if (Verifytextlist != null && Verifytextlist.contains(text)) {
					System.out.println("Validated Bold Text===>> " + Boldtext.getText());
				} else {
					System.out.println("Not Validated Bold Text===>>  " + Boldtext.getText());
				}
			}
			// Link Text verification
			List<WebElement> Linkstexts = (List<WebElement>) Contentofpage.findElements(By.tagName("a"));
			for (WebElement Linkstext : Linkstexts) {
				String text = Linkstext.getAttribute("href");
				if (Verifylinkslist != null && Verifylinkslist.contains(text)) {
					System.out.println("Validated Link Text===>>" + text);
				} else {
					System.out.println("Not Validated Link Text===>>" + text);
				}
			}
			// italic Text verification
			List<WebElement> italictexts = (List<WebElement>) Contentofpage.findElements(By.tagName("em"));
			for (WebElement italictext : italictexts) {
				String text = italictext.getText().trim();
				if (Verifyitaliclist != null && Verifyitaliclist.contains(text)) {
					System.out.println("Validated Italic Text===>> " + text);
				} else {
					System.out.println("Not Validated Italic Text===>>  " + text);
				}
			}
			System.out.println("Validated " + currentLinkText + ", Going back for the rest!!");
		}
		   // Go back
		   WebElement backButton = headingContainerElement.findElement(By.tagName(buttonSelector));
		   backButton.click();
	    }

	@AfterMethod
	public void logout(ITestResult result) throws Exception {
		// Verify if test fails
		if (ITestResult.FAILURE == result.getStatus()) {
			try {
				// Create reference of TakesScreenshot
				TakesScreenshot ts = (TakesScreenshot) d;
				// capture screenshot
				File source = ts.getScreenshotAs(OutputType.FILE);
				// Copy files to specific location to store screenshot in our
				// project home directory
				// result.getName() will return name of test case so that
				// screenshot name will be same
				File file = new File("./Screenshots/" + result.getName() + ".png");
				FileUtils.copyFile(source, file);
				Throwable errorDetail = result.getThrowable();
				System.out.println("Screenshot taken");
			} catch (Exception e) {
				System.out.println("Exception while taking screenshot " + e.getMessage());
			}
		}

		// close application
		// d.quit();
	}

}
