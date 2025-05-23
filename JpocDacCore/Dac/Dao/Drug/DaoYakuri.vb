Public Class DaoYakuri
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Yakuri"
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

#Region "Jpc(Code)による取得"
    Public Function GetByJpcCode(ByRef pDt As DataTable, _
                             ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "SELECT y.* " & _
                                    "FROM " & TABLE_NAME & " y " & _
                                    "WHERE EXISTS (" & _
                                    " SELECT * FROM " & DaoDrugData.TABLE_NAME & _
                                    " WHERE code = @code " & _
                                    " AND ( category_id1 = y.category_id " & _
                                    " OR category_id2 = y.category_id  " & _
                                    " OR category_id3 = y.category_id ) " & _
                                    " ) ORDER BY y.sequence"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatCode(pJpc))
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_YakuriRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "category_id " & _
                                                                        ",yakuri_link_label " & _
                                                                        ",yakuri_link " & _
                                                                        ",yakuri_link2 " & _
                                                                        ",categories " & _
                                                                        ",sequence " & _
                                                                        ",created_by " & _
                                                                        ",modified_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_date " & _
                                                                    ") VALUES (" & _
                                                                         "@category_id" & _
                                                                        ",@yakuri_link_label " & _
                                                                        ",@yakuri_link " & _
                                                                        ",@yakuri_link2 " & _
                                                                        ",@sequence " & _
                                                                        ",@created_by " & _
                                                                        ",@modified_by " & _
                                                                        ",@created_date " & _
                                                                        ",@modified_date " & _
                                                                    ")"

        Try
            Dim strSQL As String = String.Empty
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@category_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id)
            If Not pDr.IsNull("yakuri_link_label") Then
                Me.DbManager.SetCmdParameter("@yakuri_link_label", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yakuri_link_label)
            Else
                Me.DbManager.SetCmdParameter("@yakuri_link_label", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yakuri_link") Then
                Me.DbManager.SetCmdParameter("@yakuri_link", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yakuri_link)
            Else
                Me.DbManager.SetCmdParameter("@yakuri_link", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yakuri_link2") Then
                Me.DbManager.SetCmdParameter("@yakuri_link2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yakuri_link2)
            Else
                Me.DbManager.SetCmdParameter("@yakuri_link2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, 0)
            End If
            If Not pDr.IsNull("created_by") Then
                Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Else
                Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, 0)
            End If
            If Not pDr.IsNull("modified_by") Then
                Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Else
                Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, 0)
            End If
            If Not pDr.IsNull("created_date") Then
                Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Else
                Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow())
            End If
            If Not pDr.IsNull("modified_date") Then
                Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Else
                Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow())
            End If

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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_YakuriRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "category_id = @category_id " & _
                                            ",yakuri_link_label = @yakuri_link_label " & _
                                            ",yakuri_link = @yakuri_link " & _
                                            ",yakuri_link2 = @yakuri_link2 " & _
                                            ",sequence = @sequence " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@category_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id)
            If Not pDr.IsNull("yakuri_link_label") Then
                Me.DbManager.SetCmdParameter("@yakuri_link_label", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yakuri_link_label)
            Else
                Me.DbManager.SetCmdParameter("@yakuri_link_label", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yakuri_link") Then
                Me.DbManager.SetCmdParameter("@yakuri_link", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yakuri_link)
            Else
                Me.DbManager.SetCmdParameter("@yakuri_link", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yakuri_link2") Then
                Me.DbManager.SetCmdParameter("@yakuri_link2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yakuri_link2)
            Else
                Me.DbManager.SetCmdParameter("@yakuri_link2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, 0)
            End If
            If Not pDr.IsNull("modified_by") Then
                Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Else
                Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, 0)
            End If
            If Not pDr.IsNull("modified_date") Then
                Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Else
                Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow())
            End If
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

#Region "Code(JPC)整形"
    Private Function FormatCode(ByVal pJpc As String) As String
        Dim code As String = pJpc.Split("#"c)(0)
        Return Int32.Parse(code).ToString("00000000")
    End Function
#End Region

End Class
