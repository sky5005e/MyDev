//// JavaScript Document

//function showvalue(arg) {
//											
//	}
//	
//	$("#websites4").bind("change", function() 
//		{
//			showvalue($(this).val())
//		});
//	
//	
//	$(document).ready(function() {
//	
//	try {
//			dd3 = $('#websites3').msDropDown({mainCSS:'dd2', onInit:showvalue}).data("dd");
//			var opt = dd3.get('length');
//			//dd3 = dd3.add({text:"lucky", value:"luckyval", title:'images/icon-ok.gif'}, opt);
//			$("#open3").click(function() {dd3.open();});
//			$("#close3").click(function() {dd3.close();});
//			//dd3.addMyEvent("onOpen", showvalue);
//			//dd3.addMyEvent("onClose", showvalue);
//			dd3.addMyEvent("onclick", showvalue);
//			//dd3.disabled(true);
//			//items  = document.getElementById("ComOS2").item(1);
//			items  = dd3.item(1);
//			
//			var ver = dd3.get('version');
//			$("#ver").html(ver);
//			
//			//alert(items)
//		} catch(e) {
//			alert("Error: "+e.message);
//		}
//		//alert(dd3.form().name);
//		//document.getElementById("websites1").options[0] = new Option("Lucky", "_lucky");
//			
//	  })
//	  
//	  $(document).ready(function() {
//	
//	try {
//			dd3 = $('#websites4').msDropDown({mainCSS:'dd2', onInit:showvalue}).data("dd");
//			var opt = dd3.get('length');
//			//dd3 = dd3.add({text:"lucky", value:"luckyval", title:'images/icon-ok.gif'}, opt);
//			$("#open3").click(function() {dd3.open();});
//			$("#close3").click(function() {dd3.close();});
//			//dd3.addMyEvent("onOpen", showvalue);
//			//dd3.addMyEvent("onClose", showvalue);
//			dd3.addMyEvent("onclick", showvalue);
//			//dd3.disabled(true);
//			//items  = document.getElementById("ComOS2").item(1);
//			items  = dd3.item(1);
//			
//			var ver = dd3.get('version');
//			$("#ver").html(ver);
//			
//			//alert(items)
//		} catch(e) {
//			alert("Error: "+e.message);
//		}
//		//alert(dd3.form().name);
//		//document.getElementById("websites1").options[0] = new Option("Lucky", "_lucky");
//
//	  })



//Js by Designer

// JavaScript Document

function showvalue(arg) {
    alert(arg);
    //arg.visible(false);
}

$(document).ready(function() {

    try {
        $("#websites3").msDropDown({ mainCSS: 'dd2' });
        $("#websites2").msDropDown({ mainCSS: 'dd2' });
        $("#websites4").msDropDown({ mainCSS: 'dd2' });
        $("#websites5").msDropDown({ mainCSS: 'dd3' });
        $("#websites6").msDropDown({ mainCSS: 'dd3' });
        $("#websites7").msDropDown({ mainCSS: 'dd3' });
        $("#websites8").msDropDown({ mainCSS: 'dd3' });
        $("#websites9").msDropDown({ mainCSS: 'dd3' });
        $("#websites10").msDropDown({ mainCSS: 'dd3' });
        $("#incentex").msDropDown({ mainCSS: 'dd2' });

        $("#GeneralStatus").msDropDown({ mainCSS: 'dd2' });
        $("#ProofStatus").msDropDown({ mainCSS: 'dd2' });
        $("#ProductionStatus").msDropDown({ mainCSS: 'dd2' });
        $("#NoOfMonths").msDropDown({ mainCSS: 'dd2' });
        $("#EmployeeRank").msDropDown({ mainCSS: 'dd2' });
        $("#FirstRemindr").msDropDown({ mainCSS: 'dd2' });
        $("#SecondRemindr").msDropDown({ mainCSS: 'dd2' });
        $("#EmpStatus").msDropDown({ mainCSS: 'dd2' });
        $("#ProdCategory").msDropDown({ mainCSS: 'dd2' });
        $("#TrdReminder").msDropDown({ mainCSS: 'dd2' });
        $("#DecorateMethod").msDropDown({ mainCSS: 'dd2' });
        $("#ItemsToBePolybagged").msDropDown({ mainCSS: 'dd2' });
        $("#ExpiratinbyMonth").msDropDown({ mainCSS: 'dd2' });
        $("#ExpiratinbyDate").msDropDown({ mainCSS: 'dd2' });
        $("#ManagingShipment").msDropDown({ mainCSS: 'dd2' });
        $("#ManagingShipment").msDropDown({ mainCSS: 'dd2' });
        $("#ItemSizeSticker").msDropDown({ mainCSS: 'dd2' });
        $("#ConsolidateShipment").msDropDown({ mainCSS: 'dd2' });
        $("#ItemCardboardInsert").msDropDown({ mainCSS: 'dd2' });
        $("#FileCategoryAssociate").msDropDown({ mainCSS: 'dd2' });
        $("#Workgroup").msDropDown({ mainCSS: 'dd2' });
        $("#ItemPackedPlasticClips").msDropDown({ mainCSS: 'dd2' });
        $("#BasedStation").msDropDown({ mainCSS: 'dd2' });
        
        
        
        
        
        //alert($.msDropDown.version);

        //$.msDropDown.create("body select");
        $("#ver").html($.msDropDown.version);
    } catch (e) {
        alert("Error: " + e.message);
    }
})
	