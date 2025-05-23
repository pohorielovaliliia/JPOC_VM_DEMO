$(document).ready(function () {
    $(".openAdvice").unbind();
    $(".openAdvice").click(function () {
        tid = "#" + $(this)[0].id.replace("_b", "_a");
        if ($(tid).attr("class").match(/NoShow/)) {
            $(tid).attr("class", $(tid).attr("class").replace(/adviceNoShow/, "adviceShow"));
            $("#" + $(this)[0].id).text("▲詳細を閉じる▲");
            $("#" + $(this)[0].id).attr("title", "詳細を閉じる");
        } else {
            $(tid).attr("class", $(tid).attr("class").replace(/adviceShow/, "adviceNoShow"));
            $("#" + $(this)[0].id).text("▼詳細を見る▼");
            $("#" + $(this)[0].id).attr("title", "詳細を表示");
        }
        return false;
    });
});

$(window).load(function(){
	setTimeout(function(){
		$(".adviceArea").each(function(){
		$(this).attr("class","adviceArea adviceShow");
	})},510);
});