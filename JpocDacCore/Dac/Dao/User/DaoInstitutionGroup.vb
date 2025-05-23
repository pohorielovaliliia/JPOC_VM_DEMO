Public Class DaoInstitutionGroup
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Institution_Group"
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
    Public Function GetCountByPK(ByVal pGroupID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE GroupID = @GroupID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, pGroupID)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "施設IDによる取得"
    Public Function GetCountByInstitutionId(ByVal pInstId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstId)
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
                            ByVal pGroupID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE GroupID = @GroupID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, pGroupID)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "施設IDによる取得"
    Public Function GetByInstitutionId(ByRef pDt As DataTable, _
                                       ByVal pInstId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstId)
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
    Public Function Delete(ByVal pGroupID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE GroupID = @GroupID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, pGroupID)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_Institution_GroupRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "InstitutionID " & _
                                                                        ",GroupNameEN " & _
                                                                        ",GroupNameJP " & _
                                                                        ",UserIDs " & _
                                                                        ",created_by " & _
                                                                        ",modified_by " & _
                                                                        ",checkout_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_date " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@InstitutionID " & _
                                                                        ",@GroupNameEN " & _
                                                                        ",@GroupNameJP " & _
                                                                        ",@UserIDs " & _
                                                                        ",@created_by " & _
                                                                        ",@modified_by " & _
                                                                        ",@checkout_by " & _
                                                                        ",@created_date " & _
                                                                        ",@modified_date " & _
                                                                        "{1}" & _
                                                                    ");"

        Try
            Dim strSQL As String = String.Empty
            If pKeepIdValue Then
                strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) & _
                         String.Format(SQL_QUERY, ",GroupID ", ",@GroupID ") & _
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@GroupNameEN", SqlDbType.NVarChar, ParameterDirection.Input, pDr.GroupNameEN)
            Me.DbManager.SetCmdParameter("@GroupNameJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.GroupNameJP)
            Me.DbManager.SetCmdParameter("@UserIDs", SqlDbType.NVarChar, ParameterDirection.Input, pDr.UserIDs)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, pDr.GroupID)
                ret = Me.DbManager.Execute()
            Else
                pDr.GroupID = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                If pDr.GroupID > 0 Then ret = 1
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_Institution_GroupRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "InstitutionID = @InstitutionID " & _
                                            ",GroupNameEN = @GroupNameEN " & _
                                            ",GroupNameJP = @GroupNameJP " & _
                                            ",UserIDs = @UserIDs " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                     "WHERE GroupID = @GroupID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@GroupNameEN", SqlDbType.NVarChar, ParameterDirection.Input, pDr.GroupNameEN)
            Me.DbManager.SetCmdParameter("@GroupNameJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.GroupNameJP)
            Me.DbManager.SetCmdParameter("@UserIDs", SqlDbType.NVarChar, ParameterDirection.Input, pDr.UserIDs)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@GroupID", SqlDbType.Int, ParameterDirection.Input, pDr.GroupID)
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
