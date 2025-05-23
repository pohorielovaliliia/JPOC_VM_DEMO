Public Class DaoDrugDataCategory
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugDataCategory"
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
    Public Function GetCountByPK(ByVal pJpc As String, _
                                 ByVal pCtgId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND ctgid = @ctgid"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@ctgid", SqlDbType.Int, ParameterDirection.Input, pCtgId)
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
            If pOnlyActive Then strSql &= " WHERE " & MyBase.ActiveCondition
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
                            ByVal pJpc As String, _
                            ByVal pCtgId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND ctgid = @ctgid"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@ctgid", SqlDbType.Int, ParameterDirection.Input, pCtgId)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "JPCによるCTG_ID取得(MIN OR MAX)"
    Public Function GetCtgIdByJpc(ByVal pJpc As String, _
                                  Optional ByVal pMinMax As PublicEnum.eMinMax = PublicEnum.eMinMax.MIN) As Integer
        Const SQL_QUERY As String = "SELECT {0}(ctgid) AS ctgid " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc "
        Try
            Dim minMax As String = System.Enum.GetName(GetType(PublicEnum.eMinMax), pMinMax)
            Dim strSql As String = String.Format(SQL_QUERY, minMax)
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Dim ret As String = Me.DbManager.ExecuteScalar()
            If String.IsNullOrEmpty(ret) Then Return 0
            Return Utilities.N0Int(ret)
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
    Public Function Delete(ByVal pJpc As String, _
                           ByVal pCtgId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc " & _
                                       "AND ctgid = @ctgid"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
            Me.DbManager.SetCmdParameter("@ctgid", SqlDbType.Int, ParameterDirection.Input, pCtgId)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataCategoryRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "jpc " & _
                                                                        ",ctgid " & _
                                                                        ",categoryflag " & _
                                                                    ") VALUES (" & _
                                                                         "@jpc " & _
                                                                        ",@ctgid " & _
                                                                        ",@categoryflag " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            Me.DbManager.SetCmdParameter("@ctgid", SqlDbType.Int, ParameterDirection.Input, pDr.ctgid)
            If Not pDr.IsNull("categoryflag") Then
                Me.DbManager.SetCmdParameter("@categoryflag", SqlDbType.Int, ParameterDirection.Input, pDr.categoryflag)
            Else
                Me.DbManager.SetCmdParameter("@categoryflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataCategoryRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "jpc = @jpc " & _
                                            ",ctgid = @ctgid " & _
                                            ",categoryflag = @categoryflag " & _
                                     "WHERE jpc = @jpc_ORG " & _
                                       "AND ctgid = @ctgid_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            Me.DbManager.SetCmdParameter("@ctgid", SqlDbType.Int, ParameterDirection.Input, pDr.ctgid)
            If Not pDr.IsNull("categoryflag") Then
                Me.DbManager.SetCmdParameter("@categoryflag", SqlDbType.Int, ParameterDirection.Input, pDr.categoryflag)
            Else
                Me.DbManager.SetCmdParameter("@categoryflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@jpc_ORG", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Item("jpc", DataRowVersion.Original))
            Me.DbManager.SetCmdParameter("@ctgid_ORG", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDr.Item("ctgid", DataRowVersion.Original)))
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
