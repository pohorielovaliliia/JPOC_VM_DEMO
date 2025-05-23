Imports System.Data
Imports System.Data.SqlClient
Public Class DaoUserUpdateLoginDate
    Inherits AbstractSoredProcedureDao

#Region "定数"
    Public Const PROC_NAME As String = "P_JP_UserUpdateLoginDate"
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
    Public Function Execute(ByVal pUserID As Integer, _
                            ByVal pUserType As String, _
                            ByVal pLoginDate As DateTime, _
                            ByVal pSessionID As String, _
                            ByVal pSessionIdForSp As String) As Integer
        Me.DbManager.SetSqlCommand(PROC_NAME, CommandType.StoredProcedure)
        Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pUserID)
        Me.DbManager.SetCmdParameter("@user_type", SqlDbType.VarChar, ParameterDirection.Input, pUserType)
        Me.DbManager.SetCmdParameter("@last_login_date", SqlDbType.DateTime, ParameterDirection.Input, pLoginDate)
        Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, pSessionID)
        Me.DbManager.SetCmdParameter("@session_id_for_sp", SqlDbType.VarChar, ParameterDirection.Input, pSessionIdForSp)
        Return Me.DbManager.Execute()
    End Function
#End Region

End Class
