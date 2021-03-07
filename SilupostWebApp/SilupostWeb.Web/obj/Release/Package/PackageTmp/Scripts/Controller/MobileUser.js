
var mobileUserController = function() {

    var apiService = function (apiURI) {
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
            getTrackerStatusUserById: getTrackerStatusUserById,
            getDefaultProfilePic: getDefaultProfilePic
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI);

    var form,formSystemWebAdminUserRoles,formLegalEntityAddress,dataTableSystemUser,dataTableLegalEntityAddress;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {

        initEvent();
        appSettings.UserInTable = [];
        setTimeout(function () {
            initDefaultProfilePic();
            initGrid();
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
        var prom = [];
        for (var i in appSettings.UserInTable) {
            prom.push(api.getTrackerStatusUserById(appSettings.UserInTable[i].SystemUserId));
        }

        $.when(prom).then(function () {
            for (var i in prom) {
                prom[i].done(function (data) {
                    if (data.Data.HasFirstLogin) {
                        var dateTimeNow = new Date();
                        var lastActive = new Date(data.Data.LasteDateTimeActive);
                        var diff = Math.round((((lastActive - dateTimeNow) % 86400000) % 3600000) / 60000);
                        var isActive = diff > -1;
                        var onlineIdentifier = "#user-status-" + data.Data.SystemUserId;
                        if (isActive) {
                            $(onlineIdentifier).addClass("online");
                        } else {
                            $(onlineIdentifier).removeClass("online");
                        }
                    }
                });
            }
        });
    }

    var initDefaultProfilePic = function () {
        api.getDefaultProfilePic().done(function (data) {
            appSettings.DefaultProfilePic = data.Data;
        });
    }

    var iniValidation = function() {
    };

    var initEvent = function () {
        $("#btnView").on("click", function () {
            window.location.replace("/MobileController/Details/" + appSettings.currentId);
        });
        $("#table-mobileUser tbody").on("click", "tr .dropdown-menu a.view", function () {
            appSettings.currentId = $(this).attr("data-value");
            window.location.replace("/MobileController/Details/" + appSettings.currentId);
        });
        $('#table-mobileUser tbody').on('click', 'tr', function () {
            if(dataTableSystemUser.row(this).data()){
                appSettings.currentId = dataTableSystemUser.row(this).data().SystemUserId;
                var isSelected = !$(this).hasClass('selected');
                if (isSelected && $("#table-mobileUser").hasClass('collapsed')) {
                    $("#btnView").removeClass("hidden");
                } else {
                    $("#btnView").addClass("hidden");
                }
            }
        });
    }

    var initGrid = function() {
        dataTableSystemUser = $("#table-mobileUser").DataTable({
            processing: true,
			responsive: {
				details: {
					type: 'column',
					target: 'tr'
				}
			},
            columnDefs: [
                {
                    targets: [0,7], width:1
                },
                {
                    targets: [1], visible: false
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
                { "data": "SystemUserId","sortable":true, "orderable": true, "searchable": true},
                {
                    "data": null, "searchable": true, "orderable": false,
                    render: function (data, type, full, meta) {
                        appSettings.UserInTable.push({ SystemUserId: data.SystemUserId });
                        var src = 'data:' + appSettings.DefaultProfilePic.MimeType + ';base64,' + appSettings.DefaultProfilePic.FileContent;
                        if (data.ProfilePicture !== null && data.ProfilePicture !== undefined) {
                            if (data.ProfilePicture.IsFromStorage) {
                                src = app.appSettings.silupostWebAPIURI + "File/getFile?FileId=" + data.ProfilePicture.FileId;
                            } else if (data.ProfilePicture.FileContent !== null && data.ProfilePicture.FileContent !== undefined && data.ProfilePicture.FileContent !== "") {
                                src = 'data:' + data.ProfilePicture.MimeType + ';base64,' + data.ProfilePicture.FileContent;
                            }
                        }
                        return '<div class="user-status" id="user-status-' + data.SystemUserId + '"></div><image class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" style="width:50px;height:50px" src="' + src + '"></image>';
                    }
                },
                { "data": "UserName" },
                { "data": "LegalEntity.FullName" },
                { "data": "LegalEntity.EmailAddress" },
                { "data": "LegalEntity.MobileNumber" },
                { "data": "LegalEntity.Gender.GenderName" },
                { "data": null, "searchable": false, "orderable": false, 
                    render: function(data, type, full, meta){
                        return '<span class="dropdown pmd-dropdown dropup clearfix">'
                                +'<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button" id="drop-role-'+full.SystemUserId+'" data-toggle="dropdown" aria-expanded="true">'
                                    +'<i class="material-icons pmd-sm">more_vert</i>'
                                +'</button>'
                                +'<ul aria-labelledby="drop-role-'+full.SystemUserId+'" role="menu" class="dropdown-menu pmd-dropdown-menu-top-right">'
                                    +'<li role="presentation"><a class="view" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="'+full.SystemUserId+'" role="menuitem">Details</a></li>'
                                    + '<li role="presentation"><a class="block-posting" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="' + full.SystemUserId +'" role="menuitem">Block User From Posting</a></li>'
                                    + '<li role="presentation"><a class="disable" style="color:#000" href="javascript:void(0);" tabindex="-1" data-value="' + full.SystemUserId +'" role="menuitem">Disable User</a></li>'
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
                        SystemUserType: 2,//default for mobile user
                        ApprovalStatus: 2,
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
                "sLengthMenu": "<div class='mobileUser-lookup-table-length-menu form-group pmd-textfield pmd-textfield-floating-label'><label>Rows per page:</label>_MENU_</div>",
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
                $(".mobileUser-lookup-table-length-menu select").select2({
                    theme: "bootstrap",
                    minimumResultsForSearch: Infinity,
                });
                circleProgress.close();
                setInterval(initUserStatus, 1000);
            }
        });
		$(".custom-select-action").html('<button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button"><i class="material-icons pmd-sm">delete</i></button><button class="btn btn-sm pmd-btn-fab pmd-btn-flat pmd-ripple-effect btn-primary" type="button"><i class="material-icons pmd-sm">more_vert</i></button>');
        dataTableSystemUser.columns.adjust();
    };
    return  {
        init: init
    };
}
var mobileUser = new mobileUserController;
