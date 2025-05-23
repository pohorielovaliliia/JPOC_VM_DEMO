Public Class DaoInstitution
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Institution"
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
            If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
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
    Public Function GetCountByPK(ByVal pInstitutionID As Integer, _
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND defunct = 0 AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pInstitutionID)
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
            If pOnlyActive Then strSql &= " WHERE defunct = 0 AND Active = 1 "
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
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND defunct = 0 AND Active = 1 "
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

#Region "施設コードによる取得"
    Public Function GetByInstitutionCode(ByRef pDt As DataTable, _
                            ByVal pInstitutionCode As String, _
                            Optional pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE InstitutionCode = @InstitutionCode"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND defunct = 0 AND Active = 1 "
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionCode", SqlDbType.NVarChar, ParameterDirection.Input, pInstitutionCode)
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
    Public Function Delete(ByVal pInstitutionID As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE InstitutionID = @InstitutionID"
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

#Region "追加"
    Public Function Insert(ByRef pDr As DS_USER.T_JP_InstitutionRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                         "InstitutionCode " & _
                                                                        ",InstitutionNameEN " & _
                                                                        ",InstitutionNameJP " & _
                                                                        ",SISID " & _
                                                                        ",HQSISID " & _
                                                                        ",State " & _
                                                                        ",AddressLine1 " & _
                                                                        ",AddressLine2 " & _
                                                                        ",ZipCode " & _
                                                                        ",IPRange " & _
                                                                        ",AdditionalInfo " & _
                                                                        ",AllowRemoteAccess " & _
                                                                        ",Active " & _
                                                                        ",ContactPersonName " & _
                                                                        ",ContactPersonEmail " & _
                                                                        ",ContactPersonNumber " & _
                                                                        ",created_by " & _
                                                                        ",modified_by " & _
                                                                        ",checkout_by " & _
                                                                        ",created_date " & _
                                                                        ",modified_date " & _
                                                                        ",ContactPersonDepartment " & _
                                                                        ",defunct " & _
                                                                        "{0}" & _
                                                                    ") VALUES (" & _
                                                                         "@InstitutionCode " & _
                                                                        ",@InstitutionNameEN " & _
                                                                        ",@InstitutionNameJP " & _
                                                                        ",@SISID " & _
                                                                        ",@HQSISID " & _
                                                                        ",@State " & _
                                                                        ",@AddressLine1 " & _
                                                                        ",@AddressLine2 " & _
                                                                        ",@ZipCode " & _
                                                                        ",@IPRange " & _
                                                                        ",@AdditionalInfo " & _
                                                                        ",@AllowRemoteAccess " & _
                                                                        ",@Active " & _
                                                                        ",@ContactPersonName " & _
                                                                        ",@ContactPersonEmail " & _
                                                                        ",@ContactPersonNumber " & _
                                                                        ",@created_by " & _
                                                                        ",@modified_by " & _
                                                                        ",@checkout_by " & _
                                                                        ",@created_date " & _
                                                                        ",@modified_date " & _
                                                                        ",@ContactPersonDepartment " & _
                                                                        ",@defunct " & _
                                                                        "{1}" & _
                                                                    ");"

        Try
            Dim strSQL As String = String.Empty
            If pKeepIdValue Then
                strSQL = String.Format(IDENTITY_INSERT_ON, TABLE_NAME) & _
                         String.Format(SQL_QUERY, ",InstitutionID ", ",@InstitutionID ") & _
                         String.Format(IDENTITY_INSERT_OFF, TABLE_NAME)
            Else
                strSQL = String.Format(SQL_QUERY, String.Empty, String.Empty) & SCOPE_IDENTITY
            End If
            Me.DbManager.SetSqlCommand(strSQL, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionCode", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.InstitutionCode))
            Me.DbManager.SetCmdParameter("@InstitutionNameEN", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.InstitutionNameEN))
            Me.DbManager.SetCmdParameter("@InstitutionNameJP", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.InstitutionNameJP))
            If Not pDr.IsNull("SISID") Then
                Me.DbManager.SetCmdParameter("@SISID", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.SISID))
            Else
                Me.DbManager.SetCmdParameter("@SISID", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("HQSISID") Then
                Me.DbManager.SetCmdParameter("@HQSISID", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.HQSISID))
            Else
                Me.DbManager.SetCmdParameter("@HQSISID", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("State") Then
                Me.DbManager.SetCmdParameter("@State", SqlDbType.VarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.State.ToString))
            Else
                Me.DbManager.SetCmdParameter("@State", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("AddressLine1") Then
                Me.DbManager.SetCmdParameter("@AddressLine1", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.AddressLine1))
            Else
                Me.DbManager.SetCmdParameter("@AddressLine1", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("AddressLine2") Then
                Me.DbManager.SetCmdParameter("@AddressLine2", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.AddressLine2))
            Else
                Me.DbManager.SetCmdParameter("@AddressLine2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ZipCode") Then
                Me.DbManager.SetCmdParameter("@ZipCode", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ZipCode))
            Else
                Me.DbManager.SetCmdParameter("@ZipCode", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@IPRange", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.IPRange))
            If Not pDr.IsNull("AdditionalInfo") Then
                Me.DbManager.SetCmdParameter("@AdditionalInfo", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.AdditionalInfo))
            Else
                Me.DbManager.SetCmdParameter("@AdditionalInfo", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@AllowRemoteAccess", SqlDbType.Bit, ParameterDirection.Input, pDr.AllowRemoteAccess)
            Me.DbManager.SetCmdParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, pDr.Active)
            If Not pDr.IsNull("ContactPersonName") Then
                Me.DbManager.SetCmdParameter("@ContactPersonName", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ContactPersonName))
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonName", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ContactPersonEmail") Then
                Me.DbManager.SetCmdParameter("@ContactPersonEmail", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ContactPersonEmail))
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonEmail", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ContactPersonNumber") Then
                Me.DbManager.SetCmdParameter("@ContactPersonNumber", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ContactPersonNumber))
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonNumber", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            If Not pDr.IsNull("ContactPersonDepartment") Then
                Me.DbManager.SetCmdParameter("@ContactPersonDepartment", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ContactPersonDepartment))
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonDepartment", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
                ret = Me.DbManager.Execute()
            Else
                pDr.InstitutionID = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                If pDr.InstitutionID > 0 Then ret = 1
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
    Public Function Update(ByRef pDr As DS_USER.T_JP_InstitutionRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "InstitutionCode = @InstitutionCode " & _
                                            ",InstitutionNameEN = @InstitutionNameEN " & _
                                            ",InstitutionNameJP = @InstitutionNameJP " & _
                                            ",SISID = @SISID " & _
                                            ",HQSISID = @HQSISID " & _
                                            ",State = @State " & _
                                            ",AddressLine1 = @AddressLine1 " & _
                                            ",AddressLine2 = @AddressLine2 " & _
                                            ",ZipCode = @ZipCode " & _
                                            ",IPRange = @IPRange " & _
                                            ",AdditionalInfo = @AdditionalInfo " & _
                                            ",AllowRemoteAccess = @AllowRemoteAccess " & _
                                            ",Active = @Active " & _
                                            ",ContactPersonName = @ContactPersonName " & _
                                            ",ContactPersonEmail = @ContactPersonEmail " & _
                                            ",ContactPersonNumber = @ContactPersonNumber " & _
                                            ",created_by = @created_by " & _
                                            ",modified_by = @modified_by " & _
                                            ",checkout_by = @checkout_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_date = @modified_date " & _
                                            ",ContactPersonDepartment = @ContactPersonDepartment " & _
                                            ",defunct = @defunct " & _
                                     "WHERE InstitutionID = @InstitutionID"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@InstitutionCode", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.InstitutionCode))
            Me.DbManager.SetCmdParameter("@InstitutionNameEN", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.InstitutionNameEN))
            Me.DbManager.SetCmdParameter("@InstitutionNameJP", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.InstitutionNameJP))
            If Not pDr.IsNull("SISID") Then
                Me.DbManager.SetCmdParameter("@SISID", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.SISID))
            Else
                Me.DbManager.SetCmdParameter("@SISID", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("HQSISID") Then
                Me.DbManager.SetCmdParameter("@HQSISID", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.HQSISID))
            Else
                Me.DbManager.SetCmdParameter("@HQSISID", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("State") Then
                Me.DbManager.SetCmdParameter("@State", SqlDbType.VarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.State.ToString))
            Else
                Me.DbManager.SetCmdParameter("@State", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("AddressLine1") Then
                Me.DbManager.SetCmdParameter("@AddressLine1", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.AddressLine1))
            Else
                Me.DbManager.SetCmdParameter("@AddressLine1", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("AddressLine2") Then
                Me.DbManager.SetCmdParameter("@AddressLine2", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.AddressLine2))
            Else
                Me.DbManager.SetCmdParameter("@AddressLine2", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ZipCode") Then
                Me.DbManager.SetCmdParameter("@ZipCode", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ZipCode))
            Else
                Me.DbManager.SetCmdParameter("@ZipCode", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@IPRange", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.IPRange))
            If Not pDr.IsNull("AdditionalInfo") Then
                Me.DbManager.SetCmdParameter("@AdditionalInfo", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.AdditionalInfo))
            Else
                Me.DbManager.SetCmdParameter("@AdditionalInfo", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@AllowRemoteAccess", SqlDbType.Bit, ParameterDirection.Input, pDr.AllowRemoteAccess)
            Me.DbManager.SetCmdParameter("@Active", SqlDbType.Bit, ParameterDirection.Input, pDr.Active)
            If Not pDr.IsNull("ContactPersonName") Then
                Me.DbManager.SetCmdParameter("@ContactPersonName", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ContactPersonName))
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonName", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ContactPersonEmail") Then
                Me.DbManager.SetCmdParameter("@ContactPersonEmail", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ContactPersonEmail))
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonEmail", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ContactPersonNumber") Then
                Me.DbManager.SetCmdParameter("@ContactPersonNumber", SqlDbType.NVarChar, ParameterDirection.Input, Utilities.HtmlEncodeHtmlTag(pDr.ContactPersonNumber))
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonNumber", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            If Not pDr.IsNull("checkout_by") Then
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, pDr.checkout_by)
            Else
                Me.DbManager.SetCmdParameter("@checkout_by", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            If Not pDr.IsNull("ContactPersonDepartment") Then
                Me.DbManager.SetCmdParameter("@ContactPersonDepartment", SqlDbType.NVarChar, ParameterDirection.Input, pDr.ContactPersonDepartment)
            Else
                Me.DbManager.SetCmdParameter("@ContactPersonDepartment", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Me.DbManager.SetCmdParameter("@InstitutionID", SqlDbType.Int, ParameterDirection.Input, pDr.InstitutionID)
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
