/* コンテンツ内容に依存するページエレメント操作系スクリプトを記述 */

$(function () {

    explanationDispMsg();
    orderSetMsg();

    $('#txtSearchText').keydown(function (event) {
        if (event.keyCode == '13') {
            if ($('#btnSearch')) {
                $('#btnSearch').click();
            } else {
                return false;
            }
        }
    });

    //テキストボックス上のキーエンターでポストバックを起こさせない為の処理
    $('#txtSearchTextDummy').keydown(function (event) {
        if (event.keyCode == '13') {
            return false;
        }
    });



});

/* オーダーセット全体表示・非表示 */
function switchAll() {
    if ($("#expand01Img").attr("src") == "common/images/marker_expand01.png") {
        $('.DisplayBlock').map(function () {
            $(this).css("display", "block");
        });
        $('.expandIcon').map(function () {
            $(this).attr("src", "common/images/marker_check02.png");
        });
        $("#expand01Img").attr("src", "common/images/marker_expand02.png");
    } else {
        $('.DisplayBlock').map(function () {
            $(this).css("display", "none");
        });
        $('.expandIcon').map(function () {
            $(this).attr("src", "common/images/marker_check01.png");
        });
        $("#expand01Img").attr("src", "common/images/marker_expand01.png");
    }
}

function expandComment1() {
    var newSrc = $("img#comments_img").attr("src");
    if (newSrc.toString().indexOf("_expand01\.") >= 0) {
        try {
            $('img[src$="marker_check01.png"]').attr("src", $('img[src$="marker_check01.png"]').attr("src").replace("marker_check01\.", "marker_check02\."));
            $("img#comments_img").attr("src", $("img#comments_img").attr("src").replace("_expand01\.", "_expand02\."));
            $("div[name=comments]").css("display", "block");
            $("img[name=commentsimgNEW]").css("visibility", "hidden");
        } catch (e) {
            // no do.
        }
    } else {
        try {
            $('img[src$="marker_check02.png"]').attr("src", $('img[src$="marker_check02.png"]').attr("src").replace("marker_check02\.", "marker_check01\."));
            $("img#comments_img").attr("src", $("img#comments_img").attr("src").replace("_expand02\.", "_expand01\."));
            $("div[name=comments]").css("display", "none");
            $("img[name=commentsimgNEW]").css("visibility", "visible");
        } catch (e) {
            // no do.
        }
    }
}

function expandComment2(obj) {
    var ordersetId = $(obj).attr("id").replace("commentsimg_", "");
    var divlength = $("#commentsid_" + ordersetId).length;
    if (divlength > 0) {
        var display = $("#commentsid_" + ordersetId).css("display");
        var newSrc = $(obj).attr("src");
        var commentBlock = $("#commentsid_" + ordersetId);
        var commentIcon = $("img#commentsimgNEW_" + ordersetId);
        if (display == "none") {
            newSrc = newSrc.replace("_check01\.", "_check02\.");
            if (commentBlock != null) {
                commentBlock.css("display", "block");
            }
            if (commentIcon != null) {
                commentIcon.css("visibility", "hidden");
            }
        } else {
            newSrc = newSrc.replace("_check02\.", "_check01\.");
            if (commentBlock != null) {
                commentBlock.css("display", "none");
            }
            if (commentIcon != null) {
                commentIcon.css("visibility", "visible");
            }
        }
        $(obj).attr("src", newSrc);
    }
}

function expandComment3(obj) {
    var ordersetId = $(obj).attr("id").replace("commentsimgNEW_", "");
    var divlength = $("#commentsid_" + ordersetId).length;
    if (divlength > 0) {
        var display = $("#commentsid_" + ordersetId).css("display");
        var newSrc = $("#commentsimg_" + ordersetId).attr("src");

        if (display == "none") {
            newSrc = newSrc.replace("_check01\.", "_check02\.");
            $("#commentsid_" + ordersetId).css("display", "block");
            $(obj).css("visibility", "hidden");
        } else {
            newSrc = newSrc.replace("_check02\.", "_check01\.");
            $("#commentsid_" + ordersetId).css("display", "none");
            $(obj).css("visibility", "visible");
        }
        $("#commentsimg_" + ordersetId).attr("src", newSrc);
    }
}

function closeComment(obj) {
    var ordersetId = $(obj).attr("id").replace("commentsClose_", "");
    var divlength = $("#commentsid_" + ordersetId).length;

    if (divlength > 0) {
        var display = $("#commentsid_" + ordersetId).css("display");
        var newSrc = $("#commentsimg_" + ordersetId).attr("src");

        if (display == "block") {
            newSrc = newSrc.replace("_check02\.", "_check01\.");
            $("#commentsid_" + ordersetId).css("display", "none");
        }
        $("#commentsimg_" + ordersetId).attr("src", newSrc);
    }
}

function expandComment4(obj) {
    var targetID = $(obj).attr("id").replace("Ref_Img_", "");
    var display = $("span#Ref_" + targetID).css("display");
    if (display == "none") {
        $("span#Ref_" + targetID).css("display", "inline");
        var newSrc = $(obj).attr("src").replace("_open\.", "_close\.");
        $(obj).attr("src", newSrc);
    } else {
        $("span#Ref_" + targetID).css("display", "none");
        var newSrc = $(obj).attr("src").replace("_close\.", "_open\.");
        $(obj).attr("src", newSrc);
    }
}

function orderSetMsg() {

    $("img#comments_img,span#comments_expand")
        .keydown(function (event) {
            if (event.keyCode == '13') {
                return expandComment1();
            }
        })
        .click(function () {
            expandComment1();
        });

    // 評価治療例？
    $('img[name="commentsimg"]')
        .keydown(function (event) {
            if (event.keyCode == '13') {
                return expandComment2(this);
            }
        })
        .click(function () {
            expandComment2(this);
        });

    $('img[name="commentsimgNEW"]')
        .keydown(function (event) {
            if (event.keyCode == '13') {
                return expandComment3(this);
            }
        })
        .click(function () {
            expandComment3(this);
        });

    $('img[name="commentsClose"]')
        .keydown(function (event) {
            if (event.keyCode == '13') {
                return closeComment(this);
            }
        })
        .click(function () {
            closeComment(this);
        });

    $('div[name="JournalList"] img.Button')
        .keydown(function (event) {
            if (event.keyCode == '13') {
                return expandComment4(this);
            }
        })
        .click(function () {
            expandComment4(this);
        });
}

function explanationDispMsg() {
    // https://stackoverflow.com/questions/2459153/alternative-to-jquerys-toggle-method-that-supports-eventdata
    // .toggle with 2 handler deprecated in 1.8 https://api.jquery.com/toggle-event/

    $('div#MainContents ul.Explanation li img.Button, div#MainContents ul.Explanation li img.Button2').click(function() {
        var clicks = $(this).data('clicks');
        clicks = clicks === undefined ? true : clicks ; // show the hidden section upon the first click
        
        if (clicks) {
            $(this).siblings().css("display", "block");
            //20130228 Add (S) Requested By K.Kawakami
            $(this).siblings().find("dl").css("display", "block");
            //20130228 Add (E)
            var newSrc = $(this).attr("src").replace("_open\.", "_close\.");
            $(this).attr("src", newSrc);
        } else {
            $("dl", this).css("display", "none");
            $(this).siblings().css("display", "none");
            //20130228 Add (S) Requested By K.Kawakami
            $(this).siblings().find("dl").css("display", "none");
            //20130228 Add (E)
            var newSrc = $(this).attr("src").replace("_close\.", "_open\.");
            $(this).attr("src", newSrc);
        }
        $(this).data("clicks", !clicks);
    });


    $(document).ready(function () {
        var elem = $('.Button2');
        for (i = 0; i < elem.length; ++i) elem[i].click();
    });

}

function show_orderset() {
    var src1 = "common/images/main_btn_explanation_lv2_open.gif";
    var src2 = "common/images/main_btn_explanation_lv2_close.gif";
    var src = $("#table_img").attr("src");
    if (src == src1) {
        $("#table_img").attr("src", src2);
        $("#order_set_body").css("display", "table-row-group");
    } else {
        $("#table_img").attr("src", src1);
        $("#order_set_body").css("display", "none");
    }
    src = null;
}

function addSearchHistory(pSearchText) {
    var serviceUrl = 'ajax/Service.aspx';
    $.ajax({
        async: false
        , cache: false
        , type: 'POST'
        , dataType: 'json'
        , url: serviceUrl
        , data: { action: 'SetSearchHistory', param: pSearchText }
        , error: function (data) {
            // alert('Ajax Access Error\nStatus:' + data.status + '\nMessage:' + data.statusText);
        }
        , success: function (data) { }
    });
}

/* 個別全体表示・非表示 */
function switchItem(id) {
    var result = $("#expand01Contents" + id).is(":visible");
    if (result) {
        $("#expand01Contents" + id).css("display", "none");
        $("#expand01Icon" + id).attr("src", "common/images/marker_check01.png");
    } else {
        $("#expand01Contents" + id).css("display", "block");
        $("#expand01Icon" + id).attr("src", "common/images/marker_check02.png");
    }
    return false;
}

// test for ie6 memory leak.
addOnUnLoad(function () {
    // アタッチされているイベントをすべて削除したい
    $("#wrapper *").unbind();
    // GCの明示的呼出
    if (window.CollectGarbage) {
        window.CollectGarbage();
    }
});


