Public Class DaoDecisionDiagram
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DecisionDiagram"
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
    Public Function GetCountByDiseaseID(ByVal pDiseaseID As Integer, _
                                        Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
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

#Region "DiseaseID及びcodeによる取得"
    Public Function GetByDiseaseIdAndCode(ByRef pDt As DataTable, _
                                          ByVal pDiseaseID As Integer, _
                                          ByVal pCode As String, _
                                          Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id " & _
                                     "AND code = @code"

        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
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
    ''' <summary>
    ''' PKによる削除
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' <summary>
    ''' DiseaseIdによる削除
    ''' </summary>
    ''' <param name="pDiseaseId">T_JP_Disease.id</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteByDiseaseId(ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "guid " & _
                                                                        ",disease_id " & _
                                                                        ",situation_id " & _
                                                                        ",header_message " & _
                                                                        ",diagram " & _
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
                                                                        ",img_id " & _
                                                                        ",code " & _
                                                                        "{0}" & _
                                                                   ") VALUES ( " & _
                                                                         "@guid " & _
                                                                        ",@disease_id " & _
                                                                        ",@situation_id " & _
                                                                        ",@header_message " & _
                                                                        ",@diagram " & _
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
                                                                        ",@img_id " & _
                                                                        ",@code " & _
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
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@situation_id", SqlDbType.Int, ParameterDirection.Input, pDr.situation_id)
            Me.DbManager.SetCmdParameter("@header_message", SqlDbType.NText, ParameterDirection.Input, pDr.header_message)
            Me.DbManager.SetCmdParameter("@diagram", SqlDbType.VarChar, ParameterDirection.Input, pDr.diagram)
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
            Me.DbManager.SetCmdParameter("@img_id", SqlDbType.Int, ParameterDirection.Input, pDr.img_id)
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim decisionDiagramId As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = decisionDiagramId
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "guid = @guid " & _
                                            ",disease_id = @disease_id " & _
                                            ",situation_id = @situation_id " & _
                                            ",header_message = @header_message " & _
                                            ",diagram = @diagram " & _
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
                                            ",img_id = @img_id " & _
                                            ",code = @code " & _
                                    "WHERE id = @id "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pDr.guid)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@situation_id", SqlDbType.Int, ParameterDirection.Input, pDr.situation_id)
            Me.DbManager.SetCmdParameter("@header_message", SqlDbType.NText, ParameterDirection.Input, pDr.header_message)
            Me.DbManager.SetCmdParameter("@diagram", SqlDbType.VarChar, ParameterDirection.Input, pDr.diagram)
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
            Me.DbManager.SetCmdParameter("@img_id", SqlDbType.Int, ParameterDirection.Input, pDr.img_id)
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
