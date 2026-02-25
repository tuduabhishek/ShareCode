Imports System.Data.OracleClient
Imports System.Data
Imports System.Web.Mail
Imports System.Net.Mail
Imports System.Net

Partial Class SurveyAdm
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Protected Sub btnopt_Click(sender As Object, e As EventArgs)
        Try
            If txtotp.Text.Trim() <> "" Then
                Dim mail As String = String.Empty
                mail = txtotp.Text

                Dim qry As New OracleCommand()
                qry.CommandText = "select distinct nvl(SS_OTP_COUNT,'0')SS_OTP_COUNT ,nvl(SS_FLAG4,'0')SS_FLAG4 from t_survey_status where upper(SS_EMAIL)=:SS_EMAIL "
                qry.CommandText += " And ss_year=:ss_year order by 1,2 desc"
                qry.Connection = conHrps
                qry.Parameters.Clear()
                qry.Parameters.AddWithValue("SS_EMAIL", mail.ToUpper)
                qry.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
                Dim da As New OracleDataAdapter(qry)
                Dim gh As New DataTable()
                da.Fill(gh)
                If gh.Rows.Count > 0 Then


                    Dim f As Boolean = False
                    Dim g = SimpleCrypt(mail)
                    Dim a = SimpleCrypt(g)
                    If btnopt.Text = "Get Otp" Then
                        updateEmail(mail)
                        If CInt(gh.Rows(0)("SS_FLAG4")) >= 50 Then
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "Your authorisation for giving feedback is locked please contact system administrator...!")
                            Exit Sub
                        End If
                        Dim r As New Random()
                        Dim otp = r.Next(1000, 9999)
                        updateOtp(mail, otp)
                        btnopt.Text = "Validate OTP"
                        ViewState("otp") = otp
                        SentOtp(otp, mail)
                        txtemail.Visible = True
                        txtcaptcha.Visible = True
                        btncapt.Visible = True
                        lbcap.Visible = True
                        ShowGenericMessageModal(CommonConstants.AlertType.success, "OTP sent to your mail please check..")
                    Else

                        If btncapt.InnerText = txtcaptcha.Text.Trim() Then
                            f = True
                        Else
                            f = False
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "Captcha code not matched...!")
                            fetchcaptcha()
                            Exit Sub
                        End If
                        If CInt(ViewState("otp")) = txtemail.Text.Trim Then

                        Else
                            If gh.Rows.Count > 0 Then

                                If CInt(gh.Rows(0)("SS_OTP_COUNT")) >= 5 Then
                                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Your authorisation for giving feedback is locked please contact system administrator...!")
                                    Exit Sub
                                Else
                                    ShowGenericMessageModal(CommonConstants.AlertType.info, "OTP not matched...!")
                                    fetchcaptcha()
                                    updateWrongOTP(mail.ToUpper)
                                    Exit Sub
                                End If
                            End If
                            '  Exit Sub
                        End If
                        Dim cm As New OracleCommand()
                        cm.CommandText = "select * from t_survey_status where upper(SS_EMAIL)='" & mail.ToUpper & "' and SS_YEAR= '" & ViewState("FY") & "' and SS_INTSH_OTP='" & txtemail.Text.Trim() & "' "
                        Dim d = getRecordInDt(cm, conHrps)
                        If d.Rows.Count > 0 And f Then
                            Response.Redirect("feedback.aspx?id=" & g & SimpleCrypt(txtemail.Text.Trim))
                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "Captcha or mail id not matched...!")
                            fetchcaptcha()
                            updateWrongOTP(mail.ToUpper)
                        End If
                    End If
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.warning, "You have not been selected to provide feedback...!")
                End If
            Else
                    ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please enter mail id for geting OTP...")
                End If

        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.ToString())
        End Try
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

    Public Function SimpleCrypt(ByVal Text As String) As String
        Dim strTempChar As String = "", i As Integer
        For i = 1 To Text.Length
            If Asc(Mid$(Text, i, 1)) < 128 Then
                strTempChar = CType(Asc(Mid$(Text, i, 1)) + 128, String)
            ElseIf Asc(Mid$(Text, i, 1)) > 128 Then
                strTempChar = CType(Asc(Mid$(Text, i, 1)) - 128, String)
            End If
            Mid$(Text, i, 1) = Chr(CType(strTempChar, Integer))
        Next i
        Return Text

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
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function
    Public Sub updateOtp(id As String, otp As Integer)
        Try
            Dim g = Request.Browser
            Dim h = g.Browser
            Dim ds As System.Web.HttpBrowserCapabilities = Request.Browser
            Dim jh = ds.IsMobileDevice
            Dim dvnm As String = String.Empty
            If jh Then
                dvnm = "Mobile"
            Else
                dvnm = "Desktop"
            End If
            Dim hostName As String = Dns.GetHostName()
            Dim yu = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString()
            Dim jg = Request.UserAgent
            Dim c As New OracleCommand()
            c.CommandText = "update t_survey_status set SS_INTSH_OTP='" & otp & "',SS_INTSH_IP='" & yu.ToString() & "', SS_INTSH_BW='" & h.ToString() & "',"
            c.CommandText += "SS_INTSH_DV='" & dvnm.ToString() & "' where upper(SS_EMAIL) ='" & id.ToUpper() & "' and SS_YEAR ='" & ViewState("FY").ToString() & "' "
            c.Connection = conHrps

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
                c.ExecuteNonQuery()
            End If
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub updateEmail(id As String)
        Try
            Dim c As New OracleCommand()
            c.CommandText = "update t_survey_status set SS_FLAG4=nvl(SS_FLAG4,'0') + 1 where upper(SS_EMAIL) =:SS_EMAIL and SS_YEAR =:SS_YEAR"
            c.Connection = conHrps
            c.Parameters.Clear()
            c.Connection = conHrps
            c.Parameters.AddWithValue("SS_EMAIL", id.ToUpper())
            c.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
                c.ExecuteNonQuery()
            End If
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub SentOtp(otp As Integer, mails As String)
        Try
            Dim mail As New System.Net.Mail.MailMessage()
            Dim body As String = String.Empty

            body = " Dear Sir/Mam,<br/><br/> Feedback 360 OTP :-" & otp & "<br/><br/> Thanks <br/><br/> This is system generated mail please do not reply."

            mail.Bcc.Add(mails)
            mail.From = New MailAddress("Hrm@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            mail.Subject = "Feedback 360 OTP"

            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.Body = body

            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.IsBodyHtml = True

            Dim client As New SmtpClient()
            client.Credentials = New System.Net.NetworkCredential("Hrm@tatasteel.com", "")
            client.Port = 25
            client.Host = "144.0.11.253"
            ' client.Host = "144.0.16.7"
            client.EnableSsl = False
            client.Send(mail)
            client.Dispose()


        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.ToString())
        End Try
    End Sub

    Private Sub SurveyAdm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim s1 As New OracleCommand()
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s1.CommandText = "Select IRC_DESC from t_ir_codes where irc_type='360DT' and IRC_VALID_TAG='A'"
            Dim f1 = getRecordInDt(s1, conHrps)

            If f1.Rows.Count > 0 Then
                ViewState("FY") = f1.Rows(0)(0)
            Else
                Dim s As New OracleCommand()
                's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
                s.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY FROM DUAL"
                Dim f = getRecordInDt(s, conHrps)

                If f.Rows.Count > 0 Then
                    ViewState("FY") = f.Rows(0)(0)
                End If
            End If


        Catch ex As Exception

        End Try
    End Sub

    Public Sub fetchcaptcha()
        Try

            Dim randomText As StringBuilder = New StringBuilder()
            Dim alphabets As String = "012345679ACEFGHKLMNPRSWXZabcdefghijkhlmnopqrstuvwxyz"
            Dim r As Random = New Random()

            For j As Integer = 0 To 5
                randomText.Append(alphabets(r.[Next](alphabets.Length)))
            Next

            'lbcaptcha.Text = randomText.ToString()
            btncapt.InnerText = randomText.ToString()

        Catch ex As Exception

        End Try
    End Sub

    Public Sub updateWrongOTP(mails As String)
        Try
            Dim c As New OracleCommand()
            c.CommandText = "update t_survey_status set SS_FLAG_WOTP='Y',SS_OTP_COUNT= NVL(SS_OTP_COUNT,'0') + 1  where upper(SS_EMAIL) ='" & mails & "' and SS_YEAR ='" & ViewState("FY").ToString() & "'"
            c.Connection = conHrps

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
                c.ExecuteNonQuery()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
