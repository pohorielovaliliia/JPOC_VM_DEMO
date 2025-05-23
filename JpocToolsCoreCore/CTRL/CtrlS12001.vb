Imports System.IO
Public Class CtrlS12001
    Inherits AbstractCtrl

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS12001
        Get
            Return CType(MyBase.mDto, DtoS12001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS12001
        Get
            Return CType(MyBase.mLogic, LogicS12001)
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

#Region "実行"
    Public Sub Execute()
        Try
            '入力チェック
            Me.Logic.CheckDate()
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub
            Me.Logic.CreateSerial()
            Me.dto.RtnCD = Common.PublicEnum.eRtnCD.Normal
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
        End Try
    End Sub
#End Region
End Class
