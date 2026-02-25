
'Version    Date            Name                        Change                      Work Done
'--------------------------------------------------------------------------------------------------------------------------------------------------------
'1.0       4 Sep 2020      Mukul Mishra               Initial Creation
'--------------------------------------------------------------------------------------------------------------------------------------------------------
Imports System.Security.Principal
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.Security.Cryptography
Imports System.Data
Imports System.DirectoryServices
Imports System.Linq
Imports System.Web.Script.Services
Imports System.Web.Services
Imports System.IO
Imports ADAuthenticator
Imports System.Data.OracleClient
''' <remarks></remarks>
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Dim conHRPS As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim mycommand As New OracleCommand


    ''' <summary>
    ''' Init event of the page to gather all the Site related master details.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim strRefreshSesssion As String = Request.QueryString("RefreshSession")
        If Not IsPostBack Then

            loadSessionVariables()

            lblusername.Text = "Welcome " & Convert.ToString(Session(CommonConstants.SESSION_USER_NAME))

            Dim vRole As String = ""
            vRole = GetRole(Session(CommonConstants.SESSION_USER_ID))
            Session("ROLE") = vRole
        End If
    End Sub

    Public Sub loadSessionVariables()

        loadLoggedInUserIDAndDomainIntoSession()

        getUserDetails(Session(CommonConstants.SESSION_USER_ID).ToString())
        Session(CommonConstants.SESSION_USER_NAME) = Session("UserNamePNo")




    End Sub

    ''' <summary>
    ''' This method contains the code to load the currently logged in user into Session Variable.
    ''' </summary>
    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""

        If Session("USER_ID") Is Nothing Then
            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\"c)

            If arrUserIDParts.Length >= 2 Then

                strUserID = arrUserIDParts(arrUserIDParts.Length - 1)

                strUserDomain = arrUserIDParts(arrUserIDParts.Length - 2)
            ElseIf arrUserIDParts.Length = 1 Then

                strUserID = arrUserIDParts(arrUserIDParts.Length - 1)
                strUserDomain = "TATASTEEL"
            End If


            Session("USER_ID") = strUserID.ToUpper()
            Session("USER_DOMAIN") = strUserDomain.ToUpper()
            'Session("USER_ID") = "120324" '120324  158240

        ElseIf (Session("USER_ID") IsNot Nothing) AndAlso (Session("USER_ID").Equals("") = False) Then
            Return
        Else

            RedirectToLoginPage()
        End If



    End Sub

    Public Sub RedirectToLoginPage()

        Session.Abandon()
        Session.Clear()
        Session(CommonConstants.SESSION_USER_ID) = Nothing


        Dim c As New HttpCookie("intranetUserPno")
        c.Expires = DateTime.Now.AddDays(-1)
    End Sub

    Protected Sub getUserDetails(userId As String)
        Session("UserNamePNo") = ""

        Dim qry As String = String.Empty
        qry = "select ema_ename from t_empl_comn where ema_perno ='" & userId & "' and ema_disch_dt is null"

        Dim adp As New OracleDataAdapter(qry, conHRPS)
        Dim dt As New DataTable()
        adp.Fill(dt)
        If dt.Rows.Count > 0 Then
            Session("UserNamePNo") = dt.Rows(0)("ema_ename").ToString()
        End If
    End Sub
    Public Function GetRole(ByVal vUserID As String) As String

        Return ""
    End Function

    Public Function getRecordInDt(ByVal cmd As OracleCommand, ByVal cn As OracleConnection) As DataTable
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

        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt

    End Function

    Private Sub MasterPage_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
End Class