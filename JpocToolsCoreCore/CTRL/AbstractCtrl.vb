Imports System.Diagnostics
Public MustInherit Class AbstractCtrl
    Implements IDisposable
    Implements ICtrl

#Region "イベント"
    Public Event OnStep(ByVal sender As Object, ByVal e As OnStepEventArgs)
#End Region

#Region "変数"
    Protected mDto As AbstractDto
    Protected mLogic As AbstractLogic
    Private mDBManager As ElsDataBase
#End Region

#Region "プロパティ"
    Protected ReadOnly Property DBManager() As ElsDataBase
        Get
            Return mDBManager
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Private Sub New()
        Me.Init()
    End Sub
    Protected Sub New(pDto As AbstractDto)
        Me.New()
        Try
            mDto = pDto
            mDBManager = New ElsDataBase(mDto.ConString)
            Dim objName As String = Me.GetType.ToString
            Dim formId As String = objName.Substring(objName.Length - 6, 6)
            Dim logicType As Type = Type.GetType("Jpoc.Tools.Core.Logic" & formId)
            If logicType IsNot Nothing Then
                Dim obj As Object = Activator.CreateInstance(logicType, mDBManager, mDto)
                If obj IsNot Nothing Then mLogic = CType(obj, AbstractLogic)
            End If
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "初期化"
    Public MustOverride Sub Init() Implements ICtrl.Init
#End Region

#Region "接続先変更"
    Public Sub ResetConnection()
        Try
            mDBManager.ResetConStr(mDto.ConString)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                'マネージ状態を破棄します (マネージ オブジェクト)。
                If Me.DBManager IsNot Nothing Then
                    If Me.DBManager.State = ConnectionState.Connecting Then
                        If Me.DBManager.HasTransaction Then Me.DBManager.RollbackTransaction()
                        Me.DBManager.DisConnect()
                    End If
                    Me.DBManager.Dispose()
                End If
            End If

            'アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。

            '大きなフィールドを null に設定します。
            Me.mDBManager = Nothing
        End If
        Me.disposedValue = True
    End Sub

    '上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose, ICtrl.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "OnStepイベントRaise"
    Protected Sub PassStep(ByVal pStepEventType As PublicEnum.eStepEventType, _
                           ByVal pTarget As String, _
                           ByVal pMessage As String, _
                           Optional ByVal pCompleteStep As Integer = 0, _
                           Optional ByVal pTotalStep As Integer = 0, _
                           Optional ByVal pCompleteSubStep As Integer = 0, _
                           Optional ByVal pTotalSubStep As Integer = 0, _
                           Optional ByVal pCompleteTransactionStep As Integer = 0, _
                           Optional ByVal pTotalTransactionStep As Integer = 0)
        Dim ea As New OnStepEventArgs
        ea.StepEventType = pStepEventType
        ea.Target = pTarget
        ea.Message = pMessage
        ea.CompleteMainStep = pCompleteStep
        ea.TotalMainStep = pTotalStep
        ea.CompleteSubStep = pCompleteSubStep
        ea.TotalSubStep = pTotalSubStep
        ea.CompleteTransactionStep = pCompleteTransactionStep
        ea.TotalTransactionStep = pTotalTransactionStep
        RaiseEvent OnStep(Me, ea)
    End Sub
#End Region

#Region "StepCount加算"
    Protected Function AddStep(ByRef p As Integer) As Integer
        p += 1
        Return p
    End Function
#End Region

End Class
