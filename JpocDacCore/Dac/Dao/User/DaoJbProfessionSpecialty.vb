Public Class DaoJbProfessionSpecialty
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_JB_ProfessionSpecialty"
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
    Public Function GetCountByPK(ByVal pPOrder As Integer, _
                                 ByVal pSOrder As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE POrder = @POrder " & _
                                       "AND SOrder = @SOrder"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@POrder", SqlDbType.Int, ParameterDirection.Input, pPOrder)
            Me.DbManager.SetCmdParameter("@SOrder", SqlDbType.Int, ParameterDirection.Input, pSOrder)
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
                            ByVal pPOrder As Integer, _
                            ByVal pSOrder As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE POrder = @POrder " & _
                                       "AND SOrder = @SOrder"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@POrder", SqlDbType.Int, ParameterDirection.Input, pPOrder)
            Me.DbManager.SetCmdParameter("@SOrder", SqlDbType.Int, ParameterDirection.Input, pSOrder)
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
    Public Function Delete(ByVal pPOrder As Integer, _
                           ByVal pSOrder As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE POrder = @POrder " & _
                                       "AND SOrder = @SOrder"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@POrder", SqlDbType.Int, ParameterDirection.Input, pPOrder)
            Me.DbManager.SetCmdParameter("@SOrder", SqlDbType.Int, ParameterDirection.Input, pSOrder)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_JB_ProfessionSpecialtyRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "JB_Profession " & _
                                                                        ",JB_ProfessionJP " & _
                                                                        ",JB_Specialty " & _
                                                                        ",JB_SpecialtyJP " & _
                                                                        ",POrder " & _
                                                                        ",SOrder " & _
                                                                    ") VALUES (" & _
                                                                         "@JB_Profession " & _
                                                                        ",@JB_ProfessionJP " & _
                                                                        ",@JB_Specialty " & _
                                                                        ",@JB_SpecialtyJP " & _
                                                                        ",@POrder " & _
                                                                        ",@SOrder " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@JB_Profession", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_Profession)
            Me.DbManager.SetCmdParameter("@JB_ProfessionJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_ProfessionJP)
            Me.DbManager.SetCmdParameter("@JB_Specialty", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_Specialty)
            Me.DbManager.SetCmdParameter("@JB_SpecialtyJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_SpecialtyJP)
            Me.DbManager.SetCmdParameter("@POrder", SqlDbType.Int, ParameterDirection.Input, pDr.POrder)
            Me.DbManager.SetCmdParameter("@SOrder", SqlDbType.Int, ParameterDirection.Input, pDr.SOrder)
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_JB_ProfessionSpecialtyRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "JB_Profession = @JB_Profession " & _
                                            ",JB_ProfessionJP = @JB_ProfessionJP " & _
                                            ",JB_Specialty = @JB_Specialty " & _
                                            ",JB_SpecialtyJP = @JB_SpecialtyJP " & _
                                            ",POrder = @POrder " & _
                                            ",SOrder = @SOrder " & _
                                     "WHERE POrder = @POrder_ORG " & _
                                       "AND SOrder = @SOrder_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@JB_Profession", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_Profession)
            Me.DbManager.SetCmdParameter("@JB_ProfessionJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_ProfessionJP)
            Me.DbManager.SetCmdParameter("@JB_Specialty", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_Specialty)
            Me.DbManager.SetCmdParameter("@JB_SpecialtyJP", SqlDbType.NVarChar, ParameterDirection.Input, pDr.JB_SpecialtyJP)
            Me.DbManager.SetCmdParameter("@POrder", SqlDbType.Int, ParameterDirection.Input, pDr.POrder)
            Me.DbManager.SetCmdParameter("@SOrder", SqlDbType.Int, ParameterDirection.Input, pDr.SOrder)
            Me.DbManager.SetCmdParameter("@POrder_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("POrder", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@SOrder_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("SOrder", DataRowVersion.Original)))
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
