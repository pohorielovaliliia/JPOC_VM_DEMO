Public Class DaoRolesAccess
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Roles_Access"
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

#Region "Dispose"
    Public Shadows Sub Dispose()
        MyBase.Dispose()
    End Sub
#End Region

#Region "件数取得"

#Region "全件取得"
    Public Overrides Function GetCount() As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "PrimaryKeyによる取得"
    Public Function GetCountByPK(ByVal pRoleID As Integer, _
                                 ByVal pFunctionID As Integer, _
                                 ByVal pActionID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE RoleID = @RoleID " & _
                                       "AND FunctionID = @FunctionID " & _
                                       "AND ActionID = @ActionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pRoleID)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pFunctionID)
            Me.DbManager.SetCmdParameter("@ActionID", SqlDbType.Int, ParameterDirection.Input, pActionID)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#End Region

#Region "射影取得"

#Region "全件取得"
    Public Overrides Function GetAll(ByRef pDt As System.Data.DataTable, pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " "
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " WHERE " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "PrimaryKeyによる取得"
    Public Function GetByPK(ByRef pDt As DataTable, _
                            ByVal pRoleID As Integer, _
                            ByVal pFunctionID As Integer, _
                            ByVal pActionID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE RoleID = @RoleID " & _
                                       "AND FunctionID = @FunctionID " & _
                                       "AND ActionID = @ActionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pRoleID)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pFunctionID)
            Me.DbManager.SetCmdParameter("@ActionID", SqlDbType.Int, ParameterDirection.Input, pActionID)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#End Region

#Region "削除"
    Public Function Delete(ByVal pRoleID As Integer, _
                           ByVal pFunctionID As Integer, _
                           ByVal pActionID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE RoleID = @RoleID " & _
                                       "AND FunctionID = @FunctionID " & _
                                       "AND ActionID = @ActionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pRoleID)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pFunctionID)
            Me.DbManager.SetCmdParameter("@ActionID", SqlDbType.Int, ParameterDirection.Input, pActionID)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "追加"
    Public Function Insert(ByRef pDr As DS_USER.T_JP_Roles_AccessRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "RoleID " & _
                                                                        ",FunctionID " & _
                                                                        ",ActionID " & _
                                                                    ") VALUES (" & _
                                                                         "@RoleID " & _
                                                                        ",@FunctionID " & _
                                                                        ",@ActionID " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pDr.RoleID)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pDr.FunctionID)
            Me.DbManager.SetCmdParameter("@ActionID", SqlDbType.Int, ParameterDirection.Input, pDr.ActionID)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "更新"
    Public Function Update(ByRef pDr As DS_USER.T_JP_Roles_AccessRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "RoleID = @RoleID " & _
                                            ",FunctionID = @FunctionID " & _
                                            ",ActionID = @ActionID " & _
                                     "WHERE RoleID = @RoleID_ORG " & _
                                       "AND FunctionID = @FunctionID_ORG " & _
                                       "AND ActionID = @ActionID_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pDr.RoleID)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pDr.FunctionID)
            Me.DbManager.SetCmdParameter("@ActionID", SqlDbType.Int, ParameterDirection.Input, pDr.ActionID)
            Me.DbManager.SetCmdParameter("@RoleID_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("RoleID", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@FunctionID_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("FunctionID", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@ActionID_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("ActionID", DataRowVersion.Original)))
            Return Me.DbManager.Execute
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

End Class
