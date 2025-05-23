Imports Jpoc.Common
Public Class ValidationResult
    Implements IDisposable

#Region "プロパティ"
    Public Property ErrorExist As Boolean
    Public Property ErrorMessageSet As MessageSet
#End Region

#Region "コンストラクタ"
    Public Sub New()
        MyBase.New()
        Me._ErrorExist = False
        Me._ErrorMessageSet = New MessageSet
    End Sub
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
