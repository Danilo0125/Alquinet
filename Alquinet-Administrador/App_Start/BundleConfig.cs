using System.Web;
using System.Web.Optimization;

namespace Alquinet_Administrador
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Bundle para jQuery
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Bundle para scripts adicionales
            bundles.Add(new Bundle("~/bundles/complementos").Include(
                        "~/Scripts/scripts.js",
                        "~/Scripts/fontawesome/all.min.js",
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/dataTables.responsive.js",
                        "~/Scripts/loadingoverlay/loadingoverlay.min.js",
                        "~/Scripts/sweetalert.min.js"
                        ));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            // Bundle para CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/responsive.dataTables.css"));

            bundles.Add(new StyleBundle("~/Content/siteCss").Include(
            "~/Content/sweetalert.css",
            "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/dataTablesCss").Include(
                        "~/Content/DataTables/css/jquery.dataTables.css"));

            bundles.Add(new StyleBundle("~/Content/dataTablesResponsiveCss").Include(
                        "~/Content/DataTables/css/responsive.dataTables.css"));

        }
    }
}
