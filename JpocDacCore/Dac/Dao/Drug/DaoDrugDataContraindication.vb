Public Class DaoDrugDataContraindication
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugDataContraindication"
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
    Public Function GetCountByPK(ByVal pJpc As String) As Integer
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
    Public Function Delete(ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataContraindicationRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "jpc " & _
                                                                        ",contraindication " & _
                                                                        ",notcoadmin " & _
                                                                    ") VALUES (" & _
                                                                         "@jpc " & _
                                                                        ",@contraindication " & _
                                                                        ",@notcoadmin " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            If Not pDr.IsNull("contraindication") Then
                Me.DbManager.SetCmdParameter("@contraindication", SqlDbType.NVarChar, ParameterDirection.Input, pDr.contraindication)
            Else
                Me.DbManager.SetCmdParameter("@contraindication", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("notcoadmin") Then
                Me.DbManager.SetCmdParameter("@notcoadmin", SqlDbType.NVarChar, ParameterDirection.Input, pDr.notcoadmin)
            Else
                Me.DbManager.SetCmdParameter("@notcoadmin", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataContraindicationRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "jpc = @jpc " & _
                                            ",contraindication = @contraindication " & _
                                            ",notcoadmin = @notcoadmin " & _
                                     "WHERE jpc = @jpc_ORG "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            If Not pDr.IsNull("contraindication") Then
                Me.DbManager.SetCmdParameter("@contraindication", SqlDbType.NVarChar, ParameterDirection.Input, pDr.contraindication)
            Else
                Me.DbManager.SetCmdParameter("@contraindication", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("notcoadmin") Then
                Me.DbManager.SetCmdParameter("@notcoadmin", SqlDbType.NVarChar, ParameterDirection.Input, pDr.notcoadmin)
            Else
                Me.DbManager.SetCmdParameter("@notcoadmin", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@jpc_ORG", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Item("jpc", DataRowVersion.Original))
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
