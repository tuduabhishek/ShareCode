Imports System.Data.OracleClient
Imports System.Data
Partial Class ApprovalRespondent
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "APPROVE RESPONDENT"
                getFy()
                getsrlno()
                hdfSession.Value = Session("ADM_USER").ToString
                hdfYear.Value = ViewState("FY").ToString
                hdfCycle.Value = ViewState("SRLNO").ToString
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
                bindpendinggrid()
                GridView1.Visible = True
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
                qrypending.CommandText = "select tab.SS_ASSES_PNO,tab.ema_ename,tab.ema_desgn_desc,nvl((select D.EMA_ENAME||'('|| D.EMA_PERNO ||')' from hrps.t_emp_master_feedback360 c,hrps.t_emp_master_feedback360 d  "
                'Commented and added by TCS on 02122022, Changing query to show manager (reporting perno column)
                'qrypending.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO   and c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                qrypending.CommandText += " where c.ema_reporting_to_pno=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO   and c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                'End
                qrypending.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6')  and a.ema_comp_code='1000' "
                qrypending.CommandText += " and ss_status='SE' and ss_tag='SU' and ss_year=:fy and (ss_del_tag='N' or ss_del_tag is null) and  SS_SRLNO=:SS_SRLNO and A.EMA_BHR_PNO=:ema_perno and ss_app_tag is null"
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
                qrypending.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.ema_year=ss_year and a.ema_cycle=ss_srlno  and a.EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6') "
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
                'Commented and added by TCS on 30112022, Changing query to show manager (reporting perno column)
                'qrypending.CommandText += " where decode(c.ema_eqv_level,'I2',c.ema_dotted_pno,c.ema_reporting_to_pno)=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO  and c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                qrypending.CommandText += " where c.ema_reporting_to_pno=D.EMA_PERNO AND c.ema_perno=tab.SS_ASSES_PNO  and c.ema_year=:fy and c.ema_cycle=:SS_SRLNO AND ROWNUM=1 ),'') approver from (select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc from "
                'End
                qrypending.CommandText += " hrps.t_survey_status, hrps.t_emp_master_feedback360 a    where a.ema_perno = SS_ASSES_PNO and a.ema_year=ss_year and a.ema_cycle=ss_srlno and a.EMA_EQV_LEVEL in('I3','I4','I5','I6')   and a.ema_comp_code='1000' "
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
    Public Sub showConfirmMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showConfirmMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
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
End Class
