Imports System.Drawing
Public Class DtoS07001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property InstitutionCode As String
    Public Property LoginID As String
    Public Property Password As String
    Public Property SqlCommand As String
    Public Property WebServiceName As String
#End Region

#Region "コンストラクタ"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "初期化"
    Public Overrides Sub Init()
        Try
            MyBase.BaseInit()
            Me._InstitutionCode = String.Empty
            Me._LoginID = String.Empty
            Me._Password = String.Empty
            Me._SqlCommand = String.Empty
            Me._WebServiceName = String.Empty
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
