var appController = function(){

	var appSettings = {
		silupostWebAPIURI: "http://localhost:8100/api/v1/",
		apiToken: "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRXJ3aW5AcmFtaXJlejMiLCJTeXN0ZW1Vc2VySWQiOiJTVS0wMDAwMDAwMDAxIiwibmJmIjoxNjExNDYxODg1LCJleHAiOjE2MTE1NDgyODUsImlzcyI6Imh0dHA6Ly93d3cuY3JpbWVyZXBvcnRhcHAuY29tIiwiYXVkIjoiQUU0OUEyQ0NCMkM1M0MyQUYyMTY1QjYwODc0RDcxOTEifQ.hiafwJB7WJW-Rb6fDktk53Ok5vefL9K71QyjlphG1UQ"
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