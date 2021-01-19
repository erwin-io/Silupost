
var userController = function() {

    var apiService = function(){
        var getNextCode = function()
        {
            return $.ajax({
                url: "/LegalEntity/GetNextCode",
                data: null,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }
        var getById = function(legalEntityId)
        {
            return $.ajax({
                url: "/User/GetById",
                data: { LegalEntityId: legalEntityId},
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }

        return {
            getById : getById,
            getNextCode : getNextCode,
        };
    }
    var api = new apiService;

    var form,roleForm,dataTableUser;
    var appSettings = {
        model:{},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {
        appSettings = $.extend(appSettings, obj);
        // iniValidation();
        initEvent();
        initGrid();
        initLookup();
    };
    var initLookup = ()=>{
        var roles = [];
        for(var i in appSettings.Role){
            if(appSettings.Role[i].RoleId != undefined){
                roles.push({id:appSettings.Role[i].RoleId,name:appSettings.Role[i].Rolename})
            }
        }
        appSettings.Role = roles;
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
        roleForm.validate({
            ignore:[],
            rules: {
                UserRoles: {
                    required: true,
                    minlength: 1
                }
            },
            messages: {
                UserRoles: {
                    required: "Please select Role"
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
        $("#table-user tbody").on("click", "tr .dropdown-menu a.edit", Edit);
        $("#table-user tbody").on("click", "tr .dropdown-menu a.remove", Delete);
    };

    var initGrid = function() {
        
        dataTableUser = $("#table-user").DataTable({
            processing: true,
            responsive: true,
            columnDefs: [
                {
                    targets: [0,1], className:"hidden",
                },
                {
                    targets: 3, width:1
                }
            ],
            "columns": [
                { "data": "UserId","sortable":false, "orderable": false, "searchable": false},
                { "data": "LegalEntity.LegalEntityId"},
                { "data": "LegalEntity.FullName"},
                { "data": null, "searchable": false, "orderable": false, 
                    render: function(data, type, full, meta){
                        return '<span class="dropdown pmd-dropdown dropup clearfix">'
                                +'<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-danger" type="button" id="drop-user-'+full.UserId+'" data-toggle="dropdown" aria-expanded="true">'
                                    +'<i class="material-icons pmd-sm">more_vert</i>'
                                +'</button>'
                                +'<ul aria-labelledby="drop-user-'+full.UserId+'" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                    +'<li role="presentation"><a class="edit" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.LegalEntity.LegalEntityId+'" role="menuitem">Edit</a></li>'
                                    +'<li role="presentation"><a class="remove" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.LegalEntity.LegalEntityId+'" role="menuitem">Remove</a></li>'
                                +'</ul>'
                                +'</span>'
                    }
                }
            ],
            "order": [[2, "asc"]],
            bFilter: true,
            bLengthChange: true,
            "serverSide": true,
            "ajax": {
                "url": "/User/GetPage",
                "type": "GET",
                "datatype": "json"
            },

            "paging": true,
            "searching": true,
            "language": {
                "info": " _START_ - _END_ of _TOTAL_ ",
                "sLengthMenu": "<div class='user-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".user-lookup-table-length-menu select").select2({
                    theme: "bootstrap",
                    minimumResultsForSearch: Infinity,
                });
            }
        });
        dataTableUser.columns.adjust();
    };

    var Add = function(){
        legalEntity.appSettings.status.IsNew = true;
        var userTemplate = $.templates('#user-template');
        $("#modal-dialog").find('.modal-title').html('New User');
        $("#modal-dialog").find('.modal-footer #btnSave').html('Save');
        $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Save");
        api.getNextCode().done(function(data){
            appSettings.model = {};
            appSettings.model.IsNew = true;
            appSettings.model.LegalEntity = {};
            appSettings.model.LegalEntity.LegalEntityId = data;
            appSettings.model.LegalEntityId = data;
            appSettings.model.LegalEntity.GenderList = enums.Gender;
            appSettings.model.UserRoles = [];
            appSettings.model.Role = appSettings.Role;
            appSettings.model.LegalEntity.BirthDate = moment(appSettings.model.LegalEntity.BirthDate).format("MM/DD/YYYY");
            userTemplate.link("#modal-dialog .modal-body", appSettings.model);
            
            $(".select-tags").select2({
                tags: false,
                theme: "bootstrap",
            });

            legalEntity.init();
            form = $("#form-user");
            roleForm = $("#form-role");
            iniValidation();
            $("#modal-dialog").modal('show');
        });
    }

    var Edit = function(){
        if($(this).attr("data-value")!= ""){
            legalEntity.appSettings.status.IsNew = false;
            var userTemplate = $.templates('#user-template');
            $("#modal-dialog").find('.modal-title').html('Update User');
            $("#modal-dialog").find('.modal-footer #btnSave').html('Save');
            $("#modal-dialog").find('.modal-footer #btnSave').attr("data-name","Save");
            circleProgress.show(true);
            api.getById($(this).attr("data-value")).done(function(data){
                appSettings.model = data;
                appSettings.model.IsNew = false;
                appSettings.model.LegalEntity.GenderList = enums.Gender;
                appSettings.model.LegalEntity.BirthDate = moment(appSettings.model.LegalEntity.BirthDate).format("MM/DD/YYYY");
                appSettings.model.Role = appSettings.Role;
                appSettings.model.ConfirmPassword = appSettings.model.Password;
                var userroles = [];
                for(var i in appSettings.model.UserRoles){
                    if(appSettings.model.UserRoles[i].RoleId != undefined)
                        userroles.push({id:appSettings.model.UserRoles[i].RoleId, name:appSettings.model.UserRoles[i].Role.RoleName});
                }
                appSettings.model.UserRoles = userroles;
                userTemplate.link("#modal-dialog .modal-body", appSettings.model);

                $(".select-tags").select2({
                    tags: false,
                    theme: "bootstrap",
                });

                legalEntity.init();
                form = $("#form-user");
                roleForm = $("#form-role");
                iniValidation();
                $("#modal-dialog").modal('show');
                circleProgress.close();
            });
        }
    }

    //Save Data Function 
    var Save = function(e){
        if(!legalEntity.valid()){
            $("#tab-control-legalentity").trigger('click');
            return;
        }
        if(!form.valid()){
            $("#tab-control-credentials").trigger('click');
            return;
        }
        if(!roleForm.valid()){
            $("#tab-control-roles").trigger('click');
            return;
        }
        var obj = appSettings.model;
        obj.UserRoles = dataFormat.getMultiSelectData({keyId: "RoleId", keyName: "Rolename"}, appSettings.model.UserRoles);
        if(!legalEntity.appSettings.status.IsNew){
            legalEntity.Save($(this),obj,"/User/Update",dataTableUser);
        }else{
            legalEntity.Save($(this),obj,"/User/Create",dataTableUser);
        }
    }

    var Delete = function() {
        if($(this).attr("data-value")!= ""){
            legalEntity.Delete($(this),{LegalEntityId: $(this).attr("data-value")}, "/User/Delete",dataTableUser);
            dataTableUser.ajax.reload();
        }
    };

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var user = new userController;
