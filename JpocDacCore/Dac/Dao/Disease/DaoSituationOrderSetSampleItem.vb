Public Class DaoSituationOrderSetSampleItem
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_SituationOrderSetSampleItem"
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
    Public Function GetCountByPK(ByVal pSampleID As Integer, _
                                 ByVal pOrdersetID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE sample_id = @sample_id " & _
                                       "AND orderset_id = @orderset_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@sample_id", SqlDbType.Int, ParameterDirection.Input, pSampleID)
            Me.DbManager.SetCmdParameter("@orderset_id", SqlDbType.Int, ParameterDirection.Input, pOrdersetID)
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
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " t1 " & _
                                            "INNER JOIN " & DaoSituationOrderSetSample.TABLE_NAME & " t2 ON " & _
                                                "t2.id = t1.sample_id " & _
                                            "INNER JOIN " & DaoSituation.TABLE_NAME & " t3 ON " & _
                                                "t3.id = t2.situation_id " & _
                                     "WHERE t3.disease_id = @disease_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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
                                   ByVal pDiseaseID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " t1 " & _
                                            "INNER JOIN " & DaoSituationOrderSetSample.TABLE_NAME & " t2 ON " & _
                                                "t2.id = t1.sample_id " & _
                                            "INNER JOIN " & DaoSituation.TABLE_NAME & " t3 ON " & _
                                                "t3.id = t2.situation_id " & _
                                     "WHERE t3.disease_id = @disease_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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
                            ByVal pSampleID As Integer, _
                            ByVal pOrdersetID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE sample_id = @sample_id " & _
                                       "AND orderset_id = @orderset_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@sample_id", SqlDbType.Int, ParameterDirection.Input, pSampleID)
            Me.DbManager.SetCmdParameter("@orderset_id", SqlDbType.Int, ParameterDirection.Input, pOrdersetID)
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
    Public Function Delete(ByVal pSampleId As Integer, _
                           ByVal pOrdersetID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE sample_id = @sample_id " & _
                                       "AND orderset_id = @orderset_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@sample_id", SqlDbType.Int, ParameterDirection.Input, pSampleId)
            Me.DbManager.SetCmdParameter("@orderset_id", SqlDbType.Int, ParameterDirection.Input, pOrdersetID)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "sample_id " & _
                                                                        ",orderset_id " & _
                                                                        ",sequence " & _
                                                                        ",sequence_text " & _
                                                                   ") VALUES (" & _
                                                                         "@sample_id " & _
                                                                        ",@orderset_id " & _
                                                                        ",@sequence " & _
                                                                        ",@sequence_text " & _
                                                                   ")"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@sample_id", SqlDbType.Int, ParameterDirection.Input, pDr.sample_id)
            Me.DbManager.SetCmdParameter("@orderset_id", SqlDbType.Int, ParameterDirection.Input, pDr.orderset_id)
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence_text") Then
                Me.DbManager.SetCmdParameter("@sequence_text", SqlDbType.VarChar, ParameterDirection.Input, pDr.sequence_text)
            Else
                Me.DbManager.SetCmdParameter("@sequence_text", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
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

#Region "更新"
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "sequence = @sequence " & _
                                            ",sequence_text = @sequence_text " & _
                                     "WHERE sample_id = @sample_id " & _
                                       "AND orderset_id = @orderset_id "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@sample_id", SqlDbType.Int, ParameterDirection.Input, pDr.sample_id)
            Me.DbManager.SetCmdParameter("@orderset_id", SqlDbType.Int, ParameterDirection.Input, pDr.orderset_id)
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence_text") Then
                Me.DbManager.SetCmdParameter("@sequence_text", SqlDbType.VarChar, ParameterDirection.Input, pDr.sequence_text)
            Else
                Me.DbManager.SetCmdParameter("@sequence_text", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
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
