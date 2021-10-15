
var reportTrackerController = function() {

    var apiService = function (apiURI) {

        var getPageByTracker = function (filter) {
            return $.ajax({
                url: apiURI + "CrimeIncidentReport/GetPageByTracker?",
                data: filter,
                type: "GET",
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                headers: {
                    Authorization: 'Bearer ' + appSettings.apiToken
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
                    Authorization: 'Bearer ' + appSettings.apiToken
                }
            });
        }
        return {
            getLookup: getLookup,
            getPageByTracker: getPageByTracker
        };
    }
    var api = new apiService(app.appSettings.silupostWebAPIURI);

    var map, formTrackerFilter, formTrackerFilterCrimeIncidentCategory, locationPicker;
    var appSettings = {
        model: {},
        currentId: null,
        IsMapLoaded: false,
        IsSettingsReady: false,
        container: 'mapVeiw',
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [120.5960, 16.4023],
        zoom: 12.15,
        features: [],
        apiRefreshToken: "",
        apiToken: "",
    };
    var init = function (obj) {
        circleProgress.show(true);
        setTimeout(function () {
            initLookup();
        }, 1000);
    };

    var initLookup = function () {
        api.getLookup("EntityApprovalStatus,CrimeIncidentCategory").done(function (data) {
            appSettings.lookup = $.extend(appSettings.lookup, data.Data);
            appSettings.IsSettingsReady = true;
            iniTrackerFilter();
        });
    }

    var initMapStyle = function () {
        var width = $(window).width();
        var height = $(window).height();
        if (!appSettings.IsMapMoving) {
            if (width > 0 && height > 0) {
                var sideBarPosition = $("#basicSidebar").position();
                var sideBarWidth = sideBarPosition.left >= 0 ? $("#basicSidebar").width() : 0;
                var navBarHeight = $(".navbar").height();
                $("#" + appSettings.container).css("width", (width));
                $("#" + appSettings.container).css("height", (height - navBarHeight));
                $("#" + appSettings.container).css("right", 0);
                $("#" + appSettings.container).css("bottom", 0);
                $("#" + appSettings.container).css("margin-top", navBarHeight);
            }
            map.resize();
        }
        setTimeout(initMapStyle, 1000);
    }

    var initEvent = function () {
        map.on('load', loadMap);
        map.on('move', function (e) {
            if (appSettings.IsMapLoaded) {
                appSettings.IsMapMoving = true;
                appSettings.center = map.getCenter();
                locationPicker.setLngLat(appSettings.center);
                locationPicker.addTo(map);

                appSettings.center.lat;
                appSettings.center.lng;

                appSettings.trackerFilterMapModel.TrackerPointLatitude = appSettings.center.lat;
                appSettings.trackerFilterMapModel.TrackerPointLongitude = appSettings.center.lng;

                $("#TrackerPointLatitude").val(appSettings.center.lat);
                $("#TrackerPointLongitude").val(appSettings.center.lng);

            }
        });
        $("#btnGetCurrentLocation").on('click', getCurrentLocation);


        $("#modal-dialog-trackerFilterMap #btnSave").on("click", Search);

        map.on("moveend", function () {
            appSettings.IsMapMoving = false;
            if (appSettings.IsMapLoaded) {
                Search();
                map.getSource('source_circle_500').setData({
                    "type": "FeatureCollection",
                    "features": [{
                        "type": "Feature",
                        "geometry": {
                            "type": "Point",
                            "coordinates": [appSettings.center.lng, appSettings.center.lat],
                        }
                    }]
                });

                appSettings.circleRadius = {
                    stops: [
                        [0, 0],
                        [20, KMToPixelsAtMaxZoom(appSettings.trackerFilterMapModel.TrackerRadiusInKM, appSettings.center.lat)]
                    ],
                    base: 2
                };
                map.setPaintProperty('circle500', 'circle-radius', appSettings.circleRadius);
            }
        });
    }
    var initMapEvent = function () {
        // When a click event occurs on a feature in the report layer, open a popup at the
        // location of the feature, with description HTML from its properties.
        map.on('click', "crimeLocation", crimeLocationClick);
        // Change the cursor to a pointer when the mouse is over the places layer.
        map.on('mouseenter', "crimeLocation", function () {
            map.getCanvas().style.cursor = 'pointer';
        });

        // Change it back to a pointer when it leaves.
        map.on('mouseleave', "crimeLocation", function () {
            map.getCanvas().style.cursor = '';
        });


        //$(".mapboxgl-ctrl-geolocate").on("click", function () {

        //});

    }

    var loadMap = function (e) {
        map.resize();
        map.addControl(
            new mapboxgl.GeolocateControl({
                positionOptions: {
                    enableHighAccuracy: true
                },
                trackUserLocation: true
            })
        );
        initMapStyle();
        map.loadImage('http://silupostweb-001-site1.htempurl.com/api/v1/File/GetDefaultCrimeReportMarkerIcon?',
            function (error, image) {
                map.addImage("crimeLocation", image);
                map.addSource("crimeLocation", {
                    'type': 'geojson',
                    'data': {
                        'type': 'FeatureCollection',
                        'features': appSettings.features
                    }
                });
                // Add a layer showing the places.
                map.addLayer({
                    'id': "crimeLocation",
                    'type': 'symbol',
                    'source': "crimeLocation",
                    'layout':
                    {
                        'icon-image': "crimeLocation",
                        'icon-allow-overlap': false,
                        'icon-size': 0.15,
                        'text-field': ['get', 'title'],
                        'text-font': [
                            'Open Sans Bold',
                            'Arial Unicode MS Bold'
                        ],
                        'text-offset': [0, -4],
                        'text-anchor': 'top'
                    },
                    paint: {
                        "text-color": "Red"
                    }
                });
        });
        initMapEvent(); 

        //center location picker
        locationPicker = new mapboxgl.Marker({ "color": "#FF5252" });
        //radius marker

        appSettings.center = map.getCenter();
        locationPicker.setLngLat(appSettings.center);
        locationPicker.addTo(map);


        appSettings.circleRadius = {
            stops: [
                [0, 0],
                [20, KMToPixelsAtMaxZoom(appSettings.trackerFilterMapModel.TrackerRadiusInKM, appSettings.center.lat)]
            ],
            base: 2
        };

        map.addSource("source_circle_500", {
            "type": "geojson",
            "data": {
                "type": "FeatureCollection",
                "features": [{
                    "type": "Feature",
                    "geometry": {
                        "type": "Point",
                        "coordinates": [appSettings.center.lng, appSettings.center.lat],
                    }
                }]
            }
        });
        map.addLayer({
            "id": "circle500",
            "type": "circle",
            "source": "source_circle_500",
            "paint": {
                "circle-radius": appSettings.circleRadius,
                "circle-color": "#9894FF",
                "circle-opacity": 0.5
            }
        });

        circleProgress.close();
        appSettings.IsMapLoaded = true;
    }

    var iniTrackerFilter = function () {
        var date = new Date();

        var _dateFrom = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));
        var _dateTo = date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate()));
        var _timeFrom = date.getHours() + ":" + (date.getMinutes() < 10 ? '0' : '') + date.getMinutes() + ":" + date.getSeconds();
        var _timeto = date.getHours() + ":" + (date.getMinutes() < 10 ? '0' : '') + date.getMinutes() + ":" + date.getSeconds();

        appSettings.trackerFilterMapModel = {
            ApprovalStatusId: 3,
            CrimeIncidentCategoryIds: [],
            TrackerRadiusInKM: 30,
            DateReportedFrom: moment(_dateFrom).format("MM/DD/YYYY"),
            DateReportedTo: moment(_dateTo).format("MM/DD/YYYY"),
            PossibleDateFrom: moment(_dateFrom).format("MM/DD/YYYY"),
            PossibleDateTo: moment(_dateTo).format("MM/DD/YYYY"),
            PossibleTimeFrom: _timeFrom,
            PossibleTimeTo: _timeto,
        };

        $.each(appSettings.lookup.EntityApprovalStatus, function (key, value) {
            $('#ApprovalStatusId')
                .append($("<option></option>")
                    .attr("value", value.Id)
                    .text(value.Name));
        });
        appSettings.trackerFilterMapModel.SelectedCrimeIncidentCategory = [];
        $.each(appSettings.lookup.CrimeIncidentCategory, function (key, value) {
            appSettings.trackerFilterMapModel.SelectedCrimeIncidentCategory.push(value.Id);
            $('#CrimeIncidentTypes')
                .append($("<option></option>")
                    .attr("value", value.Id)
                    .text(value.Name));

        });

        $("#CrimeIncidentTypes").val(appSettings.trackerFilterMapModel.CrimeIncidentCategoryIds);
        appSettings.trackerFilterMapModel.CrimeIncidentCategoryIds = appSettings.trackerFilterMapModel.SelectedCrimeIncidentCategory.toString();

        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
        //end custom init


        $('#DateReportedFrom').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#DateReportedTo').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#PossibleDateFrom').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#PossibleDateTo').datetimepicker({
            format: 'MM/DD/YYYY'
        });

        // START Time picker only
        $('#PossibleTimeFrom').datetimepicker({
            format: 'LT'
        });
        $('#PossibleTimeTo').datetimepicker({
            format: 'LT'
        });
        // END Time picker only


        $('#DateReportedFrom').parent().addClass('pmd-textfield-floating-label-completed');
        $('#DateReportedTo').parent().addClass('pmd-textfield-floating-label-completed');
        $('#PossibleDateFrom').parent().addClass('pmd-textfield-floating-label-completed');
        $('#PossibleDateTo').parent().addClass('pmd-textfield-floating-label-completed');
        $('#PossibleTimeFrom').parent().addClass('pmd-textfield-floating-label-completed');
        $('#PossibleTimeTo').parent().addClass('pmd-textfield-floating-label-completed');

        $(".select-simple").select2({
            theme: "bootstrap",
            minimumResultsForSearch: Infinity,
        });
        $('.select-simple').parent().addClass('pmd-textfield-floating-label-completed');

        // single range slider
        var TrackerRadiusInKM = document.getElementById('TrackerRadiusInKM');
        noUiSlider.create(TrackerRadiusInKM, {
            start: [appSettings.trackerFilterMapModel.TrackerRadiusInKM],
            connect: 'lower',
            tooltips: [wNumb({ decimals: 0 })],
            range: {
                'min': [0],
                'max': [100]
            }
        });

        TrackerRadiusInKM.noUiSlider.on('set.one', function () {
            appSettings.trackerFilterMapModel.TrackerRadiusInKM = TrackerRadiusInKM.noUiSlider.get();

            if (appSettings.IsMapLoaded) {
                map.getSource('source_circle_500').setData({
                    "type": "FeatureCollection",
                    "features": [{
                        "type": "Feature",
                        "geometry": {
                            "type": "Point",
                            "coordinates": [appSettings.center.lng, appSettings.center.lat],
                        }
                    }]
                });

                appSettings.circleRadius = {
                    stops: [
                        [0, 0],
                        [20, KMToPixelsAtMaxZoom(appSettings.trackerFilterMapModel.TrackerRadiusInKM, appSettings.center.lat)]
                    ],
                    base: 2
                };
                map.setPaintProperty('circle500', 'circle-radius', appSettings.circleRadius);
            }

            Search();
        });


        $("#CrimeIncidentTypes").on("change", function () {
            appSettings.trackerFilterMapModel.SelectedCrimeIncidentCategory = $(this).select2("val");
            appSettings.trackerFilterMapModel.CrimeIncidentCategoryIds = appSettings.trackerFilterMapModel.SelectedCrimeIncidentCategory.toString();
        });
        $("#ApprovalStatusId").on("change", function () {
            appSettings.trackerFilterMapModel.ApprovalStatusId = $(this).val();
            Search();
        });

        $("#TrackerPointLatitude").on("focusout", function () {
            appSettings.trackerFilterMapModel.TrackerPointLatitude = $(this).val();
            Search();
        });

        $("#TrackerPointLongitude").on("focusout", function () {
            appSettings.trackerFilterMapModel.TrackerPointLongitude = $(this).val();
            Search();
        });


        $("#DateReportedFrom").on("focusout", function () {
            appSettings.trackerFilterMapModel.DateReportedFrom = $(this).val();
        });
        $("#DateReportedTo").on("focusout", function () {
            appSettings.trackerFilterMapModel.DateReportedTo = $(this).val();
            Search();
        });
        $("#PossibleDateFrom").on("focusout", function () {
            appSettings.trackerFilterMapModel.PossibleDateFrom = $(this).val();
            Search();
        });
        $("#PossibleDateTo").on("focusout", function () {
            appSettings.trackerFilterMapModel.PossibleDateTo = $(this).val();
            Search();
        });
        $("#PossibleTimeFrom").on("focusout", function () {
            appSettings.trackerFilterMapModel.PossibleTimeFrom = moment($(this).val(), ["h:mm A"]).format("HH:mm");
            Search();
        });
        $("#PossibleTimeTo").on("focusout", function () {
            appSettings.trackerFilterMapModel.PossibleTimeTo = moment($(this).val(), ["h:mm A"]).format("HH:mm");
            Search();
        });
        //set default values
        $('#ApprovalStatusId').val(appSettings.trackerFilterMapModel.ApprovalStatusId);
        $('#DateReportedFrom').val(appSettings.trackerFilterMapModel.DateReportedFrom);
        $('#DateReportedTo').val(appSettings.trackerFilterMapModel.DateReportedTo);
        $('#PossibleDateFrom').val(appSettings.trackerFilterMapModel.PossibleDateFrom);
        $('#PossibleDateTo').val(appSettings.trackerFilterMapModel.PossibleDateTo);
        $('#PossibleTimeFrom').val(appSettings.trackerFilterMapModel.PossibleTimeFrom);
        $('#PossibleTimeTo').val(appSettings.trackerFilterMapModel.PossibleTimeTo);

        mapboxgl.accessToken = app.appSettings.mapBoxToken;
        map = new mapboxgl.Map({
            container: appSettings.container,
            style: appSettings.style,
            center: appSettings.center,
            zoom: appSettings.zoom
        });

        initEvent();
    }

    var getCurrentLocation = function () {
        if (appSettings.IsMapLoaded) {
            //$(".mapboxgl-ctrl-geolocate").trigger("click");
            document.querySelector(".mapboxgl-ctrl-geolocate").click();
        }
    }

    var toggleFilter = function () {
        $("#basicSidebar").addClass("pmd-sidebar-open");
        if ($("#basicSidebar").hasClass("pmd-sidebar-open")) {
            $("#basicSidebar").removeClass("pmd-sidebar-open")
        }
        else {
            $("#basicSidebar").addClass("pmd-sidebar-open");
        }
    }


    var crimeLocationClick = function (e) {
        var coordinates = e.features[0].geometry.coordinates.slice();
        var description = e.features[0].properties.description;

        // Ensure that if the map is zoomed out such that multiple
        // copies of the feature are visible, the popup appears
        // over the copy being pointed to.
        while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
        }

        new mapboxgl.Popup().setLngLat(coordinates).setHTML(description).addTo(map);
    }

    var KMToPixelsAtMaxZoom = (km, latitude) =>
        (km * 1000) / 0.075 / Math.cos(latitude * Math.PI / 180);

    var Search = function () {
        api.getPageByTracker(appSettings.trackerFilterMapModel).done(function (data) {
            var features = [];
            for (var i in data.Data.Items) {
                var badgeStatus = 'badge-warning';
                if (data.Data.Items[i].ApprovalStatus.ApprovalStatusId === 1) {
                    badgeStatus = 'badge-info';
                } else if (data.Data.Items[i].ApprovalStatus.ApprovalStatusId === 2) {
                    badgeStatus = 'badge-error';
                }
                else {
                    badgeStatus = 'badge-warning';
                }
                var reportStatus = '<span class="badge ' + badgeStatus + '" style="padding: 10px">' + data.Data.Items[i].ApprovalStatus.ApprovalStatusName + '</span>';
                var crimeIncidentReportId = data.Data.Items[i].CrimeIncidentReportId;
                var crimeIncidentCategoryName = data.Data.Items[i].CrimeIncidentCategory.CrimeIncidentCategoryName;
                var dateReported = moment(data.Data.Items[i].DateReported).format("MMMM DD, YYYY");
                var possibleDate = moment(data.Data.Items[i].PossibleDate).format("MMMM DD, YYYY");
                var possibleTime = moment(data.Data.Items[i].PossibleTime, ["HH:mm"]).format("h:mm A");
                var description = data.Data.Items[i].Description;
                var geoTrackerLatitude = data.Data.Items[i].GeoTrackerLatitude;
                var geoTrackerLongitude = data.Data.Items[i].GeoTrackerLongitude;
                features.push({
                    'type': 'Feature',
                    'properties':
                    {
                        'title': crimeIncidentCategoryName,
                        'description':
                            '<h2><a href="/CrimeIncidentReport/Details/' + crimeIncidentReportId + '" target="_blank"  class="mapbox-modal-title pmd-tooltip" data-toggle="tooltip" data-placement="left" title="View Crime/Incident Report details">' + crimeIncidentCategoryName + '</a></h2>' +
                            '<hr/>' +
                            '<p><strong><h5>Date Reported: </h5></strong> <h5>' + dateReported + '</h5></p>' +
                            '<p><strong><h5>Actual Date and Time: </h5></strong> <h5>' + possibleDate + ' @' + possibleTime + '</h5></p>' +
                            '<p><strong><h5>Details: </h5></strong> <h5>' + description + '</h5></p>'
                        ,
                        //'icon': 'information'
                    },
                    'geometry': {
                        'type': 'Point',
                        'coordinates': [geoTrackerLongitude, geoTrackerLatitude],
                    }
                });
            }
            appSettings.features = features;
            var source = map.getSource("crimeLocation");
            if (appSettings.IsMapLoaded && source !== null) {
                map.getSource("crimeLocation").setData({
                    "type": "FeatureCollection",
                    "features": appSettings.features
                });
            }
            $('[data-toggle="tooltip"]').tooltip();
        });

    }

    var flyToLocation = function(lat,lng) {
        map.flyTo({
            center: [lng, lat],
            essential: false // this animation is considered essential with respect to prefers-reduced-motion
        });
    }
    return {
        appSettings: appSettings,
        flyToLocation: flyToLocation,
        Search: Search,
        init: init
    };
}
var reportTracker = new reportTrackerController;
