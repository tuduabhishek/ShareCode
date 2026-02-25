
Imports System.Data.OracleClient
Imports System.Data
Imports System.IO

Partial Class Manage_Code
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim comobj As New CommonVb()
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "Manage Code"
                bind_irc_type()
                'Session("USER_ID")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub bind_irc_type()
        Try
            Dim sb As New StringBuilder()
            sb.Append("Select distinct irc_type, irc_type || ' - ' || IRC_TYPE_DESC IRC_TYPE_DESC from hrps.T_IR_CODES order by irc_type ")
            Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
                Using cmd As New OracleCommand()
                    cmd.CommandText = sb.ToString()
                    cmd.Connection = con
                    Dim dt As New DataTable()
                    Dim da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                    ddlType.DataTextField = "IRC_TYPE_DESC"
                    ddlType.DataValueField = "irc_type"
                    ddlType.DataSource = dt
                    ddlType.DataBind()
                    ddlType.Items.Insert(0, New ListItem("All", "A"))

                    ddl_IRC_TYPE.DataTextField = "irc_type"
                    ddl_IRC_TYPE.DataValueField = "irc_type"
                    ddl_IRC_TYPE.DataSource = dt
                    ddl_IRC_TYPE.DataBind()

                End Using
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bind_irc_Code(irc_type As String)
        Try
            Dim sb As New StringBuilder()
            sb.Append("Select distinct IRC_CODE, IRC_CODE || ' - ' || IRC_CODE_DESC IRC_CODE_DESC from hrps.T_IR_CODES where trim(irc_type) =:irc_type order by IRC_CODE ")
            Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
                Using cmd As New OracleCommand()
                    cmd.CommandText = sb.ToString()
                    cmd.Connection = con
                    cmd.Parameters.Add(New OracleParameter("irc_type", OracleType.VarChar)).Value = irc_type.Trim()

                    Dim dt As New DataTable()
                    Dim da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                    ddl_Code.DataTextField = "IRC_CODE_DESC"
                    ddl_Code.DataValueField = "IRC_CODE"
                    ddl_Code.DataSource = dt
                    ddl_Code.DataBind()
                    ddl_Code.Items.Insert(0, New ListItem("All", "A"))

                    'ddl_IRC_TYPE.DataTextField = "irc_type"
                    'ddl_IRC_TYPE.DataValueField = "irc_type"
                    'ddl_IRC_TYPE.DataSource = dt
                    'ddl_IRC_TYPE.DataBind()

                End Using
            End Using
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            'If (txtIRC_Code.Text.Trim().Length < 0) Then
            'ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Code.")
            'Return
            'End If

            'If (ddlType.Text.Trim().Length < 0) Then
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Type.")
            '    Return
            'End If
            Dim dt As DataTable = bind_Code(ddlType.SelectedValue.Trim(), ddl_Code.SelectedValue.Trim(), txt_Start_Date.Text, txtEnd_Date.Text)
            clearControl()
            'gdvManageCode.DataSource = dt
            'gdvManageCode.DataBind()

        Catch ex As Exception

        End Try
    End Sub
    Private Function bind_Code(IRC_Type As String, IRC_CODE As String, start_date As String, End_date As String) As DataTable
        Dim dt As New DataTable()
        Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
            Using cmd As New OracleCommand()
                cmd.CommandText = "select IRC_TYPE,IRC_CODE,to_char(IRC_START_DT,'dd/MM/yyyy') Start_Date,to_char( IRC_END_DT,'dd/MM/yyyy') End_date ,IRC_DESC,case when IRC_VALID_TAG='Y' then 'Yes' when IRC_VALID_TAG='N' then 'No' when IRC_VALID_TAG='D' then 'Deactivate' when IRC_VALID_TAG='A' then 'Activate' end Valid_TAG,IRC_VALID_TAG,IRC_CHANGE_USER,IRC_CHANGE_DATE,IRC_CODE_DESC,IRC_TYPE_DESC from hrps.T_IR_CODES where 1=1 "
                If (IRC_Type.Trim().Length > 0) Then
                    If (IRC_Type.Trim().ToUpper() <> "A") Then
                        cmd.CommandText += " and IRC_TYPE=:IRC_TYPE"
                    End If

                End If
                If (IRC_CODE.Trim().Length > 0) Then
                    If (IRC_CODE.Trim().ToUpper() <> "A") Then
                        cmd.CommandText += " and IRC_CODE=:IRC_CODE"
                    End If

                End If
                If (start_date.Trim().Length > 0) Then
                    cmd.CommandText += " and IRC_START_DT=:IRC_START_DT"
                End If
                If (End_date.Trim().Length > 0) Then
                    cmd.CommandText += " and IRC_END_DT=:IRC_END_DT"
                End If
                cmd.CommandText += " order by IRC_TYPE, IRC_CODE"
                cmd.Connection = con

                If (IRC_Type.Trim().Length > 0) Then
                    If (IRC_Type.Trim().ToUpper() <> "A") Then
                        cmd.Parameters.Add(New OracleParameter("IRC_Type", OracleType.VarChar)).Value = IRC_Type.Trim()
                    End If
                End If
                If (IRC_CODE.Trim().Length > 0) Then
                    If (IRC_CODE.Trim().ToUpper() <> "A") Then
                        cmd.Parameters.Add(New OracleParameter("IRC_CODE", OracleType.VarChar)).Value = IRC_CODE.Trim()
                    End If

                End If

                If (start_date.Trim().Length > 0) Then
                    cmd.Parameters.Add(New OracleParameter("IRC_START_DT", OracleType.DateTime)).Value = start_date.Trim()
                End If
                If (End_date.Trim().Length > 0) Then
                    cmd.Parameters.Add(New OracleParameter("IRC_END_DT", OracleType.DateTime)).Value = End_date.Trim()
                End If
                Dim da As New OracleDataAdapter(cmd)
                da.Fill(dt)
                gdvManageCode.DataSource = dt
                gdvManageCode.DataBind()
            End Using
        End Using
        Return dt
    End Function
    Private Function manageCode(action As String) As Integer
        Dim ret_value As Integer = 0
        Try
            Dim sb As New StringBuilder()

            Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
                Using cmd As New OracleCommand()
                    If (action = "I") Then
                        'cmd.CommandText = "Insert into hrps.T_IR_CODES(IRC_TYPE,IRC_CODE,IRC_START_DT,IRC_END_DT,IRC_DESC,IRC_VALID_TAG,IRC_CHANGE_USER,IRC_CHANGE_DATE) values(:IRC_TYPE,:IRC_CODE,:IRC_START_DT,:IRC_END_DT,:IRC_DESC,:IRC_VALID_TAG,:IRC_CHANGE_USER,sysdate)"
                        cmd.CommandText = "Insert into hrps.T_IR_CODES(IRC_TYPE,IRC_CODE,IRC_START_DT,IRC_END_DT,IRC_DESC,IRC_VALID_TAG,IRC_CHANGE_USER,IRC_CHANGE_DATE,IRC_TYPE_DESC,IRC_CODE_DESC) values(:IRC_TYPE,:IRC_CODE,to_date(:IRC_START_DT, 'dd/mm/yyyy'),to_date(:IRC_END_DT, 'dd/mm/yyyy'),:IRC_DESC,:IRC_VALID_TAG,:IRC_CHANGE_USER,sysdate,:IRC_TYPE_DESC,:IRC_CODE_DESC)"
                    ElseIf (action = "U") Then
                        'cmd.CommandText = "Update hrps.T_IR_CODES set IRC_START_DT=:IRC_START_DT,IRC_END_DT=:IRC_END_DT,IRC_DESC=:IRC_DESC,IRC_VALID_TAG=:IRC_VALID_TAG,IRC_CHANGE_USER=:IRC_CHANGE_USER,IRC_CHANGE_DATE=sysdate where IRC_TYPE=:IRC_TYPE and IRC_CODE=:IRC_CODE"

                        cmd.CommandText = "Update hrps.T_IR_CODES set IRC_START_DT=to_date(:IRC_START_DT, 'dd/mm/yyyy'),IRC_END_DT=to_date(:IRC_END_DT, 'dd/mm/yyyy'),IRC_DESC=:IRC_DESC,IRC_VALID_TAG=:IRC_VALID_TAG,IRC_CHANGE_USER=:IRC_CHANGE_USER,IRC_CHANGE_DATE=sysdate,IRC_TYPE_DESC=:IRC_TYPE_DESC,IRC_CODE_DESC=:IRC_CODE_DESC where IRC_TYPE=:IRC_TYPE and IRC_CODE=:IRC_CODE"
                    End If
                    cmd.Connection = con
                    If (action = "I") Then
                        cmd.Parameters.Add(New OracleParameter("IRC_TYPE", OracleType.VarChar)).Value = txtIRC_TYPE.Text.Trim()
                    ElseIf (action = "U") Then
                        cmd.Parameters.Add(New OracleParameter("IRC_TYPE", OracleType.VarChar)).Value = ddl_IRC_TYPE.SelectedValue.Trim()
                    End If

                    cmd.Parameters.Add(New OracleParameter("IRC_CODE", OracleType.VarChar)).Value = txtIRC_Code.Text.Trim()
                    'cmd.Parameters.Add(New OracleParameter("IRC_START_DT", OracleType.DateTime)).Value = convertDate(txtIRC_Start_Date.Text.Trim())
                    'cmd.Parameters.Add(New OracleParameter("IRC_END_DT", OracleType.DateTime)).Value = convertDate(txtIRC_End_Date.Text.Trim())
                    cmd.Parameters.Add(New OracleParameter("IRC_START_DT", OracleType.VarChar)).Value = txtIRC_Start_Date.Text.Trim()
                    cmd.Parameters.Add(New OracleParameter("IRC_END_DT", OracleType.VarChar)).Value = txtIRC_End_Date.Text.Trim()

                    cmd.Parameters.Add(New OracleParameter("IRC_DESC", OracleType.VarChar)).Value = txtIRC_Description.Text.Trim()
                    cmd.Parameters.Add(New OracleParameter("IRC_VALID_TAG", OracleType.VarChar)).Value = ddl_Valid_Tag.SelectedValue.Trim()
                    cmd.Parameters.Add(New OracleParameter("IRC_CHANGE_USER", OracleType.VarChar)).Value = Session("USER_ID").ToString().Trim()
                    cmd.Parameters.Add(New OracleParameter("IRC_TYPE_DESC", OracleType.VarChar)).Value = txt_TYPE_DESC.Text.Trim()
                    cmd.Parameters.Add(New OracleParameter("IRC_CODE_DESC", OracleType.VarChar)).Value = txt_CODE_DESC.Text.Trim()
                    If (con.State = ConnectionState.Closed) Then
                        con.Open()
                    End If
                    ret_value = cmd.ExecuteNonQuery()
                    con.Close()
                End Using
            End Using
        Catch oex As OracleException
            If (oex.Code = 1) Then
                ret_value = -1
            Else
                ret_value = -2
            End If
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr As String = ""
            If (action = "U") Then
                dividesterr = "Error while  Updating Code :- IRC Type " & ddlType.SelectedValue.Trim() & "  IRC Code:- " & txtIRC_Code.Text.Trim() & Environment.NewLine
            ElseIf (action = "U") Then
                dividesterr = "Error while  Inserting Code :- IRC Type " & ddlType.SelectedValue.Trim() & "  IRC Code:- " & txtIRC_Code.Text.Trim() & Environment.NewLine
            End If

            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, oex.ToString())
        Catch ex As Exception
            ret_value = -3
            Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            Dim dividesterr As String = ""
            If (action = "U") Then
                dividesterr = "Error while  Updating Code :- IRC Type " & ddlType.SelectedValue.Trim() & "  IRC Code:- " & txtIRC_Code.Text.Trim() & Environment.NewLine
            ElseIf (action = "U") Then
                dividesterr = "Error while  Inserting Code :- IRC Type " & ddlType.SelectedValue.Trim() & "  IRC Code:- " & txtIRC_Code.Text.Trim() & Environment.NewLine
            End If

            File.AppendAllText(fnameerr, dividesterr)
            File.AppendAllText(fnameerr, ex.ToString())
            'ShowGenericMessageModal(CommonConstants.AlertType.error, "IRC Code :" + txtIRC_Code.Text.Trim() + "already exist.")
        End Try
        'IRC_TYPE,IRC_CODE,IRC_START_DT,IRC_END_DT,IRC_DESC,IRC_VALID_TAG,IRC_CHANGE_USER,IRC_CHANGE_DATE

        Return ret_value
    End Function
    Private Function validate_controls() As Boolean
        If (txtIRC_Code.Text.Trim().Length < 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Code.")
            txtIRC_Code.Focus()
            Return False
        End If
        If (txt_CODE_DESC.Text.Trim().Length < 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Code Desc.")
            txt_CODE_DESC.Focus()
            Return False
        End If
        If (txtIRC_TYPE.Text.Trim().Length < 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Type.")
            txtIRC_TYPE.Focus()
            Return False
        End If
        If (txt_TYPE_DESC.Text.Trim().Length < 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Type Desc.")
            txt_TYPE_DESC.Focus()
            Return False
        End If
        If (txtIRC_Start_Date.Text.Trim().Length < 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Start Date.")
            txtIRC_Start_Date.Focus()
            Return False
        End If
        If (txtIRC_End_Date.Text.Trim().Length < 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC End Date.")
            txtIRC_End_Date.Focus()
            Return False
        End If
        If (txtIRC_Description.Text.Trim().Length < 0) Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please entr IRC Description Date.")
            txtIRC_Description.Focus()
            Return False
        End If
        If (comobj.ISI(txtIRC_Code.Text.Trim())) Then
            txtIRC_Code.Focus()
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
            Return False
        End If
        If (comobj.ISI(txt_CODE_DESC.Text.Trim())) Then
            txtIRC_Code.Focus()
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
            Return False
        End If

        If (comobj.ISI(txt_TYPE_DESC.Text.Trim())) Then
            txt_TYPE_DESC.Focus()
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
            Return False
        End If
        If (comobj.ISI(txtIRC_TYPE.Text.Trim())) Then
            txtIRC_TYPE.Focus()
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
            Return False
        End If

        If (comobj.ISI(txtIRC_Start_Date.Text.Trim())) Then
            txtIRC_Start_Date.Focus()
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
            Return False
        End If
        If (comobj.ISI(txtIRC_End_Date.Text.Trim())) Then
            txtIRC_End_Date.Focus()
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
            Return False
        End If
        If (comobj.ISI(txtIRC_Description.Text.Trim())) Then
            txtIRC_Description.Focus()
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
            Return False
        End If
        Return True
    End Function
    Protected Sub btn_Action_Click(sender As Object, e As EventArgs) Handles btn_Action.Click
        If (validate_controls() = False) Then
            Return
        End If
        Dim ret_val As Integer = 0
        If (btn_Action.Text = "Update Code") Then
            ret_val = manageCode("U")
            If (ret_val > 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "IRC Code has been successfully updated.")
                clearControl()
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while updating Code.")
                'ShowGenericMessageModal(CommonConstants.AlertType.error, "IRC Code :" + txtIRC_Code.Text.Trim() + "already exist.")
            End If

        ElseIf (btn_Action.Text = "Add New Code") Then
            ret_val = manageCode("I")
            If (ret_val > 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Code has been successfully inserted.")
                clearControl()
            ElseIf (ret_val = -1) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "IRC Code : " + txtIRC_Code.Text.Trim() + " already exist.")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while updating Code.")
            End If
        End If
        bind_Code(ddlType.SelectedValue.Trim(), ddl_Code.SelectedValue.Trim(), txt_Start_Date.Text.Trim(), txtEnd_Date.Text.Trim())
        bind_irc_type()
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

    Private Sub gdvManageCode_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdvManageCode.RowCommand
        Try
            Dim btn As Button = CType(e.CommandSource, Button)
            Dim grv As GridViewRow = CType(btn.Parent.Parent, GridViewRow)
            If (e.CommandName = "UPDATE") Then
                txtIRC_Code.Text = CType(grv.FindControl("lblIRC_CODE"), Label).Text
                ddl_IRC_TYPE.SelectedValue = CType(grv.FindControl("lblIRC_TYPE"), Label).Text
                txtIRC_TYPE.Visible = False
                ddl_IRC_TYPE.Visible = True
                txtIRC_Code.Enabled = False
                ddl_IRC_TYPE.Enabled = False
                txtIRC_Description.Text = CType(grv.FindControl("lblIRC_DESC"), Label).Text
                txtIRC_Start_Date.Text = CType(grv.FindControl("lblIRC_START_DT"), Label).Text
                txtIRC_End_Date.Text = CType(grv.FindControl("lblIRC_END_DT"), Label).Text
                ddl_Valid_Tag.SelectedValue = CType(grv.FindControl("lblIRC_VALID_TAG"), Label).Text
                txt_TYPE_DESC.Text = CType(grv.FindControl("lbl_TYPE_DESC"), Label).Text
                txt_CODE_DESC.Text = CType(grv.FindControl("lbl_CODE_DESC"), Label).Text

                btn_Action.Text = "Update Code"
                txt_TYPE_DESC.Focus()
                upnlMain.UpdateMode = UpdatePanelUpdateMode.Conditional
                upnlMain.Update()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function convertDate(text As String) As DateTime
        Dim dt As New DateTime(Mid(text, 7, 4), Mid(text, 4, 2), Mid(text, 1, 2))
        Return dt
    End Function
    Private Sub clearControl()
        txtIRC_Code.Text = String.Empty
        txtIRC_TYPE.Text = String.Empty
        ddl_IRC_TYPE.SelectedIndex = 0
        txtIRC_Start_Date.Text = String.Empty
        txtIRC_End_Date.Text = String.Empty
        txt_TYPE_DESC.Text = String.Empty
        txt_CODE_DESC.Text = String.Empty
        btn_Action.Text = "Add New Code"
        ddl_IRC_TYPE.SelectedIndex = 0
        ddl_IRC_TYPE.Visible = False
        txtIRC_TYPE.Visible = True
        txtIRC_Code.Enabled = True
        txtIRC_Description.Text = String.Empty
        ddl_Valid_Tag.SelectedIndex = 0

    End Sub

    Private Sub gdvManageCode_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gdvManageCode.PageIndexChanging
        Dim dt = bind_Code(ddlType.SelectedValue.Trim(), ddl_Code.SelectedValue.Trim(), "", "")
        'gdvManageCode.DataSource = dt
        'gdvManageCode.DataBind()
        gdvManageCode.PageIndex = e.NewPageIndex
        gdvManageCode.DataBind()
    End Sub

    Private Sub gdvManageCode_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles gdvManageCode.RowUpdating

    End Sub
    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        clearControl()
        gdvManageCode.DataSource = Nothing
        gdvManageCode.DataBind()
        ddlType.SelectedIndex = 0
    End Sub
    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlType.SelectedIndexChanged

        bind_irc_Code(ddlType.SelectedValue.Trim())
    End Sub
End Class
