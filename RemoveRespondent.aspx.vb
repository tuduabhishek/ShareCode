Imports System.Data.OracleClient
Imports System.Data
Partial Class RemoveRespondent
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "REMOVE RESPONDENT"
                getFy()
                getsrlno()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub getFy()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("IRC_DESC").ToString()
                txtYear.Text = ViewState("FY")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub getsrlno()
        Try
            Dim mycommand As New OracleCommand
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            mycommand.CommandText = "select IRC_DESC from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
            Dim dtsrl = getRecordInDt(mycommand, conHrps)
            If dtsrl.Rows.Count > 0 Then
                ViewState("SRLNO") = dtsrl.Rows(0)("IRC_DESC").ToString()
                txtCycle.Text = ViewState("SRLNO")
            End If
        Catch ex As Exception


        End Try
    End Sub
    Public Sub SessionTimeOut()
        If Session("ADM_USER") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Exit Sub
        Else



        End If
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Public Function getRecordInDt(ByVal cmd As OracleCommand, ByVal cn As OracleConnection) As DataTable
        Dim dt As New DataTable()
        Try
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim da As New OracleDataAdapter(cmd.CommandText, cn)
            da.Fill(dt)

        Catch ex As Exception
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function
    Public Function getDataInDt(ByVal cmd1 As OracleCommand) As DataTable
        Dim dt As New DataTable()
        Try
            'If cn.State = ConnectionState.Closed Then
            '    cn.Open()
            'End If
            Dim da As New OracleDataAdapter(cmd1)
            da.Fill(dt)

        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function
    Protected Sub btnR_Click(sender As Object, e As EventArgs)

        'WI624: super admin can remove respondent ffrom list even if respondent list has been approved.
        'created by: Avik Mukherjee
        'Created on: 16-06-2021
        Dim ls_sql1 As String = String.Empty
        Dim pnoid As String = String.Empty
        Try
            If txtYear.Text.Trim = "" Or txtCycle.Text.Trim = "" Or txtassespno.Text.Trim = "" Or txtrespno.Text = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Respondent Per. No. and Assesee per. no. field can not be blank.")
                Exit Sub
            End If
            SessionTimeOut()
            Dim upres As New OracleCommand()
            If txtrespno.Text.ToUpper().Trim().Length > 6 Then
                Dim cmd5 As OracleCommand
                ls_sql1 = "Select SS_PNO from hrps.t_Survey_status where upper(SS_EMAIL)=:SS_EMAIL and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO and SS_WFL_STATUS<>'3' and ss_srlno=:ss_srlno"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                cmd5 = New OracleCommand(ls_sql1, conHrps)
                cmd5.Parameters.Clear()
                cmd5.Parameters.AddWithValue("SS_EMAIL", txtrespno.Text.ToUpper().Trim)
                cmd5.Parameters.AddWithValue("SS_YEAR", txtYear.Text.Trim)
                cmd5.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim)
                cmd5.Parameters.AddWithValue("ss_srlno", txtCycle.Text.Trim.ToString)
                Dim da5 As New OracleDataAdapter(cmd5)
                Dim dt5 As New DataTable
                da5.Fill(dt5)
                If dt5.Rows.Count > 0 Then
                    pnoid = dt5.Rows(0).Item(0)
                Else

                End If
            Else
                pnoid = txtrespno.Text.ToUpper().Trim()
            End If


            upres.CommandText = "delete from hrps.t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and (upper(SS_PNO)=:SS_PNO) and "
            upres.CommandText += "ss_wfl_status in('2','1','0','9') and ss_year=:ss_yar and SS_SRLNO=:SS_SRLNO"

            upres.Connection = conHrps
            upres.Parameters.Clear()
            upres.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim)
            upres.Parameters.AddWithValue("SS_PNO", pnoid)
            upres.Parameters.AddWithValue("ss_yar", txtYear.Text.Trim.ToString())
            upres.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.Trim.ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result = upres.ExecuteNonQuery()
            txtassespno.Text = ""
            txtrespno.Text = ""
            If result > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Respondent has been removed Successfully!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot remove selected respondent...!")
            End If

        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        'WI624: End of code, Created By: Avik Mukherjee, created On: 16-06-2021
    End Sub
End Class
