var MI;
var DataLoad = 0;
var ContainerNo_outputArray = [];
var ContainerNo_array = [];

$(".chosen_select_L").chosen({
    disable_search_threshold: 5,
    no_results_text: "Oops, nothing found!"
});


$(document).ready(function () {
    $(".chosen_select_L").chosen('destroy');

    //  $('#da').datepicker();
    //   $('#dd').datepicker();
    MI = GetManifest($('#ManifestId').val());
    GetHouseBLItems(MI.ShipmentId);
    if(MI.EntryTypeId == 1)
    {
        $("#EntryType")[0].selectedIndex = 0;
    }
    if(MI.EntryTypeId == 3)
    {
        $("#EntryType")[0].selectedIndex = 1;
    }
    $(".bt_datetime").datetimepicker({format: 'dd/mm/yyyy hh:ii'});
}); 

var koMM;

function ManifestModel() {
    var self = this;
    self.VoyageNo = ko.observable(MI.VoyageNo);
    self.VesselId = new selectAuto();
    self.VesselId.id = MI.VesselId;
    self.VesselId.value = MI.VesselName;
    self.DOAgent = new selectAuto();
    self.DOAgent.id = MI.ShippingAgentId;
    self.DOAgent.value = MI.ShippingAgentName;

    self.DOAgentL = new selectAuto();
    self.DOAgentL.id = MI.DeliveryAgentId;
    self.DOAgentL.value = MI.DeliveryAgentName;

    self.ModeofShipment = ko.observable(MI.ModeofShipmentId);
    self.Nationality = ko.observable(MI.NationalityId);

    self.PortOfDeparture = new selectAuto();
    self.PortOfDestination = new selectAuto();
    self.PortOfDeparture.id = MI.PortOfDeparture;
    self.PortOfDeparture.value = MI.PDeparture;
    self.PortOfDestination.id = MI.PortOfDestination;
    self.PortOfDestination.value = MI.PDestination;

    self.DateOfDeparture = ko.observable(MI.DateDeparture);
    self.DateOfArrival = ko.observable(MI.DateArrival);
    self.MasterBLno = ko.observable(MI.MasterBL);
    self.MasterName = ko.observable(MI.MasterName);
    //  self.TotalNoOfContainer = ko.observable(MI.TotalNoOfContainer);
    self.NetTonnage = ko.observable(MI.NETtonnage);
    self.GrossTonnage = ko.observable(MI.GROSStonnage);
    self.HouseBLItems = ko.observableArray();
    self.MFNumber = ko.observable(MI.MFNumber);
    self.ManifestId = ko.observable(MI.ManifestId);
    self.ShipmentId = ko.observable(MI.ShipmentId);
    //  self.StatusRemarks =ko.observable(MI.StatusRemarks);
    self.Vessel = new VesselModel();
    self.CustomOfficeId = ko.observable(MI.CustomOfficeId);
    self.POwner = new POwnerModel();
    self.EntryType = ko.observable(2);

    // self.Vessels = mapDictionaryToArray(@{Html.RenderAction("GetVessel", "Manifest");});
    self.ModeofShipments = mapDictionaryToArray(@{Html.RenderAction("GetModeofShipment", "Manifest");});
self.Nations = mapDictionaryToArray(@{Html.RenderAction("GetNationality", "Manifest");});
self.CustomOffice = mapDictionaryToArray(@{Html.RenderAction("GetCustomeOffice", "Manifest");});

self.addHouseBL = function(data){
    self.MiniSave(data);// Update the current data before creating another BL
    self.HouseBLItems.push(new HouseBLModel())
    $(".chosen_select_L").chosen();
    if($("#BLNoAutoType")[0].selectedIndex != 0)
    {
        if(self.DOAgentL.id > 0)
        {
            $.each(self.HouseBLItems(), function() {
                if(this.HouseBLno()==null || this.HouseBLno()=="")
                {
                    if(self.DOAgentL.id == 7413 || self.DOAgentL.id == 5749 || self.DOAgentL.id == 1017){
                        result = GenerateHBLnumber(self.DOAgentL.id)
                        this.HouseBLno(result.HBLNumber);
                    }
                    else{
                        this.HouseBLno("");
                    }
                }
            });
        }
    }
}
self.RemoveHBL  = function(data){
    self.HouseBLItems.remove(data)
    DeleteHouseBL(data.HouseBLId())
    $.pnotify({
        title: 'Remove BL',
        text: "BL has been removed",
        type: "success"
    });
}
self.BLPrint  = function(data){
    $.download("../Manifest/PrintBL", {HouseBLId: data.HouseBLId(), ModeofShipment: self.ModeofShipment(), DeliveryAgent:self.DOAgentL.id}, "POST");    
}
self.SendArrivalNotice  = function(data){
    var result = SendArrivalNotice(data.HouseBLId(), self.ModeofShipment(), this.BLNature())
    $.pnotify({
        title: 'Arrival Notice',
        text: result.Message,
        type: result.Status
    });
}
self.RequestDebitNote  = function(data){
    var result = RequestDebitNote(data.ShipmentId(), self.ModeofShipment())
    $.pnotify({
        title: 'Debit Note',
        text: result.Message,
        type: result.Status
    });
}

self.UpdateShipper  = function(data){
    if(data.ShipperNew.id > 0)
    {
        EditParty(data.ShipperNew.id)
    }
    else if(data.ShipperNew.id() > 0)
    {
        EditParty(data.ShipperNew.id())
    }
    else{
        $.pnotify({
            title: 'Update Shipper',
            text: "Please Select a Shipper",
            type: "error"
        });
    }
}

self.UpdateCustomer  = function(data){
    if(data.CustomerNew.id > 0)
    {
        EditParty(data.CustomerNew.id)
    }
    else if(data.CustomerNew.id() > 0)
    {
        EditParty(data.CustomerNew.id())
    }
    else{
        $.pnotify({
            title: 'Update Customer',
            text: "Please Select a Customer",
            type: "error"
        });
    }
}
self.UpdateNotifyParty  = function(data){
    if(data.NotifyPartyNew.id > 0)
    {
        EditParty(data.NotifyPartyNew.id)
    }
    else if(data.NotifyPartyNew.id() > 0)
    {
        EditParty(data.NotifyPartyNew.id())
    }
    else{
        $.pnotify({
            title: 'Update NotifyParty',
            text: "Please Select a NotifyParty",
            type: "error"
        });
    }
}

self.UpdateDOAgent  = function(data){
    if(self.DOAgent.id > 0)
    {
        EditParty(self.DOAgent.id)
    }
    else if(self.DOAgent.id() > 0)
    {
        EditParty(self.DOAgent.id())
    }
    else{
        $.pnotify({
            title: 'Update Shipping Agent',
            text: "Please Select a Shipping Agent",
            type: "error"
        });
    }
}
self.UpdateDeliveryAgent  = function(data){
    if(self.DOAgentL.id > 0)
    {
        EditParty(self.DOAgentL.id)
    }
    else if(self.DOAgentL.id() > 0)
    {
        EditParty(self.DOAgentL.id())
    }
    else{
        $.pnotify({
            title: 'Update Delivery Agent',
            text: "Please Select a Delivery Agent",
            type: "error"
        });
    }
}
self.selectionChanged = (function(event){
    var result;
    if($("#BLNoAutoType")[0].selectedIndex != 0)
    {
        if(self.DOAgentL.id > 0)
        {
            $.each(self.HouseBLItems(), function() {
                if(this.HouseBLno()==null || this.HouseBLno()=="")
                {
                    if(self.DOAgentL.id == 7413 || self.DOAgentL.id == 5749 || self.DOAgentL.id == 1017){
                        result = GenerateHBLnumber(self.DOAgentL.id)
                        this.HouseBLno(result.HBLNumber);
                    }
                    else{
                        this.HouseBLno("");
                    }
                }
            });
        }
    }
});
/* self.updateStatus = function(data){
    $.get('../Manifest/UpdateShipmentStatus',{ShipmentStatusId:$('#ShipmentStatusId').val(), StatusRemarks:data.StatusRemarks(), ShipmentId:data.ShipmentId()},function(result){
         $.pnotify({
             title:'Manifest',
             text: result.Message,
             type: result.Status
         });
    });
 }*/

self.UpdateProject  = function(data){
    if(data.ProjectId.id > 0)
    {
        EditProject(data.ProjectId.id)
    }
    else if(data.ProjectId.id() > 0)
    {
        EditProject(data.ProjectId.id())
    }
    else{
        $.pnotify({
            title: 'Update Project',
            text: "Please Select a Project",
            type: "error"
        });
    }
}

//update Vessel
self.UpdateVessel  = function(data){
    if(self.VesselId.id > 0)
    {
        EditVessel(self.VesselId.id)
    }
    else if(self.VesselId.id() > 0)
    {
        EditVessel(self.VesselId.id())
    }
    else{
        $.pnotify({
            title: 'Update Vessel info',
            text: "Please Select a Project",
            type: "error"
        });
    }
}

self.saveEnabled = ko.computed(function(){
    var flag = 1;
    if(self.VoyageNo()==""){flag = 0;}
    $.each(self.HouseBLItems(), function() { 
        if(this.BLNature()==null){flag = 0;}
        if(this.FreightIndicator()==null){flag = 0;}
        //                if( this.Shipper()=="" || this.Shipper()==null){flag = 0;}
        //                else if( this.Customer()=="" || this.Customer()==null){flag = 0;}
        //                else if( this.NoOfPackage()=="" || this.NoOfPackage()==null){flag = 0;}
        //               // else if( this.ContainerType()=="" || this.ContainerType()==null){flag = 0;}
    });
    return flag;  
});

self.TotalNoOfHBL = ko.computed(function(){
    var total = 0;
    $.each(self.HouseBLItems(), function() {
        if( this.HouseBLno()=="" || this.HouseBLno()==null){
                    
        }else{
            total++;}
    });
    return total;  
});

self.TotalWeight = ko.computed(function(){
    var total = 0;
    $.each(self.HouseBLItems(), function() {
        total = total + parseFloat(this.Weight());
    });
    return total;  
});
self.TotalMeasurement = ko.computed(function(){
    var total = 0;
    $.each(self.HouseBLItems(), function() {
        total = total + parseFloat(this.Measurement());
    });
    return total;  
});

self.TotalNoOfPackages = ko.computed(function(){
    var total = 0;
    $.each(self.HouseBLItems(), function() {
        total = total + parseInt(this.TotalBLPackages());
    });
    return total;  
});

self.TotalNoOfContainer = ko.computed(function(){
    var total = 0;
    $.each(self.HouseBLItems(), function() {
        $.each(this.ContainerItems(), function() {
            var count = 0;
            var start = false;
            if(this.ContainerNo()!=null)
            {
                ContainerNo_array.push(this.ContainerNo()); 
                for (j = 0; j < ContainerNo_array.length; j++) { 
                    for (k = 0; k < ContainerNo_outputArray.length; k++) { 
                        if ( ContainerNo_array[j] == ContainerNo_outputArray[k] ) { 
                            start = true; 
                        } 
                    } 
                    count++; 
                    if (count == 1 && start == false) { 
                        ContainerNo_outputArray.push(ContainerNo_array[j]); 
                    } 
                    start = false; 
                    count = 0; 
                } ; 
                total = ContainerNo_outputArray.length;
            }
        });
    });
    return total;
});

/*condition MOS*/
self.ModeofShipment.subscribe(function(){
    if(self.ModeofShipment()==1){self.CustomOfficeId(1);} // 1-sea
    else if(self.ModeofShipment()==2){self.CustomOfficeId(2);} //2-air
    $.each(self.HouseBLItems(), function() {
        if(self.ModeofShipment()==1){this.BLTypeId(1);} // 1-sea
        else if(self.ModeofShipment()==2){this.BLTypeId(3);} //2-air
        $.each(this.ContainerItems(), function() {
            if(self.ModeofShipment()==1){this.PackingType(1);}
            else if(self.ModeofShipment()==2){this.PackingType(2);}
        });
    });
});

self.MasterBLno.subscribe(function(){ 
    var MBL= SearchMaterBL(self.MasterBLno());
    if(MBL.ManifestId > 0)
    {
        $("#dialog-message1").text("This Master BL '"+self.MasterBLno()+"' is already entered as a Manifest");
        $( "#dialog-confirm1" ).dialog({
            closeOnEscape: false,
            open: function(event, ui) { $(".ui-dialog-titlebar-close").hide(); },
            resizable: false,
            height:180,
            width:315,
            modal: true,
            buttons: {
                "Go Back": function() {
                    self.MasterBLno("");
                    $( this ).dialog( "close" );
                },
                "MBL Detail":function(){
                    window.location = '../Manifest/EditManifest?ManifestId='+ MBL.ManifestId;
                }
            }
        });
    }
    else
    {
        if(MBL.ShipmentId > 0)
        {
            $("#dialog-message2").text("This Master BL '"+self.MasterBLno()+"' is already entered as a JOB, Do you want to Create a Manifest");
            $( "#dialog-confirm2" ).dialog({
                closeOnEscape: false,
                open: function(event, ui) { $(".ui-dialog-titlebar-close").hide(); },
                resizable: false,
                height:180,
                width:315,
                modal: true,
                buttons: {
                    "NO": function() {
                        self.MasterBLno("");
                        $( this ).dialog( "close" );
                    },
                    "YES":function(){
                        ConvertJobToManifest(MBL.ShipmentId);
                        MBL= SearchMaterBL(self.MasterBLno());
                        window.location = '../Manifest/EditManifest?ManifestId='+ MBL.ManifestId; 
                    }
                }
            });
        }
    }

});

self.Save = function(data){
    if ($("#EntryType")[0].selectedIndex == 0) {
        self.EntryType(1);
    }
    if ($("#EntryType")[0].selectedIndex == 1) {
        self.EntryType(3);
    }
    var HB = {HouseBLItems:self.HouseBLItems()};
    var json = JSON.stringify(HB);
    $.ajax({
        url: '/Manifest/UpdateManifest',
        type: 'POST',
        dataType: 'json',
        data: ko.toJSON({MM: koMM}),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.ShipmentStatus == "success") {
                $.pnotify({
                    title: 'Shipment Info ',
                    text: result.ShipmentMessage,
                    type: result.ShipmentStatus
                });
            }
            if (result.ShipmentStatus == "error") {
                $.pnotify({
                    title: 'Shipment Info ',
                    text: result.ShipmentMessage,
                    type: result.ShipmentStatus
                });
            }
            if (result.BOLStatus == "success") {
                $.pnotify({
                    title: 'BOL Info ',
                    text: result.BOLMessage,
                    type: result.BOLStatus
                });
            }
            if (result.BOLStatus == "error") {
                $.pnotify({
                    title: 'BOL Info ',
                    text: result.BOLMessage,
                    type: result.BOLStatus
                });
            }
            if (result.Status == "error") {
                $.pnotify({
                    title: 'Shipment Info ',
                    text: result.Message,
                    type: result.Status
                });
            }
                
        }
    });
}

self.MiniSave = function(data){
    if ($("#EntryType")[0].selectedIndex == 0) {
        self.EntryType(1);
    }
    if ($("#EntryType")[0].selectedIndex == 1) {
        self.EntryType(3);
    }
    var HB = {HouseBLItems:self.HouseBLItems()};
    var json = JSON.stringify(HB);
    $.ajax({
        url: '/Manifest/UpdateManifest',
        type: 'POST',
        dataType: 'json',
        data: ko.toJSON({MM: koMM}),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
        }
    });
}
}

function downloadSuccess(){
    // window.location.href="/Manifest/create";
}


function HouseBLModel(){
    var self = this;
    self.HouseBLId = ko.observable();
    self.HouseBLno = ko.observable();
    self.ContainerItems = ko.observableArray();
    //  self.ContainerItems.push( new ContainerModel());
    // self.ContainerType = ko.observable();
    // self.ContainerNo = ko.observable();
    //  self.SealNo = ko.observable();
    // self.TypeofPackage = ko.observable();
    self.NoOfPackage = ko.observable(0);
    self.TotalBLPackages = ko.observable(0);
    self.FreightIndicator = ko.observable();
    self.BLNature = ko.observable();
    self.ShippingMark = ko.observable();
    self.Shipper = ko.observable();
    self.NotifyParty = ko.observable();
    self.Customer = ko.observable();

    self.CustomerNew = new selectAuto();
    self.ShipperNew =  new selectAuto();
    self.NotifyPartyNew = new selectAuto();
    // self.DOAgent = new selectAuto();

    self.Weight = ko.observable(0);
    self.Measurement = ko.observable();
    self.ProjectId = new selectAuto();

    self.PortOfLoading = new selectAuto();
    self.PortOfUnLoading = new selectAuto();
    self.PortOfOrigin = new selectAuto();
    self.OriginalLoadingPort = new selectAuto();
    self.PortOfDelivery = new selectAuto();
    self.UltimateDestination = new selectAuto();

    self.Description = ko.observable();
    self.BLStatusId = ko.observable();
    self.BLStatusName = ko.observable();
    self.ClearanceBy = new selectAuto();
    self.BLTypeId = ko.observable();
    self.BLStateId = ko.observable();
    // self.ContainerInfo = ko.observable();


    self.FreightIndicators = mapDictionaryToArray(@{Html.RenderAction("GetFreightIndicator", "Manifest");});
self.BLStatus = mapDictionaryToArray(@{Html.RenderAction("GetBLAllStatusList", "Manifest");});
self.BLNatures = mapDictionaryToArray(@{Html.RenderAction("GetBLNature", "Manifest");});
self.BLTypes = mapDictionaryToArray(@{Html.RenderAction("GetBLType", "Manifest");});
self.BLState = mapDictionaryToArray(@{Html.RenderAction("GetBLState", "Manifest");});

       
self.addContainerItem = function(data) {
    self.ContainerItems.push( new ContainerModel());
};

self.TotalBLPackages = ko.computed(function(){
    var total = 0;
    $.each(self.ContainerItems(), function() {
        total = total + parseInt(this.CNoOfPackage());
    });
    return total;  
});

self.Weight = ko.computed(function(){
    var total = 0;
    $.each(self.ContainerItems(), function() {
        total = total + parseFloat(this.CWeight());
    });
    return total;  
});
self.Measurement = ko.computed(function(){
    var total = 0;
    $.each(self.ContainerItems(), function() {
        total = total + parseFloat(this.CMeasure());
    });
    return total;  
});

self.deleteContainerItem  = function(data){
    // console.log(data);
    self.ContainerItems.remove(data);
    DeleteContainer(data.ContainerId())
    $.pnotify({
        title: 'Remove Container',
        text: "Container has been removed",
        type: "success"
    });
}

self.HouseBLno.subscribe(function(){
    if(DataLoad > 0)
    {
        if(koMM.MasterBLno() == self.HouseBLno())
        {
            self.HouseBLno("");
        }
        var HBL= SearchHouseBL(self.HouseBLno());
        if(HBL.ManifestId > 0)
        {
            $("#dialog-message3").text("This House BL '"+self.HouseBLno()+"' is already entered as Manifest");
            $( "#dialog-confirm3" ).dialog({
                closeOnEscape: false,
                open: function(event, ui) { $(".ui-dialog-titlebar-close").hide(); },
                resizable: false,
                height:180,
                width:315,
                modal: true,
                buttons: {
                    "Go Back": function() {
                        self.HouseBLno("");
                        $( this ).dialog( "close" );
                    },
                    "BL detail":function(){
                        //window.location = '../Manifest/EditManifest?ManifestId='+ HBL.ManifestId; 
                        window.open("../Manifest/ViewManifestDetail?ManifestId="+ HBL.ManifestId, 'window name', 'window settings'); 
                    },
                    "Add this BL": function() {
                        MoveBL(HBL.HBLId,HBL.ShipmentId,MI.ShipmentId );
                        AllocateForMovingBL(self.HouseBLno(),MI.ShipmentId);
                        $( this ).dialog( "close" );
                        //  location.reload(); // refresh the page
                    },

                }
            });
        }
        else
        {
            if(HBL.ShipmentId > 0)
            {
                $("#dialog-message4").text("This BL '"+self.HouseBLno()+"' is already entered AS a JOB");
                $( "#dialog-confirm4" ).dialog({
                    closeOnEscape: false,
                    open: function(event, ui) { $(".ui-dialog-titlebar-close").hide(); },
                    resizable: false,
                    height:180,
                    width:315,
                    modal: true,
                    buttons: {
                        "Go Back": function() {
                            self.HouseBLno("");
                            $( this ).dialog( "close" );
                        },
                        /* "Convert":function(){
                         ConvertJobToManifest(HBL.ShipmentId);
                         HBL= SearchHouseBL(self.HouseBLno());
                         window.location = '../Manifest/EditManifest?ManifestId='+ HBL.ManifestId; 
                         },*/
                        "BL detail":function(){
                            //window.location = '../Manifest/EditManifest?ManifestId='+ HBL.ManifestId; 
                            window.open("../Job/ViewJob?JobId="+ HBL.JobId + "&HBLId=" + HBL.HBLId, 'window name', 'window settings'); 
                        },
                        "Add this BL": function() {
                            MoveBL(HBL.HBLId,HBL.ShipmentId,MI.ShipmentId );
                            AllocateForMovingBL(self.HouseBLno(),MI.ShipmentId);
                            $( this ).dialog( "close" );
                            // location.reload(); // refresh the page
                        },
                    }
                });
            }
        }
    }
});
      
}

function ContainerModel(){
    var self = this;
    self.PackingType = ko.observable();
    self.ContainerId  = ko.observable();
    self.ContainerNo = ko.observable();
    self.ContainerType = ko.observable();
    self.PackageType = ko.observable();
    self.ContainerSize = ko.observable();
    self.CNoOfPackage = ko.observable(0);
    self.CWeight = ko.observable(0);
    self.CMeasure = ko.observable(0);
    self.Indicator = ko.observable();
    self.SealNo = ko.observable();
    self.ContainerTypes = mapDictionaryToArray(@{Html.RenderAction("GetContainerType", "Manifest");});
self.IndicatorTypes = mapDictionaryToArray(@{Html.RenderAction("GetContainerIndicatorType", "Manifest");});
self.PackingTypes = mapDictionaryToArray(@{Html.RenderAction("GetPacking", "Manifest");});
self.PackageTypes = mapDictionaryToArray(@{Html.RenderAction("GetTypeofPackage", "Manifest");});
/*condition MOS*/
if(koMM.ModeofShipment()==1){this.PackingType(1);}
else if(koMM.ModeofShipment()==2){this.PackingType(2);}
}




/*============================= Other Functions ============================*/
function mapDictionaryToArray(dictionary) {
    var result = [];
    for (var key in dictionary) {
        if (dictionary.hasOwnProperty(key)) {
            result.push({ key: key, value: dictionary[key] });
        }  
    } 
    return result;
}


    
function mapDictionaryToArray1(dictionary) {
    var result = [];
    for (var key in dictionary) {
        if (dictionary.hasOwnProperty(key)) {
            result.push({ Value: key, Text: dictionary[key] });
        }  
    } 
    return result;
}


/* ko.bindingHandlers.datetimepicker = {
     init: function (element, valueAccessor, allBindings) {
       //initialize datepicker with some optional options
         var format;
         // var defaultFormat = 'yyyy-mm-dd hh:ii:ss'
           var defaultFormat = 'dd/mm/yyyy hh:ii'
         if (typeof allBindings == 'function') {
             format = allBindings().format || defaultFormat;
         }
         else {
             format = allBindings.get('format') || defaultFormat;
         }
         $(element).datetimepicker({
            // initialDate: new Date(),
             autoclose: true,
             todayBtn: true,
             'format': format
         })
       //when a user changes the date, update the view model
       ko.utils.registerEventHandler(element, "changeDate", function(event) {
           var value = valueAccessor();
              if (ko.isObservable(value)) {
                  value($(element).datetimepicker("getFormattedDate"));
              }
       });
     },
     update: function (element, valueAccessor) {
        // var date = ko.unwrap(valueAccessor());
         var date = ko.utils.unwrapObservable(valueAccessor());
         if (date) {
             $(element).datetimepicker('setValue', date);
         }
     }
 };*/




function selectAuto(){
    var self = this;
    self.value = ko.observable();
    self.id = ko.observable();
}

ko.bindingHandlers.PartySelect = {
    init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called when the binding is first applied to an element
        // Set up any initial state, event handlers, etc. here

        $(element).autocomplete({
            source: function( request, response ) {
                $.ajax( {
                    url: "../Party/SearchParty",
                    dataType: "json",
                    data:{ 'query':request.term ,"type":1, subtype: 2 },
                    success: function( data ) {
                        var test = [];



                        // pass a function to map
                      const map1 = data.map(x => ({
                          id:x.Id,
                          label:x.Name,
                          value:x.Name
                      }));

                        console.log(map1);
                        response( map1 );
                    }
                } );
            },
            minLength: 2,
            select: function( event, ui ) {
       
                valueAccessor().id =  ui.item.id;
                valueAccessor().value =  ui.item.value;
                console.log(valueAccessor());    
                console.log(viewModel);
                console.log( "Selected: " + ui.item.value + " aka " + ui.item.id );
            }
        } );
    },
    update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever any observables/computeds that are accessed change
        // Update the DOM element based on the supplied values here.
    }
};


ko.bindingHandlers.PortSelect = {
    init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called when the binding is first applied to an element
        // Set up any initial state, event handlers, etc. here

        $(element).autocomplete({
            source: function( request, response ) {
                $.ajax( {
                    url: "../Manifest/SearchPort",
                    dataType: "json",
                    data:{ 'query':request.term ,"type":1, subtype: 2, "mode": koMM.ModeofShipment },
                    success: function( data ) {
                        var test = [];



                        // pass a function to map
                      const map1 = data.map(x => ({
                          id:x.Id,
                          label:x.Name,
                          value:x.Name
                      }));

                        console.log(map1);
                        response( map1 );
                    }
                } );
            },
            minLength: 2,
            select: function( event, ui ) {
       
                valueAccessor().id =  ui.item.id;
                valueAccessor().value =  ui.item.value;
                console.log(valueAccessor());    
                console.log(viewModel);
                console.log( "Selected: " + ui.item.value + " aka " + ui.item.id );
            }
        } );
    },
    update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever any observables/computeds that are accessed change
        // Update the DOM element based on the supplied values here.
    }
};

//Vessel Selcet Function
ko.bindingHandlers.VesselSelect = {
    init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called when the binding is first applied to an element
        // Set up any initial state, event handlers, etc. here

        $(element).autocomplete({
            source: function( request, response ) {
                $.ajax( {
                    url: "../Party/SearchVessel",
                    dataType: "json",
                    data:{ 'query':request.term ,"type":1, subtype: 2 },
                    success: function( data ) {
                        var test = [];



                        // pass a function to map
                      const map1 = data.map(x => ({
                          id:x.Id,
                          label:x.Name,
                          value:x.Name
                      }));

                        console.log(map1);
                        response( map1 );
                    }
                } );
            },
            minLength: 2,
            select: function( event, ui ) {
       
                valueAccessor().id =  ui.item.id;
                valueAccessor().value =  ui.item.value;
                console.log(valueAccessor());    
                console.log(viewModel);
                console.log( "Selected: " + ui.item.value + " aka " + ui.item.id );
            }
        } );
    },
    update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever any observables/computeds that are accessed change
        // Update the DOM element based on the supplied values here.
    }
};

//Project Selcet Function
ko.bindingHandlers.ProjectSelect = {
    init: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called when the binding is first applied to an element
        // Set up any initial state, event handlers, etc. here

        $(element).autocomplete({
            source: function( request, response ) {
                $.ajax( {
                    url: "../Job/SearchProject",
                    dataType: "json",
                    data:{ 'query':request.term ,"type":1, subtype: 2 },
                    success: function( data ) {
                        var test = [];



                        // pass a function to map
                      const map1 = data.map(x => ({
                          id:x.Id,
                          label:x.Name,
                          value:x.Name
                      }));

                        console.log(map1);
                        response( map1 );
                    }
                } );
            },
            minLength: 2,
            select: function( event, ui ) {
       
                valueAccessor().id =  ui.item.id;
                valueAccessor().value =  ui.item.value;
                console.log(valueAccessor());    
                console.log(viewModel);
                console.log( "Selected: " + ui.item.value + " aka " + ui.item.id );
            }
        } );
    },
    update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
        // This will be called once when the binding is first applied to an element,
        // and again whenever any observables/computeds that are accessed change
        // Update the DOM element based on the supplied values here.
    }
};


//function to allow only numbers (eg:<input type=="" onkeypress="return isNumberKey(event)/>)
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    } else {
        return true;
    }      
}


function DeleteHouseBL(HBLId){
    var url = "@Url.Action("DeleteHBLitem", "Manifest")";
    var results ;
    var flag = 0;
    var param = {HBLId:HBLId};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function GenerateHBLnumber(val){
    var url = "@Url.Action("GenerateHouseBLnumber", "Manifest")";
    var results ;
    var flag = 0;
    var param = {DOAId:val};   
    var data = JSON.stringify(param);     
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function SendArrivalNotice(HBLId,Mode, BLN){
    var url = "@Url.Action("SendArrivalNotice", "Manifest")";
    var results ;
    var flag = 0;
    var param = {HBLId:HBLId, ModeofShipment: Mode, BLNature: BLN};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function RequestDebitNote(SPId,Mode){
    var url = "@Url.Action("RequestDebitNote", "Manifest")";
    var results ;
    var flag = 0;
    var param = {SPId:SPId, ModeofShipment: Mode};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function DeleteContainer(ContainerId){
    var url = "@Url.Action("DeleteContaineritem", "Manifest")";
    var results ;
    var flag = 0;
    var param = {ContainerId:ContainerId};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

    
function GetManifest(MfId){
    var url = "@Url.Action("GetManifest", "Manifest")";
    var results ;
    var flag = 0;
    var param = {MfId:MfId};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function SearchMaterBL(MasterBLnumber){
    var url = "@Url.Action("GetMasterBL", "Manifest")";
    var results ;
    var flag = 0;
    var param = {MasterBLnumber:MasterBLnumber};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function SearchHouseBL(HouseBLnumber){
    var url = "@Url.Action("GetHouseBL", "Manifest")";
    var results ;
    var flag = 0;
    var param = {HouseBLnumber:HouseBLnumber};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function ConvertJobToManifest(ShipmentId){
    var url = "@Url.Action("ConvertJobToManifest", "Manifest")";
    var results ;
    var flag = 0;
    var param = {ShipmentId:ShipmentId};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function MoveBL(BLId, CurrentShipmentId, NewShipmentId){
    var url = "@Url.Action("MoveBL", "Manifest")";
    var results ;
    var flag = 0;
    var param = {BLId:BLId, CurrShipmentId:CurrentShipmentId, NewShipmentId:NewShipmentId};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}


/* $("#updateStatus").click(function () {   
     $.get('../Manifest/UpdateShipmentStatus',{ShipmentStatusId: $('#ShipmentStatusId').val(), StatusRemarks: self.StatusRemarks()},function(result){
         $.pnotify({
             title:'Manifest',
             text: result.Message,
             type: result.Status
         });
     });
  });*/



function GotoAttachments()
{
    // var myWindow = window.open("../JobDocument/RawIndex?ShipmentId=" + MI.ShipmentId, "", "width=1100,height=600");
    // window.location.href = "../JobDocument/Index?ShipmentId=" + MI.ShipmentId;
    var page = "../JobDocument/RawIndex?ShipmentId=" + MI.ShipmentId ;
    var $dialog = $('<div style="overflow:visible"></div>')
                   .html('<iframe style="border: 0px;" src="' + page + '" width="100%" height="100%"></iframe>')
                   .dialog({
                       autoOpen: false,
                       modal: true,
                       height: 720,
                       width: 1340,
                       resizable: true,
                       title: "DOCUMENTS",
                       buttons: {
                           Close: function () {
                               $(this).dialog("close");

                           }
                       }
                   });
    $dialog.dialog('open');
}

function GenerateXML()
{
    $.download("../XML/SerializeManifestToXML", { ManifestId: MI.ManifestId}, "POST");   
    // window.location.href = "../JobDocument/Index?ShipmentId=" + MI.ShipmentId;
}

function GetHouseBLItems(SPId){
    $.getJSON("../Manifest/GetHouseBLItems",{SPId:SPId},function(data){
        for(var i=0; i <data.length;i++)
        {
            var items = new HouseBLModel();
            items.HouseBLId(data[i].HouseBLId);
            items.HouseBLno(data[i].HouseBL);
            items.Shipper(data[i].ShipperId);
            items.NotifyParty(data[i].NotifyPartyId);
            items.Customer(data[i].CustomerId);

            items.ShipperNew.id(data[i].ShipperId); items.ShipperNew.value(data[i].ShipperName);
            items.CustomerNew.id(data[i].CustomerId); items.CustomerNew.value(data[i].CustomerName);
            items.NotifyPartyNew.id(data[i].NotifyPartyId); items.NotifyPartyNew.value(data[i].NotifyName);
            //  items.DOAgent.id(data[i].DOAgentId); items.DOAgent.value(data[i].DOAgentName);
                    
            items.Description(data[i].Description);
            //   items.ContainerInfo(data[i].ContainerInfo);
            //  items.ContainerType(data[i].ContainerTypeId);
            //  items.ContainerNo(data[i].ContainerNo);
            //  items.SealNo(data[i].SealNo);
            //   items.TypeofPackage(data[i].TypeofPackageId);
            items.NoOfPackage(data[i].NoOfPackage);
            items.FreightIndicator(data[i].FreightIndicatorId);
            //  items.Weight(data[i].Weight);
            //  items.Measurement(data[i].Measurement);
            items.ProjectId.id(data[i].ProjectId); items.ProjectId.value(data[i].ProjectName);

            items.PortOfLoading.id(data[i].PortOfLoading); items.PortOfLoading.value(data[i].PortOfLoadingName);
            items.PortOfUnLoading.id(data[i].PortOfUnloading); items.PortOfUnLoading.value(data[i].PortOfUnloadingName);
            items.PortOfOrigin.id(data[i].PortOfOrigin); items.PortOfOrigin.value(data[i].PortOfOriginName);
            items.OriginalLoadingPort.id(data[i].OriginalLoadingPort); items.OriginalLoadingPort.value(data[i].OriginalLoadingPortName);
            items.UltimateDestination.id(data[i].UltimateDestination); items.UltimateDestination.value(data[i].UltimateDestinationName);
            items.PortOfDelivery.id(data[i].PortOfDelivery); items.PortOfDelivery.value(data[i].PortOfDeliveryName);
                    
            items.BLStatusId(data[i].BLStatusId);
            items.BLStatusName(data[i].BLStatusName);
            items.ClearanceBy.id(data[i].ClearanceBy); items.ClearanceBy.value(data[i].ClearanceByName);
            items.BLNature(data[i].BLNatureId);
            items.BLTypeId(data[i].BLTypesId);
            items.BLStateId(data[i].BLStateId);
            items.ShippingMark(data[i].ShippingMark);

            koMM.HouseBLItems.push(items)
            //    $(".chosen_select_L").chosen();
            GetContainerItems(data[i].HouseBLId,[i]);
        }
        DataLoad = 1;
    });
}


function GetContainerItems(HBLId,BLcount){
    console.log(HBLId,BLcount);
    $.getJSON("../Manifest/GetContainerItems",{HBLId:HBLId},function(data){
        for(var k=0; k <data.length;k++)
        {
            var items = new ContainerModel();
            items.ContainerId(data[k].Id);
            items.PackingType(data[k].PackingId);
            items.ContainerNo(data[k].ContainerNo);
            items.ContainerType(data[k].ContainerTypeId);
            items.PackageType(data[k].TypeofPackageId)
            items.ContainerSize(data[k].Size);
            items.CNoOfPackage(data[k].CNoOfPackage);
            items.CWeight(data[k].CWeight);
            items.CMeasure(data[k].CMeasure);
            items.Indicator(data[k].ContainerIndicatorId);
            items.SealNo(data[k].SealNo);

            koMM.HouseBLItems()[BLcount].ContainerItems.push(items);
        }
    });
}

/****************************Get Data for Moving BL**********************************/

function GetDataForMovingBL(BLno, newShipmentId){
    var url = "@Url.Action("GetHouseBLItem", "Manifest")";
    var results ;
    var flag = 0;
    var param = {SPId:newShipmentId,HBLno:BLno};               
    var data = JSON.stringify(param);   
    $.ajax({
        type: "POST",
        url: url,
        async:false,
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function(result){
            results = result;
        },
        dataType: "json"
    });
    return results;
}

function AllocateForMovingBL(BLno, newShipmentId){
    var BLcount = -1;
    $.each(koMM.HouseBLItems(), function() {
        BLcount = BLcount + 1;
        if(this.HouseBLno()==BLno)
        {
            //console.log("yes"+ this.HouseBLno()); //AFTMLE240800058test
            var data = GetDataForMovingBL(BLno, newShipmentId);
            this.HouseBLId(data[0].HouseBLId);
            this.HouseBLno(data[0].HouseBL);
            this.Shipper(data[0].ShipperId);
            this.NotifyParty(data[0].NotifyPartyId);
            this.Customer(data[0].CustomerId);

            this.ShipperNew.id(1123); this.ShipperNew.value("HIDEAWAY BEACH RESORT & SPA  AT DHONAKULHI INVESTMENT PVT LTD");
            this.CustomerNew.id(data[0].CustomerId); this.CustomerNew.value(data[0].CustomerName);
            this.NotifyPartyNew.id(data[0].NotifyPartyId); this.NotifyPartyNew.value(data[0].NotifyName);
                    
            this.Description(data[0].Description);
            this.NoOfPackage(data[0].NoOfPackage);
            this.FreightIndicator(data[0].FreightIndicatorId);
            this.ProjectId.id(data[0].ProjectId); this.ProjectId.value(data[0].ProjectName);

            this.PortOfLoading.id(data[0].PortOfLoading); this.PortOfLoading.value(data[0].PortOfLoadingName);
            this.PortOfUnLoading.id(data[0].PortOfUnloading); this.PortOfUnLoading.value(data[0].PortOfUnloadingName);
            this.PortOfOrigin.id(data[0].PortOfOrigin); this.PortOfOrigin.value(data[0].PortOfOriginName);
            this.OriginalLoadingPort.id(data[0].OriginalLoadingPort); this.OriginalLoadingPort.value(data[0].OriginalLoadingPortName);
            this.UltimateDestination.id(data[0].UltimateDestination); this.UltimateDestination.value(data[0].UltimateDestinationName);
            this.PortOfDelivery.id(data[0].PortOfDelivery); this.PortOfDelivery.value(data[0].PortOfDeliveryName);
                    
            this.BLStatusId(data[0].BLStatusId);
            this.BLStatusName(data[0].BLStatusName);
            this.ClearanceBy.id(data[0].ClearanceBy); this.ClearanceBy.value(data[0].ClearanceByName);
            this.BLNature(data[0].BLNatureId);
            this.BLTypeId(data[0].BLTypesId);
            this.BLStateId(data[0].BLStateId);
            this.ShippingMark(data[0].ShippingMark);

            GetContainerItems(data[0].HouseBLId,BLcount);
        }
    });
}

/*============================= Main Model Biding ============================*/
$(function(){
    koMM = new ManifestModel()
    ko.applyBindings(koMM);
})


@*On Scroll Sticky Header CSS*@
window.onscroll = function() {myFunction()};
var header = document.getElementById("myHeader");
var sticky = header.offsetTop;
function myFunction() {
    if (window.pageYOffset > sticky) {
        header.classList.add("sticky");
    } else {
        header.classList.remove("sticky");
    }
}