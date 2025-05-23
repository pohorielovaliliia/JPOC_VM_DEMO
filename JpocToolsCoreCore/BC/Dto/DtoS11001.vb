Imports System.Drawing
Public Class DtoS11001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property SourceImageFolderPath As String
    Public Property SourcePopupFolderPath As String
    Public Property CurrentDiseaseID As Integer
    Public Property TargetDiseaseTable As DataTable

    Public Property BackupFolderPath As String
    Public Property CreateBackup As Boolean
    Public Property DiseaseDataSetList As List(Of DS_DISEASE)
    Public ReadOnly Property TargetDiseaseView As DataView
        Get
            If Me.TargetDiseaseTable Is Nothing Then
                Me.InitTargetDiseaseTable()
            End If
            Return New DataView(Me.TargetDiseaseTable, String.Empty, "DISEASE_ID", DataViewRowState.CurrentRows)
        End Get
    End Property
    Public Property Lines As String()
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
            Me._SourceImageFolderPath = String.Empty
            Me._SourcePopupFolderPath = String.Empty
            Me._BackupFolderPath = String.Empty
            Me._CreateBackup = False
            Me._DiseaseDataSetList = New List(Of DS_DISEASE)
            Me._CurrentDiseaseID = -1
            Me._Lines = Nothing
            Me._NeedBase64Encode = False
            Me.InitTargetDiseaseTable()
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
        If Me._TargetDiseaseTable IsNot Nothing Then
            Me._TargetDiseaseTable.Rows.Clear()
            Me._TargetDiseaseTable.Dispose()
        End If
        If Me._DiseaseDataSetList IsNot Nothing Then
            For Each item As DS_DISEASE In Me._DiseaseDataSetList
                If item IsNot Nothing Then
                    item.Clear()
                    item.Dispose()
                End If
                item = Nothing
            Next
            Me._DiseaseDataSetList.Clear()
        End If
        If Me._Lines IsNot Nothing Then
            Erase Me._Lines
        End If
        Me._TargetDiseaseTable = Nothing
        Me._DiseaseDataSetList = Nothing
        Me._Lines = Nothing
        MyBase.Dispose()
    End Sub
#End Region

#Region "TargetDiseaseTable初期化"
    Public Sub InitTargetDiseaseTable()
        Me._TargetDiseaseTable = New DataTable("DiseaseList")
        Me._TargetDiseaseTable.Columns.Add(New DataColumn("DISEASE_ID", Type.GetType("System.Int32")))
        Me._TargetDiseaseTable.Columns.Add(New DataColumn("DISEASE_TITLE", Type.GetType("System.String")))
        Me._TargetDiseaseTable.Columns.Add(New DataColumn("RESULT", Type.GetType("System.String")))
        Me._TargetDiseaseTable.Columns.Add(New DataColumn("MESSAGE", Type.GetType("System.String")))
        Dim pk() As DataColumn = {Me._TargetDiseaseTable.Columns("DISEASE_ID")}
        Me._TargetDiseaseTable.PrimaryKey = pk
    End Sub
#End Region

End Class
