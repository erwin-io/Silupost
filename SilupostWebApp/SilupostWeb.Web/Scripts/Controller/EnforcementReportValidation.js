
var enforcementReportValidationController = function() {

    var apiService = function (apiURI) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "EnforcementReportValidation/" + Id + "/detail",
                data: { Id: Id },
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                }
            });
        }

        var getDefaultIconPic = function (Id) {
            return $.ajax({
                url: apiURI + "File/getDefaultCrimeIncidentTypeProfilePic",
                data: null,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
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
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                }
            });
        }

        return {
            getById: getById,
            getDefaultIconPic: getDefaultIconPic,
            getLookup: getLookup
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI);

    var dataTable,form;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {

        setTimeout(function () {
            initLookup();
        }, 1000);

        $(window).resize(function () {
            if ($("#table-enforcementReportValidation").hasClass('collapsed')) {
                $("#btnOpen").removeClass("hidden");
            } else {
                $("#btnOpen").addClass("hidden");
            }
        });
        $(document).ready(function () {
            if ($("#table-enforcementReportValidation").hasClass('collapsed')) {
                $("#btnOpen").removeClass("hidden");
            } else {
                $("#btnOpen").addClass("hidden");
            }
        });
    };

    var initLookup = function () {
        api.getLookup("CrimeIncidentCategory,ReportValidationStatus").done(function (data) {
            appSettings.lookup = $.extend(appSettings.lookup, data.Data);


            var advanceSearchEnforcementReportValidationTemplate = $.templates('#advanceSearchEnforcementReportValidation-template');

            var date = new Date();

            var _dateFrom = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));
            var _dateTo = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));

            appSettings.EnforcementReportValidationFilterSearchModel = {
                lookup: appSettings.lookup,
                CrimeIncidentCategoryNameFilter: "",
                EnforcementUnitName: "",
                DateSubmittedFrom: moment(_dateFrom).format("MM/DD/YYYY"),
                DateSubmittedTo: moment(_dateTo).format("MM/DD/YYYY"),
                ReportValidationFilterStatusId: 3
            };
            advanceSearchEnforcementReportValidationTemplate.link("#advanceSearchEnforcementValidationView", appSettings.EnforcementReportValidationFilterSearchModel);

            var filterReportValidationStatusTemplate = $.templates('#filterReportValidationStatus-template');
            filterReportValidationStatusTemplate.link("#filterReportValidationStatusView", appSettings.EnforcementReportValidationFilterSearchModel);

            appSettings.IsAdvanceSearchMode = false;
            initEvent();

            initGrid();
        });
    }



    var initEvent = function () {
        $("#btnOpen").on("click", OpenEnforcementReportValidation);

        $("#table-enforcementReportValidation tbody").on("click", "tr .dropdown-menu a.details", function () {
            appSettings.currentId = $(this).attr("data-value");
            OpenEnforcementReportValidation();
        });
        $("#table-enforcementReportValidation tbody").on("click", "tr .dropdown-menu a.remove", function () {
            appSettings.currentId = $(this).attr("data-value");
            Delete();
        });

        $('#table-enforcementReportValidation tbody').on('click', 'tr', function () {
            appSettings.currentId = dataTable.row(this).data().EnforcementReportValidationId;
            var isSelected = !$(this).hasClass('selected');
            if (isSelected && $("#table-enforcementReportValidation").hasClass('collapsed')) {
                $("#btnOpen").removeClass("hidden");
            } else {
                $("#btnOpen").addClass("hidden");
            }
        });


        $("#btnToggleAdvanceSearchEnforcementValidation").on("click", function () {
            if (appSettings.IsAdvanceSearchMode) {
                appSettings.IsAdvanceSearchMode = false;
                $("#form-advanceSearchEnforcementReportValidation").addClass('hidden');
                $("#table-EnforcementReportValidation_filter").removeClass('hidden');
            }
            else {
                appSettings.IsAdvanceSearchMode = true;
                $("#form-advanceSearchEnforcementReportValidation").removeClass('hidden');
                $("#table-EnforcementReportValidation_filter").addClass('hidden');
            }
            try {

                dataTable.ajax.reload();
                return;
            }
            catch (err) {
                Swal.fire("Error", err, 'error');
            }
        });


        $("#btnAdvanceSearch").on("click", function () {
            try {
                dataTable.ajax.reload();
            }
            catch (err) {
                Swal.fire("Error", err, 'error');
            }
        });

        $("#ReportValidationFilterStatusId").on("change", function () {

            try {

                appSettings.EnforcementReportValidationFilterSearchModel.ReportValidationFilterStatusId = $(this).val();
                dataTable.ajax.reload();
            }
            catch (err) {
                Swal.fire("Error", err, 'error');
            }
        });


        $('#DateSubmittedFrom').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#DateSubmittedTo').datetimepicker({
            format: 'MM/DD/YYYY'
        });

        $('#DateSubmittedFrom').parent().addClass('pmd-textfield-floating-label-completed');
        $('#DateSubmittedTo').parent().addClass('pmd-textfield-floating-label-completed');

        $("#DateSubmittedFrom").on("focusout", function () {
            appSettings.EnforcementReportValidationFilterSearchModel.DateSubmittedFrom = $(this).val();
        });
        $("#DateSubmittedTo").on("focusout", function () {
            appSettings.EnforcementReportValidationFilterSearchModel.DateSubmittedTo = $(this).val();
        });

        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
        $('.select-simple').parent().addClass('pmd-textfield-floating-label-completed');


    };

    var initGrid = function() {
        dataTable = $("#table-enforcementReportValidation").DataTable({
            processing: true,
            responsive: {
                details: {
                    type: 'column',
                    target: 'tr'
                }
            },
            columnDefs: [
                {
                    targets: [0, 1,5], width: 1
                },
                {
                    className: 'control',
                    orderable: false,
                    targets: 0
                }
            ],
            "columns": [
                {
                    "data": "", "sortable": false, "orderable": false, "searchable": false,
                    render: function (data, type, full, meta) {
                        return '';
                    }
                },
                { "data": "EnforcementReportValidationId", "sortable": true, "orderable": true, "searchable": true },
                { "data": "CrimeIncidentReport.CrimeIncidentCategory.CrimeIncidentCategoryName", },
                {
                    "data": "DateSubmitted",
                    render: function (data, type, full, meta) {
                        var date = new Date(data);
                        var newFormat = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()))
                        return newFormat;
                    }
                },
                {
                    "data": "ReportValidationStatus.ReportValidationStatusName", "sortable": false, "orderable": false, "searchable": false,
                    render: function (data, type, full, meta) {
                        var badgeStatus = 'badge-warning';
                        if (full.ReportValidationStatus.ReportValidationStatusId === 1) {
                            badgeStatus = 'badge-info';
                        } else if (full.ReportValidationStatus.ReportValidationStatusId === 2) {
                            badgeStatus = 'badge-error';
                        } else if (full.ReportValidationStatus.ReportValidationStatusId === 4) {
                            badgeStatus = 'badge-error';
                        } else {
                            badgeStatus = 'badge-warning';
                        }
                        return '<span class="badge ' + badgeStatus + '" style="padding: 10px">' + data + '</span>';
                    }
                },
                {
                    "data": null, "searchable": false, "orderable": false,
                    render: function (data, type, full, meta) {
                        return '<span class="dropdown pmd-dropdown dropup clearfix">'
                            + '<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button" id="drop-role-' + full.EnforcementReportValidationId + '" data-toggle="dropdown" aria-expanded="true">'
                            + '<i class="material-icons pmd-sm">more_vert</i>'
                            + '</button>'
                            + '<ul aria-labelledby="drop-role-' + full.EnforcementReportValidationId + '" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                            + '<li role="presentation"><a class="details" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="' + full.EnforcementReportValidationId + '" role="menuitem">Details</a></li>'
                            + '</ul>'
                            + '</span>';
                    }
                }
            ],
            "order": [[1, "asc"]],
            select: {
                style: 'single'
            },
            bFilter: true,
            bLengthChange: true,
            "serverSide": true,
            "ajax": {
                "url": app.appSettings.silupostWebAPIURI + "EnforcementReportValidation/GetTablePageByEnforcementSystemUserId",
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
                        SystemUserId: app.appSettings.appState.User.UserId,
                        CrimeIncidentCategoryName: appSettings.EnforcementReportValidationFilterSearchModel.CrimeIncidentCategoryNameFilter,
                        DateSubmittedFrom: appSettings.EnforcementReportValidationFilterSearchModel.DateSubmittedFrom,
                        DateSubmittedTo: appSettings.EnforcementReportValidationFilterSearchModel.DateSubmittedTo,
                        ReportValidationStatusId: appSettings.EnforcementReportValidationFilterSearchModel.ReportValidationFilterStatusId
                    }
                    return dataFilter;
                }
            },

            "paging": true,
            "searching": true,
            "language": {
                "info": " _START_ - _END_ of _TOTAL_ ",
                "sLengthMenu": "<div class='enforcementReportValidation-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".enforcementReportValidation-lookup-table-length-menu select").select2({
                    theme: "bootstrap",
                    minimumResultsForSearch: Infinity,
                });
                circleProgress.close();
            }
        });
        dataTable.columns.adjust();
    };

    var OpenEnforcementReportValidation = function () {
        circleProgress.show(true);
        if (appSettings.currentId !== null && appSettings.currentId !== undefined && appSettings.currentId !== "") {
            api.getById(appSettings.currentId).done(function (data) {
                appSettings.model = {
                    lookup: appSettings.lookup,
                    EnforcementReportValidationId: data.Data.EnforcementReportValidationId,
                    EnforcementUnitId: data.Data.EnforcementUnit.EnforcementUnitId,
                    EnforcementStationId: data.Data.EnforcementUnit.EnforcementStation.EnforcementStationId,
                    ValidationNotes: data.Data.ValidationNotes,
                    ReportNotes: data.Data.ReportNotes,
                    ReportValidationStatusId: data.Data.ReportValidationStatus.ReportValidationStatusId,
                    ReportValidationStatusName: data.Data.ReportValidationStatus.ReportValidationStatusName,
                    ApprovalStatusName: data.Data.CrimeIncidentReport.ApprovalStatus.ApprovalStatusName
                };
                appSettings.model.IsValidated = false;
                appSettings.model.IsRejected = false;
                appSettings.model.IsPending = false;
                appSettings.model.Canceled = false;

                appSettings.model.CanValidate = false;
                appSettings.model.CanReject = false;
                appSettings.model.IsViewOnly = false;


                appSettings.model.IsReportApproved = false;
                appSettings.model.IsReportDeclined = false;
                appSettings.model.IsReportPending = false;

                if (appSettings.model.ReportValidationStatusId === 1) {
                    appSettings.model.IsValidated = true;
                    appSettings.model.IsViewOnly = true;
                }
                if (appSettings.model.ReportValidationStatusId === 2) {
                    appSettings.model.IsRejected = true;
                    appSettings.model.IsViewOnly = true;
                }
                if (appSettings.model.ReportValidationStatusId === 3) {
                    appSettings.model.IsPending = true;
                    appSettings.model.IsViewOnly = false;
                }
                if (appSettings.model.ReportValidationStatusId === 4) {
                    appSettings.model.Canceled = true;
                    appSettings.model.IsViewOnly = true;
                }

                if (data.Data.CrimeIncidentReport.ApprovalStatus.ApprovalStatusId === 1) {
                    appSettings.model.IsViewOnly = true;
                    appSettings.model.CanValidate = false;
                    appSettings.model.CanReject = false;
                    appSettings.model.IsReportApproved = true;
                }
                else if (data.Data.CrimeIncidentReport.ApprovalStatus.ApprovalStatusId === 2) {
                    appSettings.model.IsViewOnly = true;
                    appSettings.model.CanValidate = false;
                    appSettings.model.CanReject = false;
                    appSettings.model.IsReportDeclined = true;
                }
                else {
                    appSettings.model.CanValidate = true;
                    appSettings.model.CanReject = true;
                    appSettings.model.IsReportPending = true;
                }

                if (appSettings.model.IsViewOnly) {
                    $("#modal-dialog #btnValidate").addClass("hidden");
                    $("#modal-dialog #btnReject").addClass("hidden");
                } else {
                    $("#modal-dialog #btnValidate").removeClass("hidden");
                    $("#modal-dialog #btnReject").removeClass("hidden");
                }

                var enforcementReportValidationTemplate = $.templates('#enforcementReportValidation-template');
                $("#modal-dialog").find('.modal-title').html('Submission Enforcement Report Validation');
                enforcementReportValidationTemplate.link("#modal-dialog .modal-body", appSettings.model);
                $("#modal-dialog").modal('show');

                $('.pmd-textfield').addClass('pmd-textfield-floating-label-completed');

                circleProgress.close();

                $('[data-toggle="tooltip"]').tooltip();

                $("#modal-dialog #btnShowReport").on("click", function () {
                    circleProgress.show(true);
                    var model = {
                        CrimeIncidentReportId: data.Data.CrimeIncidentReport.CrimeIncidentReportId,
                        ShowEnforcementReportValidation: false,
                        ShowApprovalControls: false,
                        ShowApprovalStatus: false,
                    };
                    $("#modal-dialog-reportView").find('.modal-title').html('Crime Incident Report Details');
                    crimeIncidentReportDetails.init(model);
                    $("#modal-dialog-reportView").modal('show');
                });


                $("#modal-dialog #btnValidate").on("click", function () {
                    appSettings.model.ReportValidationStatusId = 1;
                    form = $("#form-enforcementReportValidation");
                    form.validate({
                        ignore: [],
                        rules: {
                            ValidationNotes: {
                                required: true
                            },
                        },
                        messages: {
                            ValidationNotes: "Please enter Validation Notes",
                        },
                        errorElement: 'span',
                        errorPlacement: function (error, element) {
                            error.addClass('help-block');
                            element.closest('.form-group').append(error);
                        },
                        highlight: function (element, errorClass, validClass) {
                            $(element).closest('.form-group').addClass('has-error');
                        },
                        unhighlight: function (element, errorClass, validClass) {
                            $(element).closest('.form-group').removeClass('has-error');
                        },
                    });
                    Update();
                });

                $("#modal-dialog #btnReject").on("click", function () {
                    appSettings.model.ReportValidationStatusId = 2;
                    form = $("#form-enforcementReportValidation");
                    form.validate({
                        ignore: [],
                        rules: {
                        },
                        messages: {
                        },
                        errorElement: 'span',
                        errorPlacement: function (error, element) {
                            error.addClass('help-block');
                            element.closest('.form-group').append(error);
                        },
                        highlight: function (element, errorClass, validClass) {
                            $(element).closest('.form-group').addClass('has-error');
                        },
                        unhighlight: function (element, errorClass, validClass) {
                            $(element).closest('.form-group').removeClass('has-error');
                        },
                    });
                    Update();
                });
            });
        }
    }
    var Update = function () {
        if (!form.valid())
            return;
        var message;
        if (appSettings.model.ReportValidationStatusId === 1) {
            message = 'Set Report as Validated';
        } else if (appSettings.model.ReportValidationStatusId === 2) {
            message = 'Set Report as Rejected';
        }
        Swal.fire({
            title: message,
            text: "Do you want to continue!",
            type: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            allowOutsideClick: false
        })
        .then((result) => {
            if (result.value) {
                $(".content").find("input,button,a").prop("disabled", true).addClass("disabled");
                circleProgress.show(true);
                $.ajax({
                    url: app.appSettings.silupostWebAPIURI + "/EnforcementReportValidation/",
                    type: "PUT",
                    contentType: 'application/json;charset=utf-8',
                    data: JSON.stringify(appSettings.model),
                    dataType: "json",
                    headers: {
                        Authorization: 'Bearer ' + app.appSettings.apiToken
                    },
                    success: function (result) {
                        if (result.IsSuccess) {
                            circleProgress.close();
                            Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                dataTable.ajax.reload();
                                circleProgress.close();
                                $("#modal-dialog").modal('hide');
                            });
                        } else {
                            Swal.fire("Error!", result.Message, "error").then((result) => {
                                $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            });
                        }
                    },
                    error: function (data) {
                        var errormessage = "";
                        var errorTitle = "";
                        if (data.responseJSON.Message != null) {
                            errorTitle = "Error!";
                            errormessage = data.responseJSON.Message;
                        }
                        if (data.responseJSON.DeveloperMessage != null && data.responseJSON.DeveloperMessage.includes("Cannot insert duplicate")) {
                            errorTitle = "Not Allowed!";
                            errormessage = "Already exist!";
                        }
                        $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                        Swal.fire(errorTitle, errormessage, 'error');
                        circleProgress.close();
                    }
                });
                }
            });
    }

    return  {
        init: init
    };
}
var enforcementReportValidation = new enforcementReportValidationController;
