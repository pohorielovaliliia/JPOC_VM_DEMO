Public Class DaoDrugDataJpc
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_DrugDataJpc"
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
    Public Function GetCountByPK(ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
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
                            ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
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
    Public Function Delete(ByVal pJpc As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE jpc = @jpc"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pJpc)
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
    Public Function Insert(ByRef pDr As DS_DRUG.T_JP_DrugDataJpcRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "jpc" & _
                                                                        ",dosform " & _
                                                                        ",dosformflag " & _
                                                                        ",generics " & _
                                                                        ",genericsflag " & _
                                                                        ",rankofmed " & _
                                                                        ",rankofdosform " & _
                                                                        ",name " & _
                                                                        ",cat1 " & _
                                                                        ",cat2 " & _
                                                                        ",cat3 " & _
                                                                        ",yj " & _
                                                                        ",ind_kidney " & _
                                                                        ",ind_liver " & _
                                                                        ",ind_pregnancy " & _
                                                                        ",ind_lactating " & _
                                                                        ",ind_children " & _
                                                                        ",ind_elderly " & _
                                                                    ") VALUES (" & _
                                                                         "@jpc" & _
                                                                        ",@dosform " & _
                                                                        ",@dosformflag " & _
                                                                        ",@generics " & _
                                                                        ",@genericsflag " & _
                                                                        ",@rankofmed " & _
                                                                        ",@rankofdosform " & _
                                                                        ",@name " & _
                                                                        ",@cat1 " & _
                                                                        ",@cat2 " & _
                                                                        ",@cat3 " & _
                                                                        ",@yj " & _
                                                                        ",@ind_kidney " & _
                                                                        ",@ind_liver " & _
                                                                        ",@ind_pregnancy " & _
                                                                        ",@ind_lactating " & _
                                                                        ",@ind_children " & _
                                                                        ",@ind_elderly " & _
                                                                    ")"

        Try
            Dim strSQL As String = String.Empty
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            If Not pDr.IsNull("dosform") Then
                Me.DbManager.SetCmdParameter("@dosform", SqlDbType.NVarChar, ParameterDirection.Input, pDr.dosform)
            Else
                Me.DbManager.SetCmdParameter("@dosform", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("dosformflag") Then
                Me.DbManager.SetCmdParameter("@dosformflag", SqlDbType.Int, ParameterDirection.Input, pDr.dosformflag)
            Else
                Me.DbManager.SetCmdParameter("@dosformflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("generics") Then
                Me.DbManager.SetCmdParameter("@generics", SqlDbType.NVarChar, ParameterDirection.Input, pDr.generics)
            Else
                Me.DbManager.SetCmdParameter("@generics", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("genericsflag") Then
                Me.DbManager.SetCmdParameter("@genericsflag", SqlDbType.Int, ParameterDirection.Input, pDr.genericsflag)
            Else
                Me.DbManager.SetCmdParameter("@genericsflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("rankofmed") Then
                Me.DbManager.SetCmdParameter("@rankofmed", SqlDbType.Decimal, ParameterDirection.Input, pDr.rankofmed)
            Else
                Me.DbManager.SetCmdParameter("@rankofmed", SqlDbType.Decimal, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("rankofdosform") Then
                Me.DbManager.SetCmdParameter("@rankofdosform", SqlDbType.Decimal, ParameterDirection.Input, pDr.rankofdosform)
            Else
                Me.DbManager.SetCmdParameter("@rankofdosform", SqlDbType.Decimal, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("name") Then
                Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            Else
                Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cat1") Then
                Me.DbManager.SetCmdParameter("@cat1", SqlDbType.Int, ParameterDirection.Input, pDr.cat1)
            Else
                Me.DbManager.SetCmdParameter("@cat1", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cat2") Then
                Me.DbManager.SetCmdParameter("@cat2", SqlDbType.Int, ParameterDirection.Input, pDr.cat2)
            Else
                Me.DbManager.SetCmdParameter("@cat2", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cat3") Then
                Me.DbManager.SetCmdParameter("@cat3", SqlDbType.Int, ParameterDirection.Input, pDr.cat3)
            Else
                Me.DbManager.SetCmdParameter("@cat3", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yj") Then
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yj)
            Else
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yj") Then
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_kidney)
            Else
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_liver") Then
                Me.DbManager.SetCmdParameter("@ind_liver", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_liver)
            Else
                Me.DbManager.SetCmdParameter("@ind_liver", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_pregnancy") Then
                Me.DbManager.SetCmdParameter("@ind_pregnancy", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_pregnancy)
            Else
                Me.DbManager.SetCmdParameter("@ind_pregnancy", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_lactating") Then
                Me.DbManager.SetCmdParameter("@ind_lactating", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_lactating)
            Else
                Me.DbManager.SetCmdParameter("@ind_lactating", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_children") Then
                Me.DbManager.SetCmdParameter("@ind_children", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_children)
            Else
                Me.DbManager.SetCmdParameter("@ind_children", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_elderly") Then
                Me.DbManager.SetCmdParameter("@ind_elderly", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_elderly)
            Else
                Me.DbManager.SetCmdParameter("@ind_elderly", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
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
    Public Function Update(ByRef pDr As DS_DRUG.T_JP_DrugDataJpcRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "jpc = @jpc " & _
                                            ",dosform = @dosform " & _
                                            ",dosformflag = @dosformflag " & _
                                            ",generics = @generics " & _
                                            ",genericsflag = @genericsflag " & _
                                            ",rankofmed = @rankofmed " & _
                                            ",rankofdosform = @rankofdosform " & _
                                            ",name = @name " & _
                                            ",cat1 = @cat1 " & _
                                            ",cat2 = @cat2 " & _
                                            ",cat3 = @cat3 " & _
                                            ",yj = @yj " & _
                                            ",ind_kidney = @ind_kidney " & _
                                            ",ind_liver = @ind_liver " & _
                                            ",ind_pregnancy = @ind_pregnancy " & _
                                            ",ind_lactating = @ind_lactating " & _
                                            ",ind_children = @ind_children " & _
                                            ",ind_elderly = @ind_elderly " & _
                                     "WHERE jpc = @jpc_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@jpc", SqlDbType.NVarChar, ParameterDirection.Input, pDr.jpc)
            If Not pDr.IsNull("dosform") Then
                Me.DbManager.SetCmdParameter("@dosform", SqlDbType.NVarChar, ParameterDirection.Input, pDr.dosform)
            Else
                Me.DbManager.SetCmdParameter("@dosform", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("dosformflag") Then
                Me.DbManager.SetCmdParameter("@dosformflag", SqlDbType.Int, ParameterDirection.Input, pDr.dosformflag)
            Else
                Me.DbManager.SetCmdParameter("@dosformflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("generics") Then
                Me.DbManager.SetCmdParameter("@generics", SqlDbType.NVarChar, ParameterDirection.Input, pDr.generics)
            Else
                Me.DbManager.SetCmdParameter("@generics", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("genericsflag") Then
                Me.DbManager.SetCmdParameter("@genericsflag", SqlDbType.Int, ParameterDirection.Input, pDr.genericsflag)
            Else
                Me.DbManager.SetCmdParameter("@genericsflag", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("rankofmed") Then
                Me.DbManager.SetCmdParameter("@rankofmed", SqlDbType.Decimal, ParameterDirection.Input, pDr.rankofmed)
            Else
                Me.DbManager.SetCmdParameter("@rankofmed", SqlDbType.Decimal, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("rankofdosform") Then
                Me.DbManager.SetCmdParameter("@rankofdosform", SqlDbType.Decimal, ParameterDirection.Input, pDr.rankofdosform)
            Else
                Me.DbManager.SetCmdParameter("@rankofdosform", SqlDbType.Decimal, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("name") Then
                Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.name)
            Else
                Me.DbManager.SetCmdParameter("@name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cat1") Then
                Me.DbManager.SetCmdParameter("@cat1", SqlDbType.Int, ParameterDirection.Input, pDr.cat1)
            Else
                Me.DbManager.SetCmdParameter("@cat1", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cat2") Then
                Me.DbManager.SetCmdParameter("@cat2", SqlDbType.Int, ParameterDirection.Input, pDr.cat2)
            Else
                Me.DbManager.SetCmdParameter("@cat2", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("cat3") Then
                Me.DbManager.SetCmdParameter("@cat3", SqlDbType.Int, ParameterDirection.Input, pDr.cat3)
            Else
                Me.DbManager.SetCmdParameter("@cat3", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yj") Then
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, pDr.yj)
            Else
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("yj") Then
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_kidney)
            Else
                Me.DbManager.SetCmdParameter("@yj", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_liver") Then
                Me.DbManager.SetCmdParameter("@ind_liver", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_liver)
            Else
                Me.DbManager.SetCmdParameter("@ind_liver", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_pregnancy") Then
                Me.DbManager.SetCmdParameter("@ind_pregnancy", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_pregnancy)
            Else
                Me.DbManager.SetCmdParameter("@ind_pregnancy", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_lactating") Then
                Me.DbManager.SetCmdParameter("@ind_lactating", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_lactating)
            Else
                Me.DbManager.SetCmdParameter("@ind_lactating", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_children") Then
                Me.DbManager.SetCmdParameter("@ind_children", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_children)
            Else
                Me.DbManager.SetCmdParameter("@ind_children", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ind_elderly") Then
                Me.DbManager.SetCmdParameter("@ind_elderly", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ind_elderly)
            Else
                Me.DbManager.SetCmdParameter("@ind_elderly", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@jpc_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.NZ(pDr.Item("jpc", DataRowVersion.Original)))
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
