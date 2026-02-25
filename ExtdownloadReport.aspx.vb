Imports System.Data.OracleClient
Imports System.Data
Imports System.Web.Mail
Imports System.Net.Mail
Imports System.Net
Imports System.Data.OracleClient.OracleCommand

Partial Class ExtdownloadReport
    Inherits System.Web.UI.Page


    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Public Sub getsrlno()
        Try
            Dim srlno As New OracleCommand()
            srlno.CommandText = "select IRC_DESC from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
            Dim dtSrlno = getRecordInDt(srlno, conHrps)

            If dtSrlno.Rows.Count > 0 Then
                ViewState("SRLNO") = dtSrlno.Rows(0)(0)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub GetFy()
        Try
            Dim s As New OracleCommand()
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim f = getRecordInDt(s, conHrps)

            If f.Rows.Count > 0 Then
                ViewState("FY") = f.Rows(0)(0)
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function ChkMail(mail As String) As Boolean
        Dim val As Boolean = True
        Try
            Dim email = mail.Split("@")
            If email(1).ToUpper = UCase("TATASTEEL.COM") Then
                val = False
            Else
                val = True
            End If
        Catch ex As Exception

        End Try
        Return val
    End Function
    Protected Sub btnopt_Click(sender As Object, e As EventArgs)
        Try
            Session.Remove("otpsend")
            If ChkMail(txtotp.Text.ToUpper()) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "This page is not for Tata Steel employees. Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If
            If txtotp.Text.Trim() <> "" Then
                Dim mail As String = String.Empty
                mail = txtotp.Text.ToUpper()

                Dim qry As String = String.Empty
                Dim cmdotp As New OracleCommand()
                cmdotp.CommandText = "select distinct nvl(SS_OTP_COUNT,'0') SS_OTP_COUNT ,nvl(SS_FLAG4,'0') SS_FLAG4 from t_survey_status where upper(SS_EMAIL)=:SS_EMAIL"
                cmdotp.CommandText += " And SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO  and SS_DEL_TAG='N'  and ss_app_tag='AP' and ss_wfl_status<>'1' order by 1,2 desc"
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If

                'cmdotp.CommandText = qry
                cmdotp.Parameters.Clear()
                cmdotp.Parameters.AddWithValue("SS_EMAIL", mail)
                cmdotp.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
                cmdotp.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString)
                'cmdotp = New OracleCommand(qry, conHrps)
                cmdotp.Connection = conHrps
                Dim gh As New DataTable()

                Dim dtls As New DataTable()
                Dim da As New OracleDataAdapter(cmdotp)
                da.Fill(gh)
                If gh.Rows.Count > 0 Then


                    Dim f As Boolean = False
                    Dim g = SimpleCrypt(mail)
                    Dim a = SimpleCrypt(g)
                    If btnopt.Text = "Get Otp" Then
                        updateEmail(mail)
                        If CInt(gh.Rows(0)("SS_FLAG4")) >= 50 Then
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "Your authorization for viewing report is locked due to multiple unsuccessful attempts....!")
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

                                If CInt(gh.Rows(0)("SS_OTP_COUNT")) >= 20 Then
                                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Your authorization for viewing report is locked due to multiple unsuccessful attempts.")
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
                        If conHrps.State = ConnectionState.Closed Then
                            conHrps.Open()
                        End If
                        cm.CommandText = "select * from t_survey_status where upper(SS_EMAIL)=:SS_EMAIL and SS_YEAR=:SS_YEAR and SS_INTSH_OTP=:SS_INTSH_OTP and  SS_SRLNO=:SS_SRLNO and ss_categ='Self'"
                        cm.Parameters.Clear()
                        cm.Parameters.AddWithValue("SS_EMAIL", mail)
                        cm.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                        cm.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                        cm.Parameters.AddWithValue("SS_INTSH_OTP", txtemail.Text.Trim())
                        cm.Connection = conHrps
                        Dim d As New DataTable()
                        Dim da1 As New OracleDataAdapter(cm)
                        da1.Fill(d)
                        If d.Rows.Count > 0 And f Then
                            Session("otpsend") = g & SimpleCrypt(txtemail.Text.Trim)
                            Session("statusotp") = "Y"
                            'Response.Redirect("feedback_OPR.aspx?id=" & Session("otpsend"))
                            Response.Redirect("FeedbackSurveyRptTGExt.aspx?pno=" & SimpleCrypt(d.Rows(0)("SS_ASSES_PNO").ToString) & "&yr=" & SimpleCrypt(ViewState("FY").ToString()) & "&cyc=" & SimpleCrypt(ViewState("SRLNO").ToString()) & "&id=" & Session("otpsend"))
                        Else
                            ShowGenericMessageModal(CommonConstants.AlertType.info, "Captcha or mail id not matched...!")
                            fetchcaptcha()
                            updateWrongOTP(mail.ToUpper)
                        End If
                    End If
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.warning, "You have not been assessed.")
                End If
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please enter mail id for geting OTP...")
            End If

        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, ex.ToString())
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
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
            MsgBox(ex.Message)
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
            Dim sql As String = String.Empty
            Dim c As New OracleCommand()
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim tran_Ins As OracleTransaction
            tran_Ins = conHrps.BeginTransaction()
            sql = "update t_survey_status set SS_INTSH_OTP=:SS_INTSH_OTP,SS_INTSH_IP=:SS_INTSH_IP,SS_INTSH_BW=:SS_INTSH_BW,"
            sql += "SS_INTSH_DV=:SS_INTSH_DV where upper(SS_EMAIL) =:SS_EMAIL and SS_YEAR =:SS_YEAR and SS_SRLNO=:SS_SRLNO"
            c.Connection = conHrps
            c.CommandText = sql
            c.Parameters.Add(New OracleParameter(":SS_INTSH_OTP", otp))
            c.Parameters.Add(New OracleParameter(":SS_INTSH_IP", yu.ToString()))
            c.Parameters.Add(New OracleParameter(":SS_INTSH_BW", h.ToString()))
            c.Parameters.Add(New OracleParameter(":SS_INTSH_DV", dvnm.ToString()))
            c.Parameters.Add(New OracleParameter(":SS_EMAIL", id.ToUpper()))
            c.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            c.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
            c.Transaction = tran_Ins
            c.ExecuteNonQuery()
            tran_Ins.Commit()


        Catch ex As Exception
            ' tran_Ins.Rollback()
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub updateEmail(id As String)
        Try
            Dim c As New OracleCommand()
            Dim sql As String = String.Empty

            sql = "update t_survey_status set SS_FLAG4=nvl(SS_FLAG4,'0') + 1 where upper(SS_EMAIL) =:SS_EMAIL and SS_YEAR =:SS_YEAR and SS_SRLNO=:SS_SRLNO"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            c = New OracleCommand(sql, conHrps)
            c.Parameters.Add(New OracleParameter(":SS_EMAIL", id.ToUpper()))
            c.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            c.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
            c.ExecuteNonQuery()

        Catch ex As Exception
            'MsgBox(ex.Message)
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
            mail.From = New MailAddress("tips.hrps@tatasteel.com", "360 Feedback ", System.Text.Encoding.UTF8)

            mail.Subject = "Feedback 360 OTP"

            mail.SubjectEncoding = System.Text.Encoding.UTF8
            mail.Body = body

            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.IsBodyHtml = True

            Dim client As New SmtpClient()
            client.Credentials = New System.Net.NetworkCredential("360FEEDBACK@TATASTEEL.COM", "")
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

    Private Sub Survey_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Session("errorMsg") = "The page you were looking for is not valid."
            Response.Redirect("errorpage.aspx", True)
            Exit Sub
            getsrlno()
            GetFy()
            If Not IsPostBack Then
                fetchcaptcha()
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
            Dim sql As String = String.Empty
            Dim c As New OracleCommand()
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            sql = "update t_survey_status set SS_FLAG_WOTP='Y',SS_OTP_COUNT= NVL(SS_OTP_COUNT,'0') + 1  where upper(SS_EMAIL) =:SS_EMAIL and SS_YEAR =:SS_YEAR and SS_SRLNO=:SS_SRLNO"

            c.Connection = conHrps
            c.CommandText = sql
            c.Parameters.Add(New OracleParameter(":SS_EMAIL", mails))
            c.Parameters.Add(New OracleParameter(":SS_YEAR", ViewState("FY").ToString()))
            c.Parameters.Add(New OracleParameter(":SS_SRLNO", ViewState("SRLNO").ToString()))
            c.ExecuteNonQuery()


        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
End Class
