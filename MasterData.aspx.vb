Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports ClosedXML.Excel
Partial Class MasterData
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim cmd As OracleCommand
    Dim drRead As OracleDataReader
    Public Const companyCode As String = "1000"
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "MASTER DATA ENTRY"
                getFy()
                getsrlno()
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
                txtYearIE.Text = ViewState("FY")
                'txtYearEE.Text = ViewState("FY")
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
                txtCycleIE.Text = ViewState("SRLNO")
                'txtCycleEE.Text = ViewState("SRLNO")
            End If
        Catch ex As Exception


        End Try
    End Sub

    Protected Sub rdbtnSelection_SelectedIndexChanged(sender As Object, e As EventArgs)
        If rdbtnSelection.SelectedValue = "0" Then
            pnlIncludeEmployee.Visible = True
            'pnlExcludeEmployee.Visible = False
            pnlGrid.Visible = False
        ElseIf rdbtnSelection.SelectedValue = "1" Then
            'pnlExcludeEmployee.Visible = True
            pnlIncludeEmployee.Visible = True
            pnlGrid.Visible = False
        Else
            'pnlExcludeEmployee.Visible = False
            pnlIncludeEmployee.Visible = False
            pnlGrid.Visible = False
        End If
    End Sub
    Protected Sub lbtnExcelTemplate_Click(sender As Object, e As EventArgs)
        Try
            Dim filePath As String = String.Empty
            If rdbtnSelection.SelectedValue = "0" Then
                filePath = Server.MapPath("~/Files/Employee_Table_Master_Data.xlsx")
            ElseIf rdbtnSelection.SelectedValue = "1" Then
                filePath = Server.MapPath("~/Files/Exclude_Table_Master_Data.xlsx")
            End If

            If File.Exists(filePath) Then
                Response.ContentType = ContentType
                Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(filePath)))
                Response.WriteFile(filePath)
                Response.End()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnUpload_Click(sender As Object, e As EventArgs)
        Dim strCycle, strYear As String
        Dim excelPath As String = String.Empty
        Try
            excelPath = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(flUpload.PostedFile.FileName)
            strCycle = txtCycleIE.Text.Trim
            strYear = txtYearIE.Text.Trim
            If flUpload.HasFile Then
                Dim UploadFileName As String = flUpload.PostedFile.FileName
                Dim Extension As String = UploadFileName.Substring(UploadFileName.LastIndexOf(".") + 1).ToLower()
                If Extension <> "xlsx" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please upload Excel (xlsx) file only!")
                    Exit Sub
                End If
                flUpload.SaveAs(excelPath)
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

                    'To remove empty row from datatable
                    dt.AsEnumerable().Where(Function(row) row.ItemArray.All(Function(field) field Is Nothing Or field Is DBNull.Value Or field.Equals(""))).ToList().ForEach(Sub(row) row.Delete())
                    dt.AcceptChanges()
                    'End

                    If dt.Rows.Count > 0 Then
                        If Not checkExcelFileTemplate(dt) Then
                            ShowGenericMessageModal(CommonConstants.AlertType.error, "Please upload correct excel file. Uploaded excel file column(s)/ column name is not matching with template!")
                            Exit Sub
                        End If
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.info, "Uploaded excel file is empty, Please check!")
                    End If
                    If rdbtnSelection.SelectedValue = "0" Then
                        includeTableEmployeeMasterData(dt)
                    ElseIf rdbtnSelection.SelectedValue = "1" Then
                        excludeTableMasterData(dt)
                    End If
                End Using
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Select the File To Upload Data")
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If File.Exists(excelPath) Then
                File.Delete(excelPath)
            End If
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Private Sub includeTableEmployeeMasterData(ByVal dt As DataTable)
        Dim insertQuery, duplicateCheckQuery As String
        Dim personalNo, name, desigCode, designDesc, emailId, eqvLevel, phoneNo, subArea, subAreaDesc, reportPerNo, bhrPerNo,
            bhrName, joiningDate, deptCode, deptDesc, subGrade, empClass, dootedPerNo, execPerNo, execHead, execHeadDesc As String
        Dim dtview As New DataTable
        Dim dtUploadReport, dtIncludeData As New DataTable
        Dim rowArraylist As New ArrayList()
        Dim insertedRow As Integer = 0
        Try
            Dim distinctTable As DataTable = dt.DefaultView.ToTable(True)
            dtUploadReport = distinctTable.Clone
            dtUploadReport.Clear()
            dtUploadReport.Columns.Add("Error")
            dtview = distinctTable.Copy
            dtview.Columns.Add("Error")
            dtIncludeData = distinctTable.Clone
            For i = 0 To distinctTable.Rows.Count - 1
                personalNo = Convert.ToString(distinctTable.Rows(i)("Personal Number"))
                name = Convert.ToString(distinctTable.Rows(i)("Name"))
                desigCode = Convert.ToString(distinctTable.Rows(i)("Designation Code"))
                designDesc = Convert.ToString(distinctTable.Rows(i)("Designation Description"))
                emailId = Convert.ToString(distinctTable.Rows(i)("Email-id"))
                eqvLevel = Convert.ToString(distinctTable.Rows(i)("Eqv Level"))
                phoneNo = Convert.ToString(distinctTable.Rows(i)("Phone No"))
                subArea = Convert.ToString(distinctTable.Rows(i)("Sub Area"))
                subAreaDesc = Convert.ToString(distinctTable.Rows(i)("Sub Area Description"))
                reportPerNo = Convert.ToString(distinctTable.Rows(i)("Reporting Personal No"))
                bhrPerNo = Convert.ToString(distinctTable.Rows(i)("BHR Personal No."))
                bhrName = Convert.ToString(distinctTable.Rows(i)("BHR Name"))
                joiningDate = Convert.ToString(distinctTable.Rows(i)("Joining Date (dd-Mon-yyyy)"))
                deptCode = Convert.ToString(distinctTable.Rows(i)("Department Code"))
                deptDesc = Convert.ToString(distinctTable.Rows(i)("Department Description"))
                subGrade = Convert.ToString(distinctTable.Rows(i)("Sub Grade"))
                empClass = Convert.ToString(distinctTable.Rows(i)("Employee Class"))
                dootedPerNo = Convert.ToString(distinctTable.Rows(i)("Dotted Personal No"))
                execPerNo = Convert.ToString(distinctTable.Rows(i)("Executive Personal No"))
                execHead = Convert.ToString(distinctTable.Rows(i)("Executive Head Code"))
                execHeadDesc = Convert.ToString(distinctTable.Rows(i)("Executive Head Description"))

                If Not String.IsNullOrEmpty(personalNo.Trim) Then
                    duplicateCheckQuery = "Select * from t_emp_master_feedback360 where EMA_YEAR = :EMA_YEAR and EMA_CYCLE=:EMA_CYCLE and EMA_PERNO =:EMA_PERNO"
                    If conHrps.State = ConnectionState.Closed Then
                        conHrps.Open()
                    End If
                    cmd = New OracleCommand(duplicateCheckQuery, conHrps)
                    cmd.Parameters.Add(New OracleParameter(":EMA_YEAR", ViewState("FY").ToString))
                    cmd.Parameters.Add(New OracleParameter(":EMA_CYCLE", ViewState("SRLNO").ToString))
                    cmd.Parameters.Add(New OracleParameter(":EMA_PERNO", personalNo))
                    drRead = cmd.ExecuteReader

                    If Not drRead.HasRows Then
                        Dim employeeRecord As New List(Of String)()
                        employeeRecord.Add(personalNo)
                        employeeRecord.Add(name)
                        employeeRecord.Add(desigCode)
                        employeeRecord.Add(designDesc)
                        employeeRecord.Add(emailId)
                        employeeRecord.Add(eqvLevel)
                        employeeRecord.Add(phoneNo)
                        employeeRecord.Add(subArea)
                        employeeRecord.Add(subAreaDesc)
                        employeeRecord.Add(reportPerNo)
                        employeeRecord.Add(bhrPerNo)
                        employeeRecord.Add(bhrName)
                        employeeRecord.Add(joiningDate)
                        employeeRecord.Add(deptCode)
                        employeeRecord.Add(deptDesc)
                        employeeRecord.Add(subGrade)
                        employeeRecord.Add(empClass)
                        employeeRecord.Add(dootedPerNo)
                        employeeRecord.Add(execPerNo)
                        employeeRecord.Add(execHead)
                        employeeRecord.Add(execHeadDesc)
                        Dim regex As Regex = New Regex("^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                        Dim match As Match = regex.Match(emailId.Trim)
                        If match.Success And checkNullorEmptyValue(employeeRecord) And checkNumberValue(employeeRecord) And checkDates(joiningDate) Then
                            insertQuery = "INSERT INTO HRPS.t_emp_master_feedback360 (EMA_YEAR,EMA_CYCLE,EMA_PERNO,EMA_ENAME,EMA_DESGN_CODE,EMA_DESGN_DESC,EMA_EMAIL_ID,EMA_EQV_LEVEL,EMA_PHONE_NO,EMA_PERS_SUBAREA,EMA_PERS_SUBAREA_DESC,EMA_COMP_CODE,EMA_REPORTING_TO_PNO,EMA_BHR_PNO,EMA_BHR_NAME,EMA_JOINING_DT,EMA_DEPT_CODE,EMA_DEPT_DESC,EMA_EMPL_SGRADE,EMA_EMP_CLASS,EMA_DOTTED_PNO,EMA_PERS_EXEC_PNO,EMA_EXEC_HEAD,EMA_EXEC_HEAD_DESC)"
                            insertQuery += " VALUES (:EMA_YEAR,:EMA_CYCLE,:EMA_PERNO,:EMA_ENAME,:EMA_DESGN_CODE,:EMA_DESGN_DESC,:EMA_EMAIL_ID,:EMA_EQV_LEVEL,:EMA_PHONE_NO,:EMA_PERS_SUBAREA,:EMA_PERS_SUBAREA_DESC,:EMA_COMP_CODE,:EMA_REPORTING_TO_PNO,:EMA_BHR_PNO,:EMA_BHR_NAME,To_DATE(:EMA_JOINING_DT, 'DD-MON-RRRR'),:EMA_DEPT_CODE,:EMA_DEPT_DESC,:EMA_EMPL_SGRADE,:EMA_EMP_CLASS,:EMA_DOTTED_PNO,:EMA_PERS_EXEC_PNO,:EMA_EXEC_HEAD,:EMA_EXEC_HEAD_DESC) "
                            cmd = New OracleCommand(insertQuery, conHrps)
                            cmd.Parameters.Add(New OracleParameter(":EMA_YEAR", ViewState("FY").ToString))
                            cmd.Parameters.Add(New OracleParameter(":EMA_CYCLE", ViewState("SRLNO").ToString))
                            cmd.Parameters.Add(New OracleParameter(":EMA_PERNO", personalNo))
                            cmd.Parameters.Add(New OracleParameter(":EMA_ENAME", name))
                            cmd.Parameters.Add(New OracleParameter(":EMA_DESGN_CODE", desigCode))
                            cmd.Parameters.Add(New OracleParameter(":EMA_DESGN_DESC", designDesc))
                            cmd.Parameters.Add(New OracleParameter(":EMA_EMAIL_ID", emailId))
                            cmd.Parameters.Add(New OracleParameter(":EMA_EQV_LEVEL", eqvLevel))
                            cmd.Parameters.Add(New OracleParameter(":EMA_PHONE_NO", IIf(String.IsNullOrEmpty(phoneNo), DBNull.Value, phoneNo)))
                            cmd.Parameters.Add(New OracleParameter(":EMA_PERS_SUBAREA", subArea))
                            cmd.Parameters.Add(New OracleParameter(":EMA_PERS_SUBAREA_DESC", subAreaDesc))
                            cmd.Parameters.Add(New OracleParameter(":EMA_COMP_CODE", companyCode))
                            cmd.Parameters.Add(New OracleParameter(":EMA_REPORTING_TO_PNO", reportPerNo))
                            cmd.Parameters.Add(New OracleParameter(":EMA_BHR_PNO", bhrPerNo))
                            cmd.Parameters.Add(New OracleParameter(":EMA_BHR_NAME", bhrName))
                            cmd.Parameters.Add(New OracleParameter(":EMA_JOINING_DT", joiningDate))
                            cmd.Parameters.Add(New OracleParameter(":EMA_DEPT_CODE", deptCode))
                            cmd.Parameters.Add(New OracleParameter(":EMA_DEPT_DESC", deptDesc))
                            cmd.Parameters.Add(New OracleParameter(":EMA_EMPL_SGRADE", subGrade))
                            cmd.Parameters.Add(New OracleParameter(":EMA_EMP_CLASS", empClass))
                            cmd.Parameters.Add(New OracleParameter(":EMA_DOTTED_PNO", IIf(String.IsNullOrEmpty(dootedPerNo), DBNull.Value, dootedPerNo)))
                            cmd.Parameters.Add(New OracleParameter(":EMA_PERS_EXEC_PNO", IIf(String.IsNullOrEmpty(execPerNo), DBNull.Value, execPerNo)))
                            cmd.Parameters.Add(New OracleParameter(":EMA_EXEC_HEAD", execHead))
                            cmd.Parameters.Add(New OracleParameter(":EMA_EXEC_HEAD_DESC", execHeadDesc))

                            rowArraylist.Add(cmd)
                            dtIncludeData.ImportRow(distinctTable.Rows(i))
                        Else
                            Dim strError As String = ""
                            If Not checkNullorEmptyValue(employeeRecord) Then
                                strError += "One or more field values are empty"
                            End If
                            If Not checkNumberValue(employeeRecord) Then
                                strError += ", One or more field values are not number"
                            End If
                            If Not match.Success Then
                                strError += ", Incorrect Email"
                            End If
                            If Not checkDates(joiningDate) Then
                                strError += ", Date field is incorrect"
                            End If
                            dtview.Rows(i)("Error") = strError.TrimStart(",").Trim
                            dtUploadReport.ImportRow(dtview.Rows(i))
                        End If
                    Else
                        dtview.Rows(i)("Error") = "P.No. is already available"
                        dtUploadReport.ImportRow(dtview.Rows(i))
                    End If
                    drRead.Close()
                    cmd.Dispose()
                Else
                    dtview.Rows(i)("Error") = "P.No. is empty"
                    dtUploadReport.ImportRow(dtview.Rows(i))
                End If
            Next

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
                    'lbtnSearch_click(sender, e)
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
                    pnlGrid.Visible = True
                    grdIncludedData.DataSource = dtIncludeData
                    grdIncludedData.DataBind()
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

        End Try
    End Sub
    Private Sub excludeTableMasterData(ByVal dt As DataTable)
        Dim insertQuery, duplicateCheckQuery As String
        Dim personalNo, emailId, eqvLevel As String
        Dim dtview As New DataTable
        Dim dtUploadReport, dtExcludeData As New DataTable
        Dim rowArraylist As New ArrayList()
        Dim insertedRow As Integer = 0
        Try
            Dim distinctTable As DataTable = dt.DefaultView.ToTable(True)
            dtUploadReport = distinctTable.Clone
            dtUploadReport.Clear()
            dtUploadReport.Columns.Add("Error")
            dtview = distinctTable.Copy
            dtview.Columns.Add("Error")
            dtExcludeData = distinctTable.Clone
            For i = 0 To distinctTable.Rows.Count - 1
                personalNo = Convert.ToString(distinctTable.Rows(i)("Personal Number"))
                emailId = Convert.ToString(distinctTable.Rows(i)("Email-id"))
                eqvLevel = Convert.ToString(distinctTable.Rows(i)("Eqv Level"))

                If Not String.IsNullOrEmpty(personalNo.Trim) Then
                    duplicateCheckQuery = "Select * from t_emp_excluded where EE_YEAR = :EE_YEAR and EE_CL=:EE_CL and EE_PNO =:EE_PNO"
                    If conHrps.State = ConnectionState.Closed Then
                        conHrps.Open()
                    End If
                    cmd = New OracleCommand(duplicateCheckQuery, conHrps)
                    cmd.Parameters.Add(New OracleParameter(":EE_YEAR", ViewState("FY").ToString))
                    cmd.Parameters.Add(New OracleParameter(":EE_CL", ViewState("SRLNO").ToString))
                    cmd.Parameters.Add(New OracleParameter(":EE_PNO", personalNo))
                    drRead = cmd.ExecuteReader

                    If Not drRead.HasRows Then
                        Dim regex As Regex = New Regex("^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                        Dim match As Match = regex.Match(emailId.Trim)
                        If match.Success Then
                            insertQuery = "INSERT INTO HRPS.T_EMP_EXCLUDED (EE_YEAR,EE_CL,EE_PNO,EE_EMAIL_ID,EE_EQUIV_LEVEL,EE_COMP_CODE,EE_CREATED_BY,EE_CREATED_DT)"
                            insertQuery += " VALUES (:EE_YEAR,:EE_CL,:EE_PNO,:EE_EMAIL_ID,:EE_EQUIV_LEVEL,:EE_COMP_CODE,:EE_CREATED_BY,SYSDATE) "
                            cmd = New OracleCommand(insertQuery, conHrps)
                            cmd.Parameters.Add(New OracleParameter(":EE_YEAR", ViewState("FY").ToString))
                            cmd.Parameters.Add(New OracleParameter(":EE_CL", ViewState("SRLNO").ToString))
                            cmd.Parameters.Add(New OracleParameter(":EE_PNO", personalNo))
                            cmd.Parameters.Add(New OracleParameter(":EE_EMAIL_ID", emailId))
                            cmd.Parameters.Add(New OracleParameter(":EE_EQUIV_LEVEL", IIf(String.IsNullOrEmpty(eqvLevel), DBNull.Value, eqvLevel)))
                            cmd.Parameters.Add(New OracleParameter(":EE_COMP_CODE", companyCode))
                            cmd.Parameters.Add(New OracleParameter(":EE_CREATED_BY", Session("ADM_USER").ToString()))

                            rowArraylist.Add(cmd)
                            dtExcludeData.ImportRow(distinctTable.Rows(i))
                        Else
                            Dim strError As String = ""
                            If Not match.Success Then
                                strError += ", Incorrect Email"
                            End If
                            dtview.Rows(i)("Error") = strError.TrimStart(",").Trim
                            dtUploadReport.ImportRow(dtview.Rows(i))
                        End If
                    Else
                        dtview.Rows(i)("Error") = "P.No. is already available"
                        dtUploadReport.ImportRow(dtview.Rows(i))
                    End If
                    drRead.Close()
                    cmd.Dispose()
                Else
                    dtview.Rows(i)("Error") = "P.No. is empty"
                    dtUploadReport.ImportRow(dtview.Rows(i))
                End If
            Next

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
                    'lbtnSearch_click(sender, e)
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
                    pnlGrid.Visible = True
                    grdIncludedData.DataSource = dtExcludeData
                    grdIncludedData.DataBind()
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
            Throw ex
        End Try
    End Sub
    Protected Sub lbkInvalidRecordDump_Click(sender As Object, e As EventArgs)
        Try
            Dim strFileName = "Invalid_Data"
            Dim dt As DataTable = exporttoDatatable()
            If dt.Rows.Count > 0 Then
                dt.Columns.Remove("Error")
            End If


            Using wb As XLWorkbook = New XLWorkbook(Server.MapPath(IIf(rdbtnSelection.SelectedValue = "0", "~/Files/Employee_Table_Master_Data.xlsx", "~/Files/Exclude_Table_Master_Data.xlsx")))
                Dim NumberOfLastRow As Integer = wb.Worksheets.Worksheet(1).LastRowUsed().RowNumber()
                Dim CellForNewData As IXLCell = wb.Worksheets.Worksheet(1).Cell(NumberOfLastRow + 1, 1)
                CellForNewData.InsertData(dt.Rows)
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
    Public Function exporttoDatatable() As DataTable
        Dim dt As DataTable = New DataTable()
        'For j As Integer = 0 To grdExcelUploadReport.Columns.Count - 1 - 1
        '    dt.Columns.Add(grdExcelUploadReport.Columns(j).HeaderText)
        'Next
        'For i As Integer = 0 To grdExcelUploadReport.Rows.Count - 1
        '    Dim dr As DataRow = dt.NewRow()
        '    For j As Integer = 0 To grdExcelUploadReport.Columns.Count - 1 - 1
        '        dr(grdExcelUploadReport.Columns(j).HeaderText) = grdExcelUploadReport.Rows(i).Cells(j).Text
        '    Next
        '    dt.Rows.Add(dr)
        'Next
        If grdExcelUploadReport.HeaderRow IsNot Nothing Then
            For i As Integer = 0 To grdExcelUploadReport.HeaderRow.Cells.Count - 1
                dt.Columns.Add(grdExcelUploadReport.HeaderRow.Cells(i).Text)
            Next
        End If

        For Each row As GridViewRow In grdExcelUploadReport.Rows
            Dim dr As DataRow
            dr = dt.NewRow()
            For i As Integer = 0 To row.Cells.Count - 1
                dr(i) = Server.HtmlDecode(row.Cells(i).Text.Replace(" ", ""))
            Next
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function
    Protected Function checkNumber(ByVal value As String) As Boolean
        Dim isTrue As Boolean
        Dim i As Integer
        Try
            If String.IsNullOrEmpty(value) Then
                isTrue = True
                Return isTrue
            End If
            isTrue = Integer.TryParse(value, i)
        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Protected Function checkDates(ByVal value As String) As Boolean
        Dim isTrue As Boolean = True
        Dim dateFormat As String = "dd-MMM-yyyy"
        Dim dateTime As DateTime
        Try
            If Not String.IsNullOrEmpty(value) Then
                If DateTime.TryParseExact(value, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, dateTime) Then
                    isTrue = True
                Else
                    isTrue = False
                End If
            End If

        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Protected Function checkNullorEmptyValue(ByVal value As List(Of String)) As Boolean
        Dim isTrue As Boolean = True
        Try
            If String.IsNullOrEmpty(value(0)) Or String.IsNullOrEmpty(value(1)) Or String.IsNullOrEmpty(value(2)) Or String.IsNullOrEmpty(value(3)) Or String.IsNullOrEmpty(value(4)) Or String.IsNullOrEmpty(value(5)) Or String.IsNullOrEmpty(value(7)) Or String.IsNullOrEmpty(value(8)) Or String.IsNullOrEmpty(value(9)) Or String.IsNullOrEmpty(value(10)) Or String.IsNullOrEmpty(value(11)) Or String.IsNullOrEmpty(value(12)) Or String.IsNullOrEmpty(value(13)) Or String.IsNullOrEmpty(value(14)) Or String.IsNullOrEmpty(value(15)) Or String.IsNullOrEmpty(value(16)) Or String.IsNullOrEmpty(value(19)) Or String.IsNullOrEmpty(value(20)) Then
                isTrue = False
                Exit Try
            End If

        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Protected Function checkNumberValue(ByVal value As List(Of String)) As Boolean
        Dim isTrue As Boolean = False
        Try
            If checkNumber(value(0)) Or checkNumber(value(2)) Or checkNumber(value(6)) Or checkNumber(value(9)) Or checkNumber(value(10)) Or checkNumber(value(13)) Or checkNumber(value(16)) Or checkNumber(value(17)) Or checkNumber(value(18)) Or checkNumber(value(19)) Then
                isTrue = True
            End If

        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Protected Function checkExcelFileTemplate(ByVal dtUploadedExcelFile As DataTable) As Boolean
        Dim isTrue As Boolean = True
        Try
            Using wb As XLWorkbook = New XLWorkbook(Server.MapPath(IIf(rdbtnSelection.SelectedValue = "0", "~/Files/Employee_Table_Master_Data.xlsx", "~/Files/Exclude_Table_Master_Data.xlsx")))
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
End Class
