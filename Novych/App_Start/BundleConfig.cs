using System.Web;
using System.Web.Optimization;

namespace Novych
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            BundleTable.Bundles.UseCdn = true;

            bundles.Add(new StyleBundle("~/bundles/tether-css", "https://cdnjs.cloudflare.com/ajax/libs/tether/1.4.3/css/tether.min.css")
                .Include("~/Content/Styles/tether-{version}.min.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap-css", "https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css")
                .Include("~/Content/Styles/bootstrap-{version}.min.css"));

            bundles.Add(new StyleBundle("~/bundles/jquery-ui-css", "https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css")
                .Include("~/Content/Styles/jquery-ui-{version}.css"));

            bundles.Add(new StyleBundle("~/bundles/site-css").Include("~/Content/Styles/site.css"));

            bundles.Add(new StyleBundle("~/bundles/citerka-css").Include("~/Content/Styles/citerka.css"));

            bundles.Add(new StyleBundle("~/bundles/parniCistic-css").Include("~/Content/Styles/parniCistic.css"));

            bundles.Add(new StyleBundle("~/bundles/cookiealert-css").Include("~/Content/Styles/cookiealert.css"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr-js", "https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js")
                .Include("~/Content/Scripts/modernizr-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-js", "https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js")
                .Include("~/Content/Scripts/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui-js", "https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js")
                .Include("~/Content/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/tether-js", "https://cdnjs.cloudflare.com/ajax/libs/tether/1.4.3/js/tether.min.js")
                .Include("~/Content/Scripts/tether-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-js", "https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js")
                .Include("~/Content/Scripts/bootstrap-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/respond-js", "https://cdnjs.cloudflare.com/ajax/libs/respond.js/1.4.2/respond.min.js")
                .Include("~/Content/Scripts/respond-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval-js", "https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.1/jquery.validate.min.js")
                .Include("~/Content/Scripts/jquery.validate-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/cookiealert-js").Include("~/Content/Scripts/cookiealert.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-unobtrusive-ajax-js").Include("~/Content/Scripts/jquery.unobtrusive-ajax.3.2.6.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-validate-unobtrusive-js").Include("~/Content/Scripts/jquery.validate.unobtrusive.3.2.11.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/site-js").Include("~/Content/Scripts/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/citerka-js").Include("~/Content/Scripts/citerka.js"));

            bundles.Add(new ScriptBundle("~/bundles/parniCistic-js").Include("~/Content/Scripts/parniCistic.js"));

            /* New WAD has not defined used functions */
            bundles.Add(new StyleBundle("~/bundles/wad-js").Include("~/Content/Scripts/wad-2016-08-13.min.js"));
        }
    }
}
