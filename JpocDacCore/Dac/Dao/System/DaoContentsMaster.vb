Public Class DaoContentsMaster
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_ContentsMaster"
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
    Public Overloads Function GetCount(ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE " & MyBase.ActiveCondition
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

#Region "PrimaryKeyによる取得"
    Public Function GetCountByPK(ByVal pID As Integer, _
                                 ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
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
            If pOnlyActive Then strSql &= " WHERE " & MyBase.ActiveCondition
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
                            ByVal pId As Integer, _
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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
    Public Function Insert(ByRef pDr As DS_SYSTEM.T_JP_ContentsMasterRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "content_type " & _
                                                                        ",content_name " & _
                                                                        ",Description " & _
                                                                        ",display " & _
                                                                        ",content_category " & _
                                                                        ",content_subcategory " & _
                                                                        ",sequence " & _
                                                                        ",status " & _
                                                                        ",defunct " & _
                                                                        ",created_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_by " & _
                                                                        ",modified_date " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@content_type " & _
                                                                        ",@content_name " & _
                                                                        ",@Description " & _
                                                                        ",@display " & _
                                                                        ",@content_category " & _
                                                                        ",@content_subcategory " & _
                                                                        ",@sequence " & _
                                                                        ",@status " & _
                                                                        ",@defunct " & _
                                                                        ",@created_by " & _
                                                                        ",@created_date " & _
                                                                        ",@modified_by " & _
                                                                        ",@modified_date " & _
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
            Me.DbManager.SetCmdParameter("@content_type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_type)
            Me.DbManager.SetCmdParameter("@content_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_name)
            If Not pDr.IsNull("Description") Then
                Me.DbManager.SetCmdParameter("@Description", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Description)
            Else
                Me.DbManager.SetCmdParameter("@Description", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@display", SqlDbType.Bit, ParameterDirection.Input, pDr.display)
            If Not pDr.IsNull("content_category") Then
                Me.DbManager.SetCmdParameter("@content_category", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_category)
            Else
                Me.DbManager.SetCmdParameter("@content_category", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("content_subcategory") Then
                Me.DbManager.SetCmdParameter("@content_subcategory", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_subcategory)
            Else
                Me.DbManager.SetCmdParameter("@content_subcategory", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@status", SqlDbType.NVarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
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
    Public Function Update(ByRef pDr As DS_SYSTEM.T_JP_ContentsMasterRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "content_type = @content_type " & _
                                            ",content_name = @content_name " & _
                                            ",Description = @Description " & _
                                            ",display = @display " & _
                                            ",content_category = @content_category " & _
                                            ",content_subcategory = @content_subcategory " & _
                                            ",sequence = @sequence " & _
                                            ",status = @status " & _
                                            ",defunct = @defunct " & _
                                            ",created_by = @created_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@content_type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_type)
            Me.DbManager.SetCmdParameter("@content_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_name)
            If Not pDr.IsNull("Description") Then
                Me.DbManager.SetCmdParameter("@Description", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Description)
            Else
                Me.DbManager.SetCmdParameter("@Description", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@display", SqlDbType.Bit, ParameterDirection.Input, pDr.display)
            If Not pDr.IsNull("content_category") Then
                Me.DbManager.SetCmdParameter("@content_category", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_category)
            Else
                Me.DbManager.SetCmdParameter("@content_category", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("content_subcategory") Then
                Me.DbManager.SetCmdParameter("@content_subcategory", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_subcategory)
            Else
                Me.DbManager.SetCmdParameter("@content_subcategory", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@status", SqlDbType.NVarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
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
