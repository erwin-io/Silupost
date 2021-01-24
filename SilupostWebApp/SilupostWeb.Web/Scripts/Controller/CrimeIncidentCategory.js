
var crimeIncidentCategoryController = function() {

    var apiService = function (apiURI,apiToken) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "CrimeIncidentCategory/" + Id + "/detail",
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

    var dataTable;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {
        initEvent();

		appSettings.CrimeIncidentType = {
			CrimeIncidentTypeId : "",
			lookup : []
		};
		initLookup();


        $(window).resize(function () {
            if ($("#table-crimeIncidentCategory").hasClass('collapsed')) {
                $("#btnDelete").removeClass("hidden");
                $("#btnEdit").removeClass("hidden");
            } else {
                $("#btnDelete").addClass("hidden");
                $("#btnEdit").addClass("hidden");
            }
        });
        $(document).ready(function () {
            if ($("#table-crimeIncidentCategory").hasClass('collapsed')) {
                $("#btnDelete").removeClass("hidden");
                $("#btnEdit").removeClass("hidden");
            } else {
                $("#btnDelete").addClass("hidden");
                $("#btnEdit").addClass("hidden");
            }
        });
    };

    var initLookup = function(){
        api.getLookup("CrimeIncidentType").done(function (data) {
	    	appSettings.lookup = $.extend(appSettings.lookup, data.Data);

			appSettings.CrimeIncidentType = {
				CrimeIncidentTypeId : "",
				lookup : appSettings.lookup
			};
	        var crimeIncidentTypeTemplate = $.templates('#crimeIncidentType-template');
	        crimeIncidentTypeTemplate.link("#select-CrimeIncidentType", appSettings.CrimeIncidentType);


	        $("#CrimeIncidentType").on("select2:select", function(){
	        	initGrid();
            });

	        $(".select-simple").select2({
	            theme: "bootstrap",
	            minimumResultsForSearch: Infinity,
	        });
        });
    }
    var iniValidation = function() {
        form.validate({
            ignore:[],
            rules: {
                CrimeIncidentCategoryName: {
                    required: true
                },
                CrimeIncidentCategoryDescription: {
                    required: true
                }
            },
            messages: {
                CrimeIncidentCategoryName: "Please enter Crime Incident Category Name",
                CrimeIncidentCategoryDescription: "Please enter Crime Incident Category Description"
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
    };

    var initEvent = function () {
        $("#btnAdd").on("click", Add);
        $("#btnSave").on("click", Save);
        $("#btnEdit").on("click", Edit);
        $("#btnDelete").on("click", Delete);

        $("#table-crimeIncidentCategory tbody").on("click", "tr .dropdown-menu a.edit", function () {
            appSettings.currentId = $(this).attr("data-value");
            Edit();
        });
        $("#table-crimeIncidentCategory tbody").on("click", "tr .dropdown-menu a.remove", function () {
            appSettings.currentId = $(this).attr("data-value");
            Delete();
        });

        $('#table-crimeIncidentCategory tbody').on('click', 'tr', function () {
            var isSelected = !$(this).hasClass('selected');
            if (isSelected && $("#table-crimeIncidentCategory").hasClass('collapsed')) {
	            appSettings.currentId = dataTable.row(this).data().CrimeIncidentCategoryId;
                $("#btnDelete").removeClass("hidden");
                $("#btnEdit").removeClass("hidden");
            } else {
                $("#btnDelete").addClass("hidden");
                $("#btnEdit").addClass("hidden");
            }
        });

        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
    };

    var initGrid = function() {
        if (dataTable) {
            dataTable.destroy();
        }
        dataTable = $("#table-crimeIncidentCategory").DataTable({
            processing: true,
            responsive: true,
            columnDefs: [
                {
                    targets: 0, className:"hidden",
                },
                {
                    targets: [3], width:1
                }
            ],
            "columns": [
                { "data": "CrimeIncidentCategoryId","sortable":false, "orderable": false, "searchable": false},
                { "data": "CrimeIncidentCategoryName" },
                { "data": "CrimeIncidentCategoryDescription" },
                { "data": null, "searchable": false, "orderable": false, 
                    render: function(data, type, full, meta){
                        return '<span class="dropdown pmd-dropdown dropup clearfix">'
                                +'<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button" id="drop-role-'+full.CrimeIncidentCategoryId+'" data-toggle="dropdown" aria-expanded="true">'
                                    +'<i class="material-icons pmd-sm">more_vert</i>'
                                +'</button>'
                                +'<ul aria-labelledby="drop-role-'+full.CrimeIncidentCategoryId+'" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                    +'<li role="presentation"><a class="edit" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.CrimeIncidentCategoryId+'" role="menuitem">Edit</a></li>'
                                    +'<li role="presentation"><a class="remove" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.CrimeIncidentCategoryId+'" role="menuitem">Remove</a></li>'
                                +'</ul>'
                                +'</span>'
                    }
                }
            ],
            "order": [[0, "asc"]],
            select: {
                style: 'single'
            },
            bFilter: true,
            bLengthChange: true,
            "serverSide": true,
            "ajax": {
                "url": app.appSettings.silupostWebAPIURI + "CrimeIncidentCategory/GetPage",
                "type": "GET",
                "datatype": "json",
                contentType: 'application/json;charset=utf-8',
                headers: {
                    Authorization: 'Bearer ' + app.appSettings.apiToken
                },
                data: function (data) {
                    var dataFilter = {
                        Draw: data.draw,
                        CrimeIncidentTypeId: appSettings.CrimeIncidentType.CrimeIncidentTypeId,
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
                "sLengthMenu": "<div class='crimeIncidentCategory-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".crimeIncidentCategory-lookup-table-length-menu select").select2({
                    theme: "bootstrap",
                    minimumResultsForSearch: Infinity,
                });
                circleProgress.close();
            }
        });
        dataTable.columns.adjust();
    };

    var Add = function(){
        appSettings.status.IsNew = true;
        var crimeIncidentCategoryTemplate = $.templates('#crimeIncidentCategory-template');
        $("#modal-dialog").find('.modal-title').html('New Crime Incident Type');
        $("#modal-dialog").find('.modal-footer #btnSave').html('Save');
        $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Save");

        //reset model 
        appSettings.model = {
        };
        appSettings.model.lookup = {
            CrimeIncidentType: appSettings.lookup.CrimeIncidentType
        };
        //end reset model
        //render template
        crimeIncidentCategoryTemplate.link("#modal-dialog .modal-body", appSettings.model);

        //init form validation
        form = $('#form-crimeIncidentCategory');
        iniValidation();
        //end init form
        //custom init for ui
        form.find(".group-fields").first().addClass("hidden");

        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
        //end custom init

        //show modal
        $("#modal-dialog").modal('show');
        //end show modal
    }

    var Edit = function () {
        if (appSettings.currentId !== null || appSettings.currentId !== undefined || appSettings.currentId !== "") {
            appSettings.status.IsNew = false;
            var crimeIncidentCategoryTemplate = $.templates('#crimeIncidentCategory-template');
            $("#modal-dialog").find('.modal-title').html('Update Crime Incident Type');
            $("#modal-dialog").find('.modal-footer #btnSave').html('Update');
            $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Update");
            circleProgress.show(true);
            api.getById(appSettings.currentId).done(function (data) {
                appSettings.model = data.Data;
                appSettings.model.CrimeIncidentTypeId = data.Data.CrimeIncidentType.CrimeIncidentTypeId;

                appSettings.model.lookup = {
                    CrimeIncidentType: appSettings.lookup.CrimeIncidentType
                };
                //render template
                crimeIncidentCategoryTemplate.link("#modal-dialog .modal-body", appSettings.model);


		        $(".select-simple").select2({
		            theme: "bootstrap",
		            minimumResultsForSearch: Infinity,
		        });
                //end render template
                form = $('#form-crimeIncidentCategory');
                iniValidation();
                $("#modal-dialog").modal('show');
                circleProgress.close();

            });
        }
    }

    //Save Data Function 
    var Save = function(e){
        console.log(appSettings.model);
        if(!form.valid())
            return;
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
                        url: app.appSettings.silupostWebAPIURI + "/CrimeIncidentCategory/",
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
                                    dataTable.ajax.reload();
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
                        url: app.appSettings.silupostWebAPIURI + "/CrimeIncidentCategory/",
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
                                    dataTable.ajax.reload();
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
        if (appSettings.currentId !== null || appSettings.currentId !== undefined || appSettings.currentId !== "") {
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
                        url: app.appSettings.silupostWebAPIURI + "/CrimeIncidentCategory/" + appSettings.currentId,
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
                                    dataTable.ajax.reload();
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
var crimeIncidentCategory = new crimeIncidentCategoryController;
