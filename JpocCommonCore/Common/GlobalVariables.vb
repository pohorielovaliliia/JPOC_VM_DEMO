Imports System.Globalization
Imports System.Data
Imports System.IO
Imports log4net

''' <summary>
''' グローバル変数
''' </summary>
Public Class GlobalVariables

#Region "定数"
    Public Const JA_JP As String = "ja-JP"

    Public Class STATUS
        Public Const ACTIVE As String = "A"
        Public Const INACTIVE As String = "I"
        Public Const UNUSED As String = "U"
    End Class

    '診療科選択肢Item
    Public Shared ReadOnly Property DiseaseCategoryItemList As DataTable
        Get
            Dim strDisplayMemberArray() As String = {"＜選択してください＞", "内科", "消化器内科", "循環器内科", "呼吸器内科", "内分泌、代謝内科", "腎臓内科", "血液内科", "感染症内科", "精神科", "神経内科", "心療内科", "外科", "脳神経外科", "消化器外科", "心臓血管外科", "呼吸器外科", "乳腺外科", "整形外科", "形成外科・美容外科", "小児科・小児外科", "産婦人科（産科・婦人科）", "眼科", "耳鼻いんこう科", "皮膚科", "泌尿器科", "アレルギー、リウマチ科", "麻酔科", "放射線科", "救急科", "リハビリテーション科"}
            Dim strValueMemberArray() As String = {String.Empty, "内科", "消化器内科", "循環器内科", "呼吸器内科", "内分泌、代謝内科", "腎臓内科", "血液内科", "感染症内科", "精神科", "神経内科", "心療内科", "外科", "脳神経外科", "消化器外科", "心臓血管外科", "呼吸器外科", "乳腺外科", "整形外科", "形成外科・美容外科", "小児科・小児外科", "産婦人科（産科・婦人科）", "眼科", "耳鼻いんこう科", "皮膚科", "泌尿器科", "アレルギー、リウマチ科", "麻酔科", "放射線科", "救急科", "リハビリテーション科"}
            Return Utilities.ConvertArrayToTableForComboBox(strDisplayMemberArray, strValueMemberArray, , True)
        End Get
    End Property

    'エルゼビア管理者用権限一覧
    Public Shared ReadOnly Property ElsevierManagerRoleList As DataTable
        Get
            Dim strDisplayMemberArray() As String = {"エルゼビア管理者",
                                                     "管理者",
                                                     "利用者",
                                                     "編集者",
                                                     "営業",
                                                     "ＴＳ",
                                                     "利用者Ｓ",
                                                        "Entitlement Audit",
                                                        "Entitlement Support",
                                                        "JP Support",
                                                        "Elsevier Marketing",
                                                        "Individual Management",
                                                        "Support Admin"}
            Dim strValueMemberArray() As String = {"1",
                                                   "3",
                                                   "5",
                                                   "8",
                                                   "13",
                                                   "14",
                                                   "19",
                                                   "23",
                                                   "24",
                                                   "25",
                                                   "26",
                                                   "27",
                                                   "28"}
            Return Utilities.ConvertArrayToTableForComboBox(strDisplayMemberArray, strValueMemberArray, , True)
        End Get
    End Property

    'エルゼビア管理者用個人権限一覧
    Public Shared ReadOnly Property ElsevierManagerUserRoleList As DataTable
        Get
            Dim strDisplayMemberArray() As String = {"契約ユーザー",
                                                     "代理店ユーザー",
                                                     "トライアルユーザー",
                                                     "DVDユーザー",
                                                     "利用者Ｓ",
                                                     "RegistrationID",
                                                     "PINコード契約ユーザー",
                                                     "M3 RegistrationID for Trial",
                                                     "m3トライアルユーザー"}
            Dim strValueMemberArray() As String = {"6",
                                                   "16",
                                                   "7",
                                                   "15",
                                                   "19",
                                                   "21",
                                                   "22",
                                                   "30",
                                                   "31"}
            Return Utilities.ConvertArrayToTableForComboBox(strDisplayMemberArray, strValueMemberArray, , True)
        End Get
    End Property

    '施設管理者用権限一覧
    Public Shared ReadOnly Property InstitutionAdminRoleList As DataTable
        Get
            Dim strDisplayMemberArray() As String = {"管理者",
                                                     "利用者",
                                                      "利用者Ｓ"}
            Dim strValueMemberArray() As String = {"3",
                                                   "5",
                                                   "19"}
            Return Utilities.ConvertArrayToTableForComboBox(strDisplayMemberArray, strValueMemberArray, , True)
        End Get
    End Property
    'JPOC-1212
    Public Shared ReadOnly Property InstitutionAdminRoleList_EN As DataTable
        Get
            Dim strDisplayMemberArray() As String = {"Institution Administrator",
                                                     "Institution User",
                                                      "Institution Self User"}
            Dim strValueMemberArray() As String = {"3",
                                                   "5",
                                                   "19"}
            Return Utilities.ConvertArrayToTableForComboBox(strDisplayMemberArray, strValueMemberArray, , True)
        End Get
    End Property

    ''' <summary>
    ''' 【画像一覧非表示】文字
    ''' </summary>
    ''' <remarks>印刷ページの装飾用文字列【画像一覧非表示】</remarks>
    Public Const ImageInactiveMsg As String = "<span style='display:inline-block;color:red;background-color:yellow;'>【画像一覧非表示】</span>"
    ''' <summary>
    ''' 【非公開画像】文字
    ''' </summary>
    ''' <remarks>印刷ページの装飾用文字列【非公開画像】</remarks>
    Public Const ImageDisableMsg As String = "<span style='display:inline-block;color:red;background-color:yellow;'>【非公開画像】</span>"
    ''' <summary>
    ''' 【非公開画像】文字
    ''' </summary>
    ''' <remarks>印刷ページの装飾用文字列【出典記載不要画像】</remarks>
    Public Const ImageUnnecessaryMsg As String = "<span style='display:inline-block;color:red;background-color:yellow;'>【出典記載不要画像】</span>"

    '併用薬剤表示記号定義
    Public Const CONCOMITANT_DRUG_PLUS As String = "≡"
    Public Const CONCOMITANT_DRUG_PLUS_MINUS As String = "≡±≡"

#End Region

#Region "初期設定値(メインアプリケーションの起動時に設定する事)"

#Region "プロパティ"

#Region "必須(Web/JpocTools共用)"
    '設定順序重要！　以下の順序で設定する事！！
    Public Shared Property ApplicationType As PublicEnum.eApplicationType
    Public Shared Property AppRootPhysicalPath As String
    Public Shared Property RunTimeEnvironmentString As String
#End Region

#End Region

#End Region

#Region "UTCを基にした日本日時取得"
    ''' <summary>
    ''' UTCを基にした日本時間
    ''' </summary>
    ''' <returns>UTC+9:00の時間</returns>
    Public Shared ReadOnly Property JpnNow() As DateTime
        Get
            Return DateTime.UtcNow().AddHours(9)
        End Get
    End Property
    ''' <summary>
    ''' UTCを基にした日本日にち
    ''' </summary>
    ''' <returns>UTC+9:00の日にち</returns>
    Public Shared ReadOnly Property JpnDate() As Date
        Get
            Return DateTime.UtcNow().AddHours(9).Date
        End Get
    End Property

    Public Shared ReadOnly Property JpnLoginDate() As Date
        Get
            Return DateTime.Now.Date
        End Get
    End Property
#End Region

#Region "シングルトンインスタンス"
    ''' <summary>
    ''' AppMessagesインスタンス
    ''' </summary>
    ''' <returns>AppMessagesインスタンス</returns>
    ''' <remarks><c>AppMessages.GetInstance("Messages.xml")</c>を取得</remarks>
    Public Shared ReadOnly Property Messages As AppMessages
        Get
            Return AppMessages.GetInstance(Path.Combine(SettingsFolderPhysicalPath, "Messages.xml"))
        End Get
    End Property
    ''' <summary>
    ''' AppConfigureインスタンス
    ''' </summary>
    ''' <returns>AppConfigureインスタンス</returns>
    ''' <remarks><c>AppConfigure.GetInstance("AppSettings.xml")</c>を取得</remarks>
    Public Shared ReadOnly Property Configure As AppConfigure
        Get
            Return AppConfigure.GetInstance(Path.Combine(SettingsFolderPhysicalPath, "AppSettings.xml"))
        End Get
    End Property
    ''' <summary>
    ''' LogManagerインスタンス
    ''' </summary>
    ''' <returns>LogManagerインスタンス</returns>
    Public Shared ReadOnly Property Logger As ILog
        Get
            Return LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        End Get
    End Property
#End Region

#Region "実行環境"
    Public Shared ReadOnly Property RunTimeEnvironment() As PublicEnum.eRuntimeEnvironment
        Get
            Return Utilities.GetRuntimeEnvironmentValue(RunTimeEnvironmentString)
        End Get
    End Property
#End Region

#Region "実行アセンブリ名"
    Public Shared ReadOnly Property ExeAppFullPath As String
        Get
            Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.GetEntryAssembly()
            If asm Is Nothing Then Return String.Empty
            Return asm.Location
        End Get
    End Property
    Public Shared ReadOnly Property ExeAppDirPath As String
        Get
            Return System.IO.Path.GetDirectoryName(ExeAppFullPath)
        End Get
    End Property
    Public Shared ReadOnly Property ExeAppName As String
        Get
            Return System.IO.Path.GetFileName(ExeAppFullPath)
        End Get
    End Property
    Public Shared ReadOnly Property AssemblyFullPath As String
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly().Location
        End Get
    End Property
    Public Shared ReadOnly Property AssemblyDirPath As String
        Get
            Return System.IO.Path.GetDirectoryName(AssemblyFullPath)
        End Get
    End Property
    Public Shared ReadOnly Property AssemblyName As String
        Get
            Return System.IO.Path.GetFileName(AssemblyFullPath)
        End Get
    End Property
#End Region

#Region "カルチャ"
    Public Shared ReadOnly Property AppCultureInfo() As CultureInfo
        Get
            Return New CultureInfo(JA_JP)
        End Get
    End Property
#End Region

#Region "Path関連"
    Public Shared ReadOnly Property AppDataFolderPhysicalPath() As String
        Get
            Select Case ApplicationType
                Case Common.PublicEnum.eApplicationType.WebApplication, Common.PublicEnum.eApplicationType.WebService
                    Return Path.Combine(AppRootPhysicalPath, "App_Data")
                Case Else
                    Return Path.Combine(AppRootPhysicalPath, "AppData")
            End Select
        End Get
    End Property
    Public Shared ReadOnly Property SettingsFolderPhysicalPath() As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "Settings")
        End Get
    End Property
    Public Shared ReadOnly Property TemplatesFolderPhysicalPath() As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "Templates")
        End Get
    End Property

    Public Shared ReadOnly Property EnqueteWorkFolderPhysicalPath() As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "EnqueteWork")
        End Get
    End Property
    Public Shared ReadOnly Property WorkFolderPhysicalPath() As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "Work")
        End Get
    End Property
    Public Shared ReadOnly Property SqlCommandFolderPhysicalPath() As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "SqlCommand")
        End Get
    End Property
    Public Shared Property LastSelectedPath As String
    Public Shared ReadOnly Property Log4netConfigFilePath As String
        Get
            Return Path.Combine(SettingsFolderPhysicalPath, "log4net.config")
        End Get
    End Property
    Public Shared ReadOnly Property LogFolderPhysicalPath As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "Logs")
        End Get
    End Property

    Public Shared ReadOnly Property PhotoImportPhysicalPath As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "PhotoImport")
        End Get
    End Property
#End Region

#Region "設定ファイルから取得する項目"

#Region "3DES関連"
    Public Shared ReadOnly Property TripleDesKey As String
        Get
            Return Configure.GetConfig("TripleDesKey")
        End Get
    End Property
    Public Shared ReadOnly Property TripleDesIv As String
        Get
            Return Configure.GetConfig("TripleDesIv")
        End Get
    End Property
#End Region

    '#Region "WaterMark関連"
    '    Private Shared _waterMark As Byte()
    '    Public Shared Sub SetWaterMark(pData As Byte())
    '        _waterMark = Nothing
    '        ReDim _waterMark(pData.Length)
    '        Array.Copy(pData, _waterMark, pData.Length)
    '    End Sub
    '    Public Shared ReadOnly Property WaterMark As System.IO.MemoryStream
    '        Get
    '            If _waterMark IsNot Nothing AndAlso _waterMark.Length > 0 Then
    '                Return Utilities.ConvertByteToStream(_waterMark)
    '            End If
    '            Return New System.IO.MemoryStream
    '        End Get
    '    End Property
    '#End Region

#Region "デモ環境用設定"
    Public Shared ReadOnly Property IsDemo As PublicEnum.eDemoType
        Get

            Dim val As String = Configure.GetConfig("IsDemo")
            If String.IsNullOrEmpty(val) Then Return PublicEnum.eDemoType.None
            Select Case val.ToUpper
                Case "DEMO"
                    Return PublicEnum.eDemoType.DEMO
                Case Else
                    Return PublicEnum.eDemoType.None
            End Select
        End Get
    End Property
#End Region

#Region "デモ環境用管理者ログインフラグ"
    Public Shared ReadOnly Property DemoAdminIP As String
        Get
            Return Configure.GetConfig("DemoAdminIP")
        End Get
    End Property
#End Region

#Region "外部リンク設定"
    Public Shared ReadOnly Property DisableExternalLink As Boolean
        Get
            Dim val As String = Configure.GetConfig("DisableExternalLink")
            If String.IsNullOrEmpty(val) Then Return False
            Dim ret As Boolean = False
            If Boolean.TryParse(val, ret) Then
                Return ret
            End If
            Return False
        End Get
    End Property
    Public Shared ReadOnly Property LinkResolverUrl As String
        Get
            Return Configure.GetConfig("LinkResolverUrl")
        End Get
    End Property
#End Region

#Region "多重アクセスチェック項目"
    Public Shared ReadOnly Property EnableAccountBan As Boolean
        Get
            Dim val As String = Configure.GetConfig("EnableAccountBan")
            If String.IsNullOrEmpty(val) Then Return False
            Dim ret As Boolean = False
            If Boolean.TryParse(val, ret) Then
                Return ret
            End If
            Return False
        End Get
    End Property
    Public Shared ReadOnly Property AccountBanCheckTime As Integer
        Get
            If Not EnableAccountBan Then Return 0
            Return Utilities.N0Int(Configure.GetConfig("AccountBanCheckTime"))
        End Get
    End Property
    Public Shared ReadOnly Property AccountBanLimitCount As Integer
        Get
            If Not EnableAccountBan Then Return 0
            Return Utilities.N0Int(Configure.GetConfig("AccountBanLimitCount"))
        End Get
    End Property
    Public Shared ReadOnly Property AccountRemoveBanTime As Integer
        Get
            If Not EnableAccountBan Then Return 0
            Return Utilities.N0Int(Configure.GetConfig("AccountRemoveBanTime"))
        End Get
    End Property
#End Region

#Region "画像無し対応"
    Public Shared ReadOnly Property NoImageLinkBehavior As PublicEnum.eImageLinkBehavior
        Get
            Select Case Utilities.N0Int(Configure.GetConfig("NoImageLinkBehavior"), 2)
                Case 0
                    Return PublicEnum.eImageLinkBehavior.IconAndLink
                Case 1
                    Return PublicEnum.eImageLinkBehavior.IconOnly
                Case Else
                    Return PublicEnum.eImageLinkBehavior.HideIcon
            End Select
        End Get
    End Property
    Public Shared ReadOnly Property HideImageLinkBehavior As PublicEnum.eImageLinkBehavior
        Get
            Select Case Utilities.N0Int(Configure.GetConfig("HideImageLinkBehavior"), 0)
                Case 1
                    Return PublicEnum.eImageLinkBehavior.IconOnly
                Case 2
                    Return PublicEnum.eImageLinkBehavior.HideIcon
                Case Else
                    Return PublicEnum.eImageLinkBehavior.IconAndLink
            End Select
        End Get
    End Property
#End Region

#Region "トライアル期限関連"
    ''' <summary>
    ''' トライアル登録時設定期限
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property TrialDefaultExpiryDate As Integer
        Get
            Return Utilities.N0Int(Configure.GetConfig("TrialDefaultExpiryDate"))
        End Get
    End Property
    ''' <summary>
    ''' 更新が必要な契約期限
    ''' </summary>
    ''' <remarks>トライアルの場合、契約期限がこれの場合＋[TrialResetExpiryDate]日に更新する</remarks>
    Public Const UPDATE_NECESSART_EXPIRE As DateTime = #12/31/9999#
    ''' <summary>
    ''' トライアルの期限再設定日
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property TrialResetExpiryDate As Integer
        Get
            Return Utilities.N0Int(Configure.GetConfig("TrialResetExpiryDate"))
        End Get
    End Property
#End Region

#End Region

#Region "Solr関連"
    ''' <summary>
    ''' Solr使用可能フラグ
    ''' </summary>
    Private Shared _EnableSolr As Boolean = True
    Public Shared Sub SetEnableSolr(isSolr As Boolean)
        _EnableSolr = isSolr
    End Sub
    Public Shared ReadOnly Property EnableSolr As Boolean
        Get
            If GlobalVariables.IsDemo <> PublicEnum.eDemoType.DEMO Then
                Return _EnableSolr
            End If
            Return False
        End Get
    End Property
    ''' <summary>
    ''' Solr "症状・疾患" 検索文字列
    ''' </summary>
    ''' <remarks>disease:T_JP_Disease.title, 疾患:T_JP_Disease.synonym</remarks>
    Public Shared ReadOnly SolrDiseaseForSelect As String = "SearchTerm = 'disease' OR SearchTerm = '疾患'"
    ''' <summary>
    ''' Solr "薬剤" 検索文字列
    ''' </summary>
    ''' <remarks>drug:商品名, druggeneric:generic名</remarks>
    Public Shared ReadOnly SolrDrugForSelect As String = "SearchTerm = 'drug' OR SearchTerm = 'drugsubctg' OR SearchTerm = 'druggeneric' OR SearchTerm='drugnamesynonym' OR SearchTerm='drugsubctgsynonym'"
    ''' <summary>
    ''' Solr "検査" 検索文字列
    ''' </summary>
    Public Shared ReadOnly SolrLabForSelect As String = "SearchTerm = 'lab'"
    ''' <summary>
    ''' Solr "診療報酬点数表" 検索文字列
    ''' </summary>
    Public Shared ReadOnly SolrInsuranceForSelect As String = "SearchTerm = 'insurance'"
    ''' <summary>
    ''' Solr "医療計算機" 検索文字列
    ''' </summary>
    Public Shared ReadOnly SolrMcalcForSelect As String = "SearchTerm = 'mcalc'"
    ''' <summary>
    ''' Solr "症状・疾患" Contentype_s
    ''' </summary>
    ''' <remarks>disease:T_JP_Disease.title, 疾患:T_JP_Disease.synonym</remarks>
    Public Shared ReadOnly SolrDiseaseContents As New List(Of String) From {"disease", "疾患"}
    ''' <summary>
    ''' Solr "薬剤" Contentype_s
    ''' </summary>
    ''' <remarks>drug:商品名, druggeneric:generic名</remarks>
    Public Shared ReadOnly SolrDrugContents As New List(Of String) From {"drug", "drugsubctg", "druggeneric", "drugnamesynonym", "drugsubctgsynonym"}
    ''' <summary>
    ''' Solr "検査" Contentype_s
    ''' </summary>
    Public Shared ReadOnly SolrLabContents As New List(Of String) From {"lab"}
    ''' <summary>
    ''' Solr "診療報酬点数表" Contentype_s
    ''' </summary>
    Public Shared ReadOnly SolrInsuranceContents As New List(Of String) From {"insurance"}
    ''' <summary>
    ''' Solr "診療報酬点数表" Contentype_s
    ''' </summary>
    Public Shared ReadOnly SolrMcalcContents As New List(Of String) From {"mcalc"}
#End Region

#Region "Session for Password Enforcing"
    'added by SK - item 42393 - Password control function (JPoC)
    Public Shared ReadOnly Property IsFromEnforcePasswordPage As String
        Get
            Return "IsFromEnforcePasswordPage"
        End Get
    End Property

    'added by SK - item 42393 - Password control function (JPoC)
    Public Shared ReadOnly Property FromEnforcePasswordPage_Uid As String
        Get
            Return "FromEnforcePasswordPage_Uid"
        End Get
    End Property

    'added by SK - item 42393 - Password control function (JPoC)
    Public Shared ReadOnly Property FromEnforcePasswordPage_User_ID As String
        Get
            Return "FromEnforcePasswordPage_User_ID"
        End Get
    End Property

    'added by SK - item 42393 - Password control function (JPoC)
    Public Shared ReadOnly Property FromEnforcePasswordPage_Password As String
        Get
            Return "FromEnforcePasswordPage_Password"
        End Get
    End Property

    'added by SK - item 42393 - Password control function (JPoC)
    Public Shared ReadOnly Property FromEnforcePasswordPage_InstitutionCode As String
        Get
            Return "FromEnforcePasswordPage_InstitutionCode"
        End Get
    End Property

    'added by SK - item 42393 - Password control function (JPoC)
    Public Shared ReadOnly Property FromEnforcePasswordPage_InstitutionID As String
        Get
            Return "FromEnforcePasswordPage_InstitutionID"
        End Get
    End Property
#End Region

#Region "SiteInformation"

    Public Shared ReadOnly Property LatestXMonthsAnnouncement As Integer
        Get
            Return Utilities.N0Int(Configure.GetConfig("LatestXMonthsAnnouncement"))
        End Get
    End Property

    Public Shared ReadOnly Property LatestXNoAnnouncement As Integer
        Get
            Return Utilities.N0Int(Configure.GetConfig("LatestXNoAnnouncement"))
        End Get
    End Property

#End Region


    Public Shared ReadOnly Property isFoundation As Boolean
        Get
            Return RunTimeEnvironment = PublicEnum.eRuntimeEnvironment.FOUND_LOCAL Or RunTimeEnvironment = PublicEnum.eRuntimeEnvironment.FOUND_UAT Or RunTimeEnvironment = PublicEnum.eRuntimeEnvironment.FOUND_PROD
        End Get
    End Property
    Public Shared ReadOnly Property PageTitle As String
        Get
            Return "今日の臨床サポート｜エビデンスベースのオンラインリファレンスツール"
        End Get
    End Property

    'JPOC-747/815
    Public Shared ReadOnly Property DailyContentShareLimit As Integer
        Get
            Return Utilities.N0Int(Configure.GetConfig("DailyContentShareLimit"))
        End Get
    End Property


    Public Shared ReadOnly Property ContentShareValidity As Integer
        Get
            Return Utilities.N0Int(Configure.GetConfig("ContentShareValidity"))
        End Get
    End Property

    Public Shared ReadOnly Property ShareDefaultExpiryDate As Integer
        Get
            Return Utilities.N0Int(Configure.GetConfig("ShareDefaultExpiryDate"))
        End Get
    End Property

    'JPOC-952
    Public Shared ReadOnly Property DrugDosageImageImportPhysicalPath As String
        Get
            Return Path.Combine(AppDataFolderPhysicalPath, "DrugDosageImageImport")
        End Get
    End Property
End Class

