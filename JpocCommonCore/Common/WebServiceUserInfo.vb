Public MustInherit Class WebServiceUserInfo
    Implements IDisposable

    Public Property UserID As String
    Public Property InstitutionCode As String
    Public Property Password As String
    'TODO: FIX HERE'
    'Public ReadOnly Property EncryptedUserID As String
    '    Get
    '        If String.IsNullOrEmpty(Me.UserID) Then Return String.Empty
    '        Return Utilities.Encrypt(Me.UserID, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '    End Get
    'End Property
    'Public ReadOnly Property EncryptedInstitutionCode As String
    '    Get
    '        If String.IsNullOrEmpty(Me.InstitutionCode) Then Return String.Empty
    '        Return Utilities.Encrypt(Me.InstitutionCode, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '    End Get
    'End Property
    'Public ReadOnly Property EncryptedPassword As String
    '    Get
    '        If String.IsNullOrEmpty(Me.Password) Then Return String.Empty
    '        Return Utilities.Encrypt(Me.Password, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
    '    End Get
    'End Property
    Public Property ConnectWebServiceName As String
    Public Property Authorized As Boolean
    Protected Sub New()
        Me.Init()
    End Sub

#Region "初期化"
    Public MustOverride Sub Init()
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
