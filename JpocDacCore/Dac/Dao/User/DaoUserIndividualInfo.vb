Public Class DaoUserIndividualInfo
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_UserIndividualInfo"
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
    Public Function GetCountByPK(ByVal pUserId As Integer, _
                                 ByVal pInstitutionId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE institution_id = @institution_id " & _
                                       "AND user_id = @user_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pUserId)
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
                            ByVal pUserId As Integer, _
                            ByVal pInstitutionId As Integer) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE institution_id = @institution_id " & _
                                       "AND user_id = @user_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pUserId)
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
    Public Function Delete(ByVal pUserId As Integer, _
                           ByVal pInstitutionId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE institution_id = @institution_id " & _
                                       "AND user_id = @user_id"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pUserId)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
    Public Function DeleteByInstitutionID(ByVal pInstitutionId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE institution_id = @institution_id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.Int, ParameterDirection.Input, pInstitutionId)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_UserIndividualInfoRow) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "user_id " & _
                                                                        ",department " & _
                                                                        ",profession " & _
                                                                        ",specialty " & _
                                                                        ",tc_acceptance " & _
                                                                        ",ru_acceptance " & _
                                                                        ",pp_acceptance " & _
                                                                        ",zip_code " & _
                                                                        ",state " & _
                                                                        ",address1 " & _
                                                                        ",address2 " & _
                                                                        ",institution_name " & _
                                                                        ",job_title " & _
                                                                        ",graduation_year " & _
                                                                        ",workplace " & _
                                                                        ",promotion_code " & _
                                                                        ",institution_id " & _
                                                                    ") VALUES (" & _
                                                                         "@user_id " & _
                                                                        ",@department " & _
                                                                        ",@profession " & _
                                                                        ",@specialty " & _
                                                                        ",@tc_acceptance " & _
                                                                        ",@ru_acceptance " & _
                                                                        ",@pp_acceptance " & _
                                                                        ",@zip_code " & _
                                                                        ",@state " & _
                                                                        ",@address1 " & _
                                                                        ",@address2 " & _
                                                                        ",@institution_name " & _
                                                                        ",@job_title " & _
                                                                        ",@graduation_year " & _
                                                                        ",@workplace " & _
                                                                        ",@promotion_code " & _
                                                                        ",@institution_id " & _
                                                                    ")"

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.Int, ParameterDirection.Input, pDr.user_id)
            If Not pDr.IsNull("department") Then
                Me.DbManager.SetCmdParameter("@department", SqlDbType.NVarChar, ParameterDirection.Input, pDr.department)
            Else
                Me.DbManager.SetCmdParameter("@department", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("profession") Then
                Me.DbManager.SetCmdParameter("@profession", SqlDbType.NVarChar, ParameterDirection.Input, pDr.profession)
            Else
                Me.DbManager.SetCmdParameter("@profession", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("specialty") Then
                Me.DbManager.SetCmdParameter("@specialty", SqlDbType.NVarChar, ParameterDirection.Input, pDr.specialty)
            Else
                Me.DbManager.SetCmdParameter("@specialty", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("tc_acceptance") Then
                Me.DbManager.SetCmdParameter("@tc_acceptance", SqlDbType.Bit, ParameterDirection.Input, pDr.tc_acceptance)
            Else
                Me.DbManager.SetCmdParameter("@tc_acceptance", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ru_acceptance") Then
                Me.DbManager.SetCmdParameter("@ru_acceptance", SqlDbType.Bit, ParameterDirection.Input, pDr.ru_acceptance)
            Else
                Me.DbManager.SetCmdParameter("@ru_acceptance", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("pp_acceptance") Then
                Me.DbManager.SetCmdParameter("@pp_acceptance", SqlDbType.Bit, ParameterDirection.Input, pDr.pp_acceptance)
            Else
                Me.DbManager.SetCmdParameter("@pp_acceptance", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("zip_code") Then
                Me.DbManager.SetCmdParameter("@zip_code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.zip_code)
            Else
                Me.DbManager.SetCmdParameter("@zip_code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("state") Then
                Me.DbManager.SetCmdParameter("@state", SqlDbType.NVarChar, ParameterDirection.Input, pDr.state)
            Else
                Me.DbManager.SetCmdParameter("@state", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("address1") Then
                Me.DbManager.SetCmdParameter("@address1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.address1)
            Else
                Me.DbManager.SetCmdParameter("@address1", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("address1") Then
                Me.DbManager.SetCmdParameter("@address2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.address2)
            Else
                Me.DbManager.SetCmdParameter("@address2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("institution_name") Then
                Me.DbManager.SetCmdParameter("@institution_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.institution_name)
            Else
                Me.DbManager.SetCmdParameter("@institution_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("job_title") Then
                Me.DbManager.SetCmdParameter("@job_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.job_title)
            Else
                Me.DbManager.SetCmdParameter("@job_title", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("job_title") Then
                Me.DbManager.SetCmdParameter("@graduation_year", SqlDbType.Decimal, ParameterDirection.Input, pDr.graduation_year)
            Else
                Me.DbManager.SetCmdParameter("@graduation_year", SqlDbType.Decimal, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("workplace") Then
                Me.DbManager.SetCmdParameter("@workplace", SqlDbType.NVarChar, ParameterDirection.Input, pDr.workplace)
            Else
                Me.DbManager.SetCmdParameter("@workplace", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("promotion_code") Then
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, pDr.promotion_code)
            Else
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.Int, ParameterDirection.Input, pDr.institution_id)
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_UserIndividualInfoRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "user_id = @user_id " & _
                                            ",department = @department " & _
                                            ",profession = @profession " & _
                                            ",specialty = @specialty " & _
                                            ",tc_acceptance = @tc_acceptance " & _
                                            ",ru_acceptance = @ru_acceptance " & _
                                            ",pp_acceptance = @pp_acceptance " & _
                                            ",zip_code = @zip_code " & _
                                            ",state = @state " & _
                                            ",address1 = @address1 " & _
                                            ",address2 = @address2 " & _
                                            ",institution_name = @institution_name " & _
                                            ",job_title = @job_title " & _
                                            ",graduation_year = @graduation_year " & _
                                            ",workplace = @workplace " & _
                                            ",promotion_code = @promotion_code " & _
                                            ",institution_id = @institution_id " & _
                                     "WHERE user_id = @user_id_ORG " & _
                                       "AND institution_id = @institution_id_ORG"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@user_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.user_id)
            If Not pDr.IsNull("department") Then
                Me.DbManager.SetCmdParameter("@department", SqlDbType.NVarChar, ParameterDirection.Input, pDr.department)
            Else
                Me.DbManager.SetCmdParameter("@department", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("profession") Then
                Me.DbManager.SetCmdParameter("@profession", SqlDbType.NVarChar, ParameterDirection.Input, pDr.profession)
            Else
                Me.DbManager.SetCmdParameter("@profession", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("specialty") Then
                Me.DbManager.SetCmdParameter("@specialty", SqlDbType.NVarChar, ParameterDirection.Input, pDr.specialty)
            Else
                Me.DbManager.SetCmdParameter("@specialty", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("tc_acceptance") Then
                Me.DbManager.SetCmdParameter("@tc_acceptance", SqlDbType.Bit, ParameterDirection.Input, pDr.tc_acceptance)
            Else
                Me.DbManager.SetCmdParameter("@tc_acceptance", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ru_acceptance") Then
                Me.DbManager.SetCmdParameter("@ru_acceptance", SqlDbType.Bit, ParameterDirection.Input, pDr.ru_acceptance)
            Else
                Me.DbManager.SetCmdParameter("@ru_acceptance", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("pp_acceptance") Then
                Me.DbManager.SetCmdParameter("@pp_acceptance", SqlDbType.Bit, ParameterDirection.Input, pDr.pp_acceptance)
            Else
                Me.DbManager.SetCmdParameter("@pp_acceptance", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("zip_code") Then
                Me.DbManager.SetCmdParameter("@zip_code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.zip_code)
            Else
                Me.DbManager.SetCmdParameter("@zip_code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("state") Then
                Me.DbManager.SetCmdParameter("@state", SqlDbType.NVarChar, ParameterDirection.Input, pDr.state)
            Else
                Me.DbManager.SetCmdParameter("@state", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("address1") Then
                Me.DbManager.SetCmdParameter("@address1", SqlDbType.NVarChar, ParameterDirection.Input, pDr.address1)
            Else
                Me.DbManager.SetCmdParameter("@address1", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("address1") Then
                Me.DbManager.SetCmdParameter("@address2", SqlDbType.NVarChar, ParameterDirection.Input, pDr.address2)
            Else
                Me.DbManager.SetCmdParameter("@address2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("institution_name") Then
                Me.DbManager.SetCmdParameter("@institution_name", SqlDbType.NVarChar, ParameterDirection.Input, pDr.institution_name)
            Else
                Me.DbManager.SetCmdParameter("@institution_name", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("job_title") Then
                Me.DbManager.SetCmdParameter("@job_title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.job_title)
            Else
                Me.DbManager.SetCmdParameter("@job_title", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("job_title") Then
                Me.DbManager.SetCmdParameter("@graduation_year", SqlDbType.Decimal, ParameterDirection.Input, pDr.graduation_year)
            Else
                Me.DbManager.SetCmdParameter("@graduation_year", SqlDbType.Decimal, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("workplace") Then
                Me.DbManager.SetCmdParameter("@workplace", SqlDbType.NVarChar, ParameterDirection.Input, pDr.workplace)
            Else
                Me.DbManager.SetCmdParameter("@workplace", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("promotion_code") Then
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, pDr.promotion_code)
            Else
                Me.DbManager.SetCmdParameter("@promotion_code", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@institution_id", SqlDbType.NVarChar, ParameterDirection.Input, pDr.institution_id)
            Me.DbManager.SetCmdParameter("@user_id_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.N0Int(pDr.Item("user_id", DataRowVersion.Original)))
            Me.DbManager.SetCmdParameter("@institution_id_ORG", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.N0Int(pDr.Item("institution_id", DataRowVersion.Original)))
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
