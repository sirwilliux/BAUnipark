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
        private PageObject _pageObject;
        public PageObject PageObject => _pageObject ?? (_pageObject = new PageObject());
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

        private static string NeededDate(int daysAway, bool fullDate = false)
        {
            var yourDate = (DateTime.Today.AddDays(daysAway)).ToString("s");
            if (fullDate)
            {
                return yourDate.Substring(0, 10);
            }
            else
            {
                return yourDate.Substring(8, 2);
            }
        }

        public StepDefinition(Browser browser)
        {
            Driver = browser.Chrome;
            PageFactory.InitElements(Driver, PageObject);
        }

        [Given(@"I Open Unipark website")]
        public void GivenIOpenUniparkWebsite()
        {
            Driver.Url = "http://unipark.lt/";
        }

        [Given(@"Choose the From date to be tomorrow 3 PM")]
        public void GivenChooseTheFromDateToBeTomorrowPM()
        {
            
            Hooks.WaitIsDisplayed(PageObject.StartDateField, click: true);
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//a[contains(text(),'"+ NeededDate(1) +"')]")).Click();
            Hooks.WaitIsDisplayed(PageObject.CookieConsentAcceptField, click: true);
            PageObject.StartHourField.Click();
            Driver.FindElement(By.XPath("//ul[@class='ui-timepicker-list']/li[contains(text(),'15:00')]")).Click();
        }

        [Given(@"Make sure today cannot be selected as To date")]
        public void GivenMakeSureTodayCannotBeSelectedAsToDate()
        {
            PageObject.EndDateField.Click();
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//span[contains(text(),'" + NeededDate(0) + "')]"));

        }

        [Given(@"Set To date to two days from now on and time that is the closest one to the From date")]
        public void GivenSetToDateToTwoDaysFromNowOnAndTimeThatIsTheClosestOneToTheFromDate()
        {
            Thread.Sleep(1000);
            Driver.FindElement(By.XPath("//table[@class= 'ui-datepicker-calendar']//a[contains(text(),'" + NeededDate(2) + "')]")).Click();
            Hooks.WaitIsDisplayed(PageObject.EndHourField, click: true);
            Driver.FindElement(By.XPath("//ul[@class='ui-timepicker-list']/li[contains(text(),'14:59')]")).Click();
        }

        [Given(@"Order the parking")]
        public void GivenOrderTheParking()
        {
            PageObject.ContinueOrderButton.Click();
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
            Hooks.WaitIsDisplayed(PageObject.ParkingOptionPrices[0]);
            var parkingOptions = PageObject.ParkingOptionPrices;
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
            Hooks.WaitIsDisplayed(PageObject.ExtraBookingOptions[0]);
            var extraOptions = PageObject.ExtraBookingOptions;
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
            Hooks.WaitIsDisplayed(PageObject.FirstNameField);
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
            Hooks.WaitIsDisplayed(PageObject.AcceptTermsButton, false, true);
        }

        [Then(@"Refresh the page and make sure that all the data is still present and valid\.")]
        public void ThenRefreshThePageAndMakeSureThatAllTheDataIsStillPresentAndValid_()
        {
            Driver.Navigate().Refresh();
            Hooks.WaitIsDisplayed(PageObject.FirstNameField);
            Assert.AreEqual(NeededDate(1, true), PageObject.OrderReviewFromDate.Text);
            Assert.AreEqual(NeededDate(2, true), PageObject.OrderReviewToDate.Text);
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
            Hooks.WaitIsDisplayed(PageObject.SubmitParkingOrder, click: true);
            Hooks.WaitIsDisplayed(PageObject.GeneralErrorMessage);
        }

    }
}
