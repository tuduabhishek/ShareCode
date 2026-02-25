Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
''' <summary>
''' WI484: rectification in code to download data by buhr
''' and also allow super admin to download all data
''' created by: Avik Mukherjee
''' Date: 09-06-2021
''' </summary>
Partial Class SurveyCompletionNMBStatusRpt
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
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
    Private Sub SurveyCompletionStatusRpt_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub
    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        'Session.Clear()
        'Session.RemoveAll()
        'Session.Abandon()
        getFy()
        getsrlno()
        If Session("ADM_USER") = "" Then

            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            strUserID = "148536"


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
    Private Sub SurveyCompletionStatusRpt_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            SessionTimeOut()
            If Not IsPostBack Then

                bindExecHead()
                btn_download.Enabled = False
                btn_download_all.Visible = False
                If ChkRole() Then
                    btn_download_all.Visible = True
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub bindGrid()

        Try
            If ChkRole() = True Or ChkBUHRRole() = True Then

                'If Request.QueryString("adm") = "1" Then
                bindSelectAssesorGrid()
                'End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bindExecHead()
        Try
            Dim dtExecHead As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct ema_exec_head,ema_exec_head_desc from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head_desc<>'Not found' and ema_exec_head<>'00000000' and ema_comp_code='1000' and ema_year=:ema_year and ema_cycle=:ema_cycle and ema_eqv_level='NM'"
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
            ' Start WI368  by Manoj Kumar on 30-05-2021
            query.Parameters.Clear()      'WI368 Department not show When executive head select.
            query.Connection = conHrps      'WI368 Department not show When executive head select.
            'End by Manoj Kumar on 30-05-2021
            ddlDept.Items.Clear()
            query.CommandText = "select distinct ema_dept_code DEPT,ema_dept_desc DEPTDESC from hrps.t_emp_master_feedback360 where ema_comp_code='1000'  and ema_dept_desc<>'Not found' and ema_exec_head = :ema_exec_head and ema_year=:ema_year and ema_cycle=:ema_cycle and ema_eqv_level='NM'"
            If ChkBUHRRole() = False Then
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
        'Commented & Added by TCS on 17012023, Commented Old Code & Added new Excel download code
        Try
            ExportExcel()
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Some error occured while downloading the report.")
        End Try
        'End
        'Response.Clear()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", "attachment;filename=Status Survey Completion.xls")
        'Response.Charset = ""
        'Response.ContentType = "application/vnd.ms-excel"
        'Using sw As New StringWriter()
        '    Dim hw As New HtmlTextWriter(sw)

        '    If gdvselectAssesor.Visible = True Then
        '        'To Export all pages
        '        gdvselectAssesor.AllowPaging = False
        '        ''Me.BindGrid()

        '        gdvselectAssesor.HeaderRow.BackColor = Color.LightGray
        '        For Each cell As TableCell In gdvselectAssesor.HeaderRow.Cells
        '            cell.BackColor = gdvselectAssesor.HeaderStyle.BackColor
        '        Next
        '        For Each row As GridViewRow In gdvselectAssesor.Rows
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

        '        gdvselectAssesor.RenderControl(hw)
        '    End If
        '    'If gdvselectAssesor.Visible = True Then
        '    '    'To Export all pages
        '    '    gdvselectAssesor.AllowPaging = False
        '    '    'Me.BindGrid()

        '    '    'grdMIS.HeaderRow.BackColor = Color.LightGray
        '    '    'For Each cell As TableCell In gdvTimeCycle.HeaderRow.Cells
        '    '    '    cell.BackColor = gdvTimeCycle.HeaderStyle.BackColor
        '    '    'Next
        '    '    'For Each row As GridViewRow In gdvTimeCycle.Rows
        '    '    '    'row.BackColor = Color.LightGray
        '    '    '    For Each cell As TableCell In row.Cells
        '    '    '        If row.RowIndex Mod 2 = 0 Then
        '    '    '            cell.BackColor = gdvTimeCycle.AlternatingRowStyle.BackColor
        '    '    '        Else
        '    '    '            cell.BackColor = gdvTimeCycle.RowStyle.BackColor
        '    '    '        End If
        '    '    '        cell.CssClass = "textmode"
        '    '    '    Next
        '    '    'Next

        '    '    gdvselectAssesor.RenderControl(hw)
        '    'End If

        '    'style to format numbers to string
        '    Dim style As String = "<style> .textmode { } </style>"
        '    Response.Write(style)
        '    Response.Output.Write(sw.ToString())
        '    Response.Flush()
        '    Response.[End]()
        'End Using
    End Sub
    Protected Sub btn_download_all_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_download_all.Click
        Try
            If txtYear.Text.Trim().ToString = "" And txtCycle.Text.Trim().ToString = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill year and cycle field.")
                Exit Sub
            Else
                Dim qry1 As New OracleCommand
                Dim dt2 As New DataTable()
                qry1.CommandText = " select distinct ss.ss_asses_pno Pno,ea.EMA_ENAME,ea.EMA_EQV_LEVEL,ea.EMA_DESGN_DESC Designation,ea.ema_dept_desc Department,ea.ema_email_id Email_id, "
                    qry1.CommandText += "  ea.ema_exec_head_desc Executive_Head,decode(ea.EMA_EQV_LEVEL,'I2',ea.EMA_DOTTED_PNO,ea.EMA_REPORTING_TO_PNO) Superior_Pno,(select EMA_ENAME from hrps.t_emp_master_feedback360  where ema_perno=decode(ea.EMA_EQV_LEVEL,'I2',ea.EMA_DOTTED_PNO,ea.EMA_REPORTING_TO_PNO) and rownum=1) Superior_Name,ea.ema_bhr_pno BUHR_Pno,ea.ema_bhr_name BUHR_NAME,ss.ss_pno Respondent_Pno,"
                    qry1.CommandText += " ss.ss_name Respondent_Name,ss.ss_level Respondent_Level,ss.ss_desg Respondent_Designation,ss.ss_dept Respondent_Department,ss.ss_email Respondent_Email_Id,decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers and Subordinates',upper('intsh'),'Internal stakeholders',upper('ropt'),'People you lead') Respondent_Category1,decode(ss_wfl_status,'2','Pending','3','Complete','9','Insufficient Explosure') Status"
                    qry1.CommandText += ",decode(ea.EMA_EQV_LEVEL,'I1',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),'I2',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),'I3',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peer/Subordinate',upper('intsh'),'Internal stakeholders'),'TG',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('intsh'),'Peer/Subordinate/Internal stakeholders')) Respondent_Category"
                qry1.CommandText += " from hrps.t_emp_master_feedback360 ea,t_survey_status ss where ea.ema_perno= ss.ss_asses_pno and ss.ss_status='SE' and ss.ss_del_tag='N' and ss.ss_app_tag='AP' and ea.ema_year=ss.ss_year and ea.ema_cycle=ss.ss_srlno  and ss.ss_year=:fy and  SS_SRLNO=:srlno and ea.EMA_COMP_CODE='1000' and ea.ema_eqv_level='NM'"

                ''''WI484: end by Avik Mukherjee, on 09-06-2021
                qry1.CommandText += " and ea.ema_eqv_level in ('I1','I2','I3', 'I4','I5','I6','TG','NM') and ea.ema_comp_code='1000' order by 1"

                qry1.Connection = conHrps
                qry1.Parameters.Clear()
                qry1.Parameters.AddWithValue("fy", txtYear.Text.Trim().ToString())
                qry1.Parameters.AddWithValue("srlno", txtCycle.Text.Trim().ToString())
                'Dim qry = New OracleCommand(qry1, conHrps)
                dt2 = getDataInDt(qry1)
                'Commented & Added by TCS on 17012023, Commented Old Code & Added new Excel download code
                If dt2.Rows.Count > 0 Then
                    gdvselectAssesor.DataSource = dt2
                    gdvselectAssesor.DataBind()
                    ExportExcel()
                Else
                    gdvselectAssesor.DataSource = Nothing
                    gdvselectAssesor.DataBind()
                    dgMiniCriteria.DataSource = Nothing
                    dgMiniCriteria.DataBind()
                End If
            End If
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Some error occured while downloading the report.")
        End Try
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Confirms that an HtmlForm control is rendered for the specified ASP.NET
        '     server control at run time. 

    End Sub
    Public Sub bindSelectAssesorGrid()
        Try

            Dim dt As New DataTable()
            Dim qrypending As New OracleCommand
            qrypending.Connection = conHrps
            qrypending.Parameters.Clear()
            qrypending.CommandText = " select distinct ss.ss_asses_pno Pno,ea.EMA_ENAME,ea.EMA_EQV_LEVEL,ea.EMA_DESGN_DESC Designation,ea.ema_dept_desc Department,ea.ema_email_id Email_id, "
                qrypending.CommandText += "  ea.ema_exec_head_desc Executive_Head,decode(ea.EMA_EQV_LEVEL,'I2',ea.EMA_DOTTED_PNO,ea.EMA_REPORTING_TO_PNO) Superior_Pno,(select EMA_ENAME from hrps.t_emp_master_feedback360  where ema_perno=decode(ea.EMA_EQV_LEVEL,'I2',ea.EMA_DOTTED_PNO,ea.EMA_REPORTING_TO_PNO) and rownum=1) Superior_Name,ea.ema_bhr_pno BUHR_Pno,ea.ema_bhr_name BUHR_NAME,ss.ss_pno Respondent_Pno,"
                qrypending.CommandText += " ss.ss_name Respondent_Name,ss.ss_level Respondent_Level,ss.ss_desg Respondent_Designation,ss.ss_dept Respondent_Department,ss.ss_email Respondent_Email_Id,decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers and Subordinates',upper('intsh'),'Internal stakeholders',upper('ropt'),'People you lead') Respondent_Category1,decode(ss_wfl_status,'2','Pending','3','Complete','9','Insufficient Explosure') Status"
                qrypending.CommandText += ",decode(ea.EMA_EQV_LEVEL,'I1',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),'I2',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),'I3',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peer/Subordinate',upper('intsh'),'Internal stakeholders'),'TG',decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'Subordinates'),decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager/Superior',upper('intsh'),'Peer/Subordinate/Internal stakeholders')) Respondent_Category"
                qrypending.CommandText += " from hrps.t_emp_master_feedback360 ea,t_survey_status ss where ea.ema_perno= ss.ss_asses_pno and ea.ema_year=ss.ss_year and ea.ema_cycle=ss.ss_srlno and ss.ss_status='SE' and ss.ss_del_tag='N' and ss.ss_app_tag='AP'  and ss.ss_year=:fy and  SS_SRLNO=:srlno and ea.ema_comp_code='1000' and ea.ema_eqv_level='NM'"

                ''''WI484: End 
                If ChkBUHRRole() = True And ChkRole() = False Then
                qrypending.CommandText += " and ea.ema_bhr_pno=:buhrno"
                qrypending.Parameters.AddWithValue("buhrno", Session("ADM_USER").ToString())
            End If
            If ChkBUHRRole() = True And ChkRole() = True Then
                qrypending.CommandText += " "
            End If
            If ddlExecutive.SelectedValue <> "--Select--" Then
                qrypending.CommandText += " and ea.ema_exec_head=:exhead"
                qrypending.Parameters.AddWithValue("exhead", ddlExecutive.SelectedValue)
            End If
            If ddlDept.SelectedValue <> "--Select--" Then
                qrypending.CommandText += " and ea.ema_dept_code=:dept"
                qrypending.Parameters.AddWithValue("dept", ddlDept.SelectedValue)
            End If
            If txtpnosub.Text.Trim <> "" Then
                qrypending.CommandText += " and ss.ss_asses_pno=:asseno"
                qrypending.Parameters.AddWithValue("asseno", txtpnosub.Text.Trim.ToString)
            End If
            If txtBuhr.Text.Trim <> "" Then
                qrypending.CommandText += " and ea.ema_bhr_pno=:buhr"
                qrypending.Parameters.AddWithValue("buhr", txtBuhr.Text.Trim.ToString)
            End If
            qrypending.CommandText += " and ea.ema_eqv_level in ('I1','I2','I3', 'I4','I5','I6','TG','NM') and ea.ema_comp_code='1000' order by 1"
            qrypending.Parameters.AddWithValue("fy", txtYear.Text.Trim().ToString())
            qrypending.Parameters.AddWithValue("srlno", txtCycle.Text.Trim().ToString())
            'Dim qry = New OracleCommand(qrypending, conHrps)
            dt = getDataInDt(qrypending)
            If dt.Rows.Count > 0 Then
                gdvselectAssesor.DataSource = dt
                gdvselectAssesor.DataBind()
            Else
                gdvselectAssesor.DataSource = Nothing
                gdvselectAssesor.DataBind()
            End If

        Catch ex As Exception

        End Try
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
    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            Dim q As New OracleCommand()
            q.CommandText = "Select ema_perno from t_ir_adm_grp_privilege,hrps.t_emp_master_feedback360 where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id=:pno and EMA_COMP_CODE='1000'"
            q.CommandText += " union select ema_bhr_pno ema_perno from hrps.t_emp_master_feedback360  where ema_bhr_pno =:buhrno and rownum=1"
            ' Adding code for providing access for external user  Added by Mukul Mishra 30-Jul-2021
            q.CommandText += " union select IGP_user_id ema_perno from  t_ir_adm_grp_privilege where IGP_user_id=:pno and igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A'"
            'Ends
            q.Connection = conHrps
            q.Parameters.Clear()
            q.Parameters.AddWithValue("pno", pno.ToString)
            q.Parameters.AddWithValue("buhrno", Session("ADM_USER").ToString())
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
    'Commented & Added by TCS on 17012023, Commented Old Code & Added new Excel download code
    Protected Sub ExportExcel()
        Dim dt As New DataTable("Survey Report")
        For Each cell As TableCell In gdvselectAssesor.HeaderRow.Cells
            dt.Columns.Add(HttpUtility.HtmlDecode(cell.Text))
        Next
        For Each row As GridViewRow In gdvselectAssesor.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 1
                dt.Rows(dt.Rows.Count - 1)(i) = HttpUtility.HtmlDecode(row.Cells(i).Text)
            Next
        Next
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt)

            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=Status_Survey_Completion_" + DateTime.Now.ToString("yyyyMMddHHmmss") & ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.End()
            End Using
        End Using
    End Sub
    'End
End Class
