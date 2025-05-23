Imports System.IO
Public Class CtrlS05001
    Inherits AbstractCtrl

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS05001
        Get
            Return CType(MyBase.mDto, DtoS05001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS05001
        Get
            Return CType(MyBase.mLogic, LogicS05001)
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Public Sub New(ByRef pDto As AbstractDto)
        MyBase.New(pDto)
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

#Region "対象Disease追加"
    Public Sub AddTargetDisease()
        Try
            If Me.Logic.ExistIdInDiseaseList(Me.dto.CurrentDiseaseID) Then
                Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.dto.MessageSet = Utilities.GetMessageSet("ERR0000", "そのIDは既に存在します。")
                Exit Sub
            End If

            MyBase.DBManager.ResetConStr(Me.dto.ConString)

            MyBase.DBManager.Connect()

            Dim rowCount As Integer = Me.dto.TargetDiseaseTable.Rows.Count

            Me.Logic.GetDiseaseTitle(Me.dto.CurrentDiseaseID)

            If rowCount = Me.dto.TargetDiseaseTable.Rows.Count Then
                Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                Me.dto.MessageSet = Utilities.GetMessageSet("ERR0000", "そのIDを持つDISEASEが見つかりません。")
                Exit Sub
            End If
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            MyBase.DBManager.DisConnect()
        End Try
    End Sub
#End Region

#Region "移行実行"
    Public Sub Execute()
        Dim warningExist As Boolean = False
        Dim errorExist As Boolean = False
        Try
            '入力チェック
            Me.Logic.CheckEntry("Execute")
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub
            If String.IsNullOrEmpty(Me.dto.SourceDataBase) Then '移行元がファイルの場合
                Me.Logic.CheckEntry("ReadInputFiles")
                If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub
                Me.Logic.ReadInputFiles()
            End If

            'TODO: FIX HERE
            'Me.dto.StartTime = Utilities.GetNowMSec

            '*** 全件削除時の特別処理 (S) ***
            If Not String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then    '移行先がDBの場合

                If Me.dto.AllDelete AndAlso Me.dto.CreateBackup Then    '全削除指定の場合

                    Using tmpTagetDiseasetable As DataTable = Me.dto.TargetDiseaseTable.Copy
                        Try
                            Me.dto.InitTargetDiseaseTable()
                            Try
                                MyBase.DBManager.ResetConStr(Me.dto.ConString)

                                MyBase.DBManager.Connect()

                                Me.Logic.GetDiseaseTitle()

                            Catch ex As ElsException
                                ex.AddStackFrame(New StackFrame(True))
                                Throw
                            Catch ex As Exception
                                Throw New ElsException(ex)
                            Finally
                                MyBase.DBManager.DisConnect()
                            End Try

                            '*** 移行先データのバックアップ ***
                            For Each drvDisease As DataRowView In Me.dto.TargetDiseaseView
                                Dim diseaseId As String = Utilities.NZ(drvDisease.Item("DISEASE_ID"))

                                Me.dto.CurrentDiseaseID = Utilities.N0Int(diseaseId)

                                Dim existOldDisease As Boolean = False

                                Try
                                    '指定されたDiseaseの読み込み
                                    MyBase.DBManager.ResetConStr(Me.dto.DistinationConStr)

                                    MyBase.DBManager.Connect()

                                    existOldDisease = Me.Logic.ExistDisease()

                                    If existOldDisease Then
                                        Me.dto.DiseaseDataSet = New DS_DISEASE
                                        Dim cnt As Integer = 0
                                        cnt = Me.Logic.GetDisease()
                                        cnt = Me.Logic.GetDiseaseSubcategoryMap()
                                        cnt = Me.Logic.GetSituation()
                                        cnt = Me.Logic.GetSituationOrderset()
                                        cnt = Me.Logic.GetSituationOrderSetPatientProfile()
                                        cnt = Me.Logic.GetSituationOrderSetSample()
                                        cnt = Me.Logic.GetSituationOrderSetSampleItem()
                                        cnt = Me.Logic.GetJournalMap()
                                        cnt = Me.Logic.GetJournal()
                                        cnt = Me.Logic.GetPatientHandout()
                                        cnt = Me.Logic.GetDecisionDiagram()
                                        cnt = Me.Logic.GetDecisionDiagramExplanation()
                                        cnt = Me.Logic.GetEvidenceDisease()
                                        cnt = Me.Logic.GetEvidence()
                                        cnt = Me.Logic.GetDifferentialDiagnosis()
                                        cnt = Me.Logic.GetImgMapping()
                                        cnt = Me.Logic.GetImage()
                                        cnt = Me.Logic.GetDiseaseActionType()
                                        cnt = Me.Logic.GetActionItem()
                                        cnt = Me.Logic.GetDifferentialDiagnosis_AnswerBox()
                                    End If
                                Catch ex As Exception
                                    Throw
                                Finally
                                    MyBase.DBManager.DisConnect()
                                End Try

                                If existOldDisease Then

                                    '画像ファイルの読み込み
                                    For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                        If Me.Logic.GetImageType(dr.id, Me.dto.DiseaseDataSet).Equals("Popup") Then
                                            Me.dto.RootPath = Me.dto.SourcePopupFolderPath
                                            'raw取得
                                            Me.dto.FileUrl = Me.Logic.GetPopupFileUrl(dr.raw_image)
                                            Dim rawData As Byte() = Me.Logic.GetImageData()
                                            Dim tbData As Byte() = Nothing
                                            If dr.popup_type.Equals("video") Then
                                                Me.dto.RootPath = Me.dto.SourceImageFolderPath
                                                'tb取得
                                                Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                                tbData = Me.Logic.GetImageData()
                                            End If
                                            'レコード追加
                                            Me.Logic.AddImageDataRow(dr.id, rawData, tbData, Nothing, Nothing)
                                        Else
                                            Me.dto.RootPath = Me.dto.SourceImageFolderPath
                                            'raw取得
                                            Me.dto.FileUrl = Path.GetFileName(dr.raw_image)
                                            Dim rawData As Byte() = Me.Logic.GetImageData()
                                            'tb取得
                                            Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                            Dim tbData As Byte() = Me.Logic.GetImageData()
                                            'ad取得
                                            Me.dto.FileUrl = Path.GetFileName(dr.ad_image)
                                            Dim adData As Byte() = Me.Logic.GetImageData()
                                            'dp取得
                                            Me.dto.FileUrl = Path.GetFileName(dr.dp_image)
                                            Dim dpData As Byte() = Me.Logic.GetImageData()
                                            'レコード追加
                                            Me.Logic.AddImageDataRow(dr.id, rawData, tbData, adData, dpData)
                                        End If
                                    Next

                                    'ファイルへの書き出し
                                    If Not String.IsNullOrEmpty(Me.dto.BackupFolderPath) Then
                                        Dim outputFileFullPath As String = Path.Combine(Me.dto.BackupFolderPath, diseaseId & ".xml")
                                        If Me.dto.NeedBase64Encode Then
                                            For Each tbl As DataTable In Me.dto.DiseaseDataSet.Tables
                                                For Each dr As DataRow In tbl.Rows
                                                    For Each col As DataColumn In tbl.Columns
                                                        If col.DataType.Equals(Type.GetType("System.String")) Then
                                                            If Not dr.IsNull(col.ColumnName) Then
                                                                Dim s As String = Utilities.NZ(dr.Item(col.ColumnName))
                                                                s = Utilities.ConvertStringToBase64(s)
                                                                dr.Item(col.ColumnName) = s
                                                            End If
                                                        End If
                                                    Next
                                                Next
                                            Next
                                            Me.dto.DiseaseDataSet.AcceptChanges()
                                        End If
                                        Me.dto.DiseaseDataSet.WriteXml(outputFileFullPath, XmlWriteMode.WriteSchema)
                                    End If
                                    If Me.dto.DiseaseDataSet IsNot Nothing Then
                                        Me.dto.DiseaseDataSet.Clear()
                                        Me.dto.DiseaseDataSet.Dispose()
                                    End If
                                    Me.dto.DiseaseDataSet = Nothing
                                End If
                            Next

                            '*** 移行先から古いデータを削除する ***
                            Try
                                MyBase.DBManager.ResetConStr(Me.dto.DistinationConStr)

                                MyBase.DBManager.Connect()

                                MyBase.DBManager.BeginTransaction()

                                For Each drDisease As DataRow In Me.dto.TargetDiseaseTable.Rows
                                    Dim diseaseId As String = Utilities.NZ(drDisease.Item("DISEASE_ID"))

                                    Me.dto.CurrentDiseaseID = Utilities.N0Int(diseaseId)

                                    Dim existOldDisease As Boolean = False

                                    existOldDisease = Me.Logic.ExistDisease()

                                    If existOldDisease Then
                                        '指定されたDiseaseの読み込み
                                        Me.dto.DiseaseDataSet = New DS_DISEASE
                                        Dim cnt As Integer = 0
                                        cnt = Me.Logic.GetDisease()
                                        cnt = Me.Logic.GetDiseaseSubcategoryMap()
                                        cnt = Me.Logic.GetSituation()
                                        cnt = Me.Logic.GetSituationOrderset()
                                        cnt = Me.Logic.GetSituationOrderSetPatientProfile()
                                        cnt = Me.Logic.GetSituationOrderSetSample()
                                        cnt = Me.Logic.GetSituationOrderSetSampleItem()
                                        cnt = Me.Logic.GetJournalMap()
                                        cnt = Me.Logic.GetJournal()
                                        cnt = Me.Logic.GetPatientHandout()
                                        cnt = Me.Logic.GetDecisionDiagram()
                                        cnt = Me.Logic.GetDecisionDiagramExplanation()
                                        cnt = Me.Logic.GetEvidenceDisease()
                                        cnt = Me.Logic.GetEvidence()
                                        cnt = Me.Logic.GetDifferentialDiagnosis()
                                        cnt = Me.Logic.GetImgMapping()
                                        cnt = Me.Logic.GetImage()
                                        cnt = Me.Logic.GetDiseaseActionType()
                                        cnt = Me.Logic.GetActionItem()

                                        Me.dto.DiseaseDataSet.AcceptChanges()

                                        '*** レコード削除 ***
                                        'T_JP_Journal削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_JournalRow In Me.dto.DiseaseDataSet.T_JP_Journal.Rows
                                            cnt += Me.Logic.DeleteJournal(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_Journal.Rows.Count Then
                                            Throw New ApplicationException("T_JP_Journal 削除件数エラー")
                                        End If

                                        'T_JP_JournalMap削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_JournalMapRow In Me.dto.DiseaseDataSet.T_JP_JournalMap.Rows
                                            cnt += Me.Logic.DeleteJournalMap(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_JournalMap.Rows.Count Then
                                            Throw New ApplicationException("T_JP_JournalMap 削除件数エラー")
                                        End If

                                        'T_JP_Evidence削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_EvidenceRow In Me.dto.DiseaseDataSet.T_JP_Evidence.Rows
                                            cnt += Me.Logic.DeleteEvidence(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_Evidence.Rows.Count Then
                                            Throw New ApplicationException("T_JP_Evidence 削除件数エラー")
                                        End If

                                        'T_JP_EvidenceDisease削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_EvidenceDiseaseRow In Me.dto.DiseaseDataSet.T_JP_EvidenceDisease.Rows
                                            cnt += Me.Logic.DeleteEvidenceDisease(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_EvidenceDisease.Rows.Count Then
                                            Throw New ApplicationException("T_JP_EvidenceDisease 削除件数エラー")
                                        End If

                                        'T_JP_ActionItem削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_ActionItemRow In Me.dto.DiseaseDataSet.T_JP_ActionItem.Rows
                                            cnt += Me.Logic.DeleteActionItem(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_ActionItem.Rows.Count Then
                                            Throw New ApplicationException("T_JP_ActionItem 削除件数エラー")
                                        End If
                                        'T_JP_Prescription削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_PrescriptionRow In Me.dto.DiseaseDataSet.T_JP_Prescription.Rows
                                            cnt += Me.Logic.DeletePrescription(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_Prescription.Rows.Count Then
                                            Throw New ApplicationException("T_JP_Prescription 削除件数エラー")
                                        End If

                                        'T_JP_DiseaseActionType削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_DiseaseActionTypeRow In Me.dto.DiseaseDataSet.T_JP_DiseaseActionType.Rows
                                            cnt += Me.Logic.DeleteDiseaseActionType(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_DiseaseActionType.Rows.Count Then
                                            Throw New ApplicationException("T_JP_DiseaseActionType 削除件数エラー")
                                        End If

                                        'T_JP_DecisionDiagramExplanation削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows
                                            cnt += Me.Logic.DeleteDecisionDiagramExplanation(dr.decisiondiagram_id, dr.sequence)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows.Count Then
                                            Throw New ApplicationException("T_JP_DecisionDiagramExplanation 削除件数エラー")
                                        End If

                                        'T_JP_DecisionDiagram削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_DecisionDiagramRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagram.Rows
                                            cnt += Me.Logic.DeleteDecisionDiagram(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_DecisionDiagram.Rows.Count Then
                                            Throw New ApplicationException("T_JP_DecisionDiagram 削除件数エラー")
                                        End If

                                        'T_JP_Image削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                            cnt += Me.Logic.DeleteImage(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_Image.Rows.Count Then
                                            Throw New ApplicationException("T_JP_Image 削除件数エラー")
                                        End If

                                        'T_JP_ImgMapping削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_ImgMappingRow In Me.dto.DiseaseDataSet.T_JP_ImgMapping.Rows
                                            cnt += Me.Logic.DeleteImgMapping(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_ImgMapping.Rows.Count Then
                                            Throw New ApplicationException("T_JP_ImgMapping 削除件数エラー")
                                        End If

                                        'T_JP_SituationOrderSetPatientProfile削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows
                                            cnt += Me.Logic.DeleteSituationOrderSetPatientProfile(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows.Count Then
                                            Throw New ApplicationException("T_JP_SituationOrderSetPatientProfile 削除件数エラー")
                                        End If

                                        'T_JP_SituationOrderSetSampleItem削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows
                                            cnt += Me.Logic.DeleteSituationOrderSetSampleItem(dr.sample_id, dr.orderset_id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows.Count Then
                                            Throw New ApplicationException("T_JP_SituationOrderSetSampleItem 削除件数エラー")
                                        End If

                                        'T_JP_SituationOrderSetSample削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSample.Rows
                                            cnt += Me.Logic.DeleteSituationOrderSetSample(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSample.Rows.Count Then
                                            Throw New ApplicationException("T_JP_SituationOrderSetSample 削除件数エラー")
                                        End If

                                        'T_JP_SituationOrderSet削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_SituationOrderSetRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSet.Rows
                                            cnt += Me.Logic.DeleteSituationOrderset(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSet.Rows.Count Then
                                            Throw New ApplicationException("T_JP_SituationOrderSet 削除件数エラー")
                                        End If

                                        'T_JP_Situation削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_SituationRow In Me.dto.DiseaseDataSet.T_JP_Situation.Rows
                                            cnt += Me.Logic.DeleteSituation(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_Situation.Rows.Count Then
                                            Throw New ApplicationException("T_JP_Situation 削除件数エラー")
                                        End If

                                        'T_JP_PatientHandout削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_PatientHandoutRow In Me.dto.DiseaseDataSet.T_JP_PatientHandout.Rows
                                            cnt += Me.Logic.DeletePatientHandout(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_PatientHandout.Rows.Count Then
                                            Throw New ApplicationException("T_JP_PatientHandout 削除件数エラー")
                                        End If

                                        'T_JP_DifferentialDiagnosis削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_DifferentialDiagnosisRow In Me.dto.DiseaseDataSet.T_JP_DifferentialDiagnosis.Rows
                                            cnt = Me.Logic.DeleteDifferentialDiagnosis(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_DifferentialDiagnosis.Rows.Count Then
                                            Throw New ApplicationException("T_JP_DifferentialDiagnosis 削除件数エラー")
                                        End If

                                        'T_JP_DiseaseSubcategoryMap削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow In Me.dto.DiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows
                                            cnt += Me.Logic.DeleteDiseaseSubcategoryMap(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows.Count Then
                                            Throw New ApplicationException("T_JP_DiseaseSubcategoryMap 削除件数エラー")
                                        End If

                                        'T_JP_Disease削除
                                        cnt = 0
                                        For Each dr As DS_DISEASE.T_JP_DiseaseRow In Me.dto.DiseaseDataSet.T_JP_Disease.Rows
                                            cnt += Me.Logic.DeleteDisease(dr.id)
                                        Next
                                        If cnt <> Me.dto.DiseaseDataSet.T_JP_Disease.Rows.Count Then
                                            Throw New ApplicationException("T_JP_Disease 削除件数エラー")
                                        End If

                                        '*** 画像削除 ***
                                        For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                            If Me.Logic.GetImageType(dr.id, Me.dto.DiseaseDataSet).Equals("Popup") Then
                                                Me.dto.RootPath = Me.dto.DistinationPopupFolderPath
                                                'raw削除
                                                Me.dto.FileUrl = Me.Logic.GetPopupFileUrl(dr.raw_image)
                                                Me.Logic.DeleteImageData()
                                                If dr.popup_type.Equals("video") Then
                                                    Me.dto.RootPath = Me.dto.DistinationImageFolderPath
                                                    Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                                    'tb削除
                                                    Me.Logic.DeleteImageData()
                                                End If
                                            Else
                                                Me.dto.RootPath = Me.dto.DistinationImageFolderPath
                                                'raw削除
                                                Me.dto.FileUrl = Path.GetFileName(dr.raw_image)
                                                Me.Logic.DeleteImageData()
                                                'tb削除
                                                Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                                Me.Logic.DeleteImageData()
                                                'ad削除
                                                Me.dto.FileUrl = Path.GetFileName(dr.ad_image)
                                                Me.Logic.DeleteImageData()
                                                'dp削除
                                                Me.dto.FileUrl = Path.GetFileName(dr.dp_image)
                                                Me.Logic.DeleteImageData()
                                            End If
                                        Next
                                    End If

                                    If Me.dto.DiseaseDataSet IsNot Nothing Then
                                        Me.dto.DiseaseDataSet.Clear()
                                        Me.dto.DiseaseDataSet.Dispose()
                                    End If
                                    Me.dto.DiseaseDataSet = Nothing
                                Next

                                MyBase.DBManager.CommitTransaction()
                            Catch ex As Exception
                                MyBase.DBManager.RollbackTransaction()
                                Throw
                            Finally
                                MyBase.DBManager.DisConnect()
                            End Try
                        Catch ex As Exception
                            Throw
                        Finally
                            Me.dto.TargetDiseaseTable = tmpTagetDiseasetable.Copy
                        End Try
                    End Using
                End If
            End If
            Me.dto.DiseaseDataSet = Nothing
            '*** 全件削除時の特別処理 (E) ***

            Dim stepCount As Integer = 0
            For Each drvDisease As DataRowView In Me.dto.TargetDiseaseView
                Dim diseaseId As String = String.Empty
                Try
                    Dim oldDiseaseDataSet As DS_DISEASE = Nothing
                    stepCount += 1

                    diseaseId = Utilities.NZ(drvDisease.Item("DISEASE_ID"))

                    MyBase.PassStep(PublicEnum.eStepEventType.StartEvent, diseaseId, String.Empty)

                    Dim subStepCount As Integer = 0

                    Me.dto.CurrentDiseaseID = Utilities.N0Int(diseaseId)

                    Dim existOldDisease As Boolean = False

                    '移行先データのバックアップ
                    If Not String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then
                        '移行先がDBの場合

                        If Not Me.dto.AllDelete Then
                            '全削除以外の場合（全削除の場合はバックアップ済みの為）

                            Try
                                '指定されたDiseaseの読み込み
                                MyBase.DBManager.ResetConStr(Me.dto.DistinationConStr)

                                MyBase.DBManager.Connect()

                                existOldDisease = Me.Logic.ExistDisease()

                                If existOldDisease Then
                                    Me.dto.DiseaseDataSet = New DS_DISEASE
                                    Dim cnt As Integer = 0
                                    cnt = Me.Logic.GetDisease()
                                    cnt = Me.Logic.GetDiseaseSubcategoryMap()
                                    cnt = Me.Logic.GetSituation()
                                    cnt = Me.Logic.GetSituationOrderset()
                                    cnt = Me.Logic.GetSituationOrderSetPatientProfile()
                                    cnt = Me.Logic.GetSituationOrderSetSample()
                                    cnt = Me.Logic.GetSituationOrderSetSampleItem()
                                    cnt = Me.Logic.GetJournalMap()
                                    cnt = Me.Logic.GetJournal()
                                    cnt = Me.Logic.GetPatientHandout()
                                    cnt = Me.Logic.GetDecisionDiagram()
                                    cnt = Me.Logic.GetDecisionDiagramExplanation()
                                    cnt = Me.Logic.GetEvidenceDisease()
                                    cnt = Me.Logic.GetEvidence()
                                    cnt = Me.Logic.GetDifferentialDiagnosis()
                                    cnt = Me.Logic.GetImgMapping()
                                    cnt = Me.Logic.GetImage()
                                    cnt = Me.Logic.GetDiseaseActionType()
                                    cnt = Me.Logic.GetActionItem()
                                End If
                            Catch ex As Exception
                                Throw
                            Finally
                                MyBase.DBManager.DisConnect()
                            End Try

                            If existOldDisease Then

                                '画像ファイルの読み込み
                                For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                    If Me.Logic.GetImageType(dr.id, Me.dto.DiseaseDataSet).Equals("Popup") Then
                                        Me.dto.RootPath = Me.dto.SourcePopupFolderPath
                                        'raw取得
                                        Me.dto.FileUrl = Me.Logic.GetPopupFileUrl(dr.raw_image)
                                        Dim rawData As Byte() = Me.Logic.GetImageData()
                                        Dim tbData As Byte() = Nothing
                                        If dr.popup_type.Equals("video") Then
                                            Me.dto.RootPath = Me.dto.SourceImageFolderPath
                                            'tb取得
                                            Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                            tbData = Me.Logic.GetImageData()
                                        End If
                                        'レコード追加
                                        Me.Logic.AddImageDataRow(dr.id, rawData, tbData, Nothing, Nothing)
                                    Else
                                        Me.dto.RootPath = Me.dto.SourceImageFolderPath
                                        'raw取得
                                        Me.dto.FileUrl = Path.GetFileName(dr.raw_image)
                                        Dim rawData As Byte() = Me.Logic.GetImageData()
                                        'tb取得
                                        Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                        Dim tbData As Byte() = Me.Logic.GetImageData()
                                        'ad取得
                                        Me.dto.FileUrl = Path.GetFileName(dr.ad_image)
                                        Dim adData As Byte() = Me.Logic.GetImageData()
                                        'dp取得
                                        Me.dto.FileUrl = Path.GetFileName(dr.dp_image)
                                        Dim dpData As Byte() = Me.Logic.GetImageData()
                                        'レコード追加
                                        Me.Logic.AddImageDataRow(dr.id, rawData, tbData, adData, dpData)
                                    End If
                                Next

                                'データセット退避
                                oldDiseaseDataSet = DirectCast(Me.dto.DiseaseDataSet.Copy, DS_DISEASE)
                                If Me.dto.DiseaseDataSet IsNot Nothing Then
                                    Me.dto.DiseaseDataSet.Clear()
                                    Me.dto.DiseaseDataSet.Dispose()
                                End If
                                Me.dto.DiseaseDataSet = Nothing

                                'ファイルへの書き出し
                                If Me.dto.CreateBackup AndAlso Not String.IsNullOrEmpty(Me.dto.BackupFolderPath) Then
                                    If Me.dto.NeedBase64Encode Then
                                        For Each tbl As DataTable In oldDiseaseDataSet.Tables
                                            For Each dr As DataRow In tbl.Rows
                                                For Each col As DataColumn In tbl.Columns
                                                    If col.DataType.Equals(Type.GetType("System.String")) Then
                                                        If Not dr.IsNull(col.ColumnName) Then
                                                            Dim s As String = Utilities.NZ(dr.Item(col.ColumnName))
                                                            s = Utilities.ConvertStringToBase64(s)
                                                            dr.Item(col.ColumnName) = s
                                                        End If
                                                    End If
                                                Next
                                            Next
                                        Next
                                        oldDiseaseDataSet.AcceptChanges()
                                    End If
                                    Dim outputFileFullPath As String = Path.Combine(Me.dto.BackupFolderPath, diseaseId & ".xml")
                                    oldDiseaseDataSet.WriteXml(outputFileFullPath, XmlWriteMode.WriteSchema)
                                End If
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "移行先データバックアップ完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                            End If

                        End If
                    End If

                    Dim existNewDisease As Boolean = False

                    '移行元データの取得
                    If Not String.IsNullOrEmpty(Me.dto.SourceDataBase) Then
                        '移行元がファイルではない場合

                        Try
                            '指定されたDiseaseの読み込み
                            MyBase.DBManager.ResetConStr(Me.dto.SourceConStr)

                            MyBase.DBManager.Connect()

                            existNewDisease = Me.Logic.ExistDisease()
                            If existNewDisease Then
                                Me.dto.DiseaseDataSet = New DS_DISEASE
                                Dim cnt As Integer = 0
                                cnt = Me.Logic.GetDisease()
                                cnt = Me.Logic.GetDiseaseSubcategoryMap()
                                cnt = Me.Logic.GetSituation()
                                cnt = Me.Logic.GetSituationOrderset()
                                cnt = Me.Logic.GetSituationOrderSetPatientProfile()
                                cnt = Me.Logic.GetSituationOrderSetSample()
                                cnt = Me.Logic.GetSituationOrderSetSampleItem()
                                cnt = Me.Logic.GetJournalMap()
                                cnt = Me.Logic.GetJournal()
                                cnt = Me.Logic.GetPatientHandout()
                                cnt = Me.Logic.GetDecisionDiagram()
                                cnt = Me.Logic.GetDecisionDiagramExplanation()
                                cnt = Me.Logic.GetEvidenceDisease()
                                cnt = Me.Logic.GetEvidence()
                                cnt = Me.Logic.GetDifferentialDiagnosis()
                                cnt = Me.Logic.GetImgMapping()
                                cnt = Me.Logic.GetImage()
                                cnt = Me.Logic.GetDiseaseActionType()
                                cnt = Me.Logic.GetActionItem()
                                cnt = Me.Logic.GetPrescription()
                            End If
                        Catch ex As Exception
                            Throw
                        Finally
                            MyBase.DBManager.DisConnect()
                        End Try

                        If existNewDisease Then

                            '画像ファイルの読み込み
                            For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                If Me.Logic.GetImageType(dr.id, Me.dto.DiseaseDataSet).Equals("Popup") Then
                                    If Not Me.dto.WithoutPopupContents Then
                                        Me.dto.RootPath = Me.dto.SourcePopupFolderPath
                                        'raw取得
                                        Me.dto.FileUrl = Me.Logic.GetPopupFileUrl(dr.raw_image)
                                        Dim rawData As Byte() = Me.Logic.GetImageData()
                                        Dim tbData As Byte() = Nothing
                                        'If Not Me.dto.OnlyRawImage And String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then
                                        If Not Me.dto.OnlyRawImage Then
                                            If dr.popup_type.Equals("video") Then
                                                Me.dto.RootPath = Me.dto.SourceImageFolderPath
                                                'tb取得
                                                Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                                tbData = Me.Logic.GetImageData()
                                            End If
                                        End If
                                        'レコード追加
                                        Me.Logic.AddImageDataRow(dr.id, rawData, tbData, Nothing, Nothing)
                                    End If
                                Else
                                    Me.dto.RootPath = Me.dto.SourceImageFolderPath
                                    'raw取得
                                    Me.dto.FileUrl = Path.GetFileName(dr.raw_image)
                                    Dim rawData As Byte() = Me.Logic.GetImageData()
                                    'tb取得
                                    Dim tbData As Byte() = Nothing
                                    'If Not Me.dto.OnlyRawImage And String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then
                                    If Not Me.dto.OnlyRawImage Then
                                        Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                        tbData = Me.Logic.GetImageData()
                                    End If
                                    'ad取得
                                    Dim adData As Byte() = Nothing
                                    'If Not Me.dto.OnlyRawImage And String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then
                                    If Not Me.dto.OnlyRawImage Then
                                        Me.dto.FileUrl = Path.GetFileName(dr.ad_image)
                                        adData = Me.Logic.GetImageData()
                                    End If
                                    'dp取得
                                    Dim dpData As Byte() = Nothing
                                    'If Not Me.dto.OnlyRawImage And String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then
                                    If Not Me.dto.OnlyRawImage Then
                                        Me.dto.FileUrl = Path.GetFileName(dr.dp_image)
                                        dpData = Me.Logic.GetImageData()
                                    End If
                                    'レコード追加
                                    Me.Logic.AddImageDataRow(dr.id, rawData, tbData, adData, dpData)
                                End If
                            Next
                            MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "移行元データ取得完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                        End If

                    Else
                        '移行元がファイルの場合
                        For Each item As DS_DISEASE In Me.dto.DiseaseDataSetList
                            If Utilities.N0Int(item.T_JP_Disease.Rows(0).Item("id")) = Utilities.N0Int(diseaseId) Then
                                Me.dto.DiseaseDataSet = CType(item.Copy, DS_DISEASE)
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "移行元ファイル読み込み完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                                existNewDisease = True
                                Exit For
                            End If
                        Next
                    End If

                    If Not String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then
                        '移行先がDBの場合
                        If Me.dto.MatchID AndAlso existOldDisease Then
                            'ID一致指定かつ移行前データが存在する場合
                            'テーブル、件数、IDが全て一致するかチェックし、一致しない場合はエラー
                            If Not Me.Logic.MatchID(oldDiseaseDataSet, Me.dto.DiseaseDataSet) Then
                                Throw New ApplicationException(Me.dto.MessageSet.Message)
                            End If
                        End If
                    End If

                    If Not existNewDisease Then
                        Throw New ApplicationException("読み込み元Diseaseデータが見つかりません。")
                    End If

                    Me.dto.DiseaseDataSet.AcceptChanges()

                    '出力処理
                    If Not String.IsNullOrEmpty(Me.dto.DistinationDataBase) Then
                        '移行先がDBの場合

                        Try
                            MyBase.DBManager.ResetConStr(Me.dto.DistinationConStr)

                            MyBase.DBManager.Connect()

                            MyBase.DBManager.BeginTransaction()

                            If Not Me.dto.MatchID Then
                                'ID一致指定ではない場合

                                '*** 移行先から古いデータを削除する ***
                                If existOldDisease Then

                                    'T_JP_Journal削除
                                    Dim cnt As Integer = 0
                                    For Each dr As DS_DISEASE.T_JP_JournalRow In oldDiseaseDataSet.T_JP_Journal.Rows
                                        cnt += Me.Logic.DeleteJournal(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_Journal.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Journal 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Journal 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_JournalMap削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_JournalMapRow In oldDiseaseDataSet.T_JP_JournalMap.Rows
                                        cnt += Me.Logic.DeleteJournalMap(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_JournalMap.Rows.Count Then
                                        Throw New ApplicationException("T_JP_JournalMap 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "JournalMap 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Evidence削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_EvidenceRow In oldDiseaseDataSet.T_JP_Evidence.Rows
                                        cnt += Me.Logic.DeleteEvidence(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_Evidence.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Evidence 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Evidence 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_EvidenceDisease削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_EvidenceDiseaseRow In oldDiseaseDataSet.T_JP_EvidenceDisease.Rows
                                        cnt += Me.Logic.DeleteEvidenceDisease(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_EvidenceDisease.Rows.Count Then
                                        Throw New ApplicationException("T_JP_EvidenceDisease 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "EvidenceDisease 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_ActionItem削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_ActionItemRow In oldDiseaseDataSet.T_JP_ActionItem.Rows
                                        cnt += Me.Logic.DeleteActionItem(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_ActionItem.Rows.Count Then
                                        Throw New ApplicationException("T_JP_ActionItem 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ActionItem 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Prescription削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_PrescriptionRow In oldDiseaseDataSet.T_JP_Prescription.Rows
                                        cnt += Me.Logic.DeletePrescription(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_Prescription.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Prescription 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ActionItem 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DiseaseActionType削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DiseaseActionTypeRow In oldDiseaseDataSet.T_JP_DiseaseActionType.Rows
                                        cnt += Me.Logic.DeleteDiseaseActionType(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_DiseaseActionType.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DiseaseActionType 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseActionType 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DecisionDiagramExplanation削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow In oldDiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows
                                        cnt += Me.Logic.DeleteDecisionDiagramExplanation(dr.decisiondiagram_id, dr.sequence)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DecisionDiagramExplanation 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagramExplanation 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DecisionDiagram削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DecisionDiagramRow In oldDiseaseDataSet.T_JP_DecisionDiagram.Rows
                                        cnt += Me.Logic.DeleteDecisionDiagram(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_DecisionDiagram.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DecisionDiagram 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagram 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Image削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_ImageRow In oldDiseaseDataSet.T_JP_Image.Rows
                                        cnt += Me.Logic.DeleteImage(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_Image.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Image 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Image 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_ImgMapping削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_ImgMappingRow In oldDiseaseDataSet.T_JP_ImgMapping.Rows
                                        cnt += Me.Logic.DeleteImgMapping(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_ImgMapping.Rows.Count Then
                                        Throw New ApplicationException("T_JP_ImgMapping 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ImgMapping 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSetPatientProfile削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow In oldDiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows
                                        cnt += Me.Logic.DeleteSituationOrderSetPatientProfile(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSetPatientProfile 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetPatientProfile 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSetSampleItem削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow In oldDiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows
                                        cnt += Me.Logic.DeleteSituationOrderSetSampleItem(dr.sample_id, dr.orderset_id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSetSampleItem 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSampleItem 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSetSample削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleRow In oldDiseaseDataSet.T_JP_SituationOrderSetSample.Rows
                                        cnt += Me.Logic.DeleteSituationOrderSetSample(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_SituationOrderSetSample.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSetSample 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSample 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSet削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetRow In oldDiseaseDataSet.T_JP_SituationOrderSet.Rows
                                        cnt += Me.Logic.DeleteSituationOrderset(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_SituationOrderSet.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSet 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSet 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Situation削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationRow In oldDiseaseDataSet.T_JP_Situation.Rows
                                        cnt += Me.Logic.DeleteSituation(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_Situation.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Situation 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Situation 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_PatientHandout削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_PatientHandoutRow In oldDiseaseDataSet.T_JP_PatientHandout.Rows
                                        cnt += Me.Logic.DeletePatientHandout(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_PatientHandout.Rows.Count Then
                                        Throw New ApplicationException("T_JP_PatientHandout 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "PatientHandout 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DifferentialDiagnosis削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DifferentialDiagnosisRow In oldDiseaseDataSet.T_JP_DifferentialDiagnosis.Rows
                                        cnt += Me.Logic.DeleteDifferentialDiagnosis(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_DifferentialDiagnosis.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DifferentialDiagnosis 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DifferentialDiagnosis 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DiseaseSubcategoryMap削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow In oldDiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows
                                        cnt += Me.Logic.DeleteDiseaseSubcategoryMap(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DiseaseSubcategoryMap 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseSubcategoryMap 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Disease削除
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DiseaseRow In oldDiseaseDataSet.T_JP_Disease.Rows
                                        cnt += Me.Logic.DeleteDisease(dr.id)
                                    Next
                                    If cnt <> oldDiseaseDataSet.T_JP_Disease.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Disease 削除件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Disease 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                                End If

                                '*** 移行先に追加する（Insert）***
                                'Diseaseの追加
                                For Each dr As DS_DISEASE.T_JP_DiseaseRow In Me.dto.DiseaseDataSet.T_JP_Disease.Rows
                                    Me.Logic.InsertDisease(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Disease 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'DiseaseSubcategoryMapの追加
                                For Each dr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow In Me.dto.DiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows
                                    Me.Logic.InsertDiseaseSubcategoryMap(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseSubcategoryMap 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'PatientHandoutの追加
                                For Each dr As DS_DISEASE.T_JP_PatientHandoutRow In Me.dto.DiseaseDataSet.T_JP_PatientHandout.Rows
                                    Me.Logic.InsertPatientHandout(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "PatientHandout 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'DifferentialDiagnosisの追加
                                For Each dr As DS_DISEASE.T_JP_DifferentialDiagnosisRow In Me.dto.DiseaseDataSet.T_JP_DifferentialDiagnosis.Rows
                                    Me.Logic.InsertDifferentialDiagnosis(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DifferentialDiagnosis 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'Situationの追加
                                For Each dr As DS_DISEASE.T_JP_SituationRow In Me.dto.DiseaseDataSet.T_JP_Situation.Rows
                                    Me.Logic.InsertSituation(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Situation 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'SituationOrderSetの追加
                                For Each drSituation As DS_DISEASE.T_JP_SituationRow In Me.dto.DiseaseDataSet.T_JP_Situation.Rows

                                    Dim oldSituationId As String = Utilities.NZ(drSituation.Item("id", DataRowVersion.Original))

                                    Dim dvParentSituationOrderSet As New DataView(Me.dto.DiseaseDataSet.T_JP_SituationOrderSet,
                                                                                  "situation_id=" & oldSituationId & " AND parent_id = 0",
                                                                                  String.Empty,
                                                                                  DataViewRowState.OriginalRows)

                                    For Each drvParentSituationOrderSet As DataRowView In dvParentSituationOrderSet
                                        'まず「parent_id=0」だけ追加
                                        drvParentSituationOrderSet.Item("situation_id") = drSituation.id
                                        Me.Logic.InsertSituationOrderset(CType(drvParentSituationOrderSet.Row, DS_DISEASE.T_JP_SituationOrderSetRow))
                                        Dim oldOrdersetId As String = Utilities.NZ(drvParentSituationOrderSet.Row.Item("id", DataRowVersion.Original))
                                        Dim newOrdersetId As Integer = Utilities.N0Int(drvParentSituationOrderSet.Row.Item("id", DataRowVersion.Current))

                                        '「parent_id=0」の親レコードに紐付く「parent_id≠0」を追加
                                        Dim dvChiledSituationOrderSet As New DataView(Me.dto.DiseaseDataSet.T_JP_SituationOrderSet,
                                                                                      "situation_id=" & oldSituationId & " AND parent_id = " & oldOrdersetId,
                                                                                      String.Empty,
                                                                                      DataViewRowState.OriginalRows)

                                        For Each drvChiledSituationOrderSet As DataRowView In dvChiledSituationOrderSet
                                            drvParentSituationOrderSet.Item("situation_id") = drSituation.id
                                            drvParentSituationOrderSet.Item("parent_id") = newOrdersetId
                                            Me.Logic.InsertSituationOrderset(CType(drvChiledSituationOrderSet.Row, DS_DISEASE.T_JP_SituationOrderSetRow))
                                        Next
                                    Next
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderset 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'SituationOrderSetSampleの追加
                                For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSample
                                    Dim oldSituationId As String = Utilities.NZ(dr.situation_id)
                                    Dim dvSituation As New DataView(Me.dto.DiseaseDataSet.T_JP_Situation,
                                                                    "id=" & oldSituationId,
                                                                    String.Empty,
                                                                    DataViewRowState.OriginalRows)
                                    If dvSituation.Count > 0 Then dr.situation_id = Utilities.N0Int(dvSituation.Item(0).Row.Item("id", DataRowVersion.Current))
                                    Me.Logic.InsertSituationOrderSetSample(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSample 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'SituationOrderSetPatientProfileの追加
                                For Each dr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows
                                    Dim oldOrdersetId As String = Utilities.NZ(dr.orderset_id)
                                    Dim dvSituationOrderSet As New DataView(Me.dto.DiseaseDataSet.T_JP_SituationOrderSet,
                                                                            "id=" & oldOrdersetId,
                                                                            String.Empty,
                                                                            DataViewRowState.OriginalRows)
                                    If dvSituationOrderSet.Count > 0 Then
                                        dr.situation_id = Utilities.N0Int(dvSituationOrderSet.Item(0).Row.Item("situation_id", DataRowVersion.Current))
                                        dr.orderset_id = Utilities.N0Int(dvSituationOrderSet.Item(0).Row.Item("id", DataRowVersion.Current))
                                    End If
                                    Me.Logic.InsertSituationOrderSetPatientProfile(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetPatientProfile 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'SituationOrderSetSampleItemの追加
                                For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows
                                    Dim oldSampleId As String = Utilities.NZ(dr.sample_id)
                                    Dim oldOrdersetId As String = Utilities.NZ(dr.orderset_id)
                                    Dim dvSituationOrderSetSample As New DataView(Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSample,
                                                                                  "id=" & oldSampleId,
                                                                                  String.Empty,
                                                                                  DataViewRowState.OriginalRows)
                                    If dvSituationOrderSetSample.Count > 0 Then dr.sample_id = Utilities.N0Int(dvSituationOrderSetSample.Item(0).Row.Item("id", DataRowVersion.Current))
                                    Dim dvSituationOrderSet As New DataView(Me.dto.DiseaseDataSet.T_JP_SituationOrderSet,
                                                                            "id=" & oldOrdersetId,
                                                                            String.Empty,
                                                                            DataViewRowState.OriginalRows)
                                    If dvSituationOrderSet.Count > 0 Then dr.orderset_id = Utilities.N0Int(dvSituationOrderSet.Item(0).Row.Item("id", DataRowVersion.Current))
                                    Me.Logic.InsertSituationOrderSetSampleItem(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSampleItem 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'Evidenceの追加
                                For Each dr As DS_DISEASE.T_JP_EvidenceRow In Me.dto.DiseaseDataSet.T_JP_Evidence.Rows
                                    Me.Logic.InsertEvidence(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Evidence 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'EvidenceDiseaseの追加
                                For Each dr As DS_DISEASE.T_JP_EvidenceDiseaseRow In Me.dto.DiseaseDataSet.T_JP_EvidenceDisease.Rows
                                    Dim oldEvidenceId As String = Utilities.NZ(dr.evidence_id)
                                    Dim dvEvidence As New DataView(Me.dto.DiseaseDataSet.T_JP_Evidence,
                                                                   "id=" & oldEvidenceId,
                                                                   String.Empty,
                                                                   DataViewRowState.OriginalRows)
                                    If dvEvidence.Count > 0 Then dr.evidence_id = Utilities.N0Int(dvEvidence.Item(0).Row.Item("id", DataRowVersion.Current))
                                    Me.Logic.InsertEvidenceDisease(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "EvidenceDisease 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'Imageの追加
                                For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                    Me.Logic.InsertImage(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Image 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'Journalの追加
                                For Each dr As DS_DISEASE.T_JP_JournalRow In Me.dto.DiseaseDataSet.T_JP_Journal.Rows
                                    If dr.evidence_id <> 0 Then
                                        Dim oldEvidenceId As String = Utilities.NZ(dr.evidence_id)
                                        Dim dvEvidence As New DataView(Me.dto.DiseaseDataSet.T_JP_Evidence,
                                                                       "id=" & oldEvidenceId,
                                                                       String.Empty,
                                                                       DataViewRowState.OriginalRows)
                                        If dvEvidence.Count > 0 Then dr.evidence_id = Utilities.N0Int(dvEvidence.Item(0).Row.Item("id", DataRowVersion.Current))
                                    End If
                                    Me.Logic.InsertJournal(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Journal 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'JournalMapの追加
                                For Each dr As DS_DISEASE.T_JP_JournalMapRow In Me.dto.DiseaseDataSet.T_JP_JournalMap.Rows
                                    Dim oldJournalId As String = Utilities.NZ(dr.journal_id)
                                    Dim dvJournal As New DataView(Me.dto.DiseaseDataSet.T_JP_Journal,
                                                                  "id=" & oldJournalId,
                                                                  String.Empty,
                                                                  DataViewRowState.OriginalRows)
                                    'TODO: JournalMapはJournalに紐付かないゴミデータが存在している為
                                    'データが取得できない場合は処理しない
                                    If dvJournal.Count = 0 Then Continue For
                                    dr.journal_id = Utilities.N0Int(dvJournal.Item(0).Row.Item("id", DataRowVersion.Current))
                                    Select Case dr.parent
                                        Case "evidence"
                                            Dim oldEvidenceId As String = Utilities.NZ(dr.parent_id)
                                            Dim dvEvidence As New DataView(Me.dto.DiseaseDataSet.T_JP_Evidence,
                                                                           "id=" & oldEvidenceId,
                                                                           String.Empty,
                                                                           DataViewRowState.OriginalRows)
                                            If dvEvidence.Count > 0 Then dr.parent_id = Utilities.N0Int(dvEvidence.Item(0).Row.Item("id", DataRowVersion.Current))
                                        Case "image"
                                            Dim oldImageId As String = Utilities.NZ(dr.parent_id)
                                            Dim dvImage As New DataView(Me.dto.DiseaseDataSet.T_JP_Image,
                                                                        "id=" & oldImageId,
                                                                        String.Empty,
                                                                        DataViewRowState.OriginalRows)
                                            If dvImage.Count > 0 Then dr.parent_id = Utilities.N0Int(dvImage.Item(0).Row.Item("id", DataRowVersion.Current))
                                        Case "step1"
                                            'DiseaseIDは変わらないので処理なし
                                    End Select
                                    Me.Logic.InsertJournalMap(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "JournalMap 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'DecisionDiagramの追加
                                For Each dr As DS_DISEASE.T_JP_DecisionDiagramRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagram.Rows
                                    If dr.img_id <> 0 Then
                                        Dim oldImageId As String = Utilities.NZ(dr.img_id)
                                        Dim dvImage As New DataView(Me.dto.DiseaseDataSet.T_JP_Image,
                                                                    "id=" & oldImageId,
                                                                    String.Empty,
                                                                    DataViewRowState.OriginalRows)
                                        If dvImage.Count > 0 Then dr.img_id = Utilities.N0Int(dvImage.Item(0).Row.Item("id", DataRowVersion.Current))
                                    End If
                                    If dr.situation_id <> 0 Then
                                        Dim oldSituationId As String = Utilities.NZ(dr.situation_id)
                                        Dim dvSituation As New DataView(Me.dto.DiseaseDataSet.T_JP_Situation,
                                                                        "id=" & oldSituationId,
                                                                        String.Empty,
                                                                        DataViewRowState.OriginalRows)
                                        If dvSituation.Count > 0 Then dr.situation_id = Utilities.N0Int(dvSituation.Item(0).Row.Item("id", DataRowVersion.Current))
                                    End If
                                    Me.Logic.InsertDecisionDiagram(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagram 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'DecisionDiagramExplanationの追加
                                For Each dr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows
                                    Dim oldDecisionDiagramId As String = Utilities.NZ(dr.decisiondiagram_id)
                                    Dim dvDecisionDiagram As New DataView(Me.dto.DiseaseDataSet.T_JP_DecisionDiagram,
                                                                          "id=" & oldDecisionDiagramId,
                                                                          String.Empty,
                                                                          DataViewRowState.OriginalRows)
                                    If dvDecisionDiagram.Count > 0 Then dr.decisiondiagram_id = Utilities.N0Int(dvDecisionDiagram.Item(0).Row.Item("id", DataRowVersion.Current))
                                    Me.Logic.InsertDecisionDiagramExplanation(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagramExplanation 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'DiseaseActionTypeの追加
                                For Each dr As DS_DISEASE.T_JP_DiseaseActionTypeRow In Me.dto.DiseaseDataSet.T_JP_DiseaseActionType.Rows
                                    Me.Logic.InsertDiseaseActionType(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseActionType 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'ActionItemの追加
                                For Each dr As DS_DISEASE.T_JP_ActionItemRow In Me.dto.DiseaseDataSet.T_JP_ActionItem.Rows
                                    Dim oldDiseaseActionTypeId As String = Utilities.NZ(dr.disease_actiontype_id)
                                    Dim dvDiseaseActionType As New DataView(Me.dto.DiseaseDataSet.T_JP_DiseaseActionType,
                                                                            "id=" & oldDiseaseActionTypeId,
                                                                            String.Empty,
                                                                            DataViewRowState.OriginalRows)
                                    If dvDiseaseActionType.Count > 0 Then dr.disease_actiontype_id = Utilities.N0Int(dvDiseaseActionType.Item(0).Row.Item("id", DataRowVersion.Current))
                                    Me.Logic.InsertActionItem(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ActionItem 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'Prescriptionの追加
                                For Each dr As DS_DISEASE.T_JP_PrescriptionRow In Me.dto.DiseaseDataSet.T_JP_Prescription.Rows
                                    Me.Logic.InsertPrescription(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Prescription 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'ImgMappingの追加
                                For Each dr As DS_DISEASE.T_JP_ImgMappingRow In Me.dto.DiseaseDataSet.T_JP_ImgMapping.Rows
                                    Dim oldImageId As String = Utilities.NZ(dr.image_id)
                                    Dim dvImage As New DataView(Me.dto.DiseaseDataSet.T_JP_Image,
                                                                "id=" & oldImageId,
                                                                String.Empty,
                                                                DataViewRowState.OriginalRows)
                                    If dvImage.Count > 0 Then dr.image_id = Utilities.N0Int(dvImage.Item(0).Row.Item("id", DataRowVersion.Current))

                                    Dim oldParentId As String = Utilities.NZ(dr.parent_id)
                                    Select Case dr.img_type
                                        Case "Action"
                                            Dim dvActionItem As New DataView(Me.dto.DiseaseDataSet.T_JP_ActionItem,
                                                                             "id=" & oldParentId,
                                                                             String.Empty,
                                                                             DataViewRowState.OriginalRows)
                                            If dvActionItem.Count > 0 Then dr.parent_id = Utilities.N0Int(dvActionItem.Item(0).Row.Item("id", DataRowVersion.Current))
                                        Case "Evidence"
                                            Dim dvEvidence As New DataView(Me.dto.DiseaseDataSet.T_JP_Evidence,
                                                                           "id=" & oldParentId,
                                                                           String.Empty,
                                                                           DataViewRowState.OriginalRows)
                                            If dvEvidence.Count > 0 Then dr.parent_id = Utilities.N0Int(dvEvidence.Item(0).Row.Item("id", DataRowVersion.Current))
                                        Case "DecisionDiagram"
                                            Dim dvDecisionDiagram As New DataView(Me.dto.DiseaseDataSet.T_JP_DecisionDiagram,
                                                                                  "id=" & oldParentId,
                                                                                  String.Empty,
                                                                                  DataViewRowState.OriginalRows)
                                            If dvDecisionDiagram.Count > 0 Then dr.parent_id = Utilities.N0Int(dvDecisionDiagram.Item(0).Row.Item("id", DataRowVersion.Current))
                                        Case "DifferentialDiagnosis", "Staging"
                                            Dim dvDifferentialDiagnosis As New DataView(Me.dto.DiseaseDataSet.T_JP_DifferentialDiagnosis,
                                                                                        "id=" & oldParentId,
                                                                                        String.Empty,
                                                                                        DataViewRowState.OriginalRows)
                                            If dvDifferentialDiagnosis.Count > 0 Then dr.parent_id = Utilities.N0Int(dvDifferentialDiagnosis.Item(0).Row.Item("id", DataRowVersion.Current))
                                        Case Else 'Disease,Popup
                                            'parent_idがDiseaseIDの為、付け替えなし
                                    End Select
                                    Me.Logic.InsertImgMapping(dr)
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ImgMapping 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                'ImageDataのID値更新
                                For Each dr As DS_DISEASE.T_JP_Image_DataRow In Me.dto.DiseaseDataSet.T_JP_Image_Data.Rows
                                    Dim oldImageId As String = Utilities.NZ(dr.id)
                                    Dim dvImage As New DataView(Me.dto.DiseaseDataSet.T_JP_Image,
                                                                "id=" & oldImageId,
                                                                String.Empty,
                                                                DataViewRowState.OriginalRows)
                                    If dvImage.Count > 0 Then dr.id = Utilities.N0Int(dvImage.Item(0).Row.Item("id", DataRowVersion.Current))
                                Next
                                MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Image_Data ID値更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                            Else
                                'ID一致指定がある場合
                                If existOldDisease Then '移行先に古いデータがいる場合
                                    '*** 移行先を更新する（Update）***
                                    Dim cnt As Integer = 0
                                    For Each dr As DS_DISEASE.T_JP_JournalRow In Me.dto.DiseaseDataSet.T_JP_Journal.Rows
                                        cnt += Me.Logic.UpdateJournal(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_Journal.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Journal 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Journal 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_JournalMap更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_JournalMapRow In Me.dto.DiseaseDataSet.T_JP_JournalMap.Rows
                                        cnt += Me.Logic.UpdateJournalMap(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_JournalMap.Rows.Count Then
                                        Throw New ApplicationException("T_JP_JournalMap 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "JournalMap 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Evidence更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_EvidenceRow In Me.dto.DiseaseDataSet.T_JP_Evidence.Rows
                                        cnt += Me.Logic.UpdateEvidence(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_Evidence.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Evidence 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Evidence 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_EvidenceDisease更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_EvidenceDiseaseRow In Me.dto.DiseaseDataSet.T_JP_EvidenceDisease.Rows
                                        cnt += Me.Logic.UpdateEvidenceDisease(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_EvidenceDisease.Rows.Count Then
                                        Throw New ApplicationException("T_JP_EvidenceDisease 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "EvidenceDisease 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_ActionItem更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_ActionItemRow In Me.dto.DiseaseDataSet.T_JP_ActionItem.Rows
                                        cnt += Me.Logic.UpdateActionItem(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_ActionItem.Rows.Count Then
                                        Throw New ApplicationException("T_JP_ActionItem 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ActionItem 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Prescription更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_PrescriptionRow In Me.dto.DiseaseDataSet.T_JP_Prescription.Rows
                                        cnt += Me.Logic.UpdatePrescription(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_Prescription.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Prescription 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Prescription 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DiseaseActionType更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DiseaseActionTypeRow In Me.dto.DiseaseDataSet.T_JP_DiseaseActionType.Rows
                                        cnt += Me.Logic.UpdateDiseaseActionType(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_DiseaseActionType.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DiseaseActionType 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseActionType 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DecisionDiagramExplanation更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows
                                        cnt += Me.Logic.UpdateDecisionDiagramExplanation(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DecisionDiagramExplanation 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagramExplanation 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DecisionDiagram更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DecisionDiagramRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagram.Rows
                                        cnt += Me.Logic.UpdateDecisionDiagram(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_DecisionDiagram.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DecisionDiagram 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagram 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Image更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                        cnt += Me.Logic.UpdateImage(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_Image.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Image 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Image 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_ImgMapping更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_ImgMappingRow In Me.dto.DiseaseDataSet.T_JP_ImgMapping.Rows
                                        cnt += Me.Logic.UpdateImgMapping(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_ImgMapping.Rows.Count Then
                                        Throw New ApplicationException("T_JP_ImgMapping 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ImgMapping 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSetPatientProfile更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows
                                        cnt += Me.Logic.UpdateSituationOrderSetPatientProfile(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSetPatientProfile 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetPatientProfile 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSetSampleItem更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows
                                        cnt += Me.Logic.UpdateSituationOrderSetSampleItem(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSetSampleItem 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSampleItem 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSetSample更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSample.Rows
                                        cnt += Me.Logic.UpdateSituationOrderSetSample(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSample.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSetSample 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSample 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_SituationOrderSet更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSet.Rows
                                        cnt += Me.Logic.UpdateSituationOrderset(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_SituationOrderSet.Rows.Count Then
                                        Throw New ApplicationException("T_JP_SituationOrderSet 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSet 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Situation更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_SituationRow In Me.dto.DiseaseDataSet.T_JP_Situation.Rows
                                        cnt += Me.Logic.UpdateSituation(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_Situation.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Situation 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Situation 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_PatientHandout更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_PatientHandoutRow In Me.dto.DiseaseDataSet.T_JP_PatientHandout.Rows
                                        cnt += Me.Logic.UpdatePatientHandout(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_PatientHandout.Rows.Count Then
                                        Throw New ApplicationException("T_JP_PatientHandout 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "PatientHandout 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DifferentialDiagnosis更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DifferentialDiagnosisRow In Me.dto.DiseaseDataSet.T_JP_DifferentialDiagnosis.Rows
                                        cnt += Me.Logic.UpdateDifferentialDiagnosis(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_DifferentialDiagnosis.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DifferentialDiagnosis 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DifferentialDiagnosis 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_DiseaseSubcategoryMap更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow In Me.dto.DiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows
                                        cnt += Me.Logic.UpdateDiseaseSubcategoryMap(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows.Count Then
                                        Throw New ApplicationException("T_JP_DiseaseSubcategoryMap 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseSubcategoryMap 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'T_JP_Disease更新
                                    cnt = 0
                                    For Each dr As DS_DISEASE.T_JP_DiseaseRow In Me.dto.DiseaseDataSet.T_JP_Disease.Rows
                                        cnt += Me.Logic.UpdateDisease(dr)
                                    Next
                                    If cnt <> Me.dto.DiseaseDataSet.T_JP_Disease.Rows.Count Then
                                        Throw New ApplicationException("T_JP_Disease 更新件数エラー")
                                    End If
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Disease 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                                Else
                                    '移行先に古いデータがいない場合
                                    '*** 強制的にID指定して移行先にデータを追加する（Insert）***
                                    '*** ID重複エラーになる可能性大 ***
                                    'Diseaseの追加
                                    For Each dr As DS_DISEASE.T_JP_DiseaseRow In Me.dto.DiseaseDataSet.T_JP_Disease.Rows
                                        Me.Logic.InsertDisease(dr)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Disease 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'DiseaseSubcategoryMapの追加
                                    For Each dr As DS_DISEASE.T_JP_DiseaseSubcategoryMapRow In Me.dto.DiseaseDataSet.T_JP_DiseaseSubcategoryMap.Rows
                                        Me.Logic.InsertDiseaseSubcategoryMap(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseSubcategoryMap 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'PatientHandoutの追加
                                    For Each dr As DS_DISEASE.T_JP_PatientHandoutRow In Me.dto.DiseaseDataSet.T_JP_PatientHandout.Rows
                                        Me.Logic.InsertPatientHandout(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "PatientHandout 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'DifferentialDiagnosisの追加
                                    For Each dr As DS_DISEASE.T_JP_DifferentialDiagnosisRow In Me.dto.DiseaseDataSet.T_JP_DifferentialDiagnosis.Rows
                                        Me.Logic.InsertDifferentialDiagnosis(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DifferentialDiagnosis 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'Situationの追加
                                    For Each dr As DS_DISEASE.T_JP_SituationRow In Me.dto.DiseaseDataSet.T_JP_Situation.Rows
                                        Me.Logic.InsertSituation(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Situation 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'SituationOrderSetの追加
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSet.Rows
                                        Me.Logic.InsertSituationOrderset(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderset 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'SituationOrderSetSampleの追加
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSample
                                        Me.Logic.InsertSituationOrderSetSample(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSample 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'SituationOrderSetPatientProfileの追加
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetPatientProfileRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetPatientProfile.Rows
                                        Me.Logic.InsertSituationOrderSetPatientProfile(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetPatientProfile 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'SituationOrderSetSampleItemの追加
                                    For Each dr As DS_DISEASE.T_JP_SituationOrderSetSampleItemRow In Me.dto.DiseaseDataSet.T_JP_SituationOrderSetSampleItem.Rows
                                        Me.Logic.InsertSituationOrderSetSampleItem(dr)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "SituationOrderSetSampleItem 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'Evidenceの追加
                                    For Each dr As DS_DISEASE.T_JP_EvidenceRow In Me.dto.DiseaseDataSet.T_JP_Evidence.Rows
                                        Me.Logic.InsertEvidence(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Evidence 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'EvidenceDiseaseの追加
                                    For Each dr As DS_DISEASE.T_JP_EvidenceDiseaseRow In Me.dto.DiseaseDataSet.T_JP_EvidenceDisease.Rows
                                        Me.Logic.InsertEvidenceDisease(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "EvidenceDisease 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'Imageの追加
                                    For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                                        Me.Logic.InsertImage(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Image 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'ImgMappingの追加
                                    For Each dr As DS_DISEASE.T_JP_ImgMappingRow In Me.dto.DiseaseDataSet.T_JP_ImgMapping.Rows
                                        Me.Logic.InsertImgMapping(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ImgMapping 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'Journalの追加
                                    For Each dr As DS_DISEASE.T_JP_JournalRow In Me.dto.DiseaseDataSet.T_JP_Journal.Rows
                                        Me.Logic.InsertJournal(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Journal 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'JournalMapの追加
                                    For Each dr As DS_DISEASE.T_JP_JournalMapRow In Me.dto.DiseaseDataSet.T_JP_JournalMap.Rows
                                        Me.Logic.InsertJournalMap(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "JournalMap 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'DecisionDiagramの追加
                                    For Each dr As DS_DISEASE.T_JP_DecisionDiagramRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagram.Rows
                                        Me.Logic.InsertDecisionDiagram(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagram 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'DecisionDiagramExplanationの追加
                                    For Each dr As DS_DISEASE.T_JP_DecisionDiagramExplanationRow In Me.dto.DiseaseDataSet.T_JP_DecisionDiagramExplanation.Rows
                                        Me.Logic.InsertDecisionDiagramExplanation(dr)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DecisionDiagramExplanation 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'DiseaseActionTypeの追加
                                    For Each dr As DS_DISEASE.T_JP_DiseaseActionTypeRow In Me.dto.DiseaseDataSet.T_JP_DiseaseActionType.Rows
                                        Me.Logic.InsertDiseaseActionType(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DiseaseActionType 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'ActionItemの追加
                                    For Each dr As DS_DISEASE.T_JP_ActionItemRow In Me.dto.DiseaseDataSet.T_JP_ActionItem.Rows
                                        Me.Logic.InsertActionItem(dr, True)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ActionItem 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                    'Prescriptionの追加
                                    For Each dr As DS_DISEASE.T_JP_PrescriptionRow In Me.dto.DiseaseDataSet.T_JP_Prescription.Rows
                                        Me.Logic.InsertPrescription(dr, False)
                                    Next
                                    MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "Prescription 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                                End If
                            End If

                            MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DB 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                            MyBase.DBManager.CommitTransaction()
                        Catch ex As Exception
                            MyBase.DBManager.RollbackTransaction()
                            Throw
                        Finally
                            MyBase.DBManager.DisConnect()
                        End Try

                        Me.dto.DiseaseDataSet.AcceptChanges()

                        '画像及びポップアップデータファイルを差し替える
                        '移行先から古い画像・ポップアップ物理ファイルを削除する
                        If existOldDisease Then
                            For Each dr As DS_DISEASE.T_JP_ImageRow In oldDiseaseDataSet.T_JP_Image.Rows
                                If Me.Logic.GetImageType(dr.id, oldDiseaseDataSet).Equals("Popup") Then
                                    Me.dto.RootPath = Me.dto.DistinationPopupFolderPath
                                    'raw削除
                                    Me.dto.FileUrl = Me.Logic.GetPopupFileUrl(dr.raw_image)
                                    Me.Logic.DeleteImageData()
                                    If dr.popup_type.Equals("video") Then
                                        Me.dto.RootPath = Me.dto.DistinationImageFolderPath
                                        Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                        'tb削除
                                        Me.Logic.DeleteImageData()
                                    End If
                                Else
                                    Me.dto.RootPath = Me.dto.DistinationImageFolderPath
                                    'raw削除
                                    Me.dto.FileUrl = Path.GetFileName(dr.raw_image)
                                    Me.Logic.DeleteImageData()
                                    'tb削除
                                    Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                    Me.Logic.DeleteImageData()
                                    'ad削除
                                    Me.dto.FileUrl = Path.GetFileName(dr.ad_image)
                                    Me.Logic.DeleteImageData()
                                    'dp削除
                                    Me.dto.FileUrl = Path.GetFileName(dr.dp_image)
                                    Me.Logic.DeleteImageData()
                                End If
                            Next
                            MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "画像・ポップアップ 削除完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                        End If

                        '移行先の画像・ポップアップフォルダに画像・ポップアップデータを追加する
                        For Each dr As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows
                            If Me.Logic.GetImageType(dr.id, Me.dto.DiseaseDataSet).Equals("Popup") Then
                                Me.dto.RootPath = Me.dto.DistinationPopupFolderPath
                                'raw書き出し
                                Me.dto.FileUrl = Me.Logic.GetPopupFileUrl(dr.raw_image)
                                Dim rawData As Byte() = Me.Logic.GetImageData(dr.id, PublicEnum.eImageType.RawImage)
                                Me.Logic.WriteImageData(rawData)

                                If dr.popup_type.Equals("video") Then
                                    'tb書き出し
                                    Me.dto.RootPath = Me.dto.DistinationImageFolderPath
                                    Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                    Dim tbData As Byte() = Me.Logic.GetImageData(dr.id, PublicEnum.eImageType.TbImage)
                                    Me.Logic.WriteImageData(tbData)
                                End If
                            Else
                                Me.dto.RootPath = Me.dto.DistinationImageFolderPath

                                'raw書き出し
                                Me.dto.FileUrl = Path.GetFileName(dr.raw_image)
                                Dim rawData As Byte() = Me.Logic.GetImageData(dr.id, PublicEnum.eImageType.RawImage)
                                Me.Logic.WriteImageData(rawData)

                                'tb書き出し
                                Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                Dim tbData As Byte() = Me.Logic.GetImageData(dr.id, PublicEnum.eImageType.TbImage)
                                Me.Logic.WriteImageData(tbData)

                                'ad書き出し
                                Me.dto.FileUrl = Path.GetFileName(dr.ad_image)
                                Dim adData As Byte() = Me.Logic.GetImageData(dr.id, PublicEnum.eImageType.AdImage)
                                Me.Logic.WriteImageData(adData)

                                'dp書き出し
                                Me.dto.FileUrl = Path.GetFileName(dr.dp_image)
                                Dim dpData As Byte() = Me.Logic.GetImageData(dr.id, PublicEnum.eImageType.DpImage)
                                Me.Logic.WriteImageData(dpData)
                            End If
                        Next
                        MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "画像・ポップアップ 追加完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                    Else
                        If Me.dto.NeedBase64Encode Then
                            For Each tbl As DataTable In Me.dto.DiseaseDataSet.Tables
                                For Each dr As DataRow In tbl.Rows
                                    For Each col As DataColumn In tbl.Columns
                                        If col.DataType.Equals(Type.GetType("System.String")) Then
                                            If Not dr.IsNull(col.ColumnName) Then
                                                Dim s As String = Utilities.NZ(dr.Item(col.ColumnName))
                                                s = Utilities.ConvertStringToBase64(s)
                                                dr.Item(col.ColumnName) = s
                                            End If
                                        End If
                                    Next
                                Next
                            Next
                            Me.dto.DiseaseDataSet.AcceptChanges()
                        End If
                        '移行先がDBではない場合（ファイルへの書き出し）
                        If Not Me.dto.WriteToMemory AndAlso Not String.IsNullOrEmpty(Me.dto.OutputFolderPath) Then
                            Dim outputFileFullPath As String = Path.Combine(Me.dto.OutputFolderPath, diseaseId & ".xml")
                            Me.dto.DiseaseDataSet.WriteXml(outputFileFullPath, XmlWriteMode.WriteSchema)
                            MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "ファイル出力完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))
                        Else
                            Me.dto.DiseaseOutputDataList.Add(diseaseId, CType(Me.dto.DiseaseDataSet.Copy, DS_DISEASE))
                        End If
                    End If

                    If oldDiseaseDataSet IsNot Nothing Then
                        oldDiseaseDataSet.Clear()
                        oldDiseaseDataSet.Dispose()
                    End If
                    oldDiseaseDataSet = Nothing
                    If Me.dto.DiseaseDataSet IsNot Nothing Then
                        Me.dto.DiseaseDataSet.Clear()
                        Me.dto.DiseaseDataSet.Dispose()
                    End If
                    Me.dto.DiseaseDataSet = Nothing

                    drvDisease.Item("RESULT") = Utilities.GetJobResultName(PublicEnum.eJobResult.Success)
                    drvDisease.Item("MESSAGE") = String.Empty
                Catch ex As ApplicationException
                    warningExist = True
                    drvDisease.Item("RESULT") = Utilities.GetJobResultName(PublicEnum.eJobResult.Warning)
                    drvDisease.Item("MESSAGE") = ex.Message
                Catch ex As Exception
                    errorExist = True
                    drvDisease.Item("RESULT") = Utilities.GetJobResultName(PublicEnum.eJobResult.Error)
                    drvDisease.Item("MESSAGE") = ex.Message
                End Try
                MyBase.PassStep(PublicEnum.eStepEventType.EndEvent,
                                diseaseId,
                                Utilities.NZ(drvDisease.Item("RESULT")) & vbTab & Utilities.NZ(drvDisease.Item("MESSAGE")))
            Next

            'TODO: FIX HERE

            'Me.dto.EndTime = Utilities.GetNowMSec()

            'If errorExist Then
            '    Me.dto.RtnCD = PublicEnum.eRtnCD.FaitalError
            '    Me.dto.MessageSet = Utilities.GetMessageSet("SYS0000", "処理は終了しましたが致命的エラーとなりロールバックしたレコードがあります。" & vbCrLf &
            '                                                           "(処理時間：" & Me.dto.ElapsString & ")")
            'ElseIf warningExist Then
            '    Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
            '    Me.dto.MessageSet = Utilities.GetMessageSet("WRN0000", "処理は終了しましたがワーニングが発生しロールバックしたレコードがあります。" & vbCrLf &
            '                                                           "(処理時間：" & Me.dto.ElapsString & ")")
            'Else
            '    Me.dto.RtnCD = PublicEnum.eRtnCD.Normal
            '    Me.dto.MessageSet = Utilities.GetMessageSet("INF0000", "全件正常終了しました。" & vbCrLf &
            '                                                           "(処理時間：" & Me.dto.ElapsString & ")")
            'End If

        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            If Me.dto.DiseaseDataSetList IsNot Nothing Then
                For Each item As DS_DISEASE In Me.dto.DiseaseDataSetList
                    If item IsNot Nothing Then
                        item.Clear()
                        item.Dispose()
                    End If
                    item = Nothing
                Next
                Me.dto.DiseaseDataSetList.Clear()
            End If
            Me.dto.DiseaseDataSetList = Nothing
            If Me.dto.DiseaseDataSet IsNot Nothing Then
                Me.dto.DiseaseDataSet.Clear()
                Me.dto.DiseaseDataSet.Dispose()
            End If
            Me.dto.DiseaseDataSet = Nothing
        End Try
    End Sub
#End Region

#Region "入力ファイル読み込み"
    Public Sub ReadInputFiles()
        Try
            '入力チェック
            Me.Logic.CheckEntry("ReadInputFiles")
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            Me.Logic.ReadInputFiles()

            Me.Logic.ConvertDataToDiseaseList()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "全Disease取得"
    Public Function GetAllDisease() As Integer
        Try
            MyBase.DBManager.ResetConStr(Me.dto.ConString)

            MyBase.DBManager.Connect()

            Return Me.Logic.GetDiseaseTitle()

        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            MyBase.DBManager.DisConnect()
        End Try
    End Function
#End Region

#Region "入力ファイルをDBと照合しながら読み込む"
    Public Sub ReadDeseaseListFile()
        Try
            MyBase.DBManager.ResetConStr(Me.dto.ConString)

            MyBase.DBManager.Connect()

            '入力チェック＆読み込み
            Me.Logic.CheckEntry("ReadDiseaseListFile")
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            MyBase.DBManager.DisConnect()
        End Try
    End Sub
#End Region

End Class
