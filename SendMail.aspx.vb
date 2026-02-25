
Imports System.IO
Imports System.Net.Mail
Imports System.Data.OracleClient
Imports System.Data
Imports System.Net
Imports System.Data.OleDb
Imports ClosedXML.Excel

Partial Class SendMail
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Shared conStr As String
    Shared FilePath As String
    Public Sub One_time_mail_toIL2(enddt As String)
        Try

            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty
            Dim link As String
            link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/selectassesor.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/selectassesor.aspx <a/>"

            body = "Dear Colleague,<br/><br/> We are extending the date for finalizing the list of respondents for evaluative 360-degree feedback, by a couple of days."
            body += " <br/><br/> Request you to submit the same by tomorrow in the system, using the link given below: <br/><br/> " & link
            body += " <br/><br/> In case of any query, please contact Ms. Shruti Choudhury, Head HRM Leadership Development."
            body += "<br/><br/><br/> With regards,<br/> Zubin Palia<br/> Chief Group HR & IR"

            '  mail.Bcc.Add("mukul.mishra@partners.tatasteel.com") Mukul
            ' mail.Bcc.Add("avirup.bhowmick@tatasteel.com")
            'mail.Bcc.Add("shashikala.t@tatasteel.com")
            'mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            ' mail.Subject = "End Year 360 assessment"
            'mail.Subject = "End Year 360 assessment | Date extended by tomorrow"

            'mail.SubjectEncoding = System.Text.Encoding.UTF8
            'mail.Body = body

            'mail.BodyEncoding = System.Text.Encoding.UTF8
            'mail.IsBodyHtml = True
            ' Dim data As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("images\User_manual_360 DEGREE_Select Assessor.pdf"))
            'Data.Name = "User_manual_360 DEGREE_Select Assessor.pdf"
            ' mail.Attachments.Add(data)
            'Dim client As New SmtpClient()
            'client.Credentials = New System.Net.NetworkCredential("tips.hrps@tatasteel.com", "")
            'client.Port = 25
            '' client.Host = "144.0.11.253"
            'client.Host = "144.0.16.7"
            'client.EnableSsl = False
            'client.Send(Mail)
            'client.Dispose()
            'ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")
            Dim perno As String() = {"162523", "000052", "115351", "115442", "119046", "119852", "119907", "119960", "119992", "120041", "120393", "122185", "123418", "123491",
"123830", "127290", "127982", "128364", "128411", "147876", "147897", "148315", "148482", "148486", "148516", "148523", "149555",
"149633", "149729", "149786", "150351", "158404", "162410", "163730", "165004", "174357", "174782", "175089", "175163", "197576",
"197582", "197977", "198850", "198893", "200056", "218475", "269377", "388078", "500228", "503102", "805021", "151629"}
            For hg = 0 To perno.Count - 1
                ' Dim mail As New System.Net.Mail.MailMessage()
                Dim fg As New OracleCommand()
                fg.CommandText = "Select  distinct ema_perno ,ema_email_id from tips.t_empl_all where ema_perno='" & perno(hg).ToString() & "'"
                Dim df = getRecordInDt(fg, conHrps)
                If df.Rows.Count > 0 Then
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
                End If
                '    ' Threading.Thread.Sleep(1000)
            Next

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
            Dim qry As New OracleCommand()

            qry.CommandText = "select distinct UPPER(ss_email) ss_email ,ss_name from hrps.t_survey_status where ss_year='" & ViewState("FY") & "'  and ss_app_tag='AP'"
            qry.CommandText += " and ss_del_tag='N' and ss_wfl_status=2 and trunc(ss_tag_dt)< trunc(sysdate-" & txtdays.Text.Trim & ")"


            Dim dts = getRecordInDt(qry, conHrps)

            If dts.Rows.Count > 0 Then
                For gh = 0 To dts.Rows.Count - 1

                    Dim mail As New System.Net.Mail.MailMessage()
                    Dim body As String = String.Empty

                    Dim link As String
                    link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/Feedback.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/Feedback.aspx <a/>"


                    Dim link1 As String
                    link1 = "<a href='https://tslhr.tatasteel.co.in/Feedback_360/survey.aspx'> https://tslhr.tatasteel.co.in/Feedback_360/survey.aspx <a/>"

                    body = " Dear " & dts.Rows(gh)("ss_name").ToString() & ",<br/><br/> As communicated earlier, you have been nominated as one of the respondents to give feedback on"
                    body += " behaviours for one or more Officers. This is a gentle reminder to complete the same by <b>" & txtEndDate.Text.Trim() & ".</b> <br/><br/>"
                    body += " For Laptop/Desktop users kindly copy the link given below in Chrome/Edge browser and open. Mobile/iPad "
                    body += "users may click on the same link to open the page in Edge browser.<br/><br/> Link for Employees <br/>" & link & " <br/><br/> Link for external users"
                    body += " <br/> " & link1 & " <br/><br/>The link is unique to you. Please do not forward or share it with anyone. The feedback provided will be strictly "
                    body += "confidential and will not be disclosed under any circumstances.<br/><br/>"
                    body += "In case of any other queries, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development."
                    body += "  <br/><br/><br/><br/> With regards, <br/> Zubin Palia <br/> Chief Group HR & IR"

                    mail.Bcc.Add(dts.Rows(gh)("ss_email").ToString())
                    ' mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
                    ' mail.Bcc.Add("medha.chaturvedi@tatasteel.com")
                    mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

                    mail.Subject = "Reminder To complete the End Year 360 assessment"

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
                Next
            End If
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Public Sub Reminder_to_il2(enddt As String)
        Try
            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty

            Dim link As String
            link = "<a href='http://webappsdev01.corp.tatasteel.com/Feedback360/selectassesor.aspx'> http://webappsdev01.corp.tatasteel.com/Feedback360/selectassesor.aspx <a/>"

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

    Public Sub Reminder_to_il1()
        Try
            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty

            Dim link As String
            link = "<a href='https://irisapp.corp.tatasteel.com/feedback_360/SurveyApproval.aspx'> https://irisapp.corp.tatasteel.com/feedback_360/SurveyApproval.aspx <a/>"

            body = "Dear Ma’am/ Sir, <br/><br/> As communicated earlier, you are requested to approve the list of respondents for the evaluative "
            body += "360-degree assessment for the IL2s in your area.  There are one or more IL2s, for whom your approval is still pending. <br/>"
            body += "Request you to kindly approve the same using the link below. <br/> <br/> For Laptop/Desktop users kindly copy the link given "
            body += "below in Chrome/Edge browser and open. <br/> <br/> " & link & " <br/> <br/> Mobile/iPad users may click on the above link to "
            body += "open the page in Edge browser. <br/> <br/> Please find enclosed a user manual describing steps for approving the list of respondents.<br/>"
            body += " In case of any IT related issues, please connect with Mr. Avirup Bhowmick (avirup.bhowmick@tatasteel.com). <br/>"
            body += "In case of any other queries, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development."
            body += "  <br/><br/><br/><br/> With regards, <br/> Zubin Palia <br/> Chief Group HR & IR"

            mail.Bcc.Add("shashikala.t@tatasteel.com")
            mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
            'mail.Bcc.Add("rajiv.mukerji@tatasteel.com")
            'mail.Bcc.Add("AVNEESH.GUPTA@TATASTEEL.COM")
            'mail.Bcc.Add("SANJIVPAUL@TATASTEEL.COM")
            'mail.Bcc.Add("DIBYENDUB@TATASTEEL.COM")
            'mail.Bcc.Add("ATRAYEE@TATASTEEL.COM")
            'mail.Bcc.Add("PROBAL.GHOSH@TATASTEEL.COM")
            'mail.Bcc.Add("UTTAM.SINGH@TATASTEEL.COM")
            'mail.Bcc.Add("KOUSHIK.CHATTERJEE@TATASTEEL.COM")
            'mail.Bcc.Add("DEBASHISH.BHATTACHARJEE@TATASTEEL.COM")
            'mail.Bcc.Add("PEEYUSH@TATASTEEL.COM")
            'mail.Bcc.Add("CHANAKYA@TATASTEEL.COM")
            'mail.Bcc.Add("SUNDAR.RAMAN@TATASTEEL.COM")
            'mail.Bcc.Add("shruti.choudhury@tatasteel.com")

            mail.Bcc.Add("debashish.bhattacharjee@tatasteel.com")
            mail.Bcc.Add("Sdibyendub@tatasteel.com")
            'mail.Bcc.Add("DIBYENDUB@TATASTEEL.COM")
            'mail.Bcc.Add("UTTAM.SINGH@TATASTEEL.COM")
            'mail.Bcc.Add("KOUSHIK.CHATTERJEE@TATASTEEL.COM")
            'mail.Bcc.Add("DEBASHISH.BHATTACHARJEE@TATASTEEL.COM")
            'mail.Bcc.Add("PEEYUSH@TATASTEEL.COM")
            'mail.Bcc.Add("CHANAKYA@TATASTEEL.COM")


            ' mail.Bcc.Add("medha.chaturvedi@tatasteel.com")
            mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            mail.Subject = "Reminder for Approving the List of Respondents"

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
            Dim link As String
            Dim name As String = String.Empty
            Dim qry As New OracleCommand()

            qry.CommandText = "select distinct UPPER(ss_email) ss_email ,ss_name from hrps.t_survey_status where ss_year='" & ViewState("FY").ToString() & "'  and ss_app_tag='AP'"
            qry.CommandText += " and ss_del_tag='N' and ss_wfl_status=2 and trunc(ss_tag_dt)>=trunc(sysdate-" & txtdays.Text.Trim & ")"

            Dim g = getRecordInDt(qry, conHrps)
            If g.Rows.Count > 0 Then
                For p = 0 To g.Rows.Count - 1
                    'If g.Rows(p)("ss_type") = "NORG" Then
                    link = "<a href=' https://tslhr.tatasteel.co.in/Feedback_360/survey.aspx'>  https://tslhr.tatasteel.co.in/Feedback_360/survey.aspx <a/>"
                    'ElseIf g.Rows(p)("ss_type") = "ORG" Then

                    '    link = "<a href='http://webappsdev01.corp.tatasteel.com/Feedback360/feedback.aspx'> http://webappsdev01.corp.tatasteel.com/Feedback360/feedback.aspx <a/>"
                    'End If
                    Dim link1 = "<a href=' https://irisapp.corp.tatasteel.com/feedback_360/Feedback.aspx'>  https://irisapp.corp.tatasteel.com/feedback_360/Feedback.aspx <a/>"
                    Dim mail As New System.Net.Mail.MailMessage()
                    Dim body As String = String.Empty

                    body = "Dear " & g.Rows(p)("ss_name").ToString() & " <br/><br/> As we aspire to be the most valuable and respected steel company globally in the next 5-10"
                    body += " years, we are developing agile behaviours in our top leadership – accountability, responsiveness, collaboration and people development. As we draw near to the end of this "
                    body += "FY, we are commencing the evaluative 360-degree assessment for officers on these behaviours.<br/><br/>"
                    body += "You have been nominated as one of the respondents to give feedback on behaviours for one or more officers. Request you to kindly provide feedback using the attached link by <b>" & txtEndDate.Text.Trim() & ".</b> <br/><br/>"
                    body += "For Laptop/Desktop users kindly copy the link given below in Chrome/Edge browser and open. <br/>"
                    body += "Mobile/iPad users may click on the same link to open the page in Edge browser. <br/><br/> Link for Employees <br/> " & link1
                    body += " <br/><br/> Link for external users <br/> " & link & " <br/><br/>"
                    body += "The link is unique to you. Please do not forward or share it with anyone. The feedback provided will be strictly confidential and will not be disclosed under any circumstances."

                    body += " <br/><br/> In case of any other queries, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development."
                    body += "  <br/><br/><br/><br/> With regards, <br/> Zubin Palia <br/> Chief Group HR & IR"


                    ' mail.Bcc.Add("shashikala.t@tatasteel.com")
                    mail.Bcc.Add(g.Rows(p)("ss_email").ToString())
                    ' mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
                    ' mail.Bcc.Add("medha.chaturvedi@tatasteel.com")
                    mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

                    mail.Subject = "End-year 360 Feedback"

                    mail.SubjectEncoding = System.Text.Encoding.UTF8
                    mail.Body = body

                    ' Dim data As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("images\User_manual_360 DEGREE FEEDBACK.pdf"))
                    ' data.Name = "User_manual_360 DEGREE FEEDBACK.pdf"
                    ' mail.Attachments.Add(data)

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

                Next
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Mail has been sent...!")

            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub SendMail_Load(sender As Object, e As EventArgs) Handles Me.Load
        For i As Integer = 0 To chklstmailtype.Items.Count - 1
            chklstmailtype.Items(i).Attributes.Add("onclick", "MutExChkList(this)")
        Next
        If Not IsPostBack Then
            'Mail_to_res()
        End If
        Dim r As New OracleCommand()
        r.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY from dual"
        Dim g = getRecordInDt(r, conHrps)
        If g.Rows.Count > 0 Then
            ViewState("FY") = g.Rows(0)(0).ToString()
        End If
        'Dim yt = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        'If String.IsNullOrEmpty(yt) Then
        '    Dim f = Request.ServerVariables("REMOTE_ADDR")
        '    Dim g = ""
        'End If
        ' Response.Redirect("errorpage.aspx", True)
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
            ' readfile()
            ' Exit Sub
            If chklstmailtype.SelectedValue <> "" Then


                If chklstmailtype.SelectedValue = "mtil1" Then
                    Mail_to_IL1()
                ElseIf chklstmailtype.SelectedValue = "mtorn" Then
                    If txtEndDate.Text.Trim() = "" Then
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select end date...!")
                        Exit Sub
                    End If
                    Mail_to_IL1_when_reject(txtEndDate.Text.Trim)
                    txtEndDate.Text = ""
                ElseIf chklstmailtype.SelectedValue = "mtr" Then
                    If txtEndDate.Text.Trim() = "" And txtdays.Text.Trim() Then
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select end date and enter days...!")
                        Exit Sub
                    End If
                    Mail_to_res(txtEndDate.Text.Trim)
                ElseIf chklstmailtype.SelectedValue = "otm" Then
                    If txtEndDate.Text.Trim() = "" Then
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select end date...!")
                        Exit Sub
                    End If
                    One_time_mail_toIL2(txtEndDate.Text.Trim)
                ElseIf chklstmailtype.SelectedValue = "rtil1app" Then
                    Reminder_to_il1()
                ElseIf chklstmailtype.SelectedValue = "rtil2fin" Then
                    If txtEndDate.Text.Trim() = "" Then
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select end date...!")
                        Exit Sub
                    End If
                    Reminder_to_il2(txtEndDate.Text.Trim)
                    txtEndDate.Text = ""
                ElseIf chklstmailtype.SelectedValue = "rmtoresp" And txtdays.Text.Trim() Then
                    If txtEndDate.Text.Trim() = "" Then
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select end date and enter days...!")
                        Exit Sub
                    End If
                    Reminder_to_respondent(txtEndDate.Text.Trim)
                    txtEndDate.Text = ""
                End If
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select mail typee to sending mail...!")
                Exit Sub
                txtEndDate.Text = ""
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

    Public Sub Mail_to_IL1_when_reject(enddate As String)
        Try
            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty

            body = " Dear Mukul,<br/><br/> The list of respondents which you had submitted for evaluative 360-degree assessment for IL2s, as a part of the end "
            body += "year performance management process, has been returned by your concerned IL1.<br/><br/> You are requested to modify the list of respondents"
            body += " respondents as per comments/ instructions of your IL1 and submit the same by " & enddate & ". <br/><br/>"
            body += " If you have a query, please connect with Ms. Shruti Choudhury, Head HRM Leadership Development or Mr. Vikas Kumar, Head HRM Performance"
            body += " Management & Talent Development. <br/><br/>"
            body += " <br/><br/><br/> With regards,<br/> Zubin Palia<br/> Chief Group HR & IR"
            mail.Bcc.Add("shashikala.t@tatasteel.com")
            mail.Bcc.Add("mukul.mishra@partners.tatasteel.com")
            ' mail.Bcc.Add("medha.chaturvedi@tatasteel.com")
            mail.From = New MailAddress("hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            mail.Subject = "List of respondents for end-year 360-degree feedback returned"

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


    Protected Sub chklstmailtype_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            If chklstmailtype.SelectedValue = "mtil1" Then
                txtEndDate.Visible = False
            ElseIf chklstmailtype.SelectedValue = "mtorn" Then
                txtEndDate.Visible = True
            ElseIf chklstmailtype.SelectedValue = "mtr" Then
                txtEndDate.Visible = True
                txtdays.Visible = True
            ElseIf chklstmailtype.SelectedValue = "otm" Then
                txtEndDate.Visible = True
            ElseIf chklstmailtype.SelectedValue = "rtil1app" Then
                txtEndDate.Visible = False
            ElseIf chklstmailtype.SelectedValue = "rtil2fin" Then
                txtEndDate.Visible = True
            ElseIf chklstmailtype.SelectedValue = "rmtoresp" Then
                txtEndDate.Visible = True
                txtdays.Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SendMail_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            Dim strUserID As String = ""
            Dim vUserFullName As String = Page.User.Identity.Name
            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            If GetPno(strUserID.ToUpper()) = False Then
                Response.Redirect("errorpage.aspx", True)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
