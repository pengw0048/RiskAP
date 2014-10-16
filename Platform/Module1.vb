Imports System.IO
Module Module1
    Public mapPath As String
    Public progName(8) As String
    Public stdOut(8) As StdWriter
    Public proc(8) As Process
    Function getPath(ByVal str As String) As String
        Dim ti1 As Integer
        ti1 = InStrRev(str, "\")
        Return Microsoft.VisualBasic.Left(str, ti1)
    End Function
    Function getName(ByVal str As String) As String
        Dim ti1 As Integer
        Dim ts As String
        ti1 = InStrRev(str, "\")
        ts = Microsoft.VisualBasic.Mid(str, ti1 + 1)
        Return Microsoft.VisualBasic.Left(ts, Len(ts) - 4)
    End Function
    Sub runProg(ByVal str As String, ByVal num As Integer)
        Dim start_info As New ProcessStartInfo(str)
        start_info.UseShellExecute = False
        start_info.RedirectStandardOutput = True
        start_info.RedirectStandardInput = True
        proc(num) = New Process
        proc(num).StartInfo = start_info
        proc(num).Start()
        stdOut(num) = New StdWriter(proc(num).StandardOutput, proc(num).StandardInput, progName(num))
    End Sub
    Sub Terminate()
        On Error Resume Next
        For i = 1 To 8
            proc(i).CloseMainWindow()
        Next
    End Sub
    Sub swap(ByRef a As Integer, ByRef b As Integer)
        Dim t As Integer
        t = a
        a = b
        b = t
    End Sub
End Module
