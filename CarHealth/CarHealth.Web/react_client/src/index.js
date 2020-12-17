
import React, { Component } from 'react';
import { render } from 'react-dom';
import App from './components/app';

import CarPartsPage from './components/routes/carPartsPage/CarPartsPage.js';
import CarsControlPage from './components/routes/carsControlPage/CarsControlPage.js';

import AuthService from "./services/AuthService.js";
import ApiService from './services/ApiService.js';

import { createStore } from 'redux';
import { Provider } from 'react-redux';

import rootReducer from './store/reducers.js';



const store = createStore(rootReducer);


const routes =
  [
    {
      "route": "/carParts",
      "name": "Car parts",
      "component": CarPartsPage,
      "disabled": false,
    },
    {
      "route": "/carsControl",
      "name": "Cars control",
      "component": CarsControlPage,
      "disabled": false,
    }
  ];


const apiService = new ApiService();

apiService.getRequest("http://localhost:5003/config")
  .then(
    response => {

      console.log("----App.getRequest:", response);

      routes.push({
        "route": "/admin",
        "name": "Admin",
        "component": () => {
          window.location.href = `${response.urls.identity}/Users/Index`;
          return null;
        },
        "disabled": false,
      });

     
      render(
        <Provider store={store}>
          <App
            routes={routes}
            config={response}
          />
        </Provider>,
        document.getElementById('root'))

    })
  .catch(error => alert(`Index.js Rejected: ${error}`));






