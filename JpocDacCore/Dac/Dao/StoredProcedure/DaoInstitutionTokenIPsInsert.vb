Imports System.Data
Imports System.Data.SqlClient
Public Class DaoInstitutionTokenIPsInsert
    Inherits AbstractSoredProcedureDao

#Region "定数"
    Public Const PROC_NAME As String = "P_JP_InstitutionTokenIPs_Insert"
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
    Public Function Execute(ByRef pDt As DataTable,
                            ByVal pInstitutionID As Integer,
                            ByVal pIPRange As String,
                             ByVal pNew As Boolean,
                             ByVal pIPRangeFull As String) As Integer
        Me.DbManager.SetSqlCommand(PROC_NAME, CommandType.StoredProcedure)
        Me.DbManager.SetCmdParameter("pInstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
        Me.DbManager.SetCmdParameter("pIPRange", SqlDbType.NChar, ParameterDirection.Input, pIPRange)
        Me.DbManager.SetCmdParameter("pNew", SqlDbType.NChar, ParameterDirection.Input, pNew)
        Me.DbManager.SetCmdParameter("pIPRangeFull", SqlDbType.NChar, ParameterDirection.Input, pIPRangeFull)
        Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
    End Function
#End Region

End Class
