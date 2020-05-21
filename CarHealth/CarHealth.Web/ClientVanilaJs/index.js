//import "@babel/polyfill";



import DomUtil from './modules/DomUtil.js';
let domUtil = new DomUtil();

import globalScopes from './modules/pages/global-scopes.js';

import AppRouter from './modules/AppRouter.js';
let appRouter = {};

import CarManager from './modules/pages/manage-car.js'


var serverUrl = "https://localhost:5001";
var identityUrl = "https://localhost:5001";

import HttpUtil from './modules/HttpUtil.js'
let helper = new HttpUtil(identityUrl);



document.addEventListener("DOMContentLoaded", function (event) {

    checkingServerResponse(serverUrl, identityUrl);

    domUtil.addBubleEventListener('.js-logout-button', '.js-logout-button-text', 'click', globalScopes.getEventListenerState().logout, function (e, actualEl, desiredEl) {
        e.preventDefault();
        e.stopPropagation();

        document.location.href = serverUrl + "/Account/Logout";
    });

    helper.httpGet(serverUrl + "/config", function (config) {

        //config.urls.api="";
        start(config);

    });
});

function start(config) {

    appRouter = new AppRouter(config);
    let carManager = new CarManager(config);
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

         

            domUtil.addBubleEventListener('body', '[data-car-manager-id]', 'click', globalScopes.getEventListenerState().carManager, function (e, actualEl, desiredEl) {

                carManager.handler();

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
       
    });


}//checks server connection
