
var petGroupController = function() {

    var apiService = function (apiURI) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "PetGroup/" + Id + "/detail",
                data: { Id: Id },
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }
        var getDefaultProfilePic = function (Id) {
            return $.ajax({
                url: apiURI + "File/getDefaultPetConfigProfilePic",
                data: null,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }

        return {
            getById: getById,
            getDefaultProfilePic: getDefaultProfilePic
        };
    }
    var api = new apiService(app.appSettings.petDatingWebURI);

    var dataTable;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {
        initEvent();
        initGrid();
        initDefaultProfilePic();
    };

    var initDefaultProfilePic = function () {
        api.getDefaultProfilePic().done(function (data) {
            appSettings.DefaultProfilePic = data.Data;
        });
    }

    var iniValidation = function() {
        form.validate({
            ignore:[],
            rules: {
                Name: {
                    required: true
                }
            },
            messages: {
                Name: "Please enter Name"
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
        $("#table-petGroup tbody").on("click", "tr .dropdown-menu a.edit", Edit);
        $("#table-petGroup tbody").on("click", "tr .dropdown-menu a.remove", Delete);

    };

    var initGrid = function() {
        dataTable = $("#table-petGroup").DataTable({
            processing: true,
            responsive: true,
            columnDefs: [
                {
                    targets: 0, className:"hidden",
                },
                {
                    targets: [1, 3], width:1
                }
            ],
            "columns": [
                { "data": "PetGroupId","sortable":false, "orderable": false, "searchable": false},
                {
                    "data": null, "searchable": false, "orderable": false,
                    render: function (data, type, full, meta) {
                        var fileData = 'data:' + full.ProfilePic.MimeType + ';base64,' + full.ProfilePic.FileContent;
                        return '<image style="width:50px;height:50px" src="' + fileData + '"></image>'
                    }
                },
                { "data": "Name" },
                { "data": null, "searchable": false, "orderable": false, 
                    render: function(data, type, full, meta){
                        return '<span class="dropdown pmd-dropdown dropup clearfix">'
                                +'<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-danger" type="button" id="drop-role-'+full.PetGroupId+'" data-toggle="dropdown" aria-expanded="true">'
                                    +'<i class="material-icons pmd-sm">more_vert</i>'
                                +'</button>'
                                +'<ul aria-labelledby="drop-role-'+full.PetGroupId+'" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                    +'<li role="presentation"><a class="edit" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.PetGroupId+'" role="menuitem">Edit</a></li>'
                                    +'<li role="presentation"><a class="remove" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.PetGroupId+'" role="menuitem">Remove</a></li>'
                                +'</ul>'
                                +'</span>'
                    }
                }
            ],
            "order": [[2, "asc"]],
            select: {
                style: 'single'
            },
            bFilter: true,
            bLengthChange: true,
            "serverSide": true,
            "ajax": {
                "url": app.appSettings.petDatingWebURI + "PetGroup/GetPage",
                "type": "GET",
                "datatype": "json",
                "contentType": 'application/json; charset=utf-8',
                data: function (data) {
                    var dataFilter = {
                        draw: data.draw,
                        search: data.search.value,
                        pageStart: data.start,
                        pageSize: data.length,
                        orderColumn: data.columns[data.order[0].column].data,
                        orderDir: data.order[0].dir
                    }
                    return dataFilter;
                }
            },

            "paging": true,
            "searching": true,
            "language": {
                "info": " _START_ - _END_ of _TOTAL_ ",
                "sLengthMenu": "<div class='petGroup-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".petGroup-lookup-table-length-menu select").select2({
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
        var ratedTemplate = $.templates('#petGroup-template');
        $("#modal-dialog").find('.modal-title').html('New Pet Group');
        $("#modal-dialog").find('.modal-footer #btnSave').html('Save');
        $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Save");

        //reset model 
        appSettings.model = {
            ProfilePic: {
                FileName: appSettings.DefaultProfilePic.FileName,
                MimeType: appSettings.DefaultProfilePic.MimeType,
                FileSize: appSettings.DefaultProfilePic.FileSize,
                IsDefault: true,
                FileData: 'data:' + appSettings.DefaultProfilePic.MimeType + ';base64,' + appSettings.DefaultProfilePic.FileContent
            }
        };
        //end reset model
        //render template
        ratedTemplate.link("#modal-dialog .modal-body", appSettings.model);

        //init form validation
        form = $('#form-petGroup');
        iniValidation();
        //end init form

        //custom init for ui
        form.find(".group-fields").first().addClass("hidden");
        //end custom init
        //show modal
        $("#modal-dialog").modal('show');
        //end show modal


        $("#ProfiePic").on("change", function () {
            var file = $("input[type=file]").get(0).files[0];

            if (file && fileValid(file)) {
                var reader = new FileReader();

                reader.onload = function () {
                    $("#img-upload").attr("src", reader.result);
                }

                appSettings.model.ProfilePic = {
                    FileName: file.name,
                    MimeType: file.type,
                    FileSize: file.size,
                    File: file,
                    IsDefault: false
                }

                reader.readAsDataURL(file);
            }
        });

    }

    var Edit = function () {
        if ($(this).attr("data-value") != "") {
            appSettings.status.IsNew = false;
            var ratedTemplate = $.templates('#petGroup-template');
            $("#modal-dialog").find('.modal-title').html('Update Pet Group');
            $("#modal-dialog").find('.modal-footer #btnSave').html('Update');
            $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Update");
            circleProgress.show(true);
            api.getById($(this).attr("data-value")).done(function (data) {
                appSettings.model = data.Data;
                appSettings.model.ProfilePic.FileData = 'data:' + appSettings.model.ProfilePic.MimeType + ';base64,' + appSettings.model.ProfilePic.FileContent;
                //render template
                ratedTemplate.link("#modal-dialog .modal-body", appSettings.model);
                //end render template
                form = $('#form-petGroup');
                iniValidation();
                $("#modal-dialog").modal('show');
                circleProgress.close();


                $("#ProfiePic").on("change", function () {
                    var file = $("input[type=file]").get(0).files[0];

                        if (file && fileValid(file)) {
                        var reader = new FileReader();

                        reader.onload = function () {
                            $("#img-upload").attr("src", reader.result);
                        }

                        appSettings.model.ProfilePic.FileName = file.name;
                        appSettings.model.ProfilePic.MimeType = file.type;
                        appSettings.model.ProfilePic.FileSize = file.size;
                        appSettings.model.ProfilePic.File = file;

                        appSettings.model.ProfilePic.HasChange = true;

                        reader.readAsDataURL(file);
                    }
                });
            });
        }
    }

    var fileValid = function (file) {
        var max_size = 10000000;
        if (file.size > max_size) {
            Swal.fire("Not Allowed!", file.name + " exceeds the maximum upload size", 'error');
            return false;
        }

        var extensions = ["jpg", "jpeg", "png"];
        var extension = file.name.replace(/.*\./, '').toLowerCase();

        if (extensions.indexOf(extension) < 0) {
            Swal.fire("Not Allowed!", "File not allowed", 'error');
            return false;
        }
        return true;
    }
    //Save Data Function 
    var Save = function(e){
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
                    var data = new FormData();
                    data.append("model", JSON.stringify(appSettings.model));
                    if (!appSettings.model.ProfilePic.IsDefault) {
                        data.append("file", appSettings.model.ProfilePic.File);
                    }
                    $.ajax({
                        url: app.appSettings.petDatingWebURI + "/PetGroup/",
                        cache: false,
                        type: 'POST',
                        contentType: false,
                        processData: false,
                        data: data,
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
                    var data = new FormData();
                    data.append("model", JSON.stringify(appSettings.model));
                    if (appSettings.model.ProfilePic.HasChange) {
                        data.append("file", appSettings.model.ProfilePic.File);
                    }

                    $.ajax({
                        url: app.appSettings.petDatingWebURI + "/PetGroup/",
                        type: "PUT",
                        cache: false,
                        contentType: false,
                        processData: false,
                        data: data,
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
                        url: app.appSettings.petDatingWebURI + "/PetGroup/" + $(this).attr("data-value"),
                        type: "DELETE",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
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
var petGroup = new petGroupController;
