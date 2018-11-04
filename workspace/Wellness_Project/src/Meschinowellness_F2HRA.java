
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
import org.openqa.selenium.OutputType;
import org.openqa.selenium.TakesScreenshot;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.support.ui.Select;
import org.testng.Assert;
import org.testng.ITestResult;
//import org.testng.annotations.AfterMethod;

import org.testng.annotations.AfterMethod;
//import org.testng.annotations.BeforeTest;
//import org.testng.annotations.Test;

public class Meschinowellness_F2HRA {
	String driverPath = "C:\\geaco\\";
	
		WebDriver d;

		@Before
		public void Setup() throws Exception {
			// Launch browser
			System.out.println("launching firefox browser"); 
			System.setProperty("webdriver.gecko.driver", driverPath+"geckodriver.exe");
			
			d = new FirefoxDriver();
			d.manage().window().maximize();
			d.manage().deleteAllCookies();
			d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
			d.get("http://demo-meschinowellness.azurewebsites.net/Account/Login");
			d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
		}
		@Test//(priority = 1)
		public void login() throws Exception {
			// Enter Username
			d.findElement(By.id("UserName")).click();
			Thread.sleep(1000);
			d.findElement(By.id("UserName")).sendKeys("F2HRAtest@gmail.com");
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
		//}
		//@Test(priority = 2)
		//public void MyHraTab() throws Exception {
			// Click on my hra tab
			d.findElement(By.id("myHra")).click();
			// Verfiy myhra tilte
			if (d.findElement(By.id("myHra")) != null) {
				System.out.println("myHra  Title  is verified");
			} else {
				System.out.println("myHra  Title  is not  Present");
			}
			//
			WebElement btnRestart = d.findElement(By.id("btnRestart"));
			//click on restart button if btn is displayed
			if (btnRestart.isDisplayed()) {
				btnRestart.click();
				System.out.println("btnRestart displayed");
			}
			// Click on "weight" drop down
			d.findElement(By.id("DDLQ277")).click();
			Thread.sleep(1000);
			// select 120 weight
			Select dropdown = new Select(d.findElement(By.id("DDLQ277")));
			dropdown.selectByVisibleText("120");
			Thread.sleep(5000);
			// Click on "waist" drop down
			d.findElement(By.id("DDLQ278")).click();
			Thread.sleep(1000);
			// select 30 waist
			Select dropdown1 = new Select(d.findElement(By.id("DDLQ278")));
			dropdown1.selectByVisibleText("30");
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
					if(local_radio.isDisplayed())
					{
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
				System.out.println("F1HRA has been sucessfully completed");
		}
	    //Method to click  the option and scroll the content
		public void GetElement(WebElement element) {
			JavascriptExecutor je = (JavascriptExecutor) d;
			je.executeScript("arguments[0].click();", element);
		}
		public static Map<String, Integer> GetQuestionValue = new HashMap<String, Integer>();
		{
			GetQuestionValue.put("Q1", 3);
            GetQuestionValue.put("Q2", 0);
            GetQuestionValue.put("Q3", 2);
            GetQuestionValue.put("Q4", 1);
            GetQuestionValue.put("Q5", 1);
            GetQuestionValue.put("Q6", 1);
            GetQuestionValue.put("Q7", 3);
            GetQuestionValue.put("Q8", 3);
            GetQuestionValue.put("Q9", 0);
            GetQuestionValue.put("Q10", 0);
            GetQuestionValue.put("Q11", 2);
            GetQuestionValue.put("Q12", 3);
            GetQuestionValue.put("Q13", 3);
            GetQuestionValue.put("Q14", 2);
            GetQuestionValue.put("Q15", 1);
            GetQuestionValue.put("Q16", 3);
            GetQuestionValue.put("Q17", 2);
            GetQuestionValue.put("Q18", 0);
            GetQuestionValue.put("Q19", 0);
            GetQuestionValue.put("Q20", 0);
            GetQuestionValue.put("Q21", 2);
            GetQuestionValue.put("Q22", 0);
            // NextClick("Loaded - Step3 : Exercise Participation");
            GetQuestionValue.put("Q23", 1);
            GetQuestionValue.put("Q24", 1);
            GetQuestionValue.put("Q25", 0);
            GetQuestionValue.put("Q26", 1);
            GetQuestionValue.put("Q27", 1);
            GetQuestionValue.put("Q28", 4);
            GetQuestionValue.put("Q29", 4);
            GetQuestionValue.put("Q30", 4);
            // NextClick("Loaded - Step4 : Nutrient Deficiency");// btn next click;
            GetQuestionValue.put("Q31", 1);
            GetQuestionValue.put("Q32", 1);
            GetQuestionValue.put("Q33", 1);
            GetQuestionValue.put("Q34", 1);
            GetQuestionValue.put("Q35", 1);
            GetQuestionValue.put("Q36", 1);
            GetQuestionValue.put("Q37", 1);
            //GetQuestionValue.put("Q38", 2);// not req
            //GetQuestionValue.put("Q39", 2);// not req
            //GetQuestionValue.put("Q40", 0);// not req
            GetQuestionValue.put("Q41",4);// not req
            GetQuestionValue.put("Q42",4);// not req
            GetQuestionValue.put("Q43",1);// not req
            GetQuestionValue.put("Q44", 0);
            GetQuestionValue.put("Q45", 0);
            GetQuestionValue.put("Q46", 0);
            GetQuestionValue.put("Q47", 0);
            GetQuestionValue.put("Q48", 0);
            GetQuestionValue.put("Q49", 0);
            GetQuestionValue.put("Q50", 0);
            GetQuestionValue.put("Q51", 0);
            GetQuestionValue.put("Q52", 0);
            GetQuestionValue.put("Q53", 0);
            GetQuestionValue.put("Q54", 0);
            GetQuestionValue.put("Q55", 0);
            GetQuestionValue.put("Q56", 1);
            GetQuestionValue.put("Q57", 0);
            GetQuestionValue.put("Q58", 1);
            GetQuestionValue.put("Q59", 0);//
            GetQuestionValue.put("Q60", 1);
            GetQuestionValue.put("Q61", 1);
            GetQuestionValue.put("Q62", 1);
            GetQuestionValue.put("Q63", 1);
            GetQuestionValue.put("Q64", 1);
            GetQuestionValue.put("Q65",1);
            GetQuestionValue.put("Q66",1);
            GetQuestionValue.put("Q67", 0);
            GetQuestionValue.put("Q68", 1);
            GetQuestionValue.put("Q69", 1);
            GetQuestionValue.put("Q70", 0);
            GetQuestionValue.put("Q71", 0);
            GetQuestionValue.put("Q72", 0);
            GetQuestionValue.put("Q73", 0);
            GetQuestionValue.put("Q74", 0);
            GetQuestionValue.put("Q75", 0);
            GetQuestionValue.put("Q76", 1);
            GetQuestionValue.put("Q77", 0);
            GetQuestionValue.put("Q78", 0);
            // NextClick("Loaded - Step5 : Cardiovascular");
            GetQuestionValue.put("Q79", 1);
             GetQuestionValue.put("Q80",0);
             GetQuestionValue.put("Q81",1);
            GetQuestionValue.put("Q82", 4);
            GetQuestionValue.put("Q83", 0);
            GetQuestionValue.put("Q84", 2);
            // GetQuestionValue.put("Q85",0);
            GetQuestionValue.put("Q86", 0);
            GetQuestionValue.put("Q87", 0);
            GetQuestionValue.put("Q88", 0);
            GetQuestionValue.put("Q89", 0);
            GetQuestionValue.put("Q90", 0);
            // GetQuestionValue.put("Q91",0);
            GetQuestionValue.put("Q92", 0);
            GetQuestionValue.put("Q93", 1);
            // NextClick("Loaded - Step6 : Cancer");
            //GetQuestionValue.put("Q94", 1);
            GetQuestionValue.put("Q95",1);
            GetQuestionValue.put("Q96", 0);
            GetQuestionValue.put("Q97", 1);
            GetQuestionValue.put("Q98", 0);
            GetQuestionValue.put("Q99", 1);
            // NextClick("Loaded - Step7 : Gastrointestinal");//Personal Health History: Gastrointestinal Health
            GetQuestionValue.put("Q100", 1);
            GetQuestionValue.put("Q101", 1);
            GetQuestionValue.put("Q102", 1);
            GetQuestionValue.put("Q103", 1);
            GetQuestionValue.put("Q104", 1);
            GetQuestionValue.put("Q105", 1);
            GetQuestionValue.put("Q106", 1);
            GetQuestionValue.put("Q107", 1);
            GetQuestionValue.put("Q108", 1);
            // NextClick("Loaded - Step8 : Skin Conditions"); // Personal Health History: Skin Conditions   
            GetQuestionValue.put("Q109", 0);
            GetQuestionValue.put("Q110", 0);
            GetQuestionValue.put("Q111", 0);
            GetQuestionValue.put("Q112", 0);
            GetQuestionValue.put("Q113", 0);
            GetQuestionValue.put("Q114", 0);
            GetQuestionValue.put("Q115", 0);
            GetQuestionValue.put("Q116", 0);
            GetQuestionValue.put("Q117", 0);
            GetQuestionValue.put("Q118", 1);
            GetQuestionValue.put("Q119", 0);
            // Systemic Health
            GetQuestionValue.put("Q120", 1);
            GetQuestionValue.put("Q121",1);
            //GetQuestionValue.put("Q122", 1);
            GetQuestionValue.put("Q123", 0);
            GetQuestionValue.put("Q124", 1);
            GetQuestionValue.put("Q125", 1);
            // GetQuestionValue.put("Q126",0);
            GetQuestionValue.put("Q127", 1);
            GetQuestionValue.put("Q128", 1);
            GetQuestionValue.put("Q129", 0);
            GetQuestionValue.put("Q130", 0);
            GetQuestionValue.put("Q131", 0);
            GetQuestionValue.put("Q132", 0);
            GetQuestionValue.put("Q133", 0);
            GetQuestionValue.put("Q134", 0);
            GetQuestionValue.put("Q135", 0);
            GetQuestionValue.put("Q136", 0);
            GetQuestionValue.put("Q137", 0);
            GetQuestionValue.put("Q138", 0);
            GetQuestionValue.put("Q139", 0);
            GetQuestionValue.put("Q140", 0);
            GetQuestionValue.put("Q141", 0);
            GetQuestionValue.put("Q142", 0);
            GetQuestionValue.put("Q143", 0);
            GetQuestionValue.put("Q144", 0);
            GetQuestionValue.put("Q145", 0);
            GetQuestionValue.put("Q146", 0);
            GetQuestionValue.put("Q147", 0);
            GetQuestionValue.put("Q148", 0);
            GetQuestionValue.put("Q149", 0);
            GetQuestionValue.put("Q150", 0);
            GetQuestionValue.put("Q151", 0);
            GetQuestionValue.put("Q152", 0);
            GetQuestionValue.put("Q153", 0);
            GetQuestionValue.put("Q154", 0);
            GetQuestionValue.put("Q155", 0);
            GetQuestionValue.put("Q156", 0);
            GetQuestionValue.put("Q157", 0);
            GetQuestionValue.put("Q158", 0);
            GetQuestionValue.put("Q159", 0);
            GetQuestionValue.put("Q160", 0);
            GetQuestionValue.put("Q161", 0);
            GetQuestionValue.put("Q162", 0);
            GetQuestionValue.put("Q163", 0);
            GetQuestionValue.put("Q164", 0);
            GetQuestionValue.put("Q165", 0);
            GetQuestionValue.put("Q166", 0);
            GetQuestionValue.put("Q167", 0);
            GetQuestionValue.put("Q168", 0);
            GetQuestionValue.put("Q169", 0);
            GetQuestionValue.put("Q170", 0);
            //Men's Health
            //GetQuestionValue.put("Q171", 1);
            //GetQuestionValue.put("Q172", 1);
            //GetQuestionValue.put("Q173", 1);
            //GetQuestionValue.put("Q174", 1);
            //GetQuestionValue.put("Q175", 1);
            //GetQuestionValue.put("Q176", 1);
            //GetQuestionValue.put("Q177", 1);
            //GetQuestionValue.put("Q178", 1);
            //GetQuestionValue.put("Q179", 1);
            //GetQuestionValue.put("Q180", 1);
            //GetQuestionValue.put("Q181", 1);
            //GetQuestionValue.put("Q182", 1);
            // for females
            GetQuestionValue.put("Q183",1);
            GetQuestionValue.put("Q184",1);
            GetQuestionValue.put("Q185",1);
            GetQuestionValue.put("Q186",1);
            GetQuestionValue.put("Q187",1);
            GetQuestionValue.put("Q188",1);
            GetQuestionValue.put("Q189",1);
            GetQuestionValue.put("Q190",1);
            GetQuestionValue.put("Q191",1);
            GetQuestionValue.put("Q192",1);
            GetQuestionValue.put("Q193",0);
            GetQuestionValue.put("Q194",1);
            GetQuestionValue.put("Q195",1);
            GetQuestionValue.put("Q196",1);
            GetQuestionValue.put("Q197",1);
            GetQuestionValue.put("Q198",1);
            GetQuestionValue.put("Q199",1);
            GetQuestionValue.put("Q200",1);
            GetQuestionValue.put("Q201",1);
            GetQuestionValue.put("Q202",1);
            GetQuestionValue.put("Q203",1);
            //Stress
            GetQuestionValue.put("Q204", 0);
            GetQuestionValue.put("Q205", 0);
            // GetQuestionValue.put("Q206",0);
            // GetQuestionValue.put("Q207",3);
            // GetQuestionValue.put("Q208",2);
            GetQuestionValue.put("Q209", 3);
            // GetQuestionValue.put("Q210",0);
            // GetQuestionValue.put("Q211",3);
            // GetQuestionValue.put("Q212",2);
            GetQuestionValue.put("Q213", 1);
            GetQuestionValue.put("Q214", 1);
            GetQuestionValue.put("Q215", 1);
            GetQuestionValue.put("Q216", 1);
            GetQuestionValue.put("Q217", 1);
            GetQuestionValue.put("Q218", 1);
            GetQuestionValue.put("Q219", 0);
            //Medication Adherence
            GetQuestionValue.put("Q220", 4);
            // GetQuestionValue.put("Q221",3);
            GetQuestionValue.put("Q222", 1);
            GetQuestionValue.put("Q223", 1);
            GetQuestionValue.put("Q224", 1);
            GetQuestionValue.put("Q225", 0);
            GetQuestionValue.put("Q226", 1);
            GetQuestionValue.put("Q227", 1);
            GetQuestionValue.put("Q228", 0);
            GetQuestionValue.put("Q229", 0);
            //Personal Health History: General Considerations
            GetQuestionValue.put("Q230", 3);
            GetQuestionValue.put("Q231", 0);
            // GetQuestionValue.put("Q232",2);
            GetQuestionValue.put("Q233", 0);
            GetQuestionValue.put("Q279", 0);
            // GetQuestionValue.put("Q234",2);
            // GetQuestionValue.put("Q235",2);

            //Family Health
            GetQuestionValue.put("Q236", 1);
            GetQuestionValue.put("Q237", 0);
            GetQuestionValue.put("Q238",0);
            GetQuestionValue.put("Q239", 1);
            GetQuestionValue.put("Q240", 1);
            GetQuestionValue.put("Q241", 1);
            //Additional Screening
            GetQuestionValue.put("Q242", 1);
            //GetQuestionValue.put("Q243",1);
            GetQuestionValue.put("Q244", 1);
            GetQuestionValue.put("Q245", 1);
            GetQuestionValue.put("Q246", 1);
            GetQuestionValue.put("Q247", 1);
            GetQuestionValue.put("Q248", 1);
            GetQuestionValue.put("Q249", 1);
            GetQuestionValue.put("Q250", 1);
            GetQuestionValue.put("Q251", 1);
            GetQuestionValue.put("Q252", 1);
            GetQuestionValue.put("Q253", 1);
            GetQuestionValue.put("Q254", 0);
            GetQuestionValue.put("Q255", 0);
            GetQuestionValue.put("Q256", 0);//
            GetQuestionValue.put("Q257", 0);
            GetQuestionValue.put("Q258", 0);
            GetQuestionValue.put("Q259", 0);
            GetQuestionValue.put("Q260", 0);
            GetQuestionValue.put("Q261", 0);
            GetQuestionValue.put("Q262", 0);
            GetQuestionValue.put("Q263", 0);
            GetQuestionValue.put("Q264", 0);
            GetQuestionValue.put("Q265", 0);
            GetQuestionValue.put("Q266", 0);
            GetQuestionValue.put("Q267", 0);
            GetQuestionValue.put("Q268", 0);
            GetQuestionValue.put("Q269", 0);
            GetQuestionValue.put("Q270", 0);
            GetQuestionValue.put("Q271", 0);
            GetQuestionValue.put("Q272", 0);
            GetQuestionValue.put("Q273", 0);
            GetQuestionValue.put("Q274", 1);
            GetQuestionValue.put("Q275", 0);
            GetQuestionValue.put("Q276", 0);
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


