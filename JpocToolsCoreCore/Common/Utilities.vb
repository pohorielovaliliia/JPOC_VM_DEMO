Imports System.IO
Imports System.Drawing

Public Class Utilities
    Inherits Jpoc.Dac.Utilities

#Region "WaterMark取得"
    Public Shared Function GetWaterMark() As MemoryStream
        '現在のコードを実行しているAssemblyを取得
        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        '指定されたマニフェストリソースを読み込む
        Dim watermark As Bitmap = Jpoc.Tools.Core.My.Resources.watermark
        'メモリストリームに書き出す
        Dim m As New MemoryStream
        'TODO: FIX HERE
        'watermark.Save(m, System.Drawing.Imaging.ImageFormat.Png)
        m.Position = 0
        Return m
    End Function
#End Region

    '#Region "RuntimeEnvironment変換"
    '        Public Shared Function GetRuntimeEnvironmentValue(ByVal pRuntimeEnvironmentString As String) As PublicEnum.eRuntimeEnvironment
    '            Try
    '                Dim ret As PublicEnum.eRuntimeEnvironment
    '                If [Enum].TryParse(Of PublicEnum.eRuntimeEnvironment)(pRuntimeEnvironmentString, ret) Then Return ret
    '                Return PublicEnum.eRuntimeEnvironment.PROD
    '            Catch ex As Exception
    '                GlobalVariables.Logger.Error("実行環境変換エラー：String -> Enum", ex)
    '                Return PublicEnum.eRuntimeEnvironment.PROD
    '            End Try
    '        End Function
    '        Public Shared Function GetRuntimeEnvironmentName(ByVal pRuntimeEnvironmentValue As PublicEnum.eRuntimeEnvironment) As String
    '            Try
    '                Return System.Enum.GetName(GetType(PublicEnum.eRuntimeEnvironment), pRuntimeEnvironmentValue)
    '            Catch ex As Exception
    '                GlobalVariables.Logger.Error("実行環境変換エラー：Enum -> String", ex)
    '                Return String.Empty
    '            End Try
    '        End Function
    '#End Region

    '#Region "eJobResult変換"
    '        Public Shared Function GetJobResultValue(ByVal pJobResultString As String) As PublicEnum.eJobResult
    '            Try
    '                Dim ret As PublicEnum.eJobResult
    '                If [Enum].TryParse(Of PublicEnum.eJobResult)(pJobResultString, ret) Then Return ret
    '                Return PublicEnum.eJobResult.Success
    '            Catch ex As Exception
    '                GlobalVariables.Logger.Error("JobResult変換エラー：String -> Enum", ex)
    '                Return PublicEnum.eJobResult.Success
    '            End Try
    '        End Function
    '        Public Shared Function GetJobResultName(ByVal pJobResultValue As PublicEnum.eJobResult) As String
    '            Try
    '                Return System.Enum.GetName(GetType(PublicEnum.eJobResult), pJobResultValue)
    '            Catch ex As Exception
    '                GlobalVariables.Logger.Error("JobResult変換エラー：Enum -> String", ex)
    '                Return String.Empty
    '            End Try
    '        End Function
    '#End Region

    '#Region "eJobResult変換"
    '        Public Shared Function GetKeyDataTypeValue(ByVal pKeyDataTypeString As String) As PublicEnum.eKeyDataType
    '            Try
    '                Dim ret As PublicEnum.eKeyDataType
    '                If [Enum].TryParse(Of PublicEnum.eKeyDataType)(pKeyDataTypeString, ret) Then Return ret
    '                Return PublicEnum.eKeyDataType.Integer
    '            Catch ex As Exception
    '                GlobalVariables.Logger.Error("KeyDataType変換エラー：String -> Enum", ex)
    '                Return PublicEnum.eKeyDataType.Integer
    '            End Try
    '        End Function
    '        Public Shared Function GetKeyDataTypeName(ByVal pKeyDataTypeValue As PublicEnum.eKeyDataType) As String
    '            Try
    '                Return System.Enum.GetName(GetType(PublicEnum.eKeyDataType), pKeyDataTypeValue)
    '            Catch ex As Exception
    '                GlobalVariables.Logger.Error("KeyDataType変換エラー：Enum -> String", ex)
    '                Return String.Empty
    '            End Try
    '        End Function
    '#End Region

    '#Region "メッセージ取得"
    '        Public Shared Function GetMessage(ByVal pMessageCode As String, ByVal ParamArray Values() As String) As String
    '            Dim ms As MessageSet = GlobalVariables.Messages.GetMessageSet(pMessageCode)
    '            '項目名とかを置き換えたい場合は{0}形式でメッセージに埋め込み、呼び出し時にValuesのパラメータ配列に
    '            '項目名とかをぶっこんで呼び出してください。
    '            'メッセージは「AppData\Settings\Messages.xml」に定義してください。
    '            If Values IsNot Nothing Then
    '                ms.Message = String.Format(ms.Message, Values)
    '            End If
    '            Return ms.Message
    '        End Function
    '        Public Shared Function GetMessageSet(ByVal pMessageCode As String, ByVal ParamArray Values() As String) As MessageSet
    '            Dim ms As MessageSet = GlobalVariables.Messages.GetMessageSet(pMessageCode)
    '            '項目名とかを置き換えたい場合は{0}形式でメッセージに埋め込み、呼び出し時にValuesのパラメータ配列に
    '            '項目名とかをぶっこんで呼び出してください。
    '            'メッセージは「AppData\Settings\Messages.xml」に定義してください。
    '            If Values IsNot Nothing Then
    '                ms.Message = String.Format(ms.Message, Values)
    '            End If
    '            Return ms
    '        End Function
    '#End Region

    '#Region "列データ取得"
    '        Public Shared Function GetColText(ByVal pColName As String, _
    '                                          ByVal pTargetRow As DataRow) As String
    '            Try
    '                Return Utilities.NZ(GetColValue(pColName, pTargetRow))
    '            Catch ex As ElsException
    '                ex.AddStackFrame(New StackFrame(True))
    '                Throw
    '            Catch ex As Exception
    '                Throw New ElsException(ex)
    '            End Try
    '        End Function
    '        Public Shared Function GetColValue(ByVal pColName As String, _
    '                                           ByVal pTargetRow As DataRow) As Object
    '            Try
    '                For Each col As DataColumn In pTargetRow.Table.Columns
    '                    If col.ColumnName.ToUpper.Equals(pColName.ToUpper) Then
    '                        If pTargetRow.IsNull(col.ColumnName) Then Return Nothing
    '                        Return pTargetRow.Item(col.ColumnName)
    '                    End If
    '                Next
    '                Return Nothing
    '            Catch ex As ElsException
    '                ex.AddStackFrame(New StackFrame(True))
    '                Throw
    '            Catch ex As Exception
    '                Throw New ElsException(ex)
    '            End Try
    '        End Function
    '#End Region

End Class

