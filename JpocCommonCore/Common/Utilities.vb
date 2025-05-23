Imports System.Data
Imports System.IO
Imports System.IO.Compression
Imports System.Xml
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Security
Imports System.Security.Cryptography
Imports System.Diagnostics
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security.AccessControl
Imports System.Runtime.InteropServices
Imports System.Web
Imports Microsoft.AspNetCore.Http
''' -----------------------------------------------------------------------------
''' Project	 : Common
''' Class	 : Utilities
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' 各種ユーティリティ（WindowsForm、Web、DB等に依存（関係）しないもののみ)
''' </summary>
''' <remarks>
''' </remarks>
''' <history>
''' 	[SHONDA]	2006/07/26	Created
''' </history>
''' -----------------------------------------------------------------------------
Public Class Utilities

#Region "変数"
    Protected Shared rnd1 As New System.Random()
    Protected Shared rnd2 As New System.Random(rnd1.Next())
#End Region

#Region "列挙体"
    ''' <summary>
    ''' ファイル圧縮タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum eFileCompressType
        ''' <summary>
        ''' 無し
        ''' </summary>
        None
        ''' <summary>
        ''' ZIP形式
        ''' </summary>
        Zip
        ''' <summary>
        ''' GZIP形式
        ''' </summary>
        GZip
    End Enum
    ''' <summary>
    ''' デバッグ出力形式
    ''' </summary>
    Public Enum eDebugPrint
        ''' <summary>
        ''' テーブル名のみ
        ''' </summary>
        OnlyTableName = 0
        ''' <summary>
        ''' テーブル名及びカラム名
        ''' </summary>
        TableNameAndColumnName = 1
        ''' <summary>
        ''' すべて
        ''' </summary>
        All = 2
    End Enum
    ''' <summary>
    ''' 出力形式
    ''' </summary>
    Public Enum eOutputMode
        ''' <summary>
        ''' デバッグ出力
        ''' </summary>
        DebugPrint = 0
        ''' <summary>
        ''' コンソール
        ''' </summary>
        Console = 1
        ''' <summary>
        ''' ログ
        ''' </summary>
        Log = 2
    End Enum
    ''' <summary>
    ''' LR
    ''' </summary>
    Public Enum eLR
        ''' <summary>
        ''' Right
        ''' </summary>
        Right = 0
        ''' <summary>
        ''' Left
        ''' </summary>
        Left = 1
    End Enum
    ''' <summary>
    ''' 動作結果ステータス
    ''' </summary>
    Public Enum eOperationResultStatus
        ''' <summary>
        ''' 正常
        ''' </summary>
        OK
        ''' <summary>
        ''' 注意
        ''' </summary>
        Warning
        ''' <summary>
        ''' 論理エラー
        ''' </summary>
        LogicalErr
        ''' <summary>
        ''' システムエラー
        ''' </summary>
        SystemErr
    End Enum
    ''' <summary>
    ''' 小数値丸め設定
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum eCutRule
        ''' <summary>
        ''' 四捨五入
        ''' </summary>
        HalfAdjust = 0
        ''' <summary>
        ''' 切り上げ
        ''' </summary>
        RoundUp = 1
        ''' <summary>
        ''' 切り捨て
        ''' </summary>
        RoundDown = 2
        ''' <summary>
        ''' 丸めこみ無し
        ''' </summary>
        NoCut = 3
    End Enum
    ''' <summary>
    ''' プリントタイプ
    ''' </summary>
    Public Enum ePrintType
        ''' <summary>
        ''' プリンタ
        ''' </summary>
        Print = 1
        ''' <summary>
        ''' プレビュー
        ''' </summary>        
        Preview = 2
    End Enum
    ''' <summary>
    ''' コントロール検索
    ''' </summary>
    Public Enum eControlSearchType
        ''' <summary>
        ''' ID
        ''' </summary>
        ID
        ''' <summary>
        ''' ClientID
        ''' </summary>
        ClientID
        ''' <summary>
        ''' UniqueID
        ''' </summary>
        UniqueID
    End Enum

#End Region

#Region "定数"
    ''' <summary>
    ''' 初期日付
    ''' </summary>
    Private Const DEFAULT_DATE As String = "1901/01/01"
    ''' <summary>
    ''' 初期日付("/"無し)
    ''' </summary>
    Private Const DEFAULT_DATE_WITHOUT_SLASH As String = "19010101"
    ''' <summary>
    ''' 初期時刻
    ''' </summary>
    Private Const DEFAULT_TIME As String = "00:00:00"
    ''' <summary>
    ''' 標準時間フォーマット
    ''' </summary>
    Private Const DEFAULT_TIME_FORMAT As String = "HH:mm:ss"
    ''' <summary>
    ''' 標準日付フォーマット
    ''' </summary>
    Private Const DEFAULT_DATE_FORMAT As String = "yyyy\/MM\/dd"
    ''' <summary>
    ''' 標準日付フォーマット("/"無し)
    ''' </summary>
    Private Const DEFAULT_DATE_FORMAT_WITHOUT_SLASH As String = "yyyyMMdd"
    ''' <summary>
    ''' 標準日時フォーマット
    ''' </summary>
    ''' <remarks>"yyyy/MM/dd HH:mm:ss"</remarks>
    Private Const DEFAULT_DATE_TIME_FORMAT As String = DEFAULT_DATE_FORMAT & " " & DEFAULT_TIME_FORMAT
    ''' <summary>
    ''' 初期化ベクトル
    ''' </summary>
    Private Const INITIAL_VECTOR As String = "Password"
    ''' <summary>
    ''' 標準文字コード
    ''' </summary>
    ''' <remarks>UTF-8</remarks>
    Private Const DEFAULT_ENCORDING As String = "UTF-8"
#End Region

#Region "DllImport"
    'Public Declare Function QueryPerformanceCounter Lib "Kernel32" (ByRef X As Long) As Short
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
    Public Shared Function QueryPerformanceCounter(ByRef X As Long) As Short
    End Function

    'Private Declare Function QueryPerformanceFrequency Lib "Kernel32" (ByRef X As Long) As Short
    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
    Public Shared Function QueryPerformanceFrequency(ByRef X As Long) As Short
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True)>
    Public Shared Function GetTickCount() As Integer
    End Function

    <DllImport("user32.dll")>
    Public Shared Function FindWindow(ByVal strClassName As String,
                                          ByVal strWindowName As String) As Integer
    End Function

    <DllImport("User32.Dll")>
    Public Shared Function SendMessage(ByVal hWnd As Integer,
                                           ByVal Msg As Integer,
                                           ByVal wParam As Integer,
                                           ByVal lParam As Integer) As Integer
    End Function

    <DllImport("urlmon.dll")>
    Public Shared Function FindMimeFromData(ByVal pBC As IntPtr,
                                            <MarshalAs(UnmanagedType.LPWStr)> ByVal pwzUrl As String,
                                            <MarshalAs(UnmanagedType.LPArray,
                                                       ArraySubType:=UnmanagedType.I1,
                                                       SizeParamIndex:=3)> ByVal pBuffer As Byte(),
                                            ByVal cbSize As Integer,
                                            <MarshalAs(UnmanagedType.LPWStr)>
                                            ByVal pwzMimeProposed As String,
                                            ByVal dwMimeFlags As Integer,
                                            ByRef ppwzMimeOut As IntPtr,
                                            ByVal dwReserved As Integer) As Integer
    End Function
    <DllImport("kernel32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)>
    Private Shared Function GetShortPathName(<System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)>
                                             ByVal lpszLongPath As String,
                                             <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)>
                                             ByVal lpszShortPath As System.Text.StringBuilder,
                                             ByVal cchBuffer As Integer) As Integer
    End Function
#End Region

#Region "XML/XSD <-> DataSet 操作"

#Region "データセットよりXML作成"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' データセットよりXML作成
    ''' </summary>
    ''' <param name="ds">読み込むDataset</param>
    ''' <param name="xmlDataFileName">書き出すXMLのファイル名を含むフルパス</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Sub DataSetToXml(ByVal ds As DataSet,
                                   ByVal xmlDataFileName As String)

        Dim fs As FileStream = Nothing
        Dim xtw As XmlTextWriter = Nothing
        Try
            ' Create the FileStream to write with.
            fs = New FileStream(xmlDataFileName, FileMode.Create)

            'Create an XmlTextWriter for the FileStream.
            xtw = New XmlTextWriter(fs, Encoding.Unicode)

            'Add processing instructions to the beginning of the XML file, one
            'of which indicates a style sheet.
            With xtw
                .Indentation = 2
                .IndentChar = " "c
                .Namespaces = True
                .WriteStartDocument(True)
                .Formatting = Formatting.Indented
            End With

            'Write the XML from the dataset to the file.
            ds.WriteXml(xtw)

        Catch ex As Exception
            Throw
        Finally
            If xtw IsNot Nothing Then
                xtw.Close()
            End If
            xtw = Nothing
            If fs IsNot Nothing Then
                fs.Close()
                fs.Dispose()
            End If
            fs = Nothing
        End Try

    End Sub
#End Region

#Region "XMLよりデータセット作成"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' XMLよりデータセット作成
    ''' </summary>
    ''' <param name="ds">読み込んだXMLをセットするDataset</param>
    ''' <param name="xmlFileName">読み込むXMLのファイル名を含むフルパス</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function XmlToDataSet(ByRef ds As DataSet,
                                        ByVal xmlFileName As String) As Boolean
        Try
            'データセットへの読み込み
            ds = New DataSet(Path.GetFileNameWithoutExtension(xmlFileName))
            ds.ReadXml(xmlFileName)

            If ds.Tables.Count = 0 Then
                Throw New Exception
            End If
            ds.AcceptChanges()

        Catch ex As Exception
            ds = Nothing
            Return False
        End Try

        Return True

    End Function
#End Region

#End Region

    '#Region "データセット シリアライズ/デシリアライズ"
    '    ''' <summary>
    '    ''' データセットのシリアライズ化を行う
    '    ''' </summary>
    '    ''' <param name="pDataSet">処理対象</param>
    '    ''' <returns>シリアライズされた文字列</returns>
    '    ''' <remarks>UTF-8エンコードで処理を行う</remarks>
    '    Public Shared Function DataSetSerialize(ByVal pDataSet As DataSet) As String
    '        Dim ser As New System.Xml.Serialization.XmlSerializer(GetType(DataSet))
    '        Dim ret As String = String.Empty
    '        Using ms As New System.IO.MemoryStream
    '            ser.Serialize(ms, pDataSet)
    '            ret = Encoding.UTF8.GetString(ms.ToArray())
    '        End Using
    '        Return ret
    '    End Function
    '    ''' <summary>
    '    ''' データセットへのデシリアライズを行う
    '    ''' </summary>
    '    ''' <param name="pStr">処理対象</param>
    '    ''' <returns>デシリアライズから復元されたデータセット</returns>
    '    ''' <remarks>インプットはUTF-8以外は想定していない</remarks>
    '    Public Shared Function DataSetDeserialize(ByVal pStr As String) As DataSet
    '        Dim ds As DataSet
    '        Using ms As New System.IO.MemoryStream(Encoding.UTF8.GetBytes(pStr))
    '            Dim sz As New System.Xml.Serialization.XmlSerializer(GetType(DataSet))
    '            ds = CType(sz.Deserialize(ms), DataSet)
    '        End Using
    '        Return ds
    '    End Function
    '    ''' <summary>
    '    ''' データセットのシリアライズ化を行い3DES暗号化、GZIP圧縮を行ったバイト配列を返す
    '    ''' </summary>
    '    ''' <param name="pDataSet">処理対象</param>
    '    ''' <param name="pDesKey">3DESキー</param>
    '    ''' <param name="pDesIV">3DESイニシャルベクター</param>
    '    ''' <returns>データセットをシリアライズし、バイト配列に変換した上で3DES暗号化及びGZIP圧縮を行ったもの</returns>
    '    Public Shared Function DataSetSerialize(ByVal pDataSet As DataSet,
    '                                            ByVal pDesKey As String,
    '                                            ByVal pDesIV As String) As Byte()
    '        Dim data As Byte() = Nothing
    '        Try
    '            Using ms As New MemoryStream
    '                'データセットをシリアライズしてメモリーストリームに書き出す
    '                pDataSet.WriteXml(ms, XmlWriteMode.WriteSchema)
    '                'メモリーストリームをバイト配列にコンバートする
    '                data = Utilities.ConvertStreamToByte(ms)
    '                ms.Close()
    '                ms.Dispose()
    '            End Using
    '            'バイト配列を3DESで暗号化する
    '            data = Utilities.Encrypt(data, pDesKey, pDesIV)
    '            'バイト配列をGZIP圧縮する
    '            data = Utilities.GZipCompressByte(data)
    '            'データセットから生成したバイト配列を呼び出し元への戻り値として返す
    '            '・呼び出し元での復元順序
    '            '  GZIP伸張
    '            '  3DES復号
    '            '  バイト配列→ストリーム変換→デシリアライズ→データセット変換
    '            Return data
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase data
    '            data = Nothing
    '        End Try
    '    End Function
    '    ''' <summary>
    '    ''' データセットへのデシリアライズを行う
    '    ''' </summary>
    '    ''' <param name="pData">データセットをシリアライズし、バイト配列に変換した上で3DES暗号化及びGZIP圧縮を行ったもの</param>
    '    ''' <param name="pDesKey">3DESキー</param>
    '    ''' <param name="pDesIV">3DESイニシャルベクター</param>
    '    ''' <returns>バイト配列をGZIP伸張の上3DESにて復号し文字列に変換してからデータセットにデシリアライズしたもの</returns>
    '    Public Shared Function DataSetDeserialize(ByVal pData As Byte(),
    '                                              ByVal pDesKey As String,
    '                                              ByVal pDesIV As String) As DataSet
    '        Dim data As Byte() = Nothing
    '        Try
    '            '受け取ったデータをGZIP伸張する
    '            data = Utilities.GZipDecompressByte(pData)
    '            '伸張したデータを3DESで復号する
    '            data = Utilities.Decrypt(data, pDesKey, pDesIV)
    '            'バイト配列をメモリーストリームに変換する
    '            Using ms As MemoryStream = Utilities.ConvertByteToStream(data)
    '                'メモリーストリームからデシリアライズしてデータセットに読み込む
    '                Using ds As New DataSet
    '                    ds.ReadXml(ms, XmlReadMode.ReadSchema)
    '                    Return ds.Copy
    '                End Using
    '                ms.Close()
    '                ms.Dispose()
    '            End Using
    '        Catch ex As ElsException
    '            ex.AddStackFrame(New StackFrame(True))
    '            Throw
    '        Catch ex As Exception
    '            Throw New ElsException(ex)
    '        Finally
    '            Erase data
    '            data = Nothing
    '        End Try
    '    End Function
    '    ''' <summary>
    '    ''' データセットへのデシリアライズを行う
    '    ''' </summary>
    '    ''' <param name="pData">データセットをシリアライズし、バイト配列に変換した上で3DES暗号化及びGZIP圧縮を行ったもの</param>
    '    ''' <param name="pDesKey">3DESキー</param>
    '    ''' <param name="pDesIV">3DESイニシャルベクター</param>
    '    ''' <param name="pDs">データセット</param>
    '    ''' <returns>バイト配列をGZIP伸張の上3DESにて復号し文字列に変換してからデータセットにデシリアライズしたもの</returns>
    '    Public Shared Function DataSetDeserialize(ByVal pData As Byte(),
    '                                              ByVal pDesKey As String,
    '                                              ByVal pDesIV As String,
    '                                              ByRef pDs As DataSet) As DataSet
    '        Dim data As Byte() = Nothing
    '        Try
    '            '受け取ったデータをGZIP伸張する
    '            data = Utilities.GZipDecompressByte(pData)
    '            '伸張したデータを3DESで復号する
    '            data = Utilities.Decrypt(data, pDesKey, pDesIV)
    '            'バイト配列をメモリーストリームに変換する
    '            Using ms As MemoryStream = Utilities.ConvertByteToStream(data)
    '                'メモリーストリームからデシリアライズしてデータセットに読み込む
    '                pDs.ReadXml(ms, XmlReadMode.ReadSchema)
    '                Return pDs.Copy
    '                ms.Close()
    '                ms.Dispose()
    '            End Using
    '        Catch ex As ElsException
    '            ex.AddStackFrame(New StackFrame(True))
    '            Throw
    '        Catch ex As Exception
    '            Throw New ElsException(ex)
    '        Finally
    '            Erase data
    '            data = Nothing
    '        End Try
    '    End Function
    '#End Region

#Region "文字列操作"

#Region "DBNullまたはNull(Nothing)を長さ0の文字列に変換"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を長さ0の文字列に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function NZ(ByVal obj As Object,
                              Optional pDateIncludeTime As Boolean = False) As String
        Dim RetrunValue As String = String.Empty
        Dim tmpDecimal As Decimal = 0
        Dim tmpInt As Integer = 0
        Dim tmpUInt As UInteger = 0
        Dim tmpSingle As Single = 0.0
        Dim tmpDouble As Double = 0.0
        Dim tmpDateTime As DateTime

        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                RetrunValue = String.Empty
            Else
                If String.IsNullOrEmpty(obj.ToString) = False Then
                    Try
                        Select Case obj.GetType.ToString
                            Case "System.Decimal"
                                tmpDecimal = CType(obj, Decimal)
                                RetrunValue = CutDecimalPartLastZero(tmpDecimal.ToString(GlobalVariables.AppCultureInfo))
                            Case "System.Int16", "System.Int32", "System.Int64"
                                tmpInt = CType(obj, Integer)
                                RetrunValue = tmpInt.ToString(GlobalVariables.AppCultureInfo)
                            Case "System.UInt16", "System.UInt32", "System.UInt64"
                                tmpUInt = CType(obj, UInteger)
                                RetrunValue = tmpUInt.ToString(GlobalVariables.AppCultureInfo)
                            Case "System.Single"
                                tmpSingle = CType(obj, Single)
                                RetrunValue = CutDecimalPartLastZero(tmpSingle.ToString(GlobalVariables.AppCultureInfo))
                            Case "System.Double"
                                tmpDouble = CType(obj, Double)
                                RetrunValue = CutDecimalPartLastZero(tmpDouble.ToString(GlobalVariables.AppCultureInfo))
                            Case "System.DateTime"
                                tmpDateTime = CType(obj, DateTime)
                                RetrunValue = tmpDateTime.ToString(DEFAULT_DATE_FORMAT, GlobalVariables.AppCultureInfo.DateTimeFormat)
                                If Not pDateIncludeTime Then
                                    RetrunValue = tmpDateTime.ToString(DEFAULT_DATE_FORMAT, GlobalVariables.AppCultureInfo.DateTimeFormat)
                                Else
                                    RetrunValue = tmpDateTime.ToString(DEFAULT_DATE_TIME_FORMAT, GlobalVariables.AppCultureInfo.DateTimeFormat)
                                End If
                            Case Else
                                RetrunValue = obj.ToString
                        End Select
                    Catch ex As Exception
                        RetrunValue = obj.ToString
                    End Try
                Else
                    RetrunValue = String.Empty
                End If
            End If
        Catch ex As Exception
            RetrunValue = String.Empty
        End Try
        Return RetrunValue
    End Function
#End Region

#Region "DBNullまたはNull(Nothing)を数値0に変換(Integer)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を数値0に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="pDefaultValue"></param>
    ''' <param name="pRaiseExceptionWhenCastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function N0Int(ByVal obj As Object,
                                 Optional ByVal pDefaultValue As Integer = 0,
                                 Optional ByVal pRaiseExceptionWhenCastError As Boolean = False) As Integer
        Dim ReturnValue As Integer = pDefaultValue
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                ReturnValue = pDefaultValue
            ElseIf IsNumeric(obj) = False Then
                ReturnValue = pDefaultValue
            Else
                Try
                    ReturnValue = CType(obj, Integer)
                Catch ex As Exception When pRaiseExceptionWhenCastError = True
                    Throw
                Catch ex As Exception
                    ReturnValue = pDefaultValue
                End Try
            End If
        Catch ex As Exception When pRaiseExceptionWhenCastError = True
            Throw
        Catch ex As Exception
            ReturnValue = pDefaultValue
        End Try
        Return ReturnValue
    End Function
#End Region

#Region "DBNullまたはNull(Nothing)をNothingに変換(Nullable(Of Integer))"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を数値0に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="pRaiseExceptionWhenCastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function N0IntOrNUll(ByVal obj As Object,
                                       Optional ByVal pRaiseExceptionWhenCastError As Boolean = False) As Nullable(Of Integer)
        Dim ReturnValue As Nullable(Of Integer) = Nothing
        Dim tmpInt As Integer = 0
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                ReturnValue = Nothing
            ElseIf IsNumeric(obj) = False Then
                ReturnValue = Nothing
            Else
                Try
                    tmpInt = CType(obj, Integer)
                    ReturnValue = CType(tmpInt, Nullable(Of Integer))
                Catch ex As Exception When pRaiseExceptionWhenCastError = True
                    Throw
                Catch ex As Exception
                    ReturnValue = Nothing
                End Try
            End If

        Catch ex As Exception When pRaiseExceptionWhenCastError = True
            Throw
        Catch ex As Exception
            ReturnValue = Nothing
        End Try
        Return ReturnValue
    End Function
#End Region

#Region "DBNullまたはNull(Nothing)を数値0に変換(Short)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を数値0に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="pDefaultValue"></param>
    ''' <param name="pRaiseExceptionWhenCastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function N0Short(ByVal obj As Object,
                                   Optional ByVal pDefaultValue As Short = 0,
                                   Optional ByVal pRaiseExceptionWhenCastError As Boolean = False) As Short
        Dim ReturnValue As Short = pDefaultValue
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                ReturnValue = pDefaultValue
            ElseIf IsNumeric(obj) = False Then
                ReturnValue = pDefaultValue
            Else
                Try
                    ReturnValue = CType(obj, Short)
                Catch ex As Exception When pRaiseExceptionWhenCastError = True
                    Throw
                Catch ex As Exception
                    ReturnValue = pDefaultValue
                End Try
            End If

        Catch ex As Exception When pRaiseExceptionWhenCastError = True
            Throw
        Catch ex As Exception
            ReturnValue = pDefaultValue
        End Try
        Return ReturnValue
    End Function
#End Region

#Region "DBNullまたはNull(Nothing)を数値0に変換(Long)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を数値0に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="pDefaultValue"></param>
    ''' <param name="pRaiseExceptionWhenCastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function N0Long(ByVal obj As Object,
                                  Optional ByVal pDefaultValue As Long = 0,
                                  Optional ByVal pRaiseExceptionWhenCastError As Boolean = False) As Long
        Dim ReturnValue As Long = pDefaultValue
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                ReturnValue = pDefaultValue
            ElseIf IsNumeric(obj) = False Then
                ReturnValue = pDefaultValue
            Else
                Try
                    ReturnValue = CType(obj, Long)
                Catch ex As Exception When pRaiseExceptionWhenCastError = True
                    Throw
                Catch ex As Exception
                    ReturnValue = pDefaultValue
                End Try
            End If

        Catch ex As Exception When pRaiseExceptionWhenCastError = True
            Throw
        Catch ex As Exception
            ReturnValue = pDefaultValue
        End Try
        Return ReturnValue
    End Function
#End Region

#Region "DBNullまたはNull(Nothing)を数値0に変換(Decimal)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を数値0に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="pDefaultValue"></param>
    ''' <param name="pRaiseExceptionWhenCastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function N0(ByVal obj As Object,
                              Optional ByVal pDefaultValue As Decimal = 0,
                              Optional ByVal pRaiseExceptionWhenCastError As Boolean = False) As Decimal
        Dim ReturnValue As Decimal = pDefaultValue
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                ReturnValue = pDefaultValue
            ElseIf IsNumeric(obj) = False Then
                ReturnValue = pDefaultValue
            Else
                Try
                    ReturnValue = CType(obj, Decimal)
                Catch ex As Exception When pRaiseExceptionWhenCastError = True
                    Throw
                Catch ex As Exception
                    ReturnValue = pDefaultValue
                End Try
            End If
        Catch ex As Exception When pRaiseExceptionWhenCastError = True
            Throw
        Catch ex As Exception
            ReturnValue = pDefaultValue
        End Try
        Return ReturnValue
    End Function
#End Region

#Region "DBNullまたはNull(Nothing)を数値0に変換(Double)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を数値0に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="pDefaultValue"></param>
    ''' <param name="pRaiseExceptionWhenCastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function N0Double(ByVal obj As Object,
                                    Optional ByVal pDefaultValue As Double = 0,
                                    Optional ByVal pRaiseExceptionWhenCastError As Boolean = False) As Double
        Dim ReturnValue As Double = pDefaultValue
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                ReturnValue = pDefaultValue
            ElseIf IsNumeric(obj) = False Then
                ReturnValue = pDefaultValue
            Else
                Try
                    ReturnValue = CType(obj, Double)
                Catch ex As Exception When pRaiseExceptionWhenCastError = True
                    Throw
                Catch ex As Exception
                    ReturnValue = pDefaultValue
                End Try
            End If
        Catch ex As Exception When pRaiseExceptionWhenCastError = True
            Throw
        Catch ex As Exception
            ReturnValue = pDefaultValue
        End Try
        Return ReturnValue
    End Function
#End Region

#Region "DBNullまたはNull(Nothing)を数値0に変換(Single)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBNullまたはNull(Nothing)を数値0に変換
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="pDefaultValue"></param>
    ''' <param name="pRaiseExceptionWhenCastError"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function N0Single(ByVal obj As Object,
                                    Optional ByVal pDefaultValue As Single = 0,
                                    Optional ByVal pRaiseExceptionWhenCastError As Boolean = False) As Single
        Dim ReturnValue As Single = pDefaultValue
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                ReturnValue = pDefaultValue
            ElseIf IsNumeric(obj) = False Then
                ReturnValue = pDefaultValue
            Else
                Try
                    ReturnValue = CType(obj, Single)
                Catch ex As Exception When pRaiseExceptionWhenCastError = True
                    Throw
                Catch ex As Exception
                    ReturnValue = pDefaultValue
                End Try
            End If
        Catch ex As Exception When pRaiseExceptionWhenCastError = True
            Throw
        Catch ex As Exception
            ReturnValue = pDefaultValue
        End Try
        Return ReturnValue
    End Function
#End Region

#Region "渡された日付列の値がDBNullの場合、Nothingに変換"
    Public Shared Function DateTimeDBNullToNothing(ByVal obj As Object) As Nullable(Of DateTime)
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                Return Nothing
            End If
            Return CType(obj, DateTime)
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "渡されたNull可日付型の値がNothingの場合、DBNullに変換"
    Public Shared Sub DateTimeNothingToDBNull(ByVal val As Object, ByRef dr As DataRow, ByVal colName As String)
        Try
            If val Is Nothing Then
                dr.Item(colName) = DBNull.Value
            Else
                dr.Item(colName) = CType(val, DateTime)
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "列値をyyyy/MM/ddまたは空文字に変換"
    Public Shared Function DbDateToString(obj As Object) As String
        Try
            If IsNothing(obj) OrElse IsDBNull(obj) Then
                Return String.Empty
            Else
                Return CType(obj, DateTime).ToString("yyyy/MM/dd")
            End If
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "列値をyyyyMMdd形式の整数値に変換"
    Public Shared Function DbDateToInteger(obj As Object) As Integer
        Try
            Return Integer.Parse(CType(obj, DateTime).ToString("yyyyMMdd"))
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "文字列を指定したバイト長に切り詰め"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 文字列切り詰め
    ''' </summary>
    ''' <param name="pStr"></param>
    ''' <param name="pByteLength"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function CutStr(ByVal pStr As String,
                                  ByVal pByteLength As Integer) As String
        Dim strlen As Integer
        Dim ReturnString As String = String.Empty
        Dim TotalByte As Integer = 0

        Try
            strlen = LenByte(pStr)

            If pByteLength < strlen Then
                For i As Integer = 0 To pStr.Length - 1
                    TotalByte += LenByte(pStr.Substring(i, 1))
                    If TotalByte <= pByteLength Then
                        ReturnString &= pStr.Substring(i, 1)
                    End If
                Next
            Else
                ReturnString = pStr
            End If

        Catch ex As Exception
            Throw
        Finally

        End Try

        Return ReturnString
    End Function
#End Region

#Region "指定したバイト長に加工 (FIXED)"
    Public Shared Function FixedString(ByVal pStr As String,
                                       ByVal pByteLength As Integer,
                                       Optional ByVal pPosision As eLR = eLR.Left) As String
        Dim rtnStr As String = String.Empty
        Dim strlen As Integer
        Try
            rtnStr = NZ(pStr)
            strlen = LenByte(rtnStr)

            Select Case strlen
                Case Is > pByteLength
                    rtnStr = CutStr(rtnStr, pByteLength)
                Case Is < pByteLength
                    For i As Integer = 1 To (pByteLength - strlen)
                        If pPosision = eLR.Left Then
                            rtnStr &= " "
                        Else
                            rtnStr = " " & rtnStr
                        End If
                    Next
            End Select
        Catch ex As Exception
            Throw
        Finally

        End Try
        Return rtnStr
    End Function
#End Region

#Region "CSVをリストに分割する"
    Public Shared Function SplitCSVStringToList(ByVal pStr As String,
                                                      Optional ByVal pSeparator As Char = ","c,
                                                      Optional ByVal pToUpper As Boolean = False) As List(Of String)
        ' CSVを文コレクションに分割する
        ' 標準ではカンマ(,)をセパレータデリミタと認識する
        ' ダブルクォーテーションでくくられた部分も正しく認識する

        Const DOUBLE_QUOTATION As Char = Chr(34)
        Dim pos As Integer        ' 現在の文字の位置
        Dim len As Integer        ' 文字列の長さ
        Dim last As Integer       ' 最後に見つかったカンマの次の位置
        Dim ch As Char            ' 現在の文字
        Dim quot As Boolean       ' ダブルクォーテーションでくくられた部分にいるか否か
        Dim ret As New List(Of String) '戻り値
        Dim val As String         '個別の値

        Try
            If String.IsNullOrEmpty(pStr) = False Then
                pos = 0
                len = pStr.Length
                last = 0
                ch = Char.MinValue
                quot = False

                While (pos < len)
                    ch = pStr.Chars(pos)
                    If Not quot AndAlso ch.Equals(pSeparator) Then

                        ' ダブルクォーテーションでくくられた部分ではないところでカンマが見つかった場合
                        val = pStr.Substring(last, pos - last)

                        ' ダブルクォーテーションでくくられている場合、クォーテーションを取り除く
                        If val.StartsWith(DOUBLE_QUOTATION) AndAlso val.EndsWith(DOUBLE_QUOTATION) Then
                            val = val.Substring(1, val.Length - 2)
                        End If

                        val = val.Trim

                        If pToUpper = True AndAlso String.IsNullOrEmpty(val) = False Then
                            val = val.ToUpper
                        End If

                        ' 値をリストに追加
                        ret.Add(val)

                        ' 位置を保存
                        last = pos + 1

                    ElseIf ch.Equals(DOUBLE_QUOTATION) Then
                        quot = Not quot ' ダブルクォーテーションの状態反転
                    End If
                    ' 次の文字
                    pos += 1
                End While

                ' 最後が区切り文字で終わっている場合、空文字を追加
                'If ch.Equals(","c) OrElse ch.Equals(vbTab) OrElse ch.Equals("|"c) Then
                If ch.Equals(pSeparator) Then
                    ret.Add(String.Empty)
                Else
                    ' 最後の1件
                    val = pStr.Substring(last, pos - last)
                    ' ダブルクォーテーションでくくられている場合、クォーテーションを取り除く
                    If val.StartsWith(DOUBLE_QUOTATION) AndAlso val.EndsWith(DOUBLE_QUOTATION) Then
                        val = val.Substring(1, val.Length - 2)
                    End If

                    val = val.Trim

                    If pToUpper = True AndAlso String.IsNullOrEmpty(val) = False Then
                        val = val.ToUpper
                    End If

                    ' 値をリストに追加
                    ret.Add(val)
                End If
            End If

        Catch ex As Exception
            ret = New List(Of String)
        End Try

        Return ret
    End Function
#End Region

#Region "CSVを配列に分割する"
    Public Shared Function SplitCSVStringToArray(ByVal pStr As String,
                                                 Optional ByVal pSeparator As Char = ","c) As String()
        Dim tmpList As List(Of String)
        Dim tmpAray() As String = Nothing
        Try
            tmpList = SplitCSVStringToList(pStr, pSeparator)
            If tmpList.Count > 0 Then
                ReDim tmpAray(tmpList.Count - 1)
                For i As Integer = 0 To tmpList.Count - 1
                    tmpAray(i) = NZ(tmpList.Item(i))
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
        Return tmpAray
    End Function
#End Region

#Region "文字列に'を付与して戻す。カンマ区切りの場合は、それぞれに付与する"
    Public Shared Function ConvString(ByVal pStr As String, Optional ByVal pOption As Integer = 1) As String
        Dim tmpList As List(Of String) = Nothing
        Dim sb As StringBuilder
        Try
            sb = New StringBuilder
            Select Case pOption
                Case 1
                    sb.Append("'")
                    sb.Append(pStr)
                    sb.Append("'")
                Case 2
                    tmpList = SplitCSVStringToList(pStr)
                    For i As Integer = 0 To tmpList.Count - 1
                        sb.Append("'")
                        sb.Append(tmpList(i))
                        sb.Append("'")
                        If i < tmpList.Count - 1 Then
                            sb.Append(",")
                        End If
                    Next
            End Select
        Catch ex As Exception
            sb = New StringBuilder
        End Try
        Return sb.ToString
    End Function
#End Region

#Region "文字列をバイト配列に変換"
    Public Shared Function EncodeByteArray(ByVal pInStr As String,
                                           Optional ByVal pEncording As Object = 0) As Byte()
        Dim encDefault As System.Text.Encoding = Nothing
        If IsNumeric(pEncording) Then
            Dim codepage As Integer = N0Int(pEncording)
            encDefault = System.Text.Encoding.GetEncoding(codepage)
        Else
            Dim encordingName As String = NZ(pEncording)
            encDefault = System.Text.Encoding.GetEncoding(encordingName)
        End If
        Return encDefault.GetBytes(pInStr)
    End Function
    Public Shared Function EncodeByteArrayByShiftJIS(ByVal pInStr As String) As Byte()
        Return EncodeByteArray(pInStr, "shift_jis")
    End Function
    Public Shared Function EncodeByteArrayByUtf8(ByVal pInStr As String) As Byte()
        Return EncodeByteArray(pInStr, "utf-8")
    End Function
#End Region

#Region "バイト配列を文字列に変換"
    Public Shared Function DecodeByteArray(ByVal pInByte() As Byte,
                                           Optional ByVal pEncording As Object = 0) As String
        Dim encDefault As System.Text.Encoding = Nothing
        If IsNumeric(pEncording) Then
            Dim codepage As Integer = N0Int(pEncording)
            encDefault = System.Text.Encoding.GetEncoding(codepage)
        Else
            Dim encordingName As String = NZ(pEncording)
            encDefault = System.Text.Encoding.GetEncoding(encordingName)
        End If
        Return encDefault.GetString(pInByte)
    End Function
    Public Shared Function DecodeByteArrayByShiftJIS(ByVal pInByte() As Byte) As String
        Return DecodeByteArray(pInByte, "shift_jis")
    End Function
    Public Shared Function DecodeByteArrayByUTF8(ByVal pInByte() As Byte) As String
        Return DecodeByteArray(pInByte, "utf-8")
    End Function
#End Region

#Region "ファイル名/フォルダ名禁止文字変更"
    Public Shared Function ConvertInvalidFileNameChars(ByVal pInText As String) As String
        Dim RtnString As String = pInText
        Try
            For Each str As Char In Path.GetInvalidFileNameChars
                RtnString = RtnString.Replace(str, "_")
            Next
        Catch ex As Exception
            RtnString = pInText
        End Try
        Return RtnString
    End Function
    Public Shared Function ConvertInvalidPathChars(ByVal pInText As String) As String
        Dim RtnString As String = pInText
        Try
            For Each str As Char In Path.GetInvalidPathChars
                RtnString = RtnString.Replace(str, String.Empty)
            Next
        Catch ex As Exception
            RtnString = pInText
        End Try
        Return RtnString
    End Function
#End Region

#Region "文字列バイト取得"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 文字列バイト取得
    ''' </summary>
    ''' <param name="pTarget">文字列</param>
    ''' <param name="pEncording">エンコード</param>
    ''' <returns>バイト数</returns>
    ''' <remarks>
    ''' 文字列のバイト数を取得
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function LenByte(ByVal pTarget As String,
                                   Optional ByVal pEncording As String = "Shift_JIS") As Integer
        Return System.Text.Encoding.GetEncoding(pEncording).GetByteCount(pTarget)
    End Function
#End Region

#Region "数値文字列の不要な小数点以下の最終0を切り捨てる"
    Public Shared Function CutDecimalPartLastZero(ByVal pValue As String) As String
        Dim RetrunValue As String = pValue
        Try
            If IsNumeric(pValue) = True Then
                If RetrunValue.Contains("."c) = True Then   '小数部あり
                    Do
                        If RetrunValue.EndsWith("0") = True Then    '最後が0
                            RetrunValue = RetrunValue.Substring(0, RetrunValue.Length - 1)  '最後の0切捨て
                        Else
                            If RetrunValue.EndsWith(".") = True Then    '最後が小数点
                                RetrunValue = RetrunValue.Substring(0, RetrunValue.Length - 1)  '小数点切捨て
                                Exit Do '残りは整数部だけだからループを出る
                            Else    '小数部の0でない桁発見
                                Exit Do 'ループを出る
                            End If
                        End If
                    Loop
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
        Return RetrunValue
    End Function
#End Region

#Region "数値文字列の表示用書式を取得する"
    Public Shared Function GetIntegerFormat(ByVal pScale As Integer,
                                            Optional ByVal pLeadingZero As Boolean = False,
                                            Optional ByVal pIsInputMask As Boolean = False) As String
        Dim strMask As String = String.Empty
        Dim chrLiteral As Char = "#"c
        Dim intFloor As Integer = CType(Math.Floor(pScale / 3), Integer)
        Dim intMod As Integer = pScale Mod 3

        If pIsInputMask = True Then
            chrLiteral = "9"c
        End If

        Select Case intFloor
            Case 0
                strMask = New String(chrLiteral, pScale)
            Case Else
                If intMod <> 0 Then
                    strMask = New String(chrLiteral, intMod) & ","
                End If
                For i As Integer = 1 To intFloor
                    If pLeadingZero = False Then
                        strMask &= New String(chrLiteral, 3)
                        If i < intFloor Then
                            strMask &= ","
                        End If
                    Else
                        If i < intFloor Then
                            strMask &= New String(chrLiteral, 3)
                            strMask &= ","
                        Else
                            strMask &= New String(chrLiteral, 2) & "0"
                        End If
                    End If
                Next
        End Select
        Return strMask
    End Function
#End Region

#Region "バイナリ文字列をHex文字列に変換"
    ' * 
    ' * Convert Binary to Hex (bin2hex) 
    ' * 
    ' * @param string bindata Binary data 
    ' * @access public 
    ' * @return string 
    Public Shared Function bin2hex(ByVal bindata As String) As String
        Dim i As Integer
        Dim bytes As Byte() = Encoding.GetEncoding(1252).GetBytes(bindata)
        Dim hexString As String = String.Empty
        For i = 0 To bytes.Length - 1
            hexString += bytes(i).ToString("x2")
        Next
        Return hexString
    End Function
#End Region

#Region "Hex文字列をバイナリ文字列に変換"
    ' * 
    ' * Convert Hex to Binary (hex2bin) 
    ' * 
    ' * @param string packtype Rudimentary in this implementation 
    ' * @param string datastring Hex to be packed into Binary 
    ' * @access private 
    ' * @return string 
    ' 
    Public Shared Function pack(ByVal datastring As String) As String
        Dim i As Integer, j As Integer, datalength As Integer, packsize As Integer
        Dim bytes As Byte()
        Dim hex As Char()
        Dim tmp As String

        datalength = datastring.Length
        packsize = CType((datalength / 2) + (datalength Mod 2), Integer)
        bytes = New Byte(packsize - 1) {}
        hex = New Char(1) {}

        For i = InlineAssignHelper(j, 0) To datalength - 1 Step 2
            hex(0) = datastring(i)
            If datalength - i = 1 Then
                hex(1) = "0"c
            Else
                hex(1) = datastring(i + 1)
            End If
            tmp = New String(hex, 0, 2)
            Try
                bytes(System.Math.Max(System.Threading.Interlocked.Increment(j), j - 1)) = Byte.Parse(tmp, System.Globalization.NumberStyles.HexNumber)
            Catch
                ' grin 
            End Try
        Next
        Return Encoding.GetEncoding(1252).GetString(bytes)
    End Function
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function
#End Region

#Region "Charをバイト配列にエンコードした最初の値を取得"
    Private Shared Function ord(ByVal ch As Char) As Integer
        Return CInt(Encoding.GetEncoding(1252).GetBytes(ch & String.Empty)(0))
    End Function
#End Region

#Region "バイト配列をHex文字列に変換"
    Public Shared Function ToHexString(pData As Byte(),
                                       Optional ByVal pToUpper As Boolean = False) As String
        Dim sb As New StringBuilder(pData.Length * 2)
        For Each b As Byte In pData
            If b < 16 Then sb.Append("0")
            sb.Append(Convert.ToString(b, 16))
        Next
        Dim ret As String = sb.ToString
        If pToUpper Then
            ret = ret.ToUpper
        End If
        Return ret
    End Function
#End Region

#Region "Hex文字列をバイト配列に変換"
    Public Shared Function FromHexString(ByVal pStr As String) As Byte()
        Dim length As Integer = CInt(pStr.Length / 2) - 1
        Dim data(length) As Byte
        Dim j As Integer = 0
        For i As Integer = 0 To length
            data(i) = Convert.ToByte(pStr.Substring(j, 2), 16)
            j += 2
        Next
        Return data
    End Function
#End Region

#Region "文字列からHTMLタグを除外して返す"
    Public Shared Function RemoveHtmlTag(ByVal pInStr As String) As String
        Const TAG_PATTERN As String = "<.*?>"
        Dim newStr As String = Regex.Replace(pInStr, TAG_PATTERN, String.Empty, RegexOptions.Singleline)
        Return newStr
    End Function
#End Region

#Region "文字列をBase64文字列にする"
    Public Shared Function ConvertStringToBase64(ByVal pInStr As String) As String
        Dim bytes() As Byte = EncodeByteArrayByUtf8(pInStr)
        'Base64で文字列に変換
        Return System.Convert.ToBase64String(bytes)
    End Function
#End Region

#Region "Base64文字列を文字列にする"
    Public Shared Function ConvertBase64ToString(ByVal pInStr As String) As String
        Dim bs() As Byte = System.Convert.FromBase64String(pInStr)
        Return DecodeByteArrayByUTF8(bs)
    End Function
#End Region

#Region "文字コード判別"
    ''' <summary>
    ''' 文字コードを判別する
    ''' </summary>
    ''' <remarks>
    ''' Jcode.pmのgetcodeメソッドを移植したものです。
    ''' Jcode.pm(http://openlab.ring.gr.jp/Jcode/index-j.html)
    ''' Jcode.pmのCopyright: Copyright 1999-2005 Dan Kogai
    ''' </remarks>
    ''' <param name="bytes">文字コードを調べるデータ</param>
    ''' <returns>適当と思われるEncodingオブジェクト。
    ''' 判断できなかった時はnull。</returns>
    Public Shared Function GetCode(ByVal bytes As Byte()) As System.Text.Encoding
        Const bEscape As Byte = &H1B
        Const bAt As Byte = &H40
        Const bDollar As Byte = &H24
        Const bAnd As Byte = &H26
        Const bOpen As Byte = &H28 ''('
        Const bB As Byte = &H42
        Const bD As Byte = &H44
        Const bJ As Byte = &H4A
        Const bI As Byte = &H49

        Dim len As Integer = bytes.Length
        Dim b1 As Byte, b2 As Byte, b3 As Byte, b4 As Byte

        'Encode::is_utf8 は無視

        Dim isBinary As Boolean = False
        Dim i As Integer
        For i = 0 To len - 1
            b1 = bytes(i)
            If b1 <= &H6 OrElse b1 = &H7F OrElse b1 = &HFF Then
                ''binary'
                isBinary = True
                If b1 = &H0 AndAlso i < len - 1 AndAlso bytes(i + 1) <= &H7F Then
                    'smells like raw unicode
                    Return System.Text.Encoding.Unicode
                End If
            End If
        Next
        If isBinary Then
            Return Nothing
        End If

        'not Japanese
        Dim notJapanese As Boolean = True
        For i = 0 To len - 1
            b1 = bytes(i)
            If b1 = bEscape OrElse &H80 <= b1 Then
                notJapanese = False
                Exit For
            End If
        Next
        If notJapanese Then
            Return System.Text.Encoding.ASCII
        End If

        For i = 0 To len - 3
            b1 = bytes(i)
            b2 = bytes(i + 1)
            b3 = bytes(i + 2)

            If b1 = bEscape Then
                If b2 = bDollar AndAlso b3 = bAt Then
                    'JIS_0208 1978
                    'JIS
                    Return System.Text.Encoding.GetEncoding(50220)
                ElseIf b2 = bDollar AndAlso b3 = bB Then
                    'JIS_0208 1983
                    'JIS
                    Return System.Text.Encoding.GetEncoding(50220)
                ElseIf b2 = bOpen AndAlso (b3 = bB OrElse b3 = bJ) Then
                    'JIS_ASC
                    'JIS
                    Return System.Text.Encoding.GetEncoding(50220)
                ElseIf b2 = bOpen AndAlso b3 = bI Then
                    'JIS_KANA
                    'JIS
                    Return System.Text.Encoding.GetEncoding(50220)
                End If
                If i < len - 3 Then
                    b4 = bytes(i + 3)
                    If b2 = bDollar AndAlso b3 = bOpen AndAlso b4 = bD Then
                        'JIS_0212
                        'JIS
                        Return System.Text.Encoding.GetEncoding(50220)
                    End If
                    If i < len - 5 AndAlso
                        b2 = bAnd AndAlso b3 = bAt AndAlso b4 = bEscape AndAlso
                        bytes(i + 4) = bDollar AndAlso bytes(i + 5) = bB Then
                        'JIS_0208 1990
                        'JIS
                        Return System.Text.Encoding.GetEncoding(50220)
                    End If
                End If
            End If
        Next

        'should be euc|sjis|utf8
        'use of (?:) by Hiroki Ohzaki <ohzaki@iod.ricoh.co.jp>
        Dim sjis As Integer = 0
        Dim euc As Integer = 0
        Dim utf8 As Integer = 0
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&H81 <= b1 AndAlso b1 <= &H9F) OrElse
                (&HE0 <= b1 AndAlso b1 <= &HFC)) AndAlso
                ((&H40 <= b2 AndAlso b2 <= &H7E) OrElse
                 (&H80 <= b2 AndAlso b2 <= &HFC)) Then
                'SJIS_C
                sjis += 2
                i += 1
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If ((&HA1 <= b1 AndAlso b1 <= &HFE) AndAlso
                (&HA1 <= b2 AndAlso b2 <= &HFE)) OrElse
                (b1 = &H8E AndAlso (&HA1 <= b2 AndAlso b2 <= &HDF)) Then
                'EUC_C
                'EUC_KANA
                euc += 2
                i += 1
            ElseIf i < len - 2 Then
                b3 = bytes(i + 2)
                If b1 = &H8F AndAlso (&HA1 <= b2 AndAlso b2 <= &HFE) AndAlso
                    (&HA1 <= b3 AndAlso b3 <= &HFE) Then
                    'EUC_0212
                    euc += 3
                    i += 2
                End If
            End If
        Next
        For i = 0 To len - 2
            b1 = bytes(i)
            b2 = bytes(i + 1)
            If (&HC0 <= b1 AndAlso b1 <= &HDF) AndAlso
                (&H80 <= b2 AndAlso b2 <= &HBF) Then
                'UTF8
                utf8 += 2
                i += 1
            ElseIf i < len - 2 Then
                b3 = bytes(i + 2)
                If (&HE0 <= b1 AndAlso b1 <= &HEF) AndAlso
                    (&H80 <= b2 AndAlso b2 <= &HBF) AndAlso
                    (&H80 <= b3 AndAlso b3 <= &HBF) Then
                    'UTF8
                    utf8 += 3
                    i += 2
                End If
            End If
        Next
        'M. Takahashi's suggestion
        'utf8 += utf8 / 2;

        System.Diagnostics.Debug.WriteLine(
            String.Format("sjis = {0}, euc = {1}, utf8 = {2}", sjis, euc, utf8))
        If euc > sjis AndAlso euc > utf8 Then
            'EUC
            Return System.Text.Encoding.GetEncoding(51932)
        ElseIf sjis > euc AndAlso sjis > utf8 Then
            'SJIS
            Return System.Text.Encoding.GetEncoding(932)
        ElseIf utf8 > euc AndAlso utf8 > sjis Then
            'UTF8
            Return System.Text.Encoding.UTF8
        End If

        Return Nothing
    End Function

#End Region

#End Region

#Region "日付操作"

#Region "UTC→JPN変換"
    ''' <summary>
    ''' UTC→JPN変換を行う
    ''' </summary>
    ''' <param name="pUtc">時刻(UTC)</param>
    ''' <returns>JPN時刻</returns>
    Public Shared ReadOnly Property ConvertUtcToJpnDateTime(pUtc As DateTime) As DateTime
        Get
            Return pUtc.AddHours(9)
        End Get
    End Property
#End Region

#Region "JPN→UTC変換"
    ''' <summary>
    ''' JPN→UTC変換を行う
    ''' </summary>
    ''' <param name="pJpnDateTime">時刻(JPN)</param>
    ''' <returns>UTC時刻</returns>
    Public Shared ReadOnly Property ConvertJpnDateTimeToUtc(pJpnDateTime As DateTime) As DateTime
        Get
            Return pJpnDateTime.AddHours(-9)
        End Get
    End Property
#End Region

#Region "日付型の初期値設定"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 日付型の初期値設定
    ''' </summary>
    ''' <param name="pStr"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function [CDate](ByVal pStr As String) As Date
        Dim Result As Date
        Try
            Result = CType(pStr, Date)
            If Result.Year < 1901 Then
                Result = CType(DEFAULT_DATE, Date)
            End If
            If Result.Year > 2099 Then
                Result = CType(DEFAULT_DATE, Date)
            End If
        Catch
            Result = CType(DEFAULT_DATE, Date)
        End Try
        Return Result
    End Function
#End Region

#Region "指定年月の前月月末日算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の前月月末日算出
    ''' </summary>
    ''' <param name="yyyy">年(yyyy)</param>
    ''' <param name="mm">月(MM)</param>
    ''' <returns>算出した前月月末日</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetLastDayOfLastMonth(ByVal yyyy As String,
                                                 ByVal mm As String) As String
        Dim ts As New System.TimeSpan
        Dim ReturnDate As String
        Dim LastDate As Date

        Try
            LastDate = CType(GetFirstDayOfMonth(yyyy, mm), Date)
            If Not LastDate = CType(DEFAULT_DATE, Date) Then
                ts = TimeSpan.FromDays(1)
                LastDate = LastDate.Subtract(ts)
            End If
            ReturnDate = LastDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDate = DEFAULT_DATE
        End Try
        Return ReturnDate
    End Function
#End Region

#Region "指定年月の前月月末日算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の前月月末日算出
    ''' </summary>
    ''' <param name="ymd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetLastDayOfLastMonth(ByVal ymd As Date) As String
        Dim ReturnDate As String
        Dim ts As New System.TimeSpan
        Dim LastDate As Date

        Try
            LastDate = CType(GetFirstDayOfMonth(ymd), Date)
            If Not LastDate = CType(DEFAULT_DATE, Date) Then
                ts = TimeSpan.FromDays(1)
                LastDate = LastDate.Subtract(ts)
            End If
            ReturnDate = LastDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDate = DEFAULT_DATE
        End Try
        Return ReturnDate
    End Function
#End Region

#Region "指定年月の当月月末日算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の当月月末日算出
    ''' </summary>
    ''' <param name="yyyy">年</param>
    ''' <param name="mm">月</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetLastDayOfMonth(ByVal yyyy As String,
                                             ByVal mm As String) As String
        Dim ReturnDate As String
        Dim LastDate As Date

        Try
            LastDate = CType(yyyy & "/" & mm & "/" & CType(Date.DaysInMonth(CType(yyyy, Integer), CType(mm, Integer)), String), Date)
            ReturnDate = LastDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDate = DEFAULT_DATE
        End Try
        Return ReturnDate
    End Function
#End Region

#Region "指定年月の当月月末日算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の当月月末日算出
    ''' </summary>
    ''' <param name="ymd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetLastDayOfMonth(ByVal ymd As Date) As String
        Dim ReturnDate As String
        Dim LastDate As Date

        Try
            ReturnDate = ymd.ToString(DEFAULT_DATE_FORMAT)

            LastDate = CType(ReturnDate.Substring(0, 4) & "/" & ReturnDate.Substring(5, 2) & "/" & CType(
                Date.DaysInMonth(CType(ReturnDate.Substring(0, 4), Integer),
                CType(ReturnDate.Substring(5, 2), Integer)), String), Date)
            ReturnDate = LastDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDate = DEFAULT_DATE
        End Try
        Return ReturnDate
    End Function
#End Region

#Region "指定年月の当月月初算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の当月月初算出
    ''' </summary>
    ''' <param name="yyyy">年</param>
    ''' <param name="mm">月</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetFirstDayOfMonth(ByVal yyyy As String,
                                              ByVal mm As String) As String
        Dim ReturnDay As String
        Dim FirstDate As Date

        Try
            FirstDate = CType(yyyy & "/" & mm & "/" & "01", Date)
            ReturnDay = FirstDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDay = DEFAULT_DATE
        End Try
        Return ReturnDay
    End Function
#End Region

#Region "指定年月の当月月初算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の当月月初算出
    ''' </summary>
    ''' <param name="ymd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetFirstDayOfMonth(ByVal ymd As Date) As String
        Dim ReturnDay As String
        Dim FirstDate As Date

        Try
            ReturnDay = ymd.ToString(DEFAULT_DATE_FORMAT)
            FirstDate = CType(ReturnDay.Substring(0, 4) & "/" & ReturnDay.Substring(5, 2) & "/" & "01", Date)
            ReturnDay = FirstDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDay = DEFAULT_DATE
        End Try
        Return ReturnDay
    End Function
#End Region

#Region "指定年月の翌月月初算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の翌月月初算出
    ''' </summary>
    ''' <param name="yyyy">年(yyyyまたはyy)</param>
    ''' <param name="mm">月(MMまたはM)</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetFirstDayOfNextMonth(ByVal yyyy As String,
                                                  ByVal mm As String) As String
        Dim ReturnDay As String
        Dim FirstDate As Date

        Try
            FirstDate = CType(GetLastDayOfMonth(yyyy, mm), Date)
            If Not FirstDate = CType(DEFAULT_DATE, Date) Then
                FirstDate = FirstDate.AddDays(1)
            End If
            ReturnDay = FirstDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDay = DEFAULT_DATE
        End Try
        Return ReturnDay
    End Function
#End Region

#Region "指定年月の翌月月初算出"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定年月の翌月月初算出
    ''' </summary>
    ''' <param name="ymd"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetFirstDayOfNextMonth(ByVal ymd As Date) As String
        Dim ReturnDay As String
        Dim FirstDate As Date

        Try
            ReturnDay = ymd.ToString(DEFAULT_DATE_FORMAT)
            FirstDate = CType(GetLastDayOfMonth(ReturnDay.Substring(0, 4), ReturnDay.Substring(5, 2)), Date)
            If Not FirstDate = CType(DEFAULT_DATE, Date) Then
                FirstDate = FirstDate.AddDays(1)
            End If
            ReturnDay = FirstDate.ToString(DEFAULT_DATE_FORMAT)
        Catch
            ReturnDay = DEFAULT_DATE
        End Try
        Return ReturnDay
    End Function
#End Region

#Region "ローカル/UTCタイム変換"
    ''' <summary>
    ''' ローカル時刻←→UTC時刻変換を行う
    ''' </summary>
    ''' <param name="pOrgDateTime">処理対象文字列</param>
    ''' <param name="pToUtc">UTC変換フラグ</param>
    ''' <returns>ローカル時刻もしくはUTC時刻に変換した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertTimeZone(ByVal pOrgDateTime As String,
                                           ByVal pToUtc As Boolean) As String
        Dim RtnStr As String = String.Empty
        Dim tmpDt As DateTime = Nothing
        Dim tmpDtStr As String = String.Empty
        Try

            tmpDtStr = pOrgDateTime.Replace("/"c, String.Empty).Replace(":"c, String.Empty).Replace(" "c, String.Empty)

            If pToUtc = True Then
                tmpDt = New DateTime(CType(tmpDtStr.Substring(0, 4), Integer),
                                     CType(tmpDtStr.Substring(4, 2), Integer),
                                     CType(tmpDtStr.Substring(6, 2), Integer),
                                     CType(tmpDtStr.Substring(8, 2), Integer),
                                     CType(tmpDtStr.Substring(10, 2), Integer),
                                     CType(tmpDtStr.Substring(12, 2), Integer),
                                     DateTimeKind.Local)
                tmpDt = tmpDt.ToUniversalTime
            Else
                tmpDt = New DateTime(CType(tmpDtStr.Substring(0, 4), Integer),
                                     CType(tmpDtStr.Substring(4, 2), Integer),
                                     CType(tmpDtStr.Substring(6, 2), Integer),
                                     CType(tmpDtStr.Substring(8, 2), Integer),
                                     CType(tmpDtStr.Substring(10, 2), Integer),
                                     CType(tmpDtStr.Substring(12, 2), Integer),
                                     DateTimeKind.Utc)
                tmpDt = tmpDt.ToLocalTime
            End If
            RtnStr = tmpDt.ToString(DEFAULT_DATE_FORMAT & " " & DEFAULT_TIME)
        Catch ex As Exception
            Throw
        End Try
        Return RtnStr
    End Function
#End Region

#Region "日付フォーマット変更"
    ''' <summary>
    ''' 日付フォーマットを変更
    ''' </summary>
    ''' <param name="pStr">YYYYMMDD形式の変換前文字列</param>
    ''' <param name="pSeparater">区切り文字</param>
    ''' <returns>指定された区切り文字で「YYYY[区切り]MM[区切り]DD」形式に変換した値を返す</returns>
    Public Shared Function ChangeDateFormat(ByVal pStr As String,
                                            Optional ByVal pSeparater As String = "/") As String
        Dim returnStr As String = pStr
        Try
            If String.IsNullOrEmpty(returnStr) = False Then
                If returnStr.Contains(pSeparater) = False Then
                    If returnStr.Length = 8 Then
                        returnStr = returnStr.Insert(4, pSeparater)
                        returnStr = returnStr.Insert(7, pSeparater)
                    End If
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
        Return returnStr
    End Function
#End Region

#End Region

#Region "数値操作"

#Region "端数を切り上げる"
    ''' <summary>
    ''' 端数を切り上げる
    ''' </summary>
    ''' <param name="value">処理対象数値</param>
    ''' <returns>端数を切り上げた数値</returns>
    ''' <remarks>
    ''' <para>Input:"1" Out:"1"</para>
    ''' <para>Input:"1.1" Out:"2"</para>
    ''' <para>Input:"-1" Out:"-1"</para>
    ''' <para>Input:"-1.1" Out:"-2"</para>
    ''' </remarks>
    Public Shared Function RoundUp(ByVal value As Decimal) As Decimal
        Dim blnNegative As Boolean
        If value < 0 Then
            blnNegative = True
        End If
        value = Math.Ceiling(Math.Abs(value))
        If blnNegative = True Then
            value = value * -1
        End If
        Return value
    End Function
#End Region

#Region "端数を切り捨てる"
    ''' <summary>
    ''' 端数を切り捨てる
    ''' </summary>
    ''' <param name="value">処理対象数値</param>
    ''' <returns>端数を切り捨てた数値</returns>
    Public Shared Function RoundDown(ByVal value As Decimal) As Decimal
        Dim blnNegative As Boolean
        If value < 0 Then
            blnNegative = True
        End If
        value = Math.Floor(Math.Abs(value))
        If blnNegative = True Then
            value = value * -1
        End If
        Return value
    End Function
#End Region

#Region "指定した精度の数値に四捨五入する。"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 指定した精度の数値に四捨五入します。
    ''' </summary>
    ''' <param name="dValue">丸め対象の倍精度浮動小数点数。</param>
    ''' <param name="iDigits">戻り値の有効桁数の精度。</param>
    ''' <returns>iDigits に等しい精度の数値に四捨五入された数値。</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/07/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overloads Shared Function ToHalfAjust(ByVal dValue As Double,
                                                 ByVal iDigits As Integer) As Double
        Dim dCoef As Double = 0
        Dim Result As Double = 0
        Try
            dCoef = System.Math.Pow(10, iDigits)

            If dValue > 0 Then
                Result = System.Math.Floor((dValue * dCoef) + 0.5) / dCoef
            Else
                Result = System.Math.Ceiling((dValue * dCoef) - 0.5) / dCoef
            End If

        Catch ex As Exception
            Throw
        End Try
        Return Result
    End Function
    ''' <summary>
    ''' 指定した精度の数値に四捨五入します。
    ''' </summary>
    ''' <param name="dValue">丸め対象の倍精度浮動小数点数。</param>
    ''' <param name="iDigits">戻り値の有効桁数の精度。</param>
    ''' <returns>iDigits に等しい精度の数値に四捨五入された数値。</returns>
    ''' <remarks>
    ''' </remarks>
    Public Overloads Shared Function ToHalfAjust(ByVal dValue As Single,
                                                 ByVal iDigits As Integer) As Single
        Return CType(ToHalfAjust(CType(dValue, Decimal), iDigits), Single)
    End Function
    ''' <summary>
    ''' 指定した精度の数値に四捨五入します。
    ''' </summary>
    ''' <param name="dValue">丸め対象の倍精度浮動小数点数。</param>
    ''' <param name="iDigits">戻り値の有効桁数の精度。</param>
    ''' <returns>iDigits に等しい精度の数値に四捨五入された数値。</returns>
    ''' <remarks>
    ''' </remarks>
    Public Overloads Shared Function ToHalfAjust(ByVal dValue As Decimal,
                                                 ByVal iDigits As Integer) As Decimal
        Dim dCoef As Decimal = 0
        Dim Result As Decimal = 0
        Try
            dCoef = CType(System.Math.Pow(10, iDigits), Decimal)

            If dValue > 0 Then
                Result = System.Math.Floor((dValue * dCoef) + 0.5D) / dCoef
            Else
                Result = System.Math.Ceiling((dValue * dCoef) - 0.5D) / dCoef
            End If

        Catch ex As Exception
            Throw
        End Try
        Return Result
    End Function
#End Region

#End Region

#Region "ランダムな値を取得する"
    Public Overloads Shared Function GetRundom(ByVal pStartNum As Integer,
                                               ByVal pEndNum As Integer) As Integer
        Return rnd2.Next(pStartNum, pEndNum + 1)
    End Function
    Public Overloads Shared Function GetRundom() As Double
        ' 取得した乱数を返す
        Return rnd2.NextDouble()
    End Function
    Public Overloads Shared Function GetRundom(ByVal pLength As Integer) As Byte()
        Dim bs(pLength) As Byte
        rnd2.NextBytes(bs)
        ' 取得した乱数を返す
        Return bs
    End Function
    Public Shared Function GetStrictRundom(ByVal pLength As Integer,
                                           Optional ByVal pExceptZero As Boolean = False) As Byte()
        '暗号化に使用する厳密なランダムバイトを作成する
        '100バイト長のバイト型配列を作成
        Dim random() As Byte = New Byte(pLength) {}

        Try
            If pExceptZero = True Then
                'バイト配列に暗号化に使用する厳密な0以外の値のランダムシーケンスを設定
                Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
                    rng.GetNonZeroBytes(random)
                End Using
            Else
                'バイト配列に暗号化に使用する厳密な値のランダムシーケンスを設定
                RandomNumberGenerator.Fill(random)
            End If
            ' 取得した乱数を返す
            Return random
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' GUIDを使用してHEXの文字列を取得する
    ''' </summary>
    ''' <param name="pLength">取得する文字列長</param>
    ''' <returns>引数の長さのHEX文字列</returns>
    ''' <remarks>引数の値が不正(0未満、及びGUID超)の場合、Emptyを返す</remarks>
    Public Shared Function GetHexRundom(ByVal pLength As Integer) As String
        'GUIDの取得
        Dim guidResult As String = System.Guid.NewGuid().ToString().Replace("-", String.Empty)
        'Make sure length is valid
        If pLength <= 0 OrElse pLength > guidResult.Length Then
            'エラーにしたくなかったため、ワーニング出力を行う
            GlobalVariables.Logger.Warn("Length must be between 1 and " & guidResult.Length)
            Return String.Empty
        End If
        'Return the first length bytes
        Return guidResult.Substring(0, pLength)
    End Function
#End Region

#Region "色名やRGB数値から色オブジェクトを取得する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 色オブジェクトを取得する
    ''' </summary>
    ''' <param name="pColor">色の名前またはRGB数値</param>
    ''' <param name="pDefaultColor">エラー時の色オブジェクト</param>
    ''' <returns>pColorの色オブジェクト</returns>
    ''' <remarks>
    ''' 色名やRGB数値から色オブジェクトを取得する
    ''' </remarks>
    ''' <history>
    ''' 	[CNAITO]	2007/03/05	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function GetColor(ByVal pColor As String,
                                    ByVal pDefaultColor As System.Drawing.Color) As System.Drawing.Color
        Const Separater As Char = CChar(",")
        Const Space As Char = CChar(" ")
        Dim RGB() As String
        Dim Color As System.Drawing.Color

        Try
            pColor = pColor.TrimStart(Space)
            If pColor.IndexOf(",") > -1 Then
                RGB = pColor.Split(Separater)
                If RGB.Length <> 3 Then
                    Throw New FormatException
                End If
                For i As Integer = 0 To 2
                    If IsNumeric(RGB(i)) = False Then
                        Throw New ApplicationException
                    Else
                        If CType(RGB(i), Integer) < 0 Or
                           CType(RGB(i), Integer) > 256 Then
                            Throw New FormatException
                        End If
                    End If
                Next
                Color = System.Drawing.Color.FromArgb(CType(RGB(0), Byte), CType(RGB(1), Byte), CType(RGB(2), Byte))
            Else
                Color = System.Drawing.Color.FromName(pColor)
            End If
        Catch ex As Exception
            Color = pDefaultColor
        End Try

        Return Color
    End Function
#End Region

#Region "指定したファイルのmimeタイプを取得する"
    Public Shared Function GetMimeFromFile(ByVal pFullPath As String) As String
        Dim mimeout As IntPtr = Nothing
        Dim MaxContent As Integer = 0
        Dim fs As FileStream = Nothing
        Dim buf() As Byte = Nothing
        Dim result As Integer = 0
        Dim mime As String = String.Empty
        Try
            If System.IO.File.Exists(pFullPath) = False Then
                Throw New FileNotFoundException()
            End If

            MaxContent = CInt(New FileInfo(pFullPath).Length)
            If MaxContent > 256 Then
                MaxContent = 256
            End If

            ReDim buf(MaxContent)

            result = FindMimeFromData(IntPtr.Zero,
                                      pFullPath,
                                      buf,
                                      MaxContent,
                                      Nothing,
                                      0,
                                      mimeout,
                                      0)

            If result = 0 Then
                mime = System.Runtime.InteropServices.Marshal.PtrToStringUni(mimeout)
                System.Runtime.InteropServices.Marshal.FreeCoTaskMem(mimeout)
            Else
                mime = String.Empty
            End If

        Catch ex As Exception
            mime = String.Empty
        Finally
            If fs IsNot Nothing Then
                fs.Close()
                fs.Dispose()
            End If
            fs = Nothing
        End Try
        Return mime
    End Function
#End Region

#Region "暗号化/復号"

    ' TODO: FIX HERE
    '#Region "文字列をRC2アルゴリズムにより暗号化する"
    '    ''' -----------------------------------------------------------------------------
    '    ''' <summary>
    '    ''' 文字列をRC2アルゴリズムにより暗号化する
    '    ''' </summary>
    '    ''' <param name="str">暗号化する文字列</param>
    '    ''' <param name="key">パスワード</param>
    '    ''' <returns>暗号化された文字列</returns>
    '    ''' <history>
    '    ''' 	[SHonda]	2007/03/05	Created
    '    ''' </history>
    '    ''' -----------------------------------------------------------------------------
    '    Public Shared Function EncryptString(ByVal str As String,
    '                                         ByVal key As String) As String
    '        Dim Result As String = String.Empty
    '        Dim bytesIn As Byte()
    '        Dim rc2 As RC2CryptoServiceProvider = Nothing
    '        Dim bytesKey As Byte()
    '        Dim msOut As MemoryStream = Nothing
    '        Dim rc2decrypt As ICryptoTransform = Nothing
    '        Dim cryptStreem As CryptoStream = Nothing
    '        Dim bytesOut As Byte()

    '        Try
    '            If String.IsNullOrEmpty(str) = False AndAlso
    '               String.IsNullOrEmpty(key) = False Then
    '                '文字列をバイト型配列にする
    '                bytesIn = Encoding.UTF8.GetBytes(str)
    '                'RC2CryptoServiceProviderオブジェクトの作成
    '                rc2 = New RC2CryptoServiceProvider

    '                '共有キーと初期化ベクタを決定
    '                'パスワードをバイト配列にする
    '                bytesKey = Encoding.UTF8.GetBytes(key)

    '                '共有キーと初期化ベクタを設定
    '                rc2.Key = bytesKey
    '                rc2.IV = Encoding.UTF8.GetBytes(INITIAL_VECTOR)

    '                '暗号化されたデータを書き出すためのMemoryStream
    '                msOut = New System.IO.MemoryStream
    '                'RC2暗号化オブジェクトの作成
    '                rc2decrypt = rc2.CreateEncryptor()

    '                '書き込むためのCryptoStreamの作成
    '                cryptStreem = New CryptoStream(msOut,
    '                                               rc2decrypt,
    '                                               CryptoStreamMode.Write)

    '                '書き込む
    '                cryptStreem.Write(bytesIn, 0, bytesIn.Length)
    '                cryptStreem.FlushFinalBlock()
    '                '暗号化されたデータを取得
    '                bytesOut = msOut.ToArray()

    '                '閉じる
    '                cryptStreem.Close()
    '                msOut.Close()

    '                'Base64で文字列に変更して結果を返す
    '                Result = Convert.ToBase64String(bytesOut)
    '            Else
    '                Result = str
    '            End If
    '        Catch ex As Exception
    '            Result = str
    '        Finally
    '            If rc2 IsNot Nothing Then
    '                rc2.Clear()
    '            End If
    '            rc2 = Nothing
    '            If msOut IsNot Nothing Then
    '                Try
    '                    msOut.Close()
    '                Catch ex As Exception

    '                End Try
    '                msOut.Dispose()
    '            End If
    '            msOut = Nothing
    '            If rc2decrypt IsNot Nothing Then
    '                rc2decrypt.Dispose()
    '            End If
    '            rc2decrypt = Nothing
    '            If cryptStreem IsNot Nothing Then
    '                Try
    '                    cryptStreem.Close()
    '                Catch ex As Exception

    '                End Try
    '                cryptStreem.Dispose()
    '            End If
    '        End Try
    '        Return Result
    '    End Function
    '#End Region

    ' TODO: FIX HERE
    '#Region "RC2アルゴリズムにより暗号化された文字列を復号する"
    '    ''' -----------------------------------------------------------------------------
    '    ''' <summary>
    '    ''' RC2アルゴリズムにより暗号化された文字列を復号する
    '    ''' </summary>
    '    ''' <param name="str">暗号化された文字列</param>
    '    ''' <param name="key">パスワード</param>
    '    ''' <returns>復号化された文字列</returns>
    '    ''' <history>
    '    ''' 	[SHonda]	2007/03/05	Created
    '    ''' </history>
    '    ''' -----------------------------------------------------------------------------
    '    Public Shared Function DecryptString(ByVal str As String,
    '                                         ByVal key As String) As String
    '        Dim rc2 As RC2CryptoServiceProvider = Nothing
    '        Dim bytesKey As Byte()
    '        Dim bytesIn As Byte()
    '        Dim msIn As MemoryStream = Nothing
    '        Dim rc2decrypt As ICryptoTransform = Nothing
    '        Dim cryptStreem As CryptoStream = Nothing
    '        Dim srOut As StreamReader = Nothing
    '        Dim result As String = String.Empty

    '        Try
    '            If String.IsNullOrEmpty(str) = False AndAlso
    '               String.IsNullOrEmpty(key) = False Then
    '                'RC2CryptoServiceProviderオブジェクトの作成
    '                rc2 = New RC2CryptoServiceProvider

    '                '共有キーと初期化ベクタを決定
    '                'パスワードをバイト配列にする
    '                bytesKey = Encoding.UTF8.GetBytes(key)

    '                '共有キーと初期化ベクタを設定
    '                rc2.Key = bytesKey
    '                rc2.IV = Encoding.UTF8.GetBytes(INITIAL_VECTOR)

    '                'Base64で文字列をバイト配列に戻す
    '                bytesIn = Convert.FromBase64String(str)
    '                '暗号化されたデータを読み込むためのMemoryStream
    '                msIn = New MemoryStream(bytesIn)
    '                'DES復号化オブジェクトの作成
    '                rc2decrypt = rc2.CreateDecryptor()
    '                '読み込むためのCryptoStreamの作成
    '                cryptStreem = New CryptoStream(msIn,
    '                                               rc2decrypt,
    '                                               CryptoStreamMode.Read)


    '                '復号化されたデータを取得するためのStreamReader
    '                srOut = New StreamReader(cryptStreem, Encoding.UTF8)
    '                '復号化されたデータを取得する
    '                result = srOut.ReadToEnd()

    '                '閉じる
    '                srOut.Close()
    '                cryptStreem.Close()
    '                msIn.Close()
    '            Else
    '                result = str
    '            End If
    '        Catch ex As Exception
    '            result = str
    '        Finally
    '            If rc2 IsNot Nothing Then
    '                rc2.Clear()
    '            End If
    '            rc2 = Nothing
    '            If msIn IsNot Nothing Then
    '                Try
    '                    msIn.Close()
    '                Catch ex As Exception

    '                End Try
    '                msIn.Dispose()
    '            End If
    '            msIn = Nothing
    '            If rc2decrypt IsNot Nothing Then
    '                rc2decrypt.Dispose()
    '            End If
    '            rc2decrypt = Nothing
    '            If cryptStreem IsNot Nothing Then
    '                Try
    '                    cryptStreem.Close()
    '                Catch ex As Exception

    '                End Try
    '                cryptStreem.Dispose()
    '            End If
    '            cryptStreem = Nothing
    '            If srOut IsNot Nothing Then
    '                Try
    '                    srOut.Close()
    '                Catch ex As Exception

    '                End Try
    '                srOut.Dispose()
    '            End If
    '            srOut = Nothing
    '        End Try

    '        Return result
    '    End Function
    '#End Region

    '#Region "文字列をRC4アルゴリズムにより暗号化する"
    '    Public Shared Function EncryptStringRC4(ByVal str As String,
    '                                            ByVal key As String) As String
    '        Dim pKeyArray As Byte() = EncodeByteArray(key)
    '        Return EncryptRC4(pKeyArray.ToString, str, False)
    '    End Function
    '    ' * 
    '    ' * The symmetric encryption function 
    '    ' * 
    '    ' * @param string pKey Key to encrypt with (can be binary of hex) 
    '    ' * @param string data Content to be encrypted 
    '    ' * @param bool ispwdHex Key passed is in hexadecimal or not 
    '    ' * @access public 
    '    ' * @return string 
    '    Private Shared Function EncryptRC4(ByVal pKey As String, ByVal data As String, ByVal isPwdHex As Boolean) As String
    '        Dim a As Integer, i As Integer, j As Integer, k As Integer, tmp As Integer, pwd_length As Integer,
    '        data_length As Integer
    '        Dim key As Integer(), box As Integer()
    '        Dim cipher As Byte()
    '        'string cipher; 

    '        If isPwdHex Then
    '            'pwd = pack("H*", strKey)
    '            pKey = pack(pKey)
    '        End If
    '        ' valid input, please! 
    '        pwd_length = pKey.Length
    '        data_length = data.Length
    '        key = New Integer(255) {}
    '        box = New Integer(255) {}
    '        cipher = New Byte(data.Length - 1) {}
    '        'cipher = ""; 

    '        For i = 0 To 255
    '            key(i) = ord(pKey(i Mod pwd_length))
    '            box(i) = i
    '        Next
    '        j = InlineAssignHelper(i, 0)
    '        While i < 256
    '            j = (j + box(i) + key(i)) Mod 256
    '            tmp = box(i)
    '            box(i) = box(j)
    '            box(j) = tmp
    '            i += 1
    '        End While
    '        a = InlineAssignHelper(j, InlineAssignHelper(i, 0))
    '        While i < data_length
    '            a = (a + 1) Mod 256
    '            j = (j + box(a)) Mod 256
    '            tmp = box(a)
    '            box(a) = box(j)
    '            box(j) = tmp
    '            k = box(((box(a) + box(j)) Mod 256))
    '            'cipher += chr(ord(data[i]) ^ k); 
    '            cipher(i) = CByte((ord(data(i)) Xor k))
    '            i += 1
    '        End While
    '        'return cipher; 
    '        Return Encoding.GetEncoding(1252).GetString(cipher)
    '    End Function
    '#End Region

    '#Region "RC4アルゴリズムにより暗号化された文字列を復号する"
    '    Public Shared Function DecryptStringRC4(ByVal str As String,
    '                                            ByVal key As String) As String
    '        Dim pKeyArray As Byte() = EncodeByteArray(key)
    '        Return DecryptRC4(pKeyArray.ToString, str, False)
    '    End Function
    '    ' * 
    '    ' * Decryption, recall encryption 
    '    ' * 
    '    ' * @param string pKey Key to decrypt with (can be binary of hex) 
    '    ' * @param string data Content to be decrypted 
    '    ' * @param bool ispwdHex Key passed is in hexadecimal or not 
    '    ' * @access public 
    '    ' * @return string 
    '    Private Shared Function DecryptRC4(ByVal pKey As String, ByVal data As String, ByVal isPwdHex As Boolean) As String
    '        Return EncryptRC4(pKey, data, isPwdHex)
    '    End Function
    '#End Region

    '#Region "3DESアルゴリズムにより文字列を暗号化する"
    '    Public Shared Function Encrypt(ByVal originalStr As String,
    '                                   ByVal pKeyStr As String,
    '                                   ByVal pIvStr As String) As String
    '        Dim originalData As Byte() = Nothing
    '        Try
    '            '文字列をバイト配列に変換
    '            originalData = Encoding.Unicode.GetBytes(originalStr)

    '            Dim encrypted() As Byte = Nothing
    '            Try
    '                '暗号化する
    '                encrypted = Encrypt(originalData, pKeyStr, pIvStr)
    '                Return ToHexString(encrypted, True)
    '            Catch ex As Exception
    '                Throw
    '            Finally
    '                Erase encrypted
    '                encrypted = Nothing
    '            End Try
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase originalData
    '            originalData = Nothing
    '        End Try
    '    End Function
    '#End Region

    '#Region "3DESアルゴリズムにより暗号化された文字列を復号する"
    '    Public Shared Function Decrypt(ByVal encryptedStr As String,
    '                                   ByVal pKeyStr As String,
    '                                   ByVal pIvStr As String) As String
    '        Dim encrypted() As Byte = Nothing
    '        Try
    '            GlobalVariables.Logger.Debug(encryptedStr & "-" & pKeyStr & "-" & pIvStr)
    '            '文字列をバイト配列に変換
    '            encrypted = FromHexString(encryptedStr)
    '            Dim decrypted() As Byte = Nothing
    '            Try
    '                '複合する
    '                decrypted = Decrypt(encrypted, pKeyStr, pIvStr)
    '                Return Encoding.Unicode.GetString(decrypted)
    '            Catch ex As Exception
    '                Throw
    '            Finally
    '                Erase decrypted
    '                decrypted = Nothing
    '            End Try
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase encrypted
    '            encrypted = Nothing
    '        End Try
    '    End Function
    '#End Region

    '#Region "3DESアルゴリズムによりバイト配列を暗号化する"
    '    Public Shared Function Encrypt(ByVal pRawData As Byte(),
    '                                   ByVal pKeyStr As String,
    '                                   ByVal pIvStr As String) As Byte()
    '        Dim original As Byte() = pRawData
    '        Try
    '            ' 鍵は128ビット(16バイト)である必要がある
    '            Dim key() As Byte = Encoding.ASCII.GetBytes(pKeyStr)
    '            Dim iv() As Byte = Encoding.ASCII.GetBytes(pIvStr)

    '            Dim encrypted() As Byte = Nothing
    '            Using ms As New MemoryStream()
    '                Using cs As New CryptoStream(ms,
    '                                             New TripleDESCryptoServiceProvider().CreateEncryptor(key, iv),
    '                                             CryptoStreamMode.Write)
    '                    cs.Write(original, 0, original.Length)
    '                    cs.FlushFinalBlock()
    '                    cs.Close()
    '                End Using
    '                encrypted = ms.ToArray()
    '                ms.Close()
    '            End Using
    '            Return encrypted
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase original
    '            original = Nothing
    '        End Try
    '    End Function
    '    '#End Region

    'TODO: FIX HERE
    '#Region "3DESアルゴリズムにより暗号化されたバイト配列を復号する"
    '    Public Shared Function Decrypt(ByVal pEncryptedData As Byte(),
    '                                   ByVal pKeyStr As String,
    '                                   ByVal pIvStr As String) As Byte()
    '        Dim encrypted() As Byte = pEncryptedData
    '        Try
    '            ' 鍵は128ビット(16バイト)である必要がある
    '            Dim key() As Byte = Encoding.ASCII.GetBytes(pKeyStr)
    '            Dim iv() As Byte = Encoding.ASCII.GetBytes(pIvStr)

    '            Dim decrypted() As Byte = Nothing
    '            Using ms As New MemoryStream()
    '                Using cs As New CryptoStream(ms,
    '                                             New TripleDESCryptoServiceProvider().CreateDecryptor(key, iv),
    '                                             CryptoStreamMode.Write)
    '                    cs.Write(encrypted, 0, encrypted.Length)
    '                    cs.FlushFinalBlock()
    '                    cs.Close()
    '                End Using
    '                decrypted = ms.ToArray()
    '                ms.Close()
    '            End Using
    '            Return decrypted
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase encrypted
    '            encrypted = Nothing
    '        End Try
    '    End Function
    '#End Region

#End Region

    '#Region "時間計測(CPU起動時（OS起動時）からの経過時間を出力)"
    '    Public Shared Function GetNowMSec() As Double
    '        Dim cnt As Long = 0
    '        Dim frq As Long = 0
    '        Dim dblTemp As Double = 0.0
    '        Dim returnVal As Double = 0.0
    '        Try
    '            Call QueryPerformanceFrequency(frq)
    '            If frq <> 0 Then
    '                '高分解能パフォーマンスカウンタ使用
    '                Call QueryPerformanceCounter(cnt)
    '                returnVal = cnt * 1000 / frq
    '            Else
    '                returnVal = GetTickCount()
    '            End If
    '        Catch ex As Exception
    '            Throw
    '        End Try
    '        Return returnVal
    '    End Function
    '    Public Shared Function GetTimeFormat(ByVal pMilliseconds As Double) As String

    '        Dim tsElapsed As TimeSpan
    '        Dim blnNegative As Boolean = False
    '        Dim strReturn As String = String.Empty

    '        Try
    '            If pMilliseconds < 0 Then
    '                pMilliseconds = Math.Abs(pMilliseconds)
    '                blnNegative = True
    '            End If

    '            tsElapsed = TimeSpan.FromMilliseconds(pMilliseconds)

    '            If tsElapsed.Days > 0 Then
    '                strReturn = tsElapsed.Days.ToString("#,##0") & ":"
    '            End If
    '            strReturn = strReturn & tsElapsed.Hours.ToString("00") & ":"
    '            strReturn = strReturn & tsElapsed.Minutes.ToString("00") & ":"
    '            strReturn = strReturn & tsElapsed.Seconds.ToString("00")
    '            If tsElapsed.Milliseconds > 0 Then
    '                strReturn = strReturn & "." & tsElapsed.Milliseconds.ToString("#")
    '            End If
    '            If blnNegative = True Then
    '                strReturn = "-" & strReturn
    '            End If
    '        Catch ex As Exception
    '            strReturn = String.Empty
    '        End Try
    '        Return strReturn
    '    End Function
    '#End Region

    '#Region "外部プログラムを実行"

    '#Region "外部プログラムを実行(終了を待たない = 戻り値を取らない)"
    '    Public Shared Function ExecuteProgramNoWait(ByVal pCommand As String,
    '                                                Optional ByVal pArgs As String = "",
    '                                                Optional isBatFile As Boolean = False) As Boolean
    '        Dim ret As Boolean = False
    '        Dim psi As New ProcessStartInfo
    '        Try
    '            '**************************************************
    '            '* 外部プログラムを実行する                       *
    '            '**************************************************
    '            If isBatFile Then
    '                'ComSpecのパスを取得する
    '                psi.FileName = System.Environment.GetEnvironmentVariable("ComSpec")
    '                'コマンドラインを指定（"/c"は実行後に閉じるために必要）
    '                psi.Arguments = "/c " & pCommand & " " & pArgs
    '            Else
    '                '実行コマンドを指定する
    '                psi.FileName = pCommand
    '                'コマンドライン引数を指定
    '                psi.Arguments = pArgs
    '            End If
    '            '標準入力のリダイレクト
    '            psi.RedirectStandardInput = True
    '            '標準出力のリダイレクト
    '            psi.RedirectStandardOutput = True
    '            '標準エラー出力のリダイレクト
    '            psi.RedirectStandardError = True
    '            'ウィンドウの表示
    '            psi.CreateNoWindow = True
    '            'OSのシェルを使用しての起動
    '            If isBatFile Then
    '                psi.UseShellExecute = True
    '            Else
    '                psi.UseShellExecute = False
    '            End If
    '            'ユーザープロファイルの使用
    '            psi.LoadUserProfile = False
    '            '起動失敗時のエラーダイアログの表示
    '            psi.ErrorDialog = False
    '            '実行
    '            Using p As Process = Process.Start(psi)
    '                Try
    '                    'プロセス終了までまたない
    '                    p.WaitForExit(0)
    '                Catch ex As Exception
    '                    Throw
    '                Finally
    '                    p.Close()
    '                End Try
    '            End Using
    '            ret = True
    '        Catch ex As Exception
    '            Err.Clear()
    '        Finally
    '            psi = Nothing
    '        End Try
    '        Return ret
    '    End Function
    '#End Region

    '#Region "外部プログラムを実行"
    '    Public Shared Function ExecuteProgram(ByVal pCommand As String,
    '                                          ByVal pArgs As String,
    '                                          ByRef pStdResult As String,
    '                                          ByRef pErrResult As String,
    '                                          Optional ByVal pWorkingDirectory As String = "",
    '                                          Optional ByVal pMaxWait As Integer = 0,
    '                                          Optional isBatFile As Boolean = False) As Boolean

    '        Dim TrimChar() As Char = {CChar(vbCr), CChar(vbLf), CChar(vbCrLf), CChar(String.Empty)}
    '        Dim ret As Boolean = False
    '        Dim stdEncording As Encoding = Encoding.GetEncoding(0)
    '        Dim errEncording As Encoding = Encoding.GetEncoding(0)

    '        pStdResult = String.Empty
    '        pErrResult = String.Empty

    '        Try
    '            '**************************************************
    '            '* 外部プログラムを実行し出力データを取得する     *
    '            '**************************************************
    '            Dim psi As New ProcessStartInfo
    '            Try
    '                If isBatFile Then
    '                    'ComSpecのパスを取得する
    '                    psi.FileName = System.Environment.GetEnvironmentVariable("ComSpec")
    '                    'コマンドラインを指定（"/c"は実行後に閉じるために必要）
    '                    psi.Arguments = "/c " & pCommand & " " & pArgs
    '                Else
    '                    '実行コマンドを指定する
    '                    psi.FileName = pCommand
    '                    'コマンドライン引数を指定
    '                    psi.Arguments = pArgs
    '                End If
    '                '標準入力のリダイレクト
    '                psi.RedirectStandardInput = True
    '                '標準出力のリダイレクト
    '                psi.RedirectStandardOutput = True
    '                'エラー出力のリダイレクト
    '                psi.RedirectStandardError = True
    '                'ウィンドウの表示
    '                psi.CreateNoWindow = True
    '                'OSのシェルの使用
    '                If isBatFile Then
    '                    psi.UseShellExecute = True
    '                Else
    '                    psi.UseShellExecute = False
    '                End If
    '                'ユーザープロファイルの使用
    '                psi.LoadUserProfile = False
    '                '起動失敗時のエラーダイアログの表示
    '                psi.ErrorDialog = False
    '                'プロセスを起動するときの起動ディレクトリ
    '                If String.IsNullOrEmpty(pWorkingDirectory) Then
    '                    pWorkingDirectory = Path.GetDirectoryName(pCommand)
    '                End If
    '                psi.WorkingDirectory = pWorkingDirectory
    '                '実行
    '                Using p As Process = Process.Start(psi)
    '                    Try
    '                        'プロセス終了まで待機
    '                        If pMaxWait = 0 Then
    '                            p.WaitForExit()
    '                        Else
    '                            ret = p.WaitForExit(pMaxWait)
    '                        End If

    '                        '標準出力結果エンコードを取得
    '                        stdEncording = p.StandardOutput.CurrentEncoding
    '                        '標準出力結果を格納
    '                        Try
    '                            pStdResult = NZ(p.StandardOutput.ReadToEnd).Trim
    '                        Catch ex As Exception
    '                            Throw
    '                        Finally
    '                            'ストリームリーダーを閉じる
    '                            If p.StandardOutput IsNot Nothing Then p.StandardOutput.Close()
    '                        End Try

    '                        '標準エラー出力結果を格納
    '                        If psi.RedirectStandardError Then
    '                            '標準エラー出力結果エンコードを取得
    '                            errEncording = p.StandardError.CurrentEncoding
    '                            Try
    '                                pErrResult = NZ(p.StandardError.ReadToEnd).Trim
    '                            Catch ex As Exception
    '                                Throw
    '                            Finally
    '                                'ストリームリーダーを閉じる
    '                                If p.StandardError IsNot Nothing Then p.StandardError.Close()
    '                            End Try
    '                        End If
    '                    Catch ex As Exception
    '                        Throw
    '                    Finally
    '                        'プロセス開放
    '                        If p IsNot Nothing Then p.Close()
    '                    End Try
    '                End Using

    '            Catch ex As Exception
    '                Throw
    '            Finally
    '                psi = Nothing
    '            End Try

    '        Catch ex As Exception
    '            pErrResult &= "Error occured." & vbCrLf &
    '                          "Info:" & ex.Message & vbCrLf &
    '                          "StackTrace:" & ex.StackTrace & vbCrLf &
    '                          ex.ToString
    '        End Try
    '        Return ret
    '    End Function
    '#End Region

    '#End Region

    '#Region "フォルダ作成"
    '    Public Shared Function CreateFolder(ByRef pFolderPath As String) As Boolean
    '        Dim tmpPath As String = String.Empty
    '        Dim tmpAry() As String = Nothing
    '        Dim result As Boolean = False
    '        Try
    '            If pFolderPath.StartsWith(Path.DirectorySeparatorChar & Path.DirectorySeparatorChar) = False Then
    '                Do
    '                    If pFolderPath.EndsWith(Path.DirectorySeparatorChar) = True Then
    '                        pFolderPath = pFolderPath.Substring(0, pFolderPath.Length - 1)
    '                    Else
    '                        Exit Do
    '                    End If
    '                Loop

    '                tmpAry = pFolderPath.Split(Path.DirectorySeparatorChar)
    '                If tmpAry Is Nothing OrElse tmpAry.Length < 2 Then
    '                    Throw New ApplicationException("Invalid directory order. Path : " & pFolderPath)
    '                End If

    '                For i As Integer = 1 To tmpAry.Length - 1
    '                    tmpAry(i) = ConvertInvalidFileNameChars(tmpAry(i))
    '                Next
    '                pFolderPath = String.Empty
    '                For i As Integer = 0 To tmpAry.Length - 1
    '                    pFolderPath &= tmpAry(i)
    '                    If i < tmpAry.Length - 1 Then
    '                        pFolderPath &= Path.DirectorySeparatorChar
    '                    End If
    '                Next

    '                tmpPath = tmpAry(0) & Path.DirectorySeparatorChar
    '                If Directory.Exists(tmpPath) = False Then
    '                    Throw New DriveNotFoundException
    '                End If

    '                For i As Integer = 1 To tmpAry.Length - 1
    '                    tmpPath &= tmpAry(i) & Path.DirectorySeparatorChar
    '                    If Directory.Exists(tmpPath) = False Then
    '                        Directory.CreateDirectory(tmpPath)
    '                    End If
    '                Next
    '            End If
    '            result = True
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase tmpAry
    '            tmpAry = Nothing
    '        End Try
    '        Return result
    '    End Function
    '#End Region

    'TODO: CREATE FIX FOR ALL PLATFORMS, NOT ONLY WINDOWS

    '#Region "フォルダアクセス権変更"
    '    Public Shared Function ChangeFolderAcl(ByVal pFullPath As String,
    '                                           Optional ByVal pIdentity As String = "Everyone",
    '                                           Optional ByVal pFileSystemRights As FileSystemRights = FileSystemRights.FullControl,
    '                                           Optional ByVal pInheritanceFlags As InheritanceFlags = InheritanceFlags.ContainerInherit,
    '                                           Optional ByVal pPropagationFlags As PropagationFlags = PropagationFlags.None,
    '                                           Optional ByVal pAccessControlType As AccessControlType = AccessControlType.Allow) As Boolean
    '        Dim DirInfo As DirectoryInfo
    '        Dim DirSec As DirectorySecurity
    '        Dim AccessRule As FileSystemAccessRule
    '        Dim blnResult As Boolean = False
    '        Try
    '            If Directory.Exists(pFullPath) = False Then
    '                If CreateFolder(pFullPath) = False Then
    '                    Throw New IOException("To create folder failed.")
    '                End If
    '            End If

    '            DirInfo = New DirectoryInfo(pFullPath)

    '            DirSec = DirInfo.GetAccessControl()
    '            AccessRule = New FileSystemAccessRule(pIdentity,
    '                                                  pFileSystemRights,
    '                                                  pInheritanceFlags,
    '                                                  pPropagationFlags,
    '                                                  pAccessControlType)

    '            'アクセス権限を指定
    '            'Everyoneに対し、フルコントロールの許可
    '            '（サブフォルダ、及び、ファイルにも適用）
    '            'AccessRule = New FileSystemAccessRule("Everyone", _
    '            '                                      FileSystemRights.FullControl, _
    '            '                                      InheritanceFlags.ContainerInherit, _
    '            '                                      PropagationFlags.None, _
    '            '                                      AccessControlType.Allow)
    '            'アクセス権限を追加
    '            DirSec.AddAccessRule(AccessRule)
    '            DirInfo.SetAccessControl(DirSec)

    '            blnResult = True
    '        Catch ex As Exception
    '            Debug.WriteLine(ex.Message)
    '            Err.Clear()
    '            blnResult = False
    '        End Try
    '        Return blnResult
    '    End Function
    '#End Region

#Region "短いファイルパス名取得"
    ''' <summary>
    ''' 短いファイルパス名を取得する
    ''' </summary>
    ''' <param name="path">ファイルのパス</param>
    ''' <returns>短いパス名</returns>
    Public Shared Function GetShortPath(ByVal path As String,
                                        Optional ByVal pRaiseExceptionWhenError As Boolean = False) As String
        Dim sb As New System.Text.StringBuilder(1023)
        Dim ret As Integer = GetShortPathName(path, sb, sb.Capacity)
        If ret = 0 Then
            If pRaiseExceptionWhenError Then
                Throw New Exception("短いファイル名の取得に失敗しました。")
            End If
            Return path
        End If
        Return sb.ToString()
    End Function
#End Region

#Region "GZip圧縮/伸張"

#Region "ファイル圧縮/伸張"

#Region "GZipファイル圧縮"
    Public Shared Sub GZipCompressFile(ByVal pFileFullPath As String,
                                       Optional ByVal pDeleteSourceFile As Boolean = False)
        Dim outFile As String = String.Empty
        Dim num As Integer = 0
        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する

        Try
            ' 出力ファイルの拡張子は「.gz」
            outFile = Path.GetFileName(pFileFullPath) + ".gz"

            ' 入力ストリーム
            Using inStream As FileStream = New FileStream(pFileFullPath, FileMode.Open, FileAccess.Read)
                inStream.Seek(0, 0)
                ' 出力ストリーム
                Using outStream As FileStream = New FileStream(outFile, FileMode.Create)
                    ' 圧縮ストリーム
                    Using compStream As GZipStream = New GZipStream(outStream, CompressionMode.Compress)
                        Do
                            num = inStream.Read(buf, 0, buf.Length)
                            If num <= 0 Then Exit Do
                            compStream.Write(buf, 0, num)
                        Loop
                        compStream.Flush()
                        compStream.Close()
                    End Using
                    outStream.Flush()
                    outStream.Close()
                End Using
                inStream.Flush()
                inStream.Close()
            End Using
            If pDeleteSourceFile = True Then
                File.Delete(pFileFullPath)
            End If
        Catch ex As Exception
            Throw
        Finally
            Erase buf
            buf = Nothing
        End Try
    End Sub
#End Region

#Region "GZipファイル解凍"
    Public Shared Sub GZipDeCompressFile(ByVal pFileFullPath As String,
                                         Optional ByVal pDeleteSourceFile As Boolean = False)
        Dim outFile As String = String.Empty
        Dim num As Integer = 0
        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する

        Try
            ' 入力ファイルは.gzファイルのみ有効
            If Not pFileFullPath.ToLower().EndsWith(".gz") Then
                Return
            End If

            ' ファイル名末尾の「.gz」を削除
            outFile = pFileFullPath.Substring(0, pFileFullPath.Length - 3)

            ' 入力ストリーム
            Using inStream As FileStream = New FileStream(pFileFullPath, FileMode.Open, FileAccess.Read)
                inStream.Seek(0, 0)
                ' 出力ストリーム
                Using outStream As FileStream = New FileStream(outFile, FileMode.Create)
                    ' 解凍ストリーム
                    Using decompStream As GZipStream = New GZipStream(inStream, CompressionMode.Decompress)
                        Do
                            num = decompStream.Read(buf, 0, buf.Length)
                            If num <= 0 Then Exit Do
                            outStream.Write(buf, 0, num)
                        Loop
                        decompStream.Flush()
                        decompStream.Close()
                    End Using
                    outStream.Flush()
                    outStream.Close()
                End Using
                inStream.Flush()
                inStream.Close()
            End Using
            If pDeleteSourceFile = True Then
                File.Delete(pFileFullPath)
            End If
        Catch ex As Exception
            Throw
        Finally
            Erase buf
            buf = Nothing
        End Try
    End Sub
#End Region

#End Region

#Region "Byte配列圧縮/伸張"

#Region "ByteToByteGZip圧縮"
    Public Shared Function GZipCompressByte(ByVal data As Byte()) As Byte()
        Dim num As Integer = 0
        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する
        Dim outByte() As Byte = Nothing
        Try
            ' 入力ストリーム
            If data IsNot Nothing AndAlso data.LongLength > 0 Then
                Using inMemStrm As MemoryStream = New MemoryStream(data, False)
                    inMemStrm.Seek(0, 0)
                    ' 出力ストリーム
                    Using outMemStrm As MemoryStream = New MemoryStream
                        ' 圧縮ストリーム
                        Using gzipStrm As GZipStream = New GZipStream(outMemStrm, CompressionMode.Compress)
                            Do
                                num = inMemStrm.Read(buf, 0, buf.Length)
                                If num <= 0 Then Exit Do
                                gzipStrm.Write(buf, 0, num)
                            Loop
                            gzipStrm.Flush()
                            gzipStrm.Close()
                        End Using
                        outByte = outMemStrm.ToArray()
                        outMemStrm.Flush()
                        outMemStrm.Close()
                    End Using
                End Using
            End If
        Catch ex As Exception
            Throw
        Finally
            Erase buf
            buf = Nothing
        End Try
        Return outByte
    End Function
#End Region

#Region "ByteToByteGZip解凍"
    Public Shared Function GZipDecompressByte(ByVal data As Byte()) As Byte()
        Dim num As Integer = 0
        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する
        Dim outByte() As Byte = Nothing
        Try
            ' 入力ストリーム
            If data IsNot Nothing AndAlso data.LongLength > 0 Then
                Using inMemStrm As MemoryStream = New MemoryStream(data, False)
                    ' 解凍ストリーム
                    inMemStrm.Seek(0, 0)
                    Using outMemStream As MemoryStream = New MemoryStream
                        ' 出力ストリーム
                        Using gzipStrm As GZipStream = New GZipStream(inMemStrm, CompressionMode.Decompress)
                            Do
                                num = gzipStrm.Read(buf, 0, buf.Length)
                                If num <= 0 Then Exit Do
                                outMemStream.Write(buf, 0, num)
                            Loop
                            gzipStrm.Flush()
                            gzipStrm.Close()
                        End Using
                        outByte = outMemStream.ToArray()
                        outMemStream.Flush()
                        outMemStream.Close()
                    End Using
                    inMemStrm.Flush()
                    inMemStrm.Close()
                End Using
            End If
        Catch ex As Exception
            Throw
        Finally
            Erase buf
            buf = Nothing
        End Try
        Return outByte
    End Function
#End Region

#End Region

#Region "Stream <--> Gzip圧縮Byte配列"

#Region "StreamToByteGZip圧縮"
    Public Shared Function GZipCompressStream(ByVal pStream As MemoryStream) As Byte()
        Dim num As Integer = 0
        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する
        Dim outByte() As Byte = Nothing
        Try
            If pStream IsNot Nothing AndAlso pStream.Length > 0 Then
                ' 入力ストリーム
                Using inMemStrm As MemoryStream = pStream
                    inMemStrm.Seek(0, 0)
                    ' 出力ストリーム
                    Using outMemStrm As MemoryStream = New MemoryStream
                        ' 圧縮ストリーム
                        Using gzipStrm As GZipStream = New GZipStream(outMemStrm, CompressionMode.Compress)
                            Do
                                num = inMemStrm.Read(buf, 0, buf.Length)
                                If num <= 0 Then Exit Do
                                gzipStrm.Write(buf, 0, num)
                            Loop
                            gzipStrm.Flush()
                            gzipStrm.Close()
                        End Using
                        outByte = outMemStrm.ToArray()
                        outMemStrm.Flush()
                        outMemStrm.Close()
                    End Using
                End Using
            End If
        Catch ex As Exception
            Throw
        Finally
            Erase buf
            buf = Nothing
        End Try
        Return outByte
    End Function
#End Region

#Region "ByteToStreamGZip解凍"
    Public Shared Function GZipDecompressStream(ByVal data As Byte()) As MemoryStream
        Dim num As Integer = 0
        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する
        Dim outMemStream As MemoryStream = Nothing
        Try
            If data IsNot Nothing AndAlso data.LongLength > 0 Then
                ' 入力ストリーム
                Using inMemStrm As MemoryStream = New MemoryStream(data, False)
                    ' 解凍ストリーム
                    inMemStrm.Seek(0, 0)
                    outMemStream = New MemoryStream
                    ' 出力ストリーム
                    Using gzipStrm As GZipStream = New GZipStream(inMemStrm, CompressionMode.Decompress)
                        Do
                            num = gzipStrm.Read(buf, 0, buf.Length)
                            If num <= 0 Then Exit Do
                            outMemStream.Write(buf, 0, num)
                        Loop
                        gzipStrm.Flush()
                        gzipStrm.Close()
                    End Using
                    inMemStrm.Flush()
                    inMemStrm.Close()

                    outMemStream.Flush()
                    outMemStream.Seek(0, 0)
                End Using
            Else
                outMemStream = New MemoryStream
            End If
        Catch ex As Exception
            Throw
        Finally
            Erase buf
            buf = Nothing
        End Try
        Return outMemStream
    End Function
#End Region

#End Region

#Region "Dataset <--> Gzip圧縮Byte配列"

    '#Region "DataSetToByteGZip圧縮"
    '    Public Shared Function GZipCompressDataSet(ByVal ds As DataSet) As Byte()
    '        Dim bf As BinaryFormatter = Nothing
    '        Dim inMemStream As MemoryStream = Nothing
    '        Dim outByte As Byte() = Nothing
    '        Try
    '            If ds IsNot Nothing Then
    '                '入力ストリーム
    '                inMemStream = New MemoryStream

    '                'データセットのバイナリフォーマットでのシリアライズ
    '                ds.RemotingFormat = SerializationFormat.Binary
    '                bf = New BinaryFormatter
    '                bf.Serialize(inMemStream, ds)
    '                'バイト配列化
    '                inMemStream.Seek(0, 0)
    '                outByte = inMemStream.ToArray
    '                '圧縮
    '                outByte = GZipCompressByte(outByte)
    '            End If
    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            bf = Nothing
    '            If inMemStream IsNot Nothing Then
    '                inMemStream.Flush()
    '                inMemStream.Close()
    '                inMemStream.Dispose()
    '            End If
    '            inMemStream = Nothing
    '        End Try
    '        Return outByte
    '    End Function
    '#End Region

    '#Region "GZipDataSet解凍"
    '    Public Shared Function GZipDecompressDataSet(ByVal data As Byte()) As DataSet
    '        Dim bf As BinaryFormatter = Nothing
    '        Dim tmpByte() As Byte = Nothing
    '        Dim outDs As DataSet = Nothing
    '        Dim tmpMemStream As MemoryStream = Nothing
    '        Try
    '            If data IsNot Nothing AndAlso data.LongLength > 0 Then
    '                tmpByte = GZipDecompressByte(data)

    '                outDs = New DataSet

    '                '作業用ストリーム
    '                tmpMemStream = New MemoryStream(tmpByte)
    '                tmpMemStream.Seek(0, 0)

    '                'デシリアライズフォーマットの定義
    '                outDs.RemotingFormat = SerializationFormat.Binary
    '                'フォーマッタの作成
    '                bf = New BinaryFormatter
    '                'デシリアライズ化
    '                outDs = CType(bf.Deserialize(tmpMemStream, Nothing), DataSet)
    '            Else
    '                outDs = New DataSet
    '            End If

    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase tmpByte
    '            tmpByte = Nothing
    '            bf = Nothing
    '            If tmpMemStream IsNot Nothing Then
    '                tmpMemStream.Flush()
    '                tmpMemStream.Close()
    '                tmpMemStream.Dispose()
    '            End If
    '            tmpMemStream = Nothing
    '        End Try
    '        Return outDs.Copy
    '    End Function
    '#End Region

#End Region

#End Region

    '#Region "ストリーム操作"
    '    Public Shared Function ConvertByteToStream(ByVal pData() As Byte) As MemoryStream
    '        Dim ms As New MemoryStream
    '        Try
    '            If pData IsNot Nothing AndAlso pData.LongLength > 0 Then
    '                ms.Write(pData, 0, pData.Length)
    '                ms.Seek(0, 0)
    '            End If
    '        Catch ex As Exception
    '            Throw
    '        End Try
    '        Return ms
    '    End Function
    '    Public Shared Function ConvertStreamToByte(ByVal pMs As MemoryStream) As Byte()
    '        If pMs IsNot Nothing AndAlso pMs.Length > 0 Then
    '            pMs.Position = 0
    '            Return pMs.ToArray()
    '        Else
    '            Return Nothing
    '        End If
    '    End Function
    '    Public Shared Function ConvertStreamToStream(ByRef inStream As Stream,
    '                                                 ByVal outStreamType As Type) As Stream
    '        Dim num As Integer = 0
    '        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する
    '        Dim outStream As Stream = DirectCast(Activator.CreateInstance(outStreamType, False), Stream)
    '        Try
    '            ' 入力ストリーム
    '            If inStream Is Nothing OrElse inStream.CanRead = False Then
    '                Exit Try
    '            End If

    '            inStream.Seek(0, 0)

    '            outStream.Seek(0, 0)

    '            ' 出力ストリーム
    '            Do
    '                num = inStream.Read(buf, 0, buf.Length)
    '                If num <= 0 Then Exit Do
    '                outStream.Write(buf, 0, num)
    '            Loop
    '            inStream.Seek(0, 0)

    '            outStream.Flush()
    '            outStream.Seek(0, 0)

    '        Catch ex As Exception
    '            Throw
    '        Finally
    '            Erase buf
    '            buf = Nothing
    '        End Try
    '        Return outStream
    '    End Function
    '#End Region

#Region "エンコーディング取得"
    Public Shared Function GetEncoding(Optional ByVal pCodePage As Integer = 0) As System.Text.Encoding
        Dim encord As System.Text.Encoding = Nothing
        Try
            If pCodePage > 0 Then
                encord = System.Text.Encoding.GetEncoding(pCodePage)
            Else
                encord = System.Text.Encoding.GetEncoding(DEFAULT_ENCORDING)
            End If
        Catch ex As Exception
            encord = System.Text.Encoding.GetEncoding(GlobalVariables.AppCultureInfo.TextInfo.ANSICodePage)
        End Try
        Return encord
    End Function
#End Region

#Region "コンボボックス用単純テーブル作成"
    Public Shared Function ConvertArrayToTableForComboBox(ByVal pDisplayMembers() As String,
                                                          Optional ByVal pValueMembers() As String = Nothing,
                                                          Optional ByVal pTableName As String = "COMBO_SOURCE",
                                                          Optional ByVal pAddBlank As Boolean = False) As DataTable
        Const DISPLAY_COL_NAME As String = "NAME"
        Const VALUE_COL_NAME As String = "VALUE"
        Dim dt As New DataTable(pTableName)
        Dim blnUsedataMembers As Boolean = False

        If pValueMembers IsNot Nothing AndAlso
           pDisplayMembers.Length = pValueMembers.Length Then
            blnUsedataMembers = True
        End If

        dt.Columns.Add(New DataColumn(DISPLAY_COL_NAME, System.Type.GetType("System.String")))
        dt.Columns.Add(New DataColumn(VALUE_COL_NAME, System.Type.GetType("System.String")))

        If pAddBlank = True Then
            With dt
                For i As Integer = 0 To pDisplayMembers.Length - 1
                    .Rows.Add()
                    .Rows(i).Item(DISPLAY_COL_NAME) = pDisplayMembers(i)
                    If blnUsedataMembers = True Then
                        .Rows(i).Item(VALUE_COL_NAME) = pValueMembers(i)
                    Else
                        .Rows(i).Item(VALUE_COL_NAME) = pDisplayMembers(i)
                    End If
                Next
            End With
        Else
            With dt
                For i As Integer = 0 To pDisplayMembers.Length - 2
                    .Rows.Add()
                    .Rows(i).Item(DISPLAY_COL_NAME) = pDisplayMembers(i + 1)
                    If blnUsedataMembers = True Then
                        .Rows(i).Item(VALUE_COL_NAME) = pValueMembers(i + 1)
                    Else
                        .Rows(i).Item(VALUE_COL_NAME) = pDisplayMembers(i + 1)
                    End If
                Next
            End With
        End If
        Return dt
    End Function
    'Public Shared Function ConvertArrayToTableForComboBox(ByVal pItemStr() As String, _
    '                                                      ByVal pItemDataStr() As String, _
    '                                                      Optional ByVal pTableName As String = "COMBO_SOURCE") As DataTable
    '    Dim Dt As DataTable = Nothing
    '    Dim Col As DataColumn = Nothing
    '    Dim dr As DataRow = Nothing
    '    Try
    '        Dt = New DataTable(pTableName)

    '        Col = New DataColumn
    '        Col.DataType = System.Type.GetType("System.String")
    '        Col.ColumnName = "CODE"
    '        Col.Unique = True
    '        Col.DefaultValue = DBNull.Value
    '        Dt.Columns.Add(Col)

    '        Col = New DataColumn
    '        Col.DataType = System.Type.GetType("System.String")
    '        Col.ColumnName = "NAME"
    '        Col.DefaultValue = DBNull.Value
    '        Dt.Columns.Add(Col)

    '        If pItemDataStr IsNot Nothing Then
    '            If pItemStr IsNot Nothing Then
    '                Select Case pItemDataStr.Length
    '                    Case Is = pItemStr.Length
    '                        For i As Integer = 0 To pItemDataStr.Length - 1
    '                            dr = Dt.NewRow
    '                            With dr
    '                                .BeginEdit()
    '                                .Item("CODE") = pItemDataStr(i)
    '                                .Item("NAME") = pItemStr(i)
    '                                .EndEdit()
    '                            End With
    '                            Dt.Rows.Add(dr)
    '                        Next
    '                    Case Is > pItemStr.Length
    '                        For i As Integer = 0 To pItemDataStr.Length - 1
    '                            dr = Dt.NewRow
    '                            With dr
    '                                .BeginEdit()
    '                                .Item("CODE") = pItemDataStr(i)
    '                                If i > pItemStr.Length - 1 Then
    '                                    .Item("NAME") = pItemDataStr(i)
    '                                Else
    '                                    .Item("NAME") = pItemStr(i)
    '                                End If
    '                                .EndEdit()
    '                            End With
    '                            Dt.Rows.Add(dr)
    '                        Next
    '                    Case Is < pItemStr.Length
    '                        For i As Integer = 0 To pItemStr.Length - 1
    '                            dr = Dt.NewRow
    '                            With dr
    '                                .BeginEdit()
    '                                If i > pItemDataStr.Length - 1 Then
    '                                    .Item("CODE") = pItemStr(i)
    '                                Else
    '                                    .Item("CODE") = pItemDataStr(i)
    '                                End If
    '                                .Item("NAME") = pItemStr(i)
    '                                .EndEdit()
    '                            End With
    '                            Dt.Rows.Add(dr)
    '                        Next
    '                End Select
    '            Else
    '                For i As Integer = 0 To pItemDataStr.Length - 1
    '                    dr = Dt.NewRow
    '                    With dr
    '                        .BeginEdit()
    '                        .Item("CODE") = pItemDataStr(i)
    '                        .Item("NAME") = pItemDataStr(i)
    '                        .EndEdit()
    '                    End With
    '                    Dt.Rows.Add(dr)
    '                Next
    '            End If
    '        Else
    '            If pItemStr IsNot Nothing Then
    '                For i As Integer = 0 To pItemStr.Length - 1
    '                    dr = Dt.NewRow
    '                    With dr
    '                        .BeginEdit()
    '                        .Item("CODE") = pItemStr(i)
    '                        .Item("NAME") = pItemStr(i)
    '                        .EndEdit()
    '                    End With
    '                    Dt.Rows.Add(dr)
    '                Next
    '            End If
    '        End If

    '        Dt.AcceptChanges()
    '    Catch ex As HnException
    '        ex.AddStackFrame(New StackFrame(True)) ' スタックフレームのインスタンスを追加
    '        Throw                               ' 上位に例外を投げる
    '    Catch ex As Exception When HnCommonConstant.UseHnMessages = False
    '        Throw
    '    Catch ex As Exception
    '        Throw New HnException(ex)
    '    Finally
    '        dr = Nothing
    '        If Col IsNot Nothing Then
    '            Col.Dispose()
    '        End If
    '        Col = Nothing
    '    End Try
    '    Return Dt
    'End Function
#End Region

#Region "最初に発生したException取得"
    Public Shared Function GetFirstException(ByVal pEx As Exception) As Exception
        If pEx.InnerException Is Nothing Then
            Return pEx
        End If
        Return GetFirstException(pEx.InnerException)
    End Function
    Public Shared Function GetAllErrType(ByVal pEx As Exception,
                                         ByVal pTypeList As List(Of String)) As List(Of String)
        pTypeList.Add(pEx.GetType.ToString())
        If pEx.InnerException IsNot Nothing Then
            pTypeList = GetAllErrType(pEx.InnerException, pTypeList)
        End If
        Return pTypeList
    End Function
    Public Shared Function GetAllErrMessage(ByVal pEx As Exception,
                                            ByVal pMessageList As List(Of String)) As List(Of String)
        pMessageList.Add(pEx.Message)
        If pEx.InnerException IsNot Nothing Then
            pMessageList = GetAllErrMessage(pEx.InnerException, pMessageList)
        End If
        Return pMessageList
    End Function
    Public Shared Function GetAllErrSource(ByVal pEx As Exception,
                                           ByVal pSourceList As List(Of String)) As List(Of String)
        pSourceList.Add(pEx.Source)
        If pEx.InnerException IsNot Nothing Then
            pSourceList = GetAllErrSource(pEx.InnerException, pSourceList)
        End If
        Return pSourceList
    End Function
    Public Shared Function GetAllErrStackTrace(ByVal pEx As Exception,
                                               ByVal pStackTraceList As List(Of String)) As List(Of String)
        pStackTraceList.Add(pEx.StackTrace)
        If pEx.InnerException IsNot Nothing Then
            pStackTraceList = GetAllErrStackTrace(pEx.InnerException, pStackTraceList)
        End If
        Return pStackTraceList
    End Function
    Public Shared Function GetAllErrHelpLink(ByVal pEx As Exception,
                                             ByVal pHelpLinkList As List(Of String)) As List(Of String)
        pHelpLinkList.Add(pEx.HelpLink)
        If pEx.InnerException IsNot Nothing Then
            pHelpLinkList = GetAllErrHelpLink(pEx.InnerException, pHelpLinkList)
        End If
        Return pHelpLinkList
    End Function
    Public Shared Function GetAllErrData(ByVal pEx As Exception,
                                         ByVal pDataList As List(Of IDictionary)) As List(Of IDictionary)
        pDataList.Add(pEx.Data)
        If pEx.InnerException IsNot Nothing Then
            pDataList = GetAllErrData(pEx.InnerException, pDataList)
        End If
        Return pDataList
    End Function
#End Region

#Region "ファイルをバイトデータとして読み込み"
    Public Shared Function FileRead(ByVal pFileFullPath As String,
                                    ByRef pData As Byte(),
                                    Optional ByVal pLockRead As Boolean = False,
                                    Optional ByVal pEnableWaiting As Boolean = True,
                                    Optional ByVal pWaitInterval As Integer = 3000,
                                    Optional ByVal pMaxTimes As Integer = 100) As Boolean
        Dim result As Boolean = False
        Try
            If pLockRead = True Then
                If GetSoleRightOfUse(pFileFullPath, pEnableWaiting, pWaitInterval, pMaxTimes) = False Then
                    ' タイムアウト
                    Throw New IOException("他のプロセスで使用されているためファイルオープンに失敗しました。:" & pFileFullPath)
                End If
            End If
            pData = File.ReadAllBytes(pFileFullPath)
            result = True
        Catch ex As System.Exception
            Throw
        End Try
        Return result
    End Function
#End Region

#Region "ファイルをMemoryStreamに読み込み"
    Public Shared Function FileRead(ByVal pFileFullPath As String,
                                    ByRef pStream As MemoryStream,
                                    Optional ByVal pLockRead As Boolean = False,
                                    Optional ByVal pEnableWaiting As Boolean = True,
                                    Optional ByVal pWaitInterval As Integer = 3000,
                                    Optional ByVal pMaxTimes As Integer = 100) As Boolean
        Dim result As Boolean = False
        Dim num As Integer = 0
        Dim buf(8191) As Byte  ' 8Kbytesずつ処理する
        Dim st As FileStream = Nothing
        Try
            If pLockRead = True Then
                If GetSoleRightOfUse(pFileFullPath, pEnableWaiting, pWaitInterval, pMaxTimes) = False Then
                    ' タイムアウト
                    Throw New IOException("他のプロセスで使用されているためファイルオープンに失敗しました。:" & pFileFullPath)
                End If

                st = New FileStream(pFileFullPath,
                                    FileMode.Open,
                                    FileAccess.Read,
                                    FileShare.None)
            Else
                st = New FileStream(pFileFullPath,
                                    FileMode.Open,
                                    FileAccess.Read,
                                    FileShare.Read)
            End If

            If pStream Is Nothing Then pStream = New MemoryStream

            Using inStream As FileStream = st
                inStream.Seek(0, 0)
                Do
                    num = inStream.Read(buf, 0, buf.Length)
                    If num <= 0 Then Exit Do
                    pStream.Write(buf, 0, num)
                Loop
                inStream.Flush()
                inStream.Close()
            End Using
            pStream.Seek(0, 0)
            pStream.Flush()

            result = True
        Catch ex As Exception
            Throw
        Finally
            If st IsNot Nothing Then
                st.Close()
                st.Dispose()
            End If
            st = Nothing
            Erase buf
            buf = Nothing
        End Try
        Return result
    End Function
#End Region

#Region "ファイル排他使用権獲得"
    Public Shared Function GetSoleRightOfUse(ByVal pFileFullPath As String,
                                             ByVal pEnableWaiting As Boolean,
                                             Optional ByVal pWaitInterval As Integer = 3000,
                                             Optional ByVal pMaxTimes As Integer = 100) As Boolean
        Dim Result As Boolean = False
        Dim st As Stream = Nothing
        Dim Seconds As Double = 0
        Dim TotalSeconds As Double = 0
        Try
            For i As Integer = 0 To pMaxTimes - 1
                Try
                    st = File.Open(pFileFullPath,
                                   FileMode.Open,
                                   FileAccess.Read,
                                   FileShare.None)
                    If st IsNot Nothing Then
                        Exit For
                    End If
                Catch ex As FileNotFoundException
                    Throw
                Catch ex As IOException
                    If pEnableWaiting = True Then
#If DEBUG Then
                        Trace.WriteLine(Path.GetFileName(pFileFullPath) & " : ファイルオープン待ち " & (pWaitInterval / 1000).ToString("#,##0.00") & " 秒。 (" & i & " 回目)")
#End If
                        If Seconds > 180 Then    '3分毎にアラートをログに出力
                            GlobalVariables.Logger.Warn("ファイルオープン待ち " & TotalSeconds.ToString("#,##0.00") & " 秒" & vbCrLf &
                                     Path.GetFileName(pFileFullPath))
                            Seconds = 0
                        End If
                        Seconds += pWaitInterval / 1000
                        TotalSeconds += pWaitInterval / 1000
                        System.Threading.Thread.Sleep(pWaitInterval)
                    Else
                        Throw
                    End If
                Catch ex As System.Exception
                    Throw
                End Try
            Next
            If st Is Nothing Then
                ' タイムアウト
                Throw New IOException("他のプロセスで使用されているためファイルが開けません。:" & pFileFullPath)
            End If
            Result = True
        Catch ex As System.Exception
            Throw
        Finally
            If st IsNot Nothing Then
                st.Close()
                st.Dispose()
            End If
            st = Nothing
        End Try
        Return Result
    End Function
#End Region

#Region "メッセージのエスケープ"
    Public Shared Function GetEscapedMessageText(ByVal pMessage As String,
                                                 Optional ByVal pEscapeString() As Char = Nothing) As String
        Dim txtMessage As String = pMessage
        If pEscapeString Is Nothing Then
            txtMessage = txtMessage.Replace("'", "\'")
        Else
            For Each chrEscape As String In pEscapeString
                txtMessage = txtMessage.Replace(chrEscape, "\" & chrEscape)
            Next
        End If
        Return txtMessage
    End Function
#End Region

#Region "CSV変換"

#Region "データテーブルをCSVに変換"
    Public Shared Function ConvertDataTableToCsv(ByVal pDt As DataTable,
                                                 Optional ByVal pExceptColumns As List(Of String) = Nothing,
                                                 Optional ByVal pIncludeHeader As Boolean = False,
                                                 Optional ByVal pDelimiter As Char = ","c,
                                                 Optional ByVal pEnclosure As String = "") As String
        If pDt Is Nothing Then Return String.Empty
        Return ConvertDataViewToCsv(pDt.DefaultView, pExceptColumns, pIncludeHeader, pDelimiter, pEnclosure)
    End Function
#End Region

#Region "データビューをCSVに変換"
    Public Shared Function ConvertDataViewToCsv(ByVal pDv As DataView,
                                                Optional ByVal pExceptColumns As List(Of String) = Nothing,
                                                Optional ByVal pIncludeHeader As Boolean = False,
                                                Optional ByVal pDelimiter As Char = ","c,
                                                Optional ByVal pEnclosure As String = "") As String
        If pDv Is Nothing Then Return String.Empty

        If pExceptColumns Is Nothing Then pExceptColumns = New List(Of String)
        If Not pExceptColumns.Contains("__ROWKEY") Then pExceptColumns.Add("__ROWKEY")

        Dim sb As New StringBuilder
        If pIncludeHeader Then
            Dim rowData As New StringBuilder
            For Each col As DataColumn In pDv.Table.Columns
                If pExceptColumns IsNot Nothing Then
                    Dim except As Boolean = False
                    For Each exceptColName As String In pExceptColumns
                        If col.ColumnName.Equals(exceptColName) Then
                            except = True
                            Exit For
                        End If
                    Next
                    If except Then Continue For
                End If
                If rowData.Length > 0 Then rowData.Append(pDelimiter)
                rowData.Append(pEnclosure & col.ColumnName & pEnclosure)
            Next
            sb.AppendLine(rowData.ToString)
        End If
        For Each drv As DataRowView In pDv
            If drv.Row.RowState = DataRowState.Deleted Then Continue For
            Dim rowData As New StringBuilder
            For Each col As DataColumn In pDv.Table.Columns
                If pExceptColumns IsNot Nothing Then
                    Dim except As Boolean = False
                    For Each exceptColName As String In pExceptColumns
                        If col.ColumnName.Equals(exceptColName) Then
                            except = True
                            Exit For
                        End If
                    Next
                    If except Then Continue For
                End If
                If rowData.Length > 0 Then rowData.Append(pDelimiter)
                rowData.Append(pEnclosure & NZ(drv.Item(col.ColumnName)) & pEnclosure)
            Next
            sb.AppendLine(rowData.ToString)
        Next
        Return sb.ToString
    End Function
#End Region

#End Region

#Region "DEBUG出力"

#Region "投入されたデータの内容をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' 投入されたデータの内容をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pInData">出力したいデータオブジェクト</param>
    ''' <param name="pOption">出力したい内容 0:テーブル名のみ 1:テーブル名とカラム名 2:全て</param>
    ''' <remarks>
    ''' 投入されたデータ(DataSet,DataTable,ArrayList,Collection,Array)の内容をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Shared Function DataDump(ByVal pInData As Object,
                                    Optional ByVal pOption As eDebugPrint = eDebugPrint.All,
                                    Optional ByVal pOutputMode As eOutputMode = eOutputMode.DebugPrint) As String
        Dim buff As New StringBuilder
        Dim RtnCd As Boolean = False
        Dim ResultString As String = String.Empty
        Try
            buff.AppendLine("******************************************************")
            buff.AppendLine("Start Date Time:" & Now.ToString(DEFAULT_DATE_FORMAT & " " & DEFAULT_TIME))

            If TypeOf pInData Is DataSet Then
                buff.AppendLine(DumpData(CType(pInData, DataSet), pOption))
                RtnCd = True
            End If
            If TypeOf pInData Is DataTable Then
                buff.AppendLine(DumpData(CType(pInData, DataTable), pOption))
                RtnCd = True
            End If
            If TypeOf pInData Is DataView Then
                buff.AppendLine(DumpData(CType(pInData, DataView), pOption))
                RtnCd = True
            End If
            If TypeOf pInData Is DataRow Then
                buff.AppendLine(DumpData(CType(pInData, DataRow), pOption))
                RtnCd = True
            End If
            If TypeOf pInData Is ArrayList Then
                buff.AppendLine(DumpData(CType(pInData, ArrayList), pOption))
                RtnCd = True
            End If
            If TypeOf pInData Is Collection Then
                buff.AppendLine(DumpData(CType(pInData, Collection), pOption))
                RtnCd = True
            End If
            If TypeOf pInData Is IDictionary Then
                buff.AppendLine(DumpData(CType(pInData, IDictionary), pOption))
                RtnCd = True
            End If
            If IsArray(pInData) = True Then
                buff.AppendLine(DumpData(CType(pInData, Array), pOption))
                RtnCd = True
            End If
            If RtnCd = False Then
                buff.AppendLine("There is no data which can be dumnped!")
            End If
        Catch ex As Exception
            buff.AppendLine("Sorry, this data could not be dumnped!")
        Finally
            buff.AppendLine("******************************************************")
        End Try
        Try
            ResultString = buff.ToString
#If DEBUG Then
            Select Case pOutputMode
                Case eOutputMode.DebugPrint
                    Debug.Print(ResultString)
                Case eOutputMode.Log
                    GlobalVariables.Logger.Debug(ResultString)
                Case eOutputMode.Console
                    Console.WriteLine(ResultString)
            End Select
#End If
        Finally
            buff = Nothing
        End Try
        Return ResultString
    End Function
#End Region

#Region "データテーブルの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' データテーブルの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pDataTable">出力したいデータテーブル</param>
    ''' <param name="pOption">出力したい内容 1:テーブル名のみ 2:テーブル名とカラム名 3:全て</param>
    ''' <remarks>
    ''' データテーブルの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pDataTable As DataTable,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Dim IsChanged As Boolean = False

        Try
            If pDataTable Is Nothing Then
                buff.AppendLine("The table is Nothing.")
                Return buff.ToString
            End If

            If pOption < eDebugPrint.OnlyTableName OrElse pOption > eDebugPrint.All Then
                pOption = eDebugPrint.TableNameAndColumnName
            End If

            buff.AppendLine("=============================================")
            buff.Append("Table Name :" & NZ(pDataTable.TableName))
            Try
                If pDataTable.DataSet IsNot Nothing Then
                    buff.Append(vbTab & "HasChanges:" & NZ(pDataTable.DataSet.HasChanges))
                Else
                    IsChanged = False
                    For Each dr As DataRow In pDataTable.Rows
                        If dr.RowState <> DataRowState.Unchanged Then
                            IsChanged = True
                            Exit For
                        End If
                    Next
                    buff.Append(vbTab & "HasChanges:" & NZ(IsChanged))
                End If
            Catch ex As Exception

            Finally
                buff.Append(vbCrLf)
            End Try
            If pOption = eDebugPrint.OnlyTableName Then
                Return buff.ToString
            End If

            buff.AppendLine("<Column Information>")
            buff.AppendLine("------------------------------------------------------------------------------------------")
            For i As Integer = 0 To pDataTable.Columns.Count - 1
                buff.Append(NZ(pDataTable.Columns(i).ColumnName) & vbTab)
            Next
            buff.Append("|" & vbTab)
            buff.Append("RowState" & vbTab)
            buff.Append("Version")
            buff.Append(vbCrLf)


            For i As Integer = 0 To pDataTable.Columns.Count - 1
                buff.Append(NZ(pDataTable.Columns(i).DataType) & vbTab)
            Next
            buff.Append("|")
            buff.Append(vbCrLf)
            If pOption = eDebugPrint.TableNameAndColumnName Then
                Return buff.ToString
            End If

            buff.AppendLine("------------------------------------------------------------------------------------------")

            For i As Integer = 0 To pDataTable.Rows.Count - 1
                If pDataTable.Rows(i).RowState = DataRowState.Deleted Then
                    For j As Integer = 0 To pDataTable.Columns.Count - 1
                        buff.Append(NZ(pDataTable.Rows(i).Item(j, DataRowVersion.Original)) & vbTab)
                    Next
                    buff.Append("|" & vbTab)
                    buff.Append(NZ(pDataTable.Rows(i).RowState) & vbTab)
                    buff.Append("Original")
                    buff.Append(vbCrLf)
                Else
                    For j As Integer = 0 To pDataTable.Columns.Count - 1
                        buff.Append(NZ(pDataTable.Rows(i).Item(j, DataRowVersion.Current)) & vbTab)
                    Next
                    buff.Append("|" & vbTab)
                    buff.Append(NZ(pDataTable.Rows(i).RowState) & vbTab)
                    buff.Append("Current")
                    buff.Append(vbCrLf)
                End If

                If pDataTable.Rows(i).RowState = DataRowState.Modified Then
                    For j As Integer = 0 To pDataTable.Columns.Count - 1
                        buff.Append(NZ(pDataTable.Rows(i).Item(j, DataRowVersion.Original)) & vbTab)
                    Next
                    buff.Append("|" & vbTab)
                    buff.Append("Original")
                    buff.Append(vbCrLf)
                End If
            Next
            'buff.AppendLine("---------------------------------------------")
        Catch ex As Exception
            buff.AppendLine("Sorry, this table can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        End Try
        Return buff.ToString
    End Function
#End Region

#Region "データビューの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' データビューの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pDataView">出力したいデータビュー</param>
    ''' <param name="pOption">出力したい内容 1:テーブル名のみ 2:テーブル名とカラム名 3:全て</param>
    ''' <remarks>
    ''' データビューの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pDataView As DataView,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Dim IsChanged As Boolean = False
        Try
            If pDataView Is Nothing Then
                buff.AppendLine("The view is Nothing.")
                Return buff.ToString
            End If

            If pOption < eDebugPrint.OnlyTableName OrElse pOption > eDebugPrint.All Then
                pOption = eDebugPrint.TableNameAndColumnName
            End If

            buff.AppendLine("=============================================")
            buff.Append("Table Name :" & NZ(pDataView.Table.TableName))
            Try
                If pDataView.Table.DataSet IsNot Nothing Then
                    buff.Append(vbTab & "HasChanges:" & NZ(pDataView.Table.DataSet.HasChanges))
                Else
                    IsChanged = False
                    For Each drv As DataRowView In pDataView
                        If drv.Row.RowState <> DataRowState.Unchanged Then
                            IsChanged = True
                            Exit For
                        End If
                    Next
                    buff.Append(vbTab & "HasChanges:" & NZ(IsChanged.ToString))
                End If
            Catch ex As Exception

            Finally
                buff.Append(vbCrLf)
            End Try

            If pOption = eDebugPrint.OnlyTableName Then
                Return buff.ToString
            End If

            buff.AppendLine("<Column Information>")
            buff.AppendLine("------------------------------------------------------------------------------------------")
            For i As Integer = 0 To pDataView.Table.Columns.Count - 1
                buff.Append(NZ(pDataView.Table.Columns(i).ColumnName) & vbTab)
            Next
            buff.Append("|" & vbTab)
            buff.Append("RowState" & vbTab)
            buff.Append("Version")
            buff.Append(vbCrLf)
            If pOption = eDebugPrint.TableNameAndColumnName Then
                Return buff.ToString
            End If

            buff.AppendLine("------------------------------------------------------------------------------------------")

            For Each eachRow As DataRowView In pDataView
                If eachRow.Row.RowState = DataRowState.Deleted Then
                    For Each col As DataColumn In pDataView.Table.Columns
                        buff.Append(NZ(eachRow.Row.Item(col.ColumnName, DataRowVersion.Original)) & vbTab)
                    Next
                    buff.Append("|" & vbTab)
                    buff.Append(NZ(eachRow.Row.RowState) & vbTab)
                    buff.Append("Original")
                    buff.Append(vbCrLf)
                Else
                    For Each col As DataColumn In pDataView.Table.Columns
                        buff.Append(NZ(eachRow.Item(col.ColumnName)) & vbTab)
                    Next
                    buff.Append("|" & vbTab)
                    buff.Append(NZ(eachRow.Row.RowState) & vbTab)
                    buff.Append("Current")
                    buff.Append(vbCrLf)
                End If
                If eachRow.Row.RowState = DataRowState.Modified Then
                    For Each col As DataColumn In pDataView.Table.Columns
                        buff.Append(NZ(eachRow.Row.Item(col.ColumnName, DataRowVersion.Original)) & vbTab)
                    Next
                    buff.Append("|" & vbTab)
                    buff.Append("Original")
                    buff.Append(vbCrLf)
                End If
            Next
            'buff.AppendLine("---------------------------------------------")
        Catch ex As Exception
            buff.AppendLine("Sorry, this view can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        End Try
        Return buff.ToString
    End Function
#End Region

#Region "データセットの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' データセットの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pDataSet">出力したいデータセット</param>
    ''' <param name="pOption">出力したい内容 1:テーブル名のみ 2:テーブル名とカラム名 3:全て</param>
    ''' <remarks>
    ''' データセットの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pDataSet As DataSet,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Dim i As Integer = 1
        Try
            If pDataSet Is Nothing Then
                buff.AppendLine("The dataset is Nothing.")
                Return buff.ToString
            Else
                buff.AppendLine("=============================================")
                buff.AppendLine("Dataset Name:" & NZ(pDataSet.DataSetName) & vbTab & "HasChanges:" & NZ(pDataSet.HasChanges))
            End If

            If pOption < eDebugPrint.OnlyTableName OrElse pOption > eDebugPrint.All Then
                pOption = eDebugPrint.TableNameAndColumnName
            End If

            For Each tbl As DataTable In pDataSet.Tables
                buff.AppendLine("=============================================")
                If tbl Is Nothing Then
                    buff.AppendLine("The table (" & i & ") is Nothing.")
                Else
                    buff.AppendLine("Table (" & i & ")")
                    buff.AppendLine(DumpData(tbl,
                                             pOption,
                                             pOutputMode))
                End If
                'buff.AppendLine("---------------------------------------------")
                i += 1
            Next
        Catch ex As Exception
            buff.AppendLine("Sorry, this dataset can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        End Try
        Return buff.ToString
    End Function
#End Region

#Region "ArrayListの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' ArrayListの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pArrayList">出力したいArrayList</param>
    ''' <param name="pOption">出力したい内容 1:テーブル名のみ 2:テーブル名とカラム名 3:全て</param>
    ''' <remarks>
    ''' ArrayListの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pArrayList As ArrayList,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Try
            If pArrayList Is Nothing Then
                buff.Append("The arraylist is Nothing.")
                Return buff.ToString
            End If

            buff.Append("---------------------------------------------")
            For i As Integer = 0 To pArrayList.Count - 1
                If TypeOf pArrayList.Item(i) Is ArrayList Then
                    buff.AppendLine(DumpData(CType(pArrayList.Item(i), ArrayList), pOption, pOutputMode))
                Else
                    If TypeOf pArrayList.Item(i) Is Collection Then
                        buff.AppendLine(DumpData(CType(pArrayList.Item(i), Collection), pOption, pOutputMode))
                    Else
                        If TypeOf pArrayList.Item(i) Is IDictionary Then
                            buff.AppendLine(DumpData(CType(pArrayList.Item(i), IDictionary), pOption, pOutputMode))
                        Else
                            If TypeOf pArrayList.Item(i) Is DataTable Then
                                buff.AppendLine(DumpData(CType(pArrayList.Item(i), DataTable), pOption, pOutputMode))
                            Else
                                If TypeOf pArrayList.Item(i) Is DataSet Then
                                    buff.AppendLine(DumpData(CType(pArrayList.Item(i), DataSet), pOption, pOutputMode))
                                Else
                                    If TypeOf pArrayList.Item(i) Is DataRow Then
                                        buff.AppendLine(DumpData(CType(pArrayList.Item(i), DataRow), pOption, pOutputMode))
                                    Else
                                        If TypeOf pArrayList.Item(i) Is Hashtable Then
                                            buff.AppendLine(DumpData(CType(pArrayList.Item(i), Hashtable), pOption, pOutputMode))
                                        Else
                                            If IsArray(pArrayList.Item(i)) = True Then
                                                buff.AppendLine(DumpData(CType(pArrayList.Item(i), Array), pOption, pOutputMode))
                                            Else
                                                buff.Append(i & ":" & NZ(pArrayList.Item(i)) & vbTab)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            buff.Append(vbCrLf)
            'buff.AppendLine("---------------------------------------------")
        Catch ex As Exception
            buff.AppendLine("Sorry, this arraylist can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        End Try
        Return buff.ToString
    End Function
#End Region

#Region "Collectionの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Collectionの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pCollection">出力したいCollection</param>
    ''' <param name="pOption">出力したい内容 1:テーブル名のみ 2:テーブル名とカラム名 3:全て</param>
    ''' <remarks>
    ''' Collectionの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pCollection As Collection,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Try
            If pCollection Is Nothing Then
                buff.AppendLine("The collection is Nothing.")
                Return buff.ToString
            End If

            buff.AppendLine("---------------------------------------------")
            For i As Integer = 1 To pCollection.Count
                If TypeOf pCollection.Item(i) Is Collection Then
                    buff.AppendLine(DumpData(CType(pCollection.Item(i), IDictionary), pOption, pOutputMode))
                Else
                    If TypeOf pCollection.Item(i) Is ArrayList Then
                        buff.AppendLine(DumpData(CType(pCollection.Item(i), ArrayList), pOption, pOutputMode))
                    Else
                        If TypeOf pCollection.Item(i) Is Collection Then
                            buff.AppendLine(DumpData(CType(pCollection.Item(i), Collection), pOption, pOutputMode))
                        Else
                            If TypeOf pCollection.Item(i) Is IDictionary Then
                                buff.AppendLine(DumpData(CType(pCollection.Item(i), IDictionary), pOption, pOutputMode))
                            Else
                                If TypeOf pCollection.Item(i) Is DataTable Then
                                    buff.AppendLine(DumpData(CType(pCollection.Item(i), DataTable), pOption, pOutputMode))
                                Else
                                    If TypeOf pCollection.Item(i) Is DataSet Then
                                        buff.AppendLine(DumpData(CType(pCollection.Item(i), DataSet), pOption, pOutputMode))
                                    Else
                                        If TypeOf pCollection.Item(i) Is DataRow Then
                                            buff.AppendLine(DumpData(CType(pCollection.Item(i), DataRow), pOption, pOutputMode))
                                        Else
                                            If TypeOf pCollection.Item(i) Is Hashtable Then
                                                buff.AppendLine(DumpData(CType(pCollection.Item(i), Hashtable), pOption, pOutputMode))
                                            Else
                                                If IsArray(pCollection.Item(i)) = True Then
                                                    buff.AppendLine(DumpData(CType(pCollection.Item(i), Array), pOption, pOutputMode))
                                                Else
                                                    buff.Append(i & ":" & NZ(pCollection.Item(i)) & vbTab)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            buff.Append(vbCrLf)
        Catch ex As Exception
            buff.AppendLine("Sorry, this collection can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        End Try
        Return buff.ToString
    End Function
#End Region

#Region "IDictionaryの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' IDictionaryの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pDictionary">出力したいIDictionary</param>
    ''' <param name="pOption">出力したい内容 1:テーブル名のみ 2:テーブル名とカラム名 3:全て</param>
    ''' <remarks>
    ''' Collectionの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pDictionary As IDictionary,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Dim Keys As String() = Nothing
        Dim objs() As Object = Nothing

        Try
            If pDictionary Is Nothing Then
                buff.AppendLine("The collection is Nothing.")
                Return buff.ToString
            End If

            buff.AppendLine("---------------------------------------------")

            If pDictionary.Keys IsNot Nothing Then
                ReDim Keys(pDictionary.Keys.Count - 1)
                pDictionary.Keys.CopyTo(Keys, 0)
                For Each key As String In Keys
                    If TypeOf pDictionary.Item(key) Is Collection Then
                        buff.AppendLine(DumpData(CType(pDictionary.Item(key), IDictionary), pOption, pOutputMode))
                    Else
                        If TypeOf pDictionary.Item(key) Is Collection Then
                            buff.AppendLine(DumpData(CType(pDictionary.Item(key), Collection), pOption, pOutputMode))
                        Else
                            If TypeOf pDictionary.Item(key) Is ArrayList Then
                                buff.AppendLine(DumpData(CType(pDictionary.Item(key), ArrayList), pOption, pOutputMode))
                            Else
                                If TypeOf pDictionary.Item(key) Is IDictionary Then
                                    buff.AppendLine(DumpData(CType(pDictionary.Item(key), IDictionary), pOption, pOutputMode))
                                Else
                                    If TypeOf pDictionary.Item(key) Is DataTable Then
                                        buff.AppendLine(DumpData(CType(pDictionary.Item(key), DataTable), pOption, pOutputMode))
                                    Else
                                        If TypeOf pDictionary.Item(key) Is DataSet Then
                                            buff.AppendLine(DumpData(CType(pDictionary.Item(key), DataSet), pOption, pOutputMode))
                                        Else
                                            If TypeOf pDictionary.Item(key) Is DataRow Then
                                                buff.AppendLine(DumpData(CType(pDictionary.Item(key), DataRow), pOption, pOutputMode))
                                            Else
                                                If IsArray(pDictionary.Item(key)) = True Then
                                                    buff.AppendLine(DumpData(CType(pDictionary.Item(key), Array), pOption, pOutputMode))
                                                Else
                                                    buff.Append(NZ(key) & ":" & NZ(pDictionary.Item(key)) & vbTab)
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            Else
                If pDictionary.Values IsNot Nothing Then
                    ReDim objs(pDictionary.Keys.Count - 1)
                    pDictionary.Values.CopyTo(objs, 0)
                    For Each obj As Object In objs
                        If TypeOf obj Is Collection Then
                            buff.AppendLine(DumpData(CType(obj, IDictionary), pOption, pOutputMode))
                        Else
                            If TypeOf obj Is Collection Then
                                buff.AppendLine(DumpData(CType(obj, Collection), pOption, pOutputMode))
                            Else
                                If TypeOf obj Is ArrayList Then
                                    buff.AppendLine(DumpData(CType(obj, ArrayList), pOption, pOutputMode))
                                Else
                                    If TypeOf obj Is IDictionary Then
                                        buff.AppendLine(DumpData(CType(obj, IDictionary), pOption, pOutputMode))
                                    Else
                                        If TypeOf obj Is DataTable Then
                                            buff.AppendLine(DumpData(CType(obj, DataTable), pOption, pOutputMode))
                                        Else
                                            If TypeOf obj Is DataSet Then
                                                buff.AppendLine(DumpData(CType(obj, DataSet), pOption, pOutputMode))
                                            Else
                                                If TypeOf obj Is DataRow Then
                                                    buff.AppendLine(DumpData(CType(obj, DataRow), pOption, pOutputMode))
                                                Else
                                                    If IsArray(obj) = True Then
                                                        buff.AppendLine(DumpData(CType(obj, Array), pOption, pOutputMode))
                                                    Else
                                                        buff.Append(NZ(obj) & vbTab)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                Else
                    buff.AppendLine("Sorry, this collection can not be dumped!")
                End If
            End If

            buff.Append(vbCrLf)
            'buff.AppendLine("---------------------------------------------")
        Catch ex As Exception
            buff.AppendLine("Sorry, this collection can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        Finally
            Erase Keys
            Keys = Nothing
        End Try
        Return buff.ToString
    End Function
#End Region

#Region "Arrayの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Arrayの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pArray">出力したいArray</param>
    ''' <remarks>
    ''' Arrayの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pArray As Array,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Try
            If pArray Is Nothing Then
                buff.AppendLine("The array is Nothing.")
                Return buff.ToString
            End If

            buff.AppendLine("---------------------------------------------")
            For i As Integer = 0 To pArray.Length - 1
                If IsArray(pArray.GetValue(i)) Then
                    buff.AppendLine(DumpData(CType(pArray.GetValue(i), Array), pOption, pOutputMode))
                Else
                    If TypeOf pArray.GetValue(i) Is ArrayList Then
                        buff.AppendLine(DumpData(CType(pArray.GetValue(i), ArrayList), pOption, pOutputMode))
                    Else
                        If TypeOf pArray.GetValue(i) Is Collection Then
                            buff.AppendLine(DumpData(CType(pArray.GetValue(i), Collection), pOption, pOutputMode))
                        Else
                            If TypeOf pArray.GetValue(i) Is IDictionary Then
                                buff.AppendLine(DumpData(CType(pArray.GetValue(i), IDictionary), pOption, pOutputMode))
                            Else
                                If TypeOf pArray.GetValue(i) Is DataTable Then
                                    buff.AppendLine(DumpData(CType(pArray.GetValue(i), DataTable), pOption, pOutputMode))
                                Else
                                    If TypeOf pArray.GetValue(i) Is DataSet Then
                                        buff.AppendLine(DumpData(CType(pArray.GetValue(i), DataSet), pOption, pOutputMode))
                                    Else
                                        If TypeOf pArray.GetValue(i) Is DataRow Then
                                            buff.AppendLine(DumpData(CType(pArray.GetValue(i), DataRow), pOption, pOutputMode))
                                        Else
                                            If IsArray(pArray.GetValue(i)) = True Then
                                                buff.AppendLine(DumpData(CType(pArray.GetValue(i), Array), pOption, pOutputMode))
                                            Else
                                                buff.Append(i & ":" & NZ(pArray.GetValue(i)) & vbTab)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            buff.Append(vbCrLf)
            'buff.AppendLine("---------------------------------------------")
        Catch ex As Exception
            buff.AppendLine("Sorry, this array can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        End Try
        Return buff.ToString
    End Function
#End Region

#Region "DataRowの中身をコンソールに出力する(DEBUG用)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DataRowの中身をコンソールに出力する(DEBUG用)
    ''' </summary>
    ''' <param name="pDataRow">出力したいDataRow</param>
    ''' <remarks>
    ''' DataRowの中身をコンソールに出力する
    ''' </remarks>
    ''' <history>
    ''' 	[SHonda]	2007/03/26	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Overloads Shared Function DumpData(ByVal pDataRow As DataRow,
                                               Optional ByVal pOption As eDebugPrint = eDebugPrint.OnlyTableName,
                                               Optional ByVal pOutputMode As eOutputMode = eOutputMode.Console) As String
        Dim buff As New StringBuilder
        Dim TempArray As Array
        Try
            If pDataRow Is Nothing Then
                buff.AppendLine("The DataRow is Nothing.")
                Return buff.ToString
            End If

            If pOption < eDebugPrint.OnlyTableName OrElse pOption > eDebugPrint.All Then
                pOption = eDebugPrint.TableNameAndColumnName
            End If

            buff.AppendLine("---------------------------------------------")
            buff.AppendLine("Table Name :" & pDataRow.Table.TableName)
            If pOption = eDebugPrint.OnlyTableName Then
                Return buff.ToString
            End If

            buff.AppendLine("Column Information")
            For i As Integer = 0 To pDataRow.Table.Columns.Count - 1
                buff.Append(pDataRow.Table.Columns(i).ColumnName & vbTab)
            Next
            buff.Append(vbCrLf)
            If pOption = eDebugPrint.TableNameAndColumnName Then
                Return buff.ToString
            End If

            TempArray = pDataRow.ItemArray
            buff.AppendLine(DumpData(TempArray, pOption))
            Erase TempArray
            'buff.AppendLine("---------------------------------------------")
        Catch ex As Exception
            buff.AppendLine("Sorry, this DataRow can not be dumped!")
            buff.AppendLine("Error info:" & ex.Message)
        Finally
            Erase TempArray
            TempArray = Nothing
        End Try
        Return buff.ToString
    End Function
#End Region

#End Region

#Region "eJobResult変換"
    Public Shared Function GetJobResultValue(ByVal pJobResultString As String) As PublicEnum.eJobResult
        Try
            Dim ret As PublicEnum.eJobResult
            If [Enum].TryParse(Of PublicEnum.eJobResult)(pJobResultString, ret) Then Return ret
            Return PublicEnum.eJobResult.Success
        Catch ex As Exception
            GlobalVariables.Logger.Error("JobResult変換エラー：String -> Enum", ex)
            Return PublicEnum.eJobResult.Success
        End Try
    End Function
    Public Shared Function GetJobResultName(ByVal pJobResultValue As PublicEnum.eJobResult) As String
        Try
            Return System.Enum.GetName(GetType(PublicEnum.eJobResult), pJobResultValue)
        Catch ex As Exception
            GlobalVariables.Logger.Error("JobResult変換エラー：Enum -> String", ex)
            Return String.Empty
        End Try
    End Function
#End Region

#Region "RuntimeEnvironment変換"
    Public Shared Function GetRuntimeEnvironmentValue(ByVal pRuntimeEnvironmentString As String) As PublicEnum.eRuntimeEnvironment
        Try
            Dim ret As PublicEnum.eRuntimeEnvironment
            If [Enum].TryParse(Of PublicEnum.eRuntimeEnvironment)(pRuntimeEnvironmentString, ret) Then Return ret
            Return PublicEnum.eRuntimeEnvironment.PROD
        Catch ex As Exception
            GlobalVariables.Logger.Error("実行環境変換エラー：String -> Enum", ex)
            Return PublicEnum.eRuntimeEnvironment.PROD
        End Try
    End Function
    Public Shared Function GetRuntimeEnvironmentName(ByVal pRuntimeEnvironmentValue As PublicEnum.eRuntimeEnvironment) As String
        Try
            Return System.Enum.GetName(GetType(PublicEnum.eRuntimeEnvironment), pRuntimeEnvironmentValue)
        Catch ex As Exception
            GlobalVariables.Logger.Error("実行環境変換エラー：Enum -> String", ex)
            Return String.Empty
        End Try
    End Function
#End Region

#Region "メッセージ取得"
    Public Shared Function GetMessage(ByVal pMessageCode As String, ByVal ParamArray Values() As String) As String
        Dim ms As MessageSet = GlobalVariables.Messages.GetMessageSet(pMessageCode)
        '項目名とかを置き換えたい場合は{0}形式でメッセージに埋め込み、呼び出し時にValuesのパラメータ配列に
        '項目名とかをぶっこんで呼び出してください。
        'メッセージは「AppData\Settings\Messages.xml」に定義してください。
        If Values IsNot Nothing Then
            ms.Message = String.Format(ms.Message, Values)
        End If
        Return ms.Message
    End Function
    Public Shared Function GetMessageSet(ByVal pMessageCode As String, ByVal ParamArray Values() As String) As MessageSet
        Dim ms As MessageSet = GlobalVariables.Messages.GetMessageSet(pMessageCode)
        '項目名とかを置き換えたい場合は{0}形式でメッセージに埋め込み、呼び出し時にValuesのパラメータ配列に
        '項目名とかをぶっこんで呼び出してください。
        'メッセージは「AppData\Settings\Messages.xml」に定義してください。
        If Values IsNot Nothing Then
            ms.Message = String.Format(ms.Message, Values)
        End If
        Return ms
    End Function
#End Region

#Region "名前空間取得"
    Public Shared Function GetNameSpace(ByVal pTypeName As String) As String
        If pTypeName.Contains(".") AndAlso Not pTypeName.EndsWith(".") Then
            Dim array As String() = pTypeName.Split("."c)
            Dim ret As String = String.Empty
            For i As Integer = 0 To array.Length - 2
                If Not String.IsNullOrEmpty(ret) Then ret &= "."
                ret &= array(i)
            Next
            Return ret
        Else
            Return pTypeName.Replace(".", String.Empty)
        End If
    End Function
#End Region

    '#Region "DVD対応ロジック"
    '    ''' <summary>
    '    ''' インストールIDを発行する
    '    ''' </summary>
    '    ''' <param name="pDate">発行日付</param>
    '    ''' <param name="pDeviceId">8桁のデバイスID</param>
    '    ''' <returns>16桁のインストールID</returns>
    '    ''' <remarks></remarks>
    '    Public Function CreateDvdInstallId(ByVal pDate As Date, ByVal pDeviceId As String) As String
    '        'システム入力下限の年数
    '        Const INPUT_MIN_DATE As Date = #1/1/2012#
    '        'システム入力上限の年数（これを超えると計算式上オーバーフローする為、予防措置を行っておく）
    '        Const INPUT_MAX_DATE As Date = #5/18/2039#

    '        Const passPhrase As String = "ah9jUctwJ8pjtSU44Jzq88gPcd"
    '        '入力上限・下限チェック
    '        If pDeviceId Is Nothing OrElse pDeviceId.Length <> 8 Then Return String.Empty
    '        If pDate < INPUT_MIN_DATE OrElse
    '           pDate > INPUT_MAX_DATE Then Return String.Empty
    '        '日付情報を変換して整数値へ(2012年1月1日が0とする)
    '        Dim countOfDate As Long = DateDiff(DateInterval.Day, INPUT_MIN_DATE, pDate)
    '        Console.WriteLine("日付差分 : " & countOfDate)
    '        'デバイスID後半4桁及び、日付数値4桁を変換元のコードとする
    '        Dim code As String = pDeviceId.Substring(4, 4) & String.Format("{0:0000}", countOfDate)
    '        '3DES暗号化結果の返却
    '        Return Utilities.Encrypt(code, passPhrase, GlobalVariables.TripleDesIv)
    '    End Function
    '#End Region

#Region "ディレクトリ・ファイルの完全削除"
    'HACK: http://dobon.net/vb/dotnet/file/deletedirectory.html 参照した
    ''' <summary>
    ''' フォルダを根こそぎ削除する（ReadOnlyでも削除）
    ''' </summary>
    ''' <param name="dir">削除するフォルダ</param>
    ''' <param name="IsCurrentDelete">True指定したディレクトリ自体も削除</param>
    Public Overloads Shared Sub DeleteDirectory(ByVal dir As String, Optional ByVal IsCurrentDelete As Boolean = True)
        'DirectoryInfoオブジェクトの作成
        Dim di As New System.IO.DirectoryInfo(dir)
        DeleteDirectory(di, IsCurrentDelete)
    End Sub
    ''' <summary>
    ''' フォルダを根こそぎ削除する（ReadOnlyでも削除）
    ''' </summary>
    ''' <param name="di">削除するフォルダ</param>
    ''' <param name="IsCurrentDelete">True指定したディレクトリ自体も削除</param>
    Public Overloads Shared Sub DeleteDirectory(ByVal di As IO.DirectoryInfo, Optional ByVal IsCurrentDelete As Boolean = True)
        'フォルダ以下のすべてのファイル、フォルダの属性を削除
        RemoveReadonlyAttribute(di)
        If IsCurrentDelete Then
            'フォルダを根こそぎ削除
            di.Delete(True)
        Else
            '自身以外のファイルを削除
            For Each subdir As DirectoryInfo In di.GetDirectories
                subdir.Delete(True)
            Next
            For Each f As FileInfo In di.GetFiles
                f.Delete()
            Next
        End If
    End Sub
    ''' <summary>
    ''' ファイルの属性の読み取り専用を外す
    ''' </summary>
    ''' <param name="dirInfo">処理対象のDirectoryInfo</param>
    ''' <remarks>再帰的にサブディレクトリにも変更を行う</remarks>
    Private Shared Sub RemoveReadonlyAttribute(ByVal dirInfo As System.IO.DirectoryInfo)
        '基のフォルダの属性を変更
        If (dirInfo.Attributes And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
            dirInfo.Attributes = System.IO.FileAttributes.Normal
        End If
        'フォルダ内のすべてのファイルの属性を変更
        Dim fi As System.IO.FileInfo
        For Each fi In dirInfo.GetFiles()
            If (fi.Attributes And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
                fi.Attributes = System.IO.FileAttributes.Normal
            End If
        Next
        'サブフォルダの属性を回帰的に変更
        Dim di As System.IO.DirectoryInfo
        For Each di In dirInfo.GetDirectories()
            RemoveReadonlyAttribute(di)
        Next
    End Sub
    ''' <summary>
    ''' ファイルを削除する（ReadOnlyでも削除）
    ''' </summary>
    ''' <param name="filePath">削除するファイルパス</param>
    Public Overloads Shared Sub DeleteFile(ByVal filePath As String)
        If Not IO.File.Exists(filePath) Then Exit Sub
        DeleteFile(New IO.FileInfo(filePath))
    End Sub
    ''' <summary>
    ''' ファイルを削除する（ReadOnlyでも削除）
    ''' </summary>
    ''' <param name="file">削除するファイル</param>
    Public Overloads Shared Sub DeleteFile(ByVal file As IO.FileInfo)
        If (file.Attributes And System.IO.FileAttributes.ReadOnly) = System.IO.FileAttributes.ReadOnly Then
            file.Attributes = System.IO.FileAttributes.Normal
        End If
        file.Delete()
    End Sub
#End Region

#Region "パスワードを作成する"
    ''' <summary>
    ''' パスワードを作成する
    ''' </summary>
    ''' <param name="length">作成されるパスワード長</param>
    ''' <returns>ランダムなパスワード</returns>
    ''' <remarks>
    ''' 既存処理は Credential_Helper.CreatePassword を呼び出していたが
    ''' 記号を使用したくない、という要件があり独自に作成する
    ''' また、デフォルト値として 6 桁にする
    ''' 使用可能文字列は [半角数字][半角アルファベット(大文字・小文字含む)]のみ
    ''' また、混同しやすい文字( 0 , 1 , o , i , l ...)は使用しない
    ''' </remarks>
    Public Shared Function CreatePassword(Optional ByVal length As Integer = 6) As String
        Dim alphaCaps As String = "QWERTYUPASDFGHJKZXCVBNM"
        Dim alphaLow As String = "qwertyupasdfghjkzxcvbnm"
        Dim numerics As String = "23456789"
        Dim allChars As String = alphaCaps & alphaLow & numerics

        Dim generatedPassword As New StringBuilder
        Dim rand As New System.Random()
        For i As Integer = 0 To length - 1
            Dim position As Integer = rand.Next(allChars.Length)
            generatedPassword.Append(allChars(position))
        Next
        Return generatedPassword.ToString
    End Function
#End Region

    Public Shared Function HtmlEncodeHtmlTag(str As String) As String
        If str Is Nothing Then Return String.Empty
        Dim encodedstring As String = HttpUtility.HtmlEncode(str).Replace("&lt;", "<").Replace("&gt;", ">").Replace(HttpUtility.HtmlEncode(""""), """").Replace("&amp;", "&").Replace(HttpUtility.HtmlEncode("'"), "'")
        Dim nonAsciiRegex As New Regex("&#\d+;|&;nbsp;")
        encodedstring = nonAsciiRegex.Replace(encodedstring, Function(match) HttpUtility.HtmlDecode(match.Value))

        Return encodedstring
    End Function
    Public Shared Function SanitizeInput(input As String) As String
        ' Remove any HTML tags and script elements using a regular expression
        Dim sanitizedInput As String = Regex.Replace(input, "<.*?>", String.Empty)

        ' Additional sanitization steps can be added here

        Return HtmlEncodeHtmlTag(sanitizedInput)
    End Function
    Public Shared Function SanitizeDataSet(ds As DataSet) As DataSet

        For Each dt As DataTable In ds.Tables
            dt = SanitizeDataTable(dt)
        Next
        Return ds
    End Function
    Public Shared Function SanitizeDataTable(dt As DataTable) As DataTable
        For Each row As DataRow In dt.Rows
            For Each col As DataColumn In dt.Columns
                row.Item(col) = SanitizeCellValue(row.Item(col))
            Next
        Next
        Return dt
    End Function
    Private Shared Function SanitizeCellValue(val As Object) As Object
        If TypeOf val Is String Then
            Return HtmlEncodeHtmlTag(DirectCast(val, String))
        End If
        Return val
    End Function

    Public Shared Function UploadFile(formFile As IFormFile, httpContext As HttpContext, pFilePath As String, pFileValidation As String(), ByRef fullFilePath As String, Optional ByVal fileRename As String = "") As Boolean
        Dim fileName As String = String.Empty
        Dim fileExt As String
        Try
            If (formFile IsNot Nothing AndAlso formFile.Length > 0) Then
                fileName = Path.GetFileName(HtmlEncodeHtmlTag(formFile.FileName))
                If Not String.IsNullOrEmpty(fileRename) Then
                    fileName = fileRename
                End If
                fullFilePath = Path.GetFullPath(Path.Combine(pFilePath, fileName))
                fileExt = Path.GetExtension(fileName)
                If (pFileValidation.Contains(fileExt, StringComparer.OrdinalIgnoreCase)) Then
                    Using stream As New FileStream(fullFilePath, FileMode.Create)
                        formFile.CopyTo(stream)
                    End Using
                    Return True
                End If
            End If
        Catch ex As Exception
            GlobalVariables.Logger.Error("Failed to upload file " & fileName & " to " & pFilePath, ex)
            Return False
        End Try
        Return False
    End Function

    Public Shared Function NObjToString(ByRef obj As Object) As String
        Dim str As String = String.Empty
        If TypeOf obj Is String Then
            str = Utilities.HtmlEncodeHtmlTag(obj.ToString)
        End If
        Return str
    End Function

    Public Shared Function ConvertSingleByteCharacters(ByVal s As String) As String
        Dim sb As StringBuilder = New StringBuilder
        For Each c As Char In s
            Dim cConverted As String = Encoding.Default.GetString(Encoding.Default.GetBytes(c))
            If cConverted = "?" Then
                sb.Append(c)
            Else
                sb.Append(cConverted)
            End If
        Next
        Return sb.ToString
    End Function

End Class

