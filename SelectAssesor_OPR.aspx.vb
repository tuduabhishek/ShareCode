Imports System.Web.Services
Imports System.Data.OracleClient
Imports System.Data
Imports System.Diagnostics
''' <summary>
''' WI: WI368 (22 LINE COMMENTED, 13 line code)   Added by Manoj Kumar on 30-05-2021
''' **********************
''' 1. Not add in grid view of peer and subordinate record Tata steel and Non-Tata steel. (Bind variable error)
''' 2. Auto populate self record in Peers and Subordinates Gridview block
''' 3. Not Show correct message in final submit Min. and Max. category
''' 4. Not show message in case of exceed Max. manager added. (Bind variable error)
''' 5. Auto add NOPR in gridview. (Manager, Peers and Subordinates)
''' ***********************
''' WI: WI447 change in logic to not allow autopopulation after save as draft
''' created by: Avik Mukherjee
''' Created Date: 04-06-2021
''' </summary>
Partial Class SelectAssesor_OPR
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)


    Public Function SessionTimeOut() As Boolean
        If Session("USER_ID") Is Nothing Or Session("label") Is Nothing Then
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Your session has been expired. Kindly Refesh the page..")
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub loadLoggedInUserIDAndDomainIntoSession()
        Dim strUserID As String = ""
        Dim strUserDomain As String = ""
        Dim vUserFullName As String = Page.User.Identity.Name   '155710'
        Dim auth As Boolean
        Dim arrUserIDParts As String() = vUserFullName.Split("\")
        If arrUserIDParts.Length <> 1 Then
            strUserID = arrUserIDParts(1)
        End If
        'Temporary Added, Should be remove before final prd movmnt
        'Session("USER_ID") = "161804"
        strUserID = Session("USER_ID").ToString
        'If strUserID = "AXLPN3126C" Then
        'strUserID = "153815" '"148536"
        'ElseIf strUserID = "FQKPS1033Q" Then
        '    strUserID = "148536"
        'ElseIf strUserID = "CEWPR6781K" Then
        '    strUserID = "150006"
        'ElseIf strUserID = "IGQPK5672E" Then
        '    strUserID = "119046"
        '    'ElseIf strUserID = "197838" Then
        '    '    strUserID = "153815"
        'ElseIf strUserID = "162523" Then
        '    strUserID = "162523"
        'ElseIf strUserID = "197838" Then
        '    strUserID = "163522"
        'ElseIf strUserID = "148536" Then
        '    strUserID = "148536"
        'Else
        'If strUserID = "198415" Then
        '    strUserID = "910023"
        '    'Else
        '    '    strUserID = "150006"
        'End If
        'strUserID = "910005" '"153342" '"910271" '910212
        ''strUserID = "269139"
        ''strUserID = "149658"931216
        'strUserID = "127720" '"913215"
        'If strUserID = "162523" Then
        '    strUserID = "197082"
        'ElseIf strUserID = "148536" Then
        '    strUserID = "158250"
        'ElseIf strUserID = "197994" Then
        '    strUserID = "119336"
        'ElseIf strUserID = "197838" Then
        '    strUserID = "910020"
        'ElseIf strUserID = "153815" Then
        '    strUserID = "911066"
        'ElseIf strUserID = "163691" Then
        'strUserID = "150921"
        'End If

        Session("USER_DOMAIN") = strUserDomain.ToUpper()
        Session("USER_ID") = strUserID.ToUpper()

        Dim r1 As New OracleCommand()
        'r1.CommandText = "select EMA_YEAR,EMA_CYCLE from hrps.t_emp_master_feedback360 where ema_perno='" & strUserID.ToString() & "' And trunc(ema_step1_stdt)<=trunc(sysdate) And trunc(ema_step1_enddt)>=trunc(sysdate)"

        r1.CommandText = "Select ema_year, ema_cycle FROM hrps.t_emp_master_feedback360 WHERE ema_perno = '" & strUserID.ToString() & "' AND trunc(ema_step1_stdt) <= trunc(SYSDATE) AND trunc(ema_step1_enddt) >= trunc(SYSDATE) AND ema_year = ( SELECT irc_desc FROM t_ir_codes WHERE irc_type = '360YS' AND irc_code = '360YS' AND irc_valid_tag = 'A' )"
        Dim g1 = getRecordInDt(r1, conHrps)
        If g1.Rows.Count > 0 Then
            ViewState("FY") = g1.Rows(0)("EMA_YEAR").ToString()
            ViewState("SRLNO") = g1.Rows(0).Item("EMA_CYCLE").ToString()
            'Added by TCS on 29112022 to access FY,Cycle inside web method
            Session("FYforNamePopup") = g1.Rows(0)("EMA_YEAR").ToString()
            Session("CycleforNamePopup") = g1.Rows(0).Item("EMA_CYCLE").ToString()
            'End
        End If

        auth = getauth(strUserID)
        If auth = True Then
        Else
            'Added by TCS on 25112022, To replace error message
            'Session("errorMsg") = "You are not part of this exercise. In case of any query please be in touch with your HRBP."
            Session("errorMsg") = "This 360 degree form is not applicable to you. In case of any query please be in touch with your HRBP."
            'End
            Response.Redirect("errorpage.aspx")
            Exit Sub
        End If
        lblname.Text = GetPno(strUserID)

        If Not IsPostBack Then
            'Dim ststus = ChkAuth(strUserID)
            Session("label") = ChkAuthlabel(strUserID)
            If Session("label").Equals("I4") Or Session("label").Equals("I5") Or Session("label").Equals("I6") Then
                'spn_label.InnerText = "IL5"
                lnk_peers.Visible = False
                lnk_sub.Visible = False
                lnk_subordinates.Visible = False
                lnk_intsh.Visible = True
                nocat.InnerText = "Three"
                namecat.InnerText = "Self, Manager, Internal Stakeholder"
                catdetails.InnerText = "And verify the list Of your Stackholders"
                mgcat.InnerText = "The Manager list"
                lbl_intsh.Text = "INTERNAL STAKEHOLDER/ PEERS/ SUBORDINATES"
            ElseIf Session("label").Equals("I3") Then
                'spn_label.InnerText = "IL3"
                lnk_intsh.Visible = True
                nocat.InnerText = "Four"
                lblHeaderPeer.Text = "PEERS AND SUBORDINATES"
                namecat.InnerText = "Self, Manager, Peers And Subordinates, Internal Stakeholder"
                catdetails.InnerText = "And verify the list Of your peers ( minimum three required) And add your Stakeholders"
                mgcat.InnerText = "The Manager And Subordinate lists"
                lbl_intsh.Text = "INTERNAL STAKEHOLDER"
            ElseIf Session("label").Equals("I2") Then
                lnk_subordinates.Visible = True
                lnk_intsh.Visible = True
                nocat.InnerText = "Five"
                lblHeaderPeer.Text = "PEERS"
                namecat.InnerText = "Self, Manager, Peers, Subordinates, Internal Stakeholder"
                catdetails.InnerText = "And verify the list Of your Stackholders"
                mgcat.InnerText = "The Manager list"
                lbl_intsh.Text = "INTERNAL STAKEHOLDER"
            ElseIf Session("label").Equals("I1") Then
                lnk_subordinates.Visible = True
                lnk_intsh.Visible = False
                nocat.InnerText = "Four"
                lblHeaderPeer.Text = "PEERS"
                namecat.InnerText = "Self, Manager, Peers, Subordinates"
                catdetails.InnerText = "And verify the list Of your Stackholders"
                mgcat.InnerText = "The Manager list"

            End If

            'If ststus = False Then
            '    Response.Redirect("errorpage.aspx", True)
            'End If

            'Added by TCS on 26122022 to Hide Manager Buttons for I1 Users
            If Session("label").Equals("I1") Then
                btntatasteelopr.Visible = False
                btnnonopr.Visible = False
            Else
                btntatasteelopr.Visible = True
                btnnonopr.Visible = True
            End If
            'End
        End If


    End Sub
    Private Sub SelectAssesor_Init(sender As Object, e As EventArgs) Handles Me.Init

        loadLoggedInUserIDAndDomainIntoSession()
    End Sub

    Private Sub SelectAssesor_OPR_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                ViewState("GvPeer") = ""
                ViewState("GvManager") = ""
                ViewState("GvSubordinates") = ""
                ViewState("Gvintstholder") = ""
                ViewState("gvfinal") = ""
                ViewState("DeletePeer") = ""
                ViewState("TagPeer") = ""
                ViewState("TagManager") = ""
                ViewState("DeleteManager") = ""
                ViewState("DeleteSubordinates") = ""
                ViewState("TagSubordinates") = ""
                ViewState("Tagintstholder") = ""
                ViewState("Deleteintstholder") = ""
                Dim isvalidPerno = ValidPerno()
                Dim isvalidpage = PageValid()
                If isvalidPerno = True Then
                    'Dim isValidApprovalPage = ValidApprovalPage()
                    'If isValidApprovalPage = True Then
                    'Else
                    '    Response.Write("<center> <b><I> This website has been closed </b></I></center>")
                    '    Me.Page.Visible = False
                    '    Exit Sub
                    'End If
                Else
                    'If isvalidpage = True Then
                    'Else
                    '    Dim isvalidEmpStatus = ValidEmpStatus()
                    '    If isvalidEmpStatus = True Then

                    '    Else
                    '        Response.Write("<center> <b><I> This website has been closed </b></I></center>")
                    '        Me.Page.Visible = False
                    '        Exit Sub
                    '    End If

                    'End If
                    Response.Write("<center> <b><I> This website has been closed </b></I></center>")
                    Me.Page.Visible = False
                    Exit Sub
                End If

                Dim statussession = SessionTimeOut()
                If statussession = False Then
                    Exit Sub
                End If
                lblfinalcap.Text = GetRandomText()
                Session.Remove("codeman")


                BindGrid()


                bindFinalGrid()

                NoEachcateg()
                Dim g = ChkApprove(Session("USER_ID").ToString)

                If g.Rows(0)(0).ToString() = "RJ" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Your form has been returned with remarks: " & g.Rows(0)(1).ToString())
                    lbOrg.Visible = True
                    GvManager.Visible = True
                    btnaddtslsub.Visible = True
                    btnnontslsub.Visible = True
                    btnAddopr.Visible = True
                    div2.Visible = True
                    Div3.Visible = True
                    GvRepoties.Visible = True
                    btntatasteel.Visible = True
                    btnnontslp.Visible = True
                    rowpeer.Visible = True
                    div1.Visible = True
                    GvPeer.Visible = True
                    btnaddpeertsl.Visible = True
                    btnaddnontsl.Visible = True
                    gvSubordinates.Visible = True
                    rowSubordinates.Visible = True
                    btnAddSub.Visible = True
                    btnAddNSSub.Visible = True
                    'divtsl.Visible = True
                    'divntsl.Visible = True
                    Gvintstholder.Visible = True
                    btntatasteelopr.Visible = True
                    btnnonopr.Visible = True
                ElseIf g.Rows(0)(2).ToString() = "SU" And g.Rows(0)(0).ToString() = "" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "You have already submitted the form...!")
                    gvfinal.Columns(7).Visible = True
                    lblSubmitStatus.Text = "(Click to Review/Save/Submit)  : Not Approved"
                    lblcaptfinalmsg.Visible = False
                    txtfinalcap.Visible = False
                    lblfinalcap.Visible = False
                    btnSaveAsDraft.Visible = False
                    lbOrg.Visible = False
                    GvManager.Visible = False
                    gvSubordinates.Visible = False
                    rowSubordinates.Visible = False
                    btnAddSub.Visible = False
                    btnAddNSSub.Visible = False
                    btnaddtslsub.Visible = False
                    btnnontslsub.Visible = False
                    btnAddopr.Visible = False
                    div2.Visible = False
                    Div3.Visible = False
                    GvRepoties.Visible = False
                    btntatasteel.Visible = False
                    btnnontslp.Visible = False
                    rowpeer.Visible = False
                    div1.Visible = False
                    GvPeer.Visible = False
                    btnaddpeertsl.Visible = False
                    btnaddnontsl.Visible = False
                    divtsl.Visible = False
                    divntsl.Visible = False
                    Gvintstholder.Visible = False
                    btntatasteelopr.Visible = False
                    btnnonopr.Visible = False
                ElseIf g.Rows(0)(2).ToString() = "SU" And g.Rows(0)(0).ToString() = "AP" Then
                    ShowGenericMessageModal(CommonConstants.AlertType.success, "Your form has been approved...!")
                    gvfinal.Columns(7).Visible = True
                    lblSubmitStatus.Text = "(Click to Review/Save/Submit)  :  Approved"
                    lbOrg.Visible = False
                    GvManager.Visible = False
                    btnaddtslsub.Visible = False
                    btnnontslsub.Visible = False
                    btnAddopr.Visible = False
                    btntatasteelopr.Visible = False
                    lblcaptfinalmsg.Visible = False
                    txtfinalcap.Visible = False
                    lblfinalcap.Visible = False
                    btnSaveAsDraft.Visible = False
                    div2.Visible = False
                    Div3.Visible = False
                    GvRepoties.Visible = False
                    btntatasteel.Visible = False
                    btnnontslp.Visible = False
                    rowpeer.Visible = False
                    div1.Visible = False
                    GvPeer.Visible = False
                    btnaddpeertsl.Visible = False
                    btnaddnontsl.Visible = False
                    btnnonopr.Visible = False
                    divtsl.Visible = False
                    divntsl.Visible = False
                    gvSubordinates.Visible = False
                    Gvintstholder.Visible = False
                    btntatasteelopr.Visible = False
                    btnAddNSSub.Visible = False
                    btnAddSub.Visible = False
                End If
                ' Populate Grid
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Function ValidApprovalPage() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            ocmd.CommandText = "select IRC_CODE from t_ir_codes where irc_type ='360PG' and trunc(IRC_START_DT) <= trunc(sysdate)"
            ocmd.CommandText += "  and trunc(IRC_END_DT) >= trunc(sysdate) and IRC_VALID_TAG='A' and upper(irc_desc)=UPPER('SurveyApproval_OPR.ASPX')"
            Dim vc = getRecordInDt(ocmd, conHrps)
            If vc.Rows.Count > 0 Then
                isvalid = True
            Else
                isvalid = False
            End If
        Catch ex As Exception

        End Try
        Return isvalid
    End Function
    Public Function ValidEmpStatus() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            ocmd.CommandText = "select * from hrps.t_emp_master_feedback360 where ema_perno ='" + Session("USER_ID").ToString + "' and trunc(ema_step1_stdt) <= trunc(sysdate)"
            ocmd.CommandText += "  and trunc(ema_step1_enddt) >= trunc(sysdate) "
            Dim vc = getRecordInDt(ocmd, conHrps)
            If vc.Rows.Count > 0 Then
                isvalid = True
            Else
                isvalid = False
            End If
        Catch ex As Exception

        End Try
        Return isvalid
    End Function
    Public Function getauth(ByVal pno As String) As String
        Dim ls_sql As String = String.Empty
        Dim cmd As OracleCommand
        Dim dt As New DataTable
        Dim st As Boolean = False
        Try
            'ls_sql = "select * from hrps.t_ir_adm_grp_privilege pr where pr.igp_group_id='360ASSEESSE' and pr.igp_user_id=:pno"
            ls_sql = "select EMA_PERNO,'TSL' EMP_TYPE from hrps.t_emp_master_feedback360 where EMA_PERNO=:pno AND EMA_YEAR=:yr AND EMA_CYCLE=:cyc and ema_perno not in (select ee_pno from t_emp_excluded where EE_YEAR=:yr and EE_CL=:cyc) union SELECT sed_perno,'DPT' FROM HRPS.t_survey_emp_deputation where SED_CYCLE=:cyc and SED_YEAR=:yr and sed_perno=:pno"
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd = New OracleCommand(ls_sql, conHrps)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("pno", pno.Trim)
            cmd.Parameters.AddWithValue("yr", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("cyc", ViewState("SRLNO").ToString())

            Dim adp As OracleDataAdapter
            adp = New OracleDataAdapter(cmd)
            adp.Fill(dt)
            'dt = getRecordInDt(cmd, conHrps)
            If dt.Rows.Count > 0 Then
                ViewState("type") = dt.Rows(0)("EMP_TYPE").ToString
                st = True
            Else
                st = False
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return st
    End Function
    Public Function ChkAuthlabelbymail(pno As String) As String
        Try
            Dim chk As String = String.Empty

            Dim qry As New OracleCommand()
            qry.CommandText = "Select EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where EMA_EMAIL_ID=:EMA_EMAIL_ID  and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue(":EMA_EMAIL_ID", pno.ToString())
            qry.Parameters.AddWithValue(":EMA_YEAR", ViewState("FY").ToString())
            qry.Parameters.AddWithValue(":EMA_CYCLE", ViewState("SRLNO").ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chk = dt.Rows(0).Item("EMA_EQV_LEVEL")
            Else
                chk = String.Empty
            End If
            Return chk
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
            ' MsgBox("errorrr")
        End Try
    End Function
    Public Function ChkAuthlabel(pno As String) As String
        Try
            Dim chk As String = String.Empty
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim qry1 As New OracleCommand()
            'qry1.CommandText = "select SS_IL from t_assesse_IL  where SS_ASSESS_PNO=:ema_perno and SS_STATUS='A'"
            qry1.CommandText = "select EMA_EQV_LEVEL from hrps.t_emp_master_feedback360  where EMA_PERNO=:ema_perno and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
            qry1.Connection = conHrps
            qry1.Parameters.Clear()
            qry1.Parameters.AddWithValue("ema_perno", pno.ToString())
            qry1.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString())
            qry1.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString())
            Dim daIL As New OracleDataAdapter(qry1)
            Dim dtIL As New DataTable()
            daIL.Fill(dtIL)
            If dtIL.Rows.Count > 0 Then
                chk = dtIL.Rows(0).Item("EMA_EQV_LEVEL")
            Else
                chk = String.Empty
            End If
            Return chk
        Catch ex As Exception
            'ShowGenericMessageModal(CommonConstants.AlertType.error, ex.Message)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function
    Public Function ChkAuth(pno As String) As Boolean
        Try
            Dim chk As Boolean = False
            Dim qry As New OracleCommand()
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            qry.CommandText = "select * from hrps.t_emp_master_feedback360  where ema_perno=:ema_perno and EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6') and EMA_YEAR:EMA_YEAR AND EMA_COMP_CODE='1000' and EMA_CYCLE:EMA_CYCLE"
            'End by Manoj Kumar on 31-05-2021
            qry.Connection = conHrps
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("ema_perno", pno.ToString())
            qry.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString())
            qry.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString())
            Dim da As New OracleDataAdapter(qry)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                chk = True
            Else
                chk = False
            End If
            Return chk
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function
    Public Function GetPno(pernr As String) As String
        Dim perno As String = ""
        Try

            Dim cm As New OracleCommand()
            cm.CommandText = "  select EMA_ENAME from hrps.t_emp_master_feedback360  where EMA_EQV_LEVEL in('I1','I2','I3','I4','I5','I6') and EMA_YEAR=:yr and ema_perno=:ema_perno AND EMA_CYCLE=:cyc"

            cm.Connection = conHrps
            cm.Parameters.Clear()
            cm.Parameters.AddWithValue("yr", ViewState("FY").ToString())
            cm.Parameters.AddWithValue("cyc", ViewState("SRLNO").ToString())
            cm.Parameters.AddWithValue("ema_perno", pernr.ToUpper().ToString())
            Dim da As New OracleDataAdapter(cm)
            Dim d As New DataTable()
            da.Fill(d)
            If d.Rows.Count > 0 Then
                perno = d.Rows(0)("EMA_ENAME").ToString()
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

        Return perno
    End Function

    Public Function GetPno() As DataTable
        Try
            Dim cm As New OracleCommand()
            cm.CommandText = " select IRC_DESC,ema_ename from t_ir_codes,hrps.t_emp_master_feedback360 where irc_type='360PN' and trim(irc_desc) = ema_perno"
            Dim d = getRecordInDt(cm, conHrps)
            Return d
        Catch ex As Exception

        End Try

    End Function
    Protected Sub GvManager_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Added by TCS on 29092023, to execute the code only for row not for header
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim chk = CType(e.Row.FindControl("chkseldmanagr"), CheckBox)
                Dim perno = CType(e.Row.FindControl("lblpno"), Label)
                If CType(ViewState("GvManager"), DataTable).Rows.Count > 0 Then
                    chk.Checked = True
                End If
                'Added by TCS on 17112022 (To check if Managers auto populated, If populated disable remove selection)
                If isManagerAutoPopulated(perno.Text.Trim) Then
                    chk.Checked = True
                    chk.Enabled = False
                End If
                'End
                'Disable by TCS on 20112023, Remove this facility
                'Added by TCS on 23122022 (Disable Manager for I1 Only)
                'If Session("label").Equals("I1") Then
                '    chk.Checked = True
                '    chk.Enabled = False
                'End If
                'End
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub
    Public Sub GetSelected(dt As DataTable, categ As String, srlno As String)
        Try


            For o = 0 To dt.Rows.Count - 1
                Dim strself = New OracleCommand()
                strself.CommandText = "select * from t_survey_status  where SS_PNO =:SS_PNO and SS_ASSES_PNO=:SS_ASSES_PNO  and SS_YEAR=:fy and SS_CATEG =:SS_CATEG and SS_SRLNO=:SS_SRLNO"
                strself.Connection = conHrps
                strself.Parameters.Clear()
                'Added by TCS on 29092023, to store collaborator auto populated data in Internal Stake Holder
                If categ = "INTSH" Then
                    strself.Parameters.AddWithValue("SS_PNO", dt.Rows(o)("ss_pno").ToString())
                Else
                    strself.Parameters.AddWithValue("SS_PNO", dt.Rows(o)("ema_perno").ToString())
                End If
                'End
                strself.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID"))
                strself.Parameters.AddWithValue("fy", ViewState("FY").ToString())
                strself.Parameters.AddWithValue("SS_CATEG", categ.ToString())
                strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                Dim da As New OracleDataAdapter(strself)
                Dim dt1 As New DataTable
                da.Fill(dt1)
                'Dim dt1 = getRecordInDt(strself, conHrps)
                If dt1.Rows.Count > 0 Then
                Else
                    'Added by TCS on 29092023, to store collaborator auto populated data in Internal Stake Holder
                    If categ = "INTSH" Then
                        SaveData(categ, dt.Rows(o)("ss_pno").ToString(), dt.Rows(o)("ss_name").ToString(), dt.Rows(o)("SS_DESG").ToString(), dt.Rows(o)("SS_DEPT").ToString(), dt.Rows(o)("SS_EMAIL").ToString(), dt.Rows(o)("SS_LEVEL").ToString(), "ORG", "SE")
                    Else
                        SaveData(categ, dt.Rows(o)("ema_perno").ToString(), dt.Rows(o)("ema_ename").ToString(), dt.Rows(o)("EMA_DESGN_DESC").ToString(), dt.Rows(o)("EMA_DEPT_DESC").ToString(), dt.Rows(o)("EMA_EMAIL_ID").ToString(), dt.Rows(o)("EMA_EMPL_PGRADE").ToString(), "ORG", "SE")
                    End If
                    'End
                End If
            Next

            'If categ = "Self" Then
            '    Dim strself = New OracleCommand()
            '    strself.CommandText = "select * from t_survey_status  where  SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG='Self'  and SS_YEAR=:fy and SS_SRLNO=:SS_SRLNO"
            '    strself.Connection = conHrps
            '    strself.Parameters.Clear()
            '    strself.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID"))
            '    strself.Parameters.AddWithValue("fy", ViewState("FY").ToString())
            '    strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            '    Dim da As New OracleDataAdapter(strself)
            '    Dim dt1 As New DataTable
            '    da.Fill(dt1)
            '    'Dim dt1 = getRecordInDt(strself, conHrps)
            '    If dt1.Rows.Count > 0 Then
            '    Else
            '        strself = New OracleCommand()
            '        strself.CommandText = "select ema_perno,ema_ename,EMA_EMPL_SGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
            '        strself.Connection = conHrps
            '        strself.Parameters.Clear()
            '        strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
            '        strself.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString())
            '        strself.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString())
            '        Dim g = getDataInDt(strself)
            '        SaveData(categ, g.Rows(0)("ema_perno").ToString(), g.Rows(0)("ema_ename").ToString(), g.Rows(0)("EMA_DESGN_DESC").ToString(), g.Rows(0)("EMA_DEPT_DESC").ToString(), g.Rows(0)("EMA_EMAIL_ID").ToString(), g.Rows(0)("EMA_EMPL_SGRADE").ToString(), "ORG", "SE")
            '    End If
            'End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
            '    MsgBox(ex.ToString)
        End Try
    End Sub
    Public Function getDataInDt(ByVal cmd1 As OracleCommand) As DataTable
        Dim dt As New DataTable()
        Try
            'If cn.State = ConnectionState.Closed Then
            '    cn.Open()
            'End If
            Dim da As New OracleDataAdapter(cmd1)
            da.Fill(dt)

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            'If cn.State = ConnectionState.Open Then
            '    cn.Close()
            'End If
        End Try
        Return dt
    End Function


    Public Sub BindGrid()

        Try
            'Added by TCS on 29092023, To auto populate Internal Stake Holder from Collaborator Table (Phase 1 Change)
            Dim dtPeertoCheck As New DataTable
            Dim dtSubordinatetoCheck As New DataTable
            'End
            Dim grade As String = String.Empty
            If Session("label").Equals("I3") Then
                grade = "'IL3'"
            ElseIf Session("label").Equals("I4") Then
                grade = "IL4"
            ElseIf Session("label").Equals("I5") Then
                grade = "IL5"
            ElseIf Session("label").Equals("I6") Then
                grade = "IL6"
            ElseIf Session("label").Equals("I2") Then
                grade = "IL2"
            ElseIf Session("label").Equals("I1") Then
                grade = "IL1"
            End If
            ''''''' Displaying Manager
            Dim strself As New OracleCommand()
            Dim cmd As New OracleCommand()
            ''''WI447: add code to check if auto population allowed or not. Created By: Avik Mukherjee, Date: 04-06-2021''''
            cmd.CommandText = "select nvl(SS_WFL_STATUS,'NA') SS_WFL_STATUS from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "' and ss_year ='" & ViewState("FY").ToString() & "' and SS_CATEG='MANGR'"
            '''WI447: End of code''''''''''''''''''
            cmd.Connection = conHrps
            'cmd.Parameters.Clear()
            Dim dtchkval As DataTable = getRecordInDt(cmd, conHrps)
            If dtchkval.Rows.Count = 0 Then
                If ViewState("type") = "TSL" Then
                    strself.CommandText += " select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL  "
                    ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
                    If Session("label").Equals("I2") Then
                        'Modified by TCS on 05-11-2023, Change the query to remove EMA_PERS_EXEC_PNO from Manager list for IL2
                        'strself.CommandText += " from hrps.t_emp_master_feedback360 a,hrps.t_emp_master_feedback360 b where b.ema_perno in (a.EMA_REPORTING_TO_PNO,a.ema_dotted_pno,a.EMA_PERS_EXEC_PNO) and a.ema_perno=:ema_perno and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle AND a.EMA_YEAR=:SS_YEAR AND a.EMA_CYCLE =:SS_SRLNO"
                        strself.CommandText += " from hrps.t_emp_master_feedback360 a,hrps.t_emp_master_feedback360 b where b.ema_perno in (a.EMA_REPORTING_TO_PNO,a.ema_dotted_pno) and a.ema_perno=:ema_perno and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle AND a.EMA_YEAR=:SS_YEAR AND a.EMA_CYCLE =:SS_SRLNO"
                        'Added by TCS on 22112022 to remove dotted number if there are three managers coming
                        Dim count = managerAutoPopulatedCount(strself.CommandText)
                        If count = 3 Then
                            strself.CommandText = strself.CommandText.Replace("(a.EMA_REPORTING_TO_PNO,a.ema_dotted_pno,a.EMA_PERS_EXEC_PNO)", "(a.EMA_REPORTING_TO_PNO,a.EMA_PERS_EXEC_PNO)")
                        End If
                        'End
                    Else
                        'strself.CommandText += " from hrps.t_emp_master_feedback360 a,hrps.t_emp_master_feedback360 b where b.ema_perno in (a.EMA_REPORTING_TO_PNO,a.ema_dotted_pno) and a.ema_perno=:ema_perno  and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle AND a.EMA_YEAR=:SS_YEAR AND a.EMA_CYCLE =:SS_SRLNO"

                        strself.CommandText += " FROM hrps.t_emp_master_feedback360 a JOIN hrps.t_emp_master_feedback360 b ON (b.ema_perno = (case when a.ema_reporting_to_pno='119628' then '148013' else a.ema_reporting_to_pno end) OR b.ema_perno = a.EMA_DOTTED_PNO OR b.ema_perno = (SELECT c.EMA_REPORTING_TO_PNO FROM hrps.t_emp_master_feedback360 c WHERE c.ema_perno = a.EMA_REPORTING_TO_PNO AND c.EMA_YEAR = a.EMA_YEAR AND c.EMA_CYCLE = a.EMA_CYCLE) OR (a.EMA_DOTTED_PNO IS NULL AND b.ema_perno = (SELECT d.EMA_REPORTING_TO_PNO FROM hrps.t_emp_master_feedback360 d WHERE d.ema_perno = (SELECT c.EMA_REPORTING_TO_PNO FROM hrps.t_emp_master_feedback360 c WHERE c.ema_perno = a.EMA_REPORTING_TO_PNO AND c.EMA_YEAR = a.EMA_YEAR AND c.EMA_CYCLE = a.EMA_CYCLE) AND d.EMA_YEAR = a.EMA_YEAR AND d.EMA_CYCLE = a.EMA_CYCLE))) WHERE a.ema_perno = :ema_perno AND a.EMA_YEAR = :SS_YEAR AND a.EMA_CYCLE = :SS_SRLNO AND a.ema_year = b.ema_year AND a.ema_cycle = b.ema_cycle AND CASE WHEN a.EMA_EQV_LEVEL IN ('I4','I5','I6') THEN CASE WHEN b.EMA_EQV_LEVEL <> 'I1' THEN 1 WHEN b.ema_perno = a.EMA_REPORTING_TO_PNO THEN 1 ELSE 0 END ELSE 1 END = 1 "
                    End If
                    'WI368 add column for officer only
                    'End by Manoj Kumar on 30-05-2021
                    strself.CommandText += "  union select ss_pno ema_perno ,ss_name ema_ename,SS_LEVEL EMA_EMPL_PGRADE,SS_DESG EMA_DESGN_DESC,SS_DEPT EMA_DEPT_DESC,SS_EMAIL EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='MANGR' and SS_ASSES_PNO=:ema_perno and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"  ' Add del tag ='N' for manager case  by Manoj Kumar 25-05-2021
                Else
                    strself.CommandText += " select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL  "
                    ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
                    strself.CommandText += " from HRPS.t_survey_emp_deputation a,hrps.t_emp_master_feedback360 b where b.ema_perno=a.SED_REPORTING_TO_PNO  and a.SED_PERNO=:ema_perno AND a.SED_YEAR=:SS_YEAR AND a.SED_CYCLE =:SS_SRLNO"    'WI368 add column for officer only
                    'End by Manoj Kumar on 30-05-2021
                    strself.CommandText += "  union select ss_pno ema_perno ,ss_name ema_ename,SS_LEVEL EMA_EMPL_PGRADE,SS_DESG EMA_DESGN_DESC,SS_DEPT EMA_DEPT_DESC,SS_EMAIL EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='MANGR' and SS_ASSES_PNO=:ema_perno and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"  ' Add del tag ='N' for manager case  by Manoj Kumar 25-05-2021
                End If

            Else
                strself.CommandText = "select ss_pno ema_perno ,ss_name ema_ename,SS_LEVEL EMA_EMPL_PGRADE,SS_DESG EMA_DESGN_DESC,SS_DEPT EMA_DEPT_DESC,SS_EMAIL EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='MANGR' and SS_ASSES_PNO=:ema_perno and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"
            End If




            strself.Connection = conHrps
            strself.Parameters.Clear()
            strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
            strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim dtmgr = getDataInDt(strself)

            If dtmgr.Rows.Count > 0 Then
                GetSelected(dtmgr, "MANGR", ViewState("SRLNO").ToString())
                ViewState("GvManager") = dtmgr     'Added by Manoj Kumar 25-05-2021
                GvManager.DataSource = dtmgr
                GvManager.DataBind()
            Else
                'Added by Manoj Kumar 25-05-2021
                Dim dtManager As New DataTable

                dtManager.Columns.Add("ema_perno", GetType(String))
                dtManager.Columns.Add("ema_ename", GetType(String))
                dtManager.Columns.Add("EMA_EMPL_PGRADE", GetType(String))
                dtManager.Columns.Add("EMA_DESGN_DESC", GetType(String))
                dtManager.Columns.Add("EMA_DEPT_DESC", GetType(String))
                dtManager.Columns.Add("EMA_EMAIL_ID", GetType(String))
                dtManager.Columns.Add("SSTYPE", GetType(String))
                dtManager.Columns.Add("CATEG_SHORT", GetType(String))
                dtManager.Columns.Add("CATEG_FULL", GetType(String))
                ViewState("GvManager") = dtManager
                GvManager.DataSource = Nothing
                GvManager.DataBind()
            End If

            ''''''' Displaying Peer
            If Session("label") = "I3" Or Session("label") = "I2" Or Session("label") = "I1" Then
                strself = New OracleCommand()
                Dim cmdfetch As New OracleCommand()
                cmdfetch.CommandText = "select nvl(SS_WFL_STATUS,'NA') SS_WFL_STATUS from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "' and ss_year ='" & ViewState("FY").ToString() & "' and SS_CATEG ='PEER'"
                cmdfetch.Connection = conHrps
                'cmd.Parameters.Clear()
                Dim dtchkvalfetch As DataTable = getRecordInDt(cmdfetch, conHrps)
                If dtchkvalfetch.Rows.Count = 0 Then
                    If ViewState("type") = "TSL" Then
                        strself.CommandText += " select  ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                        ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
                        strself.CommandText += " =(select ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO ) and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"      'WI368 add column for officer only
                        'End by Manoj Kumar on 30-05-2021

                    Else
                        strself.CommandText += " select  ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                        ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
                        strself.CommandText += " =(select SED_REPORTING_TO_PNO  from HRPS.t_survey_emp_deputation where SED_PERNO=:ema_perno and SED_YEAR=:SS_YEAR and SED_CYCLE=:SS_SRLNO ) and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"      'WI368 add column for officer only
                        'End by Manoj Kumar on 30-05-2021

                    End If
                    strself.CommandText += " and ema_perno<>:ema_perno"

                    'Added by TCS on 23112022 to remove Manager from Peer list, If manager belong to same reporting perno
                    If dtmgr.Rows.Count > 0 Then
                        Dim managerList = dtmgr.AsEnumerable().[Select](Function(r) r("ema_perno").ToString())
                        Dim mangerPerno As String = String.Join("','", managerList)
                        strself.CommandText += " and ema_perno not in ('" + mangerPerno + "') "
                    End If
                    'End

                    If Session("label") = "I3" Then
                        strself.CommandText += " union Select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                        ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
                        strself.CommandText += " = :ema_perno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"   'WI368 add column for officer only
                        'End by Manoj Kumar on 30-05-2021
                    End If

                    strself.CommandText += " union Select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='PEER' and SS_ASSES_PNO=:ema_perno  and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'" ' Add del tag ='N' for manager case  by Manoj Kumar 25-05-2021
                    strself.Parameters.Clear()
                    strself.Connection = conHrps
                    strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    'strself.Parameters.AddWithValue("EMA_EQV_LEVEL", Session("label").ToString())
                    strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                Else
                    strself.CommandText = "Select ss_pno ema_perno,ss_name ema_ename,SS_LEVEL EMA_EMPL_PGRADE,SS_DESG EMA_DESGN_DESC,SS_DEPT EMA_DEPT_DESC,SS_EMAIL EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='PEER' and SS_ASSES_PNO=:ema_perno  and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"
                    strself.Connection = conHrps
                    strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    'strself.Parameters.AddWithValue("EMA_EQV_LEVEL", Session("label").ToString())
                    strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                End If





                Dim dtpeer = getDataInDt(strself)

                'Added by TCS on 29092023, Store peer value to check for Internal Stake holder validation
                dtPeertoCheck = dtpeer
                'End
                If dtpeer.Rows.Count < 3 And dtpeer.Rows.Count >= 0 Then
                    Dim str As New OracleCommand
                    str.CommandText += " Select  ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                    ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
                    If ViewState("type") = "TSL" Then
                        str.CommandText += " =(Select ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno AND ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO ) and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO and EMA_EQV_LEVEL='" & Session("label") & "'"        'WI368 add column for officer only
                    Else
                        str.CommandText += " =(select SED_REPORTING_TO_PNO  from HRPS.t_survey_emp_deputation where SED_PERNO=:ema_perno and SED_YEAR=:SS_YEAR and SED_CYCLE=:SS_SRLNO) and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO and EMA_EQV_LEVEL='" & Session("label") & "'"        'WI368 add column for officer only
                    End If

                    ' End by Manoj Kumar on 30-05-2021

                    'Start WI368 by Manoj Kumar on 30-05-2021 not add peer & subordinate. Auto populate self record
                    str.CommandText += " and ema_perno<>:ema_perno "
                    If Session("label") = "I3" Then
                        str.CommandText += " union Select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                        str.CommandText += " = :ema_perno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
                        'End by Manoj Kumar on 30-05-2021
                        str.CommandText += " and ema_perno<>:ema_perno "
                    End If

                    str.CommandText += " union select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from t_survey_status where SS_CATEG ='PEER' and SS_ASSES_PNO=:ema_perno and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO"
                    str.Connection = conHrps
                    str.Parameters.Clear()
                    str.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    str.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    Dim dtpeer1 = getDataInDt(str)



                    ViewState("GvPeer") = dtpeer1  'Added by Manoj Kumar 25-05-2021

                    GetSelected(dtpeer1, "PEER", ViewState("SRLNO").ToString())
                    GvPeer.DataSource = Nothing
                    GvPeer.DataBind()
                    GvPeer.DataSource = dtpeer1
                    GvPeer.DataBind()


                Else
                    GetSelected(dtpeer, "PEER", ViewState("SRLNO").ToString())
                    ViewState("GvPeer") = dtpeer  'Added by Manoj Kumar 25-05-2021
                    GvPeer.DataSource = Nothing
                    GvPeer.DataBind()
                    GvPeer.DataSource = dtpeer
                    GvPeer.DataBind()

                End If
            End If


            ''''''' Displaying Subordinates
            If Session("label") = "I2" Or Session("label") = "I1" Then
                strself = New OracleCommand()
                Dim cmdfetch As New OracleCommand()
                cmdfetch.CommandText = "select nvl(SS_WFL_STATUS,'NA') SS_WFL_STATUS from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "' and ss_year ='" & ViewState("FY").ToString() & "' and SS_CATEG ='ROPT'"
                cmdfetch.Connection = conHrps
                'cmd.Parameters.Clear()
                Dim dtchkvalfetch As DataTable = getRecordInDt(cmdfetch, conHrps)
                If dtchkvalfetch.Rows.Count = 0 Then

                    strself.CommandText += "Select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                    strself.CommandText += " = :ema_perno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"

                    strself.CommandText += " union Select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='ROPT' and SS_ASSES_PNO=:ema_perno  and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"
                    strself.Parameters.Clear()
                    strself.Connection = conHrps
                    strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    'strself.Parameters.AddWithValue("EMA_EQV_LEVEL", Session("label").ToString())
                    strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                Else
                    strself.CommandText = "Select ss_pno ema_perno,ss_name ema_ename,SS_LEVEL EMA_EMPL_PGRADE,SS_DESG EMA_DESGN_DESC,SS_DEPT EMA_DEPT_DESC,SS_EMAIL EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='ROPT' and SS_ASSES_PNO=:ema_perno  and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"
                    strself.Connection = conHrps
                    strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    'strself.Parameters.AddWithValue("EMA_EQV_LEVEL", Session("label").ToString())
                    strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                End If





                Dim dtSubordinates = getDataInDt(strself)

                If dtSubordinates.Rows.Count < 3 And dtSubordinates.Rows.Count >= 0 Then
                    Dim str As New OracleCommand
                    str.CommandText += "Select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                    str.CommandText += " = :ema_perno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
                    str.CommandText += " and ema_perno<>:ema_perno "

                    str.CommandText += " union select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from t_survey_status where SS_CATEG ='ROPT' and SS_ASSES_PNO=:ema_perno and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO"
                    str.Connection = conHrps
                    str.Parameters.Clear()
                    str.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    str.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    str.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    Dim dtSubordinates1 = getDataInDt(str)



                    ViewState("GvSubordinates") = dtSubordinates1  'Added by Manoj Kumar 25-05-2021

                    GetSelected(dtSubordinates1, "ROPT", ViewState("SRLNO").ToString())
                    gvSubordinates.DataSource = Nothing
                    gvSubordinates.DataBind()
                    gvSubordinates.DataSource = dtSubordinates1
                    gvSubordinates.DataBind()
                    'Added by TCS on 29092023, Store peer value to check for Internal Stake holder validation
                    dtSubordinatetoCheck = dtSubordinates1
                    'End
                Else
                    GetSelected(dtSubordinates, "ROPT", ViewState("SRLNO").ToString())
                    ViewState("GvSubordinates") = dtSubordinates  'Added by Manoj Kumar 25-05-2021
                    gvSubordinates.DataSource = Nothing
                    gvSubordinates.DataBind()
                    gvSubordinates.DataSource = dtSubordinates
                    gvSubordinates.DataBind()
                    'Added by TCS on 29092023, Store peer value to check for Internal Stake holder validation
                    dtSubordinatetoCheck = dtSubordinates
                    'End
                End If
            End If

            'Added by TCS on 29082023, Added Auto Populated Internal Stakeholder from Collaborator Table (Pahase 1 Change)
            ''''''' Displaying Internal Stake Holder
            strself = New OracleCommand()
            Dim intshfetch As New OracleCommand()
            intshfetch.CommandText = "  select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL  from t_survey_status where SS_CATEG ='INTSH' and SS_ASSES_PNO=:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"
            intshfetch.CommandText += " and SS_YEAR=:SS_YEAR "
            intshfetch.Connection = conHrps
            intshfetch.Parameters.Clear()
            intshfetch.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            intshfetch.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            intshfetch.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            Dim dtInternalStackHolderFetch = getDataInDt(intshfetch)
            If dtInternalStackHolderFetch.Rows.Count = 0 Then
                If Session("label") = "I1" Or Session("label") = "I2" Or Session("label") = "I3" Then

                    strself.CommandText = "Select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='INTSH' and SS_ASSES_PNO=:ema_perno  and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"

                    strself.Parameters.Clear()
                    strself.Connection = conHrps
                    strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())

                Else
                    'strself.CommandText = " select  ema_perno ss_pno, ema_ename ss_name,EMA_EMPL_SGRADE SS_LEVEL,EMA_DESGN_DESC SS_DESG,EMA_DEPT_DESC SS_DEPT,EMA_EMAIL_ID SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where EMA_PERNO IN (select EC_COLLABORATOR_PERNO from hrps.t_empl_collaborator where EC_PERNO = :ema_perno AND EC_YEAR = :SS_YEAR AND EC_CYCLE = :SS_SRLNO) AND  ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"

                    strself.CommandText = "SELECT ema_perno ss_pno,ema_ename ss_name,ema_eqv_level SS_LEVEL,EMA_DESGN_DESC SS_DESG,EMA_DEPT_DESC SS_DEPT,EMA_EMAIL_ID SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL FROM ( select distinct a.ema_perno ,a.ema_ename ,a.ema_eqv_level ,a.EMA_DESGN_DESC ,a.EMA_DEPT_DESC ,a.EMA_EMAIL_ID ,'ORG' ,'' ,''    from hrps.t_emp_master_feedback360  a,hrps.t_emp_master_feedback360 b where  a.ema_emp_class in ('1','2') and a.ema_reporting_to_pno=b.ema_perno and b.ema_perno= :ema_perno  and  a.ema_year= :SS_YEAR and a.ema_cycle= :SS_SRLNO and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle"

                    strself.CommandText += " UNION select distinct a.ema_perno ,a.ema_ename ,a.ema_eqv_level ,a.EMA_DESGN_DESC ,a.EMA_DEPT_DESC ,a.EMA_EMAIL_ID ,'ORG' ,'' ,'' from hrps.t_emp_master_feedback360 A WHERE A.ema_reporting_to_pno IN (SELECT B.ema_reporting_to_pno FROM HRPS.t_emp_master_feedback360 B WHERE B.EMA_PERNO= :ema_perno AND B.ema_year= :SS_YEAR AND B.ema_cycle= :SS_SRLNO) AND A.ema_year= :SS_YEAR AND A.ema_cycle= :SS_SRLNO) "
                    strself.CommandText += " WHERE 1=1 AND ema_perno<>:ema_perno"



                    If dtmgr.Rows.Count > 0 Then
                        Dim managerList = dtmgr.AsEnumerable().[Select](Function(r) r("ema_perno").ToString())
                        Dim mangerPerno As String = String.Join("','", managerList)
                        strself.CommandText += " and ema_perno not in ('" + mangerPerno + "') "
                    End If

                    If dtPeertoCheck.Rows.Count > 0 Then
                        Dim peerList = dtPeertoCheck.AsEnumerable().[Select](Function(r) r("ema_perno").ToString())
                        Dim peerPerno As String = String.Join("','", peerList)
                        strself.CommandText += " and ema_perno not in ('" + peerPerno + "') "
                    End If

                    If dtSubordinatetoCheck.Rows.Count > 0 Then
                        Dim subordinateList = dtSubordinatetoCheck.AsEnumerable().[Select](Function(r) r("ema_perno").ToString())
                        Dim subordinatePerno As String = String.Join("','", subordinateList)
                        strself.CommandText += " and ema_perno not in ('" + subordinatePerno + "') "
                    End If

                    strself.CommandText += " union Select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL from t_survey_status where SS_CATEG ='INTSH' and SS_ASSES_PNO=:ema_perno  and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"

                    strself.Parameters.Clear()
                    strself.Connection = conHrps
                    strself.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                    strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                End If
            Else
                    strself.CommandText = "  select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL  from t_survey_status where SS_CATEG ='INTSH' and SS_ASSES_PNO=:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'"
                strself.CommandText += " and SS_YEAR=:SS_YEAR "
                strself.Connection = conHrps
                strself.Parameters.Clear()
                strself.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            End If

            Dim dtstholder = getDataInDt(strself)

            If dtstholder.Rows.Count > 0 Then
                ViewState("Gvintstholder") = dtstholder
                GetSelected(dtstholder, "INTSH", ViewState("SRLNO").ToString())
                Gvintstholder.DataSource = Nothing
                Gvintstholder.DataBind()
                Gvintstholder.DataSource = dtstholder
                Gvintstholder.DataBind()

            Else
                Dim dtIntstholder As New DataTable

                dtIntstholder.Columns.Add("ss_pno", GetType(String))
                dtIntstholder.Columns.Add("ss_name", GetType(String))
                dtIntstholder.Columns.Add("SS_LEVEL", GetType(String))
                dtIntstholder.Columns.Add("SS_DESG", GetType(String))
                dtIntstholder.Columns.Add("SS_DEPT", GetType(String))
                dtIntstholder.Columns.Add("SS_EMAIL", GetType(String))
                dtIntstholder.Columns.Add("SSTYPE", GetType(String))
                dtIntstholder.Columns.Add("CATEG_SHORT", GetType(String))
                dtIntstholder.Columns.Add("CATEG_FULL", GetType(String))
                ViewState("Gvintstholder") = dtIntstholder
                Gvintstholder.DataSource = Nothing
                Gvintstholder.DataBind()

            End If
            'End

            'Commented by TCS 29092023, Added new logic above for Populate Internal Stake Holder from Collaborator table
            '''''Displaying internal stake holder
            'strself = New OracleCommand()
            ''strself.CommandText = "select ema_perno, ema_ename, EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ''strself.CommandText += " ema_perno='" & Session("USER_ID").ToString() & "' and ema_disch_dt is null  union "
            'strself.CommandText += "  select ss_pno,ss_name,SS_LEVEL,SS_DESG,SS_DEPT,SS_EMAIL,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL  from t_survey_status where SS_CATEG ='INTSH' and SS_ASSES_PNO=:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO and  SS_DEL_TAG='N'" ' Add del tag ='N' for manager case  by Manoj Kumar
            'strself.CommandText += " and SS_YEAR=:SS_YEAR "
            'strself.Connection = conHrps
            'strself.Parameters.Clear()
            'strself.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            'strself.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            'strself.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            'Dim dtstholder = getDataInDt(strself)
            ''GetSelected(dt, "Self")
            'If dtstholder.Rows.Count > 0 Then
            '    ViewState("Gvintstholder") = dtstholder
            '    Gvintstholder.DataSource = dtstholder
            '    Gvintstholder.DataBind()

            'Else
            '    Dim dtIntstholder As New DataTable

            '    dtIntstholder.Columns.Add("ss_pno", GetType(String))
            '    dtIntstholder.Columns.Add("ss_name", GetType(String))
            '    dtIntstholder.Columns.Add("SS_LEVEL", GetType(String))
            '    dtIntstholder.Columns.Add("SS_DESG", GetType(String))
            '    dtIntstholder.Columns.Add("SS_DEPT", GetType(String))
            '    dtIntstholder.Columns.Add("SS_EMAIL", GetType(String))
            '    dtIntstholder.Columns.Add("SSTYPE", GetType(String))
            '    dtIntstholder.Columns.Add("CATEG_SHORT", GetType(String))
            '    dtIntstholder.Columns.Add("CATEG_FULL", GetType(String))
            '    ViewState("Gvintstholder") = dtIntstholder
            '    Gvintstholder.DataSource = Nothing
            '    Gvintstholder.DataBind()

            'End If


        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub

    Public Sub GetSubordinates(ByVal perno As String, ByVal yearVal As String, ByVal cycle As String,
                                  ByRef SubordCount As Integer, ByRef surveyCount As Integer, ByRef RemainingAllowed As Integer)

        Dim conHrps As String = ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString

        Using con As New OracleConnection(conHrps)
            con.Open()

            'Dim sqlPeer As String = "SELECT COUNT(*) FROM hrps.t_emp_master_feedback360 A WHERE A.ema_reporting_to_pno IN (SELECT B.ema_reporting_to_pno FROM hrps.t_emp_master_feedback360 B WHERE B.EMA_PERNO = :p_perno AND B.ema_year = :p_year AND B.ema_cycle = :p_cycle) AND A.ema_year = :p_year AND A.ema_cycle = :p_cycle AND A.EMA_PERNO<> :p_perno "

            Dim sqlSubordinate As String = "select count(*)   from hrps.t_emp_master_feedback360  a,hrps.t_emp_master_feedback360 b where  a.ema_emp_class in ('1','2') and a.ema_reporting_to_pno=b.ema_perno and b.ema_perno= :pernr  and  a.ema_year= :p_year and a.ema_cycle= :p_cycle and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle  and A.EMA_PERNO<> :pernr "

            Using cmd1 As New OracleCommand(sqlSubordinate, con)
                'cmd1.Parameters.Add(New OracleParameter(":pernr", Session("USER_ID").ToString()))
                cmd1.Parameters.Add(New OracleParameter(":pernr", perno))
                cmd1.Parameters.Add(New OracleParameter(":p_year", yearVal))
                cmd1.Parameters.Add(New OracleParameter(":p_cycle", cycle))
                SubordCount = Convert.ToInt32(cmd1.ExecuteScalar())
            End Using

            'Dim sqlSurvey As String = "SELECT COUNT(*) FROM hrps.t_survey_status WHERE SS_YEAR = :p_year AND SS_CATEG = 'INTSH' AND EXISTS (SELECT 1 FROM hrps.t_emp_master_feedback360 A WHERE A.ema_reporting_to_pno IN (SELECT B.ema_reporting_to_pno FROM hrps.t_emp_master_feedback360 B WHERE B.EMA_PERNO = SS_ASSES_PNO AND B.ema_year = :p_year AND B.ema_cycle = :p_cycle) AND A.EMA_PERNO = SS_PNO  AND A.ema_year = :p_year AND A.ema_cycle = :p_cycle) AND SS_ASSES_PNO = :p_perno"
            Dim sqlSurvey As String = "SELECT COUNT(*) FROM hrps.t_survey_status WHERE SS_YEAR = :p_year AND SS_CATEG = 'INTSH' AND EXISTS (select 1 from hrps.t_emp_master_feedback360  a,hrps.t_emp_master_feedback360 b where  a.ema_emp_class in ('1','2') and a.ema_reporting_to_pno=b.ema_perno and b.ema_perno= SS_ASSES_PNO AND A.ema_perno=SS_PNO  and  a.ema_year= :p_year and a.ema_cycle= :p_cycle and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle ) AND SS_ASSES_PNO = :p_perno AND SS_SRLNO= :p_cycle "

            Using cmd2 As New OracleCommand(sqlSurvey, con)
                cmd2.Parameters.Add(New OracleParameter(":p_year", yearVal))
                cmd2.Parameters.Add(New OracleParameter(":p_cycle", cycle))
                cmd2.Parameters.Add(New OracleParameter(":p_perno", perno))
                surveyCount = Convert.ToInt32(cmd2.ExecuteScalar())
            End Using

            con.Close()
        End Using

        RemainingAllowed = SubordCount - surveyCount

    End Sub
    Public Function isSubordinate(ByVal perno As String, ByVal yearVal As String, ByVal cycle As String) As Boolean
        Dim resultCount As Integer = 0
        Dim conHrps As String = ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString

        Using con As New OracleConnection(conHrps)
            con.Open()

            'Dim sqlPeer As String = "SELECT COUNT(*) FROM hrps.t_emp_master_feedback360 A WHERE A.ema_reporting_to_pno IN (SELECT B.ema_reporting_to_pno FROM hrps.t_emp_master_feedback360 B WHERE B.EMA_PERNO = :pernr AND B.ema_year = :p_year AND B.ema_cycle = :p_cycle) and A.ema_perno= :p_perno AND A.ema_year = :p_year AND A.ema_cycle = :p_cycle"

            Dim sqlpeer As String = "select count(*)   from hrps.t_emp_master_feedback360  a,hrps.t_emp_master_feedback360 b where  a.ema_emp_class in ('1','2') and a.ema_reporting_to_pno=b.ema_perno and b.ema_perno= :pernr and  a.ema_year= :p_year and a.ema_cycle= :p_cycle and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle and a.ema_perno=:p_perno "
            Using cmd As New OracleCommand(sqlPeer, con)
                cmd.Parameters.Add(New OracleParameter(":pernr", Session("USER_ID").ToString()))
                cmd.Parameters.Add(New OracleParameter(":p_perno", perno))
                cmd.Parameters.Add(New OracleParameter(":p_year", yearVal))
                cmd.Parameters.Add(New OracleParameter(":p_cycle", cycle))

                resultCount = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using

        Return resultCount > 0
    End Function

    Public Function isSubordinate1(ByVal perno As String, ByVal yearVal As String, ByVal cycle As String) As Boolean
        Dim resultCount As Integer = 0
        Dim conHrps As String = ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString

        Using con As New OracleConnection(conHrps)
            con.Open()

            Dim sqlpeer As String = "select count(*)   from hrps.t_emp_master_feedback360  a,hrps.t_emp_master_feedback360 b where  a.ema_emp_class in ('1','2') and a.ema_reporting_to_pno=b.ema_perno and b.ema_perno= :pernr   and  a.ema_year= :p_year and a.ema_cycle= :p_cycle and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle and a.ema_perno=:p_perno "

            Using cmd As New OracleCommand(sqlpeer, con)
                cmd.Parameters.Add(New OracleParameter(":pernr", Session("USER_ID").ToString()))
                cmd.Parameters.Add(New OracleParameter(":p_perno", perno))
                cmd.Parameters.Add(New OracleParameter(":p_year", yearVal))
                cmd.Parameters.Add(New OracleParameter(":p_cycle", cycle))

                resultCount = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        End Using

        Return resultCount > 0
    End Function

    Public Sub GetSubordinates1(ByVal perno As String, ByVal yearVal As String, ByVal cycle As String,
                                  ByRef SubordCount As Integer, ByRef surveyCount As Integer, ByRef RemainingAllowed As Integer)

        Dim conHrps As String = ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString

        Using con As New OracleConnection(conHrps)
            con.Open()

            Dim sqlSubordinate As String = "select count(*)   from hrps.t_emp_master_feedback360  a,hrps.t_emp_master_feedback360 b where  a.ema_emp_class in ('1','2') and a.ema_reporting_to_pno=b.ema_perno and b.ema_perno= :pernr  and  a.ema_year= :p_year and a.ema_cycle= :p_cycle and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle  and A.EMA_PERNO<> :pernr "

            Using cmd1 As New OracleCommand(sqlSubordinate, con)
                'cmd1.Parameters.Add(New OracleParameter(":pernr", Session("USER_ID").ToString()))
                cmd1.Parameters.Add(New OracleParameter(":pernr", perno))
                cmd1.Parameters.Add(New OracleParameter(":p_year", yearVal))
                cmd1.Parameters.Add(New OracleParameter(":p_cycle", cycle))
                SubordCount = Convert.ToInt32(cmd1.ExecuteScalar())
            End Using

            Dim sqlSurvey As String = "SELECT COUNT(*) FROM hrps.t_survey_status WHERE SS_YEAR = :p_year AND SS_CATEG = 'PEER' AND EXISTS (select 1 from hrps.t_emp_master_feedback360  a,hrps.t_emp_master_feedback360 b where  a.ema_emp_class in ('1','2') and a.ema_reporting_to_pno=b.ema_perno and b.ema_perno= SS_ASSES_PNO AND A.ema_perno=SS_PNO and  a.ema_year= :p_year and a.ema_cycle= :p_cycle and a.ema_year=b.ema_year and a.ema_cycle=b.ema_cycle ) AND SS_ASSES_PNO = :p_perno AND SS_SRLNO= :p_cycle "

            Using cmd2 As New OracleCommand(sqlSurvey, con)
                cmd2.Parameters.Add(New OracleParameter(":p_year", yearVal))
                cmd2.Parameters.Add(New OracleParameter(":p_cycle", cycle))
                cmd2.Parameters.Add(New OracleParameter(":p_perno", perno))
                surveyCount = Convert.ToInt32(cmd2.ExecuteScalar())
            End Using

            con.Close()
        End Using

        RemainingAllowed = SubordCount - surveyCount

    End Sub


    Public Sub bindFinalGrid()


        Try
            GetSelf()

            Dim cmd As New OracleCommand()
            ''''WI447: check if page has been loaded . If not auto population allowed , if already loaded auto population not allowed'''''''''''
            cmd.CommandText = "select nvl(SS_WFL_STATUS,'NA') SS_WFL_STATUS from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' and SS_SRLNO='" & ViewState("SRLNO").ToString() & "' and ss_year ='" & ViewState("FY").ToString() & "'"
            ''''''WI447: End of code'''''''''''''''''''''''''''''''
            cmd.Connection = conHrps
            'cmd.Parameters.Clear()
            Dim dtchkval As DataTable = getRecordInDt(cmd, conHrps)
            If dtchkval.Rows.Count > 0 Then

                cmd.CommandText = "select SS_ID, SS_PNO ,SS_NAME,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,SS_DESG,SS_DEPT,SS_EMAIL, "
                If Session("label") = "I2" Or Session("label") = "I1" Then
                    cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholders',SS_CATEG)IRC_DESC"
                ElseIf Session("label") = "I3" Then
                    cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers and Subordinates','INTSH','Internal Stakeholders',SS_CATEG)IRC_DESC"
                ElseIf Session("label") = "I4" Or Session("label") = "I5" Or Session("label") = "I6" Then
                    cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','INTSH','Peers/Subordinates/Internal Stakeholders',SS_CATEG)IRC_DESC"
                End If

                'cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','PEER',DECODE(EMA_EQV_LEVEL,'I2','Peers','Peers and Subordinate'),'ROPT','Subordinates','INTSH',decode(EMA_EQV_LEVEL,'I2','Internal Stakeholder','I3','Internal Stakeholder','Internal Stakeholders/ Peers/ Subordinates'),SS_CATEG)IRC_DESC"

                'Commented & Added by TCS on 29112022 to show non org employee record in final grid
                'cmd.CommandText += ",'ORG' SSTYPE,'' CATEG_SHORT,decode(SS_WFL_STATUS,'2','Pending','3','Completed') SS_WFL_STATUS from hrps.t_emp_master_feedback360 , t_survey_status  where  SS_PNO=ema_perno(+) and SS_ASSES_PNO =:SS_ASSES_PNO and SS_STATUS ='SE' and ss_year=ema_year and ss_srlno=ema_cycle and SS_SRLNO=:SS_SRLNO"
                cmd.CommandText += ", SS_TYPE SSTYPE,'' CATEG_SHORT,decode(SS_WFL_STATUS,'2','Pending','3','Completed','9','Rejected') SS_WFL_STATUS from hrps.t_emp_master_feedback360 , t_survey_status  where  SS_PNO=ema_perno(+) and SS_ASSES_PNO =:SS_ASSES_PNO and SS_STATUS ='SE' and ss_year=ema_year(+) and ss_srlno=ema_cycle(+) and SS_SRLNO=:SS_SRLNO"
                'End
                cmd.CommandText += " And ss_year =:ss_year and SS_DEL_TAG='N' order by IRC_DESC"
            Else
                cmd.CommandText = "select SS_ID, SS_PNO ,SS_NAME,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,SS_DESG,SS_DEPT,SS_EMAIL, "
                If Session("label") = "I2" Or Session("label") = "I1" Then
                    cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers','ROPT','Subordinates','INTSH','Internal Stakeholders',SS_CATEG)IRC_DESC"
                ElseIf Session("label") = "I3" Then
                    cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','PEER','Peers and Subordinates','INTSH','Internal Stakeholders',SS_CATEG)IRC_DESC"
                ElseIf Session("label") = "I4" Or Session("label") = "I5" Or Session("label") = "I6" Then
                    cmd.CommandText += " decode(SS_CATEG,'MANGR','Manager/Superior','INTSH','Peers/Subordinates/Internal Stakeholders',SS_CATEG)IRC_DESC"
                End If
                'Commented & Added by TCS on 29112022 to show non org employee record in final grid
                'cmd.CommandText += ",'ORG' SSTYPE,'' CATEG_SHORT,decode(SS_WFL_STATUS,'2','Pending','3','Completed') SS_WFL_STATUS from hrps.t_emp_master_feedback360 , t_survey_status  where  SS_PNO=ema_perno(+) and SS_ASSES_PNO =:SS_ASSES_PNO and SS_STATUS ='SE' and ss_year=ema_year and ss_srlno=ema_cycle and SS_SRLNO=:SS_SRLNO"
                cmd.CommandText += ",SS_TYPE SSTYPE,'' CATEG_SHORT,decode(SS_WFL_STATUS,'2','Pending','3','Completed','9','Rejected') SS_WFL_STATUS from hrps.t_emp_master_feedback360 , t_survey_status  where  SS_PNO=ema_perno(+) and SS_ASSES_PNO =:SS_ASSES_PNO and SS_STATUS ='SE' and ss_year=ema_year(+) and ss_srlno=ema_cycle(+) and SS_SRLNO=:SS_SRLNO"
                'End
                cmd.CommandText += " And ss_year =:ss_year  and SS_WFL_STATUS in('1','2','3') and SS_DEL_TAG='N' order by IRC_DESC "
            End If


            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            cmd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            Dim f = getDataInDt(cmd)
            If f.Rows.Count > 0 Then
                ViewState("gvfinal") = f
                gvfinal.DataSource = f
                gvfinal.DataBind()
            Else
                Dim dtFinal As New DataTable

                dtFinal.Columns.Add("SS_ID", GetType(String))
                dtFinal.Columns.Add("SS_PNO", GetType(String))
                dtFinal.Columns.Add("SS_NAME", GetType(String))
                dtFinal.Columns.Add("EMA_EMPL_PGRADE", GetType(String))
                dtFinal.Columns.Add("SS_DESG", GetType(String))
                dtFinal.Columns.Add("SS_DEPT", GetType(String))
                dtFinal.Columns.Add("SS_EMAIL", GetType(String))
                dtFinal.Columns.Add("IRC_DESC", GetType(String))
                dtFinal.Columns.Add("SSTYPE", GetType(String))
                dtFinal.Columns.Add("CATEG_SHORT", GetType(String))
                dtFinal.Columns.Add("SS_WFL_STATUS", GetType(String))
                ViewState("gvfinal") = dtFinal
                gvfinal.DataSource = Nothing
                gvfinal.DataBind()
            End If

        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
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



    'Protected Sub lbOrg_Click(sender As Object, e As EventArgs)
    '    Try
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openModel", "showmodalAddSabashAwardee();", True)
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Protected Sub lbNonOrg_Click(sender As Object, e As EventArgs)
        Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "openMode1", "showmodalAddSabashAwardee1();", True)
        Catch ex As Exception

        End Try
    End Sub



    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SelectAssesor_OPR
        Try

            Dim cmd As New OracleCommand
            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select distinct ema_ename ||'('|| ema_perno ||')' EName from hrps.t_emp_master_feedback360 where (ema_perno like  :ema_perno or upper(ema_ename) like "
            ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
            cmd.CommandText += " :ema_ename) "    'WI368 add officer class
            ' End by Manoj Kumar on 30-05-2021
            'Added by TCS on 29112022 to add Year and Cycle Filter
            cmd.CommandText += " and ema_year=:ema_year and ema_cycle=:ema_cycle "
            'End
            ob.conHrps.Open()

            cmd.Connection = ob.conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", "%" & prefixText.ToUpper & "%")
            cmd.Parameters.AddWithValue("ema_ename", "%" & prefixText.ToUpper & "%")
            'Added by TCS on 29112022 to add Year and Cycle Filter
            cmd.Parameters.AddWithValue("ema_year", Convert.ToString(HttpContext.Current.Session("FYforNamePopup")))
            cmd.Parameters.AddWithValue("ema_cycle", Convert.ToString(HttpContext.Current.Session("CycleforNamePopup")))
            'End
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader
            While sdr.Read
                prefixes.Add(sdr("EName").ToString)
            End While



            Return prefixes
        Catch ex As Exception
            'MsgBox(ex.ToString())
            Return Nothing

        Finally

            ob.conHrps.Close()

        End Try

    End Function

    <System.Web.Script.Services.ScriptMethod(),
  System.Web.Services.WebMethod()>
    Public Shared Function SearchPrefixesForApprover1(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim ob As New SelectAssesor_OPR
        Try

            Dim cmd As New OracleCommand
            cmd.CommandType = Data.CommandType.Text

            cmd.CommandText = " select distinct ema_ename ||'('|| ema_perno ||')' EName from hrps.t_emp_master_feedback360 where EMA_EXEC_HEAD in (select distinct EMA_EXEC_HEAD from hrps.t_emp_master_feedback360 where ema_perno= :ema_perno1 and EMA_YEAR= :ema_year and EMA_CYCLE= :ema_cycle) and (ema_perno like  :ema_perno or upper(ema_ename) like "
            ' Start WI368  by Manoj Kumar on 30-05-2021 add emp class column for officer only)
            cmd.CommandText += " :ema_ename) "    'WI368 add officer class
            ' End by Manoj Kumar on 30-05-2021
            'Added by TCS on 29112022 to add Year and Cycle Filter
            cmd.CommandText += " and ema_year=:ema_year and ema_cycle=:ema_cycle and case WHEN (select EMA_EQV_LEVEL from hrps.t_emp_master_feedback360 where ema_year=:ema_year and ema_cycle=:ema_cycle and ema_perno= :ema_perno1) IN ('I4','I5','I6') THEN CASE WHEN EMA_EQV_LEVEL <> 'I1' THEN 1 ELSE 0 END ELSE 1 END = 1 "
            'End
            ob.conHrps.Open()

            cmd.Connection = ob.conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno1", HttpContext.Current.Session("USER_ID").ToString())
            cmd.Parameters.AddWithValue("ema_perno", "%" & prefixText.ToUpper & "%")
            cmd.Parameters.AddWithValue("ema_ename", "%" & prefixText.ToUpper & "%")
            'Added by TCS on 29112022 to add Year and Cycle Filter
            cmd.Parameters.AddWithValue("ema_year", Convert.ToString(HttpContext.Current.Session("FYforNamePopup")))
            cmd.Parameters.AddWithValue("ema_cycle", Convert.ToString(HttpContext.Current.Session("CycleforNamePopup")))
            'End
            Dim prefixes As List(Of String) = New List(Of String)
            Dim sdr As OracleDataReader = cmd.ExecuteReader
            While sdr.Read
                prefixes.Add(sdr("EName").ToString)
            End While



            Return prefixes
        Catch ex As Exception
            'MsgBox(ex.ToString())
            Return Nothing

        Finally

            ob.conHrps.Close()

        End Try

    End Function

    Protected Sub txtAddSubPno_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtAddSubPno.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " (ema_ename ||ema_perno )=:pno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
            'End by Manoj Kumar on 31-05-2021
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            strself.Parameters.Add(New OracleParameter("SS_YEAR", ViewState("FY").ToString()))
            strself.Parameters.Add(New OracleParameter("SS_SRLNO", ViewState("SRLNO").ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtSubDesignation.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtSubEmail.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtSubOrgName.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtSubEmail.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtSubOrgName.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtSubDesignation.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtAddSubPno.ToolTip = txtAddSubPno.Text
                lblpeerlevel.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

                If txtemailP.Text <> "" Then
                    txtemailP.ReadOnly = True
                Else
                    txtemailP.ReadOnly = True
                End If
                txtdesgP.ReadOnly = True
                txtorgP.ReadOnly = True
            Else

                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                reset()
                Exit Sub
            End If

        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub txtCouponNo_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoP.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " (ema_ename ||ema_perno )=:pno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
            'End by Manoj Kumar on 31-05-2021
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            strself.Parameters.Add(New OracleParameter("SS_YEAR", ViewState("FY").ToString()))
            strself.Parameters.Add(New OracleParameter("SS_SRLNO", ViewState("SRLNO").ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtdesgP.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtemailP.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtorgP.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtemailP.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtorgP.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgP.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtpnoP.ToolTip = txtpnoP.Text
                lblpeerlevel.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

                If txtemailP.Text <> "" Then
                    txtemailP.ReadOnly = True
                Else
                    txtemailP.ReadOnly = True
                End If
                txtdesgP.ReadOnly = True
                txtorgP.ReadOnly = True
            Else

                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                reset()
                Exit Sub
            End If

        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub txtPnoNoOpr_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoopr.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " (ema_ename ||ema_perno )=:pno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
            'End by Manoj Kumar
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            strself.Parameters.Add(New OracleParameter("SS_YEAR", ViewState("FY").ToString()))
            strself.Parameters.Add(New OracleParameter("SS_SRLNO", ViewState("SRLNO").ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtdesgopr.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtemailopr.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtorgopr.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtpnoopr.ToolTip = txtpnoopr.Text
                lblpeerlevel.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

                If txtemailP.Text <> "" Then
                    txtemailP.ReadOnly = True
                Else
                    txtemailP.ReadOnly = True
                End If
                txtdesgP.ReadOnly = True
                txtorgP.ReadOnly = True
            Else

                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                reset()
                Exit Sub
            End If

        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub UpdateStatus(id As String, tag As String)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            Dim query As String = String.Empty
            Dim pno = Session("USER_ID").ToString()

            If id.StartsWith("SR") Then
                query = "update t_survey_status set SS_STATUS =:SS_STATUS,SS_UPDATED_BY=:SS_UPDATED_BY,SS_UPDATED_DT=sysdate where SS_ID=:SS_ID and SS_CRT_BY =:SS_CRT_BY"
                query += "  and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO"
            Else
                query = "update t_survey_status set SS_STATUS =:SS_STATUS ,SS_UPDATED_BY=:SS_UPDATED_BY,SS_UPDATED_DT=sysdate where SS_PNO=:SS_PNO and SS_CRT_BY "
                query += " =:SS_CRT_BY and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO"
            End If


            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            If id.StartsWith("SR") Then
                comnd.Parameters.AddWithValue("SS_STATUS", tag)
                comnd.Parameters.AddWithValue("SS_ID", id)
                comnd.Parameters.AddWithValue("SS_CRT_BY", pno)
                comnd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
                comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Else
                comnd.Parameters.AddWithValue("SS_STATUS", tag)
                comnd.Parameters.AddWithValue("SS_PNO", id)
                comnd.Parameters.AddWithValue("SS_CRT_BY", pno)
                comnd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
                comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())

            End If

            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Sub ShowGenericMessageModal(ByVal alertType As CommonConstants.AlertType, ByVal Message As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType, "KeyGenericMessageModal", String.Format("showGenericMessageModal('{0}','{1}')", alertType, Message), True)
    End Sub

    Protected Sub lbview_Click(sender As Object, e As EventArgs)
        Try
            'Response.Redirect("frm1.aspx", True)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub GetSelf()
        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim c As New OracleCommand()
            c.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            c.CommandText += "ema_perno =:ema_perno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO"
            c.Connection = conHrps
            c.Parameters.Clear()
            c.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
            c.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            c.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim d = getDataInDt(c)
            If d.Rows.Count > 0 Then
                c = New OracleCommand()
                c.CommandText = " select * from t_survey_status where ss_pno=:ss_pno  and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:ss_pno and SS_SRLNO=:SS_SRLNO and SS_CATEG='Self'"
                c.Connection = conHrps
                c.Parameters.Clear()
                c.Parameters.AddWithValue("ss_pno", Session("USER_ID").ToString())
                c.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                c.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                Dim f = getDataInDt(c)
                If f.Rows.Count > 0 Then
                Else
                    SaveData("Self", d.Rows(0)("ema_perno").ToString(), d.Rows(0)("ema_ename").ToString(), d.Rows(0)("EMA_DESGN_DESC").ToString(), d.Rows(0)("EMA_DEPT_DESC").ToString(), d.Rows(0)("EMA_EMAIL_ID").ToString(), d.Rows(0)("EMA_EMPL_PGRADE").ToString(), "ORG", "SE")
                End If
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Public Function Checkmangr(year As String, assespno As String, pno As String) As String
        Dim vl As String = String.Empty
        Try
            vl = ""

            If CType(ViewState("gvfinal"), DataTable).Rows.Count > 0 Then
                Dim dtOrdg As DataTable = CType(ViewState("gvfinal"), DataTable)
                Dim DrNOrg() As DataRow
                DrNOrg = dtOrdg.Select("SS_EMAIL='" + pno + "'") ''

                For Each row1 As DataRow In DrNOrg
                    vl = row1(7).ToString
                Next
            End If
        Catch ex As Exception
        Finally
        End Try
        Return vl
    End Function
    Public Function Check(year As String, assespno As String, pno As String) As String
        Dim vl As String = String.Empty
        Try
            ''''  ''''                  Added by Manoj Kumar on 25-05-2021
            vl = ""

            If CType(ViewState("gvfinal"), DataTable).Rows.Count > 0 Then
                Dim dtOrdg As DataTable = CType(ViewState("gvfinal"), DataTable)
                Dim DrOrg() As DataRow
                DrOrg = dtOrdg.Select("SS_PNO='" + pno + "'") ', "SSTYPE"

                For Each row As DataRow In DrOrg
                    vl = row(7).ToString
                Next
            End If

            ''''  ''''                  Added by Manoj Kumar on 25-05-2021
        Catch ex As Exception
        Finally
            'If conHrps.State = ConnectionState.Open Then
            '    conHrps.Close()
            'End If
        End Try

        Return vl
    End Function
    Private Function checkautopopulate(ByVal id As String, ByVal tag As String, ByVal catg As String) As String
        Dim strself As String = String.Empty
        Dim flag As String = String.Empty
        Dim statusSession = SessionTimeOut()
        If statusSession = False Then
            Exit Function
        End If

        If conHrps.State = ConnectionState.Closed Then
            conHrps.Open()

        End If
        If catg = "MANGR" Then
            If ViewState("type") = "TSL" Then
                If Session("label").Equals("I2") Then
                    strself = " select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID  "
                    strself += " from hrps.t_emp_master_feedback360 a,hrps.t_emp_master_feedback360 b where b.ema_perno=a.EMA_DOTTED_PNO and b.ema_year=a.ema_year and b.ema_cycle=a.ema_cycle and a.ema_perno=:ema_perno AND A.EMA_YEAR=:EMA_YEAR AND A.EMA_CYCLE=:EMA_CYCLE and b.ema_perno=:EMA_REPORTING_TO_PNO "
                Else
                    strself = " select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID  "
                    strself += " from hrps.t_emp_master_feedback360 a,hrps.t_emp_master_feedback360 b where b.ema_perno=a.EMA_REPORTING_TO_PNO and b.ema_year=a.ema_year and b.ema_cycle=a.ema_cycle and a.ema_perno=:ema_perno AND A.EMA_YEAR=:EMA_YEAR AND A.EMA_CYCLE=:EMA_CYCLE and b.ema_perno=:EMA_REPORTING_TO_PNO "
                End If

            Else
                strself = " select b.ema_perno, b.ema_ename,b.EMA_EMPL_SGRADE EMA_EMPL_PGRADE,b.EMA_DESGN_DESC,b.EMA_DEPT_DESC,b.EMA_EMAIL_ID  "
                strself += " from hrps.t_survey_emp_deputation a,hrps.t_emp_master_feedback360 b where b.ema_perno=a.SED_REPORTING_TO_PNO  and b.ema_year=a.SED_YEAR and b.ema_cycle=a.SED_CYCLE and a.SED_PERNO=:ema_perno AND A.SED_YEAR=:EMA_YEAR AND A.SED_CYCLE=:EMA_CYCLE and b.ema_perno=:EMA_REPORTING_TO_PNO "
            End If


        End If
        If catg = "PEER" Then
            strself += " select  ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
            strself += " =(select ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE ) "
            strself += " and ema_perno<>:ema_perno AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE and ema_perno=:EMA_REPORTING_TO_PNO"
            strself += " union Select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
            strself += " = :ema_perno and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE and ema_perno=:EMA_REPORTING_TO_PNO"
        End If
        If catg = "ROPT" Then
            strself += "Select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
            strself += " = :ema_perno and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE and ema_perno=:EMA_REPORTING_TO_PNO"
        End If
        Dim cmd As New OracleCommand(strself, conHrps)
        cmd.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
        cmd.Parameters.AddWithValue("EMA_REPORTING_TO_PNO", id)
        cmd.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString)
        cmd.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString)
        Dim da As New OracleDataAdapter(cmd)
        Dim f As New DataTable()
        da.Fill(f)
        Dim catgory As String = String.Empty

        If f.Rows.Count > 0 Then


            flag = "Y"



        End If
        If conHrps.State = ConnectionState.Open Then
            conHrps.Close()

        End If
        Return flag
    End Function

    Public Sub SaveData(ByVal role As String, ByVal pno As String, ByVal name As String, ByVal desg As String, ByVal org As String, ByVal email As String, ByVal lvl As String, ByVal orgtype As String, ByVal status As String)

        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            Dim OrgStr As String = String.Empty
            Dim id = getRefNo()

            If pno = "" Then
                pno = id
            End If
            OrgStr = "insert into T_SURVEY_STATUS (SS_CATEG,SS_ID,SS_PNO,SS_NAME,SS_DESG,SS_DEPT,SS_EMAIL,SS_STATUS,SS_TAG,SS_CRT_BY,SS_CRT_DT, "
            OrgStr += " SS_DEL_TAG,SS_TYPE,ss_year,SS_ASSES_PNO,SS_LEVEL,SS_SRLNO,ss_wfl_status) values (:SS_CATEG,:SS_ID,:SS_PNO,:SS_NAME,:SS_DESG,:SS_DEPT,:SS_EMAIL,:SS_STATUS,"
            OrgStr += " :SS_TAG,:SS_CRT_BY,sysdate,:SS_DEL_TAG,:SS_TYPE,:ss_year,:SS_ASSES_PNO,:SS_LEVEL,:SS_SRLNO,'0')"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(OrgStr, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_CATEG", role)
            comnd.Parameters.AddWithValue("SS_ID", id)
            comnd.Parameters.AddWithValue("SS_PNO", pno)
            comnd.Parameters.AddWithValue("SS_NAME", name)
            If desg = "" Then
                comnd.Parameters.AddWithValue("SS_DESG", desg.ToString)
            Else
                comnd.Parameters.AddWithValue("SS_DESG", Replace(desg, "'", "''"))
            End If
            If org = "" Then
                comnd.Parameters.AddWithValue("SS_DEPT", org.ToString)
            Else
                comnd.Parameters.AddWithValue("SS_DEPT", Replace(org, "'", "''"))
            End If
            comnd.Parameters.AddWithValue("SS_EMAIL", email)
            comnd.Parameters.AddWithValue("SS_STATUS", status)
            comnd.Parameters.AddWithValue("SS_TAG", "N")
            comnd.Parameters.AddWithValue("SS_CRT_BY", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("SS_DEL_TAG", "N")
            comnd.Parameters.AddWithValue("SS_TYPE", orgtype)
            comnd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            comnd.Parameters.AddWithValue("SS_LEVEL", lvl)
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.ToString)

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Sub
    Public Function getRefNo() As String
        Try
            Dim strRef As String = String.Empty
            Dim refno As String = String.Empty
            strRef = " select MAX(to_number(substr(SS_ID,3,10))) SS_ID from T_SURVEY_STATUS where SS_ID like 'SR%' and SS_YEAR='" & ViewState("FY").ToString() & "'"
            Dim mycommand As OracleCommand
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            mycommand = New OracleCommand(strRef, conHrps)

            Dim result As Object = mycommand.ExecuteScalar

            If IsDBNull(result) Then
                refno = "1"
            Else
                Dim rs = result + 1
                refno = CStr(rs)
            End If

            refno = refno.PadLeft(10, "0")
            refno = "SR" + refno

            Return refno
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Function
    Public Function SimpleCrypt(ByVal Text As String) As String

        Dim strTempChar As String = "", i As Integer
        For i = 1 To Text.Length
            If Asc(Mid$(Text, i, 1)) < 128 Then
                strTempChar = CType(Asc(Mid$(Text, i, 1)) + 128, String)
            ElseIf Asc(Mid$(Text, i, 1)) > 128 Then
                strTempChar = CType(Asc(Mid$(Text, i, 1)) - 128, String)
            End If
            Mid$(Text, i, 1) = Chr(CType(strTempChar, Integer))
        Next i
        Return Text

    End Function
    Protected Sub btnAddNSSub_Click(sender As Object, e As EventArgs)
        rowSubordinates.Visible = False
        rowNTSLSubordiinates.Visible = True
    End Sub
    Protected Sub btnAddSub_Click(sender As Object, e As EventArgs)
        rowSubordinates.Visible = True
        rowNTSLSubordiinates.Visible = False
    End Sub
    Protected Sub btntatasteel_Click(sender As Object, e As EventArgs)
        rowpeer.Visible = True
        div1.Visible = False
    End Sub
    Protected Sub btnaddpeertatasteel_Click(sender As Object, e As EventArgs)
        'Added by TCS on 21112022
        Dim statmax = ChkValidationmaxstake()
        If Len(statmax) > 0 Then
            reset()
            divtsl.Visible = False
            divntsl.Visible = False
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
            Exit Sub
        End If
        'End
        divtsl.Visible = True
        divntsl.Visible = False
        txtpnoI.Focus()
    End Sub
    Protected Sub btntatasteelopr_Click(sender As Object, e As EventArgs)
        'Added by TCS on 21112022
        Dim statmax = ChkValidationmaxmgr()
        If Len(statmax) > 0 Then
            reset()
            Div18.Visible = False
            divnontsl.Visible = False
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
            Exit Sub
        End If
        'End
        Div18.Visible = True
        divnontsl.Visible = False
    End Sub
    Protected Sub btnnonopr_Click(sender As Object, e As EventArgs)
        'Added by TCS on 21112022
        Dim statmax = ChkValidationmaxmgr()
        If Len(statmax) > 0 Then
            reset()
            Div18.Visible = False
            divnontsl.Visible = False
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
            Exit Sub
        End If
        'End
        Div18.Visible = False
        divnontsl.Visible = True
        lblcaptman.Text = GetRandomText()
        Session.Remove("codeman")
    End Sub
    Private Function GetRandomText() As String
        Dim randomText As StringBuilder = New StringBuilder()

        If Session("Code") Is Nothing Then
            'Dim alphabets As String = "ABCDEFGHIJKLMNOP123456789#@*$abcdefghijklmnopqrstuvwxyz"
            Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            Dim r As Random = New Random()

            For j As Integer = 0 To 3
                randomText.Append(alphabets(r.[Next](alphabets.Length)))
            Next

            Session("Codeman") = randomText.ToString()
        End If

        Return TryCast(Session("Codeman"), String)
    End Function
    Protected Sub btnAddSubTSL_Click(sender As Object, e As EventArgs)
        Try
            Dim statmax = ChkValidationmaxsub()
            If Len(statmax) > 0 Then
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If txtAddSubPno.Text.Trim() <> "" Then
                Dim perno = txtAddSubPno.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtSubDesignation.Text.Trim(), "'", "''")
                Dim dept = Replace(txtSubOrgName.Text.Trim, "'", "''")
                Dim email = Replace(txtSubEmail.Text.Trim, "'", "''")

                Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), pno)

                If val = "" Then
                    Dim desc As String = "Subordinate"
                    CType(ViewState("GvSubordinates"), DataTable).Rows.Add(pno, name, lblpeerlevel.Text, desg, dept, email, "ORG", "ROPT", desc)   ' Added by Manoj Kumar on 25-05-2021
                    reset()
                    BindSessionSubordinates()                                                                            ' Added by Manoj Kumar on 25-05-2021
                    CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, lblpeerlevel.Text, desg, dept, email, desc, "ORG", "ROPT", "")
                    BindSessionFinalGrid()
                Else
                    reset()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                    'ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added Peers/Subordinates category...!")
                    Exit Sub
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank, Please fill...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnAddSubNTSL_Click(sender As Object, e As EventArgs)
        Try
            Dim statmax = ChkValidationmaxsub()
            If Len(statmax) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If txtAddSubName.Text.Trim() = "" Or txtSubNDes.Text.Trim() = "" Or txtSubNEmail.Text = "" Or txtSubNOrg.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If
            'If txtcaptpeer.Text.Trim = lblcaptpeer.Text.Trim Then
            'Else
            '    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Provide Appropriate Captcha!")
            '    txtcaptpeer.Text = String.Empty
            '    Exit Sub
            'End If
            If ChkMail(txtSubNEmail.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If

            Dim pno = ""
            Dim name = Replace(txtAddSubName.Text.Trim(), "'", "''")
            Dim desg = Replace(txtSubNDes.Text.Trim(), "'", "''")
            Dim dept = Replace(txtSubNOrg.Text.Trim, "'", "''")
            Dim email = Replace(txtSubNEmail.Text.Trim, "'", "''")

            Dim val = Checkmangr(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then
                CType(ViewState("GvSubordinates"), DataTable).Rows.Add(pno, name, "", desg, dept, email, "ORG", "ROPT", "Subordinates")
                reset()
                BindSessionSubordinates()


                CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, "", desg, dept, email, "Subordinates", "NORG", "ROPT", "")
                BindSessionFinalGrid()

            Else
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception
        Finally
            txtcaptpeer.Text = String.Empty
            lblcaptpeer.Text = GetRandomText()
            Session.Remove("codeman")
        End Try

    End Sub
    Protected Sub btnAddP_Click(sender As Object, e As EventArgs)
        Try
            Dim statmax = ChkValidationmaxpeer()
            If Len(statmax) > 0 Then
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If txtpnoP.Text.Trim() <> "" Then
                Dim perno = txtpnoP.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgP.Text.Trim(), "'", "''")
                Dim dept = Replace(txtorgP.Text.Trim, "'", "''")
                Dim email = Replace(txtemailP.Text.Trim, "'", "''")

                Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), pno)

                If val = "" Then
                    Dim desc As String = ""
                    If Session("label").Equals("I2") Or Session("label").Equals("I1") Then
                        desc = "Peers"
                    ElseIf Session("label").Equals("I3") Then
                        desc = "Peers and Subordinate"
                    End If
                    CType(ViewState("GvPeer"), DataTable).Rows.Add(pno, name, lblpeerlevel.Text, desg, dept, email, "ORG", "PEER", desc)   ' Added by Manoj Kumar on 25-05-2021
                    reset()
                    BindSessionPeer()                                                                            ' Added by Manoj Kumar on 25-05-2021
                    CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, lblpeerlevel.Text, desg, dept, email, desc, "ORG", "PEER", "")
                    BindSessionFinalGrid()
                Else
                    reset()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                    'ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added Peers/Subordinates category...!")
                    Exit Sub
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank, Please fill...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnaddnontsl_Click(sender As Object, e As EventArgs)
        'Added by TCS on 21112022
        Dim statmax = ChkValidationmaxstake()
        If Len(statmax) > 0 Then
            reset()
            divtsl.Visible = False
            divntsl.Visible = False
            ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
            Exit Sub
        End If
        'End
        divtsl.Visible = False
        divntsl.Visible = True
        lblcaptintsh.Text = GetRandomText()
        Session.Remove("codeman")
    End Sub
    Protected Sub btnSaveAsDraft_Click(sender As Object, e As EventArgs)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            saveAsDraft()
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try

    End Sub

    Protected Sub txtpnoI_TextChanged(sender As Object, e As EventArgs)
        Try
            Dim pno = txtpnoI.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " (ema_ename||ema_perno)=:pno and EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE "
            'End Manoj Kumar on 31-05-2021
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            strself.Parameters.Add(New OracleParameter("EMA_YEAR", ViewState("FY").ToString()))
            strself.Parameters.Add(New OracleParameter("EMA_CYCLE", ViewState("SRLNO").ToString()))
            ' strself.Parameters.Add(New OracleParameter("pno", OracleType.VarChar)).Value = pno.ToString()
            'Dim dt = getRecordInDt(strself, conHrps)
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)

            If dt.Rows.Count > 0 Then
                txtdesgI.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtemailI.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtemailI.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptI.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgI.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtpnoI.ToolTip = txtpnoI.Text
                lblinst1.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()

                If txtemailI.Text <> "" Then
                    txtemailI.ReadOnly = True
                Else
                    txtemailI.ReadOnly = False
                End If
                txtdesgI.ReadOnly = True
                txtdeptI.ReadOnly = True
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                reset()
                Exit Sub

            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub btnorgadd_Click(sender As Object, e As EventArgs)
        Try
            Dim statmax = ChkValidationmaxstake()
            If Len(statmax) > 0 Then
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If txtpnoI.Text.Trim() <> "" Then
                Dim perno = txtpnoI.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgI.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptI.Text.Trim, "'", "''")
                Dim email = Replace(txtemailI.Text.Trim, "'", "''")

                'Added by TCS on 18112022 
                If Not isINTSHSelectionValid(pno) Then
                    reset()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, perno & ", who has been identified as an internal stakeholder, has crossed the maximum limit of response as an internal stakeholder. You may wish to choose a different respondent as an Internal Stakeholder.")
                    Exit Sub
                End If
                'End

                Dim Val = Check(ViewState("FY"), Session("User_id").ToString, pno)

                If Val = "" Then
                    'SaveData("INTSH", pno, name, desg, dept, email, lblinst1.Text, "ORG", "SE")

                    CType(ViewState("Gvintstholder"), DataTable).Rows.Add(pno, name, lblinst1.Text, desg, dept, email, "ORG", "INTSH", "Internal stakeholder")   'Added by Manoj Kumar on 26-05-2021
                    reset()
                    'BindGrid()
                    'bindFinalGrid()
                    BindSessionIntsh()                                                                                                      'Added by Manoj Kumar on 26-05-2021
                    CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, lblinst1.Text, desg, dept, email, "Internal stakeholder", "ORG", "INTSH", "")    'Added by Manoj Kumar on 26-05-2021
                    BindSessionFinalGrid()                                                                                              'Added by Manoj Kumar on 26-05-2021
                Else
                    reset()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & Val & " Category...!")
                    Exit Sub
                End If


            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank, Please fill...!")
                reset()
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Protected Sub btnaddnorgI_Click(sender As Object, e As EventArgs)
        Try
            If txtnamenI.Text.Trim() = "" Or txtdeptnI.Text.Trim() = "" Or txtdesgnI.Text = "" Or txtemailnI.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If
            'If txtcaptintsh.Text.Trim = lblcaptintsh.Text.Trim Then
            'Else
            '    ShowGenericMessageModal(CommonConstants.AlertType.info, "Invalid Captcha Provided")
            '    txtcaptintsh.Text = String.Empty
            '    Exit Sub
            'End If
            Dim statmax = ChkValidationmaxstake()
            If Len(statmax) > 0 Then
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If ChkMail(txtemailnI.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If

            Dim pno = ""
            Dim name = Replace(txtnamenI.Text.Trim.Split("(")(0), "'", "''")
            Dim desg = Replace(txtdesgnI.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptnI.Text.Trim, "'", "''")
            Dim email = Replace(txtemailnI.Text.Trim, "'", "''")

            Dim val = Checkmangr(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then
                'SaveData("INTSH", pno, name, desg, dept, email, "", "NORG", "SE")          'commented by Manoj Kumar 26-05-2021
                CType(ViewState("Gvintstholder"), DataTable).Rows.Add(pno.ToString, name, "", desg, dept, email, "NORG", "INTSH", "Internal stakeholder")   'Added by Manoj Kumar on 26-05-2021
                reset()
                'BindGrid()                                                 'commented by Manoj Kumar 26-05-2021
                'bindFinalGrid()                                            'commented by Manoj Kumar 26-05-2021

                BindSessionIntsh()                                          'Added by Manoj Kumar on 26-05-2021
                CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, "", desg, dept, email, "Internal stakeholder", "NORG", "INTSH", "")    'Added by Manoj Kumar on 26-05-2021
                BindSessionFinalGrid()                                              'Added by Manoj Kumar on 26-05-2021
            Else
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            txtcaptintsh.Text = String.Empty
            lblcaptintsh.Text = GetRandomText()
            Session.Remove("codeman")
        End Try

    End Sub
    Protected Sub btnaddmanagernon_Click(sender As Object, e As EventArgs)
        Try
            If txtmgrnontslpno.Text.Trim() = "" Or txtdesgnontslpno.Text.Trim() = "" Or txtorgnamenon.Text = "" Or txtmailnontsl.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If
            Dim statmax = ChkValidationmaxmgr()
            If Len(statmax) > 0 Then
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If ChkMail(txtmailnontsl.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If
            'If txtotpman.Text.Trim = lblcaptman.Text.Trim Then
            'Else
            '    ShowGenericMessageModal(CommonConstants.AlertType.info, "Invalid Captcha Provided")
            '    txtotpman.Text = String.Empty
            '    Exit Sub
            'End If
            Dim pno = ""
            Dim name = Replace(txtmgrnontslpno.Text.Trim.Split("(")(0), "'", "''")
            Dim desg = Replace(txtdesgnontslpno.Text.Trim(), "'", "''")
            Dim dept = Replace(txtorgnamenon.Text.Trim, "'", "''")
            Dim email = Replace(txtmailnontsl.Text.Trim, "'", "''")

            Dim val = Checkmangr(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then
                'SaveData("MANGR", pno, name, desg, dept, email, "", "NORG", "SE")
                CType(ViewState("GvManager"), DataTable).Rows.Add(pno, name, "", desg, dept, email, "NORG")
                reset()
                BindSessionManager()


                CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, "", desg, dept, email, "Manager/Superior", "NORG", "MANGR", "")
                BindSessionFinalGrid()
                'BindGrid()                                 'Commented by Manoj Kumar on 26-05-2021
                'bindFinalGrid()                           'Commented by Manoj Kumar on 26-05-2021
            Else
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception
        Finally
            txtotpman.Text = String.Empty
            lblcaptman.Text = GetRandomText()
            Session.Remove("codeman")
        End Try
    End Sub
    Public Sub reset()
        Try
            txtmgrnontslpno.Text = ""
            txtdesgnontslpno.Text = ""
            txtmailnontsl.Text = ""
            txtorgnamenon.Text = ""
            txtnmpeer.Text = ""
            txtdesgpeer.Text = ""
            txtmailpeer.Text = ""
            txtdeptpeer.Text = ""
            txtpnosub.Text = ""
            txtpnoP.Text = ""
            txtdesgP.Text = ""
            txtorgP.Text = ""
            txtemailP.Text = ""
            txtnamenI.Text = ""
            txtdesgnI.Text = ""
            txtdeptnI.Text = ""
            txtemailnI.Text = ""
            txtpnoI.Text = ""
            txtdesgI.Text = ""
            txtdeptI.Text = ""
            txtemailI.Text = ""
            txtdesgsub.Text = ""
            txtmailsub.Text = ""
            txtdeptsub.Text = ""
            txtpnoopr.Text = ""
            txtdesgopr.Text = ""
            txtemailopr.Text = ""
            txtorgopr.Text = ""
            txtAddSubPno.Text = ""
            txtSubDesignation.Text = ""
            txtSubEmail.Text = ""
            txtSubOrgName.Text = ""
            txtAddSubName.Text = ""
            txtSubNDes.Text = ""
            txtSubNEmail.Text = ""
            txtSubNOrg.Text = ""
        Catch ex As Exception

        End Try
    End Sub
    Public Sub UpdateData(id As String, tag As String, deltag As String, catg As String)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If

            Dim query As String = String.Empty
            'query = "update t_survey_status set SS_STATUS ='" & tag & "' where SS_pno='" & id & "'  and SS_YEAR='" & ViewState("FY").ToString() & "'"
            ' commented by Manoj Kumar 26-06-2021
            'query = "update t_survey_status set SS_STATUS =:SS_STATUS,SS_DEL_TAG=:SS_DEL_TAG, SS_APP_DT=sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_pno=:SS_pno and SS_YEAR=:SS_YEAR "
            'query += "AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG and SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'"
            ' commented by Manoj Kumar 26-06-2021

            ' Added by Manoj Kumar 26-06-2021

            query = "delete from t_survey_status where SS_pno=:SS_pno and SS_YEAR=:SS_YEAR "
            query += "AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO"

            ' Added by Manoj Kumar 26-06-2021
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString)               ' commented by Manoj Kumar 26-06-2021
            'comnd.Parameters.AddWithValue("SS_CATEG", catg)
            comnd.Parameters.AddWithValue("SS_pno", id)
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            'comnd.Parameters.AddWithValue("SS_DEL_TAG", deltag)                ' commented by Manoj Kumar 26-06-2021
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            'comnd.Parameters.AddWithValue("SS_UPDATED_BY", Session("USER_ID").ToString())          ' commented by Manoj Kumar 26-06-2021
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Public Sub DeleteData(id As String, tag As String, deltag As String, catg As String)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If

            Dim query As String = String.Empty
            '''''WI447: re insert data in T_assesse_IL table if any transaction failure occurs: Created By: Avik Mukherjee, Date: 04-06-2021
            query = "delete from t_survey_status where SS_EMAIL=:SS_pno and SS_YEAR=:SS_YEAR "
            query += "AND SS_ASSES_PNO=:SS_ASSES_PNO and SS_CATEG=:SS_CATEG and SS_SRLNO=:SS_SRLNO"
            ''''' WI447: End of code''''''''''''
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.Parameters.AddWithValue("SS_CATEG", catg)
            comnd.Parameters.AddWithValue("SS_pno", id)
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub

    Protected Sub chkseldsel_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chkseldsel"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim email = CType(gv.FindControl("lblemail"), Label)

            Dim val = Check(ViewState("FY").ToString(), Session("User_id").ToString, email.Text)

            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "INTSH")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If

                'ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()

            Else
                Dim isSub As Boolean = isSubordinate(id.Text, ViewState("FY").ToString(), "2")
                Dim totalSub As Integer = 0
                Dim surveySub As Integer = 0
                Dim remaining As Integer = 0
                If isSub Then
                    GetSubordinates(Session("User_id").ToString(), ViewState("FY").ToString(), ViewState("SRLNO").ToString(), totalSub, surveySub, remaining)

                    If remaining >= 3 Then

                        ShowGenericMessageModal(CommonConstants.AlertType.warning, "You cannot uncheck more than 3 subordinates..")
                        Exit Sub
                    Else
                        Dim currentSelectedCount As Integer = CType(ViewState("Gvintstholder"), DataTable).Rows.Count
                        Dim uncheckedDt As DataTable = TryCast(ViewState("Deleteintstholder"), DataTable)
                        Dim uncheckedCount As Integer = If(uncheckedDt IsNot Nothing, uncheckedDt.Rows.Count, 0)
                        Dim uncheckedcount1 As Integer
                        uncheckedcount1 = remaining + uncheckedCount

                        If uncheckedCount1 >= 3 Then
                            ShowGenericMessageModal(CommonConstants.AlertType.warning, "You cannot uncheck more than 3 subordinates.")
                            chk.Checked = True
                            Exit Sub
                        End If


                    End If
                End If





                'UpdateData(id.Text, "DS", "Y", "INTSH")
                ''''Added by Manoj Kumar on 25-05-2021
                Dim dt As DataTable = CType(ViewState("Gvintstholder"), DataTable)

                Dim dtintstholder As New DataTable
                dtintstholder.Columns.Add("ema_perno", GetType(String))
                dtintstholder.Columns.Add("EMA_EMAIL_ID", GetType(String))
                dtintstholder.Rows.Add(id.Text, email.Text)
                If ViewState("Tagintstholder") = "1" Then
                    CType(ViewState("Deleteintstholder"), DataTable).Rows.Add(id.Text, email.Text)
                    ViewState("Tagintstholder") = "1"
                Else
                    ViewState("Deleteintstholder") = dtintstholder
                    ViewState("Tagintstholder") = "1"
                End If


                Dim Dr() As DataRow
                If id.Text.Trim <> "" Then
                    Dr = dt.Select("ss_pno = '" + id.Text.Trim.ToString + "'", "ss_pno") ''
                        Else
                    Dr = dt.Select("SS_EMAIL='" + email.Text.Trim + "'", "SS_EMAIL")
                End If

                For i As Integer = 0 To Dr.Length - 1
                    'If id.Text.Trim <> "" Then
                    '    UpdateData(id.Text, "DS", "Y", "INTSH")
                    'Else
                    '    DeleteData(email.Text.Trim, "DS", "Y", "INTSH")
                    'End If

                    dt.Rows.Remove(Dr(i))
                Next
                ViewState("Gvintstholder") = dt
                Gvintstholder.DataSource = ViewState("Gvintstholder")
                Gvintstholder.DataBind()


                Dim dt1 As DataTable = CType(ViewState("gvfinal"), DataTable)
                Dim Dr1() As DataRow
                If id.Text.Trim <> "" Then
                    Dr1 = dt1.Select("SS_PNO = '" + id.Text.Trim.ToString + "'", "SS_PNO") ''
                Else
                    Dr1 = dt1.Select("SS_EMAIL='" + email.Text.Trim + "'", "SS_EMAIL")
                End If

                For j As Integer = 0 To Dr1.Length - 1
                    dt1.Rows.Remove(Dr1(j))
                Next
                ViewState("gvfinal") = dt1
                gvfinal.DataSource = ViewState("gvfinal")
                gvfinal.DataBind()

                ''''Added by Manoj Kumar on 25-05-2021
                'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
                ' bindFinalGrid()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ChkApprove(pno As String) As DataTable

        Try
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim pno1 = Session("USER_ID").ToString()
            Dim qry As New OracleCommand()
            'qry.CommandText = " select SS_APP_TAG,SS_REMARKS from t_survey_status where SS_ASSES_PNO ='" & pno & "'  and SS_YEAR='" & ViewState("FY").ToString() & "' and SS_TAG ='SU' "
            qry.CommandText = " select distinct SS_APP_TAG,SS_REMARKS,ss_tag from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and SS_YEAR=:SS_YEAR  and SS_SRLNO=:SS_SRLNO"
            qry.Parameters.Clear()
            qry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString)
            qry.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            qry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            qry.Connection = conHrps
            Dim da As New OracleDataAdapter(qry)
            Dim f As New DataTable()
            da.Fill(f)

            Return f
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try

    End Function

    Public Sub NoEachcateg()
        Try
            Dim qr As New OracleCommand()
            Dim label As String = Session("label")
            Dim code As String = String.Empty
            If label.Equals("I3") Then
                code = "360V3"
            ElseIf label.Equals("I4") Then
                code = "360V4"

            ElseIf label.Equals("I5") Then
                code = "360V5"

            ElseIf label.Equals("I6") Then
                code = "360V6"
            ElseIf label.Equals("I2") Then
                code = "360V2"
            ElseIf label.Equals("I1") Then
                code = "360V1"
            End If



            'qr.CommandText = "select a.IRC_CODE,'Min '||SUBSTR(a.IRC_DESC,0,1) minmum, 'Max '||SUBSTR(a.IRC_DESC,3,2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            qr.CommandText = "select a.IRC_CODE,'Min '||REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, 'Max '||REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            qr.CommandText += "  where a.irc_type=:irc_type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' order by 1"
            qr.Connection = conHrps
            qr.Parameters.Clear()
            qr.Parameters.AddWithValue("irc_type", code.Trim)
            Dim w = getDataInDt(qr)

            If w.Rows.Count > 0 Then
                If label <> "I1" Then
                    lblinst.Text = "(" & w.Rows(0)("minmum").ToString() & " " & w.Rows(0)("maximum").ToString() & ")"
                    lblmmms.Text = "(" & w.Rows(1)("minmum").ToString() & " " & w.Rows(1)("maximum").ToString() & ")"
                    lblpeer.Text = "(" & w.Rows(2)("minmum").ToString() & " " & w.Rows(2)("maximum").ToString() & ")"
                    lblSubordinatesCriteria.Text = "(" & w.Rows(3)("minmum").ToString() & " " & w.Rows(3)("maximum").ToString() & ")"
                    lblsub.Text = "(" & w.Rows(4)("minmum").ToString() & " " & w.Rows(4)("maximum").ToString() & ")"
                Else
                    lblmmms.Text = "(" & w.Rows(0)("minmum").ToString() & " " & w.Rows(0)("maximum").ToString() & ")"
                    lblpeer.Text = "(" & w.Rows(1)("minmum").ToString() & " " & w.Rows(1)("maximum").ToString() & ")"
                    lblSubordinatesCriteria.Text = "(" & w.Rows(2)("minmum").ToString() & " " & w.Rows(2)("maximum").ToString() & ")"
                    lblsub.Text = "(" & w.Rows(3)("minmum").ToString() & " " & w.Rows(3)("maximum").ToString() & ")"
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Function ChkValidation() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty

            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            ElseIf Session("label").Equals("I1") Then
                type = "360V1"
            End If
            ' Start WI368  by Manoj Kumar on 30-05-2021 
            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,decode(b.irc_desc,'Peer','Peers And Subordinates',b.irc_desc) irc_desc from t_ir_codes a,t_ir_codes b "
            If Session("label").Equals("I2") Or Session("label").Equals("I1") Then
                cmdqry.CommandText = ""
                cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,decode(b.irc_desc,'Peer','Peers',b.irc_desc) irc_desc from t_ir_codes a,t_ir_codes b "
            End If
            'WI368  Not Show correct message in final submit Min. & Max. category
            'End by Manoj Kumar on 30-05-2021
            cmdqry.CommandText += "  where a.irc_type='" + type + "' and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            If type.Equals("360V5") Or type.Equals("360V6") Or type.Equals("360V4") Then
                cmdqry.CommandText += " and a.IRC_CODE not in('PEER','ROPT')"
            End If
            Dim dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)

                    If dtls.Rows.Count < dt.Rows(i)("minmum") Then
                        status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("minmum").ToString() & "), "
                    End If

                Next
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmax() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty

            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            ElseIf Session("label").Equals("I1") Then
                type = "360V1"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:irc_type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("irc_type", type)

            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' and SS_SRLNO=:SS_SRLNO"
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count > dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxsub() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty

            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            ElseIf Session("label").Equals("I1") Then
                type = "360V1"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:irc_type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='ROPT'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("irc_type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    cmdqry = New OracleCommand()
                    ' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    'cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N'  and SS_SRLNO=:SS_SRLNO"
                    cmdqry.Parameters.Clear()
                    cmdqry.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    Dim da As New OracleDataAdapter(cmdqry)
                    da.Fill(dtls)


                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxpeer() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty

            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            ElseIf Session("label").Equals("I1") Then
                type = "360V1"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:irc_type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='PEER'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("irc_type", type)
            Dim dt = getRecordInDt(cmdqry, conHrps)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Dim dtls3 As New DataTable()
                    dtls3 = CType(ViewState("GvPeer"), DataTable)

                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls3.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            'MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxstake() As String
        Dim status As String = ""
        Try
            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty

            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            ElseIf Session("label").Equals("I1") Then
                type = "360V1"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            cmdqry.CommandText += "  where a.irc_type=:irc_type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='INTSH'"
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("irc_type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    'cmdqry = New OracleCommand()
                    '' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    ''cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    'cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    'cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'"
                    'cmdqry.Parameters.Clear()
                    'cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    'cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    'cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    'cmdqry.Connection = conHrps
                    Dim dtls1 As New DataTable()
                    'Dim da As New OracleDataAdapter(cmdqry)
                    'da.Fill(dtls)
                    dtls1 = CType(ViewState("Gvintstholder"), DataTable)

                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls1.Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
            ' MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    Public Function ChkValidationmaxmgr() As String
        Dim status As String = ""
        Try

            Dim cmdqry As New OracleCommand()
            Dim type As String = String.Empty

            If Session("label").Equals("I5") Then
                type = "360V5"
            ElseIf Session("label").Equals("I4") Then
                type = "360V4"
            ElseIf Session("label").Equals("I3") Then
                type = "360V3"
            ElseIf Session("label").Equals("I6") Then
                type = "360V6"
            ElseIf Session("label").Equals("I2") Then
                type = "360V2"
            ElseIf Session("label").Equals("I1") Then
                type = "360V1"
            End If

            cmdqry.CommandText = "select a.IRC_CODE,REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 1) minmum, REGEXP_SUBSTR (a.IRC_DESC, '[^-]+', 1, 2) maximum,b.irc_desc from t_ir_codes a,t_ir_codes b "
            ' Start WI368  by Manoj Kumar on 30-05-2021 
            cmdqry.CommandText += "  where a.irc_type=:irc_type and a.irc_valid_tag='A' and a.irc_code=b.irc_code and b.irc_type='360RL' and b.irc_valid_tag='A' and a.IRC_CODE='MANGR'"   'WI368 Not show message in case of exceed Max. manager added. (Bind variable)
            'End by Manoj Kumar on 30-05-2021
            cmdqry.Connection = conHrps
            cmdqry.Parameters.Clear()
            cmdqry.Parameters.AddWithValue("irc_type", type)
            Dim dt = getDataInDt(cmdqry)

            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    'cmdqry = New OracleCommand()
                    '' cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO ='" & Session("USER_ID").ToString() & "' "
                    ''cmdqry.CommandText += " and upper(SS_CATEG) = '" & dt.Rows(i)("IRC_CODE").ToString().ToUpper & "' and SS_STATUS='SE' and SS_YEAR='" & ViewState("FY").ToString() & "'"
                    'cmdqry.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
                    'cmdqry.CommandText += "  and SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'"
                    'cmdqry.Parameters.Clear()
                    'cmdqry.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    'cmdqry.Parameters.AddWithValue("SS_CATEG", dt.Rows(i)("IRC_CODE").ToString().ToUpper)
                    'cmdqry.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                    'cmdqry.Connection = conHrps
                    Dim dtls As New DataTable()
                    'Dim da As New OracleDataAdapter(cmdqry)
                    'da.Fill(dtls)
                    dtls = CType(ViewState("GvManager"), DataTable)

                    If dt.Rows(i)("maximum") = "N" Then
                    Else
                        If dtls.Rows.Count >= dt.Rows(i)("maximum") Then
                            'If CType(Session("GvManager"), DataTable).Rows.Count >= dt.Rows(i)("maximum") Then
                            status += dt.Rows(i)("irc_desc").ToString() & "(" & dt.Rows(i)("maximum").ToString() & "),"
                        End If

                    End If
                Next
            End If
        Catch ex As Exception
            ' MsgBox(ex.ToString())
        End Try
        Return status.TrimEnd(",")
    End Function
    'Added by TCS on 09122022, Method SUB changes as Function to check whether code executed or not
    Public Function UpdateApprover(approver As String, pno As String) As Integer
        Dim intCount As Integer = 0
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Return intCount
                Exit Function
            End If

            Dim query As String = String.Empty
            'query = "update t_survey_status set SS_APPROVER ='" & approver & "',SS_WFL_STATUS='1' where SS_ASSES_PNO='" & pno & "'  and SS_YEAR='" & ViewState("FY").ToString() & "'"
            query = "update t_survey_status set SS_APPROVER =:SS_APPROVER,SS_WFL_STATUS='1', SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:SS_UPDATED_BY where SS_ASSES_PNO=:SS_ASSES_PNO and SS_YEAR=:SS_YEAR and SS_SRLNO=:SS_SRLNO"

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim comnd As New OracleCommand(query, conHrps)
            comnd.Parameters.Clear()
            comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            comnd.Parameters.AddWithValue("SS_APPROVER", approver)
            comnd.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            comnd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
            comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            comnd.Connection = conHrps
            intCount = comnd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
        Return intCount
    End Function
    Protected Sub chkseldsel_CheckedChanged1(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chkseldsel"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim pno = CType(gv.FindControl("lblpno"), Label)
            Dim email = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If pno.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, email.Text)
            Else
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, pno.Text)
            End If

            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "PEER")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If
                ' ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()
            Else
                Dim isSub As Boolean = isSubordinate1(id.Text, ViewState("FY").ToString(), "2")
                Dim totalSub As Integer = 0
                Dim surveySub As Integer = 0
                Dim remaining As Integer = 0
                If isSub Then
                    GetSubordinates1(Session("User_id").ToString(), ViewState("FY").ToString(), ViewState("SRLNO").ToString(), totalSub, surveySub, remaining)

                    If remaining >= 3 Then

                        ShowGenericMessageModal(CommonConstants.AlertType.warning, "You cannot uncheck more than 3 subordinates..")
                        Exit Sub
                    Else
                        Dim currentSelectedCount As Integer = CType(ViewState("GvPeer"), DataTable).Rows.Count
                        Dim uncheckedDt As DataTable = TryCast(ViewState("DeletePeer"), DataTable)
                        Dim uncheckedCount As Integer = If(uncheckedDt IsNot Nothing, uncheckedDt.Rows.Count, 0)
                        Dim uncheckedcount1 As Integer
                        uncheckedcount1 = remaining + uncheckedCount

                        If uncheckedcount1 >= 3 Then
                            ShowGenericMessageModal(CommonConstants.AlertType.warning, "You cannot uncheck more than 3 subordinates.")
                            chk.Checked = True
                            Exit Sub
                        End If


                    End If
                End If
                Dim flagst As String = String.Empty
                ' flagst = checkautopopulate(id.Text, "Y", "PEER")
                flagst = "N"
                Dim catg = "PEER"
                Dim catgory As String = String.Empty
                'If flagst = "Y" Then
                '    If catg = "PEER" Then
                '        catgory = "Subordinate/Peer"
                '    End If
                '    Dim strself1 As String = String.Empty
                '    strself1 = " select  b.ema_ename from hrps.t_emp_master_feedback360  b where b.EMA_YEAR=:EMA_YEAR AND b.EMA_CYCLE=:EMA_CYCLE and b.ema_perno='" + id.Text.Trim + "' "
                '    Dim cmd1 As New OracleCommand(strself1, conHrps)
                '    cmd1.Parameters.Clear()
                '    cmd1.Parameters.AddWithValue("ema_perno", id.Text.Trim)
                '    cmd1.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString)
                '    cmd1.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString)
                '    Dim da As New OracleDataAdapter(cmd1)
                '    Dim f1 As New DataTable()
                '    da.Fill(f1)
                '    Dim empname As String = String.Empty
                '    If f1.Rows.Count > 0 Then
                '        empname = f1.Rows(0).Item("ema_ename")
                '        ShowGenericMessageModal(CommonConstants.AlertType.error, "You cannot uncheck " + empname + " from " + catgory + " Category")
                '        chk.Checked = True
                '        Exit Sub
                '    Else

                '    End If
                'Else
                'UpdateData(id.Text, "DS", "Y", "PEER")
                ''''Added by Manoj Kumar on 25-05-2021
                'ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL
                Dim dt As New DataTable
                dt = CType(ViewState("GvPeer"), DataTable)

                Dim dtDeletePeer As New DataTable
                dtDeletePeer.Columns.Add("ema_perno", GetType(String))
                dtDeletePeer.Columns.Add("EMA_EMAIL_ID", GetType(String))
                dtDeletePeer.Rows.Add(pno.Text, email.Text)
                If ViewState("TagPeer") = "1" Then
                    CType(ViewState("DeletePeer"), DataTable).Rows.Add(pno.Text, email.Text)
                    ViewState("TagPeer") = "1"
                Else
                    ViewState("DeletePeer") = dtDeletePeer
                    ViewState("TagPeer") = "1"
                End If


                Dim Dr() As DataRow
                If id.Text.Trim <> "" Then
                    Dr = dt.Select("EMA_PERNO = '" + id.Text.Trim.ToString + "'", "EMA_PERNO") ''
                Else
                    Dr = dt.Select("EMA_EMAIL_ID='" + email.Text.Trim + "'", "EMA_EMAIL_ID")
                End If
                For i As Integer = 0 To Dr.Length - 1
                    'If id.Text.Trim <> "" Then
                    '    UpdateData(id.Text, "DS", "Y", "PEER")
                    'Else
                    '    DeleteData(email.Text, "DS", "Y", "PEER")
                    'End If

                    'ViewState("DeletePeer") = Dr



                    dt.Rows.Remove(Dr(i))
                Next
                ViewState("GvPeer") = dt
                GvPeer.DataSource = ViewState("GvPeer")
                GvPeer.DataBind()


                Dim dt1 As DataTable = CType(ViewState("gvfinal"), DataTable)
                Dim Dr1() As DataRow
                If id.Text.Trim <> "" Then
                    Dr1 = dt1.Select("SS_PNO = '" + id.Text.Trim + "'", "SS_PNO") ''
                Else
                    Dr1 = dt1.Select("SS_EMAIL='" + email.Text.Trim + "'", "SS_EMAIL")
                End If

                For j As Integer = 0 To Dr1.Length - 1
                    dt1.Rows.Remove(Dr1(j))
                Next
                ViewState("gvfinal") = dt1
                gvfinal.DataSource = ViewState("gvfinal")
                gvfinal.DataBind()

                ''''Added by Manoj Kumar on 25-05-2021
            End If
            'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
            'bindFinalGrid()
            'End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    Protected Sub chkseldmanagr_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chkseldmanagr"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim pno = CType(gv.FindControl("lblpno"), Label)
            Dim email = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If pno.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, email.Text)
            Else
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, pno.Text)
            End If

            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "MANGR")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If
                ' ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()
            Else
                Dim flagst As String = String.Empty
                'Commented by TCS on 23112022, to not execute autopopulated manager check, because manager uncheck option is already disable
                'flagst = checkautopopulate(id.Text, "Y", "MANGR")
                Dim catg = "MANGR"
                Dim catgory As String = String.Empty
                If flagst = "Y" Then
                    If catg = "MANGR" Then
                        catgory = "Manager/Superior"
                    End If
                    Dim strself1 As String = String.Empty
                    strself1 = " select  b.ema_ename from hrps.t_emp_master_feedback360  b where b.EMA_YEAR=:EMA_YEAR AND b.EMA_CYCLE=:EMA_CYCLE and b.ema_perno=:ema_perno "
                    Dim cmd1 As New OracleCommand(strself1, conHrps)
                    cmd1.Parameters.Clear()
                    cmd1.Parameters.AddWithValue("ema_perno", id.Text.Trim)
                    cmd1.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString)
                    cmd1.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString)
                    Dim da As New OracleDataAdapter(cmd1)
                    Dim f1 As New DataTable()
                    da.Fill(f1)
                    Dim empname As String = String.Empty
                    If f1.Rows.Count > 0 Then
                        empname = f1.Rows(0).Item("ema_ename")
                        ShowGenericMessageModal(CommonConstants.AlertType.error, "You cannot uncheck " + empname + " from " + catgory + " Category")
                        chk.Checked = True
                        Exit Sub
                    Else

                    End If
                Else
                    'UpdateData(id.Text, "DS", "Y", "MANGR")              Commented by Manoj Kumar 25-05-2021
                    ''''Added by Manoj Kumar on 25-05-2021
                    Dim dt As New DataTable
                    dt = CType(ViewState("GvManager"), DataTable)

                    Dim dtDeleteManager As New DataTable
                    dtDeleteManager.Columns.Add("ema_perno", GetType(String))
                    dtDeleteManager.Columns.Add("EMA_EMAIL_ID", GetType(String))
                    dtDeleteManager.Rows.Add(pno.Text, email.Text)
                    If ViewState("TagManager") = "1" Then
                        CType(ViewState("DeleteManager"), DataTable).Rows.Add(pno.Text, email.Text)
                        ViewState("TagManager") = "1"
                    Else
                        ViewState("DeleteManager") = dtDeleteManager
                        ViewState("TagManager") = "1"
                    End If


                    Dim Dr() As DataRow
                    If id.Text.Trim <> "" Then
                        Dr = dt.Select("EMA_PERNO = '" + id.Text.Trim.ToString + "'", "EMA_PERNO") ''
                    Else
                        Dr = dt.Select("EMA_EMAIL_ID='" + email.Text.Trim + "'", "EMA_EMAIL_ID")
                    End If

                    Dim dtManagerClone = dt.Copy

                    For i As Integer = 0 To Dr.Length - 1
                        'If id.Text.Trim <> "" Then
                        '    UpdateData(id.Text, "DS", "Y", "MANGR")
                        'Else
                        '    UpdateData(email.Text, "DS", "Y", "MANGR")
                        'End If

                        dt.Rows.Remove(Dr(i))
                    Next
                    ViewState("GvManager") = dt
                    GvManager.DataSource = ViewState("GvManager")
                    GvManager.DataBind()


                    Dim dt1 As DataTable = CType(ViewState("gvfinal"), DataTable)
                    Dim Dr1() As DataRow
                    If id.Text.Trim <> "" Then
                        Dr1 = dt1.Select("SS_PNO = '" + id.Text.Trim + "'", "SS_PNO") ''
                    Else
                        Dr1 = dt1.Select("SS_EMAIL='" + email.Text.Trim + "'", "SS_EMAIL")
                    End If

                    For j As Integer = 0 To Dr1.Length - 1
                        dt1.Rows.Remove(Dr1(j))
                    Next
                    'Added by TCS on 24112022 to handle auto Peer bind if removed fro Manager list and belog to peer
                    If Session("label") = "I3" Or Session("label") = "I2" Or Session("label") = "I1" Then
                        If isUncheckedManagerBelongtoPeer(id.Text.Trim) Then
                            Dim drManager() As DataRow
                            If id.Text.Trim <> "" Then
                                drManager = dtManagerClone.Select("EMA_PERNO = '" + id.Text.Trim.ToString + "'", "EMA_PERNO") ''
                            Else
                                drManager = dtManagerClone.Select("EMA_EMAIL_ID='" + email.Text.Trim + "'", "EMA_EMAIL_ID")
                            End If
                            Dim dtPeer As New DataTable
                            dtPeer = CType(ViewState("GvPeer"), DataTable)
                            For k As Integer = 0 To drManager.Length - 1
                                dtPeer.ImportRow(drManager(k))
                            Next
                            ViewState("GvPeer") = dtPeer
                            GvPeer.DataSource = ViewState("GvPeer")
                            GvPeer.DataBind()

                            Dim desc As String = ""
                            If Session("label").Equals("I2") Or Session("label").Equals("I1") Then
                                desc = "Peers"
                            ElseIf Session("label").Equals("I3") Then
                                desc = "Peers and Subordinate"
                            End If
                            'Dim dtPeer As DataTable = CType(ViewState("GvPeer"), DataTable)
                            Dim drPeer() As DataRow
                            If id.Text.Trim <> "" Then
                                drPeer = dtPeer.Select("EMA_PERNO = '" + id.Text.Trim.ToString + "'", "EMA_PERNO") ''
                            Else
                                drPeer = dtPeer.Select("EMA_EMAIL_ID='" + email.Text.Trim + "'", "EMA_EMAIL_ID")
                            End If
                            For k As Integer = 0 To drPeer.Length - 1
                                dt1.Rows.Add(drPeer(k)(0), drPeer(k)(0), drPeer(k)(1), drPeer(k)(2), drPeer(k)(3), drPeer(k)(4), drPeer(k)(5), desc, "ORG", "PEER", "")
                            Next
                        End If
                    End If
                    'End
                    ViewState("gvfinal") = dt1
                    gvfinal.DataSource = ViewState("gvfinal")
                    gvfinal.DataBind()

                    ''''Added by Manoj Kumar on 25-05-2021
                End If
                'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
                ' bindFinalGrid()         'Commented by Manoj Kumar on 26-05-2021
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub chkseldselSubordinates_CheckedChanged(sender As Object, e As EventArgs)
        Try

            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chkseldselSubordinates"), CheckBox)
            Dim id = CType(gv.FindControl("lblSubordinatespno"), Label)
            Dim pno = CType(gv.FindControl("lblSubordinatespno"), Label)
            Dim email = CType(gv.FindControl("lblSubordinatesemail"), Label)
            Dim val As String
            If pno.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, email.Text)
            Else
                val = Check(ViewState("FY").ToString(), Session("User_id").ToString, pno.Text)
            End If

            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "ROPT")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If
                ' ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()
            Else
                Dim flagst As String = String.Empty
                flagst = checkautopopulate(id.Text, "Y", "ROPT")
                Dim catg = "ROPT"
                Dim catgory As String = String.Empty
                'If flagst = "Y" Then
                '    If catg = "ROPT" Then
                '        catgory = "Subordinates"
                '    End If
                '    Dim strself1 As String = String.Empty
                '    strself1 = " select  b.ema_ename from hrps.t_emp_master_feedback360  b where b.EMA_YEAR=:EMA_YEAR AND b.EMA_CYCLE=:EMA_CYCLE and b.ema_perno=:ema_perno "
                '    Dim cmd1 As New OracleCommand(strself1, conHrps)
                '    cmd1.Parameters.Clear()
                '    cmd1.Parameters.AddWithValue("ema_perno", id.Text.Trim)
                '    cmd1.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString)
                '    cmd1.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString)
                '    Dim da As New OracleDataAdapter(cmd1)
                '    Dim f1 As New DataTable()
                '    da.Fill(f1)
                '    Dim empname As String = String.Empty
                '    If f1.Rows.Count > 0 Then
                '        empname = f1.Rows(0).Item("ema_ename")
                '        ShowGenericMessageModal(CommonConstants.AlertType.error, "You cannot uncheck " + empname + " from " + catgory + " Category")
                '        chk.Checked = True
                '        Exit Sub
                '    Else

                '    End If
                'Else
                'UpdateData(id.Text, "DS", "Y", "MANGR")              Commented by Manoj Kumar 25-05-2021
                ''''Added by Manoj Kumar on 25-05-2021
                Dim dt As New DataTable
                dt = CType(ViewState("GvSubordinates"), DataTable)

                Dim dtDeleteSubordinates As New DataTable
                dtDeleteSubordinates.Columns.Add("ema_perno", GetType(String))
                dtDeleteSubordinates.Columns.Add("EMA_EMAIL_ID", GetType(String))
                dtDeleteSubordinates.Rows.Add(pno.Text, email.Text)
                If ViewState("TagSubordinates") = "1" Then
                    CType(ViewState("DeleteSubordinates"), DataTable).Rows.Add(pno.Text, email.Text)
                    ViewState("TagSubordinates") = "1"
                Else
                    ViewState("DeleteSubordinates") = dtDeleteSubordinates
                    ViewState("TagSubordinates") = "1"
                End If

                Dim Dr() As DataRow
                If id.Text.Trim <> "" Then
                    Dr = dt.Select("EMA_PERNO = '" + id.Text.Trim.ToString + "'", "EMA_PERNO") ''
                Else
                    Dr = dt.Select("EMA_EMAIL_ID='" + email.Text.Trim + "'", "EMA_EMAIL_ID")
                End If

                For i As Integer = 0 To Dr.Length - 1
                    'If id.Text.Trim <> "" Then
                    '    UpdateData(id.Text, "DS", "Y", "ROPT")
                    'Else
                    '    UpdateData(email.Text, "DS", "Y", "ROPT")
                    'End If

                    dt.Rows.Remove(Dr(i))
                Next
                ViewState("GvSubordinates") = dt
                gvSubordinates.DataSource = ViewState("GvSubordinates")
                gvSubordinates.DataBind()


                Dim dt1 As DataTable = CType(ViewState("gvfinal"), DataTable)
                Dim Dr1() As DataRow
                If id.Text.Trim <> "" Then
                    Dr1 = dt1.Select("SS_PNO = '" + id.Text.Trim + "'", "SS_PNO") ''
                Else
                    Dr1 = dt1.Select("SS_EMAIL='" + email.Text.Trim + "'", "SS_EMAIL")
                End If

                For j As Integer = 0 To Dr1.Length - 1
                    dt1.Rows.Remove(Dr1(j))
                Next
                ViewState("gvfinal") = dt1
                gvfinal.DataSource = ViewState("gvfinal")
                gvfinal.DataBind()

                ''''Added by Manoj Kumar on 25-05-2021
            End If
            'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
            ' bindFinalGrid()         'Commented by Manoj Kumar on 26-05-2021
            'End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()
            End If
        End Try
    End Sub
    Protected Sub saveAsDraft()
        Try
            Dim dtFinalDelete As New DataTable
            If ViewState("TagPeer") = "1" Then
                dtFinalDelete.Merge(CType(ViewState("DeletePeer"), DataTable))
            End If
            If ViewState("TagManager") = "1" Then
                dtFinalDelete.Merge(CType(ViewState("DeleteManager"), DataTable))
            End If
            If ViewState("TagSubordinates") = "1" Then
                dtFinalDelete.Merge(CType(ViewState("DeleteSubordinates"), DataTable))
            End If
            If ViewState("Tagintstholder") = "1" Then
                dtFinalDelete.Merge(CType(ViewState("Deleteintstholder"), DataTable))
            End If
            If dtFinalDelete.Rows.Count > 0 Then
                For i As Integer = 0 To dtFinalDelete.Rows.Count - 1
                    UpdateData(dtFinalDelete.Rows(i)("ema_perno").ToString(), "DS", "Y", "MANGR")
                Next
            End If
            If CType(ViewState("gvfinal"), DataTable).Rows.Count > 0 Then
                Dim dtOrdg As DataTable = CType(ViewState("gvfinal"), DataTable)
                Dim DrOrg() As DataRow
                DrOrg = dtOrdg.Select("SSTYPE = 'ORG'", "SSTYPE") ''

                For Each row As DataRow In DrOrg

                    Dim cmd As New OracleCommand()

                    cmd.CommandText = "select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and SS_PNO =:SS_PNO and SS_STATUS ='SE' and SS_SRLNO=:SS_SRLNO"
                    cmd.CommandText += " And ss_year =:ss_year and SS_DEL_TAG='N'"
                    cmd.Connection = conHrps
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    cmd.Parameters.AddWithValue("SS_PNO", row(1).ToString)
                    cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
                    Dim f = getDataInDt(cmd)
                    If f.Rows.Count > 0 Then

                    Else
                        SaveData(row(9).ToString, row(1).ToString, row(2).ToString, row(4).ToString, row(5).ToString, row(6).ToString, row(3).ToString, "ORG", "SE")
                    End If
                Next


                Dim DrNOrg() As DataRow
                DrNOrg = dtOrdg.Select("SSTYPE = 'NORG'", "SSTYPE") ''

                For Each row1 As DataRow In DrNOrg
                    Dim cmd1 As New OracleCommand()

                    cmd1.CommandText = "select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and SS_EMAIL =:SS_EMAIL and SS_STATUS ='SE' and SS_SRLNO=:SS_SRLNO"
                    cmd1.CommandText += " And ss_year =:ss_year and SS_DEL_TAG='N'"
                    cmd1.Connection = conHrps
                    cmd1.Parameters.Clear()
                    cmd1.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                    cmd1.Parameters.AddWithValue("SS_EMAIL", row1(6).ToString)
                    cmd1.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                    cmd1.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
                    Dim f = getDataInDt(cmd1)
                    If f.Rows.Count > 0 Then

                    Else
                        SaveData(row1(9).ToString, row1(1).ToString, row1(2).ToString, row1(4).ToString, row1(5).ToString, row1(6).ToString, "", "NORG", "SE")
                    End If
                Next
                'Added by TCS on 30112022 to refresh the all grid to fix the delete issue after save as draft
                BindGrid()
                'End
                bindFinalGrid()
                ViewState("TagPeer") = ""
                ViewState("TagManager") = ""
                ViewState("TagSubordinates") = ""
                ViewState("Tagintstholder") = ""



            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub submit()
        Try

            Dim statussession = SessionTimeOut()
            If statussession = False Then
                Exit Sub
            End If
            If txtfinalcap.Text.Trim = lblfinalcap.Text.Trim Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Invalid Captcha Provided")
                txtfinalcap.Text = String.Empty
                Exit Sub
            End If
            saveAsDraft()

            Dim stat = ChkValidation()
            If Len(stat) > 0 Then
                lblfinalcap.Text = ""
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please select Minimum no of assessors in " & stat & " Category")
                Exit Sub
            End If
            Dim statmax = ChkValidationmax()
            If Len(statmax) > 0 Then
                lblfinalcap.Text = ""
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If

            Dim iskipMngrMax = checkSkipMAnagerMaxSelection()
            If Len(iskipMngrMax) > 0 Then
                lblfinalcap.Text = ""
                ShowGenericMessageModal(CommonConstants.AlertType.error, iskipMngrMax & " who has been identified as Manager, has crossed the maximum limit of response as a Manager. You may wish to choose a different respondent as Manager or drop the same.")
                Exit Sub
            End If

            'Added by TCS on 20112022, To check a single respondent not choosen by assesse more than the limit set for that level
            Dim intshMax = checkINTSHMaxSelection()
            If Len(intshMax) > 0 Then
                lblfinalcap.Text = ""
                ShowGenericMessageModal(CommonConstants.AlertType.error, intshMax & " who has been identified as an internal stakeholder, has crossed the maximum limit of response as an internal stakeholder. You may wish to choose a different respondent as an Internal Stakeholder.")
                Exit Sub
            End If
            'End

            Dim comd As New OracleCommand()
            Dim pno = Session("USER_ID").ToString()
            If Session("label").Equals("I2") Then
                'Commented & Added by TCS on 081222 to replace Dotted Perno to Reporting Perno
                'comd.CommandText = " select  EMA_DOTTED_PNO ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
                comd.CommandText = " select  ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
                'End
            Else
                comd.CommandText = " select  ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
            End If

            comd.Connection = conHrps
            comd.Parameters.Clear()
            comd.Parameters.AddWithValue("ema_perno", pno)
            comd.Parameters.AddWithValue("EMA_YEAR", ViewState("FY").ToString)
            comd.Parameters.AddWithValue("EMA_CYCLE", ViewState("SRLNO").ToString)
            Dim dt = getDataInDt(comd)
            If dt.Rows.Count > 0 Then
                'Added by TCS on 08122022 to chack approver, status updated or not & also session expired check
                'UpdateApprover(dt.Rows(0)("ema_reporting_to_pno").ToString, pno)
                Dim intCount = UpdateApprover(dt.Rows(0)("ema_reporting_to_pno").ToString, pno)
                If intCount < 1 Then
                    statussession = SessionTimeOut()
                    If statussession = False Then
                    Else
                        ShowGenericMessageModal(CommonConstants.AlertType.error, "Some error occured. Kindly Refesh the page & try again..")
                    End If
                    Exit Sub
                End If
                'End
            End If

            Dim qry As String = String.Empty
            'qry = " update t_survey_status set SS_TAG ='SU' , SS_TAG_DT = sysdate where SS_STATUS ='SE' and SS_ASSES_PNO ='" & pno & "' and ss_year='" & ViewState("FY").ToString() & "'"
            If Session("label").Equals("I1") Then
                qry = "update t_survey_status set SS_APP_TAG='AP',SS_TAG_DT=sysdate,SS_WFL_STATUS='2',SS_UPDATED_BY=:SS_UPDATED_BY, SS_UPDATED_DT=sysdate,SS_TAG ='SU',ss_app_dt=sysdate where "
                qry += " ss_year=:ss_year and SS_ASSES_PNO =:SS_ASSES_PNO and SS_SRLNO=:SS_SRLNO"
            Else
                qry = " update t_survey_status set SS_TAG ='SU',ss_app_tag='' , SS_TAG_DT = sysdate , ss_app_dt=sysdate,SS_UPDATED_BY=:SS_UPDATED_BY, SS_UPDATED_DT=sysdate "
                qry += " where SS_STATUS ='SE' and SS_ASSES_PNO =:SS_ASSES_PNO and ss_year=:ss_year AND SS_DEL_TAG='N' and SS_SRLNO=:SS_SRLNO"
            End If




            If conHrps.State = ConnectionState.Closed Then
                conHrps.Open()
            End If

            Dim cmd As New OracleCommand(qry, conHrps)
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", pno)
            cmd.Parameters.AddWithValue("SS_UPDATED_BY", pno)
            cmd.Parameters.AddWithValue("ss_year", ViewState("FY").ToString())
            cmd.ExecuteNonQuery()
            ShowGenericMessageModal(CommonConstants.AlertType.success, " Your response has been submitted. Thank You!")
            Session.RemoveAll()
            lbOrg.Visible = False
            lbOrg.Visible = False
            GvManager.Visible = False
            btnSaveAsDraft.Visible = False
            btnaddtslsub.Visible = False
            btnnontslsub.Visible = False
            btnAddopr.Visible = False
            div1.Visible = False
            div2.Visible = False
            Div3.Visible = False
            GvRepoties.Visible = False
            btntatasteel.Visible = False
            btnnontslp.Visible = False
            divnontsl.Visible = False
            rowpeer.Visible = False
            div1.Visible = False
            GvPeer.Visible = False
            btnaddpeertsl.Visible = False
            btnaddnontsl.Visible = False
            divtsl.Visible = False
            divntsl.Visible = False
            Gvintstholder.Visible = False
            btntatasteelopr.Visible = False
            btnnonopr.Visible = False
            lblcaptfinalmsg.Visible = False
            txtfinalcap.Visible = False
            lblfinalcap.Visible = False
        Catch ex As Exception

        Finally
            If conHrps.State = ConnectionState.Open Then
                conHrps.Close()

            End If

            lblfinalcap.Text = GetRandomText()
            Session.Remove("codeman")
        End Try
    End Sub
    Protected Sub btnnontslp_Click(sender As Object, e As EventArgs)
        rowpeer.Visible = False
        div1.Visible = True
        lblcaptpeer.Text = GetRandomText()
        Session.Remove("codeman")

    End Sub
    Protected Sub btnaddmanager_Click(sender As Object, e As EventArgs)

        Try
            Dim statmax = ChkValidationmaxmgr()
            If Len(statmax) > 0 Then
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If txtpnoopr.Text.Trim() <> "" Then
                Dim lvl As String = ChkAuthlabelbymail(txtemailopr.Text.Trim)

                'Added by TCS on 17112022
                If Not isSelecetedManagerLevelValid(lvl) Then
                    reset()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Only higher level Manager selection allowed")
                    Exit Sub
                End If
                'End

                If lvl.Equals("I3") Then
                    lvl = "IL3"
                ElseIf lvl.Equals("I2") Then
                    lvl = "IL2"
                ElseIf lvl.Equals("I4") Then
                    lvl = "IL4"
                ElseIf lvl.Equals("I5") Then
                    lvl = "IL5"
                ElseIf lvl.Equals("I6") Then
                    lvl = "IL6"
                ElseIf lvl.Equals("I1") Then
                    lvl = "IL1"
                End If

                Dim perno = txtpnoopr.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgopr.Text.Trim(), "'", "''")
                Dim dept = Replace(txtorgopr.Text.Trim, "'", "''")
                Dim email = Replace(txtemailopr.Text.Trim, "'", "''")

                Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), pno)

                If val = "" Then
                    'Modified by TCS on 01122023, Store Sub Grade insted of Equivqlent level
                    'CType(ViewState("GvManager"), DataTable).Rows.Add(pno, name, lvl, desg, dept, email, "ORG", "MANGR", "Manager/Superior")
                    CType(ViewState("GvManager"), DataTable).Rows.Add(pno, name, lblpeerlevel.Text, desg, dept, email, "ORG", "MANGR", "Manager/Superior")   ' Added by Manoj Kumar on 25-05-2021
                    'SaveData("MANGR", pno, name, desg, dept, email, lvl, "ORG", "SE")                            ' Commented by Manoj Kumar on  25-05-2021
                    reset()
                    BindSessionManager()                                                                            ' Added by Manoj Kumar on 25-05-2021
                    'BindGrid()                                                                                     ' Commented by Manoj Kumar on  25-05-2021
                    'bindFinalGrid()
                    'CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, lvl, desg, dept, email, "Manager/Superior", "ORG", "MANGR", "")
                    CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, lblpeerlevel.Text, desg, dept, email, "Manager/Superior", "ORG", "MANGR", "")
                    BindSessionFinalGrid()
                Else
                    reset()
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                    Exit Sub
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "P.No or Name Blank, Please fill...!")
                Exit Sub
            End If

        Catch ex As Exception
            MsgBox(ex.Message.ToString)
        End Try
    End Sub
    '  Add method by Manoj Kumar on 25-05-2021
    Public Sub BindSessionManager()
        Try
            If CType(ViewState("GvManager"), DataTable).Rows.Count > 0 Then
                GvManager.DataSource = ViewState("GvManager")
                GvManager.DataBind()
            Else
                GvManager.DataSource = Nothing
                GvManager.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub BindSessionIntsh()
        Try
            If CType(ViewState("Gvintstholder"), DataTable).Rows.Count > 0 Then
                Gvintstholder.DataSource = ViewState("Gvintstholder")
                Gvintstholder.DataBind()
            Else
                Gvintstholder.DataSource = Nothing
                Gvintstholder.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub BindSessionPeer()
        Try
            If CType(ViewState("GvPeer"), DataTable).Rows.Count > 0 Then
                GvPeer.DataSource = ViewState("GvPeer")
                GvPeer.DataBind()
            Else
                GvPeer.DataSource = Nothing
                GvPeer.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub BindSessionSubordinates()
        Try
            If CType(ViewState("GvSubordinates"), DataTable).Rows.Count > 0 Then
                gvSubordinates.DataSource = ViewState("GvSubordinates")
                gvSubordinates.DataBind()
            Else
                gvSubordinates.DataSource = Nothing
                gvSubordinates.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub BindSessionFinalGrid()
        Try
            If CType(ViewState("gvfinal"), DataTable).Rows.Count > 0 Then
                gvfinal.DataSource = ViewState("gvfinal")
                gvfinal.DataBind()
            Else
                gvfinal.DataSource = Nothing
                gvfinal.DataBind()
            End If
        Catch ex As Exception

        End Try
    End Sub
    '  Add method by Manoj Kumar on 25-05-2021
    Protected Sub btnaddpeer_Click(sender As Object, e As EventArgs)
        Try
            Dim statmax = ChkValidationmaxpeer()
            If Len(statmax) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If txtnmpeer.Text.Trim() = "" Or txtdesgpeer.Text.Trim() = "" Or txtmailpeer.Text = "" Or txtdeptpeer.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If
            'If txtcaptpeer.Text.Trim = lblcaptpeer.Text.Trim Then
            'Else
            '    ShowGenericMessageModal(CommonConstants.AlertType.info, "Please Provide Appropriate Captcha!")
            '    txtcaptpeer.Text = String.Empty
            '    Exit Sub
            'End If
            If ChkMail(txtmailpeer.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If

            Dim pno = ""
            Dim name = Replace(txtnmpeer.Text.Trim(), "'", "''")
            Dim desg = Replace(txtdesgpeer.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptpeer.Text.Trim, "'", "''")
            Dim email = Replace(txtmailpeer.Text.Trim, "'", "''")

            Dim val = Checkmangr(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then
                'SaveData("PEER", pno, name, desg, dept, email, "", "NORG", "SE")
                'ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL
                Dim desc As String = ""
                If Session("label").Equals("I2") Or Session("label").Equals("I1") Then
                    desc = "Peers"
                ElseIf Session("label").Equals("I3") Then
                    desc = "Peers and Subordinate"
                End If
                CType(ViewState("GvPeer"), DataTable).Rows.Add(pno, name, "", desg, dept, email, "ORG", "PEER", desc)
                reset()
                ' BindGrid()
                'bindFinalGrid()
                BindSessionPeer()


                CType(ViewState("gvfinal"), DataTable).Rows.Add(pno, pno, name, "", desg, dept, email, desc, "NORG", "PEER", "")
                BindSessionFinalGrid()

            Else
                reset()
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception
        Finally
            txtcaptpeer.Text = String.Empty
            lblcaptpeer.Text = GetRandomText()
            Session.Remove("codeman")
        End Try

    End Sub
    Protected Sub btnnontslsub_Click(sender As Object, e As EventArgs)
        div2.Visible = True
        Div3.Visible = False
    End Sub
    Protected Sub btnaddmgr_Click(sender As Object, e As EventArgs)
        Try
            If txtnamemgr.Text.Trim() = "" Or txtdesgmgr.Text.Trim() = "" Or txtemailmgr.Text = "" Or txtdeptmgr.Text.Trim() = "" Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Please Fill All Column ...!")
                Exit Sub
            End If

            If ChkMail(txtemailmgr.Text) Then
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please do not use @tatasteel.com email id...!")
                Exit Sub
            End If

            Dim pno = ""
            Dim name = Replace(txtnamemgr.Text.Trim(), "'", "''")
            Dim desg = Replace(txtdesgmgr.Text.Trim(), "'", "''")
            Dim dept = Replace(txtdeptmgr.Text.Trim, "'", "''")
            Dim email = Replace(txtemailmgr.Text.Trim, "'", "''")


            Dim val = Check(ViewState("FY").ToString(), Session("USER_ID").ToString(), email)

            If val = "" Then

                'SaveData("ROPT", pno, name, desg, dept, email, "", "NORG", "SE")
                reset()
                BindGrid()
                bindFinalGrid()
            Else
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & val & " category...!")
                Exit Sub
            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Function ChkMail(mail As String) As Boolean
        Dim val As Boolean = True
        Try
            Dim email = mail.Split("@")
            If email(1).ToUpper = UCase("TATASTEEL.COM") Then
                val = False
            Else
                val = True
            End If
        Catch ex As Exception

        End Try
        Return val
    End Function
    Protected Sub btnaddtslsub_Click(sender As Object, e As EventArgs)
        div2.Visible = False
        Div3.Visible = True
    End Sub
    Protected Sub txtaddsub_Click(sender As Object, e As EventArgs)
        Try
            Dim statmax = ChkValidationmaxsub()
            If Len(statmax) > 0 Then
                ShowGenericMessageModal(CommonConstants.AlertType.error, "Number of assessors in " & statmax & " Category exceed maximum number")
                Exit Sub
            End If
            If txtpnosub.Text <> "" Then
                Dim perno = txtpnosub.Text.Trim()
                Dim pno = Right(perno, 6)
                Dim name = perno.Remove(perno.Length - 6)
                Dim desg = Replace(txtdesgsub.Text.Trim(), "'", "''")
                Dim dept = Replace(txtdeptsub.Text.Trim, "'", "''")
                Dim email = Replace(txtmailsub.Text.Trim, "'", "''")

                Dim Val = Check(ViewState("FY"), Session("User_id").ToString, pno)

                If Val = "" Then
                    Dim label As String = ChkAuthlabel(pno)
                    If Session("label").Equals("I3") Then
                        If label = "I2" Or label = "I1" Then
                            ShowGenericMessageModal(CommonConstants.AlertType.error, "Subordinate should be same level or lower level...!")
                            Exit Sub
                        End If
                    End If
                    If Session("label").Equals("I4") Then
                        If label = "I2" Or label = "I1" Or label = "I3" Then
                            ShowGenericMessageModal(CommonConstants.AlertType.error, "Subordinate should be same level or lower level...!")
                            Exit Sub
                        End If
                    End If
                    If Session("label").Equals("I5") Then
                        If label = "I2" Or label = "I1" Or label = "I3" Or label = "I4" Then
                            ShowGenericMessageModal(CommonConstants.AlertType.error, "Subordinate should be same level or lower level...!")
                            Exit Sub
                        End If
                    End If
                    If Session("label").Equals("I6") Then
                        If label = "I2" Or label = "I1" Or label = "I3" Or label = "I4" Or label = "I5" Then
                            ShowGenericMessageModal(CommonConstants.AlertType.error, "Subordinate should be same level or lower level...!")
                            Exit Sub
                        End If
                    End If
                    SaveData("ROPT", pno, name, desg, dept, email, lblsublvl.Text, "ORG", "SE")
                    reset()
                    BindGrid()
                    bindFinalGrid()
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.error, "Already added in " & Val & " Category...!")
                    Exit Sub
                End If

            Else
                ShowGenericMessageModal(CommonConstants.AlertType.info, "Please enter name or pno..!")
                Exit Sub

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub txtpnosub_TextChanged(sender As Object, e As EventArgs)

        Try
            Dim pno = txtpnosub.Text.Trim()
            Dim strself As New OracleCommand()
            strself.CommandText = "select ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID from hrps.t_emp_master_feedback360 where "
            ' Start WI368  by Manoj Kumar on 31-05-2021 add emp class column for officer only)
            strself.CommandText += " (ema_ename ||ema_perno )=:pno AND EMA_YEAR=:EMA_YEAR AND EMA_CYCLE=:EMA_CYCLE"
            'End by Manoj KUmar on 31-05-2021
            strself.Parameters.Clear()
            strself.Connection = conHrps
            strself.Parameters.Add(New OracleParameter("pno", pno.ToString()))
            strself.Parameters.Add(New OracleParameter("EMA_YEAR", ViewState("FY").ToString()))
            strself.Parameters.Add(New OracleParameter("EMA_CYCLE", ViewState("SRLNO").ToString()))
            Dim da As New OracleDataAdapter(strself)
            Dim dt As New DataTable()
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtdesgsub.Text = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtmailsub.Text = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptsub.Text = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtmailsub.ToolTip = dt.Rows(0)("EMA_EMAIL_ID").ToString()
                txtdeptsub.ToolTip = dt.Rows(0)("EMA_DEPT_DESC").ToString()
                txtdesgsub.ToolTip = dt.Rows(0)("EMA_DESGN_DESC").ToString()
                txtpnosub.ToolTip = txtpnosub.Text
                lblsublvl.Text = dt.Rows(0)("EMA_EMPL_PGRADE").ToString()
                txtdesgsub.ReadOnly = True
                If txtmailsub.Text.Trim <> "" Then
                    txtmailsub.ReadOnly = True
                Else
                    txtmailsub.ReadOnly = False
                End If

                txtdeptsub.ReadOnly = True
            Else

                ShowGenericMessageModal(CommonConstants.AlertType.warning, "Please select Person in list...")
                reset()
                Exit Sub
            End If

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub chksub_CheckedChanged(sender As Object, e As EventArgs)
        Try
            Dim gv = CType(sender, CheckBox).Parent.Parent
            Dim chk = CType(gv.FindControl("chksub"), CheckBox)
            Dim id = CType(gv.FindControl("lblpno"), Label)
            Dim pno = CType(gv.FindControl("lblemail"), Label)
            Dim val As String
            If id.Text.StartsWith("SR") Then
                val = Check(ViewState("FY").ToString, Session("User_id").ToString, pno.Text)
            Else
                val = Check(ViewState("FY").ToString, Session("User_id").ToString, id.Text)
            End If


            If chk.Checked = True Then
                If val = "" Then
                    UpdateData(id.Text, "SE", "N", "ROPT")
                Else
                    ShowGenericMessageModal(CommonConstants.AlertType.info, "Already added in " & val & " category...!")
                    chk.Checked = False
                    Exit Sub
                End If

                'ShowGenericMessageModal(CommonConstants.AlertType.success, "Selected...!")
                bindFinalGrid()

            Else
                UpdateData(id.Text, "DS", "Y", "ROPT")
                'ShowGenericMessageModal(CommonConstants.AlertType.warning, "De-Selected...!")
                bindFinalGrid()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GvRepoties_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Added by TCS on 29092023, to execute the code only for row not for header
            If e.Row.RowType = DataControlRowType.DataRow Then
                'Dim id = CType(e.Row.FindControl("hdfnid"), HiddenField)
                Dim chk = CType(e.Row.FindControl("chksub"), CheckBox)
                Dim perno = CType(e.Row.FindControl("lblpno"), Label)

                Dim comnd As New OracleCommand()

                If perno.Text = "" Then
                    comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'ROPT' and SS_ID=:SS_ID and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO AND SS_SRLNO=:SS_SRLNO"
                Else
                    comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'ROPT' and ss_pno=:ss_pno  and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO AND SS_SRLNO=:SS_SRLNO"
                End If
                comnd.Parameters.Clear()
                If perno.Text = "" Then
                    comnd.Parameters.AddWithValue("SS_ID", perno.Text)
                Else
                    comnd.Parameters.AddWithValue("ss_pno", perno.Text)
                End If
                comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                comnd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString)
                comnd.Connection = conHrps
                Dim da As New OracleDataAdapter(comnd)
                Dim d As New DataTable()
                da.Fill(d)
                ' Dim d = getRecordInDt(comnd, conHrps)

                If d.Rows.Count > 0 Then
                    chk.Checked = True
                Else
                    chk.Checked = False
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub GvPeer_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Added by TCS on 29092023, to execute the code only for row not for header
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim chk = CType(e.Row.FindControl("chkseldsel"), CheckBox)
                Dim perno = CType(e.Row.FindControl("lblpno"), Label)
                If CType(ViewState("GvPeer"), DataTable).Rows.Count > 0 Then
                    chk.Checked = True
                    chk.Enabled = False
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub gvSubordinates_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Added by TCS on 29092023, to execute the code only for row not for header
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim chk = CType(e.Row.FindControl("chkseldselSubordinates"), CheckBox)
                Dim perno = CType(e.Row.FindControl("lblSubordinatespno"), Label)
                If CType(ViewState("GvSubordinates"), DataTable).Rows.Count > 0 Then
                    chk.Checked = True
                    chk.Enabled = False
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Gvintstholder_DataBound(sender As Object, e As EventArgs) Handles Gvintstholder.DataBound
        lblTotalCount.Text = Gvintstholder.Rows.Count.ToString()
    End Sub
    Protected Sub gvSubordinates_DataBound(sender As Object, e As EventArgs) Handles gvSubordinates.DataBound
        lblSubOrdCount.Text = gvSubordinates.Rows.Count.ToString()
    End Sub
    Protected Sub GvPeer_DataBound(sender As Object, e As EventArgs) Handles GvPeer.DataBound
        lblPeerCount.Text = GvPeer.Rows.Count.ToString()
    End Sub
    Protected Sub GvManager_DataBound(sender As Object, e As EventArgs) Handles GvManager.DataBound
        lblManagerCount.Text = GvManager.Rows.Count.ToString()
    End Sub
    Protected Sub Gvintstholder_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try

            Dim totalRows As Integer = 0
            'Added by TCS on 29092023, to execute the code only for row not for header
            If e.Row.RowType = DataControlRowType.DataRow Then
                'totalRows += 1
                'Dim strsrl As String = "select IRC_CODE from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'"
                'Dim mycommand As OracleCommand
                'If conHrps.State = ConnectionState.Closed Then
                '    conHrps.Open()
                'End If
                'mycommand = New OracleCommand(strsrl, conHrps)
                'Dim dasrl As New OracleDataAdapter(mycommand)
                'Dim dtsrl As New DataTable()
                'dasrl.Fill(dtsrl)

                Dim chk = CType(e.Row.FindControl("chkseldsel"), CheckBox)
                Dim perno = CType(e.Row.FindControl("lblpno"), Label)

                'Dim comnd As New OracleCommand()

                'If perno.Text = "" Then
                '    comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'INTSH' and SS_ID=:SS_ID and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO and SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'"
                'Else
                '    comnd.CommandText = " select *  from t_survey_status  where SS_STATUS='SE' and SS_CATEG = 'INTSH' and ss_pno=:ss_pno  and SS_YEAR=:SS_YEAR and SS_ASSES_PNO=:SS_ASSES_PNO and SS_SRLNO='" & dtsrl.Rows(0).Item(0) & "'"
                'End If
                'comnd.Parameters.Clear()
                'If perno.Text = "" Then
                '    comnd.Parameters.AddWithValue("SS_ID", perno.Text)
                'Else
                '    comnd.Parameters.AddWithValue("ss_pno", perno.Text)
                'End If
                'comnd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                'comnd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
                'comnd.Connection = conHrps
                'Dim da As New OracleDataAdapter(comnd)
                'Dim d As New DataTable()
                'da.Fill(d)
                'If d.Rows.Count > 0 Then
                '    chk.Checked = True
                'Else
                '    chk.Checked = False
                'End If

                If CType(ViewState("Gvintstholder"), DataTable).Rows.Count > 0 Then
                    chk.Checked = True
                End If
            End If

            'If e.Row.RowType = DataControlRowType.Footer Then
            '    lblTotalCount.Text = totalRows.ToString()
            'End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function ValidPerno() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            'ocmd.CommandText = "SELECT DISTINCT ss_asses_pno FROM hrps.t_survey_status WHERE SS_APP_TAG='RJ'"
            'ocmd.CommandText += "  AND SS_YEAR='" + ViewState("FY").ToString + "' AND SS_SRLNO='" + ViewState("SRLNO").ToString + "' and ss_asses_pno='" + Session("USER_ID").ToString + "'"
            ocmd.CommandText = "select * from hrps.t_emp_master_feedback360 where ema_perno ='" + Session("USER_ID").ToString + "' and trunc(ema_step1_stdt) <= trunc(sysdate)"
            ocmd.CommandText += "  and trunc(ema_step1_enddt) >= trunc(sysdate)  AND EMA_YEAR='" + ViewState("FY").ToString + "' AND EMA_CYCLE='" + ViewState("SRLNO").ToString + "' and ema_perno not in (select ee_pno from hrps.t_emp_excluded  where ee_cl='" + ViewState("SRLNO").ToString + "' and ee_year='" + ViewState("FY").ToString + "')"
            Dim vc = getRecordInDt(ocmd, conHrps)
            If vc.Rows.Count > 0 Then
                isvalid = True
            Else
                isvalid = False
            End If
        Catch ex As Exception

        End Try
        Return isvalid
    End Function
    Public Function PageValid() As Boolean
        Dim isvalid As Boolean = False
        Try
            Dim ocmd As New OracleCommand()
            ocmd.CommandText = "select IRC_CODE from t_ir_codes where irc_type ='360PG' and trunc(IRC_START_DT) <= trunc(sysdate)"
            ocmd.CommandText += "  and trunc(IRC_END_DT) >= trunc(sysdate) and IRC_VALID_TAG='A' and upper(irc_desc)='SELECTASSESOR_OPR.ASPX'"
            Dim vc = getRecordInDt(ocmd, conHrps)
            If vc.Rows.Count > 0 Then
                isvalid = True
            Else
                isvalid = False
            End If
        Catch ex As Exception

        End Try
        Return isvalid
    End Function
    'Added by TCS on 17112022
    Public Function isSelecetedManagerLevelValid(ByVal managerLevel As String) As Boolean
        Dim isValid As Boolean = False
        Dim sessionUserLevel As String
        Try
            'Added by TCS on 10122023, To bypass this check for perno eqv level is IX
            If managerLevel = "IX" Then
                Return True
            End If
            'End
            sessionUserLevel = Convert.ToString(Session("label"))
            'Added by TCS on 12112025, Including I2 & I3 also for this check
            If sessionUserLevel.Equals("I2") Or sessionUserLevel.Equals("I3") Or sessionUserLevel.Equals("I4") Or sessionUserLevel.Equals("I5") Or sessionUserLevel.Equals("I6") Then
                If Convert.ToInt32(sessionUserLevel.Remove(0, 1)) > Convert.ToInt32(managerLevel.Remove(0, 1)) Then
                    isValid = True
                Else
                    isValid = False
                End If
            Else
                isValid = True
            End If
        Catch ex As Exception

        End Try
        Return isValid
    End Function
    Public Function isManagerAutoPopulated(ByVal managerPerno As String) As Boolean
        Dim isTrue As Boolean = False
        Try
            Dim cmd As New OracleCommand()
            'cmd.CommandText = "SELECT EMA_PERNO FROM T_EMP_MASTER_FEEDBACK360 WHERE EMA_YEAR = :YEAR AND EMA_CYCLE = :CYCLE AND EMA_PERNO = :PERNO AND (EMA_REPORTING_TO_PNO = :MANAGER OR EMA_DOTTED_PNO = :MANAGER OR EMA_PERS_EXEC_PNO = :MANAGER)"
            cmd.CommandText = "SELECT EMA_PERNO FROM T_EMP_MASTER_FEEDBACK360 WHERE EMA_YEAR = :YEAR AND EMA_CYCLE = :CYCLE AND EMA_PERNO = :PERNO AND EMA_REPORTING_TO_PNO = :MANAGER"
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("YEAR", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("CYCLE", ViewState("SRLNO").ToString)
            cmd.Parameters.AddWithValue("PERNO", Session("USER_ID").ToString())
            cmd.Parameters.AddWithValue("MANAGER", managerPerno)
            Dim dt = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                isTrue = True
            Else
                isTrue = False
            End If
        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Public Function isINTSHSelectionValid(ByVal intshPerno As String) As Boolean
        Dim isTrue As Boolean = False
        Dim totalSelected, intshLevel As String
        Dim maxINTSHSelectionCount As Integer
        Dim cmd As New OracleCommand()
        Try
            cmd = New OracleCommand
            'Commented & Added by TCS on 091222, Change the query workflow status added
            'cmd.CommandText = "SELECT COUNT(*) FROM T_SURVEY_STATUS WHERE SS_CATEG = 'INTSH' AND SS_PNO =:SS_PNO AND SS_YEAR=:SS_YEAR AND SS_SRLNO=:SS_SRLNO AND SS_WFL_STATUS='1'"
            cmd.CommandText = "SELECT COUNT(*) FROM T_SURVEY_STATUS WHERE SS_CATEG = 'INTSH' AND SS_PNO =:SS_PNO AND SS_YEAR=:SS_YEAR AND SS_SRLNO=:SS_SRLNO AND SS_WFL_STATUS in ('1','2','3')"
            'End
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_PNO", intshPerno)
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString)
            Dim dt = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                totalSelected = Convert.ToString(dt.Rows(0)(0))
            Else
                totalSelected = "0"
            End If
            intshLevel = ChkAuthlabel(intshPerno)
            If Not String.IsNullOrEmpty(intshLevel) Then
                maxINTSHSelectionCount = getMaxINTSHIdentifiedCount(intshLevel)
                If Convert.ToInt32(totalSelected) < maxINTSHSelectionCount Then
                    isTrue = True
                Else
                    isTrue = False
                End If
            Else
                isTrue = True
            End If
        Catch ex As Exception

        End Try
        Return isTrue
    End Function
    Public Function getMaxINTSHIdentifiedCount(ByVal level As String) As Integer
        Dim count As Integer
        Try
            Dim cmd As New OracleCommand()
            cmd.CommandText = "select IRC_DESC from t_ir_codes where irc_type ='360IS' AND IRC_CODE = :IRC_CODE AND IRC_VALID_TAG='A'"
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("IRC_CODE", level)
            Dim dt = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                If IsNumeric(Convert.ToString(dt.Rows(0)(0))) Then
                    count = Convert.ToInt32(dt.Rows(0)(0))
                Else
                    count = 0
                End If
            Else
                count = 0
            End If
        Catch ex As Exception

        End Try
        Return count
    End Function
    Public Function checkINTSHMaxSelection() As String
        Dim errorMsg As String = String.Empty
        Dim cmd As New OracleCommand()
        Try
            cmd = New OracleCommand()
            cmd.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
            cmd.CommandText += "  and SS_TYPE = 'ORG' AND SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            cmd.Parameters.AddWithValue("SS_CATEG", "INTSH")
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Connection = conHrps
            Dim dtls As New DataTable()
            Dim da As New OracleDataAdapter(cmd)
            da.Fill(dtls)
            If dtls.Rows.Count > 0 Then
                For Each dr As DataRow In dtls.Rows
                    If Not isINTSHSelectionValid(dr("SS_PNO")) Then
                        errorMsg += dr("SS_NAME") & " (" & dr("SS_PNO") & "), "
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
        Return errorMsg.TrimEnd(",")
    End Function
    Public Function managerAutoPopulatedCount(ByVal query As String) As Integer
        Dim count As Integer
        Try
            Dim cmd As New OracleCommand()
            cmd.CommandText = query
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            Dim dt = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                count = dt.Rows.Count
            Else
                count = 0
            End If
        Catch ex As Exception

        End Try
        Return count
    End Function
    Public Function isUncheckedManagerBelongtoPeer(ByVal managerPerno As String) As Boolean
        Dim isTrue As Boolean = False
        Dim cmd As New OracleCommand()
        Try

            If ViewState("type") = "TSL" Then
                cmd.CommandText += " select  ema_perno, ema_ename,EMA_EMPL_SGRADE EMA_EMPL_PGRADE,EMA_DESGN_DESC,EMA_DEPT_DESC,EMA_EMAIL_ID,'ORG' SSTYPE,'' CATEG_SHORT,'' CATEG_FULL   from hrps.t_emp_master_feedback360 where ema_reporting_to_pno "
                cmd.CommandText += " =(select ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:ema_perno and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO ) and ema_year=:SS_YEAR and ema_cycle=:SS_SRLNO and EMA_EQV_LEVEL=:EMA_EQV_LEVEL"
                cmd.CommandText += " and ema_perno=:managerPerno"

                cmd.Parameters.Clear()
                cmd.Connection = conHrps
                cmd.Parameters.AddWithValue("ema_perno", Session("USER_ID").ToString())
                cmd.Parameters.AddWithValue("managerPerno", managerPerno)
                cmd.Parameters.AddWithValue("EMA_EQV_LEVEL", Session("label").ToString())
                cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
                cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
                Dim dtpeer = getDataInDt(cmd)
                If dtpeer.Rows.Count > 0 Then
                    isTrue = True
                End If

            End If
        Catch ex As Exception

        End Try
        Return isTrue
    End Function

    Public Function IsSkipManagerSelectionValid(ByVal skipMgrPerno As String) As Boolean
        Dim cmd As New OracleCommand()
        Dim skipManagerValid As String = String.Empty
        Dim isValid As Boolean = False

        Try
            cmd.Connection = conHrps
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("manager_pno", skipMgrPerno)
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString)

            'cmd.CommandText = "SELECT CASE WHEN COUNT(DISTINCT ts.ss_asses_pno) >= 2 THEN 'N' ELSE 'Y' END AS status FROM t_survey_status ts WHERE ts.ss_year = :SS_YEAR AND ts.ss_categ = 'MANGR' AND ts.ss_srlno = :SS_SRLNO AND ts.ss_pno = :manager_pno AND ts.SS_WFL_STATUS in ('1','2','3') AND ts.ss_asses_pno NOT IN (SELECT ema_perno FROM hrps.t_emp_master_feedback360 f_direct WHERE f_direct.ema_year = :SS_YEAR AND f_direct.ema_cycle = :SS_SRLNO AND f_direct.ema_reporting_to_pno = :manager_pno) AND EXISTS (SELECT 1 FROM (SELECT f2.ema_perno FROM hrps.t_emp_master_feedback360 f2 START WITH f2.ema_reporting_to_pno = :manager_pno CONNECT BY NOCYCLE PRIOR f2.ema_perno = f2.ema_reporting_to_pno AND f2.ema_year = :SS_YEAR AND f2.ema_cycle = :SS_SRLNO) hierarchy WHERE hierarchy.ema_perno = ts.ss_asses_pno)"
            cmd.CommandText = "SELECT CASE WHEN COUNT(DISTINCT ts.ss_asses_pno) >= 5 THEN 'N' ELSE 'Y' END AS status FROM t_survey_status ts WHERE ts.ss_year = :SS_YEAR AND ts.ss_categ = 'MANGR' AND ts.ss_srlno = :SS_SRLNO AND ts.ss_pno = :manager_pno AND ts.SS_WFL_STATUS in ('1','2','3') AND ts.ss_asses_pno NOT IN (SELECT ema_perno FROM hrps.t_emp_master_feedback360 f_direct WHERE f_direct.ema_year = :SS_YEAR AND f_direct.ema_cycle = :SS_SRLNO AND (f_direct.ema_reporting_to_pno = :manager_pno or f_direct.EMA_DOTTED_PNO  = :manager_pno))"

            Dim dt As DataTable = getDataInDt(cmd)
            If dt.Rows.Count > 0 Then
                skipManagerValid = Convert.ToString(dt.Rows(0)(0))
            End If

            If skipManagerValid = "Y" Then
                isValid = True
            Else
                isValid = False
            End If

        Catch ex As Exception
            isValid = False
        End Try
        Return isValid
    End Function
    Public Function checkSkipMAnagerMaxSelection() As String
        Dim errorMsg As String = String.Empty
        Dim cmd As New OracleCommand()
        Try
            cmd = New OracleCommand()
            cmd.CommandText = " select * from t_survey_status where SS_ASSES_PNO =:SS_ASSES_PNO and upper(SS_CATEG) = :SS_CATEG and SS_STATUS='SE' "
            cmd.CommandText += "  and SS_TYPE = 'ORG' AND SS_YEAR=:SS_YEAR and nvl(SS_DEL_TAG,'N')='N' and SS_SRLNO=:SS_SRLNO AND NOT EXISTS (SELECT 1 FROM hrps.t_emp_master_feedback360 WHERE ema_year = SS_YEAR AND EMA_CYCLE=SS_SRLNO AND ema_perno = SS_ASSES_PNO AND ema_reporting_to_pno = SS_PNO)"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("SS_SRLNO", ViewState("SRLNO").ToString())
            cmd.Parameters.AddWithValue("SS_ASSES_PNO", Session("USER_ID").ToString())
            cmd.Parameters.AddWithValue("SS_CATEG", "MANGR")
            cmd.Parameters.AddWithValue("SS_YEAR", ViewState("FY").ToString())
            cmd.Connection = conHrps
            Dim dtls As New DataTable()
            Dim da As New OracleDataAdapter(cmd)
            da.Fill(dtls)
            If dtls.Rows.Count > 0 Then
                For Each dr As DataRow In dtls.Rows
                    If Not IsSkipManagerSelectionValid(dr("SS_PNO")) Then
                        errorMsg += dr("SS_NAME") & " (" & dr("SS_PNO") & "), "
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
        Return errorMsg.TrimEnd(",")
    End Function

    'End
    <Services.WebMethod(EnableSession:=True)>
    Public Shared Function ResetSession() As Integer
        HttpContext.Current.Session("Reset") = True
        Dim timeout As Integer = GetSessionTimeout()
        Return timeout
    End Function
    Private Shared Function GetSessionTimeout() As Integer
        Dim config As System.Configuration.Configuration = Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/Web.Config")
        Dim section As Web.Configuration.SessionStateSection = CType(config.GetSection("system.web/sessionState"), Web.Configuration.SessionStateSection)
        Return Convert.ToInt32(section.Timeout.TotalMinutes * 1000 * 60)
    End Function
End Class
