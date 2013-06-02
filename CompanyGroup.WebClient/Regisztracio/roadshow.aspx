<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="lunchandlearn_Register" %>
<html>
<head>
<title>Regisztr&aacute;ci&oacute;</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-2">
<script type="text/javascript" src="../Scripts/Lib/jquery-1.7.1.js"></script>
<script type="text/javascript" src="../Scripts/Lib/jquery.json-2.2.min.js"></script>
<script src="SpryAssets/SpryValidationTextField.js" type="text/javascript"></script><script src="SpryAssets/SpryTabbedPanels.js" type="text/javascript"></script>
<script type="text/javascript"> 
    function addRegistration() {
	    var eventRegistration = {
	        EventId: $("#eventId").val(),
	        EventName: $("#eventName").val(),
	        Data: {
	            PersonName: $("#neve").val(),
	            CompanyName: $("#cegnev").val(),
	            Phone: $("#telefon").val(),
	            Email: $("#email").val(), 
                Place: $("#menu").val()
	        }
	    };
	    $.ajax({
            type: "POST",
            url: "../api/EventRegistrationApi/AddNew",
            data: JSON.stringify(eventRegistration),
            contentType: "application/json; charset=utf-8",
            timeout: 0,
            dataType: "json",
            processData: true,
            success: function (response) {
                if (response) {
                    //alert('A regisztráció sikeresen megtörtént.');
                    window.location = "roadshow_ok.aspx";
                }
                else {
                    switch (ret) {
                        case -1:
                            {
                                alert('Az üzenet elküldése nem sikerült!');
                                break;
                            }
                        case -2:
                            {
                                alert('A levélküldés nem sikerült!');
                                break;
                            }
                        default:
                            {
                                alert('Hiba történt!');
                                break;
                            }
                    }
                }
            },
            error: function () {
                alert('service call failed.');
            }
        });
    }  
    
    jQuery(document).ready(function() {
        jQuery("#anchRegister").click(function() {
            addRegistration();
        });
    });
</script>
<script type="text/javascript">
function MM_validateForm() { //v4.0
  if (document.getElementById){
    var i,p,q,nm,test,num,min,max,errors='',args=MM_validateForm.arguments;
    for (i=0; i<(args.length-2); i+=3) { test=args[i+2]; val=document.getElementById(args[i]);
      if (val) { nm=val.name; if ((val=val.value)!="") {
        if (test.indexOf('isEmail')!=-1) { p=val.indexOf('@');
          if (p<1 || p==(val.length-1)) errors+='- '+nm+' Érvénytelen e-mail cím.\n';
        } else if (test!='R') { num = parseFloat(val);
          if (isNaN(val)) errors+='- '+nm+' must contain a number.\n';
          if (test.indexOf('inRange') != -1) { p=test.indexOf(':');
            min=test.substring(8,p); max=test.substring(p+1);
            if (num<min || max<num) errors+='- '+nm+' must contain a number between '+min+' and '+max+'.\n';
      } } } else if (test.charAt(0) == 'R') errors += '- '+' Kitöltetlen mezők maradtak.\n'; }
    } if (errors) alert('Hiba a kitolteskor!\n'+'- Minden mezo kitoltese kotelezo!');
    document.MM_returnValue = (errors == '');
} }
</script>
<style type="text/css">
#regpanel {
	position:absolute;
	width:698px;
	height:289px;
	z-index:1;
	margin-left: 27px;
	top: 1026px;
	visibility: visible;
}

#helyszinekpanel{
	position:absolute;
	width:698px;
	height:423px;
	z-index:1;
	margin-left: 27px;
	top: 546px;
	padding: 20px;
	visibility: visible;
}

body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	text-align:center;
	background-color: #B4ADA7;
}
.mezok {
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	color: #84878E;
	background-color: #FFF;
	height: 25px;
	width: 280px;
	border: 1px dotted #6D7178;
}
.mezokkisero {
	font-family: Verdana, Geneva, sans-serif;
	font-size: 16px;
	color: #F90;
	background-color: transparent;
	height: 25px;
	width: 70px;
	border-top-style: none;
	border-right-style: none;
	border-bottom-style: none;
	border-left-style: none;
}
#outlook{
	position:absolute;
	width:660px;
	height:180;
	z-index:3;
	margin-left: 30px;
	margin-top: 200px;
}
#Table_01 tr td #regpanel #regform table tr td {
	font-family: Verdana, Geneva, sans-serif;
	font-size: 13px;
	color: #000;
}
</style>
<link href="SpryAssets/SpryValidationTextField_ms.css" rel="stylesheet" type="text/css">
<style type="text/css">
a:link {
	color: #39F;
	text-decoration: none;
}
a:visited {
	text-decoration: none;
	color: #39F;
}
a:hover {
	text-decoration: none;
	color: #39F;
}
a:active {
	text-decoration: none;
	color: #39F;
}
</style>
<link href="SpryAssets/SpryTabbedPanels.css" rel="stylesheet" type="text/css">
</head>
<body topmargin="0" marginheight="0">
<br>
<table width="750"  border="0" align="center" cellpadding="0" cellspacing="0" id="Table_01">
  <tr>
    <td>
    
    <div id="helyszinekpanel">
      <div id="TabbedPanels1" class="TabbedPanels">
        <ul class="TabbedPanelsTabGroup">
          <!--li class="TabbedPanelsTab" tabindex="0">Győr - 05.09.</li-->
          <!--li class="TabbedPanelsTab" tabindex="0">Debrecen - 05.16.</li-->
         <li class="TabbedPanelsTab" tabindex="0">Szeged - 05.23.</li>
          <li class="TabbedPanelsTab" tabindex="0">P&eacute;cs - 05.29.</li>
        </ul>
        <div class="TabbedPanelsContentGroup">
          <!--div class="TabbedPanelsContent">
            <table width="100%" border="0" cellspacing="0" cellpadding="2">
              <tr>
                <td valign="top" nowrap><strong>09:00-09:30&nbsp;&nbsp;</strong></td>
                <td valign="top"><strong>Regisztr&aacute;ci&oacute;</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>09:30-09:40</strong></td>
                <td valign="top"><strong>K&ouml;sz&ouml;ntő &eacute;s a HRP c&eacute;gcsoport bemutat&aacute;sa</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>09:40-10:20</strong></td>
                <td valign="top"><strong>Office 365 &eacute;s Office 2013 <br>
                  </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
                <td valign="top" nowrap>Nagy Levente - Microsoft</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>10:20-10:50</strong></td>
                <td valign="top"><strong>D-Link - A biztons&aacute;g a mi dolgunk!</strong></td>
                <td valign="top" nowrap>Kibertus Viktor - D-Link</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>10:50-11:30</strong></td>
                <td valign="top"><strong>Dell partneri visszat&eacute;r&iacute;t&eacute;sek &eacute;s akci&oacute;k</strong></td>
                <td valign="top" nowrap>Varga Attila, M&oacute;ritz Krisztina - Dell</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>11:30-12:00</strong></td>
                <td valign="top"><strong>K&aacute;v&eacute;sz&uuml;net</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>12:00-12:30</strong></td>
                <td valign="top"><strong>Windows 7 vagy Windows  8?</strong> &nbsp;&nbsp; </td>
                <td valign="top" nowrap>T&oacute;th Andr&aacute;s Mih&aacute;ly &ndash;  Microsoft</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>12:30-13:00</strong></td>
                <td valign="top"><strong>Norton 2013-as &uacute;jdons&aacute;gai PC-n, mobilon &eacute;s tableten Backup Exec.cloud - Ment&eacute;s a felhőbe</strong></td>
                <td valign="top" nowrap>Rakoncza Zsolt  <br>
                  Pulai Andr&aacute;s - Symantec</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>13:00-14:00</strong></td>
                <td valign="top"><strong>Eb&eacute;d</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>14:00-14:30</strong></td>
                <td valign="top"><strong>&Uacute;j Samsung notebook sorozatok - 2013-as modellek</strong></td>
                <td valign="top" nowrap>Kulcs&aacute;r Istv&aacute;n - Samsung</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>14:30-15:00</strong></td>
                <td valign="top"><strong>Polycom - &Uacute;jdons&aacute;gok &eacute;s Partner program</strong></td>
                <td valign="top" nowrap>Kun Viktor - BSC</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>15:00-15:30</strong></td>
                <td valign="top"><strong>Verzi&oacute;friss&iacute;t&eacute;s vagy szoftverb&eacute;rl&eacute;s? Mit jelent az Adobe Creative Cloud tags&aacute;g?</strong></td>
                <td valign="top" nowrap>Szilv&aacute;si &Aacute;kos - BSC</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>15:30-15:40</strong></td>
                <td valign="top"><strong>Z&aacute;r&oacute;sz&oacute;  </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
            </table>
            <br>
            <hr>
          <strong>Helysz&iacute;n:</strong> Hotel Famulus**** 9027 Győr, Budai &uacute;t 4-6. | <strong><a href="http://maps.google.com/maps?q=+9027+Gy%C5%91r,+Budai+%C3%BAt+4-6.&hl=en&ie=UTF8&sll=47.53453,21.622381&sspn=0.042186,0.077162&hnear=9027+Gy%C5%91r,+Budai+%C3%BAt+4,+Hungary&t=m&z=16" target="_blank">Térkép</a></strong><br>
          <strong>Parkol&aacute;s:</strong> D&iacute;jmentes a sz&aacute;lloda parkol&oacute;j&aacute;ban. <br>
          <br></div-->
          <!--div class="TabbedPanelsContent">
            <table width="100%" border="0" cellspacing="0" cellpadding="2">
              <tr>
                <td valign="top" nowrap><strong>09:00-09:30&nbsp;&nbsp;</strong></td>
                <td valign="top"><strong>Regisztr&aacute;ci&oacute;</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>09:30-09:40</strong></td>
                <td valign="top"><strong>K&ouml;sz&ouml;ntő &eacute;s a HRP c&eacute;gcsoport bemutat&aacute;sa</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>09:40-10:20</strong></td>
                <td valign="top"><strong>Office 365 &eacute;s Office 2013 <br>
                </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
                <td valign="top" nowrap>Nagy Levente - Microsoft</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>10:20-10:50</strong></td>
                <td valign="top"><strong>D-Link - A biztons&aacute;g a mi dolgunk!</strong></td>
                <td valign="top" nowrap>Korbel S&aacute;ndor - D-Link</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>10:50-11:30</strong></td>
                <td valign="top"><strong>Dell partneri visszat&eacute;r&iacute;t&eacute;sek &eacute;s akci&oacute;k</strong></td>
                <td valign="top" nowrap>Varga Attila, M&oacute;ritz Krisztina - Dell</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>11:30-12:00</strong></td>
                <td valign="top"><strong>K&aacute;v&eacute;sz&uuml;net</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>12:00-12:30</strong></td>
                <td valign="top"><strong>Windows 7 vagy Windows 8?</strong></td>
                <td valign="top" nowrap>T&oacute;th Andr&aacute;s Mih&aacute;ly - Microsoft</td>
                </tr>
              <tr>
                <td valign="top" nowrap><strong>12:30-13:00</strong></td>
                <td valign="top"><strong>Norton 2013-as &uacute;jdons&aacute;gai PC-n, mobilon &eacute;s tableten Backup Exec.cloud - Ment&eacute;s a felhőbe</strong></td>
                <td valign="top" nowrap>Rakoncza Zsolt <br>
                  Pulai Andr&aacute;s - Symantec</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>13:00-14:00</strong></td>
                <td valign="top"><strong>Eb&eacute;d</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>14:00-14:30</strong></td>
                <td valign="top"><strong>&Uacute;j Samsung notebook sorozatok - 2013-as modellek</strong></td>
                <td valign="top" nowrap>Kulcs&aacute;r Istv&aacute;n - Samsung</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>14:30-15:00</strong></td>
                <td valign="top"><strong>Polycom - &Uacute;jdons&aacute;gok &eacute;s Partner program</strong></td>
                <td valign="top" nowrap>Szilv&aacute;si &Aacute;kos- BSC</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>15:00-15:30</strong></td>
                <td valign="top"><strong>Verzi&oacute;friss&iacute;t&eacute;s vagy szoftverb&eacute;rl&eacute;s? Mit jelent az Adobe Creative Cloud tags&aacute;g?</strong></td>
                <td valign="top" nowrap>Szilv&aacute;si &Aacute;kos - BSC</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>15:30-15:40</strong></td>
                <td valign="top"><strong>Z&aacute;r&oacute;sz&oacute; </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
            </table>
            <hr>
<strong>Helysz&iacute;n:</strong>K&ouml;lcsey K&ouml;zpont, 4026 Debrecen, Hunyadi u. 1-3.  |<strong>  <a href="http://maps.google.com/maps?q=4026+Debrecen,+Hunyadi+u.+1-3.&ie=UTF8&ll=47.53453,21.622381&spn=0.042186,0.077162&oe=UTF-8&hnear=4026+Debrecen,+Hunyadi+J%C3%A1nos+utca+1+3,+Hungary&t=m&z=14" target="_blank">Térkép</a></strong> <br>
<strong>Parkol&aacute;s:</strong> D&iacute;jmentes a K&ouml;lcsey K&ouml;zpont m&eacute;lygar&aacute;zs&aacute;ban.</div-->          
<div class="TabbedPanelsContent">
<table width="100%" border="0" cellspacing="0" cellpadding="2">
  <tr>
    <td valign="top" nowrap><strong>09:00-09:30&nbsp;&nbsp;</strong></td>
    <td valign="top"><strong>Regisztr&aacute;ci&oacute;</strong></td>
    <td valign="top" nowrap>&nbsp;</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>09:30-09:40</strong></td>
    <td valign="top"><strong>K&ouml;sz&ouml;ntő &eacute;s a HRP c&eacute;gcsoport bemutat&aacute;sa</strong></td>
    <td valign="top" nowrap>&nbsp;</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>09:40-10:20</strong></td>
    <td valign="top"><strong>Office 365 &eacute;s Office 2013 <br>
    </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
    <td valign="top" nowrap>Nagy Levente - Microsoft</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>10:20-10:50</strong></td>
    <td valign="top"><strong>D-Link - A biztons&aacute;g a mi dolgunk!</strong></td>
    <td valign="top" nowrap>Korbel S&aacute;ndor - D-Link</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>10:50-11:30</strong></td>
    <td valign="top"><strong>Dell partneri visszat&eacute;r&iacute;t&eacute;sek &eacute;s akci&oacute;k</strong></td>
    <td valign="top" nowrap>Varga Attila, M&oacute;ritz Krisztina - Dell</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>11:30-12:00</strong></td>
    <td valign="top"><strong>K&aacute;v&eacute;sz&uuml;net</strong></td>
    <td valign="top" nowrap>&nbsp;</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>12:00-12:30</strong></td>
    <td valign="top"><strong>Windows 7 vagy Windows 8?</strong></td>
    <td valign="top" nowrap>T&oacute;th Andr&aacute;s Mih&aacute;ly - Microsoft</td>
    </tr>
  <tr>
    <td valign="top" nowrap><strong>12:30-13:00</strong></td>
    <td valign="top"><strong>Norton 2013-as &uacute;jdons&aacute;gai PC-n, mobilon &eacute;s tableten Backup Exec.cloud - Ment&eacute;s a felhőbe</strong></td>
    <td valign="top" nowrap>Rakoncza Zsolt <br>
      Pulai Andr&aacute;s - Symantec</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>13:00-14:00</strong></td>
    <td valign="top"><strong>Eb&eacute;d</strong></td>
    <td valign="top" nowrap>&nbsp;</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>14:00-14:30</strong></td>
    <td valign="top"><strong>&Uacute;j Samsung notebook sorozatok - 2013-as modellek</strong></td>
    <td valign="top" nowrap>Kulcs&aacute;r Istv&aacute;n - Samsung</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>14:30-15:00</strong></td>
    <td valign="top"><strong>Polycom - &Uacute;jdons&aacute;gok &eacute;s Partner program</strong></td>
    <td valign="top" nowrap>Szilv&aacute;si &Aacute;kos- BSC</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>15:00-15:30</strong></td>
    <td valign="top"><strong>Verzi&oacute;friss&iacute;t&eacute;s vagy szoftverb&eacute;rl&eacute;s? Mit jelent az Adobe Creative Cloud tags&aacute;g?</strong></td>
    <td valign="top" nowrap>Szilv&aacute;si &Aacute;kos - BSC</td>
  </tr>
  <tr>
    <td valign="top" nowrap><strong>15:30-15:40</strong></td>
    <td valign="top"><strong>Z&aacute;r&oacute;sz&oacute; </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
    <td valign="top" nowrap>&nbsp;</td>
  </tr>
</table>
<br>
            <hr>
          <strong>Helysz&iacute;n:</strong> Hunguest Hotel Forr&aacute;s**** 6726 Szeged, Szent-Gy&ouml;rgyi A. u. 16-24.<br>
          <strong>Parkol&aacute;s:</strong> D&iacute;jmentes a sz&aacute;lloda mellett tal&aacute;lhat&oacute; Napf&eacute;nyf&uuml;rdő Aquapolis m&eacute;lygar&aacute;zs&aacute;ban.| <strong> <a href="http://maps.google.com/maps?q=6726+Szeged,+Szent-Gy%C3%B6rgyi+A.+u.+16-24.&hl=en&ie=UTF8&sll=47.689041,17.645759&sspn=0.010515,0.01929&hnear=6726+Szeged,+Szent-Gy%C3%B6rgyi+Albert+utca+16,+Hungary&t=m&z=16" target="_blank">Térkép</a></strong><br></div>
          <div class="TabbedPanelsContent">
            <table width="100%" border="0" cellspacing="0" cellpadding="2">
              <tr>
                <td valign="top" nowrap><strong>09:00-09:30&nbsp;&nbsp;</strong></td>
                <td valign="top"><strong>Regisztr&aacute;ci&oacute;</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>09:30-09:40</strong></td>
                <td valign="top"><strong>K&ouml;sz&ouml;ntő &eacute;s a HRP c&eacute;gcsoport bemutat&aacute;sa</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>09:40-10:20</strong></td>
                <td valign="top"><strong>Office 365 &eacute;s Office 2013 <br>
                </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
                <td valign="top" nowrap>Nagy Levente - Microsoft</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>10:20-10:50</strong></td>
                <td valign="top"><strong>D-Link - A biztons&aacute;g a mi dolgunk!</strong></td>
                <td valign="top" nowrap>Korbel S&aacute;ndor - D-Link</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>10:50-11:30</strong></td>
                <td valign="top"><strong>Dell partneri visszat&eacute;r&iacute;t&eacute;sek &eacute;s akci&oacute;k</strong></td>
                <td valign="top" nowrap>T&oacute;th Gy&ouml;rgy - Dell</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>11:30-12:00</strong></td>
                <td valign="top"><strong>K&aacute;v&eacute;sz&uuml;net</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>12:00-12:30</strong></td>
                <td valign="top"><strong>Windows 7 vagy Windows 8?</strong></td>
                <td valign="top" nowrap>T&oacute;th Andr&aacute;s Mih&aacute;ly - Microsoft</td>
                </tr>
              <tr>
                <td valign="top" nowrap><strong>12:30-13:00</strong></td>
                <td valign="top"><strong>Norton 2013-as &uacute;jdons&aacute;gai PC-n, mobilon &eacute;s tableten Backup Exec.cloud - Ment&eacute;s a felhőbe</strong></td>
                <td valign="top" nowrap>Rakoncza Zsolt <br>
                  Pulai Andr&aacute;s - Symantec</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>13:00-14:00</strong></td>
                <td valign="top"><strong>Eb&eacute;d</strong></td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>14:00-14:30</strong></td>
                <td valign="top"><strong>&Uacute;j Samsung notebook sorozatok - 2013-as modellek</strong></td>
                <td valign="top" nowrap>Kulcs&aacute;r Istv&aacute;n - Samsung</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>14:30-15:00</strong></td>
                <td valign="top"><strong>Polycom - &Uacute;jdons&aacute;gok &eacute;s Partner program</strong></td>
                <td valign="top" nowrap>Szilv&aacute;si &Aacute;kos- BSC</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>15:00-15:30</strong></td>
                <td valign="top"><strong>Verzi&oacute;friss&iacute;t&eacute;s vagy szoftverb&eacute;rl&eacute;s? Mit jelent az Adobe Creative Cloud tags&aacute;g?</strong></td>
                <td valign="top" nowrap>Szilv&aacute;si &Aacute;kos - BSC</td>
              </tr>
              <tr>
                <td valign="top" nowrap><strong>15:30-15:40</strong></td>
                <td valign="top"><strong>Z&aacute;r&oacute;sz&oacute; </strong>(Nokia Lumia 820 sorsol&aacute;s)</td>
                <td valign="top" nowrap>&nbsp;</td>
              </tr>
            </table>
            <br>
          <hr>
<strong>Helysz&iacute;n:</strong> Corso Hotel P&eacute;cs****, 7626 P&eacute;cs Koller u. 8 | <strong> <a href="http://maps.google.com/maps?q=+7626+P%C3%A9cs+Koller+u.+8&hl=en&ie=UTF8&ll=46.076208,18.237584&spn=0.010836,0.01929&sll=46.251259,20.160379&sspn=0.010802,0.01929&hnear=7626+P%C3%A9cs,+Koller+utca+8,+Hungary&t=m&z=16" target="_blank">Térkép</a></strong><br>
<strong>Parkol&aacute;s:</strong> D&iacute;jmentes a Kir&aacute;ly-h&aacute;z m&eacute;lygar&aacute;zsban (7621, P&eacute;cs Kir&aacute;ly u 66.)<br></div>
        </div>
      </div>
</div>
    
    <div id="regpanel">
            
<form method="post" id="regform" action="">
                <br/>
                <table width="90%" border="0" align="center" cellpadding="5" cellspacing="10">
                  <tr>
              <td>N&eacute;v</td>
              <td><span id="sprytextfield1">
                      <input name="neve" type="text" class="mezok" id="neve" accesskey="1" tabindex="1" size="35"/>
                    <span class="textfieldRequiredMsg">Kötelező mező</span></span></td>
              </tr>
                  <tr>
                    <td>Email</td>
                    <td><span id="sprytextfield2">
                    <input name="email" type="text" class="mezok"  id="email" accesskey="2" tabindex="2" size="35"/>
                    <span class="textfieldRequiredMsg">Kötelező mező</span><span class="textfieldInvalidFormatMsg">Nem valódi email cím</span></span></td>
                  </tr>
                  <tr>
              <td>Telefon</td>
              <td><span id="sprytextfield3">
                      <input name="telefon" type="text" class="mezok"  id="telefon" accesskey="3" tabindex="3" size="35" />
                    <span class="textfieldRequiredMsg">K&ouml;telező mező</span></span></td>
              </tr>
                  <tr>
              <td>C&eacute;gn&eacute;v</td>
              <td><span id="sprytextfield4">
                      <input name="cegnev" type="text" class="mezok"  id="cegnev" accesskey="4" tabindex="4" size="35" />
                    <span class="textfieldRequiredMsg">K&ouml;telező mező</span></span></td>
              </tr>
                  <tr>
                    <td>Helysz&iacute;n</td>
                    <td><label for="menu"></label>
                      <select name="menu" id="menu" style="font-size:13px; width:250px; padding:5px; color:#666; border: dotted 1px #666;">
                        <option value="rs_0523_szeged">Szeged - 2013.05.23.</option>
                        <option value="rs_0529_pecs">P&eacute;cs - 2013.05.29.</option>
                    </select></td>
                  </tr>
                  <tr>
                    <td>&nbsp;</td>
                    <td>
                    <input name="eventName" type="hidden" class="mezokkisero"  id="eventName" accesskey="5" tabindex="5" value="3013.06.05. Roadshow" />
                    <input name="eventId" type="hidden" id="eventId" value="ROAD-2013" />
                    <a id="anchRegister"><img src="images/gomb.png" alt="elküld" name="reggomb" width="135" height="50" id="reggomb" style="cursor:pointer;" /></a>
                    <!--a href="roadshow_ok.aspx"><img src="images/gomb.png" alt="elküld" name="reggomb" width="135" height="50" id="reggomb" style="cursor:pointer;" /></a-->
                    </td>
                  </tr>
                </table>
  </form>
            
</div>
    
    <img src="images/roadshow.jpg" border="0" >
    </td>
  </tr>
  <tr>
    <td>
    
    </td>
  </tr>
</table>
<br>
<script type="text/javascript">
var sprytextfield1 = new Spry.Widget.ValidationTextField("sprytextfield1", "none", {validateOn:["blur", "change"]});
var sprytextfield2 = new Spry.Widget.ValidationTextField("sprytextfield2", "email", {validateOn:["blur", "change"]});
var sprytextfield3 = new Spry.Widget.ValidationTextField("sprytextfield3", "none", {validateOn:["blur", "change"]});
var sprytextfield4 = new Spry.Widget.ValidationTextField("sprytextfield4", "none", {validateOn:["blur", "change"]});
var TabbedPanels1 = new Spry.Widget.TabbedPanels("TabbedPanels1", {defaultTab:0});
</script>
</body>
</html>