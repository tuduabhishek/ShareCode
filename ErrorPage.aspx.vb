
Partial Class ErrorPage
    Inherits System.Web.UI.Page
    Private Sub ErrorPage_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblMsg.Text = Session("errorMsg").ToString()
    End Sub
End Class
