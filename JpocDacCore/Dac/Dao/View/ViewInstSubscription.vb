Option Strict On
Option Explicit On

'施設契約一覧（施設情報を含む）
Public Class ViewInstSubscription
    Inherits AbstractTableDao

#Region "定数"
    'Public Const TABLE_NAME As String = "T_JP_Subscription"
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
                                    " FROM " & DaoSubscription.TABLE_NAME & " s " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = s.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 "

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
                                    " FROM " & DaoSubscription.TABLE_NAME & " s " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = s.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE s.active = 1 "
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

#Region "PrimaryKeyによる取得"
    Public Function GetCountByPK(ByVal pSubscriptionID As Integer, _
                                 Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                    " FROM " & DaoSubscription.TABLE_NAME & " s " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = s.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 " & _
                                    " WHERE s.SubscriptionID = @SubscriptionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND s.Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pSubscriptionID)
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
        Const SQL_QUERY As String = "SELECT " & _
                                    " i.InstitutionID " & _
                                    ",s.SubscriptionID" & _
                                    ",CASE WHEN s.Active = 'False' THEN N'無効'" & _
                                    "    WHEN CONVERT(date, s.StartDate) >  CONVERT(date, GETDATE()) THEN N'契約開始待ち'" & _
                                    "    WHEN CONVERT(date, s.EndDate) < CONVERT(date, GETDATE()) THEN N'契約終了'" & _
                                    "    ELSE N'契約中' END AS 'State'" & _
                                    ",CONVERT(VARCHAR, s.StartDate, 111) AS StartDate" & _
                                    ",CONVERT(VARCHAR, s.EndDate, 111) AS EndDate" & _
                                    ",i.InstitutionCode" & _
                                    ",s.Remarks" & _
                                    ",s.Active" & _
                                    ",i.InstitutionNameJP" & _
                                    " FROM " & DaoSubscription.TABLE_NAME & " s " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = s.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE s.Active = 1 "
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

#Region "契約番号による取得"
    Public Function GetBySubscriptionId(ByRef pDt As System.Data.DataTable, _
                                        ByVal pSubScriptionId As Integer, _
                                        Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT " & _
                                    " i.InstitutionID " & _
                                    ",s.SubscriptionID" & _
                                    ",CASE WHEN s.Active = 'False' THEN N'無効'" & _
                                    "    WHEN CONVERT(date, s.StartDate) >  CONVERT(date, GETDATE()) THEN N'契約開始待ち'" & _
                                    "    WHEN CONVERT(date, s.EndDate) < CONVERT(date, GETDATE()) THEN N'契約終了'" & _
                                    "    ELSE N'契約中' END AS 'State'" & _
                                    ",CONVERT(VARCHAR, s.StartDate, 111) AS StartDate" & _
                                    ",CONVERT(VARCHAR, s.EndDate, 111) AS EndDate" & _
                                    ",i.InstitutionCode" & _
                                    ",s.Remarks" & _
                                    ",s.Active" & _
                                    ",i.InstitutionNameJP" & _
                                    " FROM " & DaoSubscription.TABLE_NAME & " s " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = s.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 " & _
                                    " WHERE s.SubscriptionID = @SubscriptionID "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND s.Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pSubScriptionId)
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
