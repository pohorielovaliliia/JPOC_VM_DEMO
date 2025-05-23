Imports System.Data
Imports System.Data.SqlClient
Public Class DaoUsersUpdateAcceptance
    Inherits AbstractSoredProcedureDao

#Region "定数"
    Public Const PROC_NAME As String = "P_JP_Users_UpdateAcceptance"
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
                            ByVal pInstitutionID As Integer) As Integer
        Me.DbManager.SetSqlCommand(PROC_NAME, CommandType.StoredProcedure)
        Me.DbManager.SetCmdParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, pUserID)
        Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
        Return Me.DbManager.Execute()
    End Function
#End Region

End Class
