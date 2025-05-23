Public Class DaoLabTestTable1
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_LabTestTable1"
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
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.Int, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "文字列による取得"
    ''' <summary>文字列による曖昧検索</summary>
    ''' <param name="pText">検索文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCountByText(ByVal pText As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                    "FROM " & TABLE_NAME & " " & _
                                    "WHERE srl_id LIKE @text " & _
                                    "OR NAME LIKE @text"

        Try
            Dim strSql As String = SQL_QUERY
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pText)
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

#Region "文字列による取得"
    ''' <summary>文字列による曖昧検索</summary>
    ''' <param name="pDt"></param>
    ''' <param name="pText">検索文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByText(ByRef pDt As DataTable, _
                              ByVal pText As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                    "FROM " & TABLE_NAME & " " & _
                                    "WHERE srl_id LIKE @text " & _
                                    "OR NAME LIKE @text"

        Try
            Dim strSql As String = SQL_QUERY
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pText)
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
                                     "WHERE srl_id = @1"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@1", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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
    Public Function Insert(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable1Row) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "srl_id " & _
                                                                        ",name " & _
                                                                        ",rank " & _
                                                                    ") VALUES (" & _
                                                                         "@srl_id " & _
                                                                        ",@name " & _
                                                                        ",@rank " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            If Not pDr.IsNull("rank") Then
                Me.DbManager.SetCmdParameter("@rank", SqlDbType.NVarChar, ParameterDirection.Input, pDr.rank)
            Else
                Me.DbManager.SetCmdParameter("@rank", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable1Row) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "srl_id = @srl_id " & _
                                            ",name = @name " & _
                                            ",rank = @rank " & _
                                     "WHERE srl_id = @srl_id_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            If Not pDr.IsNull("rank") Then
                Me.DbManager.SetCmdParameter("@rank", SqlDbType.NVarChar, ParameterDirection.Input, pDr.rank)
            Else
                Me.DbManager.SetCmdParameter("@rank", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
