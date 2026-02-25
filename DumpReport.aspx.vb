Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Partial Class DumpReport
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub DumpReport_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub
    Private Sub DumpReport_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If ChkRole() Then
                txtpnosub.Text = String.Empty
                GetFy()
            End If
        End If
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "KeyGenericMessageModal", "screenshot('02-03-2021');", True)
    End Sub
    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False
            Dim cmd As String
            cmd = "select * from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            cmd += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            Dim f = GetData(cmd, conHrps)
            If f.Rows.Count > 0 Then
                t = True
            Else
                t = False
            End If
            Return t
        Catch ex As Exception

        End Try
    End Function
    Public Sub loadLoggedInUserIDAndDomainIntoSession()

        Dim strUserID As String = ""
        Dim strUserDomain As String = ""

        If Session("ADM_USER") = "" Then

            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            'strUserID = "151629"
            Session("ADM_USER") = strUserID.ToUpper()
            If GetPno(strUserID.ToUpper()) = False Then
                Response.Redirect("errorpage.aspx", True)
            End If

            lblname.Text = "Admin"
            'lblname.Text = "Suresh Dutt Tripathi"
        ElseIf (Session("ADM_USER") IsNot Nothing) AndAlso (Session("ADM_USER").Equals("") = False) Then
            ' lblname.Text = GetPno().Rows(0)(1) '"Suresh Dutt Tripathi"
            Return
        Else
        End If
    End Sub
    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            Dim q As String
            q = "Select IGP_user_id,ema_ename from t_ir_adm_grp_privilege,TIPS.t_empl_all where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno.ToString() & "' and EMA_COMP_CODE='1000'"
            Dim p = GetData(q, conHrps)
            If p.Rows.Count > 0 Then
                d = True
            Else
                d = False

            End If
            Return d
        Catch ex As Exception

        End Try
    End Function
    Public Function GetData(qry As String, con As OracleConnection) As DataTable

        Try
            Dim comd As New OracleDataAdapter(qry, con)
            Dim data As New DataTable()
            comd.Fill(data)
            If data.Rows.Count > 0 Then
                Return data
            Else
                Return Nothing
            End If
        Catch ex As Exception

        End Try

    End Function
    Public Sub GetFy()
        Try
            Dim s As String = String.Empty
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s = "select distinct SS_YEAR from hrps.t_survey_status order by SS_YEAR"
            Dim f = GetData(s, conHrps)

            If f.Rows.Count > 0 Then
                ddlYear.DataSource = f
                ddlYear.DataValueField = "SS_YEAR"
                ddlYear.DataTextField = "SS_YEAR"
                ddlYear.DataBind()
                ddlYear.Items.Insert(0, "--Select--")
                'ddlLevel.Items.Insert(0, "--Select--")
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btn_Show_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Show.Click
        If ddlYear.SelectedValue = "--Select--" And ddlLevel.SelectedValue = "--Select--" And txtpnosub.Text.Trim = "" Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select option for serach result")
            Exit Sub
        Else
            btn_download.Enabled = True
            bindGrid()
        End If

    End Sub
    Public Sub bindGrid()
        Try
            Dim str As String
            Dim dt As New DataTable()

            str = " select SS_YEAR,SS_ASSES_PNO,SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_Q1_A,SS_Q1_B,SS_Q1_C,SS_Q1_D,SS_Q2_A,SS_Q2_B"
            str += " from hrps.t_survey_status"
            str += "  where ss_wfl_status=3"
            If ddlYear.SelectedValue <> "--Select--" Then
                str += "  and ss_year='" + ddlYear.SelectedValue + "' "
            End If
            If ddlLevel.SelectedValue <> "--Select--" Then
                str += " and ss_asses_pno in (select ema_perno from tips.t_empl_all where ema_eqv_level='" + ddlLevel.SelectedValue + "')"
            End If
            If txtpnosub.Text.Trim <> "" Then
                str += " and SS_ASSES_PNO='" + txtpnosub.Text.Trim.ToString + "'"
            End If



            str += " order by 1,2"
            dt = GetData(str, conHrps)
            If Not dt Is Nothing Then
                gdvMiniCriteria.DataSource = dt
                gdvMiniCriteria.DataBind()
            Else
                gdvMiniCriteria.DataSource = Nothing
                gdvMiniCriteria.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btn_download_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_download.Click
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=Dump Report.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            If gdvMiniCriteria.Visible = True Then
                'To Export all pages
                gdvMiniCriteria.AllowPaging = False
                ''Me.BindGrid()

                gdvMiniCriteria.HeaderRow.BackColor = Color.LightGray
                For Each cell As TableCell In gdvMiniCriteria.HeaderRow.Cells
                    cell.BackColor = gdvMiniCriteria.HeaderStyle.BackColor
                Next
                For Each row As GridViewRow In gdvMiniCriteria.Rows
                    row.BackColor = Color.White
                    'For Each cell As TableCell In row.Cells
                    '    If row.RowIndex Mod 2 = 0 Then
                    '        cell.BackColor = grdMIS.AlternatingRowStyle.BackColor
                    '    Else
                    '        cell.BackColor = grdMIS.RowStyle.BackColor
                    '    End If
                    '    cell.CssClass = "textmode"
                    'Next
                Next

                gdvMiniCriteria.RenderControl(hw)
            End If


            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Confirms that an HtmlForm control is rendered for the specified ASP.NET
        '     server control at run time. 

    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub
End Class
