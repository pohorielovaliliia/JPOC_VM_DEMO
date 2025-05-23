Public Class DaoDisease
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Disease"
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

#Region "PrimaryKey（DiseaseID）による取得"
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

#Region "検索文字列による曖昧検索"
    ''' <summary>
    ''' 検索文字列による曖昧検索
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCountByText(ByVal pText As String, Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                     "FROM " & TABLE_NAME & " " & _
                                     "WHERE (title LIKE @text OR synonyms LIKE @text) "
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

#Region "ID,Title一覧取得"
    Public Function GetTitleList(ByRef pDt As DataTable, _
                                 ByVal pOnlyActive As Boolean, _
                                 Optional ByVal pID As Integer = -1) As Integer
        Const SQL_QUERY As String = "SELECT " & _
                                             "id " & _
                                            ",title " & _
                                            ",status " & _
                                            ",defunct " & _
                                      "FROM " & TABLE_NAME & " " & _
                                      "{0}"
        Try
            Dim strSQL As String = String.Empty
            If pID = -1 Then
                If pOnlyActive Then
                    strSQL = String.Format(SQL_QUERY, "WHERE " & MyBase.ActiveCondition & " ")
                Else
                    strSQL = String.Format(SQL_QUERY, String.Empty)
                End If
            Else
                If pOnlyActive Then
                    strSQL = String.Format(SQL_QUERY, "WHERE id=@id AND " & MyBase.ActiveCondition & " ")
                Else
                    strSQL = String.Format(SQL_QUERY, "WHERE id=@id ")
                End If
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            If pID <> -1 Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
            End If
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, False)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ID,Title一覧取得"
    Public Function GetTargetDisease(ByRef pDt As DataTable, _
                                     ByVal pOnlyActive As Boolean, _
                                     Optional ByVal pID As Integer = -1) As Integer
        Const SQL_QUERY As String = "SELECT " & _
                                             "id AS DISEASE_ID " & _
                                            ",title AS DISEASE_TITLE " & _
                                            ",CAST(NULL AS VARCHAR(MAX)) AS RESULT " & _
                                            ",CAST(NULL AS NVARCHAR(MAX)) AS MESSAGE " & _
                                      "FROM " & TABLE_NAME & " " & _
                                      "{0}" & _
                                     "ORDER BY id"
        Try
            Dim strSQL As String = String.Empty
            If pID = -1 Then
                If pOnlyActive Then
                    strSQL = String.Format(SQL_QUERY, "WHERE " & MyBase.ActiveCondition & " ")
                Else
                    strSQL = String.Format(SQL_QUERY, String.Empty)
                End If
            Else
                If pOnlyActive Then
                    strSQL = String.Format(SQL_QUERY, "WHERE id=@id AND " & MyBase.ActiveCondition & " ")
                Else
                    strSQL = String.Format(SQL_QUERY, "WHERE id=@id ")
                End If
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            If pID <> -1 Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
            End If
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, False)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "PrimaryKey（DiseaseID）による取得"
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

#Region "SubcategoryIdによる取得"
    Public Function GetBySubcategoryId(ByRef pDt As DataTable, _
                                       ByVal pSubcategoryId As Integer, _
                                       ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = " SELECT disease.* " & _
                                    " FROM " & TABLE_NAME & " disease " & _
                                    " INNER JOIN " & DaoDiseaseSubcategoryMap.TABLE_NAME & " map " & _
                                    " ON map.disease_id = disease.id " & _
                                    " AND map.subcategoryId = @subcategoryId"

        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE " & MyBase.ActiveCondition
            strSql &= " ORDER BY map.sequence "
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

#Region "最小IDのDisease取得"
    ''' <summary>
    ''' 最小IDのDisease取得
    ''' </summary>
    ''' <param name="pDt"></param>
    ''' <returns>表示可能でIDが最小のDisease情報</returns>
    ''' <remarks>T_JP_Disease.status='A' and T_JP_Disease.defunct=0</remarks>
    Public Function GetByMinOpenDisease(ByRef pDt As DataTable) As Integer
        Const SQL_QUERY As String = "SELECT " & _
                                             "id " & _
                                            ",title " & _
                                      "FROM T_JP_Disease " & _
                                     "WHERE id = (SELECT MIN(id) " & _
                                                   "FROM T_JP_Disease " & _
                                                  "WHERE status='A' " & _
                                                    "AND defunct=0)"
        Try
            Dim strSql As String = SQL_QUERY
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

#Region "検索文字列による曖昧検索"
    ''' <summary>
    ''' 検索文字列による曖昧検索
    ''' </summary>
    ''' <param name="pDt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByText(ByRef pDt As DataTable, _
                                        ByVal pText As String, _
                                        Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                     "FROM " & TABLE_NAME & " " & _
                                     "WHERE (title LIKE @text OR synonyms LIKE @text) "
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

#End Region

#Region "削除"
    Public Function DeleteById(ByVal pID As Integer) As Integer
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_DiseaseRow, _
                           Optional ByVal pKeepIdValue As Boolean = True) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" &
                                                                 "id " &
                                                                ",guid " &
                                                                ",title " &
                                                                ",type " &
                                                                ",synonyms " &
                                                                ",importance " &
                                                                ",frequency " &
                                                                ",urgency " &
                                                                ",sequence " &
                                                                ",information " &
                                                                ",status " &
                                                                ",is_current_version " &
                                                                ",version " &
                                                                ",version_string " &
                                                                ",created_by " &
                                                                ",modified_by " &
                                                                ",checkout_by " &
                                                                ",created_date " &
                                                                ",modified_date " &
                                                                ",defunct " &
                                                                ",external_link " &
                                                                ",author_name " &
                                                                ",author_institution " &
                                                                ",coauthor_name " &
                                                                ",coauthor_institution " &
                                                                ",external_link2 " &
                                                                ",external_link3 " &
                                                                ",subcategoryId " &
                                                                ",sortid " &
                                                                ",prohibited_import " &
                                                                ",latest_editting_date " &
                                                                ",WideTitle " &
                                                                ",AuthorIntroducation " &
                                                                ",inactive_message " &
                                                                ",publish_prod " &
                                                                ",history_text " &
                                                                ",is_wip " &
                                                                ",introduction_author " &
                                                                ",introduction_author_institution " &
                                                                ",introduction_expertise " &
                                                                ",introduction_specialist " &
                                                                ",introduction_academy " &
                                                                ",introduction_resume " &
                                                                ",introduction_advice " &
                                                                ",introduction_external " &
                                                                ",introduction_author_message " &
                                                                ",introduction_author_photo " &
                                                                ",medicalsafety " &
                                                                ",ref_id " &
                                                            ") " &
                                                     "VALUES (" &
                                                                 "@id " &
                                                                ",@guid " &
                                                                ",@title " &
                                                                ",@type " &
                                                                ",@synonyms " &
                                                                ",@importance " &
                                                                ",@frequency " &
                                                                ",@urgency " &
                                                                ",@sequence " &
                                                                ",@information " &
                                                                ",@status " &
                                                                ",@is_current_version " &
                                                                ",@version " &
                                                                ",@version_string " &
                                                                ",@created_by " &
                                                                ",@modified_by " &
                                                                ",@checkout_by " &
                                                                ",@created_date " &
                                                                ",@modified_date " &
                                                                ",@defunct " &
                                                                ",@external_link " &
                                                                ",@author_name " &
                                                                ",@author_institution " &
                                                                ",@coauthor_name " &
                                                                ",@coauthor_institution " &
                                                                ",@external_link2 " &
                                                                ",@external_link3 " &
                                                                ",@subcategoryId " &
                                                                ",@sortid " &
                                                                ",@prohibited_import " &
                                                                ",@latest_editting_date " &
                                                                ",@WideTitle " &
                                                                ",@AuthorIntroducation " &
                                                                ",@inactive_message " &
                                                                ",@publish_prod " &
                                                                ",@history_text " &
                                                                ",@is_wip " &
                                                                ",@introduction_author " &
                                                                ",@introduction_author_institution " &
                                                                ",@introduction_expertise " &
                                                                ",@introduction_specialist " &
                                                                ",@introduction_academy " &
                                                                ",@introduction_resume " &
                                                                ",@introduction_advice " &
                                                                ",@introduction_external " &
                                                                ",@introduction_author_message " &
                                                                ",@introduction_author_photo " &
                                                                ",@medicalsafety " &
                                                                ",@ref_id " &
                                                             ");"

        Try
            ''ref_id added on 2017/01/18
            Dim strSQL As String = String.Empty
            If pKeepIdValue Then
                strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) &
                         String.Format(SQL_QUERY, ",id ", ",@id ") &
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pDr.guid)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.type)
            Me.DbManager.SetCmdParameter("@synonyms", SqlDbType.NText, ParameterDirection.Input, pDr.synonyms)
            Me.DbManager.SetCmdParameter("@importance", SqlDbType.Int, ParameterDirection.Input, pDr.importance)
            Me.DbManager.SetCmdParameter("@frequency", SqlDbType.Int, ParameterDirection.Input, pDr.frequency)
            Me.DbManager.SetCmdParameter("@urgency", SqlDbType.Int, ParameterDirection.Input, pDr.urgency)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@information", SqlDbType.NText, ParameterDirection.Input, pDr.information)
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
            If Not pDr.IsNull("external_link") Then
                Me.DbManager.SetCmdParameter("@external_link", SqlDbType.NText, ParameterDirection.Input, pDr.external_link)
            Else
                Me.DbManager.SetCmdParameter("@external_link", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("author_name") Then
                Me.DbManager.SetCmdParameter("@author_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.author_name)
            Else
                Me.DbManager.SetCmdParameter("@author_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("author_institution") Then
                Me.DbManager.SetCmdParameter("@author_institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.author_institution)
            Else
                Me.DbManager.SetCmdParameter("@author_institution", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("coauthor_name") Then
                Me.DbManager.SetCmdParameter("@coauthor_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.coauthor_name)
            Else
                Me.DbManager.SetCmdParameter("@coauthor_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("coauthor_institution") Then
                Me.DbManager.SetCmdParameter("@coauthor_institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.coauthor_institution)
            Else
                Me.DbManager.SetCmdParameter("@coauthor_institution", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("external_link2") Then
                Me.DbManager.SetCmdParameter("@external_link2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.external_link2)
            Else
                Me.DbManager.SetCmdParameter("@external_link2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("external_link3") Then
                Me.DbManager.SetCmdParameter("@external_link3", SqlDbType.NVarChar, ParameterDirection.Input, pDr.external_link3)
            Else
                Me.DbManager.SetCmdParameter("@external_link3", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("subcategoryId") Then
                Me.DbManager.SetCmdParameter("@subcategoryId", SqlDbType.Int, ParameterDirection.Input, pDr.subcategoryId)
            Else
                Me.DbManager.SetCmdParameter("@subcategoryId", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sortid") Then
                Me.DbManager.SetCmdParameter("@sortid", SqlDbType.Int, ParameterDirection.Input, pDr.sortid)
            Else
                Me.DbManager.SetCmdParameter("@sortid", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@prohibited_import", SqlDbType.Bit, ParameterDirection.Input, pDr.prohibited_import)
            If Not pDr.IsNull("latest_editting_date") Then
                Me.DbManager.SetCmdParameter("@latest_editting_date", SqlDbType.Date, ParameterDirection.Input, pDr.latest_editting_date)
            Else
                Me.DbManager.SetCmdParameter("@latest_editting_date", SqlDbType.Date, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("WideTitle") Then
                Me.DbManager.SetCmdParameter("@WideTitle", SqlDbType.Int, ParameterDirection.Input, pDr.WideTitle)
            Else
                Me.DbManager.SetCmdParameter("@WideTitle", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("AuthorIntroducation") Then
                Me.DbManager.SetCmdParameter("@AuthorIntroducation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.AuthorIntroducation)
            Else
                Me.DbManager.SetCmdParameter("@AuthorIntroducation", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("inactive_message") Then
                Me.DbManager.SetCmdParameter("@inactive_message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.inactive_message)
            Else
                Me.DbManager.SetCmdParameter("@inactive_message", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@publish_prod", SqlDbType.Bit, ParameterDirection.Input, pDr.publish_prod)
            If Not pDr.IsNull("history_text") Then
                Me.DbManager.SetCmdParameter("@history_text", SqlDbType.NVarChar, ParameterDirection.Input, pDr.history_text)
            Else
                Me.DbManager.SetCmdParameter("@history_text", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("is_wip") Then
                Me.DbManager.SetCmdParameter("@is_wip", SqlDbType.Bit, ParameterDirection.Input, pDr.is_wip)
            Else
                Me.DbManager.SetCmdParameter("@is_wip", SqlDbType.Bit, ParameterDirection.Input, False)
            End If
            '執筆者情報
            If Not pDr.IsNull("introduction_author") Then
                Me.DbManager.SetCmdParameter("@introduction_author", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_author_institution") Then
                Me.DbManager.SetCmdParameter("@introduction_author_institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author_institution)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author_institution", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_expertise") Then
                Me.DbManager.SetCmdParameter("@introduction_expertise", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_expertise)
            Else
                Me.DbManager.SetCmdParameter("@introduction_expertise", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_specialist") Then
                Me.DbManager.SetCmdParameter("@introduction_specialist", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_specialist)
            Else
                Me.DbManager.SetCmdParameter("@introduction_specialist", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_academy") Then
                Me.DbManager.SetCmdParameter("@introduction_academy", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_academy)
            Else
                Me.DbManager.SetCmdParameter("@introduction_academy", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_resume") Then
                Me.DbManager.SetCmdParameter("@introduction_resume", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_resume)
            Else
                Me.DbManager.SetCmdParameter("@introduction_resume", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_advice") Then
                Me.DbManager.SetCmdParameter("@introduction_advice", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_advice)
            Else
                Me.DbManager.SetCmdParameter("@introduction_advice", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_external") Then
                Me.DbManager.SetCmdParameter("@introduction_external", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_external)
            Else
                Me.DbManager.SetCmdParameter("@introduction_external", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_author_message") Then
                Me.DbManager.SetCmdParameter("@introduction_author_message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author_message)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author_message", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_author_photo") Then
                Me.DbManager.SetCmdParameter("@introduction_author_photo", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author_photo)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author_photo", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("medicalsafety") Then
                Me.DbManager.SetCmdParameter("@medicalsafety", SqlDbType.NVarChar, ParameterDirection.Input, pDr.medicalsafety)
            Else
                Me.DbManager.SetCmdParameter("@medicalsafety", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ref_id") Then ''added on 2017/01/18
                Me.DbManager.SetCmdParameter("@ref_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ref_id)
            Else
                Me.DbManager.SetCmdParameter("@ref_id", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim diseaseId As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = diseaseId
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_DiseaseRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "guid = @guid " & _
                                            ",title = @title " & _
                                            ",type = @type " & _
                                            ",synonyms = @synonyms " & _
                                            ",importance = @importance " & _
                                            ",frequency = @frequency " & _
                                            ",urgency = @urgency " & _
                                            ",sequence = @sequence " & _
                                            ",information = @information " & _
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
                                            ",external_link = @external_link " & _
                                            ",author_name = @author_name " & _
                                            ",author_institution = @author_institution " & _
                                            ",coauthor_name = @coauthor_name " & _
                                            ",coauthor_institution = @coauthor_institution " & _
                                            ",external_link2 = @external_link2 " & _
                                            ",external_link3 = @external_link3 " & _
                                            ",subcategoryId = @subcategoryId " & _
                                            ",sortid = @sortid " & _
                                            ",prohibited_import = @prohibited_import " & _
                                            ",latest_editting_date = @latest_editting_date " & _
                                            ",WideTitle = @WideTitle " & _
                                            ",AuthorIntroducation = @AuthorIntroducation " & _
                                            ",inactive_message = @inactive_message " & _
                                            ",publish_prod = @publish_prod " & _
                                            ",history_text = @history_text " & _
                                            ",is_wip = @is_wip " & _
                                            ",introduction_author = @introduction_author " & _
                                            ",introduction_author_institution = @introduction_author_institution " & _
                                            ",introduction_expertise = @introduction_expertise " & _
                                            ",introduction_specialist = @introduction_specialist " & _
                                            ",introduction_academy = @introduction_academy " & _
                                            ",introduction_resume = @introduction_resume " & _
                                            ",introduction_advice = @introduction_advice " & _
                                            ",introduction_external = @introduction_external " & _
                                            ",introduction_author_message = @introduction_author_message " & _
                                            ",introduction_author_photo = @introduction_author_photo " & _
                                            ",medicalsafety = @medicalsafety " & _
                                    "WHERE id = @id "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pDr.guid)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.type)
            Me.DbManager.SetCmdParameter("@synonyms", SqlDbType.NText, ParameterDirection.Input, pDr.synonyms)
            Me.DbManager.SetCmdParameter("@importance", SqlDbType.Int, ParameterDirection.Input, pDr.importance)
            Me.DbManager.SetCmdParameter("@frequency", SqlDbType.Int, ParameterDirection.Input, pDr.frequency)
            Me.DbManager.SetCmdParameter("@urgency", SqlDbType.Int, ParameterDirection.Input, pDr.urgency)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@information", SqlDbType.NText, ParameterDirection.Input, pDr.information)
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
            If Not pDr.IsNull("external_link") Then
                Me.DbManager.SetCmdParameter("@external_link", SqlDbType.NText, ParameterDirection.Input, pDr.external_link)
            Else
                Me.DbManager.SetCmdParameter("@external_link", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("author_name") Then
                Me.DbManager.SetCmdParameter("@author_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.author_name)
            Else
                Me.DbManager.SetCmdParameter("@author_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("author_institution") Then
                Me.DbManager.SetCmdParameter("@author_institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.author_institution)
            Else
                Me.DbManager.SetCmdParameter("@author_institution", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("coauthor_name") Then
                Me.DbManager.SetCmdParameter("@coauthor_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.coauthor_name)
            Else
                Me.DbManager.SetCmdParameter("@coauthor_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("coauthor_institution") Then
                Me.DbManager.SetCmdParameter("@coauthor_institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.coauthor_institution)
            Else
                Me.DbManager.SetCmdParameter("@coauthor_institution", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("external_link2") Then
                Me.DbManager.SetCmdParameter("@external_link2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.external_link2)
            Else
                Me.DbManager.SetCmdParameter("@external_link2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("external_link3") Then
                Me.DbManager.SetCmdParameter("@external_link3", SqlDbType.NVarChar, ParameterDirection.Input, pDr.external_link3)
            Else
                Me.DbManager.SetCmdParameter("@external_link3", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("subcategoryId") Then
                Me.DbManager.SetCmdParameter("@subcategoryId", SqlDbType.Int, ParameterDirection.Input, pDr.subcategoryId)
            Else
                Me.DbManager.SetCmdParameter("@subcategoryId", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sortid") Then
                Me.DbManager.SetCmdParameter("@sortid", SqlDbType.Int, ParameterDirection.Input, pDr.sortid)
            Else
                Me.DbManager.SetCmdParameter("@sortid", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@prohibited_import", SqlDbType.Bit, ParameterDirection.Input, pDr.prohibited_import)
            If Not pDr.IsNull("latest_editting_date") Then
                Me.DbManager.SetCmdParameter("@latest_editting_date", SqlDbType.Date, ParameterDirection.Input, pDr.latest_editting_date)
            Else
                Me.DbManager.SetCmdParameter("@latest_editting_date", SqlDbType.Date, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("WideTitle") Then
                Me.DbManager.SetCmdParameter("@WideTitle", SqlDbType.Int, ParameterDirection.Input, pDr.WideTitle)
            Else
                Me.DbManager.SetCmdParameter("@WideTitle", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("AuthorIntroducation") Then
                Me.DbManager.SetCmdParameter("@AuthorIntroducation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.AuthorIntroducation)
            Else
                Me.DbManager.SetCmdParameter("@AuthorIntroducation", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("inactive_message") Then
                Me.DbManager.SetCmdParameter("@inactive_message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.inactive_message)
            Else
                Me.DbManager.SetCmdParameter("@inactive_message", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@publish_prod", SqlDbType.Bit, ParameterDirection.Input, pDr.publish_prod)
            If Not pDr.IsNull("history_text") Then
                Me.DbManager.SetCmdParameter("@history_text", SqlDbType.NVarChar, ParameterDirection.Input, pDr.history_text)
            Else
                Me.DbManager.SetCmdParameter("@history_text", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("is_wip") Then
                Me.DbManager.SetCmdParameter("@is_wip", SqlDbType.Bit, ParameterDirection.Input, pDr.is_wip)
            Else
                Me.DbManager.SetCmdParameter("@is_wip", SqlDbType.Bit, ParameterDirection.Input, False)
            End If

            '執筆者情報
            If Not pDr.IsNull("introduction_author") Then
                Me.DbManager.SetCmdParameter("@introduction_author", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_author_institution") Then
                Me.DbManager.SetCmdParameter("@introduction_author_institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author_institution)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author_institution", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_expertise") Then
                Me.DbManager.SetCmdParameter("@introduction_expertise", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_expertise)
            Else
                Me.DbManager.SetCmdParameter("@introduction_expertise", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_specialist") Then
                Me.DbManager.SetCmdParameter("@introduction_specialist", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_specialist)
            Else
                Me.DbManager.SetCmdParameter("@introduction_specialist", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_academy") Then
                Me.DbManager.SetCmdParameter("@introduction_academy", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_academy)
            Else
                Me.DbManager.SetCmdParameter("@introduction_academy", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_resume") Then
                Me.DbManager.SetCmdParameter("@introduction_resume", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_resume)
            Else
                Me.DbManager.SetCmdParameter("@introduction_resume", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_advice") Then
                Me.DbManager.SetCmdParameter("@introduction_advice", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_advice)
            Else
                Me.DbManager.SetCmdParameter("@introduction_advice", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_external") Then
                Me.DbManager.SetCmdParameter("@introduction_external", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_external)
            Else
                Me.DbManager.SetCmdParameter("@introduction_external", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_author_message") Then
                Me.DbManager.SetCmdParameter("@introduction_author_message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author_message)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author_message", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("introduction_author_photo") Then
                Me.DbManager.SetCmdParameter("@introduction_author_photo", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction_author_photo)
            Else
                Me.DbManager.SetCmdParameter("@introduction_author_photo", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If

            If Not pDr.IsNull("medicalsafety") Then
                Me.DbManager.SetCmdParameter("@medicalsafety", SqlDbType.NVarChar, ParameterDirection.Input, pDr.medicalsafety)
            Else
                Me.DbManager.SetCmdParameter("@medicalsafety", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
