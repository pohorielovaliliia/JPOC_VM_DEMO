var g_currentPageWithParam, g_historyAPISupported;
$(document).ready(function () {

    g_currentPageWithParam = window.location.href
        .replace(window.location.hash, '')
        .substring(window.location.href.lastIndexOf('/') + 1);

    InitVariousPolyfill();

    OPMWidget.init(Config_ContentPage);
    
});

// Misc
function InitVariousPolyfill() {
    // for Date.now() under IE 8
    if (!Date.now) {
        Date.now = function now() {
            return new Date().getTime();
        };
    }

    // for trim() under IE 8
    if(typeof String.prototype.trim !== 'function') {
      String.prototype.trim = function() {
        return this.replace(/^\s+|\s+$/g, ''); 
      }
    }

    // only replace history if feature supported
    g_historyAPISupported = !!(window.history && window.history.pushState);
}

function isAnchorToSameDiseaseForSamePage(currentPageUrl, targetURL) {
    // smooth scroll to related section if current page and query string 'DiseaseID' are the same, and there's no JournalID present in current url
    // https://github.com/medialize/URI.js/issues/31
    var URL_currentPage = URI();
    var URL_target = URI(targetURL);

    var JournalID_currentPage = URL_currentPage.query(true)["JournalID"];

    var DiseaseID_currentPage = URL_currentPage.query(true)["DiseaseID"];
    var DiseaseID_target = URL_target.query(true)["DiseaseID"];

    var PathName_currentPage = URL_currentPage.filename();
    var PathName_target = URL_target.filename();

    return (JournalID_currentPage === undefined) && (DiseaseID_currentPage === DiseaseID_target) && (PathName_currentPage.toLowerCase() === PathName_target.toLowerCase());
}

function scrollToDiv(targetHash) {
    var trgt = targetHash,
        target_offset = $(trgt).offset(),
        target_top = target_offset.top;


    //smooth scroll to target section, disable the scroll event temporary
    OPMWidget.initScrollEvent(false);

    $('html, body').stop().animate(
        { scrollTop: target_top }
        , 500
        , function () {
            changeHash(g_currentPageWithParam, targetHash);
            OPMWidget.initScrollEvent(true);
        });
}

function changeHash(g_currentPageWithParam, anchor) {
    // http://diveintohtml5.info/everything.html
    // Collection of HTML 5 feature detecting, alternative of Modernizer
    if( g_historyAPISupported ){
        history.replaceState(null, null, g_currentPageWithParam + anchor);
    }
};


function isInViewableSection(elem, partial) {
    // $.fn.isInViewport = function() {
    //     // https://stackoverflow.com/questions/20791374/jquery-check-if-element-is-visible-in-viewport
    //     var elementTop = $(this).offset().top;
    //     var elementBottom = elementTop + $(this).outerHeight();

    //     var viewportTop = $(window).scrollTop();
    //     var viewportBottom = viewportTop + $(window).height();

    //     return elementBottom > viewportTop && elementTop < viewportBottom;
    // };

    if ($('.active-item').length > 0) {
        // https://stackoverflow.com/questions/16308037/detect-when-elements-within-a-scrollable-div-are-out-of-view
        var container = $(".left_menu");
        var contHeight = container.height();
        var contTop = container.scrollTop();
        var contBottom = contTop + contHeight;

        var elemTop = $(elem).offset().top - container.offset().top;
        var elemBottom = elemTop + $(elem).height();

        var isTotal = (elemTop >= 0 && elemBottom <= contHeight);
        var isPart = ((elemTop < 0 && elemBottom > 0) || (elemTop > 0 && elemTop <= container.height())) && partial;

        return isTotal || isPart;
    }
    return true;

}

function scrollToActiveItem() {
    // https://stackoverflow.com/questions/2905867/how-to-scroll-to-specific-item-using-jquery
    var $container = $('#divLeftmenuItem'),
        $scrollTo = $('.active-item');

    $container.scrollTop(
        $scrollTo.offset().top - $container.offset().top + $container.scrollTop()
    );
}




// JS widget with event collection/ajax data population for one page merger
var OPMWidget = (function () {
    /*  involve Handlebar JS as template engine for ajax returned results
        and references of Config JSON that's setup on the top of the List control so that it can be modify on both user control and in here
    */

    var s; // private alias to defaultSettings
    var u; // private alias to Config_ContentPage
    var clearFilter = false;


    //JPOC-373: Retrieve SituationOrderSet's title located in ActionItem's body directly and populate in left tree menu
    function _initPrescriptionTitleAnchor(){
        // <li[^>]*>処方例[：:](.*?)</li>
        // https://stackoverflow.com/questions/190253/jquery-selector-regular-expressions
        var arrmatched = $('#MainContents05 li').filter(function() {
            return $(this).html().match(/^処方例[：:](.*?)/g);
        });
        var parsedResult=   {  'Prescription':
                                {
                                    'Listing':[]
                                }
                            };
        if(arrmatched.length > 0){
            $.each(arrmatched, function(i, v){
                var title = $(v).html().replace(/<.[^>]*>/g, '').split('処方例')[1].substring(1);
                var titleAnchor = {
                    'title': title,
                    'anchor':'prescription_anchor_'+i
                };
                parsedResult.Prescription.Listing.push(titleAnchor);

                // insert id into this particular li tag for the anchor
                $(v).attr('id', titleAnchor.anchor).addClass('anchor-prescription');
            });

            _populateHandleBarTemplate(u.Template_PrescriptionList, u.Frame_PrescriptionList, parsedResult);
        }

        // for scroll event's in the prescription anchors generated 
        $('.left_menu').on('click', '.target-anchor-prescription', function(e){
            var url = $(this).attr('href');
            var targetHash = url.substring(url.indexOf('#'));

            e.preventDefault();
            scrollToDiv(targetHash);
        });
        
    }

    function _reSyncLeftMenu(targetHash) {
        //add .active-item, and scroll to it if it's out of view

        var menuItems = $("#divLeftmenuItem a");
        var parsedID = targetHash.replace('#', '');
        if (parsedID.indexOf('ID08') >= 0) {
            //if user scrolled through the order set sample, just highlight the "検査・処方例" is enough, as One-page structure doesn't populate each of the item in the left tree menu
            menuItems.removeClass('active-item').filter('[href*="#ID08"]').addClass('active-item');
            if (!isInViewableSection('.active-item', false)) {
                scrollToActiveItem();
            }
        } else {
            // JPOC-409 check if a is hidden or not, add active-item to their parent anchor if hide
            var currentActiveMenuAnchor = menuItems.removeClass('active-item').filter('[href*="#' + parsedID + '"]');

            if(currentActiveMenuAnchor.hasClass('hidden')){
                currentActiveMenuAnchor.closest('ul').siblings('.onepage-nav-to-firstchild').addClass('active-item');
            }else{
                currentActiveMenuAnchor.addClass('active-item');
            }
            if (!isInViewableSection('.active-item', false)) {
                scrollToActiveItem();
            }
        }
    }


    function _initAnchorLink() {
        //for <a /> tag generated by ConvLinkTag()
        $('a[href^="ContentPage.aspx"]').click(function (e) {
            var targetURL = $(this).attr('href');

            // if disease note section is expanded (means all the other sections in the page are hidden), reload the page with the url
            if(s.isNoteExpanded == true){
                $(this).trigger(s.customEvent.eHiddenDiseaseNote);
            }

            //smooth scroll only if link don't have reference page redirection in JPOC-230
            if (targetURL.indexOf('JournalID') == -1 ) {
                if (targetURL.indexOf('#') >= 0) {
                    if (isAnchorToSameDiseaseForSamePage(g_currentPageWithParam, targetURL)) {
                        e.preventDefault();
                        //compare vice versa as the current page's url may longer than href attr in the a link clicked (additoinal SearchTab and SearchType when redirected from solr engine)
                        //a hack to append # anchor (into the url without hash) without reload the page
                        var targetHash = targetURL.substring(targetURL.indexOf('#'));
                        scrollToDiv(targetHash);
                        _reSyncLeftMenu(targetHash);
                    }
                }else{
                    if($(this).hasClass('onepage-nav-to-firstchild')){
                        e.preventDefault();
                        $(this).siblings('ul').find('li:first-of-type a').click();
                        $(this).addClass('active-item');
                        scrollToActiveItem();

                    }
                }
            }
        });

        //for footer link
        $('.anchor-link').click(function (e) {
            e.preventDefault();
            var url = $(this).attr('href');
            var targetHash = url.substring(url.indexOf('#'));

            scrollToDiv(targetHash);
        });


    }



    function _updatePosition_OPM() {
        //239-Move ContentNote adn COI to the bottom of the OnePageStructure instead of the bottom of Disease-OnePageStructure.ascx
        var targetHolder = $('#phContentNotes-placeholder');
        $('#phContentNotes-container').insertBefore(targetHolder);

        targetHolder = $('#phCOI-placeholder');
        $('#phCOI-container').insertBefore(targetHolder);

        targetHolder = $('#phMedicalsafety-placeholder');
        $('#phMedicalsafety-container').insertBefore(targetHolder);

        targetHolder = $('#phHistoryText-placeholder');
        $('#phHistoryText-container').insertBefore(targetHolder);

        targetHolder = $('#phCaseSearch-placeholder');
        $('#phCaseSearch-container').insertBefore(targetHolder);


        // patch for vh buggy implementaiton for left menu
        $('.left_menu .content').height($(window).height()-150);



        // JPOC-338 Highlight items for special page 'situationDetails' and 'imageTab'
        var currentUri = URI();
        if(currentUri.filename().toLowerCase().indexOf('situationdetails.aspx')>=0){
            $('.nav-situation-highlighter').addClass('active-item').closest('li').siblings().find('.active-item').removeClass('active-item');
            
            setTimeout(
                function(){
                    $('.nav-situation-highlighter').siblings('.treeIcon').click();
                    scrollToActiveItem()    
                }
                , 500);
            
        }else if(currentUri.filename().toLowerCase().indexOf('imagetab.aspx')>=0){
            $('.nav-image-highlighter').addClass('active-item').closest('li').siblings().find('.active-item').removeClass('active-item');
            setTimeout(scrollToActiveItem(), 500);
        }

    }

    function _bindDrugLinkHoverEvent() {
        //do UI bindings
        //init yakuri content populating for the BS popover when hovered
        $('.onepagestructure-container').on('mouseenter', s.yakuriPopoverHandlerClassName, function () {
            $(this).trigger(s.customEvent.ePopoverYakuri);
        });

        $('.onepagestructure-container').on(s.customEvent.ePopoverYakuri, function(){
            var drugJPC = $(event.target).attr('data-drug-jpc');
            var lastTimeStampInvoked = u.TimeStamp_YakuriTable;
            var currentTimeStamp = Date.now() / 1000; //in seconds

            //set time out to avoid rapid ajax call
            if (currentTimeStamp - lastTimeStampInvoked > 2) {
                u.TimeStamp_YakuriTable = currentTimeStamp;
                OPMWidget.populateYakuriTable(drugJPC, $(event.target));

            }
        });

    }

    function _initYakuriTablePopover($targetDrug) {
        var isMouseOnDrugYakuri = false;
        // https://stackoverflow.com/questions/13202762/html-inside-twitter-bootstrap-popover
        //init Bootstrap popover binding
        $targetDrug.popover({
            html: true,
            trigger: 'manual',
            content: function () {
                return $("#yakuri-table").html();
            },
            title: function () {
                var drugName = $(this).html();
                var drugURL = $(this).attr('href');
                return '<a href="' + drugURL +'"">＞＞' + drugName + 'の薬剤詳細ページを見る<span class="popover-dismiss-yakuri">&#10006;</span></a>';
            }
        }).on('shown.bs.popover', function () {

            //init dismiss popover event when X was clicked or when "ESC" pressed
            var $popup = $(this);
            $(this).next('.popover').find('.popover-dismiss-yakuri').click(function (e) {
                $popup.popover('destroy');
            });

            $(document).keyup(function(e) {
                 if (e.keyCode == 27) { // escape key maps to keycode `27`
                    $popup.popover('destroy');
                }
            });

            $popup.next('.popover').mouseleave(function () {
                //mouse leave yakuri table
                $popup.popover('destroy');
            });
            
            $targetDrug.mouseleave(function () {
                //check if current cursor is on popover yakuri table first
                if (! $popup.next('.popover').is(':hover')) {
                    $popup.popover('destroy');
                }
            });


            //disable other popover when hover other drug link
            $('.onepagestructure-container').off(s.customEvent.ePopoverYakuri);
        }).on('hidden.bs.popover', function () {
            //init yakuri content populating for the BS popover when hovered
            _bindDrugLinkHoverEvent();
        });

        $targetDrug.popover('show');
    }

    function _initNoteExpand(){
        $('.onepage-note-expand').click(function (e){
            e.preventDefault();
            $('.onepage-note-full').show();
            $('.onepage-note-short').hide();
            $('#phDiseaseNotes-placeholder').siblings().hide();
            var currentTitle = $('.titlecol .titlecoltext').text();
            $('.titlecol .titlecoltext').text(currentTitle + '：施設コンテンツ');
            s.isNoteExpanded = true;
        });
        $('.onepage-note-back').click(function (e){
            e.preventDefault();
            $(this).trigger(s.customEvent.eHiddenDiseaseNote);
        });


        $(window).on(s.customEvent.eHiddenDiseaseNote, function(){
            $('.onepage-note-full').hide();
            $('.onepage-note-short').show();
            $('#phDiseaseNotes-placeholder').siblings().show();

            var currentTitle = $('.titlecol .titlecoltext').text();
            $('.titlecol .titlecoltext').text(currentTitle.replace('：施設コンテンツ','') );
			
            s.isNoteExpanded = false;
        });
    }
    function _initReferenceLinkTitle(skipReferenceLinkInit){

        if(skipReferenceLinkInit === false){
            $('.onepage-reference-link').each(function(){
                var journalListRowNum = $(this).html();
                var journalTitle ='';
                if(journalListRowNum.indexOf('-')>=0){
                    var rangeRowNum = journalListRowNum.split('-');
                    var ltRowNum = [];
                    for (var i = rangeRowNum[0]; i <= rangeRowNum[1]; i++) {
                        ltRowNum.push(i);
                    }

                    for (var i = 0; i < ltRowNum.length; i++) {
                       journalTitle += ' [' +ltRowNum[i] + '] ' + $(this).closest('.onepage-reference-maincontent-container')
                            // .find('.onepage-reference-journallist td:contains(' + ltRowNum[i] + ')')
                            .find('.onepage-reference-journallist td.onepage-reference-journallist-rownum').filter(function(){
                                return $(this).text().split(':')[0].trim() == ltRowNum[i];
                            }).siblings('.onepage-reference-journallist-title').text().trim();
                    }

                }else{
                    //get the td with the same row no. as journalListRowNum
                    journalTitle = $(this).closest('.onepage-reference-maincontent-container')
                            // .find('.onepage-reference-journallist td:contains('+journalListRowNum+')')
                            .find('.onepage-reference-journallist td.onepage-reference-journallist-rownum').filter(function(){
                                return $(this).text().split(':')[0].trim() == journalListRowNum;
                            }).siblings('.onepage-reference-journallist-title').text().trim();
                    
                }

                $(this).attr('title', journalTitle);
            });
        }


        // Wrap reference link with <sup> if not wrapped
        // https://stackoverflow.com/questions/10910482/wrap-items-only-if-they-are-not-already-within-a-certain-element
        $.each( ['div', 'li'], function(i, d){
            $( d + ' > .onepage-reference-link-container').each(function(){
                $(this).wrap('<sup />');
            });    
        });
    }

    function _populateHandleBarTemplate(templateFilePath, targetContainer, jsonData) {
        var source;
        var template;

        // need to use promise polyfill for older browser
        return $.ajax({
            url: templateFilePath,
            // cache: true,
            success: function (data) {
                source = data;
                template = Handlebars.compile(source);
                $(targetContainer).html(_encode(template(jsonData)));
            }
        });
    }

    function _initImageListInsertion() {
        //To insert image list between the html tags imported in the database
        var targetImageHolder = $('.convertedLinkTitle').closest('a[href$="ID0011"').closest('div');
        $('#imagelist-holder').insertBefore(targetImageHolder);
    }


    return {
        defaultSettings: {
            isNoteExpanded: false,
            yakuriPopoverHandlerClassName: 'a.bs-popover-yakuri',
            yakuriContentClassName: '.bs-popover-yakuri-content',
            customEvent: {
                ePopoverYakuri:'OnePageStructure.PopoverYakuri',
                eScrollOnePageStructure:'OnePageStructure.MouseScroll',
                eHiddenDiseaseNote:'Hidden.OnePageStructure.DiseaseNote',
                eShownDiseaseNote:'Shown.OnePageStructure.DiseaseNote',
            },
        },

        init: function (options) {
            //// this.defaultSettings = $.extend({}, this.defaultSettings, options);
            s = this.defaultSettings;
            u = options;

            if(u.IsPrintPage){
                _initReferenceLinkTitle(true);

            }else{
                _initAnchorLink();
                _bindDrugLinkHoverEvent();
                _initReferenceLinkTitle(false);
                _initNoteExpand();
                _updatePosition_OPM();
                _initPrescriptionTitleAnchor();

                // hotfix for unrealiable offset when scroll to the first .header_ancher
                var currentLocationHash = _encode(window.location.hash);
                if(currentLocationHash.replace('#', '') === $('#MainContents04 a.header_ancher:first').attr('id')
                    || currentLocationHash.replace('#', '') === $('#MainContents05 a.header_ancher:first').attr('id')){
                    //first header_ancher of Action-OnePageStructure.ascx or Evidence-OnePageStructure.ascx got a different offset with their siblings when visited directly
                    //temp workaround to scroll to the related section if user visited with the anchor hash
                    console.log(currentLocationHash +' - '+ 'true');
                    setTimeout(function () {
                        scrollToDiv(currentLocationHash);
                    }, 500);
                }
                
                OPMWidget.initScrollEvent(true);
                
                // It's ok to display image thumbnail after the paragraph contents
                // _initImageListInsertion();
            }

        },

        populateYakuriTable: function (drugJPC, $targetDrug) {
            if (drugJPC !== '') {
                var param = { drugJPC: drugJPC }
                var ret = '';
                $.ajax({
                    cache: false,
                    type: "POST",
                    url: u.URL_DisplayYakuriTable,
                    data: JSON.stringify(param),
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    success: function (response) {
                        if (!response.d.error) {
                            var promiseHandlebarPopulated = _populateHandleBarTemplate(u.Template_YakuriTable, u.Frame_YakuriTable, response.d);
                            promiseHandlebarPopulated.done(function () {
                                _initYakuriTablePopover($targetDrug);
                            });
                        }

                    },
                    error: function (response) {
                        console.log(response);
                    },
                });
            }
        },

        initScrollEvent : function (enableScrollEvent) {
            if (enableScrollEvent === true) {
                // https://stackoverflow.com/questions/9979827/change-active-menu-item-on-page-scroll
                // Cache selectors
                var scrollableSections = $('a[id^=ID], p[id^=ID], a[id=TOP], a[id=DiseaseAction], a[id=DiseaseEvidence], a[id=GUIDELINE], a[id=CASESEARCH], a[id=REFERENCE], a[id=EXTLINKS]');

                //Bind to custom event upon scrolling, 
                //Note : the event 'scroll' also used in jquery.lazyload

                // set a delay timer to minimize performance impact
                // https://stackoverflow.com/questions/7392058/more-efficient-way-to-handle-window-scroll-functions-in-jquery

                var scrollTimer = null;
                $(window).scroll(function () {
                    if (scrollTimer) {
                        clearTimeout(scrollTimer);   // clear any previous pending timer
                    }
                    scrollTimer = setTimeout(function(){
                        $(window).trigger(s.customEvent.eScrollOnePageStructure);
                    }, 150);   // set new timer
                });


                $(window).on(s.customEvent.eScrollOnePageStructure,function(){
                    /// Get container scroll pos
                    var fromTop = $(this).scrollTop() + 106;

                    // Get id of current scroll item
                    var cur = scrollableSections.map(function () {
                        if ($(this).offset().top < fromTop) {
                            return this;
                        }
                    });
                    if (cur.length > 0) {
                        //Get the id of the current element
                        cur = cur[cur.length - 1];
                        var id = cur.id;

                        //Set/remove active class
                        if ($.trim(id) !== '') {
                            _reSyncLeftMenu(id);
                            // change hash without snapping into related section
                            changeHash(g_currentPageWithParam, '#' + id);
                        }
                    }
                });

            } else {
                $(window).off(s.customEvent.eScrollOnePageStructure);
            }
        }
    }
}());


function _encode(str) {
    const el = document.createElement('div');
    el.textContent = str;
    var res = el.innerHTML;
    res = res.replaceAll("&lt;", "<");
    res = res.replaceAll("&gt;", ">");
    res = res.replaceAll("&amp;", "&");
    return res;
}