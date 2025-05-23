Public Class OnBrowserButtonClickEventArgs
    Inherits EventArgs
    Public Property Content As String
    Public Property Open As Boolean
    Private Sub New()
        Me._Content = String.Empty
        Me._Open = False
    End Sub
    Public Sub New(ByVal pContent As String)
        Me.New()
        Me._Content = pContent
    End Sub
End Class
