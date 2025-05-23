Public Class DaoRolesFunction
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Roles_Function"
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
    Public Function GetCountByPK(ByVal pFunctionID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE FunctionID = @FunctionID "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pFunctionID)
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
                            ByVal pFunctionID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE FunctionID = @FunctionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pFunctionID)
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
    Public Function Delete(ByVal pFunctionID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE FunctionID = @FunctionID "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pFunctionID)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_Roles_FunctionRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "FunctionID " & _
                                                                        ",ParentID " & _
                                                                        ",RoleID " & _
                                                                        ",SequenceOrder " & _
                                                                        ",MenuName " & _
                                                                        ",URL " & _
                                                                        ",Abbr_Function " & _
                                                                    ") VALUES (" & _
                                                                         "@FunctionID " & _
                                                                        ",@ParentID " & _
                                                                        ",@RoleID " & _
                                                                        ",@SequenceOrder " & _
                                                                        ",@MenuName " & _
                                                                        ",@URL " & _
                                                                        ",@Abbr_Function " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pDr.FunctionID)
            Me.DbManager.SetCmdParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, pDr.ParentID)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pDr.RoleID)
            Me.DbManager.SetCmdParameter("@SequenceOrder", SqlDbType.Int, ParameterDirection.Input, pDr.SequenceOrder)
            Me.DbManager.SetCmdParameter("@MenuName", SqlDbType.NVarChar, ParameterDirection.Input, pDr.MenuName)
            Me.DbManager.SetCmdParameter("@URL", SqlDbType.VarChar, ParameterDirection.Input, pDr.URL)
            Me.DbManager.SetCmdParameter("@Abbr_Function", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Abbr_Function)
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_Roles_FunctionRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "FunctionID = @FunctionID " & _
                                            ",ParentID = @ParentID " & _
                                            ",RoleID = @RoleID " & _
                                            ",SequenceOrder = @SequenceOrder " & _
                                            ",MenuName = @MenuName " & _
                                            ",URL = @URL " & _
                                            ",Abbr_Function = @Abbr_Function " & _
                                     "WHERE FunctionID = @FunctionID_ORG "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@FunctionID", SqlDbType.Int, ParameterDirection.Input, pDr.FunctionID)
            Me.DbManager.SetCmdParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, pDr.ParentID)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pDr.RoleID)
            Me.DbManager.SetCmdParameter("@SequenceOrder", SqlDbType.Int, ParameterDirection.Input, pDr.SequenceOrder)
            Me.DbManager.SetCmdParameter("@MenuName", SqlDbType.NVarChar, ParameterDirection.Input, pDr.MenuName)
            Me.DbManager.SetCmdParameter("@URL", SqlDbType.VarChar, ParameterDirection.Input, pDr.URL)
            Me.DbManager.SetCmdParameter("@Abbr_Function", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Abbr_Function)
            Me.DbManager.SetCmdParameter("@FunctionID_ORG", SqlDbType.Int, ParameterDirection.Input, pDr.Item("FunctionID", DataRowVersion.Original))
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
