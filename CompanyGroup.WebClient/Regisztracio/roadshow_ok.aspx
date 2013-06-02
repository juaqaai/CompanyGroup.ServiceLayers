<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="lunchandlearn_Register" %>
<html>
<head>
<title>Regisztr&aacute;ci&oacute;</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-2">
<script type="text/javascript" src="../clientscript/jquery-1.4.4.min.js"></script>
<script type="text/javascript" src="../clientscript/jquery.json-2.2.min.js"></script>
<script type="text/javascript"> 
    function addRegistration() {
        var personname = $("#neve").val();
        var companyname = $("#cegnev").val();
        var phone = $("#telefon").val();
        var email = $("#email").val();
        var follower = $("#szemszam").val();
        var menu = $("#menu").val();
        jQuery.ajax({
            type: "POST",
            url: "EventRegWebService.asmx/AddEventReg",
            data: '{"PersonName": "' + personname + '","CompanyName": "' + companyname + '","Phone": "' + phone + '","Email": "' + email + '","Follower": "' + follower + '","Menu": "' + menu + '"}',
            contentType: "application/json; charset=utf-8",
            timeout: 10000,
            dataType: "json",
            processData: true,
            success: function(msg) {
                var ret = msg.d; //AddEventRegResult
                if (ret > 0) {
                    //alert('A regisztráció sikeresen megtörtént.');
                    window.location = "symbol_120419_ok.aspx";
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
<title>Megh&iacute;v&oacute;</title>
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
	top: 1037px;
	visibility: visible;
}

#helyszinekpanel{
	position:absolute;
	width:734px;
	height:900px;
	z-index:1;
	margin-left: 8px;
	top: 508px;
	padding: 0px;
	visibility: visible;
	background-color: #5D8FC2;
	color: #FFF;
	text-align: center;
	font-family: Verdana, Geneva, sans-serif;
	font-size: 24px;
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
	color: #FFF;
}
a:visited {
	color: #FFF;
}
a:hover {
	color: #FFF;
}
a:active {
	color: #FFF;
}
</style>
</head>
<body topmargin="0" marginheight="0">
<br>
<table width="750"  border="0" align="center" cellpadding="0" cellspacing="0" id="Table_01">
  <tr>
    <td>
    
    <div id="helyszinekpanel">
      <p>&nbsp;</p>
      <p>Regisztr&aacute;ci&oacute;j&aacute;t rendszer&uuml;nk r&ouml;gz&iacute;tette.<br>
        K&ouml;sz&ouml;nj&uuml;k!<br>
        Ne felejtse el rendezv&eacute;ny&uuml;nket felvenni<br>
Outlook        eml&eacute;keztetői k&ouml;z&eacute;!<br>
</p>
      <table width="0" border="0" align="center" cellpadding="5" cellspacing="0" style="border:#FFF 3px solid;">
        <tr>
          <td nowrap><a href="ROADSHOW_GYOR_2013_05_09.ics">Győr 2013.05.09.</a></td>
          <td><a style="padding:2px; color:#FFF;" href="ROADSHOW_GYOR_2013_05_09.ics"><img src="images/Apps-Calendar-Metro-icon.png" alt="" width="32" height="32" border="0" align="absmiddle"></a></td>
          <td><a href="ROADSHOW_GYOR_2013_05_09.ics">Ment&eacute;s</a></td>
        </tr>
        <tr>
          <td nowrap><a href="ROADSHOW_DEBRECEN_2013_05_16.ics">Debrecen 2013.05.16.</a></td>
          <td><a style="padding:2px; color:#FFF;" href="ROADSHOW_DEBRECEN_2013_05_16.ics"><img src="images/Apps-Calendar-Metro-icon.png" alt="" width="32" height="32" border="0" align="absmiddle"></a></td>
          <td><a href="ROADSHOW_DEBRECEN_2013_05_16.ics">Ment&eacute;s</a></td>
        </tr>
        <tr>
          <td nowrap><a href="ROADSHOW_SZEGED_2013_05_23.ics">Szeged 2013.05.23.</a></td>
          <td><a style="padding:2px; color:#FFF;" href="ROADSHOW_SZEGED_2013_05_23.ics"><img src="images/Apps-Calendar-Metro-icon.png" alt="" width="32" height="32" border="0" align="absmiddle"></a></td>
          <td><a href="ROADSHOW_SZEGED_2013_05_23.ics">Ment&eacute;s</a></td>
        </tr>
        <tr>
          <td nowrap><a href="ROADSHOW_PECS_2013_05_29.ics">P&eacute;cs 2013.05.29.</a></td>
          <td><a style="padding:2px; color:#FFF;" href="ROADSHOW_PECS_2013_05_29.ics"><img src="images/Apps-Calendar-Metro-icon.png" alt="" width="32" height="32" border="0" align="absmiddle"></a></td>
          <td><a href="ROADSHOW_PECS_2013_05_29.ics">Ment&eacute;s</a></td>
        </tr>
      </table>
      <p><br>
  <br>
      </p>
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
</body>
</html>