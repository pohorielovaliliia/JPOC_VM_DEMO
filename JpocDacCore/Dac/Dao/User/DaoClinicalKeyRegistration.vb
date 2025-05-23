Public Class DaoClinicalKeyRegistration
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_ClinicalKey_Registration"
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
    Public Function GetCountByUserId(ByVal pUserId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE user_id = @user_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pUserId)
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
    Public Function GetByUserId(ByRef pDt As DataTable, _
                                ByVal pUserId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE user_id = @user_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pUserId)
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
    'Public Function Insert(ByRef pDr As DS_USER.T_JP_SingleSignOnRow, _
    '                       ByVal pKeepIdValue As Boolean) As Integer
    '    Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
    '                                                                     "user_id " & _
    '                                                                    ",token " & _
    '                                                                    ",RedirectPageID " & _
    '                                                                    ",create_date " & _
    '                                                                    ",subscription_id " & _
    '                                                                    "{0}" & _
    '                                                                ") VALUES (" & _
    '                                                                     "@user_id " & _
    '                                                                    ",@token " & _
    '                                                                    ",@RedirectPageID " & _
    '                                                                    ",@create_date " & _
    '                                                                    ",@subscription_id " & _
    '                                                                    "{1}" & _
    '                                                                ");"

    '    Try
    '        Dim strSQL As String = String.Empty
    '        If pKeepIdValue Then
    '            strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) & _
    '                     String.Format(SQL_QUERY, ",id ", ",@id ") & _
    '                     String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
    '        Else
    '            strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
    '        End If
    '        Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
    '        If Not pDr.IsNull("user_id") Then
    '            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pDr.user_id)
    '        Else
    '            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
    '        End If
    '        Me.DbManager.SetCmdParameter("@token", SqlDbType.VarChar, ParameterDirection.Input, pDr.token)
    '        If Not pDr.IsNull("RedirectPageID") Then
    '            Me.DbManager.SetCmdParameter("@RedirectPageID", SqlDbType.Int, ParameterDirection.Input, pDr.RedirectPageID)
    '        Else
    '            Me.DbManager.SetCmdParameter("@RedirectPageID", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
    '        End If
    '        Me.DbManager.SetCmdParameter("@create_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.create_date)
    '        Me.DbManager.SetCmdParameter("@subscription_id", SqlDbType.Int, ParameterDirection.Input, pDr.subscription_id)
    '        Dim ret As Integer = 0
    '        If pKeepIdValue Then
    '            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
    '            ret = Me.DbManager.Execute()
    '        Else
    '            pDr.id = Utilities.N0Int(Me.DbManager.ExecuteScalar)
    '            If pDr.id > 0 Then ret = 1
    '        End If
    '        Return ret
    '    Catch ex As ElsException
    '        ex.AddStackFrame(New StackFrame(True))
    '        Throw ex
    '    Catch ex As Exception
    '        Throw New ElsException(ex)
    '    End Try
    'End Function
#End Region

#Region "更新"
    Public Function Update(ByRef pDr As DS_USER.T_JP_ClinicalKey_RegistrationRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "user_id = @user_id " & _
                                            ",RegistrationID = @RegistrationID " & _
                                            ",RegistrationPassword = @RegistrationPassword " & _
                                            ",first_display_datetime = @first_display_datetime " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pDr.user_id)
            Me.DbManager.SetCmdParameter("@RegistrationID", SqlDbType.VarChar, ParameterDirection.Input, pDr.RegistrationID)
            Me.DbManager.SetCmdParameter("@RegistrationPassword", SqlDbType.VarChar, ParameterDirection.Input, pDr.RegistrationPassword)
            Me.DbManager.SetCmdParameter("@first_display_datetime", SqlDbType.DateTime, ParameterDirection.Input, pDr.first_display_datetime)
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
