

import java.io.File;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.openqa.selenium.By;
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeTest;
import org.testng.annotations.Test;



public class meschino_sample {
	WebDriver d;
	
	@BeforeTest

	public void Setup() throws Exception {
		System.setProperty("webdriver.firefox.marionette","C:\\geaco\\geckodriver.exe");
		// Launch browser
		d = new FirefoxDriver();
		d.manage().window().maximize();
		d.manage().deleteAllCookies();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		d.get("https://meschinowellness.com/Account/Login");
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);

	}

	@Test

	public void login() throws Exception {
		d.findElement(By.id("UserName")).click();
		Thread.sleep(1000);
		d.findElement(By.id("UserName")).sendKeys("testing@meschinowellness.com");

		// Enter password
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).click();
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).sendKeys("Welcome123");

		// Click on login button
		d.findElement(By.xpath("/html/body/div[3]/form/div[3]/div/div[2]/button")).click();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);

		//Click on close button
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

		// Click on my hra tab
		d.findElement(By.id("myHra")).click();

		// Verfiy myhra tilte

		if (d.findElement(By.id("myHra")) != null) {
			System.out.println("myHra  Title  is verified");
		} else {
			System.out.println("myHra  Title  is not  Present");
		}
		Thread.sleep(5000);

		// Click on My Wellness Report tab
		d.findElement(By.id("riskreportNew")).click();

		// Verfiy My Wellness Report tilte
		if (d.findElement(By.id("riskreportNew")).getText() != null) {
			
			Assert.assertEquals(d.findElement(By.id("riskreportNew")).getText(), "My Wellness Report");
				
			System.out.println("My Wellness Report  Title  is verified");
		} else {
			System.out.println("My Wellness Report  Title  is not  Present");
		}

		Thread.sleep(5000);
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
		
		
		// close application
		 d.quit();
	}

}


