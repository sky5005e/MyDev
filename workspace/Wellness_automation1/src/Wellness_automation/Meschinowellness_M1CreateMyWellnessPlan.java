package Wellness_automation;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.openqa.selenium.By;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.interactions.Actions;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeTest;
import org.testng.annotations.Test;

public class Meschinowellness_M1CreateMyWellnessPlan {
	WebDriver d;
	private CharSequence click;
	@BeforeTest
	public void Setup() throws Exception {
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
		d.findElement(By.id("UserName")).sendKeys("M1HRAtest@gmail.com");
		// Enter password
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).click();
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).sendKeys("Test123@");
		// Click on login button
		d.findElement(By.xpath("/html/body/div[3]/form/div[3]/div/div[2]/button")).click();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		// Verify Dashboard
		if (d.findElement(By.id("dashboard")) != null) {
			System.out.println("Dashboard Title  is verified");
		} else {
			System.out.println("Dashboard Title  is not  Present");
		}
		Thread.sleep(5000);
		System.out.println("User login successful");
	}
	@Test(priority = 2)
	public void MyWellnessPlan() throws Exception {
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
			validateMajorHealthRisks("id_riskList", 0, i, "div-tab-content", "h3");
		}
		d.manage().timeouts().implicitlyWait(50, TimeUnit.SECONDS);
	}
	private List<WebElement> getListOfLinks(String sectionName) {
		List<WebElement> sections = new ArrayList<WebElement>();
		ArrayList<WebElement> mainTag = (ArrayList<WebElement>) d.findElements(By.id(sectionName));
		sections = mainTag.get(0).findElements(By.tagName("li"));
		return sections;
	}
	
	public void clickCorrespondingLink(int elementIndex){
		List<WebElement> list = getListOfLinks("id_riskList");
		WebElement anchorTag = list.get(elementIndex).findElement(By.tagName("a"));
		Assert.assertNotNull(anchorTag);
		anchorTag.click();
		
	}
	private void validateMajorHealthRisks(String sectionName, int sectionIndex, int elementIndex,
			String headingContainer, String headingSelector) throws InterruptedException {
		// Getting the left side menu and clicking it
		
		List<WebElement> list = getListOfLinks("id_riskList");
		WebElement currentLIItem = list.get(elementIndex);
		WebElement anchorTag = list.get(elementIndex).findElement(By.tagName("a"));
		Assert.assertNotNull(anchorTag);
		String currentTabId = anchorTag.getAttribute("href");
		String[] splited = currentTabId.split("#");
		currentTabId = splited[1];
		System.out.println("Current Tab : " + currentTabId);
		anchorTag.click();
		Thread.sleep(5000);
		System.out.println("Current Major Health Risks Tilte is = " + currentLIItem.getText().trim());
		
		//Getting  the headers in the CreateMyWellnessPlan page
		WebElement headingContainerElement = d.findElement(By.id(headingContainer));
		Assert.assertNotNull(headingContainerElement);

		//Displays the headings in the CreateMyWellnessPlan page
		List<WebElement> headers = headingContainerElement.findElements(By.tagName("h3"));
		for (WebElement element : headers) {
			if (element.isDisplayed()) {
				System.out.println("Heading Texts  = " + element.getText().trim());
			}
		}
		//Getting list of goals
		WebElement currentTabContent = d.findElement(By.id(currentTabId));
		WebElement recommendedGoals = currentTabContent.findElement(By.className("recomended-box"));
		//WebElement recommendedGoals = headingContainerElement.findElement(By.className("recomended-box"));
       List<WebElement> recommenededbuttons = recommendedGoals.findElements(By.tagName("a"));
		if(recommenededbuttons != null){
		System.out.println("recommenededbuttons count = " + recommenededbuttons.size());
		for (int i = 0, l = recommenededbuttons.size(); i < l; i++) {
					clickCorrespondingLink(elementIndex);
					Thread.sleep(5000);
					validaterecommendedgoals(currentTabId,"recomended-box", 0, i);
		}	
		}
		}
public void  validaterecommendedgoals (String contentName,String sectionName, int sectionIndex, int elementIndex) throws InterruptedException{
	WebElement currentTabContent = d.findElement(By.id(contentName));
	WebElement recommendedGoalSection = currentTabContent.findElement(By.className(sectionName));
	List<WebElement> recommenededbuttonsGroup =null;
	recommenededbuttonsGroup = recommendedGoalSection.findElements(By.tagName("a"));
   
   WebElement rdButton= recommenededbuttonsGroup.get(elementIndex);
   //System.out.println("test index = " + rdButton.getAttribute("onclick"));
   if(rdButton.isDisplayed()){
	   
  
   if(contentName.equals("tab_3")){
	   rdButton.click();
	   Thread.sleep(5000);
	   WebElement popUpBox = d.findElement(By.id("SetGoalLossGainWeightDialog"));
		WebElement popUpBoxContent = popUpBox.findElement(By.className("modal-content"));
		       
		WebElement titlepopUpBox = popUpBoxContent.findElement(By.className("modal-header"));
		System.out.println("Title Text = " + titlepopUpBox.getText().trim());
		
		WebElement footerPopUpBox =popUpBox.findElement(By.className("modal-footer"));
		List<WebElement> cancelButton= footerPopUpBox.findElements(By.tagName("button"));   
		Actions actions = new Actions(d);
		actions.moveToElement(cancelButton.get(0)).click().perform();
		Thread.sleep(5000);
	   
   }
   else
   {
	  // rdButton.click();
	   rdButton.click();
	   d.manage().timeouts().implicitlyWait(15, TimeUnit.SECONDS);
	   System.out.println("Text= Clicked on recommendedGoal button" );
	   System.out.println("Text= Navigating to next Page..." );
            // active goal page title verification
			WebElement activeGoalHeading = d.findElement(By.className("active-goal-page"));
			WebElement titleElement = activeGoalHeading.findElement(By.className("title"));
			System.out.println("Title Text = " + titleElement.getText().trim());
			// Click on back button
			WebElement backButton = activeGoalHeading.findElement(By.className("btn-default"));
			backButton.click();
			System.out.println("backButton Clicked");
			 Thread.sleep(10000);
			d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
			// Click on back button myWellnessPlannerPage
			WebElement myWellnessPlannerPage = d.findElement(By.id("wallet-top-tabs"));
			WebElement backButtonMWallet = myWellnessPlannerPage.findElement(By.id("anchorBackButton"));
			Thread.sleep(5000);

			Actions actions = new Actions(d);
			actions.moveToElement(backButtonMWallet).click().perform();
				// Thread.sleep(2000);
			//backButtonMWallet.click();
			System.out.println("backButtonMWallet Clicked");
			Thread.sleep(5000);
			System.out.println("current element is verified ");
			Thread.sleep(5000);
   }
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
