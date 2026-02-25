
Imports System.Data.OracleClient
Imports System.Data

Partial Class Mail_Management
    Inherits System.Web.UI.Page
    Dim comobj As New CommonVb()
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "Mail Management"
                bind_Mail("")
                'Session("USER_ID")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bind_Mail(value As String)
        Dim dt As New DataTable()
        Try
            'Select TP_VALUE,TP_DESC,case when TP_ACTIVE='A' then 'Active' when TP_ACTIVE='D' then 'Deactive' else '' end status,TP_ACTIVE,to_char(TP_START_DT,'dd/MM/yyyy') TP_START_DT,to_char(TP_END_DT,'dd/MM/yyyy') TP_END_DT,TP_CREATED_BY,to_char(TP_CREATED_DT,'dd/MM/yyyy') TP_CREATED_DT,TP_MODIFIED_BY,to_char(TP_MODIFIED_DT,'dd/MM/yyyy') TP_MODIFIED_DT from T_PARAM
            Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
                Using cmd As New OracleCommand()
                    cmd.CommandText = "Select TP_VALUE,TP_DESC,case when TP_ACTIVE='A' then 'Active' when TP_ACTIVE='D' then 'Deactive' else '' end status,TP_ACTIVE,to_char(TP_START_DT,'dd/MM/yyyy') TP_START_DT,to_char(TP_END_DT,'dd/MM/yyyy') TP_END_DT,TP_CREATED_BY,to_char(TP_CREATED_DT,'dd/MM/yyyy') TP_CREATED_DT,TP_MODIFIED_BY,to_char(TP_MODIFIED_DT,'dd/MM/yyyy') TP_MODIFIED_DT from T_PARAM"
                    If (value.Trim().Length > 0) Then
                        cmd.CommandText += " Where TP_VALUE=:TP_VALUE"
                    End If
                    cmd.CommandText += " order by TP_VALUE "
                    cmd.Connection = con
                    If (value.Trim().Length > 0) Then
                        cmd.Parameters.Add(New OracleParameter("TP_VALUE", OracleType.VarChar)).Value = value.Trim()
                    End If
                    Dim da As New OracleDataAdapter(cmd)
                    da.Fill(dt)
                    gdv_Mail.DataSource = dt
                    gdv_Mail.DataBind()
                End Using
            End Using
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gdv_Mail_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gdv_Mail.RowCommand
        Try
            If (e.CommandName.Trim() = "_UPDATE") Then
                Dim btn As Button = CType(e.CommandSource, Button)
                Dim grv As GridViewRow = CType(btn.Parent.Parent, GridViewRow)
                txtDesc.Text = CType(grv.FindControl("lblTP_DESC"), Label).Text
                txtStart_Date.Text = CType(grv.FindControl("lblTP_START_DT"), Label).Text
                txtEnd_Date.Text = CType(grv.FindControl("lblTP_END_DT"), Label).Text
                ddl_Status.SelectedValue = CType(grv.FindControl("lblTP_ACTIVE"), Label).Text

                txtStart_Date.Enabled = True
                txtEnd_Date.Enabled = True
                ddl_Status.Enabled = True

                btnAction.Text = "Update"
                ViewState("Value") = e.CommandArgument
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub mange_Mail(desc As String, start_date As String, end_date As String, status As String, action As String)
        Dim value As String = ""
        Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
            Using cmd As New OracleCommand()
                If (action.Trim() = "I") Then
                    cmd.CommandText = "insert into T_PARAM(TP_VALUE,TP_DESC,TP_ACTIVE, TP_START_DT, TP_END_DT,TP_CREATED_BY,TP_CREATED_DT) values(:TP_VALUE,:TP_DESC,:TP_ACTIVE, to_date(:TP_START_DT, 'dd/mm/yyyy'),to_date(:TP_END_DT, 'dd/mm/yyyy'),:TP_CREATED_BY,sysdate)"
                    value = get_Max_Value()
                ElseIf (action.Trim() = "U") Then
                    cmd.CommandText = "Update T_PARAM set TP_DESC=:TP_DESC,TP_ACTIVE=:TP_ACTIVE,TP_START_DT=to_date(:TP_START_DT, 'dd/mm/yyyy'),TP_END_DT=to_date(:TP_END_DT, 'dd/mm/yyyy'),TP_MODIFIED_BY=:TP_CREATED_BY,TP_MODIFIED_DT=sysdate where TP_VALUE=:TP_VALUE"
                    If (ViewState("Value").ToString.Trim().Length > 0) Then
                        value = ViewState("Value").ToString().Trim()
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.success, "Something went wrong,please retry .")
                        clearControl()
                    End If

                End If
                cmd.Connection = con
                cmd.Parameters.Add(New OracleParameter("TP_VALUE", OracleType.VarChar)).Value = value.Trim()
                cmd.Parameters.Add(New OracleParameter("TP_DESC", OracleType.VarChar)).Value = desc.Trim()
                cmd.Parameters.Add(New OracleParameter("TP_START_DT", OracleType.VarChar)).Value = start_date.Trim()
                cmd.Parameters.Add(New OracleParameter("TP_END_DT", OracleType.VarChar)).Value = end_date.Trim()
                cmd.Parameters.Add(New OracleParameter("TP_ACTIVE", OracleType.VarChar)).Value = status.Trim()
                cmd.Parameters.Add(New OracleParameter("TP_CREATED_BY", OracleType.VarChar)).Value = Session("USER_ID").ToString().Trim()
                If (con.State = ConnectionState.Closed) Then
                    con.Open()
                End If
                Dim affec_row As Integer = cmd.ExecuteNonQuery()
                If (action = "I") Then
                    If (affec_row > 0) Then
                        ShowGenericMessageModal(CommonConstants.AlertType.success, "Data has been successfully inserted.")

                    End If
                ElseIf (action = "U") Then
                    If (affec_row > 0) Then
                        ShowGenericMessageModal(CommonConstants.AlertType.success, "Data has been successfully updated.")
                    End If
                End If
                If (con.State <> ConnectionState.Closed) Then
                    con.Close()
                End If
                'bind_Mail("")
                clearControl()
            End Using
        End Using

    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Private Function get_Max_Value() As String
        Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
            Using cmd As New OracleCommand()
                cmd.CommandText = "select max(to_number(nvl(TP_VALUE,0)))+1 value from T_PARAM where TP_VALUE<=99"
                cmd.Connection = con
                Dim da As New OracleDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)
                If (dt.Rows.Count > 0) Then
                    Return dt.Rows(0)("value").ToString().Trim()
                End If
            End Using
        End Using
        Return ""
    End Function
    Private Sub clearControl()
        txtDesc.Text = String.Empty
        txtStart_Date.Text = String.Empty
        txtEnd_Date.Text = String.Empty
        ddl_Status.SelectedIndex = 0
        txtStart_Date.Enabled = True
        txtEnd_Date.Enabled = True
        ddl_Status.Enabled = True
        btnAction.Text = "ADD"
        bind_Mail("")
    End Sub
    Protected Sub btnAction_Click(sender As Object, e As EventArgs) Handles btnAction.Click
        Try
            If (txtDesc.Text.Trim().Length <= 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please enter description.")
                txtDesc.Focus()
                Return
            End If
            If (txtStart_Date.Text.Trim().Length <> 10) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please enter valid start date(dd/mm/yyyy).")
                txtStart_Date.Focus()
                Return
            End If
            If (txtEnd_Date.Text.Trim().Length <> 10) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please enter end date(dd/mm/yyyy).")
                txtEnd_Date.Focus()
                Return
            End If
            If (txtStart_Date.Text.Trim().Length = 10 And txtEnd_Date.Text.Trim().Length = 10) Then
                Dim flag As Boolean = compare_Date(txtStart_Date.Text.Trim(), txtEnd_Date.Text.Trim())
                If (flag = False) Then
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Start Date is less than or equal to end date.")
                    'txtEnd_Date.Text = String.Empty
                    txtEnd_Date.Focus()
                    Return
                End If

            End If
            ''''''''''''''''''''''''''''''SQL/HTML Injection''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If (comobj.ISI(txtDesc.Text.Trim())) Then
                txtDesc.Focus()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
                Exit Sub
            End If
            If (comobj.ISI(txtStart_Date.Text.Trim())) Then
                txtStart_Date.Focus()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
                Exit Sub
            End If
            If (comobj.ISI(txtEnd_Date.Text.Trim())) Then
                txtEnd_Date.Focus()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Invalid text!")
                Exit Sub
            End If

            If (ddl_Status.SelectedIndex <= 0) Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Select status.")
                ddl_Status.Focus()
                Return
            End If
            If (btnAction.Text.Trim().ToUpper() = "ADD") Then
                mange_Mail(txtDesc.Text.Trim(), txtStart_Date.Text.Trim(), txtEnd_Date.Text.Trim(), ddl_Status.SelectedValue.Trim(), "I")
            ElseIf (btnAction.Text.Trim().ToUpper() = "UPDATE") Then
                mange_Mail(txtDesc.Text.Trim(), txtStart_Date.Text.Trim(), txtEnd_Date.Text.Trim(), ddl_Status.SelectedValue.Trim(), "U")
            End If

        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error.")
        End Try
    End Sub
    Private Function compare_Date(start_date As String, end_date As String) As Boolean
        Dim s_date = convertDate(start_date.Trim())

        Dim e_date As Date = convertDate(end_date.Trim())
        If (DateTime.Compare(s_date, e_date) > 0) Then
            'ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Enter Valid Joining Date(dd/MM/yyyy).Joining Date is less than or equal to current date.")
            'txt_Joining_Date.Focus()
            Return False
        Else
            Return True
        End If
    End Function
    Public Function convertDate(text As String) As DateTime
        Dim dt As New DateTime(Mid(text, 7, 4), Mid(text, 4, 2), Mid(text, 1, 2))
        Return dt.Date
    End Function

    Private Sub gdv_Mail_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gdv_Mail.PageIndexChanging
        bind_Mail("")
        gdv_Mail.PageIndex = e.NewPageIndex
        gdv_Mail.DataBind()
    End Sub
    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchFor_MailADMIN(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
        Dim cmd As New OracleCommand()
        Try



            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select TP_DESC ||'('|| TP_VALUE ||')' IGP_USER_ID from T_PARAM where TP_VALUE like "

            cmd.CommandText += " ('%" & prefixText.ToUpper & "%') or upper(TP_DESC) like ('%" & prefixText.ToUpper & "%') "   'WI368 add officer class


            cmd.Connection = con
            con.Open()
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader

            While sdr.Read
                prefixes.Add(sdr("IGP_USER_ID").ToString)
            End While



            Return prefixes
        Catch ex As Exception

            Return Nothing

        Finally
            If (con.State = ConnectionState.Open) Then
                con.Close()
            End If
            cmd.Dispose()
        End Try

    End Function

    Private Sub txtDesc_TextChanged(sender As Object, e As EventArgs) Handles txtDesc.TextChanged
        Try

            Using con As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
                Using cmd As New OracleCommand()

                    cmd.CommandText = "Select TP_VALUE,TP_DESC,case when TP_ACTIVE='A' then 'Active' when TP_ACTIVE='D' then 'Deactive' else '' end status,TP_ACTIVE,to_char(TP_START_DT,'dd/MM/yyyy') TP_START_DT,to_char(TP_END_DT,'dd/MM/yyyy') TP_END_DT,TP_CREATED_BY,to_char(TP_CREATED_DT,'dd/MM/yyyy') TP_CREATED_DT,TP_MODIFIED_BY,to_char(TP_MODIFIED_DT,'dd/MM/yyyy') TP_MODIFIED_DT from T_PARAM where TP_DESC ||'('|| TP_VALUE ||')'=:value "
                    cmd.Connection = con
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("value", txtDesc.Text.Trim())
                    Dim da As New OracleDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    If (dt.Rows.Count > 0) Then
                        gdv_Mail.DataSource = dt
                        gdv_Mail.DataBind()
                        txtStart_Date.Enabled = False
                        txtEnd_Date.Enabled = False
                        ddl_Status.Enabled = False
                    Else
                        'ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select description in list...")
                    End If
                End Using
            End Using


        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Try
            clearControl()
        Catch ex As Exception

        End Try
    End Sub
End Class
