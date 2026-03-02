Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Net.Mail
Imports ClosedXML.Excel
'Imports System.Web.Helpers

Partial Class Admin
    Inherits System.Web.UI.MasterPage
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim pernoList1000, pernoList2000, pernoList3000, pernoList4000, pernoList5000, pernoList6000, pernoList7000, pernoList8000, pernoList9000, pernoList10000 As String
    Public isSuperAdmin, isLD, isHRBP As Boolean
    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Session("USER_ID") = "805324"
        Dim strUserID As String = Session("USER_ID")
        Dim strUserDomain As String = ""
        Session("ADM_USER") = Session("USER_ID")
        'CheckSupAdm(Session("ADM_USER"))
        'Session.Clear()
        'Session.RemoveAll()
        'Session.Abandon()
        getsrlno()
        getFy()
        If Session("ADM_USER") = "" Then

            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            'strUserID = "BCZPA7350M"
            'CheckSupAdm(strUserID.ToUpper())
            'If strUserID = "148536" Then
            '    strUserID = "198777"
            '    'ElseIf strUserID = "197838" Then
            '    '    strUserID = "198777"
            '    'ElseIf strUserID = "153815" Then
            '    '    strUserID = "197994"
            'End If
            Session("ADM_USER") = strUserID.ToUpper()

            If GetPno(strUserID.ToUpper()) = False Then
                Session("errorMsg") = "You don't have admin role."
                Response.Redirect("errorpage.aspx", True)
            End If
            'lblname.Text = "Suresh Dutt Tripathi"
        ElseIf (Session("ADM_USER") Is Nothing) AndAlso (Session("ADM_USER").Equals("") = False) Then
            'Session("errorMsg") = "Your session has been expired. Kindly open the admin page.."
            'Response.Redirect("errorpage.aspx", True)
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Exit Sub
            'strUserID = "1978838"
            'Session("ADM_USER") = strUserID.ToUpper()
            ' lblname.Text = GetPno().Rows(0)(1) '"Suresh Dutt Tripathi"
            'Return
        Else

            'visibleRlink(strUserID)
            'If GetPno(Session("ADM_USER").ToUpper()) = False Then
            '    Session("errorMsg") = "You don't have admin role."
            '    Response.Redirect("errorpage.aspx", True)
            'End If
        End If
    End Sub
    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Session("errorMsg") = "You don't have access of this page."
        'Response.Redirect("errorpage.aspx", True)
        'Exit Sub
        loadLoggedInUserIDAndDomainIntoSession()
        'Added by TCS on 13122023, Added Role based validation
        Dim thisURL As String = Request.Url.Segments(Request.Url.Segments.Count - 1)
        Dim isValid = CheckAuthentication(Session("ADM_USER"), thisURL)
        If Not isValid Then
            Session("errorMsg") = "You don't have access of this page."
            Response.Redirect("errorpage.aspx", True)
        End If
        'End changes role based validation
    End Sub
    Private Sub getFy()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("IRC_DESC").ToString()
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
    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim q As New OracleCommand()
            q.Connection = conHrps
            'q.CommandText = "Select ema_perno,ema_ename from t_ir_adm_grp_privilege,hrps.t_emp_master_feedback360 where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            'q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno & "' and EMA_COMP_CODE='1000' and ema_year='" + ViewState("FY") + "' and EMA_CYCLE='" + ViewState("SRLNO") + "'"
            'q.CommandText += " union select ema_bhr_pno ema_perno,'' from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & pno & "' AND EMA_CYCLE='" & ViewState("SRLNO").ToString & "' AND EMA_YEAR='" & ViewState("FY").ToString & "' and rownum=1"
            ''WI624: allow TCS employee to log into admin page as super admin, created by : Avik Mukherjee, created on: 16-06-2021
            'q.CommandText += " union select IGP_user_id ema_perno,'' from  t_ir_adm_grp_privilege where IGP_user_id='" & pno & "' and igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A'"
            '' q.Parameters.Clear()
            ''q.Parameters.AddWithValue("IGP_user_id", Session("ADM_USER").ToString())

            'Added by TCS on 13122023, Added Role based validation
            q.CommandText = "Select ema_perno,ema_ename from t_ir_adm_grp_privilege,hrps.t_emp_master_feedback360 where igp_group_id IN ('360DF_SA','360DF_LD')  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno & "' and EMA_COMP_CODE='1000' and ema_year='" + ViewState("FY") + "' and EMA_CYCLE='" + ViewState("SRLNO") + "'"
            q.CommandText += " UNION select ema_bhr_pno, EMA_BHR_NAME ema_ename  from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & pno & "' AND EMA_CYCLE='" & ViewState("SRLNO").ToString & "' AND EMA_YEAR='" & ViewState("FY").ToString & "' and rownum=1"
            q.CommandText += " UNION select IGP_user_id, '' ema_ename from hrps.t_ir_adm_grp_privilege where igp_group_id IN ('360DF_SA','360DF_LD')  "
            q.CommandText += " and IGP_STATUS ='A' and IGP_user_id='" & pno & "'"
            'End changes role based validation


            Dim p = getDataInDt(q)
            If p.Rows.Count > 0 Then
                Session("UserName") = p.Rows(0)("ema_ename").ToString
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
            'MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            SessionTimeOut()
            'If IsPostBack Then
            '    AntiForgery.Validate()
            'End If
            If Not IsPostBack Then
                lblSessionNm.Text = Session("UserName")
            End If
            'Added by TCS on 13122023, Added Role based validation
            'CheckSupAdm(Session("ADM_USER").ToString())
            CheckUserRole()
            'End
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
    Public Sub CheckSupAdm(pno As String)
        Try
            'Dim qry As String = String.Empty
            Dim chkAdmin = ChkRole1()
            If chkAdmin = True Then
                lblname.Text = "Super Admin"
                'la.Visible = True 'Change by TCS on 070922 (Enable Individual report for Super Admin)
                la1.Visible = True
                la2.Visible = True
                'la3.Visible = True
                'A4.Visible = True
                'A5.Visible = True
                A1.Visible = True
                SurveyFeedback.Visible = True
            Else
                Dim chkBuhrRole = ChkRole()
                If chkBuhrRole = True Then
                    lblname.Text = "BUHR Admin"
                    'la.Visible = False
                    la1.Visible = False
                    'Added by TCS on 170223 (Enable Feedback report for BUHR)
                    'la2.Visible = False
                    la2.Visible = True
                    'End
                    'la3.Visible = False
                    'A4.Visible = False
                    'A5.Visible = False
                    A1.Visible = False
                    'A2.Visible = False
                    'A4.Visible = False
                    'A5.Visible = False
                    'A6.Visible = False
                    'A7.Visible = False
                    'A8.Visible = False
                    SurveyFeedback.Visible = False
                Else
                    Session("errorMsg") = "You don't have admin role."
                    Response.Redirect("errorpage.aspx", True)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function ChkRole() As Boolean
        ''''' function to check role for super admin and BUHR: WI:WI300'''''
        '''
        getsrlno()
        getFy()
        Try
            Dim t As Boolean = False

            Dim strrole As String = String.Empty
            strrole = "select IGP_user_id from hrps.t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            strrole += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"

            strrole += " UNION select ema_bhr_pno IGP_user_id from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & Session("ADM_USER").ToString() & "' AND EMA_CYCLE='" & ViewState("SRLNO").ToString & "' AND EMA_YEAR='" & ViewState("FY").ToString & "' and rownum=1"
            Dim cmd = New OracleCommand(strrole, conHrps)
            Dim f = getRecordInDt(cmd, conHrps)

            If f.Rows.Count > 0 Then
                t = True
            Else
                t = False
            End If
            Return t
        Catch ex As Exception
            MsgBox(ex.Message, ToString)
        End Try
    End Function
    Public Function ChkRole1() As Boolean
        Try
            Dim t As Boolean = False

            Dim strrole As String = String.Empty
            strrole = "select IGP_user_id from hrps.t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            strrole += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            Dim cmd = New OracleCommand(strrole, conHrps)
            Dim f = getRecordInDt(cmd, conHrps)

            If f.Rows.Count > 0 Then
                t = True
            Else
                t = False
            End If
            Return t
        Catch ex As Exception

        End Try
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
    Protected Sub ddltype_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddltype.SelectedIndexChanged
        If ddltype.SelectedValue = "pno" Then
            TextBox1.Enabled = True
            TextBox1.Text = ""
        ElseIf ddltype.SelectedValue = "IL3" Or ddltype.SelectedValue = "IL4" Then
            TextBox1.Enabled = False
            TextBox1.Text = ""
        Else
            TextBox1.Enabled = True
            TextBox1.Text = ""
        End If
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs)
        Try

            If ddltype.SelectedValue = "pno" Then

                If TextBox1.Text.Trim = "" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter Pno")
                    Exit Sub
                End If

                Dim qry As String = ""
                qry = "select ema_perno , EMA_EQV_LEVEL from hrps.t_emp_master_feedback360 where ema_perno='" & TextBox1.Text.Trim & "' AND ema_cycle='" & txtIcycle.Text.Trim().ToString & "' AND ema_year='" & txtIyear.Text.Trim().ToString & "'"
                Dim qrycmd As New OracleCommand()
                qrycmd.CommandText = qry
                Dim gh = getRecordInDt(qrycmd, conHrps)

                If gh.Rows.Count > 0 Then

                    If gh.Rows(0)(1) = "I3" Or gh.Rows(0)(1) = "I2" Or gh.Rows(0)(1) = "I1" Or gh.Rows(0)(1) = "TG" Then
                        downloadreportL3(gh.Rows(0)(0), txtIyear.Text, txtIcycle.Text)
                    ElseIf gh.Rows(0)(1) = "I4" Or gh.Rows(0)(1) = "I5" Or gh.Rows(0)(1) = "I6" Then
                        downloadreport(gh.Rows(0)(0), txtIyear.Text, txtIcycle.Text)
                    End If
                    TextBox1.Text = ""
                Else
                    qry = "select ema_perno , EMA_EQV_LEVEL from tips.t_empl_all where ema_perno='" & TextBox1.Text.Trim & "'"
                    Dim qrycmd1 As New OracleCommand()
                    qrycmd1.CommandText = qry
                    Dim gh1 = getRecordInDt(qrycmd1, conHrps)
                    If gh1.Rows(0)(1) = "I3" Then
                        downloadreportL3(gh1.Rows(0)(0), txtIyear.Text, txtIcycle.Text)
                    ElseIf gh1.Rows(0)(1) = "I4" Or gh1.Rows(0)(1) = "I5" Or gh1.Rows(0)(1) = "I6" Then
                        downloadreport(gh1.Rows(0)(0), txtIyear.Text, txtIcycle.Text)
                    End If
                    TextBox1.Text = ""

                End If

            ElseIf ddltype.SelectedValue = "IL3" Then

                downloadreportL3("", txtIyear.Text, txtIcycle.Text)

            ElseIf ddltype.SelectedValue = "IL4" Then
                downloadreport("", txtIyear.Text, txtIcycle.Text)
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select report type")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub downloadreport(pno As String, yr As String, cyc As String)
        If pno = "" Then
            pno = ""
        Else
            pno = " and sur.ss_asses_pno='" & pno & "'"
        End If
        Try
            Dim qry As String = String.Empty
            If cyc <> "1" Then
                qry = "Select distinct d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,decode(d.ss_categ,'INTSH','Internal stakeholder','MANGR','Manager/Superior','PEER','Peer','SELF','Self',d.ss_categ)ss_categ,round(max(d.Accountibility),2) ""A"",round(max(d.Col),2) ""C"",round(max(d.Res),2)"
                qry += """R"",round((max(d.team)),2) ""T"","
                qry += " (Case  When round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and"
                qry += " (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) ""ac"""
                qry += " ,(case  when round((max(d.Col)),2)<='1.6'  then 'U' when (max(d.Col))>='1.6' and (max(d.Col))<='2.6'"
                qry += "  then 'A' when (max(d.Col))>'2.6'  then 'G' end) ""Col"""
                qry += "  ,(case  when round((max(d.Res)),2)<='1.6'  then 'U' when (max(d.Res))>='1.6' and (max(d.Res))<='2.6' then 'A' when (max(d.Res))>'2.6'  then 'G' end) ""Res"""
                qry += " ,(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Team"","
                qry += "(case  when round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.col)),2)<='1.6'  then 'U' when (max(d.col))>='1.6' and (max(d.col))<='2.6' then 'A' when (max(d.col))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.res)),2)<='1.6'  then 'U' when (max(d.res))>='1.6' and (max(d.res))<='2.6' then 'A' when (max(d.res))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Combination"" from"
                qry += "(select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " avg(res.ss_qoptn) Accountibility,null Res,null Col,null team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I41','I45','I49','I51','I55','I59','I61','I65','I69') and res.SS_FLAG='I' and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " null Accountibility,avg(res.ss_qoptn) Res ,null Col,null team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I43','I47','I411','I53','I57','I511','I63','I67','I611')  and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select  distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,avg(res.ss_qoptn) Col, null team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in  ('I42','I46','I410','I52','I56','I510','I62','I66','I610') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct  emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,null Col, avg(res.ss_qoptn) team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I44','I48','I412','I54','I58','I512','I64','I68','I612') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ)d "
                qry += " group by d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,d.ss_categ order by 2 "
            Else
                qry = "Select distinct d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,decode(d.ss_categ,'INTSH','Internal stakeholder','MANGR','Manager/Superior','PEER','Peer','SELF','Self',d.ss_categ)ss_categ,round(max(d.Accountibility),2) ""A"",round(max(d.Col),2) ""C"",round(max(d.Res),2)"
                qry += """R"",round((max(d.team)),2) ""T"","
                qry += " (Case  When round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and"
                qry += " (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) ""ac"""
                qry += " ,(case  when round((max(d.Col)),2)<='1.6'  then 'U' when (max(d.Col))>='1.6' and (max(d.Col))<='2.6'"
                qry += "  then 'A' when (max(d.Col))>'2.6'  then 'G' end) ""Col"""
                qry += "  ,(case  when round((max(d.Res)),2)<='1.6'  then 'U' when (max(d.Res))>='1.6' and (max(d.Res))<='2.6' then 'A' when (max(d.Res))>'2.6'  then 'G' end) ""Res"""
                qry += " ,(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Team"","
                qry += "(case  when round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.col)),2)<='1.6'  then 'U' when (max(d.col))>='1.6' and (max(d.col))<='2.6' then 'A' when (max(d.col))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.res)),2)<='1.6'  then 'U' when (max(d.res))>='1.6' and (max(d.res))<='2.6' then 'A' when (max(d.res))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Combination"" from"
                qry += "(select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " avg(res.ss_qoptn) Accountibility,null Res,null Col,null team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I41','I45','I49','I51','I55','I59','I61','I65','I69') and res.SS_FLAG='I' and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " null Accountibility,avg(res.ss_qoptn) Res ,null Col,null team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I43','I47','I411','I53','I57','I511','I63','I67','I611')  and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select  distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,avg(res.ss_qoptn) Col, null team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in  ('I42','I46','I410','I52','I56','I510','I62','I66','I610') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct  emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,null Col, avg(res.ss_qoptn) team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I44','I48','I412','I54','I58','I512','I64','I68','I612') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ)d "
                qry += " group by d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,d.ss_categ order by 2 "
            End If
            'Added by TCS on 310122 (To separate code for year and cycle greater than 2022, 2
            Dim yearCyle As String = yr + cyc
            If yearCyle > 20221 Then
                qry = qry.Replace("('I41','I45','I49','I51','I55','I59','I61','I65','I69')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='Accountability' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL4','IL5','IL6')) and nvl(res.SS_DRAFT,'S') = 'S' ")
                qry = qry.Replace("('I43','I47','I411','I53','I57','I511','I63','I67','I611')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='Responsiveness' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL4','IL5','IL6')) and nvl(res.SS_DRAFT,'S') = 'S' ")
                qry = qry.Replace("('I42','I46','I410','I52','I56','I510','I62','I66','I610')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='Collaboration' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL4','IL5','IL6')) and nvl(res.SS_DRAFT,'S') = 'S' ")
                qry = qry.Replace("('I44','I48','I412','I54','I58','I512','I64','I68','I612')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='People Development' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL4','IL5','IL6')) and nvl(res.SS_DRAFT,'S') = 'S' ")
            End If
            'End


            Dim cmd As New OracleCommand()

            cmd.CommandText = qry
            Dim dt = getRecordInDt(cmd, conHrps)
            If dt.Rows.Count > 0 Then


                GridView3.DataSource = dt
                GridView3.DataBind()

                ExportToExcel(GridView3, "Individual")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data found")
                Exit Sub
            End If
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub downloadreportL3(pno As String, yr As String, cyc As String)
        If pno = "" Then
            pno = ""
        Else
            pno = " and sur.ss_asses_pno='" & pno & "'"
        End If
        Try

            Dim qry As String = String.Empty
            If cyc <> "1" Then
                qry = "select distinct d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,decode(d.ss_categ,'INTSH','Internal stakeholder','MANGR','Manager/Superior','PEER','Peer','SELF','Self',d.ss_categ)ss_categ,round(max(d.Accountibility),2) ""A"",round(max(d.Col),2) ""C"",round(max(d.Res),2)"
                qry += """R"",round((max(d.team)),2) ""T"","
                qry += " (Case  When round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and"
                qry += " (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) ""ac"""
                qry += " ,(case  when round((max(d.Col)),2)<='1.6'  then 'U' when (max(d.Col))>='1.6' and (max(d.Col))<='2.6'"
                qry += "  then 'A' when (max(d.Col))>'2.6'  then 'G' end) ""Col"""
                qry += "  ,(case  when round((max(d.Res)),2)<='1.6'  then 'U' when (max(d.Res))>='1.6' and (max(d.Res))<='2.6' then 'A' when (max(d.Res))>'2.6'  then 'G' end) ""Res"""
                qry += " ,(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Team"","
                qry += "(case  when round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.col)),2)<='1.6'  then 'U' when (max(d.col))>='1.6' and (max(d.col))<='2.6' then 'A' when (max(d.col))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.res)),2)<='1.6'  then 'U' when (max(d.res))>='1.6' and (max(d.res))<='2.6' then 'A' when (max(d.res))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Combination"" from"
                qry += "(select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " avg(res.ss_qoptn) Accountibility,null Res,null Col,null team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I31','I35','I39','I313') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + txtIyear.Text + "' and sur.ss_srlno='" + txtIcycle.Text + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " null Accountibility,avg(res.ss_qoptn) Res ,null Col,null team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial  and res.SS_QCODE in ('I33','I37','I311','I315') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + txtIyear.Text + "' and sur.ss_srlno='" + txtIcycle.Text + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select  distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,avg(res.ss_qoptn) Col, null team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I32','I36','I310','I314') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + txtIyear.Text + "' and sur.ss_srlno='" + txtIcycle.Text + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct  emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,null Col, avg(res.ss_qoptn) team"
                qry += " from  hrps.t_emp_master_feedback360 emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno and emp.ema_year=sur.ss_year and emp.ema_cycle = sur.ss_srlno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I34','I38','I312','I316') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + txtIyear.Text + "' and sur.ss_srlno='" + txtIcycle.Text + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ)d "
                qry += " group by d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,d.ss_categ order by 2 "
            Else
                qry = "select distinct d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,decode(d.ss_categ,'INTSH','Internal stakeholder','MANGR','Manager/Superior','PEER','Peer','SELF','Self',d.ss_categ)ss_categ,round(max(d.Accountibility),2) ""A"",round(max(d.Col),2) ""C"",round(max(d.Res),2)"
                qry += """R"",round((max(d.team)),2) ""T"","
                qry += " (Case  When round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and"
                qry += " (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) ""ac"""
                qry += " ,(case  when round((max(d.Col)),2)<='1.6'  then 'U' when (max(d.Col))>='1.6' and (max(d.Col))<='2.6'"
                qry += "  then 'A' when (max(d.Col))>'2.6'  then 'G' end) ""Col"""
                qry += "  ,(case  when round((max(d.Res)),2)<='1.6'  then 'U' when (max(d.Res))>='1.6' and (max(d.Res))<='2.6' then 'A' when (max(d.Res))>'2.6'  then 'G' end) ""Res"""
                qry += " ,(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Team"","
                qry += "(case  when round((max(d.Accountibility)),2)<='1.6'  then 'U' when (max(d.Accountibility))>='1.6' and (max(d.Accountibility))<='2.6' then 'A' when (max(d.Accountibility))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.col)),2)<='1.6'  then 'U' when (max(d.col))>='1.6' and (max(d.col))<='2.6' then 'A' when (max(d.col))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.res)),2)<='1.6'  then 'U' when (max(d.res))>='1.6' and (max(d.res))<='2.6' then 'A' when (max(d.res))>'2.6'  then 'G' end) "
                qry += " ||(case  when round((max(d.team)),2)<='1.6'  then 'U' when (max(d.team))>='1.6' and (max(d.team))<='2.6' then 'A' when (max(d.team))>'2.6'  then 'G' end) ""Combination"" from"
                qry += "(select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " avg(res.ss_qoptn) Accountibility,null Res,null Col,null team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I31','I35','I39','I313') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += " null Accountibility,avg(res.ss_qoptn) Res ,null Col,null team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial  and res.SS_QCODE in ('I33','I37','I311','I315') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select  distinct emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,avg(res.ss_qoptn) Col, null team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I32','I36','I310','I314') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ union "
                qry += " select distinct  emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ,"
                qry += "  null Accountibility,null Res,null Col, avg(res.ss_qoptn) team"
                qry += " from  TIPS.t_empl_all emp, t_survey_status sur,t_survey_response res where emp.ema_perno= sur.ss_asses_pno " & pno & " and res.ss_asses_pno = sur.ss_asses_pno"
                qry += " and sur.ss_pno = res.ss_pno and sur.ss_year=res.ss_year and sur.ss_srlno=res.ss_serial and res.SS_QCODE in ('I34','I38','I312','I316') and res.SS_FLAG='I'  and  SS_RPT_FLAG ='Y' and sur.ss_year='" + yr + "' and sur.ss_srlno='" + cyc + "' group by emp.EMA_PERNO,emp.EMA_ENAME,emp.EMA_EMPL_SGRADE,emp.ema_desgn_desc,emp.ema_exec_head_desc,emp.ema_bhr_pno,"
                qry += " emp.EMA_BHR_NAME,sur.ss_pno,sur.ss_name,sur.ss_categ)d "
                qry += " group by d.EMA_PERNO,d.EMA_ENAME,d.EMA_EMPL_SGRADE,d.ema_desgn_desc,d.ema_exec_head_desc,d.ema_bhr_pno,d.EMA_BHR_NAME,"
                qry += "d.ss_pno,d.ss_name,d.ss_categ order by 2 "
            End If
            'Added by TCS on 310122 (To separate code for year and cycle greater than 2022, 2
            Dim yearCyle As String = yr + cyc
            If yearCyle > 20221 Then
                qry = qry.Replace("('I31','I35','I39','I313')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='Accountability' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL1','IL2','IL3')) and nvl(res.SS_DRAFT,'S') = 'S' ")
                qry = qry.Replace("('I33','I37','I311','I315')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='Responsiveness' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL1','IL2','IL3')) and nvl(res.SS_DRAFT,'S') = 'S' ")
                qry = qry.Replace("('I32','I36','I310','I314')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='Collaboration' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL1','IL2','IL3')) and nvl(res.SS_DRAFT,'S') = 'S' ")
                qry = qry.Replace("('I34','I38','I312','I316')", "(select distinct sq.ss_qcode from HRPS.t_survey_question  sq where sq.ss_qtype ='People Development' and sq.ss_year='" + yr + "' and sq.ss_cycle='" + cyc + "' and sq.SS_QLEVEL in ('IL1','IL2','IL3'))  and res.ss_qoptn <> '0' and nvl(res.SS_DRAFT,'S') = 'S' ")
            End If
            'End


            Dim cmd As New OracleCommand()

            cmd.CommandText = qry
            Dim dt = getRecordInDt(cmd, conHrps)
            If dt.Rows.Count > 0 Then


                GridView3.DataSource = dt
                GridView3.DataBind()
                GridView3.HeaderRow.Cells(13).Text = "People Dev(Score)"
                GridView3.HeaderRow.Cells(17).Text = "People Dev(Category G/A/U)"
                ExportToExcel(GridView3, "Individual")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data found")
                Exit Sub
            End If
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub DropDownList2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDownList2.SelectedIndexChanged
        If DropDownList2.SelectedValue = "pno" Then
            TextBox3.Enabled = True
            TextBox3.Text = ""
        ElseIf DropDownList2.SelectedValue = "IL3" Or DropDownList2.SelectedValue = "IL4" Or DropDownList2.SelectedValue = "IL5" Or DropDownList2.SelectedValue = "IL6" Or DropDownList2.SelectedValue = "IL2" Or DropDownList2.SelectedValue = "IL1" Or DropDownList2.SelectedValue = "TG" Then
            TextBox3.Enabled = False
            TextBox3.Text = ""
        Else
            TextBox3.Enabled = True
            TextBox3.Text = ""
        End If
    End Sub
    Protected Sub Button7_Click(sender As Object, e As EventArgs)
        Try
            If DropDownList2.SelectedValue = "pno" Then

                If TextBox3.Text.Trim = "" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter Pno")
                End If

                Dim qry As String = ""
                qry = "select ema_perno , EMA_EQV_LEVEL from hrps.t_emp_master_feedback360 where ema_perno='" & TextBox3.Text.Trim & "' AND EMA_CYCLE='" & txtOCycle.Text.Trim().ToString & "' AND EMA_YEAR='" & txtOyear.Text.Trim().ToString & "'"
                Dim qrycmd As New OracleCommand()
                qrycmd.CommandText = qry
                Dim gh = getRecordInDt(qrycmd, conHrps)

                If gh.Rows.Count > 0 Then
                    overallIL1_TO_IL6(gh.Rows(0)(0), txtOyear.Text, txtOCycle.Text)
                    TextBox3.Text = ""
                Else
                    qry = "select ema_perno , EMA_EQV_LEVEL from tips.t_empl_all where ema_perno='" & TextBox3.Text.Trim & "'"
                    Dim qrycmd1 As New OracleCommand()
                    qrycmd1.CommandText = qry
                    Dim gh1 = getRecordInDt(qrycmd1, conHrps)
                    If gh1.Rows(0)(1) = "I3" Then
                        ovrallL3(gh1.Rows(0)(0), txtOyear.Text, txtOCycle.Text)
                    ElseIf gh1.Rows(0)(1) = "I4" Or gh1.Rows(0)(1) = "I5" Or gh1.Rows(0)(1) = "I6" Then
                        ovrallL3(gh1.Rows(0)(0), txtOyear.Text, txtOCycle.Text)
                    End If
                    TextBox3.Text = ""

                End If
            ElseIf DropDownList2.SelectedValue = "IL2" Or DropDownList2.SelectedValue = "IL1" Then
                If txtOCycle.Text <> "1" Then
                    overallIL1_TO_IL6("", txtOyear.Text, txtOCycle.Text)
                End If

            ElseIf DropDownList2.SelectedValue = "IL3" Or DropDownList2.SelectedValue = "TG" Then
                If txtOCycle.Text <> "1" Then
                    overallIL1_TO_IL6("", txtOyear.Text, txtOCycle.Text)
                Else
                    ovrallL3("", txtOyear.Text, txtOCycle.Text)
                End If

            ElseIf DropDownList2.SelectedValue = "IL4" Or DropDownList2.SelectedValue = "IL5" Or DropDownList2.SelectedValue = "IL6" Then
                If txtOCycle.Text <> "1" Then
                    overallIL1_TO_IL6("", txtOyear.Text, txtOCycle.Text)
                Else
                    ovrallL3("", txtOyear.Text, txtOCycle.Text)
                End If
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select report type")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Button5_Click(sender As Object, e As EventArgs)
        Try
            If TextBox2.Text <> "" Then
                ''WI7528 : PDF report for BUHR to view their departmental employee data    Added by Manoj Kumar  14-Feb-2022
                If ChkRole1() = False And ChkRole() = True Then
                    Dim qry1 As String = ""
                    qry1 = "select ema_perno , EMA_EQV_LEVEL from hrps.t_emp_master_feedback360 where ema_perno='" & TextBox2.Text.Trim & "' AND EMA_CYCLE='" & txtFcycle.Text.Trim().ToString & "' AND EMA_YEAR='" & txtFyear.Text.Trim().ToString & "' and ema_bhr_pno='" & Session("ADM_USER").ToUpper() & "'"
                    Dim qrycmd2 As New OracleCommand()
                    qrycmd2.CommandText = qry1
                    Dim gh1 = getRecordInDt(qrycmd2, conHrps)
                    If gh1.Rows.Count <= 0 Then
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "You are not authorise to see download report of this perno.")
                        Exit Sub
                    End If
                End If
                ''End by Manoj Kumar
                Dim qrycmd1 As New OracleCommand()
                qrycmd1.CommandText = "select * from t_survey_status where ss_asses_pno='" & TextBox2.Text & "' and ss_rpt_flag='Y' and ss_wfl_status='3' AND SS_SRLNO='" & txtFcycle.Text.Trim().ToString & "' AND SS_YEAR='" & txtFyear.Text.Trim().ToString & "'"
                Dim f1 = getRecordInDt(qrycmd1, conHrps)

                Dim qry As String = ""
                qry = "select ema_perno , EMA_EQV_LEVEL from hrps.t_emp_master_feedback360 where ema_perno='" & TextBox2.Text.Trim & "' AND EMA_CYCLE='" & txtFcycle.Text.Trim().ToString & "' AND EMA_YEAR='" & txtFyear.Text.Trim().ToString & "'"
                Dim qrycmd As New OracleCommand()
                qrycmd.CommandText = qry
                Dim gh = getRecordInDt(qrycmd, conHrps)
                If gh.Rows.Count > 0 Then
                    Dim yearCycle = txtFyear.Text.Trim().ToString + txtFcycle.Text.Trim().ToString
                    Dim qryMiniCriteria As New OracleCommand()
                    qryMiniCriteria.CommandText = "select irc_code,irc_desc from t_ir_codes where irc_type='" & txtFyear.Text.Substring(2, 2).ToString() & "" & txtFcycle.Text.Trim().ToString & "" & gh.Rows(0)(1) & "' and irc_valid_tag='A' order by irc_code"
                    Dim dtqryMiniCriteria = getRecordInDt(qryMiniCriteria, conHrps)
                    If dtqryMiniCriteria.Rows.Count > 0 Then

                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Cycle " & txtFcycle.Text & " is not active for this report.")
                        Exit Sub
                    End If

                    If gh.Rows(0)(1) = "I3" Or gh.Rows(0)(1) = "I2" Or gh.Rows(0)(1) = "I1" Then
                        If yearCycle > 20222 Then
                            If Not IsFunctionalManagerRespond(TextBox2.Text.Trim, txtFyear.Text.Trim().ToString, txtFcycle.Text.Trim().ToString) Then
                                ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the functional manager has not provided feedback")
                                Exit Sub
                            End If
                        End If
                        If checkcateg(TextBox2.Text, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                'Added by TCS on 08022024 (New Report Download)
                                If yearCycle >= 20262 Then
                                    Dim path As String = Server.MapPath("~/360Report.pdf")

                                    Dim generator As New ReportGenerator()

                                    generator.GenerateFullReport(path, "Abhishek Tudu", "Senior Engineer", "25")

                                    Response.ContentType = "application/pdf"
                                    Response.AppendHeader("Content-Disposition", "attachment; filename=360Report.pdf")
                                    Response.TransmitFile(path)
                                    Response.End()
                                ElseIf yearCycle > 20222 Then
                                    Dim rpt As New ReportIL1toIL3
                                    rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                Else
                                    Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                End If
                            Else
                                updateTag(TextBox2.Text)
                                'Added by TCS on 08022024 (New Report Download)
                                If yearCycle > 20222 Then
                                    Dim rpt As New ReportIL1toIL3
                                    rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                Else
                                    Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                End If
                            End If

                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                        'Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text))
                    ElseIf gh.Rows(0)(1) = "TG" Then
                        If checkcateg(TextBox2.Text, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                If yearCycle > 20222 Then
                                    Dim rpt As New ReportTG
                                    rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                Else
                                    Response.Redirect("FeedbackSurveyRptTG_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                End If
                            Else
                                updateTag(TextBox2.Text)
                                If yearCycle > 20222 Then
                                    Dim rpt As New ReportTG
                                    rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                Else
                                    Response.Redirect("FeedbackSurveyRptTG_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                End If
                            End If

                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                    ElseIf gh.Rows(0)(1) = "I4" Or gh.Rows(0)(1) = "I5" Or gh.Rows(0)(1) = "I6" Then
                        If checkcateg(TextBox2.Text, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                'Added by TCS on 08022024 (New Report Download)
                                If yearCycle > 20222 Then
                                    Dim rpt As New ReportOPR
                                    rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                Else
                                    Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                End If
                            Else
                                updateTag(TextBox2.Text)
                                'Added by TCS on 08022024 (New Report Download)
                                If yearCycle > 20222 Then
                                    Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                Else
                                    Dim rpt As New ReportOPR
                                    rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                End If
                            End If


                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                    End If
                Else
                    qry = "select ema_perno , EMA_EQV_LEVEL from tips.t_empl_all where ema_perno='" & TextBox2.Text.Trim & "'"

                    Dim qrycmd2 As New OracleCommand()
                    qrycmd2.CommandText = qry
                    Dim gh1 = getRecordInDt(qrycmd2, conHrps)

                    Dim qryMiniCriteria1 As New OracleCommand()
                    qryMiniCriteria1.CommandText = "select irc_code,irc_desc from t_ir_codes where irc_type='" & txtFyear.Text.Substring(2, 2).ToString() & "" & txtFcycle.Text.Trim().ToString & "" & gh1.Rows(0)(1) & "' and irc_valid_tag='A' order by irc_code"
                    Dim dtqryMiniCriteria1 = getRecordInDt(qryMiniCriteria1, conHrps)
                    If dtqryMiniCriteria1.Rows.Count > 0 Then

                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Cycle " & txtFcycle.Text & " is not active for this report.")
                        Exit Sub
                    End If

                    If Convert.ToInt32(txtFyear.Text) < 2023 Then
                        If gh1.Rows(0)(1) = "I3" Then
                            If checkcateg(TextBox2.Text, gh1.Rows(0)(1)) Then
                                If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                    Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                Else
                                    updateTag(TextBox2.Text)
                                    Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                End If

                            Else
                                ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                            End If
                            'Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text))
                        ElseIf gh1.Rows(0)(1) = "I4" Or gh1.Rows(0)(1) = "I5" Or gh1.Rows(0)(1) = "I6" Then
                            If checkcateg(TextBox2.Text, gh1.Rows(0)(1)) Then
                                If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                    'Commented and added by TCS on 06092022 (New Report Download for I4,I5,I6)
                                    Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                    'Dim rpt As New ReportOPR
                                    'rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                Else
                                    updateTag(TextBox2.Text)
                                    'Commented and added by TCS on 06092022 (New Report Download for I4,I5,I6)
                                    Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                                    'Dim rpt As New ReportOPR
                                    'rpt.GenerateReport(TextBox2.Text.Trim, txtFyear.Text.Trim, txtFcycle.Text.Trim)
                                End If


                            Else
                                ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                            End If
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
    Public Sub ExportToExcel(gv As GridView, name As String)
        ''''' Download report into excel WI:WI300'''''
        Dim dt As DataTable = New DataTable("GridView_Data")
        For Each cell As TableCell In gv.HeaderRow.Cells
            dt.Columns.Add(cell.Text)
        Next

        For Each row As GridViewRow In gv.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 1
                If row.Cells(i).Controls.Count > 0 Then
                    dt.Rows(dt.Rows.Count - 1)(i) = (TryCast(row.Cells(i).Controls(1), Label)).Text
                Else
                    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text
                End If
            Next
        Next

        Using wb As XLWorkbook = New XLWorkbook()
            wb.Worksheets.Add(dt)
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & name & ".xlsx")
            Using MyMemoryStream As MemoryStream = New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
        'Response.Clear()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", "attachment;filename=" & name & ".xls")
        'Response.Charset = ""
        'Response.ContentType = "application/vnd.ms-excel"
        'Using sw As New StringWriter()
        '    Dim hw As New HtmlTextWriter(sw)

        '    gv.AllowPaging = False
        '    'Me.bindGrid(txtperno1.Text.Trim)
        '    'Me.bindGrid()

        '    gv.HeaderRow.BackColor = Color.White
        '    For Each cell As TableCell In gv.HeaderRow.Cells
        '        cell.BackColor = gv.HeaderStyle.BackColor
        '    Next
        '    For Each row As GridViewRow In gv.Rows
        '        row.BackColor = Color.White
        '        For Each cell As TableCell In row.Cells
        '            If row.RowIndex Mod 2 = 0 Then
        '                cell.BackColor = gv.AlternatingRowStyle.BackColor
        '            Else
        '                cell.BackColor = gv.RowStyle.BackColor
        '            End If
        '            cell.CssClass = "textmode"
        '        Next
        '    Next

        '    gv.RenderControl(hw)
        '    'style to format numbers to string
        '    Dim style As String = "<style> .textmode { } </style>"
        '    Response.Write(style)
        '    Response.Output.Write(sw.ToString())
        '    Response.Flush()
        '    Response.[End]()
        'End Using
    End Sub
    'Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    '    ' Verifies that the control is rendered
    'End Sub
    Public Sub overallIL1_TO_IL6(pno As String, yr As String, cyc As String)
        Try
            If pno = "" Then
                Dim qry As String = ""
                Dim yrCyc As String = yr.Substring(2, 2).ToString() & "" & cyc.Trim().ToString

                qry += "Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='MANGR' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where decode(ic.irc_type,'" & yrCyc & "I1','I1','" & yrCyc & "I2','I2','" & yrCyc & "I3','I3','" & yrCyc & "I4','I4','" & yrCyc & "I5','I5','" & yrCyc & "I6','I6','" & yrCyc & "TG','TG')=se.ema_eqv_level and ic.irc_valid_tag='A' and ic.irc_code='MANGR')"

                If DropDownList2.SelectedValue = "IL1" Then
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='PEER' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I1' and ic.irc_valid_tag='A' and ic.irc_code='PEER' and se.ema_eqv_level='I1')"
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='ROPT' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I1' and ic.irc_valid_tag='A' and ic.irc_code='ROPT' and se.ema_eqv_level='I1')"
                End If

                If DropDownList2.SelectedValue = "IL2" Then
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='PEER' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I2' and ic.irc_valid_tag='A' and ic.irc_code='PEER' and se.ema_eqv_level='I2')"
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='ROPT' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I2' and ic.irc_valid_tag='A' and ic.irc_code='ROPT' and se.ema_eqv_level='I2')"
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='INTSH' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I2' and ic.irc_valid_tag='A' and ic.irc_code='INTSH' and se.ema_eqv_level='I2')"
                End If

                If DropDownList2.SelectedValue = "IL3" Then
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='PEER' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I3' and ic.irc_valid_tag='A' and ic.irc_code='PEER' and se.ema_eqv_level='I3')"

                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='INTSH' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I3' and ic.irc_valid_tag='A' and ic.irc_code='INTSH' and se.ema_eqv_level='I3')"
                End If

                If DropDownList2.SelectedValue = "TG" Then
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='ROPT' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "TG' and ic.irc_valid_tag='A' and ic.irc_code='ROPT' and se.ema_eqv_level='TG')"

                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='INTSH' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "TG' and ic.irc_valid_tag='A' and ic.irc_code='INTSH' and se.ema_eqv_level='TG')"
                End If

                If DropDownList2.SelectedValue = "IL4" Then
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='INTSH' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I4' and ic.irc_valid_tag='A' and ic.irc_code='INTSH' and se.ema_eqv_level ='I4')"
                End If

                If DropDownList2.SelectedValue = "IL5" Then
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='INTSH' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I5' and ic.irc_valid_tag='A' and ic.irc_code='INTSH' and se.ema_eqv_level ='I5')"
                End If

                If DropDownList2.SelectedValue = "IL6" Then
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='INTSH' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "I6' and ic.irc_valid_tag='A' and ic.irc_code='INTSH' and se.ema_eqv_level ='I6')"
                End If

                pernoList1000 = ""

                Dim cmd1 As New OracleCommand()
                cmd1.CommandText = qry
                Dim dtIl3 = getRecordInDt(cmd1, conHrps)
                If dtIl3.Rows.Count > 0 Then
                    If dtIl3.Rows.Count < 1000 Then
                        pernoList1000 = ""
                        For i As Integer = 0 To dtIl3.Rows.Count - 1
                            If dtIl3.Rows.Count = 1 Then
                                pernoList1000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "'"
                            Else
                                pernoList1000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                            End If
                        Next

                        If dtIl3.Rows.Count > 1 Then
                            pernoList1000 = pernoList1000.TrimEnd(",")
                        End If
                    Else
                        pernoList1000 = ""
                        pernoList2000 = ""
                        pernoList3000 = ""
                        pernoList4000 = ""
                        pernoList5000 = ""

                        '1000 row
                        For i As Integer = 0 To 999
                            pernoList1000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                        Next

                        If dtIl3.Rows.Count > 1 Then
                            pernoList1000 = pernoList1000.TrimEnd(",")
                        End If

                        '2000rows
                        If dtIl3.Rows.Count > 1000 Then
                            If dtIl3.Rows.Count < 2000 Then
                                For i As Integer = 1000 To dtIl3.Rows.Count - 1
                                    pernoList2000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 1000 To 1999
                                    pernoList2000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If


                            If dtIl3.Rows.Count > 1 Then
                                pernoList2000 = pernoList2000.TrimEnd(",")
                            End If
                        End If




                        '3000 rows
                        If dtIl3.Rows.Count > 2000 Then
                            If dtIl3.Rows.Count < 3000 Then
                                For i As Integer = 2000 To dtIl3.Rows.Count - 1
                                    pernoList3000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 2000 To 2999
                                    pernoList3000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList3000 = pernoList3000.TrimEnd(",")
                            End If
                        End If

                        '4000 rows
                        If dtIl3.Rows.Count > 3000 Then
                            If dtIl3.Rows.Count < 4000 Then
                                For i As Integer = 3000 To dtIl3.Rows.Count - 1
                                    pernoList4000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 3000 To 3999
                                    pernoList4000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList4000 = pernoList4000.TrimEnd(",")
                            End If
                        End If

                        '5000 rows
                        If dtIl3.Rows.Count > 4000 Then
                            If dtIl3.Rows.Count < 5000 Then
                                For i As Integer = 4000 To dtIl3.Rows.Count - 1
                                    pernoList5000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 4000 To 4999
                                    pernoList5000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList5000 = pernoList5000.TrimEnd(",")
                            End If
                        End If



                    End If

                End If
                pno = ""
            Else
                'If ddlTag.SelectedValue = "N" Then
                pernoList1000 = "'" + pno + "'"
                'End If
                'pno = " where ema_perno='" & pno & "'"
            End If

            Dim Str As String = String.Empty

            If ddlTag.SelectedValue = "Y" Then
                'Str += " select * from hrps.V_OVERALL_I1_I6_T where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " And") & " union select DISTINCT"
                'Str += "  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                'Str += " (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)COMBINATION"
                'Str += "     FROM HRPS.V_OVERALL_I1_I6_T where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " And") & "  GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"
                'Dim cmd As New OracleCommand()
                'cmd.CommandText = Str
                'Dim ds = getRecordInDt(cmd, conHrps)

                'If ds.Rows.Count > 0 Then
                '    GridView4.DataSource = ds
                '    GridView4.DataBind()

                '    GridView4.HeaderRow.Cells(11).Text = "People Dev(Score)"
                '    GridView4.HeaderRow.Cells(15).Text = "People Dev(Category G/A/U)"
                '    ExportToExcel(GridView4, "OverAll")
                'End If
                Dim eqvLevel = ""
                If pno <> "" Then
                    Dim qry As String = ""
                    qry = "select ema_perno , EMA_EQV_LEVEL from hrps.t_emp_master_feedback360 where ema_perno='" & pno & "' AND EMA_CYCLE='" & cyc & "' AND EMA_YEAR='" & yr & "'"
                    Dim qrycmd As New OracleCommand()
                    qrycmd.CommandText = qry
                    Dim gh = getRecordInDt(qrycmd, conHrps)
                    If gh.Rows.Count > 0 Then
                        eqvLevel = Convert.ToString(gh.Rows(0)("EMA_EQV_LEVEL"))
                    End If
                End If

                Dim yearCyle As String = yr + cyc
                    Dim ds As DataTable
                    ds = Nothing
                    If pernoList1000 <> "" Then
                        If yearCyle > 20221 Then
                        If yearCyle > 20231 And ((DropDownList2.SelectedValue = "IL4" Or DropDownList2.SelectedValue = "IL5" Or DropDownList2.SelectedValue = "IL6") Or (eqvLevel = "I4" Or eqvLevel = "I5" Or eqvLevel = "I6")) Then
                            Str += " Select * from hrps.V_ALL_I1_I6_T_2023I46  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                        Else
                            Str += " Select * from hrps.V_ALL_I1_I6_T_NEW  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                            End If
                        Else
                            Str += " Select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                        End If
                        'Str += " union select DISTINCT  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                        'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                        'Str += " (CASE WHEN round(AVG(A),2)<=1.6 THEN 'U' WHEN round(AVG(A),2)>1.6 AND round(AVG(A),2)<=2.6 THEN 'A' WHEN round(AVG(A),2)>2.6 THEN 'G' END) AC,"
                        'Str += "     (CASE WHEN round(AVG(C),2)<=1.6 THEN 'U' WHEN round(AVG(C),2)>1.6 AND round(AVG(C),2)<=2.6 THEN 'A' WHEN round(AVG(C),2)>2.6 THEN 'G' END) COL,"
                        'Str += "     (CASE WHEN round(AVG(R),2)<=1.6 THEN 'U' WHEN round(AVG(R),2)>1.6 AND round(AVG(R),2)<=2.6 THEN 'A' WHEN round(AVG(R),2)>2.6 THEN 'G' END) RES,"
                        'Str += "     (CASE WHEN round(AVG(T),2)<=1.6 THEN 'U' WHEN round(AVG(T),2)>1.6 AND round(AVG(T),2)<=2.6 THEN 'A' WHEN round(AVG(T),2)>2.6 THEN 'G' END) TEAM, "
                        'Str += "     (CASE WHEN round(AVG(A),2)<=1.6 THEN 'U' WHEN round(AVG(A),2)>1.6 AND round(AVG(A),2)<=2.6 THEN 'A' WHEN round(AVG(A),2)>2.6 THEN 'G' END) ||"
                        'Str += "     (CASE WHEN round(AVG(C),2)<=1.6 THEN 'U' WHEN round(AVG(C),2)>1.6 AND round(AVG(C),2)<=2.6 THEN 'A' WHEN round(AVG(C),2)>2.6 THEN 'G' END) ||"
                        'Str += "     (CASE WHEN round(AVG(R),2)<=1.6 THEN 'U' WHEN round(AVG(R),2)>1.6 AND round(AVG(R),2)<=2.6 THEN 'A' WHEN round(AVG(R),2)>2.6 THEN 'G' END) ||"
                        'Str += "     (CASE WHEN round(AVG(T),2)<=1.6 THEN 'U' WHEN round(AVG(T),2)>1.6 AND round(AVG(T),2)<=2.6 THEN 'A' WHEN round(AVG(T),2)>2.6 THEN 'G' END)COMBINATION"
                        'Str += "     FROM HRPS.V_OVERALL_I1_I6_T where ss_categ <>'Self' and EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"
                        Dim cmd As New OracleCommand()
                        cmd.CommandText = Str
                        Dim dt1000 = getRecordInDt(cmd, conHrps)
                        ds = dt1000
                    End If

                    If pernoList2000 <> "" Then
                        Str = ""
                        If yearCyle > 20221 Then
                        If yearCyle > 20231 And ((DropDownList2.SelectedValue = "IL4" Or DropDownList2.SelectedValue = "IL5" Or DropDownList2.SelectedValue = "IL6") Or (eqvLevel = "I4" Or eqvLevel = "I5" Or eqvLevel = "I6")) Then
                            Str += " select * from hrps.V_ALL_I1_I6_T_2023I46  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T_NEW  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                            End If
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                        End If

                        Dim cmd As New OracleCommand()
                        cmd.CommandText = Str
                        Dim dt2000 = getRecordInDt(cmd, conHrps)

                        ds.Merge(dt2000)
                    End If

                    If pernoList3000 <> "" Then
                        Str = ""
                        If yearCyle > 20221 Then
                        If yearCyle > 20231 And ((DropDownList2.SelectedValue = "IL4" Or DropDownList2.SelectedValue = "IL5" Or DropDownList2.SelectedValue = "IL6") Or (eqvLevel = "I4" Or eqvLevel = "I5" Or eqvLevel = "I6")) Then
                            Str += " select * from hrps.V_ALL_I1_I6_T_2023I46  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T_NEW  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                            End If
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                        End If

                        Dim cmd As New OracleCommand()
                        cmd.CommandText = Str
                        Dim dt3000 = getRecordInDt(cmd, conHrps)

                        ds.Merge(dt3000)
                    End If

                    If pernoList4000 <> "" Then
                        Str = ""
                        If yearCyle > 20221 Then
                        If yearCyle > 20231 And ((DropDownList2.SelectedValue = "IL4" Or DropDownList2.SelectedValue = "IL5" Or DropDownList2.SelectedValue = "IL6") Or (eqvLevel = "I4" Or eqvLevel = "I5" Or eqvLevel = "I6")) Then
                            Str += " select * from hrps.V_ALL_I1_I6_T_2023I46  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T_NEW  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                            End If
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                        End If

                        Dim cmd As New OracleCommand()
                        cmd.CommandText = Str
                        Dim dt4000 = getRecordInDt(cmd, conHrps)

                        ds.Merge(dt4000)
                    End If

                    If pernoList5000 <> "" Then
                        Str = ""
                        If yearCyle > 20221 Then
                        If yearCyle > 20231 And ((DropDownList2.SelectedValue = "IL4" Or DropDownList2.SelectedValue = "IL5" Or DropDownList2.SelectedValue = "IL6") Or (eqvLevel = "I4" Or eqvLevel = "I5" Or eqvLevel = "I6")) Then
                            Str += " select * from hrps.V_ALL_I1_I6_T_2023I46  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T_NEW  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                            End If
                        Else
                            Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                        End If


                        Dim cmd As New OracleCommand()
                        cmd.CommandText = Str
                        Dim dt5000 = getRecordInDt(cmd, conHrps)

                        ds.Merge(dt5000)
                    End If

                    If ds.Rows.Count > 0 Then
                        GridView4.DataSource = ds
                        GridView4.DataBind()

                        GridView4.HeaderRow.Cells(11).Text = "People Dev(Score)"
                        GridView4.HeaderRow.Cells(15).Text = "People Dev(Category G/A/U)"
                        If yearCyle > 20221 Then
                            GridView4.Columns.RemoveAt(8)
                            GridView4.DataBind()
                            GridView4.Columns.RemoveAt(8)
                            GridView4.DataBind()
                            GridView4.Columns.RemoveAt(8)
                            GridView4.DataBind()
                            GridView4.Columns.RemoveAt(8)
                            GridView4.DataBind()
                        End If
                        ExportToExcel(GridView4, "OverAll")
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Generate pdf report for this perno to get overall report.")
                        Exit Sub
                    End If
                Else
                    Dim ds As DataTable
                ds = Nothing
                If pernoList1000 <> "" Then
                    Dim yearCyle As String = yr + cyc
                    If yearCyle > 20221 Then
                        Str += " Select * from hrps.V_ALL_I1_I6_WT_NEW  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                    Else
                        Str += " Select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                    End If

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt1000 = getRecordInDt(cmd, conHrps)
                    ds = dt1000
                End If

                If pernoList2000 <> "" Then
                    Str = ""
                    Dim yearCyle As String = yr + cyc
                    If yearCyle > 20221 Then
                        Str += " select * from hrps.V_ALL_I1_I6_WT_NEW  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                    Else
                        Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                    End If
                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt2000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt2000)
                End If

                If pernoList3000 <> "" Then
                    Str = ""
                    Dim yearCyle As String = yr + cyc
                    If yearCyle > 20221 Then
                        Str += " select * from hrps.V_ALL_I1_I6_WT_NEW  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                    Else
                        Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                    End If


                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt3000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt3000)
                End If

                If pernoList4000 <> "" Then
                    Str = ""
                    Dim yearCyle As String = yr + cyc
                    If yearCyle > 20221 Then
                        Str += " select * from hrps.V_ALL_I1_I6_WT_NEW  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                    Else
                        Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                    End If


                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt4000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt4000)
                End If

                If pernoList5000 <> "" Then
                    Str = ""
                    Dim yearCyle As String = yr + cyc
                    If yearCyle > 20221 Then
                        Str += " select * from hrps.V_ALL_I1_I6_WT_NEW  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                    Else
                        Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"
                    End If

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt5000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt5000)
                End If


                If ds.Rows.Count > 0 Then
                    GridView4.DataSource = ds
                    GridView4.DataBind()
                    GridView4.HeaderRow.Cells(11).Text = "People Dev(Score)"
                    GridView4.HeaderRow.Cells(15).Text = "People Dev(Category G/A/U)"
                    Dim yearCyle As String = yr + cyc
                    If yearCyle > 20221 Then
                        GridView4.Columns.RemoveAt(8)
                        GridView4.DataBind()
                        GridView4.Columns.RemoveAt(8)
                        GridView4.DataBind()
                        GridView4.Columns.RemoveAt(8)
                        GridView4.DataBind()
                        GridView4.Columns.RemoveAt(8)
                        GridView4.DataBind()
                    End If
                    ExportToExcel(GridView4, "OverAll")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "No record found.")
                    Exit Sub
                End If

            End If




        Catch ex As Exception
            MsgBox(ex.Message.ToString())
        End Try
    End Sub
    Public Sub ovrallL3(pno As String, yr As String, cyc As String)
        Try
            If pno = "" Then
                'Commented by Manoj (IGQPK5672E) on 31_Dec-2021
                'Dim qry As String = "Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & ViewState("FY").ToString() & "' AND SS_SRLNO='" & ViewState("SRLNO").ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='MANGR' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL ='I3'  Group by ss_asses_pno,ss_categ HAVING count(*)>=1 INTERSECT Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & ViewState("FY").ToString() & "' AND SS_SRLNO='" & ViewState("SRLNO").ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='INTSH' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL ='I3'  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 INTERSECT Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & ViewState("FY").ToString() & "'AND SS_SRLNO='" & ViewState("SRLNO").ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='PEER' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL ='I3'  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 order by ss_asses_pno"

                'Added by Manoj (IGQPK5672E) on 31_Dec-2021
                Dim qry As String = ""
                If cyc <> "1" Then
                    qry = "Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno = se.ema_cycle and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='MANGR' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL  in ('I1','I2','I3')  Group by ss_asses_pno,ss_categ HAVING count(*)>=1 INTERSECT Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno = se.ema_cycle and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='INTSH' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I1','I2','I3')  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 INTERSECT Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno = se.ema_cycle and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "'AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='PEER' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I1','I2','I3')  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 order by ss_asses_pno"
                Else
                    qry = "Select ss_asses_pno from t_survey_status ss ,tips.t_empl_all se where  ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='MANGR' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL  in ('I1','I2','I3')  Group by ss_asses_pno,ss_categ HAVING count(*)>=1 INTERSECT Select ss_asses_pno from t_survey_status ss ,tips.t_empl_all se where ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='INTSH' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I1','I2','I3')  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 INTERSECT Select ss_asses_pno from t_survey_status ss ,tips.t_empl_all se where ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "'AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='PEER' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I1','I2','I3')  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 order by ss_asses_pno"
                End If


                Dim cmd1 As New OracleCommand()
                cmd1.CommandText = qry
                Dim dtIl3 = getRecordInDt(cmd1, conHrps)
                If dtIl3.Rows.Count > 0 Then
                    If dtIl3.Rows.Count < 1000 Then
                        pernoList1000 = ""
                        For i As Integer = 0 To dtIl3.Rows.Count - 1
                            If dtIl3.Rows.Count = 1 Then
                                pernoList1000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "'"
                            Else
                                pernoList1000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                            End If
                        Next

                        If dtIl3.Rows.Count > 1 Then
                            pernoList1000 = pernoList1000.TrimEnd(",")
                        End If
                    Else
                        pernoList1000 = ""
                        pernoList2000 = ""
                        pernoList3000 = ""
                        pernoList4000 = ""
                        pernoList5000 = ""

                        '1000 row
                        For i As Integer = 0 To 999
                            pernoList1000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                        Next

                        If dtIl3.Rows.Count > 1 Then
                            pernoList1000 = pernoList1000.TrimEnd(",")
                        End If

                        '2000rows
                        If dtIl3.Rows.Count > 1000 Then
                            If dtIl3.Rows.Count < 2000 Then
                                For i As Integer = 1000 To dtIl3.Rows.Count - 1
                                    pernoList2000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 1000 To 1999
                                    pernoList2000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If


                            If dtIl3.Rows.Count > 1 Then
                                pernoList2000 = pernoList2000.TrimEnd(",")
                            End If
                        End If




                        '3000 rows
                        If dtIl3.Rows.Count > 2000 Then
                            If dtIl3.Rows.Count < 3000 Then
                                For i As Integer = 2000 To dtIl3.Rows.Count - 1
                                    pernoList3000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 2000 To 2999
                                    pernoList3000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList3000 = pernoList3000.TrimEnd(",")
                            End If
                        End If

                        '4000 rows
                        If dtIl3.Rows.Count > 3000 Then
                            If dtIl3.Rows.Count < 4000 Then
                                For i As Integer = 3000 To dtIl3.Rows.Count - 1
                                    pernoList4000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 3000 To 3999
                                    pernoList4000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList4000 = pernoList4000.TrimEnd(",")
                            End If
                        End If

                        '5000 rows
                        If dtIl3.Rows.Count > 4000 Then
                            If dtIl3.Rows.Count < 5000 Then
                                For i As Integer = 4000 To dtIl3.Rows.Count - 1
                                    pernoList5000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 4000 To 4999
                                    pernoList5000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList5000 = pernoList5000.TrimEnd(",")
                            End If
                        End If



                    End If

                End If
                pno = ""
            Else
                pno = " where ema_perno='" & pno & "'"
            End If

            Dim Str As String = String.Empty

            If ddlTag.SelectedValue = "Y" Then
                Str += " select * from hrps.V_ALL_I3_I6_T_CYC1 where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " And") & ""
                'Str += " select * from hrps.V_OVERALL_IL3 where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " And") & " union select DISTINCT"
                'Str += "  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                'Str += " (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)COMBINATION"
                'Str += "     FROM HRPS.V_OVERALL_IL3 where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " And") & "  GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"
                Dim cmd As New OracleCommand()
                cmd.CommandText = Str
                Dim ds = getRecordInDt(cmd, conHrps)

                If ds.Rows.Count > 0 Then
                    GridView4.DataSource = ds
                    GridView4.DataBind()

                    GridView4.HeaderRow.Cells(11).Text = "People Dev(Score)"
                    GridView4.HeaderRow.Cells(15).Text = "People Dev(Category G/A/U)"
                    ExportToExcel(GridView4, "OverAll")
                End If
            Else
                Dim ds As DataTable
                If pernoList1000 <> "" Then
                    Str += " Select * from hrps.V_ALL_I3_I6_WT_CYC1  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & ""
                    'Str += " Select * from hrps.V_OVERALL_IL3_WOTAG  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " union select DISTINCT"
                    'Str += "  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                    'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    'Str += " (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)COMBINATION"
                    'Str += "     FROM HRPS.V_OVERALL_IL3_WOTAG where ss_categ <>'Self' and EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"
                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt1000 = getRecordInDt(cmd, conHrps)
                    ds = dt1000
                End If

                If pernoList2000 <> "" Then
                    Str = ""
                    Str += " Select * from hrps.V_ALL_I3_I6_WT_CYC1  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & ""
                    'Str += " select * from hrps.V_OVERALL_IL3_WOTAG  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " union select DISTINCT"
                    'Str += "  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                    'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    'Str += " (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)COMBINATION"
                    'Str += "     FROM HRPS.V_OVERALL_IL3_WOTAG where ss_categ <>'Self' and EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "   GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt2000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt2000)
                End If

                If pernoList3000 <> "" Then
                    Str = ""
                    Str += " Select * from hrps.V_ALL_I3_I6_WT_CYC1  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & ""
                    'Str += " select * from hrps.V_OVERALL_IL3_WOTAG  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " union select DISTINCT"
                    'Str += "  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                    'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    'Str += " (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)COMBINATION"
                    'Str += "     FROM HRPS.V_OVERALL_IL3_WOTAG where ss_categ <>'Self' and EMA_PERNO in (" + pernoList3000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt3000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt3000)
                End If

                If pernoList4000 <> "" Then
                    Str = ""
                    Str += " Select * from hrps.V_ALL_I3_I6_WT_CYC1  where EMA_PERNO in (" + pernoList4000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & ""
                    'Str += " select * from hrps.V_OVERALL_IL3_WOTAG  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  union select DISTINCT"
                    'Str += "  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                    'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    'Str += " (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)COMBINATION"
                    'Str += "     FROM HRPS.V_OVERALL_IL3_WOTAG where ss_categ <>'Self' and EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "   GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt4000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt4000)
                End If

                If pernoList5000 <> "" Then
                    Str = ""
                    Str += " Select * from hrps.V_ALL_I3_I6_WT_CYC1  where EMA_PERNO in (" + pernoList5000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & ""
                    'Str += " select * from hrps.V_OVERALL_IL3_WOTAG  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  union select DISTINCT"
                    'Str += "  EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) "
                    'Str += " A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    'Str += " (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)COMBINATION"
                    'Str += "     FROM HRPS.V_OVERALL_IL3_WOTAG where ss_categ <>'Self' and EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "   GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt5000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt5000)
                End If


                If ds.Rows.Count > 0 Then
                    GridView4.DataSource = ds
                    GridView4.DataBind()
                    GridView4.HeaderRow.Cells(11).Text = "People Dev(Score)"
                    GridView4.HeaderRow.Cells(15).Text = "People Dev(Category G/A/U)"
                    ExportToExcel(GridView4, "OverAll")
                End If

            End If




        Catch ex As Exception

        End Try
    End Sub
    Public Function checkcateg(pno As String, lvl As String) As Boolean
        Dim falg As Boolean = False
        Try
            Dim cntMinFullFill As Integer = 2
            Dim qry As String = String.Empty
            qry = "Select count(*) No_Records,upper(ss_categ) categ from t_survey_status where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and ss_year='" & txtFyear.Text.Trim().ToString() & "' and ss_srlno='" & txtFcycle.Text.Trim().ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') And ss_app_tag = 'AP'  Group by ss_categ order by ss_categ"
            Dim cmd As New OracleCommand()
            cmd.CommandText = qry
            Dim g = getRecordInDt(cmd, conHrps)

            'Added by Manoj (IGQPK5672E) on 31-Dec-2021
            Dim qry1 As String = String.Empty
            qry1 = "select irc_code,irc_desc from t_ir_codes where irc_type='" & txtFyear.Text.Substring(2, 2).ToString() & "" & txtFcycle.Text.Trim().ToString & "" & lvl & "' and irc_valid_tag='A' order by irc_code"
            Dim cmd1 As New OracleCommand()
            cmd1.CommandText = qry1

            Dim g1 = getRecordInDt(cmd1, conHrps)


            If Not g Is Nothing Then
                If lvl = "I3" Then
                    If g.Rows.Count < g1.Rows.Count Then
                        cntMinFullFill = 0
                    Else
                        For i As Integer = 0 To g.Rows.Count - 1
                            'Added by Manoj (IGQPK5672E) on 31-Dec-2021
                            For j As Integer = 0 To g1.Rows.Count - 1
                                If g.Rows(i)("categ").ToString = g1.Rows(j)("irc_code").ToString Then
                                    If Val(g.Rows(i)("No_Records").ToString) < Val(g1.Rows(j)("irc_desc").ToString) Then
                                        cntMinFullFill = 0
                                    End If
                                End If
                            Next

                            'Commented by Manoj (IGQPK5672E) on 31-Dec-2021
                            'If g.Rows(i)("categ").ToString = "INTSH" And Val(g.Rows(i)("No_Records").ToString) < 3 Then
                            '    cntMinFullFill = 0
                            'ElseIf g.Rows(i)("categ").ToString = "MANGR" And Val(g.Rows(i)("No_Records").ToString) < 1 Then
                            '    cntMinFullFill = 0
                            'ElseIf g.Rows(i)("categ").ToString = "PEER" And Val(g.Rows(i)("No_Records").ToString) < 3 Then
                            '    cntMinFullFill = 0
                            'End If
                        Next

                    End If
                Else
                    If g.Rows.Count < g1.Rows.Count Then
                        cntMinFullFill = 0
                    Else
                        For i As Integer = 0 To g.Rows.Count - 1
                            'Added by Manoj (IGQPK5672E) on 31-Dec-2021
                            For j As Integer = 0 To g1.Rows.Count - 1
                                If g.Rows(i)("categ").ToString = g1.Rows(j)("irc_code").ToString Then
                                    If Val(g.Rows(i)("No_Records").ToString) < Val(g1.Rows(j)("irc_desc").ToString) Then
                                        cntMinFullFill = 0
                                    End If
                                End If
                            Next

                            'Commented by Manoj (IGQPK5672E) on 31-Dec-2021
                            'If g.Rows(i)("categ").ToString = "INTSH" And Val(g.Rows(i)("No_Records").ToString) < 3 Then
                            '    cntMinFullFill = 0
                            'ElseIf g.Rows(i)("categ").ToString = "MANGR" And Val(g.Rows(i)("No_Records").ToString) < 1 Then
                            '    cntMinFullFill = 0
                            'End If
                        Next
                    End If
                End If
                'If g.Rows.Count < 3 Then
                '    cntMinFullFill = 0
                'Else


                'End If

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
            Dim sql As String = "update hrps.t_survey_status set ss_rpt_flag='Y'  where ss_asses_pno='" + pno.ToString() + "' and ss_wfl_status ='3' and ss_year='" & ViewState("FY").ToString() & "' and ss_srlno='" & ViewState("SRLNO").ToString() & "' and ss_status = 'SE' and ss_del_tag = 'N' And ss_app_tag = 'AP'"
            Dim cmd1 = New OracleCommand(sql, conHrps)
            cmd1.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    'Added by TCS on 13122023, Added Role based validation
    Public Function CheckAuthentication(pno As String, pageName As String) As Boolean
        Dim d As New Boolean
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim q As New OracleCommand()
            q.Connection = conHrps
            q.CommandText = "SELECT IGP_USER_ID,ema_ename,IPG_PROG_NAME,IGP_GROUP_ID FROM t_ir_adm_grp_privilege, t_ir_grp_pgm_privilege, T_IR_ADM_PROGRAM,t_emp_master_feedback360 WHERE IGP_GROUP_ID = GPP_GROUP_ID AND GPP_PROG_ID = IPG_PROG_ID and IGP_USER_ID = ema_perno(+) and ema_year(+)='" + ViewState("FY") + "' and ema_cycle(+)='" + ViewState("SRLNO") + "' AND IGP_STATUS = 'A' AND IGP_USER_ID = '" & pno & "' AND IGP_GROUP_ID IN ('360DF_SA','360DF_LD')  "
            'q.CommandText += " UNION SELECT IGP_USER_ID,'' ema_ename,IPG_PROG_NAME FROM t_ir_adm_grp_privilege, t_ir_grp_pgm_privilege, T_IR_ADM_PROGRAM WHERE IGP_GROUP_ID = GPP_GROUP_ID AND GPP_PROG_ID = IPG_PROG_ID AND IGP_STATUS = 'A' AND IGP_USER_ID = '" & pno & "' AND IGP_GROUP_ID IN ('360DF_SA','360DF_LD') "
            q.CommandText += " UNION SELECT DISTINCT EMA_BHR_PNO, EMA_BHR_NAME ema_ename,IPG_PROG_NAME,'360DF_HRBP' IGP_GROUP_ID   FROM T_EMP_MASTER_FEEDBACK360, t_ir_grp_pgm_privilege, T_IR_ADM_PROGRAM WHERE EMA_YEAR='" + ViewState("FY") + "' AND EMA_CYCLE='" + ViewState("SRLNO") + "' AND GPP_PROG_ID = IPG_PROG_ID AND EMA_BHR_PNO = '" & pno & "' AND GPP_GROUP_ID = '360DF_HRBP' "
            Dim p = getDataInDt(q)
            If p.Rows.Count > 0 Then
                Session("UserName") = p.Rows(0)("ema_ename").ToString
                Dim foundRow() As DataRow
                foundRow = p.Select("IPG_PROG_NAME='" + pageName + "'")
                If foundRow.Length > 0 Then
                    d = True
                End If
            Else
                d = False
            End If
        Catch ex As Exception

        End Try
        Return d
    End Function
    Public Function CheckUserRole() As Boolean
        Dim t As Boolean = False
        Dim ds As New DataSet
        Try

            Dim strAdmin, strLD, strHRBP As String
            'If userCategory = "SA" Then
            strAdmin = " select IGP_user_id from hrps.t_ir_adm_grp_privilege where igp_group_id ='360DF_SA' "
            strAdmin += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            'ElseIf userCategory = "HRBP" Then
            strHRBP = " select ema_bhr_pno IGP_user_id from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & Session("ADM_USER").ToString() & "' AND EMA_CYCLE='" & ViewState("SRLNO").ToString & "' AND EMA_YEAR='" & ViewState("FY").ToString & "' and rownum=1"
            'ElseIf userCategory = "LD" Then
            strLD = " select IGP_user_id from hrps.t_ir_adm_grp_privilege where igp_group_id ='360DF_LD' "
            strLD += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            'End If

            Dim cmdAdmin = New OracleCommand(strAdmin)
            Dim dtAdmin = getRecordInDt(cmdAdmin, conHrps)
            If dtAdmin.Rows.Count > 0 Then
                t = True
                lblname.Text = "Super Admin"
                isSuperAdmin = True
                Session("LOGINID") = "SA"
                Return t
            End If

            Dim cmdLD = New OracleCommand(strLD)
            Dim dtLD = getRecordInDt(cmdLD, conHrps)
            If dtLD.Rows.Count > 0 Then
                t = True
                lblname.Text = "LD Super Admin"
                isLD = True
                Session("LOGINID") = "LD"
                Return t
            End If

            Dim cmdHRBP = New OracleCommand(strHRBP)
            Dim dtHRBP = getRecordInDt(cmdHRBP, conHrps)
            If dtHRBP.Rows.Count > 0 Then
                t = True
                lblname.Text = "HRBP Admin"
                isHRBP = True
                Session("LOGINID") = "HRBP"
                Return t
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return t
    End Function
    'End
    'Added by TCS on 08022024 (New Report Download Check)
    Public Function IsFunctionalManagerRespond(ByVal strPerno As String, yer As String, cyc As String) As Boolean
        Dim isRespond As Boolean = False
        Dim queryResponse, queryEqvLevel As String
        Try
            queryEqvLevel = "SELECT * FROM T_EMP_MASTER_FEEDBACK360 WHERE EMA_YEAR='" & yer.ToString & "' AND EMA_CYCLE='" & cyc.ToString & "' AND EMA_PERNO='" & strPerno.ToString & "' AND EMA_EQV_LEVEL IN ('I1','I2')"

            Dim cmdLevel As New OracleCommand(queryEqvLevel)
            Dim dtLevel = getRecordInDt(cmdLevel, conHrps)
            If dtLevel.Rows.Count > 0 Then
                queryResponse = "SELECT SS_PNO FROM T_SURVEY_STATUS WHERE SS_YEAR='" & yer.ToString & "' AND SS_SRLNO='" & cyc.ToString & "' AND SS_ASSES_PNO ='" & strPerno.ToString & "' AND SS_PNO =NVL(SS_APPROVER, (SELECT EMA_REPORTING_TO_PNO FROM T_EMP_MASTER_FEEDBACK360 WHERE EMA_YEAR='" & yer.ToString & "' AND EMA_CYCLE='" & cyc.ToString & "' AND EMA_PERNO='" & strPerno.ToString & "')) AND SS_WFL_STATUS='3'"
                Dim cmd As New OracleCommand(queryResponse)
                Dim dt = getRecordInDt(cmd, conHrps)
                If dt.Rows.Count > 0 Then
                    isRespond = True
                End If
            Else
                isRespond = True
            End If
        Catch ex As Exception

        End Try
        Return isRespond
    End Function
End Class

