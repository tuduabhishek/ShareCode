
Partial Class frmLogin
    Inherits System.Web.UI.Page

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        If txtUser.Text <> "" Then
            Session("USER_ID") = txtUser.Text
            Response.Redirect("~/SelectAssesor_OPR.aspx", False)
        End If
    End Sub
End Class
