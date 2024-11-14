var dash;
//var SCInfo;
var bcrump;
//var rgvm;

var Url
$(function () {
    $("#ManifestTable").addClass("hide");
    $("#ManifestTable_wrapper").addClass("hide");
    $("#BOLTable").addClass("hide");
    $("#BOLTable_wrapper").addClass("hide");

    dash = new DashBoard();
    ko.applyBindings(dash, $("#dashboard")[0]);
    dash.init();

    bcrump = new breadCrumpVM();
    ko.applyBindings(bcrump, $("#breadcrump")[0]);
    bcrump.init();
});

$(".actionItem").live("click", function (event) {
    $.each($(".actionItem.active"), function () {
        $(this).removeClass('active');
    });
    $(this).addClass('active');
});

function DashBoard() {
    var self = this;
    self.dashboarditem = ko.observableArray();
    self.init = function () {
        $.getJSON("../Payments/GetDashboardItem", function (data) {
            self.dashboarditem(data);
        })
    }
    self.FilterStatus = function (data) {
        bcrump.breadcrumptlist.removeAll();
        bcrump.breadcrumptlist.push(new breadItem("Dash Board", true, 1, data.PaymentStatId));
        bcrump.breadcrumptlist.push(new breadItem(data.Name, false, 2, data.PaymentStatId));
        getFilterStatus(data.PaymentStatId);
    }
}

function breadCrumpVM() {
    var self = this;
    self.breadcrumptlist = ko.observableArray();
    self.init = function () {
        self.breadcrumptlist.push(new breadItem("Dash Board", false, 1, 0));
    }
    self.RemoveLast = function () {
        self.breadcrumptlist.pop();
        self.breadcrumptlist()[(bcrump.breadcrumptlist().length - 1)].divider(false);
    }
}

function breadItem(name, divider, level, PaymentStatId) {
    var self = this;
    self.Name = ko.observable(name);
    self.divider = ko.observable(divider);
    self.level = ko.observable(level);
    self.navigate = function () {
        //console.log(level)
        if (level == 2) {
            bcrump.RemoveLast();
            //   $("#dashboard").removeClass("hidden");
            dash.init();
            getFilterStatus(PaymentStatId)
        }
        if (level == 1) {
            window.location = '../Payments/RequestedPayments';
        }


    }
}


function getFilterStatus(status) {
    if (status == 3) {
        $("#BOLTable").addClass("hide");
        $("#BOLTable_wrapper").addClass("hide");
        $("#ManifestTable").removeClass("hide");
        $('#ManifestTable').css('width', '100%');
        loadManifestTable(status);

    } else {
        $("#ManifestTable").addClass("hide");
        $("#ManifestTable_wrapper").addClass("hide");
        $("#BOLTable").removeClass("hide");
        $('#BOLTable').css('width', '100%');
       // GetPaymentReadyBol(5, 1);
    }
}