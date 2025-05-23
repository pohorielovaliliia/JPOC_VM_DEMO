Public Class DaoHandout
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Handout"
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
    Public Function GetCountByPK(ByVal pID As Integer,
                                 Optional ByVal pOnlyActive As Boolean = True) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " &
                                      "FROM " & TABLE_NAME & " " &
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
    Public Function GetCountByDiseaseID(ByVal pDiseaseID As Integer,
                                        Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE disease_id = @disease_id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
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
        Const SQL_QUERY As String = "SELECT * " &
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
    Public Function GetByDiseaseID(ByRef pDt As DataTable,
                                   ByVal pDiseaseID As Integer,
                                   Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE disease_id = @disease_id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
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

#Region "PrimaryKeyによる取得"
    Public Function GetByPK(ByRef pDt As DataTable,
                            ByVal pID As Integer,
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
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
    ''' <summary>
    ''' PKによる削除
    ''' </summary>
    ''' <param name="pID">T_JP_ActionItem.id</param>
    ''' <returns>削除件数</returns>
    ''' <remarks></remarks>
    Public Function Delete(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " &
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
    ''' <summary>
    ''' DiseaseIdによる削除
    ''' </summary>
    ''' <param name="pDiseaseId">T_JP_DiseaseId</param>
    ''' <returns>削除件数</returns>
    ''' <remarks></remarks>
    Public Function DeleteByDiseaseId(ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE FROM " & TABLE_NAME &
                                     "WHERE disease_id = @disease_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_HandoutRow,
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (disease_id, title, top_title, top_col_1, top_col_2, top_col_setting, 
            btm_title, btm_col_1, btm_col_2, btm_col_setting, additional_title, additional_subtitle_1, additional_col_1, additional_subtitle_2, 
            additional_col_2, custom_html, created_date, modified_date, pdf_created_date) " &
            "VALUES ( @disease_id, @title, @top_title, @top_col_1, @top_col_2, @top_col_setting, @btm_title, @btm_col_1, @btm_col_2, 
            @btm_col_setting, @additional_title, @additional_subtitle_1, @additional_col_1, @additional_subtitle_2, @additional_col_2, 
            @custom_html, @created_date, @modified_date, @pdf_created_date);"
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
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@top_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.top_title)
            Me.DbManager.SetCmdParameter("@top_col_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.top_col_1)
            Me.DbManager.SetCmdParameter("@top_col_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.top_col_2)
            Me.DbManager.SetCmdParameter("@top_col_setting", SqlDbType.Int, ParameterDirection.Input, pDr.top_col_setting)
            Me.DbManager.SetCmdParameter("@btm_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_title)
            Me.DbManager.SetCmdParameter("@btm_col_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_col_1)
            Me.DbManager.SetCmdParameter("@btm_col_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_col_2)
            Me.DbManager.SetCmdParameter("@btm_col_setting", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_col_setting)
            Me.DbManager.SetCmdParameter("@additional_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_title)
            Me.DbManager.SetCmdParameter("@additional_subtitle_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_subtitle_1)
            Me.DbManager.SetCmdParameter("@additional_col_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_col_1)
            Me.DbManager.SetCmdParameter("@additional_subtitle_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_subtitle_2)
            Me.DbManager.SetCmdParameter("@additional_col_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_col_2)
            Me.DbManager.SetCmdParameter("@custom_html", SqlDbType.NVarChar, ParameterDirection.Input, pDr.custom_html)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@pdf_created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.pdf_created_date)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim actionItemID As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = actionItemID
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_HandoutRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " &
                                       "SET " &
                                        "title = @title, 
                                        top_title = @top_title, 
		                                top_col_1 = @top_col_1, 
		                                top_col_2 =  @top_col_2,
		                                top_col_setting = @top_col_setting,
		                                btm_title = @btm_title,
		                                btm_col_1 = @btm_col_1,
		                                btm_col_2 = @btm_col_2,
		                                btm_col_setting = @btm_col_setting,
		                                additional_title = @additional_title,
		                                additional_subtitle_1 = @additional_subtitle_1,
		                                additional_col_1 = @additional_col_1,
		                                additional_subtitle_2 = @additional_subtitle_2, 
		                                additional_col_2 = @additional_col_2, 
		                                modified_date = @modified_date" &
                                    "WHERE id = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@top_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.top_title)
            Me.DbManager.SetCmdParameter("@top_col_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.top_col_1)
            Me.DbManager.SetCmdParameter("@top_col_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.top_col_2)
            Me.DbManager.SetCmdParameter("@top_col_setting", SqlDbType.Int, ParameterDirection.Input, pDr.top_col_setting)
            Me.DbManager.SetCmdParameter("@btm_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_title)
            Me.DbManager.SetCmdParameter("@btm_col_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_col_1)
            Me.DbManager.SetCmdParameter("@btm_col_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_col_2)
            Me.DbManager.SetCmdParameter("@btm_col_setting", SqlDbType.NVarChar, ParameterDirection.Input, pDr.btm_col_setting)
            Me.DbManager.SetCmdParameter("@additional_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_title)
            Me.DbManager.SetCmdParameter("@additional_subtitle_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_subtitle_1)
            Me.DbManager.SetCmdParameter("@additional_col_1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_col_1)
            Me.DbManager.SetCmdParameter("@additional_subtitle_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_subtitle_2)
            Me.DbManager.SetCmdParameter("@additional_col_2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.additional_col_2)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
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
