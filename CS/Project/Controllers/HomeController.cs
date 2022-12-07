using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpress.XtraRichEdit;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraPrintingLinks;

namespace T135508.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            return View();
        }

        public ActionResult HtmlEditorPartial() {
            return PartialView("_HtmlEditorPartial");
        }
        public ActionResult HtmlEditorPartialImageSelectorUpload() {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor", HomeControllerControllerHtmlEditorSettings.ImageSelectorSettings);
            return null;
        }
        public ActionResult HtmlEditorPartialImageUpload() {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor", HomeControllerControllerHtmlEditorSettings.ImageUploadValidationSettings, HomeControllerControllerHtmlEditorSettings.ImageUploadDirectory);
            return null;
        }

        public ActionResult Export(string HtmlEditor) {
            using (MemoryStream stream = new MemoryStream()) {
                ExporToStream(HtmlEditor, stream);
                Response.Clear();
                Response.Buffer = false;
                Response.AppendHeader("Content-Type", "application/pdf");
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.AppendHeader("Content-Disposition", "attachment; filename=document.pdf");
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
            return View("Index");
        }

        void ExporToStream(string html, Stream stream) {
            using (RichEditDocumentServer server = new RichEditDocumentServer()) {
                server.HtmlText = html;
                string text = "<b>SOME SAMPLE TEXT</b>";
                AddHeaderToDocument(server.Document, text);
                AddFooterToDocument(server.Document, text);

                using (PrintingSystemBase ps = new PrintingSystemBase()) {
                    using (PrintableComponentLinkBase pcl = new PrintableComponentLinkBase(ps)) {
                        pcl.Component = server;
                        pcl.CreateDocument();
                        ps.ExportToPdf(stream);
                    }
                }
            }
        }

        void AddHeaderToDocument(DevExpress.XtraRichEdit.API.Native.Document document, string htmlText) {
            SubDocument doc = document.Sections[0].BeginUpdateHeader();
            doc.AppendHtmlText(htmlText);
            document.Sections[0].EndUpdateHeader(doc);
        }

        void AddFooterToDocument(DevExpress.XtraRichEdit.API.Native.Document document, string htmlText) {
            SubDocument doc = document.Sections[0].BeginUpdateFooter();
            doc.AppendHtmlText(htmlText);
            document.Sections[0].EndUpdateFooter(doc);
        }

    }
    public class HomeControllerControllerHtmlEditorSettings {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings() {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings {
            get {
                if (imageSelectorSettings == null) {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Home", Action = "HtmlEditorPartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }

}
