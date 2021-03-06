//import "@babel/polyfill";

import DomUtil from './modules/DomUtil.js';
let domUtil = new DomUtil();

import globalScopes from './modules/pages/global-scopes.js';

import AppRouter from './modules/AppRouter.js';
let appRouter = {};

import CarManager from './modules/pages/manage-car.js'
let carManager = {};

import HttpUtil from './modules/HttpUtil.js'
let helper = new HttpUtil();


import UserUtil from './modules/UserUtil.js';
export let userUtil = {};


document.addEventListener("DOMContentLoaded", function (event) {

    helper.httpGet("/config", function (config) {
        start(config);
    });
});

function start(config) {

    userUtil = new UserUtil(config);

    userUtil.getUser(function (user) {

        if (user) {
            console.log('User:', user);
            appRouter = new AppRouter(user, config);
            carManager = new CarManager(user, config);
            runApp(user, config);

        } else {
            console.log("User not logged in");
            userUtil.login();
        }
    });



    function runApp(user, config) {


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

            domUtil.addBubleEventListener('.js-logout-button', '.js-logout-button-text', 'click', globalScopes.getEventListenerState().logout, function (e, actualEl, desiredEl) {
                e.stopPropagation();

                userUtil.logout();
            });


            

            helper.httpGet(`${config.auth.authority}/Account/IsAdmin?email=${user.profile.email}`, function (response) {

                if (response === true) {

                    document.querySelector(".js-for-admins-only").classList.remove('hidden');

                    domUtil.addBubleEventListener('body', '.js-for-admins-only', 'click', globalScopes.getEventListenerState().AdminsOnly, function (e, actualEl, desiredEl) {
                        e.preventDefault();
                        e.stopPropagation();

                        document.location.href = `${config.auth.authority}/Users`;
                    });
                }
                else
                {
                    document.querySelector(".js-for-admins-only").classList.add('hidden');
                }
            }, user.access_token);
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

function checkingServerResponse(apiUrl, user) {

    helper.httpChek(apiUrl + "/ping", user.access_token, function (response) {

    });


}//checks server connection
