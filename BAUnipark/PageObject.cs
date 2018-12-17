
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BAUnipark
{
    public class PageObject
    {
        [FindsBy(How = How.CssSelector, Using = "#time_from")]
        public IWebElement StartDateField;

        [FindsBy(How = How.CssSelector, Using = "#hour_from")]
        public IWebElement StartHourField;

        [FindsBy(How = How.CssSelector, Using = "#time_to")]
        public IWebElement EndDateField;

        [FindsBy(How = How.CssSelector, Using = "#hour_to")]
        public IWebElement EndHourField;

        [FindsBy(How = How.XPath, Using = "//a[@class='cookieConsentOK']")]
        public IWebElement CookieConsentAcceptField;

        [FindsBy(How = How.XPath, Using = "//div[@class='filter-1']//div[contains(@class, 'order-button') and not(contains(@class, 'order-button-mob'))]//button[@name='submit']")]
        public IWebElement ContinueOrderButton;

        [FindsBy(How = How.XPath, Using = "//input[@id='nr' and @type='text']")]
        public IWebElement CarNoField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='firstname' and @type='text']")]
        public IWebElement FirstNameField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='lastname' and @type='text']")]
        public IWebElement LastNameField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='phone_number' and @type='text']")]
        public IWebElement PhoneNoField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='email' and @type='text']")]
        public IWebElement EmailField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//label[@for='newsletter']")]
        public IWebElement NewsletterCheckbox;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//label[@for='name']")]
        public IWebElement ReceiptCheckbox;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='title' and @type='text']")]
        public IWebElement CompanyTitleField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='code' and @type='text']")]
        public IWebElement CompanyCodeField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='address' and @type='text']")]
        public IWebElement CompanyAddressField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//input[@name='vat_code' and @type='text']")]
        public IWebElement CompanyVatCodeField;

        [FindsBy(How = How.XPath, Using = "//div[@id='step_2']//label[@for='rules']")]
        public IWebElement RulesCheckbox;

        [FindsBy(How = How.XPath, Using = "//button[@id='accept-button']")]
        public IWebElement AcceptTermsButton;

        [FindsBy(How = How.XPath, Using = "//div[@class='message red']")]
        public IWebElement GeneralErrorMessage;

        [FindsBy(How = How.XPath, Using = "//input[@name='buy_now_submit']")]
        public IWebElement SubmitParkingOrder;

        [FindsBy(How = How.XPath, Using = "//div[@id='place_0']//td[@class='coll-4']")]
        public IList<IWebElement> ParkingOptionPrices;

        [FindsBy(How = How.XPath, Using = "//table[@data-zones]")]
        public IList<IWebElement> ExtraBookingOptions;

        [FindsBy(How = How.XPath, Using = "//span[@id='step_2_time_from']")]
        public IWebElement OrderReviewFromDate;

        [FindsBy(How = How.XPath, Using = "//span[@id='step_2_time_to']")]
        public IWebElement OrderReviewToDate;
    }
}
