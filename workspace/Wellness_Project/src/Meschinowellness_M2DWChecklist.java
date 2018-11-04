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
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.Select;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeTest;
import org.testng.annotations.Test;

public class Meschinowellness_M2DWChecklist {
	String driverPath = "C:\\chromedriver_win32\\";
	private String UserName = "M1HRAtest@gmail.com"; 
	private String Password = "Test123@";
	private String Url = "http://demo-meschinowellness.azurewebsites.net/Account/Login";
	//"http://stage-meschinowellness.azurewebsites.net/Account/Login"
	
	WebDriver d;
	private CharSequence click;
	@BeforeTest
	public void Setup() throws Exception {
		// Launch browser
		System.out.println("Launching  browser"); 
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
	@Test(priority = 1)
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
		Thread.sleep(3000);
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
		Thread.sleep(2000);
		}

		@Test(priority = 2)
		public void MyWellnessPlan() throws Exception {
			// Click on Checklist
			WebElement CheckList = d.findElement(By.className("checklist-bar"));
			WebElement anchorLink = CheckList.findElement(By.tagName("a"));
			System.out.println("Moved to Checklist = " + anchorLink.getAttribute("href"));
			
			anchorLink.click();
			System.out.println("Daily Wellness check list page loaded");
			WebElement TitleElem = d.findElement(By.className("active-goal-heading"));
			WebElement UserTitleElem = TitleElem.findElement(By.className("title"));
			WebElement UserTitle = UserTitleElem.findElement(By.tagName("h2"));
			
			System.out.println("User Name verified : " + UserTitle.getText());
			System.out.println("Verified Today Date : " + d.findElement(By.id("spdate")).getAttribute("value"));
			System.out.println("Whole week start and end date verified : " + d.findElement(By.className("weekly-info-text")).getText());
			
			
			List<WebElement> list = d.findElements(By.className("checklist-list"));
			System.out.println("Verified  all four cloumns: ");
			Assert.assertNotEquals(0, list.size());
			for (int i = 0, l = list.size(); i < l; i++) {
				WebElement h3title = list.get(i).findElement(By.tagName("h3"));
				System.out.println("==>> " + h3title.getText().trim());
			}
			for (int i = 0, l = list.size(); i < l; i++) {
				String item = "Test Drinks";
				if(i== 0)
					item = "Test Drinks";
				else if (i == 1)
					item = "Test Nutrition";
				else
					item = "Test";
				
				ValidatePopupContent(i,"edit-detail-popup", item);
				Thread.sleep(2000);
			}
			
			System.out.println("Validate video links of each items");
			ValidateVideoLinks();
		}
		
	    
		
		public void ValidatePopupContent(int elementIndex, String ClassName, String item)
				throws InterruptedException {
			System.out.println("current index : " + elementIndex +" Class name : " + ClassName  + " item : " + item);
			
			List<WebElement> list = d.findElements(By.className("checklist-list"));
			WebElement currentDivItem = list.get(elementIndex);
			System.out.println("Title of Popup current-DivItem :");
			WebElement EditPopup = currentDivItem.findElement(By.className("edit-popup"));
			EditPopup.click();
			System.out.println("Title of Popup :");
			WebElement PopupContent = d.findElement(By.id(ClassName));
			WebElement popupTitle = PopupContent.findElement(By.tagName("h4"));

			System.out.println("Class found ==> : " + popupTitle.getAttribute("class"));
			System.out.println("Title name ==> : " + popupTitle.getText());
			// Fill the textbox 
			try
			{
				PopupContent.findElement(By.id("subcatename")).sendKeys(item);
			}
			catch(Exception ex)
			{
				System.out.println("Eror : " + ex.getMessage());
			}
			
			System.out.println("Add your own item entered text: " + item);
			// set the drop down
			Select dropdown = new Select(PopupContent.findElement(By.id("subcateweekltgoal")));
			dropdown.selectByVisibleText("7");
			System.out.println("Weekly Goal Selected: 7");
			// click on the add button
			PopupContent.findElement(By.className("btnAdd")).click();
			//
			PopupContent.findElement(By.tagName("button")).click();
			System.out.println("Popup Closed Clicked");
			Thread.sleep(1000);
			try
			{
				System.out.println("Switched alert ok");	
				
				System.out.println(d.switchTo().alert().getText());
				d.switchTo().alert().accept();
			}
			catch(Exception ex)
			{
				System.out.println("Switched error " + ex.getMessage());	
			}
		}
		public void ValidateVideoLinks() throws InterruptedException {
		
			List<WebElement> Videolinks = d.findElements(By.className("watchvideo"));
			Assert.assertNotEquals(0, Videolinks.size());
			System.out.println("Video count ==>> " + Videolinks.size());
			for (int i = 0, l = Videolinks.size(); i < l; i++) {
				try
				{
				WebElement link = Videolinks.get(i);
				if(link !=  null)
				{
					String _id =  link.getAttribute("id").trim();
					System.out.println("Video Popup opened ==>> " + _id);
					((JavascriptExecutor)d).executeScript("arguments[0].click();", link);
					Thread.sleep(4000);
					d.findElement(By.id("btnVideoclose")).click();
					System.out.println("Popup Closed Clicked");	
					}
				}
				catch(Exception ex)
				{
					System.out.println("error Clicked" + ex.getMessage());	
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

