Public Class DaoSubscription
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Subscription"
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
            If pOnlyActive Then strSql &= " WHERE Active = 1 "
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
    Public Function GetCountByPK(ByVal pSubscriptionID As Integer, _
                                 Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE SubscriptionID = @SubscriptionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pSubscriptionID)
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
    Public Function GetCountByInstId(ByVal pInstitutionID As Integer, _
                                     Optional pOnlyActive As Boolean = False) As Integer

        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@pInstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
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
            If pOnlyActive Then strSql &= " WHERE Active = 1 "
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
                            ByVal pSubscriptionID As Integer,
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE SubscriptionID = @SubscriptionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pSubscriptionID)
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
    Public Function GetByInstId(ByRef pDt As DataTable, _
                                ByVal pInstitutionID As Integer, _
                                Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                    "FROM " & TABLE_NAME & " " & _
                                    "WHERE InstitutionID = @InstitutionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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
    Public Function Delete(ByVal pSubscriptionID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE SubscriptionID = @SubscriptionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pSubscriptionID)
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
    Public Function Insert(ByRef pDr As DS_USER.T_JP_SubscriptionRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "InstitutionID " & _
                                                                        ",UserID " & _
                                                                        ",PackageID " & _
                                                                        ",StartDate " & _
                                                                        ",EndDate " & _
                                                                        ",Remarks " & _
                                                                        ",NotificationSent " & _
                                                                        ",SendReminder " & _
                                                                        ",Active " & _
                                                                        ",created_by " & _
                                                                        ",modified_by " & _
                                                                        ",checkout_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_date " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@InstitutionID " & _
                                                                        ",@UserID " & _
                                                                        ",@PackageID " & _
                                                                        ",@StartDate " & _
                                                                        ",@EndDate " & _
                                                                        ",@Remarks " & _
                                                                        ",@NotificationSent " & _
                                                                        ",@SendReminder " & _
                                                                        ",@Active " & _
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
                         String.Format(SQL_QUERY, ",SubscriptionID ", ",@SubscriptionID ") & _
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, pDr.UserID)
            Me.DbManager.SetCmdParameter("@PackageID", SqlDbType.Int, ParameterDirection.Input, pDr.PackageID)
            Me.DbManager.SetCmdParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, pDr.StartDate)
            Me.DbManager.SetCmdParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, pDr.EndDate)
            Me.DbManager.SetCmdParameter("@Remarks", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Remarks)
            Me.DbManager.SetCmdParameter("@NotificationSent", SqlDbType.Bit, ParameterDirection.Input, pDr.NotificationSent)
            Me.DbManager.SetCmdParameter("@SendReminder", SqlDbType.Bit, ParameterDirection.Input, pDr.SendReminder)
            Me.DbManager.SetCmdParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, pDr.Active)
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
                Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pDr.SubscriptionID)
                ret = Me.DbManager.Execute()
            Else
                pDr.SubscriptionID = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                If pDr.SubscriptionID > 0 Then ret = 1
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_SubscriptionRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "InstitutionID = @InstitutionID " & _
                                            ",UserID = @UserID " & _
                                            ",PackageID = @PackageID " & _
                                            ",StartDate = @StartDate " & _
                                            ",EndDate = @EndDate " & _
                                            ",Remarks = @Remarks " & _
                                            ",NotificationSent = @NotificationSent " & _
                                            ",SendReminder = @SendReminder " & _
                                            ",Active = @Active " & _
                                            ",created_by = @created_by " & _
                                            ",modified_by = @modified_by " & _
                                            ",checkout_by = @checkout_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_date = @modified_date " & _
                                     "WHERE SubscriptionID = @SubscriptionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
            Me.DbManager.SetCmdParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, pDr.UserID)
            Me.DbManager.SetCmdParameter("@PackageID", SqlDbType.Int, ParameterDirection.Input, pDr.PackageID)
            Me.DbManager.SetCmdParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, pDr.StartDate)
            Me.DbManager.SetCmdParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, pDr.EndDate)
            Me.DbManager.SetCmdParameter("@Remarks", SqlDbType.NVarChar, ParameterDirection.Input, pDr.Remarks)
            Me.DbManager.SetCmdParameter("@NotificationSent", SqlDbType.Bit, ParameterDirection.Input, pDr.NotificationSent)
            Me.DbManager.SetCmdParameter("@SendReminder", SqlDbType.Bit, ParameterDirection.Input, pDr.SendReminder)
            Me.DbManager.SetCmdParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, pDr.Active)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)

            Me.DbManager.SetCmdParameter("@SubscriptionID", SqlDbType.Int, ParameterDirection.Input, pDr.SubscriptionID)

            Return Me.DbManager.Execute
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

    Public Function GetDiseaseSubCategory(ByVal pDiseaseID As String, ByVal pInstitutionID As String,
                               ByRef pDt As DataTable,
                               Optional ByVal tableName As String = "DiseaseCategory") As Integer

        Const SQL_QUERY_DISEASE_CATEGORY As String = "SELECT DISTINCT" &
                                                     " DC.category " &
                                                     ", DSC.subcategory " &
                                                     ", DC.sequence, DSC.categoryID " &
                                              " FROM T_JP_DiseaseSubcategoryMap DSCM " &
                                              " INNER JOIN T_JP_DiseaseSubcategory DSC ON DSC.id = DSCM.subcategoryId " &
                                              " INNER JOIN T_JP_DiseaseCategory DC ON DC.id = DSC.categoryID  " &
                                              " INNER JOIN T_JP_SubscriptionDetails SD  ON SD.categoryID = DC.id or SD.categoryid = 0  " &
                                              " INNER JOIN T_JP_Subscription SC  ON SC.SubscriptionID = SD.SubscriptionID  AND SC.InstitutionID = @2  " &
                                              " WHERE DSCM.disease_id=@1 AND SD.Active = 1 AND SC.Active = 1 AND SC.EndDate > GETDATE() " &
                                              " ORDER BY DC.sequence "

        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY_DISEASE_CATEGORY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@1", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDiseaseID))
            Me.DbManager.SetCmdParameter("@2", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pInstitutionID))
            If pDt Is Nothing Then pDt = New DataTable(tableName)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try

    End Function

End Class
