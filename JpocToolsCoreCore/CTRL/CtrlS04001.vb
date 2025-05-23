Public Class CtrlS04001
    Inherits AbstractCtrl

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS04001
        Get
            Return CType(MyBase.mDto, DtoS04001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS04001
        Get
            Return CType(MyBase.mLogic, LogicS04001)
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

#Region "画像データ一覧取得"
    Public Sub GetImageList()
        Try
            Me.Logic.CheckEntry()
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            Me.dto.ImageListDataSet.ImageFileList.Clear()

            Logic.GetImageList()

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

#Region "画像保存"
    Public Sub SaveImage()
        Try
            Logic.SaveBackup()
            Logic.Save()
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
            Logic.AddWaterMark()
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
