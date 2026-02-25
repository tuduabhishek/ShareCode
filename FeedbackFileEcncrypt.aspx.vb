Imports System.Data.OracleClient
Imports System.Data
Imports System.IO
Imports System.Security.Cryptography
Imports System.Net

Partial Class FeedbackFileEcncrypt
    Inherits System.Web.UI.Page
    Dim conHrps As New OracleConnection(ConfigurationManager.ConnectionStrings("MYhrps").ConnectionString)
    Dim cntPeer As Integer
    Dim cntSub As Integer
    Dim cntSelf As Integer
    Dim cntMgr As Integer
    Dim cntStkholder As Integer
    Dim cntMinFullFill As Integer
    'Public networkPath As String = "\\{Your IP or Folder Name of Network}\Shared Data"
    'Private credentials As NetworkCredential = New NetworkCredential("{User Name}", "{Password}")
    'Public networkPath As String = "\\10.162.190.247"
    Public networkPath As String = "sftp22-ams.sumtotalsystems.com"
    ' Private credentials As NetworkCredential = New NetworkCredential("cust_tatasteel", "HB3tN=-5$)", "\\10.162.190.247\CandidateDocuments\CP")
    Private credentials As NetworkCredential = New NetworkCredential("cust_tatasteel", "HB3tN=-5$)", "sftp22-ams.sumtotalsystems.com")
    Private Sub FeedbackFileEcncrypt_Init(sender As Object, e As EventArgs) Handles Me.Init
        loadLoggedInUserIDAndDomainIntoSession()
    End Sub
    Private Sub FeedbackRpt1_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If ChkRole() Then
                GetFy()
                getScore()
            End If
        End If
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "KeyGenericMessageModal", "screenshot('02-03-2021');", True)
    End Sub
    Public Function ChkRole() As Boolean
        Try
            Dim t As Boolean = False
            Dim cmd As String
            cmd = "select * from t_ir_adm_grp_privilege where igp_group_id ='360FEEDBAC' "
            cmd += " and IGP_STATUS ='A' and IGP_user_id='" & Session("ADM_USER").ToString() & "'"
            Dim f = GetData(cmd, conHrps)
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

        If Session("ADM_USER") = "" Then

            Dim vUserFullName As String = Page.User.Identity.Name   '155710'

            Dim arrUserIDParts As String() = vUserFullName.Split("\")
            If arrUserIDParts.Length <> 1 Then
                strUserID = arrUserIDParts(1)
            End If
            'strUserID = "151629"
            Session("ADM_USER") = strUserID.ToUpper()
            If GetPno(strUserID.ToUpper()) = False Then
                Session("errorMsg") = "You don't have admin role."
                Response.Redirect("errorpage.aspx", True)
            End If
            'lblname.Text = "Suresh Dutt Tripathi"
        ElseIf (Session("ADM_USER") IsNot Nothing) AndAlso (Session("ADM_USER").Equals("") = False) Then
            ' lblname.Text = GetPno().Rows(0)(1) '"Suresh Dutt Tripathi"
            Return
        Else
        End If
    End Sub
    Public Function GetPno(pno As String) As Boolean
        Try
            Dim d As New Boolean
            Dim q As String
            q = "Select IGP_user_id,ema_ename from t_ir_adm_grp_privilege,TIPS.t_empl_all where igp_group_id ='360FEEDBAC'  and IGP_STATUS ='A' "
            q += "  and ema_perno=IGP_user_id and IGP_user_id='" & pno.ToString() & "' and EMA_COMP_CODE='1000'"
            Dim p = GetData(q, conHrps)
            If p.Rows.Count > 0 Then
                d = True
            Else
                d = False

            End If
            Return d
        Catch ex As Exception

        End Try
    End Function
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
    Public Sub GetFy()
        Try
            Dim s As New OracleCommand()
            's.CommandText = "SELECT EXTRACT (YEAR FROM ADD_MONTHS (sysdate, 9)) as CURR_FY FROM DUAL"
            s.CommandText = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'"
            Dim f = getRecordInDt(s, conHrps)

            If f.Rows.Count > 0 Then
                ViewState("FY") = f.Rows(0)(0)
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
            Throw ex
        Finally
            If cn.State = ConnectionState.Open Then
                cn.Close()
            End If
        End Try
        Return dt
    End Function

    Public Sub getScore()
        Try
            Dim Q1grade As String = String.Empty
            Dim Q2grade As String = String.Empty
            Dim Q3grade As String = String.Empty
            Dim Q4grade As String = String.Empty

            Dim qry As String = String.Empty
            'qry = "select decode(upper(ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager',upper('PEER'),'Peers',upper('intsh') "
            'qry += ",'Internal stakeholders',upper('ropt'),'People you lead') ss_categ,sum(ss_q1_a),sum(ss_q1_b),sum(ss_q1_c),sum(ss_q1_d),round(sum(ss_q1_a)/count(*),3) A,"
            'qry += "round(sum(ss_q1_b)/count(*),3) B,round(sum(ss_q1_c)/count(*),3) C,round(sum(ss_q1_d)/count(*),3) D"
            'qry += ",count(*) No_Records,upper(ss_categ) categ,ss_asses_pno from t_survey_status where ss_wfl_status ='3' and ss_year='" & ViewState("FY") & "' and ss_status = 'SE' and ss_del_tag = 'N' and ss_app_tag = 'AP' group by ss_categ,ss_asses_pno order by 1,2"
            qry = "select decode(upper(vw.ss_categ),upper('Self'),'Self',upper('MANGR'),'Manager',upper('PEER'),'Peers',upper('intsh') ,'Internal stakeholders',upper('ropt'),'People you lead') ss_categ,vw.A,vw.C,vw.R,vw.T,upper(vw.ss_categ) categ,vw.EMA_PERNO ss_asses_pno from hrps.v_all_i1_i6_t vw,hrps.t_emp_master_feedback360 emp where vw.ema_perno=emp.ema_perno and vw.ss_year=emp.ema_year and vw.ss_srlno=emp.ema_cycle and vw.ss_year='" & ViewState("FY") & "' and vw.ss_srlno='2' and vw.ss_categ<>'Z-OVERALL' and emp.ema_eqv_level in ('I3','I4','I5','I6')"
            Dim re As String = String.Empty
            Dim cont As Integer = 0
            Dim d = GetData(qry, conHrps)

            Dim categoryTable As New DataTable
            categoryTable.Columns.Add("EmployeeId", GetType(String))
            categoryTable.Columns.Add("ItmTxt", GetType(String))
            categoryTable.Columns.Add("Self", GetType(String))
            categoryTable.Columns.Add("Manager1", GetType(String))
            categoryTable.Columns.Add("Peer", GetType(String))
            'categoryTable.Columns.Add("Subordinate", GetType(String))
            categoryTable.Columns.Add("Internal Stakeholder", GetType(String))
            categoryTable.Columns.Add("Over All", GetType(String))

            Dim distinctPerno As New DataTable

            Dim scoreTable As New DataTable

            ' Create four typed columns in the DataTable.
            scoreTable.Columns.Add("EmployeeId", GetType(String))
            scoreTable.Columns.Add("Category", GetType(String))
            scoreTable.Columns.Add("Accountability", GetType(String))
            scoreTable.Columns.Add("Collaboration", GetType(String))
            scoreTable.Columns.Add("Responsiveness", GetType(String))
            scoreTable.Columns.Add("People Development", GetType(String))


            If Not d Is Nothing Then

                'Dim viewDistinct As New DataView(d)
                distinctPerno = d.DefaultView.ToTable(True, "ss_asses_pno")

                For i As Integer = 0 To distinctPerno.Rows.Count - 1
                    cntPeer = 0
                    cntSub = 0
                    cntMgr = 0
                    cntSelf = 0
                    cntStkholder = 0

                    'Self
                    Dim self() As DataRow = d.Select("categ = 'SELF' and ss_asses_pno = '" + distinctPerno.Rows(i)("ss_asses_pno").ToString + "'")

                    ' Loop and display.
                    For Each SelfRow As DataRow In self
                        If SelfRow(5) = "SELF" Then
                            cntSelf = 1
                        End If
                        Q1grade = GetScoreGrade(SelfRow(1))
                        Q2grade = GetScoreGrade(SelfRow(2))
                        Q3grade = GetScoreGrade(SelfRow(3))
                        Q4grade = GetScoreGrade(SelfRow(4))
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, SelfRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                    Next

                    If cntSelf = 0 Then
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, "Self", "0", "0", "0", "0")
                    End If





                    'Manager
                    Dim manager() As DataRow = d.Select("categ = 'MANGR' and ss_asses_pno = '" + distinctPerno.Rows(i)("ss_asses_pno").ToString + "'")

                    ' Loop and display.
                    For Each managerRow As DataRow In manager
                        If managerRow(5) = "MANGR" Then
                            cntMgr = 1
                        End If
                        Q1grade = GetScoreGrade(managerRow(1))
                        Q2grade = GetScoreGrade(managerRow(2))
                        Q3grade = GetScoreGrade(managerRow(3))
                        Q4grade = GetScoreGrade(managerRow(4))
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, managerRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                    Next

                    If cntMgr = 0 Then
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, "Manager", "0", "0", "0", "0")
                    End If




                    'Peer
                    Dim Peers() As DataRow = d.Select("categ = 'PEER' and ss_asses_pno = '" + distinctPerno.Rows(i)("ss_asses_pno").ToString + "'")
                    For Each PeersRow As DataRow In Peers
                        If PeersRow(5) = "PEER" Then
                            cntPeer = 1
                        End If
                        Q1grade = GetScoreGrade(PeersRow(1))
                        Q2grade = GetScoreGrade(PeersRow(2))
                        Q3grade = GetScoreGrade(PeersRow(3))
                        Q4grade = GetScoreGrade(PeersRow(4))
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, PeersRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                    Next

                    If cntPeer = 0 Then
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, "Peers", "0", "0", "0", "0")
                    End If



                    'Internal Stakeholder
                    Dim intsh() As DataRow = d.Select("categ = 'INTSH' and ss_asses_pno = '" + distinctPerno.Rows(i)("ss_asses_pno").ToString + "'")

                    ' Loop and display.
                    For Each intshRow As DataRow In intsh
                        If intshRow(5) = "INTSH" Then
                            cntStkholder = 1
                        End If
                        Q1grade = GetScoreGrade(intshRow(1))
                        Q2grade = GetScoreGrade(intshRow(2))
                        Q3grade = GetScoreGrade(intshRow(3))
                        Q4grade = GetScoreGrade(intshRow(4))
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, intshRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                    Next

                    If cntStkholder = 0 Then
                        scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, "Internal stakeholders", "0", "0", "0", "0")
                    End If



                    'Overall
                    Dim sql1 As String
                    'sql1 = "select ac.ss_asses_pno, round(sum(a)/count(*),3) A, round(sum(b)/count(*),3) B,round(sum(c)/count(*),3) C,round(sum(d)/count(*),3) D from  (SELECT ss_asses_pno,ss_categ,round(SUM(ss_q1_a) / COUNT(*),3) a,round(SUM(ss_q1_b) / COUNT(*),3) b,round(SUM(ss_q1_c) / COUNT(*),3) c,round(SUM(ss_q1_d) / COUNT(*),3) d, COUNT(*) no_records FROM hrps.t_survey_status WHERE ss_wfl_status = '3' AND ss_year = '" & ViewState("FY") & "' and ss_asses_pno='" + distinctPerno.Rows(i)("ss_asses_pno").ToString + "' AND ss_status = 'SE' AND ss_del_tag = 'N' AND ss_app_tag = 'AP' AND upper(ss_categ) <> 'SELF' GROUP BY ss_asses_pno,ss_categ) ac group by ac.ss_asses_pno "
                    sql1 = "select EMA_PERNO,A,C,R,T from hrps.v_all_i1_i6_t where EMA_PERNO='" + distinctPerno.Rows(i)("ss_asses_pno").ToString + "' AND ss_year = '" & ViewState("FY") & "' and ss_srlno='2' and ss_categ='Z-OVERALL'"
                    Dim dt4 = GetData(sql1, conHrps)
                    Q1grade = GetScoreGrade(dt4.Rows(0)("A").ToString)
                    Q2grade = GetScoreGrade(dt4.Rows(0)("C").ToString)
                    Q3grade = GetScoreGrade(dt4.Rows(0)("R").ToString)
                    Q4grade = GetScoreGrade(dt4.Rows(0)("T").ToString)
                    scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, "Overall", Q1grade, Q2grade, Q3grade, Q4grade)







                    ''Subordinate
                    'Dim Subs() As DataRow = d.Select("categ = 'ROPT' and ss_asses_pno = '" + distinctPerno.Rows(i)("ss_asses_pno").ToString + "'")
                    'For Each SubsRow As DataRow In Subs
                    '    If SubsRow(5) = "ROPT" Then
                    '        cntSub = 1
                    '    End If
                    '    Q1grade = GetScoreGrade(SubsRow(1))
                    '    Q2grade = GetScoreGrade(SubsRow(2))
                    '    Q3grade = GetScoreGrade(SubsRow(3))
                    '    Q4grade = GetScoreGrade(SubsRow(4))
                    '    scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, SubsRow(0), Q1grade, Q2grade, Q3grade, Q4grade)
                    'Next
                    '' End If

                    'If cntSub = 0 Then
                    '    scoreTable.Rows.Add(distinctPerno.Rows(i)("ss_asses_pno").ToString, "People you lead", "0", "0", "0", "0")
                    'End If









                Next
                Dim addRow As Integer
                addRow = 0
                If scoreTable.Rows.Count > 0 Then
                    For j As Integer = 0 To scoreTable.Rows.Count - 1
                        'addRow = addRow + 4
                        For c As Integer = 2 To 5
                            If c = 2 Then
                                categoryTable.Rows.Add(scoreTable.Rows(j)(0).ToString, "Accountability", scoreTable.Rows(j)(c).ToString, scoreTable.Rows(j + 1)(c).ToString, scoreTable.Rows(j + 2)(c).ToString, scoreTable.Rows(j + 3)(c).ToString, scoreTable.Rows(j + 4)(c).ToString) ', scoreTable.Rows(j + 5)(c).ToString)
                            End If
                            If c = 3 Then
                                categoryTable.Rows.Add(scoreTable.Rows(j)(0).ToString, "Collaboration", scoreTable.Rows(j)(c).ToString, scoreTable.Rows(j + 1)(c).ToString, scoreTable.Rows(j + 2)(c).ToString, scoreTable.Rows(j + 3)(c).ToString, scoreTable.Rows(j + 4)(c).ToString) ', scoreTable.Rows(j + 5)(c).ToString)
                            End If
                            If c = 4 Then
                                categoryTable.Rows.Add(scoreTable.Rows(j)(0).ToString, "Responsiveness", scoreTable.Rows(j)(c).ToString, scoreTable.Rows(j + 1)(c).ToString, scoreTable.Rows(j + 2)(c).ToString, scoreTable.Rows(j + 3)(c).ToString, scoreTable.Rows(j + 4)(c).ToString) ', scoreTable.Rows(j + 5)(c).ToString)
                            End If
                            If c = 5 Then
                                categoryTable.Rows.Add(scoreTable.Rows(j)(0).ToString, "People Development", scoreTable.Rows(j)(c).ToString, scoreTable.Rows(j + 1)(c).ToString, scoreTable.Rows(j + 2)(c).ToString, scoreTable.Rows(j + 3)(c).ToString, scoreTable.Rows(j + 4)(c).ToString) ', scoreTable.Rows(j + 5)(c).ToString)
                            End If
                        Next
                        j = j + 4
                    Next
                End If



                Dim txt As String
                Dim encryptTxt As String
                Dim fileloc1 As String = "D:\Encrypt.txt"
                'Dim fileloc As String = "D:\Result.txt"
                Dim fileloc As String = Server.MapPath("~/Images/Encrypt.txt")
                Dim line1 As String = ""
                line1 = "EmployeeId,ItmTxt,Self,Manager1,PeerSubordinate,InternalStakeholder,Overall"
                For Each row As DataRow In categoryTable.Rows
                    Dim line As String = ""
                    For Each column As DataColumn In categoryTable.Columns
                        'Add the Data rows.
                        If column.ColumnName = "EmployeeId" Then
                            'line += getSHA1Hash(row(column.ColumnName).ToString())
                            line += row(column.ColumnName).ToString()
                            'encryptTxt += getSHA1Hash(row(column.ColumnName).ToString()) & vbCrLf
                        Else
                            line += "," & row(column.ColumnName).ToString()
                            'encryptTxt += "," & row(column.ColumnName).ToString()
                        End If
                        'line += vbTab & row(column.ColumnName).ToString()
                    Next
                    'Add new line
                    ' encryptTxt += getSHA1Hash(line.Substring(1)) & vbCrLf
                    'encryptTxt += encryptTxt & vbCrLf

                    txt += line & vbCrLf

                Next
                'encryptTxt = line1 & vbCrLf & encryptTxt
                'encryptTxt = line1 & vbCrLf & encryptTxt
                txt = line1 & vbCrLf & txt


                'Using New NetworkConnection(networkPath, credentials)
                '    Dim fileList = Directory.GetDirectories("sftp22-ams.sumtotalsystems.com")

                '    'For Each item In fileList
                '    '    If item.Contains(ddlDocument.SelectedValue.ToString) Then
                '    '        myNetworkPath = item
                '    '    End If
                '    'Next
                '    'myNetworkPath = myNetworkPath
                'End Using

                '''FTP Server URL.
                ''Dim ftp As String = "sftp22-ams.sumtotalsystems.com"

                '''FTP Folder name. Leave blank if you want to list files from root folder.
                ''Dim ftpFolder As String = "/BehaviorsData/"

                '''Create FTP Request.
                ''Dim request As FtpWebRequest = DirectCast(WebRequest.Create(ftp & ftpFolder), FtpWebRequest)
                ''    request.Method = WebRequestMethods.Ftp.ListDirectoryDetails

                '''Enter FTP Server credentials.
                ''request.Credentials = New NetworkCredential("cust_tatasteel", "HB3tN=-5$)")
                ''request.UsePassive = True
                ''    request.UseBinary = True
                ''    request.EnableSsl = False

                '''Fetch the Response and read it using StreamReader.
                ''Dim response1 As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)



                ''Dim ftpRequest As FtpWebRequest = DirectCast(WebRequest.Create("www.sumtotalsystems.com"), FtpWebRequest)
                ''ftpRequest.Credentials = New NetworkCredential("cust_tatasteel", "HB3tN=-5$)")
                ''ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory
                ''Dim response As FtpWebResponse = DirectCast(ftpRequest.GetResponse(), FtpWebResponse)
                ''Dim streamReader As New StreamReader(response.GetResponseStream())
                ''Dim directories As New List(Of String)()

                ''Dim rdline As String = streamReader.ReadLine()
                ''While Not String.IsNullOrEmpty(rdline)
                ''    directories.Add(rdline)
                ''    rdline = streamReader.ReadLine()
                ''End While
                ''streamReader.Close()


                ''Using ftpClient As New WebClient()
                ''    ftpClient.Credentials = New System.Net.NetworkCredential("cust_tatasteel", "HB3tN=-5$)")

                ''    For i As Integer = 0 To directories.Count - 1
                ''        If directories(i).Contains(".") Then

                ''            Dim path As String = "sftp22-ams.sumtotalsystems.com/" + directories(i).ToString()
                ''            Dim trnsfrpth As String = "D:\\Test\" + directories(i).ToString()
                ''            ftpClient.DownloadFile(path, trnsfrpth)
                ''        End If
                ''    Next
                ''End Using


                ' If File.Exists(fileloc) Then
                Using sw As StreamWriter = New StreamWriter(fileloc)
                    sw.WriteLine(txt)
                End Using

                'Using sw1 As StreamWriter = New StreamWriter(fileloc1)
                '    sw1.WriteLine(encryptTxt)
                'End Using
                ''    End If

                'Dim filePath As String = Server.MapPath("~/Images/Encrypt")
                'Response.ContentType = ContentType
                'Response.AppendHeader("Content-Disposition", ("attachment; filename=Encrypt.txt"))
                'Response.WriteFile(filePath + ".txt")
                'Response.End()

                Response.ContentType = ContentType
                Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(Server.MapPath("~\Images\Encrypt.txt"))))
                Response.WriteFile(Server.MapPath("~\Images\Encrypt.txt"))
                Response.Flush()
                Threading.Thread.Sleep(2000)
                Dim pathout1 = Server.MapPath("~\Images\")

                'Dim di As System.IO.DirectoryInfo = New DirectoryInfo(pathout1)
                'For Each file As FileInfo In di.GetFiles()
                '    file.Delete()
                'Next
                Dim di As System.IO.DirectoryInfo = New DirectoryInfo(pathout1)
                For Each file As FileInfo In di.GetFiles()
                    If file.Name = "Encrypt.txt" Then
                        file.Delete()
                    End If

                Next
                MsgBox("ok")

            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString())
        End Try
    End Sub

    Function getSHA1Hash(ByVal strToHash As String) As String

        Dim sha1Obj As New SHA1CryptoServiceProvider
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)

        bytesToHash = sha1Obj.ComputeHash(bytesToHash)

        Dim strResult As String = ""

        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next

        Return strResult

    End Function
    'Public Shared Function WriteCSV(ByVal input As String) As String
    '    Try
    '        If (input Is Nothing) Then
    '            Return String.Empty
    '        End If

    '        Dim containsQuote As Boolean = False
    '        Dim containsComma As Boolean = False
    '        Dim len As Integer = input.Length
    '        Dim i As Integer = 0
    '        Do While ((i < len) _
    '                    AndAlso ((containsComma = False) _
    '                    OrElse (containsQuote = False)))
    '            Dim ch As Char = input(i)
    '            If (ch = Microsoft.VisualBasic.ChrW(34)) Then
    '                containsQuote = True
    '            ElseIf (ch = Microsoft.VisualBasic.ChrW(44)) Then
    '                containsComma = True
    '            End If

    '            i = (i + 1)
    '        Loop

    '        If (containsQuote AndAlso containsComma) Then
    '            input = input.Replace(""""", ", """"")", If, (containsCommaUnknown, """"" + input + ", "")
    '        Else
    '            Return input
    '        End If

    '    Catch ex As System.Exception
    '        MsgBox(ex.Message.ToString())
    '    End Try

    'End Function
    Public Function GetScoreGrade(scoreval As String) As String
        Try
            Dim grade As String = ""
            If Val(scoreval) <= Val(1.6) Then
                grade = "1"
            ElseIf Val(scoreval) <= Val(2.6) And Val(scoreval) > Val(1.6) Then
                grade = "2"
            ElseIf Val(scoreval) <= Val(3) And Val(scoreval) > Val(2.6) Then
                grade = "3"
            End If
            Return grade
        Catch ex As Exception

        End Try

    End Function
End Class
