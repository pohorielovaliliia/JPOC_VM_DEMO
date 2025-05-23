Public Class DaoJbOrganizationType
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_JB_OrganizationType"
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
    Public Function GetCountByPK(ByVal pOrderSequence As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE OrderSequence = @OrderSequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@OrderSequence", SqlDbType.Int, ParameterDirection.Input, pOrderSequence)
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
                            ByVal pOrderSequence As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE OrderSequence = @OrderSequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@OrderSequence", SqlDbType.Int, ParameterDirection.Input, pOrderSequence)
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
    Public Function Delete(ByVal pOrderSequence As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE OrderSequence = @OrderSequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@OrderSequence", SqlDbType.Int, ParameterDirection.Input, pOrderSequence)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_JB_OrganizationTypeRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "OrganizationType " & _
                                                                        ",OrganizationTypeJP " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@OrganizationType " & _
                                                                        ",@OrganizationTypeJP " & _
                                                                        "{1}" & _
                                                                    ");"

        Try
            Dim strSQL As String = String.Empty
            If pKeepIdValue Then
                strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) & _
                         String.Format(SQL_QUERY, ",OrderSequence ", ",@OrderSequence ") & _
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@OrganizationType", SqlDbType.NVarChar, ParameterDirection.Input, pDr.OrganizationType)
            Me.DbManager.SetCmdParameter("@OrganizationTypeJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.OrganizationTypeJP)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@OrderSequence", SqlDbType.Int, ParameterDirection.Input, pDr.OrderSequence)
                ret = Me.DbManager.Execute()
            Else
                pDr.OrderSequence = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                If pDr.OrderSequence > 0 Then ret = 1
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_JB_OrganizationTypeRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "OrganizationType = @OrganizationType " & _
                                            ",OrganizationTypeJP = @OrganizationTypeJP " & _
                                     "WHERE OrderSequence = @OrderSequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@OrganizationType", SqlDbType.NVarChar, ParameterDirection.Input, pDr.OrganizationType)
            Me.DbManager.SetCmdParameter("@OrganizationTypeJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.OrganizationTypeJP)
            Me.DbManager.SetCmdParameter("@OrderSequence", SqlDbType.Int, ParameterDirection.Input, pDr.OrderSequence)
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
