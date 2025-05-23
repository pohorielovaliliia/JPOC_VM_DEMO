Imports System.Data
Imports System.IO
Imports System.Configuration

Public Class AppConfigure

#Region "定数"
    Const SETTING_PREFIX As String = "ENV_"
    Const NAME_COL As String = "NAME"
    Const VAL_COL As String = "VALUE"
#End Region

#Region "変数"
    Private Shared _AppConfigure As AppConfigure
    Private _ConfigFileFullPath As String
    Private _ConfigDataSet As DataSet
    Private _fileInfo As FileInfo
    Protected _AppSettingValue As String
#End Region

#Region "コンストラクタ"
    Protected Sub New()
        Me._ConfigDataSet = New DataSet
    End Sub
    Public Sub New(ByVal pConfigFilePath As String)
        Me.New()
        Me._ConfigFileFullPath = pConfigFilePath
        Me.Init()
    End Sub
#End Region

#Region "初期化"
    Private Sub Init()
        Me._AppSettingValue = String.Empty
        Me._fileInfo = New FileInfo(Me._ConfigFileFullPath)
        Dim ds As New DataSet
        Utilities.XmlToDataSet(ds, Me._ConfigFileFullPath)
        Me._ConfigDataSet = New DataSet
        For Each tbl As DataTable In ds.Tables
            If tbl.TableName.StartsWith(SETTING_PREFIX) Then
                Me._ConfigDataSet.Tables.Add(tbl.Copy)
            End If
        Next
        If ds IsNot Nothing Then ds.Dispose()
        ds = Nothing
#If DEBUG Then
        Utilities.DataDump(Me._ConfigDataSet)
#End If
        For Each tbl As DataTable In Me._ConfigDataSet.Tables
            Dim pk() As DataColumn = {tbl.Columns(NAME_COL)}
            tbl.PrimaryKey = pk
        Next
        Me._ConfigDataSet.AcceptChanges()
    End Sub
#End Region

#Region "シングルトンインスタンス取得"
    Public Shared Function GetInstance(ByVal pConfigFileFullPath As String) As AppConfigure
        If _AppConfigure Is Nothing Then
            _AppConfigure = New AppConfigure(pConfigFileFullPath)
        End If
        Return _AppConfigure
    End Function
#End Region

#Region "設定値取得"
    Public Function GetConfig(ByVal pConfigName As String,
                              Optional ByVal pWhenNoEntryRaiseException As Boolean = True) As String
        Me.CheckUpdate()
        Dim s As String = Utilities.GetRuntimeEnvironmentName(GlobalVariables.RunTimeEnvironment)
        Dim dr As DataRow = Me._ConfigDataSet.Tables(Utilities.SanitizeInput(SETTING_PREFIX & s)).Rows.Find(pConfigName)
        If dr Is Nothing Then
            If pWhenNoEntryRaiseException Then
                Throw New ArgumentNullException("設定ファイルに" & pConfigName & "が設定されていません。")
            Else
                Return String.Empty
            End If
        End If
        'Dim itemValue As Object = dr.Item(Utilities.SanitizeInput(VAL_COL))
        'If itemValue IsNot Nothing AndAlso Not DBNull.Value.Equals(itemValue) Then
        '    Return Utilities.SanitizeInput(Utilities.NZ(itemValue))
        'Else
        '    Return String.Empty
        'End If
        Return Utilities.HtmlEncodeHtmlTag(Utilities.NZ(dr.Item(VAL_COL)))
    End Function
#End Region

#Region "設定ファイル更新チェック"
    Private Sub CheckUpdate()
        Dim fi As New FileInfo(Me._ConfigFileFullPath)
        If fi.LastWriteTime <> Me._fileInfo.LastWriteTime Then
            Me.Init()
        End If
    End Sub
#End Region

    Public Function GetConnectionString() As String
        Dim userConnString As String
        Select Case GlobalVariables.RunTimeEnvironment
            Case PublicEnum.eRuntimeEnvironment.PROD
                userConnString = ConfigurationManager.ConnectionStrings("csProd").ConnectionString
            Case PublicEnum.eRuntimeEnvironment.IT
                userConnString = ConfigurationManager.ConnectionStrings("csIT").ConnectionString
            Case PublicEnum.eRuntimeEnvironment.PRE
                userConnString = ConfigurationManager.ConnectionStrings("csPRE").ConnectionString
            Case PublicEnum.eRuntimeEnvironment.PREV
                userConnString = ConfigurationManager.ConnectionStrings("csPREVIEW").ConnectionString
            Case PublicEnum.eRuntimeEnvironment.VM
                userConnString = ConfigurationManager.ConnectionStrings("csVM").ConnectionString
            Case Else
                userConnString = ConfigurationManager.ConnectionStrings("csDev").ConnectionString
        End Select

        Return Utilities.HtmlEncodeHtmlTag(userConnString)
    End Function
End Class

