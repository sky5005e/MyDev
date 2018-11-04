
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



public class TextSelectionDemo {
	String driverPath = "C:\\geaco\\";
	public WebDriver d;
	
	@Before
	public void setUp()
	{
		System.out.println("launching firefox browser"); 
		System.setProperty("webdriver.gecko.driver", driverPath+"geckodriver.exe");
		d = new FirefoxDriver();
		//d.manage().window().maximize();
		//d.manage().deleteAllCookies();
		//d.navigate().to("http://demo-meschinowellness.azurewebsites.net/Account/Login");
		 //d.navigate().to("http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/49");//"http://www.uftHelp.com");
		 d.navigate().to("http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyRiskReport/GetFeedbackByReportAndSection?reportno=5253&fsNo=3&title=Physical%20Activity&back=healthRiskFactors");//"http://www.uftHelp.com");
		 d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		  	 
	}
	public void fnHighlightMe(WebDriver driver,String text) throws Exception{
		try
		{
		  WebElement element = d.findElement(By.xpath("//span[contains(text(),'"+text+"')]"));
			
		  //Creating JavaScriptExecuter Interface
		   JavascriptExecutor js = (JavascriptExecutor)driver;
		   for (int iCnt = 0; iCnt < 3; iCnt++) {
		      //Execute javascript
		        // js.executeScript("arguments[0].style.color='green'", element);
			   js.executeScript("arguments[0].setAttribute('style', arguments[1]);",element, "color: Red;background-color:lightskyblue; border: 2px dotted solid green;");
			  // js.executeScript("arguments[0].setAttribute('style', arguments[1]);",element, "");  
			   //js.executeScript("arguments[0].style.color='yellow'", element);
		         //js.executeScript("arguments[0].dblclick();", element);
		         Thread.sleep(1000);
		         js.executeScript("arguments[0].style.border=''", element);
		         System.out.println("Test success!");
		   }
		}
		catch(Exception ex)
		{
			System.out.println("failed");
			
		}
		 }
	@Test//(priority = 1)
	public void login() throws Exception {
			
		// Enter Username
		d.findElement(By.id("UserName")).click();
		Thread.sleep(1000);
		d.findElement(By.id("UserName")).sendKeys("M2HRAtest@gmail.com");
		// Enter password
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).click();
		d.findElement(By.xpath("/html/body/div[3]/form/div[2]/div/input")).sendKeys("Test123@");
		// Click on login button
		d.findElement(By.xpath("/html/body/div[3]/form/div[3]/div/div[2]/button")).click();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		//Open Application
		 // d.navigate().to("http://demo-meschinowellness.azurewebsites.net/AboutMeschino");//"http://www.uftHelp.com");
		
		//Find the element to highlight
		  //Function call to Highlight the element
		 // WebElement element2 = d.findElement(By.xpath("//p[contains(text(),'Dr. Meschino has extensively studied events in the body’s aging clock that makes one susceptible ')]"));
				fnHighlightMe(d,"Based upon your present age, your aerobic training zone is between 101 and 143");
		 
			
			
	}
}