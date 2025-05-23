Public Class DaoLabTest
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_LabTest"
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
    Public Function GetCountByPK(ByVal pId As Integer, _
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

#Region "SRL_IDによる取得"
    Public Function GetCountBySrlId(ByVal pSrlId As String, _
                                    Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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
                            ByVal pID As Integer, _
                            Optional ByVal pOnlyActive As Boolean = False) As Integer
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

#Region "SRL_IDによる取得"
    Public Function GetBySrlId(ByRef pDt As DataTable, _
                               ByVal pSrlId As String, _
                               Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "T_JP_TreeSubcategory.idによる取得"
    Public Function GetBySubcategoryId(ByRef pDt As DataTable, _
                                       ByVal pSubcategoryId As Integer, _
                                       Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = " SELECT lab.* " & _
                                    " FROM " & TABLE_NAME & " lab " & _
                                    " INNER JOIN " & DaoTreeSubcategoryMap.TABLE_NAME & " map " & _
                                    " ON map.unique_code = lab.srl_id " & _
                                    " WHERE map.subcategoryId = @subcategoryId "

        Const ORDER_QUERY As String = " ORDER BY map.sequence"

        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            strSql &= ORDER_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@subcategoryId", SqlDbType.Int, ParameterDirection.Input, pSubcategoryId)
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
    Public Function Insert(ByRef pDr As DS_LAB_TEST.T_JP_LabTestRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "guid " & _
                                                                        ",srl_id " & _
                                                                        ",name " & _
                                                                        ",category " & _
                                                                        ",test_method " & _
                                                                        ",when_to_use " & _
                                                                        ",explanation_notes " & _
                                                                        ",order_notes " & _
                                                                        ",clinical_meaning " & _
                                                                        ",abnormality " & _
                                                                        ",reference1 " & _
                                                                        ",reference2 " & _
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
                                                                        ",calculation_standard " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@guid " & _
                                                                        ",@srl_id " & _
                                                                        ",@name " & _
                                                                        ",@category " & _
                                                                        ",@test_method " & _
                                                                        ",@when_to_use " & _
                                                                        ",@explanation_notes " & _
                                                                        ",@order_notes " & _
                                                                        ",@clinical_meaning " & _
                                                                        ",@abnormality " & _
                                                                        ",@reference1 " & _
                                                                        ",@reference2 " & _
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
                                                                        ",@calculation_standard " & _
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
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            Me.DbManager.SetCmdParameter("@category", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category)
            Me.DbManager.SetCmdParameter("@test_method", SqlDbType.NText, ParameterDirection.Input, pDr.test_method)
            Me.DbManager.SetCmdParameter("@when_to_use", SqlDbType.NText, ParameterDirection.Input, pDr.when_to_use)
            Me.DbManager.SetCmdParameter("@explanation_notes", SqlDbType.NText, ParameterDirection.Input, pDr.explanation_notes)
            Me.DbManager.SetCmdParameter("@order_notes", SqlDbType.NText, ParameterDirection.Input, pDr.order_notes)
            Me.DbManager.SetCmdParameter("@clinical_meaning", SqlDbType.NText, ParameterDirection.Input, pDr.clinical_meaning)
            Me.DbManager.SetCmdParameter("@abnormality", SqlDbType.NText, ParameterDirection.Input, pDr.abnormality)
            Me.DbManager.SetCmdParameter("@reference1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.reference1)
            Me.DbManager.SetCmdParameter("@reference2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.reference2)
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
            If Not pDr.IsNull("calculation_standard") Then
                Me.DbManager.SetCmdParameter("@calculation_standard", SqlDbType.NVarChar, ParameterDirection.Input, pDr.calculation_standard)
            Else
                Me.DbManager.SetCmdParameter("@calculation_standard", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_LAB_TEST.T_JP_LabTestRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "guid = @guid " & _
                                            ",srl_id = @srl_id " & _
                                            ",name = @name " & _
                                            ",category = @category " & _
                                            ",test_method = @test_method " & _
                                            ",when_to_use = @when_to_use " & _
                                            ",explanation_notes = @explanation_notes " & _
                                            ",order_notes = @order_notes " & _
                                            ",clinical_meaning = @clinical_meaning " & _
                                            ",abnormality = @abnormality " & _
                                            ",reference1 = @reference1 " & _
                                            ",reference2 = @reference2 " & _
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
                                            ",calculation_standard = @calculation_standard " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pDr.guid)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            Me.DbManager.SetCmdParameter("@category", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category)
            Me.DbManager.SetCmdParameter("@test_method", SqlDbType.NText, ParameterDirection.Input, pDr.test_method)
            Me.DbManager.SetCmdParameter("@when_to_use", SqlDbType.NText, ParameterDirection.Input, pDr.when_to_use)
            Me.DbManager.SetCmdParameter("@explanation_notes", SqlDbType.NText, ParameterDirection.Input, pDr.explanation_notes)
            Me.DbManager.SetCmdParameter("@order_notes", SqlDbType.NText, ParameterDirection.Input, pDr.order_notes)
            Me.DbManager.SetCmdParameter("@clinical_meaning", SqlDbType.NText, ParameterDirection.Input, pDr.clinical_meaning)
            Me.DbManager.SetCmdParameter("@abnormality", SqlDbType.NText, ParameterDirection.Input, pDr.abnormality)
            Me.DbManager.SetCmdParameter("@reference1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.reference1)
            Me.DbManager.SetCmdParameter("@reference2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.reference2)
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
            If Not pDr.IsNull("calculation_standard") Then
                Me.DbManager.SetCmdParameter("@calculation_standard", SqlDbType.NVarChar, ParameterDirection.Input, pDr.calculation_standard)
            Else
                Me.DbManager.SetCmdParameter("@calculation_standard", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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

#Region "SrlID整形"
    Public Function FormatSrlId(ByVal pSrlId As String) As String
        Return Int32.Parse(pSrlId).ToString()
    End Function
#End Region

End Class
