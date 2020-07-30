// JavaScript Document

function showvalue(arg) {
	alert(arg);
	//arg.visible(false);
}

$(document).ready(function() {

try {
		$(".websites3").msDropDown({mainCSS:'dd2'});
		$(".websites2").msDropDown({mainCSS:'dd2'});
		$(".websites4").msDropDown({mainCSS:'dd2'});
		$(".websites5").msDropDown({mainCSS:'dd3'});
		$(".websites6").msDropDown({mainCSS:'dd3'});
		$(".websites7").msDropDown({mainCSS:'dd3'});
		$(".websites8").msDropDown({mainCSS:'dd3'});
		$(".websites9").msDropDown({mainCSS:'dd3'});
		$(".websites10").msDropDown({mainCSS:'dd3'});
		//alert($.msDropDown.version);

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
		$("#Gender").msDropDown({ mainCSS: 'dd2' });
		$("#Department").msDropDown({ mainCSS: 'dd2' });
		$("#Region").msDropDown({ mainCSS: 'dd2' });
		$("#DDepartment").msDropDown({ mainCSS: 'dd2' });
		$("#ProductStatus").msDropDown({ mainCSS: 'dd2' });
		$("#TailoringStatus").msDropDown({ mainCSS: 'dd2' });
		$("#Anniversary").msDropDown({ mainCSS: 'dd2' });
		$("#Style").msDropDown({ mainCSS: 'dd2' });
		$("#Color").msDropDown({ mainCSS: 'dd2' });
		$("#Size").msDropDown({ mainCSS: 'dd2' });
		$("#MasterItemNo").msDropDown({ mainCSS: 'dd2' });
		$("#SoldBy").msDropDown({ mainCSS: 'dd2' });
		$("#Rank").msDropDown({ mainCSS: 'dd2' });
		$("#BaseStation1").msDropDown({ mainCSS: 'dd2' });
		$("#priority").msDropDown({ mainCSS: 'dd2' });
		$("#Climate").msDropDown({ mainCSS: 'dd2' });
		$("#EmployeeType").msDropDown({ mainCSS: 'dd2' });
		//$.msDropDown.create("body select");
		$("#ver").html($.msDropDown.version);
		} catch(e) {
			alert("Error: "+e.message);
		}
})
	