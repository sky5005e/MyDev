var Uploader = new Object();
Uploader.version = '6.3.1';
Uploader.iconsURL = 'images/icons/';

var userAgent = navigator.userAgent.toLowerCase();
var is_ie = userAgent.indexOf('msie') > -1;
var is_opera = userAgent.indexOf('opera') > -1;

function rand(){return String(Math.floor(Math.random()*10000000000));}
function rand2(){return rand()+rand()+rand()+rand()+rand();}
function getObj(sId){return document.getElementById(sId);}
function checkIt(b,c){if(b){b.checked=!c;b.click();b.checked=c;}}
function showIt(sId,v){var x=$(sId);if(x)x.style.display=(v?'block':'none');}
function togView(sId){var el=$(sId);if(el){el.style.display=el.style.display=='none'?'':'none';}}
function checkAllBoxes(form,fieldName,chkVal){formObj=$(form);if(!formObj)return false;var checkBoxes=formObj.getElementsByTagName('input');for(var i=0;i<checkBoxes.length;i++){if(checkBoxes[i].name==fieldName&&!checkBoxes[i].disabled){checkBoxes[i].checked=chkVal;}}}
function countCheckedBoxes(form,fieldName){formObj=$(form);if(!formObj)return false;var checkBoxes=formObj.getElementsByTagName('input');var checkedBoxes=0;for(var i=0;i<checkBoxes.length;i++){checkedBoxes+=(checkBoxes[i].name==fieldName&&!checkBoxes[i].disabled&&checkBoxes[i].checked)?1:0;}return checkedBoxes;}
function pInt(oF){if(oF){oF.value=parseInt(oF.value);if(oF.value=='NaN'){oF.value=0;}}}
function pop(sURL){return!window.open(sURL);}
function go(url){if(url.substr(0,7)!='http://')url=base_url+url;window.location=url;return false;}
function htmlspecialchars(s){var t={'&':'&amp;','"':'&quot;',"'":'&#039;','<':'&lt;','>':'&gt;'};for(var k in t){rx=new RegExp(k,'g');s=s.replace(rx,t[k]);}return s;}
function addslashes(s){s=s.replace(/'/gi,"&#039;");s=s.replace(/"/gi,"&quot;");return s;}
function alternateRowColor(obj,el,c1,c2){if(obj){var els=obj.getElementsByTagName(el);var j=0;for(var i=0;i<els.length;i++)if(!els[i].getAttribute('skip_alternate')){els[i].style.backgroundColor=(j&1?c2:c1);j++;}}}
function basename(s){var p=-1;for(var i=0;i<s.length;i++){if( s.charAt(i)=='\\'||s.charAt(i)=='/')p=i;}if(p<0)return s;return s.substr(p+1,s.length-p);}
function dirname(s){var p=-1;for(var i=0;i<s.length;i++){if( s.charAt(i)=='\\'||s.charAt(i)=='/')p=i;}if(p<0)return s;return s.substr(0,p+1);}
function togImage(im,n){if(!im)return false;var alt=im.getAttribute('alternateImage');if(!alt)alt=dirname(im.src)+'/'+n;var cur=im.src;im.src=alt;im.setAttribute('alternateImage',cur);}
function get_extension(n){n=n.substr(n.lastIndexOf('.')+1);return n.toLowerCase();}
function str_slice(s,l){if(s.length>l)return s.substr(0,l/2)+'...'+s.substr(s.length-(l/2));return s;}
function str_preview(s,l){return s.length>l?s.substr(0,l)+'...':s;}
function setStyles(obj,styles){if(!obj)return false;for(var k in styles)obj.style[k]=styles[k];}
function number_format(n){n+='';x=n.split('.');x1=x[0];x2=x.length>1?'.'+x[1]:'';var rgx=/(\d+)(\d{3})/;while(rgx.test(x1)){x1 = x1.replace(rgx,'$1'+','+'$2');}return x1+x2;}
function setParentSize(i,s){if(!s)s=2;i.parentNode.parentNode.style.width=i.width+s+'px';i.parentNode.parentNode.style.height=i.height+s+'px';}
function set_border(i,p,s){if(typeof s=='undefined')s=2;p.style.width=i.width+s+'px';p.style.height=i.height+s+'px';}
function trim(v,p){var t=Math.pow(10,p);return Math.round(v*t)/t;}
function get_size(v,u){if(!u)u='B';if(v>1024){if(u=='B')return get_size(v/1024,'KB');if(u=='KB')return get_size(v/1024,'MB');if(u=='MB')return get_size(v/1024,'GB');if(u=='GB')return get_size(v/1024,'TB');}return trim(v,0)+'&nbsp;'+u;}
function deleteNode(n){return n.parentNode.removeChild(n);}
function confirmGo(s,url){if(confirm(s))go(url);return false;}
function str_replace(from,to,str){while(str!=str.replace(from,to))str=str.replace(from,to);return str;}

