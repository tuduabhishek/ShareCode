Imports System.Data.OracleClient
Imports System.Data
Partial Class ReturnToAssesse
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "RETURN TO ASSESSE"
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
    Protected Sub btnreset_Click(sender As Object, e As EventArgs)
        Try
            If txtYear.Text.Trim = "" Or txtCycle.Text.Trim = "" Or txtperno.Text.Trim = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Year,Cycle and Perno field can not be blank.")
                Exit Sub
            End If
            SessionTimeOut()
            If txtperno.Text.Trim <> "" Then

                Dim chk = checkreturneligible(txtperno.Text.Trim.ToUpper(), txtCycle.Text.Trim.ToString(), txtYear.Text.Trim)
                If chk = "N" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot return the form. It is already approved")
                    Exit Sub
                End If
                Dim chksurvey = checkreturneligiblesub(txtperno.Text.Trim.ToUpper(), txtCycle.Text.Trim.ToString(), txtYear.Text.Trim)
                If chksurvey = "Y" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot return the form.It is not submitted by assesse")
                    Exit Sub
                End If
                Dim com As New OracleCommand()
                Dim qry As String = String.Empty
                qry = "update hrps.t_survey_status set ss_tag='N',ss_approver=null,ss_tag_dt=null, ss_wfl_status=null,SS_UPDATED_BY=:SS_UPDATED_BY,SS_UPDATED_DT=sysdate where ss_year=:ss_year"
                qry += "  and ss_tag='SU' and upper(ss_asses_pno)=:ss_asses_pno and  SS_SRLNO =:SS_SRLNO and ss_app_tag is null"
                com = New OracleCommand(qry, conHrps)
                com.Parameters.Clear()
                com.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
                com.Parameters.AddWithValue("ss_year", txtYear.Text.Trim.ToString())
                com.Parameters.AddWithValue("ss_asses_pno", txtperno.Text.Trim.ToUpper())
                com.Parameters.AddWithValue("SS_SRLNO", txtCycle.Text.Trim().ToString())
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim r = com.ExecuteNonQuery()
                If r = 0 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Incorrect P.no entred...!")
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
    End Sub
    Private Function checkreturneligible(ByVal pno As String, ByVal serial As String, ByVal yr As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmdeligible As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            cmdeligible.CommandText = "select ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and ss_asses_pno=:pno and  SS_SRLNO=:SS_SRLNO and ss_app_tag='AP' and ss_year=:ss_year"
            cmdeligible.Connection = conHrps
            cmdeligible.Parameters.Clear()
            cmdeligible.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmdeligible.Parameters.Add(New OracleParameter("SS_SRLNO", serial.Trim))
            cmdeligible.Parameters.Add(New OracleParameter("ss_year", yr.Trim))
            dt = getDataInDt(cmdeligible)
            If dt.Rows.Count > 0 Then
                st = "N"
            Else
                st = "Y"
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return st
    End Function
    Private Function checkreturneligiblesub(ByVal pno As String, ByVal serial As String, ByVal yr As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmd As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            cmd.CommandText = "select distinct ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and ss_asses_pno=:pno and  SS_SRLNO =:SS_SRLNO and ss_app_tag is null and (SS_WFL_STATUS='1' or SS_WFL_STATUS is null) and ss_year=:ss_year"
            'If conHrps.State = ConnectionState.Closed Then
            '    conHrps.Open()
            'End If

            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmd.Parameters.Add(New OracleParameter("SS_SRLNO", serial.Trim))
            cmd.Parameters.Add(New OracleParameter("ss_year", yr.Trim))
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
