Public Class DaoDiseaseCategory
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DiseaseCategory"
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

#Region "全てのDiseaseからによる取得"
    ''' <summary>
    ''' 全てのDiseaseからによる取得
    ''' </summary>
    ''' <param name="pOnlyActive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCountByAllDisease(ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = " SELECT COUNT(*) " & _
                                    " FROM " & TABLE_NAME & " main " & _
                                    " WHERE EXISTS(" & _
                                    " SELECT * FROM " & DaoDiseaseSubcategory.TABLE_NAME & " sub " & _
                                    " INNER JOIN " & DaoDiseaseSubcategoryMap.TABLE_NAME & " map " & _
                                    " ON map.subcategoryId = sub.id " & _
                                    " INNER JOIN " & DaoDisease.TABLE_NAME & " disease " & _
                                    " ON disease.id = map.disease_id " & _
                                    " {0} " & _
                                    " WHERE sub.categoryID = main.id) "

        Try
            Dim appendSql As String = String.Empty
            If pOnlyActive Then appendSql &= " AND disease.defunct = 0 AND disease.status = 'A' "
            Me.DbManager.SetSqlCommand(String.Format(SQL_QUERY, appendSql), CommandType.Text)
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

#Region "DiseaseIdによる取得"
    Public Function GetByDiseaseId(ByRef pDt As DataTable, _
                                   ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = " SELECT * " & _
                                    " FROM " & TABLE_NAME & " main " & _
                                    " WHERE EXISTS(" & _
                                    " SELECT * FROM " & DaoDiseaseSubcategory.TABLE_NAME & " sub " & _
                                    " INNER JOIN " & DaoDiseaseSubcategoryMap.TABLE_NAME & " map " & _
                                    " ON map.subcategoryId = sub.id " & _
                                    " AND map.disease_id = @disease_id " & _
                                    " WHERE sub.categoryID = main.id) "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "全てのDiseaseからによる取得"
    ''' <summary>
    ''' 全てのDiseaseからによる取得
    ''' </summary>
    ''' <param name="pDt"></param>
    ''' <param name="pOnlyActive"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByAllDisease(ByRef pDt As DataTable, ByVal pOnlyActive As Boolean, Optional ByVal InstitutionId As String = "") As Integer
        Dim SQL_QUERY As String = " SELECT * " &
                                    " FROM " & TABLE_NAME & " main " &
                                    " WHERE EXISTS(" &
                                    " SELECT * FROM " & DaoDiseaseSubcategory.TABLE_NAME & " sub " &
                                    " INNER JOIN " & DaoDiseaseSubcategoryMap.TABLE_NAME & " map " &
                                    " ON map.subcategoryId = sub.id " &
                                    " INNER JOIN " & DaoDisease.TABLE_NAME & " disease " &
                                    " ON disease.id = map.disease_id " &
                                    " {0} " &
                                    " WHERE sub.categoryID = main.id) "
        If GlobalVariables.isFoundation Then
            SQL_QUERY = " SELECT main.* " &
                                    " FROM " & TABLE_NAME & " main " &
                                    " WHERE EXISTS(" &
                                    " SELECT * FROM " & DaoDiseaseSubcategory.TABLE_NAME & " sub " &
                                    " INNER JOIN " & DaoDiseaseSubcategoryMap.TABLE_NAME & " map " &
                                    " ON map.subcategoryId = sub.id " &
                                    " INNER JOIN " & DaoDisease.TABLE_NAME & " disease " &
                                    " ON disease.id = map.disease_id " &
                                    " {0} " &
                                    " WHERE sub.categoryID = main.id) " &
                                    " AND id IN (SELECT DISTINCT dc.id " &
                                    " FROM " & TABLE_NAME & " dc " &
                                    " INNER JOIN " & DaoSubscriptionDetails.TABLE_NAME & " sd " &
                                    " ON sd.categoryID = dc.id OR sd.categoryid = 0 " &
                                    " INNER JOIN " & DaoSubscription.TABLE_NAME & " sc " &
                                    " ON sc.SubscriptionID = sd.SubscriptionID " &
                                    " AND sc.InstitutionID = '" & InstitutionId & "' " &
                                    " WHERE sd.Active = 1 AND sc.Active = 1 AND sc.EndDate > GETDATE() )"


        End If
        Try
            Dim appendSql As String = String.Empty
            If pOnlyActive Then appendSql &= " And disease.defunct = 0 And disease.status = 'A' "
            Me.DbManager.SetSqlCommand(String.Format(SQL_QUERY, appendSql), CommandType.Text)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_DiseaseCategoryRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                      "id " & _
                                                                     ",category " & _
                                                                     ",sequence " & _
                                                                     "{0}" & _
                                                                   ") VALUES (" & _
                                                                      "id " & _
                                                                     ",@category " & _
                                                                     ",@sequence " & _
                                                                     "{1}" & _
                                                                   ")"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            If Not pDr.IsNull("category") Then
                Me.DbManager.SetCmdParameter("@category", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category)
            Else
                Me.DbManager.SetCmdParameter("@category", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_DiseaseCategoryRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "id = @id " & _
                                            ",category = @category " & _
                                            ",sequence = @sequence " & _
                                    "WHERE id = @id_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("category") Then
                Me.DbManager.SetCmdParameter("@category", SqlDbType.NVarChar, ParameterDirection.Input, pDr.category)
            Else
                Me.DbManager.SetCmdParameter("@category", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@id_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("id", DataRowVersion.Original)))
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
