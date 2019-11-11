
import DomUtil from '../DomUtil';
let domUtil = new DomUtil();

import globalScopes from './global-scopes.js';

import HttpUtil from '../HttpUtil.js';
let helper = new HttpUtil();


class CarDetails {

    constructor(config) {
        this.config = config;
    }

    pageHandler(pageEl) {

        var config = this.config;


        var itemListContainerEl = pageEl.querySelector('.js-item-list-container');
        var itemListEl = pageEl.querySelector('.js-item-list');
        var listItemTemplateEl = itemListContainerEl.querySelector('.js-list-item-template');

        function getData(offset = 0, limit = 2, callBack = null) {
            helper.httpGet(config.urls.api + '/home/cardetails' + '/' + offset + '/' + limit, function (data) {
                console.log(2, data);
                if (callBack !== null) {
                    callBack(data);
                }
            });
        }

        function addDataItemsBlock(item, i) {
            var clone = listItemTemplateEl.cloneNode(true);
            var textContainer = clone.querySelector('.js-item-container');
            var titleEl = clone.querySelector('.list-item-title-text');
            var totalRideEl = clone.querySelector('.list-item-ride-total-text');
            var changeRideEl = clone.querySelector('.list-item-change-ride-text');
            var priceEl = clone.querySelector('.list-item-price-text');
            var dateEl = clone.querySelector('.list-item-date-text');
            var recomendedReplaceEl = clone.querySelector('.list-item-recomended-replace-text');
            var listItemRideContainer = clone.querySelector('.list-item-ride-container ')

            // begin edit picture elements (delete,put)
            var containerEditPictureEl = clone.querySelector('.container-edit-icon');
            var deletePictureEl = containerEditPictureEl.querySelector('.list-item-icon-delete');
            var putPictureEl = containerEditPictureEl.querySelector('.list-item-icon-put');
            // end edit picture elements (delete,put)

            item.RecomendedReplace <= item.TotalRide ?
            listItemRideContainer.classList.add('border-wrong'):
            listItemRideContainer.classList.remove('border-wrong');

            titleEl.innerText = item.Name;
            totalRideEl.innerText = item.TotalRide;
            changeRideEl.innerText = item.ChangeRide;
            priceEl.innerText = item.PriceOfDetail;
            dateEl.innerText = item.DateOfReplace.slice(0, 10)
            recomendedReplaceEl.innerText = item.RecomendedReplace;

            textContainer.setAttribute('position-in-list', i);
            //textEl.setAttribute('position-in-list', i);

            deletePictureEl.setAttribute('position-in-list', i);
            putPictureEl.setAttribute('position-in-list', i);

            clone.setAttribute('position-in-list', i);
            clone.classList.remove('list-item--hidden');
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
        var limit = 15;
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
                totalCount = response.CountCarsItems;
                pageData = response;
                console.log("pageData",pageData);
                showPageData(response.CarDetails);
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

        domUtil.addBubleEventListener(itemListContainerEl, '.js-item-text', 'click', globalScopes.getEventListenerState().itemListButtonFromListToCard, function (e, desiredEl) {
            e.stopPropagation();

            globalScopes.getWordOrder().length = (page * limit) + Number(desiredEl.getAttribute('position-in-list'));
            globalScopes.getWordOrder().isFromCarDetails = true;

            window.location.hash = '#cards';

        });

        domUtil.addBubleEventListener(itemListContainerEl, '.list-item-icon-delete', 'click', globalScopes.getEventListenerState().itemListDeleteButton, function (e, actualEl, desiredEl) {
            e.stopPropagation();

            var numberOfItem = Number(desiredEl.getAttribute('position-in-list'));

            if (confirm(`Do you want to delete ${pageData.CarDetails[numberOfItem].Name}`)) {

                var idItemUrl = `${config.urls.api}/home/${pageData.CarDetails[numberOfItem].Detail_id}`;
                deleteData(idItemUrl);

                alert('Deleted');
                showPage(page, limit);
            }
        });

        var idItemPutUrl = '';// used for put
        var formDataSend = {};
        var modalWindowForm = document.querySelector('.form-dialog');
        var formButtonEl = document.querySelectorAll('.form-control-button');

        document.querySelector('.modal-window-close-button').onclick = function () {
            modalWindowForm.close();
        };

        domUtil.addBubleEventListener(itemListContainerEl, '.list-item-icon-put', 'click', globalScopes.getEventListenerState().itemListPutButton, function (e, actualEl, desiredEl) {
            e.stopPropagation();

            var numberOfItem1 = Number(desiredEl.getAttribute('position-in-list'));
            var formPutButtonEl = document.querySelector('.js-put-button');

            formButtonEl.forEach(function (item) {
                item.classList.remove('active');
                item.classList.add('hidden');
            });

            formPutButtonEl.classList.replace('hidden', 'active');
            modalWindowForm.showModal();

            defaultFullfieldForm(pageData.CarDetails[numberOfItem1]);

            domUtil.addBubleEventListener(formPutButtonEl, '.js-put-button', 'click', globalScopes.getEventListenerState().formPutButton, function (e) {

                idItemPutUrl =  `${config.urls.api}/home/${pageData.CarDetails[numberOfItem1].Detail_id}`;
                var totalRideObj = {};

                if (makeRequestBody(formDataSend, totalRideObj)) {

                    if (/^\d+$/.test(totalRideObj.totalRide)) {
                        var url = config.urls.api + "/home/totalride";
                        SendTotalRide(url, totalRideObj);
                    }

                    updateData(idItemPutUrl, formDataSend);
                    modalWindowForm.close();
                    location.reload()
                }
            });

        });// event listener on PUT button


        domUtil.addBubleEventListener(itemListContainerEl, '.item-list-pager__add-button', 'click', globalScopes.getEventListenerState().itemListAddButton, function (e, actualEl, desiredEl) {
            e.stopPropagation();

            var formAddButtonEl = document.querySelector('.js-add-button');

            formButtonEl.forEach(function (item) {
                item.classList.remove('active');
                item.classList.add('hidden');
            });

            formAddButtonEl.classList.replace('hidden', 'active');
            modalWindowForm.showModal();

            domUtil.addBubleEventListener(formAddButtonEl, ".js-add-button", "click", globalScopes.getEventListenerState().formAddButton, function (e, desiredEl) {

                var totalRideObj = {};

                if (makeRequestBody(formDataSend,totalRideObj)) {
                    var postUrl = `${config.urls.api}/home/addcaritem`;// used for add item

                    if (/^\d+$/.test(totalRideObj.totalRide)) {
                        var url = config.urls.api + "/home/totalride";
                        SendTotalRide(url, totalRideObj);
                    }

                    sendData(postUrl, formDataSend);
                    modalWindowForm.close();
                    location.reload()
                } else {
                    // alert("Wrong input data");
                }

            });
        });// event listener on ADD button

        function SendTotalRide(url, TotalRide) {
            helper.httpRequest(url, TotalRide, 'POST', function (request) {
            });
        }

        var formResetButton = document.querySelector('.reset-button');

        formResetButton.onclick = function (e) {

            var CarItemFormEl = document.forms.NewCarItem;

            if (CarItemFormEl.elements.name.classList.contains('input-field-empty-js')) {
                CarItemFormEl.elements.name.classList.remove('input-field-empty-js');
            }

            if (CarItemFormEl.elements.price.classList.contains('input-field-empty-js')) {
                CarItemFormEl.elements.price.classList.remove('input-field-empty-js');
            }

            if (CarItemFormEl.elements.date.classList.contains('input-field-empty-js')) {
                CarItemFormEl.elements.date.classList.remove('input-field-empty-js');
            }

            if (CarItemFormEl.elements.totalRace.classList.contains('input-field-empty-js')) {
                CarItemFormEl.elements.totalRace.classList.remove('input-field-empty-js');
            }

            if (CarItemFormEl.elements.recomendation.classList.contains('input-field-empty-js')) {
                CarItemFormEl.elements.recomendation.classList.remove('input-field-empty-js');
            }


        };// removes all input fields with red border when click on "reset"

        function defaultFullfieldForm(data) {
            var CarItemFormEl = document.forms.NewCarItem;

            CarItemFormEl.elements.name.value = data.Name;

            CarItemFormEl.elements.price.value = data.PriceOfDetail;

            CarItemFormEl.elements.recomendation.value = data.RecomendedReplace;

            CarItemFormEl.elements.totalRace.value = pageData.CarsTotalRide;

            CarItemFormEl.elements.date.value = data.DateOfReplace;

        }

        function makeRequestBody(formDataSend, totalRideObj) {

            var CarItemFormEl = document.forms.NewCarItem;


            formDataSend.Name = CarItemFormEl.elements.name.value ? CarItemFormEl.elements.name.value :
                CarItemFormEl.elements.name.classList.add('input-field-empty-js');

            formDataSend.PriceOfDetail = (CarItemFormEl.elements.price.value &&
                (/^\d+$/.test(CarItemFormEl.elements.price.value))) ?
                CarItemFormEl.elements.price.value :
                CarItemFormEl.elements.price.classList.add('input-field-empty-js');

            formDataSend.RecomendedReplace = (CarItemFormEl.elements.recomendation.value &&
                (/^\d+$/.test(CarItemFormEl.elements.recomendation.value))) ?
                CarItemFormEl.elements.recomendation.value :
                CarItemFormEl.elements.recomendation.classList.add('input-field-empty-js');

            totalRideObj.totalRide = (CarItemFormEl.elements.totalRace.value &&
                (/^\d+$/.test(CarItemFormEl.elements.totalRace.value))) ?
                CarItemFormEl.elements.totalRace.value :
                CarItemFormEl.elements.totalRace.classList.add('input-field-empty-js');

            formDataSend.DateOfReplace = (CarItemFormEl.elements.date.value &&
                (/([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))/.test(CarItemFormEl.elements.date.value))) ?
                CarItemFormEl.elements.date.value :
                CarItemFormEl.elements.date.classList.add('input-field-empty-js');


            formDataSend.ChangeRide = Number(formDataSend.RecomendedReplace) + Number(totalRideObj.totalRide);

            CarItemFormEl.elements.name.onfocus = function () {
                if (CarItemFormEl.elements.name.classList.contains('input-field-empty-js')) {
                    CarItemFormEl.elements.name.classList.remove('input-field-empty-js');
                }
            }

            CarItemFormEl.elements.price.onfocus = function () {
                if (CarItemFormEl.elements.price.classList.contains('input-field-empty-js')) {
                    CarItemFormEl.elements.price.classList.remove('input-field-empty-js');
                }
            }

            CarItemFormEl.elements.date.onfocus = function () {
                if (CarItemFormEl.elements.date.classList.contains('input-field-empty-js')) {
                    CarItemFormEl.elements.date.classList.remove('input-field-empty-js');
                }
            }

            CarItemFormEl.elements.totalRace.onfocus = function () {
                if (CarItemFormEl.elements.totalRace.classList.contains('input-field-empty-js')) {
                    CarItemFormEl.elements.totalRace.classList.remove('input-field-empty-js');
                }
            }

            CarItemFormEl.elements.recomendation.onfocus = function () {
                if (CarItemFormEl.elements.recomendation.classList.contains('input-field-empty-js')) {
                    CarItemFormEl.elements.recomendation.classList.remove('input-field-empty-js');
                }
            }

            if (formDataSend.Name &&
                formDataSend.PriceOfDetail &&
                formDataSend.DateOfReplace &&
                totalRideObj.totalRide &&
                formDataSend.RecomendedReplace) {
                return true;
            }
        }// makes request body and checks empty field(s)


        function sendData(url, data) {
            helper.httpRequest(url, data, 'POST', function (request) {
                alert("Item was add");
            });
        }

        function updateData(url, data, authToken) {
            helper.httpRequest(url, data, 'PUT', authToken, function (request) {
                alert(`Item was update`);

            });
        }

        function deleteData(url, authToken) {

            console.log("Delete to", url);
            helper.deleteRequest(url, authToken);

        }

        showPage(page, limit);
        setNumberPage(0);
    }
}

export default CarDetails;