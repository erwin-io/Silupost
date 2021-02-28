
var crimeIncidentReportDetailsController = function() {

    var apiService = function (apiURI,apiToken) {
        var getById = function (Id) {
            return $.ajax({
                url: apiURI + "CrimeIncidentReport/" + Id + "/detail",
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + apiToken
                }
            });
        }

        var getMediaFiles = function (fileId) {
            return $.ajax({
                url: apiURI + "File/getFile?FileId=" + fileId,
                type: "GET",
                headers: {
                    Authorization: 'Bearer ' + apiToken
                }
            });
        }

        var getLookup = function (tableNames) {
            return $.ajax({
                url: apiURI + "SystemLookup/GetAllByTableNames?TableNames=" + tableNames,
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + apiToken
                }
            });
        }


        return {
            getById: getById, 
            getMediaFiles: getMediaFiles, 
            getLookup: getLookup
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI,app.appSettings.apiToken);

    var form,dataTableCrimeIncidentReportMedia;
    var appSettings = {
        model: {},
        status:{ IsNew:false},
        currentId:null,
        CanEditReportHeader:false,
        CanEditReportMedia:false,
        CanApproveReportHeader:false
    };
    var init = function (obj) {
        appSettings = $.extend(appSettings, obj);
        initEvent();
        initLookup();
    };

    var initLookup = function(){
        api.getLookup("CrimeIncidentCategory,CrimeIncidentReport,EnforcementStation").done(function (data) {
        	appSettings.lookup = $.extend(appSettings.lookup, data.Data);

            console.log(appSettings.lookup);
            initDetails();
        });
    }

    var iniValidation = function() {
    };

    var initEvent = function () {
    }

    var initDetails = function(){
        api.getById(appSettings.CrimeIncidentReportId).done(function (data) {
            console.log(data);

            var crimeIncidentReportDetailsTemplate = $.templates('#crimeIncidentReport-template');

            // appSettings.model = data.Data;
            appSettings.model = $.extend(appSettings.model, data.Data);
            appSettings.model = $.extend(appSettings.model, data.Data.PostedBySystemUser);
            appSettings.model = $.extend(appSettings.model, data.Data.PostedBySystemUser.LegalEntity);
            appSettings.model = $.extend(appSettings.model, data.Data.ApprovalStatus);
            appSettings.model = $.extend(appSettings.model, data.Data.CrimeIncidentCategory);
            appSettings.model = $.extend(appSettings.model, data.Data.CrimeIncidentCategory.CrimeIncidentType);
            appSettings.model.CanEditReportHeader = appSettings.CanEditReportHeader;
            appSettings.model.CanEditReportMedia = appSettings.CanEditReportMedia;
            appSettings.model.CanApproveReportHeader = appSettings.CanApproveReportHeader;
            appSettings.model.Validated = true;
            appSettings.model.lookup = appSettings.lookup;
            console.log(appSettings.model);


            $(".select-simple").select2({
                theme: "bootstrap",
                minimumResultsForSearch: Infinity,
            });
            let promises = [];
            for (var i in appSettings.model.CrimeIncidentReportMedia) {
                if (appSettings.model.CrimeIncidentReportMedia[i].File != undefined)
                    appSettings.model.CrimeIncidentReportMedia[i].File.FileURL = app.appSettings.silupostWebAPIURI + "File/getFile?FileId=" + appSettings.model.CrimeIncidentReportMedia[i].File.FileId;
            }
            crimeIncidentReportDetailsTemplate.link("#reportView", appSettings.model);
            initCrimeIncidentReportMediaGallery();  

        });
    }


    var initCrimeIncidentReportMediaGallery = function() {
        let modalId = $('#image-gallery');

        $(document)
          .ready(function () {

            loadGallery(true, 'a.thumbnail');

            //This function disables buttons when needed
            function disableButtons(counter_max, counter_current) {
              $('#show-previous-image, #show-next-image')
                .show();
              if (counter_max === counter_current) {
                $('#show-next-image')
                  .hide();
              } else if (counter_current === 1) {
                $('#show-previous-image')
                  .hide();
              }
            }

            /**
             *
             * @param setIDs        Sets IDs when DOM is loaded. If using a PHP counter, set to false.
             * @param setClickAttr  Sets the attribute for the click handler.
             */

            function loadGallery(setIDs, setClickAttr) {
              let current_image,
                selector,
                counter = 0;

              $('#show-next-image, #show-previous-image')
                .click(function () {
                  if ($(this)
                    .attr('id') === 'show-previous-image') {
                    current_image--;
                  } else {
                    current_image++;
                  }

                  selector = $('[data-image-id="' + current_image + '"]');
                  updateGallery(selector);
                });

              function updateGallery(selector) {
                let $sel = selector;
                current_image = $sel.data('image-id');
                $('#image-gallery-title')
                  .text($sel.data('title'));
                $('#image-gallery-image')
                  .attr('src', $sel.data('image'));
                disableButtons(counter, $sel.data('image-id'));
              }

              if (setIDs == true) {
                $('[data-image-id]')
                  .each(function () {
                    counter++;
                    $(this)
                      .attr('data-image-id', counter);
                  });
              }
              $(setClickAttr)
                .on('click', function () {
                  updateGallery($(this));
                });
            }
          });

        // build key actions
        $(document)
          .keydown(function (e) {
            switch (e.which) {
              case 37: // left
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-previous-image').is(":visible")) {
                  $('#show-previous-image')
                    .click();
                }
                break;

              case 39: // right
                if ((modalId.data('bs.modal') || {})._isShown && $('#show-next-image').is(":visible")) {
                  $('#show-next-image')
                    .click();
                }
                break;

              default:
                return; // exit this handler for other keys
            }
            e.preventDefault(); // prevent the default action (scroll / move caret)
          });

    };

    //Function for clearing the textboxes
    return  {
        init: init
    };
}
var crimeIncidentReportDetails = new crimeIncidentReportDetailsController;
