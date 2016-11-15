
Partial Class Errors_OtherError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.TrySkipIisCustomErrors = True
    End Sub
End Class
