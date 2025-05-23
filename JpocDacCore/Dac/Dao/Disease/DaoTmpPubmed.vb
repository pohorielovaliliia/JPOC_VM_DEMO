Public Class DaoTmpPubmed
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_TmpPubmed"
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
            'If pOnlyActive Then strSql &= " WHERE defunct = 0 "
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
    Public Function GetCountByPK(ByVal pPubmedId As String, _
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE pubmed_id = @pubmed_id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND defunct = 0 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@pubmed_id", SqlDbType.NVarChar, ParameterDirection.Input, pPubmedId)
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
            'If pOnlyActive Then strSql &= " WHERE defunct = 0 "
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
                            ByVal pPubmedId As String, _
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE pubmed_id = @pubmed_id"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND defunct = 0"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@pubmed_id", SqlDbType.NVarChar, ParameterDirection.Input, pPubmedId)
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
    Public Function Delete(ByVal pPubmedId As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE pubmed_id = @pubmed_id"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@pubmed_id", SqlDbType.NVarChar, ParameterDirection.Input, pPubmedId)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_TmpPubmedRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                     " pubmed_id " & _
                                                                     ",title " & _
                                                                     ",authors " & _
                                                                     ",citation " & _
                                                                     ",abstract " & _
                                                                     ",text_link " & _
                                                                     ",publish_date " & _
                                                                   ") VALUES ( " & _
                                                                     " @pubmed_id " & _
                                                                     ",@title " & _
                                                                     ",@authors " & _
                                                                     ",@citation " & _
                                                                     ",@abstract " & _
                                                                     ",@text_link " & _
                                                                     ",@publish_date " & _
                                                                   "); "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@pubmed_id", SqlDbType.VarChar, ParameterDirection.Input, pDr.pubmed_id)
            
            If Not pDr.IsNull("title") Then
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Else
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("authors") Then
                Me.DbManager.SetCmdParameter("@authors", SqlDbType.NVarChar, ParameterDirection.Input, pDr.authors)
            Else
                Me.DbManager.SetCmdParameter("@authors", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("citation") Then
                Me.DbManager.SetCmdParameter("@citation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.citation)
            Else
                Me.DbManager.SetCmdParameter("@citation", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("abstract") Then
                Me.DbManager.SetCmdParameter("@abstract", SqlDbType.NVarChar, ParameterDirection.Input, pDr.abstract)
            Else
                Me.DbManager.SetCmdParameter("@abstract", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("text_link") Then
                Me.DbManager.SetCmdParameter("@text_link", SqlDbType.NVarChar, ParameterDirection.Input, pDr.text_link)
            Else
                Me.DbManager.SetCmdParameter("@text_link", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@publish_date", SqlDbType.Date, ParameterDirection.Input, pDr.publish_date)

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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_TmpPubmedRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "title = @title " & _
                                            ",authors = @authors " & _
                                            ",citation = @citation " & _
                                            ",abstract = @abstract " & _
                                            ",text_link = @text_link " & _
                                            ",publish_date = @publish_date " & _
                                    "WHERE pubmed_id = @pubmed_id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@pubmed_id", SqlDbType.Int, ParameterDirection.Input, pDr.pubmed_id)
            If Not pDr.IsNull("title") Then
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Else
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("authors") Then
                Me.DbManager.SetCmdParameter("@authors", SqlDbType.NVarChar, ParameterDirection.Input, pDr.authors)
            Else
                Me.DbManager.SetCmdParameter("@authors", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("citation") Then
                Me.DbManager.SetCmdParameter("@citation", SqlDbType.NVarChar, ParameterDirection.Input, pDr.citation)
            Else
                Me.DbManager.SetCmdParameter("@citation", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("abstract") Then
                Me.DbManager.SetCmdParameter("@abstract", SqlDbType.NVarChar, ParameterDirection.Input, pDr.abstract)
            Else
                Me.DbManager.SetCmdParameter("@abstract", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("text_link") Then
                Me.DbManager.SetCmdParameter("@text_link", SqlDbType.NVarChar, ParameterDirection.Input, pDr.text_link)
            Else
                Me.DbManager.SetCmdParameter("@text_link", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@publish_date", SqlDbType.Date, ParameterDirection.Input, pDr.publish_date)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

End Class
