
import java.io.File;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.support.ui.Select;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeTest;
import org.testng.annotations.Test;

public class Meschinowellness_demo {
	WebDriver d;
	
	@BeforeTest
	public void Setup() throws Exception {
		// Launch browser
		System.setProperty("webdriver.firefox.marionette","C:\\Selenium\\geckodriver-v0.11.1-win64\\geckodriver.exe");
		d = new FirefoxDriver();
		d.manage().window().maximize();
		d.manage().deleteAllCookies();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		d.get("http://demo-meschinowellness.azurewebsites.net/Account/Login");
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
	}
	@Test
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
		// Verfiy Dashboard
		if (d.findElement(By.id("dashboard")) != null) {
			System.out.println("Dashboard Title  is verified");
		} else {
			System.out.println("Dashboard Title  is not  Present");
		}
		Thread.sleep(5000);
		System.out.println("User login successful");
		Thread.sleep(5000);
		// Click on close button
		d.findElement(By.xpath("/html/body/div[8]/div/div/div/div/div[2]/div/a[2]")).click();
		Thread.sleep(5000);
		// Click on my hra tab
		d.findElement(By.id("myHra")).click();
		// Verfiy myhra tilte
		if (d.findElement(By.id("myHra")) != null) {
			System.out.println("myHra  Title  is verified");
		} else {
			System.out.println("myHra  Title  is not  Present");
		}	
		//click on restart button
		//d.findElement(By.id("btnRestart")).click();
		//Click on  "weight" drop down
		d.findElement(By.id("DDLQ277")).click();
		Thread.sleep(1000);
		//select 300 weight
		Select dropdown = new Select(d.findElement(By.id("DDLQ277")));
		dropdown.selectByVisibleText("300");
		Thread.sleep(5000);
		//Click on  "waist" drop down
		d.findElement(By.id("DDLQ278")).click();
		Thread.sleep(1000);
		//select 38 waist
                Select dropdown1 = new Select(d.findElement(By.id("DDLQ278")));
		dropdown1.selectByVisibleText("38");
		Thread.sleep(5000);
		//click on next button 
		d.findElement(By.xpath("/html/body/div[3]/div[2]/div/div[7]/div[2]/div[2]/div/div[2]/div[2]/div[3]/ul/li[2]/a")).click();
		Thread.sleep(5000);
		//click on 3 or more times per month 
	        d.findElement(By.xpath("/html/body/div[3]/div[2]/div/div[7]/div[2]/div[2]/div/div[2]/div[2]/div[2]/section[2]/div/div[4]/div[3]/form/div/div[2]/label/input")).click();
		Thread.sleep(5000);
		// Click on Daily radio button 
		d.findElement(By.xpath("/html/body/div[3]/div[2]/div/div[7]/div[2]/div[2]/div/div[2]/div[2]/div[2]/section[2]/div/div[5]/div[3]/form/div/div[1]/label/input")).click();
		Thread.sleep(500);
		//scroll to next question 
		WebElement element = d.findElement(By.xpath("/html/body/div[3]/div[2]/div/div[7]/div[2]/div[2]/div/div[2]/div[2]/div[2]/section[2]/div/div[6]/div[2]")); 
		((JavascriptExecutor) d).executeScript("arguments[0].scrollIntoView(true);", element); 
		Thread.sleep(500); 
		//Click on next button
		d.findElement(By.xpath("/html/body/div[3]/div[2]/div/div[7]/div[2]/div[2]/div/div[2]/div[2]/div[3]/ul/li[2]/a")).click();
		Thread.sleep(500);	
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
				// Copy files to specific location to store screenshot in our project home directory
				// result.getName() will return name of test case so that screenshot name will be same
				File file = new File("./Screenshots/" + result.getName() + ".png");
				FileUtils.copyFile(source, file);
				Throwable errorDetail = result.getThrowable(); 
				System.out.println("Screenshot taken");
			} catch (Exception e) {
				System.out.println("Exception while taking screenshot " + e.getMessage());
			}
		}
		
		//close application
		 //d.quit();
	}

}
