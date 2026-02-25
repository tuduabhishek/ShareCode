Imports System.Data.OracleClient
Imports System.Data
Partial Class RevertFeedback
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "REVERT FEEDBACK"
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
    Protected Sub btnrevert_Click(sender As Object, e As EventArgs)
        Dim ls_sql1 As String = String.Empty
        Dim pnoid As String = String.Empty
        Try
            If txtYear.Text.Trim = "" Or txtCycle.Text.Trim = "" Or txtassespno.Text.Trim = "" Or txtrespno.Text = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Respondent Per. No. and Assesee per. no. field can not be blank.")
                Exit Sub
            End If
            SessionTimeOut()
            'Added by TCS on 13122023, Added Role based validation
            If Session("LOGINID") Is Nothing Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
                Exit Sub
            End If
            If Session("LOGINID") <> "SA" Then
                If Not CheckTransactionValid() Then
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "You are not authorised to take the action for entered personal number")
                    Exit Sub
                End If
            End If
            'End
            Dim upres As New OracleCommand()
            If txtrespno.Text.ToUpper().Trim().Length > 6 Then
                Dim cmd5 As OracleCommand
                ls_sql1 = "Select SS_PNO from hrps.t_Survey_status where upper(SS_EMAIL)=:SS_EMAIL and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                cmd5 = New OracleCommand(ls_sql1, conHrps)
                cmd5.Parameters.Clear()
                cmd5.Parameters.AddWithValue("SS_EMAIL", txtrespno.Text.ToUpper().Trim)
                cmd5.Parameters.AddWithValue("SS_YEAR", txtYear.Text.Trim)
                cmd5.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim)
                cmd5.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.Trim)
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


            upres.CommandText = "update hrps.t_survey_status  set ss_wfl_status=:ss_wfl_status1,SS_Q2_A=null,SS_Q2_B=null, SS_UPDATED_DT=sysdate,SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO=:SS_ASSES_PNO and (upper(SS_PNO)=:SS_PNO) and "
            upres.CommandText += "ss_wfl_status in('3','9') and ss_year=:ss_yar and SS_SRLNO=:SS_SRLNO"

            upres.Connection = conHrps
            upres.Parameters.Clear()
            upres.Parameters.AddWithValue("ss_wfl_status1", "2")
            upres.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim)
            upres.Parameters.AddWithValue("SS_PNO", pnoid)
            upres.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            ' upres.Parameters.AddWithValue("ss_wfl_status", s)
            upres.Parameters.AddWithValue("ss_yar", txtYear.Text.Trim.ToString())
            upres.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.Trim.ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result = upres.ExecuteNonQuery()

            upres.CommandText = ""
            'upres.CommandText = "UPDATE hrps.t_survey_response SET ss_flag='D',SS_MODIFIED_BY=:SS_MODIFIED_BY,SS_MODIFIED_DT=SYSDATE where SS_ASSES_PNO=:SS_ASSES_PNO and SS_PNO=:SS_PNO and SS_SERIAL=:SS_SERIAL and SS_YEAR=:SS_YEAR "
            upres.CommandText = "delete from hrps.t_survey_response where SS_ASSES_PNO=:SS_ASSES_PNO and SS_PNO=:SS_PNO and SS_SERIAL=:SS_SERIAL and SS_YEAR=:SS_YEAR "
            upres.Connection = conHrps
            upres.Parameters.Clear()
            'upres.Parameters.AddWithValue("SS_MODIFIED_BY", Session("ADM_USER").ToString())
            upres.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim())
            upres.Parameters.AddWithValue("SS_PNO", pnoid.Trim)
            upres.Parameters.AddWithValue("SS_YEAR", txtYear.Text.Trim.ToString())
            upres.Parameters.AddWithValue("SS_SERIAL", txtCycle.Text.Trim.ToString())
            Dim result1 = upres.ExecuteNonQuery()

            txtassespno.Text = ""
            txtrespno.Text = ""
            If result1 > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Feedback has been reverted...!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No feedback was provided hence it cannot be reverted.")
            End If

        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Function CheckTransactionValid() As Boolean
        Dim isValidforTransaction As Boolean
        Try
            Dim strQuery As String
            If Session("LOGINID") = "LD" Then
                strQuery = "SELECT EMA_PERNO FROM T_EMP_MASTER_FEEDBACK360 WHERE EMA_YEAR= :YEAR AND EMA_CYCLE= :CYCLE AND EMA_PERNO = :PERNO AND EMA_EQV_LEVEL IN ('I1','I2','TG')"
            ElseIf Session("LOGINID") = "HRBP" Then
                strQuery = "SELECT EMA_PERNO FROM T_EMP_MASTER_FEEDBACK360 WHERE EMA_YEAR= :YEAR AND EMA_CYCLE= :CYCLE AND EMA_PERNO = :PERNO AND EMA_BHR_PNO = :BUHR"
            End If


            Dim ocmd1 As New OracleCommand()
            ocmd1.CommandText = strQuery
            ocmd1.Parameters.Clear()
            ocmd1.Connection = conHrps
            ocmd1.Parameters.AddWithValue("YEAR", ViewState("FY").ToString())
            ocmd1.Parameters.AddWithValue("CYCLE", ViewState("SRLNO").ToString())
            ocmd1.Parameters.AddWithValue("PERNO", txtassespno.Text.Trim)
            If Session("LOGINID") = "HRBP" Then
                ocmd1.Parameters.AddWithValue("BUHR", Session("ADM_USER").ToString())
            End If
            Dim da As New OracleDataAdapter(ocmd1)
            Dim vc As New DataTable()
            da.Fill(vc)
            If vc.Rows.Count > 0 Then
                isValidforTransaction = True
            Else
                isValidforTransaction = False
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return isValidforTransaction
    End Function
End Class
