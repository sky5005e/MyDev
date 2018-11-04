
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.datatransfer.StringSelection;
import java.awt.event.KeyEvent;
import java.util.List;
import java.util.concurrent.TimeUnit;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.support.ui.Select;


public class DemoDebugWeb {
	
	public static WebDriver d;
	
	static String driverPath = "C:\\chromedriver_win32\\";//"C:\\geaco\\";
	private static String UserName = "mwtesting111@gmail.com";
	private static String Password = "Hello789@";
	private static String Url = "http://stage-meschinowellness.azurewebsites.net/Account/Login";//"http://demo-meschinowellness.azurewebsites.net/Account/Login"; 
	
	public static void main(String[] args) throws Exception {
			
		System.setProperty("webdriver.chrome.driver", driverPath+"chromedriver.exe");
		d.manage().window().maximize();
		d.manage().deleteAllCookies();
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		d.get(Url);
		d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);

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

		
      //WebDriver driver = new FirefoxDriver();
		/*
		driver = new ChromeDriver();
		driver.manage().window().maximize();
		driver.get("http://hissvr02/Intrepid/Accent/Web");
		//driver.get("http://www.uftHelp.com");
		driver.findElement(By.linkText("Login")).click(); 
		WebElement Email = driver.findElement(By.id("txtUsername"));
		Email.sendKeys("priya.kumari@hicom.in");
		WebElement Password = driver.findElement(By.id("txtPassword"));
		Password.sendKeys("Hicom@123");
		driver.findElement(By.xpath("//button[@type='submit']")).click();
		
		Thread.sleep(2000);
		*/
	/*// Drag and Drop through Action class
		Actions builder = new Actions(driver);
		WebElement dragElementFrom = driver.findElement(By.id("30"));
		WebElement dropElementTo = driver.findElement(By.id("29"));
		builder.clickAndHold(dragElementFrom).moveToElement(dropElementTo).perform();
		Thread.sleep(5000);
		builder.release(dropElementTo).build().perform();
		Thread.sleep(5000);
		
	
		driver.findElement(By.cssSelector("a[title='My Account'] > img")).click();
		Thread.sleep(2000);
		
	// Upload file through choose file field
			WebElement elem = driver.findElement(By.id("file"));
			String js = "arguments[0].style.height='auto'; arguments[0].style.visibility='visible';";

			((JavascriptExecutor) driver).executeScript(js, elem);
			
			driver.findElement(By.id("file")).clear();
			driver.findElement(By.id("file")).sendKeys("C:\\Users\\priya kumari\\Desktop\\video-sample-img5.png");
			driver.findElement(By.id("SaveProfileImage")).click();
			Thread.sleep(5000);
		
	// upload file through Popup window	
		driver.findElement(By.id("upload_link")).click();			
		StringSelection s = new StringSelection("C:\\Users\\priya kumari\\Pictures\\Screenshot\\35.NewsPost.png");
	    Toolkit.getDefaultToolkit().getSystemClipboard().setContents(s, null);
	    Robot robot = new Robot();
		
		 Thread.sleep(1000);
		  // Press Enter
		 robot.keyPress(KeyEvent.VK_ENTER);
		 // Release Enter
		 robot.keyRelease(KeyEvent.VK_ENTER);
		 // Press CTRL+V
		 robot.keyPress(KeyEvent.VK_CONTROL);
		 robot.keyPress(KeyEvent.VK_V);
		 // Release CTRL+V
		 robot.keyRelease(KeyEvent.VK_CONTROL);
		 robot.keyRelease(KeyEvent.VK_V);
		 Thread.sleep(1000);
		 robot.keyPress(KeyEvent.VK_ENTER);
		 robot.keyRelease(KeyEvent.VK_ENTER);
		 Thread.sleep(3000);
		 driver.findElement(By.id("SaveProfileImage")).click();
		 Thread.sleep(3000);*/
		
		/*// Direct Message  
		 driver.findElement(By.cssSelector("a[title='Messages'] > img")).click();
		 Thread.sleep(2000);
		 driver.findElement(By.xpath("//div/div/div/button")).click();
		 Thread.sleep(2000);
		 
		 driver.findElement(By.xpath("//a[contains(text(),'Direct Message')]")).click();

		 Thread.sleep(2000);
		 driver.findElement(By.id("txtTo")).sendKeys("pr");
		 Thread.sleep(2000);
		 driver.findElement(By.id("txtTo")).sendKeys(Keys.DOWN);
		 Thread.sleep(2000);
		 driver.findElement(By.id("txtTo")).sendKeys(Keys.TAB);
		 Thread.sleep(2000);
		 
		 
		 driver.findElement(By.id("txtSubject")).sendKeys("Test");
		 driver.findElement(By.id("MessageText")).sendKeys("Testing of Direct Message");
		 Thread.sleep(2000);
		 driver.findElement(By.id("btnSend")).click();
		 Thread.sleep(2000);
		 driver.findElement(By.xpath("(//button[@type='button'])[3]")).click();*/
		
		//Post News Article
		 /*driver.findElement(By.linkText("Resources")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.linkText("News")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.xpath("(//button[@type='button'])[2]")).click();
		 Thread.sleep(1000);
		 int i=0;
		 for(i=0;i<21;i++){
		 driver.findElement(By.id("newNewsPostPopup")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.id("NewsTitle")).sendKeys("Test News");
		 driver.findElement(By.id("NewsDesc")).sendKeys("Testing of News Article");
		 WebElement iframe1 = driver.findElement(By.tagName("iframe"));
	     driver.switchTo().frame(iframe1);
         WebElement web1=driver.findElement(By.xpath("//body"));
         web1.clear();	    
	     Actions act1=new Actions(driver);	            
	     act1.sendKeys(web1, "hello").build().perform();
	     driver.switchTo().defaultContent();
	    
	     WebElement newsimage = driver.findElement(By.id("file"));
	     newsimage.sendKeys("C:\\Users\\priya kumari\\Pictures\\Screenshot\\35.NewsPost.png");
	     Thread.sleep(1000);
	     driver.findElement(By.cssSelector("span.fr > button.btn-modal")).click();
		 }
		 i++;*/
		 
		 
		
		//Post Article 
	     /*driver.findElement(By.linkText("Resources")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.linkText("Articles")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.xpath("(//button[@type='button'])[4]")).click();
		 Thread.sleep(1000);
		 int i=0;
		 for(i=0;i<100;i++){
		 driver.findElement(By.id("newArticlePostPopup")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.id("ArticleTitle")).sendKeys("Test Article");
		 driver.findElement(By.id("ArticleDesc")).sendKeys("Testing of Article");
		 WebElement iframe2 = driver.findElement(By.tagName("iframe"));
	     driver.switchTo().frame(iframe2);
         WebElement web2=driver.findElement(By.xpath("//body"));
         web2.clear();	    
	     Actions act2=new Actions(driver);	            
	     act2.sendKeys(web2, "Test").build().perform();
	     driver.switchTo().defaultContent();    
	     WebElement articleimage = driver.findElement(By.id("file"));
	     articleimage.sendKeys("C:\\Users\\priya kumari\\Pictures\\Screenshot\\35.NewsPost.png");
	     Thread.sleep(1000);
	     driver.findElement(By.cssSelector("span.fr > button.btn-modal")).click();   
		 }
		 i++;*/
		
		
		//Find the element to highlight
				//  WebElement element = driver.findElement(By.xpath("//a[contains(text(),'Basic Linux')]"));
				  //Function call to Highlight the element
				  //fnHighlightMe(driver,element);
				  
				
				//driver.findElement(By.cssSelector("a[title='Messages'] > img")).click();
				// Thread.sleep(5000);
				//Highlight text element
				/*WebElement text = driver.findElement(By.xpath("/html/body/div[1]/div/div/div/b[contains(text(),'Logged in as Priya Kumari')]"));
				System.out.println(text);
				Actions act = new Actions(driver);
				
				act.moveToElement(text, 10, 25).click().build().perform();
				System.out.println("test");	
				*/
				/* Actions select = new Actions(driver);
				 WebElement text = driver.findElement(By.xpath("/html/body/div[1]/div/div/div/b[contains(text(),'Priya')]"));
				
				 select.doubleClick(text).build().perform();*/
		/*
		// FAQs
		 driver.findElement(By.linkText("Resources")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.linkText("FAQs")).click();
		 Thread.sleep(2000);
		/* driver.findElement(By.xpath("//div[@id='div_Faqs']/table/tbody/tr/td")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.xpath("//div[@id='div_Faqs']/table[2]/tbody/tr/td")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.xpath("//div[@id='div_Faqs']/table[3]/tbody/tr/td")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.xpath("//div[@id='div_Faqs']/table[4]/tbody/tr/td")).click();
		 Thread.sleep(1000);
		 driver.findElement(By.id("btn_clear")).click();
		 
		 driver.findElement(By.id("newFAQPopup")).click();
		 Thread.sleep(1000);
		 Select AddFAQValue = new Select(driver.findElement(By.id("SubHeading")));
		 AddFAQValue.selectByVisibleText("FAQ");
		 AddFAQValue.selectByVisibleText("General System");
		 AddFAQValue.selectByVisibleText("Leave Manager");
		// AddFAQValue.selectByVisibleText("Reference Tables");
		 
		 WebElement ques = driver.findElement(By.id("Question"));
		 ques.sendKeys("What is Accent");
		 WebElement Ans = driver.findElement(By.id("Answer"));
		 Ans.sendKeys("Accent is a system designed to managed and support the trainee and trainer workforce of the NHS. It replaces a number of separate systems that you may have used up until now and with a single Login, you can now access the resources and functions previously available through these systems.");
		 Thread.sleep(2000);
		 driver.findElement(By.cssSelector("span.fr > button.btn-modal")).click();
		 
		 Thread.sleep(5000);
	     driver.close();*/

	}
	
	/*public static void fnHighlightMe(WebDriver driver,WebElement element) throws InterruptedException{
		  //Creating JavaScriptExecuter Interface
		   JavascriptExecutor js = (JavascriptExecutor)driver;
		   for (int iCnt = 0; iCnt < 3; iCnt++) {
		      //Execute javascript
		         js.executeScript("arguments[0].style.border='4px groove red'", element);
		         Thread.sleep(1000);
		         js.executeScript("arguments[0].style.border=''", element);
		   }
		 }*/
}
