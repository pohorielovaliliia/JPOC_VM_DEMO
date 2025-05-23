Public Class DaoDiseaseSpecialty
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DiseaseSpecialty"
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
    Public Function GetCountByPK(ByVal pDiseaseId As Integer, _
                                 ByVal pSpecialtyId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id " & _
                                       "AND specialty_id = @specialty_id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
            Me.DbManager.SetCmdParameter("@specialty_id", SqlDbType.Int, ParameterDirection.Input, pSpecialtyId)
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

#Region "PrimaryKeyによる取得"
    Public Function GetByPK(ByRef pDt As DataTable, _
                            ByVal pDiseaseId As Integer, _
                            ByVal pSpecialtyId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id " & _
                                       "AND specialty_id = @specialty_id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
            Me.DbManager.SetCmdParameter("@specialty_id", SqlDbType.Int, ParameterDirection.Input, pSpecialtyId)
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
    Public Function Delete(ByVal pDiseaseId As Integer, _
                           ByVal pSpecialtyId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id " & _
                                       "AND specialty_id = @specialty_id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
            Me.DbManager.SetCmdParameter("@specialty_id", SqlDbType.Int, ParameterDirection.Input, pSpecialtyId)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_DiseaseSpecialtyRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                      "disease_id " & _
                                                                     ",specialty_id " & _
                                                                     ",sequence " & _
                                                                   ") VALUES (" & _
                                                                      "@disease_id " & _
                                                                     ",@specialty_id " & _
                                                                     ",@sequence " & _
                                                                   ")"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@specialty_id", SqlDbType.Int, ParameterDirection.Input, pDr.specialty_id)
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_DiseaseSpecialtyRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "disease_id = @disease_id " & _
                                            ",specialty_id = @specialty_id " & _
                                            ",sequence = @sequence " & _
                                     "WHERE disease_id = @disease_id_ORG " & _
                                       "AND specialty_id = @specialty_id_ORG "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@specialty_id", SqlDbType.Int, ParameterDirection.Input, pDr.specialty_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@disease_id_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("disease_id", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@specialty_id_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("specialty_id", DataRowVersion.Original)))
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
