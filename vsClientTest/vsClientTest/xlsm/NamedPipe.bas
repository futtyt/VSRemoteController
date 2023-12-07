Attribute VB_Name = "NamedPipe"
Option Explicit

Const CREATE_NEW = 1
Const CREATE_ALWAYS = 2
Const OPEN_EXISTING = 3
Const OPEN_ALWAYS = 4
Const TRUNCATE_EXISTING = 5

'   These are the generic rights.

Const GENERIC_READ = &H80000000
Const GENERIC_WRITE = &H40000000
Const GENERIC_EXECUTE = &H20000000
Const GENERIC_ALL = &H10000000

Const FILE_ATTRIBUTE_READONLY = &H1
Const FILE_ATTRIBUTE_HIDDEN = &H2
Const FILE_ATTRIBUTE_SYSTEM = &H4
Const FILE_ATTRIBUTE_DIRECTORY = &H10
Const FILE_ATTRIBUTE_ARCHIVE = &H20
Const FILE_ATTRIBUTE_NORMAL = &H80
Const FILE_ATTRIBUTE_TEMPORARY = &H100
Const FILE_ATTRIBUTE_COMPRESSED = &H800

Const INVALID_HANDLE_VALUE = -1

Type SECURITY_ATTRIBUTES
        nLength As Long
        lpSecurityDescriptor As Long
        bInheritHandle As Long
End Type

Declare Function CreateFile Lib "kernel32" Alias "CreateFileA" (ByVal lpFileName As String, ByVal dwDesiredAccess As Long, ByVal dwShareMode As Long, ByVal lpSecurityAttributes As Long, ByVal dwCreationDisposition As Long, ByVal dwFlagsAndAttributes As Long, ByVal hTemplateFile As Long) As Long
Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long
Declare Function GetLastError Lib "kernel32" () As Long
Declare Function WriteFile Lib "kernel32" (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToWrite As Long, lpNumberOfBytesWritten As Long, ByVal lpOverlapped As Long) As Long
Declare Function ReadFile Lib "kernel32" (ByVal hFile As Long, lpBuffer As Any, ByVal nNumberOfBytesToRead As Long, lpNumberOfBytesRead As Long, ByVal lpOverlapped As Long) As Long

Function ReadMessageLength(h As Long) As Long
    Dim rlen As Long
    
    Dim bwbuflen(4) As Byte
    Call ReadFile(h, bwbuflen(0), 1, rlen, 0)
    Call ReadFile(h, bwbuflen(1), 1, rlen, 0)
    Call ReadFile(h, bwbuflen(2), 1, rlen, 0)
    Call ReadFile(h, bwbuflen(3), 1, rlen, 0)
    
    Dim mlen As Long
    mlen = CLng(bwbuflen(0)) * (2 ^ 24)
    mlen = mlen + CLng(bwbuflen(1)) * (2 ^ 16)
    mlen = mlen + CLng(bwbuflen(2)) * (2 ^ 8)
    mlen = mlen + CLng(bwbuflen(3))

    ReadMessageLength = mlen
End Function

Function WriteMessageLength(h As Long, wlen As Long) As Long
    
    Dim wlenBE(3) As Byte
    wlenBE(0) = CByte(wlen / (2 ^ 24))
    wlenBE(1) = CByte(wlen / (2 ^ 16))
    wlenBE(2) = CByte(wlen / (2 ^ 8))
    wlenBE(3) = CByte(wlen)
    
    Dim wwlen As Long
    Call WriteFile(h, wlenBE(0), 4, wwlen, 0)
    
    WriteMessageLength = wwlen
End Function

Function WriteMessage(h As Long, mes As String) As Boolean
    
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

Function ReadMessage(h As Long) As String
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


Function SendCommand(server As String, cmd As String) As Boolean

    Dim hPipe As Long
    hPipe = CreateFile("\\.\pipe\" & server, GENERIC_READ Or GENERIC_WRITE, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0)
    If hPipe = INVALID_HANDLE_VALUE Then
        SendCommand = False
        Exit Function
    End If
    
    If WriteMessage(hPipe, cmd) = False Then
        SendCommand = False
        Exit Function
    End If
    
    MsgBox ReadMessage(hPipe)
    
    Call CloseHandle(hPipe)

    SendCommand = True

End Function


Sub ƒ{ƒ^ƒ“1_Click()

    Dim server As String
    server = "CommandMonitoring.Server"

    Dim cmd As String
    cmd = "StoepMonitor"
    
    Call SendCommand(server, cmd)
    
End Sub
