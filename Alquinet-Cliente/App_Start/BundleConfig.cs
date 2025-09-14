using System.Web;
using System.Web.Optimization;

namespace Alquinet_Cliente
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js",
                      "~/Scripts/leafletjs.js",
                      "~/Scripts/fontawesome/all.min.js",
                      "~/Scripts/loadingoverlay/loadingoverlay.min.js"//,
                      //"~/Scripts/sweetalert.min.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      //"~/Content/sweetalert.css",
                      "~/Content/bootstrap.min.css"));
        }
    }
}
