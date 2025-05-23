Imports System.IO
Public MustInherit Class AbstractLogic
    Implements IDisposable
    Implements ILogic

#Region "インスタンス変数"
    Protected mDto As AbstractDto
    Private mDBManager As ElsDataBase
#End Region

#Region "プロパティ"
    Protected ReadOnly Property DBManager() As ElsDataBase
        Get
            Return mDBManager
        End Get
    End Property
    Private ReadOnly Property Dto() As AbstractDto
        Get
            Return mDto
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Private Sub New()
        MyBase.New()
        Try
            Me.Init()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
    'Protected Sub New(ByRef pDto As AbstractDto)
    '    MyBase.New()
    '    Try
    '        mDBManager = Nothing
    '        mDto = pDto
    '        Me.Init()
    '    Catch ex As ElsException
    '        ex.AddStackFrame(New StackFrame(True))
    '        Throw
    '    Catch ex As Exception
    '        Throw New ElsException(ex)
    '    End Try
    'End Sub
    Protected Sub New(ByRef pDBManager As ElsDataBase, _
                      ByRef pDto As AbstractDto)
        MyBase.New()
        Try
            mDBManager = pDBManager
            mDto = pDto
            Me.Init()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub

#End Region

#Region "初期化"
    Public MustOverride Sub Init() Implements ILogic.Init
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' マネージ状態を破棄します (マネージ オブジェクト)。
            End If
            ' アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose, ILogic.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "Image件数取得"
    Public Sub GetImageCount()
        Try
            Me.Dto.RawCount = 0
            Me.Dto.AdCount = 0
            Me.Dto.DpCount = 0
            Me.Dto.TbCount = 0
            Me.Dto.PopupCount = 0
            Using dao As New DaoImage(Me.DBManager)
                If Not String.IsNullOrEmpty(Me.Dto.ImageFolderPath) Then
                    Me.Dto.RawCount = dao.GetRawImageCount
                    Me.Dto.AdCount = dao.GetAdImageCount
                    Me.Dto.DpCount = dao.GetDpImageCount
                    Me.Dto.TbCount = dao.GetTbImageCount
                End If
                If Not String.IsNullOrEmpty(Me.Dto.PopupFolderPath) Then
                    Me.Dto.PopupCount = dao.GetPopupTbImageCount
                End If
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "RAWレコード取得＆存在チェック"
    Public Function GetImageByRawImage() As Integer
        Try
            Using dao As New DaoImage(Me.DBManager)
                Return dao.GetByRawImage(Me.Dto.DiseaseDataSet.T_JP_Image, _
                                         Me.Dto.FileUrl)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Tbレコード取得＆存在チェック"
    Public Function GetImageByTbImage() As Integer
        Try
            Using dao As New DaoImage(Me.DBManager)
                Return dao.GetByTbImage(Me.Dto.DiseaseDataSet.T_JP_Image, _
                                        Me.Dto.FileUrl)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Dpレコード取得＆存在チェック"
    Public Function GetImageByDpImage() As Integer
        Try
            Using dao As New DaoImage(Me.DBManager)
                Return dao.GetByDpImage(Me.Dto.DiseaseDataSet.T_JP_Image, _
                                        Me.Dto.FileUrl)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Adレコード取得＆存在チェック"
    Public Function GetImageByAdImage() As Integer
        Try
            Using dao As New DaoImage(Me.DBManager)
                Return dao.GetByAdImage(Me.Dto.DiseaseDataSet.T_JP_Image, _
                                        Me.Dto.FileUrl)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ファイル存在チェック"
    Public Function ExistFile() As Boolean
        Try
            If String.IsNullOrEmpty(Me.Dto.ImageFilePath) Then Return True
            Return File.Exists(Me.Dto.ImageFilePath)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ファイル一覧取得"
    Public Function GetFileList(ByVal pPath As String, _
                                ByVal pSearchPattern As String) As List(Of FileInfo)
        Try
            Dim list As New List(Of FileInfo)
            If Not IO.Directory.Exists(pPath) Then Return list
            Dim di As New IO.DirectoryInfo(pPath)
            Dim fileInfos As IO.FileInfo()
            Dim searchPattern As String = pSearchPattern
            If String.IsNullOrEmpty(searchPattern) Then searchPattern = "*.*"
            fileInfos = di.GetFiles(searchPattern, SearchOption.TopDirectoryOnly)
            For Each fi As FileInfo In fileInfos
                List.Add(fi)
            Next
            Return list
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "フォルダ一覧取得"
    Public Function GetDirectoryList(ByVal pPath As String, _
                                     ByVal pSearchPattern As String) As List(Of DirectoryInfo)
        Try
            Dim list As New List(Of DirectoryInfo)
            If Not IO.Directory.Exists(pPath) Then Return list
            Dim di As New IO.DirectoryInfo(pPath)
            Dim directoryInfos As IO.DirectoryInfo()
            Dim searchPattern As String = pSearchPattern
            If String.IsNullOrEmpty(searchPattern) Then searchPattern = "*.*"
            directoryInfos = di.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly)
            For Each dir As DirectoryInfo In directoryInfos
                List.Add(dir)
            Next
            Return list
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

End Class
