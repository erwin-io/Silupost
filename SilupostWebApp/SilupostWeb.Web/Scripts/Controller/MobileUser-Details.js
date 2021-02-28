
var mobileUserDetailsController = function() {

    var apiService = function (apiURI) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "SystemUser/" + Id + "/detail",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                }
            });
        }
        var getTrackerStatusUserById = function (Id) {
            return $.ajax({
                url: apiURI + "SystemUser/" + Id + "/TrackerStatus",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                }
            });
        }
        var getAddressByLegalEntityId = function (legalEntityId) {
            return $.ajax({
                url: apiURI + "SystemUser/GetAddressByLegalEntityId?legalEntityId=" + legalEntityId,
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
        var getDefaultProfilePic = function (Id) {
            return $.ajax({
                url: apiURI + "File/getDefaultSystemUserProfilePic",
                data: null,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                }
            });
        }

        return {
            getById: getById,
            getTrackerStatusUserById: getTrackerStatusUserById,
            getAddressByLegalEntityId: getAddressByLegalEntityId,
            getLookup: getLookup,
            getDefaultProfilePic: getDefaultProfilePic
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI);

    var form, dataTableCrimeIncidentReport,dataTableLegalEntityAddress;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {
        appSettings = $.extend(appSettings, obj);
        circleProgress.show(true);
        initEvent();
        appSettings.UserInTable = [];
        setTimeout(function () {
            initDefaultProfilePic();
            initLookup();
            setTimeout(function () {
                initDetails();
            }, 1000);
        }, 1000);

        legalEntity.appSettings.forms = {
	        Rules: {
                FirstName: {
                    required: true
                },
                MiddleName: {
                    required: false
                },
                LastName: {
                    required: true
                },
                BirthDate: {
                    required: true
                },
                GenderId: {
                    required: true
                },
                EmailAddress: {
                    required: true,
                    emailCheck: function(){
                        return true;
                    }
                },
                MobileNumber: {
                    required: true,
                    digits: true
                }
            },
            Messages: {
                FirstName: "Please enter a Firstname",
                MiddleName: "Please enter Middlename",
                LastName: "Please enter Lastname",
                BirthDate: "Please select Birth Date",
                GenderId: "Please select Gender",
                EmailAddress: {
                	required: "Please enter Email Address",
                    emailCheck : "Please enter valid email",
                },
                MobileNumber: {
                	required: "Please enter Mobile Number",
                    digits : "Please enter valid Mobile Number",
                }
            },
        }


        $(window).resize(function () {
            if ($("#table-mobileUser").hasClass('collapsed')) {
                $("#btnView").removeClass("hidden");
            } else {
                $("#btnView").addClass("hidden");
            }
        });
        $(document).ready(function () {
            if ($("#table-mobileUser").hasClass('collapsed')) {
                $("#btnView").removeClass("hidden");
            } else {
                $("#btnView").addClass("hidden");
            }
        });
    };

    var initUserStatus = function () {
        api.getTrackerStatusUserById(appSettings.SystemUserId).done(function (data) {
            if (data.Data.HasFirstLogin) {
                var dateTimeNow = new Date();
                var lastActive = new Date(data.Data.LasteDateTimeActive);
                var diff = Math.round((((lastActive - dateTimeNow) % 86400000) % 3600000) / 60000);
                var isActive = diff > -1;
                var onlineIdentifier = "#online-identifier";
                if (isActive) {
                    $(onlineIdentifier).addClass("online");
                } else {
                    $(onlineIdentifier).removeClass("online");
                }
            }
        });
    }

    var initUserConfig = function () {
        if (appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport) {
            $("#btnBlockUserFromPostingReport").text("Block user account from posting report");
        } else {
            $("#btnBlockUserFromPostingReport").text("Unblock user account from posting report");
        }
        if (appSettings.model.SystemUserConfig.IsUserEnable) {
            $("#btnDisableUser").text("Disable user account");
        } else {
            $("#btnDisableUser").text("Enable user account");
        }
    }

    var initDefaultProfilePic = function () {
        api.getDefaultProfilePic().done(function (data) {
            appSettings.DefaultProfilePic = data.Data;
        });
    }

    var initLookup = function(){
        api.getLookup("SystemWebAdminRole,EntityGender").done(function (data) {
        	appSettings.lookup = $.extend(appSettings.lookup, data.Data);
        });
    }

    var iniValidation = function() {
        form.validate({
            ignore:[],
            rules: {
                UserName: {
                    required: true,
                    minlength: 4,
                },
                Password: {
                    required: function(){
                        return appSettings.model.IsNew;
                    },
                    minlength: function(){
                        var min = appSettings.model.IsNew ? 6 : 0;
                        return min;
                    },
                    pwcheck: function(){
                        return appSettings.model.IsNew;
                    }
                },
                ConfirmPassword: {
                    required: function(){
                        return appSettings.model.IsNew;
                    },
                    equalTo: function(){
                        return appSettings.model.IsNew ? "#Password" : "";
                    }
                }
            },
            messages: {
                UserName: {
                    required: "Please enter Username",
                    minlength: $.validator.format("Please enter at least {0} characters."),
                },
                Password : {
                    required : "Please enter Password",
                    minlength : $.validator.format("Please enter at least {0} characters."),
                    pwcheck : "This field must consists of the following : uppercase, uowercase, digit and special characters",
                },
                ConfirmPassword: {
                    required: "Please Confirm Password",
                    equalTo: "Password not match"
                }
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
        formSystemWebAdminUserRoles.validate({
            ignore:[],
            rules: {
                SystemWebAdminUserRoles: {
                    required: true,
                    minlength: 1
                }
            },
            messages: {
                SystemWebAdminUserRoles: {
                    required: "Please select System Web Admin Role"
                }
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
        $.validator.addMethod("pwcheck", function(value) {
           return /^[A-Za-z0-9\d=!\-@._*]*$/.test(value) // consists of only these
               && /[a-z]/.test(value) // has a lowercase letter
               && /[A-Z]/.test(value) // has a uppercase letter
               && /\d/.test(value) // has a digit
        });
        $.validator.addMethod("emailCheck", function(value) {
           return /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/.test(value) // consists of only these
        });
    };

    var initEvent = function () {
        $("#btnBlockUserFromPostingReport").on("click", function () {
            var message = "Block User account from Posting Report?";
            var successMessage = "User account successfully blocked!";
            var configValue = false;
            if (!appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport) {
                message = "Unblock User account from Posting Report?";
                successMessage = "User account successfully unblocked!";
                configValue = true;
            }
            appSettings.model.SystemUserConfig.SystemUserId = appSettings.model.SystemUserId;
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
                    appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport = configValue;
                    $(".content").find("input,button,a").prop("disabled", true).addClass("disabled");
                    var target = $(this);
                    circleProgress.show(true);
                    $.ajax({
                        url: app.appSettings.silupostWebAPIURI + "/SystemUser/UpdateSystemUserConfig",
                        type: "PUT",
                        dataType: "json",
                        contentType: 'application/json;charset=utf-8',
                        data: JSON.stringify(appSettings.model.SystemUserConfig),
                        headers: {
                            Authorization: 'Bearer ' + app.appSettings.apiToken
                        },
                        success: function (result) {
                            if (result.IsSuccess) {
                                circleProgress.close();
                                Swal.fire("Success!", successMessage, "success").then((prompt) => {
                                    circleProgress.show(true);
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    circleProgress.close();
                                    if (!appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport) {
                                        $("#btn-alert-BlockUserFromPostingReport").click();
                                    }
                                });
                            } else {
                                Swal.fire("Error!", result.Message, "error").then((result) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                });
                                if (configValue) {
                                    appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport = false;
                                } else {
                                    appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport = true;
                                }
                            }
                            initUserConfig();
                        },
                        error: function (data) {
                            var errormessage = "";
                            var errorTitle = "";
                            if (data.responseJSON.Message != null) {
                                erroTitle = "Error!";
                                errormessage = data.responseJSON.Message;
                            }
                            $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            Swal.fire(erroTitle, errormessage, 'error');
                            circleProgress.close();
                            appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport = true;
                            initUserConfig();
                        }
                    });
                }
            });
        });
        $("#btnDisableUser").on("click", function () {
            var message = "Disable User account?";
            var successMessage = "User account successfully disabled!";
            var configValue = false;
            if (!appSettings.model.SystemUserConfig.IsUserEnable) {
                message = "Enable User account?";
                successMessage = "User account successfully enabled!";
                configValue = true;
            }
            appSettings.model.SystemUserConfig.SystemUserId = appSettings.model.SystemUserId;
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
                        appSettings.model.SystemUserConfig.IsUserEnable = configValue;
                        $(".content").find("input,button,a").prop("disabled", true).addClass("disabled");
                        var target = $(this);
                        circleProgress.show(true);
                        $.ajax({
                            url: app.appSettings.silupostWebAPIURI + "/SystemUser/UpdateSystemUserConfig",
                            type: "PUT",
                            dataType: "json",
                            contentType: 'application/json;charset=utf-8',
                            data: JSON.stringify(appSettings.model.SystemUserConfig),
                            headers: {
                                Authorization: 'Bearer ' + app.appSettings.apiToken
                            },
                            success: function (result) {
                                if (result.IsSuccess) {
                                    circleProgress.close();
                                    Swal.fire("Success!", successMessage, "success").then((prompt) => {
                                        circleProgress.show(true);
                                        $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                        circleProgress.close();
                                        if (!appSettings.model.SystemUserConfig.IsUserEnable) {
                                            $("#btn-alert-DisableUser").click();
                                        }
                                    });
                                } else {
                                    Swal.fire("Error!", result.Message, "error").then((result) => {
                                        $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    });
                                    if (configValue) {
                                        appSettings.model.SystemUserConfig.IsUserEnable = false;
                                    } else {
                                        appSettings.model.SystemUserConfig.IsUserEnable = true;
                                    }
                                }
                                initUserConfig();
                            },
                            error: function (data) {
                                var errormessage = "";
                                var errorTitle = "";
                                if (data.responseJSON.Message != null) {
                                    erroTitle = "Error!";
                                    errormessage = data.responseJSON.Message;
                                }
                                $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                Swal.fire(erroTitle, errormessage, 'error');
                                circleProgress.close();
                                if (configValue) {
                                    appSettings.model.SystemUserConfig.IsUserEnable = false;
                                } else {
                                    appSettings.model.SystemUserConfig.IsUserEnable = true;
                                }
                                initUserConfig();
                            }
                        });
                    }
                });
        });
    }

    var initGridLegalEntityAddress = function() {
        dataTableLegalEntityAddress = $("#table-legalEntityAddress").DataTable({
            processing: true,
            responsive: false,
            columnDefs: [
                {
                    targets: 0, className:"hidden",
                },
                {
                	className: 'control',
					orderable: false,
					targets:   1
				}
            ],
            "columns": [
                { "data": "LegalEntityAddressId","sortable":false, "orderable": false, "searchable": false},
                { "data": "Address" },
            ],
            "order": [[1, "asc"]],
            select: {
                style: 'single'
            },
            bFilter: true,
            bLengthChange: true,

            "paging": true,
            "searching": true,
            "language": {
                "info": " _START_ - _END_ of _TOTAL_ ",
                "sLengthMenu": "<div class='legalEntityAddress-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".legalEntityAddress-lookup-table-length-menu select").select2({
                    theme: "bootstrap",
                    minimumResultsForSearch: Infinity,
                });
            }
        });
        dataTableLegalEntityAddress.columns.adjust();
    };

    var initDetails = function () {
        api.getById(appSettings.SystemUserId).done(function (data) {
            var mobileUserTemplate = $.templates('#mobileUser-template');
            appSettings.model = {
                SystemUserId: data.Data.SystemUserId,
                UserName: data.Data.UserName,
                Password: data.Data.Password,
                ConfirmPassword: data.Data.Password,
                SystemUserConfig: data.Data.SystemUserConfig
            };
            appSettings.model = $.extend(appSettings.model, data.Data.LegalEntity);
            appSettings.model.BirthDate = moment(data.Data.LegalEntity.BirthDate).format("MM/DD/YYYY");
            appSettings.model.GenderId = data.Data.LegalEntity.Gender.GenderId;
            appSettings.model.ProfilePicture = data.Data.ProfilePicture;
            if (appSettings.model.ProfilePicture === null) {
                appSettings.model.ProfilePicture = {
                    FileId: appSettings.DefaultProfilePic.FileId,
                    FileName: appSettings.DefaultProfilePic.FileName,
                    MimeType: appSettings.DefaultProfilePic.MimeType,
                    FileSize: appSettings.DefaultProfilePic.FileSize,
                    FileContent: appSettings.DefaultProfilePic.FileContent,
                    IsDefault: true,
                    FileFromBase64String: appSettings.DefaultProfilePic.FileContent
                }
            }
            else {
                appSettings.model.ProfilePicture.FileFromBase64String = appSettings.model.ProfilePicture.FileContent;
            }

            appSettings.model.ProfilePicture.FileData = 'data:' + appSettings.model.ProfilePicture.MimeType + ';base64,' + appSettings.model.ProfilePicture.FileContent;

            appSettings.model.lookup = {
                EntityGender: appSettings.lookup.EntityGender,
                SystemWebAdminRole: []
            };

            //render template
            mobileUserTemplate.link("#userView", appSettings.model);
            //end render template

            $(".select-tags").select2({
                tags: false,
                theme: "bootstrap",
            });
            $("#userView").find("input,textarea").attr("readonly", "");
            $("#userView").find("button,select").attr("disabled", "");


            if (!appSettings.model.SystemUserConfig.IsUserAllowToPostNextReport) {
                $("#btn-alert-BlockUserFromPostingReport").click();
            }
            if (!appSettings.model.SystemUserConfig.IsUserEnable) {
                $("#btn-alert-DisableUser").click();
            }

            //init form validation
            legalEntity.init();
            //end init form
            circleProgress.close();
            initUserConfig();
            appSettings.model.LegalEntityAddress = [];
            initGridLegalEntityAddress();
            LoadSystemUserAddress();
            initCrimeReportGrid();
            setTimeout(initUserStatus, 1000);
        });
    }

    var LoadSystemUserAddress = function(){
        appSettings.model.LegalEntityAddress = [];
        dataTableLegalEntityAddress.clear().draw();
        api.getAddressByLegalEntityId(appSettings.model.LegalEntityId).done(function (data) {
            for(var i in data.Data){
                appSettings.LegalEntityAddressModel = {
                    LegalEntityAddressId:data.Data[i].LegalEntityAddressId,
                    Address:data.Data[i].Address
                }
                appSettings.model.LegalEntityAddress.push(appSettings.LegalEntityAddressModel);
                dataTableLegalEntityAddress.row.add(appSettings.LegalEntityAddressModel).draw();
            }
        });
    }

    var initCrimeReportGrid = function () {

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
                    targets: [0], width: 1
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
                {
                    "data": "CrimeIncidentReportId", "sortable": false, "orderable": false, "searchable": false,
                    render: function (data, type, full, meta) { return '<a href="/CrimeIncidentReport/Details/' + full.CrimeIncidentReportId + '">' + full.CrimeIncidentReportId + '</a>'; }
                },
                { "data": "CrimeIncidentCategory.CrimeIncidentCategoryName", "sortable": false, "orderable": false, "searchable": false, },
                {
                    "data": "ApprovalStatus.ApprovalStatusName", "sortable": false, "orderable": false, "searchable": false,
                    render: function (data, type, full, meta) {
                        var badgeStatus = 'badge-warning';
                        if (full.ApprovalStatus.ApprovalStatusId === 1) {
                            badgeStatus = 'badge-info';
                        } else if (full.ApprovalStatus.ApprovalStatusId === 2) {
                            badgeStatus = 'badge-error';
                        } else {
                            badgeStatus = 'badge-warning';
                        }
                        return '<span class="badge ' + badgeStatus + '" style="padding: 10px">' + data + '</span>';
                    }
                },
                {
                    "data": "DateReported", "sortable": false, "orderable": false, "searchable": false,
                    render: function (data, type, full, meta) {
                        var date = new Date(data);
                        var newFormat = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()))
                        return newFormat;
                    }
                },
                {
                    "data": "PossibleDate", "sortable": false, "orderable": false, "searchable": false,
                    render: function (data, type, full, meta) {
                        var date = new Date(data);
                        var newFormat = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()))
                        return newFormat;
                    }
                },
                { "data": "PossibleTime", "sortable": false, "orderable": false, "searchable": false, },
                { "data": "GeoAddress", "sortable": false, "orderable": false, "searchable": false, },
                { "data": "Description", "sortable": false, "orderable": false, "searchable": false, },
            ],
            "order": [[1, "asc"]],
            select: {
                style: 'single'
            },
            bFilter: false,
            bLengthChange: true,
            "serverSide": true,
            "ajax": {
                "url": app.appSettings.silupostWebAPIURI + "CrimeIncidentReport/GetTablePageByPostedBySystemUserId",
                "type": "GET",
                "datatype": "json",
                contentType: 'application/json;charset=utf-8',
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                },
                data: function (data) {
                    var dataFilter = {
                        Draw: data.draw,
                        PostedBySystemUserId: appSettings.SystemUserId,
                        PageNo: data.start <= 0 ? data.start + 1 : (data.start / data.length) + 1,//must be added to 1
                        PageSize: data.length,
                    }
                    return dataFilter;
                }
            },

            "paging": true,
            "searching": false,
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
    };

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var mobileUserDetails = new mobileUserDetailsController;
