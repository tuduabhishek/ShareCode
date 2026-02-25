Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Net.Mail
Imports ClosedXML.Excel
Partial Class RawData
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "Download Raw data"
                getFy()
                getsrlno()
                GetLevel()
                bindExecHead()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
    Private Sub getFy()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select DISTINCT SS_YEAR from hrps.T_SURVEY_STATUS order by SS_YEAR desc"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("SS_YEAR").ToString()
                ddlFinYr.DataSource = g
                ddlFinYr.DataTextField = "SS_YEAR"
                ddlFinYr.DataValueField = "SS_YEAR"
                ddlFinYr.DataBind()
                ddlFinYr.Items.Insert(0, New ListItem("All", "A"))
                ddlFinYr.SelectedIndex = 1
            End If
        Catch ex As Exception


        End Try
    End Sub
    Private Sub GetLevel()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select DISTINCT SS_QLEVEL from hrps.T_SURVEY_RESPONSE order by SS_QLEVEL desc"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then

                ddlLevel.DataSource = g
                ddlLevel.DataTextField = "SS_QLEVEL"
                ddlLevel.DataValueField = "SS_QLEVEL"
                ddlLevel.DataBind()
                ddlLevel.Items.Insert(0, New ListItem("All", "A"))
                ddlLevel.SelectedIndex = 0
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
            mycommand.CommandText = "select DISTINCT SS_SRLNO from hrps.T_SURVEY_STATUS order by SS_SRLNO desc"
            Dim dtsrl = getRecordInDt(mycommand, conHrps)
            If dtsrl.Rows.Count > 0 Then
                ddlCycle.DataSource = dtsrl
                ddlCycle.DataTextField = "SS_SRLNO"
                ddlCycle.DataValueField = "SS_SRLNO"
                ddlCycle.DataBind()
                ddlCycle.Items.Insert(0, New ListItem("All", "A"))
                ddlCycle.SelectedIndex = 0
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
            query.CommandText = "select distinct ema_exec_head,ema_exec_head_desc from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head_desc<>'Not found' and ema_exec_head<>'00000000' and ema_comp_code='1000'  and ema_year=:ema_year and ema_cycle=:ema_cycle"
            If ChkRole1() = False Then
                query.Parameters.Clear()
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
                ddlExecutive.DataValueField = "ema_exec_head_desc"
                ddlExecutive.DataTextField = "ema_exec_head_desc"
                ddlExecutive.DataBind()
                ddlExecutive.Items.Insert(0, New ListItem("All", "A"))
                ddlExecutive.SelectedIndex = 0
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
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Protected Sub btnsearch_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsearch.Click
        bindGrid()
    End Sub
    Public Sub bindGrid()
        Dim query As String = ""
        Try
            If ChkRole1() Then
                Dim qryrespond As New OracleCommand
                qryrespond.Connection = conHrps
                qryrespond.Parameters.Clear()

                query = "select t.SS_YEAR, t.SS_ASSES_PNO,t.SS_PNO,t.SS_QCODE, (t.SS_QCODE||'-'||q.SS_QTEXT) QTEXT, t.SS_QOPTN,t.SS_QLEVEL,t.SS_CREATED_BY, "
                query += " t.SS_MODIFIED_BY,to_char(t.SS_CREATED_DT, 'dd/mm/yy HH:MI:SS AM') SS_CREATED_DT,"
                query += " t.SS_SRL_NO,to_char(t.SS_MODIFIED_DT, 'dd/mm/yy HH:MI:SS AM')SS_MODIFIED_DT,t.SS_SERIAL,t.SS_FLAG, s.SS_CATEG, (t.SS_PNO||'-'||s.SS_NAME) Respodent,s.SS_DESG, s.SS_DEPT, s.SS_TYPE "
                query += " ,e.EMA_ENAME, c.IRC_DESC, "
                query += " (case when e.EMA_EQV_LEVEL in ('I2', 'I3') then decode(s.SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholders',s.SS_CATEG) "
                query += " when  e.EMA_EQV_LEVEL = 'I3' then decode(s.SS_CATEG,'MANGR','Manager/Superior','PEER','Peers and Subordinates','INTSH','Internal Stakeholders',s.SS_CATEG) "
                query += " when  e.EMA_EQV_LEVEL in ('I4', 'I5') then decode(s.SS_CATEG,'MANGR','Manager/Superior','PEER','Peers and Subordinates','INTSH','Internal Stakeholders',s.SS_CATEG) "
                query += " Else s.SS_CATEG end) SS_CATEG_Desc "
                query += " from hrps.T_SURVEY_RESPONSE t inner join hrps.T_SURVEY_QUESTION q "
                query += " On t.SS_YEAR=q.SS_YEAR And t.SS_QCODE=q.SS_QCODE And t.SS_QLEVEL=q.SS_QLEVEL "
                query += " inner join hrps.T_SURVEY_STATUS s On t.SS_YEAR=s.SS_YEAR And t.SS_ASSES_PNO=s.SS_ASSES_PNO "
                query += " And t.SS_SERIAL=s.SS_SRLNO And t.SS_PNO=s.SS_PNO "
                query += " Left outer join hrps.T_EMP_MASTER_FEEDBACK360 e On t.SS_YEAR=e.EMA_YEAR And t.SS_SERIAL=e.EMA_CYCLE And t.SS_ASSES_PNO=e.EMA_PERNO "
                query += " inner join T_IR_CODES C On t.SS_QCODE=c.IRC_CODE "
                query += " where 0<1 "
                If ddlFinYr.SelectedIndex > 0 Then query += " And t.SS_YEAR=:SS_YEAR "
                If ddlCycle.SelectedIndex > 0 Then query += " And t.SS_SERIAL=:SS_SERIAL "
                If ddlLevel.SelectedIndex > 0 Then query += " And t.SS_QLEVEL=:SS_QLEVEL "
                If ddlExecutive.SelectedIndex > 0 Then query += " And e.ema_exec_head_desc=:ema_exec_head_desc "
                If txtAssess.Text.Trim().Length > 0 Then query += " And t.SS_ASSES_PNO=:SS_ASSES_PNO "
                'ema_exec_head
                query += " order by t.SS_YEAR, t.SS_SERIAL,t.SS_QLEVEL, t.SS_ASSES_PNO, t.SS_PNO, t.SS_SERIAL "

                qryrespond.CommandText = query

                If ddlFinYr.SelectedIndex > 0 Then qryrespond.Parameters.Add("SS_YEAR", OracleType.VarChar, 4).Value = ddlFinYr.SelectedValue.Trim
                If ddlCycle.SelectedIndex > 0 Then qryrespond.Parameters.Add("SS_SERIAL", OracleType.VarChar, 1).Value = ddlCycle.SelectedValue.Trim
                If ddlLevel.SelectedIndex > 0 Then qryrespond.Parameters.Add("SS_QLEVEL", OracleType.VarChar, 10).Value = ddlLevel.SelectedValue.Trim
                If ddlExecutive.SelectedIndex > 0 Then qryrespond.Parameters.Add("ema_exec_head_desc", OracleType.VarChar, 100).Value = ddlExecutive.SelectedItem.Text.Trim
                If txtAssess.Text.Trim().Length > 0 Then qryrespond.Parameters.Add("SS_ASSES_PNO", OracleType.VarChar).Value = txtAssess.Text.Trim

                Dim dt = getDataInDt(qryrespond)
                gvself.Visible = True

                If dt.Rows.Count > 0 Then
                    gvself.DataSource = dt
                    gvself.DataBind()
                    lbtnExport.Visible = True
                    Session.Add("tmpdata", dt)
                Else
                    Session.Add("tmpdata", Nothing)
                    gvself.DataSource = Nothing
                    gvself.DataBind()
                    lbtnExport.Visible = False
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Sorry No Record Found")
                End If
            End If

        Catch ex As Exception
            query = ""
        End Try
    End Sub
    Protected Sub bindself()
        Try

            Dim assespno As String = String.Empty
            Dim btn As New Label
            Dim appbtn As New LinkButton
            For Each gv As GridViewRow In gvself.Rows
                assespno = CType(gv.FindControl("lblpno"), Label).Text
                btn = CType(gv.FindControl("lblstats"), Label)
                appbtn = CType(gv.FindControl("lbpendingapproval"), LinkButton)


                Dim p As Boolean = False

                Dim type1 As String = ChkAuthlabel(assespno)
                Dim type As String = String.Empty
                If type1.Equals("I5") Then
                    type = "360V5"
                ElseIf type1.Equals("I4") Then
                    type = "360V4"
                ElseIf type1.Equals("I3") Then
                    type = "360V3"
                ElseIf type1.Equals("I6") Then
                    type = "360V6"
                ElseIf type1.Equals("I2") Then
                    type = "360V2"
                End If
                Dim cmdqry As New OracleCommand()
                cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
                cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
                'Start by Manoj Kumar 30-05-2021 WI368  Not show criteria comleted in Yes/No. By default show 'No' value. (Bind variable error)
                cmdqry.Connection = conHrps
                cmdqry.Parameters.Clear()
                cmdqry.Parameters.AddWithValue("type", type)
                'cmdqry.Parameters.Add(New OracleParameter(":type", type))
                'cmdqry.Parameters.Add(New OracleParameter(":SS_SRLNO", dtsrl.Rows(0).Item(0)))
                Dim dt = getDataInDt(cmdqry)
                'End by Manoj Kumar 30-05-2021

                For d = 0 To dt.Rows.Count - 1

                    Dim qry As New OracleCommand()
                    qry.CommandText = "select * from hrps.t_survey_status where ss_asses_pno=:ss_asses_pno and ss_status='SE' and ss_del_tag='N' and upper(ss_categ) = "
                    qry.CommandText += " :IRC_CODE and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO"
                    qry.Connection = conHrps
                    qry.Parameters.Clear()
                    qry.Parameters.AddWithValue("ss_asses_pno", assespno)
                    qry.Parameters.AddWithValue("IRC_CODE", dt.Rows(d)("IRC_CODE").ToString.ToUpper)
                    qry.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
                    qry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    Dim s = getDataInDt(qry)
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
                Dim SubmitTag = CheckSubmitted(assespno)

                If tag = "AP" Then
                    appbtn.Text = "Approved"
                ElseIf tag = "RJ" Then
                    appbtn.Text = "Returned"
                Else
                    If SubmitTag = "SU" Then
                        appbtn.Text = "Submitted"
                    Else
                        appbtn.Text = "Not Submitted"
                    End If

                End If
            Next
        Catch ex As Exception
            ' MsgBox(ex.ToString)
        End Try
    End Sub
    Public Function CheckApproved(pno As String) As String
        Try
            Dim tg As String
            Dim cm As New OracleCommand()
            cm.CommandText = "select distinct ss_app_tag from hrps.t_survey_status  where ss_asses_pno=:ss_asses_pno and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO"
            cm.CommandText += " and ss_del_tag='N' and ss_status='SE'"
            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("ss_asses_pno", pno)
            cm.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            cm.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim dt = getDataInDt(cm)
            If dt.Rows.Count > 0 Then
                tg = dt.Rows(0)(0).ToString()
            Else
                tg = ""
            End If
            Return tg
        Catch ex As Exception

        End Try
    End Function
    Public Function CheckSubmitted(pno As String) As String
        Try
            Dim stg As String
            Dim cm As New OracleCommand()
            cm.CommandText = "select distinct ss_tag from hrps.t_survey_status  where ss_asses_pno=:ss_asses_pno and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO"
            cm.CommandText += " and ss_del_tag='N' and ss_status='SE'"
            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("ss_asses_pno", pno)
            cm.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            cm.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim dt = getDataInDt(cm)
            If dt.Rows.Count > 0 Then
                stg = dt.Rows(0)(0).ToString()
            Else
                stg = ""
            End If
            Return stg
        Catch ex As Exception

        End Try
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
    Protected Sub lbpendingapproval_Click(sender As Object, e As EventArgs)
        Try

            Dim gv = CType(sender, LinkButton).Parent.Parent
            Dim perno = CType(gv.FindControl("lblpno"), Label)
            Dim b = CType(gv.FindControl("lbpendingapproval"), LinkButton)
            Dim nm = CType(gv.FindControl("lnlname"), Label)
            Session("assespno") = Nothing
            Session("assespno") = perno.Text
            'lblassname.Text = nm.Text
            ''NoEachcateg(perno.Text)
            'BindAssesorGrid(perno.Text)
            'If b.Text = "Approved" Then
            '    GridView2.Visible = True
            '    gvfinal.Visible = False
            '    btnaddpeertsl.Visible = False
            '    btnaddnontsl.Visible = False
            '    btnsubmit.Visible = False
            'ElseIf b.Text = "Returned" Then
            '    GridView2.Visible = False
            '    btnaddpeertsl.Visible = True
            '    btnaddnontsl.Visible = True
            '    btnsubmit.Visible = True
            '    gvfinal.Visible = True
            'ElseIf b.Text = "Submitted" Then

            'Else
            '    GridView2.Visible = False
            '    btnaddpeertsl.Visible = True
            '    btnaddnontsl.Visible = True
            '    btnsubmit.Visible = True
            '    gvfinal.Visible = True
            'End If
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.Message)
        End Try

    End Sub
    Public Sub BindAssesorGrid(pno As String)
        Try
            'If Session("assespno") Is Nothing Then
            '    Exit Sub
            'End If

            Session("label") = ChkAuthlabel(pno)
            Dim type As String = String.Empty
            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            End If
            'Dim qry As New OracleCommand()
            Dim str As New OracleCommand
            If Session("label").Equals("I2") Then
                str.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            ElseIf Session("label").Equals("I3") Then
                str.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            Else
                str.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Peer/Subordinate/Internal Stakeholder',SS_CATEG) Category,"
            End If
            str.CommandText += "SS_CATEG,SS_DEL_TAG,SS_TAG from hrps.t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and SS_STATUS='SE' and ss_year=:fy and SS_SRLNO=:SS_SRLNO order by Category"
            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            str.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            'Dim qry = New OracleCommand(str, conHrps)
            Dim dt = getDataInDt(str)
            ViewState("Data") = dt
            Dim vl As String
            vl = ""
            Dim DrNOrg() As DataRow
            DrNOrg = dt.Select("SS_TAG='SU'") ''

            For Each row1 As DataRow In DrNOrg
                vl = "S"
            Next

            'If dt.Rows.Count > 0 Then
            '    If vl = "S" Then
            '        gvfinal.DataSource = dt
            '        gvfinal.DataBind()
            '        gvfinal.Columns(7).Visible = False
            '        btnaddnontsl.Visible = False
            '        btnaddpeertsl.Visible = False
            '        btnsubmit.Visible = False
            '        gvfinal.Visible = True
            '        '  lbOrg.Visible = True
            '        divtitle.Visible = True
            '    Else
            '        gvfinal.DataSource = dt
            '        gvfinal.DataBind()
            '        gvfinal.Columns(7).Visible = True
            '        btnaddnontsl.Visible = True
            '        btnaddpeertsl.Visible = True
            '        btnsubmit.Visible = True
            '        gvfinal.Visible = True
            '        '  lbOrg.Visible = True
            '        divtitle.Visible = True
            '    End If

            'Else
            '    gvfinal.DataSource = Nothing
            '    gvfinal.DataBind()
            '    btnaddnontsl.Visible = False
            '    btnaddpeertsl.Visible = False
            '    gvfinal.Visible = False
            '    btnsubmit.Visible = False
            '    ' lbOrg.Visible = False
            '    divtitle.Visible = False
            'End If



            Dim str1 As New OracleCommand
            If Session("label").Equals("I2") Then
                str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            ElseIf Session("label").Equals("I3") Then
                str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG) Category,"
            Else
                str1.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer/Subordinate','ROPT','Subordinates','INTSH','Peer/Subordinate/Internal Stakeholder',SS_CATEG) Category,"
            End If
            str1.CommandText += "SS_CATEG,SS_DEL_TAG from hrps.t_survey_status where ss_asses_pno=:SS_ASSES_PNO"
            'qry1.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and ss_del_tag='N' and ss_tag='SU' and SS_STATUS='SE' order by Category"
            str1.CommandText += " and ss_year=:fy and ss_del_tag='N' and SS_STATUS='SE' and SS_SRLNO=:SS_SRLNO order by Category"
            str1.Connection = conHrps
            str1.Parameters.Clear()
            str1.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            str1.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            str1.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            'Dim qry1 = New OracleCommand(str1, conHrps)
            Dim dth = getDataInDt(str1)

            'If dth.Rows.Count > 0 Then
            '    GridView2.DataSource = dth
            '    GridView2.DataBind()
            'Else
            '    GridView2.DataSource = Nothing
            '    GridView2.DataBind()
            'End If

            Dim cmdqry As New OracleCommand()
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc irc_desc1,decode(a.irc_type,'360V2',decode(a.IRC_CODE,'PEER','Peers','ROPT','Subordinates',b.irc_desc),'360V3',decode(a.IRC_CODE,'PEER','Peer/Subordinate',b.irc_desc),decode(a.IRC_CODE,'INTSH','Peer/Subordinate/Internal Stackholder',b.irc_desc)) irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            Dim dt1 = getDataInDt(cmdqry)
            Dim categ As String = String.Empty
            For d = 0 To dt1.Rows.Count - 1

                Dim qryq As New OracleCommand()
                qryq.CommandText = "select * from hrps.t_survey_status where ss_asses_pno=:SS_ASSES_PNO and ss_status='SE' and ss_del_tag='N' and upper(ss_categ) = "
                'qryq.CommandText += "upper( '" & dt1.Rows(d)("IRC_CODE") & "') and ss_year=:fy and SS_SRLNO=:SS_SRLNO"
                qryq.CommandText += ":IRC_CODE and ss_year=:fy and SS_SRLNO=:SS_SRLNO"
                qryq.Connection = conHrps
                qryq.Parameters.Clear()
                qryq.Parameters.AddWithValue("SS_ASSES_PNO", pno)
                qryq.Parameters.AddWithValue("fy", ViewState("FY").ToString())
                qryq.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                qryq.Parameters.AddWithValue("IRC_CODE", dt1.Rows(d)("IRC_CODE").ToString.ToUpper)
                Dim s = getDataInDt(qryq)
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
            '''''checking for minimum and maximum thresold value for each criteria WI:WI300'''''






        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
        End Try
    End Sub

    Public Sub Reset()

    End Sub

    Public Function Check(year As String, assespno As String, pno As String) As String
        Dim vl As String = String.Empty
        Try
            Dim str As New OracleCommand()
            str.CommandText = " select SS_PNO,irc_desc from hrps.t_survey_status,hrps.t_ir_codes where  SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO "
            str.CommandText += "and ((SS_PNO=:SS_PNO) OR (upper(ss_email)=:SS_PNO)) and SS_status='SE' and ss_del_tag ='N' and irc_type='360RL' and upper(ss_categ)=upper(irc_code) and SS_SRLNO=:SS_SRLNO"

            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            str.Parameters.AddWithValue("SS_ASSES_PNO", assespno.ToString())
            str.Parameters.AddWithValue("SS_PNO", pno.ToUpper())
            str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())

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

    Public Sub SessionTimeOut()
        If Session("ADM_USER") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Exit Sub
        Else



        End If
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

    Public Function getRefNo() As String
        Try
            Dim strRef As String = String.Empty
            Dim refno As String = String.Empty
            strRef = " select MAX(to_number(substr(SS_ID,3,10))) SS_ID from hrps.T_SURVEY_STATUS where SS_ID like 'SR%' and SS_YEAR='" & ViewState("FY").ToString() & "'"
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

            refno = refno.PadLeft(10, "0")
            refno = "SR" + refno

            Return refno
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function

    '  <System.Web.Script.Services.ScriptMethod(),
    'System.Web.Services.WebMethod()>
    'Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
    '    Dim ob As New RowData
    '    Try

    '        Dim cmd As New OracleCommand

    '        cmd.CommandType = Data.CommandType.Text

    '        cmd.CommandText = " select distinct ema_ename ||'('|| ema_perno ||')' EName from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and (ema_perno like '%" & prefixText & "%' or upper(ema_ename) like "
    '        ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
    '        cmd.CommandText += " '%" & prefixText.ToUpper & "%')"   'WI368 add officer class
    '        'End by Manoj Kumar

    '        ob.conHrps.Open()

    '        cmd.Connection = ob.conHrps
    '        Dim prefixes As List(Of String) = New List(Of String)
    '        Dim sdr As OracleDataReader = cmd.ExecuteReader
    '        While sdr.Read
    '            prefixes.Add(sdr("EName").ToString)
    '        End While



    '        Return prefixes
    '    Catch ex As Exception

    '        Return Nothing

    '    Finally

    '        ob.conHrps.Close()

    '    End Try

    'End Function
    Public Sub UpdateData(id As String, tag As String, catg As String)
        Try
            Dim query As String = String.Empty
            If tag = "Y" Then
                query = "delete from hrps.t_survey_status "  'Add by Manoj Kumar(osj1874) 18-01-2021
                query += "  where SS_PNO=:SS_PNO and ss_year=:fy"
                query += "  and SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG and nvl(SS_APP_TAG,'N') <> 'AP' and SS_SRLNO=:SS_SRLNO"
            End If
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_PNO", id)
            comnd.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString)
            comnd.Parameters.AddWithValue("SS_CATEG", catg)
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Function ChkValidationRangeCategory(ByVal cate As String) As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
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
            cmdqry.CommandText += "  where a.irc_type=:type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE=:IRC_CODE"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("type", type)
            cmdqry.Parameters.AddWithValue("IRC_CODE", cate.ToString)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from hrps.t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                        'lbls.Text = ""
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
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

    Public Function ChkValidationMiniRangeCategory() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
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
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
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
    Public Function ChkValidationmaxstake1() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
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
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
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
    Private Function GetApprover(ByVal SS_YEAR As String, ByVal SS_ASSES_PNO As String, ByVal SS_SRLNO As String) As String
        Dim res As String = ""
        Try
            Dim query As String
            query = "select distinct SS_APPROVER from T_SURVEY_STATUS where SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO And SS_SRLNO=:SS_SRLNO"
            Dim cmd As New OracleCommand(query, conHrps)
            Dim dt As New DataTable
            Dim adp As New OracleDataAdapter(cmd)
            adp.Fill(dt)
            If dt.Rows.Count > 0 Then
                res = dt.Rows(0)(0).ToString
            End If
        Catch ex As Exception

        End Try
        Return res
    End Function

    Private Sub gvself_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvself.PageIndexChanging
        gvself.PageIndex = e.NewPageIndex
        bindGrid()
    End Sub

    Private Sub lbtnExport_Click(sender As Object, e As EventArgs) Handles lbtnExport.Click
        ExportData()
        'Dim tmp_grdview As New GridView
        'Dim dt As DataTable = CType(Session("tmpdata"), DataTable)
        'tmp_grdview = gvself
        'tmp_grdview.AllowPaging = False
        'tmp_grdview.DataSource = dt
        'tmp_grdview.DataBind()
        'Dim fname As String = "Raw_Response_data_" & Now.ToShortDateString & Now.ToShortTimeString & ".xlsx"
        'Response.Clear()
        'Response.ClearContent()
        'Response.ClearHeaders()
        'Response.Buffer = True
        'Response.AddHeader("content-disposition", "attachment;filename=" & fname)
        'Response.Charset = ""
        ''Response.ContentType = "application/vnd.ms-excel"
        'Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        'Using sw As New StringWriter()
        '    Dim hw As New HtmlTextWriter(sw)

        '    If tmp_grdview.Visible = True Then
        '        'To Export all pages
        '        tmp_grdview.AllowPaging = False
        '        'Me.BindGrid()

        '        tmp_grdview.HeaderRow.BackColor = Color.LightGray
        '        For Each cell As TableCell In tmp_grdview.HeaderRow.Cells
        '            cell.BackColor = tmp_grdview.HeaderStyle.BackColor
        '        Next
        '        For Each row As GridViewRow In tmp_grdview.Rows
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
        '        tmp_grdview.RenderControl(hw)
        '    End If

        '    'style to format numbers to string
        '    Dim style As String = "<style> .textmode { } </style>"
        '    Response.Write(style)
        '    Response.Output.Write(sw.ToString())
        '    Response.Flush()
        '    Response.[End]()
        'End Using
        'gvself.AllowPaging = True

    End Sub
    Private Sub BindHead(ByVal grv As GridView)
        Try

            'grv.Columns.
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExportData()
        Dim msg As String = ""

        Dim tmp_grdview As New GridView
        Dim dt1 As DataTable = CType(Session("tmpdata"), DataTable)
        tmp_grdview = gvself
        tmp_grdview.AllowPaging = False
        tmp_grdview.DataSource = dt1
        tmp_grdview.DataBind()

        Try
            Dim fname As String = "Raw_Response_data_" & Now.ToShortDateString & Now.ToShortTimeString & ".xlsx"
            Dim dt As New DataTable
            For Each cell As TableCell In tmp_grdview.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In tmp_grdview.Rows
                Dim r = dt.NewRow
                For i As Integer = 0 To row.Cells.Count - 1
                    If row.Cells(i).Controls.Count > 0 Then
                        r(i) = (TryCast(row.Cells(i).Controls(1), Label)).Text
                    Else
                        r(i) = row.Cells(i).Text
                    End If
                Next
                dt.Rows.Add(r)
            Next
            dt.TableName = "RawData"
            Using wb As XLWorkbook = New XLWorkbook()

                Dim ws = wb.Worksheets.Add(dt)
                ' ws.Name = "Rawdata"
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=" & fname)
                Using memostrm As MemoryStream = New MemoryStream
                    wb.SaveAs(memostrm)
                    memostrm.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.End()
                End Using
            End Using
        Catch ex As Exception
            msg = ex.Message
        End Try
        gvself.AllowPaging = True

    End Sub
End Class


