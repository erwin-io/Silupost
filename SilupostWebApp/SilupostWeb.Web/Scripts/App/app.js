var appController = function () {

    var apiService = function () {
        var getRefreshToken = function (refreshToken) {
            return $.ajax({
                url: appSettings.silupostWebAPIURI + "SystemUser/GetRefreshToken?RefreshToken=" + refreshToken,
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
            });
        }

        var getAppRefreshToken = function () {
            return $.ajax({
                url: "/Account/GetApplicationState/",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json"
            });
        }
        var getApplicationState = function () {
            return $.ajax({
                url: "/Account/GetApplicationState",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json"
            });
        }
        var setAppRefreshToken = function (ApplicationToken) {
            return $.ajax({
                url: "/Account/SetApplicationToken",
                type: "POST",
                data: JSON.stringify({ ApplicationToken: ApplicationToken }),
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
            });
        }
        var setApplicationState = function (applicationState) {
            return $.ajax({
                url: "/Account/SetApplicationState",
                data: JSON.stringify({ ApplicationState: applicationState }),
                type: "POST",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
            });
        }

        return {
            getRefreshToken: getRefreshToken,
            getAppRefreshToken: getAppRefreshToken,
            getApplicationState: getApplicationState,
            setAppRefreshToken: setAppRefreshToken,
            setApplicationState: setApplicationState
        };
    }
    var api = new apiService();

    var appSettings = {
        silupostWebAPIURI: "http://localhost:8100/api/v1/",//api link
        apiToken: "",
        authorized: false,
        apiRefreshToken: "",
        refreshTokenInterval: 10000000,
        apiCLient: "silupost",
        mapBoxToken: "pk.eyJ1IjoiZXJ3aW5yYW1pcmV6MjIwIiwiYSI6ImNrZ3U1cHJzazAwYTAycm82MDRmdWNmczAifQ.TarlRjuzi62vw_hPR6uTGg"
    }

    var init = function (obj) {
        appSettings = $.extend(appSettings, obj);
        if (appSettings.AllowAnonymous === undefined || appSettings.AllowAnonymous !== null) {
            if (!appSettings.AllowAnonymous)
                initAppUser();
        }
        removeSomeDiv();
        initEvent();
    }

    var initEvent = function () {
        $("#btnLogout").on("click", function () {
            api.setAppRefreshToken(null).done(function () {
                api.setApplicationState(null).done(function () {
                    window.location.replace("/Account/Login");
                });
            });
        });
    };

    var initAppUser = function () {
        if (window.location.pathname === "/Account/Login") {
            appSettings.authorized = false;
        } else if (window.location.pathname === "/Account/Register") {
            appSettings.authorized = false;
        } else if (window.location.pathname === "/Account/ResetPassword") {
            appSettings.authorized = false;
        }else {
            api.getApplicationState().done(function (result) {
                if (result.Success) {
                    appSettings.appState = result.Data;
                    appSettings.apiToken = result.Data.ApplicationToken.AccessToken;
                    appSettings.apiRefreshToken = result.Data.ApplicationToken.RefreshToken;
                    appSettings.authorized = true;
                }
                if (appSettings.authorized) {
                    api.getRefreshToken(appSettings.apiRefreshToken).done(function (result) {
                        appSettings.apiToken = result.Data.access_token;
                        appSettings.apiRefreshToken = result.Data.refresh_token;
                        var appToken = {
                            AccessToken: appSettings.apiToken,
                            RefreshToken: appSettings.apiRefreshToken,
                        }
                        api.setAppRefreshToken(appToken).done(function (result) {
                            setInterval(getRefreshToken, appSettings.refreshTokenInterval);
                        }).error(function (result) {
                            window.location.replace("/Account/Login");
                        });
                    }).error(function (result) {
                        window.location.replace("/Account/Login");
                    });
                }
                else {
                    window.location.replace("/Account/Login");
                }
            }).error(function (result) {
                window.location.replace("/Account/Login");
            });
        }
    }

    var getRefreshToken = function () {
        if (appSettings.authorized) {
            api.getRefreshToken(appSettings.apiRefreshToken).done(function (data) {
                appSettings.apiToken = data.Data.access_token;
                appSettings.apiRefreshToken = data.Data.refresh_token;
                var appToken = {
                    AccessToken: appSettings.apiRefreshToken,
                    RefreshToken: appSettings.apiRefreshToken,
                }
                api.setAppRefreshToken(appToken).error(function (result) {
                    window.location.replace("/Account/Login");
                });
            }).error(function (result) {
                window.location.replace("/Account/Login");
            });
        } else {
            window.location.replace("/Account/Login");
        }
    }

	var removeSomeDiv = function(){
		$(document).ready(function(){
			var divToRemove = $("body").find("div.lastDivTag").nextAll();
			if(divToRemove.length !== 0){
				for(var i in divToRemove)
				{
					divToRemove[i].remove();
				}
			}
		});
	}

	return {
		appSettings: appSettings,
        init: init
    };
}
var app = new appController;