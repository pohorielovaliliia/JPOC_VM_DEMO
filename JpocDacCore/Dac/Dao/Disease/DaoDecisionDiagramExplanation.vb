Public Class DaoDecisionDiagramExplanation
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DecisionDiagramExplanation"
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

#Region "PrimaryKeyによる取得"
    Public Function GetCountByPK(ByVal pDecisiondiagramID As Integer, _
                                 ByVal pSequence As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE decisiondiagram_id = @decisiondiagram_id " & _
                                       "AND sequence = @sequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@decisiondiagram_id", SqlDbType.Int, ParameterDirection.Input, pDecisiondiagramID)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pSequence)
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
                                     "WHERE decisiondiagram_id IN (" & _
                                                                    "SELECT DISTINCT t1.decisiondiagram_id " & _
                                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                                            "INNER JOIN " & DaoDecisionDiagram.TABLE_NAME & " t2 ON " & _
                                                                                "t2.id = t1.decisiondiagram_id " & _
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

#Region "DiseaseIDによる取得"
    Public Function GetByDiseaseID(ByRef pDt As DataTable, _
                                   ByVal pDiseaseID As Integer, _
                                   Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE decisiondiagram_id IN (" & _
                                                                    "SELECT DISTINCT t1.decisiondiagram_id " & _
                                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                                            "INNER JOIN " & DaoDecisionDiagram.TABLE_NAME & " t2 ON " & _
                                                                                "t2.id = t1.decisiondiagram_id " & _
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

#Region "PrimaryKeyによる取得"
    Public Function GetByPK(ByRef pDt As DataTable, _
                            ByVal pDecisiondiagramID As Integer, _
                            ByVal pSequence As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE decisiondiagram_id = @decisiondiagram_id " & _
                                       "AND sequence = @sequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@decisiondiagram_id", SqlDbType.Int, ParameterDirection.Input, pDecisiondiagramID)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pSequence)
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
    ''' <param name="pDecisiondiagramID"></param>
    ''' <param name="pSequence"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Delete(ByVal pDecisiondiagramID As Integer, _
                           ByVal pSequence As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE decisiondiagram_id = @decisiondiagram_id " & _
                                       "AND sequence = @sequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@decisiondiagram_id", SqlDbType.Int, ParameterDirection.Input, pDecisiondiagramID)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pSequence)
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
        Const SQL_QUERY As String = " DELETE " & TABLE_NAME & " " & _
                                    " WHERE decisiondiagram_id IN " & _
                                    " (SELECT id from " & DaoDecisionDiagram.TABLE_NAME & _
                                    " WHERE disease_id= @disease_id) "
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "decisiondiagram_id " & _
                                                                        ",explanation " & _
                                                                        ",sequence " & _
                                                                    ") VALUES (" & _
                                                                         "@decisiondiagram_id " & _
                                                                        ",@explanation " & _
                                                                        ",@sequence " & _
                                                                    ")"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@decisiondiagram_id", SqlDbType.Int, ParameterDirection.Input, pDr.decisiondiagram_id)
            Me.DbManager.SetCmdParameter("@explanation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.explanation)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "更新"
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                            "explanation = @explanation " & _
                                    "WHERE decisiondiagram_id = @decisiondiagram_id " & _
                                      "AND sequence = @sequence"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@explanation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.explanation)
            Me.DbManager.SetCmdParameter("@decisiondiagram_id", SqlDbType.Int, ParameterDirection.Input, pDr.decisiondiagram_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
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
