Public Class DaoSituationOrderSetSample
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_SituationOrderSetSample"
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
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
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

#Region "DiseaseIDによる取得"
    Public Function GetByDiseaseID(ByVal pDiseaseID As Integer, _
                                   Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoSituation.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.situation_id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                       "{1}" & _
                                                 ")"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty)
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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

#Region "DiseaseIDによる取得"
    Public Function GetByDiseaseID(ByRef pDt As DataTable, _
                                   ByVal pDiseaseID As Integer, _
                                   Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoSituation.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.situation_id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                       "{1}" & _
                                                 ")"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty)
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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

#Region "DiseaseID及びcodeによる取得"
    Public Function GetByDiseaseIdAndCode(ByRef pDt As DataTable, _
                                   ByVal pDiseaseID As Integer, _
                                   ByVal pCode As String, _
                                   Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoSituation.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.situation_id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                       "{1}" & _
                                                 ") " & _
                                    "AND code = @code"

        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty)
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)
            Me.DbManager.SetCmdParameter("@code", SqlDbType.VarChar, ParameterDirection.Input, pCode)
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
                            ByVal pID As Integer, _
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
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
    Public Function Delete(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "guid " & _
                                                                        ",situation_id " & _
                                                                        ",type " & _
                                                                        ",title " & _
                                                                        ",comments " & _
                                                                        ",sequence " & _
                                                                        ",status " & _
                                                                        ",is_current_version " & _
                                                                        ",version " & _
                                                                        ",version_string " & _
                                                                        ",created_by " & _
                                                                        ",modified_by " & _
                                                                        ",checkout_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_date " & _
                                                                        ",defunct " & _
                                                                        ",actual_order " & _
                                                                        ",code " & _
                                                                        ",prescription_flag " & _
                                                                        ",lab_flag " & _
                                                                        "{0}" & _
                                                                   ") VALUES (" & _
                                                                         "@guid " & _
                                                                        ",@situation_id " & _
                                                                        ",@type " & _
                                                                        ",@title " & _
                                                                        ",@comments " & _
                                                                        ",@sequence " & _
                                                                        ",@status " & _
                                                                        ",@is_current_version " & _
                                                                        ",@version " & _
                                                                        ",@version_string " & _
                                                                        ",@created_by " & _
                                                                        ",@modified_by " & _
                                                                        ",@checkout_by " & _
                                                                        ",@created_date " & _
                                                                        ",@modified_date " & _
                                                                        ",@defunct " & _
                                                                        ",@actual_order " & _
                                                                        ",@code " & _
                                                                        ",@prescription_flag " & _
                                                                        ",@lab_flag " & _
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
            Me.DbManager.SetCmdParameter("@guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pDr.guid)
            Me.DbManager.SetCmdParameter("@situation_id", SqlDbType.Int, ParameterDirection.Input, pDr.situation_id)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.VarChar, ParameterDirection.Input, pDr.type)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@comments", SqlDbType.NText, ParameterDirection.Input, pDr.comments)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@is_current_version", SqlDbType.Bit, ParameterDirection.Input, pDr.is_current_version)
            Me.DbManager.SetCmdParameter("@version", SqlDbType.Int, ParameterDirection.Input, pDr.version)
            Me.DbManager.SetCmdParameter("@version_string", SqlDbType.NVarChar, ParameterDirection.Input, pDr.version_string)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            If Not pDr.IsNull("actual_order") Then
                Me.DbManager.SetCmdParameter("@actual_order", SqlDbType.NVarChar, ParameterDirection.Input, pDr.actual_order)
            Else
                Me.DbManager.SetCmdParameter("@actual_order", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.VarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("prescription_flag") Then
                Me.DbManager.SetCmdParameter("@prescription_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.prescription_flag)
            Else
                Me.DbManager.SetCmdParameter("@prescription_flag", SqlDbType.Bit, ParameterDirection.Input, False)
            End If
            If Not pDr.IsNull("lab_flag") Then
                Me.DbManager.SetCmdParameter("@lab_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.lab_flag)
            Else
                Me.DbManager.SetCmdParameter("@lab_flag", SqlDbType.Bit, ParameterDirection.Input, False)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim situationOrderSetSampleId As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = situationOrderSetSampleId
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "guid = @guid " & _
                                            ",situation_id = @situation_id " & _
                                            ",type = @type " & _
                                            ",title = @title " & _
                                            ",comments = @comments " & _
                                            ",sequence = @sequence " & _
                                            ",status = @status " & _
                                            ",is_current_version = @is_current_version " & _
                                            ",version = @version " & _
                                            ",version_string = @version_string " & _
                                            ",created_by = @created_by " & _
                                            ",modified_by = @modified_by " & _
                                            ",checkout_by = @checkout_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_date = @modified_date " & _
                                            ",defunct = @defunct " & _
                                            ",actual_order = @actual_order " & _
                                            ",code = @code " & _
                                            ",prescription_flag = @prescription_flag " & _
                                            ",lab_flag = @lab_flag " & _
                                    "WHERE id = @id "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pDr.guid)
            Me.DbManager.SetCmdParameter("@situation_id", SqlDbType.Int, ParameterDirection.Input, pDr.situation_id)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.VarChar, ParameterDirection.Input, pDr.type)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@comments", SqlDbType.NText, ParameterDirection.Input, pDr.comments)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@is_current_version", SqlDbType.Bit, ParameterDirection.Input, pDr.is_current_version)
            Me.DbManager.SetCmdParameter("@version", SqlDbType.Int, ParameterDirection.Input, pDr.version)
            Me.DbManager.SetCmdParameter("@version_string", SqlDbType.NVarChar, ParameterDirection.Input, pDr.version_string)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            If Not pDr.IsNull("actual_order") Then
                Me.DbManager.SetCmdParameter("@actual_order", SqlDbType.NVarChar, ParameterDirection.Input, pDr.actual_order)
            Else
                Me.DbManager.SetCmdParameter("@actual_order", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.VarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("prescription_flag") Then
                Me.DbManager.SetCmdParameter("@prescription_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.prescription_flag)
            Else
                Me.DbManager.SetCmdParameter("@prescription_flag", SqlDbType.Bit, ParameterDirection.Input, False)
            End If
            If Not pDr.IsNull("lab_flag") Then
                Me.DbManager.SetCmdParameter("@lab_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.lab_flag)
            Else
                Me.DbManager.SetCmdParameter("@lab_flag", SqlDbType.Bit, ParameterDirection.Input, False)
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
