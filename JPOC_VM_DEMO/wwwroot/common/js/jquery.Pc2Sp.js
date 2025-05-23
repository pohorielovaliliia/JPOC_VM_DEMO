/*
 * jquery.Pc2Sp.js ver1.00
 * Copyright Kazuma Nishihata
 * MIT license
 */
;(function($){
	
	$.switchPc2Sp = function(options){
		
		options = $.extend({
			mode : "sp" , //sp or pc
			spLinkBlockInPc : "#spLinkBlockInPc" , 
			anchorToPcInSp : "#anchorToPcInSp" ,
			anchorToSpInPc : "#anchorToSpInPc" 
		},options);
		
		if(!localStorage)return this;
		
		$(function(){
			if( options.mode=="sp" 
				&& localStorage.getItem("pc_flag")=="true"
				&& /(Android|iPhone|iPod)/.test(navigator.userAgent)){
				
                if ($(options.anchorToPcInSp).attr("href")) {
                    location.href = $(options.anchorToPcInSp).attr("href");
                }
				
			}else if( options.mode=="pc" 
				&& localStorage.getItem("pc_flag")!="true"
				&& /(Android|iPhone|iPod|iPod)/.test(navigator.userAgent)) {
				
                if ($(options.anchorToSpInPc).attr("href")) {
                    location.href = $(options.anchorToSpInPc).attr("href");
                }
				
			}else if( options.mode=="sp" 
				&& !/(Android|iPhone|iPod)/.test(navigator.userAgent)){
				
                if ($(options.anchorToPcInSp).attr("href")) {
                    location.href = $(options.anchorToPcInSp).attr("href");
                }
				
			}else if( options.mode=="pc" 
				&& localStorage.getItem("pc_flag")=="true"
				&& /Android|iPhone|iPod/.test(navigator.userAgent)){
				
				$(options.spLinkBlockInPc).show();
				
			}
			
			if( options.mode=="sp"){
				
				$(options.anchorToPcInSp).click(function(){
					localStorage.setItem("pc_flag","true")
				});
				
			}else if( options.mode=="pc" && /Android|iPhone|iPod/.test(navigator.userAgent)){
				
				$(options.anchorToSpInPc).click(function(){
					localStorage.setItem("pc_flag","false")
				});
				
			}
		});
		
		return this;
	}

    // Footer?p?ɒǉ?
	$.switchPc2SpFooter = function(options){
		
		options = $.extend({
			mode : "sp" , //sp or pc
			spLinkBlockInPcFooter : "#spLinkBlockInPcFooter" , 
			anchorToPcInSpFooter : "#anchorToPcInSpFooter" ,
			anchorToSpInPcFooter : "#anchorToSpInPcFooter" 
		},options);
		
		if(!localStorage)return this;
		
		$(function(){

			if( options.mode=="sp" 
				&& localStorage.getItem("pc_flag")=="true"
				&& /(Android|iPhone|iPod)/.test(navigator.userAgent)){
				
                if ($(options.anchorToPcInSpFooter).attr("href")) {
                    location.href = $(options.anchorToPcInSpFooter).attr("href");
                }
				
			}else if( options.mode=="pc" 
				&& localStorage.getItem("pc_flag")!="true"
				&& /(Android|iPhone|iPod|iPod)/.test(navigator.userAgent)) {
				
                if ($(options.anchorToSpInPcFooter).attr("href")) {
                    location.href = $(options.anchorToSpInPcFooter).attr("href");
                }
				
			}else if( options.mode=="sp" 
				&& !/(Android|iPhone|iPod)/.test(navigator.userAgent)){
				
                if ($(options.anchorToPcInSpFooter).attr("href")) {
                    location.href = $(options.anchorToPcInSpFooter).attr("href");
                }
				
			}else if( options.mode=="pc" 
				&& localStorage.getItem("pc_flag")=="true"
				&& /Android|iPhone|iPod/.test(navigator.userAgent)){
				
				$(options.spLinkBlockInPcFooter).show();
				
			}else if( options.mode=="pc" 
				//&& localStorage.getItem("pc_flag")=="true"
				&& /iPad/.test(navigator.userAgent)){
				
				$(options.spLinkBlockInPcFooter).show();
				
			}
			
			if( options.mode=="sp"){
				
				$(options.anchorToPcInSpFooter).click(function(){
					localStorage.setItem("pc_flag","true")
				});
				
			}else if( options.mode=="pc" && /Android|iPhone|iPod|iPad/.test(navigator.userAgent)){
				
				$(options.anchorToSpInPcFooter).click(function(){
					localStorage.setItem("pc_flag","false")
				});
				
			}
		});
		
		return this;
	}

})(jQuery);