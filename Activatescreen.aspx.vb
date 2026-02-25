Imports System.Data.OracleClient
Imports System.Data
Imports System
Imports System.IO
Imports System.Globalization

Partial Class Activate_Page
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Private Sub Activate_Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetFy()
        Dim vUserFullName As String = Page.User.Identity.Name   '155710'
        Dim strUserID As String = ""
        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If
        'strUserID = "197838"
        CheckSupAdm(strUserID)
    End Sub
    Public Sub CheckSupAdm(pno As String)
        Try
            Dim qry As String = String.Empty
            qry = "select * from hrps.t_ir_codes WHERE irc_TYPE ='360LR' AND trim(IRC_DESC)=:pno AND irc_valid_tag='Y'"
            Dim cmd As New OracleCommand()
            cmd.CommandText = qry
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("pno", pno)
            Dim g = getRecord(cmd, conHrps)
            If g.Rows.Count > 0 Then

            Else
                Response.Write("<center> <b><I> This screen only for super admin </b></I></center>")
                Me.Visible = False
            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Sub GetFy()
        Try
            Dim s As New OracleCommand()
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY FROM DUAL"
            Dim f = getRecord(s, conHrps)

            If f.Rows.Count > 0 Then
                ViewState("FY") = f.Rows(0)(0)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub bindRespondent(pno As String)
        Try
            Dim qry As New OracleCommand()
            qry.CommandText = "select (case when SS_PNO like 'SR%' then SS_EMAIL else SS_PNO end) ""Respondent"""
            qry.CommandText += " from t_survey_status where ss_wfl_status='2' and ss_year=:yr and ss_asses_pno=:asspno "
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("yr", ViewState("FY").ToString())
            qry.Parameters.AddWithValue("asspno", pno)
            Dim fc = getRecord(qry, conHrps)
            If fc.Rows.Count > 0 Then
                chkres.DataTextField = "Respondent"
                chkres.DataValueField = "Respondent"
                chkres.DataSource = fc
                chkres.DataBind()

                btnactivate.Visible = True
                btndact.Visible = True
            Else

                chkres.DataSource = Nothing
                chkres.DataBind()
                btndact.Visible = False
                btnactivate.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Function getRecord(ByVal cmd As OracleCommand, ByVal cn As OracleConnection) As DataTable
        Dim dt As New DataTable()
        Try
            cmd.Connection = cn
            If cn.State = ConnectionState.Closed Then
                cn.Open()
            End If
            Dim da As New OracleDataAdapter(cmd)
            da.Fill(dt)

            da.Dispose()
        Catch ex As Exception
            ex.Message.ToString()
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function


    Protected Sub btnactivate_Click(sender As Object, e As EventArgs)
        Try
            If Validation() Then
                'ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill all field")
                'Exit Sub
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "asdf", "alert('Please fill all field');", True)
                Exit Sub
            End If
            If DateTime.ParseExact(Txtstdt.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtendDt.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture) Then
                'ShowGenericMessageModal(CommonConstants.AlertType.info, "End Date should not less than start date")
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "asdf", "alert('End Date should not less than start date');", True)
                Exit Sub
            End If
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()

            End If
            ' Dim assesepno As String()
            Dim respondentisexist As String = String.Empty
            ' assesepno = Split(txtrespnoemail.Text.Trim(), ",")

            For p As Integer = 0 To chkres.Items.Count - 1
                If chkres.Items(p).Selected = True Then
                    Dim cond As String = String.Empty
                    If Len(chkres.Items(p).Text) > 6 Then
                        cond = "SS_EMAIL"
                    Else
                        cond = "SS_PNO"
                    End If
                    Dim cmd As New OracleCommand()
                    Dim strquery As String = String.Empty
                    ' strquery = "select * from hrps.t_survey_status where ss_year=:year and ss_asses_pno=:sspno and " & cond & "=:pno and SS_SRLNO='1'"
                    strquery = " select SPA_RESPONDENT from t_survey_pg_activation where upper(SPA_RESPONDENT)=:pno and SPA_TYPE='360EX' and SPA_ASSPNO=:sspno"
                    cmd.CommandText = strquery
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("sspno", txtassesspno.Text.Trim())
                    cmd.Parameters.AddWithValue("pno", chkres.Items(p).Text)

                    Dim fg = getRecord(cmd, conHrps)
                    If fg.Rows.Count > 0 Then
                        respondentisexist += chkres.Items(p).Text.ToString() & ","

                    Else
                        SaveData(txtassesspno.Text.Trim, chkres.Items(p).Text.ToString(), Txtstdt.Text.Trim(), txtendDt.Text.Trim(), ddlactivate.SelectedValue)

                    End If
                End If
            Next
            If respondentisexist <> "" Then
                Activate(txtrespnoemail.Text, respondentisexist, ddlactivate.SelectedValue)
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "asdf", "alert('Respondent :- " & respondentisexist & "Added');", True)
                Exit Sub
            Else
                ScriptManager.RegisterStartupScript(Me, GetType(Page), "asdf", "alert('Selected respondent has been added');", True)
            End If


            Reset()
        Catch ex As Exception
            Throw ex
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Public Sub SaveData(asspno As String, respno As String, stdt As String, enddt As String, tag As String)
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()

            End If
            Dim vUserFullName As String = Page.User.Identity.Name
            Dim strUserID As String = ""
            Dim arrUserIDParts As String() = vUserFullName.Split("\")

            Dim qry As String = String.Empty
            qry = "insert into t_survey_pg_activation values('360EX',:ss_apno,to_date(:stdt,'dd/MM/yyyy'),to_date(:endt,'dd/MM/yyyy'),:ss_pno,:tag,:users,sysdate,null,null)"
            Dim comnd As New OracleCommand()
            comnd.Connection = conHrps
            comnd.Parameters.Clear()
            comnd.CommandText = qry
            comnd.Parameters.AddWithValue("ss_apno", asspno)
            comnd.Parameters.AddWithValue("stdt", DateTime.ParseExact(stdt, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            comnd.Parameters.AddWithValue("endt", DateTime.ParseExact(enddt, "dd/MM/yyyy", CultureInfo.InvariantCulture))
            comnd.Parameters.AddWithValue("ss_pno", respno)
            comnd.Parameters.AddWithValue("users", arrUserIDParts(1))
            comnd.Parameters.AddWithValue("tag", tag)
            comnd.ExecuteNonQuery()
        Catch ex As Exception

            Throw ex
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnresp_Click(sender As Object, e As EventArgs)
        bindRespondent(txtassesspno.Text.Trim.ToUpper())
    End Sub
    Public Function Validation() As Boolean
        Dim flag = False

        If txtassesspno.Text.Trim() = "" Then
            flag = True
        End If
        If txtendDt.Text.Trim() = "" Then
            flag = True
        End If
        If Txtstdt.Text.Trim() = "" Then
            flag = True
        End If
        If ddlactivate.SelectedValue = "" Then
            flag = True
        End If

        Return flag
    End Function
    Public Sub Reset()
        Try
            txtassesspno.Text = ""
            txtendDt.Text = ""
            Txtstdt.Text = ""
            ddlactivate.SelectedValue = ""
            chkres.DataSource = Nothing
            chkres.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btndact_Click(sender As Object, e As EventArgs)
        Try
            For jh As Integer = 0 To chkres.Items.Count - 1
                If chkres.Items(jh).Selected = True Then
                    Activate(txtassesspno.Text.Trim, chkres.Items(jh).Text, ddlactivate.SelectedValue)
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Activate(asspno As String, respno As String, tag As String)
        Try
            Dim perno As String = String.Empty
            Dim pn = respno.Split(",")
            For r As Integer = 0 To pn.Count - 1
                perno += "'" & pn(r).ToString() & "',"
            Next
            perno = perno.TrimEnd(",")
            Dim qry As String = String.Empty
            qry = "UPDATE t_survey_pg_activation SET SPA_VALID_TAG=:SPA_VALID_TAG WHERE SPA_RESPONDENT in (" & perno & ") AND SPA_ASSPNO=:SPA_ASSPNO "
            Dim CMND As New OracleCommand()
            CMND.CommandText = qry
            CMND.Parameters.Clear()
            CMND.Connection = conHrps
            CMND.Parameters.AddWithValue("SPA_ASSPNO", asspno)
            CMND.Parameters.AddWithValue("SPA_VALID_TAG", tag)
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            CMND.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
End Class
