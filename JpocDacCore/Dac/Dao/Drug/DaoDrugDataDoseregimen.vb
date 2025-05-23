Public Class DaoDrugDataDoseregimen
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugDataDoseregimen"
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
    Public Function GetCountByPK(ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
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

#Region "JPC、Doseregimenによる取得"
    Public Function GetCountByJpcAndDoseregimen(ByVal pJpc As String, _
                                                ByVal pDoseregimen As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND doseregimen = @doseregimen"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@doseregimen", SqlDbType.NVarChar, ParameterDirection.Input, pDoseregimen)
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
                            ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pId)
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

#Region "JPC、Doseregimenによる取得"
    Public Function GetByJpcAndDoseregimen(ByRef pDt As DataTable, _
                                           ByVal pJpc As String, _
                                           ByVal pDoseregimen As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND doseregimen = @doseregimen"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@doseregimen", SqlDbType.NVarChar, ParameterDirection.Input, pDoseregimen)
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
    Public Function Delete(ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pId)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataDoseregimenRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "jpc " & _
                                                                        ",doseregimen " & _
                                                                        ",doseregimenflag " & _
                                                                        ",rankofdoseregimen " & _
                                                                        ",datasource " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@jpc " & _
                                                                        ",@doseregimen " & _
                                                                        ",@doseregimenflag " & _
                                                                        ",@rankofdoseregimen " & _
                                                                        ",@datasource " & _
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
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            Me.DbManager.SetCmdParameter("@doseregimen", SqlDbType.NVarChar, ParameterDirection.Input, pDr.doseregimen)
            If Not pDr.IsNull("doseregimenflag") Then
                Me.DbManager.SetCmdParameter("@doseregimenflag", SqlDbType.Int, ParameterDirection.Input, pDr.doseregimenflag)
            Else
                Me.DbManager.SetCmdParameter("@doseregimenflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("rankofdoseregimen") Then
                Me.DbManager.SetCmdParameter("@rankofdoseregimen", SqlDbType.Int, ParameterDirection.Input, pDr.rankofdoseregimen)
            Else
                Me.DbManager.SetCmdParameter("@rankofdoseregimen", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("datasource") Then
                Me.DbManager.SetCmdParameter("@datasource", SqlDbType.Int, ParameterDirection.Input, pDr.datasource)
            Else
                Me.DbManager.SetCmdParameter("@datasource", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                pDr.id = Utilities.N0Int(Me.DbManager.ExecuteScalar)
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataDoseregimenRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "jpc = @jpc " & _
                                            ",doseregimen = @doseregimen " & _
                                            ",doseregimenflag = @doseregimenflag " & _
                                            ",rankofdoseregimen = @rankofdoseregimen " & _
                                            ",datasource = @datasource " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            Me.DbManager.SetCmdParameter("@doseregimen", SqlDbType.NVarChar, ParameterDirection.Input, pDr.doseregimen)
            If Not pDr.IsNull("doseregimenflag") Then
                Me.DbManager.SetCmdParameter("@doseregimenflag", SqlDbType.Int, ParameterDirection.Input, pDr.doseregimenflag)
            Else
                Me.DbManager.SetCmdParameter("@doseregimenflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("rankofdoseregimen") Then
                Me.DbManager.SetCmdParameter("@rankofdoseregimen", SqlDbType.Int, ParameterDirection.Input, pDr.rankofdoseregimen)
            Else
                Me.DbManager.SetCmdParameter("@rankofdoseregimen", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("datasource") Then
                Me.DbManager.SetCmdParameter("@datasource", SqlDbType.Int, ParameterDirection.Input, pDr.datasource)
            Else
                Me.DbManager.SetCmdParameter("@datasource", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
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
