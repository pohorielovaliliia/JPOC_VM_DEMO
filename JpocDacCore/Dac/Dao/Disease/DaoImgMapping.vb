Public Class DaoImgMapping
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_ImgMapping"
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
                                        ByVal pOnlyActive As Boolean) As Integer
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


#Region "ImageIDによる取得"
    Public Function GetByImageID(ByRef pDt As DataTable, _
                                   ByVal pImageId As Integer, _
                                   ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE image_id = @image_id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@image_id", SqlDbType.Int, ParameterDirection.Input, pImageId)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_ImgMappingRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                      "img_type " & _
                                                                     ",img_type_sequence " & _
                                                                     ",parent_id " & _
                                                                     ",image_id " & _
                                                                     ",sequence " & _
                                                                     ",parent_code " & _
                                                                     ",disease_id " & _
                                                                     ",created_by " & _
                                                                     ",created_date " & _
                                                                     ",modified_by " & _
                                                                     ",modified_date " & _
                                                                     ",defunct " & _
                                                                     ",status " & _
                                                                     "{0}" & _
                                                                   ") VALUES (" & _
                                                                      "@img_type " & _
                                                                     ",@img_type_sequence " & _
                                                                     ",@parent_id " & _
                                                                     ",@image_id " & _
                                                                     ",@sequence " & _
                                                                     ",@parent_code " & _
                                                                     ",@disease_id " & _
                                                                     ",@created_by " & _
                                                                     ",@created_date " & _
                                                                     ",@modified_by " & _
                                                                     ",@modified_date " & _
                                                                     ",@defunct " & _
                                                                     ",@status " & _
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
            Me.DbManager.SetCmdParameter("@img_type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.img_type)
            If Not pDr.IsNull("img_type_sequence") Then
                Me.DbManager.SetCmdParameter("@img_type_sequence", SqlDbType.Int, ParameterDirection.Input, pDr.img_type_sequence)
            Else
                Me.DbManager.SetCmdParameter("@img_type_sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, pDr.parent_id)
            Me.DbManager.SetCmdParameter("@image_id", SqlDbType.Int, ParameterDirection.Input, pDr.image_id)
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("parent_code") Then
                Me.DbManager.SetCmdParameter("@parent_code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.parent_code)
            Else
                Me.DbManager.SetCmdParameter("@parent_code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim imgMappingID As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = imgMappingID
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_ImgMappingRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "img_type = @img_type " & _
                                            ",img_type_sequence = @img_type_sequence " & _
                                            ",parent_id = @parent_id " & _
                                            ",image_id = @image_id " & _
                                            ",sequence = @sequence " & _
                                            ",parent_code = @parent_code " & _
                                            ",disease_id = @disease_id " & _
                                            ",created_by = @created_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                            ",defunct = @defunct " & _
                                            ",status = @status " & _
                                    "WHERE id = @id "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@img_type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.img_type)
            If Not pDr.IsNull("img_type_sequence") Then
                Me.DbManager.SetCmdParameter("@img_type_sequence", SqlDbType.Int, ParameterDirection.Input, pDr.img_type_sequence)
            Else
                Me.DbManager.SetCmdParameter("@img_type_sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, pDr.parent_id)
            Me.DbManager.SetCmdParameter("@image_id", SqlDbType.Int, ParameterDirection.Input, pDr.image_id)
            If Not pDr.IsNull("sequence") Then
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Else
                Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("parent_code") Then
                Me.DbManager.SetCmdParameter("@parent_code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.parent_code)
            Else
                Me.DbManager.SetCmdParameter("@parent_code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_id)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
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
