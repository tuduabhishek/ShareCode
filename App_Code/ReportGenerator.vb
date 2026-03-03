Imports System.IO
Imports System.Data
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Public Class ReportGenerator

    Public Sub GenerateFullReport(filePath As String,
                                  employeeName As String,
                                  designation As String,
                                  fy As String)

        Dim doc As New Document(PageSize.A4, 40, 40, 50, 50)
        Dim writer = PdfWriter.GetInstance(doc, New FileStream(filePath, FileMode.Create))
        doc.Open()

        ' PAGE 1
        AddCoverPage(doc, writer, employeeName, designation, fy)

        '' PAGE 2
        AddStaticPage(doc, writer, "~/Images/page2.png")

        '' PAGE 3
        AddPage3(doc, writer, "Report for " & employeeName)

        '' PAGE 4
        AddStaticPage(doc, writer, "~/Images/page4.png")

        '' PAGE 5
        AddStaticPage(doc, writer, "~/Images/page5.png")

        '' PAGE 6 & 7 - TABLE SAMPLE
        'AddSampleTables(doc, employeeName)

        '' PAGE 8 - BULLETS
        'AddSampleInsightPage(doc, employeeName)

        '' PAGE 9 - DEVELOPMENT
        'AddSampleDevelopmentPage(doc, employeeName)

        doc.Close()
    End Sub



    Private Sub AddCoverPage(doc As Document, writer As PdfWriter,
                         name As String,
                         designation As String,
                         fy As String)

        doc.NewPage()

        Dim img = Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/Images/cover.png"))
        img.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height)
        img.SetAbsolutePosition(0, 0)
        writer.DirectContentUnder.AddImage(img)

        Dim cb = writer.DirectContent

        ' === Load Aptos Bold (TTF) and embed it ===
        ' Ensure the font file exists at this path. Adjust if needed.
        Dim aptosBoldPath As String = System.Web.HttpContext.Current.Server.MapPath("~/fonts/Aptos-Bold.ttf")

        ' Use Identity-H to support full Unicode; EMBEDDED for consistent output.
        Dim bf As BaseFont
        Try
            bf = BaseFont.CreateFont(aptosBoldPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
        Catch ex As Exception
            ' Fallback to Helvetica Bold if Aptos isn't available
            bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
        End Try

        ' Set the fill color to Tata Steel Blue (RGB: 72, 106, 174)
        cb.SetColorFill(New BaseColor(11, 118, 160))

        cb.BeginText()
        cb.SetFontAndSize(bf, 28) ' Bold + 28pt

        ' === Positions moved slightly lower (down by ~20 points) ===
        ' Original Y: 350 → 330, 325 → 305, 300 → 280
        cb.SetTextMatrix(300, 300)
        cb.ShowText(name)

        cb.SetTextMatrix(300, 275)
        cb.ShowText(designation)

        cb.SetTextMatrix(300, 250)
        cb.ShowText("FY '" & fy) ' Note: "&amp;" from HTML replaced with "&"
        cb.EndText()

    End Sub

    ' ================= STATIC PAGE =================
    Private Sub AddStaticPage(doc As Document,
                              writer As PdfWriter,
                              imagePath As String)

        doc.NewPage()

        Dim img = Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath(imagePath))
        img.ScaleToFit(PageSize.A4.Width, PageSize.A4.Height)
        img.SetAbsolutePosition(0, 0)
        writer.DirectContentUnder.AddImage(img)

    End Sub

    ' ================= SAMPLE TABLE PAGES =================

    Private Sub AddPage3(doc As Document,
                     writer As PdfWriter,
                     heading As String)

        doc.NewPage()

        ' Use the actual page size from the document
        Dim ps As Rectangle = doc.PageSize

        Dim img = Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/Images/page3.png"))
        img.ScaleToFit(ps.Width, ps.Height)
        img.SetAbsolutePosition(0, 0)
        writer.DirectContentUnder.AddImage(img)

        Dim cb As PdfContentByte = writer.DirectContent

        ' === Load Aptos Display (TTF) and embed it ===
        Dim aptosDisplayPath As String = System.Web.HttpContext.Current.Server.MapPath("~/fonts/Aptos-Display.ttf")

        Dim bf As BaseFont
        Try
            bf = BaseFont.CreateFont(aptosDisplayPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED)
        Catch ex As Exception
            bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED)
        End Try

        ' White color
        cb.SetColorFill(New BaseColor(255, 255, 255))

        ' Top-center coordinates
        Dim centerX As Single = ps.Width / 2.0F
        Dim topOffset As Single = 36.0F ' ~0.5 inch margin from top edge
        Dim yTop As Single = ps.Height - topOffset

        ' Draw heading centered at top
        ColumnText.ShowTextAligned(cb,
                               Element.ALIGN_CENTER,
                               New Phrase(heading, New Font(bf, 33.0F)),
                               centerX,
                               yTop,
                               0)
    End Sub

    Private Sub AddSampleTables(doc As Document, employeeName As String)

        doc.NewPage()

        Dim headerFont As New Font(Font.FontFamily.HELVETICA, 14, Font.BOLD)
        doc.Add(New Paragraph("Report for " & employeeName, headerFont))
        doc.Add(New Paragraph("Behaviour Summary"))
        doc.Add(New Paragraph(Environment.NewLine))

        Dim dt As New DataTable()
        dt.Columns.Add("Practice")
        dt.Columns.Add("Self")
        dt.Columns.Add("Others")

        dt.Rows.Add("Accountability", "3.5", "3.2")
        dt.Rows.Add("Collaboration", "3.8", "3.4")

        doc.Add(CreateTable(dt))

        doc.NewPage()
        doc.Add(New Paragraph("Behaviour Summary Continued", headerFont))
        doc.Add(New Paragraph(Environment.NewLine))
        doc.Add(CreateTable(dt))

    End Sub

    Private Function CreateTable(dt As DataTable) As PdfPTable

        Dim table As New PdfPTable(dt.Columns.Count)
        table.WidthPercentage = 100

        For Each col As DataColumn In dt.Columns
            Dim cell As New PdfPCell(New Phrase(col.ColumnName))
            cell.BackgroundColor = New BaseColor(0, 70, 127)
            cell.Phrase.Font.Color = BaseColor.WHITE
            table.AddCell(cell)
        Next

        For Each row As DataRow In dt.Rows
            For Each item In row.ItemArray
                table.AddCell(item.ToString())
            Next
        Next

        Return table

    End Function

    ' ================= INSIGHT PAGE =================
    Private Sub AddSampleInsightPage(doc As Document, employeeName As String)

        doc.NewPage()

        doc.Add(New Paragraph("Report Summary for " & employeeName))
        doc.Add(New Paragraph(Environment.NewLine))

        AddBulletSection(doc, "Verbatims",
                         New List(Of String) From {
                             "Strong ownership",
                             "Needs proactive communication"
                         })

        AddBulletSection(doc, "Continue",
                         New List(Of String) From {"Stakeholder alignment"})

        AddBulletSection(doc, "Develop",
                         New List(Of String) From {"Faster decisions"})

        AddBulletSection(doc, "Eliminate",
                         New List(Of String) From {"Delayed escalation"})

    End Sub

    ' ================= DEVELOPMENT PAGE =================
    Private Sub AddSampleDevelopmentPage(doc As Document, employeeName As String)

        doc.NewPage()

        doc.Add(New Paragraph("Developmental Insights for " & employeeName))
        doc.Add(New Paragraph(Environment.NewLine))

        AddBulletSection(doc, "Care",
                         New List(Of String) From {"Build stronger empathy"})

        AddBulletSection(doc, "Connect",
                         New List(Of String) From {"Improve cross-team sync"})

        AddBulletSection(doc, "Contribute",
                         New List(Of String) From {"Drive innovation"})

        AddBulletSection(doc, "Change",
                         New List(Of String) From {"Act faster"})

    End Sub

    Private Sub AddBulletSection(doc As Document,
                                 title As String,
                                 items As List(Of String))

        doc.Add(New Paragraph(title))
        doc.Add(New Paragraph(Environment.NewLine))

        Dim list As New iTextSharp.text.List(iTextSharp.text.List.UNORDERED)

        For Each item In items
            list.Add(New ListItem(item))
        Next

        doc.Add(list)
        doc.Add(New Paragraph(Environment.NewLine))

    End Sub

End Class