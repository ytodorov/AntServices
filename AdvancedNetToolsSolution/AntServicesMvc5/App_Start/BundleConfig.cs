using System.Web;
using System.Web.Optimization;

namespace AntServicesMvc5
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/jquery").Include(
virtualPath: "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/jqueryval").Include(
                       virtualPath: "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/modernizr").Include(
                       virtualPath: "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/bootstrap").Include(
                       virtualPaths: new string[] { "~/Scripts/bootstrap.js", "~/Scripts/respond.js" }));

            bundles.Add(new StyleBundle(virtualPath: "~/Content/css").Include(
                       virtualPaths: new string[] { "~/Content/bootstrap.css", "~/Content/site.css" }));
        }
    }
}
