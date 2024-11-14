var dash;
//var SCInfo;
var bcrump;
//var rgvm;

var Url
$(function () {
    $("#ManifestTable").addClass("hide");
    $("#ManifestTable_wrapper").addClass("hide");
    $("#JobTable").addClass("hide");
    $("#JobTable_wrapper").addClass("hide");
    $("#dailyclearance").addClass("hide");

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
        $.getJSON("../Job/GetDashboardItem", function (data) {
            self.dashboarditem(data);
        })
    }
    self.FilterStatus = function (data) {
        bcrump.breadcrumptlist.removeAll();
        bcrump.breadcrumptlist.push(new breadItem("Dash Board", true, 1, data.BLStatusId));
        bcrump.breadcrumptlist.push(new breadItem(data.Name, false, 2, data.BLStatusId));
        getFilterStatus(data.BLStatusId);
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

function breadItem(name, divider, level, BLStatusId) {
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
            getFilterStatus(BLStatusId)
        }
        if (level == 1) {
            window.location = '../Job/index';
        }


    }
}


function getFilterStatus(status) {
    if (status == 2){
        $("#JobTable").addClass("hide");
        $("#JobTable_wrapper").addClass("hide");
        $("#ManifestTable").removeClass("hide");
        $('#ManifestTable').css('width', '100%');
        $("#dailyclearance").addClass("hide");
        loadManifestTable(status);
    }
    else if (status == 4) {
        $("#ManifestTable").addClass("hide");
        $("#ManifestTable_wrapper").addClass("hide");
        $("#JobTable").removeClass("hide");
        $('#JobTable').css('width', '100%');
        $("#dailyclearance").addClass("hide");
        loadNewJobTable(status)
    }
    else if (status == 100) {
        $("#ManifestTable").addClass("hide");
        $("#ManifestTable_wrapper").addClass("hide");
        $("#JobTable").removeClass("hide");
        $('#JobTable').css('width', '100%');
        $("#dailyclearance").addClass("hide");
        loadDOPayRequestedJobs();
    }
    else if (status == 101) {
        $("#ManifestTable").addClass("hide");
        $("#ManifestTable_wrapper").addClass("hide");
        $("#JobTable").removeClass("hide");
        $('#JobTable').css('width', '100%');
        $("#dailyclearance").addClass("hide");
        loadDOPendingJobs();
    }
    else if (status == 102) {
        $("#ManifestTable").addClass("hide");
        $("#ManifestTable_wrapper").addClass("hide");
        $("#JobTable").removeClass("hide");
        $('#JobTable').css('width', '100%');
        $("#dailyclearance").removeClass("hide");
        loadDOCollectedJobs();
    }
    else if (status == 6) {
        $("#dailyclearance").removeClass("hide");
        $("#ManifestTable").addClass("hide");
        $("#ManifestTable_wrapper").addClass("hide");
        $("#JobTable").removeClass("hide");
        $('#JobTable').css('width', '100%');
        loadJobTable(status);
    }
    else {
        $("#dailyclearance").removeClass("hide");
        $("#ManifestTable").addClass("hide");
        $("#ManifestTable_wrapper").addClass("hide");
        $("#JobTable").removeClass("hide");
        $('#JobTable').css('width', '100%');
        loadJobTable(status);
    }
}