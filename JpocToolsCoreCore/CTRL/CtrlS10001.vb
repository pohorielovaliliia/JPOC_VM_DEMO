Imports System.IO
Public Class CtrlS10001
    Inherits AbstractCtrl

#Region "インスタンス変数"

#End Region

#Region "プロパティ"
    Public ReadOnly Property dto As DtoS10001
        Get
            Return CType(MyBase.mDto, DtoS10001)
        End Get
    End Property
    Private ReadOnly Property Logic As LogicS10001
        Get
            Return CType(MyBase.mLogic, LogicS10001)
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
            Me.Logic.CheckEntry("AddDisease")
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub
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

#Region "実行"
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
                Try
                    stepCount += 1

                    Me.dto.DiseaseID = Utilities.NZ(drvDisease.Item("DISEASE_ID"))

                    MyBase.PassStep(PublicEnum.eStepEventType.StartEvent, Me.dto.DiseaseID, String.Empty)

                    Dim subStepCount As Integer = 0

                    MyBase.PassStep(PublicEnum.eStepEventType.Message, Me.dto.DiseaseID, "データ取得開始", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount), 2)

                    'WebServiceからデータの取得
                    Me.Logic.GetDiseaseData()
                    If Me.dto.RtnCD <> Common.PublicEnum.eRtnCD.Normal Then
                        Throw New ApplicationException(Me.dto.MessageSet.Message)
                    End If
                    Me.dto.DiseaseDataSet.AcceptChanges()

                    MyBase.PassStep(PublicEnum.eStepEventType.Message, Me.dto.DiseaseID, "データ取得完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, subStepCount, 2)

                    '出力処理
                    'ファイルへの書き出し
                    MyBase.PassStep(PublicEnum.eStepEventType.Message, Me.dto.DiseaseID, "ファイル出力開始", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, MyBase.AddStep(subStepCount), 2)

                    Dim outputFileFullPath As String = Path.Combine(Me.dto.OutputFolderPath, Me.dto.DiseaseID & ".xml")
                    If File.Exists(outputFileFullPath) Then File.Delete(outputFileFullPath)
                    Me.dto.DiseaseDataSet.WriteXml(outputFileFullPath, XmlWriteMode.WriteSchema)

                    MyBase.PassStep(PublicEnum.eStepEventType.Message, Me.dto.DiseaseID, "画像ファイルチェック", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, subStepCount, 2)

                    If Not Me.Logic.ImageDataExists() Then
                        drvDisease.Item("RESULT") = Utilities.GetJobResultName(PublicEnum.eJobResult.Warning)
                        drvDisease.Item("MESSAGE") = "画像データが無いレコードが存在しています"
                        warningExist = True
                    End If

                    MyBase.PassStep(PublicEnum.eStepEventType.Message, Me.dto.DiseaseID, "ファイル出力完了", stepCount, Me.dto.TargetDiseaseTable.Rows.Count, subStepCount, 2)

                    If Me.dto.DiseaseDataSet IsNot Nothing Then
                        Me.dto.DiseaseDataSet.Clear()
                        Me.dto.DiseaseDataSet.Dispose()
                    End If
                    Me.dto.DiseaseDataSet = Nothing

                    drvDisease.Item("RESULT") = Utilities.GetJobResultName(PublicEnum.eJobResult.Success)
                    drvDisease.Item("MESSAGE") = String.Empty

                    If (New IO.FileInfo(outputFileFullPath)).Length = 0 Then
                        drvDisease.Item("RESULT") = Utilities.GetJobResultName(PublicEnum.eJobResult.Warning)
                        drvDisease.Item("MESSAGE") = "0 byte as file."
                        warningExist = True
                    End If

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
                                Me.dto.DiseaseID, _
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
            Return Me.Logic.GetAllDisease()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Function
#End Region

#Region "入力ファイルを全Diseaseと照合しながら読み込む"
    Public Sub ReadDeseaseListFile()
        Try
            '入力チェック＆読み込み
            Me.Logic.CheckEntry("ReadDiseaseListFile")
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

#Region "データ取得"
    Public Sub GetDiseaseData()
        Try
            '入力チェック
            Me.Logic.CheckEntry("")
            If Me.dto.RtnCD <> PublicEnum.eRtnCD.Normal Then Exit Sub

            Me.Logic.GetDiseaseData()
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
#End Region

End Class
