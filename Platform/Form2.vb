Public Class Form2
    Dim labels(8), cLabel(8) As Label
    Dim pic, pic2, tp1 As Bitmap
    Dim cardLen, cardType(200), cardBelong(200) As Integer
    Dim cardName() As String
    Dim conName(20) As String
    Dim conLen, conValue(20) As Integer
    Dim counName(200) As String
    Dim counLen, counOf(200), counX(200), counY(200) As Integer
    Dim conn(200, 200) As Boolean
    Dim progStat(8), whose, statTot, statNow As Integer
    Dim counBelong(200), counArmy(200) As Integer
    Dim Colors(8) As Brush
    Dim NumFont As New Font("Times New Roman", 10)
    Dim rand As New Random
    Dim startArmy, tradeNum(1000), tradeNow, getArmy As Integer
    Dim mustTrade, callBack, getCard As Boolean
    Dim attFrom, attTo, lastAtt As Integer
    Dim drawListX(100, 30000), drawListY(100, 30000), dlLen(100) As Integer
    Dim firstTime As Boolean
    Const debug As Boolean = False
    Dim delay As Integer = 100
    Dim draw1, draw2 As Integer
    Dim tPen As Pen
    Dim tColor As Color

    Private Sub Form2_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Terminate()
        End
    End Sub

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tColor = Color.FromArgb(128, 255, 255, 255)
        tPen = New Pen(tColor, 1)
        RadioButton1.Checked = True
        labels(0) = New Label
        labels(1) = Label1
        labels(2) = Label2
        labels(3) = Label3
        labels(4) = Label4
        labels(5) = Label9
        labels(6) = Label10
        labels(7) = Label11
        labels(8) = Label12
        cLabel(0) = New Label
        cLabel(1) = Label5
        cLabel(2) = Label6
        cLabel(3) = Label7
        cLabel(4) = Label8
        cLabel(5) = Label13
        cLabel(6) = Label14
        cLabel(7) = Label15
        cLabel(8) = Label16
        cardName = Split("a Cavalry Cannon Infantry wildcard")
        Colors(1) = Brushes.Red
        Colors(2) = Brushes.Blue
        Colors(3) = Brushes.Fuchsia
        Colors(4) = Brushes.Green
        Colors(5) = Brushes.Yellow
        Colors(6) = Brushes.LightBlue
        Colors(7) = Brushes.Black
        Colors(8) = Brushes.White
        tradeNum(1) = 4
        tradeNum(2) = 6
        tradeNum(3) = 8
        tradeNum(4) = 10
        tradeNum(5) = 12
        For i = 6 To 1000
            tradeNum(i) = (i - 3) * 5
        Next
        For i = 1 To 8
            If Form1.texts(i).Text <> "" Then
                progName(i) = getName(Form1.texts(i).Text)
                labels(i).Text = progName(i)
                runProg(Form1.texts(i).Text, i)
                progStat(i) = 1
                startArmy += 1
            Else
                progStat(i) = 0
                labels(i).Text = "不存在"
            End If
        Next
        ValidWatch.Enabled = True
        mapPath = getPath(Form1.TextBox5.Text)
        Dim ts, ta() As String
        Dim ti1, ti2 As Integer
        FileOpen(100, Form1.TextBox5.Text, OpenMode.Input)
        While (1)
            ts = LineInput(100)
            If ts = "[files]" Then Exit While
        End While
        While (1)
            ts = LineInput(100)
            If ts = "[continents]" Then Exit While
            If Len(ts) > 3 Then
                Select Case Microsoft.VisualBasic.Left(ts, 3)
                    Case "pic"
                        pic = New Bitmap(mapPath + Trim(Microsoft.VisualBasic.Mid(ts, 5)), False)
                        pic2 = New Bitmap(677, 425)
                        For i = 0 To 675
                            For j = 0 To 424
                                pic2.SetPixel(i, j, pic.GetPixel(i, j))
                            Next
                        Next
                    Case "map"
                        tp1 = Bitmap.FromFile(mapPath + Trim(Microsoft.VisualBasic.Mid(ts, 5)))
                        For i = 0 To tp1.Width - 1
                            For j = 0 To tp1.Height - 1
                                If tp1.GetPixel(i, j).R <> 255 Then
                                    ti1 = tp1.GetPixel(i, j).R
                                    If tp1.GetPixel(i, j).R <> tp1.GetPixel(i, j).G Or tp1.GetPixel(i, j).R <> tp1.GetPixel(i, j).B Then MsgBox("")
                                    dlLen(ti1) += 1
                                    drawListX(ti1, dlLen(ti1)) = i
                                    drawListY(ti1, dlLen(ti1)) = j
                                End If
                            Next
                        Next
                    Case "crd"
                        FileOpen(200, mapPath + Trim(Microsoft.VisualBasic.Mid(ts, 5)), OpenMode.Input)
                        While (1)
                            ts = LineInput(200)
                            If ts = "[cards]" Then Exit While
                        End While
                        While Not EOF(200)
                            ts = LineInput(200)
                            If InStr(ts, "[") Then Exit While
                            If Len(ts) > 5 Then
                                If Microsoft.VisualBasic.Left(ts, 1) <> ";" Then
                                    cardLen += 1
                                    Select Case Split(ts)(0)
                                        Case "Cavalry"
                                            cardType(cardLen) = 1
                                        Case "Cannon"
                                            cardType(cardLen) = 2
                                        Case "Infantry"
                                            cardType(cardLen) = 3
                                        Case "wildcard"
                                            cardType(cardLen) = 4
                                    End Select

                                End If
                            End If
                        End While
                        FileClose(200)
                End Select
            End If
        End While
        While (1)
            ts = LineInput(100)
            If ts = "[countries]" Then Exit While
            If Len(ts) > 3 Then
                ta = Split(ts)
                conLen += 1
                conName(conLen) = ta(0)
                conValue(conLen) = CInt(ta(1))
            End If
        End While
        While (1)
            ts = LineInput(100)
            If ts = "[borders]" Then Exit While
            If Len(ts) > 5 Then
                ta = Split(ts)
                counLen += 1
                counName(counLen) = ta(1)
                counOf(counLen) = CInt(ta(2))
                counX(counLen) = CInt(ta(3))
                counY(counLen) = CInt(ta(4))
            End If
        End While
        While Not EOF(100)
            ts = LineInput(100)
            If Len(ts) > 1 Then
                ta = Split(ts)
                ti1 = CInt(ta(0))
                For i = 1 To UBound(ta)
                    If ta(i) <> "" Then
                        ti2 = CInt(ta(i))
                        conn(ti1, ti2) = True
                    End If
                Next
            End If
        End While
        FileClose(100)
        startArmy = (10 - startArmy) * Int(Math.Round(0.12 * counLen))
        Timer1.Enabled = True
    End Sub

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint
        Dim g As Graphics = e.Graphics
        g.DrawImage(pic2, 0, 0)
        'Dim tp2 As New Bitmap(677, 425)
        If draw1 > 0 Then
            For i = 1 To dlLen(draw1)
                'tp2.SetPixel(drawListX(draw1, i), drawListY(draw1, i), tColor)
                g.DrawLine(tPen, drawListX(draw1, i), drawListY(draw1, i), drawListX(draw1, i) + 1, drawListY(draw1, i) + 1)
            Next
        End If
        If draw2 > 0 Then
            For i = 1 To dlLen(draw2)
                'tp2.SetPixel(drawListX(draw2, i), drawListY(draw2, i), tColor)
                g.DrawLine(tPen, drawListX(draw2, i), drawListY(draw2, i), drawListX(draw2, i) + 1, drawListY(draw2, i) + 1)
            Next
        End If
        'g.DrawImage(tp2, 0, 0)
        draw1 = 0
        draw2 = 0
        For i = 1 To counLen
            If counArmy(i) > 0 Then
                g.FillEllipse(Colors(counBelong(i)), counX(i) - 8, counY(i) - 8, 16, 16)
                g.DrawArc(Pens.Black, counX(i) - 8, counY(i) - 8, 16, 16, 0, 360)
                If counBelong(i) <> 5 And counBelong(i) <> 8 And counBelong(i) <> 6 Then
                    g.DrawString(Trim(Str(counArmy(i))), NumFont, Brushes.White, counX(i) - 6, counY(i) - 6)
                Else
                    g.DrawString(Trim(Str(counArmy(i))), NumFont, Brushes.Black, counX(i) - 6, counY(i) - 6)
                End If
            End If
        Next
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        For i = 1 To 8
            If progStat(i) = 1 Then
                stdOut(i).WriteLine("[continents]")
                stdOut(i).WriteLine(conLen)
                For j = 1 To conLen
                    stdOut(i).WriteLine(Trim(Str(j)) + " " + conName(j) + " " + Trim(Str(conValue(j))))
                Next
                stdOut(i).WriteLine("[countries]")
                stdOut(i).WriteLine(counLen)
                For j = 1 To counLen
                    stdOut(i).WriteLine(Trim(Str(j)) + " " + counName(j) + " " + Trim(Str(counOf(j))))
                Next
                stdOut(i).WriteLine("[borders]")
                stdOut(i).WriteLine(counLen)
                For j = 1 To counLen
                    stdOut(i).Write(j)
                    For k = 1 To counLen
                        If conn(j, k) Then stdOut(i).Write(" " + Trim(Str(k)))
                    Next
                    stdOut(i).WriteLine()
                Next
            End If
        Next
        whose = 1
        Timer2.Enabled = True
    End Sub

    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        Timer2.Enabled = False
start:
        statNow = 1
        If progStat(whose) = 1 Then
            showArmies()
            stdOut(whose).WriteLine("[place]")
            stdOut(whose).WriteLine("1")
            Timer3.Enabled = True
            Timer4.Enabled = True
        Else
            whose += 1
            If whose > 8 Then whose = 1
            GoTo start
        End If
    End Sub

    Sub showArmies()
        stdOut(whose).WriteLine("[showarmies]")
        stdOut(whose).WriteLine(counLen)
        For i = 1 To counLen
            stdOut(whose).WriteLine(Trim(Str(i)) + " " + IIf(counArmy(i) = 0, "*", Trim(labels(counBelong(i)).Text)) + " " + Trim(Str(counArmy(i))))
        Next
    End Sub

    Sub showArmies2()
        For j = 1 To 8
            If progStat(j) = 1 Then
                stdOut(j).WriteLine("[showarmies]")
                stdOut(j).WriteLine(counLen)
                For i = 1 To counLen
                    stdOut(j).WriteLine(Trim(Str(i)) + " " + IIf(counArmy(i) = 0, "*", Trim(labels(counBelong(i)).Text)) + " " + Trim(Str(counArmy(i))))
                Next
            End If
        Next
    End Sub

    Sub showCards()
        Dim tot As Integer
        If progStat(whose) <> 1 Then Exit Sub
        For i = 1 To cardLen
            If cardBelong(i) = whose Then tot += 1
        Next
        stdOut(whose).WriteLine("[showcards]")
        stdOut(whose).WriteLine(tot)
        cLabel(whose).Text = ""
        For i = 1 To cardLen
            If cardBelong(i) = whose Then
                stdOut(whose).WriteLine(cardName(cardType(i)) + " " + Trim(Str(i)))
                cLabel(whose).Text += " " + Trim(Str(i))
            End If
        Next
    End Sub

    Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
        Timer3.Enabled = False
        Dim ts, ta() As String
        Dim lLen, list(200), rnd As Integer
        'On Error Resume Next
res:
        If progStat(whose) <> 1 Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        labels(whose).BackColor = Color.Yellow
        Application.DoEvents()
        ts = stdOut(whose).ReadLine
        Timer4.Enabled = False
        ta = Split(ts)
        If ta(0) = "autoplace" Then
            For i = 1 To counLen
                If counArmy(i) = 0 Then
                    lLen += 1
                    list(lLen) = i
                End If
            Next
            rnd = rand.Next(1, lLen + 1)
            counArmy(list(rnd)) = 1
            counBelong(list(rnd)) = whose
            draw1 = list(rnd)
        ElseIf ta(0) = "placearmies" Then
            If CInt(ta(1)) > 0 And CInt(ta(1)) <= counLen Then
                If counBelong(CInt(ta(1))) = 0 And CInt(ta(2)) = 1 Then
                    counArmy(CInt(ta(1))) += CInt(ta(2))
                    counBelong(CInt(ta(1))) = whose
                    draw1 = CInt(ta(1))
                Else
                    GoTo err
                End If
            Else
                GoTo err
            End If
        Else
err:
            proc(whose).CloseMainWindow()
            MsgBox(whose & "输出错误")
            progStat(whose) = 2
            labels(whose).ForeColor = Color.Black
            whose += 1
            If whose > 8 Then whose = 1
            Timer2.Enabled = True
            Exit Sub
        End If
        Threading.Thread.Sleep(100)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        whose += 1
        If whose > 8 Then whose = 1
        For i = 1 To counLen
            If counArmy(i) = 0 Then GoTo ok2
        Next
        statTot = 2
        For i = 1 To 8
            If progStat(i) = 1 Then
                stdOut(i).WriteLine("[armiesmore]")
            End If
        Next
        Timer5.Enabled = True
        Exit Sub
ok2:
        Timer2.Enabled = True
    End Sub

    Private Sub Timer4_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer4.Tick
        Timer4.Enabled = False
        If debug Then Exit Sub
        proc(whose).CloseMainWindow()
        MsgBox(whose & "超时")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        Timer2.Enabled = True
    End Sub

    Private Sub ValidWatch_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ValidWatch.Tick
        If debug Then Exit Sub
        For i = 1 To 8
            If progStat(i) = 1 Then Exit Sub
        Next
        End
    End Sub

    Private Sub Timer5_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer5.Tick
        Timer5.Enabled = False
        Dim ts, ta() As String
        Dim count(8) As Integer
        'On Error Resume Next
        For i = 1 To counLen
            If counArmy(i) <> 0 Then count(counBelong(i)) += counArmy(i)
        Next
        For i = 1 To 8
            If progStat(i) = 1 And count(i) < startArmy Then GoTo res
        Next
        whose = 1
        callBack = False
        getCard = False
        getArmy = 0
        Timer7.Enabled = True
        firstTime = True
        Exit Sub
res:
        If progStat(whose) <> 1 Or count(whose) >= startArmy Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        showArmies()
        labels(whose).BackColor = Color.Yellow
        Application.DoEvents()
        stdOut(whose).WriteLine("[place]")
        stdOut(whose).WriteLine(startArmy - count(whose))
        Timer6.Enabled = True
        ts = stdOut(whose).ReadLine
        Timer6.Enabled = False
        ta = Split(ts)
        If ta(0) = "placearmies" Then
            If CInt(ta(1)) > 0 And CInt(ta(1)) <= counLen Then
                If counBelong(CInt(ta(1))) = whose And CInt(ta(2)) <= startArmy - count(whose) Then
                    counArmy(CInt(ta(1))) += CInt(ta(2))
                    draw1 = CInt(ta(1))
                    GoTo ok2
                End If
            End If
        End If
        proc(whose).CloseMainWindow()
        MsgBox(whose & "输出错误")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        Timer5.Enabled = True
        Exit Sub
ok2:
        Threading.Thread.Sleep(delay)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        whose += 1
        If whose > 8 Then whose = 1
        Timer5.Enabled = True
    End Sub

    Private Sub Timer6_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer6.Tick
        Timer6.Enabled = False
        If debug Then Exit Sub
        proc(whose).CloseMainWindow()
        MsgBox(whose & "超时")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        Timer5.Enabled = True
    End Sub

    Private Sub Timer7_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer7.Tick
        Timer7.Enabled = False
        If firstTime Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo out
            Next
            End
out:
            If progStat(whose) <> 1 Then
                whose += 1
                If whose > 8 Then whose = 1
                GoTo out
            End If
            For i = 1 To counLen
                If counBelong(i) = whose Then getArmy += 1
            Next
            getArmy /= 3
            If getArmy < 3 Then getArmy = 3
            Dim conCtrl(20) As Integer
            For i = 1 To counLen
                If conCtrl(counBelong(i)) <> 0 Then
                    conCtrl(counBelong(i)) = -1
                Else
                    conCtrl(counBelong(i)) = counBelong(i)
                End If
            Next
            For i = 1 To conLen
                If conCtrl(i) = whose Then getArmy += conValue(i)
            Next
        End If
        Dim cLen(8), tot As Integer
        For i = 1 To cardLen
            If cardBelong(i) = whose Then
                cLen(cardType(i)) += 1
                tot += 1
            End If
        Next
        If IIf(cLen(1) = 0, 0, 1) + IIf(cLen(2) = 0, 0, 1) + IIf(cLen(3) = 0, 0, 1) + cLen(4) >= 3 Then
            GoTo ok
        End If
        For i = 1 To 3
            If cLen(i) + cLen(4) >= 3 Then
                GoTo ok
            End If
        Next
        Timer10.Enabled = True
        Exit Sub
ok:
        mustTrade = False
        If callBack = False And tot >= 5 Then mustTrade = True
        If callBack = True Then
            callBack = False
            If tot >= 6 Then mustTrade = True
        End If
        Timer8.Enabled = True
        Timer9.Enabled = True
    End Sub

    Private Sub Timer8_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer8.Tick
        Timer8.Enabled = False
        Dim ts, ta() As String
        Dim ti1 As Integer
        'On Error Resume Next
res:
        If progStat(whose) <> 1 Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        labels(whose).BackColor = Color.Yellow
        'showArmies()
        showCards()
        stdOut(whose).WriteLine("[trade]")
        ts = stdOut(whose).ReadLine
        Timer9.Enabled = False
        ta = Split(ts)
        If ta(0) = "endtrade" Then
            If Not mustTrade Then
                Timer10.Enabled = True
            End If
        End If
        If ta(0) = "trade" And UBound(ta) = 3 Then
            Dim cLen(8), tot, sp As Integer
            For i = 1 To 3
                If Trim(Str(ti1)) = "wildcard" Then
                    sp += 1
                    If sp > cardLen Then GoTo err
                    For j = sp To cardLen
                        If cardBelong(j) = whose And cardType(j) = 4 Then
                            ta(i) = Trim(Str(j))
                            sp = j + 1
                            GoTo ok3
                        End If
                    Next
                    GoTo err
ok3:
                End If
                ti1 = CInt(ta(i))
                If ti1 > 0 And ti1 <= cardLen Then
                    If cardBelong(ti1) = whose Then
                        cLen(cardType(ti1)) += 1
                        tot += 1
                    End If
                End If
            Next
            If ta(1) = ta(2) Or ta(2) = ta(3) Or ta(1) = ta(3) Then GoTo err
            If tot = 3 Then
                If IIf(cLen(1) = 0, 0, 1) + IIf(cLen(2) = 0, 0, 1) + IIf(cLen(3) = 0, 0, 1) + cLen(4) >= 3 Then GoTo change
                For i = 1 To 3
                    If cLen(i) + cLen(4) = 3 Then GoTo change
                Next
                GoTo err
change:
                For i = 1 To 3
                    cardBelong(CInt(ta(i))) = 0
                    'If counBelong(CInt(ta(i))) = whose Then counArmy(CInt(ta(i))) += 2
                Next
                tradeNow += 1
                getArmy += tradeNum(tradeNow)
                showCards()
                firstTime = False
                Timer7.Enabled = True
                Exit Sub
            End If
        End If
err:
        proc(whose).CloseMainWindow()
        MsgBox(whose & "输出错误")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
        Exit Sub
ok2:
        Threading.Thread.Sleep(delay)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        firstTime = False
        Timer7.Enabled = True
    End Sub

    Private Sub Timer9_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer9.Tick
        Timer9.Enabled = False
        If debug Then Exit Sub
        proc(whose).CloseMainWindow()
        MsgBox(whose & "超时")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
    End Sub

    Private Sub Timer10_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer10.Tick
        Timer10.Enabled = False
res:
        If progStat(whose) <> 1 Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        If getArmy > 0 Then
            showArmies()
            stdOut(whose).WriteLine("[place]")
            stdOut(whose).WriteLine(getArmy)
            Timer9.Enabled = True
            Timer12.Enabled = True
            Exit Sub
        End If
        Timer13.Enabled = True
    End Sub

    Private Sub Timer12_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer12.Tick
        Timer12.Enabled = False
        Dim ts, ta() As String
        labels(whose).BackColor = Color.Yellow
        Application.DoEvents()
        ts = stdOut(whose).ReadLine
        Timer9.Enabled = False
        ta = Split(ts)
        If ta(0) = "placearmies" Then
            If CInt(ta(1)) > 0 And CInt(ta(1)) <= counLen Then
                If counBelong(CInt(ta(1))) = whose And CInt(ta(2)) <= getArmy Then
                    counArmy(CInt(ta(1))) += CInt(ta(2))
                    getArmy -= CInt(ta(2))
                    draw1 = CInt(ta(1))
                    GoTo ok2
                End If
            End If
        End If
        proc(whose).CloseMainWindow()
        MsgBox(whose & "输出错误")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        Timer10.Enabled = True
        Exit Sub
ok2:
        Threading.Thread.Sleep(delay)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        Timer10.Enabled = True
    End Sub

    Private Sub Timer13_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer13.Tick
        Timer13.Enabled = False
        Dim ts, ta() As String
        'On Error Resume Next
res:
        If progStat(whose) <> 1 Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        showArmies()
        labels(whose).BackColor = Color.Yellow
        stdOut(whose).WriteLine("[attack]")
        Timer9.Enabled = True
        ts = stdOut(whose).ReadLine
        Timer9.Enabled = False
        ta = Split(ts)
        If ta(0) = "endattack" Then
            Timer18.Enabled = True
            Exit Sub
        End If
        If ta(0) = "attack" Then
            attFrom = CInt(ta(1))
            attTo = CInt(ta(2))
            If attFrom > 0 And attFrom <= counLen And attTo > 0 And attTo <= counLen Then
                If counBelong(attFrom) = whose And counBelong(attTo) <> whose And conn(attFrom, attTo) And counArmy(attFrom) > 1 Then
                    draw1 = attFrom
                    Timer15.Enabled = True
                    Exit Sub
                End If
            End If
        End If
err:
        proc(whose).CloseMainWindow()
        MsgBox(whose & "输出错误")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
        Exit Sub
ok2:
        Threading.Thread.Sleep(delay)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        Timer13.Enabled = True
    End Sub

    Private Sub Timer15_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer15.Tick
        Timer15.Enabled = False
        Dim ts, ta() As String
        Dim ti, ra(3), rb(2) As Integer
        'On Error Resume Next
res:
        If progStat(whose) <> 1 Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        showArmies()
        labels(whose).BackColor = Color.Yellow
        stdOut(whose).WriteLine("[roll]")
        Timer9.Enabled = True
        ts = stdOut(whose).ReadLine
        Timer9.Enabled = False
        ta = Split(ts)
        If ta(0) = "retreat" Then
            draw1 = 0
            Timer13.Enabled = True
            Exit Sub
        End If
        If ta(0) = "roll" Then
            draw1 = attFrom
            draw2 = attTo
            ti = CInt(ta(1))
            lastAtt = ti
            If ti < counArmy(attFrom) Then
                For i = 1 To 3
                    ra(i) = rand.Next(1, 7)
                Next
                rb(1) = rand.Next(1, 7)
                rb(2) = rand.Next(1, 7)
                If ti = 3 And ra(3) > ra(2) Then swap(ra(3), ra(2))
                If ti >= 2 And ra(2) > ra(1) Then swap(ra(2), ra(1))
                If ti = 3 And ra(3) > ra(2) Then swap(ra(3), ra(2))
                If counArmy(attTo) > 1 And rb(2) > rb(1) Then swap(rb(2), rb(1))
                If counArmy(attTo) >= 2 And ti >= 2 Then
                    If ra(2) > rb(2) Then
                        counArmy(attTo) -= 1
                    Else
                        counArmy(attFrom) -= 1
                        lastAtt -= 1
                    End If
                End If
                If ra(1) > rb(1) Then
                    counArmy(attTo) -= 1
                Else
                    counArmy(attFrom) -= 1
                    lastAtt -= 1
                End If
                GoTo ok2
            End If
        End If
err:
        proc(whose).CloseMainWindow()
        MsgBox(whose & "输出错误")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
        Exit Sub
ok2:
        Threading.Thread.Sleep(delay)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        If counArmy(attTo) = 0 Then
            showArmies()
            stdOut(whose).WriteLine("[won]")
            getCard = True
            Timer16.Enabled = True
            Exit Sub
        End If
        If counArmy(attFrom) > 1 Then
            Timer15.Enabled = True
        Else
            Timer13.Enabled = True
        End If
    End Sub

    Private Sub Timer16_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer16.Tick
        Timer16.Enabled = False
        Dim ts, ta() As String
        'On Error Resume Next
res:
        If progStat(whose) <> 1 Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        labels(whose).BackColor = Color.Yellow
        Timer7.Enabled = True
        ts = stdOut(whose).ReadLine
        Timer7.Enabled = False
        ta = Split(ts)
        If ta(0) = "move" Then
            If ta(1) = "all" Then
                counArmy(attTo) = counArmy(attFrom) - 1
                counArmy(attFrom) = 1
                counBelong(attTo) = whose
                GoTo ok2
            ElseIf CInt(ta(1)) >= lastAtt Then
                counArmy(attTo) = CInt(ta(1))
                counArmy(attFrom) -= CInt(ta(1))
                counBelong(attTo) = whose
                GoTo ok2
            End If
        End If
err:
        proc(whose).CloseMainWindow()
        MsgBox(whose & "输出错误")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
        Exit Sub
ok2:
        Threading.Thread.Sleep(delay)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        Dim mark(8) As Boolean
        Dim count As Integer
        For i = 1 To counLen
            mark(counBelong(i)) = True
        Next
        For i = 1 To 8
            If mark(i) = False And progStat(i) = 1 Then
                cLabel(i).Text = ""
                proc(i).CloseMainWindow()
                progStat(i) = 2
                labels(i).ForeColor = Color.Black
                For j = 1 To cardLen
                    If cardBelong(j) = i Then cardBelong(j) = whose
                Next
                For j = 1 To cardLen
                    If cardBelong(j) = whose Then count += 1
                Next
                If count >= 6 Then
                    mustTrade = True
                    callBack = True
                    getArmy = 0
                    Timer8.Enabled = True
                    Timer9.Enabled = True
                    Exit Sub
                End If
            End If
        Next
        Timer13.Enabled = True
    End Sub

    Private Sub Timer19_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer19.Tick
        Timer19.Enabled = False
        If debug Then Exit Sub
        proc(whose).CloseMainWindow()
        MsgBox(whose & "超时")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
    End Sub

    Private Sub Timer18_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer18.Tick
        Timer18.Enabled = False
        Dim ts, ta() As String
        Dim ti1, ti2, ti3 As Integer
        'On Error Resume Next
res:
        If progStat(whose) <> 1 Then
            For i = 1 To 8
                If progStat(i) = 1 Then GoTo ok
            Next
            End
ok:
            whose += 1
            If whose > 8 Then whose = 1
            GoTo res
        End If
        showArmies()
        labels(whose).BackColor = Color.Yellow
        stdOut(whose).WriteLine("[move]")
        Timer19.Enabled = True
        ts = stdOut(whose).ReadLine
        Timer19.Enabled = False
        ta = Split(ts)
        If ta(0) = "nomove" Then
            GoTo ok2
        End If
        If ta(0) = "movearmies" Then
            ti1 = CInt(ta(1))
            ti2 = CInt(ta(2))
            ti3 = CInt(ta(3))
            If ti1 > 0 And ti2 > 0 And ti1 <= counLen And ti2 <= counLen Then
                If ti3 > 0 And ti3 < counArmy(ti1) And counBelong(ti1) = whose And counBelong(ti2) = whose And conn(ti1, ti2) Then
                    counArmy(ti1) -= ti3
                    counArmy(ti2) += ti3
                    draw1 = ti1
                    draw2 = ti2
                    GoTo ok2
                End If
            End If

        End If
err:
        proc(whose).CloseMainWindow()
        MsgBox(whose & "输出错误")
        progStat(whose) = 2
        labels(whose).ForeColor = Color.Black
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
        Exit Sub
ok2:
        Threading.Thread.Sleep(delay)
        labels(whose).BackColor = Color.Transparent
        PictureBox1.Refresh()
        If getCard Then
            Dim cLen, cList(200) As Integer
            For i = 1 To cardLen
                If cardBelong(i) = 0 Then
                    cLen += 1
                    cList(cLen) = i
                End If
            Next
            If cLen > 0 Then
                ti1 = rand.Next(1, cLen + 1)
                cardBelong(ti1) = whose
            End If
        End If
        showCards()
        whose += 1
        If whose > 8 Then whose = 1
        firstTime = True
        getCard = False
        Timer7.Enabled = True
    End Sub

    Sub setTime(ByVal value As Integer)
        Timer3.Interval = value
        Timer5.Interval = value
        Timer7.Interval = value
        Timer8.Interval = value
        Timer10.Interval = value
        Timer12.Interval = value
        Timer13.Interval = value
        Timer15.Interval = value
        Timer16.Interval = value
        Timer18.Interval = value
        delay = value
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        setTime(100)
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        setTime(300)
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        setTime(500)
    End Sub
End Class