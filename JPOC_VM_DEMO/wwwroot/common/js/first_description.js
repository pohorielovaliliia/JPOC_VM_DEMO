/** 簡単ガイド */
$(document).ready(function () {

    var isDescription = null;

    var colorBoxItem = $("<div></div>");

    // イメージ数に対してイメージのリンク及びappendを行う
    // 変数名の数字は意味は無い(ユニークにしたかっただけ)
    var image1 = $('<a class="firstdesc" href="common/images/description/pic1-top.png" ></a>');
    colorBoxItem.append(image1);
    var image2 = $('<a class="firstdesc" href="common/images/description/pic2-1_orders.png" ></a>');
    colorBoxItem.append(image2);
    var image3 = $('<a class="firstdesc" href="common/images/description/pic2-2_evidence.png" ></a>');
    colorBoxItem.append(image3);
    var image4 = $('<a class="firstdesc" href="common/images/description/pic3-2_algorithm.png" ></a>');
    colorBoxItem.append(image4);
    var image5 = $('<a class="firstdesc" href="common/images/description/pic3-2_handout.png" ></a>');
    colorBoxItem.append(image5);
    var image6 = $('<a class="firstdesc" href="common/images/description/pic4_drug_info.png" ></a>');
    colorBoxItem.append(image6);
    var image7 = $('<a class="firstdesc" href="common/images/description/pic4_labtest_info.png" ></a>');
    colorBoxItem.append(image7);
    var image8 = $('<a class="firstdesc" href="common/images/description/pic5-1_drugalart.png" ></a>');
    colorBoxItem.append(image8);
    var image9 = $('<a class="firstdesc" href="common/images/description/pic6_hanrei.png" ></a>');
    colorBoxItem.append(image9);
    var image10 = $('<a class="firstdesc" href="common/images/description/pic_end.png" ></a>');
    colorBoxItem.append(image10);

    // ドキュメントの最後に設定
    $("#wrapper").append(colorBoxItem);

    //var titleBar = $("<div style='margin-bottom:0px;'><a href='javascript:void();' onclick='JavaScript:return closeColorbox();'><img src='common/images/modal_close.gif' alt='閉じる' width='132' height='32' /></a></div>");

    var titleBar = $("<div style='margin-bottom:0px;'></div>");
    var titleBarImage = $("<a href='javascript:void();' onclick='JavaScript:return closeColorbox();'><img src='common/images/modal_close.gif' alt='閉じる' width='132' height='32' /></a>");
    var modalNextImage = $("<a href='javascript:void();' onclick='JavaScript:$.colorbox.next();'><img src='common/images/modal_next.gif' alt='進む' width='132' height='32' /></a>");
    var modalPrevImage = $("<a href='javascript:void();' onclick='JavaScript:$.colorbox.prev();'><img src='common/images/modal_back.gif' alt='戻る' width='132' height='32' /></a>");
    // var modalNextImage = $("<input type='image' src='common/images/modal_next.gif' alt='進む' width='132' height='32' onclick='JavaScript:$.colorbox.next();'/>");
    // var modalPrevImage = $("<input type='image' src='common/images/modal_back.gif' alt='戻る' width='132' height='32' onclick='JavaScript:$.colorbox.prev();' />");
    titleBar.append(modalPrevImage);
    titleBar.append(titleBarImage);
    titleBar.append(modalNextImage);

    // css は colorbox.css に定義
    var nextButton = $("<input type='image' id='cboxNextdescription' src='common/images/description/right.png' />");
    var prevButton = $("<input type='image' id='cboxPreviousdescription' src='common/images/description/left.png' />");


    $(nextButton).click(function () {
            $.colorbox.next();
    });
    $(prevButton).click(function () {
            $.colorbox.prev();
    });

    // colorbox のopen
    $("a.firstdesc").colorbox({
        rel: 'firstdesc',
        title: titleBar,
        width: '900px',
        innerWidth: '800px',
        height: '520px',
        innerHeight: '514px',
        previous: false,
        next: false,
        // current: false,
        closeButton: false,
        loop: true,
        onOpen: function () {
            openedColorbox = $("a.firstdesc").colorbox;
            isDescription = true;
        },
        onClosed: function () {
            openedColorbox = null;
            isDescription = false;
        }
    });

    $(document)
        .bind('cbox_open', function () {
            // onOpenより先に実行される為、とりあえず作成
            $('#cboxContent').append(nextButton);
            $('#cboxContent').append(prevButton);
            nextButton.css({}).show();
            prevButton.css({}).show();
        })
        .bind('cbox_load', function () {
            // onOpenより後に実行される為、パラメータを確認して、動作でない場合、非表示
            if (!isDescription) {
                nextButton.hide();
                prevButton.hide();
            }
        })
        .bind('cbox_closed', function () {
            nextButton.hide();
            prevButton.hide();
        });
});