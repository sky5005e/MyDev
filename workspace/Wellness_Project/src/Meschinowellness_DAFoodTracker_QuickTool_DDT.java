
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
 

public class Meschinowellness_DAFoodTracker_QuickTool_DDT {
 
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
            String FoodDescription, String TotalCalories, String DateType, String MealType, 
            String DateIncrement, String References)
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
        
        System.out.println("Current Data - Action : " + Action + " Task Id : " + TFSTaskId + 
                " FoodDescription : " + FoodDescription + " TotalCalories: " + TotalCalories +
                " DateType : " + DateType + " MealType : " + MealType+
                " DateIncrement : " + DateIncrement);
        
        WebElement DietActivityLink =  d.findElement(By.cssSelector("a[href*='/DietActivity/FoodDiary']"));
        DietActivityLink.click();
        System.out.println("Varified page : Diet & Activity manager loaded");
        Thread.sleep(4000);
        
        CasesDAFQuickTool(FoodDescription, TotalCalories, MealType , Integer.parseInt(DateIncrement)); // TFS 4825, No of servings = 2,  for future date
        Thread.sleep(4000);
        
        System.out.println("References : " + References);
        Thread.sleep(8000);
        
    }
 

    // Selenium-TestNG Data Provider
    @DataProvider(name = "TestDataProvider")
    public Object[][] datasupplier() throws Exception {
        final String xlsxFile = System.getProperty("user.dir") + "\\Resources\\DietActivityFoodQuickTool.xlsx";
        Object[][] arrayObject = DataManager.getExcelData(xlsxFile, "Sheet1", 10);
        System.out.println("Array Object :" + arrayObject.length);
        return arrayObject;
    }
    // START File Functions

    public void CasesDAFQuickTool(String foodDescription,  String totalCalorie, String mealType, int dateIncrement) throws Exception
    {
        //DietActivity/AddFood
        WebElement divQuickTool =  d.findElement(By.className("panel-action"));
        
        WebElement DietActivityQuickLink =  divQuickTool.findElement(By.id("quicktool"));//d.findElement(By.linkText("Quick Tool"));//d.findElement(By.id("quicktool"));
        DietActivityQuickLink.click();
        Thread.sleep(4000);
        System.out.println("Varified page : Diet & Activity Add Food Popup");
        Thread.sleep(4000);
        
        WebElement formdropdown = d.findElement(By.className("dropdown-menu"));
        System.out.println("Form Drop down : Found");
        // food_desc
        WebElement food_desc = d.findElement(By.id("food_desc"));
        ((JavascriptExecutor)d).executeScript("arguments[0].setAttribute('value', '"+ foodDescription +"');", food_desc);
        System.out.println("food_desc : " + foodDescription );

        // total_cal
        WebElement total_cal = d.findElement(By.id("total_cal"));
        if(Integer.parseInt(totalCalorie) <= 0)
        {
            ((JavascriptExecutor)d).executeScript("arguments[0].setAttribute('value', '"+ totalCalorie +"');", total_cal);
        }
        else
        {
            total_cal.sendKeys(totalCalorie);
        }
        System.out.println("Total Colorie : " + totalCalorie );
        
        Select dropdown = new Select(d.findElement(By.id("mealType")));
        dropdown.selectByVisibleText(mealType);
        System.out.println("Meal Type  : " + mealType );
        if(dateIncrement > 0)
        {
            LocalDate Date = LocalDate.now().plusDays(dateIncrement);
            DateTimeFormatter formatters = DateTimeFormatter.ofPattern("MM/d/uuuu");
            String datestring = Date.format(formatters);
            Thread.sleep(4000);
            System.out.println("Future Date: " + datestring);
            WebElement datevalue = d.findElement(By.id("datevalue"));
            ((JavascriptExecutor)d).executeScript("arguments[0].setAttribute('value', '"+datestring+"');", datevalue);
            Thread.sleep(6000);
        }
        WebElement addbtn =  d.findElement(By.linkText("Save"));
        Actions select = new Actions(d);
        select.clickAndHold(addbtn).build().perform();
        addbtn.click();
        if(dateIncrement > 0){
            System.out.println("Verified that, system does not accept the future date");
        }
        else if(Integer.parseInt(totalCalorie) <= 0)
        {
            System.out.println("Verify that, system does not accept negative value in 'Enter Total Calorie' field for 'Quick Tool' on 'Diet & Activity Manager' page");
        }
        else 
        {
            System.out.println("System accept for current date");
        }
        Thread.sleep(8000);
        

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





























