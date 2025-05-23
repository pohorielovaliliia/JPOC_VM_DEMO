Imports System.Data
Imports System.Data.SqlClient
Public Class DaoDrugCustomizedGetJPC
    Inherits AbstractSoredProcedureDao

#Region "定数"
    Public Const PROC_NAME As String = "P_JP_DrugCustomizedGetJPC"
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
    Public Function Execute(ByRef pDt As DataTable, _
                            ByVal pJpc As String, _
                            ByVal pType As String, _
                            ByVal pCategory As String) As Integer
        Me.DbManager.SetSqlCommand(PROC_NAME, CommandType.StoredProcedure)
        Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
        Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pType)
        Me.DbManager.SetCmdParameter("@category", SqlDbType.NVarChar, ParameterDirection.Input, pCategory)
        Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
    End Function
#End Region

End Class
