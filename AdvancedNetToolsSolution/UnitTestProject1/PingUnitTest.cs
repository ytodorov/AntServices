using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace seleniumUnitTests
{
    [TestClass]
    public class PingUnitTest
    {
        public void TestWithChrome()
        {
            using (var driverService = ChromeDriverService.CreateDefaultService())
            {
                driverService.Start();
                IWebDriver driver = new ChromeDriver();
                INavigation nav = driver.Navigate();
                nav.GoToUrl("http://localhost:56110/ping");
                driver.FindElement(By.Id("ip")).Clear();
                driver.FindElement(By.Id("ip")).SendKeys("abv.bg");
                driver.FindElement(By.Id("btnPing")).SendKeys(Keys.Enter);

                //Assert.AreEqual("", value);
                driver.Close();
            }
        }
    }
}