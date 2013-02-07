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

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include("~/Scripts/toastr.js"));
			
			bundles.Add(new ScriptBundle("~/bundles/skinner").Include("~/Scripts/jquery-skinner.js"));  
			
			bundles.Add(new ScriptBundle("~/bundles/prettynumber").Include("~/Scripts/jquery.prettynumber.js"));  

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

            bundles.Add(new ScriptBundle("~/bundles/scrollto").Include("~/Scripts/jquery.scrollto.js"));

            bundles.Add(new ScriptBundle("~/bundles/smarttab").Include("~/Scripts/smarttab.js"));

            bundles.Add(new ScriptBundle("~/bundles/helpers").Include("~/Scripts/app.helpers.js"));

            bundles.Add(new ScriptBundle("~/bundles/apputils").Include("~/Scripts/app.utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/partnerinfo").Include("~/Scripts/app.partnerinfo.js"));

            bundles.Add(new ScriptBundle("~/bundles/webshop").Include("~/Scripts/app.webshop.js"));

            bundles.Add(new ScriptBundle("~/bundles/registration").Include("~/Scripts/app.registration.js"));

            bundles.Add(new ScriptBundle("~/bundles/company").Include("~/Scripts/app.company.js"));

            bundles.Add(new ScriptBundle("~/bundles/guide").Include("~/Scripts/app.guide.js"));

            bundles.Add(new ScriptBundle("~/bundles/carreer").Include("~/Scripts/app.carreer.js"));

            bundles.Add(new ScriptBundle("~/bundles/newsletter").Include("~/Scripts/app.newsletter.js"));

            bundles.Add(new ScriptBundle("~/bundles/Cufon").Include("~/Scripts/cufon-yui.js"));

            bundles.Add(new ScriptBundle("~/bundles/Segoe").Include("~/Scripts/Segoe_Pro_Display_400-Segoe_Pro_Display_Semibold_600-Segoe_Pro_Light_italic_300-Segoe_Pro_Semibold_italic_600.font.js"));

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
        }
    }
}