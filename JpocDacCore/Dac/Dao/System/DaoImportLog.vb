Public Class DaoImportLog
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_ImportLog"
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
    Public Function GetCountByPK(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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

#Region "DiseaseIDによる取得"
    Public Function GetCountByDiseaseID(ByVal pDiseaseID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)
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

#Region "DiseaseIDによる取得"
    Public Function GetByDiseaseID(ByRef pDt As DataTable, _
                                   ByVal pDiseaseID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DiseaseIDによる取得"
    Public Function GetByDiseaseIDAndSessionIdAndTryCount(ByRef pDt As DataTable, _
                                   ByVal pDiseaseID As Integer, _
                                   ByVal pSessionId As String, _
                                   ByVal pTryTime As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id" & _
                                     " AND session_id = @session_id" & _
                                                                  " AND try_time = @try_time"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)
            Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, pSessionId)
            Me.DbManager.SetCmdParameter("@try_time", SqlDbType.Int, ParameterDirection.Input, pTryTime)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SessionIDによる取得"
    Public Function GetBySessionId(ByRef pDt As DataTable, _
                                   ByVal pSessionId As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE session_id = @session_id " & _
                                     " AND try_time = (SELECT MAX(try_time) FROM " & TABLE_NAME & " " & _
                                        "  WHERE session_id = @session_id )"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, pSessionId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SessionIDによる取得"
    Public Function GetMaxTryTimeBySessionId(ByVal pSessionId As String) As Integer

        Const SQL_QUERY As String = " SELECT MAX(try_time) " & _
                                    " FROM " & TABLE_NAME & " " & _
                                    " WHERE session_id = @session_id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, pSessionId)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SessionIDによる取得"
    Public Function GetBySessionIdAndDiseaseID(ByRef pDt As DataTable, _
                                               ByVal pSessionId As String, _
                                               ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE session_id = @session_id " & _
                                     " AND try_time = (SELECT MAX(try_time) FROM " & TABLE_NAME & " " & _
                                        "  WHERE session_id = @session_id ) " & _
                                     " AND disease_id = @disease_id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, pSessionId)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "更新時間指定による取得"
    ''' <summary>
    ''' 更新時間指定による取得
    ''' </summary>
    ''' <param name="pDt"></param>
    ''' <param name="pStartDate">取得開始時間</param>
    ''' <param name="pEndDate">取得終了時間（設定しない場合は現在時刻）</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByModefiedDate(ByRef pDt As DataTable, _
                                      ByVal pStartDate As DateTime, _
                                      ByVal pEndDate As DateTime) As Integer
        Const SQL_QUERY As String = " SELECT * " & _
                                    " FROM " & TABLE_NAME & " " & _
                                    " WHERE modified_date " & _
                                    " BETWEEN @startDate AND @endDate"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@startDate", SqlDbType.DateTime, ParameterDirection.Input, pStartDate)
            Me.DbManager.SetCmdParameter("@endDate", SqlDbType.DateTime, ParameterDirection.Input, pEndDate)
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
    Public Function Insert(ByRef pDr As DS_SYSTEM.T_JP_ImportLogRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "disease_id " & _
                                                                        ",message " & _
                                                                        ",sequence " & _
                                                                        ",msg_date " & _
                                                                        ",Complete " & _
                                                                        ",file_date " & _
                                                                        ",file_size " & _
                                                                        ",file_name " & _
                                                                        ",session_id " & _
                                                                        ",try_time " & _
                                                                        ",created_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_by " & _
                                                                        ",modified_date " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@disease_id " & _
                                                                        ",@message " & _
                                                                        ",@sequence " & _
                                                                        ",@msg_date " & _
                                                                        ",@Complete " & _
                                                                        ",@file_date " & _
                                                                        ",@file_size " & _
                                                                        ",@file_name " & _
                                                                        ",@session_id " & _
                                                                        ",@try_time " & _
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
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            If Not pDr.IsNull("message") Then
                Me.DbManager.SetCmdParameter("@message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.message)
            Else
                Me.DbManager.SetCmdParameter("@message", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("msg_date") Then
                Me.DbManager.SetCmdParameter("@msg_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.msg_date)
            Else
                Me.DbManager.SetCmdParameter("@msg_date", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Complete") Then
                Me.DbManager.SetCmdParameter("@Complete", SqlDbType.Int, ParameterDirection.Input, pDr.Complete)
            Else
                Me.DbManager.SetCmdParameter("@Complete", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_date") Then
                Me.DbManager.SetCmdParameter("@file_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.file_date)
            Else
                Me.DbManager.SetCmdParameter("@file_date", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_size") Then
                Me.DbManager.SetCmdParameter("@file_size", SqlDbType.Int, ParameterDirection.Input, pDr.file_size)
            Else
                Me.DbManager.SetCmdParameter("@file_size", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_name") Then
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.file_name)
            Else
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("session_id") Then
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.session_id)
            Else
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@try_time", SqlDbType.Int, ParameterDirection.Input, pDr.try_time)
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
    Public Function Update(ByRef pDr As DS_SYSTEM.T_JP_ImportLogRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "disease_id = @disease_id " & _
                                            ",message = @message " & _
                                            ",sequence = @sequence " & _
                                            ",msg_date = @msg_date " & _
                                            ",Complete = @Complete " & _
                                            ",file_date = @file_date " & _
                                            ",file_size = @file_size " & _
                                            ",file_name = @file_name " & _
                                            ",session_id = @session_id " & _
                                            ",try_time = @try_time " & _
                                            ",created_by = @created_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            If Not pDr.IsNull("message") Then
                Me.DbManager.SetCmdParameter("@message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.message)
            Else
                Me.DbManager.SetCmdParameter("@message", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("msg_date") Then
                Me.DbManager.SetCmdParameter("@msg_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.msg_date)
            Else
                Me.DbManager.SetCmdParameter("@msg_date", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Complete") Then
                Me.DbManager.SetCmdParameter("@Complete", SqlDbType.Int, ParameterDirection.Input, pDr.Complete)
            Else
                Me.DbManager.SetCmdParameter("@Complete", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_date") Then
                Me.DbManager.SetCmdParameter("@file_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.file_date)
            Else
                Me.DbManager.SetCmdParameter("@file_date", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_size") Then
                Me.DbManager.SetCmdParameter("@file_size", SqlDbType.Int, ParameterDirection.Input, pDr.file_size)
            Else
                Me.DbManager.SetCmdParameter("@file_size", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_name") Then
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.file_name)
            Else
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("session_id") Then
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.session_id)
            Else
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@try_time", SqlDbType.Int, ParameterDirection.Input, pDr.try_time)
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

    Public Function GetBySessionIdAndFileName(ByRef pDt As DataTable,
                                               ByVal pSessionId As String,
                                               ByVal pFileName As String) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE session_id = @session_id " &
                                     " AND try_time = (SELECT MAX(try_time) FROM " & TABLE_NAME & " " &
                                        "  WHERE session_id = @session_id ) " &
                                     " AND file_name = @file_name "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@session_id", SqlDbType.NVarChar, ParameterDirection.Input, pSessionId)
            Me.DbManager.SetCmdParameter("@file_name", SqlDbType.NVarChar, ParameterDirection.Input, pFileName)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
End Class
