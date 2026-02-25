Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

''' <summary>
''' Function for implementing AES Encryption-Decryption for any input
''' 
''' Created By: Sohrab Kersi Gandhi
''' Created On: 31 October, 2013 01.15 AM
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class AESEncryption
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Encrypts a string
    ''' </summary>
    ''' <param name="PlainText">Text to be encrypted</param>
    ''' <param name="Password">Password to encrypt with</param>
    ''' <param name="Salt">Salt to encrypt with</param>
    ''' <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
    ''' <param name="PasswordIterations">Number of iterations to do</param>
    ''' <param name="InitialVector">Needs to be 16 ASCII characters long</param>
    ''' <param name="KeySize">Can be 128, 192, or 256</param>
    ''' <returns>An encrypted string</returns>
    Public Shared Function Encrypt(ByVal PlainText As String, ByVal Password As String, Optional ByVal Salt As String = "S0#r@B", Optional ByVal HashAlgorithm As String = "SHA1", Optional ByVal PasswordIterations As Integer = 2, Optional ByVal InitialVector As String = "OFRna73m*aze01xY", _
     Optional ByVal KeySize As Integer = 256) As String
        If String.IsNullOrEmpty(PlainText) Then
            Return ""
        End If
        Dim InitialVectorBytes As Byte() = Encoding.ASCII.GetBytes(InitialVector)
        Dim SaltValueBytes As Byte() = Encoding.ASCII.GetBytes(Salt)
        Dim PlainTextBytes As Byte() = Encoding.UTF8.GetBytes(PlainText)
        Dim DerivedPassword As New PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations)
        Dim KeyBytes As Byte() = DerivedPassword.GetBytes(KeySize / 8)
        Dim SymmetricKey As New RijndaelManaged()
        SymmetricKey.Mode = CipherMode.CBC
        Dim CipherTextBytes As Byte() = Nothing
        Using Encryptor As ICryptoTransform = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes)
            Using MemStream As New MemoryStream()
                Using CryptoStream As New CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write)
                    CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length)
                    CryptoStream.FlushFinalBlock()
                    CipherTextBytes = MemStream.ToArray()
                    MemStream.Close()
                    CryptoStream.Close()
                End Using
            End Using
        End Using
        SymmetricKey.Clear()
        Return Convert.ToBase64String(CipherTextBytes)
    End Function

    ''' <summary>
    ''' Decrypts a string
    ''' </summary>
    ''' <param name="CipherText">Text to be decrypted</param>
    ''' <param name="Password">Password to decrypt with</param>
    ''' <param name="Salt">Salt to decrypt with</param>
    ''' <param name="HashAlgorithm">Can be either SHA1 or MD5</param>
    ''' <param name="PasswordIterations">Number of iterations to do</param>
    ''' <param name="InitialVector">Needs to be 16 ASCII characters long</param>
    ''' <param name="KeySize">Can be 128, 192, or 256</param>
    ''' <returns>A decrypted string</returns>
    Public Shared Function Decrypt(ByVal CipherText As String, ByVal Password As String, Optional ByVal Salt As String = "S0#r@B", Optional ByVal HashAlgorithm As String = "SHA1", Optional ByVal PasswordIterations As Integer = 2, Optional ByVal InitialVector As String = "OFRna73m*aze01xY", _
     Optional ByVal KeySize As Integer = 256) As String
        If String.IsNullOrEmpty(CipherText) Then
            Return ""
        End If
        Dim InitialVectorBytes As Byte() = Encoding.ASCII.GetBytes(InitialVector)
        Dim SaltValueBytes As Byte() = Encoding.ASCII.GetBytes(Salt)
        Dim CipherTextBytes As Byte() = Convert.FromBase64String(CipherText)
        Dim DerivedPassword As New PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations)
        Dim KeyBytes As Byte() = DerivedPassword.GetBytes(KeySize / 8)
        Dim SymmetricKey As New RijndaelManaged()
        SymmetricKey.Mode = CipherMode.CBC
        Dim PlainTextBytes As Byte() = New Byte(CipherTextBytes.Length - 1) {}
        Dim ByteCount As Integer = 0
        Using Decryptor As ICryptoTransform = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes)
            Using MemStream As New MemoryStream(CipherTextBytes)
                Using CryptoStream As New CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read)

                    ByteCount = CryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length)
                    MemStream.Close()
                    CryptoStream.Close()
                End Using
            End Using
        End Using
        SymmetricKey.Clear()
        Return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount)
    End Function
End Class
