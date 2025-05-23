Imports System.IO
Public Class CtrlS02001
    Inherits AbstractCtrl

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS02001
        Get
            Return CType(MyBase.mDto, DtoS02001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS02001
        Get
            Return CType(MyBase.mLogic, LogicS02001)
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

#Region "データ取得 & 存在チェック"
    Public Sub GetContentsListAndCheckRecordExist()
        Try
            Me.Logic.CheckEntry()
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            Me.dto.ImageListDataSet.ImageFileList.Clear()

            If Not String.IsNullOrEmpty(Me.dto.ImageFolderPath) Then
                Me.Logic.GetImageList()
            End If
            If Not String.IsNullOrEmpty(Me.dto.PopupFolderPath) Then
                Me.Logic.GetPopupList()
            End If

            Try
                MyBase.DBManager.ResetConStr(Me.dto.ConString)

                MyBase.DBManager.Connect()

                Me.Logic.GetImageCount()

                For Each dr As DS_IMAGE_LIST.ImageFileListRow In Me.dto.ImageListDataSet.ImageFileList.Rows
                    Dim existRec As Boolean = False
                    Me.dto.FileUrl = dr.file_url
                    If dr.ContentType.Equals("Image") Then
                        If dr.file_name.StartsWith("raw_") Then
                            existRec = Me.Logic.GetImageByRawImage > 0
                            If existRec Then
                                dr.id = Utilities.N0Int(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("id"))
                                dr.code = Utilities.NZ(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("code"))
                            End If
                        End If
                        If dr.file_name.StartsWith("tb_") Then
                            existRec = Me.Logic.GetImageByTbImage > 0
                            If existRec Then
                                dr.id = Utilities.N0Int(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("id"))
                                dr.code = Utilities.NZ(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("code"))
                            End If
                        End If
                        If dr.file_name.StartsWith("dp_") Then
                            existRec = Me.Logic.GetImageByDpImage > 0
                            If existRec Then
                                dr.id = Utilities.N0Int(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("id"))
                                dr.code = Utilities.NZ(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("code"))
                            End If
                        End If
                        If dr.file_name.StartsWith("ad_") Then
                            existRec = Me.Logic.GetImageByAdImage > 0
                            If existRec Then
                                dr.id = Utilities.N0Int(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("id"))
                                dr.code = Utilities.NZ(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("code"))
                            End If
                        End If
                    Else
                        existRec = Logic.GetImageByRawImage > 0
                        If existRec Then
                            dr.id = Utilities.N0Int(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("id"))
                            dr.code = Utilities.NZ(Me.dto.DiseaseDataSet.T_JP_Image.Rows(0).Item("code"))
                        End If
                    End If
                    If existRec Then
                        dr.record_exist = True
                    End If
                Next
            Catch ex As Exception
                Throw
            Finally
                MyBase.DBManager.DisConnect()
            End Try

            Me.dto.ImageListDataSet.ImageFileList.AcceptChanges()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "コンテンツ移動"
    Public Sub MoveFiles()
        Dim stepCount As Integer = 0
        Try
            For Each dr As DS_IMAGE_LIST.ImageFileListRow In Me.dto.ImageListDataSet.ImageFileList.Rows
                If dr.RowState = DataRowState.Deleted Then Continue For
                If dr.record_exist = True Then Continue For

                Me.dto.SourceFileFullPath = dr.file_path
                If dr.ContentType.Equals("Image") Then
                    Me.Logic.MoveImageToBackup()
                Else
                    Me.Logic.MovePopupToBackup()
                End If
                MyBase.PassStep(PublicEnum.eStepEventType.Message, String.Empty, String.Empty, MyBase.AddStep(stepCount), Me.dto.ImageListDataSet.ImageFileList.Rows.Count)
            Next

            Me.dto.RtnCD = PublicEnum.eRtnCD.Normal
            Me.dto.MessageSet = Utilities.GetMessageSet("INF0002", "画像の整理")

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
