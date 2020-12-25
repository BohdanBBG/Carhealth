
import React from 'react';
import { render } from 'react-dom';
import App from './components/app';

import CarPartsPage from './components/routes/carPartsPage/CarPartsPage.js';
import CarsControlPage from './components/routes/carsControlPage/CarsControlPage.js';

import { createStore } from 'redux';
import { Provider } from 'react-redux';

import rootReducer from './store/reducers.js';

import { setAppConfig } from './store/auth/actions.js';

const superagent = require('superagent');

export const store = createStore(rootReducer);


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

superagent
  .get("http://localhost:5003/config")
  .set('Content-Type', 'application/json; charset=utf-8 ')
  .then(response => {

    let result = JSON.parse(response.text);

    console.log("----Index.js getRequest:", result);

    routes.push({
      "route": "/admin",
      "name": "Admin",
      "component": () => {
        window.location.href = `${result.urls.identity}/Users/Index`;
        return null;
      },
      "disabled": false,
    });

    store.dispatch(setAppConfig(result));


    render(
      <Provider store={store}>
        <App
          routes={routes}
        />
      </Provider>,
      document.getElementById('root'))
  })
  .catch(err => {
    console.error(`Index.js Rejected: ${err}`);
  });

