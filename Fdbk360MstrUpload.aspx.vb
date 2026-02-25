
Imports System.Data
Imports System.Data.OracleClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class Fdbk360MstrUpload
    Inherits System.Web.UI.Page
    Dim con As New OracleConnection(ConfigurationManager.ConnectionStrings("Myhrps").ConnectionString)
    Public adid() As String
    Dim strUser As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim userName As [String] = Page.User.Identity.Name.ToString()
        'adid = userName.Split("\"c)
        'strUser = adid(1).Substring(0, 6)
        Session("strUser") = strUser
        strUser = "150006"
        'If con.State = ConnectionState.Closed Then
        '    con.Open()
        'End If
        'Try
        '    Dim qry As String = String.Empty
        '    qry = "UPDATE hrps.t_ir_hrslip SET TIH_MAIL_STATUS = 'F' WHERE TIH_YEAR_MONTH = '202324' and TIH_MAIL_STATUS='2' and TIH_PERNR  in ('121967','126661','129846','174572','151629')"
        '    Dim com As New OracleCommand(qry, con)
        '    com.ExecuteNonQuery()


        'Catch ex As Exception
        '    Throw ex
        'Finally
        '    If con.State = ConnectionState.Open Then
        '        con.Close()
        '    End If
        'End Try

    End Sub
    Protected Sub lbtnTemplateDownload_Click(sender As Object, e As EventArgs)
        Dim columns As String() = {"EMA_YEAR", "EMA_CYCLE", "EMA_PERNO", "EMA_ENAME", "EMA_DESGN_CODE", "EMA_DESGN_DESC", "EMA_EMAIL_ID", "EMA_EQV_LEVEL", "EMA_PHONE_NO", "EMA_PERS_SUBAREA", "EMA_PERS_SUBAREA_DESC", "EMA_COMP_CODE", "EMA_REPORTING_TO_PNO", "EMA_BHR_PNO", "EMA_BHR_NAME", "EMA_JOINING_DT", "EMA_DEPT_CODE", "EMA_DEPT_DESC", "EMA_EMPL_SGRADE", "EMA_EMP_CLASS", "EMA_DOTTED_PNO", "EMA_PERS_EXEC_PNO", "EMA_EXEC_HEAD", "EMA_EXEC_HEAD_DESC", "EMA_EMP_TAG", "EMA_STEP1_STDT", "EMA_STEP1_ENDDT", "EMA_STEP2_STDT", "EMA_STEP2_ENDDT", "EMA_STEP3_STDT", "EMA_STEP3_ENDDT", "EMA_STATUS", "EMA_REMARKS"}
        Try
            Using wb As XLWorkbook = New XLWorkbook()
                Dim dt As DataTable = New DataTable("FEEDBACK360")
                For Each column As String In columns
                    dt.Columns.Add(column)
                Next
                Dim dr As DataRow = dt.NewRow()
                For Each column As DataColumn In dt.Columns
                    dr(column.ColumnName) = column.ColumnName
                Next
                'Add Header rows from DataSet Table to DataTable.
                dt.Rows.Add(dr)

                Dim ws = wb.Worksheets.Add(dt.TableName)
                ws.Cell(1, 1).InsertData(dt.Rows)
                ws.Columns().AdjustToContents()

                'Export the Excel file.
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=Feedback360_Master_Template.xlsx")
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

    Protected Sub lbtnUpload_Click(sender As Object, e As EventArgs)
        Dim rowArraylist As New ArrayList()
        Dim insertedRow As Integer = 0
        Dim query As String = ""
        Try
            If fluExcel.HasFile Then
                'Save the uploaded Excel file.
                Dim filePath As String = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(fluExcel.PostedFile.FileName)
                fluExcel.SaveAs(filePath)

                'Create a new DataTable.
                Dim dt As New DataTable()

                'Open the Excel file using ClosedXML.
                Using workBook As New XLWorkbook(filePath)
                    'Read the first Sheet from Excel file.
                    Dim workSheet As IXLWorksheet = workBook.Worksheet(1)

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

                            'Dim i As Integer = 0
                            'For Each cell As IXLCell In row.Cells()
                            '    dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString()
                            '    i += 1
                            'Next

                            Dim i As Integer = 0
                            For Each cell As IXLCell In row.Cells(1, dt.Columns.Count)
                                If i = 3 Then
                                    dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString
                                Else
                                    dt.Rows(dt.Rows.Count - 1)(i) = cell.Value.ToString.ToUpper.Trim
                                End If
                                i += 1
                            Next
                        End If
                    Next
                End Using

                If dt.Rows.Count > 0 Then
                    For Each dr In dt.Rows
                        query = "INSERT INTO HRPS.T_EMP_MASTER_FEEDBACK360 (EMA_YEAR,	EMA_CYCLE,	EMA_PERNO,	EMA_ENAME,	EMA_DESGN_CODE,	EMA_DESGN_DESC,	EMA_EMAIL_ID,	EMA_EQV_LEVEL,	EMA_PHONE_NO,	EMA_PERS_SUBAREA,	EMA_PERS_SUBAREA_DESC,	EMA_COMP_CODE,	EMA_REPORTING_TO_PNO,	EMA_BHR_PNO,	EMA_BHR_NAME,	EMA_JOINING_DT,	EMA_DEPT_CODE,	EMA_DEPT_DESC,	EMA_EMPL_SGRADE,	EMA_EMP_CLASS,	EMA_DOTTED_PNO,	EMA_PERS_EXEC_PNO,	EMA_EXEC_HEAD,	EMA_EXEC_HEAD_DESC,EMA_EMP_TAG, EMA_STEP1_STDT, EMA_STEP1_ENDDT, EMA_STEP2_STDT, EMA_STEP2_ENDDT, EMA_STEP3_STDT, EMA_STEP3_ENDDT, EMA_STATUS, EMA_REMARKS) VALUES ('" + Convert.ToString(dr("EMA_YEAR")).Trim + "','" + Convert.ToString(dr(1)).Trim + "','" + Convert.ToString(dr(2)).Trim + "','" + Convert.ToString(dr(3)).Trim + "','" + Convert.ToString(dr(4)).Trim + "','" + Convert.ToString(dr(5)).Trim + "','" + Convert.ToString(dr(6)).Trim + "','" + Convert.ToString(dr(7)).Trim + "','" + Convert.ToString(dr(8)).Trim + "','" + Convert.ToString(dr(9)).Trim + "','" + Convert.ToString(dr(10)).Trim + "','" + Convert.ToString(dr(11)).Trim + "','" + Convert.ToString(dr(12)).Trim + "','" + Convert.ToString(dr(13)).Trim + "','" + Convert.ToString(dr(14)).Trim + "',"

                        If dr(15) <> "" Then
                            query += "'" & Convert.ToDateTime(dr(15)).ToString("dd-MMM-yyyy") & "', "
                        Else
                            query += " null, "
                        End If

                        query += " '" + Convert.ToString(dr(16)).Trim + "','" + Convert.ToString(dr(17)).Trim + "','" + Convert.ToString(dr(18)).Trim + "','" + Convert.ToString(dr(19)).Trim + "','" + Convert.ToString(dr(20)).Trim + "','" + Convert.ToString(dr(21)).Trim + "','" + Convert.ToString(dr(22)).Trim + "','" + Convert.ToString(dr(23)).Trim + "','" + Convert.ToString(dr(24)).Trim + "',"

                        If dr(25) <> "" Then
                            query += " '" & Convert.ToDateTime(dr(25)).ToString("dd-MMM-yyyy") & "', "
                        Else
                            query += " null, "
                        End If
                        If dr(26) <> "" Then
                            query += " '" & Convert.ToDateTime(dr(26)).ToString("dd-MMM-yyyy") & "', "
                        Else
                            query += " null, "
                        End If
                        If dr(27) <> "" Then
                            query += " '" & Convert.ToDateTime(dr(27)).ToString("dd-MMM-yyyy") & "', "
                        Else
                            query += " null, "
                        End If
                        If dr(28) <> "" Then
                            query += " '" & Convert.ToDateTime(dr(28)).ToString("dd-MMM-yyyy") & "', "
                        Else
                            query += " null, "
                        End If
                        If dr(29) <> "" Then
                            query += " '" & Convert.ToDateTime(dr(29)).ToString("dd-MMM-yyyy") & "', "
                        Else
                            query += " null, "
                        End If

                        If dr(30) <> "" Then
                            query += " '" & Convert.ToDateTime(dr(30)).ToString("dd-MMM-yyyy") & "', "
                        Else
                            query += " null, "
                        End If


                        query += " '" + Convert.ToString(dr(31)).Trim + "','" + Convert.ToString(dr(32)).Trim + "')"

                        Dim OracleCommand = New OracleCommand(query, con)

                        rowArraylist.Add(OracleCommand)
                    Next
                End If

                If rowArraylist.Count > 0 Then
                    Dim counter As Integer = 0
                    If con.State = ConnectionState.Closed Then
                        con.Open()
                    End If
                    Dim transcationforUpdate As OracleTransaction
                    transcationforUpdate = con.BeginTransaction()
                    Try
                        For counter = 0 To rowArraylist.Count - 1
                            Dim orclCmd As New OracleCommand()
                            orclCmd = rowArraylist.Item(counter)
                            orclCmd.Transaction = transcationforUpdate
                            insertedRow += orclCmd.ExecuteNonQuery()
                        Next
                        transcationforUpdate.Commit()
                        ShowMessage(String.Format("{0} rows uploaded successfully", insertedRow), MessageType.Success)
                    Catch ex As Exception
                        transcationforUpdate.Rollback()
                        ShowMessage("Some Error occured While uploading data", MessageType.Errors)
                    Finally
                        If con.State = ConnectionState.Open Then
                            con.Close()
                        End If
                    End Try
                End If

                If File.Exists(filePath) Then
                    File.Delete(filePath)
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try


    End Sub
    Enum MessageType
        Success
        Errors
        Info
        Warning
    End Enum
    Protected Sub ShowMessage(ByVal Message As String, type As MessageType)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "ShowMessage", "ShowMessage('" & Message & "','" & type.ToString & "');", True)
    End Sub
End Class
