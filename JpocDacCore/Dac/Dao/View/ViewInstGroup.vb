Option Strict On
Option Explicit On

'施設グループ一覧（施設情報を含む）
Public Class ViewInstGroup
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
                                    " FROM " & DaoInstitutionGroup.TABLE_NAME & " g " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = g.InstitutionID " & _
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
                                    " FROM " & DaoInstitutionGroup.TABLE_NAME & " g " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = g.InstitutionID " & _
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
    Public Function GetCountByPK(ByVal pGrouoId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                    " FROM " & DaoInstitutionGroup.TABLE_NAME & " g " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = g.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 " & _
                                    " WHERE g.SubscriptionID = @GrouoId"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("GrouoId", SqlDbType.Int, ParameterDirection.Input, pGrouoId)
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
    Public Overrides Function GetAll(ByRef pDt As System.Data.DataTable, ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT " & _
                                    " g.InstitutionID" & _
                                    " ,g.GroupID" & _
                                    " ,g.GroupNameJP" & _
                                    " ,i.InstitutionNameJP" & _
                                    " ,InstitutionCode" & _
                                    " FROM " & DaoInstitutionGroup.TABLE_NAME & " g " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = g.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 "
        Try
            Dim strSql As String = SQL_QUERY
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

#Region "グループ番号による取得"
    Public Function GetByGroupId(ByRef pDt As System.Data.DataTable, _
                                 ByVal pGroupId As Integer) As Integer

        Const SQL_QUERY As String = "SELECT " & _
                                    " g.InstitutionID" & _
                                    " ,g.GroupID" & _
                                    " ,g.GroupNameJP" & _
                                    " ,i.InstitutionNameJP" & _
                                    " ,InstitutionCode" & _
                                    " FROM " & DaoInstitutionGroup.TABLE_NAME & " g " & _
                                    " INNER JOIN " & DaoInstitution.TABLE_NAME & " i " & _
                                    " ON i.InstitutionID = g.InstitutionID " & _
                                    " AND i.defunct = 0 " & _
                                    " AND i.active = 1 " & _
                                    " WHERE g.GroupId = @GroupId "
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@GroupId", SqlDbType.Int, ParameterDirection.Input, pGroupId)
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
