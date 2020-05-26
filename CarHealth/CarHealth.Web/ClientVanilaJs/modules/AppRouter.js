
import Dashboard from './pages/dashboard.js';
import ManageCar from './pages/manage-car.js';
import CarDetails from './pages/car-detail.js';

class AppRouter {

    constructor(user, config){
        this.pages = {
            'dashboard': new Dashboard(),
           // 'manage-car': new CarManager(config),
            'car-details': new CarDetails(user, config),
        };
    }

    goToRoute(route) {
        if (route !== window.location.hash) {
            window.location.hash = route;
        }
    }

    processRoute(route) {

        var pages = this.pages;

        // drop '#' from begining if exists
        route = route.replace(/^#/gi, '');

        // hide all menu links and all menu pages
        var linkEls = document.querySelectorAll('[data-route-link]');
        var pageEls = document.querySelectorAll('[data-route]');

        linkEls.forEach(function (item) {
            item.classList.remove('active');
        });
        pageEls.forEach(function (item) {
            item.classList.remove('active');
        });

        // show clicked page
        var targetLinkEls = document.querySelectorAll("[data-route-link='" + route + "']");
        var targetPageEls = document.querySelectorAll("[data-route='" + route + "']");

        targetLinkEls.forEach(function (item) {
            item.classList.add('active');
        });

        targetPageEls.forEach(function (item) {
            item.classList.add('active');
            var page = pages[route];
            if (!page) {
                console.error(`Can't find handler for page: ${route}`);
            } else {
                page.pageHandler(item);
            }
        });
    }
}

export default AppRouter;