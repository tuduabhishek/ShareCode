Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Net.Mail
Imports ClosedXML.Excel
''' <summary>
''' 'WI : WI300-> Admin page implementation for feedback 360 for IL3-IL6
''' WI: WI368 (22 LINE COMMENTED, 9 line code changed)     Added by Manoj Kumar on 30-05-2021
''' **********************
''' 1. Not show pending record for approval by Manager. (Bind variable error)
''' 2. Not show Min. And Max. criteria
''' 3. Department Not show When executive head select.
''' 4. Not show criteria comleted in Yes/No. By default show 'No' value. (Bind variable error)
''' 5. Not Show correct message in final submit Min. & Max. category
''' 6. Not return to assessor (bind variable error)
''' 7. Not delete reocrd when unchecked row.
''' 8. Add officer category
''' **********************
''' WI447: Add revert back to approver feature in admin page, also add return remarks over mail. Created Date: 04-06-2021, Created By:Avik Mukherjee
'''WI484: rectification in code to allow buhr to approve pending for approval records
'''created by: Avik Mukherjee
'''Date: 09-06-2021
'''WI624: Add new features to remove respondent even if respondent list has been approved. Created By: Avik Mukherjee, Created On: 16-06-2021
'''WI7528 : PDF report for BUHR to view their departmental employee data    Added by Manoj Kumar  14-Feb-2022
''' </summary>
Partial Class SurveyAdm_OPR
    Inherits System.Web.UI.Page



    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim pernoList1000, pernoList2000, pernoList3000, pernoList4000, pernoList5000, pernoList6000, pernoList7000, pernoList8000, pernoList9000, pernoList10000 As String
    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        'strUserID = "197838"
        'strUserID = "198777"
        'CheckSupAdm(strUserID)
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
            'strUserID = "197838"
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
            If GetPno(Session("ADM_USER").ToUpper()) = False Then
                Session("errorMsg") = "You don't have admin role."
                Response.Redirect("errorpage.aspx", True)
            End If
        End If
        'visibleRlink(strUserID)
        'If GetPno(strUserID.ToUpper()) = False Then
        '    Response.Redirect("errorpage.aspx", True)
        'End If
    End Sub
    Private Sub SurveyApproval_Init(sender As Object, e As EventArgs) Handles Me.Init
        Session("errorMsg") = "The page you were looking for is not valid."
        Response.Redirect("errorpage.aspx", True)
        Exit Sub
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub

    Private Sub SurveyApproval_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            SessionTimeOut()
            'visibleRlink(Session("ADM_USER"))
            If Request.QueryString("adm") = "2" Then
                OverAll()
                'bindMinicriteria()
            ElseIf Request.QueryString("adm") = "1" Then
                resp.InnerText = "APPROVE RESPONDENT"
            Else
                resp.InnerText = "ADD/REMOVE/SUBMIT RESPONDENT"

            End If
            If Not IsPostBack Then
                'bindGrid()
                ' bindgrid1()
                lblSessionNm.Text = Session("UserName")
                hdfSession.Value = Session("ADM_USER").ToString
                hdfYear.Value = ViewState("FY").ToString
                hdfCycle.Value = ViewState("SRLNO").ToString
                ViewState("TagChangeBUHR") = ""
                ViewState("ChangeBUHR") = ""
                'getFy()
                'getsrlno()
                bindExecHead()
                PopDroupdown()
                bindTimelinePage()
                bindTimelineGdv()
                'getCycleTime()
                getCycleTimeGdv()
                visibleRlink(Session("ADM_USER"))
                ddlTimelinePage.SelectedIndex = 0
                txtStartDt.Text = ""
                txtEndDt.Text = ""
            End If
            CheckSupAdm(Session("ADM_USER").ToString())
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
    Protected Sub btnsearch_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        ''''' This search option is to search record based on filter: WI:WI300'''''

        If ddlExecutive.SelectedValue = "--Select--" And txtperno1.Text.Trim = "" And txtBuhr.Text.Trim = "" Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select option for search result")
            Exit Sub
        Else
            bindGrid()
        End If
        'bindGrid(txtperno1.Text.Trim)
    End Sub
    Public Sub bindGrid()

        Try
            If ChkRole() Then

                If Request.QueryString("adm") = "1" Then
                    bindpendinggrid()
                    GridView1.Visible = True
                    gvself.Visible = False
                    resp.InnerText = "APPROVE RESPONDENT"
                Else
                    resp.InnerText = "ADD/REMOVE/SUBMIT RESPONDENT"

                    ''''' BUHR can only able to view records of their departments/ Super admin can see overall record: WI:WI300'''''
                    Dim qryrespond As New OracleCommand
                    qryrespond.Connection = conHrps
                    qryrespond.Parameters.Clear()
                    If ChkRole1() Then

                        qryrespond.CommandText = "select tab.SS_ASSES_PNO,tab.ema_ename,tab.ema_desgn_desc,nvl((select D.EMA_ENAME||'('|| D.EMA_PERNO ||')' from hrps.t_emp_master_feedback360 c,hrps.t_emp_master_feedback360 d  "
                        qryrespond.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                        qryrespond.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6')  and a.ema_comp_code='1000' "
                        qryrespond.CommandText += " and ss_status='SE'  and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and A.EMA_BHR_PNO=:ema_perno" ' and SS_ASSES_PNO='" & txtperno1.Text.Trim & "'"

                        If ddlExecutive.SelectedValue <> "--Select--" Then
                            qryrespond.CommandText += " and A.ema_exec_head=:ema_exec_head"
                            qryrespond.Parameters.AddWithValue("ema_exec_head", ddlExecutive.SelectedValue)
                        End If
                        If ddlDept.SelectedValue <> "--Select--" Then
                            qryrespond.CommandText += " and A.ema_dept_code='" + ddlDept.SelectedValue.Trim + "'"
                            'qryrespond.Parameters.AddWithValue("ema_dept_code", ddlDept.SelectedValue)
                        End If
                        If txtperno1.Text.Trim <> "" Then
                            qryrespond.CommandText += " and ss_asses_pno=:ss_asses_pno"
                            qryrespond.Parameters.AddWithValue("ss_asses_pno", txtperno1.Text.Trim.ToString)
                        End If
                        If txtBuhr.Text.Trim <> "" Then
                            qryrespond.CommandText += " and A.ema_bhr_pno=:ema_bhr_pno"
                            qryrespond.Parameters.AddWithValue("ema_bhr_pno", txtBuhr.Text.Trim.ToString)
                        End If

                        qryrespond.CommandText += " union select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                        qryrespond.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO  and a.ema_comp_code='1000'  and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6') "
                        qryrespond.CommandText += " and ss_status='SE'  and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and a.ema_perno<>:ema_perno" ' and SS_ASSES_PNO='" & txtperno1.Text.Trim & "') tab"
                        If ddlExecutive.SelectedValue <> "--Select--" Then
                            qryrespond.CommandText += " and A.ema_exec_head=:ema_exec_head1"
                            qryrespond.Parameters.AddWithValue("ema_exec_head1", ddlExecutive.SelectedValue)
                        End If
                        If ddlDept.SelectedValue <> "--Select--" Then
                            qryrespond.CommandText += " and A.ema_dept_code='" + ddlDept.SelectedValue.Trim + "'"
                            'qryrespond.Parameters.AddWithValue("ema_dept_code1", ddlDept.SelectedValue)
                        End If
                        If txtperno1.Text.Trim <> "" Then
                            qryrespond.CommandText += " and ss_asses_pno=:ss_asses_pno1"
                            qryrespond.Parameters.AddWithValue("ss_asses_pno1", txtperno1.Text.Trim.ToString)
                        End If
                        If txtBuhr.Text.Trim <> "" Then
                            qryrespond.CommandText += " and A.ema_bhr_pno=:ema_bhr_pno1"
                            qryrespond.Parameters.AddWithValue("ema_bhr_pno1", txtBuhr.Text.Trim.ToString)
                        End If
                        qryrespond.CommandText += ") tab"
                        qryrespond.Parameters.AddWithValue("fy", ViewState("FY").ToString())
                        qryrespond.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                        qryrespond.Parameters.AddWithValue("ema_perno", Session("ADM_USER").ToString())

                    Else
                        qryrespond.CommandText = "select tab.SS_ASSES_PNO,tab.ema_ename,tab.ema_desgn_desc,nvl((select D.EMA_ENAME||'('|| D.EMA_PERNO ||')' from hrps.t_emp_master_feedback360 c,hrps.t_emp_master_feedback360 d  "
                        qryrespond.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                        qryrespond.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6')  "
                        qryrespond.CommandText += " and ss_status='SE'  and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and A.EMA_BHR_PNO=:ema_perno" 'and SS_ASSES_PNO='" & txtperno1.Text.Trim & "') tab"
                        If ddlExecutive.SelectedValue <> "--Select--" Then
                            qryrespond.CommandText += " and A.ema_exec_head=:ema_dept_code"
                            qryrespond.Parameters.AddWithValue("ema_dept_code", ddlExecutive.SelectedValue)
                        End If
                        If ddlDept.SelectedValue <> "--Select--" Then
                            qryrespond.CommandText += " and A.ema_dept_code='" + ddlDept.SelectedValue + "'"
                            'qryrespond.Parameters.AddWithValue("ema_dept_code", ddlDept.SelectedValue)
                        End If
                        If txtperno1.Text.Trim <> "" Then
                            qryrespond.CommandText += " and ss_asses_pno=:ss_asses_pno"
                            qryrespond.Parameters.AddWithValue("ss_asses_pno", txtperno1.Text.Trim.ToString)
                        End If
                        If txtBuhr.Text.Trim <> "" Then
                            qryrespond.CommandText += " and A.ema_bhr_pno=:ema_bhr_pno1"
                            qryrespond.Parameters.AddWithValue("ema_bhr_pno1", txtBuhr.Text.Trim.ToString)
                        End If
                        qryrespond.CommandText += ") tab"
                        qryrespond.Parameters.AddWithValue("fy", ViewState("FY").ToString())
                        qryrespond.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                        qryrespond.Parameters.AddWithValue("ema_perno", Session("ADM_USER").ToString())
                    End If

                    'Dim qry = New OracleCommand(qryrespond, conHrps)
                    Dim dt = getDataInDt(qryrespond)
                    GridView1.Visible = False
                    gvself.Visible = True
                    If dt.Rows.Count > 0 Then

                        gvself.DataSource = dt
                        gvself.DataBind()
                        bindself()
                    Else
                        gvself.DataSource = Nothing
                        gvself.DataBind()
                        ShowGenericMessageModal(CommonConstants.AlertType.error, "Sorry No Record Found")
                    End If
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Sub bindpendinggrid()
        Try

            Dim dt As New DataTable()
            Dim qrypending As New OracleCommand
            ''''' This search option is to search record based on filter: WI:WI300'''''
            qrypending.Connection = conHrps
            qrypending.Parameters.Clear()

            If ChkRole1() Then
                ''WI484: rectification in code to allow BUHR to approve assesse respondent list
                ''created date: 09-06-2021
                '''created by: Avik Mukherjee
                qrypending.CommandText = "select tab.SS_ASSES_PNO,tab.ema_ename,tab.ema_desgn_desc,nvl((select D.EMA_ENAME||'('|| D.EMA_PERNO ||')' from hrps.t_emp_master_feedback360 c,hrps.t_emp_master_feedback360 d  "
                qrypending.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                qrypending.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6')  and a.ema_comp_code='1000' "
                ''WI484: End of change, By Avik Mukherjee, Date: 09-06-2021
                ' Start WI368  by Manoj Kumar on 30-05-2021
                qrypending.CommandText += " and ss_status='SE' and ss_tag='SU' and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and A.EMA_BHR_PNO=:ema_perno and ss_app_tag is null" 'WI368 Not show pending record for approval by Manager
                ' End by Manoj Kumar on 30-05-2021
                If ddlExecutive.SelectedValue <> "--Select--" Then
                    qrypending.CommandText += " and A.ema_exec_head=:ema_exec_head"
                    qrypending.Parameters.AddWithValue("ema_exec_head", ddlExecutive.SelectedValue)
                End If
                If ddlDept.SelectedValue <> "" And ddlDept.SelectedValue <> "--Select--" Then
                    qrypending.CommandText += " and A.ema_dept_code='" + ddlDept.SelectedValue.Trim + "'"
                    'qrypending.Parameters.AddWithValue("ema_dept_code", ddlDept.SelectedValue)
                End If
                If txtperno1.Text.Trim <> "" Then
                    qrypending.CommandText += " and ss_asses_pno='" + txtperno1.Text.Trim + "'"
                    'qrypending.Parameters.AddWithValue("ss_asses_pno", txtperno1.Text.Trim.ToString)
                End If
                If txtBuhr.Text.Trim <> "" Then
                    qrypending.CommandText += " and A.ema_bhr_pno='" + txtBuhr.Text.Trim + "'"
                    'qrypending.Parameters.AddWithValue("ema_bhr_pno", txtBuhr.Text.Trim.ToString)
                End If
                qrypending.CommandText += " union select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                qrypending.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO  and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6') "
                qrypending.CommandText += " and ss_status='SE' and ss_tag='SU' and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and a.ema_perno<>:ema_perno and ss_app_tag is null and a.ema_comp_code='1000' "
                If ddlExecutive.SelectedValue <> "--Select--" Then
                    qrypending.CommandText += " and A.ema_exec_head=:ema_exec_head1"
                    qrypending.Parameters.AddWithValue("ema_exec_head1", ddlExecutive.SelectedValue)
                End If
                If ddlDept.SelectedValue <> "" And ddlDept.SelectedValue.Trim <> "--Select--" Then
                    qrypending.CommandText += " and A.ema_dept_code=:ema_dept_code1"
                    qrypending.Parameters.AddWithValue("ema_dept_code1", ddlDept.SelectedValue)
                End If
                If txtperno1.Text.Trim <> "" Then
                    qrypending.CommandText += " and ss_asses_pno=:ss_asses_pno1"
                    qrypending.Parameters.AddWithValue("ss_asses_pno1", txtperno1.Text.Trim.ToString)
                End If
                If txtBuhr.Text.Trim <> "" Then
                    qrypending.CommandText += " and A.ema_bhr_pno=:ss_asses_pno1"
                    qrypending.Parameters.AddWithValue("ss_asses_pno1", txtBuhr.Text.Trim.ToString)
                End If
                qrypending.CommandText += " )tab"
                qrypending.Parameters.AddWithValue("fy", ViewState("FY").ToString())
                qrypending.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                qrypending.Parameters.AddWithValue("ema_perno", Session("ADM_USER").ToString())
            Else
                qrypending.CommandText = "select tab.SS_ASSES_PNO,tab.ema_ename,tab.ema_desgn_desc,nvl((select D.EMA_ENAME||'('|| D.EMA_PERNO ||')' from hrps.t_emp_master_feedback360 c,hrps.t_emp_master_feedback360 d  "
                qrypending.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                qrypending.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.EMA_EQV_LEVEL in('I3','I4','I5','I6')   and a.ema_comp_code='1000' "
                qrypending.CommandText += " and ss_status='SE' and ss_tag='SU' and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and A.EMA_BHR_PNO=:ema_perno and ss_app_tag is null  and a.ema_comp_code='1000' "
                If ddlExecutive.SelectedValue <> "--Select--" Then
                    qrypending.CommandText += " and A.ema_exec_head=:ema_exec_head1"
                    qrypending.Parameters.AddWithValue("ema_exec_head1", ddlExecutive.SelectedValue)
                End If
                If ddlDept.SelectedValue <> "" And ddlDept.SelectedValue.Trim <> "--Select--" Then
                    qrypending.CommandText += " and A.ema_dept_code='" + ddlDept.SelectedValue + "'"
                    'qrypending.Parameters.AddWithValue("ema_dept_code1", ddlDept.SelectedValue)
                End If
                If txtperno1.Text.Trim <> "" Then
                    qrypending.CommandText += " and ss_asses_pno='" + txtperno1.Text.Trim + "'"
                    ' qrypending.Parameters.AddWithValue("ss_asses_pno1", txtperno1.Text.Trim.ToString)
                End If
                If txtBuhr.Text.Trim <> "" Then
                    qrypending.CommandText += " and A.ema_bhr_pno=:ss_asses_pno1"
                    qrypending.Parameters.AddWithValue("ss_asses_pno1", txtBuhr.Text.Trim.ToString)
                End If
                qrypending.CommandText += " )tab"
                qrypending.Parameters.AddWithValue("fy", ViewState("FY").ToString())
                qrypending.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                qrypending.Parameters.AddWithValue("ema_perno", Session("ADM_USER").ToString())
            End If

            'Dim qry = New OracleCommand(qrypending, conHrps)
            dt = getDataInDt(qrypending)
            If dt.Rows.Count > 0 Then
                GridView1.DataSource = dt
                GridView1.DataBind()
            Else
                GridView1.DataSource = Nothing
                GridView1.DataBind()
            End If

        Catch ex As Exception
            ' MsgBox(ex.Message)
        End Try
    End Sub

    Public Function ChkRole() As Boolean
        ''''' function to check role for super admin and BUHR: WI:WI300'''''
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

    Public Sub BindAssesorGrid(pno As String)
        Try
            'If Session("assespno") Is Nothing Then
            '    Exit Sub
            'End If

            Session("label") = ChkAuthlabel(pno)
            Dim type As String = String.Empty
            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            End If
            'Dim qry As New OracleCommand()
            Dim str As New OracleCommand
            If Session("label").Equals("I2") Then
                str.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            ElseIf Session("label").Equals("I3") Then
                str.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            Else
                str.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Peer/Subordinate/Internal Stakeholder',SS_CATEG) Category,"
            End If
            str.CommandText += "SS_CATEG,SS_DEL_TAG,SS_TAG from hrps.t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and SS_STATUS='SE' and ss_year=:fy and SS_SRLNO=:SS_SRLNO order by Category"
            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            str.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            'Dim qry = New OracleCommand(str, conHrps)
            Dim dt = getDataInDt(str)
            ViewState("Data") = dt
            Dim vl As String
            vl = ""
            Dim DrNOrg() As DataRow
            DrNOrg = dt.Select("SS_TAG='SU'") ''

            For Each row1 As DataRow In DrNOrg
                vl = "S"
            Next

            If dt.Rows.Count > 0 Then
                If vl = "S" Then
                    gvfinal.DataSource = dt
                    gvfinal.DataBind()
                    gvfinal.Columns(7).Visible = False
                    btnaddnontsl.Visible = False
                    btnaddpeertsl.Visible = False
                    btnsubmit.Visible = False
                    gvfinal.Visible = True
                    '  lbOrg.Visible = True
                    divtitle.Visible = True
                Else
                    gvfinal.DataSource = dt
                    gvfinal.DataBind()
                    gvfinal.Columns(7).Visible = True
                    btnaddnontsl.Visible = True
                    btnaddpeertsl.Visible = True
                    btnsubmit.Visible = True
                    gvfinal.Visible = True
                    '  lbOrg.Visible = True
                    divtitle.Visible = True
                End If

            Else
                gvfinal.DataSource = Nothing
                gvfinal.DataBind()
                btnaddnontsl.Visible = False
                btnaddpeertsl.Visible = False
                gvfinal.Visible = False
                btnsubmit.Visible = False
                ' lbOrg.Visible = False
                divtitle.Visible = False
            End If



            Dim str1 As New OracleCommand
            If Session("label").Equals("I2") Then
                str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            ElseIf Session("label").Equals("I3") Then
                str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            Else
                str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Peer/Subordinate/Internal Stakeholder',SS_CATEG) Category,"
            End If
            str1.CommandText += "SS_CATEG,SS_DEL_TAG from hrps.t_survey_status where ss_asses_pno=:SS_ASSES_PNO"
            'qry1.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and ss_del_tag='N' and ss_tag='SU' and SS_STATUS='SE' order by Category"
            str1.CommandText += " and ss_year=:fy and ss_del_tag='N' and SS_STATUS='SE' and SS_SRLNO=:SS_SRLNO order by Category"
            str1.Connection = conHrps
            str1.Parameters.Clear()
            str1.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            str1.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            str1.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            'Dim qry1 = New OracleCommand(str1, conHrps)
            Dim dth = getDataInDt(str1)

            If dth.Rows.Count > 0 Then
                GridView2.DataSource = dth
                GridView2.DataBind()
            Else
                GridView2.DataSource = Nothing
                GridView2.DataBind()
            End If

            Dim cmdqry As New OracleCommand()
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc irc_desc1,decode(a.irc_type,'360V2',decode(a.IRC_CODE,'PEER','Peers','ROPT','Subordinates',b.irc_desc),'360V3',decode(a.IRC_CODE,'PEER','Peer/Subordinate',b.irc_desc),decode(a.IRC_CODE,'INTSH','Peer/Subordinate/Internal Stackholder',b.irc_desc)) irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            Dim dt1 = getDataInDt(cmdqry)
            Dim categ As String = String.Empty
            For d = 0 To dt1.Rows.Count - 1

                Dim qryq As New OracleCommand()
                qryq.CommandText = "select * from hrps.t_survey_status where ss_asses_pno=:SS_ASSES_PNO and ss_status='SE' and ss_del_tag='N' and upper(ss_categ) = "
                'qryq.CommandText += "upper( '" & dt1.Rows(d)("IRC_CODE") & "') and ss_year=:fy and SS_SRLNO=:SS_SRLNO"
                qryq.CommandText += ":IRC_CODE and ss_year=:fy and SS_SRLNO=:SS_SRLNO"
                qryq.Connection = conHrps
                qryq.Parameters.Clear()
                qryq.Parameters.AddWithValue("SS_ASSES_PNO", pno)
                qryq.Parameters.AddWithValue("fy", ViewState("FY").ToString())
                qryq.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                qryq.Parameters.AddWithValue("IRC_CODE", dt1.Rows(d)("IRC_CODE").ToString.ToUpper)
                Dim s = getDataInDt(qryq)
                If dt1.Rows(d)("maximum") = "N" Then
                    If s.Rows.Count < dt1.Rows(d)("minmum") Then
                        categ += dt1.Rows(d)("irc_desc").ToString() & ","
                    End If
                    'categ += dt1.Rows(d)("irc_desc").ToString() & ","
                Else
                    If s.Rows.Count > dt1.Rows(d)("maximum") Or s.Rows.Count < dt1.Rows(d)("minmum") Then
                        categ += dt1.Rows(d)("irc_desc").ToString() & ","
                    End If
                End If

            Next
            '''''checking for minimum and maximum thresold value for each criteria WI:WI300'''''
            Dim w = categ.Split(",")
            categ = categ.TrimEnd(",")
            If w.Length = 2 Then
                categ = categ.TrimEnd(",")
                lbls.Text = ""
                'lbls.Text = "Minimum criteria not completed for " & categ
            Else
                Dim ls = categ.Substring(categ.LastIndexOf(",") + 1)

                Dim c = categ.Remove(categ.LastIndexOf(",") + 1)

                If categ <> "" Then
                    lbls.Text = ""
                    'lbls.Text = "Minimum criteria not completed for " & c.TrimEnd(",") & " and  " & ls & " Category"
                Else
                    lbls.Text = ""
                End If
            End If





        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Public Sub CheckSupAdm(pno As String)
        Try
            'Dim qry As String = String.Empty
            Dim chkAdmin = ChkRole1()
            If chkAdmin = True Then
                lblname.Text = "Super Admin"
                la.Visible = False
                la1.Visible = True
                la2.Visible = True
                A4.Visible = True
                A5.Visible = True
                A1.Visible = True
                SurveyFeedback.Visible = True
            Else
                Dim chkBuhrRole = ChkRole()
                If chkBuhrRole = True Then
                    lblname.Text = "BUHR Admin"
                    la.Visible = False
                    la1.Visible = False
                    la2.Visible = True
                    A4.Visible = False
                    A5.Visible = False
                    A1.Visible = False
                    SurveyFeedback.Visible = False
                Else
                    Session("errorMsg") = "You don't have admin role."
                    Response.Redirect("errorpage.aspx", True)
                End If

            End If
            'qry = "select * from hrps.t_ir_codes WHERE irc_TYPE ='360LR' AND trim(IRC_DESC)='" & pno & "' AND irc_valid_tag='Y'"
            'Dim cmd As New OracleCommand()
            'cmd.CommandText = qry
            'Dim g = getRecordInDt(cmd, conHrps)
            'If g.Rows.Count > 0 Then
            '    lblname.Text = "Super Admin"
            '    la.Visible = True
            '    la1.Visible = True
            '    la2.Visible = True
            '    A4.Visible = True
            '    A5.Visible = True
            '    A1.Visible = True
            'Else
            '    lblname.Text = "BUHR Admin"
            '    la.Visible = False
            '    la1.Visible = False
            '    la2.Visible = False
            '    A4.Visible = False
            '    A5.Visible = False
            '    A1.Visible = False
            'End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub visibleRlink(ByVal pno As String)
        Dim d As New Boolean
        If conHrps.State = ConnectionState.Closed Then
            conHrps.Open()
        End If
        Dim q As New OracleCommand()
        q.Connection = conHrps
        q.CommandText = "Select distinct IGP_user_id from t_ir_adm_grp_privilege where IGP_user_id='" & pno & "' and upper(igp_group_id)='360FEEDBAC' and IGP_STATUS='A'"
        Dim p = getDataInDt(q)
        If p.Rows.Count > 0 Then
            limenuR.Visible = True
        Else
            limenuR.Visible = False


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
            q.CommandText = "Select ema_perno,ema_ename from t_ir_adm_grp_privilege,hrps.t_emp_master_feedback360 where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno & "' and EMA_COMP_CODE='1000'"
            q.CommandText += " union select ema_bhr_pno ema_perno,'' from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & pno & "' AND EMA_CYCLE='" & ViewState("SRLNO").ToString & "' AND EMA_YEAR='" & ViewState("FY").ToString & "' and rownum=1"
            'WI624: allow TCS employee to log into admin page as super admin, created by : Avik Mukherjee, created on: 16-06-2021
            q.CommandText += " union select IGP_user_id ema_perno,'' from  t_ir_adm_grp_privilege where IGP_user_id='" & pno & "' and igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A'"
            ' q.Parameters.Clear()
            'q.Parameters.AddWithValue("IGP_user_id", Session("ADM_USER").ToString())
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
    Public Sub NoEachcateg(ByVal pno As String)
        Try
            Dim qr As New OracleCommand()
            Dim lvl As String = ChkAuthlabel(pno)
            Dim equilabel As String = String.Empty
            If lvl = "I3" Then
                equilabel = "360V3"
            ElseIf lvl = "I4" Then
                equilabel = "360V4"
            ElseIf lvl = "I5" Then
                equilabel = "360V5"
            ElseIf lvl = "I6" Then
                equilabel = "360V6"
            ElseIf lvl = "I2" Then
                equilabel = "360V2"
            End If
            'qr.CommandText = "select a.IRC_CODE, SUBSTR(a.IRC_DESC,0,1) minmum, SUBSTR(a.IRC_DESC,3,2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            'qr.CommandText += "  where a.irc_type in('360V3','360V4','360V5','360V6') and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"

            '''''get maximum and minumum thresold value based on impact level: WI:WI300'''''
            qr.CommandText = "Select a.IRC_CODE, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc,decode(a.irc_type,'360V3','IL3','360V4','IL4','360V5','IL5','360V6','IL6','360V2','IL2') Label from t_ir_codes a,t_ir_codes b"
            qr.CommandText += " where a.irc_type in(:irc_type ) and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 5,1"
            qr.Connection = conHrps
            qr.Parameters.Clear()
            qr.Parameters.AddWithValue("irc_type", equilabel)
            Dim w = getDataInDt(qr)
            ' Start WI368  by Manoj Kumar on 30-05-2021
            ' WI368 commented line
            'If w.Rows.Count > 0 Then
            '    GridView2.DataSource = w
            '    GridView2.DataBind()
            'End If
            'End by Manoj Kumar on 30-05-2021
            Dim r As New OracleCommand()
            r.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("IRC_DESC").ToString()
            End If
        Catch ex As Exception


        End Try
    End Sub
    Protected Sub lbpendingapproval_Click(sender As Object, e As EventArgs)
        Try

            Dim gv = CType(sender, LinkButton).Parent.Parent
            Dim perno = CType(gv.FindControl("lblpno"), Label)
            Dim b = CType(gv.FindControl("lbpendingapproval"), LinkButton)
            Dim nm = CType(gv.FindControl("lnlname"), Label)
            Session("assespno") = Nothing
            Session("assespno") = perno.Text
            lblassname.Text = nm.Text
            'NoEachcateg(perno.Text)
            BindAssesorGrid(perno.Text)
            If b.Text = "Approved" Then
                GridView2.Visible = True
                gvfinal.Visible = False
                btnaddpeertsl.Visible = False
                btnaddnontsl.Visible = False
                btnsubmit.Visible = False
            ElseIf b.Text = "Returned" Then
                GridView2.Visible = False
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                btnsubmit.Visible = True
                gvfinal.Visible = True
            ElseIf b.Text = "Submitted" Then

            Else
                GridView2.Visible = False
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                btnsubmit.Visible = True
                gvfinal.Visible = True
            End If
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.Message)
        End Try

    End Sub
    Public Sub Approve(ByVal pno As String, ByVal tag As String)
        Try
            SessionTimeOut()
            Dim qry As String = String.Empty
            qry = "update hrps.t_survey_status set SS_APP_TAG=:SS_APP_TAG,SS_TAG_DT = sysdate, SS_ACTION_BY='ADM',SS_WFL_STATUS='2',SS_UPDATED_BY= "
            qry += ":SS_UPDATED_BY,SS_UPDATED_DT=sysdate where  ss_year=:ss_year  and SS_ASSES_PNO =:SS_ASSES_PNO "
            qry += "and SS_STATUS='SE' and SS_DEL_TAG='N' and SS_TAG ='SU' and SS_SRLNO=:SS_SRLNO"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim c As New OracleCommand()
            c.CommandText = qry
            c.Connection = conHrps
            c.Parameters.AddWithValue("SS_APP_TAG", tag)
            c.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            c.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            c.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            c.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            c.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Private Sub getCycleTime()
        Try
            Dim dtCycle As New DataTable
            Dim dtYear As New DataTable
            Dim query As New OracleCommand
            Dim query1 As New OracleCommand
            If ddlYearSetting.SelectedIndex <> 0 Then
                query1.Connection = conHrps
                query1.CommandText = "select irc_desc from hrps.t_ir_codes where IRC_TYPE='360YS' and IRC_VALID_TAG='A' and irc_desc='" + ddlYearSetting.SelectedItem.Text.ToString() + "'"
                dtYear = getDataInDt(query1)
                If dtYear.Rows.Count > 0 Then
                    query.Connection = conHrps

                    query.CommandText = "select irc_desc from hrps.t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
                    'Dim qry = New OracleCommand(query, conHrps)
                    dtCycle = getDataInDt(query)
                    If dtCycle.Rows.Count > 0 Then
                        txtCycle.Text = Val(dtCycle.Rows(0)("irc_desc").ToString()) + Val(1)
                    Else
                        txtCycle.Text = "1"
                    End If
                Else
                    txtCycle.Text = "1"
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Private Sub bindTimelinePage()
        Try
            Dim dtTimelinePage As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select DISTINCT decode(UPPER(IRC_DESC),'SELECTASSESOR_OPR.ASPX','STEP-1 (IL2-IL6)','SURVEYAPPROVAL_OPR.ASPX','STEP-2 (IL2-IL6)','FEEDBACK_OPR.ASPX','STEP-3 (IL2-IL6)','DOWNLOADREPORT.ASPX','STEP-4 (IL2-IL6)') CODE,IRC_DESC from t_ir_codes where irc_type ='360PG' and IRC_VALID_TAG='A' order by 1"
            'Dim qry = New OracleCommand(query, conHrps)
            dtTimelinePage = getDataInDt(query)
            If dtTimelinePage.Rows.Count > 0 Then
                ddlTimelinePage.DataSource = dtTimelinePage
                ddlTimelinePage.DataValueField = "irc_desc"
                ddlTimelinePage.DataTextField = "CODE"
                ddlTimelinePage.DataBind()
                ddlTimelinePage.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Private Sub bindTimelineGdv()
        Try
            Dim dtTimelineGdv As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select DISTINCT decode(UPPER(IRC_DESC),'SELECTASSESOR_OPR.ASPX','STEP-1 (IL2-IL6)','SURVEYAPPROVAL_OPR.ASPX','STEP-2 (IL2-IL6)','FEEDBACK_OPR.ASPX','STEP-3 (IL2-IL6)','DOWNLOADREPORT.ASPX','STEP-4 (IL2-IL6)') CODE,to_char(irc_start_dt,'dd-MM-yyyy') irc_start_dt,to_char(irc_end_dt,'dd-MM-yyyy') irc_end_dt from t_ir_codes where irc_type ='360PG' and IRC_VALID_TAG='A' order by 1"
            'Dim qry = New OracleCommand(query, conHrps)
            dtTimelineGdv = getDataInDt(query)
            If dtTimelineGdv.Rows.Count > 0 Then
                gdvTimeline.DataSource = dtTimelineGdv
                gdvTimeline.DataBind()
            End If
        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Private Sub getCycleTimeGdv()
        Try
            Dim dtCycleGdv As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select (select irc_desc from hrps.t_ir_codes where IRC_TYPE='360YS' and IRC_VALID_TAG='A') Year, (select irc_desc from hrps.t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y') CycleTime ,to_char(irc_start_dt,'dd-MM-yyyy') irc_start_dt,to_char(irc_end_dt,'dd-MM-yyyy') irc_end_dt from hrps.t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
            'Dim qry = New OracleCommand(query, conHrps)
            dtCycleGdv = getDataInDt(query)
            If dtCycleGdv.Rows.Count > 0 Then
                gdvCycle.DataSource = dtCycleGdv
                gdvCycle.DataBind()
            End If
        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Private Sub SurveyApproval_Unload(sender As Object, e As EventArgs) Handles Me.Unload
        If conHrps.State = ConnectionState.Open Then
            conHrps.Close()
        End If
    End Sub
    Private Sub bindExecHead()
        Try
            Dim dtExecHead As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct ema_exec_head,ema_exec_head_desc from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head_desc<>'Not found' and ema_exec_head<>'00000000' and ema_comp_code='1000' "
            If ChkRole1() = False Then
                query.Parameters.Clear()
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())

            End If
            query.CommandText += " order by ema_exec_head_desc"
            'Dim qry = New OracleCommand(query, conHrps)
            dtExecHead = getDataInDt(query)
            If dtExecHead.Rows.Count > 0 Then
                ddlExecutive.DataSource = dtExecHead
                ddlExecutive.DataValueField = "ema_exec_head"
                ddlExecutive.DataTextField = "ema_exec_head_desc"
                ddlExecutive.DataBind()
                ddlExecutive.Items.Insert(0, New ListItem("--Select--", "--Select--"))
                ddlDept.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ddlExecutive_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlExecutive.SelectedIndexChanged
        bindDepartment()
    End Sub
    Protected Sub ddlYearSetting_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlYearSetting.SelectedIndexChanged
        getCycleTime()
    End Sub
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
    Public Sub bindDepartment()
        Try
            Dim dtDept As New DataTable
            Dim query As New OracleCommand
            ' Start WI368  by Manoj Kumar on 30-05-2021
            query.Parameters.Clear()      'WI368 Department not show When executive head select.
            query.Connection = conHrps      'WI368 Department not show When executive head select.
            'End by Manoj Kumar on 30-05-2021
            ddlDept.Items.Clear()
            query.CommandText = "select distinct ema_dept_code DEPT,ema_dept_desc DEPTDESC from hrps.t_emp_master_feedback360 where ema_comp_code='1000'  and ema_dept_desc<>'Not found' and ema_exec_head = :ema_exec_head "
            If ChkRole1() = False Then
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())
            End If
            query.CommandText += " order by ema_dept_desc"
            query.Parameters.AddWithValue("ema_exec_head", ddlExecutive.SelectedValue)


            'Dim qry = New OracleCommand(query, conHrps)
            dtDept = getDataInDt(query)
            If dtDept.Rows.Count > 0 Then
                ddlDept.DataSource = dtDept
                ddlDept.DataValueField = "DEPT"
                ddlDept.DataTextField = "DEPTDESC"
                ddlDept.DataBind()
                ddlDept.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            Else
                ddlDept.DataSource = dtDept
                ddlDept.DataBind()
                ddlDept.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If


        Catch ex As Exception

        End Try
    End Sub
    'Private Sub bindgrid1(ByVal pno As String)
    '    If ChkRole() Then

    '        If Request.QueryString("adm") = "1" Then
    '            bindpendinggrid(pno)
    '            GridView1.Visible = True
    '            gvself.Visible = False
    '            resp.InnerText = "View Pending Approval"

    '        End If
    '    End If

    'End Sub


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

    Protected Sub GvCateg_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            ' Dim id = CType(e.Row.FindControl("hdfnid"), HiddenField)
            Dim chk = CType(e.Row.FindControl("chkmgr"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)
            Dim delTag = CType(e.Row.FindControl("lblDelTag"), Label)

            '******************Commented by Manoj Kumar 18-01-2021
            'Dim comnd As New OracleCommand()
            'comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_DEL_TAG ='N' and ss_pno='" & perno.Text & "' and ss_year='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO='" & Session("assespno").ToString & "'"

            'Dim d = getRecordInDt(comnd, conHrps)

            'If d.Rows.Count > 0 Then
            '    chk.Checked = True
            'Else
            '    chk.Checked = False
            'End If

            '*********************************

            If delTag.Text.ToString = "N" Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If

        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try

    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Protected Sub gvfinal_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim dt As DataTable = CType(ViewState("Data"), DataTable)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        ' Delete from ViewState.

        If Not IsDBNull(gvfinal.DataKeys(e.RowIndex).Value) Then
            'Dim id = gvfinal.DataKeys(e.RowIndex).Value4
            Dim id = CType(gvfinal.Rows(e.RowIndex).FindControl("lblpno"), Label)
            Dim aperno = CType(gvfinal.Rows(e.RowIndex).FindControl("lblapno"), Label)
            Dim catgry = CType(gvfinal.Rows(e.RowIndex).FindControl("lblCate"), Label)
            Dim email = CType(gvfinal.Rows(e.RowIndex).FindControl("lblemail"), Label)
            Dim val As String
            If id.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString(), aperno.Text, email.Text)
            Else
                val = Check(ViewState("FY").ToString(), aperno.Text, id.Text)
            End If
            If catgry.Text.ToUpper = "SELF" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Not remove " & val & " category...!")
                Exit Sub
            Else
                'dt.Rows(index).Delete()
                'ViewState("Data") = dt
                UpdateData(id.Text, "Y", catgry.Text)
            End If

            ' ShowGenericMessageModal(CommonConstants.AlertType.warning, " De-Selected...!")
            BindAssesorGrid(aperno.Text)
            '' Delete from Database.
            'Dim constr As String = ConfigurationManager.ConnectionStrings("conString").ConnectionString
            'Using con As SqlConnection = New SqlConnection(constr)
            '    Dim query As String = "DELETE FROM Customers WHERE CustomerId = @Id"
            '    Using cmd As SqlCommand = New SqlCommand(query)
            '        cmd.Connection = con
            '        cmd.Parameters.AddWithValue("@Id", id)
            '        con.Open()
            '        cmd.ExecuteNonQuery()
            '        con.Close()
            '    End Using
            'End Using
        End If
        'BindGridView()
    End Sub

    'Protected Sub chkmgr_CheckedChanged(sender As Object, e As EventArgs)
    '    Try
    '        Dim gv = CType(sender, CheckBox).Parent.Parent
    '        Dim chk = CType(gv.FindControl("chkmgr"), CheckBox)
    '        Dim id = CType(gv.FindControl("lblpno"), Label)
    '        Dim aperno = CType(gv.FindControl("lblapno"), Label)
    '        Dim catgry = CType(gv.FindControl("lblCate"), Label)
    '        Dim email = CType(gv.FindControl("lblemail"), Label)
    '        Dim val As String
    '        If id.Text.StartsWith("SR") Then
    '            val = Check(ViewState("FY").ToString(), aperno.Text, email.Text)
    '        Else
    '            val = Check(ViewState("FY").ToString(), aperno.Text, id.Text)
    '        End If

    '        If chk.Checked = True Then

    '            If val = "" Then
    '                UpdateData(id.Text, "N", catgry.Text)
    '                BindAssesorGrid(aperno.Text)
    '            Else
    '                ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
    '                chk.Checked = False
    '                Exit Sub
    '            End If

    '        Else
    '            If catgry.Text.ToUpper = "SELF" Then
    '                ShowGenericMessageModal(CommonConstants.AlertType.info, "Not remove " & val & " category...!")
    '                chk.Checked = True
    '                Exit Sub
    '            Else
    '                UpdateData(id.Text, "Y", catgry.Text)
    '            End If

    '            ' ShowGenericMessageModal(CommonConstants.AlertType.warning, " De-Selected...!")
    '            BindAssesorGrid(aperno.Text)
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Sub UpdateData(id As String, tag As String, catg As String)
        Try
            Dim query As String = String.Empty
            If tag = "Y" Then
                query = "delete from hrps.t_survey_status "  'Add by Manoj Kumar(osj1874) 18-01-2021
                query += "  where SS_PNO=:SS_PNO and ss_year=:fy"
                query += "  and SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG and nvl(SS_APP_TAG,'N') <> 'AP' and SS_SRLNO=:SS_SRLNO"
            End If
            'query = "update t_survey_status set SS_DEL_TAG ='" & tag & "',SS_ADM_TAG='1', SS_ADM_UPD_DT=SYSDATE,SS_UPDATED_DT=sysdate,"  'Add by Manoj Kumar(osj1874) 18-01-2021
            'query += " SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "' where SS_PNO='" & id & "' and ss_year='" & ViewState("FY").ToString() & "'"
            'query += "  and SS_ASSES_PNO='" & Session("assespno").ToString & "' and SS_CATEG='" & catg & "' and nvl(SS_APP_TAG,'N') <> 'AP' and SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_PNO", id)
            comnd.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString)
            comnd.Parameters.AddWithValue("SS_CATEG", catg)
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnaddpeertsl_Click(sender As Object, e As EventArgs)
        divtsl.Visible = True
        divntsl.Visible = False
        PopDroupdown()
        Reset()
    End Sub
    Protected Sub btnaddnontsl_Click(sender As Object, e As EventArgs)
        divtsl.Visible = False
        divntsl.Visible = True
        PopDroupdown()
        Reset()
    End Sub
    Protected Sub btnaddnorgI_Click(sender As Object, e As EventArgs)
        Try
            If txtnamenI.Text.Trim() = "" Or txtdeptnI.Text.Trim() = "" Or txtdesgnI.Text.Trim() = "" Or txtemailnI.Text.Trim() = "" Or ddlrole.SelectedValue = "Select assessors category" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, " Please Fill All Column...!")
                Exit Sub
            End If
            If ChkMail(txtemailnI.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If
            Dim pno = ""
            Dim assesor As String = String.Empty
            assesor = Session("assespno")
            Dim name = Replace(txtnamenI.Text.Trim.Split("(")(0), "'", "''")
            Dim desg = Replace(txtdesgnI.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptnI.Text.Trim, "'", "''")
            Dim email = Replace(txtemailnI.Text.Trim, "'", "''")

            Dim val = Check(ViewState("FY").ToString(), assesor.ToString(), email)
            If val = "" Then
                SaveData(ddlrole.SelectedValue.ToString(), pno, name, desg, dept, email, "", "NORG", assesor)
                Reset()
                BindAssesorGrid(assesor)
                ShowGenericMessageModal(CommonConstants.AlertType.success, " Added ...!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Function ChkMail(mail As String) As Boolean
        Dim val As Boolean = True
        Try
            Dim email = mail.Split("@")
            If email(1).ToUpper = UCase("TATASTEEL.COM") Then
                val = False
            Else
                val = True
            End If
        Catch ex As Exception

        End Try
        Return val
    End Function

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SurveyAdm_OPR
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select ema_ename ||'('|| ema_perno ||')' EName from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and (ema_perno like '%" & prefixText & "%' or upper(ema_ename) like "
            ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
            cmd.CommandText += " '%" & prefixText.ToUpper & "%')"   'WI368 add officer class
            'End by Manoj Kumar

            ob.conHrps.Open()

            cmd.Connection = ob.conHrps
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader
            While sdr.Read
                prefixes.Add(sdr("EName").ToString)
            End While



            Return prefixes
        Catch ex As Exception

            Return Nothing

        Finally

            ob.conHrps.Close()

        End Try

    End Function


    Protected Sub btnAddP_Click(sender As Object, e As EventArgs)
        Try
            If txtpnoI.Text.Trim() <> "" And DropDownList1.SelectedValue <> "Select assessors category" Then
                Dim assesor As String = String.Empty
                assesor = Session("assespno").ToString()
                Dim pno = txtpnoI.Text.Trim.Split("(")(1).TrimEnd(")")
                Dim name = Replace(txtpnoI.Text.Trim.Split("(")(0), "'", "''")
                Dim desg = Replace(txtdesgI.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptI.Text.Trim, "'", "''")
                Dim email = Replace(txtemailI.Text.Trim, "'", "''")

                Dim val = Check(ViewState("FY").ToString(), assesor.ToString(), pno)
                If val = "" Then
                    SaveData(DropDownList1.SelectedValue.ToString, pno, name, desg, dept, email, lbllevel.Text, "ORG", assesor)
                    Reset()
                    BindAssesorGrid(assesor)
                    ShowGenericMessageModal(CommonConstants.AlertType.success, " Added ...!")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                    Exit Sub
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank or Select assessors category Please fill...!")
                Exit Sub
            End If

        Catch ex As Exception
            ' MsgBox(ex.ToString)
        End Try
    End Sub
    Public Function ChkAuthlabel(pno As String) As String
        Try
            Dim chk As String = String.Empty
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            'Dim qry1 As New OracleCommand()
            'qry1.CommandText = "select SS_IL from t_assesse_IL  where SS_ASSESS_PNO=:SS_ASSESS_PNO and SS_STATUS='A'"
            'qry1.Connection = conHrps
            'qry1.Parameters.Clear()
            'qry1.Parameters.AddWithValue("SS_ASSESS_PNO", pno.ToString())
            'Dim daIL As New OracleDataAdapter(qry1)
            'Dim dtIL As New DataTable()
            'daIL.Fill(dtIL)
            'If dtIL.Rows.Count = 0 Then

            Dim qry As New OracleCommand()

            qry.CommandText = "select EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where ema_perno=:ema_perno and EMA_EQV_LEVEL in('I2','I3','I4','I5','I6')  AND EMA_COMP_CODE='1000' AND EMA_CYCLE='" & ViewState("SRLNO").ToString & "' AND EMA_YEAR='" & ViewState("FY").ToString & "'"
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_perno", pno.ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chk = dt.Rows(0).Item("EMA_EQV_LEVEL")
            Else
                chk = String.Empty
            End If
            'Else
            '    chk = dtIL.Rows(0).Item("SS_IL")
            'End If

            Return chk
        Catch ex As Exception
            ' ShowGenericMessageModal(CommonConstants.AlertType.error, ex.message)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function

    Public Sub PopDroupdown()
        Try
            Dim qry As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl = "I4" Or lvl = "I5" Or lvl = "I6" Then
                qry = "select IRC_CODE,decode(upper(IRC_DESC),'INTERNAL STAKEHOLDER','Internal Stakeholder/Peers/Subordinates',IRC_DESC) IRC_DESC from t_ir_codes where irc_type='360RL' and IRC_VALID_TAG='A' and IRC_CODE not in('SELF','ROPT','PEER') order by 2"
            End If
            If lvl = "I3" Then
                qry = "select IRC_CODE,decode(UPPER(IRC_DESC),'PEER','Peers/Subordinates',IRC_DESC) IRC_DESC from t_ir_codes where irc_type='360RL' and IRC_VALID_TAG='A' and IRC_CODE not in('SELF','ROPT') order by 2"
            End If
            If lvl = "I2" Then
                qry = "select IRC_CODE,decode(UPPER(IRC_DESC),'PEER','Peers',IRC_DESC) IRC_DESC from t_ir_codes where irc_type='360RL' and IRC_VALID_TAG='A' and IRC_CODE not in('SELF') order by 2"
            End If
            Dim adp As New OracleDataAdapter(qry, conHrps)
            Dim dt As New DataTable()
            adp.Fill(dt)
            If dt.Rows.Count > 0 Then
                ddlrole.DataTextField = "IRC_DESC"
                ddlrole.DataValueField = "IRC_CODE"
                ddlrole.DataSource = dt
                ddlrole.DataBind()

                ddlrole.Items.Insert(0, "Select assessors category")

                DropDownList1.DataTextField = "IRC_DESC"
                DropDownList1.DataValueField = "IRC_CODE"
                DropDownList1.DataSource = dt
                DropDownList1.DataBind()

                DropDownList1.Items.Insert(0, "Select assessors category")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub UpdateData1(id As String, tag As String, categ As String)
        Try
            SessionTimeOut()
            Dim query As String = String.Empty
            query = "update hrps.t_survey_status set SS_DEL_TAG =:SS_DEL_TAG,ss_categ=:ss_categ,SS_UPDATED_BY=:SS_UPDATED_BY,"
            query += " SS_UPDATED_DT=sysdate where (SS_PNO or upper(ss_email))=:ss_email  and ss_year=:ss_year "
            query += " and SS_ASSES_PNO =:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO "

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_DEL_TAG", tag)
            comnd.Parameters.AddWithValue("ss_categ", categ)
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            comnd.Parameters.AddWithValue("ss_email", id)
            comnd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString)
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Function Check(year As String, assespno As String, pno As String) As String
        Dim vl As String = String.Empty
        Try
            Dim str As New OracleCommand()
            str.CommandText = " select SS_PNO,irc_desc from hrps.t_survey_status,hrps.t_ir_codes where  SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO "
            str.CommandText += "and ((SS_PNO=:SS_PNO) OR (upper(ss_email)=:SS_PNO)) and SS_status='SE' and ss_del_tag ='N' and irc_type='360RL' and upper(ss_categ)=upper(irc_code) and SS_SRLNO=:SS_SRLNO"

            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            str.Parameters.AddWithValue("SS_ASSES_PNO", assespno.ToString())
            str.Parameters.AddWithValue("SS_PNO", pno.ToUpper())
            str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())

            Dim ds As New OracleDataAdapter(str)
            Dim dt As New DataTable()
            ds.Fill(dt)

            If dt.Rows.Count > 0 Then
                vl = dt.Rows(0)(1).ToString
            Else
                vl = ""
            End If
        Catch ex As Exception

        End Try
        Return vl
    End Function
    Public Function ChkValidationmaxstake() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='INTSH'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxstake1() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count > dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxmgr() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='MANGR'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxmgr1() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='MANGR'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count > dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Sub SaveData(ByVal role As String, ByVal pno As String, ByVal name As String, ByVal desg As String, ByVal org As String, ByVal email As String, ByVal lvl As String, ByVal orgtype As String, assespno As String)

        Try
            SessionTimeOut()
            Dim OrgStr As String = String.Empty
            Dim id = getRefNo()

            If pno = "" Then
                pno = id
            End If
            'If role = "INTSH" Then
            Dim statmaxstake = ChkValidationRangeCategory(role.ToString)
            If Len(statmaxstake) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxstake & " Category exceed maximum number")
                Exit Sub
            End If
            'End If
            'If role = "MANGR" Then
            '    Dim statmaxman = ChkValidationRangeCategory(role.ToString)
            '    If Len(statmaxman) > 0 Then
            '        ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxman & " Category exceed maximum number")
            '        Exit Sub
            '    End If
            'End If
            OrgStr = "insert into hrps.T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL,SS_ADM_TAG,SS_APP_TAG,SS_APPROVER,SS_WFL_STATUS,SS_SRLNO) values (:SS_CATEG, :SS_ID,:SS_PNO,:SS_NAME,:SS_DESG,:SS_DEPT,:SS_EMAIL,:SS_STATUS,:SS_TAG, "
            OrgStr += ":SS_CRT_BY, sysdate,:SS_DEL_TAG,:SS_TYPE,:ss_year,:SS_ASSES_PNO,:SS_LEVEL,'1','','','',:SS_SRLNO)"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim comnd As New OracleCommand(OrgStr, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_CATEG", role)
            comnd.Parameters.AddWithValue("SS_ID", id)
            comnd.Parameters.AddWithValue("SS_PNO", pno)
            comnd.Parameters.AddWithValue("SS_NAME", name)
            comnd.Parameters.AddWithValue("SS_DESG", Replace(desg, "'", "''"))
            comnd.Parameters.AddWithValue("SS_DEPT", Replace(org, "'", "''"))
            comnd.Parameters.AddWithValue("SS_EMAIL", email)
            comnd.Parameters.AddWithValue("SS_STATUS", "SE")
            comnd.Parameters.AddWithValue("SS_TAG", "")
            comnd.Parameters.AddWithValue("SS_CRT_BY", Session("ADM_USER").ToString())
            comnd.Parameters.AddWithValue("SS_DEL_TAG", "N")
            comnd.Parameters.AddWithValue("SS_TYPE", orgtype.ToString())
            comnd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", assespno.ToString)
            comnd.Parameters.AddWithValue("SS_LEVEL", lvl.ToString)
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())

            comnd.ExecuteNonQuery()
            ' Clear()

        Catch ex As Exception
            ' MsgBox(ex.ToString)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub
    Public Function ChkValidationRangeCategory(ByVal cate As String) As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc irc_desc1,decode(a.irc_type,'360V2',decode(a.IRC_CODE,'PEER','Peers','ROPT','Subordinates',b.irc_desc),'360V3',decode(a.IRC_CODE,'PEER','Peer/Subordinate',b.irc_desc),decode(a.IRC_CODE,'INTSH','Peer/Subordinate/Internal Stackholder',b.irc_desc)) irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE=:IRC_CODE"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            cmdqry.Parameters.AddWithValue("IRC_CODE", cate.ToString)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                        lbls.Text = ""
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationMiniRangeCategory() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc irc_desc1,decode(a.irc_type,'360V2',decode(a.IRC_CODE,'PEER','Peers','ROPT','Subordinates',b.irc_desc),'360V3',decode(a.IRC_CODE,'PEER','Peer/Subordinate',b.irc_desc),decode(a.IRC_CODE,'INTSH','Peer/Subordinate/Internal Stackholder',b.irc_desc)) irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' "
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            'cmdqry.Parameters.AddWithValue("IRC_CODE", cate.ToString)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)



                    If dtls.Rows.Count < dt.Rows(i)("minmum") Then
                        status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("minmum").ToString() & "),"
                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function getRefNo() As String
        Try
            Dim strRef As String = String.Empty
            Dim refno As String = String.Empty
            strRef = " select MAX(to_number(substr(SS_ID,3,10))) SS_ID from hrps.T_SURVEY_STATUS where SS_ID like 'SR%' and SS_YEAR='" & ViewState("FY").ToString() & "'"
            Dim mycommand As OracleCommand
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            mycommand = New OracleCommand(strRef, conHrps)

            Dim result As Object = mycommand.ExecuteScalar

            If IsDBNull(result) Then
                refno = "1"
            Else
                Dim rs = result + 1
                refno = CStr(rs)
            End If

            refno = refno.PadLeft(10, "0")
            refno = "SR" + refno

            Return refno
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function

    Protected Sub txtpnoI_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoI.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " (ema_ename||'(' ||ema_perno ||')')=:ema_perno   and ema_comp_code='1000' AND EMA_CYCLE='" & ViewState("SRLNO").ToString & "' AND EMA_YEAR='" & ViewState("FY").ToString & "'"
            'End by Maoj Kumar on 31-05-2021
            strself.Connection = conHrps
            strself.Parameters.Clear()
            strself.Parameters.AddWithValue("ema_perno", pno.ToString())
            Dim dt = getDataInDt(strself)

            If dt.Rows.Count > 0 Then
                txtdesgI.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtemailI.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtemailI.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgI.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()

                lbllevel.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

                If txtemailI.Text = "" Then
                    txtemailI.ReadOnly = False
                Else
                    txtemailI.ReadOnly = True
                End If
                txtdeptI.ReadOnly = True
                txtdesgI.ReadOnly = True
            Else

                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                Reset()
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub Reset()
        txtnamenI.Text = ""
        txtdesgnI.Text = ""
        txtemailnI.Text = ""
        txtdeptnI.Text = ""
        ddlrole.SelectedValue = "Select assessors category"
        DropDownList1.SelectedValue = "Select assessors category"
        txtdeptI.Text = ""
        txtemailI.Text = ""
        txtdesgI.Text = ""
        txtpnoI.Text = ""
    End Sub

    Protected Sub bindself()
        Try

            Dim assespno As String = String.Empty
            Dim btn As New Label
            Dim appbtn As New LinkButton
            For Each gv As GridViewRow In gvself.Rows
                assespno = CType(gv.FindControl("lblpno"), Label).Text
                btn = CType(gv.FindControl("lblstats"), Label)
                appbtn = CType(gv.FindControl("lbpendingapproval"), LinkButton)


                Dim p As Boolean = False

                Dim type1 As String = ChkAuthlabel(assespno)
                Dim type As String = String.Empty
                If type1.Equals("I5") Then
                    type = "360V5"
                ElseIf type1.Equals("I4") Then
                    type = "360V4"
                ElseIf type1.Equals("I3") Then
                    type = "360V3"
                ElseIf type1.Equals("I6") Then
                    type = "360V6"
                ElseIf type1.Equals("I2") Then
                    type = "360V2"
                End If
                Dim cmdqry As New OracleCommand()
                cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
                cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
                'Start by Manoj Kumar 30-05-2021 WI368  Not show criteria comleted in Yes/No. By default show 'No' value. (Bind variable error)
                cmdqry.Connection = conHrps
                cmdqry.Parameters.Clear()
                cmdqry.Parameters.AddWithValue("type", type)
                'cmdqry.Parameters.Add(New OracleParameter(":type", type))
                'cmdqry.Parameters.Add(New OracleParameter(":SS_SRLNO", dtsrl.Rows(0).Item(0)))
                Dim dt = getDataInDt(cmdqry)
                'End by Manoj Kumar 30-05-2021

                For d = 0 To dt.Rows.Count - 1

                    Dim qry As New OracleCommand()
                    qry.CommandText = "select * from hrps.t_survey_status where ss_asses_pno=:ss_asses_pno and ss_status='SE' and ss_del_tag='N' and upper(ss_categ) = "
                    qry.CommandText += " :IRC_CODE and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO"
                    qry.Connection = conHrps
                    qry.Parameters.Clear()
                    qry.Parameters.AddWithValue("ss_asses_pno", assespno)
                    qry.Parameters.AddWithValue("IRC_CODE", dt.Rows(d)("IRC_CODE").ToString.ToUpper)
                    qry.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
                    qry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    Dim s = getDataInDt(qry)
                    If dt.Rows(d)("maximum") = "N" Then
                        If s.Rows.Count < dt.Rows(d)("minmum") Then
                            p = True
                        End If

                    Else
                        If s.Rows.Count > dt.Rows(d)("maximum") Or s.Rows.Count < dt.Rows(d)("minmum") Then
                            p = True
                        End If
                    End If

                Next
                If p Then
                    btn.Text = "No"
                Else
                    btn.Text = "Yes"
                End If

                Dim tag = CheckApproved(assespno)
                Dim SubmitTag = CheckSubmitted(assespno)

                If tag = "AP" Then
                    appbtn.Text = "Approved"
                ElseIf tag = "RJ" Then
                    appbtn.Text = "Returned"
                Else
                    If SubmitTag = "SU" Then
                        appbtn.Text = "Submitted"
                    Else
                        appbtn.Text = "Not Submitted"
                    End If

                End If
            Next
        Catch ex As Exception
            ' MsgBox(ex.ToString)
        End Try
    End Sub

    Public Function CheckApproved(pno As String) As String
        Try
            Dim tg As String
            Dim cm As New OracleCommand()
            cm.CommandText = "select distinct ss_app_tag from hrps.t_survey_status  where ss_asses_pno=:ss_asses_pno and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO"
            cm.CommandText += " and ss_del_tag='N' and ss_status='SE'"
            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("ss_asses_pno", pno)
            cm.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            cm.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim dt = getDataInDt(cm)
            If dt.Rows.Count > 0 Then
                tg = dt.Rows(0)(0).ToString()
            Else
                tg = ""
            End If
            Return tg
        Catch ex As Exception

        End Try
    End Function
    Public Function CheckSubmitted(pno As String) As String
        Try
            Dim stg As String
            Dim cm As New OracleCommand()
            cm.CommandText = "select distinct ss_tag from hrps.t_survey_status  where ss_asses_pno=:ss_asses_pno and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO"
            cm.CommandText += " and ss_del_tag='N' and ss_status='SE'"
            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("ss_asses_pno", pno)
            cm.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            cm.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim dt = getDataInDt(cm)
            If dt.Rows.Count > 0 Then
                stg = dt.Rows(0)(0).ToString()
            Else
                stg = ""
            End If
            Return stg
        Catch ex As Exception

        End Try
    End Function

    Protected Sub Submit()
        Try
            SessionTimeOut()
            Dim status As String = ChkValidationMiniRangeCategory()
            If Len(status) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & status & " Category")
                Exit Sub
            End If
            Dim statmaxstake = ChkValidationmaxstake1()
            If Len(statmaxstake) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxstake & " Category exceed maximum number")
                Exit Sub
            End If
            'Dim statmaxman = ChkValidationmaxmgr1()
            'If Len(statmaxman) > 0 Then
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxman & " Category exceed maximum number")
            '    Exit Sub
            'End If

            Dim comd As New OracleCommand()
            Dim approver As String = String.Empty
            If Session("label").Equals("I2") Then
                comd.CommandText = " select nvl(EMA_DOTTED_PNO,'NA') EMA_REPORTING_TO_PNO"
            Else
                comd.CommandText = " select nvl(EMA_REPORTING_TO_PNO,'NA') EMA_REPORTING_TO_PNO"
            End If

            comd.CommandText += " from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno and EMA_COMP_CODE='1000' AND EMA_CYCLE=:EMA_CYCLE AND EMA_YEAR=:EMA_YEAR"
            comd.Connection = conHrps
            comd.Parameters.Clear()
            comd.Parameters.AddWithValue("ema_perno", Session("assespno").ToString())
            comd.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString)
            comd.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString)
            Dim dt = getDataInDt(comd)
            If dt.Rows.Count > 0 Then
                approver = dt.Rows(0)(0).ToString()
            End If
            Dim upqry As String = String.Empty

            upqry = " update hrps.t_survey_status set SS_FLAG1 ='1',SS_TAG='SU',SS_WFL_STATUS='1',SS_ADM_UPD_DT=sysdate,SS_UPDATED_DT=sysdate,SS_APP_TAG=null,"
            upqry += " SS_UPDATED_BY=:SS_UPDATED_BY,SS_APPROVER =:SS_APPROVER  where  SS_ASSES_PNO =:SS_ASSES_PNO and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO"
            upqry += " and ss_del_tag='N' and ss_status='SE'"
            Dim c As New OracleCommand()
            c.CommandText = upqry
            c.Connection = conHrps
            c.Parameters.Clear()
            c.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            c.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
            c.Parameters.AddWithValue("ss_year", ViewState("FY").ToString)
            c.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            c.Parameters.AddWithValue("SS_APPROVER", approver.ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            c.ExecuteNonQuery()
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Form has been submitted...!")
            BindAssesorGrid(Session("assespno").ToString())
            bindGrid()
            gvfinal.Visible = False
            btnsubmit.Visible = False
            btnaddpeertsl.Visible = False
            btnaddnontsl.Visible = False
            divtsl.Visible = False
            divntsl.Visible = False
            Session.Remove("assespno")
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
                Session.Remove("assespno")
            End If
        End Try
    End Sub

    Public Function ChkValidation() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty

            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"

            End If
            ' Start WI368  by Manoj Kumar on 30-05-2021
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,decode(b.irc_desc,'Peer','Peers And Subordinates',b.irc_desc) irc_desc from t_ir_codes a,t_ir_codes b "   'WI368 for min. and max. variable for category wise
            'End by Manoj Kumar on 30-05-2021
            cmdqry.CommandText += "  where a.irc_type='" + type + "' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            If type.Equals("360V5") Or type.Equals("360V6") Or type.Equals("360V4") Then
                cmdqry.CommandText += " and a.IRC_CODE not in('PEER','ROPT')"
            ElseIf type.Equals("360V3") Then
                cmdqry.CommandText += " and a.IRC_CODE not in('ROPT')"
            End If
            Dim dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)

                    If dtls.Rows.Count < dt.Rows(i)("minmum") Then
                        status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("minmum").ToString() & "), "
                    End If

                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Sub submitpend(sender As Object, e As EventArgs)

    End Sub
    Public Function ChkValidationMiniApprover(ByVal pno As String) As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(pno)
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc irc_desc1,decode(a.irc_type,'360V2',decode(a.IRC_CODE,'PEER','Peers','ROPT','Subordinates',b.irc_desc),'360V3',decode(a.IRC_CODE,'PEER','Peer/Subordinate',b.irc_desc),decode(a.IRC_CODE,'INTSH','Peer/Subordinate/Internal Stackholder',b.irc_desc)) irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' "
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            'cmdqry.Parameters.AddWithValue("IRC_CODE", cate.ToString)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", pno.ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)



                    If dtls.Rows.Count < dt.Rows(i)("minmum") Then
                        status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("minmum").ToString() & "),"
                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxApprover(ByVal perno As String) As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(perno.ToString)
            If lvl.Equals("I5") Then
                type = "360V5"
            ElseIf lvl.Equals("I4") Then
                type = "360V4"
            ElseIf lvl.Equals("I3") Then
                type = "360V3"
            ElseIf lvl.Equals("I6") Then
                type = "360V6"
            ElseIf lvl.Equals("I2") Then
                type = "360V2"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", perno.ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count > dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Protected Sub lbpend_Click(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, LinkButton).Parent.Parent
            Dim asspno = CType(gv.FindControl("lblpno1"), Label)
            SessionTimeOut()
            Dim status As String = ChkValidationMiniApprover(asspno.Text.ToString)
            If Len(status) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & status & " Category")
                Exit Sub
            End If
            Dim statmaxstake = ChkValidationmaxApprover(asspno.Text.ToString)
            If Len(statmaxstake) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxstake & " Category exceed maximum number")
                Exit Sub
            End If
            showConfirmMessageModal(CommonConstants.AlertType.info, asspno.Text.ToString)
            'ClientScript.RegisterStartupScript(Me.GetType(), "Popup", "ShowApprovePopup();", True)showConfirmMessageModal
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openMode", "ShowApprovePopup();", True)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openMode", "ShowApprovePopup();", True)
            'Approve(asspno.Text, "AP")
            'ShowGenericMessageModal(CommonConstants.AlertType.success, "Approved...!")
            'bindpendinggrid()
        Catch ex As Exception

        End Try
    End Sub
    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function SubmitClick(ByVal User As String, ByVal Syear As String, ByVal Scyc As String, ByVal UserId As String) As Boolean
        ''Do your Stuff Here.
        'Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
        Dim conn As String = ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString
        Dim confun As New OracleConnection(conn)
        Try
            If Convert.ToString(HttpContext.Current.Session("ADM_USER")) <> Convert.ToString(UserId) Then
                Return False
            End If

            Dim qry As String = ""
            qry = "update hrps.t_survey_status set SS_APP_TAG=:SS_APP_TAG,SS_TAG_DT = sysdate, SS_ACTION_BY='ADM',SS_WFL_STATUS='2',SS_UPDATED_BY= "
            qry += ":SS_UPDATED_BY,SS_UPDATED_DT=sysdate where  ss_year=:ss_year  and SS_ASSES_PNO =:SS_ASSES_PNO "
            qry += "and SS_STATUS='SE' and SS_DEL_TAG='N' and SS_TAG ='SU' and SS_SRLNO=:SS_SRLNO"
            If confun.State = ConnectionState.Closed Then
                confun.Open()
            End If
            Dim c As New OracleCommand()
            c.CommandText = qry
            c.Connection = confun
            c.Parameters.AddWithValue("SS_APP_TAG", "AP")
            c.Parameters.AddWithValue("SS_UPDATED_BY", UserId.ToString())
            c.Parameters.AddWithValue("ss_year", Syear.ToString())
            c.Parameters.AddWithValue("SS_ASSES_PNO", User.ToString)
            c.Parameters.AddWithValue("SS_SRLNO", Scyc.ToString())
            Dim valid = c.ExecuteNonQuery()
            If valid > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
        Finally
            If Not confun.State = ConnectionState.Closed Then ' check if the connection is already closed
                confun.Close()
            End If
        End Try
    End Function
    Public Sub showConfirmMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showConfirmMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Protected Sub lnkreset_Click(sender As Object, e As EventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openMode", "showmodalAddSabashAwardee();", True)
    End Sub
    Private Function checkreturneligible(ByVal pno As String, ByVal serial As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmdeligible As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            ' Start WI368  by Manoj Kumar on 30-05-2021
            cmdeligible.CommandText = "select ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and ss_asses_pno=:pno and  SS_SRLNO=:SS_SRLNO and ss_app_tag='AP'"  'WI368 Not return to assessor (bind variable error)
            'End by Manoj Kumar 30-05-2021
            'If conHrps.State = ConnectionState.Closed Then
            '    conHrps.Open()
            'End If
            cmdeligible.Connection = conHrps
            cmdeligible.Parameters.Clear()
            cmdeligible.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmdeligible.Parameters.Add(New OracleParameter("SS_SRLNO", serial.Trim))
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
    Private Function checkreturneligiblesub(ByVal pno As String, ByVal serial As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmd As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            cmd.CommandText = "select distinct ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and ss_asses_pno=:pno and  SS_SRLNO =:SS_SRLNO and ss_app_tag is null and (SS_WFL_STATUS='1' or SS_WFL_STATUS is null)"
            'If conHrps.State = ConnectionState.Closed Then
            '    conHrps.Open()
            'End If

            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmd.Parameters.Add(New OracleParameter("SS_SRLNO", serial.Trim))
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
    Private Function checkreverteligiblesub(ByVal pno As String, ByVal serial As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmd As New OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            cmd.CommandText = "select distinct ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and ss_asses_pno=:pno and  SS_SRLNO =:SS_SRLNO and ss_app_tag is null and SS_WFL_STATUS='1'"
            'If conHrps.State = ConnectionState.Closed Then
            '    conHrps.Open()
            'End If

            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("pno", pno.Trim))
            cmd.Parameters.Add(New OracleParameter("SS_SRLNO", serial.Trim))
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
    Private Function checkreturnsurveyeligible(ByVal pno As String, ByVal serial As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmd As OracleCommand
        Dim dt As New DataTable
        Dim st As String = String.Empty
        Try
            ls_sql = "select ss_wfl_status from hrps.t_survey_status where ss_tag='SU' and upper(ss_asses_pno)=:ss_asses_pno and  SS_SRLNO =:SS_SRLNO"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd = New OracleCommand(ls_sql, conHrps)
            cmd.Parameters.Add(New OracleParameter(":ss_asses_pno", pno.Trim))
            cmd.Parameters.Add(New OracleParameter(":SS_SRLNO", serial.Trim))
            dt = getRecordInDt(cmd, conHrps)
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
    Protected Sub btnreset_Click(sender As Object, e As EventArgs)
        Try
            SessionTimeOut()
            If txtperno.Text.Trim <> "" Then

                Dim chk = checkreturneligible(txtperno.Text.Trim.ToUpper(), ViewState("SRLNO").ToString())
                If chk = "N" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot return the form. It is already approved")
                    Exit Sub
                End If
                Dim chksurvey = checkreturneligiblesub(txtperno.Text.Trim.ToUpper(), ViewState("SRLNO").ToString())
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
                com.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
                com.Parameters.AddWithValue("ss_asses_pno", txtperno.Text.Trim.ToUpper())
                com.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                ' Start WI368  by Manoj Kumar on 30-05-2021
                'dtsrl = getRecordInDt(com, conHrps)
                'End by Manoj Kumar on 30-05-2021
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim r = com.ExecuteNonQuery()
                ' Start WI368  by Manoj Kumar on 30-05-2021
                If r = 0 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Incorrect P.no entred...!")  'WI368 execute query sow correct message'
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Returned...!")
                    btnsearch_click(sender, e)
                    'End by Manoj Kumar on 30-05-2021
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter Personal number...!")
                lnkreset_Click(sender, e)
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btncommapp_Click(sender As Object, e As EventArgs)
        Dim asspno As String = String.Empty
        Dim strrole As String = String.Empty
        Dim d As New Boolean
        Dim newl As String = Chr(13) & Chr(10)
        Dim q As New OracleCommand()

        q.CommandText = "select distinct IGP_user_id from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
        q.CommandText += " and IGP_STATUS ='A' and IGP_user_id=:IGP_user_id"
        'If conHrps.State = ConnectionState.Closed Then
        '    conHrps.Open()
        'End If
        q.Connection = conHrps
        q.Parameters.Clear()
        q.Parameters.AddWithValue("IGP_user_id", Session("ADM_USER").ToString().Trim)
        ' q = New OracleCommand(strrole, conHrps)
        Dim p = getDataInDt(q)
        If p.Rows.Count > 0 Then
            d = True
        Else
            d = False

        End If
        If d = False Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "You Do not have priviledge to send mail")
            Exit Sub
        End If
        If txt_pnocom.Text.Trim = "" Then
            Exit Sub
        End If
        asspno = txt_approvemail.Text.Trim
        Dim msg As String = String.Empty
        Dim ls_sql As String = String.Empty
        Dim cmd As OracleCommand
        Dim dt As New DataTable
        Try
            'Start by Manoj Kumar 31-05-2021 add emp class 1
            ls_sql = " select distinct emplb.ema_perno, emplb.ema_email_id from hrps.t_emp_master_feedback360 empla,hrps.t_survey_status st,hrps.t_emp_master_feedback360 emplb where empla.ema_perno=st.ss_asses_pno   and  empla.ema_perno=st.ss_asses_pno and empla.ema_reporting_to_pno=emplb.ema_perno and empla.ema_comp_code=emplb.ema_comp_code  and empla.ema_eqv_level in('I3','I4','I5','I6')  and SS_TAG='SU' and empla.ema_comp_code='1000' and st.ss_app_tag is null   and  st.ss_year=(select IRC_DESC from hrps.t_ir_codes cd where cd.irc_type='360YS') and st.ss_srlno=( select IRC_DESC from hrps.t_ir_codes cd where cd.irc_type ='SL360' and cd.irc_valid_tag='Y')  and st.ss_wfl_status='1' and empla.ema_perno='" & asspno.Trim() & "'"
            'End by Manoj Kumar 31-05-2021
            cmd = New OracleCommand(ls_sql, conHrps)
            'cmd.Parameters.Add(New OracleParameter(":ema_perno", asspno))

            dt = getRecordInDt(cmd, conHrps)
            If dt.Rows.Count > 0 Then
                Dim link As String = "Select IRC_DESC from t_ir_codes where IRC_TYPE='ln360' and irc_code='lnka'"
                Dim date1 As String = "Select  IRC_DESC  from t_ir_codes where IRC_TYPE='360dt' and irc_code='lnka'"
                Dim line1 As String = "Select irc_desc    from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LAP1'"
                Dim line2 As String = "Select irc_desc    from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LAP2'"
                Dim line3 As String = "Select Case irc_desc    from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LAP3'"
                Dim line4 As String = "Select irc_desc    from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LAP4'"
                Dim line5 As String = "Select Case irc_desc    from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LAP6'"
                Dim line6 As String = "Select  irc_desc    from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LAP7'"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim cmd1 As OracleCommand = New OracleCommand(link, conHrps)
                Dim cmd2 As OracleCommand = New OracleCommand(date1, conHrps)
                Dim cmd3 As OracleCommand = New OracleCommand(line1, conHrps)
                Dim cmd4 As OracleCommand = New OracleCommand(line2, conHrps)
                Dim cmd5 As OracleCommand = New OracleCommand(line3, conHrps)
                Dim cmd6 As OracleCommand = New OracleCommand(line4, conHrps)
                Dim cmd7 As OracleCommand = New OracleCommand(line5, conHrps)
                Dim cmd8 As OracleCommand = New OracleCommand(line6, conHrps)

                'Dim dtlink As OracleCommand = getRecordInDt(cmd1, conHrps)
                Dim dtdate As DataTable = getRecordInDt(cmd2, conHrps)
                Dim dtline1 As DataTable = getRecordInDt(cmd3, conHrps)
                Dim dtline2 As DataTable = getRecordInDt(cmd4, conHrps)
                Dim dtline3 As DataTable = getRecordInDt(cmd5, conHrps)
                Dim dtline4 As DataTable = getRecordInDt(cmd6, conHrps)
                Dim dtline5 As DataTable = getRecordInDt(cmd7, conHrps)
                Dim dtline6 As DataTable = getRecordInDt(cmd8, conHrps)
                Dim dtlink As DataTable = getRecordInDt(cmd1, conHrps)

                Dim mail As New System.Net.Mail.MailMessage()
                mail.To.Add(dt.Rows(0).Item("ema_email_id"))
                mail.From = New MailAddress("hrm@tatasteel.com", " ", System.Text.Encoding.UTF8)
                mail.Subject = "Baseline 360-degree Survey : Identification of Respondents"
                mail.SubjectEncoding = System.Text.Encoding.UTF8
                mail.Body = " Dear Colleague," + "<br/>" + dtline1.Rows(0).Item("IRC_DESC") + "<br/>" + dtline2.Rows(0).Item("IRC_DESC") + " " + dtdate.Rows(0).Item("IRC_DESC") + " " + dtline3.Rows(0).Item("IRC_DESC") + "<br></br>" + dtlink.Rows(0).Item("IRC_DESC") + "<br/>" + dtline4.Rows(0).Item("IRC_DESC") + "<br/>" + dtline5.Rows(0).Item("IRC_DESC") + "<br/><br/>" + "With Regards," + "<br/>" + "Team HRM"

                mail.BodyEncoding = System.Text.Encoding.UTF8
                mail.IsBodyHtml = True
                mail.Priority = MailPriority.High
                Dim client As New SmtpClient()
                client.Credentials = New System.Net.NetworkCredential(" ", "")
                client.Port = 25
                client.Host = "144.0.11.253"
                'client.Port = 587
                'client.Host = "smtp.gmail.com"
                client.EnableSsl = False
                client.Send(mail)
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been send successfully!")
            End If
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Issue in sending mail")
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Protected Sub btncomm_Click(sender As Object, e As EventArgs)
        Dim asspno As String = String.Empty
        Dim strrole As String = String.Empty
        Dim d As New Boolean
        Dim newl As String = Chr(13) & Chr(10)
        Dim q As New OracleCommand()

        q.CommandText = "select distinct IGP_user_id from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
        q.CommandText += " and IGP_STATUS ='A' and IGP_user_id=:IGP_user_id"
        'If conHrps.State = ConnectionState.Closed Then
        '    conHrps.Open()
        'End If
        q.Connection = conHrps
        q.Parameters.Clear()
        q.Parameters.AddWithValue("IGP_user_id", Session("ADM_USER").ToString().Trim)
        'q = New OracleCommand(strrole, conHrps)
        Dim p = getDataInDt(q)
        If p.Rows.Count > 0 Then
            d = True
        Else
            d = False

        End If
        If d = False Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "You Do not have priviledge to send mail")
            Exit Sub
        End If
        If txt_pnocom.Text.Trim = "" Then
            Exit Sub
        End If
        asspno = txt_pnocom.Text.Trim
        Dim msg As String = String.Empty
        Dim ls_sql As String = String.Empty
        Dim cmd As OracleCommand
        Dim dt As New DataTable
        Try
            'Start by Manoj Kumar 31-05-2021 add emp class 1
            ls_sql = "select empla.ema_perno,empla.ema_email_id from hrps.t_emp_master_feedback360 empla,hrps.t_ir_adm_grp_privilege pr where empla.ema_perno=pr.igp_user_id and empla.ema_comp_code='1000' and empla.ema_eqv_level in('I3','I4','I5','I6') AND empla.ema_cycle='" & ViewState("SRLNO").ToString & "' AND empla.EMA_YEAR='" & ViewState("FY").ToString & "' and  pr.igp_group_id='360ASSEESSE' and empla.ema_perno='" & asspno.Trim & "'"
            'End by Manoj Kumar 31-05-2021
            cmd = New OracleCommand(ls_sql, conHrps)
            'cmd.Parameters.Add(New OracleParameter(":ema_perno", asspno))

            dt = getRecordInDt(cmd, conHrps)
            If dt.Rows.Count > 0 Then
                Dim link As String = "Select IRC_DESC from t_ir_codes where IRC_TYPE='ln360' and irc_code='lnks'"
                Dim date1 As String = "Select  IRC_DESC  from t_ir_codes where IRC_TYPE='360dt' and irc_code='lnks'"
                Dim line1 As String = "Select irc_desc from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LA1'"
                Dim line2 As String = "Select irc_desc  from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LA2'"
                Dim line3 As String = "Select irc_desc from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LA3'"
                Dim line4 As String = "select irc_desc from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LA4'"
                Dim line5 As String = "Select irc_desc from hrps.t_ir_codes cd where cd.irc_type='360M' and IRC_CODE='LA6'"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim cmd1 As OracleCommand = New OracleCommand(link, conHrps)
                Dim cmd2 As OracleCommand = New OracleCommand(date1, conHrps)
                Dim cmd3 As OracleCommand = New OracleCommand(line1, conHrps)
                Dim cmd4 As OracleCommand = New OracleCommand(line2, conHrps)
                Dim cmd5 As OracleCommand = New OracleCommand(line3, conHrps)
                Dim cmd6 As OracleCommand = New OracleCommand(line4, conHrps)
                Dim cmd7 As OracleCommand = New OracleCommand(line5, conHrps)


                'Dim dtlink As OracleCommand = getRecordInDt(cmd1, conHrps)
                Dim dtdate As DataTable = getRecordInDt(cmd2, conHrps)
                Dim dtline1 As DataTable = getRecordInDt(cmd3, conHrps)
                Dim dtline2 As DataTable = getRecordInDt(cmd4, conHrps)
                Dim dtline3 As DataTable = getRecordInDt(cmd5, conHrps)
                Dim dtline4 As DataTable = getRecordInDt(cmd6, conHrps)
                Dim dtline5 As DataTable = getRecordInDt(cmd7, conHrps)
                Dim dtlink As DataTable = getRecordInDt(cmd1, conHrps)

                Dim mail As New System.Net.Mail.MailMessage()
                mail.To.Add(dt.Rows(0).Item("ema_email_id"))
                mail.From = New MailAddress("hrm@tatasteel.com", " ", System.Text.Encoding.UTF8)
                mail.Subject = "Baseline 360-degree Survey : Identification of Respondents"
                mail.SubjectEncoding = System.Text.Encoding.UTF8
                mail.Body = " Dear Colleague," + "<br/>" + dtline1.Rows(0).Item("IRC_DESC") + "<br/>" + dtline2.Rows(0).Item("IRC_DESC") + dtline3.Rows(0).Item("IRC_DESC") + " " + dtdate.Rows(0).Item("IRC_DESC") + "<br/>" + "<br/>" + dtlink.Rows(0).Item("IRC_DESC") + "<br/>" + dtline4.Rows(0).Item("IRC_DESC") + "<br/>" + dtline5.Rows(0).Item("IRC_DESC") + "<br/>" + "With Regards," + "<br/>" + "Team HRM"

                mail.BodyEncoding = System.Text.Encoding.UTF8
                mail.IsBodyHtml = True
                mail.Priority = MailPriority.High
                Dim client As New SmtpClient()
                client.Credentials = New System.Net.NetworkCredential(" ", "")
                client.Port = 25
                client.Host = "144.0.11.253"
                'client.Port = 587
                'client.Host = "smtp.gmail.com"
                client.EnableSsl = False
                client.Send(mail)
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been send successfully!")
            End If
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Issue In sending mail")
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnrevert_Click(sender As Object, e As EventArgs)
        Dim ls_sql1 As String = String.Empty
        Dim pnoid As String = String.Empty
        Try

            SessionTimeOut()
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
                cmd5.Parameters.AddWithValue("SS_YEAR", ViewState("FY"))
                cmd5.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim)
                cmd5.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
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


            upres.CommandText = "update hrps.t_survey_status  set ss_wfl_status=:ss_wfl_status1, SS_UPDATED_DT=sysdate,SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO=:SS_ASSES_PNO and (upper(SS_PNO)=:SS_PNO) and "
            upres.CommandText += "ss_wfl_status in('3','9') and ss_year=:ss_yar and SS_SRLNO=:SS_SRLNO"

            upres.Connection = conHrps
            upres.Parameters.Clear()
            upres.Parameters.AddWithValue("ss_wfl_status1", "2")
            upres.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim)
            upres.Parameters.AddWithValue("SS_PNO", pnoid)
            upres.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            ' upres.Parameters.AddWithValue("ss_wfl_status", s)
            upres.Parameters.AddWithValue("ss_yar", ViewState("FY").ToString())
            upres.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
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
            upres.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            upres.Parameters.AddWithValue("SS_SERIAL", ViewState("SRLNO").ToString())
            Dim result1 = upres.ExecuteNonQuery()

            txtassespno.Text = ""
            txtrespno.Text = ""
            If result1 > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Feedback has been reverted...!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No feedback has been reverted...!")
            End If

        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnR_Click(sender As Object, e As EventArgs)

        'WI624: super admin can remove respondent ffrom list even if respondent list has been approved.
        'created by: Avik Mukherjee
        'Created on: 16-06-2021
        Dim ls_sql1 As String = String.Empty
        Dim pnoid As String = String.Empty
        Try

            SessionTimeOut()
            Dim upres As New OracleCommand()
            If txt_Rpno.Text.ToUpper().Trim().Length > 6 Then
                Dim cmd5 As OracleCommand
                ls_sql1 = "Select SS_PNO from hrps.t_Survey_status where upper(SS_EMAIL)=:SS_EMAIL and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO and SS_WFL_STATUS<>'3' and ss_srlno=:ss_srlno"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                cmd5 = New OracleCommand(ls_sql1, conHrps)
                cmd5.Parameters.Clear()
                cmd5.Parameters.AddWithValue("SS_EMAIL", txt_Rpno.Text.ToUpper().Trim)
                cmd5.Parameters.AddWithValue("SS_YEAR", ViewState("FY"))
                cmd5.Parameters.AddWithValue("SS_ASSES_PNO", txt_Rasspno.Text.Trim)
                cmd5.Parameters.AddWithValue("ss_srlno", ViewState("SRLNO").ToString)
                Dim da5 As New OracleDataAdapter(cmd5)
                Dim dt5 As New DataTable
                da5.Fill(dt5)
                If dt5.Rows.Count > 0 Then
                    pnoid = dt5.Rows(0).Item(0)
                Else

                End If
            Else
                pnoid = txt_Rpno.Text.ToUpper().Trim()
            End If


            upres.CommandText = "delete from hrps.t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and (upper(SS_PNO)=:SS_PNO) and "
            upres.CommandText += "ss_wfl_status in('2','1','0','9') and ss_year=:ss_yar and SS_SRLNO=:SS_SRLNO"

            upres.Connection = conHrps
            upres.Parameters.Clear()
            ' upres.Parameters.AddWithValue("ss_wfl_status1", "2")
            upres.Parameters.AddWithValue("SS_ASSES_PNO", txt_Rasspno.Text.Trim)
            upres.Parameters.AddWithValue("SS_PNO", pnoid)
            ' upres.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            ' upres.Parameters.AddWithValue("ss_wfl_status", s)
            upres.Parameters.AddWithValue("ss_yar", ViewState("FY").ToString())
            upres.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result = upres.ExecuteNonQuery()
            txt_Rasspno.Text = ""
            txt_Rpno.Text = ""
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
    Protected Sub btnTimeline_Click(sender As Object, e As EventArgs)

        Dim ls_sql1 As String = String.Empty
        Dim pnoid As String = String.Empty
        Try

            SessionTimeOut()
            Dim upres As New OracleCommand()
            Dim cmd5 As OracleCommand
            ls_sql1 = "Select * from hrps.t_ir_codes where irc_type ='360PG' and IRC_VALID_TAG='A' AND UPPER(IRC_DESC)=:IRC_DESC"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd5 = New OracleCommand(ls_sql1, conHrps)
            cmd5.Parameters.Clear()
            cmd5.Parameters.AddWithValue("IRC_DESC", ddlTimelinePage.SelectedValue.ToString().ToUpper())
            Dim da5 As New OracleDataAdapter(cmd5)
            Dim dt5 As New DataTable
            da5.Fill(dt5)
            If dt5.Rows.Count > 0 Then
                upres.CommandText = "UPDATE hrps.t_ir_codes SET IRC_START_DT=to_date(:IRC_START_DT,'dd-MM-yyyy'),IRC_END_DT=to_date(:IRC_END_DT,'dd-MM-yyyy')  where  irc_type ='360PG' and IRC_VALID_TAG='A' AND UPPER(IRC_DESC)=:IRC_DESC"
                upres.Connection = conHrps
                upres.Parameters.Clear()
                upres.Parameters.AddWithValue("IRC_START_DT", txtStartDt.Text.Trim)
                upres.Parameters.AddWithValue("IRC_END_DT", txtEndDt.Text.Trim)
                upres.Parameters.AddWithValue("IRC_DESC", ddlTimelinePage.SelectedValue.ToString().ToUpper())
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim result = upres.ExecuteNonQuery()
                If result > 0 Then
                    bindTimelineGdv()
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Timeline page setting successfully updated!")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Timeline page setting can not be updated...!")
                End If
            Else

            End If
            ddlTimelinePage.SelectedIndex = 0
            txtStartDt.Text = ""
            txtEndDt.Text = ""

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        'WI624: End of code, Created By: Avik Mukherjee, created On: 16-06-2021
    End Sub
    Protected Sub btnCycle_Click(sender As Object, e As EventArgs)

        Dim ls_sql1 As String = String.Empty
        Dim pnoid As String = String.Empty
        Try

            SessionTimeOut()
            Dim upres As New OracleCommand()
            Dim cmd5 As OracleCommand

            upres.CommandText = "UPDATE hrps.t_ir_codes SET irc_desc=:irc_desc, IRC_START_DT=sysdate,IRC_END_DT=to_date(:IRC_END_DT,'dd-MM-yyyy')  where  irc_type ='SL360' and IRC_VALID_TAG='Y'"
            upres.Connection = conHrps
            upres.Parameters.Clear()
            upres.Parameters.AddWithValue("irc_desc", txtCycle.Text.Trim)
            upres.Parameters.AddWithValue("IRC_END_DT", "31-12-9999")
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result = upres.ExecuteNonQuery()

            upres.CommandText = "UPDATE hrps.t_ir_codes SET irc_desc=:irc_desc, IRC_START_DT=sysdate,IRC_END_DT=to_date(:IRC_END_DT,'dd-MM-yyyy')  where  irc_type ='360YS' and IRC_VALID_TAG='A'"
            upres.Connection = conHrps
            upres.Parameters.Clear()
            upres.Parameters.AddWithValue("irc_desc", ddlYearSetting.SelectedItem.Text.Trim)
            upres.Parameters.AddWithValue("IRC_END_DT", "31-12-9999")
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result1 = upres.ExecuteNonQuery()

            If result1 > 0 Then
                getCycleTimeGdv()
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Year setting successfully updated!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Year setting can not be updated...!")
            End If
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        'WI624: End of code, Created By: Avik Mukherjee, created On: 16-06-2021
    End Sub
    Protected Sub btnCngAdd_Click(sender As Object, e As EventArgs)
        Try
            Dim cmd As New OracleCommand()
            cmd.CommandText = "select ema_perno, ema_ename,EMA_BHR_PNO,EMA_BHR_NAME from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno"
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", txtCngAssessPerNo.Text.Trim())
            Dim da As New OracleDataAdapter(cmd)
            Dim dtBuhr As New DataTable()
            da.Fill(dtBuhr)
            If dtBuhr.Rows.Count > 0 Then
                If ViewState("TagChangeBUHR") = "1" Then
                    CType(ViewState("ChangeBUHR"), DataTable).Rows.Add(dtBuhr.Rows(0)("ema_perno").ToString, dtBuhr.Rows(0)("ema_ename").ToString, dtBuhr.Rows(0)("EMA_BHR_PNO").ToString, dtBuhr.Rows(0)("EMA_BHR_NAME").ToString)
                Else
                    ViewState("ChangeBUHR") = dtBuhr
                    ViewState("TagChangeBUHR") = "1"
                End If

                gdvChangeBUHR.DataSource = CType(ViewState("ChangeBUHR"), DataTable)
                gdvChangeBUHR.DataBind()
                txtCngAssessPerNo.Text = ""
                txtCngBUHRPerno.Text = ""
            Else
                gdvChangeBUHR.DataSource = Nothing
                gdvChangeBUHR.DataBind()
            End If
        Catch ex As Exception


        End Try
    End Sub
    Protected Sub btnCngConfirmList_Click(sender As Object, e As EventArgs)
        If ViewState("TagChangeBUHR") = "1" Then
            pnlCngPnoScreen.Visible = False
            pnlBuhrChangeScreen.Visible = True
        End If
    End Sub
    Protected Sub btnCngSubmit_Click(sender As Object, e As EventArgs)
        Try
            SessionTimeOut()
            Dim dtFinalBuhr = CType(ViewState("ChangeBUHR"), DataTable)
            Dim BuhrList = ""
            If dtFinalBuhr.Rows.Count > 0 Then
                BuhrList = "'"
                For i As Integer = 0 To dtFinalBuhr.Rows.Count - 1
                    If dtFinalBuhr.Rows.Count = 1 Then
                        BuhrList += dtFinalBuhr.Rows(i)(0).ToString
                    ElseIf dtFinalBuhr.Rows.Count - 1 = i Then
                        BuhrList += dtFinalBuhr.Rows(i)(0).ToString + ""
                    Else
                        BuhrList += dtFinalBuhr.Rows(i)(0).ToString + "','"
                    End If
                Next
                BuhrList += "'"
            End If
            Dim buhrNm As String = ""
            Dim cmd As New OracleCommand()

            cmd.CommandText = "select ema_ename from tips.t_empl_all where ema_perno =:ema_perno"
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", txtCngBUHRPerno.Text.Trim)
            Dim f = getDataInDt(cmd)
            If f.Rows.Count > 0 Then
                buhrNm = f.Rows(0)("ema_ename").ToString
            End If


            Dim upBuhr1 As New OracleCommand()

            upBuhr1.CommandText = "UPDATE hrps.t_emp_master_feedback360 SET EMA_BHR_PNO=:EMA_BHR_PNO, EMA_BHR_NAME=:EMA_BHR_NAME  where  EMA_PERNO in (" + BuhrList + ") and EMA_YEAR=:EMA_YEAR and EMA_CYCLE=:EMA_CYCLE"
            upBuhr1.Connection = conHrps
            upBuhr1.Parameters.Clear()
            upBuhr1.Parameters.AddWithValue("EMA_BHR_PNO", txtCngBUHRPerno.Text.Trim)
            upBuhr1.Parameters.AddWithValue("EMA_BHR_NAME", buhrNm)
            'upBuhr.Parameters.AddWithValue("EMA_PERNO", BuhrList)
            upBuhr1.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString)
            upBuhr1.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString)
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result = upBuhr1.ExecuteNonQuery()

            If result > 0 Then

                ShowGenericMessageModal(CommonConstants.AlertType.success, "BUHR Per. no. change successfully updated!")
                pnlCngPnoScreen.Visible = True
                pnlBuhrChangeScreen.Visible = False
                ViewState("TagChangeBUHR") = ""
                ViewState("ChangeBUHR") = ""
                txtCngBUHRPerno.Text = ""
                txtCngAssessPerNo.Text = ""
                gdvChangeBUHR.DataSource = Nothing
                gdvChangeBUHR.DataBind()
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Not be updated...!")
            End If
        Catch ex As Exception
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub txtCngAppAssesPerNo_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtCngAppAssesPerNo.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select decode(ema_eqv_level,'I2',ema_dotted_pno,ema_reporting_to_pno) EMA_REPORTING_TO_PNO from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " ema_perno =:pno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
            'End by Manoj Kumar on 31-05-2021
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            strself.Parameters.Add(New OracleParameter("SS_YEAR", ViewState("FY").ToString()))
            strself.Parameters.Add(New OracleParameter("SS_SRLNO", ViewState("SRLNO").ToString()))
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
            cmd.Parameters.Add(New OracleParameter("SS_SRLNO", ViewState("SRLNO").ToString))
            cmd.Parameters.Add(New OracleParameter("SS_YEAR", ViewState("FY").ToString))
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

    Protected Sub btnChangeApproval_Click(sender As Object, e As EventArgs)
        Try
            SessionTimeOut()
            Dim chk = checkAssessData(txtCngAppAssesPerNo.Text)
            If chk = "Y" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data submitted by the assesse.")
                Exit Sub
            End If
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand
            comnd.CommandText = "update hrps.t_emp_master_feedback360 set EMA_REPORTING_TO_PNO =:EMA_REPORTING_TO_PNO where ema_perno=:ema_perno and ema_YEAR=:SS_YEAR and ema_cycle=:SS_SRLNO"
            comnd.Connection = conHrps
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.Parameters.AddWithValue("EMA_REPORTING_TO_PNO", txtCngAppPerNO.Text.Trim.ToString)
            comnd.Parameters.AddWithValue("ema_perno", txtCngAppAssesPerNo.Text.Trim.ToString())
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            Dim result = comnd.ExecuteNonQuery()

            comnd.CommandText = "update t_survey_status set SS_APPROVER =:SS_APPROVER, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO=:SS_ASSES_PNO and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO"
            comnd.Connection = conHrps
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.Parameters.AddWithValue("SS_APPROVER", txtCngAppPerNO.Text.Trim.ToString)
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", txtCngAppAssesPerNo.Text.Trim.ToString())
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
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
    Public Function checkreverteligible(ByVal pno As String, srl As String) As String
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
            cmdeligible.Parameters.Add(New OracleParameter("SS_YEAR", ViewState("FY")))
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


    Protected Sub btn_revertapp_Click(sender As Object, e As EventArgs)

        '********************WI447: Button to revert back survey request to approver*********************'
        Try
            SessionTimeOut()
            If txt_revertapp.Text.Trim <> "" Then
                Dim com As New OracleCommand()
                Dim chk = checkreverteligible(txt_revertapp.Text.Trim.ToUpper(), ViewState("SRLNO").ToString())
                If chk = "Y" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot revert the form survey already started")
                    Exit Sub
                End If
                Dim chksurvey = checkreverteligiblesub(txt_revertapp.Text.Trim.ToUpper(), ViewState("SRLNO").ToString())
                If chksurvey = "N" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "You cannot revert the form.It is not approved by manager")
                    Exit Sub
                End If
                Dim qry As String = String.Empty
                qry = "update hrps.t_survey_status set ss_wfl_status='1',ss_app_tag=null,SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "',SS_UPDATED_DT=sysdate where ss_year='" & ViewState("FY").ToString() & "'"
                qry += "  and ss_tag='SU' and upper(ss_asses_pno)='" & txt_revertapp.Text.Trim.ToUpper() & "' and  SS_SRLNO ='" & ViewState("SRLNO").ToString() & "' and ss_app_tag='AP'"
                com = New OracleCommand(qry, conHrps)

                'dtsrl = getRecordInDt(com, conHrps)

                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim r = com.ExecuteNonQuery()

                If r = 0 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Incorrect P.no entred...!")  'WI368 execute query sow correct message'
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Returned...!")
                    btnsearch_click(sender, e)
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter Personal number...!")
                lnkreset_Click(sender, e)
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        '''WI447: end of function to revert back survey to approver
    End Sub

    Public Sub OverAll()
        Try
            ''''' get overall report for super admin: WI:WI300'''''
            Dim str As New OracleCommand()
            str.CommandText = " SELECT ss_asses_pno, ema_ename, ema_empl_sgrade, ema_desgn_desc, ema_exec_head_desc , round(SUM(ss_q1_a) / COUNT(*),2) a, round(SUM(ss_q1_b) / COUNT(*),2) b,"
            str.CommandText += " round(SUM(ss_q1_c) / COUNT(*),2) c, round(SUM(ss_q1_d) / COUNT(*),2) d, COUNT(*) no_records, ( CASE WHEN round(SUM(ss_q1_a) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += " WHEN round(SUM(ss_q1_a) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_a) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_a) / COUNT(*),2) <= '3' "
            str.CommandText += " AND round(SUM(ss_q1_a) / COUNT(*),2) >= '2.5' THEN 'G' END ) r1, ( CASE WHEN round(SUM(ss_q1_b) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += "WHEN round(SUM(ss_q1_b) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_b) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_b) / COUNT(*),2) <= '3' "
            str.CommandText += "AND round(SUM(ss_q1_b) / COUNT(*),2) >= '2.5' THEN 'G' END ) r2, ( CASE WHEN round(SUM(ss_q1_c) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += " WHEN round(SUM(ss_q1_c) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_c) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_c) / COUNT(*),2) <= '3' "
            str.CommandText += " AND round(SUM(ss_q1_c) / COUNT(*),2) >= '2.5' THEN 'G' END ) r3, ( CASE WHEN round(SUM(ss_q1_d) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += " WHEN round(SUM(ss_q1_d) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_d) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_d) / COUNT(*),2) <= '3' "
            str.CommandText += " AND round(SUM(ss_q1_d) / COUNT(*),2) >= '2.5' THEN 'G' END ) r4,(( CASE WHEN round(SUM(ss_q1_a) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += " WHEN round(SUM(ss_q1_a) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_a) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_a) / COUNT(*),2) <= '3' "
            str.CommandText += " AND round(SUM(ss_q1_a) / COUNT(*),2) >= '2.5' THEN 'G' END ) ||( CASE WHEN round(SUM(ss_q1_b) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += " WHEN round(SUM(ss_q1_b) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_b) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_b) / COUNT(*),2) <= '3' "
            str.CommandText += " AND round(SUM(ss_q1_b) / COUNT(*),2) >= '2.5' THEN 'G' END ) || ( CASE WHEN round(SUM(ss_q1_c) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += " WHEN round(SUM(ss_q1_c) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_c) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_c) / COUNT(*),2) <= '3' "
            str.CommandText += " AND round(SUM(ss_q1_c) / COUNT(*),2) >= '2.5' THEN 'G' END ) || ( CASE WHEN round(SUM(ss_q1_d) / COUNT(*),2) < '1.5' THEN 'U' "
            str.CommandText += " WHEN round(SUM(ss_q1_d) / COUNT(*),2) < '2.5' AND round(SUM(ss_q1_d) / COUNT(*),2) >= '1.5' THEN 'A' WHEN round(SUM(ss_q1_d) / COUNT(*),2) <= '3' "
            str.CommandText += " AND round(SUM(ss_q1_d) / COUNT(*),2) >= '2.5' THEN 'G' END )) All1 FROM hrps.t_survey_status, hrps.t_emp_master_feedback360 WHERE  ema_perno = ss_asses_pno and ss_wfl_status = '3' "
            str.CommandText += " AND ss_year = '" & ViewState("FY").ToString & "' AND SS_SRLNO= '" & ViewState("SRLNO").ToString & "' AND ss_status = 'SE' AND ss_del_tag = 'N'    AND ss_app_tag = 'AP' AND upper(ss_categ) <> 'SELF' AND  EMA_EQV_LEVEL in('I3','I4','I5','I6')"
            str.CommandText += "  GROUP BY ss_asses_pno, ema_ename, ema_eqv_level, ema_empl_sgrade, ema_desgn_desc, ema_exec_head_desc ORDER BY 1, 2 "
            Dim dt = getRecordInDt(str, conHrps)


            If dt.Rows.Count > 0 Then
                gvoverall.DataSource = Nothing
                gvoverall.DataBind()
                gvoverall.DataSource = dt
                gvoverall.DataBind()
                ExportToExcel(gvoverall, "OverAll")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data found")
                gvoverall.DataSource = Nothing
                gvoverall.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

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

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
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
            'MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function

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


    Protected Sub Button4_Click(sender As Object, e As EventArgs)
        Try

            'If rblevel.SelectedValue = "L3" Then
            '    Response.Redirect("FeedbackSurveyRpt_OPR.aspx?pno=" & TextBox1.Text)
            'ElseIf rblevel.SelectedValue = "L4" Then
            '    Response.Redirect("Rpt_OPR.aspx?pno=" & TextBox1.Text)
            'End If
        Catch ex As Exception

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

    Public Sub ovralll4(pno As String, yr As String, cyc As String)
        Try

            If pno = "" Then
                Dim qry As String = ""
                If cyc <> "1" Then
                    qry = "Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno = se.ema_cycle and and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='MANGR' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I4','I5','I6')  Group by ss_asses_pno,ss_categ HAVING count(*)>=1 INTERSECT Select ss_asses_pno from t_survey_status ss ,hrps.t_emp_master_feedback360 se where ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno = se.ema_cycle and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='INTSH' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I4','I5','I6')  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 order by ss_asses_pno"
                Else
                    qry = "Select ss_asses_pno from t_survey_status ss ,TIPS.t_empl_all se where  ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='MANGR' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I4','I5','I6')  Group by ss_asses_pno,ss_categ HAVING count(*)>=1 INTERSECT Select ss_asses_pno from t_survey_status ss ,TIPS.t_empl_all se where ss.ss_asses_pno=se.ema_perno and ss_wfl_status ='3' and ss_year='" & yr.ToString() & "' AND SS_SRLNO='" & cyc.ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') and upper(ss_categ)='INTSH' And ss_app_tag = 'AP' and se.EMA_EQV_LEVEL in ('I4','I5','I6')  Group by ss_asses_pno,ss_categ HAVING count(*)>=3 order by ss_asses_pno"
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
                        pernoList6000 = ""
                        pernoList7000 = ""
                        pernoList8000 = ""
                        pernoList9000 = ""
                        pernoList10000 = ""

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

                        '6000 rows
                        If dtIl3.Rows.Count > 5000 Then
                            If dtIl3.Rows.Count < 6000 Then
                                For i As Integer = 5000 To dtIl3.Rows.Count - 1
                                    pernoList6000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 5000 To 5999
                                    pernoList6000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList6000 = pernoList6000.TrimEnd(",")
                            End If
                        End If

                        '7000 rows
                        If dtIl3.Rows.Count > 6000 Then
                            If dtIl3.Rows.Count < 7000 Then
                                For i As Integer = 6000 To dtIl3.Rows.Count - 1
                                    pernoList7000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 6000 To 6999
                                    pernoList7000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList7000 = pernoList7000.TrimEnd(",")
                            End If
                        End If

                        '8000 rows
                        If dtIl3.Rows.Count > 7000 Then
                            If dtIl3.Rows.Count < 8000 Then
                                For i As Integer = 7000 To dtIl3.Rows.Count - 1
                                    pernoList8000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 7000 To 7999
                                    pernoList8000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList8000 = pernoList8000.TrimEnd(",")
                            End If
                        End If

                        '9000 rows
                        If dtIl3.Rows.Count > 8000 Then
                            If dtIl3.Rows.Count < 9000 Then
                                For i As Integer = 8000 To dtIl3.Rows.Count - 1
                                    pernoList9000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 8000 To 8999
                                    pernoList9000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList9000 = pernoList9000.TrimEnd(",")
                            End If
                        End If

                        '10000 rows
                        If dtIl3.Rows.Count > 9000 Then
                            If dtIl3.Rows.Count < 10000 Then
                                For i As Integer = 9000 To dtIl3.Rows.Count - 1
                                    pernoList10000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            Else
                                For i As Integer = 9000 To 9999
                                    pernoList10000 += "'" + dtIl3.Rows(i)("ss_asses_pno").ToString() + "',"
                                Next
                            End If

                            If dtIl3.Rows.Count > 1 Then
                                pernoList10000 = pernoList10000.TrimEnd(",")
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
                Str += " Select * from hrps.V_ALL_I3_I6_T_CYC1  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " And") & " "
                'Str += " select * from hrps.V_OVERALL WHERE ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " And") & " "
                'Str += "     union"
                'Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                'Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                'Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                'Str += "     FROM HRPS.V_OVERALL  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "" & pno.Replace("where", " and") & " GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                Dim cmd As New OracleCommand()
                cmd.CommandText = Str
                Dim ds = getRecordInDt(cmd, conHrps)

                If ds.Rows.Count > 0 Then
                    GridView4.DataSource = ds
                    GridView4.DataBind()
                    ExportToExcel(GridView4, "OverAll")
                End If
            Else
                Dim ds As DataTable
                If pernoList1000 <> "" Then
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList1000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList1000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt1000 = getRecordInDt(cmd, conHrps)
                    ds = dt1000

                End If

                If pernoList2000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList2000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList2000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt2000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt2000)
                End If

                If pernoList3000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList3000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList3000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt3000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt3000)
                End If

                If pernoList4000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList4000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList4000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt4000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt4000)
                End If

                If pernoList5000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList5000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList5000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt5000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt5000)
                End If

                If pernoList6000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList6000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList6000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt6000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt6000)
                End If

                If pernoList7000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList7000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList7000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt7000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt7000)
                End If

                If pernoList8000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList8000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList8000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt8000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt8000)
                End If

                If pernoList9000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList9000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList9000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt9000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt9000)
                End If

                If pernoList10000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_OVERALL_WOTAG where ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList10000 + ")"
                    Str += "     union"
                    Str += "     select DISTINCT EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,'Z-OVERALL' SS_CATEG,SS_YEAR,SS_SRLNO,round(AVG(A),2) A,round(AVG(C),2) C,round(AVG(R),2) R,round(AVG(T),2) T,"
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) AC,"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) COL,"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) RES,"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END) TEAM, "
                    Str += "     (CASE WHEN AVG(A)<=1.6 THEN 'U' WHEN AVG(A)>1.6 AND AVG(A)<=2.6 THEN 'A' WHEN AVG(A)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(C)<=1.6 THEN 'U' WHEN AVG(C)>1.6 AND AVG(C)<=2.6 THEN 'A' WHEN AVG(C)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(R)<=1.6 THEN 'U' WHEN AVG(R)>1.6 AND AVG(R)<=2.6 THEN 'A' WHEN AVG(R)>2.6 THEN 'G' END) ||"
                    Str += "     (CASE WHEN AVG(T)<=1.6 THEN 'U' WHEN AVG(T)>1.6 AND AVG(T)<=2.6 THEN 'A' WHEN AVG(T)>2.6 THEN 'G' END)  COMBINATION"
                    Str += "     FROM HRPS.V_OVERALL_WOTAG  where ss_categ <>'Self' and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " and EMA_PERNO in (" + pernoList10000 + ") GROUP By EMA_PERNO,EMA_ENAME,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_EXEC_HEAD_DESC,EMA_BHR_PNO,EMA_BHR_NAME,SS_YEAR,SS_SRLNO ORDER BY 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt10000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt10000)
                End If


                If ds.Rows.Count > 0 Then
                    GridView4.DataSource = ds
                    GridView4.DataBind()
                    ExportToExcel(GridView4, "OverAll")
                End If

            End If



        Catch ex As Exception
        End Try
    End Sub
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
                    qry += "INTERSECT Select SS.ss_asses_pno from hrps.t_survey_status ss ,hrps.t_emp_master_feedback360 se where  ss.ss_asses_pno=se.ema_perno and ss.ss_year=se.ema_year and ss.ss_srlno= se.ema_cycle and ss.ss_wfl_status ='3' and ss.ss_year='" & yr.ToString & "' AND ss.ss_srlno='" & cyc.ToString & "' and ss_categ='PEER' AND se.ema_comp_code='1000' Group by ss.ss_asses_pno,se.ema_eqv_level HAVING count(*)>=(select substr(ic.irc_desc,1,1) from hrps.t_ir_codes ic where ic.irc_type='" & yrCyc & "TG' and ic.irc_valid_tag='A' and ic.irc_code='PEER' and se.ema_eqv_level='TG')"

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
                Dim ds As DataTable
                ds = Nothing
                If pernoList1000 <> "" Then
                    Str += " Select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
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
                    Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt2000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt2000)
                End If

                If pernoList3000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt3000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt3000)
                End If

                If pernoList4000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt4000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt4000)
                End If

                If pernoList5000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"


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
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Generate pdf report for this perno to get overall report.")
                    Exit Sub
                End If
            Else
                Dim ds As DataTable
                ds = Nothing
                If pernoList1000 <> "" Then
                    Str += " Select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList1000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"

                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt1000 = getRecordInDt(cmd, conHrps)
                    ds = dt1000
                End If

                If pernoList2000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList2000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"
                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt2000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt2000)
                End If

                If pernoList3000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList3000 + ") and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & " order by 1"


                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt3000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt3000)
                End If

                If pernoList4000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList4000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"


                    Dim cmd As New OracleCommand()
                    cmd.CommandText = Str
                    Dim dt4000 = getRecordInDt(cmd, conHrps)

                    ds.Merge(dt4000)
                End If

                If pernoList5000 <> "" Then
                    Str = ""
                    Str += " select * from hrps.V_ALL_I1_I6_WT  where EMA_PERNO in (" + pernoList5000 + ")  and ss_year =" & txtOyear.Text.Trim().ToString & " and ss_srlno =" & txtOCycle.Text.Trim().ToString & "  order by 1"

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
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "No record found.")
                    Exit Sub
                End If

            End If




        Catch ex As Exception
            MsgBox(ex.Message.ToString())
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

                    Dim qryMiniCriteria As New OracleCommand()
                    qryMiniCriteria.CommandText = "select irc_code,irc_desc from t_ir_codes where irc_type='" & txtFyear.Text.Substring(2, 2).ToString() & "" & txtFcycle.Text.Trim().ToString & "" & gh.Rows(0)(1) & "' and irc_valid_tag='A' order by irc_code"
                    Dim dtqryMiniCriteria = getRecordInDt(qryMiniCriteria, conHrps)
                    If dtqryMiniCriteria.Rows.Count > 0 Then

                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Cycle " & txtFcycle.Text & " is not active for this report.")
                        Exit Sub
                    End If

                    If gh.Rows(0)(1) = "I3" Or gh.Rows(0)(1) = "I2" Or gh.Rows(0)(1) = "I1" Then
                        If checkcateg(TextBox2.Text, gh.Rows(0)(1)) Then
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
                    ElseIf gh.Rows(0)(1) = "TG" Then
                        If checkcateg(TextBox2.Text, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                Response.Redirect("FeedbackSurveyRptTG_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                            Else
                                updateTag(TextBox2.Text)
                                Response.Redirect("FeedbackSurveyRptTG_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                            End If

                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "The report could not be downloaded because the minimum criteria was not met.")
                        End If
                    ElseIf gh.Rows(0)(1) = "I4" Or gh.Rows(0)(1) = "I5" Or gh.Rows(0)(1) = "I6" Then
                        If checkcateg(TextBox2.Text, gh.Rows(0)(1)) Then
                            If Not f1 Is Nothing And f1.Rows.Count > 0 Then
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                            Else
                                updateTag(TextBox2.Text)
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
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
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
                            Else
                                updateTag(TextBox2.Text)
                                Response.Redirect("Rpt_OPR.aspx?pno=" & SimpleCrypt(TextBox2.Text) & "&yr=" & SimpleCrypt(txtFyear.Text) & "&cyc=" & SimpleCrypt(txtFcycle.Text))
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
    Public Function checkcateg(pno As String, lvl As String) As Boolean
        Dim falg As Boolean = False
        Try
            Dim cntMinFullFill As Integer = 2
            Dim qry As String = String.Empty
            qry = "Select count(*) No_Records,upper(ss_categ) categ from t_survey_status where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and ss_year='" & txtFyear.Text.Trim().ToString() & "' and ss_srlno='" & txtFcycle.Text.Trim().ToString & "' and ss_status = 'SE' and ss_del_tag = 'N' and upper(ss_categ) NOT in ('SELF') And ss_app_tag = 'AP'  Group by ss_categ"
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
End Class
