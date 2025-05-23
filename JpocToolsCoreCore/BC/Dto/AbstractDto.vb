Imports System.Data
Imports System.IO

Public MustInherit Class AbstractDto
    Implements IDisposable
    Implements IDto

#Region "変数"

#End Region

#Region "プロパティ"
    Public Property RtnCD As PublicEnum.eRtnCD
    Public Property MessageSet As MessageSet
    Public Property StartTime As Double
    Public Property EndTime As Double
    Public Property ImageListDataSet As DS_IMAGE_LIST
    Public Property WorkDataSet As DataSet
    Public Property CommonDataSet As DataSet
    Public Property DiseaseDataSet As DS_DISEASE
    Public Property DataBaseName As String
    Public Property ImageFolderPath As String
    Public Property PopupFolderPath As String
    Public Property ImageBackupFolderPath As String
    Public Property PopupBackupFolderPath As String
    Public Property RootPath As String
    Public Property FileUrl As String
    Public Property RawCount As Integer
    Public Property DpCount As Integer
    Public Property AdCount As Integer
    Public Property TbCount As Integer
    Public Property PopupCount As Integer

    Public ReadOnly Property TotalCount As Integer
        Get
            Return RawCount + DpCount + AdCount + TbCount + PopupCount
        End Get
    End Property
    Public ReadOnly Property ConString As String
        Get
            If Not String.IsNullOrEmpty(Me.DataBaseName) Then
                Return GlobalVariables.ConnectionString(DataBaseName)
            End If
            Return String.Empty
        End Get
    End Property
    Public ReadOnly Property ImageFilePath As String
        Get
            If String.IsNullOrEmpty(Me.RootPath) OrElse String.IsNullOrEmpty(Me.FileUrl) Then Return String.Empty
            Dim filePath As String = Me.FileUrl.Replace("/", "\")
            If filePath.StartsWith("\") Then
                Return Me.RootPath & filePath
            Else
                Return Path.Combine(Me.RootPath, filePath)
            End If
        End Get
    End Property
    Public ReadOnly Property ElapsTime As Double
        Get
            Return EndTime - StartTime
        End Get
    End Property
    'TODO: FIX HERE
    'Public ReadOnly Property ElapsString As String
    '    Get
    '        Return Utilities.GetTimeFormat(ElapsTime)
    '    End Get
    'End Property
#End Region

#Region "コンストラクタ"
    Protected Sub New()
        MyBase.New()
        Try
            Me.BaseInit()
            Me.Init()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "初期化"
    Public MustOverride Sub Init() Implements IDto.Init
    Protected Sub BaseInit()
        Me._RtnCD = PublicEnum.eRtnCD.Normal
        Me._MessageSet = New MessageSet
        Me._StartTime = 0.0
        Me._EndTime = 0.0
        Me._ImageListDataSet = New DS_IMAGE_LIST
        Me._WorkDataSet = New DataSet("WorkDataSet")
        Me._CommonDataSet = New DataSet("CommonDataSet")
        Me._DiseaseDataSet = New DS_DISEASE
        Me._DataBaseName = String.Empty
        Me._ImageFolderPath = String.Empty
        Me._PopupFolderPath = String.Empty
        Me._ImageBackupFolderPath = String.Empty
        Me._PopupBackupFolderPath = String.Empty
        Me._RawCount = 0
        Me._DpCount = 0
        Me._AdCount = 0
        Me._TbCount = 0
        Me._PopupCount = 0
        Me._RootPath = String.Empty
        Me._FileUrl = String.Empty
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' マネージ状態を破棄します (マネージ オブジェクト)。
            End If
            ' アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            If Me._ImageListDataSet IsNot Nothing Then
                Me._ImageListDataSet.Clear()
                Me._ImageListDataSet.Dispose()
            End If
            If Me._WorkDataSet IsNot Nothing Then
                Me._WorkDataSet.Clear()
                Me._WorkDataSet.Dispose()
            End If
            If Me._CommonDataSet IsNot Nothing Then
                Me._CommonDataSet.Clear()
                Me._CommonDataSet.Dispose()
            End If
            ' 大きなフィールドを null に設定します。
            Me._ImageListDataSet = Nothing
            Me._WorkDataSet = Nothing
            Me._CommonDataSet = Nothing
        End If
        Me.disposedValue = True
    End Sub

    ' 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose, IDto.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
