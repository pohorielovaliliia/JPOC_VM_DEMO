Imports System.IO
Public Class LogicS10001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property Dto As DtoS10001
        Get
            Return CType(MyBase.mDto, DtoS10001)
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
                Case "AddDisease"
                    If Me.ExistIdInTargetDiseaseTable(Me.Dto.DiseaseID) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "そのIDは既に存在します。")
                        Exit Sub
                    End If
                    If Not Me.ExistIdInAllDiseaseTable(Me.Dto.DiseaseID) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "そのIDを持つDISEASEが見つかりません。")
                        Exit Sub
                    End If
                    Me.AddTargetDisease(Me.Dto.DiseaseID)
                Case "ReadDiseaseListFile"
                    'ファイル存在チェック
                    If Not File.Exists(Me.Dto.InputFilePath) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0008", "ファイル")
                        Exit Sub
                    End If
                    'ファイル読み込み
                    Dim lines As String() = IO.File.ReadAllLines(Me.Dto.InputFilePath)
                    Try
                        For Each line As String In lines
                            '1行の中にさらにCSV形式で指定されている場合を想定し、行内でも一応分解
                            Dim list As New List(Of String)
                            list = Utilities.SplitCSVStringToList(line)
                            For Each item As String In list '1行の中の1項目ごとに処理
                                Dim str As String = Utilities.NZ(item).Trim
                                str = str.Trim(Chr(&H1A))  'EOF削除
                                If String.IsNullOrEmpty(str) Then Continue For

                                'DiseaseID型チェック(S)
                                '（正の整数のみ許容）
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
                                'DiseaseID型チェック(E)

                                'Disease存在チェック
                                If Not Me.ExistIdInAllDiseaseTable(str) Then
                                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "IDに" & str & "を持つDISEASEが見つかりません。")
                                    Exit Sub
                                End If

                                '既指定チェック
                                If Me.ExistIdInTargetDiseaseTable(str) Then
                                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", "そのIDは既に存在します。")
                                    Exit Sub
                                End If
                                Me.AddTargetDisease(str)
                            Next
                        Next
                    Catch ex As Exception
                        Throw
                    Finally
                        Erase lines
                        lines = Nothing
                    End Try
                Case "Execute"
                    If String.IsNullOrEmpty(Me.Dto.OutputFolderPath) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0002", "出力フォルダ")
                        Exit Sub
                    End If
                    If Not Directory.Exists(Me.Dto.OutputFolderPath) Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0008", "出力フォルダ")
                        Exit Sub
                    End If
                    If Me.Dto.TargetDiseaseTable Is Nothing OrElse Me.Dto.TargetDiseaseTable.Rows.Count = 0 Then
                        Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
                        Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0004", "Disease")
                        Exit Sub
                    End If

                Case Else

            End Select
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            Me.Dto.TargetDiseaseTable.AcceptChanges()
        End Try
    End Sub
#End Region

#Region "TargetDiseaseテーブルへの対象Disease追加"
    Private Sub AddTargetDisease(ByVal pDiseaseID As String)
        Me.Dto.TargetDiseaseTable.ImportRow(Me.Dto.AllDiseaseTable.Rows.Find(Utilities.N0Int(pDiseaseID)))
    End Sub
#End Region

#Region "TargetDiseaseListデータテーブルへのID存在チェック"
    Private Function ExistIdInTargetDiseaseTable(ByVal pDiseaseID As String) As Boolean
        Return Me.Dto.TargetDiseaseTable.Rows.Find(Utilities.N0Int(pDiseaseID)) IsNot Nothing
    End Function
#End Region

#Region "AllDiseaseListデータテーブルへのID存在チェック"
    Private Function ExistIdInAllDiseaseTable(ByVal pDiseaseID As String) As Boolean
        Return Me.Dto.AllDiseaseTable.Rows.Find(Utilities.N0Int(pDiseaseID)) IsNot Nothing
    End Function
#End Region

#Region "WebServiceからDisease一覧取得"
    Public Function GetAllDisease() As Integer
        Dim ret As Integer = 0
        Dim sc As New JpocWeb.EditorService.EditorServiceSoapClient
        Dim data As Byte() = Nothing
        Try
            sc = New JpocWeb.EditorService.EditorServiceSoapClient
            '環境に応じたURLを設定する
            sc.Endpoint.Address = GlobalVariables.WebServiceUrlEndpointAddress(WebServiceUserInfo.GetInstance.ConnectWebServiceName)
            Dim rtnCD As Integer = 0
            Dim rtnMsg As String = String.Empty

            'TODO: FIX HERE
            'data = sc.GetDiseaseList(WebServiceUserInfo.GetInstance.EncryptedUserID, _
            '                         WebServiceUserInfo.GetInstance.EncryptedInstitutionCode, _
            '                         WebServiceUserInfo.GetInstance.EncryptedPassword, _
            '                         rtnCD, _
            '                         rtnMsg)
            Select Case rtnCD
                Case 0
                    Using ds As New DataSet
                        'TODO: FIX HERE
                        'Utilities.DataSetDeserialize(data, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv, ds)
                        Me.Dto.InitAllDiseaseTable()
                        Me.Dto.AllDiseaseTable = ds.Tables(0).Copy
                        Me.Dto.AllDiseaseTable.AcceptChanges()
                        ret = Me.Dto.AllDiseaseTable.Rows.Count
                        ds.Tables(0).Clear()
                        ds.Clear()
                    End Using
                Case Else
                    '3DESで暗号化されたメッセージを復号する
                    'TODO: FIX HERE
                    'rtnMsg = Utilities.Decrypt(rtnMsg, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
                    Throw New Exception(rtnMsg)
            End Select
            Return ret
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            sc.Close()
            sc = Nothing
            Erase data
            data = Nothing
        End Try
    End Function
#End Region

#Region "WebServiceからDisease取得"
    Public Sub GetDiseaseData()
        Dim sc As JpocWeb.EditorService.EditorServiceSoapClient = Nothing
        Dim data As Byte() = Nothing
        Try
            sc = New JpocWeb.EditorService.EditorServiceSoapClient
            '環境に応じたURLを設定する
            sc.Endpoint.Address = GlobalVariables.WebServiceUrlEndpointAddress(WebServiceUserInfo.GetInstance.ConnectWebServiceName)
            Dim rtnCD As Integer = 0
            Dim rtnMsg As String = String.Empty

            'TODO: FIX HERE
            'data = sc.GetDiseaseData(WebServiceUserInfo.GetInstance.EncryptedUserID, _
            '                         WebServiceUserInfo.GetInstance.EncryptedInstitutionCode, _
            '                         WebServiceUserInfo.GetInstance.EncryptedPassword, _
            '                         Utilities.Encrypt(Me.Dto.DiseaseID, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv), _
            '                         Me.Dto.WithoutPopupContents, _
            '                         Me.Dto.OnlyRawImage, _
            '                         True,
            '                         rtnCD, _
            '                         rtnMsg)
            Select Case rtnCD
                Case 0
                    Using ds As New DS_DISEASE
                        'TODO: FIX HERE
                        'Utilities.DataSetDeserialize(data, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv, ds)
                        Me.Dto.DiseaseDataSet = CType(ds.Copy, DS_DISEASE)
                        Me.Dto.DiseaseDataSet.AcceptChanges()
                    End Using
                Case Else
                    '3DESで暗号化されたメッセージを復号する
                    'TODO: FIX HERE
                    'rtnMsg = Utilities.Decrypt(rtnMsg, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
                    Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", rtnMsg)
                    Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError
            End Select
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            sc.Close()
            sc = Nothing
            Erase data
            data = Nothing
        End Try
    End Sub
#End Region

#Region "画像存在チェック"
    ''' <summary>
    ''' T_JP_ImageとT_JP_ImageData（画像のバイナリ情報）の突合
    ''' </summary>
    ''' <returns>全ての画像がある場合True</returns>
    ''' <remarks>All or Nothing チェックで行い、問題がある場合、そこで処理を終了させる</remarks>
    Public Function ImageDataExists() As Boolean
        For Each drImage As DS_DISEASE.T_JP_ImageRow In Me.dto.DiseaseDataSet.T_JP_Image.Rows

            Dim drImageData As DS_DISEASE.T_JP_Image_DataRow = Me.dto.DiseaseDataSet.T_JP_Image_Data.FindByid(drImage.id)
            '画像データが無い場合は処理終了
            If drImageData Is Nothing Then Return False

            '通常の画像の場合は、T_JP_Imageにあるデータに対して4種類のチェック
            If drImage.Ispopup_typeNull OrElse String.IsNullOrEmpty(drImage.popup_type) Then
                'raw_image
                If Not String.IsNullOrEmpty(drImage.raw_image) Then
                    If drImageData.IsNull("raw_image") OrElse
                       drImageData.raw_image.Length = 0 Then
                        '実画像が無い（もしくは取得出来ていない）場合？
                        Return False
                    End If
                End If

                'Raw画像のみの場合はこれだけ
                If Me.Dto.OnlyRawImage Then Continue For

                'tb_image
                If Not String.IsNullOrEmpty(drImage.tb_image) Then
                    If drImageData.IsNull("tb_image") OrElse
                       drImageData.tb_image.Length = 0 Then
                        '実画像が無い（もしくは取得出来ていない）場合？
                        Return False
                    End If
                End If
                'ad_image
                If Not String.IsNullOrEmpty(drImage.ad_image) Then
                    If drImageData.IsNull("ad_image") OrElse
                       drImageData.ad_image.Length = 0 Then
                        '実画像が無い（もしくは取得出来ていない）場合？
                        Return False
                    End If
                End If
                'dp_image
                If Not String.IsNullOrEmpty(drImage.dp_image) Then
                    If drImageData.IsNull("dp_image") OrElse
                       drImageData.dp_image.Length = 0 Then
                        '実画像が無い（もしくは取得出来ていない）場合？
                        Return False
                    End If
                End If
            Else
                'Word用の場合はチェックしない
                If Me.Dto.OnlyRawImage Then Continue For
                'NOTE: Popup無しの場合はチェックしない（DeadLogicだが為念として）
                If Me.Dto.WithoutPopupContents Then Continue For

                'Popup（動画・音声等）の場合は、tbのみ
                'tb_image
                If Not String.IsNullOrEmpty(drImage.tb_image) Then
                    If drImageData.IsNull("tb_image") OrElse
                       drImageData.tb_image.Length = 0 Then
                        '実画像が無い（もしくは取得出来ていない）場合？
                        Return False
                    End If
                End If

            End If
        Next
        Return True
    End Function

#End Region

End Class
