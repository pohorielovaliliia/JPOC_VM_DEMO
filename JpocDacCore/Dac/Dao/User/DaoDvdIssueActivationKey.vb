Public Class DaoDvdIssueActivationKey
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DvdIssueActivationKey"
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
            'If pOnlyActive Then strSql &= " WHERE Active = 1 "
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
    Public Function GetCountByPK(ByVal pId As Integer, _
                                 Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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

#Region "serial_idによる取得"
    Public Function GetCountBySerialId(ByVal pSerialId As String, _
                                       Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE serial_id = @serial_id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@serial_id", SqlDbType.Char, ParameterDirection.Input, pSerialId)
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
            'If pOnlyActive Then strSql &= " WHERE Active = 1 "
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
                            ByVal pId As Integer,
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND Active = 1 "
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

#Region "SerialIdによる取得"
    Public Function GetBySerialId(ByRef pDt As DataTable, _
                            ByVal pSerialId As String,
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                    "FROM " & TABLE_NAME & " " & _
                                    "WHERE serial_id = @serial_id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@serial_id", SqlDbType.Char, ParameterDirection.Input, pSerialId)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_DvdIssueActivationKeyRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "serial_id " & _
                                                                        ",device_id " & _
                                                                        ",start_date_jst " & _
                                                                        ",number_of_times " & _
                                                                        ",limit_date_jst " & _
                                                                        ",created_date " & _
                                                                        ",modified_date " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@serial_id " & _
                                                                        ",@device_id " & _
                                                                        ",@start_date_jst " & _
                                                                        ",@number_of_times " & _
                                                                        ",@limit_date_jst " & _
                                                                        ",@created_date " & _
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
            Me.DbManager.SetCmdParameter("@serial_id", SqlDbType.Char, ParameterDirection.Input, pDr.serial_id)
            Me.DbManager.SetCmdParameter("@device_id", SqlDbType.Char, ParameterDirection.Input, pDr.device_id)
            If Not pDr.IsNull("start_date_jst") Then
                Me.DbManager.SetCmdParameter("@start_date_jst", SqlDbType.Date, ParameterDirection.Input, pDr.start_date_jst)
            Else
                Me.DbManager.SetCmdParameter("@start_date_jst", SqlDbType.Date, ParameterDirection.Input, GlobalVariables.JpnDate)
            End If
            If Not pDr.IsNull("number_of_times") Then
                Me.DbManager.SetCmdParameter("@number_of_times", SqlDbType.Int, ParameterDirection.Input, pDr.number_of_times)
            Else
                Me.DbManager.SetCmdParameter("@number_of_times", SqlDbType.Int, ParameterDirection.Input, 1)
            End If
            If Not pDr.IsNull("limit_date_jst") Then
                Me.DbManager.SetCmdParameter("@limit_date_jst", SqlDbType.Date, ParameterDirection.Input, pDr.limit_date_jst)
            Else
                Me.DbManager.SetCmdParameter("@limit_date_jst", SqlDbType.Date, ParameterDirection.Input, pDr.start_date_jst.AddYears(1))
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_DvdIssueActivationKeyRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                            "serial_id = @serial_id " & _
                                            ",device_id = @device_id " & _
                                            ",apply_date_jst = @apply_date_jst " & _
                                            ",apply_code = @apply_code " & _
                                            ",number_of_times = @number_of_times " & _
                                            ",limit_date_jst = @limit_date_jst " & _
                                            ",created_date = @created_date " & _
                                            ",modified_date = @modified_date " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@serial_id", SqlDbType.Char, ParameterDirection.Input, pDr.serial_id)
            Me.DbManager.SetCmdParameter("@device_id", SqlDbType.Char, ParameterDirection.Input, pDr.device_id)
            Me.DbManager.SetCmdParameter("@start_date_jst", SqlDbType.Date, ParameterDirection.Input, pDr.start_date_jst)
            Me.DbManager.SetCmdParameter("@number_of_times", SqlDbType.Int, ParameterDirection.Input, pDr.number_of_times)
            Me.DbManager.SetCmdParameter("@limit_date_jst", SqlDbType.Date, ParameterDirection.Input, pDr.limit_date_jst)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
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
