using System;
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
    }
}
