Imports System.Data.OracleClient
Imports System.Data
Partial Class ChangeBUHR
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Dim status As Label = TryCast(Me.Master.FindControl("resp"), Label)
                status.Text = "CHANGE BUHR"
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
                txtYear.Text = ViewState("FY")
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
            End If
        Catch ex As Exception


        End Try
    End Sub
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
    Protected Sub btnCngAdd_Click(sender As Object, e As EventArgs)
        Try
            Dim cmd As New OracleCommand()
            cmd.CommandText = "select ema_perno, ema_ename,EMA_BHR_PNO,EMA_BHR_NAME from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno AND ema_year=:ema_year and ema_cycle=:ema_cycle"
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", txtCngAssessPerNo.Text.Trim())
            cmd.Parameters.AddWithValue("ema_year", txtYear.Text.Trim())
            cmd.Parameters.AddWithValue("ema_cycle", txtCycle.Text.Trim())
            Dim da As New OracleDataAdapter(cmd)
            Dim dtBuhr As New DataTable()
            da.Fill(dtBuhr)
            If dtBuhr.Rows.Count > 0 Then
                If ViewState("TagChangeBUHR") = "1" Then
                    CType(ViewState("ChangeBUHR"), DataTable).Rows.Add(dtBuhr.Rows(0)("ema_perno").ToString, dtBuhr.Rows(0)("ema_ename").ToString, dtBuhr.Rows(0)("EMA_BHR_PNO").ToString, dtBuhr.Rows(0)("EMA_BHR_NAME").ToString)
                Else
                    ViewState("ChangeBUHR") = dtBuhr
                    ViewState("TagChangeBUHR") = "1"
                End If

                gdvChangeBUHR.DataSource = CType(ViewState("ChangeBUHR"), DataTable)
                gdvChangeBUHR.DataBind()
                txtCngAssessPerNo.Text = ""
                txtCngBUHRPerno.Text = ""
            Else
                gdvChangeBUHR.DataSource = Nothing
                gdvChangeBUHR.DataBind()
            End If
        Catch ex As Exception


        End Try
    End Sub
    Protected Sub btnCngConfirmList_Click(sender As Object, e As EventArgs)
        If ViewState("TagChangeBUHR") = "1" Then
            pnlCngPnoScreen.Visible = False
            pnlBuhrChangeScreen.Visible = True
        End If
    End Sub
    Protected Sub btnCngSubmit_Click(sender As Object, e As EventArgs)
        Try
            SessionTimeOut()
            Dim dtFinalBuhr = CType(ViewState("ChangeBUHR"), DataTable)
            Dim BuhrList = ""
            If dtFinalBuhr.Rows.Count > 0 Then
                BuhrList = "'"
                For i As Integer = 0 To dtFinalBuhr.Rows.Count - 1
                    If dtFinalBuhr.Rows.Count = 1 Then
                        BuhrList += dtFinalBuhr.Rows(i)(0).ToString
                    ElseIf dtFinalBuhr.Rows.Count - 1 = i Then
                        BuhrList += dtFinalBuhr.Rows(i)(0).ToString + ""
                    Else
                        BuhrList += dtFinalBuhr.Rows(i)(0).ToString + "','"
                    End If
                Next
                BuhrList += "'"
            End If
            Dim buhrNm As String = ""
            Dim cmd As New OracleCommand()

            cmd.CommandText = "select ema_ename from tips.t_empl_all where ema_perno =:ema_perno"
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", txtCngBUHRPerno.Text.Trim)
            Dim f = getDataInDt(cmd)
            If f.Rows.Count > 0 Then
                buhrNm = f.Rows(0)("ema_ename").ToString
            End If


            Dim upBuhr1 As New OracleCommand()

            upBuhr1.CommandText = "UPDATE hrps.t_emp_master_feedback360 SET EMA_BHR_PNO=:EMA_BHR_PNO, EMA_BHR_NAME=:EMA_BHR_NAME  where  EMA_PERNO in (" + BuhrList + ") and EMA_YEAR=:EMA_YEAR and EMA_CYCLE=:EMA_CYCLE"
            upBuhr1.Connection = conHrps
            upBuhr1.Parameters.Clear()
            upBuhr1.Parameters.AddWithValue("EMA_BHR_PNO", txtCngBUHRPerno.Text.Trim)
            upBuhr1.Parameters.AddWithValue("EMA_BHR_NAME", buhrNm)
            'upBuhr.Parameters.AddWithValue("EMA_PERNO", BuhrList)
            upBuhr1.Parameters.AddWithValue("EMA_YEAR", txtYear.Text.Trim.ToString)
            upBuhr1.Parameters.AddWithValue("EMA_CYCLE", txtCycle.Text.Trim.ToString)
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim result = upBuhr1.ExecuteNonQuery()

            If result > 0 Then

                ShowGenericMessageModal(CommonConstants.AlertType.success, "BUHR Per. no. change successfully updated!")
                pnlCngPnoScreen.Visible = True
                pnlBuhrChangeScreen.Visible = False
                ViewState("TagChangeBUHR") = ""
                ViewState("ChangeBUHR") = ""
                txtCngBUHRPerno.Text = ""
                txtCngAssessPerNo.Text = ""
                gdvChangeBUHR.DataSource = Nothing
                gdvChangeBUHR.DataBind()
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Not be updated...!")
            End If
        Catch ex As Exception
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
End Class
