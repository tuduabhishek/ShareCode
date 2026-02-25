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

    Private Sub Rpt_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim perno = SimpleCrypt(Request.QueryString("pno"))
        Dim yr = SimpleCrypt(Request.QueryString("yr"))
        Dim cycle = SimpleCrypt(Request.QueryString("cyc"))
        'perno = "149747"
        'perno = "121473"
        'Dim f = GetData("select ema_perno, ema_ename, ema_desgn_desc from TIPS.t_empl_all  where ema_perno='" & perno & "'", conHrps)

        If Session("ADM_USER") Is Nothing Then
            'If perno <> "" Then
            If perno = Page.User.Identity.Name.Split("\")(1) Then
            Else
                Response.Write("<center> <b><I> You do not have access to view other reports</b></I></center>")
                Me.Visible = False
            End If
        End If
        'perno = "150006"
        'perno = "121473"
        Dim dc As New OracleCommand()
        If cycle = "1" Then
            dc.CommandText = "select ema_perno, ema_ename, ema_desgn_desc,EMA_EQV_LEVEL from TIPS.t_empl_all  where ema_perno=:pno"
        Else
            dc.CommandText = "select ema_perno, ema_ename, ema_desgn_desc,EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where ema_perno=:pno and ema_year=:ema_year and ema_cycle=:ema_srlno"
        End If

        dc.Parameters.Clear()
        dc.Connection = conHrps
        dc.Parameters.AddWithValue("pno", perno)
        If cycle <> "1" Then
            dc.Parameters.AddWithValue("ema_year", yr)
            dc.Parameters.AddWithValue("ema_srlno", cycle)
        End If

        Dim f = getDataInDt(dc)

        If f.Rows.Count > 0 Then
            lblReceiptNm.Text = f.Rows(0)(1).ToString()
            lblDesignation.Text = f.Rows(0)(2).ToString()
            ' lblReceiptNm.Text = " Qualitative comments for " & f.Rows(0)(1).ToString()
            lblall.Text = "Overall report for " & f.Rows(0)(1).ToString()
            lblDesignation.Text = f.Rows(0)(2).ToString()
            lbloverall.Text = "Overall report for " & f.Rows(0)(1).ToString()
            Label119.Text = "Overall report for " & f.Rows(0)(1).ToString()
            lblnmm.Text = " Qualitative comments for " & f.Rows(0)(1).ToString()
            Label66.Text = " Qualitative comments for " & f.Rows(0)(1).ToString()
            Dim lvl = f.Rows(0)(3).ToString()

            getresCoupt(perno, yr, cycle)

            If cycle <> "1" Then
                divStart.Visible = True
                ViewA11(perno, yr, cycle, lvl)
                displayOverall(perno, yr, cycle, lvl)
                insertData(perno, yr, cycle, lvl)
            Else
                divStart.Visible = True
                Responsiveness(perno, yr, cycle, lvl)
                getaccountibility(perno, yr, cycle, lvl)
                Collaboration(perno, yr, cycle, lvl)
                TeamBuilding(perno, yr, cycle, lvl)
                displayOverall_Cyc1(perno, yr, cycle, lvl)
            End If
            displayAns(perno, yr, cycle, lvl)
        End If

        'getCollaboration(perno)

        ' ClientScript.RegisterStartupScript(Me.[GetType](), "onclick", "<script language=javascript>window.open('FeedbackSurveyRpt_OPR.aspx','PrintMe','height=300px,width=300px,scrollbars=1');</script>")
    End Sub
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
    Public Function SimpleCrypt(ByVal Text As String) As String
        Dim strTempChar As String = "", i As Integer
        If Text Is Nothing Then
            Exit Function


        Else

            For i = 1 To Text.Length
                If Asc(Mid$(Text, i, 1)) < 128 Then
                    strTempChar = CType(Asc(Mid$(Text, i, 1)) + 128, String)
                ElseIf Asc(Mid$(Text, i, 1)) > 128 Then
                    strTempChar = CType(Asc(Mid$(Text, i, 1)) - 128, String)
                End If
                Mid$(Text, i, 1) = Chr(CType(strTempChar, Integer))
            Next i
            Return Text
        End If

    End Function
    Public Function GetData(qry As String, con As OracleConnection) As DataTable

        Try
            Dim comd As New OracleDataAdapter(qry, con)
            Dim data As New DataTable()
            comd.Fill(data)
            Return data
        Catch ex As Exception

        End Try

    End Function
    Public Sub getresCoupt(pno As String, yer As String, cyc As String)
        Try
            Dim qry As String = String.Empty
            qry = "select irc_desc, SS_CATEG,count(SS_CATEG)  from hrps.t_survey_status, hrps.t_ir_codes where ss_asses_pno='" & pno & "'  "
            qry += "and upper(irc_code) = Upper(ss_categ) and irc_type='360RL' and ss_year='" & yer.ToString & "' and ss_srlno='" & cyc.ToString & "' and SS_APP_TAG ='AP' and SS_RPT_FLAG='Y' and SS_Q2_A is not null group by SS_CATEG,irc_desc"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim df = GetData("select count(ss_pno) from t_survey_status where ss_asses_pno='" & pno & "' and ss_year='" & yer.ToString & "' and ss_srlno='" & cyc.ToString & "' and SS_APP_TAG ='AP' ", conHrps)

            Dim qry1 As String = String.Empty
            Dim yrCyc As String = yer.Substring(2, 2).ToString() & "" & cyc.Trim().ToString
            qry1 = "SELECT IRC_DESC FROM HRPS.T_IR_CODES WHERE IRC_TYPE ='36PDF' AND IRC_CODE='" & yrCyc & "' AND IRC_VALID_TAG='A'"
            Dim d1 = GetData(qry1, conHrps)
            lblHeading.Text = d1.Rows(0)(0).ToString

            If Not d Is Nothing Then
                For g = 0 To d.Rows.Count - 1
                    re += d.Rows(g)(0) & " - " & d.Rows(g)(2) & ", "
                    cont = cont + d.Rows(g)(2)
                Next
                re = re.Trim
                re = re.TrimEnd(",")
                lbloverscore.Text = "Your behaviour score is based on the responses of <b>" & cont & " individuals(" & re & ")</b> <br/><br/> A  total of <b>" & df.Rows(0)(0)
                lbloverscore.Text += "</b> surveys were distributed. <b>" & cont & "</b> surveys were completed and have been included in this feedback report."
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
            str = "select A.SS_PNO, (avg(A.ss_qoptn)) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO and A.ss_asses_pno ='" & pno & "' "
            str += " AND A.SS_QCODE in('I32','I36','I310','I314')  GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d = GetData(str, conHrps)

            If d.Rows.Count > 0 Then
                Dim dr() As DataRow = d.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p As DataRow In dr
                    k += 1
                    i += p(1)
                Next
                Label6.Text = Math.Round(i, 2)

                Dim dr1() As DataRow = d.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label5.Text = Math.Round(i1, 2)

                Dim dr2() As DataRow = d.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                Label4.Text = Math.Round(i2, 2)

                Dim dr3() As DataRow = d.Select("SS_CATEG='PEER'")
                Dim i3 As Integer = 0
                Dim k3 As Integer = 0
                For Each p3 As DataRow In dr3
                    k3 += 1
                    i3 += p3(1)
                Next
                Label84.Text = Math.Round(i3, 2)
            End If
            Label7.Text = (Convert.ToDouble(Label5.Text) + Convert.ToDouble(Label6.Text) + Convert.ToDouble(Label84.Text))
            Label7.Text = Math.Round((Label7.Text / 3), 2)

            Label4.Text = checkAccespt1(Label4.Text)
            Label6.Text = checkAccespt1(Label6.Text)
            Label5.Text = checkAccespt1(Label5.Text)
            Label7.Text = checkAccespt1(Label7.Text)
            Label84.Text = checkAccespt1(Label84.Text)

            lblcollaboration.Text = Left(Label7.Text, 1)

            '------------------------------------------------------------ Responsiveness-------

            Dim str1 As String = String.Empty
            str1 = "select A.SS_PNO, (avg(A.ss_qoptn)) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO and A.ss_asses_pno ='" & pno & "' "
            str1 += " AND A.SS_QCODE in('I33','I37','I311','I315')  GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d1 = GetData(str1, conHrps)

            If d.Rows.Count > 0 Then
                Dim dr() As DataRow = d1.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p1 As DataRow In dr
                    k += 1
                    i += p1(1)
                Next
                Label10.Text = Math.Round(i, 2)

                Dim dr1() As DataRow = d1.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label9.Text = Math.Round(i1, 2)

                Dim dr2() As DataRow = d1.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                Label8.Text = Math.Round(i2, 2)

                Dim dr5() As DataRow = d1.Select("SS_CATEG='PEER'")
                Dim i5 As Integer = 0
                Dim k5 As Integer = 0
                For Each p5 As DataRow In dr5
                    k5 += 1
                    i5 += p5(1)
                Next
                Label85.Text = Math.Round(i5, 2)
            End If
            Label11.Text = (Convert.ToDouble(Label9.Text) + Convert.ToDouble(Label10.Text) + Convert.ToDouble(Label85.Text))
            Label11.Text = Math.Round((Label11.Text / 3), 2)


            Label8.Text = checkAccespt1(Label8.Text)
            Label9.Text = checkAccespt1(Label9.Text)
            Label10.Text = checkAccespt1(Label10.Text)
            Label11.Text = checkAccespt1(Label11.Text)
            Label85.Text = checkAccespt1(Label85.Text)
            lblresponse.Text = Left(Label11.Text, 1)
            '------------------------------------------------------------ Team Building---------

            Dim str2 As String = String.Empty
            str2 = "select A.SS_PNO, (avg(A.ss_qoptn)) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO and A.ss_asses_pno ='" & pno & "' "
            str2 += " AND A.SS_QCODE in('I34','I38','I312','I316')   GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d2 = GetData(str2, conHrps)

            If d2.Rows.Count > 0 Then
                Dim dr() As DataRow = d2.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p1 As DataRow In dr
                    k += 1
                    i += p1(1)
                Next
                Label14.Text = Math.Round(i, 2)

                Dim dr1() As DataRow = d2.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label13.Text = Math.Round(i1, 2)

                Dim dr2() As DataRow = d2.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                Label12.Text = Math.Round(i2, 2)

                Dim dr6() As DataRow = d2.Select("SS_CATEG='PEER'")
                Dim i6 As Integer = 0
                Dim k6 As Integer = 0
                For Each p6 As DataRow In dr6
                    k6 += 1
                    i6 += p6(1)
                Next
                Label86.Text = Math.Round(i6, 2)

            End If
            Label15.Text = (Convert.ToDouble(Label14.Text) + Convert.ToDouble(Label13.Text) + Convert.ToDouble(Label86.Text))
            Label15.Text = Math.Round((Label15.Text / 3), 2)

            Label12.Text = checkAccespt1(Label12.Text)
            Label13.Text = checkAccespt1(Label13.Text)
            Label14.Text = checkAccespt1(Label14.Text)
            Label15.Text = checkAccespt1(Label15.Text)
            Label86.Text = checkAccespt1(Label86.Text)

            lblpeople.Text = Left(Label15.Text, 1)
            '------------------------------------------------------------ Accountability

            Dim str3 As String = String.Empty
            str3 = "select A.SS_PNO, (avg(A.ss_qoptn)) SCORE,B.SS_CATEG from hrps.t_survey_response A, HRPS.t_survey_status B  where A.SS_PNO = B.SS_PNO  AND A.SS_ASSES_PNO=B.SS_ASSES_PNO and A.ss_asses_pno ='" & pno & "' "
            str3 += " AND A.SS_QCODE in('I31','I35','I39','I313')  GROUP BY  B.SS_CATEG,A.SS_PNO ORDER BY 3 "

            Dim d3 = GetData(str3, conHrps)

            If d3.Rows.Count > 0 Then
                Dim dr() As DataRow = d3.Select("SS_CATEG='INTSH'")
                Dim i As Integer = 0
                Dim k As Integer = 0
                For Each p1 As DataRow In dr
                    k += 1
                    i += p1(1)
                Next
                Label2.Text = Math.Round(i / k, 2)

                Dim dr1() As DataRow = d3.Select("SS_CATEG='MANGR'")
                Dim i1 As Integer = 0
                Dim k1 As Integer = 0
                For Each p1 As DataRow In dr1
                    k1 += 1
                    i1 += p1(1)
                Next
                Label1.Text = Math.Round(i1, 2)

                Dim dr2() As DataRow = d3.Select("SS_CATEG='Self'")
                Dim i2 As Integer = 0
                Dim k2 As Integer = 0
                For Each p2 As DataRow In dr2
                    k2 += 1
                    i2 += p2(1)
                Next
                lblaccself.Text = Math.Round(i2, 2)

                Dim dr7() As DataRow = d3.Select("SS_CATEG='PEER'")
                Dim i7 As Integer = 0
                Dim k7 As Integer = 0
                For Each p7 As DataRow In dr7
                    k7 += 1
                    i7 += p7(1)
                Next
                Label83.Text = Math.Round(i7, 2)

            End If
            Label3.Text = (Convert.ToDouble(Label1.Text) + Convert.ToDouble(Label2.Text) + Convert.ToDouble(Label83.Text))
            Label3.Text = Math.Round((Label3.Text / 3), 2)

            Label2.Text = checkAccespt1(Label2.Text)
            Label1.Text = checkAccespt1(Label1.Text)
            lblaccself.Text = checkAccespt1(lblaccself.Text)
            Label3.Text = checkAccespt1(Label3.Text)
            lblaccountability.Text = Left(Label3.Text, 1)
            Label83.Text = checkAccespt1(Label83.Text)
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
    Public Sub getaccountibility(pno As String, yer As String, cyc As String, levl As String)
        Try
            lblAccMangr.Text = Val(0)
            lblAccIntsh.Text = Val(0)
            lblAccPeer.Text = Val(0)
            'lblAccSub.Text = Val(0)
            lblAccOverall.Text = Val(0)
            lblAccSelf1.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',avg(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',avg(d.ss_qoptn)),decode (f.ss_categ,'PEER',avg(d.ss_qoptn)),decode (f.ss_categ,'ROPT',avg(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where d.ss_pno = f.ss_pno and d.ss_qcode = qs.ss_qcode and d.ss_serial = f.ss_srlno  and d.ss_year = f.ss_year"
            qry += " and  d.ss_asses_pno = f.ss_asses_pno   and ss_flag='I' and f.SS_RPT_FLAG='Y' "
            qry += " and f.ss_asses_pno ='" & pno & "' and f.ss_year='" & yer.ToString & "' and f.ss_srlno='" & cyc.ToString & "' and d.SS_QCODE in ('I31','I35','I39','I313') group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I31'")
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

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I31'")
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

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I31'")
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

            Dim dr12() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I31'")
            Dim mg12 As Double = 0
            Dim cnt12 As Integer = 0
            For Each hj In dr12
                cnt12 += 1
                mg12 += hj(7)
                Label32.Text = hj(1)
            Next
            Dim df12 As String = Math.Round((mg12 / cnt12), 2)
            lblAccPeer.Text = Val(lblAccPeer.Text) + Val(df12)
            Label96.Text = checkAccespt(df12)

            'Dim mg13 As Double = 0
            'Dim cnt13 As Integer = 0
            'If levl = "I2" Then
            '    Dim dr13() As DataRow = bv.Select("ss_categ='ROPT' and SS_QCODE='I31'")
            '    For Each hj In dr13
            '        cnt13 += 1
            '        mg13 += hj(8)
            '        Label32.Text = hj(1)
            '    Next
            '    Dim df13 As String = Math.Round((mg13 / cnt13), 2)
            '    lblAccSub.Text = Val(lblAccSub.Text) + Val(df13)
            '    lblAccSubVal1.Text = checkAccespt(df13)
            'End If

            'If levl = "I2" Then
            '    HdAccSubVal.Visible = True
            '    tdAccSubVal1.Visible = True
            '    tdAccSubVal2.Visible = True
            '    tdAccSubVal3.Visible = True
            '    tdAccSubVal4.Visible = True
            '    hdAccPeer.Text = "Peer"
            '    Label19.Text = checkAccespt(Math.Round(((((mgr / cnt1) + (intsh / cnt) + (mg12 / cnt12) + (mg13 / cnt13)) / 4)), 2))
            'ElseIf levl = "I3" Then
            '    Label19.Text = checkAccespt(Math.Round(((((mgr / cnt1) + (intsh / cnt) + (mg12 / cnt12)) / 3)), 2))
            'End If

            Label19.Text = checkAccespt(Math.Round(((((mgr / cnt1) + (intsh / cnt) + (mg12 / cnt12)) / 3)), 2))


            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I35'")
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

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I35'")
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

            Dim dr5() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I35'")
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

            Dim dr15() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I35'")
            Dim mg15 As Double = 0
            Dim cnt15 As Integer = 0
            For Each hj In dr15
                cnt15 += 1
                mg15 += hj(7)
                Label33.Text = hj(1)
            Next
            Dim df15 As String = Math.Round((mg15 / cnt15), 2)
            lblAccPeer.Text = Val(lblAccPeer.Text) + Val(df15)
            Label97.Text = checkAccespt(df15)

            'Dim mg16 As Double = 0
            'Dim cnt16 As Integer = 0
            'If levl = "I2" Then
            '    Dim dr16() As DataRow = bv.Select("ss_categ='ROPT' and SS_QCODE='I35'")
            '    For Each hj In dr16
            '        cnt16 += 1
            '        mg16 += hj(8)
            '        Label33.Text = hj(1)
            '    Next
            '    Dim df16 As String = Math.Round((mg16 / cnt16), 2)
            '    lblAccSub.Text = Val(lblAccSub.Text) + Val(df16)
            '    lblAccSubVal2.Text = checkAccespt(df16)
            'End If

            'If levl = "I2" Then
            '    Label23.Text = checkAccespt(Math.Round(((((mgr4 / cnt4) + (intsh3 / cnt3) + (mg15 / cnt15) + (mg16 / cnt16)) / 4)), 2))
            'ElseIf levl = "I3" Then
            '    Label23.Text = checkAccespt(Math.Round(((((mgr4 / cnt4) + (intsh3 / cnt3) + (mg15 / cnt15)) / 3)), 2))
            'End If

            Label23.Text = checkAccespt(Math.Round(((((mgr4 / cnt4) + (intsh3 / cnt3) + (mg15 / cnt15)) / 3)), 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I39'")
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

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I39'")
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

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I39'")
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

            Dim dr19() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I39'")
            Dim mgr19 As Double = 0
            Dim cnt19 As Integer = 0
            For Each hj In dr19
                cnt19 += 1
                mgr19 += hj(7)
                Label34.Text = hj(1)
            Next
            Dim df19 As String = Math.Round((mgr19 / cnt19), 2)
            lblAccPeer.Text = Val(lblAccPeer.Text) + Val(df19)
            Label98.Text = checkAccespt(df19)


            'Dim mg20 As Double = 0
            'Dim cnt20 As Integer = 0
            'If levl = "I2" Then
            '    Dim dr20() As DataRow = bv.Select("ss_categ='ROPT' and SS_QCODE='I39'")
            '    For Each hj In dr20
            '        cnt20 += 1
            '        mg20 += hj(8)
            '        Label34.Text = hj(1)
            '    Next
            '    Dim df20 As String = Math.Round((mg20 / cnt20), 2)
            '    lblAccSub.Text = Val(lblAccSub.Text) + Val(df20)
            '    lblAccSubVal3.Text = checkAccespt(df20)
            'End If

            'If levl = "I2" Then
            '    Label27.Text = checkAccespt(Math.Round((((mgr9 / cnt9) + (intsh8 / cnt8) + (mgr19 / cnt19) + (mg20 / cnt20)) / 4), 2))
            'ElseIf levl = "I3" Then
            '    Label27.Text = checkAccespt(Math.Round((((mgr9 / cnt9) + (intsh8 / cnt8) + (mgr19 / cnt19)) / 3), 2))
            'End If


            Label27.Text = checkAccespt(Math.Round((((mgr9 / cnt9) + (intsh8 / cnt8) + (mgr19 / cnt19)) / 3), 2))


            '-----------------------------------------------------------------------------------
            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I313'")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)
                Label35.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            lblAccIntsh.Text = Val(lblAccIntsh.Text) + Val(df18)
            Label30.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I313'")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)
                Label35.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            lblAccMangr.Text = Val(lblAccMangr.Text) + Val(df91)
            Label29.Text = checkAccespt(df91)

            Dim dr110() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I313'")
            Dim mg110 As Double = 0
            Dim cnt110 As Integer = 0
            For Each hj In dr110
                cnt110 += 1
                mg110 += hj(6)
                Label35.Text = hj(1)
            Next
            Dim df110 As String = Math.Round((mg110 / cnt110), 2)
            lblAccSelf1.Text = Val(lblAccSelf1.Text) + Val(df110)
            Label28.Text = checkAccespt(df110)

            Dim dr190() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I313'")
            Dim mgr190 As Double = 0
            Dim cnt190 As Integer = 0
            For Each hj In dr190
                cnt190 += 1
                mgr190 += hj(7)
                Label35.Text = hj(1)
            Next
            Dim df190 As String = Math.Round((mgr190 / cnt190), 2)
            lblAccPeer.Text = Val(lblAccPeer.Text) + Val(df190)
            Label100.Text = checkAccespt(df190)

            'Dim mg191 As Double = 0
            'Dim cnt191 As Integer = 0
            'If levl = "I2" Then
            '    Dim dr191() As DataRow = bv.Select("ss_categ='ROPT' and SS_QCODE='I313'")
            '    For Each hj In dr191
            '        cnt191 += 1
            '        mg191 += hj(8)
            '        Label35.Text = hj(1)
            '    Next
            '    Dim df191 As String = Math.Round((mg191 / cnt191), 2)
            '    lblAccSub.Text = Val(lblAccSub.Text) + Val(df191)
            '    lblAccSubVal4.Text = checkAccespt(df191)
            'End If

            'If levl = "I2" Then
            '    Label31.Text = checkAccespt(Math.Round(((((mgr91 / cnt91) + (intsh18 / cnt18) + (mgr190 / cnt190) + (mg191 / cnt191)) / 4)), 2))
            'ElseIf levl = "I3" Then
            '    Label31.Text = checkAccespt(Math.Round(((((mgr91 / cnt91) + (intsh18 / cnt18) + (mgr190 / cnt190)) / 3)), 2))
            'End If

            Label31.Text = checkAccespt(Math.Round(((((mgr91 / cnt91) + (intsh18 / cnt18) + (mgr190 / cnt190)) / 3)), 2))




            lblAccMangr.Text = Math.Round(Val(lblAccMangr.Text) / 4, 2)
            lblAccIntsh.Text = Math.Round(Val(lblAccIntsh.Text) / 4, 2)
            lblAccPeer.Text = Math.Round(Val(lblAccPeer.Text) / 4, 2)
            lblAccSelf1.Text = Math.Round(Val(lblAccSelf1.Text) / 4, 2)


            Label1.Text = checkAccespt1(lblAccMangr.Text)
            Label2.Text = checkAccespt1(lblAccIntsh.Text)
            Label83.Text = checkAccespt1(lblAccPeer.Text)
            lblaccself.Text = checkAccespt1(lblAccSelf1.Text)

            'If levl = "I2" Then
            '    lblAccSub.Text = Math.Round(Val(lblAccSub.Text) / 4, 2)
            '    lbltdOvaAcc.Text = checkAccespt1(lblAccSub.Text)
            'End If

            'If levl = "I2" Then
            '    Label3.Text = checkAccespt1(Math.Round(((Val(lblAccMangr.Text) + Val(lblAccIntsh.Text) + Val(lblAccPeer.Text) + Val(lblAccSub.Text)) / 4), 2))
            '    lblaccountability.Text = checkAccespt(Math.Round(((Val(lblAccMangr.Text) + Val(lblAccIntsh.Text) + Val(lblAccPeer.Text) + Val(lblAccSub.Text)) / 4), 2))
            'ElseIf levl = "I3" Then
            Label3.Text = checkAccespt1(Math.Round(((Val(lblAccMangr.Text) + Val(lblAccIntsh.Text) + Val(lblAccPeer.Text)) / 3), 2))
            lblaccountability.Text = checkAccespt(Math.Round(((Val(lblAccMangr.Text) + Val(lblAccIntsh.Text) + Val(lblAccPeer.Text)) / 3), 2))
            'End If

        Catch ex As Exception
            Dim gs = ex.ToString()
        End Try
    End Sub
    Public Sub Collaboration(pno As String, yer As String, cyc As String, levl As String)
        Try
            lblCooIntsh.Text = Val(0)
            lblCooMangr.Text = Val(0)
            lblCooPeer.Text = Val(0)
            lblCooSelf.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',avg(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',avg(d.ss_qoptn)),decode (f.ss_categ,'PEER',avg(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where d.ss_pno = f.ss_pno and d.ss_qcode = qs.ss_qcode and d.ss_asses_pno = f.ss_asses_pno and d.ss_serial = f.ss_srlno  and d.ss_year = f.ss_year and f.ss_asses_pno ='" & pno & "' and f.ss_year='" & yer.ToString & "' and f.ss_srlno='" & cyc.ToString & "' "
            qry += "   and ss_flag='I'  and f.SS_RPT_FLAG='Y' "
            qry += "  and d.SS_QCODE in ('I32','I36','I310','I314') group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I32'")
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

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I32'")
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

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I32'")
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

            Dim dr13() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I32'")
            Dim mg13 As Double = 0
            Dim cnt13 As Integer = 0
            For Each hj In dr13
                cnt13 += 1
                mg13 += hj(7)
                Label36.Text = hj(1)
            Next
            Dim df13 As String = Math.Round((mg13 / cnt13), 2)
            lblCooPeer.Text = Val(lblCooPeer.Text) + Val(df13)
            Label93.Text = checkAccespt(df13)

            Label40.Text = checkAccespt(Math.Round(((((mgr / cnt1) + (intsh / cnt) + (mg13 / cnt13)) / 3)), 2))

            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I36'")
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

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I36'")
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

            Dim dr5() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I36'")
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

            Dim dr15() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I36'")
            Dim mg15 As Double = 0
            Dim cnt15 As Integer = 0
            For Each hj In dr15
                cnt15 += 1
                mg15 += hj(7)
                Label41.Text = hj(1)
            Next
            Dim df15 As String = Math.Round((mg15 / cnt15), 2)
            lblCooPeer.Text = Val(lblCooPeer.Text) + Val(df15)
            Label94.Text = checkAccespt(df15)

            Label45.Text = checkAccespt(Math.Round(((((mgr4 / cnt4) + (intsh3 / cnt3) + (mg15 / cnt15)) / 3)), 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I310'")
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

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I310'")
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

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I310'")
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

            Dim dr110() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I310'")
            Dim mg110 As Double = 0
            Dim cnt110 As Integer = 0
            For Each hj In dr110
                cnt110 += 1
                mg110 += hj(7)
                Label46.Text = hj(1)
            Next
            Dim df110 As String = Math.Round((mg110 / cnt110), 2)
            lblCooPeer.Text = Val(lblCooPeer.Text) + Val(df110)
            Label95.Text = checkAccespt(df110)

            Label50.Text = checkAccespt(Math.Round(((((mgr9 / cnt9) + (intsh8 / cnt8) + (mg110 / cnt110)) / 3)), 2))

            '--------------------------------------------------------------

            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I314'")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)

                Label101.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            lblCooIntsh.Text = Val(lblCooIntsh.Text) + Val(df18)
            Label105.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I314'")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)

                Label101.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            lblCooMangr.Text = Val(lblCooMangr.Text) + Val(df91)
            Label103.Text = checkAccespt(df91)

            Dim dr101() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I314'")
            Dim mg101 As Double = 0
            Dim cnt101 As Integer = 0
            For Each hj In dr101
                cnt101 += 1
                mg101 += hj(6)
                Label101.Text = hj(1)
            Next
            Dim df101 As String = Math.Round((mg101 / cnt101), 2)
            lblCooSelf.Text = Val(lblCooSelf.Text) + Val(df101)
            Label102.Text = checkAccespt(df101)

            Dim dr100() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I314'")
            Dim mg100 As Double = 0
            Dim cnt100 As Integer = 0
            For Each hj In dr100
                cnt100 += 1
                mg100 += hj(7)
                Label101.Text = hj(1)
            Next
            Dim df102 As String = Math.Round((mg100 / cnt100), 2)
            lblCooPeer.Text = Val(lblCooPeer.Text) + Val(df102)
            Label104.Text = checkAccespt(df102)

            Label106.Text = checkAccespt(Math.Round(((((mgr91 / cnt91) + (intsh18 / cnt18) + (mg100 / cnt100)) / 3)), 2))

            lblCooMangr.Text = Math.Round(Val(lblCooMangr.Text) / 4, 2)
            lblCooIntsh.Text = Math.Round(Val(lblCooIntsh.Text) / 4, 2)
            lblCooPeer.Text = Math.Round(Val(lblCooPeer.Text) / 4, 2)
            lblCooSelf.Text = Math.Round(Val(lblCooSelf.Text) / 4, 2)
            Label5.Text = checkAccespt1(lblCooMangr.Text)
            Label6.Text = checkAccespt1(lblCooIntsh.Text)
            Label84.Text = checkAccespt1(lblCooPeer.Text)
            Label4.Text = checkAccespt1(lblCooSelf.Text)

            Label7.Text = checkAccespt1(Math.Round(((Val(lblCooMangr.Text) + Val(lblCooIntsh.Text) + Val(lblCooPeer.Text)) / 3), 2))
            lblcollaboration.Text = checkAccespt(Math.Round(((Val(lblCooMangr.Text) + Val(lblCooIntsh.Text) + Val(lblCooPeer.Text)) / 3), 2))

        Catch ex As Exception
            Dim gs = ex.ToString()
        End Try
    End Sub
    Public Sub TeamBuilding(pno As String, yer As String, cyc As String, levl As String)
        Try
            lblDevIntsh.Text = Val(0)
            lblDevPeer.Text = Val(0)
            lblDevSelf.Text = Val(0)
            lblDevMangr.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',avg(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',avg(d.ss_qoptn)),decode (f.ss_categ,'PEER',avg(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where d.ss_asses_pno = f.ss_asses_pno and d.ss_pno = f.ss_pno and d.ss_qcode = qs.ss_qcode and d.ss_serial = f.ss_srlno  and d.ss_year = f.ss_year and f.ss_asses_pno ='" & pno & "' and f.ss_year='" & yer.ToString & "' and f.ss_srlno='" & cyc.ToString & "'  "
            qry += "    and ss_flag='I'  and f.SS_RPT_FLAG='Y'  "
            qry += " and d.SS_QCODE in ('I34','I38','I312','I316') group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I34'")
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

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I34'")
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

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I34'")
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

            Dim dr7() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I34'")
            Dim mg7 As Double = 0
            Dim cnt7 As Integer = 0
            For Each hj In dr7
                cnt7 += 1
                mg7 += hj(7)
                Label51.Text = hj(1)
            Next
            Dim df7 As String = Math.Round((mg7 / cnt7), 2)
            lblDevPeer.Text = Val(lblDevPeer.Text) + Val(df7)
            Label89.Text = checkAccespt(df7)


            Label55.Text = checkAccespt(Math.Round(((((mgr / cnt1) + (intsh / cnt) + (mg7 / cnt7)) / 3)), 2))

            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I38'")
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

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I38'")
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

            Dim dr5() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I38'")
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

            Dim dr6() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I38'")
            Dim mg6 As Double = 0
            Dim cnt6 As Integer = 0
            For Each hj In dr6
                cnt6 += 1
                mg6 += hj(7)
                Label56.Text = hj(1)
            Next
            Dim df6 As String = Math.Round((mg6 / cnt6), 2)
            lblDevPeer.Text = Val(lblDevPeer.Text) + Val(df6)
            Label88.Text = checkAccespt(df6)

            Label60.Text = checkAccespt(Math.Round(((((mgr4 / cnt4) + (intsh3 / cnt3) + (mg6 / cnt6)) / 3)), 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I312'")
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

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I312'")
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

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I312'")
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

            Dim dr11() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I312'")
            Dim mg11 As Double = 0
            Dim cnt11 As Integer = 0
            For Each hj In dr11
                cnt11 += 1
                mg11 += hj(7)
                Label61.Text = hj(1)
            Next
            Dim df11 As String = Math.Round((mg11 / cnt11), 2)
            lblDevPeer.Text = Val(lblDevPeer.Text) + Val(df11)
            Label87.Text = checkAccespt(df11)

            Label65.Text = checkAccespt(Math.Round(((((mgr9 / cnt9) + (intsh8 / cnt8) + (mg11 / cnt11)) / 3)), 2))
            '-------------------------------------------------------------------------

            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I316'")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)

                Label113.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            lblDevIntsh.Text = Val(lblDevIntsh.Text) + Val(df18)
            Label117.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I316'")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)

                Label113.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            lblDevMangr.Text = Val(lblDevMangr.Text) + Val(df91)
            Label115.Text = checkAccespt(df91)

            Dim dr101() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I316'")
            Dim mg101 As Double = 0
            Dim cnt101 As Integer = 0
            For Each hj In dr101
                cnt101 += 1
                mg101 += hj(6)
                Label113.Text = hj(1)
            Next
            Dim df101 As String = Math.Round((mg101 / cnt101), 2)
            lblDevSelf.Text = Val(lblDevSelf.Text) + Val(df101)
            Label114.Text = checkAccespt(df101)

            Dim dr100() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I316'")
            Dim mg100 As Double = 0
            Dim cnt100 As Integer = 0
            For Each hj In dr100
                cnt100 += 1
                mg100 += hj(7)
                Label113.Text = hj(1)
            Next
            Dim df102 As String = Math.Round((mg100 / cnt100), 2)
            lblDevPeer.Text = Val(lblDevPeer.Text) + Val(df102)
            Label116.Text = checkAccespt(df102)

            Label118.Text = checkAccespt(Math.Round(((((mgr91 / cnt91) + (intsh18 / cnt18) + (mg100 / cnt100)) / 3)), 2))

            lblDevMangr.Text = Math.Round(Val(lblDevMangr.Text) / 4, 2)
            lblDevIntsh.Text = Math.Round(Val(lblDevIntsh.Text) / 4, 2)
            lblDevPeer.Text = Math.Round(Val(lblDevPeer.Text) / 4, 2)
            lblDevSelf.Text = Math.Round(Val(lblDevSelf.Text) / 4, 2)
            Label13.Text = checkAccespt1(lblDevMangr.Text)
            Label14.Text = checkAccespt1(lblDevIntsh.Text)
            Label86.Text = checkAccespt1(lblDevPeer.Text)
            Label12.Text = checkAccespt1(lblDevSelf.Text)

            Label15.Text = checkAccespt1(Math.Round(((Val(lblDevMangr.Text) + Val(lblDevIntsh.Text) + Val(lblDevPeer.Text)) / 3), 2))
            lblpeople.Text = checkAccespt(Math.Round(((Val(lblDevMangr.Text) + Val(lblDevIntsh.Text) + Val(lblDevPeer.Text)) / 3), 2))

        Catch ex As Exception
            Dim gs = ex.ToString()
        End Try
    End Sub
    Public Sub Responsiveness(pno As String, yer As String, cyc As String, levl As String)
        Try
            lblResIntsh.Text = Val(0)
            lblResMangr.Text = Val(0)
            lblResPeer.Text = Val(0)
            lblResSelf.Text = Val(0)
            Dim qry As String = String.Empty
            qry = "select distinct d.ss_pno, substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))  ques,d.SS_QCODE"
            qry += " , f.ss_categ,decode (f.ss_categ,'INTSH',avg(d.ss_qoptn)),decode (f.ss_categ,'MANGR',avg(d.ss_qoptn))"
            qry += " ,decode (f.ss_categ,'Self',avg(d.ss_qoptn)) ,decode (f.ss_categ,'PEER',avg(d.ss_qoptn)) from t_survey_response d , t_survey_status  f ,t_survey_question qs where d.ss_asses_pno = f.ss_asses_pno and d.ss_pno = f.ss_pno  and d.ss_qcode = qs.ss_qcode and d.ss_serial = f.ss_srlno  and d.ss_year = f.ss_year and f.ss_asses_pno ='" & pno & "'  and f.ss_year='" & yer.ToString & "' and f.ss_srlno='" & cyc.ToString & "'"
            qry += "  and  ss_flag='I' and SS_RPT_FLAG='Y'  "
            qry += " and d.SS_QCODE in ('I33','I37','I311','I315') group by d.ss_pno, f.ss_categ,substr(qs.ss_qtext,instr(qs.ss_qtext,'$') + 1,length(qs.ss_qtext))"
            qry += " ,d.SS_QCODE"
            Dim bv = GetData(qry, conHrps)
            Dim dr() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I33'")
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

            Dim dr1() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I33'")
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

            Dim dr2() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I33'")
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

            Dim dr5() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I33'")
            Dim mg5 As Double = 0
            Dim cnt5 As Integer = 0
            For Each hj In dr5
                cnt5 += 1
                mg5 += hj(7)
                Label68.Text = hj(1)
            Next
            Dim df5 As String = Math.Round((mg5 / cnt5), 2)
            lblResPeer.Text = Val(lblResPeer.Text) + Val(df5)
            Label90.Text = checkAccespt(df5)

            Label72.Text = checkAccespt(Math.Round(((((mgr / cnt1) + (intsh / cnt) + (mg5 / cnt5)) / 3)), 2))

            '---------------------------------------------------

            Dim dr3() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I37'")
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

            Dim dr4() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I37'")
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

            Dim dr6() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I37'")
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

            Dim dr15() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I37'")
            Dim mg15 As Double = 0
            Dim cnt15 As Integer = 0
            For Each hj In dr15
                cnt15 += 1
                mg15 += hj(7)
                Label73.Text = hj(1)
            Next
            Dim df10 As String = Math.Round((mg15 / cnt15), 2)
            lblResPeer.Text = Val(lblResPeer.Text) + Val(df10)
            Label91.Text = checkAccespt(df10)

            Label77.Text = checkAccespt(Math.Round(((((mgr4 / cnt4) + (intsh3 / cnt3) + (mg15 / cnt15)) / 3)), 2))

            '-------------------------------------------------------------

            Dim dr8() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I311'")
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

            Dim dr9() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I311'")
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

            Dim dr10() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I311'")
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

            Dim dr13() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I311'")
            Dim mg13 As Double = 0
            Dim cnt13 As Integer = 0
            For Each hj In dr13
                cnt13 += 1
                mg13 += hj(7)
                Label78.Text = hj(1)
            Next
            Dim df13 As String = Math.Round((mg13 / cnt13), 2)
            lblResPeer.Text = Val(lblResPeer.Text) + Val(df13)
            Label92.Text = checkAccespt(df13)

            Label82.Text = checkAccespt(Math.Round(((((mgr9 / cnt9) + (intsh8 / cnt8) + (mg13 / cnt13)) / 3)), 2))



            '-------------------------------------------------------------

            Dim dr18() As DataRow = bv.Select("ss_categ='INTSH' and SS_QCODE='I315'")
            Dim intsh18 As Double = 0
            Dim cnt18 As Integer = 0
            For Each hj In dr18
                cnt18 += 1
                intsh18 += hj(4)
                Label107.Text = hj(1)
            Next
            Dim df18 As String = Math.Round((intsh18 / cnt18), 2)
            lblResIntsh.Text = Val(lblResIntsh.Text) + Val(df18)
            Label111.Text = checkAccespt(df18)

            Dim dr91() As DataRow = bv.Select("ss_categ='MANGR' and SS_QCODE='I315'")
            Dim mgr91 As Double = 0
            Dim cnt91 As Integer = 0
            For Each hj In dr91
                cnt91 += 1
                mgr91 += hj(5)
                Label107.Text = hj(1)
            Next
            Dim df91 As String = Math.Round((mgr91 / cnt91), 2)
            lblResMangr.Text = Val(lblResMangr.Text) + Val(df91)
            Label109.Text = checkAccespt(df91)

            Dim dr100() As DataRow = bv.Select("ss_categ='Self' and SS_QCODE='I315'")
            Dim mg100 As Double = 0
            Dim cnt100 As Integer = 0
            For Each hj In dr100
                cnt100 += 1
                mg100 += hj(6)
                Label107.Text = hj(1)
            Next
            Dim df120 As String = Math.Round((mg100 / cnt100), 2)
            lblResSelf.Text = Val(lblResSelf.Text) + Val(df120)
            Label108.Text = checkAccespt(df120)

            Dim dr130() As DataRow = bv.Select("ss_categ='PEER' and SS_QCODE='I315'")
            Dim mg130 As Double = 0
            Dim cnt130 As Integer = 0
            For Each hj In dr130
                cnt130 += 1
                mg130 += hj(7)
                Label107.Text = hj(1)
            Next
            Dim df130 As String = Math.Round((mg130 / cnt130), 2)
            lblResPeer.Text = Val(lblResPeer.Text) + Val(df130)
            Label110.Text = checkAccespt(df130)

            Label112.Text = checkAccespt(Math.Round(((((mgr91 / cnt91) + (intsh18 / cnt18) + (mg130 / cnt130)) / 3)), 2))

            lblResMangr.Text = Math.Round(Val(lblResMangr.Text) / 4, 2)
            lblResIntsh.Text = Math.Round(Val(lblResIntsh.Text) / 4, 2)
            lblResPeer.Text = Math.Round(Val(lblResPeer.Text) / 4, 2)
            lblResSelf.Text = Math.Round(Val(lblResSelf.Text) / 4, 2)
            Label9.Text = checkAccespt1(lblResMangr.Text)
            Label10.Text = checkAccespt1(lblResIntsh.Text)
            Label85.Text = checkAccespt1(lblResPeer.Text)
            Label8.Text = checkAccespt1(lblResSelf.Text)

            Label11.Text = checkAccespt1(Math.Round(((Val(lblResMangr.Text) + Val(lblResIntsh.Text) + Val(lblResPeer.Text)) / 3), 2))
            lblresponse.Text = checkAccespt(Math.Round(((Val(lblResMangr.Text) + Val(lblResIntsh.Text) + Val(lblResPeer.Text)) / 3), 2))

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
                    rn = "Unacceptable"
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

    Public Sub displayAns(pno As String, yer As String, cyc As String, levl As String)
        Try
            Dim str As String = String.Empty
            str = " select distinct ss_categ,SS_Q2_A from t_survey_status df , t_survey_response fd where  df.ss_pno=fd.ss_pno and df.ss_asses_pno=fd.ss_asses_pno and fd.ss_serial = df.ss_srlno  and fd.ss_year = df.ss_year and df.ss_asses_pno='" & pno & "'   and df.ss_year='" & yer.ToString & "' and df.ss_srlno='" & cyc.ToString & "' and df.SS_RPT_FLAG='Y'"
            Dim hg = GetData(str, conHrps)
            Dim hf() As DataRow = hg.Select("ss_categ='Self'")
            For Each tg As DataRow In hf
                litans.Text = " <h3> <b>Self </b> </h3> <br/>" & tg(1).ToString() & ""
            Next
            litans.Text += "<br /> <br /><h3> <b> Other respondents </b></h3> <br />"
            Dim jh() As DataRow = hg.Select("ss_categ<>'Self'")
            For Each tg1 As DataRow In jh
                litans.Text += " <p>" & tg1(1).ToString() & "</p>"
            Next
            'If hg.Rows.Count > 0 Then
            '    For i = 0 To hg.Rows.Count - 1
            '        litans.Text += " <p>" & hg.Rows(i)(1).ToString() & "</p>"
            '    Next
            'End If

            Dim str1 As String = String.Empty

            str1 = "select distinct df.ss_categ, df.SS_Q2_b from t_survey_status df , t_survey_response fd where  df.ss_pno=fd.ss_pno and df.ss_asses_pno=fd.ss_asses_pno  and fd.ss_serial = df.ss_srlno  and fd.ss_year = df.ss_year and df.ss_asses_pno='" & pno & "'  and df.ss_year='" & yer.ToString & "' and df.ss_srlno='" & cyc.ToString & "' and df.SS_RPT_FLAG='Y'"
            Dim hg1 = GetData(str1, conHrps)
            Dim hf1() As DataRow = hg1.Select("ss_categ='Self'")
            For Each tg1 As DataRow In hf1
                Literal1.Text = " <h3> <b>Self </b> </h3> <br/>" & tg1(1).ToString() & ""
            Next
            Literal1.Text += "<br /> <br /><h3> <b> Other respondents </b></h3> <br />"
            Dim jh1() As DataRow = hg1.Select("ss_categ<>'Self'")
            For Each tg1 As DataRow In jh1
                Literal1.Text += " <p>" & tg1(1).ToString() & "</p>"
            Next
            'If hg1.Rows.Count > 0 Then
            '    For i = 0 To hg1.Rows.Count - 1
            '        Literal1.Text += " <p>" & hg1.Rows(i)(1).ToString() & "</p>"
            '    Next
            'End If
        Catch ex As Exception
            Dim h = ex.ToString
        End Try
    End Sub
    Public Sub displayOverall(pno As String, yer As String, cyc As String, levl As String)
        Try
            'Added by TCS on 310122 (To separate code for year and cycle greater than 2022, 1)
            Dim yearCyle As String = yer + cyc
            Dim Str As String = ""
            If yearCyle > 20221 Then
                Str += " Select * from hrps.V_ALL_I1_I6_T_NEW  where EMA_PERNO = '" + pno + "' and ss_year =" & yer.ToString & " and ss_srlno =" & cyc.ToString & "  ORDER BY 1 "
            Else
                Str += " Select * from hrps.V_ALL_I1_I6_T  where EMA_PERNO = '" + pno + "' and ss_year =" & yer.ToString & " and ss_srlno =" & cyc.ToString & "  ORDER BY 1 "
            End If
            'End

            Dim cmd As New OracleCommand()
            Dim dt1000 = GetData(Str, conHrps)
            If dt1000.Rows.Count > 0 Then
                Dim drSelf() As DataRow = dt1000.Select("ss_categ='Self'")
                For Each tdSelf As DataRow In drSelf
                    lblaccself.Text = tdSelf("AC").ToString()
                    Label4.Text = tdSelf("COL").ToString()
                    Label8.Text = tdSelf("RES").ToString()
                    Label12.Text = tdSelf("TEAM").ToString()
                Next

                Dim drMangr() As DataRow = dt1000.Select("ss_categ='MANGR'")
                For Each tdMangr As DataRow In drMangr
                    Label1.Text = tdMangr("AC").ToString()
                    Label5.Text = tdMangr("COL").ToString()
                    Label9.Text = tdMangr("RES").ToString()
                    Label13.Text = tdMangr("TEAM").ToString()
                Next

                Dim drPeer() As DataRow = dt1000.Select("ss_categ='PEER'")
                For Each tdPeer As DataRow In drPeer
                    Label83.Text = tdPeer("AC").ToString()
                    Label84.Text = tdPeer("COL").ToString()
                    Label85.Text = tdPeer("RES").ToString()
                    Label86.Text = tdPeer("TEAM").ToString()
                Next

                Dim drRopt() As DataRow = dt1000.Select("ss_categ='ROPT'")
                For Each tdRopt As DataRow In drRopt
                    If levl = "I2" Then
                        lblOverallPeerHeading.Text = "Peer"
                        tdOvaPeer.Attributes.Add("style", "width:70px;text-align:center;")
                        tdOvaSub.Attributes.Add("style", "width:110px;text-align:center;")
                    ElseIf levl = "I1" Then
                        lblOverallPeerHeading.Text = "Peer"
                        tdOvaIntsh.Visible = False
                    End If
                    tdOvaSub.Visible = True
                    tdOvaAcc.Visible = True
                    tdOvaColl.Visible = True
                    tdOvaRes.Visible = True
                    tdOvaTeam.Visible = True
                    lbltdOvaAcc.Text = tdRopt("AC").ToString()
                    lbltdOvaColl.Text = tdRopt("COL").ToString()
                    lbltdOvaRes.Text = tdRopt("RES").ToString()
                    lbltdOvaTeam.Text = tdRopt("TEAM").ToString()
                Next

                If levl <> "I1" Then
                    Dim drIntsh() As DataRow = dt1000.Select("ss_categ='INTSH'")
                    For Each tdIntsh As DataRow In drIntsh
                        Label2.Text = tdIntsh("AC").ToString()
                        Label6.Text = tdIntsh("COL").ToString()
                        Label10.Text = tdIntsh("RES").ToString()
                        Label14.Text = tdIntsh("TEAM").ToString()
                    Next
                Else
                    tdOvaIntsh.Visible = False
                    tdOvaAcc1.Visible = False
                    tdOvaColl1.Visible = False
                    tdOvaRes1.Visible = False
                    tdOvaTeam1.Visible = False
                End If


                Dim drOverall() As DataRow = dt1000.Select("ss_categ='Z-OVERALL'")
                For Each tdOverall As DataRow In drOverall
                    Label3.Text = tdOverall("AC").ToString()
                    Label7.Text = tdOverall("COL").ToString()
                    Label11.Text = tdOverall("RES").ToString()
                    Label15.Text = tdOverall("TEAM").ToString()

                    lblaccountability.Text = tdOverall("AC").ToString()
                    lblcollaboration.Text = tdOverall("COL").ToString()
                    lblresponse.Text = tdOverall("RES").ToString()
                    lblpeople.Text = tdOverall("TEAM").ToString()
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub displayOverall_Cyc1(pno As String, yer As String, cyc As String, levl As String)
        Try
            Dim Str As String = ""
            Str += " Select * from hrps.V_ALL_I3_I6_T_CYC1  where EMA_PERNO = '" + pno + "' and ss_year =" & yer.ToString & " and ss_srlno =" & cyc.ToString & "  ORDER BY 1 "

            Dim cmd As New OracleCommand()
            Dim dt1000 = GetData(Str, conHrps)
            If dt1000.Rows.Count > 0 Then
                Dim drSelf() As DataRow = dt1000.Select("ss_categ='Self'")
                For Each tdSelf As DataRow In drSelf
                    lblaccself.Text = tdSelf("AC").ToString()
                    Label4.Text = tdSelf("COL").ToString()
                    Label8.Text = tdSelf("RES").ToString()
                    Label12.Text = tdSelf("TEAM").ToString()
                Next

                Dim drMangr() As DataRow = dt1000.Select("ss_categ='MANGR'")
                For Each tdMangr As DataRow In drMangr
                    Label1.Text = tdMangr("AC").ToString()
                    Label5.Text = tdMangr("COL").ToString()
                    Label9.Text = tdMangr("RES").ToString()
                    Label13.Text = tdMangr("TEAM").ToString()
                Next

                Dim drPeer() As DataRow = dt1000.Select("ss_categ='PEER'")
                For Each tdPeer As DataRow In drPeer
                    Label83.Text = tdPeer("AC").ToString()
                    Label84.Text = tdPeer("COL").ToString()
                    Label85.Text = tdPeer("RES").ToString()
                    Label86.Text = tdPeer("TEAM").ToString()
                Next

                Dim drRopt() As DataRow = dt1000.Select("ss_categ='ROPT'")
                For Each tdRopt As DataRow In drRopt
                    tdOvaSub.Visible = True
                    tdOvaAcc.Visible = True
                    tdOvaColl.Visible = True
                    tdOvaRes.Visible = True
                    tdOvaTeam.Visible = True
                    lbltdOvaAcc.Text = tdRopt("AC").ToString()
                    lbltdOvaColl.Text = tdRopt("COL").ToString()
                    lbltdOvaRes.Text = tdRopt("RES").ToString()
                    lbltdOvaTeam.Text = tdRopt("TEAM").ToString()
                Next

                Dim drIntsh() As DataRow = dt1000.Select("ss_categ='INTSH'")
                For Each tdIntsh As DataRow In drIntsh
                    Label2.Text = tdIntsh("AC").ToString()
                    Label6.Text = tdIntsh("COL").ToString()
                    Label10.Text = tdIntsh("RES").ToString()
                    Label14.Text = tdIntsh("TEAM").ToString()
                Next


                Dim drOverall() As DataRow = dt1000.Select("ss_categ='Z-OVERALL'")
                For Each tdOverall As DataRow In drOverall
                    Label3.Text = tdOverall("AC").ToString()
                    Label7.Text = tdOverall("COL").ToString()
                    Label11.Text = tdOverall("RES").ToString()
                    Label15.Text = tdOverall("TEAM").ToString()

                    lblaccountability.Text = tdOverall("AC").ToString()
                    lblcollaboration.Text = tdOverall("COL").ToString()
                    lblresponse.Text = tdOverall("RES").ToString()
                    lblpeople.Text = tdOverall("TEAM").ToString()
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ViewA11(pno As String, yer As String, cyc As String, levl As String)
        Try
            If levl = "I1" Then
                HdAccIntshVal.Visible = False
                HdIntshSubVal.Visible = False
                HdResIntshVal.Visible = False
                HdTeamIntshVal.Visible = False

                tdAccIntshVal1.Visible = False
                tdAccIntshVal2.Visible = False
                tdAccIntshVal3.Visible = False
                tdAccIntshVal4.Visible = False

                tdColIntshVal1.Visible = False
                tdColIntshVal2.Visible = False
                tdColIntshVal3.Visible = False
                tdColIntshVal4.Visible = False

                tdResIntshVal1.Visible = False
                tdResIntshVal2.Visible = False
                tdResIntshVal3.Visible = False
                tdResIntshVal4.Visible = False

                tdTeamIntshVal1.Visible = False
                tdTeamIntshVal2.Visible = False
                tdTeamIntshVal3.Visible = False
                tdTeamIntshVal4.Visible = False
            End If

            'Added by TCS on 310122 (To separate code for year and cycle greater than 2022, 1)
            Dim yearCyle As String = yer + cyc
            Dim Str As String = ""
            If yearCyle > 20221 Then
                Str += " Select * from hrps.V_ALL_QUESTIONS_T_NEW  where EMA_PERNO = '" + pno + "' and ss_year =" & yer.ToString & " and ss_srlno =" & cyc.ToString & "  ORDER BY ss_desc,ss_categ,ss_qcode "
            Else
                Str += " Select * from hrps.V_ALL_QUESTIONS_T  where EMA_PERNO = '" + pno + "' and ss_year =" & yer.ToString & " and ss_srlno =" & cyc.ToString & "  ORDER BY ss_desc,ss_categ,ss_qcode "
            End If
            'End

            Dim cmd As New OracleCommand()
            Dim dtAll = GetData(Str, conHrps)
            If dtAll.Rows.Count > 0 Then

                'Accountibility(Self)
                Dim drAccSelf() As DataRow = dtAll.Select("ss_desc='Accountability' and ss_categ='Self'")
                Dim dtAccSelf As New DataTable
                dtAccSelf.Columns.Add("ss_qcode", GetType(String))
                dtAccSelf.Columns.Add("AC", GetType(String))
                For Each tdSelf As DataRow In drAccSelf
                    dtAccSelf.Rows.Add(tdSelf(7).ToString, tdSelf(16).ToString)
                Next

                If dtAccSelf.Rows.Count > 0 Then
                    Label32.Text = dtAccSelf.Rows(0)("ss_qcode").ToString
                    Label33.Text = dtAccSelf.Rows(1)("ss_qcode").ToString
                    Label34.Text = dtAccSelf.Rows(2)("ss_qcode").ToString
                    Label34.Text = dtAccSelf.Rows(3)("ss_qcode").ToString

                    Label16.Text = dtAccSelf.Rows(0)("AC").ToString
                    Label20.Text = dtAccSelf.Rows(1)("AC").ToString
                    Label24.Text = dtAccSelf.Rows(2)("AC").ToString
                    Label28.Text = dtAccSelf.Rows(3)("AC").ToString
                End If

                'Accountibility(Manger)
                Dim drAccManger() As DataRow = dtAll.Select("ss_desc='Accountability' and ss_categ='MANGR'")
                Dim dtAccManger As New DataTable
                dtAccManger.Columns.Add("ss_qcode", GetType(String))
                dtAccManger.Columns.Add("AC", GetType(String))
                For Each tdManager As DataRow In drAccManger
                    dtAccManger.Rows.Add(tdManager(7).ToString, tdManager(16).ToString)
                Next
                If dtAccManger.Rows.Count > 0 Then
                    Label32.Text = dtAccManger.Rows(0)("ss_qcode").ToString
                    Label33.Text = dtAccManger.Rows(1)("ss_qcode").ToString
                    Label34.Text = dtAccManger.Rows(2)("ss_qcode").ToString
                    Label35.Text = dtAccManger.Rows(3)("ss_qcode").ToString

                    Label17.Text = dtAccManger.Rows(0)("AC").ToString
                    Label21.Text = dtAccManger.Rows(1)("AC").ToString
                    Label25.Text = dtAccManger.Rows(2)("AC").ToString
                    Label29.Text = dtAccManger.Rows(3)("AC").ToString
                End If

                'Accountibility(Peer)
                Dim drAccPeer() As DataRow = dtAll.Select("ss_desc='Accountability' and ss_categ='PEER'")
                Dim dtAccPeer As New DataTable
                dtAccPeer.Columns.Add("ss_qcode", GetType(String))
                dtAccPeer.Columns.Add("AC", GetType(String))
                For Each tdPeer As DataRow In drAccPeer
                    dtAccPeer.Rows.Add(tdPeer(7).ToString, tdPeer(16).ToString)
                Next
                If dtAccPeer.Rows.Count > 0 Then
                    Label32.Text = dtAccPeer.Rows(0)("ss_qcode").ToString
                    Label33.Text = dtAccPeer.Rows(1)("ss_qcode").ToString
                    Label34.Text = dtAccPeer.Rows(2)("ss_qcode").ToString
                    Label35.Text = dtAccPeer.Rows(3)("ss_qcode").ToString
                    If levl = "I2" Or levl = "I1" Then
                        hdAccPeer.Text = "Peer"
                    End If

                    Label96.Text = dtAccPeer.Rows(0)("AC").ToString
                    Label97.Text = dtAccPeer.Rows(1)("AC").ToString
                    Label98.Text = dtAccPeer.Rows(2)("AC").ToString
                    Label100.Text = dtAccPeer.Rows(3)("AC").ToString
                End If

                If levl = "I2" Or levl = "I1" Then
                    'Accountibility(ROPT)
                    HdAccSubVal.Visible = True
                    hdAccSubordinates.Visible = True
                    tdAccSubVal1.Visible = True
                    tdAccSubVal2.Visible = True
                    tdAccSubVal3.Visible = True
                    tdAccSubVal4.Visible = True
                    lblAccSubVal1.Visible = True
                    lblAccSubVal2.Visible = True
                    lblAccSubVal3.Visible = True
                    lblAccSubVal4.Visible = True


                    Dim drAccRopt() As DataRow = dtAll.Select("ss_desc='Accountability' and ss_categ='ROPT'")
                    Dim dtAccRopt As New DataTable
                    dtAccRopt.Columns.Add("ss_qcode", GetType(String))
                    dtAccRopt.Columns.Add("AC", GetType(String))
                    For Each tdRopt As DataRow In drAccRopt
                        dtAccRopt.Rows.Add(tdRopt(7).ToString, tdRopt(16).ToString)
                    Next
                    If dtAccRopt.Rows.Count > 0 Then
                        Label32.Text = dtAccRopt.Rows(0)("ss_qcode").ToString
                        Label33.Text = dtAccRopt.Rows(1)("ss_qcode").ToString
                        Label34.Text = dtAccRopt.Rows(2)("ss_qcode").ToString
                        Label35.Text = dtAccRopt.Rows(3)("ss_qcode").ToString

                        lblAccSubVal1.Text = dtAccRopt.Rows(0)("AC").ToString
                        lblAccSubVal2.Text = dtAccRopt.Rows(1)("AC").ToString
                        lblAccSubVal3.Text = dtAccRopt.Rows(2)("AC").ToString
                        lblAccSubVal4.Text = dtAccRopt.Rows(3)("AC").ToString
                    End If
                End If



                'Accountibility(Internal Statkeholder)
                Dim drAccIntsh() As DataRow = dtAll.Select("ss_desc='Accountability' and ss_categ='INTSH'")
                Dim dtAccIntsh As New DataTable
                dtAccIntsh.Columns.Add("ss_qcode", GetType(String))
                dtAccIntsh.Columns.Add("AC", GetType(String))
                For Each tdIntsh As DataRow In drAccIntsh
                    dtAccIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(16).ToString)
                Next
                If dtAccIntsh.Rows.Count > 0 Then
                    Label32.Text = dtAccIntsh.Rows(0)("ss_qcode").ToString
                    Label33.Text = dtAccIntsh.Rows(1)("ss_qcode").ToString
                    Label34.Text = dtAccIntsh.Rows(2)("ss_qcode").ToString
                    Label35.Text = dtAccIntsh.Rows(3)("ss_qcode").ToString

                    Label18.Text = dtAccIntsh.Rows(0)("AC").ToString
                    Label22.Text = dtAccIntsh.Rows(1)("AC").ToString
                    Label26.Text = dtAccIntsh.Rows(2)("AC").ToString
                    Label30.Text = dtAccIntsh.Rows(3)("AC").ToString
                End If

                'Accountibility(Overall)
                Dim drAccOverall() As DataRow = dtAll.Select("ss_desc='Accountability' and ss_categ='Z-OVERALL'")
                Dim dtAccOverall As New DataTable
                dtAccOverall.Columns.Add("ss_qcode", GetType(String))
                dtAccOverall.Columns.Add("AC", GetType(String))
                For Each tdOverall As DataRow In drAccOverall
                    dtAccOverall.Rows.Add(tdOverall(7).ToString, tdOverall(16).ToString)
                Next
                If dtAccOverall.Rows.Count > 0 Then
                    Label19.Text = dtAccOverall.Rows(0)("AC").ToString
                    Label23.Text = dtAccOverall.Rows(1)("AC").ToString
                    Label27.Text = dtAccOverall.Rows(2)("AC").ToString
                    Label31.Text = dtAccOverall.Rows(3)("AC").ToString
                End If



                'Collaboration(Self)
                Dim drCollSelf() As DataRow = dtAll.Select("ss_desc='Collaboration' and ss_categ='Self'")
                Dim dtCollSelf As New DataTable
                dtCollSelf.Columns.Add("ss_qcode", GetType(String))
                dtCollSelf.Columns.Add("AC", GetType(String))
                For Each tdSelf As DataRow In drCollSelf
                    dtCollSelf.Rows.Add(tdSelf(7).ToString, tdSelf(17).ToString)
                Next

                If dtCollSelf.Rows.Count > 0 Then
                    Label36.Text = dtCollSelf.Rows(0)("ss_qcode").ToString
                    Label41.Text = dtCollSelf.Rows(1)("ss_qcode").ToString
                    Label46.Text = dtCollSelf.Rows(2)("ss_qcode").ToString
                    Label101.Text = dtCollSelf.Rows(3)("ss_qcode").ToString

                    Label37.Text = dtCollSelf.Rows(0)("AC").ToString
                    Label42.Text = dtCollSelf.Rows(1)("AC").ToString
                    Label47.Text = dtCollSelf.Rows(2)("AC").ToString
                    Label102.Text = dtCollSelf.Rows(3)("AC").ToString
                End If

                'Collaboration(Manger)
                Dim drCollManger() As DataRow = dtAll.Select("ss_desc='Collaboration' and ss_categ='MANGR'")
                Dim dtCollManger As New DataTable
                dtCollManger.Columns.Add("ss_qcode", GetType(String))
                dtCollManger.Columns.Add("AC", GetType(String))
                For Each tdManager As DataRow In drCollManger
                    dtCollManger.Rows.Add(tdManager(7).ToString, tdManager(17).ToString)
                Next
                If dtCollManger.Rows.Count > 0 Then
                    Label36.Text = dtCollManger.Rows(0)("ss_qcode").ToString
                    Label41.Text = dtCollManger.Rows(1)("ss_qcode").ToString
                    Label46.Text = dtCollManger.Rows(2)("ss_qcode").ToString
                    Label101.Text = dtCollManger.Rows(3)("ss_qcode").ToString

                    Label38.Text = dtCollManger.Rows(0)("AC").ToString
                    Label43.Text = dtCollManger.Rows(1)("AC").ToString
                    Label48.Text = dtCollManger.Rows(2)("AC").ToString
                    Label103.Text = dtCollManger.Rows(3)("AC").ToString
                End If


                'Collaboration(Peer)
                Dim drCollPeer() As DataRow = dtAll.Select("ss_desc='Collaboration' and ss_categ='PEER'")
                Dim dtCollPeer As New DataTable
                dtCollPeer.Columns.Add("ss_qcode", GetType(String))
                dtCollPeer.Columns.Add("AC", GetType(String))
                For Each tdIntsh As DataRow In drCollPeer
                    dtCollPeer.Rows.Add(tdIntsh(7).ToString, tdIntsh(17).ToString)
                Next
                If dtCollPeer.Rows.Count > 0 Then
                    Label36.Text = dtCollPeer.Rows(0)("ss_qcode").ToString
                    Label41.Text = dtCollPeer.Rows(1)("ss_qcode").ToString
                    Label46.Text = dtCollPeer.Rows(2)("ss_qcode").ToString
                    Label101.Text = dtCollPeer.Rows(3)("ss_qcode").ToString
                    If levl = "I2" Or levl = "I1" Then
                        hdColPeer.Text = "Peer"
                    End If

                    Label93.Text = dtCollPeer.Rows(0)("AC").ToString
                    Label94.Text = dtCollPeer.Rows(1)("AC").ToString
                    Label95.Text = dtCollPeer.Rows(2)("AC").ToString
                    Label104.Text = dtCollPeer.Rows(3)("AC").ToString
                End If

                If levl = "I2" Or levl = "I1" Then
                    'Collaboration(Peer)
                    HdColSubVal.Visible = True
                    hdColSubordinates.Visible = True
                    tdColSubVal1.Visible = True
                    tdColSubVal2.Visible = True
                    tdColSubVal3.Visible = True
                    tdColSubVal4.Visible = True
                    lblColSubVal1.Visible = True
                    lblColSubVal2.Visible = True
                    lblColSubVal3.Visible = True
                    lblColSubVal4.Visible = True
                    Dim drCollRopt() As DataRow = dtAll.Select("ss_desc='Collaboration' and ss_categ='ROPT'")
                    Dim dtCollRopt As New DataTable
                    dtCollRopt.Columns.Add("ss_qcode", GetType(String))
                    dtCollRopt.Columns.Add("AC", GetType(String))
                    For Each tdRopt As DataRow In drCollRopt
                        dtCollRopt.Rows.Add(tdRopt(7).ToString, tdRopt(17).ToString)
                    Next
                    If dtCollRopt.Rows.Count > 0 Then
                        Label36.Text = dtCollRopt.Rows(0)("ss_qcode").ToString
                        Label41.Text = dtCollRopt.Rows(1)("ss_qcode").ToString
                        Label46.Text = dtCollRopt.Rows(2)("ss_qcode").ToString
                        Label101.Text = dtCollRopt.Rows(3)("ss_qcode").ToString

                        lblColSubVal1.Text = dtCollRopt.Rows(0)("AC").ToString
                        lblColSubVal2.Text = dtCollRopt.Rows(1)("AC").ToString
                        lblColSubVal3.Text = dtCollRopt.Rows(2)("AC").ToString
                        lblColSubVal4.Text = dtCollRopt.Rows(3)("AC").ToString
                    End If
                End If



                'Collaboration(Internal Statkeholder)
                Dim drCollIntsh() As DataRow = dtAll.Select("ss_desc='Collaboration' and ss_categ='INTSH'")
                Dim dtCollIntsh As New DataTable
                dtCollIntsh.Columns.Add("ss_qcode", GetType(String))
                dtCollIntsh.Columns.Add("AC", GetType(String))
                For Each tdIntsh As DataRow In drCollIntsh
                    dtCollIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(17).ToString)
                Next
                If dtCollIntsh.Rows.Count > 0 Then
                    Label36.Text = dtCollIntsh.Rows(0)("ss_qcode").ToString
                    Label41.Text = dtCollIntsh.Rows(1)("ss_qcode").ToString
                    Label46.Text = dtCollIntsh.Rows(2)("ss_qcode").ToString
                    Label101.Text = dtCollIntsh.Rows(3)("ss_qcode").ToString

                    Label39.Text = dtCollIntsh.Rows(0)("AC").ToString
                    Label44.Text = dtCollIntsh.Rows(1)("AC").ToString
                    Label49.Text = dtCollIntsh.Rows(2)("AC").ToString
                    Label105.Text = dtCollIntsh.Rows(3)("AC").ToString
                End If

                'Collaboration(Overal)
                Dim drCollOverall() As DataRow = dtAll.Select("ss_desc='Collaboration' and ss_categ='Z-OVERALL'")
                Dim dtCollOverall As New DataTable
                dtCollOverall.Columns.Add("ss_qcode", GetType(String))
                dtCollOverall.Columns.Add("AC", GetType(String))
                For Each tdOverall As DataRow In drCollOverall
                    dtCollOverall.Rows.Add(tdOverall(7).ToString, tdOverall(17).ToString)
                Next
                If dtCollOverall.Rows.Count > 0 Then
                    Label40.Text = dtCollOverall.Rows(0)("AC").ToString
                    Label45.Text = dtCollOverall.Rows(1)("AC").ToString
                    Label50.Text = dtCollOverall.Rows(2)("AC").ToString
                    Label106.Text = dtCollOverall.Rows(3)("AC").ToString
                End If



                'Responsiveness(Self)
                Dim drResSelf() As DataRow = dtAll.Select("ss_desc='Responsiveness' and ss_categ='Self'")
                Dim dtResSelf As New DataTable
                dtResSelf.Columns.Add("ss_qcode", GetType(String))
                dtResSelf.Columns.Add("AC", GetType(String))
                For Each tdSelf As DataRow In drResSelf
                    dtResSelf.Rows.Add(tdSelf(7).ToString, tdSelf(18).ToString)
                Next

                If dtResSelf.Rows.Count > 0 Then
                    Label68.Text = dtResSelf.Rows(0)("ss_qcode").ToString
                    Label73.Text = dtResSelf.Rows(1)("ss_qcode").ToString
                    Label78.Text = dtResSelf.Rows(2)("ss_qcode").ToString
                    Label107.Text = dtResSelf.Rows(3)("ss_qcode").ToString

                    Label69.Text = dtResSelf.Rows(0)("AC").ToString
                    Label74.Text = dtResSelf.Rows(1)("AC").ToString
                    Label79.Text = dtResSelf.Rows(2)("AC").ToString
                    Label108.Text = dtResSelf.Rows(3)("AC").ToString
                End If

                'Responsiveness(Manger)
                Dim drResManger() As DataRow = dtAll.Select("ss_desc='Responsiveness' and ss_categ='MANGR'")
                Dim dtResManger As New DataTable
                dtResManger.Columns.Add("ss_qcode", GetType(String))
                dtResManger.Columns.Add("AC", GetType(String))
                For Each tdManager As DataRow In drResManger
                    dtResManger.Rows.Add(tdManager(7).ToString, tdManager(18).ToString)
                Next
                If dtResManger.Rows.Count > 0 Then
                    Label68.Text = dtResManger.Rows(0)("ss_qcode").ToString
                    Label73.Text = dtResManger.Rows(1)("ss_qcode").ToString
                    Label78.Text = dtResManger.Rows(2)("ss_qcode").ToString
                    Label107.Text = dtResManger.Rows(3)("ss_qcode").ToString

                    Label70.Text = dtResManger.Rows(0)("AC").ToString
                    Label75.Text = dtResManger.Rows(1)("AC").ToString
                    Label80.Text = dtResManger.Rows(2)("AC").ToString
                    Label109.Text = dtResManger.Rows(3)("AC").ToString
                End If


                'Responsiveness(Peer)
                Dim drResPeer() As DataRow = dtAll.Select("ss_desc='Responsiveness' and ss_categ='PEER'")
                Dim dtResPeer As New DataTable
                dtResPeer.Columns.Add("ss_qcode", GetType(String))
                dtResPeer.Columns.Add("AC", GetType(String))
                For Each tdIntsh As DataRow In drResPeer
                    dtResPeer.Rows.Add(tdIntsh(7).ToString, tdIntsh(18).ToString)
                Next
                If dtResPeer.Rows.Count > 0 Then
                    Label68.Text = dtResPeer.Rows(0)("ss_qcode").ToString
                    Label73.Text = dtResPeer.Rows(1)("ss_qcode").ToString
                    Label78.Text = dtResPeer.Rows(2)("ss_qcode").ToString
                    Label107.Text = dtResPeer.Rows(3)("ss_qcode").ToString
                    If levl = "I2" Or levl = "I1" Then
                        hdResPeer.Text = "Peer"
                    End If
                    Label90.Text = dtResPeer.Rows(0)("AC").ToString
                    Label91.Text = dtResPeer.Rows(1)("AC").ToString
                    Label92.Text = dtResPeer.Rows(2)("AC").ToString
                    Label110.Text = dtResPeer.Rows(3)("AC").ToString
                End If


                If levl = "I2" Or levl = "I1" Then
                    'Collaboration(Peer)
                    HdResSubVal.Visible = True
                    hdResSubordinates.Visible = True
                    tdResSubVal1.Visible = True
                    tdResSubVal2.Visible = True
                    tdResSubVal3.Visible = True
                    tdResSubVal4.Visible = True
                    lblResSubVal1.Visible = True
                    lblResSubVal2.Visible = True
                    lblResSubVal3.Visible = True
                    lblResSubVal4.Visible = True
                    Dim drResRopt() As DataRow = dtAll.Select("ss_desc='Responsiveness' and ss_categ='ROPT'")
                    Dim dtResRopt As New DataTable
                    dtResRopt.Columns.Add("ss_qcode", GetType(String))
                    dtResRopt.Columns.Add("AC", GetType(String))
                    For Each tdRopt As DataRow In drResRopt
                        dtResRopt.Rows.Add(tdRopt(7).ToString, tdRopt(18).ToString)
                    Next
                    If dtResRopt.Rows.Count > 0 Then
                        Label68.Text = dtResRopt.Rows(0)("ss_qcode").ToString
                        Label73.Text = dtResRopt.Rows(1)("ss_qcode").ToString
                        Label78.Text = dtResRopt.Rows(2)("ss_qcode").ToString
                        Label107.Text = dtResRopt.Rows(3)("ss_qcode").ToString

                        lblResSubVal1.Text = dtResRopt.Rows(0)("AC").ToString
                        lblResSubVal2.Text = dtResRopt.Rows(1)("AC").ToString
                        lblResSubVal3.Text = dtResRopt.Rows(2)("AC").ToString
                        lblResSubVal4.Text = dtResRopt.Rows(3)("AC").ToString
                    End If
                End If


                'Responsiveness(Internal Statkeholder)
                Dim drResIntsh() As DataRow = dtAll.Select("ss_desc='Responsiveness' and ss_categ='INTSH'")
                Dim dtResIntsh As New DataTable
                dtResIntsh.Columns.Add("ss_qcode", GetType(String))
                dtResIntsh.Columns.Add("AC", GetType(String))
                For Each tdIntsh As DataRow In drResIntsh
                    dtResIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(18).ToString)
                Next
                If dtResIntsh.Rows.Count > 0 Then
                    Label68.Text = dtResIntsh.Rows(0)("ss_qcode").ToString
                    Label73.Text = dtResIntsh.Rows(1)("ss_qcode").ToString
                    Label78.Text = dtResIntsh.Rows(2)("ss_qcode").ToString
                    Label107.Text = dtResIntsh.Rows(3)("ss_qcode").ToString

                    Label71.Text = dtResIntsh.Rows(0)("AC").ToString
                    Label76.Text = dtResIntsh.Rows(1)("AC").ToString
                    Label81.Text = dtResIntsh.Rows(2)("AC").ToString
                    Label111.Text = dtResIntsh.Rows(3)("AC").ToString
                End If

                'Responsiveness(Overal)
                Dim drResOverall() As DataRow = dtAll.Select("ss_desc='Responsiveness' and ss_categ='Z-OVERALL'")
                Dim dtResOverall As New DataTable
                dtResOverall.Columns.Add("ss_qcode", GetType(String))
                dtResOverall.Columns.Add("AC", GetType(String))
                For Each tdOverall As DataRow In drResOverall
                    dtResOverall.Rows.Add(tdOverall(7).ToString, tdOverall(18).ToString)
                Next
                If dtResOverall.Rows.Count > 0 Then
                    Label72.Text = dtResOverall.Rows(0)("AC").ToString
                    Label77.Text = dtResOverall.Rows(1)("AC").ToString
                    Label82.Text = dtResOverall.Rows(2)("AC").ToString
                    Label112.Text = dtResOverall.Rows(3)("AC").ToString
                End If




                'Team(Self)
                Dim drTeamSelf() As DataRow = dtAll.Select("ss_desc='People Development' and ss_categ='Self'")
                Dim dtTeamSelf As New DataTable
                dtTeamSelf.Columns.Add("ss_qcode", GetType(String))
                dtTeamSelf.Columns.Add("AC", GetType(String))
                For Each tdSelf As DataRow In drTeamSelf
                    dtTeamSelf.Rows.Add(tdSelf(7).ToString, tdSelf(19).ToString)
                Next

                If dtTeamSelf.Rows.Count > 0 Then
                    Label51.Text = dtTeamSelf.Rows(0)("ss_qcode").ToString
                    Label56.Text = dtTeamSelf.Rows(1)("ss_qcode").ToString
                    Label61.Text = dtTeamSelf.Rows(2)("ss_qcode").ToString
                    Label113.Text = dtTeamSelf.Rows(3)("ss_qcode").ToString

                    Label52.Text = dtTeamSelf.Rows(0)("AC").ToString
                    Label57.Text = dtTeamSelf.Rows(1)("AC").ToString
                    Label62.Text = dtTeamSelf.Rows(2)("AC").ToString
                    Label114.Text = dtTeamSelf.Rows(3)("AC").ToString
                End If

                'Team(Manger)
                Dim drTeamManger() As DataRow = dtAll.Select("ss_desc='People Development' and ss_categ='MANGR'")
                Dim dtTeamManger As New DataTable
                dtTeamManger.Columns.Add("ss_qcode", GetType(String))
                dtTeamManger.Columns.Add("AC", GetType(String))
                For Each tdManager As DataRow In drTeamManger
                    dtTeamManger.Rows.Add(tdManager(7).ToString, tdManager(19).ToString)
                Next
                If dtTeamManger.Rows.Count > 0 Then
                    Label51.Text = dtTeamManger.Rows(0)("ss_qcode").ToString
                    Label56.Text = dtTeamManger.Rows(1)("ss_qcode").ToString
                    Label61.Text = dtTeamManger.Rows(2)("ss_qcode").ToString
                    Label113.Text = dtTeamManger.Rows(3)("ss_qcode").ToString

                    Label53.Text = dtTeamManger.Rows(0)("AC").ToString
                    Label58.Text = dtTeamManger.Rows(1)("AC").ToString
                    Label63.Text = dtTeamManger.Rows(2)("AC").ToString
                    Label115.Text = dtTeamManger.Rows(3)("AC").ToString
                End If


                'Team(Peer)
                Dim drTeamPeer() As DataRow = dtAll.Select("ss_desc='People Development' and ss_categ='PEER'")
                Dim dtTeamPeer As New DataTable
                dtTeamPeer.Columns.Add("ss_qcode", GetType(String))
                dtTeamPeer.Columns.Add("AC", GetType(String))
                For Each tdIntsh As DataRow In drTeamPeer
                    dtTeamPeer.Rows.Add(tdIntsh(7).ToString, tdIntsh(19).ToString)
                Next
                If dtTeamPeer.Rows.Count > 0 Then
                    Label51.Text = dtTeamPeer.Rows(0)("ss_qcode").ToString
                    Label56.Text = dtTeamPeer.Rows(1)("ss_qcode").ToString
                    Label61.Text = dtTeamPeer.Rows(2)("ss_qcode").ToString
                    Label113.Text = dtTeamPeer.Rows(3)("ss_qcode").ToString
                    If levl = "I2" Or levl = "I1" Then
                        hdTeamPeer.Text = "Peer"
                    End If
                    Label89.Text = dtTeamPeer.Rows(0)("AC").ToString
                    Label88.Text = dtTeamPeer.Rows(1)("AC").ToString
                    Label87.Text = dtTeamPeer.Rows(2)("AC").ToString
                    Label116.Text = dtTeamPeer.Rows(3)("AC").ToString
                End If


                If levl = "I2" Or levl = "I1" Then
                    'Team(Ropt)
                    HdTeamSubVal.Visible = True
                    hdTeamSubordinates.Visible = True
                    tdTeamSubVal1.Visible = True
                    tdTeamSubVal2.Visible = True
                    tdTeamSubVal3.Visible = True
                    tdTeamSubVal4.Visible = True

                    lblTeamSubVal1.Visible = True
                    lblTeamSubVal2.Visible = True
                    lblTeamSubVal3.Visible = True
                    lblTeamSubVal4.Visible = True
                    Dim drTeamRopt() As DataRow = dtAll.Select("ss_desc='People Development' and ss_categ='ROPT'")
                    Dim dtTeamRopt As New DataTable
                    dtTeamRopt.Columns.Add("ss_qcode", GetType(String))
                    dtTeamRopt.Columns.Add("AC", GetType(String))
                    For Each tdRopt As DataRow In drTeamRopt
                        dtTeamRopt.Rows.Add(tdRopt(7).ToString, tdRopt(19).ToString)
                    Next
                    If dtTeamRopt.Rows.Count > 0 Then
                        Label51.Text = dtTeamRopt.Rows(0)("ss_qcode").ToString
                        Label56.Text = dtTeamRopt.Rows(1)("ss_qcode").ToString
                        Label61.Text = dtTeamRopt.Rows(2)("ss_qcode").ToString
                        Label113.Text = dtTeamRopt.Rows(3)("ss_qcode").ToString

                        lblTeamSubVal1.Text = dtTeamRopt.Rows(0)("AC").ToString
                        lblTeamSubVal2.Text = dtTeamRopt.Rows(1)("AC").ToString
                        lblTeamSubVal3.Text = dtTeamRopt.Rows(2)("AC").ToString
                        lblTeamSubVal4.Text = dtTeamRopt.Rows(3)("AC").ToString
                    End If
                End If


                'Team(Internal Statkeholder)
                Dim drTeamIntsh() As DataRow = dtAll.Select("ss_desc='People Development' and ss_categ='INTSH'")
                Dim dtTeamIntsh As New DataTable
                dtTeamIntsh.Columns.Add("ss_qcode", GetType(String))
                dtTeamIntsh.Columns.Add("AC", GetType(String))
                For Each tdIntsh As DataRow In drTeamIntsh
                    dtTeamIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(19).ToString)
                Next
                If dtTeamIntsh.Rows.Count > 0 Then
                    Label51.Text = dtTeamIntsh.Rows(0)("ss_qcode").ToString
                    Label56.Text = dtTeamIntsh.Rows(1)("ss_qcode").ToString
                    Label61.Text = dtTeamIntsh.Rows(2)("ss_qcode").ToString
                    Label113.Text = dtTeamIntsh.Rows(3)("ss_qcode").ToString

                    Label54.Text = dtTeamIntsh.Rows(0)("AC").ToString
                    Label59.Text = dtTeamIntsh.Rows(1)("AC").ToString
                    Label64.Text = dtTeamIntsh.Rows(2)("AC").ToString
                    Label117.Text = dtTeamIntsh.Rows(3)("AC").ToString
                End If

                'Team(Overal)
                Dim drTeamOverall() As DataRow = dtAll.Select("ss_desc='People Development' and ss_categ='Z-OVERALL'")
                Dim dtTeamOverall As New DataTable
                dtTeamOverall.Columns.Add("ss_qcode", GetType(String))
                dtTeamOverall.Columns.Add("AC", GetType(String))
                For Each tdOverall As DataRow In drTeamOverall
                    dtTeamOverall.Rows.Add(tdOverall(7).ToString, tdOverall(19).ToString)
                Next
                If dtTeamOverall.Rows.Count > 0 Then
                    Label55.Text = dtTeamOverall.Rows(0)("AC").ToString
                    Label60.Text = dtTeamOverall.Rows(1)("AC").ToString
                    Label65.Text = dtTeamOverall.Rows(2)("AC").ToString
                    Label118.Text = dtTeamOverall.Rows(3)("AC").ToString
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Public Sub insertData(pno As String, yer As String, cyc As String, levl As String)
        Try
            Dim str As String = String.Empty

            'Check perno is user or admin

            str = " select * from hrps.t_feedback360_rpt_overall where  FRO_PERNO='" & pno & "'   and FRO_YEAR='" & yer.ToString & "' and FRO_CYCLE='" & cyc.ToString & "'"
            Dim gt = GetData(str, conHrps)
            If gt.Rows.Count <= 0 Then
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim qry As String = String.Empty
                'Added by TCS on 310122 (Add year and cycle in query)
                If Session("ADM_USER") Is Nothing Then
                    qry = "insert into hrps.t_feedback360_rpt_overall (select AR.ss_year,AR.ss_srlno,AR.ema_perno,AR.ema_ename,'" & levl & "',AR.ss_categ,A,C,R,T,AC,COL,RES,TEAM,AC,COL,RES,TEAM,'" & pno.ToString & "',SYSDATE,null,null from hrps.v_all_i1_i6_t AR where AR.ema_perno=:ema_perno and AR.ss_categ='Z-OVERALL' and AR.SS_YEAR='" & yer.ToString & "' and AR.SS_SRLNO='" & cyc.ToString & "')"
                Else
                    qry = "insert into hrps.t_feedback360_rpt_overall (select AR.ss_year,AR.ss_srlno,AR.ema_perno,AR.ema_ename,'" & levl & "',AR.ss_categ,A,C,R,T,AC,COL,RES,TEAM,AC,COL,RES,TEAM,'" & Session("ADM_USER").ToString & "',SYSDATE,null,null from hrps.v_all_i1_i6_t AR where AR.ema_perno=:ema_perno and AR.ss_categ='Z-OVERALL' and AR.SS_YEAR='" & yer.ToString & "' and AR.SS_SRLNO='" & cyc.ToString & "')"
                End If
                'End

                Dim comnd As New OracleCommand(qry, conHrps)
                comnd.Parameters.Clear()
                comnd.Parameters.AddWithValue("ema_perno", pno)
                comnd.ExecuteNonQuery()
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
End Class
