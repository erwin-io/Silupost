
var homeController = function() {

    var apiService = function (apiURI){
        var generateLocation = function()
        {
            return $.ajax({
                url: apiURI + "/location/getAllCountry",
                data: null,
                type: "GET",
                contentType: "application/json;charset=utf-8",
                dataType: "json"
            });
        }
        return {
            generateLocation: generateLocation
        };
    }
    var api = new apiService(app.appSettings.petDatingWebURI);

    var form,roleForm,dataTableUser;
    var appSettings = {
        model:{},
        status:{ IsNew:false},
        currentId:null
    };
    var init = function (obj) {


        var notificationFromHun = $.connection["SignalRHubWeb"];

        $.connection.hub.start().done(function () {
            alert("connection started");
        });
        notificationFromHun.client.UpdateConversationMessage = function (conversationId) {
            alert("Sent");
        }

        api.generateLocation().done(function (data) {
            console.log(data);

            const result = [];
            const map = new Map();
            for (const item of data.Data) {
                if (!map.has(item.currency)) {
                    map.set(item.currency, true);    // set any value to Map
                    result.push({
                        id: item.id,
                        currency: item.currency,
                        name: item.name
                    });
                }
            }
            console.log(result);
        }).fail(function (data) {
            console.log(data);
        });
    };

    var initLookup = () => {

    }

    var iniValidation = function() {
    };

    var initEvent = function () {
    };

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var home = new homeController;
