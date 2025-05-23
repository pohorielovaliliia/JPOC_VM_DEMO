Imports System.IO
Public Class LogicS05001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property Dto As DtoS05001
        Get
            Return CType(MyBase.mDto, DtoS05001)
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDBManager As ElsDataBase, _
                   ByRef pDto As AbstractDto)
        MyBase.New(pDBManager, pDto)
    End Sub
#End Region

#Region "初期化"
    Public Overrides Sub Init()
        Try

        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
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

#Region "入力値チェック"
    Public Sub CheckEntry(ByVal pTargetType As String)

        Try
            Select Case pTargetType
                Case "Execute"
                    If String.IsNullOrEmpty(Me.Dto.SourceDataBase) AndAlso (Not Me.Dto.ReadFromMemory AndAlso String.IsNullOrEmpty(Me.Dto.InputFolderPath)) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "移行元")
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(Me.Dto.DistinationDataBase) AndAlso (Not Me.Dto.WriteToMemory AndAlso String.IsNullOrEmpty(Me.Dto.OutputFolderPath)) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "移行先")
                        Exit Sub
                    End If
                    If Me.Dto.TargetDiseaseTable.Rows.Count = 0 Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "対象Disease")
                        Exit Sub
                    End If
                Case "ReadInputFiles"
                    If String.IsNullOrEmpty(Me.Dto.InputFolderPath) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "入力フォルダ")
                        Exit Sub
                    End If
                Case "ReadDiseaseListFile"

                    Using tbl As DataTable = Me.Dto.TargetDiseaseTable.Copy

                        For Each line As String In Me.Dto.Lines
                            Dim list As New List(Of String)
                            list = Utilities.SplitCSVStringToList(line)
                            For Each item As String In list
                                Dim str As String = Utilities.NZ(item).Trim
                                str = str.Trim(Chr(&H1A))  'EOF削除
                                If String.IsNullOrEmpty(str) Then Continue For
                                If Not IsNumeric(str) OrElse str.Contains(".") Then
                                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0005", "DiseaseID")
                                    Exit Sub
                                End If
                                Dim dec As Decimal = Utilities.N0(str)
                                If Not dec > 0 Then
                                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0006", "DiseaseID")
                                    Exit Sub
                                End If
                                Me.Dto.CurrentDiseaseID = Utilities.N0Int(str)
                                Me.Dto.TargetDiseaseTable.Clear()
                                Me.GetDiseaseTitle(Me.Dto.CurrentDiseaseID)
                                If Me.Dto.TargetDiseaseTable.Rows.Count = 0 Then
                                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "IDに" & str & "を持つDISEASEが見つかりません。")
                                    Exit Sub
                                End If
                                Dim diseaseID As Integer = Utilities.N0Int(Me.Dto.TargetDiseaseTable.Rows(0).Item("DISEASE_ID"))
                                For Each dr As DataRow In tbl.Rows
                                    If Utilities.N0Int(dr.Item("DISEASE_ID")) = diseaseID Then
                                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "ID:" & str & "が重複しています。")
                                        Exit Sub
                                    End If
                                Next
                                tbl.ImportRow(Me.Dto.TargetDiseaseTable.Rows(0))
                            Next
                        Next
                        Me.Dto.TargetDiseaseTable = tbl.Copy
                    End Using
                Case Else

            End Select
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "Popupフォルダパス生成"
    Public Function GetPopupFileUrl(ByVal pRawImagePath As String) As String
        Dim directories() As String = pRawImagePath.ToLower.Split("/"c)
        Dim fileUrl As String = String.Empty
        For Each str As String In directories
            If String.IsNullOrEmpty(str) Then Continue For
            If str.Equals("popup") Then Continue For
            fileUrl &= "/" & str
        Next
        Return fileUrl
    End Function
#End Region

#Region "画像データ取得（ファイルシステムから）"
    Public Overloads Function GetImageData() As Byte()
        Try
            'GlobalVariables.Logger.Debug("EXPORT READ IMAGE DATA: " & Me.Dto.ImageFilePath)
            If String.IsNullOrEmpty(Me.Dto.ImageFilePath) Then Return Nothing
            If Not MyBase.ExistFile Then Return Nothing

            Dim data As Byte() = Nothing
            If Utilities.FileRead(Me.Dto.ImageFilePath, data) Then
                Return data
            End If
            Return Nothing
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "画像データ取得（データテーブルから）"
    Public Overloads Function GetImageData(ByVal pImageId As Integer, _
                                           ByVal pTargetImage As PublicEnum.eImageType) As Byte()
        Try
            If Me.Dto.DiseaseDataSet.T_JP_Image_Data Is Nothing Then Return Nothing
            If pImageId < 0 Then Return Nothing
            If pTargetImage = PublicEnum.eImageType.Undefined Then Return Nothing

            For Each dr As DS_DISEASE.T_JP_Image_DataRow In Me.Dto.DiseaseDataSet.T_JP_Image_Data.Rows
                If dr.id = pImageId Then
                    Select Case pTargetImage
                        Case PublicEnum.eImageType.RawImage
                            Return dr.raw_image
                        Case PublicEnum.eImageType.TbImage
                            Return dr.tb_image
                        Case PublicEnum.eImageType.DpImage
                            Return dr.dp_image
                        Case PublicEnum.eImageType.AdImage
                            Return dr.ad_image
                        Case Else
                            Return Nothing
                    End Select
                    Exit For
                End If
            Next
            Return Nothing
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "画像データ削除"
    Public Sub DeleteImageData()
        Try
            If String.IsNullOrEmpty(Me.Dto.ImageFilePath) Then Exit Sub
            If Not MyBase.ExistFile Then Exit Sub
            File.Delete(Me.Dto.ImageFilePath)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "画像データ書き出し"
    Public Sub WriteImageData(ByVal pImageData() As Byte)
        Try
            If String.IsNullOrEmpty(Me.Dto.ImageFilePath) Then Exit Sub
            If pImageData Is Nothing Then Exit Sub
            If pImageData.Length = 0 Then Exit Sub
            Dim directoryPath As String = Path.GetDirectoryName(Me.Dto.ImageFilePath)
            If Not Directory.Exists(directoryPath) Then Exit Sub
            File.WriteAllBytes(Me.Dto.ImageFilePath, pImageData)
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "ImageType取得"
    Public Function GetImageType(ByVal pImageId As Integer, _
                                 ByVal pDs As DS_DISEASE) As String
        For Each dr As DS_DISEASE.T_JP_ImgMappingRow In pDs.T_JP_ImgMapping.Rows
            If dr.image_id = pImageId Then
                Return dr.img_type
                Exit For
            End If
        Next
        Return String.Empty
    End Function
#End Region

#Region "画像データ設定"
    Public Sub AddImageDataRow(ByVal pImageId As Integer, _
                                    ByVal pRawData As Byte(), _
                                    ByVal pTbData As Byte(), _
                                    ByVal pAdData As Byte(), _
                                    ByVal pDpData As Byte())
        Dim newRow As DS_DISEASE.T_JP_Image_DataRow = CType(Me.Dto.DiseaseDataSet.T_JP_Image_Data.NewRow, DS_DISEASE.T_JP_Image_DataRow)
        newRow.id = pImageId
        If pRawData IsNot Nothing Then
            newRow.raw_image = pRawData
        End If
        If pTbData IsNot Nothing Then
            newRow.tb_image = pTbData
        End If
        If pAdData IsNot Nothing Then
            newRow.ad_image = pAdData
        End If
        If pDpData IsNot Nothing Then
            newRow.dp_image = pDpData
        End If
        Me.Dto.DiseaseDataSet.T_JP_Image_Data.Rows.Add(newRow)
    End Sub
#End Region

#Region "入力データ読み込み"
    Public Sub ReadInputFiles()
        Try
            Dim di As New DirectoryInfo(Me.dto.InputFolderPath)
            Dim fileInfoList() As FileInfo = di.GetFiles("*.xml", SearchOption.TopDirectoryOnly)

            For Each fi As FileInfo In fileInfoList
                Me.Dto.DiseaseDataSet = New DS_DISEASE
                Me.Dto.DiseaseDataSet.ReadXml(fi.FullName)
                If Me.Dto.NeedBase64Encode Then
                    For Each tbl As DataTable In Me.Dto.DiseaseDataSet.Tables
                        For Each dr As DataRow In tbl.Rows
                            For Each col As DataColumn In tbl.Columns
                                If col.DataType.Equals(Type.GetType("System.String")) Then
                                    If Not dr.IsNull(col.ColumnName) Then
                                        Dim s As String = Utilities.NZ(dr.Item(col.ColumnName))
                                        s = Utilities.ConvertBase64ToString(s)
                                        dr.Item(col.ColumnName) = s
                                    End If
                                End If
                            Next
                        Next
                    Next
                    Me.Dto.DiseaseDataSet.AcceptChanges()
                End If
                Me.Dto.DiseaseDataSetList.Add(DirectCast(Me.Dto.DiseaseDataSet.Copy, DS_DISEASE))
                Me.Dto.DiseaseDataSet.Clear()
                Me.Dto.DiseaseDataSet.Dispose()
            Next
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "DiseaseListデータテーブルへのID存在チェック"
    Public Function ExistIdInDiseaseList(ByVal pDiseaseID As Integer) As Boolean
        Return Me.Dto.TargetDiseaseTable.Rows.Find(pDiseaseID) IsNot Nothing
    End Function
#End Region

#Region "DiseaseListへ変換"
    Public Sub ConvertDataToDiseaseList()
        Try
            Me.Dto.InitTargetDiseaseTable()

            For Each ds As DS_DISEASE In Me.Dto.DiseaseDataSetList
                Dim dr As DS_DISEASE.T_JP_DiseaseRow = DirectCast(ds.T_JP_Disease.Rows(0), DS_DISEASE.T_JP_DiseaseRow)
                Dim newRow As DataRow = Me.Dto.TargetDiseaseTable.NewRow
                newRow.Item("DISEASE_ID") = dr.id
                newRow.Item("DISEASE_TITLE") = dr.title
                newRow.Item("RESULT") = DBNull.Value
                newRow.Item("MESSAGE") = DBNull.Value
                Me.Dto.TargetDiseaseTable.Rows.Add(newRow)
            Next
            Me.Dto.TargetDiseaseTable.AcceptChanges()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "テーブル&ID一致チェック"
    Public Function MatchID(ByVal pOldDiseaseSet As DS_DISEASE, _
                            ByVal pNewDiseaseSet As DS_DISEASE) As Boolean
        Try
            For Each tbl As DataTable In pOldDiseaseSet.Tables
                'テーブル存在チェック
                If Not pNewDiseaseSet.Tables.Contains(tbl.TableName) Then
                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "テーブル不一致")
                    Return False
                End If
                If pNewDiseaseSet.Tables(tbl.TableName) Is Nothing Then
                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "テーブル不正")
                    Return False
                End If

                '件数チェック
                If Not pNewDiseaseSet.Tables(tbl.TableName).Rows.Count = tbl.Rows.Count Then
                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "レコード件数不一致")
                    Return False
                End If

                'idチェック
                For Each dr As DataRow In pNewDiseaseSet.Tables(tbl.TableName).Rows
                    If dr.Table.Columns.Contains("id") Then
                        Dim recordExist As Boolean = False
                        For Each newRow As DataRow In tbl.Rows
                            If Utilities.N0Int(dr.Item("id")) = Utilities.N0Int(newRow.Item("id")) Then
                                recordExist = True
                                Exit For
                            End If
                        Next
                        If Not recordExist Then
                            Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                            Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "ID不一致")
                            Return False
                        End If
                    End If
                Next
            Next
            Return True
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DBからDiseaseID一覧取得"
    Public Function GetDiseaseTitle(Optional ByVal pId As Integer = -1) As Integer
        Using dao As New DaoDisease(MyBase.DBManager)
            Return dao.GetTargetDisease(Me.Dto.TargetDiseaseTable, False, pId)
        End Using
    End Function
#End Region

#Region "DBからデータ取得"

#Region "Disease存在チェック"
    Public Function ExistDisease() As Boolean
        Using dao As New DaoDisease(MyBase.DBManager)
            Dim cnt As Integer = 0
            cnt = dao.GetCountByPK(Me.Dto.CurrentDiseaseID)
            Return cnt > 0
        End Using
    End Function
#End Region

#Region "Disease取得"
    Public Function GetDisease() As Integer
        Using dao As New DaoDisease(MyBase.DBManager)
            Return dao.GetByPK(Me.Dto.DiseaseDataSet.T_JP_Disease, _
                               Me.Dto.CurrentDiseaseID, _
                               False)
        End Using
    End Function
#End Region

#Region "DiseaseSubcategoryMap取得"
    Public Function GetDiseaseSubcategoryMap() As Integer
        Using dao As New DaoDiseaseSubcategoryMap(MyBase.DBManager)
            Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_DiseaseSubcategoryMap, _
                                      Me.Dto.CurrentDiseaseID)
        End Using
    End Function
#End Region

#Region "Situation取得"
    Public Function GetSituation() As Integer
        Try
            Using dao As New DaoSituation(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_Situation, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSet取得"
    Public Function GetSituationOrderset() As Integer
        Try
            Using dao As New DaoSituationOrderSet(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_SituationOrderSet, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetPatientProfile取得"
    Public Function GetSituationOrderSetPatientProfile() As Integer
        Try
            Using dao As New DaoSituationOrderSetPatientProfile(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_SituationOrderSetPatientProfile, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetSample取得"
    Public Function GetSituationOrderSetSample() As Integer
        Try
            Using dao As New DaoSituationOrderSetSample(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_SituationOrderSetSample, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetSampleItem取得"
    Public Function GetSituationOrderSetSampleItem() As Integer
        Try
            Using dao As New DaoSituationOrderSetSampleItem(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_SituationOrderSetSampleItem, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "PatientHandout取得"
    Public Function GetPatientHandout() As Integer
        Try
            Using dao As New DaoPatientHandout(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_PatientHandout, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DecisionDiagram取得"
    Public Function GetDecisionDiagram() As Integer
        Try
            Using dao As New DaoDecisionDiagram(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_DecisionDiagram, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DecisionDiagramExplanation取得"
    Public Function GetDecisionDiagramExplanation() As Integer
        Try
            Using dao As New DaoDecisionDiagramExplanation(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_DecisionDiagramExplanation, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "EvidenceDisease取得"
    Public Function GetEvidenceDisease() As Integer
        Try
            Using dao As New DaoEvidenceDisease(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_EvidenceDisease, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Evidence取得"
    Public Function GetEvidence() As Integer
        Try
            Using dao As New DaoEvidence(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_Evidence, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DifferentialDiagnosis取得"
    Public Function GetDifferentialDiagnosis() As Integer
        Try
            Using dao As New DaoDifferentialDiagnosis(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_DifferentialDiagnosis, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ImgMapping取得"
    Public Function GetImgMapping() As Integer
        Try
            Using dao As New DaoImgMapping(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_ImgMapping, _
                                          Me.Dto.CurrentDiseaseID, _
                                          False)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Image取得"
    Public Function GetImage() As Integer
        Try
            Using dao As New DaoImage(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_Image, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DiseaseActionType取得"
    Public Function GetDiseaseActionType() As Integer
        Try
            Using dao As New DaoDiseaseActionType(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_DiseaseActionType, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ActionItem取得"
    Public Function GetActionItem() As Integer
        Try
            Using dao As New DaoActionItem(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_ActionItem, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "JournalMap取得"
    Public Function GetJournalMap() As Integer
        Try
            Using dao As New DaoJournalMap(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_JournalMap, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Journal取得"
    Public Function GetJournal() As Integer
        Try
            Using dao As New DaoJournal(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_Journal, _
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Prescription取得"
    Public Function GetPrescription() As Integer
        Try
            Using dao As New DaoPrescription(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_Prescription,
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region
#End Region

#Region "DBからデータ削除"

#Region "Disease削除"
    Public Function DeleteDisease(ByVal pID As Integer) As Integer
        Using dao As New DaoDisease(MyBase.DBManager)
            Return dao.DeleteById(pID)
        End Using
    End Function
#End Region

#Region "DiseaseSubcategoryMap削除"
    Public Function DeleteDiseaseSubcategoryMap(ByVal pID As Integer) As Integer
        Using dao As New DaoDiseaseSubcategoryMap(MyBase.DBManager)
            Return dao.Delete(pID)
        End Using
    End Function
#End Region

#Region "Situation削除"
    Public Function DeleteSituation(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoSituation(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSet削除"
    Public Function DeleteSituationOrderset(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoSituationOrderSet(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetPatientProfile削除"
    Public Function DeleteSituationOrderSetPatientProfile(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoSituationOrderSetPatientProfile(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetSample削除"
    Public Function DeleteSituationOrderSetSample(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoSituationOrderSetSample(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetSampleItem削除"
    Public Function DeleteSituationOrderSetSampleItem(ByVal pSampleId As Integer, _
                                                      ByVal pOrdersetID As Integer) As Integer
        Try
            Using dao As New DaoSituationOrderSetSampleItem(MyBase.DBManager)
                Return dao.Delete(pSampleId, pOrdersetID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "JournalMap削除"
    Public Function DeleteJournalMap(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoJournalMap(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Journal削除"
    Public Function DeleteJournal(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoJournal(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "PatientHandout削除"
    Public Function DeletePatientHandout(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoPatientHandout(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DecisionDiagram削除"
    Public Function DeleteDecisionDiagram(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoDecisionDiagram(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DecisionDiagramExplanation削除"
    Public Function DeleteDecisionDiagramExplanation(ByVal pDecisiondiagramID As Integer, _
                                                     ByVal pSequence As Integer) As Integer
        Try
            Using dao As New DaoDecisionDiagramExplanation(MyBase.DBManager)
                Return dao.Delete(pDecisiondiagramID, pSequence)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "EvidenceDisease削除"
    Public Function DeleteEvidenceDisease(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoEvidenceDisease(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Evidence削除"
    Public Function DeleteEvidence(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoEvidence(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DifferentialDiagnosis削除"
    Public Function DeleteDifferentialDiagnosis(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoDifferentialDiagnosis(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ImgMapping削除"
    Public Function DeleteImgMapping(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoImgMapping(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Image削除"
    Public Function DeleteImage(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoImage(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DiseaseActionType削除"
    Public Function DeleteDiseaseActionType(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoDiseaseActionType(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ActionItem削除"
    Public Function DeleteActionItem(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoActionItem(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Prescription削除"
    Public Function DeletePrescription(ByVal pID As Integer) As Integer
        Try
            Using dao As New DaoPrescription(MyBase.DBManager)
                Return dao.Delete(pID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region
#End Region

#Region "DBにデータ追加"

#Region "Disease追加"
    Public Sub InsertDisease(ByRef pDr As DS_DISEASE.T_JP_DiseaseRow)
        Using dao As New DaoDisease(MyBase.DBManager)
            dao.Insert(pDr)
        End Using
    End Sub
#End Region

#Region "DiseaseSubcategoryMap追加"
    Public Sub InsertDiseaseSubcategoryMap(ByRef pDr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow, _
                                           Optional ByVal pKeepIdValue As Boolean = False)
        Using dao As New DaoDiseaseSubcategoryMap(MyBase.DBManager)
            dao.Insert(pDr, pKeepIdValue)
        End Using
    End Sub
#End Region

#Region "Situation追加"
    Public Sub InsertSituation(ByRef pDr As DS_DISEASE.T_JP_SituationRow, _
                               Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoSituation(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "SituationOrderSet追加"
    Public Sub InsertSituationOrderset(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetRow, _
                                       Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoSituationOrderSet(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "SituationOrderSetPatientProfile追加"
    Public Sub InsertSituationOrderSetPatientProfile(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow, _
                                                     Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoSituationOrderSetPatientProfile(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "SituationOrderSetSample追加"
    Public Sub InsertSituationOrderSetSample(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleRow, _
                                             Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoSituationOrderSetSample(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "SituationOrderSetSampleItem追加"
    Public Sub InsertSituationOrderSetSampleItem(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow)
        Try
            Using dao As New DaoSituationOrderSetSampleItem(MyBase.DBManager)
                dao.Insert(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "PatientHandout追加"
    Public Sub InsertPatientHandout(ByRef pDr As DS_DISEASE.T_JP_PatientHandoutRow, _
                                    Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoPatientHandout(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "DecisionDiagram追加"
    Public Sub InsertDecisionDiagram(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramRow, _
                                     Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoDecisionDiagram(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "DecisionDiagramExplanation追加"
    Public Sub InsertDecisionDiagramExplanation(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow)
        Try
            Using dao As New DaoDecisionDiagramExplanation(MyBase.DBManager)
                dao.Insert(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "EvidenceDisease追加"
    Public Sub InsertEvidenceDisease(ByRef pDr As DS_DISEASE.T_JP_EvidenceDiseaseRow, _
                                     Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoEvidenceDisease(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "Evidence追加"
    Public Sub InsertEvidence(ByRef pDr As DS_DISEASE.T_JP_EvidenceRow, _
                              Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoEvidence(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "DifferentialDiagnosis追加"
    Public Sub InsertDifferentialDiagnosis(ByRef pDr As DS_DISEASE.T_JP_DifferentialDiagnosisRow, _
                                           Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoDifferentialDiagnosis(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "ImgMapping追加"
    Public Sub InsertImgMapping(ByRef pDr As DS_DISEASE.T_JP_ImgMappingRow, _
                                Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoImgMapping(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "Image追加"
    Public Sub InsertImage(ByRef pDr As DS_DISEASE.T_JP_ImageRow, _
                           Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoImage(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "DiseaseActionType追加"
    Public Sub InsertDiseaseActionType(ByRef pDr As DS_DISEASE.T_JP_DiseaseActionTypeRow, _
                                       Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoDiseaseActionType(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "ActionItem追加"
    Public Sub InsertActionItem(ByRef pDr As DS_DISEASE.T_JP_ActionItemRow, _
                                Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoActionItem(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "Prescription追加"
    Public Sub InsertPrescription(ByRef pDr As DS_DISEASE.T_JP_PrescriptionRow,
                                Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoPrescription(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "Journal追加"
    Public Sub InsertJournal(ByRef pDr As DS_DISEASE.T_JP_JournalRow, _
                             Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoJournal(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "JournalMap追加"
    Public Sub InsertJournalMap(ByRef pDr As DS_DISEASE.T_JP_JournalMapRow, _
                                Optional ByVal pKeepIdValue As Boolean = False)
        Try
            Using dao As New DaoJournalMap(MyBase.DBManager)
                dao.Insert(pDr, pKeepIdValue)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#End Region

#Region "DBデータ更新"

#Region "Disease更新"
    Public Function UpdateDisease(ByRef pDr As DS_DISEASE.T_JP_DiseaseRow) As Integer
        Using dao As New DaoDisease(MyBase.DBManager)
            Return dao.Update(pDr)
        End Using
    End Function
#End Region

#Region "DiseaseSubcategoryMap更新"
    Public Function UpdateDiseaseSubcategoryMap(ByRef pDr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow) As Integer
        Using dao As New DaoDiseaseSubcategoryMap(MyBase.DBManager)
            Return dao.Update(pDr)
        End Using
    End Function
#End Region

#Region "Situation更新"
    Public Function UpdateSituation(ByRef pDr As DS_DISEASE.T_JP_SituationRow) As Integer
        Try
            Using dao As New DaoSituation(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSet更新"
    Public Function UpdateSituationOrderset(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetRow) As Integer
        Try
            Using dao As New DaoSituationOrderSet(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetPatientProfile更新"
    Public Function UpdateSituationOrderSetPatientProfile(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow) As Integer
        Try
            Using dao As New DaoSituationOrderSetPatientProfile(MyBase.DBManager)
                Return dao.UpdateSituationOrderSetPatientProfile(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetSample更新"
    Public Function UpdateSituationOrderSetSample(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleRow) As Integer
        Try
            Using dao As New DaoSituationOrderSetSample(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "SituationOrderSetSampleItem更新"
    Public Function UpdateSituationOrderSetSampleItem(ByRef pDr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow) As Integer
        Try
            Using dao As New DaoSituationOrderSetSampleItem(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "PatientHandout更新"
    Public Function UpdatePatientHandout(ByRef pDr As DS_DISEASE.T_JP_PatientHandoutRow) As Integer
        Try
            Using dao As New DaoPatientHandout(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DecisionDiagram更新"
    Public Function UpdateDecisionDiagram(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramRow) As Integer
        Try
            Using dao As New DaoDecisionDiagram(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DecisionDiagramExplanation更新"
    Public Function UpdateDecisionDiagramExplanation(ByRef pDr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow) As Integer
        Try
            Using dao As New DaoDecisionDiagramExplanation(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "EvidenceDisease更新"
    Public Function UpdateEvidenceDisease(ByRef pDr As DS_DISEASE.T_JP_EvidenceDiseaseRow) As Integer
        Try
            Using dao As New DaoEvidenceDisease(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Evidence更新"
    Public Function UpdateEvidence(ByRef pDr As DS_DISEASE.T_JP_EvidenceRow) As Integer
        Try
            Using dao As New DaoEvidence(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DifferentialDiagnosis更新"
    Public Function UpdateDifferentialDiagnosis(ByRef pDr As DS_DISEASE.T_JP_DifferentialDiagnosisRow) As Integer
        Try
            Using dao As New DaoDifferentialDiagnosis(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ImgMapping更新"
    Public Function UpdateImgMapping(ByRef pDr As DS_DISEASE.T_JP_ImgMappingRow) As Integer
        Try
            Using dao As New DaoImgMapping(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Image更新"
    Public Function UpdateImage(ByRef pDr As DS_DISEASE.T_JP_ImageRow) As Integer
        Try
            Using dao As New DaoImage(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "DiseaseActionType更新"
    Public Function UpdateDiseaseActionType(ByRef pDr As DS_DISEASE.T_JP_DiseaseActionTypeRow) As Integer
        Try
            Using dao As New DaoDiseaseActionType(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "ActionItem更新"
    Public Function UpdateActionItem(ByRef pDr As DS_DISEASE.T_JP_ActionItemRow) As Integer
        Try
            Using dao As New DaoActionItem(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Prescription更新"
    Public Function UpdatePrescription(ByRef pDr As DS_DISEASE.T_JP_PrescriptionRow) As Integer
        Try
            Using dao As New DaoPrescription(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "JournalMap更新"
    Public Function UpdateJournalMap(ByRef pDr As DS_DISEASE.T_JP_JournalMapRow) As Integer
        Try
            Using dao As New DaoJournalMap(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "Journal更新"
    Public Function UpdateJournal(ByRef pDr As DS_DISEASE.T_JP_JournalRow) As Integer
        Try
            Using dao As New DaoJournal(MyBase.DBManager)
                Return dao.Update(pDr)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#End Region

    'JPOC-1451
#Region "DifferentialDiagnosis_AnswerBox取得"
    Public Function GetDifferentialDiagnosis_AnswerBox() As Integer
        Try
            Using dao As New DaoDifferentialDiagnosis_AnswerBox(MyBase.DBManager)
                Return dao.GetByDiseaseID(Me.Dto.DiseaseDataSet.T_JP_DifferentialDiagnosis_AnswerBox,
                                          Me.Dto.CurrentDiseaseID)
            End Using
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region
End Class
