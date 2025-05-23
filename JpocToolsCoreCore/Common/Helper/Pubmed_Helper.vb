Public Class PubmedArticle

#Region "インスタンス変数"
    Private _Pmid As String
    Private _Title As String
    Private _Authors As String
    Private _Citation As String
    Private _PublishDate As Nullable(Of DateTime)
    Private _AbstractText As String
    Private _FullTextLinks As String
#End Region

#Region "プロパティ"
    Public ReadOnly Property Pmid As String
        Get
            Return Me._Pmid
        End Get
    End Property
    Public ReadOnly Property Title As String
        Get
            Return Me._Title
        End Get
    End Property
    Public ReadOnly Property Authors As String
        Get
            Return Me._Authors
        End Get
    End Property
    Public ReadOnly Property Citation As String
        Get
            Return Me._Citation
        End Get
    End Property
    Public ReadOnly Property PublishDate As Nullable(Of DateTime)
        Get
            Return Me._PublishDate
        End Get
    End Property
    Public ReadOnly Property AbstractText As String
        Get
            Return Me._AbstractText
        End Get
    End Property
    Public ReadOnly Property FullTextLinks As String
        Get
            Return Me._FullTextLinks
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    Private Sub New()
        Me._Pmid = String.Empty
        Me._Title = String.Empty
        Me._Authors = String.Empty
        Me._Citation = String.Empty
        Me._PublishDate = Nothing
        Me._AbstractText = String.Empty
        Me._FullTextLinks = String.Empty
    End Sub
    Public Sub New(ByVal pPmid As String, _
                   ByVal pTitle As String, _
                   ByVal pAuthors As String, _
                   ByVal pCitation As String, _
                   ByVal pPublishDate As Nullable(Of DateTime), _
                   ByVal pAbstractText As String, _
                   ByVal pFullTextLinks As String)
        Me._Pmid = pPmid
        Me._Title = pTitle
        Me._Authors = pAuthors
        Me._Citation = pCitation
        Me._PublishDate = pPublishDate
        Me._AbstractText = pAbstractText
        Me._FullTextLinks = pFullTextLinks
    End Sub
#End Region

End Class

Public Class Pubmed_Helper

#Region "コンストラクタ"
    Public Sub New()

    End Sub
#End Region

#Region "PMIDによるArticleの取得"
    Public Function GetArticlesByPmids(ByVal pIDs As String()) As List(Of PubmedArticle)

        Dim req As New NCBI.efetch_pubmed.eFetchRequest()
        req.id = pIDs(0)
        Dim res As New NCBI.efetch_pubmed.eFetchResult
        Using serv As New NCBI.efetch_pubmed.eUtilsServiceSoapClient
            Try
                res = serv.run_eFetch(req)
            Finally
                serv.Close()
            End Try
        End Using

        Dim citations As Dictionary(Of String, String) = GetCitationsByPmids(pIDs)
        Dim full_text_links As Dictionary(Of String, String) = GetFullTextLinksByPmids(pIDs)

        Dim articles As New List(Of PubmedArticle)
        For i As Integer = 0 To res.PubmedArticleSet.Length - 1
            If TypeOf res.PubmedArticleSet(i) Is NCBI.efetch_pubmed.PubmedArticleType Then
                Dim art As NCBI.efetch_pubmed.PubmedArticleType = DirectCast(res.PubmedArticleSet(i), NCBI.efetch_pubmed.PubmedArticleType)
                If art IsNot Nothing Then
                    Dim pmid As String = art.MedlineCitation.PMID.Value
                    Dim title As String = art.MedlineCitation.Article.ArticleTitle.Value

                    Dim authors As String = String.Empty
                    If art.MedlineCitation.Article.AuthorList IsNot Nothing Then
                        For j As Integer = 0 To art.MedlineCitation.Article.AuthorList.Author.Length - 1
                            If art.MedlineCitation.Article.AuthorList.Author(j).Items.Length >= 2 Then
                                authors &= art.MedlineCitation.Article.AuthorList.Author(j).Items(1) & " " & art.MedlineCitation.Article.AuthorList.Author(j).Items(0)
                            Else
                                authors &= art.MedlineCitation.Article.AuthorList.Author(j).Items(0)
                            End If
                            If j < art.MedlineCitation.Article.AuthorList.Author.Length - 1 Then
                                authors &= ", "
                            End If
                        Next
                    End If

                    Dim citation As String = citations(pmid)

                    Dim abstract_text As String = String.Empty
                    If art.MedlineCitation.Article.Abstract IsNot Nothing Then
                        For j As Integer = 0 To art.MedlineCitation.Article.Abstract.AbstractText.Length - 1
                            If Not String.IsNullOrEmpty(art.MedlineCitation.Article.Abstract.AbstractText(j).Label) Then
                                abstract_text += "<strong>" + art.MedlineCitation.Article.Abstract.AbstractText(j).Label & ": </strong>"
                            End If
                            abstract_text += art.MedlineCitation.Article.Abstract.AbstractText(j).Value + "<br />"
                        Next
                        If Not String.IsNullOrEmpty(art.MedlineCitation.Article.Abstract.CopyrightInformation) Then
                            abstract_text += "<br />" + art.MedlineCitation.Article.Abstract.CopyrightInformation
                        End If
                    End If

                    Dim publish_date As New DateTime(0)
                    If art.MedlineCitation.Article.ArticleDate IsNot Nothing AndAlso art.MedlineCitation.Article.ArticleDate.Length > 0 Then
                        Dim article_date As NCBI.efetch_pubmed.ArticleDateType = art.MedlineCitation.Article.ArticleDate(0)
                        publish_date = New DateTime(Convert.ToInt32(article_date.Year), Convert.ToInt32(article_date.Month), Convert.ToInt32(article_date.Day))
                    End If

                    articles.Add(New PubmedArticle(pmid, title, authors, citation, publish_date, abstract_text, full_text_links(pmid)))
                End If
            End If
        Next
        Return articles
    End Function
#End Region

#Region "PMIDによるCitationの取得"
    Private Shared Function GetCitationsByPmids(ByVal pIDs As String()) As Dictionary(Of String, String)

        Dim req As New NCBI.eutils.eSummaryRequest()
        req.db = "pubmed"
        req.id = String.Join(",", pIDs)
        Dim res As New NCBI.eutils.eSummaryResult()
        Using serv As New NCBI.eutils.eUtilsServiceSoapClient()
            Try
                res = serv.run_eSummary(req)
            Finally
                serv.Close()
            End Try
        End Using

        Dim citations As New Dictionary(Of String, String)()
        For i As Integer = 0 To res.DocSum.Length - 1
            Dim pmid As String = res.DocSum(i).Id
            Dim source As String = String.Empty
            Dim epubdate As String = String.Empty
            Dim doi As String = String.Empty
            Dim so As String = String.Empty
            For j As Integer = 0 To res.DocSum(i).Item.Length - 1
                Select Case res.DocSum(i).Item(j).Name
                    Case "Source"
                        source = res.DocSum(i).Item(j).ItemContent
                    Case "EPubDate"
                        epubdate = res.DocSum(i).Item(j).ItemContent
                    Case "DOI"
                        doi = res.DocSum(i).Item(j).ItemContent
                    Case "SO"
                        so = res.DocSum(i).Item(j).ItemContent
                End Select
            Next
            Dim citation As String = String.Empty
            If Not String.IsNullOrEmpty(source) Then citation &= source & ". "
            If Not String.IsNullOrEmpty(so) Then citation &= so & ". "
            If Not String.IsNullOrEmpty(doi) Then citation &= "doi: " & doi & ". "
            If Not String.IsNullOrEmpty(epubdate) Then citation &= "Epub " & epubdate & ". "
            If Not citations.ContainsKey(pmid) Then citations.Add(pmid, citation)
        Next
        Return citations
    End Function
#End Region

#Region "PMIDによるFullTextLinkの取得"
    Public Shared Function GetFullTextLinksByPmids(ByVal pIDs As String()) As Dictionary(Of String, String)

        Dim req As New NCBI.eutils.eLinkRequest()
        req.id = pIDs
        req.dbfrom = "pubmed"
        req.cmd = "llinks"
        Dim res As New NCBI.eutils.eLinkResult()
        Using serv As New NCBI.eutils.eUtilsServiceSoapClient()
            Try
                res = serv.run_eLink(req)
            Finally
                serv.Close()
            End Try
        End Using

        Dim full_text_links As New Dictionary(Of String, String)()
        For i As Integer = 0 To res.LinkSet.Length - 1
            For j As Integer = 0 To res.LinkSet(i).IdUrlList.Items.Length - 1
                Dim urlSet As NCBI.eutils.IdUrlSetType = DirectCast(res.LinkSet(i).IdUrlList.Items(j), NCBI.eutils.IdUrlSetType)
                Dim pmid As String = urlSet.Id.Value
                Dim links As String = String.Empty
                For k As Integer = 0 To urlSet.Items.Length - 1
                    If TypeOf urlSet.Items(k) Is NCBI.eutils.ObjUrlType Then
                        Dim urlType As NCBI.eutils.ObjUrlType = DirectCast(urlSet.Items(k), NCBI.eutils.ObjUrlType)
                        If urlType.Category.Contains("Full Text Sources") Then
                            links &= String.Format("<a href='{0}'>{1}</a>", urlType.Url.Value, urlType.Provider.Name) & "<br />"
                        End If
                    End If
                Next
                If Not full_text_links.ContainsKey(pmid) Then full_text_links.Add(pmid, links)
            Next
        Next
        Return full_text_links
    End Function
#End Region

End Class
