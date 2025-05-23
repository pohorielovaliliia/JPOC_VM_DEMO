Public Class DaoLabTestTable5
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_LabTestTable5"
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

#Region "SRL_IDによる取得"
    Public Function GetCountBySrlId(ByVal pSrlId As String) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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
            'TODO: reduced_diseaseが空白データがあり、不要レコードの為、検索結果自体から削除
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE CONVERT(nvarchar(max),reduced_disease) <> '' AND reduced_disease is not null"
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

#Region "SRL_IDによる取得"
    Public Function GetBySrlId(ByRef pDt As DataTable, _
                               ByVal pSrlId As String, _
                               ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @srl_id"
        Try
            'TODO: reduced_diseaseに空白データがあり、不要レコードの為、検索結果自体から削除
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND CONVERT(nvarchar(max),reduced_disease) <> '' AND reduced_disease is not null"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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

#Region "SRL_IDによる削除"
    Public Function DeleteBySrlId(ByVal pSrlId As String) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE srl_id = @1"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@1", SqlDbType.NVarChar, ParameterDirection.Input, Me.FormatSrlId(pSrlId))
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
    Public Function Insert(ByRef pDr As DS_LAB_TEST.T_JP_LabTestTable5Row) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "srl_id " & _
                                                                        ",name " & _
                                                                        ",clinical_significance " & _
                                                                        ",reference1 " & _
                                                                        ",reference2 " & _
                                                                        ",reduced_disease " & _
                                                                        ",disease_name " & _
                                                                    ") VALUES (" & _
                                                                         "@srl_id " & _
                                                                        ",@name " & _
                                                                        ",@clinical_significance " & _
                                                                        ",@reference1 " & _
                                                                        ",@reference2 " & _
                                                                        ",@reduced_disease " & _
                                                                        ",@disease_name " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@srl_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.srl_id)
            If Not pDr.IsNull("name") Then
                Me.DbManager.SetCmdParameter("@name", SqlDbType.NText, ParameterDirection.Input, pDr.name)
            Else
                Me.DbManager.SetCmdParameter("@name", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("clinical_significance") Then
                Me.DbManager.SetCmdParameter("@clinical_significance", SqlDbType.NText, ParameterDirection.Input, pDr.clinical_significance)
            Else
                Me.DbManager.SetCmdParameter("@clinical_significance", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("reference1") Then
                Me.DbManager.SetCmdParameter("@reference1", SqlDbType.NText, ParameterDirection.Input, pDr.reference1)
            Else
                Me.DbManager.SetCmdParameter("@reference1", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("reference2") Then
                Me.DbManager.SetCmdParameter("@reference2", SqlDbType.NText, ParameterDirection.Input, pDr.reference2)
            Else
                Me.DbManager.SetCmdParameter("@reference2", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("reduced_disease") Then
                Me.DbManager.SetCmdParameter("@reduced_disease", SqlDbType.NText, ParameterDirection.Input, pDr.reduced_disease)
            Else
                Me.DbManager.SetCmdParameter("@reduced_disease", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("disease_name") Then
                Me.DbManager.SetCmdParameter("@disease_name", SqlDbType.NText, ParameterDirection.Input, pDr.disease_name)
            Else
                Me.DbManager.SetCmdParameter("@disease_name", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
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

#Region "SrlID整形"
    Public Function FormatSrlId(ByVal pSrlId As String) As String
        Return Int32.Parse(pSrlId).ToString("0000")
    End Function
#End Region

End Class
