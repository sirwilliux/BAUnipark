using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
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
            PageFactory.InitElements(Driver, PageObject);
        }

        private PageObject _pageObject;
        public PageObject PageObject => _pageObject ?? (_pageObject = new PageObject());

        [Given(@"I Open Unipark website")]
        public void GivenIOpenUniparkWebsite()
        {
            Driver.Url = "http://unipark.lt/";
        }

        [Given(@"Choose the From date to be tomorrow 3 PM")]
        public void GivenChooseTheFromDateToBeTomorrowPM()
        {
            
            Thread.Sleep(1000);
            PageObject.StartDateField.Click();
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//a[contains(text(),'"+ (currentDay+1) +"')]")).Click();
            Thread.Sleep(1000);
            PageObject.CookieConsentAcceptField.Click();
            PageObject.StartHourField.Click();
            Driver.FindElement(By.XPath("//ul[@class='ui-timepicker-list']/li[contains(text(),'15:00')]")).Click();
        }

        [Given(@"Make sure today cannot be selected as To date")]
        public void GivenMakeSureTodayCannotBeSelectedAsToDate()
        {
            PageObject.EndDateField.Click();
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//span[contains(text(),'" + (currentDay) + "')]"));

        }

        [Given(@"Set To date to two days from now on and time that is the closest one to the From date")]
        public void GivenSetToDateToTwoDaysFromNowOnAndTimeThatIsTheClosestOneToTheFromDate()
        {
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//a[contains(text(),'" + (currentDay+2) + "')]")).Click();
            Thread.Sleep(1000);
            PageObject.EndHourField.Click();
            Driver.FindElement(By.XPath("//ul[@class='ui-timepicker-list']/li[contains(text(),'14:59')]")).Click();
        }

        [Given(@"Order the parking")]
        public void GivenOrderTheParking()
        {
            PageObject.OrderSubmitButton.Click();
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
            PageObject.CarNoField.SendKeys(Constants.CarNo);
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
            Thread.Sleep(2000);
            IList<IWebElement> extraOptions = Driver.FindElements(By.XPath("//table[@data-zones]"));
            var neededOption = "0";
            foreach (var option in extraOptions)
            {
                if (option.Displayed)
                {
                    neededOption = option.GetAttribute("data-id");
                }

            }
            Driver.FindElement(By.XPath("//table[@data-id='" + neededOption + "']//a[@class='up']")).Click();
            Driver.FindElement(By.XPath("//table[@data-id='" + neededOption + "']//a[@class='up']")).Click();
        }

        [Then(@"Fill all the personal data including all the agreements and options available")]
        public void ThenFillAllThePersonalDataIncludingAllTheAgreementsAndOptionsAvailable()
        {
            Thread.Sleep(1000);
            PageObject.FirstNameField.SendKeys(Constants.FirstName);
            PageObject.LastNameField.SendKeys(Constants.LastName);
            PageObject.PhoneNoField.SendKeys(Constants.PhoneNo);
            PageObject.EmailField.SendKeys(Constants.Email);
            PageObject.NewsletterCheckbox.Click();
            PageObject.ReceiptCheckbox.Click();
            PageObject.CompanyTitleField.SendKeys(Constants.CompanyTitle);
            PageObject.CompanyCodeField.SendKeys(Constants.CompanyCode);
            PageObject.CompanyAddressField.SendKeys(Constants.CompanyAddress);
            PageObject.CompanyVatCodeField.SendKeys(Constants.VatCode);
            PageObject.RulesCheckbox.Click();
            Thread.Sleep(500);
            PageObject.AcceptTermsButton.Click();
        }

        [Then(@"Refresh the page and make sure that all the data is still present and valid\.")]
        public void ThenRefreshThePageAndMakeSureThatAllTheDataIsStillPresentAndValid_()
        {
            Driver.Navigate().Refresh();
            Thread.Sleep(2000);
            Assert.AreEqual(PageObject.FirstNameField.GetAttribute("value"), Constants.FirstName);
            Assert.AreEqual(PageObject.LastNameField.GetAttribute("value"), Constants.LastName);
            Assert.AreEqual(PageObject.PhoneNoField.GetAttribute("value"), Constants.PhoneNo);
            Assert.AreEqual(PageObject.EmailField.GetAttribute("value"), Constants.Email);
            Assert.AreEqual(PageObject.CompanyTitleField.GetAttribute("value"), Constants.CompanyTitle);
            Assert.AreEqual(PageObject.CompanyCodeField.GetAttribute("value"), Constants.CompanyCode);
            Assert.AreEqual(PageObject.CompanyAddressField.GetAttribute("value"), Constants.CompanyAddress);
            Assert.AreEqual(PageObject.CompanyVatCodeField.GetAttribute("value"), Constants.VatCode);

        }

        [Then(@"Delete at least one of the mandatory fields and check that at least one error message is displayed\.")]
        public void ThenDeleteAtLeastOneOfTheMandatoryFieldsAndCheckThatAtLeastOneErrorMessageIsDisplayed_()
        {
            PageObject.EmailField.Clear();
            Driver.FindElement(By.XPath("//input[@name='buy_now_submit']")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(Driver.FindElement(By.XPath("//div[@class='message red']")).Displayed);
        }

    }
}
