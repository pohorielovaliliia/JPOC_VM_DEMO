Imports System.IO
Public Class LogicS09001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property Dto As DtoS09001
        Get
            Return CType(MyBase.mDto, DtoS09001)
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

#Region "データセット取得コマンド実行"
    Public Sub Execute()
        Dim sc As JpocWeb.EditorService.EditorServiceSoapClient = Nothing
        Dim data As Byte() = Nothing
        Try
            Dim rtnCd As Integer = 0
            Dim rtnMsg As String = String.Empty
            sc = New JpocWeb.EditorService.EditorServiceSoapClient
            '環境に応じたURLを設定する
            sc.Endpoint.Address = GlobalVariables.WebServiceUrlEndpointAddress(WebServiceUserInfo.GetInstance.ConnectWebServiceName)

            'TODO: FIX HERE
            'data = sc.GetDataToDataSet(WebServiceUserInfo.GetInstance.EncryptedUserID, _
            '                           WebServiceUserInfo.GetInstance.EncryptedInstitutionCode, _
            '                           WebServiceUserInfo.GetInstance.EncryptedPassword, _
            '                           Utilities.Encrypt(Me.Dto.SqlCommand, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv), _
            '                           rtnCd, _
            '                           rtnMsg)
            If rtnCd = 0 Then   'Webサービスが正常に終了した場合
                '受け取ったデータをGZIP伸張する
                data = Utilities.GZipDecompressByte(data)
                '伸張したデータを3DESで復号する

                'TODO: FIX HERE
                'data = Utilities.Decrypt(data, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
                'バイト配列をメモリーストリームに変換する

                'TODO: FIX HERE
                'Using ms As MemoryStream = Utilities.ConvertByteToStream(data)
                'メモリーストリームからデシリアライズしてデータセットに読み込む
                '    Me.Dto.WorkDataSet.ReadXml(ms, XmlReadMode.ReadSchema)
                'End Using
            Else                'Webサービス側でエラーになった場合
                'こちら側では論理エラーとして扱う
                Me.Dto.RtnCD = PublicEnum.eRtnCD.LogicalError

                'TODO: FIX HERE
                '3DESで暗号化されたメッセージを復号する
                'rtnMsg = Utilities.Decrypt(rtnMsg, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)

                'メッセージセットを格納する
                Me.Dto.MessageSet = Utilities.GetMessageSet("ERR0000", rtnMsg)
            End If
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            Erase data
            data = Nothing
            sc.Close()
            sc = Nothing
        End Try
    End Sub
#End Region

#Region "Sql Command ファイル取得"
    Public Sub ReadSqlCommandFiles(ByVal pList As List(Of IO.FileInfo))
        Dim i As Integer = 1
        For Each fi As FileInfo In pList
            Dim contents As String() = IO.File.ReadAllLines(fi.FullName, System.Text.Encoding.UTF8)
            Dim fileName As String = String.Empty
            Dim sqlCommand As String = String.Empty
            Try
                If contents.Length < 2 Then Continue For
                For Each str As String In contents
                    If String.IsNullOrEmpty(str.Trim) Then Continue For
                    If str.Trim.StartsWith("--") Then
                        If String.IsNullOrEmpty(fileName) Then
                            fileName = str.Trim.Replace("-", String.Empty)
                            Continue For
                        End If
                    End If
                    If Not String.IsNullOrEmpty(sqlCommand) Then sqlCommand &= vbCrLf
                    sqlCommand &= str
                Next
            Catch ex As Exception
                Throw
            Finally
                Erase contents
                contents = Nothing
            End Try

            If String.IsNullOrEmpty(fileName) OrElse String.IsNullOrEmpty(sqlCommand) Then Continue For
            Dim newRow As DataRow = Me.Dto.SqlCommandTable.NewRow
            newRow.Item("NO") = i
            newRow.Item("FILE_NAME") = fileName
            newRow.Item("SQL_COMMAND") = sqlCommand.Trim
            Me.Dto.SqlCommandTable.Rows.Add(newRow)
            newRow = Nothing
            i += 1
        Next
    End Sub
#End Region

End Class
