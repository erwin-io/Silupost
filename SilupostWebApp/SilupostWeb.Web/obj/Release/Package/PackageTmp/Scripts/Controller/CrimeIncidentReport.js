
var crimeIncidentReportController = function() {

    var apiService = function (apiURI,apiToken) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "CrimeIncidentReport/" + Id + "/detail",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + apiToken
                }
            });
        }
        var getAddressByLegalEntityId = function (legalEntityId) {
            return $.ajax({
                url: apiURI + "CrimeIncidentReport/GetAddressByLegalEntityId?legalEntityId=" + legalEntityId,
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + apiToken
                }
            });
        }

        var getLookup = function (tableNames) {
            return $.ajax({
                url: apiURI + "SystemLookup/GetAllByTableNames?TableNames=" + tableNames,
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + apiToken
                }
            });
        }

        return {
            getById: getById,
            getAddressByLegalEntityId: getAddressByLegalEntityId,
            getLookup: getLookup
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI,app.appSettings.apiToken);

    var form,formLegalEntityAddress,dataTableCrimeIncidentReport,dataTableLegalEntityAddress;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null,
        IsAdvanceSearchMode: false
    };
    var init = function (obj) {
        initLookup();
        $(window).resize(function () {
            if ($("#table-crimeIncidentReport").hasClass('collapsed')) {
                $("#btnMore").removeClass("hidden");
            } else {
                $("#btnMore").addClass("hidden");
            }
        });
        $(document).ready(function () {
            if ($("#table-crimeIncidentReport").hasClass('collapsed')) {
                $("#btnMore").removeClass("hidden");
            } else {
                $("#btnMore").addClass("hidden");
            }
        });
    };

    var initLookup = function(){
        api.getLookup("CrimeIncidentCategory,DocReportMediaType,EntityApprovalStatus").done(function (data) {
            appSettings.lookup = [];
        	appSettings.lookup = $.extend(appSettings.lookup, data.Data);
            appSettings.FilterSearchModel = {
                lookup: appSettings.lookup,
                ApprovalStatusId: 3
            };
            var filterApprovalStatusTemplate = $.templates('#filterApprovalStatus-template');
            filterApprovalStatusTemplate.link("#filterApprovalStatusView", appSettings.FilterSearchModel);

            initAdvanceSearchMode();
            initEvent();

        });
    }

    var initAdvanceSearchMode = function(){

        var advanceSearchModeTemplate = $.templates('#advanceSearch-template');
        var date = new Date();

        var _dateFrom = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));
        var _dateTo = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));
        var _timeFrom = date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
        var _timeto = date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();

        appSettings.AdvanceSearchModel = {
            CrimeIncidentReportId: "",
            CrimeIncidentCategoryName: "",
            PostedByFullName: "",
            DateReportedFrom: moment(_dateFrom).format("MM/DD/YYYY"),
            DateReportedTo: moment(_dateTo).format("MM/DD/YYYY"),
            PossibleDateFrom: moment(_dateFrom).format("MM/DD/YYYY"),
            PossibleDateTo: moment(_dateTo).format("MM/DD/YYYY"),
            PossibleTimeFrom: _timeFrom,
            PossibleTimeTo: _timeto,
            Description: "",
            GeoStreet: "",
            GeoDistrict: "",
            GeoCityMun: "",
            GeoProvince: "",
            GeoCountry: ""
        };
        advanceSearchModeTemplate.link("#advanceSearchView", appSettings.AdvanceSearchModel);

        console.log(appSettings.AdvanceSearchModel);
        initGrid();
    }

    var initEvent = function () {
        $("#btnToggleAdvanceSearch").on("click", function(){
            if(appSettings.IsAdvanceSearchMode){
                appSettings.IsAdvanceSearchMode = false;
                $("#form-advanceSearch").addClass('hidden');
                $("#table-crimeIncidentReport_filter").removeClass('hidden');
            }
            else{
                appSettings.IsAdvanceSearchMode = true;
                $("#form-advanceSearch").removeClass('hidden');
                $("#table-crimeIncidentReport_filter").addClass('hidden');
            }
            try {
                
                dataTableCrimeIncidentReport.ajax.reload();
            }
            catch(err) {
                Swal.fire("Error", err, 'error');
            }
        });
        $("#btnAdvanceSearch").on("click", function(){
            try {
                dataTableCrimeIncidentReport.ajax.reload();
            }
            catch(err) {
                Swal.fire("Error", err, 'error');
            }
        });

        $("#ApprovalStatusId").on("change", function(){

            try {
                
                appSettings.FilterSearchModel.ApprovalStatusId = $(this).val();
                dataTableCrimeIncidentReport.ajax.reload();
            }
            catch(err) {
                Swal.fire("Error", err, 'error');
            }
        });


        $('#DateReportedFrom').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#DateReportedTo').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#PossibleDateFrom').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#PossibleDateTo').datetimepicker({
            format: 'MM/DD/YYYY'
        });

        $('#DateReportedFrom').parent().addClass('pmd-textfield-floating-label-completed');
        $('#DateReportedTo').parent().addClass('pmd-textfield-floating-label-completed');
        $('#PossibleDateFrom').parent().addClass('pmd-textfield-floating-label-completed');
        $('#PossibleDateTo').parent().addClass('pmd-textfield-floating-label-completed');

        $('#PossibleDateTo').datetimepicker({
            format: 'MM/DD/YYYY'
        })


        $("#DateReportedFrom").on("focusout", function() {
           appSettings.AdvanceSearchModel.DateReportedFrom = $(this).val();
        });
        $("#DateReportedTo").on("focusout", function() {
           appSettings.AdvanceSearchModel.DateReportedTo = $(this).val();
        });
        $("#PossibleDateFrom").on("focusout", function() {
           appSettings.AdvanceSearchModel.PossibleDateFrom = $(this).val();
        });
        $("#PossibleDateTo").on("focusout", function() {
           appSettings.AdvanceSearchModel.PossibleDateTo = $(this).val();
        });
    }

    var initGrid = function() {
        try {

            dataTableCrimeIncidentReport = $("#table-crimeIncidentReport").DataTable({
                processing: true,
                responsive: {
                    details: {
                        type: 'column',
                        target: 'tr'
                    }
                },
                columnDefs: [
                    {
                        targets: [0,9], width:1
                    },
                    {
                        className: 'control',
                        orderable: false,
                        targets:   0
                    }
                ],
                "columns": [
                    { "data": "","sortable":false, "orderable": false, "searchable": false,
                        render: function (data, type, full, meta) {
                            return '';
                        }
                    },
                    { 
                        "data": "CrimeIncidentReportId",
                        render: function (data, type, full, meta)
                        { return '<a href="/CrimeIncidentReport/Details/' + full.CrimeIncidentReportId + '">' + full.CrimeIncidentReportId + '</a>'; }
                    },
                    { "data": "PostedBySystemUser.LegalEntity.FullName" },
                    { "data": "CrimeIncidentCategory.CrimeIncidentCategoryName" },
                    { 
                        "data": "ApprovalStatus.ApprovalStatusName",
                        render: function(data, type, full, meta){
                            var badgeStatus = 'badge-warning';
                            if(full.ApprovalStatus.ApprovalStatusId === 1){
                                badgeStatus = 'badge-info';
                            }else if(full.ApprovalStatus.ApprovalStatusId === 2){
                                badgeStatus = 'badge-error';
                            }else {
                                badgeStatus = 'badge-warning';
                            }
                            return '<span class="badge '+ badgeStatus +'" style="padding: 10px">' + data + '</span>';
                        }
                    },
                    { 
                        "data": "DateReported",
                        render: function (data, type, full, meta)
                        { 
                            var date = new Date(data);
                            var newFormat =  date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()))
                            return newFormat;
                        }
                    },
                    { 
                        "data": "PossibleDate",
                        render: function (data, type, full, meta)
                        { 
                            var date = new Date(data);
                            var newFormat =  date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()))
                            return newFormat;
                        }
                    },
                    { "data": "PossibleTime" },
                    { "data": "GeoAddress" },
                    { "data": "Description" },
                ],
                "order": [[1, "asc"]],
                select: {
                    style: 'single'
                },
                bFilter: true,
                bLengthChange: true,
                "serverSide": true,
                "ajax": {
                    "url": app.appSettings.silupostWebAPIURI + "CrimeIncidentReport/GetPage",
                    "type": "GET",
                    "datatype": "json",
                    contentType: 'application/json;charset=utf-8',
                    headers: {
                        Authorization: 'Bearer ' + app.appSettings.apiToken
                    },
                    data: function (data) {
                        var dataFilter = {
                            Draw: data.draw,
                            IsAdvanceSearchMode: appSettings.IsAdvanceSearchMode,
                            Search: data.search.value,
                            PageNo: data.start <= 0 ? data.start + 1 : (data.start / data.length) + 1,//must be added to 1
                            PageSize: data.length,
                            OrderColumn: data.columns[data.order[0].column].data,
                            OrderDir: data.order[0].dir,
                            ApprovalStatusId: appSettings.FilterSearchModel.ApprovalStatusId,
                            CrimeIncidentReportId: appSettings.AdvanceSearchModel.CrimeIncidentReportId,
                            CrimeIncidentCategoryName: appSettings.AdvanceSearchModel.CrimeIncidentCategoryName,
                            PostedByFullName: appSettings.AdvanceSearchModel.PostedByFullName,
                            DateReportedFrom: appSettings.AdvanceSearchModel.DateReportedFrom,
                            DateReportedTo: appSettings.AdvanceSearchModel.DateReportedTo,
                            PossibleDateFrom: appSettings.AdvanceSearchModel.PossibleDateFrom,
                            PossibleDateTo: appSettings.AdvanceSearchModel.PossibleDateTo,
                            PossibleTimeFrom: appSettings.AdvanceSearchModel.PossibleTimeFrom,
                            PossibleTimeTo: appSettings.AdvanceSearchModel.PossibleTimeTo,
                            Description: appSettings.AdvanceSearchModel.Description,
                            GeoStreet: appSettings.AdvanceSearchModel.GeoStreet,
                            GeoDistrict: appSettings.AdvanceSearchModel.GeoDistrict,
                            GeoCityMun: appSettings.AdvanceSearchModel.GeoCityMun,
                            GeoProvince: appSettings.AdvanceSearchModel.GeoProvince,
                            GeoCountry: appSettings.AdvanceSearchModel.GeoCountry
                        }
                        console.log(dataFilter);
                        return dataFilter;
                    }
                },

                "paging": true,
                "searching": true,
                "language": {
                    "info": " _START_ - _END_ of _TOTAL_ ",
                    "sLengthMenu": "<div class='crimeIncidentReport-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
                    "sSearch": "",
                    "sSearchPlaceholder": "Search",
                    "paginate": {
                        "sNext": " ",
                        "sPrevious": " "
                    },
                },
                dom: "<'pmd-card-title'<'data-table-responsive pull-left'><'search-paper pmd-textfield'f>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'pmd-card-footer' <'pmd-datatable-pagination' l i p>>",
                "initComplete": function (settings, json) {
                    $(".crimeIncidentReport-lookup-table-length-menu select").select2({
                        theme: "bootstrap",
                        minimumResultsForSearch: Infinity,
                    });
                    circleProgress.close();
                }
            });
            $(".custom-select-action").html('<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button"><i class="material-icons pmd-sm">delete</i></button><button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button"><i class="material-icons pmd-sm">more_vert</i></button>');
            dataTableCrimeIncidentReport.columns.adjust();
        }
        catch(err) {
            console.log(err);
            Swal.fire("Error", err, 'error');
        }
    };

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var crimeIncidentReport = new crimeIncidentReportController;
