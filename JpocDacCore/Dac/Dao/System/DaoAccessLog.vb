Public Class DaoAccessLog
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_AccessLog"
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

#Region "アクセス日による取得(UTC)"
    Public Function GetCountByAccessTimeUtc(ByVal pAccessTimeUtc As Date, _
                                            Optional ByVal pInstitutionId As Integer = -1) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE CONVERT(VARCHAR,access_time_utc,111) = @1"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetCmdParameter("@1", SqlDbType.Date, ParameterDirection.Input, pAccessTimeUtc)
            If pInstitutionId <> -1 Then
                strSql &= " AND institution_id = @2"
                Me.DbManager.SetCmdParameter("@2", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
            End If
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

#Region "アクセス日による取得(JST)"
    Public Function GetCountByAccessTimeJst(ByVal pAccessTimeJst As Date, _
                                            Optional ByVal pInstitutionId As Integer = -1) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE CONVERT(VARCHAR,access_time_jst,111) = @1"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetCmdParameter("@1", SqlDbType.Date, ParameterDirection.Input, pAccessTimeJst)
            If pInstitutionId <> -1 Then
                strSql &= " AND institution_id = @2"
                Me.DbManager.SetCmdParameter("@2", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
            End If
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

#Region "ユーザーID（及び施設ID）と指定期間内による取得"
    Public Function GetCountByUserIdAndCheckOfPeriod(ByVal pUserId As Integer, _
                                                     ByVal pInstId As Integer, _
                                                     ByVal pStartDateTime As DateTime, _
                                                     Optional ByVal pSessionId As String = "") As Integer

        'アクセスが行われた件数を分単位で抽出したい
        'TODO: 処理が重い場合はDB上にViewを作成する等考慮する
        Const SQL_QUERY As String = " SELECT COUNT(*) as count FROM" & _
                                    " (SELECT " & _
                                    " left(convert(varchar(max), log.access_time_jst,20),16) AS access_minitue " & _
                                    " FROM " & TABLE_NAME & " log " & _
                                    " WHERE log.institution_id = @instId" & _
                                    " AND log.user_id = @userId" & _
                                    " AND log.access_time_jst > @startTime" & _
                                    " AND log.content_type = 'ASPX'" & _
                                    " AND NOT EXISTS (" & _
                                    "   SELECT * FROM " & DaoContentsMaster.TABLE_NAME & " contents " & _
                                    "   WHERE contents.content_type = log.content_type " & _
                                    "   AND contents.content_name = log.content_name " & _
                                    "   AND contents.display = 0" & _
                                    "   AND contents.status = 'A'" & _
                                    "   AND contents.defunct = 0)" & _
                                    " AND content_name <> 'search.aspx'" & _
                                    " GROUP by left(convert(varchar(max), log.access_time_jst,20),16)) AS t"

        Try
            Dim strSql As String = SQL_QUERY
            If Not String.IsNullOrEmpty(pSessionId) Then
                strSql &= " AND Attribute10 =  '" & pSessionId & "'"
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("userId", SqlDbType.Int, ParameterDirection.Input, pUserId)
            Me.DbManager.SetCmdParameter("instId", SqlDbType.Int, ParameterDirection.Input, pInstId)
            Me.DbManager.SetCmdParameter("startTime", SqlDbType.DateTime, ParameterDirection.Input, pStartDateTime)
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

#Region "アクセス日による取得(UTC)"
    Public Function GetByAccessTimeUtc(ByRef pDt As DataTable, _
                                       ByVal pAccessTimeUtc As Date, _
                                       Optional ByVal pInstitutionId As Integer = -1) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE CONVERT(VARCHAR,access_time_utc,111) = @1"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetCmdParameter("@1", SqlDbType.Date, ParameterDirection.Input, pAccessTimeUtc)
            If pInstitutionId <> -1 Then
                strSql &= " AND institution_id = @2"
                Me.DbManager.SetCmdParameter("@2", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
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
#End Region

#Region "アクセス日による取得(JST)"
    Public Function GetByAccessTimeJst(ByRef pDt As DataTable, _
                                       ByVal pAccessTimeJst As Date, _
                                       Optional ByVal pInstitutionId As Integer = -1) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE CONVERT(VARCHAR,access_time_jst,111) = @1"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetCmdParameter("@1", SqlDbType.Date, ParameterDirection.Input, pAccessTimeJst)
            If pInstitutionId <> -1 Then
                strSql &= " AND institution_id = @2"
                Me.DbManager.SetCmdParameter("@2", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
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
#End Region

#Region "ユーザーID（及び施設ID）と指定期間内による取得"
    Public Function GetByUserIdAndCheckOfPeriod(ByRef pDt As DataTable, _
                                                ByVal pUserId As Integer, _
                                                ByVal pInstId As Integer, _
                                                ByVal pStartDateTime As DateTime, _
                                                Optional ByVal pSessionId As String = "") As Integer

        Const SQL_QUERY As String = "SELECT " & _
                                    " COUNT(*) AS count " & _
                                    " FROM " & TABLE_NAME & " log " & _
                                    " WHERE log.institution_id = @instId" & _
                                    " AND log.user_id = @userId" & _
                                    " AND log.access_time_jst > @startTime" & _
                                    " AND log.content_type = 'ASPX'" & _
                                    " AND NOT EXISTS (" & _
                                    "   SELECT * FROM " & DaoContentsMaster.TABLE_NAME & " contents " & _
                                    "   WHERE contents.content_type = log.content_type " & _
                                    "   AND contents.content_name = log.content_name " & _
                                    "   AND contents.display = 0" & _
                                    "   AND contents.status = 'A'" & _
                                    "   AND contents.defunct = 0)" & _
                                    " AND content_name <> 'search.aspx'"

        Try
            Dim strSql As String = SQL_QUERY
            If Not String.IsNullOrEmpty(pSessionId) Then
                strSql &= " AND Attribute10 =  '" & pSessionId & "'"
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("userId", SqlDbType.Int, ParameterDirection.Input, pUserId)
            Me.DbManager.SetCmdParameter("instId", SqlDbType.Int, ParameterDirection.Input, pInstId)
            Me.DbManager.SetCmdParameter("startTime", SqlDbType.DateTime, ParameterDirection.Input, pStartDateTime)
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

#Region "アクセス日による削除(UTC)"
    Public Function DeleteByAccessTimeUtc(ByVal pAccessTimeUtc As Date, _
                                          Optional ByVal pInstitutionId As Integer = -1) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE CONVERT(VARCHAR,access_time_utc,111) = @1"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetCmdParameter("@1", SqlDbType.Date, ParameterDirection.Input, pAccessTimeUtc)
            If pInstitutionId <> -1 Then
                strSql &= " AND institution_id = @2"
                Me.DbManager.SetCmdParameter("@2", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "アクセス日による削除(UTC)"
    Public Function DeleteByAccessTimeJst(ByVal pAccessTimeJst As Date, _
                                          Optional ByVal pInstitutionId As Integer = -1) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE CONVERT(VARCHAR,access_time_jst,111) = @1"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetCmdParameter("@1", SqlDbType.Date, ParameterDirection.Input, pAccessTimeJst)
            If pInstitutionId <> -1 Then
                strSql &= " AND institution_id = @2"
                Me.DbManager.SetCmdParameter("@2", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#End Region

#Region "追加"
    Public Function Insert(ByRef pDr As DS_SYSTEM.T_JP_AccessLogRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "ip " & _
                                                                        ",institution_id " & _
                                                                        ",user_id " & _
                                                                        ",access_time_utc " & _
                                                                        ",access_time_jst " & _
                                                                        ",content_type " & _
                                                                        ",content_name " & _
                                                                        ",Attribute01 " & _
                                                                        ",Attribute02 " & _
                                                                        ",Attribute03 " & _
                                                                        ",Attribute04 " & _
                                                                        ",Attribute05 " & _
                                                                        ",Attribute06 " & _
                                                                        ",Attribute07 " & _
                                                                        ",Attribute08 " & _
                                                                        ",Attribute09 " & _
                                                                        ",Attribute10 " & _
                                                                        ",Attribute11 " & _
                                                                        ",Attribute12 " & _
                                                                        ",Attribute13 " & _
                                                                        ",Attribute14 " & _
                                                                        ",Attribute15 " & _
                                                                        ",Attribute16 " & _
                                                                        ",Attribute17 " & _
                                                                        ",Attribute18 " & _
                                                                        ",Attribute19 " & _
                                                                        ",Attribute20 " & _
                                                                    ") VALUES (" & _
                                                                         "@ip " & _
                                                                        ",@institution_id " & _
                                                                        ",@user_id " & _
                                                                        ",@access_time_utc " & _
                                                                        ",@access_time_jst " & _
                                                                        ",@content_type " & _
                                                                        ",@content_name " & _
                                                                        ",@Attribute01 " & _
                                                                        ",@Attribute02 " & _
                                                                        ",@Attribute03 " & _
                                                                        ",@Attribute04 " & _
                                                                        ",@Attribute05 " & _
                                                                        ",@Attribute06 " & _
                                                                        ",@Attribute07 " & _
                                                                        ",@Attribute08 " & _
                                                                        ",@Attribute09 " & _
                                                                        ",@Attribute10 " & _
                                                                        ",@Attribute11 " & _
                                                                        ",@Attribute12 " & _
                                                                        ",@Attribute13 " & _
                                                                        ",@Attribute14 " & _
                                                                        ",@Attribute15 " & _
                                                                        ",@Attribute16 " & _
                                                                        ",@Attribute17 " & _
                                                                        ",@Attribute18 " & _
                                                                        ",@Attribute19 " & _
                                                                        ",@Attribute20 " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            If Not pDr.IsNull("ip") Then
                Me.DbManager.SetCmdParameter("@ip", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ip)
            Else
                Me.DbManager.SetCmdParameter("@ip", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("institution_id") Then
                Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.Int, ParameterDirection.Input, pDr.institution_id)
            Else
                Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("user_id") Then
                Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pDr.user_id)
            Else
                Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("access_time_utc") Then
                Me.DbManager.SetCmdParameter("@access_time_utc", SqlDbType.DateTime, ParameterDirection.Input, pDr.access_time_utc)
            Else
                Me.DbManager.SetCmdParameter("@access_time_utc", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("access_time_jst") Then
                Me.DbManager.SetCmdParameter("@access_time_jst", SqlDbType.DateTime, ParameterDirection.Input, pDr.access_time_jst)
            Else
                Me.DbManager.SetCmdParameter("@access_time_jst", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("content_type") Then
                Me.DbManager.SetCmdParameter("@content_type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_type)
            Else
                Me.DbManager.SetCmdParameter("@content_type", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("content_name") Then
                Me.DbManager.SetCmdParameter("@content_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.content_name)
            Else
                Me.DbManager.SetCmdParameter("@content_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute01") Then
                Me.DbManager.SetCmdParameter("@Attribute01", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute01)
            Else
                Me.DbManager.SetCmdParameter("@Attribute01", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute02") Then
                Me.DbManager.SetCmdParameter("@Attribute02", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute02)
            Else
                Me.DbManager.SetCmdParameter("@Attribute02", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute03") Then
                Me.DbManager.SetCmdParameter("@Attribute03", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute03)
            Else
                Me.DbManager.SetCmdParameter("@Attribute03", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute04") Then
                Me.DbManager.SetCmdParameter("@Attribute04", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute04)
            Else
                Me.DbManager.SetCmdParameter("@Attribute04", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute05") Then
                Me.DbManager.SetCmdParameter("@Attribute05", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute05)
            Else
                Me.DbManager.SetCmdParameter("@Attribute05", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute06") Then
                Me.DbManager.SetCmdParameter("@Attribute06", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute06)
            Else
                Me.DbManager.SetCmdParameter("@Attribute06", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute07") Then
                Me.DbManager.SetCmdParameter("@Attribute07", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute07)
            Else
                Me.DbManager.SetCmdParameter("@Attribute07", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute08") Then
                Me.DbManager.SetCmdParameter("@Attribute08", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute08)
            Else
                Me.DbManager.SetCmdParameter("@Attribute08", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute09") Then
                Me.DbManager.SetCmdParameter("@Attribute09", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute09)
            Else
                Me.DbManager.SetCmdParameter("@Attribute09", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute10") Then
                Me.DbManager.SetCmdParameter("@Attribute10", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute10)
            Else
                Me.DbManager.SetCmdParameter("@Attribute10", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute11") Then
                Me.DbManager.SetCmdParameter("@Attribute11", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute11)
            Else
                Me.DbManager.SetCmdParameter("@Attribute11", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute12") Then
                Me.DbManager.SetCmdParameter("@Attribute12", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute12)
            Else
                Me.DbManager.SetCmdParameter("@Attribute12", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute13") Then
                Me.DbManager.SetCmdParameter("@Attribute13", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute13)
            Else
                Me.DbManager.SetCmdParameter("@Attribute13", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute14") Then
                Me.DbManager.SetCmdParameter("@Attribute14", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute14)
            Else
                Me.DbManager.SetCmdParameter("@Attribute14", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute15") Then
                Me.DbManager.SetCmdParameter("@Attribute15", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute15)
            Else
                Me.DbManager.SetCmdParameter("@Attribute15", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute16") Then
                Me.DbManager.SetCmdParameter("@Attribute16", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute16)
            Else
                Me.DbManager.SetCmdParameter("@Attribute16", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute17") Then
                Me.DbManager.SetCmdParameter("@Attribute17", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute17)
            Else
                Me.DbManager.SetCmdParameter("@Attribute17", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute18") Then
                Me.DbManager.SetCmdParameter("@Attribute18", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute18)
            Else
                Me.DbManager.SetCmdParameter("@Attribute18", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute19") Then
                Me.DbManager.SetCmdParameter("@Attribute19", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute19)
            Else
                Me.DbManager.SetCmdParameter("@Attribute19", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("Attribute20") Then
                Me.DbManager.SetCmdParameter("@Attribute20", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Attribute20)
            Else
                Me.DbManager.SetCmdParameter("@Attribute20", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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

End Class
