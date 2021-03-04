
var reportTrackerController = function() {

    var apiService = function (apiURI) {
        return {
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI);

    var appSettings = {
        model: {},
        currentId:null
    };
    var init = function (obj) {

        initEvent();
        setTimeout(function () {
        }, 1000);
    };

    var initEvent = function () {
    }
    return  {
        init: init
    };
}
var reportTracker = new reportTrackerController;
