'----------------------------------------------------------------------------------------------------------------------------------------------------------
'SCREEN NAME: UploadEmpData.ASPX.VB
'==========================================================================================================================================================
'DESCRIPTION: Screen for Employee Master Data Management
'==========================================================================================================================================================

Imports System.Data.OracleClient
Imports System.Data
Partial Class UploadEmpData
    Inherits System.Web.UI.Page


    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim dtls As New DataTable
    Dim StDate, toDate As Date
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("USER_ID") = "148536"
            'Session("USER_ID") = "150000"
            PopExecHead()
            PopYear()
            PopCycle()
            PopLevel()
            PopEQVLevel()
            UpdateEmployeeData.Visible = False
            StepUpdataData.Visible = False
            Stepdates.Visible = False
            Stepbtn.Visible = False
            PopSurYear()
            PopSurCycle()
        End If
    End Sub
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
    Public Sub PopExecHead()
        Try
            Dim qryExec As String = String.Empty
            qryExec = "select distinct ema_exec_head,ema_exec_head_desc from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head_desc <>'Not found' and ema_exec_head<>'00000000' and ema_comp_code='1000' order by 2"
            Dim dtExe = GetData(qryExec, conHrps)

            If dtExe.Rows.Count > 0 Then
                ddlExecHd.DataSource = dtExe
                ddlExecHd.DataValueField = "ema_exec_head"
                ddlExecHd.DataTextField = "ema_exec_head_desc"
                ddlExecHd.DataBind()
                ddlExecHd.Items.Insert(0, New ListItem("Please Select", "0"))

                ddlExecutive.DataSource = dtExe
                ddlExecutive.DataValueField = "ema_exec_head"
                ddlExecutive.DataTextField = "ema_exec_head_desc"
                ddlExecutive.DataBind()
                ddlExecutive.Items.Insert(0, New ListItem("Please Select", "0"))
            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Sub PopYear()
        Try
            Dim qryYr As String = String.Empty
            qryYr = " select distinct ema_year from hrps.t_emp_master_feedback360 where ema_comp_code='1000' order by 1 desc"
            Dim dtYr = GetData(qryYr, conHrps)

            If dtYr.Rows.Count > 0 Then

                ddlExYr.DataSource = dtYr
                ddlExYr.DataValueField = "ema_year"
                ddlExYr.DataTextField = "ema_year"
                ddlExYr.DataBind()
                ddlExYr.Items.Insert(0, New ListItem("Please Select", "0"))

                ddlIncYr.DataSource = dtYr
                ddlIncYr.DataValueField = "ema_year"
                ddlIncYr.DataTextField = "ema_year"
                ddlIncYr.DataBind()
                ddlIncYr.Items.Insert(0, New ListItem("Please Select", "0"))

                ddlDMYear.DataSource = dtYr
                ddlDMYear.DataValueField = "ema_year"
                ddlDMYear.DataTextField = "ema_year"
                ddlDMYear.DataBind()
                ddlDMYear.Items.Insert(0, New ListItem("Please Select", "0"))

            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Sub PopCycle()

        Try
            Dim qryCycle As String = String.Empty
            qryCycle = " select distinct ema_cycle from hrps.t_emp_master_feedback360 where ema_comp_code='1000' order by 1 desc"
            Dim dtCyc = GetData(qryCycle, conHrps)

            If dtCyc.Rows.Count > 0 Then

                ddlExCycle.DataSource = dtCyc
                ddlExCycle.DataValueField = "ema_cycle"
                ddlExCycle.DataTextField = "ema_cycle"
                ddlExCycle.DataBind()
                ddlExCycle.Items.Insert(0, New ListItem("Please Select", "0"))

                ddlIncCycle.DataSource = dtCyc
                ddlIncCycle.DataValueField = "ema_cycle"
                ddlIncCycle.DataTextField = "ema_cycle"
                ddlIncCycle.DataBind()
                ddlIncCycle.Items.Insert(0, New ListItem("Please Select", "0"))

                ddlDMCycle.DataSource = dtCyc
                ddlDMCycle.DataValueField = "ema_cycle"
                ddlDMCycle.DataTextField = "ema_cycle"
                ddlDMCycle.DataBind()
                ddlDMCycle.Items.Insert(0, New ListItem("Please Select", "0"))

            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Sub PopLevel()
        Try
            Dim qryLvl As String = String.Empty
            qryLvl = " select distinct ema_empl_sgrade from hrps.t_emp_master_feedback360 where ema_comp_code='1000' order by 1"
            Dim dtLvl = GetData(qryLvl, conHrps)

            If dtLvl.Rows.Count > 0 Then

                ddlExLevel.DataSource = dtLvl
                ddlExLevel.DataValueField = "ema_empl_sgrade"
                ddlExLevel.DataTextField = "ema_empl_sgrade"
                ddlExLevel.DataBind()
                ddlExLevel.Items.Insert(0, New ListItem("Please Select", "0"))

                ddlIncLvl.DataSource = dtLvl
                ddlIncLvl.DataValueField = "ema_empl_sgrade"
                ddlIncLvl.DataTextField = "ema_empl_sgrade"
                ddlIncLvl.DataBind()
                ddlIncLvl.Items.Insert(0, New ListItem("Please Select", "0"))


                ddlDMlevel.DataSource = dtLvl
                ddlDMlevel.DataValueField = "ema_empl_sgrade"
                ddlDMlevel.DataTextField = "ema_empl_sgrade"
                ddlDMlevel.DataBind()
                ddlDMlevel.Items.Insert(0, New ListItem("Please Select", "0"))

            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Sub PopEQVLevel()
        Try
            Dim qryEqvLvl As String = String.Empty
            qryEqvLvl = " select distinct ema_eqv_level from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and ema_eqv_level <>'0' and ema_eqv_level is not null order by 1"
            Dim dt_eqvl = GetData(qryEqvLvl, conHrps)

            If dt_eqvl.Rows.Count > 0 Then

                ddlDMEqv.DataSource = dt_eqvl
                ddlDMEqv.DataValueField = "ema_eqv_level"
                ddlDMEqv.DataTextField = "ema_eqv_level"
                ddlDMEqv.DataBind()
                ddlDMEqv.Items.Insert(0, New ListItem("Please Select", "0"))

            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Sub PopSurYear()
        Try
            Dim qrySurYr As String = String.Empty
            qrySurYr = " select distinct SS_YEAR from hrps.t_survey_status order by 1 desc"
            Dim dtYr1 = GetData(qrySurYr, conHrps)

            If dtYr1.Rows.Count > 0 Then

                ddlSurYr.DataSource = dtYr1
                ddlSurYr.DataValueField = "SS_YEAR"
                ddlSurYr.DataTextField = "SS_YEAR"
                ddlSurYr.DataBind()
                ddlSurYr.Items.Insert(0, New ListItem("Please Select", "0"))

            End If

        Catch ex As Exception

        End Try

    End Sub
    Public Sub PopSurCycle()
        Try
            Dim qrySurCyc As String = String.Empty
            qrySurCyc = " select distinct SS_SRLNO from hrps.t_survey_status order by 1 desc"
            Dim dtCyc1 = GetData(qrySurCyc, conHrps)

            If dtCyc1.Rows.Count > 0 Then

                ddlSurCycle.DataSource = dtCyc1
                ddlSurCycle.DataValueField = "SS_SRLNO"
                ddlSurCycle.DataTextField = "SS_SRLNO"
                ddlSurCycle.DataBind()
                ddlSurCycle.Items.Insert(0, New ListItem("Please Select", "0"))

            End If

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnDisplay_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDisplay.Click
        If ddlExecHd.SelectedValue = "0" And ddlExLevel.SelectedValue = "0" And txtBuhr.Text.Trim = "" And txtPerno.Text.Trim = "" Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select atleast anyone option to display data")
            Exit Sub
        Else
            bindGrid()
        End If

    End Sub
    Public Sub bindGrid()
        Try
            Dim qryGrid As String
            Dim dtgv As New DataTable()

            qryGrid = " Select distinct ema_year,ema_cycle,ema_perno,ema_ename,ema_empl_sgrade,ema_eqv_level,ema_dept_desc,ema_email_id ,ema_comp_code"
            qryGrid += " from hrps.t_emp_master_feedback360 "
            qryGrid += "  where ema_perno not in (select ee_pno from t_emp_excluded)"
            qryGrid += "  and ema_comp_code='1000'"

            If ddlExYr.SelectedValue <> "0" Then
                qryGrid += " and ema_year='" & ddlExYr.SelectedValue & "' "
            End If

            If ddlExCycle.SelectedValue <> "0" Then
                qryGrid += " and ema_cycle='" & ddlExCycle.SelectedValue & "' "
            End If

            If ddlExecHd.SelectedValue <> "0" Then
                qryGrid += "  and ema_exec_head='" & ddlExecHd.SelectedValue & "' "
            End If

            If ddlExLevel.SelectedValue <> "0" Then
                qryGrid += "  and ema_empl_sgrade='" & ddlExLevel.SelectedValue & "' "
            End If

            If txtBuhr.Text.Trim <> "" Then
                qryGrid += " and ema_bhr_pno='" & txtBuhr.Text.Trim.ToString & "' "
            End If

            If txtPerno.Text.Trim <> "" Then
                qryGrid += " And ema_perno ='" & txtPerno.Text.Trim.ToString & "'"
            End If

            qryGrid += " order by 1,2,3"

            dtgv = GetData(qryGrid, conHrps)

            If CType(dtgv, DataTable) Is Nothing Then
                GvExclude.DataSource = Nothing
                GvExclude.DataBind()
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Data already exits !")
                ddlExecHd.SelectedValue = "0"
                ddlExLevel.SelectedValue = "0"
                txtBuhr.Text = ""
                txtPerno.Text = ""
            ElseIf dtgv.Rows.Count > 0 Then
                GvExclude.DataSource = dtgv
                GvExclude.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Function SaveData(ByVal year As String, ByVal cycle As String, ByVal pno As String, ByVal email As String, ByVal eqv As String, ByVal comp As String) As String
        Dim insflg As String
        Try
            Dim ins As Integer = 0
            Dim query As String = String.Empty

            query = "  insert into hrps.t_emp_excluded (ee_year, ee_cl, ee_pno, ee_email_id, ee_equiv_level, ee_emp_flag, ee_comp_code, ee_created_by, ee_created_dt) "
            query += " values ('" & year & "', '" & cycle & "','" & pno & "','" & email & "','" & eqv & "','','" & comp & "',replace('" & Session("USER_ID") & "','OSJ'),trunc(sysdate))"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim comnd As New OracleCommand(query, conHrps)
            ins = comnd.ExecuteNonQuery()

            If ins = 1 Then
                insflg = "Y"

                Return insflg
            End If

        Catch ex As Exception

            insflg = ex.Message.ToString
            Return insflg


        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Function
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try

            Dim insflag As String = String.Empty

            For i As Integer = 0 To GvExclude.Rows.Count - 1

                Dim chk = CType(GvExclude.Rows(i).FindControl("chksel"), CheckBox)

                If chk.Checked = True Then

                    Dim year1 = CType(GvExclude.Rows(i).FindControl("lblYear"), Label).Text
                    Dim cycle1 = CType(GvExclude.Rows(i).FindControl("lblCycle"), Label).Text
                    Dim pno1 = CType(GvExclude.Rows(i).FindControl("lblpno"), Label).Text
                    Dim email1 = CType(GvExclude.Rows(i).FindControl("lblemail"), Label).Text
                    Dim eqv1 = CType(GvExclude.Rows(i).FindControl("lblEqvlvl"), Label).Text
                    Dim comp1 = CType(GvExclude.Rows(i).FindControl("lblComp"), Label).Text

                    insflag = SaveData(year1, cycle1, pno1, email1, eqv1, comp1)

                End If

            Next

            If insflag = "Y" Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Data Inserted Successfully")
                bindGrid()

            ElseIf (insflag.Length > 1 And insflag <> "Y") Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Error in Inserting Data !")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select check box before saving the data")
            End If


        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ddlExecHd_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlExLevel.SelectedValue = "0"
        txtBuhr.Text = ""
        txtPerno.Text = ""
        dtls.Clear()
        GvExclude.DataSource = dtls
        GvExclude.DataBind()
    End Sub
    Protected Sub ddlExLevel_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlExecHd.SelectedValue = "0"
        txtBuhr.Text = ""
        txtPerno.Text = ""
        dtls.Clear()
        GvExclude.DataSource = dtls
        GvExclude.DataBind()
    End Sub
    Protected Sub txtBuhr_TextChanged(sender As Object, e As EventArgs)
        ddlExecHd.SelectedValue = "0"
        ddlExLevel.SelectedValue = "0"
        txtPerno.Text = ""
        dtls.Clear()
        GvExclude.DataSource = dtls
        GvExclude.DataBind()
    End Sub
    Protected Sub txtPerno_TextChanged(sender As Object, e As EventArgs)
        ddlExecHd.SelectedValue = "0"
        ddlExLevel.SelectedValue = "0"
        txtBuhr.Text = ""
        dtls.Clear()
        GvExclude.DataSource = dtls
        GvExclude.DataBind()
    End Sub

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
    Public Sub GridInclude()
        Try
            Dim qryInclude As String
            Dim dtInc As New DataTable()

            qryInclude = " select distinct ema_year,ema_cycle,ema_perno,ema_ename,ema_empl_sgrade,ema_eqv_level,ema_dept_desc"
            qryInclude += " from hrps.t_emp_master_feedback360 "
            qryInclude += "  where ema_perno in (select ee_pno from t_emp_excluded)"
            qryInclude += "  and ema_comp_code='1000'"


            If ddlIncYr.SelectedValue <> "0" Then
                qryInclude += " and ema_year='" & ddlIncYr.SelectedValue & "' "
            End If

            If ddlIncCycle.SelectedValue <> "0" Then
                qryInclude += " and ema_cycle='" & ddlIncCycle.SelectedValue & "' "
            End If

            If ddlExecutive.SelectedValue <> "0" Then
                qryInclude += "  and ema_exec_head='" & ddlExecutive.SelectedValue & "' "
            End If

            If ddlIncLvl.SelectedValue <> "0" Then
                qryInclude += "  and ema_empl_sgrade='" & ddlIncLvl.SelectedValue & "' "
            End If

            If txtBUR.Text.Trim <> "" Then
                qryInclude += " and ema_bhr_pno='" & txtBUR.Text.Trim.ToString & "' "
            End If

            If txtPNo.Text.Trim <> "" Then
                qryInclude += " And ema_perno ='" & txtPNo.Text.Trim.ToString & "'"
            End If

            qryInclude += " order by 1,2,3"

            dtInc = GetData(qryInclude, conHrps)

            If CType(dtInc, DataTable) Is Nothing Then
                GvInclude.DataSource = Nothing
                GvInclude.DataBind()
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Data not found !")
                ddlExecutive.SelectedValue = "0"
                ddlIncLvl.SelectedValue = "0"
                txtBUR.Text = ""
                txtPNo.Text = ""
            ElseIf dtInc.Rows.Count > 0 Then
                GvInclude.DataSource = dtInc
                GvInclude.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnShow_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShow.Click
        If ddlExecutive.SelectedValue = "0" And ddlIncLvl.SelectedValue = "0" And txtBUR.Text.Trim = "" And txtPNo.Text.Trim = "" Then
            ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select atleast anyone option to display data")
            TabName.Value = "tabInclude"
            Exit Sub
        Else
            GridInclude()
        End If
    End Sub
    Public Sub DeleteData(perno As String, yr As String, cl As String)
        Try
            Dim qryDel As String = String.Empty

            qryDel = " delete from hrps.t_emp_excluded where ee_pno ='" & perno & "' and ee_year = '" & yr & "' and  ee_cl='" & cl & "' "

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(qryDel, conHrps)
            comnd.ExecuteNonQuery()

        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        Try
            Dim c As Boolean = False

            For i As Integer = 0 To GvInclude.Rows.Count - 1

                Dim chk = CType(GvInclude.Rows(i).FindControl("chkseln"), CheckBox)
                Dim yr = CType(GvInclude.Rows(i).FindControl("lblYr"), Label).Text
                Dim cl = CType(GvInclude.Rows(i).FindControl("lblCyle"), Label).Text
                Dim perno As String = CType(GvInclude.Rows(i).FindControl("lblperno"), Label).Text

                If chk.Checked = True Then
                    DeleteData(perno, yr, cl)
                    c = True
                End If
            Next

            If c Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected employee has been Included")
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select check box before removing the data")
                TabName.Value = "tabInclude"
                Exit Sub
            End If
            GridInclude()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ddlExecutive_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlIncLvl.SelectedValue = "0"
        txtBUR.Text = ""
        txtPNo.Text = ""
        dtls.Clear()
        GvInclude.DataSource = dtls
        GvInclude.DataBind()
        TabName.Value = "tabInclude"
    End Sub
    Protected Sub ddlIncLvl_SelectedIndexChanged(sender As Object, e As EventArgs)
        ddlExecutive.SelectedValue = "0"
        txtBUR.Text = ""
        txtPNo.Text = ""
        dtls.Clear()
        GvInclude.DataSource = dtls
        GvInclude.DataBind()
        TabName.Value = "tabInclude"
    End Sub
    Protected Sub txtBUR_TextChanged(sender As Object, e As EventArgs)
        ddlExecutive.SelectedValue = "0"
        ddlIncLvl.SelectedValue = "0"
        txtPNo.Text = ""
        dtls.Clear()
        GvInclude.DataSource = dtls
        GvInclude.DataBind()
        TabName.Value = "tabInclude"
    End Sub
    Protected Sub txtPNo_TextChanged(sender As Object, e As EventArgs)
        ddlExecutive.SelectedValue = "0"
        ddlIncLvl.SelectedValue = "0"
        txtBUR.Text = ""
        dtls.Clear()
        GvInclude.DataSource = dtls
        GvInclude.DataBind()
        TabName.Value = "tabInclude"
    End Sub
    Protected Sub btnDisAdm_Click(sender As Object, e As EventArgs) Handles btnDisAdm.Click
        bindSuperAdmin()
        TabName.Value = "tabAdmin"
    End Sub
    Public Sub bindSuperAdmin()
        Try

            Dim qrySupRole As String
            Dim dtAdm As New DataTable()

            qrySupRole = " Select distinct irc_code,irc_desc,igp_group_id "
            qrySupRole += " From hrps.t_ir_codes,hrps.t_ir_adm_grp_privilege "
            qrySupRole += " where trim(irc_desc) = igp_user_id "
            qrySupRole += " And irc_type='360LR'"
            qrySupRole += " And irc_code ='360RL3'"
            qrySupRole += " And irc_desc='" + txtSuperAdmin.Text.Trim.ToString + "'"
            qrySupRole += " And igp_group_id='360FEEDBAC'"
            qrySupRole += " And igp_module_id='FB'"
            qrySupRole += " And igp_status ='A'"

            dtAdm = GetData(qrySupRole, conHrps)

            If CType(dtAdm, DataTable) Is Nothing Then
                gvSuperAdm.DataSource = Nothing
                gvSuperAdm.DataBind()
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Data not found !")
                btnSave.Enabled = True
                btnDelete.Enabled = False
                txtSuperAdmin.Text = ""
            ElseIf dtAdm.Rows.Count > 0 Then
                gvSuperAdm.DataSource = dtAdm
                gvSuperAdm.DataBind()
                btnSave.Enabled = False
                btnDelete.Enabled = True
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSave_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim qryAdmin1 As String = String.Empty
            qryAdmin1 = "  insert into hrps.t_ir_codes (irc_type,irc_code,irc_start_dt,irc_end_dt,irc_desc,irc_valid_tag,irc_change_user,irc_change_date) "
            qryAdmin1 += " values('360LR','360RL3',to_date(to_char(sysdate,'dd-MM-yyyy'),'dd-MM-yyyy'),'31-Dec-9999','" + txtSuperAdmin.Text.Trim.ToString + "','Y',replace('" & Session("USER_ID") & "','OSJ'),to_date(to_char(sysdate,'dd-MM-yyyy'),'dd-MM-yyyy'))"

            Dim qryAdmin2 As String = String.Empty
            qryAdmin2 = "  insert into hrps.t_ir_adm_grp_privilege (igp_location,igp_group_id,igp_module_id,igp_user_id,igp_dept_cd,igp_mode,igp_status,igp_change_date,igp_change_user,igp_remarks) "
            qryAdmin2 += " values ('JH12','360FEEDBAC','FB','" + txtSuperAdmin.Text.Trim.ToString + "','0','W','A',trunc(sysdate),replace('" & Session("USER_ID") & "','OSJ'),'Super ADM')"

            Dim strary As String() = {qryAdmin1, qryAdmin2}
            Dim oratran As OracleTransaction
            oratran = conHrps.BeginTransaction
            For i = 0 To 1
                Try

                    Dim qryAdmin As String = strary(i).ToString
                    Dim comnd As New OracleCommand(qryAdmin, conHrps, oratran)
                    comnd.ExecuteNonQuery()

                Catch ex As Exception
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Data failed to save !")
                    oratran.Rollback()
                    TabName.Value = "tabAdmin"
                    Exit For
                End Try
            Next

            oratran.Commit()
            ShowGenericMessageModal(CommonConstants.AlertType.success, "Data inserted successfully")
            bindSuperAdmin()


        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        Try
            Dim delflag As Boolean = False

            For i As Integer = 0 To gvSuperAdm.Rows.Count - 1

                Dim chkadm = CType(gvSuperAdm.Rows(i).FindControl("chkAdmim"), CheckBox)

                If chkadm.Checked = True Then
                    Dim cdadm = CType(gvSuperAdm.Rows(i).FindControl("lblcd"), Label).Text
                    Dim descadm = CType(gvSuperAdm.Rows(i).FindControl("lbldescr"), Label).Text
                    Dim gpid As String = CType(gvSuperAdm.Rows(i).FindControl("lblID"), Label).Text
                    Try
                        DeleteSuperAdmin(cdadm, descadm, gpid)
                        delflag = True
                    Catch ex As Exception
                        ShowGenericMessageModal(CommonConstants.AlertType.error, "Data failed to remove !")
                        TabName.Value = "tabAdmin"
                        Exit For
                    End Try
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please select check box before removing the data")
                    TabName.Value = "tabAdmin"
                    Exit Sub
                End If
            Next

            If delflag Then
                ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected Super admin role has been Removed")
            End If
            bindSuperAdmin()
        Catch ex As Exception

        End Try
    End Sub
    Public Sub DeleteSuperAdmin(cdadm As String, descadm As String, gpid As String)
        Try
            Dim qryDelAdmin1 As String = String.Empty
            Dim qryDelAdmin2 As String = String.Empty
            Dim ortran1 As OracleTransaction

            qryDelAdmin1 = " delete from hrps.t_ir_codes Where irc_type ='360LR' and irc_code='" & cdadm & "' and irc_desc='" & Trim(descadm) & "' and irc_valid_tag='Y' "

            qryDelAdmin2 = " delete from hrps.t_ir_adm_grp_privilege where igp_group_id='" & gpid & "'and igp_module_id='FB' and igp_user_id='" & Trim(descadm) & "' and igp_status='A' "

            Dim qrydel As String() = {qryDelAdmin1, qryDelAdmin2}

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            ortran1 = conHrps.BeginTransaction
            For i = 0 To 1
                Try
                    Dim comnd As New OracleCommand(qrydel(i), conHrps, ortran1)
                    comnd.ExecuteNonQuery()
                Catch ex As Exception
                    ortran1.Rollback()
                    Exit For
                End Try
            Next

            ortran1.Commit()
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    '*********************************DataManagementTab**********************
    Protected Sub rblupdate_SelectedIndexChanged(sender As Object, e As EventArgs)
        If rblupdate.SelectedValue = "1" Then
            UpdateEmployeeData.Visible = True
            StepUpdataData.Visible = False
            Stepdates.Visible = False
            Stepbtn.Visible = False
            ddlDMlevel.SelectedValue = "0"
            ResetStep()
        ElseIf rblupdate.SelectedValue = "2" Then
            StepUpdataData.Visible = True
            Stepdates.Visible = False
            Stepbtn.Visible = True
            UpdateEmployeeData.Visible = False
            Clear()
        End If
        TabName.Value = "tabDataManagement"
    End Sub
    Public Sub bindEmployeeDataMgt()
        Try

            Dim qryDM As String
            Dim dtDM As New DataTable()

            qryDM = " select ema_perno,ema_ename,ema_year,ema_cycle,ema_dept_code,ema_dept_desc,ema_desgn_code,ema_desgn_desc,ema_email_id,ema_eqv_level,ema_reporting_to_pno,ema_dotted_pno "
            qryDM += " from hrps.t_emp_master_feedback360 "
            qryDM += " where ema_comp_code='1000' "

            If ddlDMYear.SelectedValue <> "0" Then
                qryDM += " and ema_year='" & ddlDMYear.SelectedValue & "' "
            End If

            If ddlDMCycle.SelectedValue <> "0" Then
                qryDM += " and ema_cycle='" & ddlDMCycle.SelectedValue & "' "
            End If

            If txtPersonalno.Text.Trim <> "" Then
                qryDM += " and ema_perno='" & txtPersonalno.Text.Trim.ToString & "' "
            End If

            dtDM = GetData(qryDM, conHrps)

            If CType(dtDM, DataTable) Is Nothing Then
                GridMstrDataMgt.DataSource = Nothing
                GridMstrDataMgt.DataBind()
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Data not found !")
                txtPersonalno.Text = ""
            ElseIf dtDM.Rows.Count > 0 Then
                GridMstrDataMgt.DataSource = dtDM
                GridMstrDataMgt.DataBind()
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If ddlDMYear.SelectedValue <> "0" And ddlDMCycle.SelectedValue <> "0" And txtPersonalno.Text.Trim <> "" Then
            bindEmployeeDataMgt()
            TabName.Value = "tabDataManagement"
        End If
    End Sub
    Protected Sub GridMstrDataMgt_RowEditing(sender As Object, e As GridViewEditEventArgs)

        GridMstrDataMgt.EditIndex = e.NewEditIndex
        Dim lbl_Dept1 As Label = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("lbl_Dept"), Label)
        Dim lbl_Desgn1 As Label = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("lbl_Desgn"), Label)
        Dim lbl_eqvlvl1 As Label = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("lbl_eqvlvl"), Label)
        Dim dddeptval As HiddenField = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("hdndpt"), HiddenField)
        Dim dddesgval As HiddenField = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("hdndesgn"), HiddenField)

        bindEmployeeDataMgt()

        Dim qryDept As String = String.Empty
        qryDept = " select distinct ema_dept_code ,ema_dept_desc from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and ema_dept_desc <>'Not found' order by 2"
        Dim dtDpt = GetData(qryDept, conHrps)

        Dim ddl_Dept1 As DropDownList = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("ddl_Dept"), DropDownList)
        ddl_Dept1.DataSource = dtDpt
        ddl_Dept1.DataValueField = "ema_dept_code"
        ddl_Dept1.DataTextField = "ema_dept_desc"
        ddl_Dept1.DataBind()
        ddl_Dept1.Items.Insert(0, New ListItem("Please Select", "0"))
        ddl_Dept1.SelectedValue = dddeptval.Value.ToString


        Dim qryDsgn As String = String.Empty
        qryDsgn = " select distinct ema_desgn_code,ema_desgn_desc from hrps.t_emp_master_feedback360 where ema_comp_code='1000' order by 2"
        Dim dtdesgn = GetData(qryDsgn, conHrps)

        Dim ddl_Desgn1 As DropDownList = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("ddl_Desgn"), DropDownList)
        ddl_Desgn1.DataSource = dtdesgn
        ddl_Desgn1.DataValueField = "ema_desgn_code"
        ddl_Desgn1.DataTextField = "ema_desgn_desc"
        ddl_Desgn1.DataBind()
        ddl_Desgn1.Items.Insert(0, New ListItem("Please Select", "0"))
        ddl_Desgn1.SelectedValue = dddesgval.Value.ToString

        Dim qryeqv As String = String.Empty
        qryeqv = "select distinct ema_eqv_level from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and ema_eqv_level <>'0' and ema_eqv_level is not null order by 1 "
        Dim dteqv = GetData(qryeqv, conHrps)

        Dim ddl_eqvlvl1 As DropDownList = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("ddl_eqvlvl"), DropDownList)
        ddl_eqvlvl1.DataSource = dteqv
        ddl_eqvlvl1.DataValueField = "ema_eqv_level"
        ddl_eqvlvl1.DataTextField = "ema_eqv_level"
        ddl_eqvlvl1.DataBind()
        ddl_eqvlvl1.Items.Insert(0, New ListItem("Please Select", "0"))
        ddl_eqvlvl1.SelectedValue = lbl_eqvlvl1.Text


        Dim txt_email1 As TextBox = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("txt_email"), TextBox)
        Dim txt_ReptPNo1 As TextBox = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("txt_ReptPNo"), TextBox)
        Dim txt_Dotted1 As TextBox = DirectCast(GridMstrDataMgt.Rows(e.NewEditIndex).FindControl("txt_Dotted"), TextBox)

    End Sub
    Protected Sub GridMstrDataMgt_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        Try

            Dim lbl_perno1 As Label = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("lbl_perno"), Label)
            Dim lbl_Yr1 As Label = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("lbl_Yr"), Label)
            Dim lbl_Cyle1 As Label = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("lbl_Cyle"), Label)
            Dim dd_Dept As DropDownList = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("ddl_Dept"), DropDownList)
            Dim dd_Desgn As DropDownList = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("ddl_Desgn"), DropDownList)
            Dim tx_email As TextBox = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("txt_email"), TextBox)
            Dim dd_eqvlvl As DropDownList = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("ddl_eqvlvl"), DropDownList)
            Dim tx_ReptPNo As TextBox = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("txt_ReptPNo"), TextBox)
            Dim tx_Dotted As TextBox = DirectCast(GridMstrDataMgt.Rows(e.RowIndex).FindControl("txt_Dotted"), TextBox)


            Dim qryUpdate As String = String.Empty
            qryUpdate = " update t_emp_master_feedback360 set ema_dept_code='" & dd_Dept.SelectedValue & "',ema_dept_desc='" & dd_Dept.SelectedItem.Text.Trim & "',ema_desgn_code='" & dd_Desgn.SelectedValue & "', "
            qryUpdate += " ema_desgn_desc='" & dd_Desgn.SelectedItem.Text.Trim & "',ema_email_id=upper('" & tx_email.Text.Trim & "'),ema_eqv_level='" & dd_eqvlvl.SelectedValue & "', "
            qryUpdate += " ema_reporting_to_pno='" & tx_ReptPNo.Text.Trim & "',ema_dotted_pno='" & tx_Dotted.Text.Trim & "' "
            qryUpdate += " where ema_year='" & lbl_Yr1.Text & "'"
            qryUpdate += " and ema_cycle='" & lbl_Cyle1.Text & "'"
            qryUpdate += " and ema_perno='" & lbl_perno1.Text & "'"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(qryUpdate, conHrps)
            comnd.ExecuteNonQuery()

            ShowGenericMessageModal(CommonConstants.AlertType.success, "Data Updated Successfully")

            GridMstrDataMgt.EditIndex = -1
            bindEmployeeDataMgt()

        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Data failed to update !")
            Exit Sub
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub GridMstrDataMgt_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        GridMstrDataMgt.EditIndex = -1
        bindEmployeeDataMgt()
    End Sub
    Protected Sub btn_clear_Click(sender As Object, e As EventArgs) Handles btn_clear.Click
        Clear()
    End Sub
    Public Sub Clear()
        rblupdate.SelectedIndex = -1
        ddlDMYear.SelectedValue = "0"
        ddlDMCycle.SelectedValue = "0"
        txtPersonalno.Text = ""
        dtls.Clear()
        GridMstrDataMgt.DataSource = dtls
        GridMstrDataMgt.DataBind()
        TabName.Value = "tabDataManagement"
    End Sub
    Protected Sub btn_view_Click(sender As Object, e As EventArgs) Handles btn_view.Click
        bindGridStep()
        TabName.Value = "tabDataManagement"
    End Sub
    Public Sub bindGridStep()
        Try
            Dim qrystep As String
            Dim dtstep As New DataTable()

            qrystep = " select ema_year,ema_cycle,ema_perno,ema_ename,ema_dept_desc,ema_empl_sgrade,ema_eqv_level,to_char(ema_step1_stdt, 'dd-MM-yyyy') ema_step1_stdt,"
            qrystep += " to_char(ema_step1_enddt, 'dd-MM-yyyy') ema_step1_enddt,to_char(ema_step2_stdt, 'dd-MM-yyyy') ema_step2_stdt,to_char(ema_step2_enddt, 'dd-MM-yyyy') ema_step2_enddt, "
            qrystep += " to_char(ema_step3_stdt, 'dd-MM-yyyy') ema_step3_stdt,to_char(ema_step3_enddt, 'dd-MM-yyyy') ema_step3_enddt "
            qrystep += " from hrps.t_emp_master_feedback360 "
            qrystep += "  where ema_comp_code ='1000'"
            qrystep += "  and ema_perno not in (select ee_pno from t_emp_excluded)"
            'qrystep += "  and ema_perno in ('150000','151629')"

            If ddlDMlevel.SelectedValue <> "0" Then
                qrystep += " And ema_empl_sgrade='" & ddlDMlevel.SelectedValue & "' "
            End If

            If ddlDMEqv.SelectedValue <> "0" Then
                qrystep += " and ema_eqv_level='" & ddlDMEqv.SelectedValue & "' "
            End If

            qrystep += " order by 1,2,3"

            dtstep = GetData(qrystep, conHrps)

            If CType(dtstep, DataTable) Is Nothing Then
                GridStep.DataSource = Nothing
                GridStep.DataBind()
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Data not found !")
                btnupdate.Enabled = False
                ddlDMlevel.SelectedValue = "0"
                ddlDMEqv.SelectedValue = "0"
                ddlStep.SelectedValue = "0"
                txtStartDt.Text = ""
                txtToDate.Text = ""
                Stepdates.Visible = False
            ElseIf dtstep.Rows.Count > 0 Then
                GridStep.DataSource = dtstep
                GridStep.DataBind()
                btnupdate.Enabled = True
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GridStep_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridStep.PageIndex = e.NewPageIndex
        bindGridStep()
    End Sub
    Protected Sub btnupdate_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnupdate.Click
        Try

            If txtStartDt.Text.Trim = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "*Select Start Date")
                Exit Sub
            End If

            If txtStartDt.Text <> "" Then
                If Date.ParseExact(txtStartDt.Text, "dd-MMM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) > Date.Now Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "*Start date should not be a future date")
                    Exit Sub
                End If
            End If

            If txtToDate.Text.Trim = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.info, "*Select End Date")
                Exit Sub
            End If


            If txtStartDt.Text <> "" And txtToDate.Text <> "" Then
                StDate = CDate(txtStartDt.Text)
                toDate = CDate(txtToDate.Text)
                If StDate > toDate Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "*Start date should be less than end date")
                    Exit Sub
                End If
            End If

            Dim qryStepUpdate As String = String.Empty
            If GridStep.Rows.Count > 0 Then
                If ddlStep.SelectedValue = "ST1" AndAlso (txtStartDt.Text <> "" And txtToDate.Text <> "") Then
                    qryStepUpdate = " update hrps.t_emp_master_feedback360 Set ema_step1_stdt=to_date('" & txtStartDt.Text & "', 'dd-Mon-yyyy'),ema_step1_enddt=to_date('" & txtToDate.Text & "', 'dd-Mon-yyyy') "
                ElseIf ddlStep.SelectedValue = "ST2" AndAlso (txtStartDt.Text <> "" And txtToDate.Text <> "") Then
                    qryStepUpdate = " update hrps.t_emp_master_feedback360 Set ema_step2_stdt=to_date('" & txtStartDt.Text & "', 'dd-Mon-yyyy'),ema_step2_enddt=to_date('" & txtToDate.Text & "', 'dd-Mon-yyyy') "
                ElseIf ddlStep.SelectedValue = "ST3" AndAlso (txtStartDt.Text <> "" And txtToDate.Text <> "") Then
                    qryStepUpdate = " update hrps.t_emp_master_feedback360 Set ema_step3_stdt=to_date('" & txtStartDt.Text & "', 'dd-Mon-yyyy'),ema_step3_enddt=to_date('" & txtToDate.Text & "', 'dd-Mon-yyyy') "
                End If
                qryStepUpdate += " where ema_comp_code ='1000' "
                qryStepUpdate += " and ema_perno not in (select ee_pno from t_emp_excluded)"
                qryStepUpdate += " and ema_empl_sgrade='" & ddlDMlevel.SelectedValue & "'"
                If ddlDMEqv.SelectedValue <> "0" Then
                    qryStepUpdate += " and ema_eqv_level='" & ddlDMEqv.SelectedValue & "'"
                End If
                'qryStepUpdate += " and ema_perno in('150000','151629')"
            End If


            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(qryStepUpdate, conHrps)
            comnd.ExecuteNonQuery()

            ShowGenericMessageModal(CommonConstants.AlertType.success, "Data Updated Successfully")
            bindGridStep()


        Catch ex As Exception
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Data failed to update !")
            Exit Sub
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub
    Protected Sub ddlStep_SelectedIndexChanged(sender As Object, e As EventArgs)
        Stepdates.Visible = True
        txtStartDt.Text = ""
        txtToDate.Text = ""
        dtls.Clear()
        GridStep.DataSource = dtls
        GridStep.DataBind()
        btnupdate.Enabled = False
    End Sub
    Protected Sub ddlDMlevel_SelectedIndexChanged(sender As Object, e As EventArgs)
        ResetStep()
    End Sub
    Public Sub ResetStep()
        ddlDMEqv.SelectedValue = "0"
        ddlStep.SelectedValue = "0"
        txtStartDt.Text = ""
        txtToDate.Text = ""
        Stepdates.Visible = False
        btnupdate.Enabled = False
        dtls.Clear()
        GridStep.DataSource = dtls
        GridStep.DataBind()
        TabName.Value = "tabDataManagement"
    End Sub
    Public Sub bindSurvey()
        Try

            Dim qrySurvey As String
            Dim dtstatus As New DataTable()

            qrySurvey = " select distinct ss_year,ss_srlno,ss_asses_pno,ss_pno,ss_name,ss_categ "
            qrySurvey += " from hrps.t_survey_status "
            qrySurvey += " where ss_type='ORG' "

            If ddlSurYr.SelectedValue <> "0" Then
                qrySurvey += " and ss_year='" & ddlSurYr.SelectedValue & "'"
            End If

            If ddlSurCycle.SelectedValue <> "0" Then
                qrySurvey += " and ss_srlno='" & ddlSurCycle.SelectedValue & "'"
            End If

            If txtSurPno.Text <> "0" Then
                qrySurvey += " and ss_asses_pno='" & txtSurPno.Text.Trim & "'"
            End If
            qrySurvey += " order by 1,2,4"

            dtstatus = GetData(qrySurvey, conHrps)

            If CType(dtstatus, DataTable) Is Nothing Then
                GridSurvey.DataSource = Nothing
                GridSurvey.DataBind()
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Data not found !")

            ElseIf dtstatus.Rows.Count > 0 Then
                GridSurvey.DataSource = dtstatus
                GridSurvey.DataBind()

            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSurDisplay_Click(sender As Object, e As EventArgs) Handles btnSurDisplay.Click
        bindSurvey()
        TabName.Value = "tabSurveyMgt"
    End Sub
    Protected Sub lnkRefresh_Click(sender As Object, e As EventArgs) Handles lnkRefresh.Click
        Response.Redirect("UploadEmpData.aspx")
    End Sub


End Class
