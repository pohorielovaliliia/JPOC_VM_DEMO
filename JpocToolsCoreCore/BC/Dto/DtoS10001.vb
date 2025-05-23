Imports System.Drawing
Public Class DtoS10001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property DiseaseID As String
    Public Property TargetDiseaseTable As DataTable
    Public Property AllDiseaseTable As DataTable
    Public Property InputFilePath As String
    Public Property OutputFolderPath As String
    Public ReadOnly Property TargetDiseaseView As DataView
        Get
            If Me.TargetDiseaseTable Is Nothing Then
                Me.InitTargetDiseaseTable()
            End If
            Return New DataView(Me.TargetDiseaseTable, String.Empty, "DISEASE_ID", DataViewRowState.CurrentRows)
        End Get
    End Property
    Public Property WithoutPopupContents As Boolean
    Public Property OnlyRawImage As Boolean
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
            Me._InputFilePath = String.Empty
            Me._OutputFolderPath = String.Empty
            Me._DiseaseID = String.Empty
            Me.InitTargetDiseaseTable()
            Me.InitAllDiseaseTable()
            Me._WithoutPopupContents = True
            Me._OnlyRawImage = True
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
        If Me._AllDiseaseTable IsNot Nothing Then
            Me._AllDiseaseTable.Rows.Clear()
            Me._AllDiseaseTable.Dispose()
        End If
        Me._TargetDiseaseTable = Nothing
        Me._AllDiseaseTable = Nothing
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

#Region "AllDiseaseTable初期化"
    Public Sub InitAllDiseaseTable()
        Me._AllDiseaseTable = New DataTable("DiseaseList")
        Me._AllDiseaseTable.Columns.Add(New DataColumn("DISEASE_ID", Type.GetType("System.Int32")))
        Me._AllDiseaseTable.Columns.Add(New DataColumn("DISEASE_TITLE", Type.GetType("System.String")))
        Me._AllDiseaseTable.Columns.Add(New DataColumn("RESULT", Type.GetType("System.String")))
        Me._AllDiseaseTable.Columns.Add(New DataColumn("MESSAGE", Type.GetType("System.String")))
        Dim pk() As DataColumn = {Me._AllDiseaseTable.Columns("DISEASE_ID")}
        Me._AllDiseaseTable.PrimaryKey = pk
    End Sub
#End Region

End Class
