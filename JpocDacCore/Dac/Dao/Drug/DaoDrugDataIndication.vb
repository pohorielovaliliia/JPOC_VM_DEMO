Public Class DaoDrugDataIndication
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugDataIndication"
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
    Public Function GetCountByPK(ByVal pType As String, _
                                 ByVal pCategory As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE type = @type " & _
                                       "AND category = @category"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pType)
            Me.DbManager.SetCmdParameter("@category", SqlDbType.NChar, ParameterDirection.Input, pCategory)
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
                            ByVal pType As String, _
                            ByVal pCategory As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE type = @type " & _
                                       "AND category = @category"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pType)
            Me.DbManager.SetCmdParameter("@category", SqlDbType.NChar, ParameterDirection.Input, pCategory)
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
    Public Function Delete(ByVal pType As String, _
                           ByVal pCategory As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE type = @type " & _
                                       "AND category = @category"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pType)
            Me.DbManager.SetCmdParameter("@category", SqlDbType.NChar, ParameterDirection.Input, pCategory)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataIndicationRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "type " & _
                                                                        ",category " & _
                                                                        ",text " & _
                                                                        ",status " & _
                                                                    ") VALUES (" & _
                                                                         "@type " & _
                                                                        ",@category " & _
                                                                        ",@text " & _
                                                                        ",@status " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.type)
            Me.DbManager.SetCmdParameter("@category", SqlDbType.NChar, ParameterDirection.Input, pDr.category)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pDr.text)
            If Not pDr.IsNull("status") Then
                Me.DbManager.SetCmdParameter("@status", SqlDbType.NVarChar, ParameterDirection.Input, pDr.status)
            Else
                Me.DbManager.SetCmdParameter("@status", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataIndicationRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "type = @type " & _
                                            ",category = @category " & _
                                            ",text = @text " & _
                                            ",status = @status " & _
                                     "WHERE type = @type_ORG " & _
                                       "AND category = @category_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.type)
            Me.DbManager.SetCmdParameter("@category", SqlDbType.NChar, ParameterDirection.Input, pDr.category)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pDr.text)
            If Not pDr.IsNull("status") Then
                Me.DbManager.SetCmdParameter("@status", SqlDbType.NVarChar, ParameterDirection.Input, pDr.status)
            Else
                Me.DbManager.SetCmdParameter("@status", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@type_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("type", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@category_ORG", SqlDbType.NChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("category", DataRowVersion.Original)))
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
