Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Data
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.pdf.draw
Imports System.Drawing
Imports System.Reflection
Imports iTextSharp.text.html
Imports System.Data.OracleClient
Imports System.Configuration
Imports Font = iTextSharp.text.Font

Public Class ReportTG
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Public Sub GenerateReport(ByVal perNo As String, ByVal yr As String, ByVal cycle As String)

        Dim dob As New Dictionary(Of String, String)
        dob.Add("120273", "10081968")
        dob.Add("123418", "14121968")
        dob.Add("123444", "09101968")
        dob.Add("127721", "16101968")
        dob.Add("127982", "13071971")
        dob.Add("128001", "10111969")
        dob.Add("148303", "11071975")
        dob.Add("148607", "06051970")
        dob.Add("148668", "14081976")
        dob.Add("148736", "13071971")
        dob.Add("149946", "01041971")
        dob.Add("150767", "12061973")
        dob.Add("159505", "09011966")
        dob.Add("165044", "17041968")
        dob.Add("174411", "28081966")
        dob.Add("174780", "03011967")
        dob.Add("420018", "30061964")
        dob.Add("TG0001", "14101968")
        dob.Add("TG0002", "07021964")
        dob.Add("TG0003", "02121968")
        dob.Add("TG0004", "17111964")
        dob.Add("TG0005", "28071963")
        dob.Add("TG0007", "31121962")
        dob.Add("123595", "01121967")
        dob.Add("123774", "16051969")
        dob.Add("148491", "24101970")
        dob.Add("158078", "15071981")
        dob.Add("162938", "07061978")
        dob.Add("174950", "25061968")
        dob.Add("175110", "12061970")

        Dim name As String = String.Empty
        Dim designation As String = String.Empty
        Dim lvl As String = String.Empty
        Dim overAllReportPageHeader As String = String.Empty
        Dim qulaitiveCommentPageHeader As String = String.Empty
        Dim heading As String = String.Empty
        Dim responsecount As String = String.Empty, responsedetail As String = String.Empty, totalsurvey As String = String.Empty, completedsurvey As String = String.Empty
        Dim responsiveDt As New DataTable()
        Dim accountbltyDt As New DataTable()
        Dim collabrDt As New DataTable()
        Dim teamBuilDt As New DataTable()
        Dim overAllDt As New DataTable()
        Dim accountibiltyOverall As String = String.Empty, collaborationOverall As String = String.Empty, responsivnsOverall As String = String.Empty, teamBuildingOverall As String = String.Empty
        Dim dc As New OracleCommand()
        If cycle = "1" Then
            dc.CommandText = "select ema_perno, ema_ename, ema_desgn_desc,EMA_EQV_LEVEL from TIPS.t_empl_all  where ema_perno=:pno"
        Else
            dc.CommandText = "select ema_perno, ema_ename, ema_desgn_desc,EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where ema_perno=:pno and ema_year=:ema_year and ema_CYCLE=:ema_srlno"
        End If
        dc.Parameters.Clear()
        dc.Connection = conHrps
        dc.Parameters.AddWithValue("pno", perNo)
        If cycle <> "1" Then
            dc.Parameters.AddWithValue("ema_year", yr)
            dc.Parameters.AddWithValue("ema_srlno", cycle)
        End If
        Dim f = getDataInDt(dc)
        If f.Rows.Count > 0 Then
            name = f.Rows(0)(1).ToString()
            designation = f.Rows(0)(2).ToString()
            overAllReportPageHeader = "Overall report for " & f.Rows(0)(1).ToString()
            qulaitiveCommentPageHeader = "Qualitative comments for " & f.Rows(0)(1).ToString()
            lvl = f.Rows(0)(3).ToString()

            Dim qry As String = String.Empty
            qry = "select irc_desc, SS_CATEG,count(SS_CATEG)  from hrps.t_survey_status, hrps.t_ir_codes where ss_asses_pno='" & perNo & "'  "
            qry += "and upper(irc_code) = Upper(ss_categ) and irc_type='360RL' and ss_year='" & yr.ToString & "' and ss_srlno='" & cycle.ToString & "' and SS_APP_TAG ='AP' and SS_RPT_FLAG='Y'  and SS_Q2_A is not null group by SS_CATEG,irc_desc"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim df = GetData("select count(ss_pno) from t_survey_status where ss_asses_pno='" & perNo & "' and ss_year='" & yr.ToString & "' and ss_srlno='" & cycle.ToString & "' and SS_APP_TAG ='AP' ", conHrps)

            Dim qry1 As String = String.Empty
            Dim yrCyc As String = yr.Substring(2, 2).ToString() & "" & cycle.Trim().ToString
            qry1 = "SELECT IRC_DESC FROM HRPS.T_IR_CODES WHERE IRC_TYPE ='36PDF' AND IRC_CODE='" & yrCyc & "' AND IRC_VALID_TAG='A'"
            Dim d1 = GetData(qry1, conHrps)
            heading = d1.Rows(0)(0).ToString
            If Not d Is Nothing Then
                For g = 0 To d.Rows.Count - 1
                    re += d.Rows(g)(0) & " - " & d.Rows(g)(2) & ", "
                    cont = cont + d.Rows(g)(2)
                Next
                re = re.Trim
                re = re.TrimEnd(",")
                responsecount = cont
                responsedetail = re
                totalsurvey = df.Rows(0)(0)
                completedsurvey = cont
            End If
            Dim Str As String = ""
            Str += " Select * from hrps.V_ALL_QUESTIONS_T_NEW  where EMA_PERNO = '" + perNo + "' and ss_year =" & yr & " and ss_srlno =" & cycle & "  ORDER BY ss_desc,ss_categ,ss_qcode "
            Dim cmd As New OracleCommand()
            Dim dtAll = GetData(Str, conHrps)
            If dtAll.Rows.Count > 0 Then
                accountbltyDt = AccountibilityData(perNo, yr, cycle, dtAll, lvl)
                collabrDt = CollabarationData(perNo, yr, cycle, dtAll, lvl)
                responsiveDt = ResponsivenessData(perNo, yr, cycle, dtAll, lvl)
                teamBuilDt = TeamData(perNo, yr, cycle, dtAll, lvl)
                overAllDt = DisplayOverallData(perNo, yr, cycle, lvl, dtAll)

                accountibiltyOverall = overAllDt.Rows(0)("OVERALL")
                collaborationOverall = overAllDt.Rows(1)("OVERALL")
                responsivnsOverall = overAllDt.Rows(2)("OVERALL")
                teamBuildingOverall = overAllDt.Rows(3)("OVERALL")
            End If
        End If



        Dim fileName As String = perNo + "_FY" + Convert.ToString(Convert.ToInt32(yr.Substring(2, 2)) + 1) + "_Report.pdf"
        Dim doc As Document = New Document(PageSize.A4, 25.0F, 25.0F, 25.0F, 25.0F)

        Using ms As New MemoryStream()
            'Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
            'writer.SetEncryption(True, perNo, perNo, PdfWriter.ALLOW_SCREENREADERS)
            Dim writer As PdfWriter = PdfWriter.GetInstance(doc, ms)
            Dim password = dob.Where(Function(p) p.Key.Contains(perNo))
            writer.SetEncryption(True, password(0).Value, password(0).Value, PdfWriter.ALLOW_SCREENREADERS)
            doc.Open()
            writer.PageEvent = New MyFooter
            Dim cell As PdfPCell = Nothing
            Dim cell1 As PdfPCell = Nothing
            Dim cell2 As PdfPCell = Nothing
            Dim cell3 As PdfPCell = Nothing

            Dim glue As Chunk = New Chunk(New VerticalPositionMark())


            '#Region "Page-1"
            doc.Add(NewLine(1))
            Dim table2 = New PdfPTable(1)
            'table2.TotalWidth = 950.0F
            table2.TotalWidth = 1000.0F
            table2.LockedWidth = True
            table2.HorizontalAlignment = Element.ALIGN_CENTER
            'cell1 = ImageCell("Feedback360.jpg ", 95.0F, PdfPCell.ALIGN_CENTER)
            cell1 = ImageCell("Feedback360.jpg ", 95.0F, PdfPCell.ALIGN_CENTER)
            table2.AddCell(cell1)
            doc.Add(table2)

            Dim paragraph As Paragraph = New Paragraph()
            Dim ph1 As Phrase = New Phrase()
            Dim mm As Paragraph = New Paragraph()
            mm.SetLeading(0.0F, 1.8F)
            ph1.Add(New Chunk(Environment.NewLine))
            ph1.Add(New Chunk(name & vbLf, FontFactory.GetFont("calibri", 20, 0)))
            ph1.Add(New Chunk(designation & vbLf, FontFactory.GetFont("calibri", 16, 0)))
            ph1.Add(New Chunk(Environment.NewLine))
            ph1.Add(New Chunk("TATA STEEL" & vbLf, FontFactory.GetFont("calibri", 14, 0)))
            'ph1.Add(New Chunk(Environment.NewLine))
            ph1.Add(New Chunk(heading & vbLf, FontFactory.GetFont("calibri", 13, 0)))
            'ph1.Add(New Chunk(Environment.NewLine))
            ph1.Add(New Chunk(Environment.NewLine))
            mm.Add(ph1)
            paragraph.Add(mm)
            doc.Add(paragraph)

            Dim table4 = New PdfPTable(1)
            table4.TotalWidth = 590.0F
            table4.LockedWidth = True
            table4.AddCell(PhraseCell(New Phrase("CONFIDENTIAL And PROPRIETARY", FontFactory.GetFont("calibri", 10, 0)), PdfPCell.ALIGN_RIGHT))
            table4.AddCell(PhraseCell(New Phrase("Any use Of this material without specific permission Is strictly prohibited", FontFactory.GetFont("calibri", 10, 0)), PdfPCell.ALIGN_RIGHT))
            cell2 = PhraseCell(New Phrase(), PdfPCell.ALIGN_RIGHT)
            cell2.Colspan = 1
            table4.AddCell(cell2)

            doc.Add(table4)

            Dim jpg As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(System.AppDomain.CurrentDomain.BaseDirectory & "" & "images\Reportimage" & "\" & "Page2.PNG")
            jpg.ScaleToFit(3000, 770)
            jpg.Alignment = iTextSharp.text.Image.UNDERLYING
            jpg.SetAbsolutePosition(7, 69)
            '#End Region

            '#Region "Page-2"
            doc.NewPage()
            'doc.Add(jpg)

            'doc.NewPage()
            'Dim c1 = New Chunk(Environment.NewLine)
            'Dim c2 = New Chunk("Introduction" & vbLf, FontFactory.GetFont("Helvetica Neue", 18, 0))
            'Dim c3 = New Chunk(Environment.NewLine)
            'Dim c4 = New Chunk(New Chunk("We aspire to be the most valuable and respected steel company globally in the next 5-10 years" & vbLf, FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            'Dim c5 = New Chunk(Environment.NewLine)
            'Dim c6 = New Chunk(New Chunk("While we are constantly making changes across the organization, we are aware that to be prepared for the future, we need to increase this pace of change. We need to change the way in which we work and become more agile. The change that we are now embarking on as much ", FontFactory.GetFont("Helvetica Neue", 12)))
            'Dim c7 = New Chunk("a cultural transformation as a structural transformation." & vbLf, FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c8 = New Chunk(Environment.NewLine)
            'Dim c9 = New Chunk("We identified that we need to make the following 5 shifts in our culture:" & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c10 = New Chunk(Environment.NewLine)
            'Dim c11 = New Chunk("""My Tata Steel"" -> ""Our Tata Steel""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c12 = New Chunk(" – working together for a common goal" & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c13 = New Chunk(Environment.NewLine)
            'Dim c14 = New Chunk("""Looking up"" -> ""The buck stops here""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c15 = New Chunk(" – holding yourself accountable to your responsibilities" & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c16 = New Chunk(Environment.NewLine)
            'Dim c17 = New Chunk("""Incremental"" -> ""Bold""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c18 = New Chunk(" – pushing the boundaries of excellence" & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c19 = New Chunk(Environment.NewLine)
            'Dim c20 = New Chunk("""Activity"" -> ""Impact at speed""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c21 = New Chunk(" – having a bias to action" & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c22 = New Chunk(Environment.NewLine)
            'Dim c23 = New Chunk("""Paternalistic"" -> ""Meritocracy""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c24 = New Chunk(" – encouraging high performance" & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c25 = New Chunk(Environment.NewLine)
            'Dim c26 = New Chunk("These shifts require us to", FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c27 = New Chunk(" adopt new behaviours.", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c28 = New Chunk(" We have learnt from both research as well as experience of best-in-class organizations that this shift requires 4 things – role modelling by senior leaders, constant communication to enable understanding and conviction, capability building to help develop skills and process changes to help embed the new behaviours. A performance management system which is geared towards development and measures", FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c29 = New Chunk(" both the performance (what) and the behaviours (how)", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c30 = New Chunk(" will help us drive this", FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c31 = New Chunk(" cultural transformation." & vbLf, FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c32 = New Chunk(Environment.NewLine)
            'Dim c33 = New Chunk("4 behaviours have been identified", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c34 = New Chunk(" considering the cultural shifts articulated for agility, FGDs conducted by the culture labs in HR and the behaviours identified by some global companies which have already undertaken the journey to become agile." & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c35 = New Chunk(Environment.NewLine)
            'Dim c36 = New Chunk("These behaviours are", FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c37 = New Chunk(" ""Be Accountable""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c38 = New Chunk(" – ownership and accountability,", FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c39 = New Chunk(" ""Work Together""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c40 = New Chunk(" – collaboration,", FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c41 = New Chunk(" ""Respond Quickly""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c42 = New Chunk(" – responsiveness,", FontFactory.GetFont("Helvetica Neue", 12))
            'Dim c43 = New Chunk(" ""Unleash""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96)))
            'Dim c44 = New Chunk("  – people development" & vbLf, FontFactory.GetFont("Helvetica Neue", 12))
            'Dim phraseIntr = New Phrase()
            'phraseIntr.Add(c1)
            'phraseIntr.Add(c2)
            'phraseIntr.Add(c3)
            'phraseIntr.Add(c4)
            'phraseIntr.Add(c5)
            'phraseIntr.Add(c6)
            'phraseIntr.Add(c7)
            'phraseIntr.Add(c8)
            'phraseIntr.Add(c9)
            'phraseIntr.Add(c10)
            'phraseIntr.Add(c11)
            'phraseIntr.Add(c12)
            'phraseIntr.Add(c13)
            'phraseIntr.Add(c14)
            'phraseIntr.Add(c15)
            'phraseIntr.Add(c16)
            'phraseIntr.Add(c17)
            'phraseIntr.Add(c18)
            'phraseIntr.Add(c19)
            'phraseIntr.Add(c20)
            'phraseIntr.Add(c21)
            'phraseIntr.Add(c22)
            'phraseIntr.Add(c23)
            'phraseIntr.Add(c24)
            'phraseIntr.Add(c25)
            'phraseIntr.Add(c26)
            'phraseIntr.Add(c27)
            'phraseIntr.Add(c28)
            'phraseIntr.Add(c29)
            'phraseIntr.Add(c30)
            'phraseIntr.Add(c31)
            'phraseIntr.Add(c32)
            'phraseIntr.Add(c33)
            'phraseIntr.Add(c34)
            'phraseIntr.Add(c35)
            'phraseIntr.Add(c36)
            'phraseIntr.Add(c37)
            'phraseIntr.Add(c38)
            'phraseIntr.Add(c39)
            'phraseIntr.Add(c40)
            'phraseIntr.Add(c41)
            'phraseIntr.Add(c42)
            'phraseIntr.Add(c43)
            'phraseIntr.Add(c44)
            'Dim paragraphIntr = New Paragraph()
            'paragraphIntr.Alignment = Element.ALIGN_JUSTIFIED_ALL
            'paragraphIntr.Add(phraseIntr)
            'doc.Add(paragraphIntr)

            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("Introduction" & vbLf, FontFactory.GetFont("Helvetica Neue", 15, 1)))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("We aspire to be the most valuable and respected steel company globally in the next 5-10 years" & vbLf, FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("While we are constantly making changes across the organization, we are aware that to be prepared for the future, we need to increase this pace of change. We need to change the way in which we work and become more agile. The change that we are now embarking on as much ", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk("a cultural transformation as a structural transformation." & vbLf, FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("We identified that we need to make the following 5 shifts in our culture:" & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("""My Tata Steel"" -> ""Our Tata Steel""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – working together for a common goal" & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("""Looking up"" -> ""The buck stops here""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – holding yourself accountable to your responsibilities" & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("""Incremental"" -> ""Bold""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – pushing the boundaries of excellence" & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("""Activity"" -> ""Impact at speed""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – having a bias to action" & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("""Paternalistic"" -> ""Meritocracy""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – encouraging high performance" & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("These shifts require us to", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(" adopt new behaviours.", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" We have learnt from both research as well as experience of best-in-class organizations that this shift requires 4 things – role modelling by senior leaders, constant communication to enable understanding and conviction, capability building to help develop skills and process changes to help embed the new behaviours. A performance management system which is geared towards development and measures", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(" both the performance (what) and the behaviours (how)", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" will help us drive this", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(" cultural transformation." & vbLf, FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))

            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("4 behaviours have been identified", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" considering the cultural shifts articulated for agility, FGDs conducted by the culture labs in HR and the behaviours identified by some global companies which have already undertaken the journey to become agile." & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))

            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("These behaviours are", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(" ""Be Accountable""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – ownership and accountability,", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(" ""Work Together""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – collaboration,", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(" ""Respond Quickly""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk(" – responsiveness,", FontFactory.GetFont("Helvetica Neue", 12)))
            doc.Add(New Chunk(" ""Unleash""", FontFactory.GetFont("Helvetica Neue", 12, Font.BOLD, New iTextSharp.text.BaseColor(0, 41, 96))))
            doc.Add(New Chunk("  – people development" & vbLf, FontFactory.GetFont("Helvetica Neue", 12)))
            '#End Region

            '#Region "Page-3"
            doc.NewPage()
            doc.Add(New Chunk(Environment.NewLine))
            doc.Add(New Chunk("Selected behaviours For agility" & vbLf, FontFactory.GetFont("calibri", 15, 1)))
            'doc.Add(New Chunk(Environment.NewLine))
            Dim cell6 As PdfPCell = Nothing
            Dim table7 = New PdfPTable(1)
            table7.TotalWidth = 1000.0F
            table7.LockedWidth = True
            table7.HorizontalAlignment = Element.ALIGN_LEFT
            'cell6 = ImageCell("patch2.jpg ", 73.0F, PdfPCell.ALIGN_LEFT)
            cell6 = ImageCell("patch2.jpg ", 58.0F, PdfPCell.ALIGN_LEFT)
            table7.AddCell(cell6)
            doc.Add(table7)

            Dim cell7 As PdfPCell = Nothing
            Dim table8 = New PdfPTable(1)
            table8.TotalWidth = 1000.0F
            table8.LockedWidth = True
            table8.HorizontalAlignment = Element.ALIGN_LEFT
            'cell7 = ImageCell("patch3.jpg ", 73.0F, PdfPCell.ALIGN_LEFT)
            cell7 = ImageCell("patch3.jpg ", 58.0F, PdfPCell.ALIGN_LEFT)
            table8.AddCell(cell7)
            doc.Add(table8)
            Dim paragraph2 As Paragraph = New Paragraph()
            Dim prh2 As Phrase = New Phrase()
            Dim mm2 As Paragraph = New Paragraph()
            mm2.Add(prh2)
            paragraph2.Add(mm2)
            doc.Add(paragraph2)
            '#End Region

            '#Region "Page-4"
            doc.NewPage()

            Dim paragraphPage3 As Paragraph = New Paragraph()
            Dim phPage3 As Phrase = New Phrase()
            Dim mmPage3 As Paragraph = New Paragraph()

            phPage3.Add(New Chunk(Environment.NewLine))
            phPage3.Add(New Chunk("* Overall Score Does Not Include Self feedback Score" & vbLf, FontFactory.GetFont("calibri", 8, iTextSharp.text.Font.BOLDITALIC)))
            phPage3.Add(New Chunk("U - Unacceptable, A - Acceptable, G - Gold Standard" & vbLf, FontFactory.GetFont("calibri", 8, 0)))
            phPage3.Add(New Chunk(Environment.NewLine))

            phPage3.Add(New Chunk(Environment.NewLine))
            phPage3.Add(New Chunk(overAllReportPageHeader & vbLf, FontFactory.GetFont("calibri", 15, 1)))
            phPage3.Add(New Chunk(Environment.NewLine))
            phPage3.Add(New Chunk("Your behaviour score is based on the responses of ", FontFactory.GetFont("calibri", 9, 0)))
            phPage3.Add(New Chunk(responsecount + " individuals (" + responsedetail + ")" & vbLf, FontFactory.GetFont("calibri", 9, 1)))
            phPage3.Add(New Chunk("A total", FontFactory.GetFont("calibri", 9, 0)))
            phPage3.Add(New Chunk(" " + totalsurvey, FontFactory.GetFont("calibri", 9, 1)))
            phPage3.Add(New Chunk(" surveys were distributed. ", FontFactory.GetFont("calibri", 9, 0)))
            phPage3.Add(New Chunk(completedsurvey, FontFactory.GetFont("calibri", 9, 1)))
            phPage3.Add(New Chunk(" surveys were completed and have been included In this feedback report." & vbLf, FontFactory.GetFont("calibri", 9, 0)))
            phPage3.Add(New Chunk(Environment.NewLine))
            phPage3.Add(New Chunk(Environment.NewLine))
            phPage3.Add(New Chunk(Environment.NewLine))
            mmPage3.Add(phPage3)
            paragraphPage3.Add(mmPage3)
            doc.Add(paragraphPage3)
            Dim columnCount As Integer = IIf(lvl = "I2", 7, 6)
            Dim columnWidth As Single() = IIf(lvl = "I2", {4.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F}, {4.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F})
            Dim columnWidthOVerall As Single() = IIf(lvl = "I2", {2.5F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F}, {3.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F})
            Dim myColor1 As BaseColor = WebColors.GetRGBColor("#c1e6f8")
            Dim myColor2 As BaseColor = WebColors.GetRGBColor("#e6f4f7")
            Dim headerColor As BaseColor = WebColors.GetRGBColor("#00888C")
            Dim headerFontColor As BaseColor = WebColors.GetRGBColor("#FFFFFF")
            Dim table As PdfPTable = New PdfPTable(5)
            table.WidthPercentage = 100.0F
            table.SetWidths({4.0F, 2.0F, 2.0F, 2.0F, 2.0F})
            table.AddCell(GetCellForBorderedTable(New Phrase(" "), Element.ALIGN_LEFT, headerColor))
            table.AddCell(GetCellForBorderedTable(New Phrase("Number of 'G' on practices", FontFactory.GetFont("calibri", 12, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            table.AddCell(GetCellForBorderedTable(New Phrase("Number of 'A' on practices", FontFactory.GetFont("calibri", 12, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            table.AddCell(GetCellForBorderedTable(New Phrase("Number of 'U' on practices", FontFactory.GetFont("calibri", 12, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            table.AddCell(GetCellForBorderedTable(New Phrase("Overall behaviour standard", FontFactory.GetFont("calibri", 12, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))

            For i = 0 To overAllDt.Rows.Count - 1
                Dim myColor As BaseColor
                If (i + 1) Mod 2 = 0 Then
                    myColor = myColor1
                Else
                    myColor = myColor2
                End If
                table.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(overAllDt.Rows(i)("QUESTION"))), Element.ALIGN_LEFT, myColor))
                table.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(overAllDt.Rows(i)("G"))), Element.ALIGN_CENTER, myColor))
                table.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(overAllDt.Rows(i)("A"))), Element.ALIGN_CENTER, myColor))
                table.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(overAllDt.Rows(i)("U"))), Element.ALIGN_CENTER, myColor))
                table.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(overAllDt.Rows(i)("OVERALL"))), Element.ALIGN_CENTER, myColor))
            Next
            doc.Add(table)
            '#End Region

            '#Region "Page-5"
            doc.NewPage()
            Dim paragraphPage4 As Paragraph = New Paragraph()
            Dim phPage4 As Phrase = New Phrase()
            Dim mmPage4 As Paragraph = New Paragraph()
            'phPage4.Add(New Chunk(Environment.NewLine))
            phPage4.Add(New Chunk("* Overall Score Does Not Include Self feedback Score" & vbLf, FontFactory.GetFont("calibri", 8, iTextSharp.text.Font.BOLDITALIC)))
            phPage4.Add(New Chunk("U - Unacceptable, A - Acceptable, G - Gold Standard" & vbLf, FontFactory.GetFont("calibri", 8, 0)))
            phPage4.Add(New Chunk(Environment.NewLine))
            mmPage4.Add(phPage4)
            paragraphPage4.Add(mmPage4)
            doc.Add(paragraphPage4)

            Dim paragraphPage5 As Paragraph = New Paragraph()
            Dim phPage5 As Phrase = New Phrase()
            Dim mmPage5 As Paragraph = New Paragraph()
            phPage5.Add(New Chunk(Environment.NewLine))
            phPage5.Add(New Chunk(overAllReportPageHeader & vbLf, FontFactory.GetFont("calibri", 15, 1)))
            phPage5.Add(New Chunk(Environment.NewLine))
            mmPage5.Add(phPage5)
            paragraphPage5.Add(mmPage5)
            doc.Add(paragraphPage5)

            Dim paragraphPage6 As Paragraph = New Paragraph()
            Dim phPage6 As Phrase = New Phrase("Accountability     ", FontFactory.GetFont("calibri", 13, 1))
            Dim mmPage6 As Paragraph = New Paragraph()
            Dim chunk As Chunk = New Chunk(accountibiltyOverall & vbLf, FontFactory.GetFont("calibri", 13, 1))
            chunk.SetBackground(myColor1, 11, 4, 11, 4)
            phPage6.Add(chunk)


            phPage6.Add(New Chunk(Environment.NewLine))
            mmPage6.Add(phPage6)
            paragraphPage6.Add(mmPage6)
            doc.Add(paragraphPage6)

            Dim tablefd1 As PdfPTable = New PdfPTable(columnCount)
            tablefd1.WidthPercentage = 100.0F
            tablefd1.SetWidths(columnWidth)
            tablefd1.AddCell(GetCellForBorderedTable(New Phrase("GOLD STANDARD", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd1.AddCell(GetCellForBorderedTable(New Phrase("Self", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd1.AddCell(GetCellForBorderedTable(New Phrase("Manager", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd1.AddCell(GetCellForBorderedTable(New Phrase("Subordinates", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            If lvl <> "I1" Then
                tablefd1.AddCell(GetCellForBorderedTable(New Phrase("Internal Stakeholder", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            End If
            tablefd1.AddCell(GetCellForBorderedTable(New Phrase("Overall scores from different respondent category", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))

            For i = 0 To accountbltyDt.Rows.Count - 1
                Dim myColor As BaseColor
                If (i + 1) Mod 2 = 0 Then
                    myColor = myColor1
                Else
                    myColor = myColor2
                End If

                tablefd1.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(accountbltyDt.Rows(i)("QUESTION")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_LEFT, myColor))
                tablefd1.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(accountbltyDt.Rows(i)("SELF")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd1.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(accountbltyDt.Rows(i)("MANGR")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd1.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(accountbltyDt.Rows(i)("ROPT")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd1.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(accountbltyDt.Rows(i)("INTSH")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd1.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(accountbltyDt.Rows(i)("OVERALL")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
            Next
            doc.Add(tablefd1)

            doc.Add(NewLine(2))
            Dim paragraphPage7 As Paragraph = New Paragraph()
            Dim phPage7 As Phrase = New Phrase("Collaboration     ", FontFactory.GetFont("calibri", 13, 1))
            Dim mmPage7 As Paragraph = New Paragraph()
            Dim chunk7 As Chunk = New Chunk(collaborationOverall & vbLf, FontFactory.GetFont("calibri", 13, 1))
            chunk7.SetBackground(myColor1, 11, 4, 11, 4)
            phPage7.Add(chunk7)


            phPage7.Add(New Chunk(Environment.NewLine))
            mmPage7.Add(phPage7)
            paragraphPage7.Add(mmPage7)
            doc.Add(paragraphPage7)

            Dim tablefd2 As PdfPTable = New PdfPTable(columnCount)

            tablefd2.WidthPercentage = 100.0F
            tablefd2.SetWidths(columnWidth)
            tablefd2.AddCell(GetCellForBorderedTable(New Phrase("GOLD STANDARD", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd2.AddCell(GetCellForBorderedTable(New Phrase("Self", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd2.AddCell(GetCellForBorderedTable(New Phrase("Manager", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd2.AddCell(GetCellForBorderedTable(New Phrase("Subordinates", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            If lvl <> "I1" Then
                tablefd2.AddCell(GetCellForBorderedTable(New Phrase("Internal Stakeholder", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            End If
            tablefd2.AddCell(GetCellForBorderedTable(New Phrase("Overall scores from different respondent category", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))

            For i = 0 To collabrDt.Rows.Count - 1
                Dim myColor As BaseColor
                If (i + 1) Mod 2 = 0 Then
                    myColor = myColor1
                Else
                    myColor = myColor2
                End If
                tablefd2.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(collabrDt.Rows(i)("QUESTION")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_LEFT, myColor))
                tablefd2.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(collabrDt.Rows(i)("SELF")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd2.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(collabrDt.Rows(i)("MANGR")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd2.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(collabrDt.Rows(i)("ROPT")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd2.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(collabrDt.Rows(i)("INTSH")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd2.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(collabrDt.Rows(i)("OVERALL")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
            Next
            doc.Add(tablefd2)
            '#End Region

            '#Region "Page-6"
            doc.NewPage()

            Dim paragraphPage8 As Paragraph = New Paragraph()
            Dim phPage8 As Phrase = New Phrase()
            Dim mmPage8 As Paragraph = New Paragraph()
            'phPage8.Add(New Chunk(Environment.NewLine))
            phPage8.Add(New Chunk("* Overall Score Does Not Include Self feedback Score" & vbLf, FontFactory.GetFont("calibri", 8, iTextSharp.text.Font.BOLDITALIC)))
            phPage8.Add(New Chunk("U - Unacceptable, A - Acceptable, G - Gold Standard" & vbLf, FontFactory.GetFont("calibri", 8, 0)))
            phPage8.Add(New Chunk(Environment.NewLine))
            mmPage8.Add(phPage8)
            paragraphPage8.Add(mmPage8)
            doc.Add(paragraphPage8)
            doc.Add(paragraphPage5)

            Dim paragraphPage9 As Paragraph = New Paragraph()
            Dim phPage9 As Phrase = New Phrase("Responsiveness     ", FontFactory.GetFont("calibri", 13, 1))
            Dim mmPage9 As Paragraph = New Paragraph()
            Dim chunk9 As Chunk = New Chunk(responsivnsOverall & vbLf, FontFactory.GetFont("calibri", 13, 1))
            chunk9.SetBackground(myColor1, 11, 4, 11, 4)
            phPage9.Add(chunk9)
            phPage9.Add(New Chunk(Environment.NewLine))
            mmPage9.Add(phPage9)
            paragraphPage9.Add(mmPage9)
            doc.Add(paragraphPage9)

            Dim tablefd3 As PdfPTable = New PdfPTable(columnCount)

            tablefd3.WidthPercentage = 100.0F
            tablefd3.SetWidths(columnWidth)
            tablefd3.AddCell(GetCellForBorderedTable(New Phrase("GOLD STANDARD", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd3.AddCell(GetCellForBorderedTable(New Phrase("Self", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd3.AddCell(GetCellForBorderedTable(New Phrase("Manager", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd3.AddCell(GetCellForBorderedTable(New Phrase("Subordinates", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            If lvl <> "I1" Then
                tablefd3.AddCell(GetCellForBorderedTable(New Phrase("Internal Stakeholder", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            End If
            tablefd3.AddCell(GetCellForBorderedTable(New Phrase("Overall scores from different respondent category", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))

            For i = 0 To responsiveDt.Rows.Count - 1
                Dim myColor As BaseColor
                If (i + 1) Mod 2 = 0 Then
                    myColor = myColor1
                Else
                    myColor = myColor2
                End If
                tablefd3.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(responsiveDt.Rows(i)("QUESTION")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_LEFT, myColor))
                tablefd3.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(responsiveDt.Rows(i)("SELF")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd3.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(responsiveDt.Rows(i)("MANGR")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd3.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(responsiveDt.Rows(i)("ROPT")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd3.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(responsiveDt.Rows(i)("INTSH")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd3.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(responsiveDt.Rows(i)("OVERALL")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
            Next
            doc.Add(tablefd3)

            doc.Add(NewLine(2))

            Dim paragraphPage10 As Paragraph = New Paragraph()
            Dim phPage10 As Phrase = New Phrase("People Development     ", FontFactory.GetFont("calibri", 13, 1))
            Dim mmPage10 As Paragraph = New Paragraph()
            Dim chunk10 As Chunk = New Chunk(teamBuildingOverall & vbLf, FontFactory.GetFont("calibri", 13, 1))
            chunk10.SetBackground(myColor1, 11, 4, 11, 4)
            phPage10.Add(chunk10)
            phPage10.Add(New Chunk(Environment.NewLine))
            mmPage10.Add(phPage10)
            paragraphPage10.Add(mmPage10)
            doc.Add(paragraphPage10)

            Dim tablefd4 As PdfPTable = New PdfPTable(columnCount)
            tablefd4.WidthPercentage = 100.0F
            tablefd4.SetWidths(columnWidth)
            tablefd4.AddCell(GetCellForBorderedTable(New Phrase("GOLD STANDARD", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd4.AddCell(GetCellForBorderedTable(New Phrase("Self", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd4.AddCell(GetCellForBorderedTable(New Phrase("Manager", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            tablefd4.AddCell(GetCellForBorderedTable(New Phrase("Subordinates", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            If lvl <> "I1" Then
                tablefd4.AddCell(GetCellForBorderedTable(New Phrase("Internal Stakeholder", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))
            End If
            tablefd4.AddCell(GetCellForBorderedTable(New Phrase("Overall scores from different respondent category", FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD, headerFontColor)), Element.ALIGN_CENTER, headerColor))

            For i = 0 To teamBuilDt.Rows.Count - 1
                Dim myColor As BaseColor
                If (i + 1) Mod 2 = 0 Then
                    myColor = myColor1
                Else
                    myColor = myColor2
                End If
                tablefd4.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(teamBuilDt.Rows(i)("QUESTION")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_LEFT, myColor))
                tablefd4.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(teamBuilDt.Rows(i)("SELF")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd4.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(teamBuilDt.Rows(i)("MANGR")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd4.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(teamBuilDt.Rows(i)("ROPT")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd4.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(teamBuilDt.Rows(i)("INTSH")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
                tablefd4.AddCell(GetCellForBorderedTable(New Phrase(Convert.ToString(teamBuilDt.Rows(i)("OVERALL")), FontFactory.GetFont("calibri", 10)), Element.ALIGN_CENTER, myColor))
            Next

            doc.Add(tablefd4)

            '#End Region

            '#Region "Page-7"
            doc.NewPage()
            Dim pg1Page8 As Paragraph = New Paragraph()
            Dim ph1Page8 As Phrase = New Phrase()
            'ph1Page8.Add(New Chunk(Environment.NewLine))
            ph1Page8.Add(New Chunk(Environment.NewLine))
            ph1Page8.Add(New Chunk(qulaitiveCommentPageHeader & vbLf, FontFactory.GetFont("calibri", 15, 1)))
            ph1Page8.Add(New Chunk(Environment.NewLine))
            pg1Page8.Add(ph1Page8)
            doc.Add(pg1Page8)

            Dim table1Page8 = New PdfPTable(1)
            table1Page8.TotalWidth = 950.0F
            table1Page8.LockedWidth = True
            table1Page8.HorizontalAlignment = Element.ALIGN_CENTER
            'cell = ImageCell("Strength.png ", 115.0F, PdfPCell.ALIGN_CENTER)
            cell = ImageCell("Strength.jpg ", 76.0F, PdfPCell.ALIGN_CENTER)
            table1Page8.AddCell(cell)
            doc.Add(table1Page8)

            Dim pg3Page8 As Paragraph = New Paragraph()
            Dim ph3Page8 As Phrase = New Phrase()
            ph3Page8.Add(New Chunk("Self" & vbLf, FontFactory.GetFont("calibri", 13, iTextSharp.text.Font.BOLD)))
            Dim strengthQry As String = String.Empty
            strengthQry = " Select distinct ss_categ,SS_Q2_A from t_survey_status df , t_survey_response fd where  df.ss_pno=fd.ss_pno And df.ss_asses_pno=fd.ss_asses_pno And fd.ss_serial = df.ss_srlno  And fd.ss_year = df.ss_year And df.ss_asses_pno='" & perNo & "' and df.ss_year='" & yr & "' and df.ss_srlno='" & cycle & "' and df.SS_RPT_FLAG='Y'"
            Dim hg = GetData(strengthQry, conHrps)
            Dim hf() As DataRow = hg.Select("ss_categ='Self'")
            For Each tg As DataRow In hf
                ph3Page8.Add(New Chunk(tg(1).ToString() & vbLf, FontFactory.GetFont("calibri", 10, 0)))
            Next
            ph3Page8.Add(New Chunk(Environment.NewLine))
            ph3Page8.Add(New Chunk(Environment.NewLine))
            ph3Page8.Add(New Chunk("Other respondents" & vbLf, FontFactory.GetFont("calibri", 13, iTextSharp.text.Font.BOLD)))
            Dim jh() As DataRow = hg.Select("ss_categ<>'Self'")
            For Each tg1 As DataRow In jh
                ph3Page8.Add(New Chunk(tg1(1).ToString() & vbLf, FontFactory.GetFont("calibri", 10, 0)))
            Next
            pg3Page8.Add(ph3Page8)
            doc.Add(pg3Page8)

            '#End Region

            '#Region "Page-8"
            doc.NewPage()
            Dim pg1Page9 As Paragraph = New Paragraph()
            Dim ph1Page9 As Phrase = New Phrase()
            'ph1Page9.Add(New Chunk(Environment.NewLine))
            ph1Page9.Add(New Chunk(Environment.NewLine))
            ph1Page9.Add(New Chunk(qulaitiveCommentPageHeader & vbLf, FontFactory.GetFont("calibri", 15, 1)))
            ph1Page9.Add(New Chunk(Environment.NewLine))
            pg1Page9.Add(ph1Page9)
            doc.Add(pg1Page9)

            Dim table1Page9 = New PdfPTable(1)
            table1Page9.TotalWidth = 950.0F
            table1Page9.LockedWidth = True
            table1Page9.HorizontalAlignment = Element.ALIGN_CENTER
            'cell = ImageCell("Improvement.png", 115.0F, PdfPCell.ALIGN_CENTER)
            cell = ImageCell("Opprt.jpg ", 76.0F, PdfPCell.ALIGN_CENTER)
            table1Page9.AddCell(cell)
            doc.Add(table1Page9)

            Dim pg2Page9 As Paragraph = New Paragraph()
            Dim ph2Page9 As Phrase = New Phrase()
            ph2Page9.Add(New Chunk("Self" & vbLf, FontFactory.GetFont("calibri", 13, iTextSharp.text.Font.BOLD)))
            Dim improvementQry As String = String.Empty
            improvementQry = " select distinct df.ss_categ, df.SS_Q2_b from t_survey_status df , t_survey_response fd where  df.ss_pno=fd.ss_pno and df.ss_asses_pno=fd.ss_asses_pno and fd.ss_serial = df.ss_srlno  and fd.ss_year = df.ss_year and df.ss_asses_pno='" & perNo & "'  and df.ss_year='" & yr.ToString & "' and df.ss_srlno='" & cycle & "' and df.SS_RPT_FLAG='Y'"
            Dim hg1 = GetData(improvementQry, conHrps)
            Dim hf1() As DataRow = hg1.Select("ss_categ='Self'")
            For Each tg1 As DataRow In hf1
                ph2Page9.Add(New Chunk(tg1(1).ToString() & vbLf, FontFactory.GetFont("calibri", 10, 0)))
            Next
            ph2Page9.Add(New Chunk(Environment.NewLine))
            ph2Page9.Add(New Chunk(Environment.NewLine))
            ph2Page9.Add(New Chunk("Other respondents" & vbLf, FontFactory.GetFont("calibri", 13, iTextSharp.text.Font.BOLD)))
            Dim jh1() As DataRow = hg1.Select("ss_categ<>'Self'")
            For Each tg1 As DataRow In jh1
                ph2Page9.Add(New Chunk(tg1(1).ToString() & vbLf, FontFactory.GetFont("calibri", 10, 0)))
            Next

            pg2Page9.Add(ph2Page9)
            doc.Add(pg2Page9)

            '#End Region

            '#Region "Page-9"
            doc.NewPage()
            Dim pg1Page10 As Paragraph = New Paragraph()
            Dim ph1Page10 As Phrase = New Phrase()
            'ph1Page10.Add(New Chunk(Environment.NewLine))
            ph1Page10.Add(New Chunk(Environment.NewLine))
            ph1Page10.Add(New Chunk("Annexure- Sample Simulation of practice/behavior score FY24 End Year 360 report" & vbLf, FontFactory.GetFont("calibri", 15, 1)))
            ph1Page10.Add(New Chunk(Environment.NewLine))
            pg1Page10.Add(ph1Page10)
            doc.Add(pg1Page10)


            Dim pg2Page10 As Paragraph = New Paragraph()
            Dim ph2Page10 As Phrase = New Phrase()
            Dim items As String() = {"•   There are three categories of respondent:  Manager, Peers & Subordinates and Internal Stakeholder",
                                 "•   Each category of respondent has equal weightage.",
                                 "•   The overall behavior score is an outcome of scoring done by different respondent categories on practices."}
            For i As Integer = 0 To items.Length - 1
                ph2Page10.Add(New Chunk(items(i).ToString() & vbLf, FontFactory.GetFont("calibri", 10, 0)))
            Next
            pg2Page10.Add(ph2Page10)
            doc.Add(pg2Page10)



            Dim pg3Page10 As Paragraph = New Paragraph()
            Dim ph3Page10 As Phrase = New Phrase()
            ph3Page10.Add(New Chunk(Environment.NewLine))
            ph3Page10.Add(New Chunk(Environment.NewLine))
            ph3Page10.Add(New Chunk("Example 1" & vbLf, FontFactory.GetFont("calibri", 13, 1)))
            ph3Page10.Add(New Chunk(Environment.NewLine))
            pg3Page10.Add(ph3Page10)
            doc.Add(pg3Page10)


            Dim pg4Page10 As Paragraph = New Paragraph()
            Dim ph4Page10 As Phrase = New Phrase()
            Dim items1 As String() = {"•   Manager Score (M) (1 respondent)  - A",
                                 "•   Average Peer & Sub-ordinates Score (P) (10 respondents) - G",
                                 "•   Average Internal Stakeholder (I) (10 respondents) - G",
                                  "•   Overall Score (O) - A "}
            For i As Integer = 0 To items1.Length - 1
                ph4Page10.Add(New Chunk(items1(i).ToString() & vbLf, FontFactory.GetFont("calibri", 10, 0)))
            Next

            ph4Page10.Add(New Chunk(Environment.NewLine))
            ph4Page10.Add(New Chunk(Environment.NewLine))
            pg4Page10.Add(ph4Page10)
            doc.Add(pg4Page10)

            Dim table1Page10 = New PdfPTable(1)
            table1Page10.TotalWidth = 950.0F
            table1Page10.LockedWidth = True
            table1Page10.HorizontalAlignment = Element.ALIGN_CENTER
            cell = ImageCell("EX1IL3.jpg", 70.0F, PdfPCell.ALIGN_CENTER)
            table1Page10.AddCell(cell)
            doc.Add(table1Page10)

            Dim pg5Page10 As Paragraph = New Paragraph()
            Dim ph5Page10 As Phrase = New Phrase()
            ph5Page10.Add(New Chunk(Environment.NewLine))
            ph5Page10.Add(New Chunk(Environment.NewLine))
            ph5Page10.Add(New Chunk("Example 2" & vbLf, FontFactory.GetFont("calibri", 13, 1)))
            ph5Page10.Add(New Chunk(Environment.NewLine))
            pg5Page10.Add(ph5Page10)
            doc.Add(pg5Page10)


            Dim pg6Page10 As Paragraph = New Paragraph()
            Dim ph6Page10 As Phrase = New Phrase()
            Dim items2 As String() = {"•   Manager Score (M) (1 respondent) - A",
                                 "•   Average Peer & Sub-ordinates Score (P) (10 respondents) - G",
                                 "•   Average Internal Stakeholder (I) (10 respondents) - G",
                                 "•   Overall Score (O) - G"}
            For i As Integer = 0 To items2.Length - 1
                ph6Page10.Add(New Chunk(items2(i).ToString() & vbLf, FontFactory.GetFont("calibri", 10, 0)))
            Next

            ph6Page10.Add(New Chunk(Environment.NewLine))
            ph6Page10.Add(New Chunk(Environment.NewLine))
            pg6Page10.Add(ph6Page10)
            doc.Add(pg6Page10)

            Dim table2Page10 = New PdfPTable(1)
            table2Page10.TotalWidth = 950.0F
            table2Page10.LockedWidth = True
            table2Page10.HorizontalAlignment = Element.ALIGN_CENTER
            cell = ImageCell("EX2IL3.jpg", 70.0F, PdfPCell.ALIGN_CENTER)
            table2Page10.AddCell(cell)
            doc.Add(table2Page10)

            Dim pg7Page10 As Paragraph = New Paragraph()
            Dim ph7Page10 As Phrase = New Phrase()
            ph7Page10.Add(New Chunk(Environment.NewLine))
            ph7Page10.Add(New Chunk(Environment.NewLine))
            ph7Page10.Add(New Chunk("The final outcome i.e. overall score of a practice depends on whether average score given by one category of respondent is just crossing the Gold Standard range which means there are some respondents in that particular category who have rated acceptable OR in case the average score given by one category of respondent is towards extreme right of gold standard range which means majority of respondents in that category has rated as gold standard." & vbLf, FontFactory.GetFont("calibri", 10, 0)))

            pg7Page10.Add(ph7Page10)
            doc.Add(pg7Page10)

            Dim endParagraph As Paragraph = New Paragraph
            Dim endPhrase As Phrase = New Phrase
            endPhrase.Add(New Chunk("*** End of Report ***" & vbLf, FontFactory.GetFont("calibri", 10, iTextSharp.text.Font.BOLD)))
            endParagraph.Add(endPhrase)
            endParagraph.Alignment = 1
            doc.Add(NewLine(2))
            doc.Add(endParagraph)
            '#End Region

            doc.Close()
            Dim bytes As Byte() = ms.ToArray()
            ms.Close()
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "")
            HttpContext.Current.Response.ContentType = "application/pdf"
            HttpContext.Current.Response.Buffer = True
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            HttpContext.Current.Response.BinaryWrite(bytes)
            HttpContext.Current.Response.[End]()
            HttpContext.Current.Response.Close()
        End Using
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
    Private Function NewLine(n As Integer) As Paragraph
        Dim para As Paragraph = New Paragraph()
        Dim phrse1 As Phrase = New Phrase()
        Dim mmp As Paragraph = New Paragraph()
        For i As Integer = 0 To n
            phrse1.Add(New Chunk(Environment.NewLine))
        Next
        mmp.Add(phrse1)
        para.Add(mmp)
        Return para
    End Function
    Private Function PhraseCell(phrase As Phrase, align As Integer) As PdfPCell
        Dim cell As New PdfPCell(phrase)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 2.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
    Private Function ImageCell(path As String, scale As Single, align As Integer) As PdfPCell
        Dim phyPath As String = System.AppDomain.CurrentDomain.BaseDirectory & "" & "images\Reportimage"
        Dim DestinationLoc As String = phyPath & "\" & path
        Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(DestinationLoc)
        image.ScalePercent(scale)
        Dim cell As New PdfPCell(image)
        cell.BorderColor = BaseColor.WHITE
        cell.VerticalAlignment = PdfPCell.ALIGN_TOP
        cell.HorizontalAlignment = align
        cell.PaddingBottom = 0.0F
        cell.PaddingTop = 0.0F
        Return cell
    End Function
    Private Function GetCellForBorderedTable(ByVal phrase As Phrase, ByVal align As Integer, ByVal color As BaseColor) As PdfPCell
        Dim cell As PdfPCell = New PdfPCell(phrase)
        cell.HorizontalAlignment = align
        'cell.PaddingBottom = 10.0F
        'cell.PaddingTop = 18.0F
        cell.BackgroundColor = color
        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        'cell.HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED
        cell.MinimumHeight = 50.0F
        Return cell
    End Function

    Public Function AccountibilityData(ByVal perNo As String, ByVal year As String, ByVal cycle As String, ByVal allResponseData As DataTable, ByVal level As String) As DataTable
        Dim dtAccountibility As New DataTable
        dtAccountibility.Columns.Add("QUESTION", GetType(String))
        dtAccountibility.Columns.Add("SELF", GetType(String))
        dtAccountibility.Columns.Add("MANGR", GetType(String))
        dtAccountibility.Columns.Add("ROPT", GetType(String))
        dtAccountibility.Columns.Add("INTSH", GetType(String))
        dtAccountibility.Columns.Add("OVERALL", GetType(String))

        If allResponseData.Rows.Count > 0 Then
            'Accountibility(Self)
            Dim drAccSelf() As DataRow = allResponseData.Select("ss_desc='Accountability' and ss_categ='Self'")
            Dim dtAccSelf As New DataTable
            dtAccSelf.Columns.Add("ss_qcode", GetType(String))
            dtAccSelf.Columns.Add("AC", GetType(String))
            For Each tdSelf As DataRow In drAccSelf
                dtAccSelf.Rows.Add(tdSelf(7).ToString, tdSelf(16).ToString)
            Next

            'Accountibility(Manger)
            Dim drAccManger() As DataRow = allResponseData.Select("ss_desc='Accountability' and ss_categ='MANGR'")
            Dim dtAccManger As New DataTable
            dtAccManger.Columns.Add("ss_qcode", GetType(String))
            dtAccManger.Columns.Add("AC", GetType(String))
            For Each tdManager As DataRow In drAccManger
                dtAccManger.Rows.Add(tdManager(7).ToString, tdManager(16).ToString)
            Next



            'Accountibility(ROPT)
            Dim drAccRopt() As DataRow = allResponseData.Select("ss_desc='Accountability' and ss_categ='ROPT'")
            Dim dtAccRopt As New DataTable
            dtAccRopt.Columns.Add("ss_qcode", GetType(String))
            dtAccRopt.Columns.Add("AC", GetType(String))
            For Each tdRopt As DataRow In drAccRopt
                dtAccRopt.Rows.Add(tdRopt(7).ToString, tdRopt(16).ToString)
            Next


            'Accountibility(Internal Statkeholder)
            Dim drAccIntsh() As DataRow = allResponseData.Select("ss_desc='Accountability' and ss_categ='INTSH'")
            Dim dtAccIntsh As New DataTable
            dtAccIntsh.Columns.Add("ss_qcode", GetType(String))
            dtAccIntsh.Columns.Add("AC", GetType(String))
            For Each tdIntsh As DataRow In drAccIntsh
                dtAccIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(16).ToString)
            Next


            'Accountibility(Overall)
            Dim drAccOverall() As DataRow = allResponseData.Select("ss_desc='Accountability' and ss_categ='Z-OVERALL'")
            Dim dtAccOverall As New DataTable
            dtAccOverall.Columns.Add("ss_qcode", GetType(String))
            dtAccOverall.Columns.Add("AC", GetType(String))
            For Each tdOverall As DataRow In drAccOverall
                dtAccOverall.Rows.Add(tdOverall(7).ToString, tdOverall(16).ToString)
            Next

            Dim questionCount As Integer = dtAccOverall.Rows.Count
            For i = 0 To questionCount - 1
                dtAccountibility.Rows.Add("", "", "", "", "", "")
            Next

            If dtAccSelf.Rows.Count > 0 Then
                For i = 0 To dtAccSelf.Rows.Count - 1
                    dtAccountibility.Rows(i)("QUESTION") = Convert.ToString(dtAccSelf.Rows(i)("ss_qcode"))
                    dtAccountibility.Rows(i)("SELF") = Convert.ToString(dtAccSelf.Rows(i)("AC"))
                Next
            Else
                For i = 0 To dtAccOverall.Rows.Count - 1
                    dtAccountibility.Rows(i)("QUESTION") = Convert.ToString(dtAccOverall.Rows(i)("ss_qcode"))
                Next
            End If

            If dtAccManger.Rows.Count > 0 Then
                For i = 0 To dtAccManger.Rows.Count - 1
                    dtAccountibility.Rows(i)("MANGR") = Convert.ToString(dtAccManger.Rows(i)("AC"))
                Next
            End If


            If dtAccRopt.Rows.Count > 0 Then
                For i = 0 To dtAccRopt.Rows.Count - 1
                    dtAccountibility.Rows(i)("ROPT") = Convert.ToString(dtAccRopt.Rows(i)("AC"))
                Next
            End If

            If dtAccIntsh.Rows.Count > 0 Then
                For i = 0 To dtAccIntsh.Rows.Count - 1
                    dtAccountibility.Rows(i)("INTSH") = Convert.ToString(dtAccIntsh.Rows(i)("AC"))
                Next
            End If

            If dtAccOverall.Rows.Count > 0 Then
                For i = 0 To dtAccOverall.Rows.Count - 1
                    dtAccountibility.Rows(i)("OVERALL") = Convert.ToString(dtAccOverall.Rows(i)("AC"))
                Next
            End If
        End If
        Return dtAccountibility
    End Function
    Public Function ResponsivenessData(ByVal perNo As String, ByVal year As String, ByVal cycle As String, ByVal allResponseData As DataTable, ByVal level As String) As DataTable
        Dim dtResponsiveness As New DataTable
        dtResponsiveness.Columns.Add("QUESTION", GetType(String))
        dtResponsiveness.Columns.Add("SELF", GetType(String))
        dtResponsiveness.Columns.Add("MANGR", GetType(String))
        dtResponsiveness.Columns.Add("PEER", GetType(String))
        dtResponsiveness.Columns.Add("INTSH", GetType(String))
        dtResponsiveness.Columns.Add("OVERALL", GetType(String))

        If allResponseData.Rows.Count > 0 Then
            'Responsiveness(Self)
            Dim drResSelf() As DataRow = allResponseData.Select("ss_desc='Responsiveness' and ss_categ='Self'")
            Dim dtResSelf As New DataTable
            dtResSelf.Columns.Add("ss_qcode", GetType(String))
            dtResSelf.Columns.Add("AC", GetType(String))
            For Each tdSelf As DataRow In drResSelf
                dtResSelf.Rows.Add(tdSelf(7).ToString, tdSelf(18).ToString)
            Next

            'Responsiveness(Manger)
            Dim drResManger() As DataRow = allResponseData.Select("ss_desc='Responsiveness' and ss_categ='MANGR'")
            Dim dtResManger As New DataTable
            dtResManger.Columns.Add("ss_qcode", GetType(String))
            dtResManger.Columns.Add("AC", GetType(String))
            For Each tdManager As DataRow In drResManger
                dtResManger.Rows.Add(tdManager(7).ToString, tdManager(18).ToString)
            Next


            Dim dtResRopt As New DataTable
            dtResponsiveness.Columns.Add("ROPT", GetType(String))
            'Responsiveness(ROPT)
            Dim drResRopt() As DataRow = allResponseData.Select("ss_desc='Responsiveness' and ss_categ='ROPT'")
            dtResRopt.Columns.Add("ss_qcode", GetType(String))
            dtResRopt.Columns.Add("AC", GetType(String))
            For Each tdRopt As DataRow In drResRopt
                dtResRopt.Rows.Add(tdRopt(7).ToString, tdRopt(18).ToString)
            Next

            'Responsiveness(Internal Statkeholder)
            Dim drResIntsh() As DataRow = allResponseData.Select("ss_desc='Responsiveness' and ss_categ='INTSH'")
            Dim dtResIntsh As New DataTable
            dtResIntsh.Columns.Add("ss_qcode", GetType(String))
            dtResIntsh.Columns.Add("AC", GetType(String))
            For Each tdIntsh As DataRow In drResIntsh
                dtResIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(18).ToString)
            Next

            'Responsiveness(Overal)
            Dim drResOverall() As DataRow = allResponseData.Select("ss_desc='Responsiveness' and ss_categ='Z-OVERALL'")
            Dim dtResOverall As New DataTable
            dtResOverall.Columns.Add("ss_qcode", GetType(String))
            dtResOverall.Columns.Add("AC", GetType(String))
            For Each tdOverall As DataRow In drResOverall
                dtResOverall.Rows.Add(tdOverall(7).ToString, tdOverall(18).ToString)
            Next

            Dim questionCount As Integer = dtResOverall.Rows.Count
            For i = 0 To questionCount - 1
                dtResponsiveness.Rows.Add("", "", "", "", "", "")
            Next

            If dtResSelf.Rows.Count > 0 Then
                For i = 0 To dtResSelf.Rows.Count - 1
                    dtResponsiveness.Rows(i)("QUESTION") = Convert.ToString(dtResSelf.Rows(i)("ss_qcode"))
                    dtResponsiveness.Rows(i)("SELF") = Convert.ToString(dtResSelf.Rows(i)("AC"))
                Next
            Else
                For i = 0 To dtResOverall.Rows.Count - 1
                    dtResponsiveness.Rows(i)("QUESTION") = Convert.ToString(dtResOverall.Rows(i)("ss_qcode"))
                Next
            End If

            If dtResManger.Rows.Count > 0 Then
                For i = 0 To dtResManger.Rows.Count - 1
                    dtResponsiveness.Rows(i)("MANGR") = Convert.ToString(dtResManger.Rows(i)("AC"))
                Next
            End If


            If dtResRopt.Rows.Count > 0 Then
                For i = 0 To dtResRopt.Rows.Count - 1
                    dtResponsiveness.Rows(i)("ROPT") = Convert.ToString(dtResRopt.Rows(i)("AC"))
                Next
            End If

            If dtResIntsh.Rows.Count > 0 Then
                For i = 0 To dtResIntsh.Rows.Count - 1
                    dtResponsiveness.Rows(i)("INTSH") = Convert.ToString(dtResIntsh.Rows(i)("AC"))
                Next
            End If

            If dtResOverall.Rows.Count > 0 Then
                For i = 0 To dtResOverall.Rows.Count - 1
                    dtResponsiveness.Rows(i)("OVERALL") = Convert.ToString(dtResOverall.Rows(i)("AC"))
                Next
            End If
        End If
        Return dtResponsiveness
    End Function
    Public Function CollabarationData(ByVal perNo As String, ByVal year As String, ByVal cycle As String, ByVal allResponseData As DataTable, ByVal level As String) As DataTable
        Dim dtCollabaration As New DataTable
        dtCollabaration.Columns.Add("QUESTION", GetType(String))
        dtCollabaration.Columns.Add("SELF", GetType(String))
        dtCollabaration.Columns.Add("MANGR", GetType(String))
        dtCollabaration.Columns.Add("INTSH", GetType(String))
        dtCollabaration.Columns.Add("OVERALL", GetType(String))

        If allResponseData.Rows.Count > 0 Then
            'Collaboration(Self)
            Dim drCollSelf() As DataRow = allResponseData.Select("ss_desc='Collaboration' and ss_categ='Self'")
            Dim dtCollSelf As New DataTable
            dtCollSelf.Columns.Add("ss_qcode", GetType(String))
            dtCollSelf.Columns.Add("AC", GetType(String))
            For Each tdSelf As DataRow In drCollSelf
                dtCollSelf.Rows.Add(tdSelf(7).ToString, tdSelf(17).ToString)
            Next

            'Collaboration(Manger)
            Dim drCollManger() As DataRow = allResponseData.Select("ss_desc='Collaboration' and ss_categ='MANGR'")
            Dim dtCollManger As New DataTable
            dtCollManger.Columns.Add("ss_qcode", GetType(String))
            dtCollManger.Columns.Add("AC", GetType(String))
            For Each tdManager As DataRow In drCollManger
                dtCollManger.Rows.Add(tdManager(7).ToString, tdManager(17).ToString)
            Next

            Dim dtCollRopt As New DataTable

            dtCollabaration.Columns.Add("ROPT", GetType(String))
            'Collaboration(ROPT)
            Dim drCollRopt() As DataRow = allResponseData.Select("ss_desc='Collaboration' and ss_categ='ROPT'")
            dtCollRopt.Columns.Add("ss_qcode", GetType(String))
            dtCollRopt.Columns.Add("AC", GetType(String))
            For Each tdRopt As DataRow In drCollRopt
                dtCollRopt.Rows.Add(tdRopt(7).ToString, tdRopt(17).ToString)
            Next


            'Collaboration(Internal Statkeholder)
            Dim drCollIntsh() As DataRow = allResponseData.Select("ss_desc='Collaboration' and ss_categ='INTSH'")
            Dim dtCollIntsh As New DataTable
            dtCollIntsh.Columns.Add("ss_qcode", GetType(String))
            dtCollIntsh.Columns.Add("AC", GetType(String))
            For Each tdIntsh As DataRow In drCollIntsh
                dtCollIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(17).ToString)
            Next

            'Collaboration(Overal)
            Dim drCollOverall() As DataRow = allResponseData.Select("ss_desc='Collaboration' and ss_categ='Z-OVERALL'")
            Dim dtCollOverall As New DataTable
            dtCollOverall.Columns.Add("ss_qcode", GetType(String))
            dtCollOverall.Columns.Add("AC", GetType(String))
            For Each tdOverall As DataRow In drCollOverall
                dtCollOverall.Rows.Add(tdOverall(7).ToString, tdOverall(17).ToString)
            Next

            Dim questionCount As Integer = dtCollOverall.Rows.Count
            For i = 0 To questionCount - 1
                dtCollabaration.Rows.Add("", "", "", "", "", "")
            Next

            If dtCollSelf.Rows.Count > 0 Then
                For i = 0 To dtCollSelf.Rows.Count - 1
                    dtCollabaration.Rows(i)("QUESTION") = Convert.ToString(dtCollSelf.Rows(i)("ss_qcode"))
                    dtCollabaration.Rows(i)("SELF") = Convert.ToString(dtCollSelf.Rows(i)("AC"))
                Next
            Else
                For i = 0 To dtCollOverall.Rows.Count - 1
                    dtCollabaration.Rows(i)("QUESTION") = Convert.ToString(dtCollOverall.Rows(i)("ss_qcode"))
                Next
            End If

            If dtCollManger.Rows.Count > 0 Then
                For i = 0 To dtCollManger.Rows.Count - 1
                    dtCollabaration.Rows(i)("MANGR") = Convert.ToString(dtCollManger.Rows(i)("AC"))
                Next
            End If


            If dtCollRopt.Rows.Count > 0 Then
                For i = 0 To dtCollRopt.Rows.Count - 1
                    dtCollabaration.Rows(i)("ROPT") = Convert.ToString(dtCollRopt.Rows(i)("AC"))
                Next
            End If

            If dtCollIntsh.Rows.Count > 0 Then
                For i = 0 To dtCollIntsh.Rows.Count - 1
                    dtCollabaration.Rows(i)("INTSH") = Convert.ToString(dtCollIntsh.Rows(i)("AC"))
                Next
            End If

            If dtCollOverall.Rows.Count > 0 Then
                For i = 0 To dtCollOverall.Rows.Count - 1
                    dtCollabaration.Rows(i)("OVERALL") = Convert.ToString(dtCollOverall.Rows(i)("AC"))
                Next
            End If
        End If
        Return dtCollabaration
    End Function
    Public Function TeamData(ByVal perNo As String, ByVal year As String, ByVal cycle As String, ByVal allResponseData As DataTable, ByVal level As String) As DataTable
        Dim dtTeam As New DataTable
        dtTeam.Columns.Add("QUESTION", GetType(String))
        dtTeam.Columns.Add("SELF", GetType(String))
        dtTeam.Columns.Add("MANGR", GetType(String))
        dtTeam.Columns.Add("INTSH", GetType(String))
        dtTeam.Columns.Add("OVERALL", GetType(String))

        If allResponseData.Rows.Count > 0 Then
            'Team(Self)
            Dim drTeamSelf() As DataRow = allResponseData.Select("ss_desc='People Development' and ss_categ='Self'")
            Dim dtTeamSelf As New DataTable
            dtTeamSelf.Columns.Add("ss_qcode", GetType(String))
            dtTeamSelf.Columns.Add("AC", GetType(String))
            For Each tdSelf As DataRow In drTeamSelf
                dtTeamSelf.Rows.Add(tdSelf(7).ToString, tdSelf(19).ToString)
            Next

            'Team(Manger)
            Dim drTeamManger() As DataRow = allResponseData.Select("ss_desc='People Development' and ss_categ='MANGR'")
            Dim dtTeamManger As New DataTable
            dtTeamManger.Columns.Add("ss_qcode", GetType(String))
            dtTeamManger.Columns.Add("AC", GetType(String))
            For Each tdManager As DataRow In drTeamManger
                dtTeamManger.Rows.Add(tdManager(7).ToString, tdManager(19).ToString)
            Next


            Dim dtTeamRopt As New DataTable
            dtTeam.Columns.Add("ROPT", GetType(String))
            'Team(ROPT)
            Dim drTeamRopt() As DataRow = allResponseData.Select("ss_desc='People Development' and ss_categ='ROPT'")
            dtTeamRopt.Columns.Add("ss_qcode", GetType(String))
            dtTeamRopt.Columns.Add("AC", GetType(String))
            For Each tdRopt As DataRow In drTeamRopt
                dtTeamRopt.Rows.Add(tdRopt(7).ToString, tdRopt(19).ToString)
            Next

            'Team(Internal Statkeholder)
            Dim drTeamIntsh() As DataRow = allResponseData.Select("ss_desc='People Development' and ss_categ='INTSH'")
            Dim dtTeamIntsh As New DataTable
            dtTeamIntsh.Columns.Add("ss_qcode", GetType(String))
            dtTeamIntsh.Columns.Add("AC", GetType(String))
            For Each tdIntsh As DataRow In drTeamIntsh
                dtTeamIntsh.Rows.Add(tdIntsh(7).ToString, tdIntsh(19).ToString)
            Next

            'Team(Overal)
            Dim drTeamOverall() As DataRow = allResponseData.Select("ss_desc='People Development' and ss_categ='Z-OVERALL'")
            Dim dtTeamOverall As New DataTable
            dtTeamOverall.Columns.Add("ss_qcode", GetType(String))
            dtTeamOverall.Columns.Add("AC", GetType(String))
            For Each tdOverall As DataRow In drTeamOverall
                dtTeamOverall.Rows.Add(tdOverall(7).ToString, tdOverall(19).ToString)
            Next

            Dim questionCount As Integer = dtTeamOverall.Rows.Count
            For i = 0 To questionCount - 1
                dtTeam.Rows.Add("", "", "", "", "", "")
            Next

            If dtTeamSelf.Rows.Count > 0 Then
                For i = 0 To dtTeamSelf.Rows.Count - 1
                    dtTeam.Rows(i)("QUESTION") = Convert.ToString(dtTeamSelf.Rows(i)("ss_qcode"))
                    dtTeam.Rows(i)("SELF") = Convert.ToString(dtTeamSelf.Rows(i)("AC"))
                Next
            Else
                For i = 0 To dtTeamOverall.Rows.Count - 1
                    dtTeam.Rows(i)("QUESTION") = Convert.ToString(dtTeamOverall.Rows(i)("ss_qcode"))
                Next
            End If

            If dtTeamManger.Rows.Count > 0 Then
                For i = 0 To dtTeamManger.Rows.Count - 1
                    dtTeam.Rows(i)("MANGR") = Convert.ToString(dtTeamManger.Rows(i)("AC"))
                Next
            End If

            If dtTeamRopt.Rows.Count > 0 Then
                For i = 0 To dtTeamRopt.Rows.Count - 1
                    dtTeam.Rows(i)("ROPT") = Convert.ToString(dtTeamRopt.Rows(i)("AC"))
                Next
            End If

            If dtTeamIntsh.Rows.Count > 0 Then
                For i = 0 To dtTeamIntsh.Rows.Count - 1
                    dtTeam.Rows(i)("INTSH") = Convert.ToString(dtTeamIntsh.Rows(i)("AC"))
                Next
            End If

            If dtTeamOverall.Rows.Count > 0 Then
                For i = 0 To dtTeamOverall.Rows.Count - 1
                    dtTeam.Rows(i)("OVERALL") = Convert.ToString(dtTeamOverall.Rows(i)("AC"))
                Next
            End If
        End If
        Return dtTeam
    End Function
    Public Function DisplayOverallData(ByVal perNo As String, ByVal year As String, ByVal cycle As String, ByVal level As String, ByVal dtAll As DataTable) As DataTable
        Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
        Dim strQuery As String = String.Empty
        strQuery = " Select * from hrps.V_ALL_I1_I6_T_NEW  where EMA_PERNO = '" + perNo + "' and ss_year =" & year & " and ss_srlno =" & cycle & "  ORDER BY 1 "
        'End

        Dim cmd As New OracleCommand()
        Dim dtOverallData = GetData(strQuery, conHrps)
        Dim dtOverall As New DataTable

        If dtOverallData.Rows.Count > 0 Then
            'Team(Self)
            Dim drSelf() As DataRow = dtOverallData.Select("ss_categ='Self'")
            dtOverall.Columns.Add("QUESTION")
            dtOverall.Rows.Add("Accountability")
            dtOverall.Rows.Add("Collaboration")
            dtOverall.Rows.Add("Responsiveness")
            dtOverall.Rows.Add("People Development")
            dtOverall.Rows.Add("Sum - Total")
            dtOverall.Columns.Add("G")
            dtOverall.Columns.Add("A")
            dtOverall.Columns.Add("U")
            dtOverall.Rows(0)("G") = dtAll.Select("ss_desc='Accountability' and AC='G' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(1)("G") = dtAll.Select("ss_desc='Collaboration' and COL='G' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(2)("G") = dtAll.Select("ss_desc='Responsiveness' and RES='G' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(3)("G") = dtAll.Select("ss_desc='People Development' and TEAM='G' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(4)("G") = dtAll.Select("ss_desc='Accountability' and AC='G' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='Collaboration' and COL='G' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='Responsiveness' and RES='G' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='People Development' and TEAM='G' and ss_categ in ('Z-OVERALL')").Length

            dtOverall.Rows(0)("A") = dtAll.Select("ss_desc='Accountability' and AC='A' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(1)("A") = dtAll.Select("ss_desc='Collaboration' and COL='A' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(2)("A") = dtAll.Select("ss_desc='Responsiveness' and RES='A' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(3)("A") = dtAll.Select("ss_desc='People Development' and TEAM='A' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(4)("A") = dtAll.Select("ss_desc='Accountability' and AC='A' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='Collaboration' and COL='A' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='Responsiveness' and RES='A' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='People Development' and TEAM='A' and ss_categ in ('Z-OVERALL')").Length

            dtOverall.Rows(0)("U") = dtAll.Select("ss_desc='Accountability' and AC='U' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(1)("U") = dtAll.Select("ss_desc='Collaboration' and COL='U' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(2)("U") = dtAll.Select("ss_desc='Responsiveness' and RES='U' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(3)("U") = dtAll.Select("ss_desc='People Development' and TEAM='U' and ss_categ in ('Z-OVERALL')").Length
            dtOverall.Rows(4)("U") = dtAll.Select("ss_desc='Accountability' and AC='U' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='Collaboration' and COL='U' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='Responsiveness' and RES='U' and ss_categ in ('Z-OVERALL')").Length + dtAll.Select("ss_desc='People Development' and TEAM='U' and ss_categ in ('Z-OVERALL')").Length

            Dim drOverall() As DataRow = dtOverallData.Select("ss_categ='Z-OVERALL'")
            dtOverall.Columns.Add("OVERALL")
            For Each tdOverall As DataRow In drOverall
                dtOverall.Rows(0)("OVERALL") = tdOverall("AC").ToString()
                dtOverall.Rows(1)("OVERALL") = tdOverall("COL").ToString()
                dtOverall.Rows(2)("OVERALL") = tdOverall("RES").ToString()
                dtOverall.Rows(3)("OVERALL") = tdOverall("TEAM").ToString()
            Next
        End If
        Return dtOverall
    End Function
End Class
