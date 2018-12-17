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

        public static void WaitIsDisplayed(IWebElement element, bool scrollToObject = true, bool click = false)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(500);
                if (element.Displayed)
                {
                    if (scrollToObject)
                    {
                        StepDefinition.Driver.ExecuteJavaScript("arguments[0].scrollIntoView(false);", element);
                    }

                    if (click)
                    {
                        Thread.Sleep(500); //Trying to deal with those pesky fades
                        element.Click();
                    }
                break;
                }
                else
                {
                    Thread.Sleep(500);
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
