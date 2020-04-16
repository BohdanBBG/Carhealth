//import "@babel/polyfill";


import HttpUtil from './modules/HttpUtil.js'
let helper = new HttpUtil();

import DomUtil from './modules/DomUtil.js';
let domUtil = new DomUtil();

import globalScopes from './modules/pages/global-scopes.js';

import AppRouter from './modules/AppRouter.js';
let appRouter = {};

var serverUrl = "https://localhost:5001";


document.addEventListener("DOMContentLoaded", function (event) {

    checkingServerResponse(serverUrl, serverUrl);

    helper.httpGet(serverUrl + "/config", function (config) {
        start(config);

    });
});

function start(config) {

    appRouter = new AppRouter(config);
    runApp(config);

    function runApp(config) {


        /**
         * Inits app menu, enables menu links
         */
        function initAppMenu() {

            domUtil.addBubleEventListener('body', '[data-route-link]', 'click', globalScopes.getEventListenerState().menuLinks, function (e, actualEl, desiredEl) {
                e.preventDefault();
                e.stopPropagation();

                var targetLinkEl = desiredEl;
                appRouter.goToRoute(targetLinkEl.dataset.routeLink);

            });

            GetCarTotalMileage();

            domUtil.addBubleEventListener('.js-logout-button', '.js-logout-button-text', 'click', globalScopes.getEventListenerState().logout, function (e, actualEl, desiredEl) {
                e.preventDefault();
                e.stopPropagation();

                document.location.href = config.urls.api + "/Account/Logout";
            });

            domUtil.addBubleEventListener('.send-ride-button', '.send-ride-button-text', 'click', globalScopes.getEventListenerState().rideSendButton, function (e, actualEl, desiredEl) {

                document.forms.RideForm.elements.ride.value ? document.forms.RideForm.elements.ride.value :
                    document.forms.RideForm.elements.ride.classList.add('input-field-empty-js');

                document.forms.RideForm.elements.ride.onfocus = function () {
                    if (document.forms.RideForm.elements.ride.classList.contains('input-field-empty-js')) {
                        document.forms.RideForm.elements.ride.classList.remove('input-field-empty-js');
                    }
                }
                if (/^\d+$/.test(document.forms.RideForm.elements.ride.value)) {
                    //var url = config.urls.api + '/home/cardetails' + '/' ;
                    var url = config.urls.api + "/totalride/set/" + document.forms.RideForm.elements.ride.value;
                    SendTotalRide(url);
                } else {
                    document.forms.RideForm.elements.ride.classList.add('input-field-empty-js');
                }
            });
        }

        GetUserCars(function (data) {

            data.forEach(element => {

                var newOption = new Option(element.CarEntityName, element.Id, element.IsDefault ? true : false, element.IsDefault ? true : false);

                currentCar.cars.options[currentCar.cars.options.length] = newOption;
            });

        });

        function changeOption() {

            var selectedCar = {};
            selectedCar.CarEntityId = currentCar.cars.options[currentCar.cars.selectedIndex].value;
            helper.httpRequest(config.urls.api + '/setUserCurCar', selectedCar, "POST", function () {
                console.log("Current car: ", currentCar.cars.options[currentCar.cars.selectedIndex].text);
                location.reload();

            });
        }

        currentCar.cars.addEventListener("change", changeOption);

        function GetUserCars(callback) {
            helper.httpGet(config.urls.api + '/allUsersCars', function (data) {
                callback(data);
            });
        }

        function GetCarTotalMileage() {
            helper.httpGet(config.urls.api + '/totalride', function (data) {
                document.querySelector('.js-menu-car-total-ride').innerText = data.carsTotalRide;

            });
        }

        function SendTotalRide(url) {
            helper.httpGet(url, function (request) {
                location.reload();
                alert("Updated total vehicle mileage");

            });
        }

        initAppMenu();

        // listen hash changes
        window.addEventListener('hashchange', function (e) {
            var newHash = window.location.hash;
            appRouter.processRoute(newHash);
        }, false);

        // run route that is already in hash
        if (!!window.location.hash) {
            appRouter.processRoute(window.location.hash);
        }




    }


}

function checkingServerResponse(apiUrl, identityUrl) {


    helper.httpChek(apiUrl + "/ping", function (response) {
        if (response === 401 || response === 403) {
            document.location.href = identityUrl + "/Account/Login";
        }
        else if (response.statusCode === 500) {
            console.log('response', false);
            var descriptionIssue = document.querySelector('.js-empty-data');
            descriptionIssue.classList.replace('hidden', 'active');

            appRouter.goToRoute("#no-response");
        }
    });


}//checks server connection
