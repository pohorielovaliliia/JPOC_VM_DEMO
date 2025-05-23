Imports System.Data
Imports System.Data.SqlClient
Public Class DaoUserLogin
    Inherits AbstractSoredProcedureDao

#Region "定数"
    Public Const PROC_NAME As String = "P_JP_UserLogin"
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
    Public Function Execute(ByVal pUserID As String,
                            ByVal pInstitutionCode As String,
                            ByVal pIpAddress As String,
                            ByVal pLoginDate As DateTime,
                            ByRef pDt As DataTable,
                            Optional emailformat As Boolean = False) As Integer
        Me.DbManager.SetSqlCommand(PROC_NAME, CommandType.StoredProcedure)
        Me.DbManager.SetCmdParameter("@uid", SqlDbType.VarChar, ParameterDirection.Input, pUserID)
        Me.DbManager.SetCmdParameter("@ins_code", SqlDbType.NVarChar, ParameterDirection.Input, pInstitutionCode)
        Me.DbManager.SetCmdParameter("@ip", SqlDbType.VarChar, ParameterDirection.Input, pIpAddress)
        Me.DbManager.SetCmdParameter("@login_date", SqlDbType.DateTime, ParameterDirection.Input, pLoginDate)
        Me.DbManager.SetCmdParameter("@emailformat", SqlDbType.Bit, ParameterDirection.Input, emailformat)
        Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
    End Function

#End Region

End Class
