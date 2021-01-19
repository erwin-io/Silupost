var appController = function(){

	var appSettings = {
		petDatingWebURI: "http://localhost:29100/api/v1/"
	}

	var init = function(){
		initEvent();
		removeSomeDiv();
	}

	var initEvent = function (event) {
    };

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