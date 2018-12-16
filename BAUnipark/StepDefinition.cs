using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;


namespace BAUnipark
{
    [Binding]
    public class StepDefinition
    {
        public static int currentDay = DateTime.Today.Day;
        public static IWebDriver Driver;
        public class Browser 
        {
            public IWebDriver Chrome;

            public Browser()
            {
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                Chrome = new ChromeDriver(options);
            }
        }
        public StepDefinition(Browser browser)
        {
            Driver = browser.Chrome;
        }

        [Given(@"I Open Unipark website")]
        public void GivenIOpenUniparkWebsite()
        {
            Driver.Url = "http://unipark.lt/";
        }

        [Given(@"Choose the From date to be tomorrow 3 PM")]
        public void GivenChooseTheFromDateToBeTomorrowPM()
        {
            
            Thread.Sleep(1000);
            Driver.FindElement(By.CssSelector("#time_from")).Click();
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//a[contains(text(),'"+ (currentDay+1) +"')]")).Click();
            Thread.Sleep(1000);
            Driver.FindElement(By.CssSelector("#hour_from")).Click();
            Driver.FindElement(By.XPath("//ul[@class='ui-timepicker-list']/li[contains(text(),'15:00')]")).Click();
        }

        [Given(@"Make sure today cannot be selected as To date")]
        public void GivenMakeSureTodayCannotBeSelectedAsToDate()
        {
            Driver.FindElement(By.CssSelector("#time_to")).Click();
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//span[contains(text(),'" + (currentDay) + "')]"));

        }

        [Given(@"Set To date to two days from now on and time that is the closest one to the From date")]
        public void GivenSetToDateToTwoDaysFromNowOnAndTimeThatIsTheClosestOneToTheFromDate()
        {
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//a[contains(text(),'" + (currentDay+2) + "')]")).Click();
            Thread.Sleep(1000);
            Driver.FindElement(By.CssSelector("#hour_to")).Click();
            Driver.FindElement(By.XPath("//ul[@class='ui-timepicker-list']/li[contains(text(),'14:59')]")).Click();
        }

        [Given(@"Order the parking")]
        public void GivenOrderTheParking()
        {
            Driver.FindElement(By.XPath("//div[@class='filter-1']//div[contains(@class, 'order-button') and not(contains(@class, 'order-button-mob'))]//button[@name='submit']")).Click();
        }

        [Given(@"Select Riga's airport and make sure that the only zone is available")]
        public void GivenSelectRigaSAirportAndMakeSureThatTheOnlyZoneIsAvailable()
        {
            Driver.FindElement(By.XPath("//div[@class='zone-select-div']//a[contains(text(),'Rygos')]")).Click();
            Thread.Sleep(500);
            Driver.FindElement(By.XPath("//div[@class='choose']//div[contains(text(), 'RIX')]"));
        }

        [Given(@"Enter car related data")]
        public void GivenEnterCarRelatedData()
        {
            Driver.FindElement(By.XPath("//input[@id='nr' and @type='text']")).SendKeys("JOU351");
        }

        [Given(@"Select Vilnius cheapest zone")]
        public void GivenSelectVilniusCheapestZone()
        {
            Driver.FindElement(By.XPath("//div[@class='zone-select-div']//a[contains(text(),'Vilniaus')]")).Click();
            Thread.Sleep(500);
            IList<IWebElement> parkingOptions = Driver.FindElements(By.XPath("//div[@id='place_0']//td[@class='coll-4']"));
            List<float> prices = new List<float>();
            foreach (var parkingOption in parkingOptions)
            {
                string priceGet = parkingOption.Text;
                string parsedPrice = Regex.Replace(priceGet, "[A-Za-zž,]", "");
                if (parsedPrice != String.Empty)
                {
                    prices.Add(Int32.Parse(parsedPrice));
                }
                
            }

            var cheapest = prices.Min();
            var cheapest2 = cheapest.ToString();
            Driver.FindElement(By.XPath("//div[@class='choose']//td[contains(text(), '" + cheapest2.Remove(cheapest2.Length-2) + "')]/..//a[@title='Toliau']")).Click();

        }

        [Given(@"Add last extra service for two adults")]
        public void GivenAddLastExtraServiceForTwoAdults()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"Fill all the personal data including all the agreements and options available")]
        public void ThenFillAllThePersonalDataIncludingAllTheAgreementsAndOptionsAvailable()
        {
            ScenarioContext.Current.Pending();
        }

    }
}
