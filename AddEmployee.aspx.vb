Imports System.Data.OracleClient
Imports System.Data
Partial Class AddEmployee
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "ADD EMPLOYEE"
                getFy()
                getsrlno()
                bindDepartment()
                bindExecHead()
                bindSubArea()
                bindDesignation()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Clear()
        txtPerno.Text = ""
        txtSelfName.Text = ""
        ddlDesg.SelectedIndex = 0
        txtEmail.Text = ""
        txtEquLvl.Text = ""
        txtContactNo.Text = ""
        ddlSubarea.SelectedIndex = 0
        txtReportingTo.Text = ""
        txtBuhrNo.Text = ""
        txtBuhrNm.Text = ""
        txtJoiningDt.Text = ""
        ddlDepartment.SelectedIndex = 0
        txtEmpSgrade.Text = ""
        TxtEmpClass.Text = ""
        txtDotted.Text = ""
        txtEmpPersExec.Text = ""
        ddlExecHead.SelectedIndex = 0
        txtStep1StDt.Text = ""
        txtStep1EndDt.Text = ""
        txtStep2StDt.Text = ""
        txtStep2EndDt.Text = ""
        txtStep3StDt.Text = ""
        txtStep3EndDt.Text = ""
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
                ddlExecHead.DataSource = dtExecHead
                ddlExecHead.DataValueField = "ema_exec_head"
                ddlExecHead.DataTextField = "ema_exec_head_desc"
                ddlExecHead.DataBind()
                ddlExecHead.Items.Insert(0, New ListItem("--Select--", "--Select--"))
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
            ddlDepartment.Items.Clear()
            query.CommandText = "select distinct ema_dept_code DEPT,ema_dept_desc DEPTDESC from hrps.t_emp_master_feedback360 where ema_comp_code='1000'  and ema_dept_desc<>'Not found' "
            If ChkRole1() = False Then
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())
            End If
            query.CommandText += " order by ema_dept_desc"


            'Dim qry = New OracleCommand(query, conHrps)
            dtDept = getDataInDt(query)
            If dtDept.Rows.Count > 0 Then
                ddlDepartment.DataSource = dtDept
                ddlDepartment.DataValueField = "DEPT"
                ddlDepartment.DataTextField = "DEPTDESC"
                ddlDepartment.DataBind()
                ddlDepartment.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            Else
                ddlDepartment.DataSource = dtDept
                ddlDepartment.DataBind()
                ddlDepartment.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If


        Catch ex As Exception

        End Try
    End Sub
    Private Sub bindSubArea()
        Try
            Dim dtSubArea As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct EMA_PERS_SUBAREA,EMA_PERS_SUBAREA_DESC from hrps.t_emp_master_feedback360 where EMA_PERS_SUBAREA_DESC is not null and EMA_PERS_SUBAREA_DESC<>'Not found' and EMA_PERS_SUBAREA<>'00000000' and ema_comp_code='1000' "
            If ChkRole1() = False Then
                query.Parameters.Clear()
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())

            End If
            query.CommandText += " order by EMA_PERS_SUBAREA_DESC"
            'Dim qry = New OracleCommand(query, conHrps)
            dtSubArea = getDataInDt(query)
            If dtSubArea.Rows.Count > 0 Then
                ddlSubarea.DataSource = dtSubArea
                ddlSubarea.DataValueField = "EMA_PERS_SUBAREA"
                ddlSubarea.DataTextField = "EMA_PERS_SUBAREA_DESC"
                ddlSubarea.DataBind()
                ddlSubarea.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bindDesignation()
        Try
            Dim dtDesignation As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct EMA_DESGN_CODE,EMA_DESGN_DESC from hrps.t_emp_master_feedback360 where EMA_DESGN_DESC is not null and EMA_DESGN_DESC<>'Not found' and EMA_DESGN_CODE<>'00000000' and ema_comp_code='1000' "
            If ChkRole1() = False Then
                query.Parameters.Clear()
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())

            End If
            query.CommandText += " order by EMA_DESGN_DESC"
            'Dim qry = New OracleCommand(query, conHrps)
            dtDesignation = getDataInDt(query)
            If dtDesignation.Rows.Count > 0 Then
                ddlDesg.DataSource = dtDesignation
                ddlDesg.DataValueField = "EMA_DESGN_CODE"
                ddlDesg.DataTextField = "EMA_DESGN_DESC"
                ddlDesg.DataBind()
                ddlDesg.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If
        Catch ex As Exception

        End Try
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
    <System.Web.Services.WebMethod>
    Public Shared Function GetCustomers(ByVal prefix As String) As String()
        Dim customers As New List(Of String)()
        If prefix.Length > 2 Then
            Using conn As New OracleConnection()
                conn.ConnectionString = ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString
                Using cmd As New OracleCommand()
                    cmd.CommandText = " select EMA_ENAME||'('||EMA_PERNO||')' empPerno from tips.T_empl_all where (EMA_PERNO like  :ema_perno or upper(EMA_ENAME) like "
                    cmd.CommandText += " :ema_ename) "
                    conn.Open()

                    cmd.Connection = conn
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("ema_perno", "%" & prefix.ToUpper & "%")
                    cmd.Parameters.AddWithValue("ema_ename", "%" & prefix.ToUpper & "%")
                    'Dim prefixes As List(Of String) = New List(Of String)
                    Dim sdr As OracleDataReader = cmd.ExecuteReader
                    While sdr.Read
                        customers.Add(sdr("empPerno"))
                    End While
                    conn.Close()
                End Using
                Return customers.ToArray()
            End Using
        End If



    End Function
    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim words As String() = txtSearch.Text.Trim().Split(New Char() {"("c})
            Dim pno = words(1).Replace(")", "").ToString
            Dim sqltrntype1 As String = "select ema_perno,ema_ename,ema_desgn_code,ema_email_id,ema_eqv_level,ema_phone_no,ema_pers_subarea,ema_reporting_to_pno,ema_bhr_pno,ema_bhr_name,to_char(EMA_JOINING_DT,'dd-Mon-yyyy') EMA_JOINING_DT,EMA_DEPT_CODE,EMA_EMPL_SGRADE,EMA_EMP_CLASS,EMA_DOTTED_PNO,EMA_PERS_EXEC_PNO,EMA_EXEC_HEAD,to_char(EMA_STEP1_STDT,'dd-Mon-yyyy') EMA_STEP1_STDT,to_char(EMA_STEP1_ENDDT,'dd-Mon-yyyy') EMA_STEP1_ENDDT,to_char(EMA_STEP2_STDT,'dd-Mon-yyyy') EMA_STEP2_STDT,to_char(EMA_STEP2_ENDDT,'dd-Mon-yyyy') EMA_STEP2_ENDDT,to_char(EMA_STEP3_STDT,'dd-Mon-yyyy') EMA_STEP3_STDT,to_char(EMA_STEP3_ENDDT,'dd-Mon-yyyy') EMA_STEP3_ENDDT from hrps.T_EMP_MASTER_FEEDBACK360 where ema_perno =:pno and EMA_YEAR=:EMA_YEAR and EMA_CYCLE=:EMA_CYCLE"

            Dim dttrntype As New DataTable()
            Dim myCommand As New OracleCommand()
            myCommand.CommandText = sqltrntype1
            myCommand.Connection = conHrps
            myCommand.Parameters.Clear()
            myCommand.Parameters.AddWithValue("pno", pno.ToString())
            myCommand.Parameters.AddWithValue("EMA_YEAR", txtYear.Text.Trim().ToString())
            myCommand.Parameters.AddWithValue("EMA_CYCLE", txtCycle.Text.Trim().ToString())
            dttrntype = getDataInDt(myCommand)
            If dttrntype.Rows.Count > 0 Then
                txtPerno.Text = dttrntype.Rows(0)("ema_perno").ToString
                txtSelfName.Text = dttrntype.Rows(0)("ema_ename").ToString
                ddlDesg.Text = dttrntype.Rows(0)("ema_desgn_code").ToString
                txtEmail.Text = dttrntype.Rows(0)("ema_email_id").ToString
                txtEquLvl.Text = dttrntype.Rows(0)("ema_eqv_level").ToString
                txtContactNo.Text = dttrntype.Rows(0)("ema_phone_no").ToString
                ddlSubarea.Text = dttrntype.Rows(0)("ema_pers_subarea").ToString
                txtReportingTo.Text = dttrntype.Rows(0)("ema_reporting_to_pno").ToString
                txtBuhrNo.Text = dttrntype.Rows(0)("ema_bhr_pno").ToString
                txtBuhrNm.Text = dttrntype.Rows(0)("ema_bhr_name").ToString
                txtJoiningDt.Text = dttrntype.Rows(0)("EMA_JOINING_DT").ToString
                ddlDepartment.Text = dttrntype.Rows(0)("EMA_DEPT_CODE").ToString
                txtEmpSgrade.Text = dttrntype.Rows(0)("EMA_EMPL_SGRADE").ToString
                TxtEmpClass.Text = dttrntype.Rows(0)("EMA_EMP_CLASS").ToString
                txtDotted.Text = dttrntype.Rows(0)("EMA_DOTTED_PNO").ToString
                txtEmpPersExec.Text = dttrntype.Rows(0)("EMA_PERS_EXEC_PNO").ToString
                ddlExecHead.Text = dttrntype.Rows(0)("EMA_EXEC_HEAD").ToString
                txtStep1StDt.Text = dttrntype.Rows(0)("EMA_STEP1_STDT").ToString
                txtStep1EndDt.Text = dttrntype.Rows(0)("EMA_STEP1_ENDDT").ToString
                txtStep2StDt.Text = dttrntype.Rows(0)("EMA_STEP2_STDT").ToString
                txtStep2EndDt.Text = dttrntype.Rows(0)("EMA_STEP2_ENDDT").ToString
                txtStep3StDt.Text = dttrntype.Rows(0)("EMA_STEP3_STDT").ToString
                txtStep3EndDt.Text = dttrntype.Rows(0)("EMA_STEP3_ENDDT").ToString
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub txtPerno_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim words As String() = txtPerno.Text.Trim().Split(New Char() {"("c})
            Dim pno = words(1).Replace(")", "").ToString

            Dim sqltrntype2 As String = "select * from hrps.T_EMP_MASTER_FEEDBACK360 where ema_perno =:pno and EMA_YEAR=:EMA_YEAR and EMA_CYCLE=:EMA_CYCLE"

            Dim dtSearch As New DataTable()
            Dim Command As New OracleCommand()
            Command.CommandText = sqltrntype2
            Command.Connection = conHrps
            Command.Parameters.Clear()
            Command.Parameters.AddWithValue("pno", pno.ToString())
            Command.Parameters.AddWithValue("EMA_YEAR", txtYear.Text.Trim().ToString())
            Command.Parameters.AddWithValue("EMA_CYCLE", txtCycle.Text.Trim().ToString())
            dtSearch = getDataInDt(Command)
            If dtSearch.Rows.Count > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Already record exists in database")
                Clear()
                Exit Sub
            End If
            Clear()
            Dim sqltrntype1 As String = "select ema_perno,ema_ename,ema_desgn_code,ema_email_id,ema_eqv_level,ema_phone_no,ema_pers_subarea,ema_reporting_to_pno,ema_bhr_pno,ema_bhr_name,to_char(EMA_JOINING_DT,'dd-Mon-yyyy') EMA_JOINING_DT,EMA_DEPT_CODE,EMA_EMPL_SGRADE,EMA_EMP_CLASS,EMA_DOTTED_PNO,EMA_PERS_EXEC_PNO,EMA_EXEC_HEAD from tips.T_EMPL_ALL where ema_perno =:pno"

            Dim dttrntype As New DataTable()
            Dim myCommand As New OracleCommand()
            myCommand.CommandText = sqltrntype1
            myCommand.Connection = conHrps
            myCommand.Parameters.Clear()
            myCommand.Parameters.AddWithValue("pno", pno.ToString())
            dttrntype = getDataInDt(myCommand)
            If dttrntype.Rows.Count > 0 Then
                txtPerno.Text = dttrntype.Rows(0)("ema_perno").ToString
                txtSelfName.Text = dttrntype.Rows(0)("ema_ename").ToString
                ddlDesg.Text = dttrntype.Rows(0)("ema_desgn_code").ToString
                txtEmail.Text = dttrntype.Rows(0)("ema_email_id").ToString
                txtEquLvl.Text = dttrntype.Rows(0)("ema_eqv_level").ToString
                txtContactNo.Text = dttrntype.Rows(0)("ema_phone_no").ToString
                ddlSubarea.Text = dttrntype.Rows(0)("ema_pers_subarea").ToString
                txtReportingTo.Text = dttrntype.Rows(0)("ema_reporting_to_pno").ToString
                txtBuhrNo.Text = dttrntype.Rows(0)("ema_bhr_pno").ToString
                txtBuhrNm.Text = dttrntype.Rows(0)("ema_bhr_name").ToString
                txtJoiningDt.Text = dttrntype.Rows(0)("EMA_JOINING_DT").ToString
                ddlDepartment.Text = dttrntype.Rows(0)("EMA_DEPT_CODE").ToString
                txtEmpSgrade.Text = dttrntype.Rows(0)("EMA_EMPL_SGRADE").ToString
                TxtEmpClass.Text = dttrntype.Rows(0)("EMA_EMP_CLASS").ToString
                txtDotted.Text = dttrntype.Rows(0)("EMA_DOTTED_PNO").ToString
                txtEmpPersExec.Text = dttrntype.Rows(0)("EMA_PERS_EXEC_PNO").ToString
                ddlExecHead.Text = dttrntype.Rows(0)("EMA_EXEC_HEAD").ToString
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lbtnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnNew.Click
        lbtnNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#3c8dbc")
        lbtnNew.BorderColor = System.Drawing.ColorTranslator.FromHtml("#367fa9")
        lbtnNew.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fffefe")

        lbtnEdit.BackColor = System.Drawing.ColorTranslator.FromHtml("#f4f4f4")
        lbtnEdit.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ddd")
        lbtnEdit.ForeColor = System.Drawing.ColorTranslator.FromHtml("#444")
        txtSearch.Text = ""
        txtSearch.Visible = False
        txtPerno.Enabled = True
        lbtnUpdate.Visible = False
        lbtnSubmit.Visible = True
        Clear()
    End Sub
    Protected Sub lbtnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnEdit.Click
        ' lbtnNew.Attributes.Add("cssClass", "btn btn-secondary")
        'lbtnEdit.Attributes.Add("cssClass", "btn btn-primary")

        lbtnEdit.BackColor = System.Drawing.ColorTranslator.FromHtml("#3c8dbc")
        lbtnEdit.BorderColor = System.Drawing.ColorTranslator.FromHtml("#367fa9")
        lbtnEdit.ForeColor = System.Drawing.ColorTranslator.FromHtml("#fffefe")

        lbtnNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#f4f4f4")
        lbtnNew.BorderColor = System.Drawing.ColorTranslator.FromHtml("#ddd")
        lbtnNew.ForeColor = System.Drawing.ColorTranslator.FromHtml("#444")
        txtSearch.Text = ""
        txtSearch.Visible = True
        txtPerno.Enabled = False
        lbtnUpdate.Visible = True
        lbtnSubmit.Visible = False
        Clear()
    End Sub
    Protected Sub lbtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnSubmit.Click
        Try
            Dim sqlRequest As String = " INSERT INTO expmgmt.T_SEP_EMP (ema_year,ema_cycle,ema_perno,ema_ename,ema_desgn_code,ema_desgn_desc,ema_email_id,ema_eqv_level,ema_phone_no,ema_pers_subarea,ema_pers_subarea_desc,ema_comp_code,ema_reporting_to_pno,ema_bhr_pno,ema_bhr_name,ema_joining_dt,ema_dept_code,ema_dept_desc,ema_empl_sgrade,ema_emp_class,ema_dotted_pno,ema_pers_exec_pno,ema_exec_head,ema_exec_head_desc,ema_step1_stdt,ema_step1_enddt,ema_step2_stdt,ema_step2_enddt,ema_step3_stdt,ema_step3_enddt) VALUES "
            'sqlRequest = sqlRequest & " (:se_emp_perno,:se_emp_name,to_date(:se_emp_dob,'dd-mm-yyyy'),:se_exp,to_date(:se_retire_dt,'dd-mm-yyyy'),:se_email_id,:se_contact_no,:se_spouse_name,to_date(:se_spouse_dob,'dd-mm-yyyy'),:se_parent1_name,to_date(:se_parent1_dob,'dd-mm-yyyy'),:se_parent2_name,to_date(:se_parent2_dob,'dd-mm-yyyy'),:se_flag,:se_crt_by)"
            sqlRequest = sqlRequest & " (:ema_year,:ema_cycle,:ema_perno,:ema_ename,:ema_desgn_code,:ema_desgn_desc,:ema_email_id,:ema_eqv_level,:ema_phone_no,:ema_pers_subarea,:ema_pers_subarea_desc,:ema_comp_code,:ema_reporting_to_pno,:ema_bhr_pno,:ema_bhr_name,to_date(:ema_joining_dt,'dd-mm-yyyy'),:ema_dept_code,:ema_dept_desc,:ema_empl_sgrade,:ema_emp_class,:ema_dotted_pno,:ema_pers_exec_pno,:ema_exec_head,:ema_exec_head_desc,to_date(:ema_step1_stdt,'dd-mm-yyyy'),to_date(:ema_step1_enddt,'dd-mm-yyyy'),to_date(:ema_step2_stdt,'dd-mm-yyyy'),to_date(:ema_step2_enddt,'dd-mm-yyyy'),to_date(:ema_step3_stdt,'dd-mm-yyyy'),to_date(:ema_step3_enddt,'dd-mm-yyyy'))"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim cmd As OracleCommand = New OracleCommand()
            cmd.Connection = conHrps

            cmd.CommandText = sqlRequest
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_year", txtYear.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_cycle", txtCycle.Text.Trim.ToString)
            cmd.Parameters.AddWithValue("ema_perno", txtPerno.Text)
            cmd.Parameters.AddWithValue("ema_ename", txtSelfName.Text.Trim)
            cmd.Parameters.AddWithValue("ema_desgn_code", ddlDesg.SelectedValue.ToString)
            cmd.Parameters.AddWithValue("ema_desgn_desc", ddlDesg.SelectedItem.Text.ToString)
            cmd.Parameters.AddWithValue("ema_email_id", txtEmail.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_eqv_level", txtEquLvl.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_phone_no", txtContactNo.Text)
            cmd.Parameters.AddWithValue("ema_pers_subarea", ddlSubarea.SelectedValue.ToString)
            cmd.Parameters.AddWithValue("ema_pers_subarea_desc", ddlSubarea.SelectedItem.Text.ToString)
            cmd.Parameters.AddWithValue("ema_comp_code", "1000")
            cmd.Parameters.AddWithValue("ema_reporting_to_pno", txtReportingTo.Text)
            cmd.Parameters.AddWithValue("ema_bhr_pno", txtBuhrNo.Text)
            cmd.Parameters.AddWithValue("ema_bhr_name", txtBuhrNm.Text.Trim.ToString)
            cmd.Parameters.AddWithValue("ema_joining_dt", txtJoiningDt.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_dept_code", ddlDepartment.SelectedValue.ToString)
            cmd.Parameters.AddWithValue("ema_dept_desc", ddlDepartment.SelectedItem.Text.ToString)
            cmd.Parameters.AddWithValue("ema_empl_sgrade", txtEmpSgrade.Text.ToString)
            cmd.Parameters.AddWithValue("ema_emp_class", TxtEmpClass.Text)
            cmd.Parameters.AddWithValue("ema_dotted_pno", txtDotted.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_pers_exec_pno", txtEmpPersExec.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_exec_head", ddlExecHead.SelectedValue.ToString)
            cmd.Parameters.AddWithValue("ema_exec_head_desc", ddlExecHead.SelectedItem.Text.ToString)
            cmd.Parameters.AddWithValue("ema_step1_stdt", txtStep1StDt.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_step1_enddt", txtStep1EndDt.Text)
            cmd.Parameters.AddWithValue("ema_step2_stdt", txtStep2StDt.Text.Trim().ToString)
            cmd.Parameters.AddWithValue("ema_step2_enddt", txtStep2EndDt.Text)
            cmd.Parameters.AddWithValue("ema_step3_stdt", txtStep3StDt.Text.Trim)
            cmd.Parameters.AddWithValue("ema_step3_enddt", txtStep3EndDt.Text.Trim().ToString)
            Dim valid = cmd.ExecuteNonQuery()
            If valid > 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "savedtlserror", "logout('Record save successfully.');", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "savedtlserror", "logout('Record not saved.');", True)
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

End Class
