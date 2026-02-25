Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Net.Mail
Partial Class FeedbackAdm_OPR
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim dtls As DataTable
    Public Sub SessionTimeOut()
        If Session("USER_ID") Is Nothing Or Session("assespno") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has expired. Kindly refesh the page..")
            Exit Sub
        Else

        End If
    End Sub
    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        'Session.Clear()
        'Session.RemoveAll()
        'Session.Abandon()
        GetFy()
        getsrlno()
        'strUserID = "148536"
        If Session("ADM_USER") = "" Then

            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            'If strUserID = "148536" Then
            '    strUserID = "148536"
            'ElseIf strUserID = "IGQPK5672E" Then
            '    strUserID = "197838"
            'End If
            'strUserID = "197838"
            Session("ADM_USER") = strUserID.ToUpper()
            If GetPno(strUserID.ToUpper()) = False Then
                Session("errorMsg") = "You are not part of this exercise. In case of any query please be in touch with your BUHR."
                Response.Redirect("errorpage.aspx", True)
            End If
            ChkRole()
        ElseIf (Session("ADM_USER") IsNot Nothing) AndAlso (Session("ADM_USER").Equals("") = False) Then
            'strUserID = "198777"
            'Session("ADM_USER") = strUserID.ToUpper()
            ' lblname.Text = GetPno().Rows(0)(1) '"Suresh Dutt Tripathi"
            Return
        Else
        End If

    End Sub
    Private Sub FeedbackAdm_OPR_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnyesclick.Attributes.Add("onclick",
 "document.body.style.cursor = 'wait'; this.value='Please wait...'; this.disabled = true; " + ClientScript.GetPostBackEventReference(btnyesclick, String.Empty) + ";")
        Button1.Attributes.Add("onclick",
 "document.body.style.cursor = 'wait'; this.value='Please wait...'; this.disabled = true; " + ClientScript.GetPostBackEventReference(Button1, String.Empty) + ";")

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
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If conHrps.State = ConnectionState.Open Then
            conHrps.Close()
        End If
    End Sub

    Public Function GetPno(pernr As String) As String
        Try
            Dim d As New Boolean
            Dim q As New OracleCommand()
            q.CommandText = "Select ema_perno from t_ir_adm_grp_privilege,hrps.t_emp_master_feedback360 where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id=:pno and EMA_COMP_CODE='1000'"
            'q.CommandText += " union select ema_bhr_pno ema_perno from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & Session("ADM_USER").ToString() & "' and rownum=1"
            'WI1861: allow TCS employee to access the report as super admin, created by:Avik Mukherjee, created On: 16-06-2021
            q.CommandText += " union select IGP_user_id ema_perno from  t_ir_adm_grp_privilege where IGP_user_id='" & pernr & "' and igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A'"
            q.Connection = conHrps
            q.Parameters.Clear()
            q.Parameters.AddWithValue("pno", pernr.ToString())
            Dim p = getDataInDt(q)
            If p.Rows.Count > 0 Then
                d = True
            Else
                d = False

            End If
            Return d
        Catch ex As Exception

        End Try
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
            MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function
    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False

            Dim strrole As New OracleCommand
            strrole.CommandText = "select IGP_user_id from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            strrole.CommandText += " and IGP_STATUS ='A' and IGP_user_id=:userid"

            'strrole += "UNION select ema_bhr_pno IGP_user_id from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & Session("ADM_USER").ToString() & "' and rownum=1"
            strrole.Connection = conHrps
            strrole.Parameters.Clear()
            strrole.Parameters.AddWithValue("userid", Session("ADM_USER").ToString())

            'Dim cmd = New OracleCommand(strrole, conHrps)
            Dim f = getDataInDt(strrole)

            If f.Rows.Count > 0 Then
                lblname.Text = "Super Admin"
                t = True
            Else
                lblname.Text = "BUHR Admin"
                t = False
            End If
            Return t
        Catch ex As Exception

        End Try
    End Function
    Protected Sub bindDetl()
        Try
            Dim cnt As Integer = 0
            Dim qry As New OracleCommand()
            qry.CommandText = "select distinct r.SS_QOPTN,s.SS_Q2_A,s.SS_Q2_B,r.ss_qcode,r.SS_SRL_No from t_survey_status s,t_survey_response r where s.ss_asses_pno=r.SS_ASSES_PNO and s.SS_YEAR=r.ss_year  AND s.SS_SRLNO=r.ss_serial AND ( s.SS_PNO=:pno "
            qry.CommandText += " or upper(s.ss_email ||s.SS_INTSH_OTP )=:pno)  and s.ss_asses_pno=:ss_asses_pno and s.ss_year=:ss_year and r.SS_PNO=:pno and r.SS_SERIAL=:SS_SERIAL and r.ss_flag='I' order by r.SS_SRL_NO"
            'Dim dt1 = getRecordInDt(qry, conHrps)

            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("pno", Session("USER_ID").ToUpper().ToString())
            qry.Parameters.AddWithValue("ss_asses_pno", Session("AssesPNo").ToString())
            qry.Parameters.AddWithValue("ss_year", ViewState("FY"))
            qry.Parameters.AddWithValue("SS_SERIAL", ViewState("SRLNO").ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt1 As New DataTable()
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                Dim lvl As String = String.Empty
                lvl = populatelevel(Session("AssesPNo"))
                'If lvl.equals("IL5") Or lvl.equals("IL6") Then
                '    rblQ1.SelectedValue = dt1.Rows(0).Item(0).ToString
                '    rblQ2.SelectedValue = dt1.Rows(1).Item(0).ToString
                '    rblQ3.SelectedValue = dt1.Rows(2).Item(0).ToString
                '    rblQ4.SelectedValue = dt1.Rows(3).Item(0).ToString
                '    rblQ5.SelectedValue = dt1.Rows(4).Item(0).ToString
                '    rblQ6.SelectedValue = dt1.Rows(5).Item(0).ToString
                '    rblQ7.SelectedValue = dt1.Rows(6).Item(0).ToString
                '    rblQ8.SelectedValue = dt1.Rows(7).Item(0).ToString
                '    rblQ9.SelectedValue = dt1.Rows(8).Item(0).ToString
                '    txtAns1.Text = dt1.Rows(0).Item(1).ToString
                '    txtAns2.Text = dt1.Rows(0).Item(2).ToString
                'End If
                If lvl.Equals("IL4") Or lvl.Equals("IL5") Or lvl.Equals("IL6") Then
                    rblQ1.SelectedValue = dt1.Rows(0).Item(0).ToString
                    rblQ2.SelectedValue = dt1.Rows(1).Item(0).ToString
                    rblQ3.SelectedValue = dt1.Rows(2).Item(0).ToString
                    rblQ4.SelectedValue = dt1.Rows(3).Item(0).ToString
                    rblQ5.SelectedValue = dt1.Rows(4).Item(0).ToString
                    rblQ6.SelectedValue = dt1.Rows(5).Item(0).ToString
                    rblQ7.SelectedValue = dt1.Rows(6).Item(0).ToString
                    rblQ8.SelectedValue = dt1.Rows(7).Item(0).ToString
                    rblQ9.SelectedValue = dt1.Rows(8).Item(0).ToString
                    rblQ10.SelectedValue = dt1.Rows(9).Item(0).ToString
                    rblQ11.SelectedValue = dt1.Rows(10).Item(0).ToString
                    rblQ12.SelectedValue = dt1.Rows(11).Item(0).ToString

                    txtAns1.Text = dt1.Rows(0).Item(1).ToString
                    txtAns2.Text = dt1.Rows(0).Item(2).ToString
                End If
                If lvl.Equals("IL3") Or lvl.Equals("IL2") Or lvl.Equals("IL1") Then
                    rblQ1.SelectedValue = dt1.Rows(0).Item(0).ToString
                    rblQ2.SelectedValue = dt1.Rows(1).Item(0).ToString
                    rblQ3.SelectedValue = dt1.Rows(2).Item(0).ToString
                    rblQ4.SelectedValue = dt1.Rows(3).Item(0).ToString
                    rblQ5.SelectedValue = dt1.Rows(4).Item(0).ToString
                    rblQ6.SelectedValue = dt1.Rows(5).Item(0).ToString
                    rblQ7.SelectedValue = dt1.Rows(6).Item(0).ToString
                    rblQ8.SelectedValue = dt1.Rows(7).Item(0).ToString
                    rblQ9.SelectedValue = dt1.Rows(8).Item(0).ToString
                    rblQ10.SelectedValue = dt1.Rows(9).Item(0).ToString
                    rblQ11.SelectedValue = dt1.Rows(10).Item(0).ToString
                    rblQ12.SelectedValue = dt1.Rows(11).Item(0).ToString
                    rblQ13.SelectedValue = dt1.Rows(12).Item(0).ToString
                    rblQ14.SelectedValue = dt1.Rows(13).Item(0).ToString
                    rblQ15.SelectedValue = dt1.Rows(14).Item(0).ToString
                    rblQ16.SelectedValue = dt1.Rows(15).Item(0).ToString

                    txtAns1.Text = dt1.Rows(0).Item(1).ToString
                    txtAns2.Text = dt1.Rows(0).Item(2).ToString
                End If

            End If
        Catch ex As Exception
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while load bindDetl function :- Respondent " & Session("USER_ID").ToString() & "  Assess P.no:- " & Session("AssesPno").ToString() & "  Year" & ViewState("FY").ToString() & Environment.NewLine
            Dim val = ex.ToString
            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while bindDetl function")
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub bindPendingRecord(Asspno As String)
        Try
            'If Not Ispostback Then
            '    If Session("statusotp") Is Nothing Then
            '        response.redirect("survey_opr.aspx")
            '    End If
            'End If

            Dim mail = ""
            Session.Remove("statusotp")
            Session.Remove("otpsend")
            Dim sql As String = String.Empty
            Dim qry As New OracleCommand()
            Dim pno = Session("USER_ID").ToString()
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            'qry.CommandText = "select ss_asses_pno,ema_ename,ema_desgn_desc,ema_dept_desc,'Pending' status from tips.t_empl_all, t_survey_status "
            'qry.CommandText += "where ss_asses_pno=ema_perno and nvl(ss_wfl_status,'0')<>'3' and SS_PNO='" & pno & "'"             
            ''WI484: rectification in code to allow only data whose company code is 1000,
            '''created by: Avik Mukherjee
            '''Date: 09-06-2021
            sql = "select ss_asses_pno,ema_ename,ema_desgn_desc,ema_dept_desc,decode(ss_wfl_status,'2','Pending','3','Completed','9','Rejected') status from hrps.t_emp_master_feedback360, t_survey_status "
            sql += "where ss_asses_pno=ema_perno  and SS_YEAR =:SS_YEAR and SS_DEL_TAG='N'  and ss_app_tag='AP' and ss_wfl_status<>'1' and ema_comp_code='1000'"

            If mail <> "" Then
                mail = mail.ToUpper().Trim
                sql += " and TRIM(upper(SS_EMAIL))||SS_INTSH_OTP=:SS_INTSH_OTP and SS_SRLNO=:SS_SRLNO"
            Else
                sql += " and SS_PNO=:SS_PNO and SS_SRLNO=:SS_SRLNO"
            End If
            sql += " and ss_wfl_status in ('2','3','9') and trunc(ema_step3_stdt)<=trunc(sysdate) and trunc(ema_step3_enddt)>=trunc(sysdate)"
            'Added By Mukul Mishra
            If Asspno = "" Then
            Else
                sql += " and ss_asses_pno in (" & aspno.Text.TrimEnd(",") & ")"
            End If
            'ends
            qry.Connection = conHrps
            qry.CommandText = sql
            qry.Parameters.Clear()
            qry.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            qry.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
            If mail <> "" Then
                qry.Parameters.Add(New OracleParameter(":SS_INTSH_OTP", mail))
            Else
                qry.Parameters.Add(New OracleParameter(":SS_PNO", pno.ToUpper().Trim()))
            End If

            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                gvPending.DataSource = dt
                gvPending.DataBind()
            Else
                gvPending.DataSource = Nothing
                gvPending.DataBind()
                'Response.Redirect("survey_OPR.aspx", True)
            End If

        Catch ex As Exception
            '    Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            '    Dim dividesterr = "Error while load fedabck pending or complete :- Respondent " & Session("USER_ID").ToString() & "  Year " & ViewState("FY").ToString() & Environment.NewLine
            '    Dim val = ex.ToString & Environment.NewLine & Environment.NewLine
            '    File.AppendAllText(fnameerr, dividesterr)
            '    File.AppendAllText(fnameerr, val)
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while displaying pending/complete data")
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
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
    Private Sub freezeScreen()
        rblQ1.Enabled = False
        rblQ2.Enabled = False
        rblQ3.Enabled = False
        rblQ4.Enabled = False
        rblQ5.Enabled = False
        rblQ6.Enabled = False
        rblQ7.Enabled = False
        rblQ8.Enabled = False
        rblQ9.Enabled = False
        rblQ10.Enabled = False
        rblQ11.Enabled = False
        rblQ12.Enabled = False
        rblQ13.Enabled = False
        rblQ14.Enabled = False
        rblQ15.Enabled = False
        rblQ16.Enabled = False
        txtAns1.Enabled = False
        txtAns2.Enabled = False
        btn_submit.Visible = False
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
            Throw ex
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function
    Public Sub unfreeze()
        rblQ1.Enabled = True
        rblQ2.Enabled = True
        rblQ3.Enabled = True
        rblQ4.Enabled = True
        rblQ5.Enabled = True
        rblQ6.Enabled = True
        rblQ7.Enabled = True
        rblQ8.Enabled = True
        rblQ9.Enabled = True
        rblQ10.Enabled = True
        rblQ11.Enabled = True
        rblQ12.Enabled = True
        rblQ13.Enabled = True
        rblQ14.Enabled = True
        rblQ15.Enabled = True
        rblQ16.Enabled = True
        txtAns1.Enabled = True
        txtAns2.Enabled = True

    End Sub

    Protected Sub CheckOpenDt()
        Try
            Dim cnt As Integer = 0
            Dim qry As New OracleCommand()
            qry.CommandText = "select case when trunc(s.ss_app_dt)<=trunc(ema.ema_step3_stdt) then case when trunc(ema.ema_step3_stdt+5) >=trunc(sysdate) then 'O' else 'C' end else case when trunc(s.ss_app_dt+5) >=trunc(sysdate) then 'O' else 'C' end  end openDt from t_survey_status s,t_emp_master_feedback360 ema where s.ss_asses_pno=ema.ema_perno and ( s.SS_PNO=:pno "
            qry.CommandText += " or upper(s.ss_email ||s.SS_INTSH_OTP )=:pno)  and s.ss_asses_pno=:ss_asses_pno and s.ss_year=:ss_year and s.ss_wfl_status='2'  and s.ss_app_tag='AP'"
            'Dim dt1 = getRecordInDt(qry, conHrps)

            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("pno", Session("USER_ID").ToUpper().ToString())
            qry.Parameters.AddWithValue("ss_asses_pno", Session("AssesPNo").ToString())
            qry.Parameters.AddWithValue("ss_year", ViewState("FY"))
            Dim da As New OracleDataAdapter(qry)
            Dim dtOpen As New DataTable()
            da.Fill(dtOpen)
            If dtOpen.Rows.Count > 0 Then
                'If dtOpen.Rows(0)("openDt").ToString = "T" Then
                '    btn_reject.Visible = True
                '    updateOpenDt()
                'Else
                If dtOpen.Rows(0)("openDt").ToString = "O" Then
                    btn_reject.Visible = True
                Else
                    btn_reject.Visible = False
                End If

            Else
                btn_reject.Visible = False
            End If
        Catch ex As Exception
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while load bindDetl function :- Respondent " & Session("USER_ID").ToString() & "  Assess P.no:- " & Session("AssesPno").ToString() & "  Year" & ViewState("FY").ToString() & Environment.NewLine
            Dim val = ex.ToString
            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while bindDetl function")
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub updateOpenDt()
        Try
            SessionTimeOut()
            Dim query As String = String.Empty
            query = "update hrps.t_survey_status set SS_FEEDBACK_OPEN_DT=sysdate"
            query += " where ( SS_PNO=:pno "
            query += " or upper(ss_email ||SS_INTSH_OTP )=:pno)  and ss_asses_pno=:ss_asses_pno and ss_year=:ss_year and ss_wfl_status='2'  and ss_app_tag='AP'"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("pno", Session("USER_ID").ToUpper().ToString())
            comnd.Parameters.AddWithValue("ss_asses_pno", Session("AssesPNo").ToString())
            comnd.Parameters.AddWithValue("ss_year", ViewState("FY"))
            comnd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Protected Sub lbtnShow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnShow.Click
        If txtRespondent.Text = "119628" Then
            Session("USER_ID") = txtRespondent.Text.Trim().ToString
            bindPendingRecord("")
        Else
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please fill MD AD ID no.")
        End If

    End Sub
    Protected Sub gvPending_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            lblRateNm.Text = String.Empty
            txtAns1.Text = String.Empty
            txtAns2.Text = String.Empty
            lblRecipientNm1.Text = String.Empty
            lblRecipientNm3.Text = String.Empty
            lblRecipientNm4.Text = String.Empty
            'rblQ1a.SelectedValue = "1"
            'rblQ1b.SelectedValue = "1"
            'rblQ1c.SelectedValue = "1"
            'rblQ1d.SelectedValue = "1"
            rblQ1.SelectedIndex = -1
            rblQ2.SelectedIndex = -1
            rblQ3.SelectedIndex = -1
            rblQ4.SelectedIndex = -1
            rblQ5.SelectedIndex = -1
            rblQ6.SelectedIndex = -1
            rblQ7.SelectedIndex = -1
            rblQ8.SelectedIndex = -1
            rblQ9.SelectedIndex = -1
            rblQ10.SelectedIndex = -1
            rblQ11.SelectedIndex = -1
            rblQ12.SelectedIndex = -1
            rblQ13.SelectedIndex = -1
            rblQ14.SelectedIndex = -1
            rblQ15.SelectedIndex = -1
            rblQ16.SelectedIndex = -1


            If e.CommandName = "select" Then
                Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument)
                Dim row As GridViewRow = gvPending.Rows(rowIndex)

                Dim lbl_name As String = ""
                Dim lbl_asses_pno As String = ""
                Dim lbl_stage As String = ""
                lbl_name = TryCast(row.FindControl("lnlname"), Label).Text
                lbl_asses_pno = TryCast(row.FindControl("lblpno"), Label).Text
                lbl_stage = TryCast(row.FindControl("lblstatus"), Label).Text
                Session("AssesPNo") = lbl_asses_pno.ToString
                'Session("USER_ID") = txtRespondent.Text.Trim().ToString
                lblRateNm.Text = lbl_name.ToString()
                lblRecipientNm1.Text = lbl_name.ToString()
                lblRecipientNm3.Text = lbl_name.ToString()
                lblRecipientNm4.Text = lbl_name.ToString()
                lblRcptNm.Text = lbl_name.ToString()
                pnl.Visible = True
                If lbl_stage = "Pending" Then
                    populateQuestion(lbl_asses_pno.Trim)
                    'CheckOpenDt()
                    btn_reject.Visible = True
                    btn_submit.Visible = True
                    unfreeze()
                ElseIf lbl_stage = "Completed" Then
                    populateQuestion(lbl_asses_pno.Trim)
                    btn_submit.Visible = False
                    btn_reject.Visible = False
                    bindDetl()
                    freezeScreen()

                Else
                    populateQuestion(lbl_asses_pno.Trim)
                    btn_submit.Visible = False
                    btn_reject.Visible = False
                    freezeScreen()
                    'rblQ1a.Enabled = False
                    'rblQ1b.Enabled = False
                    'rblQ1c.Enabled = False
                    'rblQ1d.Enabled = False
                    'txtAns2.Enabled = False
                    'txtAns1.Enabled = False
                    bindDetl()

                End If

            End If
        Catch ex As Exception
            'Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            'Dim dividesterr = "Error while load fedabck pending or complete :- Respondent" & Session("USER_ID").ToString() & "  Year" & ViewState("FY").ToString() & Environment.NewLine
            'Dim val = ex.ToString
            'File.AppendAllText(fnameerr, dividesterr)
            'File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while populating data")
        End Try
    End Sub
    Private Function populatelevel(ByVal pno As String) As String
        Dim lvl As String = String.Empty
        Dim ls_pno As New OracleCommand()
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            'Dim qry1 As New OracleCommand()
            'qry1.CommandText = "select decode(SS_IL,'I3','IL3','I4','IL4','I5','IL5','I6','IL6') SS_IL from t_assesse_IL  where SS_ASSESS_PNO=:ema_perno and SS_STATUS='A'"
            'qry1.Connection = conHrps
            'qry1.Parameters.Clear()
            'qry1.Parameters.AddWithValue("ema_perno", pno.ToString())
            'Dim daIL As New OracleDataAdapter(qry1)
            'Dim dtIL As New DataTable()
            'daIL.Fill(dtIL)
            'If dtIL.Rows.Count > 0 Then
            '    lvl = dtIL.Rows(0).Item(0).ToString
            'Else
            ls_pno.CommandText = "select  decode(EMA_EQV_LEVEL,'I1','IL1','I2','IL2','I3','IL3','I4','IL4','I5','IL5','I6','IL6','TG','IL3') EMA_EQV_LEVEL from HRPS.T_EMP_MASTER_FEEDBACK360 where ema_perno=:ema_perno and EMA_COMP_CODE='1000' AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
            ls_pno.Connection = conHrps
            ls_pno.Parameters.Clear()
            ls_pno.Parameters.AddWithValue(":ema_perno", pno)
            ls_pno.Parameters.AddWithValue(":EMA_YEAR", ViewState("FY").ToString)
            ls_pno.Parameters.AddWithValue(":EMA_CYCLE", ViewState("SRLNO").ToString)
            Dim da As New OracleDataAdapter(ls_pno)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                lvl = dt.Rows(0).Item(0).ToString
            End If


            'End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return lvl
    End Function
    Private Sub cleanQuestion()
        span1.InnerText = ""
        span2.InnerText = ""
        Span3.InnerText = ""
        Span4.InnerText = ""
        Span5.InnerText = ""
        Span6.InnerText = ""
        Span7.InnerText = ""
        Span8.InnerText = ""
        Span9.InnerText = ""
        Span10.InnerText = ""
        Span11.InnerText = ""
        Span12.InnerText = ""
        Span13.InnerText = ""
        Span14.InnerText = ""
        Span15.InnerText = ""
        Span16.InnerText = ""
        span1P.InnerText = ""
        span2P.InnerText = ""
        Span3P.InnerText = ""
        Span4P.InnerText = ""
        Span5P.InnerText = ""
        Span6P.InnerText = ""
        Span7P.InnerText = ""
        Span8P.InnerText = ""
        Span9P.InnerText = ""
        Span10P.InnerText = ""
        Span11P.InnerText = ""
        Span12P.InnerText = ""
        Span13P.InnerText = ""
        Span14P.InnerText = ""
        Span15P.InnerText = ""
        Span16P.InnerText = ""
        rblQ1.Visible = True
        rblQ2.Visible = True
        rblQ3.Visible = True
        rblQ4.Visible = True
        rblQ5.Visible = True
        rblQ6.Visible = True
        rblQ7.Visible = True
        rblQ8.Visible = True
        rblQ9.Visible = True
        rblQ10.Visible = True
        rblQ11.Visible = True
        rblQ12.Visible = True
        rblQ13.Visible = True
        rblQ14.Visible = True
        rblQ15.Visible = True
        rblQ16.Visible = True
        txtAns1.Text = ""
        txtAns2.Text = ""
        txtAns1.Enabled = True
        txtAns2.Enabled = True
    End Sub
    Private Sub populateQuestion(ByVal pno As String)
        Dim ls_pno As New OracleCommand()
        Dim lvl As String = String.Empty
        Try
            cleanQuestion()
            lvl = populatelevel(pno)
            If lvl = "IL3" Or lvl = "IL2" Or lvl = "IL1" Then
                lblRecipientNm5.Text = "People Development"
            Else
                lblRecipientNm5.Text = "Team Building"
            End If
            ls_pno.CommandText = "select SS_QTEXT from hrps.t_survey_question q where q.ss_qlevel=:ss_qlevel order by q.ss_srl_no"
            ls_pno.Connection = conHrps
            ls_pno.Parameters.Clear()
            ls_pno.Parameters.AddWithValue(":ss_qlevel", lvl)
            Dim da1 As New OracleDataAdapter(ls_pno)
            Dim dt1 As New DataTable()
            da1.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                If lvl = "IL3" Or lvl = "IL2" Or lvl = "IL1" Then
                    Dim words1 As String() = dt1.Rows(0).Item(0).ToString.Split(New Char() {"$"c})

                    'span1.InnerText = dt1.Rows(0).Item(0).ToString
                    span1.InnerText = words1(0).ToString
                    span1P.InnerText = words1(1).ToString

                    Dim words2 As String() = dt1.Rows(1).Item(0).ToString.Split(New Char() {"$"c})
                    span2.InnerText = words2(0).ToString
                    span2P.InnerText = words2(1).ToString

                    Dim words3 As String() = dt1.Rows(2).Item(0).ToString.Split(New Char() {"$"c})
                    Span3.InnerText = words3(0).ToString
                    Span3P.InnerText = words3(1).ToString

                    Dim words4 As String() = dt1.Rows(3).Item(0).ToString.Split(New Char() {"$"c})
                    Span4.InnerText = words4(0).ToString
                    Span4P.InnerText = words4(1).ToString

                    Dim words5 As String() = dt1.Rows(4).Item(0).ToString.Split(New Char() {"$"c})
                    Span5.InnerText = words5(0).ToString
                    Span5P.InnerText = words5(1).ToString

                    Dim words6 As String() = dt1.Rows(5).Item(0).ToString.Split(New Char() {"$"c})
                    Span6.InnerText = words6(0).ToString
                    Span6P.InnerText = words6(1).ToString

                    Dim words7 As String() = dt1.Rows(6).Item(0).ToString.Split(New Char() {"$"c})
                    Span7.InnerText = words7(0).ToString
                    Span7P.InnerText = words7(1).ToString

                    Dim words8 As String() = dt1.Rows(7).Item(0).ToString.Split(New Char() {"$"c})
                    Span8.InnerText = words8(0).ToString
                    Span8P.InnerText = words8(1).ToString

                    Dim words9 As String() = dt1.Rows(8).Item(0).ToString.Split(New Char() {"$"c})
                    Span9.InnerText = words9(0).ToString
                    Span9P.InnerText = words9(1).ToString

                    Dim words10 As String() = dt1.Rows(9).Item(0).ToString.Split(New Char() {"$"c})
                    Span10.InnerText = words10(0).ToString
                    Span10P.InnerText = words10(1).ToString

                    Dim words11 As String() = dt1.Rows(10).Item(0).ToString.Split(New Char() {"$"c})
                    Span11.InnerText = words11(0).ToString
                    Span11P.InnerText = words11(1).ToString

                    Dim words12 As String() = dt1.Rows(11).Item(0).ToString.Split(New Char() {"$"c})
                    Span12.InnerText = words12(0).ToString
                    Span12P.InnerText = words12(1).ToString

                    Dim words13 As String() = dt1.Rows(12).Item(0).ToString.Split(New Char() {"$"c})
                    Span13.InnerText = words13(0).ToString
                    Span13P.InnerText = words13(1).ToString

                    Dim words14 As String() = dt1.Rows(13).Item(0).ToString.Split(New Char() {"$"c})
                    Span14.InnerText = words14(0).ToString
                    Span14P.InnerText = words14(1).ToString

                    Dim words15 As String() = dt1.Rows(14).Item(0).ToString.Split(New Char() {"$"c})
                    Span15.InnerText = words15(0).ToString
                    Span15P.InnerText = words15(1).ToString

                    Dim words16 As String() = dt1.Rows(15).Item(0).ToString.Split(New Char() {"$"c})
                    Span16.InnerText = words16(0).ToString
                    Span16P.InnerText = words16(1).ToString

                    tr13.Visible = True
                    tr14.Visible = True
                    tr15.Visible = True
                    tr16.Visible = True
                    Span13.Visible = True
                    Span14.Visible = True
                    Span15.Visible = True
                    Span16.Visible = True
                    Span13P.Visible = True
                    Span14P.Visible = True
                    Span15P.Visible = True
                    Span16P.Visible = True
                ElseIf lvl = "IL4" Or lvl = "IL5" Or lvl = "IL6" Then
                    Dim words1 As String() = dt1.Rows(0).Item(0).ToString.Split(New Char() {"$"c})

                    'span1.InnerText = dt1.Rows(0).Item(0).ToString
                    span1.InnerText = words1(0).ToString
                    span1P.InnerText = words1(1).ToString

                    Dim words2 As String() = dt1.Rows(1).Item(0).ToString.Split(New Char() {"$"c})
                    span2.InnerText = words2(0).ToString
                    span2P.InnerText = words2(1).ToString

                    Dim words3 As String() = dt1.Rows(2).Item(0).ToString.Split(New Char() {"$"c})
                    Span3.InnerText = words3(0).ToString
                    Span3P.InnerText = words3(1).ToString

                    Dim words4 As String() = dt1.Rows(3).Item(0).ToString.Split(New Char() {"$"c})
                    Span4.InnerText = words4(0).ToString
                    Span4P.InnerText = words4(1).ToString

                    Dim words5 As String() = dt1.Rows(4).Item(0).ToString.Split(New Char() {"$"c})
                    Span5.InnerText = words5(0).ToString
                    Span5P.InnerText = words5(1).ToString

                    Dim words6 As String() = dt1.Rows(5).Item(0).ToString.Split(New Char() {"$"c})
                    Span6.InnerText = words6(0).ToString
                    Span6P.InnerText = words6(1).ToString

                    Dim words7 As String() = dt1.Rows(6).Item(0).ToString.Split(New Char() {"$"c})
                    Span7.InnerText = words7(0).ToString
                    Span7P.InnerText = words7(1).ToString

                    Dim words8 As String() = dt1.Rows(7).Item(0).ToString.Split(New Char() {"$"c})
                    Span8.InnerText = words8(0).ToString
                    Span8P.InnerText = words8(1).ToString

                    Dim words9 As String() = dt1.Rows(8).Item(0).ToString.Split(New Char() {"$"c})
                    Span9.InnerText = words9(0).ToString
                    Span9P.InnerText = words9(1).ToString

                    Dim words10 As String() = dt1.Rows(9).Item(0).ToString.Split(New Char() {"$"c})
                    Span10.InnerText = words10(0).ToString
                    Span10P.InnerText = words10(1).ToString

                    Dim words11 As String() = dt1.Rows(10).Item(0).ToString.Split(New Char() {"$"c})
                    Span11.InnerText = words11(0).ToString
                    Span11P.InnerText = words11(1).ToString

                    Dim words12 As String() = dt1.Rows(11).Item(0).ToString.Split(New Char() {"$"c})
                    Span12.InnerText = words12(0).ToString
                    Span12P.InnerText = words12(1).ToString

                    tr13.Visible = False
                    tr14.Visible = False
                    tr15.Visible = False
                    tr16.Visible = False
                    Span13.Visible = False
                    Span14.Visible = False
                    Span15.Visible = False
                    Span13P.Visible = False
                    Span14P.Visible = False
                    Span15P.Visible = False
                    rblQ13.Visible = False
                    rblQ14.Visible = False
                    rblQ15.Visible = False
                    rblQ16.Visible = False

                End If


            End If


        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.info, ex.Message)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Protected Sub btn_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        submit()
    End Sub

    Public Sub submit()
        Try
            SessionTimeOut()
            Dim lvl As String = populatelevel(Session("AssesPno"))
            If lvl.Equals("IL4") Or lvl.Equals("IL5") Or lvl.Equals("IL6") Then
                If rblQ1.SelectedIndex = -1 Or rblQ2.SelectedIndex = -1 Or rblQ3.SelectedIndex = -1 Or rblQ4.SelectedIndex = -1 Or rblQ5.SelectedIndex = -1 Or rblQ6.SelectedIndex = -1 Or rblQ7.SelectedIndex = -1 Or rblQ8.SelectedIndex = -1 Or rblQ9.SelectedIndex = -1 Or rblQ10.SelectedIndex = -1 Or rblQ11.SelectedIndex = -1 Or rblQ12.SelectedIndex = -1 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please complete the survey statements.")
                    Exit Sub
                End If
            End If
            If lvl.Equals("IL3") Or lvl.Equals("IL2") Or lvl.Equals("IL1") Then
                If rblQ1.SelectedIndex = -1 Or rblQ2.SelectedIndex = -1 Or rblQ3.SelectedIndex = -1 Or rblQ4.SelectedIndex = -1 Or rblQ5.SelectedIndex = -1 Or rblQ6.SelectedIndex = -1 Or rblQ7.SelectedIndex = -1 Or rblQ8.SelectedIndex = -1 Or rblQ9.SelectedIndex = -1 Or rblQ10.SelectedIndex = -1 Or rblQ11.SelectedIndex = -1 Or rblQ12.SelectedIndex = -1 Or rblQ13.SelectedIndex = -1 Or rblQ14.SelectedIndex = -1 Or rblQ15.SelectedIndex = -1 Or rblQ16.SelectedIndex = -1 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please complete the survey statements.")
                    Exit Sub
                End If
            End If

            If txtAns1.Text.Trim = "" Or txtAns2.Text.Trim = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill in the answer box")
                Exit Sub
            End If
            If txtAns1.Text.Length > 500 Or txtAns2.Text.Length > 500 Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill text within 500 characters")
                Exit Sub
            End If

            If Session("AssesPno").ToString = "" Or ViewState("FY").ToString = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Your session has been expired. Please refresh the page and Submit your feedback again...!")
                Exit Sub
            End If
            'UpdateFeedbackData(Session("AssesPno"), rblQ1a.SelectedValue.ToString, rblQ1b.SelectedValue.ToString, rblQ1c.SelectedValue.ToString, rblQ1d.SelectedValue.ToString, txtAns1.Text.Trim().ToString, txtAns2.Text.Trim().ToString)

            If lvl.Equals("IL4") Or lvl.Equals("IL5") Or lvl.Equals("IL6") Then
                insertresponseIL4(Session("AssesPno"), rblQ1.SelectedValue, rblQ2.SelectedValue, rblQ3.SelectedValue, rblQ4.SelectedValue, rblQ5.SelectedValue, rblQ6.SelectedValue, rblQ7.SelectedValue, rblQ8.SelectedValue, rblQ9.SelectedValue, rblQ10.SelectedValue, rblQ11.SelectedValue, rblQ12.SelectedValue, txtAns1.Text.Trim, txtAns2.Text.Trim, lvl)
                ShowGenericMessageModal(CommonConstants.AlertType.success, "You have succesfully submitted your response!")
                btn_reject.Visible = False
                freezeScreen()
                bindPendingRecord("")
            End If
            'If lvl.Equals("IL4") Then
            '    insertresponseIL4(Session("AssesPno"), rblQ1.SelectedValue, rblQ2.SelectedValue, rblQ3.SelectedValue, rblQ4.SelectedValue, rblQ5.SelectedValue, rblQ6.SelectedValue, rblQ7.SelectedValue, rblQ8.SelectedValue, rblQ9.SelectedValue, rblQ10.SelectedValue, rblQ11.SelectedValue, rblQ12.SelectedValue, txtAns1.Text.Trim, txtAns2.Text.Trim, lvl)
            '    ShowGenericMessageModal(CommonConstants.AlertType.success, "You have succesfully submitted your response!")
            '    freezeScreen()
            '    bindPendingRecord()
            'End If
            If lvl.Equals("IL3") Or lvl.Equals("IL2") Or lvl.Equals("IL1") Then
                insertresponseIL3(Session("AssesPno"), rblQ1.SelectedValue, rblQ2.SelectedValue, rblQ3.SelectedValue, rblQ4.SelectedValue, rblQ5.SelectedValue, rblQ6.SelectedValue, rblQ7.SelectedValue, rblQ8.SelectedValue, rblQ9.SelectedValue, rblQ10.SelectedValue, rblQ11.SelectedValue, rblQ12.SelectedValue, rblQ13.SelectedValue, rblQ14.SelectedValue, rblQ15.SelectedValue, rblQ16.SelectedValue, txtAns1.Text.Trim, txtAns2.Text.Trim, lvl)
                ShowGenericMessageModal(CommonConstants.AlertType.success, "You have succesfully submitted your response!")
                btn_reject.Visible = False
                freezeScreen()
                bindPendingRecord("")
            End If

        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while submitting response")
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Private Sub insertresponseIL3(ByVal asseesse As String, ByVal q1 As String, ByVal q2 As String, ByVal q3 As String, ByVal q4 As String, ByVal q5 As String, ByVal q6 As String, ByVal q7 As String, ByVal q8 As String, ByVal q9 As String, ByVal q10 As String, ByVal q11 As String, ByVal q12 As String, ByVal q13 As String, ByVal q14 As String, ByVal q15 As String, ByVal q16 As String, ByVal txt1 As String, ByVal txt2 As String, ByVal level As String)
        SessionTimeOut()
        Dim ls_sql As String = String.Empty
        Dim ls_sql1 As String = String.Empty
        Dim pnoid As String = String.Empty
        Dim cmd As New OracleCommand()
        Dim arr_cmd As New ArrayList()
        Dim cnt As Integer = 0
        Dim res As New ArrayList()
        res.Add(q1)
        res.Add(q2)
        res.Add(q3)
        res.Add(q4)
        res.Add(q5)
        res.Add(q6)
        res.Add(q7)
        res.Add(q8)
        res.Add(q9)
        res.Add(q10)
        res.Add(q11)
        res.Add(q12)
        res.Add(q13)
        res.Add(q14)
        res.Add(q15)
        res.Add(q16)
        If asseesse = "" Then
            asseesse = ID
        End If
        'WI624: allow external respondent to provide feedback to IL3,created by: Avik Mukherjee, created on: 16-06-2021
        If Session("USER_ID").ToString.Length > 6 Then
            ls_sql1 = "select SS_PNO from t_Survey_status where upper(SS_EMAIL)||SS_INTSH_OTP='" & Session("USER_ID").ToString.ToUpper() & "' and SS_YEAR='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO='" & asseesse.ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "'"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd = New OracleCommand(ls_sql1, conHrps)
            Dim da5 As New OracleDataAdapter(cmd)
            Dim dt5 As New DataTable
            da5.Fill(dt5)
            If dt5.Rows.Count > 0 Then
                pnoid = dt5.Rows(0).Item(0)
            Else

            End If
        Else
            pnoid = Session("USER_ID")
        End If
        'WI624: end of code , created by: Avik Mukherjee, creeated on: 16-06-2021
        Try
            ls_sql = "select SS_QCODE,SS_SRL_NO from t_survey_question where SS_QLEVEL=:SS_QLEVEL and SS_YEAR=:SS_YEAR order by SS_SRL_NO"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd = New OracleCommand(ls_sql, conHrps)
            cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            cmd.Parameters.Add(New OracleParameter(":SS_QLEVEL", level.Trim))
            Dim da As New OracleDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                While cnt < dt.Rows.Count

                    ls_sql = "insert into t_survey_response(SS_YEAR,SS_ASSES_PNO,SS_PNO,SS_QCODE,SS_QOPTN,SS_QLEVEL,SS_CREATED_BY,SS_CREATED_DT,SS_SRL_NO,SS_SERIAL)"
                    ls_sql += " values(:SS_YEAR,:SS_ASSES_PNO,:SS_PNO,:SS_QCODE,:SS_QOPTN,:SS_QLEVEL,:SS_CREATED_BY,sysdate,:SS_SRL_NO,:SS_SERIAL)"
                    If conHrps.State = ConnectionState.Closed Then
                        conHrps.Open()
                    End If
                    cmd = New OracleCommand(ls_sql, conHrps)
                    cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
                    cmd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", UCase(asseesse.ToString())))
                    cmd.Parameters.Add(New OracleParameter(":SS_PNO", pnoid))
                    cmd.Parameters.Add(New OracleParameter(":SS_QCODE", dt.Rows(cnt).Item(0).ToString.Trim))
                    cmd.Parameters.Add(New OracleParameter(":SS_QOPTN", res(cnt)))
                    cmd.Parameters.Add(New OracleParameter(":SS_QLEVEL", level))
                    cmd.Parameters.Add(New OracleParameter(":SS_CREATED_BY", UCase(Session("ADM_USER").ToString())))
                    cmd.Parameters.Add(New OracleParameter(":SS_SRL_NO", dt.Rows(cnt).Item(1).ToString.Trim))
                    cmd.Parameters.Add(New OracleParameter(":SS_SERIAL", ViewState("SRLNO").ToString()))
                    arr_cmd.Add(cmd)
                    cnt = cnt + 1
                End While

                ls_sql = "update T_SURVEY_STATUS set SS_Q2_A=:SS_Q2_A,SS_Q2_B=:SS_Q2_B,SS_WFL_STATUS='3' , SS_ACTION_BY='ADM'"
                ls_sql += ", SS_FEEDBACK_DT = sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY WHERE (SS_PNO=:SS_PNO or upper(SS_EMAIL || SS_INTSH_OTP) =:SS_PNO )"
                ls_sql += "  AND SS_ASSES_PNO=:SS_ASSES_PNO  and SS_YEAR =:SS_YEAR and SS_SRLNO=:SS_SRLNO"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                cmd = New OracleCommand(ls_sql, conHrps)
                cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
                cmd.Parameters.Add(New OracleParameter(":SS_Q2_A", txt1.ToString.Trim))
                cmd.Parameters.Add(New OracleParameter(":SS_Q2_B", txt2.ToString.Trim))
                cmd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", UCase(asseesse.ToString())))
                cmd.Parameters.Add(New OracleParameter(":SS_PNO", UCase(Session("User_Id").ToString())))
                cmd.Parameters.Add(New OracleParameter(":SS_UPDATED_BY", UCase(Session("ADM_USER").ToString())))
                cmd.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
                arr_cmd.Add(cmd)
            End If





            If arr_cmd.Count > 0 Then
                Dim counter As Integer = 0
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim tran_Ins As OracleTransaction
                tran_Ins = conHrps.BeginTransaction()
                Try
                    For counter = 0 To arr_cmd.Count - 1
                        Dim con_ins As New OracleCommand()
                        con_ins = arr_cmd.Item(counter)
                        con_ins.Transaction = tran_Ins
                        con_ins.ExecuteNonQuery()
                    Next
                    tran_Ins.Commit()



                Catch ex As Exception
                    tran_Ins.Rollback()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while submiting response!")
                Finally
                    If conHrps.State = ConnectionState.Open Then
                        conHrps.Close()
                    End If
                End Try
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try


    End Sub
    Private Sub insertresponseIL4(ByVal asseesse As String, ByVal q1 As String, ByVal q2 As String, ByVal q3 As String, ByVal q4 As String, ByVal q5 As String, ByVal q6 As String, ByVal q7 As String, ByVal q8 As String, ByVal q9 As String, ByVal q10 As String, ByVal q11 As String, ByVal q12 As String, ByVal txt1 As String, ByVal txt2 As String, ByVal level As String)
        SessionTimeOut()
        Dim ls_sql As String = String.Empty
        Dim ls_sql1 As String = String.Empty
        Dim cmd As New OracleCommand()
        Dim arr_cmd As New ArrayList()
        Dim cnt As Integer = 0
        Dim pnoid As String = String.Empty
        Dim res As New ArrayList()
        res.Add(q1)
        res.Add(q2)
        res.Add(q3)
        res.Add(q4)
        res.Add(q5)
        res.Add(q6)
        res.Add(q7)
        res.Add(q8)
        res.Add(q9)
        res.Add(q10)
        res.Add(q11)
        res.Add(q12)
        If asseesse = "" Then
            asseesse = ID
        End If
        If Session("USER_ID").ToString.Length > 6 Then
            ls_sql1 = "select SS_PNO from t_Survey_status where upper(SS_EMAIL)||SS_INTSH_OTP='" & Session("USER_ID").ToString.ToUpper() & "' and SS_YEAR='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO='" & asseesse.ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "'"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd = New OracleCommand(ls_sql1, conHrps)
            Dim da5 As New OracleDataAdapter(cmd)
            Dim dt5 As New DataTable
            da5.Fill(dt5)
            If dt5.Rows.Count > 0 Then
                pnoid = dt5.Rows(0).Item(0)
            Else

            End If
        Else
            pnoid = Session("USER_ID")
        End If
        Try

            ls_sql = "select SS_QCODE,SS_SRL_NO from t_survey_question where SS_QLEVEL=:SS_QLEVEL and SS_YEAR=:SS_YEAR order by SS_SRL_NO"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd = New OracleCommand(ls_sql, conHrps)
            cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            cmd.Parameters.Add(New OracleParameter(":SS_QLEVEL", level.Trim))
            Dim da As New OracleDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then

                While cnt < dt.Rows.Count

                    ls_sql = "insert into t_survey_response(SS_YEAR,SS_ASSES_PNO,SS_PNO,SS_QCODE,SS_QOPTN,SS_QLEVEL,SS_CREATED_BY,SS_CREATED_DT,SS_SRL_NO,SS_SERIAL)"
                    ls_sql += " values(:SS_YEAR,:SS_ASSES_PNO,:SS_PNO,:SS_QCODE,:SS_QOPTN,:SS_QLEVEL,:SS_CREATED_BY,sysdate,:SS_SRL_NO,:SS_SERIAL)"
                    If conHrps.State = ConnectionState.Closed Then
                        conHrps.Open()
                    End If
                    cmd = New OracleCommand(ls_sql, conHrps)
                    cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
                    cmd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", UCase(asseesse.ToString())))
                    cmd.Parameters.Add(New OracleParameter(":SS_PNO", pnoid))
                    cmd.Parameters.Add(New OracleParameter(":SS_QCODE", dt.Rows(cnt).Item(0).ToString.Trim))
                    cmd.Parameters.Add(New OracleParameter(":SS_QOPTN", res(cnt)))
                    cmd.Parameters.Add(New OracleParameter(":SS_QLEVEL", level))
                    cmd.Parameters.Add(New OracleParameter(":SS_CREATED_BY", pnoid))
                    cmd.Parameters.Add(New OracleParameter(":SS_SRL_NO", dt.Rows(cnt).Item(1).ToString.Trim))
                    cmd.Parameters.Add(New OracleParameter(":SS_SERIAL", ViewState("SRLNO").ToString()))
                    arr_cmd.Add(cmd)
                    cnt = cnt + 1
                End While

                ls_sql = "update T_SURVEY_STATUS set SS_Q2_A=:SS_Q2_A,SS_Q2_B=:SS_Q2_B,SS_WFL_STATUS='3' "
                ls_sql += ", SS_FEEDBACK_DT = sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY WHERE (SS_PNO=:SS_PNO or upper(SS_EMAIL || SS_INTSH_OTP) =:SS_PNO )"
                ls_sql += "  AND SS_ASSES_PNO=:SS_ASSES_PNO  and SS_YEAR =:SS_YEAR and SS_SRLNO=:SS_SRLNO"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                cmd = New OracleCommand(ls_sql, conHrps)
                cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
                cmd.Parameters.Add(New OracleParameter(":SS_Q2_A", txt1.ToString.Trim))
                cmd.Parameters.Add(New OracleParameter(":SS_Q2_B", txt2.ToString.Trim))
                cmd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", UCase(asseesse.ToString())))
                cmd.Parameters.Add(New OracleParameter(":SS_PNO", pnoid))
                cmd.Parameters.Add(New OracleParameter(":SS_UPDATED_BY", Session("ADM_USER").ToString))
                cmd.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
                arr_cmd.Add(cmd)
            End If





            If arr_cmd.Count > 0 Then
                Dim counter As Integer = 0
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim tran_Ins As OracleTransaction
                tran_Ins = conHrps.BeginTransaction()
                Try
                    For counter = 0 To arr_cmd.Count - 1
                        Dim con_ins As New OracleCommand()
                        con_ins = arr_cmd.Item(counter)
                        con_ins.Transaction = tran_Ins
                        con_ins.ExecuteNonQuery()
                    Next
                    tran_Ins.Commit()



                Catch ex As Exception
                    tran_Ins.Rollback()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while submiting response!")
                Finally
                    If conHrps.State = ConnectionState.Open Then
                        conHrps.Close()
                    End If
                End Try
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try


    End Sub
    Private Sub insertresponseIL5toIL6(ByVal asseesse As String, ByVal q1 As String, ByVal q2 As String, ByVal q3 As String, ByVal q4 As String, ByVal q5 As String, ByVal q6 As String, ByVal q7 As String, ByVal q8 As String, ByVal q9 As String, ByVal txt1 As String, ByVal txt2 As String, ByVal level As String)
        SessionTimeOut()
        Dim ls_sql As String = String.Empty
        Dim cmd As New OracleCommand()
        Dim arr_cmd As New ArrayList()
        Dim cnt As Integer = 0
        Dim res As New ArrayList()
        res.Add(q1)
        res.Add(q2)
        res.Add(q3)
        res.Add(q4)
        res.Add(q5)
        res.Add(q6)
        res.Add(q7)
        res.Add(q8)
        res.Add(q9)
        If asseesse = "" Then
            asseesse = ID
        End If

        Try

            ls_sql = "select SS_QCODE,SS_SRL_NO from t_survey_question where SS_QLEVEL=:SS_QLEVEL and SS_YEAR=:SS_YEAR order by SS_SRL_NO"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd = New OracleCommand(ls_sql, conHrps)
            cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            cmd.Parameters.Add(New OracleParameter(":SS_QLEVEL", level.Trim))
            Dim da As New OracleDataAdapter(cmd)
            Dim dt As New DataTable
            dt.Clear()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then

                While cnt < dt.Rows.Count

                    ls_sql = "insert into t_survey_response(SS_YEAR,SS_ASSES_PNO,SS_PNO,SS_QCODE,SS_QOPTN,SS_QLEVEL,SS_CREATED_BY,SS_CREATED_DT,SS_SRL_NO,SS_SERIAL)"
                    ls_sql += " values(:SS_YEAR,:SS_ASSES_PNO,:SS_PNO,:SS_QCODE,:SS_QOPTN,:SS_QLEVEL,:SS_CREATED_BY,sysdate,:SS_SRL_NO,:SS_SERIAL)"
                    If conHrps.State = ConnectionState.Closed Then
                        conHrps.Open()
                    End If
                    cmd = New OracleCommand(ls_sql, conHrps)
                    cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
                    cmd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", UCase(asseesse.ToString())))
                    cmd.Parameters.Add(New OracleParameter(":SS_PNO", UCase(Session("User_Id"))))
                    cmd.Parameters.Add(New OracleParameter(":SS_QCODE", dt.Rows(cnt).Item(0).ToString.Trim))
                    cmd.Parameters.Add(New OracleParameter(":SS_QOPTN", res(cnt)))
                    cmd.Parameters.Add(New OracleParameter(":SS_QLEVEL", level))
                    cmd.Parameters.Add(New OracleParameter(":SS_CREATED_BY", UCase(Session("User_Id"))))
                    cmd.Parameters.Add(New OracleParameter(":SS_SRL_NO", dt.Rows(cnt).Item(1).ToString.Trim))
                    cmd.Parameters.Add(New OracleParameter(":SS_SERIAL", ViewState("SRLNO").ToString()))
                    arr_cmd.Add(cmd)
                    cnt = cnt + 1
                End While

                ls_sql = "update T_SURVEY_STATUS set SS_Q2_A=:SS_Q2_A,SS_Q2_B=:SS_Q2_B,SS_WFL_STATUS='3' "
                ls_sql += ", SS_FEEDBACK_DT = sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY WHERE (SS_PNO=:SS_PNO or upper(SS_EMAIL || SS_INTSH_OTP) =:SS_PNO )"
                ls_sql += "  AND SS_ASSES_PNO=:SS_ASSES_PNO  and SS_YEAR =:SS_YEAR  and SS_SRLNO=:SS_SRLNO"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                cmd = New OracleCommand(ls_sql, conHrps)
                cmd.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
                cmd.Parameters.Add(New OracleParameter(":SS_Q2_A", txt1.ToString.Trim))
                cmd.Parameters.Add(New OracleParameter(":SS_Q2_B", txt2.ToString.Trim))
                cmd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", UCase(asseesse.ToString())))
                cmd.Parameters.Add(New OracleParameter(":SS_PNO", UCase(Session("User_Id").ToString())))
                cmd.Parameters.Add(New OracleParameter(":SS_UPDATED_BY", UCase(Session("User_Id").ToString())))
                cmd.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
                arr_cmd.Add(cmd)
            End If





            If arr_cmd.Count > 0 Then
                Dim counter As Integer = 0
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim tran_Ins As OracleTransaction
                tran_Ins = conHrps.BeginTransaction()
                Try
                    For counter = 0 To arr_cmd.Count - 1
                        Dim con_ins As New OracleCommand()
                        con_ins = arr_cmd.Item(counter)
                        con_ins.Transaction = tran_Ins
                        con_ins.ExecuteNonQuery()
                    Next
                    tran_Ins.Commit()



                Catch ex As Exception
                    tran_Ins.Rollback()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while submiting response!")
                Finally
                    If conHrps.State = ConnectionState.Open Then
                        conHrps.Close()
                    End If
                End Try
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try


    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Protected Sub btn_reject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_reject.Click
    End Sub

    Public Sub reject()
        Try
            SessionTimeOut()
            Dim OrgStr As String = String.Empty
            OrgStr = "update T_SURVEY_STATUS set SS_WFL_STATUS='9', SS_FEEDBACK_DT = sysdate ,SS_UPDATED_BY=:SS_UPDATED_BY, SS_UPDATED_DT=sysdate"
            OrgStr += " WHERE (SS_PNO=:SS_PNO or upper(SS_EMAIL ||SS_INTSH_OTP) =:SS_PNO ) AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_YEAR =:SS_YEAR and  SS_SRLNO=:SS_SRLNO"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim comnd As New OracleCommand()
            comnd.Connection = conHrps
            comnd.CommandText = OrgStr
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_PNO", UCase(Session("USER_ID").ToString()))
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", UCase(Session("AssesPno").ToString()))
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim rs = comnd.ExecuteNonQuery()

            If rs > 0 Then
                'ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "Savedtls", "info('Thank you for your response!You had insufficient exposure to provide feedback','','success');", True)
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Thank you for your response!You had insufficient exposure to provide feedback")
                bindPendingRecord("")
                pnl.Visible = False
                Dim stat = ChkValidation()
                If Len(stat) > 0 Then
                    'ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & stat & " Category")

                    SentMailReturned(UCase(Session("AssesPno").ToString()), stat)
                    Exit Sub
                End If
            End If


        Catch ex As Exception
            'MsgBox(ex.ToString)

            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while Insufficient feedback : - Respondent" & Session("USER_ID").ToString() & "  Assess P.no:- " & Session("AssesPno").ToString() & "  Year" & ViewState("FY").ToString() & Environment.NewLine
            Dim val = ex.ToString
            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while clicking Insufficient feedback")

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub
    Public Sub SentMailReturned(pno As String, remarks As String)

        '''' send mail in case of reject survey to buhr
        Try
            Dim strmailcomd As New OracleCommand()
            strmailcomd.CommandText = "select ema_email_id from t_emp_master_feedback360 where ema_perno = (select ema_bhr_pno from t_emp_master_feedback360 where ema_perno='" + pno.ToString + "') and ema_email_id is not null "
            'strmailcomd.CommandText = "select ema_email_id from t_emp_master_feedback360 where ema_perno = '148536'"
            Dim df = getRecordInDt(strmailcomd, conHrps)

            Dim strDetailscomd As New OracleCommand()
            strDetailscomd.CommandText = "select EMA_ENAME,EMA_DESGN_DESC from t_emp_master_feedback360 where ema_perno ='" + pno.ToString + "'"
            Dim dtDetails = getRecordInDt(strDetailscomd, conHrps)


            Dim wordsDetails11 As String() = remarks.ToString.Split(New Char() {"("c})

            If df.Rows.Count > 0 Then

                Dim body As String = String.Empty
                body = "Dear Colleague, <br/><br/><br/>"
                body += "The list of respondents for " + dtDetails.Rows(0)("EMA_ENAME").ToString + " (" + pno.ToString + "), " + dtDetails.Rows(0)("EMA_DESGN_DESC").ToString + " , has fallen below the minimum number of respondents for the " + wordsDetails11(0).ToString + " category on account of rejection of survey. You are requested to add " + wordsDetails11(1).ToString + " more respondent/s to the aforementioned category at the earliest.<br/><br/>"
                body += " <br/><br/> With Regards,<br/>"
                body += "HRM Team<br/><br/> <b>This is system generated mail.Please do not reply</b>"


                Dim mail As New System.Net.Mail.MailMessage()
                mail.Bcc.Add(df.Rows(0)("ema_email_id").ToString)
                mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

                mail.Subject = "REMINDER : Add more respondents for " + dtDetails.Rows(0)("EMA_ENAME").ToString + " (" + pno.ToString + "), " + dtDetails.Rows(0)("EMA_DESGN_DESC").ToString + "  in the end-year 360-degree assessment"

                mail.SubjectEncoding = System.Text.Encoding.UTF8
                mail.Body = body

                mail.BodyEncoding = System.Text.Encoding.UTF8
                mail.IsBodyHtml = True
                Dim client As New SmtpClient()
                client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
                client.Port = 25
                client.Host = "144.0.11.253"
                ' client.Host = "144.0.16.7"
                client.EnableSsl = False
                client.Send(mail)
                client.Dispose()

            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Public Function ChkAuthlabel(pno As String) As String
        Try
            Dim chk As String = String.Empty
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim qry1 As New OracleCommand()
            'qry1.CommandText = "select SS_IL from t_assesse_IL  where SS_ASSESS_PNO=:ema_perno and SS_STATUS='A'"
            qry1.CommandText = "select EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where EMA_PERNO=:ema_perno and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
            qry1.Connection = conHrps
            qry1.Parameters.Clear()
            qry1.Parameters.AddWithValue("ema_perno", pno.ToString())
            qry1.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString())
            qry1.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString())
            Dim daIL As New OracleDataAdapter(qry1)
            Dim dtIL As New DataTable()
            daIL.Fill(dtIL)
            If dtIL.Rows.Count > 0 Then
                chk = dtIL.Rows(0).Item("EMA_EQV_LEVEL")
            Else
                chk = String.Empty
            End If
            Return chk
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.Message)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function
    Public Function ChkValidation() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lblChk = ChkAuthlabel(Session("AssesPNo").ToString())
            If lblChk.Equals("I5") Then
                type = "36V5"
            ElseIf lblChk.Equals("I4") Then
                type = "36V4"
            ElseIf lblChk.Equals("I3") Then
                type = "36V3"
            ElseIf lblChk.Equals("I6") Then
                type = "36V6"
            ElseIf lblChk.Equals("I2") Then
                type = "36V2"
            ElseIf lblChk.Equals("I1") Then
                type = "36V1"
            End If
            ' Start WI368  by Manoj Kumar on 30-05-2021 
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,decode(b.irc_desc,'Peer','Peers And Subordinates',b.irc_desc) irc_desc from t_ir_codes a,t_ir_codes b "
            If lblChk.Equals("I2") Or lblChk.Equals("I1") Then
                cmdqry.CommandText = ""
                cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,decode(b.irc_desc,'Peer','Peers',b.irc_desc) irc_desc from t_ir_codes a,t_ir_codes b "
            End If
            cmdqry.CommandText += "  where a.irc_type='" + type + "' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE not in('SELF')"
            If type.Equals("36V5") Or type.Equals("36V6") Or type.Equals("36V4") Then
                cmdqry.CommandText += " and a.IRC_CODE not in('PEER','ROPT')"
            End If
            Dim dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_WFL_STATUS not in ('9') and SS_SRLNO=:SS_SRLNO and SS_CATEG not in ('Self')"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("AssesPNo").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)

                    If dtls.Rows.Count < dt.Rows(i)("minmum") Then
                        Dim minVal = Val(dt.Rows(i)("minmum").ToString) - Val(dtls.Rows.Count)
                        status += dt.Rows(i)("irc_desc").ToString() & "(" & minVal & ", "
                        'status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("minmum").ToString() & "), "
                    End If

                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    'Public Sub UpdateFeedbackData(ByVal pno As String, ByVal Q1a As String, ByVal Q1b As String, ByVal Q1c As String, ByVal Q1d As String, ByVal Q2A As String, ByVal Q2B As String)
    '    Dim trans As OracleTransaction
    '    Try



    '        Dim OrgStr As String = String.Empty

    '        If pno = "" Then
    '            pno = ID
    '        End If
    '        OrgStr = "update T_SURVEY_STATUS set SS_Q1_A=:SS_Q1_A,SS_Q1_B=:SS_Q1_B,SS_Q1_C=:SS_Q1_C,SS_Q1_D=:SS_Q1_D,SS_Q2_A=:SS_Q2_A,SS_Q2_B=:SS_Q2_B,SS_WFL_STATUS='3' "
    '        OrgStr += ", SS_FEEDBACK_DT = sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY WHERE (SS_PNO=:SS_PNO or upper(SS_EMAIL || SS_INTSH_OTP) =:SS_PNO )"
    '        OrgStr += "  AND SS_ASSES_PNO=:SS_ASSES_PNO  and SS_YEAR =:SS_YEAR"
    '        If conHrps.State = ConnectionState.Closed Then
    '            conHrps.Open()
    '        End If

    '        trans = conHrps.BeginTransaction()
    '        Dim comnd As New OracleCommand()
    '        comnd.Connection = conHrps
    '        comnd.CommandText = OrgStr
    '        comnd.Parameters.Clear()
    '        comnd.Parameters.AddWithValue("SS_Q1_A", Q1a.ToString)
    '        comnd.Parameters.AddWithValue("SS_Q1_B", Q1b.ToString)
    '        comnd.Parameters.AddWithValue("SS_Q1_C", Q1c.ToString)
    '        comnd.Parameters.AddWithValue("SS_Q1_D", Q1d.ToString)
    '        comnd.Parameters.AddWithValue("SS_Q2_A", Q2A.ToString)
    '        comnd.Parameters.AddWithValue("SS_Q2_B", Q2B.ToString)
    '        comnd.Parameters.AddWithValue("SS_PNO", UCase(Session("USER_ID").ToString()))
    '        comnd.Parameters.AddWithValue("SS_UPDATED_BY", UCase(Session("USER_ID").ToString()))
    '        ' comnd.Parameters.AddWithValue("SS_PNO", "145")
    '        comnd.Parameters.AddWithValue("SS_ASSES_PNO", UCase(pno.ToString()))
    '        comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
    '        'comnd.ExecuteNonQuery()
    '        'comnd.ExecuteNonQuery()
    '        comnd.Transaction = trans
    '        Dim result = comnd.ExecuteNonQuery()
    '        If result > 0 Then
    '            trans.Commit()
    '            bindPendingRecord()
    '            pnl.Visible = False
    '            ShowGenericMessageModal(CommonConstants.AlertType.success, "Thank you for your response! Your Feedback got submitted successfully")
    '        Else

    '            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
    '            Dim dividesterr = "0 Rows updated....!!! " & Environment.NewLine
    '            Dim val = "Respondent " & Session("USER_ID").ToString() & " Year " & ViewState("FY").ToString() & " Assess " & pno & Environment.NewLine
    '            File.AppendAllText(fnameerr, dividesterr)
    '            File.AppendAllText(fnameerr, val)
    '            trans.Rollback()
    '            ShowGenericMessageModal(CommonConstants.AlertType.info, "Feedback not submitted please try again")

    '        End If


    '        ' ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "Savedtls", "info('Thank you for your response! Your Feedback got submitted successfully','','success');", True)

    '    Catch ex As Exception
    '        trans.Rollback()
    '        ' MsgBox(ex.ToString)
    '        Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
    '        Dim dividesterr = "Error while providing feedback :- Respondent" & Session("USER_ID").ToString() & "  Assess P.no:- " & pno & "  Year" & ViewState("FY").ToString() & Environment.NewLine
    '        Dim val = ex.ToString
    '        File.AppendAllText(fnameerr, dividesterr)
    '        File.AppendAllText(fnameerr, val)
    '        ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while providing feedback")
    '    Finally
    '        If conHrps.State = ConnectionState.Open Then
    '            conHrps.Close()
    '        End If
    '    End Try

    'End Sub
    Protected Sub gvPending_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPending.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim b1 As LinkButton = CType(e.Row.Cells(4).FindControl("lbselect"), LinkButton)
                Dim lblStage As Label = CType(e.Row.Cells(5).FindControl("lblstatus"), Label)
                Dim status As String = ""
                status = lblStage.Text.ToString()
                If status = "Pending" Then
                    'b1.BackColor = System.Drawing.Color.Yellow
                    'b1.ForeColor = System.Drawing.Color.Black
                    b1.CssClass = "btn-learn-more"
                    b1.Text = "Pending"
                ElseIf status = "Completed" Then
                    b1.Text = "Completed"
                    b1.CssClass = "btn-learn-more"
                Else
                    b1.Text = "Rejected"
                    b1.CssClass = "btn-learn-more"
                End If

            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Function PageValid() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim mail = SimpleCrypt(Request.QueryString("id"))
            Session.Remove("statusotp")
            Session.Remove("otpsend")
            Dim sql As String = String.Empty
            Dim qry As New OracleCommand()
            Dim pno = Session("USER_ID").ToString()
            'Dim ocmd As New OracleCommand()
            'ocmd.CommandText = "select IRC_CODE from t_ir_codes where irc_type ='360PG' and trunc(IRC_START_DT) <= trunc(sysdate)"
            'ocmd.CommandText += "  and trunc(IRC_END_DT) >= trunc(sysdate) and IRC_VALID_TAG='A' and upper(irc_desc)='FEEDBACK_OPR.ASPX'"
            sql = "select max(trunc(ema_step3_enddt)) from hrps.t_emp_master_feedback360, t_survey_status where ss_asses_pno=ema_perno and SS_YEAR=ema_year and SS_SRLNO=ema_cycle  and SS_YEAR =:SS_YEAR and SS_DEL_TAG='N' "
            sql += "and ss_app_tag='AP' and nvl(ss_wfl_status,0) not in ('1','0') and ema_comp_code='1000' "
            If mail <> "" Then
                mail = mail.ToUpper().Trim
                sql += " and TRIM(upper(SS_EMAIL))||SS_INTSH_OTP=:SS_INTSH_OTP and SS_SRLNO=:SS_SRLNO"
            Else
                sql += " and SS_PNO=:SS_PNO and SS_SRLNO=:SS_SRLNO"
            End If

            sql += " and trunc(EMA_STEP3_STDT)<=trunc(sysdate) and trunc(ema_step3_enddt)>=trunc(sysdate)"

            qry.Connection = conHrps
            qry.CommandText = sql
            qry.Parameters.Clear()
            qry.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            qry.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
            If mail <> "" Then
                qry.Parameters.Add(New OracleParameter(":SS_INTSH_OTP", mail))
            Else
                qry.Parameters.Add(New OracleParameter(":SS_PNO", pno.ToUpper().Trim()))
            End If

            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)(0).ToString <> "" Then
                    isvalid = True
                Else
                    isvalid = False
                End If
            Else
                isvalid = False
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return isvalid
    End Function

    Public Function PageValidRespondent(respon As String) As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd1 As New OracleCommand()
            ocmd1.CommandText = "select SPA_RESPONDENT,SPA_ASSPNO from t_survey_pg_activation where SPA_TYPE ='360EX' and trunc(SPA_START_DT) <= trunc(sysdate) "
            ocmd1.CommandText += " and trunc(SPA_END_DT) <= trunc(sysdate) and SPA_VALID_TAG='A' and upper(SPA_RESPONDENT)=:irc_desc "
            ocmd1.Parameters.Clear()
            ocmd1.Connection = conHrps
            ocmd1.Parameters.AddWithValue("irc_desc", respon.ToUpper())
            ' Dim vc = getRecordInDt(ocmd1, conHrps)
            Dim da As New OracleDataAdapter(ocmd1)
            Dim vc As New DataTable()
            da.Fill(vc)
            If vc.Rows.Count > 0 Then
                isvalid = True
                For k As Integer = 0 To vc.Rows.Count - 1
                    aspno.Text += "'" & vc.Rows(k)("SPA_ASSPNO").ToString() & "',"
                Next

            Else
                isvalid = False
                Session.Remove("aspno")
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return isvalid
    End Function
End Class
