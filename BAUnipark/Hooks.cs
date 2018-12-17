using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using TechTalk.SpecFlow;

namespace BAUnipark
{
    [Binding]
    public sealed class Hooks
    {
        public static void Main()
        {
            Console.WriteLine("Starting test...");
            Console.ReadLine();
        }

        public static void WaitIsDisplayed(IWebElement element, bool scrollToObject = true)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                try
                {
                    Assert.IsTrue(element.Displayed);
                    Thread.Sleep(500);               //Trying to deal with those pesky fades
                    break;
                }
                catch (AssertionException)
                {
                    Console.WriteLine("Element "+ element +" wasn't found");
                }

                if (scrollToObject)
                {
                    StepDefinition.Driver.ExecuteJavaScript("arguments[0].scrollIntoView();", element);
                }

            }

        }

        [AfterScenario]
        public void AfterScenario()
        {
            StepDefinition.Driver.Close();
        }
    }

}
