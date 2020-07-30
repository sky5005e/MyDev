//function IsSessionExist() {
//    var CheckSessionvalue = "<%=Common.SessionExpired %>";
//    if (CheckSessionvalue) {
//        alert(CheckSessionvalue);
//        return true;
//    }
//    else {
//        alert(CheckSessionvalue);
//        window.location = siteurl + "login.aspx";
//        return false;
//    }
//}
//For Get Currency rate
function CurrencyFrom(newcurrency) {
    var amount = $('#ctl00_txtCurrencyFrom');
    var from = $('#ctl00_ddlCurrencyFrom').val();
    var to = $('#ctl00_ddlCurrencyTo').val();
    $.ajax({
        type: "POST",
        url: siteurl + "UserPages/WSUser.asmx/CurrencyConversion",
        data: "{amount:" + amount.val() + ",fromCurrency:'" + from + "',toCurrency:'" + to + "'}",
        contentType: "application/json; charset=utf-8",
        processData: false,
        dataType: "json",
        success: function(msg) {
            //if (IsSessionExist()) {
                $('#ctl00_lblNewCurrencyValue').html(msg.d);
           // }
        },
        error: function(x, e) {
            if (x.status == 500) {
                alert("An error has occurred during processing your request.");
            }
        }
    });
}