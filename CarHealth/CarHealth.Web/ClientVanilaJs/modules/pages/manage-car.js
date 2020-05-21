
import DomUtil from '../DomUtil';
let domUtil = new DomUtil();

import globalScopes from './global-scopes.js';

import HttpUtil from '../HttpUtil.js';
let helper = new HttpUtil("");


class CarManager {

    constructor(config) {
        this.config = config;
    }

    handler(targetEl) {

        var config = this.config;

        var modalWindowEl = document.querySelector('.car-managmend-form-dialog');
        var carManagmendMenuEl = modalWindowEl.querySelector('.car-managmend-menu');
        var carManagmendMenuItemEl = modalWindowEl.querySelector('.car-managmend-menu-item-template');
        var carManagerMenuContainerEl = modalWindowEl.querySelector('.car-managmend-menu-container');

        var carManagmendFormContainerEl = modalWindowEl.querySelector('.car-managmend-form-container');

        var carManagmendFormPushNewCarButtonEl = modalWindowEl.querySelector('.js-car-managmend-push-new-car-button');
        var carManagmendMenuAddButtonEl = modalWindowEl.querySelector('.car-managmend-add-button');
        var carManagmendCloseFormAddButtonEl = modalWindowEl.querySelector('.js-car-managmend-close-add-form-button');
        var carManagmendFormPutButtonEl = modalWindowEl.querySelector('.js-car-managmend-put-button');
        var carManagmendFormDeleteButtonEl = modalWindowEl.querySelector('.js-car-managmend-delete-button');

        var carManagmendFormEl = document.forms.carManagmendForm;


        var pageData = {};


        GetUserCars(InitModalWindowMenu);

        modalWindowEl.showModal();

        domUtil.addBubleEventListener('.car-managmend-menu', '.car-managmend-menu-item-template', 'click', globalScopes.getEventListenerState().carManagerMenuItem, function (e, actualEl, desiredEl) {
            e.preventDefault();
            e.stopPropagation();

            carManagmendMenuEl.childNodes.forEach(function (item) {

                item.classList.remove('active');
            });

            desiredEl.classList.add('active');


            InitModalWindowForm(desiredEl.getAttribute('car-id'))

        });


        modalWindowEl.querySelector('.car-managmend-modal-window-close-button').onclick = function () {
            carManagmendFormContainerEl.classList.add('hidden');
            modalWindowEl.close();
        };

        function InitModalWindowForm(itemId) {

            carManagmendFormPushNewCarButtonEl.classList.add('hidden');
            carManagmendCloseFormAddButtonEl.classList.add('hidden');

            carManagmendFormContainerEl.classList.remove('hidden');

            // console.log('----sada',pageData);
            var car = pageData.find(x => x.id === itemId);
            carManagmendFormPutButtonEl.setAttribute('car-id', itemId);
            carManagmendFormDeleteButtonEl.setAttribute('car-id', itemId);

            console.log(car);

            carManagmendFormEl.elements.name.value = car.carEntityName;
            carManagmendFormEl.elements.totalRide.value = car.totalRide;
            carManagmendFormEl.elements.isCurrent.checked = car.isDefault;
        }

        function InitModalWindowMenu(data) {

            // remove all childs
            while (carManagmendMenuEl.firstChild) {
                carManagmendMenuEl.removeChild(carManagmendMenuEl.firstChild);
            }
            data.forEach(function (item) {
                AddMenuItem(item);
            });


        }

        function AddMenuItem(item) {

            var clone = carManagmendMenuItemEl.cloneNode(true);
            var carName = clone.querySelector('.car-managmend-menu-item-text');

            carName.innerText = item.carEntityName;

            clone.setAttribute('car-id', item.id);
            clone.classList.remove('hidden');

            if (item.isDefault === true) {
                clone.classList.add('active');
            }

            carManagmendMenuEl.appendChild(clone);
        }


        domUtil.addBubleEventListener('.car-managmend-add-button-container', '.car-managmend-add-button', 'click', globalScopes.getEventListenerState().carManagerAddbutton, function (e, actualEl, desiredEl) {
            e.preventDefault();
            e.stopPropagation();

            carManagerMenuContainerEl.classList.add('hidden');



            carManagmendFormContainerEl.classList.remove('hidden');
            carManagmendFormPushNewCarButtonEl.classList.remove('hidden');
            carManagmendCloseFormAddButtonEl.classList.remove('hidden');

            carManagmendFormPutButtonEl.classList.add('hidden');
            carManagmendFormDeleteButtonEl.classList.add('hidden');

            carManagmendFormPutButtonEl.classList.add('hidden');

            carManagmendCloseFormAddButtonEl.onclick = function () {


                carManagmendFormContainerEl.classList.add('hidden');
                carManagmendFormPushNewCarButtonEl.classList.add('hidden');
                carManagmendCloseFormAddButtonEl.classList.add('hidden');
                carManagerMenuContainerEl.classList.remove('hidden');

                carManagmendFormPutButtonEl.classList.remove('hidden');
                carManagmendFormDeleteButtonEl.classList.remove('hidden');
                carManagmendFormContainerEl.classList.add('hidden');
                modalWindowEl.close();
            };

            domUtil.addBubleEventListener('.js-car-managmend-push-new-car-button', '.js-car-managmend-push-new-car-button', 'click', globalScopes.getEventListenerState().carManagmendFormPushNewCarButtonEl, function (e, actualEl, desiredEl) {
                e.stopPropagation();

                var data = {};


                if (makeRequestBody(data)) {
                    var postUrl = `${config.urls.api}/add/car`;// used for add car


                    // console.log('-----', data);
                    SendData(postUrl, data);

                    alert("Car has been added");
                    GetUserCars(function (e) { });

                    carManagmendCloseFormAddButtonEl.onclick();

                } else {
                    // alert("Wrong input data");
                }


            });

        });

        domUtil.addBubleEventListener('.js-car-managmend-put-button', '.js-car-managmend-put-button ', 'click', globalScopes.getEventListenerState().carManagerPutbutton, function (e, actualEl, desiredEl) {

            var sendData = {};
            // var totalRideDataSend = {};

            sendData.CarEntityName = carManagmendFormEl.elements.name.value ? carManagmendFormEl.elements.name.value :
                carManagmendFormEl.elements.name.classList.add('input-field-empty-js');

            sendData.CarsTotalRide = /^\d+$/.test(carManagmendFormEl.elements.totalRide.value) ?
                Number(carManagmendFormEl.elements.totalRide.value) :
                carManagmendFormEl.elements.totalRide.classList.add('input-field-empty-js');


            carManagmendFormEl.elements.name.onfocus = function () {
                if (carManagmendFormEl.elements.name.classList.contains('input-field-empty-js')) {
                    carManagmendFormEl.elements.name.classList.remove('input-field-empty-js');
                }
            }

            carManagmendFormEl.elements.totalRide.onfocus = function () {
                if (carManagmendFormEl.elements.totalRide.classList.contains('input-field-empty-js')) {
                    carManagmendFormEl.elements.totalRide.classList.remove('input-field-empty-js');
                }
            }

            sendData.Id = carManagmendFormPutButtonEl.getAttribute('car-id');

            sendData.IsCurrent = carManagmendFormEl.elements.isCurrent.checked;

            if (sendData.CarEntityName &&
                sendData.CarsTotalRide) {

                //totalRideDataSend.Id = sendData.Id;
                 console.log('------',sendData);

                var putUrl = `${config.urls.api}/put/car`;// used for update car

                UpdateData(putUrl, sendData);

                alert("Car has been updated");

                GetUserCars(function (data) {
                    modalWindowEl.querySelector('.car-managmend-form-reset-button').onclick();
                    carManagmendFormContainerEl.classList.add('hidden');

                    InitModalWindowMenu(data);

                });
            }
        });

        domUtil.addBubleEventListener('.js-car-managmend-delete-button', '.js-car-managmend-delete-button', 'click', globalScopes.getEventListenerState().carManagerDeletebutton, function (e, actualEl, desiredEl) {

            var deleteUrl = `${config.urls.api}/delete/car?carEntityId=${carManagmendFormDeleteButtonEl.getAttribute('car-id')}`;// used for delete car
            DeleteData(deleteUrl)
            GetUserCars(function (data) {

                carManagmendFormContainerEl.classList.add('hidden');
                alert("Car has been deleted");

                InitModalWindowMenu(data)
            });
        });

        function DeleteData(deleteUrl) {
            helper.httpRequest(deleteUrl, null, "DELETE", function (response) {

            });
        }

        function UpdateData(putUrl, data) {
            helper.httpRequest(putUrl, data, "PUT", function (response) {

            });
        }

        function SendData(postUrl, data) {
            helper.httpRequest(postUrl, data, "POST", function (response) {

            });
        }

        function SendTotalRide(url, value) {
            helper.httpRequest(url, value, "POST", function (request) {
            });
        }

        function makeRequestBody(sendData) {

            sendData.CarEntityName = carManagmendFormEl.elements.name.value ? carManagmendFormEl.elements.name.value :
                carManagmendFormEl.elements.name.classList.add('input-field-empty-js');

            sendData.CarsTotalRide = /^\d+$/.test(carManagmendFormEl.elements.totalRide.value) ?
                carManagmendFormEl.elements.totalRide.value :
                carManagmendFormEl.elements.totalRide.classList.add('input-field-empty-js');

            carManagmendFormEl.elements.name.onfocus = function () {
                if (carManagmendFormEl.elements.name.classList.contains('input-field-empty-js')) {
                    carManagmendFormEl.elements.name.classList.remove('input-field-empty-js');
                }
            }

            carManagmendFormEl.elements.totalRide.onfocus = function () {
                if (carManagmendFormEl.elements.totalRide.classList.contains('input-field-empty-js')) {
                    carManagmendFormEl.elements.totalRide.classList.remove('input-field-empty-js');
                }
            }

            sendData.IsCurrent = carManagmendFormEl.elements.isCurrent.checked;

            if (sendData.CarEntityName &&
                sendData.CarsTotalRide) {
                return true;
            }

        }

        modalWindowEl.querySelector('.car-managmend-form-reset-button').onclick = function (e) {

            var carManagmendFormEl = document.forms.carManagmendForm;

            if (carManagmendFormEl.elements.name.classList.contains('input-field-empty-js')) {
                carManagmendFormEl.elements.name.classList.remove('input-field-empty-js');
            }

            if (carManagmendFormEl.elements.totalRide.classList.contains('input-field-empty-js')) {
                carManagmendFormEl.elements.totalRide.classList.remove('input-field-empty-js');
            }

        };// removes all input fields with red border when click on "reset"

        function GetUserCars(callback) {
            helper.httpGet(config.urls.api + '/allUsersCars', function (data) {
                pageData = data;
                console.log("Cars: ", data);
                callback(data);
            });
        }
    }

}

export default CarManager;
