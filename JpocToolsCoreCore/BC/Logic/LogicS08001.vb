Imports System.IO
Public Class LogicS08001
    Inherits AbstractLogic

#Region "プロパティ"
    Public ReadOnly Property Dto As DtoS08001
        Get
            Return CType(MyBase.mDto, DtoS08001)
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
    Public Sub CheckEntry()
        Try

        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        End Try
    End Sub
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
            '                         Me.Dto.WithoutPopupContents,
            '                         Me.Dto.OnlyRawImage,
            '                         Me.Dto.NeedBase64Encode,
            '                         rtnCD, _
            '                         rtnMsg)
            Select Case rtnCD
                Case 0
                    Using ds As New DS_DISEASE
                        'TODO: FIX HERE
                        'Utilities.DataSetDeserialize(data, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv, ds)
                        Me.Dto.DiseaseDataSet = CType(ds.Copy, DS_DISEASE)
                        If Me.Dto.NeedBase64Encode Then
                            For Each tbl As DataTable In Me.Dto.DiseaseDataSet.Tables
                                For Each dr As DataRow In tbl.Rows
                                    For Each col As DataColumn In tbl.Columns
                                        If col.DataType.Equals(Type.GetType("System.String")) Then
                                            If Not dr.IsNull(col.ColumnName) Then
                                                Dim s As String = Utilities.NZ(dr.Item(col.ColumnName))
                                                s = Utilities.ConvertBase64ToString(s)
                                                dr.Item(col.ColumnName) = s
                                            End If
                                        End If
                                    Next
                                Next
                            Next
                            Me.Dto.DiseaseDataSet.AcceptChanges()
                        End If
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

End Class
