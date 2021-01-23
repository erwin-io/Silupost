var accountController = function(){

    var apiService = function(){
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
            logoutUser : logoutUser
        };
    }
    var api = new apiService;

    var init = function(obj){
    	initEvent();
    }

    var initEvent = function(){
        $("#btn-logout").on("click", LogoutUser);
    }
	var LogoutUser = function(){
        Swal.fire({
            title: 'Logout',
            text: "Do you want to continue?",
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
				api.logoutUser().done(function(result){
					if (result.Success) {
                        window.location = "/";
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
	}


    return  {
    	init:init
    };
}
var account = new accountController;