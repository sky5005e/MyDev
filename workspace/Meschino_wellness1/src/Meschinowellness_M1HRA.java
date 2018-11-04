
import java.io.File;
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
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.support.ui.Select;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeTest;
import org.testng.annotations.Test;

public class Meschinowellness_M1HRA {
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
		// Verfiy Dashboard
		if (d.findElement(By.id("dashboard")) != null) {
			System.out.println("Dashboard Title  is verified");
		} else {
			System.out.println("Dashboard Title  is not  Present");
		}
		Thread.sleep(5000);
		System.out.println("User login successful");
		Thread.sleep(5000);
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
	}
	@Test(priority = 2)
	public void MyHraTab() throws Exception {
		// Click on my hra tab
		d.findElement(By.id("myHra")).click();
		// Verfiy myhra tilte
		if (d.findElement(By.id("myHra")) != null) {
			System.out.println("myHra  Title  is verified");
		} else {
			System.out.println("myHra  Title  is not  Present");
		}
		// click on restart button
		//d.findElement(By.id("btnRestart")).click();
		// Click on "weight" drop down
		d.findElement(By.id("DDLQ277")).click();
		Thread.sleep(1000);
		// select 300 weight
		Select dropdown = new Select(d.findElement(By.id("DDLQ277")));
		dropdown.selectByVisibleText("300");
		Thread.sleep(5000);
		// Click on "waist" drop down
		d.findElement(By.id("DDLQ278")).click();
		Thread.sleep(1000);
		// select 38 waist
		Select dropdown1 = new Select(d.findElement(By.id("DDLQ278")));
		dropdown1.selectByVisibleText("38");
		Thread.sleep(5000);
		// click on next button
		WebElement anchorTag1 = d.findElement(By.cssSelector("a[href='#next']"));
		anchorTag1.click();
		Thread.sleep(5000);
		//Getting the whole vertical tabs into Element 1 
		WebElement Element1 = d.findElement(By.id("vertical-tabs"));
		System.out.println("Question number====>>>>" + Element1.getAttribute("id"));
        //Getting unordered list(ul) Hratablist from Element1 
		WebElement Hratablist = Element1.findElements(By.tagName("ul")).get(0);
		//Getting list of left links with the tag name "li"
		List<WebElement> leftLinks =  Hratablist.findElements(By.tagName("li"));
		int leftLinkSize=leftLinks.size()-1;
		////To get the right side tab contents related to leftlinks
		for(int i=1;i<leftLinks.size();i++)
		{
			System.out.println("leftLinks====>>>>" + leftLinks.get(i).getText());
			//To get the right side tab contents related to leftlinks based on index
			WebElement tabContent = d.findElement(By.id("vertical-tabs-p-"+i));
			//Getting the list of all the questions under "tabcontent"
			List<WebElement> radioquestion = tabContent.findElements(By.className("Question"));
			//To answer the questions for loop is being used
			for(WebElement local_radio:radioquestion){ 
				//Getting the question Id
				String value = local_radio.getAttribute("id");
				System.out.println("Question number====>>>>" + value); 
				String AnswerId = "Answers" + value;
				//Get the webelement based on AnswerId
				WebElement AnswerGroup = local_radio.findElement(By.id(AnswerId));
				//Get the list of answers based on questionId by using tagname 'label'
				List<WebElement> AnswerList = AnswerGroup.findElements(By.tagName("label"));
				//To answer the question for loop is being used
				for (int a = 0; a < AnswerList.size(); a++) {
					//Getting the answer from the map based on question
					int answer = GetQuestionValue.get(value);
					if (a == answer) {
						//To select the option and scroll the content
						GetElement(AnswerList.get(a));
						System.out.println("Selected Option text====>>>>" + AnswerList.get(a).getText());
					}
				}
			  }
			//Finding finish button on last page
			if(i!=leftLinkSize){
			//click on next button
			WebElement anchorTag = d.findElement(By.cssSelector("a[href='#next']"));
			anchorTag.click();
			}
		}
			if(d.findElement(By.cssSelector("a[href='#finish']"))!=null){
			//click on finish button
			WebElement anchorTag = d.findElement(By.cssSelector("a[href='#finish']"));
			anchorTag.click();
				}
			Thread.sleep(3000);
			 //click on  Accept check box
			d.findElement(By.id("finishAccept")).click();
			Thread.sleep(3000);
			//click on  confirm button
			d.findElement(By.id("confirmConsent")).click();
			Thread.sleep(3000);
			System.out.println("M1HRA has been sucessfully completed");
	}
    //Method to click  the option and scroll the content
	public void GetElement(WebElement element) {
		JavascriptExecutor je = (JavascriptExecutor) d;
		je.executeScript("arguments[0].click();", element);
	}
	public static Map<String, Integer> GetQuestionValue = new HashMap<String, Integer>();
	{
		GetQuestionValue.put("Q1", 1);
		GetQuestionValue.put("Q2", 4);
		GetQuestionValue.put("Q3", 0);
		GetQuestionValue.put("Q4", 1);
		GetQuestionValue.put("Q5", 0);
		GetQuestionValue.put("Q6", 1);
		GetQuestionValue.put("Q7", 2);
		GetQuestionValue.put("Q8", 1);
		GetQuestionValue.put("Q9", 1);
		GetQuestionValue.put("Q10", 0);
		GetQuestionValue.put("Q11", 0);
		GetQuestionValue.put("Q12", 2);
		GetQuestionValue.put("Q13", 1);
		GetQuestionValue.put("Q14", 0);
		GetQuestionValue.put("Q15", 3);
		GetQuestionValue.put("Q16", 3);
		GetQuestionValue.put("Q17", 2);
		GetQuestionValue.put("Q18", 3);
		GetQuestionValue.put("Q19", 0);
		GetQuestionValue.put("Q20", 4);
		GetQuestionValue.put("Q21", 3);
		GetQuestionValue.put("Q22", 0);
		GetQuestionValue.put("Q23", 1);
		GetQuestionValue.put("Q24", 1);
		GetQuestionValue.put("Q25", 1);
		GetQuestionValue.put("Q26", 1);
		GetQuestionValue.put("Q27", 1);
		GetQuestionValue.put("Q28", 1);
		GetQuestionValue.put("Q29", 1);
		GetQuestionValue.put("Q30", 1);
		GetQuestionValue.put("Q31", 0);
		GetQuestionValue.put("Q32", 0);
		GetQuestionValue.put("Q33", 0);
		GetQuestionValue.put("Q34", 0);
		GetQuestionValue.put("Q35", 0);
		GetQuestionValue.put("Q36", 0);
		GetQuestionValue.put("Q37", 2);
		GetQuestionValue.put("Q38", 2);
		GetQuestionValue.put("Q39", 2);
		GetQuestionValue.put("Q40", 0);
		 GetQuestionValue.put("Q41",0);
		 GetQuestionValue.put("Q42",3);
		 GetQuestionValue.put("Q43",2);
		GetQuestionValue.put("Q44", 1);
		GetQuestionValue.put("Q45", 1);
		GetQuestionValue.put("Q46", 1);
		GetQuestionValue.put("Q47", 1);
		GetQuestionValue.put("Q48", 1);
		GetQuestionValue.put("Q49", 1);
		GetQuestionValue.put("Q50", 1);
		GetQuestionValue.put("Q51", 1);
		GetQuestionValue.put("Q52", 1);
		GetQuestionValue.put("Q53", 1);
		GetQuestionValue.put("Q54", 1);
		GetQuestionValue.put("Q55", 1);
		GetQuestionValue.put("Q56", 0);
		GetQuestionValue.put("Q57", 1);
		GetQuestionValue.put("Q58", 0);
		GetQuestionValue.put("Q59", 0);
		GetQuestionValue.put("Q60", 0);
		GetQuestionValue.put("Q61", 0);
		GetQuestionValue.put("Q62", 0);
		GetQuestionValue.put("Q63", 0);
		GetQuestionValue.put("Q64", 0);
		GetQuestionValue.put("Q65",1);
		GetQuestionValue.put("Q66",0);
		GetQuestionValue.put("Q67", 1);
		GetQuestionValue.put("Q68", 0);
		GetQuestionValue.put("Q69", 0);
		GetQuestionValue.put("Q70", 1);
		GetQuestionValue.put("Q71", 1);
		GetQuestionValue.put("Q72",1);
		GetQuestionValue.put("Q73", 1);
		GetQuestionValue.put("Q74", 1);
		GetQuestionValue.put("Q75", 1);
		GetQuestionValue.put("Q76", 0);
		GetQuestionValue.put("Q77", 1);
		GetQuestionValue.put("Q78", 1);
		GetQuestionValue.put("Q79", 0);
		// GetQuestionValue.put("Q80",0);
		// GetQuestionValue.put("Q81",0);
		GetQuestionValue.put("Q82", 0);
		GetQuestionValue.put("Q83", 1);
		GetQuestionValue.put("Q84", 0);
		// GetQuestionValue.put("Q85",0);
		GetQuestionValue.put("Q86", 1);
		GetQuestionValue.put("Q87", 1);
		GetQuestionValue.put("Q88", 1);
		GetQuestionValue.put("Q89", 1);
		GetQuestionValue.put("Q90", 1);
		// GetQuestionValue.put("Q91",0);
		GetQuestionValue.put("Q92", 3);
		GetQuestionValue.put("Q93", 0);
		GetQuestionValue.put("Q94", 0);
		// GetQuestionValue.put("Q95",0);
		GetQuestionValue.put("Q96", 1);
		GetQuestionValue.put("Q97", 0);
		GetQuestionValue.put("Q98", 1);
		GetQuestionValue.put("Q99", 1);
		GetQuestionValue.put("Q100", 0);
		GetQuestionValue.put("Q101", 0);
		GetQuestionValue.put("Q102", 0);
		GetQuestionValue.put("Q103", 0);
		GetQuestionValue.put("Q104", 0);
		GetQuestionValue.put("Q105", 0);
		GetQuestionValue.put("Q106", 0);
		GetQuestionValue.put("Q107", 0);
		GetQuestionValue.put("Q108", 0);
		GetQuestionValue.put("Q109", 1);
		GetQuestionValue.put("Q110", 1);
		GetQuestionValue.put("Q111", 1);
		GetQuestionValue.put("Q112", 1);
		GetQuestionValue.put("Q113", 1);
		GetQuestionValue.put("Q114", 1);
		GetQuestionValue.put("Q115", 1);
		GetQuestionValue.put("Q116", 1);
		GetQuestionValue.put("Q117", 1);
		GetQuestionValue.put("Q118", 0);
		GetQuestionValue.put("Q119", 1);
		GetQuestionValue.put("Q120", 0);
		// GetQuestionValue.put("Q121",3);
		GetQuestionValue.put("Q122", 3);
		GetQuestionValue.put("Q123", 1);
		GetQuestionValue.put("Q124", 0);
		GetQuestionValue.put("Q125", 1);
		// GetQuestionValue.put("Q126",0);
		GetQuestionValue.put("Q127", 0);
		GetQuestionValue.put("Q128", 0);
		GetQuestionValue.put("Q129", 1);
		GetQuestionValue.put("Q130", 1);
		GetQuestionValue.put("Q131", 1);
		GetQuestionValue.put("Q132", 1);
		GetQuestionValue.put("Q133", 1);
		GetQuestionValue.put("Q134", 1);
		GetQuestionValue.put("Q135", 1);
		GetQuestionValue.put("Q136", 1);
		GetQuestionValue.put("Q137", 1);
		GetQuestionValue.put("Q138", 1);
		GetQuestionValue.put("Q139", 1);
		GetQuestionValue.put("Q140", 1);
		GetQuestionValue.put("Q141", 1);
		GetQuestionValue.put("Q142", 1);
		GetQuestionValue.put("Q143", 1);
		GetQuestionValue.put("Q144", 1);
		GetQuestionValue.put("Q145", 1);
		GetQuestionValue.put("Q146", 1);
		GetQuestionValue.put("Q147", 1);
		GetQuestionValue.put("Q148", 1);
		GetQuestionValue.put("Q149", 1);
		GetQuestionValue.put("Q150", 1);
		GetQuestionValue.put("Q151", 1);
		GetQuestionValue.put("Q152", 1);
		GetQuestionValue.put("Q153", 1);
		GetQuestionValue.put("Q154", 1);
		GetQuestionValue.put("Q155", 1);
		GetQuestionValue.put("Q156", 1);
		GetQuestionValue.put("Q157", 1);
		GetQuestionValue.put("Q158", 1);
		GetQuestionValue.put("Q159", 1);
		GetQuestionValue.put("Q160", 1);
		GetQuestionValue.put("Q161", 1);
		GetQuestionValue.put("Q162", 1);
		GetQuestionValue.put("Q163", 3);
		GetQuestionValue.put("Q164", 1);
		GetQuestionValue.put("Q165", 1);
		GetQuestionValue.put("Q166", 1);
		GetQuestionValue.put("Q167", 1);
		GetQuestionValue.put("Q168", 1);
		GetQuestionValue.put("Q169", 1);
		GetQuestionValue.put("Q170", 0);
		GetQuestionValue.put("Q171", 0);
		GetQuestionValue.put("Q172", 0);
		GetQuestionValue.put("Q173", 1);
		GetQuestionValue.put("Q174", 1);
		GetQuestionValue.put("Q175", 1);
		GetQuestionValue.put("Q176", 1);
		GetQuestionValue.put("Q177", 1);
		GetQuestionValue.put("Q178", 1);
		GetQuestionValue.put("Q179", 1);
		GetQuestionValue.put("Q180", 1);
		GetQuestionValue.put("Q181", 1);
		GetQuestionValue.put("Q182", 1);
		// GetQuestionValue.put("Q183",2);
		// GetQuestionValue.put("Q184",1);
		// GetQuestionValue.put("Q185",0);
		// GetQuestionValue.put("Q186",3);
		// GetQuestionValue.put("Q187",2);
		// GetQuestionValue.put("Q188",3);
		// GetQuestionValue.put("Q189",2);
		// GetQuestionValue.put("Q190",1);
		// GetQuestionValue.put("Q191",0);
		// GetQuestionValue.put("Q192",3);
		// GetQuestionValue.put("Q193",2);
		// GetQuestionValue.put("Q194",1);
		// GetQuestionValue.put("Q195",0);
		// GetQuestionValue.put("Q196",3);
		// GetQuestionValue.put("Q197",2);
		// GetQuestionValue.put("Q198",3);
		// GetQuestionValue.put("Q199",2);
		// GetQuestionValue.put("Q200",1);
		// GetQuestionValue.put("Q201",0);
		// GetQuestionValue.put("Q202",3);
		// GetQuestionValue.put("Q203",3);
		GetQuestionValue.put("Q204", 1);
		GetQuestionValue.put("Q205", 0);
		// GetQuestionValue.put("Q206",0);
		// GetQuestionValue.put("Q207",3);
		// GetQuestionValue.put("Q208",2);
		GetQuestionValue.put("Q209", 2);
		// GetQuestionValue.put("Q210",0);
		// GetQuestionValue.put("Q211",3);
		// GetQuestionValue.put("Q212",2);
		GetQuestionValue.put("Q213", 0);
		GetQuestionValue.put("Q214", 0);
		GetQuestionValue.put("Q215", 0);
		GetQuestionValue.put("Q216", 0);
		GetQuestionValue.put("Q217", 0);
		GetQuestionValue.put("Q218", 0);
		GetQuestionValue.put("Q219", 1);
		GetQuestionValue.put("Q220", 3);
		// GetQuestionValue.put("Q221",3);
		GetQuestionValue.put("Q222", 1);
		GetQuestionValue.put("Q223", 1);
		GetQuestionValue.put("Q224", 0);
		GetQuestionValue.put("Q225", 1);
		GetQuestionValue.put("Q226", 0);
		GetQuestionValue.put("Q227", 0);
		GetQuestionValue.put("Q228", 1);
		GetQuestionValue.put("Q229", 2);
		GetQuestionValue.put("Q230", 1);
		GetQuestionValue.put("Q231", 0);
		// GetQuestionValue.put("Q232",2);
		GetQuestionValue.put("Q233", 0);
		GetQuestionValue.put("Q279", 1);
		// GetQuestionValue.put("Q234",2);
		// GetQuestionValue.put("Q235",2);
		GetQuestionValue.put("Q236", 0);
		GetQuestionValue.put("Q237", 1);
		// GetQuestionValue.put("Q238",2);
		GetQuestionValue.put("Q239", 0);
		GetQuestionValue.put("Q240", 0);
		GetQuestionValue.put("Q241", 0);
		GetQuestionValue.put("Q242", 0);
		// GetQuestionValue.put("Q243",2);
		GetQuestionValue.put("Q244", 0);
		GetQuestionValue.put("Q245", 0);
		GetQuestionValue.put("Q246", 0);
		GetQuestionValue.put("Q247", 0);
		GetQuestionValue.put("Q248", 0);
		// GetQuestionValue.put("Q249",2);
		GetQuestionValue.put("Q250", 0);
		GetQuestionValue.put("Q251", 0);
		GetQuestionValue.put("Q252", 0);
		GetQuestionValue.put("Q253", 0);
		GetQuestionValue.put("Q254", 1);
		GetQuestionValue.put("Q255", 1);
		GetQuestionValue.put("Q256", 1);
		GetQuestionValue.put("Q257", 1);
		GetQuestionValue.put("Q258", 1);
		GetQuestionValue.put("Q259", 1);
		GetQuestionValue.put("Q260", 1);
		GetQuestionValue.put("Q261", 1);
		GetQuestionValue.put("Q262", 1);
		GetQuestionValue.put("Q263", 1);
		GetQuestionValue.put("Q264", 1);
		GetQuestionValue.put("Q265", 1);
		GetQuestionValue.put("Q266", 1);
		GetQuestionValue.put("Q267", 1);
		GetQuestionValue.put("Q268", 1);
		GetQuestionValue.put("Q269", 1);
		GetQuestionValue.put("Q270", 1);
		GetQuestionValue.put("Q271", 1);
		GetQuestionValue.put("Q272", 1);
		GetQuestionValue.put("Q273", 1);
		GetQuestionValue.put("Q274", 0);
		GetQuestionValue.put("Q275", 1);
		GetQuestionValue.put("Q276", 1);
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
				//result.getName() will return name of test case so that screenshot name will be same
				File file = new File("./Screenshots/" + result.getName() + ".png");
				FileUtils.copyFile(source, file);
				Throwable errorDetail = result.getThrowable();
				System.out.println("Screenshot taken");
			} catch (Exception e) {
				System.out.println("Exception while taking screenshot " + e.getMessage());
			}
		}

		// close application
		//d.quit();
	}

}
