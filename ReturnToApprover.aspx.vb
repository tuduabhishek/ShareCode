Imports System.Data.OracleClient
Imports System.Data
Partial Class ReturnToApprover
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "RETURN TO APPROVER"
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
    Protected Sub btn_revertapp_Click(sender As Object, e As EventArgs)
        Try
            If txtYear.Text.Trim = "" Or txtCycle.Text.Trim = "" Or txtperno.Text.Trim = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Year,Cycle and Perno field can not be blank.")
                Exit Sub
            End If
            SessionTimeOut()
            If txtperno.Text.Trim <> "" Then
                Dim com As New OracleCommand()
                Dim chk = checkreverteligible(txtperno.Text.Trim.ToUpper(), txtCycle.Text.Trim.ToString(), txtYear.Text.Trim)
                If chk = "Y" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot revert the form survey already started")
                    Exit Sub
                End If
                Dim chksurvey = checkreverteligiblesub(txtperno.Text.Trim.ToUpper(), txtCycle.Text.Trim.ToString(), txtYear.Text.Trim)
                If chksurvey = "N" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot revert the form.It is not approved by manager")
                    Exit Sub
                End If
                Dim qry As String = String.Empty
                qry = "update hrps.t_survey_status set ss_wfl_status='1',ss_app_tag=null,SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "',SS_UPDATED_DT=sysdate where ss_year='" & txtYear.Text.Trim.ToString() & "'"
                qry += "  and ss_tag='SU' and upper(ss_asses_pno)='" & txtperno.Text.Trim.ToUpper() & "' and  SS_SRLNO ='" & txtCycle.Text.Trim.ToString() & "' and ss_app_tag='AP'"
                com = New OracleCommand(qry, conHrps)

                'dtsrl = getRecordInDt(com, conHrps)

                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim r = com.ExecuteNonQuery()

                If r = 0 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Incorrect P.no entred...!")  'WI368 execute query sow correct message'
                Else
                    txtperno.Text = ""
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Returned...!")
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter Personal number...!")
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        '''WI447: end of function to revert back survey to approver
    End Sub
    Public Function checkreverteligible(ByVal pno As String, srl As String, ByVal yr As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmdeligible As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            '''''WI447 : to check any record exists in t_survey status with feedback provided, Created By: Avik Mukherjee, Date: 04-06-2021

            cmdeligible.CommandText = "select ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and ss_asses_pno=:pno and  SS_SRLNO=:SS_SRLNO and ss_app_tag='AP' and ss_wfl_status='3' and SS_YEAR=:SS_YEAR"

            cmdeligible.Connection = conHrps
            cmdeligible.Parameters.Clear()
            cmdeligible.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmdeligible.Parameters.Add(New OracleParameter("SS_SRLNO", srl.Trim))
            cmdeligible.Parameters.Add(New OracleParameter("SS_YEAR", yr))
            '''''''WI447: end of code
            dt = getDataInDt(cmdeligible)
            If dt.Rows.Count > 0 Then
                st = "Y"
            Else
                st = "N"
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return st

    End Function
    Private Function checkreverteligiblesub(ByVal pno As String, ByVal serial As String, ByVal YR As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmd As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            cmd.CommandText = "select distinct ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and ss_asses_pno=:pno and  SS_SRLNO =:SS_SRLNO and ss_app_tag is null and SS_WFL_STATUS='1' and ss_year=:ss_year"
            'If conHrps.State = ConnectionState.Closed Then
            '    conHrps.Open()
            'End If

            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmd.Parameters.Add(New OracleParameter("SS_SRLNO", serial.Trim))
            cmd.Parameters.Add(New OracleParameter("ss_year", YR.Trim))
            dt = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                st = "N"
            Else
                st = "Y"
            End If
        Catch ex As Exception
            ' MsgBox(ex.Message)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return st
    End Function
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
End Class
