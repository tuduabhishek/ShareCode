Imports System.Data.OracleClient
Imports System.Data
Partial Class SurveyStatusScr
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

#Region "GenericMessage"
    ''' <summary>
    ''' Method to show the message created during operations on the form.
    ''' </summary>
    ''' <param name="alertType"></param>
    ''' <param name="Message"></param>
    ''' <remarks></remarks>
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("USER_ID") = "148536"
            'Session("USER_ID") = "150000"
            getFy()
            getsrlno()
        End If
    End Sub

    Private Sub getFy()
        Try
            Dim r As New OracleCommand()
            r.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim g = getRecordInDt(r, conHrps)
            If g.Rows.Count > 0 Then
                ViewState("FY") = g.Rows(0)("IRC_DESC").ToString()
                txtYear.Text = ViewState("FY")
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
                txtCycle.Text = ViewState("SRLNO")
                'txtCycleEE.Text = ViewState("SRLNO")
            End If
        Catch ex As Exception


        End Try
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

    Public Function getRecord(ByVal sql As String, ByVal cn As OracleConnection) As DataTable
        Dim cmd As New OracleCommand(sql, cn)
        cmd.CommandTimeout = 100
        If cn.State = ConnectionState.Closed Then
            cn.Open()
        End If
        Dim da As New OracleDataAdapter(cmd)
        Dim dt As New DataTable()
        da.Fill(dt)
        If cn.State = ConnectionState.Open Then
            cn.Close()
        End If
        da.Dispose()
        Return dt

    End Function


    Public Function BindSurStat()
        Try
            Dim Perno = txtAssesPno.Text
            If (Perno Is Nothing Or Perno <> "") Then
                Dim sqlQuery As String = String.Empty
                sqlQuery = "select SS_YEAR, SS_ASSES_PNO, SS_CATEG, SS_ID, SS_PNO, SS_NAME,  SS_EMAIL, SS_STATUS, SS_TAG, SS_DEL_TAG, TO_CHAR(SS_APP_DT,'DD-MM-YYYY') SS_APP_DT ,SS_LEVEL, SS_APPROVER, SS_APP_TAG, TO_CHAR(SS_TAG_DT,'DD-MM-YYYY') SS_TAG_DT, SS_WFL_STATUS, SS_INTSH_OTP, SS_FLAG1, SS_FLAG_WOTP, SS_OTP_COUNT,  SS_FLAG4 from hrps.t_survey_status where SS_ASSES_PNO='" & Perno & "' and SS_YEAR='2023'"

                Dim dt As DataTable = getRecord(sqlQuery, conHrps)

                If dt.Rows.Count > 0 Then
                    GvSurStat.DataSource = Nothing
                    GvSurStat.DataBind()
                    GvSurStat.DataSource = dt
                    GvSurStat.DataBind()
                Else
                    GvSurStat.DataSource = Nothing
                    GvSurStat.DataBind()
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Data not found !")
                End If
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter Perno!")
            End If

        Catch ex As Exception
            'Throw ex
        End Try
    End Function

    Protected Sub btnScrh_Click(sender As Object, e As EventArgs)
        BindSurStat()
    End Sub

    Protected Sub GvSurStat_RowEditing(sender As Object, e As GridViewEditEventArgs)
        Try
            GvSurStat.EditIndex = e.NewEditIndex
            BindSurStat()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GvSurStat_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Try

            Dim lbl_SS_YEAR = CType(GvSurStat.Rows(e.RowIndex).FindControl("lbl_SS_YEAR"), Label).Text
            Dim lbl_SS_ASSES_PNO = CType(GvSurStat.Rows(e.RowIndex).FindControl("lbl_SS_ASSES_PNO"), Label).Text
            Dim lbl_SS_ID = CType(GvSurStat.Rows(e.RowIndex).FindControl("lbl_SS_ID"), Label).Text
            Dim lbl_SS_PNO = CType(GvSurStat.Rows(e.RowIndex).FindControl("lbl_SS_PNO"), Label).Text


            Dim txt_email = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_email"), TextBox).Text
            Dim txt_SS_STATUS = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_STATUS"), TextBox).Text
            Dim txt_SS_TAG = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_TAG"), TextBox).Text
            Dim txt_SS_DEL_TAG = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_DEL_TAG"), TextBox).Text
            Dim txt_SS_APP_DT = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_APP_DT"), TextBox).Text
            Dim txt_SS_LEVEL = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_LEVEL"), TextBox).Text
            Dim txt_SS_APPROVER = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_APPROVER"), TextBox).Text
            Dim txt_SS_APP_TAG = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_APP_TAG"), TextBox).Text
            Dim txt_SS_TAG_DT = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_TAG_DT"), TextBox).Text
            Dim txt_SS_WFL_STATUS = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_WFL_STATUS"), TextBox).Text
            Dim txt_SS_FLAG1 = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_FLAG1"), TextBox).Text
            Dim txt_SS_FLAG_WOTP = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_FLAG_WOTP"), TextBox).Text
            Dim txt_SS_OTP_COUNT = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_OTP_COUNT"), TextBox).Text
            Dim txt_SS_FLAG4 = CType(GvSurStat.Rows(e.RowIndex).FindControl("txt_SS_FLAG4"), TextBox).Text






            Dim Query = "UPDATE hrps.t_survey_status SET SS_EMAIL = '" & txt_email & "', SS_STATUS= '" & txt_SS_STATUS & "', SS_TAG= '" & txt_SS_TAG & "', SS_DEL_TAG='" & txt_SS_DEL_TAG & "', SS_APP_DT= to_date('" & txt_SS_APP_DT & "','DD/MM/YYYY') , SS_LEVEL='" & txt_SS_LEVEL & "' , SS_APPROVER= '" & txt_SS_APPROVER & "',SS_APP_TAG= '" & txt_SS_APP_TAG & "',SS_TAG_DT= to_date('" + txt_SS_TAG_DT + "','DD/MM/YYYY') , SS_WFL_STATUS= '" & txt_SS_WFL_STATUS & "', SS_FLAG1='" & txt_SS_FLAG1 & "' ,SS_FLAG_WOTP= '" & txt_SS_FLAG_WOTP & "',  SS_OTP_COUNT= '" & txt_SS_OTP_COUNT & "',SS_FLAG4='" & txt_SS_FLAG4 & "'  where SS_YEAR='" + lbl_SS_YEAR + "' and SS_ASSES_PNO= '" + lbl_SS_ASSES_PNO + "'  and SS_ID='" + lbl_SS_ID + "' and SS_PNO='" + lbl_SS_PNO + "' "

            Dim cmd As New OracleCommand(Query, conHrps)
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd.ExecuteNonQuery()
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
            GvSurStat.EditIndex = -1 '// no row in edit mode 
            BindSurStat()
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Record updated successfully..")

            'ScriptManager.RegisterStartupScript(Me, GetType(Page), "asdf", "alert('Record updated successfully..');", True)


        Catch ex As Exception
            'Throw ex
        End Try

    End Sub

    Protected Sub GvSurStat_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        Try
            GvSurStat.EditIndex = -1
            BindSurStat()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkRefresh_Click(sender As Object, e As EventArgs) Handles lnkRefresh.Click
        Response.Redirect("SurveyStatusScr.aspx")
    End Sub

End Class
