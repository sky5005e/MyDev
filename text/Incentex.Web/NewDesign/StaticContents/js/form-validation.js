// JavaScript Document

jQuery(function(){
									jQuery("#ValidFname").validate({
											expression: "if (VAL) return true; else return false;",
											message: ""
									});
									jQuery("#ValidLname").validate({
											expression: "if (VAL) return true; else return false;",
											message: ""
									});
									jQuery("#coname").validate({
											expression: "if (VAL) return true; else return false;",
											message: ""
									});
									jQuery("#ValidSelection").validate({
											expression: "if (VAL != '0') return true; else return false;",
											message: "Please make a selection"
									});
									jQuery("#ValidMonth").validate({
											expression: "if (VAL.match(/^[0-9]*$/) && VAL) return true; else return false;",
											message: ""
									});
									jQuery("#Validday").validate({
											expression: "if (VAL.match(/^[0-9]*$/) && VAL) return true; else return false;",
											message: ""
									});
									jQuery("#ValidYear").validate({
											expression: "if (VAL.match(/^[0-9]*$/) && VAL) return true; else return false;",
											message: ""
									});
									jQuery("#Email").validate({
											expression: "if (VAL.match(/^[^\\W][a-zA-Z0-9\\_\\-\\.]+([a-zA-Z0-9\\_\\-\\.]+)*\\@[a-zA-Z0-9_]+(\\.[a-zA-Z0-9_]+)*\\.[a-zA-Z]{2,4}$/)) return true; else return false;",
											message: ""
									});
									jQuery("#ValidPassword").validate({
											expression: "if (VAL.length > 5 && VAL) return true; else return false;",
											message: ""
									});
                
            });