Imports System.Data.OracleClient
Imports System.Data
Partial Class ChangeApprover
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "CHANGE APPROVER"
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
    Protected Sub txtCngAppAssesPerNo_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtCngAppAssesPerNo.Text.Trim()
            Dim strself As New OracleCommand()
            'Added by TCS on 01122022 to fix Reporting perno column as approver
            'strself.CommandText = "select decode(ema_eqv_level,'I2',ema_dotted_pno,ema_reporting_to_pno) EMA_REPORTING_TO_PNO from hrps.t_emp_master_feedback360 where "
            strself.CommandText = "select EMA_REPORTING_TO_PNO from hrps.t_emp_master_feedback360 where "
            'End
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " ema_perno =:pno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
            'End by Manoj Kumar on 31-05-2021
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            strself.Parameters.Add(New OracleParameter("SS_YEAR", txtYear.Text.ToString()))
            strself.Parameters.Add(New OracleParameter("SS_SRLNO", txtCycle.Text.ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtCngExistingApprover.Text = dt.Rows(0)("EMA_REPORTING_TO_PNO").ToString()
            Else
                txtCngExistingApprover.Text = ""
                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Not show Approver Per. no.")
                Exit Sub
            End If

        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnChangeApproval_Click(sender As Object, e As EventArgs)
        Try
            SessionTimeOut()
            Dim chk = checkAssessData(txtCngAppAssesPerNo.Text)
            If chk = "Y" Then
                'Added by TCS on 01122022 to update approver in master table if there is not any record in transaction table
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim cmd As New OracleCommand
                cmd.CommandText = "select * from t_emp_master_feedback360 where ema_perno=:ema_perno and ema_year=:ema_year and ema_cycle=:ema_cycle"
                cmd.Connection = conHrps
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("ema_perno", txtCngAppAssesPerNo.Text.Trim.ToString())
                cmd.Parameters.AddWithValue("ema_year", txtYear.Text.ToString())
                cmd.Parameters.AddWithValue("ema_cycle", txtCycle.Text.ToString())
                Dim dt = getDataInDt(cmd)
                If dt.Rows.Count > 0 Then
                    cmd = New OracleCommand
                    cmd.CommandText = "update hrps.t_emp_master_feedback360 set EMA_REPORTING_TO_PNO =:EMA_REPORTING_TO_PNO where ema_perno=:ema_perno and ema_YEAR=:SS_YEAR and ema_cycle=:SS_SRLNO"
                    cmd.Connection = conHrps
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.ToString())
                    cmd.Parameters.AddWithValue("EMA_REPORTING_TO_PNO", txtCngAppPerNO.Text.Trim.ToString)
                    cmd.Parameters.AddWithValue("ema_perno", txtCngAppAssesPerNo.Text.Trim.ToString())
                    cmd.Parameters.AddWithValue("SS_YEAR", txtYear.Text.ToString())
                    Dim isExecuted = cmd.ExecuteNonQuery()
                    If isExecuted > 0 Then
                        ShowGenericMessageModal(CommonConstants.AlertType.success, "Data updated successfully in Master table only.")
                        Exit Sub
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Data not updated.")
                        Exit Sub
                    End If
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Assesse record not available, please check and try again.")
                    Exit Sub
                End If
                'End
            End If
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand
            comnd.CommandText = "update hrps.t_emp_master_feedback360 set EMA_REPORTING_TO_PNO =:EMA_REPORTING_TO_PNO where ema_perno=:ema_perno and ema_YEAR=:SS_YEAR and ema_cycle=:SS_SRLNO"
            comnd.Connection = conHrps
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.ToString())
            comnd.Parameters.AddWithValue("EMA_REPORTING_TO_PNO", txtCngAppPerNO.Text.Trim.ToString)
            comnd.Parameters.AddWithValue("ema_perno", txtCngAppAssesPerNo.Text.Trim.ToString())
            comnd.Parameters.AddWithValue("SS_YEAR", txtYear.Text.ToString())
            Dim result = comnd.ExecuteNonQuery()

            Dim getExisting = GetApprover(txtYear.Text.ToString(), txtCngAppPerNO.Text.Trim.ToString, txtCycle.Text.ToString())

            If getExisting.Rows.Count > 0 Then
                'Modified by TCS on 05122023, Removing SS_APP_TAG='AP' from Query
                'comnd.CommandText = "update hrps.t_survey_status set SS_PNO =:SS_PNO, SS_NAME=:SS_NAME, SS_DESG=:SS_DESG,SS_DEPT=:SS_DEPT,SS_EMAIL=:SS_EMAIL where SS_ASSES_PNO=:SS_ASSES_PNO AND SS_PNO =:SS_PNO1 and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO AND SS_CATEG='MANGR' AND SS_APP_TAG='AP'"
                comnd.CommandText = "update hrps.t_survey_status set SS_PNO =:SS_PNO, SS_NAME=:SS_NAME, SS_DESG=:SS_DESG,SS_DEPT=:SS_DEPT,SS_EMAIL=:SS_EMAIL where SS_ASSES_PNO=:SS_ASSES_PNO AND SS_PNO =:SS_PNO1 and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO AND SS_CATEG='MANGR'"
                comnd.Connection = conHrps
                comnd.Parameters.Clear()
                comnd.Parameters.AddWithValue("SS_PNO", txtCngAppPerNO.Text.Trim.ToString)
                comnd.Parameters.AddWithValue("SS_NAME", getExisting.Rows(0)("EMA_ENAME").ToString)
                comnd.Parameters.AddWithValue("SS_DESG", getExisting.Rows(0)("EMA_DESGN_DESC").ToString)
                comnd.Parameters.AddWithValue("SS_DEPT", getExisting.Rows(0)("EMA_DEPT_DESC").ToString)
                comnd.Parameters.AddWithValue("SS_EMAIL", getExisting.Rows(0)("EMA_EMAIL_ID").ToString)
                comnd.Parameters.AddWithValue("SS_PNO1", txtCngExistingApprover.Text.Trim.ToString)
                comnd.Parameters.AddWithValue("SS_YEAR", txtYear.Text.ToString())
                comnd.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.ToString())
                comnd.Parameters.AddWithValue("SS_ASSES_PNO", txtCngAppAssesPerNo.Text.Trim.ToString())

                Dim chk1 = comnd.ExecuteNonQuery()
            End If

            comnd.CommandText = "update hrps.t_survey_status set SS_APPROVER =:SS_APPROVER, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO=:SS_ASSES_PNO and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO"
            comnd.Connection = conHrps
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.ToString())
            comnd.Parameters.AddWithValue("SS_APPROVER", txtCngAppPerNO.Text.Trim.ToString)
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", txtCngAppAssesPerNo.Text.Trim.ToString())
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            comnd.Parameters.AddWithValue("SS_YEAR", txtYear.Text.ToString())
            Dim result1 = comnd.ExecuteNonQuery()
            If result1 > 0 Then
                txtCngAppPerNO.Text = ""
                txtCngAppAssesPerNo.Text = ""
                txtCngExistingApprover.Text = ""
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Successfully updated!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Not be updated...!")
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Private Function checkAssessData(ByVal pno As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmd As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            cmd.CommandText = "select * from hrps.t_survey_status where ss_asses_pno=:pno and  SS_SRLNO =:SS_SRLNO and SS_YEAR=:SS_YEAR"
            'If conHrps.State = ConnectionState.Closed Then
            '    conHrps.Open()
            'End If

            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmd.Parameters.Add(New OracleParameter("SS_SRLNO", txtCycle.Text.ToString))
            cmd.Parameters.Add(New OracleParameter("SS_YEAR", txtYear.Text.ToString))
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
    Private Function GetApprover(ByVal SS_YEAR As String, ByVal ApproverPno As String, ByVal SS_SRLNO As String) As DataTable
        Dim res As String = ""
        Dim dt As New DataTable
        Try
            Dim query As String
            query = "select * from hrps.t_emp_master_feedback360 where EMA_YEAR=:SS_YEAR and EMA_PERNO=:SS_ASSES_PNO And EMA_CYCLE=:SS_SRLNO"
            Dim cmd As New OracleCommand(query, conHrps)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", ApproverPno.ToString())
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())

            Dim adp As New OracleDataAdapter(cmd)
            adp.Fill(dt)
        Catch ex As Exception

        End Try
        Return dt
    End Function
End Class
