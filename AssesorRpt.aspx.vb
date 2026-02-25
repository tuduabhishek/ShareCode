Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Drawing

Partial Class AssesorRpt
    Inherits System.Web.UI.Page

    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)

    Private Sub AssesorRpt_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetFy()
        getresCoupt("148497")
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

    Public Sub getresCoupt(pno As String)
        Try
            Dim qry As String = String.Empty
            qry = "select SS_CATEG,irc_desc ,count(SS_CATEG) from t_survey_status,t_ir_codes  where ss_asses_pno='" & pno & "' and ss_app_tag='AP' "
            qry += "and upper(ss_categ)=upper(irc_code) and irc_type='360RL' and ss_year='" & ViewState("FY") & "' group by SS_CATEG,irc_desc"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim qry1 As String = String.Empty
            qry1 = "select ss_pno from t_survey_status  where ss_asses_pno ='" & pno & "'  and ss_wfl_status='3' and ss_year='" & ViewState("FY") & "' "
            'qry1 += "and upper(ss_categ)=upper(irc_code) and irc_type='360RL' and ss_year='" & ViewState("FY") & "' group by SS_CATEG,irc_desc"
            Dim re1 As String = String.Empty
            Dim cont1 As Integer = 0
            Dim d1 = GetData(qry1, conHrps)

            If d.Rows.Count > 0 Then
                For g = 0 To d.Rows.Count - 1
                    re += d.Rows(g)(1) & " - " & d.Rows(g)(2) & ", "
                    cont = cont + d.Rows(g)(2)
                Next
                re = re.Trim
                re = re.TrimEnd(",")
                lblnor.Text = "Your behaviour score is based on the responses of <b>" & cont & "individuals(" & re & ")</b> <br/><br/>A total of <b>" & cont
                lblnor.Text += "</b> surveys were distributed. <b>" & d1.Rows.Count & "</b> surveys were completed and have been included in this feedback report."
            End If
        Catch ex As Exception
            MsgBox("error")
        End Try
    End Sub
    Public Sub GetFy()
        Try
            Dim s As String = String.Empty
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, -3))  as CURR_FY FROM DUAL"
            Dim f = GetData(s, conHrps)

            If f.Rows.Count > 0 Then
                ViewState("FY") = f.Rows(0)(0)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
