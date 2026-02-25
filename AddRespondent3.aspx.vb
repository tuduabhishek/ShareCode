Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Net.Mail
Imports ClosedXML.Excel
Partial Class AddRespondent3
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "ADD/REMOVE/SUBMIT RESPONDENT"
                getFy()
                getsrlno()
                bindExecHead()
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
            query.CommandText = "select distinct ema_exec_head,ema_exec_head_desc from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head_desc<>'Not found' and ema_exec_head<>'00000000' and ema_comp_code='1000' and ema_year=:ema_year and ema_cycle=:ema_cycle"
            query.Parameters.Clear()
            If ChkRole1() = False Then

                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())

            End If
            query.CommandText += " order by ema_exec_head_desc"

            query.Parameters.AddWithValue("ema_year", ViewState("FY").ToString())
            query.Parameters.AddWithValue("ema_cycle", ViewState("SRLNO").ToString())
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
    Public Sub bindDepartment()
        Try
            Dim dtDept As New DataTable
            Dim query As New OracleCommand
            ' Start WI368  by Manoj Kumar on 30-05-2021
            query.Parameters.Clear()      'WI368 Department not show When executive head select.
            query.Connection = conHrps      'WI368 Department not show When executive head select.
            'End by Manoj Kumar on 30-05-2021
            ddlDept.Items.Clear()
            query.CommandText = "select distinct ema_dept_code DEPT,ema_dept_desc DEPTDESC from hrps.t_emp_master_feedback360 where ema_comp_code='1000'  and ema_dept_desc<>'Not found' and ema_exec_head = :ema_exec_head and ema_year=:ema_year and ema_cycle=:ema_cycle"
            If ChkRole1() = False Then
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())
            End If
            query.CommandText += " order by ema_dept_desc"
            query.Parameters.AddWithValue("ema_exec_head", ddlExecutive.SelectedValue)
            query.Parameters.AddWithValue("ema_year", ViewState("FY").ToString())
            query.Parameters.AddWithValue("ema_cycle", ViewState("SRLNO").ToString())

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

    Protected Sub ddlExecutive_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlExecutive.SelectedIndexChanged
        bindDepartment()
    End Sub
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
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Protected Sub btnsearch_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        If ddlExecutive.SelectedValue = "--Select--" And txtperno1.Text.Trim = "" And txtBuhr.Text.Trim = "" Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select option for search result")
            Exit Sub
        Else
            bindGrid()
        End If
    End Sub
    Public Sub bindGrid()

        Try
            If ChkRole() Then



                ''''' BUHR can only able to view records of their departments/ Super admin can see overall record: WI:WI300'''''
                Dim qryrespond As New OracleCommand
                qryrespond.Connection = conHrps
                qryrespond.Parameters.Clear()
                If ChkRole1() Then

                    qryrespond.CommandText = "select tab.SS_ASSES_PNO,tab.ema_ename,tab.ema_desgn_desc,nvl((select D.EMA_ENAME||'('|| D.EMA_PERNO ||')' from hrps.t_emp_master_feedback360 c,hrps.t_emp_master_feedback360 d  "
                    'Commented and added by TCS on 02122022, Changing query to show manager (reporting perno column)
                    'qryrespond.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO and c.ema_year=d.ema_year and c.ema_cycle=d.ema_cycle  and c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                    qryrespond.CommandText += " where c.ema_reporting_to_pno=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO and c.ema_year=d.ema_year and c.ema_cycle=d.ema_cycle  and c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                    'End
                    qryrespond.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.ema_year=ss_year and a.ema_cycle=ss_srlno  and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6') and a.ema_comp_code='1000' "
                    qryrespond.CommandText += " and ss_status='SE'  and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and A.EMA_BHR_PNO=:ema_perno " ' and SS_ASSES_PNO='" & txtperno1.Text.Trim & "'"
                    qryrespond.CommandText += " and SS_WFL_STATUS in ('2', '3', '9')  and  upper(SS_APP_TAG)='AP' "

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
                    qryrespond.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.ema_year=ss_year and a.ema_cycle=ss_srlno    and a.ema_comp_code='1000'  and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6') "
                    qryrespond.CommandText += " and ss_status='SE'  and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and a.ema_perno<>:ema_perno" ' and SS_ASSES_PNO='" & txtperno1.Text.Trim & "') tab"
                    qryrespond.CommandText += " and SS_WFL_STATUS in ('2', '3', '9')  and  upper(SS_APP_TAG)='AP' "
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
                    'Commented and added by TCS on 02122022, Changing query to show manager (reporting perno column)
                    'qryrespond.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO and  c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                    qryrespond.CommandText += " where c.ema_reporting_to_pno=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO and  c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                    'End
                    qryrespond.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.ema_year=ss_year and a.ema_cycle=ss_srlno and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6')  "
                    qryrespond.CommandText += " and ss_status='SE'  and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and A.EMA_BHR_PNO=:ema_perno" 'and SS_ASSES_PNO='" & txtperno1.Text.Trim & "') tab"
                    qryrespond.CommandText += " and SS_WFL_STATUS in ('2', '3', '9')  and  upper(SS_APP_TAG)='AP' "
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

        Catch ex As Exception

        End Try
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
                    If dt.Rows(d)("maximum") = "NA" Then
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
                GridView2.Visible = False
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                btnsubmit.Visible = False
                gvfinal.Visible = True
            ElseIf b.Text = "Returned" Then
                GridView2.Visible = False
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                btnsubmit.Visible = True
                gvfinal.Visible = True
            ElseIf b.Text = "Submitted" Then
                GridView2.Visible = True
                gvfinal.Visible = False
                btnaddpeertsl.Visible = False
                btnaddnontsl.Visible = False
                btnsubmit.Visible = False
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
            str.CommandText += "SS_CATEG,SS_DEL_TAG,SS_TAG,ss_app_tag from hrps.t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and SS_STATUS='SE' and ss_app_tag='AP' and ss_year=:fy and SS_SRLNO=:SS_SRLNO order by Category"
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
            DrNOrg = dt.Select("ss_app_tag='AP'") ''

            For Each row1 As DataRow In DrNOrg
                vl = "S"
            Next

            If dt.Rows.Count > 0 Then
                If vl = "S" Then
                    'gvfinal.DataSource = dt
                    'gvfinal.DataBind()
                    'gvfinal.Columns(7).Visible = False
                    'btnaddnontsl.Visible = False
                    'btnaddpeertsl.Visible = False
                    'btnsubmit.Visible = False
                    'gvfinal.Visible = True
                    ''  lbOrg.Visible = True
                    'divtitle.Visible = True

                    gvfinal.DataSource = dt
                    gvfinal.DataBind()
                    gvfinal.Columns(7).Visible = True
                    btnaddnontsl.Visible = True
                    btnaddpeertsl.Visible = True
                    'btnsubmit.Visible = True
                    gvfinal.Visible = True
                    '  lbOrg.Visible = True
                    divtitle.Visible = True
                Else
                    gvfinal.DataSource = dt
                    gvfinal.DataBind()
                    gvfinal.Columns(7).Visible = False
                    btnaddnontsl.Visible = False
                    btnaddpeertsl.Visible = False
                    btnsubmit.Visible = False
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



            'Dim str1 As New OracleCommand
            'If Session("label").Equals("I2") Then
            '    str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            'ElseIf Session("label").Equals("I3") Then
            '    str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            'Else
            '    str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Peer/Subordinate/Internal Stakeholder',SS_CATEG) Category,"
            'End If
            'str1.CommandText += "SS_CATEG,SS_DEL_TAG from hrps.t_survey_status where ss_asses_pno=:SS_ASSES_PNO"
            ''qry1.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and ss_del_tag='N' and ss_tag='SU' and SS_STATUS='SE' order by Category"
            'str1.CommandText += " and ss_year=:fy and ss_del_tag='N' and SS_STATUS='SE' and SS_SRLNO=:SS_SRLNO order by Category"
            'str1.Connection = conHrps
            'str1.Parameters.Clear()
            'str1.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            'str1.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            'str1.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            ''Dim qry1 = New OracleCommand(str1, conHrps)
            'Dim dth = getDataInDt(str1)

            'If dth.Rows.Count > 0 Then
            '    GridView2.DataSource = dth
            '    GridView2.DataBind()
            'Else
            '    GridView2.DataSource = Nothing
            '    GridView2.DataBind()
            'End If

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
                If dt1.Rows(d)("maximum") = "NA" Then
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
    Protected Sub GvCateg_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim chk = CType(e.Row.FindControl("chkmgr"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)
            Dim delTag = CType(e.Row.FindControl("lblDelTag"), Label)

            If delTag.Text.ToString = "N" Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If

        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try

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
                If IsValidforRemoval(id.Text, "Y", catgry.Text) Then
                    UpdateData(id.Text, "Y", catgry.Text)
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Sorry you cannot remove selected respondent, because respondent already submitted the response.")
                    Exit Sub
                End If
            End If
            BindAssesorGrid(aperno.Text)
        End If
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

            '''''''''get approver details'''''
            Dim approver As String = GetApprover(ViewState("FY").ToString(), pno, ViewState("SRLNO").ToString())
            '''''''''''''''''''''''''

            OrgStr = "insert into hrps.T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL,SS_ADM_TAG,SS_APP_TAG,SS_APPROVER,SS_WFL_STATUS,SS_SRLNO,SS_TAG_DT,SS_ADM_UPD_DT) values (:SS_CATEG, :SS_ID,:SS_PNO,:SS_NAME,:SS_DESG,:SS_DEPT,:SS_EMAIL,:SS_STATUS,:SS_TAG, "
            OrgStr += ":SS_CRT_BY, sysdate,:SS_DEL_TAG,:SS_TYPE,:ss_year,:SS_ASSES_PNO,:SS_LEVEL,'1','AP',:SS_APPROVER,'2',:SS_SRLNO,sysdate,sysdate)"

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
            comnd.Parameters.AddWithValue("SS_TAG", "SU")
            comnd.Parameters.AddWithValue("SS_CRT_BY", Session("ADM_USER").ToString())
            comnd.Parameters.AddWithValue("SS_DEL_TAG", "N")
            comnd.Parameters.AddWithValue("SS_TYPE", orgtype.ToString())
            comnd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", assespno.ToString)
            comnd.Parameters.AddWithValue("SS_LEVEL", lvl.ToString)
            comnd.Parameters.AddWithValue("SS_APPROVER", approver)
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
    Public Sub SessionTimeOut()
        If Session("ADM_USER") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Exit Sub
        Else



        End If
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

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New AddRespondent3
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select distinct ema_ename ||'('|| ema_perno ||')' EName from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and (ema_perno like '%" & prefixText & "%' or upper(ema_ename) like "
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
    Public Sub UpdateData(id As String, tag As String, catg As String)
        Try
            Dim query As String = String.Empty
            If tag = "Y" Then
                query = "delete from hrps.t_survey_status "  'Add by Manoj Kumar(osj1874) 18-01-2021
                query += "  where SS_PNO=:SS_PNO and ss_year=:fy"
                query += "  and SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG and nvl(SS_APP_TAG,'N') = 'AP' and SS_SRLNO=:SS_SRLNO"
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


                    If dt.Rows(i)("maximum") = "NA" Then
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
                'Commented and added by TCS on 02122022, Changing query to fix manager selection (reporting perno column)
                'comd.CommandText = " select nvl(EMA_DOTTED_PNO,'NA') EMA_REPORTING_TO_PNO"
                comd.CommandText = " select nvl(EMA_REPORTING_TO_PNO,'NA') EMA_REPORTING_TO_PNO"
                'End
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


                    If dt.Rows(i)("maximum") = "NA" Then
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
    Private Function GetApprover(ByVal SS_YEAR As String, ByVal SS_ASSES_PNO As String, ByVal SS_SRLNO As String) As String
        Dim res As String = ""
        Try
            Dim query As String
            query = "select distinct SS_APPROVER from T_SURVEY_STATUS where SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO And SS_SRLNO=:SS_SRLNO"
            Dim cmd As New OracleCommand(query, conHrps)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim dt As New DataTable
            Dim adp As New OracleDataAdapter(cmd)
            adp.Fill(dt)
            If dt.Rows.Count > 0 Then
                res = dt.Rows(0)(0).ToString
            End If
        Catch ex As Exception

        End Try
        Return res
    End Function

    Public Function IsValidforRemoval(id As String, tag As String, catg As String) As Boolean
        Dim isValid As Boolean = True
        Try
            Dim dt As New DataTable
            Dim query As String = String.Empty
            query = " SELECT * FROM T_SURVEY_STATUS WHERE SS_YEAR=:YEAR AND SS_SRLNO=:CYCLE AND SS_PNO=:SS_PNO AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG and nvl(SS_APP_TAG,'N') = 'AP' AND SS_WFL_STATUS='3'"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_PNO", id)
            comnd.Parameters.AddWithValue("YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString)
            comnd.Parameters.AddWithValue("SS_CATEG", catg)
            comnd.Parameters.AddWithValue("CYCLE", ViewState("SRLNO").ToString())

            Dim da As New OracleDataAdapter(comnd)
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                isValid = False
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return isValid
    End Function
End Class
