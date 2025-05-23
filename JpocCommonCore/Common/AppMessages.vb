Imports System.Data
Imports System.IO

#Region "メッセージクラス"

Public Class AppMessages

#Region "定数"
    ''' <summary>
    ''' 接頭辞
    ''' </summary>
    ''' <remarks>"MSG_"</remarks>
    Const SETTING_PREFIX As String = "MSG_"
#End Region

#Region "変数"
    ''' <summary>
    ''' インスタンス
    ''' </summary>
    Private Shared _Messages As AppMessages
    ''' <summary>
    ''' メッセージファイルパス
    ''' </summary>
    Private _MessageFileFullPath As String
    ''' <summary>
    ''' メッセージ<c>DataSet</c>
    ''' </summary>
    Private _MessageDataSet As DataSet
    ''' <summary>
    ''' <c>FileInfo</c>
    ''' </summary>
    Private _fileInfo As FileInfo
#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Private Sub New()
        Me._MessageDataSet = New DataSet
    End Sub
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="pMessageFilePath">ファイル名</param>
    ''' <remarks><c>GlobalVariables.SettingsFolderPhysicalPath</c>からメッセージファイルを取得する</remarks>
    Private Sub New(ByVal pMessageFilePath As String)
        Me.New()
        Me._MessageFileFullPath = pMessageFilePath
        Me.Init()
    End Sub
#End Region

#Region "初期化"
    ''' <summary>
    ''' 初期化
    ''' </summary>
    Private Sub Init()
        Me._fileInfo = New FileInfo(Me._MessageFileFullPath)
        Dim ds As New DataSet
        Utilities.XmlToDataSet(ds, Me._MessageFileFullPath)
        Me._MessageDataSet = New DataSet
        For Each tbl As DataTable In ds.Tables
            If tbl.TableName.StartsWith(SETTING_PREFIX) Then
                Me._MessageDataSet.Tables.Add(tbl.Copy)
            End If
        Next
        If ds IsNot Nothing Then ds.Dispose()
        ds = Nothing
#If DEBUG Then
        Utilities.DataDump(Me._MessageDataSet)
#End If
        For Each tbl As DataTable In Me._MessageDataSet.Tables
            Dim pk() As DataColumn = {tbl.Columns("CODE")}
            tbl.PrimaryKey = pk
        Next
        Me._MessageDataSet.AcceptChanges()
    End Sub
#End Region

#Region "シングルトンインスタンス取得"
    ''' <summary>
    ''' インスタンス取得
    ''' </summary>
    ''' <param name="pMessageFileFullPath">ファイル名</param>
    ''' <returns>インスタンス</returns>
    Public Shared Function GetInstance(ByVal pMessageFileFullPath As String) As AppMessages
        If _Messages Is Nothing Then
            _Messages = New AppMessages(pMessageFileFullPath)
        End If
        Return _Messages
    End Function
#End Region

#Region "メッセージセット取得"
    ''' <summary>
    ''' メッセージセット取得
    ''' </summary>
    ''' <param name="pMessageCode">メッセージコード</param>
    ''' <param name="pIndex">メッセージ取得インデックス</param>
    ''' <returns><c>Jpoc.MessageSet</c></returns>
    Public Function GetMessageSet(ByVal pMessageCode As String, Optional pIndex As Integer = 0) As MessageSet
        'Dim TrimChars() As Char = {CChar(vbCr), CChar(vbLf), CChar(vbCrLf), CChar(vbTab), CChar(String.Empty), CChar(" ")}
        Dim TrimChars() As Char = {CChar(vbTab), CChar(" ")}
        Me.CheckUpdate()
        Dim m As New MessageSet
        Dim dr As DataRow = Me._MessageDataSet.Tables(SETTING_PREFIX & pIndex.ToString).Rows.Find(pMessageCode)
        If dr Is Nothing Then
            m.MessageID = "ERR0000"
            m.MessageType = PublicEnum.eMessageType.Fatal
            m.Caption = "エラー"
            m.Message = "メッセージが見つかりません。"
            Return m
        End If
        m.MessageID = Utilities.HtmlEncodeHtmlTag(Utilities.NZ(dr.Item("CODE")))
        Select Case Utilities.HtmlEncodeHtmlTag(Utilities.NZ(dr.Item("MESSAGE_TYPE")))
            Case "SYS"
                m.MessageType = PublicEnum.eMessageType.Fatal
            Case "INF"
                m.MessageType = PublicEnum.eMessageType.Information
            Case "ERR"
                m.MessageType = PublicEnum.eMessageType.Error
            Case "WRN"
                m.MessageType = PublicEnum.eMessageType.Warning
            Case "QES"
                m.MessageType = PublicEnum.eMessageType.Question
            Case Else
                m.MessageType = PublicEnum.eMessageType.Undifined
        End Select
        m.Caption = Utilities.HtmlEncodeHtmlTag(Utilities.NZ(dr.Item("CAPTION")))
        '*** メッセージ本文取得 (S) ***
        'XML上のオートインデントにより前後に不要なスペースが入る場合がある為、
        '行単位に分解してTrim、再結合する
        Dim strTemp As String = Utilities.HtmlEncodeHtmlTag(Utilities.NZ(dr.Item("MESSAGE")))
        Dim messageList As New List(Of String)
        Using ms As New System.IO.MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(strTemp))
            'StreamReaderインスタンスの作成
            Using sr As New System.IO.StreamReader(ms)
                'ストリームの末端まで繰り返す
                Do While (sr.Peek() > -1)
                    strTemp = Utilities.HtmlEncodeHtmlTag(sr.ReadLine())
                    strTemp = strTemp.Trim(TrimChars)
                    'strTemp = strTemp.Trim
                    If strTemp.Length > 0 Then
                        messageList.Add(strTemp)
                    End If
                Loop
            End Using
        End Using
        Dim message As String = String.Empty
        For Each str As String In messageList
            If Not String.IsNullOrEmpty(message) Then
                message &= vbCrLf
            End If
            message &= str
        Next
        m.Message = message
        '*** メッセージ本文取得 (E) ***
        If dr.Table.Columns.Contains("SHOW_DIALOG") Then
            Dim showDialog As String = Utilities.NZ(dr.Item("SHOW_DIALOG"))
            Dim val As Boolean = False
            If Boolean.TryParse(showDialog, val) Then
                m.ShowDialog = val
            End If
        End If
        If dr.Table.Columns.Contains("WRITE_LOG") Then
            Dim writeLog As String = Utilities.NZ(dr.Item("WRITE_LOG"))
            Dim val As Boolean = False
            If Boolean.TryParse(writeLog, val) Then
                m.WriteLog = val
            End If
        End If
        Return m
    End Function
#End Region

#Region "メッセージ文字列取得"
    ''' <summary>
    ''' メッセージ文字列取得
    ''' </summary>
    ''' <param name="pMessageCode">メッセージコード</param>
    ''' <param name="pIndex">メッセージ取得インデックス</param>
    ''' <returns>メッセージ</returns>
    Public Function GetMessage(ByVal pMessageCode As String, Optional pIndex As Integer = 0) As String
        Dim ms As MessageSet = GetMessageSet(pMessageCode, pIndex)
        Return ms.Message
    End Function
#End Region

#Region "メッセージファイル更新チェック"
    ''' <summary>
    ''' メッセージファイル更新チェック
    ''' </summary>
    ''' <remarks>更新時間に差異がある場合、初期化を行う</remarks>
    Private Sub CheckUpdate()
        Dim fi As New FileInfo(Me._MessageFileFullPath)
        If fi.LastWriteTime <> Me._fileInfo.LastWriteTime Then
            Me.Init()
        End If
    End Sub
#End Region

End Class

#End Region

#Region "メッセージセットクラス"

''' <summary>
''' メッセージセットクラス
''' </summary>
Public Class MessageSet
    Implements IDisposable

#Region "列挙体"
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' メッセージコード
    ''' </summary>
    Public Property MessageID As String
    ''' <summary>
    ''' メッセージタイプ
    ''' </summary>
    ''' <remarks><c>PublicEnum.eMessageType</c></remarks>
    Public Property MessageType As PublicEnum.eMessageType
    ''' <summary>
    ''' タイトル
    ''' </summary>
    Public Property Caption As String
    ''' <summary>
    ''' 本文
    ''' </summary>
    Public Property Message As String
    ''' <summary>
    ''' ダイアログを出すか否か
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowDialog As Boolean
    ''' <summary>
    ''' ログ出力が必要か否か
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WriteLog As Boolean

#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()
        Me.Clear()
    End Sub
#End Region

#Region "クリア"
    ''' <summary>
    ''' クリア
    ''' </summary>
    Public Sub Clear()
        Me._MessageID = String.Empty
        Me._MessageType = PublicEnum.eMessageType.Undifined
        Me._Caption = String.Empty
        Me._Message = String.Empty
        Me._ShowDialog = False
        Me._WriteLog = False
    End Sub
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

#End Region
