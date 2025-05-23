Public Class DaoDrugData
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugData"
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

#Region "Jpc(Code)による取得"
    Public Function GetCountByJpc(ByVal pJpc As String, _
                                  ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE code = @code"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatCode(pJpc))
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "文字列による取得"
    ''' <summary>文字列による曖昧検索</summary>
    ''' <param name="pText">検索文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCountByText(ByVal pText As String, ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE (general_name LIKE @text " & _
                                        "OR therapeutic_class LIKE @text " & _
                                        "OR trademark_names LIKE @text " & _
                                        "OR generic_name LIKE @text " & _
                                        "OR generic_name_en LIKE @text " & _
                                        "OR preparation_name LIKE @text " & _
                                        "OR company_name LIKE @text " & _
                                        "OR code LIKE @text)"

        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pText)
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
    Public Overrides Function GetAll(ByRef pDt As DataTable, pOnlyActive As Boolean) As Integer
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

#Region "Jpc(Code)による取得"
    Public Function GetByJpc(ByRef pDt As DataTable, _
                             ByVal pJpc As String, _
                             ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE code = @code"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
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

#Region "文字列による取得"
    ''' <summary>文字列による曖昧検索</summary>
    ''' <param name="pDt"></param>
    ''' <param name="pText">検索文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByText(ByRef pDt As DataTable, _
                              ByVal pText As String, _
                              ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE (general_name LIKE @text " & _
                                        "OR therapeutic_class LIKE @text " & _
                                        "OR trademark_names LIKE @text " & _
                                        "OR generic_name LIKE @text " & _
                                        "OR generic_name_en LIKE @text " & _
                                        "OR preparation_name LIKE @text " & _
                                        "OR company_name LIKE @text " & _
                                        "OR code LIKE @text)"

        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pText)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "colletive_name,company_nameによる取得"

    ''' <summary>
    ''' colletive_name,company_nameによる取得、ただし自身となるコードの取得は行わない
    ''' </summary>
    ''' <param name="pDt"></param>
    ''' <param name="pJpc"></param>
    ''' <param name="pCollectiveName"></param>
    ''' <param name="pCompanyName"></param>
    ''' <param name="pOnlyActive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByCollectiveNameAndCompanyName(ByRef pDt As DataTable, _
                                                      ByVal pJpc As String, _
                            ByVal pCollectiveName As String, _
                            ByVal pCompanyName As String, _
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                        "FROM " & TABLE_NAME & " " & _
                                        "WHERE collective_name = @collectiveName " & _
                                        "AND company_name = @companyName " & _
                                        " AND code <> @code "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@collectiveName", SqlDbType.NVarChar, ParameterDirection.Input, pCollectiveName)
            Me.DbManager.SetCmdParameter("@companyName", SqlDbType.NVarChar, ParameterDirection.Input, pCompanyName)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "guid " & _
                                                                        ",code " & _
                                                                        ",therapeutic_class " & _
                                                                        ",general_name " & _
                                                                        ",trademark_names " & _
                                                                        ",generic_name " & _
                                                                        ",generic_name_en " & _
                                                                        ",preparation_name " & _
                                                                        ",company_name " & _
                                                                        ",units " & _
                                                                        ",effect_usage " & _
                                                                        ",contraindicate " & _
                                                                        ",caution " & _
                                                                        ",not_combined_use " & _
                                                                        ",caution_combined_use " & _
                                                                        ",serious_adverse_event " & _
                                                                        ",other_adverse_event " & _
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
                                                                        ",additional_text " & _
                                                                        ",collective_name " & _
                                                                        ",categorysynonym " & _
                                                                        ",related_products " & _
                                                                        ",category_id1 " & _
                                                                        ",category_id2 " & _
                                                                        ",category_id3 " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@guid " & _
                                                                        ",@code " & _
                                                                        ",@therapeutic_class " & _
                                                                        ",@general_name " & _
                                                                        ",@trademark_names " & _
                                                                        ",@generic_name " & _
                                                                        ",@generic_name_en " & _
                                                                        ",@preparation_name " & _
                                                                        ",@company_name " & _
                                                                        ",@units " & _
                                                                        ",@effect_usage " & _
                                                                        ",@contraindicate " & _
                                                                        ",@caution " & _
                                                                        ",@not_combined_use " & _
                                                                        ",@caution_combined_use " & _
                                                                        ",@serious_adverse_event " & _
                                                                        ",@other_adverse_event " & _
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
                                                                        ",@additional_text " & _
                                                                        ",@collective_name " & _
                                                                        ",@categorysynonym " & _
                                                                        ",@namesynonym " & _
                                                                        ",@related_products " & _
                                                                        ",@category_id1 " & _
                                                                        ",@category_id2 " & _
                                                                        ",@category_id3 " & _
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
            Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Me.DbManager.SetCmdParameter("@therapeutic_class", SqlDbType.NVarChar, ParameterDirection.Input, pDr.therapeutic_class)
            Me.DbManager.SetCmdParameter("@general_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.general_name)
            Me.DbManager.SetCmdParameter("@trademark_names", SqlDbType.NVarChar, ParameterDirection.Input, pDr.trademark_names)
            Me.DbManager.SetCmdParameter("@generic_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.generic_name)
            Me.DbManager.SetCmdParameter("@generic_name_en", SqlDbType.NVarChar, ParameterDirection.Input, pDr.generic_name_en)
            Me.DbManager.SetCmdParameter("@preparation_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.preparation_name)
            Me.DbManager.SetCmdParameter("@company_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.company_name)
            Me.DbManager.SetCmdParameter("@units", SqlDbType.NVarChar, ParameterDirection.Input, pDr.units)
            Me.DbManager.SetCmdParameter("@effect_usage", SqlDbType.NText, ParameterDirection.Input, pDr.effect_usage)
            Me.DbManager.SetCmdParameter("@contraindicate", SqlDbType.NText, ParameterDirection.Input, pDr.contraindicate)
            Me.DbManager.SetCmdParameter("@caution", SqlDbType.NText, ParameterDirection.Input, pDr.caution)
            Me.DbManager.SetCmdParameter("@not_combined_use", SqlDbType.NText, ParameterDirection.Input, pDr.not_combined_use)
            Me.DbManager.SetCmdParameter("@caution_combined_use", SqlDbType.NText, ParameterDirection.Input, pDr.caution_combined_use)
            Me.DbManager.SetCmdParameter("@serious_adverse_event", SqlDbType.NText, ParameterDirection.Input, pDr.serious_adverse_event)
            Me.DbManager.SetCmdParameter("@other_adverse_event", SqlDbType.NText, ParameterDirection.Input, pDr.other_adverse_event)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Float, ParameterDirection.Input, pDr.sequence)
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
            If Not pDr.IsNull("additional_text") Then
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, pDr.additional_text)
            Else
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("collective_name") Then
                Me.DbManager.SetCmdParameter("@collective_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.collective_name)
            Else
                Me.DbManager.SetCmdParameter("@collective_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("categorysynonym") Then
                Me.DbManager.SetCmdParameter("@categorysynonym", SqlDbType.NText, ParameterDirection.Input, pDr.categorysynonym)
            Else
                Me.DbManager.SetCmdParameter("@categorysynonym", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("namesynonym") Then
                Me.DbManager.SetCmdParameter("@namesynonym", SqlDbType.NText, ParameterDirection.Input, pDr.namesynonym)
            Else
                Me.DbManager.SetCmdParameter("@namesynonym", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("related_products") Then
                Me.DbManager.SetCmdParameter("@related_products", SqlDbType.NVarChar, ParameterDirection.Input, pDr.related_products)
            Else
                Me.DbManager.SetCmdParameter("@related_products", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("category_id1") Then
                Me.DbManager.SetCmdParameter("@category_id1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id1)
            Else
                Me.DbManager.SetCmdParameter("@category_id1", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("category_id2") Then
                Me.DbManager.SetCmdParameter("@category_id2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id2)
            Else
                Me.DbManager.SetCmdParameter("@category_id2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("category_id3") Then
                Me.DbManager.SetCmdParameter("@category_id3", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id3)
            Else
                Me.DbManager.SetCmdParameter("@category_id3", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "guid = @guid " & _
                                            ",code = @code " & _
                                            ",therapeutic_class = @therapeutic_class " & _
                                            ",general_name = @general_name " & _
                                            ",trademark_names = @trademark_names " & _
                                            ",generic_name = @generic_name " & _
                                            ",generic_name_en = @generic_name_en " & _
                                            ",preparation_name = @preparation_name " & _
                                            ",company_name = @company_name " & _
                                            ",units = @units " & _
                                            ",effect_usage = @effect_usage " & _
                                            ",contraindicate = @contraindicate " & _
                                            ",caution = @caution " & _
                                            ",not_combined_use = @not_combined_use " & _
                                            ",caution_combined_use = @caution_combined_use " & _
                                            ",serious_adverse_event = @serious_adverse_event " & _
                                            ",other_adverse_event = @other_adverse_event " & _
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
                                            ",additional_text = @additional_text " & _
                                            ",collective_name = @collective_name " & _
                                            ",categorysynonym = @categorysynonym " & _
                                            ",namesynonym = @namesynonym " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pDr.guid)
            Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Me.DbManager.SetCmdParameter("@therapeutic_class", SqlDbType.NVarChar, ParameterDirection.Input, pDr.therapeutic_class)
            Me.DbManager.SetCmdParameter("@general_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.general_name)
            Me.DbManager.SetCmdParameter("@trademark_names", SqlDbType.NVarChar, ParameterDirection.Input, pDr.trademark_names)
            Me.DbManager.SetCmdParameter("@generic_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.generic_name)
            Me.DbManager.SetCmdParameter("@generic_name_en", SqlDbType.NVarChar, ParameterDirection.Input, pDr.generic_name_en)
            Me.DbManager.SetCmdParameter("@preparation_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.preparation_name)
            Me.DbManager.SetCmdParameter("@company_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.company_name)
            Me.DbManager.SetCmdParameter("@units", SqlDbType.NVarChar, ParameterDirection.Input, pDr.units)
            Me.DbManager.SetCmdParameter("@effect_usage", SqlDbType.NText, ParameterDirection.Input, pDr.effect_usage)
            Me.DbManager.SetCmdParameter("@contraindicate", SqlDbType.NText, ParameterDirection.Input, pDr.contraindicate)
            Me.DbManager.SetCmdParameter("@caution", SqlDbType.NText, ParameterDirection.Input, pDr.caution)
            Me.DbManager.SetCmdParameter("@not_combined_use", SqlDbType.NText, ParameterDirection.Input, pDr.not_combined_use)
            Me.DbManager.SetCmdParameter("@caution_combined_use", SqlDbType.NText, ParameterDirection.Input, pDr.caution_combined_use)
            Me.DbManager.SetCmdParameter("@serious_adverse_event", SqlDbType.NText, ParameterDirection.Input, pDr.serious_adverse_event)
            Me.DbManager.SetCmdParameter("@other_adverse_event", SqlDbType.NText, ParameterDirection.Input, pDr.other_adverse_event)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Float, ParameterDirection.Input, pDr.sequence)
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
            If Not pDr.IsNull("additional_text") Then
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, pDr.additional_text)
            Else
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("collective_name") Then
                Me.DbManager.SetCmdParameter("@collective_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.collective_name)
            Else
                Me.DbManager.SetCmdParameter("@collective_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("categorysynonym") Then
                Me.DbManager.SetCmdParameter("@categorysynonym", SqlDbType.NText, ParameterDirection.Input, pDr.categorysynonym)
            Else
                Me.DbManager.SetCmdParameter("@categorysynonym", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("namesynonym") Then
                Me.DbManager.SetCmdParameter("@namesynonym", SqlDbType.NText, ParameterDirection.Input, pDr.namesynonym)
            Else
                Me.DbManager.SetCmdParameter("@namesynonym", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("related_products") Then
                Me.DbManager.SetCmdParameter("@related_products", SqlDbType.NVarChar, ParameterDirection.Input, pDr.related_products)
            Else
                Me.DbManager.SetCmdParameter("@related_products", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("category_id1") Then
                Me.DbManager.SetCmdParameter("@category_id1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id1)
            Else
                Me.DbManager.SetCmdParameter("@category_id1", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("category_id2") Then
                Me.DbManager.SetCmdParameter("@category_id2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id2)
            Else
                Me.DbManager.SetCmdParameter("@category_id2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("category_id3") Then
                Me.DbManager.SetCmdParameter("@category_id3", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category_id3)
            Else
                Me.DbManager.SetCmdParameter("@category_id3", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function FormatCode(ByVal pJpc As String) As String
        Dim code As String = pJpc.Split("#"c)(0)
        Return Int32.Parse(code).ToString("00000000")
    End Function
#End Region

End Class
