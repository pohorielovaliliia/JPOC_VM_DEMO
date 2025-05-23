Imports System.IO

Public Class GlobalVariables
    Inherits Jpoc.Dac.GlobalVariables

#Region "設定ファイルから取得する項目"

#Region "外部接続・接続文字列"
    Public Shared ReadOnly Property ConnectionString(ByVal pDatabaseName As String) As String
        Get
            Try
                Dim connectionStringList As New Dictionary(Of String, String)
                Dim s As String = String.Empty
                s = Configure.GetConfig("ConnectionStrings", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("DEFAULT", s)
                s = Configure.GetConfig("CON_STR_LOCAL", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("LOCAL", s)
                s = Configure.GetConfig("CON_STR_DEV", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("DEV", s)
                s = Configure.GetConfig("CON_STR_IT", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("IT", s)
                s = Configure.GetConfig("CON_STR_DEMO", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("DEMO", s)
                s = Configure.GetConfig("CON_STR_PREVIEW", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("PREVIEW", s)
                s = Configure.GetConfig("CON_STR_PROD", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("PROD", s)
                s = Configure.GetConfig("CON_STR_PRE", False)
                If Not String.IsNullOrEmpty(s) Then connectionStringList.Add("PRE", s)
                Return connectionStringList.Item(pDatabaseName)
            Catch ex As Exception
                Throw New ElsException(ex)
            End Try
        End Get
    End Property
    Private Shared ReadOnly Property WebServiceUrl(ByVal pWebServiceName As String) As String
        Get
            Try
                Dim WebServiceUrlList As New Dictionary(Of String, String)
                Dim s As String = String.Empty
                s = Configure.GetConfig("TOOLS_WEBSVC_URL_LOCAL", False)
                If Not String.IsNullOrEmpty(s) Then WebServiceUrlList.Add("LOCAL", s)
                s = Configure.GetConfig("TOOLS_WEBSVC_URL_DEV", False)
                If Not String.IsNullOrEmpty(s) Then WebServiceUrlList.Add("DEV", s)
                s = Configure.GetConfig("TOOLS_WEBSVC_URL_IT", False)
                If Not String.IsNullOrEmpty(s) Then WebServiceUrlList.Add("IT", s)
                s = Configure.GetConfig("TOOLS_WEBSVC_URL_UAT", False)
                If Not String.IsNullOrEmpty(s) Then WebServiceUrlList.Add("DEMO", s)
                s = Configure.GetConfig("TOOLS_WEBSVC_URL_PREVIEW", False)
                If Not String.IsNullOrEmpty(s) Then WebServiceUrlList.Add("PREVIEW", s)
                s = Configure.GetConfig("TOOLS_WEBSVC_URL_PRE", False)
                If Not String.IsNullOrEmpty(s) Then WebServiceUrlList.Add("PRE", s)
                Return WebServiceUrlList.Item(pWebServiceName)
            Catch ex As Exception
                Throw New ElsException(ex)
            End Try
        End Get
    End Property
#End Region

#End Region

#Region "WebService Endpointアドレス"
    Public Shared ReadOnly Property WebServiceUrlEndpointAddress(ByVal pWebServiceName As String) As System.ServiceModel.EndpointAddress
        Get
            Return New System.ServiceModel.EndpointAddress(GlobalVariables.WebServiceUrl(pWebServiceName))
        End Get
    End Property
#End Region

    Public Shared ReadOnly ExcelFormatAllowed As String() = New String() {".xlsx", ".xls"}
    Public Shared ReadOnly ExcelPDFFormatAllowed As String() = New String() {".xlsx", ".xls", ".pdf"}
    Public Shared ReadOnly ImageFormatAllowed As String() = New String() {".png", ".jpeg", ".jpg", ".gif"}
    Public Shared ReadOnly ZipFormatAllowed As String() = New String() {".zip"}
    Public Shared ReadOnly ZipsPDFFormatAllowed As String() = New String() {".zip", ".7z", ".pdf"}
    Public Shared ReadOnly SevenZipFormatAllowed As String() = New String() {".7z"}
    Public Shared ReadOnly TextFormatAllowed As String() = New String() {".txt"}
    Public Shared ReadOnly CSVFormatAllowed As String() = New String() {".csv"}
End Class

