Imports System.Data.OracleClient
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports ClosedXML.Excel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

''' <summary>
''' Updated to fix download behavior while preserving session/role checks and page logic.
''' </summary>
Partial Class EmployeeVerbatimCategory
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Public Sub SessionTimeOut()
        If Session("ADM_USER") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refresh the page.")
            Exit Sub
        End If
    End Sub

    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

    Private Sub getFy()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("IRC_DESC").ToString()
            End If
        Catch ex As Exception
            ' Log if needed
        End Try
    End Sub

    Public Sub getsrlno()
        Try
            Dim mycommand As New OracleCommand
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            mycommand.CommandText = "select IRC_DESC from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
            Dim dtsrl = getRecordInDt(mycommand, conHrps)
            If dtsrl.Rows.Count > 0 Then
                ViewState("SRLNO") = dtsrl.Rows(0)("IRC_DESC").ToString()
            End If
        Catch ex As Exception
            ' Log if needed
        End Try
    End Sub

    Private Sub SurveyCompletionStatusRpt_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub

    Public Sub loadLoggedInUserIDAndDomainIntoSession()
        Dim strUserID As String = ""
        Dim vUserFullName As String = Page.User.Identity.Name
        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If
        getFy()
        getsrlno()
        If Session("ADM_USER") = "" OrElse Session("ADM_USER") Is Nothing Then
            Session("ADM_USER") = strUserID.ToUpper()
            If GetPno(strUserID.ToUpper()) = False Then
                Session("errorMsg") = "You don't have admin role."
                Response.Redirect("errorpage.aspx", True)
            End If
            ChkRole()
        End If
    End Sub

    Private Sub SurveyCompletionStatusRpt_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            SessionTimeOut()
            If Not IsPostBack Then
                ' Keep existing logic and defaults
                btn_download.Enabled = False
                btn_download_all.Visible = False
                If ChkRole() Then
                    btn_download_all.Visible = True
                End If
            End If

            ' Ensure download buttons perform full postback when inside an UpdatePanel
            Dim sm As ScriptManager = ScriptManager.GetCurrent(Page)
            If sm IsNot Nothing Then
                Try
                    sm.RegisterPostBackControl(btn_download)
                    sm.RegisterPostBackControl(btn_download_all)
                Catch ex As Exception
                    ' Registration may fail if controls not created yet; ignore but log if required
                End Try
            End If
        Catch ex As Exception
            ' log if needed
        End Try
    End Sub

    Protected Sub btn_Show_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Show.Click
        If String.IsNullOrWhiteSpace(txtYear.Text) OrElse String.IsNullOrWhiteSpace(txtCycle.Text) Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill Year and Cycle fields.")
            Exit Sub
        End If

        Try
            bindSelectAssesorGrid()
            btn_download.Enabled = (gdvselectAssesor.Rows.Count > 0)
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while retrieving data.")
        End Try
    End Sub

    Protected Sub btn_download_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_download.Click
        If String.IsNullOrWhiteSpace(txtYear.Text) OrElse String.IsNullOrWhiteSpace(txtCycle.Text) Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill Year and Cycle fields.")
            Exit Sub
        End If

        Try
            ' Bind current filtered data (honors optional P.No.)
            bindSelectAssesorGrid()
            If gdvselectAssesor.Rows.Count = 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data available to download.")
                Return
            End If
            ExportExcel()
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Some error occurred while downloading the report.")
        End Try
    End Sub

    Protected Sub btn_download_all_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_download_all.Click
        If String.IsNullOrWhiteSpace(txtYear.Text) OrElse String.IsNullOrWhiteSpace(txtCycle.Text) Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please fill Year and Cycle fields.")
            Exit Sub
        End If

        Try
            ' Download All -> ignore P.No. filter and get all for Year & Cycle
            Dim dtAll As DataTable = GetData(txtYear.Text.Trim(), txtCycle.Text.Trim(), String.Empty)
            If dtAll Is Nothing OrElse dtAll.Rows.Count = 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data available to download.")
                Return
            End If
            gdvselectAssesor.DataSource = dtAll
            gdvselectAssesor.DataBind()
            ExportExcel()
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Some error occurred while downloading all records.")
        End Try
    End Sub

    ''' <summary>
    ''' Centralized data retrieval using the provided SQL.
    ''' If officerPno is empty or null, the PNO filter is omitted.
    ''' </summary>
    Private Function GetData(ByVal year As String, ByVal cycle As String, ByVal officerPno As String) As DataTable
        Dim dt As New DataTable()
        Try
            Dim cmd As New OracleCommand()
            cmd.Connection = conHrps

            Dim sb As New StringBuilder()
            sb.AppendLine("SELECT")
            sb.AppendLine("  A.SS_ASSES_PNO AS PNO,")
            sb.AppendLine("  B.EMA_ENAME AS NAME,")
            sb.AppendLine("  B.EMA_DESGN_DESC AS DESIGNATION,")
            sb.AppendLine("  A.SS_Q2_A AS INITIATE,")
            sb.AppendLine("  A.SS_Q2_B AS DEVELOP,")
            sb.AppendLine("  A.SS_Q2_C AS ELIMINATE")
            sb.AppendLine(" FROM T_SURVEY_STATUS A")
            sb.AppendLine(" INNER JOIN T_EMP_MASTER_FEEDBACK360 B ON A.SS_ASSES_PNO = B.EMA_PERNO")
            sb.AppendLine(" WHERE B.EMA_YEAR = :year")
            sb.AppendLine("  AND B.EMA_CYCLE = :cycle")

            If Not String.IsNullOrWhiteSpace(officerPno) Then
                sb.AppendLine("  AND A.SS_ASSES_PNO = :pno")
            End If

            sb.AppendLine(" ORDER BY A.SS_ASSES_PNO")

            cmd.CommandText = sb.ToString()
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("year", year)
            cmd.Parameters.AddWithValue("cycle", cycle)
            If Not String.IsNullOrWhiteSpace(officerPno) Then
                cmd.Parameters.AddWithValue("pno", officerPno)
            End If

            dt = getDataInDt(cmd)
        Catch ex As Exception
            ' Optionally log exception
            Throw
        End Try
        Return dt
    End Function

    ''' <summary>
    ''' Binds the GridView using Year, Cycle, and optional P.No.
    ''' </summary>
    Public Sub bindSelectAssesorGrid()
        Try
            Dim yearVal As String = txtYear.Text.Trim()
            Dim cycleVal As String = txtCycle.Text.Trim()
            Dim pnoVal As String = txtpnosub.Text.Trim()

            Dim dt As DataTable = GetData(yearVal, cycleVal, pnoVal)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                gdvselectAssesor.DataSource = dt
                gdvselectAssesor.DataBind()
            Else
                gdvselectAssesor.DataSource = Nothing
                gdvselectAssesor.DataBind()
            End If
        Catch ex As Exception
            ' Optionally log
            Throw
        End Try
    End Sub

    ' Existing role-checking helpers retained (no BUHR/executive filtering used anymore).
    Public Function ChkBUHRRole() As Boolean
        Try
            Dim t As Boolean = False
            Dim strrole As New OracleCommand
            strrole.CommandText = "select ema_bhr_pno IGP_user_id from hrps.t_emp_master_feedback360 where ema_bhr_pno =:buhr"
            strrole.Connection = conHrps
            strrole.Parameters.Clear()
            strrole.Parameters.AddWithValue("buhr", If(Session("ADM_USER") IsNot Nothing, Session("ADM_USER").ToString(), ""))
            Dim f = getDataInDt(strrole)
            Return (f.Rows.Count > 0)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False
            Dim strrole As New OracleCommand
            strrole.CommandText = "select IGP_user_id from t_ir_adm_grp_privilege where igp_group_id IN ('360FEEDBAC','360DF_SA','360DF_LD') and IGP_STATUS ='A' and IGP_user_id=:userid"
            strrole.Connection = conHrps
            strrole.Parameters.Clear()
            strrole.Parameters.AddWithValue("userid", If(Session("ADM_USER") IsNot Nothing, Session("ADM_USER").ToString(), ""))
            Dim f = getDataInDt(strrole)
            If f.Rows.Count > 0 Then
                lblname.Text = "Super Admin"
                t = True
            Else
                lblname.Text = "HRBP Admin"
                t = False
            End If
            Return t
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetPno(pno As String) As Boolean
        Try
            Dim q As New OracleCommand()
            q.CommandText = "Select ema_perno from t_ir_adm_grp_privilege,hrps.t_emp_master_feedback360 where igp_group_id IN ('360FEEDBAC','360DF_SA','360DF_LD') and IGP_STATUS ='A' and ema_perno=IGP_user_id and IGP_user_id=:pno and EMA_COMP_CODE='1000' " &
                            "union select ema_bhr_pno ema_perno from hrps.t_emp_master_feedback360 where ema_bhr_pno =:buhrno and rownum=1 " &
                            "union select IGP_user_id ema_perno from t_ir_adm_grp_privilege where IGP_user_id=:pno and igp_group_id IN ('360FEEDBAC','360DF_SA','360DF_LD') and IGP_STATUS ='A'"
            q.Connection = conHrps
            q.Parameters.Clear()
            q.Parameters.AddWithValue("pno", pno)
            q.Parameters.AddWithValue("buhrno", If(Session("ADM_USER") IsNot Nothing, Session("ADM_USER").ToString(), ""))
            Dim p = getDataInDt(q)
            Return (p.Rows.Count > 0)
        Catch ex As Exception
            Return False
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
            ' log if needed
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function

    Public Function getDataInDt(ByVal cmd1 As OracleCommand) As DataTable
        Dim dt As New DataTable()
        Try
            Dim da As New OracleDataAdapter(cmd1)
            da.Fill(dt)
        Catch ex As Exception
            ' For debugging: MsgBox(ex.Message.ToString) -- consider logging instead
        End Try
        Return dt
    End Function

    Public Overrides Sub VerifyRenderingInServerForm(control As Control)
        ' Required for exporting GridView to Excel
    End Sub

    ' Export the current GridView to Excel using ClosedXML
    Protected Sub ExportExcel()
        Try
            If gdvselectAssesor Is Nothing OrElse gdvselectAssesor.Rows.Count = 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "No data available to export.")
                Return
            End If

            Dim dt As New DataTable("Report")
            ' Build columns from header row (safe-check)
            If gdvselectAssesor.HeaderRow Is Nothing Then
                ' If grid has no header row, create expected columns
                dt.Columns.Add("P.No")
                dt.Columns.Add("Name")
                dt.Columns.Add("Designation")
                dt.Columns.Add("Initiate")
                dt.Columns.Add("Develop")
                dt.Columns.Add("Eliminate")
            Else
                For Each cell As TableCell In gdvselectAssesor.HeaderRow.Cells
                    dt.Columns.Add(HttpUtility.HtmlDecode(cell.Text))
                Next
            End If

            For Each row As GridViewRow In gdvselectAssesor.Rows
                Dim newRow As DataRow = dt.NewRow()
                For i As Integer = 0 To row.Cells.Count - 1
                    newRow(i) = HttpUtility.HtmlDecode(row.Cells(i).Text)
                Next
                dt.Rows.Add(newRow)
            Next

            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt, "Report")
                Using ms As New MemoryStream()
                    wb.SaveAs(ms)
                    Dim bytes() As Byte = ms.ToArray()

                    Response.Clear()
                    Response.ClearHeaders()
                    Response.ClearContent()
                    Response.Buffer = True
                    Response.BufferOutput = True
                    Response.ContentEncoding = System.Text.Encoding.UTF8
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Dim fileName As String = "EmployeeVerbatimReport_" & DateTime.Now.ToString("yyyyMMddHHmmss") & ".xlsx"
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
                    Response.AddHeader("Content-Length", bytes.Length.ToString())

                    Response.BinaryWrite(bytes)
                    Response.Flush()

                    ' Use CompleteRequest to avoid ThreadAbortException from Response.End
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End Using
            End Using
        Catch ex As Exception
            ' Surface user-friendly message; consider logging ex for diagnostics
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Error generating Excel file: " & ex.Message)
        End Try
    End Sub

End Class
