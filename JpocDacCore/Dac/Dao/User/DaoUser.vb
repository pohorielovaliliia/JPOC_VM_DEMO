Public Class DaoUser
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_User"
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
    Public Function GetCountByPK(ByVal pId As Integer,
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
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

#Region "loginIdによる取得"
    Public Function GetCountByLoginId(ByVal pLoginId As String,
                                      Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE uid = @uid"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@uid", SqlDbType.VarChar, ParameterDirection.Input, pLoginId)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "emailによる取得"
    Public Function GetCountByEmail(ByVal pEmail As String,
                                      Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE email = @email"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@email", SqlDbType.VarChar, ParameterDirection.Input, pEmail)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function

    Public Function GetInstCountByEmail(ByVal pEmail As String,
                                      Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM T_JP_InsUser " &
                                     "WHERE email = @email"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@email", SqlDbType.VarChar, ParameterDirection.Input, pEmail)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SubscriptionIdによる取得"
    Public Function GetCountBySubscriptionId(ByVal pSubscriptionId As Integer,
                                      Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE subscription_id = @subscription_id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@subscription_id", SqlDbType.Int, ParameterDirection.Input, pSubscriptionId)
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
                            ByVal pId As Integer,
                            Optional pOnlyActive As Boolean = False) As Integer
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

#Region "loginIdによる取得"
    Public Function GetByLoginId(ByRef pDt As DataTable, _
                                 ByVal pLoginId As String,
                                 Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE uid = @uid"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@uid", SqlDbType.VarChar, ParameterDirection.Input, pLoginId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "emailによる取得"
    Public Function GetByEmail(ByRef pDt As DataTable, _
                                 ByVal pEmail As String,
                                 Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE email = @email"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@email", SqlDbType.VarChar, ParameterDirection.Input, pEmail)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SubscriptionIdによる取得"
    Public Function GetBySubscriptionId(ByRef pDt As DataTable, _
                                 ByVal pSubscriptionId As Integer,
                                 Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE subscription_id = @subscription_id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@subscription_id", SqlDbType.Int, ParameterDirection.Input, pSubscriptionId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "RoleIdによる取得"
    Public Function GetByRoleId(ByRef pDt As DataTable, _
                                ByVal pRole As PublicEnum.eRole,
                                Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE user_role = @user_role"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@user_role", SqlDbType.Int, ParameterDirection.Input, pRole)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_UserRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "user_type " & _
                                                                        ",user_role " & _
                                                                        ",uid " & _
                                                                        ",password " & _
                                                                        ",salt " & _
                                                                        ",salutation " & _
                                                                        ",given_name " & _
                                                                        ",firstname_kata " & _
                                                                        ",lastname_kata " & _
                                                                        ",firstname_kanji " & _
                                                                        ",lastname_kanji " & _
                                                                        ",gender " & _
                                                                        ",additional_info " & _
                                                                        ",email " & _
                                                                        ",contact_num " & _
                                                                        ",last_login_date " & _
                                                                        ",status " & _
                                                                        ",created_date " & _
                                                                        ",created_by " & _
                                                                        ",modified_date " & _
                                                                        ",modified_by " & _
                                                                        ",defunct " & _
                                                                        ",subscription_id " & _
                                                                        ",login_expire " & _
                                                                        ",mail_permission " & _
                                                                        ",session_id " & _
                                                                        ",ban_flag " & _
                                                                        ",ban_datetime " & _
                                                                        ",login_count " & _
                                                                        ",session_id_for_sp " & _
                                                                        ",cancel_date_jst " & _
                                                                        ",is_credit_error " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@user_type " & _
                                                                        ",@user_role " & _
                                                                        ",@uid " & _
                                                                        ",@password " & _
                                                                        ",@salt " & _
                                                                        ",@salutation " & _
                                                                        ",@given_name " & _
                                                                        ",@firstname_kata " & _
                                                                        ",@lastname_kata " & _
                                                                        ",@firstname_kanji " & _
                                                                        ",@lastname_kanji " & _
                                                                        ",@gender " & _
                                                                        ",@additional_info " & _
                                                                        ",@email " & _
                                                                        ",@contact_num " & _
                                                                        ",@last_login_date " & _
                                                                        ",@status " & _
                                                                        ",@created_date " & _
                                                                        ",@created_by " & _
                                                                        ",@modified_date " & _
                                                                        ",@modified_by " & _
                                                                        ",@defunct " & _
                                                                        ",@subscription_id " & _
                                                                        ",@login_expire " & _
                                                                        ",@mail_permission " & _
                                                                        ",@session_id " & _
                                                                        ",@ban_flag " & _
                                                                        ",@ban_datetime " & _
                                                                        ",@login_count " & _
                                                                        ",@session_id_for_sp " & _
                                                                        ",@cancel_date_jst " & _
                                                                        ",@is_credit_error " & _
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
            Me.DbManager.SetCmdParameter("@user_type", SqlDbType.VarChar, ParameterDirection.Input, pDr.user_type)
            Me.DbManager.SetCmdParameter("@user_role", SqlDbType.Int, ParameterDirection.Input, pDr.user_role)
            Me.DbManager.SetCmdParameter("@uid", SqlDbType.VarChar, ParameterDirection.Input, pDr.uid)
            Me.DbManager.SetCmdParameter("@password", SqlDbType.VarChar, ParameterDirection.Input, pDr.password)
            Me.DbManager.SetCmdParameter("@salt", SqlDbType.VarChar, ParameterDirection.Input, pDr.salt)
            Me.DbManager.SetCmdParameter("@salutation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.salutation)
            Me.DbManager.SetCmdParameter("@given_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.given_name)
            Me.DbManager.SetCmdParameter("@firstname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kata)
            Me.DbManager.SetCmdParameter("@lastname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kata)
            Me.DbManager.SetCmdParameter("@firstname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kanji)
            Me.DbManager.SetCmdParameter("@lastname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kanji)
            Me.DbManager.SetCmdParameter("@gender", SqlDbType.VarChar, ParameterDirection.Input, pDr.gender)
            If Not pDr.IsNull("additional_info") Then
                Me.DbManager.SetCmdParameter("@additional_info", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_info)
            Else
                Me.DbManager.SetCmdParameter("@additional_info", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@email", SqlDbType.VarChar, ParameterDirection.Input, pDr.email)
            Me.DbManager.SetCmdParameter("@contact_num", SqlDbType.VarChar, ParameterDirection.Input, pDr.contact_num)
            If Not pDr.IsNull("last_login_date") Then
                Me.DbManager.SetCmdParameter("@last_login_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.last_login_date)
            Else
                Me.DbManager.SetCmdParameter("@last_login_date", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If

            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            If Not pDr.IsNull("subscription_id") Then
                Me.DbManager.SetCmdParameter("@subscription_id", SqlDbType.Int, ParameterDirection.Input, pDr.subscription_id)
            Else
                Me.DbManager.SetCmdParameter("@subscription_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("login_expire") Then
                Me.DbManager.SetCmdParameter("@login_expire", SqlDbType.DateTime, ParameterDirection.Input, pDr.login_expire)
            Else
                Me.DbManager.SetCmdParameter("@login_expire", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("mail_permission") Then
                Me.DbManager.SetCmdParameter("@mail_permission", SqlDbType.Bit, ParameterDirection.Input, pDr.mail_permission)
            Else
                Me.DbManager.SetCmdParameter("@mail_permission", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("session_id") Then
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, pDr.session_id)
            Else
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ban_flag") Then
                Me.DbManager.SetCmdParameter("@ban_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.ban_flag)
            Else
                Me.DbManager.SetCmdParameter("@ban_flag", SqlDbType.Bit, ParameterDirection.Input, False)
            End If
            If Not pDr.IsNull("ban_datetime") Then
                Me.DbManager.SetCmdParameter("@ban_datetime", SqlDbType.DateTime, ParameterDirection.Input, pDr.ban_datetime)
            Else
                Me.DbManager.SetCmdParameter("@ban_datetime", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("login_count") Then
                Me.DbManager.SetCmdParameter("@login_count", SqlDbType.Int, ParameterDirection.Input, pDr.login_count)
            Else
                Me.DbManager.SetCmdParameter("@login_count", SqlDbType.Int, ParameterDirection.Input, 0)
            End If
            If Not pDr.IsNull("session_id_for_sp") Then
                Me.DbManager.SetCmdParameter("@session_id_for_sp", SqlDbType.VarChar, ParameterDirection.Input, pDr.session_id_for_sp)
            Else
                Me.DbManager.SetCmdParameter("@session_id_for_sp", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cancel_date_jst") Then
                Me.DbManager.SetCmdParameter("@cancel_date_jst", SqlDbType.Date, ParameterDirection.Input, pDr.cancel_date_jst)
            Else
                Me.DbManager.SetCmdParameter("@cancel_date_jst", SqlDbType.Date, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("is_credit_error") Then
                Me.DbManager.SetCmdParameter("@is_credit_error", SqlDbType.Bit, ParameterDirection.Input, pDr.is_credit_error)
            Else
                Me.DbManager.SetCmdParameter("@is_credit_error", SqlDbType.Bit, ParameterDirection.Input, False)
            End If
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_UserRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "user_type = @user_type " & _
                                            ",user_role = @user_role " & _
                                            ",uid = @uid " & _
                                            ",password = @password " & _
                                            ",salt = @salt " & _
                                            ",salutation = @salutation " & _
                                            ",given_name = @given_name " & _
                                            ",firstname_kata = @firstname_kata " & _
                                            ",lastname_kata = @lastname_kata " & _
                                            ",firstname_kanji = @firstname_kanji " & _
                                            ",lastname_kanji = @lastname_kanji " & _
                                            ",gender = @gender " & _
                                            ",additional_info = @additional_info " & _
                                            ",email = @email " & _
                                            ",contact_num = @contact_num " & _
                                            ",last_login_date = @last_login_date " & _
                                            ",status = @status " & _
                                            ",created_date = @created_date " & _
                                            ",created_by = @created_by " & _
                                            ",modified_date = @modified_date " & _
                                            ",modified_by = @modified_by " & _
                                            ",defunct = @defunct " & _
                                            ",subscription_id = @subscription_id " & _
                                            ",login_expire = @login_expire " & _
                                            ",mail_permission = @mail_permission " & _
                                            ",session_id = @session_id " & _
                                            ",ban_flag = @ban_flag " & _
                                            ",ban_datetime = @ban_datetime " & _
                                            ",login_count = @login_count " & _
                                            ",session_id_for_sp = @session_id_for_sp " & _
                                            ",cancel_date_jst = @cancel_date_jst " & _
                                            ",is_credit_error = @is_credit_error " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@user_type", SqlDbType.VarChar, ParameterDirection.Input, pDr.user_type)
            Me.DbManager.SetCmdParameter("@user_role", SqlDbType.Int, ParameterDirection.Input, pDr.user_role)
            Me.DbManager.SetCmdParameter("@uid", SqlDbType.VarChar, ParameterDirection.Input, pDr.uid)
            Me.DbManager.SetCmdParameter("@password", SqlDbType.VarChar, ParameterDirection.Input, pDr.password)
            Me.DbManager.SetCmdParameter("@salt", SqlDbType.VarChar, ParameterDirection.Input, pDr.salt)
            Me.DbManager.SetCmdParameter("@salutation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.salutation)
            Me.DbManager.SetCmdParameter("@given_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.given_name)
            Me.DbManager.SetCmdParameter("@firstname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kata)
            Me.DbManager.SetCmdParameter("@lastname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kata)
            Me.DbManager.SetCmdParameter("@firstname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kanji)
            Me.DbManager.SetCmdParameter("@lastname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kanji)
            Me.DbManager.SetCmdParameter("@gender", SqlDbType.VarChar, ParameterDirection.Input, pDr.gender)
            If Not pDr.IsNull("additional_info") Then
                Me.DbManager.SetCmdParameter("@additional_info", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_info)
            Else
                Me.DbManager.SetCmdParameter("@additional_info", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@email", SqlDbType.VarChar, ParameterDirection.Input, pDr.email)
            Me.DbManager.SetCmdParameter("@contact_num", SqlDbType.VarChar, ParameterDirection.Input, pDr.contact_num)
            If Not pDr.IsNull("last_login_date") Then
                Me.DbManager.SetCmdParameter("@last_login_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.last_login_date)
            Else
                Me.DbManager.SetCmdParameter("@last_login_date", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If

            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            If Not pDr.IsNull("subscription_id") Then
                Me.DbManager.SetCmdParameter("@subscription_id", SqlDbType.Int, ParameterDirection.Input, pDr.subscription_id)
            Else
                Me.DbManager.SetCmdParameter("@subscription_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("login_expire") Then
                Me.DbManager.SetCmdParameter("@login_expire", SqlDbType.DateTime, ParameterDirection.Input, pDr.login_expire)
            Else
                Me.DbManager.SetCmdParameter("@login_expire", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("mail_permission") Then
                Me.DbManager.SetCmdParameter("@mail_permission", SqlDbType.Bit, ParameterDirection.Input, pDr.mail_permission)
            Else
                Me.DbManager.SetCmdParameter("@mail_permission", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("session_id") Then
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, pDr.session_id)
            Else
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ban_flag") Then
                Me.DbManager.SetCmdParameter("@ban_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.ban_flag)
            Else
                Me.DbManager.SetCmdParameter("@ban_flag", SqlDbType.Bit, ParameterDirection.Input, False)
            End If
            If Not pDr.IsNull("ban_datetime") Then
                Me.DbManager.SetCmdParameter("@ban_datetime", SqlDbType.DateTime, ParameterDirection.Input, pDr.ban_datetime)
            Else
                Me.DbManager.SetCmdParameter("@ban_datetime", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("login_count") Then
                Me.DbManager.SetCmdParameter("@login_count", SqlDbType.Int, ParameterDirection.Input, pDr.login_count)
            Else
                Me.DbManager.SetCmdParameter("@login_count", SqlDbType.Int, ParameterDirection.Input, 0)
            End If
            If Not pDr.IsNull("session_id_for_sp") Then
                Me.DbManager.SetCmdParameter("@session_id_for_sp", SqlDbType.VarChar, ParameterDirection.Input, pDr.session_id_for_sp)
            Else
                Me.DbManager.SetCmdParameter("@session_id_for_sp", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cancel_date_jst") Then
                Me.DbManager.SetCmdParameter("@cancel_date_jst", SqlDbType.Date, ParameterDirection.Input, pDr.cancel_date_jst)
            Else
                Me.DbManager.SetCmdParameter("@cancel_date_jst", SqlDbType.Date, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("is_credit_error") Then
                Me.DbManager.SetCmdParameter("@is_credit_error", SqlDbType.Bit, ParameterDirection.Input, pDr.is_credit_error)
            Else
                Me.DbManager.SetCmdParameter("@is_credit_error", SqlDbType.Bit, ParameterDirection.Input, False)
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

#Region "BANフラグの更新"
    Public Function UpdateForBanFlag(ByRef pDr As DS_USER.T_JP_UserRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "ban_flag = @ban_flag " & _
                                            ",ban_datetime = @ban_datetime " & _
                                     "WHERE id = @id"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("ban_flag", SqlDbType.Int, ParameterDirection.Input, pDr.ban_flag)
            If pDr.ban_flag Then
                Me.DbManager.SetCmdParameter("ban_datetime", SqlDbType.DateTime, ParameterDirection.Input, GlobalVariables.JpnDate)
            Else
                Me.DbManager.SetCmdParameter("ban_datetime", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            Return Me.DbManager.Execute
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#End Region


End Class
