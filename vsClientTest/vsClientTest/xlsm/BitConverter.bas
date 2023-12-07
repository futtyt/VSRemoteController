Attribute VB_Name = "BitConverter"
Option Explicit

Public Function CnvByteArrayToLong(src() As Byte) As Long
    Dim dst As Long
    
    If UBound(src) < 4 - 1 Then
        CnvByteArrayToLong = 0
        Exit Function
    End If
    
    dst = 0
    Dim i As Long
    For i = 0 To 4 - 1
        dst = dst + CLng(src(i) * (2 ^ (i * 8)))
    Next
    
    CnvByteArrayToLong = dst

End Function


Public Function CnvLongToByteArray(src As Long) As Byte()
    Dim dst() As Byte
    ReDim dst(4 - 1)
    
    Dim i As Long
    For i = 0 To 4 - 1
        dst(i) = CByte(Int(src / (2 ^ ((4 - 1 - i) * 8))))
    Next
    
    CnvLongToByteArray = dst

End Function


Public Function ReverseByteArray(src() As Byte) As Byte()
    Dim dst() As Byte
    
    Dim l As Long
    l = UBound(src)
    ReDim dst(l)
    
    Dim i As Long
    For i = 0 To l
        dst(i) = src(l - i)
    Next
    
    ReverseByteArray = dst

End Function



