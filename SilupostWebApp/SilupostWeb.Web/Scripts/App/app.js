var appController = function(){

	var appSettings = {
		silupostWebAPIURI: "http://localhost:8100/api/v1/",
		apiToken: "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJTeXN0ZW1Vc2VySWQiOiJTVS0wMDAwMDAwMDAxIiwibmJmIjoxNjEyMDg5MTI0LCJleHAiOjE2MTIxNzU1MjQsImlzcyI6Imh0dHA6Ly93d3cuY3JpbWVyZXBvcnRhcHAuY29tIiwiYXVkIjoiQUU0OUEyQ0NCMkM1M0MyQUYyMTY1QjYwODc0RDcxOTEifQ._JJMhrYb8wUAuHnC_gmr0E0v4hM8GWCLzWwC7d5TQug",
		mapBoxToken: "pk.eyJ1IjoiZXJ3aW5yYW1pcmV6MjIwIiwiYSI6ImNrZ3U1cHJzazAwYTAycm82MDRmdWNmczAifQ.TarlRjuzi62vw_hPR6uTGg"
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