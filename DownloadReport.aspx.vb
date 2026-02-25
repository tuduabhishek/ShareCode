Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Partial Class DownloadReport
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Public Function SessionTimeOut() As Boolean
        If Session("USER_ID") Is Nothing Or Session("label") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub Rpt_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("errorMsg") = "The page you were looking for is not valid."
        Response.Redirect("errorpage.aspx", True)
        Exit Sub
        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        Dim vUserFullName As String = Page.User.Identity.Name   '155710'

        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If

        If strUserID = "163523" Then
            strUserID = "123444"
        ElseIf strUserID = "148536" Then
            strUserID = "123444"
        End If

        Dim isvalidpage = PageValid()
        If isvalidpage = True Then
        Else
            Response.Write("<center> <b><I> This website has been closed </b></I></center>")
            Me.Page.Visible = False
            Exit Sub
        End If


        getsrlno()
        GetFy()
        Session("USER_DOMAIN") = strUserDomain.ToUpper()

        Session("USER_ID") = strUserID.ToUpper()
        If GetPno(strUserID.ToUpper()) = False Then
            Session("errorMsg") = "This report has not been activated for you."
            Response.Redirect("errorpage.aspx", True)
        End If
        If Not IsPostBack Then
            bindYear()
            bindSrlno()
        End If

        'Dim dc As New OracleCommand()
        'dc.CommandText = "select * from t_survey_status where ss_asses_pno=:pno and ss_rpt_flag='Y' and ss_wfl_status='3'"
        'dc.Parameters.Clear()
        'dc.Connection = conHrps
        'dc.Parameters.AddWithValue("pno", strUserID)
        'Dim f1 = getDataInDt(dc)

        'Dim dc1 As New OracleCommand()
        'dc1.CommandText = "select ema_perno , EMA_EQV_LEVEL from tips.t_empl_all where ema_perno=:pno1 "
        'dc1.Parameters.Clear()
        'dc1.Connection = conHrps
        'dc1.Parameters.AddWithValue("pno1", strUserID)
        'Dim f = getDataInDt(dc1)

        'If f.Rows.Count > 0 Then
        '    If f.Rows(0)(1) = "I3" Then
        '        If checkcateg(strUserID, f.Rows(0)(1)) Then
        '            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
        '                Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(f.Rows(0)(0)))
        '            Else
        '                updateTag(strUserID)
        '                Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(f.Rows(0)(0)))
        '            End If

        '        Else
        '            Response.Write("<center> <b><I> The report could not be downloaded because the minimum criteria was not met. </b></I></center>")
        '        End If

        '    ElseIf f.Rows(0)(1) = "I4" Or f.Rows(0)(1) = "I5" Or f.Rows(0)(1) = "I6" Then

        '        If checkcateg(strUserID, f.Rows(0)(1)) Then
        '            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
        '                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(f.Rows(0)(0)))
        '            Else
        '                updateTag(strUserID)
        '                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(f.Rows(0)(0)))
        '            End If


        '        Else
        '            Response.Write("<center> <b><I> The report could not be downloaded because the minimum criteria was not met. </b></I></center>")
        '        End If

        '    End If
        'Else
        '    Response.Write("<center> <b><I> Report not downloaded for you </b></I></center>")
        '    Me.Page.Visible = False
        '    Exit Sub
        'End If

    End Sub
    Private Sub bindYear()
        Try
            Dim dtExecHead As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "SELECT distinct '20'||substr(irc_code,1,2) FY FROM HRPS.T_IR_CODES WHERE IRC_TYPE ='36PDF' and '20'||substr(irc_code,1,2)='" & ViewState("FY") & "' AND IRC_VALID_TAG='A'"
            query.CommandText += " order by FY"
            dtExecHead = getDataInDt(query)
            If dtExecHead.Rows.Count > 0 Then
                ddlYear.DataSource = dtExecHead
                ddlYear.DataValueField = "FY"
                ddlYear.DataTextField = "FY"
                ddlYear.DataBind()
                ddlYear.Items.Insert(0, "--Select--")
                ddlCycle.Items.Insert(0, "--Select--")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bindSrlno()
        Try
            Dim dtCyc As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "SELECT substr(irc_code,3,1) CYC,irc_desc FROM HRPS.T_IR_CODES WHERE IRC_TYPE ='36PDF' and substr(irc_code,3,1)='" & ViewState("SRLNO").ToString & "' AND IRC_VALID_TAG='A'"
            query.CommandText += " order by CYC"
            dtCyc = getDataInDt(query)
            If dtCyc.Rows.Count > 0 Then
                ddlCycle.DataSource = dtCyc
                ddlCycle.DataValueField = "CYC"
                ddlCycle.DataTextField = "irc_desc"
                ddlCycle.DataBind()
                ddlCycle.Items.Insert(0, "--Select--")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function GetPno(pernr As String) As Boolean
        Dim perno As String = ""
        Try
            Dim d1 As New Boolean
            Dim cm As New OracleCommand()
            cm.CommandText = "  select EMA_ENAME from hrps.t_emp_master_feedback360  where EMA_YEAR=:yr and ema_perno=:ema_perno AND EMA_CYCLE=:cyc AND ema_eqv_level IN (SELECT IRC_DESC FROM HRPS.T_IR_CODES WHERE IRC_TYPE='360RT' AND IRC_VALID_TAG='A')"

            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("yr", ViewState("FY").ToString())
            cm.Parameters.AddWithValue("cyc", ViewState("SRLNO").ToString())
            cm.Parameters.AddWithValue("ema_perno", pernr.ToUpper().ToString())
            Dim da As New OracleDataAdapter(cm)
            Dim d As New DataTable()
            da.Fill(d)
            If d.Rows.Count > 0 Then
                lblname.Text = d.Rows(0)("EMA_ENAME").ToString()
                d1 = True
            Else
                lblname.Text = ""
                d1 = False
            End If
            Return d1
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

        'Return perno
    End Function
    Public Function PageValid() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            ocmd.CommandText = "select IRC_CODE from t_ir_codes where irc_type ='360PG' and trunc(IRC_START_DT) <= trunc(sysdate)"
            ocmd.CommandText += "  and trunc(IRC_END_DT) >= trunc(sysdate) and IRC_VALID_TAG='A' and upper(irc_desc)=UPPER('DownloadReport.ASPX')"
            Dim vc = getRecordInDt(ocmd, conHrps)
            If vc.Rows.Count > 0 Then
                isvalid = True
            Else
                isvalid = False
            End If
        Catch ex As Exception

        End Try
        Return isvalid
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
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Public Sub getsrlno()
        Try
            Dim srlno As New OracleCommand()
            srlno.CommandText = "select IRC_DESC from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
            Dim dtSrlno = getRecordInDt(srlno, conHrps)

            If dtSrlno.Rows.Count > 0 Then
                ViewState("SRLNO") = dtSrlno.Rows(0)(0)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub GetFy()
        Try
            Dim s As New OracleCommand()
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim f = getRecordInDt(s, conHrps)

            If f.Rows.Count > 0 Then
                ViewState("FY") = f.Rows(0)(0)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Button5_Click(sender As Object, e As EventArgs)
        Try
            If ddlYear.SelectedValue = "--Select--" Or ddlCycle.Text.Trim = "--Select--" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select option for search result")
                Exit Sub
            ElseIf Session("USER_ID") Is Nothing Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Else
                Dim qrycmd1 As New OracleCommand()
                qrycmd1.CommandText = "select * from t_survey_status where ss_asses_pno='" & Session("USER_ID").ToString & "' and ss_rpt_flag='Y' and ss_wfl_status='3' AND SS_SRLNO='" & ddlCycle.SelectedValue.ToString & "' AND SS_YEAR='" & ddlYear.SelectedValue.ToString & "'"
                Dim f1 = getRecordInDt(qrycmd1, conHrps)

                Dim qry As String = ""
                qry = "select ema_perno , EMA_EQV_LEVEL from hrps.t_emp_master_feedback360 where ema_perno='" & Session("USER_ID").ToString.Trim & "' AND EMA_CYCLE='" & ddlCycle.SelectedValue.ToString & "' AND EMA_YEAR='" & ddlYear.SelectedValue.ToString & "'"
                Dim qrycmd As New OracleCommand()
                qrycmd.CommandText = qry
                Dim gh = getRecordInDt(qrycmd, conHrps)
                If gh.Rows.Count > 0 Then

                    Dim qryMiniCriteria As New OracleCommand()
                    qryMiniCriteria.CommandText = "select irc_code,irc_desc from t_ir_codes where irc_type='" & ddlYear.Text.Substring(2, 2).ToString() & "" & ddlCycle.SelectedValue.ToString & "" & gh.Rows(0)(1) & "' and irc_valid_tag='A' order by irc_code"
                    Dim dtqryMiniCriteria = getRecordInDt(qryMiniCriteria, conHrps)
                    If dtqryMiniCriteria.Rows.Count > 0 Then

                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Cycle " & ddlCycle.SelectedValue.ToString & " is not active for this report.")
                        Exit Sub
                    End If

                    If gh.Rows(0)(1) = "I3" Or gh.Rows(0)(1) = "I1" Or gh.Rows(0)(1) = "I2" Then
                        If checkcateg(Session("USER_ID").ToString, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            Else
                                updateTag(Session("USER_ID").ToString)
                                Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            End If

                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                        'Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString))
                    ElseIf gh.Rows(0)(1) = "TG" Then
                        If checkcateg(Session("USER_ID").ToString, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                Response.Redirect("FeedbackSurveyRptTG_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                            Else
                                updateTag(Session("USER_ID").ToString)
                                Response.Redirect("FeedbackSurveyRptTG_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                            End If

                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                    ElseIf gh.Rows(0)(1) = "I4" Or gh.Rows(0)(1) = "I5" Or gh.Rows(0)(1) = "I6" Then
                        If checkcateg(Session("USER_ID").ToString, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            Else
                                updateTag(Session("USER_ID").ToString)
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            End If


                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                    End If
                Else
                    qry = "select ema_perno , EMA_EQV_LEVEL from tips.t_empl_all where ema_perno='" & Session("USER_ID").ToString.Trim & "'"

                    Dim qrycmd2 As New OracleCommand()
                    qrycmd2.CommandText = qry
                    Dim gh1 = getRecordInDt(qrycmd2, conHrps)

                    Dim qryMiniCriteria1 As New OracleCommand()
                    qryMiniCriteria1.CommandText = "select irc_code,irc_desc from t_ir_codes where irc_type='" & ddlYear.SelectedValue.Substring(2, 2).ToString() & "" & ddlCycle.SelectedValue.ToString & "" & gh1.Rows(0)(1) & "' and irc_valid_tag='A' order by irc_code"
                    Dim dtqryMiniCriteria1 = getRecordInDt(qryMiniCriteria1, conHrps)
                    If dtqryMiniCriteria1.Rows.Count > 0 Then

                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Cycle " & ddlCycle.SelectedValue.ToString & " is not active for this report.")
                        Exit Sub
                    End If


                    If gh1.Rows(0)(1) = "I3" Then
                        If checkcateg(Session("USER_ID").ToString, gh1.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            Else
                                updateTag(Session("USER_ID").ToString)
                                Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            End If

                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                        'Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString))
                    ElseIf gh1.Rows(0)(1) = "I4" Or gh1.Rows(0)(1) = "I5" Or gh1.Rows(0)(1) = "I6" Then
                        If checkcateg(Session("USER_ID").ToString, gh1.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            Else
                                updateTag(Session("USER_ID").ToString)
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString))
                                'Response.Write("<script>window.open ('Rpt_OPR.aspx?pno=" & SimpleCrypt(Session("USER_ID").ToString) & "&yr=" & SimpleCrypt(ddlYear.SelectedValue.ToString) & "&cyc=" & SimpleCrypt(ddlCycle.SelectedValue.ToString) & "','_blank');</script>")
                            End If


                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function SimpleCrypt(ByVal Text As String) As String
        Dim strTempChar As String = "", i As Integer
        If Text Is Nothing Then
            Exit Function


        Else

            For i = 1 To Text.Length
                If Asc(Mid$(Text, i, 1)) < 128 Then
                    strTempChar = CType(Asc(Mid$(Text, i, 1)) + 128, String)
                ElseIf Asc(Mid$(Text, i, 1)) > 128 Then
                    strTempChar = CType(Asc(Mid$(Text, i, 1)) - 128, String)
                End If
                Mid$(Text, i, 1) = Chr(CType(strTempChar, Integer))
            Next i
            Return Text
        End If

    End Function
    Public Function GetData(qry As String, con As OracleConnection) As DataTable

        Try
            Dim comd As New OracleDataAdapter(qry, con)
            Dim data As New DataTable()
            comd.Fill(data)
            Return data
        Catch ex As Exception
            Dim g = ex.ToString
        End Try

    End Function
    Public Function checkcateg(pno As String, lvl As String) As Boolean
        Dim falg As Boolean = False
        Try
            Dim cntMinFullFill As Integer = 2
            Dim qry As String = String.Empty
            qry = "Select count(*) No_Records,upper(ss_categ) categ from t_survey_status where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and ss_year='" & ddlYear.SelectedValue.Trim().ToString() & "' and ss_srlno='" & ddlCycle.SelectedValue.Trim().ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') And ss_app_tag = 'AP'  Group by ss_categ"
            Dim cmd As New OracleCommand()
            cmd.CommandText = qry
            Dim g = getRecordInDt(cmd, conHrps)

            'Added by Manoj (IGQPK5672E) on 31-Dec-2021
            Dim qry1 As String = String.Empty
            qry1 = "select irc_code,irc_desc from t_ir_codes where irc_type='" & ddlYear.SelectedValue.Substring(2, 2).ToString() & "" & ddlCycle.SelectedValue.ToString & "" & lvl & "' and irc_valid_tag='A' order by irc_code"
            Dim cmd1 As New OracleCommand()
            cmd1.CommandText = qry1

            Dim g1 = getRecordInDt(cmd1, conHrps)


            If Not g Is Nothing Then
                If lvl = "I3" Then
                    If g.Rows.Count < g1.Rows.Count Then
                        cntMinFullFill = 0
                    Else
                        For i As Integer = 0 To g.Rows.Count - 1
                            For j As Integer = 0 To g1.Rows.Count - 1
                                If g.Rows(i)("categ").ToString = g1.Rows(j)("irc_code").ToString Then
                                    If Val(g.Rows(i)("No_Records").ToString) < Val(g1.Rows(j)("irc_desc").ToString) Then
                                        cntMinFullFill = 0
                                    End If
                                End If
                            Next
                        Next

                    End If
                Else
                    If g.Rows.Count < g1.Rows.Count Then
                        cntMinFullFill = 0
                    Else
                        For i As Integer = 0 To g.Rows.Count - 1
                            For j As Integer = 0 To g1.Rows.Count - 1
                                If g.Rows(i)("categ").ToString = g1.Rows(j)("irc_code").ToString Then
                                    If Val(g.Rows(i)("No_Records").ToString) < Val(g1.Rows(j)("irc_desc").ToString) Then
                                        cntMinFullFill = 0
                                    End If
                                End If
                            Next
                        Next
                    End If
                End If
            Else
            End If
            If cntMinFullFill = 0 Then
                falg = False
            ElseIf cntMinFullFill = 2 Then
                falg = True
            End If
        Catch ex As Exception

        End Try
        Return falg
    End Function
    Public Sub updateTag(pno As String)
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim qry As String = String.Empty
            'qry = "Select * from t_survey_status where ss_asses_pno='" + pno.ToString() + "' and ss_wfl_status ='3' and ss_year='2021' and ss_status = 'SE' and ss_del_tag = 'N' And ss_app_tag = 'AP' and ss_rpt_flag is null"
            'Dim cmd As New OracleCommand()
            'cmd.CommandText = qry
            'Dim g = getRecordInDt(cmd, conHrps)
            Dim sql As String = "update hrps.t_survey_status set ss_rpt_flag='Y'  where ss_asses_pno='" + pno.ToString() + "' and ss_wfl_status ='3' and ss_year='" & ddlYear.SelectedValue.ToString() & "' and ss_srlno='" & ddlCycle.SelectedValue.ToString() & "' and ss_status = 'SE' and ss_del_tag = 'N' And ss_app_tag = 'AP'"
            Dim cmd1 = New OracleCommand(sql, conHrps)
            cmd1.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Function getDataInDt(ByVal cmd1 As OracleCommand) As DataTable
        Dim dt As New DataTable()
        Try
            'If cn.State = ConnectionState.Closed Then
            '    cn.Open()
            'End If
            Dim da As New OracleDataAdapter(cmd1)
            da.Fill(dt)

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If conHrps.State = ConnectionState.Open Then
            conHrps.Close()
        End If
    End Sub

End Class


