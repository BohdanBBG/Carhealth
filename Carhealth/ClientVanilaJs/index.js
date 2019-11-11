//import "@babel/polyfill";


import HttpUtil from './modules/HttpUtil.js'
let helper = new HttpUtil();

import DomUtil from './modules/DomUtil.js';
let domUtil = new DomUtil();

import globalScopes from './modules/pages/global-scopes.js';

import AppRouter from './modules/AppRouter.js';
let appRouter = {};


document.addEventListener("DOMContentLoaded", function (event) {
    helper.httpGet('https://localhost:5001/home/config', function (config) {


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

                helper.httpChek('https://localhost:5001/Account/Logout', function (data) {})
                
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
                    var url = config.urls.api + "/home/totalride";
                    var SendRide = {};
                    SendRide.TotalRide = document.forms.RideForm.elements.ride.value;
                    SendTotalRide(url, SendRide);
                } else {
                    document.forms.RideForm.elements.ride.classList.add('input-field-empty-js');
                }
            });
        }

        function GetCarTotalMileage() {
            helper.httpGet(config.urls.api + '/home/car', function (data) {
                document.querySelector('.js-menu-car-total-ride').innerText = data.CarsTotalRide;
            });
        }

        function SendTotalRide(url, TotalRide) {
            helper.httpRequest(url, TotalRide, 'POST', function (request) {
                location.reload();
                alert("Updated total vehicle mileage");

            });
        }

        domUtil.addBubleEventListener('body', '.js-logout-button', 'click', globalScopes.getEventListenerState().logoutButton, function (e, desiredEl) {
            e.stopPropagation();

        });

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

        function checkingServerResponse() {

            function getData(offset = 0, limit = 2, callBack = null) {
                helper.httpGet(config.urls.api + '/home/cardetails' + '/' + offset + '/' + limit, function (data) {
                    console.log(2, data);
                    if (callBack !== null) {
                        callBack(data);
                    }
                });
            }

            getData(0, 1, function (response) {
                if (response.length === 0) {
                    console.log('respone', false);
                    var descriptionIssue = document.querySelector('.js-empty-data');
                    descriptionIssue.classList.replace('hidden', 'active');

                    appRouter.goToRoute("#no-response");
                }
            });
        }//checks server connection

        checkingServerResponse();
    }


}
