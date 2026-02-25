Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Partial Class FeedbackSurveyRpt
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim cntPeer As Integer
    Dim cntSub As Integer
    Dim cntSelf As Integer
    Dim cntMgr As Integer
    Dim cntStkholder As Integer

    Private Sub FeedbackSurveyRpt(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("perno") <> Nothing Then
                Dim requestPerno As String = Request.QueryString("perno")
                GetFy()
                getPnoDetails(requestPerno)
                getresCoupt(requestPerno)
                getScore(requestPerno)
                getQ2AresCoupt(requestPerno)
                getQ2BresCoupt(requestPerno)
            Else
                GetFy()
                getPnoDetails("119046")
                getresCoupt("119046")
                getScore("119046")
                getQ2AresCoupt("119046")
                getQ2BresCoupt("119046")
            End If

            'bgImg.ImageUrl = Server.MapPath(".") + "/Images/Feedback360.JPG"
            'imgLogo.ImageUrl = Server.MapPath(".") + "/Images/logo.JPG"
            'Response.ContentType = "application/pdf"
            'Response.AddHeader("content-disposition", "attachment;filename=UserDetails.pdf")
            'Response.Cache.SetCacheability(HttpCacheability.NoCache)
            'Dim jpg1 As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Server.MapPath(".") + "/Images/Feedback360.JPG")
            'Dim jpg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(Server.MapPath(".") + "/Images/logo.JPG")
            'Dim sw As New StringWriter()
            'Dim hw As New HtmlTextWriter(sw)
            'Me.Page.RenderControl(hw)
            'Dim sr As New StringReader(sw.ToString())
            'Dim pdfDoc As New Document(PageSize.A4, 10.0F, 100.0F, 10.0F, 10.0F)
            'Dim htmlparser As New HTMLWorker(pdfDoc)
            'PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
            'pdfDoc.Open()
            'pdfDoc.Add(jpg)
            'pdfDoc.Add(jpg1)
            'htmlparser.Parse(sr)
            'pdfDoc.Close()
            'Response.Write(pdfDoc)
            'Response.[End]()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "KeyGenericMessageModal", "screenshot('02-03-2021');", True)
        'ASPXToPDF1.RenderAsPDF()
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
                lblQAOtrRespond.Text = "<ul style='list-style-type:disc;'>"
                For i = 0 To dt1.Rows.Count - 1
                    lblQAOtrRespond.Text += "<li>" + dt1.Rows(i)("ss_q2_a").ToString + "</li>"
                Next
                lblQAOtrRespond.Text += "</ul>"
            End If

            If Not dt Is Nothing Then
                lblQASelf.Text = "<ul style='list-style-type:disc;'>"
                For g = 0 To dt.Rows.Count - 1
                    lblQASelf.Text += "<li>" + dt.Rows(g)("ss_q2_a").ToString + "</li>"
                Next
                lblQASelf.Text += "</ul>"
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
                lblQBOtrRespond.Text = "<ul style='list-style-type:disc;'>"
                For i = 0 To dt1.Rows.Count - 1
                    lblQBOtrRespond.Text += "<li>" + dt1.Rows(i)("ss_q2_b").ToString + "</li>"
                Next
                lblQBOtrRespond.Text += "</ul>"
            End If

            If Not dt Is Nothing Then
                lblQBSelf.Text = "<ul  style='list-style-type:disc;'>"
                For g = 0 To dt.Rows.Count - 1
                    lblQBSelf.Text += "<li>" + dt.Rows(g)("ss_q2_b").ToString + "</li>"
                Next
                lblQBSelf.Text += "</ul>"
            End If


        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub
    Public Sub getPnoDetails(pno As String)
        Dim qry As String = String.Empty
        qry = "select ema_ename,ema_dept_desc from tips.t_empl_all where ema_perno='" & pno & "'"
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
            qry = "select SS_CATEG,irc_desc ,count(SS_CATEG) from t_survey_status,t_ir_codes  where ss_asses_pno='" & pno & "' and ss_app_tag='AP' "
            qry += "and upper(ss_categ)=upper(irc_code) and irc_type='360RL' and ss_year='" & ViewState("FY") & "' group by SS_CATEG,irc_desc"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim qry1 As String = String.Empty
            qry1 = "select ss_pno from t_survey_status  where ss_asses_pno ='" & pno & "'  and ss_wfl_status='3' and ss_year='" & ViewState("FY") & "' "
            'qry1 += "and upper(ss_categ)=upper(irc_code) and irc_type='360RL' and ss_year='" & ViewState("FY") & "' group by SS_CATEG,irc_desc"
            Dim re1 As String = String.Empty
            Dim cont1 As Integer = 0
            Dim d1 = GetData(qry1, conHrps)

            If Not d Is Nothing Then
                For g = 0 To d.Rows.Count - 1
                    re += d.Rows(g)(1) & " - " & d.Rows(g)(2) & ", "
                    cont = cont + d.Rows(g)(2)
                Next
                re = re.Trim
                re = re.TrimEnd(",")
                lblnor.Text = "Your behaviour score is based on the responses of <b>" & cont & " individuals(" & re & ")</b> <br/><br/> A  total of <b>" & cont
                lblnor.Text += "</b> surveys were distributed. <b>" & d1.Rows.Count & "</b> surveys were completed and have been included in this feedback report."
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
            qry += ",'Internal stakeholders',upper('ropt'),'Subordinates') ss_categ,sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),2) A,"
            qry += "round(sum(ss_q1_b)/count(*),2) B,round(sum(ss_q1_c)/count(*),2) C,round(sum(ss_q1_d)/count(*),2) D"
            qry += ",count(*) No_Records,upper(ss_categ) categ from t_survey_status where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and ss_year='" & ViewState("FY") & "' group by ss_categ"
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


                Dim Peersub() As DataRow = d.Select("categ = 'PEER' OR categ = 'ROPT'")
                For Each chkRow As DataRow In Peersub
                    If chkRow(10) = "PEER" Then
                        cntPeer = Val(chkRow(9))
                    End If

                    If chkRow(10) = "ROPT" Then
                        cntSub = Val(chkRow(9))
                    End If
                Next


                ' Loop and display.
                For Each PeersubRow As DataRow In Peersub
                    If (cntPeer < 3 And cntPeer >= 0) Or (cntSub < 3 And cntSub >= 0) Then
                        'Q1grade = GetScoreGrade(PeersubRow(5))
                        'Q2grade = GetScoreGrade(PeersubRow(6))
                        'Q3grade = GetScoreGrade(PeersubRow(7))
                        'Q4grade = GetScoreGrade(PeersubRow(8))
                        If cntPeer = 0 And cntSub = 0 Then
                            scoreTable.Rows.Add("Peers & Subordinate", "NA", "NA", "NA", "NA")
                        Else
                            Dim sql As String
                            sql = "select sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),2) A,round(sum(ss_q1_b)/count(*),2) B,"
                            sql += "round(sum(ss_q1_c)/count(*),2) C,round(sum(ss_q1_d)/count(*),2) D,count(*) No_Records from t_survey_status"
                            sql += " where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and upper(ss_categ) in ('PEER','ROPT') and ss_year='" & ViewState("FY") & "'"
                            Dim dt3 = GetData(sql, conHrps)
                            Q1grade = GetScoreGrade(dt3.Rows(0)("A").ToString)
                            Q2grade = GetScoreGrade(dt3.Rows(0)("B").ToString)
                            Q3grade = GetScoreGrade(dt3.Rows(0)("C").ToString)
                            Q4grade = GetScoreGrade(dt3.Rows(0)("D").ToString)
                            scoreTable.Rows.Add("Peers & Subordinate", Q1grade, Q2grade, Q3grade, Q4grade)
                        End If
                    End If
                Next

                If cntPeer >= 3 And cntSub >= 3 Then
                    Dim Peers() As DataRow = d.Select("categ = 'PEER'")
                    For Each PeersRow As DataRow In Peers
                        Q1grade = GetScoreGrade(PeersRow(5))
                        Q2grade = GetScoreGrade(PeersRow(6))
                        Q3grade = GetScoreGrade(PeersRow(7))
                        Q4grade = GetScoreGrade(PeersRow(8))
                        scoreTable.Rows.Add(PeersRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                    Next

                    Dim Subs() As DataRow = d.Select("categ = 'ROPT'")
                    For Each SubsRow As DataRow In Subs
                        Q1grade = GetScoreGrade(SubsRow(5))
                        Q2grade = GetScoreGrade(SubsRow(6))
                        Q3grade = GetScoreGrade(SubsRow(7))
                        Q4grade = GetScoreGrade(SubsRow(8))
                        scoreTable.Rows.Add(SubsRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                    Next
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
                sql1 = "select sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),2) A,round(sum(ss_q1_b)/count(*),2) B,"
                sql1 += "round(sum(ss_q1_c)/count(*),2) C,round(sum(ss_q1_d)/count(*),2) D,count(*) No_Records from t_survey_status"
                sql1 += " where ss_asses_pno ='" & pno & "' and ss_wfl_status ='3' and upper(ss_categ) <> 'SELF' and ss_year='" & ViewState("FY") & "'"
                Dim dt4 = GetData(sql1, conHrps)
                Q1grade = GetScoreGrade(dt4.Rows(0)("A").ToString)
                Q2grade = GetScoreGrade(dt4.Rows(0)("B").ToString)
                Q3grade = GetScoreGrade(dt4.Rows(0)("C").ToString)
                Q4grade = GetScoreGrade(dt4.Rows(0)("D").ToString)
                scoreTable.Rows.Add("Overall", Q1grade, Q2grade, Q3grade, Q4grade)

                If scoreTable.Rows.Count > 0 Then
                    lblTableScore.Text = "<table style='border: 1px solid black; border-collapse: collapse; width:50%;'><tr><th style='border: 1px solid black; border-collapse: collapse;'></th><th style='border: 1px solid black; border-collapse: collapse;'>Accountability</th><th style='border: 1px solid black; border-collapse: collapse;'>Collaboration</th><th style='border: 1px solid black; border-collapse: collapse;'>Responsiveness</th><th style='border: 1px solid black; border-collapse: collapse;'>People Development</th></tr>"
                    For i As Integer = 0 To scoreTable.Rows.Count - 1
                        lblTableScore.Text += "<tr><td style='border: 1px solid black; border-collapse: collapse;'>" + scoreTable.Rows(i)("Category").ToString + "</td><td style='border: 1px solid black; border-collapse: collapse;'>" + scoreTable.Rows(i)("Q1").ToString + "</td><td style='border: 1px solid black; border-collapse: collapse;'>" + scoreTable.Rows(i)("Q2").ToString + "</td><td style='border: 1px solid black; border-collapse: collapse;'>" + scoreTable.Rows(i)("Q3").ToString + "</td><td style='border: 1px solid black; border-collapse: collapse;'>" + scoreTable.Rows(i)("Q4").ToString + "</td></tr>"
                    Next
                    lblTableScore.Text += "<table>"
                End If
                'gvScore.DataSource = scoreTable
                'gvScore.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString())
        End Try
    End Sub
    Public Function GetScoreGrade(scoreval As String) As String
        Try
            Dim grade As String = ""
            If Val(scoreval) < Val(1.5) Then
                grade = "Unacceptable"
            ElseIf Val(scoreval) < Val(2.5) And Val(scoreval) >= Val(1.5) Then
                grade = "Acceptable"
            ElseIf Val(scoreval) <= Val(3.5) And Val(scoreval) >= Val(2.5) Then
                grade = "Gold Standard"
            End If
            Return grade
        Catch ex As Exception

        End Try

    End Function
End Class
