Imports Microsoft.VisualBasic
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc
Imports DevExpress.XtraRichEdit
Imports System.IO
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraRichEdit.API.Native
Imports DevExpress.XtraPrintingLinks

Namespace T135508.Controllers
	Public Class HomeController
		Inherits Controller

		Public Function Index() As ActionResult
			Return View()
		End Function

		Public Function HtmlEditorPartial() As ActionResult
			Return PartialView("_HtmlEditorPartial")
		End Function
		Public Function HtmlEditorPartialImageSelectorUpload() As ActionResult
			HtmlEditorExtension.SaveUploadedImage("HtmlEditor", HomeControllerControllerHtmlEditorSettings.ImageSelectorSettings)
			Return Nothing
		End Function
		Public Function HtmlEditorPartialImageUpload() As ActionResult
			HtmlEditorExtension.SaveUploadedImage("HtmlEditor", HomeControllerControllerHtmlEditorSettings.ImageUploadValidationSettings, HomeControllerControllerHtmlEditorSettings.ImageUploadDirectory)
			Return Nothing
		End Function

		Public Function Export(ByVal HtmlEditor As String) As ActionResult
			Using stream As New MemoryStream()
				ExporToStream(HtmlEditor, stream)
				Response.Clear()
				Response.Buffer = False
				Response.AppendHeader("Content-Type", "application/pdf")
				Response.AppendHeader("Content-Transfer-Encoding", "binary")
				Response.AppendHeader("Content-Disposition", "attachment; filename=document.pdf")
				Response.BinaryWrite(stream.ToArray())
				Response.End()
			End Using
			Return View("Index")
		End Function

		Private Sub ExporToStream(ByVal html As String, ByVal stream As Stream)
			Using server As New RichEditDocumentServer()
				server.HtmlText = html
				Dim text As String = "<b>SOME SAMPLE TEXT</b>"
				AddHeaderToDocument(server.Document, text)
				AddFooterToDocument(server.Document, text)

				Using ps As New PrintingSystemBase()
					Using pcl As New PrintableComponentLinkBase(ps)
						pcl.Component = server
						pcl.CreateDocument()
						ps.ExportToPdf(stream)
					End Using
				End Using
			End Using
		End Sub

		Private Sub AddHeaderToDocument(ByVal document As DevExpress.XtraRichEdit.API.Native.Document, ByVal htmlText As String)
			Dim doc As SubDocument = document.Sections(0).BeginUpdateHeader()
			doc.AppendHtmlText(htmlText)
			document.Sections(0).EndUpdateHeader(doc)
		End Sub

		Private Sub AddFooterToDocument(ByVal document As DevExpress.XtraRichEdit.API.Native.Document, ByVal htmlText As String)
			Dim doc As SubDocument = document.Sections(0).BeginUpdateFooter()
			doc.AppendHtmlText(htmlText)
			document.Sections(0).EndUpdateFooter(doc)
		End Sub

	End Class
	Public Class HomeControllerControllerHtmlEditorSettings
		Public Const ImageUploadDirectory As String = "~/Content/UploadImages/"
		Public Const ImageSelectorThumbnailDirectory As String = "~/Content/Thumb/"

        Public Shared ImageUploadValidationSettings As New DevExpress.Web.ASPxUploadControl.ValidationSettings() With
            {.AllowedFileExtensions = New String() {".jpg", ".jpeg", ".jpe", ".gif", ".png"}, .MaxFileSize = 4000000}

		Private Shared imageSelectorSettings_Renamed As DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings
		Public Shared ReadOnly Property ImageSelectorSettings() As DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings
			Get
				If imageSelectorSettings_Renamed Is Nothing Then
					imageSelectorSettings_Renamed = New DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(Nothing)
					imageSelectorSettings_Renamed.Enabled = True
					imageSelectorSettings_Renamed.UploadCallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "HtmlEditorPartialImageSelectorUpload"}
					imageSelectorSettings_Renamed.CommonSettings.RootFolder = ImageUploadDirectory
					imageSelectorSettings_Renamed.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory
					imageSelectorSettings_Renamed.CommonSettings.AllowedFileExtensions = New String() { ".jpg", ".jpeg", ".jpe", ".gif" }
					imageSelectorSettings_Renamed.UploadSettings.Enabled = True
				End If
				Return imageSelectorSettings_Renamed
			End Get
		End Property
	End Class

End Namespace
