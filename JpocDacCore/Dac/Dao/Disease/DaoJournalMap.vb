Public Class DaoJournalMap
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_JournalMap"
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
            If pOnlyActive Then strSql &= " WHERE defunct = 0 "
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
            If pOnlyActive Then strSql &= " AND defunct = 0 "
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
                                     "WHERE id IN (" & _
                                                    "SELECT id " & _
                                                      "FROM " & TABLE_NAME & " " & _
                                                     "WHERE parent IN ('step1','top') " & _
                                                       "AND parent_id = @disease_id " & _
                                                       "{0}" & _
                                                     "UNION " & _
                                                    "SELECT id " & _
                                                      "FROM " & TABLE_NAME & " " & _
                                                     "WHERE parent = 'evidence' " & _
                                                       "AND parent_id IN (" & _
                                                                         "SELECT t1.id " & _
                                                                           "FROM " & DaoEvidence.TABLE_NAME & " t1 " & _
                                                                                    "INNER JOIN " & DaoEvidenceDisease.TABLE_NAME & " t2 ON " & _
                                                                                        "t2.evidence_id = t1.id " & _
                                                                          "WHERE t2.disease_id = @disease_id" & _
                                                                            "{1}" & _
                                                                         ") " & _
                                                       "{0}" & _
                                                     "UNION " & _
                                                    "SELECT id " & _
                                                      "FROM " & TABLE_NAME & " " & _
                                                     "WHERE parent = 'image' " & _
                                                       "AND parent_id IN (" & _
                                                                         "SELECT t1.id " & _
                                                                           "FROM " & DaoImage.TABLE_NAME & " t1 " & _
                                                                                    "INNER JOIN " & DaoImgMapping.TABLE_NAME & " t2 ON " & _
                                                                                        "t2.image_id = t1.id " & _
                                                                                        "{2}" & _
                                                                          "WHERE t2.disease_id = @disease_id" & _
                                                                            "{1}" & _
                                                                         ")" & _
                                                       "{0}" & _
                                                 ")" & _
                                       "{0}"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND defunct = 0 ", " AND " & MyBase.ActiveCondition("t1"), " AND " & MyBase.ActiveCondition("t2"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)
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
            If pOnlyActive Then strSql &= " WHERE defunct = 0 "
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
                                                    "SELECT id " & _
                                                      "FROM " & TABLE_NAME & " " & _
                                                     "WHERE parent IN ('step1', 'top') " & _
                                                       "AND parent_id = @disease_id " & _
                                                       "{0}" & _
                                                     "UNION " & _
                                                    "SELECT id " & _
                                                      "FROM " & TABLE_NAME & " " & _
                                                     "WHERE parent = 'evidence' " & _
                                                       "AND parent_id IN (" & _
                                                                         "SELECT t1.id " & _
                                                                           "FROM " & DaoEvidence.TABLE_NAME & " t1 " & _
                                                                                    "INNER JOIN " & DaoEvidenceDisease.TABLE_NAME & " t2 ON " & _
                                                                                        "t2.evidence_id = t1.id " & _
                                                                          "WHERE t2.disease_id = @disease_id" & _
                                                                            "{1}" & _
                                                                         ") " & _
                                                       "{0}" & _
                                                     "UNION " & _
                                                    "SELECT id " & _
                                                      "FROM " & TABLE_NAME & " " & _
                                                     "WHERE parent = 'image' " & _
                                                       "AND parent_id IN (" & _
                                                                         "SELECT t1.id " & _
                                                                           "FROM " & DaoImage.TABLE_NAME & " t1 " & _
                                                                                    "INNER JOIN " & DaoImgMapping.TABLE_NAME & " t2 ON " & _
                                                                                        "t2.image_id = t1.id " & _
                                                                                        "{2}" & _
                                                                          "WHERE t2.disease_id = @disease_id" & _
                                                                            "{1}" & _
                                                                         ")" & _
                                                       "{0}" & _
                                                 ")" & _
                                       "{0}"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND defunct = 0 ", " AND " & MyBase.ActiveCondition("t1"), " AND " & MyBase.ActiveCondition("t2"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)
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

#Region "PrimaryKeyによる取得"
    Public Function GetByPK(ByRef pDt As DataTable, _
                            ByVal pID As Integer, _
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND defunct = 0"
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_JournalMapRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                      "parent " & _
                                                                     ",parent_id " & _
                                                                     ",journal_id " & _
                                                                     ",defunct " & _
                                                                     "{0}" & _
                                                                   ") VALUES ( " & _
                                                                      "@parent " & _
                                                                     ",@parent_id " & _
                                                                     ",@journal_id " & _
                                                                     ",@defunct " & _
                                                                     "{1}" & _
                                                                   "); "
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
            If Not pDr.IsNull("parent") Then
                Me.DbManager.SetCmdParameter("@parent", SqlDbType.VarChar, ParameterDirection.Input, pDr.parent)
            Else
                Me.DbManager.SetCmdParameter("@parent", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("parent_id") Then
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, pDr.parent_id)
            Else
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("journal_id") Then
                Me.DbManager.SetCmdParameter("@journal_id", SqlDbType.Int, ParameterDirection.Input, pDr.journal_id)
            Else
                Me.DbManager.SetCmdParameter("@journal_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("defunct") Then
                Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Else
                Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim journalMapID As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = journalMapID
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_JournalMapRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "parent = @parent " & _
                                            ",parent_id = @parent_id " & _
                                            ",journal_id = @journal_id " & _
                                            ",defunct = @defunct " & _
                                    "WHERE id = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            If Not pDr.IsNull("parent") Then
                Me.DbManager.SetCmdParameter("@parent", SqlDbType.VarChar, ParameterDirection.Input, pDr.parent)
            Else
                Me.DbManager.SetCmdParameter("@parent", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("parent_id") Then
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, pDr.parent_id)
            Else
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("journal_id") Then
                Me.DbManager.SetCmdParameter("@journal_id", SqlDbType.Int, ParameterDirection.Input, pDr.journal_id)
            Else
                Me.DbManager.SetCmdParameter("@journal_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("defunct") Then
                Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Else
                Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
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
