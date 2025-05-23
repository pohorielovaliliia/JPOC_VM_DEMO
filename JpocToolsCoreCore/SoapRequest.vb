Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Xml
Imports System.Xml.Linq

Public Class SoapRequest

    Public Function Execute(ByVal SoapServiceUrl As String, ByVal SoapXML As String) As String

        Dim soapResult As String = String.Empty
        Dim request As HttpWebRequest = CreateWebRequest(SoapServiceUrl)
        Dim soapEnvelopeXml As XmlDocument = New XmlDocument()
        'soapEnvelopeXml.LoadXml("<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">
        '        <soap12:Body>
        '            <Authorize xmlns=""http://clinicalsup.jp/"">
        '                <pUserID>vtelsadmin</pUserID>
        '                    <pInstitutionCode>999</pInstitutionCode>
        '                    <pPassword>vtelsadmin</pPassword>
        '                    <RtnCD>0</RtnCD>
        '                    <RtnMsg></RtnMsg>
        '            </Authorize>
        '        </soap12:Body>
        '    </soap12:Envelope>")
        soapEnvelopeXml.LoadXml(SoapXML)

        Using stream As Stream = request.GetRequestStream()
            soapEnvelopeXml.Save(stream)
        End Using

        Using response As WebResponse = request.GetResponse()
            Using rd As New StreamReader(response.GetResponseStream())
                soapResult = rd.ReadToEnd()
            End Using
        End Using
        Return soapResult
    End Function

    'TODO: FIX HERE
    Private Function CreateWebRequest(ByVal SoapServiceUrl As String) As HttpWebRequest
        'Dim _webReq As HttpWebRequest = CType(WebRequest.Create(SoapServiceUrl), HttpWebRequest)

        '_webReq.ContentType = "text/xml;charset=""utf-8"""
        '_webReq.Accept = "text/xml"
        '_webReq.Method = "POST"

        'Return _webReq
        Return Nothing
    End Function

    Public Function GetSoapXml(ByVal SoapMethod As String, ByVal SoapParam As List(Of SoapRequestParam)) As String
        Dim sb As StringBuilder = New StringBuilder
        sb.AppendLine("<soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">")
        sb.AppendLine("<soap12:Body>")
        sb.AppendLine(String.Format("<{0} xmlns=""http://clinicalsup.jp/"">", SoapMethod))
        For Each param As SoapRequestParam In SoapParam

            sb.AppendLine(String.Format("<{0}>{1}</{0}>", param.Key, param.Value))
        Next
        sb.AppendLine(String.Format("</{0}>", SoapMethod))
        sb.AppendLine("</soap12:Body>")
        sb.AppendLine("</soap12:Envelope>")
        Return sb.ToString
    End Function

    Public Function ReadResult(ByVal SoapResult As String, ByVal SoapMethod As String, ByVal Key As String) As String

        Dim Result As String = ""
        Dim doc As XDocument = XDocument.Parse(SoapResult)
        Dim ns As XNamespace = "http://clinicalsup.jp/"
        Dim responses As IEnumerable(Of XElement) = doc.Descendants(ns + (SoapMethod & "Response"))
        For Each response As XElement In responses
            Result = CType(response.Element(ns + Key), String)
        Next
        Return Result
    End Function
End Class
Public Class SoapRequestParam
    Public Property Key As String
    Public Property Value As String
    Public Sub New(ByVal _key As String, ByVal _val As String)

        Key = _key
        Value = _val
    End Sub

End Class