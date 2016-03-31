using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace seleniumUnitTests
{
    [TestClass]
    public class PingUnitTest
    {
        static IWebDriver driverGC;
        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            driverGC = new ChromeDriver(@"C:\chomedriver_win32");
        }

        public static void main(String[] args)
        {
            driverGC.Navigate().GoToUrl("http://localhost:56110/ping");
            driverGC.FindElement(By.Id("ip")).SendKeys("www.abv.bg");
            driverGC.FindElement(By.Id("btnPing")).SendKeys(Keys.Enter);

        }
    }
}