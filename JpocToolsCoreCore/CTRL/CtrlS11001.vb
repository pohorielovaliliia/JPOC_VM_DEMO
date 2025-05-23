Imports System.IO
Public Class CtrlS11001
    Inherits AbstractCtrl

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS11001
        Get
            Return CType(MyBase.mDto, DtoS11001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS11001
        Get
            Return CType(MyBase.mLogic, LogicS11001)
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

#Region "削除実行"
    Public Sub Execute()
        Dim warningExist As Boolean = False
        Dim errorExist As Boolean = False
        Try
            '入力チェック
            Me.Logic.CheckEntry("Execute")
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            'TODO: FIX HERE
            'Me.dto.StartTime = Utilities.GetNowMSec

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
                    Try
                        '指定されたDiseaseの読み込み
                        MyBase.DBManager.ResetConStr(Me.dto.ConString)

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

                    If Not existOldDisease Then
                        Throw New ApplicationException("削除対象が見つかりません。")
                    End If

                    '削除処理
                    Try
                        MyBase.DBManager.ResetConStr(Me.dto.ConString)

                        MyBase.DBManager.Connect()

                        MyBase.DBManager.BeginTransaction()

                        '*** 移行先から古いデータを削除する ***
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

                        MyBase.PassStep(PublicEnum.eStepEventType.Message, diseaseId, "DB 更新完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount))

                        MyBase.DBManager.CommitTransaction()
                    Catch ex As Exception
                        MyBase.DBManager.RollbackTransaction()
                        Throw
                    Finally
                        MyBase.DBManager.DisConnect()
                    End Try

                    '移行先から古い画像・ポップアップ物理ファイルを削除する
                    For Each dr As DS_DISEASE.T_JP_ImageRow In oldDiseaseDataSet.T_JP_Image.Rows
                        If Me.Logic.GetImageType(dr.id, oldDiseaseDataSet).Equals("Popup") Then
                            Me.dto.RootPath = Me.dto.SourcePopupFolderPath
                            'raw削除
                            Me.dto.FileUrl = Me.Logic.GetPopupFileUrl(dr.raw_image)
                            Me.Logic.DeleteImageData()
                            If dr.popup_type.Equals("video") Then
                                Me.dto.RootPath = Me.dto.SourceImageFolderPath
                                Me.dto.FileUrl = Path.GetFileName(dr.tb_image)
                                'tb削除
                                Me.Logic.DeleteImageData()
                            End If
                        Else
                            Me.dto.RootPath = Me.dto.SourceImageFolderPath
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
                MyBase.PassStep(PublicEnum.eStepEventType.EndEvent, _
                                diseaseId, _
                                Utilities.NZ(drvDisease.Item("RESULT")) & vbTab & Utilities.NZ(drvDisease.Item("MESSAGE")))
            Next

            'TODO: FIX HERE
            'Me.dto.EndTime = Utilities.GetNowMSec()

            'If errorExist Then
            '    Me.dto.RtnCD = PublicEnum.eRtnCD.FaitalError
            '    Me.dto.MessageSet = Utilities.GetMessageSet("SYS0000", "処理は終了しましたが致命的エラーとなりロールバックしたレコードがあります。" & vbCrLf & _
            '                                                           "(処理時間：" & Me.dto.ElapsString & ")")
            'ElseIf warningExist Then
            '    Me.dto.RtnCD = PublicEnum.eRtnCD.LogicalError
            '    Me.dto.MessageSet = Utilities.GetMessageSet("WRN0000", "処理は終了しましたがワーニングが発生しロールバックしたレコードがあります。" & vbCrLf & _
            '                                                           "(処理時間：" & Me.dto.ElapsString & ")")
            'Else
            '    Me.dto.RtnCD = PublicEnum.eRtnCD.Normal
            '    Me.dto.MessageSet = Utilities.GetMessageSet("INF0000", "全件正常終了しました。" & vbCrLf & _
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
