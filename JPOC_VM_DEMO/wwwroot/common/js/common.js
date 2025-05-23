/* コンテンツ内容に依存しない汎用的なスクリプトを記述                 */
/* 原則としてIDやクラスを指定した操作を伴うものはここに記述しないこと */

// onload
window.onload = function () {
    disableRightClickOnImage();
    // GCの明示的呼出
    if (window.CollectGarbage) {
        window.CollectGarbage();
    }
}

// onloadイベントを追加する。   
function addOnload(func) {
    try {
        window.addEventListener("load", func, false);
    } catch (e) {
        // IE用   
        window.attachEvent("onload", func);
    }
}

//以降、JQuery外でonloadイベント実行したい場合は以下のように記述
//addOnload(function() { alert("1"); });   

window.onpageshow = function () {};

// onpageshowイベントを追加する。   
function addOnPageShow(func) {
    try {
        window.addEventListener("pageshow", func, false);
    } catch (e) {
        // IE用?   
        window.attachEvent("onpageshow", func);
    }
}

//以降、JQuery外でpageshowイベント実行したい場合は以下のように記述
//addOnPageShow(function() { alert("1"); });   

window.onunload = function () { };

// onunloadイベントを追加する。   
function addOnUnLoad(func) {
    try {
        window.addEventListener("unload", func, false);
    } catch (e) {
        // IE用?   
        window.attachEvent("onunload", func);
    }
}

//以降、JQuery外でonunloadイベント実行したい場合は以下のように記述
//addOnUnLoad(function() { alert("1"); });   

// 画像上での右クリック無効化(「input type='image'」は無効化しない)
function disableRightClickOnImage() {
    for (var i = 0; i < document.getElementsByTagName("img").length; i++) {
        document.getElementsByTagName("img")[i].oncontextmenu = function () {
            return false;
        }
        document.getElementsByTagName("img")[i].onmousedown = function () {
            return false;
        }
    }
}

function getUserAgentInfo() {
    var ua = new Object();
    ua.Type = 'unknown';
    ua.Name = 'unknown';
    try {
        var userAgent = window.navigator.userAgent.toLowerCase();
        var appVersion = window.navigator.appVersion.toLowerCase();
        if (userAgent.indexOf('msie') != -1) {
            ua.Type = 'ie';
            if (appVersion.indexOf('msie 6.') != -1) {
                ua.Name = 'ie6';
            } else if (appVersion.indexOf('msie 7.') != -1) {
                ua.Name = 'ie7';
            } else if (appVersion.indexOf('msie 8.') != -1) {
                ua.Name = 'ie8';
            } else if (appVersion.indexOf('msie 9.') != -1) {
                ua.Name = 'ie9';
            } else if (appVersion.indexOf('msie 10.') != -1) {
                ua.Name = 'ie10';
            }
        } else if (userAgent.indexOf('chrome') != -1) {
            ua.Type = 'chrome';
            ua.Name = 'chrome';
        } else if (userAgent.indexOf('ipad') != -1) {
            ua.Type = 'ipad';
            ua.Name = 'ipad';
        } else if (userAgent.indexOf('ipod') != -1) {
            ua.Type = 'ipod';
            ua.Name = 'ipod';
        } else if (userAgent.indexOf('iphone') != -1) {
            ua.Type = 'iphone';
            var ios = (navigator.appVersion).match(/OS (\d+)_(\d+)_?(\d+)?/);
            ua.Name = [parseInt(ios[1], 10), parseInt(ios[2], 10), parseInt(ios[3] || 0, 10)];
        } else if (userAgent.indexOf('safari') != -1) {
            ua.Type = 'safari';
            ua.Name = 'safari';
        } else if (userAgent.indexOf('gecko') != -1) {
            ua.Type = 'gecko';
            ua.Name = 'gecko';
        } else if (userAgent.indexOf('opera') != -1) {
            ua.Type = 'opera';
            ua.Name = 'opera';
        } else if (userAgent.indexOf('android') != -1) {
            ua.Type = 'android';
            ua.Name = 'android';
        } else if (userAgent.indexOf('mobile') != -1) {
            ua.Type = 'mobile';
            ua.Name = 'mobile';
        };
    } catch (e) {
        ua.Type = 'unknown';
        ua.Name = 'unknown';
    }
    return ua;
}

function isFlashInstalled() {
    if (navigator.plugins["Shockwave Flash"]) {
        return true;
    }
    try {
        new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
        return true;
    } catch (a) {
        return false;
    }
}

function perform_acrobat_detection() {
    //
    // The returned object
    // 
    var browser_info = {
        name: null,
        acrobat: null,
        acrobat_ver: null
    };

    if (navigator && (navigator.userAgent.toLowerCase()).indexOf("chrome") > -1) {
        browser_info.name = "chrome";
    } else if (navigator && (navigator.userAgent.toLowerCase()).indexOf("msie") > -1) {
        browser_info.name = "ie";
    } else if (navigator && (navigator.userAgent.toLowerCase()).indexOf("firefox") > -1) {
        browser_info.name = "firefox";
    } else if (navigator && (navigator.userAgent.toLowerCase()).indexOf("msie") > -1) {
        browser_info.name = "other";
    }

    try {
        if (browser_info.name == "ie") {
            var control = null;
            try {
                // AcroPDF.PDF is used by version 7 and later
                control = new ActiveXObject('AcroPDF.PDF');
            } catch (e) {
                // no do.
            }
            if (!control) {
                try {
                    // PDF.PdfCtrl is used by version 6 and earlier
                    control = new ActiveXObject('PDF.PdfCtrl');
                } catch (e) {
                    // no do.
                }
            }
            if (!control) {
                browser_info.acrobat == null;
                return browser_info;
            }
            var version = control.GetVersions().split(',');
            version = version[0].split('=');
            browser_info.acrobat = "installed";
            browser_info.acrobat_ver = parseFloat(version[1]);
        } else if (browser_info.name == "chrome") {
            for (key in navigator.plugins) {
                if (navigator.plugins[key].name == "Chrome PDF Viewer" || navigator.plugins[key].name == "Adobe Acrobat") {
                    browser_info.acrobat = "installed";
                    browser_info.acrobat_ver = parseInt(navigator.plugins[key].version) || "Chome PDF Viewer";
                }
            }
        } else if (navigator.plugins != null) {
            //
            // NS3+, Opera3+, IE5+ Mac, Safari (support plugin array):  check for Acrobat plugin in plugin array
            //    
            var acrobat = navigator.plugins['Adobe Acrobat'];
            if (acrobat == null) {
                browser_info.acrobat = null;
                return browser_info;
            }
            browser_info.acrobat = "installed";
            browser_info.acrobat_ver = parseInt(acrobat.version[0]);
        }
    } catch (e) {
        browser_info.acrobat_ver = null;
    }
    return browser_info;
}

// queryStringの文字列を連想配列で取得する
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(_encode(hash[0]));
        vars[_encode(hash[0])] = _encode(hash[1]);
    }
    return vars;
}
// 現在のページ名を取得する
function getCurrentPageName(isExcludeQueryString) {
    var pageName = location.href.substring(location.href.lastIndexOf("/") + 1, location.href.length);
    if (isExcludeQueryString) {
        pageName = pageName.substring(0, pageName.indexOf("?"));
    }
    return pageName
}





// For quotation request JPOC-256
var QuotationWidget = (function () {
    var s; // private alias to defaultSettings
    var u; // private alias to Config_Quotation
    var clearFilter = false;

    function _initControl(){
        $('#hypRequestQuotation').click(function(e){
            // return false when institutoin naem is not selected to ensure the single quotation returned
            if($('#ddlInstitutionName').val() ===''){
                alert('県名、市町村名、病院を選択してください。');
                return false;
            }
            //JPOC-524
            if ($('#QuotationDate').val() === '') {
                alert('利用開始月を選択してください。');
                return false;
            }
        });

        $('#chkDelegate').change(function(){
            _toggleDelegateContactForm(this.checked);
        });

    }

    function _toggleDelegateContactForm(enableSubmission){
        // enable all the fields except placeholder for hospital selected if chkDelegate checked
        $('#hidQuotationDelegateSubmission').val(enableSubmission);
        $('.contact-form-delegate .form-control')
            .not('.institution-name-placeholder')
            .val('')
            .prop('disabled', !enableSubmission);

        $('#txtInstitutionNameDelegate').val($('#txtInstitutionName').val());

        if(typeof ValidatorEnable !=='undefined'){
            ValidatorEnable(rfvInstitutionNameDelegate, enableSubmission);
            ValidatorEnable(rfvDepartmentDelegate, enableSubmission);
            ValidatorEnable(rfvNameDelegate, enableSubmission);
            ValidatorEnable(rfvPhoneNumberDelegate, enableSubmission);
            ValidatorEnable(revPhoneNumberDelegate, enableSubmission);
            ValidatorEnable(rfvEmailDelegate, enableSubmission);
            ValidatorEnable(revEmailDelegate, enableSubmission);

        }
    }
    function _formatCurrency(){
        $('.currency').each(function(){
            // var origPriceString = parseInt($(this).html());

            $(this).html(
                parseInt($(this).html()).toLocaleString());
        });
    }
    
    return {
        defaultSettings: {
            quotationHandlerClassName: 'a#hypRequestQuotation',
            customEvent: {
                eShowQuotation:'JPOC.ShowQuotation'
            }
        },

        init: function (options) {
            // this.defaultSettings = $.extend({}, this.defaultSettings, options);
            s = this.defaultSettings;
            u = options;

            _initControl();
            // _formatCurrency();
            _toggleDelegateContactForm(false);
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
String.prototype.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find, 'g'), replace);
};


