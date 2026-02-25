Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Partial Class RespondentSelectedRpt
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Public Sub SessionTimeOut()
        If Session("ADM_USER") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Exit Sub
        Else

        End If
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
    Public Function getsrlno() As DataTable
        Dim strsrl As String = "select IRC_CODE from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
        Dim mycommand As OracleCommand
        If conHrps.State = ConnectionState.Closed Then
            conHrps.Open()
        End If
        mycommand = New OracleCommand(strsrl, conHrps)
        Dim dasrl As New OracleDataAdapter(mycommand)
        Dim dtsrl As New DataTable()
        dasrl.Fill(dtsrl)
        Return dtsrl
    End Function
    Private Sub RespondentSelectedRpt_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub

    Private Sub RespondentSelectedRpt_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            SessionTimeOut()
            If Not IsPostBack Then
                bindGrid()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub bindGrid()

        Try
            If ChkRole() Then

                'If Request.QueryString("adm") = "1" Then
                bindSelectAssesorGrid()
                'End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub bindSelectAssesorGrid()
        Try

            Dim dt As New DataTable()
            Dim qrypending As String = String.Empty
            Dim dtsrl As DataTable = getsrlno()
            qrypending = " select distinct ea.ema_perno Pno,ea.EMA_ENAME,ea.EMA_EQV_LEVEL,ea.EMA_DESGN_DESC Designation,ea.ema_dept_desc Department,ea.ema_email_id Email_id, "
            qrypending += "  ea.ema_exec_head_desc Executive_Head,ea.EMA_REPORTING_TO_PNO Superior_Pno,(select EMA_FSTNAME||' '||ema_lstname from tips.t_empl_all  where ema_perno=ea.EMA_REPORTING_TO_PNO) Superior_Name,ea.ema_bhr_pno BUHR_Pno,ea.ema_bhr_name BUHR_NAME,ss.ss_pno Respondent_Pno,"
            qrypending += " ss.ss_name Respondent_Name,ss.ss_level Respondent_Level,ss.ss_desg Respondent_Designation,ss.ss_dept Respondent_Department,ss.ss_email Respondent_Email_Id,decode(upper(ss.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager',upper('PEER'),'Peers',upper('intsh'),'Internal stakeholders',upper('ropt'),'People you lead') Respondent_Category"
            qrypending += " from tips.t_empl_all ea,t_survey_status ss where ea.ema_perno= ss.ss_pno and ss.ss_level in ('IL3','IL4','IL5','IL6')  and ss.ss_year='" & ViewState("FY").ToString() & "' and  SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'  order by 1"
            Dim qry = New OracleCommand(qrypending, conHrps)
            dt = getRecordInDt(qry, conHrps)
            If dt.Rows.Count > 0 Then
                gdvselectAssesor.DataSource = dt
                gdvselectAssesor.DataBind()
            Else
                gdvselectAssesor.DataSource = Nothing
                gdvselectAssesor.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""

        If Session("ADM_USER") = "" Then

            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            strUserID = "197838"
            Session("ADM_USER") = strUserID.ToUpper()
            If GetPno(strUserID.ToUpper()) = False Then
                Response.Redirect("errorpage.aspx", True)
            End If


            lblname.Text = "Admin"
            'lblname.Text = "Suresh Dutt Tripathi"
        ElseIf (Session("ADM_USER") IsNot Nothing) AndAlso (Session("ADM_USER").Equals("") = False) Then
            'strUserID = "198777"
            'Session("ADM_USER") = strUserID.ToUpper()
            ' lblname.Text = GetPno().Rows(0)(1) '"Suresh Dutt Tripathi"
            Return
        Else
        End If

    End Sub

    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False

            Dim strrole As String = String.Empty
            strrole = "select IGP_user_id from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            strrole += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"

            strrole += "UNION select ema_bhr_pno IGP_user_id from tips.t_empl_all  where ema_bhr_pno ='" & Session("ADM_USER").ToString() & "' and rownum=1"
            Dim cmd = New OracleCommand(strrole, conHrps)
            Dim f = getRecordInDt(cmd, conHrps)

            If f.Rows.Count > 0 Then
                t = True
            Else
                t = False
            End If

            Dim str As String = String.Empty
            str = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY from dual"
            Dim r = New OracleCommand(str, conHrps)
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)(0).ToString()
            End If
            Return t
        Catch ex As Exception

        End Try
    End Function
    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            Dim q As New OracleCommand()
            q.CommandText = "Select ema_perno from t_ir_adm_grp_privilege,TIPS.t_empl_all where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q.CommandText += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno.ToString() & "' and EMA_COMP_CODE='1000'"
            q.CommandText += " union select ema_bhr_pno ema_perno from tips.t_empl_all  where ema_bhr_pno ='" & Session("ADM_USER").ToString() & "' and rownum=1"

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
End Class
