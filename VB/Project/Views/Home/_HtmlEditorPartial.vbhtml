@Html.DevExpress().HtmlEditor( _
    Sub(settings)
             
            settings.Name = "HtmlEditor"
            settings.CallbackRouteValues = New With { _
                Key .Controller = "Home", _
                Key .Action = "HtmlEditorPartial" _
            }
            settings.Width = 800
            settings.ToolbarMode = HtmlEditorToolbarMode.Menu
            settings.SettingsImageUpload.UploadCallbackRouteValues = New With { _
                Key .Controller = "Home", _
                Key .Action = "HtmlEditorPartialImageUpload" _
            }
            settings.SettingsImageUpload.UploadImageFolder = HomeControllerControllerHtmlEditorSettings.ImageUploadDirectory
            settings.SettingsImageUpload.ValidationSettings.Assign(HomeControllerControllerHtmlEditorSettings.ImageUploadValidationSettings)
            settings.SettingsImageSelector.Assign(HomeControllerControllerHtmlEditorSettings.ImageSelectorSettings)

    End Sub).GetHtml()