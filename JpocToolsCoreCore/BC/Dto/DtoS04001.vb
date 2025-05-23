Imports System.Drawing
Public Class DtoS04001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property Filter As String
    Public Property LastUpdatedateFrom As Nullable(Of DateTime)
    Public Property LastUpdatedateTo As Nullable(Of DateTime)
    Public Property TragetFileName As String
    Public Property EdittingImage As Image
    Public Property ExpandHeight As Boolean

    Public Property SourceFileFullPath As String
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
            Me._Filter = String.Empty
            Me._LastUpdatedateFrom = Nothing
            Me._TragetFileName = String.Empty
            Me._EdittingImage = Nothing
            Me._ExpandHeight = False

            Me._SourceFileFullPath = String.Empty
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

    'TODO: FIX HERE
    '#Region "Dispose"
    '    Public Shadows Sub Dispose()
    '        If Me._EdittingImage IsNot Nothing Then
    '            Me._EdittingImage.Dispose()
    '        End If
    '        Me._EdittingImage = Nothing
    '        MyBase.Dispose()
    '    End Sub
    '#End Region

End Class
