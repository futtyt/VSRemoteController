Attribute VB_Name = "UserEventHandler"
Option Explicit

Sub ƒ{ƒ^ƒ“1_Click()
    Dim server As String
    server = "CommandMonitoring.Server"

    Dim cmd As String
    cmd = "StopeMonitor"
    
    Dim np As NamedPipe
    Set np = New NamedPipe
    
    np.ServerName = server
    ShowMessage np.SendCommand(ActiveSheet.Cells(3, 3).value & vbTab & ActiveSheet.Cells(3, 4))
    Set np = Nothing
    
End Sub

Public Sub ShowMessage(mes As String)
    Dim mess() As String
    mess = Split(mes, vbTab)
    
    Dim b As Long
    Select Case mess(0)
    Case "Success"
        MsgBox mess(1), vbInformation
    Case "Error"
        MsgBox mess(1), vbCritical
    Case Else
        MsgBox mess(1), vbQuestion
    End Select
End Sub


