Public Class DaoImage
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_Image"
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDbmanager As ElsDataBase)
        MyBase.New(pDbmanager)
    End Sub
#End Region

#Region "Init"
    Public Overrides Sub Init()
        Try

        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
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
    Public Function GetCountByPK(ByVal pID As Integer, _
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " WHERE " & MyBase.ActiveCondition
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
    Public Function GetCountByDiseaseID(ByVal pDiseaseID As Integer, _
                                        Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoImgMapping.TABLE_NAME & " t2 ON " & _
                                                                "t2.image_id = t1.id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                     "{1}" & _
                                                 ")"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty)
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
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

#Region "RawImageがNULLでも空でもないレコード件数取得（ポップアップ分除外）"
    Public Function GetRawImageCount() As Integer
        Const SQL_QUERY As String = "SELECT COUNT(raw_image) " & _
                                      "FROM " & TABLE_NAME & " I " & _
                                        "INNER JOIN " & DaoImgMapping.TABLE_NAME & " IM ON " & _
                                            "IM.image_id = I.id " & _
                                     "WHERE IM.img_type <> 'Popup' " & _
                                       "AND I.raw_image <> '' "

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

#Region "DpImageがNULLでも空でもないレコード件数取得（ポップアップ分除外）"
    Public Function GetDpImageCount() As Integer
        Const SQL_QUERY As String = "SELECT COUNT(dp_image) " & _
                                      "FROM T_JP_IMAGE I " & _
                                        "INNER JOIN " & DaoImgMapping.TABLE_NAME & " IM ON " & _
                                            "IM.image_id = I.id " & _
                                     "WHERE IM.img_type <> 'Popup' " & _
                                       "AND dp_image <> '' "

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

#Region "AdImageがNULLでも空でもないレコード件数取得（ポップアップ分除外）"
    Public Function GetAdImageCount() As Integer
        Const SQL_QUERY As String = "SELECT COUNT(ad_image) " & _
                                      "FROM T_JP_IMAGE I " & _
                                        "INNER JOIN " & DaoImgMapping.TABLE_NAME & " IM ON " & _
                                            "IM.image_id = I.id " & _
                                     "WHERE IM.img_type <> 'Popup' " & _
                                       "AND ad_image <> '' "

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

#Region "TbImageがNULLでも空でもないレコード件数取得（ポップアップ分除外）"
    Public Function GetTbImageCount() As Integer
        Const SQL_QUERY As String = "SELECT COUNT(tb_image) " & _
                                      "FROM T_JP_IMAGE I " & _
                                        "INNER JOIN " & DaoImgMapping.TABLE_NAME & " IM ON " & _
                                            "IM.image_id = I.id " & _
                                     "WHERE IM.img_type <> 'Popup' " & _
                                       "AND tb_image <> '' "

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

#Region "TbImageがNULLでも空でもないレコード件数取得（ポップアップ分のみ）"
    Public Function GetPopupTbImageCount() As Integer
        Const SQL_QUERY As String = "SELECT COUNT(tb_image) " & _
                                      "FROM T_JP_IMAGE I " & _
                                        "INNER JOIN " & DaoImgMapping.TABLE_NAME & " IM ON " & _
                                            "IM.image_id = I.id " & _
                                     "WHERE IM.img_type = 'Popup' " & _
                                       "AND I.popup_type = 'video' " & _
                                       "AND tb_image <> '' "

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

#Region "検索文字列による曖昧検索"
    ''' <summary>
    ''' 検索文字列による曖昧検索
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>リファクタリングの為、T_JP_Image.metadataのみが検索対象</remarks>
    Public Function GetCountByTextForMetadata(ByVal pText As String, ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT i.* " & _
                                    "FROM " & TABLE_NAME & " i " & _
                                    "WHERE EXISTS(" & _
                                    "  SELECT * FROM " & DaoImgMapping.TABLE_NAME & " m " & _
                                    "  INNER JOIN " & DaoDisease.TABLE_NAME & " d " & _
                                    "  ON d.id = m.disease_id " & _
                                    "  {0} " & _
                                    "  " & _
                                    "  WHERE m.image_id = i.id  " & _
                                    "  {1}  ) " & _
                                    "AND i.metadata like @text " & _
                                    "{2} "

        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("d"), " AND " & MyBase.ActiveCondition("m"), " AND " & MyBase.ActiveCondition("i"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)

            End If
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pText)
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

#Region "RawImageRecord取得"
    Public Function GetByRawImage(ByRef pDt As DataTable, _
                                  ByVal pFileUrl As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE raw_image = @raw_image "
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@raw_image", SqlDbType.VarChar, ParameterDirection.Input, pFileUrl)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "TbImageRecord取得"
    Public Function GetByTbImage(ByRef pDt As DataTable, _
                                 ByVal pFileUrl As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE tb_image = @tb_image "
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@tb_image", SqlDbType.VarChar, ParameterDirection.Input, pFileUrl)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DpImageRecord取得"
    Public Function GetByDpImage(ByRef pDt As DataTable, _
                                 ByVal pFileUrl As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE dp_image = @dp_image "
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@dp_image", SqlDbType.VarChar, ParameterDirection.Input, pFileUrl)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "AdImageRecord取得"
    Public Function GetByAdImage(ByRef pDt As DataTable, _
                                 ByVal pFileUrl As String) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE ad_image = @ad_image "
        Try
            Dim strSql As String = SQL_QUERY
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@ad_image", SqlDbType.VarChar, ParameterDirection.Input, pFileUrl)
            Return Me.DbManager.ExecuteAndGetDataTable(pDt, True)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "一覧取得"
    Public Function GetImageList(ByRef pDt As DataTable) As Integer
        Const SQL_QUERY As String = "SELECT " & _
                                          "IM.disease_id " & _
                                         ",D.title AS disease_title " & _
                                         ",I.id " & _
                                         ",I.title " & _
                                         ",I.description " & _
                                         ",I.raw_image " & _
                                         ",CAST(0 AS bit) AS raw_exist " & _
                                         ",I.tb_image " & _
                                         ",CAST(0 AS bit) AS tb_exist " & _
                                         ",I.dp_image " & _
                                         ",CAST(0 AS bit) AS dp_exist " & _
                                         ",I.metadata " & _
                                         ",I.status " & _
                                         ",I.created_by " & _
                                         ",I.created_date " & _
                                         ",I.modified_by " & _
                                         ",I.modified_date " & _
                                         ",I.defunct " & _
                                         ",I.reference_text " & _
                                         ",I.ad_image " & _
                                         ",CAST(0 AS bit) AS ad_exist " & _
                                         ",I.code " & _
                                         ",I.type " & _
                                         ",I.NoPrint " & _
                                         ",I.popup_type " & _
                                         ",I.typical " & _
                                         ",IM.img_type " & _
                                      "FROM " & TABLE_NAME & " I " & _
                                            "INNER JOIN " & DaoImgMapping.TABLE_NAME & " IM ON " & _
                                                "IM.image_id = I.id " & _
                                            "INNER JOIN " & DaoDisease.TABLE_NAME & " D ON " & _
                                                "D.id = IM.disease_id "
        Try
            Me.DbManager.ClearCmdParameter()
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
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
                                   ByVal pDiseaseID As Integer, _
                                   Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoImgMapping.TABLE_NAME & " t2 ON " & _
                                                                "t2.image_id = t1.id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                     "{1}" & _
                                                 ")"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty)
            End If
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

#Region "DiseaseIDによる取得(T_JP_Imgmapping.image_type_sequenceによる並び替え)"
    Public Function GetByDiseaseIDForImageList(ByRef pDt As DataTable, _
                                               ByVal pDiseaseID As Integer, _
                                               Optional ByVal pOnlyActive As Boolean = False) As Integer

        Const SQL_QUERY As String = " SELECT distinct t1.*, t2.img_type_sequence " & _
                                    " FROM " & TABLE_NAME & " t1 " & _
                                    " INNER JOIN " & DaoImgMapping.TABLE_NAME & " t2 " & _
                                    " ON t2.image_id = t1.id " & _
                                    " {0}" & _
                                    " WHERE t2.disease_id = @disease_id" & _
                                    " {1}" & _
                                    " ORDER BY t2.img_type_sequence ASC, t1.code ASC"


        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty)
            End If
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

#Region "DiseaseID及びcodeによる取得"
    Public Function GetByDiseaseIdAndCode(ByRef pDt As DataTable, _
                                          ByVal pDiseaseID As Integer, _
                                          ByVal pCode As String, _
                                          Optional ByVal pOnlyActive As Boolean = False) As Integer
        'Const SQL_QUERY As String = "SELECT t1.* " &
        '                              "FROM " & TABLE_NAME & " t1 " &
        '                             "WHERE EXISTS (" &
        '                                            " SELECT *  FROM " & DaoImgMapping.TABLE_NAME & " t2 " &
        '                                            " WHERE t2.disease_id = @disease_id" &
        '                                            " AND t2.image_id = t1.id " &
        '                                            " {0} " &
        '                                            ") " &
        '                              " AND code = @code " & "{1} "
        Const SQL_QUERY As String = "SELECT t1.* " &
                                    ",CAST(ISNULL(imp.NonPublish, 0) AS BIT) AS NonPublish " &
                                      "FROM " & TABLE_NAME & " t1 " &
                                    " INNER JOIN T_JP_ImgMapping t2 ON " &
                                        "t2.image_id=t1.id  " &
                                    " LEFT OUTER JOIN T_JP_Image_Important imp ON " &
                                        "t2.disease_id=imp.disease_id AND " &
                                        "t1.code=imp.code " &
                                           "WHERE t2.disease_id=@disease_id " &
                                      " AND t1.code = @code " & "{0} " & "{1}"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty)
            End If
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)
            Me.DbManager.SetCmdParameter("@code", SqlDbType.VarChar, ParameterDirection.Input, pCode)
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
                            ByVal pID As Integer, _
                            ByVal pOnlyActive As Boolean) As Integer
        'Const SQL_QUERY As String = "SELECT * " &
        '                              "FROM " & TABLE_NAME & " " &
        '                             "WHERE id = @id"

        Const SQL_QUERY As String = "SELECT t1.* " &
                                    ",CAST(ISNULL(imp.NonPublish, 0) AS BIT) AS NonPublish " &
                                      "FROM " & TABLE_NAME & " t1 " &
                                    " INNER JOIN T_JP_ImgMapping t2 ON " &
                                        "t2.image_id=t1.id  " &
                                    " LEFT OUTER JOIN T_JP_Image_Important imp ON " &
                                        "t2.disease_id=imp.disease_id AND " &
                                        "t1.code=imp.code " &
                                           "WHERE t1.id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
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

#Region "検索文字列による曖昧検索"
    ''' <summary>
    ''' 検索文字列による曖昧検索
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>リファクタリングの為、T_JP_Image.metadataのみが検索対象</remarks>
    Public Function GetByTextForMetadata(ByRef pDt As DataTable, ByVal pText As String, ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT i.* " & _
                                    "FROM " & TABLE_NAME & " i " & _
                                    "WHERE EXISTS(" & _
                                    "  SELECT * FROM " & DaoImgMapping.TABLE_NAME & " m " & _
                                    "  INNER JOIN " & DaoDisease.TABLE_NAME & " d " & _
                                    "  ON d.id = m.disease_id " & _
                                    "  {0} " & _
                                    "  " & _
                                    "  WHERE m.image_id = i.id  " & _
                                    "  {1}  ) " & _
                                    "AND i.metadata like @text " & _
                                    "{2} "

        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("d"), " AND " & MyBase.ActiveCondition("m"), " AND " & MyBase.ActiveCondition("i"))
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)

            End If
            If Not String.IsNullOrEmpty(pText) Then pText = "%" & pText & "%"
            Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
            Me.DbManager.SetCmdParameter("@text", SqlDbType.NVarChar, ParameterDirection.Input, pText)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_ImageRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                      "title " & _
                                                                     ",description " & _
                                                                     ",raw_image " & _
                                                                     ",tb_image " & _
                                                                     ",dp_image " & _
                                                                     ",metadata " & _
                                                                     ",status " & _
                                                                     ",created_by " & _
                                                                     ",created_date " & _
                                                                     ",modified_by " & _
                                                                     ",modified_date " & _
                                                                     ",defunct " & _
                                                                     ",reference_text " & _
                                                                     ",ad_image " & _
                                                                     ",code " & _
                                                                     ",type " & _
                                                                     ",NoPrint " & _
                                                                     ",popup_type " & _
                                                                     ",typical " & _
                                                                     "{0}" & _
                                                                   ") VALUES (" & _
                                                                      "@title " & _
                                                                     ",@description " & _
                                                                     ",@raw_image " & _
                                                                     ",@tb_image " & _
                                                                     ",@dp_image " & _
                                                                     ",@metadata " & _
                                                                     ",@status " & _
                                                                     ",@created_by " & _
                                                                     ",@created_date " & _
                                                                     ",@modified_by " & _
                                                                     ",@modified_date " & _
                                                                     ",@defunct " & _
                                                                     ",@reference_text " & _
                                                                     ",@ad_image " & _
                                                                     ",@code " & _
                                                                     ",@type " & _
                                                                     ",@NoPrint " & _
                                                                     ",@popup_type " & _
                                                                     ",@typical " & _
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
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@description", SqlDbType.NText, ParameterDirection.Input, pDr.description)
            Me.DbManager.SetCmdParameter("@raw_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.raw_image)
            Me.DbManager.SetCmdParameter("@tb_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.tb_image)
            Me.DbManager.SetCmdParameter("@dp_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.dp_image)
            Me.DbManager.SetCmdParameter("@metadata", SqlDbType.NText, ParameterDirection.Input, pDr.metadata)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            If Not pDr.IsNull("reference_text") Then
                Me.DbManager.SetCmdParameter("@reference_text", SqlDbType.NText, ParameterDirection.Input, pDr.reference_text)
            Else
                Me.DbManager.SetCmdParameter("@reference_text", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ad_image") Then
                Me.DbManager.SetCmdParameter("@ad_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.ad_image)
            Else
                Me.DbManager.SetCmdParameter("@ad_image", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("type") Then
                Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.type)
            Else
                Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("NoPrint") Then
                Me.DbManager.SetCmdParameter("@NoPrint", SqlDbType.Int, ParameterDirection.Input, pDr.NoPrint)
            Else
                Me.DbManager.SetCmdParameter("@NoPrint", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("popup_type") Then
                Me.DbManager.SetCmdParameter("@popup_type", SqlDbType.VarChar, ParameterDirection.Input, pDr.popup_type)
            Else
                Me.DbManager.SetCmdParameter("@popup_type", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("typical") Then
                Me.DbManager.SetCmdParameter("@typical", SqlDbType.Bit, ParameterDirection.Input, pDr.typical)
            Else
                Me.DbManager.SetCmdParameter("@typical", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim imageID As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = imageID
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_ImageRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "title = @title " & _
                                            ",description = @description " & _
                                            ",raw_image = @raw_image " & _
                                            ",tb_image = @tb_image " & _
                                            ",dp_image = @dp_image " & _
                                            ",metadata = @metadata " & _
                                            ",status = @status " & _
                                            ",created_by = @created_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                            ",defunct = @defunct " & _
                                            ",reference_text = @reference_text " & _
                                            ",ad_image = @ad_image " & _
                                            ",code = @code " & _
                                            ",type = @type " & _
                                            ",NoPrint = @NoPrint " & _
                                            ",popup_type = @popup_type " & _
                                            ",typical = @typical " & _
                                    "WHERE id = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@description", SqlDbType.NText, ParameterDirection.Input, pDr.description)
            Me.DbManager.SetCmdParameter("@raw_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.raw_image)
            Me.DbManager.SetCmdParameter("@tb_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.tb_image)
            Me.DbManager.SetCmdParameter("@dp_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.dp_image)
            Me.DbManager.SetCmdParameter("@metadata", SqlDbType.NText, ParameterDirection.Input, pDr.metadata)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            If Not pDr.IsNull("reference_text") Then
                Me.DbManager.SetCmdParameter("@reference_text", SqlDbType.NText, ParameterDirection.Input, pDr.reference_text)
            Else
                Me.DbManager.SetCmdParameter("@reference_text", SqlDbType.NText, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("ad_image") Then
                Me.DbManager.SetCmdParameter("@ad_image", SqlDbType.VarChar, ParameterDirection.Input, pDr.ad_image)
            Else
                Me.DbManager.SetCmdParameter("@ad_image", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("type") Then
                Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, pDr.type)
            Else
                Me.DbManager.SetCmdParameter("@type", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("NoPrint") Then
                Me.DbManager.SetCmdParameter("@NoPrint", SqlDbType.Int, ParameterDirection.Input, pDr.NoPrint)
            Else
                Me.DbManager.SetCmdParameter("@NoPrint", SqlDbType.Int, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("popup_type") Then
                Me.DbManager.SetCmdParameter("@popup_type", SqlDbType.VarChar, ParameterDirection.Input, pDr.popup_type)
            Else
                Me.DbManager.SetCmdParameter("@popup_type", SqlDbType.VarChar, ParameterDirection.Input, DBNull.Value)
            End If
            If Not pDr.IsNull("typical") Then
                Me.DbManager.SetCmdParameter("@typical", SqlDbType.Bit, ParameterDirection.Input, pDr.typical)
            Else
                Me.DbManager.SetCmdParameter("@typical", SqlDbType.Bit, ParameterDirection.Input, DBNull.Value)
            End If
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
    Public Function UpdateForTypicalAndStatus(ByRef pDr As DS_DISEASE.T_JP_ImageRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                            " status = @status " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                            ",typical = @typical " & _
                                    "WHERE id = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@typical", SqlDbType.Bit, ParameterDirection.Input, pDr.typical)
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
