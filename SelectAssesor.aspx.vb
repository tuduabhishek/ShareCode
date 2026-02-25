Imports System.Web.Services
Imports System.Data.OracleClient
Imports System.Data
Imports System.Diagnostics

Partial Class SelectAssesor
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Public Sub loadLoggedInUserIDAndDomainIntoSession()
        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        Dim vUserFullName As String = Page.User.Identity.Name   '155710'

        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If

        ' strUserID = "148497"
        strUserID = "119046"
        Session("USER_DOMAIN") = strUserDomain.ToUpper()
        Session("USER_ID") = strUserID.ToUpper()
        lblname.Text = GetPno(strUserID)



        Dim ststus = ChkAuth(strUserID)
        If ststus = False Then
            Response.Redirect("errorpage.aspx", True)
        End If

    End Sub
    Public Function ChkAuth(pno As String) As Boolean
        Try
            Dim chk As Boolean = False
            Dim qry As New OracleCommand()
            qry.CommandText = "select * from tips.t_empl_all  where ema_perno=:ema_perno and EMA_EQV_LEVEL='I2' and ema_disch_dt is null AND EMA_COMP_CODE='1000'"
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
            MsgBox("errorrr")
        End Try
    End Function
    Public Function GetPno(pernr As String) As String
        Dim perno As String = ""
        Try

            Dim cm As New OracleCommand()
            cm.CommandText = "  select EMA_ENAME from tips.t_empl_all  where EMA_EQV_LEVEL='I2' and ema_disch_dt is null and ema_perno=:ema_perno AND EMA_COMP_CODE='1000'"

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

    Public Function GetPno() As DataTable

        Try

            Dim cm As New OracleCommand()
            cm.CommandText = " select IRC_DESC,ema_ename from t_ir_codes,tips.t_empl_all where irc_type='360PN' and trim(irc_desc) = ema_perno"
            Dim d = getRecordInDt(cm, conHrps)
            Return d
        Catch ex As Exception

        End Try

    End Function

    Protected Sub GvCateg_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim id = CType(e.Row.FindControl("hdfnid"), HiddenField)
            Dim chk = CType(e.Row.FindControl("chkseldsel"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)

            Dim comnd As New OracleCommand()

            If perno.Text = "" Then
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_ID=:SS_ID and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            Else
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and ss_pno=:ss_pno  and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            End If
            comnd.Parameters.Clear()
            If perno.Text = "" Then
                comnd.Parameters.AddWithValue("SS_ID", id.Value)
            Else
                comnd.Parameters.AddWithValue("ss_pno", perno.Text)
            End If
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString)
            comnd.Connection = conHrps
            Dim da As New OracleDataAdapter(comnd)
            Dim d As New DataTable()
            da.Fill(d)
            ' Dim d = getRecordInDt(comnd, conHrps)

            If d.Rows.Count > 0 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
            'Dim chk1 = CType(e.Row.FindControl("chkall"), CheckBox)
            'Dim chk2 = CType(e.Row.FindControl("chkmgr"), CheckBox)
            'If chk1.Checked = True Then
            '    chk2.Checked = True
            'Else
            '    chk2.Checked = False
            'End If

        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try

    End Sub
    Public Sub GetSelected(dt As DataTable, categ As String)
        Try


            For o = 0 To dt.Rows.Count - 1
                Dim strself = New OracleCommand()
                strself.CommandText = "select * from t_survey_status  where SS_PNO ='" & dt.Rows(o)("ema_perno").ToString() & "' and SS_ASSES_PNO='" & Session("USER_ID") & "'  and SS_YEAR='" & ViewState("FY").ToString() & "' and SS_CATEG ='" & categ & "'"
                Dim dt1 = getRecordInDt(strself, conHrps)
                If dt1.Rows.Count > 0 Then
                Else
                    SaveData(categ, dt.Rows(o)("ema_perno").ToString(), dt.Rows(o)("ema_ename").ToString(), dt.Rows(o)("EMA_DESGN_DESC").ToString(), dt.Rows(o)("EMA_DEPT_DESC").ToString(), dt.Rows(o)("EMA_EMAIL_ID").ToString(), dt.Rows(o)("EMA_EMPL_PGRADE").ToString(), "ORG", "SE")
                End If
            Next

            If categ = "Self" Then
                Dim strself = New OracleCommand()
                strself.CommandText = "select * from t_survey_status  where  SS_ASSES_PNO='" & Session("USER_ID") & "' and SS_CATEG='Self'  and SS_YEAR='" & ViewState("FY").ToString() & "'"
                Dim dt1 = getRecordInDt(strself, conHrps)
                If dt1.Rows.Count > 0 Then
                Else
                    strself = New OracleCommand()
                    strself.CommandText = "select ema_perno,ema_ename,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_perno='" & Session("USER_ID").ToString() & "' and EMA_COMP_CODE='1000'"
                    Dim g = getRecordInDt(strself, conHrps)
                    SaveData(categ, g.Rows(0)("ema_perno").ToString(), g.Rows(0)("ema_ename").ToString(), g.Rows(0)("EMA_DESGN_DESC").ToString(), g.Rows(0)("EMA_DEPT_DESC").ToString(), g.Rows(0)("EMA_EMAIL_ID").ToString(), g.Rows(0)("EMA_EMPL_SGRADE").ToString(), "ORG", "SE")
                End If
            End If
        Catch ex As Exception
            '    MsgBox(ex.ToString)
        End Try
    End Sub



    Public Sub BindGrid()

        Try

            Dim r As New OracleCommand()
            r.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY from dual"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)(0).ToString()
            End If
            ''''''' Displaying Manager
            Dim strself As New OracleCommand()

            ' strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            ' strself.CommandText += " ema_perno='" & Session("USER_ID").ToString() & "' and ema_disch_dt is null union "
            strself.CommandText += " select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID  "
            strself.CommandText += " from tips.t_empl_all a,tips.t_empl_all b where b.ema_perno=a.EMA_REPORTING_TO_PNO and a.ema_perno='" & Session("USER_ID").ToString() & "' AND A.EMA_COMP_CODE='1000'"
            strself.CommandText += " union select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID  "
            strself.CommandText += " from tips.t_empl_all a,tips.t_empl_all b where b.ema_perno=a.EMA_PERS_EXEC_PNO and a.ema_perno='" & Session("USER_ID").ToString() & "'AND A.EMA_COMP_CODE='1000' "
            strself.CommandText += " union select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID  "
            strself.CommandText += " from tips.t_empl_all a,tips.t_empl_all b where b.ema_perno=a.EMA_DOTTED_PNO and a.ema_perno='" & Session("USER_ID").ToString() & "'AND A.EMA_COMP_CODE='1000' "

            Dim dtmgr = getRecordInDt(strself, conHrps)

            If dtmgr.Rows.Count > 0 Then
                GetSelected(dtmgr, "MANGR")
                GvManager.DataSource = dtmgr
                GvManager.DataBind()

            Else
                GvManager.DataSource = Nothing
                GvManager.DataBind()
            End If

            ''''''' Displaying Peer
            strself = New OracleCommand()
            strself.CommandText += " select  ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
            strself.CommandText += " =(select ema_reporting_to_pno from tips.t_empl_all where ema_perno='" & Session("USER_ID").ToString() & "' AND EMA_COMP_CODE='1000') and ema_disch_dt is null "
            strself.CommandText += " and EMA_EMPL_sGRADE in ('IL1','IL2','E3','R2') and ema_perno<>'" & Session("USER_ID").ToString() & "' AND EMA_COMP_CODE='1000'"
            strself.CommandText += " union select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL from t_survey_status where SS_CATEG ='PEER' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"


            Dim dtpeer = getRecordInDt(strself, conHrps)

            If dtpeer.Rows.Count < 3 And dtpeer.Rows.Count >= 0 Then
                Dim str As New OracleCommand
                str.CommandText += " select  ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
                str.CommandText += " =(select ema_reporting_to_pno from tips.t_empl_all where ema_perno='" & Session("USER_ID").ToString() & "' AND EMA_COMP_CODE='1000') and ema_disch_dt is null "
                str.CommandText += " and EMA_emp_class in ('1','2') and ema_perno<>'" & Session("USER_ID").ToString() & "' AND EMA_COMP_CODE='1000'"
                str.CommandText += " union select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL from t_survey_status where SS_CATEG ='PEER' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"
                Dim dtpeer1 = getRecordInDt(str, conHrps)
                GetSelected(dtpeer1, "PEER")
                GvPeer.DataSource = Nothing
                GvPeer.DataBind()
                GvPeer.DataSource = dtpeer1
                GvPeer.DataBind()

            Else
                GetSelected(dtpeer, "PEER")
                GvPeer.DataSource = Nothing
                GvPeer.DataBind()
                GvPeer.DataSource = dtpeer
                GvPeer.DataBind()

            End If
            ''''''' Displaying Reporties
            strself = New OracleCommand()
            'strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            'strself.CommandText += " ema_perno='" & Session("USER_ID").ToString() & "' and ema_disch_dt is null union all "
            strself.CommandText += " select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
            strself.CommandText += " = '" & Session("USER_ID").ToString() & "' and ema_disch_dt is null and EMA_EMPL_SGRADE  in ('IL2','IL3','E4','E5','R3S') AND EMA_COMP_CODE='1000' union "
            strself.CommandText += "select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL from t_survey_status where SS_CATEG ='ROPT' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"
            Dim dt = getRecordInDt(strself, conHrps)

            If dt.Rows.Count >= 3 Then
                GetSelected(dt, "ROPT")
                GvRepoties.DataSource = Nothing
                GvRepoties.DataBind()
                GvRepoties.DataSource = dt
                GvRepoties.DataBind()

            ElseIf dt.Rows.Count < 3 Then
                Dim strcond1 As New OracleCommand()
                strcond1.CommandText = " Select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
                strcond1.CommandText += " = '" & Session("USER_ID").ToString() & "' and ema_disch_dt is null and EMA_EMPL_SGRADE  in ('IL2','IL3','E4','E5','R3S','IL4','E6','E7','O1','R3','R4.10') AND EMA_COMP_CODE='1000' union "
                strcond1.CommandText += " Select ss_pno, ss_name, SS_LEVEL, SS_DESG, SS_DEPT, SS_EMAIL from t_survey_status where SS_CATEG ='ROPT' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"
                ' strcond1.CommandText += "union select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
                'strcond1.CommandText += " = '" & Session("USER_ID").ToString() & "' and ema_disch_dt is null and EMA_EMPL_SGRADE  in ('IL4','E6','E7','O1','R3','R4.10')"
                Dim dt1 = getRecordInDt(strcond1, conHrps)
                If dt1.Rows.Count < 3 Then
                    Dim strcond2 As New OracleCommand()
                    strcond2.CommandText = " select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
                    strcond2.CommandText += " = '" & Session("USER_ID").ToString() & "' and ema_disch_dt is null and EMA_EMPL_SGRADE  in ('IL2','IL3','E4','E5','R3S','IL4','E6','E7','O1','R3','R4.10','IL5', 'IL6','O2','O3','O4','O5','O6','R4.1N','R4.2') AND EMA_COMP_CODE='1000' union "
                    strcond2.CommandText += " Select ss_pno, ss_name, SS_LEVEL, SS_DESG, SS_DEPT, SS_EMAIL from t_survey_status where SS_CATEG ='ROPT' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"
                    'strcond2.CommandText += "union select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
                    'strcond2.CommandText += " = '" & Session("USER_ID").ToString() & "' and ema_disch_dt is null and EMA_EMPL_SGRADE  in ('IL4','E6','E7','O1','R3','R4.10')"
                    'strcond2.CommandText += " union select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
                    'strcond2.CommandText += " = '" & Session("USER_ID").ToString() & "' and ema_disch_dt is null and EMA_EMPL_SGRADE  in ('IL5', 'IL6','O2','O3','O4','O5','O6','R4.1N','R4.2')"
                    Dim dt2 = getRecordInDt(strcond2, conHrps)
                    If dt2.Rows.Count < 3 Then
                        Dim strcond3 As New OracleCommand()
                        strcond3.CommandText += " select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where ema_reporting_to_pno "
                        strcond3.CommandText += " = '" & Session("USER_ID").ToString() & "' and ema_disch_dt is null and EMA_emp_class  in ('1', '2') AND EMA_COMP_CODE='1000' union "
                        strcond3.CommandText += " Select ss_pno, ss_name, SS_LEVEL, SS_DESG, SS_DEPT, SS_EMAIL from t_survey_status where SS_CATEG ='ROPT' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"
                        Dim dt3 = getRecordInDt(strcond3, conHrps)
                        GetSelected(dt3, "ROPT")
                        GvRepoties.DataSource = Nothing
                        GvRepoties.DataBind()
                        GvRepoties.DataSource = dt3
                        GvRepoties.DataBind()

                    Else
                        GetSelected(dt2, "ROPT")
                        GvRepoties.DataSource = Nothing
                        GvRepoties.DataBind()
                        GvRepoties.DataSource = dt2
                        GvRepoties.DataBind()

                    End If

                Else
                    GetSelected(dt1, "ROPT")
                    GvRepoties.DataSource = Nothing
                    GvRepoties.DataBind()
                    GvRepoties.DataSource = dt1
                    GvRepoties.DataBind()


                End If
            End If

            ''''Displaying internal stake holder
            strself = New OracleCommand()
            'strself.CommandText = "select ema_perno, ema_ename, EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            'strself.CommandText += " ema_perno='" & Session("USER_ID").ToString() & "' and ema_disch_dt is null  union "
            strself.CommandText += "  select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL from t_survey_status where SS_CATEG ='INTSH' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"
            strself.CommandText += " and SS_YEAR='" & ViewState("FY").ToString() & "' "
            Dim dtstholder = getRecordInDt(strself, conHrps)
            'GetSelected(dt, "Self")
            If dtstholder.Rows.Count > 0 Then
                Gvintstholder.DataSource = dtstholder
                Gvintstholder.DataBind()

            Else
                Gvintstholder.DataSource = Nothing
                Gvintstholder.DataBind()
            End If


        Catch ex As Exception

        End Try

    End Sub

    Public Sub bindFinalGrid()


        Try
            GetSelf()

            Dim cmd As New OracleCommand()
            cmd.CommandText = "select SS_ID, SS_PNO ,SS_NAME,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,SS_DESG,SS_DEPT,SS_EMAIL, "
            cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peer','ROPT','Subordinates','INTSH','Internal Stakeholder',SS_CATEG)IRC_DESC "
            cmd.CommandText += " from tips.t_empl_all , t_survey_status  where  SS_PNO=ema_perno(+) and SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' and SS_STATUS ='SE'"
            cmd.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and SS_DEL_TAG='N' order by IRC_DESC"
            Dim f = getRecordInDt(cmd, conHrps)
            If f.Rows.Count > 0 Then
                gvfinal.DataSource = f
                gvfinal.DataBind()
            Else
                gvfinal.DataSource = Nothing
                gvfinal.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub SelectAssesor_Init(sender As Object, e As EventArgs) Handles Me.Init

        loadLoggedInUserIDAndDomainIntoSession()
    End Sub

    Private Sub SelectAssesor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim isvalidpage = PageValid()
            If isvalidpage = True Then
            Else
                Response.Write("<center> <b><I> This website has been closed </b></I></center>")
                Me.Page.Visible = False
                Exit Sub
            End If
            If Not IsPostBack Then
                BindGrid()
                bindFinalGrid()
                NoEachcateg()
                Dim g = ChkApprove(Session("USER_ID").ToString)

                If g.Rows(0)(0).ToString() = "RJ" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Your form has been returned with remarks: " & g.Rows(0)(1).ToString())
                    lbOrg.Visible = True
                    GvManager.Visible = True
                    btnaddtslsub.Visible = True
                    btnnontslsub.Visible = True
                    div2.Visible = True
                    Div3.Visible = True
                    GvRepoties.Visible = True
                    btntatasteel.Visible = True
                    btnnontslp.Visible = True
                    rowpeer.Visible = True
                    div1.Visible = True
                    GvPeer.Visible = True
                    btnaddpeertsl.Visible = True
                    btnaddnontsl.Visible = True
                    divtsl.Visible = True
                    divntsl.Visible = True
                    Gvintstholder.Visible = True
                ElseIf g.Rows(0)(2).ToString() = "SU" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "You have already submitted the form...!")
                    lbOrg.Visible = False
                    GvManager.Visible = False
                    btnaddtslsub.Visible = False
                    btnnontslsub.Visible = False
                    div2.Visible = False
                    Div3.Visible = False
                    GvRepoties.Visible = False
                    btntatasteel.Visible = False
                    btnnontslp.Visible = False
                    rowpeer.Visible = False
                    div1.Visible = False
                    GvPeer.Visible = False
                    btnaddpeertsl.Visible = False
                    btnaddnontsl.Visible = False
                    divtsl.Visible = False
                    divntsl.Visible = False
                    Gvintstholder.Visible = False

                ElseIf g.Rows(0)(2).ToString() = "SU" And g.Rows(0)(2).ToString() = "AP" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "You form has been approved...!")
                    lbOrg.Visible = False
                    GvManager.Visible = False
                    btnaddtslsub.Visible = False
                    btnnontslsub.Visible = False
                    div2.Visible = False
                    Div3.Visible = False
                    GvRepoties.Visible = False
                    btntatasteel.Visible = False
                    btnnontslp.Visible = False
                    rowpeer.Visible = False
                    div1.Visible = False
                    GvPeer.Visible = False
                    btnaddpeertsl.Visible = False
                    btnaddnontsl.Visible = False
                    divtsl.Visible = False
                    divntsl.Visible = False
                    Gvintstholder.Visible = False
                End If
                ' Populate Grid
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



    Protected Sub lbOrg_Click(sender As Object, e As EventArgs)
        Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openModel", "showmodalAddSabashAwardee();", True)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lbNonOrg_Click(sender As Object, e As EventArgs)
        Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openMode1", "showmodalAddSabashAwardee1();", True)
        Catch ex As Exception

        End Try
    End Sub



    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SelectAssesor
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select ema_ename ||'('|| ema_perno ||')' EName from tips.t_empl_all where (ema_perno like  :ema_perno or upper(ema_ename) like "
            cmd.CommandText += " :ema_ename)  and ema_disch_dt is null AND EMA_COMP_CODE='1000'"


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

    Protected Sub txtCouponNo_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoP.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            strself.CommandText += " (ema_ename ||ema_perno )=:pno and ema_disch_dt is null AND EMA_COMP_CODE='1000'"
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtdesgP.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtemailP.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtorgP.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtemailP.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtorgP.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgP.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtpnoP.ToolTip = txtpnoP.Text
                lblpeerlevel.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

                If txtemailP.Text <> "" Then
                    txtemailP.ReadOnly = True
                Else
                    txtemailP.ReadOnly = True
                End If
                txtdesgP.ReadOnly = True
                txtorgP.ReadOnly = True
            Else

                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                reset()
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub UpdateStatus(id As String, tag As String)
        Try
            Dim query As String = String.Empty
            Dim pno = Session("USER_ID").ToString()

            If id.StartsWith("SR") Then
                query = "update t_survey_status set SS_STATUS =:SS_STATUS,SS_UPDATED_BY=:SS_UPDATED_BY,SS_UPDATED_DT=sysdate where SS_ID=:SS_ID and SS_CRT_BY =:SS_CRT_BY"
                query += "  and SS_YEAR=:SS_YEAR"
            Else
                query = "update t_survey_status set SS_STATUS =:SS_STATUS ,SS_UPDATED_BY=:SS_UPDATED_BY,SS_UPDATED_DT=sysdate where SS_PNO=:SS_PNO and SS_CRT_BY "
                query += " =:SS_CRT_BY and SS_YEAR=:SS_YEAR"
            End If


            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            If id.StartsWith("SR") Then
                comnd.Parameters.AddWithValue("SS_STATUS", tag)
                comnd.Parameters.AddWithValue("SS_ID", id)
                comnd.Parameters.AddWithValue("SS_CRT_BY", pno)
                comnd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
                comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            Else
                comnd.Parameters.AddWithValue("SS_STATUS", tag)
                comnd.Parameters.AddWithValue("SS_PNO", id)
                comnd.Parameters.AddWithValue("SS_CRT_BY", pno)
                comnd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
                comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())

            End If

            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

    Protected Sub lbview_Click(sender As Object, e As EventArgs)
        Try
            'Response.Redirect("frm1.aspx", True)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub GetSelf()
        Try
            Dim c As New OracleCommand()
            c.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            c.CommandText += "ema_perno ='" & Session("USER_ID").ToString() & "' and ema_disch_dt is null AND EMA_COMP_CODE='1000'"
            Dim d = getRecordInDt(c, conHrps)
            If d.Rows.Count > 0 Then
                c = New OracleCommand()
                c.CommandText = " select * from t_survey_status where ss_pno='" & Session("USER_ID").ToString() & "'  and SS_YEAR='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO='" & Session("USER_ID").ToString() & "'"
                Dim f = getRecordInDt(c, conHrps)
                If f.Rows.Count > 0 Then
                Else
                    SaveData("Self", d.Rows(0)("ema_perno").ToString(), d.Rows(0)("ema_ename").ToString(), d.Rows(0)("EMA_DESGN_DESC").ToString(), d.Rows(0)("EMA_DEPT_DESC").ToString(), d.Rows(0)("EMA_EMAIL_ID").ToString(), d.Rows(0)("EMA_EMPL_PGRADE").ToString(), "ORG", "SE")
                End If
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
            str.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
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


    Public Sub SaveData(ByVal role As String, ByVal pno As String, ByVal name As String, ByVal desg As String, ByVal org As String, ByVal email As String, ByVal lvl As String, ByVal orgtype As String, ByVal status As String)

        Try

            Dim OrgStr As String = String.Empty
            Dim id = getRefNo()

            If pno = "" Then
                pno = id
            End If

            OrgStr = "insert into T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL) values (:SS_CATEG,:SS_ID,:SS_PNO,:SS_NAME,:SS_DESG,:SS_DEPT,:SS_EMAIL,:SS_STATUS,"
            OrgStr += " :SS_TAG,:SS_CRT_BY,sysdate,:SS_DEL_TAG,:SS_TYPE,:ss_year,:SS_ASSES_PNO,:SS_LEVEL)"

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
            comnd.Parameters.AddWithValue("SS_TAG", "N")
            comnd.Parameters.AddWithValue("SS_CRT_BY", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("SS_DEL_TAG", "N")
            comnd.Parameters.AddWithValue("SS_TYPE", orgtype)
            comnd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("SS_LEVEL", lvl)
            comnd.ExecuteNonQuery()
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
    Public Function SimpleCrypt(ByVal Text As String) As String

        Dim strTempChar As String = "", i As Integer
        For i = 1 To Text.Length
            If Asc(Mid$(Text, i, 1)) < 128 Then
                strTempChar = CType(Asc(Mid$(Text, i, 1)) + 128, String)
            ElseIf Asc(Mid$(Text, i, 1)) > 128 Then
                strTempChar = CType(Asc(Mid$(Text, i, 1)) - 128, String)
            End If
            Mid$(Text, i, 1) = Chr(CType(strTempChar, Integer))
        Next i
        Return Text

    End Function

    Protected Sub btntatasteel_Click(sender As Object, e As EventArgs)
        rowpeer.Visible = True
        div1.Visible = False
    End Sub
    Protected Sub btnaddpeertatasteel_Click(sender As Object, e As EventArgs)
        divtsl.Visible = True
        divntsl.Visible = False
    End Sub
    Protected Sub btnAddP_Click(sender As Object, e As EventArgs)
        Try
            If txtpnoP.Text.Trim() <> "" Then
                Dim perno = txtpnoP.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgP.Text.Trim(), "'", "''")
                Dim dept = Replace(txtorgP.Text.Trim, "'", "''")
                Dim email = Replace(txtemailP.Text.Trim, "'", "''")

                Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), pno)

                If val = "" Then
                    SaveData("PEER", pno, name, desg, dept, email, lblpeerlevel.Text, "ORG", "SE")
                    reset()
                    BindGrid()
                    bindFinalGrid()
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                    Exit Sub
                End If

            Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank, Please fill...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnaddnontsl_Click(sender As Object, e As EventArgs)
        divtsl.Visible = False
        divntsl.Visible = True
    End Sub
    Protected Sub txtpnoI_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoI.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            strself.CommandText += " (ema_ename||ema_perno)=:pno and ema_disch_dt is null AND EMA_COMP_CODE='1000'"
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            ' strself.Parameters.Add(New OracleParameter("pno", OracleType.VarChar)).Value = pno.ToString()
            'Dim dt = getRecordInDt(strself, conHrps)
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
                txtpnoI.ToolTip = txtpnoI.Text
                lblinst1.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

                If txtemailI.Text <> "" Then
                    txtemailI.ReadOnly = True
                Else
                    txtemailI.ReadOnly = False
                End If
                txtdesgI.ReadOnly = True
                txtdeptI.ReadOnly = True
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                Exit Sub
                reset()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnorgadd_Click(sender As Object, e As EventArgs)
        Try
            If txtpnoI.Text.Trim() <> "" Then
                Dim perno = txtpnoI.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgI.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptI.Text.Trim, "'", "''")
                Dim email = Replace(txtemailI.Text.Trim, "'", "''")

                Dim Val = Check(ViewState("FY"), Session("User_id").ToString, pno)

                If Val = "" Then
                    SaveData("INTSH", pno, name, desg, dept, email, lblinst1.Text, "ORG", "SE")
                    reset()
                    BindGrid()
                    bindFinalGrid()
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & Val & " Category...!")
                    Exit Sub
                End If


            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank, Please fill...!")
                reset()
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnaddnorgI_Click(sender As Object, e As EventArgs)
        Try
            If txtnamenI.Text.Trim() = "" Or txtdeptnI.Text.Trim() = "" Or txtdesgnI.Text = "" Or txtemailnI.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If

            If ChkMail(txtemailnI.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If

            Dim pno = ""
            Dim name = Replace(txtnamenI.Text.Trim.Split("(")(0), "'", "''")
            Dim desg = Replace(txtdesgnI.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptnI.Text.Trim, "'", "''")
            Dim email = Replace(txtemailnI.Text.Trim, "'", "''")

            Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then
                SaveData("INTSH", pno, name, desg, dept, email, "", "NORG", "SE")
                reset()
                BindGrid()
                bindFinalGrid()
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub reset()
        Try
            txtpnosub.Text = ""
            txtpnoP.Text = ""
            txtdesgP.Text = ""
            txtorgP.Text = ""
            txtemailP.Text = ""
            txtnamenI.Text = ""
            txtdesgnI.Text = ""
            txtdeptnI.Text = ""
            txtemailnI.Text = ""
            txtpnoI.Text = ""
            txtdesgI.Text = ""
            txtdeptI.Text = ""
            txtemailI.Text = ""
            txtdesgsub.Text = ""
            txtmailsub.Text = ""
            txtdeptsub.Text = ""
        Catch ex As Exception

        End Try
    End Sub
    Public Sub UpdateData(id As String, tag As String, deltag As String, catg As String)
        Try
            Dim query As String = String.Empty
            'query = "update t_survey_status set SS_STATUS ='" & tag & "' where SS_pno='" & id & "'  and SS_YEAR='" & ViewState("FY").ToString() & "'"
            query = "update t_survey_status set SS_STATUS =:SS_STATUS,SS_DEL_TAG=:SS_DEL_TAG, SS_APP_DT=sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_pno=:SS_pno and SS_YEAR=:SS_YEAR "
            query += "AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_STATUS", tag)
            comnd.Parameters.AddWithValue("SS_CATEG", catg)
            comnd.Parameters.AddWithValue("SS_pno", id)
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_DEL_TAG", deltag)
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", Session("USER_ID").ToString())
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub chkseldsel_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chkseldsel"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim email = CType(gv.FindControl("lblemail"), Label)

            Dim val = Check(ViewState("FY").ToString(), Session("User_id").ToString, email.Text)

            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "INTSH")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If

                'ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()

            Else
                UpdateData(id.Text, "DS", "Y", "INTSH")
                'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
                bindFinalGrid()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function ChkApprove(pno As String) As DataTable

        Try
            Dim pno1 = Session("USER_ID").ToString()
            Dim qry As New OracleCommand()
            'qry.CommandText = " select SS_APP_TAG,SS_REMARKS from t_survey_status where SS_ASSES_PNO ='" & pno & "'  and SS_YEAR='" & ViewState("FY").ToString() & "' and SS_TAG ='SU' "
            qry.CommandText = " select distinct SS_APP_TAG,SS_REMARKS,ss_tag from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and SS_YEAR=:SS_YEAR  "
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            qry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            qry.Connection = conHrps
            Dim da As New OracleDataAdapter(qry)
            Dim f As New DataTable()
            da.Fill(f)

            Return f
        Catch ex As Exception

        End Try

    End Function

    Public Sub NoEachcateg()
        Try
            Dim qr As New OracleCommand()
            qr.CommandText = "select a.IRC_CODE,'Min '||SUBSTR(a.IRC_DESC,0,1) minmum, 'Max '||SUBSTR(a.IRC_DESC,3,2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            qr.CommandText += "  where a.irc_type='360VL' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"

            Dim w = getRecordInDt(qr, conHrps)

            If w.Rows.Count > 0 Then
                lblinst.Text = "(" & w.Rows(0)("minmum").ToString() & " " & w.Rows(0)("maximum").ToString() & ")"
                lblmmms.Text = "(" & w.Rows(1)("minmum").ToString() & " " & w.Rows(1)("maximum").ToString() & ")"
                lblpeer.Text = "(" & w.Rows(2)("minmum").ToString() & " " & w.Rows(2)("maximum").ToString() & ")"
                lblsub.Text = "(" & w.Rows(3)("minmum").ToString() & " " & w.Rows(3)("maximum").ToString() & ")"
            End If
        Catch ex As Exception

        End Try
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
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)

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
            MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function

    Public Sub UpdateApprover(approver As String, pno As String)
        Try
            Dim query As String = String.Empty
            'query = "update t_survey_status set SS_APPROVER ='" & approver & "',SS_WFL_STATUS='1' where SS_ASSES_PNO='" & pno & "'  and SS_YEAR='" & ViewState("FY").ToString() & "'"
            query = "update t_survey_status set SS_APPROVER =:SS_APPROVER,SS_WFL_STATUS='1', SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO=:SS_ASSES_PNO and SS_YEAR=:SS_YEAR "

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_APPROVER", approver)
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Connection = conHrps
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub chkseldsel_CheckedChanged1(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chkseldsel"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim pno = CType(gv.FindControl("lblpno"), Label)
            Dim email = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If pno.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, email.Text)
            Else
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, pno.Text)
            End If

            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "PEER")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If
                ' ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()
            Else
                UpdateData(id.Text, "DS", "Y", "PEER")
                'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
                bindFinalGrid()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lbOrg_Click1(sender As Object, e As EventArgs) Handles lbOrg.Click
        Try
            Dim stat = ChkValidation()
            If Len(stat) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & stat & " Category")
                Exit Sub
            End If
            Dim comd As New OracleCommand()
            Dim pno = Session("USER_ID").ToString()
            comd.CommandText = " select EMA_PERS_EXEC_PNO from tips.t_empl_all where ema_perno='" & pno & "'"
            Dim dt = getRecordInDt(comd, conHrps)
            If dt.Rows.Count > 0 Then
                UpdateApprover(dt.Rows(0)("EMA_PERS_EXEC_PNO").ToString, pno)
            End If

            Dim qry As String = String.Empty
            'qry = " update t_survey_status set SS_TAG ='SU' , SS_TAG_DT = sysdate where SS_STATUS ='SE' and SS_ASSES_PNO ='" & pno & "' and ss_year='" & ViewState("FY").ToString() & "'"
            qry = " update t_survey_status set SS_TAG ='SU',ss_app_tag='' , SS_TAG_DT = sysdate , ss_app_dt=sysdate,SS_UPDATED_BY=:SS_UPDATED_BY, SS_UPDATED_DT=sysdate "
            qry += " where SS_STATUS ='SE' and SS_ASSES_PNO =:SS_ASSES_PNO and ss_year=:ss_year AND SS_DEL_TAG='N'"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim cmd As New OracleCommand(qry, conHrps)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            cmd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
            cmd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            cmd.ExecuteNonQuery()
            ShowGenericMessageModal(CommonConstants.AlertType.success, " Your response has been submitted. Thank You!")
            lbOrg.Visible = False
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnnontslp_Click(sender As Object, e As EventArgs)
        rowpeer.Visible = False
        div1.Visible = True
    End Sub
    Protected Sub btnaddpeer_Click(sender As Object, e As EventArgs)
        Try
            If txtnmpeer.Text.Trim() = "" Or txtdesgpeer.Text.Trim() = "" Or txtmailpeer.Text = "" Or txtdeptpeer.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If
            If ChkMail(txtmailpeer.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If

            Dim pno = ""
            Dim name = Replace(txtnmpeer.Text.Trim(), "'", "''")
            Dim desg = Replace(txtdesgpeer.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptpeer.Text.Trim, "'", "''")
            Dim email = Replace(txtmailpeer.Text.Trim, "'", "''")

            Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then
                SaveData("PEER", pno, name, desg, dept, email, "", "NORG", "SE")
                reset()
                BindGrid()
                bindFinalGrid()
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnnontslsub_Click(sender As Object, e As EventArgs)
        div2.Visible = True
        Div3.Visible = False
    End Sub
    Protected Sub btnaddmgr_Click(sender As Object, e As EventArgs)
        Try
            If txtnamemgr.Text.Trim() = "" Or txtdesgmgr.Text.Trim() = "" Or txtemailmgr.Text = "" Or txtdeptmgr.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If

            If ChkMail(txtemailmgr.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If

            Dim pno = ""
            Dim name = Replace(txtnamemgr.Text.Trim(), "'", "''")
            Dim desg = Replace(txtdesgmgr.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptmgr.Text.Trim, "'", "''")
            Dim email = Replace(txtemailmgr.Text.Trim, "'", "''")


            Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then
                SaveData("ROPT", pno, name, desg, dept, email, "", "NORG", "SE")
                reset()
                BindGrid()
                bindFinalGrid()
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
    Protected Sub btnaddtslsub_Click(sender As Object, e As EventArgs)
        div2.Visible = False
        Div3.Visible = True
    End Sub
    Protected Sub txtaddsub_Click(sender As Object, e As EventArgs)
        Try
            If txtpnosub.Text <> "" Then
                Dim perno = txtpnosub.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgsub.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptsub.Text.Trim, "'", "''")
                Dim email = Replace(txtmailsub.Text.Trim, "'", "''")

                Dim Val = Check(ViewState("FY"), Session("User_id").ToString, pno)

                If Val = "" Then
                    SaveData("ROPT", pno, name, desg, dept, email, lblsublvl.Text, "ORG", "SE")
                    reset()
                    BindGrid()
                    bindFinalGrid()
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & Val & " Category...!")
                    Exit Sub
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter name or pno..!")
                Exit Sub

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub txtpnosub_TextChanged(sender As Object, e As EventArgs)

        Try
            Dim pno = txtpnosub.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from tips.t_empl_all where "
            strself.CommandText += " (ema_ename ||ema_perno )=:pno and ema_disch_dt is null AND EMA_COMP_CODE='1000'"
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtdesgsub.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtmailsub.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptsub.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtmailsub.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptsub.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgsub.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtpnosub.ToolTip = txtpnosub.Text
                lblsublvl.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()
                txtdesgsub.ReadOnly = True
                If txtmailsub.Text.Trim <> "" Then
                    txtmailsub.ReadOnly = True
                Else
                    txtmailsub.ReadOnly = False
                End If

                txtdeptsub.ReadOnly = True
                Else

                    ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                reset()
                Exit Sub
            End If

        Catch ex As Exception

        End Try

    End Sub
    Protected Sub chksub_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chksub"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim pno = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If id.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString, Session("User_id").ToString, pno.Text)
            Else
                val = Check(ViewState("FY").ToString, Session("User_id").ToString, id.Text)
            End If


            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "ROPT")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If

                'ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()

            Else
                UpdateData(id.Text, "DS", "Y", "ROPT")
                'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
                bindFinalGrid()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GvRepoties_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Dim id = CType(e.Row.FindControl("hdfnid"), HiddenField)
            Dim chk = CType(e.Row.FindControl("chksub"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)

            Dim comnd As New OracleCommand()

            If perno.Text = "" Then
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'ROPT' and SS_ID=:SS_ID and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            Else
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'ROPT' and ss_pno=:ss_pno  and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            End If
            comnd.Parameters.Clear()
            If perno.Text = "" Then
                comnd.Parameters.AddWithValue("SS_ID", perno.Text)
            Else
                comnd.Parameters.AddWithValue("ss_pno", perno.Text)
            End If
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            comnd.Connection = conHrps
            Dim da As New OracleDataAdapter(comnd)
            Dim d As New DataTable()
            da.Fill(d)
            ' Dim d = getRecordInDt(comnd, conHrps)

            If d.Rows.Count > 0 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GvPeer_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim chk = CType(e.Row.FindControl("chkseldsel"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)

            Dim comnd As New OracleCommand()

            If perno.Text = "" Then
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'PEER' and SS_ID=:SS_ID and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            Else
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'PEER' and ss_pno=:ss_pno  and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            End If
            comnd.Parameters.Clear()
            If perno.Text = "" Then
                comnd.Parameters.AddWithValue("SS_ID", perno.Text)
            Else
                comnd.Parameters.AddWithValue("ss_pno", perno.Text)
            End If
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            'comnd.Parameters.AddWithValue("SS_CATEG", "PEER")
            comnd.Connection = conHrps
            Dim da As New OracleDataAdapter(comnd)
            Dim d As New DataTable()
            da.Fill(d)
            ' Dim d = getRecordInDt(comnd, conHrps)

            If d.Rows.Count > 0 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Gvintstholder_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim chk = CType(e.Row.FindControl("chkseldsel"), CheckBox)
            Dim perno = CType(e.Row.FindControl("lblpno"), Label)

            Dim comnd As New OracleCommand()

            If perno.Text = "" Then
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'INTSH' and SS_ID=:SS_ID and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            Else
                comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'INTSH' and ss_pno=:ss_pno  and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO"
            End If
            comnd.Parameters.Clear()
            If perno.Text = "" Then
                comnd.Parameters.AddWithValue("SS_ID", perno.Text)
            Else
                comnd.Parameters.AddWithValue("ss_pno", perno.Text)
            End If
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            comnd.Connection = conHrps
            Dim da As New OracleDataAdapter(comnd)
            Dim d As New DataTable()
            da.Fill(d)
            If d.Rows.Count > 0 Then
                chk.Checked = True
            Else
                chk.Checked = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function PageValid() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            ocmd.CommandText = "select IRC_CODE from t_ir_codes where irc_type ='360PG' and to_char(IRC_START_DT,'dd/mm/yyyy') <= to_char(sysdate,'dd/mm/yyyy')"
            ocmd.CommandText += "  and to_char(IRC_END_DT,'dd/mm/yyyy') >= to_char(sysdate,'dd/mm/yyyy') and IRC_VALID_TAG='A' and upper(irc_desc)='SELECTASSESOR.ASPX'"
            Dim vc = getRecordInDt(ocmd, conHrps)
            If vc.Rows.Count > 0 Then
                isvalid = True
            Else
                isvalid = False
            End If
        Catch ex As Exception

        End Try
        Return isvalid
    End Function
End Class
