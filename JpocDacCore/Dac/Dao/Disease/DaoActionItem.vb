Public Class DaoActionItem
    Inherits AbstractTableDao

#Region "定数"
    Public Const TABLE_NAME As String = "T_JP_ActionItem"
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
    Public Function GetCountByPK(ByVal pID As Integer, _
                                 Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT COUNT(*) " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
        Try
            Dim strSql As String = SQL_QUERY
            If pOnlyActive Then strSql &= " AND " & MyBase.ActiveCondition
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
                                                            "INNER JOIN " & DaoDiseaseActionType.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.disease_actiontype_id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                     "{1}" & _
                                                 ")" & _
                                        "{2}"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"), " AND " & MyBase.ActiveCondition)
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)
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

#Region "DiseaseIDによる取得"
    Public Function GetByDiseaseID(ByRef pDt As DataTable, _
                                   ByVal pDiseaseID As Integer, _
                                   Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoDiseaseActionType.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.disease_actiontype_id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                     "{1}" & _
                                                 ")" & _
                                        "{2}"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"), " AND " & MyBase.ActiveCondition)
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)
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

#Region "PrimaryKeyによる取得"
    Public Function GetByPK(ByRef pDt As DataTable, _
                            ByVal pID As Integer, _
                            ByVal pOnlyActive As Boolean) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id = @id"
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

#Region "DiseaseID及びcodeによる取得"
    Public Function GetByDiseaseIdAndCode(ByRef pDt As DataTable, _
                                          ByVal pDiseaseID As Integer, _
                                          ByVal pCode As String, _
                                          Optional ByVal pOnlyActive As Boolean = False) As Integer
        Const SQL_QUERY As String = "SELECT * " & _
                                      "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoDiseaseActionType.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.disease_actiontype_id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                     "{1}" & _
                                                 ") " & _
                                    "AND code = @code " & _
                                        "{2}"
        Try
            Dim strSql As String = String.Empty
            If pOnlyActive Then
                strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"), " AND " & MyBase.ActiveCondition)
            Else
                strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)
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


#Region "DiseaseID及びcodeによる取得"
    Public Function GetTopCodeByDiseaseId(ByVal pDiseaseID As Integer, _
                                          Optional ByVal pOnlyActive As Boolean = False) As String
        Const SQL_QUERY As String = "SELECT TOP 1 * " & _
                                     "FROM " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoDiseaseActionType.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.disease_actiontype_id " & _
                                                                "{0}" & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                     "{1}" & _
                                                 ") " & _
                                    "{2}" & _
                                    " ORDER BY code "
        Try
            Using dt As New Jpoc.Dac.DS_DISEASE.T_JP_ActionItemDataTable

                Dim strSql As String = String.Empty
                If pOnlyActive Then
                    strSql = String.Format(SQL_QUERY, " AND " & MyBase.ActiveCondition("t2"), " AND " & MyBase.ActiveCondition("t1"), " AND " & MyBase.ActiveCondition)
                Else
                    strSql = String.Format(SQL_QUERY, String.Empty, String.Empty, String.Empty)
                End If
                Me.DbManager.SetSqlCommand(strSql, CommandType.Text)
                Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseID)

                Me.DbManager.ExecuteAndGetDataTable(dt, True)
                For Each dr As Jpoc.Dac.DS_DISEASE.T_JP_ActionItemRow In dt.Rows
                    Return dr.code
                Next
                Return String.Empty
            End Using
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
    ''' <summary>
    ''' PKによる削除
    ''' </summary>
    ''' <param name="pID">T_JP_ActionItem.id</param>
    ''' <returns>削除件数</returns>
    ''' <remarks></remarks>
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
    ''' <summary>
    ''' DiseaseIdによる削除
    ''' </summary>
    ''' <param name="pDiseaseId">T_JP_DiseaseId</param>
    ''' <returns>削除件数</returns>
    ''' <remarks></remarks>
    Public Function DeleteByDiseaseId(ByVal pDiseaseId As Integer) As Integer
        Const SQL_QUERY As String = "DELETE " & TABLE_NAME & " " & _
                                     "WHERE id IN (" & _
                                                    "SELECT DISTINCT t1.id " & _
                                                      "FROM " & TABLE_NAME & " t1 " & _
                                                            "INNER JOIN " & DaoDiseaseActionType.TABLE_NAME & " t2 ON " & _
                                                                "t2.id = t1.disease_actiontype_id " & _
                                                     "WHERE t2.disease_id = @disease_id" & _
                                                 ")"
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@disease_id", SqlDbType.Int, ParameterDirection.Input, pDiseaseId)
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
    Public Function Insert(ByRef pDr As DS_DISEASE.T_JP_ActionItemRow, _
                           ByVal pKeepIdValue As Boolean) As Integer
        Const SQL_QUERY As String = "INSERT INTO " & TABLE_NAME & " (" & _
                                                                      "disease_actiontype_id " & _
                                                                     ",sequence " & _
                                                                     ",code " & _
                                                                     ",title " & _
                                                                     ",body " & _
                                                                     ",header " & _
                                                                     ",created_by " & _
                                                                     ",created_date " & _
                                                                     ",modified_by " & _
                                                                     ",modified_date " & _
                                                                     ",defunct " & _
                                                                     ",status " & _
                                                                     "{0}" & _
                                                                   ") VALUES (" & _
                                                                      "@disease_actiontype_id " & _
                                                                     ",@sequence " & _
                                                                     ",@code " & _
                                                                     ",@title " & _
                                                                     ",@body " & _
                                                                     ",@header " & _
                                                                     ",@created_by " & _
                                                                     ",@created_date " & _
                                                                     ",@modified_by " & _
                                                                     ",@modified_date " & _
                                                                     ",@defunct " & _
                                                                     ",@status " & _
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
            Me.DbManager.SetCmdParameter("@disease_actiontype_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_actiontype_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@body", SqlDbType.NText, ParameterDirection.Input, pDr.body)
            Me.DbManager.SetCmdParameter("@header", SqlDbType.Bit, ParameterDirection.Input, pDr.header)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Dim ret As Integer = 0
            If pKeepIdValue Then
                Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
                ret = Me.DbManager.Execute()
            Else
                Dim actionItemID As Integer = Utilities.N0Int(Me.DbManager.ExecuteScalar)
                pDr.id = actionItemID
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
    Public Function Update(ByRef pDr As DS_DISEASE.T_JP_ActionItemRow) As Integer
        Const SQL_QUERY As String = "UPDATE " & TABLE_NAME & " " & _
                                       "SET " & _
                                             "disease_actiontype_id = @disease_actiontype_id " & _
                                            ",sequence = @sequence " & _
                                            ",code = @code " & _
                                            ",title = @title " & _
                                            ",body = @body " & _
                                            ",header = @header " & _
                                            ",created_by = @created_by " & _
                                            ",created_date = @created_date " & _
                                            ",modified_by = @modified_by " & _
                                            ",modified_date = @modified_date " & _
                                            ",defunct = @defunct " & _
                                            ",status = @status " & _
                                    "WHERE id = @id "
        Try
            Me.DbManager.SetSqlCommand(SQL_QUERY, CommandType.Text)
            Me.DbManager.SetCmdParameter("@id", SqlDbType.Int, ParameterDirection.Input, pDr.id)
            Me.DbManager.SetCmdParameter("@disease_actiontype_id", SqlDbType.Int, ParameterDirection.Input, pDr.disease_actiontype_id)
            Me.DbManager.SetCmdParameter("@sequence", SqlDbType.Int, ParameterDirection.Input, pDr.sequence)
            If Not pDr.IsNull("code") Then
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, pDr.code)
            Else
                Me.DbManager.SetCmdParameter("@code", SqlDbType.NVarChar, ParameterDirection.Input, DBNull.Value)
            End If
            Me.DbManager.SetCmdParameter("@title", SqlDbType.NVarChar, ParameterDirection.Input, pDr.title)
            Me.DbManager.SetCmdParameter("@body", SqlDbType.NText, ParameterDirection.Input, pDr.body)
            Me.DbManager.SetCmdParameter("@header", SqlDbType.Bit, ParameterDirection.Input, pDr.header)
            Me.DbManager.SetCmdParameter("@created_by", SqlDbType.Int, ParameterDirection.Input, pDr.created_by)
            Me.DbManager.SetCmdParameter("@created_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.created_date)
            Me.DbManager.SetCmdParameter("@modified_by", SqlDbType.Int, ParameterDirection.Input, pDr.modified_by)
            Me.DbManager.SetCmdParameter("@modified_date", SqlDbType.DateTime, ParameterDirection.Input, pDr.modified_date)
            Me.DbManager.SetCmdParameter("@defunct", SqlDbType.Bit, ParameterDirection.Input, pDr.defunct)
            Me.DbManager.SetCmdParameter("@status", SqlDbType.VarChar, ParameterDirection.Input, pDr.status)
            Return Me.DbManager.Execute()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw ex
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Journal取得"
    ''' <summary>
    ''' Journal取得
    ''' </summary>
    ''' <param name="pConn"></param>
    ''' <param name="pDiseaseID"></param>
    ''' <param name="pJournalType"></param>
    ''' <param name="pDt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetJournal(ByVal pDiseaseID As String,
                               ByVal pJournalType As PublicEnum.eJournalType, ByRef pDt As DataTable) As DataTable
        Const SQL_QUERY_IMAGE_JOURNAL As String = "SELECT " &
                                                         "ROW_NUMBER() OVER(ORDER BY j.sequence) AS rownum " &
                                                        ",jm.id AS journal_map_id " &
                                                        ",jm.parent " &
                                                        ",jm.parent_id " &
                                                        ",j.id AS journal_id " &
                                                        ",j.sequence " &
                                                        ",j.pubmed_id " &
                                                        ",j.title AS original_title " &
                                                        ",j.authors " &
                                                        ",j.citation AS original_citation" &
                                                        ",j.abstract_text " &
                                                        ",j.full_text_links " &
                                                        ",j.publish_date " &
                                                        ",j.status " &
                                                        ",j.fulltext " &
                                                        ",CASE ISNULL(j.pubmed_id,'') " &
                                                            "WHEN '' THEN ISNULL(j.title,'')+ISNULL(j.citation ,'') " &
                                                            "ELSE j.title " &
                                                         "END title " &
                                                        ",CASE ISNULL(j.pubmed_id,'') " &
                                                            "WHEN '' THEN '' " &
                                                            "ELSE j.citation " &
                                                         "END citation " &
                                                  "FROM T_JP_Journal j " &
                                                            "INNER JOIN T_JP_JournalMap jm ON " &
                                                                "jm.journal_id = j.id AND " &
                                                                "jm.parent='image' AND " &
                                                                "jm.defunct = 0 " &
                                                 "WHERE jm.parent_id IN (" &
                                                                          "SELECT " &
                                                                                 "IMAGE.id " &
                                                                            "FROM " &
                                                                                    "T_JP_Image IMAGE " &
                                                                                        " INNER JOIN T_JP_ImgMapping IM ON " &
                                                                                            "IM.image_id= IMAGE.id AND " &
                                                                                            "IM.disease_id=@1 " &
                                                                           "WHERE IMAGE.status IN('A','I') " &
                                                                       ") " &
                                                   "AND j.defunct = 0 " &
                                                   "AND j.status = 'A' " &
                                                 "ORDER BY " &
                                                         "jm.parent_id " &
                                                        ",j.sequence"


        Const SQL_QUERY_EVIDENCE_JOURNAL As String = "SELECT " &
                                                 "ROW_NUMBER() OVER(ORDER BY j.sequence) AS rownum " &
                                                ",jm.id AS journal_map_id " &
                                                ",jm.parent " &
                                                ",jm.parent_id " &
                                                ",j.id AS journal_id " &
                                                ",j.sequence " &
                                                ",j.pubmed_id " &
                                                ",j.title AS original_title " &
                                                ",j.authors " &
                                                ",j.citation AS original_citation " &
                                                ",j.abstract_text " &
                                                ",j.full_text_links " &
                                                ",j.publish_date " &
                                                ",j.status " &
                                                ",j.fulltext " &
                                                ",CASE " &
                                                    "WHEN j.pubmed_id='' AND j.title='' THEN j.citation " &
                                                    "ELSE j.title " &
                                                "END AS title " &
                                                ",CASE " &
                                                    "WHEN j.pubmed_id='' AND j.title='' THEN '' " &
                                                    "ELSE j.citation " &
                                                "END AS citation " &
                                          "FROM T_JP_Journal j " &
                                                    "INNER JOIN T_JP_JournalMap jm ON " &
                                                        "jm.Journal_id =j.id AND " &
                                                        "jm.parent='evidence' AND " &
                                                        "jm.defunct=0 " &
                                                    "INNER JOIN T_JP_EvidenceDisease ed ON " &
                                                        "jm.parent_id=ed.evidence_id AND " &
                                                        "ed.disease_id=@1 " &
                                                    "INNER JOIN T_JP_Evidence e ON " &
                                                        "ed.evidence_id=e.id AND " &
                                                        "e.status='A' AND " &
                                                        "e.defunct=0 " &
                                         "WHERE j.status='A' " &
                                           "AND j.defunct=0 " &
                                         "ORDER BY " &
                                                   "jm.parent_id " &
                                                  ",j.sequence "

        Const SQL_QUERY_STEP1_TOP_JOURNAL As String = "SELECT " &
                                                         "ROW_NUMBER() OVER(ORDER BY j.sequence) AS rownum " &
                                                        ",jm.id AS journal_map_id " &
                                                        ",jm.parent " &
                                                        ",jm.parent_id " &
                                                        ",j.id AS journal_id " &
                                                        ",j.sequence " &
                                                        ",j.pubmed_id " &
                                                        ",j.title AS original_title " &
                                                        ",j.authors " &
                                                        ",j.citation AS original_citation " &
                                                        ",j.abstract_text " &
                                                        ",j.full_text_links " &
                                                        ",j.publish_date " &
                                                        ",j.status " &
                                                        ",j.fulltext " &
                                                        ",CASE " &
                                                            "WHEN j.pubmed_id='' AND j.title='' THEN j.citation " &
                                                            "ELSE  j.title " &
                                                         "END AS title " &
                                                        ",CASE " &
                                                            "WHEN j.pubmed_id='' AND j.title='' THEN '' " &
                                                            "ELSE j.citation " &
                                                         "END AS citation " &
                                                "FROM T_JP_Journal j " &
                                                        "INNER JOIN T_JP_JournalMap jm ON " &
                                                            "jm.Journal_id=j.id AND " &
                                                            "jm.parent in ('step1','top') AND " &
                                                            "jm.defunct=0 " &
                                               "WHERE jm.parent_id=@1 " &
                                                 "AND j.status='A' " &
                                                 "AND j.defunct=0 " &
                                               "ORDER BY " &
                                                           "jm.parent_id " &
                                                          ",j.sequence "
        Try
            Me.DbManager.ClearCmdParameter()

            'JPOC-468
            If pDt Is Nothing Then pDt = New DataTable("Journal")
            If pJournalType = PublicEnum.eJournalType.All OrElse
               pJournalType = PublicEnum.eJournalType.Image Then
                Dim dt As DataTable = New DataTable()
                Me.DbManager.SetSqlCommand(SQL_QUERY_IMAGE_JOURNAL, CommandType.Text)
                Me.DbManager.SetCmdParameter("@1", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDiseaseID))
                Me.DbManager.ExecuteAndGetDataTable(dt, True)
                pDt.Merge(dt)
            End If
            If pJournalType = PublicEnum.eJournalType.All OrElse
               pJournalType = PublicEnum.eJournalType.Evidence Then
                Dim dt As DataTable = New DataTable()
                Me.DbManager.SetSqlCommand(SQL_QUERY_EVIDENCE_JOURNAL, CommandType.Text)
                Me.DbManager.SetCmdParameter("@1", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDiseaseID))
                Me.DbManager.ExecuteAndGetDataTable(dt, True)
                pDt.Merge(dt)
            End If
            If pJournalType = PublicEnum.eJournalType.All OrElse
               pJournalType = PublicEnum.eJournalType.Step1 Then
                Dim dt As DataTable = New DataTable()
                Me.DbManager.SetSqlCommand(SQL_QUERY_STEP1_TOP_JOURNAL, CommandType.Text)
                Me.DbManager.SetCmdParameter("@1", SqlDbType.Int, ParameterDirection.Input, Utilities.N0Int(pDiseaseID))
                Me.DbManager.ExecuteAndGetDataTable(dt, True)
                pDt.Merge(dt)
            End If
            pDt.AcceptChanges()
            Return pDt
        Catch ex As Exception
            GlobalVariables.Logger.Debug(ex.Message, ex)
            Throw ex
        End Try
    End Function
#End Region

End Class
