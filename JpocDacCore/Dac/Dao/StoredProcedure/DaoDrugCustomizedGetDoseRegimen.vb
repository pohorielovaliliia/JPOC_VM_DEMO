Imports System.Data
Imports System.Data.SqlClient
Public Class DaoDrugCustomizedGetDoseRegimen
    Inherits AbstractSoredProcedureDao

#Region "定数"
    Public Const PROC_NAME As String = "P_JP_DrugCustomizedGetDoseRegimen"
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
                             ByVal pJpc As String) As Integer
        Me.DbManager.SetSqlCommand(PROC_NAME, CommandType.StoredProcedure)
        Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
        Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
    End Function
#End Region

End Class
