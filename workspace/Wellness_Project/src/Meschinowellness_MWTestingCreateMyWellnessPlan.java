import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.junit.Before;
import org.junit.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.NoSuchElementException;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.StaleElementReferenceException;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
//import org.openqa.selenium.firefox.FirefoxDriver;

import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.testng.Assert;
import org.testng.ITestResult;
//import org.testng.annotations.AfterMethod;

import org.testng.annotations.AfterMethod;

import com.google.common.base.Function;
//import org.testng.annotations.BeforeTest;
//import org.testng.annotations.Test;

public class Meschinowellness_MWTestingCreateMyWellnessPlan {
	String driverPath = "C:\\chromedriver_win32\\";//"C:\\geaco\\";
	private String UserName = "M2HRAtest@gmail.com"; //"mwtesting111@gmail.com"
	private String Password = "Test123@"; //"Hello789@"
	private String Url = "http://demo-meschinowellness.azurewebsites.net/Account/Login"; //"http://stage-meschinowellness.azurewebsites.net/Account/Login"
	
	WebDriver d;
	private CharSequence click;
	@Before
	public void Setup() throws Exception {
		// Launch browser
		System.out.println("launching firefox browser"); 
		//System.setProperty("webdriver.gecko.driver", driverPath+"geckodriver.exe");
		System.setProperty("webdriver.chrome.driver", driverPath+"chromedriver.exe");
		//d = new FirefoxDriver();
		d = new  ChromeDriver();
		d.manage().window().maximize();
		d.manage().deleteAllCookies();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		d.get(Url);
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
	}
	@Test//(priority = 1)
	public void login() throws Exception {
		// Enter Username
		d.findElement(By.id("UserName")).click();
		Thread.sleep(1000);
		d.findElement(By.id("UserName")).sendKeys(UserName);
		// Enter password
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).click();
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).sendKeys(Password);
		// Click on login button
		d.findElement(By.xpath("/html/body/div[3]/form/div[3]/div/div[2]/button")).click();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		// Verfiy Dashboard
		if (d.findElement(By.id("dashboard")) != null) {
			System.out.println("Dashboard Title  is verified");
		} else {
			System.out.println("Dashboard Title  is not  Present");
		}
		Thread.sleep(5000);
		System.out.println("User login successful");

		try
		{
			WebElement popupElement = d.findElement(By.id("chklistModelVideoPopup"));
			if (popupElement!=null) {
				System.out.println("popup displayed");
				// Click on close button
				d.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div[2]/div/a[2]")).click();
			}			
		} catch (Exception  e) {
			System.out.println("Exception while closing popup " + e.getMessage());
		}
		Thread.sleep(5000);
	//}

	//@Test//(priority = 2)
	//public void MyWellnessPlan() throws Exception {
		// Click on My Wellness Report tab
		WebElement mWellnessWalletPage = d.findElement(By.id("wallet-top-tabs"));
		WebElement mWellnessWalletPlan = mWellnessWalletPage.findElement(By.id("riskreport"));
		Thread.sleep(5000);
		mWellnessWalletPlan.click();
		Thread.sleep(10000);
		System.out.println("Text = Getting  Major Health Risks");
		List<WebElement> list = getListOfLinks("id_riskList");
		Assert.assertNotEquals(0, list.size());
		for (int i = 1, l = list.size(); i < l; i++) {
			validateMajorHealthRisks(i, "div-tab-content", "h3");
		}
		System.out.println("click-Corresponding-Link END : ");
		
		d.manage().timeouts().implicitlyWait(50, TimeUnit.SECONDS);
	}
	
	public static boolean isElementPresent(WebElement element) {
		boolean flag = false;
		try {
			if (element.isDisplayed()
					|| element.isEnabled())
				flag = true;
		} catch (NoSuchElementException e) {
			flag = false;
		} catch (StaleElementReferenceException e) {
			flag = false;
		}
		return flag;
	}

	// Getting the ListOfLinks 
	private List<WebElement> getListOfLinks(String sectionName) {
		List<WebElement> sections = new ArrayList<WebElement>();
		ArrayList<WebElement> mainTag = (ArrayList<WebElement>) d.findElements(By.id(sectionName));
		sections = mainTag.get(0).findElements(By.tagName("li"));
		return sections;
	}
        // Method for clickCorrespondingLink
	public void clickCorrespondingLink(int elementIndex) {
		List<WebElement> list = getListOfLinks("id_riskList");
		WebElement anchorTag = list.get(elementIndex).findElement(By.tagName("a"));
		System.out.println("click-Corresponding-Link : " + elementIndex +" " + anchorTag.getText());
		Assert.assertNotNull(anchorTag);
		anchorTag.click();
		
	}
	    // Method for validateMajorHealthRisks
	private void validateMajorHealthRisks(int elementIndex,
			String headingContainer, String headingSelector) throws InterruptedException {
		// Getting the left side menu and clicking it
		List<WebElement> list = getListOfLinks("id_riskList");
		WebElement currentLIItem = list.get(elementIndex);
		// getting left side menu elements into anchor tag
		WebElement anchorTag = list.get(elementIndex).findElement(By.tagName("a"));
		System.out.println("current-Link : " + anchorTag.getText());
		Assert.assertNotNull(anchorTag);
		String currentTabId = anchorTag.getAttribute("href");
		String[] splited = currentTabId.split("#");
		currentTabId = splited[1];
		System.out.println("Current Tab : " + currentTabId);
		// click on left menu side element
		anchorTag.click();
		Thread.sleep(5000);
		System.out.println("Current Major Health Risks Tilte is = " + anchorTag.getText().trim());
		// Getting the headers in the CreateMyWellnessPlan page
		WebElement headingContainerElement = d.findElement(By.id(headingContainer));
		Assert.assertNotNull(headingContainerElement);
		// List of the the headings on right hand side  in the CreateMyWellnessPlan page
		List<WebElement> headers = headingContainerElement.findElements(By.tagName("h3"));
		//For loop for getting heading text
		for (WebElement element : headers) {
			if (element.isDisplayed()) {
				System.out.println("Heading Texts  = " + element.getText().trim());
			}
		}
		// Displays the headings on right hand side  in the CreateMyWellnessPlan page
		WebElement currentTabContent = d.findElement(By.id(currentTabId));
		// Getting list of images in current tab(right hand side  in the CreateMyWellnessPlan page)
		List<WebElement> images = currentTabContent.findElements(By.className("graph"));
		System.out.println("Number of Images present = " + images.size());
		//For loop for getting the image dial color 
		for (WebElement element : images) {
			WebElement imagedial = element.findElement(By.tagName("input"));
			System.out.println("Image Dial Color  = " + GetColorName(imagedial.getAttribute("data-fgcolor")));
		}
		
		// Getting list of recommended goals
		WebElement recommendedGoals = currentTabContent.findElement(By.className("recomended-box"));
		// Getting list of recommended  goals buttons
		List<WebElement> recommenededButtons = recommendedGoals.findElements(By.tagName("a"));
		if (recommenededButtons != null) {
			System.out.println("recommenededbuttons count = " + recommenededButtons.size());
			//For loop for recommended goals buttons
			for (int i = 0, l = recommenededButtons.size(); i < l; i++) {
				//System.out.println("recommenededbuttons Text = " + recommenededButtons.get(i).getText());
				clickCorrespondingLink(elementIndex);
				Thread.sleep(5000);
				validaterecommendedgoals(currentTabId, "recomended-box", 0, i);
			}
		}
	}
	public String GetColorName (String ColorCode)
	{
		String colorName = "Green";
		switch(ColorCode){  
		case "#00B40D": 
			colorName = "Green";
			break;
		case "#B61300": 
			colorName = "Red";
			break;
		case "#FFDA0E": 
			colorName = "Yellow";
			break;
		default:
			colorName = "Green";
			break;  
		}  
		return colorName;
	}
	private static Function<WebDriver,WebElement> presenceOfElementLocated(final By locator) {
	    return new Function<WebDriver, WebElement>() {
	        @Override
	        public WebElement apply(WebDriver driver) {
	            return driver.findElement(locator);
	        }
	    };
	}
	public void validaterecommendedgoals(String contentName, String sectionName, int sectionIndex, int elementIndex)
			throws InterruptedException {
		System.out.println("contentName: " + contentName + "; sectionName :" + sectionName + "; Element-Index+" + elementIndex );
		
		WebElement currentTabContent = d.findElement(By.id(contentName));
		WebElement recommendedGoalSection = currentTabContent.findElement(By.className(sectionName));
		List<WebElement> recommenededButtonsGroup = null;
		recommenededButtonsGroup = recommendedGoalSection.findElements(By.tagName("a"));

		WebElement rdButton = recommenededButtonsGroup.get(elementIndex);
		System.out.println("Goal Buttton Text " + rdButton.getText());
		String btnText = rdButton.getText();
		if (rdButton.isDisplayed()){// && btnText.equalsIgnoreCase("SET GOAL")) {
			// if condition for tab_3
			if (contentName.equals("tab_3") && btnText.equalsIgnoreCase("SET GOAL")) {
				rdButton.click();
				System.out.println(" Getting the pop box elements");
				
				Thread.sleep(5000);
				// Getting the pop box elements
				WebElement popUpBox = d.findElement(By.id("SetGoalLossGainWeightDialog"));
				WebElement popUpBoxContent = popUpBox.findElement(By.className("modal-content"));
				// Getting the title pop box
				WebElement titlePopUpBox = popUpBoxContent.findElement(By.className("modal-header"));
				System.out.println("Title Text = " + titlePopUpBox.getText().trim());
				// Getting the footer elements from pop box
				WebElement footerPopUpBox = popUpBox.findElement(By.className("modal-footer"));
				List<WebElement> cancelButton = footerPopUpBox.findElements(By.tagName("button"));
				// Click on cancel button
				Actions actions = new Actions(d);
				actions.moveToElement(cancelButton.get(0)).click().perform();
				Thread.sleep(5000);
			} else {
				// Click on recommendedGoal button
				System.out.println(" Click on recommendedGoal button");
				rdButton.click();
				d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
				System.out.println("Text= Clicked on recommendedGoal button");
				System.out.println("Text= Navigating to next Page...");
				// active goal page title verification
				WebElement activeGoalHeading = d.findElement(By.className("active-goal-page"));
				WebElement titleElement = activeGoalHeading.findElement(By.className("title"));
				System.out.println("Title Text = " + titleElement.getText().trim());
				// Click on back button
				try
				{
					//WebElement activeGoalBackBtn = d.findElement(By.className("active-goal-heading"));
					
					//WebElement backButton = activeGoalBackBtn.findElement(By.className("pull-left"));

					//WebElement backButtonContains = activeGoalBackBtn.findElement(By.xpath("//a[contains(text(),'Back')]"));
					
					// times out after 5 seconds
					WebDriverWait wait = new WebDriverWait(d, 15);

					// while the following loop runs, the DOM changes - 
					// page is refreshed, or element is removed and re-added
					wait.until( presenceOfElementLocated(By.className("active-goal-heading")));        

					// now we're good - let's click the element
					
					d.findElement(By.className("pull-left")).click();
					//Actions actions = new Actions(d);
					//actions.moveToElement(backButton).perform();
					//backButtonContains.click();
					Thread.sleep(1000);
					//System.out.println("Back button class - " + backButton.getClass());
					//System.out.println("Back button Text - " + backButtonContains.getText());
					//System.out.println("Back button href - " + backButton.getAttribute("href"));
					//backButton.click();
					System.out.println("backButton Clicked");
					//Method to click  the option and scroll the content
						//JavascriptExecutor je = (JavascriptExecutor) d;
						//je.executeScript("arguments[0].click();", backButtonContains);
					
				} catch (Exception  e) {
					System.out.println("Exception while going back " + e.getMessage());
				}
				Thread.sleep(10000);
				d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
				
			}
		}
	}

	// }
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

