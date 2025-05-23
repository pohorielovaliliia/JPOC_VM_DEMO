Public Class DaoRoles
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Roles"
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
    Public Function GetCountByPK(ByVal pRoleID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE RoleID = @RoleID "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pRoleID)
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
                            ByVal pRoleID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE RoleID = @RoleID "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pRoleID)
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
    Public Function Delete(ByVal pRoleID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE RoleID = @RoleID "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pRoleID)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_RolesRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "RoleID " & _
                                                                        ",RoleTarget " & _
                                                                        ",RoleNameEN " & _
                                                                        ",RoleNameJP " & _
                                                                        ",created_by " & _
                                                                        ",modified_by " & _
                                                                        ",checkout_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_date " & _
                                                                    ") VALUES (" & _
                                                                         "@RoleID " & _
                                                                        ",@RoleTarget " & _
                                                                        ",@RoleNameEN " & _
                                                                        ",@RoleNameJP " & _
                                                                        ",@created_by " & _
                                                                        ",@modified_by " & _
                                                                        ",@checkout_by " & _
                                                                        ",@created_date " & _
                                                                        ",@modified_date " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pDr.RoleID)
            Me.DbManager.SetCmdParameter("@RoleTarget", SqlDbType.NVarChar, ParameterDirection.Input, pDr.RoleTarget)
            Me.DbManager.SetCmdParameter("@RoleNameEN", SqlDbType.NVarChar, ParameterDirection.Input, pDr.RoleNameEN)
            Me.DbManager.SetCmdParameter("@RoleNameJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.RoleNameJP)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_RolesRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "RoleID = @RoleID " & _
                                            ",RoleTarget = @RoleTarget " & _
                                            ",RoleNameEN = @RoleNameEN " & _
                                            ",RoleNameJP = @RoleNameJP " & _
                                            ",created_by = @created_by " & _
                                            ",modified_by = @modified_by " & _
                                            ",checkout_by = @checkout_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_date = @modified_date " & _
                                     "WHERE RoleID = @RoleID_ORG "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@RoleID", SqlDbType.Int, ParameterDirection.Input, pDr.RoleID)
            Me.DbManager.SetCmdParameter("@RoleTarget", SqlDbType.NVarChar, ParameterDirection.Input, pDr.RoleTarget)
            Me.DbManager.SetCmdParameter("@RoleNameEN", SqlDbType.NVarChar, ParameterDirection.Input, pDr.RoleNameEN)
            Me.DbManager.SetCmdParameter("@RoleNameJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.RoleNameJP)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@RoleID_ORG", SqlDbType.Int, ParameterDirection.Input, pDr.Item("RoleID", DataRowVersion.Original))
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
