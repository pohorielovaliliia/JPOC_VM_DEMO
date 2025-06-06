﻿Public Class LogicS00001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS00001
        Get
            Return CType(MyBase.mDto, DtoS00001)
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDBManager As ElsDataBase, _
                   ByRef pDto As AbstractDto)
        MyBase.New(pDBManager, pDto)
    End Sub
#End Region

#Region "Dispose"
    Public Shadows Sub Dispose()
        MyBase.Dispose()
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

End Class
