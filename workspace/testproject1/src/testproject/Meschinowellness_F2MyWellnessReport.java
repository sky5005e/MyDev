package testproject;


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
import org.openqa.selenium.support.ui.Select;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
//import org.testng.annotations.Test;



public class Meschinowellness_F2MyWellnessReport {
	String driverPath = "C:\\geaco\\";
	public WebDriver d;
	private static final String CONTAINER_SELECTOR = "white-box-cms-wrap";
	//WebDriver driver;

	@Before
	public void setUp()
	{
		System.out.println("launching firefox browser"); 
		System.setProperty("webdriver.gecko.driver", driverPath+"geckodriver.exe");
		d = new FirefoxDriver();
		d.manage().window().maximize();
		d.manage().deleteAllCookies();
		d.navigate().to("http://demo-meschinowellness.azurewebsites.net/Account/Login");
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
	}
	
	@Test//(priority = 1)
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
		Thread.sleep(8000);
		System.out.println("User login successful");
	//}
	//@Test(priority = 2)
	//public void MyHealthRiskConsiderations() throws Exception {
		// Click on My Wellness Report tab
		d.findElement(By.id("riskreportNew")).click();
		Thread.sleep(8000);
		// Click on what is this link
		//WebElement Linkcontainer = d.findElement(By.className("bmi-list"));
		//WebElement whatisthislink =	Linkcontainer.findElement(By.id("btnBioAgeScorePopup"));
		//whatisthislink.click();
		// Click on close button
       //d.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div/div/div/a")).click();
       Thread.sleep(5000);
      // Click on MHR what is this link
       //WebElement MhrScorePopup =Linkcontainer.findElement(By.id("btnMHRScorePopup"));
       //MhrScorePopup.click();
       //Thread.sleep(3000);
    // Click on MHR close button
       //d.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div[2]/div/a")).click();
       //Thread.sleep(5000);
		
		ArrayList<WebElement> listt = getListOfLinks("tracking-tools", 0);
		Assert.assertNotEquals(0, listt.size());
		for (int i = 0, l = listt.size(); i < l; i++) {
			validateLinkAndRelatedPage("tracking-tools", 0, i, "wbc-heading", "h3", "button", true);
		}
		d.manage().timeouts().implicitlyWait(50, TimeUnit.SECONDS);
		
	//}
	//@Test(priority = 3)
	//public void MyHealthIssues() throws Exception {
		// Click on My Wellness Report tab
		
	    d.findElement(By.id("riskreportNew")).click();
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
			List<String> Verifytextlist = //new ArrayList<>(
					Arrays.asList("Introduction to Your Dietary Assessment",
					"Based upon your responses", 
					"13.00.", 
					"It is impressive that you have maintained a low intake of whole egg consumption.",
					"It is impressive that your diet is not high in fried foods.",
					"It is impressive that you have maintained a low intake of high fat pastries and related treats.",
					"It is impressive that you have maintained a low intake of fried snack foods and regular chocolate products.",
					"It is impressive that you have maintained a low intake of highly refined sugar-containing foods and beverages.",
					"Your intake of vegetables appears to be consistent with practices associated with a healthy diet.",
					"Your diet appears to lack proper attention to fruit intake.",
					"Your low intake of alcohol is most impressive from a health-risk standpoint.",
					"You indicated that you consume high fat meat products only 2-3 times per month, and seldom, if ever consume high fat dairy products, on average.",
					"It is most impressive that you do not use butter on bread products or on baked potatoes or vegetables.",
					// Body Composition text verification
					"Your Body Mass Index is:��20.60", 
					"This is a good reading and is associated with a low risk for the development of many degenerative conditions",
					"Your present waist circumference is", 
					"30.",
					// Physical Activity
					"You indicated that you perform endurance/aerobic exercise for 120 to 150 minutes per week, on average.",
					"You indicated that you regularly perform resistance or strength training a minimum of 4 times a week.",
					"You indicated that you regularly perform flexibility or stretching exercises a minimum of 4 times per week.",

					// Basic Supplement Considerations
					// No bold text Verifications
					
					// Nutrient Deficiencies and Depletion of Nutrients
					"Nutrient Deficiencies and Depletion of Nutrients",
					"Damage to the Skin from Sunlight and Tanning Beds",
					"Cracks at the outer margins (corners) of the lips can be caused or aggravated by a Vitamin B2 or vitamin B6 deficiency",
					"A sore or burning tongue can be caused by deficiencies in Vitamins B2, B3, B6, and/or B12.",
					"A reduced ability to taste food is can be caused by a marginal zinc deficiency.",
					"Gums that bleed easily is often a sign of Vitamin C deficiency. ",
					"From a nutritional standpoint skin that bruises easily can be caused by a deficiency ",
					"Slow healing capability can be an indication",
					"Feeling chronically fatigued ",
					"Irregular eating patterns often results in inadequate intake of many essential vitamins and minerals,",
					"Calorie restricted diets cannot provide optimal intake levels of all vitamins and minerals",
					"Feeling run down and/or in a weakened state of immunity",
					"Hair falling out easily can be from genetic or hormonal causes",
					"Your regular use of antacid drugs is associated with depletion of the following nutrients:",
					"Your regular use of cholesterol-lowering bile acid sequestrant drugs is associated with depletion of the following nutrients: ",
					"Your regular use of beta-blocker drugs",
					"Your regular use of digitalis is associated with depletion of the following nutrients: Vitamin B1.",
					"Your regular use of corticosteroids is associated with depletion of the following nutrients: ",
					"Your regular use of indomethacin is associated with depletion of the following nutrients",
					"Your regular use of diuretic drugs is associated with depletion of the following nutrients:",
					"Your regular use of L-dopa is associated with depletion of the following nutrients:",
					"The asthmatic drug theophylline decreases",
					"Stress",
					
					// Drug-Nutrient Interactions and Precautions
					"Drug-Nutrient Interactions and Precautions",
					"Various sections of your Meschino Health and Wellness report",
					
					// Family Health History
					"You indicated that there is a family history of breast cancer.",


					// Healthy Aging Supplement Considerations and Early Detection
					"Mammogram:  After age 40 it advisable to speak to your doctor about having a mammogram.",
					"Because you are over 45 you should consider a supplement providing coenzyme Q10 and hawthorn for heart health."
					
					//)
				);

			// Link Text verification
			List<String> Verifylinkslist = //new ArrayList<>(
					// Dietary Assessment
					Arrays.asList(
					// Drug-Nutrient Interactions and Precautions - No Links
					// No Link text verification required

					// Basic Blood Tests of Significance
					"http://www.nature.com/nrc/journal/v4/n3/full/nrc1298.html",
					"http://www.pnas.org/content/110/23/9523.short",
					"http://journals.plos.org/plosone/article?id=10.1371/journal.pone.0012244",
					"http://www.sciencedaily.com/releases/2010/09/100912213050.htm",
					// Nutrient Deficiencies and Depletion of Nutrients
					"http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/188",
					"http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/StressTracker?healthActivityId=13",
					// Family Helath 
					"https://meschinowellness.blob.core.windows.net/downloads/26F FHH Breast.pdf",
					//Basic Blood Tests of Significance 
					"https://meschinowellness.blob.core.windows.net/downloads/basic_blood_tests.pdf",
					"https://meschinowellness.blob.core.windows.net/downloads/blood_interp.pdf"
					
					//)
			);

			// italic Text verification
			List<String> Verifyitaliclist = //new ArrayList<>(
					// Drug-Nutrient Interactions and Precautions
					Arrays.asList("\"Do you have an active ulcer?\"",
					"\"Are you presently taking any anti-inflammatory drugs, other than acetaminophen, or blood thinners (anti-coagulants)?\"",
					"\"Do you have a pacemaker?\"",
					"\"Are you taking drugs to correct a heart arrhythmia problem?\"",
					"\"Are you taking any medications for Alzheimer's or dementia?\"",
					"\"Are you allergic to aspirin?\"",
					"\"Do you suffer from hemophilia?\"",
					"\"Are you presently experiencing a flare up of gout (gouty arthritis)?\"",
					"\"Do you have advanced liver disease?\"",
					"\"Are you taking a drug a called methotrexate?\"",
					"\"Are you presently taking any chemotherapy drugs or undergoing radiation therapy for the treatment of cancer?\"",
					"\"Are you currently taking any drugs for depression or to treat a psychological disorder of any kind (etc. bipolar disease, schizophrenia, obsessive compulsive disorder etc.)?\"",
					"\"Are you taking radioactive iodine to treat thyroid cancer, Grave�s disease or other thyroid disorder?\""
					//)
			);

			// Bold Text verification
			WebElement Contentofpage = d.findElement(By.className("wbc-content"));
			List<WebElement> Boldtexts = (List<WebElement>) Contentofpage.findElements(By.tagName("strong"));
			for (WebElement Boldtext : Boldtexts) {
				String text = Boldtext.getText();
				if (Verifytextlist != null && Verifytextlist.contains(text)) {
					// High Light element
					HighlightContent("strong",text);
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
					// High Light element
					HighlightContent("a",Linkstext.getText());
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
					// High Light element
					HighlightContent("em",text);
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
	public void HighlightContent(String elementTag, String text) {
		try
		{
			//System.out.println("Matched"); 
		  WebElement element = d.findElement(By.xpath("//"+elementTag+"[contains(text(),'"+text+"')]"));
	      //Creating JavaScriptExecuter Interface
		  JavascriptExecutor js = (JavascriptExecutor)d;
	      js.executeScript("arguments[0].scrollIntoView(true);", element); // scroll 
		  Thread.sleep(5000);
		  js.executeScript("arguments[0].style.border='4px groove red'", element);
	      //Thread.sleep(5000);
	      //js.executeScript("arguments[0].style.border=''", element);
		}
		catch(Exception ex )
		{
			System.out.println("Not Matched!"); 
		}
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