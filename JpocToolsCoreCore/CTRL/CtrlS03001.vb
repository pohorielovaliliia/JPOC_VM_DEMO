Imports System.IO
Public Class CtrlS03001
    Inherits AbstractCtrl

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS03001
        Get
            Return CType(MyBase.mDto, DtoS03001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS03001
        Get
            Return CType(MyBase.mLogic, LogicS03001)
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

#Region "Dispose"
    Public Shadows Sub Dispose()


        MyBase.Dispose()
    End Sub
#End Region

#Region "イメージレコード取得"
    Public Sub GetImageRecAndCheckFileExist()
        Try
            Me.Logic.CheckEntry()
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            Try
                MyBase.DBManager.ResetConStr(Me.dto.ConString)

                MyBase.DBManager.Connect()

                Me.Logic.GetImageCount()

                Me.Logic.GetImageList()
            Catch ex As Exception
                Throw
            Finally
                MyBase.DBManager.DisConnect()
            End Try

            For Each dr As DS_IMAGE_LIST.ImageListRow In Me.dto.ImageListDataSet.ImageList.Rows
                If dr.img_type.Equals("Popup") Then
                    Me.dto.RootPath = Me.dto.PopupFolderPath
                    Dim directories() As String = dr.raw_image.ToLower.Split("/"c)
                    Dim fileUrl As String = String.Empty
                    For Each str As String In directories
                        If String.IsNullOrEmpty(str) Then Continue For
                        If str.Equals("popup") Then Continue For
                        fileUrl &= "/" & str
                    Next
                    Me.dto.FileUrl = fileUrl
                    dr.raw_exist = Me.Logic.ExistFile
                    If dr.popup_type.Equals("video") Then
                        Me.dto.RootPath = Me.dto.ImageFolderPath
                        Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                        dr.tb_exist = Me.Logic.ExistFile
                    Else
                        dr.tb_exist = True
                    End If
                    dr.ad_exist = True
                    dr.dp_exist = True
                Else
                    Me.dto.RootPath = Me.dto.ImageFolderPath
                    Me.dto.FileUrl = Path.GetFileName(dr.raw_image)
                    dr.raw_exist = Me.Logic.ExistFile
                    Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                    dr.tb_exist = Me.Logic.ExistFile
                    Me.dto.FileUrl = Path.GetFileName(dr.ad_image)
                    dr.ad_exist = Me.Logic.ExistFile
                    Me.dto.FileUrl = Path.GetFileName(dr.dp_image)
                    dr.dp_exist = Me.Logic.ExistFile
                End If
            Next
            Me.dto.ImageListDataSet.AcceptChanges()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
