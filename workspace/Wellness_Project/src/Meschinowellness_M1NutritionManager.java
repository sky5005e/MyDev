import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.apache.commons.lang3.ObjectUtils.Null;
//import org.junit.Before;
//import org.junit.Test;
import org.testng.annotations.Test;

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
import org.openqa.selenium.support.ui.Select;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;

import com.google.common.base.Function;
import org.testng.annotations.BeforeTest;

public class Meschinowellness_M1NutritionManager {

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
	System.setProperty("webdriver.chrome.driver", driverPath + "chromedriver.exe");
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
		System.out.println("Dashboard Title is verified");
	} else {
		System.out.println("Dashboard Title is not Present");
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
			// Click Trackers & Resources
			d.findElement(By.id("toolsAndResources")).click();
			// Verify Trackers & Resources
			if (d.findElement(By.id("toolsAndResources")) != null) {
				System.out.println("Trackers & Resources is verified");
			} else {
				System.out.println("Trackers & Resources is not Present");
			}
			// Nutrition Manager
			System.out.println("Nutrition Manager");
			WebElement NutritionManagerLink = d.findElement(By.xpath("//a[contains(text(),'Nutrition Manager')]"));
			((JavascriptExecutor)d).executeScript("arguments[0].click();", NutritionManagerLink);
			// Nutrition Manager
			System.out.println("Moved on Nutrition Manager Page");
			Thread.sleep(3000);
			
			ClosePopupContent("nutrition-popup-master");
			System.out.println("Popup Verified of Nutrition Manager");
			
			
			WebElement tabContent = d.findElement(By.id("toptab"));
			List<WebElement> tabHeadings = tabContent.findElements(By.tagName("li"));
			Assert.assertNotEquals(0, tabHeadings.size());
			System.out.println("Heading title count ==>> " + tabHeadings.size());
			for (int i = 1, l = tabHeadings.size(); i < l; i++) {
				try
				{
				WebElement link = tabHeadings.get(i).findElement(By.tagName("a"));
				if(link !=  null)
				{
					((JavascriptExecutor)d).executeScript("arguments[0].click();", link);
					Thread.sleep(2000);
					System.out.println("Popup Open Clicked " + link.getText());	
					String _id =  link.getAttribute("id").trim();
					System.out.println("Popup Id: " + _id);	
					System.out.println("Popup Closed Clicked");
					//if(i == 0)
					//{
						//ClosePopupContent("nutrition-popup-master");
					//}
					//else 
					if(i == 1)
					{
					    ClosePopupContent("nutrition-popup-cancer");
						AddItem("Fiber Supplements");
					}
					else if(i == 2)
					{
					    ClosePopupContent("nutrition-popup-cholesterol");
					    AddItem("Vegetables");
					}
					else if(i == 3)
					{
					    ClosePopupContent("nutrition-popup-blood-glucose");
					    AddItem("High Fat Desserts, Treats, Snacks - Ooey Gooey");
					}
					else if(i == 4)
					{
					    ClosePopupContent("nutrition-popup-blood-pressure");
					    AddItem("Weight Loss");
					}
					Thread.sleep(20000);
				
				}
				}
				catch(Exception ex)
				{
					System.out.println("error Clicked" + ex.getMessage());	
				}
			}
			
			System.out.println("Master Link");  
            WebElement masterLink = d.findElement(By.id("master"));
            ((JavascriptExecutor)d).executeScript("arguments[0].click();", masterLink);
            Thread.sleep(2000);
            
            ClosePopupContent("nutrition-popup-master");
            System.out.println("Validated Master Item Content :");
            ValidateMasterItemContent();
             
             
            System.out.println("Remove-All-Item");
            //RemoveAllItem();
            List<WebElement> contentItems = d.findElements(By.className("btnaddremove"));
            Assert.assertNotEquals(0, contentItems.size());
            System.out.println("Remove Items count ==>> " + contentItems.size());
            for (int i = 0, l = contentItems.size(); i < l; i++) {
                try
                {
                    Thread.sleep(1000);
                    ((JavascriptExecutor)d).executeScript("arguments[0].click();", contentItems.get(i));
                    CloseOk(2);
                }
                catch(Exception ex)
                {
                    System.out.println("Eror on close button: " + ex.getMessage());
                    d.navigate().refresh();
                    ClosePopupContent("nutrition-popup-master");
                    RemoveAllItem();
                }
            }
        }
       //Method to click  Close Popup
		public void ClosePopupContent(String className) {
		try
		{
		    Thread.sleep(2000);
			WebElement PopupContent = d.findElement(By.id(className));
			WebElement closeButton =  PopupContent.findElement(By.className("close-btn"));
			((JavascriptExecutor)d).executeScript("arguments[0].click();", closeButton);
		}
		catch(Exception ex)
		{
			System.out.println("Eror on close button: " + ex.getMessage());
		}
		System.out.println("Popup Closed Clicked " + className);	
		
		}
		public void AddItem(String text) {
            try
            {
                System.out.println("select item to add " + text);  
                WebElement element = d.findElement(By.xpath("//span[contains(text(),'"+text+"')]/following::a[2]"));//[@class='btnaddremove']"));
                System.out.println("Get the add all link: element: " + element.getText());
                Thread.sleep(1000);
                ((JavascriptExecutor)d).executeScript("arguments[0].click();", element);
                System.out.println("Get the add all link: element click " + element.getText());
                CloseOk(1);
            }
            catch(Exception ex)
            {
                System.out.println("Eror on close button: " + ex.getMessage());
            }   
	    }
		//Method to Close-Ok
	    public void CloseOk(int count) throws InterruptedException  {
	        for (int i = 1; i <= count; i++) {
            
	            WebElement OkButton = d.findElement(By.className("btn-primary"));
                System.out.println("Ok Button : " + OkButton.getText());
                Thread.sleep(3000);
                if(OkButton.isDisplayed())
                {
                    OkButton.click(); 
                }
                else
                {
                   ((JavascriptExecutor)d).executeScript("arguments[0].click();", OkButton);
                }
                Thread.sleep(3000);
                
	        }
            
	    }
	    public void ValidateMasterItemContent() throws InterruptedException 
	    {
	        Thread.sleep(1000);
            List<WebElement> contentItems = d.findElements(By.className("showpopup"));
	        Assert.assertNotEquals(0, contentItems.size());
            System.out.println("Content Items count ==>> " + contentItems.size());
            for (int i = 0, l = contentItems.size(); i < l; i++) {
                try
                {
                    Thread.sleep(1000);
                    String text = contentItems.get(i).getText();
                    System.out.println("Recently Added item :" + text);
                }
                catch(Exception ex)
                {
                    System.out.println("Item not found error" + ex.getMessage());  
                }
            }
	    }
	    
	    public void RemoveAllItem() {
            List<WebElement> contentItems = d.findElements(By.className("btnaddremove"));
            Assert.assertNotEquals(0, contentItems.size());
            System.out.println("Remove Items count ==>> " + contentItems.size());
           // for (int i = 0, l = contentItems.size(); i < l; i++) {
                try
                {
                    Thread.sleep(1000);
                    ((JavascriptExecutor)d).executeScript("arguments[0].click();", contentItems.get(0));
                    CloseOk(2);
                }
                catch(Exception ex)
                {
                    System.out.println("Eror on close button: " + ex.getMessage());
                    d.navigate().refresh();
                    ClosePopupContent("nutrition-popup-master");
                    
                }
            //}
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

