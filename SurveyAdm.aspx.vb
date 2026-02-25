Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Partial Class SurveyAdm
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Public Sub bindGrid()
        Try
            If ChkRole() Then

                If Request.QueryString("adm") = "1" Then
                    bindpendinggrid()
                    GridView1.Visible = True
                    gvself.Visible = False
                    resp.InnerText = "View Pending Approval"
                Else

                    resp.InnerText = "ADD RESPONDENT"
                    Dim qry As New OracleCommand
                    qry.CommandText = " select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc, case when ss_approver is null then '' else b.ema_ename "
                    qry.CommandText += "  ||'('||SS_APPROVER ||')' end Approver from "
                    qry.CommandText += " t_survey_status, tips.t_empl_all a,tips.t_empl_all b    where a.ema_perno = SS_ASSES_PNO  and  SS_APPROVER=b.ema_perno(+) "
                    qry.CommandText += " and ss_status='SE'  and ss_year='" & ViewState("FY").ToString() & "' and (ss_del_tag='N' or ss_del_tag is null)"
                    Dim dt = getRecordInDt(qry, conHrps)
                    GridView1.Visible = False
                    gvself.Visible = True
                    If dt.Rows.Count > 0 Then

                        gvself.DataSource = dt
                        gvself.DataBind()
                    Else
                        gvself.DataSource = Nothing
                        gvself.DataBind()

                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub bindpendinggrid()
        Try
            Dim qry As New OracleCommand()
            Dim dt As New DataTable()

            qry.CommandText = " select distinct SS_ASSES_PNO,a.ema_ename,a.ema_desgn_desc, case when ss_approver is null then '' else b.ema_ename "
            qry.CommandText += "  ||'('||SS_APPROVER ||')' end Approver from "
            qry.CommandText += " t_survey_status, tips.t_empl_all a,tips.t_empl_all b    where a.ema_perno = SS_ASSES_PNO  and  SS_APPROVER=b.ema_perno(+) "
            qry.CommandText += " and ss_status='SE' and ss_del_tag='N' and SS_APPROVER is not null and ss_app_tag is null and ss_year='" & ViewState("FY").ToString() & "'"
            dt = getRecordInDt(qry, conHrps)
            If dt.Rows.Count > 0 Then
                GridView1.DataSource = dt
                GridView1.DataBind()
            Else
                GridView1.DataSource = Nothing
                GridView1.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False
            Dim cmd As New OracleCommand()
            cmd.CommandText = "select * from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            cmd.CommandText += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            Dim f = getRecordInDt(cmd, conHrps)
            If f.Rows.Count > 0 Then
                t = True
            Else
                t = False
            End If
            Dim r As New OracleCommand()
            r.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY from dual"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)(0).ToString()
            End If
            Return t
        Catch ex As Exception

        End Try
    End Function

    Public Sub BindAssesorGrid(pno As String)
        Try
            Dim qry As New OracleCommand()
            qry.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer','ROPT','Subordinates'"
            qry.CommandText += " ,'INTSH','Internal Stakeholder',SS_CATEG) Category,SS_CATEG,SS_DEL_TAG from t_survey_status where SS_ASSES_PNO='" & pno & "' and SS_STATUS='SE' and ss_year='" & ViewState("FY").ToString() & "' order by Category"
            Dim dt = getRecordInDt(qry, conHrps)

            If dt.Rows.Count > 0 Then
                gvfinal.DataSource = dt
                gvfinal.DataBind()
                btnaddnontsl.Visible = True
                btnaddpeertsl.Visible = True
                btnsubmit.Visible = True
                gvfinal.Visible = True
                '  lbOrg.Visible = True
                divtitle.Visible = True
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

            Dim qry1 As New OracleCommand()
            qry1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer','ROPT',"
            qry1.CommandText += "'Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,SS_CATEG,SS_DEL_TAG from t_survey_status where ss_asses_pno='" & pno & "'"
            'qry1.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and ss_del_tag='N' and ss_tag='SU' and SS_STATUS='SE' order by Category"
            qry1.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and ss_del_tag='N' and SS_STATUS='SE' order by Category"
            Dim dth = getRecordInDt(qry1, conHrps)

            If dth.Rows.Count > 0 Then
                GridView2.DataSource = dth
                GridView2.DataBind()
            Else
                GridView2.DataSource = Nothing
                GridView2.DataBind()
            End If

            Dim cmdqry As New OracleCommand()
            cmdqry.CommandText = "select a.IRC_CODE,SUBSTR(a.IRC_DESC,0,1) minmum, trim(substr(a.irc_desc,3,2)) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type='360VL' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"
            Dim dt1 = getRecordInDt(cmdqry, conHrps)
            Dim categ As String = String.Empty
            For d = 0 To dt1.Rows.Count - 1

                Dim qryq As New OracleCommand()
                qryq.CommandText = "select * from t_survey_status where ss_asses_pno='" & pno & "' and ss_status='SE' and ss_del_tag='N' and upper(ss_categ) = "
                qryq.CommandText += "upper( '" & dt1.Rows(d)("IRC_CODE") & "') and ss_year='" & ViewState("FY").ToString() & "'"
                Dim s = getRecordInDt(qryq, conHrps)
                If dt1.Rows(d)("maximum") = "N" Then
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
            Dim w = categ.Split(",")
            categ = categ.TrimEnd(",")
            If w.Length = 2 Then
                categ = categ.TrimEnd(",")
                lbls.Text = ""
                lbls.Text = "Minimum criteria not completed for " & categ
            Else
                Dim ls = categ.Substring(categ.LastIndexOf(",") + 1)

                Dim c = categ.Remove(categ.LastIndexOf(",") + 1)

                If categ <> "" Then
                    lbls.Text = ""
                    lbls.Text = "Minimum criteria not completed for " & c.TrimEnd(",") & " and  " & ls & " Category"
                Else
                    lbls.Text = ""
                End If
            End If





        Catch ex As Exception

        End Try
    End Sub

    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""

        If Session("ADM_USER") = "" Then

            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            'strUserID = "151629"
            Session("ADM_USER") = strUserID.ToUpper()
            If GetPno(strUserID.ToUpper()) = False Then
                Response.Redirect("errorpage.aspx", True)
            End If

            lblname.Text = "Admin"
            'lblname.Text = "Suresh Dutt Tripathi"
        ElseIf (Session("ADM_USER") IsNot Nothing) AndAlso (Session("ADM_USER").Equals("") = False) Then
            ' lblname.Text = GetPno().Rows(0)(1) '"Suresh Dutt Tripathi"
            Return
        Else
        End If
    End Sub

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
                GridView2.Visible = True
                gvfinal.Visible = False
                btnaddpeertsl.Visible = False
                btnaddnontsl.Visible = False
                btnsubmit.Visible = False
            ElseIf b.Text = "Returned" Then
                GridView2.Visible = False
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                btnsubmit.Visible = True
                gvfinal.Visible = True
            Else
                GridView2.Visible = False
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                btnsubmit.Visible = True
                gvfinal.Visible = True
            End If
        Catch ex As Exception

        End Try

    End Sub

    Public Sub Approve(ByVal pno As String, ByVal tag As String)
        Try
            Dim qry As String = String.Empty
            qry = "update t_survey_status set SS_APP_TAG='" & tag & "',SS_TAG_DT = sysdate, SS_ACTION_BY='ADM',SS_WFL_STATUS='2',SS_UPDATED_BY= "
            qry += "'" & Session("ADM_USER").ToString() & "',SS_UPDATED_DT=sysdate where  ss_year='" & ViewState("FY").ToString() & "'  and SS_ASSES_PNO ='" & pno & "' "
            qry += "and SS_STATUS='SE' and SS_DEL_TAG='N' and SS_TAG ='SU' "
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim c As New OracleCommand()
            c.CommandText = qry
            c.Connection = conHrps
            c.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Private Sub SurveyApproval_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub

    Private Sub SurveyApproval_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If Request.QueryString("adm") = "2" Then
                OverAll()
            End If
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
            ' Dim id = CType(e.Row.FindControl("hdfnid"), HiddenField)
            Dim chk = CType(e.Row.FindControl("chkmgr"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)
            Dim delTag = CType(e.Row.FindControl("lblDelTag"), Label)

            '******************Commented by Manoj Kumar 18-01-2021
            'Dim comnd As New OracleCommand()
            'comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_DEL_TAG ='N' and ss_pno='" & perno.Text & "' and ss_year='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO='" & Session("assespno").ToString & "'"

            'Dim d = getRecordInDt(comnd, conHrps)

            'If d.Rows.Count > 0 Then
            '    chk.Checked = True
            'Else
            '    chk.Checked = False
            'End If

            '*********************************

            If delTag.Text.ToString = "N" Then
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
            Dim aperno = CType(gv.FindControl("lblapno"), Label)
            Dim catgry = CType(gv.FindControl("lblCate"), Label)
            Dim email = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If id.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString(), aperno.Text, email.Text)
            Else
                val = Check(ViewState("FY").ToString(), aperno.Text, id.Text)
            End If

            If chk.Checked = True Then

                If val = "" Then
                    UpdateData(id.Text, "N", catgry.Text)
                    BindAssesorGrid(aperno.Text)
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If

            Else
                UpdateData(id.Text, "Y", catgry.Text)
                ' ShowGenericMessageModal(CommonConstants.AlertType.warning, " De-Selected...!")
                BindAssesorGrid(aperno.Text)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub UpdateData(id As String, tag As String, catg As String)
        Try
            Dim query As String = String.Empty
            query = "update t_survey_status set SS_DEL_TAG ='" & tag & "',SS_ADM_TAG='1', SS_ADM_UPD_DT=SYSDATE,SS_UPDATED_DT=sysdate,"  'Add by Manoj Kumar(osj1874) 18-01-2021
            query += " SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "' where SS_PNO='" & id & "' and ss_year='" & ViewState("FY").ToString() & "'"
            query += "  and SS_ASSES_PNO='" & Session("assespno").ToString & "' and SS_CATEG='" & catg & "' and nvl(SS_APP_TAG,'N') <> 'AP'"
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

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SurveyAdm
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
    Public Sub UpdateData1(id As String, tag As String, categ As String)
        Try
            Dim query As String = String.Empty
            query = "update t_survey_status set SS_DEL_TAG ='" & tag & "',ss_categ='" & categ & "',SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "',"
            query += " SS_UPDATED_DT=sysdate where (SS_PNO or upper(ss_email))='" & id & "'  and ss_year='" & ViewState("FY").ToString() & "' "
            query += " and SS_ASSES_PNO ='" & Session("assespno").ToString & "' "

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

    Public Sub SaveData(ByVal role As String, ByVal pno As String, ByVal name As String, ByVal desg As String, ByVal org As String, ByVal email As String, ByVal lvl As String, ByVal orgtype As String, assespno As String)

        Try

            Dim OrgStr As String = String.Empty
            Dim id = getRefNo()

            If pno = "" Then
                pno = id
            End If
            OrgStr = "insert into T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL,SS_ADM_TAG,SS_APP_TAG,SS_APPROVER,SS_WFL_STATUS) values ('" & role & "', '" & id & "','" & pno & "','" & name & "','" & Replace(desg, "'", "''") & "','" & Replace(org, "'", "''") & "','" & email & "','SE','', "
            OrgStr += " '" & Session("ADM_USER").ToString() & "', sysdate,'N','" & orgtype & "','" & ViewState("FY") & "','" & assespno & "','" & lvl & "','1','','','')"

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
            strself.CommandText += " (ema_ename||'(' ||ema_perno ||')')='" & pno.ToString() & "' and ema_disch_dt is null  and ema_comp_code='1000'" '*********Added by Manoj Kumar 18-01-2021
            Dim dt = getRecordInDt(strself, conHrps)

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

    Protected Sub gvself_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim assespno As String = String.Empty
            assespno = CType(e.Row.FindControl("lblpno"), Label).Text
            Dim btn = CType(e.Row.FindControl("lblstats"), Label)
            Dim appbtn = CType(e.Row.FindControl("lbpendingapproval"), LinkButton)
            Dim p As Boolean = False

            Dim cmdqry As New OracleCommand()
            cmdqry.CommandText = "select a.IRC_CODE,SUBSTR(a.IRC_DESC,0,1) minmum, trim(substr(a.irc_desc,3,2)) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type='360VL' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            Dim dt = getRecordInDt(cmdqry, conHrps)

            For d = 0 To dt.Rows.Count - 1

                Dim qry As New OracleCommand()
                qry.CommandText = "select * from t_survey_status where ss_asses_pno='" & assespno & "' and ss_status='SE' and ss_del_tag='N' and upper(ss_categ) = "
                qry.CommandText += " upper('" & dt.Rows(d)("IRC_CODE") & "') and ss_year='" & ViewState("FY").ToString() & "'"
                Dim s = getRecordInDt(qry, conHrps)
                If dt.Rows(d)("maximum") = "N" Then
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

            If tag = "AP" Then
                appbtn.Text = "Approved"
            ElseIf tag = "RJ" Then
                appbtn.Text = "Returned"
            Else
                appbtn.Text = "Detail"
            End If
        Catch ex As Exception
            ' MsgBox(ex.ToString)
        End Try
    End Sub

    Public Function CheckApproved(pno As String) As String
        Try
            Dim tg As String
            Dim cm As New OracleCommand()
            cm.CommandText = "select distinct ss_app_tag from t_survey_status  where ss_asses_pno='" & pno & "' and ss_year='" & ViewState("FY").ToString() & "'"
            cm.CommandText += " and ss_del_tag='N' and ss_status='SE'"
            Dim dt = getRecordInDt(cm, conHrps)
            If dt.Rows.Count > 0 Then
                tg = dt.Rows(0)(0).ToString()
            Else
                tg = ""
            End If
            Return tg
        Catch ex As Exception

        End Try
    End Function

    Protected Sub btnsubmit_Click(sender As Object, e As EventArgs)
        Try
            Dim stat = ChkValidation()
            If Len(stat) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & stat & " Category")
                Exit Sub
            End If
            Dim comd As New OracleCommand()
            Dim approver As String = String.Empty
            comd.CommandText = " select EMA_PERS_EXEC_PNO "
            comd.CommandText += " from tips.t_empl_all where ema_perno='" & Session("assespno").ToString() & "' and EMA_COMP_CODE='1000' "
            Dim dt = getRecordInDt(comd, conHrps)
            If dt.Rows.Count > 0 Then
                approver = dt.Rows(0)(0).ToString()

            End If
            Dim upqry As String = String.Empty
            upqry = " update t_survey_status set SS_FLAG1 ='1',SS_TAG='SU',SS_APPROVER='" & approver & "',SS_WFL_STATUS='1',SS_ADM_UPD_DT=sysdate,SS_UPDATED_DT=sysdate,"
            upqry += " SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "'  where  SS_ASSES_PNO ='" & Session("assespno").ToString() & "' and ss_year='" & ViewState("FY") & "'"
            upqry += " and ss_del_tag='N' and ss_status='SE'"
            Dim c As New OracleCommand()
            c.CommandText = upqry
            c.Connection = conHrps
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            c.ExecuteNonQuery()
            BindAssesorGrid(Session("assespno").ToString())
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Form has been submitted...!")
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Function ChkValidation() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            cmdqry.CommandText = "Select a.IRC_CODE,SUBSTR(a.IRC_DESC,0,1) minmum, trim(substr(a.irc_desc,3,2)) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
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
    Protected Sub lbpend_Click(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, LinkButton).Parent.Parent
            Dim asspno = CType(gv.FindControl("lblpno1"), Label)

            Approve(asspno.Text, "AP")
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Approved...!")
            bindpendinggrid()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lnkreset_Click(sender As Object, e As EventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openMode", "showmodalAddSabashAwardee();", True)
    End Sub
    Protected Sub btnreset_Click(sender As Object, e As EventArgs)
        Try
            If txtperno.Text.Trim <> "" Then
                Dim com As New OracleCommand()
                com.CommandText = "update t_survey_status set ss_tag='N',ss_approver=null,ss_tag_dt=null, ss_wfl_status=null,SS_UPDATED_BY='" & Session("ADM_USER").ToString() & "',SS_UPDATED_DT=sysdate where ss_year='" & ViewState("FY").ToString() & "'"
                com.CommandText += "  and ss_tag='SU' and upper(ss_asses_pno)='" & txtperno.Text.Trim.ToUpper() & "' and ss_app_tag is null"

                com.Connection = conHrps
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim r = com.ExecuteNonQuery()
                If r > 0 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Returned...!")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Incorrect P.no entred...!")
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter Personal number...!")
                lnkreset_Click(sender, e)
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnrevert_Click(sender As Object, e As EventArgs)
        Try
            Dim upres As New OracleCommand()
            upres.CommandText = "update t_survey_status  set ss_wfl_status=:ss_wfl_status1, SS_UPDATED_DT=sysdate,SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO=:SS_ASSES_PNO and SS_PNO=:SS_PNO and "
            upres.CommandText += "ss_wfl_status in('3','9') and ss_year=:ss_yar"

            upres.Connection = conHrps
            upres.Parameters.Clear()
            upres.Parameters.AddWithValue("ss_wfl_status1", "2")
            upres.Parameters.AddWithValue("SS_ASSES_PNO", txtassespno.Text.Trim())
            upres.Parameters.AddWithValue("SS_PNO", txtrespno.Text.Trim())
            upres.Parameters.AddWithValue("SS_UPDATED_BY", Session("ADM_USER").ToString())
            ' upres.Parameters.AddWithValue("ss_wfl_status", s)
            upres.Parameters.AddWithValue("ss_yar", ViewState("FY").ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result = upres.ExecuteNonQuery()
            txtassespno.Text = ""
            txtrespno.Text = ""
            If result > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Feedback has been reverted...!")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No feedback has been reverted...!")
            End If

        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub OverAll()
        Try
            Dim str As New OracleCommand()
            str.CommandText = "SELECT ac.ss_asses_pno, ac.ema_ename, ac.ema_empl_sgrade, ac.ema_desgn_desc, ac.ema_exec_head_desc, 'Z-Overall' a1, round(SUM(a) / COUNT(*),3) a, round(SUM(b) / COUNT "
            str.CommandText += " (*),3) b, round(SUM(c) / COUNT(*),3) c, round(SUM(d) / COUNT(*),3) d, ( CASE  WHEN round(SUM(a) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(a) / COUNT(*),3) <= '2.5' "
            str.CommandText += " AND round(SUM(a) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(a) / COUNT(*),3) <= '3' AND round(SUM(a) / COUNT(*),3) > '2.5' THEN 'G' END ) r1, "
            str.CommandText += " ( CASE WHEN round(SUM(b) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(b) / COUNT(*),3) <= '2.5' AND round(SUM(b) / COUNT(*),3) >= '1.6' THEN 'A' "
            str.CommandText += "WHEN round(SUM(b) / COUNT(*),3) <= '3'  AND round(SUM(b) / COUNT(*),3) > '2.5' THEN 'G' END ) r2, ( CASE WHEN round(SUM(c) / COUNT(*),3) < '1.6' THEN  "
            str.CommandText += "'U' WHEN round(SUM(c) / COUNT(*),3) <= '2.5' AND round(SUM(c) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(c) / COUNT(*),3) <= '3' AND round(SUM(c) "
            str.CommandText += "/ COUNT(*),3) > '2.5' THEN 'G' END ) r3, ( CASE  WHEN round(SUM(d) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(d) / COUNT(*),3) <= '2.5' AND round "
            str.CommandText += "(SUM(d) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(d) / COUNT(*),3) <= '3' AND round(SUM(d) / COUNT(*),3) > '2.5' THEN 'G' END ) r4,( ( CASE WHEN round(SUM(a) / COUNT(*),3) < '1.6' THEN 'U'"
            str.CommandText += "WHEN round(SUM(a) / COUNT(*),3) <= '2.5' AND round(SUM(a) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(a) / COUNT(*),3) <= '3' AND round(SUM(a) / COUNT(*),3) > '2.5'  "
            str.CommandText += "THEN 'G' END ) || ( CASE WHEN round(SUM(b) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(b) / COUNT(*),3) <= '2.5' AND round(SUM(b) / COUNT(*),3) >= '1.6' THEN 'A' "
            str.CommandText += " WHEN round(SUM(b) / COUNT(*),3) <= '3' AND round(SUM(b) / COUNT(*),3) > '2.5' THEN 'G' END ) || ( CASE WHEN round(SUM(c) / COUNT(*),3) < '1.6' THEN "
            str.CommandText += "'U' WHEN round(SUM(c) / COUNT(*),3) <= '2.5' AND round(SUM(c) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(c) / COUNT(*),3) <= '3' AND round(SUM(c) "
            str.CommandText += "/ COUNT(*),3) > '2.5' THEN 'G' END ) || ( CASE  WHEN round(SUM(d) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(d) / COUNT(*),3) <= '2.5' AND round "
            str.CommandText += "(SUM(d) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(d) / COUNT(*),3) <= '3' AND round(SUM(d) / COUNT(*),3) > '2.5' THEN 'G' END ) ) all1 FROM ( SELECT ss_asses_pno, ema_ename, ema_empl_sgrade, "
            str.CommandText += "ema_desgn_desc, ema_exec_head_desc, ss_categ, round(SUM(ss_q1_a) / COUNT(*),3) a, round(SUM(ss_q1_b) / COUNT(*),3) b, round(SUM(ss_q1_c) / COUNT(*),3) c, "
            str.CommandText += "round(SUM(ss_q1_d) / COUNT(*),3) d, COUNT(*) no_records, ( CASE  WHEN round(SUM(ss_q1_a) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(ss_q1_a) / COUNT "
            str.CommandText += "(*),3) <= '2.5' AND round(SUM(ss_q1_a) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(ss_q1_a) / COUNT(*),3) <= '3' AND round(SUM(ss_q1_a) / COUNT(*),3) > "
            str.CommandText += "'2.5' THEN 'G' END ) r1, ( CASE  WHEN round(SUM(ss_q1_b) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(ss_q1_b) / COUNT(*),3) <= '2.5' AND round(SUM "
            str.CommandText += "(ss_q1_b) / COUNT(*),3) >= '1.6' THEN 'A' WHEN round(SUM(ss_q1_b) / COUNT(*),3) <= '3' AND round(SUM(ss_q1_b) / COUNT(*),3) > '2.5' THEN 'G' END ) r2, ( "
            str.CommandText += "CASE WHEN round(SUM(ss_q1_c) / COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(ss_q1_c) / COUNT(*),3) <= '2.5' AND round(SUM(ss_q1_c) / COUNT(*),3) >= '1.6' "
            str.CommandText += "THEN 'A' WHEN round(SUM(ss_q1_c) / COUNT(*),3) <= '3' AND round(SUM(ss_q1_c) / COUNT(*),3) > '2.5' THEN 'G' END ) r3, ( CASE WHEN round(SUM(ss_q1_d) /  "
            str.CommandText += "COUNT(*),3) < '1.6' THEN 'U' WHEN round(SUM(ss_q1_d) / COUNT(*),3) <= '2.5' AND round(SUM(ss_q1_d) / COUNT(*),3) >= '1.6' THEN 'A'  WHEN round(SUM "
            str.CommandText += "(ss_q1_d) / COUNT(*),3) <= '3' AND round(SUM(ss_q1_d) / COUNT(*),3) > '2.5' THEN 'G' END ) r4  FROM hrps.t_survey_status, tips.t_empl_all WHERE ss_wfl_status = '3' AND ss_year = '2020' AND ss_status = 'SE' AND ss_del_tag = 'N' "
            str.CommandText += " AND ss_app_tag = 'AP' AND upper(ss_categ) <> 'SELF' AND ema_perno = ss_asses_pno AND ema_disch_dt IS NULL GROUP BY ss_asses_pno, ema_ename, ema_eqv_level, "
            str.CommandText += " ema_empl_sgrade, ema_desgn_desc,ema_exec_head_desc, ss_categ) ac GROUP BY ac.ss_asses_pno, ac.ema_ename, ac.ema_empl_sgrade, ac.ema_desgn_desc, "
            str.CommandText += "     ac.ema_exec_head_desc order by 1,2"
            str.CommandText += ""
            Dim dt = getRecordInDt(str, conHrps)


            If dt.Rows.Count > 0 Then
                gvoverall.DataSource = Nothing
                gvoverall.DataBind()
                gvoverall.DataSource = dt
                gvoverall.DataBind()
                ExportToExcel(gvoverall, "OverAll")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data found")
                gvoverall.DataSource = Nothing
                gvoverall.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub ExportToExcel(gv As GridView, name As String)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=" & name & ".xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            gv.AllowPaging = False
            Me.bindGrid()

            gv.HeaderRow.BackColor = Color.White
            For Each cell As TableCell In gv.HeaderRow.Cells
                cell.BackColor = gv.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In gv.Rows
                row.BackColor = Color.White
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = gv.AlternatingRowStyle.BackColor
                    Else
                        cell.BackColor = gv.RowStyle.BackColor
                    End If
                    cell.CssClass = "textmode"
                Next
            Next

            gv.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub

End Class
