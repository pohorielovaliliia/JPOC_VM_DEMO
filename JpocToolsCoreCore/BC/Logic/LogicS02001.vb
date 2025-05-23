Imports System.IO
Public Class LogicS02001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS02001
        Get
            Return CType(MyBase.mDto, DtoS02001)
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

#Region "入力値チェック"
    Public Sub CheckEntry()
        Try
            If String.IsNullOrEmpty(Me.dto.ConString) Then
                Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.dto.MessageSet = Utilities.GetMessageSet("ERR0002", "DB接続先")
                Exit Sub
            End If

            If String.IsNullOrEmpty(Me.dto.ImageFolderPath) AndAlso String.IsNullOrEmpty(Me.dto.PopupFolderPath) Then
                Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.dto.MessageSet = Utilities.GetMessageSet("ERR0003", "イメージ", "ポップアップ")
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(Me.dto.ImageFolderPath) Then
                If String.IsNullOrEmpty(Me.dto.ImageBackupFolderPath) Then
                    Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    Me.dto.MessageSet = Utilities.GetMessageSet("ERR0002", "イメージのバックアップフォルダ")
                    Exit Sub
                End If
                If Me.dto.ImageFolderPath.Equals(Me.dto.ImageBackupFolderPath) Then
                    Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    Me.dto.MessageSet = Utilities.GetMessageSet("ERR0004", "バックアップフォルダはソースと異なる場所")
                    Exit Sub
                End If
            End If
            If Not String.IsNullOrEmpty(Me.dto.PopupFolderPath) Then
                If String.IsNullOrEmpty(Me.dto.PopupBackupFolderPath) Then
                    Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    Me.dto.MessageSet = Utilities.GetMessageSet("ERR0002", "ポップアップのバックアップフォルダ")
                    Exit Sub
                End If
                If Me.dto.PopupFolderPath.Equals(Me.dto.PopupBackupFolderPath) Then
                    Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    Me.dto.MessageSet = Utilities.GetMessageSet("ERR0004", "バックアップフォルダはソースと異なる場所")
                    Exit Sub
                End If
            End If
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "フォルダ内画像一覧取得"
    Public Function GetImageList() As Integer
        Try
            Dim di As New DirectoryInfo(Me.dto.ImageFolderPath)
            Dim fileList() As FileInfo = Nothing
            fileList = di.GetFiles("*.jpeg", SearchOption.TopDirectoryOnly)
            AddImageListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.ImageFolderPath)
            fileList = di.GetFiles("*.jpg", SearchOption.TopDirectoryOnly)
            AddImageListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.ImageFolderPath)
            fileList = di.GetFiles("*.png", SearchOption.TopDirectoryOnly)
            AddImageListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.ImageFolderPath)
            fileList = di.GetFiles("*.gif", SearchOption.TopDirectoryOnly)
            AddImageListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.ImageFolderPath)
            fileList = di.GetFiles("*.bmp", SearchOption.TopDirectoryOnly)
            AddImageListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.ImageFolderPath)
            Return Me.dto.ImageListDataSet.ImageFileList.Rows.Count
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ImageFileListレコード追加(Image)"
    Private Sub AddImageListRow(ByRef dt As DS_IMAGE_LIST.ImageFileListDataTable, _
                                ByVal fileList() As FileInfo, _
                                ByVal rootPath As String)
        Try
            For Each fi As FileInfo In fileList
                Dim newRow As DS_IMAGE_LIST.ImageFileListRow = CType(dt.NewRow, DS_IMAGE_LIST.ImageFileListRow)
                newRow.ContentType = "Image"
                newRow.file_name = fi.Name
                newRow.file_path = fi.FullName
                newRow.file_url = "/images/cms/" & fi.Name
                newRow.create_date = fi.CreationTime.ToString("yyyy/MM/dd")
                newRow.last_update_date = fi.LastWriteTime.ToString("yyyy/MM/dd")
                dt.Rows.Add(newRow)
            Next
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "動画など一覧取得"
    Public Function GetPopupList() As Integer
        Try
            Dim di As New DirectoryInfo(Me.dto.PopupFolderPath)
            Dim fileList() As FileInfo = Nothing
            fileList = di.GetFiles("*.mp4", SearchOption.AllDirectories)
            AddPopupListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.PopupFolderPath)
            fileList = di.GetFiles("*.docx", SearchOption.AllDirectories)
            AddPopupListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.PopupFolderPath)
            fileList = di.GetFiles("*.pptx", SearchOption.AllDirectories)
            AddPopupListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.PopupFolderPath)
            fileList = di.GetFiles("*.pdf", SearchOption.AllDirectories)
            AddPopupListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.PopupFolderPath)
            fileList = di.GetFiles("*.mp3", SearchOption.AllDirectories)
            AddPopupListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.PopupFolderPath)
            fileList = di.GetFiles("*.html", SearchOption.AllDirectories)
            AddPopupListRow(Me.dto.ImageListDataSet.ImageFileList, fileList, Me.dto.PopupFolderPath)
            Return Me.dto.ImageListDataSet.ImageFileList.Rows.Count
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ImageFileListレコード追加(Popup)"
    Private Sub AddPopupListRow(ByRef dt As DS_IMAGE_LIST.ImageFileListDataTable, _
                                ByVal fileList() As FileInfo, _
                                ByVal rootPath As String)
        Try
            For Each fi As FileInfo In fileList
                Dim newRow As DS_IMAGE_LIST.ImageFileListRow = DirectCast(dt.NewRow, DS_IMAGE_LIST.ImageFileListRow)
                newRow.ContentType = "Popup"
                newRow.file_name = fi.Name
                newRow.file_path = fi.FullName
                newRow.file_url = "/popup" & fi.FullName.Replace(rootPath, String.Empty).Replace("\", "/")
                dt.Rows.Add(newRow)
            Next
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "バックアップへファイル移動(Image)"
    Public Sub MoveImageToBackup()
        Try
            Dim fileName As String = IO.Path.GetFileName(Me.dto.SourceFileFullPath)
            Dim backupFileFullPath As String = Path.Combine(Me.dto.ImageBackupFolderPath, fileName)
            'File.Move(Me.dto.SourceFileFullPath, backupFileFullPath)
            File.Copy(Me.dto.SourceFileFullPath, backupFileFullPath, True)
            File.Delete(Me.dto.SourceFileFullPath)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "バックアップへファイル移動(Popup)"
    Public Sub MovePopupToBackup()
        Try
            Dim directoryFullPath As String = Path.GetDirectoryName(Me.dto.SourceFileFullPath)
            Dim relatedPath As String = directoryFullPath.Replace(Me.dto.PopupFolderPath, String.Empty)
            Dim subDirectories() As String = relatedPath.Split("\"c)
            Dim backupPath As String = Me.dto.PopupBackupFolderPath
            For Each str As String In subDirectories
                If String.IsNullOrEmpty(str) Then Continue For
                backupPath = Path.Combine(backupPath, str)
                If Not IO.Directory.Exists(backupPath) Then
                    IO.Directory.CreateDirectory(backupPath)
                End If
            Next
            Dim backupFileFullPath As String = String.Empty
            If relatedPath.StartsWith("\") Then
                backupFileFullPath = Me.dto.PopupBackupFolderPath & relatedPath
            Else
                backupFileFullPath = Path.Combine(Me.dto.PopupBackupFolderPath, relatedPath)
            End If
            Dim fileName As String = Path.GetFileName(Me.dto.SourceFileFullPath)
            backupFileFullPath = Path.Combine(backupFileFullPath, fileName)

            'File.Move(Me.dto.SourceFileFullPath, backupFileFullPath)
            File.Copy(Me.dto.SourceFileFullPath, backupFileFullPath, True)
            File.Delete(Me.dto.SourceFileFullPath)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
