Imports System.Data
Imports System.Data.SqlClient
Public Class DaoAccessLogInsert
    Inherits AbstractSoredProcedureDao

#Region "定数"
    Public Const PROC_NAME As String = "P_JP_AccessLogInsert"
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDbmanager As ElsDataBase)
        MyBase.New(pDbmanager)
    End Sub
#End Region

#Region "Init"
    Public Overrides Sub Init()

    End Sub
#End Region

#Region "実行"
    Public Function Execute(ByVal pIp As Object,
                            ByVal pInstitutionId As Object,
                            ByVal pUserId As Object,
                            ByVal pContentType As String,
                            ByVal pContentName As Object,
                            ByVal pAttribute01 As Object,
                            ByVal pAttribute02 As Object,
                            ByVal pAttribute03 As Object,
                            ByVal pAttribute04 As Object,
                            ByVal pAttribute05 As Object,
                            ByVal pAttribute06 As Object,
                            ByVal pAttribute07 As Object,
                            ByVal pAttribute08 As Object,
                            ByVal pAttribute09 As Object,
                            ByVal pAttribute10 As Object,
                            ByVal pAttribute11 As Object,
                            ByVal pAttribute12 As Object,
                            ByVal pAttribute13 As Object,
                            ByVal pAttribute14 As Object,
                            ByVal pAttribute18 As Object,
                            ByVal pAttribute19 As Object, Optional ByVal pAttribute20 As Object = Nothing) As Integer

        Me.DbManager.SetSqlCommand(PROC_NAME, CommandType.StoredProcedure)
        DbManager.SetCmdParameter("ip", SqlDbType.NVarChar, ParameterDirection.Input, pIp)
        DbManager.SetCmdParameter("institution_id", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
        DbManager.SetCmdParameter("user_id", SqlDbType.Int, ParameterDirection.Input, pUserId)
        DbManager.SetCmdParameter("content_type", SqlDbType.NVarChar, ParameterDirection.Input, pContentType)
        DbManager.SetCmdParameter("content_name", SqlDbType.NVarChar, ParameterDirection.Input, pContentName)
        DbManager.SetCmdParameter("Attribute01", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute01)
        DbManager.SetCmdParameter("Attribute02", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute02)
        DbManager.SetCmdParameter("Attribute03", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute03)
        DbManager.SetCmdParameter("Attribute04", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute04)
        DbManager.SetCmdParameter("Attribute05", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute05)
        DbManager.SetCmdParameter("Attribute06", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute06)
        DbManager.SetCmdParameter("Attribute07", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute07)
        DbManager.SetCmdParameter("Attribute08", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute08)
        DbManager.SetCmdParameter("Attribute09", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute09)
        DbManager.SetCmdParameter("Attribute10", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute10)
        DbManager.SetCmdParameter("Attribute11", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute11)
        DbManager.SetCmdParameter("Attribute12", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute12)
        DbManager.SetCmdParameter("Attribute13", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute13)
        DbManager.SetCmdParameter("Attribute14", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute14)
        DbManager.SetCmdParameter("Attribute15", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
        DbManager.SetCmdParameter("Attribute16", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
        DbManager.SetCmdParameter("Attribute17", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
        'Attribute18 Sampleか？(='Sample')/本物か？(=null)/スマホか？(='SmartPhone')
        DbManager.SetCmdParameter("Attribute18", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute18)
        'Attribute19 Set to stored procedure.
        DbManager.SetCmdParameter("Attribute19", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute19)
        'Attribute20 Set to stored procedure.
        If pAttribute20 Is Nothing Then
            pAttribute20 = DBNull.Value
        End If
        DbManager.SetCmdParameter("Attribute20", SqlDbType.NVarChar, ParameterDirection.Input, pAttribute20)

        Return Me.DbManager.ExecuteAndGetDataTable(New DataTable, True)
    End Function
#End Region

End Class
