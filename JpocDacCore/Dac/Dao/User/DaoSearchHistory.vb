Public Class DaoSearchHistory
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_SearchHistory"
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
    Public Function GetCountByPK(ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pId)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "UniqueID及び検索文字列よる取得"
    Public Function GetCountByUniqueIdAndText(ByVal pUniqueId As String, pSearchText As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                    "FROM " & TABLE_NAME & " " & _
                                    "WHERE unique_identity = @unique_identity " & _
                                    "AND search_text = @search_text"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@unique_identity", SqlDbType.VarChar, ParameterDirection.Input, pUniqueId)
            Me.DbManager.SetCmdParameter("@search_text", SqlDbType.NVarChar, ParameterDirection.Input, pSearchText)
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
                            ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "UniqueIDによる取得"
    Public Function GetByUniqueId(ByRef pDt As DataTable, _
                                  ByVal pUniqueId As String, _
                                  Optional ByVal pGetCount As Integer = 0) As Integer
        Const SQL_QUERY As String = "SELECT {0} * " & _
                                    "FROM " & TABLE_NAME & " " & _
                                    "WHERE unique_identity = @unique_identity " & _
                                    "ORDER BY create_date DESC "
        Try
            Dim sqlQuery As String = String.Empty
            If pGetCount = 0 Then
                sqlQuery = String.Format(SQL_QUERY, "")
            Else
                sqlQuery = String.Format(SQL_QUERY, "TOP " & pGetCount)
            End If
            Me.DbManager.SetSqlCommand(sqlQuery, CommandType.Text)
            Me.DbManager.SetCmdParameter("@unique_identity", SqlDbType.VarChar, ParameterDirection.Input, pUniqueId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "UniqueID及び検索文字列よる取得"
    Public Function GetByUniqueIdAndText(ByRef pDt As DataTable, ByVal pUniqueId As String, pSearchText As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                    "FROM " & TABLE_NAME & " " & _
                                    "WHERE unique_identity = @unique_identity " & _
                                    "AND search_text = @search_text"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@unique_identity", SqlDbType.VarChar, ParameterDirection.Input, pUniqueId)
            Me.DbManager.SetCmdParameter("@search_text", SqlDbType.NVarChar, ParameterDirection.Input, pSearchText)
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
    Public Function Delete(ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pId)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_SearchHistoryRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                        "unique_identity " & _
                                                                        ",search_text " & _
                                                                        ",create_date " & _
                                                                        ",searched_count " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                        "@unique_identity " & _
                                                                        ",@search_text " & _
                                                                        ",@create_date " & _
                                                                        ",@searched_count " & _
                                                                        "{1}" & _
                                                                    ");"

        Try
            Dim strSQL As String = String.Empty
            If pKeepIdValue Then
                strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) & _
                         String.Format(SQL_QUERY, ",id ", ",@id ") & _
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@unique_identity", SqlDbType.VarChar, ParameterDirection.Input, pDr.unique_identity)
            Me.DbManager.SetCmdParameter("@search_text", SqlDbType.NVarChar, ParameterDirection.Input, pDr.search_text)
            Me.DbManager.SetCmdParameter("@create_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.create_date)
            Me.DbManager.SetCmdParameter("@searched_count", SqlDbType.Int, ParameterDirection.Input, pDr.searched_count)

            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                pDr.id = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                If pDr.id > 0 Then ret = 1
            End If
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_SearchHistoryRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "unique_identity = @unique_identity " & _
                                            ",search_text = @search_text " & _
                                            ",create_date = @create_date " & _
                                            ",searched_count = @searched_count " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)

            Me.DbManager.SetCmdParameter("@unique_identity", SqlDbType.VarChar, ParameterDirection.Input, pDr.unique_identity)
            Me.DbManager.SetCmdParameter("@search_text", SqlDbType.NVarChar, ParameterDirection.Input, pDr.search_text)
            Me.DbManager.SetCmdParameter("@create_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.create_date)
            Me.DbManager.SetCmdParameter("@searched_count", SqlDbType.Int, ParameterDirection.Input, pDr.searched_count)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
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
