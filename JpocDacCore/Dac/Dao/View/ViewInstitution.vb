Option Strict On
Option Explicit On

'施設一覧
Public Class ViewInstitution
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Institution"
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
            If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
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
    Public Function GetCountByPK(ByVal pInstitutionID As Integer, _
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND defunct = 0 AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
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

        Const SQL_QUERY As String = "SELECT " & _
                                    "InstitutionID, " & _
                                    "InstitutionCode, " & _
                                    "InstitutionNameJP, " & _
                                    "AddressLine1 + NCHAR(10) +  AddressLine2 AS AddressLine," & _
                                    "IPRange," & _
                                   " CASE WHEN Active = 'True'" & _
                                   "   THEN N'はい' " & _
                                   "   ELSE N'いいえ' " & _
                                   " END AS Active " & _
                                   " FROM " & TABLE_NAME & " "
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
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
                            ByVal pInstitutionID As Integer, _
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT " & _
                                    "InstitutionID, " & _
                                    "InstitutionCode, " & _
                                    "InstitutionNameJP, " & _
                                    "AddressLine1 + NCHAR(10) +  AddressLine2 AS AddressLine," & _
                                    "IPRange," & _
                                   " CASE WHEN Active = 'True'" & _
                                   "   THEN N'はい' " & _
                                   "   ELSE N'いいえ' " & _
                                   " END AS Active" & _
                                   "FROM " & TABLE_NAME & "  " & _
                                   "WHERE InstitutionID = @InstitutionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND defunct = 0 AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
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
