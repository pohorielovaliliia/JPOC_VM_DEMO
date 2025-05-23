Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlDbType

Public Class ElsDataBase
    Implements IDisposable

#Region "定数"
    Private Const TAB_CHAR As String = ControlChars.Tab
#End Region

#Region "インスタンス変数"
    Private _ConnectionString As String
    Private _Connection As SqlConnection
    Private _Transaction As SqlTransaction
    Private _Command As SqlCommand
    Private _ResultMessage As String
#End Region

#Region "プロパティ"
    Public ReadOnly Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
    End Property
    Public ReadOnly Property State() As System.Data.ConnectionState
        Get
            If _Connection IsNot Nothing Then
                Return _Connection.State
            Else
                Return ConnectionState.Closed
            End If
        End Get
    End Property
    Public ReadOnly Property ResultMessage() As String
        Get
            Return _ResultMessage
        End Get
    End Property
    Public ReadOnly Property Command() As String
        Get
            Dim strTemp As String = String.Empty
            If _Command IsNot Nothing Then
                strTemp = Utilities.NZ(_Command.CommandText)
            End If
            Return strTemp
        End Get
    End Property
    Public ReadOnly Property HasTransaction As Boolean
        Get
            Return _Transaction IsNot Nothing
        End Get
    End Property
    Public Property AutoCommit As Boolean
    ''' <summary>
    ''' SQLパラメータを変換したSQLクエリ
    ''' </summary>
    ''' <value></value>
    ''' <returns>実際にDBに投げられた（と想定している）Query文字列</returns>
    ''' <remarks>セキュリティ的には良くない為、デバッグでの使用を推奨</remarks>
    Public ReadOnly Property RawSqlCommand As String
        Get
            Dim executeSql As String = Command
            For Each param As SqlParameter In _Command.Parameters
                If param.Direction = ParameterDirection.Input OrElse
                   param.Direction = ParameterDirection.InputOutput Then

                    Dim paramString As String = String.Empty

                    '値の編集
                    Select Case param.DbType
                        Case DbType.String, DbType.StringFixedLength, DbType.AnsiString, DbType.AnsiStringFixedLength, _
                             DbType.Binary, DbType.Guid
                            '文字列は前後に[']付与、ただし、文中に[']が存在する場合は、エスケープを先に
                            paramString = "'" & Utilities.NZ(param.Value).Replace("'", "''") & "'"
                        Case DbType.Byte, DbType.Int16, DbType.Int32, DbType.Int64, _
                             DbType.UInt16, DbType.UInt32, DbType.UInt64, _
                             DbType.SByte, DbType.Currency, DbType.Decimal, DbType.Double, DbType.VarNumeric, _
                             DbType.Single
                            '数値はそのまま
                            paramString = Utilities.NZ(param.Value)
                        Case DbType.Boolean
                            'Boolean(0 or 1)は数値変換
                            If Utilities.NZ(param.Value) = "0" OrElse
                               Utilities.NZ(param.Value) = "1" Then
                                paramString = Utilities.NZ(param.Value)
                            Else
                                Dim IsParam As Boolean = Boolean.TryParse(Utilities.NZ(param.Value), IsParam)
                                If IsParam Then
                                    paramString = "1"
                                Else
                                    paramString = "0"
                                End If
                            End If
                        Case DbType.Date, DbType.DateTime, DbType.DateTime2, DbType.DateTimeOffset, DbType.Time
                            '時間は文字列と同じ想定
                            paramString = "'" & Utilities.NZ(param.Value).Replace("'", "''") & "'"
                        Case DbType.Object, DbType.Xml
                            'その他の処理は、文字列と同等にしておく
                            paramString = "'" & Utilities.NZ(param.Value).Replace("'", "''") & "'"
                        Case Else
                            'それ以外のケース（未設定）は文字列と同等にしておく
                            paramString = "'" & Utilities.NZ(param.Value).Replace("'", "''") & "'"
                    End Select

                    'パラメータが取れなかった場合は、回避しておく
                    If String.IsNullOrEmpty(paramString) Then Continue For
                    executeSql = System.Text.RegularExpressions.Regex.Replace(executeSql, param.ParameterName, paramString)
                End If
            Next
            Return executeSql
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Private Sub New()
        MyBase.New()
        Me._AutoCommit = False
    End Sub
    '''-----------------------------------------------------------------------------------------------------------------
    '''<summary>
    '''コンストラクタ
    '''</summary>
    '''<remarks>
    '''定義ファイルから接続文字列を読み込み、DB接続を確立する。
    '''</remarks>
    '''<peram>
    '''</peram>
    '''<history>
    '''
    '''</history>
    '''-----------------------------------------------------------------------------------------------------------------
    Public Sub New(ByVal pConStr As String)
        Me.New()
        Try
            _ResultMessage = String.Empty
            If Not String.IsNullOrEmpty(pConStr) Then
                Call CreateConnection(pConStr)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "接続文字列を変更"
    Public Sub ResetConStr(ByVal pConStr As String)
        Call CreateConnection(pConStr)
    End Sub
#End Region

#Region "コネクションを作成する"
    Private Sub CreateConnection(ByVal pConStr As String)
        Try
            If _Connection IsNot Nothing Then
                Call DisConnect()
                _Connection.Dispose()
                _Connection = Nothing
            End If

            'If String.IsNullOrEmpty(_ConnectionString) Then
            '_ConnectionString = pConStr
            'End If
            If Not String.IsNullOrEmpty(pConStr) Then
                _ConnectionString = pConStr
            End If

            'コネクションを作成する
            _Connection = New SqlConnection(_ConnectionString)

            SqlConnection.ClearPool(_Connection)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "コマンドオブジェクトを作成する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドオブジェクトを作成する
    ''' </summary>
    ''' <param name="sqlCmdText"></param>
    ''' <param name="cmdType"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub SetSqlCommand(ByVal sqlCmdText As String, ByVal cmdType As CommandType)

        Try
            _Command = New SqlCommand()
            With _Command
                .Connection = _Connection
                .CommandType = cmdType
                .CommandText = sqlCmdText
                .CommandTimeout = 0
                '.NotificationAutoEnlist = True TODO: FIX HERE
                .Transaction = Me._Transaction
                .Prepare()
            End With

        Catch ex As Exception
            _ResultMessage = ex.Message
            _Command = Nothing
            Throw ex
        End Try
    End Sub
#End Region

#Region "コマンドオブジェクトのパラメータズコレクションにパラメータオブジェクトを追加する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドオブジェクトのパラメータズコレクションにパラメータオブジェクトを追加する
    ''' </summary>
    ''' <param name="pName"></param>
    ''' <param name="value"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub SetCmdParameter(ByVal pName As String, _
                                           Optional ByVal value As Object = Nothing)
        Try
            Dim val As Object = DBNull.Value
            If value IsNot Nothing Then val = value
            Dim param As New SqlParameter(pName, value)
            _Command.Parameters.Add(param)
        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Sub
#End Region

#Region "コマンドオブジェクトのパラメータズコレクションにパラメータオブジェクトを追加する （オーバーロード）"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドオブジェクトのパラメータズコレクションにパラメータオブジェクトを追加する
    ''' </summary>
    ''' <param name="pName"></param>
    ''' <param name="pType"></param>
    ''' <param name="pDirection"></param>
    ''' <param name="value"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub SetCmdParameter(ByVal pName As String, _
                                           ByVal pType As SqlDbType, _
                                           ByVal pDirection As ParameterDirection, _
                                           Optional ByVal value As Object = Nothing)
        Try
            Dim param As New SqlParameter(pName, pType)
            param.Direction = pDirection
            If value IsNot Nothing Then
                Select Case pType
                    Case SqlDbType.Int
                        Dim vInt As Integer = 0
                        Integer.TryParse(value.ToString, vInt)
                        param.Value = vInt
                    Case SqlDbType.Float
                        Dim vDecimal As Decimal = 0
                        Decimal.TryParse(value.ToString, vDecimal)
                        param.Value = vDecimal
                    Case SqlDbType.Date
                        Dim vDate As Date = Date.MinValue
                        Date.TryParse(value.ToString, vDate)
                        param.Value = vDate
                    Case SqlDbType.DateTime
                        Dim vDate As DateTime = System.DateTime.MinValue
                        System.DateTime.TryParse(value.ToString, vDate)
                        param.Value = vDate
                    Case SqlDbType.Bit
                        Dim vBoolean As Boolean = False
                        Boolean.TryParse(value.ToString, vBoolean)
                        param.Value = vBoolean
                    Case Else
                        param.Value = Utilities.HtmlEncodeHtmlTag(value.ToString)
                End Select
            End If
            _Command.Parameters.Add(param)
        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Sub
#End Region

#Region "コマンドオブジェクトのパラメータズコレクションにパラメータオブジェクトを追加する （オーバーロード）"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドオブジェクトのパラメータズコレクションにパラメータオブジェクトを追加する
    ''' </summary>
    ''' <param name="pName"></param>
    ''' <param name="pType"></param>
    ''' <param name="pSize"></param>
    ''' <param name="pDirection"></param>
    ''' <param name="value"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub SetCmdParameter(ByVal pName As String, _
                                           ByVal pType As SqlDbType, _
                                           ByVal pSize As Integer, _
                                           ByVal pDirection As ParameterDirection, _
                                           Optional ByVal value As Object = Nothing)
        Try
            Dim param As New SqlParameter(pName, pType)
            param.Direction = pDirection
            If value IsNot Nothing Then
                param.Size = pSize
                param.Value = value
            End If
            _Command.Parameters.Add(param)
        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Sub
#End Region

#Region "コマンドオブジェクトのパラメータズコレクションを取得する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドオブジェクトのパラメータズコレクションを取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function GetCmdParameter() As SqlParameterCollection
        Try
            Return _Command.Parameters
        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Function
#End Region

#Region "コマンドオブジェクトからパラメータズコレクションをクリアする"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドオブジェクトからパラメータズコレクションをクリアする
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub ClearCmdParameter()
        Try
            If _Command IsNot Nothing Then
                _Command.Parameters.Clear()
            End If
        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Sub
#End Region

#Region "トランザクションの開始を指定する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' トランザクションの開始を指定する
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub BeginTransaction()
        Try
            _Transaction = _Connection.BeginTransaction()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "トランザクションのコミットを指定する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' トランザクションのコミットを指定する
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub CommitTransaction()
        Try
            If _Transaction IsNot Nothing Then
                _Transaction.Commit()
                _Transaction.Dispose()
            End If
            _Transaction = Nothing

        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Sub
#End Region

#Region "トランザクションのロールバックを指定する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' トランザクションのロールバックを指定する
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub RollbackTransaction()
        Try

            If _Transaction IsNot Nothing Then
                _Transaction.Rollback()
                _Transaction.Dispose()
            End If
            _Transaction = Nothing

        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Sub
#End Region

#Region "コマンドを実行しデータセットを取得する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドを実行しデータセットを取得する
    ''' </summary>
    ''' <param name="pDataSet"></param>
    ''' <param name="pTableName"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function ExecuteAndGetDataSet(ByRef pDataSet As DataSet, _
                                                     ByVal pTableName As String, _
                                                     Optional pClear As Boolean = True) As Integer

        Dim ret As Integer = -1
        Try
            _ResultMessage = String.Empty

            Using adapter As New SqlDataAdapter(_Command)
                If pTableName Is Nothing Then
                    ret = adapter.Fill(pDataSet)
                Else
                    If pDataSet.Tables.Contains(pTableName) Then
                        If pClear Then
                            pDataSet.Tables(pTableName).Rows.Clear()
                        End If
                    End If
                    ret = adapter.Fill(pDataSet, pTableName)
                End If
            End Using

            pDataSet.AcceptChanges()
        Catch ex As Exception
            _ResultMessage = ex.Message & vbCrLf & "Command : " & _Command.CommandText
            Throw ex
        End Try
        Return ret
    End Function
#End Region

#Region "コマンドを実行しデータテーブルを取得する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドを実行しデータセットを取得する
    ''' </summary>
    ''' <param name="pDataTable"></param>
    ''' <param name="pClear"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function ExecuteAndGetDataTable(ByRef pDataTable As DataTable, _
                                                       Optional pClear As Boolean = True) As Integer

        Dim ret As Integer = -1
        Try
            _ResultMessage = String.Empty

            Using adapter As New SqlDataAdapter(_Command)
                If pClear Then
                    pDataTable.Rows.Clear()
                End If
                ret = adapter.Fill(pDataTable)
            End Using

            pDataTable.AcceptChanges()
        Catch ex As Exception
            _ResultMessage = ex.Message & vbCrLf & "Command : " & _Command.CommandText
            Throw ex
        End Try
        Return ret
    End Function
#End Region

#Region "コマンドを実行する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドを実行する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function Execute() As Integer
        Dim ret As Integer = 0
        Try
            _ResultMessage = String.Empty
            ret = _Command.ExecuteNonQuery
        Catch ex As Exception
            _ResultMessage = ex.Message & vbCrLf & "Command : " & _Command.CommandText
            Throw ex
        End Try
        Return ret
    End Function
#End Region

#Region "コマンドを実行する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' コマンドを実行する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Function ExecuteScalar() As String
        Dim ret As String = String.Empty
        Try
            _ResultMessage = String.Empty
            ret = Utilities.NZ(_Command.ExecuteScalar)
        Catch ex As Exception
            _ResultMessage = ex.Message & vbCrLf & "Command : " & _Command.CommandText
            Throw ex
        End Try
        Return ret
    End Function

    Public Function JPoCExecute(ByVal query As String,
                                       ByRef parameters As List(Of SqlParameter),
                                       Optional ByVal pCommandType As CommandType = CommandType.StoredProcedure) As DataSet
        Dim ds As New DataSet

        Using conn As SqlConnection = New SqlConnection(_ConnectionString)
            Try
                conn.Open()
                Using comm As New SqlCommand(query, conn)
                    With comm
                        .CommandType = pCommandType
                        .CommandTimeout = 600
                    End With
                    If parameters IsNot Nothing Then
                        For Each parameter As SqlParameter In parameters
                            comm.Parameters.Add(parameter)
                        Next
                    End If
                    Using da As New SqlDataAdapter
                        da.SelectCommand = comm
                        da.Fill(ds)
                    End Using
                End Using
            Catch ex As Exception
                GlobalVariables.Logger.Debug(ex.Message, ex)
                Throw ex
            Finally
                If conn.State = ConnectionState.Open Then conn.Close()
                conn.Dispose()
            End Try
        End Using
        Return ds
    End Function

    Public Function JPoCScalar(ByVal query As String,
                                      ByRef parameters As List(Of SqlParameter),
                                      Optional ByVal pCommandType As CommandType = CommandType.StoredProcedure) As String
        Dim result As String = String.Empty
        Using conn As SqlConnection = New SqlConnection(_ConnectionString)
            Try
                conn.Open()
                Using comm As New SqlCommand(query, conn)
                    With comm
                        .CommandType = pCommandType
                        .CommandTimeout = 600
                    End With
                    If parameters IsNot Nothing Then
                        For Each parameter As SqlParameter In parameters
                            comm.Parameters.Add(parameter)
                        Next
                    End If
                    result = Utilities.NZ(comm.ExecuteScalar())
                End Using
            Catch ex As Exception
                GlobalVariables.Logger.Debug(ex.Message, ex)
                Throw ex
            Finally
                If conn.State = ConnectionState.Open Then conn.Close()
                conn.Dispose()
            End Try
        End Using
        Return result
    End Function
#End Region

#Region "DBMSへ接続する"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBMSへ接続する
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' 	[SHONDA]	2008/06/03	Modified
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub Connect()
        Dim Counter As Integer = 1
        Dim ConnectionCounter As Integer = 1
        Try
            If _Connection.State = ConnectionState.Closed Then
                ConnectionCounter = 1
                Do Until _Connection.State = ConnectionState.Open
                    Try
                        Counter = 1
                        Do Until _Connection.State = ConnectionState.Open   'Openできるか10回トライしてNGになるまで繰り返す
                            Try
                                _Connection.Open()
                            Catch ex As TimeoutException
                                If Counter > 5 Then '5回リトライしてもダメならあきらめる
                                    Throw ex
                                End If
                                System.Threading.Thread.Sleep(1500)    '1.5秒ウエイト
                                Counter += 1
                            Catch ex As Exception
                                Throw ex
                            End Try
                        Loop
                    Catch ex As TimeoutException
                        If ConnectionCounter > 5 Then   'Connectionを再作成後2回リトライしてもダメならあきらめる
                            Throw ex
                        End If
                        System.Threading.Thread.Sleep(3000)    '3秒ウエイト
                        Try
                            SqlConnection.ClearPool(_Connection)
                        Catch ex1 As Exception
                            Err.Clear()
                        End Try
                        ConnectionCounter += 1
                    Catch ex As Exception
                        Throw ex
                    End Try
                Loop
            End If

        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
    End Sub
#End Region

#Region "DBMSへの接続を閉じる(プール使用時はプールへの返却)"
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' DBMSへの接続を閉じる(プール使用時はプールへの返却)
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[SHONDA]	2006/08/24	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub DisConnect()
        Try
            If _Connection Is Nothing Then Exit Sub
            If _Connection.State = ConnectionState.Open Then
                If Me.HasTransaction Then
                    If Me.AutoCommit Then
                        Me.CommitTransaction()
                    Else
                        Me.RollbackTransaction()
                    End If
                End If
                'クローズする
                _Connection.Close()
            End If
        Catch ex As Exception
            _ResultMessage = ex.Message
            Throw ex
        End Try
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
