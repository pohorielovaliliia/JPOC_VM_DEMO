Public Class DaoLabTestTable3
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_LabTestTable3"
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
    Public Function GetCountByPK(ByVal pId As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE ID = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pId)
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
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
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
                            ByVal pId As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE ID = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.NVarChar, ParameterDirection.Input, pId)
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
    Public Function Delete(ByVal pId As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE ID = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.NVarChar, ParameterDirection.Input, pId)
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
    Public Function Insert(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable3Row) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "srl_id " & _
                                                                        ",necessary_amount " & _
                                                                        ",container_name " & _
                                                                        ",container1_name " & _
                                                                        ",storage_temperature " & _
                                                                        ",others " & _
                                                                        ",ID " & _
                                                                    ") VALUES (" & _
                                                                         "@srl_id " & _
                                                                        ",@necessary_amount " & _
                                                                        ",@container_name " & _
                                                                        ",@container1_name " & _
                                                                        ",@storage_temperature " & _
                                                                        ",@others " & _
                                                                        ",@ID " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            If Not pDr.IsNull("srl_id") Then
                Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Else
                Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("necessary_amount") Then
                Me.DbManager.SetCmdParameter("@necessary_amount", SqlDbType.NVarChar, ParameterDirection.Input, pDr.necessary_amount)
            Else
                Me.DbManager.SetCmdParameter("@necessary_amount", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("container_name") Then
                Me.DbManager.SetCmdParameter("@container_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.container_name)
            Else
                Me.DbManager.SetCmdParameter("@container_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("container1_name") Then
                Me.DbManager.SetCmdParameter("@container1_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.container1_name)
            Else
                Me.DbManager.SetCmdParameter("@container1_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("storage_temperature") Then
                Me.DbManager.SetCmdParameter("@storage_temperature", SqlDbType.NVarChar, ParameterDirection.Input, pDr.storage_temperature)
            Else
                Me.DbManager.SetCmdParameter("@storage_temperature", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("others") Then
                Me.DbManager.SetCmdParameter("@others", SqlDbType.NText, ParameterDirection.Input, pDr.others)
            Else
                Me.DbManager.SetCmdParameter("@others", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@ID", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ID)
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
    Public Function Update(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable3Row) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "ID = @ID " & _
                                            ",srl_id = @srl_id " & _
                                            ",necessary_amount = @necessary_amount " & _
                                            ",container_name = @container_name " & _
                                            ",container1_name = @container1_name " & _
                                            ",storage_temperature = @storage_temperature " & _
                                            ",others = @others " & _
                                            ",ID = @ID " & _
                                     "WHERE ID = @ID_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@ID", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ID)
            If Not pDr.IsNull("srl_id") Then
                Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Else
                Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("necessary_amount") Then
                Me.DbManager.SetCmdParameter("@necessary_amount", SqlDbType.NVarChar, ParameterDirection.Input, pDr.necessary_amount)
            Else
                Me.DbManager.SetCmdParameter("@necessary_amount", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("container_name") Then
                Me.DbManager.SetCmdParameter("@container_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.container_name)
            Else
                Me.DbManager.SetCmdParameter("@container_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("container1_name") Then
                Me.DbManager.SetCmdParameter("@container1_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.container1_name)
            Else
                Me.DbManager.SetCmdParameter("@container1_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("storage_temperature") Then
                Me.DbManager.SetCmdParameter("@storage_temperature", SqlDbType.NVarChar, ParameterDirection.Input, pDr.storage_temperature)
            Else
                Me.DbManager.SetCmdParameter("@storage_temperature", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("others") Then
                Me.DbManager.SetCmdParameter("@others", SqlDbType.NText, ParameterDirection.Input, pDr.others)
            Else
                Me.DbManager.SetCmdParameter("@others", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@ID_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("ID", DataRowVersion.Original)))
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
