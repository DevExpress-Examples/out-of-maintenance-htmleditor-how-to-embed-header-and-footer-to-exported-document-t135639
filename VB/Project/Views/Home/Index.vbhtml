@Code 
    ViewBag.Title = "Index"
End Code

<h2>Index</h2>


@Using (Html.BeginForm("Export", "Home"))
    @Html.Action("HtmlEditorPartial")

    @<input type="submit" value="Export" />
End Using