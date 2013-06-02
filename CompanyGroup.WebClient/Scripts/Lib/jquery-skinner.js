/************************************************************************
*************************************************************************
@Name :		skinner - jQuery Plugin
@Revison :  1.2
@Date :		17/10/2011
@Author:	Andrea (anbi) Bianchin - http://www.andreabianchin.it#projects - http://twitter.com/#!/anbi
@License : 	Open Source - MIT License : http://www.opensource.org/licenses/mit-license.php
*************************************************************************
@Usage : 	allowed parameters
			# type : 	- left > float left (default)
						- right > float right
			 			- block > display block
			# width : 	- auto > (default)
			 			- pixel > e.g. 100px
			 			- percent > e.g. 80%
			# textwrap : - false > text doesn't wrap on dropdown (default)
			 			 - true > text wrap on dropdown
			# maxitem : - 2,3,4,5... "n" > number of item to show (default = 6)
						- false > show all item without scollbar
**************************************************************************
*************************************************************************/

(function($) {
	$.fn.skinner = function(opt){
		var cfg = {'type':'block','width':'auto','textwrap':false,'maxitem':'6'};    
	    if(opt){$.extend(cfg,opt);}
		var element = this;
		var skin = {
			selectskinned: function (element){
				$(element).each(function(){
					sc = $(this);
					sc.wrap('<div class="select-skinned" />');
					childrennn = $('<ul></ul>').hide();
					sc.children('option').each(function(i,e){				
						myText = ($(e).html()=='') ? '&nbsp;' : $(e).html();
						itemLI = $('<li>'+myText+'</li>').click(function(){
							itemUL = $(this).parent('ul');
							itemUL.hide();
							itemUL.next('select').find('option').removeAttr('selected');
							itemUL.next('select').find('option').eq(i).attr('selected','selected');
							testoSelected = (itemUL.next('select').find('option').eq(i).text()=='') ? '&nbsp;' : itemUL.next('select').find('option').eq(i).text();
							itemUL.prev('.select-skinned-text').html('<div class="select-skinned-cont">' + testoSelected + '</div>');
							itemUL.next('select').change();					
						});
						childrennn.append(itemLI);
						childrennn.hover(function(){},function(){ $(this).hide(); });						
						$(document).click(function(e) { $('.select-skinned > ul').hide(); });
						$('.select-skinned').click(function(e) { e.stopPropagation(); });					
					});
							
					testoSel = (sc.children('option:selected').text()=='') ? '&nbsp;' : sc.children('option:selected').text();			
					selectedItem = $('<div class="select-skinned-text"><div class="select-skinned-cont">'+testoSel+'</div></div>');
					selectedItem.click(function(){ 
						elemUL = $(this).next('ul');						
						pos = $(this).parent('.select-skinned').position();
						bodyScroll = $('body').scrollTop();
						if($(window).height()<=(pos.top+elemUL.outerHeight()+15-bodyScroll)){
							elemUL.css({'top':'auto','bottom':'0'})
						}else{
							elemUL.css({'top':$(this).prev('.select-skinned-text').children('.select-skinned-cont').height(),'bottom':'auto'});				
						}
						if($(window).width()<=(pos.left+elemUL.outerWidth())){
							elemUL.css({'left':'auto','right':'0'})
						}else{
							elemUL.css({'left':'0','right':'auto'})
						}
						if(elemUL.is(':visible')){ elemUL.hide(); }else{ elemUL.show(); }
						if(cfg.maxitem){
							hTotItem = 0;
							for(i=0;i<cfg.maxitem;i++){
								hTotItem += elemUL.children('li:eq('+i+')').outerHeight();
							}
							elemUL.css({'overflow-y':'scroll','height':hTotItem+'px'});	
						}					
					});
					sc.before(selectedItem);
					sc.before(childrennn);						
					sc.change(function(){ skin.checkSelect(element); });
					sc.hide();					
				});
				skin.addStyle();
				
			},
			
			checkSelect : function(elem){
				$(elem).each(function(){
					e = $(this);
					e.prev('ul').html('');
					e.children('option').each(function(i,el){
						mytext = ($(el).html()=='') ? '&nbsp;' : $(el).html();
						itemLI = $('<li>'+mytext+'</li>').click(function(){
							itemUL = $(this).parent('ul');
							itemUL.hide();
							itemUL.next('select').find('option').removeAttr('selected');
							itemUL.next('select').find('option').eq(i).attr('selected','selected');
							itemUL.prev('.select-skinned-text').html('<div class="select-skinned-cont">'+itemUL.next('select').find('option').eq(i).text()+'</div>');
							itemUL.next('select').change();
						});
						e.prev('ul').append(itemLI);						
					});	
				});				
				skin.addStyle();
			},
			
			resetSelect : function(e){
				if($(e).prev('ul').size()!=0){
					$(e).prev('ul').prev('.select-skinned-text').html('<div class="select-skinned-cont">' + $(e).find('option').eq(0).text() + '</div>');
				}
			},
			
			addStyle : function(){
				if(cfg.type!='block'){ $(element).parent('.select-skinned').css({'float':cfg.type}); }							
				$(element).parent('.select-skinned').width(cfg.width);
				if(cfg.textwrap){ $(element).parent('.select-skinned').children('ul').children('li').css({'white-space':'normal'}); }
				$('.select-skinned > ul > li').hover( function(){$(this).attr({'class':'hover'});}, function(){$(this).attr({'class':''});} );
			}
		}		
		skin.selectskinned(element);		
		return this;	
	}
})(jQuery);