
var legalEntityController = function() {



    var form;
    var appSettings = {
        model:null,
        status:{ IsNew:false},
    };
    var init = function (obj) {
        form = $('#form-legalentity');
        iniValidation();
        initEvent();
    };

    var iniValidation = function() {
        form.validate({
            ignore:[],
            rules: {
                LegalEntityId: {
                    required: true
                },
                Firstname: {
                    required: true
                },
                Middlename: {
                    required: false
                },
                Lastname: {
                    required: true
                },
                BirthDate: {
                    required: true
                },
                Gender: {
                    required: true
                }
            },
            messages: {
                LegalEntityId: "Please enter LegalEntityId",
                Firstname: "Please enter a Firstname",
                Middlename: "Please enter Middlename",
                Lastname: "Please enter Lastname",
                BirthDate: "Please select Birth Date",
                Gender: "Please select Gender",
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
        $('#BirthDate').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
    };

    var valid = ()=>{
        return form.valid();
    }
    
    var Save = function(target,obj,url,dataTable) {
        if(!form.valid())
            return;
        Swal.fire({
            title: (appSettings.status.IsNew ? 'Save' : 'Update'),
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
                var targetName = target.attr("data-name");
                target.html(targetName+'&nbsp;<span class="spinner-border spinner-border-sm"></span>');
                circleProgress.show(true);
                $.ajax({
                    url: url,
                    data: JSON.stringify(obj),
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.Success) {
                            Swal.fire("Success!",result.Message,"success");
                            $("#modal-dialog").modal('hide');
                        } else {
                            Swal.fire('Error!',result.Message,'error');
                        }
                        $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                        target.empty();
                        target.html(targetName);
                        dataTable.ajax.reload();
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
    };
    var Delete = function(target,obj,url,dataTable) {
        if(obj.LegalEntityId!= ""){
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
                    var targetName = target.attr("data-name");
                    target.html(targetName+'&nbsp;<span class="spinner-border spinner-border-sm"></span>');
                    circleProgress.show();
                    $.ajax({
                        url: url,
                        data: JSON.stringify({ LegalEntityId: obj.LegalEntityId}),
                        type: "POST",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            if (result.Success) {
                                Swal.fire("Success!",result.Message,"success");
                            } else {
                                Swal.fire('Error!',result.Message,'error');
                            }
                            $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                            target.empty();
                            target.html(targetName);
                            dataTable.ajax.reload();
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
    };

    //Function for clearing the textboxes
    return  {
        appSettings: appSettings,
        valid: valid,
        init: init,
        Save: Save,
        Delete: Delete,
    };
}
var legalEntity = new legalEntityController;
