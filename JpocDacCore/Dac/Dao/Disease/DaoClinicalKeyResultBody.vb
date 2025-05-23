Public Class DaoClinicalKeyResultBody
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_ClinicalKeyResultBody"
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
    Public Function GetCountByPK(ByVal pHeaderId As Integer, _
                                 ByVal pSequence As Integer, _
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE header_id = @header_id" & _
                                     " AND sequence = @sequence"

        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@header_id", SqlDbType.Int, ParameterDirection.Input, pHeaderId)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pSequence)
            Return Utilities.N0Int(Me.DbManager.ExecuteScalar())
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DiseaseIdによる取得"
    Public Function GetCountByDiseaseId(ByVal pDiseaseId As Integer, _
                                        Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " body " & _
                                     "WHERE EXISTS (" & _
                                        "SELECT * " & _
                                        " FROM " & DaoClinicalKeyResultHeader.TABLE_NAME & " header " & _
                                        " WHERE header.id = body.header_id" & _
                                        " AND header.disease_id = @disease_id" & _
                                     ")"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
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
                            ByVal pHeaderId As Integer, _
                            ByVal pSequence As Integer, _
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE header_id = @header_id" & _
                                     " AND sequence = @sequence"
        Try
            Dim strSql As String = SQL_QUERY
            'If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@header_id", SqlDbType.Int, ParameterDirection.Input, pHeaderId)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pSequence)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DiseaseIdによる取得"
    Public Function GetByDiseaseId(ByRef pDt As DataTable, _
                            ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " body " & _
                                     "WHERE EXISTS (" & _
                                        "SELECT * " & _
                                        " FROM " & DaoClinicalKeyResultHeader.TABLE_NAME & " header " & _
                                        " WHERE header.id = body.header_id" & _
                                        " AND header.disease_id = @disease_id" & _
                                     ") " & _
                                     " ORDER BY body.sequence "
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
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
    Public Function DeleteById(ByVal pHeaderId As Integer, _
                               ByVal pSequence As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE header_id = @header_id" & _
                                     " AND sequence = @sequence"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@header_id", SqlDbType.Int, ParameterDirection.Input, pHeaderId)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pSequence)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_ClinicalKeyResultBodyRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                     "header_id " & _
                                                                     ",sequence " & _
                                                                     ",eid " & _
                                                                     ",cid " & _
                                                                     ",itemtitle " & _
                                                                     ",sourcetitle " & _
                                                                     ",contenttype " & _
                                                                   ") VALUES (" & _
                                                                      "@header_id " & _
                                                                      ",@sequence " & _
                                                                      ",@eid " & _
                                                                     ",@cid " & _
                                                                     ",@itemtitle " & _
                                                                     ",@sourcetitle " & _
                                                                     ",@contenttype " & _
                                                                   ");"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)

            Me.DbManager.SetCmdParameter("@header_id", SqlDbType.Int, ParameterDirection.Input, pDr.header_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@eid", SqlDbType.NVarChar, ParameterDirection.Input, pDr.eid)
            Me.DbManager.SetCmdParameter("@cid", SqlDbType.NVarChar, ParameterDirection.Input, pDr.cid)
            Me.DbManager.SetCmdParameter("@itemtitle", SqlDbType.NVarChar, ParameterDirection.Input, pDr.itemtitle)
            Me.DbManager.SetCmdParameter("@sourcetitle", SqlDbType.NVarChar, ParameterDirection.Input, pDr.sourcetitle)
            Me.DbManager.SetCmdParameter("@contenttype", SqlDbType.NVarChar, ParameterDirection.Input, pDr.contenttype)

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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_ClinicalKeyResultBodyRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                            ",eid = @eid " & _
                                            ",cid = @cid " & _
                                            ",itemtitle = @itemtitle " & _
                                            ",sourcetitle = @sourcetitle " & _
                                            ",contenttype = @contenttype " & _
                                     "WHERE header_id = @header_id" & _
                                     " AND sequence = @sequence"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@header_id", SqlDbType.Int, ParameterDirection.Input, pDr.header_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Me.DbManager.SetCmdParameter("@eid", SqlDbType.NVarChar, ParameterDirection.Input, pDr.eid)
            Me.DbManager.SetCmdParameter("@cid", SqlDbType.NVarChar, ParameterDirection.Input, pDr.cid)
            Me.DbManager.SetCmdParameter("@itemtitle", SqlDbType.NVarChar, ParameterDirection.Input, pDr.itemtitle)
            Me.DbManager.SetCmdParameter("@sourcetitle", SqlDbType.NVarChar, ParameterDirection.Input, pDr.sourcetitle)
            Me.DbManager.SetCmdParameter("@contenttype", SqlDbType.NVarChar, ParameterDirection.Input, pDr.contenttype)

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
