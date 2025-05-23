Public Class DaoDifferentialDiagnosis_AnswerBox
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DifferentialDiagnosis_AnswerBox"
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
    Public Function GetCountByPK(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
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
    Public Function GetCountByDiseaseID(ByVal pDiseaseID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE disease_id = @disease_id"
        Try
            Dim strSql As String = SQL_QUERY
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
                                   ByVal pDiseaseID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE disease_id = @disease_id"
        Try
            Dim strSql As String = SQL_QUERY
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
    Public Function GetByDiseaseIdAndCode(ByRef pDt As DataTable,
                                          ByVal pDiseaseID As Integer,
                                          ByVal pCode As String) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE disease_id = @disease_id " &
                                     "AND code = @code"
        Try
            Dim strSql As String = SQL_QUERY
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
    Public Function GetByPK(ByRef pDt As DataTable,
                            ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                      "FROM " & TABLE_NAME & " " &
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
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
    ''' <param name="pDiseaseId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteByDiseaseId(ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " &
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_DifferentialDiagnosis_AnswerBoxRow,
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" &
                                                                  "disease_id, content, sequence, code, title, priority, created_by, created_date, modified_by, modified_date" &
                                                                  "{0}" &
                                                                  ") VALUES (" &
                                                                  "@disease_id, @content, @sequence, @code, @title, @priority, @created_by, @created_date, @modified_by, @modified_date" &
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
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@content", SqlDbType.NText, ParameterDirection.Input, pDr.content)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@priority", SqlDbType.Int, ParameterDirection.Input, pDr.priority)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim differentialDiagnosisID As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = differentialDiagnosisID
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_DifferentialDiagnosis_AnswerBoxRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " &
                                       "SET " &
                                             "disease_id = @disease_id " &
                                            ",content = @content " &
                                            ",sequence = @sequence " &
                                            ",code = @code " &
                                            ",title = @title " &
                                            ",priority = @priority " &
                                            ",created_by = @created_by " &
                                            ",modified_by = @modified_by " &
                                            ",created_date = @created_date " &
                                            ",modified_date = @modified_date " &
                                    "WHERE id = @id "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@content", SqlDbType.NText, ParameterDirection.Input, pDr.content)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@priority", SqlDbType.Int, ParameterDirection.Input, pDr.priority)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
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
