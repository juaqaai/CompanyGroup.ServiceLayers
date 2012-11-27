using System.Web;
using System.Web.Optimization;

namespace CompanyGroup.WebClient
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.unobtrusive*"));
            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            //head.load.js regisztráció
            bundles.Add(new ScriptBundle("~/bundles/head").Include("~/Scripts/head.load.js"));

            bundles.Add(new ScriptBundle("~/bundles/mustache").Include("~/Scripts/mustache.js"));

            //legutolsó verziójú sammy.js regisztráció
            bundles.Add(new ScriptBundle("~/bundles/sammy").Include("~/Scripts/sammy.js"));

            bundles.Add(new ScriptBundle("~/bundles/sammytitle").Include("~/Scripts/sammy.title.js"));

            bundles.Add(new ScriptBundle("~/bundles/sammytmpl").Include("~/Scripts/sammy.tmpl-latest.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sammymustache").Include("~/Scripts/sammy.mustache.js"));

            bundles.Add(new ScriptBundle("~/bundles/amplify").Include("~/Scripts/amplify.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include("~/Scripts/chosen.js"));
            
            //bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include("~/Scripts/jquery.autocomplete.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include("~/Scripts/jquery.fileupload.js"));

            bundles.Add(new ScriptBundle("~/bundles/floatingmessage").Include("~/Scripts/jquery.floatingmessage.js"));

            bundles.Add(new ScriptBundle("~/bundles/json2").Include("~/Scripts/jquery.json-2.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/tmpl").Include("~/Scripts/jquery.tmpl.js"));

            bundles.Add(new ScriptBundle("~/bundles/vegas").Include("~/Scripts/jquery.vegas.js"));

            bundles.Add(new ScriptBundle("~/bundles/easypaginate").Include("~/Scripts/easypaginate.js"));

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include("~/Scripts/jquery.fancybox.js"));

            bundles.Add(new ScriptBundle("~/bundles/smarttab").Include("~/Scripts/smarttab.js"));

            bundles.Add(new ScriptBundle("~/bundles/apputils").Include("~/Scripts/app.utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/partnerinfo").Include("~/Scripts/app.partnerinfo.js"));

            bundles.Add(new ScriptBundle("~/bundles/webshop").Include("~/Scripts/app.webshop.js"));

            bundles.Add(new ScriptBundle("~/bundles/registration").Include("~/Scripts/app.registration.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       // "http://fonts.googleapis.com/css?family=Lobster&subset=latin",
                        "~/Content/themes/custom/site.css",
                        "~/Content/themes/custom/hrp_bsc_html_css.css",
                        "~/Content/themes/custom/jquery-ui-1.8.17.custom.css",
                        "~/Content/themes/custom/chosen.css",
                        "~/Content/themes/custom/basketwiz.css",
                        "~/Content/themes/custom/jquery.fancybox.css",
                        "~/Content/themes/custom/jquery.autocomplete.css",
                        "~/Content/themes/custom/jquery.fileupload-ui.css",
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}