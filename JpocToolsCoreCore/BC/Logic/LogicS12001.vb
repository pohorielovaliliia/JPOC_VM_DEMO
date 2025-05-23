Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions

Public Class LogicS12001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property Dto As DtoS12001
        Get
            Return CType(MyBase.mDto, DtoS12001)
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDBManager As ElsDataBase, _
                   ByRef pDto As AbstractDto)
        MyBase.New(pDBManager, pDto)
    End Sub
#End Region

#Region "初期化"
    Public Overrides Sub Init()
        Try

        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "Dispose"
    Public Shadows Sub Dispose()
        MyBase.Dispose()
    End Sub
#End Region

#Region "入力値チェック"
    Public Sub CheckDate()
        Try
            If String.Empty = Me.Dto.InstallDate Then
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "インストール日")
                Exit Sub
            End If
            If String.Empty = Me.Dto.MACAddress Then
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "MACアドレス")
                Exit Sub
            End If
            If Not Regex.IsMatch(Me.Dto.InstallDate, "^\d{4}/\d{2}/\d{2}$") Then
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "インストール日はyyyy/mm/ddで入力してください。")
                Exit Sub
            End If
            If Not IsDate(Me.Dto.InstallDate) Then
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "認識できないインストール日です。")
                Exit Sub
            End If
            If Not Regex.IsMatch(Me.Dto.MACAddress.ToLower, "^(([0-9]|[a-f]){2}:){5}([0-9]|[a-f]){2}$") Then
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "MACアドレスはxx:xx:xx:xx:xx:xxで入力してください。")
                Exit Sub
            End If
            Me.Dto.RtnCD = Common.PublicEnum.eRtnCD.Normal
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "シリアル番号生成"
    Sub CreateSerial()
        Try
            Dim original() As Byte
            Dim InstallID As Byte() = New Byte(15) {}
            Dim encrypted() As Byte

            ' IV&鍵は128ビット(16バイト)である必要がある
            Dim key() As Byte = Encoding.ASCII.GetBytes(GlobalVariables.TripleDesKey)
            Dim iv() As Byte = Encoding.ASCII.GetBytes(GlobalVariables.TripleDesIv & GlobalVariables.TripleDesIv)

            original = Encoding.ASCII.GetBytes(Convert.ToDateTime(Me.Dto.InstallDate.Trim).ToString("yyyyMMdd"))
            Dim MACString() As String = Me.Dto.MACAddress.Split(CChar(":"))
            'For i = 0 To MACString.Length - 1
            '    InstallID(i) = CByte(Convert.ToByte(MACString(i), 16))
            'Next
            'Array.Copy(original, 0, InstallID, 6, original.Length)

            'UNDONE: 発行するシリアル番号は「M(1の位)dd:XXXXXX:y(西暦の1の位)MMdd」で変換を行う
            Dim MACByte As Byte() = New Byte(5) {}
            For i As Integer = 0 To MACString.Length - 1
                MACByte(i) = CByte(Convert.ToByte(MACString(i), 16))
            Next

            InstallID(0) = original(5)
            InstallID(1) = original(6)
            InstallID(2) = original(7)
            InstallID(3) = Encoding.ASCII.GetBytes(":"c)(0)
            Array.Copy(MACByte, 0, InstallID, 4, MACString.Length)
            InstallID(10) = Encoding.ASCII.GetBytes(":"c)(0)
            InstallID(11) = original(3)
            InstallID(12) = original(4)
            InstallID(13) = original(5)
            InstallID(14) = original(6)
            InstallID(15) = original(7)

            Dim bf As Blowfish = New Blowfish(key)
            encrypted = bf.Encrypt_ECB(InstallID)
            Dim result As New StringBuilder()
            For Each b As Byte In encrypted
                result.Append(b.ToString("X2"))
            Next
            Me.Dto.SerialNumber = result.ToString
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
