Imports System.Drawing
Public Class DtoS08001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property OutputFilePath As String
    Public Property DiseaseID As String
    Public Property WithoutPopupContents As Boolean
    Public Property OnlyRawImage As Boolean
    Public Property NeedBase64Encode As Boolean
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
            Me._OutputFilePath = String.Empty
            Me._DiseaseID = String.Empty
            Me._WithoutPopupContents = True
            Me._OnlyRawImage = True
            Me._NeedBase64Encode = False
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
