Imports System.Data.OracleClient
Imports System.Data
Partial Class UserCreation
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim comobj As New CommonVb()
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "Assign Admin Role"
                'bind_Admin(txtpnoI.Text.Trim())
                popiRadios.SelectedValue = "I"
                bind_Admin("", "", "P")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub txtpnoI_TextChanged(sender As Object, e As EventArgs) Handles txtpnoI.TextChanged
        If (txtpnoI.Text.Trim().Length <= 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
            txtpnoI.Focus()
            Exit Sub
        End If
        bind_Data(txtpnoI.Text.Trim())
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Private Function bind_Data(id As String) As DataTable
        Try

            Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
                Using cmd As New OracleCommand()

                    cmd.CommandText = "select upper(IGP_ADM_NAME ||'('|| IGP_USER_ID ||')') user_id,ema_perno,IGP_MODE, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,IGP_USER_ID,to_char(IGP_CHANGE_DATE,'dd/MM/yyyy') IGP_CHANGE_DATE,IGP_STATUS,IGP_REMARKS,IGP_EMAIL_ID,IGP_ADM_NAME,'I' USER_TYPE,'E' call_from  from t_emp_master_feedback360 em left join T_IR_ADM_GRP_PRIVILEGE ap on em.ema_perno=ap.IGP_USER_ID where ema_comp_code='1000'"
                    If (id.Trim().Length > 0) Then
                        cmd.CommandText += " and (ema_ename||'(' ||ema_perno ||')')=:ema_perno"
                    End If
                    'cmd.CommandText += ""
                    cmd.Connection = conHrps
                    cmd.Parameters.Clear()
                    If (id.Trim().Length > 0) Then
                        cmd.Parameters.AddWithValue("ema_perno", id.Trim())
                    End If

                    Dim da As New OracleDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    If (dt.Rows.Count > 0) Then
                        gdv_Create_User.DataSource = dt
                        gdv_Create_User.DataBind()
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.warning, "No Data found in employee master.")

                    End If
                End Using
            End Using


        Catch ex As Exception

        End Try
    End Function
    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New UserCreation
        Dim cmd As New OracleCommand()
        Try



            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select distinct ema_ename ||'('|| ema_perno ||')' EName from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and (ema_perno like '%" & prefixText & "%' or upper(ema_ename) like "
            ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
            cmd.CommandText += " '%" & prefixText.ToUpper & "%')"   'WI368 add officer class
            'End by Manoj Kumar

            cmd.Connection = ob.conHrps
            ob.conHrps.Open()
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader

            While sdr.Read
                prefixes.Add(sdr("EName").ToString)
            End While



            Return prefixes
        Catch ex As Exception

            Return Nothing

        Finally
            If (ob.conHrps.State = ConnectionState.Open) Then
                ob.conHrps.Close()
            End If
            cmd.Dispose()
        End Try

    End Function
    Protected Sub gdv_Create_User_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gdv_Create_User.RowDataBound
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim ddl_status As DropDownList = CType(e.Row.FindControl("ddl_Active_deactive"), DropDownList)
                Dim btnADD As Button = CType(e.Row.FindControl("btnADD"), Button)
                Dim btn_Update_Admin As Button = CType(e.Row.FindControl("btn_Update_Admin"), Button)

                Dim txt_IGP_EMAIL_ID As TextBox = CType(e.Row.FindControl("txt_IGP_EMAIL_ID"), TextBox)
                Dim lblEMA_EMAIL_ID As Label = CType(e.Row.FindControl("lblEMA_EMAIL_ID"), Label)

                Dim txt_IGP_ADM_NAME As TextBox = CType(e.Row.FindControl("txt_IGP_ADM_NAME"), TextBox)
                Dim lblema_ename As Label = CType(e.Row.FindControl("lblema_ename"), Label)


                e.Row.Cells(4).Attributes.Add("style", "word-break:break-all;word-wrap:break-word;")
                If (DataBinder.Eval(e.Row.DataItem, "IGP_MODE").ToString().Trim().ToUpper() = "E") Then
                    txt_IGP_EMAIL_ID.Visible = True
                    lblEMA_EMAIL_ID.Visible = False

                    txt_IGP_ADM_NAME.Visible = True
                    lblema_ename.Visible = False

                Else
                    txt_IGP_EMAIL_ID.Visible = False
                    lblEMA_EMAIL_ID.Visible = True

                    txt_IGP_ADM_NAME.Visible = False
                    lblema_ename.Visible = True
                End If

                If (DataBinder.Eval(e.Row.DataItem, "IGP_STATUS").ToString().Trim().Length > 0) Then
                    ddl_status.SelectedValue = DataBinder.Eval(e.Row.DataItem, "IGP_STATUS").ToString().Trim()
                    btn_Update_Admin.Visible = True
                    btnADD.Visible = False
                    btn_Update_Admin.Text = "Update"
                Else
                    btnADD.Visible = True
                    btn_Update_Admin.Visible = False
                    btn_Update_Admin.Text = "Edit"
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub gdv_Create_User_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdv_Create_User.RowCommand
        Try
            Dim val As Integer = 0
            Dim btn As Button = CType(e.CommandSource, Button)
            Dim grv As GridViewRow = CType(btn.Parent.Parent, GridViewRow)
            Dim perno As String = CType(grv.FindControl("lblema_perno"), Label).Text
            Dim user_id As String = CType(grv.FindControl("lbl_user_id"), Label).Text
            Dim remarks As String = CType(grv.FindControl("txt_IGP_REMARKS"), TextBox).Text
            Dim name As String = CType(grv.FindControl("txt_IGP_ADM_NAME"), TextBox).Text
            Dim email As String = CType(grv.FindControl("txt_IGP_EMAIL_ID"), TextBox).Text
            Dim user_type As String = CType(grv.FindControl("lblUSER_TYPE"), Label).Text
            Dim ddl_status As DropDownList = CType(grv.FindControl("ddl_Active_deactive"), DropDownList)
            Dim btn_Update_Admin As Button = CType(grv.FindControl("btn_Update_Admin"), Button)

            Dim rex As New Regex("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
            If (user_type.Trim() = "E") Then
                If (email.Trim().Length <= 0) Then
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Please enter email id.")
                    Return
                Else
                    If (rex.Match(email.Trim()).Success) Then
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.error, "Please enter valid email id.")
                        Return
                    End If
                End If
                If (name.Trim().Length <= 0) Then
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Name Should not be left blank.")
                    Return
                ElseIf (comobj.ISI(name)) Then
                    CType(grv.FindControl("txt_IGP_ADM_NAME"), TextBox).Focus()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
                    Exit Sub
                End If
            End If

            remarks = "ADM"


            If (user_id.Trim().Length <= 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Employee Per. No. should not be blank.")
                Return
            End If
            'If (remarks.Trim().Length <= 0) Then
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Remarks should not be blank.")
            '    Return
            'End If
            If (ddl_status.SelectedValue.Trim().Length <= 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Status should not be blank.")
                Return
            End If
            If (e.CommandName.Trim() = "ADD") Then
                val = Manage_User("1000", "360FEEDBAC", "FB", perno.Trim(), "0", popiRadios.SelectedValue.Trim(), ddl_status.SelectedValue.Trim(), remarks.Trim(), "I", "", "", "I")
                If (val > 0) Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Successfully Added.")
                End If
            ElseIf (e.CommandName.Trim() = "_UPDATE") Then
                If (bind_Admin(email.Trim(), user_id, "P") = True) Then
                    'txtEmail.Focus()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "This email id is already exist with another user.")
                    Exit Sub
                End If
                val = Manage_User("1000", "360FEEDBAC", "FB", perno.Trim(), "0", popiRadios.SelectedValue.Trim(), ddl_status.SelectedValue.Trim(), remarks.Trim(), "U", IIf(user_type.Trim() = "I", "", email.Trim()), IIf(user_type.Trim() = "I", "", name.Trim), user_type.Trim())
                If (val > 0) Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Successfully Updated.")
                    bind_Admin("", user_id, "P")
                End If
            End If

            'End If
            If (user_type.Trim().ToUpper() = "E") Then
                bind_Admin("", txtPAN.Text.Trim(), "P")
            ElseIf (user_type.Trim().ToUpper() = "I") Then
                bind_Admin("", txtpnoI.Text.Trim(), "P")
            End If

        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.Message.ToString())
        End Try

    End Sub
    Private Function Manage_User(LOCATION As String, GROUP_ID As String, MODULE_ID As String, USER_ID As String, DEPT As String, MODE As String, Status As String, Remarks As String, action As String, email_id As String, name As String, user_Type As String) As Integer
        Dim ret_value As Integer = 0
        'Try
        Dim sb As New StringBuilder()
        If (action = "I") Then
            sb.Append("Insert into T_IR_ADM_GRP_PRIVILEGE(IGP_LOCATION,IGP_GROUP_ID,IGP_MODULE_ID,IGP_USER_ID,IGP_DEPT_CD,IGP_MODE,IGP_STATUS,IGP_CHANGE_DATE,IGP_CHANGE_USER,IGP_REMARKS")
            If (user_Type = "E") Then 'E means external
                sb.Append(",IGP_EMAIL_ID,IGP_ADM_NAME")
            End If
            sb.Append(") values(:IGP_LOCATION,:IGP_GROUP_ID,:IGP_MODULE_ID,:IGP_USER_ID,:IGP_DEPT_CD,:IGP_MODE,:IGP_STATUS,sysdate,:IGP_CHANGE_USER,:IGP_REMARKS")
            If (user_Type = "E") Then
                sb.Append(",:IGP_EMAIL_ID,:IGP_ADM_NAME")
            End If
            sb.Append(")")
        ElseIf (action = "U") Then
            sb.Append("Update T_IR_ADM_GRP_PRIVILEGE set IGP_STATUS=:IGP_STATUS,IGP_REMARKS=:IGP_REMARKS,IGP_CHANGE_DATE=sysdate,IGP_CHANGE_USER=:IGP_CHANGE_USER  ")
            If (user_Type = "E") Then
                sb.Append(",IGP_EMAIL_ID=:IGP_EMAIL_ID,IGP_ADM_NAME=:IGP_ADM_NAME")
            End If
            sb.Append(" where IGP_LOCATION=:IGP_LOCATION and IGP_GROUP_ID=:IGP_GROUP_ID and IGP_MODULE_ID=:IGP_MODULE_ID and IGP_USER_ID=upper(:IGP_USER_ID) and IGP_DEPT_CD=:IGP_DEPT_CD And IGP_MODE=:IGP_MODE")
        End If
        Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

            Using cmd As New OracleCommand()
                cmd.Connection = con
                cmd.CommandText = sb.ToString().Trim()

                cmd.Parameters.Add(New OracleParameter("IGP_LOCATION", OracleType.VarChar)).Value = LOCATION.Trim()
                cmd.Parameters.Add(New OracleParameter("IGP_GROUP_ID", OracleType.VarChar)).Value = GROUP_ID.Trim()
                cmd.Parameters.Add(New OracleParameter("IGP_MODULE_ID", OracleType.VarChar)).Value = MODULE_ID.Trim()
                cmd.Parameters.Add(New OracleParameter("IGP_USER_ID", OracleType.VarChar)).Value = USER_ID.Trim()
                cmd.Parameters.Add(New OracleParameter("IGP_DEPT_CD", OracleType.VarChar)).Value = DEPT.Trim()

                cmd.Parameters.Add(New OracleParameter("IGP_MODE", OracleType.VarChar)).Value = MODE.Trim()

                cmd.Parameters.Add(New OracleParameter("IGP_STATUS", OracleType.VarChar)).Value = Status.Trim()
                cmd.Parameters.Add(New OracleParameter("IGP_CHANGE_USER", OracleType.VarChar)).Value = Session("USER_ID").ToString().Trim()
                cmd.Parameters.Add(New OracleParameter("IGP_REMARKS", OracleType.VarChar)).Value = Remarks.Trim()
                If (user_Type.Trim() = "E") Then
                    cmd.Parameters.Add(New OracleParameter("IGP_EMAIL_ID", OracleType.VarChar)).Value = email_id.Trim()
                    cmd.Parameters.Add(New OracleParameter("IGP_ADM_NAME", OracleType.VarChar)).Value = name.Trim()
                End If

                If (con.State = ConnectionState.Closed) Then
                    con.Open()
                End If
                ret_value = cmd.ExecuteNonQuery()
                con.Close()
                Return ret_value
            End Using
        End Using
        'Catch ex As Exception
        '    Return -1
        'End Try
    End Function
    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        txtpnoI.Text = String.Empty
        div_int.Visible = False
        div_ext.Visible = False
        popiRadios.ClearSelection()
        bind_Admin("", "", "P")
        clear_control()
    End Sub
    Private Sub clear_control()
        txtPAN.Text = String.Empty
        'txtRemarks.Text = String.Empty
        txtName.Text = String.Empty
        txtEmail.Text = String.Empty
        ddl_Ext_Status.SelectedIndex = 0
        'txtRemarks.Enabled = True
        ddl_Ext_Status.Enabled = True
        btnAssign.Enabled = True
    End Sub
    Protected Sub btnAssign_Click(sender As Object, e As EventArgs) Handles btnAssign.Click
        Try

            If (txtPAN.Text.Trim().Length <= 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Enter Valid Employee ID.")
                Return
            ElseIf (txtPAN.Text.Trim().Length > 0) Then
                If (comobj.ISI(txtPAN.Text.Trim().ToUpper)) Then
                    txtPAN.Focus()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
                    Exit Sub
                End If
            End If
            If (ddl_Ext_Status.SelectedIndex <= 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Select Status.")
                Return
            End If
            If (txtName.Text.Trim().Length <= 0) Then
                txtName.Focus()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please enter name.")
                Exit Sub
            End If
            If (comobj.ISI(txtName.Text.Trim())) Then
                txtName.Focus()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
                Exit Sub
            End If
            If (txtEmail.Text.Trim().Length <= 0) Then
                txtEmail.Focus()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please enter email id.")
                Exit Sub
            Else
                If (bind_Admin(txtEmail.Text.Trim(), "", "P") = True) Then
                    txtEmail.Focus()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "This email id is already exist with another user.")
                    Exit Sub
                End If
            End If
            'If (txtRemarks.Text.Trim().Length <= 0) Then
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Enter Remarks.")
            '    Return
            'End If

            Dim Val = Manage_User("1000", "360FEEDBAC", "FB", txtPAN.Text.Trim().ToUpper(), "0", popiRadios.SelectedValue.Trim(), ddl_Ext_Status.SelectedValue.Trim(), "ADM", "I", txtEmail.Text.Trim(), txtName.Text.Trim(), "E")
            If (Val > 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Successfully Added.")
                bind_Admin("", "", "P")
                clear_control()

            End If
        Catch oex As OracleException
            If (oex.Code = 1) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Data already exist.")
                clear_control()
            End If
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error.")
        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForADMIN(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New UserCreation
        Dim cmd As New OracleCommand()
        Try



            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select IGP_ADM_NAME ||'('|| IGP_USER_ID ||')' IGP_USER_ID from hrps.T_IR_ADM_GRP_PRIVILEGE where IGP_USER_ID like "

            cmd.CommandText += " ('%" & prefixText.ToUpper & "%') or upper(IGP_ADM_NAME) like ('%" & prefixText.ToUpper & "%') "   'WI368 add officer class


            cmd.Connection = ob.conHrps
            ob.conHrps.Open()
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader

            While sdr.Read
                prefixes.Add(sdr("IGP_USER_ID").ToString)
            End While



            Return prefixes
        Catch ex As Exception

            Return Nothing

        Finally
            If (ob.conHrps.State = ConnectionState.Open) Then
                ob.conHrps.Close()
            End If
            cmd.Dispose()
        End Try

    End Function
    Protected Sub txtPAN_TextChanged(sender As Object, e As EventArgs) Handles txtPAN.TextChanged
        bind_Admin("", txtPAN.Text.Trim(), "E")
    End Sub
    Protected Sub popiRadios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles popiRadios.SelectedIndexChanged
        Try
            If (popiRadios.SelectedValue = "I") Then
                div_int.Visible = True
                div_ext.Visible = False
            ElseIf (popiRadios.SelectedValue = "E") Then
                div_ext.Visible = True
                div_int.Visible = False
            End If
            bind_Admin("", "", "E")
            'gdv_Create_User.DataSource = Nothing
            'gdv_Create_User.DataBind()
            'btnReset_Click(sender, e)
            clear_control()
        Catch ex As Exception

        End Try
    End Sub

    Private Function bind_Admin(email As String, USER_ID As String, call_from As String) As Boolean
        Try
            Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

                Using cmd As New OracleCommand()
                    cmd.CommandText = "select upper(IGP_ADM_NAME ||'('|| IGP_USER_ID ||')') user_id,IGP_MODE,IGP_USER_ID ema_perno,case when IGP_ADM_NAME is null then EMA_ENAME else IGP_ADM_NAME end ema_ename,EMA_DESGN_DESC, EMA_DEPT_DESC,case when IGP_EMAIL_ID is null then EMA_EMAIL_ID else IGP_EMAIL_ID end EMA_EMAIL_ID,IGP_EMAIL_ID, IGP_USER_ID,IGP_USER_ID,to_char(IGP_CHANGE_DATE,'dd/MM/yyyy') IGP_CHANGE_DATE,IGP_STATUS,IGP_REMARKS,IGP_ADM_NAME,case when IGP_EMAIL_ID is null and IGP_ADM_NAME is null then 'I' else 'E' end USER_TYPE,'" + call_from + "' call_from from T_IR_ADM_GRP_PRIVILEGE p left join t_emp_master_feedback360 em on p.IGP_USER_ID=em.EMA_PERNO where 1=1 "
                    If (USER_ID.Trim().Length > 0 And email.Trim().Length <= 0) Then
                        cmd.CommandText += " and upper(IGP_ADM_NAME ||'('|| IGP_USER_ID ||')')=upper(:IGP_USER_ID)"
                    End If
                    If (email.Trim().Length > 0 And USER_ID.Trim().Length > 0) Then
                        cmd.CommandText += " and upper(IGP_EMAIL_ID)=upper(:email) and upper(IGP_ADM_NAME ||'('|| IGP_USER_ID ||')')<>upper(:IGP_USER_ID)"
                    End If
                    cmd.CommandText += " and IGP_MODE=:IGP_MODE"

                    cmd.CommandText += " order by IGP_CHANGE_DATE desc"
                    cmd.Connection = conHrps
                    cmd.Parameters.Clear()
                    If (USER_ID.Trim().Length > 0) Then
                        cmd.Parameters.AddWithValue("IGP_USER_ID", USER_ID.Trim())
                    End If
                    If (email.Trim().Length > 0) Then
                        cmd.Parameters.AddWithValue("email", email.Trim())
                    End If
                    cmd.Parameters.AddWithValue("IGP_MODE", popiRadios.SelectedValue.Trim())
                    Dim da As New OracleDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    If (dt.Rows.Count > 0) Then
                        If (email.Trim().Length > 0) Then
                            Return True
                        End If
                        gdv_Create_User.DataSource = dt
                        gdv_Create_User.DataBind()
                        'txtRemarks.Enabled = False
                        'btnAssign.Enabled = False
                        ddl_Ext_Status.Enabled = False
                    Else
                        'txtRemarks.Enabled = True
                        'btnAssign.Enabled = True
                        ddl_Ext_Status.Enabled = True
                        Return False
                        'ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                    End If
                End Using
            End Using

        Catch ex As Exception
            Return True
        End Try
    End Function

    Private Sub gdv_Create_User_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gdv_Create_User.PageIndexChanging
        bind_Admin("", "", "P")
        gdv_Create_User.PageIndex = e.NewPageIndex
        gdv_Create_User.DataBind()
    End Sub
End Class
