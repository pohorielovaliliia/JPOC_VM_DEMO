Public Class DaoDiseaseSubcategoryMap
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DiseaseSubcategoryMap"
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
    Public Function GetCountByPK(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id"
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

#Region "CategoryIDによる取得"
    Public Function GetCountByCategoryID(ByVal pCategoryID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                    "FROM " & TABLE_NAME & " m " & _
                                    "INNER JOIN " & DaoDiseaseSubcategory.TABLE_NAME & " s " & _
                                    "ON s.id = m.subcategoryId " & _
                                    "WHERE s.categoryID = @categoryID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@categoryID", SqlDbType.Int, ParameterDirection.Input, pCategoryID)
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
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE disease_id = @disease_id"
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

#Region "CategoryIDによる取得"
    Public Function GetByCategoryID(ByRef pDt As DataTable, ByVal pCategoryID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT m.* " & _
                                    "FROM " & TABLE_NAME & " m " & _
                                    "INNER JOIN " & DaoDiseaseSubcategory.TABLE_NAME & " s " & _
                                    "ON s.id = m.subcategoryId " & _
                                    "WHERE s.categoryID = @categoryID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@categoryID", SqlDbType.Int, ParameterDirection.Input, pCategoryID)
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
                            ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "subcategoryId " & _
                                                                        ",disease_id " & _
                                                                        ",sequence " & _
                                                                        ",main_category_flag " & _
                                                                        ",suffix_mark " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@subcategoryId " & _
                                                                        ",@disease_id " & _
                                                                        ",@sequence " & _
                                                                        ",@main_category_flag " & _
                                                                        ",@suffix_mark " & _
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
            Me.DbManager.SetCmdParameter("@subcategoryId", SqlDbType.Int, ParameterDirection.Input, pDr.subcategoryId)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@main_category_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.main_category_flag)
            If Not pDr.IsNull("suffix_mark") Then
                Me.DbManager.SetCmdParameter("@suffix_mark", SqlDbType.NVarChar, ParameterDirection.Input, pDr.suffix_mark)
            Else
                Me.DbManager.SetCmdParameter("@suffix_mark", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim diseaseSubcategoryMapId As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = diseaseSubcategoryMapId
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "subcategoryId = @subcategoryId " & _
                                            ",disease_id = @disease_id " & _
                                            ",sequence = @sequence " & _
                                            ",main_category_flag = @main_category_flag " & _
                                            ",suffix_mark = @suffix_mark " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@subcategoryId", SqlDbType.Int, ParameterDirection.Input, pDr.subcategoryId)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@main_category_flag", SqlDbType.Bit, ParameterDirection.Input, pDr.main_category_flag)
            If Not pDr.IsNull("suffix_mark") Then
                Me.DbManager.SetCmdParameter("@suffix_mark", SqlDbType.NVarChar, ParameterDirection.Input, pDr.suffix_mark)
            Else
                Me.DbManager.SetCmdParameter("@suffix_mark", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Return Me.DbManager.Execute
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

End Class
