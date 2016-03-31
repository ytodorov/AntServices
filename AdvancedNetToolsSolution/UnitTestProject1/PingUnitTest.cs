using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.IO;

namespace seleniumUnitTests
{
    [TestClass]
    public class PingUnitTest
    {
        static IWebDriver driverGC;
        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            //driverGC = new ChromeDriver(@"C:\chomedriver_win32");
            string pathToDrive = Path.Combine(Environment.CurrentDirectory);
            driverGC = new ChromeDriver(pathToDrive);
            
        }

        [TestMethod]
        public void PingTestUI()
        {
            driverGC.Navigate().GoToUrl("http://localhost:56110/ping");
            driverGC.FindElement(By.Id("ip")).SendKeys("www.abv.bg");
            driverGC.FindElement(By.Id("btnPing")).SendKeys(Keys.Enter);

        }
    }
}