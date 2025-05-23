Public Class WebServiceUserInfo
    Inherits Jpoc.Common.WebServiceUserInfo

#Region "インスタンス変数"
    Private Shared _WebServiceUserInfo As WebServiceUserInfo
#End Region

#Region "コンストラクタ"
    Private Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "シングルトンインスタンス取得"
    Public Shared Function GetInstance() As WebServiceUserInfo
        If _WebServiceUserInfo Is Nothing Then
            _WebServiceUserInfo = New WebServiceUserInfo
        End If
        Return _WebServiceUserInfo
    End Function
#End Region

#Region "初期化"
    Public Overrides Sub Init()
        MyBase.UserID = String.Empty
        MyBase.InstitutionCode = String.Empty
        MyBase.Password = String.Empty
        MyBase.ConnectWebServiceName = String.Empty
        MyBase.Authorized = False
    End Sub
#End Region

#Region "認証"
    Public Function Authorize() As Boolean
        Dim sc As JpocWeb.EditorService.EditorServiceSoapClient = Nothing
        Try
            sc = New JpocWeb.EditorService.EditorServiceSoapClient
            '環境に応じたURLを設定する
            sc.Endpoint.Address = GlobalVariables.WebServiceUrlEndpointAddress(MyBase.ConnectWebServiceName)
            Dim rtnCD As Integer = 0
            Dim rtnMsg As String = String.Empty
            'Dim soapReq As SoapRequest = New SoapRequest
            'Dim soapParam As List(Of SoapRequestParam) = New List(Of SoapRequestParam)
            'soapParam.Add(New SoapRequestParam("pUserID", Me.EncryptedUserID))
            'soapParam.Add(New SoapRequestParam("pInstitutionCode", Me.EncryptedInstitutionCode))
            'soapParam.Add(New SoapRequestParam("pPassword", Me.EncryptedPassword))
            'soapParam.Add(New SoapRequestParam("RtnCD", rtnCD.ToString))
            'soapParam.Add(New SoapRequestParam("RtnMsg", rtnMsg))
            'Dim SoapXml As String = soapReq.GetSoapXml("Authorize", soapParam)
            'Dim soapResult As String = soapReq.Execute(GlobalVariables.WebServiceUrlEndpointAddress(MyBase.ConnectWebServiceName).ToString(), SoapXml)
            ''Dim AuthorizedResult As String = 
            'Dim SoapAuthoried As Boolean = False
            'Boolean.TryParse(soapReq.ReadResult(soapResult, "Authorize", "AuthorizeResult"), SoapAuthoried)
            'Integer.TryParse(soapReq.ReadResult(soapResult, "Authorize", "rtnCD"), rtnCD)
            'rtnMsg = soapReq.ReadResult(soapResult, "Authorize", "rtnMsg")
            'MyBase.Authorized = SoapAuthoried

            'TODO: FIX HERE
            'MyBase.Authorized = sc.Authorize(Me.EncryptedUserID, Me.EncryptedInstitutionCode, Me.EncryptedPassword, rtnCD, rtnMsg)
            Select Case rtnCD
                Case 0, 8

                Case Else
                    '3DESで暗号化されたメッセージを復号する

                    'TODO: FIX HERE
                    'rtnMsg = Utilities.Decrypt(rtnMsg, GlobalVariables.TripleDesKey, GlobalVariables.TripleDesIv)
                    Throw New Exception(rtnMsg)
            End Select
            Return MyBase.Authorized
        Catch ex As ElsException
            ex.AddStackFrame(New StackFrame(True))
            Throw
        Catch ex As Exception
            Throw New ElsException(ex)
        Finally
            sc.Close()
            sc = Nothing
        End Try
    End Function
#End Region

End Class
