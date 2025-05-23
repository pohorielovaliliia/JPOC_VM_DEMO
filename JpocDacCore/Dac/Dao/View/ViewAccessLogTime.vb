'ログイン回数
Public Class ViewAccessLogTime
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "V_JP_AccessLogTime"
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
    Public Overloads Function GetCount(ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                              "FROM " & TABLE_NAME

        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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
    Public Overrides Function GetAll(ByRef pDt As System.Data.DataTable, _
                                     ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                              "FROM " & TABLE_NAME
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
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

#Region "日付指定による取得"
    Public Function GetByAccessDate(ByRef pDt As System.Data.DataTable,
                                    ByVal pStartDate As DateTime,
                                    ByVal pEndDate As DateTime) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                    " FROM " & TABLE_NAME & " " &
                                    " WHERE CONVERT(DATETIME, access_date) BETWEEN @start_date AND @end_date "

        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@start_date", SqlDbType.DateTime, ParameterDirection.Input, pStartDate)
            Me.DbManager.SetCmdParameter("@end_date", SqlDbType.DateTime, ParameterDirection.Input, pEndDate)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function

    Public Function GetByAccessDateByIP(ByRef pDt As System.Data.DataTable,
                                    ByVal pStartDate As DateTime,
                                    ByVal pEndDate As DateTime) As Integer
        Const SQL_QUERY As String = "SELECT * " &
                                    " FROM V_JP_AccessLogTimeByIP " &
                                    " WHERE CONVERT(DATETIME, access_date) BETWEEN @start_date AND @end_date "

        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@start_date", SqlDbType.DateTime, ParameterDirection.Input, pStartDate)
            Me.DbManager.SetCmdParameter("@end_date", SqlDbType.DateTime, ParameterDirection.Input, pEndDate)

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

End Class
