Imports Microsoft.VisualBasic

Public Class CommonVb
    Public Function ISI(ByVal value As String) As Boolean
        value = value.Trim()
        If value.Length = 0 Then
            Return False
        End If
        Dim original As String = value.Trim
        value = Replace(value.Trim, "--", "")
        value = Replace(value.Trim, "#", "")
        'value = Replace(value.Trim, "'", "")
        value = Replace(value.Trim, "/*", "")
        value = Replace(value.Trim, "*/", "")
        value = Replace(value.Trim, ";", "")
        value = Replace(value.Trim, "$", "")
        value = Replace(value.Trim, "^", "")
        value = Replace(value.Trim, "<", "")
        value = Replace(value.Trim, ">", "")
        value = Replace(value.Trim, "<!", "")
        value = Replace(value.Trim, "%", "")
        value = Replace(value.Trim, "./", "")

        value = Replace(value.Trim.ToUpper, "UNION", "")
        value = Replace(value.Trim.ToUpper, "DROP", "")
        value = Replace(value.Trim.ToUpper, "DELETE", "")
        value = Replace(value.Trim.ToUpper, "ALTER", "")
        value = Replace(value.Trim.ToUpper, "RENAME", "")
        value = Replace(value.Trim.ToUpper, "UPDATE", "")
        value = Replace(value.Trim.ToUpper, "SELECT", "")
        value = Replace(value.Trim.ToUpper, "RENAME", "")
        value = Replace(value.Trim.ToUpper, "WINDOWS", "")
        value = Replace(value.Trim.ToUpper, "C:\", "")
        value = Replace(value.Trim.ToUpper, "SLEEP", "")

        value = Regex.Replace(value.Trim.ToUpper, "\bUNION\b", "")
        value = Regex.Replace(value.Trim.ToUpper, "\bDROP\b", "")
        value = Regex.Replace(value.Trim.ToUpper, "\bALTER\b", "")
        value = Regex.Replace(value.Trim.ToUpper, "\bRENAME\b", "")
        value = Regex.Replace(value.Trim.ToUpper, "\bUPDATE\b", "")
        value = Regex.Replace(value.Trim.ToUpper, "\bSELECT\b", "")
        value = Regex.Replace(value.Trim.ToUpper, "\bSCRIPT\b", "")
        value = Regex.Replace(value.Trim.ToUpper, "\bHTML\b", "")
        If original.Length <> value.Length Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
