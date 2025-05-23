// IE6対応の為に、IE6では取り込まない
// Scriptをここに記述
// 前提条件: jQuery,contentsCommon.js, jquery.lazyload

// DrugCustomization.prototype.url = "/jpoc/ajax/DrugCustomization.aspx";

$(function () {
    // lazyload 設定 
    $("img.lazy").lazyload();

    //○保表示
    //Select all anchor tag with rel set to tooltip
    $('div[rel=maruho_tooltip]').mouseover(function (e) {
        /** IE6 z-index 対応の為、普段はpsotion:static;にしておき、必要のある分のみrelativeにおきかえる */
        $(this).css('position', 'relative');
        //Grab the title attribute's value and assign it to a variable
        var tip = $(this).attr('altTitle');
        var desc = $(this).attr('name');
        //Append the tooltip template and its value
        $(this).append('<div id="tooltip_maruho"><div class="entry"><div class="maruhoArrow" ></div><div class="maruhoBox"></div></div></div>')
        $('.maruhoBox').append('<div class="title">' + tip + '</div>');
        $('.maruhoBox').append('<div class="txt">' + desc + '</div>');

        /** leftの位置として、左メニュー分を考慮する */
        /** またelementに対して若干ひだりに寄せる(矢印画像横幅考慮) */
        if (($('body').width() - $(this).offset().left) > 220) {
            // POPUP領域(220px想定)が十分にある場合は、右出力
            $('#tooltip_maruho').css('left', -14);
        } else {
            // POPUP領域(220px想定)が十分にない場合は、出力を左によせる
            $('#tooltip_maruho').css('left', $(this).position().left - 190);
            $(this).find('div.maruhoArrow').css({
                'background-position': '190px 0px'
            });
        }

        if (getUserAgentInfo().Name == 'ie6' || getUserAgentInfo().Name == 'ie7') {
            $('#tooltip_maruho').css('top', 14);

            $(this).closest('div.OrderItemTitle').css('z-index', '2');
            $(this).closest('table').css('z-index', '2');
            // $('#tooltip_maruho').css('z-index', '2');
        }

    }).mouseout(function () {
        $(this).children('div#tooltip_maruho').remove();
        $(this).css('position', 'static');
    });
});
