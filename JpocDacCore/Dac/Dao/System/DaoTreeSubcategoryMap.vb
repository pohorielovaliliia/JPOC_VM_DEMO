Option Strict On
Option Explicit On

Public Class DaoTreeSubcategoryMap
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_TreeSubcategoryMap"
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
    Public Function GetCountByPK(ByVal pID As Integer, _
                                 ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "CategoryIdによる取得"
    Public Function GetCountByCategoryId(ByVal pCategoryId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                     " FROM " & TABLE_NAME & " map " & _
                                     " INNER JOIN " & DaoTreeSubcategory.TABLE_NAME & " sub " & _
                                     " ON sub.id = map.subcategoryId" & _
                                     " INNER JOIN " & DaoTreeCategory.TABLE_NAME & " main " & _
                                     " ON main.id = sub.categoryID" & _
                                     " WHERE main.id = @categoryID"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("categoryID", SqlDbType.Int, ParameterDirection.Input, pCategoryId)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SubCategoryIdによる取得"
    Public Function GetCountBySubcategoryId(ByVal pSubcategoryId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                     " FROM " & TABLE_NAME & " " & _
                                     " WHERE subcategoryId = @subcategoryId"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("subcategoryId", SqlDbType.Int, ParameterDirection.Input, pSubcategoryId)
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
        Const SQL_QUERY As String = " SELECT * " & _
                                    " FROM " & TABLE_NAME & " " & _
                                    " WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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

#Region "CategoryIdによる取得"
    Public Function GetByCategoryId(ByRef pDt As DataTable, _
                                    ByVal pCategoryId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT map.* " & _
                                     " FROM " & TABLE_NAME & " map " & _
                                     " INNER JOIN " & DaoTreeSubcategory.TABLE_NAME & " sub " & _
                                     " ON sub.id = map.subcategoryId" & _
                                     " INNER JOIN " & DaoTreeCategory.TABLE_NAME & " main " & _
                                     " ON main.id = sub.categoryID" & _
                                     " WHERE main.id = @categoryID"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("categoryID", SqlDbType.Int, ParameterDirection.Input, pCategoryId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SubCategoryIdによる取得"
    Public Function GetBySubcategoryId(ByRef pDt As DataTable, _
                                    ByVal pSubcategoryId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                     " FROM " & TABLE_NAME & " " & _
                                     " WHERE subcategoryId = @subcategoryId"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("subcategoryId", SqlDbType.Int, ParameterDirection.Input, pSubcategoryId)
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
