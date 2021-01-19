var loginController = function(){

    var apiService = function(){
        var getByCredentials = function(model)
        {
            return $.ajax({
                url: "/Account/GetByCredentials",
                data: { Username: model.Username, Password: model.Password},
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }

        return {
            getByCredentials : getByCredentials
        };
    }
    var api = new apiService;

    var form;
    var appSettings = {
        model : {}
    }
	var init = function(){

        setTimeout(function(){
            var loginTemplate = $.templates('#login-template');
            loginTemplate.link(".logincard", appSettings.model);
            form = $("#form-login");
            initValidation();
            initEvent();
        }, 1000);
	}


    var initValidation = function() {
        form.validate({
            ignore:[],
            rules: {
                Username: {
                    required: true
                },
                Password: {
                    required: true
                }
            },
            messages: {
                Username: {
                    required: "Please enter Username"
                },
                Password: {
                    required: "Please enter Password"
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

    var initEvent = function () {
        $("#btn-login").on("click", Login);
    };

    var Login = function(){
        if(!form.valid())
            return;
        else{
            Swal.fire({
                title: 'Login',
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
                    api.getByCredentials(appSettings.model)
                    .done(function(result){
                        if (result.Success) {
                            Swal.fire({
                                title: "Success!",
                                text: "Successfully Login!",
                                type: "success",
                                allowOutsideClick: false
                            }).then((result) => {
                                window.location.replace("/");
                            });
                        } else {
                            Swal.fire('Error!',result.Message,'error');
                        }
                        $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                        circleProgress.close();
                    }).error(function(errormessage){
                        $(".content").find("input,button,a").prop("disabled", false).removeClass("disabled");
                        Swal.fire('Error!',errormessage.Message,'error');
                        circleProgress.close();
                    });
                }
            });
        }
    }

    return  {
        init: init
    };
}
var login = new loginController;