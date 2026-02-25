Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing

Partial Class SurveyRpt
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Public Sub bindGrid()
        Try
            Dim cmd As New OracleCommand()
            'cmd.CommandText = "select ss_pno Feedback_Giver_PNo,ss_name Feedback_Giver_Name,ss_email Feedback_Giver_Email ,ss_level Feedback_Giver_Level"
            'cmd.CommandText += " ,ss_desg Feedback_Giver_Designation,nvl((select b.EMA_EXEC_HEAD_DESC from tips.t_empl_all B where b.ema_perno=ss_pno and b.ema_comp_code='1000'),' ') "
            'cmd.CommandText += " feedback_giver_exec_head , decode(ss_wfl_status,'','Pending With Assesor','1','Pending With Manager','2','Pending With Respondent') "
            'cmd.CommandText += " Status ,irc_desc category,ss_asses_pno Assesor_pno,a.ema_ename Assesor_Name,a.ema_email_id Assesor_Email, a.ema_desgn_desc Assesor_Designation,A.EMA_EXEC_HEAD_DESC Assesor_Executive_Head"
            'cmd.CommandText += "  from tips.t_empl_all  A, t_survey_status,t_ir_codes where ss_asses_pno=a.ema_perno and irc_type='360RL' and upper(irc_code) = upper(ss_categ)"
            cmd.CommandText = "SELECT ss_pno feedback_giver_pno, ss_name feedback_giver_name, ss_email feedback_giver_email,  ss_level feedback_giver_level, ss_desg"
            cmd.CommandText += " feedback_giver_designation,nvl( ( SELECT b.ema_exec_head_desc FROM tips.t_empl_all b  WHERE b.ema_perno = ss_pno AND b.ema_comp_code = '1000')"
            cmd.CommandText += " ,' ') feedback_giver_exec_head, DECODE(ss_wfl_status,'','Pending With Assesor','1','Pending With Manager','2','Pending With Respondent'"
            cmd.CommandText += ",'3','Completed','9','Insufficient Exposure' ) status, irc_desc category,ss_asses_pno assesor_pno, a.ema_ename assesor_name,"
            cmd.CommandText += " a.ema_email_id assesor_email, a.ema_desgn_desc assesor_designation,a.ema_exec_head_desc   assesor_executive_head, ss_level,"
            cmd.CommandText += "DECODE(ss_del_tag,'Y','DELETED','N','SELECTED',ss_del_tag) adm_del_tag, DECODE(ss_app_tag,'AP','Approved','RJ','Returned to officer',ss_app_tag)"
            cmd.CommandText += " il1_approve_status FROM tips.t_empl_all a, t_survey_status, t_ir_codes WHERE ss_asses_pno = a.ema_perno AND irc_type = '360RL' "
            cmd.CommandText += " AND upper(irc_code) = upper(ss_categ) AND ss_asses_pno IN (SELECT ema_perno FROM tips.t_empl_all WHERE ema_disch_dt IS NULL) AND ss_year "
            cmd.CommandText += " = '2020' AND ss_status = 'SE' AND ss_del_tag = 'N' AND a.ema_disch_dt IS NULL"
            If txtpno.Text.Trim() <> "" Then
                cmd.CommandText += " and ss_asses_pno =:pno"
            End If
            If ddlyear.SelectedValue <> "" Then
                ' cmd.CommandText += " and ss_year= '2020' "
            End If
            cmd.CommandText += " and a.ema_comp_code='1000'  order by ss_asses_pno, SS_categ"
            If txtpno.Text.Trim() <> "" Then
                cmd.Parameters.Add(New OracleParameter("pno", Right(txtpno.Text, 6)))
            End If
            cmd.Connection = conHrps
            Dim da As New OracleDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                gvself.DataSource = dt
                gvself.DataBind()
            Else
                gvself.DataSource = Nothing
                gvself.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SurveyRpt
        Try

            Dim cmd As New OracleCommand

            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select ema_ename ||'('|| ema_perno ||')' EName from tips.t_empl_all where (ema_perno like  :ema_perno or upper(ema_ename) like "
            cmd.CommandText += " :ema_ename)  and ema_disch_dt is null"


            ob.conHrps.Open()

            cmd.Connection = ob.conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", "%" & prefixText.ToUpper & "%")
            cmd.Parameters.AddWithValue("ema_ename", "%" & prefixText.ToUpper & "%")
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader
            While sdr.Read
                prefixes.Add(sdr("EName").ToString)
            End While



            Return prefixes
        Catch ex As Exception

            Return Nothing

        Finally

            ob.conHrps.Close()

        End Try

    End Function


    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False
            Dim cmd As New OracleCommand()
            cmd.CommandText = "select * from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            cmd.CommandText += " and IGP_STATUS ='A' and IGP_user_id='" & Session("RPT_USER").ToString() & "'"
            Dim f = getRecordInDt(cmd, conHrps)
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

        If Session("RPT_USER") Is Nothing Then
            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\"c)

            If arrUserIDParts.Length >= 2 Then

                strUserID = arrUserIDParts(arrUserIDParts.Length - 1)

                strUserDomain = arrUserIDParts(arrUserIDParts.Length - 2)
            ElseIf arrUserIDParts.Length = 1 Then

                strUserID = arrUserIDParts(arrUserIDParts.Length - 1)
                strUserDomain = "TATASTEEL"
            End If

            Session("RPT_USER") = strUserID.ToUpper()
            Session("USER_DOMAIN") = strUserDomain.ToUpper()
            Session("RPT_USER") = "" '"151629" '120324  158240
            lblname.Text = ""
            'lblname.Text = "Suresh Dutt Tripathi"
        ElseIf (Session("RPT_USER") IsNot Nothing) AndAlso (Session("RPT_USER").Equals("") = False) Then
            lblname.Text = "" '"Suresh Dutt Tripathi"
            Return
        Else
        End If
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
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

    Private Sub SurveyRpt_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            response.redirect("errorpage.aspx")
            If Not IsPostBack Then
                PopDroupdown()
            End If

            If Request.QueryString("adm") = "1" Then
                bindGridCount(DropDownList1.SelectedValue)
                gvself.Visible = False
                txtpno.Visible = False
                ddlyear.Visible = False
                btnfind.Visible = False
                btndownload.Visible = False
                btndownloadrej.Visible = True
                DropDownList1.Visible = True
            Else
                gvself.Visible = True
                GVrejectcount.Visible = False
                bindGrid()
                txtpno.Visible = True
                ddlyear.Visible = True
                btnfind.Visible = True
                btndownload.Visible = True
                btndownloadrej.Visible = False
                DropDownList1.Visible = False
            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Sub PopDroupdown()
        Try
            Dim qry As String = String.Empty
            qry = "select distinct ss_year from t_survey_status order by 1 desc"

            Dim adp As New OracleClient.OracleDataAdapter(qry, conHrps)
            Dim dt As New DataTable()
            adp.Fill(dt)
            If dt.Rows.Count > 0 Then
                ddlyear.DataTextField = "ss_year"
                ddlyear.DataValueField = "ss_year"
                ddlyear.DataSource = dt
                ddlyear.DataBind()
                ddlyear.SelectedValue = dt.Rows(0)(0).ToString()

                DropDownList1.DataTextField = "ss_year"
                DropDownList1.DataValueField = "ss_year"
                DropDownList1.DataSource = dt
                DropDownList1.DataBind()
                DropDownList1.SelectedValue = dt.Rows(0)(0).ToString()

            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Sub ExportToExcel(gv As GridView)
        Response.Clear()
        Response.Buffer = True
        Response.AddHeader("content-disposition", "attachment;filename=SurveyRpt.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.ms-excel"
        Using sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)

            gv.AllowPaging = False
            Me.bindGrid()

            gv.HeaderRow.BackColor = Color.White
            For Each cell As TableCell In gv.HeaderRow.Cells
                cell.BackColor = gv.HeaderStyle.BackColor
            Next
            For Each row As GridViewRow In gv.Rows
                row.BackColor = Color.White
                For Each cell As TableCell In row.Cells
                    If row.RowIndex Mod 2 = 0 Then
                        cell.BackColor = gv.AlternatingRowStyle.BackColor
                    Else
                        cell.BackColor = gv.RowStyle.BackColor
                    End If
                    cell.CssClass = "textmode"
                Next
            Next

            gv.RenderControl(hw)
            'style to format numbers to string
            Dim style As String = "<style> .textmode { } </style>"
            Response.Write(style)
            Response.Output.Write(sw.ToString())
            Response.Flush()
            Response.[End]()
        End Using
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Verifies that the control is rendered
    End Sub
    Protected Sub btndownload_Click(sender As Object, e As EventArgs)
        Try
            ExportToExcel(gvself)
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try
    End Sub
    Protected Sub btnfind_Click(sender As Object, e As EventArgs)
        Try
            bindGrid()

        Catch ex As Exception

        End Try
    End Sub

    Public Sub bindGridCount(year As String)
        Try
            Dim cmd As New OracleCommand()
            cmd.CommandText = "select A.ss_year,A.ss_asses_pno,decode(A.ss_categ,'INTSH','Internal Stakeholder','MANGR','Manager/Superior','PEER','Peer','ROPT','Subordinates','Self','Self') SS_CATEG"
            cmd.CommandText += " ,decode(a.ss_categ,'INTSH',3,'MANGR',1,'PEER',3,'ROPT',3,'Self',1) minim,a.approved ,nvl(c.completed,0) Completed ,NVL(b.rejected,0) Insufficient_Exposure"
            cmd.CommandText += " ,decode(sign(a.approved-b.rejected-decode(a.ss_categ,'INTSH',3,'MANGR',1,'PEER',3,'ROPT',3)),'-1','LESS','OK') criteria from  (select ss_year,ss_asses_pno,ss_categ"
            cmd.CommandText += " ,count(*) approved from t_survey_status where ss_year='" & year & "' and ss_status='SE' and ss_del_tag='N' and ss_app_tag='AP' group by ss_year,ss_asses_pno,ss_categ) a, (select ss_year,ss_asses_pno,ss_categ"
            cmd.CommandText += " ,count(*) rejected from t_survey_status where ss_year='" & year & "' and ss_status='SE' and ss_del_tag='N' and ss_app_tag='AP' and nvl(ss_wfl_status,'0') ='9'  group by ss_year,ss_asses_pno,ss_categ) b,"
            cmd.CommandText += " (select ss_year,ss_asses_pno,ss_categ ,count(*) completed from hrps.t_survey_status where ss_year='" & year & "' and ss_status='SE' and ss_del_tag='N' and ss_app_tag='AP' and nvl(ss_wfl_status,'0')='3'"
            cmd.CommandText += " group by ss_year,ss_asses_pno,ss_categ) c where a.ss_year=b.ss_year(+) and a.ss_asses_pno=b.ss_asses_pno(+) and a.ss_categ=b.ss_categ (+) and a.ss_year=c.ss_year(+)"
            cmd.CommandText += " and a.ss_asses_pno=c.ss_asses_pno(+) and a.ss_categ=c.ss_categ (+) order by 1,2,3,7"
            cmd.Connection = conHrps
            Dim da As New OracleDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                GVrejectcount.DataSource = Nothing
                GVrejectcount.DataBind()
                GVrejectcount.Visible = True
                GVrejectcount.DataSource = dt
                GVrejectcount.DataBind()
            Else
                GVrejectcount.DataSource = Nothing
                GVrejectcount.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btndownloadrej_Click(sender As Object, e As EventArgs)
        Try
            ExportToExcel(GVrejectcount)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            bindGridCount(DropDownList1.SelectedValue)
        Catch ex As Exception

        End Try
    End Sub
End Class
