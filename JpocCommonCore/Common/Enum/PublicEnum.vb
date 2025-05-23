Imports System.ComponentModel

Public Class PublicEnum
    Public Enum eApplicationType
        Undefined
        WindowForm
        WebApplication
        WebService
    End Enum
    ''' <summary>
    ''' ユーザー種類
    ''' </summary>
    Public Enum eUserType
        ''' <summary>
        ''' 未定
        ''' </summary>
        Undefined = 0
        ''' <summary>
        ''' 内部ユーザー(エルゼビア)
        ''' </summary>
        INT = 1
        ''' <summary>
        ''' 個人ユーザー
        ''' </summary>
        IND = 2
        ''' <summary>
        ''' 施設ユーザー
        ''' </summary>
        INS = 3
    End Enum

    ''' <summary>
    ''' 実行環境
    ''' </summary>
    ''' <remarks>
    ''' AppSetingsのENV_[XXXX]の[XXXX]部
    ''' </remarks>
    Public Enum eRuntimeEnvironment
        ''' <summary>
        ''' LOCAL(開発)環境
        ''' </summary>
        LOCAL
        ''' <summary>
        ''' IT環境
        ''' </summary>
        IT
        ''' <summary>
        ''' DEMO環境
        ''' </summary>
        DEMO
        ''' <summary>
        ''' Preview環境
        ''' </summary>
        PREV
        ''' <summary>
        ''' 本番環境
        ''' </summary>
        PROD
        ''' <summary>
        ''' VM
        ''' </summary>
        VM
        ''' <summary>
        ''' DEV
        ''' </summary>
        DEV
        ''' <summary>
        ''' PRE
        ''' </summary>
        PRE
        ''' <summary>
        ''' TEST
        ''' </summary>
        TEST
        ''' <summary>
        ''' FOUND_UAT
        ''' </summary>
        FOUND_UAT
        ''' <summary>
        ''' FOUND_LOCAL
        ''' </summary>
        FOUND_LOCAL
        ''' <summary>
        ''' FOUND_PROD
        ''' </summary>
        FOUND_PROD
        ''' <summary>
        ''' ELIB
        ''' </summary>
        ELIB
        ''' <summary>
        ''' ENURSE
        ''' </summary>
        ENURSE
    End Enum

    ''' <summary>
    ''' ページアクセス権
    ''' </summary>
    ''' <remarks>
    ''' None=アクセス権なし
    ''' ReferenceOnly=読み取り専用
    ''' Editable=読み書き可能
    ''' </remarks>
    Public Enum eAccsessRight
        ''' <summary>
        ''' アクセス権なし
        ''' </summary>
        None = 0
        ''' <summary>
        ''' 読み取り専用
        ''' </summary>
        ReferenceOnly = 1
        ''' <summary>
        ''' 読み書き可能
        ''' </summary>
        Editable = 2
    End Enum

    ''' <summary>
    ''' 対象ページ
    ''' </summary>
    ''' <remarks>タブメニューインデックス</remarks>
    Public Enum eTabTargetPage
        ''' <summary>
        ''' 症状・疾患
        ''' </summary>
        Disease = 0
        ''' <summary>
        ''' 評価・治療例
        ''' </summary>
        Situation = 1
        ''' <summary>
        ''' 画像
        ''' </summary>
        ImageList = 2
        ''' <summary>
        ''' エビデンス・解説
        ''' </summary>
        Evidence = 3
        ''' <summary>
        ''' 詳細情報
        ''' </summary>
        Actions = 4
    End Enum

    ''' <summary>
    ''' DAOで使用するキー項目の種類
    ''' </summary>
    ''' <remarks>検索キー項目を示す</remarks>
    Public Enum eKeyValueType
        ''' <summary>
        ''' DiseaseID
        ''' </summary>
        DiseaseID = 0
        ''' <summary>
        ''' SituationID
        ''' </summary>
        SituationID = 1
        ''' <summary>
        ''' SituationOrderSetID
        ''' </summary>
        SituationOrderSetID = 2
        ''' <summary>
        ''' SituationOrderSetParentID
        ''' </summary>
        SituationOrderSetParentID = 3
        ''' <summary>
        ''' SituationOrderSetSampleID
        ''' </summary>
        SituationOrderSetSampleID = 4
        ''' <summary>
        ''' SituationOrderSetPatientProfileID
        ''' </summary>
        SituationOrderSetPatientProfileID = 5
        ''' <summary>
        ''' SrlID
        ''' </summary>
        SrlID = 6
        ''' <summary>
        ''' Name
        ''' </summary>
        Name = 7
        ''' <summary>
        ''' Code
        ''' </summary>
        Code = 8
    End Enum

    ''' <summary>
    ''' アップロード時の編集モード
    ''' </summary>
    ''' <remarks>新規か変更かを示す</remarks>
    Public Enum eUploadFileMode
        ''' <summary>
        ''' 新規
        ''' </summary>
        [New] = 0
        ''' <summary>
        ''' 更新
        ''' </summary>
        Edit = 1
    End Enum

    ''' <summary>
    ''' DiseaseActionType及びActionItemsのデータ種別
    ''' </summary>
    ''' <remarks>ヘッダかボディかを示す</remarks>
    Public Enum eActionDataType
        ''' <summary>
        ''' 未定
        ''' </summary>
        Undifined = -1
        ''' <summary>
        ''' ボディ部
        ''' </summary>
        Body = 0
        ''' <summary>
        ''' ヘッダー行
        ''' </summary>
        Header = 1
    End Enum

    ''' <summary>
    ''' ページ種類
    ''' </summary>
    ''' <remarks>表示する内容を示す</remarks>
    Public Enum ePageType
        ''' <summary>
        ''' 未定
        ''' </summary>
        Undifined = 0
        ''' <summary>
        ''' 疾患・症状
        ''' </summary>
        Disease = 1
        ''' <summary>
        ''' 評価・治療例
        ''' </summary>
        Situation = 2
        ''' <summary>
        ''' 画像
        ''' </summary>
        ImageList = 3
        ''' <summary>
        ''' エビデンス
        ''' </summary>
        Evidence = 4
        ''' <summary>
        ''' 詳細情報
        ''' </summary>
        Actions = 5
        ''' <summary>
        ''' 検査
        ''' </summary>
        LabTest = 6
        ''' <summary>
        ''' 薬剤
        ''' </summary>
        Drug = 7
        ''' <summary>
        ''' 白本
        ''' </summary>
        Shirobon = 8
        ''' <summary>
        ''' Added 2016/10/04
        ''' </summary>
        Handout = 9
    End Enum

    ''' <summary>
    ''' ジャーナル（リファレンス）ソース区分
    ''' </summary>
    ''' <remarks>ジャーナルが何に紐付いているかを示す</remarks>
    Public Enum eJournalType
        ''' <summary>
        ''' すべて
        ''' </summary>
        All = 0
        ''' <summary>
        ''' 画像
        ''' </summary>
        Image = 1
        ''' <summary>
        ''' エビデンス
        ''' </summary>
        Evidence = 2
        ''' <summary>
        ''' Step1
        ''' </summary>
        Step1 = 3
    End Enum

    ''' <summary>
    ''' ユーザーロール値
    ''' </summary>
    ''' <remarks>
    ''' ユーザの役割番号<br />
    ''' T_JP_Roleと一致していること
    ''' </remarks>
    Public Enum eRole
        ''' <summary>
        ''' 不定
        ''' </summary>
        Undefine = 0
        ''' <summary>
        ''' エルゼビア管理者
        ''' </summary>
        <Description("ElsevierAdmin")>
        ElsevierAdmin = 1
        ''' <summary>
        ''' 施設管理者
        ''' </summary>
        <Description("InstitutionAdmin")>
        InstitutionAdmin = 3
        ''' <summary>
        ''' IPアドレスログイン
        ''' </summary>
        <Description("InstitutionIP")>
        InstitutionIP = 4
        ''' <summary>
        ''' 施設ユーザー
        ''' </summary>
        <Description("InstitutionUser")>
        InstitutionUser = 5
        ''' <summary>
        ''' 個人ユーザー
        ''' </summary>
        <Description("Individual")>
        Individual = 6
        ''' <summary>
        ''' トライアルユーザー
        ''' </summary>
        <Description("Trial")>
        Trial = 7
        ''' <summary>
        ''' エルゼビア編集者
        ''' </summary>
        <Description("ElsevierEditor")>
        ElsevierEditor = 8
        ''' <summary>
        ''' エルゼビアユーザー管理
        ''' </summary>
        <Description("ElsevierManager")>
        ElsevierManager = 9
        ''' <summary>
        ''' エルゼビア担当者
        ''' </summary>
        <Description("ElsevierUser")>
        ElsevierUser = 10
        ''' <summary>
        ''' VM管理者
        ''' </summary>
        <Description("VMAdmin")>
        VMAdmin = 11
        ''' <summary>
        ''' VMユーザ(自動ログインユーザ)
        ''' </summary>
        <Description("VMUser")>
        VMUser = 12
        ''' <summary>
        ''' 営業アカウント
        ''' </summary>
        <Description("ElsevierSales")>
        ElsevierSales = 13
        ''' <summary>
        ''' テクニカルサポート
        ''' </summary>
        <Description("TechnicalSupport")>
        TechnicalSupport = 14
        ''' <summary>
        ''' DVD ユーザー
        ''' </summary>
        <Description("DvdUser")>
        DvdUser = 15
        ''' <summary>
        ''' 代理店ユーザー
        ''' </summary>
        <Description("Agency")>
        Agency = 16
        ''' <summary>
        ''' VMコンテンツ作業者
        ''' </summary>
        <Description("VMContents")>
        VMContents = 17
        ''' <summary>
        ''' InstitutionSelfUser 2016 04 20
        ''' </summary>
        <Description("InstitutionSelfUser")>
        InstitutionSelfUser = 19
        ''' <summary>
        ''' Tentative
        ''' </summary>
        <Description("Tentative")>
        Tentative = 20
        <Description("RegistrationID")>
        RegistrationID = 21
        <Description("Subscriber")>
        Subscriber = 22
        <Description("Entitlement Audit")>
        EntitlementAudit = 23
        <Description("Entitlement Support")>
        EntitlementSupport = 24
        <Description("JP Support")>
        JPSupport = 25
        <Description("Elsevier Marketing")>
        ElsevierMarketing = 26
        <Description("Individual Management")>
        IndividualManagement = 27
        <Description("Support Admin")>
        SupportAdmin = 28
        <Description("RegistrationIDForM3")>
        RegistrationIDForM3 = 29
        <Description("RegistrationIDForTrialM3")>
        RegistrationIDForTrialM3 = 30
        <Description("TrialSubscriber")>
        TrialSubscriber = 31
    End Enum

    ''' <summary>
    ''' ログイン種類
    ''' </summary>
    Public Enum eLoginType
        ''' <summary>
        ''' 未設定（未ログイン）
        ''' </summary>
        Undifined = 0
        ''' <summary>
        ''' パスワード
        ''' </summary>
        Password = 1
        ''' <summary>
        ''' IPアドレス
        ''' </summary>
        IpAddress = 2
    End Enum

    ''' <summary>
    ''' メッセージ種類
    ''' </summary>
    ''' <remarks>
    ''' Undifined=未定
    ''' Question=質問
    ''' Information=情報
    ''' Warning=警告
    ''' Error=業務論理エラー
    ''' Fatal=システムエラー
    ''' </remarks>
    Public Enum eMessageType
        Undifined = 0
        Question = 1
        Information = 2
        Warning = 3
        [Error] = 4
        Fatal = 5
    End Enum


    ''' <summary>
    ''' 画像へのリンクを作成する際の挙動
    ''' </summary>
    Public Enum eImageLinkBehavior
        ''' <summary>
        ''' アイコンも表示しリンクも設定
        ''' </summary>
        IconAndLink = 0
        ''' <summary>
        ''' アイコンは表示するがリンクは張らない
        ''' </summary>
        IconOnly = 1
        ''' <summary>
        ''' アイコン非表示
        ''' </summary>
        HideIcon = 2
    End Enum
    Public Enum eRtnCD
        Normal = 0
        LogicalError = 1
        FaitalError = 9
    End Enum

    Public Enum eJobResult
        Success = 0
        Warning = 1
        [Error] = 9
    End Enum

    Public Enum eImageType
        Undefined = 0
        RawImage = 1
        TbImage = 2
        AdImage = 3
        DpImage = 4
    End Enum

    Public Enum eStepEventType
        Message = 0
        StartEvent = 1
        EndEvent = 2
    End Enum

    ''' <summary>
    ''' デモタイプ
    ''' </summary>
    Public Enum eDemoType
        ''' <summary>
        ''' 未デモ
        ''' </summary>
        None = 0
        ''' <summary>
        ''' デモ１
        ''' </summary>
        DEMO = 1
    End Enum

    Public Enum eButtonAction
        Undifined = 0
        ShowForm = 1
        ShowFormAsDialog = 2
        CloseForm = 3
        UpdateDataBase = 4
        SelectDataBase = 5
        AnyTransaction = 6
    End Enum

    Public Enum eKeyDataType
        [Integer]
        [String]
        CsvString
    End Enum

    Public Enum eInputType
        Undefined
        Entry
        SearchCriteria
        MultiPurpose
    End Enum

    Public Enum eContentType
        Text
        Image
    End Enum

    Public Enum eMinMax
        MIN
        MAX
    End Enum

    Public Shared Function TryParseEnumByName(enumType As Type, enumName As String) As Object
        If [Enum].IsDefined(enumType, enumName) Then
            Try
                Return [Enum].Parse(enumType, enumName, False)
            Catch
            End Try
        End If
        Return Nothing
    End Function



End Class
