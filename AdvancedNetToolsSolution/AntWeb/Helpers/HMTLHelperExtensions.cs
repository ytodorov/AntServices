using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace SmartAdminMvc
{
#pragma warning disable JustCode_NamingConventions // Naming conventions inconsistency
    public static class HMTLHelperExtensions
#pragma warning restore JustCode_NamingConventions // Naming conventions inconsistency
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            string cssClass = "active";
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }
}
