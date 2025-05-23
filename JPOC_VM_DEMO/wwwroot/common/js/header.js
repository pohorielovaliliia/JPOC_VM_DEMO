// JPOC-236 click event for the updated Top Nav bar
$(document).ready(function () {
    $(".jpoc-nav").hide();

    $('#imgAdminMenu').click(function (e) {
        e.preventDefault();
        $(".jpoc-nav").hide();
        $("#adminMenu-nav").toggle();
    });

    $('#imgUserAccount').click(function (e) {
        e.preventDefault();
        $(".jpoc-nav").hide();
        $("#userMenu-nav").toggle();
    });

    $('#imgNotification').click(function (e) {
        e.preventDefault();
        $(".jpoc-nav").not('#AnnouncementMenu-nav').hide();
        $("#AnnouncementMenu-nav").toggle();

        setAnnouncementBadge();
    });

    $('#imgNotification').hover(function () {
        var query = $("#AnnouncementMenu-nav");
        $(".jpoc-nav").hide();
        var isVisible = query.is(':visible');
        if (isVisible == false) {
            $("#AnnouncementMenu-nav").show();
            setAnnouncementBadge();
        }
    });

    $('#closeAnnouncement').click(function () {
        $("#AnnouncementMenu-nav").hide();
    });
    

    $('#imgQuestion').click(function (e) {
        e.preventDefault();
        $(".jpoc-nav").hide();
        $("#welcome-nav").toggle();
    });

    $('.jpoc-guide-popup').click(function (e) {
        e.preventDefault();
        $('a.firstdesc').eq(0).click();
    });

    checkAnnouncement();


    $(".iframe_feedback").colorbox({
        iframe: true,
        // innerWidth: "750px",
        // Must not set innerWidth and innerHeight, because ie6 is used.
        width: "840px",
        // innerHeight: "500px",
        height: "600px",
        maxWidth: "90%",
        maxHeight: "90%",
        // fixed: true,
        returnFocus: true,
        overlayClose: false,
        title: "<div style='margin-bottom:0px;'><a href='javascript:void(0);' onclick='JavaScript:return sendFeedback();' alt='メール'><img src='common/images/modal_sendemail.gif' alt='送信' width='132' height='32' /></a>&nbsp;&nbsp;<a href='javascript:void(0);' onclick='JavaScript:return closeColorbox();'><img src='common/images/modal_cancel.gif' alt='キャンセル' width='132' height='32' /></a></div>",
        onOpen: function () {
            openedColorbox = $(".iframe_feedback").colorbox;
            // hideSearchTermForIe6();
        },
        onClosed: function () {
            openedColorbox = null;
            // showSearchTermForIe6();
        }
    });

    $(document).mouseup(function (e) {
        if (!$(e.target).closest('#imgUserAccount').length) {
            if (!$(e.target).closest('#userMenu-nav').length)
                $('#userMenu-nav').hide();
        };

        if (!$(e.target).closest('#imgAdminMenu').length) {
            if (!$(e.target).closest('#adminMenu-nav').length)
                $('#adminMenu-nav').hide();
        };


        if (!$(e.target).closest('#imgNotification').length) {
            if (!$(e.target).closest('#AnnouncementMenu-nav').length)
                $('#AnnouncementMenu-nav').hide();
        };

        if (!$(e.target).closest('#imgQuestion').length) {
            if (!$(e.target).closest('#welcome-nav').length)
                $('#welcome-nav').hide();
        };

    });

    // TODO: JPOC-161 : UNCOMMENT line below to replace solr autocomplete with T_JP_KeywordCounter for frequent searched keyword
    initAutoCompleteSearchBox();
});


function setAnnouncementBadge(){
    var cookiename = $("#hdnAnnouncementCookiesName").val();
    setCookie(cookiename, "lastTimeStamp=", 30);
    $("#BadgeNotification").hide();
}

function setCookie(cname, cvalue, exdays) {
    var clickDate = new Date();
    if (exdays) {
        var date = new Date();
        date.setTime(date.getTime() + (exdays * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    var month = ("0" + (clickDate.getMonth() + 1)).slice(-2);
    var newdate = clickDate.getFullYear() + "/" + month + "/" + ("0" + clickDate.getDate()).slice(-2) + " " + clickDate.getHours() + ":" + clickDate.getMinutes() + ":" + clickDate.getSeconds();

    document.cookie = cname + "=" + cvalue + newdate + ";" + expires + ";path=/";
}

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}



function checkAnnouncement() {
    var cookiename = $("#hdnAnnouncementCookiesName").val();
    var lastannouncementString = $("#hdnlastAnnouncementDate").val();
    var lastTimestampString = getCookie(cookiename);
    if (isempty(cookiename)) {
        $("#BadgeNotification").hide();
    }
    else if (isempty(lastannouncementString)) {
        $("#BadgeNotification").hide();
    }
    else
    {
        if (isempty(lastTimestampString)) {
            $("#BadgeNotification").show();
        }
        else {
            lastTimestampString = lastTimestampString.replace('lastTimeStamp=', '');
            var LastAnnouncementDate = new Date(lastannouncementString);
            var lastTimestampDate = new Date(lastTimestampString);

            if (lastTimestampDate >= LastAnnouncementDate) {
                $("#BadgeNotification").hide();
            } else {
                $("#BadgeNotification").show();
            }
        }
    }     
}

function isempty(str) {
    if (typeof str == 'undefined' || !str || str.length === 0 || str === "" || !/[^\s]/.test(str) || /^\s*$/.test(str) || str.replace(/\s/g, "") === "") {
        return true;
    }
    else {
        return false;
    }
}






// Replace solr autoComplete under search box with KeywordCounter
// Function below referred to solr_select.js
function initAutoCompleteSearchBox(){
    var selectUrl = 'ajax/OnePageStructureServices.aspx/DisplayFrequentSearchedKeyword';
    var cache = {};
    if($('#txtSearchText').length){
        
        $('#txtSearchText').autocomplete({
            source: function (request, response) {
                var term = $.trim(request.term);
                var param = {searchText: term};
                
                // キャッシュが存在する場合は、キャッシュから取得
                if (term in cache) {
                    response($.map(cache[term], function (item) {
                        return {
                            label: item.keyword,
                            value: item.keyword
                        }
                    }))
                }
                

                $.ajax({
                    async: true
                    , cache: false
                    , type: 'POST'
                    , dataType: 'json'
                    , url: selectUrl
                    , data: JSON.stringify(param)
                    , contentType: "application/json; charset=utf-8"
                    , beforeSend: function (xhr, settings) {
                        $('.ajaxBusy').show();
                    }
                    , success: function (data) {
                        cache[term] = data.d.FrequentKeyword;
                        response($.map(data.d.FrequentKeyword, function (item) {
                            return {
                                label: item.keyword,
                                value: item.keyword
                            }
                        }))
                    }
                    , error: function (data) {
                        console.log(data);
                    }
                    , complete: function (xhr, status) {
                        $('.ajaxBusy').hide();
                    }
                });
            },
            minLength: 1, delay: 300,
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
                autocompleteSubmit(ui.item.value);
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
             //chromeは日本語入力中に変換(Enterキー)を判別して拾えない為、制御しない
             if (getUserAgentInfo().Type == 'chrome') {
                 $(this).autocomplete("search");
             } else if (event.keyCode == 13 //enter
                 || event.keyCode == 32 //space
                 || (event.keyCode >= 48 && event.keyCode <= 90) // 0-9, a-z
                 || (event.keyCode >= 96 && event.keyCode <= 105) //Num 0-9
             ) {
                  $(this).autocomplete("search");
             }
        })
        .blur(function (event) {
            // ipad, iphone対応、ソフトウェアキーボード閉じたときはblurで拾う
            if (getUserAgentInfo().Type == 'ipad' || getUserAgentInfo().Type == 'iphone') {
                $(this).autocomplete("search");
            }
        })
    }

    // Autocomplete用の画面遷移
    function autocompleteSubmit(pSearchText) {
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
        var redirectUrl = "fulltextsearch.aspx"
                            + "?SearchTerm=" + encodeURI('すべて')
                            + "&SearchText=" + encodeURI(pSearchText)
                            + "&SearchTab=all"

        // 全文検索のすべての薬剤チェック用
        if ($("#chkAllDrugSearch").attr("checked")) {
            redirectUrl += "&AllDrugFlag=true"
        }

        $('.searchBusy').show();

        location.href = redirectUrl;

        dupValCheckElementVal.value = '1';

        return false;
    }
}
$(document).ready(function () {
    if (document.getElementById('NavLinkShare')) {
        $("#BadgeNotification").css("left", "40px");
    }
});