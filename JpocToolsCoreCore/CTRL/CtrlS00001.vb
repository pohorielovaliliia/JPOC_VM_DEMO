Public Class CtrlS00001
    Inherits AbstractCtrl

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS00001
        Get
            Return CType(MyBase.mDto, DtoS00001)
        End Get
    End Property
    Public ReadOnly Property Logic As LogicS00001
        Get
            Return CType(MyBase.mLogic, LogicS00001)
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

End Class
