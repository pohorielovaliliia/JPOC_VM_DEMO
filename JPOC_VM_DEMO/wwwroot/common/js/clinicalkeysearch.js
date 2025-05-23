$(document).ready(function () {

    // エラーの場合
    // 「結果表示できません」を表示
    function ClinicalKeyNoResult() {
        var ckbox = $('.ck_search_box')
        var ckSearchResult = $('<div></div>').addClass('ck_search_result');
        var ckTitle = $('<span></span>').addClass('sub').html('現在、検索結果が表示できません');
        ckSearchResult.append(ckTitle);
        ckbox.append(ckSearchResult);
    }

    // 処理前用
    // 「取得中…」を表示
    function PreClinicalKey() {
        var ckbox = $('.ck_search_box')
        var ckSearchResult = $('<div></div>').addClass('ck_search_result');
        var ckTitle = $('<span></span>').addClass('sub').html('取得中…');
        ckSearchResult.append(ckTitle);
        ckbox.append(ckSearchResult);
    }

    // ClinicalKey検索結果からjQueryDOMを作成
    // 「結果表示できません」を表示
    function CreateClinicalKeyResult(data) {

        var ckSearchResult = $('<div></div>').addClass('ck_search_result');

        // タイトル
        var ckTitle = $('<span></span>').addClass('sub').html(data.title);
        ckSearchResult.append(ckTitle);


        var resultTitle = $('<div></div>').addClass('ck_result_title');

        // source_title(存在する場合のみ)
        if (data.sourceTitle != "") {
            var sTitle = $('<div></div>').addClass('ck_source_title');
            var sTitleAncher = $('<a></a>').html(data.sourceTitle);
            sTitle.append(sTitleAncher);
            resultTitle.append(sTitle);
        }

        // item_title
        var itemTitle = $('<a></a>').html(data.itemTitle);
        itemTitle.attr('target', '_blank');
        itemTitle.attr('href', data.linkUrl);
        resultTitle.append(itemTitle);

        ckSearchResult.append(resultTitle);

        return ckSearchResult;
    }

    $.support.cors = true;
    if ($('#hdnSearch')) {
        $.ajax({
            type: 'POST',
            url: 'ajax/ClinicalKeySearchForAjax.aspx',
            cache: true,
            async: true,
            dataType: "json",
            data: {
                searchText: $('#hdnSearch').val(),
                diseaseId: $('#hdnDiseaseId').val(),
                parentPage: $('#hdnParentPage').val()
            },
            beforeSend: function (xhr, settings) {
                // 「取得中…」を表示
                PreClinicalKey();
            },
            success: function (data) {

                // 取得中を削除
                $('.ck_search_box').find('.ck_search_result').remove();

                if (data.length == 0) {
                    // 検索結果無し
                    ClinicalKeyNoResult();
                } else {
                    // 追加してゆく
                    var ckbox = $('.ck_search_box')
                    $.each(data, function (item) {
                        ckbox.append(CreateClinicalKeyResult(this));
                    });
                }
            },
            error: function (httpReq, status, err) {
                // 取得中を削除
                $('.ck_search_box').find('.ck_search_result').remove();
                // エラー
                ClinicalKeyNoResult();
            },
            complete: function (httpReq, status) {
                // alert(status);
            }
        });
    } else {
        // 検索文字列取得出来ない場合
        ClinicalKeyNoResult();
    }
});