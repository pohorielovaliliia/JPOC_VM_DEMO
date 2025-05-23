Imports System.Drawing
Public Class DtoS09001
    Inherits AbstractDto

#Region "プロパティ"
    Public Property SqlCommandTable As DataTable
    Public Property SqlCommand As String
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
            Me._SqlCommand = String.Empty
            Me.InitSqlCommandTable()
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

#Region "TargetDiseaseTable初期化"
    Public Sub InitSqlCommandTable()
        Me._SqlCommandTable = New DataTable("SqlCommandList")
        Me._SqlCommandTable.Columns.Add(New DataColumn("SELECT", Type.GetType("System.Boolean")))
        Me._SqlCommandTable.Columns.Add(New DataColumn("NO", Type.GetType("System.Int32")))
        Me._SqlCommandTable.Columns.Add(New DataColumn("FILE_NAME", Type.GetType("System.String")))
        Me._SqlCommandTable.Columns.Add(New DataColumn("SQL_COMMAND", Type.GetType("System.String")))
        Dim pk() As DataColumn = {Me._SqlCommandTable.Columns("ID")}
        Me._SqlCommandTable.Columns("SELECT").DefaultValue = False
        Me._SqlCommandTable.PrimaryKey = pk
    End Sub
#End Region


End Class
