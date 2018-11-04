import java.io.File;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

import org.apache.commons.io.FileUtils;
import org.apache.commons.lang3.ObjectUtils.Null;
import org.junit.Before;
import org.junit.Test;
//import org.testng.annotations.Test;

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
import org.openqa.selenium.support.ui.ExpectedCondition;
import org.openqa.selenium.support.ui.Select;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;

import com.google.common.base.Function;
import org.testng.annotations.BeforeTest;
import org.openqa.selenium.interactions.Actions;

public class Meschinowellness_M1WeightTracker {

String driverPath = "C:\\chromedriver_win32\\";
private String UserName = "M1HRAtest@gmail.com"; 
private String Password = "Test123@";
private String Url = "http://demo-meschinowellness.azurewebsites.net/Account/Login";
//"http://stage-meschinowellness.azurewebsites.net/Account/Login"
WebDriver d;
private CharSequence click;
int weightValue = 0;
@Before//Test
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
    //}

    //@Test(priority = 2)
    //public void MyWellnessPlan() throws Exception {
            // Click Trackers & Resources
            d.findElement(By.id("toolsAndResources")).click();
            // Verify Trackers & Resources
            if (d.findElement(By.id("toolsAndResources")) != null) {
                System.out.println("Trackers & Resources is verified");
            } else {
                System.out.println("Trackers & Resources is not Present");
            }
            // Weight Tracker
            System.out.println("Weight Tracker");
            WebElement WTLink = d.findElement(By.linkText("Weight Tracker"));
                    //By.xpath("//a[contains(text(),'Weight Tracker')]"));
            ((JavascriptExecutor)d).executeScript("arguments[0].click();", WTLink);
            
            WebElement date =  d.findElement(By.id("Date"));
            ((JavascriptExecutor)d).executeScript("arguments[0].click();", date);
            LocalDate today = LocalDate.now();
            DateTimeFormatter formatters = DateTimeFormatter.ofPattern("MM/d/uuuu");
            String datestring = today.format(formatters);
            
            Thread.sleep(4000);
            System.out.println("Today: " + datestring);
            ((JavascriptExecutor)d).executeScript("arguments[0].setAttribute('value', '"+datestring+"');", date);
            // Weight Tracker
            System.out.println("enter value Tracker");
            WebElement Weight =  d.findElement(By.id("Weight"));
            Thread.sleep(2000);
            weightValue = 120; // set 120 pounds to the 
            ((JavascriptExecutor)d).executeScript("arguments[0].setAttribute('value', '"+120+"');", Weight); // 120 pounds
            Select dropdown1 = new Select(d.findElement(By.id("UOM_NUM")));
            dropdown1.selectByVisibleText("Pounds");
            Thread.sleep(2000);
            
            // Weight Tracker
            System.out.println("Save Button Clicked");
            
            WebElement frmWeightTracker = d.findElement(By.id("frmWeightTracker"));
            WebElement SaveButton = frmWeightTracker.findElement(By.className("btn-default"));
           // SaveButton.click();
            ((JavascriptExecutor)d).executeScript("arguments[0].click();", SaveButton);
            CloseOk();
            ClosePopupContent("weight-tracker-popup");
            Thread.sleep(2000);
            
            
           // profile
            WebElement ahover = d.findElement(By.className("dropdown-toggle"));
            Actions select = new Actions(d);
            select.clickAndHold(ahover).build().perform();
            Thread.sleep(4000);
            WebElement profile =  d.findElement(By.cssSelector("a[href*='/Account/ManageProfile']"));
            ((JavascriptExecutor)d).executeScript("arguments[0].click();", profile);
            //tbmeasurements
            WebElement table_element = d.findElement(By.id("tbmeasurements"));
            List<WebElement> tr_collection = table_element.findElements(By.xpath("id('tbmeasurements')/tbody/tr"));
            System.out.println("NUMBER OF ROWS IN THIS TABLE = " +tr_collection.size());
            Thread.sleep(4000);
            
            WebElement CurrentRowElement = tr_collection.get(1);
            for(int i=1; i<=2; i++)
            {
                Select dropdown = new Select(CurrentRowElement.findElement(By.className("cms-dropdownlist")));
                if(i == 1)
                {
                    System.out.println("set drop down : Kilograms");
                    dropdown.selectByValue("6");//("Kilograms");
                }
                else
                {
                    System.out.println("set drop down : Pounds");
                    dropdown.selectByValue("16");//("Pounds");
                }
                GetRowData(CurrentRowElement);
            }
            WebElement backButton = d.findElement(By.cssSelector("a[href*='/MyWellnessWallet']"));
            backButton.click();
            
          //}

            //@Test(priority = 3)
            //public void CalorieCalculatorTab() throws Exception {
                    // Click Trackers & Resources
                Thread.sleep(4000);
                d.findElement(By.id("toolsAndResources")).click();
                    // Verify Trackers & Resources
                    if (d.findElement(By.id("toolsAndResources")) != null) {
                        System.out.println("Trackers & Resources is verified");
                    } else {
                        System.out.println("Trackers & Resources is not Present");
                    }
                    // Weight Tracker
                    System.out.println("Weight Tracker");
                    Thread.sleep(4000);
                    WebElement CCalLink = d.findElement(By.linkText("Calorie Calculator"));
                    ((JavascriptExecutor)d).executeScript("arguments[0].click();", CCalLink);
                    Thread.sleep(4000);
                    
                    WebElement SetGoalWeightDialog = d.findElement(By.id("SetGoalWeightDialog"));
                    Thread.sleep(2000);
                    WebElement label = SetGoalWeightDialog.findElement(By.xpath("//label[contains(text(),'Current Weight')]"));
                    System.out.println("Current weight : " + label.getText());
                    if(label.getText().contains(Integer.toString(weightValue)))
                    {
                        System.out.println("Recent added Current weight matched : " + weightValue);
                    }
                    else
                    {
                        System.out.println("Recent added Current weight not matched : " + weightValue);
                    }
                   
        }

        public void GetRowData(WebElement CurrentRowElement) throws Exception
        {
            List<WebElement> td_collection = CurrentRowElement.findElements(By.xpath("td"));
            System.out.println("NUMBER OF COLUMNS="+td_collection.size());
            int col_num = 1;
            for(WebElement tdElement : td_collection)
            {
                Thread.sleep(2000);
                System.out.println("col # " +col_num+ "text = "+tdElement.getText());
                if(col_num == 2 && tdElement.getText() == Integer.toString(weightValue))
                {
                    System.out.println("Weight matched on drop down change : " + weightValue);
                }
                col_num++;
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
    public void CloseOk() throws InterruptedException  {
        WebElement OkButton = d.findElement(By.className("btn-primary"));
        System.out.println("Ok Button : " + OkButton.getText());
        Thread.sleep(3000);
        ((JavascriptExecutor)d).executeScript("arguments[0].click();", OkButton);
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

