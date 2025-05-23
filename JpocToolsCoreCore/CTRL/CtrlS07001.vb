Imports System.IO
Public Class CtrlS07001
    Inherits AbstractCtrl

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS07001
        Get
            Return CType(MyBase.mDto, DtoS07001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS07001
        Get
            Return CType(MyBase.mLogic, LogicS07001)
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDto As AbstractDto)
        MyBase.New(pDto)
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

#Region "コマンド実行"
    Public Sub Execute()
        Try
            '入力チェック
            Me.Logic.CheckEntry("Execute")
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            'TODO: FIX HERE
            If Me.dto.SqlCommand.ToLower.StartsWith("select") Then
                'Me.Logic.EncryptSendData()
                'Me.Logic.Execute()

                'TODO: FIX HERE
            ElseIf Me.dto.SqlCommand.ToLower.StartsWith("insert") OrElse _
                   Me.dto.SqlCommand.ToLower.StartsWith("update") OrElse _
                   Me.dto.SqlCommand.ToLower.StartsWith("delete") Then
                'Me.Logic.EncryptSendData()
                Me.Logic.ExecuteNonResult()
            Else
                Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.dto.MessageSet = Utilities.GetMessageSet("ERR0000", "サポート外のコマンドです。")
            End If
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
