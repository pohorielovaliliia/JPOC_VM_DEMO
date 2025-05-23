Public Class DtoS12001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property InstallDate As String
    Public Property SerialNumber As String
    Public Property MACAddress As String
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
            Me.InstallDate = String.Empty
            Me.SerialNumber = String.Empty
            Me.MACAddress = String.Empty
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
