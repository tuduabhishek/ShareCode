Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.OleDb

Partial Class BulkRecordUpdate
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim cmd As OracleCommand
    Dim drRead As OracleDataReader
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            txtStep1SD.Attributes.Add("readonly", "readonly")
            txtStep1ED.Attributes.Add("readonly", "readonly")
            txtStep2SD.Attributes.Add("readonly", "readonly")
            txtStep2ED.Attributes.Add("readonly", "readonly")
            txtStep3SD.Attributes.Add("readonly", "readonly")
            txtStep3ED.Attributes.Add("readonly", "readonly")
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "BULK RECORD UPDATE"
                getFy()
                getsrlno()
                'bindDepartment()
                bindExecHead()
                bindLevel()
                bindSubArea()
                'bindDesignation()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub getFy()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("IRC_DESC").ToString()
                txtYear.Text = ViewState("FY")
                txtYearforLessRecord.Text = ViewState("FY")
            End If
        Catch ex As Exception

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
                txtCycle.Text = ViewState("SRLNO")
                txtCycleforLessRecord.Text = ViewState("SRLNO")
            End If
        Catch ex As Exception


        End Try
    End Sub
    Private Sub bindExecHead()
        Try
            Dim dtExecHead As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct ema_exec_head,ema_exec_head_desc from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head_desc<>'Not found' and ema_exec_head<>'00000000' and ema_comp_code='1000' "
            If ChkRole1() = False Then
                query.Parameters.Clear()
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())

            End If
            query.CommandText += " order by ema_exec_head_desc"
            'Dim qry = New OracleCommand(query, conHrps)
            dtExecHead = getDataInDt(query)
            If dtExecHead.Rows.Count > 0 Then
                ddlExecHead.DataSource = dtExecHead
                ddlExecHead.DataValueField = "ema_exec_head"
                ddlExecHead.DataTextField = "ema_exec_head_desc"
                ddlExecHead.DataBind()
                ddlExecHead.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bindLevel()
        Try
            Dim dtLevel As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct EMA_EMPL_SGRADE from t_emp_master_feedback360 where EMA_EMPL_SGRADE is not null and ema_comp_code='1000' order by 1 "

            'Dim qry = New OracleCommand(query, conHrps)
            dtLevel = getDataInDt(query)
            If dtLevel.Rows.Count > 0 Then
                ddlSGrade.DataSource = dtLevel
                ddlSGrade.DataValueField = "EMA_EMPL_SGRADE"
                ddlSGrade.DataTextField = "EMA_EMPL_SGRADE"
                ddlSGrade.DataBind()
                ddlSGrade.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub bindSubArea()
        Try
            Dim dtSubArea As New DataTable
            Dim query As New OracleCommand
            query.Connection = conHrps
            query.CommandText = "select distinct EMA_PERS_SUBAREA,EMA_PERS_SUBAREA_DESC from hrps.t_emp_master_feedback360 where EMA_PERS_SUBAREA_DESC is not null and EMA_PERS_SUBAREA_DESC<>'Not found' and EMA_PERS_SUBAREA<>'00000000' and ema_comp_code='1000' "
            If ChkRole1() = False Then
                query.Parameters.Clear()
                query.CommandText += " and ema_bhr_pno=:ema_bhr_pno"
                query.Parameters.AddWithValue("ema_bhr_pno", Session("ADM_USER").ToString())

            End If
            query.CommandText += " order by EMA_PERS_SUBAREA_DESC"
            'Dim qry = New OracleCommand(query, conHrps)
            dtSubArea = getDataInDt(query)
            If dtSubArea.Rows.Count > 0 Then
                ddlSubarea.DataSource = dtSubArea
                ddlSubarea.DataValueField = "EMA_PERS_SUBAREA"
                ddlSubarea.DataTextField = "EMA_PERS_SUBAREA_DESC"
                ddlSubarea.DataBind()
                ddlSubarea.Items.Insert(0, New ListItem("--Select--", "--Select--"))
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function ChkRole1() As Boolean
        Try
            Dim t As Boolean = False

            Dim strrole As String = String.Empty
            strrole = "select IGP_user_id from hrps.t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            strrole += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            Dim cmd = New OracleCommand(strrole, conHrps)
            Dim f = getRecordInDt(cmd, conHrps)

            If f.Rows.Count > 0 Then
                t = True
            Else
                t = False
            End If
            'Return t
            Return True
        Catch ex As Exception

        End Try
    End Function
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
    Public Function getDataInDt(ByVal cmd1 As OracleCommand) As DataTable
        Dim dt As New DataTable()
        Try
            'If cn.State = ConnectionState.Closed Then
            '    cn.Open()
            'End If
            Dim da As New OracleDataAdapter(cmd1)
            da.Fill(dt)

        Catch ex As Exception
            'MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function
    Protected Sub lbtnSearch_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnSearch.Click
        Try
            Dim qryrespond As New OracleCommand
            qryrespond.Connection = conHrps
            qryrespond.Parameters.Clear()
            If ChkRole1() Then
                qryrespond.CommandText = "select EMA_YEAR,EMA_CYCLE,EMA_PERNO,ema_ename,ema_desgn_desc,ema_email_id,ema_eqv_level,ema_reporting_to_pno,ema_bhr_pno,ema_bhr_name,ema_dotted_pno,ema_pers_exec_pno,to_char(ema_step1_stdt,'dd-Mon-yyyy') ema_step1_stdt,to_char(ema_step1_enddt,'dd-Mon-yyyy') ema_step1_enddt,to_char(ema_step2_stdt,'dd-Mon-yyyy') ema_step2_stdt,to_char(ema_step2_enddt,'dd-Mon-yyyy') ema_step2_enddt,to_char(ema_step3_stdt,'dd-Mon-yyyy') ema_step3_stdt,to_char(ema_step3_enddt,'dd-Mon-yyyy') ema_step3_enddt from t_emp_master_feedback360 where EMA_PERNO NOT IN (SELECT EE_PNO FROM t_emp_excluded WHERE EE_YEAR=:EMA_YEAR AND EE_CL=:EMA_CYCLE) AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE "
                If ddlSGrade.SelectedIndex <> 0 Then
                    qryrespond.CommandText += " AND EMA_EMPL_SGRADE=:EMA_EMPL_SGRADE"
                    qryrespond.Parameters.AddWithValue("EMA_EMPL_SGRADE", ddlSGrade.SelectedValue)
                End If
                If ddlExecHead.SelectedIndex <> 0 Then
                    qryrespond.CommandText += " AND EMA_EXEC_HEAD=:EMA_EXEC_HEAD"
                    qryrespond.Parameters.AddWithValue("EMA_EXEC_HEAD", ddlExecHead.SelectedValue)
                End If
                If ddlSubarea.SelectedIndex <> 0 Then
                    qryrespond.CommandText += " AND EMA_PERS_SUBAREA=:EMA_PERS_SUBAREA"
                    qryrespond.Parameters.AddWithValue("EMA_PERS_SUBAREA", ddlSubarea.SelectedValue)
                End If
                If txtPerno.Text.Trim.Length > 0 Then
                    qryrespond.CommandText += " AND EMA_PERNO=:EMA_PERNO"
                    qryrespond.Parameters.AddWithValue("EMA_PERNO", txtPerno.Text.Trim.ToString())
                End If

                qryrespond.CommandText += " order by EMA_PERNO"

                qryrespond.Parameters.AddWithValue("EMA_YEAR", txtYear.Text.Trim.ToString())
                qryrespond.Parameters.AddWithValue("EMA_CYCLE", txtCycle.Text.Trim.ToString())
                Dim dt = getDataInDt(qryrespond)
                If dt.Rows.Count > 0 Then
                    GridView4.DataSource = dt
                    GridView4.DataBind()
                    lbtnExport.Visible = True
                    pnlUpload.Visible = True
                    'ExportToExcel(GridView4, "OverAll")
                Else
                    GridView4.DataSource = dt
                    GridView4.DataBind()
                    lbtnExport.Visible = False
                    pnlUpload.Visible = False
                End If
                pnlGrid.Visible = True
                GridView4.Columns(0).Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lbtnExport_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnExport.Click
        Try
            pnlUpload.Visible = True
            ExportToExcel(GridView4, "Employee_Details_" + DateTime.Now.ToString("yyyyMMddHHmmss"))
        Catch ex As Exception

        End Try

    End Sub
    Public Sub ExportToExcel(gv As GridView, name As String)
        Try

            Dim dt As DataTable = New DataTable("GridView_Data")
            For Each cell As TableCell In gv.HeaderRow.Cells
                If cell.Text <> "&nbsp;" Then
                    dt.Columns.Add(cell.Text)
                End If
            Next

            For Each row As GridViewRow In gv.Rows
                row.Controls.RemoveAt(0)
                dt.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    If row.Cells(i).Controls.Count > 0 Then
                        dt.Rows(dt.Rows.Count - 1)(i) = (TryCast(row.Cells(i).Controls(1), Label)).Text
                    Else
                        If row.Cells(i).Text = "&nbsp;" Then
                            dt.Rows(dt.Rows.Count - 1)(i) = ""
                        Else
                            dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text
                        End If
                    End If
                Next
            Next

            dt.Columns.Remove("Year")
            dt.Columns.Remove("Cycle")
            dt.Columns.Remove("Designation")
            'dt.Columns.Remove("Equivalent Level")
            'dt.Columns.Remove("Reporting Personal No.")
            'dt.Columns.Remove("BHR Name")
            'dt.Columns.Remove("Dotted Personal No.")
            'dt.Columns.Remove("Executive Personal No.")

            Using wb As XLWorkbook = New XLWorkbook(Server.MapPath("~/Files/Template.xlsx"))
                Dim NumberOfLastRow As Integer = wb.Worksheets.Worksheet(1).LastRowUsed().RowNumber()
                Dim CellForNewData As IXLCell = wb.Worksheets.Worksheet(1).Cell(NumberOfLastRow + 1, 1)
                CellForNewData.InsertData(dt.Rows)
                'wb.Worksheets.Add(dt)
                'wb.Worksheets.Worksheet(1).Protect()
                'wb.Worksheets.Worksheet(1).Column(3).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(4).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(5).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(6).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(7).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(8).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(9).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(10).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(11).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(12).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(13).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(14).Style.Protection.SetLocked(False)
                'wb.Worksheets.Worksheet(1).Column(1).CellsUsed().SetDataType(XLDataType.Text)
                ''wb.Worksheets.Worksheet(1).Column(4).CellsUsed().SetDataType(XLDataType.Text)
                'wb.Worksheets.Worksheet(1).Cell("D").DataType = XLDataType.Text
                ''wb.Worksheets.Worksheet(1).Column(5).Style.DateFormat.Format = "dd-MMM-yyyy"
                'wb.Worksheets.Worksheet(1).Column(6).Style.DateFormat.Format = "dd-MMM-yyyy"
                'wb.Worksheets.Worksheet(1).Column(7).Style.DateFormat.Format = "dd-MMM-yyyy"
                'wb.Worksheets.Worksheet(1).Column(8).Style.DateFormat.Format = "dd-MMM-yyyy"
                'wb.Worksheets.Worksheet(1).Column(9).Style.DateFormat.Format = "dd-MMM-yyyy"
                'wb.Worksheets.Worksheet(1).Column(10).Style.DateFormat.Format = "dd-MMM-yyyy"
                'wb.Worksheets.Worksheet(1).Column(5).SetDataType(XLDataType.Text)
                ''wb.Worksheets.Worksheet(1).Column(5).CellsUsed().SetDataType(XLDataType.Text)
                ''wb.Worksheets.Worksheet(1).Column(6).CellsUsed().SetDataType(XLDataType.Text)
                ''wb.Worksheets.Worksheet(1).Column(7).CellsUsed().SetDataType(XLDataType.Text)
                ''wb.Worksheets.Worksheet(1).Column(8).CellsUsed().SetDataType(XLDataType.Text)
                ''wb.Worksheets.Worksheet(1).Column(9).CellsUsed().SetDataType(XLDataType.Text)
                ''wb.Worksheets.Worksheet(1).Column(10).CellsUsed().SetDataType(XLDataType.Text)
                'wb.Worksheets.Worksheet(1).Columns().AdjustToContents()
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=" & name & ".xlsx")
                Using MyMemoryStream As MemoryStream = New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.End()
                End Using
            End Using
        Catch ex As Exception
        End Try

    End Sub
    'Public Overrides Sub VerifyRenderingInServerForm(control As Control)
    '    ' Verifies that the control is rendered
    'End Sub
    Protected Sub btnUpload_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Dim strPersonalNo, strCycle, strYear As String
        Dim strBhrNo, strEmail, strStep1StartDate, strStep1EndDate As String
        Dim strStep2StartDate, strStep2EndDate, strStep3StartDate, strStep3EndDate As String
        Dim strReportingPerno, strBHRName, strDottedPerno, strExecitivePerno, strEqvLevel As String
        Dim sqlQuerytoCheckRow, sqlUpdateQuery As String
        Dim rowArraylist As New ArrayList()
        Dim insertedRow As Integer = 0
        Dim dtview As New DataTable
        Dim dtUploadReport As New DataTable
        Dim excelPath As String = Server.MapPath("~/") + Path.GetFileName(FileUpload1.PostedFile.FileName)
        Try
            strCycle = txtCycle.Text.Trim
            strYear = txtYear.Text.Trim
            If FileUpload1.HasFile Then
                Dim UploadFileName As String = FileUpload1.PostedFile.FileName
                Dim Extension As String = UploadFileName.Substring(UploadFileName.LastIndexOf(".") + 1).ToLower()
                If Extension <> "xlsx" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please upload Excel (xlsx) file only!")
                    Exit Sub
                End If
                'Dim excelPath As String = Server.MapPath("~/") + Path.GetFileName(FileUpload1.PostedFile.FileName)
                FileUpload1.SaveAs(excelPath)

                Using workBook As New XLWorkbook(excelPath)

                    'Read the first Sheet from Excel file.
                    Dim workSheet As IXLWorksheet = workBook.Worksheet(1)
                    ' file_Namephoto.Replace("'", "''")
                    'Create a new DataTable.
                    Dim dt As New DataTable()

                    'Loop through the Worksheet rows.
                    Dim firstRow As Boolean = True
                    For Each row As IXLRow In workSheet.Rows()
                        'Use the first row to add columns to DataTable.
                        If firstRow Then
                            For Each cell As IXLCell In row.Cells()
                                dt.Columns.Add(cell.Value.ToString())
                            Next
                            firstRow = False
                        Else
                            'Add rows to DataTable.
                            dt.Rows.Add()
                            Dim i As Integer = 0
                            For Each cell As IXLCell In row.Cells(1, dt.Columns.Count)
                                If i = 1 Then
                                    dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString.Trim
                                Else
                                    dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString.ToUpper.Trim.Replace(" ", String.Empty)
                                End If
                                i += 1
                            Next
                        End If
                    Next

                    'Dim dtrow As DataRow = dt.Rows(dt.Rows.Count - 1)
                    'If String.IsNullOrEmpty(dtrow(0)) Then
                    '    dt.Rows.RemoveAt(dt.Rows.Count - 1)
                    '    dt.AcceptChanges()
                    'End If

                    'To remove empty row from datatable
                    dt = dt.Rows.Cast(Of DataRow)().Where(Function(row) Not row.ItemArray.All(Function(f) TypeOf f Is DBNull OrElse String.IsNullOrEmpty(If(TryCast(f, String), f.ToString())))).CopyToDataTable()
                    'End

                    If dt.Rows.Count > 0 Then
                        If Not checkExcelFileTemplate(dt) Then
                            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please upload correct excel file. Uploaded excel file column(s)/ column name is not matching with template!")
                            Exit Sub
                        End If
                        Dim distinctTable As DataTable = dt.DefaultView.ToTable(True)
                        dtUploadReport = distinctTable.Clone
                        dtUploadReport.Clear()
                        dtUploadReport.Columns.Add("Error")
                        dtview = distinctTable.Copy
                        dtview.Columns.Add("Error")
                        For i = 0 To distinctTable.Rows.Count - 1
                            strPersonalNo = Convert.ToString(distinctTable.Rows(i)("Personal Number"))
                            strEmail = Convert.ToString(distinctTable.Rows(i)("Email-id"))
                            strEqvLevel = Convert.ToString(distinctTable.Rows(i)("Equivalent Level"))
                            strBhrNo = Convert.ToString(distinctTable.Rows(i)("BHR Personal No."))
                            strStep1StartDate = Convert.ToString(distinctTable.Rows(i)("Step 1 Sart Date (dd-Mon-yyyy)"))
                            strStep1EndDate = Convert.ToString(distinctTable.Rows(i)("Step 1 End Date (dd-Mon-yyyy)"))
                            strStep2StartDate = Convert.ToString(distinctTable.Rows(i)("Step 2 Start Date (dd-Mon-yyyy)"))
                            strStep2EndDate = Convert.ToString(distinctTable.Rows(i)("Step 2 End Date (dd-Mon-yyyy)"))
                            strStep3StartDate = Convert.ToString(distinctTable.Rows(i)("Step 3 Start Date (dd-Mon-yyyy)"))
                            strStep3EndDate = Convert.ToString(distinctTable.Rows(i)("Step 3 End Date (dd-Mon-yyyy)"))
                            strReportingPerno = Convert.ToString(distinctTable.Rows(i)("Reporting Personal No.")).Trim
                            strBHRName = Convert.ToString(distinctTable.Rows(i)("BHR Name")).Trim
                            strDottedPerno = Convert.ToString(distinctTable.Rows(i)("Dotted Personal No.")).Trim
                            strExecitivePerno = Convert.ToString(distinctTable.Rows(i)("Executive Head Personal No.")).Trim
                            If Not String.IsNullOrEmpty(strPersonalNo.Trim) Then
                                sqlQuerytoCheckRow = "Select * from t_emp_master_feedback360 where EMA_YEAR = :EMA_YEAR and EMA_CYCLE=:EMA_CYCLE and EMA_PERNO =:EMA_PERNO"
                                If conHrps.State = ConnectionState.Closed Then
                                    conHrps.Open()
                                End If
                                cmd = New OracleCommand(sqlQuerytoCheckRow, conHrps)
                                cmd.Parameters.Add(New OracleParameter(":EMA_YEAR", strYear))
                                cmd.Parameters.Add(New OracleParameter(":EMA_CYCLE", strCycle))
                                cmd.Parameters.Add(New OracleParameter(":EMA_PERNO", strPersonalNo))
                                drRead = cmd.ExecuteReader
                                If drRead.HasRows Then
                                    Dim regex As Regex = New Regex("^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                                    Dim match As Match = regex.Match(strEmail.Trim)
                                    Dim strDateFileds As New List(Of String)()
                                    strDateFileds.Add(strStep1StartDate.Trim)
                                    strDateFileds.Add(strStep1EndDate.Trim)
                                    strDateFileds.Add(strStep2StartDate.Trim)
                                    strDateFileds.Add(strStep2EndDate.Trim)
                                    strDateFileds.Add(strStep3StartDate.Trim)
                                    strDateFileds.Add(strStep3EndDate.Trim)
                                    'If match.Success And checkHR(strBhrNo) And checkDates(strStep1StartDate, strStep1EndDate, strStep2StartDate, strStep2EndDate, strStep3StartDate, strStep3EndDate) Then
                                    If match.Success And strBhrNo.Length = 6 And Not String.IsNullOrEmpty(strEqvLevel.Trim) And strEqvLevel.Trim.Length < 5 And Not String.IsNullOrEmpty(strBHRName) And strReportingPerno.Length = 6 And isCorrectPernoLength(strDottedPerno) And isCorrectPernoLength(strExecitivePerno) And checkDates(strDateFileds) Then
                                        sqlUpdateQuery = "UPDATE t_emp_master_feedback360 SET EMA_EMAIL_ID=:EMA_EMAIL_ID, EMA_EQV_LEVEL = :EMA_EQV_LEVEL, EMA_REPORTING_TO_PNO = :EMA_REPORTING_TO_PNO, EMA_BHR_PNO=:EMA_BHR_PNO, EMA_BHR_NAME = :EMA_BHR_NAME,"
                                        sqlUpdateQuery += " EMA_DOTTED_PNO = :EMA_DOTTED_PNO, EMA_PERS_EXEC_PNO = :EMA_PERS_EXEC_PNO, EMA_STEP1_STDT = :EMA_STEP1_STDT, EMA_STEP1_ENDDT = :EMA_STEP1_ENDDT, EMA_STEP2_STDT = :EMA_STEP2_STDT,  EMA_STEP2_ENDDT = :EMA_STEP2_ENDDT, EMA_STEP3_STDT = :EMA_STEP3_STDT, EMA_STEP3_ENDDT = :EMA_STEP3_ENDDT "
                                        sqlUpdateQuery += " WHERE EMA_YEAR = :EMA_YEAR and EMA_CYCLE=:EMA_CYCLE and EMA_PERNO =:EMA_PERNO "
                                        cmd = New OracleCommand(sqlUpdateQuery, conHrps)
                                        cmd.Parameters.Add(New OracleParameter(":EMA_YEAR", strYear))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_CYCLE", strCycle))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_PERNO", strPersonalNo))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_EMAIL_ID", strEmail.Trim))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_EQV_LEVEL", strEqvLevel.Trim.ToUpper))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_REPORTING_TO_PNO", strReportingPerno))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_BHR_PNO", strBhrNo))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_BHR_NAME", strBHRName))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_DOTTED_PNO", IIf(String.IsNullOrEmpty(strDottedPerno), DBNull.Value, strDottedPerno)))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_PERS_EXEC_PNO", IIf(String.IsNullOrEmpty(strExecitivePerno), DBNull.Value, strExecitivePerno)))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_STEP1_STDT", IIf(String.IsNullOrEmpty(strStep1StartDate), DBNull.Value, strStep1StartDate)))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_STEP1_ENDDT", IIf(String.IsNullOrEmpty(strStep1EndDate), DBNull.Value, strStep1EndDate)))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_STEP2_STDT", IIf(String.IsNullOrEmpty(strStep2StartDate), DBNull.Value, strStep2StartDate)))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_STEP2_ENDDT", IIf(String.IsNullOrEmpty(strStep2EndDate), DBNull.Value, strStep2EndDate)))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_STEP3_STDT", IIf(String.IsNullOrEmpty(strStep3StartDate), DBNull.Value, strStep3StartDate)))
                                        cmd.Parameters.Add(New OracleParameter(":EMA_STEP3_ENDDT", IIf(String.IsNullOrEmpty(strStep3EndDate), DBNull.Value, strStep3EndDate)))
                                        rowArraylist.Add(cmd)
                                    Else
                                        Dim strError As String = ""
                                        If Not match.Success Then
                                            strError += "Email is incorrect"
                                        End If
                                        If String.IsNullOrEmpty(strEqvLevel.Trim) Then
                                            strError += ", Equivalent Level is empty"
                                        End If
                                        If strEqvLevel.Trim.Length > 5 Then
                                            strError += ", Enter correct Equivalent Level (Check length)"
                                        End If
                                        If strBhrNo.Length <> 6 Then
                                            strError += ", BHR is incorrect"
                                        End If
                                        If String.IsNullOrEmpty(strBHRName) Then
                                            strError += ", BHR Name is empty"
                                        End If
                                        If strReportingPerno.Length <> 6 Then
                                            strError += ", Reporting Perno is incorrect"
                                        End If
                                        If Not isCorrectPernoLength(strDottedPerno) Then
                                            strError += ", Dotted Perno is incorrect, check lenght"
                                        End If
                                        If Not isCorrectPernoLength(strExecitivePerno) Then
                                            strError += ", Executive Head is incorrect, check lenght"
                                        End If
                                        If Not checkDates(strDateFileds) Then
                                            strError += ", Date fields are incorrect"
                                        End If
                                        dtview.Rows(i)("Error") = strError.TrimStart(",").Trim
                                        dtUploadReport.ImportRow(dtview.Rows(i))
                                    End If
                                Else
                                    dtview.Rows(i)("Error") = "P.No. is incorrect"
                                    dtUploadReport.ImportRow(dtview.Rows(i))
                                End If
                                drRead.Close()
                                cmd.Dispose()
                            Else
                                dtview.Rows(i)("Error") = "P.No. is empty"
                                dtUploadReport.ImportRow(dtview.Rows(i))
                            End If
                        Next
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Uploaded excel file is empty, Please check!")
                    End If
                End Using

                'Dim connString As String = String.Empty
                'Dim extension As String = Path.GetExtension(FileUpload1.PostedFile.FileName)
                'Select Case extension
                '    Case ".xls"
                '        'Excel 97-03
                '        connString = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
                '        Exit Select
                '    Case ".xlsx"
                '        'Excel 07 or higher
                '        connString = ConfigurationManager.ConnectionStrings("Excel07ConString").ConnectionString
                '        Exit Select

                'End Select
                ''connString = String.Format(connString, excelPath)
                'connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & excelPath & ";Extended Properties=""Excel 8.0;IMEX=1;HDR=Yes;"""
                'Using excel_con As New OleDbConnection(connString)
                '    excel_con.Open()
                '    Dim sheet1 As String = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing).Rows(0)("GridView_Data").ToString()
                '    Dim dtExcelData As New DataTable()

                '    '[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
                '    dtExcelData.Columns.AddRange(New DataColumn(17) {New DataColumn("EMA_YEAR", GetType(Integer)),
                '                                                    New DataColumn("EMA_CYCLE", GetType(String)),
                '                                                    New DataColumn("EMA_PERNO", GetType(Decimal)),
                '                                                    New DataColumn("ema_ename", GetType(Integer)),
                '                                                    New DataColumn("ema_desgn_desc", GetType(String)),
                '                                                    New DataColumn("ema_email_id", GetType(Integer)),
                '                                                    New DataColumn("ema_eqv_level", GetType(String)),
                '                                                    New DataColumn("ema_reporting_to_pno", GetType(Integer)),
                '                                                    New DataColumn("ema_bhr_pno", GetType(String)),
                '                                                    New DataColumn("ema_bhr_name", GetType(Integer)),
                '                                                    New DataColumn("ema_dotted_pno", GetType(String)),
                '                                                    New DataColumn("ema_pers_exec_pno", GetType(String)),
                '                                                    New DataColumn("ema_step1_stdt", GetType(Integer)),
                '                                                    New DataColumn("ema_step1_enddt", GetType(String)),
                '                                                    New DataColumn("ema_step2_stdt", GetType(Integer)),
                '                                                    New DataColumn("ema_step2_enddt", GetType(String)),
                '                                                    New DataColumn("ema_step3_stdt", GetType(Integer)),
                '                                                    New DataColumn("ema_step3_enddt", GetType(String))})

                '    Using oda As New OleDbDataAdapter((Convert.ToString("SELECT * FROM [") & sheet1) + "]", excel_con)
                '        oda.Fill(dtExcelData)
                '    End Using
                '    excel_con.Close()

                'Dim conString As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
                'Using con As New SqlConnection(conString)
                '    Using sqlBulkCopy As New SqlBulkCopy(con)
                '        'Set the database table name
                '        sqlBulkCopy.DestinationTableName = "dbo.tblPersons"

                '        '[OPTIONAL]: Map the Excel columns with that of the database table
                '        sqlBulkCopy.ColumnMappings.Add("Id", "PersonId")
                '        sqlBulkCopy.ColumnMappings.Add("Name", "Name")
                '        sqlBulkCopy.ColumnMappings.Add("Salary", "Salary")
                '        con.Open()
                '        sqlBulkCopy.WriteToServer(dtExcelData)
                '        con.Close()
                '    End Using
                'End Using
                'End Using
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Select the File To Upload Data")
            End If

            If rowArraylist.Count > 0 Then
                Dim counter As Integer = 0
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim transcationforUpdate As OracleTransaction
                transcationforUpdate = conHrps.BeginTransaction()
                Try
                    For counter = 0 To rowArraylist.Count - 1
                        Dim orclCmd As New OracleCommand()
                        orclCmd = rowArraylist.Item(counter)
                        orclCmd.Transaction = transcationforUpdate
                        insertedRow += orclCmd.ExecuteNonQuery()
                    Next
                    transcationforUpdate.Commit()
                    lbtnSearch_click(sender, e)
                    lblRecordUploadbyUser.InnerText = dtview.Rows.Count
                    lblInsertRecord.InnerText = insertedRow
                    lblIncorrectRecord.InnerText = dtUploadReport.Rows.Count
                    If dtUploadReport.Rows.Count > 0 Then
                        Session("invalidReportdt") = dtUploadReport
                        divUploadErroorSection.Visible = True
                        grdExcelUploadReport.DataSource = dtUploadReport
                        grdExcelUploadReport.DataBind()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", "openReportModal()", True)
                    Else
                        divUploadErroorSection.Visible = False
                        grdExcelUploadReport.DataSource = Nothing
                        grdExcelUploadReport.DataBind()
                        ShowGenericMessageModal(CommonConstants.AlertType.success, String.Format("Total {0} record updated successfully!", insertedRow))
                    End If
                    'ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", "openReportModal()", True)
                    'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "randomText", "$(document).ready(Function() {$('#myModalUploadReport').modal();});", True)
                    'ShowGenericMessageModal(CommonConstants.AlertType.success, String.Format("Total {0} record updated successfully!", insertedRow))
                Catch ex As Exception
                    transcationforUpdate.Rollback()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Error while updating the record!")
                Finally
                    If conHrps.State = ConnectionState.Open Then
                        conHrps.Close()
                    End If
                End Try
            Else
                lblRecordUploadbyUser.InnerText = dtview.Rows.Count
                lblInsertRecord.InnerText = insertedRow
                lblIncorrectRecord.InnerText = dtUploadReport.Rows.Count
                If dtUploadReport.Rows.Count > 0 Then
                    Session("invalidReportdt") = dtUploadReport
                    divUploadErroorSection.Visible = True
                    grdExcelUploadReport.DataSource = dtUploadReport
                    grdExcelUploadReport.DataBind()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", "openReportModal()", True)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            If File.Exists(excelPath) Then
                File.Delete(excelPath)
            End If
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Function checkHR(ByVal strBHRNo As String) As Boolean
        Dim isTrue As Boolean = False
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim qry As New OracleCommand()
            qry.CommandText = "select * from hrps.t_empl_comn  where EMA_PERNO=:ema_perno"
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_perno", strBHRNo.Trim)
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                isTrue = True
            End If
        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Protected Function isCorrectPernoLength(ByVal strPerno As String) As Boolean
        Dim isTrue As Boolean = False
        Try
            If String.IsNullOrEmpty(strPerno) Then
                isTrue = True
            Else
                If strPerno.Length = 6 Then
                    isTrue = True
                Else
                    isTrue = False
                End If
            End If
        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    'Protected Function checkDates(ByVal strStep1StartDate As String, ByVal strStep1EndDate As String, ByVal strStep2StartDate As String, ByVal strStep2EndDate As String, ByVal strStep3StartDate As String, ByVal strStep3EndDate As String) As Boolean
    '    Dim isTrue As Boolean = False
    '    Dim dateFormat As String = "dd-MMM-yyyy"
    '    Dim dateTime As DateTime
    '    Try
    '        If DateTime.TryParseExact(strStep1StartDate, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, dateTime) Then
    '            isTrue = True
    '        Else
    '            isTrue = False
    '        End If
    '    Catch ex As Exception

    '    End Try
    '    Return isTrue
    'End Function
    Protected Function checkDates(ByVal strDateFields As List(Of String)) As Boolean
        Dim isTrue As Boolean = True
        Dim dateFormat As String = "dd-MMM-yyyy"
        Dim dateTime As DateTime
        Dim strDate As String
        Try
            For Each strDate In strDateFields
                If Not String.IsNullOrEmpty(strDate) Then
                    If DateTime.TryParseExact(strDate, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, dateTime) Then
                        isTrue = True
                    Else
                        isTrue = False
                        Exit For
                    End If
                End If
            Next

            If (Not String.IsNullOrEmpty(strDateFields(0)) And String.IsNullOrEmpty(strDateFields(1))) Or (Not String.IsNullOrEmpty(strDateFields(0)) And String.IsNullOrEmpty(strDateFields(1))) Or (Not String.IsNullOrEmpty(strDateFields(0)) And String.IsNullOrEmpty(strDateFields(1))) Then
                isTrue = False
                Exit Try
            End If

        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Protected Sub lbkInvalidRecordDump_Click(sender As Object, e As EventArgs)
        Try
            Dim strFileName = "Invalid_Data"
            Dim dt As DataTable = Session("invalidReportdt")
            If dt.Rows.Count > 0 Then
                dt.Columns.Remove("Error")
            End If


            Using wb As XLWorkbook = New XLWorkbook(Server.MapPath("~/Files/Template.xlsx"))
                Dim NumberOfLastRow As Integer = wb.Worksheets.Worksheet(1).LastRowUsed().RowNumber()
                Dim CellForNewData As IXLCell = wb.Worksheets.Worksheet(1).Cell(NumberOfLastRow + 1, 1)
                CellForNewData.InsertData(dt.Rows)
                wb.Worksheets.Worksheet(1).Protect()
                wb.Worksheets.Worksheet(1).Column(3).Style.Protection.SetLocked(False)
                wb.Worksheets.Worksheet(1).Column(4).Style.Protection.SetLocked(False)
                wb.Worksheets.Worksheet(1).Column(5).Style.Protection.SetLocked(False)
                wb.Worksheets.Worksheet(1).Column(6).Style.Protection.SetLocked(False)
                wb.Worksheets.Worksheet(1).Column(7).Style.Protection.SetLocked(False)
                wb.Worksheets.Worksheet(1).Column(8).Style.Protection.SetLocked(False)
                wb.Worksheets.Worksheet(1).Column(9).Style.Protection.SetLocked(False)
                wb.Worksheets.Worksheet(1).Column(10).Style.Protection.SetLocked(False)
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=" & strFileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") & ".xlsx")
                Using MyMemoryStream As MemoryStream = New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.End()
                End Using
            End Using
        Catch ex As Exception
        End Try
    End Sub
    Protected Sub ddlRecords_TextChanged(sender As Object, e As EventArgs)
        Try
            If ddlRecords.SelectedValue = "1" Then
                pnlforlessRecord.Visible = True
                pnlforExcelRecord.Visible = False
                pnlGrid.Visible = False
                pnlUpload.Visible = False
                lbtnExport.Visible = False
            ElseIf ddlRecords.SelectedValue = "2" Then
                pnlforlessRecord.Visible = False
                pnlforExcelRecord.Visible = True
                pnlUpload.Visible = False
                pnlGrid.Visible = False
                lbtnExport.Visible = False
            Else
                pnlforlessRecord.Visible = False
                pnlforExcelRecord.Visible = False
                pnlUpload.Visible = False
                pnlGrid.Visible = False
                lbtnExport.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lbkEdit_Click(sender As Object, e As EventArgs)
        Try
            Dim rowIndex As Integer = Convert.ToInt32((TryCast((TryCast(sender, LinkButton)).NamingContainer, GridViewRow)).RowIndex)
            Dim row As GridViewRow = GridView4.Rows(rowIndex)
            txtPopupPerno.Text = (TryCast(row.FindControl("lblGridPerno"), Label)).Text
            lblPopupYear.Text = (TryCast(row.FindControl("lblGridYear"), Label)).Text
            lblPopupCycle.Text = (TryCast(row.FindControl("lblGridCycle"), Label)).Text
            txtPopupName.Text = (TryCast(row.FindControl("lblGridName"), Label)).Text
            txtPopupEqvLvl.Text = (TryCast(row.FindControl("lblGridEqvLevel"), Label)).Text
            txtPopupEmail.Text = (TryCast(row.FindControl("lblGridEmail"), Label)).Text
            txtPopupReportingperno.Text = (TryCast(row.FindControl("lblGridReportingPNO"), Label)).Text
            txtPopupBperno.Text = (TryCast(row.FindControl("lblGridBHRPNO"), Label)).Text
            txtPopupBhrName.Text = (TryCast(row.FindControl("lblGridBHRName"), Label)).Text
            txtPopupDottedPno.Text = (TryCast(row.FindControl("lblGridDottedPerno"), Label)).Text
            txtPopupExecutivePno.Text = (TryCast(row.FindControl("lblGridExecPerno"), Label)).Text
            txtStep1SD.Text = (TryCast(row.FindControl("lblGridS1SD"), Label)).Text
            txtStep1ED.Text = (TryCast(row.FindControl("lblGridS1ED"), Label)).Text
            txtStep2SD.Text = (TryCast(row.FindControl("lblGridS2SD"), Label)).Text
            txtStep2ED.Text = (TryCast(row.FindControl("lblGridS2ED"), Label)).Text
            txtStep3SD.Text = (TryCast(row.FindControl("lblGridS3SD"), Label)).Text
            txtStep3ED.Text = (TryCast(row.FindControl("lblGridS3ED"), Label)).Text
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", "openEmployeeRecordUpdateModal()", True)
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub btnEmployeeRecordUpdate_Click(sender As Object, e As EventArgs)
        Dim strPersonalNo, strCycle, strYear As String
        Dim strBhrNo, strEmail, strStep1StartDate, strStep1EndDate As String
        Dim strStep2StartDate, strStep2EndDate, strStep3StartDate, strStep3EndDate As String
        Dim strBhrName, strDottedPerno, strExecutivePerno, strReportingPerno, strEqvLevel As String
        Dim sqlUpdateQuery As String
        Dim intCount As Integer = 0
        Try
            strPersonalNo = txtPopupPerno.Text
            strYear = lblPopupYear.Text
            strCycle = lblPopupCycle.Text
            strEmail = txtPopupEmail.Text.Trim
            strBhrNo = txtPopupBperno.Text.Trim
            strStep1StartDate = txtStep1SD.Text.Trim
            strStep1EndDate = txtStep1ED.Text.Trim
            strStep2StartDate = txtStep2SD.Text.Trim
            strStep2EndDate = txtStep2ED.Text.Trim
            strStep3StartDate = txtStep3SD.Text.Trim
            strStep3EndDate = txtStep3ED.Text.Trim
            strBhrName = txtPopupBhrName.Text.Trim
            strDottedPerno = txtPopupDottedPno.Text.Trim
            strExecutivePerno = txtPopupExecutivePno.Text.Trim
            strReportingPerno = txtPopupReportingperno.Text.Trim
            strEqvLevel = txtPopupEqvLvl.Text.Trim.ToUpper

            Dim regex As Regex = New Regex("^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
            Dim match As Match = regex.Match(strEmail.Trim)
            If match.Success And Not String.IsNullOrEmpty(strEqvLevel) And strEqvLevel.Length < 6 And strReportingPerno.Length = 6 And Not String.IsNullOrEmpty(strBhrName) And strBhrNo.Length = 6 And isCorrectPernoLength(strExecutivePerno) And isCorrectPernoLength(strDottedPerno) Then
                sqlUpdateQuery = "UPDATE t_emp_master_feedback360 SET EMA_EMAIL_ID=:EMA_EMAIL_ID, EMA_EQV_LEVEL = :EMA_EQV_LEVEL, EMA_REPORTING_TO_PNO = :EMA_REPORTING_TO_PNO, EMA_BHR_PNO=:EMA_BHR_PNO, EMA_BHR_NAME = :EMA_BHR_NAME, EMA_DOTTED_PNO = :EMA_DOTTED_PNO, EMA_PERS_EXEC_PNO = :EMA_PERS_EXEC_PNO,"
                sqlUpdateQuery += " EMA_STEP1_STDT = :EMA_STEP1_STDT, EMA_STEP1_ENDDT = :EMA_STEP1_ENDDT, EMA_STEP2_STDT = :EMA_STEP2_STDT,  EMA_STEP2_ENDDT = :EMA_STEP2_ENDDT, EMA_STEP3_STDT = :EMA_STEP3_STDT, EMA_STEP3_ENDDT = :EMA_STEP3_ENDDT "
                sqlUpdateQuery += " WHERE EMA_YEAR = :EMA_YEAR and EMA_CYCLE=:EMA_CYCLE and EMA_PERNO =:EMA_PERNO "
                If conHrps.State = ConnectionState.Closed Then
                    conHrps.Open()
                End If
                Dim command As New OracleCommand(sqlUpdateQuery, conHrps)
                command.Parameters.Add(New OracleParameter(":EMA_YEAR", strYear))
                command.Parameters.Add(New OracleParameter(":EMA_CYCLE", strCycle))
                command.Parameters.Add(New OracleParameter(":EMA_PERNO", strPersonalNo))
                command.Parameters.Add(New OracleParameter(":EMA_EMAIL_ID", strEmail))
                command.Parameters.Add(New OracleParameter(":EMA_EQV_LEVEL", strEqvLevel))
                command.Parameters.Add(New OracleParameter(":EMA_REPORTING_TO_PNO", strReportingPerno))
                command.Parameters.Add(New OracleParameter(":EMA_BHR_PNO", strBhrNo))
                command.Parameters.Add(New OracleParameter(":EMA_BHR_NAME", strBhrName))
                command.Parameters.Add(New OracleParameter(":EMA_DOTTED_PNO", IIf(String.IsNullOrEmpty(strDottedPerno), DBNull.Value, strDottedPerno)))
                command.Parameters.Add(New OracleParameter(":EMA_PERS_EXEC_PNO", IIf(String.IsNullOrEmpty(strExecutivePerno), DBNull.Value, strDottedPerno)))
                command.Parameters.Add(New OracleParameter(":EMA_STEP1_STDT", IIf(String.IsNullOrEmpty(strStep1StartDate), DBNull.Value, strStep1StartDate)))
                command.Parameters.Add(New OracleParameter(":EMA_STEP1_ENDDT", IIf(String.IsNullOrEmpty(strStep1EndDate), DBNull.Value, strStep1EndDate)))
                command.Parameters.Add(New OracleParameter(":EMA_STEP2_STDT", IIf(String.IsNullOrEmpty(strStep2StartDate), DBNull.Value, strStep2StartDate)))
                command.Parameters.Add(New OracleParameter(":EMA_STEP2_ENDDT", IIf(String.IsNullOrEmpty(strStep2EndDate), DBNull.Value, strStep2EndDate)))
                command.Parameters.Add(New OracleParameter(":EMA_STEP3_STDT", IIf(String.IsNullOrEmpty(strStep3StartDate), DBNull.Value, strStep3StartDate)))
                command.Parameters.Add(New OracleParameter(":EMA_STEP3_ENDDT", IIf(String.IsNullOrEmpty(strStep3EndDate), DBNull.Value, strStep3EndDate)))
                intCount = command.ExecuteNonQuery()
                'ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", "removeBackdrop()", True)
                If intCount > 0 Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Record updated successfully!")
                    lbkLessSearch_Click(sender, e)
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Record not updated, please check & try again later!")
                End If
            Else
                Dim strError As String = ""
                If Not match.Success Then
                    strError += "Email is incorrect"
                End If
                If String.IsNullOrEmpty(strEqvLevel) Then
                    strError += ", Equivalent Level is blank"
                End If
                If strEqvLevel.Length > 5 Then
                    strError += ", Equivalent Level value length is incorrect"
                End If
                If strReportingPerno.Length <> 6 Then
                    strError += ", Reporting Perno is incorrect"
                End If
                If String.IsNullOrEmpty(strBhrName) Then
                    strError += ", BHR Name is blank"
                End If
                If strBhrNo.Length <> 6 Then
                    strError += ", BHR Perno is incorrect"
                End If
                If Not String.IsNullOrEmpty(strDottedPerno.Trim) Then
                    If strDottedPerno.Length <> 6 Then
                        strError += ", Dotted Perno is incorrect"
                    End If
                End If
                If Not String.IsNullOrEmpty(strExecutivePerno.Trim) Then
                    If strExecutivePerno.Length <> 6 Then
                        strError += ", Executive Head Perno is incorrect"
                    End If
                End If
                ShowGenericMessageModal(CommonConstants.AlertType.error, "" + strError.TrimStart(",").Trim + ",! Please enter correct detail & try again!")
            End If
        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Some error occured while updating the record!")
        End Try
    End Sub
    Protected Sub lbkLessSearch_Click(sender As Object, e As EventArgs)
        Dim strTextValue As String = ""
        Try
            strTextValue = txtPernoforLessRecord.Text.Trim.Replace(" ", "")
            If String.IsNullOrEmpty(strTextValue) Then
                txtPernoforLessRecord.Text = String.Empty
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter personall number!")
                Exit Sub
            End If
            Dim strPerNoList As List(Of String) = strTextValue.Trim.Split(","c).ToList()
            Dim qryrespond As New OracleCommand
            qryrespond.Connection = conHrps
            qryrespond.Parameters.Clear()
            If ChkRole1() Then
                qryrespond.CommandText = "select EMA_YEAR,EMA_CYCLE,EMA_PERNO,ema_ename,ema_desgn_desc,ema_email_id,ema_eqv_level,ema_reporting_to_pno,ema_bhr_pno,ema_bhr_name,ema_dotted_pno,ema_pers_exec_pno,to_char(ema_step1_stdt,'dd-Mon-yyyy') ema_step1_stdt,to_char(ema_step1_enddt,'dd-Mon-yyyy') ema_step1_enddt,to_char(ema_step2_stdt,'dd-Mon-yyyy') ema_step2_stdt,to_char(ema_step2_enddt,'dd-Mon-yyyy') ema_step2_enddt,to_char(ema_step3_stdt,'dd-Mon-yyyy') ema_step3_stdt,to_char(ema_step3_enddt,'dd-Mon-yyyy') ema_step3_enddt from t_emp_master_feedback360 where EMA_PERNO NOT IN (SELECT EE_PNO FROM t_emp_excluded WHERE EE_YEAR=:EMA_YEAR AND EE_CL=:EMA_CYCLE) AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE "

                'If txtPernoforLessRecord.Text.Trim.Length > 0 Then
                '    qryrespond.CommandText += " AND EMA_PERNO=:EMA_PERNO"
                '    qryrespond.Parameters.AddWithValue("EMA_PERNO", txtPernoforLessRecord.Text.Trim.ToString())
                'End If

                'qryrespond.CommandText += " order by EMA_PERNO"

                Dim parameterNames = New List(Of String)(strPerNoList.Count)

                For index As Integer = 0 To strPerNoList.Count - 1
                    Dim name As String = ":perNo" & index
                    qryrespond.Parameters.AddWithValue(name, strPerNoList(index))
                    parameterNames.Add(name)
                Next
                qryrespond.CommandText += String.Format(" AND EMA_PERNO IN ({0})", String.Join(", ", parameterNames))
                qryrespond.CommandText += " order by EMA_PERNO"

                qryrespond.Parameters.AddWithValue("EMA_YEAR", txtYearforLessRecord.Text.Trim.ToString())
                qryrespond.Parameters.AddWithValue("EMA_CYCLE", txtCycleforLessRecord.Text.Trim.ToString())
                Dim dt = getDataInDt(qryrespond)
                If dt.Rows.Count > 0 Then
                    GridView4.DataSource = dt
                    GridView4.DataBind()
                    lbtnExport.Visible = True
                    pnlUpload.Visible = False
                Else
                    GridView4.DataSource = dt
                    GridView4.DataBind()
                    lbtnExport.Visible = False
                    pnlUpload.Visible = False
                End If
                pnlGrid.Visible = True
                GridView4.Columns(0).Visible = True
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub lbkLessRefresh_Click(sender As Object, e As EventArgs)
        txtPernoforLessRecord.Text = String.Empty
        GridView4.DataSource = Nothing
        GridView4.DataBind()
        pnlGrid.Visible = False
        txtPernoforLessRecord.Focus()
    End Sub
    Protected Sub lbtnExcelSectionRefresh_Click(sender As Object, e As EventArgs)
        ddlSGrade.SelectedIndex = -1
        ddlExecHead.SelectedIndex = -1
        ddlSubarea.SelectedIndex = -1
        txtPerno.Text = String.Empty
        GridView4.DataSource = Nothing
        GridView4.DataBind()
        pnlGrid.Visible = False
        pnlUpload.Visible = False
        lbtnExport.Visible = False
    End Sub
    Protected Function checkExcelFileTemplate(ByVal dtUploadedExcelFile As DataTable) As Boolean
        Dim isTrue As Boolean = True
        Try
            Using wb As XLWorkbook = New XLWorkbook(Server.MapPath("~/Files/Template.xlsx"))
                Dim workSheet As IXLWorksheet = wb.Worksheet(1)

                'Create a new DataTable.
                Dim dt As New DataTable()

                'Loop through the Worksheet rows.
                Dim firstRow As Boolean = True
                For Each row As IXLRow In workSheet.Rows()
                    'Use the first row to add columns to DataTable.
                    If firstRow Then
                        For Each cell As IXLCell In row.Cells()
                            dt.Columns.Add(cell.Value.ToString())
                        Next
                        firstRow = False
                    End If
                Next

                If dt.Columns.Count = dtUploadedExcelFile.Columns.Count Then
                    For i = 0 To dt.Columns.Count - 1
                        If dt.Columns(i).ColumnName.Trim.ToUpper = dtUploadedExcelFile.Columns(i).ColumnName.Trim.ToUpper Then
                            isTrue = True
                        Else
                            isTrue = False
                            Exit For
                        End If
                    Next
                Else
                    isTrue = False
                End If
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return isTrue
    End Function
    Protected Sub lbtnExcelTemplate_Click(sender As Object, e As EventArgs)
        Try
            Dim filePath As String = Server.MapPath("~/Files/Template.xlsx")
            If File.Exists(filePath) Then
                Response.ContentType = ContentType
                Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(filePath)))
                Response.WriteFile(filePath)
                Response.End()
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
