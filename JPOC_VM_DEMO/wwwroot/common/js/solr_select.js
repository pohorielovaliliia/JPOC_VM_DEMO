$(document).ready(function () {
    $.support.cors = true;
    var selectUrl = 'ajax/SolrSelect.aspx';
    var cache = {};
    var intxt = $('#txtSearchText');

    // TODO: JPOC-161 : UNCOMMENT line below to replace solr autocomplete with T_JP_KeywordCounter for frequent searched keyword
    var intxt = $('#txtSearchText_SolrAutocomplete');
    intxt.autocomplete({
        source: function (request, response) {
            var term = request.term;
            // キャッシュが存在する場合は、キャッシュから取得
            if (term in cache) {
                response($.map(cache[term], function (item) {
                    return {
                        label: item.key,
                        value: item.value,
                        term: item.term
                    }
                }))
            }
            $.ajax({
                async: true
                , cache: false
                , type: 'POST'
                , dataType: 'json'
                , url: selectUrl
                , data: {
                    searchText: term
                }
                , beforeSend: function (xhr, settings) {
                    $('.ajaxBusy').show();
                }
                , success: function (data) {
                    cache[term] = data;
                    response($.map(data, function (item) {
                        return {
                            label: item.key,
                            value: item.value,
                            term: item.term,
                            dataId: item.dataId
                        }
                    }))
                }
                , complete: function (xhr, status) {
                    $('.ajaxBusy').hide();
                }
            });
        },
        minLength: 1, delay: 0,
        search: function (event, ui) {
            // 上下(左右)ではイベントを走らせない
            if (event.keyCode == 37
                || event.keyCode == 38
                || event.keyCode == 39
                || event.keyCode == 40) {
                return false;
            }
            // chromeは日本語入力中に変換(Enterキー)を判別して拾えない為、制御しない
            if (getUserAgentInfo().Type == 'chrome') {
                return true;
            }
            if (event.keyCode == 229) {
                return false;
            }
            return true;
        },
        select: function (event, ui) {

            $(this).val(ui.item.value);
            autocompleteSubmit(ui.item.term, ui.item.value, ui.item.dataId);
        }
    })
    .keyup(function (event) {
        // 上下(左右)ではイベントを走らせない
        if (event.keyCode == 37
            || event.keyCode == 38
            || event.keyCode == 39
            || event.keyCode == 40) {
            return;
        }
        // chromeは日本語入力中に変換(Enterキー)を判別して拾えない為、制御しない
        if (getUserAgentInfo().Type == 'chrome') {
            $(this).autocomplete("search");
        } else if (event.keyCode == 13) {
            $(this).autocomplete("search");
        }
    })
    .blur(function (event) {
        // ipad, iphone対応、ソフトウェアキーボード閉じたときはblurで拾う
        if (getUserAgentInfo().Type == 'ipad' || getUserAgentInfo().Type == 'iphone') {
            $(this).autocomplete("search");
        }
    })

    //アイコン設定
    .data("autocomplete")._renderItem = function (ul, item) {
        var termName = ""
        if (item.term == "症状・疾患") {
            termName = "疾患・症状"
        } else if (item.term == "薬剤") {
            termName = "薬剤"
        } else if (item.term == "検査") {
            termName = "検査"
        } else if (item.term == "画像") {
            termName = "画像"
        } else if (item.term == "診療報酬点数表") {
            termName = "診療報酬"
        } else if (item.term == "医療計算機") {
            termName = "医療計算機"
        }
        // アイコン文字列設定
        var iconData = $('<li></li>').data("item.autocomplete", item);
        iconData.append('<a><div class="auto_complete_term_name"><span>' + termName + '</span></div>' + item.label + '</a>');
        return iconData.appendTo(ul);
    }
    delete intxt;
    expandSolr();

    function expandSolr() {
        var targetObject;
        var viewCount = 0;
        // 各カテゴリに対して、各々の折畳み件数を定義
        if ($('#perfectDiseaseListItem') != "undefine") {
            targetObject = $('#perfectDiseaseListItem');
            viewCount = $('#defaultPerfectDiseaseViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        if ($('#perfectDrugListItem') != "undefine") {
            targetObject = $('#perfectDrugListItem');
            viewCount = $('#defaultPerfectDrugViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        if ($('#perfectLabListItem') != "undefine") {
            targetObject = $('#perfectLabListItem');
            viewCount = $('#defaultPerfectLabViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        if ($('#perfectInsuranceItem') != "undefine") {
            targetObject = $('#perfectInsuranceItem');
            viewCount = $('#defaultPerfectInsuranceViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        if ($('#partialDiseaseListItem') != "undefine") {
            targetObject = $('#partialDiseaseListItem');
            viewCount = $('#defaultPartialDiseaseViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        if ($('#partialDrugListItem') != "undefine") {
            targetObject = $('#partialDrugListItem');
            viewCount = $('#defaultPartialDrugViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        if ($('#partialLabListItem') != "undefine") {
            targetObject = $('#partialLabListItem');
            viewCount = $('#defaultPartialLabViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        if ($('#partialInsuranceItem') != "undefine") {
            targetObject = $('#partialInsuranceItem');
            viewCount = $('#defaultPartialInsuranceViewCount').val();

            invisibleElement(targetObject, viewCount);
            createSeeMoreElement(targetObject, viewCount);
        }
        delete targetObject;
        delete viewCount;
    }
    // タグの非表示処理(検索結果コンテンツ)
    function invisibleElement(parentObject, higherThanCount) {
        var targetElementTag = "div";
        // 対象タグが存在しない場合は処理しない
        if (parentObject.find(targetElementTag).size() == 0) {
            return false;
        }
        // 対象タグの数が表示設定数以下の場合、非表示にしない（全表示）
        if (parentObject.find(targetElementTag).size() <= higherThanCount) {
            return false;
        }

        var invisibleTargetTag = targetElementTag;
        if (higherThanCount > 0) {
            invisibleTargetTag = targetElementTag + ':gt(' + (parseInt(higherThanCount) - 1) + ')';
        }
        // 非表示処理
        parentObject.find(invisibleTargetTag).each(
                function () {
                    $(this).hide();
                });
        return true;
    }

    // もっと読むリンク生成
    function createSeeMoreElement(parentObject, defaultViewCount) {
        // (h5:検索結果数)Elementの数が無い場合は処理しない
        if (parentObject.find('div').size() == 0) {
            return false;
        }

        // (h5:検索結果数)Elementの数が表示設定数以下の場合、処理させない
        if (parentObject.find('div').size() <= defaultViewCount) {
            return false;
        }

        parentObject.append('<a>…もっと見る</a>');
        var seeMoreElement = parentObject.find('a:last');

        // もっと読むリンク押下時の一度に表示するカウント数
        var seeMoreLinkShow = 80;

        seeMoreElement
                .css({
                    'padding-left': '8em',
                    'display': 'block',
                    'color': '#017FD6',
                    'text-decoration': 'underline',
                    'cursor': 'pointer'
                })
                .click(
                    function () {
                        var targetElements = $(this).parent().find('div');
                        var hiddenElementsCount = 0;
                        var showElementsCount = 0;
                        // 非表示要素のカウント
                        targetElements.each(
                            function () {
                                if ($(this).css('display') == 'none') {
                                    hiddenElementsCount++;
                                }
                            });


                        if (hiddenElementsCount == 0) {
                            // 非表示要素が0の場合は、もっと見るを消すだけ（表示はされていない想定）
                            $(this).remove();
                            return false;
                        }

                        // 表示要素数()
                        if (parseInt(targetElements.size()) - parseInt(hiddenElementsCount) > 0) {
                            showElementsCount = parseInt(targetElements.size()) - parseInt(hiddenElementsCount);
                        }
                        // 検索結果を表示する
                        if (parseInt(hiddenElementsCount) > parseInt(seeMoreLinkShow)) {
                            var showCount = parseInt(showElementsCount) + parseInt(seeMoreLinkShow);
                            $(this).parent().find('div:lt(' + showCount + ')').show();
                        } else {
                            // 隠れている残り数が既定の表示数以下の場合は、もっと読むリンクは削除
                            $(this).parent().find('div').show();
                            $(this).remove();
                        }
                    }
                );
        delete seeMoreElement;
    }


    // Autocomplete用の画面遷移
    function autocompleteSubmit(pSearchTermText, pSearchText, pDataId) {
        var curUrl = document.URL;

        var dupValCheckElement = document.getElementById("hdnDupClickControl");
        if (dupValCheckElement == null
            || dupValCheckElement == undefined
            || dupValCheckElement == 'undefined') {
            return false;
        }
        var dupValCheckElementVal = dupValCheckElement.value;
        if (dupValCheckElementVal != null && dupValCheckElementVal.length != 0) {
            return false;
        }
        var redirectUrl;
        var uniqueParam = '';
        var onePageParam = '';

        if (pSearchTermText == "症状・疾患") {
            redirectUrl = "ContentPage.aspx";
            onePageParam = "?DiseaseID=" + pDataId;
            // uniqueParam = "&DiseaseID=" + pDataId;
        } else if (pSearchTermText == "薬剤") {
            redirectUrl = "drugDetails.aspx";
            uniqueParam = "&Code=" + pDataId;
        } else if (pSearchTermText == "検査") {
            redirectUrl = "labtestDetails.aspx";
            uniqueParam = "&Code=" + pDataId;
        } else if (pSearchTermText == "画像") {
            //redirectUrl = "ImageSearch.aspx";
        } else if (pSearchTermText == "診療報酬点数表") {
            redirectUrl = "shinryou.aspx";
            uniqueParam = "&File=" + pDataId;
        } else if (pSearchTermText == "医療計算機") {
            redirectUrl = "fulltextsearch.aspx";
            // uniqueParam = "&name=" + pDataId;
            // NOTE: PC版は医療計算機の独自画面が無い為、全文検索に遷移
            // また検索用Tabは合わせておく
            uniqueParam = "&SearchTab=mcalc";
        } else if (pSearchTermText == "すべて") {
            /* 
            基点となるページを設定
            searchページの場合はsearchDetailに遷移
            それ以外は自身のページを基点とする。
            （検索順序に影響する）
            */
            if (curUrl.toLowerCase().indexOf('search.aspx') >= 0) {
                redirectUrl = "ContentPage.aspx";
            } else {
                var curPos = curUrl.indexOf("?");
                if (curPos <= 0) {
                    redirectUrl = curUrl;
                } else {
                    redirectUrl = curUrl.substr(0, curPos);
                }
                // 起点ページが検索可能ページではない場合強制的にsearchDetailに遷移
                if (redirectUrl.toLowerCase().indexOf('searchdetails.aspx') < 0 &&
                    redirectUrl.toLowerCase().indexOf('contentpage.aspx') < 0 &&
                    redirectUrl.toLowerCase().indexOf('drugdetails.aspx') < 0 &&
                    redirectUrl.toLowerCase().indexOf('labtestdetails.aspx') < 0 &&
                    redirectUrl.toLowerCase().indexOf('imagesearch.aspx') < 0 &&
                    redirectUrl.toLowerCase().indexOf('shinryou.aspx') < 0) {
                    redirectUrl = "ContentPage.aspx";
                }
            }
        } else {
            redirectUrl = "ContentPage.aspx";
        }

        if (!pDataId) {
            onePageParam ='';
            uniqueParam = '';
        }

        if(onePageParam.length > 0){
            redirectUrl = redirectUrl
                        + onePageParam
                        + "&SearchTerm=" + encodeURI(pSearchTermText)
                        + "&SearchText=" + encodeURI(pSearchText)
        }else{
            redirectUrl = redirectUrl
                        + "?SearchTerm=" + encodeURI(pSearchTermText)
                        + "&SearchText=" + encodeURI(pSearchText)
        }

        // 全文検索対応(QueryStringのSearchtabがある場合取得して設定する)
        if (getUrlVars()['SearchTab']) {
            redirectUrl += "&SearchTab=" + getUrlVars()['SearchTab']
        }

        // autocompleteからの場合全文検索に遷移させない
        // 薬剤中分類検索の為にロジックは残す
        if (true) {
            redirectUrl += "&SearchType=autocomplete"
        }

        // 全文検索のすべての薬剤チェック用
        if ($("#chkAllDrugSearch").attr("checked")) {
            redirectUrl += "&AllDrugFlag=true"
        }

        redirectUrl = redirectUrl + uniqueParam

        $('.searchBusy').show();

        location.href = _encode(redirectUrl);

        dupValCheckElementVal.value = '1';

        addSearchHistory(pSearchText);

        return false;
    }
});

function _encode(str) {
    const el = document.createElement('div');
    el.textContent = str;
    var res = el.innerHTML;
    res = res.replaceAll("&lt;", "<");
    res = res.replaceAll("&gt;", ">");
    res = res.replaceAll("&amp;", "&");
    return res;
}