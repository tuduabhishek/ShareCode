Imports System.Data.OracleClient
Imports System.Data
Imports System.IO

Partial Class Feedback
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim dtls As DataTable
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strUserID As String = ""
        Dim strUserDomain As String = ""

        If PageValid() Then
            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If



            'strUserID = "197296"
            Session("USER_DOMAIN") = strUserDomain.ToUpper()
            ' Session("USER_ID") = "150292"
            Dim mail = SimpleCrypt(Request.QueryString("id"))
            If mail <> "" Then
                lblname.Text = GetPno(mail)
                Session("USER_ID") = mail.ToUpper()
                'strUserID = "119336"
                If CheckLvl(mail.Substring(0, mail.Length - 4).ToUpper()) Then

                Else
                    Response.Write("<center> <b><I> This website has been closed </b></I></center>")
                    Me.Page.Visible = False
                    Exit Sub
                End If
            Else
                'lblname.Text = GetPno(strUserID)
                ' strUserID = "151629"
                Session("USER_ID") = strUserID.ToUpper()
                lblname.Text = GetPno(strUserID)
                'lblname.Text = strUserID
                'lblname.Text = "Avirup Bhowmick"
                'Session("USER_ID") = "162523"
            End If
            GetFy()
            'lblname.Text = "Shruti Choudhury"
            bindPendingRecord()
        Else
            Response.Write("<center> <b><I> This website has been closed </b></I></center>")
            Me.Page.Visible = False
            Exit Sub
        End If
    End Sub

    Public Sub GetFy()
        Try

            Dim s1 As New OracleCommand()
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s1.CommandText = "Select IRC_DESC from t_ir_codes where irc_type='360DT' and IRC_VALID_TAG='A'"
            Dim f1 = getRecordInDt(s1, conHrps)

            If f1.Rows.Count > 0 Then
                ViewState("FY") = f1.Rows(0)(0)
            Else
                Dim s As New OracleCommand()
                's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
                s.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY FROM DUAL"
                Dim f = getRecordInDt(s, conHrps)

                If f.Rows.Count > 0 Then
                    ViewState("FY") = f.Rows(0)(0)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function GetPno(pernr As String) As String
        Dim perno As String = ""
        Try

            Dim cm As New OracleCommand()
            cm.CommandText = "  select SS_NAME from t_survey_status"
            If pernr.Length > 6 Then
                cm.CommandText += " where TRIM(upper(SS_EMAIL))||SS_INTSH_OTP=:SS_INTSH_OTP "
            Else
                cm.CommandText += " where upper(ss_pno) =:SS_INTSH_OTP"
            End If
            'Dim d = getRecordInDt(cm, conHrps)
            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("SS_INTSH_OTP", pernr.ToUpper().ToString())
            Dim da As New OracleDataAdapter(cm)
            Dim d As New DataTable()
            da.Fill(d)
            If d.Rows.Count > 0 Then
                perno = d.Rows(0)("SS_NAME").ToString()
            End If
        Catch ex As Exception

        End Try

        Return perno
    End Function
    Protected Sub bindDetl()
        Try

            Dim qry As New OracleCommand()
            qry.CommandText = "select SS_Q1_A,SS_Q1_B,SS_Q1_C,SS_Q1_D,SS_Q2_A,SS_Q2_B from t_survey_status where( SS_PNO=:pno "
            qry.CommandText += " or upper(ss_email ||SS_INTSH_OTP )=:pno)  and ss_asses_pno=:ss_asses_pno and ss_year=:ss_year"
            'Dim dt1 = getRecordInDt(qry, conHrps)

            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("pno", Session("USER_ID").ToUpper().ToString())
            qry.Parameters.AddWithValue("ss_asses_pno", Session("AssesPNo").ToString())
            qry.Parameters.AddWithValue("ss_year", ViewState("FY"))
            Dim da As New OracleDataAdapter(qry)
            Dim dt1 As New DataTable()
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                rblQ1a.SelectedValue = dt1.Rows(0)("SS_Q1_A").ToString()
                rblQ1b.SelectedValue = dt1.Rows(0)("SS_Q1_B").ToString()
                rblQ1c.SelectedValue = dt1.Rows(0)("SS_Q1_C").ToString()
                rblQ1d.SelectedValue = dt1.Rows(0)("SS_Q1_D").ToString()
                txtAns1.Text = dt1.Rows(0)("SS_Q2_A").ToString()
                txtAns2.Text = dt1.Rows(0)("SS_Q2_B").ToString()
            End If
        Catch ex As Exception
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while load bindDetl function :- Respondent " & Session("USER_ID").ToString() & "  Assess P.no:- " & Session("AssesPno").ToString() & "  Year" & ViewState("FY").ToString() & Environment.NewLine
            Dim val = ex.ToString
            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while bindDetl function")
        End Try
    End Sub
    Public Sub bindPendingRecord()
        Try
            Dim mail = SimpleCrypt(Request.QueryString("id"))

            Dim qry As New OracleCommand()
            Dim pno = Session("USER_ID").ToString()
            'qry.CommandText = "select ss_asses_pno,ema_ename,ema_desgn_desc,ema_dept_desc,'Pending' status from tips.t_empl_all, t_survey_status "
            'qry.CommandText += "where ss_asses_pno=ema_perno and nvl(ss_wfl_status,'0')<>'3' and SS_PNO='" & pno & "'"
            qry.CommandText = "select ss_asses_pno,ema_ename,ema_desgn_desc,ema_dept_desc,decode(ss_wfl_status,'2','Pending','3','Completed','9','Insufficient exposure to provide feedback') status from tips.t_empl_all, t_survey_status "
            qry.CommandText += "where ss_asses_pno=ema_perno and ss_app_tag='AP'  and SS_YEAR =:SS_YEAR and EMA_EQV_LEVEL='I1'"

            If mail <> "" Then
                qry.CommandText += " and TRIM(upper(SS_EMAIL))||SS_INTSH_OTP=:SS_INTSH_OTP"
            Else
                qry.CommandText += " and SS_PNO=:SS_PNO"
            End If
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            If mail <> "" Then
                qry.Parameters.AddWithValue("SS_INTSH_OTP", mail.ToUpper().Trim())
            Else
                qry.Parameters.AddWithValue("SS_PNO", pno.ToUpper().Trim())
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
                Response.Redirect("survey.aspx", True)
            End If

        Catch ex As Exception
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while load fedabck pending or complete :- Respondent " & Session("USER_ID").ToString() & "  Year " & ViewState("FY").ToString() & Environment.NewLine
            Dim val = ex.ToString & Environment.NewLine & Environment.NewLine
            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while displaying pending/complete data")
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
    Protected Sub gvPending_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            lblRateNm.Text = String.Empty
            txtAns1.Text = String.Empty
            txtAns2.Text = String.Empty
            lblRecipientNm1.Text = String.Empty
            lblRecipientNm3.Text = String.Empty
            lblRecipientNm4.Text = String.Empty
            rblQ1a.SelectedValue = "1"
            rblQ1b.SelectedValue = "1"
            rblQ1c.SelectedValue = "1"
            rblQ1d.SelectedValue = "1"
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
                lblRateNm.Text = lbl_name.ToString()
                lblRecipientNm1.Text = lbl_name.ToString()
                lblRecipientNm3.Text = lbl_name.ToString()
                lblRecipientNm4.Text = lbl_name.ToString()
                pnl.Visible = True
                If lbl_stage = "Pending" Then
                    rblQ1a.Enabled = True
                    rblQ1b.Enabled = True
                    rblQ1c.Enabled = True
                    rblQ1d.Enabled = True
                    txtAns2.Enabled = True
                    txtAns1.Enabled = True
                    btn_submit.Visible = True
                    btn_reject.Visible = True
                ElseIf lbl_stage = "Completed" Then
                    btn_submit.Visible = False
                    btn_reject.Visible = False
                    rblQ1a.Enabled = False
                    rblQ1b.Enabled = False
                    rblQ1c.Enabled = False
                    rblQ1d.Enabled = False
                    txtAns2.Enabled = False
                    txtAns1.Enabled = False
                    bindDetl()

                Else
                    btn_submit.Visible = False
                    btn_reject.Visible = False
                    rblQ1a.Enabled = False
                    rblQ1b.Enabled = False
                    rblQ1c.Enabled = False
                    rblQ1d.Enabled = False
                    txtAns2.Enabled = False
                    txtAns1.Enabled = False
                    bindDetl()

                End If

            End If
        Catch ex As Exception
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while load fedabck pending or complete :- Respondent" & Session("USER_ID").ToString() & "  Year" & ViewState("FY").ToString() & Environment.NewLine
            Dim val = ex.ToString
            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while populating data")
        End Try
    End Sub
    Protected Sub btn_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_submit.Click

    End Sub

    Public Sub submit()
        Try
            If txtAns1.Text.Trim = "" Or txtAns2.Text.Trim = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill answer box")
                Exit Sub
            End If
            If Session("AssesPno").ToString = "" Or ViewState("FY").ToString = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Your session has been expired please refresh the page and submit again...!")
                Exit Sub
            End If
            UpdateFeedbackData(Session("AssesPno"), rblQ1a.SelectedValue.ToString, rblQ1b.SelectedValue.ToString, rblQ1c.SelectedValue.ToString, rblQ1d.SelectedValue.ToString, txtAns1.Text.Trim().ToString, txtAns2.Text.Trim().ToString)

        Catch ex As Exception

        End Try
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Protected Sub btn_reject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_reject.Click
    End Sub

    Public Sub reject()
        Try
            Dim OrgStr As String = String.Empty
            OrgStr = "update T_SURVEY_STATUS set SS_WFL_STATUS='9', SS_FEEDBACK_DT = sysdate ,SS_UPDATED_BY=:SS_UPDATED_BY, SS_UPDATED_DT=sysdate"
            OrgStr += " WHERE (SS_PNO=:SS_PNO or upper(SS_EMAIL ||SS_INTSH_OTP) =:SS_PNO ) AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_YEAR =:SS_YEAR"

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
            Dim rs = comnd.ExecuteNonQuery()

            If rs > 0 Then
                'ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "Savedtls", "info('Thank you for your response!You had insufficient exposure to provide feedback','','success');", True)
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Thank you for your response!You had insufficient exposure to provide feedback")
                bindPendingRecord()
                pnl.Visible = False
            End If


        Catch ex As Exception
            'MsgBox(ex.ToString)

            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while Insufficient feedback :- Respondent" & Session("USER_ID").ToString() & "  Assess P.no:- " & Session("AssesPno").ToString() & "  Year" & ViewState("FY").ToString() & Environment.NewLine
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
    Public Sub UpdateFeedbackData(ByVal pno As String, ByVal Q1a As String, ByVal Q1b As String, ByVal Q1c As String, ByVal Q1d As String, ByVal Q2A As String, ByVal Q2B As String)
        Dim trans As OracleTransaction
        Try



            Dim OrgStr As String = String.Empty

            If pno = "" Then
                pno = ID
            End If
            OrgStr = "update T_SURVEY_STATUS set SS_Q1_A=:SS_Q1_A,SS_Q1_B=:SS_Q1_B,SS_Q1_C=:SS_Q1_C,SS_Q1_D=:SS_Q1_D,SS_Q2_A=:SS_Q2_A,SS_Q2_B=:SS_Q2_B,SS_WFL_STATUS='3' "
            OrgStr += ", SS_FEEDBACK_DT = sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY WHERE (SS_PNO=:SS_PNO or upper(SS_EMAIL || SS_INTSH_OTP) =:SS_PNO )"
            OrgStr += "  AND SS_ASSES_PNO=:SS_ASSES_PNO  and SS_YEAR =:SS_YEAR"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            trans = conHrps.BeginTransaction()
            Dim comnd As New OracleCommand()
            comnd.Connection = conHrps
            comnd.CommandText = OrgStr
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_Q1_A", Q1a.ToString)
            comnd.Parameters.AddWithValue("SS_Q1_B", Q1b.ToString)
            comnd.Parameters.AddWithValue("SS_Q1_C", Q1c.ToString)
            comnd.Parameters.AddWithValue("SS_Q1_D", Q1d.ToString)
            comnd.Parameters.AddWithValue("SS_Q2_A", Q2A.ToString)
            comnd.Parameters.AddWithValue("SS_Q2_B", Q2B.ToString)
            comnd.Parameters.AddWithValue("SS_PNO", UCase(Session("USER_ID").ToString()))
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", UCase(Session("USER_ID").ToString()))
            ' comnd.Parameters.AddWithValue("SS_PNO", "145")
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", UCase(pno.ToString()))
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            'comnd.ExecuteNonQuery()
            'comnd.ExecuteNonQuery()
            comnd.Transaction = trans
            Dim result = comnd.ExecuteNonQuery()
            If result > 0 Then
                trans.Commit()
                bindPendingRecord()
                pnl.Visible = False
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Thank you for your response! Your Feedback got submitted successfully")
            Else

                Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
                Dim dividesterr = "0 Rows updated....!!! " & Environment.NewLine
                Dim val = "Respondent " & Session("USER_ID").ToString() & " Year " & ViewState("FY").ToString() & " Assess " & pno & Environment.NewLine
                File.AppendAllText(fnameerr, dividesterr)
                File.AppendAllText(fnameerr, val)
                trans.Rollback()
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Feedback not submitted please try again")

            End If


            ' ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "Savedtls", "info('Thank you for your response! Your Feedback got submitted successfully','','success');", True)

        Catch ex As Exception
            trans.Rollback()
            ' MsgBox(ex.ToString)
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr = "Error while providing feedback :- Respondent" & Session("USER_ID").ToString() & "  Assess P.no:- " & pno & "  Year" & ViewState("FY").ToString() & Environment.NewLine
            Dim val = ex.ToString
            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, val)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while providing feedback")
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub
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
                    b1.Text = "Insufficient exposure to provide feedback"
                    b1.CssClass = "btn-learn-more"
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function PageValid() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            ocmd.CommandText = "select IRC_CODE from t_ir_codes where irc_type ='360PG' and to_char(IRC_START_DT,'dd/mm/yyyy') <= to_char(sysdate,'dd/mm/yyyy')"
            ocmd.CommandText += "  and to_char(IRC_END_DT,'dd/mm/yyyy') >= to_char(sysdate,'dd/mm/yyyy') and IRC_VALID_TAG='A' and upper(irc_desc)='FEEDBACK.ASPX'"
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

    Public Function CheckLvl(perno As String) As Boolean
        Dim flag As Boolean = False
        Try
            Dim qry As New OracleCommand

            qry.CommandText = "select * from tips.t_empl_all  where ema_perno=:ema_perno or upper(ema_email_id)=:ema_perno and EMA_EQV_LEVEL='I1' and ema_disch_dt is null AND EMA_COMP_CODE='1000'"
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_perno", perno.ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                flag = True
            Else
                flag = False

            End If
        Catch ex As Exception
            MsgBox("err")
        End Try
        Return flag
    End Function
End Class
