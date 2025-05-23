Public Class DaoShirobon
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Shirobon"
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
    Public Function GetCountByPK(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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

#Region "UniqueKeyによる取得"
    ''' <summary>
    ''' T_JP_Shirobon.dir_path, T_JP_Shirobon.file_nameによる取得
    ''' </summary>
    ''' <param name="pDirPath"></param>
    ''' <param name="pFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCountByDirPathAndFileName(ByVal pDirPath As String, ByVal pFileName As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE dir_path = @dir_path " & _
                                     "AND file_name = @file_name "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@dir_path", SqlDbType.Int, ParameterDirection.Input, pDirPath)
            Me.DbManager.SetCmdParameter("@file_name", SqlDbType.Int, ParameterDirection.Input, pFileName)
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
    ''' <summary>文字列による曖昧検索
    ''' </summary>
    ''' <param name="pText">検索文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCountByText(ByVal pText As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE title LIKE @text " & _
                                     "OR body LIKE @text "
        Try
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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
                            ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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

#Region "UniqueKeyによる取得"
    ''' <summary>
    ''' T_JP_Shirobon.dir_path, T_JP_Shirobon.file_nameによる取得
    ''' </summary>
    ''' <param name="pDt"></param>
    ''' <param name="pDirPath"></param>
    ''' <param name="pFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByDirPathAndFileName(ByRef pDt As DataTable, _
                                            ByVal pDirPath As String, _
                                            ByVal pFileName As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE dir_path = @dir_path " & _
                                     "AND file_name = @file_name "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@dir_path", SqlDbType.VarChar, ParameterDirection.Input, pDirPath)
            Me.DbManager.SetCmdParameter("@file_name", SqlDbType.VarChar, ParameterDirection.Input, pFileName)
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
    ''' <summary>文字列による曖昧検索
    ''' </summary>
    ''' <param name="pDt"></param>
    ''' <param name="pText">検索文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetByText(ByRef pDt As DataTable, ByVal pText As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE title LIKE @text " & _
                                     "OR body LIKE @text "
        Try
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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
    Public Function Delete(ByVal pId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pId)
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
    Public Function Insert(ByRef pDr As DS_SYSTEM.T_JP_ShirobonRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "parent_id " & _
                                                                        ",dir_path " & _
                                                                        ",file_name " & _
                                                                        ",chapter " & _
                                                                        ",section " & _
                                                                        ",subsection " & _
                                                                        ",paragraph " & _
                                                                        ",item " & _
                                                                        ",division " & _
                                                                        ",title " & _
                                                                        ",head " & _
                                                                        ",body " & _
                                                                        ",contents " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@parent_id " & _
                                                                        ",@dir_path " & _
                                                                        ",@file_name " & _
                                                                        ",@chapter " & _
                                                                        ",@section " & _
                                                                        ",@subsection " & _
                                                                        ",@paragraph " & _
                                                                        ",@item " & _
                                                                        ",@division " & _
                                                                        ",@title " & _
                                                                        ",@head " & _
                                                                        ",@body " & _
                                                                        ",@contents " & _
                                                                        "{1}" & _
                                                                    ");"

        Try
            Dim strSQL As String = String.Empty
            If pKeepIdValue Then
                strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) & _
                         String.Format(SQL_QUERY, ",id ", ",@id ") & _
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            If Not pDr.IsNull("parent_id") Then
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, pDr.parent_id)
            Else
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("dir_path") Then
                Me.DbManager.SetCmdParameter("@dir_path", SqlDbType.VarChar, ParameterDirection.Input, pDr.dir_path)
            Else
                Me.DbManager.SetCmdParameter("@dir_path", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_name") Then
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.VarChar, ParameterDirection.Input, pDr.file_name)
            Else
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("chapter") Then
                Me.DbManager.SetCmdParameter("@chapter", SqlDbType.VarChar, ParameterDirection.Input, pDr.chapter)
            Else
                Me.DbManager.SetCmdParameter("@chapter", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("section") Then
                Me.DbManager.SetCmdParameter("@section", SqlDbType.VarChar, ParameterDirection.Input, pDr.section)
            Else
                Me.DbManager.SetCmdParameter("@section", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("subsection") Then
                Me.DbManager.SetCmdParameter("@subsection", SqlDbType.VarChar, ParameterDirection.Input, pDr.subsection)
            Else
                Me.DbManager.SetCmdParameter("@subsection", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("paragraph") Then
                Me.DbManager.SetCmdParameter("@paragraph", SqlDbType.VarChar, ParameterDirection.Input, pDr.paragraph)
            Else
                Me.DbManager.SetCmdParameter("@paragraph", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("item") Then
                Me.DbManager.SetCmdParameter("@item", SqlDbType.VarChar, ParameterDirection.Input, pDr.item)
            Else
                Me.DbManager.SetCmdParameter("@item", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("division") Then
                Me.DbManager.SetCmdParameter("@division", SqlDbType.VarChar, ParameterDirection.Input, pDr.division)
            Else
                Me.DbManager.SetCmdParameter("@division", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("title") Then
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Else
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("head") Then
                Me.DbManager.SetCmdParameter("@head", SqlDbType.NVarChar, ParameterDirection.Input, pDr.head)
            Else
                Me.DbManager.SetCmdParameter("@head", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("body") Then
                Me.DbManager.SetCmdParameter("@body", SqlDbType.NVarChar, ParameterDirection.Input, pDr.body)
            Else
                Me.DbManager.SetCmdParameter("@body", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("contents") Then
                Me.DbManager.SetCmdParameter("@contents", SqlDbType.NVarChar, ParameterDirection.Input, pDr.contents)
            Else
                Me.DbManager.SetCmdParameter("@contents", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                pDr.id = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                If pDr.id > 0 Then ret = 1
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
    Public Function Update(ByRef pDr As DS_SYSTEM.T_JP_ShirobonRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "parent_id = @parent_id " & _
                                            ",dir_path = @dir_path " & _
                                            ",file_name = @file_name " & _
                                            ",chapter = @chapter " & _
                                            ",section = @section " & _
                                            ",subsection = @subsection " & _
                                            ",paragraph = @paragraph " & _
                                            ",item = @item " & _
                                            ",division = @division " & _
                                            ",title = @title " & _
                                            ",head = @head " & _
                                            ",body = @body " & _
                                            ",contents = @contents " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            If Not pDr.IsNull("parent_id") Then
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, pDr.parent_id)
            Else
                Me.DbManager.SetCmdParameter("@parent_id", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("dir_path") Then
                Me.DbManager.SetCmdParameter("@dir_path", SqlDbType.VarChar, ParameterDirection.Input, pDr.dir_path)
            Else
                Me.DbManager.SetCmdParameter("@dir_path", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("file_name") Then
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.VarChar, ParameterDirection.Input, pDr.file_name)
            Else
                Me.DbManager.SetCmdParameter("@file_name", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("chapter") Then
                Me.DbManager.SetCmdParameter("@chapter", SqlDbType.VarChar, ParameterDirection.Input, pDr.chapter)
            Else
                Me.DbManager.SetCmdParameter("@chapter", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("section") Then
                Me.DbManager.SetCmdParameter("@section", SqlDbType.VarChar, ParameterDirection.Input, pDr.section)
            Else
                Me.DbManager.SetCmdParameter("@section", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("subsection") Then
                Me.DbManager.SetCmdParameter("@subsection", SqlDbType.VarChar, ParameterDirection.Input, pDr.subsection)
            Else
                Me.DbManager.SetCmdParameter("@subsection", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("paragraph") Then
                Me.DbManager.SetCmdParameter("@paragraph", SqlDbType.VarChar, ParameterDirection.Input, pDr.paragraph)
            Else
                Me.DbManager.SetCmdParameter("@paragraph", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("item") Then
                Me.DbManager.SetCmdParameter("@item", SqlDbType.VarChar, ParameterDirection.Input, pDr.item)
            Else
                Me.DbManager.SetCmdParameter("@item", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("division") Then
                Me.DbManager.SetCmdParameter("@division", SqlDbType.VarChar, ParameterDirection.Input, pDr.division)
            Else
                Me.DbManager.SetCmdParameter("@division", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("title") Then
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Else
                Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("head") Then
                Me.DbManager.SetCmdParameter("@head", SqlDbType.NVarChar, ParameterDirection.Input, pDr.head)
            Else
                Me.DbManager.SetCmdParameter("@head", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("body") Then
                Me.DbManager.SetCmdParameter("@body", SqlDbType.NVarChar, ParameterDirection.Input, pDr.body)
            Else
                Me.DbManager.SetCmdParameter("@body", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("contents") Then
                Me.DbManager.SetCmdParameter("@contents", SqlDbType.NVarChar, ParameterDirection.Input, pDr.contents)
            Else
                Me.DbManager.SetCmdParameter("@contents", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
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
