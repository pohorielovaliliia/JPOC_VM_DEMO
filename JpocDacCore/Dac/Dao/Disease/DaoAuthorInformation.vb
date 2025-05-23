Imports System.Data

Public Class DaoAuthorInformation
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_AuthorInformation"
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
                                      "FROM " & TABLE_NAME & " ai INNER JOIN T_JP_AuthorMapping am on ai.id = am.author_id " &
                                     "WHERE am.disease_id = @disease_id and am.softdelete = 0"
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
        Const SQL_QUERY As String = "SELECT ai.* " &
                                      "FROM " & TABLE_NAME & " ai INNER JOIN T_JP_AuthorMapping am on ai.id = am.author_id " &
                                     "WHERE am.disease_id = @disease_id and am.softdelete = 0"
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
    Public Function DeleteById(ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " SET softdelete = 1 " &
                                     "WHERE id = @id and softdelete = 0"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function

    Public Function DeleteByDiseaseId(ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " SET softdelete = 1 " &
                                     "WHERE id in (select author_id from T_JP_AuthorMapping where disease_id = @disease_id) and softdelete = 0"
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_AuthorInformationRow,
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " ( id" &
                                                                     ",name " &
                                                                     ",institution " &
                                                                     ",introduction " &
                                                                     ",expertise " &
                                                                     ",specialist " &
                                                                     ",academy " &
                                                                     ",resume " &
                                                                     ",advice " &
                                                                     ",[external] " &
                                                                     ",message " &
                                                                     ",photo " &
                                                                     ",modified_by " &
                                                                     ",modified_date " &
                                                                     ",created_by " &
                                                                     ",created_date " &
                                                                     ",softdelete " &
                                                                     ",coi " &
                                                                     ",coi_year " &
                                                                   ") VALUES (" &
                                                                      "@id " &
                                                                      ",@name " &
                                                                      ",@institution " &
                                                                      ",@introduction " &
                                                                      ",@expertise " &
                                                                      ",@specialist " &
                                                                      ",@academy " &
                                                                      ",@resume " &
                                                                      ",@advice " &
                                                                      ",@external " &
                                                                      ",@message " &
                                                                      ",@photo " &
                                                                      ",@modified_by " &
                                                                      ",@modified_date " &
                                                                      ",@created_by " &
                                                                      ",@created_date " &
                                                                      ",@softdelete " &
                                                                      ",@coi " &
                                                                      ",@coi_year " &
                                                                    ");"
        Try
            Dim strSQL As String = SQL_QUERY & SCOPE_IDENTITY
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            Me.DbManager.SetCmdParameter("@institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.institution)
            Me.DbManager.SetCmdParameter("@introduction", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction)
            Me.DbManager.SetCmdParameter("@expertise", SqlDbType.NVarChar, ParameterDirection.Input, pDr.expertise)
            Me.DbManager.SetCmdParameter("@specialist", SqlDbType.NVarChar, ParameterDirection.Input, pDr.specialist)
            Me.DbManager.SetCmdParameter("@academy", SqlDbType.NVarChar, ParameterDirection.Input, pDr.academy)
            Me.DbManager.SetCmdParameter("@resume", SqlDbType.NVarChar, ParameterDirection.Input, pDr._resume)
            Me.DbManager.SetCmdParameter("@advice", SqlDbType.NVarChar, ParameterDirection.Input, pDr.advice)
            Me.DbManager.SetCmdParameter("@external", SqlDbType.NVarChar, ParameterDirection.Input, pDr.external)
            Me.DbManager.SetCmdParameter("@message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.message)
            Me.DbManager.SetCmdParameter("@photo", SqlDbType.NVarChar, ParameterDirection.Input, pDr.photo)

            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@softdelete", SqlDbType.Bit, ParameterDirection.Input, pDr.softdelete)
            Me.DbManager.SetCmdParameter("@coi", SqlDbType.NText, ParameterDirection.Input, pDr.coi)
            Me.DbManager.SetCmdParameter("@coi_year", SqlDbType.Int, ParameterDirection.Input, pDr.coi_year)
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_AuthorInformationRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " &
                                       "SET " &
                                             "name = @name " &
                                            ", institution = @institution " &
                                            ", introduction = @introduction " &
                                            ", expertise = @expertise " &
                                            ", specialist = @specialist " &
                                            ", academy = @academy " &
                                            ", resume = @resume " &
                                            ", advice = @advice " &
                                            ", [external] = @external " &
                                            ", message = @message " &
                                            ", photo = @photo " &
                                            ", modified_by = @modified_by " &
                                            ", modified_date = @modified_date " &
                                            ", created_by = @created_by " &
                                            ", created_date = @created_date " &
                                            ", softdelete = @softdelete " &
                                            ", coi = @coi " &
                                            ", coi_year = @coi_year " &
                                    "WHERE id = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            Me.DbManager.SetCmdParameter("@institution", SqlDbType.NVarChar, ParameterDirection.Input, pDr.institution)
            Me.DbManager.SetCmdParameter("@introduction", SqlDbType.NVarChar, ParameterDirection.Input, pDr.introduction)
            Me.DbManager.SetCmdParameter("@expertise", SqlDbType.NVarChar, ParameterDirection.Input, pDr.expertise)
            Me.DbManager.SetCmdParameter("@specialist", SqlDbType.NVarChar, ParameterDirection.Input, pDr.specialist)
            Me.DbManager.SetCmdParameter("@academy", SqlDbType.NVarChar, ParameterDirection.Input, pDr.academy)
            Me.DbManager.SetCmdParameter("@resume", SqlDbType.NVarChar, ParameterDirection.Input, pDr._resume)
            Me.DbManager.SetCmdParameter("@advice", SqlDbType.NVarChar, ParameterDirection.Input, pDr.advice)
            Me.DbManager.SetCmdParameter("@external", SqlDbType.NVarChar, ParameterDirection.Input, pDr.external)
            Me.DbManager.SetCmdParameter("@message", SqlDbType.NVarChar, ParameterDirection.Input, pDr.message)
            Me.DbManager.SetCmdParameter("@photo", SqlDbType.NVarChar, ParameterDirection.Input, pDr.photo)

            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@softdelete", SqlDbType.Int, ParameterDirection.Input, pDr.softdelete)
            Me.DbManager.SetCmdParameter("@coi", SqlDbType.NText, ParameterDirection.Input, pDr.coi)
            Me.DbManager.SetCmdParameter("@coi_year", SqlDbType.Int, ParameterDirection.Input, pDr.coi_year)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
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
