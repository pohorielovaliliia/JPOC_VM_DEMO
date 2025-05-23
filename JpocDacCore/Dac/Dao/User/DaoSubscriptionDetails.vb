Public Class DaoSubscriptionDetails
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_SubscriptionDetails"
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
        Const SQL_QUERY As String = "SELECT COUNT(*) " &
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
    Public Overloads Function GetCount(ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " &
                                      "FROM " & TABLE_NAME
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region


#Region "施設IDによる取得"
    Public Function GetCountBySubscriptionID(ByVal pSubscriptionID As Integer,
                                     Optional pOnlyActive As Boolean = False) As Integer

        Const SQL_QUERY As String = "SELECT COUNT(*) " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE SubscriptionID = @SubscriptionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pSubscriptionID)
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
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE Active = 1 "
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
    Public Function GetByPK(ByRef pDt As DataTable,
                            ByVal pSubscriptionDetailsID As Integer,
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE Id = @Id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@Id", SqlDbType.Int, ParameterDirection.Input, pSubscriptionDetailsID)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "施設IDによる取得"
    Public Function GetBySubscriptionId(ByRef pDt As DataTable,
                                ByVal pSubscriptionID As Integer,
                                Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                    "FROM " & TABLE_NAME & " " &
                                    "WHERE SubscriptionID = @SubscriptionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pSubscriptionID)
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
    Public Function Delete(ByVal pSubscriptionDetailsID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " &
                                     "WHERE Id = @Id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@Id", SqlDbType.Int, ParameterDirection.Input, pSubscriptionDetailsID)
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
    Public Function Insert(ByVal SubscriptionId As Integer, ByVal CategoryId As Integer, ByVal _userid As Integer) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" &
                                                                         "SubscriptionID " &
                                                                        ",CategoryId " &
                                                                        ",Active " &
                                                                        ",created_by " &
                                                                        ",modified_by " &
                                                                    ") VALUES (" &
                                                                         "@SubscriptionID " &
                                                                        ",@CategoryId " &
                                                                        ",@Active " &
                                                                        ",@UserID " &
                                                                        ",@UserID " &
                                                                    ");"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, SubscriptionId)
            Me.DbManager.SetCmdParameter("@CategoryId", SqlDbType.Int, ParameterDirection.Input, CategoryId)
            Me.DbManager.SetCmdParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, True)
            Me.DbManager.SetCmdParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, _userid)
            Dim ret As Integer = 0
            ret = Me.DbManager.Execute()
            Return ret
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "更新"
    Public Function Update(ByVal SubscriptionDetailsId As String, ByVal Active As Boolean, ByVal _userid As Integer) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " &
                                       "SET " &
                                             "Active = @Active " &
                                            ",modified_by = @UserID " &
                                            ",modified_date = GETDATE() " &
                                     "WHERE Id = @Id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@Id", SqlDbType.Int, ParameterDirection.Input, Integer.Parse(SubscriptionDetailsId))
            Me.DbManager.SetCmdParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, Active)
            Me.DbManager.SetCmdParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, _userid)
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
