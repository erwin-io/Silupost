
var locationController = function() {

    var apiService = function (apiURI) {

        return {
            getById : getById
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
    };

    var iniValidation = function() {
    };

    var initEvent = function () {
        $("#btnAdd").on("click", Add);
        $("#btnSave").on("click", Save);
    };

    var initGrid = function() {
    };

    var Add = function(){
        appSettings.status.IsNew = true;
    }

    var Edit = function(){
    }
    //Save Data Function 
    var Save = function(e){
    }

    var Delete = function() {
    };

    return  {
        init: init
    };
}
var location = new locationController;
