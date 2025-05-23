Public Class DaoSituationOrderSetPatientProfile
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_SituationOrderSetPatientProfile"
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
            If pOnlyActive Then strSql &= " WHERE " & MyBase.ActiveCondition
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
    Public Function GetCountByPK(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
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

#Region "DiseaseIDによる取得"
    Public Function GetCountByDiseaseID(ByVal pDiseaseID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoSituation.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.situation_id " & _
                                                                "" & _
                                                    "WHERE t2.disease_id = @disease_id" & _
                                                      "" & _
                                                 ")"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)
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

#Region "DiseaseIDによる取得"
    Public Function GetByDiseaseID(ByRef pDt As DataTable, _
                                   ByVal pDiseaseID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoSituation.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.situation_id " & _
                                                                "" & _
                                                    "WHERE t2.disease_id = @disease_id" & _
                                                      "" & _
                                                 ")"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationIDによる取得"
    Public Function GetBySituationID(ByRef pDt As DataTable, _
                                   ByVal ppSituationId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE situation_id = @situation_id"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@situation_id", SqlDbType.Int, ParameterDirection.Input, ppSituationId)
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
                            ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
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
    Public Function Delete(ByVal pID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pID)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                             "situation_id" & _
                                                                            ",orderset_id" & _
                                                                            ",patient_profile_short" & _
                                                                            ",patient_profile_long" & _
                                                                            ",grade" & _
                                                                            ",grade_suffix" & _
                                                                            ",sequence" & _
                                                                            "{0}" & _
                                                                    ") VALUES (" & _
                                                                             "@situation_id" & _
                                                                            ",@orderset_id" & _
                                                                            ",@patient_profile_short" & _
                                                                            ",@patient_profile_long" & _
                                                                            ",@grade" & _
                                                                            ",@grade_suffix" & _
                                                                            ",@sequence" & _
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
            Me.DbManager.SetCmdParameter("@situation_id", SqlDbType.Int, ParameterDirection.Input, pDr.situation_id)
            Me.DbManager.SetCmdParameter("@orderset_id", SqlDbType.Int, ParameterDirection.Input, pDr.orderset_id)
            Me.DbManager.SetCmdParameter("@patient_profile_short", SqlDbType.NText, ParameterDirection.Input, pDr.patient_profile_short)
            Me.DbManager.SetCmdParameter("@patient_profile_long", SqlDbType.NText, ParameterDirection.Input, pDr.patient_profile_long)
            Me.DbManager.SetCmdParameter("@grade", SqlDbType.NVarChar, ParameterDirection.Input, pDr.grade)
            Me.DbManager.SetCmdParameter("@grade_suffix", SqlDbType.NVarChar, ParameterDirection.Input, pDr.grade_suffix)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim situationOrderSetPatientProfileId As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = situationOrderSetPatientProfileId
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
    Public Function UpdateSituationOrderSetPatientProfile(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "situation_id = @situation_id " & _
                                            ",orderset_id = @orderset_id " & _
                                            ",patient_profile_short = @patient_profile_short " & _
                                            ",patient_profile_long = @patient_profile_long " & _
                                            ",grade = @grade " & _
                                            ",grade_suffix = @grade_suffix " & _
                                            ",sequence = @sequence " & _
                                    "WHERE id = @id "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@situation_id", SqlDbType.Int, ParameterDirection.Input, pDr.situation_id)
            Me.DbManager.SetCmdParameter("@orderset_id", SqlDbType.Int, ParameterDirection.Input, pDr.orderset_id)
            Me.DbManager.SetCmdParameter("@patient_profile_short", SqlDbType.NText, ParameterDirection.Input, pDr.patient_profile_short)
            Me.DbManager.SetCmdParameter("@patient_profile_long", SqlDbType.NText, ParameterDirection.Input, pDr.patient_profile_long)
            Me.DbManager.SetCmdParameter("@grade", SqlDbType.NVarChar, ParameterDirection.Input, pDr.grade)
            Me.DbManager.SetCmdParameter("@grade_suffix", SqlDbType.NVarChar, ParameterDirection.Input, pDr.grade_suffix)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
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
