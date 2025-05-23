Imports System.IO
Public Class LogicS03001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS03001
        Get
            Return CType(MyBase.mDto, DtoS03001)
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
                Me.dto.MessageSet = Utilities.GetMessageSet("ERR0004", "イメージとポップアップのファイルが格納されているフォルダ")
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

#Region "イメージレコード取得"
    Public Function GetImageList() As Integer
        Try
            Using dao As New DaoImage(MyBase.DBManager)
                Return dao.GetImageList(Me.dto.ImageListDataSet.ImageList)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

End Class
