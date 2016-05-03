using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartAdminMvc.Controllers;
using SmartAdminMvc.Infrastructure;
using SmartAdminMvc.Models;


#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
namespace seleniumUnitTests
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
{
    [TestClass]    
    public class PingUnitTest
    {
        [TestInitialize]
        public void Init()
        {
            Driver.Initialize();
        }
        [TestMethod]
        public void PingTestWithChrome()
        {
            PingController.GoTo();
            PingController.Ping(address: "google.bg");
            Assert.IsTrue(DashboardPage.IsAt, message: "Failed to ping");

        }
        [TestCleanup]
        public void Cleanup()
        {
            Driver.Close();
        }
    }
}