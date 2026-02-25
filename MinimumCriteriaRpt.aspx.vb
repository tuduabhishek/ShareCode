Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
''' <summary>
''' WI1861: display approved records only and also change in logic to check equivalent level and provide access to TCS team as super admin
''' created By: Avik Mukherjee
''' Created On: 21-07-2021
''' </summary>
Partial Class MinimumCriteriaRpt
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Public Sub SessionTimeOut()
        If Session("ADM_USER") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
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
        getFy()
        getsrlno()
        'strUserID = "197838"
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
            'strUserID = "148536"
            Session("ADM_USER") = strUserID.ToUpper()
            If GetPno(strUserID.ToUpper()) = False Then
                Session("errorMsg") = "You don't have admin role."
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
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Private Sub MinimumCriteriaRpt_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
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
    Private Sub MinimumCriteriaRpt_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            SessionTimeOut()
            If Not IsPostBack Then

                bindExecHead()
                btn_download.Enabled = False
                'bindDepartment()
                'bindGrid()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bindExecHead()
        Try
            Dim dtExecHead As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct ema_exec_head,ema_exec_head_desc from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head_desc<>'Not found' and ema_exec_head<>'00000000' and ema_comp_code='1000' and ema_year=:ema_year and ema_cycle=:ema_cycle"
            query.Parameters.Clear()
            If ChkBUHRRole() = True And ChkRole() = False Then
                query.CommandText += " and ema_bhr_pno=:buhr"
                query.Parameters.AddWithValue("buhr", Session("ADM_USER").ToString())
            End If
            If ChkBUHRRole() = True And ChkRole() = True Then
                query.CommandText += " "
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
    Protected Sub ddlExecutive_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlExecutive.SelectedIndexChanged
        bindDepartment()
    End Sub
    Public Sub bindDepartment()
        Try
            Dim dtDept As New DataTable
            Dim query As New OracleCommand
            query.Parameters.Clear()
            query.Connection = conHrps
            ddlDept.Items.Clear()
            query.CommandText = "select distinct ema_dept_code DEPT,ema_dept_desc DEPTDESC from hrps.t_emp_master_feedback360 where ema_comp_code='1000'  and ema_dept_desc<>'Not found' and ema_exec_head = :ema_exec_head and ema_year=:ema_year and ema_cycle=:ema_cycle"
            If ChkBUHRRole() Then
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
    Public Sub bindGrid()

        Try
            If ChkRole() = True Or ChkBUHRRole() = True Then

                'If Request.QueryString("adm") = "1" Then
                bindMinimumCriteria()
                'End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btn_Show_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        If txtYear.Text.Trim().ToString = "" And txtCycle.Text.Trim().ToString = "" Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill year and cycle field.")
            Exit Sub
        ElseIf ddlExecutive.SelectedValue = "--Select--" And txtpnosub.Text.Trim = "" And txtBuhr.Text.Trim = "" Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select option for serach result")
            Exit Sub
        Else
            btn_download.Enabled = True
            bindGrid()
        End If

    End Sub
    Protected Sub btn_download_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_download.Click
        'Commented & Added by TCS on 13012024, Commented Old Code & Added new Excel download code
        Try
            ExportExcel()
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Some error occured while downloading the report.")
        End Try
        'Response.Clear()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", "attachment;filename=Minimum Criteria.xls")
        'Response.Charset = ""
        'Response.ContentType = "application/vnd.ms-excel"
        'Using sw As New StringWriter()
        '    Dim hw As New HtmlTextWriter(sw)
        '    gdvMiniCriteria.RenderControl(hw)
        '    Response.Write(sw.ToString())
        '    Response.End()
        '    If gdvMiniCriteria.Visible = True Then
        '        'To Export all pages
        '        gdvMiniCriteria.AllowPaging = False
        '        ''Me.BindGrid()

        '        gdvMiniCriteria.HeaderRow.BackColor = Color.LightGray
        '        For Each cell As TableCell In gdvMiniCriteria.HeaderRow.Cells
        '            cell.BackColor = gdvMiniCriteria.HeaderStyle.BackColor
        '        Next
        '        For Each row As GridViewRow In gdvMiniCriteria.Rows
        '            row.BackColor = Color.White
        '            'For Each cell As TableCell In row.Cells
        '            '    If row.RowIndex Mod 2 = 0 Then
        '            '        cell.BackColor = grdMIS.AlternatingRowStyle.BackColor
        '            '    Else
        '            '        cell.BackColor = grdMIS.RowStyle.BackColor
        '            '    End If
        '            '    cell.CssClass = "textmode"
        '            'Next
        '        Next

        '        gdvMiniCriteria.RenderControl(hw)
        '    End If


        '    'style to format numbers to string
        '    Dim style As String = "<style> .textmode { } </style>"
        '    Response.Write(style)
        '    Response.Output.Write(sw.ToString())
        '    Response.Flush()
        '    Response.[End]()
        'End Using
    End Sub


    Protected Sub btn_download_all_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_download_all.Click
        Dim executiveCode, deptCode, buhrPno, officerPno, officerCatg As String
        executiveCode = ""
        deptCode = ""
        buhrPno = ""
        officerPno = ""
        officerCatg = ""
        ddlExecutive.SelectedValue = "--Select--"
        ddlDept.SelectedValue = "--Select--"
        txtBuhr.Text = ""
        txtpnosub.Text = ""
        If txtYear.Text.Trim().Length <> 4 And txtCycle.Text.Trim().Length <> 1 Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill year and cycle.")
            Exit Sub
        Else
            bindMinimumCriteria()
            btn_download_click(sender, e)
        End If

        'Try
        '    Dim qry1 As New OracleCommand()
        '    Dim dt2 As New DataTable()
        '    If conHrps.State = ConnectionState.Closed Then
        '        conHrps.Open()
        '    End If
        '    qry1.CommandText = " select ss.ss_asses_pno,ea.ema_ename,decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'People you lead') ss_categ1,ea.ema_bhr_pno,ea.ema_bhr_name ,SUBSTR(ic.IRC_DESC,1,1) minmum,count ( case when ss_wfl_status = '2' then 'Pending' end ) Pending, "
        '    qry1.CommandText += "decode(ic.irc_type,'360V2',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),'360V3',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peer/Subordinate',upper('intsh'),'Internal stakeholders'),decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('intsh'),'Peer/Subordinate/Internal stakeholders')) ss_categ,"
        '    qry1.CommandText += " count ( case when ss_wfl_status = '3' then 'Completed' end ) Completed,"
        '    qry1.CommandText += "  case when count ( case when ss_wfl_status = '3' then 'Completed' end )>=SUBSTR(ic.IRC_DESC,1,1) then 'OK' else 'Less' end Status "
        '    '''WI1861: change in query to check equivalent level, created by: Avik Mukherjee, created On: 21-07-2021
        '    qry1.CommandText += " from t_survey_status ss,hrps.t_emp_master_feedback360 ea,t_ir_codes ic where ss.ss_asses_pno=ea.ema_perno AND DECODE(ic.irc_type,'360V2','I2','360V3','I3','360V4','I4','360V5','I5','360V6','I6')=ea.ema_eqv_level and upper(ss.ss_categ)=upper(ic.irc_code) and ea.ema_comp_code='1000' and ea.ema_eqv_level in ('I2','I3', 'I4','I5','I6') and ic.irc_valid_tag='A' and  ss.ss_status='SE' and ss.ss_del_tag='N' and ss.ss_app_tag='AP' and ss.ss_wfl_statusin('2','3') and ss_year=:fy and ss_srlno=:srlno"

        '    qry1.CommandText += "  group by ss.ss_asses_pno,ss.ss_categ,ea.ema_ename,ea.ema_bhr_pno,ea.ema_bhr_name,ic.IRC_DESC,ic.irc_type order by ss.ss_asses_pno"
        '    qry1.Connection = conHrps
        '    qry1.Parameters.Clear()
        '    qry1.Parameters.AddWithValue("fy", ViewState("FY").ToString())
        '    qry1.Parameters.AddWithValue("srlno", ViewState("SRLNO").ToString())
        '    dt2 = getDataInDt(qry1)
        '    If dt2.Rows.Count > 0 Then
        '        dgMiniCriteria.DataSource = dt2
        '        dgMiniCriteria.DataBind()

        '        Response.Clear()
        '        Response.Buffer = True
        '        Response.AddHeader("content-disposition", "attachment;filename=Minimum Criteria.xls")
        '        Response.Charset = ""
        '        Response.ContentType = "application/vnd.ms-excel"

        '        Using sw As New StringWriter()
        '            Dim hw As New HtmlTextWriter(sw)
        '            dgMiniCriteria.HeaderStyle.BackColor = Color.Brown
        '            dgMiniCriteria.BorderStyle = BorderStyle.Solid
        '            'dgMiniCriteria.GridLines = True

        '            If dgMiniCriteria.Visible = True Then


        '                'For Each row As DataGridItem In gdvMiniCriteria.Rows
        '                '    row.BackColor = Color.White
        '                '    'For Each cell As TableCell In row.Cells
        '                '    '    If row.RowIndex Mod 2 = 0 Then
        '                '    '        cell.BackColor = grdMIS.AlternatingRowStyle.BackColor
        '                '    '    Else
        '                '    '        cell.BackColor = grdMIS.RowStyle.BackColor
        '                '    '    End If
        '                '    '    cell.CssClass = "textmode"
        '                '    'Next
        '                'Next
        '                'To Export all pages
        '                dgMiniCriteria.AllowPaging = False
        '                dgMiniCriteria.RenderControl(hw)
        '            End If


        '            'style to format numbers to string
        '            'Dim style As String = "<style> .textmode { } </style>"
        '            'Response.Write(style)
        '            Response.Output.Write(sw.ToString())
        '            Response.Flush()
        '            Response.[End]()
        '        End Using
        '    Else
        '        dgMiniCriteria.DataSource = Nothing
        '        dgMiniCriteria.DataBind()
        '    End If

        'Catch ex As Exception
        'Finally
        '    If conHrps.State = ConnectionState.Open Then
        '        conHrps.Close()
        '    End If
        'End Try
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Confirms that an HtmlForm control is rendered for the specified ASP.NET
        '     server control at run time. 

    End Sub

    Public Sub bindMinimumCriteria()
        Try
            Dim executiveCode, deptCode, buhrPno, officerPno, officerCatg As String
            executiveCode = ""
            deptCode = ""
            buhrPno = ""
            officerPno = ""
            officerCatg = ""
            If ddlExecutive.SelectedValue <> "--Select--" Then
                executiveCode = ddlExecutive.SelectedValue
            End If
            If ddlDept.SelectedValue <> "--Select--" Then
                deptCode = ddlDept.SelectedValue
            End If

            If txtBuhr.Text.Trim().Length > 0 Then
                buhrPno = txtBuhr.Text
            End If
            If ChkBUHRRole() = True And ChkRole() = False Then
                buhrPno = Session("ADM_USER").ToString()
            End If

            If txtpnosub.Text.Trim().Length > 0 Then
                officerPno = txtpnosub.Text
            End If

            officerCatg = txtYear.Text.Substring(2, 2).ToString() + "" + txtCycle.Text.Trim.ToString
            Dim qry As New OracleCommand()
            Dim dt As New DataTable()
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            qry.Connection = conHrps
            qry.Parameters.Clear()

            qry.CommandText = "select aa.pno,aa.ema_ename,aa.ema_empl_sgrade,aa.ema_eqv_level,aa.ema_desgn_desc,aa.ema_dept_desc,aa.ema_email_id,aa.ema_exec_head_desc,aa.ema_reporting_to_pno Superior_PNo,aa.Superior_Name,aa.Superior_EmaiId,aa.Location,aa.ema_bhr_pno,aa.ema_bhr_name,aa.Buhr_emailId,aa.pending1,aa.completed1,bb.pending2,bb.completed2,dd.pending3,dd.completed3,0 pending4,0 completed4,0 pending5,0 completed5,0 pending6,0 completed6,case when aa.Criteria1='Ok' and bb.Criteria2='Ok' and dd.Criteria3='Ok' then 'Ok' else 'Less' end Criterial from "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending1,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed1,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I6','I6','" & officerCatg.ToString & "I5','I5','" & officerCatg.ToString & "I4','I4')=y.ema_eqv_level and irc_code='SELF') then 'Ok' else 'Less' end Criteria1 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and  y.ema_eqv_level in ('I4','I5','I6') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='Self'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC) aa,"
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending2,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed2,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I6','I6','" & officerCatg.ToString & "I5','I5','" & officerCatg.ToString & "I4','I4')=y.ema_eqv_level and irc_code='MANGR') then 'Ok' else 'Less' end Criteria2 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I4','I5','I6') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='MANGR'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC) bb,"
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending3,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed3,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I6','I6','" & officerCatg.ToString & "I5','I5','" & officerCatg.ToString & "I4','I4')=y.ema_eqv_level and irc_code='INTSH') then 'Ok' else 'Less' end Criteria3 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I4','I5','I6') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='INTSH'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC) dd"
            qry.CommandText += " where aa.pno=bb.pno(+) and aa.pno=dd.pno(+) order by 1"
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("EMA_EXEC_HEAD", executiveCode.ToString)
            qry.Parameters.AddWithValue("EMA_DEPT_CODE", deptCode.ToString)
            qry.Parameters.AddWithValue("ema_bhr_pno", buhrPno.ToString)
            qry.Parameters.AddWithValue("ss_asses_pno", officerPno.ToString)
            qry.Parameters.AddWithValue("yr", txtYear.Text.ToString)
            qry.Parameters.AddWithValue("srlno", txtCycle.Text.ToString)
            dt = getDataInDt(qry)

            'I1 level
            qry.CommandText = ""
            qry.CommandText += " select aa.pno,aa.ema_ename,aa.ema_empl_sgrade,aa.ema_eqv_level,aa.ema_desgn_desc,aa.ema_dept_desc,aa.ema_email_id,aa.ema_exec_head_desc,aa.ema_reporting_to_pno Superior_PNo,aa.Superior_Name,aa.Superior_EmaiId,aa.Location,aa.ema_bhr_pno,aa.ema_bhr_name,aa.Buhr_emailId,aa.pending1,aa.completed1,bb.pending2,bb.completed2,0 pending3,0 completed3,cc.pending4,cc.completed4,ee.pending5,ee.completed5,0 pending6,0 completed6,case when Criteria1='Ok' and Criteria2='Ok' and  Criteria4='Ok' and Criteria5='Ok' then 'Ok' else 'Less' end Criterial from "

            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending1,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed1,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I1','I1')=y.ema_eqv_level and irc_code='SELF') then 'Ok' else 'Less' end Criteria1 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I1') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='Self'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) aa, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending2,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed2,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I1','I1')=y.ema_eqv_level and irc_code='MANGR') then 'Ok' else 'Less' end Criteria2 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and  y.ema_eqv_level in ('I1') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='MANGR'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) bb, "
            'qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending3,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed3,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I2','I2')=y.ema_eqv_level and irc_code='INTSH') then 'Ok' else 'Less' end Criteria3 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_dotted_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I2') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='INTSH'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) dd, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending4,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed4,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I1','I1')=y.ema_eqv_level and irc_code='PEER') then 'Ok' else 'Less' end Criteria4 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I1') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='PEER'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) cc, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending5,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed5,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I1','I1')=y.ema_eqv_level and irc_code='ROPT') then 'Ok' else 'Less' end Criteria5 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I1') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='ROPT'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC) ee "
            qry.CommandText += "where aa.pno=bb.pno(+) and aa.pno=cc.pno(+) and aa.pno=ee.pno order by 1"
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("EMA_EXEC_HEAD", executiveCode.ToString)
            qry.Parameters.AddWithValue("EMA_DEPT_CODE", deptCode.ToString)
            qry.Parameters.AddWithValue("ema_bhr_pno", buhrPno.ToString)
            qry.Parameters.AddWithValue("ss_asses_pno", officerPno.ToString)
            qry.Parameters.AddWithValue("yr", txtYear.Text.ToString)
            qry.Parameters.AddWithValue("srlno", txtCycle.Text.ToString)

            Dim dtI1 = getDataInDt(qry)

            'I2 level
            qry.CommandText = ""
            qry.CommandText += " select aa.pno,aa.ema_ename,aa.ema_empl_sgrade,aa.ema_eqv_level,aa.ema_desgn_desc,aa.ema_dept_desc,aa.ema_email_id,aa.ema_exec_head_desc,aa.ema_reporting_to_pno Superior_PNo,aa.Superior_Name,aa.Superior_EmaiId,aa.Location,aa.ema_bhr_pno,aa.ema_bhr_name,aa.Buhr_emailId,aa.pending1,aa.completed1,bb.pending2,bb.completed2,dd.pending3,dd.completed3,cc.pending4,cc.completed4,ee.pending5,ee.completed5,0 pending6,0 completed6,case when Criteria1='Ok' and Criteria2='Ok' and Criteria3='Ok' and Criteria4='Ok' and Criteria5='Ok' then 'Ok' else 'Less' end Criterial from "

            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending1,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed1,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I2','I2')=y.ema_eqv_level and irc_code='SELF') then 'Ok' else 'Less' end Criteria1 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_dotted_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I2') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='Self'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) aa, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending2,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed2,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I2','I2')=y.ema_eqv_level and irc_code='MANGR') then 'Ok' else 'Less' end Criteria2 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_dotted_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and  y.ema_eqv_level in ('I2') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='MANGR'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) bb, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending3,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed3,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I2','I2')=y.ema_eqv_level and irc_code='INTSH') then 'Ok' else 'Less' end Criteria3 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_dotted_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I2') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='INTSH'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) dd, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending4,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed4,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I2','I2')=y.ema_eqv_level and irc_code='PEER') then 'Ok' else 'Less' end Criteria4 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_dotted_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I2') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='PEER'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) cc, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending5,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed5,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I2','I2')=y.ema_eqv_level and irc_code='ROPT') then 'Ok' else 'Less' end Criteria5 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_dotted_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I2') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='ROPT'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_dotted_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC) ee "
            qry.CommandText += "where aa.pno=bb.pno(+) and aa.pno=dd.pno(+) and aa.pno=cc.pno(+) and aa.pno=ee.pno order by 1"
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("EMA_EXEC_HEAD", executiveCode.ToString)
            qry.Parameters.AddWithValue("EMA_DEPT_CODE", deptCode.ToString)
            qry.Parameters.AddWithValue("ema_bhr_pno", buhrPno.ToString)
            qry.Parameters.AddWithValue("ss_asses_pno", officerPno.ToString)
            qry.Parameters.AddWithValue("yr", txtYear.Text.ToString)
            qry.Parameters.AddWithValue("srlno", txtCycle.Text.ToString)

            Dim dtI2 = getDataInDt(qry)
            'I3 level
            qry.CommandText = ""
            qry.CommandText += " select aa.pno,aa.ema_ename,aa.ema_empl_sgrade,aa.ema_eqv_level,aa.ema_desgn_desc,aa.ema_dept_desc,aa.ema_email_id,aa.ema_exec_head_desc,aa.ema_reporting_to_pno Superior_PNo,aa.Superior_Name,aa.Superior_EmaiId,aa.Location,aa.ema_bhr_pno,aa.ema_bhr_name,aa.Buhr_emailId,aa.pending1,aa.completed1,bb.pending2,bb.completed2,dd.pending3,dd.completed3,0 pending4,0 completed4,0 pending5,0 completed5,cc.pending6,cc.completed6,case when Criteria1='Ok' and Criteria2='Ok' and Criteria3='Ok' and Criteria4='Ok' then 'Ok' else 'Less' end Criterial from "

            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending1,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed1,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I3','I3','" & officerCatg.ToString & "TG','TG')=y.ema_eqv_level and irc_code='SELF') then 'Ok' else 'Less' end Criteria1 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I3','TG') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='Self'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) aa, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending2,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed2,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I3','I3','" & officerCatg.ToString & "TG','TG')=y.ema_eqv_level and irc_code='MANGR') then 'Ok' else 'Less' end Criteria2 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I3','TG') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='MANGR'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) bb, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending3,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed3,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I3','I3','" & officerCatg.ToString & "TG','TG')=y.ema_eqv_level and irc_code='INTSH') then 'Ok' else 'Less' end Criteria3 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I3','TG') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ='INTSH'  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) dd, "
            qry.CommandText += "(select a.ss_asses_pno pno,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename Superior_Name,z.ema_email_id Superior_EmaiId,y.EMA_PERS_SUBAREA_DESC Location,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id Buhr_emailId,a.ss_categ catg,count(decode(a.ss_wfl_status,'2','Pending','9','Pending',null)) Pending6,count(decode(a.ss_wfl_status,'3','Completed',null)) Completed6,case when count(decode(a.ss_wfl_status,'3','Completed',null))>=(select nvl(substr(irc_desc,1,1),0) crt from t_ir_codes where decode(irc_type,'" & officerCatg.ToString & "I3','I3','" & officerCatg.ToString & "TG','TG')=y.ema_eqv_level and irc_code='PEER') then 'Ok' else 'Less' end Criteria4 from hrps.t_survey_status a,hrps.t_emp_master_feedback360 y,hrps.t_emp_master_feedback360 z,hrps.t_emp_master_feedback360 x where a.ss_asses_pno=y.ema_perno and z.ema_perno(+)=y.ema_reporting_to_pno and x.ema_perno(+)=y.ema_bhr_pno and a.ss_year=y.ema_year and a.ss_srlno=y.ema_cycle and y.ema_year=z.ema_year(+) and y.ema_year=x.ema_year(+) and y.ema_cycle=z.ema_cycle(+) and y.ema_cycle=x.ema_cycle(+) and y.ema_eqv_level in ('I3','TG') and a.ss_year=:yr and a.ss_srlno=:srlno and a.ss_wfl_status in ('2','3','9') and ss_categ in ('PEER','ROPT')  and nvl(y.ema_bhr_pno,'x')=nvl(:ema_bhr_pno,nvl(y.ema_bhr_pno,'x')) and nvl(y.EMA_EXEC_HEAD,'x')=nvl(:EMA_EXEC_HEAD,nvl(y.EMA_EXEC_HEAD,'x')) and nvl(y.EMA_DEPT_CODE,'x')=nvl(:EMA_DEPT_CODE,nvl(y.EMA_DEPT_CODE,'x')) and a.ss_asses_pno=nvl(:ss_asses_pno,a.ss_asses_pno) group by a.ss_asses_pno,a.ss_categ,y.ema_ename,y.ema_empl_sgrade,y.ema_eqv_level,y.ema_desgn_desc,y.ema_dept_desc,y.ema_email_id,y.ema_exec_head_desc,y.ema_reporting_to_pno,z.ema_ename,z.ema_email_id ,y.ema_bhr_pno,y.ema_bhr_name,x.ema_email_id,y.EMA_PERS_SUBAREA_DESC ) cc "
            qry.CommandText += "where aa.pno=bb.pno(+) and aa.pno=dd.pno(+) and aa.pno=cc.pno(+) order by 1"
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("EMA_EXEC_HEAD", executiveCode.ToString)
            qry.Parameters.AddWithValue("EMA_DEPT_CODE", deptCode.ToString)
            qry.Parameters.AddWithValue("ema_bhr_pno", buhrPno.ToString)
            qry.Parameters.AddWithValue("ss_asses_pno", officerPno.ToString)
            qry.Parameters.AddWithValue("yr", txtYear.Text.ToString)
            qry.Parameters.AddWithValue("srlno", txtCycle.Text.ToString)
            Dim dtI3 = getDataInDt(qry)
            'qry.CommandText += ""
            'qry.CommandText += ""

            'qry.CommandText = " select ss.ss_asses_pno,ea.ema_ename,decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'People you lead') ss_categ1,ea.ema_bhr_pno,ea.ema_bhr_name ,SUBSTR(ic.IRC_DESC,1,1) minmum,count ( case when ss_wfl_status = '2' then 'Pending' end ) Pending, "
            'qry.CommandText += "decode(ic.irc_type,'360V2',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),'360V3',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peer/Subordinate',upper('intsh'),'Internal stakeholders'),decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('intsh'),'Peer/Subordinate/Internal stakeholders')) ss_categ,"
            'qry.CommandText += " count ( case when ss_wfl_status = '3' then 'Completed' end ) Completed,"
            'qry.CommandText += "  case when count ( case when ss_wfl_status = '3' then 'Completed' end )>=SUBSTR(ic.IRC_DESC,1,1) then 'OK' else 'Less' end Status "
            ''''WI1861: change in query to check equivalent level, created by: Avik Mukherjee, created On: 21-07-2021
            'qry.CommandText += " from t_survey_status ss,hrps.t_emp_master_feedback360 ea,t_ir_codes ic where ss.ss_asses_pno=ea.ema_perno  AND DECODE(ic.irc_type,'360V2','I2','360V3','I3','360V4','I4','360V5','I5','360V6','I6')=ea.ema_eqv_level and upper(ss.ss_categ)=upper(ic.irc_code) and ea.ema_comp_code='1000' and ea.ema_eqv_level in ('I2','I3', 'I4','I5','I6') and ic.irc_valid_tag='A' and  ss.ss_status='SE' and ss.ss_del_tag='N' and ss.ss_app_tag='AP' and ss.ss_wfl_status in('2','3') and ss_year=:fy and ss_srlno=:srlno"

            'If ChkBUHRRole() = True And ChkRole() = False Then
            '    qry.CommandText += " and ea.ema_bhr_pno=:buhr"
            '    qry.Parameters.AddWithValue("buhr", Session("ADM_USER").ToString())
            'End If
            'If ChkBUHRRole() = True And ChkRole() = True Then
            '    qry.CommandText += ""
            'End If
            'If ddlExecutive.SelectedValue <> "--Select--" Then
            '    qry.CommandText += " and ea.ema_exec_head=:exhead"
            '    qry.Parameters.AddWithValue("exhead", ddlExecutive.SelectedValue)
            'End If
            'If ddlDept.SelectedValue <> "--Select--" Then
            '    qry.CommandText += " and ea.ema_dept_code=:dept"
            '    qry.Parameters.AddWithValue("dept", ddlDept.SelectedValue)
            'End If
            'If txtpnosub.Text.Trim <> "" Then
            '    qry.CommandText += " and ss.ss_asses_pno=:pno"
            '    qry.Parameters.AddWithValue("pno", txtpnosub.Text.Trim.ToString)
            'End If
            'If txtBuhr.Text.Trim <> "" Then
            '    qry.CommandText += " and ea.ema_bhr_pno=:buhrnm"
            '    qry.Parameters.AddWithValue("buhrnm", txtBuhr.Text.Trim.ToString)
            'End If
            'qry.Parameters.AddWithValue("fy", txtYear.Text.ToString())
            'qry.Parameters.AddWithValue("srlno", txtCycle.Text.ToString())
            'qry.CommandText += "  group by ss.ss_asses_pno,ss.ss_categ,ea.ema_ename,ea.ema_bhr_pno,ea.ema_bhr_name,ic.IRC_DESC,ic.irc_type order by ss.ss_asses_pno"
            Dim dtMerge As New DataTable
            dtMerge.Merge(dt)
            dtMerge.Merge(dtI1)
            dtMerge.Merge(dtI2)
            dtMerge.Merge(dtI3)
            If dtMerge.Rows.Count > 0 Then
                gdvMiniCriteria.DataSource = dtMerge
                gdvMiniCriteria.DataBind()
            Else
                gdvMiniCriteria.DataSource = Nothing
                gdvMiniCriteria.DataBind()
            End If

        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub OnDataBound(sender As Object, e As EventArgs)
        Dim row As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
        Dim cell As New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 2
        cell.Text = "Self"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 2
        cell.Text = "Manager"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 2
        cell.Text = "Internal Stakeholders/Peers/Subordinates (IL2-6)"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 2
        cell.Text = "Peers (IL1-2)"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 2
        cell.Text = "Subordinates (IL1-2)"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        cell.ColumnSpan = 2
        cell.Text = "Peers & Subordinates (IL3)"
        row.Controls.Add(cell)

        cell = New TableHeaderCell()
        'cell.RowSpan = 2
        cell.Text = ""
        row.Controls.Add(cell)

        'row.BackColor = ColorTranslator.FromHtml("#3AC0F2")
        gdvMiniCriteria.HeaderRow.Parent.Controls.AddAt(0, row)
    End Sub

    Public Function ChkBUHRRole() As Boolean
        Try
            Dim t As Boolean = False

            Dim strrole As New OracleCommand


            strrole.CommandText = "select ema_bhr_pno IGP_user_id from hrps.t_emp_master_feedback360  where ema_bhr_pno =:buhr"
            strrole.Connection = conHrps
            strrole.Parameters.Clear()
            strrole.Parameters.AddWithValue("buhr", Session("ADM_USER").ToString())
            'Dim cmd = New OracleCommand(strrole, conHrps)
            Dim f = getDataInDt(strrole)

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
        Try
            Dim t As Boolean = False

            Dim strrole As New OracleCommand
            strrole.CommandText = "select IGP_user_id from t_ir_adm_grp_privilege where igp_group_id IN ('360FEEDBAC','360DF_SA','360DF_LD') "
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
                lblname.Text = "HRBP Admin"
                t = False
            End If
            Return t
        Catch ex As Exception

        End Try
    End Function
    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            Dim q As New OracleCommand()
            q.CommandText = "Select ema_perno from t_ir_adm_grp_privilege,hrps.t_emp_master_feedback360 where igp_group_id IN ('360FEEDBAC','360DF_SA','360DF_LD')  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id=:pno and EMA_COMP_CODE='1000'"
            'q.CommandText += " union select ema_bhr_pno ema_perno from hrps.t_emp_master_feedback360  where ema_bhr_pno ='" & Session("ADM_USER").ToString() & "' and rownum=1"
            'WI1861: allow TCS employee to access the report as super admin, created by:Avik Mukherjee, created On: 16-06-2021
            q.CommandText += " union select IGP_user_id ema_perno from  t_ir_adm_grp_privilege where IGP_user_id='" & pno & "' and igp_group_id IN ('360FEEDBAC','360DF_SA','360DF_LD')  and IGP_STATUS ='A'"
            q.Connection = conHrps
            q.Parameters.Clear()
            q.Parameters.AddWithValue("pno", pno.ToString())
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
            MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function
    'Commented & Added by TCS on 13012024, Commented Old Code & Added new Excel download code
    Protected Sub ExportExcel()
        Dim dt As New DataTable("Minimum Criteria")
        Dim columnDuplication As Integer = 0
        For Each cell As TableCell In gdvMiniCriteria.HeaderRow.Cells
            If dt.Columns.Contains(HttpUtility.HtmlDecode(cell.Text)) Then
                Dim strBlank As String = ""
                columnDuplication = columnDuplication + 1
                For i = 1 To columnDuplication
                    strBlank = strBlank + " "
                Next
                dt.Columns.Add(HttpUtility.HtmlDecode(cell.Text) + strBlank)
            Else
                dt.Columns.Add(HttpUtility.HtmlDecode(cell.Text))
            End If
        Next
        For Each row As GridViewRow In gdvMiniCriteria.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 1
                dt.Rows(dt.Rows.Count - 1)(i) = HttpUtility.HtmlDecode(row.Cells(i).Text)
            Next
        Next
        Using wb As New XLWorkbook()
            Dim ws = wb.Worksheets.Add(dt)
            ws.Row(1).InsertRowsAbove(1)
            ws.Cell(1, "P").Value = "Self"
            ws.Range("P1:Q1").Merge()
            ws.Cell(1, "P").Style.Alignment.WrapText = True

            ws.Cell(1, "R").Value = "Manager"
            ws.Range("R1:S1").Merge()
            ws.Cell(1, "R").Style.Alignment.WrapText = True

            ws.Cell(1, "T").Value = "Internal Stakeholders/Peers/Subordinates (IL2-6)"
            ws.Range("T1:U1").Merge()
            ws.Cell(1, "T").Style.Alignment.WrapText = True

            ws.Cell(1, "V").Value = "Peers (IL1-2)"
            ws.Range("V1:W1").Merge()
            ws.Cell(1, "V").Style.Alignment.WrapText = True

            ws.Cell(1, "X").Value = "Subordinates (IL1-2)"
            ws.Range("X1:Y1").Merge()
            ws.Cell(1, "X").Style.Alignment.WrapText = True

            ws.Cell(1, "Z").Value = "Peers & Subordinates (IL3)"
            ws.Range("Z1:AA1").Merge()
            ws.Cell(1, "Z").Style.Alignment.WrapText = True
            ws.Range("P1:AB1").Style.Font.Bold = True
            ws.Range("P1:AB1").Style.Fill.BackgroundColor = XLColor.DarkBlue
            ws.Range("P1:AB1").Style.Font.FontColor = XLColor.White
            ws.Rows(1).Height = 60
            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=Minimum_Criteria_Report_" + DateTime.Now.ToString("yyyyMMddHHmmss") & ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                'Response.End()
                HttpContext.Current.Response.Flush()
                HttpContext.Current.Response.SuppressContent = True
                HttpContext.Current.ApplicationInstance.CompleteRequest()
            End Using
        End Using
    End Sub
    'End
End Class
