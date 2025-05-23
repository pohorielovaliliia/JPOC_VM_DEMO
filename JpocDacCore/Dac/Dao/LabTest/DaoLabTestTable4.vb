Public Class DaoLabTestTable4
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_LabTestTable4"
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
    Public Function GetCountByPK(ByVal pSrlId As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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
                            ByVal pSrlId As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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
    Public Function Delete(ByVal pSrlId As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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
    Public Function Insert(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable4Row) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "srl_id " & _
                                                                        ",receipt_name " & _
                                                                        ",applied_material " & _
                                                                        ",determined_material " & _
                                                                        ",distinguish " & _
                                                                        ",inspection_methods " & _
                                                                        ",num_of_days_used " & _
                                                                        ",type1 " & _
                                                                        ",note_point " & _
                                                                        ",additional_text " & _
                                                                    ") VALUES (" & _
                                                                         "@srl_id " & _
                                                                        ",@receipt_name " & _
                                                                        ",@applied_material " & _
                                                                        ",@determined_material " & _
                                                                        ",@distinguish " & _
                                                                        ",@inspection_methods " & _
                                                                        ",@num_of_days_used " & _
                                                                        ",@type1 " & _
                                                                        ",@note_point " & _
                                                                        ",@additional_text " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            If Not pDr.IsNull("receipt_name") Then
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, pDr.receipt_name)
            Else
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("applied_material") Then
                Me.DbManager.SetCmdParameter("@applied_material", SqlDbType.NText, ParameterDirection.Input, pDr.applied_material)
            Else
                Me.DbManager.SetCmdParameter("@applied_material", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("determined_material") Then
                Me.DbManager.SetCmdParameter("@determined_material", SqlDbType.NText, ParameterDirection.Input, pDr.determined_material)
            Else
                Me.DbManager.SetCmdParameter("@determined_material", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("distinguish") Then
                Me.DbManager.SetCmdParameter("@distinguish", SqlDbType.NText, ParameterDirection.Input, pDr.distinguish)
            Else
                Me.DbManager.SetCmdParameter("@distinguish", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("inspection_methods") Then
                Me.DbManager.SetCmdParameter("@inspection_methods", SqlDbType.NText, ParameterDirection.Input, pDr.inspection_methods)
            Else
                Me.DbManager.SetCmdParameter("@inspection_methods", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("num_of_days_used") Then
                Me.DbManager.SetCmdParameter("@num_of_days_used", SqlDbType.NText, ParameterDirection.Input, pDr.num_of_days_used)
            Else
                Me.DbManager.SetCmdParameter("@num_of_days_used", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("type1") Then
                Me.DbManager.SetCmdParameter("@type1", SqlDbType.NText, ParameterDirection.Input, pDr.type1)
            Else
                Me.DbManager.SetCmdParameter("@type1", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("note_point") Then
                Me.DbManager.SetCmdParameter("@note_point", SqlDbType.NText, ParameterDirection.Input, pDr.note_point)
            Else
                Me.DbManager.SetCmdParameter("@note_point", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("additional_text") Then
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, pDr.additional_text)
            Else
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable4Row) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "srl_id = @srl_id " & _
                                            ",receipt_name = @receipt_name " & _
                                            ",applied_material = @applied_material " & _
                                            ",determined_material = @determined_material " & _
                                            ",distinguish = @distinguish " & _
                                            ",inspection_methods = @inspection_methods " & _
                                            ",num_of_days_used = @num_of_days_used " & _
                                            ",type1 = @type1 " & _
                                            ",note_point = @note_point " & _
                                            ",additional_text = @additional_text " & _
                                     "WHERE srl_id = @srl_id_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            If Not pDr.IsNull("receipt_name") Then
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, pDr.receipt_name)
            Else
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("applied_material") Then
                Me.DbManager.SetCmdParameter("@applied_material", SqlDbType.NText, ParameterDirection.Input, pDr.applied_material)
            Else
                Me.DbManager.SetCmdParameter("@applied_material", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("determined_material") Then
                Me.DbManager.SetCmdParameter("@determined_material", SqlDbType.NText, ParameterDirection.Input, pDr.determined_material)
            Else
                Me.DbManager.SetCmdParameter("@determined_material", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("distinguish") Then
                Me.DbManager.SetCmdParameter("@distinguish", SqlDbType.NText, ParameterDirection.Input, pDr.distinguish)
            Else
                Me.DbManager.SetCmdParameter("@distinguish", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("inspection_methods") Then
                Me.DbManager.SetCmdParameter("@inspection_methods", SqlDbType.NText, ParameterDirection.Input, pDr.inspection_methods)
            Else
                Me.DbManager.SetCmdParameter("@inspection_methods", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("num_of_days_used") Then
                Me.DbManager.SetCmdParameter("@num_of_days_used", SqlDbType.NText, ParameterDirection.Input, pDr.num_of_days_used)
            Else
                Me.DbManager.SetCmdParameter("@num_of_days_used", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("type1") Then
                Me.DbManager.SetCmdParameter("@type1", SqlDbType.NText, ParameterDirection.Input, pDr.type1)
            Else
                Me.DbManager.SetCmdParameter("@type1", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("note_point") Then
                Me.DbManager.SetCmdParameter("@note_point", SqlDbType.NText, ParameterDirection.Input, pDr.note_point)
            Else
                Me.DbManager.SetCmdParameter("@note_point", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("additional_text") Then
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, pDr.additional_text)
            Else
                Me.DbManager.SetCmdParameter("@additional_text", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@srl_id_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("srl_id", DataRowVersion.Original)))
            Return Me.DbManager.Execute
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SrlID整形"
    Public Function FormatSrlId(ByVal pSrlId As String) As String
        Return Int32.Parse(pSrlId).ToString("0000")
    End Function
#End Region

End Class
