Imports System.Drawing
Public Class DtoS05001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property SourceDataBase As String
    Public Property DistinationDataBase As String
    Public Property SourceImageFolderPath As String
    Public Property SourcePopupFolderPath As String
    Public Property DistinationImageFolderPath As String
    Public Property DistinationPopupFolderPath As String
    Public Property CurrentDiseaseID As Integer
    Public Property TargetDiseaseTable As DataTable

    Public Property InputFolderPath As String
    Public Property OutputFolderPath As String
    Public Property BackupFolderPath As String
    Public Property MatchID As Boolean
    Public Property AllDelete As Boolean
    Public Property OnlyRawImage As Boolean
    Public Property WriteToMemory As Boolean
    Public Property ReadFromMemory As Boolean
    Public Property CreateBackup As Boolean
    Public Property WithoutPopupContents As Boolean
    Public Property DiseaseOutputDataList As Dictionary(Of String, DS_DISEASE)
    Public Property DiseaseDataSetList As List(Of DS_DISEASE)
    Public ReadOnly Property SourceConStr As String
        Get
            If Not String.IsNullOrEmpty(Me.SourceDataBase) Then
                Return GlobalVariables.ConnectionString(Me.SourceDataBase)
            End If
            Return String.Empty
        End Get
    End Property
    Public ReadOnly Property DistinationConStr As String
        Get
            If Not String.IsNullOrEmpty(Me.DistinationDataBase) Then
                Return GlobalVariables.ConnectionString(Me.DistinationDataBase)
            End If
            Return String.Empty
        End Get
    End Property
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
            Me._SourceDataBase = String.Empty
            Me._DistinationDataBase = String.Empty
            Me._SourceImageFolderPath = String.Empty
            Me._SourcePopupFolderPath = String.Empty
            Me._DistinationImageFolderPath = String.Empty
            Me._DistinationPopupFolderPath = String.Empty
            Me._InputFolderPath = String.Empty
            Me._OutputFolderPath = String.Empty
            Me._BackupFolderPath = String.Empty
            Me._MatchID = False
            Me._AllDelete = False
            Me._OnlyRawImage = False
            Me._WriteToMemory = False
            Me._ReadFromMemory = False
            Me._CreateBackup = False
            Me._WithoutPopupContents = False
            Me._DiseaseOutputDataList = New Dictionary(Of String, DS_DISEASE)
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
