Imports System.IO
Imports System.Data
Imports System.Diagnostics

Public Class ElsException
    Inherits Exception

#Region "変数"
    Private mStackTrace As String
    Private mAdditionalLogInfos As List(Of String)
    Private mAltErrorMessage As String
#End Region

#Region "プロパティ"
    Public Property MessageID As String
    Public Property MessageParam As String()
    '''''''''''''''''''''''''''''''''''''
    'トレース情報を取得するプロパティ
    '''''''''''''''''''''''''''''''''''''
    Public Overrides ReadOnly Property StackTrace() As String
        Get
            If MyBase.StackTrace IsNot Nothing Then
                Return mStackTrace & MyBase.StackTrace
            Else
                Return mStackTrace
            End If
        End Get
    End Property

    '''''''''''''''''''''''''''''''''''''
    '補足情報を取得するプロパティ
    '''''''''''''''''''''''''''''''''''''
    Public ReadOnly Property AdditionalLogInfos() As List(Of String)
        Get
            Return mAdditionalLogInfos
        End Get
    End Property
    Public ReadOnly Property AdditionalLogInfo() As String
        Get
            Dim tmpStr As String = String.Empty
            If AdditionalLogInfos IsNot Nothing Then
                For i As Integer = 0 To AdditionalLogInfos.Count - 1
                    tmpStr &= AdditionalLogInfos.Item(i)
                    If i < AdditionalLogInfos.Count - 1 Then
                        tmpStr &= vbCrLf
                    End If
                Next
            End If
            Return tmpStr
        End Get
    End Property

    '''''''''''''''''''''''''''''''''''''
    'エラー発生元のクラス名+プロシージャ名を取得するプロパティ
    '''''''''''''''''''''''''''''''''''''
    Public ReadOnly Property SourceName() As String
        Get
            Dim strTmp As String
            Dim endOfSource As Integer
            Dim startOfSource As Integer
            Try
                strTmp = Me.StackTrace
                endOfSource = InStr(strTmp, "(")
                If endOfSource < 2 Then Return String.Empty
                strTmp = Left(strTmp, endOfSource - 1)
                startOfSource = InStrRev(strTmp, Space(1))
                If startOfSource = 0 Then Return strTmp
                strTmp = Mid(strTmp, startOfSource + 1)
            Catch ex As Exception
                strTmp = String.Empty
            End Try
            Return strTmp
        End Get
    End Property

    Public Overloads ReadOnly Property Message As String
        Get
            If Not String.IsNullOrEmpty(MessageID) Then
                Dim msg As String = Utilities.GetMessage(MessageID, MessageParam)
                If Not String.IsNullOrEmpty(msg) Then Return msg
            End If
            If Not String.IsNullOrEmpty(mAltErrorMessage) Then
                Return mAltErrorMessage
            End If
            Return MyBase.Message
        End Get
    End Property
    Public ReadOnly Property AltMessage As String
        Get
            Return Me.mAltErrorMessage
        End Get
    End Property

#End Region

#Region "コンストラクタ"
    Public Sub New()
        MyBase.New()
        Me.mStackTrace = String.Empty
        Me.mAdditionalLogInfos = New List(Of String)
        Me.MessageID = String.Empty
        Me.MessageParam = Nothing
        Me.mAltErrorMessage = String.Empty
    End Sub
    Public Sub New(ByVal message As String, _
                   ByVal innerEx As Exception)
        MyBase.New(message, innerEx)
        Me.MessageID = String.Empty
        Me.MessageParam = Nothing
        Me.mStackTrace = String.Empty
        Me.mAltErrorMessage = message
        Me.mAdditionalLogInfos = New List(Of String)
        If TypeOf innerEx Is ElsException Then
            Me.MessageID = CType(innerEx, ElsException).MessageID
            Me.MessageParam = CType(innerEx, ElsException).MessageParam
            Me.mStackTrace = CType(innerEx, ElsException).StackTrace
            Me.AddStackFrame(New StackFrame(1, True))
            Me.mAdditionalLogInfos = CType(innerEx, ElsException).AdditionalLogInfos
        Else
            Me.MessageID = "SYS0001"
            If String.IsNullOrEmpty(message) Then
                Me.mAltErrorMessage = innerEx.Message
                Me.AddAdditionalLogInfo(message)
            End If
        End If
    End Sub
    Public Sub New(ByVal innerEx As Exception)
        Me.New(innerEx.Message, innerEx)
    End Sub
    Public Sub New(ByVal pMessageID As String, _
                   ByVal ParamArray Values() As String)
        Me.New()
        Me.MessageID = pMessageID
        Me.MessageParam = Values
    End Sub
    Public Sub New(ByVal innerEx As Exception, _
                   ByVal pMessageID As String, _
                   ByVal ParamArray Values() As String)
        Me.New(innerEx.Message, innerEx)
        If Not String.IsNullOrEmpty(pMessageID) Then
            Me.MessageID = pMessageID
            Me.MessageParam = Values
        End If
    End Sub
#End Region

#Region "トレース情報追加"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' トレース情報追加
    ''' </summary>
    ''' <param name="sf">トレース情報を取得するためのStackFrameオブジェクト</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/22	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub AddStackFrame(ByVal sf As StackFrame)
        Dim mt As System.Reflection.MethodBase
        Dim attributes As String = String.Empty
        Dim tmpStr As String
        Try
            'トレース情報作成
            mt = sf.GetMethod
            Try
                For i As Integer = 0 To mt.GetParameters.Length - 1
                    tmpStr = mt.GetParameters(i).ParameterType.ToString() & " " & mt.GetParameters(i).Name
                    If mt.GetParameters(i).IsOptional = True Then
                        tmpStr = "Optional " & tmpStr
                        tmpStr &= " Default Value = '" & mt.GetParameters(i).DefaultValue.ToString & "'"
                    End If
                    attributes &= tmpStr
                    If i < mt.GetParameters.Length - 1 Then
                        attributes &= ", "
                    End If
                Next
            Catch ex As Exception
                attributes = String.Empty
            End Try
            '.NETが出力するStackTrace情報とフォーマットを合わせる
            mStackTrace &= Space(3) & "at" & Space(1)
            mStackTrace &= mt.ReflectedType.Namespace & "." & mt.ReflectedType.Name & "." & mt.Name & "(" & attributes & ")"
            mStackTrace &= Space(1) & "in" & Space(1) & sf.GetFileName & ":" & sf.GetFileLineNumber & vbCrLf
        Catch ex As Exception
            Err.Clear()
        Finally
            mt = Nothing
        End Try

    End Sub
#End Region

#Region "補足ログ情報追加"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 補足ログ情報追加
    ''' </summary>
    ''' <param name="pMessage">Additional Infomation</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2007/06/27	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Sub AddAdditionalLogInfo(ByVal pMessage As String)
        Try
            If mAdditionalLogInfos Is Nothing Then
                mAdditionalLogInfos = New List(Of String)
            End If
            If mAdditionalLogInfos.Contains(pMessage) = False Then
                mAdditionalLogInfos.Add(pMessage)
            End If
        Catch ex As Exception
            Err.Clear()
        End Try
    End Sub
#End Region

End Class
