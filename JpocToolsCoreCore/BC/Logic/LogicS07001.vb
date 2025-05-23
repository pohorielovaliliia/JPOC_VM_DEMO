Imports System.IO
Public Class LogicS07001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property Dto As DtoS07001
        Get
            Return CType(MyBase.mDto, DtoS07001)
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
    Public Sub CheckEntry(ByVal pTargetType As String)

        Try
            Select Case pTargetType
                Case "Execute"
                    If String.IsNullOrEmpty(Me.Dto.InstitutionCode) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "施設コード")
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(Me.Dto.LoginID) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "ログインID")
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(Me.Dto.Password) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "パスワード")
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(Me.Dto.SqlCommand) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "コマンド")
                        Exit Sub
                    End If

                    'If Me.Dto.SqlCommand.Contains(";") Then
                    '    If Not Me.Dto.SqlCommand.EndsWith(";") Then
                    '        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    '        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "複数のコマンドは許容していません。")
                    '        Exit Sub
                    '    End If
                    'End If
                Case Else

            End Select
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region
    'TODO: FIX HERE
    '#Region "文字列暗号化"
    '    Public Sub EncryptSendData()
    '        Try
    '            'Webサービスに送るパラメータを3DESで暗号化する
    '            Me.Dto.LoginID = Utilities.Encrypt(Me.Dto.LoginID, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '            Me.Dto.InstitutionCode = Utilities.Encrypt(Me.Dto.InstitutionCode, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '            Me.Dto.Password = Utilities.Encrypt(Me.Dto.Password, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '            Me.Dto.SqlCommand = Utilities.Encrypt(Me.Dto.SqlCommand, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '        Catch ex As ElsException
    '            ex.AddStackFrame(New StackFrame(True))
    '            Throw
    '        Catch ex As Exception
    '            Throw New ElsException(ex)
    '        End Try
    '    End Sub
    '#End Region

#Region "コマンド実行"
    Public Sub ExecuteNonResult()
        Dim sc As JpocWeb.AdminService.AdminServiceSoapClient = Nothing
        Try
            Dim rtnCd As Integer = 0
            Dim rtnMsg As String = String.Empty
            sc = New JpocWeb.AdminService.AdminServiceSoapClient
            '環境に応じたURLを設定する
            sc.Endpoint.Address = GlobalVariables.WebServiceUrlEndpointAddress(Me.Dto.WebServiceName)

            'Webサービスにリクエストを発行する
            Dim ret As Integer = sc.ExecuteNoResult(Me.Dto.LoginID, _
                                                    Me.Dto.InstitutionCode, _
                                                    Me.Dto.Password, _
                                                    Me.Dto.SqlCommand, _
                                                    rtnCd, _
                                                    rtnMsg)
            If rtnCd = 0 Then   'Webサービスが正常に終了した場合
                Me.Dto.MessageSet = Utilities.GetMessageSet("INF0000", "影響を受けたレコード件数：" & ret.ToString)
            Else                'Webサービス側でエラーになった場合
                'こちら側では論理エラーとして扱う
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                '3DESで暗号化されたメッセージを復号する
                'TODO: FIX HERE
                'rtnMsg = Utilities.Decrypt(rtnMsg, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
                'メッセージセットを格納する
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", rtnMsg)
            End If
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            sc.Close()
            sc = Nothing
        End Try
    End Sub
#End Region
    'TODO: FIX HERE
    '#Region "データセット取得コマンド実行"
    '    Public Sub Execute()
    '        Dim sc As JpocWeb.AdminService.AdminServiceSoapClient = Nothing
    '        Dim data As Byte() = Nothing
    '        Try
    '            Dim rtnCd As Integer = 0
    '            Dim rtnMsg As String = String.Empty
    '            sc = New JpocWeb.AdminService.AdminServiceSoapClient
    '            '環境に応じたURLを設定する
    '            sc.Endpoint.Address = GlobalVariables.WebServiceUrlEndpointAddress(Me.Dto.WebServiceName)

    '            data = sc.GetDataToDataSet(Me.Dto.LoginID, _
    '                                       Me.Dto.InstitutionCode, _
    '                                       Me.Dto.Password, _
    '                                       Me.Dto.SqlCommand, _
    '                                       rtnCd, _
    '                                       rtnMsg)
    '            If rtnCd = 0 Then   'Webサービスが正常に終了した場合
    '                '受け取ったデータをGZIP伸張する
    '                data = Utilities.GZipDecompressByte(data)
    '                '伸張したデータを3DESで復号する
    '                data = Utilities.Decrypt(data, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '                'バイト配列をメモリーストリームに変換する
    '                Using ms As MemoryStream = Utilities.ConvertByteToStream(data)
    '                    'メモリーストリームからデシリアライズしてデータセットに読み込む
    '                    Me.Dto.CommonDataSet.ReadXml(ms, XmlReadMode.ReadSchema)
    '                End Using
    '            Else                'Webサービス側でエラーになった場合
    '                'こちら側では論理エラーとして扱う
    '                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
    '                '3DESで暗号化されたメッセージを復号する
    '                rtnMsg = Utilities.Decrypt(rtnMsg, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '                'メッセージセットを格納する
    '                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", rtnMsg)
    '            End If
    '        Catch ex As ElsException
    '            ex.AddStackFrame(New StackFrame(True))
    '            Throw
    '        Catch ex As Exception
    '            Throw New ElsException(ex)
    '        Finally
    '            Erase data
    '            data = Nothing
    '            sc.Close()
    '            sc = Nothing
    '        End Try
    '    End Sub
    '#End Region

End Class
