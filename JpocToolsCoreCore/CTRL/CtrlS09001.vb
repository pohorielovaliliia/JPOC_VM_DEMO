Imports System.IO
Public Class CtrlS09001
    Inherits AbstractCtrl

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS09001
        Get
            Return CType(MyBase.mDto, DtoS09001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS09001
        Get
            Return CType(MyBase.mLogic, LogicS09001)
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

#Region "SQL Commandファイル取得"
    Public Sub GetSqlCommandFiles()
        Try
            Dim list As List(Of FileInfo) = Me.Logic.GetFileList(GlobalVariables.SqlCommandFolderPhysicalPath, "*.sql")
            Me.Logic.ReadSqlCommandFiles(list)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "コマンド実行"
    Public Sub Execute()
        Try
            '入力チェック
            Me.Logic.CheckEntry("Execute")
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            Dim i As Integer = 0
            For Each row As DataRow In Me.dto.SqlCommandTable.Rows
                Dim selected As Boolean = CType(row.Item("SELECT"), Boolean)
                If Not selected Then Continue For
                Me.dto.WorkDataSet = New DataSet
                Me.dto.SqlCommand = Utilities.NZ(row.Item("SQL_COMMAND")).Trim
                If String.IsNullOrEmpty(Me.dto.SqlCommand) Then Continue For
                Me.Logic.Execute()
                If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

                Dim tableName As String = Utilities.NZ(row.Item("FILE_NAME")).Trim
                Me.dto.WorkDataSet.Tables(0).TableName = tableName
                Me.dto.CommonDataSet.Tables.Add(Me.dto.WorkDataSet.Tables(0).Copy)
                Me.dto.WorkDataSet.Clear()
                Me.dto.WorkDataSet.Dispose()
                Me.dto.WorkDataSet = Nothing
                i += 1
            Next
            Me.dto.CommonDataSet.AcceptChanges()

            If i = 0 Then
                Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.dto.MessageSet = Utilities.GetMessageSet("ERR0000", "実行対象が選択されていません。")
            End If
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
