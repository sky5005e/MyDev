
import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;
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
import org.openqa.selenium.WebDriverException;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.*;
import org.testng.Assert;
import org.testng.ITestResult;
import org.testng.annotations.AfterMethod;
import org.testng.annotations.BeforeTest;
import org.testng.annotations.Test;

import org.testng.internal.Nullable;

public class Meschinowellness_F2MyWellnessReport_updated {

    private static final String CONTAINER_SELECTOR = "white-box-cms-wrap";
    WebDriver driver;

    @BeforeTest
    public void Setup() throws Exception {
        // Launch browser

        System.setProperty("webdriver.chrome.driver", "C:\\chromedriver_win32\\chromedriver.exe");
        driver = new ChromeDriver();
        driver.manage().window().maximize();
        driver.manage().deleteAllCookies();
        driver.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
        driver.get("http://demo-meschinowellness.azurewebsites.net/Account/Login");
        driver.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
    }

    @Test(priority = 1)
    public void login() throws Exception {
        // Enter Username
        driver.findElement(By.id("UserName")).click();
        Thread.sleep(1000);
        driver.findElement(By.id("UserName")).sendKeys("F2HRAtest@gmail.com");
        // Enter password
        driver.findElement(By.id("Password")).click();
        driver.findElement(By.id("Password")).sendKeys("Test123@");
        // Click on login button
        driver.findElement(By.id("btnLogin")).click();
        driver.manage().timeouts().implicitlyWait(1, TimeUnit.MINUTES);
        try
        {   
            GetPopupContent("chklistModelVideoPopup","close");
        } catch (Exception  e) {
            System.out.println("Exception while closing popup " + e.getMessage());
        }
        Thread.sleep(5000);
        // Verfiy Dashboard
        if (driver.findElement(By.id("dashboard")) != null) {
            System.out.println("Dashboard Title  is verified");
        } else {
            System.out.println("Dashboard Title  is not  Present");
        }
        Thread.sleep(5000);
        System.out.println("User login successful");
    }

    @Test(priority = 2)
    public void MyHealthRiskConsiderations() throws Exception {
        // Click on My Wellness Report tab
        driver.findElement(By.id("riskreportNew")).click();
        Thread.sleep(5000);
     // Click on what is this link
        WebElement Linkcontainer = driver.findElement(By.className("bmi-list"));
        WebElement whatisthislink = Linkcontainer.findElement(By.id("btnBioAgeScorePopup"));
        //whatisthislink.click();
        ((JavascriptExecutor)driver).executeScript("arguments[0].click();", whatisthislink);
        // Click on close button
        GetPopupContent("divBioAgeDialog","custom-blue-btn");
        Thread.sleep(5000);
        // Click on MHR what is this link
        WebElement MhrScorePopup = Linkcontainer.findElement(By.id("btnMHRScorePopup"));
        //MhrScorePopup.click();
        ((JavascriptExecutor)driver).executeScript("arguments[0].click();", MhrScorePopup);
        Thread.sleep(3000);
        // Click on MHR close button
        GetPopupContent("divMhrDialog","custom-blue-btn");
        Thread.sleep(5000);
        ArrayList<WebElement> list = getListOfLinks("tracking-tools", 0);
        Assert.assertNotEquals(0, list.size());
        for (int i = 0, l = list.size(); i < l; i++) {
            validateLinkAndRelatedPage("tracking-tools", 0, i, "wbc-heading", "h3", "button");
        }
        System.out.println("MyHealthRiskConsiderations test completed.");
        Thread.sleep(10000);
    }

    @Test(priority = 3)
    public void MyHealthIssues() throws Exception {
        System.out.println("MyHealthIssues test started.");
        // Click on My Wellness Report tab
        ArrayList<WebElement> list1 = getListOfLinks("tracking-tools", 1);
        Assert.assertNotEquals(0, list1.size());
        for (int i = 0, l = list1.size(); i < l; i++) {
            ValidateLinkAndRelatedPage2("tracking-tools", 1, i, "active-goal-heading", "h2", "a");
        }
    }
    public void GetPopupContent(String ElementId, String classname)
    {
        WebElement popupElement = driver.findElement(By.id(ElementId));
        WebElement element =  popupElement.findElement(By.className(classname));//.click();
        ((JavascriptExecutor)driver).executeScript("arguments[0].click();", element);
    }
  //Method to click  the option and scroll the content
    public void ClickElement(WebElement element) {
        System.out.println("js click ");
        JavascriptExecutor je = (JavascriptExecutor) driver;
        je.executeScript("arguments[0].click();", element);
        
    }
    private ArrayList<WebElement> getListOfLinks(String sectionName, int index) {
        ArrayList<WebElement> sections = (ArrayList<WebElement>) driver.findElements(By.className(sectionName));
        if (sections.size() > 0) {
            WebElement section = sections.get(index);
            // Get the track-list
            WebElement listContainer = (WebElement) section.findElements(By.className("track-list")).get(0);
            ArrayList<WebElement> list = (ArrayList<WebElement>) listContainer.findElements(By.tagName("li"));
            return list;
        }
        return null;
    }
    /*
     * Map Dietary Assessment_Links - List<String> Dietary Assessment_Italics -
     * List<String> Dietary Assessment_Strong - List<String>
     */

    public static Map<String, List<String>> EXPECTED_STRINGS = new HashMap<String, List<String>>();
    {
        /**
         * Map Format: Key = LinkText_<format> - Eg: Dietary Assessment_Strong,
         * Dietary Assessment_Italics Value = List of expected strings
         */
        // Bold strings Dietary Assessment
        EXPECTED_STRINGS.put("Dietary Assessment_Strong",
                Arrays.asList("Introduction to Your Dietary Assessment",
                        "The Meschino Optimal Living Program",
                        "Your Personal Feedback Report",
                        "Based upon your responses",
                        "18.00",
                        "It is impressive that you have maintained a low intake of whole egg consumption",
                        "It is impressive that your diet is not high in fried foods.",
                        "It is impressive that you have maintained a low intake of high fat pastries and related treats.",
                        "It is impressive that you have maintained a low intake of fried snack foods and regular chocolate products",
                        "It is impressive that you have maintained a low intake of highly refined sugar-containing foods and beverages.",
                        "Your intake of vegetables appears to be consistent with practices associated with a healthy diet.",
                        "Your diet appears to lack proper attention to fruit intake.",
                        "Your low intake of alcohol is most impressive from a health-risk standpo",
                        "You indicated that you consume high fat meat products only 2-3 times per month, but consume high fat dairy products on a daily basis,",
                        "It is most impressive that you do not use butter on bread products or on baked potatoes or vegetables."));

        // Links Dietary Assessment
        EXPECTED_STRINGS.put("Dietary Assessment_Links",
                Arrays.asList());

        // Bold strings Body Composition
        EXPECTED_STRINGS.put("Body Composition_Strong",
                Arrays.asList(
                        "Your Body Mass Index is: 20.60",
                        "This is a good reading",
                        "Waist Circumference",
                        "Your present waist circumference is",
                        "30."));

        // Bold strings Physical Activity
        EXPECTED_STRINGS.put("Physical Activity_Strong",
                Arrays.asList(
                        "Endurance / Aerobic Activity",
                        "You indicated that you perform endurance/aerobic exercise for 120 to 150 minutes per week, on average.",
                        "Aerobic Training Zone",
                        "Based upon your present age, your aerobic training zone is between 104 and 147 heart beats per minute.",
                        "Resistance / Strength Training",
                        "You indicated that you regularly perform resistance or strength training a minimum of 4 times a",
                        "your protein requirement would be approximately 60 grams per day.",
                        "Flexibility / Stretching Exercise",
                        "You indicated that you regularly perform flexibility or stretching exercises a minimum of 4 times per week"));

        // Links Physical Activity
        EXPECTED_STRINGS.put("Physical Activity_Links",
                Arrays.asList("http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/StepTracker?healthActivityId=10",
                        "https://meschinowellness.blob.core.windows.net/downloads/protein_food_chart.pdf",
                        "https://meschinowellness.blob.core.windows.net/downloads/StrengthTraining2.pdf"));

        // Bold strings Basic Supplement Considerations
        EXPECTED_STRINGS.put("Basic Supplement Considerations_Strong",
                Arrays.asList());

        // Links Basic Supplement Considerations
        EXPECTED_STRINGS.put("Basic Supplement Considerations_Links",
                Arrays.asList("http://www.nature.com/nrc/journal/v4/n3/full/nrc1298.html",
                        "http://www.pnas.org/content/110/23/9523.short",
                        "http://journals.plos.org/plosone/article?id=10.1371/journal.pone.0012244",
                        "http://www.sciencedaily.com/releases/2010/09/100912213050.htm"));

        // Bold strings Nutrient Deficiencies and Depletion of Nutrients
        EXPECTED_STRINGS.put("Nutrient Deficiencies and Depletion of Nutrients_Strong", 
                Arrays.asList("Nutrient Deficiencies and Depletion of Nutrients",
                        "Damage to the Skin from Sunlight and Tanning Beds",
                        "Cracks at the outer margins (corners) of the lips can be caused or aggravated by a Vitamin B2 or vitamin B6 deficiency",
                        "A sore or burning tongue can be caused by deficiencies in Vitamins B2, B3, B6, and/or B12.",
                        "A reduced ability to taste food is can be caused by a marginal zinc deficiency.",
                        "Gums that bleed easily is often a sign of Vitamin C deficiency.",
                        "From a nutritional standpoint skin that bruises easily can be caused by a deficiency",
                        "Slow healing capability can be an indication",
                        "Feeling chronically fatigued",
                        "Irregular eating patterns often results in inadequate intake of many essential vitamins and minerals",
                        "Calorie restricted diets cannot provide optimal intake levels of all vitamins and minerals",
                        "Feeling run down and/or in a weakened state of immunity",
                        "Hair falling out easily can be from genetic or hormonal causes,",
                        "Your regular use of antacid drugs is associated with depletion of the following nutrients",
                        "Your regular use of cholesterol-lowering bile acid sequestrant drugs is associated with depletion of the following nutrients:",
                        "Your regular use of beta-blocker drugs",
                        "Your regular use of digitalis",
                        "Your regular use of corticosteroids is associated with depletion",
                        "Your regular use of indomethacin is",
                        "Your regular use of diuretic drugs is associated with depletion",
                        "Your regular use of L-dopa",
                        "The asthmatic drug theophylline decreases",
                        "Stress"));

        // Links Nutrient Deficiencies and Depletion of Nutrients
        EXPECTED_STRINGS.put("Nutrient Deficiencies and Depletion of Nutrients_Links",
                Arrays.asList("http://demo-meschinowellness.azurewebsites.net/LearnEarn/LearningInfoByModule/188",
                "http://demo-meschinowellness.azurewebsites.net/MyWellnessWallet/MyHealthProfile/StressTracker?healthActivityId=13"));

        // Bold strings Drug-Nutrient Interactions and Precautions
        EXPECTED_STRINGS.put("Drug-Nutrient Interactions and Precautions_Strong", 
                Arrays.asList("Drug-Nutrient Interactions and Precautions",
                        "Drug-Nutrient Interactions and Precautions",
                        "Various sections of your Meschino Health and Wellness report",
                        "these warning statements will be listed immediately below this paragraph. If no warning appears then you may consider proceeding with the supplement considerations that appear in various sections of your feedback report. However, you should always consult a health care professional before making any change to your diet, exercise or supplementation program.",
                        "If no warning appears then you may consider proceeding with the supplement considerations that appear in various sections of your feedback report. However, you should always consult a health care professional before making any change to your diet, exercise or supplementation program."));

        // Italics Drug-Nutrient Interactions and Precautions
        EXPECTED_STRINGS.put("Drug-Nutrient Interactions and Precautions_Italics", 
                Arrays.asList(
                        "Are you taking the drug digitalis or digoxin?",
                        "Do you have an active ulcer?",
                        "Are you presently taking any anti-inflammatory drugs, other than acetaminophen, or blood thinners (anti-coagulants)?",
                        "Do you have a pacemaker?",
                        "Are you taking drugs to correct a heart arrhythmia problem?",
                        "Are you allergic to aspirin?",
                        "Do you suffer from hemophilia?",
                        "Are you presently experiencing a flare up of gout (gouty arthritis)?",
                        "Do you have advanced liver disease?",
                        "Do you have hyperparathyroidism, sarcoidosis, active tuberculosis, silicosis, or lymphoma?",
                        "Are you taking a drug a called methotrexate?",
                        "Are you presently taking any chemotherapy drugs or undergoing radiation therapy for the treatment of cancer?",
                        "Are you currently taking any drugs for depression or to treat a psychological disorder of any kind (etc. bipolar disease, schizophrenia, obsessive compulsive disorder etc.)?",
                        "Are you taking the drug accutane (usually for acne)?",
                        "Are you presently taking a narcotic drug (e.g. Percodan, Percocet, Oxycontin, Oxycodone, Morphine etc)?",
                        "Are you presently taking an anti-anxiety drug (e.g. benzodiazepine such as Valium, Ativan etc)?",
                        "Are you presently taking a sleep aid medication (e.g. Sonata, Ambien, etc)?",
                        "Are you known to be allergic to morphine or opiod-containing drugs?",
                        "Are you taking radioactive iodine to treat thyroid cancer, Grave�s disease or other thyroid disorder?",
                        "Are you taking vitamin-K blocking anti-coagulants (e.g. Coumadin, Warfarin, Jantoven, Marevan, Lawarin, Waran, Warfant)?"));

        // Bold strings Family Health History
        EXPECTED_STRINGS.put("Family Health History_Strong",
                Arrays.asList("You indicated that there is a family history of breast cancer."));

        // Links Family Health History
        EXPECTED_STRINGS.put("Family Health History_Links",
                Arrays.asList("https://meschinowellness.blob.core.windows.net/downloads/27F%20FHH%20Breast-Colon.pdf"));

        // Bold strings Healthy Aging Supplement Considerations and Early
        // Detection
        EXPECTED_STRINGS.put("Healthy Aging Supplement Considerations and Early Detection_Strong",
                Arrays.asList("Mammogram:  After age 40 it advisable to speak to your doctor about having a mammogram.",
                        "Because you are over 45 you should consider a supplement providing coenzyme Q10 and hawthorn for heart health."));

        // Bold strings Basic Blood Tests of Significance
        EXPECTED_STRINGS.put("Basic Blood Tests of Significance_Strong",
                Arrays.asList());

        // Links Basic Blood Tests of Significance
        EXPECTED_STRINGS.put("Basic Blood Tests of Significance_Links",
                Arrays.asList("https://meschinowellness.blob.core.windows.net/downloads/basic_blood_tests.pdf",
                        "https://meschinowellness.blob.core.windows.net/downloads/blood_interp.pdf"));

    }
    
    private void validateLinkAndRelatedPage(String sectionName, int sectionIndex, int elementIndex,
            String headingContainer, String headingSelector, String buttonSelector) {
        ArrayList<WebElement> list = getListOfLinks(sectionName, sectionIndex);
        System.out.println("Text = " + list.get(elementIndex).getText());
        String currentLinkText = list.get(elementIndex).getText(); // Eg:DietaryAssessment
        WebElement currentLIItem = list.get(elementIndex);
        // Find the anchor tag
        WebElement anchorTag = currentLIItem.findElement(By.tagName("a"));
        Assert.assertNotNull(anchorTag);
        // continue execution and click on links
        JavascriptExecutor je = (JavascriptExecutor) driver;
        je.executeScript("arguments[0].click();", anchorTag);

        // Goes to the next page, get the header in the next page
        WebElement headingContainerElement = driver.findElement(By.className(headingContainer));
        Assert.assertNotNull(headingContainerElement);

        // Get the heading text (h3)
        WebElement headingText = headingContainerElement.findElement(By.tagName(headingSelector));
        Assert.assertNotNull(headingText);
        System.out.println("Heading Text = " + headingText.getText() + ", Link Text = " + currentLinkText);
        Assert.assertEquals(headingText.getText(), currentLinkText);

        // Query map for strong strings

        List<String> expectedBoldStrings = EXPECTED_STRINGS.get(currentLinkText + "_Strong");

        if (expectedBoldStrings != null) {
            WebElement container = driver.findElement(By.className(CONTAINER_SELECTOR));
            List<WebElement> boldStringsInSection = (List<WebElement>) container.findElements(By.tagName("strong"));

            List<String> listOfBoldTextOnPage = new ArrayList<String>();
            for (WebElement element : boldStringsInSection) {
                listOfBoldTextOnPage.add(element.getText().trim());
            }
            for (String expectedBoldString : expectedBoldStrings) {

                if (listOfBoldTextOnPage.stream().filter(x -> x.contains(expectedBoldString)).count() > 0) {
                    System.out.println("Validated Bold Text===>> " + expectedBoldString);
                } else {
                    System.out.println("Not Validated Bold Text===>>  " + expectedBoldString);
                }
            }
        }

        // Query map for Link Texts
        List<String> expectedLinkTexts = EXPECTED_STRINGS.get(currentLinkText + "_Links");

        if (expectedLinkTexts != null) {
            WebElement container1 = driver.findElement(By.className(CONTAINER_SELECTOR));
            List<WebElement> linkTextsInSection = (List<WebElement>) container1.findElements(By.tagName("a"));

            List<String> listOfLinkTextOnPage = new ArrayList<String>();
            for (WebElement element : linkTextsInSection) {
                listOfLinkTextOnPage.add(element.getAttribute("href"));
            }

            for (String expectedLinkText : expectedLinkTexts) {

                if (listOfLinkTextOnPage.stream().filter(x -> x.contains(expectedLinkText)).count() > 0) {
                    System.out.println("Validated Link Text===>> " + expectedLinkText);
                } else {
                    System.out.println("Not Validated Link Text===>>  " + expectedLinkText);
                }
            }
        }
        // Query map for ItalicTexts
        List<String> expectedItalicTexts = EXPECTED_STRINGS.get(currentLinkText + "_Italics");
        if (expectedItalicTexts != null) {
            WebElement container2 = driver.findElement(By.className(CONTAINER_SELECTOR));
            List<WebElement> italicTextsInSection = (List<WebElement>) container2.findElements(By.tagName("em"));

            List<String> listOfItalicTextOnPage = new ArrayList<String>();
            for (WebElement element : italicTextsInSection) {
                listOfItalicTextOnPage.add(element.getText().trim());
            }

            for (String expectedItalicText : expectedItalicTexts) {

                if (listOfItalicTextOnPage.stream().filter(x -> x.contains(expectedItalicText)).count() > 0) {
                    System.out.println("Validated Italic Text===>> " + expectedItalicText);
                } else {
                    System.out.println("Not Validated Italic Text===>>  " + expectedItalicText);
                }
            }
        }
        System.out.println("Validated " + currentLinkText + ", Going back for the rest!!");
        WaitForPageLoaded();
        // Go back
        WebElement backButton = headingContainerElement.findElement(By.tagName(buttonSelector));
        backButton.click();
    }
    
    private void ValidateLinkAndRelatedPage2(String sectionName, int sectionIndex, int elementIndex,
            String headingContainer, String headingSelector, String buttonSelector) {
        ArrayList<WebElement> list = getListOfLinks(sectionName, sectionIndex);
        System.out.println("Text = " + list.get(elementIndex).getText());
        String currentLinkText = list.get(elementIndex).getText(); // Eg:DietaryAssessment
        WebElement currentLIItem = list.get(elementIndex);
        // Find the anchor tag
        WebElement anchorTag = currentLIItem.findElement(By.tagName("a"));
        Assert.assertNotNull(anchorTag);
        // continue execution and click on links
        JavascriptExecutor je = (JavascriptExecutor) driver;
        je.executeScript("arguments[0].click();", anchorTag);

        // Goes to the next page, get the header in the next page
        WebElement headingContainerElement = driver.findElement(By.className(headingContainer));
        Assert.assertNotNull(headingContainerElement);

        // Get the heading text (h3)
        WebElement headingText = headingContainerElement.findElement(By.tagName(headingSelector));
        Assert.assertNotNull(headingText);
        System.out.println("Heading Text = " + headingText.getText() + ", Link Text = " + currentLinkText);
        
        System.out.println("Validated " + currentLinkText + ", Going back for the rest!!");
        WaitForPageLoaded();
        // Go back
        WebElement backButton = headingContainerElement.findElement(By.tagName(buttonSelector));
        backButton.click();
    }
 // Method to wait for the next page to load
    public void WaitForPageLoaded() {
        int maxWait = 30;
        long tStart = System.currentTimeMillis();
        long elapsedSeconds = 0;
        for (; elapsedSeconds < maxWait;) {
            try {
                    driver.manage().timeouts().implicitlyWait(20, TimeUnit.SECONDS);
                    if (driver.getCurrentUrl().contains("ErrorCode=1052")) {
                     // Error page, exit immediately instead of keep on waiting.
                    System.out.println("\nErrorCode=1052");
                    return;
                }
                System.out.println("\nNew page successfully loaded");
                return;
            } catch (WebDriverException ex) {
                // Intentionally left blank.
            }
            elapsedSeconds = (System.currentTimeMillis() - tStart) / 1000;
        }
        System.out.println("\n waitForPageLoaded(): elapsedSeconds= " + elapsedSeconds);
    }
    
    @AfterMethod
    public void logout(ITestResult result) throws Exception {
        // Verify if test fails
        if (ITestResult.FAILURE == result.getStatus()) {
            try {
                // Create reference of TakesScreenshot
                TakesScreenshot ts = (TakesScreenshot) driver;
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
