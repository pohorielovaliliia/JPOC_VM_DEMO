
$(document).ready(function() {
	imgRollover();
	smoothScroll();
	countBg();
});

$(window).load(function() {
});




/**********************************************************
* imgRollover */

function imgRollover() {
	$('a img.rollover, input.rollover').not('[src*="_on."],[src*="_over."]').each(function(){
		var img = $(this);
		var src = img.attr('src');
		var src_on = src.substr(0, src.lastIndexOf('.')) + '_over' + src.substring(src.lastIndexOf('.'));
		$('<img>').attr('src', src_on);
		img.hover(function(){
			img.attr('src', src_on);
		},function(){
			img.attr('src', src);
		});
	});
}




/**********************************************************
* imgFade */

function imgFade() {
	$('a img').not('.rollover').each(function(){
		$(this).fadeTo(0, 1.0);
		$(this).hover(function(){
			$(this).stop().fadeTo(200, 0.7);
		},function(){
			$(this).stop().fadeTo(200, 1.0);
		});
	});
}



/**********************************************************
* smoothScroll */

function smoothScroll() {
	$('a[href^=#]').not('[href$=#], .no').click(function(){
		var win = $(window).height();
		var all = $(document).height();
		var HashOffset = $(this.hash).offset().top;
		if (HashOffset < 0) {
			HashOffset = 0;
		} else if (HashOffset > all-win) {
			HashOffset = all-win;
		}
		$('html,body').animate({scrollTop:HashOffset}, 800, 'slowdown');
		return false;
	});
}
$.easing.slowdown = function(x,t,b,c,d){
    return -c * ((t=t/d-1)*t*t*t - 1) + b;
}




/**********************************************************
* objHeight */

$.fn.objHeight = function(columns) {
	var tiles, max, c, h, last = this.length - 1, s;
	if(!columns) columns = this.length;
	this.each(function() {
		s = this.style;
		if(s.removeProperty) s.removeProperty("height");
		if(s.removeAttribute) s.removeAttribute("height");
	});
	return this.each(function(i) {
		c = i % columns;
		if(c == 0) tiles = [];
		tiles[c] = $(this);
		h = tiles[c].height();
		if(c == 0 || h > max) max = h;
		if(i == last || c == columns - 1)
			$.each(tiles, function() { this.height(max); });
	});
}
/*
$(obj).objHeight(2);
*/




/**********************************************************
* countBg */

function countBg() {
	var element = $('table.countBg');
	element.each(function(){
		$(this).find('tr:even').addClass('bg1');
		$(this).find('tr:odd').addClass('bg2');
	});
}




/**********************************************************
* openWindow */

function openWindow(winURL,winName,winW,winH) {
	var winFeatures = 'toolbar=no,resizable=no,menubar=no,directories=no,scrollbars=yes,status=no,location=no,width=' + winW + ',height=' + winH + '';
	window.subwin = window.open(winURL,winName,winFeatures);
	window.subwin.focus();
}
