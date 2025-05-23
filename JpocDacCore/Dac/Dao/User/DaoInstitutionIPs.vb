Public Class DaoInstitutionIPs
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_InstitutionIPs"
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
    Public Function GetCountByPK(ByVal pInstitutionID As Integer, _
                                 ByVal pIP As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID " & _
                                       "AND IP = @IP"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
            Me.DbManager.SetCmdParameter("@IP", SqlDbType.NVarChar, ParameterDirection.Input, pIP)
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
                            ByVal pInstitutionID As Integer, _
                            ByVal pIP As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID " & _
                                       "AND IP = @IP"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
            Me.DbManager.SetCmdParameter("@IP", SqlDbType.NVarChar, ParameterDirection.Input, pIP)
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
    Public Function GetByInstitutionID(ByRef pDt As DataTable, _
                                       ByVal pInstitutionID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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

#Region "削除"
    Public Function Delete(ByVal pInstitutionID As Integer, _
                           ByVal pIP As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID " & _
                                       "AND IP = @IP"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
            Me.DbManager.SetCmdParameter("@IP", SqlDbType.NVarChar, ParameterDirection.Input, pIP)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#Region "施設IDによる削除"
    Public Function DeleteByInstitutionID(ByVal pInstitutionID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region
#End Region

#Region "追加"
    Public Function Insert(ByRef pDr As DS_USER.T_JP_InstitutionIPsRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "InstitutionID " & _
                                                                        ",IP " & _
                                                                    ") VALUES (" & _
                                                                         "@InstitutionID " & _
                                                                        ",@IP " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@IP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.IP)
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_InstitutionIPsRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "InstitutionID = @InstitutionID " & _
                                            ",IP = @IP " & _
                                     "WHERE InstitutionID = @InstitutionID_ORG " & _
                                       "AND IP = @IP_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@IP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.IP)
            Me.DbManager.SetCmdParameter("@InstitutionID_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("InstitutionID", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@IP_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("IP", DataRowVersion.Original)))
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
