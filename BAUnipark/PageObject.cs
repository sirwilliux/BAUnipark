
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace BAUnipark
{
    public class PageObject
    {
        [FindsBy(How = How.CssSelector, Using = "#time_from")]
        public IWebElement StartDateField;


    }
}
