Public Class DaoDrugDataCustMedList
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugDataCustMedList"
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
    Public Function GetCountByPK(ByVal pListId As Integer, _
                                 ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE listid = @listid " & _
                                       "AND jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@listid", SqlDbType.Int, ParameterDirection.Input, pListId)
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

#Region "Jpcによる取得"
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
                            ByVal pListId As Integer, _
                            ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE listid = @listid " & _
                                       "AND jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@listid", SqlDbType.Int, ParameterDirection.Input, pListId)
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

#Region "Jpcによる取得"
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
    Public Function Delete(ByVal pListId As Integer, _
                           ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE listid = @listid " & _
                                       "AND jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@listid", SqlDbType.Int, ParameterDirection.Input, pListId)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataCustMedListRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "listid " & _
                                                                        ",jpc " & _
                                                                        ",selectionflag " & _
                                                                    ") VALUES (" & _
                                                                         "@listid " & _
                                                                        ",@jpc " & _
                                                                        ",@selectionflag " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@listid", SqlDbType.Int, ParameterDirection.Input, pDr.listid)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            If Not pDr.IsNull("selectionflag") Then
                Me.DbManager.SetCmdParameter("@selectionflag", SqlDbType.Int, ParameterDirection.Input, pDr.selectionflag)
            Else
                Me.DbManager.SetCmdParameter("@selectionflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataCustMedListRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "listid = @listid " & _
                                            ",jpc = @jpc " & _
                                            ",selectionflag = @selectionflag " & _
                                     "WHERE listid = @listid_ORG " & _
                                       "AND jpc = @jpc_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@listid", SqlDbType.Int, ParameterDirection.Input, pDr.listid)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            If Not pDr.IsNull("selectionflag") Then
                Me.DbManager.SetCmdParameter("@selectionflag", SqlDbType.Int, ParameterDirection.Input, pDr.selectionflag)
            Else
                Me.DbManager.SetCmdParameter("@selectionflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@listid_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("listid", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@jpc_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("jpc", DataRowVersion.Original)))
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
