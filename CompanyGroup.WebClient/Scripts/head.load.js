/**
 	Head JS		The only script in your <HEAD>
	Copyright	Tero Piirainen (tipiirai)
	License 		MIT / http://bit.ly/mit-license
	
	http://headjs.com
*/(function(a){var b=a.documentElement,c=navigator.userAgent.toLowerCase().indexOf("msie")!=-1,d=false,e=[],f={},g={};var h=window.head_conf&&head_conf.head||"head",i=window[h]=window[h]||function(){i.ready.apply(null,arguments)};i.js=function(){var a=arguments,b=[].slice.call(a,1),c=b[0];if(!d){e.push(function(){i.js.apply(null,a)});return i}c?(m(c)||l(b,function(a){m(a)||o(k(a))}),p(k(a[0]),m(c)?c:function(){i.js.apply(null,b)})):p(k(a[0]));return i},i.ready=function(a,b){var c=g[a];if(c&&c.state=="loaded"){b.call();return i}m(a)&&(b=a,a="ALL");var d=f[a];d?d.push(b):d=f[a]=[b];return i};function j(a){var b=a.split("/"),c=b[b.length-1],d=c.indexOf("?");return d!=-1?c.substring(0,d):c}function k(a){var b;if(typeof a=="object")for(var c in a)a[c]&&(b={name:c,url:a[c]});else b={name:j(a),url:a};var d=g[b.name];if(d)return d;g[b.name]=b;return b}function l(a,b){if(a){typeof a=="object"&&(a=[].slice.call(a));for(var c=0;c<a.length;c++)b.call(a,a[c],c)}}function m(a){return Object.prototype.toString.call(a)=="[object Function]"}function n(a){a.state="preloaded",l(a.onpreload,function(a){a.call()})}function o(c,d){if(!c.state){c.state="preloading",c.onpreload=[];if(/Firefox/.test(navigator.userAgent)){var e=a.createElement("object");e.data=c.url,e.width=0,e.height=0,e.onload=function(){n(c),setTimeout(function(){b.removeChild(e)},1)},b.appendChild(e)}else q({src:c.url,type:"cache"},function(){n(c)})}}function p(a,b){if(a.state=="loaded")return b();if(a.state=="preloading")return a.onpreload.push(function(){p(a,b)});a.state="loading",q(a.url,function(){a.state="loaded",b&&b.call(),l(f[a.name],function(a){a.call()});var c=true;for(var d in g)g[d].state!="loaded"&&(c=false);c&&l(f.ALL,function(a){a.done||a.call(),a.done=true})})}function q(c,d){var e=a.createElement("script");e.type="text/"+(c.type||"javascript"),e.src=c.src||c,e.onreadystatechange=e.onload=function(){var a=e.readyState;!d.done&&(!a||/loaded|complete/.test(a))&&(d.call(),d.done=true)},b.appendChild(e)}setTimeout(function(){d=true,l(e,function(a){a.call()})},200)})(document)