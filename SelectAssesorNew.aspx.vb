Imports System.Web.Services
Imports System.Data.OracleClient
Imports System.Data
Imports System.Diagnostics

Partial Class SelectAssesorNew
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SelectAssesorNew
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select distinct ema_ename ||'('|| ema_perno ||')' EName from tips.t_empl_all where (ema_perno like  :ema_perno or upper(ema_ename) like "
            cmd.CommandText += " :ema_ename)  and ema_disch_dt is null AND EMA_COMP_CODE='1000'and ema_emp_class in ('1','2')  and EMA_EQV_LEVEL in('I2','I1')"


            ob.conHrps.Open()

            cmd.Connection = ob.conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", "%" & prefixText.ToUpper & "%")
            cmd.Parameters.AddWithValue("ema_ename", "%" & prefixText.ToUpper & "%")
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader
            While sdr.Read
                prefixes.Add(sdr("EName").ToString)
            End While



            Return prefixes
        Catch ex As Exception
            MsgBox(ex.ToString())
            Return Nothing

        Finally

            ob.conHrps.Close()

        End Try

    End Function

    Public Sub bindFinalGrid(pno As String)
        Dim r As String = String.Empty
        r = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY from dual"

        Dim oda As New OracleDataAdapter(r, conHrps)
        Dim g As New DataTable()
        oda.Fill(g)
        If g.Rows.Count > 0 Then
            lblyear.Text = g.Rows(0)(0).ToString()
        End If

        Try
            Dim qry As String = String.Empty
            qry = "select  SS_PNO,SS_NAME,ss_level,SS_DESG,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer','ROPT','Subordinates','INTSH','Internal Stakeholder"
            qry += "',SS_CATEG) categ,SS_CATEG ,SS_DEPT,SS_EMAIL from t_survey_status  where ss_asses_pno =:ss_asses_pno and ss_year=:ss_year "
            qry += " and nvl(SS_WFL_STATUS,'0') not in ('2','3','9') order by categ"

            Dim cmd As New OracleCommand()
            cmd.Connection = conHrps
            cmd.CommandText = qry
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ss_asses_pno", pno)
            cmd.Parameters.AddWithValue("ss_year", lblyear.Text)
            oda = New OracleDataAdapter(cmd)
            g = New DataTable()
            oda.Fill(g)
            If g.Rows.Count > 0 Then
                GvRepoties.DataSource = Nothing
                GvRepoties.DataBind()
                GvRepoties.DataSource = g
                GvRepoties.DataBind()
                'txtperno.ReadOnly = True
                ' Button1.Visible = True
                ' btnnontslsub.Visible = True
                ' btnsubmit.Visible = True
            Else

                txtperno.ReadOnly = False
                GvRepoties.DataSource = Nothing
                GvRepoties.DataBind()
                'Button1.Visible = False
                ' btnnontslsub.Visible = False
                'btnsubmit.Visible = False
            End If
            txtperno.ReadOnly = True
        Catch ex As Exception
            'MsgBox("ree")
        End Try
    End Sub

    Protected Sub btnsubmit_Click(sender As Object, e As EventArgs)
        Try

            Dim comd As New OracleCommand()
            Dim approver As String = String.Empty
            comd.CommandText = " select EMA_PERS_EXEC_PNO "
            comd.CommandText += " from tips.t_empl_all where ema_perno='" & Right(txtperno.Text.Trim, 7).TrimEnd(")") & "' and EMA_COMP_CODE='1000' "
            Dim dt = getRecordInDt(comd, conHrps)
            If dt.Rows.Count > 0 Then
                approver = dt.Rows(0)(0).ToString()

            End If
            Dim upqry As String = String.Empty
            upqry = " update t_survey_status set SS_FLAG1 ='2',SS_TAG='SU',SS_APPROVER='" & approver & "',SS_WFL_STATUS='2',SS_ADM_UPD_DT=sysdate,SS_APP_TAG='AP',ss_tag_dt =sysdate, "
            upqry += " SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "', SS_UPDATED_DT=sysdate where  SS_ASSES_PNO ='" & Right(txtperno.Text.Trim, 7).TrimEnd(")") & "' and ss_year='" & lblyear.Text & "' and ss_del_tag='N' and ss_status='SE' and nvl(SS_WFL_STATUS,'0') not in ('2','3','9')"
            Dim c As New OracleCommand()
            c.CommandText = upqry
            c.Connection = conHrps
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            c.ExecuteNonQuery()
            bindFinalGrid(Right(txtperno.Text.Trim, 7).TrimEnd(")"))
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Form has been submitted...!")
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
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
    Protected Sub btnaddtslsub_Click(sender As Object, e As EventArgs)
        If txtperno.Text.Length > 6 Then
            Dim pno As String = String.Empty
            pno = Right(txtperno.Text.Trim, 7).TrimEnd(")")
            bindFinalGrid(pno)
            Dim strself = New OracleCommand()
            strself.CommandText = "select SS_ASSES_PNO from t_survey_status  where  SS_ASSES_PNO='" & pno & "' and SS_CATEG='Self'  and SS_YEAR='" & lblyear.Text & "'"
            Dim dt1 = getRecordInDt(strself, conHrps)
            If dt1.Rows.Count > 0 Then
            Else
                strself = New OracleCommand()
                strself.CommandText = "select ema_perno,ema_ename,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_perno='" & pno & "' and EMA_COMP_CODE='1000'"
                Dim g1 = getRecordInDt(strself, conHrps)
                If g1.Rows.Count > 0 Then
                    SaveData("Self", g1.Rows(0)("ema_perno").ToString(), g1.Rows(0)("ema_ename").ToString(), g1.Rows(0)("EMA_DESGN_DESC").ToString(), g1.Rows(0)("EMA_DEPT_DESC").ToString(), g1.Rows(0)("EMA_EMAIL_ID").ToString(), g1.Rows(0)("EMA_EMPL_SGRADE").ToString(), "ORG", pno)
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "P.no or name not exist...!")
                    Exit Sub
                End If

            End If
            bindFinalGrid(pno)
            Button1.Visible = True
            btnnontslsub.Visible = True
            btnsubmit.Visible = True
        Else
            Button1.Visible = False
            btnnontslsub.Visible = False
            btnsubmit.Visible = False
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select pno in a list...!")
        End If
    End Sub
    Protected Sub GvRepoties_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim qry As String = String.Empty
            Dim pno = Right(txtperno.Text.Trim(), 7).TrimEnd(")")
            Dim aspno As String = String.Empty
            aspno = CType(e.Row.FindControl("lblpno"), Label).Text
            Dim chk = CType(e.Row.FindControl("chksub"), CheckBox)
            Dim cat = CType(e.Row.FindControl("Label1"), Label)
            qry = "select * from t_survey_status  where ss_asses_pno='" & pno & "' and ss_year='" & lblyear.Text & "' and nvl(ss_wfl_status,'0') not in ('2','3','9') "
            qry += " and ss_pno='" & aspno & "' and ss_status='SE' and ss_del_tag='N' and ss_categ='" & cat.Text & "'"
            Dim com1 As New OracleCommand()
            com1.CommandText = qry
            com1.Connection = conHrps
            Dim oda As New OracleDataAdapter(com1)
            Dim dt As New DataTable()
            oda.Fill(dt)
            If dt.Rows.Count > 0 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
            If cat.Text = "Self" Then
                chk.Checked = True
                chk.Enabled = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub chksub_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent

            Dim respno = CType(gv.FindControl("lblpno"), Label)
            Dim categ = CType(gv.FindControl("Label1"), Label)
            Dim chk = CType(gv.FindControl("chksub"), CheckBox)
            Dim email = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If respno.Text.StartsWith("SR") Then
                val = Check(lblyear.Text, Right(txtperno.Text.Trim(), 7).TrimEnd(")"), email.Text)
            Else
                val = Check(lblyear.Text, Right(txtperno.Text.Trim(), 7).TrimEnd(")"), respno.Text)
            End If
            If chk.Checked = True Then

                If val = "" Then
                    UpdateData(respno.Text, "SE", "N", categ.Text)
                    bindFinalGrid(Right(txtperno.Text.Trim(), 7).TrimEnd(")"))
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If

            Else
                UpdateData(respno.Text, "DS", "Y", categ.Text)
            End If

        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub

    Public Function Check(year As String, assespno As String, pno As String) As String
        Dim vl As String = String.Empty
        Try
            Dim str As New OracleCommand()
            str.CommandText = " select SS_PNO,irc_desc from hrps.t_survey_status,hrps.t_ir_codes where  SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO "
            str.CommandText += "and ((SS_PNO=:SS_PNO) OR (upper(ss_email)=:SS_PNO)) and SS_status='SE' and ss_del_tag ='N' and irc_type='360RL' and upper(ss_categ)=upper(irc_code)"

            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_YEAR", lblyear.Text)
            str.Parameters.AddWithValue("SS_ASSES_PNO", assespno.ToString())
            str.Parameters.AddWithValue("SS_PNO", pno.ToUpper())


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

            Dim str As New OracleCommand()
            str.CommandText = " select * from t_survey_status where  SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO and (SS_PNO=:SS_PNO OR upper(ss_email)=:SS_PNO) "
            str.CommandText += " and SS_CATEG=:SS_CATEG"
            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_YEAR", lblyear.Text)
            str.Parameters.AddWithValue("SS_ASSES_PNO", Right(txtperno.Text.Trim(), 7).TrimEnd(")"))
            str.Parameters.AddWithValue("SS_CATEG", role)
            If orgtype = "ORG" Then
                str.Parameters.AddWithValue("SS_PNO", pno)
            Else
                str.Parameters.AddWithValue("SS_PNO", email.ToUpper())
                'pno = email
            End If
            Dim ds As New OracleDataAdapter(str)
            Dim dt As New DataTable()
            ds.Fill(dt)

            If dt.Rows.Count > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added please use add/remove methord for select...!")
                Exit Sub
            End If

            Dim OrgStr As String = String.Empty
            Dim id = getRefNo()

            If pno = "" Then
                pno = id
            End If
            OrgStr = "insert into T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL,SS_ADM_TAG,SS_APP_TAG,SS_APPROVER,SS_WFL_STATUS) values ('" & role & "', '" & id & "','" & pno & "','" & name & "','" & Replace(desg, "'", "''") & "','" & Replace(org, "'", "''") & "','" & email & "','SE','N', "
            OrgStr += " '" & Session("ADM_USER").ToString() & "', sysdate,'N','" & orgtype & "','" & lblyear.Text & "','" & assespno & "','" & lvl & "','2','','','')"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim comnd As New OracleCommand(OrgStr, conHrps)
            comnd.ExecuteNonQuery()
            ' Clear()

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub

    Protected Sub btnaddnorgI_Click(sender As Object, e As EventArgs)
        Try
            If txtnamenI.Text.Trim() = "" Or txtdeptnI.Text.Trim() = "" Or txtdesgnI.Text.Trim() = "" Or txtemailnI.Text.Trim() = "" Or ddlrole.SelectedValue = "Select assessors category" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, " Please Fill All Column...!")
                Exit Sub
            End If
            Dim pno = ""
            Dim assesor As String = String.Empty
            assesor = Right(txtperno.Text.Trim(), 7).TrimEnd(")")
            Dim name = Replace(txtnamenI.Text.Trim.Split("(")(0), "'", "''")
            Dim desg = Replace(txtdesgnI.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptnI.Text.Trim, "'", "''")
            Dim email = Replace(txtemailnI.Text.Trim, "'", "''")

            Dim val = Check(lblyear.Text, assesor, email)

            If val = "" Then
                SaveData(ddlrole.SelectedValue.ToString(), pno, name, desg, dept, email, "", "NORG", assesor)
                Reset()
                bindFinalGrid(assesor)
                ShowGenericMessageModal(CommonConstants.AlertType.success, " Added ...!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Public Function getRefNo() As String
        Try
            Dim strRef As String = String.Empty
            Dim refno As String = String.Empty
            strRef = " select MAX(substr(SS_ID,3,5)) SS_ID from T_SURVEY_STATUS where SS_ID like 'SR%' "
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

            refno = refno.PadLeft(5, "0")
            refno = "SR" + refno

            Return refno
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function

    Public Sub UpdateData(id As String, tag As String, deltag As String, catg As String)
        Try
            Dim query As String = String.Empty
            'query = "update t_survey_status set SS_STATUS ='" & tag & "' where SS_pno='" & id & "'  and SS_YEAR='" & ViewState("FY").ToString() & "'"
            query = "update t_survey_status set SS_STATUS =:SS_STATUS,SS_DEL_TAG=:SS_DEL_TAG, SS_APP_DT=sysdate, ss_updated_by =:ss_updated_by , ss_updated_dt =sysdate where SS_pno=:SS_pno and SS_YEAR=:SS_YEAR "
            query += "AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG "
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_STATUS", tag)
            comnd.Parameters.AddWithValue("SS_CATEG", catg)
            comnd.Parameters.AddWithValue("SS_pno", id)
            comnd.Parameters.AddWithValue("SS_YEAR", lblyear.Text)
            comnd.Parameters.AddWithValue("SS_DEL_TAG", deltag)
            comnd.Parameters.AddWithValue("ss_updated_by", Session("ADM_USER").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Right(txtperno.Text.Trim(), 7).TrimEnd(")"))
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnnontslsub_Click(sender As Object, e As EventArgs)
        divtsl.Visible = False
        divntsl.Visible = True
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs)
        divtsl.Visible = True
        divntsl.Visible = False
    End Sub
    Public Sub PopDroupdown()
        Try
            Dim qry As String = String.Empty
            qry = "select IRC_CODE,IRC_DESC from t_ir_codes where irc_type='360RL' and IRC_VALID_TAG='A' and IRC_CODE <> 'SELF' order by 2"

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

    Private Sub SelectAssesorNew_Load(sender As Object, e As EventArgs) Handles Me.Load
        If GetPno(Session("ADM_USER").ToString()) Then

        Else
            Response.Redirect("errorpage.aspx", True)
        End If
        If Not IsPostBack Then
            PopDroupdown()
        End If

    End Sub
    Protected Sub btnorgadd_Click(sender As Object, e As EventArgs)
        Try
            If txtpnoI.Text.Trim() <> "" And DropDownList1.SelectedValue <> "Select assessors category" Then
                Dim assesor As String = String.Empty
                assesor = Right(txtperno.Text.Trim(), 7).TrimEnd(")")
                Dim pno = txtpnoI.Text.Trim.Split("(")(1).TrimEnd(")")
                Dim name = Replace(txtpnoI.Text.Trim.Split("(")(0), "'", "''")
                Dim desg = Replace(txtdesgI.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptI.Text.Trim, "'", "''")
                Dim email = Replace(txtemailI.Text.Trim, "'", "''")

                Dim val = Check(lblyear.Text, assesor, pno)

                If val = "" Then
                    SaveData(DropDownList1.SelectedValue.ToString, pno, name, desg, dept, email, lbllvl.Text, "ORG", assesor)
                    Reset()
                    bindFinalGrid(assesor)
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
            MsgBox(ex.ToString)
        End Try
    End Sub

    Protected Sub txtpnoI_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoI.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            strself.CommandText += " (ema_ename||'(' ||ema_perno ||')')='" & pno.ToString() & "' and ema_disch_dt is null  and ema_comp_code='1000'" '*********Added by Manoj Kumar 18-01-2021
            Dim dt As New DataTable()
            strself.Connection = conHrps
            Dim oda As New OracleDataAdapter(strself)
            oda.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtdesgI.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtemailI.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtemailI.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgI.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()

                lbllvl.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

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

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover1(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SelectAssesorNew
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select ema_ename ||'('|| ema_perno ||')' EName from tips.t_empl_all where ema_disch_dt is null and ema_comp_code='1000' and (ema_perno like '%" & prefixText & "%' or upper(ema_ename) like "
            cmd.CommandText += " '%" & prefixText.ToUpper & "%')"


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
    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            Dim q As New OracleCommand()
            q.CommandText = "Select IGP_user_id,ema_ename from t_ir_adm_grp_privilege,TIPS.t_empl_all where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno.ToString() & "' and EMA_COMP_CODE='1000'"
            Dim p = getRecordInDt(q, conHrps)
            If p.Rows.Count > 0 Then
                d = True
            Else
                d = False

            End If
            Return d
        Catch ex As Exception

        End Try
    End Function
    Protected Sub Button2_Click(sender As Object, e As EventArgs)
        txtperno.Text = ""
        bindFinalGrid("")
        txtperno.ReadOnly = False
    End Sub
End Class
