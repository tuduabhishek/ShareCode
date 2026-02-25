Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Partial Class FeedbackRpt1
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim cntPeer As Integer
    Dim cntSub As Integer
    Dim cntSelf As Integer
    Dim cntMgr As Integer
    Dim cntStkholder As Integer
    Dim cntMinFullFill As Integer

    Private Sub FeedbackRpt1_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub
    Private Sub FeedbackRpt1_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblMessage.Text = ""
            pnlSerach.Visible = False
            If ChkRole() Then
                'If Request.QueryString("adm") = "1" Then
                pnlSerach.Visible = True
                    GetFy()
                    getPnoDetails(Session("ADM_USER").ToString())
                    getresCoupt(Session("ADM_USER").ToString())
                    getScore(Session("ADM_USER").ToString())
                    getQ2AresCoupt(Session("ADM_USER").ToString())
                    getQ2BresCoupt(Session("ADM_USER").ToString())
                'End If
            End If
        End If
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "KeyGenericMessageModal", "screenshot('02-03-2021');", True)
    End Sub
    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False
            Dim cmd As String
            cmd = "select * from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            cmd += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            Dim f = GetData(cmd, conHrps)
            If f.Rows.Count > 0 Then
                t = True
            Else
                t = False
            End If
            Return t
        Catch ex As Exception

        End Try
    End Function
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
            Dim q As String
            q = "Select IGP_user_id,ema_ename from t_ir_adm_grp_privilege,TIPS.t_empl_all where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno.ToString() & "' and EMA_COMP_CODE='1000'"
            Dim p = GetData(q, conHrps)
            If p.Rows.Count > 0 Then
                d = True
            Else
                d = False

            End If
            Return d
        Catch ex As Exception

        End Try
    End Function
    Protected Sub lbtnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnView.Click
        Try
            cntMinFullFill = 1
            lblMessage.Text = ""
            If txt_Pno.Text <> "" Then
                Dim qry As String = String.Empty
                qry = "select decode(upper(ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager',upper('PEER'),'Peers',upper('intsh') "
                qry += ",'Internal stakeholders',upper('ropt'),'Subordinates') ss_categ,sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),2) A,"
                qry += "round(sum(ss_q1_b)/count(*),2) B,round(sum(ss_q1_c)/count(*),2) C,round(sum(ss_q1_d)/count(*),2) D"
                qry += ",count(*) No_Records,upper(ss_categ) categ from t_survey_status where ss_asses_pno ='" & txt_Pno.Text & "' and ss_wfl_status ='3' and ss_year='" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP' and upper(ss_categ) not in ('PEER','ROPT')  group by ss_categ"
                Dim re As String = String.Empty
                Dim cont As Integer = 0
                Dim dMinScore = GetData(qry, conHrps)
                If Not dMinScore Is Nothing Then
                    If dMinScore.Rows.Count < 3 Then
                        cntMinFullFill = 0
                    Else
                        For i As Integer = 0 To dMinScore.Rows.Count - 1
                            If dMinScore.Rows(i)("categ").ToString = "SELF" And Val(dMinScore.Rows(i)("No_Records").ToString) < 1 Then
                                cntMinFullFill = 0
                            ElseIf dMinScore.Rows(i)("categ").ToString = "INTSH" And Val(dMinScore.Rows(i)("No_Records").ToString) < 3 Then
                                cntMinFullFill = 0
                            ElseIf dMinScore.Rows(i)("categ").ToString = "MANGR" And Val(dMinScore.Rows(i)("No_Records").ToString) < 1 Then
                                cntMinFullFill = 0
                                'ElseIf dMinScore.Rows(i)("categ").ToString = "PEER" And Val(dMinScore.Rows(i)("No_Records").ToString) < 3 Then
                                '    cntMinFullFill = 0
                                'ElseIf dMinScore.Rows(i)("categ").ToString = "ROPT" And Val(dMinScore.Rows(i)("No_Records").ToString) < 3 Then
                                '    cntMinFullFill = 0
                            End If
                        Next
                    End If

                    If cntMinFullFill = 0 Then
                        'txt_Pno.Text = ""
                        lblMessage.Text = "Minimum criteria not met."
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "savedtlserror", "info('Minimum criteria not full fill respondent.','','info');", True)
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "show", "Showmodaldiv('#MSGBOX');", True)
                        'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "show", "Showmodaldiv('#IMGBOX');", True)
                    Else
                        getPnoDetails(txt_Pno.Text)
                        getresCoupt(txt_Pno.Text)
                        getScore(txt_Pno.Text)
                        getQ2AresCoupt(txt_Pno.Text)
                        getQ2BresCoupt(txt_Pno.Text)
                    End If
                Else
                    'txt_Pno.Text = ""
                    lblMessage.Text = "Record not found."
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "show", "Showmodaldiv('#MSGBOX');", True)
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "savedtlserror", "info('Minimum criteria not full fill respondent.','','info');", True)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function GetData(qry As String, con As OracleConnection) As DataTable

        Try
            Dim comd As New OracleDataAdapter(qry, con)
            Dim data As New DataTable()
            comd.Fill(data)
            If data.Rows.Count > 0 Then
                Return data
            Else
                Return Nothing
            End If
        Catch ex As Exception

        End Try

    End Function
    Public Sub GetFy()
        Try
            Dim s As String = String.Empty
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY FROM DUAL"
            Dim f = GetData(s, conHrps)

            If f.Rows.Count > 0 Then
                ViewState("FY") = f.Rows(0)(0)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub getQ2AresCoupt(pno As String)
        Try
            Dim qry As String = String.Empty
            qry = "select ss_q2_a from t_survey_status  where ss_asses_pno='" & pno & "' and ss_year='" & ViewState("FY") & "' and upper(ss_categ) = upper('self') and ss_q2_a is not null and ss_wfl_status='3'"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim dt As DataTable
            dt = GetData(qry, conHrps)

            Dim qry1 As String = String.Empty
            qry1 = "select ss_q2_a from t_survey_status  where ss_asses_pno='" & pno & "' and ss_year='" & ViewState("FY") & "' and upper(ss_categ) <> upper('self') and ss_q2_a is not null and ss_wfl_status='3' "
            Dim dt1 As DataTable
            dt1 = GetData(qry1, conHrps)

            If Not dt1 Is Nothing Then
                lblQAOtrRespond.Text = "<ul>"
                For i = 0 To dt1.Rows.Count - 1
                    lblQAOtrRespond.Text += "<li>" + dt1.Rows(i)("ss_q2_a").ToString + "</li>"
                Next
                lblQAOtrRespond.Text += "</ul>"
            Else
                lblQAOtrRespond.Text = ""
            End If

            If Not dt Is Nothing Then
                lblQASelf.Text = "<ul>"
                For g = 0 To dt.Rows.Count - 1
                    lblQASelf.Text += "<li>" + dt.Rows(g)("ss_q2_a").ToString + "</li>"
                Next
                lblQASelf.Text += "</ul>"
            Else
                lblQASelf.Text = ""
            End If


        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub
    Public Sub getQ2BresCoupt(pno As String)
        Try
            Dim qry As String = String.Empty
            qry = "select ss_q2_b from t_survey_status  where ss_asses_pno='" & pno & "' and ss_year='" & ViewState("FY") & "' and upper(ss_categ) = upper('self') and ss_q2_b is not null and ss_wfl_status='3'"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim dt As DataTable
            dt = GetData(qry, conHrps)

            Dim qry1 As String = String.Empty
            qry1 = "select ss_q2_b from t_survey_status  where ss_asses_pno='" & pno & "' and ss_year='" & ViewState("FY") & "' and upper(ss_categ) <> upper('self') and ss_q2_b is not null and ss_wfl_status='3' "
            Dim dt1 As DataTable
            dt1 = GetData(qry1, conHrps)

            If Not dt1 Is Nothing Then
                lblQBOtrRespond.Text = "<ul>"
                For i = 0 To dt1.Rows.Count - 1
                    lblQBOtrRespond.Text += "<li>" + dt1.Rows(i)("ss_q2_b").ToString + "</li>"
                Next
                lblQBOtrRespond.Text += "</ul>"
            Else
                lblQBOtrRespond.Text = ""
            End If

            If Not dt Is Nothing Then
                lblQBSelf.Text = "<ul>"
                For g = 0 To dt.Rows.Count - 1
                    lblQBSelf.Text += "<li>" + dt.Rows(g)("ss_q2_b").ToString + "</li>"
                Next
                lblQBSelf.Text += "</ul>"
            Else
                lblQBSelf.Text = ""
            End If


        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub
    Public Sub getPnoDetails(pno As String)
        Dim qry As String = String.Empty
        qry = "select ema_ename,ema_dept_desc from tips.t_empl_all where ema_perno='" & pno & "' and EMA_COMP_CODE='1000' and EMA_DISCH_DT is null"
        Dim dtPno = GetData(qry, conHrps)
        If dtPno.Rows.Count > 0 Then
            lblDesignation.Text = dtPno.Rows(0)("ema_dept_desc").ToString
            lblReceiptNm.Text = dtPno.Rows(0)("ema_ename").ToString
            lblReceiptNm1.Text = dtPno.Rows(0)("ema_ename").ToString
            lblReceiptNm3.Text = dtPno.Rows(0)("ema_ename").ToString
            lblReceiptNm4.Text = dtPno.Rows(0)("ema_ename").ToString
        End If
    End Sub
    Public Sub getresCoupt(pno As String)
        Try
            Dim qry As String = String.Empty
            qry = "select A.ss_year,A.ss_asses_pno,A.ss_categ, decode(a.ss_categ,'INTSH',3,'MANGR',1,'PEER',3,'ROPT',3,'Self',1,'SELF',1) minim,a.approved,nvl(c.completed, 0) Completed,NVL(b.rejected, 0) Insufficient_Exposure,decode(sign(a.approved - b.rejected -decode(a.ss_categ,'INTSH', 3, 'MANGR',1, 'PEER', 3,'ROPT',3)) ,'-1','LESS','OK') criteria,d.irc_desc from (select ss_year,ss_asses_pno,ss_categ,count(*) approved from hrps.t_survey_status where ss_year = '" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP' group by ss_year, ss_asses_pno, ss_categ) a,(select ss_year, ss_asses_pno, ss_categ , count(*) rejected from hrps.t_survey_status where ss_year = '2020' and ss_status = 'SE'  and ss_del_tag = 'N'  and ss_app_tag = 'AP' and ss_wfl_status = 9  group by ss_year, ss_asses_pno, ss_categ) b,(select ss_year, ss_asses_pno, ss_categ, count(*) completed  from hrps.t_survey_status where ss_year = '" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP' and ss_wfl_status = 3 group by ss_year, ss_asses_pno, ss_categ) c,(select irc_code,irc_desc from t_ir_codes where  irc_type='360RL' ) d where a.ss_year = b.ss_year(+)  and a.ss_asses_pno = b.ss_asses_pno(+) and a.ss_categ = b.ss_categ(+) and a.ss_year = c.ss_year(+) and a.ss_asses_pno = c.ss_asses_pno(+) and a.ss_categ = c.ss_categ(+) and upper(a.ss_categ)=d.irc_code(+) and a.ss_asses_pno ='" & pno & "' order by 1, 2, 3, 7"

            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim qry1 As String = String.Empty
            qry1 = "select A.ss_year,A.ss_asses_pno,nvl(sum(a.approved),0) approved,nvl(sum(c.completed), 0) Completed,NVL(sum(b.rejected), 0) Insufficient_Exposure from (select ss_year,ss_asses_pno,ss_categ,count(*) approved from hrps.t_survey_status where ss_year = '" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP'  group by ss_year, ss_asses_pno, ss_categ) a,(select ss_year, ss_asses_pno, ss_categ , count(*) rejected from hrps.t_survey_status where ss_year = '" & ViewState("FY") & "' and ss_status = 'SE'  and ss_del_tag = 'N'  and ss_app_tag = 'AP' and ss_wfl_status = 9  group by ss_year, ss_asses_pno, ss_categ) b,(select ss_year, ss_asses_pno, ss_categ, count(*) completed from hrps.t_survey_status where ss_year = '" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP' and ss_wfl_status = 3  group by ss_year, ss_asses_pno, ss_categ) c where a.ss_year = b.ss_year(+)  and a.ss_asses_pno = b.ss_asses_pno(+) and a.ss_categ = b.ss_categ(+) and a.ss_year = c.ss_year(+) and a.ss_asses_pno = c.ss_asses_pno(+) and a.ss_categ = c.ss_categ(+) and a.ss_asses_pno ='" & pno & "' group by A.ss_year,A.ss_asses_pno "
            'qry1 += "and upper(ss_categ)=upper(irc_code) and irc_type='360RL' and ss_year='" & ViewState("FY") & "' group by SS_CATEG,irc_desc"
            Dim re1 As String = String.Empty
            Dim cont1 As Integer = 0
            Dim d1 = GetData(qry1, conHrps)

            If Not d Is Nothing Then
                For g = 0 To d.Rows.Count - 1
                    're += d.Rows(g)("irc_desc") & " - " & d.Rows(g)("Completed") & ", "
                    cont = cont + Val(d.Rows(g)("Completed"))
                Next
                're = re.Trim
                're = re.TrimEnd(",")
                'lblnor.Text = "Your behaviour score is based on the responses of <b>" & cont & " individuals(" & re & ")</b> <br/><br/> A  total of <b>" & d1.Rows(0)(2)
                lblnor.Text = "Your behaviour score is based on the responses of <b>" & cont & " individuals( Internal stakeholder, Manager/Superior, Peer, People you lead, Self)</b>. "
            Else
                cont = 0
                're = "Internal stakeholder - 0, Manager/Superior - 0, Peer - 0, Subordinates - 0, Self - 0"
                re = " Internal stakeholder, Manager/Superior, Peer, People you lead, Self"
                If Not d1 Is Nothing Then
                    lblnor.Text = "Your behaviour score is based on the responses of <b>" & cont & " individuals(" & re & ")</b>."
                Else
                    lblnor.Text = "Your behaviour score is based on the responses of <b>" & cont & " individuals(" & re & ")</b>."
                End If

            End If
        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub
    Public Sub getScore(pno As String)
        Try
            Dim Q1grade As String = String.Empty
            Dim Q2grade As String = String.Empty
            Dim Q3grade As String = String.Empty
            Dim Q4grade As String = String.Empty
            cntPeer = 0
            cntSub = 0
            cntMgr = 0
            cntSelf = 0
            cntStkholder = 0
            Dim qry As String = String.Empty
            qry = "select decode(upper(ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager',upper('PEER'),'Peers',upper('intsh') "
            qry += ",'Internal stakeholders',upper('ropt'),'People you lead') ss_categ,sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),3) A,"
            qry += "round(sum(ss_q1_b)/count(*),3) B,round(sum(ss_q1_c)/count(*),3) C,round(sum(ss_q1_d)/count(*),3) D"
            qry += ",count(*) No_Records,upper(ss_categ) categ from t_survey_status where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and ss_year='" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP' group by ss_categ"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim scoreTable As New DataTable

            ' Create four typed columns in the DataTable.
            scoreTable.Columns.Add("Category", GetType(String))
            scoreTable.Columns.Add("Q1", GetType(String))
            scoreTable.Columns.Add("Q2", GetType(String))
            scoreTable.Columns.Add("Q3", GetType(String))
            scoreTable.Columns.Add("Q4", GetType(String))


            If Not d Is Nothing Then
                Dim self() As DataRow = d.Select("categ = 'SELF'")

                ' Loop and display.
                For Each SelfRow As DataRow In self
                    If SelfRow(10) = "SELF" Then
                        cntSelf = Val(SelfRow(9))
                    End If
                    Q1grade = GetScoreGrade(SelfRow(5))
                    Q2grade = GetScoreGrade(SelfRow(6))
                    Q3grade = GetScoreGrade(SelfRow(7))
                    Q4grade = GetScoreGrade(SelfRow(8))
                    scoreTable.Rows.Add(SelfRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                Next

                If cntSelf = 0 Then
                    scoreTable.Rows.Add("Self", "NA", "NA", "NA", "NA")
                End If

                Dim manager() As DataRow = d.Select("categ = 'MANGR'")

                ' Loop and display.
                For Each managerRow As DataRow In manager
                    If managerRow(10) = "MANGR" Then
                        cntMgr = Val(managerRow(9))
                    End If
                    Q1grade = GetScoreGrade(managerRow(5))
                    Q2grade = GetScoreGrade(managerRow(6))
                    Q3grade = GetScoreGrade(managerRow(7))
                    Q4grade = GetScoreGrade(managerRow(8))
                    scoreTable.Rows.Add(managerRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                Next

                If cntMgr = 0 Then
                    scoreTable.Rows.Add("Manager", "NA", "NA", "NA", "NA")
                End If


                'Dim Peersub() As DataRow = d.Select("categ = 'PEER' OR categ = 'ROPT'")
                'For Each chkRow As DataRow In Peersub
                '    If chkRow(10) = "PEER" Then
                '        cntPeer = Val(chkRow(9))
                '    End If

                '    If chkRow(10) = "ROPT" Then
                '        cntSub = Val(chkRow(9))
                '    End If
                'Next


                '' Loop and display.
                'If (cntPeer < 3 And cntPeer >= 0) Or (cntSub < 3 And cntSub >= 0) Then
                '    For Each PeersubRow As DataRow In Peersub
                '        If cntPeer = 0 And cntSub = 0 Then
                '            scoreTable.Rows.Add("Peers & Subordinate", "NA", "NA", "NA", "NA")
                '        Else
                '            Dim sql As String
                '            sql = "select sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),2) A,round(sum(ss_q1_b)/count(*),2) B,"
                '            sql += "round(sum(ss_q1_c)/count(*),2) C,round(sum(ss_q1_d)/count(*),2) D,count(*) No_Records from t_survey_status"
                '            sql += " where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and upper(ss_categ) in ('PEER','ROPT') and ss_year='" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP'"
                '            Dim dt3 = GetData(sql, conHrps)
                '            Q1grade = GetScoreGrade(dt3.Rows(0)("A").ToString)
                '            Q2grade = GetScoreGrade(dt3.Rows(0)("B").ToString)
                '            Q3grade = GetScoreGrade(dt3.Rows(0)("C").ToString)
                '            Q4grade = GetScoreGrade(dt3.Rows(0)("D").ToString)
                '            scoreTable.Rows.Add("Peers & Subordinate", Q1grade, Q2grade, Q3grade, Q4grade)
                '        End If
                '    Next
                'End If

                'If cntPeer >= 3 And cntSub >= 3 Then
                Dim Peers() As DataRow = d.Select("categ = 'PEER'")
                For Each PeersRow As DataRow In Peers
                    If PeersRow(10) = "PEER" Then
                        cntPeer = Val(PeersRow(9))
                    End If
                    Q1grade = GetScoreGrade(PeersRow(5))
                    Q2grade = GetScoreGrade(PeersRow(6))
                    Q3grade = GetScoreGrade(PeersRow(7))
                    Q4grade = GetScoreGrade(PeersRow(8))
                    scoreTable.Rows.Add(PeersRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                Next

                If cntPeer = 0 Then
                    scoreTable.Rows.Add("Peers", "NA", "NA", "NA", "NA")
                End If

                Dim Subs() As DataRow = d.Select("categ = 'ROPT'")
                For Each SubsRow As DataRow In Subs
                    If SubsRow(10) = "ROPT" Then
                        cntSub = Val(SubsRow(9))
                    End If
                    Q1grade = GetScoreGrade(SubsRow(5))
                    Q2grade = GetScoreGrade(SubsRow(6))
                    Q3grade = GetScoreGrade(SubsRow(7))
                    Q4grade = GetScoreGrade(SubsRow(8))
                    scoreTable.Rows.Add(SubsRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                Next
                ' End If

                If cntSub = 0 Then
                    scoreTable.Rows.Add("People you lead", "NA", "NA", "NA", "NA")
                End If

                Dim intsh() As DataRow = d.Select("categ = 'INTSH'")

                ' Loop and display.
                For Each intshRow As DataRow In intsh
                    If intshRow(10) = "INTSH" Then
                        cntStkholder = Val(intshRow(9))
                    End If
                    Q1grade = GetScoreGrade(intshRow(5))
                    Q2grade = GetScoreGrade(intshRow(6))
                    Q3grade = GetScoreGrade(intshRow(7))
                    Q4grade = GetScoreGrade(intshRow(8))
                    scoreTable.Rows.Add(intshRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                Next

                If cntStkholder = 0 Then
                    scoreTable.Rows.Add("Internal stakeholders", "NA", "NA", "NA", "NA")
                End If
                'Dim ropt() As DataRow = d.Select("categ = 'ROPT'")

                '' Loop and display.
                'For Each roptRow As DataRow In ropt
                '    Q1grade = GetScoreGrade(roptRow(5))
                '    Q2grade = GetScoreGrade(roptRow(6))
                '    Q3grade = GetScoreGrade(roptRow(7))
                '    Q4grade = GetScoreGrade(roptRow(8))
                '    scoreTable.Rows.Add(roptRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                'Next



                Dim sql1 As String
                sql1 = "select ac.ss_asses_pno, round(sum(a)/count(*),3) A, round(sum(b)/count(*),3) B,round(sum(c)/count(*),3) C,round(sum(d)/count(*),3) D from  (SELECT ss_asses_pno,ss_categ,round(SUM(ss_q1_a) / COUNT(*),3) a,round(SUM(ss_q1_b) / COUNT(*),3) b,round(SUM(ss_q1_c) / COUNT(*),3) c,round(SUM(ss_q1_d) / COUNT(*),3) d, COUNT(*) no_records FROM hrps.t_survey_status WHERE ss_wfl_status = '3' AND ss_year = '" & ViewState("FY") & "' AND ss_status = 'SE' AND ss_del_tag = 'N' AND ss_app_tag = 'AP' AND upper(ss_categ) <> 'SELF'  AND ss_asses_pno = '" & pno & "' GROUP BY ss_asses_pno,ss_categ) ac group by ac.ss_asses_pno "

                Dim dt4 = GetData(sql1, conHrps)
                Q1grade = GetScoreGrade(dt4.Rows(0)("A").ToString)
                Q2grade = GetScoreGrade(dt4.Rows(0)("B").ToString)
                Q3grade = GetScoreGrade(dt4.Rows(0)("C").ToString)
                Q4grade = GetScoreGrade(dt4.Rows(0)("D").ToString)
                scoreTable.Rows.Add("Overall", Q1grade, Q2grade, Q3grade, Q4grade)



                'Dim sql1 As String
                'sql1 = "select sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),2) A,round(sum(ss_q1_b)/count(*),2) B,"
                'sql1 += "round(sum(ss_q1_c)/count(*),2) C,round(sum(ss_q1_d)/count(*),2) D,count(*) No_Records from t_survey_status"
                'sql1 += " where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and upper(ss_categ) <> 'SELF' and ss_year='" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP'"
                'Dim dt4 = GetData(sql1, conHrps)
                'Q1grade = GetScoreGrade(dt4.Rows(0)("A").ToString)
                'Q2grade = GetScoreGrade(dt4.Rows(0)("B").ToString)
                'Q3grade = GetScoreGrade(dt4.Rows(0)("C").ToString)
                'Q4grade = GetScoreGrade(dt4.Rows(0)("D").ToString)
                'scoreTable.Rows.Add("Overall", Q1grade, Q2grade, Q3grade, Q4grade)


                gvScore.DataSource = scoreTable
                gvScore.DataBind()
            Else
                scoreTable.Rows.Add("Self", "NA", "NA", "NA", "NA")
                scoreTable.Rows.Add("Manager", "NA", "NA", "NA", "NA")
                scoreTable.Rows.Add("Peers", "NA", "NA", "NA", "NA")
                scoreTable.Rows.Add("People you lead", "NA", "NA", "NA", "NA")
                scoreTable.Rows.Add("Internal stakeholders", "NA", "NA", "NA", "NA")
                scoreTable.Rows.Add("Overall", "NA", "NA", "NA", "NA")
                gvScore.DataSource = scoreTable
                gvScore.DataBind()

            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString())
        End Try
    End Sub
    Public Function GetScoreGrade(scoreval As String) As String
        Try
            Dim grade As String = ""
            If Val(scoreval) <= Val(1.6) Then
                grade = "Unacceptable"
            ElseIf Val(scoreval) <= Val(2.6) And Val(scoreval) > Val(1.6) Then
                grade = "Acceptable"
            ElseIf Val(scoreval) <= Val(3) And Val(scoreval) > Val(2.6) Then
                grade = "Gold Standard"
            End If
            Return grade
        Catch ex As Exception

        End Try

    End Function
End Class
