
import java.io.IOException;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.Calendar;
import java.util.List;
import java.util.concurrent.TimeUnit;
 
import org.openqa.selenium.Alert;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebDriverException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.interactions.Actions;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.Select;
import org.openqa.selenium.support.ui.WebDriverWait;
import org.testng.Assert;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.AfterSuite;
import org.testng.annotations.BeforeMethod;
import org.testng.annotations.BeforeSuite;
import org.testng.annotations.DataProvider;
import org.testng.annotations.Test;
 

public class Meschinowellness_DAFoodTracker_DDT {
 
    WebDriver d = null;
    public static boolean keepAlive = true;
    public static long purgeInterval = 10; // in milliseconds
    public static long implicitlyWait = 20; // in seconds
    public static String driverPath = "C:\\chromedriver_win32\\";
    //public static String driverPath = "C:\\geaco\\";
    private String _UserName = "M1HRAtest@gmail.com"; 
    private String _Password = "Test123@";
    
    private String SiteUrl = "http://demo-meschinowellness.azurewebsites.net/Account/Login";
    
    // Selenium-TestNG Suite Initialization
    @BeforeSuite
    public void suiteSetup() {
        System.out.println("suiteSetup");
        System.setProperty("webdriver.chrome.driver", driverPath + "chromedriver.exe");
        d = new  ChromeDriver();
        d.manage().window().maximize();
        d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
        System.out.println("Suite Setup");
        startMonitor();
    }
 

    // Selenium-TestNG Suite cleanup
    @AfterSuite
    public void suiteTeardown() {
        System.out.println("suiteTeardown");
        d.close();
        //d.quit();
    }
 

    @BeforeMethod
    public void beforeTest() throws InterruptedException {
        System.out.println("Open Browser");
        Thread.sleep(1000);
        d.manage().deleteAllCookies();
        d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
        d.get(SiteUrl);
        System.out.println("Exit from openBrowser()");
        System.out.println("beforeTest");
    }
 

    @AfterMethod
    public void afterTest() {
        // Intentionally left blank.
        System.out.println("afterTest");
    }
 

    // Selenium-TestNG Execution Engine
    @Test(dataProvider = "TestDataProvider")
    public void MeschinoWellness(String UserName, String Password, String TFSTaskId, String Action,
            String Serving, String DateType, String DateIncrement, String References)
            throws Exception {
        
        System.out.println("Test data Provider");
        System.out.println("UserName : " + UserName);
        System.out.println("Password  : " + Password);
     // Enter Username
        d.findElement(By.id("UserName")).click();
        Thread.sleep(1000);
        d.findElement(By.id("UserName")).sendKeys(UserName);
        // Enter password
        d.findElement(By.id("Password")).click();
        d.findElement(By.id("Password")).sendKeys(Password);
        // Click on login button
        d.findElement(By.id("btnLogin")).click();
        d.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
        Thread.sleep(2000);
        // Verfiy Dashboard
        if (d.findElement(By.id("dashboard")) != null) {
            System.out.println("Dashboard Title is verified");
        } else {
            System.out.println("Dashboard Title is not Present");
        }
     
        
        Thread.sleep(3000);
        System.out.println("User login successful");
        ClosePopupContent("chklistModelVideoPopup","close");
        Thread.sleep(4000);
        
        System.out.println("Action : " + Action + " Task Id : " + TFSTaskId + " Serving : " + Serving + " DateType : " + DateType + " DateIncrement : " + DateIncrement);
        
        WebElement DietActivityLink =  d.findElement(By.cssSelector("a[href*='/DietActivity/FoodDiary']"));
        DietActivityLink.click();
        System.out.println("Varified page : Diet & Activity manager loaded");
        Thread.sleep(4000);
        
        if(Action.equals("AddFood"))
        {
            CasesDAF(Integer.parseInt(Serving), Integer.parseInt(DateIncrement)); // TFS 4825, No of servings = 2,  for future date
            //Thread.sleep(4000);
            
            //System.out.println("Back to page : Diet & Activity manager loaded");
            //CasesDAF(1, 0);//TFS 4786 No of servings = 1, for current date
            //Thread.sleep(6000);
        
            WebElement backButton = d.findElement(By.cssSelector("a[href*='/MyWellnessWallet']"));
            backButton.click();
            Thread.sleep(2000);
        }
        
        else if(Action.equals("RemoveFood"))
        {
            WebElement FoodListTbl = d.findElement(By.id("breakfast"));
            List<WebElement> tr_collection = FoodListTbl.findElements(By.xpath("//tbody/tr"));//By.cssSelector("td[class='/MyWellnessWallet']");
            System.out.println("NUMBER OF ROWS = " +tr_collection.size());
            
            WebElement FirstRowElement = tr_collection.get(0);
            Thread.sleep(1000);
            WebElement Delbtn = FirstRowElement.findElement(By.linkText("Delete"));
            Delbtn.click();
            System.out.println("Delete first item : " +Delbtn.getText());
        }
        System.out.println("References : " + References);
        Thread.sleep(8000);
        
    }
 

    // Selenium-TestNG Data Provider
    @DataProvider(name = "TestDataProvider")
    public Object[][] datasupplier() throws Exception {
        final String xlsxFile = System.getProperty("user.dir") + "\\Resources\\DietActivityFood.xlsx";
        Object[][] arrayObject = DataManager.getExcelData(xlsxFile, "Sheet1", 8);
        System.out.println("Array Object :" + arrayObject.length);
        return arrayObject;
    }
    // START File Functions

    public void CasesDAF(int servings, int dateIncrement) throws Exception
    {
        //DietActivity/AddFood
        WebElement DietActivityAddFoodLink =  d.findElement(By.cssSelector("a[href*='/DietActivity/AddFood']"));
        DietActivityAddFoodLink.click();
        System.out.println("Varified page : Diet & Activity Add Food loaded");
        Thread.sleep(4000);
        
        WebElement FoodListTbl = d.findElement(By.id("tbllist"));
        List<WebElement> tr_collection = FoodListTbl.findElements(By.xpath("id('tbllist')/tbody/tr"));
        Thread.sleep(4000);
        
        WebElement ahover = tr_collection.get(0).findElement(By.className("dropdown-toggle"));
        ahover.click();
        Thread.sleep(4000);
        WebElement formdropdown = tr_collection.get(0).findElement(By.className("dropdown-menu"));
        System.out.println("Form Drop down : Found");
        List<WebElement> Inputs = formdropdown.findElements(By.tagName("input"));
        System.out.println("NUMBER OF Input = " +Inputs.size());
         
        ((JavascriptExecutor)d).executeScript("arguments[0].setAttribute('value', '"+servings+"');", Inputs.get(0));
        System.out.println("No. Servings : " + servings );

        LocalDate Date = LocalDate.now().plusDays(dateIncrement);
        DateTimeFormatter formatters = DateTimeFormatter.ofPattern("MM/d/uuuu");
        String datestring = Date.format(formatters);
        Thread.sleep(4000);
        System.out.println("Future Date: " + datestring);
        ((JavascriptExecutor)d).executeScript("arguments[0].setAttribute('value', '"+datestring+"');", Inputs.get(1));
        Thread.sleep(6000);
        WebElement addbtn =  formdropdown.findElement(By.linkText("Save"));
        Actions select = new Actions(d);
        select.clickAndHold(addbtn).build().perform();
        addbtn.click();
       
        if(Inputs.get(1).getText() == ""){
            System.out.println("Verified that, system does not accept the future date");
        }
        else
        {
            System.out.println("System accept for current date");
            System.out.println("Verified that, system successfully adds the food item under Breakfast and food value are also added in the total.");
        }
        Thread.sleep(4000);
    }
    
    //Method to click  Close Popup
    public void ClosePopupContent(String ElementId, String className){
        try
        {
            Thread.sleep(2000);
            WebElement popupElement = d.findElement(By.id(ElementId));
            WebElement element =  popupElement.findElement(By.className(className));//.click();
            ((JavascriptExecutor)d).executeScript("arguments[0].click();", element);
        }
        catch(Exception ex)
        {
            System.out.println("Eror on close button: " + ex.getMessage());
        }
        System.out.println("Popup Closed Clicked " + className);    

    }
    // END File Functions

    // Utility Functions
 
    // Method to remove alerts from the web page
    public void purgeAllAlerts() {
        //System.out.println("Purge hits");
        try {
            Thread.sleep(purgeInterval);
            Alert alert = d.switchTo().alert();
            if (alert != null)
                alert.dismiss();
        } catch (Exception ex) {
            //System.out.println("Exception : " + ex.getMessage());
        }
    }
 

    
    // Method to start background thread for removing alerts
    public void startMonitor() {
        System.out.println("enter into AlertMonitor().");
        keepAlive = true;
        Thread t = new Thread(new Runnable() {
            public void run() {
                for (;;) {
                    purgeAllAlerts();
                    if (!keepAlive)
                        break;
                }
                System.out.println("exit from AlertMonitor() thread.");
            }
        });
        t.start();
        System.out.println("Exit from AlertMonitor().");
    }
 
    // Method to stop alert monitor thread
    public void stopMonitor() {
        keepAlive = false;
    }
    
    // Method to wait for the next page to load
    public void waitForPageLoaded() {
        int maxWait = 30;
        long tStart = System.currentTimeMillis();
        long elapsedSeconds = 0;
        for (; elapsedSeconds < maxWait;) {
            try {
                purgeAllAlerts();
                d.manage().timeouts().implicitlyWait(implicitlyWait, TimeUnit.SECONDS);
 
                if (d.getCurrentUrl().contains("ErrorCode=1052")) {
                    // Error page, exit immediately instead of keep on waiting.
                    System.out.println("\nErrorCode=1052");
                    return;
                }
                d.findElement(By.cssSelector("p.ng-binding"));
                System.out.println("\nNew page successfully loaded");
                return;
            } catch (WebDriverException ex) {
                // Intentionally left blank.
                //System.out.println("Exception : " + ex.getMessage());
            }
            elapsedSeconds = (System.currentTimeMillis() - tStart) / 1000;
        }
        System.out.println("\n waitForPageLoaded(): elapsedSeconds= " + elapsedSeconds);
    }
}





























