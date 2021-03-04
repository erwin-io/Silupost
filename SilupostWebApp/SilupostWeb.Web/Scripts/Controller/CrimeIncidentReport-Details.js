
var crimeIncidentReportDetailsController = function() {

    var apiService = function (apiURI) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "CrimeIncidentReport/" + Id + "/detail",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                }
            });
        }

        var getMediaFiles = function (fileId) {
            return $.ajax({
                url: apiURI + "File/getFile?FileId=" + fileId,
                type: "GET",
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                }
            });
        }

        var getEnforcementReportValidationById = function (Id) {
            return $.ajax({
                url: apiURI + "EnforcementReportValidation/" + Id + "/detail",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
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

        var getLookupEnforcementUnitByEnforcementStationId = function (EnforcementStationId) {
            return $.ajax({
                url: apiURI + "SystemLookup/GetEnforcementUnitByEnforcementStationId?EnforcementStationId=" + EnforcementStationId,
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
            getEnforcementReportValidationById: getEnforcementReportValidationById,
            getMediaFiles: getMediaFiles, 
            getLookup: getLookup,
            getLookupEnforcementUnitByEnforcementStationId: getLookupEnforcementUnitByEnforcementStationId
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI);

    var form, formValidationSubmissionEnforcementReport, dataTableCrimeIncidentReportMedia, datatableEnforcementReportValidation;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null,
        CanEditReportHeader:false,
        CanEditReportMedia:false,
        CanApproveReportHeader:false
    };
    var init = function (obj) {
        circleProgress.show(true);
        try {
            appSettings = $.extend(appSettings, obj);
            setTimeout(function () {
                initLookup();
            }, 1000);
        }
        catch (err) {
            Swal.fire("Error", err, 'error');
        }
    };

    var initLookup = function () {
        api.getLookup("CrimeIncidentCategory,CrimeIncidentReport,EnforcementStation,ReportValidationStatus").done(function (data) {
            appSettings.lookup = $.extend(appSettings.lookup, data.Data);
            initDetails();
        });
    }

    var iniValidation = function() {
    };

    var initEvent = function () {
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

                datatableEnforcementReportValidation.ajax.reload();
            }
            catch (err) {
                Swal.fire("Error", err, 'error');
            }
        });
        $("#btnEnforcementReportValidation").on("click", ViewEnforcementReportValidation);
        $("#btnApproved").on("click", function () {
            var model = {
                CrimeIncidentReportId: appSettings.CrimeIncidentReportId,
                ApprovalStatusId: 1
            };
            UpdateReportStatus(model, 'Approve Report');
        });

        $("#btnDeclined").on("click", function () {
            var model = {
                CrimeIncidentReportId: appSettings.CrimeIncidentReportId,
                ApprovalStatusId: 2
            };
            UpdateReportStatus(model, 'Decline Report');
        });

        $("#btnAdvanceSearch").on("click", function () {
            try {
                datatableEnforcementReportValidation.ajax.reload();
            }
            catch (err) {
                Swal.fire("Error", err, 'error');
            }
        });

        $("#ReportValidationStatusId").on("change", function () {

            try {

                appSettings.EnforcementReportValidationFilterSearchModel.ReportValidationStatusId = $(this).val();
                datatableEnforcementReportValidation.ajax.reload();
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


        $("#table-enforcementReportValidation tbody").on("click", "tr .dropdown-menu a.cancel", function () {
            api.getEnforcementReportValidationById($(this).attr("data-value")).done(function (data) {
                var model = {
                    EnforcementReportValidationId: data.Data.EnforcementReportValidationId,
                    EnforcementUnitId: data.Data.EnforcementUnit.EnforcementUnitId,
                    ValidationNotes: data.Data.ValidationNotes,
                    ReportNotes: data.Data.ReportNotes,
                    ReportValidationStatusId: 4 //canceled status
                };
                CancelSubmissionEnforcementReportValidation(model);
            });
        });

        $("#table-enforcementReportValidation tbody").on("click", "tr .dropdown-menu a.edit", function () {
            api.getEnforcementReportValidationById($(this).attr("data-value")).done(function (data) {
                var model = {
                    EnforcementReportValidationId: data.Data.EnforcementReportValidationId,
                    EnforcementUnitId: data.Data.EnforcementUnit.EnforcementUnitId,
                    EnforcementStationId: data.Data.EnforcementUnit.EnforcementStation.EnforcementStationId,
                    ValidationNotes: data.Data.ValidationNotes,
                    ReportNotes: data.Data.ReportNotes,
                    ReportValidationStatusId: data.Data.ReportValidationStatus.ReportValidationStatusId, //canceled status
                    IsViewOnly: false
                };
                api.getLookupEnforcementUnitByEnforcementStationId(model.EnforcementStationId).done(function (data) {
                    appSettings.lookup.EnforcementUnit = data.Data.EnforcementUnit;
                    EditEnforcementReportValidation(model);
                });
            });
        });

        $("#table-enforcementReportValidation tbody").on("click", "tr .dropdown-menu a.view", function () {
            api.getEnforcementReportValidationById($(this).attr("data-value")).done(function (data) {
                var model = {
                    EnforcementReportValidationId: data.Data.EnforcementReportValidationId,
                    EnforcementUnitId: data.Data.EnforcementUnit.EnforcementUnitId,
                    EnforcementStationId: data.Data.EnforcementUnit.EnforcementStation.EnforcementStationId,
                    ValidationNotes: data.Data.ValidationNotes,
                    ReportNotes: data.Data.ReportNotes,
                    ReportValidationStatusId: data.Data.ReportValidationStatus.ReportValidationStatusId, //canceled status
                    IsViewOnly: true
                };
                api.getLookupEnforcementUnitByEnforcementStationId(model.EnforcementStationId).done(function (data) {
                    //appSettings.lookup = $.extend(appSettings.lookup, data.Data);
                    appSettings.lookup.EnforcementUnit = data.Data.EnforcementUnit;
                    EditEnforcementReportValidation(model);
                });
            });
        });

        $("#btnNewEnforcementReportValidation").on("click", AddEnforcementReportValidation);

    }

    var initDetails = function(){
        api.getById(appSettings.CrimeIncidentReportId).done(function (data) {

            var crimeIncidentReportDetailsTemplate = $.templates('#crimeIncidentReport-template');

            // appSettings.model = data.Data;
            appSettings.model = $.extend(appSettings.model, data.Data);
            appSettings.model = $.extend(appSettings.model, data.Data.PostedBySystemUser);
            appSettings.model = $.extend(appSettings.model, data.Data.PostedBySystemUser.LegalEntity);
            appSettings.model = $.extend(appSettings.model, data.Data.ApprovalStatus);
            appSettings.model = $.extend(appSettings.model, data.Data.CrimeIncidentCategory);
            appSettings.model = $.extend(appSettings.model, data.Data.CrimeIncidentCategory.CrimeIncidentType);
            appSettings.model.CanEditReportHeader = appSettings.CanEditReportHeader;
            appSettings.model.CanEditReportMedia = appSettings.CanEditReportMedia;
            appSettings.model.CanApproveReportHeader = appSettings.CanApproveReportHeader;
            appSettings.model.ShowEnforcementReportValidation = appSettings.ShowEnforcementReportValidation;
            appSettings.model.ShowApprovalControls = appSettings.ShowApprovalControls;
            appSettings.model.ShowApprovalStatus = appSettings.ShowApprovalStatus;
            //appSettings.model.Validated = false;


            appSettings.model.IsPending = false;
            appSettings.model.IsDeclined = false;
            appSettings.model.IsApproved = false;

            appSettings.model.AllowedToApproveReport = false;
            appSettings.model.AllowedToDeclineReport = false;
            appSettings.model.AllowedToCancelReport = false;

            appSettings.model.PossibleTime = moment(appSettings.model.PossibleTime, ["HH:mm"]).format("h: mm A");
            if (data.Data.ApprovalStatus.ApprovalStatusId === 1) {
                appSettings.model.IsApproved = true;
                appSettings.model.CanApprove = false;
                appSettings.model.CanDecline = false;
                appSettings.model.CanSubmissionEnforcementReportValidation = false;
            }
            else if (data.Data.ApprovalStatus.ApprovalStatusId === 2) {
                appSettings.model.IsDeclined = true;
                appSettings.model.CanApprove = false;
                appSettings.model.CanDecline = false;
                appSettings.model.CanSubmissionEnforcementReportValidation = false;
            }
            else {
                appSettings.model.IsPending = true;
                appSettings.model.CanApprove = appSettings.model.Validated;
                appSettings.model.CanDecline = true;
                appSettings.model.CanSubmissionEnforcementReportValidation = (!appSettings.model.Validated);
            }
            appSettings.model.lookup = appSettings.lookup;


            appSettings.model.AllowedToApproveReport = app.appSettings.appState.Privileges.filter(p => p.SystemWebAdminPrivilegeId === 2).length > 0;
            appSettings.model.AllowedToDeclineReport = app.appSettings.appState.Privileges.filter(p => p.SystemWebAdminPrivilegeId === 3).length > 0;
            appSettings.model.AllowedToCancelReport = app.appSettings.appState.Privileges.filter(p => p.SystemWebAdminPrivilegeId === 6).length > 0;


            $(".select-simple").select2({
                theme: "bootstrap",
                minimumResultsForSearch: Infinity,
            });
            let promises = [];
            for (var i in appSettings.model.CrimeIncidentReportMedia) {
                if (appSettings.model.CrimeIncidentReportMedia[i].File !== undefined)
                    appSettings.model.CrimeIncidentReportMedia[i].File.FileURL = app.appSettings.silupostWebAPIURI + "File/getFile?FileId=" + appSettings.model.CrimeIncidentReportMedia[i].File.FileId;
            }
            crimeIncidentReportDetailsTemplate.link("#reportView", appSettings.model);
            initCrimeIncidentReportMediaGallery();  

            appSettings.ShowEnforcementReportValidation

            if (appSettings.model.ShowEnforcementReportValidation) {

                var advanceSearchEnforcementReportValidationTemplate = $.templates('#advanceSearchEnforcementReportValidation-template');

                var date = new Date();

                var _dateFrom = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));
                var _dateTo = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));

                appSettings.EnforcementReportValidationFilterSearchModel = {
                    lookup: appSettings.lookup,
                    CrimeIncidentReportId: appSettings.CrimeIncidentReportId,
                    EnforcementUnitName: "",
                    DateSubmittedFrom: moment(_dateFrom).format("MM/DD/YYYY"),
                    DateSubmittedTo: moment(_dateTo).format("MM/DD/YYYY"),
                    ReportValidationStatusId: 3
                };
                advanceSearchEnforcementReportValidationTemplate.link("#advanceSearchEnforcementValidationView", appSettings.EnforcementReportValidationFilterSearchModel);

                var filterReportValidationStatusTemplate = $.templates('#filterReportValidationStatus-template');
                filterReportValidationStatusTemplate.link("#filterReportValidationStatusView", appSettings.EnforcementReportValidationFilterSearchModel);

                appSettings.IsAdvanceSearchMode = false;
                initGridEnforcementReportValidation();
            }
            initEvent();
            circleProgress.close();
            $('[data-toggle="tooltip"]').tooltip();
        });
    }

    var initCrimeIncidentReportMediaGallery = function() {
        let modalId = $('#image-gallery');

        $(document)
          .ready(function () {

            loadGallery(true, 'a.thumbnail');

            //This function disables buttons when needed
            function disableButtons(counter_max, counter_current) {
              $('#show-previous-image, #show-next-image')
                .show();
              if (counter_max === counter_current) {
                $('#show-next-image')
                  .hide();
              } else if (counter_current === 1) {
                $('#show-previous-image')
                  .hide();
              }
            }

            /**
             *
             * @param setIDs        Sets IDs when DOM is loaded. If using a PHP counter, set to false.
             * @param setClickAttr  Sets the attribute for the click handler.
             */

            function loadGallery(setIDs, setClickAttr) {
              let current_image,
                selector,
                counter = 0;

              $('#show-next-image, #show-previous-image')
                .click(function () {
                  if ($(this)
                    .attr('id') === 'show-previous-image') {
                    current_image--;
                  } else {
                    current_image++;
                  }

                  selector = $('[data-image-id="' + current_image + '"]');
                  updateGallery(selector);
                });

              function updateGallery(selector) {
                let $sel = selector;
                current_image = $sel.data('image-id');
                $('#image-gallery-title')
                  .text($sel.data('title'));
                $('#image-gallery-image')
                  .attr('src', $sel.data('image'));
                disableButtons(counter, $sel.data('image-id'));
              }

              if (setIDs == true) {
                $('[data-image-id]')
                  .each(function () {
                    counter++;
                    $(this)
                      .attr('data-image-id', counter);
                  });
              }
              $(setClickAttr)
                .on('click', function () {
                  updateGallery($(this));
                });
            }
          });

        // build key actions
        $(document)
          .keydown(function (e) {
            switch (e.which) {
              case 37: // left
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-previous-image').is(":visible")) {
                  $('#show-previous-image')
                    .click();
                }
                break;

              case 39: // right
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-next-image').is(":visible")) {
                  $('#show-next-image')
                    .click();
                }
                break;

              default:
                return; // exit this handler for other keys
            }
            e.preventDefault(); // prevent the default action (scroll / move caret)
          });

    };

    var ViewEnforcementReportValidation = function () {
        $("#modal-dialog-EnforcementReportValidation").find('.modal-title').html('Enforcement Report Validation');
        $("#modal-dialog-EnforcementReportValidation").modal('show');
        $("body").addClass("modal-open");
        if (!appSettings.model.CanSubmissionEnforcementReportValidation) {
            appSettings.EnforcementReportValidationFilterSearchModel.ReportValidationStatusId = 1;
            $("#btnNewEnforcementReportValidation").attr("disabled", true)
        }
    }

    var initGridEnforcementReportValidation = function () {
        if (datatableEnforcementReportValidation !== undefined && datatableEnforcementReportValidation !== null) {
            return;
        }
        datatableEnforcementReportValidation = $("#table-enforcementReportValidation").DataTable({
            processing: true,
            responsive: {
                details: {
                    type: 'column',
                    target: 'tr'
                }
            },
            columnDefs: [
                {
                    targets: [0,2], width: 1
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
                { "data": "EnforcementUnit.LegalEntity.FullName" },
                {
                    "data": "DateSubmitted",
                    render: function (data, type, full, meta) {
                        var date = new Date(data);
                        var newFormat = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));
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
                        var control = '';
                        if (full.ReportValidationStatus.ReportValidationStatusId === 3) {
                            if (appSettings.model.AllowedToCancelReport) {
                                control = '<span class="dropdown pmd-dropdown dropup clearfix">'
                                    + '<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button" id="drop-role-' + full.EnforcementReportValidationId + '" data-toggle="dropdown" aria-expanded="true">'
                                    + '<i class="material-icons pmd-sm">more_vert</i>'
                                    + '</button>'
                                    + '<ul aria-labelledby="drop-role-' + full.EnforcementReportValidationId + '" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                    + '<li role="presentation"><a class="edit" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="' + full.EnforcementReportValidationId + '" role="menuitem">Edit</a></li>'
                                    + '<li role="presentation"><a class="cancel" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="' + full.EnforcementReportValidationId + '" role="menuitem">Cancel</a></li>'
                                    + '</ul>'
                                    + '</span>';
                            } else {
                                control = '<span class="dropdown pmd-dropdown dropup clearfix">'
                                    + '<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button" id="drop-role-' + full.EnforcementReportValidationId + '" data-toggle="dropdown" aria-expanded="true">'
                                    + '<i class="material-icons pmd-sm">more_vert</i>'
                                    + '</button>'
                                    + '<ul aria-labelledby="drop-role-' + full.EnforcementReportValidationId + '" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                    + '<li role="presentation"><a class="edit" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="' + full.EnforcementReportValidationId + '" role="menuitem">Edit</a></li>'
                                    + '</ul>'
                                    + '</span>';
                            }
                        }
                        else
                            control = '<span class="dropdown pmd-dropdown dropup clearfix">'
                                + '<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button" id="drop-role-' + full.EnforcementReportValidationId + '" data-toggle="dropdown" aria-expanded="true">'
                                + '<i class="material-icons pmd-sm">more_vert</i>'
                                + '</button>'
                                + '<ul aria-labelledby="drop-role-' + full.EnforcementReportValidationId + '" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                + '<li role="presentation"><a class="view" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="' + full.EnforcementReportValidationId + '" role="menuitem">Details</a></li>'
                                + '</ul>'
                                + '</span>';
                        return control
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
                "url": app.appSettings.silupostWebAPIURI + "EnforcementReportValidation/GetTablePageByCrimeIncidentReportId",
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
                        CrimeIncidentReportId: appSettings.EnforcementReportValidationFilterSearchModel.CrimeIncidentReportId,
                        EnforcementUnitName: appSettings.EnforcementReportValidationFilterSearchModel.EnforcementUnitName,
                        DateSubmittedFrom: appSettings.EnforcementReportValidationFilterSearchModel.DateSubmittedFrom,
                        DateSubmittedTo: appSettings.EnforcementReportValidationFilterSearchModel.DateSubmittedTo,
                        ReportValidationStatusId: appSettings.EnforcementReportValidationFilterSearchModel.ReportValidationStatusId
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
        $(".custom-select-action").html('<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button"><i class="material-icons pmd-sm">delete</i></button><button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button"><i class="material-icons pmd-sm">more_vert</i></button>');
        datatableEnforcementReportValidation.columns.adjust();
    }

    var UpdateReportStatus = function (model, message) {
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
                    url: app.appSettings.silupostWebAPIURI + "/CrimeIncidentReport/UpdateStatus",
                    type: "PUT",
                    contentType: 'application/json;charset=utf-8',
                    data: JSON.stringify(model),
                    dataType: "json",
                    headers: {
                        Authorization: 'Bearer ' + app.appSettings.apiToken
                    },
                    success: function (result) {
                        if (result.IsSuccess) {
                            circleProgress.close();
                            Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                circleProgress.close();
                                initDetails();
                            });
                        } else {
                            Swal.fire("Error!", result.Message, "error").then((result) => {
                                $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            });
                        }
                    },
                    error: function (errormessage) {
                        $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                        Swal.fire('Error!', errormessage.Message, 'error');
                        circleProgress.close();
                    }
                });
            }
        });
    }

    var AddEnforcementReportValidation = function (model) {
        appSettings.lookup.EnforcementUnit = [];
        appSettings.EnforcementReportValidationModel = {
            CrimeIncidentReportId: appSettings.CrimeIncidentReportId,
            lookup: appSettings.lookup,
            IsViewOnly: false
        };
        var manageEnforcementReportValidationTemplate = $.templates('#manageEnforcementReportValidation-template');
        $("#modal-dialog-ManageEnforcementReportValidation").find('.modal-title').html('New Submission Enforcement Report Validation');
        $("#modal-dialog-ManageEnforcementReportValidation").find('.modal-footer #btnSave').html('Save');
        $("#modal-dialog-ManageEnforcementReportValidation").find('.modal-footer #btnSave').attr("data-name", "Save");
        manageEnforcementReportValidationTemplate.link("#modal-dialog-ManageEnforcementReportValidation .modal-body", appSettings.EnforcementReportValidationModel);
        $("#modal-dialog-ManageEnforcementReportValidation").modal('show');

        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
        $('.pmd-textfield').addClass('pmd-textfield-floating-label-completed');
        formValidationSubmissionEnforcementReport = $("#form-newEnforcementReportValidation");
        InitFormValidationSubmissionEnforcementReport();

        $("#EnforcementStationId").on("change", EnforcementStationCahnge);
        $("#EnforcementUnitId").on("change", function () {
            appSettings.EnforcementReportValidationModel.EnforcementUnitId = $(this).val();
        });
        $("#modal-dialog-ManageEnforcementReportValidation #btnSave").on("click", CreateSubmissionEnforcementReportValidation);
    }

    var EditEnforcementReportValidation = function (model) {
        appSettings.EnforcementReportValidationModel = model;
        appSettings.EnforcementReportValidationModel.lookup = appSettings.lookup; 
        var manageEnforcementReportValidationTemplate = $.templates('#manageEnforcementReportValidation-template');
        $("#modal-dialog-ManageEnforcementReportValidation").find('.modal-title').html('Edit Submission Enforcement Report Validation');
        $("#modal-dialog-ManageEnforcementReportValidation").find('.modal-footer #btnSave').html('Update');
        $("#modal-dialog-ManageEnforcementReportValidation").find('.modal-footer #btnSave').attr("data-name", "Update");
        manageEnforcementReportValidationTemplate.link("#modal-dialog-ManageEnforcementReportValidation .modal-body", appSettings.EnforcementReportValidationModel);
        $("#modal-dialog-ManageEnforcementReportValidation").modal('show');

        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
        $('.pmd-textfield').addClass('pmd-textfield-floating-label-completed');
        formValidationSubmissionEnforcementReport = $("#form-newEnforcementReportValidation");
        InitFormValidationSubmissionEnforcementReport();

        $("#EnforcementStationId").on("change", EnforcementStationCahnge);
        $("#EnforcementUnitId").on("change", function () {
            appSettings.EnforcementReportValidationModel.EnforcementUnitId = $(this).val();
        });
        $("#modal-dialog-ManageEnforcementReportValidation #btnSave").on("click", UpdateSubmissionEnforcementReportValidation);
    }

    var EnforcementStationCahnge = function () {
        var enforcementStationId = $(this).val();
        var manageEnforcementReportValidationTemplate = $.templates('#manageEnforcementReportValidation-template');
        api.getLookupEnforcementUnitByEnforcementStationId(enforcementStationId).done(function (data) {
            appSettings.lookup.EnforcementUnit = data.Data.EnforcementUnit;
            var model = {
                lookup: appSettings.lookup,
                CrimeIncidentReportId: appSettings.CrimeIncidentReportId,
                EnforcementReportValidationId: appSettings.EnforcementReportValidationModel.EnforcementReportValidationId,
                EnforcementStationId: enforcementStationId,
                ValidationNotes: appSettings.EnforcementReportValidationModel.ValidationNotes,
                ReportNotes: appSettings.EnforcementReportValidationModel.ReportNotes,
                ReportValidationStatusId: appSettings.EnforcementReportValidationModel.ReportValidationStatusId,
                IsViewOnly: false
            }
            appSettings.EnforcementReportValidationModel = model;
            manageEnforcementReportValidationTemplate.link("#modal-dialog-ManageEnforcementReportValidation .modal-body", appSettings.EnforcementReportValidationModel);


            $("#EnforcementStationId").on("change", EnforcementStationCahnge);
            $(".select-simple").select2({
                theme: "bootstrap",
                minimumResultsForSearch: Infinity,
            });
            $('.pmd-textfield').addClass('pmd-textfield-floating-label-completed');
            $("#EnforcementUnitId").on("change", function () {
                appSettings.EnforcementReportValidationModel.EnforcementUnitId = $(this).val();
            });
            formValidationSubmissionEnforcementReport = $("#form-newEnforcementReportValidation");
            InitFormValidationSubmissionEnforcementReport();
        });
    }

    var CancelSubmissionEnforcementReportValidation = function (model) {
        Swal.fire({
            title: 'Cancel Submission of Enforcement Report Validation',
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
                        data: JSON.stringify(model),
                        dataType: "json",
                        headers: {
                            Authorization: 'Bearer ' + app.appSettings.apiToken
                        },
                        success: function (result) {
                            if (result.IsSuccess) {
                                circleProgress.close();
                                Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    datatableEnforcementReportValidation.ajax.reload();
                                    circleProgress.close();
                                });
                            } else {
                                Swal.fire("Error!", result.Message, "error").then((result) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                });
                            }
                        },
                        error: function (errormessage) {
                            $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            Swal.fire('Error!', errormessage.Message, 'error');
                            circleProgress.close();
                        }
                    });
                }
            });
    }

    var CreateSubmissionEnforcementReportValidation = function () {
        if (!formValidationSubmissionEnforcementReport.valid())
            return;
        Swal.fire({
            title: 'Save New Submission of Enforcement Report Validation',
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
                        type: "POST",
                        contentType: 'application/json;charset=utf-8',
                        data: JSON.stringify(appSettings.EnforcementReportValidationModel),
                        dataType: "json",
                        headers: {
                            Authorization: 'Bearer ' + app.appSettings.apiToken
                        },
                        success: function (result) {
                            if (result.IsSuccess) {
                                circleProgress.close();
                                Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    datatableEnforcementReportValidation.ajax.reload();
                                    circleProgress.close();
                                    $("#modal-dialog-ManageEnforcementReportValidation").modal('hide');
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

    var UpdateSubmissionEnforcementReportValidation = function () {
        if (!formValidationSubmissionEnforcementReport.valid())
            return;
        Swal.fire({
            title: 'Update Submission of Enforcement Report Validation',
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
                        data: JSON.stringify(appSettings.EnforcementReportValidationModel),
                        dataType: "json",
                        headers: {
                            Authorization: 'Bearer ' + app.appSettings.apiToken
                        },
                        success: function (result) {
                            if (result.IsSuccess) {
                                circleProgress.close();
                                Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    datatableEnforcementReportValidation.ajax.reload();
                                    circleProgress.close();
                                    $("#modal-dialog-ManageEnforcementReportValidation").modal('hide');
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

    var InitFormValidationSubmissionEnforcementReport = function () {
        formValidationSubmissionEnforcementReport.validate({
            ignore: [],
            rules: {
                EnforcementStationId: {
                    required: false
                },
                EnforcementUnitId: {
                    required: true
                },
            },
            messages: {
                EnforcementReportValidationId: "Please enter Enforcement Report Validation",
                EnforcementStationId: "Please Select Enforcement Station",
                EnforcementUnitId: "Please Select Enforcement Unit",
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
    }

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var crimeIncidentReportDetails = new crimeIncidentReportDetailsController;
