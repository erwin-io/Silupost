
var systemUserController = function() {

    var apiService = function (apiURI,apiToken) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "SystemUser/" + Id + "/detail",
                data: { Id: Id },
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
            getLookup: getLookup
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI,app.appSettings.apiToken);

    var form,formSystemWebAdminUserRoles,dataTableSystemUser;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {
        initEvent();
        initGrid();
        initLookup();
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
                    required: true
                }
            },
            Messages: {
                LegalEntityId: "Please enter LegalEntityId",
                Firstname: "Please enter a Firstname",
                Middlename: "Please enter Middlename",
                Lastname: "Please enter Lastname",
                BirthDate: "Please select Birth Date",
                Gender: "Please select Gender",
            },
        }
    };

    var initLookup = function(){
        api.getLookup("SystemWebAdminRole,EntityGender").done(function (data) {
        	appSettings.lookup = $.extend(appSettings.lookup, data.Data);
        	console.log(data.Data);
        });
    }

    var iniValidation = function() {
        form.validate({
            ignore:[],
            rules: {
                Username: {
                    required: true,
                    minlength: 4,
                },
                Password: {
                    required: function(){
                        return appSettings.model.IsNew;
                    },
                    minlength: function(){
                        var min = appSettings.model.IsNew ? 8 : 0;
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
                Username: {
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
    };

    var initEvent = function () {
        $("#btnAdd").on("click", Add);
        $("#btnSave").on("click", Save);
        $("#table-systemUser tbody").on("click", "tr .dropdown-menu a.edit", Edit);
        $("#table-systemUser tbody").on("click", "tr .dropdown-menu a.remove", Delete);

    };

    var initGrid = function() {
        dataTableSystemUser = $("#table-systemUser").DataTable({
            processing: true,
            responsive: true,
            columnDefs: [
                {
                    targets: 0, className:"hidden",
                },
                {
                    targets: [6], width:1
                }
            ],
            "columns": [
                { "data": "SystemUserId","sortable":false, "orderable": false, "searchable": false},
                { "data": "UserName" },
                { "data": "LegalEntity.FullName" },
                { "data": "LegalEntity.Gender.GenderName" },
                { "data": "LegalEntity.EmailAddress" },
                { "data": "LegalEntity.MobileNumber" },
                { "data": null, "searchable": false, "orderable": false, 
                    render: function(data, type, full, meta){
                        return '<span class="dropdown pmd-dropdown dropup clearfix">'
                                +'<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-danger" type="button" id="drop-role-'+full.SystemUserId+'" data-toggle="dropdown" aria-expanded="true">'
                                    +'<i class="material-icons pmd-sm">more_vert</i>'
                                +'</button>'
                                +'<ul aria-labelledby="drop-role-'+full.SystemUserId+'" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                    +'<li role="presentation"><a class="edit" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.SystemUserId+'" role="menuitem">Edit</a></li>'
                                    +'<li role="presentation"><a class="remove" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.SystemUserId+'" role="menuitem">Remove</a></li>'
                                +'</ul>'
                                +'</span>'
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
                "url": app.appSettings.silupostWebAPIURI + "SystemUser/GetPage",
                "type": "GET",
                "datatype": "json",
                contentType: 'application/json;charset=utf-8',
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                },
                data: function (data) {
                    var dataFilter = {
                        Draw: data.draw,
                        SystemUserType: 1,//default for web admin user
                        Search: data.search.value,
                        PageNo: data.start <= 0 ? data.start + 1 : (data.start / data.length) + 1,//must be added to 1
                        PageSize: data.length,
                        OrderColumn: data.columns[data.order[0].column].data,
                        OrderDir: data.order[0].dir
                    }
                    return dataFilter;
                }
            },

            "paging": true,
            "searching": true,
            "language": {
                "info": " _START_ - _END_ of _TOTAL_ ",
                "sLengthMenu": "<div class='systemUser-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".systemUser-lookup-table-length-menu select").select2({
                    theme: "bootstrap",
                    minimumResultsForSearch: Infinity,
                });
                circleProgress.close();
            }
        });
        dataTableSystemUser.columns.adjust();
    };

    var Add = function(){
        appSettings.status.IsNew = true;
        legalEntity.appSettings.status.IsNew = true;
        var systemUserTemplate = $.templates('#systemUser-template');
        $("#modal-dialog").find('.modal-title').html('New System Web User');
        $("#modal-dialog").find('.modal-footer #btnSave').html('Save');
        $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Save");

        //reset model 
        appSettings.model = {};
        appSettings.model.IsNew = true;
        appSettings.model.SystemUserTypeId = 1;
        appSettings.model.BirthDate = moment(new Date()).format("MM/DD/YYYY");
        appSettings.model.SystemWebAdminUserRoles = [];
        appSettings.model.lookup = {
            EntityGender: appSettings.lookup.EntityGender,
            SystemWebAdminRole: []
        };
        for (var i in appSettings.lookup.SystemWebAdminRole) {
            if (appSettings.lookup.SystemWebAdminRole[i].Id != undefined)
                appSettings.model.lookup.SystemWebAdminRole.push({ id: appSettings.lookup.SystemWebAdminRole[i].Id, name: appSettings.lookup.SystemWebAdminRole[i].Name });
        }
        //end reset model
        //render template
        systemUserTemplate.link("#modal-dialog .modal-body", appSettings.model);

        $(".select-tags").select2({
            tags: false,
            theme: "bootstrap",
        });


        //init form validation
        legalEntity.init();
        form = $('#form-systemUser');
        formSystemWebAdminUserRoles = $("#form-SystemWebAdminUserRoles");
        iniValidation();
        //end init form

        //custom init for ui
        //form.find(".group-fields").first().addClass("hidden");
        //end custom init
        //show modal
        $("#modal-dialog").modal('show');
        $("body").addClass("modal-open");
        //end show modal

    }

    var Edit = function () {
        if ($(this).attr("data-value") != "") {
            appSettings.status.IsNew = false;
            var systemUserTemplate = $.templates('#systemUser-template');
            $("#modal-dialog").find('.modal-title').html('Update System Web User');
            $("#modal-dialog").find('.modal-footer #btnSave').html('Update');
            $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Update");
            circleProgress.show(true);
            api.getById($(this).attr("data-value")).done(function (data) {
                appSettings.model = {
                    SystemUserId: data.Data.SystemUserId,
                    UserName: data.Data.UserName,
                    Password: data.Data.Password,
                    ConfirmPassword: data.Data.Password
                };

                appSettings.model = $.extend(appSettings.model, data.Data.LegalEntity);
                appSettings.model.BirthDate = moment(data.Data.LegalEntity.BirthDate).format("MM/DD/YYYY");
                appSettings.model.GenderId = data.Data.LegalEntity.Gender.GenderId;
                //appSettings.model.lookup = appSettings.lookup;
                appSettings.model.lookup = {
                    EntityGender: appSettings.lookup.EntityGender,
                    SystemWebAdminRole : []
                };

                //for (var i in appSettings.lookup.EntityGender) {
                //    if (appSettings.lookup.EntityGender[i].Id != undefined)
                //        appSettings.model.lookup.EntityGender.push({ id: appSettings.lookup.EntityGender[i].Id, name: appSettings.lookup.EntityGender[i].Name });
                //}
                for (var i in appSettings.lookup.SystemWebAdminRole) {
                    if (appSettings.lookup.SystemWebAdminRole[i].Id != undefined)
                        appSettings.model.lookup.SystemWebAdminRole.push({ id: appSettings.lookup.SystemWebAdminRole[i].Id, name: appSettings.lookup.SystemWebAdminRole[i].Name });
                }
                var systemWebAdminUserRoles = [];
                for(var i in data.Data.SystemWebAdminUserRoles){
                    if (data.Data.SystemWebAdminUserRoles[i].SystemWebAdminRole.SystemWebAdminRoleId != undefined)
                        systemWebAdminUserRoles.push({id:data.Data.SystemWebAdminUserRoles[i].SystemWebAdminRole.SystemWebAdminRoleId, name:data.Data.SystemWebAdminUserRoles[i].SystemWebAdminRole.RoleName});
                }
                appSettings.model.SystemWebAdminUserRoles = systemWebAdminUserRoles;
                //render template
                systemUserTemplate.link("#modal-dialog .modal-body", appSettings.model);
                //end render template

                $(".select-tags").select2({
                    tags: false,
                    theme: "bootstrap",
                });

		        //init form validation
		        legalEntity.init();
		        form = $('#form-systemUser');
		        formSystemWebAdminUserRoles = $("#form-SystemWebAdminUserRoles");
		        iniValidation();
		        //end init form
                circleProgress.close();

                $("#modal-dialog").modal('show');
                $("body").addClass("modal-open");
                setTimeout(1000, function()
                {
                    $("body").addClass("modal-open");
                })

            });
        }
    }
    //Save Data Function 
    var Save = function(e){
        console.log(appSettings.model);
        if (!legalEntity.valid()) {
            $("#tab-control-legalentity").trigger('click');
            return;
        }
        if (!form.valid()) {
            $("#tab-control-credentials").trigger('click');
            return;
        }
        if (!formSystemWebAdminUserRoles.valid()) {
            $("#tab-control-roles").trigger('click');
            return;
        }
        var systemWebAdminUserRoles = [];
        for (var i in appSettings.model.SystemWebAdminUserRoles) {
            if (appSettings.model.SystemWebAdminUserRoles[i].id != undefined)
                systemWebAdminUserRoles.push({ SystemWebAdminRoleId: appSettings.model.SystemWebAdminUserRoles[i].id });
        }

        appSettings.model.SystemWebAdminUserRoles = systemWebAdminUserRoles;
        if(appSettings.status.IsNew){
            Swal.fire({
                title: 'Save',
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
                    var target = $(this);
                    var targetName = target.attr("data-name");
                    target.html(targetName+'&nbsp;<span class="spinner-border spinner-border-sm"></span>');
                    circleProgress.show(true);
                    $.ajax({
                        url: app.appSettings.silupostWebAPIURI + "/SystemUser/",
                        type: 'POST',
                        dataType: "json",
                        contentType: 'application/json;charset=utf-8',
                        headers: {
                            Authorization: 'Bearer ' + app.appSettings.apiToken
                        },
                        data: JSON.stringify(appSettings.model),
                        success: function (result) {
                            console.log(result);
                            if (result.IsSuccess) {
                                circleProgress.close();
                                Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                    circleProgress.show(true);
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    target.empty();
                                    target.html(targetName);
                                    dataTableSystemUser.ajax.reload();
                                    circleProgress.close();
                                    $("#modal-dialog").modal('hide');
                                });
                            } else {
                                Swal.fire("Error!", result.Message, "error").then((result) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    target.empty();
                                    target.html(targetName);
                                });
                            }
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        },
                        error: function (data) {
                            var errormessage = "";
                            var errorTitle = "";
                            if (data.responseJSON.Message != null) {
                                erroTitle = "Error!";
                                errormessage = data.responseJSON.Message;
                            }
                            if (data.responseJSON.DeveloperMessage != null && data.responseJSON.DeveloperMessage.includes("Cannot insert duplicate")) {
                                erroTitle = "Not Allowed!";
                                errormessage = "Already exist!";
                            }
                            $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            target.empty();
                            target.html(targetName);
                            Swal.fire(erroTitle, errormessage, 'error');
                            circleProgress.close();
                        }
                    });
                }
            });
        }
        else{
            Swal.fire({
                title: 'Update',
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
                    var target = $(this);
                    var targetName = target.attr("data-name");
                    target.html(targetName+'&nbsp;<span class="spinner-border spinner-border-sm"></span>');
                    circleProgress.show(true);
                    $.ajax({
                        url: app.appSettings.silupostWebAPIURI + "/SystemUser/",
                        type: "PUT",
                        dataType: "json",
                        contentType: 'application/json;charset=utf-8',
                        data: JSON.stringify(appSettings.model),
                        headers: {
                            Authorization: 'Bearer ' + app.appSettings.apiToken
                        },
                        success: function (result) {
                            if (result.IsSuccess) {
                                circleProgress.close();
                                Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                    circleProgress.show(true);
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    target.empty();
                                    target.html(targetName);
                                    dataTableSystemUser.ajax.reload();
                                    circleProgress.close();
                                    $("#modal-dialog").modal('hide');
                                });
                            } else {
                                Swal.fire("Error!", result.Message, "error").then((result) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    target.empty();
                                    target.html(targetName);
                                });
                            }
                        },
                        error: function (data) {
                            var errormessage = "";
                            var errorTitle = "";
                            if (data.responseJSON.Message != null) {
                                erroTitle = "Error!";
                                errormessage = data.responseJSON.Message;
                            }
                            if (data.responseJSON.DeveloperMessage != null && data.responseJSON.DeveloperMessage.includes("Cannot insert duplicate")) {
                                erroTitle = "Not Allowed!";
                                errormessage = "Already exist!";
                            }
                            $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            target.empty();
                            target.html(targetName);
                            Swal.fire(erroTitle, errormessage, 'error');
                            circleProgress.close();
                        }
                    });
                }
            });
        }
    }

    var Delete = function() {
        if($(this).attr("data-value")!= ""){
            Swal.fire({
                title: 'Remove',
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
                    var target = $(this);
                    var targetName = target.attr("data-name");
                    target.html(targetName+'&nbsp;<span class="spinner-border spinner-border-sm"></span>');
                    circleProgress.show(true);
                    $.ajax({
                        url: app.appSettings.silupostWebAPIURI + "/SystemUser/" + $(this).attr("data-value"),
                        type: "DELETE",
                        contentType: 'application/json;charset=utf-8',
                        dataType: "json",
                        headers: {
                            Authorization: 'Bearer ' + app.appSettings.apiToken
                        },
                        success: function (result) {
                            if (result.IsSuccess) {
                                circleProgress.close();
                                Swal.fire("Success!", result.Message, "success").then((prompt) => {
                                    circleProgress.show(true);
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    target.empty();
                                    target.html(targetName);
                                    dataTableSystemUser.ajax.reload();
                                    circleProgress.close();
                                });
                            } else {
                                Swal.fire("Error!", result.Message, "error").then((result) => {
                                    $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                                    target.empty();
                                    target.html(targetName);
                                });
                            }
                        },
                        error: function (errormessage) {
                            $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            target.empty();
                            target.html(targetName);
                            Swal.fire('Error!',errormessage.Message,'error');
                            circleProgress.close();
                        }
                    });
                }
            });
        }
    };

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var systemUser = new systemUserController;
