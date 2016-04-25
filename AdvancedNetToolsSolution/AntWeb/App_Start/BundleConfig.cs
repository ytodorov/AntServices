using System.Web;
using System.Web.Optimization;

namespace SmartAdminMvc
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Homer style
            bundles.Add(new StyleBundle(virtualPath: "~/bundles/homer/css").Include(
                    virtualPath: "~/Content/style.css", transforms: new System.Web.Optimization.IItemTransform[] { new CssRewriteUrlTransform() }));

            // Homer script
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/homer/js").Include(
                    virtualPaths: new string[] { "~/Vendor/metisMenu/dist/metisMenu.min.js", "~/Vendor/iCheck/icheck.min.js", "~/Scripts/homer.js" }));

            // Animate.css
            bundles.Add(new StyleBundle(virtualPath: "~/bundles/animate/css").Include(
                    virtualPath: "~/Vendor/animate.css/animate.min.css"));

            // Pe-icon-7-stroke
            bundles.Add(new StyleBundle(virtualPath: "~/bundles/peicon7stroke/css").Include(
                    virtualPath: "~/Icons/pe-icon-7-stroke/css/pe-icon-7-stroke.css", transforms: new System.Web.Optimization.IItemTransform[] { new CssRewriteUrlTransform() }));

            // Font Awesome icons style
            bundles.Add(new StyleBundle(virtualPath: "~/bundles/font-awesome/css").Include(
                    virtualPath: "~/Vendor/fontawesome/css/font-awesome.min.css", transforms: new System.Web.Optimization.IItemTransform[] { new CssRewriteUrlTransform() }));

            // Bootstrap style
            bundles.Add(new StyleBundle(virtualPath: "~/bundles/bootstrap/css").Include(
                    virtualPath: "~/Vendor/bootstrap/dist/css/bootstrap.min.css", transforms: new System.Web.Optimization.IItemTransform[] { new CssRewriteUrlTransform() }));

            // Bootstrap
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/bootstrap/js").Include(
                    virtualPath: "~/Vendor/bootstrap/dist/js/bootstrap.min.js"));

            // Ladda
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/ladda/js").Include(
                    virtualPaths: new string[] { "~/Vendor/ladda/dist/spin.min.js", "~/Vendor/ladda/dist/ladda.min.js", "~/Vendor/ladda/dist/ladda.jquery.min.js" }));

            // Ladda style
            bundles.Add(new StyleBundle(virtualPath: "~/bundles/ladda/css").Include(
                    virtualPath: "~/Vendor/ladda/dist/ladda-themeless.min.css"));

            // jQuery
            //bundles.Add(new ScriptBundle("~/bundles/jquery/js").Include(
            //          "~/Vendor/jquery/dist/jquery.min.js"));

            // jQuery UI
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/jqueryui/js").Include(
                    virtualPath: "~/Vendor/jquery-ui/jquery-ui.min.js"));

            // jQuery Validation
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/validation/js").Include(
                    virtualPath: "~/Vendor/jquery-validation/jquery.validate.min.js"));

            // Toastr
            bundles.Add(new ScriptBundle(virtualPath: "~/bundles/toastr/js").Include(
                    virtualPath: "~/Vendor/toastr/build/toastr.min.js"));

            // Toastr style
            bundles.Add(new StyleBundle(virtualPath: "~/bundles/toastr/css").Include(
                    virtualPath: "~/Vendor/toastr/build/toastr.min.css"));

        }

    }
}
