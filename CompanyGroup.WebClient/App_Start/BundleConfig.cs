using System.Web;
using System.Web.Optimization;

namespace CompanyGroup.WebClient
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = false;
            bundles.IgnoreList.Clear();
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*intellisense.js");

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/Lib/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/Lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/Lib/jquery-ui-{version}.js"));

            //head.load.js regisztráció
            //bundles.Add(new ScriptBundle("~/bundles/head").Include("~/Scripts/Lib/head.load.js"));
            //bundles.Add(new ScriptBundle("~/bundles/autocomplete").Include("~/Scripts/Lib/jquery.autocomplete.js"));

            bundles.Add(new ScriptBundle("~/bundles/lib").Include("~/Scripts/Lib/jquery.unobtrusive*", 
                                                                  "~/Scripts/Lib/jquery.validate.js", 
                                                                  "~/Scripts/Lib/toastr.js", 
                                                                  "~/Scripts/Lib/jquery-skinner.js", 
                                                                  "~/Scripts/Lib/jquery.prettynumber.js", 
                                                                  "~/Scripts/Lib/mustache.js", 
                                                                  "~/Scripts/Lib/sammy.js", 
                                                                  "~/Scripts/Lib/sammy.title.js", 
                                                                  "~/Scripts/Lib/sammy.tmpl-latest.min.js", 
                                                                  "~/Scripts/Lib/sammy.mustache.js", 
                                                                  "~/Scripts/Lib/amplify.js", 
                                                                  "~/Scripts/Lib/chosen.js", 
                                                                  "~/Scripts/Lib/jquery.fileupload.js", 
                                                                  "~/Scripts/Lib/jquery.form.js", 
                                                                  "~/Scripts/Lib/jquery.floatingmessage.js", 
                                                                  "~/Scripts/Lib/jquery.json-2.2.min.js", 
                                                                  "~/Scripts/Lib/jquery.tmpl.js", 
                                                                  "~/Scripts/Lib/jquery.vegas.js", 
                                                                  "~/Scripts/Lib/easypaginate.js", 
                                                                  "~/Scripts/Lib/jquery.fancybox.js", 
                                                                  "~/Scripts/Lib/jquery.scrollto.js",
                                                                  "~/Scripts/Lib/easypaginate.js", 
                                                                  "~/Scripts/Lib/smarttab.js", 
                                                                  "~/Scripts/Lib/cufon-yui.js", 
                                                                  "~/Scripts/Lib/Segoe_Pro_Display_400-Segoe_Pro_Display_Semibold_600-Segoe_Pro_Light_italic_300-Segoe_Pro_Semibold_italic_600.font.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include("~/Scripts/App/app.helpers.js", 
                                                                    "~/Scripts/App/app.utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/partnerinfo").Include("~/Scripts/App/app.partnerinfo.js"));

            bundles.Add(new ScriptBundle("~/bundles/webshop").Include("~/Scripts/App/app.webshop.js", 
                                                                      "~/Scripts/App/dataservice.webshop.js",
                                                                      "~/Scripts/App/main.webshop.js"));

            bundles.Add(new ScriptBundle("~/bundles/registration").Include("~/Scripts/App/app.registration.js"));

            bundles.Add(new ScriptBundle("~/bundles/company").Include("~/Scripts/App/app.company.js"));

            bundles.Add(new ScriptBundle("~/bundles/guide").Include("~/Scripts/App/app.guide.js"));

            bundles.Add(new ScriptBundle("~/bundles/carreer").Include("~/Scripts/App/app.carreer.js"));

            bundles.Add(new ScriptBundle("~/bundles/newsletter").Include("~/Scripts/App/app.newsletter.js",
                                                                         "~/Scripts/App/dataservice.newsletter.js",
                                                                         "~/Scripts/App/main.newsletter.js"));


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
                        "~/Content/themes/base/jquery.ui.theme.css",
                        "~/Content/themes/base/toartr.css"));

            //BundleTable.EnableOptimizations = true;

        }
    }
}