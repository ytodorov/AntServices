﻿#region Using

using System.Web.Mvc;

#endregion

namespace SmartAdminMvc
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //http://stackoverflow.com/questions/7265193/mvc-problem-with-custom-error-pages
            filters.Add(new HandleErrorAttribute() { View = "Error500" });
        }
    }
}