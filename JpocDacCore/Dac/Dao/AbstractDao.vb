Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlDbType
Public MustInherit Class AbstractDao
    Implements IDisposable
    Implements IDao

#Region "定数"

#End Region

#Region "インスタンス変数"
    Private mDbManager As ElsDataBase
#End Region

#Region "プロパティ"
    Protected ReadOnly Property DbManager As ElsDataBase
        Get
            Return Me.mDbManager
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Private Sub New()
        MyBase.New()
        Try
            Me.Init()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
    Protected Sub New(ByRef pDbmanager As ElsDataBase)
        Me.New()
        Try
            Me.mDbManager = pDbmanager
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "初期化"
    Public MustOverride Sub Init() Implements IDao.Init
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
    Public Overridable Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
