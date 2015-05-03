using System.Web.Optimization;

namespace DreamReader.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/script")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery-ui-{version}.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/knockout-{version}.js")
                .Include("~/Scripts/knockout.validation.js")
                .Include("~/Scripts/jquery.iframe-transport.js")
                .Include("~/Scripts/jquery.fileupload.js")
                .Include("~/Scripts/dreamreader.js")
                .Include("~/Scripts/ViewModels/DreamReaderViewModel.js")
                .Include("~/Scripts/ViewModels/SignUpViewModel.js")
                .Include("~/Scripts/ViewModels/SignInViewModel.js")
                .Include("~/Scripts/ViewModels/BookViewModel.js")
                .Include("~/Scripts/ViewModels/BookSectionViewModel.js")
                .Include("~/Scripts/ViewModels/BookSectionRowViewModel.js")
                .Include("~/Scripts/ViewModels/BookUploadViewModel.js")
                );

            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/loader.css")
                .Include("~/Content/jquery.fileupload.css")
                .Include("~/Content/dreamreader.css")
                );
        }
    }
}
