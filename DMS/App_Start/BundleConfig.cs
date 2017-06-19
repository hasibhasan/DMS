using System.Web;
using System.Web.Optimization;

namespace DMS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/js/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/bootstrap.css",
                      "~/Content/css/font-awesome.css", 
                      "~/Content/css/ace-fonts.css", "~/Content/css/ace.css"));     

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/js/ace/elements.scroller.js", "~/Scripts/js/ace/elements.colorpicker.js", "~/Scripts/js/ace/elements.fileinput.js",
                       "~/Scripts/js/ace/elements.typeahead.js", "~/Scripts/js/ace/elements.wysiwyg.js", "~/Scripts/js/ace/elements.spinner.js",
                       "~/Scripts/js/ace/elements.treeview.js", "~/Scripts/js/ace/elements.wizard.js", "~/Scripts/js/ace/elements.aside.js",
                       "~/Scripts/js/ace/ace.js", "~/Scripts/js/ace/ace.ajax-content.js", "~/Scripts/js/ace/ace.touch-drag.js", "~/Scripts/js/ace/ace.sidebar.js",
                       "~/Scripts/js/ace/ace.sidebar-scroll-1.js", "~/Scripts/js/ace/ace.submenu-hover.js", "~/Scripts/js/ace/ace.widget-box.js",
                       "~/Scripts/js/ace/ace.settings.js", "~/Scripts/js/ace/ace.settings-rtl.js", "~/Scripts/js/ace/ace.widget-on-reload.js", "~/Scripts/js/ace/ace.searchbox-autocomplete.js", "~/Scripts/js/jquery.date-dropdowns.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable-script").Include(
                 "~/Scripts/js/dataTables/jquery.dataTables.js","~/Scripts/js/dataTables/jquery.dataTables.bootstrap.js", 
                    "~/Scripts/js/dataTables/extensions/TableTools/js/dataTables.tableTools.js","~/Scripts/js/dataTables/extensions/ColVis/js/dataTables.colVis.js"
                 ));            
        }
    }
}
