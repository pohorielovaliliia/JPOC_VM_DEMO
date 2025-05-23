Public Class DaoDrugDataNumberOfAdministrations
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugDataNumberOfAdministrations"
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
    Public Function GetCountByPK(ByVal pJpc As String, _
                            ByVal pNumOfAdmin As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND numOfAdmin = @numOfAdmin"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@numOfAdmin", SqlDbType.NVarChar, ParameterDirection.Input, pNumOfAdmin)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "JPCによる取得"
    Public Function GetCountByJpc(ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
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
                            ByVal pJpc As String, _
                            ByVal pNumOfAdmin As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND numOfAdmin = @numOfAdmin"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@numOfAdmin", SqlDbType.NVarChar, ParameterDirection.Input, pNumOfAdmin)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "JPCによる取得"
    Public Function GetByJpc(ByRef pDt As DataTable, _
                             ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
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
    Public Function Delete(ByVal pJpc As String, _
                           ByVal pNumOfAdmin As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND numOfAdmin = @numOfAdmin"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@numOfAdmin", SqlDbType.NVarChar, ParameterDirection.Input, pNumOfAdmin)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataNumberOfAdministrationsRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "jpc " & _
                                                                        ",numOfAdmin " & _
                                                                        ",sort " & _
                                                                    ") VALUES (" & _
                                                                         "@jpc " & _
                                                                        ",@numOfAdmin " & _
                                                                        ",@sort " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            Me.DbManager.SetCmdParameter("@numOfAdmin", SqlDbType.NVarChar, ParameterDirection.Input, pDr.numOfAdmin)
            If Not pDr.IsNull("sort") Then
                Me.DbManager.SetCmdParameter("@sort", SqlDbType.Int, ParameterDirection.Input, pDr.sort)
            Else
                Me.DbManager.SetCmdParameter("@sort", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataNumberOfAdministrationsRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "jpc = @jpc " & _
                                            ",numOfAdmin = @numOfAdmin " & _
                                            ",sort = @sort " & _
                                     "WHERE jpc = @jpc_ORG " & _
                                       "AND numOfAdmin = @numOfAdmin_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            Me.DbManager.SetCmdParameter("@numOfAdmin", SqlDbType.NVarChar, ParameterDirection.Input, pDr.numOfAdmin)
            If Not pDr.IsNull("sort") Then
                Me.DbManager.SetCmdParameter("@sort", SqlDbType.Int, ParameterDirection.Input, pDr.sort)
            Else
                Me.DbManager.SetCmdParameter("@sort", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@jpc_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("jpc", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@numOfAdmin_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("numOfAdmin")))
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
