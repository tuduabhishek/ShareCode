Imports System.Data.OracleClient
Imports System.Data
Imports System.Net.Mail
''' <summary>
''' This object is for approval page to provide approval : WI:WI3000
''' WI: WI368  (14 LINE COMMENTED, 7 line code added)   Added by Manoj Kumar on 30-05-2021
''' **********************
''' Not delete reocrd when unchecked the gridview row. 
''' Method UpdateData not work because query no space left between and conditions
''' Add officer category
''' ***********************
''' WI: WI447 
''' ***************
''' Created By: Avik Mukherjee
''' Date: 04-06-2021
''' Add maximum and minimum thresold value during approval process for each respondent criteria
''' </summary>
Partial Class SurveyApproval_OPR
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub SurveyApproval_OPR_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub
    Public Sub loadLoggedInUserIDAndDomainIntoSession()


        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        Dim vUserFullName As String = Page.User.Identity.Name   '155710'

        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If

        Session("USER_DOMAIN") = strUserDomain.ToUpper()
        strUserID = Session("USER_ID").ToString
        'If strUserID = "197838" Then
        '    strUserID = "120324"
        'ElseIf strUserID = "162523" Then
        '    strUserID = "197717"
        'ElseIf strUserID = "148536" Then
        '    strUserID = "119336"
        'ElseIf strUserID = "153815" Then
        '    strUserID = "163691"
        'End If

        'strUserID = "465021"

        'If strUserID = "199878" Then
        '    strUserID = "163691"
        'ElseIf strUserID = "198777" Then
        '    strUserID = "148497"
        'ElseIf strUserID = "199864" Then
        '    strUserID = "120324"
        'End If

        getFy()
        getsrlno()
        Session("USER_ID") = strUserID.ToUpper()
        'If Session("USER_ID").ToString().ToUpper <> "197838" Then
        '    Response.Redirect("errorpage.aspx", True)
        'End If
        ' Dim ststus = ChkAuth(strUserID)
        lblname.Text = GetPno(strUserID)
        'If ststus = False Then
        '    Response.Redirect("errorpage.aspx", True)
        'End If


    End Sub
    Private Sub SurveyApproval_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            txtremarks.Visible = False
            btnrej.Visible = False
            If Not IsPostBack Then
                Dim isvalidpage = PageValid()
                If isvalidpage = True Then
                Else
                    Response.Write("<center> <b><I> No pending request. </b></I></center>")
                    Me.Page.Visible = False
                    Exit Sub
                End If
                bindGrid()
                'PopDroupdown()
                GetApprovalLastDate()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function SessionTimeOut() As Boolean
        If Session("USER_ID") Is Nothing Or Session("assespno") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub getFy()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("IRC_DESC").ToString()
                Session("FYforNamePopup") = g.Rows(0)("IRC_DESC").ToString() 'Added by TCS on 171222, Session value store for web method uses
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
                Session("CycleforNamePopup") = dtsrl.Rows(0)("IRC_DESC").ToString() 'Added by TCS on 171222, Session value store for web method uses
            End If
        Catch ex As Exception


        End Try
    End Sub
    Public Sub NoEachcateg(ByVal sp As String)
        Try
            Dim qr As New OracleCommand()
            Dim lvl As String = ChkAuthlabel(sp)
            Dim code As String = String.Empty
            'qr.CommandText = "select a.IRC_CODE, SUBSTR(a.IRC_DESC,0,1) minmum, SUBSTR(a.IRC_DESC,3,2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            'qr.CommandText += "  where a.irc_type in('360V3','360V4','360V5','360V6') and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"
            If lvl = "I5" Then
                code = "360V5"
            ElseIf lvl = "I4" Then
                code = "360V4"
            ElseIf lvl = "I6" Then
                code = "360V6"
            ElseIf lvl = "I3" Then
                code = "360V3"
            ElseIf lvl = "I2" Then
                code = "360V2"
            End If
            qr.CommandText = "Select a.IRC_CODE, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1)  minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2)  maximum,decode(b.irc_desc,'Peer','Peers and Subordinate',b.irc_desc) irc_desc1,decode(a.irc_type,'360V2',decode(a.IRC_CODE,'PEER','Peers','ROPT','Subordinates',b.irc_desc),'360V3',decode(a.IRC_CODE,'PEER','Peer/Subordinate',b.irc_desc),decode(a.IRC_CODE,'INTSH','Peer/Subordinate/Internal Stackholder',b.irc_desc)) irc_desc,decode(a.irc_type,'360V3','IL3','360V4','IL4','360V5','IL5','360V6','IL6','360V2','IL2') Label from t_ir_codes a,t_ir_codes b"
            'If lvl = "I2" Then
            '    qr.CommandText = "Select a.IRC_CODE, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1)  minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2)  maximum,decode(b.irc_desc,'Peer','Peers',b.irc_desc) irc_desc,decode(a.irc_type,'360V3','IL3','360V4','IL4','360V5','IL5','360V6','IL6','360V2','IL2') Label from t_ir_codes a,t_ir_codes b"
            'End If
            ' Start WI368  by Manoj Kumar on 30-05-2021 call another method to load data in datatable
            qr.CommandText += " where a.irc_type =:irc_type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 5,1"
            qr.Connection = conHrps
            qr.Parameters.Clear()
            qr.Parameters.AddWithValue("irc_type", code)
            Dim w = getDataInDt(qr)
            ' End by Manoj kumar on 30-05-2021
            If w.Rows.Count > 0 Then
                GridView2.Visible = True
                GridView2.DataSource = w
                GridView2.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)

        End Try
    End Sub
    ' Start WI368  by Manoj Kumar on 30-05-2021 add method to load data in datatable
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
    'End by Manoj Kumar on 30-05-2021
    Public Sub bindGrid()
        Try

            Dim qry As New OracleCommand()
            Dim pno = Session("USER_ID").ToString()
            qry.CommandText = "select distinct SS_ASSES_PNO,ema_ename,ema_desgn_desc "
            qry.CommandText += " from t_survey_status,hrps.t_emp_master_feedback360 where SS_APPROVER =:SS_APPROVER "


            'qry.CommandText += "and ema_perno = SS_ASSES_PNO  and SS_ASSES_PNO <> :SS_ASSES_PNO and ss_year='" & ViewState("FY").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "' and decode(ss_wfl_status,'1',trunc(ema_step2_enddt),'2',trunc(sysdate)) >= trunc(sysdate)"

            qry.CommandText += "and ema_perno = SS_ASSES_PNO  and SS_ASSES_PNO <> :SS_ASSES_PNO and decode(ss_wfl_status,'1',trunc(ema_step2_enddt),'2',trunc(sysdate)) >= trunc(sysdate) And trunc(ema_step2_stdt)<=trunc(sysdate) And trunc(ema_step2_enddt)>=trunc(sysdate)"

            'Added by TCS on 13122022, Added Year & Cycle in where clause
            qry.CommandText += " and SS_YEAR = :SS_YEAR and SS_SRLNO = :SS_SRLNO "
            'End

            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            qry.CommandText += " and (ss_app_tag<>'RJ' or ss_app_tag is null )"
            'End by Manoj Kumar on 31-05-2021

            ' Dim dt = getRecordInDt(qry, conHrps)
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("SS_APPROVER", pno.ToString())
            qry.Parameters.AddWithValue("SS_ASSES_PNO", pno.ToString())
            'Added by TCS on 13122022, binding parameters.
            qry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            qry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            'End
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
            'MsgBox(ex.ToString())
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
            'qr.CommandText = "select a.IRC_CODE, SUBSTR(a.IRC_DESC,0,1) minmum, SUBSTR(a.IRC_DESC,3,2) maximum,decode(b.irc_desc,'Peer','Peers and Subordinate',b.irc_desc) irc_desc from t_ir_codes a,t_ir_codes b "
            'qr.CommandText += "  where a.irc_type in('360V3','360V4','360V5','360V6') and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"

            qr.CommandText = "Select decode(a.IRC_CODE,'PEER','PEERS and Subordinates',a.IRC_CODE) IRC_CODE SUBSTR(a.IRC_DESC,0,1) minmum, SUBSTR(a.IRC_DESC,3,2) maximum,decode(b.irc_desc,'Peer','Peers and Subordinate',b.irc_desc) irc_desc1,decode(a.irc_type,'360V2',decode(a.IRC_CODE,'PEER','Peers','ROPT','Subordinates',b.irc_desc),'360V3',decode(a.IRC_CODE,'PEER','Peer/Subordinate',b.irc_desc),decode(a.IRC_CODE,'INTSH','Peer/Subordinate/Internal Stackholder',b.irc_desc)) irc_desc,decode(a.irc_type,'360V3','IL3','360V4','IL4','360V5','IL5','360V6','IL6','360V2','IL2') Label from t_ir_codes a,t_ir_codes b"
            qr.CommandText += " where a.irc_type in('360V3','360V4','360V5','360V6','360V2') and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 5,1"

            Dim w = getRecordInDt(qr, conHrps)

            If w.Rows.Count > 0 Then
                GridView2.DataSource = w
                GridView2.DataBind()
            End If
        Catch ex As Exception


        End Try
    End Sub

    Public Sub BindAssesorGrid(pno As String)
        Try
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

            'str.CommandText += "SS_CATEG from hrps.t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and SS_STATUS='SE'  and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO  order by Category"

            str.CommandText += "SS_CATEG from hrps.t_survey_status ss where SS_ASSES_PNO in (select ema_perno from hrps.t_emp_master_feedback360 em where ss.ss_asses_pno=em.ema_perno and ss.ss_year=em.ema_year and ss.ss_srlno=em.ema_cycle and ema_perno = :SS_ASSES_PNO And trunc(ema_step2_stdt)<=trunc(sysdate) And trunc(ema_step2_enddt)>=trunc(sysdate)) and SS_STATUS='SE' order by Category"
            'Dim qry As New OracleCommand()
            'qry.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers And Subordinates','ROPT','Subordinates'"
            'qry.CommandText += " ,'INTSH','Internal Stakeholder',SS_CATEG) Category,SS_CATEG from t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and SS_STATUS='SE'  and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO  order by Category"
            ' Dim dt = getRecordInDt(qry, conHrps)
            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("SS_ASSES_PNO", pno.ToString())
            'str.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            'str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim da As New OracleDataAdapter(str)
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
                'btnrej.Visible = True
                'txtremarks.Visible = True
                divtitleCollab.Visible = True
            Else
                gvfinal.DataSource = Nothing
                gvfinal.DataBind()
                GridView1.DataSource = Nothing
                GridView1.DataBind()
                btnaddnontsl.Visible = False
                btnaddpeertsl.Visible = False
                lbOrg.Visible = False
                divtitle.Visible = False
                'btnrej.Visible = False
                'txtremarks.Visible = False
                divtitleCollab.Visible = False
            End If
            CheckApproved(pno)

            Dim r2 As New OracleCommand()
            'r2.CommandText = "select EMA_YEAR,EMA_CYCLE from hrps.t_emp_master_feedback360 where ema_perno='" & pno.ToString() & "' And trunc(ema_step1_stdt)<=trunc(sysdate) And trunc(ema_step1_enddt)>=trunc(sysdate)"
            r2.CommandText = "Select ema_year, ema_cycle FROM hrps.t_emp_master_feedback360 WHERE ema_perno = '" & pno.ToString() & "' AND trunc(ema_step1_stdt) <= trunc(SYSDATE) AND trunc(ema_step1_enddt) >= trunc(SYSDATE) AND ema_year = ( SELECT irc_desc FROM t_ir_codes WHERE irc_type = '360YS' AND irc_code = '360YS' AND irc_valid_tag = 'A' )"

            Dim g2 = getRecordInDt(r2, conHrps)
            If g2.Rows.Count > 0 Then
                ViewState("FY") = g2.Rows(0)("EMA_YEAR").ToString()
                ViewState("SRLNO") = g2.Rows(0).Item("EMA_CYCLE").ToString()
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub


    Public Function GetPno(pernr As String) As String
        Dim perno As String = ""
        Try

            Dim cm As New OracleCommand()
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            cm.CommandText = "Select EMA_ENAME from hrps.t_emp_master_feedback360  where ema_perno=:ema_perno "
            'End by Manoj Kumar on 31-05-2021
            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue(":ema_perno", pernr.ToUpper().ToString())
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
            qry.CommandText = "Select ema_ename from hrps.t_emp_master_feedback360 where  "
            qry.CommandText += "  and ema_perno=:IGP_user_id and EMA_EQV_LEVEL in('I2','I3','I4','I5','I6')"
            'qry.CommandText = "select * from hrps.t_emp_master_feedback360  where ema_perno=:ema_perno and EMA_EQV_LEVEL='I1' and ema_disch_dt is null "
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue(":IGP_user_id", pno.ToString())
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
                gvCollaborator.Visible = False
                'btnrej.Visible = False
                lbOrg.Visible = False
                'txtremarks.Visible = False
                btnaddnontsl.Visible = False
                btnaddpeertsl.Visible = False
                NoEachcateg(perno.Text.Trim)
            Else
                GridView1.Visible = False
                gvfinal.Visible = True
                gvCollaborator.Visible = True
                'btnrej.Visible = True
                lbOrg.Visible = True
                'txtremarks.Visible = True
                btnaddnontsl.Visible = True
                btnaddpeertsl.Visible = True
                NoEachcateg(perno.Text.Trim)
                PopDroupdown()
            End If

            'Added by TCS on 231222, to clear remarks textbox
            txtremarks.Text = String.Empty
            'End
            BindCollaboratorGrid(perno.Text)
        Catch ex As Exception

        End Try

    End Sub

    Public Function PageValid() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            'ocmd.CommandText = "select IRC_CODE from t_ir_codes where irc_type ='360PG' and trunc(IRC_START_DT) <= trunc(sysdate)"
            'ocmd.CommandText += "  and trunc(IRC_END_DT) >= trunc(sysdate) and IRC_VALID_TAG='A' and upper(irc_desc)=UPPER('SurveyApproval_OPR.ASPX')"


            'ocmd.CommandText = "select max(ema_step2_enddt) from t_emp_master_feedback360 where ema_perno in (select ss_pno from t_survey_status where ss_approver='" + Session("USER_ID") + "' and ss_year='" + ViewState("FY").ToString + "' and ss_srlno='" + ViewState("SRLNO").ToString + "' and ss_wfl_status='1') and trunc(ema_step2_enddt)>=trunc(sysdate)"

            ocmd.CommandText = "select max(ema_step2_enddt) from t_emp_master_feedback360 where ema_perno in (select ss_pno from t_survey_status where ss_approver='" + Session("USER_ID") + "' and ss_wfl_status='1') and trunc(ema_step2_enddt)>=trunc(sysdate)"
            Dim vc = getRecordInDt(ocmd, conHrps)
            If vc.Rows.Count > 0 Then
                If vc.Rows(0)(0).ToString <> "" Then
                    isvalid = True
                Else
                    isvalid = False
                End If
            Else
                isvalid = False
            End If
        Catch ex As Exception

        End Try
        Return isvalid
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
            MsgBox(ex.Message)
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function

    'Protected Sub GvCateg_RowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    Try

    '        'Dim dtsrl As DataTable = getsrlno()
    '        Dim id = CType(e.Row.FindControl("lblcatrg"), Label)
    '        Dim chk = CType(e.Row.FindControl("chkmgr"), CheckBox)
    '        Dim perno = CType(e.Row.FindControl("lblpno"), Label)

    '        Dim comnd As New OracleCommand()

    '        comnd.CommandText = "select *  from t_survey_status  where SS_STATUS='SE' and SS_DEL_TAG ='N' and ss_pno=:ss_pno  and ss_year=:ss_year and SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG and SS_SRLNO=(select IRC_CODE from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y')"
    '        comnd.Connection = conHrps
    '        comnd.Parameters.Clear()
    '        comnd.Parameters.AddWithValue(":ss_pno", perno.Text)
    '        comnd.Parameters.AddWithValue(":ss_year", ViewState("FY").ToString())
    '        comnd.Parameters.AddWithValue(":SS_ASSES_PNO", Session("assespno").ToString())
    '        comnd.Parameters.AddWithValue(":SS_CATEG", id.Text)
    '        Dim d = getRecordInDt(comnd, conHrps)

    '        If d.Rows.Count > 0 Then
    '            chk.Checked = True
    '        Else
    '            chk.Checked = False
    '        End If

    '    Catch ex As Exception
    '        'MsgBox(ex.ToString())
    '    End Try

    'End Sub
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
                    BindCollaboratorGrid(aperno.Text)
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If
                'ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                BindAssesorGrid(aperno.Text)
                BindCollaboratorGrid(aperno.Text)
            Else
                If Session("user_id") = id.Text.Trim Then
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Reporting Manager Cannot be unchecked")
                    chk.Checked = True
                    Exit Sub
                ElseIf categ.Text = "Self" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Self Cannot be unchecked")
                    chk.Checked = True
                    Exit Sub
                End If
                UpdateData(id.Text, "Y", categ.Text)
                ' ShowGenericMessageModal(CommonConstants.AlertType.warning, " De-Selected...!")
                BindAssesorGrid(aperno.Text)
                BindCollaboratorGrid(aperno.Text)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub UpdateData(id As String, tag As String, catg As String)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            'Session("USER_ID") = "197030"
            Dim perno = Session("USER_ID").ToString()
            Dim query As String = String.Empty
            'query = "update t_survey_status set SS_DEL_TAG ='" & tag & "', SS_UPDATED_DT=sysdate,SS_UPDATED_BY='" & perno & "' where SS_PNO='" & id & "' "
            'query += "and ss_year='" & ViewState("FY").ToString() & "' and SS_ASSES_PNO ='" & Session("assespno").ToString & "' and ss_categ='" & catg & "' and SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'"
            'Dim tran_Ins As OracleTransaction
            'tran_Ins = conHrps.BeginTransaction()
            query = "delete from t_survey_status where SS_PNO=:SS_PNO"
            ' Start WI368  by Manoj Kumar on 30-05-2021 
            query += " and ss_year=:ss_year and SS_ASSES_PNO =:SS_ASSES_PNO and ss_categ=:ss_categ and SS_SRLNO=:SS_SRLNO"  'WI368 query no space left between and conditions
            ' End by Manoj Kumar on 30-05-2021
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Add(New OracleParameter(":SS_PNO", id))
            comnd.Parameters.Add(New OracleParameter(":ss_year", ViewState("FY").ToString()))
            comnd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", Session("assespno").ToString))
            comnd.Parameters.Add(New OracleParameter(":ss_categ", catg))
            comnd.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
            comnd.ExecuteNonQuery()
            'c.Transaction = tran_Ins
            'c.ExecuteNonQuery()
            'tran_Ins.Commit()
        Catch ex As Exception
            'tran_Ins.Rollback()
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub UpdateData1(id As String, tag As String, categ As String)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            Dim query As String = String.Empty
            Dim perno = Session("USER_ID").ToString()
            query = "update t_survey_status set SS_DEL_TAG =:SS_DEL_TAG,ss_categ=:ss_categ,SS_UPDATED_DT=sysdate,SS_UPDATED_BY=:SS_UPDATED_BY where SS_PNO=:SS_PNO  and ss_year=:ss_year and SS_ASSES_PNO =:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Add(New OracleParameter(":SS_DEL_TAG", tag))
            comnd.Parameters.Add(New OracleParameter(":ss_categ", categ))
            comnd.Parameters.Add(New OracleParameter(":SS_UPDATED_BY", perno))
            comnd.Parameters.Add(New OracleParameter(":SS_PNO", id))
            comnd.Parameters.Add(New OracleParameter(":ss_year", ViewState("FY").ToString()))
            comnd.Parameters.Add(New OracleParameter(":SS_ASSES_PNO", Session("assespno").ToString))
            comnd.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
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
        PopDroupdown()
    End Sub
    Protected Sub btnaddnontsl_Click(sender As Object, e As EventArgs)
        divtsl.Visible = False
        divntsl.Visible = True
        Reset()
        PopDroupdown()
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

                BindAssesorGrid(assesor)
                BindCollaboratorGrid(assesor)
                ShowGenericMessageModal(CommonConstants.AlertType.success, " Added ...!")
                Reset()
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
        Dim ob As New SurveyApproval_OPR
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select distinct ema_ename ||'('|| ema_perno ||')' EName from hrps.t_emp_master_feedback360 where (ema_perno like  :ema_perno or upper(ema_ename) like "
            ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
            cmd.CommandText += " :ema_ename) "    'WI368 add officer class
            'End by Manoj Kumar on 30-05-2021
            'Added by TCS on 17122022 to add Year and Cycle Filter
            cmd.CommandText += " and ema_year=:ema_year and ema_cycle=:ema_cycle "
            'End


            ob.conHrps.Open()

            ' cmd.Connection = ob.conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", "%" & prefixText.ToUpper & "%")
            cmd.Parameters.AddWithValue("ema_ename", "%" & prefixText.ToUpper & "%")
            'Added by TCS on 17122022 to add Year and Cycle Filter
            cmd.Parameters.AddWithValue("ema_year", Convert.ToString(HttpContext.Current.Session("FYforNamePopup")))
            cmd.Parameters.AddWithValue("ema_cycle", Convert.ToString(HttpContext.Current.Session("CycleforNamePopup")))
            'End
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

                Dim r1 As New OracleCommand()
                r1.CommandText = "select EMA_YEAR,EMA_CYCLE from hrps.t_emp_master_feedback360 where ema_perno='" & assesor.ToString() & "' And trunc(ema_step1_stdt)<=trunc(sysdate) And trunc(ema_step1_enddt)>=trunc(sysdate)"
                Dim g1 = getRecordInDt(r1, conHrps)
                If g1.Rows.Count > 0 Then
                    ViewState("FY") = g1.Rows(0)("EMA_YEAR").ToString()
                    ViewState("SRLNO") = g1.Rows(0).Item("EMA_CYCLE").ToString()
                End If

                Dim perno = txtpnoI.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgI.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptI.Text.Trim, "'", "''")
                Dim email = Replace(txtemailI.Text.Trim, "'", "''")

                Dim val = Check(ViewState("FY").ToString(), assesor, pno)
                If val = "" Then
                    'Added by TCS on 09122022 to verify added respondent is correct or not as per below logic
                    If DropDownList1.SelectedValue.ToString = "INTSH" Then
                        If Not isINTSHSelectionValid(pno) Then
                            Reset()
                            ShowGenericMessageModal(CommonConstants.AlertType.error, perno & ", who has been identified as an internal stakeholder, has crossed the maximum limit of response as an internal stakeholder. You may wish to choose a different respondent as an Internal Stakeholder.")
                            Exit Sub
                        End If
                    ElseIf DropDownList1.SelectedValue.ToString = "MANGR" Then
                        Dim lvl = ChkAuthlabel(pno)
                        If Not isSelecetedManagerLevelValid(lvl) Then
                            Reset()
                            ShowGenericMessageModal(CommonConstants.AlertType.error, "Only higher level Manager selection allowed")
                            Exit Sub
                        End If
                    End If
                    'End
                    SaveData(DropDownList1.SelectedValue.ToString, pno, name, desg, dept, email, lbldesg1.Text, "ORG", "SE", assesor)
                    Reset()
                    BindAssesorGrid(assesor)
                    BindCollaboratorGrid(assesor)
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
            'MsgBox(ex.ToString)
        End Try
    End Sub
    Public Function ChkAuthlabel(pno As String) As String
        Try
            Dim chk As String = String.Empty
            'If conHrps.State = ConnectionState.Closed Then
            '    conHrps.Open()
            'End If
            'Dim qry1 As New OracleCommand()
            'qry1.CommandText = "select SS_IL from t_assesse_IL  where SS_ASSESS_PNO=:ema_perno and SS_STATUS='A'"
            'qry1.Connection = conHrps
            'qry1.Parameters.Clear()
            'qry1.Parameters.AddWithValue("ema_perno", pno.ToString())
            'Dim daIL As New OracleDataAdapter(qry1)
            'Dim dtIL As New DataTable()
            'daIL.Fill(dtIL)
            'If dtIL.Rows.Count = 0 Then

            Dim qry As New OracleCommand()

            'Commentec & Added by TCS on 17122022, Replace Step 1 Date with Year & Cycle
            'qry.CommandText = "select EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where ema_perno=:ema_perno And trunc(ema_step1_stdt)<=trunc(sysdate) And trunc(ema_step1_enddt)>=trunc(sysdate) and EMA_EQV_LEVEL in('I2','I3','I4','I5','I6') "
            qry.CommandText = "select EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where ema_perno=:ema_perno And ema_year=:ema_year and ema_cycle=:ema_cycle and EMA_EQV_LEVEL in('I2','I3','I4','I5','I6') "
            'End
            qry.Connection = conHrps
                qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_perno", pno.ToString())
            'Added by TCS on 17122022, Replace Step 1 Date with Year & Cycle
            qry.Parameters.AddWithValue("ema_year", ViewState("FY").ToString())
            qry.Parameters.AddWithValue("ema_cycle", ViewState("SRLNO").ToString())
            'End
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
            'ShowGenericMessageModal(CommonConstants.AlertType.error, ex.message)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function
    Public Sub PopDroupdown()
        Try
            Dim qry As String = String.Empty
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim lvl As String = ChkAuthlabel(Session("assespno"))
            If lvl = "I4" Or lvl = "I5" Or lvl = "I6" Then
                qry = "select IRC_CODE,decode(upper(IRC_DESC),'INTERNAL STAKEHOLDER','Internal Stakeholder/Peers/Subordinates',IRC_DESC) IRC_DESC from t_ir_codes where irc_type='360RL' and IRC_VALID_TAG='A' and IRC_CODE not in('SELF','ROPT','PEER') order by 2"
            End If
            If lvl = "I3" Then
                qry = "select IRC_CODE,decode(UPPER(IRC_DESC),'PEER','Peers and Subordinates',IRC_DESC) IRC_DESC from t_ir_codes where irc_type='360RL' and IRC_VALID_TAG='A' and IRC_CODE not in('SELF','ROPT') order by 2"
            End If
            If lvl = "I2" Then
                qry = "select IRC_CODE,decode(UPPER(IRC_DESC),'PEER','Peers','ROPT','Subordinates',IRC_DESC) IRC_DESC from t_ir_codes where irc_type='360RL' and IRC_VALID_TAG='A' and IRC_CODE not in('SELF') order by 2"
            End If
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
            str.CommandText += "and ((SS_PNO=:SS_PNO) OR (upper(ss_email)=:SS_PNO)) and SS_status='SE' and ss_del_tag ='N' and irc_type='360RL' and upper(ss_categ)=upper(irc_code) and SS_SRLNO='" & ViewState("SRLNO").ToString() & "'"

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
    Public Function ChkValidationmaxmgr1() As String
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

            End If
            ''''WI447: add logic to allow maximum and minimum thresold value for each respondent,Changed by: Avik Mukherjee on 30-06-2021''''

            'cmdqry.CommandText = "select a.IRC_CODE,SUBSTR(a.IRC_DESC,0,1) minmum, SUBSTR(a.IRC_DESC,3,2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2)  maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type='" + type + "' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='MANGR'"
            '''''''End of change WI447
            cmdqry.Connection = conHrps
            'cmdqry.Parameters.AddWithValue("irc_type", type)
            Dim dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
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
            MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxmgr() As String
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

            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1)  minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2)  maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type='" + type + "' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='MANGR'"
            cmdqry.Connection = conHrps
            'cmdqry.Parameters.AddWithValue("irc_type", type)
            Dim dt = getRecordInDt(cmdqry, conHrps)
            'Dim dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxstake() As String
        Dim status As String = ""
        Try
            Dim cmdqryst As New OracleCommand()
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
            Dim ls_sql As String = String.Empty
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmdqryst.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1)  minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2)  maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqryst.CommandText += "  where a.irc_type='" + type + "' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='INTSH'"
            cmdqryst.Connection = conHrps
            ' cmdqryst = New OracleCommand(ls_sql, conHrps)
            '  cmdqryst.Parameters.AddWithValue("irc_type", type)
            Dim danew As New OracleDataAdapter(cmdqryst.CommandText, conHrps)
            Dim dt As New DataTable
            danew.Fill(dt)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()

                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("assespno").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da1 As New OracleDataAdapter(cmdqry)
                    da1.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
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
    Public Sub SaveData(ByVal role As String, ByVal pno As String, ByVal name As String, ByVal desg As String, ByVal org As String, ByVal email As String, ByVal lvl As String, ByVal orgtype As String, ByVal status As String, assespno As String)

        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            Dim OrgStr As String = String.Empty
            Dim id = getRefNo()
            If pno = "" Then
                pno = id
            End If
            'If role = "INTSH" Then
            '    Dim statmaxstake = ChkValidationmaxstake()

            '    If Len(statmaxstake) > 0 Then
            '        ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxstake & " Category exceed maximum number")
            '        Exit Sub
            '    End If
            'End If
            'If role = "MANGR" Then
            '    Dim statmaxman = ChkValidationmaxmgr()
            '    If Len(statmaxman) > 0 Then
            '        ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxman & " Category exceed maximum number")
            '        Exit Sub
            '    End If
            'End If
            Dim statmaxstake = ChkValidationRangeCategory(role.ToString)
            If Len(statmaxstake) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxstake & " Category exceed maximum number")
                Exit Sub
            End If
            OrgStr = "insert into T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL,ss_wfl_status,ss_approver,SS_SRLNO) values (:SS_CATEG,:SS_ID,:SS_PNO,:SS_NAME,:SS_DESG,:SS_DEPT,:SS_EMAIL,:SS_STATUS,"
            OrgStr += " :SS_TAG,:SS_CRT_BY,sysdate,:SS_DEL_TAG,:SS_TYPE,:ss_year,:SS_ASSES_PNO,:SS_LEVEL,'1',:ss_approver,:SS_SRLNO)"

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
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.ExecuteNonQuery()
            ' comnd.ExecuteNonQuery()
            ' Clear()

        Catch ex As Exception
            'MsgBox(ex.ToString)

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
            strRef = " select MAX(to_number(substr(SS_ID,3,10))) SS_ID from T_SURVEY_STATUS where SS_ID like 'SR%' and SS_YEAR='" & ViewState("FY").ToString() & "'"
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

    Protected Sub txtpnoI_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoI.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " (ema_ename||ema_perno)=:pno"
            ' End by Manoj Kumar on 31-05-2021
            'Added by TCS on 17122022, Year & Cycle filteration
            strself.CommandText += " and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE "
            'End
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            'Added by TCS on 17122022, Year & Cycle filteration
            strself.Parameters.Add(New OracleParameter("EMA_YEAR", ViewState("FY").ToString()))
            strself.Parameters.Add(New OracleParameter("EMA_CYCLE", ViewState("SRLNO").ToString()))
            'End
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
        PopDroupdown()
        txtdeptI.Text = ""
        txtemailI.Text = ""
        txtdesgI.Text = ""
        txtpnoI.Text = ""
    End Sub

    Public Function ChkValidation() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty
            Dim type1 As String = ChkAuthlabel(Session("assespno"))
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
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1)  minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2)  maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type='" + type + "' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            cmdqry.Connection = conHrps
            'cmdqry.Parameters.Clear()
            'cmdqry.Parameters.AddWithValue("irc_type", type)
            Dim dt = getRecordInDt(cmdqry, conHrps)
            If type.Equals("360V5") Or type.Equals("360V6") Or type.Equals("360V4") Then
                cmdqry.CommandText += " and a.IRC_CODE not in('PEER','ROPT')"
            ElseIf type.Equals("360V3") Then
                cmdqry.CommandText += " and a.IRC_CODE not in('ROPT')"
            End If
            dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'"
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
                        status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("minmum").ToString() & "), "
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
    Protected Sub submit()
        Try
            'Dim statmaxstake = ChkValidationmaxstake1()
            'If Len(statmaxstake) > 0 Then
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxstake & " Category exceed maximum number")
            '    Exit Sub
            'End If
            'Dim statmaxman = ChkValidationmaxmgr1()
            'If Len(statmaxman) > 0 Then
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxman & " Category exceed maximum number")
            '    Exit Sub
            'End If
            'Dim stat = ChkValidation()
            'If Len(stat) > 0 Then
            '    ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & stat & " Category")
            '    Exit Sub
            'End If
            Dim status As String = ChkValidationMiniRangeCategory()
            If Len(status) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & status & " Category")
                Exit Sub
            End If
            Dim statmaxstake = ChkValidationmaxstake1()
            If Len(statmaxstake) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmaxstake & " Category exceed maximum number")
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
            'Added by TCS on 13122022, to remove add any respondent after approval
            divtsl.Visible = False
            gvfinal.Visible = False
            gvCollaborator.Visible = False
            btnaddpeertsl.Visible = False
            btnaddnontsl.Visible = False
            'End
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Approved...")
            gvfinal.Visible = False
            gvCollaborator.Visible = False
            GridView1.Visible = True
            bindGrid()
            BindAssesorGrid(Session("assespno").ToString)
            BindCollaboratorGrid(Session("assespno").ToString)
            'Added by TCS on 231222, to clear remarks textbox
            txtremarks.Text = String.Empty
            'End
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Approve(ByVal pno As String, ByVal tag As String)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            Dim qry As String = String.Empty
            'qry = "update t_survey_status set SS_APP_TAG='" & tag & "',SS_TAG_DT=sysdate,SS_WFL_STATUS='2' where SS_PNO='" & pno & "'  and ss_year='" & ViewState("FY").ToString() & "'"
            'qry += "  and SS_ASSES_PNO ='" & Session("assespno") & "'"

            qry = "update t_survey_status set SS_APP_TAG=:SS_APP_TAG,SS_TAG_DT=sysdate,SS_WFL_STATUS='2',SS_UPDATED_BY=:SS_UPDATED_BY, SS_UPDATED_DT=sysdate where "
            qry += " SS_PNO=:SS_PNO and ss_year=:ss_year and SS_ASSES_PNO =:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO"
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
            c.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
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
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim assespno As String = String.Empty
                assespno = CType(e.Row.FindControl("lblpno"), Label).Text
                Dim btn = CType(e.Row.FindControl("lbpendingapproval"), LinkButton)

                Dim qry As New OracleCommand()

                'qry.CommandText = "select distinct SS_APP_TAG from t_survey_status  where SS_ASSES_PNO='" & assespno & "'  and ss_year='" & ViewState("FY").ToString() & "' and SS_TAG='SU' and SS_DEL_TAG='N' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "'"

                qry.CommandText = "select distinct SS_APP_TAG from t_survey_status  where SS_ASSES_PNO in (select ema_perno from hrps.t_emp_master_feedback360 where ema_perno = '" & assespno & "' And trunc(ema_step2_stdt)<=trunc(sysdate) And trunc(ema_step2_enddt)>=trunc(sysdate)) and SS_TAG='SU' and SS_DEL_TAG='N'"

                'Added by TCS on 12292023, to filter only crrent cycle record
                qry.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "'"
                'End

                'And trunc(ema_step2_stdt)<=trunc(sysdate) And trunc(ema_step2_enddt)>=trunc(sysdate)
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
            End If
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try
    End Sub
    Public Sub CheckApproved(pno As String)

        Try
            Dim qry As New OracleCommand()
            'qry.CommandText = "select * from t_survey_status  where SS_APP_TAG='AP' and SS_ASSES_PNO='" & pno & "'  and ss_year='" & ViewState("FY").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "'"

            qry.CommandText = "select * from t_survey_status ss  where SS_APP_TAG='AP' and SS_ASSES_PNO in (select ema_perno from hrps.t_emp_master_feedback360 em where ss.ss_asses_pno=em.ema_perno and ss.ss_year=em.ema_year and ss.ss_srlno=em.ema_cycle and ema_perno = '" & pno & "' And trunc(ema_step2_stdt)<=trunc(sysdate) And trunc(ema_step2_enddt)>=trunc(sysdate))"

            'Added by TCS on 12292023, to filter only crrent cycle record
            qry.CommandText += " and ss_year='" & ViewState("FY").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "'"
            'End

            Dim s = getRecordInDt(qry, conHrps)
            If s.Rows.Count > 0 Then
                btnaddpeertsl.Visible = False
                btnaddnontsl.Visible = False
                lbOrg.Visible = False
                'btnrej.Visible = False
                divtitle.Visible = True
                'txtremarks.Visible = False
                divtitleCollab.Visible = True
            Else
                btnaddpeertsl.Visible = True
                btnaddnontsl.Visible = True
                lbOrg.Visible = True
                divtitle.Visible = True
                'btnrej.Visible = True
                'txtremarks.Visible = True
                divtitleCollab.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Reject()
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            If txtremarks.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please enter the remarks for sending the form back to the officer")
                Exit Sub
            End If

            Dim uprem As New OracleCommand()
            uprem.Connection = conHrps
            Dim remarks As String = Replace(txtremarks.Text.Trim(), "'", "''")
            remarks = Replace(txtremarks.Text.Trim(), ",", " ")
            remarks = Replace(txtremarks.Text.Trim(), ";", " ")
            remarks = Replace(txtremarks.Text.Trim(), ":", " ")
            remarks = Replace(txtremarks.Text.Trim(), "&", "and")
            Dim p = Session("assespno").ToString()
            ' uprem.CommandText = " update t_survey_status set SS_REMARKS ='" & remarks & "' ,SS_APP_TAG='RJ', SS_TAG_DT=sysdate "
            ' uprem.CommandText += " where SS_ASSES_PNO ='" & p & "'  and ss_year='" & ViewState("FY").ToString() & "' "
            uprem.CommandText = " update t_survey_status set SS_REMARKS =:SS_REMARKS,SS_APP_TAG='RJ', SS_TAG_DT=sysdate ,ss_wfl_status=null,ss_tag='N', "
            uprem.CommandText += "SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO =:SS_ASSES_PNO and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO "
            uprem.Parameters.Clear()
            uprem.Parameters.AddWithValue("SS_REMARKS", remarks)
            uprem.Parameters.AddWithValue("SS_ASSES_PNO", p)
            uprem.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            uprem.Parameters.AddWithValue("SS_UPDATED_BY", Session("USER_ID").ToString())
            uprem.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            uprem.ExecuteNonQuery()
            ShowGenericMessageModal(CommonConstants.AlertType.success, "The Form Has Been sent back to the officer...!")
            'txtremarks.Visible = False
            'btnrej.Visible = False
            lbOrg.Visible = False
            divntsl.Visible = False
            divtsl.Visible = False
            gvfinal.Visible = False
            gvCollaborator.Visible = False
            btnaddpeertsl.Visible = False
            btnaddnontsl.Visible = False
            divtitle.Visible = False
            GridView2.Visible = False
            bindGrid()
            SentMailReturned(p, remarks)
            'Added by TCS on 231222, to clear remarks textbox
            txtremarks.Text = String.Empty
            'End
            divtitleCollab.Visible = False
        Catch ex As Exception
            'MsgBox(ex.ToString())
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub SentMailReturned(pno As String, remarks As String)

        '''' send mail in case of approver revert request to assesse
        Try
            Dim strmailcomd As New OracleCommand()
            strmailcomd.CommandText = "Select ema_email_id from hrps.t_emp_master_feedback360 where ema_perno='" & pno.ToString & "' "

            Dim df = getRecordInDt(strmailcomd, conHrps)

            If df.Rows.Count > 0 Then
                strmailcomd.CommandText = "Select nvl(SS_REMARKS,'NA') SS_REMARKS from hrps.t_survey_status where SS_ASSES_PNO='" & pno.ToString & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "' and SS_YEAR='" & ViewState("FY") & "'"
                Dim link As String = ""
                Dim dtmessage As DataTable = getRecordInDt(strmailcomd, conHrps)
                link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/SelectAssesor_OPR.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/SelectAssesor_OPR.aspx <a/>"
                Dim body As String = String.Empty
                'Added & Commented by TCS on 14122022, Change the text of email body
                'body = "Dear Colleague, <br/><br/>The list of respondents which you had submitted for your baseline 360-degree feedback has been returned by your manager.<br/>"
                body = "Dear Colleague, <br/><br/>The list of respondents which you had submitted for your End Year 360-degree feedback has been returned by your manager.<br/>"
                'End
                body += "You are requested to modify the list of respondents as per comments of your manager and submit the same at the earliest.<br/><br/>"
                body += link
                body += "<br/>"
                body += "Remarks from approver: " + dtmessage.Rows(0).Item(0).ToString()
                body += "<br/>If you have a query, please connect with your HRBP. For IT related assistance, please log a call with the IT helpdesk (it_helpdesk@tatasteel.com)."
                body += " <br/><br/> With Regards,<br/>"
                body += "HRM Team<br/><br/> <b>This is system generated mail.Please do not reply</b>"


                Dim mail As New System.Net.Mail.MailMessage()
                mail.Bcc.Add(df.Rows(0)("ema_email_id"))
                mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

                mail.Subject = "360 feedback returned by Manager"

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
    'Added by TCS on 15122022 to validate respondent
    Public Function isINTSHSelectionValid(ByVal intshPerno As String) As Boolean
        Dim isTrue As Boolean = False
        Dim totalSelected, intshLevel As String
        Dim maxINTSHSelectionCount As Integer
        Dim cmd As New OracleCommand()
        Try
            cmd = New OracleCommand
            cmd.CommandText = "SELECT COUNT(*) FROM T_SURVEY_STATUS WHERE SS_CATEG = 'INTSH' AND SS_PNO =:SS_PNO AND SS_YEAR=:SS_YEAR AND SS_SRLNO=:SS_SRLNO AND SS_WFL_STATUS in ('1','2','3')"
            'End
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_PNO", intshPerno)
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString)
            Dim dt = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                totalSelected = Convert.ToString(dt.Rows(0)(0))
            Else
                totalSelected = "0"
            End If
            intshLevel = ChkAuthlabel(intshPerno)
            If Not String.IsNullOrEmpty(intshLevel) Then
                maxINTSHSelectionCount = getMaxINTSHIdentifiedCount(intshLevel)
                If Convert.ToInt32(totalSelected) < maxINTSHSelectionCount Then
                    isTrue = True
                Else
                    isTrue = False
                End If
            Else
                isTrue = True
            End If
        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Public Function getMaxINTSHIdentifiedCount(ByVal level As String) As Integer
        Dim count As Integer
        Try
            Dim cmd As New OracleCommand()
            cmd.CommandText = "select IRC_DESC from t_ir_codes where irc_type ='360IS' AND IRC_CODE = :IRC_CODE AND IRC_VALID_TAG='A'"
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("IRC_CODE", level)
            Dim dt = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                If IsNumeric(Convert.ToString(dt.Rows(0)(0))) Then
                    count = Convert.ToInt32(dt.Rows(0)(0))
                Else
                    count = 0
                End If
            Else
                count = 0
            End If
        Catch ex As Exception

        End Try
        Return count
    End Function
    Public Function isSelecetedManagerLevelValid(ByVal managerLevel As String) As Boolean
        Dim isValid As Boolean = False
        Dim sessionUserLevel As String
        Try
            sessionUserLevel = Convert.ToString(Session("label"))
            If sessionUserLevel.Equals("I4") Or sessionUserLevel.Equals("I5") Or sessionUserLevel.Equals("I6") Then
                If Convert.ToInt32(sessionUserLevel.Remove(0, 1)) > Convert.ToInt32(managerLevel.Remove(0, 1)) Then
                    isValid = True
                Else
                    isValid = False
                End If
            Else
                isValid = True
            End If
        Catch ex As Exception

        End Try
        Return isValid
    End Function
    'End
    Public Sub BindCollaboratorGrid(pno As String)
        Try
            Session("label") = ChkAuthlabel(pno)

            'Dim qry As New OracleCommand()
            Dim str As New OracleCommand

            str.CommandText = "SELECT EC_COLLABORATOR_PERNO, EMA_ENAME, EMA_EQV_LEVEL,EMA_DESGN_DESC, EMA_DEPT_DESC, EMA_EMAIL_ID FROM T_EMPL_COLLABORATOR, T_EMP_MASTER_FEEDBACK360 WHERE EC_COLLABORATOR_PERNO = EMA_PERNO AND EC_YEAR = EMA_YEAR AND EC_CYCLE = EMA_CYCLE AND EC_PERNO = :PERNO AND EC_YEAR = :YEAR AND EC_CYCLE = :CYCLE AND EC_COLLABORATOR_PERNO NOT IN (SELECT SS_PNO FROM T_SURVEY_STATUS WHERE SS_ASSES_PNO = :PERNO AND SS_WFL_STATUS = '1' AND SS_YEAR=:YEAR AND SS_SRLNO=:CYCLE) ORDER BY 1"
            'Dim qry As New OracleCommand()
            'qry.CommandText = "select SS_ASSES_PNO, SS_PNO,SS_NAME,SS_DESG,ss_level,SS_DEPT,SS_EMAIL,decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers And Subordinates','ROPT','Subordinates'"
            'qry.CommandText += " ,'INTSH','Internal Stakeholder',SS_CATEG) Category,SS_CATEG from t_survey_status where SS_ASSES_PNO=:SS_ASSES_PNO and SS_STATUS='SE'  and ss_year=:ss_year and SS_SRLNO=:SS_SRLNO  order by Category"
            ' Dim dt = getRecordInDt(qry, conHrps)
            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("PERNO", pno)
            str.Parameters.AddWithValue("YEAR", ViewState("FY").ToString())
            str.Parameters.AddWithValue("CYCLE", ViewState("SRLNO").ToString())
            Dim da As New OracleDataAdapter(str)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                gvCollaborator.DataSource = Nothing
                gvCollaborator.DataBind()
                gvCollaborator.DataSource = dt
                gvCollaborator.DataBind()
                divtitleCollab.Visible = True
            Else
                gvCollaborator.DataSource = Nothing
                gvCollaborator.DataBind()
                divtitleCollab.Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Public Sub GetApprovalLastDate()
        Try
            Dim str As New OracleCommand

            str.CommandText = "SELECT to_char(MAX(EMA_STEP2_ENDDT),'dd-Mon-yyyy') FROM T_EMP_MASTER_FEEDBACK360 WHERE EMA_YEAR=:ss_year AND EMA_CYCLE=:SS_SRLNO"
            str.Connection = conHrps
            str.Parameters.Clear()
            str.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim da As New OracleDataAdapter(str)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                spnLastapprovaldate.InnerText = Convert.ToString(dt.Rows(0)(0))
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
End Class
