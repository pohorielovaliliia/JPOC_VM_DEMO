Public Class DaoInsUser
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_InsUser"
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
                                 Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " &
                                      "FROM " & TABLE_NAME & " " &
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

#Region "InstitutionIDによる取得"
    Public Function GetCountByInstitutionID(ByVal pInstitutionID As Integer,
                                       Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                    "FROM " & TABLE_NAME & " " &
                                    "WHERE InstitutionID = @InstitutionID "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
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

            If pOnlyActive Then
                strSql &= " WHERE " & MyBase.ActiveCondition
            ElseIf pOnlyActive = False Then
                strSql &= " WHERE " & MyBase.InActiveCondition
            End If

            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function

    Public Function GetAllStatus(ByRef pDt As System.Data.DataTable) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " "
        Try
            Dim strSql As String = SQL_QUERY
            strSql &= " WHERE " & MyBase.AllCondition

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
                            ByVal pId As Integer,
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
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

#Region "InstitutionIDによる取得"
    Public Function GetByInstitutionID(ByRef pDt As DataTable,
                                       ByVal pInstitutionID As Integer,
                                       Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                    "FROM " & TABLE_NAME & " " &
                                    "WHERE InstitutionID = @InstitutionID "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then
                strSql &= " AND " & MyBase.ActiveCondition
            ElseIf pOnlyActive = False Then
                strSql &= " AND " & MyBase.InActiveCondition
            End If

            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function

    Public Function GetByInstitutionIDAllStatus(ByRef pDt As DataTable,
                                       ByVal pInstitutionID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                    "FROM " & TABLE_NAME & " " &
                                    "WHERE InstitutionID = @InstitutionID "
        Try
            Dim strSql As String = SQL_QUERY
            strSql &= " AND " & MyBase.AllCondition

            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function

#End Region

#Region "GroupIdによる取得"
    Public Function GetBypGroupID(ByRef pDt As DataTable,
                                  ByVal pGroupID As Integer,
                                  Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                    "FROM " & TABLE_NAME & " " &
                                    "WHERE GroupID = @GroupID "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("GroupID", SqlDbType.Int, ParameterDirection.Input, pGroupID)
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
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " &
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_InsUserRow,
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" &
                                                                         "InstitutionID " &
                                                                        ",user_role " &
                                                                        ",uid " &
                                                                        ",password " &
                                                                        ",salt " &
                                                                        ",firstname_kata " &
                                                                        ",lastname_kata " &
                                                                        ",firstname_kanji " &
                                                                        ",lastname_kanji " &
                                                                        ",email " &
                                                                        ",job_title " &
                                                                        ",GroupID " &
                                                                        ",last_login_date " &
                                                                        ",status " &
                                                                        ",created_date " &
                                                                        ",created_by " &
                                                                        ",modified_date " &
                                                                        ",modified_by " &
                                                                        ",defunct " &
                                                                        ",subscription_id " &
                                                                        ",login_expire " &
                                                                        ",mail_permission " &
                                                                        ",promotion_code " &
                                                                        ",user_type " &
                                                                        ",session_id " &
                                                                        "{0}" &
                                                                    ") VALUES (" &
                                                                         "@InstitutionID " &
                                                                        ",@user_role " &
                                                                        ",@uid " &
                                                                        ",@password " &
                                                                        ",@salt " &
                                                                        ",@firstname_kata " &
                                                                        ",@lastname_kata " &
                                                                        ",@firstname_kanji " &
                                                                        ",@lastname_kanji " &
                                                                        ",@email " &
                                                                        ",@job_title " &
                                                                        ",@GroupID " &
                                                                        ",@last_login_date " &
                                                                        ",@status " &
                                                                        ",@created_date " &
                                                                        ",@created_by " &
                                                                        ",@modified_date " &
                                                                        ",@modified_by " &
                                                                        ",@defunct " &
                                                                        ",@subscription_id " &
                                                                        ",@login_expire " &
                                                                        ",@mail_permission " &
                                                                        ",@promotion_code " &
                                                                        ",@user_type " &
                                                                        ",@session_id " &
                                                                        "{1}" &
                                                                    ");"

        Try
            Dim strSQL As String = String.Empty
            If pKeepIdValue Then
                strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) &
                         String.Format(SQL_QUERY, ",id ", ",@id ") &
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@user_role", SqlDbType.Int, ParameterDirection.Input, pDr.user_role)
            Me.DbManager.SetCmdParameter("@uid", SqlDbType.VarChar, ParameterDirection.Input, pDr.uid)
            Me.DbManager.SetCmdParameter("@password", SqlDbType.VarChar, ParameterDirection.Input, pDr.password)
            Me.DbManager.SetCmdParameter("@salt", SqlDbType.VarChar, ParameterDirection.Input, pDr.salt)
            Me.DbManager.SetCmdParameter("@firstname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kata)
            Me.DbManager.SetCmdParameter("@lastname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kata)
            Me.DbManager.SetCmdParameter("@firstname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kanji)
            Me.DbManager.SetCmdParameter("@lastname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kanji)
            Me.DbManager.SetCmdParameter("@email", SqlDbType.VarChar, ParameterDirection.Input, pDr.email)
            Me.DbManager.SetCmdParameter("@job_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.job_title)
            If Not pDr.IsNull("GroupID") Then
                Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, pDr.GroupID)
            Else
                Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
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
            If Not pDr.IsNull("promotion_code") Then
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, pDr.promotion_code)
            Else
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("user_type") Then
                Me.DbManager.SetCmdParameter("@user_type", SqlDbType.VarChar, ParameterDirection.Input, pDr.user_type)
            Else
                Me.DbManager.SetCmdParameter("@user_type", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("session_id") Then
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, pDr.session_id)
            Else
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_InsUserRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " &
                                       "SET " &
                                             "InstitutionID = @InstitutionID " &
                                            ",user_role = @user_role " &
                                            ",uid = @uid " &
                                            ",password = @password " &
                                            ",salt = @salt " &
                                            ",firstname_kata = @firstname_kata " &
                                            ",lastname_kata = @lastname_kata " &
                                            ",firstname_kanji = @firstname_kanji " &
                                            ",lastname_kanji = @lastname_kanji " &
                                            ",email = @email " &
                                            ",job_title = @job_title " &
                                            ",GroupID = @GroupID " &
                                            ",last_login_date = @last_login_date " &
                                            ",status = @status " &
                                            ",created_date = @created_date " &
                                            ",created_by = @created_by " &
                                            ",modified_date = @modified_date " &
                                            ",modified_by = @modified_by " &
                                            ",defunct = @defunct " &
                                            ",subscription_id = @subscription_id " &
                                            ",login_expire = @login_expire " &
                                            ",mail_permission = @mail_permission " &
                                            ",promotion_code = @promotion_code " &
                                            ",user_type = @user_type " &
                                            ",session_id = @session_id " &
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@user_role", SqlDbType.Int, ParameterDirection.Input, pDr.user_role)
            Me.DbManager.SetCmdParameter("@uid", SqlDbType.VarChar, ParameterDirection.Input, pDr.uid)
            Me.DbManager.SetCmdParameter("@password", SqlDbType.VarChar, ParameterDirection.Input, pDr.password)
            Me.DbManager.SetCmdParameter("@salt", SqlDbType.VarChar, ParameterDirection.Input, pDr.salt)
            Me.DbManager.SetCmdParameter("@firstname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kata)
            Me.DbManager.SetCmdParameter("@lastname_kata", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kata)
            Me.DbManager.SetCmdParameter("@firstname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.firstname_kanji)
            Me.DbManager.SetCmdParameter("@lastname_kanji", SqlDbType.NVarChar, ParameterDirection.Input, pDr.lastname_kanji)
            Me.DbManager.SetCmdParameter("@email", SqlDbType.VarChar, ParameterDirection.Input, pDr.email)
            Me.DbManager.SetCmdParameter("@job_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.job_title)
            If Not pDr.IsNull("GroupID") Then
                Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, pDr.GroupID)
            Else
                Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
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
            If Not pDr.IsNull("promotion_code") Then
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, pDr.promotion_code)
            Else
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("user_type") Then
                Me.DbManager.SetCmdParameter("@user_type", SqlDbType.VarChar, ParameterDirection.Input, pDr.user_type)
            Else
                Me.DbManager.SetCmdParameter("@user_type", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("session_id") Then
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, pDr.session_id)
            Else
                Me.DbManager.SetCmdParameter("@session_id", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function UpdateForBanFlag(ByRef pDr As DS_USER.T_JP_InsUserRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " &
                                       "SET " &
                                             "ban_flag = @ban_flag " &
                                            ",ban_datetime = @ban_datetime " &
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

#Region "グループIDの更新"
    Public Function UpdateForGroupId(ByRef pDr As DS_USER.T_JP_InsUserRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " &
                                       "SET " &
                                             "GroupID = @GroupID " &
                                     "WHERE id = @id"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("GroupID", SqlDbType.Int, ParameterDirection.Input, pDr.GroupID)

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
