Imports System.Data.OracleClient
Imports System.Data
Imports System
Imports System.IO
Partial Class Activate_Page
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Private Sub Activate_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CheckRole()
            LoadPageName()
        End If
    End Sub

    Public Sub LoadPageName()
        Try

            Dim mycommand As New OracleCommand
            Dim sql As String = "select IRC_CODE ""Code"",IRC_CODE ||'-'|| IRC_DESC ""Desc"" from hrps.t_ir_codes where irc_type =:irc_type and irc_desc like :irc_desc "
            mycommand.CommandText = sql
            mycommand.Parameters.Clear()
            mycommand.Parameters.Add(New OracleParameter("irc_type", "360PG"))
            mycommand.Parameters.Add(New OracleParameter("irc_desc", "%" & "OPR" & "%"))
            Dim dt As DataTable = getRecord(mycommand, conHrps)
            If dt.Rows.Count > 0 Then
                ddlPageNm.DataSource = dt
                ddlPageNm.DataTextField = "Desc"
                ddlPageNm.DataValueField = "Code"
                ddlPageNm.DataBind()
                ddlPageNm.Items.Insert(0, New ListItem("[Select]", ""))
            End If

        Catch ex As Exception

        End Try

    End Sub
    Private Sub ddlPageNm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPageNm.SelectedIndexChanged
        If ddlPageNm.SelectedValue = "" Then
            txtStartDt.Text = ""
            txtEndDt.Text = ""
            ddlStatus.SelectedIndex = -1
        Else
            Dim mycommand As New OracleCommand
            Dim sqlRole As String = "select trunc(IRC_START_DT) StDt,trunc(IRC_END_DT) EndDt,IRC_VALID_TAG Status  from hrps.t_ir_codes where irc_type =:irc_type and irc_code=:irc_code and upper(irc_code ||'-'|| irc_desc)=:irc_desc "
            mycommand.CommandText = sqlRole
            mycommand.Parameters.Clear()
            mycommand.Parameters.Add(New OracleParameter("irc_type", "360PG"))
            mycommand.Parameters.Add(New OracleParameter("irc_code", ddlPageNm.SelectedValue))
            mycommand.Parameters.Add(New OracleParameter("irc_desc", ddlPageNm.SelectedItem.Text.ToUpper))
            Dim dt As DataTable = getRecord(mycommand, conHrps)
            If dt.Rows.Count > 0 Then
                txtStartDt.Text = dt.Rows(0)("StDt")
                txtEndDt.Text = dt.Rows(0)("EndDt")
                ddlStatus.SelectedValue = dt.Rows(0)("Status")
            Else
                txtStartDt.Text = ""
                txtEndDt.Text = ""
                ddlStatus.SelectedIndex = -1
            End If

        End If
    End Sub

    Public Function getRecord(ByVal cmd As OracleCommand, ByVal cn As OracleConnection) As DataTable
        Dim dt As New DataTable()
        Try
            cmd.Connection = cn
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim da As New OracleDataAdapter(cmd)
            da.Fill(dt)

            da.Dispose()
        Catch ex As Exception
            ex.Message.ToString()
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim sqlUpdateStatus As String = ""
            Dim myCmd As New OracleCommand
            sqlUpdateStatus = "UPDATE HRPS.T_IR_CODES SET IRC_START_DT=to_date(:IRC_START_DT,'mm/dd/yyyy'), IRC_END_DT=to_date(:IRC_END_DT,'mm/dd/yyyy'), IRC_VALID_TAG=:IRC_VALID_TAG, IRC_CHANGE_USER=:IRC_CHANGE_USER, IRC_CHANGE_DATE=sysdate where upper(irc_code ||'-'|| irc_desc)=:IRC_DESC"
            myCmd.Parameters.Clear()
            myCmd.Parameters.Add(New OracleParameter("IRC_START_DT", txtStartDt.Text))
            myCmd.Parameters.Add(New OracleParameter("IRC_END_DT", txtEndDt.Text))
            myCmd.Parameters.Add(New OracleParameter("IRC_VALID_TAG", ddlStatus.SelectedValue))
            myCmd.Parameters.Add(New OracleParameter("IRC_CHANGE_USER", Session("USER_ID").ToString()))
            myCmd.Parameters.Add(New OracleParameter("IRC_DESC", ddlPageNm.SelectedItem.Text.ToUpper))

            myCmd.Connection = conHrps
            myCmd.CommandText = sqlUpdateStatus
            myCmd.ExecuteNonQuery()

            ddlPageNm.SelectedIndex = -1
            txtStartDt.Text = ""
            txtEndDt.Text = ""
            ddlStatus.SelectedIndex = -1
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Success", "showMsgSuccess();", True)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me, Me.[GetType](), "Success", "showMsgError();", True)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub CheckRole()
        Try
            Dim mycommand As New OracleCommand

            Dim sqlRole As String = "select distinct IGP_user_id from t_ir_adm_grp_privilege where igp_group_id =:igp_group_id "
            sqlRole += " and IGP_STATUS =:IGP_STATUS and IGP_user_id=:IGP_user_id"
            mycommand.CommandText = sqlRole
            mycommand.Parameters.Clear()
            mycommand.Parameters.Add(New OracleParameter("igp_group_id", "360FEEDBAC"))
            mycommand.Parameters.Add(New OracleParameter("IGP_STATUS", "A"))
            mycommand.Parameters.Add(New OracleParameter("IGP_user_id", Session("USER_ID").ToString()))
            Dim dt As DataTable = getRecord(mycommand, conHrps)
            If dt.Rows.Count > 0 Then

            Else
                Response.Redirect("errorpage.aspx", True)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
