
import DomUtil from '../DomUtil';
let domUtil = new DomUtil();

import globalScopes from './global-scopes.js';

import HttpUtil from '../HttpUtil.js';
let helper = new HttpUtil();


class ServiceLife {

    constructor(config) {
        this.config = config;
    }

    pageHandler(pageEl) {

        var config = this.config;


        
        var itemListContainerEl = pageEl.querySelector('.js-item-list-service-life-container');
        var itemListEl = pageEl.querySelector('.js-item-list-service-life');
        var listItemTemplateEl = itemListContainerEl.querySelector('.js-list-item-template-service-life');

        function getData(offset = 0, limit = 2, callBack = null) {
            helper.httpGet(config.urls.api + '/home/cardetails/1' + '/' + offset + '/' + limit, function (data) {
                console.log(2, data);
                if (callBack !== null) {
                    callBack(data);
                }
            });
        }

        function addDataItemsBlock(item, i) {
            var clone = listItemTemplateEl.cloneNode(true);
            var titleEl = clone.querySelector('.list-item-service-life-title-text');
            var recomendedReplaceEl = clone.querySelector('.list-item-recomended-replace-text');

            titleEl.innerText = item.Name;
            recomendedReplaceEl.innerText = item.RecomendedReplace;

            clone.classList.remove('list-item-service-life--hidden');
            itemListEl.appendChild(clone);
        }

        function showPageData(items) {
            // remove all childs
            while (itemListEl.firstChild) {
                itemListEl.removeChild(itemListEl.firstChild);
            }

            items.forEach(function (item, i) {
                addDataItemsBlock(item, i);
            });
            //addDataItemsBlock(items[0], 0);
        }

        var totalCount = 0;
        var page = 0;
        var limit = 20;
        var pageData = {};
        var offsetCarDetails = page * limit;
        var calcPagesCount = function () {
            return Math.ceil(totalCount / limit);
        }

        function getPageData(page = 0, limit = 15, callBack) {
            var pages = Math.ceil(totalCount / limit);
            page = page < 0 ? 0 : page;
            page = page > pages ? pages : page;
            offsetCarDetails = page * limit;

            getData(offsetCarDetails, limit, callBack);
        }

        function showPage(page, limit) {
            getPageData(page, limit, function (response) {
                totalCount = 20;
                // totalCount = response.CountCarsItems;//////////////////
                 pageData = response;
                pageData = response;
                console.log("pageData",pageData);
                showPageData(responses);
            });
        }

        function setNumberPage(page) {
            currentButtonEl.innerText = page + 1;
        }

        var firstButtonEl = itemListContainerEl.querySelector('.js-first-page-button');
        var prevButtonEl = itemListContainerEl.querySelector('.js-prev-page-button');
        var currentButtonEl = itemListContainerEl.querySelector('.js-current-page-button');
        var nextButtonEl = itemListContainerEl.querySelector('.js-next-page-button');
        var lastButtonEl = itemListContainerEl.querySelector('.js-last-page-button');


        domUtil.addBubleEventListener(firstButtonEl, ".button-icon", "click", globalScopes.getEventListenerState().itemListFirstButton, function (e) {
            page = 0;
            setNumberPage(page)
            showPage(page, limit);
        });

        domUtil.addBubleEventListener(prevButtonEl, ".button-icon", "click", globalScopes.getEventListenerState().itemListPrevButton, function (e) {
            page = page <= 0 ? 0 : page - 1;
            setNumberPage(page)
            showPage(page, limit);
        });

        domUtil.addBubleEventListener(nextButtonEl, ".button-icon", "click", globalScopes.getEventListenerState().itemListNextButton, function (e) {
            page = (page < calcPagesCount() - 1) ? page + 1 : (calcPagesCount() - 1);
            setNumberPage(page)
            showPage(page, limit);
        });

        domUtil.addBubleEventListener(lastButtonEl, ".button-icon", "click", globalScopes.getEventListenerState().itemListLastButton, function (e) {
            page = calcPagesCount() - 1;
            setNumberPage(page)
            showPage(page, limit);
        });

        showPage(page, limit);
        setNumberPage(0);
    }
}

export default ServiceLife;
