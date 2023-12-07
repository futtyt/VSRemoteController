VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "NamedPipe"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Private server As String

Public Property Let ServerName(v As String)
    server = v
End Property

Private Function ReadBytes(h As Long, buf() As Byte, rlen As Long) As Boolean
    If rlen < 1 Then
        ReadBytes = False
        Exit Function
    End If
    
    ReDim buf(rlen - 1)
    
    Dim i As Long
    For i = 0 To rlen - 1
        Dim rrlen As Long
        If ReadFile(h, buf(i), 1, rrlen, 0) = 0 Then
            ReadBytes = False
            Exit Function
        ElseIf rrlen <> 1 Then
            ReadBytes = False
            Exit Function
        End If
    Next
    
    ReadBytes = True
    
End Function

Private Function ReadMessageLength(h As Long) As Long
    Dim rbuf() As Byte
    If ReadBytes(h, rbuf, 4) = False Then
        ReadMessageLength = -1
        Exit Function
    End If
    
    ReadMessageLength = CnvByteArrayToLong(ReverseByteArray(rbuf))

End Function

Private Function WriteMessageLength(h As Long, wlen As Long) As Long
    Dim wlenBE() As Byte
    wlenBE = CnvLongToByteArray(wlen)
    
    Dim wwlen As Long
    Call WriteFile(h, wlenBE(0), 4, wwlen, 0)
    
    WriteMessageLength = wwlen

End Function

Private Function WriteMessage(h As Long, mes As String) As Boolean
    Dim wbuf() As Byte
    wbuf = mes
    
    Dim wlen As Long
    wlen = UBound(wbuf) + 1
    
    Call WriteMessageLength(h, wlen)

    Dim wwlen As Long
    Call WriteFile(h, wbuf(0), wlen, wwlen, 0)

    If wlen = wwlen Then
        WriteMessage = True
    Else
        WriteMessage = False
    End If

End Function

Private Function ReadMessage(h As Long) As String
    Dim rlen As Long
    rlen = ReadMessageLength(h)
    If rlen <= 0 Then
        ReadMessage = ""
        Exit Function
    End If
    
    Dim rbuf() As Byte
    ReDim rbuf(rlen - 1)
    
    Dim rrlen As Long
    If ReadFile(h, rbuf(0), rlen, rrlen, 0) = 0 Then
        ReadMessage = ""
        Exit Function
    End If
    
    ReadMessage = rbuf

End Function

Public Function SendCommand(cmd As String) As String
    If server = "" Then
        SendCommand = "Error" & vbTab & "ServerName is empty."
        GoTo ErrorSendCommand
    End If

    Dim hPipe As Long
    hPipe = CreateFile("\\.\pipe\" & server, GENERIC_READ Or GENERIC_WRITE, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0)
    If hPipe = INVALID_HANDLE_VALUE Then
        SendCommand = "Error" & vbTab & "Couldn't acsess """ & server & """."
        GoTo ErrorSendCommand
    End If
    
    If WriteMessage(hPipe, cmd) = False Then
        SendCommand = "Error" & vbTab & "Couldn't write message to """ & server & """."
        GoTo ErrorSendCommand
    End If
    
    SendCommand = ReadMessage(hPipe)
    
ErrorSendCommand:
    Call CloseHandle(hPipe)

End Function

