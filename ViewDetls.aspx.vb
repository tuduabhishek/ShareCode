Imports System.Data.OracleClient
Imports System.Data
Imports System
Imports System.IO
Partial Class ViewDetls
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Private Sub Activate_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        End If
    End Sub


    Protected Sub btnsearch_Click(sender As Object, e As EventArgs)
        Try

            If rbbtton.SelectedValue = "res" Then

                GetDataRes(txtrespno.Text.Trim.ToUpper())
            ElseIf rbbtton.SelectedValue = "asse" Then

                GetDataAss(txtrespno.Text.Trim.ToUpper())
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub GetDataAss(pno As String)
        Try
            Dim strqry As String = String.Empty
            strqry = " select SS_YEAR,SS_ASSES_PNO Assessepno,decode(SS_CATEG,'INTSH','Internal Stakeholder','MANGR','Manager/Superior','PEER','Peer','ROPT','Subordinates',SS_CATEG)"
            strqry += " cat,SS_ID ids,SS_PNO respno,SS_NAME resname,SS_DESG desg,SS_DEPT dept,SS_EMAIL mail ,decode(SS_DEL_TAG ,'Y','Yes','N','No',SS_DEL_TAG) "
            strqry += " del,SS_APPROVER approver,SS_APP_TAG approved,decode(SS_WFL_STATUS,'1','Submitted','2','Approved','3','Completed',SS_WFL_STATUS) status from t_survey_status"
            strqry += " where upper(SS_ASSES_PNO) LIKE :SS_ASSES_PNO "
            Dim cmd As New OracleCommand()
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", "%" & pno.ToUpper() & "%")
            cmd.CommandText = strqry

            cmd.Connection = conHrps
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim adp As New OracleDataAdapter(cmd)
            Dim dt As New DataTable()
            adp.Fill(dt)
            If dt.Rows.Count > 0 Then
                GvManager.DataSource = dt
                GvManager.DataBind()
            End If
        Catch ex As Exception
            Dim g = ex.ToString()
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub GetDataRes(pno As String)
        Try
            Dim strqry As String = String.Empty
            strqry = " select SS_YEAR,SS_ASSES_PNO Assessepno,decode(SS_CATEG,'INTSH','Internal Stakeholder','MANGR','Manager/Superior','PEER','Peer','ROPT','Subordinates',SS_CATEG)"
            strqry += " cat,SS_ID ids,SS_PNO respno,SS_NAME resname,SS_DESG desg,SS_DEPT dept,SS_EMAIL mail ,decode(SS_DEL_TAG ,'Y','Yes','N','No',SS_DEL_TAG) "
            strqry += " del,SS_APPROVER approver,SS_APP_TAG approved,decode(SS_WFL_STATUS,'1','Submitted','2','Approved','3','Completed',SS_WFL_STATUS) status from t_survey_status"
            strqry += " where (upper(SS_PNO)  like :SS_PNO or upper(SS_EMAIL) like :SS_PNO)"
            Dim cmd As New OracleCommand()
            cmd.CommandText = strqry
            cmd.Parameters.Clear()
            cmd.Connection = conHrps
            cmd.Parameters.AddWithValue("SS_PNO", "%" & pno.ToUpper() & "%")
            Dim adp As New OracleDataAdapter(cmd)
            Dim dt As New DataTable()
            adp.Fill(dt)
            If dt.Rows.Count > 0 Then
                GvManager.DataSource = dt
                GvManager.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub rbbtton_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            If rbbtton.SelectedValue = "res" Then
                txtrespno.Text = ""
                lbltype.Text = "Pno/Email"
            ElseIf rbbtton.SelectedValue = "asse" Then
                txtrespno.Text = ""
                lbltype.Text = "Pno"
            Else
                lbltype.Text = ""
                txtrespno.Text = ""
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
