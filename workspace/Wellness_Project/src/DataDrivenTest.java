
import java.io.IOException;
import java.util.Calendar;
import java.util.List;
import java.util.concurrent.TimeUnit;
 
import org.openqa.selenium.Alert;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebDriverException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.firefox.FirefoxDriver;
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
 

public class DataDrivenTest {
 
    WebDriver driver = null;
    public static boolean keepAlive = true;
    public static long purgeInterval = 10; // in milliseconds
    public static long implicitlyWait = 20; // in seconds
    public static String driverPath = "C:\\geaco\\";
 
    // Selenium-TestNG Suite Initialization
    @BeforeSuite
    public void suiteSetup() {
        System.out.println("suiteSetup");
        System.setProperty("webdriver.gecko.driver", driverPath + "geckodriver.exe");
        DesiredCapabilities caps = new DesiredCapabilities();
        caps.setCapability("marionette", true);
        driver = new FirefoxDriver();
        startMonitor();
    }
 

    // Selenium-TestNG Suite cleanup
    @AfterSuite
    public void suiteTeardown() {
        System.out.println("suiteTeardown");
        driver.close();
        driver.quit();
    }
 

    @BeforeMethod
    public void beforeTest() throws InterruptedException {
        System.out.println("Open Browser");
        driver.get("https://www.google.co.in/?gws_rd=ssl");//http://beta.wakanow.com/flightsv2");
        Thread.sleep(1000);
        driver.manage().window().maximize();
        System.out.println("exit from openBrowser()");
    }
 

    @AfterMethod
    public void afterTest() {
        // Intentionally left blank.
    }
 

    // Selenium-TestNG Execution Engine
    @Test(dataProvider = "FlightBookingData")
    public void flightDeals(String UserName, String Password, String TFSTaskId, String Action,
            String Serving, String DateType, String DateIncrement, String References)
            throws InterruptedException, IOException {
 
        driver.manage().timeouts().implicitlyWait(implicitlyWait, TimeUnit.SECONDS);
		System.out.println("Test data Provider");
        // Click on roundtrip radio button
       // WebElement radioBtn = driver.findElement(By.id("roundtrip"));
        //radioBtn.click();
		System.out.println("Action : " + Action);
        System.out.println("Task Id : " + TFSTaskId);
        System.out.println("Serving : " + Serving);
        System.out.println("DateType : " + DateType);
        System.out.println("DateIncrement : " + DateIncrement);
        
        // Insert data into Flying from text box
        driver.findElement(By.id("lst-ib")).click();
        driver.findElement(By.id("lst-ib")).sendKeys(UserName);
        driver.findElement(By.id("lst-ib")).click();
        Thread.sleep(20000);
 /*
        // Working with Autocomplete text in Flying from text box
        WebElement autoOptions = driver.findElement(By.className("ac_results"));
        List<WebElement> optionsToSelect = autoOptions.findElements(By.tagName("li"));
        for (WebElement option : optionsToSelect) {
            if (option.getText().contains(col2)) {
                System.out.println("Trying to select: ");
                option.click();
                break;
            }
        }
        // Remove duplicate autoOptions object so that Flying to can work
        ((JavascriptExecutor) driver).executeScript("arguments[0].parentNode.removeChild(arguments[0])", autoOptions);
 
        // Insert data into Flying to text box
        driver.findElement(By.id("intlArrvCode")).click();
        driver.findElement(By.id("intlArrvCode")).sendKeys(col3);
        driver.findElement(By.id("intlArrvCode")).click();
        */
        /*
        // Working with Autocomplete text in Flying to text box
        WebElement autoOptions1 = driver.findElement(By.className("ac_results"));
        System.out.println("Selected the Flying to List");
        WebDriverWait wait = new WebDriverWait(driver, 10);
        wait.until(ExpectedConditions.visibilityOf(autoOptions1));
        List<WebElement> optionsToSelect1 = autoOptions1.findElements(By.tagName("li"));
        for (WebElement option : optionsToSelect1) {
            String var = option.getText();
            System.out.println("Trying to select1: " + var);
            if (option.getText().contains(Destination)) {
                System.out.println("Trying to select123: ");
                option.click();
                break;
            }
        }
        // Enter Departing date
        // split date to take out month,year,day in the date
        // String depdate="24-Feb-2016";
        String depdate = departdate;
        String[] temp;
        String delimiter = "-";
        temp = depdate.split(delimiter);
        for (int i = 0; i < temp.length; i++)
            System.out.println(temp[i]);
 
        // Click on textbox so that datepicker will come
        WebElement Cal = driver.findElement(By.id("deptDate"));
        Cal.click();
 
        // Select the month in the calender
        Select oSelect = new Select(driver.findElement(By.className("ui-datepicker-month")));
        oSelect.selectByVisibleText(temp[1]);
 
        // Get the year difference between current year and year to set in
        // calendar
        int yearDiff = Integer.parseInt(temp[2]) - Calendar.getInstance().get(Calendar.YEAR);
 
        if (yearDiff != 0) {
            Select ySelect = new Select(driver.findElement(By.className("ui-datepicker-year")));
            ySelect.selectByVisibleText(temp[2]);
        }
        */
         // DatePicker is a table.So navigate to each cell If a particular cell
         // matches date value then select it
/*
        WebElement dateWidget = driver.findElement(By.id("ui-datepicker-div"));
        List<WebElement> rows = dateWidget.findElements(By.tagName("tr"));
        List<WebElement> columns = dateWidget.findElements(By.tagName("td"));
 
        for (WebElement cell : columns) {
            // Select Date
            if (cell.getText().equals(temp[0])) {
                cell.findElement(By.linkText(temp[0])).click();
                break;
            }
        }
 



        // Enter Arriving date
        // split date to take out month,year,day in the date
        // String arrdate="26-Feb-2016";
        String arrdate = returndate;
        String[] temp1;
        String delimit = "-";
        temp1 = arrdate.split(delimit);
        for (int i = 0; i < temp1.length; i++)
            System.out.println(temp1[i]);
 
        // Click on textbox so that datepicker will come
        WebElement Calen = driver.findElement(By.id("arrvDate"));
        Calen.click();
 
        // Select the month in the calender
        Select mSelect = new Select(driver.findElement(By.className("ui-datepicker-month")));
        mSelect.selectByVisibleText(temp1[1]);
 
        // Get the year difference between current year and year to set in
        // calander
        int yearDif = Integer.parseInt(temp1[2]) - Calendar.getInstance().get(Calendar.YEAR);
 
        if (yearDif != 0) {
            Select ySelect1 = new Select(driver.findElement(By.className("ui-datepicker-year")));
            ySelect1.selectByVisibleText(temp1[2]);
        }
 */
        /*


         * DatePicker is a table.So navigate to each cell If a particular cell
         * matches date value then select it
         */
/*
        WebElement dateWidget1 = driver.findElement(By.id("ui-datepicker-div"));
        List<WebElement> row = dateWidget1.findElements(By.tagName("tr"));
        List<WebElement> column = dateWidget1.findElements(By.tagName("td"));
 
        for (WebElement cell : column) {
            // Select Date
            if (cell.getText().equals(temp1[0])) {
                cell.findElement(By.linkText(temp1[0])).click();
                break;
            }
        }
 



        // Select Ticket class
        Select tktClass = new Select(driver.findElement(By.className("ticketclass")));
        tktClass.selectByVisibleText(ticketClass);
 
        // Select Adults
        Select adult = new Select(driver.findElement(By.id("adults")));
        adult.selectByVisibleText(adults);
 
        // Select Infants
        Select infant = new Select(driver.findElement(By.id("infants")));
        infant.selectByVisibleText(infants);
 
        // Select Children
        Select child = new Select(driver.findElement(By.id("fltchildren")));
        child.selectByVisibleText(children);
 
        // Click on Search button
        driver.findElement(By.id("btnSearch")).click();
 
        waitForPageLoaded();
 
        int iFlightOption = 0;
        try {

            String results = driver.findElement(By.cssSelector("p.ng-binding")).getText();
            iFlightOption = Integer.parseInt(results.split(" ")[0]);
        } catch (Exception ex) {
            iFlightOption = 0;
        }

        System.out.println(" Flight option found: " + iFlightOption);
        Assert.assertTrue(iFlightOption > 0, "No flight option found!");
        */
    }
 

    // Selenium-TestNG Data Provider
    @DataProvider(name = "FlightBookingData")
    public Object[][] datasupplier() throws Exception {
        final String xlsxFile = System.getProperty("user.dir") + "\\Resources\\DietActivityFood.xlsx";
        Object[][] arrayObject = DataManager.getExcelData(xlsxFile, "Sheet1", 8);
		System.out.println("Array Object :" + arrayObject.length);
        return arrayObject;
    }
 

    // Utility Functions
 
    // Method to remove alerts from the web page
    public void purgeAllAlerts() {
        try {
            Thread.sleep(purgeInterval);
            Alert alert = driver.switchTo().alert();
            if (alert != null)
                alert.dismiss();
        } catch (Exception ex) {
            // Intentionally left blank.
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
                driver.manage().timeouts().implicitlyWait(implicitlyWait, TimeUnit.SECONDS);
 
                if (driver.getCurrentUrl().contains("ErrorCode=1052")) {
                    // Error page, exit immediately instead of keep on waiting.
                    System.out.println("\nErrorCode=1052");
                    return;
                }
                driver.findElement(By.cssSelector("p.ng-binding"));
                System.out.println("\nNew page successfully loaded");
                return;
            } catch (WebDriverException ex) {
                // Intentionally left blank.
            }
            elapsedSeconds = (System.currentTimeMillis() - tStart) / 1000;
        }
        System.out.println("\n waitForPageLoaded(): elapsedSeconds= " + elapsedSeconds);
    }
}





























