/**
	トライアル期限切れpopup.js
    以下のHTMLを生成する
    <div id="ipop">
        <div id="ipop_close">
            <img id="ipop_close_button" src="osirase_close.png">
        </div>
        <div id="ipop_contents">
	        <div class="pop_info">
		        <span class="empha">重要なお知らせ</span>
	        </div>
	        <div class="pop_contents">
		        無料トライアルは<span class="empha">00日後</span>に終了いたします。<br />
		        ぜひ、ご継続をご検討ください。
	        </div>
        </div>
        <div id="ipop_footer">
	        <div id="ipop_footer_contents">
		        <p>
			        臨床の疑問に答える専門医のアドバイスが凝縮。<br />
			        「今日の臨床サポート」
		        </p>
	        </div>
	        <div id="ipop_footer_button">
		        <a href="#" class="">購入する</a>
	        </div>
        </div>
    </div>
*/
$(document).ready(function () {
    var pop = $('<div id="ipop">');

    var pop_close = $('<div id="ipop_close">');
    var pop_close_buttin = $('<img id="ipop_close_button" src="common/images/osirase_close.png">');
    pop_close.append(pop_close_buttin);
    pop.append(pop_close);

    var pop_contents = $('<div id="ipop_contents">');
    var pop_info = $('<div class="pop_info">');
    pop_info.html('<span class="empha">重要なお知らせ</span>');
    pop_contents.append(pop_info);
    var pop_contents_html = $('<div class="pop_contents">');

    var htmlValue = '無料トライアルは<span class="empha">'
                  + $('#hdnTrialExpireLimit').val()
                  + '日後</span>に終了いたします<br />ぜひ、ご継続をご検討ください。';
    pop_contents_html.html(htmlValue);
    pop_contents.append(pop_contents_html);

    pop_contents_html = $('<div class="ipop_description">');
    pop_contents_html.html('１ID、12000円/年にて契約可能です。<a href="TrialEntry.aspx#Contract">個人契約の場合には＞＞</a>');
    pop_contents.append(pop_contents_html);

    pop_contents_html = $('<div class="ipop_description">');
    pop_contents_html.html('Ｘｘｘの場合には施設契約がお得です。<a href="Quotation.aspx">施設契約の場合には＞＞</a>');
    pop_contents.append(pop_contents_html);

    pop.append(pop_contents);

    var pop_footer = $('<div id="ipop_footer">');

    var pop_footer_contents = $('<div id="ipop_footer_contents">');
    pop_footer_contents.html('<p>臨床の疑問に答える専門医のアドバイスが凝縮。<br />「今日の臨床サポート」</p>');
    pop_footer.append(pop_footer_contents);

    var pop_footer_button = $('<div id="ipop_footer_button">');
    var pop_footer_button_image = $('<a href="https://clinicalsup.jp/jpoc/TrialEntry.aspx#Contract" target="_blank">');
    pop_footer_button_image.html('購入する');
    pop_footer_button.append(pop_footer_button_image);
    pop_footer.append(pop_footer_button);

    pop.append(pop_footer);

    $('body').append(pop);
    $.ipop();
});
(function ($) {
    $.ipop = function () {

        var wx, wy;

        // ウインドウの座標を画面中央にする。
       wx = $(document).scrollLeft() + ($(window).width() - $('#ipop').outerWidth()) / 2;
       if (wx < 0) wx = 0;
       wy = $(document).scrollTop() + ($(window).height() - $('#ipop').outerHeight()) / 2;
       if (wy < 0) wy = 0;

        // ウィンドウの位置を固定化
        // wx = $(document).scrollTop() + 312;
        // wy = $(document).scrollLeft() + 253;

        // ポップアップウインドウを表示する。
        $('#ipop').css({ top: wy, left: wx }).fadeIn(100);

        // 閉じるボタンを押したとき
        $('#ipop_close_button').click(function () { $('#ipop').fadeOut(100); });

        // POPUPをドラッグしたとき
        $('#ipop').mousedown(function (e) {
            var mx = e.pageX;
            var my = e.pageY;
            $(document).on('mousemove.ipop', function (e) {
                wx += e.pageX - mx;
                wy += e.pageY - my;
                $('#ipop').css({ top: wy, left: wx });
                mx = e.pageX;
                my = e.pageY;
                return false;
            }).one('mouseup', function (e) {
                $(document).off('mousemove.ipop');
            });
            return false;
        });
    }
})(jQuery);
