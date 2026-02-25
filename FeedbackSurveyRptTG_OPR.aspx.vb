Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Partial Class FeedbackSurveyRptTG_OPR
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub FeedbackSurveyRptTG_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim perno = SimpleCrypt(Request.QueryString("pno"))
        Dim yr = SimpleCrypt(Request.QueryString("yr"))
        Dim cycle = SimpleCrypt(Request.QueryString("cyc"))
        'perno = "149747"
        'perno = "121473"
        'Dim f = GetData("select ema_perno, ema_ename, ema_desgn_desc from TIPS.t_empl_all  where ema_perno='" & perno & "'", conHrps)
        If Session("ADM_USER") Is Nothing Then
            If perno = Page.User.Identity.Name.Split("\")(1) Then
                'If perno = "123444" Then
            Else
                    Dim mail = SimpleCrypt(Request.QueryString("id"))
                    If mail <> "" Then
                        Dim validPno = GetPno(mail, yr, cycle)
                        If perno <> validPno Then
                            Response.Write("<center> <b><I> You do not have access to view other reports</b></I></center>")
                            Me.Visible = False
                        End If
                    Else
                        Response.Write("<center> <b><I> You do not have access to view other reports</b></I></center>")
                        Me.Visible = False
                    End If
                End If
            End If
            'perno = "149747"
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
                If lvl = "TG" Then
                    divStart.Visible = True
                End If
                ViewA11(perno, yr, cycle)
                displayOverall(perno, yr, cycle, lvl)
                insertData(perno, yr, cycle, lvl)
            End If
            displayAns(perno, yr, cycle, lvl)
        End If
    End Sub
    Public Function GetPno(pernr As String, yr As String, cyc As String) As String
        Dim perno As String = ""
        Try
            Dim cm As New OracleCommand()
            Dim sql As String = String.Empty
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            sql = "  select ss_pno from t_survey_status"
            If pernr.Length > 6 Then
                pernr = pernr.ToUpper().Trim
                sql += " where TRIM(upper(SS_EMAIL))||SS_INTSH_OTP=:SS_INTSH_OTP and SS_SRLNO=:SS_SRLNO and SS_YEAR=:SS_YEAR and ss_categ='Self'"
            Else
                sql += " where upper(ss_pno) =:SS_INTSH_OTP and SS_SRLNO=:SS_SRLNO and SS_YEAR=:SS_YEAR"
            End If
            'Dim d = getRecordInDt(cm, conHrps)
            cm.Connection = conHrps
            cm.CommandText = sql
            cm.Parameters.Clear()
            cm.Parameters.Add(New OracleParameter(":SS_INTSH_OTP", pernr))
            cm.Parameters.Add(New OracleParameter(":SS_SRLNO", cyc.ToString()))
            cm.Parameters.Add(New OracleParameter(":SS_YEAR", yr.ToString()))
            Dim da As New OracleDataAdapter(cm)
            Dim d As New DataTable()
            da.Fill(d)
            If d.Rows.Count > 0 Then
                perno = d.Rows(0)("ss_pno").ToString()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If

        End Try

        Return perno
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
            qry = "select decode(ss_categ,'INTSH','External Stakeholder','PEER','Subordinates','MANGR','Manager',irc_desc) irc_desc, SS_CATEG,count(SS_CATEG)  from hrps.t_survey_status, hrps.t_ir_codes where ss_asses_pno='" & pno & "'  "
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
    Public Sub ViewA11(pno As String, yer As String, cyc As String)
        Try
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
                    Label35.Text = dtAccSelf.Rows(3)("ss_qcode").ToString

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

                    Label96.Text = dtAccPeer.Rows(0)("AC").ToString
                    Label97.Text = dtAccPeer.Rows(1)("AC").ToString
                    Label98.Text = dtAccPeer.Rows(2)("AC").ToString
                    Label100.Text = dtAccPeer.Rows(3)("AC").ToString
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

                    Label93.Text = dtCollPeer.Rows(0)("AC").ToString
                    Label94.Text = dtCollPeer.Rows(1)("AC").ToString
                    Label95.Text = dtCollPeer.Rows(2)("AC").ToString
                    Label104.Text = dtCollPeer.Rows(3)("AC").ToString
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

                    Label90.Text = dtResPeer.Rows(0)("AC").ToString
                    Label91.Text = dtResPeer.Rows(1)("AC").ToString
                    Label92.Text = dtResPeer.Rows(2)("AC").ToString
                    Label110.Text = dtResPeer.Rows(3)("AC").ToString
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

                    Label89.Text = dtTeamPeer.Rows(0)("AC").ToString
                    Label88.Text = dtTeamPeer.Rows(1)("AC").ToString
                    Label87.Text = dtTeamPeer.Rows(2)("AC").ToString
                    Label116.Text = dtTeamPeer.Rows(3)("AC").ToString
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
                'Added by TCS (Query modified for Year Cycle 2022, 2)
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
