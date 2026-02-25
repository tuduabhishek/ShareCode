Imports System.Data.OracleClient
Imports System.Data
Imports System.Net.Mail
Partial Class SurveyApproval
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Public Sub bindGrid()
        Try
            NoEachcateg()
            Dim qry As New OracleCommand()
            Dim pno = Session("USER_ID").ToString()
            qry.CommandText = "select distinct SS_ASSES_PNO,ema_ename,ema_desgn_desc "
            qry.CommandText += " from t_survey_status,tips.t_empl_all where SS_APPROVER =:SS_APPROVER "
            qry.CommandText += "and ema_perno = SS_ASSES_PNO  and SS_ASSES_PNO <> :SS_ASSES_PNO and ss_year='" & ViewState("FY").ToString() & "' "
            qry.CommandText += " and (ss_app_tag<>'RJ' or ss_app_tag is null ) "

            ' Dim dt = getRecordInDt(qry, conHrps)
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("SS_APPROVER", pno.ToString())
            qry.Parameters.AddWithValue("SS_ASSES_PNO", pno.ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then


                gvself.DataSource = dt
                gvself.DataBind()
            Else

                gvself.DataSource = Nothing
                gvself.DataBind()

            End If

        Catch ex As Exception
            MsgBox(ex.ToString())
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

    Public Sub NoEachcateg()
        Try
            Dim qr As New OracleCommand()
            qr.CommandText = "select a.IRC_CODE, SUBSTR(a.IRC_DESC,0,1) minmum, SUBSTR(a.IRC_DESC,3,2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            qr.CommandText += "  where a.irc_type='360VL' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"

            Dim w = getRecordInDt(qr, conHrps)

            If w.Rows.Count > 0 Then
                GridView2.DataSource = w
                GridView2.DataBind()
            End If
            Dim r As New OracleCommand()
            r.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY from dual"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)(0).ToString()
            End If
        Catch ex As Exception


        End Try
    End Sub

    Public Sub BindAssesorGrid(pno As String)
        Try
            Dim qry As New OracleCommand()
            qry.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer','ROPT','Subordinates'"
            qry.CommandText += " ,'INTSH','Internal Stakeholder',SS_CATEG) Category,SS_CATEG from t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and SS_STATUS='SE'  and ss_year='" & ViewState("FY").ToString() & "'    order by Category"
            ' Dim dt = getRecordInDt(qry, conHrps)
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("SS_ASSES_PNO", pno.ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                gvfinal.DataSource = Nothing
                gvfinal.DataBind()
                gvfinal.DataSource = dt
                gvfinal.DataBind()
                GridView1.DataSource = Nothing
                GridView1.DataBind()
                GridView1.DataSource = dt
                GridView1.DataBind()
                btnaddnontsl.Visible = True
                btnaddpeertsl.Visible = True
                lbOrg.Visible = True
                divtitle.Visible = True
                btnrej.Visible = True
                txtremarks.Visible = True
            Else
                gvfinal.DataSource = Nothing
                gvfinal.DataBind()
                GridView1.DataSource = Nothing
                GridView1.DataBind()
                btnaddnontsl.Visible = False
                btnaddpeertsl.Visible = False
                lbOrg.Visible = False
                divtitle.Visible = False
                btnrej.Visible = False
                txtremarks.Visible = False
            End If
            CheckApproved(pno)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        Dim vUserFullName As String = Page.User.Identity.Name   '155710'

        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If
        ' strUserID = "120324"
        Session("USER_DOMAIN") = strUserDomain.ToUpper()
        Session("USER_ID") = strUserID.ToUpper()
        lblname.Text = GetPno(strUserID)

        Dim ststus = ChkAuth(strUserID)
        If ststus = False Then
            Response.Redirect("errorpage.aspx", True)
        End If

    End Sub
    Public Function GetPno(pernr As String) As String
        Dim perno As String = ""
        Try

            Dim cm As New OracleCommand()
            cm.CommandText = "  select EMA_ENAME from tips.t_empl_all  where EMA_EQV_LEVEL='I1' and ema_disch_dt is null and ema_perno=:ema_perno"

            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("ema_perno", pernr.ToUpper().ToString())
            Dim da As New OracleDataAdapter(cm)
            Dim d As New DataTable()
            da.Fill(d)
            If d.Rows.Count > 0 Then
                perno = d.Rows(0)("EMA_ENAME").ToString()
            End If
        Catch ex As Exception

        End Try

        Return perno
    End Function
    Public Function ChkAuth(pno As String) As Boolean
        Try
            Dim chk As Boolean = False
            Dim qry As New OracleCommand()
            qry.CommandText = "select * from tips.t_empl_all  where ema_perno=:ema_perno and EMA_EQV_LEVEL='I1' and ema_disch_dt is null "
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_perno", pno.ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chk = True
            Else
                chk = False
            End If
            Return chk
        Catch ex As Exception

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
            BindAssesorGrid(perno.Text)
            If b.Text = "Approved" Then
                GridView1.Visible = True
                gvfinal.Visible = False
                btnrej.Visible = False
                lbOrg.Visible = False
                txtremarks.Visible = False
                btnaddnontsl.Visible = False
                btnaddpeertsl.Visible = False
            Else
                GridView1.Visible = False
                gvfinal.Visible = True
                btnrej.Visible = True
                lbOrg.Visible = True
                txtremarks.Visible = True
                btnaddnontsl.Visible = True
                btnaddpeertsl.Visible = True
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub SurveyApproval_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub

    Private Sub SurveyApproval_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                bindGrid()
                PopDroupdown()
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

    Protected Sub GvCateg_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim id = CType(e.Row.FindControl("lblcatrg"), Label)
            Dim chk = CType(e.Row.FindControl("chkmgr"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)

            Dim comnd As New OracleCommand()

            comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_DEL_TAG ='N' and ss_pno='" & perno.Text & "'  and ss_year='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO='" & Session("assespno").ToString() & "' and SS_CATEG='" & id.Text & "'"

            Dim d = getRecordInDt(comnd, conHrps)

            If d.Rows.Count > 0 Then
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

    Protected Sub chkmgr_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chkmgr"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim aperno = CType(gv.FindControl("lblassess"), Label)
            Dim categ = CType(gv.FindControl("lblcatrg"), Label)
            Dim email = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If id.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString(), aperno.Text, email.Text)
            Else
                val = Check(ViewState("FY").ToString(), aperno.Text, id.Text)
            End If
            If chk.Checked = True Then

                If val = "" Then
                    UpdateData(id.Text, "N", categ.Text)
                    BindAssesorGrid(aperno.Text)
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If
                'ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                BindAssesorGrid(aperno.Text)
            Else
                UpdateData(id.Text, "Y", categ.Text)
                ' ShowGenericMessageModal(CommonConstants.AlertType.warning, " De-Selected...!")
                BindAssesorGrid(aperno.Text)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub UpdateData(id As String, tag As String, catg As String)
        Try
            Dim perno = Session("USER_ID").ToString()
            Dim query As String = String.Empty
            query = "update t_survey_status set SS_DEL_TAG ='" & tag & "', SS_UPDATED_DT=sysdate,SS_UPDATED_BY='" & perno & "' where SS_PNO='" & id & "' "
            query += "and ss_year='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO ='" & Session("assespno").ToString & "' and ss_categ='" & catg & "'"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub UpdateData1(id As String, tag As String, categ As String)
        Try
            Dim query As String = String.Empty
            Dim perno = Session("USER_ID").ToString()
            query = "update t_survey_status set SS_DEL_TAG ='" & tag & "',ss_categ='" & categ & "',SS_UPDATED_DT=sysdate,SS_UPDATED_BY='" & perno & "' where SS_PNO='" & id & "'  and ss_year='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO ='" & Session("assespno").ToString & "'"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
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
        Reset()
    End Sub
    Protected Sub btnaddnontsl_Click(sender As Object, e As EventArgs)
        divtsl.Visible = False
        divntsl.Visible = True
        Reset()
    End Sub
    Protected Sub btnaddnorgI_Click(sender As Object, e As EventArgs)
        Try
            If txtnamenI.Text.Trim() = "" Or txtdeptnI.Text.Trim() = "" Or txtdesgnI.Text.Trim() = "" Or txtemailnI.Text.Trim() = "" Or ddlrole.SelectedValue = "Select assessors category" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, " Please Fill All Column and select Assessor category also ...!")
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

            Dim val = Check(ViewState("FY").ToString, assesor.ToString(), email)

            If val = "" Then

                SaveData(ddlrole.SelectedValue.ToString(), pno, name, desg, dept, email, "", "NORG", "SE", Session("assespno"))
                Reset()
                BindAssesorGrid(assesor)
                ShowGenericMessageModal(CommonConstants.AlertType.success, " Added ...!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SurveyApproval
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select ema_ename ||'('|| ema_perno ||')' EName from tips.t_empl_all where (ema_perno like  :ema_perno or upper(ema_ename) like "
            cmd.CommandText += " :ema_ename)  and ema_disch_dt is null and EMA_COMP_CODE='1000'"


            ob.conHrps.Open()

            ' cmd.Connection = ob.conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", "%" & prefixText.ToUpper & "%")
            cmd.Parameters.AddWithValue("ema_ename", "%" & prefixText.ToUpper & "%")
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
                Dim perno = txtpnoI.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgI.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptI.Text.Trim, "'", "''")
                Dim email = Replace(txtemailI.Text.Trim, "'", "''")

                Dim val = Check(ViewState("FY").ToString(), assesor, pno)
                If val = "" Then
                    SaveData(DropDownList1.SelectedValue.ToString, pno, name, desg, dept, email, lbldesg1.Text, "ORG", "SE", assesor)
                    Reset()
                    BindAssesorGrid(assesor)
                    ShowGenericMessageModal(CommonConstants.AlertType.success, " Added ...!")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    Exit Sub
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank, Please fill and select assessor category also...!")
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
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

    Public Function Check(year As String, assespno As String, pno As String) As String
        Dim vl As String = String.Empty
        Try
            Dim str As New OracleCommand()
            str.CommandText = " select SS_PNO,irc_desc from hrps.t_survey_status,hrps.t_ir_codes where  SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO "
            str.CommandText += "and ((SS_PNO=:SS_PNO) OR (upper(ss_email)=:SS_PNO)) and SS_status='SE' and ss_del_tag ='N' and irc_type='360RL' and upper(ss_categ)=upper(irc_code)"

            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
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

    Public Sub SaveData(ByVal role As String, ByVal pno As String, ByVal name As String, ByVal desg As String, ByVal org As String, ByVal email As String, ByVal lvl As String, ByVal orgtype As String, ByVal status As String, assespno As String)

        Try
            Dim OrgStr As String = String.Empty
            Dim id = getRefNo()

            If pno = "" Then
                pno = id
            End If
            OrgStr = "insert into T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL,ss_wfl_status,ss_approver) values (:SS_CATEG,:SS_ID,:SS_PNO,:SS_NAME,:SS_DESG,:SS_DEPT,:SS_EMAIL,:SS_STATUS,"
            OrgStr += " :SS_TAG,:SS_CRT_BY,sysdate,:SS_DEL_TAG,:SS_TYPE,:ss_year,:SS_ASSES_PNO,:SS_LEVEL,'1',:ss_approver)"

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
            comnd.Parameters.AddWithValue("SS_STATUS", status)
            comnd.Parameters.AddWithValue("SS_TAG", "SU")
            comnd.Parameters.AddWithValue("SS_CRT_BY", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("ss_approver", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("SS_DEL_TAG", "N")
            comnd.Parameters.AddWithValue("SS_TYPE", orgtype)
            comnd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
            comnd.Parameters.AddWithValue("SS_LEVEL", lvl)
            comnd.ExecuteNonQuery()
            ' comnd.ExecuteNonQuery()
            ' Clear()

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

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

    Protected Sub txtpnoI_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoI.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            strself.CommandText += " (ema_ename||ema_perno)=:pno and ema_disch_dt is null and EMA_COMP_CODE='1000'"
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtdesgI.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtemailI.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtemailI.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgI.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()

                lbldesg1.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

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

    Public Function ChkValidation() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            cmdqry.CommandText = "select a.IRC_CODE,SUBSTR(a.IRC_DESC,0,1) minmum, SUBSTR(a.IRC_DESC,3,2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type='360VL' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            Dim dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("assespno").ToString() & "' "
                    cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' AND SS_DEL_TAG='N' and ss_year='" & ViewState("FY").ToString() & "'"
                    Dim dtls = getRecordInDt(cmdqry, conHrps)

                    If dtls.Rows.Count < dt.Rows(i)("minmum") Then
                        status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("minmum").ToString() & "), "
                    End If
                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count > dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & ", "
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Protected Sub lbOrg_Click(sender As Object, e As EventArgs)
        Try
            Dim stat = ChkValidation()
            If Len(stat) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum/Maximum no of assessors in " & stat & " Category")
                Exit Sub
            End If
            ' Dim tg As Boolean = False
            For i = 0 To gvfinal.Rows.Count - 1
                Dim perno As String = String.Empty
                Dim chk = CType(gvfinal.Rows(i).FindControl("chkmgr"), CheckBox)
                perno = CType(gvfinal.Rows(i).FindControl("lblpno"), Label).Text
                If chk.Checked = True Then
                    Approve(perno, "AP")
                End If

            Next
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Approved...")
            gvfinal.Visible = False
            GridView1.Visible = True
            bindGrid()
            BindAssesorGrid(Session("assespno").ToString)
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Approve(ByVal pno As String, ByVal tag As String)
        Try
            Dim qry As String = String.Empty
            'qry = "update t_survey_status set SS_APP_TAG='" & tag & "',SS_TAG_DT=sysdate,SS_WFL_STATUS='2' where SS_PNO='" & pno & "'  and ss_year='" & ViewState("FY").ToString() & "'"
            'qry += "  and SS_ASSES_PNO ='" & Session("assespno") & "'"

            qry = "update t_survey_status set SS_APP_TAG=:SS_APP_TAG,SS_TAG_DT=sysdate,SS_WFL_STATUS='2',SS_UPDATED_BY=:SS_UPDATED_BY, SS_UPDATED_DT=sysdate where "
            qry += " SS_PNO=:SS_PNO and ss_year=:ss_year and SS_ASSES_PNO =:SS_ASSES_PNO"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim c As New OracleCommand()
            c.CommandText = qry
            c.Connection = conHrps
            c.Parameters.Clear()
            c.Parameters.AddWithValue("SS_APP_TAG", tag)
            c.Parameters.AddWithValue("SS_PNO", pno)
            c.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            c.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
            c.Parameters.AddWithValue("SS_UPDATED_BY", Session("USER_ID").ToString())
            c.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub gvself_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim assespno As String = String.Empty
            assespno = CType(e.Row.FindControl("lblpno"), Label).Text
            Dim btn = CType(e.Row.FindControl("lbpendingapproval"), LinkButton)

            Dim qry As New OracleCommand()
            qry.CommandText = "select distinct SS_APP_TAG from t_survey_status  where SS_ASSES_PNO='" & assespno & "'  and ss_year='" & ViewState("FY").ToString() & "' and SS_TAG='SU' and SS_DEL_TAG='N'"
            Dim s = getRecordInDt(qry, conHrps)
            If s.Rows(0)(0).ToString() = "AP" Then
                btn.Text = "Approved"
                ' btn.Enabled = False
                'gvfinal.Columns(7).Visible = False
            ElseIf s.Rows(0)(0).ToString() = "RJ" Then
                btn.Text = "Returned"
            Else
                btn.Text = "Pending"
                ' btn.Enabled = True
                'gvfinal.Columns(7).Visible = True

            End If
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try
    End Sub
    Public Sub CheckApproved(pno As String)

        Try
            Dim qry As New OracleCommand()
            qry.CommandText = "select * from t_survey_status  where SS_APP_TAG='AP' and SS_ASSES_PNO='" & pno & "'  and ss_year='" & ViewState("FY").ToString() & "'"
            Dim s = getRecordInDt(qry, conHrps)
            If s.Rows.Count > 0 Then
                btnaddpeertsl.Visible = False
                btnaddnontsl.Visible = False
                lbOrg.Visible = False
                btnrej.Visible = False
                divtitle.Visible = True
                txtremarks.Visible = False
            Else
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                lbOrg.Visible = True
                divtitle.Visible = True
                btnrej.Visible = True
                txtremarks.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnrej_Click(sender As Object, e As EventArgs)
        Try
            If txtremarks.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please enter the remarks for sending the form back to the officer")
                Exit Sub
            End If
            Dim uprem As New OracleCommand()
            uprem.Connection = conHrps
            Dim remarks = Replace(txtremarks.Text.Trim(), "'", "''")
            Dim p = Session("assespno").ToString()
            ' uprem.CommandText = " update t_survey_status set SS_REMARKS ='" & remarks & "' ,SS_APP_TAG='RJ', SS_TAG_DT=sysdate "
            ' uprem.CommandText += " where SS_ASSES_PNO ='" & p & "'  and ss_year='" & ViewState("FY").ToString() & "' "
            uprem.CommandText = " update t_survey_status set SS_REMARKS =:SS_REMARKS,SS_APP_TAG='RJ', SS_TAG_DT=sysdate ,ss_wfl_status=null,ss_tag='N', "
            uprem.CommandText += "SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO =:SS_ASSES_PNO and ss_year=:ss_year "
            uprem.Parameters.Clear()
            uprem.Parameters.AddWithValue("SS_REMARKS", remarks)
            uprem.Parameters.AddWithValue("SS_ASSES_PNO", p)
            uprem.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            uprem.Parameters.AddWithValue("SS_UPDATED_BY", Session("USER_ID").ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            uprem.ExecuteNonQuery()
            ShowGenericMessageModal(CommonConstants.AlertType.success, "The Form Has Been sent back to the officer...!")
            bindGrid()
            SentMailReturned(p, remarks)
        Catch ex As Exception
            MsgBox(ex.ToString())
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub SentMailReturned(pno As String, remarks As String)
        Try
            Dim strmailcomd As New OracleCommand()
            strmailcomd.CommandText = "Select ema_email_id from tips.t_empl_all where ema_perno='" & pno.ToString & "' and ema_comp_code='1000' and ema_disch_dt is null"

            Dim df = getRecordInDt(strmailcomd, conHrps)
            If df.Rows.Count > 0 Then
                Dim link As String = ""
                link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/SelectAssesor.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/SelectAssesor.aspx <a/>"
                Dim body As String = String.Empty
                body = " Dear " & lblassname.Text & ", <br/><br/>The list of respondents which you had submitted for evaluative 360-degree assessment for IL2s,"
                body += "as a part of the end year performance management process, has been returned by your concerned IL1 with following remarks: " & remarks & " <br/>"
                body += "<br/>You are requested to modify the list of respondents as per comments / instructions of your IL1 and submit the same within next 2 days."
                body += " <br/><br/> Link for accessing the form: <br/> " & link
                body += " If you have a query, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development.<br/><br/><br/><br/> With regards,"
                body += " <br/> Zubin Palia <br/> Chief Group HR & IR"

                Dim mail As New System.Net.Mail.MailMessage()
                mail.Bcc.Add(df.Rows(0)("ema_email_id"))
                mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

                mail.Subject = "List of respondents for end-year 360-degree feedback returned"

                mail.SubjectEncoding = System.Text.Encoding.UTF8
                mail.Body = body

                mail.BodyEncoding = System.Text.Encoding.UTF8
                mail.IsBodyHtml = True
                Dim client As New SmtpClient()
                client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
                client.Port = 25
                client.Host = "144.0.11.253"
                ' client.Host = "144.0.16.7"
                client.EnableSsl = False
                client.Send(mail)
                client.Dispose()

            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
