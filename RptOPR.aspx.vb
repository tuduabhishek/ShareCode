Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Partial Class RPTOPR
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Private Sub Rpt_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetFy()
        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        ' Dim perno = Request.QueryString("pno")
        'perno = "162523"
        Dim vUserFullName As String = Page.User.Identity.Name   '155710'

        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If
        'strUserID = "162523"
        Dim f = GetData("select ema_perno, ema_ename, ema_desgn_desc from TIPS.t_empl_all  where ema_perno='" & strUserID & "'", conHrps)

        If f.Rows.Count > 0 Then

            lblReceiptNm.Text = f.Rows(0)(1).ToString()
                ' lblReceiptNm.Text = " Qualitative comments for " & f.Rows(0)(1).ToString()
                lblall.Text = "Overall report for " & f.Rows(0)(1).ToString()
                lblDesignation.Text = f.Rows(0)(2).ToString()
                lbloverall.Text = "Overall report for " & f.Rows(0)(1).ToString()
                Label119.Text = "Overall report for " & f.Rows(0)(1).ToString()
                lblnmm.Text = " Qualitative comments for " & f.Rows(0)(1).ToString()
                Label66.Text = " Qualitative comments for " & f.Rows(0)(1).ToString()

            getresCoupt(strUserID)
            Responsiveness(strUserID)
            'getCollaboration(perno)
            getaccountibility(strUserID)
            Collaboration(strUserID)
            TeamBuilding(strUserID)
            displayAns(strUserID)

            If Checkdownload(strUserID) Then
                Response.Write("<center> <b><I> Report shuold be downloaded at once.. </b></I></center>")
                Me.Page.Visible = False
                Exit Sub
            End If
        Else
            Response.Write("<center> <b><I> Report not downloaded for you </b></I></center>")
            Me.Page.Visible = False
            Exit Sub
        End If


        ' ClientScript.RegisterStartupScript(Me.[GetType](), "onclick", "<script language=javascript>window.open('FeedbackSurveyRpt_OPR.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>")
    End Sub

    Public Function Checkdownload(pno As String) As Boolean
        Dim flag As Boolean
        Try
            Dim qry = "select SS_FLAG3 from t_survey_status where SS_ASSES_PNO ='" & pno & "' and SS_YEAR='" & ViewState("FY") & "' and SS_WFL_STATUS ='3'"
            qry += " and nvl(SS_FLAG3,'N') = 'Y'"
            Dim gd = GetData(qry, conHrps)

            If gd.Rows.Count > 0 Then
                flag = True
            Else
                Dim cmd As New OracleCommand()
                cmd.CommandText = " update t_survey_status set SS_FLAG3='Y' where SS_ASSES_PNO ='" & pno & "' and SS_YEAR='" & ViewState("FY") & "' and SS_WFL_STATUS ='3' "
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                cmd.Connection = conHrps
                cmd.ExecuteNonQuery()
                flag = False
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return flag
    End Function

    Public Function GetData(qry As String, con As OracleConnection) As DataTable

        Try
            Dim comd As New OracleDataAdapter(qry, con)
            Dim data As New DataTable()
            comd.Fill(data)
            Return data
        Catch ex As Exception
            Dim g = ex.ToString
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
    Public Sub getresCoupt(pno As String)
        Try
            Dim qry As String = String.Empty
            qry = "select irc_desc, SS_CATEG,count(SS_CATEG)  from hrps.t_survey_status, hrps.t_ir_codes where ss_asses_pno='" & pno & "'  "
            qry += "and upper(irc_code) = Upper(ss_categ) and irc_type='360RL' and ss_year='" & ViewState("FY") & "' and SS_APP_TAG ='AP'  and SS_Q2_A is not null group by SS_CATEG,irc_desc"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim df = GetData("select count(ss_pno) from t_survey_status where ss_asses_pno='" & pno & "' and ss_year='" & ViewState("FY") & "' and SS_APP_TAG ='AP'", conHrps)

            Dim qry1 As String = String.Empty
            qry1 = "select count( distinct SS_PNO) from hrps.t_survey_response where ss_asses_pno ='" & pno & "'  "
            Dim d1 = GetData(qry1, conHrps)

            If Not d Is Nothing Then
                For g = 0 To d.Rows.Count - 1
                    re += d.Rows(g)(0) & " - " & d.Rows(g)(2) & ", "
                    cont = cont + d.Rows(g)(2)
                Next
                re = re.Trim
                re = re.TrimEnd(",")
                lbloverscore.Text = "Your behaviour score is based on the responses of <b>" & cont & " individuals(" & re & ")</b> <br/><br/> A  total of <b>" & df.Rows(0)(0)
                lbloverscore.Text += "</b> surveys were distributed. <b>" & d1.Rows(0)(0) & "</b> surveys were completed and have been included in this feedback report."
            End If
        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub

    Public Sub getCollaboration(pno As String)
        Try
            Dim clinst As String = String.Empty
            Dim clmgr As String = String.Empty
            Dim cloverall As String = String.Empty
            Dim clself As String = String.Empty
            Dim resinst As String = String.Empty
            Dim resmgr As String = String.Empty
            Dim resoverall As String = String.Empty
            Dim resself As String = String.Empty
            Dim accinst As String = String.Empty
            Dim accmgr As String = String.Empty
            Dim accself As String = String.Empty
            Dim accoverall As String = String.Empty
            Dim tbinst As String = String.Empty
            Dim tbmgr As String = String.Empty
            Dim tbself As String = String.Empty
            Dim tboverall As String = String.Empty
            '---------------------------------- Collaboration-------------------
            Dim str As String = String.Empty
            str = "select A.SS_PNO, round((avg(A.ss_qoptn)),2) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.ss_asses_pno ='" & pno & "' "
            str += " AND A.SS_QCODE in('I42','I46','I410') And A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d = GetData(str, conHrps)

            If d.Rows.Count > 0 Then
                Dim dr() As DataRow = d.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p As DataRow In dr
                    k += 1
                    i += p(1)
                Next
                Label6.Text = Math.Round(((i) / k), 2)

                Dim dr1() As DataRow = d.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label5.Text = Math.Round((i1 / k1), 2)

                Dim dr2() As DataRow = d.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                Label4.Text = Math.Round((i2 / k2), 2)


            End If
            Label7.Text = (Convert.ToDouble(Label4.Text) + Convert.ToDouble(Label5.Text) + Convert.ToDouble(Label6.Text))
            Label7.Text = Math.Round(Convert.ToDouble((Label7.Text) / 3), 2)

            Label4.Text = checkAccespt1(Label4.Text)
            Label6.Text = checkAccespt1(Label6.Text)
            Label5.Text = checkAccespt1(Label5.Text)
            Label7.Text = checkAccespt1(Label7.Text)
            ' Label7.Text = checkAccespt(Label84.Text)
            lblcollaboration.Text = Left(Label7.Text, 1)

            '------------------------------------------------------------ Responsiveness-------

            Dim str1 As String = String.Empty
            str1 = "select A.SS_PNO, round((avg(A.ss_qoptn)),2) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.ss_asses_pno ='" & pno & "' "
            str1 += " AND A.SS_QCODE in('I43','I47','I411') And A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d1 = GetData(str1, conHrps)

            If d.Rows.Count > 0 Then
                Dim dr() As DataRow = d1.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p1 As DataRow In dr
                    k += 1
                    i += p1(1)
                Next
                Label10.Text = Math.Round((i / k), 2)

                Dim dr1() As DataRow = d1.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label9.Text = Math.Round((i1 / k1), 2)

                Dim dr2() As DataRow = d1.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                Label8.Text = Math.Round((i2 / k2), 2)


            End If
            Label11.Text = (Convert.ToDouble(Label8.Text) + Convert.ToDouble(Label9.Text) + Convert.ToDouble(Label10.Text))
            Label11.Text = Math.Round(Convert.ToDouble((Label11.Text) / 3), 2)

            Label8.Text = checkAccespt1(Label8.Text)
            Label9.Text = checkAccespt1(Label9.Text)
            Label10.Text = checkAccespt1(Label10.Text)
            Label11.Text = checkAccespt1(Label11.Text)
            ' Label85.Text = checkAccespt(Label85.Text)
            lblresponse.Text = Left(Label11.Text, 1)
            '------------------------------------------------------------ Team Building---------

            Dim str2 As String = String.Empty
            str2 = "select A.SS_PNO, round((avg(A.ss_qoptn)),2) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.ss_asses_pno ='" & pno & "' "
            str2 += " AND A.SS_QCODE in('I44','I48','I412') And A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d2 = GetData(str2, conHrps)

            If d2.Rows.Count > 0 Then
                Dim dr() As DataRow = d2.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p1 As DataRow In dr
                    k += 1
                    i += p1(1)
                Next
                Label14.Text = Math.Round((i / k), 2)

                Dim dr1() As DataRow = d2.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label13.Text = Math.Round((i1 / k1), 2)

                Dim dr2() As DataRow = d2.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                Label12.Text = Math.Round((i2 / k2), 2)


            End If
            Label15.Text = (Convert.ToDouble(Label14.Text) + Convert.ToDouble(Label13.Text) + Convert.ToDouble(Label12.Text))
            Label15.Text = Math.Round(Convert.ToDouble((Label15.Text / 3)), 2)

            Label12.Text = checkAccespt1(Label12.Text)
            Label13.Text = checkAccespt1(Label13.Text)
            Label14.Text = checkAccespt1(Label14.Text)
            Label15.Text = checkAccespt1(Label15.Text)
            ' Label86.Text = checkAccespt(Label86.Text)

            lblpeople.Text = Left(Label15.Text, 1)
            '------------------------------------------------------------ Accountability

            Dim str3 As String = String.Empty
            str3 = "select A.SS_PNO, round((avg(A.ss_qoptn)),2) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.ss_asses_pno ='" & pno & "' "
            str3 += " AND A.SS_QCODE in('I41','I45','I49') And A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d3 = GetData(str3, conHrps)

            If d3.Rows.Count > 0 Then
                Dim dr() As DataRow = d3.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p1 As DataRow In dr
                    k += 1
                    i += p1(1)
                Next
                Label2.Text = Math.Round((i / k), 2)

                Dim dr1() As DataRow = d3.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label1.Text = Math.Round((i1 / k1), 2)

                Dim dr2() As DataRow = d3.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                lblaccself.Text = Math.Round((i2 / k2), 2)


            End If
            Label3.Text = (Convert.ToDouble(lblaccself.Text) + Convert.ToDouble(Label1.Text) + Convert.ToDouble(Label2.Text))
            Label3.Text = Math.Round(Convert.ToDouble((Label3.Text) / 3), 2)

            Label2.Text = checkAccespt1(Label2.Text)
            Label1.Text = checkAccespt1(Label1.Text)
            lblaccself.Text = checkAccespt1(lblaccself.Text)
            Label3.Text = checkAccespt1(Label3.Text)
            lblaccountability.Text = Left(Label3.Text, 1)
            ' Label83.Text = checkAccespt(Label83.Text)
            Dim catg As New Label
            Dim catg1 As New Label
            Dim catg2 As New Label
            Dim catg3 As New Label
            catg.Text = ""
            catg1.Text = ""
            catg2.Text = ""
            catg3.Text = ""

            Dim dtscore As New DataTable
            dtscore.Columns.Add("categ", GetType(String))
            dtscore.Columns.Add("Self", GetType(String))
            dtscore.Columns.Add("Manager", GetType(String))
            dtscore.Columns.Add("Internal Stakeholder", GetType(String))
            dtscore.Columns.Add("Overall", GetType(String))

            dtscore.Rows.Add(catg.Text, accself, accmgr, accinst, accoverall)
            dtscore.Rows.Add(catg1.Text, clself, clmgr, clinst, cloverall)
            dtscore.Rows.Add(catg2.Text, resself, resmgr, resinst, resoverall)
            dtscore.Rows.Add(catg3.Text, tbself, tbmgr, tbinst, tboverall)

            dtscore.AcceptChanges()
            'grdscore.DataSource = dtscore
            'grdscore.DataBind()
        Catch ex As Exception
            Dim d = ex.ToString
        End Try
    End Sub

    Public Sub getaccountibility(pno As String)
        Try
            lblAccSelf1.Text = Val(0)
            lblAccMangr.Text = Val(0)
            lblAccIntsh.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',sum(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',sum(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where f.ss_asses_pno ='" & pno & "' "
            qry += " and d.SS_QCODE in ('I41','I45','I49','I51','I55','I59','I61','I65','I69') and d.ss_pno = f.ss_pno  and ss_flag='I' and d.ss_qcode = qs.ss_qcode  "
            qry += " and d.ss_asses_pno = f.ss_asses_pno group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I41','I51','I61')")
            Dim intsh As Double = 0
            Dim cnt As Integer = 0
            For Each hj In dr
                cnt += 1
                intsh += hj(4)
                Label32.Text = hj(1)
            Next
            Dim df As String = Math.Round((intsh / cnt), 2)
            lblAccIntsh.Text = Val(lblAccIntsh.Text) + Val(df)
            Label18.Text = checkAccespt(df)

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I41','I51','I61')")
            Dim mgr As Double = 0
            Dim cnt1 As Integer = 0
            For Each hj In dr1
                cnt1 += 1
                mgr += hj(5)
                Label32.Text = hj(1)
            Next
            Dim df1 As String = Math.Round((mgr / cnt1), 2)
            lblAccMangr.Text = Val(lblAccMangr.Text) + Val(df1)
            Label17.Text = checkAccespt(df1)

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I41','I51','I61')")
            Dim mg2 As Double = 0
            Dim cnt2 As Integer = 0
            For Each hj In dr2
                cnt2 += 1
                mg2 += hj(6)
                Label32.Text = hj(1)
            Next
            Dim df2 As String = Math.Round((mg2 / cnt2), 2)
            lblAccSelf1.Text = Val(lblAccSelf1.Text) + Val(df2)
            Label16.Text = checkAccespt(df2)


            Label19.Text = checkAccespt(Math.Round(((mgr / cnt1) + (intsh / cnt)) / 2, 2))

            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I45','I55','I65')")
            Dim intsh3 As Double = 0
            Dim cnt3 As Integer = 0
            For Each hj In dr3
                cnt3 += 1
                intsh3 += hj(4)
                Label33.Text = hj(1)
            Next
            Dim df3 As String = Math.Round((intsh3 / cnt3), 2)
            lblAccIntsh.Text = Val(lblAccIntsh.Text) + Val(df3)
            Label22.Text = checkAccespt(df3)

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I45','I55','I65')")
            Dim mgr4 As Double = 0
            Dim cnt4 As Integer = 0
            For Each hj In dr4
                cnt4 += 1
                mgr4 += hj(5)
                Label33.Text = hj(1)
            Next
            Dim df4 As String = Math.Round((mgr4 / cnt4), 2)
            lblAccMangr.Text = Val(lblAccMangr.Text) + Val(df4)
            Label21.Text = checkAccespt(df4)

            Dim dr5() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I45','I55','I65')")
            Dim mg5 As Double = 0
            Dim cnt5 As Integer = 0
            For Each hj In dr5
                cnt5 += 1
                mg5 += hj(6)
                Label33.Text = hj(1)
            Next
            Dim df5 As String = Math.Round((mg5 / cnt5), 2)
            lblAccSelf1.Text = Val(lblAccSelf1.Text) + Val(df5)
            Label20.Text = checkAccespt(df5)



            Label23.Text = checkAccespt(Math.Round(((mgr4 / cnt4) + (intsh3 / cnt3)) / 2, 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I49','I59','I69')")
            Dim intsh8 As Double = 0
            Dim cnt8 As Integer = 0
            For Each hj In dr8
                cnt8 += 1
                intsh8 += hj(4)
                Label34.Text = hj(1)
            Next
            Dim df8 As String = Math.Round((intsh8 / cnt8), 2)
            lblAccIntsh.Text = Val(lblAccIntsh.Text) + Val(df8)
            Label26.Text = checkAccespt(df8)

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I49','I59','I69')")
            Dim mgr9 As Double = 0
            Dim cnt9 As Integer = 0
            For Each hj In dr9
                cnt9 += 1
                mgr9 += hj(5)
                Label34.Text = hj(1)
            Next
            Dim df9 As String = Math.Round((mgr9 / cnt9), 2)
            lblAccMangr.Text = Val(lblAccMangr.Text) + Val(df9)
            Label25.Text = checkAccespt(df9)

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I49','I59','I69')")
            Dim mg10 As Double = 0
            Dim cnt10 As Integer = 0
            For Each hj In dr10
                cnt10 += 1
                mg10 += hj(6)
                Label34.Text = hj(1)
            Next
            Dim df10 As String = Math.Round((mg10 / cnt10), 2)
            lblAccSelf1.Text = Val(lblAccSelf1.Text) + Val(df10)
            Label24.Text = checkAccespt(df10)


            Label27.Text = checkAccespt(Math.Round(((mgr9 / cnt9) + (intsh8 / cnt8)) / 2, 2))


            lblAccMangr.Text = Math.Round(Val(lblAccMangr.Text) / 3, 2)
            lblAccIntsh.Text = Math.Round(Val(lblAccIntsh.Text) / 3, 2)
            lblAccSelf1.Text = Math.Round(Val(lblAccSelf1.Text) / 3, 2)
            Label1.Text = checkAccespt1(lblAccMangr.Text)
            Label2.Text = checkAccespt1(lblAccIntsh.Text)
            lblaccself.Text = checkAccespt1(lblAccSelf1.Text)

            Label3.Text = checkAccespt1(Math.Round(((Val(lblAccMangr.Text) + Val(lblAccIntsh.Text)) / 2), 2))
            lblaccountability.Text = checkAccespt(Math.Round(((Val(lblAccMangr.Text) + Val(lblAccIntsh.Text)) / 2), 2))
            '-----------------------------------------------------------------------------------
            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I413','I513','I613')")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)
                'Label35.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            'Label30.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I413','I513','I613')")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)
                ' Label35.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            'Label29.Text = checkAccespt(df91)

            Dim dr110() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I413','I513','I613')")
            Dim mg110 As Double = 0
            Dim cnt110 As Integer = 0
            For Each hj In dr110
                cnt110 += 1
                mg110 += hj(6)
                ' Label35.Text = hj(1)
            Next
            Dim df110 As String = Math.Round((mg110 / cnt110), 2)
            ' Label28.Text = checkAccespt(df110)

            'Dim dr190() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I413'")
            'Dim mgr190 As Double = 0
            'Dim cnt190 As Integer = 0
            'For Each hj In dr190
            '    cnt190 += 1
            '    mgr190 += hj(5)
            '    Label35.Text = hj(1)
            'Next
            'Dim df190 As String = (mgr190 / cnt190)
            'Label100.Text = checkAccespt(df190)

            'Label31.Text = checkAccespt(((mg110 / cnt110) + (mgr91 / cnt91) + (intsh18 / cnt18)))



        Catch ex As Exception
            Dim gs = ex.ToString()
        End Try
    End Sub

    Public Sub Collaboration(pno As String)
        Try
            lblCooIntsh.Text = Val(0)
            lblCooMangr.Text = Val(0)
            lblCooSelf.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',sum(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',sum(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where f.ss_asses_pno ='" & pno & "' "
            qry += " and d.SS_QCODE in ('I42','I46','I410','I52','I56','I510','I62','I66','I610') and d.ss_pno = f.ss_pno  and ss_flag='I' and d.ss_qcode = qs.ss_qcode  "
            qry += " and d.ss_asses_pno = f.ss_asses_pno group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I42','I52','I62')")
            Dim intsh As Double = 0
            Dim cnt As Integer = 0
            For Each hj In dr
                cnt += 1
                intsh += hj(4)
                Label36.Text = hj(1)
            Next
            Dim df As String = Math.Round((intsh / cnt), 2)
            lblCooIntsh.Text = Val(lblCooIntsh.Text) + Val(df)
            Label39.Text = checkAccespt(df)

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I42','I52','I62')")
            Dim mgr As Double = 0
            Dim cnt1 As Integer = 0
            For Each hj In dr1
                cnt1 += 1
                mgr += hj(5)
                Label36.Text = hj(1)
            Next
            Dim df1 As String = Math.Round((mgr / cnt1), 2)
            lblCooMangr.Text = Val(lblCooMangr.Text) + Val(df1)
            Label38.Text = checkAccespt(df1)

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE  in('I42','I52','I62')")
            Dim mg2 As Double = 0
            Dim cnt2 As Integer = 0
            For Each hj In dr2
                cnt2 += 1
                mg2 += hj(6)
                Label36.Text = hj(1)
            Next
            Dim df2 As String = Math.Round((mg2 / cnt2), 2)
            lblCooSelf.Text = Val(lblCooSelf.Text) + Val(df2)
            Label37.Text = checkAccespt(df2)

            'Dim dr13() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I42'")
            'Dim mg13 As Double = 0
            'Dim cnt13 As Integer = 0
            'For Each hj In dr2
            '    cnt13 += 1
            '    mg13 += hj(6)
            '    Label36.Text = hj(1)
            'Next
            'Dim df13 As String = (mg13 / cnt13)
            'Label93.Text = checkAccespt(df13)

            Label40.Text = checkAccespt(Math.Round(((mgr / cnt1) + (intsh / cnt)) / 2, 2))

            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE  in('I46','I56','I66')")
            Dim intsh3 As Double = 0
            Dim cnt3 As Integer = 0
            For Each hj In dr3
                cnt3 += 1
                intsh3 += hj(4)
                Label41.Text = hj(1)
            Next
            Dim df3 As String = Math.Round((intsh3 / cnt3), 2)
            lblCooIntsh.Text = Val(lblCooIntsh.Text) + Val(df3)
            Label44.Text = checkAccespt(df3)

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE  in('I46','I56','I66')")
            Dim mgr4 As Double = 0
            Dim cnt4 As Integer = 0
            For Each hj In dr4
                cnt4 += 1
                mgr4 += hj(5)
                Label41.Text = hj(1)
            Next
            Dim df4 As String = Math.Round((mgr4 / cnt4), 2)
            lblCooMangr.Text = Val(lblCooMangr.Text) + Val(df4)
            Label43.Text = checkAccespt(df4)

            Dim dr5() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE  in('I46','I56','I66')")
            Dim mg5 As Double = 0
            Dim cnt5 As Integer = 0
            For Each hj In dr5
                cnt5 += 1
                mg5 += hj(6)
                Label41.Text = hj(1)
            Next
            Dim df5 As String = Math.Round((mg5 / cnt5), 2)
            lblCooSelf.Text = Val(lblCooSelf.Text) + Val(df5)
            Label42.Text = checkAccespt(df5)

            'Dim dr15() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I46'")
            'Dim mg15 As Double = 0
            'Dim cnt15 As Integer = 0
            'For Each hj In dr15
            '    cnt15 += 1
            '    mg15 += hj(6)
            '    Label41.Text = hj(1)
            'Next
            'Dim df15 As String = (mg15 / cnt15)
            'Label94.Text = checkAccespt(df15)

            Label45.Text = checkAccespt(Math.Round(((mgr4 / cnt4) + (intsh3 / cnt3)) / 2, 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I410','I510','I610')")
            Dim intsh8 As Double = 0
            Dim cnt8 As Integer = 0
            For Each hj In dr8
                cnt8 += 1
                intsh8 += hj(4)

                Label46.Text = hj(1)
            Next
            Dim df8 As String = Math.Round((intsh8 / cnt8), 2)
            lblCooIntsh.Text = Val(lblCooIntsh.Text) + Val(df8)
            Label49.Text = checkAccespt(df8)

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I410','I510','I610')")
            Dim mgr9 As Double = 0
            Dim cnt9 As Integer = 0
            For Each hj In dr9
                cnt9 += 1
                mgr9 += hj(5)

                Label46.Text = hj(1)
            Next
            Dim df9 As String = Math.Round((mgr9 / cnt9), 2)
            lblCooMangr.Text = Val(lblCooMangr.Text) + Val(df9)
            Label48.Text = checkAccespt(df9)

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I410','I510','I610')")
            Dim mg10 As Double = 0
            Dim cnt10 As Integer = 0
            For Each hj In dr10
                cnt10 += 1
                mg10 += hj(6)
                Label46.Text = hj(1)
            Next
            Dim df10 As String = Math.Round((mg10 / cnt10), 2)
            lblCooSelf.Text = Val(lblCooSelf.Text) + Val(df10)
            Label47.Text = checkAccespt(df10)

            'Dim dr110() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I410'")
            'Dim mg110 As Double = 0
            'Dim cnt110 As Integer = 0
            'For Each hj In dr110
            '    cnt110 += 1
            '    mg110 += hj(6)
            '    Label46.Text = hj(1)
            'Next
            'Dim df110 As String = (mg110 / cnt110)
            'Label95.Text = checkAccespt(df110)

            Label50.Text = checkAccespt(Math.Round(((mgr9 / cnt9) + (intsh8 / cnt8)) / 2, 2))

            lblCooMangr.Text = Math.Round(Val(lblCooMangr.Text) / 3, 2)
            lblCooIntsh.Text = Math.Round(Val(lblCooIntsh.Text) / 3, 2)
            lblCooSelf.Text = Math.Round(Val(lblCooSelf.Text) / 3, 2)
            Label5.Text = checkAccespt1(lblCooMangr.Text)
            Label6.Text = checkAccespt1(lblCooIntsh.Text)
            Label4.Text = checkAccespt1(lblCooSelf.Text)

            Label7.Text = checkAccespt1(Math.Round(((Val(lblCooMangr.Text) + Val(lblCooIntsh.Text)) / 2), 2))
            lblcollaboration.Text = checkAccespt(Math.Round(((Val(lblCooMangr.Text) + Val(lblCooIntsh.Text)) / 2), 2))
            '--------------------------------------------------------------

            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I414'")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)

                'Label101.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            'Label105.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I414'")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)

                ' Label101.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            '            Label103.Text = checkAccespt(df91)

            Dim dr101() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I414'")
            Dim mg101 As Double = 0
            Dim cnt101 As Integer = 0
            For Each hj In dr101
                cnt101 += 1
                mg101 += hj(6)
                'Label101.Text = hj(1)
            Next
            Dim df101 As String = Math.Round((mg101 / cnt101), 2)
            'Label102.Text = checkAccespt(df101)

            'Dim dr100() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I414'")
            'Dim mg100 As Double = 0
            'Dim cnt100 As Integer = 0
            'For Each hj In dr100
            '    cnt110 += 1
            '    mg110 += hj(6)
            '    Label101.Text = hj(1)
            'Next
            'Dim df102 As String = (mg110 / cnt110)
            'Label104.Text = checkAccespt(df102)

            'Label106.Text = checkAccespt(((mg101 / cnt101) + (mgr91 / cnt91) + (intsh18 / cnt18) + (mg110 / cnt110)))

        Catch ex As Exception
            Dim gs = ex.ToString()
        End Try
    End Sub

    Public Sub TeamBuilding(pno As String)
        Try
            lblDevSelf.Text = Val(0)
            lblDevMangr.Text = Val(0)
            lblDevIntsh.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',sum(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',sum(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where f.ss_asses_pno ='" & pno & "' "
            qry += " and d.SS_QCODE in ('I44','I48','I412','I54','I58','I512','I64','I68','I612') and d.ss_pno = f.ss_pno  and ss_flag='I' and d.ss_qcode = qs.ss_qcode  "
            qry += " and d.ss_asses_pno = f.ss_asses_pno group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I44','I54','I64')")
            Dim intsh As Double = 0
            Dim cnt As Integer = 0
            For Each hj In dr
                cnt += 1
                intsh += hj(4)
                Label51.Text = hj(1)
            Next
            Dim df As String = Math.Round((intsh / cnt), 2)
            lblDevIntsh.Text = Val(lblDevIntsh.Text) + Val(df)
            Label54.Text = checkAccespt(df)

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I44','I54','I64')")
            Dim mgr As Double = 0
            Dim cnt1 As Integer = 0
            For Each hj In dr1
                cnt1 += 1
                mgr += hj(5)
                Label51.Text = hj(1)
            Next
            Dim df1 As String = Math.Round((mgr / cnt1), 2)
            lblDevMangr.Text = Val(lblDevMangr.Text) + Val(df1)
            Label53.Text = checkAccespt(df1)

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I44','I54','I64')")
            Dim mg2 As Double = 0
            Dim cnt2 As Integer = 0
            For Each hj In dr2
                cnt2 += 1
                mg2 += hj(6)
                Label51.Text = hj(1)
            Next
            Dim df2 As String = Math.Round((mg2 / cnt2), 2)
            lblDevSelf.Text = Val(lblDevSelf.Text) + Val(df2)
            Label52.Text = checkAccespt(df2)

            Dim dr7() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I44'")
            Dim mg7 As Double = 0
            Dim cnt7 As Integer = 0
            For Each hj In dr7
                cnt7 += 1
                mg7 += hj(6)
                Label51.Text = hj(1)
            Next
            Dim df7 As String = Math.Round((mg7 / cnt7), 2)
            'Label89.Text = checkAccespt(df7)


            Label55.Text = checkAccespt(Math.Round(((mgr / cnt1) + (intsh / cnt)) / 2, 2))

            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I48','I58','I68')")
            Dim intsh3 As Double = 0
            Dim cnt3 As Integer = 0
            For Each hj In dr3
                cnt3 += 1
                intsh3 += hj(4)
                Label56.Text = hj(1)
            Next
            Dim df3 As String = Math.Round((intsh3 / cnt3), 2)
            lblDevIntsh.Text = Val(lblDevIntsh.Text) + Val(df3)
            Label59.Text = checkAccespt(df3)

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I48','I58','I68')")
            Dim mgr4 As Double = 0
            Dim cnt4 As Integer = 0
            For Each hj In dr4
                cnt4 += 1
                mgr4 += hj(5)
                Label56.Text = hj(1)
            Next
            Dim df4 As String = Math.Round((mgr4 / cnt4), 2)
            lblDevMangr.Text = Val(lblDevMangr.Text) + Val(df4)
            Label58.Text = checkAccespt(df4)

            Dim dr5() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I48','I58','I68')")
            Dim mg5 As Double = 0
            Dim cnt5 As Integer = 0
            For Each hj In dr5
                cnt5 += 1
                mg5 += hj(6)
                Label56.Text = hj(1)
            Next
            Dim df5 As String = Math.Round((mg5 / cnt5), 2)
            lblDevSelf.Text = Val(lblDevSelf.Text) + Val(df5)
            Label57.Text = checkAccespt(df5)

            Dim dr6() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I48'")
            Dim mg6 As Double = 0
            Dim cnt6 As Integer = 0
            For Each hj In dr6
                cnt6 += 1
                mg6 += hj(6)
                Label56.Text = hj(1)
            Next
            Dim df6 As String = Math.Round((mg6 / cnt6), 2)
            'Label88.Text = checkAccespt(df6)

            Label60.Text = checkAccespt(Math.Round(((mgr4 / cnt4) + (intsh3 / cnt3)) / 2, 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I412','I512','I612')")
            Dim intsh8 As Double = 0
            Dim cnt8 As Integer = 0
            For Each hj In dr8
                cnt8 += 1
                intsh8 += hj(4)
                Label61.Text = hj(1)
            Next
            Dim df8 As String = Math.Round((intsh8 / cnt8), 2)
            lblDevIntsh.Text = Val(lblDevIntsh.Text) + Val(df8)
            Label64.Text = checkAccespt(df8)

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I412','I512','I612')")
            Dim mgr9 As Double = 0
            Dim cnt9 As Integer = 0
            For Each hj In dr9
                cnt9 += 1
                mgr9 += hj(5)
                Label61.Text = hj(1)
            Next
            Dim df9 As String = Math.Round((mgr9 / cnt9), 2)
            lblDevMangr.Text = Val(lblDevMangr.Text) + Val(df9)
            Label63.Text = checkAccespt(df9)

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I412','I512','I612')")
            Dim mg10 As Double = 0
            Dim cnt10 As Integer = 0
            For Each hj In dr10
                cnt10 += 1
                mg10 += hj(6)
                Label61.Text = hj(1)
            Next
            Dim df10 As String = Math.Round((mg10 / cnt10), 2)
            lblDevSelf.Text = Val(lblDevSelf.Text) + Val(df10)
            Label62.Text = checkAccespt(df10)

            Dim dr11() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I412'")
            Dim mg11 As Double = 0
            Dim cnt11 As Integer = 0
            For Each hj In dr11
                cnt11 += 1
                mg11 += hj(6)
                Label61.Text = hj(1)
            Next
            Dim df11 As String = (mg11 / cnt11)
            'Label87.Text = checkAccespt(df11)

            Label65.Text = checkAccespt(Math.Round(((mgr9 / cnt9) + (intsh8 / cnt8)) / 2, 2))

            lblDevMangr.Text = Math.Round(Val(lblDevMangr.Text) / 3, 2)
            lblDevIntsh.Text = Math.Round(Val(lblDevIntsh.Text) / 3, 2)
            lblDevSelf.Text = Math.Round(Val(lblDevSelf.Text) / 3, 2)
            Label13.Text = checkAccespt1(lblDevMangr.Text)
            Label14.Text = checkAccespt1(lblDevIntsh.Text)
            Label12.Text = checkAccespt1(lblDevSelf.Text)

            Label15.Text = checkAccespt1(Math.Round(((Val(lblDevMangr.Text) + Val(lblDevIntsh.Text)) / 2), 2))
            lblpeople.Text = checkAccespt(Math.Round(((Val(lblDevMangr.Text) + Val(lblDevIntsh.Text)) / 2), 2))
            '-------------------------------------------------------------------------

            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I416'")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)

                '   Label113.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            'Label117.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I416'")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)

                '  Label113.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            ' Label115.Text = checkAccespt(df91)

            Dim dr101() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I416'")
            Dim mg101 As Double = 0
            Dim cnt101 As Integer = 0
            For Each hj In dr101
                cnt101 += 1
                mg101 += hj(6)
                ' Label113.Text = hj(1)
            Next
            Dim df101 As String = Math.Round((mg101 / cnt101), 2)
            ' Label114.Text = checkAccespt(df101)

            Dim dr100() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I416'")
            Dim mg100 As Double = 0
            Dim cnt100 As Integer = 0
            For Each hj In dr100
                cnt100 += 1
                mg100 += hj(6)
                ' Label113.Text = hj(1)
            Next
            Dim df102 As String = Math.Round((mg100 / cnt100), 2)
            ' Label116.Text = checkAccespt(df102)

            ' Label118.Text = checkAccespt(((mg101 / cnt101) + (mgr91 / cnt91) + (intsh18 / cnt18) + (mg100 / cnt100)))


        Catch ex As Exception
            Dim gs = ex.ToString()
        End Try
    End Sub

    Public Sub Responsiveness(pno As String)
        Try
            lblResSelf.Text = Val(0)
            lblResMangr.Text = Val(0)
            lblResIntsh.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',sum(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',sum(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where f.ss_asses_pno ='" & pno & "' "
            qry += " and d.SS_QCODE in ('I43','I47','I411','I53','I57','I511','I63','I67','I611') and d.ss_pno = f.ss_pno  and ss_flag='I' and d.ss_qcode = qs.ss_qcode  "
            qry += " and d.ss_asses_pno = f.ss_asses_pno group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I43','I53','I63')")
            Dim intsh As Double = 0
            Dim cnt As Integer = 0
            For Each hj In dr
                cnt += 1
                intsh += hj(4)
                Label68.Text = hj(1)
            Next
            Dim df As String = Math.Round((intsh / cnt), 2)
            lblResIntsh.Text = Val(lblResIntsh.Text) + Val(df)
            Label71.Text = checkAccespt(df)

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I43','I53','I63')")
            Dim mgr As Double = 0
            Dim cnt1 As Integer = 0
            For Each hj In dr1
                cnt1 += 1
                mgr += hj(5)
                Label68.Text = hj(1)
            Next
            Dim df1 As String = Math.Round((mgr / cnt1), 2)
            lblResMangr.Text = Val(lblResMangr.Text) + Val(df1)
            Label70.Text = checkAccespt(df1)

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I43','I53','I63')")
            Dim mg2 As Double = 0
            Dim cnt2 As Integer = 0
            For Each hj In dr2
                cnt2 += 1
                mg2 += hj(6)
                Label68.Text = hj(1)
            Next
            Dim df2 As String = Math.Round((mg2 / cnt2), 2)
            lblResSelf.Text = Val(lblResSelf.Text) + Val(df2)
            Label69.Text = checkAccespt(df2)

            Dim dr5() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I43'")
            Dim mg5 As Double = 0
            Dim cnt5 As Integer = 0
            For Each hj In dr5
                cnt5 += 1
                mg5 += hj(6)
                Label68.Text = hj(1)
            Next
            Dim df5 As String = Math.Round((mg5 / cnt5), 2)
            'Label90.Text = checkAccespt(df5)

            Label72.Text = checkAccespt(Math.Round(((mgr / cnt1) + (intsh / cnt)) / 2, 2))

            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I47','I57','I67')")
            Dim intsh3 As Double = 0
            Dim cnt3 As Integer = 0
            For Each hj In dr3
                cnt3 += 1
                intsh3 += hj(4)
                Label73.Text = hj(1)
            Next
            Dim df3 As String = Math.Round((intsh3 / cnt3), 2)
            lblResIntsh.Text = Val(lblResIntsh.Text) + Val(df3)
            Label76.Text = checkAccespt(df3)

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I47','I57','I67')")
            Dim mgr4 As Double = 0
            Dim cnt4 As Integer = 0
            For Each hj In dr4
                cnt4 += 1
                mgr4 += hj(5)
                Label73.Text = hj(1)
            Next
            Dim df4 As String = Math.Round((mgr4 / cnt4), 2)
            lblResMangr.Text = Val(lblResMangr.Text) + Val(df4)
            Label75.Text = checkAccespt(df4)

            Dim dr6() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I47','I57','I67')")
            Dim mg6 As Double = 0
            Dim cnt6 As Integer = 0
            For Each hj In dr6
                cnt6 += 1
                mg6 += hj(6)
                Label73.Text = hj(1)
            Next
            Dim df6 As String = Math.Round((mg6 / cnt6), 2)
            lblResSelf.Text = Val(lblResSelf.Text) + Val(df6)
            Label74.Text = checkAccespt(df6)

            Dim dr15() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I47'")
            Dim mg15 As Double = 0
            Dim cnt15 As Integer = 0
            For Each hj In dr15
                cnt15 += 1
                mg15 += hj(6)
                Label73.Text = hj(1)
            Next
            Dim df10 As String = Math.Round((mg15 / cnt15), 2)
            ' Label91.Text = checkAccespt(df10)

            Label77.Text = checkAccespt(Math.Round(((mgr4 / cnt4) + (intsh3 / cnt3)) / 2, 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE in('I411','I511','I611')")
            Dim intsh8 As Double = 0
            Dim cnt8 As Integer = 0
            For Each hj In dr8
                cnt8 += 1
                intsh8 += hj(4)
                Label78.Text = hj(1)
            Next
            Dim df8 As String = Math.Round((intsh8 / cnt8), 2)
            lblResIntsh.Text = Val(lblResIntsh.Text) + Val(df8)
            Label81.Text = checkAccespt(df8)

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE in('I411','I511','I611')")
            Dim mgr9 As Double = 0
            Dim cnt9 As Integer = 0
            For Each hj In dr9
                cnt9 += 1
                mgr9 += hj(5)
                Label78.Text = hj(1)
            Next
            Dim df9 As String = Math.Round((mgr9 / cnt9), 2)
            lblResMangr.Text = Val(lblResMangr.Text) + Val(df9)
            Label80.Text = checkAccespt(df9)

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE in('I411','I511','I611')")
            Dim mg10 As Double = 0
            Dim cnt10 As Integer = 0
            For Each hj In dr10
                cnt10 += 1
                mg10 += hj(6)
                Label78.Text = hj(1)
            Next
            Dim df12 As String = Math.Round((mg10 / cnt10), 2)
            lblResSelf.Text = Val(lblResSelf.Text) + Val(df12)
            Label79.Text = checkAccespt(df12)

            Dim dr13() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I411'")
            Dim mg13 As Double = 0
            Dim cnt13 As Integer = 0
            For Each hj In dr13
                cnt13 += 1
                mg13 += hj(6)
                Label78.Text = hj(1)
            Next
            Dim df13 As String = Math.Round((mg13 / cnt13), 2)
            ' Label92.Text = checkAccespt(df13)

            Label82.Text = checkAccespt(Math.Round(((mgr9 / cnt9) + (intsh8 / cnt8)) / 2, 2))


            lblResMangr.Text = Math.Round(Val(lblResMangr.Text) / 3, 2)
            lblResIntsh.Text = Math.Round(Val(lblResIntsh.Text) / 3, 2)
            lblResSelf.Text = Math.Round(Val(lblResSelf.Text) / 3, 2)
            Label9.Text = checkAccespt1(lblResMangr.Text)
            Label10.Text = checkAccespt1(lblResIntsh.Text)
            Label8.Text = checkAccespt1(lblResSelf.Text)

            Label11.Text = checkAccespt1(Math.Round(((Val(lblResMangr.Text) + Val(lblResIntsh.Text)) / 2), 2))
            lblresponse.Text = checkAccespt(Math.Round(((Val(lblResMangr.Text) + Val(lblResIntsh.Text)) / 2), 2))
            '-------------------------------------------------------------

            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I415'")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)
                'Label107.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            ' Label111.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I415'")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)
                ' Label107.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            ' Label109.Text = checkAccespt(df91)

            Dim dr100() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I415'")
            Dim mg100 As Double = 0
            Dim cnt100 As Integer = 0
            For Each hj In dr100
                cnt100 += 1
                mg100 += hj(6)
                'Label107.Text = hj(1)
            Next
            Dim df120 As String = Math.Round((mg100 / cnt100), 2)
            ' Label108.Text = checkAccespt(df120)

            Dim dr130() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I415'")
            Dim mg130 As Double = 0
            Dim cnt130 As Integer = 0
            For Each hj In dr130
                cnt130 += 1
                mg130 += hj(6)
                Label78.Text = hj(1)
            Next
            Dim df130 As String = Math.Round((mg130 / cnt130), 2)
            ' Label110.Text = checkAccespt(df130)

            ' Label112.Text = checkAccespt(((mg100 / cnt100) + (mgr91 / cnt91) + (intsh18 / cnt18) + (mg130 / cnt130)))

        Catch ex As Exception
            Dim gs = ex.ToString()
        End Try
    End Sub
    Public Function checkAccespt(val As Double) As String
        Dim rn As String = ""
        Try
            If val <> 0 Then

                If val <= "1.6" Then
                    rn = "U"
                ElseIf val > "1.6" AndAlso val <= "2.6" Then
                    rn = "A"

                ElseIf val > "2.6" Then
                    rn = "G"
                End If
            Else
                rn = ""
            End If
            Return rn
        Catch ex As Exception

        End Try
    End Function

    Public Function checkAccespt1(val As Double) As String
        Dim rn As String = ""
        Try
            If val <> 0 Then

                If val <= "1.6" Then
                    rn = "Uacceptable"
                ElseIf val > "1.6" AndAlso val <= "2.6" Then
                    rn = "Acceptable"

                ElseIf val > "2.6" Then
                    rn = "Gold Standard"
                End If
            Else
                rn = ""
            End If
            Return rn
        Catch ex As Exception

        End Try
    End Function

    Public Sub displayAns(pno As String)
        Try
            Dim str As String = String.Empty
            str = " select distinct ss_categ,SS_Q2_A from t_survey_status df , t_survey_response fd where  df.ss_pno=fd.ss_pno and df.ss_asses_pno=fd.ss_asses_pno and df.ss_asses_pno='" & pno & "'"
            Dim hg = GetData(str, conHrps)
            Dim hf() As DataRow = hg.Select("ss_categ='Self'")
            For Each tg As DataRow In hf
                litans.Text = " <h3> <b>Self </b> </h3> <br/>" & tg(1).ToString() & "<br /> <br />  <h3> <b> Other respondents </b>  </h3> <br />"
            Next

            If hg.Rows.Count > 0 Then
                For i = 0 To hg.Rows.Count - 1
                    litans.Text += " <p>" & hg.Rows(i)(1).ToString() & "</p>"
                Next
            End If

            Dim str1 As String = String.Empty
            str1 = " select distinct df.ss_categ, df.SS_Q2_b from t_survey_status df , t_survey_response fd where  df.ss_pno=fd.ss_pno and df.ss_asses_pno=fd.ss_asses_pno and df.ss_asses_pno='" & pno & "'"
            Dim hg1 = GetData(str1, conHrps)
            Dim hf1() As DataRow = hg1.Select("ss_categ='Self'")
            For Each tg1 As DataRow In hf1
                Literal1.Text = " <h3> <b>Self </b> </h3> <br/>" & tg1(1).ToString() & "<br /> <br /><h3> <b> Other respondents </b> </h3> <br />"
            Next

            If hg1.Rows.Count > 0 Then
                For i = 0 To hg1.Rows.Count - 1
                    Literal1.Text += " <p>" & hg1.Rows(i)(1).ToString() & "</p>"
                Next
            End If
        Catch ex As Exception
            Dim h = ex.ToString
        End Try
    End Sub
End Class
