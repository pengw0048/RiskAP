Imports System.IO
Public Class StdWriter
    Private stdIn As StreamReader
    Private stdOut As StreamWriter
    Private fn, n As Integer
    Private buf As String
    Public Sub New(ByVal ins As StreamReader, ByVal out As StreamWriter, ByVal name As String)
        stdOut = out
        stdIn = ins
        fn = FreeFile()
        FileOpen(fn, getPath(Application.ExecutablePath) + name + ".txt", OpenMode.Output)
    End Sub
    Public Sub WriteLine(ByRef str As String)
        stdOut.WriteLine(str)
        Print(fn, ">" + str + vbCrLf)
    End Sub
    Public Sub WriteLine(ByVal num As Integer)
        stdOut.WriteLine(num)
        Print(fn, ">" + Trim(Str(num)) + vbCrLf)
    End Sub
    Public Sub Write(ByRef str As String)
        stdOut.Write(str)
        buf += ">" + str
    End Sub
    Public Sub Write(ByVal num As Integer)
        stdOut.Write(num)
        buf += ">" + Trim(Str(num))
    End Sub
    Public Sub WriteLine()
        stdOut.WriteLine()
        Print(fn, buf + vbCrLf)
        buf = ""
    End Sub
    Public Function ReadLine() As String
        buf = stdIn.ReadLine
        Print(fn, "<" + buf + vbCrLf)
        Return buf
    End Function
End Class
