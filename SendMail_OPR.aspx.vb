
Imports System.IO
Imports System.Net.Mail
Imports System.Data.OracleClient
Imports System.Data
Imports System.Net
Imports System.Data.OleDb
Imports ClosedXML.Excel

Partial Class SendMail_OPR
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Shared conStr As String
    Shared FilePath As String
    Private Sub SendMail_OPR_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            Dim strUserID As String = ""
            Dim auth As Boolean
            Dim vUserFullName As String = Page.User.Identity.Name
            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
                'strUserID = "197838"
            End If
            'strUserID = "197838"
            Dim r1 As New OracleCommand()
            r1.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g1 = getRecordInDt(r1, conHrps)
            If g1.Rows.Count > 0 Then
                ViewState("FY") = g1.Rows(0)("IRC_DESC").ToString()
            End If
            Dim strsrl As String = "select IRC_DESC from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
            Dim mycommand As OracleCommand
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            mycommand = New OracleCommand(strsrl, conHrps)
            Dim dasrl As New OracleDataAdapter(mycommand)
            Dim dtsrl1 As New DataTable()
            dasrl.Fill(dtsrl1)
            If dtsrl1.Rows.Count > 0 Then
                ViewState("SRLNO") = dtsrl1.Rows(0).Item("IRC_DESC").ToString()
            End If

            auth = GetPno(strUserID.ToUpper())
            If auth = True Then
            Else
                Session("errorMsg") = "You are not part of this exercise. In case of any query please be in touch with your BUHR."
                Response.Redirect("errorpage.aspx")
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub One_time_mail_toIL2(enddt As String)
        Try
            Dim cnt As Integer = 0
            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty
            Dim link As String
            link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/selectassesor_OPR.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/selectassesor_OPR.aspx <a/>"

            body = "Dear Colleague,<br/><br/> We are extending the date for finalizing the list of respondents for evaluative 360-degree feedback, by a couple of days."
            body += " <br/><br/> Request you to submit the same by tomorrow in the system, using the link given below: <br/><br/> " & link
            body += " <br/><br/> In case of any query, please contact Ms. Shruti Choudhury, Head HRM Leadership Development."
            body += "<br/><br/><br/> With regards,<br/> Zubin Palia<br/> Chief Group HR & IR"
            '            Dim perno As String() = {"162523", "000052", "115351", "115442", "119046", "119852", "119907", "119960", "119992", "120041", "120393", "122185", "123418", "123491",
            '"123830", "127290", "127982", "128364", "128411", "147876", "147897", "148315", "148482", "148486", "148516", "148523", "149555",
            '"149633", "149729", "149786", "150351", "158404", "162410", "163730", "165004", "174357", "174782", "175089", "175163", "197576",
            '"197582", "197977", "198850", "198893", "200056", "218475", "269377", "388078", "500228", "503102", "805021", "151629"}

            ' Dim mail As New System.Net.Mail.MailMessage()
            Dim fg As New OracleCommand()
            fg.CommandText = "Select  distinct ema_perno ,ema_email_id from tips.t_empl_all where and EMA_EQV_LEVEL in ('I3') and EMA_COMP_CODE='1000'"
            Dim df = getRecordInDt(fg, conHrps)
            If df.Rows.Count > 0 Then
                While cnt < df.Rows.Count
                    If df.Rows(0)("ema_email_id") = "" Then

                    Else
                        mail.Bcc.Add(df.Rows(0)("ema_email_id"))
                        ' mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
                        ' Exit Sub
                        mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

                        mail.Subject = "End Year 360 assessment | Date extended by tomorrow"

                        mail.SubjectEncoding = System.Text.Encoding.UTF8
                        mail.Body = body

                        mail.BodyEncoding = System.Text.Encoding.UTF8
                        mail.IsBodyHtml = True
                        ' Dim data As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("images\User_manual_360 DEGREE_Select Assessor.pdf"))
                        'Data.Name = "User_manual_360 DEGREE_Select Assessor.pdf"
                        'mail.Attachments.Add(Data)
                        Dim client As New SmtpClient()
                        client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
                        client.Port = 25
                        'client.Host = "144.0.11.253"
                        client.Host = "144.0.16.7"
                        client.EnableSsl = False
                        ' client.Send(mail)
                        client.Dispose()

                        '            '  Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
                        '            ' Dim dividesterr = perno(hg) & "   -- Success" & Environment.NewLine
                        '            ' File.AppendAllText(fnameerr, dividesterr)
                    End If
                    cnt = cnt + 1
                End While
            End If
            '    ' Threading.Thread.Sleep(1000)


            ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Error")
            'Dim fnameerr As String = Server.MapPath("images/LogFile.txt")
            'Dim dividesterr = ex.ToString & Environment.NewLine
            'File.AppendAllText(fnameerr, dividesterr)
        End Try
    End Sub

    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            Dim q As New OracleCommand()
            q.CommandText = "Select IGP_user_id,ema_ename from t_ir_adm_grp_privilege,TIPS.t_empl_all where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno.ToString() & "' and EMA_COMP_CODE='1000'"
            Dim p = getRecordInDt(q, conHrps)
            If p.Rows.Count > 0 Then
                d = True
            Else
                d = False

            End If
            Return d
        Catch ex As Exception

        End Try
    End Function

    Public Sub Mail_to_IL1()
        Try

            Dim qry As New OracleCommand()
            qry.CommandText = "select distinct ss_email ,ss_pno from t_survey_status where ss_year='2020' and ss_categ='MANGR'"
            qry.CommandText += "  and ss_level='IL1'  and ss_pno<>'119628' and ss_email<>'NARENDRAN@TATASTEEL.COM'"

            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty

            Dim link As String
            link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/SurveyApproval.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/SurveyApproval.aspx <a/>"

            'body = " Dear Ma’am/ Sir,<br/><br/> As we draw near to our end-year appraisal, we are commencing the evaluative 360-degree assessment for IL2s."
            'body += "The survey is similar to the developmental survey that happened in July 2020 and feedback is being sought from four categories of respondents"
            'body += " – Manager, Subordinates, Peers & Internal Stakeholders. <br/><br/> The IL2s have the option to select the stakeholders with whom they work closely"
            'body += " and collaborate frequently. You are requested to approve the list of respondents for each of the IL2 Officers in your area once they submit their"
            'body += "forms. You can also modify the list of respondents and send the form back to the concerned officer, if required. The minimum number of respondents "
            'body += "for each of the categories is mentioned in the form.<br/><br/> Link for accessing the 360 assessment form (Recommended Browser: "
            'body += "Laptop: Google chrome || Mobile/ ipad / Tab: Microsoft Edge): <br/><br/>" & link & " <br/><br/> Please find enclosed a user manual describing "
            'body += "steps for approving the list of respondents. Request you not to approve the form in case of IL2 officers who are on deputation. <br/><br/> "
            'body += "In case of any queries, please connect with Ms. Shruti Choudhury, Head HRM Leadership  Development. <br/><br/><br/><br/> With regards,"
            'body += " <br/> Zubin Palia <br/> Chief Group HR & IR"

            '**************** new body
            body = "Dear Ma’am/ Sir,<br/><br/> As we draw near to our end-year appraisal, we are commencing the evaluative 360-degree assessment for IL2s."
            body += " The survey is similar to the developmental survey that happened earlier in FY 21. The feedback is being sought from four categories of respondents"
            body += "– Manager, Subordinates, Peers & Internal Stakeholders. <br/><br/> The IL2s have the option to select the stakeholders with whom they work closely"
            body += " and collaborate frequently. <b> The respondents whose name is checked (tick-box) will be the final list of respondents to whom the survey "
            body += "will float post your approval.</b> <br/><br/> You are requested to approve the list of respondents for each of the IL2 Officers in your area once they "
            body += "submit their forms. You can also modify the list of respondents and send the form back to the concerned officer, if required. <br/><br/> "
            body += "The minimum number of respondents for each of the categories is mentioned in the form. <br/><br/> "
            body += " For Laptop/Desktop users kindly copy the link given below in Chrome/Edge browser and open. <br/><br/>" & link & "<br/><br/>"
            body += " Mobile/iPad users may click on the above link to open the page in Edge browser. <br/><br/> "
            body += " Please find enclosed a user manual describing steps for approving the list of respondents. <br/><br/>"
            body += " In case of any IT related issues, please connect with Mr. Avirup Bhowmick (avirup.bhowmick@tatasteel.com). <br/><br/>"
            body += "In case of any other queries, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development."
            body += "  <br/><br/><br/><br/> With regards, <br/> Zubin Palia <br/> Chief Group HR & IR"

            mail.Bcc.Add("shashikala.t@tatasteel.com")
            mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
            mail.Bcc.Add("atrayee@tatasteel.com")
            ' mail.Bcc.Add("uttam.singh@tatasteel.com")
            'mail.Bcc.Add("RAJIV.MUKERJI@TATASTEEL.COM")
            'mail.Bcc.Add("SUDHANSUPATHAK@TATASTEEL.COM")
            'mail.Bcc.Add("AVNEESH.GUPTA@TATASTEEL.COM")
            'mail.Bcc.Add("SANJIVPAUL@TATASTEEL.COM")
            'mail.Bcc.Add("DIBYENDUB@TATASTEEL.COM")
            'mail.Bcc.Add("PROBAL.GHOSH@TATASTEEL.COM")
            'mail.Bcc.Add("RAJ.KUMAR@TATASTEEL.COM")
            'mail.Bcc.Add("UTTAM.SINGH@TATASTEEL.COM")
            'mail.Bcc.Add("KOUSHIK.CHATTERJEE@TATASTEEL.COM")
            'mail.Bcc.Add("DEBASHISH.BHATTACHARJEE@TATASTEEL.COM")
            'mail.Bcc.Add("PEEYUSH@TATASTEEL.COM")
            'mail.Bcc.Add("CHANAKYA@TATASTEEL.COM")
            'mail.Bcc.Add("SUNDAR.RAMAN@TATASTEEL.COM")
            'mail.Bcc.Add("avirup.bhowmick@tatasteel.com")
            ' mail.Bcc.Add("sdt@tatasteel.com")

            mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            mail.Subject = "End-Year 360 Assessment"

            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.Body = body

            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.IsBodyHtml = True
            Dim data As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("images\User_manual_360 DEGREE Survey Approval.pdf"))
            data.Name = "User_manual_360 DEGREE Survey Approval.pdf"
            mail.Attachments.Add(data)
            Dim client As New SmtpClient()
            client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
            client.Port = 25
            client.Host = "144.0.11.253"
            'client.Host = "144.0.16.7"
            client.EnableSsl = False
            client.Send(mail)
            client.Dispose()
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")

        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Public Sub Reminder_to_respondent(enddt As String)
        Try
            'Dim qry As New OracleCommand()

            'qry.CommandText = "select distinct UPPER(ss_email) ss_email ,ss_name from hrps.t_survey_status,tips.t_empl_all where ss_year='" & ViewState("FY") & "'  and ss_app_tag='AP'"
            'qry.CommandText += " and ss_del_tag='N' and ss_wfl_status=2 and trunc(ss_tag_dt)< trunc(sysdate-" & txtdays.Text.Trim & ") and SS_ASSES_PNO=EMA_PERNO and ema_eqv_level in('I3','I4','I5','I6') and EMA_COMP_CODE='1000'"


            'Dim dts = getRecordInDt(qry, conHrps)

            'If dts.Rows.Count > 0 Then
            '    For gh = 0 To dts.Rows.Count - 1

            '        Dim mail As New System.Net.Mail.MailMessage()
            '        Dim body As String = String.Empty

            '        Dim link As String
            '        link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/Feedback.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/Feedback.aspx <a/>"


            '        Dim link1 As String
            '        link1 = "<a href='https://tslhr.tatasteel.co.in/Feedback_360/survey.aspx'> https://tslhr.tatasteel.co.in/Feedback_360/survey.aspx <a/>"

            '        body = " Dear " & dts.Rows(gh)("ss_name").ToString() & ",<br/><br/> As communicated earlier, you have been nominated as one of the respondents to give feedback on"
            '        body += " behaviours for one or more Officers. This is a gentle reminder to complete the same by <b>" & txtEndDate.Text.Trim() & ".</b> <br/><br/>"
            '        body += " For Laptop/Desktop users kindly copy the link given below in Chrome/Edge browser and open. Mobile/iPad "
            '        body += "users may click on the same link to open the page in Edge browser.<br/><br/> Link for Employees <br/>" & link & " <br/><br/> Link for external users"
            '        body += " <br/> " & link1 & " <br/><br/>The link is unique to you. Please do not forward or share it with anyone. The feedback provided will be strictly "
            '        body += "confidential and will not be disclosed under any circumstances.<br/><br/>"
            '        body += "In case of any other queries, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development."
            '        body += "  <br/><br/><br/><br/> With regards, <br/> Zubin Palia <br/> Chief Group HR & IR"

            '        mail.Bcc.Add(dts.Rows(gh)("ss_email").ToString())
            '        ' mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
            '        ' mail.Bcc.Add("medha.chaturvedi@tatasteel.com")
            '        mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            '        mail.Subject = "Reminder To complete the End Year 360 assessment"

            '        mail.SubjectEncoding = System.Text.Encoding.UTF8
            '        mail.Body = body

            '        mail.BodyEncoding = System.Text.Encoding.UTF8
            '        mail.IsBodyHtml = True

            '        Dim client As New SmtpClient()
            '        client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
            '        client.Port = 25
            '        client.Host = "144.0.11.253"
            '        ' client.Host = "144.0.16.7"
            '        client.EnableSsl = False
            '        client.Send(mail)
            '        client.Dispose()
            '    Next
            'End If
            'ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Public Sub Reminder_to_il2(enddt As String)
        Try
            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty

            Dim link As String
            link = "<a href='http://webappsdev01.corp.tatasteel.com/Feedback360/selectassesor_OPR.aspx'> http://webappsdev01.corp.tatasteel.com/Feedback360/selectassesor_OPR.aspx <a/>"

            body = " Dear Colleague,<br/><br/> As communicated earlier, you have been requested to finalize the list of respondents for your evaluative 360-degree "
            body += "feedback, as a part of the end year assessment process. The same is still pending. Request you to submit the same by " & enddt & " on the system, "
            body += " using the link below:<br/>" & link & "<br/> If you have a query, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development "
            body += "or Mr. Vikas Kumar, Head HRM Performance Management & Talent Development."

            mail.Bcc.Add("shashikala.t@tatasteel.com")
            mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
            mail.Bcc.Add("medha.chaturvedi@tatasteel.com")
            mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            mail.Subject = "End-Year 360 Assessment"

            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.Body = body

            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.IsBodyHtml = True

            Dim client As New SmtpClient()
            client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
            client.Port = 25
            'client.Host = "144.0.11.253"
            client.Host = "144.0.16.7"
            client.EnableSsl = False
            client.Send(mail)
            client.Dispose()
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")

        Catch ex As Exception
            MsgBox(ex.ToString())
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

    Public Sub Mail_to_res(enddt As String)
        Try
            'Dim link As String
            'Dim name As String = String.Empty
            'Dim qry As New OracleCommand()

            'qry.CommandText = "select distinct UPPER(ss_email) ss_email ,ss_name from hrps.t_survey_status,tips.tips.t_empl_all where ss_year='" & ViewState("FY").ToString() & "'  and ss_app_tag='AP'"
            'qry.CommandText += " and ss_del_tag='N' and ss_wfl_status=2 and trunc(ss_tag_dt)>=trunc(sysdate-" & txtdays.Text.Trim & ") and SS_ASSES_PNO=EMA_PERNO and ema_eqv_level in('I3','I4','I5','I6') and EMA_COMP_CODE='1000'"

            'Dim g = getRecordInDt(qry, conHrps)
            'If g.Rows.Count > 0 Then
            '    For p = 0 To g.Rows.Count - 1
            '        'If g.Rows(p)("ss_type") = "NORG" Then
            '        link = "<a href=' http://webappsdev01.corp.tatasteel.com/Feedback360/feedback_OPR.aspx'>  http://webappsdev01.corp.tatasteel.com/Feedback360/feedback_OPR.aspx <a/>"
            '        'ElseIf g.Rows(p)("ss_type") = "ORG" Then

            '        '    link = "<a href='http://webappsdev01.corp.tatasteel.com/Feedback360/feedback.aspx'> http://webappsdev01.corp.tatasteel.com/Feedback360/feedback.aspx <a/>"
            '        'End If
            '        Dim link1 = "<a href=' http://webappsdev01.corp.tatasteel.com/Feedback360/feedback_OPR.aspx'>  http://webappsdev01.corp.tatasteel.com/Feedback360/feedback_OPR.aspx <a/>"
            '        Dim mail As New System.Net.Mail.MailMessage()
            '        Dim body As String = String.Empty

            '        body = "Dear " & g.Rows(p)("ss_name").ToString() & " <br/><br/> As we aspire to be the most valuable and respected steel company globally in the next 5-10"
            '        body += " years, we are developing agile behaviours in our top leadership – accountability, responsiveness, collaboration and people development. As we draw near to the end of this "
            '        body += "FY, we are commencing the evaluative 360-degree assessment for officers on these behaviours.<br/><br/>"
            '        body += "You have been nominated as one of the respondents to give feedback on behaviours for one or more officers. Request you to kindly provide feedback using the attached link by <b>" & txtEndDate.Text.Trim() & ".</b> <br/><br/>"
            '        body += "For Laptop/Desktop users kindly copy the link given below in Chrome/Edge browser and open. <br/>"
            '        body += "Mobile/iPad users may click on the same link to open the page in Edge browser. <br/><br/> Link for Employees <br/> " & link1
            '        body += " <br/><br/> Link for external users <br/> " & link & " <br/><br/>"
            '        body += "The link is unique to you. Please do not forward or share it with anyone. The feedback provided will be strictly confidential and will not be disclosed under any circumstances."

            '        body += " <br/><br/> In case of any other queries, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development."
            '        body += "  <br/><br/><br/><br/> With regards, <br/> Zubin Palia <br/> Chief Group HR & IR"


            '        ' mail.Bcc.Add("shashikala.t@tatasteel.com")
            '        mail.Bcc.Add(g.Rows(p)("ss_email").ToString())
            '        ' mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
            '        ' mail.Bcc.Add("medha.chaturvedi@tatasteel.com")
            '        mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            '        mail.Subject = "End-year 360 Feedback"

            '        mail.SubjectEncoding = System.Text.Encoding.UTF8
            '        mail.Body = body

            '        ' Dim data As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("images\User_manual_360 DEGREE FEEDBACK.pdf"))
            '        ' data.Name = "User_manual_360 DEGREE FEEDBACK.pdf"
            '        ' mail.Attachments.Add(data)

            '        mail.BodyEncoding = System.Text.Encoding.UTF8
            '        mail.IsBodyHtml = True

            '        Dim client As New SmtpClient()
            '        client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
            '        client.Port = 25
            '        client.Host = "144.0.11.253"
            '        ' client.Host = "144.0.16.7"
            '        client.EnableSsl = False
            '        client.Send(mail)
            '        client.Dispose()

            '    Next
            '    ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")

            'End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub SendMail_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'Mail_to_res()
            btnopt.Visible = False
        End If
    End Sub

    Private Function GetDataTable() As DataTable
        'conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & FilePath & ";Extended Properties=""Excel 12.0;IMEX=2;HDR=Yes;"""
        'conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FilePath + ";Extended Properties=\Excel 8.0;HDR=YES;IMEX=1;"""
        conStr = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=" & FilePath & ";Extended Properties=""Excel 12.0;IMEX=2;HDR=Yes;"""
        Dim con As New OleDb.OleDbConnection(conStr)
        con.Open()
        Dim sql As String = ""
        Dim dtExcelSheetName As DataTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
        Dim getExcelSheetName As String = dtExcelSheetName.Rows(0)("Table_Name").ToString()
        sql = "SELECT * FROM [" & getExcelSheetName & "]"
        Dim da As New OleDb.OleDbDataAdapter(sql, con)
        Dim ds As New DataSet
        Dim dt As DataTable
        da.Fill(ds)
        dt = ds.Tables(0)
        con.Close()
        Return dt
    End Function

    Protected Sub btnopt_Click(sender As Object, e As EventArgs)
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim cmd As New OracleCommand("PR_MAIIL_TO_ALL_TYPE")
            'cmd.CommandText = "call hrps.PR_MAIIL_TO_ALL_TYPE(:year,:cycle,:level,:perno,:type)"
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("yr", txtYear.Text.ToString())
            cmd.Parameters.AddWithValue("cyc", txtCycle.Text.ToString())
            cmd.Parameters.AddWithValue("lvl", ddlLevel.Text.ToString())
            cmd.Parameters.AddWithValue("perno", txtPerno.Text.ToString())
            cmd.Parameters.AddWithValue("para", ddlMailType.SelectedValue.ToString())
            cmd.Parameters.AddWithValue("sgrad", ddlSgrade.SelectedItem.Text.ToString())
            cmd.Parameters.AddWithValue("sub", txtSubject.Text.Trim().ToString())
            Dim chk = cmd.ExecuteNonQuery()
            If chk > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, " Your mail send suucessfully. Thank You!")
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnShow_Click(sender As Object, e As EventArgs)
        Try
            Dim qry As New OracleCommand()
            If ddlMailType.SelectedValue = 1 Then
                qry.CommandText = "select ema_perno,EMA_ENAME,ema_desgn_desc,initcap(to_char(EMA_STEP1_STDT,'dd-Mon-yyyy')) Step1_Sdt,initcap(to_char(EMA_STEP1_ENDDT,'dd-Mon-yyyy')) Step1_Edt,initcap(to_char(EMA_STEP2_STDT,'dd-Mon-yyyy')) Step2_Sdt,initcap(to_char(EMA_STEP2_ENDDT,'dd-Mon-yyyy')) Step2_Edt,initcap(to_char(EMA_STEP3_STDT,'dd-Mon-yyyy')) Step3_Sdt,initcap(to_char(EMA_STEP3_ENDDT,'dd-Mon-yyyy')) Step3_Edt from hrps.t_emp_master_feedback360 emf where ema_perno not in (select ee_pno from hrps.t_emp_excluded) and ema_email_id is not null and ema_eqv_level =:ema_eqv_level and ema_eqv_level not in ('I1') and ema_year=:ema_year and ema_cycle=:ema_cycle"
            ElseIf ddlMailType.SelectedValue = 2 Then
                qry.CommandText = "select ema_perno,EMA_ENAME,ema_desgn_desc,initcap(to_char(EMA_STEP1_STDT,'dd-Mon-yyyy')) Step1_Sdt,initcap(to_char(EMA_STEP1_ENDDT,'dd-Mon-yyyy')) Step1_Edt,initcap(to_char(EMA_STEP2_STDT,'dd-Mon-yyyy')) Step2_Sdt,initcap(to_char(EMA_STEP2_ENDDT,'dd-Mon-yyyy')) Step2_Edt,initcap(to_char(EMA_STEP3_STDT,'dd-Mon-yyyy')) Step3_Sdt,initcap(to_char(EMA_STEP3_ENDDT,'dd-Mon-yyyy')) Step3_Edt from hrps.t_emp_master_feedback360 emf where ema_perno not in (select ee_pno pno from hrps.t_emp_excluded union select ss_asses_pno pno from hrps.t_survey_status where ss_categ='Self' and ss_wfl_status in ('1','2','3') and ss_year=:ema_year and ss_srlno=:ema_cycle) and ema_email_id is not null and ema_eqv_level =:ema_eqv_level and ema_eqv_level not in ('I1')"
            ElseIf ddlMailType.SelectedValue = 3 Or ddlMailType.SelectedValue = 4 Then
                qry.CommandText = "select ema_perno,EMA_ENAME,ema_desgn_desc,initcap(to_char(EMA_STEP1_STDT,'dd-Mon-yyyy')) Step1_Sdt,initcap(to_char(EMA_STEP1_ENDDT,'dd-Mon-yyyy')) Step1_Edt,initcap(to_char(EMA_STEP2_STDT,'dd-Mon-yyyy')) Step2_Sdt,initcap(to_char(EMA_STEP2_ENDDT,'dd-Mon-yyyy')) Step2_Edt,initcap(to_char(EMA_STEP3_STDT,'dd-Mon-yyyy')) Step3_Sdt,initcap(to_char(EMA_STEP3_ENDDT,'dd-Mon-yyyy')) Step3_Edt from hrps.t_emp_master_feedback360 emf,t_survey_status ss where emf.ema_perno=ss.ss_approver and ss.ss_year=:ema_year and ss.ss_srlno=:ema_cycle and ss.ss_wfl_status ='1' and ss.ss_app_tag is null and emf.ema_eqv_level =:ema_eqv_level and emf.ema_email_id is not null"
            End If

            If txtPerno.Text.Trim().ToString <> "" Then
                qry.CommandText += " and ema_perno=:ema_perno"
            End If
            If ddlSgrade.SelectedValue <> 0 And ddlMailType.SelectedValue > 0 Then
                qry.CommandText += " and emf.EMA_EMPL_SGRADE=:EMA_EMPL_SGRADE"
            End If



            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_year", txtYear.Text.ToString())
            qry.Parameters.AddWithValue("ema_cycle", txtCycle.Text.ToString())
            qry.Parameters.AddWithValue("ema_eqv_level", ddlLevel.SelectedValue.ToString())
            If txtPerno.Text.Trim().ToString <> "" Then
                qry.Parameters.AddWithValue("ema_perno", txtPerno.Text.ToString())
            End If
            If ddlSgrade.SelectedValue <> 0 And ddlMailType.SelectedValue > 0 Then
                qry.Parameters.AddWithValue("EMA_EMPL_SGRADE", ddlSgrade.SelectedItem.Text.ToString)
            End If
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                btnopt.Visible = True
                gvself.DataSource = dt
                gvself.DataBind()
            Else
                btnopt.Visible = False
                gvself.DataSource = Nothing
                gvself.DataBind()
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Protected Sub ddlMailType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMailType.SelectedIndexChanged
        Try
            Dim qry As New OracleCommand()
            If ddlMailType.SelectedValue = 1 Then
                txtSubject.Text = "End Year 360-degree Survey : Identification of Respondents"
                qry.CommandText = "select count(*) cnt from hrps.t_emp_master_feedback360 emf where ema_perno not in (select ee_pno from hrps.t_emp_excluded) and ema_email_id is not null and ema_eqv_level =:ema_eqv_level and ema_eqv_level not in ('I1') and ema_year=:ema_year and ema_cycle=:ema_cycle"
            ElseIf ddlMailType.SelectedValue = 2 Then
                txtSubject.Text = "REMINDER : End Year 360-degree Survey : Identification of Respondents - DEADLINE "
                qry.CommandText = "select count(*) cnt from hrps.t_emp_master_feedback360 emf where ema_perno not in (select ee_pno pno from hrps.t_emp_excluded union select ss_asses_pno pno from hrps.t_survey_status where ss_categ='Self' and ss_wfl_status in ('1','2','3') and ss_year=:ema_year and ss_srlno=:ema_cycle) and ema_email_id is not null and ema_eqv_level =:ema_eqv_level and ema_eqv_level not in ('I1')"
            ElseIf ddlMailType.SelectedValue = 3 Then
                txtSubject.Text = "End Year 360-degree Survey : Approval of Respondents "
                qry.CommandText = "select count(distinct emf.ema_perno) cnt from hrps.t_emp_master_feedback360 emf,t_survey_status ss where emf.ema_perno=ss.ss_approver and ss.ss_year=:ema_year and ss.ss_srlno=:ema_cycle and ss.ss_wfl_status ='1' and ss.ss_app_tag is null and emf.ema_eqv_level =:ema_eqv_level and emf.ema_email_id is not null"
            ElseIf ddlMailType.SelectedValue = 4 Then
                txtSubject.Text = "REMINDER : End Year 360-degree Survey : Approval of Respondents - DEADLINE "
                qry.CommandText = "select count(distinct emf.ema_perno) cnt from hrps.t_emp_master_feedback360 emf,t_survey_status ss where emf.ema_perno=ss.ss_approver and ss.ss_year=:ema_year and ss.ss_srlno=:ema_cycle and ss.ss_wfl_status ='1' and ss.ss_app_tag is null and emf.ema_eqv_level =:ema_eqv_level and emf.ema_email_id is not null"
            ElseIf ddlMailType.SelectedValue = 5 Then
                txtSubject.Text = " "

            End If

            If txtPerno.Text.Trim().ToString <> "" Then
                qry.CommandText += " and emf.ema_perno=:ema_perno"
            End If

            If ddlSgrade.SelectedValue <> 0 And ddlMailType.SelectedValue > 0 Then
                qry.CommandText += " and emf.EMA_EMPL_SGRADE=:EMA_EMPL_SGRADE"
            End If

            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_year", txtYear.Text.ToString())
            qry.Parameters.AddWithValue("ema_cycle", txtCycle.Text.ToString())
            qry.Parameters.AddWithValue("ema_eqv_level", ddlLevel.SelectedValue.ToString())
            If txtPerno.Text.Trim().ToString <> "" Then
                qry.Parameters.AddWithValue("ema_perno", txtPerno.Text.Trim().ToString())
            End If
            If ddlSgrade.SelectedValue <> 0 And ddlMailType.SelectedValue > 0 Then
                qry.Parameters.AddWithValue("EMA_EMPL_SGRADE", ddlSgrade.SelectedItem.Text.ToString())
            End If
            Dim da As New OracleDataAdapter(qry)
            Dim dt1 As New DataTable()
            da.Fill(dt1)
            If dt1.Rows.Count > 0 Then
                lblCount.Text = dt1.Rows(0)(0).ToString + " Records"
            Else
                lblCount.Text = "0"
                btnopt.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ddlLevel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlLevel.SelectedIndexChanged
        ddlMailType_SelectedIndexChanged(sender, e)
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Protected Sub chklstmailtype_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            'If chklstmailtype.SelectedValue = "mtil1" Then
            '    txtEndDate.Visible = False
            'ElseIf chklstmailtype.SelectedValue = "mtorn" Then
            '    txtEndDate.Visible = True
            'ElseIf chklstmailtype.SelectedValue = "mtr" Then
            '    txtEndDate.Visible = True
            '    txtdays.Visible = True
            'ElseIf chklstmailtype.SelectedValue = "otm" Then
            '    txtEndDate.Visible = True
            'ElseIf chklstmailtype.SelectedValue = "rtil1app" Then
            '    txtEndDate.Visible = False
            'ElseIf chklstmailtype.SelectedValue = "rtil2fin" Then
            '    txtEndDate.Visible = True
            'ElseIf chklstmailtype.SelectedValue = "rmtoresp" Then
            '    txtEndDate.Visible = True
            '    txtdays.Visible = True
            'End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub txtPerno_TextChanged(sender As Object, e As EventArgs)
        ddlMailType_SelectedIndexChanged(sender, e)
    End Sub
    Protected Sub ddlSgrade_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSgrade.SelectedIndexChanged
        ddlMailType_SelectedIndexChanged(sender, e)
    End Sub
End Class
