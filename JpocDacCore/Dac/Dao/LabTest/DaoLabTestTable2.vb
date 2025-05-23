Public Class DaoLabTestTable2
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_LabTestTable2"
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
    Public Function GetCountByPK(ByVal pSrlId As String, _
                                 ByVal pItem1 As String, _
                                 ByVal pItem2 As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id " & _
                                       "AND item1 = @item1 " & _
                                       "AND item2 = @item2"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
            Me.DbManager.SetCmdParameter("@item1", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatItem(pItem1))
            Me.DbManager.SetCmdParameter("@item2", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatItem(pItem2))
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SRL_IDによる取得"
    Public Function GetCountBySrlId(ByVal pSrlId As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id "
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
                            ByVal pSrlId As String, _
                            ByVal pItem1 As String, _
                            ByVal pItem2 As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id " & _
                                       "AND item1 = @item1 " & _
                                       "AND item2 = @item2"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
            Me.DbManager.SetCmdParameter("@item1", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatItem(pItem1))
            Me.DbManager.SetCmdParameter("@item2", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatItem(pItem2))
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SRL_IDによる取得"
    Public Function GetBySrlId(ByRef pDt As DataTable, _
                               ByVal pSrlId As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id "
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
    Public Function Delete(ByVal pSrlId As String, _
                           ByVal pItem1 As String, _
                           ByVal pItem2 As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id " & _
                                       "AND item1 = @item1 " & _
                                       "AND item2 = @item2"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
            Me.DbManager.SetCmdParameter("@item1", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatItem(pItem1))
            Me.DbManager.SetCmdParameter("@item2", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatItem(pItem2))
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
    Public Function Insert(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable2Row) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "srl_id " & _
                                                                        ",item1 " & _
                                                                        ",item2 " & _
                                                                        ",material " & _
                                                                        ",material_additional " & _
                                                                        ",receipt_name " & _
                                                                        ",reference_value " & _
                                                                        ",ID " & _
                                                                    ") VALUES (" & _
                                                                         "@srl_id " & _
                                                                        ",@item1 " & _
                                                                        ",@item2 " & _
                                                                        ",@material " & _
                                                                        ",@material_additional " & _
                                                                        ",@receipt_name " & _
                                                                        ",@reference_value " & _
                                                                        ",@ID " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Me.DbManager.SetCmdParameter("@item1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.item1)
            Me.DbManager.SetCmdParameter("@item2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.item2)
            If Not pDr.IsNull("material") Then
                Me.DbManager.SetCmdParameter("@material", SqlDbType.NText, ParameterDirection.Input, pDr.material)
            Else
                Me.DbManager.SetCmdParameter("@material", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("material_additional") Then
                Me.DbManager.SetCmdParameter("@material_additional", SqlDbType.NText, ParameterDirection.Input, pDr.material_additional)
            Else
                Me.DbManager.SetCmdParameter("@material_additional", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("receipt_name") Then
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, pDr.receipt_name)
            Else
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("reference_value") Then
                Me.DbManager.SetCmdParameter("@reference_value", SqlDbType.NText, ParameterDirection.Input, pDr.reference_value)
            Else
                Me.DbManager.SetCmdParameter("@reference_value", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ID") Then
                Me.DbManager.SetCmdParameter("@ID", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ID)
            Else
                Me.DbManager.SetCmdParameter("@ID", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable2Row) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "srl_id = @srl_id " & _
                                            ",item1 = @item1 " & _
                                            ",item2 = @item2 " & _
                                            ",material = @material " & _
                                            ",material_additional = @material_additional " & _
                                            ",receipt_name = @receipt_name " & _
                                            ",reference_value = @reference_value " & _
                                            ",ID = @ID " & _
                                     "WHERE srl_id = @srl_id_ORG " & _
                                       "AND item1 = @item1_ORG " & _
                                       "AND item2 = @item2_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Me.DbManager.SetCmdParameter("@item1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.item1)
            Me.DbManager.SetCmdParameter("@item2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.item2)
            If Not pDr.IsNull("material") Then
                Me.DbManager.SetCmdParameter("@material", SqlDbType.NText, ParameterDirection.Input, pDr.material)
            Else
                Me.DbManager.SetCmdParameter("@material", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("material_additional") Then
                Me.DbManager.SetCmdParameter("@material_additional", SqlDbType.NText, ParameterDirection.Input, pDr.material_additional)
            Else
                Me.DbManager.SetCmdParameter("@material_additional", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("receipt_name") Then
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, pDr.receipt_name)
            Else
                Me.DbManager.SetCmdParameter("@receipt_name", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("reference_value") Then
                Me.DbManager.SetCmdParameter("@reference_value", SqlDbType.NText, ParameterDirection.Input, pDr.reference_value)
            Else
                Me.DbManager.SetCmdParameter("@reference_value", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ID") Then
                Me.DbManager.SetCmdParameter("@ID", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ID)
            Else
                Me.DbManager.SetCmdParameter("@ID", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@srl_id_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("srl_id", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@item1_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("item1", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@item2_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("item2", DataRowVersion.Original)))
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

#Region "Item整形"
    Public Function FormatItem(ByVal pItem As String) As String
        Return Int32.Parse(pItem).ToString("00")
    End Function
#End Region

End Class
