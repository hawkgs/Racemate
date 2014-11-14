namespace Racemate.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                "~/Scripts/libs/jquery-{version}.js",
                "~/Scripts/header-menu-light-switcher.js",
                "~/Scripts/common.js",
                "~/Scripts/menus.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/libs/jquery.validate*"
            ));

            bundles.Add(new ScriptBundle("~/bundles/createRace").Include(
                "~/Scripts/jquery.datetimepicker.js",
                "~/Scripts/maps/map-core.js",
                "~/Scripts/maps/route-builder.js",
                "~/Scripts/maps/create-race.js"
            ));

            bundles.Add(new ScriptBundle("~/bundle/visualizeRoute").Include(
                "~/Scripts/maps/map-core.js",
                "~/Scripts/maps/route-builder.js",
                "~/Scripts/maps/visualize-route.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/globalMap").Include(
                "~/Scripts/maps/map-core.js",
                "~/Scripts/maps/global-map.js"
            ));

            // Style bundles

            bundles.Add(new StyleBundle("~/Content/public").Include(
                "~/Content/common.css",
                "~/Content/public.css",
                "~/Content/msg.css",
                "~/Content/forms.css"
            ));

            bundles.Add(new StyleBundle("~/Content/private").Include(
                "~/Content/common.css",
                "~/Content/msg.css",
                "~/Content/forms.css",
                "~/Content/structure.css",
                "~/Content/style.css"
            ));

            bundles.Add(new StyleBundle("~/Content/raceDetails").Include(
                "~/Content/race.css"
            ));

            bundles.Add(new StyleBundle("~/Content/grids").Include(
                "~/Content/grids.css"
            ));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
