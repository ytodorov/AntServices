using OpenQA.Selenium;
using SmartAdminMvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartAdminMvc.Controllers
{
    public class DashboardPage
    {
        public static bool IsAt
        {
            get
            {
                var button = Driver.Instance.FindElements(By.Id(idToFind: "pingResultsGrid"));
                if (button != null)
                {
                    return true;
                }
                return false;
            }
        }
    }
}