Imports System.IO
Public Class LogicS04001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property Dto As DtoS04001
        Get
            Return CType(MyBase.mDto, DtoS04001)
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDBManager As ElsDataBase, _
                   ByRef pDto As AbstractDto)
        MyBase.New(pDBManager, pDto)
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

#Region "入力値チェック"
    Public Sub CheckEntry()
        Try
            If String.IsNullOrEmpty(Me.Dto.ConString) Then
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "DB接続先")
                Exit Sub
            End If

            If String.IsNullOrEmpty(Me.Dto.ImageFolderPath) OrElse String.IsNullOrEmpty(Me.Dto.ImageBackupFolderPath) Then
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0004", "イメージソースとバックアップのフォルダ")
                Exit Sub
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
    Public Sub GetImageList()
        Try
            Me.Dto.Filter &= "*.*"
            Dim di As New DirectoryInfo(Me.Dto.ImageFolderPath)
            Dim fileList() As FileInfo
            fileList = di.GetFiles(Me.Dto.Filter, SearchOption.TopDirectoryOnly)
            For Each fi As FileInfo In fileList
                Select Case fi.Extension.ToLower
                    Case ".jpeg", ".jpg", ".png", ".gif", ".bmp"

                    Case Else
                        Continue For
                End Select
                If Me.Dto.LastUpdatedateFrom.HasValue Then
                    If Utilities.N0Int(fi.LastWriteTime.ToString("yyyyMMdd")) < Utilities.N0Int(Me.Dto.LastUpdatedateFrom.Value.ToString("yyyyMMdd")) Then Continue For
                End If
                If Me.Dto.LastUpdatedateTo.HasValue Then
                    If Utilities.N0Int(fi.LastWriteTime.ToString("yyyyMMdd")) > Utilities.N0Int(Me.Dto.LastUpdatedateTo.Value.ToString("yyyyMMdd")) Then Continue For
                End If

                Dim newRow As DS_IMAGE_LIST.ImageFileListRow = DirectCast(Me.Dto.ImageListDataSet.ImageFileList.NewRow, DS_IMAGE_LIST.ImageFileListRow)
                newRow.ContentType = "Image"
                newRow.file_name = fi.Name
                newRow.file_path = fi.FullName
                newRow.file_url = "/images/cms/" & fi.Name
                newRow.create_date = fi.CreationTime.ToString("yyyy/MM/dd")
                newRow.last_update_date = fi.LastWriteTime.ToString("yyyy/MM/dd")
                Me.Dto.ImageListDataSet.ImageFileList.Rows.Add(newRow)
            Next
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "画像バックアップ保存"
    Public Sub SaveBackup()
        Try
            If Not Directory.Exists(Me.Dto.ImageBackupFolderPath) Then
                IO.Directory.CreateDirectory(Me.Dto.ImageBackupFolderPath)
            End If

            Dim backupFileFullPath As String = String.Empty

            If Me.Dto.TragetFileName.StartsWith("\") Then
                backupFileFullPath = Me.Dto.ImageBackupFolderPath & Me.Dto.TragetFileName
            Else
                backupFileFullPath = Path.Combine(Me.Dto.ImageBackupFolderPath, Me.Dto.TragetFileName)
            End If

            If File.Exists(backupFileFullPath) Then File.Delete(backupFileFullPath)

            Dim fileFullPath As String = String.Empty
            If Me.Dto.TragetFileName.StartsWith("\") Then
                fileFullPath = Me.Dto.ImageFolderPath & Me.Dto.TragetFileName
            Else
                fileFullPath = Path.Combine(Me.Dto.ImageFolderPath, Me.Dto.TragetFileName)
            End If

            File.Move(fileFullPath, backupFileFullPath)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "画像保存"
    Public Sub Save()
        Try
            Dim fileFullPath As String = String.Empty
            If Me.Dto.TragetFileName.StartsWith("\") Then
                fileFullPath = Me.Dto.ImageFolderPath & Me.Dto.TragetFileName
            Else
                fileFullPath = Path.Combine(Me.Dto.ImageFolderPath, Me.Dto.TragetFileName)
            End If
            'TODO: FIX HERE
            'Me.Dto.EdittingImage.Save(fileFullPath)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "ウォーターマーク追加"
    Public Sub AddWaterMark()
        Try
            Dim fileFullPath As String = String.Empty
            If Me.Dto.TragetFileName.StartsWith("\") Then
                fileFullPath = Me.Dto.ImageFolderPath & Me.Dto.TragetFileName
            Else
                fileFullPath = Path.Combine(Me.Dto.ImageFolderPath, Me.Dto.TragetFileName)
            End If
            Using m As New MemoryStream
                Utilities.FileRead(fileFullPath, m, False)
                Me.Dto.EdittingImage = Image_Helper.AddWaterMark(m, Me.Dto.ExpandHeight, Utilities.GetWaterMark())
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
