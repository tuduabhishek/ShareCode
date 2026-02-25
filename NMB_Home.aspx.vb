
Partial Class NMB_Home
    Inherits System.Web.UI.Page
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "Home"
            End If
        Catch ex As Exception

        End Try

    End Sub
End Class
