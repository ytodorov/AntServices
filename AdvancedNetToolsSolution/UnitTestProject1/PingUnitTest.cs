using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
namespace seleniumUnitTests
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
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
                nav.GoToUrl(url: "http://localhost:56110/ping");
                driver.FindElement(By.Id(idToFind: "ip")).Clear();
                driver.FindElement(By.Id(idToFind: "ip")).SendKeys(text: "abv.bg");
                driver.FindElement(By.Id(idToFind: "btnPing")).SendKeys(Keys.Enter);

                //Assert.AreEqual("", value);
                driver.Close();
            }
        }
    }
}