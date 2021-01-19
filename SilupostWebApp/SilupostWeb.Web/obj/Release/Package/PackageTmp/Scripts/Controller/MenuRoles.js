
var menurolesController = function() {

    var apiService = function(){
        var getAll = function(RoleId, ModuleName)
        {
            return $.ajax({
                url: "/MenuRole/GetAll",
                data: { RoleId: RoleId, ModuleName: ModuleName},
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }
        var logoutUser = function()
        {
            return $.ajax({
                url: "/Account/Logout",
                data: null,
                type: "POST",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }

        return {
            getAll : getAll,
            logoutUser: logoutUser
        };
    }
    var api = new apiService;

    var dataTable,form,menurolesTemplate;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {
        appSettings = $.extend(appSettings, obj);
        form = $("#form-menuroles");
        setTimeout(function(){
            var selectTemplate = $.templates('#menuroles-select-template');
            appSettings.model.RoleList = appSettings.Role;
            appSettings.model.ModuleList = createModule();
            selectTemplate.link(".select-container", appSettings.model);
            iniValidation();
            initEvent();
            initGrid();
            loadMenuRoles();
        }, 1000);
    };

    var createModule = function(){
        var modules = [
            {
                ModuleName: "Dashboard"
            },
            {
                ModuleName: "Configuration"
            },
            {
                ModuleName: "Security"
            }
        ]
        return modules;
    }

    var iniValidation = function() {
        form.validate({
            ignore:[],
            rules: {
                RoleId: {
                    required: true
                },
                ModuleName: {
                    required: true
                }
            },
            messages: {
                RoleId: {
                    required: "Please select Role"
                },
                ModuleName: {
                    required: "Please select Module"
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
    };

    var loadMenuRoles = function(){ 
        if(appSettings.model.RoleId != undefined && appSettings.model.ModuleName != undefined){
            circleProgress.show(false);
            api.getAll(appSettings.model.RoleId,appSettings.model.ModuleName).done(function(data){
                menurolesTemplate = $.templates("#menu-roles-template");
                dataTable.clear().draw();
                appSettings.model.MenuRoles = data;
                for(var i in data){
                    if(data[i].MenuId != undefined)
                    {
                        var obj = {
                            RoleId:data[i].RoleId,
                            MenuId:data[i].MenuId,
                            PageName:data[i].Menu.PageName,
                            IsAllowed:data[i].IsAllowed
                        };
                        dataTable.row.add(obj).draw();
                        dataTable.columns.adjust();
                    }
                }
                circleProgress.close();
            });
        }
    }

    var initEvent = function () {
        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
        $("#RoleId").on("select2:select", loadMenuRoles);
        $("#ModuleName").on("select2:select", loadMenuRoles);
        $("#btn-save").on("click", Save);
        $("#table-menuroles tbody").on('change', 'tr .pmd-checkbox input[type="checkbox"]', setAllowed);
    };

    var initGrid = function() {
        dataTable = $("#table-menuroles").DataTable({
            processing: true,
            responsive: true,
            columnDefs: [
                {
                    targets: 0, className:"hidden",
                },
                {
                    targets: 2,width: 1
                }
            ],
            "columns": [
                { "data": "MenuId","sortable":false, "orderable": false, "searchable": false},
                { "data": "PageName"},
                { "data": null, "searchable": false, "orderable": false,
                    render: function(data, type, full, meta){
                        return '<label class="checkbox-inline pmd-checkbox pmd-checkbox-ripple-effect">'
                                    +'<input data-link="'+full.MenuId+'" type="checkbox" '+(full.IsAllowed ? 'checked="checked"' : '')+' class="pm-ini">'
                                    +'<span class="pmd-checkbox-label"></span>'
                                +'</label>'
                    }
                }
            ],
            bFilter: true,
            bLengthChange: true,
            "paging": true,
            "searching": true,
            "language": {
                "info": " _START_ - _END_ of _TOTAL_ ",
                "sLengthMenu": "<div class='genre-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".genre-lookup-table-length-menu select").select2({
                    theme: "bootstrap",
                    minimumResultsForSearch: Infinity,
                });
            }
        });
        for(var i in appSettings.MenuRoles){

        }
    };

    var setAllowed = function(){
        for(var i in appSettings.model.MenuRoles){
            if(appSettings.model.MenuRoles[i].MenuId == $(this).attr("data-link")){
                appSettings.model.MenuRoles[i].IsAllowed = this.checked;
            }
        }
    }

    //Save Data Function 
    var Save = function(e){
        if(!form.valid())
            return;
        else{
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
                    circleProgress.show(false);
                    $.ajax({
                        url: "/MenuRole/Save",
                        data: JSON.stringify(appSettings.model.MenuRoles),
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.Success) {
                                Swal.fire({
                                    title: "Success!",
                                    text: result.Message + "Changes will effect after logout. Do you want to logout?",
                                    type: "success",
                                    showCancelButton: true,
                                    confirmButtonColor: '#3085d6',  
                                    cancelButtonColor: '#d33',
                                    confirmButtonText: 'Yes',
                                    allowOutsideClick: false
                                }).then((result) => {
                                    if(result.value){
                                        api.logoutUser().done(function(result){
                                            if (result.Success) {
                                                window.location.reload();
                                            } else {
                                                Swal.fire('Error!',result.Message,'error');
                                            }
                                            circleProgress.close();
                                        }).error(function(errormessage){
                                            Swal.fire('Error!',errormessage.Message,'error');
                                            circleProgress.close();
                                        });
                                    }
                                });
                            } else {
                                Swal.fire('Error!',result.Message,'error');
                            }
                            $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            circleProgress.close();
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
    }

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var menuroles = new menurolesController;
